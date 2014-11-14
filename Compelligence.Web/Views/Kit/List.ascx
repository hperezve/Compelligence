<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Kit>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'KitAll');
    }).trigger('resize');
</script>
<style>
   .newshidden
    {
         display:none;
    }
</style>
<script>
    function KitAllView() {

        var Val = $('#<%= ViewData["Scope"] %>KitCheckbox').prop("checked");
        if (Val) {
            $('#KitCheckbox').removeClass('newshidden');
            $('#KitHierarchyCheckbox').addClass('newshidden');
            $('#KitAchievedCheckbox').addClass('newshidden');

            $('#KitDuplicate').removeClass('newshidden');
            $('#KitHierarchyDuplicate').addClass('newshidden');
            $('#KitAchievedDuplicate').addClass('newshidden');

            $('#KitSearch').removeClass('newshidden');
            $('#KitHierarchySearch').addClass('newshidden');
            $('#KitAchievedSearch').addClass('newshidden');

            $('#KitFilter').removeClass('newshidden');
            $('#KitHierarchyFilter').addClass('newshidden');
            $('#KitAchievedFilter').addClass('newshidden');

            $('#KitSearchall').removeClass('newshidden');
            $('#KitSearchByParent').addClass('newshidden');
            $('#KitSearchWithAchieved').addClass('newshidden');

            $('#KitFilterall').removeClass('newshidden');
            $('#KitFilterByParent').addClass('newshidden');
            $('#KitFilterWithAchieved').addClass('newshidden');
        }
        else {
            $('#KitCheckbox').removeClass('newshidden');
            $('#KitHierarchyCheckbox').addClass('newshidden');
            $('#KitAchievedCheckbox').addClass('newshidden');

            $('#KitDuplicate').removeClass('newshidden');
            $('#KitHierarchyDuplicate').addClass('newshidden');
            $('#KitAchievedDuplicate').addClass('newshidden');

            $('#KitSearch').removeClass('newshidden');
            $('#KitHierarchySearch').addClass('newshidden');
            $('#KitAchievedSearch').addClass('newshidden');

            $('#KitFilter').removeClass('newshidden');
            $('#KitHierarchyFilter').addClass('newshidden');
            $('#KitAchievedFilter').addClass('newshidden');

            $('#KitSearchall').removeClass('newshidden');
            $('#KitSearchByParent').addClass('newshidden');
            $('#KitSearchWithAchieved').addClass('newshidden');

            $('#KitFilterall').removeClass('newshidden');
            $('#KitFilterByParent').addClass('newshidden');
            $('#KitFilterWithAchieved').addClass('newshidden');
        }
    }
    function kitHierarchy() {
        var value = $('#<%= ViewData["Scope"] %>KitHierarchyCheckbox').prop("checked");
        if (value) {
            $('#KitCheckbox').addClass('newshidden');
            $('#KitHierarchyCheckbox').removeClass('newshidden');
            $('#KitAchievedCheckbox').addClass('newshidden');

            $('#KitDuplicate').addClass('newshidden');
            $('#KitHierarchyDuplicate').removeClass('newshidden');
            $('#KitAchievedDuplicate').addClass('newshidden');

            $('#KitSearch').addClass('newshidden');
            $('#KitHierarchySearch').removeClass('newshidden');
            $('#KitAchievedSearch').addClass('newshidden');

            $('#KitFilter').addClass('newshidden');
            $('#KitHierarchyFilter').removeClass('newshidden');
            $('#KitAchievedFilter').addClass('newshidden');

            $('#KitSearchall').addClass('newshidden');
            $('#KitSearchByParent').removeClass('newshidden');
            $('#KitSearchWithAchieved').addClass('newshidden');

            $('#KitFilterall').addClass('newshidden');
            $('#KitFilterByParent').removeClass('newshidden');
            $('#KitFilterWithAchieved').addClass('newshidden');
        }
        else {
            $('#KitCheckbox').removeClass('newshidden');
            $('#KitHierarchyCheckbox').addClass('newshidden');
            $('#KitAchievedCheckbox').addClass('newshidden');

            $('#KitDuplicate').removeClass('newshidden');
            $('#KitHierarchyDuplicate').addClass('newshidden');
            $('#KitAchievedDuplicate').addClass('newshidden');

            $('#KitSearch').removeClass('newshidden');
            $('#KitHierarchySearch').addClass('newshidden');
            $('#KitAchievedSearch').addClass('newshidden');

            $('#KitFilter').removeClass('newshidden');
            $('#KitHierarchyFilter').addClass('newshidden');
            $('#KitAchievedFilter').addClass('newshidden');

            $('#KitSearchall').removeClass('newshidden');
            $('#KitSearchByParent').addClass('newshidden');
            $('#KitSearchWithAchieved').addClass('newshidden');

            $('#KitFilterall').removeClass('newshidden');
            $('#KitFilterByParent').addClass('newshidden');
            $('#KitFilterWithAchieved').addClass('newshidden');
        }
    }
    function kitAchieved() {
        var value = $('#<%= ViewData["Scope"] %>KitAchievedCheckbox').prop("checked");
        if (value) {
            $('#KitCheckbox').addClass('newshidden');
            $('#KitHierarchyCheckbox').addClass('newshidden');
            $('#KitAchievedCheckbox').removeClass('newshidden');
            
            $('#KitDuplicate').addClass('newshidden');
            $('#KitHierarchyDuplicate').addClass('newshidden');
            $('#KitAchievedDuplicate').removeClass('newshidden');

            $('#KitSearch').addClass('newshidden');
            $('#KitHierarchySearch').addClass('newshidden');
            $('#KitAchievedSearch').removeClass('newshidden');

            $('#KitFilter').addClass('newshidden');
            $('#KitHierarchyFilter').addClass('newshidden');
            $('#KitAchievedFilter').removeClass('newshidden');

            $('#KitSearchall').addClass('newshidden');
            $('#KitSearchByParent').addClass('newshidden');
            $('#KitSearchWithAchieved').removeClass('newshidden');

            $('#KitFilterall').addClass('newshidden');
            $('#KitFilterByParent').addClass('newshidden');
            $('#KitFilterWithAchieved').removeClass('newshidden');
        }
        else {
            $('#KitCheckbox').addClass('newshidden');
            $('#KitHierarchyCheckbox').addClass('newshidden');
            $('#KitAchievedCheckbox').removeClass('newshidden');

            $('#KitDuplicate').removeClass('newshidden');
            $('#KitHierarchyDuplicate').addClass('newshidden');
            $('#KitAchievedDuplicate').addClass('newshidden');

            $('#KitSearch').removeClass('newshidden');
            $('#KitHierarchySearch').addClass('newshidden');
            $('#KitAchievedSearch').addClass('newshidden');

            $('#KitFilter').removeClass('newshidden');
            $('#KitHierarchyFilter').addClass('newshidden');
            $('#KitAchievedFilter').addClass('newshidden');

            $('#KitSearchall').removeClass('newshidden');
            $('#KitSearchByParent').addClass('newshidden');
            $('#KitSearchWithAchieved').addClass('newshidden');

            $('#KitFilterall').removeClass('newshidden');
            $('#KitFilterByParent').addClass('newshidden');
            $('#KitFilterWithAchieved').addClass('newshidden');
        }
    }
