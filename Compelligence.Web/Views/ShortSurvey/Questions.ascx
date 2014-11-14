<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="ShortSurveyContent">
    <% using (Ajax.BeginForm("Answer", "ShortSurvey", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = "ShortSurveyContent"
           },
           new { id = "ShortSurveyForm" }))
       { %>
    <% Quiz shortSurvey = (Quiz)ViewData["ShortSurvey"];
       if (shortSurvey != null)
       {
    %>
    <h3>
        <%= shortSurvey.Title%></h3>
    <hr />
    <input type="hidden" id="QuizId" name="Id" value='<%= shortSurvey.Id %>' />
    <% string errorMessage = (string)ViewData["ErrorMessage"]; %>
    <% if (!string.IsNullOrEmpty(errorMessage))
       {%>
    <h3 style="color: red">
        <%= errorMessage %></h3>
    <% } %>
    <table>
        <%    foreach (Question question in shortSurvey.Questions)
              {%>
        <tr>
            <td>
                <%=Html.QuestionControl(shortSurvey, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"])%>
            </td>
        </tr>
        <%}%>
    </table>
    <br />
    <div>
        <input class="shortButton" type="submit" value="Submit" />
    </div>
    <%}
       }%>
</div>
