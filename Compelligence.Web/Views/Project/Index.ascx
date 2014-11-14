<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var displayCommentButton=false;
    var ddpc = '<%= ViewData["DefaultsDisabPublComm"] %>';
    if(ddpc=='true' || ddpc=='True'|| ddpc=='TRUE'){displayCommentButton=true;}
    $(function() {
        ProjectSubtabs = new Ext.TabPanel({
            renderTo: 'ProjectContent',
            //tabPosition: 'bottom',
            autoWidth: true,
            frame: true,
            id: 'ProjectTabPanel',
            defaults: { autoHeight: true },
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                    if (displayCommentButton) {
                        RemoveSubTabOfTabPanel('ProjectTabPanel', '<%= ViewData["Scope"] %>ProjectCommentContent');
                    }
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>ProjectEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>ProjectEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Header";
                            getEntityToRefresh('<%= Url.Action("Edit", "Project") %>', '<%= ViewData["Scope"] %>', 'Project', getIdBySelectedRow('<%= ViewData["Scope"] %>', 'ProjectAll'), 'ProjectAll', '#ProjectContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectFileContent', title: 'Files', id: '<%= ViewData["Scope"] %>ProjectFileContent', height: 100,
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Files";
                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.File %>', '#<%= ViewData["Scope"] %>ProjectFileContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectObjectiveContent', title: 'Objective', id: '<%= ViewData["Scope"] %>ProjectObjectiveContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Objective";
                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Objective %>', '#<%= ViewData["Scope"] %>ProjectObjectiveContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectKitContent', title: 'KITs', id: '<%= ViewData["Scope"] %>ProjectKitContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Kit";
                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Kit %>', '#<%= ViewData["Scope"] %>ProjectKitContent');
                        }
                        }
                    },

                    { contentEl: '<%= ViewData["Scope"] %>ProjectTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>ProjectTeamContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>ProjectTeamContent');
                        }
                        }
                    },
            //---------- User

                                        {contentEl: '<%= ViewData["Scope"] %>ProjectUserContent', title: 'User', id: '<%= ViewData["Scope"] %>ProjectUserContent',
                                        listeners: { activate: function() {
                                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > User";
                                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>ProjectUserContent');
                                        }
                                        }
                                    },
            //----------
                    {contentEl: '<%= ViewData["Scope"] %>ProjectLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>ProjectLibraryContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Library";
                        loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>ProjectLibraryContent');
                    }
                    }
                },

            /*    { contentEl: '<%= ViewData["Scope"] %>ProjectLabelContent', title: 'Labels', id: '<%= ViewData["Scope"] %>ProjectLabelContent',
            listeners: { activate: function() {
            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Label";
            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Label %>', '#<%= ViewData["Scope"] %>ProjectLabelContent');
            }
            }
            },*/

                    {contentEl: '<%= ViewData["Scope"] %>ProjectDiscussionContent',
                    title: 'Discussion',
                    id: '<%= ViewData["Scope"] %>ProjectDiscussionContent',
                    listeners: {
                        activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Discussion";

                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                                        '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>ProjectDiscussionContent');
                        }

                    }
                }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>ProjectCommentContent',
                        title: 'Comment',
                        id: '<%= ViewData["Scope"] %>ProjectCommentContent',
                        disabled: displayCommentButton,
                        listeners: {
                            activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Comment";
                                loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>ProjectCommentContent');
                            }

                        }
                    }
                    ,

                    { contentEl: '<%= ViewData["Scope"] %>ProjectFeedbackContent',
                        title: 'FeedBack',
                        id: '<%= ViewData["Scope"] %>ProjectFeedbackContent',
                        listeners: {
                            activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Feedbacks";
                                loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                                '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Feedback %>', '#<%= ViewData["Scope"] %>ProjectFeedbackContent');
                            }
                        }

                    },
                    { contentEl: '<%= ViewData["Scope"] %>ProjectApprovalListContent',
                        title: 'Approval List',
                        id: '<%= ViewData["Scope"] %>ProjectApprovalListContent',
                        hidden: 'true',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Approval List";
                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.ApprovalList %>', '#<%= ViewData["Scope"] %>ProjectApprovalListContent');
                        }
                        }
                    },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>ProjectBudgetContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Budget";
                        loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>ProjectBudgetContent');
                    }
                    }
                },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>ProjectPlanContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Plan";
                        loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>ProjectPlanContent');
                    }
                    }
                },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectMetricContent',
                    title: 'Metric',
                    id: '<%= ViewData["Scope"] %>ProjectMetricContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Metric";
                        loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>ProjectMetricContent');
                    }
                    }
                },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>ProjectSourceContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Source";
                        loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>ProjectSourceContent');
                    }
                    }
                },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProjectEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>ProjectEntityRelationContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > Related";
                        loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>ProjectEntityRelationContent');
                    }
                    }
                }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>ProjectActionHistoryContent', title: 'History', id: '<%= ViewData["Scope"] %>ProjectActionHistoryContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Project</u> > History";
                            loadDetailList('<%= Url.Action("GetDetails", "Project") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Project'),
                            '<%= ViewData["Scope"] %>Project', '<%= (int) DetailType.ActionHistory %>', '#<%= ViewData["Scope"] %>ProjectActionHistoryContent');
                        }
                        }
                    }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>ProjectList');
    });
    var showInactiveProjects = function(invoke) {
        if (invoke.checked) {
            loadContent('<%= Url.Action("GetInactiveProjects", "Project") %>', '#ProjectContent', "Workspace");
        }
        else {
            loadContent('<%= Url.Action("Index", "Project") %>', '#ProjectContent', "Workspace");
        }
    };
    
</script>



<asp:Panel ID="ProjectListContent" runat="server">    
    <div id="<%= ViewData["Scope"] %>ProjectList" class="indexOne">
        <% Html.RenderPartial("List"); %>        
    </div>
    <div class="resizeS"></div>
</asp:Panel>
<br />

<asp:Panel ID="ProjectFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProjectEditFormContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectFileContent" class="x-hide-display heightSubPanels"></div>
    <div id="<%= ViewData["Scope"] %>ProjectObjectiveContent" class="x-hide-display heightSubPanels"></div>
    <div id="<%= ViewData["Scope"] %>ProjectKitContent" class="x-hide-display heightSubPanels"></div>
    <div id="<%= ViewData["Scope"] %>ProjectTeamContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectUserContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectBudgetContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectLibraryContent" class="x-hide-display heightSubPanels"></div>
    <div id="<%= ViewData["Scope"] %>ProjectPlanContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectLabelContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectMetricContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectDiscussionContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectCommentContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectFeedbackContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectApprovalListContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectSourceContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectEntityRelationContent" class="x-hide-display heightSubPanels" ></div>
    <div id="<%= ViewData["Scope"] %>ProjectActionHistoryContent" class="x-hide-display heightSubPanels" ></div>
    
</asp:Panel>

