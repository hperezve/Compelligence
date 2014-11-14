<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    $(function() {
        ConfigurationGeneralSubtabs = new Ext.TabPanel({
            renderTo: 'ConfigurationGeneralContent',
            id: 'AdminConfigurationGeneralContentPanel',
            height: 640,
            autoWidth: true,
            enableTabScroll: true,
            frame: true,
            defaults: { autoHeight: true },
            height: 640,
            items: [
                {
                    contentEl: '<%= ViewData["Scope"] %>ConfigurationGeneralProjectsApprovalContent',
                    title: 'Approve Projects',
                    id: '<%= ViewData["Scope"] %>ConfigurationGeneral_ProjectsApprovalContent',
                    listeners: {
                        activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Configuration</u> > Approve Projects";
                            //loadContent('<%= Url.Action("IndexApproval","Configuration") %>', '#<%= ViewData["Scope"] %>ConfigurationGeneralProjectsApprovalContent', '<%= ViewData["Scope"] %>ConfigurationGeneral');
                            loadContent('<%= Url.Action("IndexConfiguration","Configuration") %>', '#<%= ViewData["Scope"] %>ConfigurationGeneralProjectsApprovalContent', '<%= ViewData["Scope"] %>ConfigurationGeneral');
                        }
                    }
                },
                {
                    contentEl: '<%= ViewData["Scope"] %>ConfigurationGeneralNewsSendEmailContent',
                    title: 'Email',
                    id: '<%= ViewData["Scope"] %>ConfigurationGeneral_NewsSendEmailContent',
                    listeners: {
                        activate: function() {
                            document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Configuration News Mail</u>";
                            loadContent('<%= Url.Action("Test","Configuration") %>', '#<%= ViewData["Scope"] %>ConfigurationGeneralNewsSendEmailContent', '<%= ViewData["Scope"] %>ConfigurationGeneral');
                        }
                    }
                },
                {
                    contentEl: '<%= ViewData["Scope"] %>ConfigurationGeneralUsersToReceiveTopContent',
                    title: 'Users to Receive News',
                    id: '<%= ViewData["Scope"] %>ConfigurationGeneral_UsersToReceiveTopContent',
                    listeners: {
                        activate: function() {
                            document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Users to Receive News</u>";
                            loadContent('<%= Url.Action("UserToReceiveTop","Configuration") %>', '#<%= ViewData["Scope"] %>ConfigurationGeneralUsersToReceiveTopContent', '<%= ViewData["Scope"] %>ConfigurationGeneral');
                        }
                    }
                },
                {
                    contentEl: '<%= ViewData["Scope"] %>ConfigurationGeneralDefaultsContent',
                    title: 'Defaults',
                    id: '<%= ViewData["Scope"] %>ConfigurationGeneral_DefaultsContent',
                    listeners: {
                        activate: function() {
                        document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Configuration Defaults</u>";
                        loadContent('<%= Url.Action("Defaults","Configuration") %>', '#<%= ViewData["Scope"] %>ConfigurationGeneralDefaultsContent', '<%= ViewData["Scope"] %>ConfigurationGeneral');
                        }
                    }
                },
                {
                    contentEl: '<%= ViewData["Scope"] %>ConfigurationGeneralConfigurationLabelsContent',
                    title: 'Labels',
                    id: '<%= ViewData["Scope"] %>ConfigurationGeneral_ConfigurationLabelsContent',
                    listeners: {
                        activate: function() {
                            document.getElementById("breadcrumb").innerHtml = "<u>Admin</u> <u>Configurations</u> <u>Configuration Labels</u>";
                            loadContent('<%= Url.Action("Index","ConfigurationLabels") %>', '#<%= ViewData["Scope"] %>ConfigurationGeneralConfigurationLabelsContent', '<%= ViewData["Scope"] %>ConfigurationGeneral');
                        }
                    }
                }
            ],
            listeners: {
                activeitemchange: function(ConfigurationGeneralSubtabs, value, oldValue) {
                   // alert(ConfigurationGeneralSubtabs.id);
                },
                click: function() {
                    //alert('test');
                }
            }
        });
    });
</script>
<asp:Panel ID="ConfigurationGeneralFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ConfigurationGeneralProjectsApprovalContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>ConfigurationGeneralNewsSendEmailContent" class="x-hide-display heightPanel" />
    <div id="<%= ViewData["Scope"] %>ConfigurationGeneralUsersToReceiveTopContent" class="x-hide-display heightPanel" />
    <div id="<%= ViewData["Scope"] %>ConfigurationGeneralDefaultsContent" class="x-hide-display heightPanel" />
    <div id="<%= ViewData["Scope"] %>ConfigurationGeneralConfigurationLabelsContent" class="x-hide-display heightPanel" />
</asp:Panel>
