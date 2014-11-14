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

using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using System.Collections.Generic;
using Compelligence.DataTransfer.FrontEnd;
using System.Text;
using System.Web.Mvc;

namespace Compelligence.Web.Models.Helpers
{
    public static class QuestionHelper
    {
        public static string QuestionControl(this HtmlHelper helper, Quiz quiz, Question pQuestion, object answerObject, IEnumerable<SelectListItem> industryIdList, IEnumerable<SelectListItem> allCompetitorIdList, IEnumerable<SelectListItem> ProductIdList, String WinningItem, String ProductLabel, String CompetitorLabel, String IndustryLabel)
        {
            return QuestionControl(helper, quiz, pQuestion, answerObject, industryIdList, allCompetitorIdList, ProductIdList, WinningItem, ProductLabel, CompetitorLabel, IndustryLabel, null, null);
        }

        public static string QuestionControl(this HtmlHelper helper, Quiz quiz, Question pQuestion, object answerObject, IEnumerable<SelectListItem> industryIdList, IEnumerable<SelectListItem> allCompetitorIdList, IEnumerable<SelectListItem> ProductIdList, String WinningItem, String ProductLabel, String CompetitorLabel, String IndustryLabel, IEnumerable<SelectListItem> primaryIndustryList, IEnumerable<SelectListItem> primaryCompetitorList)
        {
            StringBuilder Script = new StringBuilder();
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);
            String urlChangedIndList = url.Action("ChangeEnableIndustryList", "DealSupport");
            String urlCompOfInd = url.Action("GetCompetitorsOfIndustry", "DealSupport");
            String urlCompOfProd = url.Action("GetProductsOfCompetitor", "DealSupport");
            IEnumerable<SelectListItem> competitorIdList = new MultiSelectList(new List<Competitor>(), "Id", "Name");


            //Quiz quiz = (Quiz) helper.ViewData["quiz"];
            //string _Answer = (string)helper.ViewData["Q" + pQuestion.Item];
            string answer = (answerObject != null) ? answerObject.ToString() : string.Empty;
            string Checked = "";

