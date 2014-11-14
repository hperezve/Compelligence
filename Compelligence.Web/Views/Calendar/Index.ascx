<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    // Calendar subtabs
	        CalendarSubtabs = new Ext.TabPanel({
                renderTo: 'CalendarContent',
                autoWidth:true,
                frame:true,
                height: 640,
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>CalendarEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>CalendarEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Calendar</u> > Header";
                        }
                        }
                     }   
                     
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>CalendarList');
	    });
</script>
 <script src="<%= Url.Content("~/Scripts/System/BackEnd/Functions.js") %>" type="text/javascript"></script>
<asp:Panel ID="CalendarListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CalendarList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="CalendarFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CalendarEditFormContent" class="x-hide-display" />
</asp:Panel>
