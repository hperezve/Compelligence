<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    ContentTypeSubtabs = new Ext.TabPanel({
                renderTo: 'ContentTypeContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>ContentTypeEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>ContentTypeEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Content Type</u>  > Header";
                        }
                        }
                     }
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>ContentTypeList');
	    });
</script>

<asp:Panel ID="ContentTypeListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ContentTypeList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="ContentTypeFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ContentTypeEditFormContent" class="x-hide-display" />
</asp:Panel>
