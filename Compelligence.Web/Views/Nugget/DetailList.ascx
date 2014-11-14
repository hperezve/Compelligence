<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'NuggetDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>NuggetDetailDataListContent" class="absolute">
        <asp:Panel ID="NuggetDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Nugget") + "', '" + ViewData["Scope"] + "', 'Nugget', 'NuggetDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Nugget") + "', '" + ViewData["Scope"] + "', 'Nugget', 'NuggetDetail', 'NuggetDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Nugget") + "', '" + ViewData["Scope"] + "', 'Nugget', 'NuggetDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Nugget") + "', '" + ViewData["Scope"] + "', 'Nugget', 'NuggetDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "Nugget") + "', '" + ViewData["Scope"] + "', 'Nugget', 'NuggetDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="NuggetDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("NuggetDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="NuggetDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>NuggetEditFormContent" />
    </asp:Panel>
</div>
