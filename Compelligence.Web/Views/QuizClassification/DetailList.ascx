<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'QuizClassificationDetail');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>QuizClassificationDetailDataListContent" class="absolute">
        <asp:Panel ID="QuizClassificationDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "QuizClassification") + "', '" + ViewData["Scope"] + "', 'QuizClassification', 'QuizClassificationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "QuizClassification") + "', '" + ViewData["Scope"] + "', 'QuizClassification', 'QuizClassificationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("Delete", "QuizClassification") + "', '" + ViewData["Scope"] + "', 'QuizClassification', 'QuizClassificationDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="QuizClassificationDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("QuizClassificationDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="QuizClassificationDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>QuizClassificationEditFormContent" />
    </asp:Panel>
</div>