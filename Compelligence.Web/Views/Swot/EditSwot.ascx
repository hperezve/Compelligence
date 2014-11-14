<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Swot>" %>
<% string formId = ViewData["Scope"].ToString() + "SwotEditForm"; %>
<style>
  .miDiv ul 
    {
       list-style-position   :inside;
       list-style-type:disc;
    }
      .miDiv ol 
    {
	   list-style-type: decimal ;
       list-style-position   :inside;
 
    }
    .midlefield
    {
        float:left;
        margin-left:10px;
        padding:7px 8px 5px 0;
        width:31%;
    }
    .line
    {
        clear:both;
        display:table-row;
        float:left;
        width:100%;
    }
    .ui-multiselect-checkboxes
    {
        font-size:1.3em;
    }
    
</style>
<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>

<script type="text/javascript">
    var displayMultiselect = function() {
        $("#IndustryId").multiselect({
            multiple: false,
            noneSelectedText: 'Select an Industry',
            selectedList: 1,
            clas_ajust: "adjust-textc",
            minWidth: 200,
            uncheckAll: function() {
                var MySelectIndustryObject = $("input[name=multiselect_IndustryId]:checked");
                var MySelectObject = $("input[name=multiselect_CompetitorId]:checked");
                if (changeSelect == 'ChangeCompetiror') {
                    setValuesForCompetitors2('-1', MySelectObject.val());
                    timeChangeSelectIndustry('-1', MySelectObject.val());
                    GetActionsByIndustry(MySelectIndustryObject, MySelectObject);

                }
                else {
                    setValuesForCompetitors2('-1', '-1');
                    timeChangeSelectIndustry('-1', '-1');
                    GetActionsByIndustry(MySelectIndustryObject, MySelectObject);
                }
            },
            click: function(event, ui) {
                var MySelectIndustry = $("input[name=multiselect_IndustryId]:checked").val();
                var MySelect = $("input[name=multiselect_CompetitorId]:checked").val();
                var MySelectIndustryObject = $("input[name=multiselect_IndustryId]:checked");
                var MySelectObject = $("input[name=multiselect_CompetitorId]:checked");
                setValuesForCompetitors2(MySelectIndustry, MySelect);
                timeChangeSelectIndustry(MySelectIndustry, MySelect);
                GetActionsByIndustry(MySelectIndustryObject, MySelectObject);
            },
            classes: "auto fixed"
        }).multiselectfilter();

        $("#CompetitorId").multiselect({
            multiple: false,
            noneSelectedText: 'Select a Competitor',
            uncheckAll: function() {
                var MySelectIndustryObject = $("input[name=multiselect_IndustryId]:checked");
                var MySelectObject = $("input[name=multiselect_CompetitorId]:checked");
                if (changeSelect == 'ChangeIndustry') {
                    setValuesForIndustry('-1', MySelectIndustryObject.val());
                    timeChangeSelectIndustry(MySelectIndustryObject.val(), '-1');
                    GetSWByCompetitor(MySelectIndustryObject, MySelectObject);

                }
                else {
                    setValuesForIndustry('-1', '-1');
                    timeChangeSelectIndustry('-1', '-1');
                    GetSWByCompetitor(MySelectIndustryObject, MySelectObject);

                }
            },
            click: function(event, ui) {
                var MySelectIndustry = $("input[name=multiselect_IndustryId]:checked").val();
                var MySelect = $("input[name=multiselect_CompetitorId]:checked").val();
                var MySelectIndustryObject = $("input[name=multiselect_IndustryId]:checked");
                var MySelectObject = $("input[name=multiselect_CompetitorId]:checked");
                setValuesForIndustry(MySelect, MySelectIndustry);
                timeChangeSelectIndustry(MySelectIndustry, MySelect);
                GetSWByCompetitor(MySelectIndustryObject, MySelectObject);
            },
            selectedList: 1,
            clas_ajust: "adjust-textc",
            minWidth: 200,
            classes: "auto fixed"
        }).multiselectfilter();
    }
    var changeSelect = "changeSelect";
    var htmlEncode = function(value) {
        return $('<div/>').text(value).html();
    };
    var htmlDecode = function(value) {
        return $('<div/>').html(value).text();
    };
    function ConvertTheText() {
        //        alert($('#so').val());
        //        alert($('#wo').val());
        //        alert($('#st').val());
        //        alert($('#wt').val());


        $('#SO').val(htmlEncode($('#SO').val()));
        $('#WO').val(htmlEncode($('#WO').val()));
        $('#ST').val(htmlEncode($('#ST').val()));
        $('#WT').val(htmlEncode($('#WT').val()));
    };
    function countChar(val) {
        var len = val.value.length;
        if (len >= 200) {
            val.value = val.value.substring(0, 200);
        } else {
            $('#charNum').text(200 - len);
        }
    };
    function ShowAlert(dialogTitle, message) {
        var dialogObject = $("#AletShoeMessageTemplateDialog");
        //Competitor and Industry is required
        var errorMessage = '<p><span class="ui-icon ui-icon-alert alertFailedResponseDialog"></span>' + message + '</p>';
        dialogObject.html(errorMessage);
        dialogObject.dialog({ autoOpen: false,
            title: dialogTitle,
            width: 360,
            modal: true,
            buttons: { "Ok": function() {
                $(this).dialog("destroy");
            }
            }
        });
        //commentObject.html(dialogContent);
        dialogObject.dialog("open");
    };
    function InitializaeClEditor() {
        //        $('#Strength').cleditor({ height: 160, width: 400 });
        //        $('#Weakness').cleditor({ height: 160, width: 400 });
        $('#SO').cleditor({ height: 160, width: "-moz-available" });
        $('#WO').cleditor({ height: 160, width: "-moz-available" });
        $('#ST').cleditor({ height: 160, width: "-moz-available" });
        $('#WT').cleditor({ height: 160, width: "-moz-available" });
        

        //        $(".cleditorMain iframe").contents().find('body').bind('keyup', function() {
        //            var v = $(this).text(); // or .html() if desired
        //            $('#<%=formId%>Description').html(v);
        //        });
    };
    function DestroyClEditor() {
        //        $('#Strength').cleditor(function() { this.destroy(); });
        //        $('#Weakness').cleditor(function() { this.destroy(); });
        $('#SO').cleditor(function() { this.destroy(); });
        $('#WO').cleditor(function() { this.destroy(); });
        $('#ST').cleditor(function() { this.destroy(); });
        $('#WT').cleditor(function() { this.destroy(); });
    };
    $(function() {
        $('#typeSelect').val(changeSelect); 
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        //$('#SO').charCount();
        InitializaeClEditor();

        displayMultiselect();
    });
