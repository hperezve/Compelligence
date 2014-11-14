<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Event>" %>
<% string formId = ViewData["Scope"].ToString() + "EventEditForm"; %>
       <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
       <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Retrieve.js") %>"></script>
<style>
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
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryEvent');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#EventEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
    
    
    function addNewIndustriesToList(results) {
        var i = $(this);
        i.queue(function() {
            setTimeout(function() {
                i.dequeue();
            }, 1000);
        });

        var arrayIndustries = [];
        arrayIndustries = results.split("_");
        var options = $('#IndustryIds').prop('options');        
        $('#IndustryIds')[0].options.length = 0;
        for (j = 0; j < arrayIndustries.length; j++) {
            var arrayCompet = arrayIndustries[j].split(":");
            if (arrayCompet[2] == 'True') {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
            }
            else {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
            }
        }
        $("#IndustryIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });        
    };

    var validatelist = function() {

        updateUpHeight('#IndustryIds');
        updateUpHeight('#CompetitorIds');
        updateUpHeight('#ProductIds');

        fixOptionTitle("#IndustryIds");
        fixOptionTitle("#CompetitorIds");
        fixOptionTitle("#ProductIds");

    } 
    
    $(window).bind('resize', function() {
        $('#eventIndexTwo').width($(window).width() - 9);
    });
</script>
<script type="text/javascript">
    var loadUrl = function() {
    var url = $('#<%=formId %>Url').prop('value');
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "Url", "scrollbars=1,width=900,height=500")
        }
    };

    var AutoCompleteType = function() {
        $.ajax({
            type: "POST",
            url: '<%= Url.Action("GetEventTypes", "Event")%>',
            dataType: "json",
            success: function(data) {
                $("#EventTypeName").autocomplete(data, {
                    matchContains: true,
                    minChars: 0,
                    max: 200,
                    formatItem: function(row, i, max) {
                        return row.Text;
                    }
                });
            }
        });
        $('#EventTypeName').result(findValueEventTypeCallback);
    };
</script>

