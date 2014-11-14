<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% IList<Quiz> surveyCollection = (IList<Quiz>)ViewData["LongSurvey"];
    if(surveyCollection != null && surveyCollection.Count>0) { %>
<div class="rightTitle">Survey</div>
<div id="SurveyContent" class="shadow">

    <ul>
        <% 
           if (surveyCollection == null)
           {
               surveyCollection = new List<Quiz>();
           }

           foreach (Quiz activeQuiz in surveyCollection)
           {%>
        <% if (string.IsNullOrEmpty(activeQuiz.Description))
           { %>
        <div class="rightTextHome">
            Select the Long Survey here, and answer the questions</div>
        <% }
           else
           { %>
        <div class="rightTextHome AddTargetBlankToLin">
            <%= activeQuiz.Description%></div>
        <% } %>
        <li class="lineList">

        <a href="<%= Url.Action("GetQuestions", "Answer") + "/"+ activeQuiz.Id %>">
            <%= activeQuiz.Title %>
        </a></li>
        <%}%>
    </ul>
</div>
<%}%>