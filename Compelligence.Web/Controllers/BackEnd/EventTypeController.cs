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
using System.Text;


namespace Compelligence.Web.Controllers
{
    public class EventTypeController : BackEndAsyncFormController<EventType, decimal>
    {

        #region Public Properties

        public IEventTypeService EventTypeService
        {
            get { return (IEventTypeService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(EventType EventType, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(EventType.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.EventTypeNameRequiredError);
            }
            return ValidationDictionary.IsValid;
        }

        protected override void SetFormData()
        {
            IList<ResourceObject> statusList = ResourceService.GetAll<EventTypeStatus>();
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
        }

        #endregion

    }
}
