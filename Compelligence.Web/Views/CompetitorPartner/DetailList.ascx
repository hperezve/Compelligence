<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>PartnerDetailDataListContent" class="absolute">
        <asp:Panel ID="PartnerDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "CompetitorPartner") + "', '" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseDetailName"] + "', 'PartnerDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Partner") + "', '" + ViewData["Scope"] + "', 'Partner', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Partner") + "', '" + ViewData["Scope"] + "', 'Partner', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "CompetitorPartner") + "', '" + ViewData["Scope"] + "', 'Competitor', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "Partner") + "', '" + ViewData["Scope"] + "', 'Partner', '" + ViewData["BrowseDetailName"] + "',  '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
        </asp:Panel>
        <asp:Panel ID="PartnerDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="PartnerDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>PartnerEditFormContent" />
    </asp:Panel>
</div>