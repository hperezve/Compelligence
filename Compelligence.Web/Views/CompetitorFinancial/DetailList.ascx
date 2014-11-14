<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<link href="<%= Url.Content("~/Content/Styles/BackEndContentGnral.css") %>" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorFinancialBalanceSheetAnnual');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorFinancialIncomeStatementAnnual');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorFinancialCashFlowAnnual');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorFinancialBalanceSheetQuarterly');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorFinancialIncomeStatementQuarterly');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorFinancialCashFlowQuarterly');
    }).trigger('resize');
</script>
<script type="text/javascript">
    function updateView() {
        var period = $("input[name='PeriodType']:checked").val();
        var financial = $('#financialView').val();

        if (financial == 0) {

            if (period == "A") {
                $('#gridBalanceSheetAnnual').css("display", "none");
                $('#gridIncomeStatementAnnual').css("display", "block");
                $('#gridCashFlowAnnual').css("display", "none");
                $('#gridBalanceSheetQuarterly').css("display", "none");
                $('#gridIncomeStatementQuarterly').css("display", "none");
                $('#gridCashFlowQuarterly').css("display", "none");
            }
            else {
                $('#gridBalanceSheetAnnual').css("display", "none");
                $('#gridIncomeStatementAnnual').css("display", "none");
                $('#gridCashFlowAnnual').css("display", "none");
                $('#gridBalanceSheetQuarterly').css("display", "none");
                $('#gridIncomeStatementQuarterly').css("display", "block");
                $('#gridCashFlowQuarterly').css("display", "none");
            }
        }
        else if (financial == 1) {
            if (period == "A") {
                $('#gridBalanceSheetAnnual').css("display", "block");
                $('#gridIncomeStatementAnnual').css("display", "none");
                $('#gridCashFlowAnnual').css("display", "none");
                $('#gridBalanceSheetQuarterly').css("display", "none");
                $('#gridIncomeStatementQuarterly').css("display", "none");
                $('#gridCashFlowQuarterly').css("display", "none");
            }
            else {
                $('#gridBalanceSheetAnnual').css("display", "none");
                $('#gridIncomeStatementAnnual').css("display", "none");
                $('#gridCashFlowAnnual').css("display", "none");
                $('#gridBalanceSheetQuarterly').css("display", "block");
                $('#gridIncomeStatementQuarterly').css("display", "none");
                $('#gridCashFlowQuarterly').css("display", "none");
            }
        }
        else if (financial == 2) {
            if (period == "A") {
                $('#gridBalanceSheetAnnual').css("display", "none");
                $('#gridIncomeStatementAnnual').css("display", "none");
                $('#gridCashFlowAnnual').css("display", "block");
                $('#gridBalanceSheetQuarterly').css("display", "none");
                $('#gridIncomeStatementQuarterly').css("display", "none");
                $('#gridCashFlowQuarterly').css("display", "none");
            }
            else {
                $('#gridBalanceSheetAnnual').css("display", "none");
                $('#gridIncomeStatementAnnual').css("display", "none");
                $('#gridCashFlowAnnual').css("display", "none");
                $('#gridBalanceSheetQuarterly').css("display", "none");
                $('#gridIncomeStatementQuarterly').css("display", "none");
                $('#gridCashFlowQuarterly').css("display", "block");
            }
        }

    };
    function editFinancial() {
        var period = $("input[name='PeriodType']:checked").val();
        var financial = $('#financialView').val();

        if (financial == 0) {
            if (period == "A") {
                editEntity('<%=Url.Action("EditIS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialIncomeStatementAnnual', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            } else {
                editEntity('<%=Url.Action("EditIS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialIncomeStatementQuarterly', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            }
        } else if (financial == 1) {
            if (period == "A") {
                editEntity('<%=Url.Action("EditBS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialBalanceSheetAnnual', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            } else {
                editEntity('<%=Url.Action("EditBS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialBalanceSheetQuarterly', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            }
        } else if (financial == 2) {
            if (period == "A") {
                editEntity('<%=Url.Action("EditCF", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialCashFlowAnnual', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            } else {
                editEntity('<%=Url.Action("EditCF", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialCashFlowQuarterly', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            }
        }
    };

    function deleteFinancial() {
        var period = $("input[name='PeriodType']:checked").val();
        var financial = $('#financialView').val();

        if (financial == 0) {
            if (period == "A") {
                deleteDetailEntity('<%=Url.Action("DeleteIS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialIncomeStatementAnnual', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            } else {
                deleteDetailEntity('<%=Url.Action("DeleteIS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialIncomeStatementQuarterly', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            }
        } else if (financial == 1) {
            if (period == "A") {
                deleteDetailEntity('<%=Url.Action("DeleteBS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialBalanceSheetAnnual', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            } else {
                deleteDetailEntity('<%=Url.Action("DeleteBS", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialBalanceSheetQuarterly', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            }
        } else if (financial == 2) {
            if (period == "A") {
                deleteDetailEntity('<%=Url.Action("DeleteCF", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialCashFlowAnnual', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            } else {
                deleteDetailEntity('<%=Url.Action("DeleteCF", "CompetitorFinancial")%>', '<%=ViewData["Scope"]%>', 'CompetitorFinancial', 'CompetitorFinancialCashFlowQuarterly', '<%=ViewData["Container"]%>', '<%=ViewData["HeaderType"]%>', '<%=ViewData["DetailFilter"]%>');
            }
        }
    };

    $(function() {
        $("#FinancialErrorMessageDialog").dialog({
            resizable: false,
            autoOpen: false,
            buttons: {
                Ok: function() {
                    $(this).dialog('close');
                }
            }
        });
    });

    function getFinancial() {
        var urlAction = '<%=Url.Action("GetData", "CompetitorFinancial")%>';
        var competitorId = '<%=ViewData["DetailFilter"].ToString()%>';
        var entityName = '';
        competitorId = competitorId.replace("CompetitorFinancial.CompetitorId_Eq_", "");
        var urlActionGetCompetitor = '<%=Url.Action("GetNameCompetitor", "CompetitorFinancial")%>';
        $.get(urlActionGetCompetitor + '/' + competitorId,
            null,
            function(data) {
                entityName = data;
            }
            );
        showLoadingDialog();
        $.get(
            urlAction + '/' + competitorId,
            null,
            function(data) {
                hideLoadingDialog();
                if (data == 0) {
                    
                    if (entityName == null || entityName == undefined || entityName == '') {
                        entityName = 'Competitor';
                    }
                    $('#FinancialErrorMessageDialog').text('Please add a ticker symbol for ' + entityName + ' in header field before using Get Data');
                    $('#FinancialErrorMessageDialog').dialog('open');
                }
                else if (data == 2) {
                    $('#FinancialErrorMessageDialog').text("Ticker symbol is invalid");
                    $('#FinancialErrorMessageDialog').dialog('open');
                }
                reloadG();
            }
        );
//        var target = '#' + competitorId + 'EditFormContent';
//        $.get('<%=Url.Action("DetailList", "CompetitorFinancial")%>/' + competitorId,
//            null,
//            function(data) {
//                $("#EnvironmentCompetitorCompetitorFinancialEditFormContent").html(data);
//            }
            //        );
            
        };
        var reloadG = function() {
            reloadGrid('#<%= ViewData["Scope"]%>' + 'CompetitorFinancialBalanceSheetAnnualListTable');
            reloadGrid('#<%= ViewData["Scope"]%>' + 'CompetitorFinancialIncomeStatementAnnualListTable');
            reloadGrid('#<%= ViewData["Scope"]%>' + 'CompetitorFinancialCashFlowAnnualListTable');
            reloadGrid('#<%= ViewData["Scope"]%>' + 'CompetitorFinancialBalanceSheetQuarterlyListTable');
            reloadGrid('#<%= ViewData["Scope"]%>' + 'CompetitorFinancialIncomeStatementQuarterlyListTable');
            reloadGrid('#<%= ViewData["Scope"]%>' + 'CompetitorFinancialCashFlowQuarterlyListTable');
        }
</script>
<div id="FinancialErrorMessageDialog" style="display:block;" title="Failed to get data"></div>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>CompetitorFinancialDetailDataListContent" class="absolute">
        <asp:Panel ID="CompetitorFinancialDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.GetData, new { onClick = "javascript: getFinancial();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "CompetitorFinancial") + "', '" + ViewData["Scope"] + "', 'CompetitorFinancial', 'CompetitorFinancialDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editFinancial();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteFinancial();" })%>
            <!-- <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("Duplicate", "CompetitorFinancial") + "', '" + ViewData["Scope"] + "', 'CompetitorFinancial', 'CompetitorFinancialDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%> -->
   
        <label for="FilterFinancialView" class="textFontSize13em">
            <asp:Literal ID="FilterFinancialView" runat="server" Text="Financial" />:
        </label>
        <select id="financialView">
                <option value="0" onclick="javascript: updateView();">Income Statement</option>  
                <option value="1" onclick="javascript: updateView();">Balance Sheet</option>  
                <option value="2" onclick="javascript: updateView();">Cash Flow</option>           
        </select>
        <label for="FilterFinancialView" class="textFontSize13em">
		    <input id="pAnnual" name="PeriodType" type="radio" value="A" checked onclick="javascript: updateView();" class="radioBoxAling"/>
		    <asp:Literal ID="Annual" runat="server" Text="Annual" />
		    <input id="pQuarterly" name="PeriodType" type="radio" value="Q" onclick="javascript: updateView();" class="radioBoxAling"/>
		    <asp:Literal ID="Quarterly" runat="server" Text="Quarterly" />
         </label>     
        </asp:Panel>
        <asp:Panel ID="CompetitorFinancialDetailDataListContent" runat="server" CssClass="contentDetailData">
            <div id="gridBalanceSheetAnnual" class="gridOverflow" style="display: none"><%= Html.DataGrid("CompetitorFinancialBalanceSheetAnnual", new { BrowseDetailFilter = ViewData["DetailFilter"].ToString().Insert(19, "BalanceSheetDetailView") })%></div>
            <div id="gridBalanceSheetQuarterly" class="gridOverflow" style="display: none"><%= Html.DataGrid("CompetitorFinancialBalanceSheetQuarterly", new { BrowseDetailFilter = ViewData["DetailFilter"].ToString().Insert(19, "BalanceSheetDetailView") })%></div>
            <div id="gridIncomeStatementAnnual" class="gridOverflow" style="display: block"><%= Html.DataGrid("CompetitorFinancialIncomeStatementAnnual", new { BrowseDetailFilter = ViewData["DetailFilter"].ToString().Insert(19, "IncomeStatementDetailView") })%></div>
            <div id="gridIncomeStatementQuarterly" class="gridOverflow" style="display: none"><%= Html.DataGrid("CompetitorFinancialIncomeStatementQuarterly", new { BrowseDetailFilter = ViewData["DetailFilter"].ToString().Insert(19, "IncomeStatementDetailView") })%></div>
            <div id="gridCashFlowAnnual" class="gridOverflow" style="display: none"><%= Html.DataGrid("CompetitorFinancialCashFlowAnnual", new { BrowseDetailFilter = ViewData["DetailFilter"].ToString().Insert(19, "CashFlowDetailView") })%></div>
            <div id="gridCashFlowQuarterly" class="gridOverflow" style="display: none"><%= Html.DataGrid("CompetitorFinancialCashFlowQuarterly", new { BrowseDetailFilter = ViewData["DetailFilter"].ToString().Insert(19, "CashFlowDetailView") })%></div>
        </asp:Panel>
    </div>
    <asp:Panel ID="CompetitorFinancialDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>CompetitorFinancialEditFormContent" />
    </asp:Panel>
</div>