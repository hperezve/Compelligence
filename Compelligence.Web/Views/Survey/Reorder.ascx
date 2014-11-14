<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% using (Ajax.BeginForm("SaveReorder", "Survey",
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SurveyReorderContent",
               OnBegin = "showLoadingDialog",
               OnComplete = "function() { hideLoadingDialog();}",
               OnSuccess = "function(){}",
               OnFailure = "showFailedResponseDialog"
           }, new { id = (string)ViewData["Scope"] + "SurveyReorderForm" }
           ))
   { %>
<div id="contentSurveyItems" class="indexTwo">
   <%--<fieldset>
   <legend>Reorder</legend>--%>
   <div class="reorder">
        <div class="reordTitle"><b> Reorder </b></div>
        
        <div class="buttonLink">
            <input class="button" id="Submit2" type="submit" value="Save" />
            <input class="button" type="reset" value="Cancel" " />
        </div>
        <input type="hidden" id="QuizId" name="QuizId" value='<%=ViewData["QuizId"] %>' />
        <div class="sortlist">
            <% IList<Question> Questions = (IList<Question>)ViewData["Questions"];
           foreach (Question Question in Questions)
           {%>
        <div class="rowQuestion" id="row_<%=Question.Id %>">
            <img src="<%= Url.Content("~/Content/Images/Styles/scroller-td.gif") %>" class="icono float-left"
                alt="move" />
            <div style="padding: 4px; height: 30px; overflow: auto">
                <input type="hidden" id="txtQuestionId" name="txtQuestionId" value='<%=Question.Id %>' />
                <%=Question.Item %>.
                <label style="font-size: 13px">
                    <%=Question.QuestionText%></label>
            </div>
        </div>
        <%} %>
        </div>
        </div>
  <%-- </fieldset>--%>
</div>
<%} %>

<script type="text/javascript">
    $(document).ready(function() {
        function ActiveSortable(collectionId, typeCollection, targetviewer) {
            $(collectionId).sortable({
                items: typeCollection,
                cursor: "pointer",
                opacity: 0.7,
                handle: "img.icono",
                update: function() { }
            });
        }
        ActiveSortable("div.sortlist", "> div", "input#sortlist");
        $("#accordion").accordion();
    });
</script>

