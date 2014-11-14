<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'IndustryFinancialDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>IndustryFinancialDetailDataListContent" class="absolute">
        <asp:Panel ID="IndustryFinancialDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "IndustryFinancial") + "', '" + ViewData["Scope"] + "', 'IndustryFinancial', 'IndustryFinancialDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "IndustryFinancial") + "', '" + ViewData["Scope"] + "', 'IndustryFinancial', 'IndustryFinancialDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "IndustryFinancial") + "', '" + ViewData["Scope"] + "', 'IndustryFinancial', 'IndustryFinancialDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("Duplicate", "IndustryFinancial") + "', '" + ViewData["Scope"] + "', 'IndustryFinancial', 'IndustryFinancialDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
       
        <asp:Panel ID="IndustryFinancialDetailDataListContent" runat="server" CssClass="contentDetailData">
   <table id="list4"></table>   
           <%= Html.DataGrid("IndustryFinancialDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="IndustryFinancialDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>IndustryFinancialEditFormContent" />
    </asp:Panel>
</div>