<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'WinLossDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>WinLossDetailDataListContent" class="absolute">
        <asp:Panel ID="WinLossDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "WinLoss") + "', '" + ViewData["Scope"] + "', 'WinLoss', 'WinLossDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "WinLoss") + "', '" + ViewData["Scope"] + "', 'WinLoss', 'WinLossDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "WinLoss") + "', '" + ViewData["Scope"] + "', 'WinLoss', 'WinLossDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteEntity('" + Url.Action("Delete", "WinLoss") + "', '" + ViewData["Scope"] + "', 'WinLoss', 'WinLossDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "WinLoss") + "', '" + ViewData["Scope"] + "', 'WinLoss', 'WinLossDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="WinLossDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("WinLossDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="WinLossDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>WinLossEditFormContent" />
    </asp:Panel>
</div>