<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');
</script>
<div >
    <div id="<%= ViewData["Scope"] %>CompetitorDetailDataListContent" >
        <asp:Panel ID="CompetitorDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Competitor") + "', '" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Competitor")+ "', '" + ViewData["Scope"]+ "', 'Competitor', '" + ViewData["BrowseDetailName"]+ "', 'CompetitorDetailSelect', '" + ViewData["HeaderType"]+ "', '" + ViewData["DetailFilter"]+ "');" })%>            
            <% string hasRows = ViewData["HasRows"].ToString();
               if (!string.IsNullOrEmpty(hasRows) && hasRows.Equals("True"))
               { %>
                 <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Competitor") + "', '" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
             <%} %>
        </asp:Panel>
        <asp:Panel ID="CompetitorDetailDataListContent" runat="server" >
            <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="CompetitorDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>CompetitorEditFormContent" />
    </asp:Panel>
</div>
