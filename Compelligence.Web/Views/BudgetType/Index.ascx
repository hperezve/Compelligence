<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        BudgetTypeSubtabs = new Ext.TabPanel({
                renderTo: 'BudgetTypeContent',
                autoWidth:true,
                frame: true,
                height: 640,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>BudgetTypeEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>BudgetTypeEditFormContent' },
                       ]
            });
            resizeContent('#<%= ViewData["Scope"] %>BudgetTypeList');
	    });
</script>

<asp:Panel ID="BudgetTypeListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>BudgetTypeList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="BudgetTypeFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>BudgetTypeEditFormContent" class="x-hide-display" />
</asp:Panel>
