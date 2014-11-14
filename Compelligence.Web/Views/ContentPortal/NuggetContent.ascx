<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity"  %>


	<%--Submit your Nugget Reports here, Critical to our competitive information is your participation. Please let us know how you are doing--%>
	<label for="LblMessage1">
		<asp:Literal ID="LtMessage1" runat="server" Text="<%$ Resources:LabelResource, ContentPortalNuggetContentMessage1 %>" />
	</label>
<br />
<br />

<% IList<Quiz> QuizCollection = (IList<Quiz>)ViewData["Nuggets"];
   if (QuizCollection == null)
       QuizCollection = new List<Quiz>();
    foreach (Quiz ActiveQuiz in QuizCollection)
   {%>
<div>
    <b class="colorBlack">
    <a href='<%=Url.Action("Questions", "Quiz",new {QuizId=ActiveQuiz.Id},null)%>'>
            <%=ActiveQuiz.Title%>. </a></b>
</div>
<%}%>



