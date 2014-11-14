<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">


    $(function() {
    ProjectArchiveSubtabs = new Ext.TabPanel({
        renderTo: 'ProjectArchiveContent',
            //tabPosition: 'bottom',
            autoWidth: true,
            frame: true,
            defaults: { autoHeight: true },
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>ProjectArchiveEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>ProjectArchiveEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Header";
                        getEntityToRefresh('<%= Url.Action("Edit", "ProjectArchive") %>', '<%= ViewData["Scope"] %>', 'ProjectArchive', getIdBySelectedRow('<%= ViewData["Scope"] %>', 'ProjectArchive'), 'ProjectArchive', '#ProjectArchiveContent');
                        }
                        }
                    }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>ProjectFileContent', title: 'Files', id: '<%= ViewData["Scope"] %>ProjectFileContent', height: 100,
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Files";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.File %>', '#<%= ViewData["Scope"] %>ProjectFileContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectObjectiveContent', title: 'Objective', id: '<%= ViewData["Scope"] %>ProjectObjectiveContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Objective";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Objective %>', '#<%= ViewData["Scope"] %>ProjectObjectiveContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectKitContent', title: 'KIT', id: '<%= ViewData["Scope"] %>ProjectKitContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Kit";
                            loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Kit %>', '#<%= ViewData["Scope"] %>ProjectKitContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>ProjectTeamContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>ProjectTeamContent');
                        }
                        }
                    },
                    //User
                    {contentEl: '<%= ViewData["Scope"] %>ProjectUserContent', title: 'Users', id: '<%= ViewData["Scope"] %>ProjectUserContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > User";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>ProjectUserContent');
                    }
                    }
                },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>ProjectLibraryContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Library";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>ProjectLibraryContent');
                        }
                        }
                    },

                    { contentEl: '<%= ViewData["Scope"] %>ProjectLabelContent', title: 'Labels', id: '<%= ViewData["Scope"] %>ProjectLabelContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Label";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Label %>', '#<%= ViewData["Scope"] %>ProjectLabelContent');
                        }
                        }
                    },
                    
                    { contentEl: '<%= ViewData["Scope"] %>ProjectDiscussionContent',
                        title: 'Discussion',
                        id: '<%= ViewData["Scope"] %>ProjectDiscussionContent',
                        listeners: {
                        activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Discussion";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>ProjectDiscussionContent');
                            }

                        }
                    }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>ProjectCommentContent',
                        title: 'Comment',
                        id: '<%= ViewData["Scope"] %>ProjectCommentContent',
                        listeners: {
                            activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Comment";
                            loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>ProjectCommentContent');
                            }

                        }
                    }
                    ,

                    { contentEl: '<%= ViewData["Scope"] %>ProjectFeedbackContent',
                        title: 'FeedBacks',
                        id: '<%= ViewData["Scope"] %>ProjectFeedbackContent',
                        listeners: {
                            activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Feedbacks";
                            loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                                '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Feedback %>', '#<%= ViewData["Scope"] %>ProjectFeedbackContent');
                            }
                        }

                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectApprovalListContent', title: 'Approval List', id: '<%= ViewData["Scope"] %>ProjectApprovalListContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Approval List";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.ApprovalList %>', '#<%= ViewData["Scope"] %>ProjectApprovalListContent');
                        }
                        }
                    },
                    //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>ProjectBudgetContent',
                    listeners: { activate: function() {
                    document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Budget";
                    loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>ProjectBudgetContent');
                    }
                    }
                    },
                    //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>ProjectPlanContent',
                    listeners: { activate: function() {
                    document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Plan";
                    loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>ProjectPlanContent');
                    }
                    }
                    },
                    //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectMetricContent',
                    title: 'Metric',
                    id: '<%= ViewData["Scope"] %>ProjectMetricContent',
                    listeners: { activate: function() {
                    document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Metric";
                    loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>ProjectMetricContent');
                    }
                    }
                    },                    
                    //(Small)
                    { contentEl: '<%= ViewData["Scope"] %>ProjectSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>ProjectSourceContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Source";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>ProjectSourceContent');
                        }
                        }
                    },
                    //(Small)
                    { contentEl: '<%= ViewData["Scope"] %>ProjectEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>ProjectEntityRelationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > Related";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>ProjectEntityRelationContent');
                        } 
                        }
                    }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>ProjectActionHistoryContent', title: 'History', id: '<%= ViewData["Scope"] %>ProjectActionHistoryContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Project Archive</u> > History";
                        loadDetailList('<%= Url.Action("GetDetails", "ProjectArchive") %>', getIdValue('<%= ViewData["Scope"] %>', 'ProjectArchive'),
                            '<%= ViewData["Scope"] %>ProjectArchive', '<%= (int) DetailType.ActionHistory %>', '#<%= ViewData["Scope"] %>ProjectActionHistoryContent');
                        }
                        }
                    }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>ProjectArchiveList');
    });
//    var showInactiveProjects = function(invoke) {
//        if (invoke.checked) {
//            loadContent('<%= Url.Action("GetInactiveProjects", "ProjectArchive") %>', '#ProjectArchiveContent', "Tools");
//        }
//        else {
//            loadContent('<%= Url.Action("Index", "ProjectArchive") %>', '#ProjectArchiveContent', "Tools");
//        }
//    };
</script>



<asp:Panel ID="ProjectArchiveListContent" runat="server">    
    <div id="<%= ViewData["Scope"] %>ProjectArchiveList" class="indexOne">
        <% Html.RenderPartial("List"); %>        
    </div>
    
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />

<asp:Panel ID="ProjectArchiveFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProjectArchiveEditFormContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectFileContent" class="x-hide-display heightSubPanels"/>
    <div id="<%= ViewData["Scope"] %>ProjectObjectiveContent" class="x-hide-display heightSubPanels"/>
    <div id="<%= ViewData["Scope"] %>ProjectKitContent" class="x-hide-display heightSubPanels"></div>
    <div id="<%= ViewData["Scope"] %>ProjectTeamContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectUserContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectBudgetContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectLibraryContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectPlanContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectLabelContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectMetricContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectDiscussionContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectCommentContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectFeedbackContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectApprovalListContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectSourceContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectEntityRelationContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ProjectActionHistoryContent" class="x-hide-display heightSubPanels" />
</asp:Panel>

