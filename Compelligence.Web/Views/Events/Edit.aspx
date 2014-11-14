<%@ Page Title="Compelligence - Events" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Event>" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/thickbox.css") %>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/FeedBack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type=" /javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>">  </script>   
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script> 
    <% string formId = "EventForm"; %>
<style type="text/css">
    #contentleft
    {
        width: 70%; 	 
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
        var ShowIndustriesByHierarchy = function(chxbox) {
            var realvaluesIndustry = [];
            var textvaluesIndustry = [];
            var xmlhttp;
            var results = null;
            $('#IndustryIds :selected').each(function(i, selected) {
                realvaluesIndustry[i] = $(selected).val();
                textvaluesIndustry[i] = $(selected).text();
            });
            var parameters = { IsChecked: chxbox.checked, IndustryIds: realvaluesIndustry };
            $.get(
            '<%= Url.Action("ChangeIndustryList", "Events") %>',
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        addNewIndustriesToList(results);
                    }
                }
            });
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
    </script>
    <script type="text/javascript">
        $(function() {
            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetEventTypes", "Events")%>',
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
        });

        var findValueEventTypeCallback = function(event, data, formatted) {
            $('#EventTypeId').val(data.Value);
            //loadCompetitorData();
        };

        var visibleTime = function() {
        var selectedValue = $('#TimeFrameFrm > option:selected').prop('value');

            if (selectedValue != '') {
                if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Year %>') {
                    $('#StartIntervalDateForm').css("display", "block");
                    $('#StartDateForm').css("display", "none");
                    $('#EndDateForm').css("display", "none");
                    $('#YearQuarterForm').css("display", "none");
                }
                else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.StartEndDate %>') {
                    $('#StartIntervalDateForm').css("display", "none");
                    $('#StartDateForm').css("display", "block");
                    $('#EndDateForm').css("display", "block");
                    $('#YearQuarterForm').css("display", "none");
                }
                else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.SpecificDate %>') {
                    $('#StartIntervalDateForm').css("display", "none");
                    $('#StartDateForm').css("display", "block");
                    $('#EndDateForm').css("display", "none");
                    $('#YearQuarterForm').css("display", "none");
                }
                else if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Quarter %>' || '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Month %>') {
                    $('#StartIntervalDate').css("display", "block");
                    $('#StartDateForm').css("display", "none");
                    $('#EndDateForm').css("display", "none");
                    $('#YearQuarterForm').css("display", "block");
                }
            }
            else 
            {
                $('#StartIntervalDateForm').css("display", "none");
                $('#StartDateForm').css("display", "none");
                $('#EndDateForm').css("display", "none");
                $('#YearQuarterForm').css("display", "none");
            }

        };

        var visibleLink = function() {
        var url = $('#<%=formId %>Url').prop('value');
            if (url != '') {
                $('#ProductShowLink').css("display", "block");
            }
        };
    
        var initializeForm = function(formId, urscas, dateFormFields) {
            if (urscas == '<%= UserSecurityAccess.Read %>') {
                disableFormFields(formId);
            }
            // If there are date fields in this form, so Datepicker will be applied
            if (dateFormFields != null) {
                for (var i = 0; i < dateFormFields.length; i++) {
                    $(formId + ' input[type=text][name=' + dateFormFields[i] + ']').datepicker();
                }
            }

            visibleLink();
        };
          
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', ["StartDateFrm", "EndDateFrm"]); 
        });
        
        var showDateForm = function(objCombo) {                    
            objStartIntervalDate = document.getElementById('StartIntervalDateForm');
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
                    if(selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.SpecificDate %>')
                    {
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
                    else
                    {
                    objStartDateForm.style.display = "none";
                    objStartIntervalDate.style.display = "block";
                    objStartSelectQuarter.style.display = "none";
                    objStartSelectMonth.style.display = "none";
                    objStartSelectYear.style.display = "none";
                    objStartSelectDateSpecific.style.display = "none";
                    objStartSelectDateStartEnd.style.display = "none";
                    getCascadeObjects('<%= Url.Action("GetStartDateByTimeFrame", "Events") %>', '#TimeFrameFrm', '#EventForm [name=StartIntervalDate]', '#StartIntervalDateLoader');
                        if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Quarter %>') 
                        {
                            objStartSelectQuarter.style.display = "block";
                            objYearQuarterForm.style.display = "block";
                        }
                        if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Year %>') 
                        {
                            objStartSelectYear.style.display = "block";
                            objYearQuarterForm.style.display = "none";
                        }
                        if (selectedDate == '<%= Compelligence.Domain.Entity.Resource.EventTypePeriod.Month %>') 
                        {
                            objStartSelectMonth.style.display = "block";
                            objYearQuarterForm.style.display = "block";
                        }
                    }
                }
            }
        }


        $(function() 
        {
            visibleTime();
            updateCheckByHierarchy();
        });

        var loadUrl = function() {
        var url = $('#<%=formId %>Url').prop('value');
            if (url != '') {
                if (url.indexOf("http://") == -1) {
                    url = "http://" + url;
                }
                window.open(url, "Url", "scrollbars=1,width=900,height=500")
            }
        };