<script type="text/javascript">

    $(function() {
        AutoCompleteType();
    });

    function findValueEventTypeCallback(event, data, formatted) {
        $('#EventTypeId').val(data.Value);
    }

    var visibleTime = function() {
        //    var selectedValue = $('#TimeFrameFrmSelect > option:selected').attr('value');
    var selectedValue = $('#<%= formId %>TimeFrameFrm > option:selected').prop('value');
        if (selectedValue != '') {
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Year %>') {
                $('#StartIntervalDate').css("display", "block");
                $('#StartDateForm').css("display", "none");
                $('#EndDateForm').css("display", "none");
                $('#YearQuarterForm').css("display", "none");
            }
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.StartEndDate %>') {
                $('#StartIntervalDate').css("display", "none");
                $('#StartDateForm').css("display", "block");
                $('#EndDateForm').css("display", "block");
                $('#YearQuarterForm').css("display", "none");
            }
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.SpecificDate %>') {
                $('#StartIntervalDate').css("display", "none");
                $('#StartDateForm').css("display", "block");
                $('#EndDateForm').css("display", "none");
                $('#YearQuarterForm').css("display", "none");
            }
            if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Quarter %>' || selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Month %>') {
                $('#StartIntervalDate').css("display", "block");
                $('#StartDateForm').css("display", "none");
                $('#EndDateForm').css("display", "none");
                $('#YearQuarterForm').css("display", "block");
            }
        }
    };

    var visibleLink = function() {
    var url = $('#<%=formId %>Url').prop('value');
        if (url != '') {
            $('#EventShowLink').css("display", "block");
        }
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
    
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>StartDateFrm', 'dt:#<%= formId %>EndDateFrm']);
        visibleTime();
        visibleLink();
        ResizeHeightForm();
        updateCheckByHierarchy();
    });

    var showDateForm = function(objCombo) {

        objStartIntervalDate = document.getElementById('StartIntervalDate');
        objStartDateForm = document.getElementById('StartDateForm');
        objEndDateForm = document.getElementById('EndDateForm');
        objYearQuarterForm = document.getElementById('YearQuarterForm');

        objStartSelectDateStartEnd = document.getElementById('DateStartEnd');
        objStartSelectDateSpecific = document.getElementById('DateSpecific');
        objStartSelectQuarter = document.getElementById('DateQuarter');
        objStartSelectMonth = document.getElementById('DateMonth');
        objStartSelectYear = document.getElementById('DateYear');

        var selectedDate = objCombo.value;
        if (selectedDate != '') {
            if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.StartEndDate %>') {

                objStartDateForm.style.display = "block";
                objEndDateForm.style.display = "block";

                objStartSelectDateStartEnd.style.display = "block";

                objStartIntervalDate.style.display = "none";

                objStartSelectQuarter.style.display = "none";
                objStartSelectMonth.style.display = "none";
                objStartSelectYear.style.display = "none";
                objStartSelectDateSpecific.style.display = "none";
                objYearQuarterForm.style.display = "none";
            }
            else {
                objEndDateForm.style.display = "none";
                if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.SpecificDate %>') {
                    objStartDateForm.style.display = "block";
                    //objEndDateForm.style.display = "block";                        
                    objStartSelectDateSpecific.style.display = "block";

                    objStartIntervalDate.style.display = "none";

                    objStartSelectDateStartEnd.style.display = "none";
                    objStartSelectQuarter.style.display = "none";
                    objStartSelectMonth.style.display = "none";
                    objStartSelectYear.style.display = "none";
                    objYearQuarterForm.style.display = "none";
                }
                else {
                    objStartDateForm.style.display = "none";
                    objStartIntervalDate.style.display = "block";
                    objStartSelectQuarter.style.display = "none";
                    objStartSelectMonth.style.display = "none";
                    objStartSelectYear.style.display = "none";
                    objStartSelectDateSpecific.style.display = "none";
                    objStartSelectDateStartEnd.style.display = "none";
                    //getCascadeObjects('/EventFrontEnd.aspx/GetStartDateByTimeFrame', '#TimeFrameFrm', '#EventForm [name=StartIntervalDate]', '#StartIntervalDateLoader');
                    getCascadeObjects('<%= Url.Action("GetStartDateByTimeFrame", "Event") %>', '#WorkspaceEventEditForm [name=TimeFrameFrm]', '#WorkspaceEventEditForm [name=StartIntervalDate]', '#WorkspaceEventEditFormStartIntervalDateLoader', [], []);
                    if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Quarter %>') {
                        objStartSelectQuarter.style.display = "block";
                        objYearQuarterForm.style.display = "block";
                    }
                    if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Year %>') {
                        objStartSelectYear.style.display = "block";
                        objYearQuarterForm.style.display = "none";
                    }
                    if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Month %>') {
                        objStartSelectMonth.style.display = "block";
                        objYearQuarterForm.style.display = "block";
                    }
                }
            }
        }
    }

    var executeReport = function(browseId) {
        var urlAction = '<%= Url.Action("Generate", "Report") %>';
        showLoadingDialog();
        $.post(urlAction, { BrowseId: browseId },
            function(data) {
                var urlReportsOut = '<%= Url.Content("~" + ConfigurationSettings.AppSettings["ReportFilePath"]) %>';
                window.open(urlReportsOut + data + '.pdf', "ReportPopup", "width=700,height=400");
                hideLoadingDialog();
            });
    };
</script>

