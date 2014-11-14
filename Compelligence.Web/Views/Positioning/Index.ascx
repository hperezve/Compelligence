<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
	    $(function() {
	        PositioningSubtabs = new Ext.TabPanel({
                renderTo: 'PositioningContent',
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
                    {contentEl:'<%= ViewData["Scope"] %>PositioningEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>PositioningEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Positionings</u> > Header";
                        }
                        }
                    }
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>PositioningList');
	    });
</script>
<asp:Panel ID="PositioningListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>PositioningList"  class="indexOne">
    <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="PositioningFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>PositioningEditFormContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>DetailPositioning" class="x-hide-display heightSubPanels">List of Positioning</div>
</asp:Panel>