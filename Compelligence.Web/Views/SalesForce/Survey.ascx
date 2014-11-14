<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

<link href="<%= Url.Content("~/Content/Styles/Survey.css") %>" rel="stylesheet" type="text/css" />

<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.filestyle.js") %>" type="text/javascript" charset="utf-8"></script>

<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
  
<script type="text/javascript">

    function ChangeSFAction(url) {
        //showLoadingDialog();
        $('#FrontEndForm').prop("action", url);
        $('#FrontEndForm').submit();
    }

</script>

  <script type="text/javascript">

        $(function() {       
        $("input[type=file]").filestyle({
        image: '<%= Url.Content("~/Images/Styles/panelrigth_title_8.jpg") %>',
        imageheight : 16,
        imagewidth : 65,
        width : 280
        });
        });
  
</script>

<div id="contentleftmenu">

</div>
<br />    
<%--Next lines need verify--%>
<%if (ViewData["Message"] == null){%>
    <% Quiz survey = (Quiz)ViewData["LongSurvey"];
       if (survey != null)
       {   %>
             
        <span class="title"><%= Html.Encode(survey.Title)%></span>
       
        <% if (survey.SalesForceType.Equals(SurveySalesForceType.Html))
           { %>
         <div class="backLongSurvey">
         <%=survey.Description %>
         </div>
     <%}else{ %>
        <div class="backLongSurvey">
        <% string message = (string)ViewData["ErrorMessage"]; %>
        <% if (!string.IsNullOrEmpty(message))
           { %>
           <span class="marginTextSurvey" style="color:red"><%= Html.Encode(message)%></span>
        <% }
           else
           { %>
        <span class="marginTextSurvey">Answer the following questions</span>
        <% } %>
        <div id="LongSurveyForm">
            <% using (Html.BeginForm("Answer", "SalesForce",
                   FormMethod.Post, new { id = "LongSurveyForm", ENCTYPE = "multipart/form-data" }))
               { %>
                   <%=Html.Hidden("U",(string)ViewData["U"]) %>
                   <%=Html.Hidden("C",(string)ViewData["C"]) %>
    
            <input type="hidden" id="QuizId" name="Id" value="<%= survey.Id %>" />
            <table style="font-family:Arial;font-size:small">
                <%if (survey.Questions != null)
                  {
                      foreach (Question question in survey.Questions)
                      {
                %>
                <tr>
                    <td>
                        <%=Html.QuestionControl(survey, question, ViewData["Q" + question.Item])%>
                    </td>
                </tr>
                <%}
                  }
                  else
                  {%>
no questions
<%} %>          <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%=Html.MultiUploadControlSurvey()%>                        
                    </td>
                </tr>
            </table>
            <div class="padding10">
                <input class="buttonSurvey" id="Submit1" type="submit" value="Submit" />
            </div>
            <%} %>
        </div>
    </div>
       <%} %>
     <%}
      else
      { %>
    There's no result.
    <%} %>
    
<%} %>
<%if (ViewData["Message"] != null)
  { %>
    <%=ViewData["Message"]%>
<%} %>


