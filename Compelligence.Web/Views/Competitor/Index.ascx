<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    var GetHeaderRefreshCompetitor = function() {
        var idOfGrid = getIdBySelectedRow('<%= ViewData["Scope"] %>', 'CompetitorAll');
        if (idOfGrid == null) {
            idOfGrid = getIdValue('<%= ViewData["Scope"] %>', 'Competitor');
        }
        if (idOfGrid != null && idOfGrid != undefined) {
            getEntityToRefresh('<%= Url.Action("Edit", "Competitor") %>', '<%= ViewData["Scope"] %>', 'Competitor', idOfGrid, 'CompetitorAll', '#CompetitorContent');
        }
    }
</script>
<script type="text/javascript">
    $(function() {
        CompetitorSubtabs = new Ext.TabPanel({
            renderTo: 'CompetitorContent',
            autoWidth: true,
            frame: true,
            height:640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                   { contentEl: '<%= ViewData["Scope"] %>CompetitorEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>CompetitorEditFormContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Header";
                           //                           var currentId = getIdValue('<%= ViewData["Scope"] %>', 'Competitor');
                           //                           if (currentId == null) {
                           //                               getEntity('<%= Url.Action("Edit", "Competitor") %>', '<%= ViewData["Scope"] %>', 'Competitor', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'), 'CompetitorAll', '#CompetitorContent');
                           GetHeaderRefreshCompetitor();
                           // }
                       }
                       }
                   },
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorEmployeeContent', title: 'People', id: '<%= ViewData["Scope"] %>CompetitorEmployeeContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > People";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Employee %>', '#<%= ViewData["Scope"] %>CompetitorEmployeeContent');
                        } }
                        },
                   { contentEl: '<%= ViewData["Scope"] %>CompetitorIndustryContent', title: 'Industries', id: '<%= ViewData["Scope"] %>CompetitorIndustryContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Industries";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>CompetitorIndustryContent');
                       } }
                       },
                   { contentEl: '<%= ViewData["Scope"] %>CompetitorProductContent', title: 'Product', id: '<%= ViewData["Scope"] %>CompetitorProductContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Product";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Product %>', '#<%= ViewData["Scope"] %>CompetitorProductContent');
                       } }
                       },
                   { contentEl: '<%= ViewData["Scope"] %>CompetitorTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>CompetitorTeamContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Team";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>CompetitorTeamContent');
                       } 
                       }
                   },
                   { contentEl: '<%= ViewData["Scope"] %>CompetitorUserContent', title: 'User', id: '<%= ViewData["Scope"] %>CompetitorUserContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > User";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>CompetitorUserContent');
                       }
                       }
                   },
                   { contentEl: '<%= ViewData["Scope"] %>CompetitorLocationContent', title: 'Location', id: '<%= ViewData["Scope"] %>CompetitorLocationContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Location";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Location %>', '#<%= ViewData["Scope"] %>CompetitorLocationContent');
                       } }
                       },
                   { contentEl: '<%= ViewData["Scope"] %>CompetitorDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>CompetitorDiscussionContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Discussion";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>CompetitorDiscussionContent');
                       } }
                   },
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorFeedbackContent', title: 'FeedBacks', id: '<%= ViewData["Scope"] %>CompetitorFeedbackContent',
                        listeners: {
                            activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Feedbacks";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Feedback %>', '#<%= ViewData["Scope"] %>CompetitorFeedbackContent');
                            }
                        }

                    },
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorCustomerContent', title: 'Customer', id: '<%= ViewData["Scope"] %>CompetitorCustomerContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Customer";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Customer %>', '#<%= ViewData["Scope"] %>CompetitorCustomerContent');
                        } 
                        }
                    },
                        { contentEl: '<%= ViewData["Scope"] %>CompetitorLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>CompetitorLibraryContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Library";
                                loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>CompetitorLibraryContent');
                            }
                            }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorLibraryNewsContent', title: 'News', id: '<%= ViewData["Scope"] %>CompetitorLibraryNewsContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitor</u> > News";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.LibraryNews %>', '#<%= ViewData["Scope"] %>CompetitorLibraryNewsContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorProductFamilyContent', title: 'Product Families', id: '<%= ViewData["Scope"] %>CompetitorProductFamilyContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitor</u> > Product Family";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.ProductFamily %>', '#<%= ViewData["Scope"] %>CompetitorProductFamilyContent');
                        }
                        }
                    },
            //(Small)
