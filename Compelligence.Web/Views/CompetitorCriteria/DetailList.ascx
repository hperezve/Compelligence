<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorCriteriaDetail');
}).trigger('resize');
function inizializeType() {
    $('#Type').val("List");   
    }
</script>
<div id="<%= ViewData["Scope"] %>CompetitorCriteriaDetailDataFormContent">
    <% Industry industry = (Industry)ViewData["CompetitorCriteriaIndustry"]; %>
    <% if (industry != null)
       {
    %>
    <div id="TitleSearchResult">
        Criterias by Industry
        <%= Html.Encode(industry.Name) %></div>
    <% } %>
</div>
<br />
<div id="<%= ViewData["Scope"] %>CompetitorCriteriaDetailDataListContent">
    <% if (ViewData["UserSecurityAccess"].ToString().Equals(UserSecurityAccess.Edit))
       { %>
    <asp:Panel ID="CompetitorCriteriaDetailToolbarContent" runat="server" CssClass="buttonLink">
         <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "CompetitorCriteria") + "', '" + ViewData["Scope"] + "', 'CompetitorCriteria', 'CompetitorCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');inizializeType();" })%>
         <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("EditCompetitorCriteria", "CompetitorCriteria") + "', '" + ViewData["Scope"] + "', 'CompetitorCriteria', 'CompetitorCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
         <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteEntity('" + Url.Action("DeleteCompetitorCriteria", "CompetitorCriteria") + "', '" + ViewData["Scope"] + "', 'CompetitorCriteria', 'CompetitorCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
         <input class="button" type="button" value="Filter" onclick="javascript: filterEntity ('<%= ViewData["Scope"] %>', 'CompetitorCriteria', 'CompetitorCriteriaDetail');" />  
         <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'CompetitorCriteria', 'CompetitorCriteriaDetail');" />       
         <input type="button" value="Cancel" onclick="javascript: loadDetailList('<%= Url.Action("GetDetails", "Competitor") %>', getIdValue('<%= ViewData["Scope"] %>'), '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>IndustryContent');" />
         <span id="<%= ViewData["Scope"] %>CompetitorCriteriaDetailAllSelectedOption" class="selectedOption"></span>
    </asp:Panel>
    <% } %>
    <asp:Panel ID="CompetitorCriteriaDetailDataListContent" runat="server" CssClass="contentDetailData">
        <%= Html.DataGrid("CompetitorCriteriaDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
    </asp:Panel>
</div>
<asp:Panel ID="CompetitorCriteriaDetailFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CompetitorCriteriaEditFormContent" />
</asp:Panel>
<asp:Panel ID="CompetitorFilterContent" runat="server">
    <%= Html.GridFilter("CompetitorCriteriaDetail")%>
</asp:Panel>
<asp:Panel ID="CompetitorSearchContent" runat="server">
    <%= Html.GridSearch("CompetitorCriteriaDetail")%>
</asp:Panel>
