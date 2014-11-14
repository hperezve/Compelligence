<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="indexTwo">
        <div id="<%= ViewData["Scope"] %>ActionHistoryDetailDataListContent">
        <asp:Panel ID="ActionHistoryDetailToolbarContent" runat="server" CssClass="buttonLink">

        </asp:Panel>
        <asp:Panel ID="ActionHistoryDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("ActionHistoryDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
            
        </asp:Panel>
    </div>
    <asp:Panel ID="ActionHistoryDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ActionHistoryEditFormContent" />
    </asp:Panel>
</div>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'ActionHistoryDetail');
    }).trigger('resize');
</script>