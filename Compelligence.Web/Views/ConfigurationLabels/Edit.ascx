<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.ConfigurationLabels>" %>
<% string formId = ViewData["Scope"].ToString() + "ConfigurationLabelsEditForm"; %>

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
               UpdateTargetId = (string)ViewData["Scope"] + "ConfigurationLabelsEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DateFrm']); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','ConfigurationLabels', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'ConfigurationLabels', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
                    <label for="<%= formId %>Value" class="required">
                        <asp:Literal ID="ConfigurationLabelsValue" runat="server" Text="<%$ Resources:LabelResource, ConfigurationLabelsValue %>" />:</label>
                    <%= Html.TextBox("Value", null, new { id = formId + "Value"})%>
                    <%= Html.ValidationMessage("Value", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Name">
                        <asp:Literal ID="ConfigurationLabelsName" runat="server" Text="<%$ Resources:LabelResource, ConfigurationLabelsName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name", @readonly = "readonly", @class = "Disable" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Description" class="required">
                        <asp:Literal ID="ConfigurationLabelsDescription" runat="server" Text="<%$ Resources:LabelResource, ConfigurationLabelsDescription %>" />:</label>
                    <%= Html.TextBox("Description", null, new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
                <%--Remove by Ticket 1694
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="ConfigurationLabelsStatus" runat="server" Text="<%$ Resources:LabelResource, ConfigurationLabelsStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>--%>
             </div>
             <div class="line"> 
                    <div class="fieldLabel">               
                    <label for="ConfigurationLabelsInstructionalText">
                        <asp:Literal ID="ConfigurationLabelsInstructionalText" runat="server" Text="<%$ Resources:LabelResource, ConfigurationLabelsInstructionalText %>"></asp:Literal>
                    </label>
                    </div>                
             </div>
          </div>
    </fieldset>
</div>
<% } %>
