<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        // Industry subtabs
	        CriteriaSetSubtabs = new Ext.TabPanel({
                renderTo: 'CriteriaSetContent',
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
                    {contentEl:'<%= ViewData["Scope"] %>CriteriaSetEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>CriteriaSetEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Criterias</u> > <u>Criteria Set</u> > Header";
                        }
                        }
                    }
                    
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>CriteriaSetList');
	    });
</script>

<asp:Panel ID="CriteriaSetListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CriteriaSetList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="CriteriaSetFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CriteriaSetEditFormContent" class="x-hide-display" />
</asp:Panel>
