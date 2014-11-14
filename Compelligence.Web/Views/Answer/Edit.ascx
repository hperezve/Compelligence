<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Answer>" %>
<% string formId = ViewData["Scope"].ToString() + "AnswerEditForm"; %>

<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
    });
</script>

<%--<script type="text/javascript">
    var visible = function() {
        var selectedValue = $('#<%= formId %> [name=Type] > option:selected').attr('value');    
        
        if (selectedValue != '') { 
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.SurveyVisibility.Long %>') {
                $('#<%= formId %> [name=Visible]').attr("disabled", "disabled");
            } else {
                $('#<%= formId %> [name=Visible]').removeAttr("disabled");
            }
        }
        //$(formId + ' input[type=text][name=' + dateFormFields[i] + ']').datepicker();
    };    
</script>--%>
<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SurveyAnswerContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["AnswerSecurityAccess"] + "', '" +  ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Answer', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Answer', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <% using (Html.BeginForm("Answerss", "Survey", FormMethod.Post, new { id = "Quiz" }))
           { %>
        <input type="hidden" id="QuizId" name="QuizId" value='<%=ViewData["QuizId"] %>' />
        <table>
            <% IList<Question> QuestionCollection = (IList<Question>)ViewData["QuizQuestions"];
               foreach (Question ActiveQuestion in QuestionCollection)
               {%>
            <tr>
                <td>
                    <%=Html.QuestionControl(ActiveQuestion)%>
                </td>
            </tr>
            <%} %>
        </table>
        <div class="padding10">
            <input class="button" id="Submit1" type="submit" value="Send" />
            <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("Index","FrontEnd") %>'" />
        </div>
        <%} %>
    </fieldset>
</div>
<% } %>
