<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity" %>
  
   <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Sticky/stickytooltip.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/jqgrid/ui.multiselect.js") %>" type="text/javascript"></script>
    
   <style type="text/css">
   .field span {
    font-size: 1em;
}
   </style> 
   <script type="text/javascript">
       var enabledPeriod = function(operatorValue, id) {
           if (operatorValue.value == 'Quarterly') {
               $('#' + id).removeAttr("disabled")
           }
           else {
               $('#' + id).prop("disabled", "disabled");
           }
       };

       var nroCompetitor = function(operatorValue) {
           alert(operatorValue.value);
       };
       var enabledField = function(operatorValue, id) {
           if (operatorValue.value != '') {
               $('#' + id).removeAttr("disabled")
           }
           else {
               $('#' + id).prop("disabled", "disabled");
           }
       };
       var disabledField = function(optionList, id) {
           if (optionList.value != '') {
               $('#' + id).prop("disabled", "disabled");
           }
           else {
               $('#'+id).removeAttr("disabled");
           }
       };

       var addCompetitorsToList = function(results) {
           var arrayCompetitors = [];
           var actualValues = [];
           arrayCompetitors = results.split("_");
           var optionsActual = $('#Competitors').prop('options');
           var itemsSelected = $('#CompetitorsselectedList');
           var allItems = itemsSelected.children();
           for (var i = 0; i < allItems.length; i++) {
               if (allItems[i].children.length > 0) {
                   var childrenLi = allItems[i];
                   for (var h = 0; h < childrenLi.childNodes.length; h++) {
                       var tempo = childrenLi.childNodes[h];
                       if (tempo.tagName == 'INPUT') {
                           var inputItem = tempo;
                           var len = actualValues.length;
                           actualValues[len] = inputItem.value;
                       }
                   }
               }
           }
           var newValues = [];
           $('#Competitors').find('option').remove();
           for (j = 0; j < arrayCompetitors.length; j++) {
               var options = $('#Competitors').prop('options');
               var arrayIndt = arrayCompetitors[j].split(":");
               var actual = $("select#Competitors").children().map(function() { return $(this).val(); }).get();
               if ($.inArray(arrayIndt[0], actual) == -1) {
                   var isSelect = false;
                   for (var t1 = 0; t1 < actualValues.length; t1++) {
                       if (actualValues[t1] == arrayIndt[0]) {
                           isSelect = true;
                           t1 = actualValues.length;
                       }
                   }
                   if (isSelect) {
                       options[options.length] = new Option(arrayIndt[1], arrayIndt[0], true, true);
                   }
                   else {
                       options[options.length] = new Option(arrayIndt[1], arrayIndt[0], true, false);
                   }
                   newValues[newValues.length] = arrayIndt[0];
               }
           }
           $("#Competitors").multiSelect('destroy');
           $("#Competitors").multiSelect({ dividerLocation: 0.45 });
       };
       
       var removeAllItems = function(id) {
       $('#'+id).find('option').remove();
       $("#" + id).multiSelect('destroy');
       $("#" + id).multiSelect({ dividerLocation: 0.45 });
       };

       var updateItemsOfCompetitor = function(select) {
           var critery = select.value;
           var selectOSI = $('#CompetitorStatusOperator');
           var selectVSI = $('#CompetitorStatusValue');
           var selectOTI = $('#CompetitorTierOperator');
           var selectVTI = $('#CompetitorTierValue');
           var operatorValue = selectOSI[0].options[selectOSI[0].selectedIndex].value;

           var competitorTierOperator = selectOTI[0].options[selectOTI[0].selectedIndex].value;
           var competitorTierValue = selectVTI[0].options[selectVTI[0].selectedIndex].value;
           var competitorStatusOperator = selectOSI[0].options[selectOSI[0].selectedIndex].value;
           var competitorStatusValue = selectVSI[0].options[selectVSI[0].selectedIndex].value;
           var xmlhttp;
           var parameters = { CompetitorStatusOperator: competitorStatusOperator, CompetitorStatusValue: competitorStatusValue, CompetitorTierOperator: competitorTierOperator, CompetitorTierValue: competitorTierValue };
           var results = null;
           var url = '<%= Url.Action("GetCompetitorByStatus", "Report")%>';
           $.post(
            url,
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        addCompetitorsToList(results);
                    }
                    else {
                        removeAllItems('Competitors');
                    }
                }
            });           
           return results;
       };
   </script>
   <script type="text/javascript">
       $(function() {
       $("#Competitors").multiSelect({ dividerLocation: 0.45 });
       });
    </script>
    <script type="text/javascript">
        var executeReportDynamicTempo = function(title, reportId) {
            var competitorIds = [];

            var ulCompetitorList = $("#CompetitorsselectedList");
            var liCompetitorsNodes = ulCompetitorList[0].getElementsByTagName('li');
            for (var i = 0; i < liCompetitorsNodes.length; i++) {
                var liCompetitorsChildren = liCompetitorsNodes.item(i).childNodes;
                for (var j = 0; j < liCompetitorsChildren.length; j++) {
                    if (liCompetitorsChildren.item(j).nodeName == 'INPUT') {
                        competitorIds[i] = liCompetitorsChildren.item(j).value;
                    }
                }
            }

            var filterCompetitor = '';
            var resultCompetitor = '';
            
            var competitorStatusValue = $("#CompetitorStatusValue");
            var competitorStatusOperator = $("#CompetitorStatusOperator");
            if (competitorStatusValue[0].options[0].selected == false && competitorStatusOperator[0].options[0].selected == false) {
                filterCompetitor += 'CompetitorStatus_' + competitorStatusOperator[0].value + '_' + competitorStatusValue[0].value + ":";
            }
            var competitorTierValue = $("#CompetitorTierValue");
            var competitorTierOperator = $("#CompetitorTierOperator");
            if (competitorTierOperator[0].options[0].selected == false && competitorTierValue[0].options[0].selected == false) {
                filterCompetitor += 'CompetitorTier_' + competitorTierOperator[0].value + '_' + competitorTierValue[0].value + ":";
            }
            if (filterCompetitor != '') {
                resultCompetitor = filterCompetitor.substring(0, filterCompetitor.length - 1);
            }

            var financial = $('#FinancialView');
            var periodType = $('#FinancialPeriodType'); 
            var periodValue = $('#FinancialPeriodValue');
            var year = $('#Year');
            
            executeReportDynamicFinancial(title, reportId, competitorIds, resultCompetitor, financial[0].value, periodType[0].value, periodValue[0].value, year[0].value);
        };
    </script>
