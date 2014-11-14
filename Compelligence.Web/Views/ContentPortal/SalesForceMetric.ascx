<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<!--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"
    type="text/css" />-->
<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet"
    type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/FrontEndSite.css") %>" rel="stylesheet"
    type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/rating.css") %>" rel="stylesheet" type="text/css" />
<style type="text/css">
    dselect
    {
        color: #000000;
        font-weight: bold;
        font-size: 10px;
        cursor: hand;
        border-left-width: thin;
        border-left-color: #000000;
        border-top-width: thin;
        border-top-color: #000000;
        width: 130px;
        _width: 130px;
        background-color: #EEEEEE;
        font-family: Arial;
        font-size: 1.2em;
        text-align: center;
        color: #666666;
        letter-spacing: 2px;
    }
    body
    {
        background: none;
    }
</style>
<!--
<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>
-->
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1/jquery-ui.min.js"></script>

<script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript">    </script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Feedback.js") %>" type="text/javascript">  </script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

<%--<script type="text/javascript">

    function ChangeSFAction(url) {
        //showLoadingDialog();
       // alert($('#FrontEndForm').size());
        $('#FrontEndForm').attr("action", url);
        $('#FrontEndForm').submit();
    }

</script>--%>

<script type="text/javascript">
    function hideSelectFilterValues() {
        var contador = 0;
        $('#typeColumn option').each(function() {
            alert('T.T');
            if ($('#typeColumn option:eq(' + contador + ')').val() != "" && $('#typeColumn option:eq(' + contador + ')').val() != "Date") {
                var selectValuesFilter = document.getElementsByName($('#typeColumn option:eq(' + contador + ')').val());
                for (d = 0; d < selectValuesFilter.length; d++) {
                    var name = $('#typeColumn option:eq(' + contador + ')').val();
                    name = '#' + name + d;
                    alert(name);
                    $(name).hide();
                }
            }
            else {
                alert("esta vacio o es igual a date:" + $('#typeColumn option:eq(' + contador + ')').val());
            }
            contador++;
        })

    };
    var cont = 0;
    hideSelectFilterValues();

    $(function() {
        $('#typeColumn').hide();
        $('#typeColumn option').each(function() {
            if ($('#typeColumn option:eq(' + cont + ')').val() != "" && ($('#typeColumn option:eq(' + cont + ')').val() != "Date")) {
                var selectValuesFilter = document.getElementsByName($('#typeColumn option:eq(' + cont + ')').val());
                for (d = 0; d < selectValuesFilter.length; d++) {
                    var name = $('#typeColumn option:eq(' + cont + ')').val();
                    name = '#' + name + d;
                    $(name).hide();
                }
            }

            if ($('#typeColumn option:eq(' + cont + ')').val() != "") {
                if ($('#typeColumn option:eq(' + cont + ')').val() == 'Date') {
                    $('#filterValue' + cont).datepicker();
                    $('#filterOperator' + cont).empty();
                    $('#filterOperator' + cont).append("<option value='Eq'>equal</option>");

                    $('#filterOperator' + cont).append("<option value='Ne'>not equal</option>");

                    $('#filterOperator' + cont).append("<option value='Lt'>less</option>");

                    $('#filterOperator' + cont).append("<option value='Le'>less or equal</option>");

                    $('#filterOperator' + cont).append("<option value='Gt'>greater</option>");

                    $('#filterOperator' + cont).append("<option value='Ge'>greater or equal</option>");
                } else {
                    var nameSelect = $('#typeColumn option:eq(' + cont + ')').val();
                    nameSelect = nameSelect + cont;
                    var pos = $('#filterValue' + cont).position();
                    nameSelect = "#" + nameSelect + "";
                    $(nameSelect).show();
                    $(nameSelect).css({ position: "absolute",

                        marginLeft: 0, marginTop: 0,

                        top: pos.top, left: pos.left
                    });

                    $('#filterOperator' + cont).empty();
                    $('#filterOperator' + cont).append("<option value='Eq'>equal</option>");

                    $('#filterOperator' + cont).append("<option value='Ne'>not equal</option>");

                    $('#filterOperator' + cont).append("<option value='Lt'>less</option>");

                    $('#filterOperator' + cont).append("<option value='Le'>less or equal</option>");

                    $('#filterOperator' + cont).append("<option value='Gt'>greater</option>");

                    $('#filterOperator' + cont).append("<option value='Ge'>greater or equal</option>");
                }
            }
            cont++;
        }
           )

    });

    var fillFilterOperator = function(type, i) {
        $('#filterOperator' + i).empty();

        if (type == "Other") {
            $('#filterOperator' + i).append("<option value='Bw'>begins with</option>");
        }

        $('#filterOperator' + i).append("<option value='Eq'>equal</option>");

        $('#filterOperator' + i).append("<option value='Ne'>not equal</option>");

        $('#filterOperator' + i).append("<option value='Lt'>less</option>");

        $('#filterOperator' + i).append("<option value='Le'>less or equal</option>");

        $('#filterOperator' + i).append("<option value='Gt'>greater</option>");

        $('#filterOperator' + i).append("<option value='Ge'>greater or equal</option>");

        if (type == "Other") {
            $('#filterOperator' + i).append("<option value='Ew'>ends with</option>");
        }
        if (type == "Other") {
            $('#filterOperator' + i).append("<option value='Cn'>contains</option>");
        }
    };

    var changeFilterValue = function(name, k) {
        controlName = name + k;
        controlName = "#" + controlName;
        var valueInput = $(controlName).prop("value");
        $('#filterValue' + k).val(valueInput);
    };

    var changeTxtBox = function(i) {
        var typeColumn = $("#typeColumn");
        var selected = $("#filterColumn" + i + " option:selected");
        $('#typeColumn')[0].selectedIndex = $('#filterColumn' + i)[0].selectedIndex;
        if ($("#typeColumn").prop("value") == "") {
            $('#filterValue' + i).val("");
            if ($('#filterValue' + i).prop("class") == "hasDatepicker") {
                $('#filterValue' + i).datepicker('destroy');
            }
            hideSelect(i);
            fillFilterOperator("Other", i);
        } else {
        if ($("#typeColumn").prop("value") == "Date") {
                //                   alert("Aqui hay un Date xD");
                $('#filterValue' + i).val("");
                hideSelect(i);
                $('#filterValue' + i).datepicker();
                fillFilterOperator("Date", i);
            } else {
                $('#filterValue' + i).val("");
                if ($('#filterValue' + i).prop("class") == "hasDatepicker") {
                    $('#filterValue' + i).datepicker('destroy');
                }
                hideSelect(i);
                fillFilterOperator("Standard", i);
                var indice = document.getElementById('typeColumn').selectedIndex;
                var nameSelect = document.getElementById('typeColumn').options[indice].value;
                nameSelect = nameSelect + i;
                var pos = $('#filterValue' + i).position();
                nameSelect = "#" + nameSelect + "";
                $(nameSelect).css({ position: "absolute",

                    marginLeft: 0, marginTop: 0,

                    top: pos.top, left: pos.left
                });
                $(nameSelect).show();
            }
        }
    };


    var hideSelect = function(i) {
        var count = 0;
        $('#typeColumn option').each(function() {
            if ($('#typeColumn option:eq(' + count + ')').val() != "" || $('#typeColumn option:eq(' + count + ')').val() == "Date") {
                var nameSelectChoosed = $('#typeColumn option:eq(' + count + ')').val();
                nameSelectChoosed = "#" + nameSelectChoosed + i;
                $(nameSelectChoosed).hide();
            }
            count++;
        })
    };

    var executeFilterReport = function(formId) {
        var filterCriteria = '';
        var filterColumnFields = $(formId + ' select[name=filterColumn]');
        var filterOperatorFields = $(formId + ' select[name=filterOperator]');
        var filterValueFields = $(formId + ' input[name=filterValue]');

        for (var i = 0; i < filterColumnFields.length; i++) {
            if (i > 0) {
                filterCriteria += ':';
            }

            filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + filterValueFields[i].value);
        }

        return filterCriteria;
    };

    var executeHiddenColumnReport = function(formId) {
        var hiddenCriteria = '';
        var filterColumnFields = $(formId + ' select[name=filterColumn]');
        var filterOperatorFields = $(formId + ' select[name=filterOperator]');
        var filterValueFields = $(formId + ' input[name=filterValue]');
        var filterHiddenColummns = $(formId + ' input[name=check]');
        for (var i = 0; i < filterColumnFields.length; i++) {
            if (i > 0) {
                hiddenCriteria += ':';
            }

            hiddenCriteria += (filterColumnFields[i].value + '_isVisible_' + filterHiddenColummns[i].checked);
        }

        return hiddenCriteria;
    };

    var executeReport = function(scope, browseId, title, formId) {
        var urlAction = '<%= Url.Action("GenerateReport", "ContentPortal") %>';

        //showLoadingDialogForSection('#ReportsModuleContent');
        $.post(urlAction, { Scope: scope, BrowseId: browseId, Title: title, FilterCriteria: executeFilterReport(formId), HiddenColumnCriteria: executeHiddenColumnReport(formId) },
            function(data) {
                var urlReportsOut = '<%= Url.Content("~" + ConfigurationSettings.AppSettings["ReportFilePath"]) %>';
                window.open(urlReportsOut + data + '.pdf', "ReportPopup", "width=700,height=400");
                hideLoadingDialogForSection('#ReportsModuleContent');
            });
    };
    
