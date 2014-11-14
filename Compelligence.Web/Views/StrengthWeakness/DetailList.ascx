<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'StrengthWeaknessDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>StrengthWeaknessDetailDataListContent" class="absolute">
        <asp:Panel ID="StrengthWeaknessDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "StrengthWeakness") + "', '" + ViewData["Scope"] + "', 'StrengthWeakness', 'StrengthWeaknessDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "StrengthWeakness") + "', '" + ViewData["Scope"] + "', 'StrengthWeakness', 'StrengthWeaknessDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "StrengthWeakness") + "', '" + ViewData["Scope"] + "', 'StrengthWeakness', 'StrengthWeaknessDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("Duplicate", "StrengthWeakness") + "', '" + ViewData["Scope"] + "', 'StrengthWeakness', 'StrengthWeaknessDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="StrengthWeaknessDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("StrengthWeaknessDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="StrengthWeaknessDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>StrengthWeaknessEditFormContent" />
    </asp:Panel>
</div>