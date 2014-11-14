//$ = jQuery.noConflict(true); 
function MessageDlg(dlgTitle, dlgContent) {
    var dlg = $("#FormMessage");
    dlg.dialog(
      { autoOpen: false,
          title: dlgTitle,
          buttons: { "Ok": function() {
              $(this).dialog("destroy");
          }
          }
      }
      );
    dlg.html(dlgContent);
    dlg.dialog("open");
}

function MessageDlgEvent(dlgTitle, dlgContent, actionEvent, entityid, objectType, urlForum, dialogType) {
    $.post(urlForum, { ids: entityid, ObjectType: objectType }, function(data) {
        var evs = data;
        if (evs == "false") {

            RemoveComentsclassImage(entityid);
        }

    });
    var dataForAjax = entityid;
    var dlg = $("#FormMessage");
    dlg.dialog(
      { autoOpen: false,
          modal: true,
          title: dlgTitle,
          buttons: { "Ok": function() {
              $(dialogType).dialog("destroy");
              eval(actionEvent);
              $(this).dialog("destroy");
          }
          }
      }
      );
    dlg.html(dlgContent);
    dlg.dialog("open");
}

function MessageDlgSilverComment(dlgTitle, dlgContent, actionEvent, entityid, objectType, urlForum, dialogType, industryId, productId) {
    $.post(urlForum, { ids: entityid, ObjectType: objectType, industryId:industryId, productId: productId }, function(data) {
        var evs = data;
        if (evs == "false") {
            RemoveComentsclassImage('_'+industryId+'_'+productId);
        }

    });
    var dataForAjax = entityid;
    var dlg = $("#FormMessage");
    dlg.dialog(
      { autoOpen: false,
          modal: true,
          title: dlgTitle,
          buttons: { "Ok": function() {
              $(dialogType).dialog("destroy");
              eval(actionEvent);
              $(this).dialog("destroy");
          }
          }
      }
      );
    dlg.html(dlgContent);
    dlg.dialog("open");
};

function MessageDlgUrl(dlgTitle, urlFrom, urlReal) {
    var dlg = $("#FormMessage");
    dlg.dialog(
      { autoOpen: false,
          height: 380,
          width: 650,
          modal: true,
          title: dlgTitle,
          buttons:
          { "Ok": function() {
              $(this).dialog("destroy");
          },
              "Open news": function() {
                  window.open(urlReal, "Windows", "width=900,height=600,scrollbars=YES");
              }
          }
      }
      );

    $.get(urlFrom, {}, function(data) { dlg.html(data) });  //Close
    dlg.dialog("open");
}

function MessageConfirm(urlAction, onsuccesevent, entityid, ObjectType, urlForum, dialogType) {
    var dlg = $("#FormConfirm").dialog({
        bgiframe: true,
        resizable: false,
        width: 400,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Yes': function() {
                $.get(urlAction, {}, function() {
                    onsuccesevent = replaceAll(onsuccesevent, "|", "'");
                    //eval(onsuccesevent);
                    $("#FormConfirm").dialog("destroy");
                    MessageDlgEvent("Message", "<br>Comment Successfully deleted!", onsuccesevent, entityid, ObjectType, urlForum, dialogType);
                });
                //$(this).dialog('close');


            },
            'No': function() {
                $(this).dialog('close');
            }
        }
    });
    /*dlg.html("Are you sure")*/
    dlg.dialog("open");
}
function MessageConfirmRemoveSilverComment(urlAction, onsuccesevent, entityid, ObjectType, urlForum, dialogType, industryId, productId) {
    var dlg = $("#FormConfirm").dialog({
        bgiframe: true,
        resizable: false,
        width: 400,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Yes': function() {
                $.get(urlAction, {}, function() {
                    onsuccesevent = replaceAll(onsuccesevent, "|", "'");
                    //eval(onsuccesevent);
                    $("#FormConfirm").dialog("destroy");
                    //MessageDlgEvent("Message", "<br>Comment Successfully deleted!", onsuccesevent, entityid, ObjectType, urlForum, dialogType);
                    MessageDlgSilverComment("Message", "<br>Comment Successfully deleted!", onsuccesevent, entityid, ObjectType, urlForum, dialogType, industryId, productId);    
                });
                //$(this).dialog('close');


            },
            'No': function() {
                $(this).dialog('close');
            }
        }
    });
    /*dlg.html("Are you sure")*/
    dlg.dialog("open");
}
function MessageConfirmAndRefreshIcon(urlAction, onsuccesevent, entityId) {
    var dlg = $("#FormConfirm").dialog({
        bgiframe: true,
        resizable: false,
        width: 400,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Yes': function() {
                RemoveComentsclassImage(entityId);
                $.get(urlAction, {}, function() {
                    onsuccesevent = replaceAll(onsuccesevent, "|", "'");
                    //eval(onsuccesevent);
                    $("#FormConfirm").dialog("destroy");
                    MessageDlgEvent("Message", "<br>COMMENT SUCCESSFULLY DELETED", onsuccesevent);
                });
                //$(this).dialog('close');


            },
            'No': function() {
                $(this).dialog('close');
            }
        }
    });
    /*dlg.html("Are you sure")*/
    dlg.dialog("open");
}

