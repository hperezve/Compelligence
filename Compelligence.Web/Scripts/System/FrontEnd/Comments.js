

//
// Utility functions
//

function EasyToggleClass(target, fromClass, toClass) {
    target.removeClass(fromClass);
    target.addClass(toClass);
}


// Messages Functions
//

function MessageBox(dialogTitle, dialogContent) {
    var commentObject = $("#MessageBox");

    commentObject.dialog({ autoOpen: false,
        title: dialogTitle,
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            $(this).dialog("destroy");
        }
        }
    });
    commentObject.html(dialogContent);
    commentObject.dialog("open");
}

function FormBox(object, dialogTitle, dialogContent, urlAction, target, refval1) {
    var commentObject = $("#MessageBox");
    commentObject.empty();
    commentObject.dialog({ autoOpen: false,
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var urls = $("#ComparinatorFormEdit")[0].txtLinks.value;
            if (urls.length > 0 && !isUrls(urls)) {
                alert('URL is not valid');
            }
            else {
                $.post(urlAction,
                  $("#ComparinatorFormEdit").serialize(),
                  function(data) {
                      //return value + "@" + notes + "@" + links + "@" + feature
                      //update values
                      var data_array = data.split("@");
                      $('#val' + refval1).children("div").children("div").children("div").children("span").text(data_array[0]);
                      $('#Note' + refval1).html(data_array[1]);
                      $('#Links' + refval1).html(ToLinks(data_array[2].toString()));

                      //Assign featuec lass for filters
                      var featureClass = data_array[3].toString();
                      $('#val' + refval1).prop("class", featureClass);
                      $('#Feature' + refval1).text(featureClass);

                      var tempo = data_array[0];
                      GetParentOfObject(object, tempo);
                      //try change next line with parameter,with if have parameter then callback function
                      if ($('#chkcolorizeon').prop('checked')) //only colorize if checked
                          colorizeFeature(1); //force update color
                  });
                $(this).dialog("destroy");

            }


        } //end-ok
        , "Close": function() { $(this).dialog("destroy"); }
        }
    });
    commentObject.keypress(
        function(e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                var parentMessageBox = $(this).parent();
                var divButtonPane = parentMessageBox.find('.ui-dialog-buttonpane');
                var divButtonSet = divButtonPane.find('.ui-dialog-buttonset');
                var divButton = divButtonSet.find('.ui-button');
                var spanButton = divButton.find('.ui-button-text');
                spanButton.click();
                return false;
            }
        }
    );
    commentObject.dialog('option', 'title', dialogTitle);
    commentObject.html(dialogContent);
    commentObject.dialog("open");

}
function GetParentOfObjectColor(objectid, value, featureClass, type, dataArray) {
    var object = $('#' + objectid);
    var dataArrayValue = dataArray.split("KEYS");
    for (var j = 0; j < dataArrayValue.length; j++) {
        var ValoresRow = dataArrayValue[j].split(";");
        var valordata = ValoresRow[1];
        for (var i = 0; i < object[0].children.length; i++) {
            var tds = object[0].children[i];
            if (tds.id != '') {
                var oids = tds.id;
                var oids = oids.replace("''", "");
                var valor = $('#' + oids).children("div").children("div").children("div").children("span").text();
                if (valor == valordata) {
                    $('#' + oids).prop("class", ValoresRow[0]);
                    $('#Feature' + oids).text(ValoresRow[0]);
                }
            }
        }
    }
}
function GetParentOfObject(objectid, value) {
    var object = $('#' + objectid);
    var areEqual = true;

    $('#' + objectid + ' td').each(function() {
        if ($(this).prop('id') != '') {
            if ($(this).children("div").children("div").children("div").children("span").text() != value) {
                areEqual = false;
            }
        }
    })

    if (areEqual) {
        $('#' + objectid).removeClass().addClass("comp_eq");

    }
    else {
        $('#' + objectid).removeClass().addClass("comp_neq");

    }


}

function trim(stringToTrim) {
    return stringToTrim.replace(/^\s+|\s+$/g, "");
}

function LinkDialog(urlAction) {
    MessageBox("Add Link", "<form onsubmit='return false;'><input type='text' name='txtlink' style='width:320px' /> </form>");
}

function NoteDialog(urlAction) {
    MessageBox("Add Note", "<form onsubmit='return false;'><textarea name='txtnote' WRAP=SOFT COLS=50 ROWS=4/></textarea> </form>");
}
function AddDialog(object, Title, urlAction, val0, val1, val2, val3, refval1) {
    var val4 = jQuery.trim($('#val' + refval1).children("div").children("div").children("div").children("span").text());
    var val5 = $('#Note' + refval1).text();
    var val6 = $('#Links' + refval1).text();
    var feature = $('#Feature' + refval1).text();
    var strForm = "<form id='ComparinatorFormEdit'>";
    strForm += "<input type='hidden' name='IndustryId' value='" + val0 + "' />";
    strForm += "<input type='hidden' name='txtType' value='" + val1 + "' />";
    strForm += "<input type='hidden' name='txtCriteriaId' value='" + val2 + "' />";
    strForm += "<input type='hidden' name='txtEntityId' value='" + val3 + "' />";

    strForm += "<label>Value</label><input type='text' id='txtValue' name='txtValue' onfocus='LoadAutoComplete(\"" + val0 + "\" , \"" + val2 + "\")' value='" + val4 + "'  /><br />";

    strForm += "<label>Notes:</label><textarea name='txtNotes' WRAP=SOFT COLS=50 ROWS=4 class='textareadialog' onClick='setClassOnFocus(this)'>" + jQuery.trim(val5) + "</textarea><br />";
    strForm += "<label>Links:(include http://)</label><textarea name='txtLinks' WRAP=SOFT COLS=50 ROWS=4 class='textareadialog' onClick='setClassOnFocus(this)'>" + jQuery.trim(val6) + "</textarea><br />";
    //it's dangerous, because need get from database
    strForm += "<label>Feature</label><select name='Feature'><option value=''></option>";
    strForm += "<option value='BC' " + (feature == "BC" ? "selected" : "") + ">Best in Class</option>";
    strForm += "<option value='MA' " + (feature == "MA" ? "selected" : "") + ">Market Advantage</option>";
    strForm += "<option value='MP' " + (feature == "MP" ? "selected" : "") + ">Market Parity</option>";
    strForm += "<option value='MD' " + (feature == "MD" ? "selected" : "") + ">Market Disadvantage</option>";
    strForm += "<option value='LM' " + (feature == "LM" ? "selected" : "") + ">Lagging Market</option>";
    strForm += "</select>";
    //alert("|"+feature+"|");
    strForm += "</form>";

    FormBox(object, Title, strForm, urlAction, "MessageStatus", refval1);
}



