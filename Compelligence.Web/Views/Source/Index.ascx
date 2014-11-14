<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        // Source subtabs
	        SourceSubtabs = new Ext.TabPanel({
                renderTo: 'SourceContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: { 
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    {contentEl:'<%= ViewData["Scope"] %>SourceEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>SourceEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Research</u> > <u>Source</u>  > Header";
                        }
                        }
                    }
                    
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>SourceList');
	    });
</script>

<asp:Panel ID="SourceListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SourceList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="SourceFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SourceEditFormContent" class="x-hide-display" />
</asp:Panel>
