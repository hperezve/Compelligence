<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Trend>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'TrendAll');
    }).trigger('resize');
</script>
<asp:Panel ID="TrendToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Trend") %>', '<%= ViewData["Scope"] %>', 'Trend', 'TrendAll','<%= ViewData["Container"] %>');" />
    <input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Trend") %>', '<%= ViewData["Scope"] %>', 'Trend', 'TrendAll')" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Trend','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Trend") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Trend','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Trend") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Trend', 'TrendAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Trend', 'TrendAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Trend','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Trend %>');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>TrendAllSelectedOption" class="selectedOption"></span>
    
   <%-- <input id="<%= ViewData["Scope"] %>TrendCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'TrendAll', 'TrendAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>TrendCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="TrendCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label> 
    <span id="<%= ViewData["Scope"] %>TrendAllSelectedOption" class="selectedOption"></span>--%>
</asp:Panel>
<asp:Panel ID="TrendDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("TrendAll", "Trend", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="TrendSearchContent" runat="server">
    <%= Html.GridSearch("TrendAll") %>
</asp:Panel>
<asp:Panel ID="TrendFilterContent" runat="server">
    <%= Html.GridFilter("TrendAll") %>
</asp:Panel>