<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.CompetitorFinancial>" %>
<% string formId = ViewData["Scope"].ToString() + "CompetitorFinancialEditForm";%>

<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        $("#periodEnding").click(function() {
            $(this).datepicker().datepicker("show")
        });
        if ('<%=ViewData["EditView"]%>' == "BS") {
            $("#is").css("display", "none");
            $("#cf").css("display", "none");
        }
        if ('<%=ViewData["EditView"]%>' == "IS") {
            $("#bs").css("display", "none");
            $("#cf").css("display", "none");
        }
        if ('<%=ViewData["EditView"]%>' == "CF") {
            $("#is").css("display", "none");
            $("#bs").css("display", "none");
        }
    });
</script>
    
<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CompetitorFinancialEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'CompetitorFinancial', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <legend>Edit Financial data</legend>
        <div class="buttonLink">
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.GetData, new { onClick = "javascript: getFinancial();" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'CompetitorFinancial', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("CompetitorId",ViewData["CompetitorId"].ToString())%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("EditView")%>
        <%= Html.Hidden("succesGet")%>
        <div class="contentFormEdit">
            <div>
                 <div class="field">
                    <label for="<%= formId %>periodEnding">
                        <asp:Literal ID="periodEnding" runat="server" Text="Period Ending" />:</label>
                    <%= Html.TextBox("periodEnding", ViewData["PeriodEnding"].ToString(), new { id = "periodEnding" })%>
                    <%= Html.ValidationMessage("periodEnding", "*")%>
                </div>
                 <div class="field">
                    <label for="<%= formId %>periodType">
                        <asp:Literal ID="periodType" runat="server" Text="Period Type" />:</label>
                    <%= Html.DropDownList( "PeriodTypeList", ViewData["PeriodType"] as SelectList, new { id = formId + "PeriodType" })%>
                    <%= Html.ValidationMessage("periodType", "*")%>
                </div>
            </div>
             <div class="line">
                <div id="is" class="financial">
                    <label style="margin-left:10px;">Income Statement</label>
                    <% for (int i = 1; i < 24; i++) { %>
                    <div class="field">
                        <label >
                            <asp:Literal runat="server" Text=""/><%=IncomeStatementItems.Value[i]%>:
                        </label>
                        <%= Html.TextBox("IS" + i.ToString(), ViewData["IS" + i.ToString()].ToString().Trim(), new { id = formId + IncomeStatementItems.Id[i] })%>        
                        <%= Html.ValidationMessage(IncomeStatementItems.Id[i], "*")%>
                    </div>
                    <% } %> 
               </div>
               <div id="bs" class="financial">
                    <label style="margin-left:10px;">Balance Sheet</label>
                    <% for (int i = 1; i < 35; i++) { %>
                    <div class="field">
                        <label >
                            <asp:Literal runat="server" Text=""/><%=BalanceSheetItems.Value[i]%>:
                        </label>
                        <%= Html.TextBox("BS" + i.ToString(), ViewData["BS" + i.ToString()].ToString().Trim(), new { id = formId + BalanceSheetItems.Id[i] })%>        
                        <%= Html.ValidationMessage(BalanceSheetItems.Id[i], "*")%>
                    </div>
                    <% } %>                 
                </div>
                <div id="cf" class="financial">
                <label style="margin-left:10px;">Cash Flow</label>
                <% for (int i = 1; i < 20; i++) { %>
                    <div class="field">
                        <label >
                            <asp:Literal runat="server" Text=""/><%=CashFlowItems.Value[i]%>:
                        </label>
                        <%= Html.TextBox("CF" + i.ToString(), ViewData["CF" + i.ToString()].ToString().Trim(), new { id = formId + CashFlowItems.Id[i] })%>        
                        <%= Html.ValidationMessage(CashFlowItems.Id[i], "*")%>
                    </div>
                    <% } %> 
                </div>
            </div>
        </div>
        
    </fieldset>
</div>    
<% } %>
