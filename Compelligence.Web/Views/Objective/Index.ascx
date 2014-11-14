<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Objective>>" %>
<script type="text/javascript">
    var showObjectiveByHierarchy = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            $('#gridOverflowObjective').css("display", "none");
            $('#gridAchievedOverflowObjective').css("display", "none");
            $('#gridHierarchyOverflowObjective').css("display", "block");
            posentity = entity.indexOf('All');
            controller = entity.substring(0, posentity);
            var myEntitiesCheck = $('#' + scope + controller + 'Checkbox').prop('checked');
            var achivedEntitiesCheck = $('#' + scope + controller + 'AchievedCheckbox').prop('checked');
            if (myEntitiesCheck) {
                $('#' + scope + controller + 'Checkbox').prop('checked', false);
            }
            if (achivedEntitiesCheck) {
                $('#' + scope + controller + 'AchievedCheckbox').prop('checked', false);
            }
        }
        else {
            $('#gridOverflowObjective').css("display", "block");
            $('#gridHierarchyOverflowObjective').css("display", "none");
            $('#gridAchievedOverflowObjective').css("display", "none");
            showMyEntities(invoke, scope, entity, entityall, assignedTo, userId);
        }
    };

    var uncheckedOther = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            $('#gridAchievedOverflowObjective').css("display", "none");
            $('#gridOverflowObjective').css("display", "block");
            $('#gridHierarchyOverflowObjective').css("display", "none");
            posentity = entity.indexOf('All');
            controller = entity.substring(0, posentity);

            var showHierarChyCheck = $('#' + scope + controller + 'HierarchyCheckbox').prop('checked');
            var achivedEntitiesCheck = $('#' + scope + controller + 'AchievedCheckbox').prop('checked');
            if (achivedEntitiesCheck) {
                $('#' + scope + controller + 'AchievedCheckbox').prop('checked', false);
            }
            if (showHierarChyCheck) {
                $('#' + scope + controller + 'HierarchyCheckbox').prop('checked', false);
            }
        }
        else {
            $('#gridOverflowObjective').css("display", "block");
            $('#gridHierarchyOverflowObjective').css("display", "none");
            $('#gridAchievedOverflowObjective').css("display", "none");
            showMyEntities(invoke, scope, entity, entityall, assignedTo, userId);
        }
    };
    
    var hierarchyHidden = function(invoke, scope, controller) {
        if (invoke.checked) {
            $('#gridOverflowObjective').css("display", "block");
            $('#gridHierarchyOverflowObjective').css("display", "none");
            $('#gridAchievedOverflowObjective').css("display", "none");
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
            $('#gridOverflowObjective').css("display", "block");
            $('#gridHierarchyOverflowObjective').css("display", "none");
            $('#gridAchievedOverflowObjective').css("display", "none");
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
    var showObjectiveWithAchieved = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            $('#gridAchievedOverflowObjective').css("display", "block");
            $('#gridOverflowObjective').css("display", "none");
            $('#gridHierarchyOverflowObjective').css("display", "none");
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
            $('#gridOverflowObjective').css("display", "block");
            $('#gridHierarchyOverflowObjective').css("display", "none");
            $('#gridAchievedOverflowObjective').css("display", "none");
            showMyEntities(invoke, scope, entity, entityall, assignedTo, userId);
        }
    };
