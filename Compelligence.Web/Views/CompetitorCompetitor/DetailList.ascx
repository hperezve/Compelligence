<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>CompetitorCompetitorDetailDataListContent" class="absolute">
        <asp:Panel ID="CompetitorCompetitorDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "CompetitorCompetitor") + "', '" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseDetailName"] + "', 'CompetitorDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <% string hasRows = ViewData["HasRows"].ToString();
               if (!string.IsNullOrEmpty(hasRows) && hasRows.Equals("True"))
                { %>
                <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "CompetitorCompetitor") + "', '" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%  } %>
        </asp:Panel>
        <asp:Panel ID="CompetitorCompetitorDetailDataListContent" runat="server" CssClass="contentDetailData" >
            <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="CompetitorCompetitorDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>CompetitorCompetitorEditFormContent" />
    </asp:Panel>
</div>