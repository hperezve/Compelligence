<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Source>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'SourceAll');
    }).trigger('resize');
</script>
<asp:Panel ID="SourceToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Source") %>', '<%= ViewData["Scope"] %>', 'Source', 'SourceAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Source") %>', '<%= ViewData["Scope"] %>', 'Source', 'SourceAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Source','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Source") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Source','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Source") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Source', 'SourceAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Source', 'SourceAll');" />
    <input id="<%= ViewData["Scope"] %>SourceCheckbox"  class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'SourceAll', 'SourceAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>SourceCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="SourceCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Source','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Source %>');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>SourceAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="SourceDataListContent" runat="server" CssClass="contentDetailData">
    <%= Html.DataGrid("SourceAll", "Source", ViewData["Container"].ToString())%>
</asp:Panel>
<asp:Panel ID="SourceSearchContent" runat="server">
    <%= Html.GridSearch("SourceAll") %>
</asp:Panel>
<asp:Panel ID="SourceFilterContent" runat="server">
    <%= Html.GridFilter("SourceAll") %>
</asp:Panel>