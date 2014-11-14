<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Industry>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'IndustryAll');
    resizeGrid('<%= ViewData["Scope"]%>' + 'IndustryByParent');
    }).trigger('resize');
</script>
<asp:Panel ID="IndustryToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntityHierarchy('<%= Url.Action("Create", "Industry") %>', '<%= ViewData["Scope"] %>', 'Industry', 'IndustryAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Industry") %>', '<%= ViewData["Scope"] %>', 'Industry', 'IndustryAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperationByHierarchy('Industry','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Industry") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByHierarchy('Industry','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Industry") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Industry', 'IndustryAll');hiddenHierarchy('<%= ViewData["Scope"] %>', 'Industry');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Industry', 'IndustryAll');hiddenHierarchy('<%= ViewData["Scope"] %>', 'Industry');" />
    <input class="button" type="button" value="Enable" onclick="javascript: changeStatusOfEntity('Industry','IndustryAll','Enable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("UpdateEnableStatus", "Industry") %>');" />
    <input class="button" type="button" value="Disable" onclick="javascript: changeStatusOfEntity('Industry','IndustryAll','Disable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("UpdateDisableStatus", "Industry") %>');" />
    <input id="<%= ViewData["Scope"] %>IndustryCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'IndustryAll', 'IndustryAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');hierarchyHidden(this,'<%= ViewData["Scope"] %>', 'Industry');" />
    <label for="<%= ViewData["Scope"] %>IndustryCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="IndustryCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>        
    <input id="<%= ViewData["Scope"] %>IndustryHierarchyCheckbox" class="checkbox" type="checkbox" onclick="javascript:showIndustryByHierarchy(this,'<%= ViewData["Scope"] %>', 'IndustryAll', 'IndustryAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>IndustryHierarchyCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="IndustryHierarchyCheckbox" runat="server" Text="<%$ Resources:LabelResource, IndustryHierarchyChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Industry %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Environment:Industries');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>IndustryAllSelectedOption" class="selectedOption"> </span>
</asp:Panel>

<asp:Panel ID="IndustryDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow" style="display: block"><%= Html.DataGrid("IndustryAll", "Industry", ViewData["Container"].ToString())%></div>
    <div id="gridHierarchyOverflow" class="gridOverflow" style="display: none"><%= Html.DataGrid("IndustryByParent", "Industry", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="IndustrySearchContent" runat="server">
    <%= Html.GridSearch("IndustryAll") %>
</asp:Panel>
<asp:Panel ID="IndustryFilterContent" runat="server">
    <%= Html.GridFilter("IndustryAll") %>
</asp:Panel>