//                   {contentEl: '<%= ViewData["Scope"] %>CompetitorBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>CompetitorBudgetContent',
//                   listeners: { activate: function() {
//                       document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitor</u> > Budget";
//                       loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
//                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>CompetitorBudgetContent');
//                   }
//                   }
//               },
            //(Small)         
                    {contentEl: '<%= ViewData["Scope"] %>CompetitorSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>CompetitorSourceContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Source";
                        loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>CompetitorSourceContent');
                    }
                    }
                },
            //(Small)
//                    {contentEl: '<%= ViewData["Scope"] %>CompetitorEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>CompetitorEntityRelationContent',
//                    listeners: { activate: function() {
//                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Related";
//                        loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
//                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>CompetitorEntityRelationContent');
//                    }
//                    }
//                },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>CompetitorPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>CompetitorPlanContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Plan";
                        loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>CompetitorPlanContent');
                    }
                    }
                },
            //(Small)
//                    {contentEl: '<%= ViewData["Scope"] %>CompetitorMetricContent', title: 'Metric', id: '<%= ViewData["Scope"] %>CompetitorMetricContent',
//                    listeners: { activate: function() {
//                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Metric";
//                        loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
//                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>CompetitorMetricContent');
//                    }
//                    }
//                },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>CompetitorPartnerContent', title: 'Partners', id: '<%= ViewData["Scope"] %>CompetitorPartnerContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Partner";
                        loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.CompetitorPartner %>', '#<%= ViewData["Scope"] %>CompetitorPartnerContent');
                    }
                    }
                },
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorSupplierContent', title: 'Suppliers', id: '<%= ViewData["Scope"] %>CompetitorSupplierContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Suppliers";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.CompetitorSupplier %>', '#<%= ViewData["Scope"] %>CompetitorSupplierContent');
                        }
                        }
                    }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorCompetitorContent', title: 'Competitors', id: '<%= ViewData["Scope"] %>CompetitorCompetitorContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Competitor";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.CompetitorCompetitor %>', '#<%= ViewData["Scope"] %>CompetitorCompetitorContent');
                        }
                        }
                    }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorThreatContent', title: 'Threat', id: '<%= ViewData["Scope"] %>CompetitorThreatContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Threat";
                            loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Threat %>', '#<%= ViewData["Scope"] %>CompetitorThreatContent');
                        }
                        }
                    }
                    ,
                    { contentEl: '<%= ViewData["Scope"] %>CompetitorFinancialContent', title: 'Financial', id: '<%= ViewData["Scope"] %>CompetitorFinancialContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitors</u> > Financial";
                              loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                                 '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.CompetitorFinancial %>', '#<%= ViewData["Scope"] %>CompetitorFinancialContent');
                            
                        }
                        }
                    },
                       { contentEl: '<%= ViewData["Scope"] %>CompetitorPositioningContent', title: 'Positioning', id: '<%= ViewData["Scope"] %>CompetitorPositioningContent',
                           listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitor</u> > Positioning";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.Positioning %>', '#<%= ViewData["Scope"] %>CompetitorPositioningContent');
                           }
                           }
                       },
                       { contentEl: '<%= ViewData["Scope"] %>CompetitorCompetitiveMessagingContent', title: 'Competitive Messaging', id: '<%= ViewData["Scope"] %>CompetitorCompetitiveMessagingContent',
                           listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitor</u> > Competitive Messaging";
                           loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.CompetitiveMessaging %>', '#<%= ViewData["Scope"] %>CompetitorCompetitiveMessagingContent');
                           }
                           }
                       }
                      ,
                       { contentEl: '<%= ViewData["Scope"] %>CompetitorStrengthWeaknessContent', title: 'S&W', id: '<%= ViewData["Scope"] %>CompetitorStrengthWeaknessContent',
                           listeners: { activate: function() {
                               document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Competitor</u> > S&W";
                               loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Competitor'),
                            '<%= ViewData["Scope"] %>Competitor', '<%= (int) DetailType.StrengthWeakness %>', '#<%= ViewData["Scope"] %>CompetitorStrengthWeaknessContent');
                           }
                           }
                       }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>CompetitorList');
    });
</script>
<asp:Panel ID="CompetitorListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CompetitorList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="CompetitorFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CompetitorEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorEmployeeContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorIndustryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorProductContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorLocationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorCustomerContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorFeedbackContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorPlanContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorMetricContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorPartnerContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorLibraryNewsContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorProductFamilyContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorSupplierContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorCompetitorContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorThreatContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorFinancialContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorPositioningContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorCompetitiveMessagingContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CompetitorStrengthWeaknessContent" class="x-hide-display" />
</asp:Panel>
