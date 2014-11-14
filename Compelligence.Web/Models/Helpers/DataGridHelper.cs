using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Mvc;
using Resources;
using System.IO;
using Compelligence.Common.Browse;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Compelligence.Util.Type;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity.Resource;
using Compelligence.DataAccess.Resource;
using Compelligence.DataAccess.Interface;
using System.Text.RegularExpressions;
using Compelligence.Web.Models.Web;
using Compelligence.Security.Managers;

namespace Compelligence.Web.Models.Helpers
{
    [ValidateInput(false)]
    public static class DataGridHelper
    {
        //string labeltest = (string) HttpContext.GetGlobalResourceObject("LabelResource", "ProjectName");
        //string path = HttpContext.Current.Request.ApplicationPath;
        //string a = HttpContext.Current.Request.FilePath;
        //string b = HttpContext.Current.Request.Path;
        //string c = HttpContext.Current.Request.PhysicalApplicationPath;
        //string d = HttpContext.Current.Request.PhysicalPath;

        // string path1 = c + xmlBrowsePath;

        public static IDictionary<string, string> Parameters = new Dictionary<string, string>();

        /// <summary>
        /// DataGrid helper generated only browseId
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="browseId"></param>
        /// <returns></returns>
        public static string DataGrid(this HtmlHelper helper, string browseId)
        {
            return DataGrid(helper, browseId, browseId, null);
        }

        public static string DataGrid(this HtmlHelper helper, string browseId, IDictionary<string, string> defaultParamCriteria)
        {
            return DataGrid(helper, browseId, browseId, null, null, defaultParamCriteria);
        }

        public static string DataGrid(this HtmlHelper helper, string browseId, string controller)
        {
            return DataGrid(helper, browseId, controller, string.Empty, null, null);
        }

        public static string DataGrid(this HtmlHelper helper, string browseId, string controller, string container)
        {
            return DataGrid(helper, browseId, controller, container, null, null);
        }

        public static string DataGrid(this HtmlHelper helper, string browseId, string controller, string container, object defaultFilterCriteria)
        {
            return DataGrid(helper, browseId, controller, container, defaultFilterCriteria, null);
        }

        public static string DataGrid(this HtmlHelper helper, string browseId, object defaultFilterCriteria)
        {
            return DataGrid(helper, browseId, browseId, string.Empty, defaultFilterCriteria, null);
        }

        public static string DataGrid(this HtmlHelper helper, string browseId, string container, object defaultFilterCriteria)
        {
            return DataGrid(helper, browseId, browseId, container, defaultFilterCriteria, null);
        }

        public static string DataGrid(this HtmlHelper helper, string browseId, string controller, string container, object defaultFilterCriteria, IDictionary<string, string> defaultParamCriteria)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string componentPrefix = scope + browseId;
            string tableId = componentPrefix + "ListTable";
            string pagerId = componentPrefix + "ListPager";
            string columnPopupId = componentPrefix + "ColumnPopup";
            string gridScript = BuildGridScript(helper, url, browseObject, scope, controller, container, defaultFilterCriteria, defaultParamCriteria, tableId, pagerId, columnPopupId);
            StringBuilder urlConfig = new StringBuilder(url.Action("saveConfig", "Browse"));
            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];
            string columnPopup = BuildColumnPopup(browseObject, columnPopupId, tableId,userId,urlConfig);

            var tableTagBuilder = new TagBuilder("table");
            tableTagBuilder.GenerateId(tableId);
            tableTagBuilder.AddCssClass("scroll");
            tableTagBuilder.MergeAttribute("cellpadding", "0");
            tableTagBuilder.MergeAttribute("cellspacing", "0");

            var pagerTagBuilder = new TagBuilder("div");
            pagerTagBuilder.GenerateId(pagerId);
            pagerTagBuilder.AddCssClass("scroll");
            pagerTagBuilder.MergeAttribute("style", "text-align: center;");

