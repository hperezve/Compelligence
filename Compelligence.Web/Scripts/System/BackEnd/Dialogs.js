$(function() {
    $("#AlertFailedResponseDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });

    $("#AlertSelectItemDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });

    $("#AlertNoFoundCalendarDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });

    $("#ReSendEmailToApprovedDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });
    $("#IncorrectProcessDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });
    $("#CreateObjectDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });

    $("#AlertReturnMessageDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
                $('#AlertReturnMessageDialog').empty();
            }
        }
    });

    $("#DeleteConfirmDialog").dialog({
        bgiframe: true,
        resizable: false,
        //        height: 140,
        height: 190,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Delete selected item': function() {
                var itemId = $(this).dialog('option', 'itemId');
                var urlAction = $(this).dialog('option', 'urlAction');
                var targetDelete = $(this).dialog('option', 'targetDelete');
                var targetSection = $(this).dialog('option', 'targetSection');
                var isDetail = $(this).dialog('option', 'isDetail');
                var headerType = $(this).dialog('option', 'headerType');
                var detailFilter = $(this).dialog('option', 'detailFilter');
                var containerSection = $(this).dialog('option', 'containerSection');

                deleteData(urlAction, itemId, targetDelete, targetSection, isDetail, containerSection, headerType, detailFilter);

                $(this).dialog('close');
            },
            Cancel: function() {
                $(this).dialog('close');
            }
        },
        itemId: '',
        urlAction: '',
        targetDelete: '',
        targetSection: '',
        isDetail: false,
        headerType: '',
        detailFilter: '',
        containerSection: ''
    });

    $("#ChooseConfirmDueDate").dialog({
        bgiframe: true,
        resizable: false,
        //        height: 140,
        height: 190,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            Accept: function() {

                $(this).dialog('close');
            },
            Cancel: function() {
                $(this).dialog('close');
                $('#WorkspaceProjectEditFormDueDateFrm').val('')

            }
        }
    });

    $("#DeleteDetailConfirmDialog").dialog({
        bgiframe: true,
        resizable: false,
        height: 140,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Delete selected relation': function() {
                var itemId = $(this).dialog('option', 'itemId');
                var urlAction = $(this).dialog('option', 'urlAction');
                var targetDelete = $(this).dialog('option', 'targetDelete');
                var targetSection = $(this).dialog('option', 'targetSection');
                var isDetail = $(this).dialog('option', 'isDetail');
                var headerType = $(this).dialog('option', 'headerType');
                var detailFilter = $(this).dialog('option', 'detailFilter');
                var containerSection = $(this).dialog('option', 'containerSection');

                deleteDetailData(urlAction, itemId, targetDelete, targetSection, isDetail, containerSection, headerType, detailFilter);

                $(this).dialog('close');
            },
            Cancel: function() {
                $(this).dialog('close');
            }
        },
        itemId: '',
        urlAction: '',
        targetDelete: '',
        targetSection: '',
        isDetail: false,
        headerType: '',
        detailFilter: '',
        containerSection: ''
    });

    $("#UploadFileConfirmDialog").dialog({
        bgiframe: true,
        resizable: false,
        //        height: 140,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Yes': function() {
                uploadFileComponent.submit();
                $(this).dialog('close');
            },
            'No': function() {
                $(this).dialog('close');
            }
        }
    });

    $("#SendResponseMessageDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });

    $("#RemoveUsersConfirmDialog").dialog({
        bgiframe: true,
        resizable: false,
        height: 140,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Remove Users': function() {
                var itemId = $(this).dialog('option', 'itemId');
                var urlAction = $(this).dialog('option', 'urlAction');
                var targetDelete = $(this).dialog('option', 'targetDelete');
                var targetSection = $(this).dialog('option', 'targetSection');
                var isDetail = $(this).dialog('option', 'isDetail');
                var headerType = $(this).dialog('option', 'headerType');
                var detailFilter = $(this).dialog('option', 'detailFilter');
                var containerSection = $(this).dialog('option', 'containerSection');
                if (containerSection != null) {
                    showLoadingDialogForSection(containerSection);
                } else {
                    showLoadingDialog();
                }
                //was remove + '/' + itemId for compatibility with jquery 1.9
                var newId = itemId + "";
                $.post(urlAction, { Id: newId, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter }, function() { reloadGrid(targetDelete); if (containerSection != null) { hideLoadingDialogForSection(containerSection); } else { hideLoadingDialog(); } });
                $(this).dialog('close');
            },
            Cancel: function() {
                $(this).dialog('close');
            }
        },
        itemId: '',
        urlAction: '',
        targetDelete: '',
        targetSection: '',
        isDetail: false,
        headerType: '',
        detailFilter: '',
        containerSection: ''
    });
    $("#PublishConfirmationDialog").dialog({
        bgiframe: true,
        resizable: false,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        },
        buttons: {
            'Publish selected Project': function() {
                var itemId = $(this).dialog('option', 'itemId');
                var urlAction = $(this).dialog('option', 'urlAction');
                var scope = $(this).dialog('option', 'scope');
                var container = $(this).dialog('option', 'container');
                var selectedItems = $(this).dialog('option', 'selectedItems');
                changeStatusOfProject(scope+'ProjectsWithStatusCompleted', scope, container, urlAction, selectedItems);
                $(this).dialog('close');
            },
            Cancel: function() {
                $(this).dialog('close');
            }
        },
        itemId: '',
        urlAction: '',
        scope: '',
        selectedItems: '',
        container: ''
    });
});

