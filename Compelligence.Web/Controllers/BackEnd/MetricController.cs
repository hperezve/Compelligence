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
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class MetricController : BackEndAsyncFormController<Metric, decimal>
    {

        #region Public Properties

        public IMetricService MetricService
        {
            get { return (IMetricService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Metric metric, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(metric.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.MetricNameRequiredError);
            }

            return ValidationDictionary.IsValid;

        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> metricTypeList = ResourceService.GetAll<MetricType>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            ViewData["TypeList"] = new SelectList(metricTypeList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["Actual"] = 0;
        }

        protected override void SetEntityDataToForm(Metric metric)
        {
            ViewData["StartDateFrm"] = DateTimeUtility.ConvertToString(metric.StartDate, GetFormatDate());
            ViewData["EndDateFrm"] = DateTimeUtility.ConvertToString(metric.EndDate, GetFormatDate());
            ViewData["Actual"] = metric.Actual;
        }

        protected override void GetFormData(Metric metric, FormCollection collection)
        {
            metric.StartDate = DateTimeUtility.ConvertToDate(collection["StartDateFrm"], GetFormatDate());
            metric.EndDate = DateTimeUtility.ConvertToDate(collection["EndDateFrm"], GetFormatDate());
        }

        #endregion
    }
}
