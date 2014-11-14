<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>CriteriaGroupDetailDataListContent" class="absolute">
        <asp:Panel ID="CriteriaGroupDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "CriteriaGroup") + "', '" + ViewData["Scope"] + "', 'CriteriaGroup', '" + ViewData["BrowseDetailName"] + "', 'CriteriaGroupDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "CriteriaGroup") + "', '" + ViewData["Scope"] + "', 'CriteriaGroup', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="CriteriaGroupDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="CriteriaGroupDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>CriteriaGroupEditFormContent" />
    </asp:Panel>
</div>
