using System;
using System.Data;
using System.Text;

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
using System.Collections.Generic;
using System.Reflection;  // reflection namespace
using Compelligence.Common.Utility.Parser;
using Compelligence.Common.Utility;
using Compelligence.Common.Browse;
using Compelligence.Web.Controllers;
using Spring.Context;
using Spring.Context.Support;

namespace Compelligence.Web.Models.Helpers
{
    public static class StaticDataGridHelper
    {
        private static bool SearchPropertyName(string PropertyName, PropertyInfo[] properties)
        {
            foreach (PropertyInfo pi in properties)
            {
                if (PropertyName.Equals(pi.Name))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool SearchTitleName(string PropertyName, IList<string> titles)
        {
            foreach (string title in titles)
            {
                if (PropertyName.Equals(title))
                {
                    return true;
                }
            }
            return false;
        }

        private static IList<PropertyInfo> UniqueProperties(PropertyInfo[] source)
        {
            IList<PropertyInfo> target = new List<PropertyInfo>();
            foreach (PropertyInfo pi in source)
            {
                if (!SearchPropertyName(pi.Name, target.ToArray()))
                {
                    target.Add(pi);
                }
            }
            return target;
        }

        static T Cast<T>(object obj, T type)
        {
            return (T)obj;
        }

        private static string MakeScript(string Name, string SubRutineName)
        {
            StringBuilder Result = new StringBuilder("<script type=\"text/javascript\">");
            Result.AppendLine("  function " + Name + "() {");

            Result.AppendLine("  var RowSelected=$(\"#myGrid tbody\").children(\".highlight\"); ");
            Result.AppendLine("  if ( RowSelected!=null)");
            Result.AppendLine("  {  var id=RowSelected[0];");
            Result.AppendLine("      " + SubRutineName + " (id);");
            Result.AppendLine("  } ");
            Result.AppendLine("} ");
            Result.AppendLine("</script>");
            return Result.ToString();
        }
        private static string MakePager(string XmlSource, int page, int totalpages, string sGroupBy, string sOrderBy, string sAsc, UrlHelper url, int totalRecords, int rowsByPage, int pageSize, string sIndustry, string sCompetitor, string showAllEntities)
        {
            string sFirstPage = string.Empty;
            string sNextPage = string.Empty;
            string sPrevPage = string.Empty;
            string sLastPage = string.Empty;


            StringBuilder Result = new StringBuilder("<div class='pager' id='pager' style='padding:10px 0px 10px 0px' >");
            Result.AppendLine("<div style='width: 50%; float: left;margin-top: -6px;'>");
            Result.AppendLine("<form>");
            if (sGroupBy.Length > 0 && sOrderBy.Length > 0)
            {
                sFirstPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, order = sOrderBy, page = 1, showAll = showAllEntities });
                sNextPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, order = sOrderBy, page = ((page + 1) > totalpages ? totalpages : page + 1), showAll = showAllEntities });
                sPrevPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, order = sOrderBy, page = page - 1, showAll = showAllEntities });
                sLastPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, order = sOrderBy, page = totalpages, showAll = showAllEntities });
            }
            else if (sGroupBy.Length > 0)
            {
                sFirstPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, page = 1, showAll = showAllEntities });
                sNextPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, page = ((page + 1) > totalpages ? totalpages : page + 1), showAll = showAllEntities });
                sPrevPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, page = page - 1, showAll = showAllEntities });
                sLastPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, page = totalpages, showAll = showAllEntities });
            }
            else if (sOrderBy.Length > 0)
            {
                sFirstPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, order = sOrderBy, asc = sAsc, page = 1, showAll = showAllEntities });
                sNextPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, order = sOrderBy, asc = sAsc, page = ((page + 1) > totalpages ? totalpages : page + 1), showAll = showAllEntities });
                sPrevPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, order = sOrderBy, asc = sAsc, page = page - 1, showAll = showAllEntities });
                sLastPage = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, order = sOrderBy, asc = sAsc, page = totalpages, showAll = showAllEntities });
            }

            Result.AppendLine("	<input type='button' class='button' value='<<' onclick=\"location.href='" + sFirstPage + "'\" />");
            Result.AppendLine("	<input type='button' class='button' value='<' onclick=\"location.href='" + sPrevPage + "'\" /> ");
            Result.AppendLine("	<input type='text' class='pagedisplay' value='" + page + "/" + totalpages + "' />");
            Result.AppendLine("	<input type='button' class='button' value='>' onclick=\"location.href='" + sNextPage + "'\" />");
            Result.AppendLine("	<input type='button' class='button' value='>>' onclick=\"location.href='" + sLastPage + "'\" />");
            //Result.AppendLine ("	<select class='pagesize'>");
            //Result.AppendLine ("	    <option value=15 selected='selected'>15</option>");
            //Result.AppendLine ("		<option value='30'>30</option>");
            //Result.AppendLine ("		<option value='45'>45</option>");
            //Result.AppendLine ("		<option value='60'>60</option>");
            //Result.AppendLine ("	</select>");

            Result.AppendLine("</form>");
            Result.AppendLine("</div>");

            int iLow = (page - 1) * pageSize + 1;
            int iUp = page * pageSize > totalRecords ? totalRecords : page * pageSize;

            Result.AppendLine("<div style='width: 50%; float: right; text-align: right;'>");
            Result.AppendLine("View " + iLow + " - " + iUp + " of " + totalRecords);
            Result.AppendLine("</div>");

            //View 1 - 10 of 109

            Result.AppendLine("</div>");
            return Result.ToString();
        }

        private static string MakePagerFooter(string Controller, string Action, string entityId, string entityType, string XmlSource, int page, int totalpages, string sGroupBy, string sOrderBy, string sAsc, UrlHelper url, int totalRecords, int rowsByPage, int pageSize, string sIndustry, string sCompetitor, string showAllEntities)
        {
            return MakePagerFooter(Controller, Action, entityId, entityType, XmlSource, page, totalpages, sGroupBy, sOrderBy, sAsc, url, totalRecords, rowsByPage, pageSize, sIndustry, sCompetitor, showAllEntities, string.Empty);
        }

        private static string MakePagerFooter(string Controller, string Action, string entityId, string entityType, string XmlSource, int page, int totalpages, string sGroupBy, string sOrderBy, string sAsc, UrlHelper url, int totalRecords, int rowsByPage, int pageSize, string sIndustry, string sCompetitor, string showAllEntities, string sClientCompany)
        {
            string sFirstPage = string.Empty;
            string sNextPage = string.Empty;
            string sPrevPage = string.Empty;
            string sLastPage = string.Empty;
            string salescompany = string.Empty;
            if (!string.IsNullOrEmpty(sClientCompany))
            {
                salescompany = Encryptor.Encode(sClientCompany);
            }


            StringBuilder Result = new StringBuilder("<div class='pager' id='pager' style='padding:10px 0px 10px 0px' >");
            Result.AppendLine("<div style='width: 50%; float: left;'>");
            Result.AppendLine("<form>");
            if (sGroupBy.Length > 0 && sOrderBy.Length > 0)
            {
                sFirstPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, order = sOrderBy, page = 1, C = salescompany });
                sNextPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, order = sOrderBy, C = salescompany, page = ((page + 1) > totalpages ? totalpages : page + 1) });
                sPrevPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, order = sOrderBy, page = page - 1, C = salescompany });
                sLastPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, order = sOrderBy, page = totalpages, C = salescompany });
            }
            else if (sGroupBy.Length > 0)
            {
                sFirstPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, page = 1, C = salescompany });
                sNextPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, C = salescompany, page = ((page + 1) > totalpages ? totalpages : page + 1) });
                sPrevPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, page = page - 1, C = salescompany });
                sLastPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, page = totalpages, C = salescompany });
            }
            else if (sOrderBy.Length > 0)
            {
                sFirstPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, order = sOrderBy, asc = sAsc, page = 1, C = salescompany });
                sNextPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, order = sOrderBy, asc = sAsc, C = salescompany, page = ((page + 1) > totalpages ? totalpages : page + 1) });
                sPrevPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, order = sOrderBy, asc = sAsc, page = page - 1, C = salescompany });
                sLastPage = url.Action(Action, Controller, new { CompetitorId = entityId, EntityType = entityType, order = sOrderBy, asc = sAsc, page = totalpages, C = salescompany });
            }

            //Result.AppendLine("	<input type='button' class='button' value='<<' onclick=\"location.href='" + sFirstPage + "'\" />");
            //Result.AppendLine("	<input type='button' class='button' value='<' onclick=\"location.href='" + sPrevPage + "'\" /> ");
            //Result.AppendLine("	<input type='text' class='pagedisplay' value='" + page + "/" + totalpages + "' />");
            //Result.AppendLine("	<input type='button' class='button' value='>' onclick=\"location.href='" + sNextPage + "'\" />");
            //Result.AppendLine("	<input type='button' class='button' value='>>' onclick=\"location.href='" + sLastPage + "'\" />");

            Result.AppendLine("	<input type='button' class='button' value='<<' onclick=\"GetNewsOfCompetitorByProduct('" + sFirstPage + "','ResultNewsContent');\" />");
            Result.AppendLine("	<input type='button' class='button' value='<' onclick=\"GetNewsOfCompetitorByProduct('" + sPrevPage + "','ResultNewsContent');\" /> ");
            Result.AppendLine("	<input type='text' class='pagedisplay' value='" + page + "/" + totalpages + "' />");
            Result.AppendLine("	<input type='button' class='button' value='>' onclick=\"GetNewsOfCompetitorByProduct('" + sNextPage + "','ResultNewsContent');\" />");
            Result.AppendLine("	<input type='button' class='button' value='>>' onclick=\"GetNewsOfCompetitorByProduct('" + sLastPage + "','ResultNewsContent');\" />");

            //Result.AppendLine ("	<select class='pagesize'>");
            //Result.AppendLine ("	    <option value=15 selected='selected'>15</option>");
            //Result.AppendLine ("		<option value='30'>30</option>");
            //Result.AppendLine ("		<option value='45'>45</option>");
            //Result.AppendLine ("		<option value='60'>60</option>");
            //Result.AppendLine ("	</select>");

            Result.AppendLine("</form>");
            Result.AppendLine("</div>");

            int iLow = (page - 1) * pageSize + 1;
            int iUp = page * pageSize > totalRecords ? totalRecords : page * pageSize;

            Result.AppendLine("<div style='width: 50%; float: right; text-align: right;'>");
            Result.AppendLine("View " + iLow + " - " + iUp + " of " + totalRecords);
            Result.AppendLine("</div>");

            //View 1 - 10 of 109

            Result.AppendLine("</div>");
            return Result.ToString();
        }

        private static string MakeTableGroup(string XmlSource, string[] ActionGroups, string sGroupBy, string sOrderBy, string sAsc, string sPage, UrlHelper url)
        {
            return MakeTableGroup(XmlSource, ActionGroups, sGroupBy, sOrderBy, sAsc, sPage, url, null);
        }

        private static string MakeTableGroup(string XmlSource, string[] ActionGroups, string sGroupBy, string sOrderBy, string sAsc, string sPage, UrlHelper url, string[] filterParameters)
        {
            var routeValues = new System.Web.Routing.RouteValueDictionary();
            //To filter by parameters to Event and Deal
            if (filterParameters != null)
            {
                int filterParameterLeng = filterParameters.Length;
                for (int j = 0; j < filterParameterLeng; j++)
                {
                    string filterParameter = filterParameters[j];
                    string[] parameter = filterParameter.Split(',');
                    if (parameter.Length == 2)
                    {
                        if (!string.IsNullOrEmpty(parameter[0]) && !string.IsNullOrEmpty(parameter[1]))
                        {
                            routeValues[parameter[0]] = parameter[1];
                        }
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            int GroupsLength = ActionGroups.Length;
            sb.AppendLine("<div style='width:65%; float: right; text-align: right;'>");//float: left;
            if (ActionGroups.Length > 0)
                sb.AppendLine("Group by :");

            for (int i = 0; i < GroupsLength; i++)
            {
                string ActionGroup = ActionGroups[i];
                string[] ArrayGroup = (ActionGroup as String).Split(',');
                string labelGroup = string.Empty;

                StringBuilder linkGroup = new StringBuilder();
                linkGroup.AppendLine("&nbsp;&nbsp;&nbsp;&nbsp;");
                if (sOrderBy.Length > 0 && sPage.Length > 0) //have page number and order
                {
                    //                                         ..., DealSupport, new ...
                    routeValues["group"] = ArrayGroup[1];
                    routeValues["order"] = sOrderBy;
                    routeValues["asc"] = sAsc;
                    routeValues["page"] = sPage;
                }                                              // var a=     url.Action("Index",XmlSource,routeValues);    
                else if (sOrderBy.Length > 0 && sPage.Length > 0) //have page number
                {
                    routeValues["group"] = ArrayGroup[1];
                    routeValues["asc"] = sAsc;
                    routeValues["page"] = sPage;
                }
                else
                {
                    routeValues["group"] = ArrayGroup[1];
                }
                linkGroup.AppendLine("<a class='gridlink' href='" + url.Action("Index", XmlSource, routeValues) + "'> " + ArrayGroup[0] + " </a> &nbsp;&nbsp;&nbsp;&nbsp;");
                if (sGroupBy.Length > 0 && sGroupBy.Equals(ArrayGroup[1]))
                    sb.AppendLine("<i>" + linkGroup.ToString() + "</i>");
                else
                    sb.AppendLine(linkGroup.ToString());
                if (i < (GroupsLength - 1))
                    sb.Append("-");
            }

            sb.AppendLine("</div>");
            return sb.ToString();
        }

        private static string MakeTableGroupFooter(string entityId, string entityType, string XmlSource, string[] ActionGroups, string sGroupBy, string sOrderBy, string sAsc, string sPage, UrlHelper url)
        {
            StringBuilder sb = new StringBuilder();
            int GroupsLength = ActionGroups.Length;
            sb.AppendLine("<div style='width:65%; float: right; text-align: right;'>");//float: left;
            if (ActionGroups.Length > 0)
                sb.AppendLine("Group by :");

            for (int i = 0; i < GroupsLength; i++)
            {
                string ActionGroup = ActionGroups[i];
                string[] ArrayGroup = (ActionGroup as String).Split(',');
                string labelGroup = string.Empty;

                StringBuilder linkGroup = new StringBuilder();
                linkGroup.AppendLine("&nbsp;&nbsp;&nbsp;&nbsp;");
                if (sOrderBy.Length > 0 && sPage.Length > 0) //have page number and order
                    //                                         ..., DealSupport, new ...
                    linkGroup.AppendLine("<a class='gridlink' onclick='GetNewsOfCompetitorByProduct(" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, group = ArrayGroup[1], order = sOrderBy, asc = sAsc, page = sPage }) + "','ResultNewsContent');'> " + ArrayGroup[0] + " </a> &nbsp;&nbsp;&nbsp;&nbsp;");
                else
                    if (sOrderBy.Length > 0 && sPage.Length > 0) //have page number
                        linkGroup.AppendLine("<a class='gridlink' onclick='GetNewsOfCompetitorByProduct(" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, group = ArrayGroup[1], asc = sAsc, page = sPage }) + "','ResultNewsContent');'> " + ArrayGroup[0] + " </a> &nbsp;&nbsp;&nbsp;&nbsp;");
                    else
                        linkGroup.AppendLine("<a class='gridlink' onclick='GetNewsOfCompetitorByProduct(" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, group = ArrayGroup[1] }) + "','ResultNewsContent');'> " + ArrayGroup[0] + " </a> &nbsp;&nbsp;&nbsp;&nbsp;");

                if (sGroupBy.Length > 0 && sGroupBy.Equals(ArrayGroup[1]))
                    sb.AppendLine("<i>" + linkGroup.ToString() + "</i>");
                else
                    sb.AppendLine(linkGroup.ToString());
                if (i < (GroupsLength - 1))
                    sb.Append("-");
            }

            sb.AppendLine("</div>");
            return sb.ToString();
        }

        private static string MakeTableHeader(string Name, string XmlSource, IList<string> titles, IList<string> fields, string ColumnSizes, string sGroupBy, string sOrderBy, string sAsc, string sPage, UrlHelper url, string sIndustry, string sCompetitor, string pShowAll, string pDateFilter)
        {
            return MakeTableHeader(Name, XmlSource, titles, fields, ColumnSizes, sGroupBy, sOrderBy, sAsc, sPage, url, sIndustry, sCompetitor, pShowAll, pDateFilter, true);
        }

        private static string MakeTableHeader(string Name, string XmlSource, IList<string> titles, IList<string> fields, string ColumnSizes, string sGroupBy, string sOrderBy, string sAsc, string sPage, UrlHelper url, string sIndustry, string sCompetitor, string pShowAll, string pDateFilter, bool showColumnAction)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table id='" + Name + "' class='sortable' cellpadding=0 cellspacing=0 border=0 style='width: 100%;margin:0px'>");

            sb.AppendLine("<colgroup>");
            string[] ArrayColumnSizes = ColumnSizes.Split(',');
            foreach (string ColumnSize in ArrayColumnSizes)
            {
                sb.AppendLine("  <col width= '" + ColumnSize + "' />");
            }
            sb.AppendLine("</colgroup>");


            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");


            //TagBuilder colNumber = new TagBuilder("th");
            //colNumber.Attributes.Add("style", "padding: 0; margin: 0;width:3px");
            //sb.AppendLine(colNumber.ToString());

            for (int i = 1; i < titles.Count; i++)
            {
                TagBuilder linkCol = new TagBuilder("a");

                if (sGroupBy.Length > 0 && sPage.Length > 0)
                {
                    if (sAsc.Equals("DESC"))
                        linkCol.Attributes["href"] = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, order = fields[i], page = sPage, showAll = pShowAll, dateFilter = pDateFilter });
                    else
                        linkCol.Attributes["href"] = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, group = sGroupBy, order = fields[i], asc = "DESC", page = sPage, showAll = pShowAll, dateFilter = pDateFilter });

                }
                else
                {
                    if (sOrderBy.Equals(fields[i])) //it's Change to Descending
                    {
                        if (sAsc.Equals("DESC"))
                            linkCol.Attributes["href"] = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, order = fields[i], showAll = pShowAll, dateFilter = pDateFilter });
                        else
                            linkCol.Attributes["href"] = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, order = fields[i], asc = "DESC", showAll = pShowAll, dateFilter = pDateFilter });
                    }
                    else
                        linkCol.Attributes["href"] = url.Action("Index", XmlSource, new { Industry = sIndustry, Competitor = sCompetitor, order = fields[i], showAll = pShowAll, dateFilter = pDateFilter });
                }


                linkCol.InnerHtml = titles[i];
                TagBuilder col = new TagBuilder("th") { InnerHtml = "<h3>" + linkCol.ToString() + "</h3>" };

                if (sOrderBy.Length > 0 && fields[i].Equals(sOrderBy))
                {
                    if (sAsc.Length > 0)
                        col.AddCssClass("desc");
                    else
                        col.AddCssClass("asc");
                }
                col.AddCssClass("head");
                sb.Append(col.ToString());
            }
            if (showColumnAction)
            {
                TagBuilder colAction = new TagBuilder("th") { InnerHtml = "<h3>Actions</h3>" };
                colAction.AddCssClass("nosort");
                sb.Append(colAction.ToString());
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            return sb.ToString();
        }

        private static string MakeTableFooter(string Name, string XmlSource, IList<string> titles, IList<string> fields, string entityId, string entityType, string ColumnSizes, string sGroupBy, string sOrderBy, string sAsc, string sPage, UrlHelper url, string sIndustry, string sCompetitor)
        {
            return MakeTableFooter(Name, XmlSource, titles, fields, entityId, entityType, ColumnSizes, sGroupBy, sOrderBy, sAsc, sPage, url, sIndustry, sCompetitor, string.Empty);
        }

        private static string MakeTableFooter(string Name, string XmlSource, IList<string> titles, IList<string> fields, string entityId, string entityType, string ColumnSizes, string sGroupBy, string sOrderBy, string sAsc, string sPage, UrlHelper url, string sIndustry, string sCompetitor, string sClientCompany)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table id='" + Name + "' class='sortable' cellpadding=0 cellspacing=0 border=0 style='width: 100%;margin:0px;table-layout:fixed'>");

            sb.AppendLine("<colgroup>");
            string[] ArrayColumnSizes = ColumnSizes.Split(',');
            foreach (string ColumnSize in ArrayColumnSizes)
            {
                sb.AppendLine("  <col width= " + ColumnSize + "/>");
            }
            sb.AppendLine("</colgroup>");


            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");

            TagBuilder colNumber = new TagBuilder("th");
            colNumber.Attributes.Add("style", "padding: 0; margin: 0;");
            sb.AppendLine(colNumber.ToString());
            string salesClientCompnay = string.Empty;
            if (!string.IsNullOrEmpty(sClientCompany))
            {
                salesClientCompnay = Encryptor.Encode(sClientCompany);
            }
            for (int i = 0; i < titles.Count; i++)
            {
                TagBuilder linkCol = new TagBuilder("a");

                if (sGroupBy.Length > 0 && sPage.Length > 0)
                {
                    if (sAsc.Equals("DESC"))
                    {
                        //linkCol.Attributes["href"] = "#";
                        linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, order = fields[i], page = sPage, C = salesClientCompnay }) + "','ResultNewsContent')";
                        //linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator") + "','ResultNewsContent')";
                    }
                    else
                    {
                        //linkCol.Attributes["href"] = "#";
                        linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, group = sGroupBy, order = fields[i], asc = "DESC", page = sPage, C = salesClientCompnay }) + "','ResultNewsContent')";
                        //                       linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator") + "','ResultNewsContent')";
                    }
                }
                else
                {
                    if (sOrderBy.Equals(fields[i])) //it's Change to Descending
                    {
                        if (sAsc.Equals("DESC"))
                        {
                            //linkCol.Attributes["href"] = "#";
                            linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, order = fields[i], C = salesClientCompnay }) + "','ResultNewsContent')";
                            //linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator") + "','ResultNewsContent')";
                        }
                        else
                        {
                            //linkCol.Attributes["href"] = "#";
                            linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, order = fields[i], asc = "DESC", C = salesClientCompnay }) + "','ResultNewsContent')";
                            //                           linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator") + "','ResultNewsContent')";
                        }
                    }
                    else
                    {
                        //linkCol.Attributes["href"] = "#";
                        linkCol.Attributes["onclick"] = "GetNewsOfCompetitorByProduct('" + url.Action("GetNewsByProduct", "Comparinator", new { CompetitorId = entityId, EntityType = entityType, order = fields[i], C = salesClientCompnay }) + "','ResultNewsContent')";
                    }
                }


                linkCol.InnerHtml = titles[i];
                TagBuilder col = new TagBuilder("th") { InnerHtml = "<h3>" + linkCol.ToString() + "</h3>" };

                if (sOrderBy.Length > 0 && fields[i].Equals(sOrderBy))
                {
                    if (sAsc.Length > 0)
                        col.AddCssClass("desc");
                    else
                        col.AddCssClass("asc");
                }
                col.AddCssClass("head");
                sb.Append(col.ToString());
            }

            //TagBuilder colAction = new TagBuilder("th") { InnerHtml = "<h3>Actions</h3>" };
            //colAction.AddCssClass("nosort");
            //sb.Append(colAction.ToString());
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            return sb.ToString();
        }

        public static int FindInArray<T>(T Key, string[] Collection)
        {
            for (int i = 0; i < Collection.Length; i++)
            {
                if (Collection[i].Equals(Key))
                {
                    return i;
                }
            }
            return -1;
        }

        public static string StaticListGrid(this HtmlHelper helper, string Name, string XmlSource, string BrowseDetailFilter, string entityId, string entityType, string CC, string CssHeader, string[] ActionButtons, string[] ActionGroups, string[] ActionColumn, String ColumnSizes)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder sb = new StringBuilder();

            BrowseController bc = new BrowseController();
            IApplicationContext context = ContextRegistry.GetContext();
            bc = (BrowseController)context.GetObject("BrowseController", typeof(BrowseController));


            var sPage = helper.ViewContext.HttpContext.Request["page"] ?? "0";
            var sGroupBy = helper.ViewContext.HttpContext.Request["group"] ?? string.Empty;
            var sOrderBy = helper.ViewContext.HttpContext.Request["order"] ?? "Score";
            var sAsc = helper.ViewContext.HttpContext.Request["asc"] ?? "";


            int CurrentPage = int.Parse(sPage) == 0 ? 1 : int.Parse(sPage);
            StaticGridData SGD = new StaticGridData();
            if (string.IsNullOrEmpty(CC))
            {
                SGD = bc.GetStaticGridDataNews(XmlSource, helper.ViewContext.HttpContext, "", "Session", CurrentPage, sGroupBy, sOrderBy, sAsc, BrowseDetailFilter);
            }
            else
            {
                SGD = bc.GetStaticGridDataNews(XmlSource, helper.ViewContext.HttpContext, CC, "Request", CurrentPage, sGroupBy, sOrderBy, sAsc, BrowseDetailFilter);
            }
            IList<string> titles = SGD.titles; //titles
            IList<string> fields = SGD.fields; //fields
            int records = SGD.rowsbypage; //rowsbypage
            int totalpages = SGD.totalPages; //totalpages
            object[] rows = SGD.rows; //data


            sb.AppendLine("<div style='width:100%'>");
            sb.AppendLine("<div style='width:35%;float:left'>");//float:left
            String TopButtons = string.Empty;

            int k = 0;
            foreach (string ActionButton in ActionButtons)
            {
                string[] ArrayButton = (ActionButton as String).Split(',');
                TopButtons += "<input class='shortButton' type='button' value='" + ArrayButton[0] + "'   onclick=\"GenericFunction" + k + "();return false;\" />";
                sb.AppendLine(MakeScript("GenericFunction" + k, ArrayButton[1]));
                k++;
            }

            if (records > 0)
                sb.AppendLine(TopButtons);

            sb.AppendLine("</div>");

            sb.AppendLine(MakeTableGroupFooter(entityId, entityType, XmlSource, ActionGroups, sGroupBy, sOrderBy, sAsc, sPage, url));

            sb.AppendLine("</div>");
            //sb.AppendLine("<br>");

            //
            // Table for Group
            //

            String GroupField = string.Empty;
            int iGroupField = FindInArray<string>(sGroupBy, fields.ToArray());
            int i = 0;
            while (i < records)
            {
                object Id = SGD.GetField(i, "id");
                object[] columns = SGD.GetAllFields(i, "cell");
                if (string.IsNullOrEmpty(CC))
                {
                    sb.AppendLine(MakeTableFooter(Name, XmlSource, titles, fields, entityId, entityType, ColumnSizes, sGroupBy, sOrderBy, sAsc, sPage, url, string.Empty, string.Empty));
                }
                else
                {
                    sb.AppendLine(MakeTableFooter(Name, XmlSource, titles, fields, entityId, entityType, ColumnSizes, sGroupBy, sOrderBy, sAsc, sPage, url, string.Empty, string.Empty, CC));
                }
                sb.AppendLine("<tbody>");
                if (sGroupBy.Length > 0)
                    GroupField = (string)columns[iGroupField];

                while (i < records && (sGroupBy.Length == 0 ? true : GroupField == (string)columns[iGroupField]))
                {
                    sb.AppendLine("<tr id='" + Id.ToString() + "' ondblclick='GenericFunction0();return false;' onMouseover=\"this.bgColor='#EEEEEE'\" onMouseout=\"this.bgColor='#FFFFFF'\">");
                    sb.AppendLine("<td class='collapsible_alt'></td>");

                    for (int j = 1; j < titles.Count + 1; j++)
                    {
                        string value = (string)columns[j];
                        TagBuilder col = new TagBuilder("td") { InnerHtml = value };
                        sb.Append(col.ToString());
                    }

                    //TagBuilder DivRating = new TagBuilder("div") { InnerHtml = helper.RatingStarts(decimal.Parse(Id.ToString()), 0, 0, url.Action("Rating", "DealSupport"), true).ToString() };
                    //DivRating.Attributes["style"] = "display:none";
                    //DivRating.Attributes["id"] = "D" + Id.ToString();

                    //sb.AppendLine(DivRating.ToString());
                    String Buttons = string.Empty;

                    //foreach (string ActionCol in ActionColumn)
                    //{
                    //    string[] ArrayCol = (ActionCol as String).Split(',');
                    //    Buttons += "<a href='javascript:void(0)' onclick=\"" + ArrayCol[1] + "('" + Id.ToString() + "')\" >" + ArrayCol[0] + "</a>";
                    //}

                    //TagBuilder divAction = new TagBuilder("div") { InnerHtml = Buttons.ToString() };
                    //divAction.Attributes["style"] = "width:60px";

                    //TagBuilder tdAction = new TagBuilder("td") { InnerHtml = divAction.ToString() };
                    //tdAction.Attributes["style"] = "text-align:center";
                    //sb.AppendLine(tdAction.ToString());

                    sb.AppendLine("</tr>");
                    i++;
                    if (i >= records)
                        break;
                    Id = SGD.GetField(i, "id");
                    columns = SGD.GetAllFields(i, "cell");
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");
            }

            if (records > 0)
            {
                if (string.IsNullOrEmpty(CC))
                {
                    sb.AppendLine(MakePagerFooter("Comparinator", "GetNewsByProduct", entityId, entityType, XmlSource, CurrentPage, totalpages, sGroupBy, sOrderBy, sAsc, url, SGD.totalRecords, SGD.rowsbypage, SGD.pageSize, string.Empty, string.Empty, string.Empty));
                }
                else
                {
                    sb.AppendLine(MakePagerFooter("Comparinator", "GetNewsByProduct", entityId, entityType, XmlSource, CurrentPage, totalpages, sGroupBy, sOrderBy, sAsc, url, SGD.totalRecords, SGD.rowsbypage, SGD.pageSize, string.Empty, string.Empty, string.Empty, CC));
                }
            }
            return sb.ToString();
        }

        public static string StaticGrid(this HtmlHelper helper, string Name, string XmlSource, string CssHeader, string[] ActionButtons, string[] ActionGroups, string[] ActionColumn, String ColumnSizes, string urlImage)
        {
            return StaticGrid(helper, Name, XmlSource, CssHeader, ActionButtons, ActionGroups, ActionColumn, ColumnSizes, urlImage, "false");
        }

        public static string StaticGrid(this HtmlHelper helper, string Name, string XmlSource, string CssHeader, string[] ActionButtons, string[] ActionGroups, string[] ActionColumn, String ColumnSizes, string urlImage, string disabledPublicComment)
        {
            bool disabledPC = false;
            if (!string.IsNullOrEmpty(disabledPublicComment))
            {
                disabledPC = Convert.ToBoolean(disabledPublicComment);
            }
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            string[] array = new string[] { };
            string result = string.Empty;
            StringBuilder sb = new StringBuilder();

            BrowseController bc = new BrowseController();

            IApplicationContext context = ContextRegistry.GetContext();
            bc = (BrowseController)context.GetObject("BrowseController", typeof(BrowseController));


            var sPage = helper.ViewContext.HttpContext.Request["page"] ?? "0";
            var sGroupBy = helper.ViewContext.HttpContext.Request["group"] ?? string.Empty;
            var sOrderBy = helper.ViewContext.HttpContext.Request["order"] ?? "Name";
            if (string.IsNullOrEmpty(sOrderBy.ToString())) { sOrderBy = "Name"; }
            var sAsc = helper.ViewContext.HttpContext.Request["asc"] ?? "";
            // For Event
            var sIndustry = helper.ViewContext.HttpContext.Request["Industry"] ?? "";
            var sCompetitor = helper.ViewContext.HttpContext.Request["Competitor"] ?? "";
            //var dateFilter = helper.ViewContext.HttpContext.Request["dateFilter"] ?? "";
            //var showAll = helper.ViewContext.HttpContext.Request["showAll"] ?? "";
            var showAll = (string)helper.ViewData["ShowAll"];
            var dateFilter = (string)helper.ViewData["dateFilter"];
            // For Deal

            if (showAll == null || showAll.Equals(""))
            {
                showAll = helper.ViewContext.HttpContext.Request["showAll"] ?? "";
            }
            bool showColumnAction = true;
            if (ActionColumn.Length <= 1 && disabledPC)
            {
                showColumnAction = false;
            }
            if (sIndustry != null && !string.IsNullOrEmpty(sIndustry.ToString())) { result += "Industry," + sIndustry.ToString() + ";"; }
            if (sCompetitor != null && !string.IsNullOrEmpty(sCompetitor.ToString())) { result += "Competitor," + sCompetitor.ToString() + ";"; }
            if (showAll != null && !string.IsNullOrEmpty(showAll.ToString())) { result += "EventsDealCheckbox," + showAll.ToString() + ";"; }
            if (dateFilter != null && !string.IsNullOrEmpty(dateFilter.ToString())) { result += "txtStartDate," + dateFilter.ToString() + ";"; }

            int CurrentPage = int.Parse(sPage) == 0 ? 1 : int.Parse(sPage);

            StaticGridData SGD = bc.GetStaticGridData(XmlSource, helper.ViewContext.HttpContext.Session, CurrentPage, sGroupBy, sOrderBy, sAsc, sIndustry, sCompetitor, showAll, dateFilter);

            IList<string> titles = new List<String>(); //titles
            String[] IndLabel = ActionGroups[3].Split(',');
            String[] ComLabel = ActionGroups[2].Split(',');
            foreach (var tit in SGD.titles)
            {
                if (tit.Equals("Industry"))
                    titles.Add(IndLabel[0]);
                else if (tit.Equals("Competitor"))
                    titles.Add(ComLabel[0]);
                else
                    titles.Add(tit);
            }

            IList<string> fields = SGD.fields; //fields
            int records = SGD.rowsbypage; //rowsbypage
            int totalpages = SGD.totalPages; //totalpages
            object[] rows = SGD.rows; //data


            sb.AppendLine("<div style='width:100%'>");
            sb.AppendLine("<div style='width:35%;float:left'>");//float:left
            String TopButtons = string.Empty;

            int k = 0;
            foreach (string ActionButton in ActionButtons)
            {
                string[] ArrayButton = (ActionButton as String).Split(',');
                TopButtons += "<input class='shortButton' type='button' value='" + ArrayButton[0] + "'   onclick=\"GenericFunction" + k + "();return false;\" />";
                sb.AppendLine(MakeScript("GenericFunction" + k, ArrayButton[1]));
                k++;
            }

            if (records > 0)
                sb.AppendLine(TopButtons);

            sb.AppendLine("</div>");
            if (string.IsNullOrEmpty(result) || XmlSource.IndexOf("Event") == -1)
            {
                sb.AppendLine(MakeTableGroup(XmlSource, ActionGroups, sGroupBy, sOrderBy, sAsc, sPage, url));
            }
            else
            {
                sb.AppendLine(MakeTableGroup(XmlSource, ActionGroups, sGroupBy, sOrderBy, sAsc, sPage, url, result.Split(';')));
            }
            sb.AppendLine("</div>");
            sb.AppendLine("<br>");

            //
            // Table for Group
            //

            String GroupField = string.Empty;
            int iGroupField = FindInArray<string>(sGroupBy, fields.ToArray());
            int i = 0;
            while (i < records)
            {
                object Id = SGD.GetField(i, "id");
                object[] columns = SGD.GetAllFields(i, "cell");

                if (showColumnAction)
                    sb.AppendLine(MakeTableHeader(Name, XmlSource, titles, fields, ColumnSizes, sGroupBy, sOrderBy, sAsc, sPage, url, sIndustry, sCompetitor, showAll, dateFilter));
                else
                {
                    sb.AppendLine(MakeTableHeader(Name, XmlSource, titles, fields, ColumnSizes, sGroupBy, sOrderBy, sAsc, sPage, url, sIndustry, sCompetitor, showAll, dateFilter, showColumnAction));
                }
                sb.AppendLine("<tbody>");
                if (sGroupBy.Length > 0)
                    GroupField = (string)columns[iGroupField];

                while (i < records && (sGroupBy.Length == 0 ? true : GroupField == (string)columns[iGroupField]))
                {
                    sb.AppendLine("<tr id='" + Id.ToString() + "' ondblclick='GenericFunction0();return false;' onMouseover=\"this.bgColor='#EEEEEE'\" onMouseout=\"this.bgColor='#FFFFFF'\">");
                    //sb.AppendLine("<td class='collapsible_alt'></td>");

                    for (int j = 1; j < titles.Count; j++)
                    {
                        string value = (string)columns[j];
                        TagBuilder col = new TagBuilder("td") { InnerHtml = value };
                        sb.Append(col.ToString());
                    }

                    TagBuilder DivRating = new TagBuilder("div") { InnerHtml = helper.RatingStarts(decimal.Parse(Id.ToString()), 0, 0, url.Action("Rating", "DealSupport"), true).ToString() };
                    DivRating.Attributes["style"] = "display:none";
                    DivRating.Attributes["id"] = "D" + Id.ToString();

                    sb.AppendLine(DivRating.ToString());

                    if (showColumnAction)
                    {
                        String Buttons = string.Empty;
                        string showImage = urlImage;
                        int count = 0;

                        foreach (string ActionCol in ActionColumn)
                        {
                            if (count == 0)
                            {
                                if (!disabledPC)
                                {
                                    string[] ArrayCol = (ActionCol as String).Split(',');

                                    if (ArrayCol.Length > 1)
                                    {
                                        bool showComments = bc.verifiedComments(Convert.ToDecimal(Id), ArrayCol[1]);

                                        if (!showComments)
                                        {
                                            Buttons += "<a href='javascript:void(0)' onclick=\"" + ArrayCol[1] + "('" + Id.ToString() + "')\" >" + ArrayCol[0] + "<img id='ImgComents" + Id.ToString() + "' class='ImageCommentsN' style='width: 22px;' src=" + showImage + "></a>";
                                        }
                                        else
                                        {
                                            Buttons += "<a href='javascript:void(0)' onclick=\"" + ArrayCol[1] + "('" + Id.ToString() + "')\" >" + ArrayCol[0] + "<img id='ImgComents" + Id.ToString() + "' class='ImageCommentsY'  style='width: 22px;' src=" + showImage + "></a>";
                                        }
                                    }
                                }
                                count++;
                            }
                            else
                            {
                                string[] ArrayCol = (ActionCol as String).Split(',');
                                Buttons += "<a href='javascript:void(0)' onclick=\"" + ArrayCol[1] + "('" + Id.ToString() + "')\" >" + ArrayCol[0] + "</a>";
                            }
                        }

                        TagBuilder divAction = new TagBuilder("div") { InnerHtml = Buttons.ToString() };
                        divAction.Attributes["style"] = "width:60px";

                        TagBuilder tdAction = new TagBuilder("td") { InnerHtml = divAction.ToString() };
                        tdAction.Attributes["style"] = "text-align:center";
                        sb.AppendLine(tdAction.ToString());
                    }
                    sb.AppendLine("</tr>");
                    i++;
                    if (i >= records)
                        break;
                    Id = SGD.GetField(i, "id");
                    columns = SGD.GetAllFields(i, "cell");
                }
                sb.AppendLine("</tbody>");
                sb.AppendLine("</table>");
            }

            if (records > 0)
                sb.AppendLine(MakePager(XmlSource, CurrentPage, totalpages, sGroupBy, sOrderBy, sAsc, url, SGD.totalRecords, SGD.rowsbypage, SGD.pageSize, sIndustry, sCompetitor, showAll));


            return sb.ToString();
        }

    }
}
