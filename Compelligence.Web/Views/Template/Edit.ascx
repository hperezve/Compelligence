<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Template>" %>
<% string formId = ViewData["Scope"].ToString() + "TemplateEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummaryTemplate');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#TemplateEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        });
</script>

<div id="ValidationSummaryTemplate">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "TemplateEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Template', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Template', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>        
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="TemplateEditFormInternalContent" class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="TemplateName" runat="server" Text="<%$ Resources:LabelResource, TemplateName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name"})%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="TemplateAssignedTo" runat="server" Text="<%$ Resources:LabelResource, TemplateAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Version">
                        <asp:Literal ID="TemplateVersion" runat="server" Text="<%$ Resources:LabelResource, TemplateVersion %>" />:</label>
                    <%= Html.TextBox("Version", null, new { id = formId + "Version" })%>
                    <%= Html.ValidationMessage("Version", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Language">
                        <asp:Literal ID="TemplateLanguage" runat="server" Text="<%$ Resources:LabelResource, TemplateLanguage %>" />:</label>
                    <%= Html.DropDownList("Language", (SelectList)ViewData["LanguageList"], string.Empty, new { id = formId + "Language" })%>
                    <%= Html.ValidationMessage("Language", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Format">
                        <asp:Literal ID="TemplateFormat" runat="server" Text="<%$ Resources:LabelResource, TemplateFormat %>" />:</label>
                    <%= Html.DropDownList("Format", (SelectList)ViewData["FormatList"], string.Empty, new { id = formId + "Format" })%>
                    <%= Html.ValidationMessage("Format", "*")%>
                </div>
<%--                <div class="field">
                    <label for="<%= formId %>Size">
                        <asp:Literal ID="TemplateSize" runat="server" Text="<%$ Resources:LabelResource, TemplateSize %>" />:</label>
                    <%= Html.TextBox("Size", null, new { id = formId + "Size" })%>
                    <%= Html.ValidationMessage("Size", "*")%>
                </div>--%>
                <div class="field">
                    <label for="<%= formId %>TemplateType">
                        <asp:Literal ID="TemplateType" runat="server" Text="<%$ Resources:LabelResource, TemplateType %>" />:</label>
                    <%= Html.DropDownList("TemplateType", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "TemplateType" })%>
                    <%= Html.ValidationMessage("TemplateType", "*")%>
                </div>                
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="TemplateStatus" runat="server" Text="<%$ Resources:LabelResource, TemplateStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
      
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="TemplateDescription" runat="server" Text="<%$ Resources:LabelResource, TemplateDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
