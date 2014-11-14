<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var displayCommentButton = false;
    var ddpc = '<%= ViewData["DefaultsDisabPublComm"] %>';
    if (ddpc == 'true' || ddpc == 'True' || ddpc == 'TRUE') { displayCommentButton = true; }
    $(function() {
        DealSubtabs = new Ext.TabPanel({
            renderTo: 'DealContent',
            autoWidth: true,
            frame: true,
            id: 'DealTabPanel',
            //defaults: { autoHeight: true },
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                    if (displayCommentButton) {
                        RemoveSubTabOfTabPanel('DealTabPanel', '<%= ViewData["Scope"] %>DealCommentContent');
                    }
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>DealEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>DealEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Header";
                            var currentId = getIdValue('<%= ViewData["Scope"] %>', 'Deal');
                            if (currentId == null) {
                                currentId = getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal;')
                                if (currentId != null) {
                                    getEntity('<%= Url.Action("Edit", "Deal") %>', '<%= ViewData["Scope"] %>', 'Deal', currentId, 'DealAll', '#DealContent');
                                }
                            }
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DealTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>DealTeamContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                            '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>DealTeamContent');
                        }
                        }
                    },
                        { contentEl: '<%= ViewData["Scope"] %>DealUserContent', title: 'User', id: '<%= ViewData["Scope"] %>DealUserContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > User";
                                loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                            '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>DealUserContent');
                            }
                            }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>DealImplicationContent', title: 'Implications', id: '<%= ViewData["Scope"] %>DealImplicationContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Implications";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                            '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Implication %>', '#<%= ViewData["Scope"] %>DealImplicationContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DealMetricContent', title: 'Metrics', id: '<%= ViewData["Scope"] %>DealMetricContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Metric";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                            '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>DealMetricContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DealDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>DealDiscussionContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Discussion";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                        '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>DealDiscussionContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DealCommentContent',
                        title: 'Comment',
                        id: '<%= ViewData["Scope"] %>DealCommentContent',
                        disabled: displayCommentButton,
                        listeners: {
                            activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Comment";
                                loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                        '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>DealCommentContent');
                            }
                        }
                    }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>DealSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>DealSourceContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Source";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                        '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>DealSourceContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DealEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>DealEntityRelationContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Related";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                        '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>DealEntityRelationContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DealLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>DealLibraryContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                        '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>DealLibraryContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DealPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>DealPlanContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Deal</u> > Plan";
                            loadDetailList('<%= Url.Action("GetDetails", "Deal") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Deal'),
                            '<%= ViewData["Scope"] %>Deal', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>DealPlanContent');
                        }
                        }
                    }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>DealList');
    });
    var showArchived = function(invoke) {
        if (invoke.checked) {
            loadContent('<%= Url.Action("GetInactiveDeal", "Deal") %>', '#DealContent', "Workspace");
        }
        else {
            loadContent('<%= Url.Action("Index", "Deal") %>', '#DealContent', "Workspace");
        }
    };
</script>

<asp:Panel ID="DealListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>DealList" class="indexOne">
    <% Html.RenderPartial("List"); %>
    </div>    
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="DealFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>DealEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealImplicationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealMetricContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealCommentContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealPlanContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>DealClosedContent" class="x-hide-display" />
</asp:Panel>
