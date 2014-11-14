<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Kit>" %>
<% string formId = ViewData["Scope"].ToString() + "KitEditForm"; %>
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
        var options = $('#KitIndustryIds').prop('options');
        $('#KitIndustryIds')[0].options.length = 0;
        for (j = 0; j < arrayIndustries.length; j++) {

            var arrayCompet = arrayIndustries[j].split(":");
            if (arrayCompet[2] == 'True') {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
            }
            else {
                options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
            }
        }
        $("#KitIndustryIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
        
    };

    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryKit');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#KitEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };


    $(window).bind('resize', function() {

        $('#KitIndexTwo').width($(window).width() - 9);

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
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummaryKit');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#KitEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
    
    
    $(window).bind('resize', function() {

        $('#kitIndexTwo').width($(window).width() - 9);

    });
</script>
<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DueDateFrm']);
        ResizeHeightForm();
        updateCheckByHierarchy();
    });
</script>
<script type="text/javascript">
    var ubfcompetitors = '<%= Url.Action("GetMasiveCompetitors", "Kit")%>'; //set for portability
    var ubfproducts = '<%= Url.Action("GetMasiveProducts", "Kit")%>'; //set for portability
    var ubfhierarchy = '<%= Url.Action("ChangeIndustryList", "Kit") %>'; //set for hierarchy industries

    //utility functions
    function updateUpHeight(reference) {
        var newheight = ($(reference)[0].options.length) * 16 + 10;
        $(reference).css('height', newheight + "px");
    }

    var validatelist = function() {

    updateUpHeight('#KitIndustryIds');
    updateUpHeight('#KitCompetitorIds');
    updateUpHeight('#KitProductIds');

    fixOptionTitle("#KitIndustryIds");
    fixOptionTitle("#KitCompetitorIds");
    fixOptionTitle("#KitProductIds");

    } 
    
    var updateProd = function() {
        var realvaluesIndustry = [];
        var realvaluesProduct = [];
        var realvaluesCompetitor = [];
        $('#KitProductIds :selected').each(function(i, selected) {
            realvaluesProduct[i] = $(selected).val();
        });
        $('#KitCompetitorIds :selected').each(function(i, selected) {
            realvaluesCompetitor[i] = $(selected).val();
        });
        $('#KitIndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
        });
        $('#KitProductIds')[0].options.length = 0;
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

        $('#KitIndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });
        if (realvaluesIndustry == "") {
            $('#KitCompetitorIds')[0].options.length = 0;
            $('#KitProductIds')[0].options.length = 0;
        } else {
            $('#KitCompetitorIds :selected').each(function(i, selected) {
                realvaluesCompetitor[i] = $(selected).val();
                realtextsCompetitor[i] = $(selected).text();
            });
            $('#KitProductIds :selected').each(function(i, selected) {
                realvaluesProduct[i] = $(selected).val();
            });
            $('#KitCompetitorIds')[0].options.length = 0;
            $('#KitProductIds')[0].options.length = 0;

            for (i = 0; i < realvaluesIndustry.length; i++) {

                setValuesForCompetitors(realvaluesIndustry[i], realvaluesCompetitor, realvaluesProduct);
            }

        }
    };


    function setValuesForCompetitors(id, realvaluesCompetitor, realvaluesProduct) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var url = '<%= Url.Action("GetCompetitorsOfIndustry", "Kit")%>/' + id;
        $.get(
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
        $('#KitIndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });

        var arrayComppetitors = [];
        arrayComppetitors = results.split("_");
        for (j = 0; j < arrayComppetitors.length; j++) {
            var options = $('#KitCompetitorIds').prop('options');
            var arrayCompet = arrayComppetitors[j].split(":");
            var actual = $("select#KitCompetitorIds").children().map(function() { return $(this).val(); }).get();
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

        var count2 = ($('#KitCompetitorIds')[0].options.length) * 16 + 10;
        var count3 = ($('#KitProductIds')[0].options.length) * 16 + 10;

        $('.contentscrollableC select').css('height', count2 + "px");
        $('.contentscrollableP select').css('height', count3 + "px");

        $("#KitCompetitorIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });
        $("#KitProductIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });   
    }

    function setValuesForProductsOfCompetitor(id, realvaluesProduct, realvaluesIndustry) {
        var xmlhttp;
        var parameters = { Id: id };
        var results = null;
        var idKit = $('#Id').val();
        var idsIndustries = realvaluesIndustry;
        var url = '<%= Url.Action("GetProductsOfCompetitor", "Kit")%>/' + id + '?idKit=' + idKit + '&idsIndustries=' + idsIndustries;
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
            var options = $('#KitProductIds').prop('options');
            var arrayProd = arrayProducts[j].split(":");
            var actual = $("select#KitProductIds").children().map(function() { return $(this).val(); }).get();
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


        var count3 = ($('#KitProductIds')[0].options.length) * 16 + 10;        
        $('.contentscrollableP select').css('height', count3 + "px");
       
        $("#KitProductIds option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
        });   
    }
    function resetMultiSelect() {
        $('#KitCompetitorIds')[0].options.length = 0;
        $('#KitProductIds')[0].options.length = 0;

        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        var realvaluesCompetitor = [];
        var realvaluesProduct = [];

        realvaluesCompetitor = $('#OldKitCompetitorsIds').val().split(",");
        realvaluesProduct = $('#OldKitProductsIds').val().split(",");

        $('#KitIndustryIds :selected').each(function(i, selected) {
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
    updateUpHeight('#KitIndustryIds');
    updateUpHeight('#KitCompetitorIds');
    updateUpHeight('#KitProductIds');

    fixOptionTitle("#KitIndustryIds");
    fixOptionTitle("#KitCompetitorIds");
    fixOptionTitle("#KitProductIds");
}); 

</script>

<div id="ValidationSummaryKit">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "KitEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DueDateFrm']);loadMiniHtmlEditor(); ResizeHeightForm();reloadOtherGrid('" + ViewData["Scope"] + "', 'Kit','ByParent');reloadOtherGrid('" + ViewData["Scope"] + "', 'Kit','WithAchieved'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Kit', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");validatelist(); updateCheckByHierarchy();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div  id="kitIndexTwo" class="indexTwo">

    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Kit', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("OldKitIndustriesIds")%>
        <%= Html.Hidden("OldKitCompetitorsIds")%>
        <%= Html.Hidden("OldKitProductsIds")%>
        <%= Html.Hidden("checkedbyHierarchy")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="KitEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="KitName" runat="server" Text="<%$ Resources:LabelResource, KitName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="KitAssignedTo" runat="server" Text="<%$ Resources:LabelResource, KitAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CreatedByFrm">
                        <asp:Literal ID="KitCreatedByFrm" runat="server" Text="<%$ Resources:LabelResource, KitOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByFrm", "*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>KitIndustryId" style="float:left;">
                        <asp:Literal ID="KitIndustryId" runat="server" Text="<%$ Resources:LabelResource, KitIndustryId %>" />:</label>
                        <%= Html.CheckBox("CheckIndustryIds", (bool)ViewData["CheckIndustryIds"], new { onclick = "javascript: ShowIndustriesByHierarchy('#Kit',this);", Style = "float:left;margin-left:5px;height:25px;" })%><label for="CheckIndustryIds" >&nbsp;By Hierarchy</label>
                    <div class='contentscrollableI'>
                    <%= @Html.ListBox("KitIndustryIds", (MultiSelectList)ViewData["IndustryIdMultiList"], new { Multiple = "true", onchange = "javascript: updCompetitors('#Kit');", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                    </div>
                </div>
                <div class="field">
                    <label for="<%= formId %>KitCompetitorId">
                        <asp:Literal ID="KitCompetitorId" runat="server" Text="<%$ Resources:LabelResource, KitCompetitorId %>" />:</label>
                    <div class='contentscrollableC'>
                    <%= @Html.ListBox("KitCompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", onchange = "javascript: updProducts('#Kit');", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                    </div>
                </div>
                <div class="field">
                    <label for="<%= formId %>KitProductId">
                        <asp:Literal ID="KitProductId" runat="server" Text="<%$ Resources:LabelResource, KitProductId %>" />:</label>
                    <div class='contentscrollableP'>
                    <%= @Html.ListBox("KitProductIds", (MultiSelectList)ViewData["ProductIdList"], new { Multiple = "true", @class = "fileListForm" })%>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                    </div>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>DueDateFrm">
                        <asp:Literal ID="KitDueDate" runat="server" Text="<%$ Resources:LabelResource, KitDueDate %>" />:</label>
                    <%= Html.TextBox("DueDateFrm", null, new { id = formId + "DueDateFrm" })%>
                    <%= Html.ValidationMessage("DueDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="KitType" runat="server" Text="<%$ Resources:LabelResource, KitType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>FinancialBudgetUnit">
                    <asp:Literal ID="KitFinancialBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ObjectiveFinancialBudgetUnit %>" /></label>
                    <%= Html.TextBox("FinancialBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("FinancialBudgetUnit", "*")%>
                </div>
            </div>
            
                 <div class="line">              
                <div class="field">
                    <label for="<%= formId %>TotalFinancialBudget">
                    <asp:Literal ID="KitTotalFinancialBudget" runat="server" Text="<%$ Resources:LabelResource, ObjectiveTotalFinancialBudget %>" /></label>
                    <%= Html.TextBox("TotalFinancialBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalFinancialBudget", "*")%>
                </div>          
               <div class="field">
                 <label for="<%= formId %>TimeBudgetUnit">
                    <asp:Literal ID="KitTimeBudgetUnit" runat="server" Text="<%$ Resources:LabelResource, ObjectiveTimeBudgetUnit %>" /></label>
                    <%= Html.TextBox("TimeBudgetUnit", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TimeBudgetUnit", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>TotalTimeBudget">
                    <asp:Literal ID="KitTotalTimeBudget" runat="server" Text="<%$ Resources:LabelResource, ObjectiveTotalTimeBudget %>" /></label>
                    <%= Html.TextBox("TotalTimeBudget", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("TotalTimeBudget", "*")%>
                </div>
                </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Parent">
                        <asp:Literal ID="KitParent" runat="server" Text="<%$ Resources:LabelResource, KitParent %>" />:</label>
                    <%= Html.DropDownList("Parent", (SelectList)ViewData["KitParentList"], string.Empty, new { id = formId + "Parent" })%>
                    <%= Html.ValidationMessage("Parent", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="KitStatus" runat="server" Text="<%$ Resources:LabelResource, KitStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Comment">
                        <asp:Literal ID="KitComment" runat="server" Text="<%$ Resources:LabelResource, KitComment %>" />:</label>
                    <%= Html.TextArea("Comment", new { id = formId + "Detail" })%>
                    <%= Html.ValidationMessage("Comment", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="KitMetaData" runat="server" Text="<%$ Resources:LabelResource, KitMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        
        </div>
    </fieldset>
</div>
<% } %>