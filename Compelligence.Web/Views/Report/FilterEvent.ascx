<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<script type="text/javascript">

       var cont = 0;
       var properties = '<%= ViewData["PropertiesOfBrowseId"] %>';
       hideSelectFilterValues(properties);
       $('#typeColumn').hide();

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

           if (name == 'TimeFrame' && valueInput != '') {
               if (valueInput == 'SE') {
                   $('#contentSelectStartEndDateFilter' + k).css("display", "block");
                   $('#contentSelectSpecificDateFilter' + k).css("display", "block");
                   $('#filterValueEndDate' + k).val("");
                   $('#filterValueEndDate' + k).css("display", "inline");
                   $('#filterValueEndDate' + k).datepicker();
                   $('#filterValueStartDate' + k).val("");
                   $('#filterValueStartDate' + k).css("display", "inline");
                   $('#filterValueStartDate' + k).datepicker();
                   if ($('#contentSelectStartIntervalDateFilter' + k).css("display") == 'block') {
                       $('#contentSelectStartIntervalDateFilter' + k).css("display", "none");
                   }
               }
               else if (valueInput == 'D') {
                   $('#contentSelectSpecificDateFilter' + k).css("display", "block");
                   $('#filterValueStartDate' + k).val("");
                   $('#filterValueStartDate' + k).css("display", "inline");
                   $('#filterValueStartDate' + k).datepicker();
                   if ($('#contentSelectStartEndDateFilter' + k).css("display") == 'block') {
                       $('#contentSelectStartEndDateFilter' + k).css("display", "none");
                   }
                   if ($('#contentSelectStartIntervalDateFilter' + k).css("display") == 'block') {
                       $('#contentSelectStartIntervalDateFilter' + k).css("display", "none");
                   }
               }
               else {
                   $('#contentSelectStartIntervalDateFilter' + k).css("display", "block");
                   if ($('#contentSelectStartEndDateFilter' + k).css("display") == 'block') {
                       $('#contentSelectStartEndDateFilter' + k).css("display", "none");
                   }
                   if ($('#contentSelectSpecificDateFilter' + k).css("display") == 'block') {
                       $('#contentSelectSpecificDateFilter' + k).css("display", "none");
                   }
                   if (valueInput == 'Q') {
                       $('#filterValueStartIntervalDateQ' + k).css("display", "inline");
                       if ($('#filterValueStartIntervalDateM' + k).css("display") == 'inline') {
                           $('#filterValueStartIntervalDateM' + k).css("display", "none");
                       }
                       if ($('#filterValueStartIntervalDateY' + k).css("display") == 'inline') {
                           $('#filterValueStartIntervalDateY' + k).css("display", "none");
                       }
                   }
                   else if (valueInput == 'M') {
                   $('#filterValueStartIntervalDateM' + k).css("display", "inline");
                   if ($('#filterValueStartIntervalDateQ' + k).css("display") == 'inline') {
                       $('#filterValueStartIntervalDateQ' + k).css("display", "none");
                       }
                       if ($('#filterValueStartIntervalDateY' + k).css("display") == 'inline') {
                           $('#filterValueStartIntervalDateY' + k).css("display", "none");
                       }
                   }
                   else if (valueInput == 'Y') {
                   $('#filterValueStartIntervalDateY' + k).css("display", "inline");
                   if ($('#filterValueStartIntervalDateQ' + k).css("display") == 'inline') {
                       $('#filterValueStartIntervalDateQ' + k).css("display", "none");
                       }
                       if ($('#filterValueStartIntervalDateM' + k).css("display") == 'inline') {
                           $('#filterValueStartIntervalDateM' + k).css("display", "none");
                       }
                   }
               }
           }
           else {
               if ($('#contentSelectStartEndDateFilter' + k).css("display") == 'block') {
                   $('#contentSelectStartEndDateFilter' + k).css("display", "none");
               }
               if ($('#contentSelectSpecificDateFilter' + k).css("display") == 'block') {
                   $('#contentSelectSpecificDateFilter' + k).css("display", "none");
               }
               if ($('#contentSelectStartIntervalDateFilter' + k).css("display") == 'block') {
                   $('#contentSelectStartIntervalDateFilter' + k).css("display", "none");
               }
           }
           $('#filterValue' + k).val(valueInput);
           //changeTxtBox(k);
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
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperator("Date", m);
                   }
                   else if (column[0] == "StandardData") {
                       $("#" + newFilterValueId + m).css("display", "inline");
                       $("#filterValue" + m).css("display", "none");
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperator("StandardData", m);
                       //alert(newFilterValueId);
                   }
                   else if (column[0] == "Single") {
                       $("#filterValue" + m).css("display", "inline");
                       //$("#filterValue" + m).val(""); 
                       HidenOtherFilterValues(newFilterValueId, m, properties);
                       fillFilterOperator("Other", m);
                   }
                   i = columns.length;
               }
           }
           if ($('#filterValueStartDate' + m).css("display") == "inline") {
               $('#filterValueStartDate' + m).css("display", "none");
               $('#labelForfilterValueStartDate' + m).css("display", "none");
               $("#filterValue" + m).val("");
               if ($('#filterValueEndDate' + m).css("display") == "inline") {
                   $('#filterValueEndDate' + m).css("display", "none");
                   //$('#labelForfilterEndDateByTimeFrameValue' + m).css("display", "none");
               }
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
           if ($('#contentSelectStartEndDateFilter' + j).css("display") == 'block') {
               $('#contentSelectStartEndDateFilter' + j).css("display", "none");
           }
           if ($('#contentSelectSpecificDateFilter' + j).css("display") == 'block') {
               $('#contentSelectSpecificDateFilter' + j).css("display", "none");
           }
           if ($('#contentSelectStartIntervalDateFilter' + j).css("display") == 'block') {
               $('#contentSelectStartIntervalDateFilter' + j).css("display", "none");
           }
       }
       
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
                   //isTimeFrame(nameSelectChoosed);
                   nameSelectChoosed = "#" + nameSelectChoosed + i;
                   //alert("nameSelectChoosed : " + nameSelectChoosed);
                   //                   alert("esconderemos " + nameSelectChoosed);
                   $(nameSelectChoosed).hide();
               }
               count++;
           })
       };

       var isTimeFrame = function(nameSelectChoosed) {
           if (nameSelectChoosed == "EventTypePeriod") {
               $('#filterValue' + i).css("display", "none");
               $('#filterValueStartDate' + i).val("");
               $('#filterValueStartDate' + i).css("display", "in-line");
               //$('#labelForfilterValueStartDate' + i).css("display", "in-line");
               //$('#labelForfilterValueStartDate' + i).attr("innerHTML", "Srart Date");

               $('#filterValueEndDate' + i).val("");
               $('#filterValueEndDate' + i).css("display", "block");
               //$('#labelForfilterEndDateByTimeFrameValue' + i).css("display", "block");
              // $('#labelForfilterEndDateByTimeFrameValue' + i).attr("innerHTML", "Between End Date");

               $('#filterValueStartDate' + i).datepicker();
               $('#filterValueEndDate' + i).datepicker();
           }
           else {
               $('#filterValueStartDate' + i).css("display", "none");
               //$('#labelForfilterValueStartDate' + i).css("display","none");
               $('#filterValueEndDate' + i).css("display", "none");
               //$('#labelForfilterEndDateByTimeFrameValue' + i).css("display","none");
//               $('#filterValue' + i).val("");
//               if ($('#filterValue' + i).attr("class") == "hasDatepicker") {
//                   $('#filterValue' + i).datepicker('destroy');
//               }
//               if ($('#filterValue' + i).css("display") == "none") {
//                   alert(i + " esta oculto")
//                   $('#filterValue' + i).css("display", "block");
//               }
           }
       };

       function hideSelectFilterValues(properties) {    
           var  propertiesOfBrose = properties.toString().split(',');
           var count = 0;
           $('#typeColumn option').each(function() {
               if ($('#typeColumn option:eq(' + count + ')').val() != "" && $('#typeColumn option:eq(' + count + ')').val() != "Date") {
                   var name = $('#typeColumn option:eq(' + count + ')').val();
                   for (var i = 0; i < propertiesOfBrose.length; i++) {
                       var propertie = propertiesOfBrose[i].split(':');
                       if (propertie[0] == name) {
                           $('#filterValue' + count).css('display', 'none');
                           $('#' + propertie[1] + count).css('display', 'inline');
                           i = propertiesOfBrose.length;
                       }
                   }
               }
               count++;
           })
       }
</script>
<input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Reports','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Reports %>');" style="float: right;margin-right: 5px;margin-top:5px"/>
<div>
    <%= Html.GridFilterEventReport(ViewData["ReportFilter"].ToString(), ViewData["ReportTitle"].ToString(),ViewData["ReportModule"].ToString())%>
</div>
<%--<div class="line" style="float: left;">
    <div class="field">
        <a href="javascript: void(0);" onclick="javascript: loadListReport('<%= ViewData["ReportModule"] %>');">
            Return to List</a>
    </div>
</div>--%>
<%--<div>
    <%= Html.CreateSelectStandardData(ViewData["ReportFilter"].ToString())%>
</div>--%>