function updateRowStatus(rowid, value) 
{
        var row = $(rowid);
        var areEqual = true;

        row.children("td").each(function() 
        {
            var tdid = $(this).prop('id');
            if (tdid != '' && tdid.substr(0, 1) == "P") //If id start with P is Product Cell
            {
                if ($(this).children("div").children("span").text() != value) {
                    areEqual = false;
                }
            }
        });

        if (areEqual) 
            row.removeClass().addClass("comp_eq");
        else 
            row.removeClass().addClass("comp_neq");
}

function CellBenefitDlg(criteriaid, title, urlAction) 
{

    var commentObject = $("#MessageBox");
    commentObject.empty();
    commentObject.dialog({ autoOpen: false,
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var benefit = $("#ComparinatorFormEdit").find("#txtValue").val();
            benefit = benefit.trim();
            if (benefit.length > 200) {
                benefit = benefit.substring(0, 200); //get the first 200 characters
                var messageToValuesTruncated = "The Benefit entered is longer than 200 characters and has been truncated.";
                var messageDialog = $('#AlertMessageDialog');
                messageDialog.dialog({ title: "Alert Message", buttons: { 'Ok': function() { $(this).dialog('close'); } } });
                messageDialog.html(messageToValuesTruncated);
                messageDialog.dialog("open");
            }
            $.post(urlAction,
                  $("#ComparinatorFormEdit").serialize(),
                  function(data) {
                      var data_array = data.split("@");
                      $('#B' + criteriaid).children("div").children("span").text(data_array[0]);
                      //Next line was comment because only in necesary for products values
                      //updateRowStatus("#C"+criteriaid, data_array[0]);
                      if ($('#chkcolorizeon').prop('checked')) //only colorize if checked
                          colorizeFeature(1); //force update color
                  });
            $(this).dialog("destroy");
        } //end-ok
        , "Close": function() { $(this).dialog("destroy"); }
        }
    });
    commentObject.keypress(
        function(e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                var parentMessageBox = $(this).parent();
                var divButtonPane = parentMessageBox.find('.ui-dialog-buttonpane');
                var divButtonSet = divButtonPane.find('.ui-dialog-buttonset');
                var divButton = divButtonSet.find('.ui-button');
                var spanButton = divButton.find('.ui-button-text');
                spanButton.click();
                return false;
            }
        }
    );

    var benefit = jQuery.trim($('#B' + criteriaid).children("div:first").children("span").text());
    var strForm = "<form id='ComparinatorFormEdit'>";
    strForm += "<input type='hidden' name='txtCriteriaId' value='" + criteriaid + "' />";
    strForm += "<label>Benefit:  </label><input type='text' id='txtValue' name='txtValue' onfocus='LoadAutoBenefit(\"" + criteriaid + "\")' value='" + benefit + "'  /><br />";
    strForm += "</form>";
    
    commentObject.dialog('option', 'title', title);
    commentObject.html(strForm);
    commentObject.dialog("open");
}



function CellCostDlg(criteriaid, title, urlAction) {

    var commentObject = $("#MessageBox");
    commentObject.empty();
    commentObject.dialog({ autoOpen: false,
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var cost = $("#ComparinatorFormEdit").find("#txtValueCost").val();
            cost = cost.trim();
            if (cost.length > 200) {
                cost = cost.substring(0, 200); //get the first 200 characters
                var messageToValuesTruncated = "The Cost entered is longer than 200 characters and has been truncated.";
                var messageDialog = $('#AlertMessageDialog');
                messageDialog.dialog({ title: "Alert Message", buttons: { 'Ok': function() { $(this).dialog('close'); } } });
                messageDialog.html(messageToValuesTruncated);
                messageDialog.dialog("open");
            }
            $.post(urlAction,
                  $("#ComparinatorFormEdit").serialize(),
                  function(data) {
                      var data_array = data.split("@");
                      $('#O' + criteriaid).children("div").children("span").text(data_array[0]);
                      //Next line was comment because only in necesary for products values
                      //updateRowStatus("#C" + criteriaid, data_array[0]);
                      if ($('#chkcolorizeon').prop('checked')) //only colorize if checked
                          colorizeFeature(1); //force update color
                  });
            $(this).dialog("destroy");
        } //end-ok
        , "Close": function() { $(this).dialog("destroy"); }
        }
    });
    commentObject.keypress(
        function(e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                var parentMessageBox = $(this).parent();
                var divButtonPane = parentMessageBox.find('.ui-dialog-buttonpane');
                var divButtonSet = divButtonPane.find('.ui-dialog-buttonset');
                var divButton = divButtonSet.find('.ui-button');
                var spanButton = divButton.find('.ui-button-text');
                spanButton.click();
                return false;
            }
        }
    );

    var cost = jQuery.trim($('#O' + criteriaid).children("div:first").children("span").text());

    var strForm = "<form id='ComparinatorFormEdit'>";
    strForm += "<input type='hidden' name='txtCriteriaId' value='" + criteriaid + "' />";
    strForm += "<label>Cost:  </label><input type='text' id='txtValueCost' name='txtValue' onfocus='LoadAutoCost(\"" + criteriaid + "\")' value='" + cost + "'  /><br />";
    strForm += "</form>";
    
    commentObject.dialog('option', 'title', title);
    commentObject.html(strForm);
    commentObject.dialog("open");
}

