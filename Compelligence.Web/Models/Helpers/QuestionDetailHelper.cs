using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Compelligence.Web.Models.Helpers
{
    public static class QuestionDetailHelper
    {
        public static string QuestionOptionRow(this HtmlHelper helper, string formId, int index, string firstLabel, string firstValue, string secondLabel, string secondValue)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder result = new StringBuilder();

            result.Append("<div class='line' id='divLineResponse" + index + "'>");
            result.Append(" <div class='field'>");
            result.Append("     <label for='" + formId + "ResponseValue" + index + "'>" + firstLabel + " :</label>");
            result.Append("     <input type='text' value='" + firstValue + "' name='ResponseValue" + index + "' id='" + formId + "ResponseValue" + index + "'>");
            result.Append(" </div>");
            result.Append(" <div class='field'>");
            result.Append("     <label for='" + formId + "ResponseText" + index + "'>" + secondLabel + " :</label>");
            result.Append("     <input type='text' value='" + secondValue + "' name='ResponseText" + index + "' id='" + formId + "ResponseText" + index + "'>");
            result.Append(" </div>");
            result.Append("</div>");
            return result.ToString();
        }
    }
}
