<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.MarketType>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'MarketTypeAll');
    }).trigger('resize');
</script>
<asp:Panel ID="MarketTypeToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "MarketType") %>', '<%= ViewData["Scope"] %>', 'MarketType', 'MarketTypeAll','<%= ViewData["Container"] %>');" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('MarketType','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "MarketType") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('MarketType','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "MarketType") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'MarketType', 'MarketTypeAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'MarketType', 'MarketTypeAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.MarketType %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Environment:Market Type');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>MarketTypeAllSelectedOption" class="selectedOption"></span>
   
</asp:Panel>
<asp:Panel ID="MarketTypeDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("MarketTypeAll", "MarketType", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="MarketTypeSearchContent" runat="server">
    <%= Html.GridSearch("MarketTypeAll") %>
</asp:Panel>
<asp:Panel ID="MarketTypeFilterContent" runat="server">
    <%= Html.GridFilter("MarketTypeAll") %>
</asp:Panel>