function CellIndustryStandardDlg(criteriaid, industryStandard, title,urlAction) {

    var commentObject = $("#MessageBox");
    commentObject.empty();
    commentObject.dialog({ autoOpen: false,
        width: 380,
        modal: true,
        buttons: { "Ok": function() {
            $.post(urlAction,
                  $("#ComparinatorFormEdit").serialize(),
                  function(data) {
                      var data_array = data.split("@");
                      $('#Ind' + criteriaid).children("div").children("span").text(data_array[0]);
                      //Next line was comment because only in necesary for products values
                      //updateRowStatus("#C" + criteriaid, data_array[0]);
                      if ($('#chkcolorizeon').prop('checked')) //only colorize if checked
                          colorizeFeature(1); //force update color
                  });
            $(this).dialog("destroy");
        } //end-ok
        , "Close": function() { $(this).dialog("destroy"); }
        }
    });
    commentObject.keypress(
        function(e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                var parentMessageBox = $(this).parent();
                var divButtonPane = parentMessageBox.find('.ui-dialog-buttonpane');
                var divButtonSet = divButtonPane.find('.ui-dialog-buttonset');
                var divButton = divButtonSet.find('.ui-button');
                var spanButton = divButton.find('.ui-button-text');
                spanButton.click();
                return false;
            }
        }
    );

    var cost = jQuery.trim($('#O' + criteriaid).children("div:first").children("span").text());

    var strForm = "<form id='ComparinatorFormEdit'>";
    strForm += "<input type='hidden' name='txtCriteriaId' value='" + criteriaid + "' />";
    strForm += "<label>Industry Standard :  </label><input type='text' id='txtValueIndustryStandard ' name='txtValue'  value='" + industryStandard + "'  /><br />";
    strForm += "</form>";

    commentObject.dialog('option', 'title', title);
    commentObject.html(strForm);
    commentObject.dialog("open");
}



function vKey(target) {
    $(target).keypress(function(event) { return event.keyCode != 13; });
}
// Works at FrontEnd Project, Comparinator
function CommentFormDlg(url, dlgTitle, urlNew, EntityId) {
    showLoadingDialog();
    var quizObject = $("#ExternalResponse");
    var ImageclassId = EntityId;
    quizObject.empty();
    quizObject.dialog(

          {
              bgiframe: true,
              autoOpen: false,
              title: dlgTitle,
              height: 400,
              width: 800,
              buttons:
             {
                 "Close": function() { $(this).dialog("destroy"); },
                 "New": function() {
                     $(this).dialog("destroy");
                     //                     if (ImageclassId != null) {
                     //                         AddComentsclassImage(ImageclassId);
                     //                     }

                     ExternalResponseNewDlg(urlNew, 0);



                 }
             }
          }
          );

    quizObject.load(url, {}, function() {
        $("#FormSubmit").css('visibility', 'hidden');
        $(this).dialog("open");
        hideLoadingDialog();
    });



    return false;
}

//
//New Comparinator Dialog
//
// Works at FrontEnd Project, Comparinator
function ExternalCommentsDlg(url, urlNew) {
    showLoadingDialog();
    var quizObject = $("#ExternalResponse");
    quizObject.empty();
    quizObject.dialog(
      {
       autoOpen: false,
       height: 400,
       width: 800,
       buttons:
       {
         "Close": function() {
             $(this).dialog("destroy");
         },
         "New": function() {
             $(this).dialog("destroy");
             ExternalResponseNewDlg(urlNew);
         }
         }
      }
      );

    //load and fill list of comments
    quizObject.load(url, {}, function() {
        $(this).dialog("open");
        hideLoadingDialog();
    });

    return false;
}


