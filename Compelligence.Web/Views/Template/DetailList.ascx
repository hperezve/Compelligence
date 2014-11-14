<%--Second Div For User of event--%>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'UserDetail');
    }).trigger('resize');
</script>
<div class="indexTwo2">
    <div id="<%= ViewData["Scope"] %>UserDetailDataListContent" class="absolute">
        <asp:Panel ID="UserDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "User") + "', '" + ViewData["Scope"] + "', 'User', '" + ViewData["BrowseDetailName"] + "', 'UserDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "User") + "', '" + ViewData["Scope"] + "', 'User', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "User") + "', '" + ViewData["Scope"] + "', 'User', 'UserDetail', 'UserDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "User") + "', '" + ViewData["Scope"] + "', 'User', 'UserDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="UserDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%--<%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>--%>
            <%= Html.DataGrid("UserDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="UserDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>UserEditFormContent" />
    </asp:Panel>
    <asp:Panel ID="UserIndustryDetailFilterContent" runat="server">
    <%= Html.GridFilter("UserIndustryDetail") %>
    </asp:Panel>
</div>