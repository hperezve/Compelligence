<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'EntityRelationDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>EntityRelationDetailDataListContent" class="absolute">
        <asp:Panel ID="EntityRelationDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "EntityRelation") + "', '" + ViewData["Scope"] + "', 'EntityRelation', 'EntityRelationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "EntityRelation") + "', '" + ViewData["Scope"] + "', 'EntityRelation', 'EntityRelationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteEntity('" + Url.Action("Delete", "EntityRelation") + "', '" + ViewData["Scope"] + "', 'EntityRelation', 'EntityRelationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="EntityRelationDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("EntityRelationDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="EntityRelationDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>EntityRelationEditFormContent" />
    </asp:Panel>
</div>