var showFailedResponseDialog = function() {
    $("#AlertFailedResponseDialog").dialog('open');
};

var showAlertSelectItemDialog = function() {
    $("#AlertSelectItemDialog").dialog('open');
};

var showAlertNoFoundCalendarDialog = function() {
$("#AlertNoFoundCalendarDialog").dialog('open');
};


var showReSendEmailToApprovedDialog = function() {
    $("#ReSendEmailToApprovedDialog").dialog('open');
};
var showIncorrectProcessDialog = function() {
$("#IncorrectProcessDialog").dialog('open');
};
var showCreateObjectDialog = function() {
    $("#CreateObjectDialog").dialog('open');
};

var showLoadingDialog = function() {

    $.blockUI({ message: $('#LoadingDialog'),
        css: { width: '20%', margin: 'auto'}
    });
};

var showLoadingDialogForSection = function(sectionId) {

    $(sectionId).block({ message: $('#LoadingDialog').html(),
        css: { margin: 'auto'}
    });
};

var hideLoadingDialog = function() {
    $.unblockUI();
};

var hideLoadingDialogForSection = function(sectionId) {

    $(sectionId).unblock();

};

var showSendResponseMessageDialog = function() {
    $("#SendResponseMessageDialog").dialog('open');
};
var showPublishConfirmationDialog = function(scopeSection, containerSection, url, browse) {
    var gridId = '#' + browse + 'ListTable';
    var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
    var selectedItems;

    if (multiselect) {
        selectedItems = $(gridId).getGridParam("selarrrow");
        selectedItems = selectedItems.join(':');
    } else {
        selectedItems = $(gridId).getGridParam('selrow');
    }
    if (checkNullString(selectedItems) == '') {
        showAlertSelectItemDialog();
        return;
    }
    $("#PublishConfirmationDialog").dialog('option', 'scope', scopeSection);
    $("#PublishConfirmationDialog").dialog('option', 'container', containerSection);
    $("#PublishConfirmationDialog").dialog('option', 'urlAction', url);
    $("#PublishConfirmationDialog").dialog('option', 'selectedItems', selectedItems);
    $("#PublishConfirmationDialog").dialog('open');
};

var browsePopup;
    var popupParameters;
    var addUser = function(url, scope, entity, browseId, browsePopupId, headerType, detailFilter) {
        var gridId = browseId;
        var isDetail = isDetailOperation(detailFilter);
        $('#' + gridId + 'ListTable').resetSelection();
        parentUrl = url;
        parentScope = '';
        parentEntity = entity;
        parentBrowseId = browseId;
        parentIsDetail = isDetail;
        parentHeaderType = headerType;
        parentDetailFilter = detailFilter;
        popupParameters = '?BrowseName=';
        popupParameters += browsePopupId;
        blockInterface();
        browsePopup = window.open(urlBrowsePopup + popupParameters, "BrowsePopup", "width=700,height=500");

        if (window.focus) {
            browsePopup.focus();
        }
    };

    var removeUsers = function(entity, scope, container, urlAction, browseId) {
        var browseName = entity + 'All';
        var gridId = '#' + browseId + 'ListTable';
        var targetSection = container;
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }

        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        var deleteConfirmDialog = $("#RemoveUsersConfirmDialog").dialog('option', 'itemId', IDsSelected).dialog('option', 'urlAction', urlAction);
        deleteConfirmDialog.dialog('option', 'targetDelete', gridId);
        deleteConfirmDialog.dialog('option', 'targetSection', targetSection);
        deleteConfirmDialog.dialog('option', 'containerSection', container);
        deleteConfirmDialog.dialog('open');
    }
    var MessageBackEndDialog = function(dialogTitle, dialogContent) {
        var messageDialog = $("#MessageBackEndDialog");
        messageDialog.dialog({ autoOpen: false,
            title: dialogTitle,
            width: 360,
            modal: true,
            buttons: { "Ok": function() {
                $(this).dialog("destroy");
            }
            }
        });
        messageDialog.html(dialogContent);
        messageDialog.dialog("open");
    };