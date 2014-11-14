/*
* uses jQuery
*
*
*
*/

var ShowIndustriesByHierarchy = function(prefix, chxbox) {

var industryids = getOptionValues(prefix + 'IndustryIds', true); //id1 | id1,id2,id3,...
    var url = ubfhierarchy + '?IsChecked=' + chxbox.checked + '&IndustryIds=' + industryids;
   // alert(ubfhierarchy);
    $.post(url, function(result) {
        if (result != "") {
            var arrayIndustries = result.split("_");
            var options = $(prefix + 'IndustryIds').prop('options');
            $(prefix + 'IndustryIds')[0].options.length = 0;
            for (j = 0; j < arrayIndustries.length; j++) {

                var arrayCompet = arrayIndustries[j].split(":");
                if (arrayCompet[2] == 'True') {
                    options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
                }
                else {
                    options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
                }
            }
            fixOptionTitle(prefix + "IndustryIds");
        }
    });

};