<script type="text/javascript">

    var ubfcompetitors = '<%= Url.Action("GetMasiveCompetitors", "Event")%>'; //set for portability
    var ubfproducts = '<%= Url.Action("GetMasiveProducts", "Event")%>'; //set for portability
    var ubfhierarchy = '<%= Url.Action("ChangeIndustryList", "Event") %>'; //set for hierarchy industries
    var updateProd = function() {
        var realvaluesIndustry = [];
        var realvaluesProduct = [];
        var realvaluesCompetitor = [];
        $('#ProductIds :selected').each(function(i, selected) {
            realvaluesProduct[i] = $(selected).val();
        });
        $('#CompetitorIds :selected').each(function(i, selected) {
            realvaluesCompetitor[i] = $(selected).val();
        });
        $('#IndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
        });
        $('#ProductIds')[0].options.length = 0;
        if (realvaluesCompetitor != "") {
            for (i = 0; i < realvaluesCompetitor.length; i++) {
                setValuesForProductsOfCompetitor(realvaluesCompetitor[i], realvaluesProduct, realvaluesIndustry);
            }
        }
    };

    var updateCompAndProd = function() {
        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        var realvaluesCompetitor = [];
        var realtextsCompetitor = [];
        var realvaluesProduct = [];

        $('#IndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });
        if (realvaluesIndustry == "") {
            $('#CompetitorIds')[0].options.length = 0;
            $('#ProductIds')[0].options.length = 0;
        } else {
            $('#CompetitorIds :selected').each(function(i, selected) {
                realvaluesCompetitor[i] = $(selected).val();
                realtextsCompetitor[i] = $(selected).text();
            });
            $('#ProductIds :selected').each(function(i, selected) {
                realvaluesProduct[i] = $(selected).val();
            });
            $('#CompetitorIds')[0].options.length = 0;
            $('#ProductIds')[0].options.length = 0;

            for (i = 0; i < realvaluesIndustry.length; i++) {
                setValuesForCompetitors(realvaluesIndustry[i], realvaluesCompetitor, realvaluesProduct);
            }
        }
    };

    function setValuesForCompetitors(id, realvaluesCompetitor, realvaluesProduct) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var url = '<%= Url.Action("GetCompetitorsOfIndustry", "Event")%>/' + id;
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
        $('#IndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });

        var arrayComppetitors = [];
        arrayComppetitors = results.split("_");
        for (j = 0; j < arrayComppetitors.length; j++) {
            var options = $('#CompetitorIds').prop('options');
            var arrayCompet = arrayComppetitors[j].split(":");
            var actual = $("select#CompetitorIds").children().map(function() { return $(this).val(); }).get();
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

        //according to screen size,rearrange styles by size
        
        var count2 = ($('#CompetitorIds')[0].options.length) * 16 + 10;
        var count3 = ($('#ProductIds')[0].options.length) * 16 + 10;

        $('.contentscrollableC select').css('height', count2 + "px");
        $('.contentscrollableP select').css('height', count3 + "px");

        $("#CompetitorIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
        $("#ProductIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });        
    }

    function setValuesForProductsOfCompetitor(id, realvaluesProduct, realvaluesIndustry) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var idEvent = $('#Id').val();
        var idsIndustries = realvaluesIndustry;
        var url = '<%= Url.Action("GetProductsOfCompetitor", "Event")%>/' + id + '?idEvent=' + idEvent + '&idsIndustries=' + idsIndustries;
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
            var options = $('#ProductIds').prop('options');
            var arrayProd = arrayProducts[j].split(":");
            var actual = $("select#ProductIds").children().map(function() { return $(this).val(); }).get();
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

        var count3 = ($('#ProductIds')[0].options.length) * 16 + 10;
        $('.contentscrollableP select').css('height', count3 + "px");

        $("#ProductIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });        
    }

    function resetMultiSelect() {
        $('#CompetitorIds')[0].options.length = 0;
        $('#ProductIds')[0].options.length = 0;

        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        var realvaluesCompetitor = [];
        var realvaluesProduct = [];

        realvaluesCompetitor = $('#OldCompetitorsIds').val().split(",");
        realvaluesProduct = $('#OldProductsIds').val().split(",");

        $('#IndustryIds :selected').each(function(i, selected) {
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
    $(function() {
    updateUpHeight('#IndustryIds');
    updateUpHeight('#CompetitorIds');
    updateUpHeight('#ProductIds');



    fixOptionTitle("#IndustryIds");
    fixOptionTitle("#CompetitorIds");
    fixOptionTitle("#ProductIds");
    }); 
</script>

<div id="ValidationSummaryEvent">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "EventEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "',['dt:#" + formId + "StartDateFrm', 'dt:#" + formId + "EndDateFrm']); ResizeHeightForm(); AutoCompleteType(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Event', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");visibleLink();visibleTime();validatelist(); updateCheckByHierarchy();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = (string)ViewData["Scope"] + "EventEditForm" }))
   { %>
<div id="eventIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Event', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("OldIndustriesIds")%>
        <%= Html.Hidden("OldCompetitorsIds")%>
        <%= Html.Hidden("OldProductsIds")%>
        <%= Html.Hidden("checkedbyHierarchy")%>
        <div id="EventEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>EventName" class="required">
                        <asp:Literal ID="EventName" runat="server" Text="<%$ Resources:LabelResource, EventName %>" />:</label>
                    <%= Html.TextBox("EventName", null, new { id = formId + "EventName" })%>
                    <%= Html.ValidationMessage("EventName", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="EventStatus" runat="server" Text="<%$ Resources:LabelResource, EventStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("EventStatus", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TimeFrameFrm" class="required">
                        <asp:Literal ID="EventTimeFrameFrm" runat="server" Text="<%$ Resources:LabelResource, TimeFrame %>" />:</label>                    
                    <%= Html.DropDownList("TimeFrameFrm", (SelectList)ViewData["EventPeriodList"], string.Empty, new { onchange = "javascript: showDateForm(this);", id = formId + "TimeFrameFrm" })%>
                    <%= Html.ValidationMessage("TimeFrameFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">                    
                    <label for="<%= formId %>IndustryId" style="float:left">
                        <asp:Literal ID="EventIndustryId" runat="server" Text="<%$ Resources:LabelResource, EventIndustryId %>" />:</label>
                    <%= Html.CheckBox("CheckIndustryIds", (bool)ViewData["CheckIndustryIds"], new { onclick = "javascript: ShowIndustriesByHierarchy('#',this);", style = "float:left;margin-left:5px;height:25px; margin-right:5px;" })%><label for="CheckIndustryIds">By Hierarchy</label>
                    <div class="contentscrollableI">
                    <%= Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript: updCompetitors('#');", Style = "border: none;" })%>
                    </div>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CompetitorId">
                        <asp:Literal ID="EventCompetitorId" runat="server" Text="<%$ Resources:LabelResource, EventCompetitorId %>" />:</label>
                     <div class="contentscrollableC">
                    <%= Html.ListBox("CompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updProducts('#');", Style = "border: none;" })%>
                    </div>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>ProductId">
                        <asp:Literal ID="EventProductId" runat="server" Text="<%$ Resources:LabelResource, EventProductId %>" />:</label>
                        <div class="contentscrollableP">
                    <%= Html.ListBox("ProductIds", (MultiSelectList)ViewData["ProductIdList"], new { Multiple = "true", Style = "border: none;" })%>
                        </div>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Confidence" class="required">
                        <asp:Literal ID="EventConfidence" runat="server" Text="<%$ Resources:LabelResource, Confidence %>" />:</label>
                    <%= Html.DropDownList("Confidence", (SelectList)ViewData["EventConfidenceList"], string.Empty, new { id = formId + "Confidence" })%>
                    <%= Html.ValidationMessage("Confidence", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>MarketTypeId"><asp:Literal ID="EventMarketTiypeId" runat="server" Text="<%$ Resources:LabelResource, EventMarketTypeId %>" />:</label>
                    <%= Html.DropDownList("MarketTypeId", (SelectList)ViewData["MarketTypeList"], string.Empty, new { id=formId+"MarketTypeId"})%>
                    <%= Html.ValidationMessage("MarketTypeId","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Severity">
                        <asp:Literal ID="EventSeverity" runat="server" Text="<%$ Resources:LabelResource, Severity %>" />:</label>
                    <%= Html.DropDownList("Severity", (SelectList)ViewData["EventSeverityList"], string.Empty, new { id = formId + "Severity" })%>
                    <%= Html.ValidationMessage("Severity", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CreatedByFrm">
                        <asp:Literal ID="EventOpenedByFrm" runat="server" Text="<%$ Resources:LabelResource, EventOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Location">
                        <asp:Literal ID="EventLocation" runat="server" Text="<%$ Resources:LabelResource, EventLocation %>" />:</label>
                    <%= Html.TextBox("Location", null, new { id = formId + "Location" })%>
                    <%= Html.ValidationMessage("Location", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>AssignedTo">
                        <asp:Literal ID="EventAssignedTo" runat="server" Text="<%$ Resources:LabelResource, EventAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["EventOwnerList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%=formId %>Url">
                        <asp:Literal ID="EventUrl" runat="server" Text="<%$ Resources:LabelResource, EventUrl %>"></asp:Literal>:</label>
                    <%= Html.TextBox("Url", null, new { id = formId + "Url", Ondblclick = "loadUrl();" })%>
                    <%= Html.ValidationMessage("Url","*") %>
                    <div id="EventShowLink" style="display: none">
                        <a href="javascript:void(0);" onclick="loadUrl();">Go To URL</a>
                    </div>
                </div>
                <div class="field">
                    <label for="EventTypeName">
                        <asp:Literal ID="EventEventTypeName" runat="server" Text="<%$ Resources:LabelResource, EventType %>" />:</label>
                    <%= Html.TextBox("EventTypeName")%>
                    <%= Html.ValidationMessage("EventTypeName", "*")%>
                    <%= Html.Hidden("EventTypeId")%>
                </div>
            </div>
            <div class="line">
                <div id="DateStartEnd" style="display: none" class="fieldLine" >
                    <label>Please indicate the dates for start and end</label></div>
                <div id="DateSpecific" style="display: none" class="fieldLine">
                    <label>Please indicate the Specific Date</label></div>
                <div id="DateQuarter" style="display: none" class="fieldLine">
                    <label>Pick the Start Interval Date of the quarter and year.</label></div>
                <div id="DateMonth" style="display: none" class="fieldLine">
                    <label>Pick the Start Interval Date of the month and year.</label></div>
                <div id="DateYear" style="display: none" class="fieldLine">
                    <label>Pick the Start Interval Date of the year</label></div>
            </div>
            <div class="line">                
                <div class="field" id="StartIntervalDate" style="display: none">
                    <label for="<%= formId %>StartIntervalDate" class="required">
                        <asp:Literal ID="EventStartIntervalDate" runat="server" Text="<%$ Resources:LabelResource, EventStartIntervalDate %>" />:</label>
                    <%= Html.CascadingChildDropDownList("StartIntervalDate", (SelectList)ViewData["StartDateList"], string.Empty, formId)%>
                    <%= Html.ValidationMessage("StartIntervalDate", "*")%>
                </div>
                <div class="field" id="YearQuarterForm" style="display: none">
                    <label for="<%= formId %>YearQuarter" class="required">
                        <asp:Literal ID="EventYearQuarter" runat="server" Text="<%$ Resources:LabelResource, EventYearQuarter %>" />:</label>
                    <%= Html.CascadingChildDropDownList("YearQuarter", (SelectList)ViewData["YearQuarterList"], string.Empty, formId)%>
                    <%= Html.ValidationMessage("YearQuarter", "*")%>
                </div>
                <div class="field" id="StartDateForm" style="display: none">
                    <label for="<%= formId %>StartDateFrm" class="required">
                        <asp:Literal ID="EventStartDate" runat="server" Text="<%$ Resources:LabelResource, EventStartDate %>" />:</label>
                    <%= Html.TextBox("StartDateFrm", null, new { id = formId + "StartDateFrm" })%>
                    <%= Html.ValidationMessage("StartDateFrm", "*")%>
                </div>
                <div class="field" id="EndDateForm" style="display: none">
                    <label for="<%= formId %>EndDateFrm" class="required">
                        <asp:Literal ID="EventEndDate" runat="server" Text="<%$ Resources:LabelResource, EventEndDate %>" />:</label>
                    <%= Html.TextBox("EndDateFrm", null, new { id = formId + "EndDateFrm" })%>
                    <%= Html.ValidationMessage("EndDateFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field" style="width: 535px;">
                    <label for="<%= formId %>Scenario" class="required">
                        <asp:Literal ID="EventScenario" runat="server" Text="<%$ Resources:LabelResource, Scenario %>" />:</label>
                    <%= Html.TextArea("Scenario", null, new { id = formId + "Scenario", style = "width:822px" })%>
                    <%= Html.ValidationMessage("Scenario", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MarketImpact">
                        <asp:Literal ID="EventMarketImpact" runat="server" Text="<%$ Resources:LabelResource, MarketImpact %>" />:</label>
                    <%= Html.TextArea("MarketImpact", new { id = formId + "MarketImpact", style = "width:822px" })%>
                    <%= Html.ValidationMessage("MarketImpact", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CompanyImpact">
                        <asp:Literal ID="EventCompanyImpact" runat="server" Text="<%$ Resources:LabelResource, CompanyImpact %>" />:</label>
                    <%= Html.TextArea("CompanyImpact", new { id = formId + "CompanyImpact", style = "width:822px" })%>
                    <%= Html.ValidationMessage("CompanyImpact", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>RecommendedActions">
                        <asp:Literal ID="EventRecommendedActions" runat="server" Text="<%$ Resources:LabelResource, RecommendedActions %>" />:</label>
                    <%= Html.TextArea("RecommendedActions", new { id = formId + "RecommendedActions", style = "width:822px" })%>
                    <%= Html.ValidationMessage("RecommendedActions", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Comment">
                        <asp:Literal ID="EventComment" runat="server" Text="<%$ Resources:LabelResource, Comment %>" />:</label>
                    <%= Html.TextArea("Comment", new { id = formId + "Comment", style = "width:822px" })%>
                    <%= Html.ValidationMessage("Comment", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="EventMetaData" runat="server" Text="<%$ Resources:LabelResource, EventMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
