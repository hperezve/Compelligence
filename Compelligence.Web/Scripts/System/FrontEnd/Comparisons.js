
function removeComparisonList(userid) {
    var userprofilecomparisonid = $("#UserProfileComparisonId").val();
    var userprofilecomparisonname = $("#UserProfileComparisonId option:selected").text();


    if (userprofilecomparisonid == "")
        return;


    $("#ConfirmBox").dialog({
        title: "Confirm delete?",
        autoOpen: false,
        modal: true,
        buttons: {
            'Ok': function() {
                var urlAction = $(this).dialog('option', 'urlAction');
                var itemid = $(this).dialog('option', 'itemid');

                $.get(urlAction, { id: itemid, U: userid }, function(data) {
                    if (data != null) {
                        var items = "";
                        $.each(data, function(i, item) {
                            items += "<option value='" + item.value + "' >" + item.text + "</option>";
                        });
                        $("#UserProfileComparisonId").html(items);
                        //$("#UserProfileComparisonId option:selected").prop("selected", false);

                        $("#UserProfileComparisonId").multiselect("refresh");
                        $("#UserProfileComparisonId").multiselect("uncheckAll");


                        $("#CompetitorId").empty();
                        $("#CompetitorId").multiselect("refresh");

                        $("#ProductId").empty();
                        $("#ProductId").multiselect("refresh");

                        $("#IndustryId").val(0);
                        $("#IndustryId").multiselect("refresh");
                        updIndustryDropDown();

                        $("#FormProducts").html("");
                        $("#FormResults").html("");

                        $("#ConfirmBox").dialog('close'); // is different context

                    }

                });


            },
            Cancel: function() {
                $(this).dialog('close');
            }
        },
        userprofilecomparisonid: '',
        urlAction: ''
    }).html("You are about to remove the comparison named <b id='itemname'></b>. Please click \"Ok\" to continue.");

    var ConfirmDialog = $("#ConfirmBox").dialog('option', 'urlAction', urlc_remove).dialog('option', 'itemid', userprofilecomparisonid);
    ConfirmDialog.find("#itemname").text(userprofilecomparisonname);
    ConfirmDialog.dialog('open');

}


function loadComparisonList(pU,pC) {
    var userprofilecomparisonid = $("#UserProfileComparisonId").val();
    if (userprofilecomparisonid == "")
        return;
    $.get(urlc_load, { id: userprofilecomparisonid, U: pU, C: pC }, function(data) {
        //FrontEnd return null and Salesforce return empty
        if (data != null && data.length > 0) {
            //console.log(data.industryid );
            //console.log(data.competitorid);
            //console.log(data.productids);
            $("#FormProducts").html(data);
            $("#FormResults").empty();

            $.getJSON(urlc_info, { id: userprofilecomparisonid, U: pU, C: pC }, function(data) {
                if (data != null) {
                    $("#IndustryId").val(data.industryid);
                    $("#IndustryId").multiselect("refresh");

                    var items = "";
                    $.each(data.competitors, function(i, item) {
                        items += "<option value='" + item.value + "' >" + item.text + "</option>";
                    });
                    $("#CompetitorId").html(items);
                    $("#CompetitorId").val(data.competitorid);
                    $("#CompetitorId").multiselect("refresh");


                    var items = "";
                    $.each(data.products, function(i, item) {
                        items += "<option value='" + item.value + "' " + item.selected + " " + item.disabled + ">" + item.text + "</option>";
                    });
                    $("#ProductId").html(items);
                    $("#ProductId").multiselect("refresh");

                    updIndustryDropDown();
                    updCompetitorDropDown();

                    $("#btnCompare").click();
                } else {
                    errorLoadComparison();//to system
                }
            });
            resizeHeightOfTdImage();
            resizeHeightOfTdRecommendImage();
            saved_comparison(1);  
        } else {
            errorLoadComparison();//to salesforce
        }
    });
}

