<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<% string typeObject = ViewData["TypeObject"].ToString();
   string formId = ViewData["Scope"].ToString() + typeObject + "ResultForm";
   string formResetId = ViewData["Scope"].ToString() + typeObject + "ResultForm"; %>
<% decimal quizid = decimal.Parse(ViewData["QuizId"].ToString()); %>

<script type="text/javascript">
    var hiddenMessage = function() {
        objAnswerBackMessage = document.getElementById('AnswerBackMessage');
        objAnswerBackMessage.style.display = "none";
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>');
        $('input').blur();
    });
    $(document).ready(function() {
    $('#<%= ViewData["Scope"] %><%= ViewData["TypeObject"] %>ResultFormCreated').focus();
    });
</script>

<div id="ValidationSummaryAnswer">
    <%= Html.ValidationSummary()%>
</div>
<div>
    <div id="AnswerBackMessage" style="display: block">
        <% string message = (string)ViewData["Message"]; %>
        <% if (!string.IsNullOrEmpty(message))
           { %>
        <% if (message.IndexOf("Thank") != -1)
           {%>

        <script type="text/javascript">
            showSendResponseMessageDialog();
        </script>

        <%}
           else
           { %>
        <ul class="validation-summary-errors">
            <li>
                <%= Html.Encode(message) %>
            </li>
        </ul>
        <% } %>
        <% } %>
    </div>
</div>
<%OperationStatus astatus = ((OperationStatus)ViewData["OperationStatus"]);%>
<% using (Ajax.BeginForm("Respond", "Answer", new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + typeObject + "ResultContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnFailure = "showFailedResponseDialog",
               OnSuccess = "function(content) { if ( content.get_data()=='Success') MessageDlg('Message','Success..!');}"
           }, new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <div class="buttonLink">
            <input class="button" id="Submit1" type="submit" value="Send" />
            <input class="button" type="button" value="Reset" onclick="javascript: hiddenMessage();resetFormFields('#<%= formId %>');" />
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("TypeObject")%>
        <input type="hidden" id="QuizId" name="QuizId" value='<%=ViewData["QuizId"] %>' />
        <% Quiz survey = (Quiz)ViewData["quiz"]; %>
        <div class="contentFormEdit">
            <div id="contentrespond" style="border-bottom: 1px solid black; margin: 5px">
                <div class="tblOne" style="padding: 5px">
                    <label for="SelectEndUser">
                        <asp:Literal ID="SurveySelectEndUser" runat="server" Text="<%$ Resources:LabelResource, SurveySelectUser %>" />:</label>
                    <%= Html.DropDownList("Created", (SelectList)ViewData["AssignedToEndUserList"], new { id = formId + "Created", onclick = "hiddenMessage();" })%>
                </div>
            </div>
            <% IList<Question> QuestionCollection = (IList<Question>)ViewData["QuizQuestions"];
               foreach (Question ActiveQuestion in QuestionCollection)
               {%>
            <div class="line">
                <div class="field">
                    <% if (ViewData["PrimaryIndustryList"] != null || ViewData["PrimaryCompetitorList"] != null)
                       { %>
                    <%=Html.QuestionControl(survey, ActiveQuestion, ViewData["Q" + ActiveQuestion.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"], (SelectList)ViewData["PrimaryIndustryList"], (SelectList)ViewData["PrimaryCompetitorList"])%>
                    <% }
                       else
                       { %>
                    <%=Html.QuestionControl(survey, ActiveQuestion, ViewData["Q" + ActiveQuestion.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"])%>
                    <% } %>
                </div>
            </div>
            <%} %>
        </div>
    </fieldset>
</div>
<%} %>