function replaceAll(source, stext, rtext) {
    var strReplaceAll = source;
    var intIndexOfMatch = strReplaceAll.indexOf(stext);

    // Loop over the string value replacing out each matching
    // substring.
    while (intIndexOfMatch != -1) {
        // Relace out the current instance.
        strReplaceAll = strReplaceAll.replace(stext, rtext)
        // Get the index of any next matching substring.
        intIndexOfMatch = strReplaceAll.indexOf(stext);
    }
    return strReplaceAll;
}
//Function for Dialog delete company
function ConfirmDlg(urlAction, dlgTitle) {

    if (dlgTitle == null)
        dlgTitle = "Delete Company";
    var dlg = $("#FormMessages");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 150,
        width: 300,
        modal: true,
        buttons:
        {
            'No': function() {
                $(this).dialog('close');
            },
            'Yes': function() {

                location = urlAction;
                //$.get(urlAction + "?DealId=" + Id, {}, function(data) //Close
                //$.get(urlAction);
                $(this).dialog('close');

            }
        }
    });
    //End-Dialog defined
    DialogContent = 'Are you Sure of delete this Company?';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

function ConfirmDDlg(urlAction, dlgTitle) {

    if (dlgTitle == null)
        dlgTitle = "Delete Company";
    var dlg = $("#FormMessages");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 150,
        width: 300,
        modal: true,
        buttons:
        {
            'No': function() {
                $(this).dialog('close');
            },
            'Yes': function() {

                location = urlAction;
                //$.get(urlAction + "?DealId=" + Id, {}, function(data) //Close
                //$.get(urlAction);
                $(this).dialog('close');

            }
        }
    });
    //End-Dialog defined
    DialogContent = 'Are you Sure of delete this Company?';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

function ConfirmToDeleteDDlg(urlAction, urlAction2, dlgTitle, hidenvalues) {

    if (dlgTitle == null)
        dlgTitle = "Delete Company";
    var dlg = $("#FormMessages");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 150,
        width: 300,
        modal: true,
        buttons:
        {
            'No': function() {
                $(this).dialog('close');
            },
            'Yes': function() {
                $.ajax({
                    url: urlAction,
                    type: 'POST',
                    data: { Id: hidenvalues },
                    traditional: true,
                    success: function(Data) {
                        if (Data != "") {
                            location.href = urlAction2;
                        }
                    }
                });
                $(this).dialog('close');

            }
        }
    });
    //End-Dialog defined
    DialogContent = 'Are you Sure of delete this Company?';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

//Function for Dialog delete company
function ConfirmDeleteWhitePaper(urlAction, dlgTitle) {

    if (dlgTitle == null)
        dlgTitle = "Delete WhitePaper";
    var dlg = $("#FormMessages");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 150,
        width: 300,
        modal: true,
        buttons:
        {
            'No': function() {
                $(this).dialog('close');
            },
            'Yes': function() {

                location = urlAction;
                //$.get(urlAction + "?DealId=" + Id, {}, function(data) //Close
                //$.get(urlAction);
                $(this).dialog('close');

            }
        }
    });
    //End-Dialog defined
    DialogContent = 'Are you Sure of delete this WhitePaper?';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}

function ConfirmDeleteClientCompanyFile(urlAction, dlgTitle) {

    if (dlgTitle == null)
        dlgTitle = "Delete File";
    var dlg = $("#FormMessages");

    dlg.dialog({
        autoOpen: false,
        title: dlgTitle,
        height: 150,
        width: 300,
        modal: true,
        buttons:
        {
            'No': function() {
                $(this).dialog('close');
            },
            'Yes': function() {

                location = urlAction;
                $(this).dialog('close');

            }
        }
    });
    //End-Dialog defined
    DialogContent = 'Are you Sure of delete this file?';
    dlg.html(DialogContent);
    dlg.dialog('open');
    return false;
}