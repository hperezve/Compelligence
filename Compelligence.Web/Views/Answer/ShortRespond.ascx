<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    function EditWinning(question) {
        $('#WinningCompetitor' + question).show();
    };
    function updateWinning(question) {
        $('#Q' + question).val($('#WinningCompetitor' + question + ' :selected').text());
        $('#WinningCompetitor' + question).hide();
    };

    function updateList(question) {
        list = '#WinningCompetitor' + question;
        var filter = eval("/" + $('#Q' + question).val() + "/i");
        $('#WinningCompetitor' + question + ' option').each(function(index, value) {
            if ($(value).text().search(filter) < 0) {
                $(value).hide();
            } else {
                $(value).show();
            }
        });

    };
</script>
<div id="ShortSurveyContent">
    <% using (Ajax.BeginForm("ShortRespond", "Answer", null,
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
                <%--<%=Html.QuestionControl(shortSurvey, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"])%>--%>
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
        <%}%>
    </table>
    <br />
    <div>
        <input class="shortButton" type="submit" value="Submit" />
    </div>
    <%}
       }%>
</div>
