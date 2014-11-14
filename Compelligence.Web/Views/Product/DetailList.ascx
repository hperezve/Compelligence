<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>ProductDetailDataListContent" class="absolute">
        <asp:Panel ID="ProductDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Product") + "', '" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            
             <% string scopeValueToTeam = ViewData["Scope"].ToString();
                if (!string.IsNullOrEmpty(scopeValueToTeam))
                {
                    if (scopeValueToTeam.IndexOf("Team") != -1)
                    {
            %>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Product") + "', '" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseDetailName"] + "', 'ProductDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <% }
                } %>
            
            <% string hasRows = ViewData["HasRows"].ToString();
               if (!string.IsNullOrEmpty(hasRows) && hasRows.Equals("True"))
               { %>
                <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Product") + "', '" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
             <%} %>
            
            <% string scopeValue2 = ViewData["Scope"].ToString();
               if (!string.IsNullOrEmpty(scopeValue2))
               {
                   if (scopeValue2.IndexOf("Industry") != -1)
                   {
            %>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Product") + "', '" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseDetailName"] + "', 'ProductIndustryDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
              <%}
                   else if (scopeValue2.IndexOf("Competitor") != -1)
                   {%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Product") + "', '" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseDetailName"] + "', 'ProductDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <% }
               }%>
            
            <% if (!string.IsNullOrEmpty(hasRows) && hasRows.Equals("True"))
               { %>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Product") + "', '" + ViewData["Scope"] + "', 'Product', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <% string scopeValue = ViewData["Scope"].ToString();
               if (!string.IsNullOrEmpty(scopeValue))
               {
                   if (scopeValue.IndexOf("Industry") != -1)
                   {
            %>
            <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'ProductIndustry', 'ProductIndustryDetail');" />
            <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'ProductIndustry', 'ProductIndustryDetail');" />
            <%}
                   else if (scopeValue.IndexOf("Competitor") != -1)
                   {%>
            <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'ProductCompetitor', 'ProductCompetitorDetail');" />
            <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'ProductCompetitor', 'ProductCompetitorDetail');" />
            <% }
               }
               } %>
        </asp:Panel>
        <asp:Panel ID="ProductDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="ProductDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ProductEditFormContent" />
    </asp:Panel>
    <% string scopeValue = ViewData["Scope"].ToString();
       if (!string.IsNullOrEmpty(scopeValue))
       {
           if (scopeValue.IndexOf("Industry") != -1)
           {
    %>
    <asp:Panel ID="ProductIndustryDetailSearchContent" runat="server">
        <%= Html.GridSearch("ProductIndustryDetail")%>
    </asp:Panel>
    <asp:Panel ID="ProductIndustryDetailFilterContent" runat="server">
        <%= Html.GridFilter("ProductIndustryDetail") %>
    </asp:Panel>
    <%}
           else if (scopeValue.IndexOf("Competitor") != -1)
           {%>
    <asp:Panel ID="ProductCompetitorDetailSearchContent" runat="server">
        <%= Html.GridSearch("ProductCompetitorDetail")%>
    </asp:Panel>
    <asp:Panel ID="ProductCompetitorDetailFilterContent" runat="server">
        <%= Html.GridFilter("ProductCompetitorDetail") %>
    </asp:Panel>
    <% }
       } %>
</div>