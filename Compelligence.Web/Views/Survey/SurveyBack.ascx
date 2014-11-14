<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<%--<% string formId = ViewData["Scope"].ToString() + "SurveyEditForm"; %>--%>
<% string formId = ViewData["Scope"].ToString() + "SurveyResultForm"; %>
<% decimal quizid = decimal.Parse(ViewData["QuizId"].ToString()); %>
<script type="text/javascript">
    var hiddenMessage = function() {
    objSurveyBackMessage = document.getElementById('SurveyBackMessage');
        objSurveyBackMessage.style.display = "none";
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>');
    });
</script>
<div class="contentSurveyItems">
   <%OperationStatus astatus = ((OperationStatus)ViewData["OperationStatus"]);%>
    <% using (Ajax.BeginForm("AnswersBackend", "Answer", new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SurveyResultContent",
               //OnBegin = "showLoadingDialog",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               //OnComplete = "function() { hideLoadingDialog();onCOM();}",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnFailure = "showFailedResponseDialog",
               OnSuccess = "function(content) { if ( content.get_data()=='Success') MessageDlg('Message','Success..!');isSuccessful();}"
               //OnSuccess = "function(content) { " + (astatus.Equals(OperationStatus.Successful) ? "alert('ok " + astatus.ToString() + "')" : "alert('!ok " + astatus .ToString()+ "')") + "}"
               
           }, new { id = formId }))
       { %>
    <%= Html.Hidden("Scope")%>
    <%= Html.Hidden("BrowseId")%>
    <%= Html.Hidden("IsDetail")%>
    <%= Html.Hidden("OperationStatus")%>
    <%= Html.Hidden("Container")%>
    <%= Html.Hidden("HeaderType")%>
    <%= Html.Hidden("DetailFilter")%>
    <input type="hidden" id="QuizId" name="QuizId" value='<%=ViewData["QuizId"] %>' />
    <% Quiz survey = (Quiz)ViewData["quiz"]; %>
    <div id="contentrespond" style="border-bottom: 1px solid black; margin: 5px">
        <div class="tblOne" style="padding: 5px">
            <label for="SelectEndUser">
                <asp:Literal ID="SurveySelectEndUser" runat="server" Text="<%$ Resources:LabelResource, SurveySelectUser %>" />:</label>
            <%= Html.DropDownList("Created", (SelectList)ViewData["AssignedToEndUserList"], new { id = formId + "Created", onclick = "hiddenMessage();" })%>
        </div>
    </div>
    <div id="SurveyBackMessage" style="display: block">
    <% string message = (string)ViewData["Message"]; %>

    
    <% if (!string.IsNullOrEmpty(message))
       { %>
            <% if (message.IndexOf("Thank") != -1)
               {%>
                    <%--<span class="marginTextSurvey" style="size:14">
                       <%= Html.Encode(message) %></span>--%>
                       <%--<%=Html.SendSuccessDialog(message)%>--%>
                       <script type="text/javascript">
                           showSendResponseMessageDialog();
                       </script>
            <%}
               else
               { %>
                    <p><span class="marginTextSurvey" style="color: red">
                        <%= Html.Encode(message) %></span></p>
            <% } %>
    <% } %>
    </div>
    <table>
        <% IList<Question> QuestionCollection = (IList<Question>)ViewData["QuizQuestions"];
           foreach (Question ActiveQuestion in QuestionCollection)
           {%>
        <tr>
            <td>
                <%=Html.QuestionControl(survey, ActiveQuestion, ViewData["Q" + ActiveQuestion.Item], (MultiSelectList)ViewData["IndustryIdMultiList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], "Product","Competitor","Industry")%>
            </td>
        </tr>
        <%} %>
    </table>
    
    <div class="buttonLink">
        <input class="button" id="Submit1" type="submit" value="Send" />
        <input class="button" type="button" value="Reset" onclick="hiddenMessage();javascript: resetFormFields('#<%= formId %>');" />
        <%--<input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("SurveyBack","Survey")+ "/  ?QuizId=      " + Id %>'"/>--%>
    </div>
    <%} %>
</div>
