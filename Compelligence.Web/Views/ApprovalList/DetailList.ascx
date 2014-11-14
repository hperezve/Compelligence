<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var ReSendMail = function(projectid, userid) {
        var urlAction = '<%= Url.Action("ReSendEmailToUser", "ApprovalList") %>';

        $.get(urlAction + "?ProjectId=" + projectid + "&UserIdToSend=" + userid);
        showReSendEmailToApprovedDialog();
    };
</script>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'ApprovalListDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
        <div id="<%= ViewData["Scope"] %>ApprovalListDetailDataListContent">
        <asp:Panel ID="ApprovalListDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%--<input class="button" type="button" value="Add" onclick="javascript: addEntity('<%= Url.Action("CreateDetail", "ApprovalList") %>', '<%= ViewData["Scope"] %>', 'ApprovalList', 'ApprovalListDetail', 'ApprovalListDetailSelect', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />
        <input class="button" type="button" value="Delete" onclick="javascript: deleteDetailEntity('<%= Url.Action("DeleteDetail", "ApprovalList") %>', '<%= ViewData["Scope"] %>', 'ApprovalList', 'ApprovalListDetail', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />--%>
        </asp:Panel>
        <asp:Panel ID="ApprovalListDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("ApprovalListDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="ApprovalListDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ApprovalListEditFormContent" />
    </asp:Panel>
</div>