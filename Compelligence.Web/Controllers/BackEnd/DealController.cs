using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using System.Text;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Util;
using Compelligence.Common.Utility.Web;
using System.Globalization;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class DealController : BackEndAsyncFormController<Deal, decimal>
    {

        #region Public Properties

        public IDealService DealService
        {
            get { return (IDealService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IResourceService ResourceService { get; set; }

        public ICustomerService CustomerService { get; set; }

        public IDealCompetitorService DealCompetitorService { get; set; }

        public IDealIndustryService DealIndustryService { get; set; }

        public IDealProductService DealProductService { get; set; }

        public IMarketTypeService MarketTypeService { get; set; }

        public IIndustryProductService IndustryProductService { get; set; }

        public IIndustryCompetitorService IndustryCompetitorService { get; set; }

        public IDealUserProfileService DealUserProfileService { get; set; }

        public IEmployeeService EmployeeService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCompetitorsByIndustry(decimal id)
        {
            IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsByCompetitor(decimal id)
        {
            decimal industryId = Convert.ToDecimal(StringUtility.CheckNull(Request["IndustryId"]));

            IList<Product> productList = ProductService.GetByIndustryAndCompetitor(industryId, id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCustomers()
        {
            IList<Customer> customerList = CustomerService.GetAllActiveByClientCompany(CurrentCompany);
            return ControllerUtility.GetSelectOptionsFromGenericList<Customer>(customerList, "Id", "Name");
        }

        public ActionResult DetailGridAttach()
        {
            decimal entityId = Convert.ToDecimal(GetDetailFilterValue("Deal.DealId"));
            ViewData["EntityId"] = entityId;
            ViewData["Scope"] = Request["Scope"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];

            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["BrowseDetailName"] = Request["BrowseDetailName"];
            ViewData["BrowseDetailFilter"] = Request["BrowseDetailFilter"];
            ViewData["UserSecurityAccess"] = Request["UserSecurityAccess"];
            ViewData["EntityLocked"] = Request["EntityLocked"];

            return View();
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Deal deal, FormCollection formCollection)
        {
            
            string primaryCompetitor = formCollection["PrimaryCompetitor"];

            deal.IndustriesIds = formCollection["IndustryIds"];
            deal.CompetitorsIds = formCollection["CompetitorIds"];
            deal.ProductsIds = formCollection["ProductIds"];

            if (Validator.IsBlankOrNull(deal.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.DealNameRequiredError);
            }
            if (Validator.IsBlankOrNull(deal.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.DealAssignedToRequiredError);
            }
            if (string.IsNullOrEmpty(deal.IndustriesIds))
            {
                ValidationDictionary.AddError("IndustryId", LabelResource.DealIndustryIdRequiredError);
            }
            else
            {
                string[] idsIndustry = deal.IndustriesIds.Split(',');
                if (idsIndustry.Length > 1 && Validator.IsBlankOrNull(deal.PrimaryIndustry))
                {
                    ValidationDictionary.AddError("PrimaryIndustry", LabelResource.DealPrimaryIndustryRequiredError);
                }

            }
            if (deal.CompetitorsIds.Equals(""))
            {
                //ValidationDictionary.AddError("CompetitorIds", LabelResource.DealCompetitorIdRequiredError);
            }
            else
            {
                string[] idsCompetitor = deal.CompetitorsIds.Split(',');
                if (idsCompetitor.Length > 1 && Validator.IsBlankOrNull(deal.PrimaryCompetitor))
                {
                    ValidationDictionary.AddError("PrimaryCompetitor", LabelResource.DealPrimaryCompetitorRequiredError);
                }

            }
            if (Validator.IsBlankOrNull(deal.DueDateFrm))
            {
                ValidationDictionary.AddError("DueDateFrm", LabelResource.DealDueDateRequiredError);
            }
            else
            {
                if (!Validator.IsDate(deal.DueDateFrm, GetFormatDate()))
                    ValidationDictionary.AddError("DueDateFrm", string.Format(LabelResource.DealDueDateFormatError, GetFormatDate()));
            }

            if (deal.CompetitorDiscount.Length > 15)
            {
                ValidationDictionary.AddError("CompetitorDiscount", LabelResource.DealCompetitorDiscountLengthError);
            }

            if (deal.CompetitorAccountStrategy.Length > 2000)
            {
                ValidationDictionary.AddError("CompetitorAccountStrategy", LabelResource.DealAccountStrategyLengthError);
            }

            if (deal.Notes.Length > 2000)
            {
                ValidationDictionary.AddError("Notes", LabelResource.DealNotesLengthError);
            }

            if (deal.Details.Length > 2000)
            {
                ValidationDictionary.AddError("Details", LabelResource.DealDetailsLengthError);
            }

            if (deal.RequestSupported.Length > 2000)
            {
                ValidationDictionary.AddError("RequestSupported", LabelResource.DealRequestSupportedLengthError);
            }

            if (!ValidationDictionary.IsValid)
            {
                SetUsersIds(deal, formCollection);
            }

            //if(Validator.IsDecimal(deal.CurrencyId))
            //{
            //    ValidationDictionary.AddError("CurrencyId", LabelResource.DealCurrencyIdSizeError);
            //}

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void LoadFormData()
        {
            GetDataOfConfiguration(CurrentCompany);
        }

        protected override void SetFormData()
        {
            //MigrateDeals();
            IList<ResourceObject> dealStatusList = ResourceService.GetAll<DealStatus>();
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(CurrentCompany);
            decimal industryListCount = IndustryService.GetAllActiveByClientCompany(CurrentCompany).Count;
            IList<Customer> customerList = CustomerService.GetAllActiveByClientCompany(CurrentCompany);
            IList<ResourceObject> dealPhaseList = ResourceService.GetAllBySortValue<DealPhaseDate>();
            IList<MarketType> marketTypeList = MarketTypeService.GetAllSortByClientCompany("Name", CurrentCompany);
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(CurrentCompany);
            //IList<UserProfile> endUserList = UserProfileService.GetEndUsersWithoutUser(CurrentCompany, CurrentUser);
            IList<UserProfile> alluserslist = UserProfileService.GetAllUsersProfile(CurrentCompany);
            //IList<UserProfile> alluserslist = UserProfileService.GetAllUsersProfile(CurrentCompany);

            ViewData["StatusList"] = new SelectList(dealStatusList, "Id", "Value");
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["IndustryIdMultiListCount"] = industryListCount;
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["CustomerIdList"] = new SelectList(customerList, "Id", "Name");
            ViewData["ProductIdList"] = new SelectList(new int[] { });
            ViewData["PhaseList"] = new SelectList(dealPhaseList, "Id", "Value");
            ViewData["MarketTypeList"] = new SelectList(marketTypeList, "Id", "Name");
            ViewData["SelectedCompetitorIdList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["SelectedIndustryIdList"] = new SelectList(new List<Industry>(), "Id", "Name");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["EndUserIdMultiList"] = new MultiSelectList(alluserslist, "Id", "Name");
            ViewData["EndUserIdMultiListDisabled"] = "false";
            ViewData["EmployeeIdList"] = new MultiSelectList(new List<Employee>(), "Id", "Name");
            ViewData["WinningCompetitorList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["CheckIndustryIds"] = false;
            SetEmployeesByCustmerid(customerList);
        }

        protected override void SetEntityDataToForm(Deal deal)
        {
            if (deal.CustomerId != null)
            {
                Customer customer = CustomerService.GetById(DecimalUtility.CheckNull(deal.CustomerId));
                if (customer != null)
                {
                    if (!string.IsNullOrEmpty(deal.DealEmployeeIds))
                    {
                        string[] selectedEmployeeValues = deal.DealEmployeeIds.Split(',');
                        var selectedEmployees = selectedEmployeeValues;
                        IList<Employee> employeeList = EmployeeService.GetByCompanyAndType(new decimal[] { (decimal)deal.CustomerId }, "CUST", CurrentCompany);
                        ViewData["EmployeeIdList"] = new MultiSelectList(employeeList, "Id", "Name", selectedEmployees);
                    }
                    deal.CustomerName = customer.Name;
                }
            }

            //obtain all industries which are related with this project
            deal.IndustriesIds = GetIndustriesForDeal(deal.Id);
            string[] selectedValues = deal.IndustriesIds.Split(',');
            var selected = selectedValues;
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(CurrentCompany);
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);

            if (CurrentUser.Equals(deal.CreatedBy))
            {
                ViewData["EndUserIdMultiListDisabled"] = "false";
            }
            else
            {
                ViewData["EndUserIdMultiListDisabled"] = "true";
            }

            ViewData["DueDateFrm"] = DateTimeUtility.ConvertToString(deal.DueDate, GetFormatDate());
            ViewData["CreatedDateFrm"] = DateTimeUtility.ConvertToString(deal.CreatedDate, GetFormatDate());
            ViewData["CloseDateFrm"] = DateTimeUtility.ConvertToString(deal.CloseDate, GetFormatDate());
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(deal.MetaData);
            SetCascadingData(deal, true);

            //obtain primary industry
            deal.PrimaryIndustry = GetPrimaryIndustry(deal.Id);

            if (!string.IsNullOrEmpty(deal.IndustriesIds))
            {
                IList<Industry> lstIndustriesSelected = new List<Industry>();
                decimal[] idIndustries = FormatUtility.GetDecimalArrayFromStringValues(deal.IndustriesIds.Split(','));

                lstIndustriesSelected = IndustryService.GetById(idIndustries);

                if (deal.PrimaryIndustry == 0 || deal.PrimaryIndustry == null)
                {
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name");
                }
                else
                {
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name", Convert.ToString(deal.PrimaryIndustry));
                }
            }

            //obtain primary competitor
            deal.PrimaryCompetitor = GetPrimaryCompetitor(deal.Id);

            if (!string.IsNullOrEmpty(deal.CompetitorsIds))
            {
                IList<Competitor> lstCompetitorsSelected = new List<Competitor>();
                decimal[] idCompetitors = FormatUtility.GetDecimalArrayFromStringValues(deal.CompetitorsIds.Split(','));

                lstCompetitorsSelected = CompetitorService.GetById(idCompetitors);

                if (deal.PrimaryCompetitor == 0 || deal.PrimaryCompetitor == null)
                {
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name");
                }
                else
                {
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name", Convert.ToString(deal.PrimaryCompetitor));
                }
            }

            if (!string.IsNullOrEmpty(deal.AssignedTo))
            {
                ViewData["AssignedToFrm"] = UserProfileService.GetById(deal.AssignedTo).Name;
            }

            deal.OldIndustriesIds = deal.IndustriesIds;
            deal.OldCompetitorsIds = deal.CompetitorsIds;
            deal.OldProductsIds = deal.ProductsIds;
            deal.OldName = deal.Name;
            deal.OldPhase = deal.Phase;
            deal.OldPrimaryCompetitor = deal.PrimaryCompetitor;
            deal.OldPrimaryIndustry = deal.PrimaryIndustry;
            deal.OldUsersIds = deal.UsersIds;
            deal.oldDealEmployeeIds = deal.DealEmployeeIds;
        }

        protected override void SetFormEntityDataToForm(Deal deal)
        {

            //obtain all industries which are related with this deal
            if (string.IsNullOrEmpty(deal.IndustriesIds))
            {
                deal.IndustriesIds = GetIndustriesForDeal(deal.Id);
            }
            string[] selected = deal.IndustriesIds.Split(',');
            string checkedbyHierarchy = "";
            IList<IndustryByHierarchyView> industryListByHierarchy = new List<IndustryByHierarchyView>();
            IList<Industry> industryList = new List<Industry>();
            if (deal.CheckIndustryIds)
            {
                if (!deal.ClientCompany.Equals(""))
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy(deal.ClientCompany);
                }
                else
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryListByHierarchy, "Id", "Name", selected);
            }
            else
            {
                if (!deal.ClientCompany.Equals(""))
                {
                    industryList = IndustryService.GetAllActiveByClientCompany(deal.ClientCompany);
                }
                else
                {
                    industryList = IndustryService.GetAllActiveByClientCompany((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);
            }
            ViewData["CheckIndustryIds"] = deal.CheckIndustryIds;
            if (deal.CheckIndustryIds == true)
                checkedbyHierarchy = "true";
            else if (deal.CheckIndustryIds == false)
                checkedbyHierarchy = "false";

            ModelState.SetModelValue("checkedbyHierarchy", new ValueProviderResult(checkedbyHierarchy, checkedbyHierarchy, CultureInfo.InvariantCulture));

            SetCascadingData(deal, false);

            IList<ResourceObject> dealStatusList = ResourceService.GetAll<DealStatus>();
            if (deal.Status != null)
            {
                ViewData["StatusList"] = new SelectList(dealStatusList, "Id", "Value", deal.Status);
            }

            //obtain primary industry
            //deal.PrimaryIndustry = GetPrimaryIndustry(deal.Id);

            if (!string.IsNullOrEmpty(deal.IndustriesIds))
            {
                IList<Industry> lstIndustriesSelected = new List<Industry>();
                decimal[] idIndustries = FormatUtility.GetDecimalArrayFromStringValues(deal.IndustriesIds.Split(','));

                lstIndustriesSelected = IndustryService.GetById(idIndustries);

                if (deal.PrimaryIndustry == 0 || deal.PrimaryIndustry == null)
                {
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name");
                }
                else
                {
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name", deal.PrimaryIndustry);
                }
            }

            //obtain primary competitor
            //deal.PrimaryCompetitor = GetPrimaryCompetitor(deal.Id);
            if (!string.IsNullOrEmpty(deal.CompetitorsIds))
            {
                IList<Competitor> lstCompetitorsSelected = new List<Competitor>();
                decimal[] idCompetitors = FormatUtility.GetDecimalArrayFromStringValues(deal.CompetitorsIds.Split(','));
                lstCompetitorsSelected = CompetitorService.GetById(idCompetitors);

                if (deal.PrimaryCompetitor == 0 || deal.PrimaryCompetitor == null)
                {
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name");
                }
                else
                {
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name", deal.PrimaryCompetitor);
                }
            }
            
            //ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(deal.MetaData);
            deal.MetaData = FormFieldsUtility.GetMultilineValue(deal.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(deal.MetaData, deal.MetaData, CultureInfo.InvariantCulture));
            deal.OldName = deal.Name;
            deal.OldPrimaryCompetitor = deal.PrimaryCompetitor;
            deal.OldPrimaryIndustry = deal.PrimaryIndustry;
            deal.oldDealEmployeeIds = deal.DealEmployeeIds;
            ModelState.SetModelValue("OldName", new ValueProviderResult(deal.OldName, deal.OldName, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(deal.OldIndustriesIds, deal.OldIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorsIds", new ValueProviderResult(deal.OldCompetitorsIds, deal.OldCompetitorsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldProductsIds", new ValueProviderResult(deal.OldProductsIds, deal.OldProductsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldPrimaryCompetitor", new ValueProviderResult(deal.OldPrimaryCompetitor.ToString(), deal.OldPrimaryCompetitor.ToString(), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldPrimaryIndustry", new ValueProviderResult(deal.OldPrimaryIndustry.ToString(), deal.OldPrimaryIndustry.ToString(), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("PrimaryIndustry", new ValueProviderResult(Convert.ToString(deal.PrimaryIndustry), Convert.ToString(deal.PrimaryIndustry), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("PrimaryCompetitor", new ValueProviderResult(Convert.ToString(deal.PrimaryCompetitor), Convert.ToString(deal.PrimaryCompetitor), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("Status", new ValueProviderResult(Convert.ToString(deal.Status), Convert.ToString(deal.Status), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("oldDealEmployeeIds", new ValueProviderResult(deal.oldDealEmployeeIds, deal.oldDealEmployeeIds, CultureInfo.InvariantCulture));
        }

        protected override void GetFormData(Deal deal, FormCollection collection)
        {
            deal.DueDate = DateTimeUtility.ConvertToDate(collection["DueDateFrm"], GetFormatDate());
            deal.CloseDate = deal.DueDate;
            deal.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);

            string selectedIndustries = collection["IndustryIds"];
            string selectedCompetitors = collection["CompetitorIds"];
            string selectedProducts = collection["ProductIds"];
            string primaryCompetitor = collection["PrimaryCompetitor"];
            string winningCompetitor = collection["WinningCompetitor"];
            string selectedEndUsers = collection["UsersIds"];
            string selectedEmployees = collection["DealEmployeeIds"];


            deal.OldIndustriesIds = GetIndustriesForDeal(deal.Id);
            deal.OldCompetitorsIds = GetCompetitorsForDeal(deal.Id);
            deal.OldProductsIds = GetProductsForDeal(deal.Id);
            deal.OldUsersIds = GetEndUserForDeal(deal.Id);

            deal.IndustriesIds = selectedIndustries;
            deal.CompetitorsIds = selectedCompetitors;
            deal.ProductsIds = selectedProducts;
            deal.UsersIds = selectedEndUsers;
            deal.DealEmployeeIds = selectedEmployees;
            deal.WinningCompetitor = winningCompetitor;
            if (!collection["PrimaryCompetitor"].Equals("0") && !collection["PrimaryCompetitor"].Equals(""))
            {
                deal.PrimaryCompetitor = Convert.ToDecimal(primaryCompetitor);
            }
        }

        protected void SetUsersIds(Deal deal, FormCollection collection)
        {
            string selectedEndUsers = collection["UsersIds"];
            deal.UsersIds = selectedEndUsers;
        }

        protected override void SetDefaultEntityDataForSave(Deal deal)
        {
            deal.MetaData = deal.Name + ":" + deal.MetaData;
                

            if (!string.IsNullOrEmpty(deal.IndustriesIds))
            {
                decimal[] idIndustries = FormatUtility.GetDecimalArrayFromStringValues(deal.IndustriesIds.Split(','));
                IList<Industry> industries = IndustryService.GetById(idIndustries);

                if (industries != null)
                {
                    foreach (Industry industry in industries)
                    {
                        deal.MetaData += ":" + industry.Name;
                    }
                }
            }
            if (!string.IsNullOrEmpty(deal.CompetitorsIds))
            {
                decimal[] idCompetitors = FormatUtility.GetDecimalArrayFromStringValues(deal.CompetitorsIds.Split(','));
                IList<Competitor> competitors = CompetitorService.GetById(idCompetitors);

                if (competitors != null)
                {
                    foreach (Competitor competitor in competitors)
                    {
                        deal.MetaData += ":" + competitor.Name;
                    }
                }
            }
            if (!string.IsNullOrEmpty(deal.ProductsIds))
            {
                decimal[] idProducts = FormatUtility.GetDecimalArrayFromStringValues(deal.ProductsIds.Split(','));
                IList<Product> products = ProductService.GetById(idProducts);

                if (products != null)
                {
                    foreach (Product product in products)
                    {
                        deal.MetaData += ":" + product.Name;
                    }
                }
            }
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Deal;

            switch (detailType)
            {
                case DetailType.Library:
                    //AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    //AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Deal);
                    //AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    //childController = "Library";
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Deal);
                    AddFilter(browseDetailFilter, "DealsWithAttachView.EntityId", parentId.ToString());
                    childController = "Deal::DetailGridAttach";
                    break;

                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Deal);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Deal);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Deal);
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Deal);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                case DetailType.Metric:
                    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Deal);
                    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                    childController = "Metric";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Deal);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Comment:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Deal);
                    childController = "ForumComment";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Deal);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Deal);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Deal deal)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (DealService.HasAccessToDeal(deal, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion

        #region Private Methods

        private decimal GetPrimaryCompetitor(decimal idDeal)
        {
            decimal idPrimary = 0;

            DealCompetitor dealCompetitor = DealCompetitorService.GetPrimaryCompetitorByDeal(idDeal);

            if (dealCompetitor != null)
            {
                idPrimary = dealCompetitor.Id.CompetitorId;
            }

            return idPrimary;
        }

        private decimal GetPrimaryIndustry(decimal idDeal)
        {
            decimal idPrimary = 0;
            DealIndustry dealIndustry = DealIndustryService.GetPrimaryIndustryByDeal(idDeal);

            if (dealIndustry != null)
            {
                idPrimary = dealIndustry.Id.IndustryId;
            }

            return idPrimary;
        }

        private void SetCascadingData(Deal deal, bool flag)
        {
            //if (!DecimalUtility.IsBlankOrNull(deal.IndustryId))
            //{
            //    IList<Competitor> competitorList = CompetitorService.GetByIndustryId(deal.IndustryId.Value);

            //    ViewData["CompetitorIdList"] = new SelectList(competitorList, "Id", "Name");

            //    if (!DecimalUtility.IsBlankOrNull(deal.CompetitorId))
            //    {
            //        IList<Product> productList = ProductService.GetByIndustryAndCompetitor(deal.IndustryId.Value, deal.CompetitorId.Value);

            //        ViewData["ProductIdList"] = new SelectList(productList, "Id", "Name");
            //    }
            //}
            if (flag)//flag indicates if we get competitors and products from DB or from a form
            {
                deal.CompetitorsIds = GetCompetitorsForDeal(deal.Id);
                deal.ProductsIds = GetProductsForDeal(deal.Id);
                deal.UsersIds = GetEndUserForDeal(deal.Id);
            }
            string[] selectedProducts = deal.ProductsIds.Split(',');
            string[] selectedCompetitors = deal.CompetitorsIds.Split(',');
            decimal[] idIndustries = null;

            string[] selectedEndUsersValues = deal.UsersIds.Split(',');
            var selectedEndUsers = selectedEndUsersValues;
            IList<UserProfile> alluserslist = UserProfileService.GetAllUsersProfile(CurrentCompany);
            ViewData["EndUserIdMultiList"] = new MultiSelectList(alluserslist, "Id", "Name", selectedEndUsers);
            ModelState.SetModelValue("UsersIdsHidden", new ValueProviderResult(deal.UsersIds, deal.UsersIds, CultureInfo.InvariantCulture));

            IList<Competitor> competitorList = new List<Competitor>();
            IList<Product> productList = new List<Product>();

            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");

            if (!string.IsNullOrEmpty(deal.IndustriesIds))
            {
                idIndustries = FormatUtility.GetDecimalArrayFromStringValues(deal.IndustriesIds.Split(','));
                competitorList = CompetitorService.GetByIndustry(idIndustries);
            }

            if ((selectedCompetitors != null) && (selectedCompetitors.Length > 0))
            {
                decimal[] idCompetitors = FormatUtility.GetDecimalArrayFromStringValues(selectedCompetitors);

                if ((idIndustries != null) && (idIndustries.Length > 0))
                {
                    productList = ProductService.GetByCompetitorAndIndustry(idCompetitors, idIndustries);
                }
                else
                {
                    productList = ProductService.GetByCompetitor(idCompetitors);
                }
            }

            if (productList.Count > 0)
            {
                if (deal.ProductsIds == null)
                {
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name");
                }
                else
                {
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name", selectedProducts);
                }
            }

            if (competitorList.Count > 0)
            {
                if (deal.CompetitorsIds == null)
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                }
                else
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedCompetitors);
                }
            }
            //set Contacts of Customer
            if (deal.CustomerId != null)
            {
                ViewData["EmployeeIdList"] = new MultiSelectList(new List<Employee>(), "Id", "Name");
                Customer customer = CustomerService.GetById(DecimalUtility.CheckNull(deal.CustomerId));
                if (customer != null)
                {
                    if (!string.IsNullOrEmpty(deal.DealEmployeeIds))
                    {
                        string[] selectedEmployeeValues = deal.DealEmployeeIds.Split(',');
                        var selectedEmployees = selectedEmployeeValues;
                        IList<Employee> employeeList = EmployeeService.GetByCompanyAndType(new decimal[] { (decimal)deal.CustomerId }, "CUST", CurrentCompany);
                        ViewData["EmployeeIdList"] = new MultiSelectList(employeeList, "Id", "Name", selectedEmployees);
                    }
                    deal.CustomerName = customer.Name;
                }
            }
        }

        private string GetCompetitorsForDeal(decimal idDeal)
        {
            string ids = null;
            IList<DealCompetitor> lstDealComp = DealCompetitorService.GetByDealId(idDeal);
            int cont = 0;
            foreach (DealCompetitor dealComp in lstDealComp)
            {
                cont++;

                if (lstDealComp.Count == cont)
                {
                    ids = ids + dealComp.Id.CompetitorId.ToString();
                }
                else
                {
                    ids = ids + dealComp.Id.CompetitorId + ",";
                }

            }
            return ids;
        }

        private string GetProductsForDeal(decimal idDeal)
        {
            string ids = null;
            IList<DealProduct> lstDealProd = DealProductService.GetByDealId(idDeal);
            int cont = 0;
            foreach (DealProduct dealProd in lstDealProd)
            {
                cont++;

                if (lstDealProd.Count == cont)
                {
                    ids = ids + dealProd.Id.ProductId.ToString();
                }
                else
                {
                    ids = ids + dealProd.Id.ProductId + ",";
                }

            }
            return ids;
        }

        private string GetIndustriesForDeal(decimal idDeal)
        {
            string ids = null;
            IList<DealIndustry> lstDealInd = DealIndustryService.GetByDealId(idDeal);
            int cont = 0;
            foreach (DealIndustry dealInd in lstDealInd)
            {
                cont++;

                if (lstDealInd.Count == cont)
                {
                    ids = ids + dealInd.Id.IndustryId.ToString();
                }
                else
                {
                    ids = ids + dealInd.Id.IndustryId + ",";
                }

            }
            return ids;
        }

        private string GetEndUserForDeal(decimal dealid)
        {
            string ids = null;
            IList<DealUserProfile> lstDealInd = DealUserProfileService.GetByDealId(dealid);
            int cont = 0;
            foreach (DealUserProfile dealInd in lstDealInd)
            {
                cont++;

                if (lstDealInd.Count == cont)
                {
                    ids = ids + dealInd.Id.UserId.ToString();
                }
                else
                {
                    ids = ids + dealInd.Id.UserId + ",";
                }

            }
            return ids;
        }
        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetCompetitorsOfIndustry(decimal id)
        {
            string result = string.Empty;
            IList<Competitor> lstCompetitors = CompetitorService.GetByIndustryId(id);
            int cont = 0;
            foreach (Competitor comp in lstCompetitors)
            {
                cont++;
                if (lstCompetitors.Count == cont)
                {
                    result = result + comp.Id + ":" + comp.Name;
                }
                else
                {
                    result = result + comp.Id + ":" + comp.Name + "_";
                }

            }
            return Content(result);
        }

        //migrate update

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetMasiveCompetitors() //?ids=id1,id2,....
        {
            string ids = StringUtility.CheckNull(Request["ids"]);
            string[] industryids = ids.Split(',');
            string result = string.Empty;
            IList<Competitor> lstCompetitors = new List<Competitor>();
            foreach (string industryid in industryids) //is possible improve getting all data using unique query
            {
                lstCompetitors = lstCompetitors.Union(CompetitorService.GetByIndustryId(decimal.Parse(industryid))).ToList();
            }
            result += string.Join("_", lstCompetitors.Select(c => string.Format("{0}:{1}", c.Id, c.Name)).ToArray());
            return Content(result);

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetProductsOfCompetitor(decimal id)
        {
            string result = string.Empty;
            string idDeal = Request["idDeal"];
            string industries = Request["idsIndustries"];
            decimal[] idIndustries = FormatUtility.GetDecimalArrayFromStringValues(industries.Split(','));
            IList<Product> lstProds;

            if ((idIndustries != null) && (idIndustries.Length > 0))
            {
                lstProds = ProductService.GetByCompetitorAndIndustry(new decimal[] { id }, idIndustries);
            }
            else
            {
                lstProds = ProductService.GetByCompetitor(new decimal[] { id });
            }


            int cont = 0;
            foreach (Product prod in lstProds)
            {
                cont++;

                if (lstProds.Count == cont)
                {
                    result = result + prod.Id + ":" + prod.Name;
                }
                else
                {
                    result = result + prod.Id + ":" + prod.Name + "_";
                }
            }

            if (result.Length > 0 && result.Substring(result.Length - 1, 1).Equals("_"))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetMasiveProducts()
        {
            string result = string.Empty;
            string sidCompetitors = Request["idCompetitors"];
            string sidIndustries = Request["idIndustries"];

            decimal[] idIndustries = FormatUtility.GetDecimalArrayFromStringValues(sidIndustries.Split(','));
            decimal[] idCompetitors = FormatUtility.GetDecimalArrayFromStringValues(sidCompetitors.Split(','));

            IList<Product> lstProds = ProductService.GetByCompetitorAndIndustry(idCompetitors, idIndustries);

            result = string.Join("_", lstProds.Select(c => string.Format("{0}:{1}", c.Id, c.Name)).ToArray());
            return Content(result);

        }

        //public void MigrateDealStatusByPhase(Deal deal)
        //{
        //   if (deal.Phase.Equals(DealPhaseDate.Cancelled))
        //     {
        //       deal.Status = DealStatus.Archive;
        //     }  
        //}

        //it's for test delete or migrate any object
        //exist only for test
        public void MigrateDeals()
        {
            IList<Deal> deals = DealService.GetAll();
            foreach (Deal p in deals)
            {
                //if (p.IndustryId != null)
                //{
                //    DealIndustry pi = new DealIndustry(new DealIndustryId((decimal)p.Id, (decimal)p.IndustryId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    DealIndustryService.Save(pi);
                //}
                //if (p.CompetitorId != null)
                //{
                //    DealCompetitor pi = new DealCompetitor(new DealCompetitorId((decimal)p.Id, (decimal)p.CompetitorId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    DealCompetitorService.Save(pi);
                //}
                //if (p.ProductId != null)
                //{
                //    DealProduct pi = new DealProduct(new DealProductId((decimal)p.Id, (decimal)p.ProductId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    DealProductService.Save(pi);
                //}

            }
        }

        public ContentResult ChangeIndustryList(bool IsChecked, string IndustryIds)
        {
            string[] selected=new string[]{};

            if ( !string.IsNullOrEmpty(IndustryIds) )
            {
                selected =  IndustryIds.Split(',');
            }
            string result = string.Empty;
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

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ContentResult GetCustomerContactOfCustomer(decimal id)
        //{
        //    string result = string.Empty;
        //    IList<Employee> employeeList = EmployeeService.GetByCompanyIdAndCompanyType(id, "CUST", CurrentCompany);
        //    int cont = 0;
        //    foreach (Employee employee in employeeList)
        //    {
        //        cont++;
        //        if (employeeList.Count == cont)
        //        {
        //            result = result + employee.Id +":"+ employee.FirstName + employee.LastName;
        //        }
        //        else
        //        {
        //            result = result + employee.Id + ":" + employee.FirstName + employee.LastName + "_";
        //        }
        //    }
        //    if (result.Length > 0 && result.Substring(result.Length - 1, 1).Equals("_"))
        //    {
        //        result = result.Substring(0, result.Length - 1);
        //    }
        //    return Content(result);
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public JsonResult GetCustomerContactByCustomer(decimal id)
        //{
        //    string result = string.Empty;
        //    IList<Employee> employeeList = EmployeeService.GetByCompanyIdAndCompanyType(id, "CUST", CurrentCompany);
        //    return ControllerUtility.GetSelectOptionsFromGenericList<Employee>(employeeList, "Id", "FirstName");
        //    //return Content(result);
        //}

        #endregion

        public ActionResult GetInactiveDeal()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["ShowArchived"] = "all";
            return View("Index");
        }
        private void SetEmployeesByCustmerid(IList<Customer> customerList)
        {
            List<SelectListItem> selectItemsList = new List<SelectListItem>();
            List<decimal> customerIds = new List<decimal>();

            if (customerList != null)
            {
                foreach (Customer customer in customerList)
                {
                    customerIds.Add(customer.Id);
                }
            }

            StringBuilder employeeInfo = new StringBuilder();

            IList<Employee> customerContacList = EmployeeService.GetByCompanyAndType(customerIds.ToArray(), "CUST", CurrentCompany);

            if (customerContacList != null)
            {
                decimal currentCustomer = 0;
                for (int i = 0; i < customerContacList.Count; i++)
                {
                    Employee employee = customerContacList[i];
                    decimal companyId = DecimalUtility.CheckNull(employee.CompanyId);

                    if (i == 0)
                    {
                        currentCustomer = companyId;
                    }

                    if (currentCustomer.CompareTo(companyId) != 0)
                    {
                        selectItemsList.Add(new SelectListItem { Value = currentCustomer.ToString(), Text = employeeInfo.ToString() });
                        customerIds.Remove(currentCustomer);

                        currentCustomer = companyId;
                        employeeInfo = new StringBuilder();
                    }

                    if (employeeInfo.Length > 0)
                    {
                        employeeInfo.Append("_");
                    }

                    employeeInfo.Append(employee.Id.ToString()).Append(":").Append(employee.FirstName).Append(" ").Append(employee.LastName);

                    if ((i + 1) == customerContacList.Count)
                    {
                        selectItemsList.Add(new SelectListItem { Value = currentCustomer.ToString(), Text = employeeInfo.ToString() });
                        customerIds.Remove(currentCustomer);
                    }
                }
            }

            foreach (decimal customerId in customerIds)
            {
                selectItemsList.Add(new SelectListItem { Value = customerId.ToString(), Text = string.Empty });
            }

            ViewData["EmployeesByCustomerList"] = new SelectList(selectItemsList, "Value", "Text");
        }
    }
}
