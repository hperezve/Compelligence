<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'SourceDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>SourceDetailDataListContent" class="absolute">
        <asp:Panel ID="SourceDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Source") + "', '" + ViewData["Scope"] + "', 'Source', 'SourceDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Source") + "', '" + ViewData["Scope"] + "', 'Source', 'SourceDetail', 'SourceDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Source") + "', '" + ViewData["Scope"] + "', 'Source', 'SourceDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("DuplicateDetail", "Source") + "', ' " + ViewData["Scope"] + "', 'Source', 'SourceDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
        </asp:Panel>
        <asp:Panel ID="SourceDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("SourceDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="SourceDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>SourceEditFormContent" />
    </asp:Panel>
</div>