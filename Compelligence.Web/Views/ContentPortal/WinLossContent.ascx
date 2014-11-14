<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% IList<Quiz> QuizCollection = (IList<Quiz>)ViewData["WinLoss"];
   if (QuizCollection != null && QuizCollection.Count > 0)
   {
   foreach (Quiz ActiveQuiz in QuizCollection)
   {%>
       <a href="<%=Url.Action("GetQuestions", "Answer", new { id = ActiveQuiz.Id})%>" class="fesubmenu" onmouseover="showOptions('mnuTools','optionsTools');" onmouseout="hideOptions('optionsWinLoss');hideOptions('optionsTools');">
            <%=ActiveQuiz.Title%></a>
   <%}%>
 
<%}%>
