<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
        // Website subtabs
        WebsiteSubtabs = new Ext.TabPanel({
            renderTo: 'WebsiteContent',
            autoWidth: true,
            frame: true,
            //defaults: { autoHeight: true },
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items:
                [
                    { contentEl: '<%= ViewData["Scope"] %>WebsiteEditFormContent',
                        title: 'Header',
                        id: '<%= ViewData["Scope"] %>WebsiteEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Website</u>  > Header";
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>WebsiteDetailFormContent',
                        title: 'Detail',
                        id: '<%= ViewData["Scope"] %>WebsiteDetailFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Website</u>  > Detail";

                            loadContent('<%= Url.Action("EditDetail", "Website") %>', '#<%= ViewData["Scope"] %>WebsiteDetailFormContent', 'Admin');

                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>WebsitePreviewFormContent',
                        title: 'Preview',
                        id: '<%= ViewData["Scope"] %>WebsitePreviewFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Website</u>  > Preview";

                            loadContent('<%= Url.Action("EditPreview", "Website") %>', '#<%= ViewData["Scope"] %>WebsitePreviewFormContent', 'Admin');

                        }
                        }
                    },
                   { contentEl: '<%= ViewData["Scope"] %>WebsitePanelFormContent',
                       title: 'Panels',
                       id: '<%= ViewData["Scope"] %>WebsitePanelFormContent',
                       listeners: { activate: function() {
                           document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Website</u>  > Panel";
                           var id = getIdValue('<%= ViewData["Scope"] %>', 'Website');
                           loadContent('<%= Url.Action("EditPanel", "Website") %>'+'?websiteid='+id, '#<%= ViewData["Scope"] %>WebsitePanelFormContent', 'Admin');

                       }
                       }
                   }



                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>WebsiteList');
    });
</script>

<asp:Panel ID="WebsiteListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>WebsiteList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="WebsiteFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>WebsiteEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>WebsiteDetailFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>WebsitePreviewFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>WebsitePanelFormContent" class="x-hide-display" />
</asp:Panel>
