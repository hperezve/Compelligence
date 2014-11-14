<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<style type="text/css">
    



</style>

<script type="text/javascript">
    var showLoadingDialog = function() {

        $.blockUI({ message: $('#LoadingDialog'),
            css: { width: '20%', margin: 'auto' }
        });
    };
    var hideLoadingDialog = function() {
        $.unblockUI();
    };
    function updatCompetitorDdl() {
        var indValues = new Array(); ///--- ARRAY TO SET THE IDS TO INDUSTRIES SELECTED
        $('input[name=multiselect_IndustryIds]:checked').each(function(i, checked) {///----- FOR EACH INDUSTRY CHECKED WILL GET THE VALUE
            indValues.push($(checked).val());
        });

        var comValues = new Array(); ///--- ARRAY TO SET THE IDS TO COMPETITORS SELECTED
        $('input[name=multiselect_CompetitorIds]:checked').each(function(i, checked) {///----- FOR EACH INDUSTRY CHECKED WILL GET THE VALUE
            comValues.push($(checked).val());
        });

        var urlGetCompetitors = '<%= Url.Action("GetProductsByCompetitor", "Report") %>';
        $.ajax({
            type: "POST",
            url: urlGetCompetitors,
            data: { ids: indValues.join(";"), cps: comValues.join(";") }, //// THE IDS(iNdUSTRY) WILL SEND TO CONTROLLER WITH ; SEPARATOR AND THE CPS(cOMpTITOR)
            dataType: 'json',
            beforeSend: function() {
                showLoadingDialog();
            },

            success: function(json) {
                var items = "";
                $.each(json, function(i, item) {
                    items += "<option value='" + item.Value + "' " + item.Disabled + " >" + item.Text + "</option>";
                })
                $("#ProductIds").html(items);
            }, complete: function() {
                hideLoadingDialog();
                $("#ProductIds").multiselect('refresh');
            }
        })
    };
    function updCompetitorDropDown() {
        $('input[name="multiselect_CompetitorIds"]').click(function(Comp) {
            updatCompetitorDdl();
        });
    };
    function updatIndustryDdl() {
        var indValues = new Array(); ///--- ARRAY TO SET THE IDS TO INDUSTRIES SELECTED
        $('input[name=multiselect_IndustryIds]:checked').each(function(i, checked) {///----- FOR EACH INDUSTRY CHECKED WILL GET THE VALUE
            indValues.push($(checked).val());
        });
        var urls = '<%= Url.Action("GetCompetitorsByIndustry", "Report") %>';
        $.ajax({
            type: "POST",
            url: urls,
            data: { ids: indValues.join(";") }, //// THE IDS(iNdUSTRY) WILL SEND TO CONTROLLER WITH ; SEPARATOR
            dataType: 'json',
            beforeSend: function() {
                showLoadingDialog();
            },
            success: function(json) {
                var items = "";
                var items1 = "";
                var lista = "";
                $.each(json, function(i, item) {
                    items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                })

                $("#CompetitorIds").html(items);
                $("#ProductIds").html(items1);
            }, complete: function() {
                hideLoadingDialog();
                $("#CompetitorIds").multiselect('refresh');
                $("#ProductIds").multiselect('refresh');

                updCompetitorDropDown();

            } //complete
        })

    };
    function updIndustryDropDown() {
        $('input[name="multiselect_IndustryIds"]').click(function(Ind) {
            updatIndustryDdl();
        });
    };
    function setMultiselectFeature() {
        $('#IndustryIds').multiselect({
            //header: 'Select a Industry' ///-------- IF SHOW HEADER THEN NO SHOW SELECT ALL AND UNSELECT ALL
            noneSelectedText: 'Select a Industry'
        , multiple: true
        , clas_ajust: "adjust-text-344"
        , classes: "w344"
        , checkAll: function() {
            setTimeout(function() { updatIndustryDdl(); }, 500);
        }
        , uncheckAll: function() {
            setTimeout(function() { updatIndustryDdl(); }, 500);
        }
            //,maxWidth: 400
            //, click: function(event, ui) { updCompetitors('#'); }
        }).multiselectfilter();
        $('#CompetitorIds').multiselect({
            //header: 'Select a Industry' ///-------- IF SHOW HEADER THEN NO SHOW SELECT ALL AND UNSELECT ALL
            noneSelectedText: 'Select a Competitor'
        , multiple: true
        , clas_ajust: "adjust-text-344"
        , classes: "w344"
        , checkAll: function() {
            setTimeout(function() { updatCompetitorDdl(); }, 500);
        }
        , uncheckAll: function() {
            setTimeout(function() { updatCompetitorDdl(); }, 500);
        }
            //,maxWidth: 400
        }).multiselectfilter();
        $('#ProductIds').multiselect({
            //header: 'Select a Industry' ///-------- IF SHOW HEADER THEN NO SHOW SELECT ALL AND UNSELECT ALL
            noneSelectedText: 'Select a Product'
        , multiple: true
        , clas_ajust: "adjust-text-344"
        , classes: "w344"
            //,maxWidth: 400
        }).multiselectfilter();
    };
    $(function() {
        setMultiselectFeature();
        updIndustryDropDown();
    });
    $(document).ready(function() {
        $('#EndDateFrm').datepicker();
        $('#StartDateFrm').datepicker();
    });
    var drawChart = function(data) {
        $('d3Content').empty();
        $('.div-lbl-titl').show();
        var productIds = new Array();
        var productNames = new Array();
        var countComparisons = new Array();
        var numberOfComparisons = data;
        var arrayNC = numberOfComparisons.split(';');
        var maxValue = 0;
        var longName = 0;
        var marginBotton = 70; ///--- BOTON DEFAULT
        var data = [];
        // TO GET THE NUMBERS OF COMPARISONS
        for (var nc = 0; nc < arrayNC.length; nc++) {
            var ncItem = arrayNC[nc].split('_');
            if (maxValue < parseFloat(ncItem[1])) maxValue = parseFloat(ncItem[1]);
            if (longName < ncItem[2].length) longName = ncItem[2].length;
            countComparisons.push(ncItem[1]);
            productNames.push(ncItem[2]);
            data.push({
                id: ncItem[2]
                    , value: parseFloat(ncItem[1])
            });
        }
        if (longName != undefined && longName != null && longName > 7) {////--- IF EXIST PRODUCT NAMES MANY LONG 
            marginBotton = (30 + (longName * 5));
        }



        //// declare d3
        var margin = { top: 20, right: 20, bottom: marginBotton, left: 60 }, width = 980 - margin.left - margin.right, height = 540 - margin.top - margin.bottom;
        var barPadding = 1;
        var scaleNumber = maxValue / 5;
        //$('#divChart').css('height', height + 'px');
        var rangeColor = Math.round(255 / arrayNC.length);
        var counterColor = 0;
        var x = d3.scale.ordinal().rangeRoundBands([0, width], .1);
        var y = d3.scale.linear().range([height, 0]);

        var commaFormat = d3.format(',');

        var changeColor = 1;
        var xAxis = d3.svg.axis().scale(x).orient("bottom");

        var yAxis = d3.svg.axis().scale(y).orient("left").ticks(20, "#");
        // ------- CREATE SVG ELEMENT
        var svg = d3.select("d3Content")
        .append("svg")
        .attr("width", width + margin.left + margin.right)//---------- SET WIDTH
        .attr("height", height + margin.top + margin.bottom)//-------- SET HEIGHT
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        x.domain(data.map(function(d) { return d.id; }));
        y.domain([0, d3.max(data, function(d) { return d.value; })]); ///------ TO GET THE MAX VALUE

        ///---- DRAW AXIS X AND Y
        svg.append("g").attr("class", "x axis").attr("transform", "translate(0," + height + ")").call(xAxis).selectAll("text").style("text-anchor", "end")
        .attr("dx", "-.8em")
        .attr("dy", ".15em")
        .attr("transform", function(d) {
            return "rotate(-65)"
        });

        svg.append("g").attr("class", "y axis").call(yAxis).append("text").attr("transform", "rotate(-90)").attr("x", 0 - (height / 2)).attr("y", 0 - margin.left).attr("dy", ".71em").style("text-anchor", "middle").text("Number of Comparisons");

        ///---- DRAW BARS AND LABES WITH VALUES
        svg.selectAll(".bar")
        .data(data)//----------- SET DATA AND SEE THAT  HAVE # OF VALUES
        .enter() //-------------  IT CALL # TIMES
        .append("rect") //-------- GENERATE RECTS AND ADD TO SVG
        .attr("class", "bar")
        .attr("fill", function(d) { counterColor += rangeColor; return "rgb(0, 0," + counterColor + ")"; })
        .attr("x", function(d) { return x(d.id); }).attr("width", x.rangeBand())
        .attr("y", function(d) { return y(d.value); }).attr("height", function(d) { return height - y(d.value); })
        .on("mouseover", function(d, i) {
            var xPosition = i * (width / data.length) + (width / data.length - barPadding) / 2;
            var yPosition = (height/2 + y(d.value)+90);

            //Update Tooltip Position & value
            d3.select("#d3tooltip")
            .style("left", xPosition + "px")
            .style("top", yPosition + "px");
            
            d3.select("#d3tooltip")
            .select("#numberComparisons")
            .text(commaFormat(d.value));

            d3.select("#d3tooltip")
            .select("#keyword")
            .style("color", d3.select(this).style("fill"))
            .text(d.id);
            
            d3.select("#d3tooltip").classed("hidden", false);
                                                                                                                     
        })
        .on("mouseout", function () {
            //Remove the d3tooltip
            d3.select("#d3tooltip").classed("hidden", true);
        });

        svg.selectAll("text.labels")
			   .data(data)
			   .enter()
			   .append("text")
			   .text(function(d) {
			       return d.value;
			   })
			   .attr("text-anchor", "middle")
			   .attr("x", function(d, i) {
			       return i * (width / data.length) + (width / data.length - barPadding) / 2;
			   })
			   .attr("y", function(d) {
			       var hSV = 0;
			       if (height > (y(d.value) + 16)) {
			           hSV = y(d.value) + 14;
			       } else { hSV = y(d.value) - 6 }
			       return hSV;
			   })
			   .attr("font-family", "sans-serif")
			   .attr("font-size", "11px")
			   .attr("fill", function(d) {
			       if (height > (y(d.value) + 16)) {
			           return "white";
			       } else { return "black" }
			   });
    };
    var loadTable = function(data) {
        $('.tbl_comparisons').remove();
        $('.tbl-comp-resu').show();
        var numberOfComparisons = data;
        var arrayNC = numberOfComparisons.split(';');
        for (var nc = 0; nc < arrayNC.length; nc++) {
            var ncItem = arrayNC[nc].split('_');
            $('#tblComparisonResult tr:last').after('<tr class="tbl_comparisons"><td>' + ncItem[0] + '</td><td>' + ncItem[1] + '</td><td>' + ncItem[2] + '</td><td>' + ncItem[3] + '</td></tr>');
        }
        hideLoadingDialog();
    };
    var loadChart = function() {
        var indValues = new Array();
        var comValues = new Array();
        var proValues = new Array();
        $('#IndustryIds :selected').each(function(i, selected) {
            indValues.push($(selected).val());
        });
        if (indValues.length > 0) {
            $('#CompetitorIds :selected').each(function(i, selected) {
                comValues.push($(selected).val());
            });
            if (comValues.length > 0) {
                $('#ProductIds :selected').each(function(i, selected) {
                    proValues.push($(selected).val());
                });
                var startDate = $('#StartDateFrm').val();
                var endDate = $('#EndDateFrm').val();
                var ddlSorting = $('#ddlSorting').val();
                if (proValues.length > 0) {
                    showLoadingDialog();
                    var parameters = { ind: indValues.join(";"), com: comValues.join(";"), pro: proValues.join(";"), sDate: startDate, eDate: endDate, sort: ddlSorting };
                    var results = null;
                    $.get('<%= Url.Action("GetDataComparison", "Report") %>', parameters, function(data) {
                        if (data != undefined && data != null && data != '') {
                            drawChart(data);
                            $.get('<%= Url.Action("GetDataComparisonByIndustry", "Report") %>', parameters, function(data) {
                                if (data != undefined && data != null && data != '') {
                                    loadTable(data);
                                }
                            });
                        } else {
                            $('.tbl_comparisons').remove();
                            $('.tbl-comp-resu').hide();
                            $('.div-lbl-titl').hide();
                            $('d3Content').empty();
                        }
                        hideLoadingDialog();
                    });
                } else {
                    hideLoadingDialog();
                    var Message = '<p>You must add a product to run this report.</p>';
                    MessageBackEndDialog('Error Message', Message);
                }
            } else {
                hideLoadingDialog();
                var Message = '<p>You must add a competitor and a product to run this report.</p>';
                MessageBackEndDialog('Error Message', Message);
            }
        } else {
            hideLoadingDialog();
            var Message = '<p>You must add an industry, competitor, and product to run this report.</p>';
            MessageBackEndDialog('Error Message', Message);
        }
    };