</script>

<asp:Panel ID="KitToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Kit") %>', '<%= ViewData["Scope"] %>', 'Kit', 'KitAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Kit") %>', '<%= ViewData["Scope"] %>', 'Kit', 'KitAll')" />--%>
    <input id="KitCheckbox" class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Kit','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Kit") %>');" />
    <input id="KitHierarchyCheckbox" class="button newshidden" type="button" value="Delete" onclick="javascript: sendParamsToOperationByBrowse('Kit','Delete','KitByParent','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Kit") %>');" />
    <input id="KitAchievedCheckbox"  class="button newshidden" type="button" value="Delete" onclick="javascript: sendParamsToOperationByBrowse('Kit','Delete','KitWithAchieved','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Kit") %>');" />
    
    <input id="KitDuplicate" class="button " type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Kit','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Kit") %>');" />
    <input id="KitHierarchyDuplicate" class="button newshidden" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByBrowse('Kit','Duplicate','KitByParent','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Kit") %>');" />
    <input id="KitAchievedDuplicate" class="button newshidden" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByBrowse('Kit','Duplicate','KitWithAchieved','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Kit") %>');" />
    
    <input id="KitSearch" class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Kit', 'KitAll');" />
    <input id="KitHierarchySearch" class="button newshidden" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Kit', 'KitByParent');" />
    <input id="KitAchievedSearch"  class="button newshidden" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Kit', 'KitWithAchieved');" />
      
    <input id="KitFilter" class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Kit', 'KitAll');" />
    <input id="KitHierarchyFilter" class="button newshidden" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Kit', 'KitByParent');" />
    <input id="KitAchievedFilter" class="button newshidden" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Kit', 'KitWithAchieved');" />
      
    <input id="<%= ViewData["Scope"] %>KitCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'KitAll', 'KitAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');uncheckedOther(this,'<%= ViewData["Scope"] %>', 'KitAll', 'KitAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');KitAllView();" />
    <label for="<%= ViewData["Scope"] %>KitCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="KitCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label> 
    <input id="<%= ViewData["Scope"] %>KitHierarchyCheckbox" class="checkbox" type="checkbox" onclick="javascript:showKitByHierarchy(this,'<%= ViewData["Scope"] %>', 'KitAll', 'KitAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');kitHierarchy();" />
    <label for="<%= ViewData["Scope"] %>KitHierarchyCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="KitHierarchyCheckbox" runat="server" Text="<%$ Resources:LabelResource, KitHierarchyChecked %>" />
    </label>   
    <input id="<%= ViewData["Scope"] %>KitAchievedCheckbox" class="checkbox" type="checkbox" onclick="javascript:showKitWithAchieved(this,'<%= ViewData["Scope"] %>', 'KitAll', 'KitAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');kitAchieved();" />
    <label for="<%= ViewData["Scope"] %>KitAchievedCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="KitAchievedCheckbox" runat="server" Text="<%$ Resources:LabelResource, KitAchievedAndNotAchievedCheckbox %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Kit %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Workspace:Kits');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>KitAllSelectedOption" class="selectedOption"></span>    
   
