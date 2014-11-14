<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.TeamRole>" %>
<% string formId = ViewData["Scope"].ToString() + "TeamRoleEditForm"; %>

<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        });
</script>

<%= Html.ValidationSummary() %>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "TeamRoleEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'TeamRole', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'TeamRole', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="Name" class="required">
                        <asp:Literal ID="TeamRoleName" runat="server" Text="<%$ Resources:LabelResource, TeamRoleName %>" />:</label>
                    <%= Html.TextBox("Name") %>
                    <%= Html.ValidationMessage("Name", "*") %>
                </div>
                <div class="field">
                    <label for="Description">
                        <asp:Literal ID="TeamRoleDescription" runat="server" Text="<%$ Resources:LabelResource, TeamRoleDescription %>" />:</label>
                    <%= Html.TextArea("Description")%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
