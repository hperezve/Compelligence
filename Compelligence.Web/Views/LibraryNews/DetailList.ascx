<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    var SendEmail = function() {
        var urlAction = '<%= Url.Action("GetParametersOfEmail", "LibraryNews") %>';
        var xmlhttp;
        var result = null;
        var scope = '<%= ViewData["Scope"] %>';
        var id = $('#' + scope + 'LibraryNewsDetailListTable').getGridParam('selrow');
        if (id) {
            $.get(
            urlAction + "?LibraryId=" + id,
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    var posSubject = result.indexOf('LibraryNewsEmailSubject:');
                    var posBody = result.indexOf('LibraryNewsEmailBody:');
                    var subject = result.substring(0, posBody);
                    var bodyEmail = result.substring(posBody);
                    var posValueSubject = subject.indexOf(':');
                    var posValueBody = bodyEmail.indexOf(':');

                    var valueSubject = subject.substring(posValueSubject + 1);
                    var valueBody = bodyEmail.substring(posValueBody + 1);
                    //window.open('mailto:?Subject=' + valueSubject + '&body=type+your&body=' + valueBody, '', '200', '200');
                    location.href = 'mailto:?Subject=' + valueSubject + '&body=' + valueBody;
                }
            });            
            //            showReSendEmailToApprovedDialog();
            //openPopup(urlAction + "?LibraryId=" + id);


        }
        else {
            showAlertSelectItemDialog();
        }
    };
</script>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'LibraryNewsDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>LibraryNewsDetailDataListContent" class="absolute">
        <asp:Panel ID="LibraryNewsDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "LibraryNews") + "', '" + ViewData["Scope"] + "', 'LibraryNews', 'LibraryNewsDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "LibraryNews") + "', '" + ViewData["Scope"] + "', 'Library', 'EntityNewsDetail', 'LibraryDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "LibraryNews") + "', '" + ViewData["Scope"] + "', 'LibraryNews', 'LibraryNewsDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "LibraryNews") + "', '" + ViewData["Scope"] + "', 'LibraryNews', 'LibraryNewsDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "Library") + "', '" + ViewData["Scope"] + "', 'Library', 'LibraryDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <input class="button" type="button" value="Send" onclick="javascript:SendEmail()" />
            <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'LibraryNews', 'LibraryNewsDetail');" />
        </asp:Panel>
        <asp:Panel ID="LibraryNewsDetailDataListContent" runat="server"  CssClass="contentDetailData">
            <%= Html.DataGrid("LibraryNewsDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="LibraryNewsDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>LibraryNewsEditFormContent" />
    </asp:Panel>
    <asp:Panel ID="LibraryNewsDetailFilterContent" runat="server">
        <%= Html.GridFilterAlter("LibraryNewsDetail", true)%>
    </asp:Panel>
</div>
