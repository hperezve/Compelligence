<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% Quiz shortSurvey = (Quiz)ViewData["ShortSurvey"];
   if (shortSurvey != null)
   { %>
<div class="rightTitle";">
    Short Survey<br />
    <br />
</div>
<div class="rightQuestionsHome">
    <div id="ShortSurveyContent" class="shadow">
        <% using (Ajax.BeginForm("ShortRespond", "Answer", null,
               new AjaxOptions
               {
                   HttpMethod = "POST",
                   UpdateTargetId = "ShortSurveyContent"
               },
               new { id = "ShortSurveyForm", style = " margin-left: 15px;" }))
           {   
        %>
        <h3>
            <%= shortSurvey.Title%></h3>
        <hr />
        <input type="hidden" id="QuizId" name="Id" value='<%= shortSurvey.Id %>' />
        <% string errorMessage = (string)ViewData["ErrorMessage"]; %>
        <% if (!string.IsNullOrEmpty(errorMessage))
           { %>
        <h3 style="color: red">
            <%= errorMessage %></h3>
        <% } %>
        <table>
            <% foreach (Question question in shortSurvey.Questions)
               { %>
            <tr>
                <td>
                    <% if (ViewData["PrimaryIndustryList"] != null || ViewData["PrimaryCompetitorList"] != null)
                       { %>
                    <%=Html.QuestionControl(shortSurvey, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"], (SelectList)ViewData["PrimaryIndustryList"], (SelectList)ViewData["PrimaryCompetitorList"])%>
                    <% }
                       else
                       { %>
                    <%=Html.QuestionControl(shortSurvey, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"])%>
                    <% } %>
                </td>
            </tr>
            <% } %>
            <tr>
                <td>
                    <input class="shortButton" type="submit" value="Submit" />
                </td>
            </tr>
        </table>
        <br />
        <% } %>
    </div>
</div>
<% } %>
