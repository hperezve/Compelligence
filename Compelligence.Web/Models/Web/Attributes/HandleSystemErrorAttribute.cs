using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Web.Attributes
{
    public class HandleSystemErrorAttribute : HandleErrorAttribute
    {
        public bool ShowStackTraceIfNotDebug { get; set; }

        //Next lines need comments 
        public override void OnException(ExceptionContext filterContext)
        {
        }
    }
}
