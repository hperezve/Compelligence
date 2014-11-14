<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Template>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'TemplateAll');
    }).trigger('resize');
</script>
<asp:Panel ID="TemplateToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Template") %>', '<%= ViewData["Scope"] %>', 'Template', 'TemplateAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Template") %>', '<%= ViewData["Scope"] %>', 'Template', 'TemplateAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Template','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Template") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Template','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Template") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Template', 'TemplateAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Template', 'TemplateAll');" />
    <input id="<%= ViewData["Scope"] %>TemplateCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'TemplateAll', 'TemplateAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>TemplateCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="TemplateCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Template %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Admin:Templates');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>TemplateAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="TemplateDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("TemplateAll", "Template", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="TemplateSearchContent" runat="server">
    <%= Html.GridSearch("TemplateAll") %>
</asp:Panel>
<asp:Panel ID="TemplateFilterContent" runat="server">
    <%= Html.GridFilter("TemplateAll") %>
</asp:Panel>