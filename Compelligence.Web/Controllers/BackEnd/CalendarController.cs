using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using Compelligence.Common.Utility.Web;
using Compelligence.Web.Models.Helpers;
using Compelligence.Web.Models.Web;
using System.Reflection;
//using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class CalendarController : BackEndAsyncFormController<Calendar, decimal>
    {

        #region Public Properties

        public ICalendarService CalendarService
        {
            get { return (ICalendarService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IDealService DealService { get; set; }

        public IKitService KitService { get; set; }

        public IObjectiveService ObjectiveService { get; set; }

        public IPlanService PlanService { get; set; }

        public IProjectService ProjectService { get; set; }

        public IEventService EventService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Calendar calendar, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(calendar.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.CalendarNameRequiredError);
            }

            if (!Validator.IsDate(calendar.DateFrm, GetFormatDate()))
            {
                ValidationDictionary.AddError("DateFrm", LabelResource.CalendarDateRequiredError);
            }

            if (Validator.IsBlankOrNull(calendar.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.CalendarOwnerRequiredError);
            }
            return ValidationDictionary.IsValid;

        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> calendarTypeList = ResourceService.GetAll<CalendarType>();
            IList<ResourceObject> calendarStatusList = ResourceService.GetAll<CalendarStatus>();
            //IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            ViewData["TypeList"] = new SelectList(calendarTypeList, "Id", "Value");
            ViewData["StatusList"] = new SelectList(calendarStatusList, "Id", "Value");
            //ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["EntityName"] = string.Empty;
            ViewData["EntityLinkId"] = string.Empty;
            ViewData["EntityLinkEntity"] = string.Empty;
            ViewData["EntityLinkScope"] = string.Empty;
        }

        protected override void SetEntityDataToForm(Calendar calendarEntity)
        {
            ViewData["DateFrm"] = DateTimeUtility.ConvertToString(calendarEntity.Date, GetFormatDate());
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(calendarEntity.MetaData);
            calendarEntity.OldName = calendarEntity.Name;
            SetCascadingData(calendarEntity);
        }


        protected override void GetFormData(Calendar calendarEntity, FormCollection collection)
        {
            calendarEntity.Date = DateTimeUtility.ConvertToDate(collection["DateFrm"], GetFormatDate());
            calendarEntity.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        protected override void SetDefaultEntityDataForSave(Calendar calendarEntity)
        {
            calendarEntity.MetaData = calendarEntity.Name + ":" + calendarEntity.MetaData;
            if (DateTime.Today > calendarEntity.Date)
            {
                calendarEntity.Status = CalendarStatus.OverDue;
            }
        }

        protected override void SetFormEntityDataToForm(Calendar calendarEntity)
        {
            calendarEntity.OldName = calendarEntity.Name;
            calendarEntity.MetaData = FormFieldsUtility.GetMultilineValue(calendarEntity.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(calendarEntity.MetaData, calendarEntity.MetaData, System.Globalization.CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(calendarEntity.OldName, calendarEntity.OldName, System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(Calendar calendar)
        {
            try
            {
                if (!String.IsNullOrEmpty(calendar.EntityType))
                {
                    if (calendar.EntityType == CalendarType.Deal)
                    {
                        Deal deal = DealService.GetById((decimal)calendar.EntityId);
                        ViewData["EntityName"] = deal.Name;
                        //ViewData["EntityLink"] = "/Deal.aspx/Edit', 'Workspace', 'Deal', "+ deal.Id +", 'DealAll', '#DealContent'";
                        //ViewData["EntityLink"] = "Edit,Deal"  + deal.Id ;
                        //ViewData["EntityLink"] = "Edit,Deal,Workspace, " + deal.Id.ToString() + ",DealAll, #DealContent";
                        ViewData["EntityLinkId"] = deal.Id;
                        ViewData["EntityLinkEntity"] = "Deal";
                        ViewData["EntityLinkScope"] = "Workspace";
                    }
                    else if (calendar.EntityType == CalendarType.Kit)
                    {
                        Kit kit = KitService.GetById((decimal)calendar.EntityId);
                        ViewData["EntityName"] = kit.Name;
                        ViewData["EntityLinkId"] = kit.Id;
                        ViewData["EntityLinkEntity"] = "Kit";
                        ViewData["EntityLinkScope"] = "Environment";
                    }
                    else if (calendar.EntityType == CalendarType.Objective)
                    {
                        Objective objective = ObjectiveService.GetById((decimal)calendar.EntityId);
                        ViewData["EntityName"] = objective.Name;
                        ViewData["EntityLinkId"] = objective.Id;
                        ViewData["EntityLinkEntity"] = "Objective";
                        ViewData["EntityLinkScope"] = "Environment";
                    }
                    else if (calendar.EntityType == CalendarType.Plan)
                    {
                        Plan plan = PlanService.GetById((decimal)calendar.EntityId);
                        if (plan.EntityType.Equals(DomainObjectType.Project))
                        {
                            Project project = ProjectService.GetById((decimal)plan.EntityId);
                            if (project != null)
                            {
                                ViewData["EntityName"] = project.Name;
                            }
                        }
                        else if (plan.EntityType.Equals(DomainObjectType.Deal))
                        {
                            Deal deal = DealService.GetById((decimal)plan.EntityId);
                            if (deal != null)
                            {
                                ViewData["EntityName"] = deal.Name;
                            }
                        }
                        else if (plan.EntityType.Equals(DomainObjectType.Event))
                        { 
                            Event eventEntity = EventService.GetById((decimal)plan.EntityId);
                            if (eventEntity != null)
                            {
                                ViewData["EntityName"] = eventEntity.EventName;
                            }
                        }
                        else if (plan.EntityType.Equals(DomainObjectType.Objective))
                        {
                            Objective objective = ObjectiveService.GetById((decimal)plan.EntityId);
                            if (objective != null)
                            {
                                ViewData["EntityName"] = objective.Name;
                            }
                        }
                        else if (plan.EntityType.Equals(DomainObjectType.Kit))
                        {
                            Kit kit = KitService.GetById((decimal)plan.EntityId);
                            if (kit != null)
                            {
                                ViewData["EntityName"] = kit.Name;
                            }
                        }
                        else
                        {
                            ViewData["EntityName"] = plan.Task;
                        }
                        ViewData["EntityLinkId"] = plan.Id;
                        string pathEntity = DataGridHelper.GetCurrentPath();
                        if (pathEntity.IndexOf("/Calendar.aspx") != -1)
                        {
                            pathEntity = pathEntity.Replace("/Calendar.aspx", string.Empty);
                        }
                       // ViewData["EntityName"] = string.Empty;
                       // ViewData["EntityLinkId"] = string.Empty;
                        ViewData["EntityLinkEntity"] = "Plan";
                        ViewData["EntityLinkScope"] = pathEntity;
                    }
                    else if (calendar.EntityType == CalendarType.Project)
                    {
                        Project project = ProjectService.GetById((decimal)calendar.EntityId);
                        ViewData["EntityName"] = project.Name;
                        ViewData["EntityLinkId"] = project.Id;
                        ViewData["EntityLinkEntity"] = "Project";
                        ViewData["EntityLinkScope"] = "Workspace";
                    }
                    else if (calendar.EntityType == CalendarType.Event)
                    {
                        Event eventEntity = EventService.GetById((decimal)calendar.EntityId);
                        ViewData["EntityName"] = eventEntity.EventName;
                        ViewData["EntityLinkId"] = eventEntity.Id;
                        ViewData["EntityLinkEntity"] = "Event";
                        ViewData["EntityLinkScope"] = "Workspace";
                    }
                    else
                    {
                        ViewData["EntityName"] = string.Empty;
                        ViewData["EntityLinkId"] = string.Empty;
                        ViewData["EntityLinkEntity"] = string.Empty;
                        ViewData["EntityLinkScope"] = string.Empty;
                    }
                }
                else
                {
                    ViewData["EntityName"] = string.Empty;
                    ViewData["EntityLinkId"] = string.Empty;
                    ViewData["EntityLinkEntity"] = string.Empty;
                    ViewData["EntityLinkScope"] = string.Empty;
                }
            }
            catch
            {
                ViewData["EntityName"] = "This object was deleted";
            }
        }

        #endregion
        

        protected override void SetUserSecurityAccess(Calendar calendar)
        {
            string securityAccess = UserSecurityAccess.Read;
            if (CalendarService.HasAccessToCalendar(calendar))
            {
                securityAccess = UserSecurityAccess.Edit;
            }
            else
            {
                ViewData["FormHeaderMessage"] = "View";
            }
            ViewData["UserSecurityAccess"] = securityAccess;
        }
        public ActionResult CreateCalendar()
        {

            int year = DateTime.Today.Year;

            ViewData["Calendars"] = CalendarService.GetItemsByYear(year, CurrentUser, CurrentCompany);
            return View("CreateCalendar");
        }

        public ActionResult GetItems()
        {
            var objectResult = new object[0];

            try
            {

                String datemount = Request["datemount"];
                String dateyear = Request["dateyear"];

                IList<Calendar> items = CalendarService.GetItemsByDateCalendar(datemount, dateyear, CurrentCompany);

                objectResult = new object[items.Count + 1];
                int index = 0;


                foreach (Calendar item in items)
                {


                    objectResult[index++] = new
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name
                    };
                }

            }
            catch
            {

            }
            return Json(objectResult);

        }

   
        //public JsonResult GetItems(decimal id)
        //{
        //    Calendar calendar = CalendarService.GetById(id);

        //    return Json(new
        //    {
        //        Name = calendar.Name.ToString(),
        //        Date = calendar.Date.ToString()
        //    });
        //}


               
    }
}
