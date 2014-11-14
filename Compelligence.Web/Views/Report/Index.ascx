<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity" %>

<script type="text/javascript">
    $(function() {
        var urlAction = '<%= Url.Action("List", "Report") %>';
        var parameters = {ReportModule: '<%= ViewData["ReportModule"]%>'};
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
        });
    });

    var loadListReport = function(reportModule) {
        var urlAction = '<%= Url.Action("List", "Report") %>';
        var parameters = { ReportModule: reportModule };

        showLoadingDialogForSection('#ReportsModuleContent');
        
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
            hideLoadingDialogForSection('#ReportsModuleContent');
        });
    };

    var loadFilterReport = function(reportFilter, reportTitle, reportModule) {
        var urlAction = '<%= Url.Action("Filter", "Report") %>';
        var parameters = { ReportFilter: reportFilter, ReportTitle: reportTitle, ReportModule: reportModule };

        showLoadingDialogForSection('#ReportsModuleContent');
        
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
            hideLoadingDialogForSection('#ReportsModuleContent');
        });
    };

//    var executeReport = function(scope, browseId, title, formId) {
//        var urlAction = '<%= Url.Action("Generate", "Report") %>';

//        showLoadingDialogForSection('#ReportsModuleContent');

//        $.post(urlAction, { Scope: scope, BrowseId: browseId, Title: title, FilterCriteria: executeFilterReport(formId), HiddenColumnCriteria: executeHiddenColumnReport(formId)},
//            function(data)
//            {
//                var urlReportsOut = '<%= Url.Content("~" + ConfigurationSettings.AppSettings["ReportFilePath"]) %>';
//                window.open(urlReportsOut + data + '.pdf', "ReportPopup", "width=700,height=400");
//                hideLoadingDialogForSection('#ReportsModuleContent');
//            });
//       };

    var executeReport = function(scope, browseId, title, formId) {
        browsePopup = window.open('<%= Url.Action("ReportViewer", "Report") %>' + '?Scope=' + scope + '&BrowseId=' + browseId + '&Title=' + title + '&FilterCriteria=' + executeFilterReport(formId) + '&HiddenColumnCriteria=' + executeHiddenColumnReport(formId), "ReportViewerPopup", "width=800,height=730");
        if (window.focus) {
            browsePopup.focus();
            hideLoadingDialogForSection('#ReportsModuleContent');
        }
    };

    var executeReportWithSpecialData = function(scope, browseId, title, formId, properties) {
    browsePopup = window.open('<%= Url.Action("ReportViewer", "Report") %>' + '?Scope=' + scope + '&BrowseId=' + browseId + '&Title=' + title + '&FilterCriteria=' + executeSpecialFilterReport(formId, properties) + '&HiddenColumnCriteria=' + executeHiddenColumnReport(formId), "ReportViewerPopup", "width=770,height=500");
        if (window.focus) {
            browsePopup.focus();
            hideLoadingDialogForSection('#ReportsModuleContent');
        }
    };
    var executeReportForm2 = function(scope, browseId, title, formId, properties) {
    browsePopup = window.open('<%= Url.Action("ReportViewer", "Report") %>' + '?Scope=' + scope + '&BrowseId=' + browseId + '&Title=' + title + '&FilterCriteria=' + executeFilterReportForm2(formId, properties) + '&HiddenColumnCriteria=' + executeHiddenColumnReport(formId), "ReportViewerPopup", "width=770,height=500");
        if (window.focus) {
            browsePopup.focus();
            hideLoadingDialogForSection('#ReportsModuleContent');
        }
    };
//    var executeReportWithSpecialData = function(scope, browseId, title, formId, properties) {
//    //var urlAction = '<%= Url.Action("ReportViewer", "Report") %>';

//        showLoadingDialogForSection('#ReportsModuleContent');

//        $.post(urlAction, { Scope: scope, BrowseId: browseId, Title: title, FilterCriteria: executeSpecialFilterReport(formId, properties), HiddenColumnCriteria: executeHiddenColumnReport(formId) },
//            function(data) {
//                var urlReportsOut = '<%= Url.Content("~" + ConfigurationSettings.AppSettings["ReportFilePath"]) %>';
//                window.open(urlReportsOut + data + '.pdf', "ReportPopup", "width=700,height=400");
//                hideLoadingDialogForSection('#ReportsModuleContent');
//            });
//    };
</script>

<div id="ReportsModuleContent">
</div>
