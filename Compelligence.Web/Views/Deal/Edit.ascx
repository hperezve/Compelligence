<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Deal>" %>
<% string formId = ViewData["Scope"].ToString() + "DealEditForm"; %>
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/thickbox.css") %>" rel="stylesheet"
        type="text/css" />
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>
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


    var AddNewCustomerContact = function(text) {
        var customerContact = $("#WorkspaceDealEditFormCustomerContact");
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
        var customerContact = $("#WorkspaceDealEditFormCustomerContact");
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
        var customerContact = $("#WorkspaceDealEditFormCustomerContact");
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
                //customerId[0].value = ''
                //previousCustomerId[0].value = '';
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

    //migrate update
    function SetValuesEmployees()  //reset employees to default
    {
        var selectedEmployees = $('#oldDealEmployeeIds').prop('value');
        updateOptionStatus("#DealEmployeeIds", selectedEmployees, true);
    }

    //migrate update
    function SetSelectedEndUserIdMultiList() {
        selectedEndUsers = $('#UsersIdsHidden').prop('value');
        updateOptionStatus("#UsersIds", selectedEndUsers, true);
    }

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


    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryDeal');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#DealEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };

    var validatelist = function() {

        updateUpHeight('#ProductIds');
        updateUpHeight('#CompetitorsIds');
        updateUpHeight('#IndustryIds');

        fixOptionTitle("#IndustriesIds");
        fixOptionTitle("#CompetitorsIds");
        fixOptionTitle("#ProductIds");

    } 
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
                        //                        var customerId = $("#CustomerId");
                        //                        customerId[0].value = row.Value;
                        //                        var customerValue = $("#CustomerValue");
                        //                        customerValue[0].value = row.Text;
                        return row.Text;
                    },
                    formatResult: function(row) {
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

        $("#CustomerName").blur(function() {
            if ($(this).val() == '') {
                $("#CustomerId").val('');
            }
        });
    };
    $(window).bind('resize', function() {

        $('#dealIndexTwo').width($(window).width() - 9);

    });
    $(function() {
        disabledEndUsers();
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DueDateFrm']);
        showWinning('#<%= formId %>Phase');
        hideMe('#WinningCompetitorList');
        ResizeHeightForm();
        AutoCompleteCustomer();
        SetValuesEmployees();
        SetSelectedEndUserIdMultiList();
        updateCheckByHierarchy();
    });
</script>


<script type="text/javascript">
    var ubfcompetitors = '<%= Url.Action("GetMasiveCompetitors", "Deal")%>'; //set for portability
    var ubfproducts = '<%= Url.Action("GetMasiveProducts", "Deal")%>'; //set for portability
    var ubfhierarchy = '<%= Url.Action("ChangeIndustryList", "Deal") %>'; //set for hierarchy industries




    var updateComboPrimaryIndustry = function() {

        var fId = '<%=ViewData["Scope"] %>DealEditForm';

        var realIndustries = getOptionPairs('#IndustryIds', true); //{Text,Value},...
        var valPrimary = getOptionValues('#' + fId + 'PrimaryIndustry', true);

        $('#' + fId + 'PrimaryIndustry')[0].options.length = 0;
        $('#' + fId + 'PrimaryIndustry').prepend("<option value=''></option>");

        var options = $('#' + fId + 'PrimaryIndustry')[0].options;
        appendOption(options, realIndustries, valPrimary);

    };
