<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        CustomFieldSubtabs = new Ext.TabPanel({
                renderTo: 'CustomFieldContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>CustomFieldEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>CustomFieldEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Custom Field</u>  > Header";
                        }
                        }
                     },
                       ]
            });
            resizeContent('#<%= ViewData["Scope"] %>CustomFieldList');
	    });
</script>

<asp:Panel ID="CustomFieldListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CustomFieldList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="CustomFieldFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CustomFieldEditFormContent" class="x-hide-display" />
</asp:Panel>
