function getOptionValues(targetid) {
    var dynamicretrieve = "";
    if ((arguments.length == 2) && (arguments[1] == true))
        dynamicretrieve += ":selected";
    return $(targetid + ' option' + dynamicretrieve).map(function(i, e) { return $(this).val(); }).get();
}

function getOptionTexts(targetid) {
    var dynamicretrieve = "";
    if ((arguments.length == 2) && (arguments[1] == true))
        dynamicretrieve += ":selected";

    return $(targetid + ' option' + dynamicretrieve).map(function(i, e) { return $(this).text(); }).get();
}
function getOptionPairs(targetid) {
    var dynamicretrieve = "";
    if ((arguments.length == 2) && (arguments[1] == true))
        dynamicretrieve += ":selected";

    return $(targetid + ' option' + dynamicretrieve).map(function(i, e) { return { value: this.value, text: this.text }; }).get();
}

function fixOptionTitle(targetid) {
    $(targetid + " option").each(function() {
    $(this).prop("title", $(this).text());
    });
}

function addOptionIf(options, value, text, arrayValues, arrayValues2) {
    if ($.inArray(value, arrayValues) == -1) {
        var ilast = options.length;
        if ($.inArray(value, arrayValues2) == -1)
            options[ilast] = new Option(text, value, true, false);
        else
            options[ilast] = new Option(text, value, true, true);
        options[ilast].title = text; //verify if works on safari
    }
}

function appendOption(options, newoptions, valuefs) {
    if (newoptions != "") {
        for (i = 0; i < newoptions.length; i++) {
            var ilast = options.length;
            if (valuefs != "" && newoptions[i].value == valuefs)
                options[ilast] = new Option(newoptions[i].text, newoptions[i].value, true, true);
            else
                options[ilast] = new Option(newoptions[i].text, newoptions[i].value, true, false);
            options[ilast].title = newoptions[i].text; //verify if works on safari
        }
    }
}


function inOptions(value, voptions) {
    for (i = 0; i < voptions.length; i++) {
        if (value == voptions[i].value) return i;
    }
    return -1;
}

//change for use option
function addOption(voptions, vtext, vvalue, arrayValues2) //options, option, productids
{
    if (inOptions(vvalue, voptions) == -1) {
        var vlast = voptions.length;
        if ($.inArray(vvalue, arrayValues2) == -1)
            voptions[vlast] = new Option(vtext, vvalue, true, false);
        else
            voptions[vlast] = new Option(vtext, vvalue, true, true);
        voptions[vlast].title = vtext; //verify if works on safari
    }
}

function addOptionIf2(options, value, text, arrayValues, arrayValues2, reffunction, refparams) {
    if ($.inArray(value, arrayValues) == -1) {
        if ($.inArray(value, arrayValues2) == -1)
            options[options.length] = new Option(text, value, true, false);
        else {
            options[options.length] = new Option(text, value, true, true);
            if (typeof reffunction == 'function') //is a function
            {
                reffunction.apply(this, refparams); //call function with parameters [param1,param2,...]
            }
        }
        options[options.length].title = text; //verify if works on safari
    }
}

//change for use option
function addOptionWithCall(voptions, vtext, vvalue, arrayValues2, reffunction, refparams) //options, option, productids
{
    if (inOptions(vvalue, voptions) == -1) {
        var vlast = voptions.length;
        if ($.inArray(vvalue, arrayValues2) == -1)
            voptions[vlast] = new Option(vtext, vvalue, true, false);
        else {
            voptions[vlast] = new Option(vtext, vvalue, true, true);
            if (typeof reffunction == 'function') //is a function
            {
                reffunction.apply(this, refparams); //call function with parameters [param1,param2,...]
            }
        }
        voptions[vlast].title = vtext; //verify if works on safari
    }
}

function updateOptionStatus(sourceselectid, list/*l1,l2,l3,...*/, status) {
    var arraylist = [];
    if (list != null && list != undefined && list != '')
        arraylist = list.split(',');

    $(sourceselectid + " option").each(function() {
        for (var j = 0; j < arraylist.length; j++) {
            if ($(this).val() == arraylist[j]) {
                $(this).prop("selected", status);
            }
        }
    });

}


function updateSelect(targetref, source) {
    var qt = document.getElementById(targetref);
    var elements = "";
    for (i = 0; i < qt.length; i++) {
        elements += qt.options[i].text + "\n";
    }
    //alert(source.id + ":" + elements + " -> " + qt.id);
}

function loadSelect(targetref, source, urlaction, othertargetref) {
    var sourceurl = urlaction;
    var IdProcess = source.options[source.selectedIndex].value;
    //alert(source.id);

    switch (source.id) {
        case "IndustryId":
            sourceurl += "?IndustryId=" + IdProcess; break;
        case "CompetitorId":
            sourceurl += "?CompetitorId=" + IdProcess; break;
        default:
            return;
    }


    //alert("[" + source.options[source.selectedIndex].value + "]");

    var qt = document.getElementById(targetref);
    qt.length = 0;

    if (IdProcess.length == 0) return;


    $.getJSON(sourceurl,
        function(data) {
            $.each(data, function(i, item) {
                qt.options[i] = new Option(item.Text, item.Value);
            });
            if (othertargetref != null) {
                var nextqt = document.getElementById(othertargetref);
                nextqt.length = 0;
                $.each(data, function(i, item) {
                    nextqt.options[i] = new Option(item.Text, item.Value);
                });
            }

        });
}


