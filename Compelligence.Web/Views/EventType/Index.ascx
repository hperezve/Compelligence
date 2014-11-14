<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        EventTypeSubtabs = new Ext.TabPanel({
                renderTo: 'EventTypeContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>EventTypeEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>EventTypeEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Event Type</u>  > Header";
                        }
                        }
                     },
                       ]
            });
            resizeContent('#<%= ViewData["Scope"] %>EventTypeList');
	    });
</script>

<asp:Panel ID="EventTypeListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>EventTypeList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="EventTypeFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>EventTypeEditFormContent" class="x-hide-display" />
</asp:Panel>
