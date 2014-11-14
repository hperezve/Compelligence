<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ConfigurationLabelsDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>ConfigurationLabelsDetailDataListContent" class="absolute">
        <asp:Panel ID="ConfigurationLabelsDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "ConfigurationLabels") + "', '" + ViewData["Scope"] + "', 'ConfigurationLabels', 'ConfigurationLabelsDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "ConfigurationLabels") + "', '" + ViewData["Scope"] + "', 'ConfigurationLabels', 'ConfigurationLabelsDetail', 'ConfigurationLabelsAllSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "ConfigurationLabels") + "', '" + ViewData["Scope"] + "', 'ConfigurationLabels', 'ConfigurationLabelsDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "ConfigurationLabels") + "', '" + ViewData["Scope"] + "', 'ConfigurationLabels', 'ConfigurationLabelsDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "ConfigurationLabels") + "', '" + ViewData["Scope"] + "', 'ConfigurationLabels', 'ConfigurationLabelsDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
        </asp:Panel>
        <asp:Panel ID="ConfigurationLabelsDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("ConfigurationLabelsDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="ConfigurationLabelsDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ConfigurationLabelsEditFormContent" />
    </asp:Panel>
</div>
