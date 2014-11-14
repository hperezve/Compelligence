<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Partner>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'PartnerAll');
    }).trigger('resize');
</script>
<asp:Panel ID="PartnerToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Partner") %>', '<%= ViewData["Scope"] %>', 'Partner', 'PartnerAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Partner") %>', '<%= ViewData["Scope"] %>', 'Partner', 'PartnerAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Partner','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Partner") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Partner','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Partner") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Partner', 'PartnerAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Partner', 'PartnerAll');" />
    <input id="<%= ViewData["Scope"] %>PartnerCheckbox" class="checkbox" type="checkbox" value="Show Mine" onchange="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'PartnerAll', 'PartnerAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <label for="<%= ViewData["Scope"] %>PartnerCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="PartnerCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <span id="<%= ViewData["Scope"] %>PartnerAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="PartnerDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow">
        <%= Html.DataGrid("PartnerAll", "Partner", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="PartnerSearchContent" runat="server">
    <%= Html.GridSearch("PartnerAll")%>
</asp:Panel>
<asp:Panel ID="PartnerFilterContent" runat="server">
    <%= Html.GridFilter("PartnerAll")%>
</asp:Panel>