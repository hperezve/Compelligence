<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    var ReloadTabHeader = function(Tid) {
        if (Tid != undefined) {
            showEntity('/Product.aspx/Edit', 'Environment', 'Product', Tid, 'ProductAll', '#ProductContent', '', '');
        }
    };
    var GetHeaderRefresh = function() {
        var idOfGrid = getIdBySelectedRow('<%= ViewData["Scope"] %>', 'ProductAll');
        if (idOfGrid == null) {
            idOfGrid = getIdValue('<%= ViewData["Scope"] %>', 'Product');
        }
        if (idOfGrid != null && idOfGrid != undefined) {
            getEntityToRefresh('<%= Url.Action("Edit", "Product") %>', '<%= ViewData["Scope"] %>', 'Product', idOfGrid, 'ProductAll', '#ProductContent');
        }
    }
</script>
<%--<style type="text/css">
    ul { margin-left:1.5em; padding-left:0px; list-style: inline; }
    li { margin-bottom:0.5em; }
</style>--%>
<script type="text/javascript">
    $(function() {
        ProductSubtabs = new Ext.TabPanel({
            renderTo: 'ProductContent',
            autoWidth: true,
            frame: true,
            //defaults:{autoHeight: true},
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                        { contentEl: '<%= ViewData["Scope"] %>ProductEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>ProductEditFormContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Header";
                                GetHeaderRefresh();
                            }
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>ProductCompetitorContent', title: 'Competitor', id: '<%= ViewData["Scope"] %>ProductCompetitorContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Competitor";
                                loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                                '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Competitor %>', '#<%= ViewData["Scope"] %>ProductCompetitorContent');
                            }
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>ProductIndustryContent', title: 'Industries', id: '<%= ViewData["Scope"] %>ProductIndustryContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Industries";
                                loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                                '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>ProductIndustryContent');
                            }
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>ProductTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>ProductTeamContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Team";
                                loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>ProductTeamContent');
                            }
                            }
                        },
            //User
                        {contentEl: '<%= ViewData["Scope"] %>ProductUserContent', title: 'User', id: '<%= ViewData["Scope"] %>ProductUserContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > User";
                            loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.User%>', '#<%= ViewData["Scope"] %>ProductUserContent');
                        }
                        }
                    },
            //EndUser

//                        {contentEl: '<%= ViewData["Scope"] %>ProductBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>ProductBudgetContent',
//                        listeners: { activate: function() {
//                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Product</u> > Budget";
//                            loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
//                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>ProductBudgetContent');
//                        }
//                        }
//                    },
                        { contentEl: '<%= ViewData["Scope"] %>ProductDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>ProductDiscussionContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Discussion";
                                loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>ProductDiscussionContent');
                            }
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>ProductImplicationContent', title: 'Implication', id: '<%= ViewData["Scope"] %>ProductImplicationContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Implication";
                                loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Implication %>', '#<%= ViewData["Scope"] %>ProductImplicationContent');
                            }
                            }
                        },

                        { contentEl: '<%= ViewData["Scope"] %>ProductLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>ProductLibraryContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Library";
                                loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>ProductLibraryContent');
                            }
                            }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>ProductLibraryNewsContent', title: 'News', id: '<%= ViewData["Scope"] %>ProductLibraryNewsContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Product</u> > News";
                            loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.LibraryNews %>', '#<%= ViewData["Scope"] %>ProductLibraryNewsContent');
                        }
                        }
                    },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProductSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>ProductSourceContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Source";
                        loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>ProductSourceContent');
                    }
                    }
                },
            //(Small)
//                    {contentEl: '<%= ViewData["Scope"] %>ProductEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>ProductEntityRelationContent',
//                    listeners: { activate: function() {
//                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Related";
//                        loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
//                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>ProductEntityRelationContent');
//                    }
//                    }
//                },
            //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>ProductPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>ProductPlanContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Plan";
                        loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>ProductPlanContent');
                    }
                    }
                },
            //(Small)
//                        {contentEl: '<%= ViewData["Scope"] %>ProductMetricContent', title: 'Metric', id: '<%= ViewData["Scope"] %>ProductMetricContent',
//                        listeners: { activate: function() {
//                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Metric";
//                            loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
//                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>ProductMetricContent');
//                        }
//                        }
//                    },
            //(Small)
                        {contentEl: '<%= ViewData["Scope"] %>ProductCustomerContent', title: 'Customer', id: '<%= ViewData["Scope"] %>ProductCustomerContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Customer";
                            loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                                '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Customer %>', '#<%= ViewData["Scope"] %>ProductCustomerContent');
                        }
                        }
                    },
            //feedback
                       {contentEl: '<%= ViewData["Scope"] %>ProductFeedbackContent', title: 'FeedBacks', id: '<%= ViewData["Scope"] %>ProductFeedbackContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Feedback";
                           loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Feedback %>', '#<%= ViewData["Scope"] %>ProductFeedbackContent');
                       }
                       }
                   }
            //endfeedback
                    ,
                       { contentEl: '<%= ViewData["Scope"] %>ProductMarketTypeContent', title: 'Market Type', id: '<%= ViewData["Scope"] %>ProductMarketTypeContent',
                       listeners: { activate: function() {
                       document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Market Type";
                           loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.MarketType %>', '#<%= ViewData["Scope"] %>ProductMarketTypeContent');
                       }
                       }
                   },
                       { contentEl: '<%= ViewData["Scope"] %>ProductPriceContent', title: 'Pricing', id: '<%= ViewData["Scope"] %>ProductPriceContent',
                           listeners: { activate: function() {
                               document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Pricing";
                               loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Price %>', '#<%= ViewData["Scope"] %>ProductPriceContent');
                           }
                           }
                       },
                        { contentEl: '<%= ViewData["Scope"] %>ProductCommentContent',
                            title: 'Comment',
                            id: '<%= ViewData["Scope"] %>ProductCommentContent',
                            //disabled: displayCommentButton,
                            listeners: {
                                activate: function() {
                                    document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Product</u> > Comment";
                                    loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                                '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>ProductCommentContent');
                                }

                            }
                        },
                       { contentEl: '<%= ViewData["Scope"] %>ProductPositioningContent', title: 'Positioning', id: '<%= ViewData["Scope"] %>ProductPositioningContent',
                           listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Positioning";
                               loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.Positioning %>', '#<%= ViewData["Scope"] %>ProductPositioningContent');
                           }
                           }
                       },
                       { contentEl: '<%= ViewData["Scope"] %>ProductCompetitiveMessagingContent', title: 'Competitive Messaging', id: '<%= ViewData["Scope"] %>ProductCompetitiveMessagingContent',
                           listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Products</u> > Competitive Messaging";
                               loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Product'),
                            '<%= ViewData["Scope"] %>Product', '<%= (int) DetailType.CompetitiveMessaging %>', '#<%= ViewData["Scope"] %>ProductCompetitiveMessagingContent');
                           }
                           }
                       }
                    ]
        });
        resizeContent('#<%= ViewData["Scope"] %>ProductList');
    });
</script>

<asp:Panel ID="ProductListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProductList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="ProductFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProductEditFormContent" />
    <div id="<%= ViewData["Scope"] %>ProductCompetitorContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductIndustryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductPlanContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductImplicationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductMetricContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductCustomerContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductLibraryNewsContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductFeedbackContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductMarketTypeContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductPriceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductCommentContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductPositioningContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ProductCompetitiveMessagingContent" class="x-hide-display" />
</asp:Panel>
