<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    TeamSubtabs = new Ext.TabPanel({
	            renderTo: 'TeamContent', 
                autoWidth:true,
                frame:true,
                //defaults:{autoHeight: true},
                height:640,
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }
                },
                items: [
                    {contentEl: '<%= ViewData["Scope"] %>TeamEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>TeamEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Teams</u>  > Header";
                        }
                        }
                    },
                    {contentEl:'<%= ViewData["Scope"] %>TeamTeamRoleContent', title: 'Role', id: '<%= ViewData["Scope"] %>TeamTeamRoleContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Teams</u>  > Role";
                            loadDetailList('<%= Url.Action("GetDetails", "Team") %>', getIdValue('<%= ViewData["Scope"] %>', 'Team'), 
                            '<%= ViewData["Scope"] %>Team', '<%= (int) DetailType.TeamRole %>', '#<%= ViewData["Scope"] %>TeamTeamRoleContent'); 
                        } }},
                    {contentEl:'<%= ViewData["Scope"] %>TeamTeamMemberContent', title: 'Members', id: '<%= ViewData["Scope"] %>TeamTeamMemberContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Teams</u>  > Members";
                            loadDetailList('<%= Url.Action("GetDetails", "Team") %>', getIdValue('<%= ViewData["Scope"] %>', 'Team'), 
                            '<%= ViewData["Scope"] %>Team', '<%= (int) DetailType.TeamMember %>', '#<%= ViewData["Scope"] %>TeamTeamMemberContent'); 
                        } }},
                    {contentEl: '<%= ViewData["Scope"] %>TeamProjectContent', title: 'Project', id: '<%= ViewData["Scope"] %>TeamProjectContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Teams</u>  > Project";
                                loadDetailList('<%= Url.Action("GetDetails", "Team") %>', getIdValue('<%= ViewData["Scope"] %>', 'Team'), 
                                '<%= ViewData["Scope"] %>Team', '<%= (int) DetailType.Project %>', '#<%= ViewData["Scope"] %>TeamProjectContent'); 
                            } }},

                   { contentEl: '<%= ViewData["Scope"] %>TeamCompetitorsContent', title: 'Competitor', id: '<%= ViewData["Scope"] %>TeamCompetitorsContent',
                                listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Teams</u>  > Competitor";
                                    loadDetailList('<%= Url.Action("GetDetails", "Team") %>', getIdValue('<%= ViewData["Scope"] %>', 'Team'),
                                '<%= ViewData["Scope"] %>Team', '<%= (int) DetailType.Competitor %>', '#<%= ViewData["Scope"] %>TeamCompetitorsContent');
                                } }
                            },
                   { contentEl: '<%= ViewData["Scope"] %>TeamProductContent', title: 'Product', id: '<%= ViewData["Scope"] %>TeamProductContent',
                                    listeners: { activate: function() {
                                    document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Teams</u>  > Product";
                                        loadDetailList('<%= Url.Action("GetDetails", "Team") %>', getIdValue('<%= ViewData["Scope"] %>', 'Team'),
                                '<%= ViewData["Scope"] %>Team', '<%= (int) DetailType.Product %>', '#<%= ViewData["Scope"] %>TeamProductContent');
                                    } }
                                    }
                            
                            
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>TeamList');
	    });
</script>

<asp:Panel ID="TeamListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>TeamList" class="indexOne">
    <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="TeamFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>TeamEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TeamTeamRoleContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TeamTeamMemberContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TeamProjectContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TeamCompetitorsContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TeamProductContent" class="x-hide-display" />
</asp:Panel>
