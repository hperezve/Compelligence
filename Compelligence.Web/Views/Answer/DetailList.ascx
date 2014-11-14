<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var loadanswerdetail = function(id) {
        var urlAction = '<%= Url.Action("GetAnswerbySurvey", "Survey") %>';
        //        showLoadingDialog();
        openPopup(urlAction + "?QuizResponseId=" + id);
        //      hideLoadingDialog();
    };

    var loadresultdetail = function(id) {
        //  alert('hola mundo ' + id);
        var urlAction = '<%= Url.Action("GetSurvey", "Survey") %>';
        showLoadingDialog();
        window.open(urlAction + "?QuizResponseId=" + id, "AnswerPopup", "width=700,height=400");
        hideLoadingDialog();

    };

    var ShowAnswers = function() {
        var scope = '<%= ViewData["Scope"] %>';
        var id = $('#' + scope + 'AnswerDetailListTable').getGridParam('selrow');
        var selectedItems = $('#' + scope + 'AnswerDetailListTable').getGridParam('selarrrow');
        selectedItems = selectedItems.join(':');
        var IDsSelected = selectedItems.split(":");
        if (IDsSelected.length > 1) {
            var Message = '<span class="ui-icon ui-icon-alert alertSelectItemDialog"></span><p>You must select only one item from the list.</p>';
            
            $('#AlertReturnMessageDialog').html(Message);
            $('#AlertReturnMessageDialog').dialog('option', 'title', 'Alert Message');
            $('#AlertReturnMessageDialog').dialog('open');
        }
        else {
            if (id) {
                loadanswerdetail(id);
            }
            else {
                showAlertSelectItemDialog();
            }
        }
    };

</script>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'AnswerDetail');
    }).trigger('resize');
</script>
<div id="<%= ViewData["Scope"] %>AnswerDetailDataListContent">
    <%--<asp:Panel ID="AnswerDetailToolbarContent" runat="server" CssClass="buttonLink">
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>AnswerDetailDataListContent">
--%>
    <asp:Panel ID="AnswerDetailToolbarContent" runat="server" CssClass="buttonLink">
        <input class="button" type="button" value="Export Selected" onclick="javascript: ExportBySelected('<%=Url.Action("Export","Survey")%>','<%= ViewData["Scope"]%>', 'AnswerDetail');" />
        <input class="button" type="button" value="Export All" onclick="javascript: Export('<%=Url.Action("ExportAll","Survey")%>','<%= ViewData["Scope"]%>','1');" />
        <input class="button" type="button" value="Show Answers" onclick="javascript: ShowAnswers()" />
        <input class="button" type="button" value="Delete" onclick="javascript: deleteDetailEntity('<%= Url.Action("DeleteByResponseId", "Answer")%>', '<%= ViewData["Scope"] %>', 'Answer', 'AnswerDetail', '<%=ViewData["Container"] %>', '<%= ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');" />
    </asp:Panel>
    <asp:Panel ID="AnswerDetailDataListContent" runat="server" CssClass="contentDetailData">
        <%= Html.DataGrid("AnswerDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
    </asp:Panel>
    <%--</div>
    <asp:Panel ID="AnswerDetailFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SurveyAnswerContent" />
</asp:Panel>--%>
</div>