<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'MarketTypeDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>MarketTypeDetailDataListContent" class="absolute">
        <asp:Panel ID="MarketTypeDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "MarketType") + "', '" + ViewData["Scope"] + "', 'MarketType', 'MarketTypeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "MarketType") + "', '" + ViewData["Scope"] + "', 'MarketType', 'MarketTypeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "MarketType") + "', '" + ViewData["Scope"] + "', 'MarketType', 'MarketTypeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("Duplicate", "MarketType") + "', '" + ViewData["Scope"] + "', 'MarketType', 'MarketTypeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            --%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "MarketType") + "', '" + ViewData["Scope"] + "', 'MarketType', 'MarketTypeDetail', 'MarketTypeDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "MarketType") + "', '" + ViewData["Scope"] + "', 'MarketType', 'MarketTypeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="MarketTypeDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("MarketTypeDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="MarketTypeDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>MarketTypeEditFormContent" />
    </asp:Panel>
</div>
