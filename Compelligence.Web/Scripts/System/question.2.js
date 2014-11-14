var ShowIndustriesByHierarchy = function(chxbox, url, question) {
    var realvaluesIndustry = [];
    var textvaluesIndustry = [];
    var xmlhttp;
    var results = null;
    $('#QuestionIndustriesIds' + question + ' :selected').each(function(i, selected) {
        realvaluesIndustry[i] = $(selected).val();
        textvaluesIndustry[i] = $(selected).text();
    });
    var parameters = { IsChecked: chxbox.checked, IndustryIds: realvaluesIndustry };
    $.get(url, parameters, function(data) {
        if (data != null && data != "") {
            results = data;
            if (results != "") {
                addNewIndustriesToList(results, question);
            }
        }
    });
};

function addNewIndustriesToList(results, question) {

    var arrayIndustries = [];
    arrayIndustries = results.split("_");
    var options = $('#QuestionIndustriesIds' + question).prop('options');
    $('#QuestionIndustriesIds' + question)[0].options.length = 0;
    for (j = 0; j < arrayIndustries.length; j++) {

        var arrayCompet = arrayIndustries[j].split(":");
        if (arrayCompet[2] == 'True') {
            options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
        }
        else {
            options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
        }
    }

    $("#QuestionIndustriesIds" + question + " option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
    });

};

var updateComboPrimaryCompetitor = function(realvaluesCompetitorSent, realtextsCompetitorSent, idCmbPrimaryComp, question, winning) {
    var realvaluesCompetitor = [];
    var realtextsCompetitor = [];

    var valPrimary;
    var textPrimary;

    $('#QuestionCompetitorsIds' + question + ' :selected').each(function(i, selected) {
        realvaluesCompetitor[i] = $(selected).val();
        realtextsCompetitor[i] = $(selected).text();
    });

    $('#' + idCmbPrimaryComp + question + ' :selected').each(function(i, selected) {
        valPrimary = $(selected).val();
        textPrimary = $(selected).text();
    });
    if (winning != "X") {
        $('#' + 'WinningCompetitor' + winning + ' :selected').each(function(i, selected) {
            valPrimary = $(selected).val();
            textPrimary = $(selected).text();
        });
        $('#' + 'WinningCompetitor' + winning)[0].options.length = 0;
        $('#' + 'WinningCompetitor' + winning).prepend("<option value=''></option>");
    }

    $('#' + idCmbPrimaryComp + question)[0].options.length = 0;
    $('#' + idCmbPrimaryComp + question).prepend("<option value=''></option>");




    if (realvaluesCompetitor != "") {
        for (i = 0; i < realvaluesCompetitor.length; i++) {
            
            if (winning != "X") { var optionsw = $('#' + 'WinningCompetitor' + winning).prop('options'); }
            var options = $('#' + idCmbPrimaryComp + question).prop('options');
            if (valPrimary == realvaluesCompetitor[i]) {
                
                options[options.length] = new Option(realtextsCompetitor[i], realvaluesCompetitor[i], true, true);
                if (winning != "X")
                { optionsw[optionsw.length] = new Option(realtextsCompetitor[i], realvaluesCompetitor[i], true, true); }
            } else {
                options[options.length] = new Option(realtextsCompetitor[i], realvaluesCompetitor[i], true, false);
                if (winning != "X")
                { optionsw[optionsw.length] = new Option(realtextsCompetitor[i], realvaluesCompetitor[i], true, false); }
            }

        }
    }
};

var updateComboPrimaryIndustry = function(realvaluesIndustrySent, realtextsIndustrySent, qq, question) {

    var realvaluesIndustry = [];
    var realtextsIndustry = [];

    var valPrimary;
    var textPrimary;

    $('#QuestionIndustriesIds' + question + ' :selected').each(function(i, selected) {
        realvaluesIndustry[i] = $(selected).val();
        realtextsIndustry[i] = $(selected).text();
    });

    $('#PrimaryIndustry' + question + ' :selected').each(function(i, selected) {
        valPrimary = $(selected).val();
        textPrimary = $(selected).text();
    });

    $('#PrimaryIndustry' + question)[0].options.length = 0;
    $('#PrimaryIndustry' + question).prepend("<option value=''></option>");

    if (realvaluesIndustry != "") {
        for (i = 0; i < realvaluesIndustry.length; i++) {
            var options = $('#PrimaryIndustry' + question).prop('options');
            if (valPrimary == realvaluesIndustry[i])
                options[options.length] = new Option(realtextsIndustry[i], realvaluesIndustry[i], true, true);
            else
                options[options.length] = new Option(realtextsIndustry[i], realvaluesIndustry[i], true, false);

        }
    }

};

