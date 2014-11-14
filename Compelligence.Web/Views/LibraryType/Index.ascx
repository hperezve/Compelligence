<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        LibraryTypeSubtabs = new Ext.TabPanel({
                renderTo: 'LibraryTypeContent',
                autoWidth:true,
                frame:true,
                defaults:{autoHeight: true},
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>LibraryTypeEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>LibraryTypeEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Library Type</u>  > Header";
                        }
                        }
                     },
                       ]
            });
            resizeContent('#<%= ViewData["Scope"] %>LibraryTypeList');
	    });
</script>

<asp:Panel ID="LibraryTypeListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryTypeList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="LibraryTypeFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryTypeEditFormContent" class="x-hide-display" />
</asp:Panel>
