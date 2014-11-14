<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Trend>" %>
<% string formId = ViewData["Scope"].ToString() + "TrendEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryTrend');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 70 - (10 * lis.length);
                var edt = $('#TrendEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };    
</script>
<script type="text/javascript">
    $(function() {
    initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>StartDateFrm', 'dt:#<%= formId %>EndDateFrm', 'dt:#<%= formId %>DateOfMaturityFrm']);
    ResizeHeightForm();
    });

</script>

<div id="ValidationSummaryTrend">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "TrendEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "',['dt:#" + formId + "StartDateFrm'],['dt:#" + formId + "EndDateFrm'],['dt:#" + formId + "DateOfMaturityFrm']); ResizeHeightForm();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Trend', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = (string)ViewData["Scope"] + "TrendEditForm" }))
   { %>
<div id="trendIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Trend', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
            <%--<input class="button" type="button" value="Report" onclick="javascript: executeReport('TrendAll');" />--%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("Id", default(decimal))%>
        
        <div id="TrendEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="TrendName" runat="server" Text="<%$ Resources:LabelResource, TrendName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "TrendName" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="TrendAssignedTo" runat="server" Text="<%$ Resources:LabelResource, TrendAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="TrendType" runat="server" Text="<%$ Resources:LabelResource, TrendType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id=formId + "Type"})%>
                    <%= Html.ValidationMessage("Type","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>StartDateFrm">
                        <asp:Literal ID="TrendStartDate" runat="server" Text="<%$ Resources:LabelResource, TrendStartDate %>" />:</label>
                    <%= Html.TextBox("StartDateFrm", null, new { id = formId + "StartDateFrm" })%>
                    <%= Html.ValidationMessage("StartDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>EndDateFrm">
                        <asp:Literal ID="TrendEndDate" runat="server" Text="<%$ Resources:LabelResource, TrendEndDate %>" />:</label>
                    <%= Html.TextBox("EndDateFrm", null, new { id = formId + "EndDateFrm" })%>
                    <%= Html.ValidationMessage("EndDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DateOfMaturityFrm">
                        <asp:Literal ID="TrendDateOfMaturity" runat="server" Text="<%$ Resources:LabelResource, TrendDateOfMaturity %>" />:</label>
                    <%= Html.TextBox("DateOfMaturityFrm", null, new { id= formId + "DateOfMaturityFrm"})%>
                    <%= Html.ValidationMessage("DateOfMaturityFrm","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Tier">
                    <asp:Literal ID="TrendTier" runat="server" Text="<% $ Resources:LabelResource, TrendTier %>" />:</label>
                    <%= Html.DropDownList("Tier", (SelectList)ViewData["TierList"], string.Empty, new { id=formId + "Tier"})%>
                    <%= Html.ValidationMessage("Tier","*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>IndustryType">
                    <asp:Literal ID="TrendIndustryType" runat="server" Text="<% $ Resources:LabelResource, TrendIndustryType %>"/>:</label>
                    <%= Html.DropDownList("IndustryType", (SelectList)ViewData["IndustryTypeList"], string.Empty, new {id=formId + "IndustryType"}) %>
                    <%= Html.ValidationMessage("IndustryType","*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>ProjectedValue">
                    <asp:Literal ID="TrendProjectedValue" runat="server" Text="<% $ Resources:LabelResource, TrendProjectedValue %>"></asp:Literal>:</label>
                    <%= Html.TextBox("ProjectedValue", null, new { id = formId + "ProjectedValue" })%>
                    <%= Html.ValidationMessage("ProjectedValue","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>GeoType">
                    <asp:Literal ID="TrendGeoType" runat="server" Text="<% $ Resources:LabelResource, TrendGeoType %>" />:</label>
                    <%= Html.DropDownList("GeoType", (SelectList)ViewData["GeoTypeList"], string.Empty, new { id = formId + "GeoType" })%>
                    <%= Html.ValidationMessage("GeoType","*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>ForSwot">
                    <asp:Literal ID="TrendForSwot" runat="server" Text="<% $ Resources:LabelResource, TrendForSwot %>" />:</label>
                    <%= Html.DropDownList("ForSwot", (SelectList)ViewData["ForSwotList"], string.Empty, new { id = formId + "ForSwot" })%>
                    <%= Html.ValidationMessage("ForSwot", "*")%>
                </div>
                <%--<div class="field">
                    <label for="<%= formId %>TrendEvents">
                    <asp:Literal ID="TrendTrendEvents" runat="server" Text="<% $ Resources:LabelResource, TrendTrendEvents %>" />:</label>
                    <%= Html.TextBox("TrendEvents", null, new {id=formId + "TrendEvents"}) %>
                    <%= Html.ValidationMessage("TrendEvents", "*")%>
                </div>--%>
                <%--<div class="field">
                    <label for="<%= formId %>TrendLibrary">
                    <asp:Literal ID="TrendTrendLibrary" runat="server" Text="<% $ Resources:LabelResource, TrendTrendLibrary %>" />:</label>
                    <%= Html.TextBox("TrendLibrary", null, new { id=formId + "TrendLibrary"})%>
                    <%= Html.ValidationMessage("TrendLibrary", "*")%>
                </div>--%>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="TrendDescription" runat="server" Text="<% $ Resources:LabelResource, TrendDescription %>"/>:</label>
                        <%= Html.TextArea("Description", new  { id=formId + "Description"})%>
                        <%= Html.ValidationMessage("Description", "*") %>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>