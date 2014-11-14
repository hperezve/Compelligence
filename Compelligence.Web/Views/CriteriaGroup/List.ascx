<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.CriteriaGroup>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'CriteriaGroupAll');
    }).trigger('resize');
</script>
<asp:Panel ID="CriteriaGroupToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "CriteriaGroup") %>', '<%= ViewData["Scope"] %>', 'CriteriaGroup', 'CriteriaGroupAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "CriteriaGroup") %>', '<%= ViewData["Scope"] %>', 'CriteriaGroup', 'CriteriaGroupAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('CriteriaGroup','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "CriteriaGroup") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('CriteriaGroup','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "CriteriaGroup") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'CriteriaGroup', 'CriteriaGroupAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'CriteriaGroup', 'CriteriaGroupAll');" />
    <span id="<%= ViewData["Scope"] %>CriteriaGroupAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="CriteriaGroupDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("CriteriaGroupAll", "CriteriaGroup", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="CriteriaGroupSearchContent" runat="server">
    <%= Html.GridSearch("CriteriaGroupAll")%>
</asp:Panel>
<asp:Panel ID="CriteriaGroupFilterContent" runat="server">
    <%= Html.GridFilter("CriteriaGroupAll")%>
</asp:Panel>