//
    var updateComboPrimaryCompetitor = function(realCompetitors, idtarget) 
    {
      var valPrimary = getOptionValues(idtarget, true);
      $(idtarget)[0].options.length = 0;
      $(idtarget).prepend("<option value=''></option>");
      var options = $(idtarget)[0].options;
      appendOption(options, realCompetitors, valPrimary);
    };

    //
    //
    function resetMultiSelect() {
        $('#CompetitorIds')[0].options.length = 0;
        $('#ProductIds')[0].options.length = 0;

        var competitorids = $('#OldCompetitorsIds').val().split(",");
        var productids = $('#OldProductsIds').val().split(",");
        var industryids = getOptionValues('#IndustryIds', true); //???????

        updCompetitorMasiveList('#',industryids, competitorids, productids);

        //remember get only products of competitors selected
        setTimeout(function() {
            //console.log("Ps:" + productids); //only for debug
            updProductMasiveList('#', competitorids, productids, industryids)
        }, 1000);
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
            updateComboPrimaryCompetitor(realcompetitors, '#WorkspaceDealEditFormPrimaryCompetitor');
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

<div id="ValidationSummaryDeal">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "DealEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DueDateFrm']);ResizeHeightForm(); AutoCompleteCustomer(); SetValuesEmployees(); SetSelectedEndUserIdMultiList(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Deal', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");validatelist(); updateCheckByHierarchy();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="dealIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Deal', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("OldIndustriesIds")%>
        <%= Html.Hidden("OldCompetitorsIds")%>
        <%= Html.Hidden("OldProductsIds")%>
        <%= Html.Hidden("OldPhase")%>
        <%= Html.Hidden("OldPrimaryCompetitor")%>
        <%= Html.Hidden("OldPrimaryIndustry")%>
        <%= Html.Hidden("OldUsersIds")%>
        <%= Html.Hidden("oldDealEmployeeIds")%>
        <%= Html.Hidden("UsersIdsHidden")%>
        <%= Html.Hidden("checkedbyHierarchy")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="DealEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required" >
                    <% string stylea = string.Empty;
                        stylea = "width:540px;";%>                        
                        <asp:Literal ID="DealName" runat="server" Text="<%$ Resources:LabelResource, DealName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name", @style = stylea })%>                    
                    <%= Html.ValidationMessage("Name", "*")%> 
                   
                </div>
                
            </div>
            <div class="line">
             <div class="field">
                    <label for="<%= formId %>CreatedByName">
                        <asp:Literal ID="DealOpenedByFrm" runat="server" Text="<%$ Resources:LabelResource, DealOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByName", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByName", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CurrencyId">
                        <asp:Literal ID="DealCurrencyId" runat="server" Text="<%$ Resources:LabelResource, DealCurrencyId %>" />:</label>
                    <%= Html.TextBox("CurrencyId", null, new { id = formId + "CurrencyId", onkeypress = "return isNumbersAndPointKeyOrValue(event, this, true)" })%>
                    <%= Html.ValidationMessage("CurrencyId", "*")%>
                    <div id="divMessageAlert" style="display: none; position: absolute;">
                        <label id="lblMessageAlert" style="color: rgb(255, 0, 0); font-size:smaller;">System will only accept a number or empty</label><br />
                    </div>
                </div>
            </div>
            <div class="line">
              <%--<div class="field">
                    <label for="ExternalId">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, DealExternalId %>" />:</label>
                    <%= Html.TextBox("ExternalId", null, new { id = formId + "ExternalId",Readonly="readonly" })%>
              </div>--%>
               <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="DealAssignedTo" runat="server" Text="<%$ Resources:LabelResource, DealAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="DealStatus" runat="server" Text="<%$ Resources:LabelResource, DealStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
             <div class="line">
                <div class="field">                              
                    <label for="<%= formId %>IndustryIds"  style="float:left" class="required">
                        <asp:Literal ID="IndustryIds" runat="server" Text="<%$ Resources:LabelResource, DealIndustryId %>" />:</label>                                                
                    <%= Html.CheckBox("CheckIndustryIds", (bool)ViewData["CheckIndustryIds"], new { onclick = "javascript: ShowIndustriesByHierarchy('#',this);", Style = "float:left;margin-left:5px;height:25px;" })%><label for="CheckIndustryIds">&nbsp;By Hierarchy</label>                                            
                    <div class="contentscrollableI">   

                    <%= @Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript: updCompetitors('#');updateComboPrimaryIndustry();", @class = "fileListForm" })%>

                    </div>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CompetitorIds" class="required">
                        <asp:Literal ID="CompetitorIds" runat="server" Text="<%$ Resources:LabelResource, DealCompetitorId %>" />:</label>
                    <div class="contentscrollableC">                           

                    <%= @Html.ListBox("CompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updProducts('#');updPrimaries();", @class = "fileListForm" })%>

                    </div>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>ProductId">
                        <asp:Literal ID="DealProductId" runat="server" Text="<%$ Resources:LabelResource, DealProductId %>" />:</label>
                    <div class="contentscrollableP">                           
                    <%= @Html.ListBox("ProductIds", (MultiSelectList)ViewData["ProductIdList"], new { Multiple = "true", @class = "fileListForm" })%>
                    </div>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="CustomerName">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:LabelResource, DealCustomerId %>" />:</label>
                    <%= Html.TextBox("CustomerName", null)%>
                    <%= Html.ValidationMessage("CustomerName", "*") %>
                    <%= Html.Hidden("CustomerId") %>
                    <%= Html.Hidden("PreviousCustomerId") %>
                    <%= Html.Hidden("CustomerValue") %>
                </div>
                <div>
                    <%= Html.ListBox("CustomerNameHidden", (SelectList)ViewData["EmployeesByCustomerList"], new { style = "display:none" })%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DealEmployeeId">
                        <asp:Literal ID="DealDealEmployeeId" runat="server" Text="<%$ Resources:LabelResource, DealEmployeeIdsOfCustomer %>" />:</label>
                    <%= Html.ListBox("DealEmployeeIds", (MultiSelectList)ViewData["EmployeeIdList"], new { id = "DealEmployeeIds", size = 5, style = "height:150px", Multiple = "true", onchange = "javascript:AddEmployeesToCustomerContact(this);" })%>
                    <%= Html.ValidationMessage("DealEmployeeIds", "*")%>
                    <%= Html.Hidden("EmployeeIdsSelected") %>
                    <%= Html.Hidden("NewCustomerContact") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>CustomerContact">
                        <asp:Literal ID="DealCustomerContact" runat="server" Text="<%$ Resources:LabelResource, DealCustomerContact %>" />:</label>
                    <%= Html.TextArea("CustomerContact", null, new { id = formId + "CustomerContact", style = "height:150px;width:247px", onblur = "javascript:AddNewCustomerContact();" })%>
                    <%= Html.ValidationMessage("CustomerContact", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CompetitorDiscount">
                        <asp:Literal ID="DealCompetitorDiscount" runat="server" Text="<%$ Resources:LabelResource, DealCompetitorDiscount %>" />:</label>
                    <%= Html.TextBox("CompetitorDiscount", null, new { id = formId + "CompetitorDiscount" })%>
                    <%= Html.ValidationMessage("CompetitorDiscount", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CompetitorAccountManager">
                        <asp:Literal ID="DealCompetitorAccountManager" runat="server" Text="<%$ Resources:LabelResource, DealCompetitorAccountManager %>" />:</label>
                    <%= Html.TextBox("CompetitorAccountManager", null, new { id = formId + "CompetitorAccountManager" })%>
                    <%= Html.ValidationMessage("CompetitorAccountManager", "*")%>
                </div>
                 <div class="field">
                    <label for="<%= formId %>CreatedDateFrm">
                        <asp:Literal ID="DealCreatedDateFrm" runat="server" Text="<%$ Resources:LabelResource, AnswerDate %>" />:</label>
                    <%= Html.TextBox("CreatedDateFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedDateFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>PrimaryIndustry">
                        <asp:Literal ID="DealPrimaryIndustry" runat="server" Text="<%$ Resources:LabelResource, DealPrimaryIndustry %>" />:</label>
                    <%= Html.DropDownList("PrimaryIndustry", (SelectList)ViewData["SelectedIndustryIdList"], string.Empty, new { id = formId + "PrimaryIndustry" })%>
                    <%= Html.ValidationMessage("PrimaryIndustry", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>PrimaryCompetitor">
                        <asp:Literal ID="DealPrimaryCompetitor" runat="server" Text="<%$ Resources:LabelResource, DealPrimaryCompetitor %>" />:</label>
                    <%= Html.DropDownList("PrimaryCompetitor", (SelectList)ViewData["SelectedCompetitorIdList"], string.Empty, new { id = formId + "PrimaryCompetitor" })%>
                    <%= Html.ValidationMessage("PrimaryCompetitor", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DueDateFrm" class="required">
                        <asp:Literal ID="DealDueDateFrm" runat="server" Text="<%$ Resources:LabelResource, DealDueDate %>" />:</label>
                    <%= Html.TextBox("DueDateFrm", null, new { id = formId + "DueDateFrm" })%>
                    <%= Html.ValidationMessage("DueDateFrm", "*")%>
                </div>   
            </div>
             <div class="line">
             <div class="field">
                    <label for="<%= formId %>Phase">
                        <asp:Literal ID="DealPhase" runat="server" Text="<%$ Resources:LabelResource, DealPhase %>" />:</label>
                    <%= Html.DropDownList("Phase", (SelectList)ViewData["PhaseList"], string.Empty, new { id = formId + "Phase", onchange = "javascript: showWinning(this);" })%>
                    <%= Html.ValidationMessage("Phase", "*")%>
                </div>
                <div id="WinningCompetitorField" class="field">
                    <label for="<%= formId %>WinningCompetitor">
                        <asp:Literal ID="DealWinning" runat="server" Text="<%$ Resources:LabelResource, DealWinning %>" />:</label>
                    <%= Html.TextBox("WinningCompetitor", null, new { id = "WinningCompetitor", onclick="javascript: toggleCombo('WinningCompetitorList');", onkeyup="javascript: updateCombo(this,'WinningCompetitorList');" })%><br />
                    <%= Html.ListBox("WinningCompetitorList", (SelectList)ViewData["SelectedCompetitorIdList"], new { style = "width:248px; height:150px;", id = "WinningCompetitorList", onchange = "javascript: updateText('WinningCompetitorList','WinningCompetitor')" })%>
                    <%= Html.ValidationMessage("WinningCompetitor", "*")%>
                </div>
                 <div class="field">
                    <label for="<%= formId %>UsersIds">
                        <asp:Literal ID="DealUsersIds" runat="server" Text="<%$ Resources:LabelResource, DealEndUserIds %>" />:</label>
                    <%= Html.DropDownList("UsersIds", (MultiSelectList)ViewData["EndUserIdMultiList"], new { size = 5, style = "height:150px", Multiple = "true" })%>
                    <%= Html.ValidationMessage("UsersIds", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>MarketTypeId"><asp:Literal ID="DealMarketTiypeId" runat="server" Text="<%$ Resources:LabelResource, DealMarketTypeId %>" />:</label>
                    <%= Html.DropDownList("MarketTypeId", (SelectList)ViewData["MarketTypeList"], string.Empty, new { id=formId+"MarketTypeId"})%>
                    <%= Html.ValidationMessage("MarketTypeId","*") %>
                </div>
             </div>
             <div class="line">
             </div>
             <div class="line">
                <div class="field">
                    <label for="<%= formId %>CompetitorAccountStrategy">
                        <asp:Literal ID="DealCompetitorAccountStrategy" runat="server" Text="<%$ Resources:LabelResource, DealCompetitorAccountStrategy %>" />:</label>
                    <%= Html.TextArea("CompetitorAccountStrategy", new { id = formId + "CompetitorAccountStrategy" })%>
                    <%= Html.ValidationMessage("CompetitorAccountStrategy", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Details">
                        <asp:Literal ID="DealDetails" runat="server" Text="<%$ Resources:LabelResource, DealDetails %>" />:</label>
                    <%= Html.TextArea("Details", new { id = formId + "Details" })%>
                    <%= Html.ValidationMessage("Details", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>RequestSupported">
                        <asp:Literal ID="DealRequestSupported" runat="server" Text="<%$ Resources:LabelResource, DealRequestSupported %>" />:</label>
                    <%= Html.TextArea("RequestSupported", new { id = formId + "RequestSupported" })%>
                    <%= Html.ValidationMessage("RequestSupported", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Notes">
                        <asp:Literal ID="DealNotes" runat="server" Text="<%$ Resources:LabelResource, DealNotes %>" />:</label>
                    <%= Html.TextArea("Notes", new { id = formId + "Notes" })%>
                    <%= Html.ValidationMessage("Notes", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="DealMetaData" runat="server" Text="<%$ Resources:LabelResource, DealMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                    <%= Html.ValidationMessage("MetaData", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>    
<% } %>
