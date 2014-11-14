<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'LibraryDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>LibraryDetailDataListContent" class="absolute">
        <asp:Panel ID="LibraryDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Library") + "', '" + ViewData["Scope"] + "', 'Library', 'LibraryDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Library") + "', '" + ViewData["Scope"] + "', 'Library', 'LibraryDetail', 'LibraryAllSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Library") + "', '" + ViewData["Scope"] + "', 'Library', 'LibraryDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Library") + "', '" + ViewData["Scope"] + "', 'Library', 'LibraryDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "Library") + "', '" + ViewData["Scope"] + "', 'Library', 'LibraryDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
        </asp:Panel>
        <asp:Panel ID="LibraryDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("LibraryDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="LibraryDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>LibraryEditFormContent" />
    </asp:Panel>
</div>