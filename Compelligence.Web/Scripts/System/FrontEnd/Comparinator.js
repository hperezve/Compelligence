
var comp_events;
var ArrayRankingDisplay = new Array();
var ArrayRankingHidden = new Array();
var ArrayRelevancyDisplay = new Array();
var arraReleavy = new Array();
function GetRankingandRelevance(ValueRa, ValueRe) {
    ArrayRankingHidden = ValueRa;
    ArrayRankingDisplay = ValueRe;
}

function CollapseCriteriaSet() {
    $(".comp_sethead").click(function() {
        if ($(this).next(".comp_table").is(":hidden")) {//EXPAND
            //$(this).next(".comp_table").show();
            $(this).next(".comp_table").removeClass("comp_hiddentable");
            //$(this).next(".comp_table").slideDown("slow");
            $(this).children(":first-child").removeClass('comp_criteriaexpand')
            $(this).children(":first-child").addClass('comp_criteriacollapse')

        } else {//COLLAPASE
        //$(this).next(".comp_table").hide();
            $(this).next(".comp_table").addClass("comp_hiddentable");
            //$(this).next(".comp_table").slideUp("slow");
            $(this).children(":first-child").removeClass('comp_criteriacollapse')
            $(this).children(":first-child").addClass('comp_criteriaexpand')
        }
        DisplayFirstRowFeature();
    });
}


function CollapsePricingSet() {
    $(".comp_pricinghead").click(function() {
        if ($(this).next(".comp_pricingtable").is(":hidden")) {
            $(this).next(".comp_pricingtable").show();
            //$(this).next(".comp_table").slideDown("slow");
            $(this).children(":first-child").removeClass('comp_criteriaexpand')
            $(this).children(":first-child").addClass('comp_criteriacollapse')

        } else {
            $(this).next(".comp_pricingtable").hide();
            //$(this).next(".comp_table").slideUp("slow");
            $(this).children(":first-child").removeClass('comp_criteriacollapse')
            $(this).children(":first-child").addClass('comp_criteriaexpand')
        }

    });
}



function ResizeWithOfDivHead() {
    var table = $('#comp_table_result');
    var newWidth = '';
    if (table[0] != undefined && table[0] != null) {
        if (table[0].style != undefined && table[0].style != null) {
            if (table[0].style.width != undefined && table[0].style.width != null) {
                newWidth = table[0].style.width;
                var divGroupHead = $('.comp_grouphead');
                if (divGroupHead != null && divGroupHead != undefined) {
                    if (divGroupHead.length != null && divGroupHead.length != undefined) {
                        for (var i = 0; i < divGroupHead.length; i++) {
                            divGroupHead[i].style.width = newWidth;
                        }
                    }
                }
                var divSetHead = $('.comp_sethead');
                if (divSetHead != null && divSetHead != undefined) {
                    if (divSetHead.length != null && divSetHead.length != undefined) {
                        for (var i = 0; i < divSetHead.length; i++) {
                            divSetHead[i].style.width = newWidth;
                        }
                    }
                }
            }
        }
    }

}
function UpdateWidths(expand, value) {
    var tbl = $('.comp_table');
    var seth = $('.comp_sethead');
    var grouph = $('.comp_grouphead');

    ResizeWithOfDivHead();
}

//Hide/Show Colum
function HideColumn(checked, column) {

    if (!checked) {
        UpdateWidths(false, 150);
        //Collapse and Show Column-1 Expand
        $('.comp_table th:nth-child(' + (column) + ')').hide();
        $('.comp_table td:nth-child(' + (column) + ')').hide();
        $('.comp_table col:nth-child(' + (column) + ')').hide();
    }
    else {
        //Expand
        UpdateWidths(true, 150);
        $('.comp_table th:nth-child(' + (column) + ')').show();
        $('.comp_table td:nth-child(' + (column) + ')').show();
        $('.comp_table col:nth-child(' + (column) + ')').show();
    }
}

function HideBenefitColumn(checked, check) {
    var criteriaSetTables2 = $('table[id="comp_table_result"]');
    if (criteriaSetTables2.length > 0) {
        var table = criteriaSetTables2[0];
        var numberColumn = GetNumberOfPreColumn(table);
        //var tbody = table.getElementsByTagName("tbody");
        if (numberColumn != undefined && numberColumn != null && numberColumn > 0) {
            HideColumn(checked, numberColumn);
        }
    }
}

function HideCostColumn(checked, check) {
    var criteriaSetTables3 = $('table[id="comp_table_result"]');
    if (criteriaSetTables3.length > 0) {
        var table3 = criteriaSetTables3[0];
        var numberColumn = GetNumberOfColumn(table3);
        //var tbody = table.getElementsByTagName("tbody");
        if (numberColumn != undefined && numberColumn != null && numberColumn > 0) {
            HideColumn(checked, numberColumn);
        }
    }
}

//Hide/Show Colum
function ToggleColumn(source, column, enabled) {
    $('.msg_table th:nth-child(' + (column - 1) + ')').find(".clickone").each(
  function(index) {
      $(this).toggleClass("clicktwo");
      if (enabled.toString() == 0) {
          this.onclick = function() { ToggleColumn(source, column, 1); };
      }
      else {
          this.onclick = function() { ToggleColumn(source, column, 0); };
      }
  }
  );
    if (enabled.toString() == 0) {
        $('.msg_table td:nth-child(' + column + ')').hide();
        $('.msg_table th:nth-child(' + column + ')').hide();
    }
    else {
        $('.msg_table td:nth-child(' + column + ')').show();
        $('.msg_table th:nth-child(' + column + ')').show();
    }
}

