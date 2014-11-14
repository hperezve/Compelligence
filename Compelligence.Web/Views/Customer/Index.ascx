<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
        // Customer subtabs
        CustomerSubtabs = new Ext.TabPanel({
            renderTo: 'CustomerContent',
            autoWidth: true,
            frame: true,
            //defaults: { autoHeight: true },
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>CustomerEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>CustomerEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Header";
                            var currentId = getIdValue('<%= ViewData["Scope"] %>', 'Customer');
                            if (currentId == null) {
                                currentId = getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer');
                                if (currentId != null) {
                                    getEntity('<%= Url.Action("Edit", "Customer") %>', '<%= ViewData["Scope"] %>', 'Customer', currentId, 'CustomerAll', '#CustomerContent');
                                }
                            }
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>CustomerEmployeeContent', title: 'People', id: '<%= ViewData["Scope"] %>CustomerEmployeeContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > People";
                            loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Employee %>', '#<%= ViewData["Scope"] %>CustomerEmployeeContent');
                        } 
                        }
                    },
                        { contentEl: '<%= ViewData["Scope"] %>CustomerLocationContent', title: 'Location', id: '<%= ViewData["Scope"] %>CustomerLocationContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Location";
                                loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Location %>', '#<%= ViewData["Scope"] %>CustomerLocationContent');
                            } 
                            }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>CustomerTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>CustomerTeamContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>CustomerTeamContent');
                        }
                        }
                    },
            //User
                    {contentEl: '<%= ViewData["Scope"] %>CustomerUserContent', title: 'User', id: '<%= ViewData["Scope"] %>CustomerUserContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > User";
                        loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>CustomerUserContent');
                    }
                    }
                },
            //EndUser
                    {contentEl: '<%= ViewData["Scope"] %>CustomerBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>CustomerBudgetContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customer</u> > Budget";
                        loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>CustomerBudgetContent');
                    } 
                    }
                },
                    { contentEl: '<%= ViewData["Scope"] %>CustomerCompetitorContent', title: 'Competitor', id: '<%= ViewData["Scope"] %>CustomerCompetitorContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Competitor";
                            loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                                '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Competitor %>', '#<%= ViewData["Scope"] %>CustomerCompetitorContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>CustomerSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>CustomerSourceContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Source";
                            loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>CustomerSourceContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>CustomerEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>CustomerEntityRelationContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Related";
                            loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>CustomerEntityRelationContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>CustomerLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>CustomerLibraryContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>CustomerLibraryContent');
                        } 
                        }
                    },
                        { contentEl: '<%= ViewData["Scope"] %>CustomerMetricContent',
                            title: 'Metric',
                            id: '<%= ViewData["Scope"] %>CustomerMetricContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Metric";
                                loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>CustomerMetricContent');
                            } 
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>CustomerDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>CustomerDiscussionContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Discussion";
                                loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                            '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>CustomerDiscussionContent');
                            } 
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>CustomerProductContent', title: 'Product', id: '<%= ViewData["Scope"] %>CustomerProductContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Product";
                                loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                                '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Product %>', '#<%= ViewData["Scope"] %>CustomerProductContent');
                            } 
                            }
                        },
                        { contentEl: '<%= ViewData["Scope"] %>CustomerIndustryContent', title: 'Industry', id: '<%= ViewData["Scope"] %>CustomerIndustryContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Customers</u> > Industry";
                                loadDetailList('<%= Url.Action("GetDetails", "Customer") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Customer'),
                                '<%= ViewData["Scope"] %>Customer', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>CustomerIndustryContent');
                            } 
                            }
                        }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>CustomerList');
    });
</script>

<asp:Panel ID="CustomerListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CustomerList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="CustomerFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CustomerEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerEmployeeContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerLocationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerImplicationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerCompetitorContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerMetricContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerProductContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CustomerIndustryContent" class="x-hide-display" />
</asp:Panel>