</script>
<script type="text/javascript">
	    $(function() {
	        ObjectiveSubtabs = new Ext.TabPanel({
                renderTo: 'ObjectiveContent',
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
                    {contentEl:'<%= ViewData["Scope"] %>ObjectiveEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>ObjectiveEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Header";
                        getEntityToRefresh('<%= Url.Action("Edit", "Objective") %>', '<%= ViewData["Scope"] %>', 'Objective', getIdBySelectedRow('<%= ViewData["Scope"] %>', 'ObjectiveAll'), 'ObjectiveAll', '#ObjectiveContent');
                        }
                        }
                    },
                    {contentEl:'<%= ViewData["Scope"] %>ObjectiveTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>ObjectiveTeamContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Team";
                           loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'), 
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>ObjectiveTeamContent');
                       } }
                   },
                   //User
                        {contentEl: '<%= ViewData["Scope"] %>ObjectiveUserContent', title: 'User', id: '<%= ViewData["Scope"] %>ObjectiveUserContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > User";
                            loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'),
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>ObjectiveUserContent');
                        } }
                    },
                    //Enduser                        
                    { contentEl: '<%= ViewData["Scope"] %>ObjectiveBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>ObjectiveBudgetContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Budget";
                            loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'),
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>ObjectiveBudgetContent');
                        }}},     
                    {contentEl:'<%= ViewData["Scope"] %>ObjectivePlanContent', title: 'Plan', id: '<%= ViewData["Scope"] %>ObjectivePlanContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Plan";
                            loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'), 
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Plan %>', '#<%= ViewData["Scope"] %>ObjectivePlanContent'); 
                        } }},
                    {contentEl:'<%= ViewData["Scope"] %>ObjectiveImplicationContent', title: 'Implications', id: '<%= ViewData["Scope"] %>ObjectiveImplicationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Implications";
                        loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'), 
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Implication %>', '#<%= ViewData["Scope"] %>ObjectiveImplicationContent'); 
                        } }},
                    {contentEl:'<%= ViewData["Scope"] %>ObjectiveMetricContent', title: 'Metrics', id: '<%= ViewData["Scope"] %>ObjectiveMetricContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Metrics";
                            loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'), 
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Metric %>', '#<%= ViewData["Scope"] %>ObjectiveMetricContent'); 
                        } }},
                    {contentEl: '<%= ViewData["Scope"] %>ObjectiveDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>ObjectiveDiscussionContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Discussion";
                               loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'), 
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>ObjectiveDiscussionContent');
                        }}}, 
                      { contentEl: '<%= ViewData["Scope"] %>ObjectiveSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>ObjectiveSourceContent',
                          listeners: { activate: function() {
                          document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Source";
                            loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'),
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>ObjectiveSourceContent');
                        }}},   
                    {contentEl: '<%= ViewData["Scope"] %>ObjectiveEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>ObjectiveEntityRelationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Related";
                               loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'), 
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>ObjectiveEntityRelationContent');
                        }}},
                        { contentEl: '<%= ViewData["Scope"] %>ObjectiveLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>ObjectiveLibraryContent',
                            listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'),
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>ObjectiveLibraryContent');
                        } }},
                        { contentEl: '<%= ViewData["Scope"] %>ObjectiveProjectContent', title: 'Project', id: '<%= ViewData["Scope"] %>ObjectiveProjectContent',
                            listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Objectives</u> > Project";
                                loadDetailList('<%= Url.Action("GetDetails", "Objective") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Objective'),
                            '<%= ViewData["Scope"] %>Objective', '<%= (int) DetailType.Project %>', '#<%= ViewData["Scope"] %>ObjectiveProjectContent');
                            } }
                            }    
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>ObjectiveList');
	    });
</script>

<asp:Panel ID="ObjectiveListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ObjectiveList"  class="indexOne">
    <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="ObjectiveFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ObjectiveEditFormContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>ObjectiveTeamContent" class="x-hide-display " />
    <div id="<%= ViewData["Scope"] %>ObjectiveUserContent" class="x-hide-display " />
    <div id="<%= ViewData["Scope"] %>ObjectiveBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ObjectivePlanContent" class="x-hide-display " />
    <div id="<%= ViewData["Scope"] %>ObjectiveImplicationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ObjectiveMetricContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ObjectiveDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ObjectiveSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ObjectiveEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ObjectiveLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ObjectiveProjectContent" class="x-hide-display" />
</asp:Panel>