function filterBlocks() {
    var tablesfiltered = $(".filtered"); //capture tables

    //remove old filter
    tablesfiltered.each(function() {
        $(this).removeClass("comp_hiddentable");
        $(this).prev().removeClass("comp_hiddentable");
    });



    var tablesi = 0;
    tablesfiltered.each(function() {

        var tablesrows = $(this).find("tbody tr");
        var tablesrowsi = 0;
        tablesrows.each(function() {
            if (!$(this).is(":hidden")) //if not hidden
            {
                tablesrowsi++;
            }
        });
        if (tablesrowsi == 0) //catch table without rows
        {
            $(this).addClass("comp_hiddentable");
            $(this).prev().addClass("comp_hiddentable");
        }
        tablesi++;
    });

    //alert(tablesfiltered.size());
}

function filterRows() 
{

    initializeStatistics();
    var filter = $('#txtFilter').val();
    var rows = $(".filtered tbody tr");

    //trying deselect checks filter
    $("input.criteriafeature:checked").each(function() {
        $(this).prop('checked', false);
    });
    $('#hdnfeatures').val('');

    rows.each(function() //items selected
    {
        if ($(this).text().search(new RegExp(filter, "i")) < 0) {
            $(this).addClass("hidden");
        } else {
            $(this).removeClass("hidden");
        }
    });

    filterBlocks();
}

//Checkbox for Comparinator Product
function colorizeFeature(state) {
    var keys = ["BC", "MA", "MP", "MD", "LM","MO"];
    if (state == 1) {
        var rows = $(".filtered tbody tr");
        rows.each(function() //items selected
        {
            for (var i = 0; i < keys.length; i++) {
                var coltd = $(this).find("td." + keys[i]);
                if (coltd.size() > 0) //Match td.BC/td.MA/td.MP/td.MD/td.LM
                {
                    if (keys[i] == "BC") { coltd.css("background-color", "green"); }
                    if (keys[i] == "MA") { coltd.css("background-color", "lime"); }
                    if (keys[i] == "MP") { coltd.css("background-color", "white"); }
                    if (keys[i] == "MD") { coltd.css("background-color", "pink"); }
                    if (keys[i] == "LM") { coltd.css("background-color", "red"); }
                    if (keys[i] == "MO") { coltd.css("background-color", "white"); }
                }
            }

        });
        //$("#filter-count").text(count);
    }
    else {
        var rows = $(".filtered tbody tr");
        rows.each(function() //items selected
        {
            var coltd = $(this).find("td");
            if (coltd.size() > 0) //Match td.BC/td.MA/td.MP/td.MD/td.LM
            {
                coltd.css("background-color", "");
            }
        });

    }

}

function updateStatistics(matchs, total) {

    var XX = matchs;
    var YY = total - matchs;
    var XXp = (matchs / total) * 100;
    if (XXp == NaN || XXp == undefined || XXp == null) {
        XXp = 0;
    }
    $('#statistics').html(
    "Total Matching Features " + XX + "<br />  " +
    "Total Differing Features " + YY + "<br />  " +
    "Percent Matching " + XXp.toFixed(2) + "% <br />");

}

function initializeStatistics() {
    $('#statistics').empty();
}
function removeHidden(rows) {
    rows.each(function() //items selected
    {
        $(this).removeClass("hidden");
    });
}
function addHidden(rows) {
    rows.each(function() //items selected
    {
        $(this).addClass("hidden");
    });
}
function removeHidden(rows) {
    rows.each(function() //items selected
    {
        $(this).removeClass("hidden");
    });
}
function addHidden(rows) {
    rows.each(function() //items selected
    {
        $(this).addClass("hidden");
    });
}
function removeHiddenWithFeature(rows) {
    ///Get Ranking Values
    var keys = new Array();
    if ($('#hdnfeatures').val() != null && $('#hdnfeatures').val() != undefined && $('#hdnfeatures').val() != '') {
        keys = $('#hdnfeatures').val().split(':');
    }
    ///End Ranking Value
    rows.each(function() //items selected
    {
        if (keys.length != null && keys.length != undefined && keys.length > 0) {
            var kc = 0;
            for (var i = 0; i < keys.length; i++) {

                var si = $(this).find("td." + keys[i]).size();
                kc = kc + si;
            }
            if (kc < keys.length) //don't have key
            {
                $(this).addClass("hidden");
            } else {
                $(this).removeClass("hidden");
                count++;
            }
        } else {
            $(this).removeClass("hidden");
        }
    });
}

// Filter apply when select a value by Default
function DisplaySameValues(option) {
    //initializeStatistics();
    var rowsneq = $('.comp_neq');
    var rowseq = $('.comp_eq');
    var sumallrows = $(".filtered tbody tr");

    ///if (option == 'all') {
        //removeHidden(rowsneq);
        ///removeHiddenWithFeature(rowsneq);
        //rowsneq.show();
        //removeHidden(rowseq);
        ///removeHiddenWithFeature(rowseq);
        //rowseq.show();
    ///}
    if (option == 'same') {
        $(".filtered tbody tr.comp_neq").addClass("hidden");
        var samerows = $(".filtered tbody tr.comp_eq");
        //updateStatistics(samerows.size(), sumallrows.size());        
    }
    else if (option == 'diff') {
        $(".filtered tbody tr.comp_eq").addClass("hidden");
        var diffrows = $(".filtered tbody tr.comp_neq");
        //updateStatistics(diffrows.size(), sumallrows.size());        

    }
    else if (option == 'all') {
        var allrows = $(".filtered tbody tr");
        console.log("rows: " + allrows.size());
        //updateStatistics(rowseq.size(), allrows.size());
    }       
    
    //    filterBlocks();
    DisplayFirstRowFeature();
    UpdatedOptionsToFilter();
}


