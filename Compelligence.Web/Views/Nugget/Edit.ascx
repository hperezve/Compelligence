<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Quiz>" %>
<% string formId = ViewData["Scope"].ToString() + "NuggetEditForm"; %>

<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        });
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "NuggetEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Nugget', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Nugget', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldTitle")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Title" class="required">
                        <asp:Literal ID="NuggetTitle" runat="server" Text="<%$ Resources:LabelResource, NuggetTitle %>" />:</label>
                    <%= Html.TextBox("Title", null, new { id = formId + "Title" })%>
                    <%= Html.ValidationMessage("Title", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status" class="required">
                        <asp:Literal ID="NuggetStatus" runat="server" Text="<%$ Resources:LabelResource, NuggetStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="NuggetAssignedTo" runat="server" Text="<%$ Resources:LabelResource, NuggetAssignedTo %>" />:</label>
                    <%=Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="NuggetMetaData" runat="server" Text="<%$ Resources:LabelResource, NuggetMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
