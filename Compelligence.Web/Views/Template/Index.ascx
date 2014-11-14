<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
        TemplateSubtabs = new Ext.TabPanel({
            renderTo: 'TemplateContent',
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
                    { contentEl: '<%= ViewData["Scope"] %>TemplateEditFormContent',
                        title: 'Header',
                        id: '<%= ViewData["Scope"] %>TemplateEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Templates</u>  > Header";
                            var currentId = getIdValue('<%= ViewData["Scope"] %>', 'Template');
                            if (currentId == null) {
                                currentId = getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Template');
                                if (currentId != null) {
                                    getEntity('<%= Url.Action("Edit", "Template") %>', '<%= ViewData["Scope"] %>', 'Template', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Template'), 'TemplateAll', '#TemplateContent');
                                }
                            }
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>TemplateFileContent',
                        title: 'Files',
                        id: '<%= ViewData["Scope"] %>TemplateFileContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Templates</u>  > File";
                            loadDetailList('<%= Url.Action("GetDetails", "Template") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Template'),
                            '<%= ViewData["Scope"] %>Template', '<%= (int) DetailType.File %>', '#<%= ViewData["Scope"] %>TemplateFileContent');
                        } 
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>TemplateTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>TemplateTeamContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Templates</u>  > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Template") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Template'),
                            '<%= ViewData["Scope"] %>Template', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>TemplateTeamContent');
                        }
                        }
                    },
                        { contentEl: '<%= ViewData["Scope"] %>TemplateUserContent', title: 'User', id: '<%= ViewData["Scope"] %>TemplateUserContent',
                            listeners: { activate: function() {
                                document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Templates</u>  > User";
                                loadDetailList('<%= Url.Action("GetDetails", "Template") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Template'),
                            '<%= ViewData["Scope"] %>Template', '<%= (int) DetailType.User %>', '#<%= ViewData["Scope"] %>TemplateUserContent');
                            }
                            }
                        },
                    { contentEl: '<%= ViewData["Scope"] %>TemplateDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>TemplateDiscussionContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Templates</u>  > Discussion";
                            loadDetailList('<%= Url.Action("GetDetails", "Template") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'Template'),
                            '<%= ViewData["Scope"] %>Template', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>TemplateDiscussionContent');
                        } 
                        }
                    }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>TemplateList');
    });
</script>

<asp:Panel ID="TemplateListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>TemplateList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="TemplateFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>TemplateEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TemplateFileContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TemplateTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TemplateUserContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>TemplateDiscussionContent" class="x-hide-display" />
</asp:Panel>