function hidePricing(checked) {

    var group = $(".comp_pricinggroup");
    var head = $(".comp_pricinghead");
    var table = $(".comp_pricingtable");
    if (checked) {
        group.addClass('comp_hidden');
        head.addClass('comp_hidden');
        table.addClass('comp_hidden');
    }
    else {
        group.removeClass('comp_hidden');
        head.removeClass('comp_hidden');
        table.removeClass('comp_hidden');
    }

}

function setExportParams(link, type) {
    var objlink = $(link);
    var textfilter = $('#txtFilter').val();
    var filteropts = '';
    var hidesame = '';
    var exportfiltered = '';
    var idCriteriasSet = '';
    var idCriterias = '';
    var idCSBestClass = '';
    var criteriaSetTables = $('table[id="comp_table_result"]');
    for (var i = 0; i < criteriaSetTables.length; i++) 
    {
        var crit = criteriaSetTables[i];
        if (criteriaSetTables[i].style.display == 'none') {

            if (idCriteriasSet != '') {
                idCriteriasSet = idCriteriasSet + ':';
            }
            idCriteriasSet = idCriteriasSet + criteriaSetTables[i].tBodies[0].id;
        }
        if (criteriaSetTables[i].className == 'comp_table filtered comp_hiddentable') 
        {
            if (idCSBestClass != '') 
            {
                idCSBestClass = idCSBestClass + ':';
            }
            idCSBestClass = idCSBestClass + criteriaSetTables[i].tBodies[0].id;
        } else {
            if (criteriaSetTables[i].className == 'comp_table filtered') {
                var trowByTBodies = criteriaSetTables[i].tBodies[0].rows;
                if (trowByTBodies.length > 0) {
                    for (var m = 0; m < trowByTBodies.length; m++) {
                        var trCell = trowByTBodies[m];
                        if (trCell.className == 'comp_neq hidden') 
                        {
                            if (trCell.cells.length > 0) {
                                var lastChildren = trCell.cells[0].lastChild;
                                if (idCriterias != '') {
                                    idCriterias = idCriterias + ':';
                                }
                                idCriterias = idCriterias + trCell.id.substr(1);
                            }
                        }
                    }
                }
            }
        }
    }
    //alert(idCriterias);
    var criteriaTr = $('tr[class="comp_neq hidden"]');
    if (criteriaTr.length > 0) 
    {
        for (var n = 0; n < criteriaTr.length; n++) 
        {
          var mcriteriaid = criteriaTr[n].id.substr(1); //<td>
          if (idCriterias.indexOf(mcriteriaid) == -1) 
          {
            if (idCriterias != '') {
                idCriterias = idCriterias + ':';
            }
            idCriterias = idCriterias + mcriteriaid;
          }
        }
    }

    var criteriaNTr = $('tr[class="comp_eq hidden"]');
    if (criteriaNTr.length > 0) 
    {
       for (var n = 0; n < criteriaNTr.length; n++) 
       {
         var mcriteriaid = criteriaNTr[n].id;
         if (idCriterias.indexOf(mcriteriaid) == -1) 
         {
            if (idCriterias != '') {
                idCriterias = idCriterias + ':';
            }
            idCriterias = idCriterias + mcriteriaid;
         }
       }
    }

    var criteriaSetTables2 = $('table[id="comp_table_result"]');
    for (var i = 0; i < criteriaSetTables2.length; i++) {
        var crit = criteriaSetTables[i];
        if (criteriaSetTables2[i].style.display == 'none') {

            if (idCriteriasSet != '') {
                idCriteriasSet = idCriteriasSet + ':';
            }
            idCriteriasSet = idCriteriasSet + criteriaSetTables2[i].tBodies[0].id;
        }
    }


    if ($('#chkexportfiltered').is(':checked')) {
        //filteropts = $('#hdnfeatures').val();
        exportfiltered = 'y';
    } else {
        exportfiltered = 'n';
    }
    if ($('#chkhidesamevalues').is(':checked')) {
        hidesame = 'y';
    } else {
        hidesame = 'n';
    }
    var showIndustryStandard = '';
    if ($('#chkindstandard').is(':checked')) {
        showIndustryStandard = 'y';
    } else {
        showIndustryStandard = 'n';
    }
    var showOnlyBestClass = '';
    if ($('#chkbestfeature').is(':checked')) {
        showOnlyBestClass = 'y';
    } else {
        showOnlyBestClass = 'n';
    }
    var aurl = objlink.prop('href');
    var aidx = aurl.indexOf('?');
    if (aidx > 0) {
        aurl = aurl.substring(0, aidx);
    }
    var showBenefit = '';
    if ($('#chkhidebenefit').is(':checked')) {
        showBenefit = 'y';
    } else {
        showBenefit = 'n';
    }
    var showCost = '';
    if ($('#chkhidecost').is(':checked')) {
        showCost = 'y';
    } else {
        showCost = 'n';
    }
    objlink.prop('href', aurl + '?tp=' + type + '&ef=' + exportfiltered + '&ft=' + filteropts + '&he=' + hidesame + '&tf=' + textfilter + '&sis=' + showIndustryStandard + '&hcs=' + idCriteriasSet + '&csbc=' + idCSBestClass + '&hc=' + idCriterias + '&sb=' + showBenefit + '&sc=' + showCost);
}

