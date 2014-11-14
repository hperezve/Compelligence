using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Resources;

using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility;
using Compelligence.Common.Utility.Web;

using Compelligence.Security.Filters;
using Compelligence.Util.Validation;
using Compelligence.Web.Models.Util;
using Compelligence.Util.Type;
using Compelligence.Security.Managers;

using System.Configuration;
using Compelligence.Util.Common;
using System.Globalization;
using Compelligence.Domain.Entity.Views;
using System.Text;

namespace Compelligence.Web.Controllers
{
    [AuthenticationFilter] //Enabled because this class not is used from SFDC
    public class DealSupportController : FrontEndFormController<Deal, decimal>
    {

        #region Public Properties

        public IDealService DealService
        {
            get { return (IDealService)_genericService; }
            set { _genericService = value; }
        }

        public ISalesForceService SalesForceService { get; set; }

        public IForumResponseService ForumResponseService { get; set; }

        public IForumService ForumService { get; set; }

        public IClientCompanyService ClientCompanyService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICustomerService CustomerService { get; set; }

        public ILibraryService LibraryService { get; set; }

        public IFileService FileService { get; set; }

        public IEmailService EmailService { get; set; }

        public IQuizService QuizService { get; set; }

        public IDealCompetitorService DealCompetitorService { get; set; }

        public IDealIndustryService DealIndustryService { get; set; }

        public IDealProductService DealProductService { get; set; }

        public IActionHistoryService ActionHistoryService { get; set; }

        public IIndustryProductService IndustryProductService { get; set; }

        public IIndustryCompetitorService IndustryCompetitorService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IDealUserProfileService DealUserProfileService { get; set; }

        public IEmployeeService EmployeeService { get; set; }
        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public override ActionResult Index()
        {
            SetLabels();
            SetDefaultDataToLoadPage();
            GetEnabledDeals();
            ViewData["Page"] = Request["Page"];
            if (Request.Params["ShowArchived"] != null && !Request.Params["ShowArchived"].Equals(""))
            {
                ViewData["ShowArchived"] = Request.Params["ShowArchived"];
            }
            ViewData["ShowAll"] = "yes";
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
        }

        public ActionResult GetArchivedDeals()
        {
            SetLabels(); 
            Session["ShowArchived"] = "yes";
            ViewData["ShowArchived"] = "yes";
            ViewData["ShowAll"] = "yes";
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
        }

