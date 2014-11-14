<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    //    jQuery(window).bind('resize', function() {
    //    resizeGrid('<%= ViewData["Scope"]%>' + 'NewsAll');
    //    resizeGrid('<%= ViewData["Scope"]%>' + 'NewsAllByIndustry');
    //    resizeGrid('<%= ViewData["Scope"]%>' + 'NewsAllByCompetitor');
    //    resizeGrid('<%= ViewData["Scope"]%>' + 'NewsAllByProduct');
    //    }).trigger('resize');
</script>
<asp:Panel ID="NewsToolbarContent" runat="server" CssClass="buttonLink">
    <input id="ButtonNew" class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "News") %>','<%= ViewData["Scope"] %>', 'News', '<%= ViewData["BrowseDetailName"] %>', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />

    <input id="DeleteButtonNewsAll" class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperationOnNews('News','NewsAll','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "News") %>');" />
    <input id="SendButtonNewsAll" class="button" type="button" value="Send" onclick="javascript:SendEmail('NewsAll')" />
    <input class="button" id="searchButtonNewsAll" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'News', 'NewsAll');" />
    <input class="button" id="filterButtonNewsAll" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'News', 'NewsAll');" />
    <label for="FilterIndustryId" style="font-size: 1.3em;">
        <asp:Literal ID="FilterIndustryId" runat="server" Text="Industry" />:</label>
    <%= Html.DropDownList("IndustryId", (SelectList)ViewData["IndustryList"], string.Empty, new { id = ViewData["Scope"] + "NewsAllIndustryId", onchange = "javascript: ExecuteFilterByIndustry(this, this.value,'" + ViewData["Scope"] + "', 'News', 'NewsAll', 'NewsAllByIndustry','" + ViewData["OptionDateList"] + "');", style = "width:120px;" })%>
    <label for="FilterCompetitorId" style="font-size: 1.3em;">
        <asp:Literal ID="FilterCompetitorId" runat="server" Text="Competitor" />:</label>
    <%= Html.CascadingParentDropDownListWP(ViewData["Scope"] + "NewsAllCompetitorId", (SelectList)ViewData["CompetitorList"], string.Empty, false, Url.Action("GetProductByCompetitor", "News"), new string[] { }, string.Empty, ViewData["Scope"] + "NewsAllProductId", new string[] { }, null, "ExecuteFilterByCompetitorT(this, this.value, '" + ViewData["Scope"] + "','News', 'NewsAll', 'NewsAllByCompetitor', '" + ViewData["OptionDateList"] + "');", "width:120px;")%>
    <label for="FilterProductId" style="font-size: 1.3em;">
        <asp:Literal ID="FilterProductId" runat="server" Text="Product" />:</label>
    <%= Html.CascadingChildDropDownListWP(ViewData["Scope"] + "NewsAllProductId", (SelectList)ViewData["ProductList"], string.Empty, string.Empty, "ExecuteFilterByProductT(this, this.value, '" + ViewData["Scope"] + "','News', 'NewsAll', 'NewsAllByProduct', '" + ViewData["OptionDateList"] + "');", "width:120px;")%>
    <label for="FilterDate" style="font-size: 1.3em;">
        <asp:Literal ID="FilterDate" runat="server" Text="Date" />:</label>
    <%= Html.DropDownList("Date", (SelectList)ViewData["DateList"], string.Empty, new { id = ViewData["Scope"] + "NewsAllDate", onchange = "javascript: ExecuteFilterByDateT(this, this.value,'" + ViewData["Scope"] + "', 'News', 'NewsAll', '" + ViewData["OptionDateList"] + "');", style = "width:120px;" })%>
    <label for="FilterScoreId" style="font-size: 1.3em;">
        <asp:Literal ID="FilterScore" runat="server" Text="Min Score" />:</label>
   <input type="text" id="FilterScoreId" style="width:50px;" />
   <input type="button" value="Min Score" onclick="javascript: ExecuteFilterByScoreT('<%= ViewData["Scope"] %>', 'News', 'NewsAll', '<%= ViewData["OptionDateList"]%>');" />
   <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'News','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.News %>');" style="float: right;margin-right: 5px;"/><br />
   <span id="<%= ViewData["Scope"] %>NewsAllSelectedOption" class="selectedOption"></span>
   
</asp:Panel>
<asp:Panel ID="NewsListContent" runat="server" CssClass="contentDetailData">
    <div id="gridNewsAll" class="gridOverflow" style="display: block">
    <%= Html.DataGrid("NewsAll", "News", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="NewsSearchContent" runat="server">
    <%= Html.GridSearch("NewsAll")%>
</asp:Panel>
<asp:Panel ID="NewsFilterContent" runat="server">
    <%= Html.GridFilter("NewsAll")%>
</asp:Panel>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {

        NewsAllGrid = '#' + '<%= ViewData["Scope"]%>' + 'NewsAllListTable';
        NewsAllByIndustryGrid = '#' + '<%= ViewData["Scope"]%>' + 'NewsAllByIndustryListTable';
        NewsAllByCompetitorGrid = '#' + '<%= ViewData["Scope"]%>' + 'NewsAllByCompetitorListTable';
        NewsAllByProductGrid = '#' + '<%= ViewData["Scope"]%>' + 'NewsAllByProductListTable';

        var width = Math.round($(window).width() * 0.98) + '';

        $(NewsAllGrid).jqGrid('setGridWidth', width);
        $(NewsAllByIndustryGrid).jqGrid('setGridWidth', width);
        $(NewsAllByCompetitorGrid).jqGrid('setGridWidth', width);
        $(NewsAllByProductGrid).jqGrid('setGridWidth', width);

    }).trigger('resize'); 
</script>