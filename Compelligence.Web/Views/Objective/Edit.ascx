<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Objective>" %>
<% string formId = ViewData["Scope"].ToString() + "ObjectiveEditForm"; %>
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
<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />

       <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>

    <script type="text/javascript">
        var loadMiniHtmlEditor = function() {
            $('#<%=formId%>Detail').cleditor();
            $(".cleditorMain iframe").contents().find('body').bind('keyup', function() {
                var v = $(this).text(); // or .html() if desired
                $('#<%=formId%>Detail').html(v);
            });
        };

        $(function() {
            loadMiniHtmlEditor();

        });
        
      
    </script>
<script type="text/javascript">
   

    function addNewIndustriesToList(results) {
        var i = $(this);
        i.queue(function() {
            setTimeout(function() {
                i.dequeue();
            }, 1000);
        });

        var arrayIndustries = [];
        arrayIndustries = results.split("_");
        var options = $('#ObjectiveIndustryIds').prop('options');
        $('#ObjectiveIndustryIds')[0].options.length = 0;
        for (j = 0; j < arrayIndustries.length; j++) {

            var arrayCompet = arrayIndustries[j].split(":");
            if (arrayCompet[2] == 'True') {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
            }
            else {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
            }
        }
             
        $("#ObjectiveIndustryIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
            });
            
   };

   var ResizeHeightForm = function() {
       var div = document.getElementById('ValidationSummaryObjective');
       var ul = div.getElementsByTagName('ul');
       if ((ul != null) || (ul != undefined)) {
           var lis = div.getElementsByTagName('li');
           if (lis.length > 0) {
               var newHeigth = 328 - 58 - (10 * lis.length);
               var edt = $('#ObjectiveEditFormInternalContent').css('height', newHeigth + 'px');
           }
       }
   };
   var ubfcompetitors = '<%= Url.Action("GetMasiveCompetitors", "Kit")%>'; //set for portability
   var ubfproducts = '<%= Url.Action("GetMasiveProducts", "Kit")%>'; //set for portability
   var ubfhierarchy = '<%= Url.Action("ChangeIndustryList", "Kit") %>'; //set for hierarchy industries

   //utility functions
   function updateUpHeight(reference) {
       var newheight = ($(reference)[0].options.length) * 16 + 10;
       $(reference).css('height', newheight + "px");
   }
    var validatelist = function() {

    updateUpHeight('#ObjectiveIndustryIds');
    updateUpHeight('#ObjectiveCompetitorIds');
    updateUpHeight('#ObjectiveProductIds');

    fixOptionTitle("#ObjectiveIndustryIds");
    fixOptionTitle("#ObjectiveCompetitorIds");
    fixOptionTitle("#ObjectiveProductIds");
      
     
    }

    $(window).bind('resize', function() {

        $('#objectiveIndexTwo').width($(window).width() - 9);
        
    });

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
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DueDateFrm']);
        ResizeHeightForm();
        updateCheckByHierarchy();
    });
</script>

