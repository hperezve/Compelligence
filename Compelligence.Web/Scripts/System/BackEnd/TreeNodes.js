var disableItems = function(id) {
    $(id).unbind('click', id.click);
}

var enabled_disabled = function() {
    $('#tree_Workspace').click(); $('#tree_Workspace').click();
    $('#tree_Environment').click(); $('#tree_Environment').click();
    $('#tree_Tools').click(); $('#tree_Tools').click();
    $('#tree_Research').click(); $('#tree_Research').click();
    $('#tree_Report').click(); $('#tree_Report').click();
    $('#tree_Admin').click(); $('#tree_Admin').click();
}

var enableClassSelected = function(id) {
    $(id).addClass("onSelectedItem");
}

var disableAllClassSelected = function() {
    
    //var disableAllClassSelected = function(id) {
    //$(id).removeClass("onSelectedItem");

    $('#tree_Workspace_Dashboard').removeClass("onSelectedItem");
    $('#tree_Workspace_Project').removeClass("onSelectedItem");
    $('#tree_Workspace_Reports').removeClass("onSelectedItem");
    $('#tree_Workspace_Deal').removeClass("onSelectedItem");
    $('#tree_Workspace_Event').removeClass("onSelectedItem");
    $('#tree_Workspace_Feedback').removeClass("onSelectedItem");
    $('#tree_Workspace_Survey').removeClass("onSelectedItem");
    $('#tree_Workspace_Calendar').removeClass("onSelectedItem");
    $('#tree_Workspace_Newsletter').removeClass("onSelectedItem");
    
    $('#tree_Environment_Objetives').removeClass("onSelectedItem");
    $('#tree_Environment_Kits').removeClass("onSelectedItem");
    $('#tree_Environment_Industries').removeClass("onSelectedItem");
    $('#tree_Environment_Competitors').removeClass("onSelectedItem");
    $('#tree_Environment_Products').removeClass("onSelectedItem");
    $('#tree_Environment_Customers').removeClass("onSelectedItem");
    $('#tree_Environment_Suppliers').removeClass("onSelectedItem");
    $('#tree_Environment_Partners').removeClass("onSelectedItem");
    $('#tree_Environment_Libraries').removeClass("onSelectedItem");
    $('#tree_Environment_Comparinator').removeClass("onSelectedItem");
//    $('#tree_Environment_CriteriaGroup').removeClass("onSelectedItem");
//    $('#tree_Environment_CriteriaSet').removeClass("onSelectedItem");
//    $('#tree_Environment_Criteria').removeClass("onSelectedItem");

    $('#tree_Tools_WinLoss').removeClass("onSelectedItem");
    $('#tree_Tools_Nugget').removeClass("onSelectedItem");

    $('#tree_Research_Source').removeClass("onSelectedItem");

    $('#tree_Report_Workspace').removeClass("onSelectedItem");
    $('#tree_Report_Environment').removeClass("onSelectedItem");
    $('#tree_Report_Admin').removeClass("onSelectedItem");

    $('#tree_Admin_Users').removeClass("onSelectedItem");
    $('#tree_Admin_Teams').removeClass("onSelectedItem");
    $('#tree_Admin_Templates').removeClass("onSelectedItem");
    $('#tree_Admin_Website').removeClass("onSelectedItem");
    $('#tree_Admin_Workflows').removeClass("onSelectedItem");
    $('#tree_Admin_Content').removeClass("onSelectedItem");
    $('#tree_Admin_LibraryType').removeClass("onSelectedItem");
    $('#tree_Admin_LibraryExternalSource').removeClass("onSelectedItem");
    }

var refreshItems = function(id) {
    disableAllClassSelected();
    $(id).addClass("onSelectedItem");
}

