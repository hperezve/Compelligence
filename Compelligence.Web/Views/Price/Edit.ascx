<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Price>" %>
<% string formId = ViewData["Scope"].ToString() + "PriceEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryPrice');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#PriceEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        var EntityId = '<%=ViewData["EntityId"]%>';
        var EntityType = '<%=ViewData["EntityType"]%>';
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetPricingTypesToIndustry", "Price")%>',
            dataType: "json",
            data: { EntityId: EntityId, EntityType: EntityType },
            success: function(data) {
                $("#PricingType").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {
                        return row.Text;
                    }
                });
            }
        });
    });
    
    $("#PricingType").result(findValuePricingTypeCallback);
    $("#PricingType").blur(function() {
        if ($(this).val() == '') {
            $("#PricingTypeId").val('');
            //$("#CriteriaSetId").val('');
            //                    $("#CriteriaSetName").val('').setOptions({ data: [] }).attr("readonly", true);
        }
        //                else {
        //                    $("#CriteriaSetName").attr("readonly", false);
        //                }
    });
    function findValuePricingTypeCallback(event, data, formatted) {
        $('#PricingTypeId').val(data.Value);
//        $("#CriteriaSetId").val('');
//        //		    $('#CriteriaSetName').attr("readonly", false).val('');
//        if ($('#CriteriaGroupId')) {
//            loadCriteriaSetData();
//        }
    };
</script>

<div id="ValidationSummaryPrice">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "PriceEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Price', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Price', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldPricingTypeId")%> 
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="PriceEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="PriceName" runat="server" Text="<%$ Resources:LabelResource, PriceName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>PartNumber">
                        <asp:Literal ID="PricePartNumber" runat="server" Text="<%$ Resources:LabelResource, PricePartNumber %>" />:</label>
                    <%= Html.TextBox("PartNumber", null, new { id = formId + "PartNumber" })%>
                    <%= Html.ValidationMessage("PartNumber", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type" class="required">
                        <asp:Literal ID="PriceType" runat="server" Text="<%$ Resources:LabelResource, PriceType %>" />:</label>
                    <%--<%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty,  new {id =formId +"Type"})%>--%>
                    <%--<%= Html.ValidationMessage("Type", "*")%>--%>
                    <%= Html.TextBox("PricingType")%>
                    <%= Html.ValidationMessage("PricingType", "*")%>
                    <%= Html.Hidden("PricingTypeId")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Units" class="required">
                        <asp:Literal ID="PriceUnits" runat="server" Text="<%$ Resources:LabelResource, PriceUnits %>" />:</label>
                    <%= Html.DropDownList("Units", (SelectList)ViewData["UnitsList"], string.Empty, new { id = formId + "Units" })%>
                    <%= Html.ValidationMessage("Units", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Value" class="required">
                        <asp:Literal ID="PriceValue" runat="server" Text="<%$ Resources:LabelResource, PriceValue %>" />:</label>
                    <%= Html.TextBox("ValueFrm", null, new { id = formId + "Value" })%>
                    <%= Html.ValidationMessage("ValueFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="PriceStatus" runat="server" Text="<%$ Resources:LabelResource, PriceStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Required">
                        <asp:Literal ID="PriceRequired" runat="server" Text="<%$ Resources:LabelResource, PriceRequired %>" />:</label>
                    <%= Html.DropDownList("Required", (SelectList)ViewData["RequiredList"], string.Empty, new { id = formId + "Required" })%>
                    <%= Html.ValidationMessage("Required", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Url">
                        <asp:Literal ID="PriceUrl" runat="server" Text="<%$ Resources:LabelResource, PriceUrl %>" />:</label>
                    <%= Html.TextBox("Url", null, new { id = formId + "Url" })%>
                    <%= Html.ValidationMessage("Url", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
