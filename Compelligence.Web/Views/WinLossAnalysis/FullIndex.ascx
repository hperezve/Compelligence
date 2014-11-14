<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
        WinLossAnalysisSubtabs = new Ext.TabPanel({
            renderTo: 'WinLossAnalysisContent',
            autoWidth: true,
            frame: true,
            //defaults:{autoHeight: true},
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                      { contentEl: '<%= ViewData["Scope"] %>WinLossAnalysisEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>WinLossAnalysisEditFormContent',
                          listeners: { activate: function() {
                          document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Win/Loss Analysis</u>  > Header";
                          }
                          }
                      }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>WinLossAnalysisList');
    });
</script>



<asp:Panel ID="WinLossAnalysisListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>WinLossAnalysisList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS">
        <img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>" /></div>
</asp:Panel>
<br />
<asp:Panel ID="WinLossAnalysisFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>WinLossAnalysisEditFormContent" class="x-hide-display" />
</asp:Panel>
