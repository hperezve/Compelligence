<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Library>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'LibraryAll');
    }).trigger('resize');
</script>
<asp:Panel ID="LibraryToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryByEntityAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Library','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Library") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Library','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Library") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Library', 'LibraryAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Library', 'LibraryAll');" />
    <input id="<%= ViewData["Scope"] %>LibraryCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'LibraryAll', 'LibraryAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>LibraryCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="LibraryCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Library %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Environment:Libraries');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>LibraryAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="LibraryDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow" style="display: block"><%= Html.DataGrid("LibraryAll", "Library", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="LibrarySearchContent" runat="server">
    <%= Html.GridSearch("LibraryAll")%>
</asp:Panel>
<asp:Panel ID="LibraryFilterContent" runat="server">
    <%= Html.GridFilter("LibraryAll")%>
</asp:Panel>