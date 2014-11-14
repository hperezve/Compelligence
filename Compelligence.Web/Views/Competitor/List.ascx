<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Competitor>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorAll');
    }).trigger('resize');
</script>
<asp:Panel ID="CompetitorToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Competitor") %>', '<%= ViewData["Scope"] %>', 'Competitor', 'CompetitorAll','<%= ViewData["Container"] %>');" />
<%--    <input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Competitor") %>', '<%= ViewData["Scope"] %>', 'Competitor', 'CompetitorAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Competitor','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Competitor") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Competitor','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Competitor") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity ('<%= ViewData["Scope"] %>', 'Competitor', 'CompetitorAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity ('<%= ViewData["Scope"] %>', 'Competitor', 'CompetitorAll');" />
    <input class="button" type="button" value="Enable" onclick="javascript: changeStatusOfEntity('Competitor','CompetitorAll','Enable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("UpdateEnableStatus", "Competitor") %>');" />
    <input class="button" type="button" value="Disable" onclick="javascript: changeStatusOfEntity('Competitor','CompetitorAll','Disable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("UpdateDisableStatus", "Competitor") %>');" />
    <input id="<%= ViewData["Scope"] %>CompetitorCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'CompetitorAll', 'CompetitorAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>CompetitorCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="CompetitorCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Competitor%>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Environment:Competitors');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>CompetitorAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="CompetitorDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("CompetitorAll", "Competitor", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="CompetitorSearchContent" runat="server">
    <%= Html.GridSearch("CompetitorAll") %>
</asp:Panel>
<asp:Panel ID="CompetitorFilterContent" runat="server">
    <%= Html.GridFilter("CompetitorAll") %>
</asp:Panel>