<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	            ProjectsApprovalSubtabs = new Ext.TabPanel({
	            renderTo: '<%= ViewData["Scope"] %>ProjectsApprovalContent',
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
                    {
                        contentEl: '<%= ViewData["Scope"] %>ProjectsApprovalFormContent',
                        title: 'Header',
                        id: '<%= ViewData["Scope"] %>ProjectsApprovalEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Admin</u> > <u>Configurations</u> > <u>Approve Projects</u> > Header";
                        }}}
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>ProjectsApprovalList');
	    });
</script>

<asp:Panel ID="ProjectsApprovalListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProjectsApprovalList" class="indexOne">
        <% Html.RenderPartial("ApprovalList"); %>
    </div>
</asp:Panel>
<br />
<asp:Panel ID="ProjectsApprovalFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProjectsApprovalEditFormContent" class="x-hide-display" />
</asp:Panel>