function AddComentsclassImage(idcriteria) {
    $("#ImgComents" + idcriteria).removeClass("ImageCommentsN");
    $("#ImgComents" + idcriteria).addClass("ImageCommentsY");
}

function GetNumberOfColumn(obj) {
    var tbody = $(obj).children("tbody");
    var tblRows = tbody[0].rows[0].cells;
    return tblRows.length;
}
function GetNumberOfPreColumn(obj) {
    var tbody = $(obj).children("tbody");
    var tblRows = tbody[0].rows[0].cells;
    return tblRows.length-1;
}
function resizeHeightOfTdRecommendImage() {
    setTimeout(function() { timeChangeSelectIndustry(); }, 300)
}

function timeChangeSelectIndustry() 
{
    $('.removebutton').css('dispaly', 'none');
    var tdsr = $('.tdRecommendImage');
    var heightRMax = 0;
    if (tdsr != undefined && tdsr != null) {
        if (tdsr.length > 1) {
            for (var i = 0; i < tdsr.length; i++) {
                var tdTempoR = tdsr[i];
                var aTempoR = tdTempoR.children[0];
                var imgTempoR = aTempoR.children[0];
                if (aTempoR.children[0] == null || aTempoR.children[0] == undefined) {
                    imgTempoR = aTempoR;
                }
                if (imgTempoR != null && imgTempoR != undefined) {
                    if (imgTempoR.height > heightRMax) {
                        heightRMax = imgTempoR.height;
                    }
                }
            }           
            $('.tdRecommendImage').css('height', '0px');
            $('.tdRecommendImage').css('height', heightRMax);
            $('.tdRecommendImage').css('width', '200px');
            $('.tdRecommendImage').css('vertical-align', 'middle');
            $('.tdRecommendImage').css('text-align', 'center');
            $('.tdRecommendImage').css('display', 'table-cell');
        }
    }
    var tdt = $('.tdText');
    var heightMaxt = 0;

    if (tdt != undefined && tdt != null) {
        if (tdt.length > 1) {
            for (var i = 0; i < tdt.length; i++) {
                var tdtTempo = tdt[i];
                if (tdtTempo == null || tdtTempo == undefined) {
                    imgTempoR = aTempoR;
                }
                if (tdtTempo != null && tdtTempo != undefined) {
                    if ($(tdtTempo).height() > heightMaxt) {
                        heightMaxt = $(tdtTempo).height();
                    }
                }
            }            
            $('.tdText').css('height', '0px');
            $('.tdText').css('height', heightMaxt);
            $('.tdText').css('width', '200px');
            $('.tdText').css('vertical-align', 'middle');
            $('.tdText').css('text-align', 'center');
            $('.tdText').css('display', 'table-cell');
        }
    }
    $('.removebutton').css('dispaly', 'block');
    resizeHeightOfTdRecommendImage();
};
function resizeHeightOfTdImage() {
    setTimeout(function() { timeChangeresizeHeightOfTdImage(); }, 200)
}
function timeChangeresizeHeightOfTdImage() {
    var tds = $('.tdImage');
    var heightMax = 0;
    if (tds != undefined && tds != null) {
        if (tds.length > 1) {
            for (var i = 0; i < tds.length; i++) {
                var tdTempo = tds[i];
                var aTempo = tdTempo.children[0];
                var imgTempo = aTempo.children[0];
                if (imgTempo == null || imgTempo == undefined) {imgTempo = aTempo; }
                if (imgTempo != null && imgTempo != undefined) {
                    if (imgTempo.height > heightMax) {
                        heightMax = imgTempo.height;
                    }
                }
            }
            $('.tdImage').css('height', '0px');
            $('.tdImage').css('height', heightMax);
            $('.tdImage').css('width', '200px');          
            $('.tdImage').css('vertical-align', 'middle');
            $('.tdImage').css('text-align', 'center');
            $('.tdImage').css('display', 'table-cell');
        }
    }
    var tdt = $('.tdText2');
    var heightMaxt = 0;
    if (tdt != undefined && tdt != null) {
        if (tdt.length > 1) {
            for (var i = 0; i < tdt.length; i++) {
                var tdtTempo = tdt[i];
                var aTempo = tdTempo.children[0];
                var imgTempoR = aTempo.children[0];  
                              
                if (tdtTempo == null || tdtTempo == undefined) {
                    imgTempoR = aTempo;
                }
                if (tdtTempo != null && tdtTempo != undefined) {
                    if ($(tdtTempo).height() > heightMaxt) {
                        heightMaxt = $(tdtTempo).height();
                    }
                }
            }
            $('.tdText2').css('height', '0px');
            $('.tdText2').css('height', heightMaxt);
            $('.tdText2').css('width', '200px');
            $('.tdText2').css('vertical-align', 'middle');
            $('.tdText2').css('text-align', 'center');
            $('.tdText2').css('display', 'table-cell');
        }
    }
    resizeHeightOfTdImage();
};
function AddProduct(competitorName, productId, urlAction, encodeCompanyId) {
    var industryId = $('#IndustryId option:selected').val();
    var competitorId = '';
    var optionsOfCompetitor = $('#CompetitorId option');
    for (var i = 0; i < optionsOfCompetitor.length; i++) {
        if (optionsOfCompetitor[i].text == competitorName) {
            competitorId = optionsOfCompetitor[i].value;
            i = optionsOfCompetitor.length;
        }
    }
    if (encodeCompanyId == null || encodeCompanyId == undefined) {
        encodeCompanyId = '';
    }
    var parameters = { IndustryId: industryId, CompetitorId: competitorId, ProductId: productId, C: encodeCompanyId };
    $.post(urlAction, parameters, function(data) {
        $('#FormProducts').html(data);
        resizeHeightOfTdImage();
        resizeHeightOfTdRecommendImage();
    });
};
function AddProductAdvance(competitorName, productId, urlAction, encodeCompanyId, urlGetProduct) {
    var industryId = $('#IndustryId option:selected').val();
    var competitorId = '';
    var optionsOfCompetitor = $('#CompetitorId option');
    for (var i = 0; i < optionsOfCompetitor.length; i++) {
        if (optionsOfCompetitor[i].text == competitorName) {
            competitorId = optionsOfCompetitor[i].value;
            i = optionsOfCompetitor.length;
            $("#CompetitorId option[value='" + competitorId + "']").prop("selected", 1);
            $("#CompetitorId").multiselect("refresh");
            $('input[name="multiselect_CompetitorId"]').click(function(Comp) {
                var var_name = $("input[name=multiselect_CompetitorId]:checked").val();
                var select_industry = $("input[name=multiselect_IndustryId]:checked").val();
                var urlComparinator = urlGetProduct + '/' + var_name + "?IndustryId=" + select_industry + '&C=' + encodeCompanyId;
                $.ajax({
                    type: "POST",
                    url: urlGetProduct + '/' + var_name + "?IndustryId=" + select_industry + '&C=' + encodeCompanyId,
                    dataType: 'json',
                    beforeSend: function() {
                        showLoadingDialog();
                    },

                    success: function(json) {
                        var items = "";
                        $.each(json, function(i, item) {
                            items += "<option value='" + item.Value + "' " + item.Disabled + " >" + item.Text + "</option>";
                        })
                        $("#ProductId").html(items);
                    }, complete: function() {
                        hideLoadingDialog();
                        $("#ProductId").multiselect('refresh');
                        $("#ProductIdLoader").hide();
                    }
                })
            })
            $.ajax({
                type: "POST",
                url: urlGetProduct + '/' + competitorId + "?IndustryId=" + industryId + '&C=' + encodeCompanyId,
                dataType: 'json',
                beforeSend: function() {
                    showLoadingDialog();
                },
                success: function(json) {
                    var items = "";
                    $.each(json, function(i, item) {
                        items += "<option value='" + item.Value + "' " + item.Disabled + " >" + item.Text + "</option>";
                    })
                    $("#ProductId").html(items);
                }, complete: function() {
                    hideLoadingDialog();
                    $("#ProductId option[value='" + productId + "']").prop("selected", 1).prop("checked", "checked");
                    $("#ProductId").multiselect('refresh');
                    $("#ProductIdLoader").hide();
                }
            });
        }
    }
    var parameters = { IndustryId: industryId, CompetitorId: competitorId, ProductId: productId, C: encodeCompanyId };
    $.post(urlAction, parameters, function(data) {
        $('#FormProducts').html(data);
        resizeHeightOfTdImage();
        resizeHeightOfTdRecommendImage();
    });
};


