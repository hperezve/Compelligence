using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Common.Utility;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using Compelligence.DataTransfer.Comparinator;
using Compelligence.Domain.Entity;
using Compelligence.Web.Models.Util;
using Compelligence.Domain.Entity.Resource;
using Resources;
using Compelligence.Util.Common;
using System.Configuration;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Helpers;
using Common.Logging;
using Compelligence.Util.Validation;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
namespace Compelligence.Web.Controllers
{
    public class ComparinatorCriteriaUploaderController : GenericBackEndController
    {
        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }
        public IFileService FileService { get; set; }
        public IIndustryService IndustryService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IProductService ProductService { get; set; }
        public ICriteriaSetService CriteriaSetService { get; set; }
        public ICriteriaGroupService CriteriaGroupService { get; set; }
        public IIndustryCriteriaService IndustryCriteriaService { get; set; }
        public ICompetitorCriteriaService CompetitorCriteriaService { get; set; }
        public IProductCriteriaService ProductCriteriaService { get; set; }
        public ICriteriaService CriteriaService { get; set; }
        public IIndustryCriteriasService IndustryCriteriasService { get; set; }
        public IIndustryProductService IndustryProductService { get; set; }
        public IExportService ExportService { get; set; }
        public IResourceService ResourceService { get; set; }
        public IPriceService PriceService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }
        public IPricingTypeService PricingTypeService { get; set; }
        public string[,] myranking = new string[400,400];
        public string[,] myValue = new string[400,400];
        private static readonly ILog LOG = LogManager.GetLogger(typeof(ComparinatorCriteriaUploaderController));
        //
        // GET: /ComparinatorCriteriaUploader/

        public ActionResult Index()
        {
            //My change//
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            // end my change
            string clientCompany = (string)Session["ClientCompany"];
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["Message"] = "";
            Industry industr = new Industry();
            industr.Id = -1;
            industr.Name = "Generic";
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            industryList.Insert(0, industr);
            ViewData["CriteriaDowloap"] = new SelectList(industryList, "Id", "Name");
            return View();
        }

        public FileUploadJsonResult AjaxUpload(HttpPostedFileBase file)
        {
            // TODO: Add your business logic here and/or save the file
            // Return JSON
            string mess = string.Empty;
            if (file != null)
            {
                mess = UpdateOrSaveCriterias(file,"Feature");
                string valuetitulo = "<center><b>" + System.IO.Path.GetFileName(file.FileName) + "</b></br></center>";
                return new FileUploadJsonResult { Data = new { message = string.Format("{0} " + mess + ".", valuetitulo) } };
            }
            else
            {
                mess = "Select to File";
                return new FileUploadJsonResult { Data = new { message = mess } };
            }
        }
        public FileUploadJsonResult AjaxUploadPricing(HttpPostedFileBase file)
        {
            // TODO: Add your business logic here and/or save the file
            // Return JSON
            string mess = string.Empty;
            if (file != null)
            {
                mess = UpdateOrSaveCriterias(file,"Pricing");
                string valuetitulo = "<center><b>" + System.IO.Path.GetFileName(file.FileName) + "</b></br></center>";
                return new FileUploadJsonResult { Data = new { message = string.Format("{0} " + mess + ".", valuetitulo) } };
            }
            else
            {
                mess = "Select to File";
                return new FileUploadJsonResult { Data = new { message = mess } };
            }
        }

        private void SetDefaultDataForSave<T>(DomainObject<T> entity)
        {
            entity.CreatedBy = CurrentUser;
            entity.LastChangedBy = CurrentUser;
            entity.CreatedDate = DateTime.Today;
            entity.LastChangedDate = DateTime.Today;
            entity.ClientCompany = CurrentCompany;
        }

        private void SetDefaultDataForUpdate<T>(DomainObject<T> entity)
        {
            entity.LastChangedBy = CurrentUser;
            entity.LastChangedDate = DateTime.Today;
        }
        private string UpdateOrSaveCriterias(HttpPostedFileBase hpf, string typeFile)
        {
            string result = string.Empty;
            string fileNameResult = string.Empty;
            string tempo = string.Empty;
            Compelligence.Domain.Entity.File newFile = new Compelligence.Domain.Entity.File();

            newFile.FileName = System.IO.Path.GetFileName(hpf.FileName);
            newFile.FileFormat = newFile.FileName.Substring(newFile.FileName.LastIndexOf('.') + 1);
            SetDefaultDataForSave(newFile);
            FileService.Save(newFile);
            fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            fileNameResult += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');
            hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"], fileNameResult));
            tempo = newFile.FileName;

            Stream uploadFileStream;
            IList<string> valueList = new List<string>();
            string filestreamv = System.IO.Path.Combine(ContextFilePath + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"], fileNameResult);

            int leng = filestreamv.Length;
            Byte[] buffer = new Byte[filestreamv.Length];

            FileStream tyem = new FileStream(filestreamv.ToString(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            try
            {
                tyem.Read(buffer, 0, leng);

                uploadFileStream = hpf.InputStream;
                IWorkbook hssfworkbook = new XSSFWorkbook();
                try
                {
                    int j = 0;
                    NPOI.POIFS.FileSystem.POIFSFileSystem fs = new NPOI.POIFS.FileSystem.POIFSFileSystem();
                    if (newFile.FileFormat.Equals("XLS") || newFile.FileFormat.Equals("xls"))
                    {
                        fs = new NPOI.POIFS.FileSystem.POIFSFileSystem(tyem);
                        hssfworkbook = new HSSFWorkbook(fs, true);

                    }
                    else
                    {
                        hssfworkbook = new XSSFWorkbook(uploadFileStream);

                    }
                    for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
                    {


                        if (typeFile.Equals("Feature"))
                        {
                            if (!Validator.Equals(hssfworkbook.GetSheetName(i), "Legend"))
                            {
                                result += "</br><b>" + "Result of Sheet : " + hssfworkbook.GetSheetName(i) + "</b></br>";
                            }
                            if (!Validator.Equals(hssfworkbook.GetSheetName(i), "Legend"))
                            {
                                tyem.Close();
                                result += ReadAllExcelFile(tyem, fs, newFile, hssfworkbook.GetSheetName(i));
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                result += "</br><b>" + "Result of Sheet : " + hssfworkbook.GetSheetName(i) + "</b></br>";
                                result += ReadAllExcelFilePricing(tyem, fs, newFile, hssfworkbook.GetSheetName(i));
                                result += "Uploaded Successfully";
                                j++;
                            }
                        }


                    }
                }
                catch (Exception e)
                {
                    LOG.Error(" ComparinatorCriteriaUploader:UpdateOrSaveCriterias - XLS:" + e.Message, e);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (tyem != null)
                {
                    tyem.Dispose();
                }
            }
            //}
            return result;
        }
        private string checkMissingColumn(Compelligence.Domain.Entity.File newFile, string industryName, string grupName, string setName, string criteria, string type, string relevancy, string benefit, string IndustryStandard,string MostDesiredValuecolumn)
        {
            string result = string.Empty;
            if (Validator.IsBlankOrNull(industryName))
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderIndustryRequiredError, newFile.FileName));
            }
            if (Validator.IsBlankOrNull(grupName) || string.Compare(grupName.ToUpper(), "GROUP NAME") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Group Name"));
            }
            if (Validator.IsBlankOrNull(setName) || string.Compare(setName.ToUpper(), "SET NAME") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Set Name"));
            }
            if (Validator.IsBlankOrNull(criteria) || string.Compare(criteria.ToUpper(), "CRITERIA") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Criteria"));
            }
            if (Validator.IsBlankOrNull(type) || string.Compare(type.ToUpper(), "TYPE") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Type"));
            }
            //if (Validator.IsBlankOrNull(ranking) || string.Compare(ranking.ToUpper(), "RANKING") == -1)
            //{
            //    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Ranking"));
            //}
            if (Validator.IsBlankOrNull(relevancy) || string.Compare(relevancy.ToUpper(), "RELEVANCY") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Relevancy"));
            }
            if (Validator.IsBlankOrNull(benefit) || string.Compare(benefit.ToUpper(), "BENEFIT") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Benefit"));
            }
            if (Validator.IsBlankOrNull(IndustryStandard) || string.Compare(IndustryStandard.ToUpper(), "INDUSTRY STANDARD") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Industry Standard"));
            }
            if (Validator.IsBlankOrNull(MostDesiredValuecolumn) || string.Compare(MostDesiredValuecolumn.ToUpper(), "MOST DESIRED VALUE") == -1)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.MissingColumnGrupName, "Most Desired Value"));
            }
            return result;
        }
        private string validRankingandValue(string criteriaName, string value, string typeName, string relevancyName, string productName, Criteria criteria, string rankingName, ProductCriteria productCriteria,string notes , string links)
        {
            string result = string.Empty;
            IList<ResourceObject> typeList = ResourceService.GetAll<CriteriaType>();
            IList<ResourceObject> criteriaVisibleList = ResourceService.GetAll<CriteriaVisible>();
            bool passTypelist = false;
            bool passValueboolean = false;
            bool passValuenumeric = false;
            bool passRankingValue = false;
            bool passValueNumericLength = false;
            bool chechVal = false;
            foreach (ResourceObject types in typeList)
            {
                if (string.Compare(types.Value.ToUpper(), typeName.ToUpper()) == 0)
                {
                    passTypelist = true;
                    if (string.Compare(typeName.ToUpper(), "BOOLEAN") == 0)
                    {
                        //Validate boolean with expresion regular  n/a N/a n/A NA na Na nA yes Yes yEs YES no No NO
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(^[nN](/)?[aA]$)|(^[nN][oO]$)|(^[yY][eE][sS]$)");
                        if (regex.IsMatch(value))
                        {
                            passValueboolean = true;
                        }
                    }
                    else if (string.Compare(typeName.ToUpper(), "NUMERIC") == 0)
                    {
                        if (Validator.IsDecimal(value) || value.ToUpper().Equals("N/A"))
                        {
                            passValuenumeric = true;
                            if (value.IndexOf(".")!=-1 || value.IndexOf(",")!=-1)
                            {
                                string newValue = value.Replace(",", ".");
                                string[] subValue = newValue.Split('.');
                                if (subValue.Length == 2)
                                {
                                    if (subValue[1].Length > 8)
                                    {
                                        passValueNumericLength = true;//to show error message
                                    }
                                }
                                else
                                {
                                    passValuenumeric = false;
                                }
                            }
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                value = "null";
                chechVal=true;
            }
            if (!passValueboolean && passTypelist && string.Compare(typeName.ToUpper(), "BOOLEAN") == 0 && !value.Equals("null"))
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValueTypeBooleanProduct, criteriaName, productName, value));
            }
            if (!passValuenumeric && passTypelist && string.Compare(typeName.ToUpper(), "NUMERIC") == 0 && !value.Equals("null"))
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValueTypeNumericProduct, criteriaName, productName, value));
            }
            string RakingResult = convetRakingName(rankingName.ToUpper());

            if (!Validator.IsBlankOrNull(rankingName) && RakingResult.Equals("NO") && Validator.Equals(typeName.ToUpper(), "LIST"))
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.RankingValueError, criteriaName, rankingName));
            }
            if (!Validator.IsBlankOrNull(rankingName) && passTypelist)
            {
                if (!Validator.Equals(typeName.ToUpper(), "LIST"))
                {
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.RankingValueNotAllowList, rankingName, typeName, criteriaName, productName));
                }
            }
            if (chechVal)
            {
                value = string.Empty;
            }
            if (!passRankingValue && !Validator.IsBlankOrNull(rankingName) && Validator.IsBlankOrNull(value))
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidationRankinvalueNull, productName, rankingName));
            }
            if (!string.IsNullOrEmpty(notes))
            {
                if (notes.Length >= 1000)
                {
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderNotesMaxLength, criteriaName, "1000", productName));
                }
            }
            if (passValueNumericLength)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValueLengthTypeNumericProduct, criteriaName, value, productName));
            }
            string[] urlObjects = links.Split(',');
            if (!Validator.IsBlankOrNull(links))
            {
                for (int i = 0; i < urlObjects.Length; i++)
                {
                    if (!Validator.IsValidUrl(urlObjects[i]))
                    {
                        result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ComparinatorCriteriaLink, criteriaName, urlObjects[i], productName));
                    }
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                //if (!string.IsNullOrEmpty(notes))
                //{
                //    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidateValueCriteria, criteriaName, "Notes",notes, productName));
                //}
                if (!string.IsNullOrEmpty(links))
                {
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidateValueCriteria, criteriaName, "Links",links, productName));
                }
                if (!string.IsNullOrEmpty(rankingName))
                {
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidateValueCriteria, criteriaName, "Ranking",rankingName, productName));
                }
            }
           
            return result;
        }
        private string ValidationRankTypeReali(string criteriaName, string value, string typeName, string relevancyName, string productName, Criteria criteria, string rankingName, string MostDesiredValue)
        {
            string result = string.Empty;
            IList<ResourceObject> relevancyList = ResourceService.GetAll<CriteriaRelevancy>();
            IList<ResourceObject> typeList = ResourceService.GetAll<CriteriaType>();
            IList<ResourceObject> criteriaVisibleList = ResourceService.GetAll<CriteriaVisible>();
            IList<ResourceObject> criteriaMostDBooleanList = ResourceService.GetAll<CriteriaMostDesiredBoolean>();
            IList<ResourceObject> criteriaMostDNumericList = ResourceService.GetAll<CriteriaMostDesiredNumeric>();
            bool passTypelist = false;
            bool passTypelistifnull = false;
            bool passMostDesiredValue = false;
            foreach (ResourceObject types in typeList)
            {
                if (Validator.Equals(types.Value.ToUpper(), typeName.ToUpper()))
                {
                    passTypelist = true;
                }
            }
            if ((criteria == null || Validator.IsBlankOrNull(criteria.Type)))
            {
                if (Validator.IsBlankOrNull(typeName))
                {
                    passTypelistifnull = true;
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.CriteriaTypeIsnull, criteriaName));
                }
            }
            if (passTypelist && !Validator.IsBlankOrNull(MostDesiredValue))
            {
                if ( Validator.Equals(typeName.ToUpper(), "NUMERIC"))
                {
                    foreach (ResourceObject typesumericList in criteriaMostDNumericList)
                    {
                        if (Validator.Equals(typesumericList.Value.ToUpper(), MostDesiredValue.ToUpper()))
                        {
                            passMostDesiredValue = true;
                        }
                    }
                }
                else if (Validator.Equals(typeName.ToUpper(), "BOOLEAN"))
                {
                    foreach (ResourceObject typesumericList in criteriaMostDBooleanList)
                    {
                        if (Validator.Equals(typesumericList.Value.ToUpper(), MostDesiredValue.ToUpper()))
                        {
                            passMostDesiredValue = true;
                        }
                    }
                }
                else if (Validator.Equals(typeName.ToUpper(), "LIST"))
                {
                    passMostDesiredValue = true;
                }
            }
            if (passTypelist && !passMostDesiredValue && !Validator.IsBlankOrNull(MostDesiredValue))
            {
                if (Validator.Equals(typeName.ToUpper(), "BOOLEAN"))
                {
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NoExistMostDesiredValueNumeric, MostDesiredValue, criteriaName, "NO, Yes"));
                }
                else if (Validator.Equals(typeName.ToUpper(), "NUMERIC"))
                {
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NoExistMostDesiredValueNumeric, MostDesiredValue, criteriaName, "Highest, Lowest, NA"));
                }
            }
            if (!passTypelist && !passTypelistifnull)
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.CriteriaTypeNoExist, typeName, criteriaName));
            }

            if ((criteria != null && !Validator.IsBlankOrNull(criteria.Type)))
            {
                if (!Validator.Equals(ResourceService.GetName<CriteriaType>(criteria.Type).ToLower(), typeName.ToLower()))
                {
                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.CriteriaTypeIsnew, criteriaName, ResourceService.GetName<CriteriaType>(criteria.Type)));
                }
            }

            bool passRelevancy = false;
            foreach (ResourceObject relevancy in relevancyList)
            {
                if (Validator.Equals(relevancy.Value.ToUpper(), relevancyName.ToUpper()))
                {
                    passRelevancy = true;

                }
            }
            if (!passRelevancy && !Validator.IsBlankOrNull(relevancyName))
            {
                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.CriteriaRelevancyNoExist, criteriaName, relevancyName));
            }

            return result;
        }
        public string getRankingValue(string ranking)
        {
            string value = "";
            switch (ranking)
            {
                case "BC":
                    value = "Best in Class";
                    break;
                case "MA":
                    value = "Market Advantage";
                    break;
                case "MP":
                    value = "Market Parity";
                    break;
                case "MD":
                    value = "Market Disadvantage";
                    break;
                case "LM":
                    value = "Lagging Market";
                    break;

            }

            return value;
        }
        public string convetRakingName(string rankingName)
        {
            string value = "NO";
            switch (rankingName.ToUpper())
            {
                case "BEST IN CLASS":
                    value = "BC";
                    break;
                case "MARKET ADVANTAGE":
                    value = "MA";
                    break;
                case "MARKET PARITY":
                    value = "MP";
                    break;
                case "MARKET DISADVANTAGE":
                    value = "MD";
                    break;
                case "LAGGING MARKET":
                    value = "LM";
                    break;
                default:
                    value = "NO";
                    break;

            }
            return value;
        }
        private string ReadAllExcelFilePricing(FileStream tyem,NPOI.POIFS.FileSystem.POIFSFileSystem fs,Compelligence.Domain.Entity.File newFile, string valueList)
        {
            string result = string.Empty;
            IWorkbook hssfworkbook = new XSSFWorkbook();


            if (fs.Root.EntryCount == 0)
            {

                hssfworkbook = new XSSFWorkbook(tyem.Name);
            }
            else
            {
                hssfworkbook = new HSSFWorkbook(fs, true);
            }
            int pos = 0;
            ISheet sheet = hssfworkbook.GetSheet(valueList);
            int beginRow = 1;
            int numberRow = sheet.PhysicalNumberOfRows;
            int endRow = numberRow;
            try
            {
                HSSFCell CellTempo = (HSSFCell)sheet.GetRow(beginRow).GetCell(0);
                if (CellTempo == null)
                {
                    beginRow++;
                    CellTempo = (HSSFCell)sheet.GetRow(beginRow).GetCell(0);
                }
                string c0 = string.Empty;
                string c1 = string.Empty;
                string c2 = string.Empty;
                while (string.IsNullOrEmpty(c0) && string.IsNullOrEmpty(c1) && string.IsNullOrEmpty(c2))
                {
                    c0 = GetValueOfCell(sheet, beginRow, 0);
                    c1 = GetValueOfCell(sheet, beginRow, 1);
                    c2 = GetValueOfCell(sheet, beginRow, 2);
                    if (string.IsNullOrEmpty(c0) && string.IsNullOrEmpty(c1) && string.IsNullOrEmpty(c2))
                    {
                        beginRow++;
                        endRow++;
                    }
                }
            }
            catch
            {
                beginRow++;
                endRow++;
            }
            int t =beginRow;
            int k =endRow;
            int totalColumns = sheet.GetRow(beginRow).LastCellNum;
            IList<Price> priceList = new List<Price>();
            for (int r = beginRow; r < endRow; r++)
            {
                Price price = new Price();
                string ProdouctName = GetValueOfCell(sheet, r, 0).ToLower();
                string PriceName = GetValueOfCell(sheet, r, 1);                
                string PartNumber = GetValueOfCell(sheet, r, 2).ToLower();
                price.PartNumber = PartNumber;
                string Type = GetValueOfCell(sheet, r, 3);
                string Units = GetValueOfCell(sheet, r, 4).ToLower();
                string Value = GetValueOfCell(sheet, r, 5).ToLower();
                string Status = GetValueOfCell(sheet, r, 6).ToLower();
                string Requiered = GetValueOfCell(sheet, r, 7).ToLower();
                string Url = GetValueOfCell(sheet, r, 8).ToLower();
                if (!string.IsNullOrEmpty(ProdouctName) || !string.IsNullOrEmpty(PriceName) || !string.IsNullOrEmpty(PartNumber) || !string.IsNullOrEmpty(Type) || !string.IsNullOrEmpty(Units) || !string.IsNullOrEmpty(Value) || !string.IsNullOrEmpty(Status) || !string.IsNullOrEmpty(Requiered) || !string.IsNullOrEmpty(Url))
                {
                    string resultemp = string.Empty;
                    resultemp = ValidatePricing(ProdouctName, PriceName, PartNumber, Type, Units, Value, Status, Requiered, Url);
                    result += resultemp;
                    if (resultemp == string.Empty)
                    {
                        Product product = ProductService.GetFirstResultByName(ProdouctName, CurrentCompany);
                        if (product != null)
                        {
                            price.EntityId = product.Id;
                        }
                        price.Name = PriceName;
                        //string ValidType = ResourceService.GetValue<PriceType>(StringUtility.UppercaseFirst(Type.ToLower()));
                        //price.Type = ValidType;
                        if (string.IsNullOrEmpty(Type))
                        { price.PricingType = "Unknown"; }
                        else
                        {
                            if (product != null)
                            {
                                //search some pricingtype to product assigened to industry with the same label
                                PricingType pricingType = PricingTypeService.GetByEntityIdAndLabel(product.Id, DomainObjectType.Product, Type.Trim().ToUpper(), CurrentCompany);
                                if (pricingType == null) price.PricingType = Type;
                                else
                                {
                                    price.PricingType = pricingType.Label;
                                    price.PricingTypeId = pricingType.Id;
                                }

                            }
                            else price.PricingType = Type;
                        }

                        string ValidUnits = ResourceService.GetValue<PriceUnits>(StringUtility.UppercaseFirst(Units.ToLower()));
                        price.Units = ValidUnits;
                        if (string.IsNullOrEmpty(Value))
                        {
                            price.Value = null;
                        }
                        else
                        {
                            price.Value = Convert.ToDecimal(Value);
                        }
                        string StatusValid = ResourceService.GetValue<PriceStatus>(StringUtility.UppercaseFirst(Status.ToLower()));
                        price.Status = StatusValid;
                        string requieredValidation = ResourceService.GetValue<PriceRequired>(StringUtility.UppercaseFirst(Requiered.ToLower()));
                        price.Required = requieredValidation;
                        price.Url = Url;
                        priceList.Add(price);
                    }
                }
            }
            SaveOrUpdateNewPrice(priceList);
            return result;

        }

        public string ValidatePricing(string ProdouctName, string PriceName, string PartNumber, string Type, string Units, string Value, string Status, string Requiered, string Url)
        {
            string result = string.Empty;
            Product product = null;
            if (!string.IsNullOrEmpty(ProdouctName))
            {
                product = ProductService.GetFirstResultByName(ProdouctName, CurrentCompany);
                if(product == null)
                    result += StringUtility.InsertHtmlLabels(string.Format("Product " + ProdouctName + " Must Exist in System"));
            }            
            else
            {
                result += StringUtility.InsertHtmlLabels(string.Format("Product " + ProdouctName + " Must Exist in System"));
            }
            if (string.IsNullOrEmpty(PriceName))
            {
                result += StringUtility.InsertHtmlLabels(string.Format("Name of Pricing is required"));
            }
            if (!string.IsNullOrEmpty(Type))
            {
                if(Type.Length>60)// max 60characters
                    result += StringUtility.InsertHtmlLabels(string.Format("For the Product " + ProdouctName + " the Type should have a maximum of 60 characters"));
            }
            if (!string.IsNullOrEmpty(Units))
            {
                string ValidUnits = ResourceService.GetValue<PriceUnits>(StringUtility.UppercaseFirst(Units.ToLower()));
                if (string.IsNullOrEmpty(ValidUnits))
                {
                    result += StringUtility.InsertHtmlLabels(string.Format("For the Product " + ProdouctName + " the Units " + Units + " and Only acceptable Data are :Other ,Dollar"));
                }
            }
            if (!Validator.IsDecimal(Value))
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    result += StringUtility.InsertHtmlLabels("For the Product " + ProdouctName + " Error the Value " + Value + " is not number");
                }
            }
           
            if (!string.IsNullOrEmpty(Status))
            {
                string StatusValid = ResourceService.GetValue<PriceStatus>(StringUtility.UppercaseFirst(Status.ToLower()));
                if (string.IsNullOrEmpty(StatusValid))
                {
                    result += StringUtility.InsertHtmlLabels("For the Product " + ProdouctName + " the Status " + Status + " and Only acepptable Data are : Enable / Disabled");
                }
            }
            if (!string.IsNullOrEmpty(Requiered))
            {
                string requieredValidation = ResourceService.GetValue<PriceRequired>(StringUtility.UppercaseFirst(Requiered.ToLower()));
                if (string.IsNullOrEmpty(requieredValidation))
                {
                    result += StringUtility.InsertHtmlLabels("For the Product " + ProdouctName + " the Required " + Requiered + " and Only acepptable Data are : Required / Optional");
                } 
            }
            if (!string.IsNullOrEmpty(Url))
            {
                if (!Validator.IsValidUrl(Url))
                {
                    result += result += StringUtility.InsertHtmlLabels("For the Product "+ProdouctName+" Invalid Url " + Url);
                }
            }
            return result;
        }
        private string ReadAllExcelFile(FileStream tyem, NPOI.POIFS.FileSystem.POIFSFileSystem fs, Compelligence.Domain.Entity.File newFile, string valueList)
        {
            string result = string.Empty;
            IWorkbook hssfworkbook = new XSSFWorkbook();


            if (fs.Root.EntryCount == 0)
            {
               
                hssfworkbook = new XSSFWorkbook(tyem.Name);
            }
            else {
                hssfworkbook = new HSSFWorkbook(fs, true);
            }
            int pos = 0;
            ISheet sheet = hssfworkbook.GetSheet(valueList);
             
            int numberRow = sheet.PhysicalNumberOfRows; //TOTAL OF ROWS IN SHEET
            string industryName = GetValueOfCell(sheet, 0, 1);
            string grupName = GetValueOfCell(sheet, 3, 0);
            string setName1 = GetValueOfCell(sheet, 3, 1);
            string criteriaN = GetValueOfCell(sheet, 3, 2);
            string type = GetValueOfCell(sheet, 3, 3);
            string MostDesiredValuecolumn = GetValueOfCell(sheet, 3, 4);
            string relevancy = GetValueOfCell(sheet, 3, 5);
            string benefit = GetValueOfCell(sheet, 3, 6);
            string Cost = GetValueOfCell(sheet, 3, 7);
            string IndustryStandard = GetValueOfCell(sheet, 3, 8);
            IList<ResourceObject> Rankinglist = ResourceService.GetAll<ProductRanking>();
            IList<string> convertRakingstring = convertRankingstring(Rankinglist);
            result = checkMissingColumn(newFile, industryName, grupName, setName1, criteriaN, type, relevancy, benefit, IndustryStandard, MostDesiredValuecolumn);

            if (result == string.Empty)
            {
                Industry industry = IndustryService.GetFirstResultByName(industryName, CurrentCompany);
                if (industry != null)
                {
                    int beginRow = 1;
                    int endRow = numberRow-1; //THE INDEX BEGIN IN 0 THEN REST 1 
                    try
                    {
                        HSSFCell CellTempo = (HSSFCell)sheet.GetRow(beginRow).GetCell(0);
                        if (CellTempo == null)
                        {
                            beginRow++;
                            CellTempo = (HSSFCell)sheet.GetRow(beginRow).GetCell(0);
                        }
                        string c0 = string.Empty;
                        string c1 = string.Empty;
                        string c2 = string.Empty;
                        while (string.IsNullOrEmpty(c0) && string.IsNullOrEmpty(c1) && string.IsNullOrEmpty(c2))
                        {
                            c0 = GetValueOfCell(sheet, beginRow, 0);
                            c1 = GetValueOfCell(sheet, beginRow, 1);
                            c2 = GetValueOfCell(sheet, beginRow, 2);
                            if (string.IsNullOrEmpty(c0) && string.IsNullOrEmpty(c1) && string.IsNullOrEmpty(c2))
                            {
                                beginRow++;
                            }
                        }
                    }
                    catch
                    {
                        beginRow++;
                    }
                    int totalColumns = sheet.GetRow(beginRow).LastCellNum;
                    IList<string> nameProductList = new List<string>();
                    IList<ComparinatorCell> cellList = new List<ComparinatorCell>();

                    bool isListGroupEmpty = true;
                    bool isListProductEmpty = true;
                    string tempo = string.Empty;
                    for (int d = 0; d < totalColumns; d++)
                    {
                        tempo = GetValueOfCell(sheet, beginRow, d);
                        tempo = string.Empty;
                    }
                    int totalcol = 9;

                    int flagGrup = 0;
                    int flagset = 0;
                    int flagcri = 0;
                    //int checkcolumrankingvalue = 0;
                    int flag = 0;
                    int controw=0;
                    int indexrow = 0;
                    # region Checked Value Ranking 
                    string resulranking = string.Empty;
                    for (int r = beginRow + 1; r < endRow + 1; r++)
                    {
                        Criteria criteria = new Criteria();
                        CriteriaGroup criteriagroup = new CriteriaGroup();
                        CriteriaSet Criteriaset = new CriteriaSet();
                        string typeecell = string.Empty;
                        string groupNamecell = string.Empty;
                        string setNamecell = string.Empty;
                        string criteriaNamecell = string.Empty;
                        string typecelldb=string.Empty;
                        IList<string> cellranking = new List<string>();
                        IList<string> cellvalue = new List<string>();
                        IList<string> cellproduct = new List<string>();
                        IList<string> cellvaluedb = new List<string>();
                        IList<string> cellrankingdb = new List<string>();
                        
                        typeecell = GetValueOfCell(sheet, r, 3).ToLower();
                        groupNamecell = GetValueOfCell(sheet, r, 0);
                        setNamecell = GetValueOfCell(sheet, r, 1);
                        criteriaNamecell = GetValueOfCell(sheet, r, 2);
                        criteriagroup = CriteriaGroupService.GetFirstResultByName(groupNamecell, industry.Id, CurrentCompany);
                        if (criteriagroup == null)
                        {
                           
                        }
                        else
                        {
                            Criteriaset = CriteriaSetService.GetFirstResultByName(setNamecell, industry.Id, criteriagroup.Id, CurrentCompany);
                        }
                        if (Criteriaset == null && criteriagroup != null)
                        {

                            criteria = CriteriaService.GetFirstResultByNameAndGroup(criteriaNamecell, industry.Id, criteriagroup.Id, CurrentCompany);
                        }
                        else if (criteriagroup != null && Criteriaset != null)
                        {
                            criteria = CriteriaService.GetFirstResultByName(criteriaNamecell, industry.Id, Criteriaset.Id, criteriagroup.Id, CurrentCompany);
                        }
                            for (int c = totalcol; c < totalColumns; c = c + 4)
                            {
                                string cellproductName = string.Empty;
                                cellproductName = GetValueOfCell(sheet, beginRow - 1, c);
                                Product productCell = null;

                                if (!string.IsNullOrEmpty(cellproductName))
                                {
                                    productCell = ProductService.GetFirstResultByName(cellproductName, CurrentCompany);
                                }
                                ProductCriteria productvalue = new ProductCriteria();

                                if (criteria != null && productCell!=null)
                                {
                                    typecelldb=criteria.Type;
                                    productvalue = ProductCriteriaService.GetByIndustry(industry.Id, productCell.Id, criteria.Id);
                                }
                                string rakingNamecell = string.Empty;
                                string valuecell = string.Empty;
                                
                                valuecell = GetValueOfCell(sheet, r, c);
                                rakingNamecell = GetValueOfCell(sheet, r, c + 1);
                                
                                cellranking.Add(rakingNamecell);
                                cellvalue.Add(valuecell);
                                cellproduct.Add(cellproductName);
                                if (productvalue != null)
                                {
                                    cellvaluedb.Add(productvalue.Value);
                                    cellrankingdb.Add(productvalue.Feature);
                                }
                                else
                                {
                                    cellvaluedb.Add("");
                                    cellrankingdb.Add("");
                                }
                            }
                            string DataRanking = string.Empty;
                            if (cellranking.Count > 0)
                            {
                                resulranking += CheckRankingValue(cellranking, cellvalue, cellproduct, cellvaluedb, cellrankingdb, typeecell, controw, convertRakingstring, typecelldb, criteriaNamecell);
                              
                            }

                        controw++;
                    }
                    #endregion


                    int col=0;
                    
                    for (int c = totalcol; c < totalColumns; c = c + 4)
                    {
                        int productflag = 0;
                        string value = string.Empty;
                        string rakingName = string.Empty;
                        string productName = string.Empty;
                        productName = GetValueOfCell(sheet, beginRow - 1, c);
                        Product product = null;

                        if (!string.IsNullOrEmpty(productName))
                        {
                            isListProductEmpty = false;
                            product = ProductService.GetFirstResultByName(productName, CurrentCompany);
                        }
                        int rows = 0;
                        for (int r = beginRow + 1; r < endRow + 1; r++)
                        {

                            string groupName = string.Empty;
                            string setName = string.Empty;
                            string criteriaName = string.Empty;
                            string criteriaBenefit = string.Empty;
                            string criteriaCost = string.Empty;
                            string criteriaIndustryStandard = string.Empty;
                            string typeName = string.Empty;
                            string MostDesiredValue = string.Empty;
                            string relevancyName = string.Empty;
                            string Notes = string.Empty;
                            string Links = string.Empty;

                            groupName = GetValueOfCell(sheet, r, 0);
                            if (!string.IsNullOrEmpty(groupName))
                            {
                                isListGroupEmpty = false;
                            }
                            setName = GetValueOfCell(sheet, r, 1);
                            criteriaName = GetValueOfCell(sheet, r, 2);
                            value = myValue[col, rows]; 
                            typeName = GetValueOfCell(sheet, r, 3).ToLower();
                            MostDesiredValue = GetValueOfCell(sheet, r, 4).ToLower();
                            rakingName = myranking[col,rows];
                            Notes = GetValueOfCell(sheet, r, c + 2);
                            Links=GetValueOfCell(sheet, r,  c + 3);
                            relevancyName = GetValueOfCell(sheet, r, 5).ToLower();
                            criteriaBenefit = GetValueOfCell(sheet, r, 6);
                            criteriaCost = GetValueOfCell(sheet, r, 7);
                            criteriaIndustryStandard = GetValueOfCell(sheet, r, 8);
                            
                                if (!string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(criteriaName) && (product != null))
                                {
                                    //validation to length (DB) 
                                    if (typeName.ToUpper().Equals("LIST"))
                                    {
                                        if (value.Length > 225)
                                        {
                                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderListValueMaxLength, value, "225", "225"));
                                            value = value.Substring(0, 225);
                                        }
                                    }
                                    if (indexrow < r)
                                    {
                                        if (groupName.Length > 100)
                                        {
                                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderGroupMaxLength, groupName));
                                            groupName = groupName.Substring(0, 100);
                                        }
                                        if (setName.Length > 100)
                                        {
                                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderSetMaxLength, setName));
                                            setName = setName.Substring(0, 100);
                                        }
                                        if (criteriaName.Length > 100)
                                        {
                                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderCriteriaMaxLength, criteriaName));
                                            criteriaName = criteriaName.Substring(0, 100);
                                        }
                                        if (criteriaBenefit.Length > 200)// if the benefit value is greater than 200
                                        {
                                            criteriaBenefit = criteriaBenefit.Substring(0, 200);//this value should be resize
                                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderBenefitMaxLength, criteriaBenefit));
                                        }
                                        if (criteriaCost.Length > 200)
                                        {
                                            criteriaCost = criteriaCost.Substring(0, 200);
                                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderCostMaxLength, criteriaCost));
                                        }
                                        indexrow = r;
                                    }
                                    else
                                    {
                                        if (groupName.Length > 100)
                                        {                                            
                                            groupName = groupName.Substring(0, 100);
                                        }
                                        if (setName.Length > 100)
                                        {
                                            setName = setName.Substring(0, 100);
                                        }
                                        if (criteriaName.Length > 100)
                                        {                                            
                                            criteriaName = criteriaName.Substring(0, 100);
                                        }
                                        if (criteriaBenefit.Length > 200)
                                        {
                                            criteriaBenefit = criteriaBenefit.Substring(0, 200);
                                        }
                                        if (criteriaCost.Length > 200)
                                        {
                                            criteriaCost = criteriaCost.Substring(0, 200);
                                        }
                                    }

                                if (value.Length <= 225 && criteriaBenefit.Length <= 200 && criteriaIndustryStandard.Length <= 1000)
                                {  
                                    Criteria criteria = new Criteria();
                                    CriteriaGroup criteriagroup = new CriteriaGroup();
                                    CriteriaSet Criteriaset = new CriteriaSet();
                                    criteriagroup = CriteriaGroupService.GetFirstResultByName(groupName, industry.Id, CurrentCompany);
                                    if (criteriagroup == null)
                                    {
                                        //criteria = CriteriaService.GetFirstCriteriaByName(criteriaName, CurrentCompany);
                                    }
                                    else
                                    {
                                        Criteriaset = CriteriaSetService.GetFirstResultByName(setName, industry.Id, criteriagroup.Id, CurrentCompany);
                                    }
                                    if (Criteriaset == null && criteriagroup != null)
                                    {

                                        criteria = CriteriaService.GetFirstResultByNameAndGroup(criteriaName, industry.Id, criteriagroup.Id, CurrentCompany);
                                    }
                                    else if (criteriagroup != null && Criteriaset != null)
                                    {
                                        criteria = CriteriaService.GetFirstResultByName(criteriaName, industry.Id, Criteriaset.Id, criteriagroup.Id, CurrentCompany);
                                    }
                                    if (flag == 0)
                                    {

                                        result += ValidationRankTypeReali(criteriaName, value, typeName, relevancyName, productName, criteria, rakingName, MostDesiredValue);
                                    }
                                    ProductCriteria productvalue = new ProductCriteria();
                                    if (criteria != null)
                                    {
                                        productvalue = ProductCriteriaService.GetByIndustry(industry.Id, product.Id, criteria.Id);
                                    }
                                    else
                                    {

                                    }
                                    result += validRankingandValue(criteriaName, value, typeName, relevancyName, productName, criteria, rakingName, productvalue, Notes, Links);
                                

                                        ComparinatorCell comparinatorCell = new ComparinatorCell();
                                        comparinatorCell.ValueCell = value;
                                        comparinatorCell.IndustryId = industry.Id;
                                        comparinatorCell.ProductId = product.Id;
                                        comparinatorCell.Group = groupName;
                                        comparinatorCell.Set = setName;
                                        comparinatorCell.Cost = criteriaCost;
                                        if (!result.Contains("Note has more than"))
                                        {
                                            comparinatorCell.Note = Notes;
                                        }
                                        if (!result.Contains("has an invalid link"))
                                        {
                                            string[] urlObjects = Links.Split(',');
                                            string links=string.Empty;
                                            for (int l = 0; l < urlObjects.Length; l++)
                                            {
                                               links = urlObjects[l]+" "+links;
                                            }
                                            comparinatorCell.Link = links;
                                        }
                                        comparinatorCell.Criteria = criteriaName;
                                        comparinatorCell.Industrystandard = criteriaIndustryStandard;
                                        comparinatorCell.Benefit = criteriaBenefit;
                                        if (Validator.IsBlankOrNull(relevancyName))
                                        {
                                            comparinatorCell.Relevancy = CriteriaRelevancy.Medium;
                                        }
                                        else
                                        {
                                            comparinatorCell.Relevancy = ResourceService.GetValue<CriteriaRelevancy>(StringUtility.UppercaseFirst(relevancyName));
                                        }

                                        comparinatorCell.Type = ResourceService.GetValue<CriteriaType>(StringUtility.UppercaseFirst(typeName));
                                        if (StringUtility.UppercaseFirst(typeName).Equals("List"))
                                        {
                                            if (!Validator.IsBlankOrNull(rakingName))
                                            {
                                                comparinatorCell.Ranking = convetRakingName(rakingName.ToUpper());
                                            }
                                        }
                                        else
                                        {
                                            if (StringUtility.UppercaseFirst(typeName).Equals("Numeric"))
                                            {
                                                if (MostDesiredValue.ToUpper().Equals("N/A"))
                                                {
                                                    comparinatorCell.MostDesiredValue = "NA";
                                                }
                                                else
                                                {
                                                    comparinatorCell.MostDesiredValue = ResourceService.GetValue<CriteriaMostDesiredNumeric>(StringUtility.UppercaseFirst(MostDesiredValue));
                                                }
                                               
                                            }
                                            else if (StringUtility.UppercaseFirst(typeName).Equals("Boolean"))
                                            {
                                                comparinatorCell.MostDesiredValue = ResourceService.GetValue<CriteriaMostDesiredBoolean>(StringUtility.UppercaseFirst(MostDesiredValue));
                                            }
                                        }
                                        cellList.Add(comparinatorCell);
                                    
                                }
                                else
                                {                                    
                                    if (value.Length > 225)
                                    {
                                        result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderValueMaxLength, value, "225"));
                                    }
                                    if (criteriaBenefit.Length > 200)
                                    {
                                        result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploadertMaxLength, "Benefit"));
                                    }
                                    if (criteriaIndustryStandard.Length > 1000)
                                    {
                                        result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploadertMaxLength, "Industry Standard"));
                                    }
                                }
                            }
                            else
                            {
                                if (product != null)
                                {
                                    if ((!string.IsNullOrEmpty(groupName) || !string.IsNullOrEmpty(setName) || !string.IsNullOrEmpty(criteriaName)) && !string.IsNullOrEmpty(value))
                                    {
                                        if (string.IsNullOrEmpty(groupName) && flagGrup == 0)
                                        {
                                            result += StringUtility.InsertHtmlLabels("missing the value of Group name in row :" + (r + 1));
                                            flagGrup++;
                                        }
                                        if (string.IsNullOrEmpty(setName) && flagset == 0)
                                        {
                                            result += StringUtility.InsertHtmlLabels("missing the value of Set Name in row :" + (r + 1));
                                            flagset++;
                                        }
                                        if (string.IsNullOrEmpty(criteriaName) && flagcri == 0)
                                        {
                                            result += StringUtility.InsertHtmlLabels("missing the value of Criteria in row :" + (r + 1));
                                            flagcri++;
                                        }
                                    }

                                }
                                else
                                {
                                    if ((!string.IsNullOrEmpty(groupName) || !string.IsNullOrEmpty(setName) || !string.IsNullOrEmpty(criteriaName)) && !string.IsNullOrEmpty(value) && productflag == 0)
                                    {
                                        result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderProductExistError, productName));
                                    }
                                    productflag++;
                                }
                            }

                                rows++;
                          
                        }
                        flag = 1;
                          col++;
                    }
                    result += resulranking;
                    if (cellList.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(result))
                            result = StringUtility.InsertHtmlLabels("Uploaded Successfully, but :") + result;
                        else result = StringUtility.InsertHtmlLabels("Uploaded Successfully");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(result))
                        {
                            result = StringUtility.InsertHtmlLabels("does not contain proper data");
                        }
                        if (isListProductEmpty)
                        {
                            if (!string.IsNullOrEmpty(result))
                            {
                                result += "</br>";
                            }
                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ComparinatorCriteriaUploaderProductExistError, newFile.FileName));
                        }
                        if (isListGroupEmpty)
                        {
                            if (!string.IsNullOrEmpty(result))
                            {
                                result += "</br>";
                            }
                            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ComparinatorCriteriaUploaderGroupExistError, newFile.FileName));
                        }
                    }
                    int s = cellList.Count / endRow;
                    SaveOrUpdateNewValues(cellList);
                }
                else
                {
                    result = StringUtility.InsertHtmlLabels(string.Format(LabelResource.NewComparinatorCriteriaUploaderIndustryExistError, industryName));
                }
            }
            return result;
        }
        public IList<string> convertRankingstring(IList<ResourceObject> Rankinglist)
        {
            IList<string> rankgstring = new List<string>();
            foreach (ResourceObject Ranking in Rankinglist)
            {
                rankgstring.Add(Ranking.Value);
            }
            return rankgstring;
        }
        //public string CheckRankingValue(IList<string> cellList, IList<string> cellListvalue, IList<string> cellproduct, IList<string> cellvaluedb, IList<string> cellrankingdb, string typeecell, int contcol, IList<string> convertrankingstring, string typecelldb,string criteriaNamecell)
        //{
        //    string result = string.Empty;

        //    IList<string> cellselect = new List<string>();
        //    int d = 0;
        //    int controw=0;
        //    int flagMarket = 0;
        //    string markeauxma = "ANY";
        //    string markeauxbi = "ANY";
        //    string markeauxmp = "ANY";
        //    string markeauxmd = "ANY";
        //    string markeauxlm = "ANY";
        //    string bestaxu = "";
        //    string bestvalue = "";
        //    int bestauxs = 0;
        //    string bestauxscont ="";
        //    int contbestvalue = 0;
        //    int bestvaluenull=0;
        //    string validadMarketParty="";
             
        //    foreach (string cel in cellList)
        //    {
        //        if (bestauxs == 0)
        //        {
        //            if (cel.ToUpper().Equals("MARKET PARITY") && !Validator.IsBlankOrNull(cellListvalue[contbestvalue]))
        //            {
        //                bestaxu = cel;
        //                bestvalue = cellListvalue[contbestvalue];
        //                bestauxs++;
        //            }
        //            else if (!Validator.IsBlankOrNull(cellListvalue[contbestvalue]) && bestvaluenull==0)
        //            {
        //                bestauxscont = cellListvalue[contbestvalue];
        //                bestvaluenull++;
        //            }
        //        }
        //        contbestvalue++;
        //    }
        //    if(Validator.IsBlankOrNull(bestvalue))
        //    {
        //       bestvalue= bestauxscont;
        //    }
        //    int posisionrank = -1;
        //    foreach (string cellListValues in cellListvalue)
        //    {
        //        if ((Validator.Equals(typeecell, "list") && Validator.IsBlankOrNull(typecelldb)) || (Validator.Equals(typecelldb, "LIS") && Validator.Equals(typeecell, "list")))
        //        {
        //            int c = 0;
                    
        //            string valaxu = cellList[d];
        //            string valueaux = cellListValues;
        //            string bestRanking = valaxu;
        //            string nameproduct = cellproduct[d];
        //            if (valaxu.Equals(markeauxma))
        //            {
        //                flagMarket = 1;
        //            }
        //            else if (valaxu.Equals(markeauxbi))
        //            {
        //                flagMarket = 1;
        //            }
        //            else if (valaxu.Equals(markeauxmp))
        //            {
        //                flagMarket = 1;
        //            }
        //            else if (valaxu.Equals(markeauxmd))
        //            {
        //                flagMarket = 1;
        //            }
        //            else if (valaxu.Equals(markeauxlm))
        //            {
        //                flagMarket = 1;
        //            }
        //            else
        //            {
        //                flagMarket = 0;
        //            }
        //            if (flagMarket == 0)
        //            {
        //                int flagRak = 0;
                        
        //                foreach (string cellListRanking in cellList)
        //                {
        //                    if (!Validator.IsBlankOrNull(valaxu) && !Validator.IsBlankOrNull(cellListvalue[c]) && !Validator.IsBlankOrNull(cellListRanking) && !Validator.IsBlankOrNull(valueaux))
        //                    {
        //                        if (valaxu.Equals(cellListRanking))
        //                        {
        //                            if (valueaux.Equals(cellListvalue[c]))
        //                            {
        //                                bestRanking = valaxu;
        //                                posisionrank = c;
        //                                bestvalue = cellListvalue[c];
        //                            }
        //                            else
        //                            {
        //                                if (valaxu.Equals("Market Advantage"))
        //                                {
        //                                    markeauxma = valaxu;
        //                                }
        //                                else if (valaxu.Equals("Best in Class"))
        //                                {
        //                                    markeauxbi = valaxu;
        //                                }
        //                                else if (valaxu.Equals("Market Parity"))
        //                                {
        //                                    markeauxmp = valaxu;
        //                                }
        //                                else if (valaxu.Equals("Market Disadvantage"))
        //                                {
        //                                    markeauxmd = valaxu;
        //                                }
        //                                else if (valaxu.Equals("Lagging Market"))
        //                                {
        //                                    markeauxlm = valaxu;
        //                                }
        //                                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidationRankingandValue, cellproduct[c], valaxu, valueaux, criteriaNamecell));
        //                                bestRanking = valaxu;
        //                            }

        //                        }
        //                        else
        //                        {
        //                            bestRanking = valaxu;
                                   
        //                        }
        //                    }
        //                    if (flagRak==0)
        //                    {
        //                        flagRak++;
                                
        //                        if (Validator.IsBlankOrNull(valaxu) && !Validator.IsBlankOrNull(cellListvalue[c]) && !Validator.IsBlankOrNull(valueaux))
        //                        {

        //                            if (valueaux.Equals(bestvalue) && posisionrank == -1)
        //                            {
        //                                if (!Validator.IsBlankOrNull(cellListRanking))
        //                                {
        //                                    bestRanking = cellListRanking;
        //                                }
        //                                else
        //                                {
                                            
        //                                    bestRanking = "Market Parity";
        //                                }
        //                                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));
        //                            }
        //                            else if (posisionrank != -1 && valueaux.Equals(bestvalue))
        //                            {
                                        
        //                                bestRanking = cellList[posisionrank];
        //                                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));
        //                            }
        //                            else
        //                            {
        //                                if (validadMarketParty.Equals(valueaux) && !Validator.IsBlankOrNull(validadMarketParty))
        //                                {
        //                                    bestRanking = "Market Parity";
        //                                    validadMarketParty = valueaux;
        //                                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));

        //                                }
        //                                if (cellList.Count > 0 && cellList != null && Validator.IsBlankOrNull(validadMarketParty) && !Validator.IsBlankOrNull(bestRanking))
        //                                {
        //                                    convertrankingstring = convertrankingstring.Except(cellList).ToList();
        //                                    bestRanking = convertrankingstring.First(x => x == "Market Parity");


        //                                }
        //                                if (!Validator.IsBlankOrNull(bestRanking) && Validator.IsBlankOrNull(validadMarketParty))
        //                                {
        //                                    result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));
        //                                    validadMarketParty = valueaux;
        //                                }
        //                                else if (Validator.IsBlankOrNull(bestRanking))
        //                                {
        //                                    if (!Validator.IsBlankOrNull(validadMarketParty))
        //                                    {
        //                                        result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidationRankingandValue, nameproduct, "Market Parity", validadMarketParty, criteriaNamecell));
        //                                    }
        //                                    else
        //                                    {

        //                                        result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidationRankingandValue, nameproduct, "Market Parity", bestvalue, criteriaNamecell));
        //                                    }

        //                                }

        //                            }
        //                            //else if (cellrankingdb.Count < 0)
        //                            //{
                                       
        //                            //        convertrankingstring = convertrankingstring.Except(cellList).ToList();
        //                            //        if (convertrankingstring.Count > 0)
        //                            //        {
        //                            //            if (cellselect != null && cellselect.Count > 0)
        //                            //            {
        //                            //                convertrankingstring = convertrankingstring.Except(cellselect).ToList();
        //                            //            }
        //                            //            bestRanking = convertrankingstring[0];
        //                            //            cellselect.Add(bestRanking);
        //                            //            result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));
        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            bestRanking = valaxu;

        //                            //        }
                                       
        //                            //}
        //                            //else if (cellrankingdb.Count > 0)
        //                            //{
        //                            //    if (Validator.IsBlankOrNull(cellrankingdb[d]))
        //                            //    {
        //                            //        convertrankingstring = convertrankingstring.Except(cellList).ToList();
        //                            //        if (convertrankingstring.Count > 0)
        //                            //        {
        //                            //            if (cellselect != null && cellselect.Count > 0)
        //                            //            {
        //                            //                convertrankingstring = convertrankingstring.Except(cellselect).ToList();
        //                            //            }
        //                            //            if (convertrankingstring.Count > 0)
        //                            //            {
        //                            //                bestRanking = convertrankingstring[0];
        //                            //                cellselect.Add(bestRanking);
        //                            //                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));
        //                            //            }
        //                            //            else
        //                            //            {
        //                            //                bestRanking = "Market Parity" ;
        //                            //            }
        //                            //        }
        //                            //        else
        //                            //        {
        //                            //            bestRanking = valaxu;

        //                            //        }
        //                            //    }

        //                            //}
        //                        }
        //                    }
        //                    c++;
        //                }
                      
        //            }
        //            myranking[controw, contcol] = bestRanking;
        //            myValue[controw, contcol] = cellListValues;

        //            d++;
        //        }
        //        else
        //        {
        //            myranking[controw,contcol] = cellList[controw];
        //            myValue[controw,contcol] = cellListValues;
        //        }
        //        controw++;
        //    }


        //    return result;
        //}
        public string CheckRankingValue(IList<string> cellList, IList<string> cellListvalue, IList<string> cellproduct, IList<string> cellvaluedb, IList<string> cellrankingdb, string typeecell, int contcol, IList<string> convertrankingstring, string typecelldb, string criteriaNamecell)
        {
            string result = string.Empty;
            if ((cellList != null || cellList.Count > 0) && (cellListvalue != null || cellListvalue.Count > 0) && (cellproduct != null || cellproduct.Count > 0) && (cellvaluedb != null || cellvaluedb.Count > 0) && (cellrankingdb != null || cellrankingdb.Count > 0) && (convertrankingstring != null || convertrankingstring.Count > 0))
            {

                IList<string> cellselect = new List<string>();
                int d = 0;
                int controw = 0;
                int flagMarket = 0;
                string markeauxma = "ANY";
                string markeauxbi = "ANY";
                string markeauxmp = "ANY";
                string markeauxmd = "ANY";
                string markeauxlm = "ANY";
                string bestaxu = "";
                string bestvalue = "";

                string bestauxscont = "";
                int contbestvalue = 0;

                string validadMarketParty = "";

                foreach (string cel in cellList)
                {
                    if (cel.ToUpper().Equals("MARKET PARITY") && !Validator.IsBlankOrNull(cellListvalue[contbestvalue]))
                    {
                        bestaxu = cel;
                        bestvalue = cellListvalue[contbestvalue];
                        break;
                    }
                    contbestvalue++;
                }
                if (Validator.IsBlankOrNull(bestaxu))
                {
                    for (int i = 0; i < cellListvalue.Count; i++)
                    {
                        int bestauxs = 0;
                        for (int j = 0; j < cellListvalue.Count; j++)
                        {
                            if (i != j)
                            {
                                if (Validator.Equals(cellListvalue[i], cellListvalue[j]))
                                {
                                    if (!string.IsNullOrEmpty(cellList[j]))
                                    {
                                        bestauxs = 1;
                                        break;
                                    }
                                }
                            }
                        }
                        if (bestauxs == 0 && Validator.IsBlankOrNull(cellList[i]))
                        {
                            bestaxu = "Market Parity";
                            bestauxscont = cellListvalue[i];
                            break;
                        }
                    }
                }

                int posisionrank = -1;
                foreach (string cellListValues in cellListvalue)
                {
                    if ((Validator.Equals(typeecell, "list") && Validator.IsBlankOrNull(typecelldb)) || (Validator.Equals(typecelldb, "LIS") && Validator.Equals(typeecell, "list")))
                    {
                        int c = 0;

                        string valaxu = cellList[d];
                        string valueaux = cellListValues;
                        string bestRanking = valaxu;
                        string nameproduct = cellproduct[d];
                        string error = "";
                        string ValorRep = string.Empty;
                        bool marpetRe = false;
                        bool ValueR = false;
                        if (valaxu.Equals(markeauxma))
                        {
                            flagMarket = 1;
                        }
                        else if (valaxu.Equals(markeauxbi))
                        {
                            flagMarket = 1;
                        }
                        else if (valaxu.Equals(markeauxmp))
                        {
                            flagMarket = 1;
                        }
                        else if (valaxu.Equals(markeauxmd))
                        {
                            flagMarket = 1;
                        }
                        else if (valaxu.Equals(markeauxlm))
                        {
                            flagMarket = 1;
                        }
                        else
                        {
                            flagMarket = 0;
                        }
                        if (flagMarket == 0)
                        {
                            int flagRak = 0;

                            foreach (string cellListRanking in cellList)
                            {
                                if (!Validator.IsBlankOrNull(valaxu) && !Validator.IsBlankOrNull(cellListvalue[c]) && !Validator.IsBlankOrNull(cellListRanking) && !Validator.IsBlankOrNull(valueaux))
                                {
                                    if (!Validator.Equals(valaxu, cellListRanking))
                                    {
                                        if (valueaux.Equals(cellListvalue[c]))
                                        {
                                            ValueR = true;
                                            if (cellListRanking.Equals("Market Advantage"))
                                            {
                                                markeauxma = cellListRanking;
                                            }
                                            else if (cellListRanking.Equals("Best in Class"))
                                            {
                                                markeauxbi = cellListRanking;
                                            }
                                            else if (cellListRanking.Equals("Market Parity"))
                                            {
                                                markeauxmp = cellListRanking;
                                            }
                                            else if (cellListRanking.Equals("Market Disadvantage"))
                                            {
                                                markeauxmd = cellListRanking;
                                            }
                                            else if (cellListRanking.Equals("Lagging Market"))
                                            {
                                                markeauxlm = cellListRanking;
                                            }
                                            string Resu = StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidationRankingandValue, cellproduct[c], valaxu, cellListvalue[c], criteriaNamecell, cellListRanking));
                                            if (result.Contains(Resu))
                                            {


                                            }
                                            else
                                            {
                                                result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidationRankingandValue, cellproduct[c], valaxu, cellListvalue[c], criteriaNamecell, cellListRanking));

                                            }

                                            bestRanking = cellListRanking;


                                        }
                                    }
                                }
                                if (Validator.IsBlankOrNull(valaxu) && !Validator.IsBlankOrNull(cellListvalue[c]) && !Validator.IsBlankOrNull(valueaux))
                                {

                                    if (cellListValues.Equals(cellListvalue[c]))
                                    {
                                        if (!Validator.IsBlankOrNull(cellListRanking))
                                        {
                                            ValueR = true;
                                            bestRanking = cellListRanking;
                                            ValorRep = bestRanking;

                                            marpetRe = true;
                                        }
                                    }
                                    if (cellListValues.Equals(bestauxscont))
                                    {
                                        ValueR = true;
                                        bestRanking = "Market Parity";
                                        ValorRep = bestRanking;
                                        marpetRe = true;


                                    }
                                }


                                c++;
                            }


                        }
                        if (!ValueR)
                        {
                            if (Validator.IsBlankOrNull(bestRanking) && !Validator.IsBlankOrNull(cellListValues))
                            {
                                bestRanking = "Market Parity";
                                //result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));
                            }
                        }
                        if (marpetRe)
                        {
                            //result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.AssignationRanking, nameproduct, bestRanking, criteriaNamecell));
                        }
                        if (Validator.IsBlankOrNull(bestRanking))
                        {
                            if (!string.IsNullOrEmpty(bestauxscont))
                            {
                                if (!string.IsNullOrEmpty(nameproduct) && !string.IsNullOrEmpty(bestaxu) && !string.IsNullOrEmpty(bestauxscont) &&  !string.IsNullOrEmpty(criteriaNamecell))
                                {
                                    //result += StringUtility.InsertHtmlLabels(string.Format(LabelResource.ValidationRankingandValue, nameproduct, bestaxu, bestauxscont, criteriaNamecell));
                                }
                            }
                            myranking[controw, contcol] = "";
                            error = "error";
                        }
                        bool cellListValueBool = false;
                        if (error.Equals("error"))
                        {
                            myranking[controw, contcol] = "";
                            myValue[controw, contcol] = "";
                            cellListValueBool = true;
                        }
                        if (!Validator.IsBlankOrNull(bestRanking) && !Validator.Equals(error, "error"))
                        {
                            myranking[controw, contcol] = bestRanking;
                        }
                        if (!cellListValueBool)
                        {
                            myValue[controw, contcol] = cellListValues;
                        }
                        d++;
                    }
                    else
                    {
                        myranking[controw, contcol] = cellList[controw];
                        myValue[controw, contcol] = cellListValues;
                    }
                    controw++;
                }

            }
            return result;
        }
        public ActionResult UploadImages(FormCollection formValue)
        {
            string fileNameResult = string.Empty;
            string result = string.Empty;
            string headerType = (string)Request["HeaderType"];
            string tempo = string.Empty;
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                Compelligence.Domain.Entity.File newFile = new Compelligence.Domain.Entity.File();
                if (hpf.ContentLength == 0)
                {
                    continue;
                }
                newFile.FileName = System.IO.Path.GetFileName(hpf.FileName);
                newFile.FileFormat = newFile.FileName.Substring(newFile.FileName.LastIndexOf('.') + 1);
                SetDefaultDataForSave(newFile);
                FileService.Save(newFile);
                fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                if (hpf.ContentLength == 0) continue;
                fileNameResult += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');
                hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"], fileNameResult));
                tempo = newFile.FileName;
                //do something with file
            }
            return RedirectToAction("FileForm");
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FileForm2()
        {
            string fileNameResult = string.Empty;

            System.Web.Configuration.HttpRuntimeSection runTime = (System.Web.Configuration.HttpRuntimeSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/httpRuntime");
            int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;//Approx 100 Kb(for page content) size
            if ((Request.ContentLength > maxRequestLength))
            {
                return Content("Fail");
            }
            string result = string.Empty;
            string hpf2 = (string)Request["FileUpload"];
            string headerType = (string)Request["HeaderType"];
            string tempo = string.Empty;
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                Compelligence.Domain.Entity.File newFile = new Compelligence.Domain.Entity.File();
                if (hpf.ContentLength == 0)
                {
                    continue;
                }
                newFile.FileName = System.IO.Path.GetFileName(hpf.FileName);
                newFile.FileFormat = newFile.FileName.Substring(newFile.FileName.LastIndexOf('.') + 1);
                SetDefaultDataForSave(newFile);
                FileService.Save(newFile);
                fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                if (hpf.ContentLength == 0) continue;
                fileNameResult += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');
                hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"], fileNameResult));
                tempo = newFile.FileName;
            }
            string filestreamv = System.IO.Path.Combine(ContextFilePath + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"], fileNameResult);
            int leng = filestreamv.Length;
            Byte[] buffer = new Byte[filestreamv.Length];

            FileStream tyem = null;
            try
            {
                tyem = new FileStream(filestreamv.ToString(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                tyem.Read(buffer, 0, leng);
                NPOI.POIFS.FileSystem.POIFSFileSystem fs = new NPOI.POIFS.FileSystem.POIFSFileSystem(tyem);
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs, true);
                int pos = 0;
                HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(pos);
                int numberRow = sheet.PhysicalNumberOfRows;
                result = "Succesful";
                string industryName = GetValueOfCell(sheet, 0, 1);
                if (!string.IsNullOrEmpty(industryName))
                {
                    Industry industry = IndustryService.GetFirstResultByName(industryName, CurrentCompany);
                    if (industry != null)
                    {
                        int beginRow = 1;
                        int endRow = numberRow;
                        try
                        {
                            HSSFCell CellTempo = (HSSFCell)sheet.GetRow(1).GetCell(0);
                        }
                        catch
                        {
                            beginRow++;
                            endRow++;
                        }
                        int totalColumns = sheet.GetRow(beginRow).LastCellNum;
                        IList<string> nameProductList = new List<string>();
                        IList<ComparinatorCell> cellList = new List<ComparinatorCell>();
                        for (int r = beginRow + 1; r < endRow + 1; r++)
                        {
                            string groupName = string.Empty;
                            string setName = string.Empty;
                            string criteriaName = string.Empty;
                            groupName = GetValueOfCell(sheet, r, 0);
                            setName = GetValueOfCell(sheet, r, 1);
                            criteriaName = GetValueOfCell(sheet, r, 2);
                            for (int c = 3; c < totalColumns; c++)
                            {
                                string value = string.Empty;
                                string productName = string.Empty;
                                productName = GetValueOfCell(sheet, beginRow, c);
                                value = GetValueOfCell(sheet, r, c);
                                if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(setName) && !string.IsNullOrEmpty(criteriaName))
                                {
                                    ComparinatorCell comparinatorCell = new ComparinatorCell();
                                    comparinatorCell.ValueCell = value;
                                    comparinatorCell.Group = groupName;
                                    comparinatorCell.Set = setName;
                                    comparinatorCell.Criteria = criteriaName;
                                    cellList.Add(comparinatorCell);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        result += "\n With Warning - Dont have Value: Industry= " + industryName + ", Criteria Group: " + groupName + ", Criteria Set: " + setName + ",";
                                    }
                                    else if (!string.IsNullOrEmpty(groupName))
                                    { }
                                    else if (!string.IsNullOrEmpty(setName)) { }
                                    else if (!string.IsNullOrEmpty(criteriaName)) { }
                                    result += "\n With Warning - Dont have Value: Industry= " + industryName + ", Criteria Group: " + groupName + ", Criteria Set: " + setName + ",";
                                }
                            }
                        }
                        SaveOrUpdateNewValues(cellList);
                    }
                    else
                    {
                        result = string.Format(LabelResource.ComparinatorCriteriaUploaderIndustryExistError, industryName);
                    }

                }
                else
                {
                    result = LabelResource.NewComparinatorCriteriaUploaderIndustryRequiredError;
                }
            }
            catch (Exception e)
            { }
            ViewData["ShowMessageResult"] = result;
            return View("FileForm");
        }


        public ActionResult RedirectToTest()
        {
            return null;
        }

        private string GetValueOfCell(ISheet sheet, int row, int column)
        {
            string value = string.Empty;
            try
            {
                ICell cell = sheet.GetRow(row).GetCell(column);
                if (cell != null)
                {
                    if (cell.CellType.ToString().Equals("STRING"))
                    {
                        value = cell.StringCellValue.Trim();
                    }
                    else if (cell.CellType.ToString().Equals("NUMERIC"))
                    {
                        value = cell.NumericCellValue.ToString().Trim();
                        value = value.Replace(",", ".");
                    }
                    else if (cell.CellType.ToString().Equals("FORMULA"))
                    {
                        value = cell.StringCellValue.ToString().Trim();
                    }
                    else
                    {
                    }
                }
            }
            catch (Exception e)
            {
                LOG.Error(" ComparinatorCriteriaUploader:GetValueOfCell:" + e.Message, e);
            }
            return value;
        }
        public void SaveOrUpdateNewPrice(IList<Price> cellList)
        {
            if (cellList != null && cellList.Count > 0)
            {
                foreach (Price cellprice in cellList)
                {
                    if ((cellprice.EntityId != null || cellprice.EntityId != -1) && !string.IsNullOrEmpty(cellprice.Name))
                    {
                        Price price = PriceService.GetByEntityIdAndEntityTypeName(Convert.ToDecimal(cellprice.EntityId), "PRODT", cellprice.Name.ToLower(), CurrentCompany);
                        if (price != null)
                        {
                            PricingType pricingType = PricingTypeService.GetByPriceId(price.Id, CurrentCompany);
                            //if (!string.IsNullOrEmpty(cellprice.Name))
                            //{
                            //    price.Name = cellprice.Name;
                            //}
                            if (!string.IsNullOrEmpty(cellprice.PartNumber))
                            {
                                price.PartNumber = cellprice.PartNumber;
                            }
                           //yif (!string.IsNullOrEmpty(cellprice.PricingType))
                           // {
                           //     price.PricingType = cellprice.PricingType;
                           //     if (pricingType != null) 
                           //     {
                           //         //in the case that pricing type is updated
                           //         price.OldPricingTypeId = pricingType.Id;
                           //         if (cellprice.PricingTypeId != pricingType.Id)
                           //         {
                           //             price.PricingTypeId = cellprice.PricingTypeId;
                           //         }
                           //     }
                           // }
                            if (!string.IsNullOrEmpty(cellprice.Units))
                            {
                                price.Units = cellprice.Units;
                            }
                            if (cellprice.Value != null)
                            {
                                price.Value = cellprice.Value;
                            }
                            if (!string.IsNullOrEmpty(cellprice.Status))
                            {
                                price.Status = cellprice.Status;
                            }
                            if (!string.IsNullOrEmpty(cellprice.Required))
                            {
                                price.Required = cellprice.Required;
                            }
                            if (!string.IsNullOrEmpty(cellprice.Url))
                            {
                                price.Url = cellprice.Url;
                            }
                            PriceService.Update(price);
                        }
                        else
                        {
                            price = new Price();
                            price.EntityType = "PRODT";
                            price.EntityId = cellprice.EntityId;
                            price.ClientCompany = CurrentCompany;
                            price.CreatedBy = CurrentUser;
                            price.CreatedDate = DateTime.Now;
                            price.LastChangedBy = CurrentUser;
                            price.LastChangedDate = DateTime.Now;
                            if (!string.IsNullOrEmpty(cellprice.Name))
                            {
                                price.Name = cellprice.Name;

                                if (!string.IsNullOrEmpty(cellprice.PartNumber))
                                {
                                    price.PartNumber = cellprice.PartNumber;
                                }
                                if (!string.IsNullOrEmpty(cellprice.PricingType))
                                {
                                    price.PricingType = cellprice.PricingType;
                                }
                                if (cellprice.PricingTypeId != null && cellprice.PricingTypeId != 0)
                                {
                                    price.PricingTypeId = cellprice.PricingTypeId;
                                }
                                if (!string.IsNullOrEmpty(cellprice.Units))
                                {
                                    price.Units = cellprice.Units;
                                }
                                if (cellprice.Value != null)
                                {
                                    price.Value = cellprice.Value;
                                }
                                if (!string.IsNullOrEmpty(cellprice.Status))
                                {
                                    price.Status = cellprice.Status;
                                }
                                if (!string.IsNullOrEmpty(cellprice.Required))
                                {
                                    price.Required = cellprice.Required;
                                }
                                if (!string.IsNullOrEmpty(cellprice.Url))
                                {
                                    price.Url = cellprice.Url;
                                }
                                PriceService.Save(price);
                            }
                        }
                    }
                }
            }
        }
        public void SaveOrUpdateNewValues(IList<ComparinatorCell> cellList)
        {
            if (cellList != null && cellList.Count > 0)
            {
                foreach (ComparinatorCell cell in cellList)
                {
                    CriteriaGroup criteriaGroup = CriteriaGroupService.GetFirstCriteriaGroupByName(cell.Group, (decimal)cell.IndustryId, CurrentCompany);
                    if (criteriaGroup == null)
                    {
                        criteriaGroup = new CriteriaGroup();
                        criteriaGroup.Name = cell.Group;
                        criteriaGroup.CreatedBy = CurrentUser;
                        criteriaGroup.IndustryId = cell.IndustryId;
                        criteriaGroup.CreatedDate = DateTime.Now;
                        criteriaGroup.LastChangedBy = CurrentUser;
                        criteriaGroup.LastChangedDate = DateTime.Now;
                        criteriaGroup.ClientCompany = CurrentCompany;

                        CriteriaGroupService.Save(criteriaGroup);
                    }

                    if (criteriaGroup != null)
                    {
                        Criteria criteria = new Criteria();
                        if (!string.IsNullOrEmpty(cell.Set))
                        {
                            CriteriaSet criteriaSet = CriteriaSetService.GetFirstCriteriaSetByName(cell.Set, CurrentCompany, criteriaGroup.Id);
                            if (criteriaSet == null)
                            {
                                criteriaSet = new CriteriaSet();
                                criteriaSet.Name = cell.Set;
                                criteriaSet.CreatedBy = CurrentUser;
                                criteriaSet.IndustryId = cell.IndustryId;
                                criteriaSet.CreatedDate = DateTime.Now;
                                criteriaSet.LastChangedBy = CurrentUser;
                                criteriaSet.LastChangedDate = DateTime.Now;
                                criteriaSet.ClientCompany = CurrentCompany;
                                criteriaSet.CriteriaGroupId = criteriaGroup.Id;
                                CriteriaSetService.Save(criteriaSet);
                            }
                            if (criteriaSet != null)
                            {
                                criteria = CriteriaService.GetFirstResultByName(cell.Criteria, (decimal)cell.IndustryId, criteriaSet.Id, criteriaGroup.Id, CurrentCompany);
                                if (criteria == null)
                                {
                                    criteria = CreateNewCriteria(cell, criteriaSet.Id, criteriaGroup.Id);
                                }
                            }
                        }
                        else
                        {
                            criteria = CriteriaService.GetFirstResultByNameAndGroup(cell.Criteria, (decimal)cell.IndustryId, criteriaGroup.Id, CurrentCompany);
                            if (criteria == null)
                            {
                                criteria = CreateNewCriteria(cell, criteriaGroup.Id);
                            }
                        }
                        if (criteria != null)
                        {
                            UpdateCriteria(cell, criteria);
                            CreateIndustryCriteriaByComparinatorCell(cell, criteria);
                            CreateProductCriteriaByComparinatorCell(cell, criteria);
                            CreateOrUpdateIndustryProduct((decimal)cell.IndustryId, (decimal)cell.ProductId);
                            //CreateIndustryCriteriasByComparinatorCell(cell, criteria);
                        }
                    }
                }
            }
        }
        public Criteria checkCriteriaGrup(string name, decimal industryId, decimal grupId, string CurrentCompany)
        {
            Criteria criteria = CriteriaService.GetFirstResultByNameAndIndustryCriterias(name, industryId, CurrentCompany);
            if (criteria == null)
            {
            }
            else
            {
                IndustryCriterias industrycriterias = IndustryCriteriasService.GetByCriteria(industryId, criteria.Id);
                industrycriterias.CriteriaGroupId = grupId;
                IndustryCriteriasService.Update(industrycriterias);
            }
            return criteria;

        }
        public Criteria checkCriteriaSet(string name, decimal industryId, decimal grupId, string CurrentCompany, decimal criteriaSetId)
        {
            Criteria criteria = CriteriaService.GetFirstResultByNameAndGroup(name, industryId, grupId, CurrentCompany);
            if (criteria == null)
            {
            }
            else
            {
                IndustryCriterias industrycriterias = IndustryCriteriasService.GetByCriteria(industryId, criteria.Id);
                industrycriterias.CriteriaGroupId = grupId;
                industrycriterias.CriteriaSetId = criteriaSetId;
                IndustryCriteriasService.Update(industrycriterias);
            }
            return criteria;

        }
        public Criteria CreateNewCriteria(ComparinatorCell comparinatorCell, decimal criteriaSetId, decimal criteriaGroupId)
        {
            Criteria criteria = new Criteria();
            criteria.Name = comparinatorCell.Criteria;
            criteria.CreatedBy = CurrentUser;
            criteria.IndustryId = comparinatorCell.IndustryId;
            criteria.CreatedDate = DateTime.Now;
            criteria.LastChangedBy = CurrentUser;
            criteria.LastChangedDate = DateTime.Now;
            criteria.ClientCompany = CurrentCompany;
            criteria.CriteriaSetId = criteriaSetId;
            criteria.CriteriaSetName = comparinatorCell.Set;
            criteria.CriteriaGroupId = criteriaGroupId;
            criteria.CriteriaGroupName = comparinatorCell.Group;
            criteria.IndustryStandard = comparinatorCell.Industrystandard;
            criteria.Benefit = comparinatorCell.Benefit;
            criteria.MostDesiredValue = comparinatorCell.MostDesiredValue;
            criteria.Type = comparinatorCell.Type;
            criteria.Relevancy = comparinatorCell.Relevancy;
            criteria.Cost = comparinatorCell.Cost;
            CriteriaService.Save(criteria);
            return criteria;
        }

        public Criteria CreateNewCriteria(ComparinatorCell comparinatorCell, decimal criteriaGroupId)
        {
            Criteria criteria = new Criteria();
            criteria.Name = comparinatorCell.Criteria;
            criteria.CreatedBy = CurrentUser;
            criteria.IndustryId = comparinatorCell.IndustryId;
            criteria.CreatedDate = DateTime.Now;
            criteria.LastChangedBy = CurrentUser;
            criteria.LastChangedDate = DateTime.Now;
            criteria.ClientCompany = CurrentCompany;
            criteria.CriteriaGroupId = criteriaGroupId;
            criteria.CriteriaGroupName = comparinatorCell.Group;
            criteria.IndustryStandard = comparinatorCell.Industrystandard;
            criteria.Benefit = comparinatorCell.Benefit;
            criteria.MostDesiredValue = comparinatorCell.MostDesiredValue;
            criteria.Type = comparinatorCell.Type;
            criteria.Relevancy = comparinatorCell.Relevancy;
            criteria.Cost = comparinatorCell.Cost;
            CriteriaService.Save(criteria);
            return criteria;
        }

        public void CreateIndustryCriteriaByComparinatorCell(ComparinatorCell comparinatorCell, Criteria criteria)
        {
            IndustryCriteriaId industryCriteriaId = new IndustryCriteriaId((decimal)comparinatorCell.IndustryId, criteria.Id);
            IndustryCriteria industryCriteria = IndustryCriteriaService.GetById(industryCriteriaId);
            if (industryCriteria == null)
            {
                industryCriteria = new IndustryCriteria(industryCriteriaId);
                industryCriteria.CriteriaId = criteria.Id;

                industryCriteria.CreatedBy = CurrentUser;
                industryCriteria.IndustryId = (decimal)comparinatorCell.IndustryId;
                industryCriteria.CreatedDate = DateTime.Now;
                industryCriteria.LastChangedBy = CurrentUser;
                industryCriteria.LastChangedDate = DateTime.Now;
                industryCriteria.ClientCompany = CurrentCompany;
                IndustryCriteriaService.Save(industryCriteria);
            }
        }

        public void CreateProductCriteriaByComparinatorCell(ComparinatorCell comparinatorCell, Criteria criteria)
        {
            if (!string.IsNullOrEmpty(comparinatorCell.ValueCell) || !string.IsNullOrEmpty(comparinatorCell.Link) || !string.IsNullOrEmpty(comparinatorCell.Note))
            {
                ProductCriteriaId id = new ProductCriteriaId((decimal)comparinatorCell.ProductId, criteria.Id, (decimal)comparinatorCell.IndustryId);
                ProductCriteria productcriteria = ProductCriteriaService.GetById(id);
                if (productcriteria == null)
                {
                    productcriteria = ProductCriteriaService.GetNew((decimal)comparinatorCell.IndustryId, (decimal)comparinatorCell.ProductId, criteria.Id);
                    SetDefaultDataForSave(productcriteria);
                    if (!string.IsNullOrEmpty(comparinatorCell.ValueCell) || !string.IsNullOrEmpty(comparinatorCell.Note))
                    {
                        productcriteria.Value = comparinatorCell.ValueCell;
                        if (criteria.Type.Equals(CriteriaType.Boolean))
                        {
                            productcriteria.Value = GetBoolValue(comparinatorCell.ValueCell);
                        }
                        if (criteria.Type.Equals(CriteriaType.Numeric))
                        {
                            if (DecimalUtility.IsDecimal(comparinatorCell.ValueCell) || comparinatorCell.ValueCell.ToUpper().Equals("N/A") || comparinatorCell.ValueCell.Equals("0"))
                            {
                                productcriteria.Value = comparinatorCell.ValueCell.ToUpper();
                                if (DecimalUtility.IsDecimal(comparinatorCell.ValueCell))
                                {
                                    try
                                    {
                                        productcriteria.ValueDecimal = DecimalUtility.ConvertStringToDecimal(comparinatorCell.ValueCell);
                                        string tempo = DecimalUtility.ConvertDecimalToStringWithFormat(productcriteria.ValueDecimal);
                                        productcriteria.Value = DecimalUtility.ConvertDoubleToString(Convert.ToDouble(productcriteria.ValueDecimal));
                                    }
                                    catch (Exception ex)
                                    {
                                        productcriteria.Value = string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                productcriteria.Value = string.Empty;
                            }
                        }
                        if (!string.IsNullOrEmpty(comparinatorCell.Note))
                        {
                            productcriteria.Notes = comparinatorCell.Note;
                        }
                        if (!string.IsNullOrEmpty(comparinatorCell.Link))
                        {
                            productcriteria.Links = comparinatorCell.Link;
                        }
                        productcriteria.CriteriaUploader = true;
                        productcriteria.Feature = comparinatorCell.Ranking;
                        productcriteria.CriteriaType = criteria.Type;
                        ProductCriteriaService.Save(productcriteria);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(comparinatorCell.ValueCell))
                    {
                        if (!Validator.IsBlankOrNull(comparinatorCell.ValueCell))
                        {
                            productcriteria.Value = comparinatorCell.ValueCell;
                        }
                        if (criteria.Type.Equals(CriteriaType.Boolean))
                        {
                            productcriteria.Value = GetBoolValue(comparinatorCell.ValueCell);
                        }
                        if (criteria.Type.Equals(CriteriaType.Numeric))
                        {
                            if (DecimalUtility.IsDecimal(comparinatorCell.ValueCell) || comparinatorCell.ValueCell.ToUpper().Equals("N/A") || comparinatorCell.ValueCell.Equals("0"))
                            {
                                productcriteria.Value = comparinatorCell.ValueCell.ToUpper();
                                if (DecimalUtility.IsDecimal(comparinatorCell.ValueCell))
                                {
                                    if (comparinatorCell.ValueCell.IndexOf(".") != -1)
                                    {
                                        comparinatorCell.ValueCell = comparinatorCell.ValueCell.Replace(".", ",");
                                    }
                                    productcriteria.ValueDecimal = Decimal.Parse(comparinatorCell.ValueCell);
                                    if (productcriteria.ValueDecimal != 0)
                                    {
                                        productcriteria.Value = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#,#.########}", productcriteria.ValueDecimal);
                                    }
                                }
                            }
                            else
                            {
                                productcriteria.Value = string.Empty;
                            }
                        }
                        if (!Validator.IsBlankOrNull(comparinatorCell.Ranking))
                        {
                            productcriteria.Feature = comparinatorCell.Ranking;
                        }
                        if (!string.IsNullOrEmpty(comparinatorCell.Note))
                        {
                            productcriteria.Notes = comparinatorCell.Note;
                        }
                        if (!string.IsNullOrEmpty(comparinatorCell.Link))
                        {
                            productcriteria.Links = comparinatorCell.Link;
                        }
                        productcriteria.CriteriaType = criteria.Type;
                        productcriteria.CriteriaUploader = true;
                        SetDefaultDataForUpdate(productcriteria);
                        ProductCriteriaService.Update(productcriteria);
                    }
                }
            }
        }

        public void CreateOrUpdateIndustryProduct(decimal industryId, decimal productId)
        {
            IndustryProductId industryProductId = new IndustryProductId(industryId, productId);
            IndustryProduct industryProduct = IndustryProductService.GetById(industryProductId);
            if (industryProduct == null)
            {
                industryProduct = new IndustryProduct(industryProductId);
                industryProduct.CreatedBy = CurrentUser;
                industryProduct.CreatedDate = DateTime.Now;
                industryProduct.LastChangedBy = CurrentUser;
                industryProduct.LastChangedDate = DateTime.Now;
                industryProduct.ClientCompany = CurrentCompany;

                IndustryProductService.Save(industryProduct);
                Product product = ProductService.GetById(productId);
                if (product != null)
                {
                    if (product.Status.Equals(ProductStatus.Disabled) || product.Status.Equals(ProductStatus.New) || string.IsNullOrEmpty(product.Status))
                    {
                        product.Status = ProductStatus.BackEndOnly;
                        ProductService.Update(product);
                    }
                }
            }
        }
        public void UpdateCriteria(ComparinatorCell comparinatorCell, Criteria criteria)
        {
            int updated = 0;
            if (criteria != null)
            {
                if (!string.IsNullOrEmpty(comparinatorCell.Benefit))
                {
                    criteria.Benefit = comparinatorCell.Benefit;
                    updated++;
                }
                if (!string.IsNullOrEmpty(comparinatorCell.Industrystandard))
                {
                    criteria.IndustryStandard = comparinatorCell.Industrystandard;
                    updated++;
                }
                if (!string.IsNullOrEmpty(comparinatorCell.Relevancy))
                {
                    criteria.Relevancy = comparinatorCell.Relevancy;
                    updated++;
                }
                if (!string.IsNullOrEmpty(comparinatorCell.Type))
                {
                    criteria.Type = comparinatorCell.Type;
                    updated++;
                }
                if (!string.IsNullOrEmpty(comparinatorCell.MostDesiredValue))
                {
                    criteria.MostDesiredValue = comparinatorCell.MostDesiredValue;
                    updated++;
                }
                if (!string.IsNullOrEmpty(comparinatorCell.Cost))
                {
                    criteria.Cost = comparinatorCell.Cost;
                    updated++;
                }
                if (updated != 0)
                {
                    CriteriaService.Update(criteria);
                }
            }
        }
        public void CreateIndustryCriteriasByComparinatorCell(ComparinatorCell comparinatorCell, Criteria criteria)
        {
            IndustryCriteriasId industryCriteriasId = new IndustryCriteriasId((decimal)comparinatorCell.IndustryId, criteria.Id);
            IndustryCriterias industryCriterias = IndustryCriteriasService.GetById(industryCriteriasId);
            if (industryCriterias == null)
            {
                industryCriterias = new IndustryCriterias(industryCriteriasId);
                industryCriterias.CriteriaGroupId = criteria.CriteriaGroupId;
                industryCriterias.GroupName = criteria.CriteriaGroupName;
                industryCriterias.OrderGroup = 0;
                if (criteria.CriteriaSetId != null)
                {
                    industryCriterias.CriteriaSetId = criteria.CriteriaSetId;
                    industryCriterias.SetName = criteria.CriteriaSetName;
                    industryCriterias.OrderSet = 0;
                }
                industryCriterias.Order = 0;
                industryCriterias.Visible = "Y";
                industryCriterias.CreatedBy = CurrentUser;
                industryCriterias.CreatedDate = DateTime.Now;
                industryCriterias.LastChangedBy = CurrentUser;
                industryCriterias.LastChangedDate = DateTime.Now;
                industryCriterias.ClientCompany = CurrentCompany;
            }
            else
            {

            }
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckOutPricing()
        {
            decimal id = (decimal)Session["IndustryIdPricing"];
            string fileNameExcel = GetFileNameWithCompanyAndIndustry("Pricing Uploader Template", id);
            if (id != -1)
            {
                string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
                string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
                string tempfile = (string)Session["tempfilePricing"];
                GetDownloadFileWithSpaceResponse(path, tempfile, fileNameExcel + ".xls");
            }

            else
            {
                string userId = (string)Session["UserId"];
                Compelligence.Domain.Entity.File file = new Compelligence.Domain.Entity.File();
                string mimeType = null;
                Compelligence.Domain.Entity.File lastVersion = FileService.UpdateLastFileVersionToCheckOut(DecimalUtility.CheckNull(file.EntityId), file.EntityType);
                string filePath = string.Empty;
                filePath = ConfigurationSettings.AppSettings["ComparinatorPath"];
                string fileName = "CriteriaPricingTemplate.xls";
                string path = filePath + fileName;
                mimeType = FileUtility.GetMimeType("~" + filePath + fileName);

                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + path))
                {
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + fileNameExcel + ".xls\"");
                    Response.Clear();
                    Response.WriteFile("~" + filePath + fileName);
                    Response.End();
                    return Content("File was not found");
                }
            }
            return Content("File was not found");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckOut()
        {
            decimal id = (decimal)Session["IndustryId"];
            string fileNameExcel = GetFileNameWithCompanyAndIndustry("Criteria Uploader Template", id);
            if (id != -1)
            {
                string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
                string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
                string tempfile = (string)Session["tempfile"];
                GetDownloadFileWithSpaceResponse(path, tempfile, fileNameExcel + ".xls");
            }

            else
            {
                string userId = (string)Session["UserId"];
                Compelligence.Domain.Entity.File file = new Compelligence.Domain.Entity.File();
                string mimeType = null;
                Compelligence.Domain.Entity.File lastVersion = FileService.UpdateLastFileVersionToCheckOut(DecimalUtility.CheckNull(file.EntityId), file.EntityType);
                string filePath = string.Empty;
                filePath = ConfigurationSettings.AppSettings["ComparinatorPath"];
                string fileName = "CriteriaUploaderTemplate.xls";
                string path = filePath + fileName;
                mimeType = FileUtility.GetMimeType("~" + filePath + fileName);

                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + path))
                {
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + fileNameExcel + ".xls\"");
                    Response.Clear();
                    Response.WriteFile("~" + filePath + fileName);
                    Response.End();
                    return Content("File was not found");
                }
            }
            return Content("File was not found");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckExistFilePricing()
        {
            string result = "File was not found";
            string filePath = string.Empty;
            filePath = ConfigurationSettings.AppSettings["ComparinatorPath"];
            string fileName = "TemplatePricingUploader.xls";
            string path = filePath + fileName;
            string IndustryId = Request.Params["IndustryId"];
            string chkValue = Request.Params["chkValue"];
            if (string.IsNullOrEmpty(chkValue))
            {
                chkValue = string.Empty;
            }
            string clientCompany = (string)Session["ClientCompany"];
            decimal id = Convert.ToDecimal(IndustryId);
            Session["IndustryIdPricing"] = id;
            if (id != -1)
            {
                Industry industryName = IndustryService.GetById(id);

                string nameIndustry = industryName.Name;
                IList<Product> productList = ProductService.GetByIndustryId(id, clientCompany);
                string path2 = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
                string fullpath = AppDomain.CurrentDomain.BaseDirectory + path2;
                string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                ExportService.ExportComparinatorToExcelPricing(id, chkValue, CurrentUser, CurrentCompany, productList, fullpath, tempfile);
                Session["tempfilePricing"] = tempfile;
            }
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + path))
            {
                result = "Exist";
            }
            return new JsonResult() { Data = result };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckExistFile()
        {
            string result = "File was not found";
            string filePath = ConfigurationSettings.AppSettings["ComparinatorPath"];
            string path = Path.Combine(filePath, "TemplateCriteriaUploader.xls");
            string IndustryId = HttpUtility.HtmlEncode(Request["IndustryId"]);
            string chkValue = HttpUtility.HtmlEncode(Request["chkValue"]);

            if (string.IsNullOrEmpty(chkValue))
            {
                chkValue = string.Empty;
            }
            string clientCompany = (string)Session["ClientCompany"];
            decimal id = Convert.ToDecimal(IndustryId);
            Session["IndustryId"] = id;

            if (id != -1)
            {
                Industry industryName = IndustryService.GetById(id);

                string nameIndustry = industryName.Name;
                IList<IndustryProduct> productList = IndustryProductService.GetByIndustryId(id, clientCompany);
                Session["Products"] = productList;
               
                ExportToExcel(nameIndustry, chkValue);
            }
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + path))
            {
                result = "Exist";
            }
            return new JsonResult() { Data = result };
        }
       
        public void ExportToExcel(string industryName, string chkValue)
        {

            string type = StringUtility.CheckNull(Request["tp"]);
            bool exportFiltered = (string.Compare(StringUtility.CheckNull(Request["ef"]), "y", true) == 0);
            string filters = StringUtility.CheckNull(Request["ft"]);
            string textFilter = StringUtility.CheckNull(Request["tf"]);
            bool hideSameValues = (string.Compare(StringUtility.CheckNull(Request["he"]), "y", true) == 0);
            bool showIndustryStandard = true;
            bool showBenefit = (string.Compare(StringUtility.CheckNull(Request["sb"]), "y", true) == 0);
            string criteriaSetIdHidden = StringUtility.CheckNull(Request["hcs"]);
            string csIdBCHidden = StringUtility.CheckNull(Request["csbc"]);
            string criteriaIdHidden = StringUtility.CheckNull(Request["hc"]);
            if (!string.IsNullOrEmpty(criteriaSetIdHidden))
            {
                criteriaSetIdHidden = criteriaSetIdHidden.Replace("tbl", "");
            }
            if (!string.IsNullOrEmpty(csIdBCHidden))
            {
                csIdBCHidden = csIdBCHidden.Replace("tbl", "");
            }
            List<string> filterOptions = new List<string>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterOptions = filters.Split(new char[] { ':' }).ToList();
            }

            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            ExportService.ExportComparinatorUpdloaderToExcel(Session, type, exportFiltered, filterOptions, textFilter, hideSameValues, showIndustryStandard, showBenefit, fullpath, tempfile, criteriaSetIdHidden, criteriaIdHidden, csIdBCHidden, CurrentUser, CurrentCompany, industryName, chkValue);
            Session["tempfile"] = tempfile;
        }

        private string GetBoolValue(string value)
        {
            string result = value;
            System.Text.RegularExpressions.Regex regexNA = new System.Text.RegularExpressions.Regex("^[nN](/)?[aA]$");
            System.Text.RegularExpressions.Regex regexYes = new System.Text.RegularExpressions.Regex("^[yY][eE][sS]$");
            System.Text.RegularExpressions.Regex regexNo = new System.Text.RegularExpressions.Regex("^[nN][oO]$");
            if (regexYes.IsMatch(value))
            {
                result = ProductCriteriaValueBoolean.Yes;
            }
            else if (regexNo.IsMatch(value))
            {
                result = ProductCriteriaValueBoolean.No;
            }
            else if (regexNA.IsMatch(value))
            {
                result = ProductCriteriaValueBoolean.NA;
            } 
            else 
            {
                result = string.Empty;
            }
            return result;
        }

        private string GetFileNameWithCompanyAndIndustry(string templateName, decimal industryId)
        {
            string fileName = templateName;
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            if (clientCompany != null)
            {
                fileName += "-" + clientCompany.Name;
            }
            if (industryId != -1)
            {
                Industry industry = IndustryService.GetById(industryId);
                if (industry != null)
                {
                    fileName += "-" + industry.Name + " Industry";
                }
            }
            else
            {
                fileName += "-Generic Industry";
            }
            fileName += "-" + DateTime.Today.ToString("MMM-dd-yy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            return fileName;
        }

        public void ExportTextFile(FormCollection form)
        {
            var sb = new System.Text.StringBuilder();
            string nameFile = "Result of ";
            string content = StringUtility.CheckNull(form["hdnContent"]);
            string[] array = content.Split('?');
            nameFile += array[0] + ".txt";
            sb.AppendLine("                                  " + array[0]);
            for (int i = 1; i < array.Length; i++)
            {
                sb.AppendLine(array[i]);
            }

            var response = System.Web.HttpContext.Current.Response;

            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.AddHeader("content-disposition", "attachment; filename=\"" + nameFile + "\"");
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }
    }
}