<script type="text/javascript">
    var updateProd = function() {
        var realvaluesIndustry = [];
        var realvaluesProduct = [];
        var realvaluesCompetitor = [];
        $('#ObjectiveProductIds :selected').each(function(i, selected) {
            realvaluesProduct[i] = $(selected).val();
        });
        $('#ObjectiveCompetitorIds :selected').each(function(i, selected) {
            realvaluesCompetitor[i] = $(selected).val();
        });
        $('#ObjectiveIndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
        });
        $('#ObjectiveProductIds')[0].options.length = 0;
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

        $('#ObjectiveIndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });
        if (realvaluesIndustry == "") {
            $('#ObjectiveCompetitorIds')[0].options.length = 0;
            $('#ObjectiveProductIds')[0].options.length = 0;
        } else {
        $('#ObjectiveCompetitorIds :selected').each(function(i, selected) {
                realvaluesCompetitor[i] = $(selected).val();
                realtextsCompetitor[i] = $(selected).text();
            });
            $('#ObjectiveProductIds :selected').each(function(i, selected) {
                realvaluesProduct[i] = $(selected).val();
            });
            $('#ObjectiveCompetitorIds')[0].options.length = 0;
            $('#ObjectiveProductIds')[0].options.length = 0;

            for (i = 0; i < realvaluesIndustry.length; i++) {

                setValuesForCompetitors(realvaluesIndustry[i], realvaluesCompetitor, realvaluesProduct);
            }

        }
       
    };


    function setValuesForCompetitors(id, realvaluesCompetitor, realvaluesProduct) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var url = '<%= Url.Action("GetCompetitorsOfIndustry", "Objective")%>/' + id;
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
        $('#ObjectiveIndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });

        var arrayComppetitors = [];
        arrayComppetitors = results.split("_");
        for (j = 0; j < arrayComppetitors.length; j++) {
            var options = $('#ObjectiveCompetitorIds').prop('options');
            var arrayCompet = arrayComppetitors[j].split(":");
            var actual = $("select#ObjectiveCompetitorIds").children().map(function() { return $(this).val(); }).get();
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

        var count2 = ($('#ObjectiveCompetitorIds')[0].options.length) * 16 + 10;
        var count3 = ($('#ObjectiveProductIds')[0].options.length) * 16 + 10;

        $('.contentscrollableC select').css('height', count2 + "px");
        $('.contentscrollableP select').css('height', count3 + "px");

        $("#ObjectiveCompetitorIds option").each(function() {
            $(this).prop("title", $(this).text());
            $(this).prop("style", "height:16px;");
        });
        $("#ObjectiveProductIds option").each(function() {
            $(this).prop("title", $(this).text());
            $(this).prop("style", "height:16px;");
        });   
      
       
    }

    function setValuesForProductsOfCompetitor(id, realvaluesProduct, realvaluesIndustry) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var idObjective = $('#Id').val();
        var idsIndustries = realvaluesIndustry;
        var url = '<%= Url.Action("GetProductsOfCompetitor", "Objective")%>/' + id + '?idObjective=' + idObjective + '&idsIndustries=' + idsIndustries;
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
            var options = $('#ObjectiveProductIds').prop('options');
            var arrayProd = arrayProducts[j].split(":");
            var actual = $("select#ObjectiveProductIds").children().map(function() { return $(this).val(); }).get();
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

       var count3 = ($('#ObjectiveProductIds')[0].options.length) * 16 + 10;
        $('.contentscrollableP select').css('height', count3 + "px");      
        $("#ObjectiveProductIds option").each(function() {
            $(this).prop("title", $(this).text());
            $(this).prop("style", "height:16px;");
        });   
      
       
    }

    function resetMultiSelect() {
        $('#ObjectiveCompetitorIds')[0].options.length = 0;
        $('#ObjectiveProductIds')[0].options.length = 0;

        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        var realvaluesCompetitor = [];
        var realvaluesProduct = [];

        realvaluesCompetitor = $('#OldCompetitorsIds').val().split(",");
        realvaluesProduct = $('#OldProductsIds').val().split(",");

        $('#ObjectiveIndustryIds :selected').each(function(i, selected) {
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
        updateUpHeight('#ObjectiveIndustryIds');
        updateUpHeight('#ObjectiveCompetitorIds');
        updateUpHeight('#ObjectiveProductIds');

        fixOptionTitle("#ObjectiveIndustryIds");
        fixOptionTitle("#ObjectiveCompetitorIds");
        fixOptionTitle("#ObjectiveProductIds");
    }); 

</script>

<div id="ValidationSummaryObjective">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "ObjectiveEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DueDateFrm']); loadMiniHtmlEditor();ResizeHeightForm();reloadOtherGrid('" + ViewData["Scope"] + "','Objective','ByParent'); reloadOtherGrid('" + ViewData["Scope"] + "','Objective','WithAchieved');executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Objective', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + "); validatelist(); updateCheckByHierarchy();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div id="objectiveIndexTwo" class="indexTwo">
    <fieldset>
        <legend><%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Objective', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
        <%= Html.Hidden("checkedbyHierarchy")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="ObjectiveEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="ObjectiveName" runat="server" Text="<%$ Resources:LabelResource, ObjectiveName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name"})%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="ObjectiveAssignedTo" runat="server" Text="<%$ Resources:LabelResource, ObjectiveAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CreatedByFrm">
                        <asp:Literal ID="ObjectiveCreatedByFrm" runat="server" Text="<%$ Resources:LabelResource, ObjectiveOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByFrm", "*") %>
                </div>   
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="ObjectiveStatus" runat="server" Text="<%$ Resources:LabelResource, ObjectiveStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DueDateFrm">
                        <asp:Literal ID="ObjectiveDueDate" runat="server" Text="<%$ Resources:LabelResource, ObjectiveDueDate %>" />:</label>
                    <%= Html.TextBox("DueDateFrm", null, new { id = formId + "DueDateFrm" })%>
                    <%= Html.ValidationMessage("DueDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="ObjectiveType" runat="server" Text="<%$ Resources:LabelResource, ObjectiveType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>ObjectiveIndustryId" style="float:left">
                        <asp:Literal ID="ObjectiveIndustryId" runat="server" Text="<%$ Resources:LabelResource, ObjectiveIndustry %>" />:</label>
                        <%= Html.CheckBox("CheckIndustryIds", (bool)ViewData["CheckIndustryIds"], new { onclick = "ShowIndustriesByHierarchy('#Objective',this);", Style = "float:left;margin-left:5px;height:25px;" })%><label for="CheckIndustryIds" >&nbsp;By Hierarchy</label>
                    <div class='contentscrollableI'> 
                    <%= @Html.ListBox("ObjectiveIndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript: updCompetitors('#Objective');", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                    </div>
                </div>
                <div class="field">
                    <label for="<%= formId %>ObjectiveCompetitorId">
                        <asp:Literal ID="ObjectiveCompetitorId" runat="server" Text="<%$ Resources:LabelResource, ObjectiveCompetitor %>" />:</label>
                    <div class='contentscrollableC'> 
                    <%= @Html.ListBox("ObjectiveCompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updProducts('#Objective');", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                    </div>
                </div>
                <div class="field">
                    <label for="<%= formId %>ObjectiveProductId">
                        <asp:Literal ID="ObjectiveProductId" runat="server" Text="<%$ Resources:LabelResource, ObjectiveProduct %>" />:</label>
                    <div class='contentscrollableP'> 
                    <%= @Html.ListBox("ObjectiveProductIds", (MultiSelectList)ViewData["ProductIdList"], new { Multiple = "true", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Parent">
                        <asp:Literal ID="ObjectiveParent" runat="server" Text="<%$ Resources:LabelResource, ObjetiveParent %>" />:</label>
                    <%= Html.DropDownList("Parent", (SelectList)ViewData["ObjetiveParentList"], string.Empty, new { id = formId + "Parent" })%>
                    <%= Html.ValidationMessage("Parent", "*")%>
                </div>
             <%--   <div class="field">
                    <label for="<%= formId %>Budget">
                        <asp:Literal ID="ObjectiveBudget" runat="server" Text="<%$ Resources:LabelResource, ObjectiveBudget %>" />:</label>
                    <%= Html.TextBox("BudgetFrm", null, new { id = formId + "Budget" })%>
                    <%= Html.ValidationMessage("BudgetFrm", "*")%>
                </div>
--%>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>FinancialBudgetUnit">
                    <asp:Literal ID="ObjectiveFinancialBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ObjectiveFinancialBudgetUnit %>" /></label>
                    <%= Html.TextBox("FinancialBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("FinancialBudgetUnit", "*")%>
                </div>


                <div class="field">
                    <label for="<%= formId %>TotalFinancialBudget">
                    <asp:Literal ID="ObjectiveTotalFinancialBudget" runat="server" Text="<%$ Resources:LabelResource, ObjectiveTotalFinancialBudget %>" /></label>
                    <%= Html.TextBox("TotalFinancialBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalFinancialBudget", "*")%>
                </div>
             </div>
             <div class="line">
               <div class="field">
                 <label for="<%= formId %>TimeBudgetUnit">
                    <asp:Literal ID="ObjectiveTimeBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ObjectiveTimeBudgetUnit %>" /></label>
                    <%= Html.TextBox("TimeBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TimeBudgetUnit", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TotalTimeBudget">
                    <asp:Literal ID="ObjectiveTotalTimeBudget" runat="server" Text="<%$ Resources:LabelResource, ObjectiveTotalTimeBudget %>" /></label>
                    <%= Html.TextBox("TotalTimeBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalTimeBudget", "*")%>
                </div>
                </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Detail">
                        <asp:Literal ID="ObjectiveDetail" runat="server" Text="<%$ Resources:LabelResource, ObjectiveDetail %>" />:</label>
                    <%= Html.TextArea("Detail", new { id = formId + "Detail" })%>
                    <%= Html.ValidationMessage("Detail", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="ObjectiveMetaData" runat="server" Text="<%$ Resources:LabelResource, ObjectiveMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
