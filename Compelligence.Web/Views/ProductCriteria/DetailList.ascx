<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ProductCriteriaDetail');
    }).trigger('resize');
</script>
<div id="<%= ViewData["Scope"] %>ProductCriteriaDetailDataFormContent">
    <% Industry industry = (Industry)ViewData["ProductCriteriaIndustry"]; %>
    <% if (industry != null)
       {
    %>
    <div id="TitleSearchResult">
        Criterias by Industry
        <%= Html.Encode(industry.Name) %></div>
    <% } %>
</div>
<br />
<div id="<%= ViewData["Scope"] %>ProductCriteriaDetailDataListContent" > 
    <asp:Panel ID="ProductCriteriaDetailToolbarContent" runat="server" CssClass="buttonLink">
        <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "ProductCriteria") + "', '" + ViewData["Scope"] + "', 'ProductCriteria', 'ProductCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
        <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("EditProductCriteria", "ProductCriteria") + "', '" + ViewData["Scope"] + "', 'ProductCriteria', 'ProductCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteEntity('" + Url.Action("DeleteProductCriteria", "ProductCriteria") + "', '" + ViewData["Scope"] + "', 'ProductCriteria', 'ProductCriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'ProductCriteria', 'ProductCriteriaDetail');" />
        <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'ProductCriteria', 'ProductCriteriaDetail');" />
        <input type="button" value="Cancel" onclick="javascript: loadDetailList('<%= Url.Action("GetDetails", "Product") %>', getIdValue('<%= ViewData["Scope"] %>'), '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>IndustryContent');" />
        <span id="<%= ViewData["Scope"] %>ProductCriteriaDetailAllSelectedOption" class="selectedOption"></span>
    </asp:Panel>
    <asp:Panel ID="ProductCriteriaDetailDataListContent" runat="server" CssClass="contentDetailData">
        <%= Html.DataGrid("ProductCriteriaDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
    </asp:Panel>
</div>
<asp:Panel ID="ProductCriteriaDetailFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>ProductCriteriaEditFormContent"></div>
</asp:Panel>
<asp:Panel ID="ProductCriteriaFilterContent" runat="server">
    <%= Html.GridFilter("ProductCriteriaDetail")%>
</asp:Panel>
<asp:Panel ID="ProductSearchContent" runat="server">
    <%= Html.GridSearch("ProductCriteriaDetail")%>
</asp:Panel>
