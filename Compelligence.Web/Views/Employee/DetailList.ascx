<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'EmployeeDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>EmployeeDetailDataListContent" class="absolute">
        <asp:Panel ID="EmployeeDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Employee") + "', '" + ViewData["Scope"] + "', 'Employee', 'EmployeeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Employee") + "', '" + ViewData["Scope"] + "', 'Employee', 'EmployeeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "Employee") + "', '" + ViewData["Scope"] + "', 'Employee', 'EmployeeDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="EmployeeDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("EmployeeDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="EmployeeDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>EmployeeEditFormContent" />
    </asp:Panel>
</div>
