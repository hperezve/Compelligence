<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var displayCommentButton = false;
    var ddpc = '<%= ViewData["DefaultsDisabPublComm"] %>';
    if (ddpc == 'true' || ddpc == 'True' || ddpc == 'TRUE') { displayCommentButton = true; }
    $(function() {
        EventSubtabs = new Ext.TabPanel({
            renderTo: 'EventContent',
            autoWidth: true,
            frame: true,
            id: 'EventTabPanel',
            //defaults:{autoHeight: true},
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                    if (displayCommentButton) {
                        RemoveSubTabOfTabPanel('EventTabPanel', '<%= ViewData["Scope"] %>EventCommentContent');
                    }
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>EventEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>EventEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Header";
                            var currentId = getIdValue('<%= ViewData["Scope"] %>', 'Event');
                            if (currentId == null) {
                                currentId = getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event');
                                if (currentId != null) {
                                    getEntity('<%= Url.Action("Edit", "Event") %>', '<%= ViewData["Scope"] %>', 'Event', currentId, 'EventAll', '#EventContent');
                                }
                            }
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>EventTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>EventTeamContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>EventTeamContent');
                        }
                        }
                    },
            //User
                     {contentEl: '<%= ViewData["Scope"] %>EventUserContent', title: 'User', id: '<%= ViewData["Scope"] %>EventUserContent',
                     listeners: { activate: function() {
                         document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > User";
                         loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>EventUserContent');
                     }
                     }
                 },
            //EndUser
                    {contentEl: '<%= ViewData["Scope"] %>EventImplicationContent', title: 'Implication', id: '<%= ViewData["Scope"] %>EventImplicationContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Implication";
                        loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Implication %>', '#<%= ViewData["Scope"] %>EventImplicationContent');
                    }
                    }
                },
                    { contentEl: '<%= ViewData["Scope"] %>EventPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>EventPlanContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Plan";
                            loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>EventPlanContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>EventLabelContent', title: 'Labels', id: '<%= ViewData["Scope"] %>EventLabelContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Labels";
                            loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Label %>', '#<%= ViewData["Scope"] %>EventLabelContent');
                        }
                        }
                    },

                     { contentEl: '<%= ViewData["Scope"] %>EventDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>EventDiscussionContent',
                         listeners: { activate: function() {
                             document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Discussion";
                             loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>EventDiscussionContent');
                         }
                         }
                     },

                    { contentEl: '<%= ViewData["Scope"] %>EventCommentContent',
                        title: 'Comment',
                        id: '<%= ViewData["Scope"] %>EventCommentContent',
                        disabled: displayCommentButton,
                        listeners: {
                            activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Event</u> > Comment";
                                loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                        '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>EventCommentContent');
                            }
                        }
                    },
                         { contentEl: '<%= ViewData["Scope"] %>EventFeedBackContent',
                             title: 'FeedBacks',
                             id: '<%= ViewData["Scope"] %>EventFeedbackContent',
                             listeners: {
                                 activate: function() {
                                     document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Event</u> > Feedbacks";
                                     loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                                '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Feedback %>', '#<%= ViewData["Scope"] %>EventFeedbackContent');
                                 }
                             }
                         },
                        { contentEl: '<%= ViewData["Scope"] %>EventSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>EventSourceContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Source";
                                loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>EventSourceContent');
                            }
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>EventEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>EventEntityRelationContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Related";
                                loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>EventEntityRelationContent');
                            }
                            }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>EventLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>EventLibraryContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>EventLibraryContent');
                        }
                        }
                    },
                        { contentEl: '<%= ViewData["Scope"] %>EventTrendContent', title: 'Trend', id: '<%= ViewData["Scope"] %>EventTrendContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Events</u> > Trend";
                                loadDetailList('<%= Url.Action("GetDetails", "Event") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Event'),
                            '<%= ViewData["Scope"] %>Event', '<%= (int) DetailType.Trend %>', '#<%= ViewData["Scope"] %>EventTrendContent');
                            }
                            }
                        }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>EventList');
        // $('#<%= ViewData["Scope"] %>EventEditFormContent').css({ 'height': '100px' });
    });
    var loadEvents = function() {
    if ($('#AllEventCheckbox').prop('checked')) {
        
        loadContent('<%= Url.Action("GetPastEvents", "Event") %>', '#EventContent', "Workspace");
    } else {
        
        loadContent('<%= Url.Action("Index", "Event") %>', '#EventContent', "Workspace");
        
    }

    };
</script>

<asp:Panel ID="EventListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>EventList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS">
        <img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>" /></div>
</asp:Panel>
<br />
<asp:Panel ID="EventFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>EventEditFormContent" class="x-hide-display" />
    <%--<div id="<%= ViewData["Scope"] %>EventIndustryContent" class="x-hide-display" />--%>
    <div id="<%= ViewData["Scope"] %>EventTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventImplicationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventPlanContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventLabelContent" class="x-hide-display" />
    <%--<div id="<%= ViewData["Scope"] %>ProjectDiscussionContent" class="x-hide-display" />--%>
    <div id="<%= ViewData["Scope"] %>EventDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventCommentContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventFeedBackContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>EventTrendContent" class="x-hide-display" />
</asp:Panel>
