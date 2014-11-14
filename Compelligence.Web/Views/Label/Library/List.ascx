<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Library>>" %>

<asp:Panel ID="LibraryToolbarContent" runat="server" CssClass="buttonLink">
    <input type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryAll');" />
    <input type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryAll')" />
    <input type="button" value="Delete" onclick="javascript: deleteEntity('<%= Url.Action("Delete", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryAll');" />
    <input type="button" value="Duplicate" onclick="javascript: duplicateEntity('<%= Url.Action("Duplicate", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryAll');" />
    <input type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Library', 'LibraryAll');" />
    <input type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Library', 'LibraryAll');" />
</asp:Panel>
<asp:Panel ID="LibraryDataListContent" runat="server" ScrollBars="Auto" Height="250px"
    BorderStyle="Solid" BorderColor="Silver" BorderWidth="1">
    <%= Html.DataGrid("LibraryAll", "Library")%>
</asp:Panel>
<asp:Panel ID="LibrarySearchContent" runat="server">
    <%= Html.GridSearch("LibraryAll") %>
</asp:Panel>
<asp:Panel ID="LibraryFilterContent" runat="server">
    <%= Html.GridFilter("LibraryAll") %>
</asp:Panel>