var workspaceRefresh = function(id) {
    //WorkspaceSubtabs.setActiveTab(id);
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_DashboardContent'); //$('#tree_workspace_dashboard').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_ProjectContent'); //$('#tree_workspace_Projects').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_DealContent'); //$('#tree_workspace_DealSupport').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_EventContent'); //$('#tree_workspace_Events').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_SurveyContent'); //$('#tree_workspace_Survey').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_CalendarContent'); //$('#tree_workspace_Calendar').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_NewsletterContent'); //$('#tree_workspace_Newsletter').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_WinLossAnalysisContent'); //$('#tree_workspace_WinLossAnalysis').click();
    WorkspaceSubtabs.setActiveTab('WorkspaceTab_ActionHistoryContent'); //$('#tree_workspace_WinLossAnalysis').click();
}
var environmentRefresh = function() {
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_ObjectiveContent'); //$('#tree_Environment_Objetives').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_KitContent'); //$('#tree_Environment_Kits').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_IndustryContent');// $('#tree_Environment_Industries').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CompetitorContent');// $('#tree_Environment_Competitors').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_ProductContent');// $('#tree_Environment_Products').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CustomerContent');// $('#tree_Environment_Customers').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_SupplierContent');// $('#tree_Environment_Suppliers').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_PartnerContent');// $('#tree_Environment_Partners').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_LibraryContent');// $('#tree_Environment_Libraries').click();
    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriasContent');// $('#tree_Environment_Comparinator').click();
//    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaGroupContent');// $('#tree_Environment_CriteriaGroup').click();
//    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaSetContent');// $('#tree_Environment_CriteriaSet').click();
//    EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaContent');// $('#tree_Environment_Criteria').click();
}

var toolsRefresh = function() {
    ToolsSubtabs.setActiveTab('ToolsTab_WinLossContent');//$('#tree_Tools_WinLoss').click();
    ToolsSubtabs.setActiveTab('ToolsTab_NuggetContent');// $('#tree_Tools_Nugget').click();
}

var researchRefresh = function() {
    $('#tree_Research_Source').click();
}

var reportRefresh = function() {
    ReportsSubtabs.setActiveTab('ReportsTab_ReportsWorkspaceContent'); //$('#tree_Workspace_Admin').click();
    ReportsSubtabs.setActiveTab('ReportsTab_ReportsEnvironmentContent'); //$('#tree_Report_Environment').click();
    ReportsSubtabs.setActiveTab('ReportsTab_ReportsAdminContent'); //$('#tree_Report_Admin').click();
    
}

var adminRefresh = function() {
    AdminSubtabs.setActiveTab('AdminTab_UserContent'); //$('#tree_Admin_Users').click();
    AdminSubtabs.setActiveTab('AdminTab_TeamContent'); //$('#tree_Admin_Teams').click();
    AdminSubtabs.setActiveTab('AdminTab_TemplateContent');// $('#tree_Admin_Templates').click();
    AdminSubtabs.setActiveTab('AdminTab_WebsiteContent');// $('#tree_Admin_Website').click();
    AdminSubtabs.setActiveTab('AdminTab_WorkflowContent');// $('#tree_Admin_Workflows').click();
    AdminSubtabs.setActiveTab('AdminTab_ContentTypeContent');// $('#tree_Admin_Content').click();
    AdminSubtabs.setActiveTab('AdminTab_LibraryTypeContent');// $('#tree_Admin_LibraryType').click();
    AdminSubtabs.setActiveTab('AdminTab_LibraryExternalSourceContent'); // $('#tree_Admin_LibraryExternalSource').click();
    AdminSubtabs.setActiveTab('AdminTab_CustomFieldContent'); // $('#tree_Admin_CustomField').click();
}

