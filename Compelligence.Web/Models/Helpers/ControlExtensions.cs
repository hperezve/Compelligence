using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Compelligence.Web.Models.Helpers
{
    public static class ControlExtensions
    {
        public static string RenderHtml(this Control control)
        {
            var sb = new StringBuilder();
            var hw = new HtmlTextWriter(new StringWriter(sb));

            control.RenderControl(hw);
            return sb.ToString();
        }

    }
}
