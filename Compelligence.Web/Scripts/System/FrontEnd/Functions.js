var htmlEncode = function(value) {
    return $('<div/>').text(value).html();
};
var htmlDecode = function(value) {
    return $('<div/>').html(value).text();
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

function showTableAnnual() {
    $('.tbl_comp_fin_annual').show();
    $('.tbl_comp_fin_quartely').hide();
    $('#chkQuarterly').prop("checked", false);
};
function showTableQuartely() {
    $('.tbl_comp_fin_annual').hide();
    $('.tbl_comp_fin_quartely').show();
    $('#chkAnnual').prop("checked", false);
};

function showAnnual(chkd) {
    if (chkd.checked) {
        showTableAnnual();
    } else {
        $('#chkQuarterly').prop("checked", true);
        showTableQuartely();
    }
};
function showQuartely(chkd) {
    if (chkd.checked) {
        showTableQuartely();
    } else {
        $('#chkAnnual').prop("checked", true);
        showTableAnnual();
    }
};