using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.IO;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Models.Helpers
{
    public static class PanelHelper
    {

        public static string RenderPartialControl(this HtmlHelper helper, string partial)
        {
            using (StringWriter sw = new StringWriter())
            {
                
                ViewEngineResult result = ViewEngines.Engines.FindPartialView(helper.ViewContext, partial);

                if (result.View == null)
                {
                    throw new InvalidOperationException(string.Format("The partial view '{0}' could not be found", partial));
                }

                IView view = result.View;
                view.Render(helper.ViewContext, sw);

                return sw.ToString();
            }
        }

        public static string RenderPanels(this HtmlHelper helper, IList<WebsitePanel> websitepanels, bool DefaultsSocialLog)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder result = new StringBuilder();
            if (websitepanels == null)
                return string.Empty;
            foreach(WebsitePanel wspanel in websitepanels)
            {
                if (wspanel.ComponentName.Equals(WebsitePanelType.SocialLog) && wspanel.Displayable.Equals(WebsiteDetailDisplayable.Yes) && DefaultsSocialLog)
                {
                    result.AppendLine(RenderPartialControl(helper, "SocialLogContent"));
                    //result.Append("<img src='Content/imagebutton.JPG'>");
                }
                else
                    if (wspanel.ComponentName.Equals(WebsitePanelType.ShortSurvey) && wspanel.Displayable.Equals(WebsiteDetailDisplayable.Yes))
                    {
                        result.AppendLine(RenderPartialControl(helper, "ShortSurveyContent"));
                        

                    }
                    else
                        if (wspanel.ComponentName.Equals(WebsitePanelType.Survey) && wspanel.Displayable.Equals(WebsiteDetailDisplayable.Yes))
                        {
                            result.AppendLine(RenderPartialControl(helper, "SurveyContent"));
                           
                        }
            }

           return result.ToString();
        }
    }
}
