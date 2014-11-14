<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');
</script>
<div id="<%= ViewData["Scope"] %>ProjectDetailDataListContent" class="absolute">
    <asp:Panel ID="ProjectDetailToolbarContent" runat="server" CssClass="buttonLink">
        <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Project") + "', '" + ViewData["Scope"] + "', 'Project', '" + ViewData["BrowseDetailName"]+ "', 'ProjectDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Project") + "', '" + ViewData["Scope"] + "', 'Project', '" + ViewData["BrowseDetailName"]+ "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
    </asp:Panel>
    <asp:Panel ID="ProjectDetailDataListContent" runat="server" CssClass="contentDetailData">
        <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
    </asp:Panel>
</div>
<asp:Panel ID="ProjectDetailFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProjectEditFormContent" />
</asp:Panel>