var updateProd = function(url, question,winning) {
    var realvaluesIndustry = [];
    var realvaluesProduct = [];
    var realvaluesCompetitor = [];
    var realtextsCompetitor = [];
    $('#QuestionProductsIds' + question + ' :selected').each(function(i, selected) {
        realvaluesProduct[i] = $(selected).val();
    });
    $('#QuestionCompetitorsIds' + question + ' :selected').each(function(i, selected) {
        realvaluesCompetitor[i] = $(selected).val();
        realtextsCompetitor[i] = $(selected).text();
    });
    $('#QuestionIndustriesIds' + question + ' :selected').each(function(i, selected) {
        realvaluesIndustry[i] = $(selected).val();
    });
    $('#QuestionProductsIds' + question)[0].options.length = 0;
    if (realvaluesCompetitor != "") {
        for (i = 0; i < realvaluesCompetitor.length; i++) {
            setValuesForProductsOfCompetitor(realvaluesCompetitor[i], realvaluesProduct, realvaluesIndustry, url, question);
        }
    }
    updateComboPrimaryCompetitor(realvaluesCompetitor, realtextsCompetitor, 'PrimaryCompetitor', question, winning);
};

var updateCompAndProd = function(urlCofI, urlPofC, question) {
    var realvaluesIndustry = [];
    var textvaluesIndustry = [];
    var realvaluesCompetitor = [];
    var realtextsCompetitor = [];
    var realvaluesProduct = [];

    $('#QuestionIndustriesIds' + question + ' :selected').each(function(i, selected) {
        realvaluesIndustry[i] = $(selected).val();
        textvaluesIndustry[i] = $(selected).text();
    });
    if (realvaluesIndustry == "") {
        $('#QuestionCompetitorsIds' + question)[0].options.length = 0;
        $('#QuestionProductsIds' + question)[0].options.length = 0;
    } else {
        $('#QuestionCompetitorsIds' + question + ' :selected').each(function(i, selected) {
            realvaluesCompetitor[i] = $(selected).val();
            realtextsCompetitor[i] = $(selected).text();
        });
        $('#QuestionProductsIds' + question + ' :selected').each(function(i, selected) {
            realvaluesProduct[i] = $(selected).val();
        });
        $('#QuestionCompetitorsIds' + question)[0].options.length = 0;
        $('#QuestionProductsIds' + question)[0].options.length = 0;

        for (i = 0; i < realvaluesIndustry.length; i++) {

            setValuesForCompetitors(realvaluesIndustry[i], realvaluesCompetitor, realvaluesProduct, urlCofI, urlPofC, question);
        }

    }
    updateComboPrimaryIndustry(realvaluesIndustry, textvaluesIndustry, 'PrimaryIndustry', question);
};


function setValuesForCompetitors(id, realvaluesCompetitor, realvaluesProduct, urlCofI, urlPofC, question) {
    var parameters = { Id: id };
    var url = urlCofI + '/' + id;
    $.post(
            url,
            null,
            function(data) {
                if (data != "") {
                    addCompetitorsToList(data, realvaluesCompetitor, realvaluesProduct, urlPofC, question);
                }
            }, "text");
    
//    xmlhttp = $.post(url, null, function(data) {
//        if (data != "") {
//            addCompetitorsToList(data, realvaluesCompetitor, realvaluesProduct, urlPofC, question);
//            }
//    },"text");

}

function addCompetitorsToList(results, realvaluesCompetitor, realvaluesProduct, url, question) {
    if (results != null) {
        var realvaluesIndustry = [];
        var textvaluesIndustry = [];
        $('#QuestionIndustriesIds' + question + ' :selected').each(function(i, selected) {
            realvaluesIndustry[i] = $(selected).val();
            textvaluesIndustry[i] = $(selected).text();
        });

        var arrayComppetitors = [];
        arrayComppetitors = results.split("_");
        for (j = 0; j < arrayComppetitors.length; j++) {
            var options = $('#QuestionCompetitorsIds' + question).prop('options');
            var arrayCompet = arrayComppetitors[j].split(":");
            var actual = $("select#QuestionCompetitorsIds" + question).children().map(function() { return $(this).val(); }).get();
            if ($.inArray(arrayCompet[0], actual) == -1) {
                if (realvaluesCompetitor == "") {
                    options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
                } else {
                    if ($.inArray(arrayCompet[0], realvaluesCompetitor) == -1) {
                        options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, false);
                    } else {
                        options[options.length] = new Option(arrayCompet[1], arrayCompet[0], true, true);
                        setValuesForProductsOfCompetitor(arrayCompet[0], realvaluesProduct, realvaluesIndustry, url, question);
                    }
                }
            }
        }

        var count2 = ($('#QuestionCompetitorsIds' + question)[0].options.length) * 16 + 10;
        var count3 = ($('#QuestionProductsIds' + question)[0].options.length) * 16 + 10;

        $('#ContentCompetitor' + question + ' select').css('height', count2 + "px");
        $('#ContentProduct' + question + ' select').css('height', count3 + "px");

        $("#QuestionCompetitorsIds" + question + " option").each(function() {
            $(this).prop("title", $(this).text());
            $(this).prop("style", "height:16px;");
        });
        $("#QuestionProductsIds" + question + " option").each(function() {
            $(this).prop("title", $(this).text());
            $(this).prop("style", "height:16px;");
        });
    }

}