function ExternalResponseNewVDlg(urlAction, urlValidate) {
    showLoadingDialog();
    var ernDlg = $("#ExternalResponseNew");
    ernDlg.empty();
    ernDlg.dialog(
          {
              autoOpen: false,
              title: 'Comment Dialog',
              width: 680,
              close: function(event, ui) { $(this).dialog("destroy"); hideLoadingDialog(); },
              buttons:
             {
                 "Close": function() {
                   $(this).dialog("destroy"); hideLoadingDialog();
                   //it's for update corner, only in comparinator
                   if (!(updateCorner == undefined)) {
                       updateCorner();
                   }                     
                 },
                 "Send": function() {
                     $("#frameCommentResponse").contents().find(".error").html('');
                     var success = true;
                     var message = '';
                     var comment = $("#frameCommentResponse").contents().find("#commentExternalId").val();
                     if (comment == null || comment == undefined || comment == '') {
                         message = '<li>Comment is required</li>';
                         success = false;
                         $("#frameCommentResponse").contents().find("#commentExternalId").addClass('validation-error-items');
                         $("#frameCommentResponse").contents().find(".required").addClass('validation-error-text');
                     } else {
                         $("#frameCommentResponse").contents().find("#commentExternalId").removeClass('validation-error-items');
                         $("#frameCommentResponse").contents().find(".required").removeClass('validation-error-text');
                     }
                     var inputFiles = $("#frameCommentResponse").contents().find(".fileDialogUpload");
                     if (inputFiles != null && inputFiles != undefined && inputFiles.length > 0) {
                         showLoadingDialog();
                         for (var i = 0; i < inputFiles.length; i++) {
                             var v = inputFiles[i].value;
                             var file = inputFiles[i];
                             var fileId = file.id;
                             if (v != null && v != undefined && v != '') {
                                 if (file.files != null || file.files != undefined) {
                                     if (file.files.length != null && file.files.length != undefined && file.files.length > 0) {
                                         file = file.files[0];
                                     }
                                 }
                                 var response = '';
                                 if (file) {
                                     var fd = new FormData();
                                     fd.append('file', file);
                                     var xhr = new XMLHttpRequest();

                                     xhr.onreadystatechange = function() {
                                         if (xhr.readyState == 4 && xhr.status == 200) {
                                             response = xhr.responseText;
                                             fileId = fileId.replace('uploadfile', '');
                                             if (response == 'Sucessfull') {
                                                 $("#frameCommentResponse").contents().find("#txtuploadfile" + fileId).removeClass('validation-error-items');
                                             }
                                             else if (response == 'UnSucessfull') {
                                                 success = false;
                                                 message = message + '<li>The file ' + v + ' was not successfully uploaded. Please re-submit</li> ';
                                                 $("#frameCommentResponse").contents().find("#txtuploadfile" + fileId).addClass('validation-error-items');
                                             } else {
                                                $("#frameCommentResponse").contents().find("#txtuploadfile" + fileId).removeClass('validation-error-items');
                                             }
                                         }
                                     };
                                     xhr.open('POST', urlValidate, false);
                                     xhr.send(fd);
                                 }
                             } else {
                                 // file is empty
                                 message = message + '<li>The file is empty</li> ';
                                 success = false;
                                 fileId = fileId.replace('uploadfile', '');
                                 $("#frameCommentResponse").contents().find("#txtuploadfile" + fileId).addClass('validation-error-items');
                             }
                         }
                     }
                     if (success) {
                         $("#frameCommentResponse").contents().find("#CommentsResponseForm").submit();
                         $(".ui-dialog-buttonpane button:contains('Send')").button().hide();
                         $(".ui-dialog-buttonpane button:contains('Reset')").button().hide();
                         showLoadingDialog();
                     } else {
                     $("#frameCommentResponse").contents().find(".error").html('<ul class="validation-summary-errors">' + message + '</ul>');
                     hideLoadingDialog();
                     }
                 },
                 "Reset": function() {
                     $("#frameCommentResponse").contents().find("#CommentsResponseForm")[0].reset();
                 }

             }
          }
          );
    //get url into iframe
          ernDlg.html("<iframe id='frameCommentResponse' src='" + urlAction + "' style='overflow-x:hidden' width=656 height=290 frameBorder=0></iframe>");
    ernDlg.dialog("open");
    hideLoadingDialog();

    return false;
}

//try migrate to comment from feedback script
//

