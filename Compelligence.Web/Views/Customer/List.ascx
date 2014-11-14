<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Customer>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'CustomerAll');
    }).trigger('resize');
</script>
<asp:Panel ID="CustomerToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Customer") %>', '<%= ViewData["Scope"] %>', 'Customer', 'CustomerAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Customer") %>', '<%= ViewData["Scope"] %>', 'Customer', 'CustomerAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Customer','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Customer") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Customer','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Customer") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Customer', 'CustomerAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Customer', 'CustomerAll');" />
    <input id="<%= ViewData["Scope"] %>CustomerCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'CustomerAll', 'CustomerAllView', 'AssignedTo','<%= Session["UserId"] %>' );" />
    <label for="<%= ViewData["Scope"] %>CustomerCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="CustomerCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Customer %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Environment:Customers');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>CustomerAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="CustomerDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow">
        <%= Html.DataGrid("CustomerAll", "Customer", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="CustomerSearchContent" runat="server">
    <%= Html.GridSearch("CustomerAll") %>
</asp:Panel>
<asp:Panel ID="CustomerFilterContent" runat="server">
    <%= Html.GridFilter("CustomerAll") %>
</asp:Panel>