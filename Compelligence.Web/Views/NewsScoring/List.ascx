<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.NewsScoring>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'NewsScoringAll');
    }).trigger('resize');
</script>
<asp:Panel ID="NewsScoringToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "NewsScoring") %>', '<%= ViewData["Scope"] %>', 'NewsScoring', 'NewsScoringAll','<%= ViewData["Container"] %>');" />
    <input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "NewsScoring") %>', '<%= ViewData["Scope"] %>', 'NewsScoring', 'NewsScoringAll')" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('NewsScoring','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "NewsScoring") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('NewsScoring','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "NewsScoring") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'NewsScoring', 'NewsScoringAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'NewsScoring', 'NewsScoringAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.NewsScoring %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Tools:News Scoring');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>NewsScoringAllSelectedOption" class="selectedOption"></span>
   
</asp:Panel>
<asp:Panel ID="NewsScoringDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow"><%= Html.DataGrid("NewsScoringAll", "NewsScoring", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="NewsScoringSearchContent" runat="server">
    <%= Html.GridSearch("NewsScoringAll") %>
</asp:Panel>
<asp:Panel ID="NewsScoringFilterContent" runat="server">
    <%= Html.GridFilter("NewsScoringAll") %>
</asp:Panel>