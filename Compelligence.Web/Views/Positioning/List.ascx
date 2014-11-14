<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Positioning>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'PositioningAll');
    }).trigger('resize');
</script>
<asp:Panel ID="PositioningToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Positioning") %>', '<%= ViewData["Scope"] %>', 'Positioning', 'PositioningAll','<%= ViewData["Container"] %>');" />
    <input class="button" type="button" value="Edit" onclick="javascript: editPositioning('<%= Url.Action("Edit", "Positioning") %>', '<%= ViewData["Scope"] %>', 'Positioning', 'PositioningAll')" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperationByPositioning('Positioning','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Positioning") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByPositioning('Positioning','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Positioning") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Positioning', 'PositioningAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Positioning', 'PositioningAll');" />
    <input class="button" type="button" value="Select Industry" onclick="javascript: addEntityToPositioning('<%= Url.Action("GetPositioningByCompetitor", "Positioning") %>', '<%= ViewData["Scope"] %>', 'Positioning', 'PositioningAll', 'PositioningAllByHierarchy', 'IndustryDetailSelect', '<%= ViewData["HeaderType"]%>', '<%=  ViewData["DetailFilter"]%>', 'INDTR');" />
    <input class="button" type="button" value="Select Competitor" onclick="javascript: addEntityToPositioning('<%= Url.Action("GetPositioningByCompetitor", "Positioning") %>', '<%= ViewData["Scope"] %>', 'Positioning', 'PositioningAll', 'PositioningAllByHierarchy', 'CompetitorDetailSelect', '<%= ViewData["HeaderType"]%>', '<%=  ViewData["DetailFilter"]%>', 'COMPT');" />
    <input class="button" type="button" value="Select Product" onclick="javascript: addEntityToPositioning('<%= Url.Action("GetPositioningByCompetitor", "Positioning") %>', '<%= ViewData["Scope"] %>', 'Positioning', 'PositioningAll', 'PositioningAllByHierarchy', 'ProductDetailSelect', '<%= ViewData["HeaderType"]%>', '<%=  ViewData["DetailFilter"]%>', 'PRODT');" />
    <input id="<%= ViewData["Scope"] %>PositioningHierarchyCheckbox" class="checkbox" type="checkbox" onclick="javascript:showPositioningByHierarchy(this,'<%= ViewData["Scope"] %>', 'PositioningAll', 'PositioningAllByHierarchy', 'CTIV');" />
    <label for="<%= ViewData["Scope"] %>PositioningHierarchyCheckbox" style="font-size: 12px;
        color: black;">
        <asp:Literal ID="PositioningHierarchyCheckbox" runat="server" Text="<%$ Resources:LabelResource, CompetitiveMessagingHierarchyChecked %>" />
    </label>
    <input id="<%= ViewData["Scope"] %>CompetitiveHierarchyCheckbox" class="checkbox" type="checkbox" onclick="javascript:showPositioningByHierarchy(this,'<%= ViewData["Scope"] %>', 'PositioningAll','PositioningAllByHierarchy', 'PSTN');" />
    <label for="<%= ViewData["Scope"] %>CompetitiveHierarchyCheckbox" style="font-size: 12px;
        color: black;">
        <asp:Literal ID="CompetitiveHierarchyCheckbox" runat="server" Text="<%$ Resources:LabelResource, PositioningMessagingHierarchyChecked  %>" />
    </label>
    <input id="<%= ViewData["Scope"] %>ShowMasterCheckbox" class="checkbox" type="checkbox" onclick="javascript:showPositioningMaster(this,'<%= ViewData["Scope"] %>', 'PositioningAll','PositioningAllByHierarchy', 'PSTN');" />
    <label for="<%= ViewData["Scope"] %>ShowMasterCheckbox" style="font-size: 12px;
        color: black;">
        <asp:Literal ID="ShowMasterCheckbox" runat="server" Text="<%$ Resources:LabelResource, PositioningShowMasterChecked  %>" />
    </label>
    <span id="<%= ViewData["Scope"] %>PositioningAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="PositioningDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridPositioningListOverflow" class="gridOverflow" style="display: block">
        <%= Html.DataGridPositioning("PositioningAll", "PositioningAllByHierarchy", "Positioning", ViewData["Container"].ToString(), null, null)%></div>
    <div id="gridPositioningHierarchyOverflow" class="gridOverflow" style="display: none">
        <%= Html.DataGridPositioning("PositioningAllByHierarchy", "PositioningAll", "Positioning", ViewData["Container"].ToString(), null, null)%></div>
</asp:Panel>
<asp:Panel ID="PositioningSearchContent" runat="server">
    <%= Html.GridSearch("PositioningAll") %>
</asp:Panel>
<asp:Panel ID="PositioningFilterContent" runat="server">
    <%= Html.GridFilter("PositioningAll") %>
</asp:Panel>