<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.LibraryExternalSource>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'LibraryExternalSourceAll');
    }).trigger('resize');
</script>
<asp:Panel ID="LibraryExternalSourceToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "LibraryExternalSource") %>', '<%= ViewData["Scope"] %>', 'LibraryExternalSource', 'LibraryExternalSourceAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "LibraryExternalSource") %>', '<%= ViewData["Scope"] %>', 'LibraryExternalSource', 'LibraryExternalSourceAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('LibraryExternalSource','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "LibraryExternalSource") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('LibraryExternalSource','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "LibraryExternalSource") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'LibraryExternalSource', 'LibraryExternalSourceAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'LibraryExternalSource', 'LibraryExternalSourceAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.LibraryExternalSource %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Admin:Library External Source');" style="float: right;margin-right: 5px;"/><br />    
    <span id="<%= ViewData["Scope"] %>LibraryExternalSourceAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="LibraryExternalSourceDataListContent" runat="server" CssClass="contentDetailData" >
    <%= Html.DataGrid("LibraryExternalSourceAll", "LibraryExternalSource", ViewData["Container"].ToString())%>
</asp:Panel>
<asp:Panel ID="LibraryExternalSourceSearchContent" runat="server">
    <%= Html.GridSearch("LibraryExternalSourceAll") %>
</asp:Panel>
<asp:Panel ID="LibraryExternalSourceFilterContent" runat="server">
    <%= Html.GridFilter("LibraryExternalSourceAll")%>
</asp:Panel>