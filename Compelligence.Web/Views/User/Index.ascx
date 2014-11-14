<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    UserSubtabs = new Ext.TabPanel({
                renderTo: 'UserContent',
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
                    { contentEl: '<%= ViewData["Scope"] %>UserEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>UserEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Users</u>  > Header";
                        }
                        }
                    },

                    { contentEl: '<%= ViewData["Scope"] %>UserOwnershipContent', title: 'Ownership', id: '<%= ViewData["Scope"] %>UserOwnershipContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Users</u> > Ownership";
                        loadDetailList('<%= Url.Action("GetDetails", "User") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'User'),
                            '<%= ViewData["Scope"] %>User', '<%= (int) DetailType.UserRelation %>', '#<%= ViewData["Scope"] %>UserOwnershipContent');                                                      
                          
                        }
                        }
                    },

                    { contentEl: '<%= ViewData["Scope"] %>UserHistoryFieldContent', title: 'History', id: '<%= ViewData["Scope"] %>UserHistoryFieldContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Users</u> > History";
                            loadDetailList('<%= Url.Action("GetDetails", "User") %>', getCurrentHeaderId('<%= ViewData["Scope"] %>', 'User'),
                            '<%= ViewData["Scope"] %>User', '<%= (int) DetailType.HistoryField %>', '#<%= ViewData["Scope"] %>UserHistoryFieldContent');
                        }
                        }
                    }

                     
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>UserList');
	    });
</script>


<asp:Panel ID="UserListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>UserList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="UserFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>UserEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>UserOwnershipContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>UserHistoryFieldContent" class="x-hide-display heightSubPanels" />
</asp:Panel>
