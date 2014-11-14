<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        // Criteria subtabs
	        CriteriaSubtabs = new Ext.TabPanel({
                renderTo: 'CriteriaContent',
                autoWidth:true,
                frame:true,
                //defaults:{autoHeight: true},
                height:640,
                listeners: {  
                       render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    {contentEl:'<%= ViewData["Scope"] %>CriteriaEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>CriteriaEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Criterias</u> > <u>Criteria</u> > Header";
                        }
                        }
                    }
                    
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>CriteriaList');
	    });
</script>

<asp:Panel ID="CriteriaListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CriteriaList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="CriteriaFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CriteriaEditFormContent" class="x-hide-display" />
</asp:Panel>
