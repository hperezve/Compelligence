<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.CriteriaSet>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'CriteriaSetAll');
    }).trigger('resize');
</script>
<asp:Panel ID="CriteriaSetToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "CriteriaSet") %>', '<%= ViewData["Scope"] %>', 'CriteriaSet', 'CriteriaSetAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "CriteriaSet") %>', '<%= ViewData["Scope"] %>', 'CriteriaSet', 'CriteriaSetAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('CriteriaSet','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "CriteriaSet") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('CriteriaSet','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "CriteriaSet") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'CriteriaSet', 'CriteriaSetAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'CriteriaSet', 'CriteriaSetAll');" />
    <span id="<%= ViewData["Scope"] %>CriteriaSetAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="CriteriaSetDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("CriteriaSetAll", "CriteriaSet", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="CriteriaSetSearchContent" runat="server">
    <%= Html.GridSearch("CriteriaSetAll") %>
</asp:Panel>
<asp:Panel ID="CriteriaSetFilterContent" runat="server">
    <%= Html.GridFilter("CriteriaSetAll") %>
</asp:Panel>