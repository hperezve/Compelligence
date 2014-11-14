<%@ Page Title="Compelligence - Deal Support" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Deal>" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
   <%-- <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"
        type="text/css" />--%>
        <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/thickbox.css") %>" rel="stylesheet"
        type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"
        type="text/javascript"></script>
        
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.bgiframe.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.ajaxQueue.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/thickbox-compressed.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.json.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookiejar.pack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.tableFilter.js") %>" type="text/javascript"></script>

    <% string formId = "DealSupportForm"; %>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/FeedBack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>">  </script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>
    <style type="text/css">
 	
 	#contentleft
    {
        width: 70%; 	 
    }
</style>  
 <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Retrieve.js") %>"></script>
<style type="text/css">
    
    .contentscrollableP select option {
           height:16px;
    }
    .contentscrollableC select option {
           height:16px;
    }
    .contentscrollableI select option {
           height:16px;
    }
    #DealCompetitorsIds option    
    {
    	height:16px;
    }
</style>   
<script type="text/javascript">

    $(function() {
        var fId = '<%=formId %>';
        $("#" + fId).submit(function(e) {
        convertText();
        });
    });
    function convertText() {
        var textboxes = document.getElementsByTagName("input");
        for (var i = 0; i < textboxes.length; i++) {
            if ((textboxes[i].type == "text")) {
                if (textboxes[i].value != "") {
                    textboxes[i].value = convertTextPlainHtml(textboxes[i].value);
                }
            }
        }
    }
    var AddNewCustomerContact = function(text) {
    var customerContact = $("#DealSupportFormCustomerContact");
        var newCustomerContact = $("#NewCustomerContact");
        newCustomerContact[0].value = '';
        var employeeIdsSelected = $("#EmployeeIdsSelected");
        if (employeeIdsSelected[0].value != '') {
            var temporal = employeeIdsSelected[0].value.split(',');
            var array = new Array(temporal.length);
            for (var v = 0; v < temporal.length; v++) {
                var tempo = temporal[v].split(':');
                array[v] = tempo[1];
            }
            var ccTemporal = customerContact[0].value.split('\n');
            var isNewEmployee;
            for (var w = 0; w < ccTemporal.length; w++) {
                isNewEmployee = true;
                for (var z = 0; z < array.length; z++) {
                    if (ccTemporal[w] == array[z]) {
                        isNewEmployee = false;
                    }
                }
                if (isNewEmployee) {
                    if (newCustomerContact[0].value != '') {
                        newCustomerContact[0].value = newCustomerContact[0].value + '\n';
                    }
                    newCustomerContact[0].value = newCustomerContact[0].value + ccTemporal[w];
                }
            }
        }
        else {
            newCustomerContact[0].value = customerContact[0].value;
        }
        UpdateCustomerContactValue();
    };

    var UpdateCustomerContactValue = function() {
    var customerContact = $("#DealSupportFormCustomerContact");
        customerContact[0].value = '';
        var employeeIdsSelected = $("#EmployeeIdsSelected");
        var newCustomerContact = $("#NewCustomerContact");
        if (employeeIdsSelected[0].value != '') {
            var tempo = employeeIdsSelected[0].value.split(',');
            for (var x = 0; x < tempo.length; x++) {
                var item = tempo[x].split(':');
                if (customerContact[0].value != '') {
                    customerContact[0].value = customerContact[0].value + '\n';
                }
                customerContact[0].value = customerContact[0].value + item[1];
            }
        }
        if (newCustomerContact[0].value != '') {
            if (employeeIdsSelected[0].value != '') {
                customerContact[0].value = customerContact[0].value + '\n';
            }
            customerContact[0].value = customerContact[0].value + newCustomerContact[0].value;
        }
    }

    var AddEmployeesToCustomerContact = function(select) {
    var customerContact = $("#DealSupportFormCustomerContact");
        var employeeIdsSelected = $("#EmployeeIdsSelected");
        employeeIdsSelected[0].value = '';
        for (var i = 0; i < select.options.length; i++) {
            if (select.options[i].selected == true) {
                if (employeeIdsSelected[0].value != '') {
                    employeeIdsSelected[0].value = employeeIdsSelected[0].value + ',';
                }
                employeeIdsSelected[0].value = employeeIdsSelected[0].value + select.options[i].value + ':' + select.options[i].text;
            }
            else {

            }
        }
        UpdateCustomerContactValue();
    };

    var UpdateCustomerId = function(text, value) {
        if (value == '') { }
        else {
            var customerId = $("#CustomerId");
            var optionsEmployees = $("#CustomerNameHidden").prop('options');
            var employeelist;
            for (var i = 0; i < optionsEmployees.length; i++) {
                if (optionsEmployees[i].value == customerId[0].value) {
                    employeelist = optionsEmployees[i].text;
                    i = optionsEmployees.length;
                }
            }
            var customerValue = $("#CustomerValue");
            var options = $("#DealEmployeeIds").prop('options');
            var isCustomerValue = customerValue[0].value.indexOf(value);
            options.length = 0;
            if ((isCustomerValue != -1) && (customerValue[0].value == value)) {
                if (employeelist != '') {
                    var arrayEmployees = employeelist.split('_'); //array of employees of customer
                    for (var j = 0; j < arrayEmployees.length; j++) {
                        var arrayEmployee = arrayEmployees[j].split(':');
                        options[options.length] = new Option(arrayEmployee[1], arrayEmployee[0], true, false);
                    }
                }
            }
            else {
                customerValue[0].value = 0;
            }
        }
    };
    var UpdateCustomerIdByResult = function(value) {
        if (value == '') { }
        else {
            var customerId = $("#CustomerId");
            var optionsEmployees = $("#CustomerNameHidden").prop('options');
            var employeelist;
            for (var i = 0; i < optionsEmployees.length; i++) {
                if (optionsEmployees[i].value == value) {
                    employeelist = optionsEmployees[i].text;
                    i = optionsEmployees.length;
                }
            }
            var options = $("#DealEmployeeIds").prop('options');
            options.length = 0;
            if (employeelist != '') {
                var arrayEmployees = employeelist.split('_'); //array of employees of customer
                for (var j = 0; j < arrayEmployees.length; j++) {
                    var arrayEmployee = arrayEmployees[j].split(':');
                    options[options.length] = new Option(arrayEmployee[1], arrayEmployee[0], true, false);
                }
            }
        }
    };

    function disabledEndUsers() {
        var UsersIds = $('#UsersIds');
        var disabled = '<%= ViewData["EndUserIdMultiListDisabled"] %>';
        if (disabled == 'true') {
            UsersIds.prop("disabled", "true");
        }
        else {
            UsersIds.removeAttr('disabled');
        }
    };

    function SetValuesEmployees() {
        var selectedEmployees = '';
        var options = $("#DealEmployeeIds").prop('options');
        if (options.length > 0) {
            selectedEmployees = $('#oldDealEmployeeIds').prop('value');
            if (selectedEmployees != null && selectedEmployees != undefined && selectedEmployees != '') {
                var arrayEmployee = selectedEmployees.split(',');
                for (var i = 0; i < options.length; i++) {
                    var t = options[i].value;
                    for (var j = 0; j < arrayEmployee.length; j++) {
                        if (t == arrayEmployee[j]) {
                            options[i].selected = true;
                        }
                    }
                }
            }
        }
    };

    function SetSelectedEndUserIdMultiList() {
        var options = $("#UsersIds").prop('options');
        if (options.length > 0) {
            selectedEndUsers = $('#UsersIdsHidden').prop('value');
            if (selectedEndUsers != null && selectedEndUsers != undefined && selectedEndUsers != '') {
                var arrayUsers = selectedEndUsers.split(',');
                for (var i = 0; i < options.length; i++) {
                    var t = options[i].value;
                    for (var j = 0; j < arrayUsers.length; j++) {
                        if (t == arrayUsers[j]) {
                            options[i].selected = true;
                        }
                    }
                }
            }
        }
    };

    var updateCheckByHierarchy = function() {
        var checked = $('#checkedbyHierarchy').val();
        if (checked != '') {
            if (checked == 'true') {
                $('#CheckIndustryIds').prop('checked', true);
            }
            else if (checked == 'false') {
            $('#CheckIndustryIds').prop('checked', false);
            }
        }
    }

    function addNewIndustriesToList(results) {
        var i = $(this);
        i.queue(function() {
            setTimeout(function() {
                i.dequeue();
            }, 1000);
        });

        var arrayIndustries = [];
        arrayIndustries = results.split("_");
        var options = $('#DealIndustriesIds').prop('options');
        $('#DealIndustriesIds')[0].options.length = 0;
        for (j = 0; j < arrayIndustries.length; j++) {

            var arrayCompet = arrayIndustries[j].split(":");
            if (arrayCompet[2] == 'True') {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
            }
            else {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
            }
        }

        $("#DealIndustriesIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
    };
</script>
<script type="text/javascript">
    var AutoCompleteCustomer = function() {
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetCustomers", "DealSupport")%>',
            dataType: "json",
            success: function(data) {
                $("#CustomerName").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {                        
                        return row.Text;
                    },
                    formtaResult: function(row) {
                        return row.Text;
                    }
                });
                $("#CustomerName").result(function(event, data, formatted) {
                    var customerId = $("#CustomerId");
                    customerId[0].value = data.Value;
                    UpdateCustomerIdByResult(data.Value);
                });
            }
        });
    }

    $(function() {
        $("#DueDateFrm").datepicker();
        $("#CloseDateFrm").datepicker();
        disabledEndUsers();
        AutoCompleteCustomer();
        SetValuesEmployees();
        SetSelectedEndUserIdMultiList();
        updateCheckByHierarchy();
        
        $('#CustomerName').result(findValueCustomerCallback);

        $("#CustomerName").blur(function() {
            if ($(this).val() == '') {
                $("#CustomerId").val('');
            }
        });
    });
          
    var findValueCustomerCallback = function (event, data, formatted) {
        $('#CustomerId').val(data.Value);
    };
