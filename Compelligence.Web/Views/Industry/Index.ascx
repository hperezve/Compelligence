<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    var showIndustryByHierarchy = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            $('#gridOverflow').css("display", "none");
            $('#gridHierarchyOverflow').css("display", "block");
            posentity = entity.indexOf('All');
            controller = entity.substring(0, posentity);
            var myEntitiesCheck = $('#' + scope + controller + 'Checkbox').prop('checked');
            if (myEntitiesCheck) {
                $('#' + scope + controller + 'Checkbox').prop('checked', false);
            }
        }
        else {
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            showMyEntities(invoke, scope, entity, entityall, assignedTo, userId);
        }
    };
    var hierarchyHidden = function(invoke, scope, controller) {
        if (invoke.checked) {
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            var hierarchyCheck = $('#' + scope + controller + 'HierarchyCheckbox').prop('checked');
            if (hierarchyCheck) {
                $('#' + scope + controller + 'HierarchyCheckbox').prop('checked', false);
            }
        }
    };
    var hiddenHierarchy = function(scope, controller) {
    var hierarchyCheck = $('#' + scope + controller + 'HierarchyCheckbox').prop('checked');
        if (hierarchyCheck) {
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            var hierarchyCheck = $('#' + scope + controller + 'HierarchyCheckbox').prop('checked');
            if (hierarchyCheck) {
                $('#' + scope + controller + 'HierarchyCheckbox').prop('checked', false);
            }
        }
    };
</script>
<script type="text/javascript">
    var GetHeaderRefreshIndustry = function() {
        var iAll = $('#gridOverflow').css("display");
        var iHier = $('#gridHierarchyOverflow').css("display");
        var browseIdT = 'IndustryAll';
        if (iAll == 'none') {
            browseIdT = 'IndustryByParent';
        }
        var idOfGrid = getIdBySelectedRow('<%= ViewData["Scope"] %>', browseIdT);
        if (idOfGrid == null) {
            idOfGrid = getIdValue('<%= ViewData["Scope"] %>', 'Industry');
        }
        if (idOfGrid != null && idOfGrid != undefined) {
            getEntityToRefresh('<%= Url.Action("Edit", "Industry") %>', '<%= ViewData["Scope"] %>', 'Industry', idOfGrid, browseIdT, '#IndustryContent');
        }
    }
</script>
<script type="text/javascript">
	    $(function() {
	        IndustrySubtabs = new Ext.TabPanel({
                renderTo: 'IndustryContent',
                autoWidth: true,
                frame: true,
                height: 640,
                listeners: {  
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    {contentEl:'<%= ViewData["Scope"] %>IndustryEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>IndustryEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Header";
//                            var currentId = getIdValue('<%= ViewData["Scope"] %>', 'Industry');
//                            if (currentId == null) {
//                                getEntity('<%= Url.Action("Edit", "Industry") %>', '<%= ViewData["Scope"] %>', 'Industry', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'), 'IndustryAll', '#IndustryContent');
                                GetHeaderRefreshIndustry();
                            //}
                        }
                        }
                    },
                    {contentEl: '<%= ViewData["Scope"] %>IndustryCompetitorContent', title: 'Competitors', id: '<%= ViewData["Scope"] %>IndustryCompetitorContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Competitors";
                                loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'), 
                                '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Competitor %>', '#<%= ViewData["Scope"] %>IndustryCompetitorContent'); 
                            } }},
                    {contentEl: '<%= ViewData["Scope"] %>IndustryProductContent', title: 'Product', id: '<%= ViewData["Scope"] %>IndustryProductContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Product";
                                loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'), 
                                '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Product %>', '#<%= ViewData["Scope"] %>IndustryProductContent'); 
                            } }},
                    {contentEl:'<%= ViewData["Scope"] %>IndustryTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>IndustryTeamContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Team";
                           loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'), 
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>IndustryTeamContent');
                       } }
                   },
                      { contentEl: '<%= ViewData["Scope"] %>IndustryUserContent', title: 'User', id: '<%= ViewData["Scope"] %>IndustryUserContent',
                          listeners: { activate: function() {
                              document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > User";
                              loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>IndustryUserContent');
                          } 
                          }
                      },
                    { contentEl: '<%= ViewData["Scope"] %>IndustryCriteriaContent', title: 'Comparinator', id: '<%= ViewData["Scope"] %>IndustryCriteriaContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Comparinator";
                            loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                                '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Criteria %>', '#<%= ViewData["Scope"] %>IndustryCriteriaContent');
                        } }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>IndustryDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>IndustryDiscussionContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Discussion";
                            loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>IndustryDiscussionContent');
                        } 
                        }
                    },
                       //feedback
                       {contentEl: '<%= ViewData["Scope"] %>IndustryFeedbackContent', title: 'FeedBacks', id: '<%= ViewData["Scope"] %>IndustryFeedbackContent',
                       listeners: { activate: function() {
                       document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industrys</u> > Feedback";
                       loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Feedback %>', '#<%= ViewData["Scope"] %>IndustryFeedbackContent');
                       }
                       }
                   },
                    { contentEl: '<%= ViewData["Scope"] %>IndustryLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>IndustryLibraryContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>IndustryLibraryContent');
                        } }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>IndustryLibraryNewsContent', title: 'News', id: '<%= ViewData["Scope"] %>IndustryLibraryNewsContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > News";
                            loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.LibraryNews %>', '#<%= ViewData["Scope"] %>IndustryLibraryNewsContent');
                        } }
                    },
                    //(Small)