</script>

<script type="text/javascript">
    function GetValueByKey(value, key) {
        var result = '';
        var pos = value.indexOf(key);
        if (pos != -1) {
            var subValu = value.substring(pos + key.length + 1);
            var tokenPos = subValu.indexOf("[T+K]");
            if (tokenPos != -1) {
                result = subValu.substring(0, tokenPos);
            }
            else {
                result = subValu;
            }
        }
        return result;
    };

    function CleanIndustrySelected() {
        $("#IndustryId").prop('selectedIndex', '-1');
    };

    function GetSwotByIndustryAndCompetitor(select, selected) {
        var selectIndustry = select;
        var selectCompetitor = selected;
        var dataSOToShow = $('#SO');
        var dataWOToShow = $('#WO');
        var dataSTToShow = $('#ST');
        var dataWTToShow = $('#WT');
        var dataSOToShowChildren = $('#ChildrenSO');
        var dataWOToShowChildren = $('#ChildrenWO');
        var dataSTShowChildren = $('#ChildrenST');
        var dataWTShowChildren = $('#ChildrenWT');
        if (selectIndustry.val() != null && selectIndustry.val() != '' && selectIndustry.val() != undefined && selectCompetitor.val() != null && selectCompetitor.val() != '' && selectCompetitor.val() != undefined) {
            var xmlhttp;
            var results = null;
            var parameters = { IndustryId: selectIndustry.val(), CompetitorId: selectCompetitor.val() };

            $.get(
            '<%= Url.Action("GetSwot", "Swot") %>',
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;

                    var so = ' ';
                    var wo = ' ';
                    var st = ' ';
                    var wt = ' ';
                    var sochild = '';
                    var wochild = ' ';
                    if (results != '') {
                        so = GetValueByKey(results, 'SwotStrengthOpportunities_' + selectIndustry.val());
                        wo = GetValueByKey(results, 'SwotWeaknessOpportunities_' + selectIndustry.val());
                        st = GetValueByKey(results, 'SwotStrengthThreats_' + selectIndustry.val());
                        wt = GetValueByKey(results, 'SwotWeaknessThreats_' + selectIndustry.val());
                        sochild = GetValueByKey(results, 'SwotStrengthOpportunitiesChildren_' + selectIndustry.val());
                        wochild = GetValueByKey(results, 'SwotWeaknessOpportunitiesChildren_' + selectIndustry.val());
                        stchild = GetValueByKey(results, 'SwotStrengthThreatsChildren_' + selectIndustry.val());
                        wtchild = GetValueByKey(results, 'SwotWeaknessThreatsChildren_' + selectIndustry.val());
                        if (so == '' || so == null || so == undefined) {
                            so = '<br/>';
                        }
                        if (wo == '' || wo == null || wo == undefined) {
                            wo = '<br/>';
                        }
                        if (st == '' || st == null || st == undefined) {
                            st = '<br/>';
                        }
                        if (wt == '' || wt == null || wt == undefined) {
                            wt = '<br/>';
                        }
                    }
                    $('#SO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", so, null, null).focus();
                    $('#WO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", wo, null, null).focus();
                    $('#ST').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", st, null, null).focus();
                    $('#WT').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", wt, null, null).focus();
                    //                    $('#SO').value = so;
                    //                    $('#WO').value = wo;
                    //                    $('#ST').value = st;
                    //                    $('#WT').value = wt;
                    $('#SO').html(so);
                    $('#WO').html(wo);
                    $('#ST').html(st);
                    $('#WT').html(wt);
                    dataSOToShowChildren[0].innerHTML = sochild;
                    dataWOToShowChildren[0].innerHTML = wochild;
                    dataSTShowChildren[0].innerHTML = stchild;
                    dataWTShowChildren[0].innerHTML = wtchild;                    
                }
            });
        }
        else {

            dataSOToShowChildren[0].innerHTML = "";
            dataWOToShowChildren[0].innerHTML = "";
            dataSTShowChildren[0].innerHTML = "";
            dataWTShowChildren[0].innerHTML = "";
            $('#SO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#WO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#ST').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#WT').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
   
        }
    };

    function GetSWByCompetitor(select, selected) {
        $("#CuncurrentIndustry").prop('checked', true);
        $("#SubIndustry").prop('checked', true);
        $("#Global").prop('checked', true);
        var MySelect = selected;
        var MySelectIndustry = select;
        if (MySelect.val() != null && MySelect.val() != '' && MySelect.val() != undefined && MySelect.val() != "-1") {
            //CleanOtherFields();
            var xmlhttp;
            var results = null;
            var parameters = { CompetitorId: MySelect.val(), IndustryId: MySelectIndustry.val() };
            // xmlhttp = $.get('<%= Url.Action("GetSWByCompetitorId", "Swot") %>', parameters);

            $.get('<%= Url.Action("GetSWByCompetitorId", "Swot") %>', parameters, function(data) {
                results = data;
                var dataStrengthToShow = $('#Strength');
                var dataWeaknessToShow = $('#Weakness');
                var chilStrength = '';
                var childWeakness = '';
                var globalStrength = '';
                var globalWeakness = '';
                if (results != '') {
                    globalWeakness = GetValueByKey(results, 'WeaknessGlobal_' + MySelect.val())
                    globalStrength = GetValueByKey(results, 'StrengthGlobal_' + MySelect.val());
                    var values = results.split('[t+k]');
                    chilStrength = GetValueByKey(results, 'StrengthChildren_' + MySelect.val());
                    dataStrengthToShow[0].innerHTML = GetValueByKey(results, 'Strength_' + MySelect.val()) + globalStrength + chilStrength;

                    childWeakness = GetValueByKey(results, 'WeaknessChildren_' + MySelect.val());
                    dataWeaknessToShow[0].innerHTML = GetValueByKey(results, 'Weakness_' + MySelect.val()) + globalWeakness + childWeakness;

                } else {
                    dataStrengthToShow[0].innerHTML = "";
                    dataWeaknessToShow[0].innerHTML = "";
                }
            });
            GetSwotByIndustryAndCompetitor(select, selected);

        } else {
            CleanOtherFields();
        }
        $('#CompetitorId').focus();
    };
    function GetQuickProfileCompetitor(select, selected) {
        var MySelect = selected;
        var MySelectIndustry = select;
        if (MySelect.val() != null && MySelect.val() != '' && MySelect.val() != undefined) {
            //CleanOtherFields();
            var xmlhttp;
            var results = null;
            var parameters = { CompetitorId: MySelect.val(), IndustryId: MySelectIndustry.val() };
            $.get(
            '<%= Url.Action("GetQuickProfileCompetitorId", "Swot") %>',
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    var total = '';
                    var totalproduct = '';
                    var totalDeal = '';
                    var name = '';
                    var dataQuickProfileShow = $('#CompetitorQuickProfile');
                    if (results != '') {
                        var values = results.split('[t+k]');
                        name = GetValueByKey(results, 'Name_' + MySelect.val());
                        total = GetValueByKey(results, 'TotalRenueve_' + MySelect.val());
                        totalproduct = GetValueByKey(results, 'TotalProduc_' + MySelect.val());
                        totalDeal = GetValueByKey(results, 'TotalDeal_' + MySelect.val());
                        dataQuickProfileShow[0].innerHTML = name + GetValueByKey(results, 'Quarters_' + MySelect.val()) + total + totalproduct + totalDeal;

                    } else {
                        dataStrengthToShow[0].innerHTML = "";
                    }
                }
            });
            
