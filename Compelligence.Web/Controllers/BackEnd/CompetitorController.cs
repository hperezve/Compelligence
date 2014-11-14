using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Text.RegularExpressions;

using Resources;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Common.Utility;
using Compelligence.Web.Models.Web;
using System.Text;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility.Web;
using Compelligence.Util.Type;
using System.Globalization;
using System.IO;
using System.Net;
using System.Drawing;
using Compelligence.Common.Utility.Upload;
using Compelligence.Common.Utility.Parser;
using System.Configuration;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class CompetitorController : BackEndAsyncFormController<Competitor, decimal>
    {

        #region Public Properties
        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        public ICompetitorService CompetitorService
        {
            get { return (ICompetitorService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IEmailService EmailService { get; set; }

        public IMarketTypeService MarketTypeService { get; set; }
        
        public ICustomFieldService CustomFieldService { get; set; }

        public IFileService FileService { get; set; }

        public ILibraryService LibraryService { get; set; }

        public IClientCompanyService ClientCompanyService { get; set; }

        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }
        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Competitor competitor, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];

             string dupName = formCollection["dupName"];
             if (!competitor.Name.Equals(dupName))
             {
                 if (CompetitorService.GetByNameCompetitor(competitor.Name, CurrentCompany) > 0)
                 {
                     ValidationDictionary.AddError("Name", LabelResource.CompetitorErrorNameExist);
                 }
             }
            if (Validator.IsBlankOrNull(competitor.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.CompetitorNameRequiredError);
            }

            if (Validator.IsBlankOrNull(competitor.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.CompetitorOwnerIdRequiredError);
            }

            if (!Validator.IsBlankOrNull(competitor.BudgetFrm) && !Validator.IsDecimal(competitor.BudgetFrm))
            {
                ValidationDictionary.AddError("BudgetFrm", LabelResource.CompetitorBudgetFormatError);
            }
            if ((competitor.Status.Equals(CompetitorStatus.Disabled)) && (CompetitorService.HassDependences(competitor, clientCompany)))
            {
                ValidationDictionary.AddError("Status", LabelResource.CompetitorStatusValueError);
            }
            if (!Validator.IsBlankOrNull(competitor.Website) && !Validator.IsUrl(competitor.Website))
            {
                ValidationDictionary.AddError("Website", LabelResource.GlobalUrlFormatError);
            }
            if (!Validator.NumberFaxAndPhone(competitor.Phone))
            {
                ValidationDictionary.AddError("Phone", LabelResource.ValidateTextPhone);
            }
            if (!Validator.NumberFaxAndPhone(competitor.Fax))
            {
                ValidationDictionary.AddError("Fax", LabelResource.ValidateTextFax);
            }
            if (!string.IsNullOrEmpty(competitor.ImageUrl))
            {
                if (competitor.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                {
                    byte[] imageData = ResizeImage.GetBytesFromUrl(competitor.ImageUrl);
                    if (imageData != null)
                    {
                        string[] urlObjects = competitor.ImageUrl.Split('/');

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
                            competitor.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");

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
            IList<ResourceObject> statusList = ResourceService.GetAll<CompetitorStatus>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<ResourceObject> tierList = ResourceService.GetAll<CompetitorTier>();
            IList<MarketType> marketTypeList = MarketTypeService.GetAllSortByClientCompany("Name", clientCompany);
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["TierList"] = new SelectList(tierList, "Id", "Value");
            ViewData["MarketTypeList"] = new SelectList(marketTypeList, "Id", "Name");
            IList<CustomField> customfieldlist = CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Competitor, CurrentCompany);
            if (customfieldlist == null)
                customfieldlist = new List<CustomField>();
            ViewData["CustomFieldList"] = customfieldlist;
            ViewData["ShowSubTab"] = "false";
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Competitor;

            switch (detailType)
            {
                case DetailType.Employee:
                    AddFilter(detailFilter, "Employee.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Employee.CompanyType", CompanyType.Competitor);
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyType", CompanyType.Competitor);
                    childController = "Employee";
                    break;
                case DetailType.Industry:
                    AddFilter(detailFilter, "Industry.CompetitorId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "IndustryCompetitorDetailView.CompetitorId", parentId.ToString());
                    childController = "Industry:IndustryCompetitorDetail";
                    break;
                case DetailType.Product:
                    AddFilter(detailFilter, "Product.CompetitorId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProductCompetitorDetailView.CompetitorId", parentId.ToString());
                    childController = "Product:ProductCompetitorDetail";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Competitor);
                    childController = "User";
                    break;
                //EndUSer
                //case DetailType.Budget:
                //    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                //    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Competitor);
                //    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                //    childController = "Budget";
                //    break;
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                case DetailType.Location:
                    AddFilter(detailFilter, "Location.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Location.CompanyType", CompanyType.Competitor);
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyType", CompanyType.Competitor);
                    childController = "Location";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Competitor);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Feedback:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Competitor);
                    childController = "ForumFeedback";
                    break;
                case DetailType.Customer:
                    AddFilter(detailFilter, "Customer.CompetitorId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CustomerCompetitorDetailView.CompetitorId", parentId.ToString());
                    childController = "Customer:CustomerCompetitorDetail";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Competitor);
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
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
                //case DetailType.Metric:
                //    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                //    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Competitor);
                //    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                //    childController = "Metric";
                //    break;
                case DetailType.Label:
                    AddFilter(detailFilter, "Label.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Label.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "LabelDetailView.EntityId", parentId.ToString());
                    childController = "Label";
                    break;
                case DetailType.CompetitorPartner://single relations
                    AddFilter(detailFilter, "CompetitorPartner.CompetitorId", parentId.ToString());
                    AddFilter(browseDetailFilter, "PartnerCompetitorDetailView.CompetitorId", parentId.ToString());
                    childController = "CompetitorPartner:PartnerCompetitorDetail";
                    break;
                case DetailType.LibraryNews:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "LibraryNewsDetailView.EntityId", Compelligence.Common.Browse.BrowseFilter.Operator.Cn, parentId.ToString());
                    childController = "LibraryNews:LibraryNewsDetail";
                    break;
                case DetailType.ProductFamily:
                    AddFilter(detailFilter, "ProductFamily.CompetitorId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProductFamilyAllView.CompetitorId", parentId.ToString());
                    childController = "ProductFamily";
                    break;
                case DetailType.CompetitorSupplier:
                    AddFilter(detailFilter, "CompetitorSupplier.CompetitorId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorSupplierDetailView.CompetitorId", parentId.ToString());
                    childController = "CompetitorSupplier:CompetitorSupplierDetail";
                    break;
                case DetailType.CompetitorCompetitor:
                    AddFilter(detailFilter, "CompetitorCompetitor.CompetitorId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorCompetitorDetailView.CompetitorId", parentId.ToString());
                    childController = "CompetitorCompetitor:CompetitorCompetitorDetail";
                    break;
                case DetailType.Threat:
                    AddFilter(detailFilter, "Threat.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Threat.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "ThreatDetailView.EntityId", parentId.ToString());
                    childController = "Threat";
                    break;
                case DetailType.CompetitorFinancial:
                    AddFilter(detailFilter, "CompetitorFinancial.CompetitorId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorFinancialDetailView.CompetitorId", parentId.ToString());
                    childController = "CompetitorFinancial";
                    break;
                case DetailType.Positioning:
                    AddFilter(detailFilter, "Positioning.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Positioning.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "PositioningDetailView.EntityId", parentId.ToString());
                    childController = "Positioning";
                    break;
                case DetailType.CompetitiveMessaging:
                    AddFilter(detailFilter, "Positioning.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Positioning.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "CompetitiveMessagingDetailView.EntityId", parentId.ToString());
                    childController = "CompetitiveMessaging";
                    break;
                case DetailType.StrengthWeakness:
                    AddFilter(detailFilter, "StrengthWeakness.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "StrengthWeakness.EntityType", DomainObjectType.Competitor);
                    AddFilter(browseDetailFilter, "StrengthWeaknessDetailView.EntityId", parentId.ToString());
                    childController = "StrengthWeakness";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Competitor competitor)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;
            if (CompetitorService.HasAccessToCompetitor(competitor, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void SetEntityDataToForm(Competitor competitor)
        {
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(competitor.MetaData);
            ViewData["BudgetFrm"] = FormatUtility.GetFormatValue("{0:0.##}", competitor.Budget);
            competitor.OriginalStatus = competitor.Status;
            competitor.OldName = competitor.Name;
            IList<CustomField> fields = CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Competitor, CurrentCompany);
            competitor = CompetitorService.GetDynamicFieldsById(competitor, fields);
            ClientCompany cc = ClientCompanyService.GetById(CurrentCompany);
            if (cc != null)
            {
                if (competitor.Name.ToUpper().Equals(cc.Name.ToUpper()) || competitor.Name.ToUpper().Equals(cc.Dns.ToUpper()))
                {
                    ViewData["ShowSubTab"] = "true";
                }
            }
        }

        protected override void GetFormData(Competitor competitor, FormCollection collection)
        {
            competitor.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
            competitor.Budget = FormatUtility.GetDecimalValue(collection["BudgetFrm"]);
            IList<CustomField> fields = CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Competitor, CurrentCompany);
            foreach (CustomField field in fields)
            {
                String value = collection[field.PhysicalName] ?? string.Empty;
                competitor.CustomFields.Add(field.PhysicalName, value);
            }

            competitor.ImageUrl = GetNewUrlToImage(competitor.ImageUrl, ContextFilePath, FileUploadType.Image); 
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

        protected override void SetDefaultEntityDataForSave(Competitor competitor)
        {
            competitor.MetaData = competitor.Name + ":" + competitor.MetaData;
            if (string.IsNullOrEmpty(competitor.KeyWord))
            {
                competitor.KeyWord = competitor.Name;
            }
        }
        
        protected override void SetFormEntityDataToForm(Competitor competitor)
        {
            competitor.OldName = competitor.Name;
            competitor.MetaData = FormFieldsUtility.GetMultilineValue(competitor.MetaData);
            competitor.OriginalStatus = competitor.Status;
            ModelState.SetModelValue("MetaData", new ValueProviderResult(competitor.MetaData, competitor.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(competitor.OldName, competitor.OldName, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(competitor.OriginalStatus, competitor.OriginalStatus, CultureInfo.InvariantCulture));
        }

        public override ActionResult CreateDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');

            string headerType = (string)Request["HeaderType"];
            if (headerType.Equals(DomainObjectType.Objective) || headerType.Equals(DomainObjectType.Team) || headerType.Equals(DomainObjectType.Industry) || headerType.Equals(DomainObjectType.Competitor) || headerType.Equals(DomainObjectType.Product) || headerType.Equals(DomainObjectType.Customer) || headerType.Equals(DomainObjectType.Trend))
            {

                string detailTypeParam = Request["DetailCreateType"];
                //IList<T> newEntities = new List<T>();
                IList<Competitor> newEntities = new List<Competitor>();
                DetailCreateType detailType = DetailCreateType.Override;

                if (!string.IsNullOrEmpty(detailTypeParam))
                {
                    detailType = (DetailCreateType)Enum.ToObject(typeof(DetailCreateType), Convert.ToInt32(detailTypeParam));
                }

                foreach (object id in ids)
                {
                    if (!string.IsNullOrEmpty(id as string))
                    {
                        //T entity = GenericService.GetById((IdT)Convert.ChangeType(id, typeof(IdT)));
                        Competitor entity = CompetitorService.GetById((Decimal)Convert.ChangeType(id, typeof(Decimal)));
                        if (entity != null)
                        {
                            switch (detailType)
                            {
                                case DetailCreateType.Clone:

                                    Competitor entityClone = GenericService.GetEntityClone(entity);

                                    SetDefaultDataFromRequest(entityClone);

                                    SetDetailFilterData(entityClone);

                                    SetDefaultDataForSave(entityClone);

                                    newEntities.Add(entityClone);

                                    break;

                                default:

                                    SetDefaultDataFromRequest(entity);

                                    SetDetailFilterData(entity);

                                    SetDefaultDataForUpdate(entity);

                                    newEntities.Add(entity);

                                    break;
                            }
                        }
                    }
                }

                switch (detailType)
                {
                    case DetailCreateType.Clone:
                        GenericService.SaveCollection(newEntities);
                        break;
                    default:
                        GenericService.UpdateCollection(newEntities);
                        break;
                }

                return null;
            }
            else
            {
                string detailTypeParam = Request["DetailCreateType"];
                string entityId = GetDetailFilterValue("File.EntityId");
                string entityType = GetDetailFilterValue("File.EntityType");

                string fileNameResult = string.Empty;


                //foreach (string idlibrary in ids)
                //{
                //    Library library = LibraryService.GetById(decimal.Parse(idlibrary));

                //     File file = FileService.GetByEntityId(decimal.Parse(idlibrary), DomainObjectType.Library);
                //    if (file != null) //have file
                //    {
                //        file.EntityId = decimal.Parse(entityId);
                //        file.EntityType = entityType;
                //        String oldPhysicalName = file.PhysicalName;
                //        FileService.SaveNewFileVersion(file, CurrentUser);

                //        System.IO.File.Move(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["LibraryFilePath"], oldPhysicalName),
                //                            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["ContentFilePath"], file.PhysicalName));
                //        LibraryService.Delete(library.Id);
                //    }
                //}
                return Content(" done.!");
            }
        }
        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Competitor entity = CompetitorService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult UpdateEnableStatus()
        {
            string[] ids = StringUtility.CheckNull(Request["Id[]"]).Split(',');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            string nameEntitiesChangeSuccessful = string.Empty;
            string nameEntitiesChangesUnsuccessful = string.Empty;

            foreach (string identifier in ids)
            {
                Competitor competitor = CompetitorService.GetById(decimal.Parse(identifier));
                if (competitor != null)
                {
                    if (!competitor.Status.Equals(CompetitorStatus.Enabled))
                    {
                        competitor.Status = CompetitorStatus.Enabled;
                        SetDefaultDataForUpdate(competitor);
                        CompetitorService.Update(competitor);
                        if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
                        {
                            nameEntitiesChangeSuccessful += ":";
                        }
                        nameEntitiesChangeSuccessful += competitor.Name;
                    }
                    else
                    {
                        string message = string.Empty;
                        if (competitor.Status.Equals(CompetitorStatus.Enabled))
                        {
                            message = LabelResource.CompetitorStatusCurrencyValueError + " " + ResourceService.GetName<CompetitorStatus>(competitor.Status);
                        }
                        else
                        {
                            message = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                        {
                            nameEntitiesChangesUnsuccessful += ":";
                        }
                        nameEntitiesChangesUnsuccessful += competitor.Name + " ( " + message + " )";
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
                returnMessage.Append("<br/><div  style='color:Red;text-indent:30px '>The status of these competitors could not be changed successfully");
                returnMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntityUnSuccessful)
                {
                    returnMessage.Append("<li>" + neus + "</li>");
                }
                returnMessage.Append("</ul></p></div>");
                return Content(returnMessage.ToString());
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult UpdateDisableStatus()
        {
            string[] ids = StringUtility.CheckNull(Request["Id[]"]).Split(',');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            string nameEntitiesChangeSuccessful = string.Empty;
            string nameEntitiesChangesUnsuccessful = string.Empty;
            
            foreach (string identifier in ids)
            {
                Competitor competitor = CompetitorService.GetById(decimal.Parse(identifier));
                if (competitor != null)
                {
                    if (!competitor.Status.Equals(CompetitorStatus.Disabled) && (!CompetitorService.HassDependences(competitor, CurrentCompany)))
                    {
                        competitor.Status = CompetitorStatus.Disabled;
                        SetDefaultDataForUpdate(competitor);
                        CompetitorService.Update(competitor);
                        if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
                        {
                            nameEntitiesChangeSuccessful += ":";
                        }
                        nameEntitiesChangeSuccessful += competitor.Name;
                    }
                    else
                    {
                        string message = string.Empty;
                        if (competitor.Status.Equals(CompetitorStatus.Disabled))
                        {
                            message = LabelResource.CompetitorStatusCurrencyValueError + " " + ResourceService.GetName<CompetitorStatus>(competitor.Status);
                        }
                        else if (CompetitorService.HassDependences(competitor, CurrentCompany))
                        {
                            message = LabelResource.CompetitorStatusValueError;
                        }
                        else
                        {
                            message = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                        {
                            nameEntitiesChangesUnsuccessful += ":";
                        }
                        nameEntitiesChangesUnsuccessful += competitor.Name + " ( " + message + " )";
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
                returnMessage.Append("<br/><div  style='color:Red;text-indent:30px '>The status of these competitors could not be changed successfully:");
                returnMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntityUnSuccessful)
                {
                    returnMessage.Append("<li >" + neus + "</li>");
                }
                returnMessage.Append("</ul></p></div>");
                return Content(returnMessage.ToString());
            }
            return null;
        }

        #endregion

        protected override void SetDetailFormData()
        {
            ViewData["HasRows"] = false;
            string headerType = Request["HeaderType"];
            string XId = null;
            decimal count = 0;

            if (!string.IsNullOrEmpty(headerType))
            {
                if (headerType.Equals(DomainObjectType.Industry))
                    XId = GetDetailFilterValue("Competitor.IndustryId");

                else if (headerType.Equals(DomainObjectType.Competitor))
                    XId = GetDetailFilterValue("Competitor.CompetitorId");

                else if (headerType.Equals(DomainObjectType.Customer))
                    XId = GetDetailFilterValue("Competitor.CustomerId");

                else if (headerType.Equals(DomainObjectType.Trend))
                    XId = GetDetailFilterValue("Competitor.TrendId");

                else if (headerType.Equals(DomainObjectType.Team))
                    XId = GetDetailFilterValue("Competitor.TeamId");

                if (!string.IsNullOrEmpty(XId))
                {
                    decimal xId = decimal.Parse(XId);

                    if (headerType.Equals(DomainObjectType.Industry))
                    {
                        IList<CompetitorIndustryDetailView> xByIndustry = CompetitorService.GetByIndustryIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Competitor))
                    {
                        IList<CompetitorCompetitorDetailView> xByIndustry = CompetitorService.GetByCompetitorIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Customer))
                    {
                        IList<CompetitorCustomerDetailView> xByIndustry = CompetitorService.GetByCustomerIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Trend))
                    {
                        IList<CompetitorTrendDetailView> xByIndustry = CompetitorService.GetByTrendIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Team))
                    {
                        IList<CompetitorTeamDetailView> xByIndustry = CompetitorService.GetByTeamIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    if (count > 0)
                    {
                        ViewData["HasRows"] = true;
                    }
                }
            }
        }
    }
}