function setValuesForProductsOfCompetitor(id, realvaluesProduct, realvaluesIndustry, url, question) {

    var parameters = { Id: id };

    var idDeal = $('#Id').val();
    var idsIndustries = realvaluesIndustry;
    url = url + '/' + id + '?idDeal=' + idDeal + '&idsIndustries=' + idsIndustries;
    $.post(url, null, function(data) {
    if (data != "") {
        addProductsToList(data, realvaluesProduct, question);
            }
    },"text");

}


function addProductsToList(results, realvaluesProduct, question) {

    if (results != null) {
        var arrayProducts = [];
        
        arrayProducts = results.split("_");
        for (j = 0; j < arrayProducts.length; j++) {
            var options = $('#QuestionProductsIds' + question).prop('options');
            var arrayProd = arrayProducts[j].split(":");
            var actual = $("select#QuestionProductsIds" + question).children().map(function() { return $(this).val(); }).get();
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


        var count3 = ($('#QuestionProductsIds' + question)[0].options.length) * 16 + 10;

        $('#ContentProduct' + question + ' select').css('height', count3 + "px");

        $("#QuestionProductsIds" + question + " option").each(function() {
            $(this).prop("title", $(this).text());
            $(this).prop("style", "height:16px;");
        });
    }
}

function resetMultiSelect() {
    $('#QuestionCompetitorsIds')[0].options.length = 0;
    $('#QuestionProductsIds')[0].options.length = 0;

    var realvaluesIndustry = [];
    var textvaluesIndustry = [];
    var realvaluesCompetitor = [];
    var realvaluesProduct = [];

    realvaluesCompetitor = $('#OldCompetitorsIds').val().split(",");
    realvaluesProduct = $('#OldProductsIds').val().split(",");

    $('#QuestionIndustriesIds :selected').each(function(i, selected) {
        realvaluesIndustry[i] = $(selected).val();
        textvaluesIndustry[i] = $(selected).text();
    });


    if (realvaluesIndustry != "") {
        for (i = 0; i < realvaluesIndustry.length; i++) {

            setValuesForCompetitors(realvaluesIndustry[i], realvaluesCompetitor, realvaluesProduct);
        }
    }

}

function updateAnswer(qItem) {
    var Industry = "";
    var Competitor = "";
    var Product = "";
    var patron = /[\255]*·\s*/g;
    $('#QuestionIndustriesIds' + qItem + ' :selected').each(function(i, selected) {
        if (i == 0) {
            Industry = Industry + $(selected).text();
        } else {
            Industry = Industry + ", " + $(selected).text();
        }
        Industry = Industry.replace(patron, "");
    });
    Industry = Industry + ";";

    $('#QuestionCompetitorsIds' + qItem + ' :selected').each(function(i, selected) {
        if (i == 0) {
            Competitor = Competitor + $(selected).text();
        } else {
            Competitor = Competitor + ", " + $(selected).text();
        }
    });
    Competitor = Competitor + ";";

    $('#QuestionProductsIds' + qItem + ' :selected').each(function(i, selected) {
        if (i == 0) {
            Product = Product + $(selected).text();
        } else {
            Product = Product + ", " + $(selected).text();
        }
    });
    Product = Product + ";";

    pIndustry = $('#PrimaryIndustry' + qItem + ' :selected').text() + ';';
    pIndustry = pIndustry.replace(patron, "");
    pCompetitor = $('#PrimaryCompetitor' + qItem + ' :selected').text() + ';';

    var answer = Industry + Competitor + Product + pIndustry + pCompetitor;
    $('#Q' + qItem).val(answer);

};

function LoadQuestion(question, size) {
    var count1 = size * 16 + 10;
    var count2 = ($('#QuestionCompetitorsIds' + question)[0].options.length) * 16 + 10;
    var count3 = ($('#QuestionProductsIds' + question)[0].options.length) * 16 + 10;

    $('#ContentIndustry' + question + ' select').css('height', count1 + "px");
    $('#ContentCompetitor' + question + ' select').css('height', count2 + "px");
    $('#ContentProduct' + question + ' select').css('height', count3 + "px");

    $("#QuestionIndustriesIds" + question + " option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
    });
    $("#QuestionCompetitorsIds" + question + " option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
    });
    $("#QuestionProductsIds" + question + " option").each(function() {
        $(this).prop("title", $(this).text());
        $(this).prop("style", "height:16px;");
    });
};
function EditWinning(question) {
    $('#WinningCompetitor' + question).show();
};
function updateWinning(question) {
    $('#Q' + question).val($('#WinningCompetitor' + question + ' :selected').text());
    $('#WinningCompetitor' + question).hide();
};

function updateList(question) {
    list = '#WinningCompetitor' + question;
    var filter = eval("/" + $('#Q' + question).val() + "/i");
    $('#WinningCompetitor' + question + ' option').each(function(index, value) {
        if ($(value).text().search(filter) < 0) {
            $(value).hide();
        } else {
            $(value).show();
        }
    });

};