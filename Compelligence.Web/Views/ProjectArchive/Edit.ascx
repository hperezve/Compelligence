<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Project>" %>
<% string formId = ViewData["Scope"].ToString() + "ProjectArchiveEditForm"; %>

<script type="text/javascript">

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
            '<%= Url.Action("ChangeIndustryList", "ProjectArchive") %>',
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
    };
    
    var getHeight = function() {
        var div = document.getElementById('ValidationSummaryProject');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var edt = $('#ProjectArchiveEditFormInternalContent');
                var tempo = 328 - 58 - (10 * lis.length);
                var edt = $('#ProjectArchiveEditFormInternalContent').css('height', tempo + 'px');
            }
        }
    };


    var validatelist = function() {

    var count1 = '<%= ViewData["IndustryIdMultiListCount"]%>';
    var count2 = ($('#CompetitorIds')[0].options.length) * 15;
    var count3 = ($('#ProductIds')[0].options.length) * 15;
    $('.contentscrollableI select').css('height', count1 + "px");
    $('.contentscrollableC select').css('height', count2 + "px");
    $('.contentscrollableP select').css('height', count3 + "px");
  

    } 
   
   
   
    $(function() {
    initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DueDateFrm']);
    getHeight();
    });
    
    $(window).bind('resize', function() {

    $('#projectIndexTwo').width($(window).width() - 9);

    });
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
        var url = '<%= Url.Action("GetCompetitorsOfIndustry", "ProjectArchive")%>/' + id;
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
        i.queue(function(){   
            setTimeout(function(){   
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
        var count2 = ($('#CompetitorIds')[0].options.length) * 15;
        var count3 = ($('#ProductIds')[0].options.length) * 15;
        $('.contentscrollableC select').css('height', count2 + "px");
        $('.contentscrollableP select').css('height', count3 + "px");
     }


    function setValuesForProductsOfCompetitor(id, realvaluesProduct, realvaluesIndustry) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var idProject = $('#Id').val();
        var idsIndustries = realvaluesIndustry;
        var url = '<%= Url.Action("GetProductsOfCompetitor", "ProjectArchive")%>/' + id + '?idProject=' + idProject + '&idsIndustries=' + idsIndustries;
        $.get(
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

        
        var count3 = ($('#ProductIds')[0].options.length) * 15;        
        $('.contentscrollableP select').css('height', count3 + "px");
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

    function confirmDueDate() {
        if ($('#ToolsProjectArchiveEditFormDueDateFrm').val() != "") {
            var dateNow = new Date();
            var dayNow = dateNow.getDate();
            var MonthNow = dateNow.getMonth() + 1;
            var yearNow = dateNow.getFullYear();
            var textDateNow = MonthNow + '/' + dayNow + '/' + yearNow;

            var currentDate = new Date(textDateNow);
            var dueDateFrm = new Date($('#ToolsProjectArchiveEditFormDueDateFrm').val());

            if (dueDateFrm < currentDate) {

                $("#ChooseConfirmDueDate").dialog("open");
            }
        }
    }
    
</script>

<script type="text/javascript">
    $(function() {
    var count1 = '<%= ViewData["IndustryIdMultiListCount"]%>';
    var count2 = ($('#CompetitorIds')[0].options.length) * 15;
    var count3 = ($('#ProductIds')[0].options.length) * 15;
    $('.contentscrollableI select').css('height', count1 + "px");
    $('.contentscrollableC select').css('height', count2 + "px");
    $('.contentscrollableP select').css('height', count3 + "px");
  
  
   }); 

</script>

<div id="ValidationSummaryProject">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "ProjectArchiveEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DueDateFrm']);getHeight(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'ProjectArchive', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");validatelist ();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="projectIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'ProjectArchive', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OriginalStatus")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("OldIndustryId")%>
        <%= Html.Hidden("OldCompetitorId")%>
        <%= Html.Hidden("OldProductId")%>
        <%= Html.Hidden("OldIndustriesIds")%>
        <%= Html.Hidden("OldCompetitorsIds")%>
        <%= Html.Hidden("OldProductsIds")%>
        <%= Html.Hidden("OldAssignedTo")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="ProjectEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="ProjectName" runat="server" Text="<%$ Resources:LabelResource, ProjectName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="ProjectAssignedTo" runat="server" Text="<%$ Resources:LabelResource, ProjectAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CreatedByFrm">
                        <asp:Literal ID="ProjectCreatedByFrm" runat="server" Text="<%$ Resources:LabelResource, ProjectOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByFrm", "*") %>
                </div>                
            </div>
            <div class="line">
                <div class="field">
                <label for="<%= formId %>IndustryId" style=" float:left;">
                        <asp:Literal ID="ProjectIndustryId" runat="server" Text="<%$ Resources:LabelResource, ProjectIndustryId %>" />:</label>
                    <%= Html.CheckBox("CheckIndustryIds", new { onclick = "ShowIndustriesByHierarchy(this);", style="float:left;margin-left:5px;height:25px;"})%><label for="CheckIndustryIds" style=" margin-right:5px;">By Hierarchy</label>
                    
                        
                        <%SelectList l = (SelectList)ViewData["IndustryIdList"];%>
                    <div class="contentscrollableI">
                    <%= Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript: updateCompAndProd(this);" })%>
                    </div>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CompetitorId">
                        <asp:Literal ID="ProjectCompetitorId" runat="server" Text="<%$ Resources:LabelResource, ProjectCompetitorId %>" />:</label>
                        <div class="contentscrollableC">
                    <%= Html.ListBox("CompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updateProd(this);" })%>
                    </div>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>ProductId">
                        <asp:Literal ID="ProductId" runat="server" Text="<%$ Resources:LabelResource, ProjectProductId %>" />:</label>
                        <div class="contentscrollableP">
                    <%= Html.ListBox("ProductIds", (MultiSelectList)ViewData["ProductIdList"], new {  Multiple = "true" })%>
                    </div>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="ProjectStatus" runat="server" Text="<%$ Resources:LabelResource, ProjectStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DueDateFrm">
                        <asp:Literal ID="ProjectDueDate" runat="server" Text="<%$ Resources:LabelResource, ProjectDueDate %>" />:</label>
                    <%= Html.TextBox("DueDateFrm", null, new { id = formId + "DueDateFrm" , onchange = "javascript: confirmDueDate();"})%>
                    <%= Html.ValidationMessage("DueDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="ProjectType" runat="server" Text="<%$ Resources:LabelResource, ProjectType %>" />:</label>
                    <%= Html.DropDownList("ContentTypeId", (SelectList)ViewData["ContentTypeList"], string.Empty, new { id = formId + "Type"})%>
                    <%= Html.ValidationMessage("ContentTypeId", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CreatedDateFrm">
                        <asp:Literal ID="DealCreatedDateFrm" runat="server" Text="<%$ Resources:LabelResource, ProjectCreatedDate %>" />:</label>
                    <%= Html.TextBox("CreatedDateFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>FinancialBudgetUnit">
                    <asp:Literal ID="ProjectFinancialBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ProjectFinancialBudgetUnit %>" /></label>
                    <%= Html.TextBox("FinancialBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("FinancialBudgetUnit", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TotalFinancialBudget">
                    <asp:Literal ID="ProjectTotalFinancialBudget" runat="server" Text="<%$ Resources:LabelResource, ProjectTotalFinancialBudget %>" /></label>
                    <%= Html.TextBox("TotalFinancialBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalFinancialBudget", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>TimeBudgetUnit">
                    <asp:Literal ID="ProjectTimeBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ProjectTimeBudgetUnit %>" /></label>
                    <%= Html.TextBox("TimeBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TimeBudgetUnit", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TotalTimeBudget">
                    <asp:Literal ID="ProjectTotalTimeBudget" runat="server" Text="<%$ Resources:LabelResource, ProjectTotalTimeBudget %>" /></label>
                    <%= Html.TextBox("TotalTimeBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalTimeBudget", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>MarketTypeId"><asp:Literal ID="ProjectMarketTiypeId" runat="server" Text="<%$ Resources:LabelResource, ProjectMarketTypeId %>" />:</label>
                    <%= Html.DropDownList("MarketTypeId", (SelectList)ViewData["MarketTypeList"], string.Empty, new { id=formId+"MarketTypeId"})%>
                    <%= Html.ValidationMessage("MarketTypeId","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Visibility">
                        <asp:Literal ID="ProjectVisibility" runat="server" Text="<%$ Resources:LabelResource, ProjectVisibility %>" />:</label>
                    <%= Html.DropDownList("Visibility", (SelectList)ViewData["VisibilityList"], new { id = formId + "Visibility" })%>
                    <%= Html.ValidationMessage("Visibility", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description"><asp:Literal ID="ProjectDescription" runat="server" Text="<%$ Resources:LabelResource, ProjectDescription %>"></asp:Literal>:</label>
                    <%= Html.TextArea("Description", new  { id = formId+"Description"})%>
                    <%= Html.ValidationMessage("Description","*") %>
                </div>

            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="ProjectMetaData" runat="server" Text="<%$ Resources:LabelResource, ProjectMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
