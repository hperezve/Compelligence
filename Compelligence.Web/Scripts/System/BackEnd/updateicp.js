// use jquery
//
//



//utility functions
function updateUpHeight(reference) {
    var newheight = ($(reference)[0].options.length) * 16 + 10;
    $(reference).css('height', newheight + "px");
}


var updCompetitors = function(prefix) //#Deal
{
    var productids = getOptionValues(prefix + 'ProductIds', true); //id1 | id1,id2,id3,...
    var competitorids = getOptionValues(prefix + 'CompetitorIds', true); //id1 | id1,id2,id3,...
    var industryids = getOptionValues(prefix + 'IndustryIds', true); //id1 | id1,id2,id3,...

    //reset competitorids and productids
    $(prefix + 'CompetitorIds')[0].options.length = 0;
    $(prefix + 'ProductIds')[0].options.length = 0;

    //console.log("Ps:" + productids); //enable for debug
    updCompetitorMasiveList(prefix, industryids, competitorids, productids);

    //remember get only products of competitors selected
    setTimeout(function() {
        //console.log("Ps:" + productids);
        updProductMasiveList(prefix, competitorids, productids, industryids)

    }, 1000);

    //alert(typeof (updateComboPrimaryIndustry));
    //it's
    
};


function updCompetitorMasiveList(prefix, industryids, competitorids, productids) {
    var url = ubfcompetitors + '?ids=' + industryids; //append industrires
    $.post(url, function(data) {

        if (data != "") {
            var lstcompetitors = data.split("_");
            var voptions = $(prefix + 'CompetitorIds')[0].options;
            for (j = 0; j < lstcompetitors.length; j++) {
                var vt = lstcompetitors[j].split(":"); //v1:t1
                //alert(lstcompetitors[j]);
                addOption(voptions, vt[1], vt[0], competitorids);
            }
            updateUpHeight(prefix + 'CompetitorIds');
        }
    });
}


function updProductMasiveList(prefix, competitorids, productids, industryids) {
    var url = ubfproducts + '?idCompetitors=' + competitorids + '&idIndustries=' + industryids; //append competitors and industries
    $.post(url, function(data) {
        if (data != "") {
            var lstproducts = data.split("_"); //v1:t1_v2:t2_v3:t3_ ...
            var voptions = $(prefix + 'ProductIds')[0].options; //[<opt0>,<opt1>,...]
            for (j = 0; j < lstproducts.length; j++) {
                var vproduct = lstproducts[j].split(":"); //v1:t1
                addOption(voptions, vproduct[1], vproduct[0], productids);
            }
            //console.log(" Ps:" + productids);
            updateUpHeight(prefix + 'ProductIds');
        }
    });
}

var updProducts = function(prefix) //#Deal
{
    var realcompetitors = getOptionPairs(prefix + 'CompetitorIds', true);
    var productids = getOptionValues(prefix + 'ProductIds', true); //id1 | id1,id2,id3,...
    var competitorids = getOptionValues(prefix + 'CompetitorIds', true); //id1 | id1,id2,id3,...
    var industryids = getOptionValues(prefix + 'IndustryIds', true); //id1 | id1,id2,id3,...

    $(prefix + 'ProductIds')[0].options.length = 0;
    updProductMasiveList(prefix, competitorids, productids, industryids)
    
};