//Checkbox for Comparinator Product
function colorizeFeatureForProducts(state) {
    var keys = ["BC", "MA", "MP", "MD", "LM", "WOR"];
    
    if (state == 1) {
        var rows = $(".filtered tbody tr");
        rows.each(function() //items selected
        {
          for (var i = 0; i < keys.length; i++) 
          {
            var coltd = $(this).find("td." + keys[i]);
            if (coltd.size() > 0) //Match td.BC/td.MA/td.MP/td.MD/td.LM
            {
                    if (keys[i] == "BC") { coltd.css("background-color", "green"); }
                    if (keys[i] == "MA") { coltd.css("background-color", "lime"); }
                    if (keys[i] == "MP") { coltd.css("background-color", "white"); }
                    if (keys[i] == "MD") { coltd.css("background-color", "pink"); }
                    if (keys[i] == "LM") { coltd.css("background-color", "red"); }
                    if (keys[i] == "WOR") { coltd.css("background-color", ""); }
             }
          }
        });
        //$("#filter-count").text(count);
    }
    else {
        var rows = $(".filtered tbody tr");
        rows.each(function() //items selected
        {
            var coltd = $(this).find("td");
            if (coltd.size() > 0) //Match td.BC/td.MA/td.MP/td.MD/td.LM
            {
                coltd.css("background-color", "");
            }
        });

    }

};

//Check if should show to All Products or Selected products
function CompareToAllProducts() {
    var myRadio = $('input[name=chktoprods]');
    var checkedValue = myRadio.filter(':checked').val();
    var toallproducts = false;
    if (checkedValue == 'all') {
        toallproducts = true;
    }
    return toallproducts
};