</script>

<div id="ReportsModuleContent" style="height: auto;margin-bottom: 20px;">
    <div id="filterComparison">
        <div class="line">
            <label style="margin-left: 12px;" class="title-rep-comp">
                Report Filter</label></div>
        <div class="line">
            <div class="field">
                <label for="IndustryId" style="float: left">
                    Industry</label>
                <%= Html.DropDownList("IndustryIds", (MultiSelectList)ViewData["IndustryList"], string.Empty, new { id = "IndustryIds",  multiple = "multiple" })%>
            </div>
            <div class="field">
                <label for="CompetitorId" style="float: left">
                    Competitor</label>
                <%= Html.DropDownList("CompetitorIds", (MultiSelectList)ViewData["CompetitorList"], string.Empty, new { id = "CompetitorIds",  multiple = "multiple" })%>
            </div>
            <div class="field">
                <label for="ProductId" style="float: left">
                    Product</label>
                <%= Html.DropDownList("ProductIds", (MultiSelectList)ViewData["ProductList"], string.Empty, new { id = "ProductIds",  multiple = "multiple" })%>
            </div>
        </div>
        <div class="line" style="margin-top: 5px; border-top: 2px solid gray; margin-left: 12px;
            padding-top: 5px;">
            <label class="title-rep-comp">
                Date Filter</label></div>
        <div class="line">
            <div class="field">
                <label>
                    Start Date</label>
                <%= Html.TextBox("StartDateFrm", null, new { id = "StartDateFrm", Class = "Date" })%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label>
                    End Date</label>
                <%= Html.TextBox("EndDateFrm", null, new { id = "EndDateFrm", Class = "Date" })%>
            </div>
        </div>
        <div class="line" style="margin-top: 5px; border-top: 2px solid gray; margin-left: 12px;
            padding-top: 5px;">
            <label  class="title-rep-comp">
                Advance Filter</label></div>
        <div class="line">
            <div class="field">
                <label>
                    Sorting</label>
                <select id="ddlSorting">
                    <option value=""></option>
                    <option value="ASC">Ascending </option>
                    <option value="DSC">Descending</option>
                </select>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <input type="button" id="btnLoadChart" value="Generate" onclick="loadChart();" />
            </div>
        </div>
    </div>
    <div>
        <div class="line" style="width:100%;">
            <table id="tblChart" style="width:100%;">
                <col width="33%">
                <col width="33%">
                <col width="33%">
                <tr>
                    <td width="33%">
                    </td>
                    <td width="33%">
                        <div>
                            <div class="div-lbl-titl title-rep-comp" style="display:none;text-align: center; margin-top: 5px;"><label>Comparison Report</label></div>
                            <div id="divChart" style="width: 80%">
                                <d3content id="chart"></d3content>
                            </div>
                            <div id="d3tooltip" class="hidden">
                                <p class="heading"><span id="keyword"></span></p>
                                <p class="heading">Number of Comparisons: <span id="numberComparisons"></span></p>
                            </div>
                        </div>
                    </td>
                    <td width="33%">
                    </td>
                </tr>
            </table>
        </div>
        <div class="line">
            <div class="div-lbl-titl title-rep-comp" style="display:none;margin: 5px auto; text-align: center;"><label>Number comparison by Industry</label></div>
            <table id="tblComparisonResult" class="tbl-comp-resu" style="display: none; width: 60%;">
                <tbody>
                    <tr>
                        <th>
                            Industry
                        </th>
                        <th>
                            Competitor
                        </th>
                        <th>
                            Product
                        </th>
                        <th>
                            Number of Comparisons
                        </th>
                    </tr>
                </tbody>
                <tr>
                </tr>
            </table>
        </div>
    </div>
    <div>
    </div>
</div>
<div id="LoadingDialog" class="displayNone">
    <p>
        <img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt="" class="left" /><span
            class="loadingDialog">Loading ...</span>
    </p>
</div>
