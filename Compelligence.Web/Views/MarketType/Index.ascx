<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
	    $(function() {
	        // MarketType subtabs
	        MarketTypeSubtabs = new Ext.TabPanel({
                renderTo: 'MarketTypeContent',
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
                    {contentEl:'<%= ViewData["Scope"] %>MarketTypeEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>MarketTypeEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>MarketType</u> > Header";
                        }}}
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>MarketTypeList');
	    });
</script>

<asp:Panel ID="MarketTypeListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>MarketTypeList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
     <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="MarketTypeFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>MarketTypeEditFormContent" class="x-hide-display" />
</asp:Panel>
