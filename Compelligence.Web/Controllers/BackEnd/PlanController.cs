using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class PlanController : BackEndAsyncFormController<Plan, decimal>
    {

        #region Public Properties

        public IPlanService PlanService
        {
            get { return (IPlanService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Plan plan, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(plan.Task))
            {
                ValidationDictionary.AddError("Task", LabelResource.PlanTaskRequiredError);
            }

            if (Validator.IsBlankOrNull(plan.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.PlanAssignedToRequiredError);
            }

            //if (Validator.IsBlankOrNull(plan.Detail))
            //{
            //    ValidationDictionary.AddError("Detail", LabelResource.PlanDetailRequiredError);
            //}

            if (Validator.IsBlankOrNull(plan.DueDateFrm))
            {
                ValidationDictionary.AddError("DueDateFrm", LabelResource.PlanDueDateRequiredError);
            }

            if (!(Validator.IsBlankOrNull(plan.DueDateFrm) || Validator.IsDate(plan.DueDateFrm, GetFormatDate())))
            {
                ValidationDictionary.AddError("DueDateFrm", string.Format(LabelResource.PlanDueDateFormatError, GetFormatDate()));
            }

            return ValidationDictionary.IsValid;

        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];

            IList<ResourceObject> planTypeList = ResourceService.GetAll<PlanType>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);


            ViewData["TypeList"] = new SelectList(planTypeList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;

        }

        protected override void SetEntityDataToForm(Plan plan)
        {
            ViewData["DueDateFrm"] = DateTimeUtility.ConvertToString(plan.DueDate, GetFormatDate());
            ViewData["CreatedByFrm"] = UserProfileService.GetById(plan.CreatedBy).Name;
        }

        protected override void SetFormEntityDataToForm(Plan plan)
        {
            //base.SetFormEntityDataToForm(formEntity);
        }

        protected override void GetFormData(Plan plan, FormCollection collection)
        {
            plan.DueDate = DateTimeUtility.ConvertToDate(collection["DueDateFrm"], GetFormatDate());
        }

        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetDataEntity(decimal id)
        {
            string result = string.Empty;
            string EntityName = ResourceService.GetName<DomainObjectType>(PlanService.GetById(id).EntityType);
            string EntityId = PlanService.GetById(id).EntityId.ToString();
            string MainTab = "";
            if (EntityName.Equals("Project") || EntityName.Equals("Deal") || EntityName.Equals("Event"))
            {
                MainTab = "Workspace";
            }
            if (EntityName.Equals("Objective") || EntityName.Equals("Kit") || EntityName.Equals("Industry") || EntityName.Equals("Competitor"))
            {
                MainTab = "Environment";
            }
            string path = obtainPath();
            result = EntityName + "_" + EntityId + "_" + MainTab + "_" + path;
            return Content(result);
        }

        public static string obtainPath()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            string path = "";
            bool start = false;
            int initPos = 0;
            int endPos = 0;
            int begin = 7;
            if (currentUrl.IndexOf("https://") != -1) { begin++; }
            for (int i = begin; i < currentUrl.Length; i++)
            {
                string actualChar = currentUrl[i].ToString();

                if (start == false && actualChar.Equals("/"))
                {
                    start = true;
                    initPos = i;
                    break;
                }

            }
            endPos = currentUrl.IndexOf("/Plan.aspx");
            path = currentUrl.Substring(initPos, endPos - initPos);
            return path;
        }

        #endregion
    }
}