        public ActionResult GetEnabledDeals()
        {
            SetLabels();
            SetDefaultDataToLoadPage();
            Session["ShowArchived"] = "no";
            ViewData["ShowAll"] = "yes";
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCustomers()
        {
            SetLabels();  
            IList<Customer> customerList = CustomerService.GetAllActiveByClientCompany(CurrentCompany);
            //IList<Customer> newCustomerList = new List<Customer>();
            //string employeeInfo = string.Empty;
            //foreach (Customer customer in customerList)
            //{
            //    string customerEmployee = string.Empty;
            //    employeeInfo = string.Empty;
            //    customerEmployee = customer.Id.ToString();
            //    IList<Employee> customerContacList = EmployeeService.GetByCompanyIdAndCompanyType(customer.Id, "CUST", CurrentCompany);
            //    if ((customerContacList != null) && (customerContacList.Count > 0))
            //    {
            //        //customerEmployee += ";";
            //        foreach (Employee employee in customerContacList)
            //        {
            //            if (!string.IsNullOrEmpty(employeeInfo))
            //            {
            //                employeeInfo += "_";
            //            }
            //            employeeInfo += employee.Id.ToString() + ":" + employee.FirstName + " " + employee.LastName;
            //        }
            //        customerEmployee += "/" + employeeInfo;
            //    }
            //    customer.Employees = customerEmployee;
            //    newCustomerList.Add(customer);
            //    //dataDictionario.Add(employeeInfo, customer.Name);
            //}
            //return ControllerUtility.GetSelectOptionsFromGenericList<Customer>(newCustomerList, "Employees", "Name");
            return ControllerUtility.GetSelectOptionsFromGenericList<Customer>(customerList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetIndustries()
        {
            IList<Industry> industryList = IndustryService.GetAllByClientCompany(CurrentCompany);

            return ControllerUtility.GetSelectOptionsFromGenericList(industryList, "Id", "Name");
        }

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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download(decimal id)
        {
            File file = FileService.GetByEntityId(id, DomainObjectType.Library);
            string check = StringUtility.CheckNull(Request["chk"]);

            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            if (check.ToLower().Equals("true"))
            {
                if ((file != null) && System.IO.File.Exists(fullpath + file.PhysicalName))
                    return Content("Found");
                else
                    return Content("NotFound");
            }
            else
            {
                //ActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File);
                ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
            }

            return Content(string.Empty);
        }

        public ActionResult RemoveLibrary(string LibraryId)
        {
            return Content(string.Empty);
        }

        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public ActionResult Comments(decimal id)
        {
            SetLabels();
            Deal deal = DealService.GetById(id);
            Forum forum = ForumService.GetByEntityId(deal.Id, DomainObjectType.Deal, ForumType.Comment);

            ViewData["Libraries"] = LibraryService.GetByEntityIdAndType(deal.Id, DomainObjectType.Deal);
            ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id,DomainObjectType.Deal);
            //
            SetUserSecurityAccess(deal);
            //Commented due to new multiple relations with Industry, COmpetitor and Product
            //deal.Industry = IndustryService.GetById(DecimalUtility.CheckNull(deal.IndustryId));
            //deal.Competitor = CompetitorService.GetById(DecimalUtility.CheckNull(deal.CompetitorId));
            //deal.Product = ProductService.GetById(DecimalUtility.CheckNull(deal.ProductId));
            ViewData["showResultMessage"] = "false";
            GetDataOfConfiguration(CurrentCompany);
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return RedirectToAction("Index", "DealSupport");
            }
            //SetDefatultEntityToLoadPage(FrontEndPages.ForumDeal, "Deal Support:Comments");
            ///ONLY HELP IS TO ALL PAGES OF DEAL
            SetDefaultDataToLoadPage();
            return View("Comments", deal);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveComments(decimal id,decimal forumresponseid)
        {
            SetLabels(); 
            Deal deal = DealService.GetById(id);
            Forum forum = ForumService.GetByEntityId(deal.Id, DomainObjectType.Deal, ForumType.Comment);

            ForumResponseService.DeleteCascading(forum.Id, forumresponseid);
            GetDataOfConfiguration(CurrentCompany);
            return Comments(id);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendComment(decimal id, FormCollection form)
        {
            SetLabels(); 
            //Deal dealToUpdateCounter = DealService.GetById(decimal.Parse(DealId));
            //DealService.Update(dealToUpdateCounter);
            Deal deal = DealService.GetById(id);
            ForumResponse forumResponse = new ForumResponse();

            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Deal;
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            forumResponse.ParentResponseId = (!string.IsNullOrEmpty(form["ParentResponseId"])) ? Convert.ToDecimal(form["ParentResponseId"]) : 0;
            forumResponse.ClientCompany = CurrentCompany;

            forumResponse.Libraries = GetLibrariesForEntity(deal.Id,DomainObjectType.Deal, LibraryTypeKeyCode.DealSupport);

            ForumService.SaveForumResponse(forumResponse, ForumType.Comment);

            EmailService.SendCommentEmail(forumResponse.CreatedBy,deal.Name, DomainObjectType.Deal, id, CurrentUser, forumResponse.Response, CurrentCompany, forumResponse.Libraries);
            
            //ActionHistory(id, EntityAction.Commented, DomainObjectType.Forum);
            ActionHistoryService.RecordActionHistory(id, EntityAction.Commented, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);


            Forum forum = ForumService.GetByEntityId(deal.Id, DomainObjectType.Deal, ForumType.Comment);

            ViewData["Libraries"] = LibraryService.GetByEntityIdAndType(deal.Id, DomainObjectType.Deal);
            ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Deal);
            //
            SetUserSecurityAccess(deal);
            ViewData["showResultMessage"] = "true";
            GetDataOfConfiguration(CurrentCompany);
            return View("Comments", deal);
            
        }

        public ActionResult Close(string DealId)
        {
            Deal deal = DealService.GetById(decimal.Parse(DealId));
            deal.Status = DealStatus.Disable;
            DealService.Update(deal);
            //ActionHistory(deal.Id, EntityAction.Closed, DomainObjectType.Deal);
            ActionHistoryService.RecordActionHistory(deal.Id, EntityAction.Closed, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            return Content(string.Empty);
        }

        public ActionResult CloseRating(string DealId) //So, ProjectId is Deal.Id
        {
            ViewData["DealId"] = decimal.Parse(DealId);
            return View("Rating");
        }

        public ActionResult CloseList()
        {
            return View("List");
        }

        public ActionResult GetOpportunities(string OwnerId)
        {
            SetLabels();
            int error = 0;
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            ViewData["Opportunities"] = SalesForceService.GetOpportunities(clientCompany, CurrentUser, ref error);
            ViewData["ErrorMessage"] = string.Empty;
            //SetDefatultEntityToLoadPage(FrontEndPages.DealSupportOportunities, "Deal Support:Import from SalesForce");
            ///ONLY HELP IS TO ALL PAGES OF DEAL
            SetDefaultDataToLoadPage();
            switch ((SalesForceAPI.SalesForceStatus)error)
            {
                case SalesForceAPI.SalesForceStatus.ConnectNoItems:
                    ViewData["ErrorMessage"] = SalesForceAPI.Resource.ErrorMessages.SalesforceConnectNoItems;
                    break;
                case SalesForceAPI.SalesForceStatus.NoConnectWrongToken:
                    ViewData["ErrorMessage"] = SalesForceAPI.Resource.ErrorMessages.SalesforceNoConnectWrongToken;
                    break;
                case SalesForceAPI.SalesForceStatus.NoConnectWrongUser:
                    ViewData["ErrorMessage"] = SalesForceAPI.Resource.ErrorMessages.SalesforceNoConnectWrongUser;
                    break;
                case SalesForceAPI.SalesForceStatus.ConnectUserNotFound:
                    ViewData["ErrorMessage"] = SalesForceAPI.Resource.ErrorMessages.SalesforceConnectNoUserFound;
                    break;
            }
            ActionHistoryService.RecordActionHistory(0, EntityAction.Listed, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            //ActionHistory(0, EntityAction.Listed, DomainObjectType.Deal);
            return View("Opportunity");
        }

        public ActionResult AddOpportunity(string Id)
        {
            Deal deal = SalesForceService.GetPartialOpportunity(Id);
            deal.Status = DealStatus.Enable;

            if (!UserManager.GetInstance().IsEndUser(CurrentUser))
            {
                deal.AssignedTo = CurrentUser;
            }

            SetDefaultDataForSave(deal);

            SalesForceToLibraries(deal, Id); // Download from SalesForce and Upload to Compelligence

            DealService.Save(deal);

            ActionHistoryService.RecordActionHistory(deal.Id, EntityAction.Imported, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            return Index();  //For Set WiewData
        }

        public ActionResult Rating(string ProjectId, string Rating) //So, ProjectId is Deal.Id
        {
            if (ProjectId != null && Rating != null)
            {
                DealService.SaveRating(decimal.Parse(ProjectId), decimal.Parse(Rating));
                //ActionHistory(decimal.Parse(ProjectId), EntityAction.SetedRating, DomainObjectType.Deal);
                ActionHistoryService.RecordActionHistory(decimal.Parse(ProjectId), EntityAction.SetedRating, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);

            }
            return Content(string.Empty);
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Deal deal, FormCollection formCollection)
        {
            string selectedIndustries = formCollection["IndustryIds"];
            string selectedCompetitors = formCollection["CompetitorIds"];
            string selectedProducts = formCollection["ProductIds"];
            string primaryCompetitor = formCollection["PrimaryCompetitor"];

            deal.IndustriesIds = selectedIndustries;
            deal.CompetitorsIds = selectedCompetitors;
            deal.ProductsIds = selectedProducts;

            if (Validator.IsBlankOrNull(deal.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.DealNameRequiredError);
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
                ValidationDictionary.AddError("CompetitorId", LabelResource.DealCompetitorIdRequiredError);
            }
            else
            {
                string[] idsCompetitor = deal.CompetitorsIds.Split(',');
                if (idsCompetitor.Length > 1 && Validator.IsBlankOrNull(deal.PrimaryCompetitor))
                {
                    ValidationDictionary.AddError("PrimaryCompetitor", LabelResource.DealPrimaryCompetitorRequiredError);
                }
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

            if (Validator.IsBlankOrNull(deal.DueDateFrm))
            {
                ValidationDictionary.AddError("DueDateFrm", LabelResource.DealDueDateRequiredError);
            }
            else
            {
                if (!Validator.IsDate(deal.DueDateFrm, GetFormatDate()))
                    ValidationDictionary.AddError("DueDateFrm", string.Format(LabelResource.DealDueDateFormatError, GetFormatDate()));
            }

            if (!ValidationDictionary.IsValid)
            {
                SetUsersIds(deal, formCollection);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            SetLabels();
            GetDataOfConfiguration(CurrentCompany);
            //SetDefatultEntityToLoadPage(FrontEndPages.DealSupportEdit, "Deal Support:Edit");
            ///ONLY HELP IS TO ALL PAGES OF DEAL
            SetDefaultDataToLoadPage();
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetIndustryEnabled(clientCompany);
            decimal industryListCount = IndustryService.GetIndustryEnabled(clientCompany).Count;
            IList<ResourceObject> dealPhaseList = ResourceService.GetAllBySortValue<DealPhaseDate>();

            //IList<UserProfile> endUserList = UserProfileService.GetEndUsersWithoutUser(clientCompany, CurrentUser);
            IList<UserProfile> alluserslist = UserProfileService.GetAllUsersProfile(CurrentCompany);
            ViewData["EndUserIdMultiList"] = new MultiSelectList(alluserslist, "Id", "Name");
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["IndustryIdMultiListCount"] = industryListCount;
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["PhaseList"] = new SelectList(dealPhaseList, "Id", "Value");
            ViewData["idDeal"] = 0;
            ViewData["SelectedCompetitorIdList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["SelectedIndustryIdList"] = new SelectList(new List<Industry>(), "Id", "Name");
            ViewData["EndUserIdMultiListDisabled"] = "false";
            ViewData["EmployeeIdList"] = new MultiSelectList(new List<Employee>(), "Id", "Name");
            ViewData["WinningCompetitorList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["CheckIndustryIds"] = false;
            SetEmployeesByCustmerid();
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
            deal.IndustriesIds = obtainIndustriesForDeal(deal.Id);
            string[] selectedValues = deal.IndustriesIds.Split(',');
            var selected = selectedValues;
            IList<Industry> industryList = IndustryService.GetIndustryEnabled(deal.ClientCompany);
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);

            ViewData["CloseDateFrm"] = DateTimeUtility.ConvertToString(deal.CloseDate, GetFormatDate());
            ViewData["CreatedDateFrm"] = DateTimeUtility.ConvertToString(deal.CreatedDate, GetFormatDate());
            SetCascadingData(deal, true);

            //obtain primary industry
            deal.PrimaryIndustry = obtainPrimaryIndustry(deal.Id);
            if (deal.IndustriesIds != null && !deal.IndustriesIds.Equals(""))
            {
                IList<Industry> lstIndustriesSelected = new List<Industry>();
                string[] idIndustries = deal.IndustriesIds.Split(',');
                for (int i = 0; i < idIndustries.Length; i++)
                {
                    Industry tempIndustry = IndustryService.GetById(decimal.Parse(idIndustries[i]));
                    lstIndustriesSelected.Add(tempIndustry);
                }
                if (deal.PrimaryIndustry == 0 || deal.PrimaryIndustry == null)
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name");
                else
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name", deal.PrimaryIndustry);
            }

            //obtain primary competitor
            deal.PrimaryCompetitor = obtainPrimaryCompetitor(deal.Id);
            
            if (deal.CompetitorsIds != null && !deal.CompetitorsIds.Equals(""))
            {
                IList<Competitor> lstCompetitorsSelected = new List<Competitor>();
                string[] idCompetitors = deal.CompetitorsIds.Split(',');
                for (int i = 0; i < idCompetitors.Length; i++)
                {
                    Competitor tempCompetitor = CompetitorService.GetById(decimal.Parse(idCompetitors[i]));
                    lstCompetitorsSelected.Add(tempCompetitor);
                }
                if (deal.PrimaryCompetitor == 0 || deal.PrimaryCompetitor == null)
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name");
                else
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name", deal.PrimaryCompetitor);
            }

            ViewData["DueDateFrm"] = DateTimeUtility.ConvertToString(deal.DueDate, GetFormatDate());

            if (CurrentUser.Equals(deal.CreatedBy))
            {
                ViewData["EndUserIdMultiListDisabled"] = "false";
            }
            else
            {
                ViewData["EndUserIdMultiListDisabled"] = "true";
            }
            IList<Library> libraries = LibraryService.GetByEntityId(deal.Id, DomainObjectType.Deal);
            ViewData["Libraries"] = libraries == null ? new List<Library>() : libraries;
            ViewData["idDeal"] = deal.Id;
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
                deal.IndustriesIds = obtainIndustriesForDeal(deal.Id);

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
                    industryList = IndustryService.GetIndustryEnabled(deal.ClientCompany);
                }
                else
                {
                    industryList = IndustryService.GetIndustryEnabled((string)Session["ClientCompany"]);
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

            //obtain primary industry
            //deal.PrimaryIndustry = obtainPrimaryIndustry(deal.Id);
            if (deal.IndustriesIds != null && !deal.IndustriesIds.Equals(""))
            {
                IList<Industry> lstIndustriesSelected = new List<Industry>();
                string[] idIndustries = deal.IndustriesIds.Split(',');
                for (int i = 0; i < idIndustries.Length; i++)
                {
                    Industry tempIndustry = IndustryService.GetById(decimal.Parse(idIndustries[i]));
                    lstIndustriesSelected.Add(tempIndustry);
                }
                if (deal.PrimaryIndustry == 0 || deal.PrimaryIndustry == null)
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name");
                else
                    ViewData["SelectedIndustryIdList"] = new SelectList(lstIndustriesSelected, "Id", "Name", deal.PrimaryIndustry);
            }

            //obtain primary competitor
            //deal.PrimaryCompetitor = obtainPrimaryCompetitor(deal.Id);
            if (deal.CompetitorsIds != null && !deal.CompetitorsIds.Equals(""))
            {
                IList<Competitor> lstCompetitorsSelected = new List<Competitor>();
                string[] idCompetitors = deal.CompetitorsIds.Split(',');
                for (int i = 0; i < idCompetitors.Length; i++)
                {
                    Competitor tempCompetitor = CompetitorService.GetById(decimal.Parse(idCompetitors[i]));
                    lstCompetitorsSelected.Add(tempCompetitor);
                }
                if (deal.PrimaryCompetitor == 0 || deal.PrimaryCompetitor == null)
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name");
                else
                    ViewData["SelectedCompetitorIdList"] = new SelectList(lstCompetitorsSelected, "Id", "Name", deal.PrimaryCompetitor);
            }

            IList<Library> libraries = LibraryService.GetByEntityId(deal.Id, DomainObjectType.Deal);
            ViewData["Libraries"] = libraries == null ? new List<Library>() : libraries;

            deal.OldPrimaryCompetitor = deal.PrimaryCompetitor;
            deal.OldPrimaryIndustry = deal.PrimaryIndustry;
            deal.oldDealEmployeeIds = deal.DealEmployeeIds;
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(deal.OldIndustriesIds, deal.OldIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorsIds", new ValueProviderResult(deal.OldCompetitorsIds, deal.OldCompetitorsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldProductsIds", new ValueProviderResult(deal.OldProductsIds, deal.OldProductsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldPrimaryCompetitor", new ValueProviderResult(deal.OldPrimaryCompetitor.ToString(), deal.OldPrimaryCompetitor.ToString(), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldPrimaryIndustry", new ValueProviderResult(deal.OldPrimaryIndustry.ToString(), deal.OldPrimaryIndustry.ToString(), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("PrimaryIndustry", new ValueProviderResult(Convert.ToString(deal.PrimaryIndustry), Convert.ToString(deal.PrimaryIndustry), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("PrimaryCompetitor", new ValueProviderResult(Convert.ToString(deal.PrimaryCompetitor), Convert.ToString(deal.PrimaryCompetitor), CultureInfo.InvariantCulture));
            ModelState.SetModelValue("oldDealEmployeeIds", new ValueProviderResult(deal.oldDealEmployeeIds, deal.oldDealEmployeeIds, CultureInfo.InvariantCulture));
        }

        protected void SetUsersIds(Deal deal, FormCollection collection)
        {
            string selectedEndUsers = collection["UsersIds"];
            deal.UsersIds = selectedEndUsers;
        }

        protected override void SetDefaultEntityDataForSave(Deal deal)
        {
            deal.MetaData = deal.Name + ":" + deal.MetaData;
            deal.Status = DealStatus.Enable;
            if (!string.IsNullOrEmpty(deal.IndustriesIds))
            {
                string[] industriesid = deal.IndustriesIds.Split(',');
                for (int i = 0; i < industriesid.Length; i++)
                {
                    Industry industry = IndustryService.GetById(decimal.Parse(industriesid[i]));
                    deal.MetaData += ":" + industry.Name;
                }
            }
            if (!string.IsNullOrEmpty(deal.CompetitorsIds))
            {
                string[] competitorsId = deal.CompetitorsIds.Split(',');
                for (int j = 0; j < competitorsId.Length; j++)
                {
                    Competitor competitor = CompetitorService.GetById(decimal.Parse(competitorsId[j]));
                    deal.MetaData += ":" + competitor.Name;
                }
            }
            if (!string.IsNullOrEmpty(deal.ProductsIds))
            {
                string[] productsid = deal.ProductsIds.Split(',');
                for (int k = 0; k < productsid.Length; k++)
                {
                    Product product = ProductService.GetById(decimal.Parse(productsid[k]));
                    deal.MetaData += ":" + product.Name;
                }
            }
        }

        protected override void GetFormData(Deal deal, FormCollection collection) //At save first time
        {
            deal.DueDate = DateTimeUtility.ConvertToDate(collection["DueDateFrm"], GetFormatDate());
            deal.CloseDate = DateTimeUtility.ConvertToDate(collection["CloseDateFrm"], GetFormatDate());
            deal.Libraries = GetLibrariesForEntity(deal.Id,DomainObjectType.Deal,LibraryTypeKeyCode.DealSupport);

            string selectedIndustries = collection["IndustryIds"];
            string selectedCompetitors = collection["CompetitorIds"];
            string selectedProducts = collection["ProductIds"];
            string primaryCompetitor = collection["PrimaryCompetitor"];
            string winningCompetitor = collection["WinningCompetitor"];
            string primaryIndustry = collection["PrimaryIndustry"];
            string selectedEndUsers = collection["UsersIds"];
            string selectedEmployees = collection["DealEmployeeIds"];

            deal.OldIndustriesIds = obtainIndustriesForDeal(deal.Id);
            deal.OldCompetitorsIds = obtainCompetitorsForDeal(deal.Id);
            deal.OldProductsIds = obtainProductsForDeal(deal.Id);
            deal.OldUsersIds = GetEndUserForDeal(deal.Id); 

            deal.IndustriesIds = selectedIndustries;
            deal.CompetitorsIds = selectedCompetitors;
            deal.ProductsIds = selectedProducts;
            deal.UsersIds = selectedEndUsers;
            deal.DealEmployeeIds = selectedEmployees;
            deal.WinningCompetitor = winningCompetitor;
            if (!collection["PrimaryCompetitor"].Equals("0") && !collection["PrimaryCompetitor"].Equals(""))
                deal.PrimaryCompetitor = Convert.ToDecimal(primaryCompetitor);
            if (!collection["PrimaryIndustry"].Equals("0") && !collection["PrimaryIndustry"].Equals(""))
                deal.PrimaryIndustry = Convert.ToDecimal(primaryIndustry);
        }

        protected override void SetUserSecurityAccess(Deal deal)
        {
            string securityAccess = UserSecurityAccess.Read;

            if (DealService.HasAccessToDeal(deal, CurrentUser))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void ExecuteActionsAfterToSave(Deal deal)
        {
            ActionHistoryService.RecordActionHistory(deal.Id, EntityAction.Opened, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
        }

        protected override void ExecuteActionsAfterToUpdate(Deal deal)
        {
            ActionHistoryService.RecordActionHistory(deal.Id, EntityAction.Updated, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
        }

        protected override void SetDefaultDataByPage()
        {
            ViewData["Entity"] = FrontEndPages.DealSupport;
            ViewData["TitleHelp"] = "Deal Support";
        }
        #endregion

        #region Private Methods

        private void SalesForceToLibraries(Deal deal, string OpportunityId)
        {
            IList<Library> libraryItems = new List<Library>();
            String Target = GenericService.GetPathBase("TempFilePath", true);
            IList<string> xfiles = SalesForceService.GetAttachFiles(OpportunityId, Target);
            foreach (string xfilename in xfiles)
            {
                Library library = new Library();

                //Descriptor information for Library
                library.Name = xfilename;
                library.Description = deal.Name;
                library.MetaData = deal.Name;
                library.DateAdded = DateTime.Today;
                //End-Descriptor information for Library

                library.FileName = xfilename;
                library.FilePhysicalName = xfilename;
                library.HeaderType = DomainObjectType.Deal;
                library.EntityType = DomainObjectType.Deal;
                libraryItems.Add(library);
            }
            deal.Libraries = libraryItems;

        }

        private string obtainCompetitorsForDeal(decimal idDeal)
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

        private string obtainProductsForDeal(decimal idDeal)
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

        private string obtainIndustriesForDeal(decimal idDeal)
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

        private decimal obtainPrimaryCompetitor(decimal idDeal)
        {
            decimal idPrimary = 0;
            IList<DealCompetitor> lstDealComp = DealCompetitorService.GetByDealId(idDeal);

            foreach (DealCompetitor dealInd in lstDealComp)
            {
                if (dealInd.PrimaryCompetitor.Equals("Y"))
                    idPrimary = dealInd.Id.CompetitorId;
            }
            return idPrimary;
        }

        private decimal obtainPrimaryIndustry(decimal idDeal)
        {
            decimal idPrimary = 0;
            IList<DealIndustry> lstDealComp = DealIndustryService.GetByDealId(idDeal);

            foreach (DealIndustry dealInd in lstDealComp)
            {
                if (dealInd.PrimaryIndustry.Equals("Y"))
                    idPrimary = dealInd.Id.IndustryId;
            }
            return idPrimary;
        }

        private void SetCascadingData(Deal deal, bool action)
        {
            if (action)//action indicates if we get competitors and products from DB or from a form
            {
                deal.CompetitorsIds = obtainCompetitorsForDeal(deal.Id);
                deal.ProductsIds = obtainProductsForDeal(deal.Id);
                deal.UsersIds = GetEndUserForDeal(deal.Id);
            }

            string[] selectedProducts = deal.ProductsIds.Split(',');
            string[] selectedCompetitors = deal.CompetitorsIds.Split(',');

            string[] selectedEndUsersValues = deal.UsersIds.Split(',');
            var selectedEndUsers = selectedEndUsersValues;
            IList<UserProfile> alluserslist = UserProfileService.GetAllUsersProfile(CurrentCompany);
            ViewData["EndUserIdMultiList"] = new MultiSelectList(alluserslist, "Id", "Name", selectedEndUsers);
            ModelState.SetModelValue("UsersIdsHidden", new ValueProviderResult(deal.UsersIds, deal.UsersIds, CultureInfo.InvariantCulture));

            IList<Competitor> competitorList = new List<Competitor>();
            IList<Product> productList = new List<Product>();

            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");

            string[] idsInd = deal.IndustriesIds.Split(',');
            string clientCompany = (string)Session["ClientCompany"];

            if (idsInd[0].Equals(""))
                idsInd = null;

            if (deal.IndustriesIds != null && !deal.IndustriesIds.Equals(""))
            {
                for (int i = 0; i < idsInd.Length; i++)
                {
                    IList<Competitor> tempCompetitorList = CompetitorService.GetByIndustryId(Convert.ToDecimal(idsInd[i]));
                    foreach (Competitor comp in tempCompetitorList)
                    {
                        if (competitorList.IndexOf(comp) == -1)
                        {
                            competitorList.Add(comp);
                        }
                    }
                }
            }
            if (deal.CompetitorsIds != null && !deal.CompetitorsIds.Equals(""))
            {
                for (int j = 0; j < selectedCompetitors.Length; j++)
                {
                    IList<Product> tempProductList = ProductService.GetByCompetitor(Convert.ToDecimal(selectedCompetitors[j]));
                    //obtain products by Industry
                    foreach (Product prod in tempProductList)
                    {
                        bool addProduct = false;
                        if (!deal.IndustriesIds.Equals(""))
                        {
                            //Verifying if Products from the Competitor are from Industries too
                            string[] actualIndustries = deal.IndustriesIds.Split(',');
                            foreach (string idInd in actualIndustries)
                            {
                                IndustryProductId idIndProd = new IndustryProductId(Convert.ToDecimal(idInd), prod.Id);
                                IndustryProduct tempIndProd = IndustryProductService.GetById(idIndProd);
                                if (tempIndProd != null)
                                {
                                    addProduct = true;
                                }
                            }
                        }
                        else
                        { addProduct = true; }

                        if (productList.IndexOf(prod) == -1 && addProduct)
                        {
                            productList.Add(prod);
                        }
                    }
                }
            }

            if (productList.Count > 0)
            {
                if (deal.ProductsIds == null)
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name");
                else
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name", selectedProducts);
            }

            if (competitorList.Count > 0)
            {
                if (deal.CompetitorsIds == null)
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                else
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedCompetitors);
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

        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetCompetitorsOfIndustry(decimal id)
        {
            string result = string.Empty;
            IList<Competitor> lstCompetitors = CompetitorService.GetByIndustryIdAndStatusEnabled(id);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetProductsOfCompetitor(decimal id)
        {
            string result = string.Empty;
            string idDeal = Request["idDeal"];
            string idsIndustries = Request["idsIndustries"];
            string[] arrayIndustries = idsIndustries.Split(',');

            IList<Product> lstProds = ProductService.GetByCompetitorAndStatusEnabled(id);
            int cont = 0;
            foreach (Product prod in lstProds)
            {
                cont++;
                 bool addProd = false;

                if (!idsIndustries.Equals(""))
                {
                    if (prod.CompetitorId != null)
                    {
                        foreach (string idInd in arrayIndustries)
                        {
                            IndustryProductId idIndProd = new IndustryProductId(Convert.ToDecimal(idInd), prod.Id);
                            IndustryProduct tempIndProd = IndustryProductService.GetById(idIndProd);
                            if (tempIndProd != null)
                            {
                                addProd = true;
                            }
                        }
                    }
                }
                else
                {
                    addProd = true;
                }

                if (addProd)
                {
                    if (lstProds.Count == cont)
                    {
                        result = result + prod.Id + ":" + prod.Name;
                    }
                    else
                    {
                        result = result + prod.Id + ":" + prod.Name + "_";
                    }
                }
            }

            if (result.Length > 0 && result.Substring(result.Length - 1, 1).Equals("_"))
            {
                result = result.Substring(0, result.Length - 1);
            }
            return Content(result);
        }

        #endregion

        public ContentResult ChangeIndustryList(bool IsChecked, string[] IndustryIds)
        {
            string[] tempo = new string[] { };
            var selected = tempo;
            if (IndustryIds != null)
            {
                selected = IndustryIds;
            }
            string result = string.Empty;
            bool isSelected = false;
            if (IsChecked)
            {
                IList<IndustryByHierarchyView> industryListHierarchy = IndustryService.FindIndustryHierarchy(CurrentCompany);
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
                IList<Industry> industryList = IndustryService.GetIndustryEnabled(CurrentCompany);
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

        public ContentResult ChangeEnableIndustryList(bool IsChecked, string[] IndustryIds)
        {
            string[] tempo = new string[] { };
            var selected = tempo;
            if (IndustryIds != null)
            {
                selected = IndustryIds;
            }
            string result = string.Empty;
            bool isSelected = false;
            if (IsChecked)
            {
                IList<IndustryByHierarchyView> industryListHierarchy = IndustryService.FindAllActiveByHierarchyAndStatus(CurrentCompany, IndustryStatus.Enabled, "Eq");
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
                IList<Industry> industryList = IndustryService.GetIndustryEnabled(CurrentCompany);
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

        private void SetEmployeesByCustmerid()
        {
            IList<Customer> customerList = CustomerService.GetAllActiveByClientCompany(CurrentCompany);
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
