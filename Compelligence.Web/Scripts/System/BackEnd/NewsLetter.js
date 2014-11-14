var returnValues = function() {
    var browseName = 'NewsLetterSection';
    var browseEvents = 'NewsLetterSectionEvent';
    var browseNews = 'NewsLetterSectionNews';

    var gridId = '#' + browseName + 'ListTable';
    var gridIdEvents = '#' + browseEvents + 'ListTable';
    var gridIdNews = '#' + browseNews + 'ListTable';
    var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
    var multiselect2 = getBooleanValue($(gridIdEvents).getGridParam('multiselect'));
    var multiselect3 = getBooleanValue($(gridIdNews).getGridParam('multiselect'));

    var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
            selectedItems2 = $(gridIdNews).getGridParam("selarrrow");
            selectedItems2 = selectedItems2.join(':');
            selectedItems3 = $(gridIdEvents).getGridParam("selarrrow");
            selectedItems3 = selectedItems3.join(':');

        }

        else {
            selectedItems = $(gridId).getGridParam('selrow');
            //alert
            alert(selectedItems + " selrow");
        }

        var funcional = selectedItems.split(":")
        var funcional2 = selectedItems2.split(":")
        var funcional3 = selectedItems3.split(":")

        var gRow = $(gridId).jqGrid('getRowData', $(gridId).getGridParam('selrow'));
        var entityType = 'Project';

        var gRowNews = $(gridIdNews).jqGrid('getRowData', $(gridIdNews).getGridParam('selrow'));
        var entityTypeNews = 'News';

        var gRowEvents = $(gridIdEvents).jqGrid('getRowData', $(gridIdEvents).getGridParam('selrow'));
        var entityTypeEvents = 'Events';

        if (checkNullString(selectedItems) == '' && checkNullString(selectedItems2) == '' && checkNullString(selectedItems3) == '') {
            showAlertSelectItemDialog();
            return;
        }
       
            if (funcional != "") {
                $.each(funcional, function(key, value) {
                    var param = { id: value, type: 'Project', Description: FuncionaCk };
                    cocoa(param);
                });
            }

            if (funcional2 != "") {
                $.each(funcional2, function(key, value) {
                    var param2 = { id: value, type: 'News', Description: FuncionaCk };
                    cocoa(param2);
                });
            }

            if (funcional3 != "") {
                $.each(funcional3, function(key, value) {
                    var param3 = { id: value, type: 'Events', Description: FuncionaCk };
                    cocoa(param3);
                });
            }

};

function NewsLetterAddItemDlg(urlAction,formId) {
//    showLoadingDialog();
    var Dlg = $("#" + formId + "NewsLetterAddItemDlg");
    Dlg.empty();
    Dlg.dialog(
          {
              bgiframe: true,
              autoOpen: false,
              modal: true,
              resizable: false,
              title: 'Add Item Dialog',
              height: 450,
              width: 730,
              close: function(event, ui) { $(this).dialog("destroy"); hideLoadingDialog(); },
              buttons:
             {
                 "Close": function() { $(this).dialog("destroy").empty(); hideLoadingDialog(); },
                 "Add": function() { returnValues(); $(this).dialog("destroy").empty(); hideLoadingDialog(); }
             }
          }
          );
    Dlg.load(urlAction);
    Dlg.dialog("open");
    return false;
}