<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ProductFamilyAll');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>ProductFamilyDetailDataListContent" class="absolute">
        <asp:Panel ID="ProductFamilyToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail   , new { onClick = "javascript: newEntity('" + Url.Action("Create"   , "ProductFamily") + "', '" + ViewData["Scope"] + "', 'ProductFamily','ProductFamilyAll', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail  , new { onClick = "javascript: editEntity('" + Url.Action("Edit"    , "ProductFamily") + "', '" + ViewData["Scope"] + "', 'ProductFamily','ProductFamilyAll', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteEntity('" + Url.Action("Delete", "ProductFamily") + "', '" + ViewData["Scope"] + "', 'ProductFamily', 'ProductFamilyAll', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
<%--            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "Plan") + "', '" + ViewData["Scope"] + "', 'Plan', 'PlanDetail',  '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
--%>        </asp:Panel>
        <asp:Panel ID="PlanDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("ProductFamilyAll", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="ProductFamilyFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ProductFamilyEditFormContent" />
    </asp:Panel>

</div>