function ExternalFeedBackDlg(urlLoad, urlSave) {

    var dlg = $("#FormFeedBack");

    dlg.dialog({
        autoOpen: false,
        title: "Feedback Dialog",
        height: 250,
        width: 360,
        modal: true,
        buttons:
        {
            'Close': function() {
                $(this).dialog('close');
            },
            'Send': function() {
                var bValid = true;
                var txtComment = $("#txtComment");
                var tips = $("#validateTips");

                txtComment.removeClass('ui-state-error');

                bValid = bValid && checkLength(tips, txtComment, "Comment", 3, 255);
                bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                    $.post(urlSave, { Comment: txtComment.val() });
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#txtComment");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });

    $.get(urlLoad, function(content) {
        dlg.html(content);
        dlg.dialog('open');
    });
    return false;
}


//
//

function ExternalFeedBackWithIndustryDlg(urlAction, dlgTitle, entityId, entityType, industryId, user) {

    if (dlgTitle == null)
        dlgTitle = "FeedBack Dialog";

    var dlg = $("#FormFeedBack");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 240,
        width: 360,
        modal: true,
        buttons:
        {
            'Close': function() {
                $(this).dialog('close');
            },
            'Send': function() {
                var bValid = true;
                var txtComment = $("#txtComment");
                var tips = $("#validateTips");
                var hdnEntityId = $("#hdnEntityId");
                var hdnIndustryId = $("#hdnIndustryId");
                var hdnEntityType = $("#hdnEntityType");
                var hdnUser = $("#hdnUser");
                txtComment.removeClass('ui-state-error');

                bValid = bValid && checkLength(tips, txtComment, "Comment", 3, 255);
                bValid = bValid && checkRegexp(tips, txtComment, /^[a-z]|[0-9]|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]+$/i, "Comment accept only alfabetic values.");

                if (bValid) {
                    //Send FeedBack
                    $.post(urlAction, { EntityId: hdnEntityId.val(), ObjectType: hdnEntityType.val(), Comment: txtComment.val(), IndustryId: hdnIndustryId.val(), U: hdnUser.val() });
                    $(this).dialog('close');
                }
            }
        },
        close: function() {
            var tips = $("#validateTips");
            var txtComment = $("#txtComment");
            txtComment.val("").removeClass('ui-state-error');
            tips.html("");
        }
    });
    //End-Dialog defined
    DialogContent = '<strong id="validateTips"></strong>' +
	        '<form>' + '<label for="txtComment">Comment</label>' +
            '<textarea name="txtComment" id="txtComment" WRAP=SOFT class="textareadialog text ui-widget-content ui-corner-all" COLS=50 ROWS=4 onClick="setClassOnFocus(this)"></textarea>' +
	        '<input type="hidden" value="' + entityId + '" name="hdnEntityId" id="hdnEntityId"/>' +
	        '<input type="hidden" value="' + industryId + '" name="hdnIndustryId" id="hdnIndustryId"/>' +
	        '<input type="hidden" value="' + user + '" name="hdnUser" id="hdnUser"/>' +
	        '<input type="hidden" value="' + entityType + '" name="hdnEntityType" id="hdnEntityType" />' +

	        '</form>';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

// External FeedBack with Attached Files
function ExternalFeedBackWithAttachedDlg(urlAction, dlgTitle) {
    var pcnDlg = $("#PrivateCommentNewDlg");
    pcnDlg.empty();
    pcnDlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        width: 500,
        //resizable: false,
        close: function(event, ui) { $(this).dialog("destroy"); hideLoadingDialog(); },
        buttons:
                {
                    "Close": function() { $(this).dialog("destroy"); hideLoadingDialog(); }
                     ,
                    "Send": function() {
                        $("#frameFeedBack").contents().find(".error").html('');
                        var success = true;
                        var message = '';
                        var comment = $("#frameFeedBack").contents().find("#txtComment").val();
                        if (comment == null || comment == undefined || comment == '') {
                            message = '<li>Comment is required</li>';
                            success = false;
                            $("#frameFeedBack").contents().find("#txtComment").addClass('validation-error-items');
                            $("#frameFeedBack").contents().find(".required").addClass('validation-error-text');
                        } else {
                            $("#frameFeedBack").contents().find("#txtComment").removeClass('validation-error-items');
                            $("#frameFeedBack").contents().find(".required").removeClass('validation-error-text');
                        }
                        var inputFiles = $("#frameFeedBack").contents().find(".fileDialogUpload ");
                        if (inputFiles != null && inputFiles != undefined && inputFiles.length > 0) {
                            for (var i = 0; i < inputFiles.length; i++) {

                                var v = inputFiles[i].value;
                                var file = inputFiles[i];
                                var fileId = file.id;
                                if (v != null && v != undefined && v != '') {
                                    if (file.files != null || file.files != undefined) {
                                        if (file.files.length != null && file.files.length != undefined && file.files.length > 0) {
                                            file = file.files[0];
                                        }
                                    }
                                    var response = '';
                                    if (file) {
                                        var fd = new FormData();
                                        fd.append('file', file);
                                        var xhr = new XMLHttpRequest();

                                        xhr.onreadystatechange = function() {
                                            if (xhr.readyState == 4 && xhr.status == 200) {
                                                response = xhr.responseText;
                                                fileId = fileId.replace('uploadfile', '');
                                                if (response == 'Sucessfull') {
                                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).removeClass('validation-error-items');
                                                }
                                                else if (response == 'UnSucessfull') {
                                                    success = false;
                                                    message = message + '<li>The file ' + v + ' was not successfully uploaded. Please re-submit</li> ';
                                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).addClass('validation-error-items');
                                                } else {
                                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).removeClass('validation-error-items');
                                                }
                                            }
                                        };
                                        xhr.open('POST', '/Forum.aspx/ValidateFile', false);
                                        xhr.send(fd);

                                    }
                                } else {
                                    // file is empty
                                    message = message + '<li>The file is empty</li> ';
                                    success = false;
                                    fileId = fileId.replace('uploadfile', '');
                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).addClass('validation-error-items');
                                }

                            }
                        }
//                        var filesInvalid = $("#frameFeedBack").contents().find(".validation-error-items ");
//                        if (filesInvalid != undefined && filesInvalid != null && filesInvalid.length > 0) {
//                            success = false;
//                        }
                        if (success) {
                            $("#frameFeedBack").contents().find("#FeeBackResponseForm").submit();
                            $(".ui-dialog-buttonpane button:contains('Send')").button().hide();
                            showLoadingDialog();
                        } else {
                            $("#frameFeedBack").contents().find(".error").html('<ul class="validation-summary-errors">' + message + '</ul>');
                        }
                    }
                }
    });
    pcnDlg.html("<iframe id='frameFeedBack' src='" + urlAction + "' style='overflow-x:hidden' width=100% height=200 frameBorder=0></iframe>");
    pcnDlg.dialog("open");
    hideLoadingDialog();
    return false;
};
//
function ExternalFeedBackWithAttachedVDlg(urlAction, dlgTitle, urlValidate) {
    var pcnDlg = $("#PrivateCommentNewDlg");
    pcnDlg.empty();
    pcnDlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        width: 500,
        //resizable: false,
        close: function(event, ui) { $(this).dialog("destroy"); hideLoadingDialog(); },
        buttons:
                {
                    "Close": function() { $(this).dialog("destroy"); hideLoadingDialog(); }
                     ,
                    "Send": function() {
                        $("#frameFeedBack").contents().find(".error").html('');
                        var success = true;
                        var message = '';
                        var comment = $("#frameFeedBack").contents().find("#txtComment").val();
                        if (comment == null || comment == undefined || comment == '') {
                            message = '<li>Comment is required</li>';
                            success = false;
                            $("#frameFeedBack").contents().find("#txtComment").addClass('validation-error-items');
                            $("#frameFeedBack").contents().find(".required").addClass('validation-error-text');
                        } else {
                            $("#frameFeedBack").contents().find("#txtComment").removeClass('validation-error-items');
                            $("#frameFeedBack").contents().find(".required").removeClass('validation-error-text');
                        }
                        var inputFiles = $("#frameFeedBack").contents().find(".fileDialogUpload ");
                        if (inputFiles != null && inputFiles != undefined && inputFiles.length > 0) {
                            showLoadingDialog();
                            for (var i = 0; i < inputFiles.length; i++) {

                                var v = inputFiles[i].value;
                                var file = inputFiles[i];
                                var fileId = file.id;
                                if (v != null && v != undefined && v != '') {
                                    if (file.files != null || file.files != undefined) {
                                        if (file.files.length != null && file.files.length != undefined && file.files.length > 0) {
                                            file = file.files[0];
                                        }
                                    }
                                    var response = '';
                                    if (file) {
                                        var fd = new FormData();
                                        fd.append('file', file);
                                        var xhr = new XMLHttpRequest();

                                        xhr.onreadystatechange = function() {
                                            if (xhr.readyState == 4 && xhr.status == 200) {
                                                response = xhr.responseText;
                                                fileId = fileId.replace('uploadfile', '');
                                                if (response == 'Sucessfull') {
                                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).removeClass('validation-error-items');
                                                }
                                                else if (response == 'UnSucessfull') {
                                                    success = false;
                                                    message = message + '<li>The file ' + v + ' was not successfully uploaded. Please re-submit</li> ';
                                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).addClass('validation-error-items');
                                                } else {
                                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).removeClass('validation-error-items');
                                                }
                                            }
                                        };
                                        xhr.open('POST', urlValidate, false);
                                        xhr.send(fd);

                                    }
                                } else {
                                    // file is empty
                                    message = message + '<li>The file is empty</li> ';
                                    success = false;
                                    fileId = fileId.replace('uploadfile', '');
                                    $("#frameFeedBack").contents().find("#txtuploadfile" + fileId).addClass('validation-error-items');
                                }

                            }
                        }
                        //                        var filesInvalid = $("#frameFeedBack").contents().find(".validation-error-items ");
                        //                        if (filesInvalid != undefined && filesInvalid != null && filesInvalid.length > 0) {
                        //                            success = false;
                        //                        }
                        if (success) {
                            $("#frameFeedBack").contents().find("#FeeBackResponseForm").submit();
                            $(".ui-dialog-buttonpane button:contains('Send')").button().hide();
                            showLoadingDialog();
                        } else {
                            $("#frameFeedBack").contents().find(".error").html('<ul class="validation-summary-errors">' + message + '</ul>');
                            hideLoadingDialog();
                        }
                    }
                }
    });
    pcnDlg.html("<iframe id='frameFeedBack' src='" + urlAction + "' style='overflow-x:hidden' width=100% height=200 frameBorder=0></iframe>");
    pcnDlg.dialog("open");
    hideLoadingDialog();
    return false;
};

