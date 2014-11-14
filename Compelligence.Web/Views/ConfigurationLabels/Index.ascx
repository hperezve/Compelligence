<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        ConfigurationLabelsSubtabs = new Ext.TabPanel({
	        renderTo: '<%= ViewData["Scope"] %>ConfigurationLabelsContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>ConfigurationLabelsEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>ConfigurationLabelsEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Configurations</u> > <u>Labels</u>  > Header";
                        }
                        }
                     },
                       ]
            });
            resizeContent('#<%= ViewData["Scope"] %>ConfigurationLabelsList');
	    });
</script>

<asp:Panel ID="ConfigurationLabelsListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ConfigurationLabelsList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="ConfigurationLabelsFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ConfigurationLabelsEditFormContent" class="x-hide-display" />
</asp:Panel>