</asp:Panel>
<asp:Panel ID="KitDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow" style="display: block">    <%= Html.DataGrid("KitAll", "Kit", ViewData["Container"].ToString())%></div>
    <div id="gridHierarchyOverflow" class="gridOverflow" style="display: none"><%= Html.DataGrid("KitByParent", "Kit", ViewData["Container"].ToString())%></div>
    <div id="gridAchievedOverflow" class="gridOverflow" style="display: none"><%= Html.DataGrid("KitWithAchieved", "Kit", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="KitSearchContent" runat="server">
    <div id="KitSearchall"><%= Html.GridSearch("KitAll") %></div>
    <div id="KitSearchByParent" class="newshidden"><%= Html.GridSearch("KitByParent")%></div>
    <div id="KitSearchWithAchieved" class="newshidden"><%= Html.GridSearch("KitWithAchieved")%></div>
</asp:Panel>
<asp:Panel ID="KitFilterContent" runat="server">
    <div id="KitFilterall"><%= Html.GridFilter("KitAll") %></div>
    <div id="KitFilterByParent" class="newshidden"><%= Html.GridFilter("KitByParent")%></div>
    <div id="KitFilterWithAchieved" class="newshidden"><%= Html.GridFilter("KitWithAchieved")%></div>
</asp:Panel>