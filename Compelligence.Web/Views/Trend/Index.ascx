<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
	    $(function() {
	        TrendSubtabs = new Ext.TabPanel({
                renderTo: 'TrendContent',
                autoWidth:true,
                frame:true,
                //defaults:{autoHeight: true},
                height: 640,
                listeners: {  
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    {contentEl:'<%= ViewData["Scope"] %>TrendEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>TrendEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Header";
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>TrendIndustryContent', title: 'Industry', id: '<%= ViewData["Scope"] %>TrendIndustryContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Industry";
                            loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>TrendIndustryContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>TrendCompetitorContent', title: 'Competitor', id: '<%= ViewData["Scope"] %>TrendCompetitorContent',
                            listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Competitor";
                                loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Competitor %>', '#<%= ViewData["Scope"] %>TrendCompetitorContent');
                            } }
                        },

                        { contentEl: '<%= ViewData["Scope"] %>TrendProductContent', title: 'Products', id: '<%= ViewData["Scope"] %>TrendProductContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Product";
                                loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Product %>', '#<%= ViewData["Scope"] %>TrendProductContent');
                            } 
                            }
                        },
                    {contentEl:'<%= ViewData["Scope"] %>TrendTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>TrendTeamContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Team";
                           loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'), 
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>TrendTeamContent');
                       } }
                   },
                                            { contentEl: '<%= ViewData["Scope"] %>TrendUserContent', title: 'User', id: '<%= ViewData["Scope"] %>TrendUserContent',
                                                listeners: { activate: function() {
                                                    document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > User";
                                                    loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>TrendUserContent');
                                                } }
                                                },

                     { contentEl: '<%= ViewData["Scope"] %>TrendDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>TrendDiscussionContent',
                         listeners: { activate: function() {
                         document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Discussion";
                             loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>TrendDiscussionContent');
                         } }
                         }, 
                      { contentEl: '<%= ViewData["Scope"] %>TrendSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>TrendSourceContent',
                          listeners: { activate: function() {
                          document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Source";
                            loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>TrendSourceContent');
                        }}},
                    { contentEl: '<%= ViewData["Scope"] %>TrendCommentContent', title: 'Comment', id: '<%= ViewData["Scope"] %>TrendCommentContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Comment";
                               loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>TrendCommentContent');
                           } }
                       },
 
                        { contentEl: '<%= ViewData["Scope"] %>TrendEventContent', title: 'Events', id: '<%= ViewData["Scope"] %>TrendEventContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trends</u> > Events";
                                loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Event %>', '#<%= ViewData["Scope"] %>TrendEventContent');
                            } 
                            }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>TrendLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>TrendLibraryContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trend</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>TrendLibraryContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>TrendNewsContent', title: 'News', id: '<%= ViewData["Scope"] %>TrendNewsContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Trend</u> > News";
                            loadDetailList('<%= Url.Action("GetDetails", "Trend") %>', getIdValue('<%= ViewData["Scope"] %>', 'Trend'),
                            '<%= ViewData["Scope"] %>Trend', '<%= (int) DetailType.News %>', '#<%= ViewData["Scope"] %>TrendNewsContent');
                        }
                        }
                    }            
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>TrendList');
	    });
</script>
<asp:Panel ID="TrendListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>TrendList"  class="indexOne">
    <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />

<asp:Panel ID="TrendFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>TrendEditFormContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>TrendIndustryContent" class="x-hide-display " />
    <div id="<%= ViewData["Scope"] %>TrendCompetitorContent" class="x-hide-display " />
    <div id="<%= ViewData["Scope"] %>TrendProductContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TrendTeamContent" class="x-hide-display " />
    <div id="<%= ViewData["Scope"] %>TrendUserContent" class="x-hide-display " />
    <div id="<%= ViewData["Scope"] %>TrendDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TrendSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TrendCommentContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TrendEventContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TrendLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TrendNewsContent" class="x-hide-display" />
</asp:Panel>