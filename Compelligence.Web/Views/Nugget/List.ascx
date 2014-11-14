<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Quiz>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'NuggetAll');
    }).trigger('resize');
</script>
<asp:Panel ID="NuggetToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Nugget") %>', '<%= ViewData["Scope"] %>', 'Nugget', 'NuggetAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Nugget") %>', '<%= ViewData["Scope"] %>', 'Nugget', 'NuggetAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Nugget','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Nugget") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Nugget','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Nugget") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Nugget', 'NuggetAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Nugget', 'NuggetAll');" />
    <span id="<%= ViewData["Scope"] %>NuggetAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="NuggetDataListContent" runat="server" CssClass="contentDetailData">
    <%= Html.DataGrid("NuggetAll", "Nugget", ViewData["Container"].ToString())%>
</asp:Panel>
<asp:Panel ID="NuggetSearchContent" runat="server">
    <%= Html.GridSearch("NuggetAll") %>
</asp:Panel>
<asp:Panel ID="NuggetFilterContent" runat="server">
    <%= Html.GridFilter("NuggetAll") %>
</asp:Panel>