<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.ContentType>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ContentTypeAll');
    }).trigger('resize');
</script>
<asp:Panel ID="ContentTypeToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "ContentType") %>', '<%= ViewData["Scope"] %>', 'ContentType', 'ContentTypeAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "ContentType") %>', '<%= ViewData["Scope"] %>', 'ContentType', 'ContentTypeAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('ContentType','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "ContentType") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('ContentType','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "ContentType") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'ContentType', 'ContentTypeAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'ContentType', 'ContentTypeAll');" />
    <input id="<%= ViewData["Scope"] %>ContentTypeCheckbox"class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'ContentTypeAll', 'ContentTypeAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>ContentTypeCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="ContentTypeCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.ContentType %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Admin:Content Type');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>ContentTypeAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="ContentTypeDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("ContentTypeAll", "ContentType", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="ContentTypeSearchContent" runat="server">
    <%= Html.GridSearch("ContentTypeAll") %>
</asp:Panel>
<asp:Panel ID="ContentTypeFilterContent" runat="server">
    <%= Html.GridFilter("ContentTypeAll")%>
</asp:Panel>