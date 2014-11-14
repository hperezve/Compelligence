using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Security.Filters;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;

namespace Compelligence.Web.Controllers
{
    public class BackEndController : GenericBackEndController
    {
        public IClientCompanyService ClientCompanyService
        { get; set; }
        #region Action Methods
        //[AuthenticationTypeUserFilter]
        [AuthenticationFilter]
        public ActionResult Index()
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            ViewData["ClientCompany"] = clientCompany;
            return View();
        }
        public ActionResult TestGrid()
        {
            return View();
        }
        #endregion
    }
}
