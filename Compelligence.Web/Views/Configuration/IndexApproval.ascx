<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
    var urlAction = '<%= Url.Action("ApprovalProject","Configuration") %>';
       
       $.get(urlAction,null, function(data){$("#ConfigurationModuleContent").html(data);});
    });
//    $(function() {
//        var urlAction = '<%= Url.Action("List", "Report") %>';
//        var parameters = { ReportModule: '<%= ViewData["ReportModule"]%>' };
//        $.get(urlAction, parameters, function(data) {
//            $("#ReportsModuleContent").html(data);
//        });
//    });
</script>

<div id="ConfigurationModuleContent">

</div>

