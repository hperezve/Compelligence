//Text Filter
function ApplyTextFilter() //Only Text
{

    var filter = $('#txtFilter').val(); //get current text for apply filter
    var rows = $(".filtered tbody tr"); //get all rows of entire document, think in use global

    //apply filter
    rows.each(function() //items selected
    {
        if ($(this).text().search(new RegExp(filter, "i")) < 0) {
            $(this).addClass("hidden");
        } else {
            $(this).removeClass("hidden");
        }
    });

    //Disable Set Header
    //filterBlocks();
}


//Feature Filter
function ApplyFeatureFilter(productid, control)  //Pure
{
    var lstproducts = $(".lstproducts"); //look at filter by product on headers
    for (q = 0; q < lstproducts.size(); q++) {
        var fvalue = $(lstproducts[q]).data("header_filter");
        console.log(fvalue.features);
        if (fvalue.id == productid) {
            if (control.checked)
                fvalue.features.push($(control).val());
            else
                fvalue.features.destroy($(control).val());
            break;
        }
    }

    var rows = $(".filtered tbody tr");

    rows.each(function() //items selected
    {
        var foundfeature = 0;
        for (q = 0; q < lstproducts.size(); q++) //Every row need verify by own header filter
        {
            var fvalue = $(lstproducts[q]).data("header_filter");
            var currenttd = $(this).find("#P" + fvalue.id); // <td class="NF" id="P19071721096825971">....
            //if currenttd has any element of header column filter
            var hasfeaturetd = 0;
            for (l = 0; l < fvalue.features.length; l++) {
                var currentfeature = fvalue.features[l];
                if (currenttd.hasClass(currentfeature)) {
                    hasfeaturetd++;
                }

            }
            if (hasfeaturetd > 0)
                foundfeature++;
        }
        if (foundfeature == 0)
            $(this).addClass("hidden");
        else
            $(this).removeClass("hidden");
    });
}

//Relevent Filter
function ApplyRelevantFilter() //Pure
{
    var keys = ["BC", "MA", "MP", "MD", "LM"];
    if ($('#chkrelevant').prop("checked")) {
        $('#txtFilter').val("");
        $("input.criteriafeature").prop("checked", "checked");
        $(".colorizefeature:first").prop('checked', 'checked');
        var rowsnorelevants = $(".filtered tbody tr[relevant='N']");
        //rowsnorelevants.addClass('hidden');
        rowsnorelevants.each(function() //items selected
        {
            var kc = 0;
            for (var i = 0; i < keys.length; i++) {
                var si = $(this).find("td." + keys[i]).size();
                kc = kc + si;
            }

            if (kc == 0) //don't have key
            {
                $(this).addClass("hidden");
            }
        });
        colorizeFeature(1);
    }

    else {
        $('#txtFilter').val("");
        $("input.criteriafeature").prop("checked", "");
        $(".colorizefeature:first").prop('checked', '');
        var rowsnorelevants = $(".filtered tbody tr[relevant='N']");
        rowsnorelevants.removeClass('hidden');
        colorizeFeature(0);
    }

    //try hidden tables
    filterBlocks();
}





function gettextfilter() //return string
{
    return $('#txtFilter').val(); //get current text for apply filter
}

function getfeatfilter() //return object array
{
    var lstproducts = $(".lstproducts"); //look at filter by product on headers
    var result = [];
    for (q = 0; q < lstproducts.size(); q++) //Every row need verify by own header filter
    {
        var fvalue = $(lstproducts[q]).data("header_filter");
        result.push(fvalue);
    }
    return result;//[,,,]
}

function getrelefilter() //return object array
{
    var relevantchecks = $(".criteriarelevancy:checked"); //every time retrieve 3 elements
    var result = [];
    relevantchecks.each(function(i, e) {
        //Every check selectedç
        result.push($(e).val());
    });
    
    return result;
}

function getsamedifffilter() 
{ 
   var all=$("#rallvalues").prop("checked");
   var same=$("#ronlysamevalues").prop("checked");
   var diff = $("#ronlydiff").prop("checked");
   return all ? 1 : ( same ? 2: 3 );
} 


