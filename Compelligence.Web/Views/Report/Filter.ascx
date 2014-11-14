<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity" %>

   <script type="text/javascript">
       var fillFilterOperatorStandard = function(i) {
           $('#filterOperator' + i).empty();
           $('#filterOperator' + i).append("<option value='Eq'>equal</option>");
           $('#filterOperator' + i).append("<option value='Ne'>not equal</option>");
       };


       var fillFilterOperatorDate = function(i) {
           $('#filterOperator' + i).empty();
           $('#filterOperator' + i).empty();
           $('#filterOperator' + i).append("<option value='Eq'>equal</option>");
           $('#filterOperator' + i).append("<option value='Ne'>not equal</option>");
           $('#filterOperator' + i).append("<option value='Lt'>less</option>");
           $('#filterOperator' + i).append("<option value='Le'>less or equal</option>");
           $('#filterOperator' + i).append("<option value='Gt'>greater</option>");
           $('#filterOperator' + i).append("<option value='Ge'>greater or equal</option>");
       };

       var fillFilterOperatorMultiStandard = function(i) {
           $('#filterOperator' + i).empty();
           $('#filterOperator' + i).append("<option value='Cn'>contains</option>");
           $('#filterOperator' + i).append("<option value='Nc'>not contains</option>");
       };

       var fillFilterOperatorOther = function(i) {
           $('#filterOperator' + i).empty();
           $('#filterOperator' + i).append("<option value='Bw'>begins with</option>");
           $('#filterOperator' + i).append("<option value='Eq'>equal</option>");
           $('#filterOperator' + i).append("<option value='Ne'>not equal</option>");
           $('#filterOperator' + i).append("<option value='Lt'>less</option>");
           $('#filterOperator' + i).append("<option value='Le'>less or equal</option>");
           $('#filterOperator' + i).append("<option value='Gt'>greater</option>");
           $('#filterOperator' + i).append("<option value='Ge'>greater or equal</option>");
           $('#filterOperator' + i).append("<option value='Ew'>ends with</option>");
           $('#filterOperator' + i).append("<option value='Cn'>contains</option>");
       };
       
       var fillFilterOperatorToDateRange = function(type, i) {
           $('#filterOperator' + i).empty();

           if (type == "Date") {
               $('#filterOperator' + i).append("<option value='Bt'>between</option>");
           }
       };

       var HidenOtherFilterValues = function(newFilterValueId, j, properties) {
           var columns = properties.toString().split(':');
           for (var i = 0; i < columns.length; i++) {
               var column = columns[i].split('_');
               if (column[1].toString() != newFilterValueId) {
                   $("#" + column[1] + j).css("display", "none");
               }
           }
       };
       var HidenOtherOperatorFilterValues = function(key, j) {
           if (key == 'Single') {
               $("#SinglefilterOperator" + j).css("display", "inline");
               $("#DatefilterOperator" + j).css("display", "none");
               $("#StandardfilterOperator" + j).css("display", "none");
               $("#MultiStandardfilterOperator" + j).css("display", "none");
           }
           else if (key == 'Date') {
               $("#SinglefilterOperator" + j).css("display", "none");
               $("#DatefilterOperator" + j).css("display", "inline");
               $("#StandardfilterOperator" + j).css("display", "none");
               $("#MultiStandardfilterOperator" + j).css("display", "none");
           }
           else if (key == 'StandardData') {
               $("#SinglefilterOperator" + j).css("display", "none");
               $("#DatefilterOperator" + j).css("display", "none");
               $("#StandardfilterOperator" + j).css("display", "inline");
               $("#MultiStandardfilterOperator" + j).css("display", "none");
           }
           else if (key == 'MultiStandard') {
               $("#SinglefilterOperator" + j).css("display", "none");
               $("#DatefilterOperator" + j).css("display", "none");
               $("#StandardfilterOperator" + j).css("display", "none");
               $("#MultiStandardfilterOperator" + j).css("display", "inline");
           }
       }; 
       var changeFilterColumn = function(selected, m, properties) {
           var optionSelected = selected.toString().split('.');
           var newFilterValueId = optionSelected[1];
           var columns = properties.toString().split(':');
           for (var i = 0; i < columns.length; i++) {
               var column = columns[i].split('_');
               if (column[1] == newFilterValueId) {
                   if (column[0] == "Date") {
                       $('#filterValue' + m).val("");
                       $('#filterValue' + m).datepicker();
                       $('#filterSecondValue' + m).css("display", "inline");
                       $('#filterSecondValue' + m).val("");
                       $('#filterSecondValue' + m).datepicker();
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperatorToDateRange("Date", m);
                   }
                   else if (column[0] == "StandardData") {
                       $("#" + newFilterValueId + m).css("display", "inline");
                       $("#filterValue" + m).css("display", "none");
                       $('#filterSecondValue' + m).css("display", "none");
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperator("StandardData", m);
                   }
                   else if (column[0] == "Single") {
                       $("#filterValue" + m).val("");
                       $("#filterSecondValue" + m).val("");
                       $("#filterValue" + m).css("display", "inline");

                       $('#filterSecondValue' + m).css("display", "none");
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperator("Other", m);
                   }
                   i = columns.length;
               }
           }
       };
       var changeFilterColumnReport = function(selected, m, properties) {
           var optionSelected = selected.toString().split('.');
           var newFilterValueId = optionSelected[1];
           var columns = properties.toString().split(':');
           for (var i = 0; i < columns.length; i++) {
               var column = columns[i].split('_');
               if (column[1] == newFilterValueId) {
                   if (column[0] == "Date") {
                       $('#filterValue' + m).val("");
                       $('#filterValue' + m).datepicker();
                       $('#filterValue' + m).css("display", "inline");
                       //                       $('#filterSecondValue' + m).val("");
                       //                       $('#filterSecondValue' + m).datepicker();
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperatorDate(m);
                       HidenOtherOperatorFilterValues("Date", m);
                   }
                   else if (column[0] == "StandardData") {
                       $("#" + newFilterValueId + m).css("display", "inline");
                       $("#filterValue" + m).css("display", "none");
                       //$('#filterSecondValue' + m).css("display", "none");
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperatorStandard(m);
                       HidenOtherOperatorFilterValues("StandardData", m);
                   }
                   else if (column[0] == "MultiStandardData") {
                       $("#" + newFilterValueId + m).css("display", "inline");
                       $("#filterValue" + m).css("display", "none");
                       //$('#filterSecondValue' + m).css("display", "none");
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperatorMultiStandard(m);
                       HidenOtherOperatorFilterValues("MultiStandardData", m);
                   }
                   else if (column[0] == "Single") {
                       $("#filterValue" + m).val("");
                       //$("#filterSecondValue" + m).val("");
                       $("#filterValue" + m).css("display", "inline");

                       // $('#filterSecondValue' + m).css("display", "none");
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperatorOther(m);
                       HidenOtherOperatorFilterValues("Single", m);
                   }
                   i = columns.length;
               }
           }
       };
   </script>
   <script type="text/javascript">

       var cont = 0;
       var tempoPropertie = $('#filterFormHiddenValue').val();
       var tempoColumnType = $('#columnTypeListHiddenValue').val();
       $('#typeColumn').hide();
       if ($('#filterFormHiddenValue').val() == '') {
           hideSelectFilterValues();
           $(function() {
               $('#typeColumn option').each(function() {
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
                           var filterSecondValue = $('#filterSecondValue' + cont);
                           if ((filterSecondValue != undefined) || (filterSecondValue != null)) {
                               $('#filterSecondValue' + cont).css("display", "inline");
                               $('#filterSecondValue' + cont).datepicker();
                               if ($('#filterSecondValue' + cont).css("display") == "inline") {
                                   fillFilterOperatorToDateRange("Date", cont);
                               }
                           }
                       } else {
                           var nameSelect = $('#typeColumn option:eq(' + cont + ')').val();
                           nameSelect = nameSelect + cont;
                           var pos = $('#filterValue' + cont).position();
                           var wid = $('#filterValue' + cont).width();
                           nameSelect = "#" + nameSelect + "";
                           $(nameSelect).show();
                           $(nameSelect).css({ position: "absolute",

                               marginLeft: 0, marginTop: 0,

                               top: pos.top, left: pos.left,
                            
                               width: wid+4

                           });
                           var pos2 = $(nameSelect).position();
                           if (pos2.top == pos.top) {

                           }
                           else {
                               if (pos2.top < pos.top) {
                                   $(nameSelect).css({ position: "absolute",

                                       marginLeft: 0, marginTop: 0,

                                       top: pos.top + (pos.top - pos2.top), left: pos.left

                                   });
                               }
                               else if (pos2.top > pos.top) {
                                   $(nameSelect).css({ position: "absolute",

                                       marginLeft: 0, marginTop: 0,

                                       top: pos.top - (pos2.top - pos.top), left: pos.left

                                   });
                               }

                           }


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
       }
       else {//the other option
           hideSelectFilterOperatorsR(tempoPropertie);
           hideSelectFilterValuesR(tempoColumnType);
           $('#SelectStandardData').css("display", "none");
           $(function() {

               $('#typeColumn option').each(function() {
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
                           var filterSecondValue = $('#filterSecondValue' + cont);
                           if ((filterSecondValue != undefined) || (filterSecondValue != null)) {
                               $('#filterSecondValue' + cont).css("display", "inline");
                               $('#filterSecondValue' + cont).datepicker();
                               if ($('#filterSecondValue' + cont).css("display") == "inline") {
                                   fillFilterOperatorToDateRange("Date", cont);
                               }
                           }
                       } 
                   }
                   cont++;
               }
           )

           });
       }
       

       

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
                   //alert("Aqui hay un Date xD");
                   $('#filterValue' + i).val("");
                   hideSelect(i);
                   $('#filterValue' + i).datepicker();
                   fillFilterOperator("Date", i);
                   var filterSecondValue = $('#filterSecondValue' + i);
                   if ((filterSecondValue != undefined) || (filterSecondValue != null)) {
                       $('#filterSecondValue' + i).css("display", "in-line");
                       $('#filterSecondValue' + i).val("");
                       $('#filterSecondValue' + i).datepicker();
                       fillFilterOperatorToDateRange("Date", i);
                   }

               }
               else {               
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
                   var wid = $('#filterValue' + i).width();
                   nameSelect = "#" + nameSelect + "";
                   $(nameSelect).css({ position: "absolute",

                       marginLeft: 0, marginTop: 0,

                       top: pos.top, left: pos.left,
                       
                       width:wid+4                      
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
                 // alert("esconderemos " + nameSelectChoosed);
                   $(nameSelectChoosed).hide();
               }
               count++;
           })
       };

       function hideSelectFilterValues()
       {
           var count = 0;
           $('#BudgetType0').hide();
           $('#typeColumn option').each(function()
             {
                 if ($('#typeColumn option:eq(' + count + ')').val() != "" && $('#typeColumn option:eq(' + count + ')').val() != "Date")
                   {
                   var selectValuesFilter = document.getElementsByName($('#typeColumn option:eq(' + count + ')').val());
                   for (d = 0; d < selectValuesFilter.length; d++)
                   {
                       var name = $('#typeColumn option:eq(' + count + ')').val();
                       name = '#' + name + d;
                       $(name).hide();
                   }
               }
               count++;
           })
       };
       function hideSelectFilterValuesR(properties) {          
           var propertiesOfBrose = properties.toString().split(':');
           var count = 0;
           $('#typeColumn option').each(function() {
               if ($('#typeColumn option:eq(' + count + ')').val() != "" && $('#typeColumn option:eq(' + count + ')').val() != "Date") {
                   var name = $('#typeColumn option:eq(' + count + ')').val();
                   if (name != '') {
                       for (var i = 0; i < propertiesOfBrose.length; i++) {
                           var propertie = propertiesOfBrose[i].split('_');
                           if (propertie.length == 3) {
                               if (propertie[0] == 'StandardData' || propertie[0] == 'MultiStandardData') {
                                   if (propertie[1] == name) {
                                       $('#filterValue' + count).css('display', 'none');
                                       $('#' + propertie[2] + count).css('display', 'inline');
                                       i = propertiesOfBrose.length;
                                   }
                               }
                           }
                       }
                   }
               }
               count++;
           })
       };

       function hideSelectFilterOperatorsR(properties) {
            var properties3 = '<%= ViewData["ReportFilter"] %>';
            var filterColumnFields = $('#' + properties3 + 'FilterForm select[name= filterColumn]');
            var propertiesOfBrosew2 = properties.toString().split(':');
            for (var i = 0; i < filterColumnFields.length; i++) {
                var columnValue = filterColumnFields[i].value.split('.');
                for (var p = 0; p < propertiesOfBrosew2.length; p++) {
                    var ccc = propertiesOfBrosew2[p].toString().split('_');
                    if (ccc[1] == columnValue[1]) {
                        if (ccc[0] == 'StandardData') {
                            fillFilterOperatorStandard(i);
                        }
                        else if (ccc[0] == 'MultiStandardData') {
                            fillFilterOperatorMultiStandard(i);
                        }
                        else if (ccc[0] == 'Date') {
                            fillFilterOperatorDate(i);
                        }
                        else {
                            fillFilterOperatorOther(i);
                        }
                        p = propertiesOfBrosew2.length;
                    }
                }
            }

        };
        
    </script>
