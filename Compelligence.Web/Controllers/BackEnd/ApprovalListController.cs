using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;

namespace Compelligence.Web.Controllers
{
    public class ApprovalListController : BackEndAsyncFormController<ApprovalList, decimal>
    {
        #region Public Properties

        private IEmailService _emailService;

        public IApprovalListService ApprovalListService
        {
            get { return (IApprovalListService)_genericService; }
            set { _genericService = value; }
        }

        public IEmailService EmailService
        {
            get { return _emailService; }
            set { _emailService = value; }
        }
        
        #endregion

        public ActionResult ReSendEmailToUser(decimal ProjectId, string UserIdToSend)
        {
            EmailService.SendProjectAppovalEmailByUser(ProjectId, UserIdToSend);
            return null;
        }
    }
}
