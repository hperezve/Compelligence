<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        LibrarySubtabs = new Ext.TabPanel({
                renderTo: 'LibraryContent',
                autoWidth:true,
                frame:true,
                autoHeight: true,
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>LibraryEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>LibraryEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Libraries</u> > Header";
                        }
                        }
                    },
//                    { contentEl: '<%= ViewData["Scope"] %>LibraryShowListContent', title: 'Content', id: '<%= ViewData["Scope"] %>LibraryShowListContent',
//                        listeners: { activate: function() 
//                        {
//                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Libraries</u> > Content";
//                            loadDetailList('<%= Url.Action("GetDetails", "Library") %>', 
//                            getIdValue('<%= ViewData["Scope"] %>', 'Library'),
//                            '<%= ViewData["Scope"] %>Library', '<%= (int) DetailType.Library %>', 
//                            '#<%= ViewData["Scope"] %>LibraryShowListContent');
//                        }
//                        }
//                    },
                    { contentEl: '<%= ViewData["Scope"] %>LibraryShowListOwners', title: 'Owners', id: '<%= ViewData["Scope"] %>LibraryShowListOwners',
                        listeners: { activate: function() 
                        {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Libraries</u> > Owners";
                            loadDetailList('<%= Url.Action("GetDetails", "Library") %>', 
                            getIdValue('<%= ViewData["Scope"] %>', 'Library'),
                            '<%= ViewData["Scope"] %>Library', '<%= (int) DetailType.EntityRelation %>',
                            '#<%= ViewData["Scope"] %>LibraryShowListOwners');
                        }
                        }
                    }                    
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>LibraryList');
	    });
</script>

<asp:Panel ID="LibraryListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="LibraryFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>LibraryEditFormContent" class="x-hide-display" ></div>
    <div id="<%= ViewData["Scope"] %>LibraryShowListOwners" class="x-hide-display" ></div>
</asp:Panel>
