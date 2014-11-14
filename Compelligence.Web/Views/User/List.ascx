<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.UserProfile>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'UserAll');
    }).trigger('resize');
</script>
<asp:Panel ID="UserToolbarContent" runat="server" CssClass="buttonLink">
   <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "User") %>', '<%= ViewData["Scope"] %>', 'User', 'UserAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "User") %>', '<%= ViewData["Scope"] %>', 'User', 'UserAll','<%= ViewData["Container"] %>')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('User','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "User") %>');" />
    <%--<input type="button" value="Duplicate" onclick="javascript: duplicateEntity('<%= Url.Action("Duplicate", "User") %>', '<%= ViewData["Scope"] %>', 'User', 'UserAll','<%= ViewData["Container"] %>');" />--%>
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('User','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "User") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'User', 'UserAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'User', 'UserAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.User %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Admin:Users');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>UserAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="UserDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("UserAll", "User", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="UserSearchContent" runat="server">
    <%= Html.GridSearch("UserAll") %>
</asp:Panel>
<asp:Panel ID="UserFilterContent" runat="server">
    <%= Html.GridFilter("UserAll") %>
</asp:Panel>