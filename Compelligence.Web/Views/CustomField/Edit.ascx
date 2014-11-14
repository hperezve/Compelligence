<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.CustomField>" %>
<% string formId = ViewData["Scope"].ToString() + "CustomFieldEditForm"; %>

<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DateFrm']);
    });
</script>

<%= Html.ValidationSummary() %>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CustomFieldEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DateFrm']); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','CustomField', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'CustomField', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>  
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("PhysicalName")%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="CustomFieldName" runat="server" Text="<%$ Resources:LabelResource, CustomFieldName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type" class="required">
                        <asp:Literal ID="CustomFieldType" runat="server" Text="<%$ Resources:LabelResource, CustomFieldType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Size" class="required">
                        <asp:Literal ID="CustomFieldSize" runat="server" Text="<%$ Resources:LabelResource, CustomFieldSize %>" />:</label>
                    <%= Html.TextBox("Size", null, new { id = formId + "Size" })%>
                    <%= Html.ValidationMessage("Size", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>EntityType" class="required">
                        <asp:Literal ID="CustomFieldEntityType" runat="server" Text="<%$ Resources:LabelResource, CustomFieldEntityType %>" />:</label>
                    <%= Html.DropDownList("EntityType", (SelectList)ViewData["EntityTypeList"], string.Empty, new { id = formId + "EntityType" })%>
                    <%= Html.ValidationMessage("EntityType", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
