using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Resources;
using System.Text;
using System.Collections;

using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Common.Browse;
using Compelligence.Common.Utility.Web;
using Compelligence.Common.Utility.Parser;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Common.Utility;
using System.Linq;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;
using Compelligence.Web.Models.Util;
using Compelligence.Reports.Helpers;
using System.Globalization;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class EventController : BackEndAsyncFormController<Event, decimal>
    {

        #region Public Properties

        public IEventService EventService
        {
            get { return (IEventService)_genericService; }
            set { _genericService = value; }
        }

        private IReportService ReportService { get; set; }
        public IForumResponseService ForumResponseService { get; set; }
        public IForumService ForumService { get; set; }
        public IUserProfileService UserProfileService { get; set; }
        public IIndustryService IndustryService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IProductService ProductService { get; set; }
        public IResourceService ResourceService { get; set; }
        public IEventTypeService EventTypeService { get; set; }
        public IEventCompetitorService EventCompetitorService { get; set; }
        public IEventIndustryService EventIndustryService { get; set; }
        public IEventProductService EventProductService { get; set; }
        public IMarketTypeService MarketTypeService { get; set; }
        public IIndustryProductService IndustryProductService { get; set; }
        public IIndustryCompetitorService IndustryCompetitorService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }

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

            IList<Product> productList = ProductService.GetAllByIndustryAndCompetitor(industryId, id);

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
                    if (id.Equals(EventTypePeriod.Year))
                    {
                        IList<DateTime> eventYearList = new List<DateTime>();
                        for (int i = DateTime.Today.Year; i <= 2029; i++)
                        {
                            eventYearList.Add(new DateTime(i, 1, 1));
                        }
                        return ControllerUtility.GetSelectOptionsFromGenericList<DateTime>(eventYearList, "Year", "Year");
                    }
                    else
                    {
                        IList<DateTime> eventDayList = new List<DateTime>();
                        for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month); i++)
                        {
                            eventDayList.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, i));
                        }
                        return ControllerUtility.GetSelectOptionsFromGenericList<DateTime>(eventDayList, "Day", "Day");//here the day option

                    }
                }
            }
        }


        public JsonResult GetName(decimal id)
        {
            string key = Request["key"];
            Event Event = EventService.GetById(id);
            string companyId = (string)Session["ClientCompany"];
            ClientCompany clientcompany = ClientCompanyService.GetById(companyId);            
            
            return Json(new
            {
                Name = Event.EventName.ToString(),                
                Description = Event.EventDescription,
                Link = Url.Action("Edit", "Events", null,null,clientcompany.Dns + ".compelligence.com") + "/" + Event.Id,                                 
                DateStart = Event.StartDate.ToString(),   
                DateEnd = Event.EndDate.ToString()  
             
            });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateEventReport(FormCollection collection)
        {
            IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();
            BrowseObject browseObject = ReportManager.GetInstance().GetBrowseObject("EventAll");
            browseObject.WhereClause = ExpressionParser.GetExpression(Session, browseObject.WhereClause);

            IList dataSourceObjects = ReportService.GetData(browseObject);
            //IList<UserAllView> dataSource = Collection.ConvertObjectListToGenericList<UserAllView>(dataSourceObjects);

            reportParameters["BrowseObject"] = browseObject;
            //reportParameters["dataSource"] = dataSource;
            reportParameters["DataSource"] = dataSourceObjects;

            string reportFile = ReportHelper.PrintReport(reportParameters);

            //string reportUrl = AppDomain.CurrentDomain.BaseDirectory + "\\Reports\\Out\\" + reportFile + ".pdf";

            return Content(reportFile);
        }

        public ActionResult GetPastEvents()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["ShowPastEvents"] = "all";
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetEventTypes()
        {
            IList<EventType> eventTypeList = EventTypeService.GetAllActiveByClientCompany(CurrentCompany);
            return ControllerUtility.GetSelectOptionsFromGenericList(eventTypeList, "Id", "Name");
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
            else {
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
        protected override void LoadFormData()
        {
            GetDataOfConfiguration(CurrentCompany);
        }

        protected override void SetFormData()
        {            
            SetLabels();
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];
            IList<ResourceObject> eventStatusList = ResourceService.GetAll<EventStatus>();
            IList<UserProfile> eventUserList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = IndustryService.GetAllActiveByClientCompany(clientCompany).Count;
            IList<ResourceObject> eventPeriodList = ResourceService.GetAll<EventTypePeriod>();
            IList<ResourceObject> eventQuarterList = ResourceService.GetAll<EventTypeQuarter>();
            IList<ResourceObject> eventMonthList = ResourceService.GetAllBySortValue<EventTypeMonth>();

            IList<ResourceObject> eventConfidenceList = ResourceService.GetAllBySortValue<EventConfidence>();
            IList<ResourceObject> eventSeverityList = ResourceService.GetAllBySortValue<EventSeverity>();
            
            IList<DateTime> eventYearList = new List<DateTime>();
            IList<MarketType> marketTypeList = MarketTypeService.GetAllSortByClientCompany("Name", clientCompany);
            for (int i = DateTime.Today.Year; i <= 2029; i++)
            {
                eventYearList.Add(new DateTime(i, 1, 1));
            }

            ViewData["StartDateList"] = new SelectList(eventMonthList,"Id","Value");
            ViewData["YearQuarterList"] = new SelectList(eventYearList, "Year", "Year");
            ViewData["StatusList"] = new SelectList(eventStatusList, "Id", "Value");
            ViewData["EventOwnerList"] = new SelectList(eventUserList, "Id", "Name");
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["IndustryIdMultiListCount"] = industryListCount;
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["EventPeriodList"] = new SelectList(eventPeriodList, "Id", "Value");
            ViewData["EventQuarterList"] = new SelectList(eventQuarterList, "Id", "Value");
            ViewData["EventMonthList"] = new SelectList(eventMonthList, "Id", "Value");
            ViewData["EventConfidenceList"] = new SelectList(eventConfidenceList, "Id", "Value");
            ViewData["EventSeverityList"] = new SelectList(eventSeverityList, "Id", "Value");
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;
            ViewData["MarketTypeList"] = new SelectList(marketTypeList, "Id", "Name");
            ViewData["CheckIndustryIds"] = false;
        }

        protected override void SetEntityDataToForm(Event eventEntity)
        {
            //obtain all industries which are related with this event
            eventEntity.IndustriesIds = obtainIndustriesForEvent(eventEntity.Id);
            string[] selectedValues = eventEntity.IndustriesIds.Split(',');
            var selected = selectedValues;
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(eventEntity.ClientCompany);
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);

            //ViewData["TimeFrameFrm"] = DateTimeUtility.ConvertToString(eventEntity.TimeFrame, GetFormatDate());
            ViewData["StartDateFrm"] = DateTimeUtility.ConvertToString(eventEntity.StartDate, GetFormatDate());
            ViewData["EndDateFrm"] = DateTimeUtility.ConvertToString(eventEntity.EndDate, GetFormatDate());
            //ViewData["EventDateActualFrm"] = DateTimeUtility.ConvertToString(eventEntity.DateActual, GetFormatDate());
            //ViewData["EventDateProjectedFrm"] = DateTimeUtility.ConvertToString(eventEntity.DateProjected, GetFormatDate());
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(eventEntity.MetaData);
            ViewData["CreatedByFrm"] = UserProfileService.GetById(eventEntity.CreatedBy).Name;
            SetCascadingData(eventEntity, true);
            eventEntity.OldIndustriesIds = eventEntity.IndustriesIds;
            eventEntity.OldCompetitorsIds = eventEntity.CompetitorsIds;
            eventEntity.OldProductsIds = eventEntity.ProductsIds;
            eventEntity.OldName = eventEntity.EventName;
            eventEntity.TimeFrameFrm = eventEntity.TimeFrame;

            if (eventEntity.EventTypeId != null)
            {
                decimal idEventType = (decimal) eventEntity.EventTypeId;
                EventType type = EventTypeService.GetById(idEventType);
                eventEntity.EventTypeName = type.Name;
            }
        }

        protected override void SetFormEntityDataToForm(Event eventEntity)
        {
            //obtain all industries which are related with this event
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
                    industryList = IndustryService.GetAllActiveByClientCompany(eventEntity.ClientCompany);
                }
                else
                {
                    industryList = IndustryService.GetAllActiveByClientCompany((string)Session["ClientCompany"]);
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
            eventEntity.MetaData = FormFieldsUtility.GetMultilineValue(eventEntity.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(eventEntity.MetaData, eventEntity.MetaData, CultureInfo.InvariantCulture));
            eventEntity.OldName = eventEntity.EventName;
            ModelState.SetModelValue("OldName", new ValueProviderResult(eventEntity.OldName, eventEntity.OldName, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(eventEntity.OldIndustriesIds, eventEntity.OldIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorsIds", new ValueProviderResult(eventEntity.OldCompetitorsIds, eventEntity.OldCompetitorsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldProductsIds", new ValueProviderResult(eventEntity.OldProductsIds, eventEntity.OldProductsIds, CultureInfo.InvariantCulture));
        }

        protected override void GetFormData(Event eventEntity, FormCollection collection)
        {
            eventEntity.OldStartDate = eventEntity.StartDate;
            eventEntity.StartDate = DateTimeUtility.ConvertToDate(collection["StartDateFrm"], GetFormatDate());
            eventEntity.EndDate = DateTimeUtility.ConvertToDate(collection["EndDateFrm"], GetFormatDate());
            eventEntity.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
            eventEntity.TimeFrame = eventEntity.TimeFrameFrm;
            string idType = collection["EventTypeId"];

            IList<Library> libraryItems = new List<Library>();
            string pathtemp = GenericService.GetPathBase("TempFilePath", true);
            //string pathtemp = GetPathTemp();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    string physicalname = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                    physicalname += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');
                    hpf.SaveAs(System.IO.Path.Combine(pathtemp, physicalname));
                    Library library = new Library();

                    library.Name = eventEntity.EventName;
                    library.FileName = System.IO.Path.GetFileName(hpf.FileName);
                    library.FilePhysicalName = physicalname;
                    library.HeaderType = DomainObjectType.Deal;
                    library.EntityType = DomainObjectType.Deal;
                    libraryItems.Add(library);
                }
            }

            eventEntity.Libraries = libraryItems;

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

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Event;

            switch (detailType)
            {
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Event);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Event);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Event);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Event);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Event);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
                case DetailType.Label:
                    AddFilter(detailFilter, "Label.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Label.EntityType", DomainObjectType.Event);
                    AddFilter(browseDetailFilter, "LabelDetailView.EntityId", parentId.ToString());
                    childController = "Label";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Event);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Comment:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Event);
                    childController = "ForumComment";
                    break;
                case DetailType.Feedback:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Event);
                    childController = "ForumFeedback";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Event);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.Trend:
                    AddFilter(detailFilter, "Trend.EventId", parentId.ToString());
                    //AddFilter(detailFilter, "Event.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "TrendEventDetailView.EventId", parentId.ToString());
                    childController = "Trend:TrendEventDetail";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Event events)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (EventService.HasAccessToEvent(events, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion

        #region Private Methods

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
                    ViewData["StartDateList"] = new SelectList(eventMonthList,"Id","Value");
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

        private string obtainCompetitorsForEvent(decimal idKit)
        {
            string ids = null;
            IList<EventCompetitor> lstEventComp = EventCompetitorService.GetByEventId(idKit);
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

        private string obtainProductsForEvent(decimal idKit)
        {
            string ids = null;
            IList<EventProduct> lstEventProd = EventProductService.GetByEventId(idKit);
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

        private string obtainIndustriesForEvent(decimal idKit)
        {
            string ids = null;
            IList<EventIndustry> lstEventInd = EventIndustryService.GetByEventId(idKit);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetProductsOfCompetitor(decimal id)
        {
            string result = string.Empty;
            string idEvent = Request["idEvent"];
            string idsIndustries = Request["idsIndustries"];
            string[] arrayIndustries = idsIndustries.Split(',');

            IList<Product> lstProds = ProductService.GetByCompetitor(id);
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Event entity = EventService.GetById(id);
            if (entity != null) result = entity.EventName;
            return Content(result);
        }
        #endregion

        public ActionResult CreateCalendar()
        {
            int year = DateTime.Today.Year;

            ViewData["Calendars"] = EventService.GetItemsByYear(year, CurrentCompany);
            return View("EventsCalendar");
        }
    }
}
