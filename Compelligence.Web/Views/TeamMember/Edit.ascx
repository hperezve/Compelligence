<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.TeamMember>" %>
<% string formId = ViewData["Scope"].ToString() + "TeamMemberEditForm"; %>

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
               UpdateTargetId = (string)ViewData["Scope"] + "TeamMemberEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'TeamMember', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'TeamMember', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
                    <label for="<%= formId %>UserId" class="required">
                        <asp:Literal ID="TeamMemberUserId" runat="server" Text="<%$ Resources:LabelResource, TeamMemberUserId %>" />:</label>
                    <%= Html.DropDownList("UserId", (SelectList)ViewData["UserList"], string.Empty, new { id = formId + "UserId" })%>
                    <%= Html.ValidationMessage("UserId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TeamRoleId" class="required">
                        <asp:Literal ID="TeamMemberTeamRoleId" runat="server" Text="<%$ Resources:LabelResource, TeamMemberTeamRoleId %>" />:</label>
                    <%= Html.DropDownList("TeamRoleId", (SelectList)ViewData["TeamRoleList"], string.Empty, new { id = formId + "TeamRoleId" })%>
                    <%= Html.ValidationMessage("TeamRoleId", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
