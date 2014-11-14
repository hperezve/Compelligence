<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<style type="text/css">
    .ui-jqgrid tr.jqgrow td{
        white-space: pre-wrap;
        word-wrap: break-word;
    }
</style>
<div class="indexTwo">
        <div id="<%= ViewData["Scope"] %>ActionHistoryDetailDataListContent">
        <asp:Panel ID="HistoryFieldUserProfileToolbarContent" runat="server" CssClass="buttonLink">

        </asp:Panel>
        <asp:Panel ID="HistoryFieldUserProfileDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("UserAccountHistory", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="HistoryFieldUserProfileFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>HistoryFieldUserProfileEditFormContent" />
    </asp:Panel>
</div>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'HistoryFieldUserProfile');
    }).trigger('resize');
</script>