<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'UserRelationAll');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>UserRelationDetailDataListContent" class="absolute">
   <%--  <asp:Panel ID="UserRelationDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "EntityRelation") + "', '" + ViewData["Scope"] + "', 'EntityRelation', 'EntityRelationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "EntityRelation") + "', '" + ViewData["Scope"] + "', 'EntityRelation', 'EntityRelationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteEntity('" + Url.Action("Delete", "EntityRelation") + "', '" + ViewData["Scope"] + "', 'EntityRelation', 'EntityRelationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>--%>
        <asp:Panel ID="UserRelationDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("UserRelationAll", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="UserRelationDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>UserRelationEditFormContent" />
    </asp:Panel>
</div>