//Update Feature Filter Cache, Call from ApplyDynamicFilter
function UpdateFeatureFilter(productid, control) 
{
    //<input ... class="lstproducts" value="productid"/>              
    var lstproducts = $(".lstproducts"); //look at filter by product on headers
    for (q = 0; q < lstproducts.size(); q++) {
        var fvalue = $(lstproducts[q]).data("header_filter");
        console.log(fvalue.features);
        if (fvalue.id == productid) {
            if (control.checked) {
                fvalue.features.push($(control).val());
                $('input:checkbox[id^="chkmarketdfeature"]:not(:checked):not(:disabled)').each(function() {//get all checkbox no checked and enabled 
                    $(this).attr("disabled", "disabled");//set attribute disabled
                });
            }
            else
                fvalue.features.destroy($(control).val());
            break;
        }
    }

}
function ReloadCheckbocksEnalbed(productid, control) {
    var lstproducts = $(".lstproducts"); //look at filter by product on headers
    for (q = 0; q < lstproducts.size(); q++) {
        var fvalue = $(lstproducts[q]).data("header_filter");
        console.log(fvalue.features);
        if (fvalue.id == productid) {
            if (control.checked) {
                fvalue.features.push($(control).val());
                $('input:checkbox[id^="chkmarketdfeature"]:not(:checked):not(:disabled)').each(function() {
                    $(this).attr("disabled", "disabled");
                });
            }
            else {

                var features = '';
                features = $.param({ features: fvalue.features });
                if (features == []) features = $.param({ features: ["NF"] });
                var bc = 0, ma = 0, mp = 0, md = 0, lm = 0;
                var currcol = $(".comp_table tbody tr td[id='P" + productid + "']");
                currcol.each(function(index, element) {
                    var curr_td = $(element);
                    //if (!curr_td.parent().hasClass("hidden")) 
                    {
                        if (curr_td.hasClass("BC")) bc++;
                        if (curr_td.hasClass("MA")) ma++;
                        if (curr_td.hasClass("MP")) mp++;
                        if (curr_td.hasClass("MD")) md++;
                        if (curr_td.hasClass("LM")) lm++;
                    }

                });
                if (bc > 0) { $('input:checkbox[id^="chkmarketdfeature"][value=BC]:not(:checked):disabled').removeAttr("disabled"); }
                if (ma > 0) { $('input:checkbox[id^="chkmarketdfeature"][value=MA]:not(:checked):disabled').removeAttr("disabled"); }
                if (mp > 0) { $('input:checkbox[id^="chkmarketdfeature"][value=MP]:not(:checked):disabled').removeAttr("disabled"); }
                if (md > 0) { $('input:checkbox[id^="chkmarketdfeature"][value=MD]:not(:checked):disabled').removeAttr("disabled"); }
                if (lm > 0) { $('input:checkbox[id^="chkmarketdfeature"][value=LM]:not(:checked):disabled').removeAttr("disabled"); }
                //fvalue.features.destroy($(control).val());
            }
            break;
        }
    }
};
function FeatureFilter(productid, control) //update filter information
{
    UpdateFeatureFilter(productid, control);  //Update Header Filters
    ApplyDynamicFilter(); //Apply
    ReloadCheckbocksEnalbed(productid, control);//Reload the checkbox enabled to Filter by Ranking
}

/*
* Apply three mix filters Text/Feature/Relevant
*/