</script>

<script type="text/javascript">
    var initializeForm = function(formId, urscas, dateFormFields, closeDate) {
        if (urscas == '<%= UserSecurityAccess.Read %>') {
            disableFormFields(formId);
        }
        
        // If there are date fields in this form, so Datepicker will be applied
        if (dateFormFields != null) 
        {
            for (var i = 0; i < dateFormFields.length; i++) 
            {
                $(formId + ' input[type=text][name=' + dateFormFields[i] + ']').datepicker();
            }
        }
        if (closeDate != null) {
            for (var i = 0; i < closeDate.length; i++) {
                $(formId + ' input[type=text][name=' + closeDate[i] + ']').datepicker();
            }
        }
    };

    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', ["CloseDateFrm"], ["DueDateFrm"]);
        showWinning('#<%= formId %>Phase');
        hideMe('#WinningCompetitorList');
    });
        
    function SubmitForm(Url) 
    {
        var values = $("#DealSupportForm").serialize();
        var files = $("#DealSupportForm input[@name='uploadfile']");
        var sfiles = files.serialize();
        alert(files.length);
        $.post(Url, values + "&" + sfiles);
    }   
</script>

<script type="text/javascript">

    var ubfcompetitors = '<%= Url.Action("GetMasiveCompetitors", "Deal")%>'; //set for portability
    var ubfproducts = '<%= Url.Action("GetMasiveProducts", "Deal")%>'; //set for portability
    var ubfhierarchy = '<%= Url.Action("ChangeIndustryList", "Deal") %>'; //set for hierarchy industries


    var updateComboPrimaryCompetitor = function(realCompetitors, idtarget) {
        var valPrimary = getOptionValues(idtarget, true);
        $(idtarget)[0].options.length = 0;
        $(idtarget).prepend("<option value=''></option>");
        var options = $(idtarget)[0].options;
        appendOption(options, realCompetitors, valPrimary);
    };
    var updateComboPrimaryIndustry = function() {

    var fId = 'DealSupportForm';

        var realIndustries = getOptionPairs('#IndustryIds', true); //{Text,Value},...
        var valPrimary = getOptionValues('#' + fId + 'PrimaryIndustry', true);

        $('#' + fId + 'PrimaryIndustry')[0].options.length = 0;
        $('#' + fId + 'PrimaryIndustry').prepend("<option value=''></option>");

        var options = $('#' + fId + 'PrimaryIndustry')[0].options;
        appendOption(options, realIndustries, valPrimary);

    };

    

    var updateCompAndProd = function() {
        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        var realvaluesCompetitor = [];
        var realtextsCompetitor = [];
        var realvaluesProduct = [];

        $('#DealIndustriesIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });
        if (realvaluesIndustry == "") {
            $('#DealCompetitorsIds')[0].options.length = 0;
            $('#DealProductsIds')[0].options.length = 0;
        } else {
            $('#DealCompetitorsIds :selected').each(function(i, selected) {
                realvaluesCompetitor[i] = $(selected).val();
                realtextsCompetitor[i] = $(selected).text();
            });
            $('#DealProductsIds :selected').each(function(i, selected) {
                realvaluesProduct[i] = $(selected).val();
            });
            $('#DealCompetitorsIds')[0].options.length = 0;
            $('#DealProductsIds')[0].options.length = 0;

            for (i = 0; i < realvaluesIndustry.length; i++) {
                setValuesForCompetitors(realvaluesIndustry[i], realvaluesCompetitor, realvaluesProduct);
            }
        }
        updateComboPrimaryIndustry(realvaluesIndustry, textvaluesIndustry, 'DealSupportFormPrimaryIndustry');
    };

    function setValuesForCompetitors(id, realvaluesCompetitor, realvaluesProduct) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var url = '<%= Url.Action("GetCompetitorsOfIndustry", "DealSupport")%>/' + id;
        $.post(
            url,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        addCompetitorsToList(results, realvaluesCompetitor, realvaluesProduct);
                    }
                }
            });
        
        return results;
    }

    function addCompetitorsToList(results, realvaluesCompetitor, realvaluesProduct) {
        var i = $(this);
        i.queue(function() {
            setTimeout(function() {
                i.dequeue();
            }, 1000);
        });
        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        $('#DealIndustriesIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });

        var arrayComppetitors = [];
        arrayComppetitors = results.split("_");
        for (j = 0; j < arrayComppetitors.length; j++) {
            var options = $('#DealCompetitorsIds').prop('options');
            var arrayCompet = arrayComppetitors[j].split(":");
            var actual = $("select#DealCompetitorsIds").children().map(function() { return $(this).val(); }).get();
            if ($.inArray(arrayCompet[0], actual) == -1) {
                if (realvaluesCompetitor == "") {
                    options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
                } else {
                    if ($.inArray(arrayCompet[0], realvaluesCompetitor) == -1) {
                        options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
                    } else {
                        options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
                        setValuesForProductsOfCompetitor(arrayCompet[0], realvaluesProduct, realvaluesIndustry);
                    }
                }
            }
        }

        var count2 = ($('#DealCompetitorsIds')[0].options.length) * 16 + 10;
        var count3 = ($('#DealProductsIds')[0].options.length) * 16 + 10;

        $('.contentscrollableC select').css('height', count2 + "px");
        $('.contentscrollableP select').css('height', count3 + "px");

        $("#DealCompetitorsIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
        
        $("#DealProductsIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
    }

    function setValuesForProductsOfCompetitor(id, realvaluesProduct, realvaluesIndustry) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var idDeal = $('#Id').val();
        var idsIndustries = realvaluesIndustry;
        var url = '<%= Url.Action("GetProductsOfCompetitor", "DealSupport")%>/' + id + '?idDeal=' + idDeal + '&idsIndustries=' + idsIndustries;
        $.post(
            url,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        addProductsToList(results, realvaluesProduct);
                    }
                }
            });
        return results;
    }

    function addProductsToList(results, realvaluesProduct) {
        var arrayProducts = [];
        arrayProducts = results.split("_");
        for (j = 0; j < arrayProducts.length; j++) {
            var options = $('#DealProductsIds').prop('options');
            var arrayProd = arrayProducts[j].split(":");
            var actual = $("select#DealProductsIds").children().map(function() { return $(this).val(); }).get();
            if ($.inArray(arrayProd[0], actual) == -1) {
                if (realvaluesProduct == "") {
                    options[options.length] = new Option(arrayProd[1], arrayProd[0], true, false);
                } else {
                    if ($.inArray(arrayProd[0], realvaluesProduct) == -1) {
                        options[options.length] = new Option(arrayProd[1], arrayProd[0], true, false);
                    } else {
                        options[options.length] = new Option(arrayProd[1], arrayProd[0], true, true);
                    }
                }
            }
        }
  
        var count3 = ($('#DealProductsIds')[0].options.length) * 16 + 10;
        $('.contentscrollableP select').css('height', count3 + "px");

        $("#DealProductsIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
    }

    function resetMultiSelect() {
        $('#DealCompetitorsIds')[0].options.length = 0;
        $('#DealProductsIds')[0].options.length = 0;

        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        var realvaluesCompetitor = [];
        var realvaluesProduct = [];

        realvaluesCompetitor = $('#OldCompetitorsIds').val().split(",");
        realvaluesProduct = $('#OldProductsIds').val().split(",");

        $('#DealIndustriesIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });

        if (realvaluesIndustry != "") {
            for (i = 0; i < realvaluesIndustry.length; i++) {
                setValuesForCompetitors(realvaluesIndustry[i], realvaluesCompetitor, realvaluesProduct);
            }
        }
    }
</script>
<!--Need set parameter ubfcompetitors-->
<!--Need set parameter ubfproducts-->
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/BackEnd/updateicp.js")%>"></script>

<!--Need set parameter ubfHierarchy-->
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/BackEnd/hierarchy.js")%>"></script>

<script type="text/javascript">
    function updPrimaries() {
        var realcompetitors = getOptionPairs('#CompetitorIds', true);
        updateComboPrimaryCompetitor(realcompetitors, '#DealSupportFormPrimaryCompetitor');
        updateComboPrimaryCompetitor(realcompetitors, '#WinningCompetitorList');
    }
    $(function() {

        updateUpHeight('#IndustryIds');
        updateUpHeight('#CompetitorIds');
        updateUpHeight('#ProductIds');



        fixOptionTitle("#IndustryIds");
        fixOptionTitle("#CompetitorIds");
        fixOptionTitle("#ProductIds");

    });
    /*function to numeric value*/
    function isNumbersAndPointKey(event, element, _float) {
        event = event || window.event;
        var charCode = event.which || event.keyCode;
        if (charCode == 8 || charCode == 13 || (_float ? (element.value.indexOf('.') == -1 ? charCode == 46 : false) : false))
            return true;
        else if ((charCode < 48) || (charCode > 57))
            return false;
        return true;
    };
    function isNumbersAndPointKeyOrValue(event, element, _float) {
        event = event || window.event;
        var charCode = event.which || event.keyCode;
        var patronA = /[\^(\d|.)]/; //THIS PATRON IS TO NUMERIC AND POINT
        var patronB = /([nN])|([nN]+\/)/; //TO SEARCH /N/A
        var patronC = /[a-zA-Z\/]/; //TO ALL CHARACTER AND /
        var result = false;
        // TO FUTURE
        // shift + 5 = % = 37 , event.shiftKey = TRUE if key press shift
        // shift + 4 = $ = 36
        //             / = 47
        // left arrow    = 37
        // apostrophe ( ' )  = 39 , event.keyCode= 0 & event.which=39 [FireFox]
        // apostrophe ( ' )  = 39 ,  event.keyCode= 39 & event.which=undefined [IE]
        // right arrow  = 39, event.keyCode= 39 & event.which=0
        // backspace=8, delete = 46
        // left arrow, right arrow no working to IE and Safary    
        if (charCode == 8 || charCode == 13 || (charCode == 39 && (event.keyCode == 39 && event.which == 0) && !patronB.test(element.value)) || (!event.shiftKey && charCode == 37 && !patronB.test(element.value)) || (_float ? (element.value.indexOf('.') == -1 ? charCode == 46 : false) : false))
            result = true;
        else if ((((_float ? ((element.value.indexOf('A') == -1 && element.value.indexOf('a') == -1) ? charCode == 65 : false) : false) && (element.value != '' && (element.value == 'n/' || element.value == 'N/'))) ||
                  ((_float ? ((element.value.indexOf('A') == -1 && element.value.indexOf('a') == -1) ? charCode == 97 : false) : false) && (element.value != '' && (element.value == 'n/' || element.value == 'N/'))) ||
                  (_float ? ((element.value.indexOf('N') == -1 && element.value.indexOf('n') == -1) ? charCode == 110 : false) : false) ||
                  (_float ? ((element.value.indexOf('N') == -1 && element.value.indexOf('n') == -1) ? charCode == 78 : false) : false) ||
                  ((_float ? (element.value.indexOf('/') == -1 ? charCode == 47 : false) : false) && element.value != ''))
                  && !patronA.test(element.value) && (element.value == '' || patronB.test(element.value)))
            result = true;
        else if ((charCode < 48) || (charCode > 57))
            result = false;
        else
            result = !patronC.test(element.value);
        ShowMessage(result);
        return result;
    };
    function ShowMessage(value) {
        if (value) $('#divMessageAlert').hide();
        else $('#divMessageAlert').show();
    };
    
</script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  <% string formId = "DealSupportForm"; %>
    
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <% Html.RenderPartial("FrontEndFormMessages"); %>
    <div>
        <div class="headerContentRightMenu">
        
        <asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:LabelResource, DealSupportEdit %>" />
        <%--Edit --%>
        </div>
        <fieldset class="contentRightMenu">
            <%= Html.ValidationSummary() %>
            <% using (Html.BeginForm((string)ViewData["ActionMethod"], "DealSupport", FormMethod.Post,
      new { id = "DealSupportForm", enctype = "multipart/form-data" }))
               { %>
            <%= Html.Hidden("Id",default(decimal)) %>
            <%= Html.Hidden("OldName")%>
            <%= Html.Hidden("OldIndustriesIds")%>
            <%= Html.Hidden("OldCompetitorsIds")%>
            <%= Html.Hidden("OldProductsIds")%>
            <%= Html.Hidden("OldPhase")%>
            <%= Html.Hidden("OldPrimaryCompetitor")%>
            <%= Html.Hidden("OldPrimaryIndustry")%>
            <%= Html.Hidden("OperationStatus")%>
            <%= Html.Hidden("OldUsersIds")%>
            <%= Html.Hidden("oldDealEmployeeIds")%>
            <%= Html.Hidden("UsersIdsHidden")%>
            <%= Html.Hidden("checkedbyHierarchy")%>
            <div class="line">
                <div class="field">
                    <label for="Name" class="required">
                        <asp:Literal ID="DealName" runat="server" Text="<%$ Resources:LabelResource, DealName %>" />:</label>
                        <% string stylea = string.Empty;
                        stylea = "width:440px;";%>
                    <%= Html.TextBox("Name",null, new { id = formId + "Name", @style = stylea })%>
                    <%--<%=Html.TextBox("Name")%> antes--%>
                    <%= Html.ValidationMessage("Name", "*") %>
                </div>
            </div>
            
            <div class="line">                
                <div class="field">
                    <label for="<%= formId %>CreatedByName">
                        <asp:Literal ID="DealOpenedByFrm" runat="server" Text="<%$ Resources:LabelResource, DealOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByName", null, new { @readonly = "readonly",@class="Disable" })%>
                    <%= Html.ValidationMessage("CreatedByName", "*")%>
                </div>
                <div class="field">
                    <label for="CurrencyId">
                        <asp:Literal ID="DealCurrencyId" runat="server" Text="<%$ Resources:LabelResource, DealCurrencyId %>" />:</label>
                    <%= Html.TextBox("CurrencyId", null, new { id = formId + "CurrencyId", onkeypress = "return isNumbersAndPointKeyOrValue(event, this, true)" })%>
                    <%= Html.ValidationMessage("CurrencyId", "*") %>
                    <div id="divMessageAlert" style="display: none; position: absolute;">
                        <label id="lblMessageAlert" style="color: rgb(255, 0, 0); font-size:smaller;">System will only accept a number or empty</label><br />
                    </div>
                </div>
            </div>
            <div class="line">
                <%--<div class="field">
                    <label for="ExternalId">
                        <asp:Literal runat="server" Text="<%$ Resources:LabelResource, DealExternalId %>" />:</label>
                    <%= Html.TextBox("ExternalId", null, new { id = formId + "ExternalId",Readonly="readonly",@class="Disable" })%>
                </div>--%>
                <div class="field">
                    <label for="<%= formId %>CloseDateFrm">
                        <asp:Literal ID="DealCloseDateFrm" runat="server" Text="<%$ Resources:LabelResource, DealCloseDate %>" />:</label>
                    <%= Html.TextBox("CloseDateFrm", null, new { id = formId + "CloseDateFrm" })%>
                    <%= Html.ValidationMessage("CloseDateFrm", "*")%>
                </div>                
                <div class="field">
                    <label for="<%= formId %>CreatedDateFrm">
                        <asp:Literal ID="DealCreatedDateFrm" runat="server" Text="<%$ Resources:LabelResource, AnswerDate %>" />:</label>
                    <%= Html.TextBox("CreatedDateFrm", null, new { @readonly = "readonly",@class="Disable" })%>
                    <%= Html.ValidationMessage("CreatedDateFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>IndustryId" class="required">
                            <asp:Literal ID="KitIndustryId" runat="server" Text="" /><%=ViewData["IndustryLabel"]%>:</label>                        
                    <label for="CheckIndustryIds" >
                           <%= Html.CheckBox("CheckIndustryIds", (bool)ViewData["CheckIndustryIds"], new { onclick = "ShowIndustriesByHierarchy('#',this);", Style = "float:left;margin-left:0px;height:15px;" })%>
                           &nbsp;By Hierarchy
                    </label>
                    <div class='contentscrollableI'>
                    <%= Html.DropDownList("IndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript:  updCompetitors('#');updateComboPrimaryIndustry();", Style = "border: none;" })%>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                    </div>
                </div>
                <div class="field">
                    <label>&nbsp;</label>
                    <label for="<%= formId %>CompetitorId" class="required">
                        <asp:Literal ID="KitCompetitorId" runat="server" Text="" /><%=ViewData["CompetitorLabel"]%>:</label>
                    <div class='contentscrollableC'>                        
                        <%= Html.DropDownList("CompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updProducts('#');updPrimaries();", Style = "border: none;" })%>
                        <%= Html.ValidationMessage("CompetitorId", "*")%>
                    </div>
                </div>
                <div class="field">
                    <label>&nbsp;</label>
                    <label for="<%= formId %>ProductId">
                        <%=ViewData["ProductLabel"]%>:</label>
                    <div class='contentscrollableP'>
                        <%= Html.DropDownList("ProductIds", (MultiSelectList)ViewData["ProductIdList"], new { Multiple = "true", Style = "border: none;" })%>
                        <%= Html.ValidationMessage("ProductId", "*")%>
                    </div> 
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>PrimaryIndustry"  class="required">
                        <asp:Literal ID="DealPrimaryIndustry" runat="server" Text="" />Primary <%=ViewData["IndustryLabel"]%>:</label>  
                    <%= Html.DropDownList("PrimaryIndustry", (SelectList)ViewData["SelectedIndustryIdList"], string.Empty, new { style = "width:208px", id = formId + "PrimaryIndustry" })%>
                    <%= Html.ValidationMessage("PrimaryIndustry", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>PrimaryCompetitor"  class="required">
                        <asp:Literal ID="DealPrimaryCompetitor" runat="server" Text="" />Primary <%=ViewData["CompetitorLabel"]%>:</label>
                    <%= Html.DropDownList("PrimaryCompetitor", (SelectList)ViewData["SelectedCompetitorIdList"], string.Empty, new { style = "width:208px", id = formId + "PrimaryCompetitor" })%>
                    <%= Html.ValidationMessage("PrimaryCompetitor", "*")%>
                </div>           
                <div class="field">
                    <label for="CustomerName">
                        <asp:Literal ID="DealCustomerId" runat="server" Text="<%$ Resources:LabelResource, DealCustomerId %>" />:</label>
                    <%= Html.TextBox("CustomerName", null)%>
                    <%= Html.ValidationMessage("CustomerName", "*") %>
                    <%= Html.Hidden("CustomerId") %>
                    <%= Html.Hidden("PreviousCustomerId") %>
                    <%= Html.Hidden("CustomerValue") %>
                </div> 
            </div>           
            <div class="line">                               
                <div>
                    <%= Html.ListBox("CustomerNameHidden", (SelectList)ViewData["EmployeesByCustomerList"], new { style = "display:none" })%>
                </div>                                     
                <div class="field">
                    <label for="CustomerContact">
                        <asp:Literal ID="DealCustomerContact" runat="server" Text="<%$ Resources:LabelResource, DealCustomerContact %>" />:</label>
                    <%= Html.TextArea("CustomerContact", null, new { id = formId + "CustomerContact", style = "height:145px;width:205px", onblur = "javascript:AddNewCustomerContact();" })%>
                    <%= Html.ValidationMessage("CustomerContact", "*")%>
                </div>
                <div class="field">                
                    <label for="<%= formId %>DealEmployeeId">
                        <asp:Literal ID="DealDealEmployeeId" runat="server" Text="<%$ Resources:LabelResource, DealEmployeeIdsOfCustomer %>" />:</label>
                    <%= Html.ListBox("DealEmployeeIds", (MultiSelectList)ViewData["EmployeeIdList"], new { id = "DealEmployeeIds", size = 5, style = "height:150px;width:208px", Multiple = "true", onchange = "javascript:AddEmployeesToCustomerContact(this);" })%>
                    <%= Html.ValidationMessage("DealEmployeeIds", "*")%>
                    <%= Html.Hidden("EmployeeIdsSelected") %>
                    <%= Html.Hidden("NewCustomerContact") %>
                </div>                
                <div class="field">
                    <label for="<%= formId %>UsersIds">
                        <asp:Literal ID="DealUsersIds" runat="server" Text="<%$ Resources:LabelResource, DealEndUserIds %>" />:</label>
                    <%= Html.DropDownList("UsersIds", (MultiSelectList)ViewData["EndUserIdMultiList"], new { size = 5, style = "height:150px; width:208px", Multiple = "true" })%>
                    <%= Html.ValidationMessage("UsersIds", "*")%>
                </div>   
            </div>
            <div class="line">
                <div class="field">
                    <label for="CompetitorDiscount">
                        <asp:Literal ID="DealCompetitorDiscount" runat="server" Text="" /><%=ViewData["CompetitorLabel"]%> Discount:</label>
                    <%= Html.TextBox("CompetitorDiscount")%>
                    <%= Html.ValidationMessage("CompetitorDiscount", "*")%>
                </div>
                <div class="field">
                    <label for="CompetitorAccountManager">
                        <asp:Literal ID="DealCompetitorAccountManager" runat="server" Text="<%$ Resources:LabelResource, DealCompetitorAccountManager %>" />:</label>
                    <%= Html.TextBox("CompetitorAccountManager")%>
                    <%= Html.ValidationMessage("CompetitorAccountManager", "*")%>
                </div>
            </div>
            <div class="line">
                 <div class="field">
                    <label  class="required" for="<%= formId %>DueDateFrm"  class="required">
                        <asp:Literal ID="DealDueDateFrm" runat="server" Text="<%$ Resources:LabelResource, DealDueDate %>" />:</label>
                    <%= Html.TextBox("DueDateFrm", null, new { id = formId + "DueDateFrm" })%>
                    <%= Html.ValidationMessage("DueDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Phase">
                        <asp:Literal ID="DealPhase" runat="server" Text="<%$ Resources:LabelResource, DealPhase %>" />:</label>
                    <%= Html.DropDownList("Phase", (SelectList)ViewData["PhaseList"], string.Empty, new { style = "width:208px", id = formId + "Phase", onchange = "javascript: showWinning(this);" })%>
                    <%= Html.ValidationMessage("Phase", "*")%>
                </div>
                <div id="WinningCompetitorField" class="field">
                    <label for="<%= formId %>WinningCompetitor">
                        <asp:Literal ID="DealWinning" runat="server" Text="" />Winning <%=ViewData["CompetitorLabel"]%>:</label>
                    <%= Html.TextBox("WinningCompetitor", null, new { id = "WinningCompetitor", onclick="javascript: toggleCombo('WinningCompetitorList');", onkeyup="javascript: updateCombo(this,'WinningCompetitorList');" })%><br />
                    <%= Html.ListBox("WinningCompetitorList", (SelectList)ViewData["SelectedCompetitorIdList"], new { style = "width:208px", id = "WinningCompetitorList", onchange = "javascript: updateText('WinningCompetitorList','WinningCompetitor')" })%>
                    <%= Html.ValidationMessage("WinningCompetitor", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="CompetitorAccountStrategy">
                        <asp:Literal ID="DealCompetitorAccountStrategy" runat="server" Text="<%$ Resources:LabelResource, DealCompetitorAccountStrategy %>" />:</label>
                    <%= Html.TextArea("CompetitorAccountStrategy")%>
                    <%= Html.ValidationMessage("CompetitorAccountStrategy", "*")%>
                </div>
                <div class="field">
                    <label for="Notes">
                        <asp:Literal ID="DealNotes" runat="server" Text="<%$ Resources:LabelResource, DealNotes %>" />:</label>
                    <%= Html.TextArea("Notes")%>
                    <%= Html.ValidationMessage("Notes", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="RequestSupported">
                        <asp:Literal ID="DealRequestSupported" runat="server" Text="<%$ Resources:LabelResource, DealRequestSupported %>" />:</label>
                    <%= Html.TextArea("RequestSupported")%>
                    <%= Html.ValidationMessage("RequestSupported", "*")%>
                </div>
                <div class="field">
                    <label for="Details">
                        <%=ViewData["ProductLabel"]%> Details:</label>
                    <%= Html.TextArea("Details")%>
                    <%= Html.ValidationMessage("Details", "*") %>
                </div>
            </div>
            <div class="line">
               <div class="field">
               <%IList<Library> libraries=(IList<Library>)ViewData["Libraries"]; 
                 if( libraries!=null && libraries.Count>0 )
                 {%>
                Files Attached:
                <%}%>
               </div>
            </div>
            <div class="line">
               <div class="field">
                     <ul>
                        <% if (libraries == null)
                               libraries = new List<Library>();
                   
                            foreach (Library library in libraries)
                            {
                                if (!string.IsNullOrEmpty(library.FileName))
                                {%>
                        <li>
                            <a href="javascript: void(0);" onclick="javascript: return downloadFile('<%= Url.Action("Download", "Events") + "/" + library.Id %>');"><%= Html.Encode(library.FileName)%></a>
                        </li>
                        <%       }
                            } %>
                    </ul>
                </div>
            </div>
          <div class="line">
                <div class="field">
                    <%=Html.MultiUploadControl() %>
                </div>
            </div>
            <div class="buttonLink">
                <input class="shortButton2" style=" margin-left:10px;" type='submit' value='Save' />
                <input class="shortButton2" type='button' value='Reset' onclick="javascript: resetFormFields('#<%= formId %>'); resetMultiSelect();" />
                <input class="shortButton2" type='button' value='Cancel' onclick="javascript: location.href = '<%= Url.Action("Index", "DealSupport") %>'" />
            </div>
            <% } %>
        </fieldset>
    </div>
    <div id="FileNotFound">
        <br />
    </div>
</asp:Content>
<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
    <% Html.RenderPartial("Options"); %>
</asp:Content>