$(function() {
    //$("#TreeviewMenu").treeview();

    $('#tree_Workspace').click(function() {

        BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
        $('#tree_Workspace_Dashboard').click(function() {
            workspaceRefresh();
            enabled_disabled();
            refreshItems('#tree_Workspace_Dashboard');
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_DashboardContent');
            disableItems('#tree_Workspace_Dashboard');
        });

        $('#tree_Workspace_Project').click(function() {
            workspaceRefresh();
            enabled_disabled();
            refreshItems('#tree_Workspace_Project');
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_ProjectContent');
            disableItems('#tree_Workspace_Project');
        });

        $('#tree_Workspace_Deal').click(function() {
            workspaceRefresh();
            enabled_disabled();
            refreshItems('#tree_Workspace_Deal');
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_DealContent');
            disableItems('#tree_Workspace_Deal');
        });

        $('#tree_Workspace_Event').click(function() {
            workspaceRefresh();
            enabled_disabled();
            refreshItems('#tree_Workspace_Event');
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_EventContent');
            disableItems('#tree_Workspace_Event');
        });

        $('#tree_Workspace_Survey').click(function() {
            workspaceRefresh();
            enabled_disabled();
            refreshItems('#tree_Workspace_Survey');
            //$('#tree_workspace_Survey').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_SurveyContent');
            disableItems('#tree_Workspace_Survey');
        });

        $('#tree_Workspace_Calendar').click(function() {
        workspaceRefresh();
            enabled_disabled();
            refreshItems('#tree_Workspace_Calendar');
            //$('#tree_workspace_Calendar').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_CalendarContent');
            disableItems('#tree_Workspace_Calendar');
        });
        $('#tree_Workspace_Newsletter').click(function() {
            workspaceRefresh();
            enabled_disabled();
            refreshItems('#tree_Workspace_Newsletter');
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_NewsletterContent');
            disableItems('#tree_Workspace_Newsletter');
        });
                
    });
    $('#tree_Environment').click(function() {
        BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');

        $('#tree_Environment_Objetives').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Objetives');
            //$('#tree_Environment_Objetives').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_ObjectiveContent');
            disableItems('#tree_Environment_Objetives');
        });

        $('#tree_Environment_Kits').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Kits');
            //$('#tree_Environment_Kits').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_KitContent');
            disableItems('#tree_Environment_Kits');
        });

        $('#tree_Environment_Industries').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Industries');
            //$('#tree_Environment_Industries').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_IndustryContent');
            disableItems('#tree_Environment_Industries');
        });
        $('#tree_Environment_Competitors').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Competitors');
            //$('#tree_Environment_Competitors').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_CompetitorContent');
            disableItems('#tree_Environment_Competitors');
        });
        $('#tree_Environment_Products').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Products');
            //$('#tree_Environment_Products').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_ProductContent');
            disableItems('#tree_Environment_Products');
        });
        $('#tree_Environment_Customers').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Customers');
            //$('#tree_Environment_Customers').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_CustomerContent');
            disableItems('#tree_Environment_Customers');
        });
        $('#tree_Environment_Suppliers').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Suppliers');
            //$('#tree_Environment_Suppliers').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_SupplierContent');
            disableItems('#tree_Environment_Suppliers');
        });
        $('#tree_Environment_Partners').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Partners');
            //$('#tree_Environment_Partners').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_PartnerContent');
            disableItems('#tree_Environment_Partners');
        });
        $('#tree_Environment_Libraries').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Libraries');
            //$('#tree_Environment_Libraries').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_LibraryContent');
            disableItems('#tree_Environment_Libraries');
        });
        $('#tree_Environment_Comparinator').click(function() {
            environmentRefresh();
            enabled_disabled();
            refreshItems('#tree_Environment_Comparinator');
            //$('#tree_Environment_Comparinator').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            EnvironmentSubtabs.setActiveTab('EnvironmentTab_ComparinatorContent');
            disableItems('#tree_Environment_Comparinator');
        });