function GetValueOfSameValues() {
    var selectedVal = "";
    //var selected = $("#radioDiv input[type='radio']:checked");
    var selected = $("input[type='radio'][name='SameValues']:checked");
    var idValue = $("input[type='radio'][name='SameValues']:checked").prop('id');
    if (selected.length > 0)
        selectedVal = selected.val();
    return idValue;
};
function UpdatedOptionsToFilter() {
    var keys;
    var keyssel = ["BC", "MA", "MP", "MD", "LM"];
    var keysall = ["BCA", "MAA", "MPA", "MDA", "LMA"];

    //
    var toallproducts = CompareToAllProducts();
    //
    if (toallproducts) {
        keys = keysall;
    } else {
        keys = keyssel;
    }

    var rows = $(".filtered tbody tr");
    var columnsfitered = $('#hdnproductids').val();
    var productids = new Array();
    $("input.hdnproductid").each(function() {
        productids.push($(this).val());
    });
    var rankingbyprodu = new Array(1);
    if (productids != null && productids != undefined && productids.length > 0) {
        rankingbyprodu = new Array(productids.length);
        for (var rp = 0; rp < rankingbyprodu.length; rp++) {
            rankingbyprodu[rp] = new Array(keys.length);
        }
    }
    for (var p = 0; p < productids.length; p++) {
        for (var i = 0; i < keys.length; i++) {
            rankingbyprodu[p][i] = false;
            var exist = false;
            rows.each(function() {
                var className = $(this).prop('class');
                if (className != null && className != undefined && className != '' && className.indexOf('hidden') == -1) {
                    var si = $(this).find("td." + keys[i] + productids[p]).size();
                    if (si != null && si != undefined && si > 0) {
                        exist = true;
                        rankingbyprodu[p][i] = true;
                    }
                }
            });
        }
    }
    for (var p = 0; p < productids.length; p++) {
        for (var i = 0; i < keys.length; i++) {
            if (rankingbyprodu[p][i]) {//becuase the class is only BC0000000
                $("input." + keyssel[i] + productids[p]).each(function() {
                    $(this).removeAttr("disabled");
                });
            } else {
                $("input." + keyssel[i] + productids[p]).each(function() {
                    $(this).prop("disabled", true);
                });
            }
        }
    }
};
var setValuesByDefault = function() {
   //restore color radio and action, no yet
    
    $('#chkboxselprods').prop('checked', true);
    $('#chkboxallprods').prop('checked', false);
    ResetRelevancyFilter();
    resetfilter();
    UpdatedOptionsToFilter();
    ResetDisplayValues();
    UpdateRowsByConfig();
};
var FormConfirmResetFilterBox = function(dialogContent) {
    var confirmObject = $("#ConfirmBox");
    confirmObject.empty();
    confirmObject.dialog({
        width: 250,
        heigth: 150,
        modal: true,
        buttons: {
            "Ok": function() {
                //setValuesByDefault();
                resetOnFeaturesTab();
                $(this).dialog('destroy');
            },
            "Cancel": function() {
                $(this).dialog('destroy');
            }
        }
    });
    confirmObject.dialog('option', 'title', 'Confirm Reset Filter');
    confirmObject.html(dialogContent);
    confirmObject.dialog('open');
};
var showConfirmResetFilterDialog = function() {
    var strCFDForm = "<form id='ConfirmFilterFormEdit'>";
    strCFDForm += "<label>All filters will be reset, do you want to continue?</label>";
    strCFDForm += "</form>";
    FormConfirmResetFilterBox(strCFDForm);
};

var AddTargetBlankToLin = function() {
$('.changeTargetBlank a').live('click', function() {
$(this).attr('target', '_blank');
});
};

function setLinkTarget(divId) {
    $('#' + divId + ' a').attr('target', '_blank');
};



/*
* Result
*/

function UpdateMostDesiredField(select, selected) {
    if (selected == 'LIS') {
        $('.trmostdesiredvalue').hide();
    } else {
        $('.trmostdesiredvalue').show();
        if (selected == 'BOL') {
            $("#SelMostDesiredValue option").changeToHidden();
            $('#SelMostDesiredValue input[type="hidden"].Boolean').changeToOption();
        }
        else {
            $("#SelMostDesiredValue option").changeToHidden();
            $('#SelMostDesiredValue input[type="hidden"].Numeric').changeToOption();

        }
    }
};

function ShowTooglePopup(divId) {
    $('#' + divId).show();
};
function unchekedOtherColumns(classcheckbox) {
    $("input.criteriafeature:checked").each(function() {
        if (toallproducts) {
            keys.push($(this).val() + 'A' + productid);
        } else {
            //keys.push($(this).val());
            keys.push($(this).val() + productid);
        }
    });
};

function chekedAllByClass(classchecbox) {
    $('input:checkbox:unchecked.' + classchecbox).attr('checked', true);
};
function unchekedAllByClass(classchecbox) {
    $('input:checkbox:checked.' + classchecbox).attr('checked', false);
};
function removeKeyAndProductId(checkboxvalue, productid) {
    if ($('#hdnproductids').val().indexOf(productid) != -1) {
        var tempo = $('#hdnproductids').val().split(':');
        var result = '';
        for (var i = 0; i < tempo.length; i++) {
            if (tempo[i] != productid) {
                if (result != '') {
                    result = result + ":";
                }
                result = result + tempo[i];
            }
        }
        $('#hdnproductids').val(result);
    }
    var toallproducts = CompareToAllProducts();
    var key = checkboxvalue + productid;
    if (toallproducts) {//when the option show all products is active
        key = checkboxvalue + 'A' + productid;
    }
    if ($('#hdnfeatures').val().indexOf(key) != -1) {
        var features = $('#hdnfeatures').val().split(':');
        var resultfeatures = '';
        for (var m = 0; m < features.length; m++) {
            if (features[m] != key) {
                if (resultfeatures != '') {
                    resultfeatures = resultfeatures + ":";
                }
                resultfeatures = resultfeatures + features[m];
            }
        }
        if (resultfeatures.indexOf(key) != -1) {
            if (resultfeatures.indexOf(':' + key) != -1) {
                resultfeatures = resultfeatures.replace(':' + key, '');
            } else {
                resultfeatures = resultfeatures.replace(key, '');
            }
        }
        $('#hdnfeatures').val(resultfeatures);
    }
};

