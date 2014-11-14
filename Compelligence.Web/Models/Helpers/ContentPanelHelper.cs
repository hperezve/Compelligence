using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compelligence.DataTransfer.FrontEnd;
using System.Text;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Helpers
{
    public static class ContentPanelHelper
    {
        public static string HtmlEncode(this HtmlHelper helper, string source)
        {
            return HttpContext.Current.Server.HtmlEncode(source);
        }
        public static string HtmlEncode(this HtmlHelper helper, string source, string tag)
        {
            if (source.IndexOf(tag) > -1)
            {
                source = source.Replace(tag, @"\" + tag);
            }

            return HttpContext.Current.Server.HtmlEncode(source);
        }

        public static string ContentPanels(this HtmlHelper helper)
        {
            StringBuilder contentPanels = new StringBuilder();
            IList<LibraryCatalog> libraryCatalog = (IList<LibraryCatalog>)helper.ViewData["LibraryCatalog"];

            contentPanels.Append("<script type=\"text/javascript\">");
            //contentPanels.Append("  var tools = [{");
            //contentPanels.Append("      id:'close',");
            //contentPanels.Append("      handler: function(e, target, panel){");
            //contentPanels.Append("          panel.ownerCt.remove(panel, true);");
            //contentPanels.Append("      }");
            //contentPanels.Append("  }];");
            contentPanels.Append("  Ext.onReady(function(){");
            contentPanels.Append("     var contentPanel = new Ext.Panel({");
            contentPanels.Append("     layout: 'column',");
            contentPanels.Append("     id:'panelContent',");
            contentPanels.Append("     margins:'5 5 5 5',");
            contentPanels.Append("     renderTo: 'MainContent',");
            contentPanels.Append("     autoScroll: true,");
            contentPanels.Append("     split: true,");
            //contentPanels.Append("     collapsible: true,");
            //contentPanels.Append("     layoutConfig: {columns:2},");
            contentPanels.Append("     baseCls:'x-plain',");
            contentPanels.Append("     defaults: {frame:true, columnWidth:.50, height:300 },");
            //contentPanels.Append("     bodyStyle: \"background-color:#eeeeee !important\",");
            contentPanels.Append("      bodyStyle:'float:left;',");
            contentPanels.Append("     items:[");
            string subBlock = string.Empty;
            for (int i = 0; i < libraryCatalog.Count; i++)
            {
                //if (libraryCatalog[i].Displayable && libraryCatalog[i].Projects != null)
                if (libraryCatalog[i].Displayable)
                {
                    if ((libraryCatalog[i].Projects.Count != 0)) //  (string.IsNullOrEmpty)))
                    {
                        subBlock+=("          {");
                        subBlock += ("          title: '" + HtmlEncode(helper, libraryCatalog[i].Description, "'") + "',");
                        subBlock += ("          split: true,");
                        subBlock += ("          html: $('#contentPanel" + i + "').html(),");
                        subBlock += ("          columnWidth:" + libraryCatalog[i].Width + ",");
                        subBlock += ("           height:285,");
                        subBlock += ("          bodyStyle:'padding:4px 4px 4px 4px',");
                        subBlock += ("          autoScroll: true");
                        subBlock += ("        }");
                        subBlock += ",";
                    }
                }
            }
            if (subBlock.Length!=null && subBlock.Length > 0 )
             subBlock = subBlock.Substring(0, subBlock.Length - 1);
            
            contentPanels.AppendLine(subBlock);
            contentPanels.Append("      ]");
            contentPanels.Append("    });");
            contentPanels.Append("});");
            contentPanels.Append("</script>");

            return contentPanels.ToString();
        }
    }
}
