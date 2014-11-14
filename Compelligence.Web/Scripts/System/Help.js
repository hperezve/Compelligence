var htmlEncode = function(value) {
    return $('<div/>').text(value).html();
};
var htmlDecode = function(value) {
    return $('<div/>').html(value).text();
};
var isEmpty = function(str) {
    var result = true;
    if (str != undefined && str != null && str.length > 0)
        result = false;
    return result;
};
var ShowAuthentication = function(scope, entity, dialogTitle, urlAction, urlActionUpdate) {
    var isValidTheForm = false;
    var messageOfValidation = '';
    var lblValidSum = $('#LblValidationSumary');
    var helpDialog = $("#HelpDialog");
    helpDialog.empty();
    helpDialog.dialog({
        width: 285, //if change the width on FrontEndSite.css
        modal: true,
        dialogClass: 'noTitleStuff',
        buttons: {
            "Go": function() {
                $('#LblValidationSumary').text('');
                var uName = $('#txtUserName').val();
                var uKennwort = $('#txtKennwort').val();
                if (uName != undefined && uName != null && uName.length > 0 && uKennwort != undefined && uKennwort != null && uKennwort.length > 0) {
                    if ((uName === 'ed@compelligence.com' && uKennwort === 'Password1!') || (uName === 'mitch@compelligence.com' && uKennwort === 'Password1!')) {
                        isValidTheForm = true;
                    } else {
                        isValidTheForm = false;
                        messageOfValidation += 'The email or password provided is incorrect<br/>';
                    }
                } else {
                    isValidTheForm = false;
                    messageOfValidation += 'The email or password provided is empty<br/>';
                }
                if (isValidTheForm) {
                    $(this).dialog('destroy');
                    SetValuesToShowHelp(scope, entity, 'edit', urlAction, urlActionUpdate,'true', actionFrom);
                } else {
                    $('#LblValidationSumary').empty();
                    $('#LblValidationSumary').prop('innerHTML', messageOfValidation);
                    $('#LblValidationSumary').css("color", "red");
                }
            },
            "Cancel": function() {
                $(this).dialog('destroy');
            }
        }
    });
    var strShowHelpForm = "<div>";
    strShowHelpForm += "<label id='LblValidationSumary'></label>";
    strShowHelpForm += "<br />";
    strShowHelpForm += "<div style='margin-left: 5px;'>";
    strShowHelpForm += "<lable>User Name: </label>";
    strShowHelpForm += "</div>";
    strShowHelpForm += "<div>";
    strShowHelpForm += "<input type='text' name='txtUserName' id='txtUserName' value='' />";
    strShowHelpForm += "</div>";
    strShowHelpForm += "<br />";
    strShowHelpForm += "<div style='margin-left: 5px;'>";
    strShowHelpForm += "<lable>Password: </label>";
    strShowHelpForm += "</div>";
    strShowHelpForm += "<div>";
    strShowHelpForm += "<input type='password' name='txtKennwort' id='txtKennwort' value='' />";
    strShowHelpForm += "</div>";
    helpDialog.dialog('option', 'title', dialogTitle);
    helpDialog.html(strShowHelpForm);
    helpDialog.dialog("open");
};
var ShowHelp = function(scope, entity, action, dialogTitle, dialogContent, urlAction, urlActionUpdate, editHelp, actionFrom, title) {
    var helpDialog = $("#HelpDialog");
    helpDialog.empty();
    if (action === 'edit') {
        helpDialog.dialog({
            width: 575, //if change the width on FrontEndSite.css
            height: 350,
            modal: true,
            buttons: {
                "Save": function() {
                    var txtSubject = $('#txtSubject').val();
                    var txtContent = $('#txtContent').val();
                    var hwpEncode = htmlEncode(txtContent);
                    var parameters = { subject: txtSubject, content: hwpEncode, scope: scope, entity: entity, actionFrom: actionFrom };
                    var lblValidSum = $('#LblValidationSumary');
                    var isValidTheForm = true;
                    var messageOfValidation = '';
                    if (isEmpty(txtSubject) || txtSubject.length > 255 || isEmpty(txtContent)) {
                        isValidTheForm = false;
                    }
                    if (!isValidTheForm) {
                        if (isEmpty(txtSubject)) {
                            messageOfValidation += 'Subject is required<br/>';
                        }
                        if (txtSubject.length > 255) {
                            messageOfValidation += 'Subject has more than 255 characters<br/>';
                        }
                        if (isEmpty(txtContent)) {
                            messageOfValidation += 'Message is required<br/>';
                        }
                        lblValidSum.prop('innerHTML', messageOfValidation);
                        lblValidSum.css("color", "red");
                    } else {
                        $.post(urlActionUpdate, parameters, function(data) {

                        });
                        $(this).dialog('destroy');
                    }
                },
                "Cancel": function() {
                    $(this).dialog('destroy');
                }
            }
        });
    } else {
        if (editHelp != undefined && editHelp != null && (editHelp === 'true' || editHelp === 'True' || editHelp === 'TRUE')) {
            helpDialog.dialog({
                width: 575, //if change the width on FrontEndSite.css
                height: 350,
                modal: true,
                buttons: {
                    "Edit": function() {
                        $(this).dialog('destroy');
                        SetToShowHelpWithTitle(scope, entity, 'edit', urlAction, urlActionUpdate, editHelp, actionFrom, title);
                    },
                    "Ok": function() {
                        $(this).dialog('destroy');
                    }
                }
            });
        } else {
            helpDialog.dialog({
                width: 575, //if change the width on FrontEndSite.css
                height: 350,
                modal: true,
                buttons: {
                    "Ok": function() {
                        $(this).dialog('destroy');
                    }
                }
            });
        }
    }
    helpDialog.dialog('option', 'title', dialogTitle);
    helpDialog.html(dialogContent);
    helpDialog.dialog("open");
    //$(".ui-dialog-titlebar").hide();
    $('.txtBoxHtml').cleditor({ height: 150, width: 545 });

};
var SetValuesToShowHelp = function(scope, objectName, action, urlAction, urlActionUpdate, editHelp, actionFrom, entity) {
    var title = [scope, objectName];
    SetToShowHelpWithTitle(scope, entity, action, urlAction, urlActionUpdate, editHelp, actionFrom, title.join(":"));
}
var SetToShowHelpWithTitle = function(scope, entity, action, urlAction, urlActionUpdate, editHelp, actionFrom, title) {
    var titleText = scope + " > " + title;
    if (title != undefined && title != null && title.length > 0) {
        titleText = "";
        var array = title.split(",");
        var tiT = title.split(":");
        for (var x = 0; x < tiT.length; x++) {
            if (titleText.length>0)
                titleText += " > ";
            titleText += tiT[x];
        }
    }
    var results = null;
    var subject = '';
    var contentValue = '';
    var parametro = { entityType: entity, actionFrom: actionFrom };
    $.get(urlAction, parametro, function(data) {
        if (data != null && data != '') {
            results = data;
            if (results != "") {
                var r = results.split('[TOKEN]');
                subject = r[0];
                contentValue = htmlDecode(r[1]);


                var strShowHelpForm = "<div>";
                if (action === 'edit'){
                    strShowHelpForm += "<br />"
                    strShowHelpForm += "<div id='helpEdit' style=margin-top:-7px>";
                    strShowHelpForm += "<label id='LblValidationSumary'></label>";
                    strShowHelpForm += "<label><b style='margin-left:-2px'>Subject:</b></label> ";
                    strShowHelpForm += "<div style=margin-left:-3px;>";
                    strShowHelpForm += "<input type='text' name='txtSubject' id='txtSubject' value='" + subject + "'/>";
                    strShowHelpForm += "</div>";
                    strShowHelpForm += "<label style=margin-top:2px><b>Message:</b></label>";
                    strShowHelpForm += "<br/>";
                    strShowHelpForm += "<div style=margin-top:5px;margin-left:2px>";
                    strShowHelpForm += "<textarea name='txtContent' id='txtContent' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
                    strShowHelpForm += contentValue;
                    strShowHelpForm += "</textarea>";
                    strShowHelpForm += "</div>";
                    strShowHelpForm += "</div>";
                } else {

                    strShowHelpForm += "<div id='helpShow' style=margin-top:7px>";
                    strShowHelpForm += "<div>";
                    strShowHelpForm += "<label id='lblSubject'>" + subject + "</label>";
                    strShowHelpForm += "</div>";
                    strShowHelpForm += "<br />";
                    strShowHelpForm += "<div style=margin-left:-3px>";
                    strShowHelpForm += contentValue;
                    strShowHelpForm += "</div>";
                    strShowHelpForm += "</div>";
                }
                strShowHelpForm += "<br />";
                strShowHelpForm += "";

                strShowHelpForm += "<br />";
                strShowHelpForm += "</div>";
                ShowHelp(scope, entity, action, 'Compelligence Help: ' + titleText, strShowHelpForm, urlAction, urlActionUpdate, editHelp, actionFrom, title);
            }
        }
    });
};