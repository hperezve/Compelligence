using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using System.Text.RegularExpressions;
using Spring.Globalization;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Web.Models.Web;
using System.Text;
using System.Configuration;
using Compelligence.Common.Utility;
using Compelligence.Common.Utility.Web;
using Compelligence.Util.Common;
using System.Threading;
using System.Net;
using Compelligence.Web.Models.Util;
using System.Web.Configuration;
using System.Globalization;
using Compelligence.Util.Type;

using System.Drawing;
using Compelligence.Common.Utility.Upload;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity.Views;
using System.IO;

namespace Compelligence.Web.Controllers
{
    public class ProductController : BackEndAsyncFormController<Product, decimal>
    {

        #region Public Properties
        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        public IProductService ProductService
        {
            get { return (IProductService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductFamilyService ProductFamilyService { get; set; }

        public IMarketTypeService MarketTypeService { get; set; }

        public ICustomFieldService CustomFieldService { get; set; }

        public IIndustryProductService IndustryProductService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public IFileService FileService { get; set; }

        public IForumService ForumService { get; set; }

        public ILibraryService LibraryService { get; set; }

        public IForumResponseService ForumResponseService { get; set; }

        public IQuizService QuizService { get; set; }

        public IEmailService EmailService { get; set; }

        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }

        public IPriceService PriceService { get; set; }
        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Product product, FormCollection formCollection)
        {
            product.ImageurlOriginal = product.ImageUrl;
            string clientCompany = (string)Session["ClientCompany"];
             //string dupName = formCollection["dupName"];

             if (ValidationDictionary.IsValid == false)
             {
                 ValidationDictionary.AddError("ListPrice", LabelResource.ProductListPriceRequiredError);
             }
            
             //if (!product.Name.Equals(dupName))
             //{
                 if (ProductService.GetByNameProduct(product.Name, CurrentCompany) > 0)
                 {
                     ValidationDictionary.AddError("Name", LabelResource.ProductErrorNameExist);
                 }
             //}
            if (Validator.IsBlankOrNull(product.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ProductNameRequiredError);
            }

            if (Validator.IsBlankOrNull(product.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.ProductOwnerIdRequiredError);
            }

            if (product.Status.Equals(ProductStatus.Disabled))
            {
                string messageObject = ProductService.HassDependences(product, clientCompany);

                if (messageObject != null)
                {
                    ValidationDictionary.AddError("Status", string.Format(LabelResource.ProductStatusValueError, messageObject));
                }

            }
            if (!string.IsNullOrEmpty(product.ProductFamilyName) && product.CompetitorId==null)
            {
                ValidationDictionary.AddError("ProductFamily", LabelResource.ProductFamilyValueRelationError);
            }
            if (Validator.IsBlankOrNull(product.CompetitorId))
            {
                ValidationDictionary.AddError("CompetitorId", LabelResource.ProductCompetitorIdRequiredError);
            }

            //if (Validator.IsBlankOrNull(product.ListPrice))
            //{
            //    ValidationDictionary.AddError("ListPrice", LabelResource.ProductListPriceRequiredError);
            //}

            //if (!string.IsNullOrEmpty(product.KeyWord))
            //{
            //    if (!ValidateKeyWord(product.KeyWord))
            //    {
            //        ValidationDictionary.AddError("KeyWord", LabelResource.ProductKeyWordValueError);
            //    }
            //}

            if (!Validator.IsBlankOrNull(product.Url) && !Validator.IsValidUrl(product.Url))
            {
                ValidationDictionary.AddError("Url", LabelResource.GlobalUrlFormatError);
            }
            //if (!Validator.IsBlankOrNull(product.ImageUrl) && !Validator.IsUrl(product.ImageUrl))
            //{
            //    ValidationDictionary.AddError("ImageUrl", LabelResource.ImageValueError);
            //}

            if (!string.IsNullOrEmpty(product.Description))
            {
                if (product.Description.Length > 3000)
                {
                    ValidationDictionary.AddError("Description", LabelResource.ProductDescriptionLongError);
                }
            }
            ViewData["imageurlOriginal"] = string.Empty;
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                ViewData["imageurlOriginal"] = product.ImageUrl;
                if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                {
                    byte[] imageData = ResizeImage.GetBytesFromUrl(product.ImageUrl);
                    if (imageData != null)
                    {
                        string[] urlObjects = product.ImageUrl.Split('/');

                        Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                        System.Drawing.Image newImage = ResizeImage.GetResizeStream(imageData, 170, 170);

                        if (newImage != null)
                        {

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

        protected override bool ValidateEditFormData(Product product, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (ValidationDictionary.IsValid == false)
            {
                ValidationDictionary.AddError("ListPrice", LabelResource.ProductListPriceRequiredError);
            }
            if (ProductService.GetAllNameProductAndId(product.Id, product.Name, CurrentCompany) > 0)
            {
                ValidationDictionary.AddError("Name", LabelResource.ProductErrorNameExist);
            }
            if (Validator.IsBlankOrNull(product.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ProductNameRequiredError);
            }
            //if (Validator.IsBlankOrNull(product.ListPrice))
            //{
            //    ValidationDictionary.AddError("ListPrice", LabelResource.ProductListPriceRequiredError);
            //}
            if (Validator.IsBlankOrNull(product.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.ProductOwnerIdRequiredError);
            }

            if (product.Status.Equals(ProductStatus.Disabled))
            {
                string messageObject = ProductService.HassDependences(product, clientCompany);

                if (messageObject != null)
                {                 
                    ValidationDictionary.AddError("Status", string.Format(LabelResource.ProductStatusValueError, messageObject));
                }
            }
            if (!string.IsNullOrEmpty(product.ProductFamilyName) && product.CompetitorId == null)
            {
                ValidationDictionary.AddError("ProductFamily", LabelResource.ProductFamilyValueRelationError);
            }
            if (Validator.IsBlankOrNull(product.CompetitorId))
            {
                ValidationDictionary.AddError("CompetitorId", LabelResource.ProductCompetitorIdRequiredError);
            }
            //if (!string.IsNullOrEmpty(product.KeyWord))
            //{
            //    if (!ValidateKeyWord(product.KeyWord))
            //    {
            //        ValidationDictionary.AddError("KeyWord", LabelResource.ProductKeyWordValueError);
            //    }
            //}

            if (!Validator.IsBlankOrNull(product.Url) && !Validator.IsValidUrl(product.Url))
            {
                ValidationDictionary.AddError("Url", LabelResource.GlobalUrlFormatError);
            }

            if (!string.IsNullOrEmpty(product.Description))
            {
                if (product.Description.Length > 3000)
                {
                    ValidationDictionary.AddError("Description", LabelResource.ProductDescriptionLongError);
                }
            }
            ViewData["imageurlOriginal"] = string.Empty;
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                ViewData["imageurlOriginal"] = product.ImageUrl;
                
                if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                {
                    byte[] imageData = ResizeImage.GetBytesFromUrl(product.ImageUrl);
                    if (imageData != null)
                    {
                        string[] urlObjects = product.ImageUrl.Split('/');

                        Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                        System.Drawing.Image newImage = ResizeImage.GetResizeStream(imageData, 170, 170);
                      
                        if (newImage != null)
                        {

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
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<ResourceObject> statusList = ResourceService.GetAll<ProductStatusWithoutIndustry>();
            IList<ResourceObject> statusListWithIndustry = ResourceService.GetAll<ProductStatus>();
            IList<ResourceObject> tierList = ResourceService.GetAll<ProductTier>();
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            ViewData["StatusListByIndustry"] = new SelectList(statusListWithIndustry, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");

            foreach (ResourceObject o in tierList)
            {
                o.Value = GeneralStringParser.InsertBlank(o.Value);

            }

            ViewData["TierList"] = new SelectList(tierList, "Id", "Value");
            IList<ResourceObject> budgetTypeFinancial = ResourceService.GetAll<BudgetTypeFinancial>();
            ViewData["CurrencyList"] = new SelectList(budgetTypeFinancial, "Id", "Value");
            string competitorIdValue = GetDetailFilterValue("Product.CompetitorId");
            if (Request["HeaderType"] == null || !Request["HeaderType"].Equals(DomainObjectType.Competitor))
            {
                IList<Competitor> competitorList = CompetitorService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["CompetitorIdList"] = new SelectList(competitorList, "Id", "Name");
            }
            else
            {
                if (Request["HeaderType"].Equals(DomainObjectType.Competitor))
                {
                    IList<Competitor> competitorList = CompetitorService.GetAllSortByClientCompany("Name", clientCompany);
                    ViewData["CompetitorIdList"] = new SelectList(competitorList, "Id", "Name", Convert.ToDecimal(competitorIdValue));
                }
            }

            IList<CustomField> customfieldlist=CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Product,CurrentCompany);
            if (customfieldlist == null)
                customfieldlist = new List<CustomField>();
            ViewData["CustomFieldList"] = customfieldlist;
            ViewData["ShowSubTab"] = "false";
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Product;

            switch (detailType)
            {
                case DetailType.Competitor:
                    AddFilter(detailFilter, "Competitor.ProductId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorProductDetailView.ProductId", parentId.ToString());
                    childController = "Competitor:CompetitorProductDetail";
                    break;
                case DetailType.Industry:
                    AddFilter(detailFilter, "Industry.ProductId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "IndustryProductDetailView.ProductId", parentId.ToString());
                    childController = "Industry:IndustryProductDetail";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Product);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Product);
                    childController = "User";
                    break;
                //EndUSer
                //case DetailType.Budget:
                //    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                //    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Product);
                //    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                //    childController = "Budget";
                //    break;
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                //case DetailType.Metric:
                //    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                //    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Product);
                //    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                //    childController = "Metric";
                //    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Customer:
                    AddFilter(detailFilter, "Customer.ProductId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CustomerProductDetailView.ProductId", parentId.ToString());
                    childController = "Customer:CustomerProductDetail";
                    break;
                case DetailType.LibraryNews:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "LibraryNewsDetailView.EntityId", Compelligence.Common.Browse.BrowseFilter.Operator.Cn, parentId.ToString());
                    childController = "LibraryNews:LibraryNewsDetail";
                    break;
                case DetailType.Feedback:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Product);
                    childController = "ForumFeedback";
                    break;
                case DetailType.MarketType:
                    AddFilter(detailFilter, "MarketType.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "MarketType.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "MarketTypeDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "MarketTypeDetailView.EntityType", DomainObjectType.Product);
                    childController = "MarketType";
                    break;
                case DetailType.Price:
                    AddFilter(detailFilter, "Price.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Price.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "PriceDetailView.EntityId", parentId.ToString());
                    //AddFilter(browseDetailFilter, "PriceDetailView.EntityType", DomainObjectType.Product);
                    childController= "Price";
                    break;
                case DetailType.Positioning:
                    AddFilter(detailFilter, "Positioning.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Positioning.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "PositioningDetailView.EntityId", parentId.ToString());
                    //childController = "Positioning:PositioningDetail";
                    childController = "Positioning";
                    break;
                case DetailType.CompetitiveMessaging:
                    AddFilter(detailFilter, "Positioning.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Positioning.EntityType", DomainObjectType.Product);
                    AddFilter(browseDetailFilter, "CompetitiveMessagingDetailView.EntityId", parentId.ToString());
                    //childController = "Positioning:PositioningDetail";
                    childController = "CompetitiveMessaging";
                    break;
                case DetailType.Comment:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Product);
                    childController = "ForumComment";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Product product)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (ProductService.HasAccessToProduct(product, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void SetEntityDataToForm(Product product)
        {
            Session["ProductId"] = product.Id;
            ViewData["DateOfLaunchFrm"] = DateTimeUtility.ConvertToString(product.DateOfLaunch, GetFormatDate());
            ViewData["EndOfLifeFrm"] = DateTimeUtility.ConvertToString(product.EndOfLife, GetFormatDate());
            ViewData["TransitionProduct"] = product.CompetitorId == null ? new List<Product>() : ProductService.GetByCompetitor((decimal)product.CompetitorId);
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(product.MetaData);
            product.OldName = product.Name;
            product.OldDateOfLaunch = product.DateOfLaunch;
            product.OldStatus = product.Status;
            product.OldCompetitorIdStr = product.CompetitorId.ToString();
            IList<ResourceObject> budgetTypeFinancial = ResourceService.GetAll<BudgetTypeFinancial>();
            ViewData["CurrencyList"] = new SelectList(budgetTypeFinancial, "Id", "Value");
            if (product.ProductFamilyId != null)
            {
                ProductFamily pf = ProductService.GetProductFamilyById((decimal)product.ProductFamilyId);
                if (pf != null)
                    product.ProductFamilyName = pf.Name;
            }

            if (product.ProductFamilyId != null)
            {
                decimal idProductFamily = (decimal)product.ProductFamilyId;
                ProductFamily family = ProductFamilyService.GetById(idProductFamily);
                if (family != null)
                {
                    product.ProductFamilyName = family.Name;
                }
            }
            IList<IndustryProduct> industryProductList = IndustryProductService.GetByProductId(product.Id, CurrentCompany);
            if ((industryProductList != null) && industryProductList.Count > 0)
            {
                IList<ResourceObject> statusList = ResourceService.GetAll<ProductStatus>();
                ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            }
            else
            {
                IList<ResourceObject> statusListWithoutIndustry = ResourceService.GetAll<ProductStatusWithoutIndustry>();
                ViewData["StatusList"] = new SelectList(statusListWithoutIndustry, "Id", "Value");
            }
            IList<CustomField> fields=CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Product,CurrentCompany);
            product = ProductService.GetDynamicFieldsById(product, fields);
            IList<Price> priceList = PriceService.GetListByEntityIdAndEntityType(product.Id, DomainObjectType.Product, CurrentCompany);
            if (priceList != null && priceList.Count > 0)
            {
                product.PriceUnits = ResourceService.GetName<PriceUnits>(priceList[0].Units);
            }
            if (ProductService.HaveCompetitorClient(product.Id, CurrentCompany))
            {
                ViewData["ShowSubTab"] = "true";
            }

        }

        protected override void GetFormData(Product product, FormCollection collection)
        {
            product.ImageurlOriginal =ViewData["imageurlOriginal"].ToString();
            //product.OldDateOfLaunch = product.DateOfLaunch;
            product.DateOfLaunch = DateTimeUtility.ConvertToDate(collection["DateOfLaunchFrm"], GetFormatDate());
            product.EndOfLife = DateTimeUtility.ConvertToDate(collection["EndOfLifeFrm"], GetFormatDate());
            //product.OldStatus = product.Status;
            product.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
            if (!string.IsNullOrEmpty(product.ProductFamilyName))
            {
                ProductFamily pf = ProductService.GetByProductFamilyName(product.ProductFamilyName, (decimal)product.CompetitorId, CurrentCompany);
                if (pf == null)
                {
                    pf = new ProductFamily();
                    pf.CompetitorId = product.CompetitorId;
                    pf.Name = product.ProductFamilyName;
                    pf.Status = ProductFamilyStatus.Enabled;
                    SetDefaultDataForSave(pf);
                    ProductFamilyService.Save(pf);
                    product.ProductFamilyId = pf.Id;
                }
                else
                {
                    if (product.ProductFamilyId == null)
                    {
                        product.ProductFamilyId = pf.Id;
                    }
                    else 
                    {
                        if (product.ProductFamilyId != pf.Id)
                        {
                            product.ProductFamilyId = pf.Id;
                        }
                    }
                }
            }
            IList<CustomField> fields = CustomFieldService.GetByEntityTypeId(CustomFieldEntityType.Product, CurrentCompany);
            foreach (CustomField field in fields)
            {
                String value = collection[field.PhysicalName] ?? string.Empty;
                product.CustomFields.Add(field.PhysicalName, value);
            }
            string idProductFamily = collection["ProductFamilyId"];

            //if (!string.IsNullOrEmpty(product.ImageUrl))
            //{
            //    if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
            //    {
            //        byte[] imageData = ResizeImage.GetBytesFromUrl(product.ImageUrl);
            //        if (imageData != null)
            //        {
            //            MemoryStream stream = new MemoryStream(imageData);
            //            try{ Image fullsizeImage = Image.FromStream(stream);
            //            stream.Close();

            //            string[] urlObjects = product.ImageUrl.Split('/');

            //            int newWidth = 170;
            //            int newHeight = 170;

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
            //            System.Drawing.Image newImage = ResizeImage.DownloadImageFromUrl(product.ImageUrl.Trim());
                       
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
            //                newFileImage.FileName = newFileImage.FileName.Substring(0, newFileImage.FileName.LastIndexOf('.')) +"."  + newFileImage.FileFormat;
            //            }
            //            decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
            //            string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
            //            string fileNameResult = string.Empty;

            //            SetDefaultDataForSave(newFileImage);
            //            newFileImage.PhysicalName = newPhysicalName;

            //            FileService.Save(newFileImage);
            //            fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
            //            fileNameResult = fileNameResult.Replace("\\\\", "\\");
            //            newImage.Save(fileNameResult);
            //            product.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");
            //            }
            //            catch
            //            {
            //                bool ErrorImageS = true;
            //                ErrorImage(ErrorImageS);
            //            }
            //        }
            //    }
            //}
            product.ImageUrl = GetNewUrlToImage(product.ImageUrl, ContextFilePath, FileUploadType.Image); 
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
        protected override void SetDefaultEntityDataForSave(Product product)
        {
            product.MetaData = product.Name + ":" + product.MetaData;
            if (string.IsNullOrEmpty(product.KeyWord))
            {
                product.KeyWord = product.Name;
            }
        }

        protected override void SetFormEntityDataToForm(Product product)
        {
            product.OldName = product.Name;
            product.OldStatus = product.Status;
            product.OldDateOfLaunch = product.DateOfLaunch;
            product.OldCompetitorIdStr = product.CompetitorId.ToString() ;
            product.MetaData = FormFieldsUtility.GetMultilineValue(product.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(product.MetaData, product.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(product.OldName, product.OldName, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldStatus", new ValueProviderResult(product.OldStatus, product.OldStatus, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldDateOfLaunch", new ValueProviderResult(product.OldDateOfLaunch.ToString(), product.OldDateOfLaunch.ToString(), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorIdStr", new ValueProviderResult(product.OldCompetitorIdStr, product.OldCompetitorIdStr, CultureInfo.InvariantCulture));
            IList<IndustryProduct> industryProductList = IndustryProductService.GetByProductId(product.Id, CurrentCompany);
            if ((industryProductList != null) && industryProductList.Count > 0)
            {
                IList<ResourceObject> statusList = ResourceService.GetAll<ProductStatus>();
                ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            }
            else
            {
                IList<ResourceObject> statusListWithoutIndustry = ResourceService.GetAll<ProductStatusWithoutIndustry>();
                ViewData["StatusList"] = new SelectList(statusListWithoutIndustry, "Id", "Value");
            }
            if (ProductService.HaveCompetitorClient(product.Id, CurrentCompany))
            {
                ViewData["ShowSubTab"] = "true";
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsFamilyByCompetitor(decimal id)
        {

            IList<ProductFamily> pfList = ProductService.GetProductFamilyByCompetitor(id);
            return ControllerUtility.GetSelectOptionsFromGenericList<ProductFamily>(pfList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult  Create(Product entity, FormCollection collection)
        {
 	         return base.Create(entity, collection);
        }


        protected override bool ValidateDeleteData(Product entity, StringBuilder errorMessage)
        {
            string clientCompany = StringUtility.CheckNull((string)Session["ClientCompany"]);
            string messageObject = ProductService.HassDependencesToValidate(entity, clientCompany);

            if (messageObject != null)
            {
                //errorMessage.AppendLine(string.Format(LabelResource.ProductStatusValueDeleteError, messageObject));
                errorMessage.AppendLine(string.Format(LabelResource.ProductDeleteAssignedError, entity.Name, messageObject));
                return false;
            }

            return base.ValidateDeleteData(entity, errorMessage);
        }


        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetProductName(decimal id)
        {
            string result = string.Empty;
            Product prod = ProductService.GetById(id);
            if (prod != null)
                result = prod.Name;
            return Content(result);
        }

        //private bool ValidateKeyWord(string keyWord)
        //{
        //    bool isValid = true;

        //    int posSumSimbol = keyWord.IndexOf('+');
        //    int posResSimbol = keyWord.IndexOf('-');


        //    if (posSumSimbol != -1)
        //    {
        //        if (posSumSimbol == 0)
        //        {
        //            //Error
        //            isValid = false;
        //        }
        //        else
        //        {
        //            if (posResSimbol != -1)
        //            {
        //                if (posSumSimbol > posResSimbol)
        //                {
        //                    isValid = false;
        //                }
        //                else
        //                {
        //                    // preguntar orden
        //                    int posTempoSumSimbol = posSumSimbol;
        //                    int posTempoResSimbol = posResSimbol;
        //                    string tempokeyWord = keyWord;
        //                    while (posTempoResSimbol > posTempoSumSimbol)
        //                    {
        //                        tempokeyWord = tempokeyWord.Substring(posSumSimbol + 1);
        //                        posTempoSumSimbol = tempokeyWord.IndexOf('+');
        //                        if (posTempoResSimbol < posTempoSumSimbol)
        //                        {
        //                            isValid = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (posResSimbol != -1)
        //        {
        //            if (posResSimbol == 0)
        //            { 
        //                //error
        //                isValid = false;
        //            }
        //        }
        //    }
        //    return isValid;
        //}

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Comments(decimal id)
        {
            Product product = ProductService.GetById(id);            
            UserProfile userProfile = UserProfileService.GetById(product.CreatedBy);
            if (userProfile != null)
            {
                product.CreatedBy = userProfile.Name;
            }
            Forum forum = ForumService.GetByEntityId(product.Id, DomainObjectType.Product, ForumType.Comment);
            if (forum != null)
            {
                //if (forum.ViewsCounter == null)
                //    forum.ViewsCounter = 0;
                //forum.ViewsCounter = forum.ViewsCounter + 1;
                //ForumService.Update(forum);
            }
            //IList<Forum> forumCriteria = ForumService.GetByProductCriteriaEntityId(product.Id, DomainObjectType.ProductCriteria, ForumType.Comment);

            ViewData["Libraries"] = LibraryService.GetByEntityId(product.Id, DomainObjectType.Product);
            //if (forumCriteria == null || forumCriteria.Count > 0)
            //{
                ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Product);
            //}
            //else
            //{
            //    if (forum == null) forumCriteria.Add(forum);
            //    ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForums(forumCriteria, DomainObjectType.Product);
            //}
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            SetUserSecurityAccess(product);
            SetLabels();
            ViewData["showResultMessage"] = "false";
            GetDataOfConfiguration(CurrentCompany);
            ///This view data can not set in genericfrontend
            ///this controller use BackEndAsyncFormController then we use SetDataToHelp of GenericController
            /// to set the title we use ViewData["ProductLabel"] with the label to product
            ///SetDataToHelp((string)Session["EditHelp"], ActionFrom.FrontEnd, FrontEndPages.ForumProduct, ViewData["ProductLabel"].ToString()+":Comments");
            SetDataToHelp(ActionFrom.FrontEnd, FrontEndPages.Forum, "Forum");
            return View("Comments", product);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendComment(decimal id, FormCollection form)
        {
            Product product = ProductService.GetById(id);
            ForumResponse forumResponse = new ForumResponse();
            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Product;
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            forumResponse.ParentResponseId = (!string.IsNullOrEmpty(form["ParentResponseId"])) ? Convert.ToDecimal(form["ParentResponseId"]) : 0;
            forumResponse.ClientCompany = CurrentCompany;
            forumResponse.Libraries = GetLibrariesForEntity(product.Id, DomainObjectType.Product, LibraryTypeKeyCode.Product);
            ForumService.SaveForumResponse(forumResponse, ForumType.Comment);
            EmailService.SendCommentEmail(forumResponse.CreatedBy, product.Name, DomainObjectType.Product, id, CurrentUser, forumResponse.Response, CurrentCompany, forumResponse.Libraries);
            ActionHistoryService.RecordActionHistory(id, EntityAction.Commented, DomainObjectType.Product, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);



          
            UserProfile userProfile = UserProfileService.GetById(product.CreatedBy);
            if (userProfile != null)
            {
                product.CreatedBy = userProfile.Name;
            }
            Forum forum = ForumService.GetByEntityId(product.Id, DomainObjectType.Product, ForumType.Comment);
            if (forum != null)
            {
                //if (forum.ViewsCounter == null)
                //    forum.ViewsCounter = 0;
                //forum.ViewsCounter = forum.ViewsCounter + 1;
                //ForumService.Update(forum);
            }
            //IList<Forum> forumCriteria = ForumService.GetByProductCriteriaEntityId(product.Id, DomainObjectType.ProductCriteria, ForumType.Comment);

            ViewData["Libraries"] = LibraryService.GetByEntityId(product.Id, DomainObjectType.Product);
            //if (forumCriteria == null || forumCriteria.Count > 0)
            //{
            ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Product);
            //}
            //else
            //{
            //    if (forum == null) forumCriteria.Add(forum);
            //    ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForums(forumCriteria, DomainObjectType.Product);
            //}
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            SetUserSecurityAccess(product);
            SetLabels();
            ViewData["showResultMessage"] = "true";
            GetDataOfConfiguration(CurrentCompany);
            return View("Comments", product);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveComments(decimal id, decimal forumresponseid)
        {
            Product product = ProductService.GetById(id);
            Forum forum = ForumService.GetByEntityId(product.Id, DomainObjectType.Product, ForumType.Comment);
            ForumResponseService.DeleteCascading(forum.Id, forumresponseid);
            GetDataOfConfiguration(CurrentCompany);
            return Comments(id);
        }

        public ActionResult DownloadExecute(decimal id)
        {
            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string check = StringUtility.CheckNull(Request["chk"]);

            Compelligence.Domain.Entity.File file = FileService.GetByEntityId(id, DomainObjectType.Library);
            if (file == null)
                return Content("NotFound");

            fullpath += file.PhysicalName;

            if (check.ToLower().Equals("true"))
            {
                if ((file != null) && System.IO.File.Exists(fullpath))
                    return Content("Found");
                else
                    return Content("NotFound");
            }
            else
            {
                GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
            }

            return Content(string.Empty);
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
                Product product = ProductService.GetById(decimal.Parse(identifier));
                if (product != null)
                {
                    IList<IndustryProduct> industryProductList = IndustryProductService.GetByProductId(product.Id, CurrentCompany);
                    if (!product.Status.Equals(ProductStatus.Enabled) && (industryProductList != null ) && (industryProductList.Count>0))
                    {
                        product.Status = ProductStatus.Enabled;
                        SetDefaultDataForUpdate(product);
                        ProductService.Update(product);
                        if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
                        {
                            nameEntitiesChangeSuccessful += ":";
                        }
                        nameEntitiesChangeSuccessful += product.Name;
                    }
                    else
                    {
                        string message = string.Empty;
                        if (product.Status.Equals(ProductStatus.Enabled))
                        {
                            message = LabelResource.ProductStatusCurrencyValueError + " " + ResourceService.GetName<ProductStatus>(product.Status);
                        }
                        else if ((industryProductList == null) || (industryProductList.Count <= 0))
                        {
                            message = "This product is not assigned to an industry";
                        }
                        else
                        {
                            message = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                        {
                            nameEntitiesChangesUnsuccessful += ":";
                        }
                        nameEntitiesChangesUnsuccessful += product.Name + " ( " + message + " )";
                    }
                }
            }
            //if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
            //{
            //    string[] namesEntitySuccessful = nameEntitiesChangeSuccessful.Split(':');
            //    returnMessage.Append("The Status of Products was changed successfully");
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
                returnMessage.Append("<br/><div style='color:Red;text-indent:30px>");
                returnMessage.Append("The status of these products could not be changed successfully:");
                returnMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntityUnSuccessful)
                {
                    returnMessage.Append("<li>" + neus + "</li>");
                }
                returnMessage.Append("</ul></p>");
                returnMessage.Append("</div>");
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
            string[] ids = StringUtility.CheckNull(Request["Id[]"]).Split(',');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            string nameEntitiesChangeSuccessful = string.Empty;
            string nameEntitiesChangesUnsuccessful = string.Empty;
            IList<Industry> industriesByProductId = new List<Industry>();
            
            foreach (string identifier in ids)
            {
                Product product = ProductService.GetById(decimal.Parse(identifier));
                if (product != null)
                {
                    industriesByProductId = IndustryService.GetByProductId(product.Id);
                    if (!product.Status.Equals(ProductStatus.Disabled) && (ProductService.HassDependences(product, CurrentCompany) != null) && (industriesByProductId == null || industriesByProductId.Count==0))
                    {
                        product.Status = ProductStatus.Disabled;
                        SetDefaultDataForUpdate(product);
                        ProductService.Update(product);
                        if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
                        {
                            nameEntitiesChangeSuccessful += ":";
                        }
                        nameEntitiesChangeSuccessful += product.Name;
                    }
                    else
                    {
                        string message = string.Empty;
                        string messageByIndustry = string.Empty;
                        string messageObject = ProductService.HassDependences(product, CurrentCompany);

                        if (product.Status.Equals(ProductStatus.Disabled))
                        {
                            message = LabelResource.CompetitorStatusCurrencyValueError + " " + ResourceService.GetName<ProductStatus>(product.Status);
                        }
                        else if (messageObject != null)
                        {
                            message = string.Format(LabelResource.ProductStatusValueError, messageObject);
                        }
                        else
                        {
                            message = string.Empty;
                        }
                        if (industriesByProductId != null && industriesByProductId.Count > 0)
                        {
                            if (industriesByProductId.Count == 1)
                            {
                                messageByIndustry = string.Format(LabelResource.ProductWithIndustryDeleteError, product.Name, industriesByProductId[0].Name);
                            }
                            else
                            {
                                string allNameIndustries = string.Empty;
                                for (int i = 0; i < industriesByProductId.Count; i++ )
                                {
                                    if (!string.IsNullOrEmpty(allNameIndustries))
                                    {
                                        if (i < industriesByProductId.Count - 1)
                                        {
                                            allNameIndustries += ", ";
                                        }
                                        else
                                        {
                                            allNameIndustries += " and ";
                                        }
                                    }
                                    allNameIndustries += industriesByProductId[i].Name;
                                }
                                messageByIndustry =string.Format(LabelResource.ProductWithIndustriesDeleteError, product.Name, allNameIndustries);
                            }
                        }
                        if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                        {
                            nameEntitiesChangesUnsuccessful += ":";
                        }
                        //nameEntitiesChangesUnsuccessful += product.Name + " ( " + message + " )";
                        /////This line to remove the first line
                        //nameEntitiesChangesUnsuccessful = string.Empty;
                        if (!string.IsNullOrEmpty(messageByIndustry))
                        {
                            if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                            {
                                nameEntitiesChangesUnsuccessful += ":";
                            }
                            nameEntitiesChangesUnsuccessful += messageByIndustry;
                        }
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
                returnMessage.Append("<br/><div style='color:Red;text-indent:30px>The Status of Products was not changed successfully");
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
        #endregion

        public override ActionResult CreateDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');

            string headerType = (string)Request["HeaderType"];
            if (headerType.Equals(DomainObjectType.Objective) || headerType.Equals(DomainObjectType.Team)|| headerType .Equals(DomainObjectType.Industry )|| headerType .Equals (DomainObjectType .Competitor )|| headerType .Equals (DomainObjectType .Product )|| headerType .Equals (DomainObjectType .Customer )|| headerType .Equals (DomainObjectType .Trend ))
            {

                string detailTypeParam = Request["DetailCreateType"];
                //IList<T> newEntities = new List<T>();
                IList<Product> newEntities = new List<Product>();
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
                        Product entity = ProductService.GetById((Decimal)Convert.ChangeType(id, typeof(Decimal)));
                        if (entity != null)
                        {
                            switch (detailType)
                            {
                                case DetailCreateType.Clone:

                                    Product entityClone = GenericService.GetEntityClone(entity);

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

        protected override void SetDetailFormData()
        {
            ViewData["HasRows"] = false;
            string headerType = Request["HeaderType"];
            string XId = null;
            decimal count = 0;

            if (!string.IsNullOrEmpty(headerType))
            {
                if (headerType.Equals(DomainObjectType.Industry))
                    XId = GetDetailFilterValue("Product.IndustryId");

                else if(headerType.Equals(DomainObjectType.Competitor))
                    XId = GetDetailFilterValue("Product.CompetitorId");

                else if (headerType.Equals(DomainObjectType.Customer))
                    XId = GetDetailFilterValue("Product.CustomerId");

                else if (headerType.Equals(DomainObjectType.Trend))
                    XId = GetDetailFilterValue("Product.TrendId");

                else if (headerType.Equals(DomainObjectType.Team))
                    XId = GetDetailFilterValue("Product.TeamId");
                
                if (!string.IsNullOrEmpty(XId))
                {
                    decimal xId = decimal.Parse(XId);

                    if (headerType.Equals(DomainObjectType.Industry))
                    {
                        IList<ProductIndustryDetailView> xByIndustry = ProductService.GetByIndustryIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Competitor))
                    {
                        IList<ProductCompetitorDetailView> xByIndustry = ProductService.GetByCompetitorIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Customer))
                    {
                        IList<ProductCustomerDetailView> xByIndustry = ProductService.GetByCustomerIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Trend))
                    {
                        IList<ProductTrendDetailView> xByIndustry = ProductService.GetByTrendIdAndClientCompany(xId, CurrentCompany);
                        if (xByIndustry != null)
                        {
                            count = xByIndustry.Count;
                        }
                    }

                    else if (headerType.Equals(DomainObjectType.Team))
                    {
                        IList<ProductTeamDetailView> xByIndustry = ProductService.GetByTeamIdAndClientCompany(xId, CurrentCompany);
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
