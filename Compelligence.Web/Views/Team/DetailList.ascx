    <%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'TeamDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>TeamDetailDataListContent" class="absolute">
        <asp:Panel ID="TeamDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Team") + "', '" + ViewData["Scope"] + "', 'Team', '" + ViewData["BrowseDetailName"] + "', 'TeamDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Team") + "', '" + ViewData["Scope"] + "', 'Team', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Team") + "', '" + ViewData["Scope"] + "', 'Team', 'TeamDetail', 'TeamDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Team") + "', '" + ViewData["Scope"] + "', 'Team', 'TeamDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="TeamDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%--<%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>--%>
            <%= Html.DataGrid("TeamDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="TeamDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>TeamEditFormContent" />
    </asp:Panel>
    <asp:Panel ID="TeamIndustryDetailFilterContent" runat="server">
    <%= Html.GridFilter("TeamIndustryDetail") %>
    </asp:Panel>
</div>
