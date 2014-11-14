<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.LibraryType>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'LibraryTypeAll');
    }).trigger('resize');
</script>
<asp:Panel ID="LibraryTypeToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "LibraryType") %>', '<%= ViewData["Scope"] %>', 'LibraryType', 'LibraryTypeAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "LibraryType") %>', '<%= ViewData["Scope"] %>', 'LibraryType', 'LibraryTypeAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('LibraryType','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "LibraryType") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('LibraryType','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "LibraryType") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'LibraryType', 'LibraryTypeAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'LibraryType', 'LibraryTypeAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.LibraryType %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Admin:Library Type');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>LibraryTypeAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="LibraryTypeDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("LibraryTypeAll", "LibraryType", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="LibraryTypeSearchContent" runat="server">
    <%= Html.GridSearch("LibraryTypeAll")%>
</asp:Panel>
<asp:Panel ID="LibraryTypeFilterContent" runat="server">
    <%= Html.GridFilter("LibraryTypeAll")%>
</asp:Panel>