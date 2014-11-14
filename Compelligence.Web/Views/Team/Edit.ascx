<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Team>" %>
<% string formId = ViewData["Scope"].ToString() + "TeamEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryTeam');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#TeamEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>StartDateFrm']);
        ResizeHeightForm();
        });
</script>

<div id="ValidationSummaryTeam">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "TeamEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "StartDateFrm']); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Team', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Team', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="TeamEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="TeamName" runat="server" Text="<%$ Resources:LabelResource, TeamName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="TeamAssignedTo" runat="server" Text="<%$ Resources:LabelResource, TeamAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>StartDateFrm">
                        <asp:Literal ID="StartDate" runat="server" Text="<%$ Resources:LabelResource, TeamDate %>" />:</label>
                    <%= Html.TextBox("StartDateFrm", null, new { id = formId + "StartDateFrm" })%>
                    <%= Html.ValidationMessage("StartDateFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Email">
                        <asp:Literal ID="TeamEmail" runat="server" Text="<%$ Resources:LabelResource, TeamEmail %>" />:</label>
                    <%= Html.TextBox("Email", null, new { id = formId + "Email" })%>
                    <%= Html.ValidationMessage("Email", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status" class="required">
                        <asp:Literal ID="TeamStatus" runat="server" Text="<%$ Resources:LabelResource, TeamStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
