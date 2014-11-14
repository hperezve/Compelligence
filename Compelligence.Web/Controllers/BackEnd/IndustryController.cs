using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using System.Text;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility.Web;
using Compelligence.Util.Type;
using System.Globalization;
using Compelligence.DataTransfer.Comparinator;
using Compelligence.Common.Utility;
using System.IO;
using System.Drawing;
using Compelligence.Common.Utility.Upload;
using Compelligence.Common.Browse;
using System.Text.RegularExpressions;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class IndustryController : BackEndAsyncFormController<Industry, decimal>
    {

        #region Public Properties
        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        public IIndustryService IndustryService
        {
            get { return (IIndustryService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IEmailService EmailService { get; set; }

        public IDealService DealService { get; set; }

        public IProductService ProductService { get; set; }

        public ICustomFieldService CustomFieldService { get; set; }
        public IIndustryCriteriasService IndustryCriteriasService { get; set; }
        public ICriteriaGroupService CriteriaGroupService { get; set; }
        public ICriteriaSetService CriteriaSetService { get; set; }
        public IFileService FileService { get; set; }
        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }

        #endregion

        #region Validation Methods


        protected override bool ValidateFormData(Industry industry, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];

            string dupName = formCollection["dupName"];
            if (!industry.Name.Equals(dupName))
            {
                if (IndustryService.GetByNameIndustry(industry.Name, CurrentCompany) > 0)
                {
                    ValidationDictionary.AddError("Name", LabelResource.IndustryErrorNameExist);
                }
            }
            if (Validator.IsBlankOrNull(industry.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.IndustryNameRequiredError);
            }

            if (Validator.IsBlankOrNull(industry.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.IndustryOwnerIdRequiredError);
            }
            if (!Validator.IsBlankOrNull(industry.BudgetFrm) && !Validator.IsDecimal(industry.BudgetFrm))
            {
                ValidationDictionary.AddError("BudgetFrm", LabelResource.IndustryBudgetFormatError);
            }

            if ((industry.Status.Equals(IndustryStatus.Disabled)) && (IndustryService.HassDependences(industry, clientCompany)))
            {
                ValidationDictionary.AddError("Status", LabelResource.IndustryStatusValueError);
            }
            if (!Validator.IsBlankOrNull(industry.Parent) && (industry.Id!=0) && (IndustryService.IsChild((decimal)industry.Id, (decimal)industry.Parent)))
            {
                ValidationDictionary.AddError("Parent", LabelResource.IndustryParentRequiredError);
            }
            if (!Validator.IsBlankOrNull(industry.Website) && !Validator.IsUrl(industry.Website))
            {
                ValidationDictionary.AddError("Website", LabelResource.GlobalUrlFormatError);
            }
            if (!string.IsNullOrEmpty(industry.ImageUrl))
            {
                if (industry.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                {
                    byte[] imageData = ResizeImage.GetBytesFromUrl(industry.ImageUrl);
                    if (imageData != null)
                    {
                        string[] urlObjects = industry.ImageUrl.Split('/');

                        Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                        System.Drawing.Image newImage = ResizeImage.GetResizeStream(imageData, 250, 80);

                        if (newImage != null)
                        {

                            if (urlObjects.Length > 0)
                            {
                                newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                                if (newFileImage.FileName.IndexOf("%20") != -1)
                                {
                                    newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
                                }
                                if (newFileImage.FileName.IndexOf("?") != -1)
                                {
                                    string[] parameterBegin = newFileImage.FileName.Split('?');
                                    if (parameterBegin.Length > 1)
                                    {
                                        newFileImage.FileName = parameterBegin[0];
                                    }
                                }
                                if (newFileImage.FileName.IndexOf("&") != -1)
                                {
                                    string[] parameterOther = newFileImage.FileName.Split('?');
                                    if (parameterOther.Length > 1)
                                    {
                                        newFileImage.FileName = parameterOther[0];
                                    }
                                }
                                if (newFileImage.FileName.IndexOf("=") != -1)
                                {
                                    string[] parameterAssignment = newFileImage.FileName.Split('?');
                                    if (parameterAssignment.Length > 1)
                                    {
                                        newFileImage.FileName = parameterAssignment[0];
                                    }
                                }
                            }
                            if (newFileImage.FileName.LastIndexOf('.') != -1)
                            {
                                newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir

                                if (newFileImage.FileFormat.Equals("ashx"))
                                {
                                    newFileImage.FileFormat = "jpg";
                                    newFileImage.FileName = newFileImage.FileName.Substring(0, newFileImage.FileName.LastIndexOf('.')) + "." + newFileImage.FileFormat;
                                }
                            }
                            else
                            {
                                newFileImage.FileFormat = "jpg";
                                string mpgfile = ResizeImage.GetUrlFileName(newFileImage.FileName);
                                newFileImage.FileName = mpgfile + "." + newFileImage.FileFormat;
                            }

                            decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                            string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
                            string fileNameResult = string.Empty;

                            SetDefaultDataForSave(newFileImage);
                            newFileImage.PhysicalName = newPhysicalName;

                            FileService.Save(newFileImage);
                            fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
                            newImage.Save(fileNameResult);
                            industry.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");

                        }
                        else //get null is dont exist resource
                            ErrorImage(true);
                    }
                    else //get null is dont exist resource
                        ErrorImage(true);
                }
            }

            return ValidationDictionary.IsValid;


            
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> industryStatusList = ResourceService.GetAll<IndustryStatus>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Industry> industryParentList = IndustryService.GetAllSortByClientCompany("Name", clientCompany);
            IList<ResourceObject> tierList = ResourceService.GetAll<IndustryTier>();
            UrlParser url = new UrlParser(Request.Url.ToString());

            string industry = url.GetLastUrlObjects(Request.FilePath);
            //string industry = Request.FilePath.Substring(20);
            IList<Industry> parentList = new List<Industry>();

            if (industry != null)
            {
                foreach (Industry item in industryParentList)
                {
                    if (!item.Id.ToString().Equals(industry))
                    {
                        parentList.Add(item);
                    }
                }
                ViewData["IndustryParentList"] = new SelectList(parentList, "Id", "Name");
            }
            else
            {
                ViewData["IndustryParentList"] = new SelectList(industryParentList, "Id", "Name");
            }

            foreach (ResourceObject o in tierList)
            {
                o.Value = GeneralStringParser.InsertBlank(o.Value);

            }

            ViewData["StatusList"] = new SelectList(industryStatusList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");

            ViewData["TierList"] = new SelectList(tierList, "Id", "Value");

            IList<CustomField> customfieldlist = CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Industry, CurrentCompany);
            if (customfieldlist == null)
                customfieldlist = new List<CustomField>();
            ViewData["CustomFieldList"] = customfieldlist;
        }

        public ContentResult CheckSomething(bool IsChecked, string[] IndustryIds)
        {
            string[] tempo = new string[] { };
            var selected = tempo;
            if (IndustryIds != null)
            {
                selected = IndustryIds;
            }
            string result = string.Empty;
            result = result + string.Empty + ":" + string.Empty + ":" + string.Empty + "_";
            bool isSelected = false;
            if (IsChecked)
            {
                IList<IndustryByHierarchyView> industryListHierarchy = IndustryService.FindAllActiveByHierarchy(CurrentCompany);

                int cont = 0;
                foreach (IndustryByHierarchyView industryHierarchy in industryListHierarchy)
                {
                    isSelected = false;
                    if (selected.Length > 0)
                    {
                        for (int m = 0; m < selected.Length; m++)
                        {
                            if (decimal.Parse(selected[m]) == industryHierarchy.Id)
                            {
                                isSelected = true;
                                m = selected.Length;
                            }
                        }
                    }

                    cont++;
                    if (industryListHierarchy.Count == cont)
                    {
                        result = result + industryHierarchy.Id + ":" + industryHierarchy.Name + ":" + isSelected.ToString();
                    }
                    else
                    {
                        result = result + industryHierarchy.Id + ":" + industryHierarchy.Name + ":" + isSelected.ToString() + "_";
                    }
                }
            }
            else
            {
                IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(CurrentCompany);
                int cont2 = 0;
                foreach (Industry industry in industryList)
                {
                    isSelected = false;
                    if (selected.Length > 0)
                    {
                        for (int m = 0; m < selected.Length; m++)
                        {
                            if (decimal.Parse(selected[m]) == industry.Id)
                            {
                                isSelected = true;
                                m = selected.Length;
                            }
                        }
                    }

                    cont2++;
                    if (industryList.Count == cont2)
                    {
                        result = result + industry.Id + ":" + industry.Name + ":" + isSelected.ToString();
                    }
                    else
                    {
                        result = result + industry.Id + ":" + industry.Name + ":" + isSelected.ToString() + "_";
                    }

                }
            }
            return Content(result);
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Industry;

            switch (detailType)
            {
                case DetailType.Competitor:
                    AddFilter(detailFilter, "Competitor.IndustryId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorIndustryDetailView.IndustryId", parentId.ToString());
                    childController = "Competitor:CompetitorIndustryDetail";
                    break;
                case DetailType.Product:
                    AddFilter(detailFilter, "Product.IndustryId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProductIndustryDetailView.IndustryId", parentId.ToString());
                    childController = "Product:ProductIndustryDetail";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Industry);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());                    
                    childController = "Team:TeamIndustryDetail";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Industry);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Industry);
                    childController = "User";
                    break;
                //EndUSer
                //case DetailType.Budget:
                //    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                //    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Industry);
                //    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                //    childController = "Budget";
                //    break;
                case DetailType.Implication:
                    string entityIdValueImplication = GetDetailFilterValue("Industry.EntityId");
                    string entityTypeValueImplication = GetDetailFilterValue("Industry.EntityType");
                    
                    AddFilter(detailFilter, "Implication.IndustryId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityId", entityIdValueImplication);
                    AddFilter(detailFilter, "Implication.EntityType", entityTypeValueImplication);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", entityIdValueImplication);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityType", entityTypeValueImplication);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.IndustryId", parentId.ToString());
                    
                    childController = "Implication";
                    break;
                //case DetailType.Metric:
                //    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                //    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Industry);
                //    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                //    childController = "Metric";
                //    break;
                case DetailType.ProductCriteria:
                    string productIdValue = GetDetailFilterValue("Industry.ProductId");
                    AddFilter(detailFilter, "ProductCriteria.IndustryId", parentId.ToString());
                    AddFilter(detailFilter, "ProductCriteria.ProductId", productIdValue);
                    AddFilter(browseDetailFilter, "ProductCriteriaDetailView.IndustryId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProductCriteriaDetailView.ProductId", productIdValue);
                    childController = "ProductCriteria";
                    break;
                case DetailType.CompetitorCriteria:
                    string competitorIdValue = GetDetailFilterValue("Industry.CompetitorId");
                    AddFilter(detailFilter, "CompetitorCriteria.IndustryId", parentId.ToString());
                    AddFilter(detailFilter, "CompetitorCriteria.CompetitorId", competitorIdValue);
                    AddFilter(browseDetailFilter, "CompetitorCriteriaDetailView.IndustryId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorCriteriaDetailView.CompetitorId", competitorIdValue);
                    childController = "CompetitorCriteria";
                    break;
                case DetailType.Criteria: //Comparinator
                    AddFilter(detailFilter, "Criteria.IndustryId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CriteriaIndustryDetailView.IndustryId", parentId.ToString());
                    childController = "Criteria:CriteriaIndustryDetail";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Industry);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Feedback:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Industry);
                    childController = "ForumFeedback";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Industry);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Industry);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Industry);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
                case DetailType.Positioning:
                    string entityIdValue = GetDetailFilterValue("Industry.EntityId");
                    string entityTypeValue = GetDetailFilterValue("Industry.EntityType");

                    AddFilter(detailFilter, "Positioning.IndustryId", parentId.ToString());
                    AddFilter(detailFilter, "Positioning.EntityId", entityIdValue);
                    AddFilter(detailFilter, "Positioning.EntityType", entityTypeValue);
                    AddFilter(browseDetailFilter, "PositioningDetailView.EntityId", entityIdValue);
                    AddFilter(browseDetailFilter, "PositioningDetailView.EntityType", entityTypeValue);
                    AddFilter(browseDetailFilter, "PositioningDetailView.IndustryId", parentId.ToString());
                    //AddFilter(browseDetailFilter, "PositioningDetailView.IsMaster", "Y");
                    childController = "Positioning";
                    break;
                case DetailType.LibraryNews:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Industry);
                    AddFilter(browseDetailFilter, "LibraryNewsDetailView.EntityId", BrowseFilter.Operator.Cn, parentId.ToString());
                    childController = "LibraryNews";
                    break;
                case DetailType.IndustryFinancial:
                    AddFilter(detailFilter, "IndustryFinancial.IndustryId", parentId.ToString());
                    AddFilter(browseDetailFilter, "IndustryFinancialDetailView.IndustryId", parentId.ToString());
                    childController = "IndustryFinancial";
                    break;

                //case DetailType.CriteriaGroup: //Comparinator
                //    AddFilter(detailFilter, "Criteria.IndustryId", parentId.ToString());
                //    AddFilter(browseDetailFilter, "CriteriaIndustryDetailView.IndustryId", parentId.ToString());
                //    childController = "Criteria:CriteriaIndustryDetail";
                //    break;
                case DetailType.Comment:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Industry);
                    childController = "ForumComment";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Industry industry)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (IndustryService.HasAccessToIndustry(industry, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            string productIdValue = GetDetailFilterValue("Industry.ProductId");

            if (!string.IsNullOrEmpty(productIdValue))
            {
                Product prod = ProductService.GetById(Convert.ToDecimal(productIdValue));
                if (prod != null && ProductService.HasAccessToProduct(prod, userId))
                {
                    securityAccess = UserSecurityAccess.Edit;
                }
            }
            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void SetEntityDataToForm(Industry industry)
        {
            Session["IndustryId"] = industry.Id;
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(industry.MetaData);
            ViewData["BudgetFrm"] = FormatUtility.GetFormatValue("{0:0.##}", industry.Budget);
            ViewData["BudgetTimeFrm"] = FormatUtility.GetFormatValue("{0:0.##}", industry.BudgetTime);
            industry.OriginalStatus = industry.Status;
            industry.OldName = industry.Name;
            industry.OldParent = industry.Parent;
            IList<CustomField> fields = CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Industry, CurrentCompany);
            industry = IndustryService.GetDynamicFieldsById(industry, fields);
        }

        protected override void GetFormData(Industry industry, FormCollection collection)
        {
            //industry.OldParent = industry.Parent;
            industry.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
            industry.Budget = FormatUtility.GetDecimalValue(collection["BudgetFrm"]);
            industry.BudgetTime = FormatUtility.GetDecimalValue(collection["BudgetTimeFrm"]);
            IList<CustomField> fields = CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Industry, CurrentCompany);
            foreach (CustomField field in fields)
            {
                String value = collection[field.PhysicalName] ?? string.Empty;
                if (!industry.CustomFields.ContainsKey(field.PhysicalName))
                industry.CustomFields.Add(field.PhysicalName, value);
            }

            //if (!string.IsNullOrEmpty(industry.ImageUrl))
            //{
            //    if (industry.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
            //    {
            //        byte[] imageData = ResizeImage.GetBytesFromUrl(industry.ImageUrl);
            //        if (imageData != null)
            //        {
            //            MemoryStream stream = new MemoryStream(imageData);

            //            try {Image fullsizeImage = Image.FromStream(stream);
            //            stream.Close();

            //            string[] urlObjects = industry.ImageUrl.Split('/');

            //            int newWidth = 250;
            //            int newHeight = 80;

            //            fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            //            fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            //            if (fullsizeImage.Width > newWidth)
            //            {
            //                newWidth = fullsizeImage.Width;
            //            }
            //            int resizeHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
            //            if (resizeHeight > newHeight)
            //            {
            //                newWidth = fullsizeImage.Width * newHeight / fullsizeImage.Height;
            //                resizeHeight = newHeight;
            //            }

            //            System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, resizeHeight, null, IntPtr.Zero);
            //            fullsizeImage.Dispose();

            //            Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();


            //            if (urlObjects.Length > 0)
            //            {
            //                newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
            //                if (newFileImage.FileName.IndexOf("%20") != -1)
            //                {
            //                    newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
            //                }
            //                if (newFileImage.FileName.IndexOf("?") != -1)
            //                {
            //                    string[] parameterBegin = newFileImage.FileName.Split('?');
            //                    if (parameterBegin.Length > 1)
            //                    {
            //                        newFileImage.FileName = parameterBegin[0];
            //                    }
            //                }
            //                if (newFileImage.FileName.IndexOf("&") != -1)
            //                {
            //                    string[] parameterOther = newFileImage.FileName.Split('?');
            //                    if (parameterOther.Length > 1)
            //                    {
            //                        newFileImage.FileName = parameterOther[0];
            //                    }
            //                }
            //                if (newFileImage.FileName.IndexOf("=") != -1)
            //                {
            //                    string[] parameterAssignment = newFileImage.FileName.Split('?');
            //                    if (parameterAssignment.Length > 1)
            //                    {
            //                        newFileImage.FileName = parameterAssignment[0];
            //                    }
            //                }
            //            }
            //            if (newFileImage.FileName.LastIndexOf('.') != -1)
            //            {
            //                newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir
            //            }
            //            if (newFileImage.FileFormat.Equals("ashx"))
            //            {
            //                newFileImage.FileFormat = "jpg";
            //                newFileImage.FileName = newFileImage.FileName.Substring(0, newFileImage.FileName.LastIndexOf('.')) + "." + newFileImage.FileFormat;
            //            }
            //            decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
            //            string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
            //            string fileNameResult = string.Empty;

            //            SetDefaultDataForSave(newFileImage);
            //            newFileImage.PhysicalName = newPhysicalName;

            //            FileService.Save(newFileImage);
            //            fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
            //            newImage.Save(fileNameResult);
            //            industry.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");
            //            }
            //            catch
            //            {
            //                bool ErrorImageS = true;
            //                ErrorImage(ErrorImageS);
            //            }
            //        }
            //    }
            //}
            industry.ImageUrl = GetNewUrlToImage(industry.ImageUrl, ContextFilePath, FileUploadType.Image); 
        }
        
        protected override void SaveFile(Compelligence.Domain.Entity.File file)
        {
            FileService.Save(file);
        }

        protected override void ReturnResult(bool error)
        {
            ErrorImage(error);
        }
        
        protected void ErrorImage(bool ErrorImage)
        {
            if (ErrorImage == true)
            {
                ValidationDictionary.AddError("ImageUrl", LabelResource.ImageValueError);
            }
        }

        protected override void SetDefaultEntityDataForSave(Industry industry)
        {
            industry.MetaData = industry.Name + ":" + industry.MetaData;
        }

        protected override void SetFormEntityDataToForm(Industry industry)
        {
            industry.OldName = industry.Name;
            industry.MetaData = FormFieldsUtility.GetMultilineValue(industry.MetaData);
            industry.OriginalStatus = industry.Status;
            
            ModelState.SetModelValue("MetaData", new ValueProviderResult(industry.MetaData, industry.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(industry.OldName, industry.OldName, CultureInfo.InvariantCulture));

            ModelState.SetModelValue("OriginalStatus", new ValueProviderResult(industry.OriginalStatus, industry.OriginalStatus, CultureInfo.InvariantCulture));
        }

        protected override bool ValidateDeleteData(Industry entity, System.Text.StringBuilder errorMessage)
        {
            //if this industry has child industries
            if (ListChildren(entity.Id).Count > 0)
            {
                Industry ChildIndustry = ListChildren(entity.Id)[0];
                errorMessage.AppendLine(LabelResource.IndustryCannotDeleteError + " " + ChildIndustry.Name);
                return false;
            } 

            //if this industry has associated deals
            if (ListDeals(entity.Id).Count > 0)
            {
                Deal deal = ListDeals(entity.Id)[0];
                errorMessage.AppendLine(LabelResource.IndustryCannotDeleteErrorDeal + " " + deal.Name);
                return false;
            } 
            return base.ValidateDeleteData(entity, errorMessage);
        }
        #endregion

        #region Private Methods

        private IList<Industry> ListChildren(decimal parentId)
        {
            IList<Industry> lstIndustry = IndustryService.ListChildren(parentId);
            return lstIndustry;
        }
        private IList<Deal> ListDeals(decimal industryId)
        {
            IList<Deal> lstDeals = DealService.GetByÍndustryId(industryId);
            return lstDeals;
        }
        #endregion

        #region Action Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CustomCriteriaStep2(decimal IndustryId)
        {
            ComparinatorCatalog catalog = new ComparinatorCatalog();


            UserProfile User = UserProfileService.GetById(CurrentUser);


            CriteriaGroup AnonymousGroup = CriteriaGroupService.GetNew(IndustryId);
            AnonymousGroup.Name = "Without Group";
            CriteriaSet AnonymousSet = CriteriaSetService.GetNew(IndustryId);
            AnonymousSet.Name = "Without Set";

            IList<Criteria> criterias = IndustryCriteriasService.GetCriteriasByIndustry(IndustryId, CurrentCompany);

            //Fill Catalog with Criterias and Products
            foreach (Criteria criteria in criterias)
            {

                ComparinatorCriteria rowCriteria = new ComparinatorCriteria(criteria);
                IndustryCriterias industrycriterias = IndustryCriteriasService.GetByCriteria(IndustryId, criteria.Id);

                if (industrycriterias.CriteriaGroupId != null && industrycriterias.CriteriaSetId != null)
                {
                    rowCriteria.CriteriaSet = CriteriaSetService.GetById((decimal)industrycriterias.CriteriaSetId);
                    rowCriteria.CriteriaGroup = CriteriaGroupService.GetById((decimal)industrycriterias.CriteriaGroupId);
                    if( rowCriteria.CriteriaSet==null)
                        rowCriteria.CriteriaSet = AnonymousSet;
                    if( rowCriteria.CriteriaGroup==null)
                        rowCriteria.CriteriaGroup = AnonymousGroup;


                }
                else if (industrycriterias.CriteriaGroupId != null && industrycriterias.CriteriaSetId == null)
                {
                    rowCriteria.CriteriaSet = AnonymousSet;
                    rowCriteria.CriteriaGroup = CriteriaGroupService.GetById((decimal)industrycriterias.CriteriaGroupId);
                    if (rowCriteria.CriteriaGroup == null)
                        rowCriteria.CriteriaGroup = AnonymousGroup;

                }
                else //Asume Set and Group = null
                {
                    rowCriteria.CriteriaSet = AnonymousSet;
                    rowCriteria.CriteriaGroup = AnonymousGroup;
                }

                catalog.Push(rowCriteria);
            }
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["IndustryId"] = IndustryId;
            ViewData["TargetIndustryId"] = Request["TargetIndustryId"];
            ViewData["ComparinatorGroups"] = catalog.Groups;
            Session["ComparinatorGroups"] = catalog.Groups;
            ViewData["User"] = User;
            return View("CustomCriteriaStep2");
        }
        
        public ActionResult AddCriterias()
        {
            if (Request["CriteriaId"] != null && 
                Request["IndustryId"] != null &&
                Request["TargetIndustryId"] != null)
            {
                String industryid = (String)Request["IndustryId"];
                String targetindustryid = (String)Request["TargetIndustryId"];
                string[] criteriaids = Request["CriteriaId"].Split(',');
                foreach (String criteriaid in criteriaids)
                {
                    IndustryCriterias industrycriterias=IndustryCriteriasService.GetByCriteria(decimal.Parse(industryid), decimal.Parse(criteriaid));
                    IndustryCriterias newindustrycriterias = IndustryCriteriasService.GetByCriteria(decimal.Parse(targetindustryid), decimal.Parse(criteriaid));
                    if (newindustrycriterias == null)
                    {
                        IndustryCriteriasId id = new IndustryCriteriasId(decimal.Parse(targetindustryid), decimal.Parse(criteriaid));
                        newindustrycriterias = new IndustryCriterias(id);
                        newindustrycriterias.CriteriaSetId = industrycriterias.CriteriaSetId;
                        newindustrycriterias.CriteriaGroupId = industrycriterias.CriteriaGroupId;
                        newindustrycriterias.Visible = "Y";
                        newindustrycriterias.OrderGroup = 0;
                        newindustrycriterias.OrderSet = 0;
                        newindustrycriterias.CreatedBy = CurrentUser;
                        newindustrycriterias.CreatedDate = DateTime.Now;
                        newindustrycriterias.LastChangedBy = CurrentUser;
                        newindustrycriterias.LastChangedDate = DateTime.Now;
                        newindustrycriterias.ClientCompany = CurrentCompany;
                        IndustryCriteriasService.Save(newindustrycriterias);

                        //Here change if don't work Copy Criterias
                        // CriteriaGroup cg = CriteriaGroupService.GetByIndustryId(decimal.Parse(targetindustryid));

                    }
                    //IndustryCriterias
                }
            }
            return Content("<script> self.close(); </script>");
        }

        public ContentResult multiAddCriteria(string ids, string industryIds, string targetIndustryIds)
        {
            string result = "false";

            if (!string.IsNullOrEmpty(ids) &&
                !string.IsNullOrEmpty(industryIds) &&
                !string.IsNullOrEmpty(targetIndustryIds))
            {
                String industryid = industryIds;
                String targetindustryid = targetIndustryIds;
                string[] criteriaids = ids.Split(',');
                foreach (String criteriaid in criteriaids)
                {
                    IndustryCriterias industrycriterias = IndustryCriteriasService.GetByCriteria(decimal.Parse(industryid), decimal.Parse(criteriaid));
                    IndustryCriterias newindustrycriterias = IndustryCriteriasService.GetByCriteria(decimal.Parse(targetindustryid), decimal.Parse(criteriaid));
                    if (newindustrycriterias == null)
                    {
                        IndustryCriteriasId id = new IndustryCriteriasId(decimal.Parse(targetindustryid), decimal.Parse(criteriaid));
                        newindustrycriterias = new IndustryCriterias(id);
                        newindustrycriterias.CriteriaSetId = industrycriterias.CriteriaSetId;
                        newindustrycriterias.CriteriaGroupId = industrycriterias.CriteriaGroupId;
                        newindustrycriterias.Visible = "Y";
                        newindustrycriterias.OrderGroup = 0;
                        newindustrycriterias.OrderSet = 0;
                        newindustrycriterias.CreatedBy = CurrentUser;
                        newindustrycriterias.CreatedDate = DateTime.Now;
                        newindustrycriterias.LastChangedBy = CurrentUser;
                        newindustrycriterias.LastChangedDate = DateTime.Now;
                        newindustrycriterias.ClientCompany = CurrentCompany;
                        IndustryCriteriasService.Save(newindustrycriterias);

                        //Here change if don't work Copy Criterias
                        // CriteriaGroup cg = CriteriaGroupService.GetByIndustryId(decimal.Parse(targetindustryid));

                    }
                    //IndustryCriterias
                }
                result = "true";
            }
            return Content(result);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CustomCriteriaStep1()
        {
            String IndustryId = GetDetailFilterValue("Criteria.IndustryId");
            IList<Industry> industries = IndustryService.GetAllActiveByClientCompany(CurrentCompany);
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["Industries"] = industries;
            ViewData["TargetIndustryId"] = IndustryId;
            return View("CustomCriteriaStep1");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ReturnCustomCriteriaStep1(string detailFilter)
        {
            ViewData["DetailFilter"] = detailFilter;
            String IndustryId = GetDetailFilterValue("Criteria.IndustryId");           
            IList<Industry> industries = IndustryService.GetAllActiveByClientCompany(CurrentCompany);            
            ViewData["Industries"] = industries;
            ViewData["TargetIndustryId"] = IndustryId;
            return View("CustomCriteriaStep1");
            return View("CustomCriteriaStep1");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult UpdateEnableStatus()
        {
            string[] ids = StringUtility.CheckNull(Request["Id[]"]).Split(','); //change becasue jquery-1.9
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            string nameEntitiesChangeSuccessful = string.Empty;
            string nameEntitiesChangesUnsuccessful = string.Empty;

            foreach (string  identifier in ids)
            {
                Industry industry = IndustryService.GetById(decimal.Parse(identifier));
                if (industry != null)
                {
                    if (!industry.Status.Equals(IndustryStatus.Enabled))
                    {
                        industry.Status = IndustryStatus.Enabled;
                        SetDefaultDataForUpdate(industry);
                        IndustryService.Update(industry);
                        if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
                        {
                            nameEntitiesChangeSuccessful += ":";
                        }
                        nameEntitiesChangeSuccessful += industry.Name;
                    }
                    else
                    {
                        string message = string.Empty;
                        if (industry.Status.Equals(IndustryStatus.Enabled))
                        {
                            message = LabelResource.IndustryStatusCurrencyValueError + " " + ResourceService.GetName<IndustryStatus>(industry.Status);
                        }
                        else
                        {
                            message = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                        {
                            nameEntitiesChangesUnsuccessful += ":";
                        }
                        nameEntitiesChangesUnsuccessful += industry.Name + " ( " + message + " )";
                    }
                }
            }
            //if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
            //{
            //    string[] namesEntitySuccessful = nameEntitiesChangeSuccessful.Split(':');
            //    returnMessage.Append("The Status of Industries was changed successfully");
            //    returnMessage.Append("<p><ul class='disc' style='display: block;'>");
            //    foreach (string nes in namesEntitySuccessful)
            //    {
            //        returnMessage.Append("<li>"+nes+"</li>");
            //    }
            //    returnMessage.Append("</ul></p>");
            //}
            if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
            {
                string[] namesEntityUnSuccessful = nameEntitiesChangesUnsuccessful.Split(':');
                returnMessage.Append("<br/><div  style='color:Red;text-indent:30px>The Status of Industries was not changed successfully");
                returnMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntityUnSuccessful)
                {
                    returnMessage.Append("<li>" + neus + "</li>");
                }
                returnMessage.Append("</ul></p></div>");
                return Content(returnMessage.ToString());
            }
            else
            {
                return null;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult UpdateDisableStatus()
        {
            string[] ids = StringUtility.CheckNull(Request["Id[]"]).Split(','); //change becasue jquery-1.9

            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            string nameEntitiesChangeSuccessful = string.Empty;
            string nameEntitiesChangesUnsuccessful = string.Empty;
            
            foreach (string identifier in ids)
            {
                Industry industry = IndustryService.GetById(decimal.Parse(identifier));
                if (industry != null)
                {
                    if (!industry.Status.Equals(IndustryStatus.Disabled) && (!IndustryService.HassDependences(industry, CurrentCompany)))
                    {
                        industry.Status = IndustryStatus.Disabled;
                        SetDefaultDataForUpdate(industry);
                        IndustryService.Update(industry);
                        if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
                        {
                            nameEntitiesChangeSuccessful += ":";
                        }
                        nameEntitiesChangeSuccessful += industry.Name;
                    }
                    else
                    {
                        string message = string.Empty;
                        if (industry.Status.Equals(IndustryStatus.Disabled))
                        {
                            message = LabelResource.IndustryStatusCurrencyValueError + " " + ResourceService.GetName<IndustryStatus>(industry.Status);
                        }
                        else if (IndustryService.HassDependences(industry, CurrentCompany))
                        {
                            message = LabelResource.IndustryStatusValueError;
                        }
                        else
                        {
                            message = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                        {
                            nameEntitiesChangesUnsuccessful += ":";
                        }
                        nameEntitiesChangesUnsuccessful += industry.Name + " ( " + message+" )";
                    }
                }
            }
            //if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
            //{
            //    string[] namesEntitySuccessful = nameEntitiesChangeSuccessful.Split(':');
            //    returnMessage.Append("The Status of Industries was changed successfully");
            //    returnMessage.Append("<p><ul class='disc' style='display: block;'>");
            //    foreach (string nes in namesEntitySuccessful)
            //    {
            //        returnMessage.Append("<li>" + nes + "</li>");
            //    }
            //    returnMessage.Append("</ul></p>");
            //}
            if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
            {
                string[] namesEntityUnSuccessful = nameEntitiesChangesUnsuccessful.Split(':');
                returnMessage.Append("<br/><div  style='color:Red;text-indent:30px>The status of these industries could not be changed successfully:");
                returnMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntityUnSuccessful)
                {
                    returnMessage.Append("<li>" + neus + "</li>");
                }
                returnMessage.Append("</ul></p></div>");
            }
            return Content(returnMessage.ToString());
        }
        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Industry entity = IndustryService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }
        #endregion
    }
}
