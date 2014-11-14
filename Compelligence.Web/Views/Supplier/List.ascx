<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Supplier>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'SupplierAll');
    }).trigger('resize');
</script>
<asp:Panel ID="SupplierToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Supplier") %>', '<%= ViewData["Scope"] %>', 'Supplier', 'SupplierAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Supplier") %>', '<%= ViewData["Scope"] %>', 'Supplier', 'SupplierAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Supplier','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Supplier") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Supplier','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Supplier") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Supplier', 'SupplierAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Supplier', 'SupplierAll');" />
    <input id="<%= ViewData["Scope"] %>SupplierCheckbox" class="checkbox" type="checkbox" value="Show Mine" onchange="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'SupplierAll', 'SupplierAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>SupplierCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="SupplierCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <span id="<%= ViewData["Scope"] %>SupplierAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="SupplierDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow">
        <%= Html.DataGrid("SupplierAll", "Supplier", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="SupplierSearchContent" runat="server">
    <%= Html.GridSearch("SupplierAll") %>
</asp:Panel>
<asp:Panel ID="SupplierFilterContent" runat="server">
    <%= Html.GridFilter("SupplierAll") %>
</asp:Panel>