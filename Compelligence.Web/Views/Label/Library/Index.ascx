<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    // Library subtabs
	        LibrarySubtabs = new Ext.TabPanel({
                renderTo: 'LibraryContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>LibraryEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>LibraryEditFormContent' }
                ]
            });
	    });
</script>

<asp:Panel ID="LibraryListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryList" />
    <% Html.RenderPartial("List"); %>
</asp:Panel>
<br />
<asp:Panel ID="LibraryFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryEditFormContent" class="x-hide-display" />
</asp:Panel>