function saveComparisonList(pU) {

    var industryids = getOptionValues('#IndustryId', true); //get all ids
    var competitorids = getOptionValues('#CompetitorId', true); //get all ids
    var productids = getOptionValues('#ProductId', true); //get all ids


    var commentObject = $("#ComparisonBox");

    commentObject.dialog({ autoOpen: false,
        title: "Save Products for comparison",
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var name = commentObject.children("#name").val();
            //start validation
            //length validation
            if (name == "") {
                commentObject.children("#alert").html("<b style='color:red'>Please enter a valid name!</b><br><br>");
                commentObject.children("#name").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }

            //name exist validation

            var lstcomparison = getOptionTexts("#UserProfileComparisonId");
            var comparisonexist = false;
            for (i = 0; i < lstcomparison.length; i++) {
                if (name != "" && name == lstcomparison[i]) {
                    comparisonexist = true;
                    break;
                }
            }
            if (comparisonexist) {
                commentObject.children("#alert").html("<b style='color:red'>Comparison name already exist!</b><br><br>");
                commentObject.children("#name").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }
            //ebd-validation

            $.get(urlc_save, { iids: industryids + "", cids: competitorids + "", pids: productids + "", name: name, U: pU }, function(data) {
                if (data != "") {
                    //
                    var items = "";
                    $.each(data, function(i, item) {
                        items += "<option value='" + item.value + "' >" + item.text + "</option>";
                    });

                    $("#UserProfileComparisonId").html(items);


                    $('#UserProfileComparisonId option:contains(' + name + ')').each(function() {
                        if ($(this).text() == name) {
                            $(this).prop('selected', 'selected');
                            return false;
                        }
                        return true;

                    });
                    $("#UserProfileComparisonId").multiselect("refresh");
                    //
                }
                commentObject.dialog("destroy");
            });

        }
        }
    });

    var contentlines = "<label id='alert'></label>";
       contentlines += "Comparison Name: <input id='name' type='text' value='' style='width:324px'>";
    commentObject.html(contentlines);
    commentObject.dialog("open");

    commentObject.children("#name").focus();

}

function errorLoadComparison() {
    var commentObject = $("#ComparisonBox");
    commentObject.dialog({
        autoOpen: false,
        title: "Error Loading Save Comparison",
        width: 300,
        modal: true,
        buttons: {
            "Ok": function() {
                $(this).dialog("destroy");
            }
        }
    });
    var contentlines = "<label id='alert'>The comparison could not be loaded</label>";
    commentObject.html(contentlines);
    commentObject.dialog("open");
};

function renameComparisonList(pU) {

    var userprofilecomparisonid = $("#UserProfileComparisonId").val();
    var userprofilecomparisonname = $("#UserProfileComparisonId option:selected").text();
    if (userprofilecomparisonid == "")
        return;

    var commentObject = $("#ComparisonBox");

    commentObject.dialog({ autoOpen: false,
        title: "Rename Saved Comparison item",
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var name = commentObject.children("#name").val();
            //start validation
            //length validation
            if (name == "") {
                commentObject.children("#alert").html("<b style='color:red'>Please enter a valid name!</b><br><br>");
                commentObject.children("#name").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }

            //name exist validation

            var lstcomparison = getOptionTexts("#UserProfileComparisonId");
            var comparisonexist = false;
            for (i = 0; i < lstcomparison.length; i++) {
                if (name != "" && name == lstcomparison[i]) {
                    comparisonexist = true;
                    break;
                }
            }
            if (comparisonexist) {
                commentObject.children("#alert").html("<b style='color:red'>Comparison name already exist!</b><br><br>");
                commentObject.children("#name").focus();
                setTimeout(function() { commentObject.children("#alert").html(""); }, 2000);
                return false;
            }
            //ebd-validation

            $.get(urlc_rename, { id: userprofilecomparisonid, name: name, U: pU }, function(data) {
                if (data != "") {
                    //
                    var items = "";
                    $.each(data, function(i, item) {
                        items += "<option value='" + item.value + "' >" + item.text + "</option>";
                    });

                    $("#UserProfileComparisonId").html(items);


                    $('#UserProfileComparisonId option:contains(' + name + ')').each(function() {
                        if ($(this).text() == name) {
                            $(this).attr('selected', 'selected');
                            return false;
                        }
                        return true;

                    });
                    $("#UserProfileComparisonId").multiselect("refresh");
                    //
                }
                commentObject.dialog("destroy");
            });

        }
        }
    });
    var contentlines = "<label id='alert'></label>";
    contentlines += "New Comparison Name: <input id='name' type='text' value='" + userprofilecomparisonname + "' style='width:324px'>";
    commentObject.html(contentlines);
    commentObject.dialog("open");

    commentObject.children("#name").focus();

};
var hiddenIcons = function(chkBox) {
    if (chkBox) {
        $('.silverComment').show();
        $('.silverFeedback').show();
    }
    else {
        $('.silverComment').hide();
        $('.silverFeedback').hide();
    }
};
