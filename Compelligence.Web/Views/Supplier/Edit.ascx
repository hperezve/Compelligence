<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Supplier>" %>
<% string formId = ViewData["Scope"].ToString() + "SupplierEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummarySupplier');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#SupplierEditFormInternalContent').css('height', newHeigth + 'px');
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

<div id="ValidationSummarySupplier">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SupplierEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "'); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Supplier', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Supplier', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldName")%>        
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="SupplierEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="SupplierName" runat="server" Text="<%$ Resources:LabelResource, SupplierName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="SupplierAssignedTo" runat="server" Text="<%$ Resources:LabelResource, SupplierAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
            </div>
            <%--<div class="line">
        <div class="field">
            <label for="AssignedTo" class="required">
                <asp:Literal ID="SupplierAssignedTo" runat="server" Text="<%$ Resources:LabelResource, SupplierAssignedTo %>" />:</label>
            <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty)%>
            <%= Html.ValidationMessage("AssignedTo", "*")%>
        </div>
        <div class="field">
            <label for="ProductId" class="required">
                <asp:Literal ID="SupplierProductId" runat="server" Text="<%$ Resources:LabelResource, SupplierProductId %>" />:</label>
            <%= Html.DropDownList("ProductId", (SelectList)ViewData["ProductIdList"], string.Empty)%>
            <%= Html.ValidationMessage("ProductId", "*")%>
        </div>
    </div>
    <div class="line">
        <div class="field">
            <label for="Status" class="required">
                <asp:Literal ID="SupplierStatus" runat="server" Text="<%$ Resources:LabelResource, SupplierStatus %>" />:</label>
            <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty)%>
            <%= Html.ValidationMessage("Status", "*")%>
        </div>
        <div class="field">
            <label for="Type" class="required">
                <asp:Literal ID="SupplierType" runat="server" Text="<%$ Resources:LabelResource, SupplierType %>" />:</label>
            <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty)%>
            <%= Html.ValidationMessage("Type", "*")%>
        </div>
    </div>--%>
            <div class="line">
                <%--<div class="field">
            <label for="DueDateFrm">
                <asp:Literal ID="SupplierDueDateFrm" runat="server" Text="<%$ Resources:LabelResource, SupplierDueDate %>" />:</label>
            <%= Html.TextBox("SupplierDueDateFrm")%>
            <%= Html.ValidationMessage("SupplierDueDateFrm", "*")%>
            <label for="Budget" class="required">
                <asp:Literal ID="SupplierBudget" runat="server" Text="<%$ Resources:LabelResource, SupplierBudget %>" />:</label>
            <%= Html.TextBox("Budget")%>
            <%= Html.ValidationMessage("Budget", "*")%>
        </div>--%>
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="SupplierDescription" runat="server" Text="<%$ Resources:LabelResource, SupplierDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="SupplierMetaData" runat="server" Text="<%$ Resources:LabelResource, SupplierMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