//            GetSwotByIndustryAndCompetitor();
        } else {
            CleanOtherFields();
        }
        $('#CompetitorId').focus();
    };
    function Cleanswot() {
        var MySelectIndustry = $('#IndustryId');
        var MySelect = $('#CompetitorId');        
        var CompetitorQuickProfileShow = $('#CompetitorQuickProfile');
        var dataStrengthToShow = $('#Strength');
        var dataWeaknessToShow = $('#Weakness');
        var dataOpportunitiesToShow = $('#Opportunities');
        var dataThreatsToShow = $('#Threats');

        var dataSOToShowChildren = $('#ChildrenSO');
        var dataWOToShowChildren = $('#ChildrenWO');
        var dataSTShowChildren = $('#ChildrenST');
        var dataWTShowChildren = $('#ChildrenWT');
        if (MySelectIndustry.val() == "" || MySelectIndustry.val() == "-1") {

            CompetitorQuickProfileShow[0].innerHTML = "";
            dataOpportunitiesToShow[0].innerHTML = "";
            dataThreatsToShow[0].innerHTML = "";

            dataSOToShowChildren[0].innerHTML = "";
            dataWOToShowChildren[0].innerHTML = "";
            dataSTShowChildren[0].innerHTML = "";
            dataWTShowChildren[0].innerHTML = "";
            $('#SO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#WO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#ST').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#WT').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
        }
        if (MySelect.val() == "" || MySelect.val() == "-1") {
            dataStrengthToShow[0].innerHTML = "";
            dataWeaknessToShow[0].innerHTML = "";
            CompetitorQuickProfileShow[0].innerHTML = "";

            dataSOToShowChildren[0].innerHTML = "";
            dataWOToShowChildren[0].innerHTML = "";
            dataSTShowChildren[0].innerHTML = "";
            dataWTShowChildren[0].innerHTML = "";
            $('#SO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#WO').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#ST').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
            $('#WT').cleditor({ height: 160, width: 400, updateTextArea: function() { } })[0].clear().execCommand("inserthtml", '<br/>', null, null).focus();
        }
    }
    function timeChangeSelectIndustry(MySelectIndustry, MySelect) {
        setTimeout(function() { ChangeSelectIndustry(MySelectIndustry, MySelect); }, 300)
    }
    function ChangeSelectIndustry(MySelectIndustrys, MySelects) {;
        if (MySelectIndustrys != "" && changeSelect == "changeSelect" && MySelectIndustrys != null && MySelectIndustrys != "-1") {
            changeSelect = "ChangeIndustry";
        }
        else if (MySelects != "" && changeSelect == "changeSelect" && MySelects != null && MySelects != "-1") {
        changeSelect = "ChangeCompetiror";
    }
      $('#typeSelect').val(changeSelect); 
      Cleanswot();
    };
    function ShowQuickCompetitor() {
        var MySelectIndustry = $("input[name=multiselect_IndustryId]:checked");
        var MySelect = $("input[name=multiselect_CompetitorId]:checked");
        if (MySelectIndustry.val() != "" && MySelect.val() != "" && MySelectIndustry.val() != null && MySelect.val() != null &&
            MySelectIndustry.val() != "-1" && MySelect.val() != "-1") {
            setTimeout(function() {           
                GetQuickProfileCompetitor(MySelectIndustry, MySelect);
            }, 100)
        }

    };
    function setValuesForIndustry(MySelect, MySelectIndustry) {        
        if (changeSelect == "ChangeCompetiror" || changeSelect == "changeSelect") {
            $('#IndustryId')[0].options.length = 0;
            if (MySelect == null || MySelect == "" || MySelect == "-1") {
                changeSelect = "changeSelect";
                $('#typeSelect').val(changeSelect); 
            }

            if (MySelect == '-1' && MySelectIndustry == '-1') {
                var url = '<%= Url.Action("SelectAllValues", "Swot")%>';
                $.post(url, null, function(data) {
                addndustrysToList(data.ListIndustry, MySelect);
                addCompetitorsToList(data.ListCompetitor, MySelectIndustry);
                    Cleanswot();
                });
            }
            else {
                var url = '<%= Url.Action("GetComplexIndustry", "Swot")%>';
                $.post(url, { ids: MySelect }, function(data) {
                    if (data != "") {
                        addndustrysToList(data, MySelect);
                        Cleanswot();
                    }
                    else {
                        Cleanswot();
                    }
                });
            }
        }
        ShowQuickCompetitor();
    }
    function addndustrysToList(results, idCompetitor) {

        var realvaluesIndustry = [];
        $('#IndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
        });
        var items = "";
        var arrayComppetitors = results.split("_");
        for (j = 0; j < arrayComppetitors.length; j++) {
            var arrayCompet = arrayComppetitors[j].split(":");
            if (j == 0) {
                items += "<option value=''>     </option>";
            }           
            items += "<option value='" + arrayCompet[0] + "'>" + arrayCompet[1] + "</option>";
        }
        $("#IndustryId").html(items);
        $("#IndustryId").multiselect('refresh');

    }
    function setValuesForCompetitors2(MySelectIndustry, MySelectCompetitor) {
        if (changeSelect == "ChangeIndustry" || changeSelect == "changeSelect") {
            $('#CompetitorId')[0].options.length = 0;
            if (MySelectIndustry == null || MySelectIndustry == "" || MySelectIndustry == "-1") {
                changeSelect = "changeSelect";
                $('#typeSelect').val(changeSelect); 
            }
            if (MySelectIndustry == '-1' && MySelectCompetitor == '-1') {
                var url = '<%= Url.Action("SelectAllValues", "Swot")%>';
                $.post(url, null, function(data) {
                    addndustrysToList(data.ListIndustry, MySelectCompetitor);
                    addCompetitorsToList(data.ListCompetitor, MySelectIndustry);
                    Cleanswot();
                });
            }
            else {
                var url = '<%= Url.Action("GetComplexCompetitors", "Swot")%>';
                $.post(url, { ids: MySelectIndustry }, function(data) {
                    if (data != "") {
                        addCompetitorsToList(data, MySelectIndustry);
                        Cleanswot();
                    }
                    else {
                        Cleanswot();
                    }
                });
            }
        }
        ShowQuickCompetitor();
    }
    function addCompetitorsToList(results, idIdustry) {
        var realvaluesIndustry = [];
        $('#IndustryIds :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
        });
        var items = "";
        var arrayComppetitors = results.split("_");
        for (j = 0; j < arrayComppetitors.length; j++) {
            var arrayCompet = arrayComppetitors[j].split(":");
            if (j == 0) {
                items += "<option value=''>     </option>";
            }          
            items += "<option value='" + arrayCompet[0] + "'>" + arrayCompet[1] + "</option>";

        }
        $("#CompetitorId").html(items);
        $("#CompetitorId").multiselect('refresh');
    }
    function GetActionsByIndustry(select, selected) {
        $("#CuncurrentIndustry").prop('checked', true);
        $("#SubIndustry").prop('checked', true);
        $("#Global").prop('checked', true);
        var MySelectIndustry = select;
        var MySelect = selected;
        if (MySelectIndustry.val() != null && MySelectIndustry.val() != '' && MySelectIndustry.val() != '-1' && MySelectIndustry.val() != undefined) {

            var xmlhttp;
            var results = null;
            if (MySelect.val() != null && MySelect.val() != '' && MySelect.val() != '-1' && MySelect.val() != undefined) {
                GetSWByCompetitor(MySelectIndustry, MySelect);
            }

            var parameters = { IndustryId: MySelectIndustry.val(), CompetitorId: MySelect.val() };
            //xmlhttp = $.get('<%= Url.Action("GetTrendsByIndustryId", "Swot") %>', parameters);
            $.get('<%= Url.Action("GetTrendsByIndustryId", "Swot") %>', parameters, function(data) {
                results = data;
                var dataOpportunitiesToShow = $('#Opportunities');
                var dataThreatsToShow = $('#Threats');
                var opportunities = '';
                var threats = '';
                var oandt = '';
                var oandtthreats = '';
                var opportunitiesChildren = '';
                var threatsChildren = '';
                var oandtChildren = '';
                var Child = '';
                var globalopportunities = '';
                var globalthreats = '';
                var globaloandt = '';
                var globaloandttrend = '';
                var oandtChildrentrend = '';

                if (results != '') {

                    opportunities = GetValueByKey(results, 'TrendOpportunity_' + MySelectIndustry.val());
                    threats = GetValueByKey(results, 'TrendThreat_' + MySelectIndustry.val());
                    oandt = GetValueByKey(results, 'TrendThreatOpportunity_' + MySelectIndustry.val());
                    oandtthreats = GetValueByKey(results, 'TrendThreatOpportunityTrend_' + MySelectIndustry.val());

                    threatsChildren = GetValueByKey(results, 'TrendThreatChildren_' + MySelectIndustry.val());
                    opportunitiesChildren = GetValueByKey(results, 'TrendOpportunityChildren_' + MySelectIndustry.val());
                    oandtChildren = GetValueByKey(results, 'TrendThreatOpportunityChildren_' + MySelectIndustry.val());
                    oandtChildrentrend = GetValueByKey(results, 'TrendThreatOpportunityChildrentrend_' + MySelectIndustry.val());
                    globalopportunities = GetValueByKey(results, 'TrendOpportunityGlobal_' + MySelectIndustry.val());
                    globalthreats = GetValueByKey(results, 'TrendThreatGlobal_' + MySelectIndustry.val());
                    globaloandt = GetValueByKey(results, 'TrendThreatOpportunityGlobal_' + MySelectIndustry.val());
                    globaloandttrend = GetValueByKey(results, 'TrendThreatOpportunityGlobalTrend_' + MySelectIndustry.val());
                    if (opportunitiesChildren != '' || oandtChildren != '') {
                        Child = '<h1>CHILD</h1>'
                    }

                    dataOpportunitiesToShow[0].innerHTML = opportunities + oandt + globalopportunities + globaloandt + opportunitiesChildren + oandtChildren;
                    //                        if (opportunities != '') {
                    //                            dataOpportunitiesToShow[0].innerHTML = opportunities;
                    //                        } else {
                    //                            dataOpportunitiesToShow[0].innerHTML =  oandt;
                    //                        }
                    dataThreatsToShow[0].innerHTML = threats + oandtthreats + globalthreats + globaloandttrend + threatsChildren + oandtChildrentrend;
                    //                        if (threats != '') {
                    //                            dataThreatsToShow[0].innerHTML = threats;
                    //                        }
                    //                        else {
                    //                            dataThreatsToShow[0].innerHTML = oandt;
                    //                        }
                } else {
                    dataOpportunitiesToShow[0].innerHTML = "";
                    dataThreatsToShow[0].innerHTML = "";
                }
            });

            //xmlhttp.onreadystatechange = function() {   

            // if (xmlhttp.readyState == 4) {

            //          results = xmlhttp.responseText;

            //      }
            //  }
            GetSwotByIndustryAndCompetitor(select, selected);

        } else {
            CleanOtherFields();
        }
        $('#IndustryId').focus();
    };
    function InitializeEditSwot() {
        var MySelectIndustry = $('#IndustryId');
        var MySelect = $('#CompetitorId');
        displayMultiselect();
        
        GetSWByCompetitor(MySelect, MySelectIndustry);
        GetActionsByIndustry(MySelectIndustry, MySelect);
        GetQuickProfileCompetitor(MySelectIndustry, MySelect);
    };    
    var htmlEncode = function(value) {
        return $('<div/>').text(value).html();
    };
    var htmlDecode = function(value) {
        return $('<div/>').html(value).text();
    };
    function GetCuncurrentIndustry(check, column) {
        var MySelectIndustry = $('#IndustryId');
        var MySelect = $('#CompetitorId');
        if (!check && column == 1) {
            $('#' + 'TrendOpportunity_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreat_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatOpportunity_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatOpportunityTrend_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'Strength_' + MySelect.val()).addClass('newshidden');
            $('#' + 'Weakness_' + MySelect.val()).addClass('newshidden');

        }
        else if (check && column == 1) {
            $('#' + 'TrendOpportunity_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatOpportunity_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreat_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatOpportunityTrend_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'Strength_' + MySelect.val()).removeClass('newshidden');
            $('#' + 'Weakness_' + MySelect.val()).removeClass('newshidden');
        }
        else if (!check && column == 2) {
            $('#' + 'TrendOpportunityGlobal_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatGlobal_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatOpportunityGlobal_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatOpportunityGlobalTrend_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'StrengthGlobal_' + MySelect.val()).addClass('newshidden');
            $('#' + 'WeaknessGlobal_' + MySelect.val()).addClass('newshidden')
        }
        else if (check && column == 2) {
            $('#' + 'TrendOpportunityGlobal_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatOpportunityGlobal_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatGlobal_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatOpportunityGlobalTrend_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'StrengthGlobal_' + MySelect.val()).removeClass('newshidden');
            $('#' + 'WeaknessGlobal_' + MySelect.val()).removeClass('newshidden');
        }

        else if (!check && column == 3) {

            $('#' + 'TrendOpportunityChildren_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatChildren_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatOpportunityChildren_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'TrendThreatOpportunityChildrentrend_' + MySelectIndustry.val()).addClass('newshidden');
            $('#' + 'StrengthChildren_' + MySelect.val()).addClass('newshidden');
            $('#' + 'WeaknessChildren_' + MySelect.val()).addClass('newshidden');

        }
        else if (check && column == 3) {
            $('#' + 'TrendOpportunityChildren_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatChildren_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatOpportunityChildren_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'TrendThreatOpportunityChildrentrend_' + MySelectIndustry.val()).removeClass('newshidden');
            $('#' + 'StrengthChildren_' + MySelect.val()).removeClass('newshidden');
            $('#' + 'WeaknessChildren_' + MySelect.val()).removeClass('newshidden');
        }

    }
    function GenerateDocument(urlaction, TypeDoc) {
        var MySelectIndustry = $('#IndustryId');
        var MySelectCompetitor = $('#CompetitorId');
        if (MySelectIndustry.val() != null && MySelectIndustry.val() != '' && MySelectIndustry.val() != undefined && MySelectCompetitor.val() != null && MySelectCompetitor.val() != '' && MySelectCompetitor.val() != undefined) {
            var cicheckbox = $('#CuncurrentIndustry').is(':checked');
            var gcheckbox = $('#Global').is(':checked');
            var sicheckbox = $('#SubIndustry').is(':checked');
            var url = urlaction + '?IndustryId=' + MySelectIndustry.val() + '&CompetitorId=' + MySelectCompetitor.val() + '&CurrentIndustry=' + cicheckbox + '&Global=' + gcheckbox + '&SubIndustry=' + sicheckbox + '&TypeDocument=' + TypeDoc + '&lparam=';

            $("#down_iframe").remove();
            $("#down_form").remove();

            $("body").append('<iframe id="down_iframe" name="downloadFrame"  style="display: none;" src="" />');
            $("body").append('<form   id="down_form" target="downloadFrame" action="' + url + '" method="POST" />');
            $("#down_form").submit();

        } else {
            ShowAlert('This document can not be generated', 'Competitor and Industry is required');
        }
    };
    function update_tools(value) {
        if (value == 1) {
            $('#ChildrenSO').removeClass('newshidden');
            $('#ChildrenWO').removeClass('newshidden');
            setLinkTarget('ChildrenSO');
            setLinkTarget('ChildrenWO');
            $('#comp_tools_button_close').removeClass('newshidden');
            $('#comp_tools_button_show').addClass('newshidden');
        }
        else {
            $('#ChildrenSO').addClass('newshidden');
            $('#ChildrenWO').addClass('newshidden');
            $('#comp_tools_button_close').addClass('newshidden');
            $('#comp_tools_button_show').removeClass('newshidden');
        }
    }
    function update_toolsST(value) {
        if (value == 1) {
            $('#ChildrenST').removeClass('newshidden');
            $('#ChildrenWT').removeClass('newshidden');
            setLinkTarget('ChildrenST');
            setLinkTarget('ChildrenWT');
            $('#comp_tools_button_closeST').removeClass('newshidden');
            $('#comp_tools_button_showST').addClass('newshidden');
        }
        else {
            $('#ChildrenST').addClass('newshidden');
            $('#ChildrenWT').addClass('newshidden');
            $('#comp_tools_button_closeST').addClass('newshidden');
            $('#comp_tools_button_showST').removeClass('newshidden');
        }
    }
</script>
<style type="text/css">
    .counter{
	position:absolute;
	right:0;
	top:0;
	font-size:20px;
	font-weight:bold;
	color:#ccc;
	}
	.warning{color:#600;}
	.exceeded{color:#e00;}
</style>
<style type="text/css">    
    .newshidden
    {
         display:none;
    }
    a:hover, a:active {
       text-decoration: underline;
       color:#DFDFDF;
    }
</style>
<div id="ValidationSummarySwot">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm("CreateSwot", "Swot",
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SwotEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ InitializeEditSwot();InitializaeClEditor();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div>   
        <%--<legend><%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>--%>
      <div class="buttonLink">
         <input type="submit" value="Save" onclick="ConvertTheText();" />
         <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Swot','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Swot %>');" style="float: right;margin-right: 5px;"/>
         </div> 
         <div style="width:94%;float:left">
          <label style="width:7%;float:left;Margin-left:10px">Export Tools:</label>         
            <a href="#" title="Export to Word format" >
                <img  width="22px" alt="Export to Word format" src="<%= Url.Content("~/Content/Images/Icons/export_word.gif")%>" onclick="GenerateDocument('<%= Url.Action("GenerateDocumentoSwot", "Swot")%>','word');" class="export_icon" />
            </a>
             <a  href="#" title="Export to pdf format">
                <img width="22px" alt="Export to PDF format" src="<%= Url.Content("~/Content/Images/Icons/export_pdf.gif")%>" onclick="GenerateDocument('<%= Url.Action("GenerateDocumentoSwot", "Swot")%>','pdf');" class="export_icon" />
            </a>
            <a href="#" title="Export to Excel format" >
                <img width="24px" alt="Export to Excel format" src="<%= Url.Content("~/Content/Images/Icons/export_excel.png")%>" onclick="GenerateDocument('<%= Url.Action("GenerateDocumentoSwot", "Swot")%>','xls');" class="export_icon" />
            </a>        
         </div>
        <%=Html.Hidden("typeChangeSelect","", new  { id = "typeSelect"})%>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="SwotEditFormInternalContent">
            <div class="line" style="width:98%;">
                <div class="midlefield" style="display: block;margin: 0 auto 0 auto;width:98%;">
                   <div class="midlefield" style="width:252px" >
                   <label for="IndustryId">
                        <asp:Literal ID="SwotIndustry" runat="server" Text="<%$ Resources:LabelResource, SwotIndustryTrend %>" />:</label>
                    <%= Html.DropDownList("IndustryId", (SelectList)ViewData["IndustryList"], string.Empty, new { id = "IndustryId", onchange = "setValuesForCompetitors2();timeChangeSelectIndustry();GetActionsByIndustry(this, this.selected)" })%>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                    </div>
                    <div class="midlefield" style="width:252px">
                        <label for="CompetitorId">
                            <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:LabelResource, SwotCompetitor %>" />:</label>
                        <%= Html.DropDownList("CompetitorId", (SelectList)ViewData["CompetitorList"], string.Empty, new { id = "CompetitorId", onchange = "setValuesForIndustry();timeChangeSelectIndustry();GetSWByCompetitor(this, this.selected);" })%>
                        <%= Html.ValidationMessage("CompetitorId", "*")%>
                    </div>
                    <div class="midlefield" style="width:556px;">
                                       
                        <div id="CheckboxGlobal">
                            <fieldset style= "border: 1px solid #A4A4A4; height: 38px;margin-top: -10px;">
                                <legend align= "center" style=" font-family: arial; color:#445566; font-weight: bold; font-size: 0.85em;">Show Strengths, Weaknesses and Trend Assigments</legend>                        
                                <div class="midlefield" style="width:25%;margin-left:46px;margin-top:-2px">
                                    <input  style="width:12px; height:12px; vertical-align:middle; float:left;" type="checkbox" id="CuncurrentIndustry" onclick="GetCuncurrentIndustry(this.checked,1);"/><label style="float: left; margin-left: 5px; margin-top: 0px;">Current Industry</label><img src="<%= Url.Content("~/Content/Images/Icons/current_Industry.jpg") %>" style="width:23px;position:absolute;padding-top:-5px;padding-left:6px;"/>        
                                </div>
                                <div class="midlefield" style="width:15%;margin-top:-2px;vertical-align:middle;margin-left: 21px;">
                                    <input  style="width:12px;height:12px;vertical-align:-1px;float:left;" type="checkbox" id="Global" onclick="GetCuncurrentIndustry(this.checked,2);"/><label style="float: left; margin-left: 5px; margin-top: 0px;">Global</label><img src="<%= Url.Content("~/Content/Images/Icons/global.jpg") %>" style="width:16px;position:absolute;padding-left:6px;margin-top:-2px;" />
                                </div>
                                <div class="midlefield" style="width:30%;margin-top:-2px;vertical-align:middle;">
                                    <input style="width:12px;height:12px;vertical-align:middle;float:left;" type="checkbox" id="SubIndustry" onclick="GetCuncurrentIndustry(this.checked,3);"/><label style="float: left; margin-left: 5px;margin-top: 0px;">Sub-Industry Roll-up</label><img src="<%= Url.Content("~/Content/Images/Icons/sub-IndustryRoll-up.jpg") %>" style="width:14px;position:absolute;padding-left:6px" />
                                </div>
                            </fieldset>
                        </div>
                     
                </div>
                </div>
            </div>
            <div class="line">
                <div class="midlefield" style="width:33%">
                   <label for="Strength">
                        <asp:Literal ID="Literal15" runat="server" Text="<%$ Resources:LabelResource, SwotCompetitorQuickProfile %>"></asp:Literal>:</label>
                    <%--<%= Html.TextArea("Strength", new { id = "Strength", @class = "txtareaswot", @readonly = "readonly" })%>--%>
                    <div id="CompetitorQuickProfile" style="width: 100%;height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    
                    </div>
                    <%= Html.ValidationMessage("CompetitorQuickProfile", "*")%>
                </div>
                <div class="midlefield">
                    <label for="Strength">
                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:LabelResource, SwotStrength %>"></asp:Literal>:</label>
                    <%--<%= Html.TextArea("Strength", new { id = "Strength", @class = "txtareaswot", @readonly = "readonly" })%>--%>
                    <div id="Strength" style="width: 100%;height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>
                    <%= Html.ValidationMessage("Strength", "*")%>
                </div>
                <div class="midlefield">
                    <label for="Weakness">
                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:LabelResource, SwotWeakness %>"></asp:Literal>:</label>
                    <%--<%= Html.TextArea("Weakness", new { id = "Weakness", @class = "txtareaswot", @readonly = "readonly" })%>--%>
                    <div id="Weakness" style="width: 100%;height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>
                    <%= Html.ValidationMessage("Weakness", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="midlefield" style="width:33%">
                    <label for="Opportunities">
                        <asp:Literal ID="A" runat="server" Text="<%$ Resources:LabelResource, SwotOpportunities %>"></asp:Literal>:</label>
                    <%--<%= Html.TextArea("Opportunities", new { id = "Opportunities", @class = "txtareaswot", @readonly = "readonly" })%>--%>
                    <div id="Opportunities" style="width: 100%;height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>
                    <%= Html.ValidationMessage("Opportunities", "*")%>
                </div>
                <div class="midlefield">
                    <label for="SO">
                        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:LabelResource, SwotStrengthOpportunities %>"></asp:Literal>:</label>
                    <%= Html.TextArea("SO", new { id = "SO", @class = "txtareaswot" })%>
                    <%= Html.ValidationMessage("SO", "*")%>
                </div>
                <div class="midlefield">
                    <label for="WO">
                        <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:LabelResource, SwotWeaknessOpportunities %>"></asp:Literal>:</label>
                    <%= Html.TextArea("WO", new { id = "WO", @class = "txtareaswot" })%>
                    <%= Html.ValidationMessage("WO", "*")%>
                </div>
            </div>
            <div class="line" style="" class="expand_button">
                   <div class="midlefield" style="width:36%">
                </div>
                 <div  id="comp_tools_button_close"  class="newshidden" style="padding-left:9px" ><img src="<%=Url.Content("~/Content/Images/Icons/tools_close.png") %>"
                onclick="update_tools(0)" style="float: left;margin-left: -30px;"/><label for="comp_tools_button_close" style="float:left; margin-top: 0;margin-left: 5px;">Show/Hide sub-industry strategies</label></div>
                
                <div id="comp_tools_button_show" style="padding-left:9px"><img src="<%=Url.Content("~/Content/Images/Icons/tools_show.png") %>"
                onclick="update_tools(1)" style="float: left;margin-left: -30px;"/><label for="comp_tools_button_show" style="float:left; margin-top: 0;margin-left: 5px;">Show/Hide sub-industry strategies</label></div>
                
                <div class="line" style="width:100%">
                <div class="midlefield" style="width:33%">
                </div>
                <div class="midlefield">
                    <div id="ChildrenSO" class="newshidden" style="width: 100%;height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>
                </div>
               <div class="midlefield">
                    <div id="ChildrenWO" class="newshidden" style="width: 100%; height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>          
                </div>
                 </div>
                
            </div> 
            <div class="line">
                <div class="midlefield" style="width:33%">
                    <label for="Threats">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, SwotThreats %>"></asp:Literal>:</label>
                    <%--<%= Html.TextArea("Threats", new { id = "Threats", @class = "txtareaswot", @readonly = "readonly" })%>--%>
                    <div id="Threats" style="width: 100%; height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>
                    <%= Html.ValidationMessage("Threats", "*")%>
                </div>
                <div class="midlefield">
                    <label for="WO">
                        <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:LabelResource, SwotStrengthThreats %>"></asp:Literal>:</label>
                    <%= Html.TextArea("ST", new { id = "ST", @class = "txtareaswot" })%>
                    <%= Html.ValidationMessage("ST", "*")%>
                </div>
                <div class="midlefield">
                    <label for="WT">
                        <asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:LabelResource, SwotWeaknessThreats %>"></asp:Literal>:</label>
                    <%= Html.TextArea("WT", new { id = "WT", @class = "txtareaswot" })%>
                    <%= Html.ValidationMessage("WT", "*")%>
                </div>
            </div>
             <div class="line" style="" class="expand_button">
                   <div class="midlefield" style="width:36%">
                </div>
                
                 <div  id="comp_tools_button_closeST"  class="newshidden" style="padding-left:9px" ><img src="<%=Url.Content("~/Content/Images/Icons/tools_close.png") %>"
                onclick="update_toolsST(0)" style="float: left;margin-left:-30px;"/><label for="comp_tools_button_closeST" style="float:left; margin-top: 0;margin-left: 5px;">Show/Hide sub-industry strategies</label></div>
                
                <div id="comp_tools_button_showST" style="padding-left:9px"><img src="<%=Url.Content("~/Content/Images/Icons/tools_show.png") %>"
                onclick="update_toolsST(1)" style="float: left;margin-left:-30px;"/><label for="comp_tools_button_showST" style="float:left; margin-top: 0;margin-left: 5px;">Show/Hide sub-industry strategies</label></div>
                                
                <div class="line" style="width:100%">
                <div class="midlefield" style="width:33%">
                </div>
                <div class="midlefield">
                    <div id="ChildrenST" class="newshidden" style="width:100%; height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>
                </div>
               <div class="midlefield">
                    <div id="ChildrenWT" class="newshidden" style="width: 100%;height: 160px; border: 1px solid #A4A4A4;overflow: auto;">
                    </div>          
                </div>
                 </div>
            </div> 
        </div>
    </div>
<% } %>

