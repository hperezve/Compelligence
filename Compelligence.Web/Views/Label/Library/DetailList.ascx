<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="<%= ViewData["Scope"] %>LibraryDetailDataListContent">
    <asp:Panel ID="LibraryDetailToolbarContent" runat="server" CssClass="buttonLink">
        <input type="button" value="Create" onclick="javascript: newEntity('<%= Url.Action("Create", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryDetail', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />
        <input type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryDetail', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>')" />
        <input type="button" value="Delete" onclick="javascript: deleteEntity('<%= Url.Action("Delete", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryDetail', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />
        <input type="button" value="Duplicate" onclick="javascript: duplicateEntity('<%= Url.Action("Duplicate", "Library") %>', '<%= ViewData["Scope"] %>', 'Library', 'LibraryDetail', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />
    </asp:Panel>
    <asp:Panel ID="LibraryDetailDataListContent" runat="server" ScrollBars="Auto" Height="250px"
        BorderStyle="Solid" BorderColor="Silver" BorderWidth="1">
         <%= Html.DataGrid("LibraryDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
    </asp:Panel>
</div>
<asp:Panel ID="LibraryDetailFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryEditFormContent" />
</asp:Panel>
