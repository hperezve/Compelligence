using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Compelligence.Domain.Entity;
using Compelligence.Security.Managers;
using System.Collections.Generic;
using System.Reflection;

namespace Compelligence.Web.Models.Helpers
{
    public static class HtmlSelectHelper
    {
        public static String SelectList(this HtmlHelper helper, String name, Boolean multiple, String size, IEnumerable<SelectListItem> items, String options)
        {
            String Multiple = "MULTIPLE";
            if (!multiple)
            {
                Multiple = "";
            }
            StringBuilder Script= new StringBuilder("");
            Script.Append(" <SELECT ID=\"" + name + "\" NAME=\"" + name + "\" " + Multiple + " SIZE=\"" + size + "\" " + options + " >    ");

            if (items != null)
            {
                foreach (SelectListItem item in items)
                {
                    String Selected = "";
                    if (item.Selected)
                    {
                        Selected = " selected ";
                    }
                    Script.Append(" <OPTION " + Selected + "VALUE=\"" + item.Value + "\">" + item.Text + "</OPTION>   ");
                }
            }

            Script.Append(" </SELECT>   ");
                   
            return Script.ToString();
        }

        public static string BuildHeaderSingleScript(this HtmlHelper helper, string nameObject)
        {
            StringBuilder buildHeaderSingleScript = new StringBuilder();
            /* sobre le plugin anterior
            buildHeaderSingleScript.AppendLine("$(" + nameObject + ").multiselect({");
            buildHeaderSingleScript.AppendLine("multiple: false,");
            buildHeaderSingleScript.AppendLine("header: 'Select an option',");
            buildHeaderSingleScript.AppendLine("noneSelectedText: 'Select an Option',");
            buildHeaderSingleScript.AppendLine("selectedList: 1");
            buildHeaderSingleScript.AppendLine("});");
            */
            buildHeaderSingleScript.Append("<script type='text/javascript' >" +
                "$(function() {" +
                "$('.ui-multiselect')" +
                   "$(this).focus(function() {" +
                        "alert('aqui estoy')" +
                        "$(this).css('ui-state-hover'));" +
                    "})" +
                    "$('#span" + nameObject + "').click(function() {" +
                        "$('#divList" + nameObject + "').show();" +
                    "});" +
                 "});</script>");

            return buildHeaderSingleScript.ToString();

        }

        //public static string DropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes);
        public static string BuildMultipleSelectScript(this HtmlHelper htmlHelper, string nameObject, string nameOption, int widthObject, IEnumerable<SelectListItem> items, object htmlAttributes)
        //public static string BuildMultipleSelectScript(this HtmlHelper htmlHelper, string nameObject, string nameOption, IList<Industry> items)
        {
            string name = "";
            string value = "";

            UrlHelper url = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            StringBuilder SelectRoot = new StringBuilder();

            /*
             SelectRoot.Append(" <script type='text/javascript' >");
            var ui_multiselect = "ui-multiselect-menu";*/
            //SelectRoot.AppendLine(BuildHeaderSingleScript(htmlHelper, nameObject));

            var initializeMessage = "Select an Option";
            var button = "<button id='button" + nameObject + "' type='button' class='ui-multiselect ui-widget ui-state-default ui-corner-all' title='' aria-haspopup='true' style='width:" + widthObject + "px;'>" +
                         "<span class='ui-icon ui-icon-triangle-2-n-s'></span>" +
                         "<span>" + initializeMessage + "</span></button>";

            var divHeader = "<div id='divPadre" + nameObject + "' class='ui-multiselect-menu ui-widget ui-widget-content ui-corner-all ui-multiselect-single',style='width:" + widthObject + "px'>";
            var divButton = "<div class='ui-widget-header ui-corner-all ui-multiselect-header ui-helper-clearfix'>" +
                    "<ul class='ui-helper-reset'><li>" + initializeMessage + "</li>" +
                    "<li class='ui-multiselect-close'><a href='#' class='ui-multiselect-close'>" +
                    "<span class='ui-icon ui-icon-circle-close'  id='cerrarSpan" + nameObject + "'></span></a></li></ul></div>";

            var ulSelect = "<ul id='listado" + nameObject + "' class='ui-multiselect-checkboxes ui-helper-reset' style='height: 175px;'>";

            SelectRoot.Append(button);
            SelectRoot.Append(divHeader);
            SelectRoot.Append(divButton);
            SelectRoot.Append(ulSelect);
            if (htmlAttributes != null)
            {
                Type type = htmlAttributes.GetType();

                foreach (PropertyInfo property in type.GetProperties())
                {
                    name = property.Name.ToLower();
                    value = (string)property.GetValue(htmlAttributes, null);
                }
            }
            //Type type = htmlAttributes.GetType();
            //PropertyInfo verificar = type.GetProperty("onblick");
            //string name=verificar.Name.ToLower();
            foreach (SelectListItem item in items)
            {
                var childListOption = "<li class='listadoSelect'> " +
                    "<label for='" + item.Text + "' class='ui-corner-all'>" + item.Text +
                    "<input id='value" + nameObject + "' type='text' value='" + item.Value + "'/></label>" +
                    "</li>";
                /*
                var childListOption = "<li " + name + "=" + value + "' class='listadoSelect'> "+
                    "<label for='" + item.Text + "' class='ui-corner-all'>" + item.Text + 
                    "<input id='value"+nameObject+"' type='text' value='"+item.Value+"'/></label>"+
                    "</li>";*/

                SelectRoot.Append(childListOption);

            }
            SelectRoot.Append("</ul>");
            SelectRoot.Append("</div>");
            SelectRoot.Append("</div>");



            return SelectRoot.ToString();
        }

    }
    public class Listado
    {
        public int idOption { get; set; }
        public int nameOption { get; set; }
    }
}
