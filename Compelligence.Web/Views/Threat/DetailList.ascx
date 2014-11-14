<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ThreatDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>ThreatDetailDataListContent" class="absolute">
        <asp:Panel ID="ThreatDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Threat") + "', '" + ViewData["Scope"] + "', 'Threat', 'ThreatDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Threat") + "', '" + ViewData["Scope"] + "', 'Threat', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Threat") + "', '" + ViewData["Scope"] + "', 'Threat', 'ThreatDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "Threat") + "', '" + ViewData["Scope"] + "', 'Threat', 'ThreatDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            
        </asp:Panel>
        <asp:Panel ID="ThreatDetailDataListContent" runat="server" CssClass="contentDetailData">
           <%-- <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>--%>
           <%= Html.DataGrid("ThreatDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="ThreatDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ThreatEditFormContent" />
    </asp:Panel>
</div>