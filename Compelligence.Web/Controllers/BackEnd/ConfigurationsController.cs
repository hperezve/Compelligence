using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Compelligence.Web.Controllers
{
    public class ConfigurationsController : GenericBackEndController
    {
        //
        // GET: /Configurations/

        public ActionResult Index()
        {
            ViewData["Scope"] = Request["Scope"];
            return View();
        }

    }
}
