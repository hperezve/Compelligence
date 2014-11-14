<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    var showKitByHierarchy = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            $('#gridOverflow').css("display", "none");
            $('#gridAchievedOverflow').css("display", "none");
            $('#gridHierarchyOverflow').css("display", "block");
            posentity = entity.indexOf('All');
            controller = entity.substring(0, posentity);
            var myEntitiesCheck = $('#' + scope + controller + 'Checkbox').prop('checked');
            if (myEntitiesCheck) {
                $('#' + scope + controller + 'Checkbox').prop('checked', false);
            }
            var achivedEntitiesCheck = $('#' + scope + controller + 'AchievedCheckbox').prop('checked');
            if (achivedEntitiesCheck) {
                $('#' + scope + controller + 'AchievedCheckbox').prop('checked', false);
            }
        }
        else {
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            $('#gridAchievedOverflow').css("display", "none");
            showMyEntities(invoke, scope, entity, entityall, assignedTo, userId);
        }
    };
    var uncheckedOther = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            $('#gridAchievedOverflow').css("display", "none");
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            posentity = entity.indexOf('All');
            controller = entity.substring(0, posentity);
            var achivedEntitiesCheck = $('#' + scope + controller + 'AchievedCheckbox').prop('checked');
            var showHierarChyCheck = $('#' + scope + controller + 'HierarchyCheckbox').prop('checked');
            if (achivedEntitiesCheck) {
                $('#' + scope + controller + 'AchievedCheckbox').prop('checked', false);
            }
            if (showHierarChyCheck) {
                $('#' + scope + controller + 'HierarchyCheckbox').prop('checked', false);
            }
        }
        else {
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            $('#gridAchievedOverflow').css("display", "none");
            showMyEntities(invoke, scope, entity, entityall, assignedTo, userId);
        }
    };
    var hierarchyHidden = function(invoke, scope, controller) {
        if (invoke.checked) {
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            $('#gridHierarchyOverflow').css("display", "none");
            var hierarchyCheck = $('#' + scope + controller + 'HierarchyCheckbox').prop('checked');
            if (hierarchyCheck) {
                $('#' + scope + controller + 'HierarchyCheckbox').prop('checked', false);
            }
            var achivedEntitiesCheck = $('#' + scope + controller + 'AchievedCheckbox').prop('checked');
            if (achivedEntitiesCheck) {
                $('#' + scope + controller + 'AchievedCheckbox').prop('checked', false);
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
            var achivedEntitiesCheck = $('#' + scope + controller + 'AchievedCheckbox').prop('checked');
            if (achivedEntitiesCheck) {
                $('#' + scope + controller + 'AchievedCheckbox').prop('checked', false);
            }
        }
    };
    var showKitWithAchieved = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            $('#gridAchievedOverflow').css("display", "block");
            $('#gridOverflow').css("display", "none");
            $('#gridHierarchyOverflow').css("display", "none");
            posentity = entity.indexOf('All');
            controller = entity.substring(0, posentity);
            var myEntitiesCheck = $('#' + scope + controller + 'Checkbox').prop('checked');
            var showHierarChyCheck = $('#' + scope + controller + 'HierarchyCheckbox').prop('checked');
            if (myEntitiesCheck) {
                $('#' + scope + controller + 'Checkbox').prop('checked', false);
            }
            if (showHierarChyCheck) {
                $('#' + scope + controller + 'HierarchyCheckbox').prop('checked', false);
            }
        }
        else {
            $('#gridOverflow').css("display", "block");
            $('#gridHierarchyOverflow').css("display", "none");
            $('#gridAchievedOverflow').css("display", "none");
            showMyEntities(invoke, scope, entity, entityall, assignedTo, userId);
        }
    };
</script>
<script type="text/javascript">
    $(function() {
        // Kit subtabs
        KitSubtabs = new Ext.TabPanel({
            renderTo: 'KitContent',
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
                    { contentEl: '<%= ViewData["Scope"] %>KitEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>KitEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Header";
                            var currentId = getIdValue('<%= ViewData["Scope"] %>', 'Kit');
                            if (currentId == null) {
                                currentId = getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit');
                                if (currentId != null) {
                                    getEntity('<%= Url.Action("Edit", "Kit") %>', '<%= ViewData["Scope"] %>', 'Kit', currentId, 'KitAll', '#KitContent');
                                }
                            }
                            getEntityToRefresh('<%= Url.Action("Edit", "Kit") %>', '<%= ViewData["Scope"] %>', 'Kit', getIdBySelectedRow('<%= ViewData["Scope"] %>', 'KitAll'), 'KitAll', '#KitContent');
                        }
                        }
                    },

                     { contentEl: '<%= ViewData["Scope"] %>KitTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>KitTeamContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>KitTeamContent');
                        }
                        }
                    },
            //User
                           {contentEl: '<%= ViewData["Scope"] %>KitUserContent', title: 'User', id: '<%= ViewData["Scope"] %>KitUserContent',
                           listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > User";
                               loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>KitUserContent');
                           }
                           }
                       },
            //Endsuer

                    {contentEl: '<%= ViewData["Scope"] %>KitBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>KitBudgetContent',
                    listeners: { activate: function() {
                    document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kit</u> > Budget";
                        loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>KitBudgetContent');
                    } 
                    }
                },
                    { contentEl: '<%= ViewData["Scope"] %>KitPlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>KitPlanContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Plan";
                            loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>KitPlanContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>KitImplicationContent', title: 'Implications', id: '<%= ViewData["Scope"] %>KitImplicationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Implications";
                            loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Implication %>', '#<%= ViewData["Scope"] %>KitImplicationContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>KitMetricContent', title: 'Metrics', id: '<%= ViewData["Scope"] %>KitMetricContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Metrics";
                            loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>KitMetricContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>KitDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>KitDiscussionContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Discussion";
                            loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>KitDiscussionContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>KitSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>KitSourceContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Source";
                            loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>KitSourceContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>KitEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>KitEntityRelationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Related";
                            loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>KitEntityRelationContent');
                        } 
                        }
                    },
                        { contentEl: '<%= ViewData["Scope"] %>KitLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>KitLibraryContent',
                            listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Kits</u> > Library";
                                loadDetailList('<%= Url.Action("GetDetails", "Kit") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Kit'),
                            '<%= ViewData["Scope"] %>Kit', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>KitLibraryContent');
                            }
                            }
                        }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>KitList');
    });
</script>

<asp:Panel ID="KitListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>KitList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS">
        <img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>" /></div>
</asp:Panel>
<br />
<asp:Panel ID="KitFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>KitEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitPlanContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitImplicationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitMetricContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>KitLibraryContent" class="x-hide-display" />
</asp:Panel>
