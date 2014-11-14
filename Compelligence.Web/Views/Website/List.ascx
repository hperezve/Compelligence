<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Website>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'WebsiteAll');
    }).trigger('resize');
</script>
<asp:Panel ID="WebsiteToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Website") %>', '<%= ViewData["Scope"] %>', 'Website', 'WebsiteAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Website") %>', '<%= ViewData["Scope"] %>', 'Website', 'WebsiteAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Website','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Website") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Website','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Website") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Website', 'WebsiteAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Website', 'WebsiteAll');" />
    <input id="<%= ViewData["Scope"] %>WebsiteCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'WebsiteAll', 'WebsiteAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>WebsiteCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="WebsiteCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Website','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Website %>');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>WebsiteAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="WebsiteDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("WebsiteAll", "Website", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="WebsiteSearchContent" runat="server">
    <%= Html.GridSearch("WebsiteAll") %>
</asp:Panel>
<asp:Panel ID="WebsiteFilterContent" runat="server">
    <%= Html.GridFilter("WebsiteAll") %>
</asp:Panel>