//
function replaceContent(targetId, urlAction) {
    var TargetDiv = $('#' + targetId);
    TargetDiv.load(urlAction); //Load form for send response

    $("#FormComments").dialog("option", "buttons",
  { "Send": function() {
      var form = $("#DetailResponseForm");
      var data = form.serialize();
      var urlActionForm = form.prop('action');
      $.post(urlActionForm, data, function(response) {
          alert("Thank for comment!");
          $("#FormComments").dialog("destroy");
      });
  },
      "Cancel": function() { $(this).dialog("destroy"); }

  }
  );
}

function changeContent(targetId, urlAction) {
    var TargetDiv = $('#' + targetId);

    MakeIFrame("frameResponse", TargetDiv[0], urlAction, "100%", "290px")

    $("#FormComments").dialog("option", "buttons",
  { "Send": function() {
      $("#frameResponse").contents().find("#CommentsResponseForm")[0].submit();
      $(this).dialog("destroy");
  },
      "Cancel": function() { $(this).dialog("destroy"); }

  }
  );
}


function MakeIFrame(name, target, source, width, height) {
    ifrm = document.createElement("IFRAME");

    ifrm.setAttribute("name", name);
    ifrm.setAttribute("id", name);
    ifrm.setAttribute("src", source);
    ifrm.setAttribute("frameborder", "0");
    ifrm.style.width = width;
    ifrm.style.height = height;
    while (target.childNodes.length >= 1) {
        target.removeChild(target.firstChild);
    }

    target.appendChild(ifrm);
}
//for update Image icon of a comments after of update.
function AddComentsclassImage(Entityid) {
    $("#ImgComents" + Entityid).removeClass("ImageCommentsN");
    $("#ImgComents" + Entityid).addClass("ImageCommentsY");
}

function RemoveComentsclassImage(Entityid) {
    $("#ImgComents" + Entityid).removeClass("ImageCommentsY");
    var test = $("#ImgComents" + Entityid);
    $("#ImgComents" + Entityid).addClass("ImageCommentsN");
}

