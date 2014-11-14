<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.ConfigurationLabels>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ConfigurationLabelsAll');
    }).trigger('resize');
</script>
<asp:Panel ID="ConfigurationLabelsToolbarContent" runat="server" CssClass="buttonLink">
    <%--<input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "ConfigurationLabels") %>', '<%= ViewData["Scope"] %>', 'ConfigurationLabels', 'ConfigurationLabelsAll','<%= ViewData["Container"] %>');" />--%>
    <input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "ConfigurationLabels") %>', '<%= ViewData["Scope"] %>', 'ConfigurationLabels', 'ConfigurationLabelsAll')" />
    <%--<input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('ConfigurationLabels','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "ConfigurationLabels") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('ConfigurationLabels','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "ConfigurationLabels") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'ConfigurationLabels', 'ConfigurationLabelsAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'ConfigurationLabels', 'ConfigurationLabelsAll');" />--%>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', 'CNL','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Admin:Configurations:Labels');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>ConfigurationLabelsAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="ConfigurationLabelsDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("ConfigurationLabelsAll", "ConfigurationLabels", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="ConfigurationLabelsSearchContent" runat="server">
    <%= Html.GridSearch("ConfigurationLabelsAll")%>
</asp:Panel>
<asp:Panel ID="ConfigurationLabelsFilterContent" runat="server">
    <%= Html.GridFilter("ConfigurationLabelsAll")%>
</asp:Panel>