</script>

<script type="text/javascript">
    //    $(function() {
    //        var urlAction = '<%= Url.Action("List", "Report") %>';
    //        var parameters = { ReportModule: '<%= ViewData["ReportModule"]%>' };
    //        $.get(urlAction, parameters, function(data) {
    //            $("#ReportsModuleContent").html(data);
    //        });
    //    });

    //    var loadListReport = function(reportModule) {
    //        var urlAction = '<%= Url.Action("List", "Report") %>';
    //        var parameters = { ReportModule: reportModule };

    //        showLoadingDialogForSection('#ReportsModuleContent');

    //        $.get(urlAction, parameters, function(data) {
    //            $("#ReportsModuleContent").html(data);
    //            hideLoadingDialogForSection('#ReportsModuleContent');
    //        });
    //    };

    //    var loadFilterReport = function(reportFilter, reportTitle, reportModule) {
    //        var urlAction = '<%= Url.Action("Filter", "Report") %>';
    //        var parameters = { ReportFilter: reportFilter, ReportTitle: reportTitle, ReportModule: reportModule };

    //        showLoadingDialogForSection('#ReportsModuleContent');

    //        $.get(urlAction, parameters, function(data) {
    //            $("#ReportsModuleContent").html(data);
    //            hideLoadingDialogForSection('#ReportsModuleContent');
    //        });
    //    };

    
</script>
<div>
        <%= Html.GridFilterReport("MetricsByProject", "Metrics by Project Report")%>
    </div>
    <div>
    <%= Html.CreateSelectStandardData("MetricsByProject")%>
</div>
<div id="ReportsModuleContent">
</div>
<div id="MetricsContent">
    



</div>