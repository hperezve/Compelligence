<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var browsePopup;
    var popupParameters;
    var addUser = function(url, scope, entity, browseId, browsePopupId, headerType, detailFilter) {
        var gridId = browseId;
        var isDetail = isDetailOperation(detailFilter);
        $('#' + gridId + 'ListTable').resetSelection();
        parentUrl = url;
        parentScope = '';
        parentEntity = 'Configuration';
        parentBrowseId = 'UserProfileAll';
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

    var removeUsers = function(entity, scope, container, urlAction) {
        var browseName = entity + 'All';
        var gridId = '#UserProfileAllListTable';
        var targetSection = '#UsersToReceiveTopContent';
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
    };
</script>
<script language="text/javascript">
    Ext.onReady(function() {
        var tabs = new Ext.TabPanel({
            renderTo: 'tabsContainer',
            autoWidth: true,
            heigth: 688,
            activeTab: 0,
            frame: true,
            defaults: { autoHeigth: true },
            items: [
                { contentEl: 'ProjectsApprovalContent', title: 'Approve Projects',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Configuration Projects</u>";
                        loadContent('<%= Url.Action("IndexApproval","Configuration") %>', '#ProjectsApprovalContent', 'Admin');
                    }
                    }
                },
                { contentEl: 'NewsSendEmailContent', title: 'Email',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Configuration News Mail</u>";
                        loadContent('<%= Url.Action("Test","Configuration") %>', '#NewsSendEmailContent', 'Admin');
                    }
                    }
                },
                { contentEl: 'UsersToReceiveTopContent', title: 'Users to Receive News',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Users to Receive News</u>";
                        loadContent('<%= Url.Action("UserToReceiveTop","Configuration") %>', '#UsersToReceiveTopContent', 'Admin');
                    }
                    }
                },
                { contentEl: 'DefaultsContent', title: 'Defaults',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Configuration Defaults</u>";
                        loadContent('<%= Url.Action("IndexDefault","Configuration") %>', '#DefaultsContent', 'Admin');
                    }
                    }
                },
                { contentEl: 'LabelsContent', title: 'Labels',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Configuration Labels</u>";
                        loadContent('<%= Url.Action("Index","ConfigurationLabels") %>', '#LabelsContent', 'Admin');
                    }
                    }
                }
            ]
        });
    });
</script>

<script type="text/javascript">
    var ProjectsApprovalSubtabs;
    var NewsSendEmailSubtabs;
    var UsersToReceiveTopSubTabs;
    var DefaultsSubTabs;
    var LabelsSubTabs;
</script>

<div id="tabsContainer">
    <div id="ProjectsApprovalContent" class="x-hide-display heightPanel">
    </div>
    <div id="NewsSendEmailContent" class="x-hide-display heightPanel">
    </div>
    <div id="UsersToReceiveTopContent" class="x-hide-display heightPanel">
    </div>
    <div id="DefaultsContent" class="x-hide-display heightPanel">
    </div>
    <div id="LabelsContent" class="x-hide-display heightPanel">
    </div>
</div>

