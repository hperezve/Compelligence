<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'TeamMemberDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>TeamMemberDetailDataListContent"  class="absolute">
        <asp:Panel ID="TeamMemberDetailToolbarContent" runat="server" CssClass="buttonLink">
            <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "User") %>', '<%= ViewData["Scope"] %>', 'TeamMember', 'TeamMemberDetail','<%= ViewData["Container"] %>','<%= ViewData["HeaderType"] %>','<%= ViewData["DetailFilter"] %>');" />
            <input class="button" type="button" value="Add" onclick="javascript: newEntity('<%= Url.Action("Create", "TeamMember") %>', '<%= ViewData["Scope"] %>', 'TeamMember', 'TeamMemberDetail','<%= ViewData["Container"] %>','<%= ViewData["HeaderType"] %>','<%= ViewData["DetailFilter"] %>');" />
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "TeamMember") + "', '" + ViewData["Scope"] + "', 'TeamMember', 'TeamMemberDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "TeamMember") + "', '" + ViewData["Scope"] + "', 'TeamMember', 'TeamMemberDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "TeamMember") + "', '" + ViewData["Scope"] + "', 'TeamMember', 'TeamMemberDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("Duplicate", "TeamMember") + "', '" + ViewData["Scope"] + "', 'TeamMember', 'TeamMemberDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="TeamMemberDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("TeamMemberDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="TeamMemberDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>TeamMemberEditFormContent" />
    </asp:Panel>
</div>