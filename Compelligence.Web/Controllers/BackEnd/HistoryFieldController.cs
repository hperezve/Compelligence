using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    public class HistoryFieldController : BackEndAsyncFormController<HistoryField, decimal>
    {
        public IHistoryFieldService HistoryFieldService
        {
            get { return (IHistoryFieldService)_genericService; }
            set { _genericService = value; }
        }
        public IUserActionFieldService UserActionFieldService { get; set; }
        public IUserProfileService UserProfileService { get; set; }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateFieldChanges(string Note, string Ids)
        {
            string[] Id = Ids.Split(',');
            foreach (string id in Id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    UserActionField userActionField = UserActionFieldService.GetById(Convert.ToDecimal(id)); ;
                    userActionField.Notes = Note;
                    UserActionFieldService.Update(userActionField);
                }
            }            
            return Content(string.Empty);
        }
    }
}
