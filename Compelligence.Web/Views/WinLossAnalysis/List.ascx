<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.WinLossAnalysis>>" %>
<script type="text/javascript">
    function executeLink(url) {
        showLoadingDialog();
        $.get(url, {}, function(data) {
            hideLoadingDialog();
        });
    }
</script>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'WinLossAnalysisAll');
    }).trigger('resize');
</script>
<asp:Panel ID="WinLossAnalysisToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "WinLossAnalysis") %>', '<%= ViewData["Scope"] %>', 'WinLossAnalysis', 'WinLossAnalysisAll','<%= ViewData["Container"] %>');" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('WinLossAnalysis','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "WinLossAnalysis") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('WinLossAnalysis','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "WinLossAnalysis") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity ('<%= ViewData["Scope"] %>', 'WinLossAnalysis', 'WinLossAnalysisAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity ('<%= ViewData["Scope"] %>', 'WinLossAnalysis', 'WinLossAnalysisAll');" />
    <input class="button" type="button" value="Execute" onclick = "javascript: executeLink('<%=Url.Action("ExecuteReader", "WinlossAnalysis")%>')" />
    <input id="<%= ViewData["Scope"] %>WinLossAnalysisCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'WinLossAnalysisAll', 'WinLossAnalysisAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    

    <label for="<%= ViewData["Scope"] %>WinLossAnalysisCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="WinLossAnalysisCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <span id="<%= ViewData["Scope"] %>WinLossAnalysisAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="WinLossAnalysisDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("WinLossAnalysisAll", "WinLossAnalysis", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="WinLossAnalysisSearchContent" runat="server">
    <%= Html.GridSearch("WinLossAnalysisAll") %>
</asp:Panel>
<asp:Panel ID="WinLossAnalysisFilterContent" runat="server">
    <%= Html.GridFilter("WinLossAnalysisAll") %>
</asp:Panel>