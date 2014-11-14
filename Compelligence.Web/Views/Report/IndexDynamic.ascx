<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity" %>

<script type="text/javascript">
    $(function() {
        var urlAction = '<%= Url.Action("ListDynamic", "Report") %>';
        var parameters = {ReportModule: '<%= ViewData["ReportModule"]%>'};
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
        });
    });

    var loadListDynamicReport = function(reportModule) {
        var urlAction = '<%= Url.Action("ListDynamic", "Report") %>';
        var parameters = { ReportModule: reportModule };

        showLoadingDialogForSection('#ReportsModuleContent');

        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
            hideLoadingDialogForSection('#ReportsModuleContent');
        });
    };

    var loadFilterReportDynamic = function(reportTitle, reportId, reportModule, entityRow, entityColumn) {
        var urlAction = '<%= Url.Action("FilterDynamic", "Report") %>';
        var parameters = { ReportTitle: reportTitle, ReportId :reportId, ReportModule: reportModule, EntityRow: entityRow, EntityColumn: entityColumn };
        showLoadingDialogForSection('#ReportsModuleContent');
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
            hideLoadingDialogForSection('#ReportsModuleContent');
        });
    };

    var loadFilterDynamicFinancial = function(reportTitle, reportId, reportModule, entityColumn) {
    var urlAction = '<%= Url.Action("FilterDynamicFinancial", "Report") %>';
        var parameters = { ReportTitle: reportTitle, ReportId: reportId, ReportModule: reportModule, EntityColumn: entityColumn };
        showLoadingDialogForSection('#ReportsModuleContent');
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
            hideLoadingDialogForSection('#ReportsModuleContent');
        });
    };

    var executeReportDynamic = function(value, reportId,itemsA, itemsB, filterA, filterB) {
    if (itemsA != '' && itemsB != '') {
            browsePopup = window.open('<%= Url.Action("DynICReport", "Report") %>?Title=' + value + '&ReportId=' + reportId + '&ItemsA=' + itemsA + '&ItemsB=' + itemsB + '&FilterValueA=' + filterA + '&FilterValueB=' + filterB, "ReportViewerPopup", "width=800,height=730");
            if (window.focus) {
                browsePopup.focus();
                hideLoadingDialogForSection('#ReportsModuleContent');
            }
        }
    };

    var executeReportDynamicFinancial = function(value, reportId, items, filter, financial, periodType, periodValue, year) {
        if (items != '') {
            browsePopup = window.open('<%= Url.Action("DynICReportFinancial", "Report") %>?Title=' + value + '&ReportId=' + reportId + '&Items=' + items + '&FilterValue=' + filter + '&financial=' + financial + '&periodType=' + periodType + '&periodValue=' + periodValue + '&year=' + year, "ReportViewerPopup", "width=800,height=730");
            if (window.focus) {
                browsePopup.focus();
                hideLoadingDialogForSection('#ReportsModuleContent');
            }
        }
    };
</script>

<div id="ReportsModuleContent">
</div>
