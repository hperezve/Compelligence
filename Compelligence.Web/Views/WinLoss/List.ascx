<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Quiz>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'WinLossAll');
    }).trigger('resize');
</script>
<asp:Panel ID="WinLossToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "WinLoss") %>', '<%= ViewData["Scope"] %>', 'WinLoss', 'WinLossAll','<%= ViewData["Container"] %>');" />
   <%-- <input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "WinLoss") %>', '<%= ViewData["Scope"] %>', 'WinLoss', 'WinLossAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('WinLoss','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "WinLoss") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('WinLoss','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "WinLoss") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'WinLoss', 'WinLossAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'WinLoss', 'WinLossAll');" />
    <input id="<%= ViewData["Scope"] %>WinLossCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'WinLossAll', 'QuizAllView', 'AssignedTo','<%= Session["UserId"] %>' );" />
    <label for="<%= ViewData["Scope"] %>WinLossCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="WinLossCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.WinLoss %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Tools:Win Loss');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>WinLossAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="WinLossDataListContent" runat="server" CssClass="contentDetailData">
    <%= Html.DataGrid("WinLossAll", "WinLoss", ViewData["Container"].ToString())%>
</asp:Panel>
<asp:Panel ID="WinLossSearchContent" runat="server">
    <%= Html.GridSearch("WinLossAll") %>
</asp:Panel>
<asp:Panel ID="WinLossFilterContent" runat="server">
    <%= Html.GridFilter("WinLossAll") %>
</asp:Panel>