// Works at FrontEnd ContentPortal>Competitor
function DiscussionFormDlg(url, dlgTitle, urlNew, EntityId) {
    showLoadingDialog();
    var quizObject = $("#DiscussionsResponse");
    var ImageclassId = EntityId;
    quizObject.empty();
    quizObject.dialog(

          {
              bgiframe: true,
              autoOpen: false,
              title: dlgTitle,
              height: 400,
              width: 800,
              buttons:
             {
                 "Close": function() { $(this).dialog("destroy"); },
                 "New": function() {
                     $(this).dialog("destroy");
                     //                     if (ImageclassId != null) {
                     //                         AddComentsclassImage(ImageclassId);
                     //                     }
                     DiscussionsResponseNewDlg(urlNew, 0);
                 }
             }
          }
          );

    quizObject.load(url, {}, function() {
        $("#FormSubmit").css('visibility', 'hidden');
        $(this).dialog("open");
        hideLoadingDialog();
    });



    return false;
}
function ExternalDiscussionsDlg(url, dlgTitle, urlNew, EntityId) {
    showLoadingDialog();
    var quizObject = $("#DiscussionsResponse");
    var ImageclassId = EntityId;
    quizObject.empty();
    quizObject.dialog(

          {
              autoOpen: false,
              title: dlgTitle,
              height: 400,
              width: 800,
              buttons:
             {
                 "Close": function() { $(this).dialog("destroy"); },
                 "New": function() {
                     $(this).dialog("destroy");
                     //                     if (ImageclassId != null) {
                     //                         AddComentsclassImage(ImageclassId);
                     //                     }
                     DiscussionsResponseNewDlg(urlNew, 0);
                 }
             }
          }
          );

    //load and fill list of comments
    quizObject.load(url, {}, function() {
        $(this).dialog("open");
        hideLoadingDialog();
    });

    return false;
}
function GetExternalDiscussionsDlg(url, dlgTitle, urlNew, EntityId) {
    showLoadingDialog();
    var quizObject = $("#DiscussionsResponse");
    var ImageclassId = EntityId;
    quizObject.empty();
    quizObject.dialog(

          {
              autoOpen: false,
              title: dlgTitle,
              height: 400,
              width: 800,
              buttons:
             {
                 "Close": function() { $(this).dialog("destroy"); },
                 "New": function() {
                     $(this).dialog("destroy");
                     DiscussionsResponseNewDlg(urlNew, ImageclassId);
                 }
             }
          }
          );

    //load and fill list of comments
    quizObject.load(url, {}, function() {
        $(this).dialog("open");
        hideLoadingDialog();
    });

    return false;
}
function DiscussionsResponseNewDlg(urlAction) {
    showLoadingDialog();
    var ernDlg = $("#DiscussionsResponseNew");
    ernDlg.empty();
    ernDlg.dialog(
          {
              autoOpen: false,
              title: 'Discussion Dialog',
              width: 700,
              close: function(event, ui) { $(this).dialog("destroy"); hideLoadingDialog(); },
              buttons:
             {
                 "Close": function() { $(this).dialog("destroy"); hideLoadingDialog(); },
                 "Send": function() {
                 $("#frameCommentResponse").contents().find("#DiscussionsResponseForm").submit();
                     $(".ui-dialog-buttonpane button:contains('Send')").button().hide();
                     $(".ui-dialog-buttonpane button:contains('Reset')").button().hide();
                 },
                 "Reset": function() {
                 $("#frameCommentResponse").contents().find("#DiscussionsResponseForm")[0].reset();       
                 }
             }
          }
          );
    //get url into iframe
          ernDlg.html("<iframe id='frameCommentResponse' src='" + urlAction + "' style='overflow-x:hidden' width=100% height=290 frameBorder=0></iframe>");
    ernDlg.dialog("open");
    hideLoadingDialog();

    return false;
}
function RemoveComentsclassImage(Entityid) {
    $("#ImgComents" + Entityid).removeClass("ImageCommentsY");
    var test = $("#ImgComents" + Entityid);
    $("#ImgComents" + Entityid).addClass("ImageCommentsN");
    if ($('img[name=ImgComents' + Entityid + ']').length > 0) {
        $('img[name=ImgComents' + Entityid + ']').removeClass("ImageCommentsY");
        $('img[name=ImgComents' + Entityid + ']').addClass("ImageCommentsN");
    }    
}
function isNumbersAndPointKey(event, element, _float) {
    event = event || window.event;
    var charCode = event.which || event.keyCode;
    if (charCode == 8 || charCode == 13 || (_float ? (element.value.indexOf('.') == -1 ? charCode == 46 : false) : false))
        return true;
    else if ((charCode < 48) || (charCode > 57))
        return false;
    return true;
}

function FormBoxDynamic(object, dialogTitle, dialogContent, urlAction, target, refval1, type) {
    var commentObject = $("#MessageBox");
    commentObject.empty();
    commentObject.dialog({ autoOpen: false,
        width: 360,
        modal: true,
        buttons: { "Ok": function() {
            var urls = $("#ComparinatorFormEdit")[0].txtLinks.value;
            if (urls.length > 0 && !isUrls(urls)) {
                alert('URL is not valid');
            }
            else {
                $.post(urlAction,
                  $("#ComparinatorFormEdit").serialize(),
                  function(data) {
                      //return value + "@" + notes + "@" + links + "@" + feature
                      //update values
                      var data_array = data.split("@");
                      $('#val' + refval1).children("div").children("div").children("div").children("span").text(data_array[0]);
                      $('#Note' + refval1).html(data_array[1]);
                      $('#Links' + refval1).html(ToLinks(data_array[2].toString()));

                      //Assign featuec lass for filters
                      var featureClass = data_array[3].toString();
                      $('#val' + refval1).prop("class", featureClass);
                      $('#Feature' + refval1).text(featureClass);

                      var tempo = data_array[0];
                      GetParentOfObjectColor(object, tempo, featureClass, type, data_array[4]);
                      GetParentOfObject(object, tempo);
                      if ($('#chkcolorizeon').prop('checked')) {
                          colorizeFeatureForProducts(1);
                      }
                      else {
                          colorizeFeatureForProducts(0);
                      }

                  });
                $(this).dialog("destroy");

            }


        } //end-ok
        , "Close": function() { $(this).dialog("destroy"); }
        }
    });
    commentObject.keypress(
        function(e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                var parentMessageBox = $(this).parent();
                var divButtonPane = parentMessageBox.find('.ui-dialog-buttonpane');
                var divButtonSet = divButtonPane.find('.ui-dialog-buttonset');
                var divButton = divButtonSet.find('.ui-button');
                var spanButton = divButton.find('.ui-button-text');
                spanButton.click();
                return false;
            }
        }
    );
    commentObject.dialog('option', 'title', dialogTitle);
    commentObject.html(dialogContent);
    commentObject.dialog("open");
}

