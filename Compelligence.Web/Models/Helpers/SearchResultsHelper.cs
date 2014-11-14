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
using Compelligence.Common.Search;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Compelligence.Domain.Entity.Views;
using Resources;

namespace Compelligence.Web.Models.Helpers
{
    public static class SearchResultsHelper
    {

        public static string ItemToSearch(this HtmlHelper helper, ProjectFileShowView item)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(item.FileVersion))
            {
                result.Append("<div class='lineResult'>");
                string divId = string.Empty;
                string labelToSpan = string.Empty;
                string labelDescription = LabelResource.ProjectDescription;
                if (item.FileVersion.Equals("URLNews")) {
                    divId = "urlNews" + item.Id;
                    labelToSpan = "News";
                }
                else if (item.FileVersion.Equals("URLDeal"))
                {
                    divId = "urlDeal";
                    labelToSpan = "Deal";
                    labelDescription = LabelResource.DealDetails;
                }
                else if (item.FileVersion.Equals("URLEvent"))
                {
                    divId = "urlEvent";
                    labelToSpan = "Event";
                    labelDescription = LabelResource.Comment;
                }
                else if (item.FileVersion.Equals("URLProduct"))
                {
                    divId = "urlProduct";
                    labelToSpan = "Product";
                }
                else if (item.FileVersion.Equals("URLObjective"))
                {
                    divId = "urlObjective";
                    labelToSpan = "Objective";
                    labelDescription = LabelResource.ObjectiveDetail;
                }
                else if (item.FileVersion.Equals("URLCalendar"))
                {
                    divId = "urlDeal";
                    labelToSpan = "Deal";
                }
                else if (item.FileVersion.Equals("URLKit")) 
                {
                    divId = "urlKit";
                    labelToSpan = "Kit";
                    labelDescription = LabelResource.KitComment;
                }
                else if (item.FileVersion.Equals("URLIndustry"))
                {
                    divId = "urlIndustry";
                    labelToSpan = "Industry";
                }
                else if (item.FileVersion.Equals("URLCompetitor"))
                {
                    divId = "urlCompetitor";
                    labelToSpan = "Competitor";
                }
                else if (item.FileVersion.Equals("URLCustomer"))
                {
                    divId = "urlCustomer";
                    labelToSpan = "Customer";
                }
                else
                {



                }
                if (item.FileVersion.Equals("URLNews") || item.FileVersion.Equals("URLDeal") || item.FileVersion.Equals("URLEvent")
                      || item.FileVersion.Equals("URLProduct") || item.FileVersion.Equals("URLObjective")
                      || item.FileVersion.Equals("URLCalendar") || item.FileVersion.Equals("URLKit")
                      || item.FileVersion.Equals("URLIndustry") || item.FileVersion.Equals("URLCompetitor")
                      || item.FileVersion.Equals("URLCustomer"))
                {
                    result.Append("<div id='" + divId + "' style='visibility:hidden;'>" + item.FilePhysicalName + "</div>");
                }
                result.Append("<div class='itemLabel'>");
                result.Append("<span class='under14'>");
                if (!string.IsNullOrEmpty(labelToSpan))
                {
                    result.Append(labelToSpan +": ");
                }
                if (!item.FileVersion.Equals("URLNews") && !item.FileVersion.Equals("URLDeal") && !item.FileVersion.Equals("URLEvent")
                      && !item.FileVersion.Equals("URLProduct") && !item.FileVersion.Equals("URLObjective")
                      && !item.FileVersion.Equals("URLCalendar") && !item.FileVersion.Equals("URLKit")
                      && !item.FileVersion.Equals("URLIndustry") && !item.FileVersion.Equals("URLCompetitor")
                      && !item.FileVersion.Equals("URLCustomer"))
                {
                    string urlAction = url.Action("Download", "ContentSearch", new { id = item.Id });
                    result.Append("     <a href=\"javascript: void(0);\" onclick=\"javascript: return downloadFile('"+urlAction+"');\">");
                }
                else if (item.FileVersion.Equals("URLNews"))
                {
                    result.Append("<a href=\"javascript: void(0);\" onclick=\"loadUrl('" + item.Id + "');\">" );
                }
                else if (item.FileVersion.Equals("URLDeal"))
                {
                    result.Append("<a href=\"javascript: void(0);\" onclick=\"DealEdit('" + item.Id + "');\">");
                }
                else if (item.FileVersion.Equals("URLEvent"))
                {
                    result.Append("<a href=\"javascript: void(0);\" onclick=\"EventEdit('" + item.Id + "');\">");
                }
                else 
                { }
                result.Append(item.ProjectName);
                if ( !item.FileVersion.Equals("URLProduct") && !item.FileVersion.Equals("URLObjective")
                      && !item.FileVersion.Equals("URLCalendar") && !item.FileVersion.Equals("URLKit")
                      && !item.FileVersion.Equals("URLIndustry") && !item.FileVersion.Equals("URLCompetitor")
                      && !item.FileVersion.Equals("URLCustomer"))
                { result.Append("     </a>"); }

                
                
                result.Append("</span>");
                result.Append("</div>");
                if (!item.FileVersion.Equals("URLNews") && !item.FileVersion.Equals("URLDeal") && !item.FileVersion.Equals("URLEvent")
                      && !item.FileVersion.Equals("URLProduct") && !item.FileVersion.Equals("URLObjective")
                      && !item.FileVersion.Equals("URLCalendar") && !item.FileVersion.Equals("URLKit")
                      && !item.FileVersion.Equals("URLIndustry") && !item.FileVersion.Equals("URLCompetitor")
                      && !item.FileVersion.Equals("URLCustomer"))
                {
                    if (!string.IsNullOrEmpty(item.TextToDisplay))
                    {
                        result.Append("<div class='itemResult'>");
                        result.Append("<label class='labelResult'>");
                        result.Append(" Text To Display:");
                        result.Append("</label>");
                        result.Append(" <div class='divResult'>");
                        result.Append(item.TextToDisplay);
                        result.Append(" </div>");
                        result.Append("</div>");
                    }
                }

                if (!string.IsNullOrEmpty(item.Description))
                {
                    result.Append("<div class='itemResult'>");
                    result.Append("<label class='labelResult'>");
                    result.Append(labelDescription+":");
                    result.Append("</label>");
                    result.Append(" <div class='divResult'>");
                    result.Append(item.Description);
                    result.Append(" </div>");
                    result.Append("</div>");
                }
                

                result.Append("</div>");
            }
            return result.ToString();
        }

        public static string ShowSearchResults(this HtmlHelper helper)
        {
            IList<object> resultFoundObjects = (IList<object>)helper.ViewData["SearchResults"];
            IList<SearchObject> searchObjects = SearchManager.GetInstance().GetSearchObjects();
            int indexSearchObject = 0;
            StringBuilder results = new StringBuilder();

            foreach (object listFoundObject in resultFoundObjects)
            {
                SearchObject searchObject = searchObjects[indexSearchObject];
                indexSearchObject++;
                IList<object> foundObjects = (IList<object>)listFoundObject;
                var divTagBuilderEntity = new TagBuilder("div");
                var divTagBuilderLabelEntitity = new TagBuilder("div");
                var tableTagBuilder = new TagBuilder("table");
                var divTagBuilder = new TagBuilder("div");
                var headerTagBuilder = new TagBuilder("tr");

                tableTagBuilder.AddCssClass("stats");

                divTagBuilderLabelEntitity.InnerHtml = "<h1>" + searchObject.Label + "</h1><br> Found results " + foundObjects.Count;
                divTagBuilderEntity.InnerHtml += divTagBuilderLabelEntitity.ToString();
                //foreach (string label in searchObject.LabelsShow)
                //{
                //    var cellTagBuilder = new TagBuilder("th");
                //    cellTagBuilder.InnerHtml = label;
                //        //(string)HttpContext.GetGlobalResourceObject("LabelResource", label); 

                //    headerTagBuilder.InnerHtml += cellTagBuilder.ToString();
                //}
                tableTagBuilder.InnerHtml += headerTagBuilder.ToString();
                foreach (object foundObject in foundObjects)
                {
                    var rowTagBuilder = new TagBuilder("tr");
                    Type classEntity = foundObject.GetType();
                    string stringProperties = string.Empty;
                    //foreach (string property in searchObject.PropertiesShow)
                    //{
                    //    var cellTagBuilder = new TagBuilder("td");
                    //    PropertyInfo propertyEntity = classEntity.GetProperty(property);
                    //    cellTagBuilder.InnerHtml = 
                    //        propertyEntity.GetValue(foundObject, null)==null?
                    //        "null":
                    //        propertyEntity.GetValue(foundObject, null).ToString();

                    //    rowTagBuilder.InnerHtml += cellTagBuilder.ToString();
                    //}
                    tableTagBuilder.InnerHtml += rowTagBuilder.ToString();
                }
                divTagBuilder.InnerHtml = tableTagBuilder.ToString();
                divTagBuilderEntity.InnerHtml += divTagBuilder.ToString();
                results.Append(divTagBuilderEntity.ToString());
            }

            return results.ToString();
        }

    }
}
