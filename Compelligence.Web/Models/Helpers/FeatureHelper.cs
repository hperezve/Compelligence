using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Helpers
{
    public static class FeatureHelper
    {
        public static string FeatureInput(this HtmlHelper helper, string[] features, string value,string count, string onclick)
        {
            StringBuilder result = new StringBuilder();
            string tag = string.Empty;

            foreach(string cf in features)
            {
                if (cf.ToUpper().Equals(value.ToUpper()))
                {
                    tag = "checked";
                    break;
                }
            }
            result.Append("<input type='checkbox' id='chkmarketdfeature'  onclick=\"" + onclick + "\" value='" + value + "'  "+tag+" "+(count.Equals("0") ? "disabled" : string.Empty )+"/>");
            return result.ToString();
        }
    }
}

