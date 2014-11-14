<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.IndustryFinancial>" %>
<% string formId = ViewData["Scope"].ToString() + "IndustryFinancialEditForm"; %>

<script type="text/javascript">
    var funtionTest = function() {
        var value = $('#' + '<%=formId%>' + "TimePeriod").val();
        if (value == 'Quarterly') {
            $('#Year').css("display", "block");
        }
        else {
            $('#Year').css("display", "none");
        }
    };
    var showYearForm = function(selected, value) {
        if (value == 'Quarterly') {
            $('#Year').css("display", "block");
        }
        else {
            $('#Year').css("display", "none");
        }
        //Html.CascadingParentDropDownList("TimePeriod", (SelectList)ViewData["TimePeriodList"], string.Empty, Url.Action("GetUpdateTimePeriodValue", "IndustryFinancial"), formId, "TimePeriodValue", new string[] { },"funtionTest(this.value);")%>
        getCascadeObjects('<%= Url.Action("GetUpdateTimePeriodValue", "IndustryFinancial") %>', '#EnvironmentIndustryIndustryFinancialEditForm [name=TimePeriod]', '#EnvironmentIndustryIndustryFinancialEditForm [name=TimePeriodValue]', '#EnvironmentIndustryIndustryFinancialEditFormTimePeriodValueLoader', [], []);
    };
</script>

<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        funtionTest();
    });
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "IndustryFinancialEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'IndustryFinancial', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");funtionTest();}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'IndustryFinancial', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("IndustryId",ViewData["IndustryId"].ToString())%>
        <%= Html.Hidden("TimePeriodOld")%>
        <%= Html.Hidden("TimePeriodValueOld")%>
        <%= Html.Hidden("YearOld")%>
        <%= Html.Hidden("TotalAddressableMarketGlobalOld")%>
        <%= Html.Hidden("TotalAddressableMarketProjectedOld")%>
        <%= Html.Hidden("CAGRActualOld")%>
        <%= Html.Hidden("CAGRProjectedOld")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>TimePeriod" class="required">
                        <asp:Literal ID="IndustryFinancialTimePeriod" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceTimePeriod %>" />:</label>
                    <%= Html.DropDownList("TimePeriod", (SelectList)ViewData["TimePeriodList"], string.Empty, new { onchange = "javascript: showYearForm(this, this.value);", id = formId + "TimePeriod" })%>
                    <%--<%= Html.CascadingParentDropDownList("TimePeriod", (SelectList)ViewData["TimePeriodList"], string.Empty, Url.Action("GetUpdateTimePeriodValue", "IndustryFinancial"), formId, "TimePeriodValue", new string[] { },"funtionTest(this.value);")%>--%>
                    <%= Html.ValidationMessage("TimePeriod", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TimePeriodValue" class="required">
                        <asp:Literal ID="IndustryFinancialTimePeriodValue" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceTimePeriodValue %>" />:</label>
                    <%= Html.CascadingChildDropDownList("TimePeriodValue", (SelectList)ViewData["TimePeriodValueList"], string.Empty, formId)%>
                    <%= Html.ValidationMessage("TimePeriodValue", "*")%>
                </div>
                <div id="Year" class="field" style="display: none">
                    <label for="<%= formId %>Year" class="required">
                        <asp:Literal ID="IndustryFinancialYear" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceYear %>" />:</label>
                    <%= Html.DropDownList("Year", (SelectList)ViewData["YearList"], string.Empty, formId)%>
                    <%= Html.ValidationMessage("Year", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Source">
                        <asp:Literal ID="IndustryFinancialSource" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceSource %>" />:</label>
                    <%= Html.TextBox("Source", null, new { id = formId + "Source" })%>
                    <%= Html.ValidationMessage("Source", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>TotalAddressableMarketGlobal" class="required">
                        <asp:Literal ID="IndustryFinancialTotalAddressableMarketGlobal" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceTotalAddressableMarketGlobal %>" />:</label>
                    <%= Html.TextBox("TotalAddressableMarketGlobal", null, new { id = formId + "TotalAddressableMarketGlobal" })%>
                    <%= Html.ValidationMessage("TotalAddressableMarketGlobal", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TotalAddressableMarketProjected" class="required">
                        <asp:Literal ID="IndustryFinancialTotalAddressableMarketProjected" runat="server"
                            Text="<%$ Resources:LabelResource, FinancialPerformanceTotalAddressableMarketProjected %>" />:</label>
                    <%= Html.TextBox("TotalAddressableMarketProjected", null, new { id = formId + "TotalAddressableMarketProjected" })%>
                    <%= Html.ValidationMessage("TotalAddressableMarketProjected", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CAGRActual">
                        <asp:Literal ID="IndustryFinancialCAGRActual" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceCAGRActual %>" />:</label>
                    <%= Html.TextBox("CAGRActual", null, new { id = formId + "CAGRActual" })%>
                    <%= Html.ValidationMessage("CAGRActual", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CAGRProjected">
                        <asp:Literal ID="IndustryFinancialCAGRProjected" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceCAGRProjected %>" />:</label>
                    <%= Html.TextBox("CAGRProjected", null, new { id = formId + "CAGRProjected" })%>
                    <%= Html.ValidationMessage("CAGRProjected", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="UseParentFigures">
                        Use Parent Figures</label>
                    <%= Html.CheckBox("UseParentFiguresVal")%>
                </div>
                <div class="field">
                    <label for="Required">
                        Required</label>
                    <%= Html.CheckBox("RequiredVal")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="IndustryFinancialDescription" runat="server" Text="<%$ Resources:LabelResource, FinancialPerformanceDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>