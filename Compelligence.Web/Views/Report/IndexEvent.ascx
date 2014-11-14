<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity" %>

<script type="text/javascript">
    $(function() {
        var urlAction = '<%= Url.Action("ListEvent", "Report") %>';
        var parameters = {ReportModule: '<%= ViewData["ReportModule"]%>'};
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
        });
    });

    var loadListReport = function(reportModule) {
        var urlAction = '<%= Url.Action("ListEvent", "Report") %>';
        var parameters = { ReportModule: reportModule };

        showLoadingDialogForSection('#ReportsModuleContent');
        
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
            hideLoadingDialogForSection('#ReportsModuleContent');
        });
    };

    var loadFilterReport = function(reportFilter, reportTitle, reportModule) {
        var urlAction = '<%= Url.Action("FilterEvent", "Report") %>';
        var parameters = { ReportFilter: reportFilter, ReportTitle: reportTitle, ReportModule: reportModule };

        showLoadingDialogForSection('#ReportsModuleContent');
        
        $.get(urlAction, parameters, function(data) {
            $("#ReportsModuleContent").html(data);
            hideLoadingDialogForSection('#ReportsModuleContent');
        });
    };

//    var executeReportToEvents = function(scope, browseId, title, formId, properties, propertiesHidden) {
//        var urlAction = '<%= Url.Action("GenerateEvent", "Report") %>';

//        showLoadingDialogForSection('#ReportsModuleContent');

//        $.post(urlAction, { Scope: scope, BrowseId: browseId, Title: title, FilterCriteria: executeEventFilterReport(formId, properties, propertiesHidden), HiddenColumnCriteria: executeHiddenColumnReport(formId) },
//            function(data)
//            {
//                var urlReportsOut = '<%= Url.Content("~" + ConfigurationSettings.AppSettings["ReportFilePath"]) %>';
//                window.open(urlReportsOut + data + '.pdf', "ReportPopup", "width=700,height=400");
//                hideLoadingDialogForSection('#ReportsModuleContent');
//            });
//        };
    var executeReportToEvents = function(scope, browseId, title, formId, properties, propertiesHidden) {
    browsePopup = window.open('<%= Url.Action("ReportViewer", "Report") %>' + '?Scope=' + scope + '&BrowseId=' + browseId + '&Title=' + title + '&FilterCriteria=' + executeEventFilterReport(formId, properties, propertiesHidden) + '&HiddenColumnCriteria=' + executeHiddenColumnReport(formId), "ReportViewerPopup", "width=800,height=730");
            if (window.focus) {
                browsePopup.focus();
                hideLoadingDialogForSection('#ReportsModuleContent');
            }
        };
</script>

<div id="ReportsModuleContent">
</div>
