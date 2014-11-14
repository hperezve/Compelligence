<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Employee>" %>

<% string formId = ViewData["Scope"].ToString() + "EmployeeEditPasswordForm"; %>

<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>');
        });
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm("EditPassword", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "EmployeeEditFormContent",
               OnBegin = "showLoadingDialog",
               OnComplete = "hideLoadingDialog",
               OnSuccess = "function(){ initializeForm('#" + formId + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Employee', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>

    <fieldset>
        <legend>
            Edit Password</legend>
        <div class="buttonLink">
            <input class="button" type="submit" value="Save" />
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
            <input class="button" type="button" value="Cancel" onclick="javascript: getEntity('<%= Url.Action("Edit", "Employee") %>', '<%= ViewData["Scope"] %>', 'Employee', '<%= Model.Id %>', '<%= ViewData["BrowseId"] %>', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
<div class="contentFormEdit">
        <div class="line">
            <div class="field">
                <label for="CurrentPassword" class="required">
                    <asp:Literal ID="EmployeeCurrentPassword" runat="server" Text="<%$ Resources:LabelResource, EmployeeCurrentPassword %>" />:</label>
                <%= Html.Password("CurrentPassword")%>
                <%= Html.ValidationMessage("CurrentPassword", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="Password" class="required">
                    <asp:Literal ID="EmployeeNewPassword" runat="server" Text="<%$ Resources:LabelResource, EmployeeNewPassword %>" />:</label>
                <%= Html.Password("keyPassword")%>
                <%= Html.ValidationMessage("Password", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="RePassword" class="required">
                    <asp:Literal ID="EmployeeRePassword" runat="server" Text="<%$ Resources:LabelResource, EmployeeRePassword %>" />:</label>
                <%= Html.Password("RePassword")%>
                <%= Html.ValidationMessage("RePassword", "*")%>
            </div>
        </div>
</div>
    </fieldset>

<% } %>