function AddDynamicDialog(object, Title, urlAction, val0, val1, val2, val3, refval1, type, feature) {
    var val4 = jQuery.trim($('#val' + refval1).children("div").children("div").children("div").children("span").text());
    var val5 = $('#Note' + refval1).text();
    var val6 = $('#Links' + refval1).text();
    var feature = $('#Feature' + refval1).text();
    var strForm = "<form id='ComparinatorFormEdit'>";
    strForm += "<input type='hidden' name='IndustryId' value='" + val0 + "' />";
    strForm += "<input type='hidden' name='txtType' value='" + val1 + "' />";
    strForm += "<input type='hidden' name='txtCriteriaId' value='" + val2 + "' />";
    strForm += "<input type='hidden' name='txtEntityId' value='" + val3 + "' />";
    strForm += "<input type='hidden' name='txtCriteriaType' value='" + type + "' />";

    strForm += "<label>Value</label>";
    if (type == 'NUM') {
        strForm += "<input type='text' id='txtValue' name='txtValue' onfocus='LoadAutoComplete(\"" + val0 + "\" , \"" + val2 + "\")' value='" + val4 + "' onkeypress = 'return isNumbersAndPointKey(event, this, true)'  />"
    }
    else if (type == 'BOL') {
        strForm += "<select name='txtValue' id='txtValue' >"
        strForm += "<option value=''></option>";
        var syes = "", snot = "";

        if (val4 == 'Yes') {
           syes="selected";
        }
        else if (val4 == 'No') {
            snot="selected";
        } 
        strForm += "<option value='No'  "+syes+">No</option>";
        strForm += "<option value='Yes' "+snot+">Yes</option>";
        strForm += "</select>";
    }
    else if (type == 'LIS') { //????
        strForm += "<input type='text' id='txtValue' name='txtValue' onfocus='LoadAutoComplete(\"" + val0 + "\" , \"" + val2 + "\")' value='" + val4 + "'  />"
    }
    else {
        strForm += "<input type='text' id='txtValue' name='txtValue' onfocus='LoadAutoComplete(\"" + val0 + "\" , \"" + val2 + "\")' value='" + val4 + "'  />"

    } strForm += "<br />";

    strForm += "<label>Notes:</label><textarea name='txtNotes' WRAP=SOFT COLS=50 ROWS=4 class='textareadialog' onClick='setClassOnFocus(this)'>" + jQuery.trim(val5) + "</textarea><br />";
    strForm += "<label>Links:(include http://)</label><textarea name='txtLinks' WRAP=SOFT COLS=50 ROWS=4 class='textareadialog' onClick='setClassOnFocus(this)'>" + jQuery.trim(val6) + "</textarea><br />";
    //it's dangerous, because need get from database
    if (type != 'NUM' && type != 'BOL') {
        strForm += "<label>Ranking</label><select name='Feature'><option value=''></option>";
        strForm += "<option value='BC' " + (feature == "BC" ? "selected" : "") + ">Best in Class</option>";
        strForm += "<option value='MA' " + (feature == "MA" ? "selected" : "") + ">Market Advantage</option>";
        strForm += "<option value='MP' " + (feature == "MP" ? "selected" : "") + ">Market Parity</option>";
        strForm += "<option value='MD' " + (feature == "MD" ? "selected" : "") + ">Market Disadvantage</option>";
        strForm += "<option value='LM' " + (feature == "LM" ? "selected" : "") + ">Lagging Market</option>";
        strForm += "</select>";
    }
    //alert("|"+feature+"|");
    strForm += "</form>";
    FormBoxDynamic(object, Title, strForm, urlAction, 'MessageStatus', refval1, type);
}


//append flexibility dialogs

function CellPropertyDlg(ptitle,urlLoad, urlSave)
{
  var commentObject = $("#MessageBox"); 
  commentObject.empty();
  commentObject.dialog({ autoOpen: false,
      title: ptitle,
      width: 360,
      modal: true,
      buttons: { "Ok": function() {
          var urls = $("#ComparinatorFormEdit")[0].txtLinks.value;
          if (urls.length > 0 && !isUrls(urls)) {
              alert('URL is not valid');
          }
          else {
              $.post(urlSave,
                  $("#ComparinatorFormEdit").serialize(),
                  function(data) {
                      if (data != null) {
                          $("#C" + data.cid).find("#P" + data.pid).find("div:first span").text(data.value);
                          $("#C" + data.cid).find("#P" + data.pid).find("div:first").next().addClass("comp_crbr");
                          $("#C" + data.cid).attr('class', data.creq);
                          var useselected = $('#chkboxselprods').prop('checked');
                          //Update Feature
                          for (i = 0; i < data.pfea.length; i++) {
                              $("#C" + data.cid).find("#P" + data.pfea[i].pid).attr("s", data.pfea[i].s);
                              $("#C" + data.cid).find("#P" + data.pfea[i].pid).attr("a", data.pfea[i].a);
                              if (useselected)
                                  $("#C" + data.cid).find("#P" + data.pfea[i].pid).attr("class", data.pfea[i].s);
                              else
                                  $("#C" + data.cid).find("#P" + data.pfea[i].pid).attr("class", data.pfea[i].a);
                          }
                      }
                      //colorize corner
                      //colorize comments icon
                      //colorize rows by feature
                      colorizeFeatureForProducts(0);
                      var rdColor = $('#radcolorizeon').attr('checked');
                      if (rdColor == undefined || rdColor == null) {
                          rdColor = $('#radcolorizeon').is(':checked');
                      }
                      if (rdColor)
                          colorizeFeatureForProducts(1);


                      if (data.rtm != null && data.rtm != "") {
                          //$(this).dialog("destroy");
                          var FormMessage = $("#FormMessage");
                          FormMessage.empty();
                          FormMessage.dialog({width: 500, title: "Alert Message", buttons: { "Ok": function() { $(this).dialog("destroy"); } } });
                          FormMessage.html(data.rtm);
                          FormMessage.dialog("open");
                      }

                  });
              $(this).dialog("destroy");

          }

      } //end-ok
        , "Close": function() { $(this).dialog("destroy"); }
      }
  });
    commentObject.keypress(
        function(e) {
            if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                var parentMessageBox = $(this).parent();
                var divButtonPane = parentMessageBox.find('.ui-dialog-buttonpane');
                var divButtonSet = divButtonPane.find('.ui-dialog-buttonset');
                var divButton = divButtonSet.find('.ui-button');
                var spanButton = divButton.find('.ui-button-text');
                spanButton.click();
                return false;
            }
        }
    );
    
    $.get(urlLoad, function(data) {
//        commentObject.dialog('option', 'title', dialogTitle);
        commentObject.html(data);
        commentObject.dialog("open");
    });

}

