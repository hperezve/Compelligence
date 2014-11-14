<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'PriceDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>PriceDetailDataListContent" class="absolute">
        <asp:Panel ID="PriceDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Price") + "', '" + ViewData["Scope"] + "', 'Price', 'PriceDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Price") + "', '" + ViewData["Scope"] + "', 'Price', 'PriceDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "Price") + "', '" + ViewData["Scope"] + "', 'Price', 'PriceDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("Duplicate", "Price") + "', '" + ViewData["Scope"] + "', 'Price', 'PriceDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Price', 'PriceDetail');" />
            <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Price', 'PriceDetail');" />
        </asp:Panel>
        <asp:Panel ID="PriceDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("PriceDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="PriceDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>PriceEditFormContent" />
    </asp:Panel>
    <asp:Panel ID="PriceDetailSearchContent" runat="server">
        <%= Html.GridSearch("PriceDetail")%>
    </asp:Panel>
    <asp:Panel ID="PriceDetailFilterContent" runat="server">
        <%= Html.GridFilter("PriceDetail")%>
    </asp:Panel>
</div>
