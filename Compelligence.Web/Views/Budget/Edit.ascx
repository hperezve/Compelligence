<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Budget>" %>
<% string formId = ViewData["Scope"].ToString() + "BudgetEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryBudget');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#BudgetEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
    var typeselected = function() {
        var SelectedType = '<%= ViewData["BudgetType"] %>';
        var SelectedUnit = '<%= ViewData["BudgetUnit"] %>';
        if (SelectedType != null) {
            if (SelectedType == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeUnit.Financial %>') {
                $('#<%= formId %>Type').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeUnit.Financial %>');
                $('#<%= formId %>Type').prop('disabled', 'disabled');
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Dollars %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Dollars %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');

                }
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Euro %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Euro %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Peso %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Peso %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Real %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Real %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Yen %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeFinancial.Yen %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
            }
            if (SelectedType == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeUnit.Time %>') {
                $('#<%= formId %>Type').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeUnit.Time %>');
                $('#<%= formId %>Type').prop('disabled', 'disabled');
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Days %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Days %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Hours %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Hours %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Months %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Months %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
                if (SelectedUnit == '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Weeks %>') {
                    $('#<%= formId %>UnitMeasureCode').prop('value', '<%= Compelligence.Domain.Entity.Resource.BudgetTypeTime.Weeks %>');
                    $('#<%= formId %>UnitMeasureCode').prop('disabled', 'disabled');
                }
            }

        }
    };
</script>
<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        typeselected();
        ResizeHeightForm();
    });
</script>

<div id="ValidationSummaryBudget">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "BudgetEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Budget', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Budget', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("OldType")%>
        <%= Html.Hidden("OldValue")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="BudgetEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="BudgetName" runat="server" Text="<%$ Resources:LabelResource, BudgetName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="BudgetAssignedTo" runat="server" Text="<%$ Resources:LabelResource, BudgetOwner %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <%--<div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="BudgetType" runat="server" Text="<%$ Resources:LabelResource, BudgetType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>--%>
                <%--<div class="field">
                    <label for="<%= formId %>BudgetTypeId">
                        <asp:Literal ID="BudgetTypeId" runat="server" Text="<%$ Resources:LabelResource, BudgetType %>" />:</label>
                    <%= Html.CascadingParentDropDownList("BudgetTypeId", (SelectList)ViewData["TypeList"], string.Empty, Url.Action("GetUnitsByType", "Budget"), formId, "UnitMeasureCode")%>
                    <%= Html.ValidationMessage("BudgetTypeId", "*")%>
                </div>--%>
                <div class="field">
                    <label for="<%= formId %>Type" class="required">
                        <asp:Literal ID="BudgetType" runat="server" Text="<%$ Resources:LabelResource, BudgetType %>" />:</label>
                    <%= Html.CascadingParentDropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, Url.Action("GetUnitsByType", "Budget", new { detailfilter = ViewData["DetailFilter"].ToString() }), formId, "UnitMeasureCode")%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
            </div>
            <div class="line">
                <%--<div class="field">
                    <label for="<%= formId %>UnitMeasureId">
                        <asp:Literal ID="BudgetUnitMeasureId" runat="server" Text="<%$ Resources:LabelResource, BudgetUnitMeasureId %>" />:</label>
                    <%= Html.DropDownList("UnitMeasureId", (SelectList)ViewData["UnitMeasureList"], string.Empty, new { id = formId + "UnitMeasureId" })%>
                    <%= Html.ValidationMessage("UnitMeasureId", "*")%>
                </div>--%>
                <div class="field"  id="UnitMeasureCode">
                    <label for="<%= formId %>UnitMeasureCode">
                        <asp:Literal ID="BudgetUnitMeasureCode" runat="server" Text="<%$ Resources:LabelResource, BudgetUnitMeasureId %>" />:</label>
                    <%= Html.CascadingChildDropDownList("UnitMeasureCode", (SelectList)ViewData["UnitMeasureList"], string.Empty, formId)%>
                    <%= Html.ValidationMessage("UnitMeasureCode", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Value">
                        <asp:Literal ID="BudgetValue" runat="server" Text="<%$ Resources:LabelResource, BudgetValue %>" />:</label>
                    <%= Html.TextBox("ValueFrm", null, new { id = formId + "Value" })%>
                    <%= Html.ValidationMessage("ValueFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="BudgetStatus" runat="server" Text="<%$ Resources:LabelResource, BudgetStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Comment">
                        <asp:Literal ID="BudgetComment" runat="server" Text="<%$ Resources:LabelResource, BudgetComment %>" />:</label>
                    <%= Html.TextArea("Comment", new { id = formId + "Comment" })%>
                    <%= Html.ValidationMessage("Comment", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>