            string Result = gridScript + columnPopup + tableTagBuilder.ToString() + pagerTagBuilder.ToString();
            return Result;
        }

        public static string DataGridPositioning(this HtmlHelper helper, string browseId, string browseHidden, string controller, string container, object defaultFilterCriteria, IDictionary<string, string> defaultParamCriteria)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
            BrowseObject browseHiddenObject = BrowseManager.GetInstance().GetBrowseObject(browseHidden);
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string componentPrefix = scope + browseId;
            string componentPrefixHidden = scope + browseHidden;
            string tableId = componentPrefix + "ListTable";
            string tableIdHidden = componentPrefixHidden + "ListTable";
            string pagerId = componentPrefix + "ListPager";
            string columnPopupId = componentPrefix + "ColumnPopup";
            string gridScript = BuildGridScript(helper, url, browseObject, scope, controller, container, defaultFilterCriteria, defaultParamCriteria, tableId, pagerId, columnPopupId);
            string columnPopup = BuildColumnPopupPositioning(browseObject, browseHiddenObject, columnPopupId, tableId, tableIdHidden);

            var tableTagBuilder = new TagBuilder("table");
            tableTagBuilder.GenerateId(tableId);
            tableTagBuilder.AddCssClass("scroll");
            tableTagBuilder.MergeAttribute("cellpadding", "0");
            tableTagBuilder.MergeAttribute("cellspacing", "0");

            var pagerTagBuilder = new TagBuilder("div");
            pagerTagBuilder.GenerateId(pagerId);
            pagerTagBuilder.AddCssClass("scroll");
            pagerTagBuilder.MergeAttribute("style", "text-align: center;");

            string Result = gridScript + columnPopup + tableTagBuilder.ToString() + pagerTagBuilder.ToString();
            return Result;
        }

        public static string GridFilter(this HtmlHelper helper, string browseId)
        {
            return GridFilter(helper, browseId, true);
        }

        public static string GridFilter(this HtmlHelper helper, string browseId, bool modalDialog)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string dialogTitle = "FilterDialog"; //(string)HttpContext.GetGlobalResourceObject("LabelResource", browseObject.DefaultEntity + "Filter");
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;

            bool haveDate = BrowseHaveDate(browseObject);
            bool haveStanard = BrowseHaveStandard(browseObject);
            string attributesOfColumnFilter = GetAttributesOfFilterColumn(browseObject);

            if (modalDialog)
            {
                gridFilterScript.Append("<script type=\"text/javascript\">");
                gridFilterScript.Append(" $(function(){ ");
                gridFilterScript.Append("   $('#" + namePrefix + "FilterDialog').dialog({ ");
                gridFilterScript.Append("       bgiframe: true, ");
                gridFilterScript.Append("       autoOpen: false, ");
                gridFilterScript.Append("       modal: true ");
                gridFilterScript.Append("   }); ");


                
                gridFilterScript.Append(" }); ");
                gridFilterScript.Append("</script>");
            }

            gridFilterScript.Append("<div id='" + namePrefix + "FilterDialog' title='" + dialogTitle + "' style='overflow-x:hidden'>");
            if (haveDate || haveStanard)
            {
                gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeWithParametersFilter('" + scope + "', '" + browseId + "', '" + attributesOfColumnFilter + "',this);return false;\" method='post'>");
            }
            else
            {
                gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeFilter('" + scope + "', '" + browseId + "');return false;\" method='post'>");
            }
            gridFilterScript.Append("<fieldset>");
            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            gridFilterScript.AppendLine(BuildSelectFilterScript(browseObject, scope));
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");

            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            gridFilterScript.AppendLine(BuildSelectOperatorScript(88, browseId, scope));
            if (haveDate)
            {
                gridFilterScript.AppendLine(BuildSelectOperatorDateScript(89, scope, browseId));
            }
            if (haveStanard)
            {
                gridFilterScript.AppendLine(BuildSelectOperatorStandardScript(90, scope, browseId));
            }
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");

            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            //if (gridFilterScript.ToString().IndexOf("changeOptions") != -1)
            //{
            gridFilterScript.AppendLine(GetLabelToInputText("filterValue", browseObject.BrowseId, scope));
            gridFilterScript.AppendLine(GetInputTextWithEvents(scope + browseId + "filterValue", browseObject, scope));
            //}
            //else
            //{
            //    gridFilterScript.AppendLine(GetInputText(scope+browseId+"filterValue"));
            //}
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");
            if (haveStanard)
            {
                IDictionary<string, string> dataList = GetListStandard(browseObject);
                foreach (KeyValuePair<string, string> data in dataList)
                {
                    gridFilterScript.Append("<div class='lineShort'>");
                    gridFilterScript.Append("<div class='fieldShort'>");
                    gridFilterScript.AppendLine(BuildSelectFilterValue(data.Key, data.Value, scope, browseId));
                    gridFilterScript.Append("</div>");
                    gridFilterScript.Append("</div>");
                }
            }
            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            gridFilterScript.AppendLine(GetSubmitButton("Filter"));
            if (gridFilterScript.ToString().IndexOf("changeOptions") != -1)
            {
                gridFilterScript.AppendLine(GetResetButtonAll(namePrefix + "FilterForm", scope, browseId, attributesOfColumnFilter));
            }
            else
            {
                gridFilterScript.AppendLine(GetResetButton(namePrefix + "FilterForm"));
            }
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");

            gridFilterScript.Append("</fieldset>");
            gridFilterScript.Append("</form>");
            gridFilterScript.Append("</div>");
            return gridFilterScript.ToString();
        }

        public static string GridFilterAlter(this HtmlHelper helper, string browseId, bool modalDialog)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string dialogTitle = "FilterDialog"; //(string)HttpContext.GetGlobalResourceObject("LabelResource", browseObject.DefaultEntity + "Filter");
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;

            bool haveDate = BrowseHaveDate(browseObject);
            bool haveStanard = BrowseHaveStandard(browseObject);
            string attributesOfColumnFilter = GetAttributesOfFilterColumn(browseObject);

            if (modalDialog)
            {
                gridFilterScript.Append("<script type=\"text/javascript\">");
                gridFilterScript.Append(" $(function(){ ");
                gridFilterScript.Append("   $('#" + namePrefix + "FilterDialog').dialog({ ");
                gridFilterScript.Append("       bgiframe: true, ");
                gridFilterScript.Append("       autoOpen: false, ");
                gridFilterScript.Append("       modal: true ");
                gridFilterScript.Append("   }); ");
                gridFilterScript.Append(" }); ");
                gridFilterScript.Append("</script>");
            }

            gridFilterScript.Append("<div id='" + namePrefix + "FilterDialog' title='" + dialogTitle + "' style='overflow-x:hidden'>");
            if (haveDate || haveStanard)
            {
                gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeWithParametersFilter('" + scope + "', '" + browseId + "', '" + attributesOfColumnFilter + "',this);return false;\" method='post'>");
            }
            else
            {
                gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeFilter('" + scope + "', '" + browseId + "');return false;\" method='post'>");
            }
            gridFilterScript.Append("<fieldset>");
            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            gridFilterScript.AppendLine(BuildSelectFilterSpecialScript(browseObject, scope));
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");

            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            gridFilterScript.AppendLine(BuildSelectOperatorScript(88, browseId, scope));
            if (haveDate)
            {
                gridFilterScript.AppendLine(BuildSelectOperatorDateScript(89, scope, browseId));
            }
            if (haveStanard)
            {
                gridFilterScript.AppendLine(BuildSelectOperatorStandardScript(90, scope, browseId));
            }
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");

            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            //if (gridFilterScript.ToString().IndexOf("changeOptions") != -1)
            //{
            gridFilterScript.AppendLine(GetLabelToInputText("filterValue", browseObject.BrowseId, scope));
            gridFilterScript.AppendLine(GetInputTextWithEvents(scope + browseId + "filterValue", browseObject, scope));
            //}
            //else
            //{
            //    gridFilterScript.AppendLine(GetInputText(scope+browseId+"filterValue"));
            //}
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");
            if (haveStanard)
            {
                IDictionary<string, string> dataList = GetListStandard(browseObject);
                foreach (KeyValuePair<string, string> data in dataList)
                {
                    gridFilterScript.Append("<div class='lineShort'>");
                    gridFilterScript.Append("<div class='fieldShort'>");
                    gridFilterScript.AppendLine(BuildSelectFilterValue(data.Key, data.Value, scope, browseId));
                    gridFilterScript.Append("</div>");
                    gridFilterScript.Append("</div>");
                }
            }
            gridFilterScript.Append("<div class='lineShort'>");
            gridFilterScript.Append("<div class='fieldShort'>");
            gridFilterScript.AppendLine(GetSubmitButton("Filter"));
            if (gridFilterScript.ToString().IndexOf("changeOptions") != -1)
            {
                gridFilterScript.AppendLine(GetResetButtonAll(namePrefix + "FilterForm", scope, browseId, attributesOfColumnFilter));
            }
            else
            {
                gridFilterScript.AppendLine(GetResetButton(namePrefix + "FilterForm"));
            }
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");

            gridFilterScript.Append("</fieldset>");
            gridFilterScript.Append("</form>");
            gridFilterScript.Append("</div>");
            return gridFilterScript.ToString();
        }

        public static string GridFilterReport(this HtmlHelper helper, string browseId, string title,string reportModule)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            StringBuilder gridGroupFilterScript = new StringBuilder();
            StringBuilder gridWithoutGroupFilterScript = new StringBuilder();
            StringBuilder gridEndFilterScript = new StringBuilder();
            BrowseObject browseObject = ReportManager.GetInstance().GetBrowseObject(browseId);
            int rowFilters = browseObject.BrowseColumns.Count;
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;
            int optionSelected = -1;

            if (rowFilters < 1)
            {
                rowFilters = 1;
            }
            
            gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeReport('" + scope + "', '" + browseId + "', '" + title + "', '#" + namePrefix + "FilterForm'); return false;\" method='post'>");
            gridFilterScript.Append("<fieldset>");
            gridFilterScript.Append("<legend style='padding-left: 0px;'>" + title + "</legend>");
            gridFilterScript.Append("<div class='line'>");
            gridFilterScript.Append("<div class='contentSelectReports'>");
            gridFilterScript.AppendLine(GetInputHidden("filterFormHiddenValue", string.Empty));
            gridFilterScript.AppendLine(GetInputHidden("columnTypeListHiddenValue", string.Empty));
            

            
            //if ((browseId == "NewsByCompetitor") || (browseId == "NewsByProduct"))
            //{
            //    for (int i = 0; i < rowFilters; i++)
            //    {
            //        gridFilterScript.Append("<div class='contentSelectItem'>");
            //        gridFilterScript.AppendLine(BuildSelectWithOptionsFilterScript(browseObject, ref optionSelected, i));
            //        gridFilterScript.AppendLine(BuildSelectOperatorScript(i));
            //        gridFilterScript.AppendLine(GetInputText("filterValue", i));
            //        gridFilterScript.AppendLine(GetLabelForStartDate("filterStartDate", i));
            //        gridFilterScript.AppendLine(GetInputStartDateText("filterStartDate", i));
            //        gridFilterScript.AppendLine(GetLabelForStartDate("filterEndDate", i));
            //        gridFilterScript.AppendLine(GetInputEndDateText("filterEndDate", i));
            //        gridFilterScript.AppendLine(GetCheckBox("check", i));
            //        gridFilterScript.Append(" Show");
            //        gridFilterScript.Append("</div>");
            //    }
            //}
            //else {
            int h = 0;
            for (int i = 0; i < rowFilters; i++)
            {
                if (browseObject.BrowseColumns[i].Group)
                {
                    if (i == 0)
                    {
                        gridGroupFilterScript.Append("<div><div style='width:310px;float:left'>Standard Filters and Fields</div>");
                        gridGroupFilterScript.Append("<div style='width:70%;'><a href='javascript: void(0);'onclick=\"javascript: loadListReport('" + reportModule + "');\">Return to Reporting Menu</a></div></div>");
                    }
                    gridGroupFilterScript.Append("<div class='contentSelectItem'>");
                    gridGroupFilterScript.AppendLine(BuildSelectFilterScript(browseObject, ref optionSelected, i));
                    gridGroupFilterScript.AppendLine(BuildSelectOperatorScript(i));
                    gridGroupFilterScript.AppendLine(GetInputText("filterValue", i));
                    gridGroupFilterScript.AppendLine(GetCheckBox("check", i));
                    gridGroupFilterScript.Append(" Show");
                    gridGroupFilterScript.Append("</div>");
                }
                else
                {
                   
                    if (i == 0)
                    {
                        gridWithoutGroupFilterScript.Append("<div><div style='width:310px;float:left'>Report Specific Fields and Filters</div>");
                        gridWithoutGroupFilterScript.Append("<div style='width:70%;'><a href='javascript: void(0);'onclick=\"javascript: loadListReport('" + reportModule + "');\">Return to Reporting Menu</a></div></div>");
                        h++;
                    }
                    else 
                    {
                        if (h == 0 )
                        {
                            gridWithoutGroupFilterScript.Append("Report Specific Fields and Filters");
                        }
                        h++;
                    }
                    gridWithoutGroupFilterScript.Append("<div class='contentSelectItem'>");
                    gridWithoutGroupFilterScript.AppendLine(BuildSelectFilterScript(browseObject, ref optionSelected, i));
                    gridWithoutGroupFilterScript.AppendLine(BuildSelectOperatorScript(i));
                    gridWithoutGroupFilterScript.AppendLine(GetInputText("filterValue", i));
                    gridWithoutGroupFilterScript.AppendLine(GetCheckBox("check", i));
                    gridWithoutGroupFilterScript.Append(" Show");
                    gridWithoutGroupFilterScript.Append("</div>");
                }
            }
            //}
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("<div class='contentSelectReports'>");
            gridEndFilterScript.Append("<div class='buttonLink'>");
            gridEndFilterScript.Append("<div class='float-left-Report'>");
            gridEndFilterScript.Append(GetSubmitButton("Create Report"));
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("<div  class='float-left-Report'>");
            gridEndFilterScript.Append(GetResetButton(namePrefix + "FilterForm"));
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.AppendLine(BuildSelectTypesScript(browseObject));
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("</fieldset>");
            gridEndFilterScript.Append("</form>");
            string fields = string.Empty;
            if (gridGroupFilterScript.ToString().Length > 30)
            {
                fields += gridGroupFilterScript.ToString();
            }
            fields += gridWithoutGroupFilterScript.ToString();
            return gridFilterScript.ToString() + fields + gridEndFilterScript.ToString();
        }

        public static string GridFilterEventReport(this HtmlHelper helper, string browseId, string title, string reportModule)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            BrowseObject browseObject = ReportManager.GetInstance().GetBrowseObject(browseId);
            IDictionary<string, string> dataList = GetListStandard(browseObject);
            IDictionary<string, string> dataTimeFrame = GetListOfItemStandard("EventTypePeriod");
            string properties = GetProperties(browseObject);
            string properitesHidden = GetHiddenProperties(browseObject);
            int rowFilters = browseObject.BrowseColumns.Count;
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;
            int optionSelected = -1;

            if (rowFilters < 1)
            {
                rowFilters = 1;
            }

            gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeReportToEvents('" + scope + "', '" + browseId + "', '" + title + "', '#" + namePrefix + "FilterForm','" + properties + "','" + properitesHidden + "'); return false;\" method='post'>");
            gridFilterScript.Append("<fieldset>");
            gridFilterScript.Append("<div><div style='width:312px;float:left;'><legend>" + title + "</legend></div>");
            gridFilterScript.Append("<div style='width:75%'><a href='javascript: void(0);'onclick=\"javascript: loadListReport('" + reportModule + "');\">Return to Reporting Menu</a></div></div>");
            gridFilterScript.Append("<div class='line'>");
            gridFilterScript.Append("<div class='contentSelectReports'>");
            for (int i = 0; i < rowFilters; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    gridFilterScript.Append("<div class='contentSelectItem'>");
                    gridFilterScript.AppendLine(BuildSelectEventFilterScript(browseObject, ref optionSelected, i));
                    gridFilterScript.AppendLine(BuildSelectOperatorScript(i));
                    gridFilterScript.AppendLine(GetInputText("filterValue", i));

                    foreach (KeyValuePair<string, string> data in dataList)
                    {
                        gridFilterScript.AppendLine(BuildSelectEventFilterValue(data.Key, data.Value, i, browseObject.BrowseColumns[i].ColumnType));
                    }
                    gridFilterScript.AppendLine(GetCheckBox("check", i));
                    gridFilterScript.Append(" Show");
                    gridFilterScript.Append("</div>");
                    gridFilterScript.Append("<div  id='contentSelectStartIntervalDateFilter" + i + "' class='contentSelectItem' style='display:none;'>");
                    gridFilterScript.AppendLine(BuildColumnTimeFrameFilterValue("filterColumnStartIntervalDateField", "EventTimeFrame", i, "StartIntervalDate"));
                    gridFilterScript.Append(BuildVisibleSelectOperatorStandardScript(i, "filterOperatorStartIntervalDateField"));
                    foreach (KeyValuePair<string, string> dataTF in dataTimeFrame)
                    {
                        if (string.IsNullOrEmpty(dataTF.Value))
                        {
                            gridFilterScript.AppendLine(BuildSelectYearValue("filterValueStartIntervalDate" + dataTF.Key, i));
                        }
                        else
                        {
                            gridFilterScript.AppendLine(BuildSelectEventFilterValue("filterValueStartIntervalDate" + dataTF.Key, dataTF.Value, i, browseObject.BrowseColumns[i].ColumnType));
                        }
                    }
                    gridFilterScript.Append("");
                    gridFilterScript.Append("</div>");
                    gridFilterScript.Append("<div id='contentSelectSpecificDateFilter" + i + "'  class='contentSelectItem' style='display:none;'>");
                    gridFilterScript.AppendLine(BuildColumnTimeFrameFilterValue("filterColumnSpecificDateField", "EventTimeFrame", i, "StartDate"));
                    gridFilterScript.AppendLine(BuildSelectOperatorEventDateScript("filterOperatorSpecificDateField", i));
                    gridFilterScript.AppendLine(GetInputStartDateText("filterValueStartDate", i));
                    gridFilterScript.Append("</div>");
                    gridFilterScript.Append("<div id='contentSelectStartEndDateFilter" + i + "' class='contentSelectItem' style='display:none;'>");
                    gridFilterScript.AppendLine(BuildColumnTimeFrameFilterValue("filterColumnStartEndDateField", "EventTimeFrame", i, "EndDate"));
                    gridFilterScript.AppendLine(BuildSelectOperatorEventDateScript("filterOperatorStartEndDateField", i));
                    gridFilterScript.AppendLine(GetInputEndDateText("filterValueEndDate", i));
                    gridFilterScript.Append("</div>");
                }
            }
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("<div class='contentSelectReports'>");
            gridFilterScript.Append("<div class='buttonLink'>");
            gridFilterScript.Append("<div class='float-left-Report'>");
            gridFilterScript.Append(GetSubmitButton("Create Report"));
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("<div  class='float-left-Report'>");
            gridFilterScript.Append(GetResetButton(namePrefix + "FilterForm"));
            gridFilterScript.Append("</div>");
            gridFilterScript.AppendLine(BuildSelectTypesScript(browseObject));
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</fieldset>");
            gridFilterScript.Append("</form>");
            return gridFilterScript.ToString();
        }

        private static string BuildSelectOperatorDateScript(string id, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less"), BrowseFilter.Operator.Lt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less or equal"), BrowseFilter.Operator.Le.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater"), BrowseFilter.Operator.Gt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater or equal"), BrowseFilter.Operator.Ge.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            //selectTagBuilder.MergeAttribute("name", "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", id + j.ToString());
            //selectTagBuilder.MergeAttribute("style", "display:none;width:181px");
            //selectTagBuilder.MergeAttribute("style", "display:none;width:181px");
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectEventFieldFilterScript(BrowseObject browseObject, ref int optionSelected, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            bool hasSelected = false;
            string properties = string.Empty;
            //for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            //{
            //    if (browseObject.BrowseColumns[i].Hidden)
            //    {
            //        //if (browseObject.BrowseColumns[i].Filter)
            //        //{
            //        //    TagBuilder optionTagBuilder = new TagBuilder("option")
            //        //    {
            //        //        InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
            //        //    };

            //        //    optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

            //        //    if ((!hasSelected) && (optionSelected < i))
            //        //    {
            //        //        hasSelected = true;
            //        //        optionSelected = i;
            //        //        optionTagBuilder.Attributes["selected"] = "selected";
            //        //    }

            //        //    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            //        //}
            //        //if (browseObject.BrowseColumns[i].ColumnType == "Date")
            //        //{
            //        //    if (!string.IsNullOrEmpty(properties))
            //        //    {
            //        //        properties += ":";
            //        //    }
            //        //    properties += "Date_" + browseObject.BrowseColumns[i].Property;
            //        //}
            //        //else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
            //        //{
            //        //    if (!string.IsNullOrEmpty(properties))
            //        //    {
            //        //        properties += ":";
            //        //    }
            //        //    properties += "StandardData_" + browseObject.BrowseColumns[i].Property;
            //        //}
            //        //else
            //        //{
            //        //    if (!string.IsNullOrEmpty(properties))
            //        //    {
            //        //        properties += ":";
            //        //    }
            //        //    properties += "Single_" + browseObject.BrowseColumns[i].Property;
            //        //}
            //    }
            //}
            string name = "filterColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            //selectTagBuilder.MergeAttribute("onchange", "changeFilterColumn(this.value," + j.ToString() + ",'" + properties + "')");
            selectTagBuilder.MergeAttribute("id", name + j.ToString());
            selectTagBuilder.GenerateId(name);

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectEventFilterScript(BrowseObject browseObject, ref int optionSelected, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            bool hasSelected = false;
            string properties = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    if (browseObject.BrowseColumns[i].Filter)
                    {
                        TagBuilder optionTagBuilder = new TagBuilder("option")
                        {
                            InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                        };

                        optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

                        if ((!hasSelected) && (optionSelected < i))
                        {
                            hasSelected = true;
                            optionSelected = i;
                            optionTagBuilder.Attributes["selected"] = "selected";
                        }

                        listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                    }
                    if (browseObject.BrowseColumns[i].ColumnType == "Date")
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ":";
                        }
                        properties += "Date_" + browseObject.BrowseColumns[i].Property;
                    }
                    else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ":";
                        }
                        properties += "StandardData_" + browseObject.BrowseColumns[i].Property;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ":";
                        }
                        properties += "Single_" + browseObject.BrowseColumns[i].Property;
                    }
                }
            }
            string name = "filterColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.MergeAttribute("onchange", "changeFilterColumn(this.value," + j.ToString() + ",'" + properties + "')");
            selectTagBuilder.MergeAttribute("id", name + j.ToString());
            selectTagBuilder.GenerateId(name);

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }


        public static string GridSearch(this HtmlHelper helper, string browseId)
        {
            return GridSearch(helper, browseId, false, true);
        }

        public static string GridSearch(this HtmlHelper helper, string browseId, bool returnValue)
        {
            return GridSearch(helper, browseId, returnValue, false);
        }

        public static string GridSearchOptions(this HtmlHelper helper, string browseId)
        {
            return GridSearchOptions(helper, browseId, true);
        }

        public static string GridSearch(this HtmlHelper helper, string browseId, bool returnValue, bool modalDialog)
        {
            StringBuilder gridSearchScript = new StringBuilder();
            BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string dialogTitle = "Search Dialog";//(string)HttpContext.GetGlobalResourceObject("LabelResource", browseObject.DefaultEntity + "Search");
            string namePrefix = scope + browseId;

            if (modalDialog)
            {
                gridSearchScript.Append("<script type=\"text/javascript\">");
                gridSearchScript.Append(" $(function(){ ");
                gridSearchScript.Append("   $('#" + namePrefix + "SearchDialog').dialog({ ");
                gridSearchScript.Append("       bgiframe: true, ");
                gridSearchScript.Append("       autoOpen: false, ");
                gridSearchScript.Append("       modal: true ");
                gridSearchScript.Append("   }); ");
                gridSearchScript.Append(" }); ");
                gridSearchScript.Append("</script>");
            }

            gridSearchScript.Append("<div id='" + namePrefix + "SearchDialog' title='" + dialogTitle + "' style='overflow-x:hidden'>");
            gridSearchScript.Append("<form id='" + namePrefix + "SearchForm' action='' onsubmit=\" javascript: executeSearch('" + scope + "', '" + browseId + "', " + modalDialog.ToString().ToLower() + "); return false;\" method='post'>");

            gridSearchScript.AppendLine("<fieldset>");

            gridSearchScript.Append("<div class='lineShort'>");

            gridSearchScript.Append("<div class='fieldShort'>");
            //gridSearchScript.Append("<span class='titlePopup'>Select items to add</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            gridSearchScript.AppendLine(GetInputText("searchValue"));
            gridSearchScript.AppendLine(GetSubmitButton("Search"));

            gridSearchScript.AppendLine(GetResetButton(namePrefix + "SearchForm"));

            //this code is for Search Dialog close on find search
            //if (returnValue && (!browseObject.ReturnByRow))
            //{
            //    gridSearchScript.AppendLine(GetReturnValueButton("Return"));
            //}
            //
            gridSearchScript.Append("</div>");
            gridSearchScript.Append("</div>");

            gridSearchScript.Append("</fieldset>");

            gridSearchScript.Append("</form>");
            gridSearchScript.Append("</div>");

            return gridSearchScript.ToString();
        }

        public static string GridSearchOptions(this HtmlHelper helper, string browseId, bool tempVal)
        {
            StringBuilder gridSearchScript = new StringBuilder();
            BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;


            gridSearchScript.Append("<div id='" + namePrefix + "Options'" + "' style='margin:1em auto 0 auto; width:20%;'>");

            gridSearchScript.AppendLine(GetReturnValueButton("Ok"));
            gridSearchScript.AppendLine(GetCancelButton("Cancel"));

            gridSearchScript.Append("</div>");

            return gridSearchScript.ToString();

        }

        private static string BuildEventFunction(string eventName, string content)
        {
            StringBuilder result = new StringBuilder();
            result.Append("         " + eventName + " :function(id) {");
            result.Append("         " + content);
            result.Append("         }");
            return result.ToString();
        }

        private static string BuildActiveTab(string tab, string subtab)
        {
            //Workspace                                   //Project  
            return tab + "Subtabs.setActiveTab('" + tab + "Tab_" + subtab + "Content');";
        }
        private static string BuildShowEntity(string scope, string entity, string action, string root)
        {
            //                                                                    Workspace    Project          //ProjectAll     //ProjectContent 
            return "         showEntity('" + root + "/" + action + "', '" + scope + "', '" + entity + "', id, '" + entity + "All', '#" + entity + "Content');";
        }

        private static string BuildGridScript(HtmlHelper helper, UrlHelper url, BrowseObject browseObject, string scope, string controller, string container, object defaultFilterCriteria, IDictionary<string, string> defaultParamsCriteria, string tableId, string pagerId, string columnPopupId)
        {
            StringBuilder gridScript = new StringBuilder();
            StringBuilder urlAction = new StringBuilder(url.Action("GetData", "Browse"));
            StringBuilder urlConfig = new StringBuilder(url.Action("saveConfig", "Browse"));
            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];
            string browseId = browseObject.BrowseId;
            string pathEntity = GetCurrentPath();

            int ncol = 0;
            foreach (var v in browseObject.BrowseColumns)
            {
                if (v.Hidden.Equals(false))
                    ncol++;
            }
            AddParameter("urlEdit", url.Action("Edit", controller));
            AddParameter("scope", scope);
            AddParameter("controller", controller);
            AddParameter("browseId", browseObject.BrowseId);
            AddParameter("container", container);
            AddParameter("currentPath", pathEntity);

            urlAction.Append("?bid=" + browseObject.BrowseId);
            urlAction.Append(GetDefaultFilterCriteria(defaultFilterCriteria));
            urlAction.Append(GetDefaultParamsCriteria(defaultParamsCriteria));
            urlAction.Append("&eou");

            gridScript.Append("<script type=\"text/javascript\"> ");
            /////
            gridScript.Append("  var save_grid_selection" + tableId + " = [];");// stores temporary variable rows cache
            gridScript.Append("  var colNam = \"" + GetColumnNamesShort(browseObject) + "\";  ");
            gridScript.Append("  var colMod = \"[\";  ");
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                gridScript.Append(" colMod = colMod + \"{name:'" + browseObject.BrowseColumns[i].Column + "',");
                gridScript.Append("index:'" + browseObject.BrowseColumns[i].Column + "'\";  ");

                if (browseObject.BrowseColumns[i].Hidden)
                {
                    gridScript.Append(" colMod = colMod +\", hidden: true\";   ");
                }

                gridScript.Append("     var nwidth = $('#datatmp').data('#" + tableId + browseObject.BrowseColumns[i].Column + "W');        ");
                gridScript.Append("     if (nwidth != '' && nwidth != null && nwidth != 'undefined') {  ");
                gridScript.Append("         colMod = colMod + \", width: \";      ");
                if (browseObject.BrowseColumns[i].Width != default(int))
                {
                    gridScript.Append("         colMod = colMod + " + browseObject.BrowseColumns[i].Width + ";      ");
                }
                else
                {
                    gridScript.Append("         colMod = colMod + nwidth;      ");
                }
                gridScript.Append("     } else {    ");
                if (browseId.Length >= 9 && browseId.Substring(browseId.Length - 9, 9).Equals("Dashboard"))
                {
                    gridScript.Append("     var screen = Math.round(($(window).width() * 0.9535) / 2); ");
                }
                else
                {
                    gridScript.Append("     var screen = Math.round($(window).width() * 0.98); ");
                }               
                gridScript.Append("         var ncol = " + ncol + ";     ");
                if (browseObject.BrowseColumns[i].Width != default(int))
                {
                    gridScript.Append("         var newW = " + browseObject.BrowseColumns[i].Width + "; ");
                }
                else
                {
                    gridScript.Append("         var newW = screen / ncol;     ");
                }
                gridScript.Append("         colMod = colMod + \", width: \";      ");
                gridScript.Append("         colMod = colMod + newW; }  ");
                gridScript.Append("     colMod = colMod + \"}\";     ");

                if ((browseObject.BrowseColumns.Count > 0) && (i < (browseObject.BrowseColumns.Count - 1)))
                {
                    gridScript.Append("     colMod = colMod + \",\";  ");
                }
            }
            gridScript.Append("     colMod = colMod + \"]\";  ");
            ////
            gridScript.Append(" $(function(){ ");
            gridScript.Append("     $('#" + tableId + "').jqGrid({");
            gridScript.Append("         url:'" + urlAction.ToString() + "',");
            gridScript.Append("         datatype: \"json\",");
            gridScript.Append("         mtype: \"POST\",");
            //// event for user sort column
            gridScript.Append("         sortable: {                                                  ");
            gridScript.Append("         update: function (permutation) {                             ");
            gridScript.Append("                     savePermutation(permutation,'" + tableId + "','" + userId + "','" + urlConfig + "');   ");
            gridScript.Append("         }},                                                          ");
            gridScript.Append("         colNames: eval(colNam),");
            gridScript.Append("         colModel: eval(colMod),");
            gridScript.Append("         pager: '#" + pagerId + "',");
            gridScript.Append("         rowNum: " + browseObject.CfgPageSize + ",");
            gridScript.Append("         rowList:[10,20,30,40,50],");
            gridScript.Append("         imgpath: '" + url.Content("/Content/Styles/jqgrid/sand/images") + "',");
            gridScript.Append("         sortname: '" + browseObject.OrderByClause + "',");
            gridScript.Append("         multiselect: " + (browseObject.Multiselect ? "true" : "false") + ", ");
            gridScript.Append("onSelectRow: function (id, isSelected) {");
            gridScript.Append("if ('NewsLetterSectionNewsListTable'=='" + tableId + "' || 'NewsLetterSectionEventListTable'=='" + tableId + "'|| 'NewsLetterSectionListTable'=='" + tableId + "' ) {");
            gridScript.Append("EventscheckDescrition(isSelected,id);");
            gridScript.Append("}");
            gridScript.Append("save_grid_selection" + tableId + " = $('#" + tableId + "').jqGrid('getGridParam', 'selarrrow');");//saves the selected rows
            gridScript.Append("},");
            gridScript.Append("gridComplete: function() {");
            gridScript.Append("if ('NewsLetterSectionNewsListTable'=='" + tableId + "' || 'NewsLetterSectionEventListTable'=='" + tableId + "'|| 'NewsLetterSectionListTable'=='" + tableId + "' ) {");
            gridScript.Append("var gps =  $('#" + tableId + "').getGridParam();");
            gridScript.Append("gps['selarrrow'] = save_grid_selection" + tableId + ";");
            gridScript.Append("for (var i=0; i < save_grid_selection" + tableId + ".length; i++) {");
            gridScript.Append("var $row = $('#'+save_grid_selection" + tableId + "[i]);");
            gridScript.Append("if ($row) {");
            gridScript.Append("if ('NewsLetterSectionNewsListTable'=='" + tableId + "' || 'NewsLetterSectionEventListTable'=='" + tableId + "'|| 'NewsLetterSectionListTable'=='" + tableId + "' ) {");
            gridScript.Append("var IdRow = save_grid_selection" + tableId + "[i];"); //
            gridScript.Append("var IdCheck = FuncionaCk.split(':');"); //
            gridScript.Append("for (var k=0; k < IdCheck.length; k++) {");
            gridScript.Append("$row.find('.cbox2').removeAttr('disabled');");//added attribute disbled
            gridScript.Append("if(IdRow==IdCheck[k]){");
            gridScript.Append("$row.find('.cbox2').attr('checked',true);");//added attribute Check
            gridScript.Append("}}}");
            gridScript.Append("$row.find('.cbox').attr('checked',true);");//added attribute Check
            gridScript.Append("$row.attr('aria-selected',true);");
            gridScript.Append("$row.addClass('ui-state-highlight');"); 
            gridScript.Append("}}}},");
            gridScript.Append("         multikey: false,"); //'ctrlKey'
            gridScript.Append("         multiboxonly:true,");
            gridScript.Append("         viewrecords: true,");
            gridScript.Append("         forceFit : false,");       
            gridScript.Append("         pginput:true,");
            if (browseId.Length >= 9 && browseId.Substring(browseId.Length - 9, 9).Equals("Dashboard"))
            {
                gridScript.Append("     width: Math.round(($(window).width() * 0.9535) / 2), ");
            }
            else if (browseId.Contains("NewsLetterSection"))
            {
                gridScript.Append("     width: 700, ");
            }
            else
            {
                gridScript.Append("     width: Math.round($(window).width() * 0.98), ");
            }

            gridScript.Append("         forceFit : false, ");
            gridScript.Append("         autowidth: false, ");

            gridScript.Append("         shrinkToFit: false, ");
            gridScript.Append("         sortorder: '" + browseObject.SortOrder + "' ");

            if (browseId.Length >= 9 && browseId.Substring(browseId.Length - 9, 9).Equals("Dashboard"))
            {
                if (browseId.Equals("ApprovalDashboard"))
                {
                    gridScript.Append("       , ondblClickRow : function(id) {");
                    gridScript.Append("       RedirectToApprove('" + GetApplicationHost() + "/EmailApprove.aspx/GoToApproveProject', id);");
                    gridScript.Append("         } ");
                }
             }

            ///// event for save in session custom grid changes
            gridScript.Append("       ,resizeStop : function(newwidth, index) {         ");
            gridScript.Append("       saveWidth('#" + tableId + "',newwidth, index,'" + userId + "','" + urlConfig + "'); }  ");
                         

            foreach (KeyValuePair<string, string> pair in browseObject.BrowseEvents)
            {
                string browseEventBody = pair.Value;
                gridScript.Append(" , ").Append(pair.Key).Append(" : function(");

                if (string.Compare(pair.Key, BrowseEvent.AfterInsertRow) == 0)
                {
                    gridScript.Append("id, rowdata, rowelem");
                }
                else if (string.Compare(pair.Key, BrowseEvent.BeforeRequest) == 0)
                {
                    gridScript.Append(string.Empty);
                }
                else if (string.Compare(pair.Key, BrowseEvent.BeforeSelectRow) == 0)
                {
                    gridScript.Append("id, e");
                }
                else if (string.Compare(pair.Key, BrowseEvent.GridComplete) == 0)
                {
                    gridScript.Append(string.Empty);
                }
                else if (string.Compare(pair.Key, BrowseEvent.LoadBeforeSend) == 0)
                {
                    gridScript.Append("xhr");
                }
                else if (string.Compare(pair.Key, BrowseEvent.LoadComplete) == 0)
                {
                    gridScript.Append("data");
                }
                else if (string.Compare(pair.Key, BrowseEvent.LoadError) == 0)
                {
                    gridScript.Append("xhr, status, error");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OnCellSelect) == 0)
                {
                    gridScript.Append("id, iCol, cellcontent, e");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OndblClickRow) == 0)
                {
                    gridScript.Append("id, iRow, iCol, e");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OnHeaderClick) == 0)
                {
                    gridScript.Append("gridstate");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OnPaging) == 0)
                {
                    gridScript.Append("pgButton");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OnRightClickRow) == 0)
                {
                    gridScript.Append("id, iRow, iCol, e");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OnSelectAll) == 0)
                {
                    gridScript.Append("aRowids, status");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OnSelectRow) == 0)
                {
                    gridScript.Append("id, status");
                }
                else if (string.Compare(pair.Key, BrowseEvent.OnSortCol) == 0)
                {
                    gridScript.Append("index, iCol, sortorder");
                }
                else if (string.Compare(pair.Key, BrowseEvent.ResizeStart) == 0)
                {
                    gridScript.Append("event, index");
                }
                else if (string.Compare(pair.Key, BrowseEvent.ResizeStop) == 0)
                {
                    gridScript.Append("newwidth, index");
                }
                else if (string.Compare(pair.Key, BrowseEvent.SerializeGridData) == 0)
                {
                    gridScript.Append("postData");
                }

                gridScript.Append(") { ");

                browseEventBody = ExpressionParser.GetExpression(Parameters, browseEventBody);
                browseEventBody = ExpressionParser.GetExpression(helper.ViewContext.HttpContext.Session, browseEventBody);
                browseEventBody = ExpressionParser.GetExpression(browseEventBody);

                gridScript.Append(browseEventBody);
                gridScript.Append(" } ");
            }

            gridScript.Append("});");
            gridScript.Append(" $('#" + tableId + "').jqGrid('navGrid', '#" + pagerId + "',{edit:false,add:false,del:false,search:false, refresh: " + browseObject.Refresh.ToString().ToLower() + "});");


            /////permutacion
            gridScript.Append("      var perm = $('#datatmp').data('" + tableId + "perm'); ");
            gridScript.Append("      if (perm != null && perm != '' && perm != 'undefined' ){ ");
            gridScript.Append("      permArray = perm.split(',');   ");
            gridScript.Append("      $('#" + tableId + "').jqGrid('remapColumns', permArray, true, false); }");
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                gridScript.Append("  checkHidden('#" + tableId + "','" + browseObject.BrowseColumns[i].Column + "');    ");
            }          

            if (browseObject.ShowAll)
            {
                if (browseId.Equals("PositioningAll"))
                {
                    gridScript.Append(" $('#" + tableId + "').jqGrid('navButtonAdd', '#" + pagerId + "', {caption:'', title: 'Load all data', buttonicon:'" + "setIconLoadAll" + "'");
                    gridScript.Append(" , onClickButton: function(){ showAllDataTwoBrowse('#" + tableId + "','#ToolsPositioningAllByHierarchyListTable','" + scope + "','" + controller + "');unCheckMyEntities('#" + tableId + "','" + scope + "','" + controller + "')}, position:'last' });");
                }
                else if (browseId.Equals("PositioningAllByHierarchy"))
                {
                    gridScript.Append(" $('#" + tableId + "').jqGrid('navButtonAdd', '#" + pagerId + "', {caption:'', title: 'Load all data', buttonicon:'" + "setIconLoadAll" + "'");
                    gridScript.Append(" , onClickButton: function(){ showAllDataTwoBrowse('#" + tableId + "','#ToolsPositioningAllListTable','" + scope + "','" + controller + "');unCheckMyEntities('#" + tableId + "','" + scope + "','" + controller + "')}, position:'last' });");
                }
                else
                {
                    gridScript.Append(" $('#" + tableId + "').jqGrid('navButtonAdd', '#" + pagerId + "', {caption:'', title: 'Load all data', buttonicon:'" + "setIconLoadAll" + "'");
                    gridScript.Append(" , onClickButton: function(){ showAllData('#" + tableId + "','" + scope + "','" + controller + "');unCheckMyEntities('#" + tableId + "','" + scope + "','" + controller + "')}, position:'last' });");
                }
            }

            if (browseObject.ToggleColumns)
            {
                gridScript.Append(" $('#" + tableId + "').jqGrid('navButtonAdd', '#" + pagerId + "',{caption:'', title: 'Toggle columns', buttonicon:'" + "setIconToggle" + "'");
                gridScript.Append(" , onClickButton: function(){ toggleColumnPopup('#" + columnPopupId + "');}, position:'last' });");
            }

            if (browseId.Length > 12 && browseId.Substring(browseId.Length - 12, 12).Equals("DetailSelect"))
            {
                gridScript.Append("   $('#" + tableId + "').setGridHeight(227); ");
            }

            if (browseId.Equals("IndustryProductDetail"))
            {
                gridScript.Append("   $('#" + tableId + "').setGridHeight(213); ");
            }

            if (browseId.Equals("ProductCriteriaDetail") || browseId.Equals("IndustryProductDetail"))
            {
                // gridScript.Append("   $('#" + tableId + "').setForceGridWidth(Math.round($(window).width() * 0.975),false); ");
            }
            //gridScript.Append("   alert('#" + tableId + "'); ");
            string columnsHidden = GetColumnHiddenOfBrowse(browseObject);
            if (!columnsHidden.Equals("[]"))
            {
                gridScript.Append(" $('#" + tableId + "').hideCol(" + columnsHidden + " );");
                gridScript.Append(" reziseAfterToggleColumn('" + scope + "','" + browseObject.BrowseId + "');");
            }
            //gridScript.Append(" jQuery('#" + tableId + "').jqGrid('gridResize',{minWidth:350,maxWidth:800,minHeight:80, maxHeight:350});");

            gridScript.Append(" });");


            gridScript.Append("</script>");

            return gridScript.ToString();
        }

        public static string GetCurrentPath()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            int begin = 7;
            if (currentUrl.IndexOf("https://") != -1) { begin++; }
            string path = "";
            bool start = false;
            bool end = false;
            for (int i = begin; i < currentUrl.Length; i++)
            {
                string actualChar = currentUrl[i].ToString();
                if (start == true && actualChar.Equals("/"))
                {
                    end = true;
                    break;
                }

                if (start == false && actualChar.Equals("/"))
                {
                    start = true;
                }


                if (start == true)
                {
                    path = path + actualChar;
                }
            }
            if (end == false)
            {
                path = "";
            }
            return path;
        }

        private static bool BrowseHaveDate(BrowseObject browseObject)
        {
            bool containDate = false;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    if (!string.IsNullOrEmpty(browseObject.BrowseColumns[i].ColumnType))
                    {
                        if (browseObject.BrowseColumns[i].ColumnType.Equals("Date"))
                        {
                            containDate = true;
                        }
                    }
                }
            }

            return containDate;
        }

        private static bool BrowseHaveStandard(BrowseObject browseObject)
        {
            bool containStandard = false;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                    {
                        containStandard = true;
                    }
                }
            }
            return containStandard;
        }

        private static bool BrowseHaveStandardBMR(BrowseObject browseObject)
        {
            bool containStandard = false;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("StandardMultiple"))
                    {
                        containStandard = true;
                    }
                }
            }
            return containStandard;
        }

        private static string BuildSelectFilterScript(BrowseObject browseObject)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            bool haveDate = false;
            bool haveStandard = false;
            string selectTag;
            string properties = string.Empty;
            string propertiesWithType = string.Empty;
            string propertyDate = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    TagBuilder optionTagBuilder = new TagBuilder("option")
                    {
                        InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                    };
                    if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                    {
                        properties += browseObject.BrowseColumns[i].Property + ";";
                        propertiesWithType += "Standard," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                        haveStandard = true;
                    }

                    if (browseObject.BrowseColumns[i].ColumnType == "Date")
                    {
                        haveDate = true;
                        propertyDate = browseObject.BrowseColumns[i].Property;
                        properties += browseObject.BrowseColumns[i].Property + ";";
                        propertiesWithType += "Date," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                    }

                    optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

                    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                }
            }

            string name = "filterColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.GenerateId(name);

            if (haveDate || haveStandard)
            {
                selectTag = selectTagBuilder.ToString(TagRenderMode.Normal);
                int postId = selectTag.IndexOf("name");
                selectTag = selectTag.Substring(0, postId) + "onChange=\"changeOptions(this.value,'" + browseObject.BrowseId + "','" + propertyDate + "','" + properties + "','" + propertiesWithType + "')\" " + selectTag.Substring(postId, selectTag.Length - 1 - postId);
                return selectTag;
            }
            else
            {
                return selectTagBuilder.ToString(TagRenderMode.Normal);
            }
        }

        private static string BuildSelectFilterScript(BrowseObject browseObject, string scope)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            bool haveDate = false;
            bool haveStandard = false;
            string selectTag;
            string properties = string.Empty;
            string propertiesWithType = string.Empty;
            string propertyDate = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    TagBuilder optionTagBuilder = new TagBuilder("option")
                    {
                        InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                    };
                    if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                    {
                        properties += browseObject.BrowseColumns[i].Property + ";";
                        propertiesWithType += "Standard," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                        haveStandard = true;
                    }

                    if (browseObject.BrowseColumns[i].ColumnType == "Date")
                    {
                        haveDate = true;
                        propertyDate = browseObject.BrowseColumns[i].Property;
                        properties += browseObject.BrowseColumns[i].Property + ";";
                        propertiesWithType += "Date," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                    }

                    optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

                    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                }
            }

            string name = scope + browseObject.BrowseId + "filterColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.GenerateId(name);
            selectTagBuilder.MergeAttribute("style", "width:181px");
            if (haveDate || haveStandard)
            {
                selectTag = selectTagBuilder.ToString(TagRenderMode.Normal);
                int postId = selectTag.IndexOf("name");
                selectTag = selectTag.Substring(0, postId) + "onChange=\"changeOptionsWPrope(this.value,'" + browseObject.BrowseId + "','" + propertyDate + "','" + properties + "','" + propertiesWithType + "','" + scope + "')\" " + selectTag.Substring(postId, selectTag.Length - 1 - postId);
                return selectTag;
            }
            else
            {
                return selectTagBuilder.ToString(TagRenderMode.Normal);
            }
        }

        private static string BuildSelectFilterSpecialScript(BrowseObject browseObject, string scope)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            bool haveDate = false;
            bool haveStandard = false;
            string selectTag;
            string properties = string.Empty;
            string propertiesWithType = string.Empty;
            string propertyDate = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter && !browseObject.BrowseColumns[i].Hidden)
                {
                    TagBuilder optionTagBuilder = new TagBuilder("option")
                    {
                        InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                    };
                    if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                    {
                        properties += browseObject.BrowseColumns[i].Property + ";";
                        propertiesWithType += "Standard," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                        haveStandard = true;
                    }

                    if (browseObject.BrowseColumns[i].ColumnType == "Date")
                    {
                        haveDate = true;
                        propertyDate = browseObject.BrowseColumns[i].Property;
                        properties += browseObject.BrowseColumns[i].Property + ";";
                        propertiesWithType += "Date," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                    }

                    optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

                    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                }
            }

            string name = scope + browseObject.BrowseId + "filterColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.GenerateId(name);
            selectTagBuilder.MergeAttribute("style", "width:181px");
            if (haveDate || haveStandard)
            {
                selectTag = selectTagBuilder.ToString(TagRenderMode.Normal);
                int postId = selectTag.IndexOf("name");
                selectTag = selectTag.Substring(0, postId) + "onChange=\"changeOptionsWPrope(this.value,'" + browseObject.BrowseId + "','" + propertyDate + "','" + properties + "','" + propertiesWithType + "','" + scope + "')\" " + selectTag.Substring(postId, selectTag.Length - 1 - postId);
                return selectTag;
            }
            else
            {
                return selectTagBuilder.ToString(TagRenderMode.Normal);
            }
        }

        private static string BuildSelectFilterScript(BrowseObject browseObject, ref int optionSelected, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            bool hasSelected = false;

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    TagBuilder optionTagBuilder = new TagBuilder("option")
                    {
                        InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                    };

                    optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

                    if ((!hasSelected) && (optionSelected < i))
                    {
                        hasSelected = true;
                        optionSelected = i;
                        optionTagBuilder.Attributes["selected"] = "selected";
                    }

                    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                }
            }

            string name = "filterColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.MergeAttribute("onchange", "changeTxtBox(" + j.ToString() + ")");
            selectTagBuilder.MergeAttribute("id", name + j.ToString());
            selectTagBuilder.GenerateId(name);

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        //private static string BuildSelectWithOptionsFilterScript(BrowseObject browseObject, ref int optionSelected, int j)
        //{
        //    StringBuilder listItemBuilder = new StringBuilder();
        //    bool hasSelected = false;

        //    for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
        //    {
        //        if (browseObject.BrowseColumns[i].Filter)
        //        {
        //            TagBuilder optionTagBuilder = new TagBuilder("option")
        //            {
        //                InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
        //            };

        //            optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

        //            if ((!hasSelected) && (optionSelected < i))
        //            {
        //                hasSelected = true;
        //                optionSelected = i;
        //                optionTagBuilder.Attributes["selected"] = "selected";
        //            }

        //            listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
        //        }
        //    }

        //    string name = "filterColumn";
        //    TagBuilder selectTagBuilder = new TagBuilder("select")
        //    {
        //        InnerHtml = listItemBuilder.ToString()
        //    };

        //    selectTagBuilder.MergeAttribute("name", name);
        //    selectTagBuilder.MergeAttribute("onchange", "changeTxtBox(" + j.ToString() + ")");
        //    selectTagBuilder.MergeAttribute("id", name + j.ToString());
        //    selectTagBuilder.GenerateId(name);

        //    return selectTagBuilder.ToString(TagRenderMode.Normal);
        //}

        public static string CreateSelectStandardData(this HtmlHelper helper, string browseId)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            BrowseObject browseObject = ReportManager.GetInstance().GetBrowseObject(browseId);
            int rowFilters = browseObject.BrowseColumns.Count;
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;
            List<string> listStandardData = new List<string>();
            listStandardData = ContainsStandarData(browseObject);

            if (listStandardData.Count > 0)
            {
                for (int j = 0; j < listStandardData.Count; j++)
                {
                    for (int k = 0; k < rowFilters; k++)
                    {
                        gridFilterScript.AppendLine(BuildSelectStandardDataScript(listStandardData[j], k));
                    }

                }

            }

            return gridFilterScript.ToString();
        }

        public static string BuildSelectStandardDataScript(string name, int k)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            string type = "Compelligence.Domain.Entity.Resource." + name;

            //Get the current assembly object
            Assembly assembly = Assembly.Load("Compelligence.Domain");

            //Get the name of the assembly (this will include the public token and version number
            AssemblyName assemblyName = assembly.GetName();

            //Use just the name concat to the class chosen to get the type of the object
            Type t = assembly.GetType(assemblyName.Name + "." + "Entity.Resource." + name);

            //IList<ResourceObject> list = ResourceManagerRepository.LoadAll<ResourceObject>(typeof(ProjectStatus));
            IList<ResourceObject> list = ResourceManagerRepository.LoadAll<ResourceObject>(t);
            list= list.OrderBy(resourceObject => resourceObject.Id).ToList();

            TagBuilder optionTagBuilder0 = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode("")
            };
            optionTagBuilder0.Attributes["value"] = "";
            listItemBuilder.AppendLine(optionTagBuilder0.ToString(TagRenderMode.Normal));

            for (int c = 0; c < list.Count; c++)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = HttpUtility.HtmlEncode(list[c].Value)
                };
                optionTagBuilder.Attributes["value"] = list[c].Id;
                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.MergeAttribute("onchange", "changeFilterValue('" + name + "','" + k.ToString() + "')");
            selectTagBuilder.MergeAttribute("id", name + k.ToString());
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }


        private static string BuildSelectTypesScript(BrowseObject browseObject)
        {
            StringBuilder listItemBuilder = new StringBuilder();


            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    TagBuilder optionTagBuilder = new TagBuilder("option")
                    {
                        InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                    };

                    optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].ColumnType;

                    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                }
            }

            string name = "typeColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.GenerateId(name);

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectTypesToEventScript(BrowseObject browseObject, string name, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                {
                    TagBuilder optionTagBuilder = new TagBuilder("option")
                    {
                        InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                    };
                    optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].ColumnType;
                    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                }
            }
            //string name = "typeColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name + j.ToString());
            selectTagBuilder.MergeAttribute("style", "display:none;");
            selectTagBuilder.GenerateId(name);

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectOperatorScript(int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("begins with"), BrowseFilter.Operator.Bw.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less"), BrowseFilter.Operator.Lt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less or equal"), BrowseFilter.Operator.Le.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater"), BrowseFilter.Operator.Gt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater or equal"), BrowseFilter.Operator.Ge.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("ends with"), BrowseFilter.Operator.Ew.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("contains"), BrowseFilter.Operator.Cn.ToString())
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator");
            selectTagBuilder.MergeAttribute("id", "filterOperator" + j.ToString());
            //selectTagBuilder.GenerateId("filterOperator");

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectOperatorScript(int j, string browseId, string scope)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("begins with"), BrowseFilter.Operator.Bw.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less"), BrowseFilter.Operator.Lt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less or equal"), BrowseFilter.Operator.Le.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater"), BrowseFilter.Operator.Gt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater or equal"), BrowseFilter.Operator.Ge.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("ends with"), BrowseFilter.Operator.Ew.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("contains"), BrowseFilter.Operator.Cn.ToString())
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator");
            selectTagBuilder.MergeAttribute("style", "width:181px");
            selectTagBuilder.MergeAttribute("id", scope + browseId + "filterOperator" + j.ToString());
            //selectTagBuilder.GenerateId("filterOperator");

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectOperatorDateScript(int j, string scope, string browseId)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less"), BrowseFilter.Operator.Lt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less or equal"), BrowseFilter.Operator.Le.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater"), BrowseFilter.Operator.Gt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater or equal"), BrowseFilter.Operator.Ge.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("id", scope + browseId + "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("style", "display:none;width:181px");
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectOperatorStandardScript(int j, string scope, string browseId)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("id", scope + browseId + "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("style", "display:none;width:181px");
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectOperatorStandardMultipleScript(int j, string scope, string browseId)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("contains"), BrowseFilter.Operator.Cn.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not contains"), BrowseFilter.Operator.Nc.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("id", scope + browseId + "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("style", "display:none");
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectStandardScript(BrowseObject browseObject)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            List<string> listStandardData = new List<string>();
            listStandardData = ContainsStandarData(browseObject);
            if (listStandardData.Count > 0)
            {
                for (int j = 0; j < listStandardData.Count; j++)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        gridFilterScript.AppendLine(BuildSelectStandardDataScript(listStandardData[j], k));
                    }

                }

            }

            return gridFilterScript.ToString();
        }

        private static List<string> ContainsStandarData(BrowseObject browseObject)
        {
            List<string> listStandardDataTypes = new List<string>();
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!string.IsNullOrEmpty(browseObject.BrowseColumns[i].ColumnType))
                {
                    if (!browseObject.BrowseColumns[i].ColumnType.Equals("Date"))
                    //if ((!browseObject.BrowseColumns[i].ColumnType.Equals("Date")) || (!browseObject.BrowseColumns[i].ColumnType.Equals("Date:Between")))
                    {
                        listStandardDataTypes.Add(browseObject.BrowseColumns[i].ColumnType);
                    }
                }
            }

            return listStandardDataTypes;
        }

        private static string GetColumnNamesShort(BrowseObject browseObject)
        {
            StringBuilder columnNames = new StringBuilder("[");

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                columnNames.Append("'" + browseObject.BrowseColumns[i].Label + "'");

                if ((browseObject.BrowseColumns.Count > 0) && (i < (browseObject.BrowseColumns.Count - 1)))
                {
                    columnNames.Append(",");
                }
            }
            columnNames.Append("]");

            return columnNames.ToString();
        }

        private static string GetColumnNames(BrowseObject browseObject)
        {
            StringBuilder columnNames = new StringBuilder("colNames:[");

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                columnNames.Append("'" + browseObject.BrowseColumns[i].Label + "'");

                if ((browseObject.BrowseColumns.Count > 0) && (i < (browseObject.BrowseColumns.Count - 1)))
                {
                    columnNames.Append(",");
                }
            }
            columnNames.Append("],");

            return columnNames.ToString();
        }

        private static string GetColumnModel(BrowseObject browseObject)
        {
            StringBuilder columnModel = new StringBuilder("colModel:[");

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                columnModel.Append(GetColumnExpression(browseObject.BrowseColumns[i]));

                if ((browseObject.BrowseColumns.Count > 0) && (i < (browseObject.BrowseColumns.Count - 1)))
                {
                    columnModel.Append(",");
                }
            }

            columnModel.Append("],");

            return columnModel.ToString();
        }

        private static string GetColumnHidden(BrowseObject browseObject)
        {
            StringBuilder columnNames = new StringBuilder("[");

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].ToggleUnChecked)
                {
                    columnNames.Append("'" + browseObject.BrowseColumns[i].Label + "'");

                    if ((browseObject.BrowseColumns.Count > 0) && (i < (browseObject.BrowseColumns.Count - 1)))
                    {
                        columnNames.Append(",");
                    }
                }
            }
            columnNames.Append("]");

            return columnNames.ToString();
        }

        private static string GetColumnHiddenOfBrowse(BrowseObject browseObject)
        {
            StringBuilder columnNames = new StringBuilder("[");

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].ToggleUnChecked)
                {
                    columnNames.Append("'" + browseObject.BrowseColumns[i].Column + "'");

                    if ((browseObject.BrowseColumns.Count > 0) && (i < (browseObject.BrowseColumns.Count - 1)))
                    {
                        columnNames.Append(",");
                    }
                }
            }
            columnNames.Append("]");

            return columnNames.ToString();
        }

        private static string GetColumnExpression(BrowseColumn browseColumn)
        {
            StringBuilder column = new StringBuilder("{");
            string propertyColumn = browseColumn.Column;

            column.Append("name:'" + propertyColumn + "',");
            column.Append("index:'" + propertyColumn + "'");


            if (browseColumn.Hidden)
            {
                column.Append(", hidden: true");
            }

            if (browseColumn.Width != default(int))
            {
                column.Append(", width: " + browseColumn.Width);
            }

            //if (browseColumn.Link != default(string))
            //{
            //    column.Append(", formatter: " + "'link'" );

            //}

            column.Append("}");

            return column.ToString();
        }

        private static string BuildColumnPopup(BrowseObject browseObject, string columnPopupId, string tableId,string userId,StringBuilder urlConfig)
        {
            string scope = string.Empty;
            scope = GetScope(browseObject.BrowseId, tableId);
            double currentsize = 139;
            for(int i=0;i<browseObject.BrowseColumns.Count;i++)
            {
                if (browseObject.BrowseColumns[i].Label != null)
                {
                    //Transforms of string to pixels
                    double columnsize = (browseObject.BrowseColumns[i].Label.Length) * 8;

                    if (!browseObject.BrowseColumns[i].Id && !browseObject.BrowseColumns[i].Hidden)
                    {
                        if (columnsize >= currentsize)
                        {
                            currentsize = columnsize;
                        }
                    }
                }
            }
            StringBuilder columnPopup = new StringBuilder("<div id=\"" + columnPopupId + "\" Style=\"display:none;position:absolute;float:left;margin-left:90px;margin-top:60px;width:" + (currentsize+1) + "px;z-index:2000;border:1px solid gray;-moz-border-radius-bottomleft: 4px;-moz-border-radius-bottomright: 4px;-moz-border-radius-topleft: 4px;-moz-border-radius-topright: 4px;background:url('../content/images/styles/contentbg.jpg') repeat-x scroll center top #eeeeee; padding:2px;\" >");
            int numPopupColumns = 0;

            columnPopup.Append("<div style=\"height:100px; width:" +currentsize+ "px; overflow:auto; _overflow-x:hidden; \">");

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Id && !browseObject.BrowseColumns[i].Hidden)
                {
                    string checkboxFlag = string.Empty;

                    if (numPopupColumns > 0)
                    {
                        columnPopup.Append("<br/>");
                    }

                    if (!browseObject.BrowseColumns[i].Hidden)
                    {
                        checkboxFlag = "checked";
                    }
                    if (browseObject.BrowseColumns[i].ToggleUnChecked)
                    {
                        checkboxFlag = "unchecked";
                    }
                    if (!string.IsNullOrEmpty(scope))
                    {
                        columnPopup.Append("<input class=\"checkbox\" type=\"checkbox\" id=\"grid_" + tableId + "_fld_" + i + "\" name\"showcolumn\" " + checkboxFlag + " onclick=\"toggleColumnView('#" + tableId + "','" + browseObject.BrowseColumns[i].Column + "', this,'" + userId + "','" + urlConfig + "');\" ><label for=\"grid_" + tableId + "_fld_" + i + "\" style=\"font-size:12px;color:black; \">" + browseObject.BrowseColumns[i].Label + "</label>");
                    }
                    else
                    {
                        columnPopup.Append("<input class=\"checkbox\" type=\"checkbox\" id=\"grid_" + tableId + "_fld_" + i + "\" name\"showcolumn\" " + checkboxFlag + " onclick=\"toggleColumnView('#" + tableId + "','" + browseObject.BrowseColumns[i].Column + "', this,'" + userId + "','" + urlConfig + "')\" ><label for=\"grid_" + tableId + "_fld_" + i + "\" style=\"font-size:12px;color:black; \">" + browseObject.BrowseColumns[i].Label + "</label>");
                    }
                    //columnPopup.Append("<input class=\"checkbox\" type=\"checkbox\" id=\"grid_" + tableId + "_fld_" + i + "\" name\"showcolumn\" " + checkboxFlag + " onclick=\"toggleColumnView('#" + tableId + "','" + browseObject.BrowseColumns[i].Column + "', this)\" > <label for=\"grid_" + tableId + "_fld_" + i + "\" style=\"font-size= 0.75em;font-family= Arial, Helvetica, Sans-Serif;background-color=#000000;margin-left=10%; \"> " + browseObject.BrowseColumns[i].Label + "</label>");

                    numPopupColumns++;
                }
            }

            columnPopup.Append("</div>");
            columnPopup.Append("<div class=\"closeIcon\"><a href=\"javascript: toggleColumnPopup('#" + columnPopupId + "');\">Close</div></a>");//<img src=\"../Content/Images/Icons/close.gif\">
            columnPopup.Append("</div>");

            return columnPopup.ToString();
        }

        private static string BuildColumnPopupPositioning(BrowseObject browseObject, BrowseObject browseHiddenObject, string columnPopupId, string tableId, string tableIdHidden)
        {
            StringBuilder columnPopup = new StringBuilder("<div id=\"" + columnPopupId + "\" Style=\"display:none;position:absolute;float:left;margin-left:90px;margin-top:60px;width:140px;z-index:2000;border:1px solid gray;-moz-border-radius-bottomleft: 4px;-moz-border-radius-bottomright: 4px;-moz-border-radius-topleft: 4px;-moz-border-radius-topright: 4px;background:url('../content/images/styles/contentbg.jpg') repeat-x scroll center top #eeeeee; padding:2px;\" >");
            int numPopupColumns = 0;
            columnPopup.Append("<div style=\"height:100px; width:139px; overflow:auto; _overflow-x:hidden; \">");
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Id && !browseObject.BrowseColumns[i].Hidden)
                {
                    string checkboxFlag = string.Empty;

                    if (numPopupColumns > 0)
                    {
                        columnPopup.Append("<br/>");
                    }
                    if (!browseObject.BrowseColumns[i].Hidden)
                    {
                        checkboxFlag = "checked";
                    }
                    columnPopup.Append("<input class=\"checkbox\" type=\"checkbox\" id=\"grid_" + tableId + "_fld_" + i + "\" name\"showcolumn\" " + checkboxFlag + " onclick=\"toggleColumnView('#" + tableId + "','" + browseObject.BrowseColumns[i].Column + "', this);toggleColumnView('#" + tableIdHidden + "','" + browseHiddenObject.BrowseColumns[i].Column + "', this);UpdateCheckBoxToggleColumn('grid_" + tableIdHidden + "_fld_" + i + "',this)\" ><label for=\"grid_" + tableId + "_fld_" + i + "\" style=\"font-size:12px;color:black; \">" + browseObject.BrowseColumns[i].Label + "</label>");
                    numPopupColumns++;
                }
            }

            columnPopup.Append("</div>");
            columnPopup.Append("<div class=\"closeIcon\"><a href=\"javascript: toggleColumnPopup('#" + columnPopupId + "');\">Close</div></a>");//<img src=\"../Content/Images/Icons/close.gif\">
            columnPopup.Append("</div>");

            return columnPopup.ToString();
        }

        private static string GetInputText(string id)
        {
            TagBuilder textTagBuilder = new TagBuilder("input");
            textTagBuilder.MergeAttribute("type", "text");
            textTagBuilder.MergeAttribute("name", id);
            textTagBuilder.MergeAttribute("style", "width:177px");
            textTagBuilder.MergeAttribute("id", id);
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string BuildSelectFilterValue(string id, string value, string scope, string browseId)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            string type = "Compelligence.Domain.Entity.Resource." + value;
            Assembly assembly = Assembly.Load("Compelligence.Domain");
            AssemblyName assemblyName = assembly.GetName();
            Type t = assembly.GetType(assemblyName.Name + ".Entity.Resource." + value);
            IList<ResourceObject> list = ResourceManagerRepository.LoadAll<ResourceObject>(t);
            //list = list.OrderBy(resourceObject => resourceObject.Id).ToList();
            if (id.Equals("TimeFrame") && value.Equals("EventTypePeriod"))
            {
                list = list.OrderBy(resourceObject => resourceObject.Value).ToList();
            }
            TagBuilder optionTagBuilder0 = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode("")
            };
            optionTagBuilder0.Attributes["value"] = "";

            listItemBuilder.AppendLine(optionTagBuilder0.ToString(TagRenderMode.Normal));
            for (int i = 0; i < list.Count; i++)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = HttpUtility.HtmlEncode(list[i].Value)
                };
                optionTagBuilder.Attributes["value"] = list[i].Id;
                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }


            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", scope + browseId + id + "_" + value);
            selectTagBuilder.MergeAttribute("style", "display:none;width:177px");
            return selectTagBuilder.ToString(TagRenderMode.Normal);

        }

        private static string BuildSelectEventFilterValue(string id, string value, int j, string columnType)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            string type = "Compelligence.Domain.Entity.Resource." + value;
            Assembly assembly = Assembly.Load("Compelligence.Domain");
            AssemblyName assemblyName = assembly.GetName();
            Type t = assembly.GetType(assemblyName.Name + ".Entity.Resource." + value);
            IList<ResourceObject> list = ResourceManagerRepository.LoadAll<ResourceObject>(t);
            TagBuilder optionTagBuilder0 = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode("")
            };
            optionTagBuilder0.Attributes["value"] = "";

            listItemBuilder.AppendLine(optionTagBuilder0.ToString(TagRenderMode.Normal));
            for (int i = 0; i < list.Count; i++)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = HttpUtility.HtmlEncode(list[i].Value)
                };
                optionTagBuilder.Attributes["value"] = list[i].Id;
                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", id + j);
            selectTagBuilder.MergeAttribute("style", "display:none;width:247px");
            if (value.Equals("EventTypePeriod"))
            {
                selectTagBuilder.MergeAttribute("onchange", "changeFilterValue('" + id + "','" + j + "')");
            }
            return selectTagBuilder.ToString(TagRenderMode.Normal);

        }

        private static string GetInputTextWithEvents(string id, BrowseObject browseObject, string scope)
        {
            string propertyDate = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    if (browseObject.BrowseColumns[i].ColumnType == "Date")
                    {
                        if (!string.IsNullOrEmpty(propertyDate))
                        {
                            propertyDate += ";";
                        }
                        propertyDate += browseObject.BrowseColumns[i].Property;
                    }
                }
            }

            TagBuilder textTagBuilder = new TagBuilder("input");
            textTagBuilder.MergeAttribute("type", "text");
            textTagBuilder.MergeAttribute("name", id);
            textTagBuilder.MergeAttribute("style", "width:177px");
            textTagBuilder.MergeAttribute("onClick", "CleanFilterValue(this.value,'" + scope + "','" + browseObject.BrowseId + "')");
            textTagBuilder.MergeAttribute("onBlur", "validatorFilterValue(this.value,'" + scope + "','" + browseObject.BrowseId + "','" + propertyDate + "')");
            textTagBuilder.MergeAttribute("id", id);
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetLabelToInputText(string id, string browseId, string scope)
        {
            TagBuilder textTagBuilder = new TagBuilder("label");
            textTagBuilder.MergeAttribute("for", scope + browseId + id);
            textTagBuilder.MergeAttribute("style", "color:red");
            textTagBuilder.MergeAttribute("id", "for" + scope + browseId + id);
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetInputText(string id, int j)
        {
            TagBuilder textTagBuilder = new TagBuilder("input");
            textTagBuilder.MergeAttribute("type", "text");
            textTagBuilder.MergeAttribute("name", id);
            textTagBuilder.MergeAttribute("id", id + j.ToString());
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetInputStartDateText(string id, int j)
        {
            TagBuilder textTagBuilder = new TagBuilder("input");
            textTagBuilder.MergeAttribute("type", "text");
            textTagBuilder.MergeAttribute("name", id);
            textTagBuilder.MergeAttribute("id", id + j.ToString());
            textTagBuilder.MergeAttribute("style", "display:none;width:247px");
            //textTagBuilder.MergeAttribute("style", "visibility:hidden;width:247px");
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetLabelForStartDate(string id, int j)
        {
            TagBuilder textTagBuilder = new TagBuilder("label");
            textTagBuilder.MergeAttribute("for", id);
            textTagBuilder.MergeAttribute("id", "labelFor" + id + j.ToString());
            //textTagBuilder.MergeAttribute("style", "display:none;width:247px");
            textTagBuilder.MergeAttribute("style", "display:none");
            //textTagBuilder.MergeAttribute("style", "visibility:hidden");
            //textTagBuilder.MergeAttribute("title", "between");
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetInputEndDateText(string id, int j)
        {
            TagBuilder textTagBuilder = new TagBuilder("input");
            textTagBuilder.MergeAttribute("type", "text");
            textTagBuilder.MergeAttribute("name", id);
            textTagBuilder.MergeAttribute("id", id + j.ToString());
            textTagBuilder.MergeAttribute("style", "display:none;width:247px");
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }
        private static string GetCheckBox(string id, int j)
        {
            TagBuilder textTagBuilder = new TagBuilder("input");
            textTagBuilder.MergeAttribute("type", "checkbox");
            textTagBuilder.MergeAttribute("name", id);
            textTagBuilder.MergeAttribute("id", id + j.ToString());
            textTagBuilder.MergeAttribute("checked", "true");
            textTagBuilder.GenerateId(id);

            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetSubmitButton(string value)
        {
            TagBuilder submitTagBuilder = new TagBuilder("input");
            submitTagBuilder.MergeAttribute("type", "submit");
            submitTagBuilder.MergeAttribute("class", "button");
            submitTagBuilder.Attributes["value"] = value;

            return submitTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetReturnValueButton(string value)
        {
            TagBuilder buttonTagBuilder = new TagBuilder("input");
            buttonTagBuilder.MergeAttribute("type", "button");
            buttonTagBuilder.Attributes["value"] = value;
            buttonTagBuilder.Attributes["onclick"] = "javascript: returnValues();";
            buttonTagBuilder.Attributes["style"] = "width:65px";
            return buttonTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetResetButton(string formId)
        {
            TagBuilder resetTagBuilder = new TagBuilder("input");
            resetTagBuilder.MergeAttribute("type", "button");
            resetTagBuilder.MergeAttribute("class", "button");
            resetTagBuilder.Attributes["value"] = "Reset";
            resetTagBuilder.Attributes["onclick"] = "javascript: resetFormFields('#" + formId + "');";

            return resetTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetCancelButton(string formId)
        {
            TagBuilder resetTagBuilder = new TagBuilder("input");
            resetTagBuilder.MergeAttribute("type", "button");
            resetTagBuilder.MergeAttribute("class", "button");
            resetTagBuilder.Attributes["value"] = "Cancel";
            resetTagBuilder.Attributes["onclick"] = "javascript: window.close();";
            resetTagBuilder.Attributes["style"] = "width:65px";
            return resetTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetResetButtonAll(string formId, string scope, string browseId, string attributes)
        {
            TagBuilder resetTagBuilder = new TagBuilder("input");
            resetTagBuilder.MergeAttribute("type", "button");
            resetTagBuilder.MergeAttribute("class", "button");
            resetTagBuilder.Attributes["value"] = "Reset";
            resetTagBuilder.Attributes["onclick"] = "javascript: resetFormFields('#" + formId + "');resetFilterOperator('#" + formId + "','" + scope + "','" + browseId + "','" + attributes + "');";

            return resetTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetDefaultParamsCriteria(IDictionary<string, string> defaultParamsCriteria)
        {
            StringBuilder defaultParams = new StringBuilder();

            if (defaultParamsCriteria != null)
            {
                int numDefaultParams = 0;

                defaultParams.Append("&prmcrt=");

                foreach (string paramKey in defaultParamsCriteria.Keys)
                {
                    if (!string.IsNullOrEmpty(defaultParamsCriteria[paramKey]))
                    {
                        if (numDefaultParams > 0)
                        {
                            defaultParams.Append(":");
                        }

                        defaultParams.Append(paramKey + "_" + defaultParamsCriteria[paramKey]);
                        numDefaultParams++;
                    }
                }
            }

            return defaultParams.ToString();
        }

        private static string GetDefaultFilterCriteria(object defaultFilterCriteriaObject)
        {
            StringBuilder defaultFilters = new StringBuilder();

            if (defaultFilterCriteriaObject != null)
            {
                Type type = defaultFilterCriteriaObject.GetType();
                int numDefaultFilters = 0;
                defaultFilters.Append("&defaultCriteria=");

                foreach (PropertyInfo property in type.GetProperties())
                {
                    string value = (string)property.GetValue(defaultFilterCriteriaObject, null);

                    if ((!string.IsNullOrEmpty(value)) && property.Name.Equals("BrowseDetailFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        defaultFilters.Append(value);
                    }
                    else
                    {

                        string[] filterValues = value.Split('_');

                        if ((filterValues != null) && (filterValues.Length == 3))
                        {
                            if (numDefaultFilters > 0)
                            {
                                defaultFilters.Append(":");
                            }

                            defaultFilters.Append(filterValues[0] + "_" + filterValues[1] + "_" + filterValues[2]);
                            numDefaultFilters++;
                        }
                    }
                }
            }

            return defaultFilters.ToString();
        }

        public static void AddParameter(string key, string value)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters.Remove(key);
            }

            Parameters[key] = value;
        }

        public static string DataGridSearch(this HtmlHelper helper, string browseId, string searchId, string searchQuery, string securityGroup)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string componentPrefix = scope + browseId;
            string tableId = componentPrefix + "ListTable";
            string pagerId = componentPrefix + "ListPager";
            string columnPopupId = componentPrefix + "ColumnPopup";
            string gridScript = BuildGridScriptSearch(url, browseObject, scope, searchId, browseId, tableId, pagerId, searchQuery, columnPopupId, securityGroup);
            StringBuilder urlConfig = new StringBuilder(url.Action("saveConfig", "Browse"));
            string userId = (string)helper.ViewContext.HttpContext.Session["UserId"];
            string showColumnWindow = BuildColumnPopup(browseObject, columnPopupId, tableId, userId, urlConfig);

            var tableTagBuilder = new TagBuilder("table");
            tableTagBuilder.GenerateId(tableId);
            tableTagBuilder.AddCssClass("scroll");
            tableTagBuilder.MergeAttribute("cellpadding", "0");
            tableTagBuilder.MergeAttribute("cellspacing", "0");

            var pagerTagBuilder = new TagBuilder("div");
            pagerTagBuilder.GenerateId(pagerId);
            pagerTagBuilder.AddCssClass("scroll");
            pagerTagBuilder.MergeAttribute("style", "text-align: center;");

            return gridScript + showColumnWindow + tableTagBuilder.ToString() + pagerTagBuilder.ToString();
        }

        private static string BuildGridScriptSearch(UrlHelper url, BrowseObject browseObject,
            string scope, string searchId, string browseId, string tableId, string pagerId,
            string search, string columnPopupId, string securityGroup)
        {
            string gridScript;
            StringBuilder urlAction = new StringBuilder(url.Action("SearchData", "InternalSearch"));

            urlAction.Append("?bid=" + browseObject.BrowseId);
            urlAction.Append("&sid=" + searchId);
            urlAction.Append("&sq=" + search);
            urlAction.Append("&eou");
            string tab = "";
            string pathEntity = obtainPathSearch();
            string entity = browseId.Substring(0, browseId.Length - 9);
            if (entity.Equals("kit"))
            {
                entity = "Kit";
            }
            else if (entity.Equals("LibraryNewsA"))
            {
                entity = "News";
            }
            if (browseId.Equals("ProjectAllSearch") || browseId.Equals("DealAllSearch") ||
            browseId.Equals("EventAllSearch") ||
            browseId.Equals("SurveyAllSearch") || browseId.Equals("CalendarAllSearch") ||
            browseId.Equals("LibraryNewsAllSerarch") || browseId.Equals("ObjectiveAllSearch") ||
            browseId.Equals("kitAllSearch"))
            {
                tab = "Workspace";
            }
            if (browseId.Equals("ProductAllSearch") ||
            browseId.Equals("IndustryAllSearch") || browseId.Equals("CompetitorAllSearch") ||
            browseId.Equals("CustomerAllSearch")
            || browseId.Equals("LibraryAllSearch"))
            {
                tab = "Environment";
            }
            //if (browseId.Equals("WinLossAllSearch"))
            //{
            //    tab = "Tools";
            //}
            if (browseId.Equals("SourceAllSearch"))
            {
                tab = "Tools";
            }
            gridScript = "<script type=\"text/javascript\"> " +
                                "$(function(){ " +
                                " $('#" + tableId + "').jqGrid({" +
                                "url:'" + urlAction.ToString() + "'," +
                                "datatype: \"json\"," +
                                " mtype: \"POST\"," +
                                GetColumnNames(browseObject) +
                                GetColumnModel(browseObject) +
                                "pager: '#" + pagerId + "'," +
                                "rowNum:10," +
                                "rowList:[10,20,30]," +
                                " imgpath: '" + url.Content("/Content/Styles/jqgrid/sand/images") + "'," +
                                "sortname: '" + browseObject.OrderByClause + "'," +
                                "multiselect: " + (browseObject.Multiselect ? "true" : "false") + ", " +
                                "viewrecords: true," +
                                " pginput: true," +
                                "sortorder: '" + browseObject.SortOrder + "'," +
                                "width: Math.round($(window).width() * 0.98), ";
            if (browseId.Equals("ProjectAllSearch") || browseId.Equals("CalendarAllSearch")
                || (browseId.Equals("DealAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("EventAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("SurveyAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("ProductAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("ObjectiveAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("kitAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("IndustryAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("CompetitorAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("CustomerAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                || (browseId.Equals("LibraryAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER") || securityGroup.Equals("ANALYST")))
                //|| (browseId.Equals("WinLossAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER")))
                || (browseId.Equals("SourceAllSearch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN") || securityGroup.Equals("MANAGER")))
                || (browseId.Equals("LibraryNewsAllSerarch") && (securityGroup.Equals("ROOT") || securityGroup.Equals("ADMIN")))
                )
            {
                gridScript +=
                "ondblClickRow : function(id) {" +
                                "BackEndTabs.setActiveTab('AdminTabs_" + tab + "Tab'); if($('#gview_" + tab + entity + "AllListTable').length <= 0){" + tab + "Subtabs.setActiveTab('" + tab + "Tab_" + entity + "Content');}" +
                                "showEntity('" + pathEntity + "/" + entity + ".aspx/Edit', '" + tab + "', '" + entity + "', id, '" + entity + "All', '#" + entity + "Content');" +
                                "}, ";
            }
            gridScript +=
                          "loadComplete: function(){ " +
                          "} " +
                      "});" +
                  "});" +
          "</script>";
            return gridScript;
        }

        public static string obtainPathSearch()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            string path = "";
            bool start = false;
            int initPos = 0;
            int endPos = 0;
            int begin = 7;
            if (currentUrl.IndexOf("https://") != -1) { begin++; }
            for (int i = begin; i < currentUrl.Length; i++)
            {
                string actualChar = currentUrl[i].ToString();

                if (start == false && actualChar.Equals("/"))
                {
                    start = true;
                    initPos = i;
                    break;
                }

            }
            endPos = currentUrl.IndexOf("/InternalSearch.aspx/Search");
            path = currentUrl.Substring(initPos, endPos - initPos);
            return path;
        }

        private static IDictionary<string, string> GetListStandard(BrowseObject browseObject)
        {
            IDictionary<string, string> list = new Dictionary<string, string>();
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard") || browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("MultiStandard"))
                {
                    list.Add(browseObject.BrowseColumns[i].Property, browseObject.BrowseColumns[i].ColumnType);
                }
            }
            return list;
        }

        private static string GetProperties(BrowseObject browseObject)
        {
            string properties = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    if (browseObject.BrowseColumns[i].Filter)
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ",";
                        }
                        if (browseObject.BrowseColumns[i].ColumnType == "Date")
                        {
                            properties += browseObject.BrowseColumns[i].ColumnTypeClass + ":" + browseObject.BrowseColumns[i].Property + ":" + browseObject.BrowseColumns[i].Label;
                        }
                        else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                        {
                            properties += browseObject.BrowseColumns[i].ColumnType + ":" + browseObject.BrowseColumns[i].Property + ":" + browseObject.BrowseColumns[i].Label;
                        }
                        else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("MultiStandard"))
                        {
                            properties += browseObject.BrowseColumns[i].ColumnType + ":" + browseObject.BrowseColumns[i].Property + ":" + browseObject.BrowseColumns[i].Label;
                        }
                        else
                        {
                            properties += "Single:" + browseObject.BrowseColumns[i].Property + ":" + browseObject.BrowseColumns[i].Label;
                        }
                    }
                }
            }
            return properties;
        }

        private static string GetHiddenProperties(BrowseObject browseObject)
        {
            string properties = string.Empty;
            for (int j = 0; j < browseObject.BrowseColumns.Count; j++)
            {
                if (!string.IsNullOrEmpty(browseObject.BrowseColumns[j].ColumnType))
                {
                    //if (!string.IsNullOrEmpty(properties))
                    //{
                    //    properties += ",";
                    //}
                    if (browseObject.BrowseColumns[j].ColumnType.Equals("EventTypePeriod"))
                    {
                        properties += browseObject.BrowseColumns[j].ColumnTypeClass + ":" + browseObject.BrowseColumns[j].Property + ":" + browseObject.BrowseColumns[j].ColumnType;
                    }
                }
            }

            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Hidden)
                {
                    if (browseObject.BrowseColumns[i].Filter)
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ",";
                        }
                        if (browseObject.BrowseColumns[i].ColumnType == "Date")
                        {
                            properties += browseObject.BrowseColumns[i].ColumnTypeClass + ":" + browseObject.BrowseColumns[i].Property + ":" + browseObject.BrowseColumns[i].ColumnType;
                        }
                    }
                }
            }
            return properties;
        }

        private static string GetAttributesOfFilterColumn(BrowseObject browseObject)
        {
            string propertiesWithType = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (browseObject.BrowseColumns[i].Filter)
                {
                    if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                    {
                        propertiesWithType += "Standard," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                    }
                    else if (browseObject.BrowseColumns[i].ColumnType == "Date")
                    {
                        propertiesWithType += "Date," + browseObject.BrowseColumns[i].Property + "," + browseObject.BrowseColumns[i].ColumnType + ";";
                    }
                }
            }
            return propertiesWithType;

        }

        public static bool ValidateToResize(this HtmlHelper helper)
        {
            bool flag = false;
            if (!helper.ViewData.ModelState.IsValid)
            {
                flag = true;

            }
            return flag;
        }

        public static string GridFilterWithSpecialDateReport(this HtmlHelper helper, string browseId, string title,string reportModule)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            BrowseObject browseObject = ReportManager.GetInstance().GetBrowseObject(browseId);
            IDictionary<string, string> dataList = GetListStandard(browseObject);
            string properties = GetProperties(browseObject);
            int rowFilters = browseObject.BrowseColumns.Count;
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;
            int optionSelected = -1;

            if (rowFilters < 1)
            {
                rowFilters = 1;
            }

            gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeReportWithSpecialData('" + scope + "', '" + browseId + "', '" + title + "', '#" + namePrefix + "FilterForm','" + properties + "'); return false;\" method='post'>");
            gridFilterScript.Append("<fieldset>");
            gridFilterScript.Append("<div><div style='float:left;width:310px'><legend>" + title + "</legend></div>");
            gridFilterScript.Append("<div style='width:75%;'><a href='javascript: void(0);'onclick=\"javascript: loadListReport('" + reportModule + "');\">Return to Reporting Menu</a></div></div>");
            gridFilterScript.Append("<div class='line'>");
            gridFilterScript.Append("<div class='contentSelectReports'>");
            gridFilterScript.AppendLine(GetInputHidden("filterFormHiddenValue", string.Empty));
            gridFilterScript.AppendLine(GetInputHidden("columnTypeListHiddenValue", string.Empty));
            for (int i = 0; i < rowFilters; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    gridFilterScript.Append("<div class='contentSelectItem'>");
                    gridFilterScript.AppendLine(BuildSelectEventFilterScript(browseObject, ref optionSelected, i));
                    gridFilterScript.AppendLine(BuildSelectOperatorScript(i));
                    gridFilterScript.AppendLine(GetInputText("filterValue", i));
                    gridFilterScript.AppendLine(GetLabelForField("filterSecondValue", i));
                    gridFilterScript.AppendLine(GetSecondInputText("filterSecondValue", i));
                    foreach (KeyValuePair<string, string> data in dataList)
                    {
                        gridFilterScript.AppendLine(BuildSelectEventFilterValue(data.Key, data.Value, i, browseObject.BrowseColumns[i].ColumnType));
                    }
                    //gridFilterScript.AppendLine(BuildSelectTypesToEventScript(browseObject, "typeColumn", i));
                    gridFilterScript.AppendLine(GetCheckBox("check", i));
                    gridFilterScript.Append(" Show");
                    gridFilterScript.Append("</div>");
                    //gridFilterScript.Append("<div class='contentSelectEventItem'>");
                    //gridFilterScript.AppendLine(GetLabelForStartDate("filterStartDateByTimeFrameValue", i));
                    //gridFilterScript.AppendLine(GetInputStartDateText("filterStartDateByTimeFrameValue", i));
                    //gridFilterScript.AppendLine(GetLabelForStartDate("filterEndDateByTimeFrameValue", i));
                    //gridFilterScript.AppendLine(GetInputEndDateText("filterEndDateByTimeFrameValue", i));
                    //gridFilterScript.Append("</div>");
                }
            }
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("<div class='contentSelectReports'>");
            gridFilterScript.Append("<div class='buttonLink'>");
            gridFilterScript.Append("<div class='float-left-Report'>");
            gridFilterScript.Append(GetSubmitButton("Create Report"));
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("<div  class='float-left-Report'>");
            gridFilterScript.Append(GetResetButton(namePrefix + "FilterForm"));
            gridFilterScript.Append("</div>");
            gridFilterScript.AppendLine(BuildSelectTypesScript(browseObject));
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("</fieldset>");
            gridFilterScript.Append("</form>");
            return gridFilterScript.ToString();
        }

        private static string GetLabelForField(string id, int j)
        {
            TagBuilder textTagBuilder = new TagBuilder("label");
            textTagBuilder.MergeAttribute("for", id);
            textTagBuilder.MergeAttribute("id", "labelFor" + id + j.ToString());
            textTagBuilder.MergeAttribute("style", "display:none");
            textTagBuilder.GenerateId(id);
            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetSecondInputText(string id, int j)
        {
            TagBuilder textTagBuilder = new TagBuilder("input");
            textTagBuilder.MergeAttribute("type", "text");
            textTagBuilder.MergeAttribute("name", id);
            textTagBuilder.MergeAttribute("id", id + j.ToString());
            textTagBuilder.MergeAttribute("style", "display:none");
            textTagBuilder.GenerateId(id);
            return textTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        public static string SortingByIndustryHierarchy(this HtmlHelper helper, string browseId)
        {
            string tempo = string.Empty;

            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            if (browseId.Equals("IndustryDetailSelect"))
            {
                BrowseObject browseObject = BrowseManager.GetInstance().GetBrowseObject(browseId);
                var checkBoxTagBuilder = new TagBuilder("input");
                checkBoxTagBuilder.MergeAttribute("id", browseId + "CheckBox");
                checkBoxTagBuilder.MergeAttribute("type", "checkbox");
                checkBoxTagBuilder.MergeAttribute("name", browseId + "CheckBox");
                checkBoxTagBuilder.MergeAttribute("onclick", "javascript: ShowIndustriesByHierarchy(this);");
                checkBoxTagBuilder.GenerateId(browseId);
                tempo = checkBoxTagBuilder.ToString(TagRenderMode.SelfClosing);
            }
            return tempo;
        }

        public static string GetApplicationHost()
        {
            string regExpUrl = "^/(\\w+)/(\\w+).aspx";
            string url = HttpContext.Current.Request.Url.ToString();
            string currentPath = HttpContext.Current.Request.Path;

            if (Regex.IsMatch(currentPath, regExpUrl))
            {
                currentPath = currentPath.Substring(currentPath.IndexOf('/', 1));
            }

            return url.Substring(0, url.IndexOf(currentPath));
        }

        private static string BuildColumnTimeFrameFilterValue(string id, string value, int j, string selected)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            string type = "Compelligence.Domain.Entity.Resource." + value;
            Assembly assembly = Assembly.Load("Compelligence.Domain");
            AssemblyName assemblyName = assembly.GetName();
            Type t = assembly.GetType(assemblyName.Name + ".Entity.Resource." + value);
            IList<ResourceObject> list = ResourceManagerRepository.LoadAll<ResourceObject>(t);
            TagBuilder optionTagBuilder0 = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode("")
            };
            optionTagBuilder0.Attributes["value"] = "";

            listItemBuilder.AppendLine(optionTagBuilder0.ToString(TagRenderMode.Normal));
            for (int i = 0; i < list.Count; i++)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = HttpUtility.HtmlEncode(list[i].Value)
                };
                optionTagBuilder.Attributes["value"] = list[i].Id;
                if (list[i].Value == selected)
                {
                    //optionTagBuilder.MergeAttribute("selected","selected");
                    optionTagBuilder.Attributes["selected"] = "selected";
                }
                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", id + j);
            selectTagBuilder.MergeAttribute("disabled", "disabled");
            //selectTagBuilder.MergeAttribute("style", "display:none;width:247px");
            //if (value.Equals("EventTypePeriod"))
            //{
            //    selectTagBuilder.MergeAttribute("onchange", "changeFilterValue('" + id + "','" + j + "')");
            //}
            return selectTagBuilder.ToString(TagRenderMode.Normal);

        }

        private static string BuildVisibleSelectOperatorStandardScript(int j, string id)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", id + j.ToString());
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static IDictionary<string, string> GetListOfItemStandard(string value)
        {
            IDictionary<string, string> list = new Dictionary<string, string>();

            StringBuilder listItemBuilder = new StringBuilder();
            string type = "Compelligence.Domain.Entity.Resource." + value;
            Assembly assembly = Assembly.Load("Compelligence.Domain");
            AssemblyName assemblyName = assembly.GetName();
            Type t = assembly.GetType(assemblyName.Name + ".Entity.Resource." + value);
            IList<ResourceObject> listValues = ResourceManagerRepository.LoadAll<ResourceObject>(t);

            for (int i = 0; i < listValues.Count; i++)
            {
                if (listValues[i].Id == "Q")
                {
                    list.Add(listValues[i].Id, "EventTypeQuarter");
                }
                else if (listValues[i].Id == "M")
                {
                    list.Add(listValues[i].Id, "EventTypeMonth");
                }
                else if (listValues[i].Id == "Y")
                {
                    list.Add(listValues[i].Id, string.Empty);
                }
            }
            return list;
        }

        private static string BuildSelectYearValue(string id, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();

            IList<DateTime> eventYearList = new List<DateTime>();

            TagBuilder optionTagBuilder0 = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode("")
            };
            optionTagBuilder0.Attributes["value"] = "";

            listItemBuilder.AppendLine(optionTagBuilder0.ToString(TagRenderMode.Normal));
            for (int i = (DateTime.Today.Year) - 2; i <= 2029; i++)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = HttpUtility.HtmlEncode(i.ToString())
                };
                optionTagBuilder.Attributes["value"] = i.ToString();
                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }
            //for (int i = DateTime.Today.Year; i <= 2029; i++)
            //{
            //    eventYearList.Add(new DateTime(i, 1, 1));
            //}

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", id + j);
            //selectTagBuilder.MergeAttribute("style", "display:none;width:247px");
            //if (value.Equals("EventTypePeriod"))
            //{
            //    selectTagBuilder.MergeAttribute("onchange", "changeFilterValue('" + id + "','" + j + "')");
            //}
            return selectTagBuilder.ToString(TagRenderMode.Normal);

        }

        private static string BuildSelectOperatorEventDateScript(string id, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less"), BrowseFilter.Operator.Lt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less or equal"), BrowseFilter.Operator.Le.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater"), BrowseFilter.Operator.Gt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater or equal"), BrowseFilter.Operator.Ge.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            //selectTagBuilder.MergeAttribute("name", "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", id + j.ToString());
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        public static string EmailBody(this HtmlHelper helper, string body)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            //string componentPrefix = scope + browseId;
            //string tableId = componentPrefix + "ListTable";
            //string pagerId = componentPrefix + "ListPager";
            //string columnPopupId = componentPrefix + "ColumnPopup";
            //string gridScript = BuildGridScript(url, browseObject, scope, controller, browseId, container, defaultFilterCriteria, defaultParamCriteria, tableId, pagerId, columnPopupId);
            //string columnPopup = BuildColumnPopup(browseObject, columnPopupId, tableId);

            StringBuilder textAreaScript = new StringBuilder();
            textAreaScript.Append("<textarea rows='2' name='BodyEmail' id='BodyEmail' cols='20'>");
            textAreaScript.Append(body);
            textAreaScript.Append("</textarea>");
            //var tableTagBuilder = new TagBuilder("table");
            //tableTagBuilder.GenerateId(tableId);
            //tableTagBuilder.AddCssClass("scroll");
            //tableTagBuilder.MergeAttribute("cellpadding", "0");
            //tableTagBuilder.MergeAttribute("cellspacing", "0");

            //var pagerTagBuilder = new TagBuilder("div");
            //pagerTagBuilder.GenerateId(pagerId);
            //pagerTagBuilder.AddCssClass("scroll");
            //pagerTagBuilder.MergeAttribute("style", "text-align: center;");

            string Result = textAreaScript.ToString();
            return Result;
        }

        public static string GridSelectListReport(this HtmlHelper helper)
        {
            //StringBuilder gridFilterScript = new StringBuilder();
            //foreach (IndustryByHierarchyView industryByHierarchyView in industries)
            //{
            //    gridFilterScript.AppendLine(BuildCheckBoxFilterScript(industryByHierarchyView.Id));
            //    gridFilterScript.Append(industryByHierarchyView.Name);
            //    gridFilterScript.Append("<br>");
            //}
            //return gridFilterScript.ToString();

            StringBuilder gridFilterScript = new StringBuilder();
            //            gridFilterScript.Append("<div class='ui-multiselect-menu ui-widget ui-widget-content ui-corner-all' style='width: 217px; top: 641px; left: 252.5px; display: block;'>");
            gridFilterScript.Append("<button type='button' class='ui-multiselect ui-widget ui-state-default ui-corner-all ui-state-active' aria-haspopup='true' tabindex='0' style='width: 225px;'><span class='ui-icon ui-icon-triangle-2-n-s'></span><span>Select options</span></button>");
            gridFilterScript.Append("<div class='ui-multiselect-menu ui-widget ui-widget-content ui-corner-all' style='width: 217px; top: 641px; left: 252.5px; display: block;'>");

            gridFilterScript.Append("<div class='ui-widget-header ui-corner-all ui-multiselect-header ui-helper-clearfix'>");
            gridFilterScript.Append("<ul class='ui-helper-reset'>");
            gridFilterScript.Append("<li><a href='#' class='ui-multiselect-all'><span class='ui-icon ui-icon-check'></span><span>Check all</span></a></li>");
            gridFilterScript.Append("<li><a href='#' class='ui-multiselect-none'><span class='ui-icon ui-icon-closethick'></span><span>Uncheck all</span></a></li>");
            gridFilterScript.Append("<li class='ui-multiselect-close'><a class='ui-multiselect-close' href='#'><span class='ui-icon ui-icon-circle-close'></span></a></li>");
            gridFilterScript.Append("</ul>");
            gridFilterScript.Append("</div>");

            gridFilterScript.Append("<ul class='ui-multiselect-checkboxes ui-helper-reset' style='height: 175px;'>");
            for (int j = 0; j <= 10; j++)
            {
                gridFilterScript.Append("<li>");
                gridFilterScript.Append("class='ui-corner-all' for='ui-multiselect-multiselect-demo-option-" + j.ToString() + "'");
                gridFilterScript.Append("<input type='checkbox' title='Option 1' value='" + j + "' name='multiselect_multiselect-demo' id='ui-multiselect-multiselect-demo-option-" + j + "'>");
                gridFilterScript.Append("<span>Option " + j.ToString() + "</span>");
                gridFilterScript.Append("</label>");
                gridFilterScript.Append("</li>");
            }
            gridFilterScript.Append("</ul>");

            gridFilterScript.Append("</div>");
            //gridFilterScript.Append("<div class='ui-widget ui-corner-all' style='width: 217px; top: 641px; left: 252.5px; display: block;'>");
            //gridFilterScript.Append("<div class='ui-widget-header ui-corner-all'>");
            //gridFilterScript.Append("<ul class='ui-multiselect-checkboxes ui-helper-reset'>");
            //for (int j = 0; j <= 10; j++)
            //{
            //    gridFilterScript.Append("<li>");
            //    gridFilterScript.Append("<label class='ui-corner-all ui-state-hover'>");
            //    gridFilterScript.Append("<input type='checkbox' title='Option 1' value='"+j+"' name='multiselect_multiselect-demo' id='ui-multiselect-multiselect-demo-option-"+j+"'>");
            //    gridFilterScript.Append("<span>Option "+ j.ToString() +"</span>");
            //    gridFilterScript.Append("</label>");
            //    gridFilterScript.Append("</li>");
            //}
            //gridFilterScript.Append("</ul>");
            //gridFilterScript.Append("</div>");
            //gridFilterScript.Append("</div>");
            return gridFilterScript.ToString();
            //StringBuilder listItemBuilder = new StringBuilder();

            ////IList<DateTime> eventYearList = new List<DateTime>();

            //TagBuilder optionTagBuilder0 = new TagBuilder("option")
            //{
            //    InnerHtml = HttpUtility.HtmlEncode("")
            //};
            //optionTagBuilder0.Attributes["value"] = "";

            //listItemBuilder.AppendLine(optionTagBuilder0.ToString(TagRenderMode.Normal));
            //for (int i = 0; i <= 10; i++)
            //{
            //    TagBuilder optionTagBuilder = new TagBuilder("option")
            //    {
            //        InnerHtml = HttpUtility.HtmlEncode(i.ToString())
            //    };
            //    optionTagBuilder.Attributes["value"] = i.ToString();
            //    listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            //}

            //TagBuilder selectTagBuilder = new TagBuilder("select")
            //{
            //    InnerHtml = listItemBuilder.ToString()
            //};
            //selectTagBuilder.MergeAttribute("name", "AAAAA");
            //selectTagBuilder.MergeAttribute("id", "XXX");
            //return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        public static string GridFilterReportForm2(this HtmlHelper helper, string browseId, string title, string reportModule)
        {
            StringBuilder gridFilterScript = new StringBuilder();
            StringBuilder gridGroupFilterScript = new StringBuilder();
            StringBuilder gridWithoutGroupFilterScript = new StringBuilder();
            StringBuilder gridEndFilterScript = new StringBuilder();
            BrowseObject browseObject = ReportManager.GetInstance().GetBrowseObject(browseId);
            IDictionary<string, string> dataList = GetListStandard(browseObject);
            string properties = GetProperties(browseObject);
            bool haveDate = BrowseHaveDate(browseObject);
            bool haveStandard = BrowseHaveStandard(browseObject);
            bool haveStandardMultiple = BrowseHaveStandardBMR(browseObject);
            int rowFilters = browseObject.BrowseColumns.Count;
            string scope = StringUtility.CheckNull((string)helper.ViewData["Scope"]);
            string namePrefix = scope + browseId;
            int optionSelected = -1;

            if (rowFilters < 1)
            {
                rowFilters = 1;
            }
            string propertiesList = GetPropertiesOfBrowse(browseObject);
            string columnTypeList = GetColumnTypeOfBrowse(browseObject);
            gridFilterScript.Append("<form id='" + namePrefix + "FilterForm' action='' onsubmit=\" javascript: executeReportForm2('" + scope + "', '" + browseId + "', '" + title + "', '#" + namePrefix + "FilterForm','" + properties + "'); return false;\" method='post'>");
            gridFilterScript.Append("<fieldset>");
            gridFilterScript.Append("<legend style='padding-left: 0px;'>" + title + "</legend>");
            gridFilterScript.Append("<div>");
            gridEndFilterScript.Append("<input class='button' type='button' value='Help' 'onclick=\"javascript: SetValuesToShowHelp('<%= ViewData['Scope'] %>', 'Reports','show','<%= Url.Action('GetHelp','Help') %>','<%= Url.Action('Update','Help') %>','<%= ViewData['EditHelp'] %>','<%= ViewData['ActionFrom'] %>'\");' style='float: right;margin-right: 5px;margin-top:5px'/>");
            gridFilterScript.Append("</div>");
            gridFilterScript.Append("<div class='line'>");
            gridFilterScript.Append("<div class='contentSelectReports'>");
            gridFilterScript.AppendLine(GetInputHidden("filterFormHiddenValue", propertiesList));
            gridFilterScript.AppendLine(GetInputHidden("columnTypeListHiddenValue", columnTypeList));
            gridWithoutGroupFilterScript.Append("Report Specific Fields and Filters");
            gridGroupFilterScript.Append("<div><div style='width:310px;float:left'>Standard Filters and Fields</div>");
            gridGroupFilterScript.Append("<div style='width:70%;'><a href='javascript: void(0);'onclick=\"javascript: loadListReport('" + reportModule + "');\">Return to Reporting Menu</a></div></div>");
            
            for (int i = 0; i < rowFilters; i++)
            {
                if (browseObject.BrowseColumns[i].Group)
                {
                    if (!browseObject.BrowseColumns[i].Hidden)
                    {
                        gridGroupFilterScript.Append("<div class='contentSelectItem'>");
                        gridGroupFilterScript.AppendLine(BuildSelectFilterColumnScript(browseObject, ref optionSelected, i));
                        gridGroupFilterScript.AppendLine(BuildSelectOperatorScript(i));
                        //gridFilterScript.AppendLine(BuildSelectOperatorRScript(i, string.Empty, "Single"));
                        //if (haveDate)
                        //{
                        //    gridFilterScript.AppendLine(BuildSelectOperatorDateRScript(i, string.Empty, "Date"));
                        //}
                        //if (haveStandard)
                        //{
                        //    gridFilterScript.AppendLine(BuildSelectOperatorStandardRScript(i, string.Empty, "Standard"));
                        //}
                        //if (haveStandardMultiple)
                        //{
                        //    gridFilterScript.AppendLine(BuildSelectOperatorStandardMultipleScript(i, string.Empty, "MultiStandard"));
                        //}
                        gridGroupFilterScript.AppendLine(GetInputText("filterValue", i));
                        foreach (KeyValuePair<string, string> data in dataList)
                        {
                            gridGroupFilterScript.AppendLine(BuildSelectSDFilterValue(data.Key, data.Value, i, browseObject.BrowseColumns[i].ColumnType));
                        }
                        gridGroupFilterScript.AppendLine(GetCheckBox("check", i));
                        gridGroupFilterScript.Append(" Show");
                        gridGroupFilterScript.Append("</div>");
                    }
                }
                else
                {
                    if (!browseObject.BrowseColumns[i].Hidden)
                    {
                        gridWithoutGroupFilterScript.Append("<div class='contentSelectItem'>");
                        gridWithoutGroupFilterScript.AppendLine(BuildSelectFilterColumnScript(browseObject, ref optionSelected, i));
                        gridWithoutGroupFilterScript.AppendLine(BuildSelectOperatorScript(i));
                        //gridFilterScript.AppendLine(BuildSelectOperatorRScript(i, string.Empty, "Single"));
                        //if (haveDate)
                        //{
                        //    gridFilterScript.AppendLine(BuildSelectOperatorDateRScript(i, string.Empty, "Date"));
                        //}
                        //if (haveStandard)
                        //{
                        //    gridFilterScript.AppendLine(BuildSelectOperatorStandardRScript(i, string.Empty, "Standard"));
                        //}
                        //if (haveStandardMultiple)
                        //{
                        //    gridFilterScript.AppendLine(BuildSelectOperatorStandardMultipleScript(i, string.Empty, "MultiStandard"));
                        //}
                        gridWithoutGroupFilterScript.AppendLine(GetInputText("filterValue", i));
                        foreach (KeyValuePair<string, string> data in dataList)
                        {
                            gridWithoutGroupFilterScript.AppendLine(BuildSelectSDFilterValue(data.Key, data.Value, i, browseObject.BrowseColumns[i].ColumnType));
                        }
                        gridWithoutGroupFilterScript.AppendLine(GetCheckBox("check", i));
                        gridWithoutGroupFilterScript.Append(" Show");
                        gridWithoutGroupFilterScript.Append("</div>");
                    }
                }
            }

            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("<div class='contentSelectReports'>");
            gridEndFilterScript.Append("<div class='buttonLink'>");
            gridEndFilterScript.Append("<div class='float-left-Report'>");
            gridEndFilterScript.Append(GetSubmitButton("Create Report"));
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("<div  class='float-left-Report'>");
            gridEndFilterScript.Append(GetResetButton(namePrefix + "FilterForm"));

            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.AppendLine(BuildSelectTypesScript(browseObject));
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("</div>");
            gridEndFilterScript.Append("</fieldset>");
            gridEndFilterScript.Append("</form>");
            string fields = string.Empty;
            if (gridGroupFilterScript.ToString().Length > 30)
            {
                fields += gridGroupFilterScript.ToString();
            }
            fields += gridWithoutGroupFilterScript.ToString();
            return gridFilterScript.ToString() + fields + gridEndFilterScript.ToString();
        }

        private static string BuildSelectFilterColumnScript(BrowseObject browseObject, ref int optionSelected, int j)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            bool hasSelected = false;
            string properties = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    if (browseObject.BrowseColumns[i].Filter)
                    {
                        TagBuilder optionTagBuilder = new TagBuilder("option")
                        {
                            InnerHtml = HttpUtility.HtmlEncode(browseObject.BrowseColumns[i].Label)
                        };

                        optionTagBuilder.Attributes["value"] = browseObject.BrowseColumns[i].Column;

                        if ((!hasSelected) && (optionSelected < i))
                        {
                            hasSelected = true;
                            optionSelected = i;
                            optionTagBuilder.Attributes["selected"] = "selected";
                        }

                        listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
                    }
                    if (browseObject.BrowseColumns[i].ColumnType == "Date")
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ":";
                        }
                        properties += "Date_" + browseObject.BrowseColumns[i].Property;
                    }
                    else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ":";
                        }
                        properties += "StandardData_" + browseObject.BrowseColumns[i].Property;
                    }
                    else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("MultiStandard"))
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ":";
                        }
                        properties += "MultiStandardData_" + browseObject.BrowseColumns[i].Property;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ":";
                        }
                        properties += "Single_" + browseObject.BrowseColumns[i].Property;
                    }
                }
            }
            string name = "filterColumn";
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", name);
            selectTagBuilder.MergeAttribute("onchange", "changeFilterColumnReport(this.value," + j.ToString() + ",'" + properties + "')");
            selectTagBuilder.MergeAttribute("id", name + j.ToString());
            selectTagBuilder.GenerateId(name);

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }


        private static string BuildSelectSDFilterValue(string id, string value, int j, string columnType)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            string type = "Compelligence.Domain.Entity.Resource." + value;
            Assembly assembly = Assembly.Load("Compelligence.Domain");
            AssemblyName assemblyName = assembly.GetName();
            Type t = assembly.GetType(assemblyName.Name + ".Entity.Resource." + value);
            IList<ResourceObject> list = ResourceManagerRepository.LoadAll<ResourceObject>(t);
            TagBuilder optionTagBuilder0 = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode("")
            };
            optionTagBuilder0.Attributes["value"] = "";

            listItemBuilder.AppendLine(optionTagBuilder0.ToString(TagRenderMode.Normal));
            for (int i = 0; i < list.Count; i++)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = HttpUtility.HtmlEncode(list[i].Value)
                };
                optionTagBuilder.Attributes["value"] = list[i].Id;
                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }
            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };
            selectTagBuilder.MergeAttribute("name", id);
            selectTagBuilder.MergeAttribute("id", id + j);
            selectTagBuilder.MergeAttribute("style", "display:none;");
            //if (value.Equals("EventTypePeriod"))
            //{
            //    selectTagBuilder.MergeAttribute("onchange", "changeFilterValue('" + id + "','" + j + "')");
            //}
            return selectTagBuilder.ToString(TagRenderMode.Normal);

        }

        private static string BuildSelectOperatorDateRScript(int j, string scope, string browseId)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less"), BrowseFilter.Operator.Lt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less or equal"), BrowseFilter.Operator.Le.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater"), BrowseFilter.Operator.Gt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater or equal"), BrowseFilter.Operator.Ge.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("id", scope + browseId + "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("style", "display:none;");
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectOperatorRScript(int j, string browseId, string scope)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("begins with"), BrowseFilter.Operator.Bw.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less"), BrowseFilter.Operator.Lt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("less or equal"), BrowseFilter.Operator.Le.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater"), BrowseFilter.Operator.Gt.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("greater or equal"), BrowseFilter.Operator.Ge.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("ends with"), BrowseFilter.Operator.Ew.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("contains"), BrowseFilter.Operator.Cn.ToString())
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator");
            selectTagBuilder.MergeAttribute("id", scope + browseId + "filterOperator" + j.ToString());
            //selectTagBuilder.GenerateId("filterOperator");

            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string BuildSelectOperatorStandardRScript(int j, string scope, string browseId)
        {
            StringBuilder listItemBuilder = new StringBuilder();
            BrowseFilterOption[] operators = {
                            new BrowseFilterOption(HttpUtility.HtmlEncode("equal"), BrowseFilter.Operator.Eq.ToString()),
                            new BrowseFilterOption(HttpUtility.HtmlEncode("not equal"), BrowseFilter.Operator.Ne.ToString()),
                            };

            foreach (BrowseFilterOption operatorOption in operators)
            {
                TagBuilder optionTagBuilder = new TagBuilder("option")
                {
                    InnerHtml = operatorOption.Text
                };

                optionTagBuilder.Attributes["value"] = operatorOption.Value;

                listItemBuilder.AppendLine(optionTagBuilder.ToString(TagRenderMode.Normal));
            }

            TagBuilder selectTagBuilder = new TagBuilder("select")
            {
                InnerHtml = listItemBuilder.ToString()
            };

            selectTagBuilder.MergeAttribute("name", "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("id", scope + browseId + "filterOperator" + j.ToString());
            selectTagBuilder.MergeAttribute("style", "display:none");
            return selectTagBuilder.ToString(TagRenderMode.Normal);
        }

        private static string GetInputHidden(string id, string value)
        {
            TagBuilder inputTagBuilder = new TagBuilder("input");
            inputTagBuilder.MergeAttribute("type", "hidden");
            inputTagBuilder.MergeAttribute("value", value);
            inputTagBuilder.MergeAttribute("name", id);
            inputTagBuilder.MergeAttribute("id", id);
            inputTagBuilder.GenerateId(id);

            return inputTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string GetPropertiesOfBrowse(BrowseObject browseObject)
        {
            string properties = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    if (browseObject.BrowseColumns[i].Filter)
                    {
                        if (browseObject.BrowseColumns[i].ColumnType == "Date")
                        {
                            if (!string.IsNullOrEmpty(properties))
                            {
                                properties += ":";
                            }
                            properties += "Date_" + browseObject.BrowseColumns[i].Property;
                        }
                        else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                        {
                            if (!string.IsNullOrEmpty(properties))
                            {
                                properties += ":";
                            }
                            properties += "StandardData_" + browseObject.BrowseColumns[i].Property;
                        }
                        else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("MultiStandard"))
                        {
                            if (!string.IsNullOrEmpty(properties))
                            {
                                properties += ":";
                            }
                            properties += "MultiStandardData_" + browseObject.BrowseColumns[i].Property;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(properties))
                            {
                                properties += ":";
                            }
                            properties += "Single_" + browseObject.BrowseColumns[i].Property;
                        }
                    }
                }
            }

            return properties;
        }

        private static string GetColumnTypeOfBrowse(BrowseObject browseObject)
        {
            string properties = string.Empty;
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    if (browseObject.BrowseColumns[i].Filter)
                    {
                        if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("MultiStandard"))
                        {
                            if (!string.IsNullOrEmpty(properties))
                            {
                                properties += ":";
                            }
                            properties += "MultiStandardData_" + browseObject.BrowseColumns[i].ColumnType + "_" + browseObject.BrowseColumns[i].Property;
                        }
                        else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                        {
                            if (!string.IsNullOrEmpty(properties))
                            {
                                properties += ":";
                            }
                            properties += "StandardData_" + browseObject.BrowseColumns[i].ColumnType + "_" + browseObject.BrowseColumns[i].Property;
                        }

                    }
                }
            }

            return properties;
        }

        //private static string[] GetKeyToOperator(BrowseObject browseObject)
        //{
        //    string[] keys = new string { }();

        //    return keys;
        //}

        private static string GetScope(string browseid, string target)
        {
            string scope = string.Empty;
            int posBrowseId = target.IndexOf(browseid);
            if (posBrowseId != -1)
            {
                scope = target.Substring(0, posBrowseId);
            }
            return scope;
        }
    }


}