//when change options with radio buttons
function ApplyDynamicFilter()
{
    //start filter
    var keys = [];

    //Configure
    var textfilter = gettextfilter(); //Retrieve Text entered ""
    var featfilter = getfeatfilter(); //Retrieve features by product []
    var relefilter = getrelefilter(); //Retrieve list of relevancies selected []
    var samefilter = getsamedifffilter();

    console.log("---------");
    console.log("samefilter: "+samefilter);
    console.log("---------");

    ///************ HIDDEN ROWS TO SHOW SHOW ALL VALUES, ONLY SAME VALUES AND ONLY DIFERENCES FILTER
    if (samefilter == 2) //same, then disable diff
    {
        $(".filtered tbody tr.comp_neq").addClass("hidden");
        var rows = $(".filtered tbody tr.comp_eq");
    }
    else if (samefilter == 3) //different, then disable same
    {
        $(".filtered tbody tr.comp_eq").addClass("hidden");
        var rows = $(".filtered tbody tr.comp_neq");
    }
    else
        var rows = $(".filtered tbody tr");
    
    console.log("rows: " + rows.size());

    //**************** FEATURES FILTER
    var setfeatures = 0;
    for (q = 0; q < featfilter.length; q++) //Every row need verify by own header filter
    {
        var fvalue = featfilter[q];
        for (l = 0; l < fvalue.features.length; l++) {
            setfeatures++;
        }
    }
    
    
    var whichfilter=0;
   // if /*have least one.*/ and/or /*have least one character.*/ and/or  /*have least one checked*/  )
    if (setfeatures > 0 && textfilter.length > 0 && relefilter.length > 0 ) //three filters
       whichfilter=111;
     else if ( (setfeatures > 0 && textfilter.length > 0 && relefilter.length==0) ) //two filters
           whichfilter=110;
     else if ( (setfeatures==0 && textfilter.length > 0 && relefilter.length>0) ) //two filters
           whichfilter=011;
     else if ( (setfeatures>0 && textfilter.length==0 && relefilter.length>0) ) //two filters
           whichfilter=101;
     else if ( (setfeatures>0 && textfilter.length==0 && relefilter.length==0) ) //one filter
           whichfilter=100; 
     else if ( (setfeatures==0 && textfilter.length>0 && relefilter.length==0) ) //one filter
           whichfilter=010;
     else if ( (setfeatures==0 && textfilter.length==0 && relefilter.length>0) ) //one filter
           whichfilter=001;


    console.log(whichfilter);
       
    count = 0;
    rows.each(function() //items selected
    {
        $(this).removeClass("hidden"); //By Default remove and next set

        //Process for Feature Filter
        var foundfeature = 0;
        for (q = 0; q < featfilter.length; q++) //Every row need verify by own header filter
        {
            var fvalue = featfilter[q];
            var currenttd = $(this).find("#P" + fvalue.id); // <td class="NF" id="P19071721096825971">....
            //if currenttd has any element of header column filter
            var hasfeaturetd = 0;
            for (l = 0; l < fvalue.features.length; l++) {
                var currentfeature = fvalue.features[l];
                if (currenttd.hasClass(currentfeature)) {
                    hasfeaturetd++;
                    //console.log("hasfeaturetd=" + hasfeaturetd);
                }

            }
            //console.log("for>hasfeaturetd=" + hasfeaturetd);
            if (hasfeaturetd == fvalue.features.length)
                foundfeature++;
        }
        //console.log("founfeature=" + foundfeature);

        if (foundfeature == featfilter.length) //exactly
            foundfeature = 1;
        else
            foundfeature = 0; //restart counter


        //Process for Text Filter
        var foundtext = 0;
        if ($(this).text().search(new RegExp(textfilter, "i")) >= 0) //if < 0, then not have
        {
            foundtext++;
        }

        //Process for Relevant Filter
        var foundrelevant = 0;
        for (ir = 0; ir < relefilter.length; ir++) {
            if ($(this).attr("relevant") == relefilter[ir]) //
            {
                foundrelevant++;
            }
        }

        console.log("ffea : " + foundfeature + " ftext: " + foundtext + " frele: " + foundrelevant);

        if (whichfilter == 111) {
            if (foundfeature == 0 || foundtext == 0 || foundrelevant == 0)
                $(this).addClass("hidden");
        }
        else if (whichfilter == 110) {
            if (foundfeature == 0 || foundtext == 0)
                $(this).addClass("hidden");
        }
        else if (whichfilter == 011) {
            if (foundtext == 0 || foundrelevant == 0)
                $(this).addClass("hidden");
        }
        else if (whichfilter == 101) {
            if (foundfeature == 0 || foundrelevant == 0)
                $(this).addClass("hidden");
        }
        else if (whichfilter == 100) {
            if (foundfeature == 0)
                $(this).addClass("hidden");
        }
        else if (whichfilter == 010) {
            if (foundtext == 0)
                $(this).addClass("hidden");
        }
        else if (whichfilter == 001) {
            if (foundrelevant == 0)
                $(this).addClass("hidden");

        }


    });
    filterBlocks(); //hidden empty tables
    DisplayFirstRowFeature();
}


function SwapFeature(source) 
{
  var rows = $(".filtered tbody tr");
  if (source == 1) //from is Selected
  {
      rows.each(function() //items selected
      {
          var cols = $(this).children("td");
          cols.each(function() {
              $(this).attr("class", $(this).attr("s"));
          });
      });
    
  }
  else  //from all
  {
      rows.each(function() //items selected
      {
          var cols = $(this).children("td");
          cols.each(function() {
              $(this).attr("class", $(this).attr("a"));
      });
      });

  }
  if ($("#radcolorizeon").prop("checked")) //it's active colorize
  {
      //if enable colorize, apply
      colorizeFeatureForProducts(0); //1:colorize, 0: uncolorize
      colorizeFeatureForProducts(1); //1:colorize, 0: uncolorize
  }
  //filterBlocks(); //hidden empty tables
  ApplyDynamicFilter();
}