<input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Reports','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Reports %>');" style="float: right;margin-right: 5px;margin-top:5px"/>

<% if ((ViewData["ReportFilter"].ToString().Equals("NewsByCompetitor")) || (ViewData["ReportFilter"].ToString().Equals("NewsByProduct")))
   { %>  <div>  
         <%= Html.GridFilterWithSpecialDateReport(ViewData["ReportFilter"].ToString(), ViewData["ReportTitle"].ToString(), ViewData["ReportModule"].ToString())%>
         </div>
<% }
   else if ((ViewData["ReportFilter"].ToString().Equals("ProjectReport")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyCompetitor")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyContentSatisfaction")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyIndustries")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyKits")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyObjectives")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyProducts")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyStatus")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyTeam")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsbyWebDownload")) || (ViewData["ReportFilter"].ToString().Equals("ProjectsByOwner")) || (ViewData["ReportFilter"].ToString().Equals("DealReport")) || (ViewData["ReportFilter"].ToString().Equals("KitReport")) || (ViewData["ReportFilter"].ToString().Equals("ObjectiveReport")) || (ViewData["ReportFilter"].ToString().Equals("EventReport")))
   { %>
   <div>  
         <%= Html.GridFilterReportForm2(ViewData["ReportFilter"].ToString(), ViewData["ReportTitle"].ToString(), ViewData["ReportModule"].ToString())%>
         </div>
   <% } else { %>
<div>
    <%= Html.GridFilterReport(ViewData["ReportFilter"].ToString(), ViewData["ReportTitle"].ToString(), ViewData["ReportModule"].ToString())%>
</div>
<% } %>
<%--<div class="line" style="float: left;">
    <div class="field">
        <a href="javascript: void(0);" onclick="javascript: loadListReport('<%= ViewData["ReportModule"] %>');">
            Return to List</a>
    </div>
</div>--%>

<div id="SelectStandardData">
    <%= Html.CreateSelectStandardData(ViewData["ReportFilter"].ToString())%>
</div>