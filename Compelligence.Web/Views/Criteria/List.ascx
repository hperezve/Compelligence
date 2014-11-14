<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Criteria>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'CriteriaAll');
    }).trigger('resize');
</script>
<asp:Panel ID="CriteriaToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Criteria") %>', '<%= ViewData["Scope"] %>', 'Criteria', 'CriteriaAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Criteria") %>', '<%= ViewData["Scope"] %>', 'Criteria', 'CriteriaAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Criteria','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Criteria") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Criteria','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Criteria") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Criteria', 'CriteriaAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Criteria', 'CriteriaAll');" />
    <span id="<%= ViewData["Scope"] %>CriteriaAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="CriteriaDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("CriteriaAll", "Criteria", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="CriteriaSearchContent" runat="server">
    <%= Html.GridSearch("CriteriaAll") %>
</asp:Panel>
<asp:Panel ID="CriteriaFilterContent" runat="server">
    <%= Html.GridFilter("CriteriaAll") %>
</asp:Panel>
</asp:Panel>