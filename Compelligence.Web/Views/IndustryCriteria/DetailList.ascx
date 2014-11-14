<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'IndustryCriteriaDetail');
    }).trigger('resize');
</script>
<div id="<%= ViewData["Scope"] %>IndustryCriteriaDetailDataListContent" class="absolute">
    <asp:Panel ID="IndustryCriteriaDetailToolbarContent" runat="server" CssClass="buttonLink">
        <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "IndustryCriteria") + "', '" + ViewData["Scope"] + "', 'IndustryCriteria', 'IndustryCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("EditIndustryCriteria", "IndustryCriteria") + "', '" + ViewData["Scope"] + "', 'IndustryCriteria', 'IndustryCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteEntity('" + Url.Action("DeleteIndustryCriteria", "IndustryCriteria") + "', ' " + ViewData["Scope"] + "', 'IndustryCriteria', 'IndustryCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
    </asp:Panel>
    <asp:Panel ID="IndustryCriteriaDetailDataListContent" runat="server" CssClass="contentDetailData">
        <%= Html.DataGrid("IndustryCriteriaDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
    </asp:Panel>
</div>
<asp:Panel ID="IndustryCriteriaDetailFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>IndustryCriteriaEditFormContent" />
</asp:Panel>