function disabledCheckboxToProduct(productid, value) {
    var keys;
    var keyssel = ["BC", "MA", "MP", "MD", "LM"];

    keys = keyssel;

    for (var k = 0; k < keys.length; k++) {
        if (keys[k] != value) {
            $("input." + keys[k] + productid).each(function() {
                $(this).attr("disabled", true);
            });
        }
    }
};




function isEmpty(mytext) {
    var re = /^\s{1,}$/g; //match any white space including space, tab, form-feed, etc.
    if ((mytext.length == 0) || (mytext.search(re)) > -1 || (mytext == null)) {
        return true;
    }
    else {
        return false;
    }
};

function checkfilter(state) {

    var hdnfeat = $('#hdnfeatures').val();
    var hdnpids = $('#hdnproductids').val();
    if (hdnfeat != null && hdnfeat != undefined && hdnfeat != '' && hdnpids != null && hdnpids != undefined && hdnpids != '') {
        showConfirmFilterDialog(state);
    } else {
        toprodsfeature(state);
    }
    UpdatedOptionsToFilter();
};

function DoColorize(value) 
{
    colorizeon = !colorizeon;
    $("#radcolorizeon").prop("checked", colorizeon);

    if (colorizeon) 
        colorizeFeatureForProducts(1);
    else
        colorizeFeatureForProducts(0);
};

function ResetRelevancyFilter() {
    $('.criteriarelevancy').attr('checked', false);
};
function ResetSameValueFilter() {
    $('#rallvalues').attr('checked', true);
};
function ResetDisplayValues() 
{
    var valueDefault = '<%= ViewData["DefaultsSameValues"] %>';
    if (valueDefault != null && valueDefault != undefined) 
    {
      //Need Complete
    }
};

//Probably next functions is duplicated in result.ascx

function ResetAllFileter() {
    $('#chkcolorizeon').attr('checked', true);
    colorizeFeatureForProducts(1);
    $('#chkcolorizeoff').attr('checked', false);
    
    //restore color radio 

    $('#chkboxselprods').attr('checked', true);
    checkfilter(1);
    $('#chkboxallprods').attr('checked', false);
    ResetRelevancyFilter();
    resetfilter();
    ResetDisplayValues();
    UpdatedOptionsToFilter();
};
function resetfilter() {
    $('#hdnfeatures').val('');
    $('#hdnproductids').val('');
    //to unchecked other values
    $("input.criteriafeature:checked").each(function() {
        $(this).attr('checked', false);
    });
    var rows = $(".filtered tbody tr");
    rows.each(function() //items selected
    {
        $(this).removeClass("hidden");
    });
    var con = $(".colorizefeature:first").attr('checked');
    if (con) {
        colorizeFeatureForProducts(0);
        colorizeFeatureForProducts(1);
    }

    filterBlocks();
    UpdatedOptionsToFilter();
};



function disablerow(obj) {
    $(obj).parent().parent().parent().addClass('hidden');
}
function enablerow(obj) {
    var tblTable = $(obj).parent().parent().parent().parent();

    var tblRows = tblTable.children("tbody").children("tr");
    tblRows.removeClass('hidden');
}

function update_tools(value) {
    if (value == 1) {
        $('#comp_tools_button_close').hide();
        $('#comp_tools_button_show').show();
        $('#LegenFieldSet').hide();
        $('#LegendName').hide();
        $('#comp_tools table tbody').hide();
    }
    else {
        $('#comp_tools_button_close').show();
        $('#comp_tools_button_show').hide();
        $('#LegenFieldSet').show();
        $('#LegendName').show();
        $('#comp_tools table tbody').show();
    }
}

function saved_comparison(value) {
    if (value == 1) {
        $('#saved_comp_button_close').hide();
        $('#saved_comp_button_show').show();
        $('#headTitleComparison').hide();
        $('#bodysavedComparison').hide();
        $('#headTitleCreateComparison').hide();
        $('#bodyCreateComparison').hide();        
    }
    else {
        $('#saved_comp_button_close').show();
        $('#saved_comp_button_show').hide();
        $('#headTitleComparison').show();
        $('#bodysavedComparison').show();
        $('#headTitleCreateComparison').show();
        $('#bodyCreateComparison').show();        
    }
}

function update_silver_tools(value) {
    if (value == 1) {
        $('#silbull_tools_button_close').hide();
        $('#silbull_tools_button_show').show();
        $('#LegenFieldSet').hide();
        $('#LegendName').hide();
        $('#silbull_tools table tbody').hide();
    }
    else {
        $('#silbull_tools_button_close').show();
        $('#silbull_tools_button_show').hide();
        $('#LegenFieldSet').show();
        $('#LegendName').show();
        $('#silbull_tools table tbody').show();
    }
}


function update_headers() {
    alert($('#comp_table_result').width());
    var width = $('#comp_table_result').width();
    $('#comp_grouphead').width(width);
    $('#comp_sethead').width(width);
}



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
function GetValueByKeyAndToken(value, key, token) {
    var result = '';
    var pos = value.indexOf(key);
    if (pos != -1) {
        var subValu = value.substring(pos + key.length + 1);
        var tokenPos = subValu.indexOf(token);
        if (tokenPos != -1) {
            result = subValu.substring(0, tokenPos);
        }
        else {
            result = subValu;
        }
    }
    return result;
};



