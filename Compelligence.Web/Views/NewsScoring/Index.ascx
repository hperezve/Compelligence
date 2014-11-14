<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
	    $(function() {
	        NewsScoringSubtabs = new Ext.TabPanel({
                renderTo: 'NewsScoringContent',
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
                    {contentEl:'<%= ViewData["Scope"] %>NewsScoringEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>NewsScoringEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>News Scoring</u> > Header";
                        }
                        }
                    }    
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>NewsScoringList');
	    });
</script>
<asp:Panel ID="NewsScoringListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NewsScoringList"  class="indexOne">
    <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="NewsScoringFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NewsScoringEditFormContent" class="x-hide-display heightSubPanels" />
</asp:Panel>