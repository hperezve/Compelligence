<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Product>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ProductAll');
    }).trigger('resize');
</script>
<%-- <!-- the grid definition in html is a table tag with class 'scroll' --> 
<table id="list2" class="scroll" cellpadding="0" cellspacing="0"></table> 
 <!-- pager definition. class scroll tels that we want to use the same theme as grid --> 
 <div id="pager2" class="scroll" style="text-align:center;"></div> --%>
<asp:Panel ID="ProductToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Product") %>', '<%= ViewData["Scope"] %>', 'Product', 'ProductAll','<%= ViewData["Container"] %>');" />
<%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Product") %>', '<%= ViewData["Scope"] %>', 'Product', 'ProductAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Product','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Product") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Product','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Product") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Product', 'ProductAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Product', 'ProductAll');" />
    <input class="button" type="button" value="Enable" onclick="javascript: changeStatusOfEntity('Product','ProductAll','Enable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("UpdateEnableStatus", "Product") %>');" />
    <input class="button" type="button" value="Disable" onclick="javascript: changeStatusOfEntity('Product','ProductAll','Disable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("UpdateDisableStatus", "Product") %>');" />
    <input id="<%= ViewData["Scope"] %>ProductCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'ProductAll', 'ProductAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>ProductCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="ProductCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Product %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>','<%= ViewData["ActionFrom"] %>','Environment:Products/Offering');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>ProductAllSelectedOption" class="selectedOption"></span>
    <span id="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="ProductDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow">
        <%= Html.DataGrid("ProductAll", "Product", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="ProductSearchContent" runat="server">
    <%= Html.GridSearch("ProductAll") %>
</asp:Panel>
<asp:Panel ID="ProductFilterContent" runat="server">
    <%= Html.GridFilter("ProductAll") %>
</asp:Panel>