function EnableTools(checked) 
{
    if (checked) {
        $('table div.cp').show();
        $('table div.benefit').show();
        $('table div.cost').show();
        $('table div.relevancy').show();
        $('table div.indstd').show();
    }
    else {
        
        $('table div.cp').hide();
        $('table div.benefit').hide();
        $('table div.cost').hide();
        $('table div.relevancy').hide();
        $('table div.indstd').hide(); 
    }
        
};
function toprodsfeature(state) {
    var mychkcolorize = $('input[name=chkcolorize]');
    var chkcolorizeValue = mychkcolorize.filter(':checked').val();
    if (chkcolorizeValue != undefined) {
        colorizeFeatureForProducts(chkcolorizeValue);
    }
};

/*Hidden empty groups and restore functions ...*/
function emptygroups_off() 
{
    var groups = $(".comp_grouphead");
    for (k = 0; k < (groups.size() - 1); k++) {
        var current = $(groups[k]);
        var next = current.next(); //
        //alert(current.text() + "-" + next.text() + ">" + next.is(":visible"));
        if (!next.is(":visible")) //if next is not visible hidde me
        {
            current.hide(); //is possible add class for tag element
            current.addClass("comp_gh");
        }
    }
}
function emptygroups_restore() {
    $(".comp_gh").removeClass("comp_gh");
}

var htmlEncode = function(value) {
    return $('<div/>').text(value).html();
};
var htmlDecode = function(value) {
    return $('<div/>').html(value).text();
};


function taketime(expression) {
    console.time('native');
    $(expression);
    console.timeEnd('native');
}


/*Corners functions*/
function SetBenefitCorner(pwho, piid)  // piid~parameter industry id
{
    $(pwho).cluetip({ ajaxCache: false, width: '150px', closePosition: 'title', showTitle: false, cluetipClass: 'jtip', mouseOutClose: false, activation: 'click',sticky: true, arrows: true, cursor: 'pointer', urlParser: function(link) {
        var criteriaid = $(this).parent().parent().prop("id").substr(1);
        return link + '?cid=' + criteriaid + '&iid=' + piid;
    },
    onShow: function() {
        $(document).click(function(e) {
            var isInClueTip = $(e.target).closest('#cluetip');
            if (isInClueTip.length === 0) {
                $(document).trigger('hideCluetip');
            }
        })
    }
});
}
function SetCostCorner(pwho, piid) {
    $(pwho).cluetip({ ajaxCache: false, width: '150px', closePosition: 'title', showTitle: false, cluetipClass: 'jtip', mouseOutClose: false, activation: 'click', sticky: true, arrows: true, cursor: 'pointer', urlParser: function(link) {
    var criteriaid = $(this).parent().parent().prop("id").substr(1);
        return link + '?cid=' + criteriaid + '&iid=' + piid; ;
    },
    onShow: function() {
        $(document).click(function(e) {
            var isInClueTip = $(e.target).closest('#cluetip');
            if (isInClueTip.length === 0) {
                $(document).trigger('hideCluetip');
            }
        })
    }
});
}
function SetValueCorner(pwho, urlAction, piid, pU, pC,spc) {
    $(pwho).cluetip({ ajaxCache: false, width: '200px', closePosition: 'title', showTitle: false, cluetipClass: 'jtip', mouseOutClose: false, activation: 'click', sticky: true, arrows: true, cursor: 'pointer', urlParser: function(link) {
        var reftd = $(this).parent(); // <tr id"C..."><td id="v..."></td>...</tr>
        var productid = reftd.prop("id").substr(1);
        var criteriaid = reftd.parent().prop("id").substr(1);
        return urlAction + '?iid=' + piid + '&cid=' + criteriaid + '&pid=' + productid + '&U=' + pU + '&C=' + pC + '&spc='+spc;
    },
    onShow: function() {
        $(document).click(function(e) {
            var isInClueTip = $(e.target).closest('#cluetip');
            if (isInClueTip.length === 0) {
                $(document).trigger('hideCluetip');
            }
        })
    }
});
}
function SetRelevancyCorner(pwho, urlAction) {
    $(pwho).cluetip({ ajaxCache: false, width: '91px', closePosition: 'title', showTitle: false, cluetipClass: 'jtip', mouseOutClose: false, activation: 'click', sticky: true, arrows: true, cursor: 'pointer', urlParser: function(link) {
        var criteriaid = $(this).parent().parent().prop("id").substr(1);
        return urlAction + '?cid=' + criteriaid;
    },
    onShow: function() {
        $(document).click(function(e) {
            var isInClueTip = $(e.target).closest('#cluetip');
            if (isInClueTip.length === 0) {
                $(document).trigger('hideCluetip');
            }
        })
    }
});
}

function SetIndustryStandarCorner(pwho, piid) {
    $(pwho).cluetip({ ajaxCache: false, width: '150px', closePosition: 'title', showTitle: false, cluetipClass: 'jtip', mouseOutClose: false, activation: 'click', sticky: true, arrows: true, cursor: 'pointer', urlParser: function(link) {
        var criteriaid = $(this).parent().parent().prop("id").substr(1);
        return link + '?cid=' + criteriaid + '&iid=' + piid; ;
    },
        onShow: function() {
            $(document).click(function(e) {
                var isInClueTip = $(e.target).closest('#cluetip');
                if (isInClueTip.length === 0) {
                    $(document).trigger('hideCluetip');
                }
            })
        }
    });
}

function EnqueueUpdateCorner(criteriaid, productid) //enqueue event in global variable
{
    comp_events = { cid: criteriaid, pid: productid };
}

function updateCorner() {
    //console.log("running updateCorner()");
    if (comp_events != null) {
        var data = comp_events;
        $("#C" + data.cid).find("#P" + data.pid).find("div:first").next().addClass("comp_crbr");
        comp_events = null;
    }
}