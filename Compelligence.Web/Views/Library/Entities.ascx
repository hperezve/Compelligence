<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'EntityLibraryAll');
    }).trigger('resize');

    $(function() {
        $('#EnvironmentLibraryShowListOwners').css('height', '636px');
    });
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>LibraryShowListOwners" class="absolute">
        <asp:Panel ID="LibraryDetailDataListContent" runat="server" CssClass="contentDetailData">
            <br />
            <%= Html.DataGrid("EntityLibraryAll", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
</div>



