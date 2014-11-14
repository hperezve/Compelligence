using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Compelligence.Web.Controllers
{
    public class CriteriasController : GenericBackEndController
    {
        #region Action Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
