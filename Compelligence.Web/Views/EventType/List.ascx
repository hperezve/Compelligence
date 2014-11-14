<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.EventType>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'EventTypeAll');
    }).trigger('resize');
</script>
<asp:Panel ID="EventTypeToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "EventType") %>', '<%= ViewData["Scope"] %>', 'EventType', 'EventTypeAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "EventType") %>', '<%= ViewData["Scope"] %>', 'EventType', 'EventTypeAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('EventType','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "EventType") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('EventType','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "EventType") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'EventType', 'EventTypeAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'EventType', 'EventTypeAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.EventType %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Admin:Event Type');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>EventTypeAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="EventTypeDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow"><%= Html.DataGrid("EventTypeAll", "EventType", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="EventTypeSearchContent" runat="server">
    <%= Html.GridSearch("EventTypeAll")%>
</asp:Panel>
<asp:Panel ID="EventTypeFilterContent" runat="server">
    <%= Html.GridFilter("EventTypeAll")%>
</asp:Panel>