//        $('#tree_Environment_CriteriaGroup').click(function() {
//            environmentRefresh();
//            enabled_disabled();
//            refreshItems('#tree_Environment_CriteriaGroup');
//            //$('#tree_Environment_CriteriaGroup').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
//            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
//            EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaGroupContent');
//            disableItems('#tree_Environment_CriteriaGroup');
//        });
//        $('#tree_Environment_CriteriaSet').click(function() {
//            environmentRefresh();
//            enabled_disabled();
//            refreshItems('#tree_Environment_CriteriaSet');
//            //$('#tree_Environment_CriteriaSet').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
//            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
//            EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaSetContent');
//            disableItems('#tree_Environment_CriteriaSet');
//        });
//        $('#tree_Environment_Criteria').click(function() {
//            environmentRefresh();
//            enabled_disabled();
//            refreshItems('#tree_Environment_Criteria');
//            //$('#tree_Environment_Criteria').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
//            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
//            EnvironmentSubtabs.setActiveTab('EnvironmentTab_CriteriaContent');
//            disableItems('#tree_Environment_Criteria');
//        });

    });

    $('#tree_Tools').click(function() {
        BackEndTabs.setActiveTab('AdminTabs_ToolsTab');

        $('#tree_Tools_WinLoss').click(function() {
            toolsRefresh();
            enabled_disabled();
            refreshItems('#tree_Tools_WinLoss');
            //$('#tree_Tools_WinLoss').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_ToolsTab');
            ToolsSubtabs.setActiveTab('ToolsTab_WinLossContent');
            disableItems('#tree_Tools_WinLoss');
        });
        $('#tree_Tools_Nugget').click(function() {
            toolsRefresh();
            enabled_disabled();
            refreshItems('#tree_Tools_Nugget');
            //$('#tree_Tools_Nugget').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_ToolsTab');
            ToolsSubtabs.setActiveTab('ToolsTab_NuggetContent');
            disableItems('#tree_Tools_Nugget');
        });
    });

    $('#tree_Research').click(function() {
        BackEndTabs.setActiveTab('AdminTabs_ResearchTab');
        $('#tree_Research_Source').click(function() {
            //researchRefresh();
            enabled_disabled();
            refreshItems('#tree_Research_Source');
            BackEndTabs.setActiveTab('AdminTabs_ResearchTab');
            ResearchSubtabs.setActiveTab('ResearchTab_SourceContent');
            disableItems('#tree_Research_Source');
            //$('#tree_Research_Source').unbind('click', ('#tree_Research_Source').click);
//            $('#tree_Research_Source').one('click', function() {
//                $(this).unbind('click', (this).click);
//            });
        });
    });

    $('#tree_Report').click(function() {
        BackEndTabs.setActiveTab('AdminTabs_ReportsTab');

        $('#tree_Report_Workspace').click(function() {
            //reportRefresh();
            enabled_disabled();
            refreshItems('#tree_Report_Workspace');
            BackEndTabs.setActiveTab('AdminTabs_ReportsTab');            
            ReportsSubtabs.setActiveTab('ReportsTab_ReportsWorkspaceContent');
            disableItems('#tree_Report_Workspace');
        });

        $('#tree_Report_Environment').click(function() {
            //reportRefresh();
            enabled_disabled();
            refreshItems('#tree_Report_Environment');
            BackEndTabs.setActiveTab('AdminTabs_ReportsTab');            
            ReportsSubtabs.setActiveTab('ReportsTab_ReportsEnvironmentContent');
            disableItems('#tree_Report_Environment');
        });
        $('#tree_Report_Admin').click(function() {
            //reportRefresh();
            enabled_disabled();
            refreshItems('#tree_Report_Admin');
            BackEndTabs.setActiveTab('AdminTabs_ReportsTab');            
            ReportsSubtabs.setActiveTab('ReportsTab_ReportsAdminContent');
            disableItems('#tree_Report_Admin');
        });

    });

    $('#tree_Admin').click(function() {
        BackEndTabs.setActiveTab('AdminTabs_AdminTab');

        $('#tree_Admin_Users').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_Users');
            //$('#tree_Admin_Users').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_UserContent');
            disableItems('#tree_Admin_Users');
        });
        $('#tree_Admin_Teams').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_Teams');
            //$('#tree_Admin_Teams').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_TeamContent');
            disableItems('#tree_Admin_Teams');
        });
        $('#tree_Admin_Templates').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_Templates');
            //$('#tree_Admin_Templates').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_TemplateContent');
            disableItems('#tree_Admin_Templates');
        });
        $('#tree_Admin_Website').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_Website');
            //$('#tree_Admin_Website').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_WebsiteContent');
            disableItems('#tree_Admin_Website');
        });
        $('#tree_Admin_Workflows').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_Workflows');
            //$('#tree_Admin_Workflows').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_WorkflowContent');
            disableItems('#tree_Admin_Workflows');
        });
        $('#tree_Admin_Content').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_Content');
            //$('#tree_Admin_Content').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_ContentTypeContent');
            disableItems('#tree_Admin_Content');
        });
        $('#tree_Admin_LibraryType').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_LibraryType');
            //$('#tree_Admin_LibraryType').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_LibraryTypeContent');
            disableItems('#tree_Admin_LibraryType');
        });
        $('#tree_Admin_LibraryExternalSource').click(function() {
            adminRefresh();
            enabled_disabled();
            refreshItems('#tree_Admin_LibraryExternalSource');
            //$('#tree_Admin_LibraryExternalSource').css({ 'color': '#990000', 'font-weight': 'bold', 'background-color': 'antiqueWhite', 'background': 'url(../content/images/treeview/folder_selected.gif) no-repeat scroll 0 0;' });
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            AdminSubtabs.setActiveTab('AdminTab_CustomFieldContent');
            disableItems('#tree_Admin_LibraryExternalSource');
        });

    });
    
    $('#tree_Search').click(function() {
        BackEndTabs.setActiveTab('AdminTabs_SearchTab');
    });
});