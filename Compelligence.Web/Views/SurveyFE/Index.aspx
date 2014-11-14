<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/frontendsite.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import namespace="Compelligence.Domain.Entity"  %>
<%@ Import namespace="Compelligence.Domain.Entity.Resource"  %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Survey Questions</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

 <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />
 <script src="<%= Url.Content("~/Scripts/jquery-1.3.2.min.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>

  <script type="text/javascript">        
        var visibleDiv = "N";
        $(function() {
            <%
                IList<Quiz> quizList = (IList<Quiz>)ViewData["QuizCollection"];
                foreach (Quiz quiz in quizList)
                { %>
                    $("#<%= quiz.Id.ToString() %>Question").hide();
            <%  } %>
        });
  
      $(document).ready(function() {
            $('th').attr('style', 'text-align:left;font-size:medium ');
        }
      );
      function Load2Tag(target, source) 
      {
        $.post(source, function(data) {  $('#'+target).append( data );});

         // alert(source);
        //$('#'+target).load(source);
      }
      
      function ShowQuestions(questionDiv) 
      {
        if (visibleDiv == "N") {
              $(questionDiv).show();
              visibleDiv = "Y";
        }   
        else if (visibleDiv == "Y") { 
                $(questionDiv).hide();
              visibleDiv = "N";
            }   
      }
     
  </script>
  
     
  <script type="text/javascript" src="../../Scripts/System/FrontEnd/Upload.js"></script>  
  
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titleText">Welcome </div>
    
    Select the Long Survey here, and answer the questions

<% IList<Quiz> QuizCollection = (IList<Quiz>)ViewData["QuizCollection"];
   if (QuizCollection == null)
       QuizCollection = new List<Quiz>();
    foreach (Quiz ActiveQuiz in QuizCollection)
   {%>
<div>
    <b style="color: black">
    <a href="#" onclick="ShowQuestions('#<%=ActiveQuiz.Id.ToString()%>Question')">
        <%=ActiveQuiz.Title%>. </a></b>
   
   <div id="<%=ActiveQuiz.Id.ToString()%>Question">
   <% IList<Question> QuestionCollection = ActiveQuiz.QuestionList;
   foreach(Question ActiveQuestion in QuestionCollection) 
  {%>
     <tr>
     <td>
        <%=Html.QuestionControl(ActiveQuestion) %>
     </td>
     </tr>
<%} %>
        <center>  <input type="submit" value="Send" /> </center>
   </div>
   
</div>
 <%}%>
<div>
<% using (Html.BeginForm("Answers","Quiz",FormMethod.Post)) 
 {%>

<%--<input type="hidden" id="QuizId" name="QuizId" value='<%=ViewData["QuizId"] %>' />--%>
<table style="background-color:white">
   
<%--<% IList<Question> QuestionCollection = (IList<Question>)ViewData["QuestionCollection"];
   foreach(Question ActiveQuestion in QuestionCollection) 
  {%>
     <tr>
     <td>
        <%=Html.QuestionControl(ActiveQuestion) %>
     </td>
     </tr>
<%} %>--%>
</table>
</div>
<%} %>

</asp:Content>

