<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'SurveyDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>SurveyDetailDataListContent" class="absolute">
        <asp:Panel ID="SurveyDetailToolbarContent" runat="server" CssClass="buttonLink">
            <input class="button" type="button" value="Add" onclick="javascript: addEntity('<%= Url.Action("CreateDetail", "Survey") %>', '<%= ViewData["Scope"] %>', 'Survey', 'SurveyDetail', 'SurveyDetailSelect', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Survey") + "', '" + ViewData["Scope"] + "', 'Survey', 'SurveyDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Survey") + "', '" + ViewData["Scope"] + "', 'Survey', 'SurveyDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "Survey") + "', '" + ViewData["Scope"] + "', 'Survey', 'SurveyDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "Survey") + "', '" + ViewData["Scope"] + "', 'Survey', 'SurveyDetail', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="SurveyDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("SurveyDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="SurveyDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>SurveyEditFormContent" />
    </asp:Panel>
</div>