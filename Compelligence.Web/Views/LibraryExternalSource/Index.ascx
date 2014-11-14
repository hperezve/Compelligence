<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    LibraryExternalSourceSubtabs = new Ext.TabPanel({
                renderTo: 'LibraryExternalSourceContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>LibraryExternalSourceEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>LibraryExternalSourceEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Library External Source</u>  > Header";
                        }
                        }
                     }
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>LibraryExternalSourceList');
	    });
</script>

<asp:Panel ID="LibraryExternalSourceListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryExternalSourceList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="LibraryExternalSourceFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryExternalSourceEditFormContent" class="x-hide-display" />
</asp:Panel>
