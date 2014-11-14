using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Util.Type;
using Compelligence.Common.Utility.Web;
using Compelligence.Web.Models.Util;
using Compelligence.Security.Filters;
using Compelligence.Web.Models.Web;
using Compelligence.Common.Utility;
using Compelligence.Domain.Entity.Views;
using Compelligence.DataTransfer.FrontEnd;
using System.Configuration;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    [AuthenticationFilter] //Enabled because this class not is used from SFDC
    public class EventsController : FrontEndFormController<Event, decimal>
    {

        #region Public Properties

        public IEventService EventService
        {
            get { return (IEventService)_genericService; }
            set { _genericService = value; }
        }

        public IEmailService EmailService { get; set; }

        public IForumResponseService ForumResponseService { get; set; }

        public IForumService ForumService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IResourceService ResourceService { get; set; }

        public ILibraryService LibraryService { get; set; }

        public IFileService FileService { get; set; }

        public IQuizService QuizService { get; set; }

        public IEventTypeService EventTypeService { get; set; }

        public IEventCompetitorService EventCompetitorService { get; set; }

        public IEventIndustryService EventIndustryService { get; set; }

        public IEventProductService EventProductService { get; set; }

        public IIndustryProductService IndustryProductService { get; set; }

        public IIndustryCompetitorService IndustryCompetitorService { get; set; }       

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetEventTypes()
        {
            IList<EventType> eventTypeList = EventTypeService.GetAllActiveByClientCompany(CurrentCompany);
            return ControllerUtility.GetSelectOptionsFromGenericList(eventTypeList, "Id", "Name");
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

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetStartDateByTimeFrame(string id)
        {
            IList<ResourceObject> eventQuarterList = ResourceService.GetAll<EventTypeQuarter>();
            IList<ResourceObject> eventMonthList = ResourceService.GetAllBySortValue<EventTypeMonth>();
            if (id.Equals(EventTypePeriod.Quarter))
            {
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(eventQuarterList, "Id", "Value");
            }
            else
            {
                if (id.Equals(EventTypePeriod.Month))
                {
                    return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(eventMonthList, "Id", "Value");
                }
                else
                {
                    IList<DateTime> eventYearList = new List<DateTime>();
                    for (int i = DateTime.Today.Year; i <= 2029; i++)
                    {
                        eventYearList.Add(new DateTime(i, 1, 1));
                    }
                    return ControllerUtility.GetSelectOptionsFromGenericList<DateTime>(eventYearList, "Year", "Year");
                }
            }
        }

        public void GetParemeterList()
        {
            string sPage = Request.Params["page"];
            string sGroupBy = Request.Params["group"];
            string sOrderBy = Request.Params["order"];
            string sAsc = Request.Params["asc"];
            if (string.IsNullOrEmpty(sPage)) { sPage = "0"; }
            if (string.IsNullOrEmpty(sGroupBy)) { sGroupBy = string.Empty; }
            if (string.IsNullOrEmpty(sOrderBy)) { sOrderBy = string.Empty; }
            if (string.IsNullOrEmpty(sAsc)) { sAsc = string.Empty; }
            ViewData["page"] = sPage;
            ViewData["group"] = sGroupBy;
            ViewData["order"] = sOrderBy;
            ViewData["asc"] = sAsc;
        }

        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public override ActionResult Index()
        {
           // GetParemeterList();
            SetLabels();
            SetDefaultDataToLoadPage();
            IList<EventAllView> EventCollection = EventService.GetByStatus(CurrentCompany);
            ViewData["Event"] = EventCollection;
            if (Request.Params["ShowAll"] != null && !Request.Params["ShowAll"].Equals(""))
            {
                ViewData["ShowAll"] = Request.Params["ShowAll"];
            }
            else
            {
                if (Request.Params["lstevent"] != null && !Request.Params["lstevent"].Equals(""))
                {
                    ViewData["ShowAll"] = Request.Params["lstevent"];
                }
            }
            if (Request.Params["dateFilter"] != null && !Request.Params["dateFilter"].Equals(""))
            {
                ViewData["dateFilter"] = Request.Params["dateFilter"];
            }
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            return ChangeIndustry();
         }

        public void UpdateIndustryAndCompetitorDropDown()
        {
            string sIndustryId = Request.Params["Industry"];
            string sCompetitorId = Request.Params["Competitor"];
            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(CurrentCompany));
            if (string.IsNullOrEmpty(sIndustryId))
            {
                UpdateDropDownWith("Competitor", new List<Competitor>());
            }
            else
            {
                UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(decimal.Parse(sIndustryId)));
            }
        }

        public void GetDateFilter()
        {
            ViewData["dateFilter"] = string.Empty;
            string inputStartDate = Request.Params["txtStartDate"];
            if (!string.IsNullOrEmpty(inputStartDate))
            {
                ViewData["dateFilter"] = inputStartDate;
            }
        }

        public ActionResult GetAllEvents()
        {
            GetParemeterList();
            SetLabels();
            UpdateIndustryAndCompetitorDropDown();
            Session["showAll"] = "yes";
            ViewData["ShowAll"] = "yes";
            GetDateFilter();
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
        }

        public ActionResult GetActualEvents()
        {
            GetParemeterList();
            SetLabels();
            UpdateIndustryAndCompetitorDropDown();
            Session["showAll"] = "no";
            ViewData["ShowAll"] = "no";
            GetDateFilter();
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
        }

        public ActionResult GetEventsByDate()
        {
            GetParemeterList();
            SetLabels();
            UpdateIndustryAndCompetitorDropDown();
            string date = Request.Params["txtStartDate"];
            string showAll = Request.Params["lstevent"];
            ViewData["dateFilter"] = date;
            ViewData["showAll"] = showAll;
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
        }

        public ActionResult ChangeIndustry() //Update Competitor and Product
        {
            GetParemeterList();
            SetLabels();
            //UpdateDropDownWith("Industry", IndustryService.GetAllActiveByClientCompany(CurrentCompany));
            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(CurrentCompany));
            UpdateDropDownWith("Competitor", new List<Competitor>());
            GetDateFilter();
            string sIndustryId = Request.Params["Industry"];
            string chbox = Request.Params["lstevent"];
            //string inputStartDate = Request.Params["txtStartDate"];
            if (sIndustryId != null && sIndustryId.Length > 0)
            {                
                //if (!string.IsNullOrEmpty(inputStartDate))
                //{
                //    ViewData["dateFilter"] = inputStartDate;
                //}
                Session["Industry"] = sIndustryId;
                UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(decimal.Parse(sIndustryId)));
            }
            if (!string.IsNullOrEmpty(chbox))
            {
                ViewData["ShowAll"] = chbox;
            }
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
        }

        public ActionResult ChangeCompetitor() //Update Competitor and Product
        {
            GetParemeterList();
            SetLabels();
            string sIndustryId = Request.Params["Industry"];
            string sCompetitorId = Request.Params["Competitor"];
            string chbox = Request.Params["lstevent"];
            GetDateFilter();
            //string inputStartDate = Request.Params["txtStartDate"];
            //UpdateDropDownWith("Industry", IndustryService.GetAllActiveByClientCompany(CurrentCompany));
            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(CurrentCompany));
            UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(decimal.Parse(sIndustryId)));

            if (sCompetitorId.Length > 0)
            {
                Session["Industry"] = sIndustryId;
                Session["Competitor"] = sCompetitorId;                
                //if (!string.IsNullOrEmpty(inputStartDate))
                //{
                //    ViewData["dateFilter"] = inputStartDate;
                //}
            }
            else
            {
                return ChangeIndustry();
            }
            if (!string.IsNullOrEmpty(chbox))
            {
                ViewData["ShowAll"] = chbox;
            }
            GetDataOfConfiguration(CurrentCompany);
            return View("Index");
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

        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public ActionResult Comments(decimal id)
        {
            Event eventObject = EventService.GetById(id);
            SetPropertiesToEvent(eventObject);
            Forum forum = ForumService.GetByEntityId(eventObject.Id, DomainObjectType.Event, ForumType.Comment);
            SetLabels();
            ViewData["Libraries"] = LibraryService.GetByEntityId(eventObject.Id, DomainObjectType.Event);
            ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id,DomainObjectType.Event);
            SetUserSecurityAccess(eventObject);
            ViewData["showResultMessage"] = "false";
            GetDataOfConfiguration(CurrentCompany);
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return RedirectToAction("Index", "Events");
            }
            //SetDefatultEntityToLoadPage(FrontEndPages.ForumEvents, "Event:Comments");
            ///ONLY HELP IS TO ALL PAGES OF EVENT
            SetDefaultDataToLoadPage();
            return View("Comments", eventObject);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveComments(decimal id, decimal forumresponseid)
        {
            Event eventObject = EventService.GetById(id);
            SetPropertiesToEvent(eventObject);
            Forum forum = ForumService.GetByEntityId(eventObject.Id, DomainObjectType.Event, ForumType.Comment);
            ForumResponseService.DeleteCascading(forum.Id, forumresponseid);
            GetDataOfConfiguration(CurrentCompany);
            return Comments(id);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendComment(decimal id, FormCollection form)
        {
            Event eventObject = EventService.GetById(id);
            ForumResponse forumResponse = new ForumResponse();
            SetLabels();
            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Event;
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            forumResponse.ParentResponseId = (!string.IsNullOrEmpty(form["ParentResponseId"])) ? Convert.ToDecimal(form["ParentResponseId"]) : 0;
            forumResponse.ClientCompany = CurrentCompany;

            forumResponse.Libraries = GetLibrariesForEntity(eventObject.Id,DomainObjectType.Event, LibraryTypeKeyCode.Event);

            ForumService.SaveForumResponse(forumResponse, ForumType.Comment);

            EmailService.SendCommentEmail(forumResponse.CreatedBy, eventObject.EventName, DomainObjectType.Event, id, CurrentUser, forumResponse.Response, CurrentCompany, forumResponse.Libraries);

            //ActionHistory(id, EntityAction.Commented, DomainObjectType.Forum);
            ActionHistoryService.RecordActionHistory(id, EntityAction.Commented, DomainObjectType.Event, ActionFrom.FrontEnd,CurrentUser, CurrentCompany);

            SetPropertiesToEvent(eventObject);
            Forum forum = ForumService.GetByEntityId(eventObject.Id, DomainObjectType.Event, ForumType.Comment);
            SetLabels();
            ViewData["Libraries"] = LibraryService.GetByEntityId(eventObject.Id, DomainObjectType.Event);
            ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Event);
            SetUserSecurityAccess(eventObject);
            ViewData["showResultMessage"] = "true";
            GetDataOfConfiguration(CurrentCompany);
            return View("Comments", eventObject);           
           
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBack(decimal id, FormCollection form)
        {
            ForumResponse forumResponse = new ForumResponse();

            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Event;
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            forumResponse.ClientCompany = CurrentCompany;

            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

            //ActionHistory(id, EntityAction.FeedBack, DomainObjectType.Event);
            ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Event, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);

            return null;
        }

        [AuthenticationFilter]
        public ActionResult Close(decimal Id)
        {
            Event EventUpdated = EventService.GetById(Id);
            EventUpdated.Status = EventStatus.Disable;
            EventService.Update(EventUpdated);
            return Index();
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Event eventEntity, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(eventEntity.EventName))
            {
                ValidationDictionary.AddError("EventName", LabelResource.EventCaseNameRequiredError);
            }
            if (Validator.IsBlankOrNull(eventEntity.Scenario))
            {
                ValidationDictionary.AddError("Scenario", LabelResource.ScenarioRequiredError);
            }
            else
            {
                if (eventEntity.Scenario.Length > 2000)
                {
                    ValidationDictionary.AddError("Scenario", LabelResource.EventScenarioMaxLength);
                }
            }
            if (Validator.IsBlankOrNull(eventEntity.Confidence))
            {
                ValidationDictionary.AddError("Confidence", LabelResource.ConfidenceRequiredError);
            }
            if (eventEntity.MarketImpact.Length > 2000)
            {
                ValidationDictionary.AddError("MarketImpact", LabelResource.EventMarketImpactMaxLength);
            }
            if (eventEntity.CompanyImpact.Length > 2000)
            {
                ValidationDictionary.AddError("CompanyImpact", LabelResource.EventCompanyImpactMaxLength);
            }
            if (eventEntity.RecommendedActions.Length > 2000)
            {
                ValidationDictionary.AddError("RecommendedActions", LabelResource.EventRecommendedActionsMaxLength);
            }
            if (eventEntity.Comment.Length > 2000)
            {
                ValidationDictionary.AddError("Comment", LabelResource.EventCommentMaxLength);
            }
            if (!ValidationDictionary.IsValid)
            {
                GetFormData(eventEntity, formCollection);
            }

            if (Validator.IsBlankOrNull(eventEntity.TimeFrameFrm))
            {
                ValidationDictionary.AddError("TimeFrameFrm", LabelResource.TimeFrameFrmError);
            }

            /*Month*/
            if (eventEntity.TimeFrameFrm.Contains('M'))
            {
                if (Validator.IsBlankOrNull(eventEntity.StartIntervalDate) && Validator.IsBlankOrNull(eventEntity.YearQuarter))
                {
                    ValidationDictionary.AddError("StartIntervalDate", LabelResource.StartIntervalDateFrmError);
                    ValidationDictionary.AddError("YearQuarter", LabelResource.YearFrmError);
                }
                else if (Validator.IsBlankOrNull(eventEntity.StartIntervalDate))
                {
                    ValidationDictionary.AddError("StartIntervalDate", LabelResource.StartIntervalDateFrmError);
                }
                else if (Validator.IsBlankOrNull(eventEntity.YearQuarter))
                {
                    ValidationDictionary.AddError("YearQuarter", LabelResource.YearFrmError);
                }
            }

            /*Quarter*/
            if (eventEntity.TimeFrameFrm.Contains('Q'))
            {
                if (Validator.IsBlankOrNull(eventEntity.StartIntervalDate) && Validator.IsBlankOrNull(eventEntity.YearQuarter))
                {
                    ValidationDictionary.AddError("StartIntervalDate", LabelResource.StartIntervalDateFrmError);
                    ValidationDictionary.AddError("YearQuarter", LabelResource.YearQuarterFrmError);
                }
                else if (Validator.IsBlankOrNull(eventEntity.StartIntervalDate))
                {
                    ValidationDictionary.AddError("StartIntervalDate", LabelResource.StartIntervalDateFrmError);
                }
                else if (Validator.IsBlankOrNull(eventEntity.YearQuarter))
                {
                    ValidationDictionary.AddError("YearQuarter", LabelResource.YearQuarterFrmError);
                }
            }

            /*Specific Date */
            if (eventEntity.TimeFrameFrm.Contains('D'))
            {
                if (Validator.IsBlankOrNull(eventEntity.StartDateFrm))
                {
                    ValidationDictionary.AddError("StartDateFrm", LabelResource.StartIntervalDateFrmError);
                }
            }

            /*Start date to End Date*/
            if (eventEntity.TimeFrameFrm.Contains("SE"))
            {
                if (Validator.IsBlankOrNull(eventEntity.StartDateFrm) && Validator.IsBlankOrNull(eventEntity.EndDateFrm))
                {
                    ValidationDictionary.AddError("StartDateFrm", LabelResource.StartDateFrmError);
                    ValidationDictionary.AddError("EndDateFrm", LabelResource.EndDateFrmError);
                }
                else if (Validator.IsBlankOrNull(eventEntity.StartDateFrm))
                {
                    ValidationDictionary.AddError("StartDateFrm", LabelResource.StartDateFrmError);
                }
                else if (Validator.IsBlankOrNull(eventEntity.EndDateFrm))
                {
                    ValidationDictionary.AddError("EndDateFrm", LabelResource.EndDateFrmError);
                }
            }

            /*Year*/
            if (eventEntity.TimeFrameFrm.Contains('Y'))
            {
                if (Validator.IsBlankOrNull(eventEntity.StartIntervalDate))
                {
                    ValidationDictionary.AddError("StartIntervalDate", LabelResource.YearFrmError);
                }
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            SetLabels();
            GetDataOfConfiguration(CurrentCompany);
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> eventStatusList = ResourceService.GetAll<EventStatus>();
            IList<UserProfile> eventUserList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<IndustryByHierarchyView> industryList = IndustryService.FindIndustryHierarchy(clientCompany);
            IList<Industry> IndustryCollection = IndustryService.GetIndustryEnabled(clientCompany);
            decimal IndustryCollectionCount = IndustryService.GetIndustryEnabled(clientCompany).Count;
            IList<ResourceObject> eventPeriodList = ResourceService.GetAll<EventTypePeriod>();
            IList<ResourceObject> eventQuarterList = ResourceService.GetAll<EventTypeQuarter>();
            IList<ResourceObject> eventMonthList = ResourceService.GetAllBySortValue<EventTypeMonth>();
            eventMonthList = eventMonthList.OrderBy(resourceObject => resourceObject.Id).ToList();
            IList<ResourceObject> eventConfidenceList = ResourceService.GetAllBySortValue<EventConfidence>();
            IList<ResourceObject> eventSeverityList = ResourceService.GetAllBySortValue<EventSeverity>();
            IList<DateTime> eventYearList = new List<DateTime>();
            for (int i = DateTime.Today.Year; i <= 2029; i++)
            {
                eventYearList.Add(new DateTime(i, 1, 1));
            }
            ViewData["YearQuarterList"] = new SelectList(eventYearList, "Year", "Year");

            ViewData["StatusList"] = new SelectList(eventStatusList, "Id", "Value");
            ViewData["EventOwnerList"] = new SelectList(eventUserList, "Id", "Name");
            ViewData["IndustryIdMultiList"] = new SelectList(IndustryCollection, "Id", "Name");
            ViewData["IndustryIdMultiListCount"] = IndustryCollectionCount;
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["EventPeriodList"] = new SelectList(eventPeriodList, "Id", "Value");
            ViewData["EventQuarterList"] = new SelectList(eventQuarterList, "Id", "Value");
            ViewData["EventMonthList"] = new SelectList(eventMonthList, "Id", "Value");
            ViewData["EventConfidenceList"] = new SelectList(eventConfidenceList, "Id", "Value");
            ViewData["EventSeverityList"] = new SelectList(eventSeverityList, "Id", "Value");
            ViewData["StartDateList"] = new SelectList(new int[] { });
            ViewData["CheckIndustryIds"] = false;
            //SetDefatultEntityToLoadPage(FrontEndPages.EventEdit, "Event:Edit");
            ///ONLY HELP IS TO ALL PAGES OF EVENT
            SetDefaultDataToLoadPage();
        }

        protected override void SetEntityDataToForm(Event eventEntity)
        {
            SetLabels();
            //obtain all industries which are related with this event
            eventEntity.IndustriesIds = obtainIndustriesForEvent(eventEntity.Id);
            string[] selectedValues = eventEntity.IndustriesIds.Split(',');
            var selected = selectedValues;
            IList<Industry> industryList = IndustryService.GetIndustryEnabled(eventEntity.ClientCompany);
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);

            ViewData["StartDateFrm"] = DateTimeUtility.ConvertToString(eventEntity.StartDate, GetFormatDate());
            ViewData["EndDateFrm"] = DateTimeUtility.ConvertToString(eventEntity.EndDate, GetFormatDate());
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(eventEntity.MetaData);

            IList<Library> libraries = LibraryService.GetByEntityId(eventEntity.Id, DomainObjectType.Event);
            ViewData["Libraries"] = libraries == null ? new List<Library>() : libraries;

            SetCascadingData(eventEntity, true);
            eventEntity.OldIndustriesIds = eventEntity.IndustriesIds;
            eventEntity.OldCompetitorsIds = eventEntity.CompetitorsIds;
            eventEntity.OldProductsIds = eventEntity.ProductsIds;            
            eventEntity.OldName = eventEntity.EventName;
            eventEntity.TimeFrameFrm = eventEntity.TimeFrame;
            if (eventEntity.EventTypeId != null)
            {
                decimal idEventType = (decimal)eventEntity.EventTypeId;
                EventType type = EventTypeService.GetById(idEventType);
                eventEntity.EventTypeName = type.Name;
            }
        }

        protected override void SetFormEntityDataToForm(Event eventEntity)
        {
            //obtain all industries which are related with this event
            SetLabels();
            
            string[] selected = eventEntity.IndustriesIds.Split(',');
            string checkedbyHierarchy = ""; 
            IList<IndustryByHierarchyView> industryListByHierarchy = new List<IndustryByHierarchyView>();
            IList<Industry> industryList = new List<Industry>();
            if (eventEntity.CheckIndustryIds)
            {
                if (!eventEntity.ClientCompany.Equals(""))
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy(eventEntity.ClientCompany);
                }
                else
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryListByHierarchy, "Id", "Name", selected);
            }
            else
            {
                if (!eventEntity.ClientCompany.Equals(""))
                {
                    industryList = IndustryService.GetIndustryEnabled(eventEntity.ClientCompany);
                }
                else
                {
                    industryList = IndustryService.GetIndustryEnabled((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);
            }
            ViewData["CheckIndustryIds"] = eventEntity.CheckIndustryIds;
            if (eventEntity.CheckIndustryIds == true)
                checkedbyHierarchy = "true";
            else if (eventEntity.CheckIndustryIds == false)
                checkedbyHierarchy = "false";

            ModelState.SetModelValue("checkedbyHierarchy", new ValueProviderResult(checkedbyHierarchy, checkedbyHierarchy, CultureInfo.InvariantCulture));
            SetCascadingData(eventEntity, false);

            eventEntity.OldName = eventEntity.EventName;
            ModelState.SetModelValue("OldName", new ValueProviderResult(eventEntity.OldName, eventEntity.OldName, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(eventEntity.OldIndustriesIds, eventEntity.OldIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorsIds", new ValueProviderResult(eventEntity.OldCompetitorsIds, eventEntity.OldCompetitorsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldProductsIds", new ValueProviderResult(eventEntity.OldProductsIds, eventEntity.OldProductsIds, CultureInfo.InvariantCulture));

            IList<Library> libraries = LibraryService.GetByEntityId(eventEntity.Id, DomainObjectType.Event);
            ViewData["Libraries"] = libraries == null ? new List<Library>() : libraries;
        }

        protected override void GetFormData(Event eventEntity, FormCollection collection)
        {
            eventEntity.StartDate = DateTimeUtility.ConvertToDate(collection["StartDateFrm"], GetFormatDate());
            eventEntity.EndDate = DateTimeUtility.ConvertToDate(collection["EndDateFrm"], GetFormatDate());
            eventEntity.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
            eventEntity.TimeFrame = eventEntity.TimeFrameFrm;

            eventEntity.Libraries = GetLibrariesForEntity(eventEntity.Id,DomainObjectType.Event,LibraryTypeKeyCode.Event);

            string selectedIndustries = collection["IndustryIds"];
            string selectedCompetitors = collection["CompetitorIds"];
            string selectedProducts = collection["ProductIds"];

            eventEntity.OldIndustriesIds = obtainIndustriesForEvent(eventEntity.Id);
            eventEntity.OldCompetitorsIds = obtainCompetitorsForEvent(eventEntity.Id);
            eventEntity.OldProductsIds = obtainProductsForEvent(eventEntity.Id);

            eventEntity.IndustriesIds = selectedIndustries;
            eventEntity.CompetitorsIds = selectedCompetitors;
            eventEntity.ProductsIds = selectedProducts;
        }

        protected override void SetDefaultEntityDataForSave(Event eventEntity)
        {
            eventEntity.Status = EventStatus.Enable;

            eventEntity.MetaData = eventEntity.EventName + ":" + eventEntity.MetaData;
            if (!string.IsNullOrEmpty(eventEntity.IndustriesIds))
            {
                string[] industriesid = eventEntity.IndustriesIds.Split(',');
                for (int i = 0; i < industriesid.Length; i++)
                {
                    Industry industry = IndustryService.GetById(decimal.Parse(industriesid[i]));
                    eventEntity.MetaData += ":" + industry.Name;
                }
            }
            if (!string.IsNullOrEmpty(eventEntity.CompetitorsIds))
            {
                string[] competitorsId = eventEntity.CompetitorsIds.Split(',');
                for (int j = 0; j < competitorsId.Length; j++)
                {
                    Competitor competitor = CompetitorService.GetById(decimal.Parse(competitorsId[j]));
                    eventEntity.MetaData += ":" + competitor.Name;
                }
            }
            if (!string.IsNullOrEmpty(eventEntity.ProductsIds))
            {
                string[] productsid = eventEntity.ProductsIds.Split(',');
                for (int k = 0; k < productsid.Length; k++)
                {
                    Product product = ProductService.GetById(decimal.Parse(productsid[k]));
                    eventEntity.MetaData += ":" + product.Name;
                }
            }
        }

        protected override void SetUserSecurityAccess(Event eventEntity)
        {
            string securityAccess = UserSecurityAccess.Read;

            if (EventService.HasAccessToEvent(eventEntity, CurrentUser))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void ExecuteActionsAfterToSave(Event eventEntity)
        {
            ActionHistoryService.RecordActionHistory(eventEntity.Id, EntityAction.Created, DomainObjectType.Event, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
        }

        protected override void ExecuteActionsAfterToUpdate(Event eventEntity)
        {
            ActionHistoryService.RecordActionHistory(eventEntity.Id, EntityAction.Updated, DomainObjectType.Event, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
        }

        protected override void SetDefaultDataByPage()
        {
            ViewData["Entity"] = FrontEndPages.Events;
            ViewData["TitleHelp"] = "Event";
        }
        #endregion

        #region Private Methods

        //Block code for keeping Any selected on DropDownList
        private void UpdateDropDownWith<T>(string Label, IList<T> Collection)
        {
            IList<T> TempCollection = new List<T>();

            foreach (T objectCol in Collection)
            {
                if (Label.Equals("Industry"))
                {
                    IndustryByHierarchyView tempInd = (IndustryByHierarchyView)((object)objectCol);
                    if (tempInd.Status.Equals("ENBL"))
                    {
                        TempCollection.Add(objectCol);
                    }
                }
                if (Label.Equals("Competitor"))
                {
                    Competitor tempComp = (Competitor)((object)objectCol);
                    if (tempComp.Status.Equals("ENBL"))
                    {
                        TempCollection.Add(objectCol);
                    }
                }
                if (Label.Equals("Product"))
                {
                    Product tempProd = (Product)((object)objectCol);
                    if (tempProd.Status.Equals("ENBL"))
                    {
                        TempCollection.Add(objectCol);
                    }
                }
            }

            decimal Id = -1;
            T EmptyObject = default(T);
            string sId = Request.Params[Label];
            if (sId == null || sId.Length == 0)
                TempCollection.Insert(0, EmptyObject);
            else
            {
                TempCollection.Insert(0, EmptyObject);
                Id = decimal.Parse(Request.Params[Label]);
            }

            ViewData[Label + "Collection"] = new SelectList(TempCollection, "Id", "Name", Id);
        }

        private void SetCascadingData(Event eventEntity, bool action)
        {
            if (action)//action indicates if we get CompetitorsIds and ProductsIds from entity or from a form(edit action or create action)
            {
                eventEntity.CompetitorsIds = obtainCompetitorsForEvent(eventEntity.Id);
                eventEntity.ProductsIds = obtainProductsForEvent(eventEntity.Id);
            }
            string[] selectedProducts = eventEntity.ProductsIds.Split(',');
            string[] selectedCompetitors = eventEntity.CompetitorsIds.Split(',');

            IList<Competitor> competitorList = new List<Competitor>();
            IList<Product> productList = new List<Product>();

            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");

            string[] idsInd = eventEntity.IndustriesIds.Split(',');
            string clientCompany = (string)Session["ClientCompany"];
            if (idsInd[0].Equals(""))
                idsInd = null;

            if (eventEntity.IndustriesIds != null && !eventEntity.IndustriesIds.Equals(""))
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
            if (eventEntity.CompetitorsIds != null && !eventEntity.CompetitorsIds.Equals(""))
            {
                for (int j = 0; j < selectedCompetitors.Length; j++)
                {
                    IList<Product> tempProductList = ProductService.GetByCompetitor(Convert.ToDecimal(selectedCompetitors[j]));
                    //obtain products by Industry
                    foreach (Product prod in tempProductList)
                    {
                        bool addProduct = false;
                        if (!eventEntity.IndustriesIds.Equals(""))
                        {
                            //Verifying if Products from the Competitor are from Industries too
                            string[] actualIndustries = eventEntity.IndustriesIds.Split(',');
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
                if (eventEntity.ProductsIds == null)
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name");
                else
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name", selectedProducts);
            }

            if (competitorList.Count > 0)
            {
                if (eventEntity.CompetitorsIds == null)
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                else
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedCompetitors);
            }  
            
            if (!string.IsNullOrEmpty(eventEntity.TimeFrame) || !string.IsNullOrEmpty(eventEntity.TimeFrameFrm))
            {
                if (eventEntity.TimeFrame.Equals(EventTypePeriod.Quarter) || eventEntity.TimeFrameFrm.Equals(EventTypePeriod.Quarter))
                {
                    IList<ResourceObject> eventQuarterList = ResourceService.GetAll<EventTypeQuarter>();
                    ViewData["StartDateList"] = new SelectList(eventQuarterList, "Id", "Value");

                    IList<DateTime> eventYearList = new List<DateTime>();
                    for (int i = DateTime.Today.Year; i <= 2029; i++)
                    {
                        eventYearList.Add(new DateTime(i, 1, 1));
                    }
                    ViewData["YearQuarterList"] = new SelectList(eventYearList, "Year", "Year");
                }
                if (eventEntity.TimeFrame.Equals(EventTypePeriod.Month) || eventEntity.TimeFrameFrm.Equals(EventTypePeriod.Month))
                {
                    IList<ResourceObject> eventMonthList = ResourceService.GetAllBySortValue<EventTypeMonth>();
                    ViewData["StartDateList"] = new SelectList(eventMonthList, "Id", "Value");
                }
                else if (eventEntity.TimeFrame.Equals(EventTypePeriod.Year) || eventEntity.TimeFrameFrm.Equals(EventTypePeriod.Year))
                {
                    IList<DateTime> eventYearList = new List<DateTime>();
                    for (int i = DateTime.Today.Year; i <= 2029; i++)
                    {
                        eventYearList.Add(new DateTime(i, 1, 1));
                    }
                    ViewData["StartDateList"] = new SelectList(eventYearList, "Year", "Year");
                }
            }
        }

        private string obtainCompetitorsForEvent(decimal idEvent)
        {
            string ids = null;
            IList<EventCompetitor> lstEventComp = EventCompetitorService.GetByEventId(idEvent);
            int cont = 0;
            foreach (EventCompetitor eventComp in lstEventComp)
            {
                cont++;

                if (lstEventComp.Count == cont)
                {
                    ids = ids + eventComp.Id.CompetitorId.ToString();
                }
                else
                {
                    ids = ids + eventComp.Id.CompetitorId + ",";
                }

            }
            return ids;
        }

        private string obtainProductsForEvent(decimal idEvent)
        {
            string ids = null;
            IList<EventProduct> lstEventProd = EventProductService.GetByEventId(idEvent);
            int cont = 0;
            foreach (EventProduct eventProd in lstEventProd)
            {
                cont++;

                if (lstEventProd.Count == cont)
                {
                    ids = ids + eventProd.Id.ProductId.ToString();
                }
                else
                {
                    ids = ids + eventProd.Id.ProductId + ",";
                }

            }
            return ids;
        }

        private string obtainIndustriesForEvent(decimal idEvent)
        {
            string ids = null;
            IList<EventIndustry> lstEventInd = EventIndustryService.GetByEventId(idEvent);
            int cont = 0;
            foreach (EventIndustry eventInd in lstEventInd)
            {
                cont++;

                if (lstEventInd.Count == cont)
                {
                    ids = ids + eventInd.Id.IndustryId.ToString();
                }
                else
                {
                    ids = ids + eventInd.Id.IndustryId + ",";
                }
            }
            return ids;
        }

        private decimal obtainPrimaryCompetitor(decimal idDeal)
        {
            decimal idPrimary = 0;
            IList<EventCompetitor> lstDealComp = EventCompetitorService.GetByEventId(idDeal);

            foreach (EventCompetitor eventInd in lstDealComp)
            {
                if (eventInd.PrimaryCompetitor.Equals("Y"))
                    idPrimary = eventInd.Id.CompetitorId;
            }
            return idPrimary;
        }

        private decimal obtainPrimaryIndustry(decimal idDeal)
        {
            decimal idPrimary = 0;
            IList<EventIndustry> lstEventComp = EventIndustryService.GetByEventId(idDeal);

            foreach (EventIndustry eventInd in lstEventComp)
            {
                if (eventInd.PrimaryIndustry.Equals("Y"))
                    idPrimary = eventInd.Id.IndustryId;
            }
            return idPrimary;
        }

        private void SetPropertiesToEvent(Event eventObject)
        {
            UserProfile user = UserProfileService.GetById(eventObject.CreatedBy);
            if (user != null)
            { eventObject.CreatedByFrm = user.Name; }
            eventObject.NamesIndustries = IndustryService.GetNamesOfIndustryByEvent(eventObject.Id);
            eventObject.NamesCompetitors = CompetitorService.GetNamesOfCompetitorByEvent(eventObject.Id);
            eventObject.NamesProducts = ProductService.GetNamesOfProductByEvent(eventObject.Id);
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
            string idEvent = Request["idEvent"];
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
            if (result.Length > 0 && result.Substring(result.Length -1 , 1).Equals("_"))
            {
                result = result.Substring(0, result.Length - 1);
            }
            return Content(result);
        }
        #endregion
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
        public ContentResult ChangeIndustryList(bool IsChecked, string IndustryIds)
        {
            string[] tempo = new string[] { };
            var selected = tempo;
            if (!string.IsNullOrEmpty(IndustryIds))
            {
                selected = IndustryIds.Split(',');
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
    }
}