//                    {contentEl: '<%= ViewData["Scope"] %>IndustryBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>IndustryBudgetContent',
//                    listeners: { activate: function() {
//                    document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Budget";
//                        loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
//                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>IndustryBudgetContent');
//                    }
//                    }
//                    },
//                    //(Small)
//                    {contentEl: '<%= ViewData["Scope"] %>IndustryMetricContent', title: 'Metrics', id: '<%= ViewData["Scope"] %>IndustryMetricContent',
//                    listeners: { activate: function() {
//                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Metrics";
//                        loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
//                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>IndustryMetricContent');
//                    }
//                    }
//                    },
                    //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>IndustrySourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>IndustrySourceContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Source";
                        loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>IndustrySourceContent');
                    }
                    }
                    },
                    //(Small)
//                    {contentEl: '<%= ViewData["Scope"] %>IndustryEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>IndustryEntityRelationContent',
//                    listeners: { activate: function() {
//                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Related";
//                        loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
//                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>IndustryEntityRelationContent');
//                    } 
//                    }
//                    },
                    //(Small)
                    {contentEl: '<%= ViewData["Scope"] %>IndustryPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>IndustryPlanContent',
                    listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Plan";
                        loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>IndustryPlanContent');
                    } 
                    }
                    },
                    //(Small)
                        {contentEl: '<%= ViewData["Scope"] %>IndustryFinancialContent', title: 'Financial', id: '<%= ViewData["Scope"] %>IndustryFinancialContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Industry</u> > Financial";
                            loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                                '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.IndustryFinancial %>', '#<%= ViewData["Scope"] %>IndustryFinancialContent');
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>IndustryCommentContent',
                        title: 'Comment',
                        id: '<%= ViewData["Scope"] %>IndustryCommentContent',
                        //disabled: displayCommentButton,
                        listeners: {
                            activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Industry</u> > Comment";
                                loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Industry'),
                            '<%= ViewData["Scope"] %>Industry', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>IndustryCommentContent');
                            }

                        }
                    }                      
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>IndustryList');
	    });
</script>
<asp:Panel ID="IndustryListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>IndustryList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
     <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="IndustryFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>IndustryEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryCompetitorContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryProductContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryMetricContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryCriteriaContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryCriteriaGroupContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryFeedbackContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustrySourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryPlanContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryLibraryNewsContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryFinancialContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>IndustryCommentContent" class="x-hide-display" />
</asp:Panel> 
