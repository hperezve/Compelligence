using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Compelligence.DataTransfer.FrontEnd;
using System.Web.Mvc;
using Compelligence.Domain.Entity.Resource;
using Resources;
using Compelligence.Domain.Entity;
using Compelligence.Util.Type;

namespace Compelligence.Web.Models.Helpers
{
    public static class LeftContentHelper
    {
        private static string BuildContentToIndustries(UrlHelper url, string idTemp, LibraryCatalog oLibraryCatalog)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("      <div id='" + idTemp + "'  class='contentBoxDataList'>");
            foreach (var oIndustry in oLibraryCatalog.Industries)
            {
                contentPanels.Append("          <div class='contentAreaItems' style='border-bottom: 1px solid rgb(204, 204, 204);' >");
                contentPanels.Append("            <div class='tip'>");
                contentPanels.Append("              <a href='javascript:void(0)' onclick=\"javascript:setInd( '" + oIndustry.Id + "');\" >");
                contentPanels.Append(oIndustry.Name);
                contentPanels.Append("              </a><br />");
                contentPanels.Append("              <div class='tipbox'>");
                contentPanels.Append("                  <div class='tiptitle'>Description</div>");
                contentPanels.Append("                  <div class='tipdescription'>" + oIndustry.Description + "</div>");
                contentPanels.Append("              </div>");
                contentPanels.Append("             </div>");
                contentPanels.Append("          </div>");
            }
            contentPanels.Append("      </div>");
            return contentPanels.ToString();
        }
        public static string BuildTableToFinancial(IList<CompetitorFinancialIncomeStatement> competitorFinancialIncomeStatement)
        {
            StringBuilder contentPanels = new StringBuilder();
            // TABLE ANUAL
            contentPanels.Append("<table id='IC_" + competitorFinancialIncomeStatement[0].CompetitorId + "'  class='tbl_comp_fin_annual' width='100%'>");
            // SET LABELS
            contentPanels.Append("<tr style='font-weight: bold;'>");
            contentPanels.Append("<th class='first-column-labels'></th>");
            int counterLabelAnnual = 0;
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Annual))
                {
                    contentPanels.Append("<th class='column-values'>" + DateTimeUtility.ConvertToString(competitorFinancialIncomeStatement[comFin].PeriodEnding, "MM/dd/yyyy") + "</th>");
                    counterLabelAnnual++;
                }
            }
            if (counterLabelAnnual < 5)
            {
                for (int cla = 0; cla < (5-counterLabelAnnual); cla++)
                {

                    contentPanels.Append("<th  class='column-values'></th>");
                }
            }
            contentPanels.Append("</tr>");

            ////TOTAL INCOME
            contentPanels.Append("<tr>");
            contentPanels.Append("<td  style='font-weight: bold;'>Total Income</td>");
            int counterTIA = 0; // COUNTER TO TOTAL INCOME 
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Annual))
                {
                    if (string.IsNullOrEmpty(competitorFinancialIncomeStatement[comFin].TotalRevenue.Trim())) competitorFinancialIncomeStatement[comFin].TotalRevenue = "0";
                    contentPanels.Append("<td>" + competitorFinancialIncomeStatement[comFin].TotalRevenue + "</td>");
                    counterTIA++;
                }
            }
            if (counterTIA < 5)
            {
                for (int ctia = 0; ctia < (5-counterTIA); ctia++)
                {

                    contentPanels.Append("<td></td>");
                }
            }
            contentPanels.Append("</tr>");
            ///// OPERATING INCOME
            contentPanels.Append("<tr>");
            contentPanels.Append("<td  style='font-weight: bold;'>Operationg Income</td>");
            int counterOIA = 0; // COUNTER TO OPERATIONG INCOME
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Annual))
                {
                    if (string.IsNullOrEmpty(competitorFinancialIncomeStatement[comFin].OperatingIncomeorLoss.Trim())) competitorFinancialIncomeStatement[comFin].OperatingIncomeorLoss = "0";
                    contentPanels.Append("<td>" + competitorFinancialIncomeStatement[comFin].OperatingIncomeorLoss + "</td>");
                    counterOIA++;
                }
            }
            if (counterOIA < 5)
            {
                for (int coia = 0; coia < (5- counterOIA); coia++)
                {

                    contentPanels.Append("<td></td>");
                }
            }
            contentPanels.Append("</tr>");

            //// NET INCOME
            contentPanels.Append("<tr>");
            contentPanels.Append("<td  style='font-weight: bold;'>Net Income</td>");
            int counterNIA = 0; // COUNTER TO NET INCOME
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Annual))
                {
                    if (string.IsNullOrEmpty(competitorFinancialIncomeStatement[comFin].NetIncome.Trim())) competitorFinancialIncomeStatement[comFin].NetIncome = "0";
                    contentPanels.Append("<td>" + competitorFinancialIncomeStatement[comFin].NetIncome + "</td>");
                    counterNIA++;
                }
            }
            if (counterNIA < 5)
            {
                for (int cnia = 0; cnia < (5-counterNIA); cnia++)
                {
                    contentPanels.Append("<td></td>");
                }
            }
            contentPanels.Append("</tr>");
            contentPanels.Append("</table>");

            // TABLE Quarterly
            contentPanels.Append("<table id='IC_" + competitorFinancialIncomeStatement[0].CompetitorId + "'  class='tbl_comp_fin_quartely' style='display:none;' width='100%'>");
            contentPanels.Append("<tr style='font-weight: bold;'>");
            contentPanels.Append("<th  class='first-column-labels'></th>");
            int counterLabelQuarterly = 0;
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Quarterly))
                {
                    contentPanels.Append("<th class='column-values'>" + DateTimeUtility.ConvertToString(competitorFinancialIncomeStatement[comFin].PeriodEnding, "MM/dd/yyyy") + "</th>");
                    counterLabelQuarterly++;
                }
            }
            if (counterLabelQuarterly < 5)
            {
                for (int clq = 0; clq <(5- counterLabelQuarterly); clq++)
                {

                    contentPanels.Append("<th class='column-values'></th>");
                }
            }
            contentPanels.Append("</tr>");

            ////TOTAL INCOME
            contentPanels.Append("<tr>");
            contentPanels.Append("<td  style='font-weight: bold;'>Total Income</td>");
            int counterTIQ = 0; // COUNTER TO TOTAL INCOME Quarterly
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Quarterly))
                {
                    if (string.IsNullOrEmpty(competitorFinancialIncomeStatement[comFin].TotalRevenue.Trim())) competitorFinancialIncomeStatement[comFin].TotalRevenue = "0";
                    contentPanels.Append("<td>" + competitorFinancialIncomeStatement[comFin].TotalRevenue + "</td>");
                    counterTIQ++;
                }
            }
            if (counterTIQ < 5)
            {
                for (int ctiq = 0; ctiq < (5-counterTIQ); ctiq++)
                {
                    contentPanels.Append("<td></td>");
                }
            }
            contentPanels.Append("</tr>");
            ///// OPERATING INCOME
            contentPanels.Append("<tr>");
            contentPanels.Append("<td  style='font-weight: bold;'>Operationg Income</td>");
            int counterOIQ = 0; // COUNTER TO OPERATING INCOME Quarterly
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Quarterly))
                {
                    if (string.IsNullOrEmpty(competitorFinancialIncomeStatement[comFin].OperatingIncomeorLoss.Trim())) competitorFinancialIncomeStatement[comFin].OperatingIncomeorLoss = "0";
                    contentPanels.Append("<td>" + competitorFinancialIncomeStatement[comFin].OperatingIncomeorLoss + "</td>");
                    counterOIQ++;
                }
            }
            if (counterOIQ < 5)
            {
                for (int coiq = 0; coiq <(5- counterOIQ); coiq++)
                {
                    contentPanels.Append("<td></td>");
                }
            }
            contentPanels.Append("</tr>");

            //// NET INCOME
            contentPanels.Append("<tr>");
            contentPanels.Append("<td  style='font-weight: bold;'>Net Income</td>");
            int counterNIQ = 0; // COUNTER TO NET INCOME Quarterly
            for (int comFin = 0; comFin < competitorFinancialIncomeStatement.Count; comFin++)
            {
                if (competitorFinancialIncomeStatement[comFin].PeriodType.Trim().Equals(FinancialTimePeriod.Quarterly))
                {
                    if (string.IsNullOrEmpty(competitorFinancialIncomeStatement[comFin].NetIncome.Trim())) competitorFinancialIncomeStatement[comFin].NetIncome = "0";
                    contentPanels.Append("<td>" + competitorFinancialIncomeStatement[comFin].NetIncome + "</td>");
                    counterNIQ++;
                }
            }
            if (counterNIQ < 5)
            {
                for (int cniq = 0; cniq < (5-counterNIQ); cniq++)
                {
                    contentPanels.Append("<td></td>");
                }
            }
            contentPanels.Append("</tr>");
            contentPanels.Append("</table>");

            return contentPanels.ToString();
        }

        private static string BuildContentToCompetitors(UrlHelper url, string idTemp, LibraryCatalog oLibraryCatalog)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("      <div id='" + idTemp + "'  class='contentBoxDataList'>");
            foreach (var oCompetitor in oLibraryCatalog.Competitors)
            {
                contentPanels.Append("          <div class='contentAreaItems' style='border-bottom: 1px solid rgb(204, 204, 204);' >");
                contentPanels.Append("            <div class='tip'>");
                contentPanels.Append("              <a href='javascript:void(0)' onclick=\"javascript:setIndAndCompVal( '" + oCompetitor.Id + "');\" >");
                contentPanels.Append(oCompetitor.Name);
                contentPanels.Append("              </a><br />");
                contentPanels.Append("              <div class='tipbox'>");
                contentPanels.Append("                  <div class='tiptitle'>Description</div>");
                contentPanels.Append("                  <div class='tipdescription'>" + oCompetitor.Description + "</div>");
                contentPanels.Append("              </div>");
                contentPanels.Append("             </div>");
                contentPanels.Append("          </div>");
            }
            contentPanels.Append("      </div>");
            return contentPanels.ToString();
        }
        private static string BuildContentToProducts(UrlHelper url, string idTemp, LibraryCatalog oLibraryCatalog)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("      <div id='" + idTemp + "'  class='contentBoxDataList'>");
            if (oLibraryCatalog.ProductsWithCompetitor.Count > 0)
            {
                foreach (System.Object[] objt in oLibraryCatalog.ProductsWithCompetitor)
                {
                    contentPanels.Append("          <div class='contentAreaItems' style='border-bottom: 1px solid rgb(204, 204, 204);' >");
                    contentPanels.Append("             <div class='tip'>");
                    contentPanels.Append("              <a href='javascript:void(0)' onclick=\"javascript:setIndCompAndProdVal('" + objt[2] + "','" + objt[0] + "');\" >");
                    contentPanels.Append(objt[3] + " - " + objt[1]);
                    contentPanels.Append("              </a><br />");
                    contentPanels.Append("              <div class='tipbox'>");
                    contentPanels.Append("                  <div class='tiptitle'>Description</div>");
                    contentPanels.Append("                  <div class='tipdescription'>" + objt[4] + "</div>");
                    contentPanels.Append("              </div>");
                    contentPanels.Append("             </div>");
                    contentPanels.Append("          </div>");
                }
            }
            else {
                foreach (var oProduct in oLibraryCatalog.Products)
                {
                    //String urlChangeCompetitor = url.Action("ChangeCompetitor", "ContentPortal", new {Industry= oLibraryCatalog.IndustryId, Competitor = oCompetitor.Id });
                    contentPanels.Append("          <div class='contentAreaItems' style='border-bottom: 1px solid rgb(204, 204, 204);' >");
                    contentPanels.Append("            <div class='tip'>");
                    contentPanels.Append("              <a href='javascript:void(0)' onclick=\"javascript:setIndCompAndProdVal( '" + oProduct.CompetitorId + "','" + oProduct.Id + "');\" >");
                    contentPanels.Append(oProduct.Name);
                    contentPanels.Append("              </a><br />");
                    contentPanels.Append("              <div class='tipbox'>");
                    contentPanels.Append("                  <div class='tiptitle'>Description</div>");
                    contentPanels.Append("                  <div class='tipdescription'>" + oProduct.Description + "</div>");
                    contentPanels.Append("              </div>");
                    contentPanels.Append("             </div>");
                    contentPanels.Append("          </div>");
                }
            }
            contentPanels.Append("      </div>");
            return contentPanels.ToString();
        }
        private static string BuildContentToSW(UrlHelper url, string SWCompetitorId, string SWIndustryId, bool CompetitorHasComment, bool DefaultsDisabPublComm, IList<StrengthWeakness> strengthbyindustry, IList<StrengthWeakness> weaknessbyindustry)
        {
            StringBuilder contentPanels = new StringBuilder();
            String urlFeedBackContent = url.Action("FeedBackMessage", "Forum", new { EntityId = SWCompetitorId, EntityType = DomainObjectType.Competitor, IndustryId = SWIndustryId, SubmittedVia = FeedBackSubmitedVia.StrengthWeakness });
            String urlGetDiscussions = url.Action("GetDiscussions", "Forum", new { EntityId = SWCompetitorId, ObjectType = DomainObjectType.Competitor });
            String urlDiscussionsResponse = url.Action("DiscussionsResponse", "Forum", new { EntityId = SWCompetitorId, ForumResponseId = 0, ObjectType = DomainObjectType.Competitor, IndustryId = SWIndustryId });
            contentPanels.Append("<div id='items' class='contentBoxDataList'>");
            contentPanels.Append("  <div>");
            contentPanels.Append("      <div class='float-right' style='width:55px; _width:55px;'> ");
            string CompetitorIdComments = "ImgComents" + SWCompetitorId;
            bool competitorHasComments = CompetitorHasComment;
            string classToImageComments = "ImageCommentsN";
            if (competitorHasComments) { classToImageComments = "ImageCommentsY"; }

            string competitorIdObject = StringUtility.CheckNull(SWCompetitorId);
            string industryIdObject = StringUtility.CheckNull(SWIndustryId);

            if (!DefaultsDisabPublComm)
            {
                contentPanels.Append("              <a id='A1' href='javascript:void(0)' onclick=\"GetExternalDiscussionsDlg('" + urlGetDiscussions + "','Discussions Form','" + urlDiscussionsResponse + "','" + SWCompetitorId + "');\">");
                contentPanels.Append("                  <img id ='ImgComents" + SWCompetitorId + "' class='" + classToImageComments + "' src='/Content/Images/Icons/CommentWhite.png' width='22px' title='Add public comment' />");
                contentPanels.Append("              </a>");
            }
            contentPanels.Append("              <a id='A2' href='javascript: void(0);'  style='float: right;padding-right:5px;' onclick=\"ExternalFeedBackWithAttachedDlg('" + urlFeedBackContent + "','FeedBack Dialog');\">");
            contentPanels.Append("                  <img src='/Content/Images/Icons/testfeedback.gif' width='22px' title='Add private feedback' />");
            contentPanels.Append("              </a>");
            contentPanels.Append("      </div>");
            contentPanels.Append("  </div>");

            contentPanels.Append("  <u title='Click to Add new Strengths.' onclick=\"addStrength('" + SWCompetitorId + "','" + SWIndustryId + "','" + url.Action("createStrength", "ContentPortal") + "','" + url.Action("UpdateStrength", "ContentPortal") + "');\"><h3>Strengths</h3></u>");
            
            if (strengthbyindustry == null || strengthbyindustry.Count == 0)
            {
                contentPanels.Append("  <ul id='Strengths' class='listWithoutBullet '>");
                contentPanels.Append("      <li id='messageS'><b>No strengths configured for this competitor and industry</b></li>");
                contentPanels.Append("  </ul>");
            }
            else
            {
                contentPanels.Append("  <ul id='Strengths'>");
                if (strengthbyindustry.Count > 0)
                {
                    foreach (StrengthWeakness item in strengthbyindustry)
                    {
                        contentPanels.Append("<div id='strengths'>");
                        contentPanels.Append("<li id='strengths_" + item.Id.ToString() + "' title='Click to re-name saved Strength.' onclick= \"editStrength('" + item.Id + "','','','" + item.IsGlobal + "','" + url.Action("UpdateStrength", "ContentPortal") + "','" + SWIndustryId + "');\" namesw='" + item.Name + "' globalsw='" + item.IsGlobal + "'>" + item.Description + "</li>");
                        //contentPanels.Append("<button type='button' title='Right click to re-name saved Strength or Weakness.' style='background-color:white;'>");
                        //contentPanels.Append("<span class='comparison_titlec' >" + item.Name + "</span></button>");
                        //contentPanels.Append("              <div class='tiptitle'>Description</div>");
                        //contentPanels.Append("              <div class='tipdescription'>" + item.Description + "</div>");
                        //contentPanels.Append("          </div>");
                        contentPanels.Append("      </div>");
                    }
                }
                contentPanels.Append("  </ul>");//
            }
            contentPanels.Append("  <u title='Click to Add new Weakness.' onclick=\"addWeakness('" + SWCompetitorId + "','" + SWIndustryId + "','" + url.Action("createWeakness", "ContentPortal") + "','" + url.Action("UpdateWeakness", "ContentPortal") + "');\"><h3>Weaknesses</h3></u>");
            if (weaknessbyindustry == null || weaknessbyindustry.Count == 0)
            {
                contentPanels.Append("  <ul id='Weaknesses' class='listWithoutBullet'>");
                contentPanels.Append("      <li id='messageW'><b>No weaknesses configured for this competitor and industry</b></li>");
                contentPanels.Append("  </ul>");
            }
            else
            {
                contentPanels.Append("  <ul id='Weaknesses'>");
                if (weaknessbyindustry.Count > 0)                    
                {
                    foreach (StrengthWeakness item in weaknessbyindustry)
                    {
                        contentPanels.Append("      <div id='weakness'>");
                        contentPanels.Append("          <li id='weakness_" + item.Id.ToString() + "' title='Click to re-name saved Weakness.' onclick=\"editWeakness('" + item.Id + "','','','" + item.IsGlobal + "','" + url.Action("UpdateWeakness", "ContentPortal") + "','" + SWIndustryId + "');\" namesw='" + item.Name + "' globalsw='" + item.IsGlobal + "'>" + item.Description + "</li>");
                        //contentPanels.Append("          <div class='tipbox'>");
                        //contentPanels.Append("              <div class='tiptitle'>Description</div>");
                        //contentPanels.Append("              <div class='tipdescription'>" + item.Description + "</div>");
                        //contentPanels.Append("          </div>");
                        contentPanels.Append("      </div>");
                    }
                }
                contentPanels.Append("  </ul>");
            }
            contentPanels.Append("</div>");
            return contentPanels.ToString();
        }
        private static string BuildContentToEntityDetails(string idTemp, LibraryCatalog oLibraryCatalog, string ImageDetail, string UrlDetail, string UrlDetailText, string DescriptionDetail)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("<div id='DetailsList' class='contentBoxDataList'>");
            contentPanels.Append("  <table width='100%'>");
            contentPanels.Append("      <tr>");
            contentPanels.Append("          <td>");
            contentPanels.Append("              <table width='100%'>");
            contentPanels.Append("                  <tr>");
            contentPanels.Append("                       <td style='width:50%;'>");
            contentPanels.Append("                          <div class='contentBoxProject' style='color:#666666;'>");
            contentPanels.Append("                              <label for='LblLeftContentPicture'>");
            contentPanels.Append("                                  <asp:Literal ID='LtLeftContentPicture' runat='server' Text='" + LabelResource.ContentPortalLeftContentPicture + "' />");
            contentPanels.Append("                              </label>");
            contentPanels.Append("                          </div><br />");
            contentPanels.Append("                          <div>");
            if (!string.IsNullOrEmpty(ImageDetail))
            {
                string ImageDetail1 = ImageDetail;
                if (ImageDetail1.Substring(0, 2).Equals("./"))
                    ImageDetail1 = "." + ImageDetail1;
                contentPanels.Append("                          <img id='imgDetail' src='" + ImageDetail1 + "' onload=\"resize('imgDetail')\" data-tooltip='sticky1'>");
            }
            else
            {
                contentPanels.Append("                          <img id='img1' src='/Content/Images/Icons/none.png' onload=\"resize('imgDetail')\" >");
                contentPanels.Append("                          <label for='LblLeftContentMessage1'>");
                contentPanels.Append("                              <asp:Literal ID='LtLeftContentMessage1' runat='server' Text='" + LabelResource.ContentPortalLeftContentMessage1 + "' />");
                contentPanels.Append("                          </label>");
            }
            contentPanels.Append("                          </div>");
            contentPanels.Append("                      </td>");
            contentPanels.Append("                      <td style='width:50%;' valign='top'>");
            contentPanels.Append("                          <div class='contentBoxProject' style='color:#666666;'>");
            contentPanels.Append("                              <label for='LblLeftContentURL'>");
            contentPanels.Append("                                  <asp:Literal ID='LtLeftContentURL' runat='server' Text='" + LabelResource.ContentPortalLeftContentURL + "' />");
            contentPanels.Append("                              </label>");
            contentPanels.Append("                          </div><br />");
            if (!string.IsNullOrEmpty(UrlDetail))
            {
                contentPanels.Append("                          <div>");
                contentPanels.Append("                              <a href='javascript:void(0);' id='LinkEntity' onclick='loadUrl();'>" + UrlDetailText + "</a>");
                contentPanels.Append("                          </div>");
            }
            else
            {
                contentPanels.Append("                          <label for='LblLeftContentMessage2'>");
                contentPanels.Append("                              <asp:Literal ID='LtLeftContentMessage2' runat='server' Text='" + LabelResource.ContentPortalLeftContentMessage2 + "' />");
                contentPanels.Append("                          </label>");
            }
            contentPanels.Append("                      </td>");
            contentPanels.Append("                  </tr>");
            contentPanels.Append("              </table>");
            contentPanels.Append("          </td>");
            contentPanels.Append("      </tr>");
            contentPanels.Append("      <tr>");
            contentPanels.Append("          <td>");
            contentPanels.Append("              <div class='contentBoxProject' style='color:#666666;'>");
            contentPanels.Append("                  <label for='LblLeftContentDescripcion'>");
            contentPanels.Append("                      <asp:Literal ID='LtLeftContentDescripcion' runat='server' Text='" + LabelResource.ContentPortalLeftContentDescripcion + "' />");
            contentPanels.Append("                  </label>");
            contentPanels.Append("              </div><br />");
            contentPanels.Append("              <div class='AddTargetBlankToLin'>");
            if (!string.IsNullOrEmpty(DescriptionDetail))
            {
                contentPanels.Append(DescriptionDetail);
            }
            else
            {
                contentPanels.Append("              <label for='LblLeftContentMessage3'>");
                contentPanels.Append("                  <asp:Literal ID='LtLeftContentMessage3' runat='server' Text='" + LabelResource.ContentPortalLeftContentMessage3 + "' />");
                contentPanels.Append("              </label>");
            }
            contentPanels.Append("              </div>");
            contentPanels.Append("          </td>");
            contentPanels.Append("      </tr>");
            contentPanels.Append("  </table>");
            contentPanels.Append("</div>");
            return contentPanels.ToString();
        }

        private static string BuildContentsToProjects(UrlHelper url, bool DefaultsDisabPublComm, LibraryCatalog oLibraryCatalog, string contentId, string type)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("      <div id='" + type + "' class='contentBoxDataList'>");
            foreach (var oProject in oLibraryCatalog.Projects)
            { 
                string contentStyleProject = "border-top:1px solid #cccccc;";
                if(oProject.Id == oLibraryCatalog.Projects[0].Id) contentStyleProject="border-top:0px; padding:0px;";
                contentPanels.Append("          <div>");
                contentPanels.Append("              <div class='contentBoxDataSubList' style='" + contentStyleProject + "'>");
                contentPanels.Append("                  <div>");
                contentPanels.Append("                      <div class='contentBoxProject'>");
                contentPanels.Append("                          <a id='ap" + oProject.Id + "' href='javascript:void(0);' onclick=\"return downloadFile('" + url.Action("Download", "ContentPortal") + "/" + oProject.Id + "');\">" + oProject.Name + "</a>");
                contentPanels.Append("                      </div>");
                contentPanels.Append("                  <div class='float-right' style='width:55px; _width:55px;'>");
                string IdComments = "ImgComents" + oProject.Id;
                /// URL TO FEEDBACK ON FORUM
                String urlFeedBackContent = url.Action("FeedBackMessage", "Forum", new { EntityId = oProject.Id, EntityType = DomainObjectType.Project, SubmittedVia = FeedBackSubmitedVia.ContentPortal });
                //// URL TO GET COMMENTS BY PROJECT
                String urlGetComments = url.Action("GetComments", "Forum", new { EntityId = oProject.Id, ObjectType = DomainObjectType.Project });
                //// URL TO EXTERNAL RESPONSE
                String urlExternalResponse = url.Action("ExternalResponse", "Forum", new { EntityId = oProject.Id, ForumResponseId = 0, ObjectType = DomainObjectType.Project });
                if (!DefaultsDisabPublComm) {
                    contentPanels.Append("                      <a id='testforumicon' href='javascript:void(0)' onclick=\"ExternalCommentsDlg('" + urlGetComments + "','" + urlExternalResponse + "','" + oProject.Id + "');\">");
                    string classCommentsToProject = "ImageCommentsN";
                    if (oProject.Comments)
                    {
                        classCommentsToProject = "ImageCommentsY";
                    }
                    contentPanels.Append("                          <img id ='"+IdComments+"' class='"+classCommentsToProject+"' src='" + url.Content("~/Content/Images/Icons/CommentWhite.png")+"' width='22px' title='Add public comment' />");
                    contentPanels.Append("                      </a>");
                }
                contentPanels.Append("                      <a id='testfeedbackicon' href='javascript: void(0);' onclick=\"ExternalFeedBackWithAttachedDlg('" + urlFeedBackContent + "','FeedBack Dialog');\">");
                contentPanels.Append("                          <img src='"+ url.Content("~/Content/Images/Icons/testfeedback.gif") +"' width='22px' title='Add private feedback' />");
                contentPanels.Append("                      </a>");
                contentPanels.Append("                  </div><br/>");
                if (oProject.File != null && oProject.File.DateIn != null)
                {
                    contentPanels.Append("                  <div id='labelDate' class='float-left'>");
                    contentPanels.Append("                      <div class='labelContent AddTargetBlankToLin' style='font-size:11px; color:#A9A9A9'>");
                    contentPanels.Append("Last Updated:" + ((DateTime)oProject.File.DateIn).ToString("MM/dd/yyyy"));
                    contentPanels.Append("                      </div>");
                    contentPanels.Append("                  </div><br/>");
                }
                contentPanels.Append("                  </div><br />");
                contentPanels.Append("                  <div id='labels' class='float-left'>");
                contentPanels.Append("                      <div class='labelContent AddTargetBlankToLin' style='font-size:11px; color:Gray'>            ");
                contentPanels.Append(oProject.TextToDisplay + "...");
                contentPanels.Append("                      </div>");
                contentPanels.Append("                  </div>");
                contentPanels.Append("              </div>");
                contentPanels.Append("              <div class='projectBoxRating'>");
                String urlRatingContent = url.Action("Rating", "ContentPortal", new { ProjectId = oProject.Id });
                if (oProject.RatingCounter > 0)
                {
                    contentPanels.Append("                  <div style='float:left; width:120px; margin-right:20px;'>");
                    contentPanels.Append("Other user Ratings");
                    contentPanels.Append("                  </div>");
                    contentPanels.Append("                  <div>");
                    contentPanels.Append("Your Rating");
                    contentPanels.Append("                  </div>");
                    ////current Rating
                    string currentRating = string.Empty;
                    currentRating = RatingStarts(oProject.Id, oProject.Rating, oProject.RatingCounter, urlRatingContent, oProject.RatingAllowed);
                    contentPanels.Append(currentRating);
                }
                else
                {
                    contentPanels.Append("              <div id='NoRatingImage' style='width:85px; height:25px; float:left; color:#e5332c; font-size:11px; width:200px;float:left;'>");
                    contentPanels.Append("This Document has not yet been rated");
                    contentPanels.Append("              </div>");
                    ///First Ratin
                    string firstRating = string.Empty;
                    firstRating = FirstRatingStarts( oProject.Id, oProject.Rating, oProject.RatingCounter, urlRatingContent, oProject.RatingAllowed);
                    contentPanels.Append(firstRating);
                }
                contentPanels.Append("              ");
                contentPanels.Append("              </div>");
                contentPanels.Append("          </div>");
                
            }
            contentPanels.Append("     </div> ");
            return contentPanels.ToString();
        }

        private static string BuildContentToNews(UrlHelper url, string idTemp, LibraryCatalog oLibraryCatalog)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("      <div id='" + idTemp + "'  class='contentBoxDataList'>");
            foreach (var oLibrary in oLibraryCatalog.Library)
            {
                String urlGetLibraryContent = url.Action("GetLibrary", "ContentPortal", new { libraryid = oLibrary.Id });
                contentPanels.Append("          <div class='contentAreaItems' style='border-bottom: 1px solid rgb(204, 204, 204);' >");
                contentPanels.Append("              <a href='javascript:void(0)' onclick=\"javascript:MessageDlgUrl('News','" + urlGetLibraryContent + "','" + oLibrary.Link + "');\" >");
                contentPanels.Append(oLibrary.Name);
                contentPanels.Append("              </a><br />");
                contentPanels.Append("          </div>");
            }
            contentPanels.Append("      </div>");
            return contentPanels.ToString();
        }

        private static string AppendBeginContentBox(LibraryCatalog oLibraryCatalog, string idTemp)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("<div class=" + oLibraryCatalog.CssClass + ">");
            contentPanels.Append(" <div style='padding-right:5px;'>");
            contentPanels.Append("  <div id='contentBoxData_" + idTemp + "' class='contentBoxData'>");
            return contentPanels.ToString();
        }
        private static string AppendBeginContentPanel(LibraryCatalog oLibraryCatalog)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("      <div id='contentPanelTitle' class='contentBoxDataHeader'> " + oLibraryCatalog.Description + "</div>");
            return contentPanels.ToString();
        }



        private static string AppendBeginContent(LibraryCatalog oLibraryCatalog, string idTemp)
        {
            return AppendBeginContent(oLibraryCatalog, idTemp, string.Empty, string.Empty, string.Empty);
        }

        private static string AppendBeginContent(LibraryCatalog oLibraryCatalog, string idBoxData, string type, string labelHeader, string postHeader)
        { 
            return AppendBeginContent( oLibraryCatalog,  idBoxData, string.Empty, type, labelHeader, postHeader);
        }
        private static string AppendBeginContent(LibraryCatalog oLibraryCatalog, string idBoxData, string idBoxHeader, string type, string labelHeader, string postHeader)
        {
            return AppendBeginContent(oLibraryCatalog, idBoxData, string.Empty, type, labelHeader, postHeader,null, null, string.Empty);
        }

        private static string AppendBeginContent(LibraryCatalog oLibraryCatalog, string idBoxData, string idBoxHeader, string type, string labelHeader, string postHeader, UserProfile user, Positioning positioning, string fieldPosition)
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append(AppendBeginContentBox(oLibraryCatalog, idBoxData));
            if (string.IsNullOrEmpty(idBoxHeader)) idBoxHeader = "contentPanelTitle";
            contentPanels.Append("      <div id='" + idBoxHeader + "' class='contentBoxDataHeader'> ");
            if (string.IsNullOrEmpty(labelHeader))
            {
                contentPanels.Append(oLibraryCatalog.Description);
            }
            else
            {
                string lblHeader = "Detailsof";
                string preHeader = string.Empty;
                if (type.Equals("Positioning")) { lblHeader = "PositioningTitle"; preHeader = LabelResource.Positioning+": "; }
                else if (type.Equals("CompetitiveMessaging")) { lblHeader = "CompetitiveMessagingTitle"; preHeader = LabelResource.CompetitiveMessaging +": "; }
                else { }
                contentPanels.Append("<label for='Lbl" + lblHeader + "'>");
                contentPanels.Append(preHeader + " <asp:Literal ID='Lt" + lblHeader + "' runat='server' Text='" + labelHeader + "' > " + labelHeader + " </asp:Literal>");
                contentPanels.Append("</label>");
                if(!string.IsNullOrEmpty(postHeader))
                    contentPanels.Append(postHeader);
                if (type.Equals("Positioning") || type.Equals("CompetitiveMessaging")) 
                {
                    if (user != null && positioning!= null)
                    {
                        if (user.SecurityGroupId != "ENDUSER")
                        {
                            contentPanels.Append("<img id='Img" + fieldPosition + "_" + positioning.Id + "_" + positioning.EntityType + "_" + positioning.PositioningRelation + "_" + positioning.IsGlobal + "' alt='' class='icons-edit-positioning pos-edt-dlg' title='Edit Statement' src='/Content/Images/Icons/properties.png' />");
                        }
                    }
                }
            }
            contentPanels.Append("      </div>");
            return  contentPanels.ToString();
        }

        private static string AppendEndContent()
        {
            StringBuilder contentPanels = new StringBuilder();
            contentPanels.Append("  </div>");
            contentPanels.Append(" </div>");
            contentPanels.Append("</div>");
            return contentPanels.ToString();
        }

        public static string Porlet(this HtmlHelper helper, LibraryCatalog oLibraryCatalog, int i, bool DefaultsDisabPublComm, string DetailsEntities, string EntityDetail, string NameDetail, string ImageDetail, string UrlDetail, string UrlDetailText, string DescriptionDetail, IList<StrengthWeakness> strengthbyindustry, IList<StrengthWeakness> weaknessbyindustry, string SWCompetitorId, string SWIndustryId, bool CompetitorHasComment, Positioning Positioning, Positioning competitiveMessaging, UserProfile user)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder contentPanels = new StringBuilder();

            string idTemp = oLibraryCatalog.Description.Replace(" ", "_").Replace("/", "_");
            if (oLibraryCatalog.Displayable && (oLibraryCatalog.Projects.Count != 0 || oLibraryCatalog.Positioning.Count != 0 || oLibraryCatalog.Library.Count != 0))
            {
                if (oLibraryCatalog.Projects.Count > 0)
                {
                    contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp));
                    contentPanels.Append(BuildContentsToProjects(url, DefaultsDisabPublComm, oLibraryCatalog, "contentPanel" + i.ToString(), "Projects"));
                    contentPanels.Append(AppendEndContent());
                }
                //else if (oLibraryCatalog.Positioning.Count > 0)
                //{ }
                /// NEWS CONTENT
                else if (oLibraryCatalog.Library.Count > 0)
                {
                    contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp));
                    contentPanels.Append(BuildContentToNews(url, idTemp, oLibraryCatalog));
                    contentPanels.Append(AppendEndContent());
                }
            }
            else
            {
                if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Details Industry Competitor Product"))
                {
                    if (string.IsNullOrEmpty(DetailsEntities) && !string.IsNullOrEmpty(EntityDetail))
                    {
                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp, DomainObjectType.Library, Resources.LabelResource.PortalContentDetailsof, EntityDetail + ": " + NameDetail));
                        contentPanels.Append(BuildContentToEntityDetails(idTemp, oLibraryCatalog, ImageDetail, UrlDetail, UrlDetailText, DescriptionDetail));
                        contentPanels.Append(AppendEndContent());
                    }
                }
                else if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Strengths/Weaknesses"))
                {
                    if (strengthbyindustry != null & weaknessbyindustry != null)
                    {
                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp));
                        contentPanels.Append(BuildContentToSW(url, SWCompetitorId, SWIndustryId, CompetitorHasComment, DefaultsDisabPublComm, strengthbyindustry, weaknessbyindustry));
                        contentPanels.Append(AppendEndContent());
                    }
                }
                else if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Positioning Statements"))
                {
                    if (Positioning != null)
                    {
                        if (!string.IsNullOrEmpty(Positioning.HowWePosition))
                        {
                            ///TO HOW WE 
                            contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp + "HWP", "DivPositioningHowWePositionHeader", "Positioning", LabelResource.PositioningHowWePosition, string.Empty, user, Positioning, "Whp"));
                            contentPanels.Append("<div id='Whp_" + Positioning.Id + "'  class='contentBoxDataList AddTargetBlankToLin'>" + Positioning.HowWePosition + "</div>");
                            contentPanels.Append(AppendEndContent());
                        }
                        else if (!string.IsNullOrEmpty(Positioning.HowTheyPosition) || !string.IsNullOrEmpty(Positioning.HowWeAttack))
                        {
                            contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp + "HTP", "DivPositionHowTheyPositionHearder", "Positioning", LabelResource.PositioningHowTheyPosition, string.Empty, user, Positioning, "Htp"));
                            contentPanels.Append("<div id='Htp_" + Positioning.Id + "'  class='contentBoxDataList AddTargetBlankToLin'>" + Positioning.HowTheyPosition + "</div>");
                            contentPanels.Append(AppendEndContent());

                            contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp + "HWA", "DivPositionHowWeAttackHeader", "Positioning", LabelResource.PositioningHowWeAttack, string.Empty, user, Positioning, "Hwa"));
                            contentPanels.Append("<div id='Hwa_" + Positioning.Id + "'  class='contentBoxDataList AddTargetBlankToLin'>" + Positioning.HowWeAttack + "</div>");
                            contentPanels.Append(AppendEndContent());
                        }
                    }

                }
                else if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Competitive Messaging"))
                {
                    if (competitiveMessaging != null && competitiveMessaging.Id != 0)
                    {
                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp + "HTA", "DivPositionHowTheyAttackHearder", "CompetitiveMessaging", LabelResource.PositioningHowTheyAttack, string.Empty, user, competitiveMessaging, "Hta"));
                        contentPanels.Append("<div id='Hta_" + competitiveMessaging.Id + "'  class='contentBoxDataList AddTargetBlankToLin'>" + competitiveMessaging.HowTheyAttack + "</div>");
                        contentPanels.Append(AppendEndContent());

                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp + "HWD", "DivPositionHowWeDefendHeader", "CompetitiveMessaging", LabelResource.PositioningHowWeDefend, string.Empty, user, competitiveMessaging, "Hwd"));
                        contentPanels.Append("<div id='Hwd_" + competitiveMessaging.Id + "'  class='contentBoxDataList AddTargetBlankToLin'>" + competitiveMessaging.HowWeDefend + "</div>");
                        contentPanels.Append(AppendEndContent());
                    }
                }
                else if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Competitors in Industry"))
                {
                    if (oLibraryCatalog.Competitors.Count > 0)
                    {
                        //contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp));
                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp, "", "", oLibraryCatalog.Description + " : " + oLibraryCatalog.Industry.Name, "", null, null, ""));
                        contentPanels.Append(BuildContentToCompetitors(url, idTemp, oLibraryCatalog));
                        contentPanels.Append(AppendEndContent());
                    }
                }
                else if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Products in Industry"))
                {
                    if (oLibraryCatalog.Products.Count > 0 || oLibraryCatalog.ProductsWithCompetitor.Count > 0)
                    {
                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp, "", "", oLibraryCatalog.Description + " : " + oLibraryCatalog.Industry.Name, "", null, null, ""));
                        contentPanels.Append(BuildContentToProducts(url, idTemp, oLibraryCatalog));
                        contentPanels.Append(AppendEndContent());
                    }
                }
                else if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Industries"))
                {
                    if (oLibraryCatalog.Industries.Count > 0)
                    {
                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp, "", "", oLibraryCatalog.Description + " : " + oLibraryCatalog.Competitor.Name, "", null, null, ""));
                        contentPanels.Append(BuildContentToIndustries(url, idTemp, oLibraryCatalog));
                        contentPanels.Append(AppendEndContent());
                    }
                }
                else if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Financial Information"))
                {
                    if (oLibraryCatalog.CompetitorFinancialIncomeStatement.Count > 0)
                    {
                        contentPanels.Append(AppendBeginContent(oLibraryCatalog, idTemp, "", "", oLibraryCatalog.Description, "", null, null, ""));
                        contentPanels.Append("      <div id='" + idTemp + "'  class='contentBoxDataList'>");
                        //contentPanels.Append("Competitor:");
                        contentPanels.Append("<div style='float:right;'>");
                        contentPanels.Append("<input type='checkbox' id='chkAnnual' onclick='showAnnual(this);' checked /> <label> Annual</label> ");
                        contentPanels.Append("<input type='checkbox' id='chkQuarterly' onclick='showQuartely(this);' /> <label> Quarterly </label>");
                        //contentPanels.Append("<br/>");
                        //contentPanels.Append("<label>Competitor: " + oLibraryCatalog.Competitor.Name + "</label>");
                        contentPanels.Append("</div>");
                        contentPanels.Append(BuildTableToFinancial(oLibraryCatalog.CompetitorFinancialIncomeStatement));
                        //if (oLibraryCatalog.ClientFinancialIncomeStatement.Count > 0)
                        //{
                        //    contentPanels.Append("<br/>");
                        //    contentPanels.Append("<label>Competitor: " + oLibraryCatalog.ClientName + "</label>");
                        //    contentPanels.Append(BuildTableToFinancial(oLibraryCatalog.ClientFinancialIncomeStatement));
                        //}
                        contentPanels.Append("</div>");
                        contentPanels.Append(AppendEndContent());
                        contentPanels.Append("<br/>");
                    }
                }
                else { }
            }
            return contentPanels.ToString();
        }
        
        public static string RatingStarts(decimal? Id, decimal? Rating, decimal? RatingCounter, string UrlUpdate, bool RatingAllowed)
        {
            double RatingWidth = Math.Truncate((double)Rating * 84 / 100);
            string EventControl = RatingAllowed ? " onmousedown=\"mouseUpdate(event,this,'" + RatingCounter + "','" + UrlUpdate + "');\"  onmousemove='mouseMove(event,this)' onmouseout='mouseOut(event,this)'" : " onmousemove='toggle_visibility(this,true)' onmouseout='toggle_visibility(this,false)' ";
            string ClassControl = RatingAllowed ? "" : "tooltip";
            string SpanControl = RatingAllowed ? "" : "<span>Rating already established.</span>";

            string _Result =
            "<div id='Ra" + Id + "' class='ratingBox'>" +
            "  <div class='rating-average' style='float:left;'>" +
            "    <div class='rating-foreground' style='width:" + RatingWidth + "px;'> &nbsp; </div>" +
            "  </div>      " +
            "  <div class='rating-percent' > " + Rating + "%  </div>" +
            "</div> " +
            "<div id='Rc" + Id + "' class='ratingBox' style='background-color:transparent'>" +
            "  <div id='" + Id + "' class='" + ClassControl + " rating' " + EventControl + " >" + SpanControl +
            "    <div class='rating-foreground' style='width: 0px;'></div>" +
            "  </div>      " +
            "  <div class='rating-percent' > 0% </div>" +
            "</div> "
                      ;
            return _Result;
        }

        public static string FirstRatingStarts(decimal? Id, decimal? Rating, decimal? RatingCounter, string UrlUpdate, bool RatingAllowed)
        {
            double RatingWidth = Math.Truncate((double)Rating * 84 / 100);
            string EventControl = RatingAllowed ? " onmousedown=\"mouseUpdate(event,this,'" + RatingCounter + "','" + UrlUpdate + "');\"  onmousemove='mouseMove(event,this)' onmouseout='mouseOut(event,this)'" : string.Empty;
            string _Result =
            "<div id='Ra" + Id + "' class='ratingBox' style='display:none;'>" +
            "  <div class='rating-average' style='float:left;'>" +
            "    <div class='rating-foreground' style='width:" + RatingWidth + "px;'> &nbsp; </div>" +
            "  </div>      " +
            "  <div class='rating-percent' > " + Rating + "%  </div>" +
            "</div> " +
            "<div id='Rc" + Id + "' class='ratingBox' style='background-color:transparent'>" +
            "  <div id='" + Id + "' class='rating' " + EventControl + " >" +
            "    <div class='rating-foreground' style='width: 0px;'></div>" +
            "  </div>      " +
            "  <div class='rating-percent' > 0% </div>" +
            "</div> ";
            return _Result;
        }
    }
}