<br />
<br />
<div class="line" >
    <% if (!string.IsNullOrEmpty(ViewData["ReportTitle"].ToString()))
   { %>    
    <div class="field" style="width:392px;float:left">
        <%= ViewData["ReportTitle"].ToString() %>
    </div>
    <%} %>
    <div class="field" style="width:30%">
        <a href="javascript: void(0);" onclick="javascript: loadListDynamicReport('<%= ViewData["ReportModule"] %>');">
            Return to Reporting Menu</a>
    </div>
</div>



<br />
<div class="line">
    <div class="field">
    <br />
    <label for="FinancialPeriodType">
        <asp:Literal ID="FinancialPeriodType"  runat="server" Text="<%$ Resources:LabelResource, FinancialPeriodType %>" ></asp:Literal>:</label>
     </div>   
    <div class="field">
        <br />
        <%= Html.DropDownList("FinancialPeriodType", (SelectList)ViewData["FinancialPeriodTypeList"], new { id = "FinancialPeriodType", onchange = "javascript: enabledPeriod(this,'FinancialPeriodValue');" })%>
    </div>
    <div class="field">
        <br />
        <%= Html.DropDownList("FinancialPeriodValue", (SelectList)ViewData["FinancialPeriodValueList"], new { id = "FinancialPeriodValue", onchange = "javascript: updateItemsOfIndustry(this); disabledField(this,'PeriodOperator');", disabled = "true" })%>
    </div>
    <div class="field">
        <label for="Year"><asp:Literal ID="Year" runat="server" Text="Year" />:</label>
        <%= Html.TextBox("Year", DateTime.Now.Year, new { id = "Year"})%>
    </div>
</div>
<div class="line">
    <div class="field" >
        <label for="FinancialView">
        <asp:Literal ID="FinancialView"  runat="server" Text="<%$ Resources:LabelResource, FinancialView %>" ></asp:Literal>:</label>
    </div>
    <div class="field">
        <%= Html.DropDownList("FinancialView", (SelectList)ViewData["FinancialViewList"], new { id = "FinancialView", onchange = "javascript: enabledField(this,'IndustryStatusValue');" })%>
    </div>
</div>
<div class="line">
    <div >
        <label for="ReportCompetitors">
        <asp:Literal ID="ReportCompetitors"  runat="server" Text="<%$ Resources:LabelResource, ReportSelectCompetitors %>" ></asp:Literal>:</label>
        <%= Html.DropDownList("Competitors", (SelectList)ViewData["Competitors"], null, new { id = "Competitors", style = "height:200px;width:80%;display:none;", ccsClass = "multiselect", multiple = "multiple", onclick = "javascript: nroCompetitor(this);" })%>
    </div>
</div>
<div class="line">
    <div class="field" style="width:110px;">
        <label for="CompetitorTierOperator">
        <asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:LabelResource, ReportDynamicCompetitorTierOperator %>" ></asp:Literal>:</label>
    </div>
    <div class="field">
        <%= Html.DropDownList("CompetitorTierOperator", (SelectList)ViewData["FilterOperator"], string.Empty, new { id = "CompetitorTierOperator", onchange = "javascript: enabledField(this,'CompetitorTierValue');" })%>
    </div>
    <div class="field">
        <%= Html.DropDownList("CompetitorTierValue", (SelectList)ViewData["CompetitorTierList"], string.Empty, new { id = "CompetitorTierValue", onchange = "javascript:updateItemsOfCompetitor(this); disabledField(this,'CompetitorTierOperator');", disabled = "true" })%>
    </div>
</div>
<div class="line">
    <div class="field" style="width:110px;">
        <label for="CompetitorStatusOperator">
        <asp:Literal ID="Literal3"  runat="server" Text="<%$ Resources:LabelResource, ReportDynamicCompetitorStatusOperator %>" ></asp:Literal>:</label>
    </div>
    <div class="field">
        <%= Html.DropDownList("CompetitorStatusOperator", (SelectList)ViewData["FilterOperator"], string.Empty, new { id = "CompetitorStatusOperator", onchange = "javascript: enabledField(this,'CompetitorStatusValue');" })%>
    </div>
    <div class="field">
        <%= Html.DropDownList("CompetitorStatusValue", (SelectList)ViewData["CompetitorStatusList"], string.Empty, new { id = "CompetitorStatusValue", onchange = "javascript: updateItemsOfCompetitor(this); disabledField(this,'CompetitorStatusOperator');", disabled = "true" })%>
    </div>
</div>
<div class="line">
    <div class="field">
        <input id="buttonFilter" type="button" onclick="executeReportDynamicTempo('Financial Performance per Competitor','<%= ViewData["ReportId"] %>');" Value="Create Report"/>
        <input id="buttonAdd" type="button" class="add-all" Value="Add All"/>
        <input id="buttonReset" type="button" class="remove-all" Value="Reset"/>
    </div>
</div>

