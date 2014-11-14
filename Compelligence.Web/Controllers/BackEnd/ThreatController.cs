using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Util.Type;
using Compelligence.Util.Validation;
using Resources;

namespace Compelligence.Web.Controllers
{
    public class ThreatController : BackEndAsyncFormController<Threat, decimal>
    {
       public IThreatService ThreatService
        {
            get { return (IThreatService)_genericService; }
            set { _genericService = value; }
        }

       protected override bool ValidateFormData(Threat threat, FormCollection formCollection)
       {
           if (Validator.IsBlankOrNull(threat.Name))
           {
               ValidationDictionary.AddError("Name", LabelResource.ThreatNameRequiredError);
           }
           return ValidationDictionary.IsValid;
       }

        
        protected override void SetEntityDataToForm(Threat threat)
       {
           ViewData["StartDateFrm"] = DateTimeUtility.ConvertToString(threat.StartDate, GetFormatDate());
           ViewData["EndDateFrm"] = DateTimeUtility.ConvertToString(threat.EndDate, GetFormatDate());
       }

       protected override void GetFormData(Threat threat, FormCollection collection)
       {
           threat.StartDate = DateTimeUtility.ConvertToDate(collection["StartDateFrm"], GetFormatDate());
           threat.EndDate = DateTimeUtility.ConvertToDate(collection["EndDateFrm"], GetFormatDate());
       }
    }
}
