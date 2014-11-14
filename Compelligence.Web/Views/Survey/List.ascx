<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Quiz>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'SurveyAll');
    }).trigger('resize');
</script>
<asp:Panel ID="SurveyToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Survey") %>', '<%= ViewData["Scope"] %>', 'Survey', 'SurveyAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Survey") %>', '<%= ViewData["Scope"] %>', 'Survey', 'SurveyAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Survey','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Survey") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Survey','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Survey") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Survey', 'SurveyAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Survey', 'SurveyAll');" />
    <input id="<%= ViewData["Scope"] %>SurveyCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'SurveyAll', 'QuizAllView', 'AssignedTo','<%= Session["UserId"] %>' );" />
    <label for="<%= ViewData["Scope"] %>SurveyCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="SurveyCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Survey','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Survey %>');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>SurveyAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="SurveyDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow">
        <%= Html.DataGrid("SurveyAll", "Survey", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="SurveySearchContent" runat="server">
    <%= Html.GridSearch("SurveyAll") %>
</asp:Panel>
<asp:Panel ID="SurveyFilterContent" runat="server">
    <%= Html.GridFilter("SurveyAll") %>
</asp:Panel>