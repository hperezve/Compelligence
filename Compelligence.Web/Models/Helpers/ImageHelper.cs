using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Helpers
{
    public static class ImageHelper
    {
        public static string ImageUrl(this HtmlHelper helper, string pathfile)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);

            string fullpath = AppDomain.CurrentDomain.BaseDirectory + pathfile;

            if (pathfile.IndexOf("http://")==0) //if pathfile is URL
                return pathfile;

            if ((!String.IsNullOrEmpty(pathfile)) && System.IO.File.Exists(fullpath)) //is pathfile is local
            {
               // return fullpath;
                return url.Content("~/" + pathfile);
            }
            return url.Content("~/Content/Images/Icons/none.png"); // remove path to resource
        }
    }
}
