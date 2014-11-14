<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Team>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'TeamAll');
    }).trigger('resize');
</script>
<asp:Panel ID="TeamToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Team") %>', '<%= ViewData["Scope"] %>', 'Team', 'TeamAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Team") %>', '<%= ViewData["Scope"] %>', 'Team', 'TeamAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Team','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Team") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Team', 'TeamAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Team', 'TeamAll');" />
    <input id="<%= ViewData["Scope"] %>TeamCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'TeamAll', 'TeamAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>TeamCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="TeamCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', 'TEAM','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Admin:Teams');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>TeamAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="TeamDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("TeamAll", "Team", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="TeamSearchContent" runat="server">
    <%= Html.GridSearch("TeamAll") %>
</asp:Panel>
<asp:Panel ID="TeamFilterContent" runat="server">
    <%= Html.GridFilter("TeamAll") %>
</asp:Panel>