            Script.Append(" <div class=\"QuestionHead\" >   ");
            Script.Append("     <b>" + pQuestion.Item + ".&nbsp;" + pQuestion.QuestionText + "</b>");
            Script.Append(" </div> ");
            Script.Append(" <div class=\"QuestionResponse\" >   ");
            if (pQuestion.Type.Equals(QuestionType.WinningCompetitor))
            {
                Script.Append(" <div class=\"QuestionPart\"> ");
                Script.Append("     <label for=\"fWinningCompetitor\"> ");
                Script.Append("     <asp:Literal ID=\"kWinningCompetitor\" runat=\"server\" Text=\"WinningCompetitor\" />Winning " + CompetitorLabel + ":</label><br/> ");
                Script.Append(" <input id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' value='" + answer + "' style = \"width:247px;\" onclick = \"javascript: EditWinning('" + pQuestion.Item + "');\" onkeyup = \"javascript: updateList('" + pQuestion.Item + "');\"><br/> ");
                Script.Append(HtmlSelectHelper.SelectList(helper, "WinningCompetitor" + pQuestion.Item.ToString(), false, "10", allCompetitorIdList, " style = \"width:250px;height:200px;display:none;\" onchange = \"javascript: updateWinning('" + pQuestion.Item + "');\" "));
                Script.Append(" </div> ");

                Script.Append(" <script type=\"text/javascript\"> ");
                Script.Append(" $(\"#" + "WinningCompetitor" + pQuestion.Item.ToString() + "\").hide();  ");
                Script.Append(" </script> ");

            }
            else if (pQuestion.Type.Equals(QuestionType.Competitors))
            {
               // IList<Industry> primaryIndustryList = new List<Industry>();
                int industryListSize = 0;
                if (industryIdList != null)
                {
                    industryListSize = industryIdList.Count();
                    //if(industryIdList.
                }
                int competitorListSize = 0;
                if (allCompetitorIdList != null)
                {
                    competitorListSize = allCompetitorIdList.Count();
                    competitorIdList = allCompetitorIdList;
                }
                int productListSize = 0;
                if (ProductIdList != null)
                {
                    productListSize = ProductIdList.Count();
                    //competitorIdList = allCompetitorIdList;
                }
                Script.Append(" <div class=\"QuestionComp\" > ");
                Script.Append(" <div class=\"QuestionPart\" > ");
                Script.Append("     <label for=\"IndustryId\" style=\"float:left;\"> ");
                Script.Append("     <asp:Literal ID=\"KitIndustryId\" runat=\"server\" Text=\"IndustryId\" />" + IndustryLabel + ":</label> ");
                Script.Append("     <input type=\"CheckBox\" id=\"CheckIndustryIds" + pQuestion.Item.ToString() + "\" onclick = \"ShowIndustriesByHierarchy(this,'" + urlChangedIndList + "','" + pQuestion.Item + "');\" Style = \"float:left;margin-left:5px;margin-top:1px;height:10px;\" /><label for=\"CheckIndustryIds\" >&nbsp;By Hierarchy</label> ");
                Script.Append("     <div id=\"ContentIndustry" + pQuestion.Item.ToString() + "\" class='contentscrollableQ'> ");
                Script.Append(HtmlSelectHelper.SelectList(helper, "QuestionIndustriesIds" + pQuestion.Item.ToString(), true, "10", industryIdList, "  style = \"height:" + ((industryListSize * 14) + 10) + "px\" onload=\"javascript: LoadQuestion('" + pQuestion.Item + "'," + industryListSize + ");\" onchange = \"javascript: updateCompAndProd('" + urlCompOfInd + "','" + urlCompOfProd + "','" + pQuestion.Item + "');updateAnswer('" + pQuestion.Item + "');\" "));
                Script.Append(" </div></div> ");
                Script.Append(" <div class=\"QuestionPart\" > ");
                Script.Append("     <label for=\"CompetitorId\"> ");
                Script.Append("     <asp:Literal ID=\"KitCompetitorId\" runat=\"server\" Text=\"CompetitorId\"/>" + CompetitorLabel + ":</label> ");
                Script.Append("     <div id=\"ContentCompetitor" + pQuestion.Item.ToString() + "\" class='contentscrollableQ'> ");
                Script.Append(HtmlSelectHelper.SelectList(helper, "QuestionCompetitorsIds" + pQuestion.Item.ToString(), true, "10", competitorIdList, "  style = \"height:" + ((competitorListSize * 14) + 10) + "px\"  onchange = \"javascript: updateProd('" + urlCompOfProd + "','" + pQuestion.Item + "','" + WinningItem + "');updateAnswer('" + pQuestion.Item + "');\" "));
                Script.Append(" </div></div> ");
                Script.Append(" <div class=\"QuestionPart\" > ");
                Script.Append("     <label for=\"ProductId\"> ");
                Script.Append("     <asp:Literal ID=\"KitProductId\" runat=\"server\" Text=\"ProductId\"/>" + ProductLabel + ":</label> ");
                Script.Append("     <div id=\"ContentProduct" + pQuestion.Item.ToString() + "\" class='contentscrollableQ'> ");
                Script.Append(HtmlSelectHelper.SelectList(helper, "QuestionProductsIds" + pQuestion.Item.ToString(), true, "10", ProductIdList, " style = \"height:" + ( (productListSize * 14) + 10) + "px\"  onchange = \"javascript: updateAnswer('" + pQuestion.Item + "');\" "));
                Script.Append(" </div></div></div> ");
                Script.Append(" <div class=\"primary\" > ");
                Script.Append(" <div class=\"QuestionPart\" > ");
                Script.Append("     <label for=\"fPrimaryIndustry\"> ");
                Script.Append("     <asp:Literal ID=\"kPrimaryIndustry\" runat=\"server\" Text=\"PrimaryIndustry\" /> Primary " + IndustryLabel + ":</label><br/> ");
                if(primaryIndustryList != null && primaryIndustryList.Count()>0)
                {
                    Script.Append(HtmlSelectHelper.SelectList(helper, "PrimaryIndustry" + pQuestion.Item.ToString(), false, "", primaryIndustryList, " style = \"width:247px\" onchange = \"javascript: updateAnswer('" + pQuestion.Item + "');\" "));    
                }
                else
                {
                Script.Append(HtmlSelectHelper.SelectList(helper, "PrimaryIndustry" + pQuestion.Item.ToString(), false, "", null, " style = \"width:247px\" onchange = \"javascript: updateAnswer('" + pQuestion.Item + "');\" "));
                }
                Script.Append(" </div> ");
                Script.Append(" <div class=\"QuestionPart\" > ");
                Script.Append("     <label for=\"fPrimaryCompetitor\"> ");
                Script.Append("     <asp:Literal ID=\"kPrimaryCompetitor\" runat=\"server\" Text=\"PrimaryCompetitor\" />Primary " + CompetitorLabel + ":</label><br/> ");
                if (primaryCompetitorList != null && primaryCompetitorList.Count() > 0)
                {
                    Script.Append(HtmlSelectHelper.SelectList(helper, "PrimaryCompetitor" + pQuestion.Item.ToString(), false, "", primaryCompetitorList, " style = \"width:247px\" onchange = \"javascript: updateAnswer('" + pQuestion.Item + "');\" "));
                }
                else
                {
                    Script.Append(HtmlSelectHelper.SelectList(helper, "PrimaryCompetitor" + pQuestion.Item.ToString(), false, "", null, " style = \"width:247px\" onchange = \"javascript: updateAnswer('" + pQuestion.Item + "');\" "));
                }
                
                Script.Append(" </div></div> ");
                Script.Append(" <script type=\"text/javascript\"> ");
                Script.Append("     LoadQuestion('" + pQuestion.Item + "'," + industryListSize + "); ");
                Script.Append(" </script> ");

                Script.Append(" <input type=\"hidden\" id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' value='" + answer + "'> ");
            }
            else if (pQuestion.Type.Equals(QuestionType.OpenText))
            {
                Script.Append(" <textarea id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' value='" + answer + "'  style='resize:both;height:50px;");
                //Script.Append(" <input type='text' id ='Q" + pQuestion.Item + "' name ='Q" + pQuestion.Item + "'  ");
                if (string.Compare(quiz.Type, "Short") != 0)
                {
                    Script.Append(" width:550px;");
                }
                else { Script.Append(" width:35%;"); }
                Script.Append("'>"+answer+"</textarea>");
                //Script.Append("'></input>");

            }
            else if (pQuestion.Type.Equals(QuestionType.YesorNot))
            {

                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        if (Detail.ResponseValue.Equals(answer))
                        {
                            Checked = "checked";
                        }
                        else
                        {
                            Checked = "";
                        }
                        Script.Append(" <input type='radio' id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' " + Checked + " style='vertical-align: middle;'><label style='vertical-align: middle;display: inline;'> " + Detail.ResponseText + "</label><br />");
                    }
                }

            }
            else if (pQuestion.Type.Equals(QuestionType.MultipleChoice))
            {

                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        if (Detail.ResponseValue.Equals(answer))
                        {
                            Checked = "checked";
                        }
                        else
                        {
                            Checked = "";
                        }
                        Script.Append(" <input type='radio' id='Q" + pQuestion.Item + "' Name='Q" + pQuestion.Item + "' value='" + Detail.ResponseValue + "' " + Checked + " style='vertical-align: middle;'><label style='vertical-align: middle;display: inline;'> " + Detail.ResponseText + "</label><br />");
                    }
                }

            }

            Script.Append(" </div>");

            return Script.ToString();
        }

        public static string QuestionControl(this HtmlHelper helper, Quiz quiz, Question pQuestion, object answerObject)
        {
            string _Result = string.Empty;

            //Quiz quiz = (Quiz) helper.ViewData["quiz"];
            //string _Answer = (string)helper.ViewData["Q" + pQuestion.Item];
            string answer = (answerObject != null) ? answerObject.ToString() : string.Empty;
            string _checked = "";
            _Result += "<div>";

            _Result += "<br /> <b>" + pQuestion.Item + ".&nbsp;" + pQuestion.QuestionText + "</b> <br />";

            if (pQuestion.Type.Equals(QuestionType.OpenText))
            {
                _Result += "<textarea id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' value='" + answer + "'  style='resize:both;height:50px;";

                if (string.Compare(quiz.Type, "Short") != 0)
                {
                    _Result += "width:550px";
                }

                _Result += "'></textarea>";
            }
            else if (pQuestion.Type.Equals(QuestionType.YesorNot))
            {
                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        if (Detail.ResponseValue.Equals(answer))
                        {
                            _checked = "checked";
                        }
                        else
                        {
                            _checked = "";
                        }
                        _Result += "<input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' " + _checked + "> " + Detail.ResponseText + "<br />";
                    }
                }
            }
            else if (pQuestion.Type.Equals(QuestionType.MultipleChoice))
            {
                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        if (Detail.ResponseValue.Equals(answer))
                        {
                            _checked = "checked";
                        }
                        else
                        {
                            _checked = "";
                        }
                        _Result += "<input type='radio' id='Q" + pQuestion.Item + "'Name='Q" + pQuestion.Item + "' value='" + Detail.ResponseValue + "' " + _checked + "> " + Detail.ResponseText + "<br />";
                    }
                }

            }

            _Result += "</div>";
            return _Result;
        }

        public static string QuestionControl(this HtmlHelper helper, Question pQuestion, string pQuizType)
        {
            string _Result = string.Empty;
            _Result += "<div>";

            _Result += "<br /> " + pQuestion.Item + ".<label>" + pQuestion.QuestionText + "</Label> <br />";

            if (pQuestion.Type.Equals(QuestionType.OpenText))
            {
                if (pQuizType.Equals(QuizType.Nugget))
                    _Result += "<textarea id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' style='width:550px'></textarea>";
                else
                    _Result += "<input id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' type=text style='width:550px'>";
            }
            else if (pQuestion.Type.Equals(QuestionType.YesorNot))
            {
                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        _Result += "<input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "'> " + Detail.ResponseText + "<br />";
                    }
                }
            }
            else if (pQuestion.Type.Equals(QuestionType.MultipleChoice))
            {
                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        _Result += "<input type='radio' id='Q" + pQuestion.Item + "'Name='Q" + pQuestion.Item + "' value='" + Detail.ResponseValue + "'> " + Detail.ResponseText + "<br />";
                    }
                }

            }

            _Result += "</div>";
            return _Result;
        }
        public static string QuestionControl(this HtmlHelper helper, Question pQuestion, Answer pAnswer)
        {
            string _Result = string.Empty;
            _Result += "<div>";

            _Result += "<br /> " + pQuestion.Item + ".<label>" + pQuestion.QuestionText + "</Label> <br />";

            if (pQuestion.Type.Equals(QuestionType.WinningCompetitor))
            {
                _Result += "<input id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' type=text style='width:550px;border:0px;background-color:#FFFFFF;' value='" + pAnswer.AnswerText + "' disabled='disabled'>";
            }
            else if (pQuestion.Type.Equals(QuestionType.Competitors))
            {
                String[] Answer = pAnswer.AnswerText.Split(';');
                if (Answer.Length >= 5)
                {
                    _Result += "Industries:</br>";
                    _Result += "<textarea rows=5 cols=600 id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' type=text style='width:550px;border:0px;background-color:#FFFFFF;' value='" + Answer[0] + "' disabled='disabled'>" + Answer[0] + "</textarea></br>";
                    _Result += "Competitors:</br>";
                    _Result += "<textarea rows=5 cols=600 id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' type=text style='width:550px;border:0px;background-color:#FFFFFF;' value='" + Answer[1] + "' disabled='disabled'>" + Answer[1] + "</textarea></br>";
                    _Result += "Product:</br>";
                    _Result += "<textarea rows=5 cols=600 id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' type=text style='width:550px;border:0px;background-color:#FFFFFF;' value='" + Answer[2] + "' disabled='disabled'>" + Answer[2] + "</textarea></br>";
                    _Result += "Primary Industry:</br>";
                    _Result += "<input id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' type=text style='width:550px;border:0px;background-color:#FFFFFF;' value='" + Answer[3] + "' disabled='disabled'></br>";
                    _Result += "Primary Competitor:</br>";
                    _Result += "<input id='Q" + pQuestion.Item + "' name='Q" + pQuestion.Item + "' type=text style='width:550px;border:0px;background-color:#FFFFFF;' value='" + Answer[4] + "' disabled='disabled'></br>";
                }
            }
            else if (pQuestion.Type.Equals(QuestionType.OpenText))
            {
                _Result += "<textarea id='Q'" + pQuestion.Item + "name='Q" + pQuestion.Item + "' type=text style='width:550px;border:0px;background-color:#FFFFFF;' disabled='disabled'>" + pAnswer.AnswerText.Replace("'", "&#39;") + "</textarea>";
            }
            else if (pQuestion.Type.Equals(QuestionType.YesorNot))
            {
                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        //if (Detail.ResponseValue.Equals(pAnswer.AnswerText))
                        //{
                        //    _Result += "<div class='radioinput' style='background-color:#F5F5F5;'><input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' checked='checked' disabled='disabled'> " + Detail.ResponseText + "</div>";
                        //}
                        //else
                        //{
                        //    _Result += "<div class='radioinput' style='background-color:#F5F5F5;'><input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' disabled='disabled'> " + Detail.ResponseText + "</div>";
                        //}
                        if (Detail.ResponseValue.Equals(pAnswer.AnswerText))
                        {
                            _Result += "<input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' checked='checked' disabled='disabled'><label> " + Detail.ResponseText + "</label><br />";
                        }
                        else
                        {
                            _Result += "<input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' disabled='disabled'><label> " + Detail.ResponseText + "</label><br />";
                        }
                    }
                }
            }
            else if (pQuestion.Type.Equals(QuestionType.MultipleChoice))
            {
                foreach (QuestionDetail Detail in pQuestion.QuestionDetails)
                {
                    if (Detail.ResponseValue == "")
                    { }
                    else
                    {
                        //if (Detail.ResponseValue.Equals(pAnswer.AnswerText))
                        //{
                        //    _Result += "<div class='radioinput' style='background-color:#F5F5F5;'><input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' checked='checked' disabled='disabled' > " + Detail.ResponseText + "</div>";
                        //}
                        //else
                        //{
                        //    _Result += "<div class='radioinput' style='background-color:#F5F5F5;'><input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' disabled='disabled'> " + Detail.ResponseText + "</div>";
                        //}
                        if (Detail.ResponseValue.Equals(pAnswer.AnswerText))
                        {
                            _Result += "<input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' checked='checked' disabled='disabled'><label> " + Detail.ResponseText + "</label><br />";
                        }
                        else
                        {
                            _Result += "<input type='radio' id='Q" + pQuestion.Item + "'name='Q" + pQuestion.Item + "'  value='" + Detail.ResponseValue + "' disabled='disabled'><label> " + Detail.ResponseText + "</label><br />";
                        }
                    }

                }

            }

            _Result += "</div>";
            return _Result;
        }

    }
}