</script>

<script type="text/javascript">
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
        var url = '<%= Url.Action("GetCompetitorsOfIndustry", "Events")%>/' + id;
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
        var url = '<%= Url.Action("GetProductsOfCompetitor", "Events")%>/' + id + '?idEvent=' + idEvent + '&idsIndustries=' + idsIndustries;
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
<script type="text/javascript">
    $(function() {
                                     
        var count1 = ('<%= ViewData["IndustryIdMultiListCount"]%>') * 16 + 10;
        var count2 = ($('#CompetitorIds')[0].options.length) * 16 + 10;
        var count3 = ($('#ProductIds')[0].options.length) * 16 + 10;

        $('.contentscrollableI select').css('height', count1 + "px");
        $('.contentscrollableC select').css('height', count2 + "px");
        $('.contentscrollableP select').css('height', count3 + "px");

        $("#IndustryIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
        $("#CompetitorIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
        $("#ProductIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
    }); 
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% string formId = "EventForm"; %>
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <% Html.RenderPartial("FrontEndFormMessages"); %>
    <div> 
        <div class="headerContentRightMenu">
        
        <%--Edit  --%>
        <label for="<%= formId %>Edit">
        <asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:LabelResource, EventsEditEdit %>" />
        </label>
        </div>
        <fieldset class="contentRightMenu">
        <%= Html.ValidationSummary() %>
        <% using (Html.BeginForm((string)ViewData["ActionMethod"], "Events", FormMethod.Post, new { id = formId, enctype = "multipart/form-data" }))
           { %>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("OldIndustriesIds")%>
        <%= Html.Hidden("OldCompetitorsIds")%>
        <%= Html.Hidden("OldProductsIds")%>
        <%= Html.Hidden("checkedbyHierarchy")%>
        <div class="line">
            <div class="field">
                <%= Html.Hidden("Id",default(decimal)) %>
                <label for="EventName" class="required">
                    <asp:Literal ID="EventName" runat="server" Text="<%$ Resources:LabelResource, EventCaseName %>" />:</label>
                <%= Html.TextBox("EventName")%>
                <%= Html.ValidationMessage("EventName", "*")%>
            </div>
        </div>

        <div class="line">
            <div class="field">
                <label for="<%= formId %>IndustryId">
                    <asp:Literal ID="EventIndustryId" runat="server" Text="" /><%=ViewData["IndustryLabel"]%>:
                </label>
				<label for="CheckIndustryIds" >
				<%= Html.CheckBox("CheckIndustryIds", (bool)ViewData["CheckIndustryIds"], new { onclick = "ShowIndustriesByHierarchy(this);", Style = "float:left;margin-left:0px;height:15px;" })%>
					&nbsp;By Hierarchy
				</label>                    
              <div class='contentscrollableI'> 
                <%= @Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript: updateCompAndProd(this);", @class = "fileListForm" })%>
                <%= Html.ValidationMessage("IndustryId", "*")%>
              </div>
            </div>
            <div class="field">
                <label>&nbsp;</label>
                <label for="<%= formId %>CompetitorId">
                    <asp:Literal ID="EventCompetitorId" runat="server" Text="" /><%=ViewData["CompetitorLabel"]%>:</label>
                <div class='contentscrollableC'> 
                    <%= @Html.ListBox("CompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updateProd(this);", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                </div>
            </div>
            <div class="field">
                <label>&nbsp;</label>
                <label for="<%= formId %>ProductId">
                    <asp:Literal ID="EventProductId" runat="server" Text="" /><%=ViewData["ProductLabel"]%>:</label>
                <div class='contentscrollableP'> 
                    <%=@Html.ListBox("ProductIds", (MultiSelectList)ViewData["ProductIdList"], new { Multiple = "true", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                </div>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="Confidence" class="required">
                    <asp:Literal ID="EventConfidence" runat="server" Text="<%$ Resources:LabelResource, Confidence %>" />:</label>
                <%= Html.DropDownList("Confidence", (SelectList)ViewData["EventConfidenceList"], string.Empty, new { Class = "cmbStandart"})%>
                <%= Html.ValidationMessage("Confidence", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="Severity">
                    <asp:Literal ID="EventSeverity" runat="server" Text="<%$ Resources:LabelResource, Severity %>" />:</label>
                <%= Html.DropDownList("Severity", (SelectList)ViewData["EventSeverityList"], string.Empty, new { Class = "cmbStandart" })%>
                <%= Html.ValidationMessage("Severity", "*")%>
            </div>
            <div class="field">
                <label for="<%= formId %>Location">
                    <asp:Literal ID="EventLocation" runat="server" Text="<%$ Resources:LabelResource, EventLocation %>" />:</label>
                <%= Html.TextBox("Location", null, new { id = formId + "Location", Class = "TextBox2" })%>
                <%= Html.ValidationMessage("Location", "*")%>
            </div>
        </div>
        
        <div class="line">
            <div class="field">
                <label for="TimeFrameFrm" class="required">
                    <asp:Literal ID="EventTimeFrame" runat="server" Text="<%$ Resources:LabelResource, TimeFrame %>" />:</label>
                <%= Html.DropDownList("TimeFrameFrm", (SelectList)ViewData["EventPeriodList"], string.Empty, new { onchange = "javascript: showDateForm(this);" , Class = "cmbStandart"})%>
                <%= Html.ValidationMessage("TimeFrameFrm", "*")%>
            </div>
            <div class="field">
                <label for="<%=formId %>Url">
                    <asp:Literal ID="EventUrl" runat="server" Text="<%$ Resources:LabelResource, EventUrl %>"></asp:Literal>:</label>
                <%= Html.TextBox("Url", null, new { id = formId + "Url", Ondblclick = "loadUrl();", Class = "TextBox" })%>
                <%= Html.ValidationMessage("Url","*") %>
                <div id="ProductShowLink" style="display: none">
                    <a href="javascript:void(0);" onclick="loadUrl();">Go To URL</a>
                </div>
            </div>
            <div class="field">
                <label for="EventTypeName">
                    <asp:Literal ID="EventEventTypeName" runat="server" Text="<%$ Resources:LabelResource, EventType %>" />:</label>
                <%= Html.TextBox("EventTypeName", null, new { Class = "TextBox" })%>
                <%= Html.ValidationMessage("EventTypeName", "*")%>
                <%= Html.Hidden("EventTypeId")%>
            </div>
        </div>
        <div class="line">
            <div class="field" style="padding-bottom: 0px; margin-bottom: -7px;">
             <%--<div id="DateStartEnd" style="display:none"><label style="font-weight: normal;">Please indicate the dates for start and end</label></div>
             <div id="DateSpecific" style="display:none"><label style="font-weight: normal;">Please indicate the Specific Date</label></div>
             <div id="DateQuarter" style="display:none"><label style="font-weight: normal;">Pick the Start Interval Date of the quarter and a year</label></div> 
             <div id="DateMonth" style="display:none"><label style="font-weight: normal;">Pick the Start Interval Date of the month</label></div>
             <div id="DateYear" style="display:none"><label style="font-weight: normal;">Pick the Start Interval Date of the year</label></div>--%>
             <div id="DateStartEnd" style="display:none"><label class="labelInfo">Please indicate the dates for start and end</label></div>
             <div id="DateSpecific" style="display:none"><label class="labelInfo">Please indicate the Specific Date</label></div>
             <div id="DateQuarter" style="display:none"><label class="labelInfo">Pick the Start Interval Date of the quarter and a year</label></div> 
             <div id="DateMonth" style="display:none"><label class="labelInfo">Pick the Start Interval Date of the month</label></div>
             <div id="DateYear" style="display:none"><label class="labelInfo">Pick the Start Interval Date of the year</label></div>
            </div>
         </div>
        <div class="line">
                             
            <div class="field" id="StartIntervalDateForm" >
                <label for="StartIntervalDate" class="required">
                    <asp:Literal ID="EventStartIntervalDate" runat="server" Text="<%$ Resources:LabelResource, EventStartIntervalDate %>" />:</label>
                <%= Html.CascadingChildDropDownList("StartIntervalDate", (SelectList)ViewData["StartDateList"], string.Empty)%>
                <%= Html.ValidationMessage("StartIntervalDate", "*")%>
            </div>
            <div class="field" id="YearQuarterForm" style="display: none" >
                <label for="<%= formId %>YearQuarter" class="required">
                    <asp:Literal ID="EventYearQuarter" runat="server" Text="<%$ Resources:LabelResource, EventYearQuarter %>" />:</label>
                <%= Html.CascadingChildDropDownList("YearQuarter", (SelectList)ViewData["YearQuarterList"], string.Empty, formId)%>
                <%= Html.ValidationMessage("YearQuarter", "*")%>
            </div>
            <div class="field" id="StartDateForm">
                <label for="<%= formId %>StartDateFrm" class="required">
                    <asp:Literal ID="EventStartDate" runat="server" Text="<%$ Resources:LabelResource, EventStartDate %>" />:</label>
                <%= Html.TextBox("StartDateFrm", null, new { id = formId + "StartDateFrm", Class = "Date" })%>
                <%= Html.ValidationMessage("StartDateFrm", "*")%>
            </div>
            <div class="field" id="EndDateForm">
                <label for="<%= formId %>EndDateFrm" class="required">
                    <asp:Literal ID="EventEndDate" runat="server" Text="<%$ Resources:LabelResource, EventEndDate %>" />:</label>
                <%= Html.TextBox("EndDateFrm", null, new { id = formId + "EndDateFrm", Class = "Date" })%>
                <%= Html.ValidationMessage("EndDateFrm", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="Scenario" class="required">
                    <asp:Literal ID="Scenario" runat="server" Text="<%$ Resources:LabelResource, Scenario %>" />:</label>
                <%= Html.TextArea("Scenario", new { Class = "TextArea3" })%>
                <%= Html.ValidationMessage("Scenario", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="MarketImpact">
                    <asp:Literal ID="MarketImpact" runat="server" Text="<%$ Resources:LabelResource, MarketImpact %>" />:</label>
                <%= Html.TextArea("MarketImpact", new { Class = "TextArea3" })%>
                <%= Html.ValidationMessage("MarketImpact", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="CompanyImpact">
                    <asp:Literal ID="CompanyImpact" runat="server" Text="<%$ Resources:LabelResource, CompanyImpact %>" />:</label>
                <%= Html.TextArea("CompanyImpact", new { Class = "TextArea3" })%>
                <%= Html.ValidationMessage("CompanyImpact", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="RecommendedActions">
                    <asp:Literal ID="RecommendedActions" runat="server" Text="<%$ Resources:LabelResource, RecommendedActions %>" />:</label>
                <%= Html.TextArea("RecommendedActions", new { Class = "TextArea3" })%>
                <%= Html.ValidationMessage("RecommendedActions", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="Comment">
                    <asp:Literal ID="Comment" runat="server" Text="<%$ Resources:LabelResource, Comment %>" />:</label>
                <%= Html.TextArea("Comment", new { Class = "TextArea3" })%>
                <%= Html.ValidationMessage("Comment", "*")%>
            </div>
        </div>
        
        
        <div class="line">
            <div class="field">
               <%IList<Library> libraries=(IList<Library>)ViewData["Libraries"]; 
                 if( libraries!=null && libraries.Count>0 )
                 {%>
                 
                 <%--Files Attached:  --%>
                 <label for="FilesAttached">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:LabelResource, EventsEditFilesAttached %>" />:
                 </label>
                
                
                <%}%>
            </div>
         </div>   
         <div class="line">
            <div class="field">   
                <ul>
                    <% if (libraries == null)
                           libraries = new List<Library>();
                        foreach (Library library in libraries){ %>
                    <li>
                        <a href="javascript: void(0);" onclick="javascript: return downloadFile('<%= Url.Action("Download", "Events") + "/" + library.Id %>');"><%= Html.Encode(library.FileName) %></a>
                    </li>
                    <% } %>
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
            <input class="shortButton2" type='button' value='Reset' onclick="javascript: resetFormFields('#<%= formId %>');resetMultiSelect();" />
            <input class="shortButton2" type='button' value='Cancel' onclick="javascript: location.href = '<%= Url.Action("Index", "Events") %>'" />
        </div>
        <% } %>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
<% Html.RenderPartial("Options"); %>
</asp:Content>