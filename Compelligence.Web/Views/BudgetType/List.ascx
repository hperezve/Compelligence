<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.BudgetType>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'BudgetTypeAll');
    }).trigger('resize');
</script>
<asp:Panel ID="BudgetTypeToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "BudgetType") %>', '<%= ViewData["Scope"] %>', 'BudgetType', 'BudgetTypeAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "BudgetType") %>', '<%= ViewData["Scope"] %>', 'BudgetType', 'BudgetTypeAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('BudgetType','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "BudgetType") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('BudgetType','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "BudgetType") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'BudgetType', 'BudgetTypeAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'BudgetType', 'BudgetTypeAll');" />
</asp:Panel>
<asp:Panel ID="BudgetTypeDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("BudgetTypeAll", "BudgetType", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="BudgetTypeSearchContent" runat="server">
    <%= Html.GridSearch("BudgetTypeAll")%>
</asp:Panel>
<asp:Panel ID="BudgetTypeFilterContent" runat="server">
    <%= Html.GridFilter("BudgetTypeAll")%>
</asp:Panel>