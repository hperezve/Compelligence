<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Objective>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ObjectiveAll');
    }).trigger('resize');
</script>
<style>
   .newshidden
    {
         display:none;
    }
</style>
<script>
    function objectiveallView() {
    
        var Val=$('#<%= ViewData["Scope"] %>ObjectiveCheckbox').prop("checked");
        if (Val) {
            $('#ObjectiveCheckbox').removeClass('newshidden');
            $('#ObjectiveHierarchyCheckbox').addClass('newshidden');
            $('#ObjectiveAchievedCheckbox').addClass('newshidden');
            $('#deletIni').addClass('newshidden');
            
            $('#ObjectiveCheckboxUpdated').removeClass('newshidden');
            $('#ObjectiveHierarchyCheckboxUpdated').addClass('newshidden');
            $('#ObjectiveAchievedCheckboxUpdated').addClass('newshidden');

            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveSearchHierarchy').addClass('newshidden');
            $('#ObjectiveSearchAchieved').addClass('newshidden');

            $('#Objectivefilter').removeClass('newshidden');
            $('#ObjectivefilterHierarchy').addClass('newshidden');
            $('#ObjectivefilterAchieved').addClass('newshidden');


            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveByParentSearch').addClass('newshidden');
            $('#ObjectiveWithAchievedSearch').addClass('newshidden');


            $('#ObjectiveFilter').removeClass('newshidden');
            $('#ObjectiveByParentFilter').addClass('newshidden');
            $('#ObjectiveWithAchievedFilter').addClass('newshidden');
        }
        else {
            $('#ObjectiveCheckbox').addClass('newshidden');
            $('#ObjectiveHierarchyCheckbox').addClass('newshidden');
            $('#ObjectiveAchievedCheckbox').addClass('newshidden');
            $('#deletIni').removeClass('newshidden');

            $('#ObjectiveCheckboxUpdated').removeClass('newshidden');
            $('#ObjectiveHierarchyCheckboxUpdated').addClass('newshidden');
            $('#ObjectiveAchievedCheckboxUpdated').addClass('newshidden');

            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveSearchHierarchy').addClass('newshidden');
            $('#ObjectiveSearchAchieved').addClass('newshidden');

            $('#Objectivefilter').removeClass('newshidden');
            $('#ObjectivefilterHierarchy').addClass('newshidden');
            $('#ObjectivefilterAchieved').addClass('newshidden');


            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveByParentSearch').addClass('newshidden');
            $('#ObjectiveWithAchievedSearch').addClass('newshidden');


            $('#ObjectiveFilter').removeClass('newshidden');
            $('#ObjectiveByParentFilter').addClass('newshidden');
            $('#ObjectiveWithAchievedFilter').addClass('newshidden');
            
            
        }
    }
    function objectiveHierarchy() {
        var value = $('#<%= ViewData["Scope"] %>ObjectiveHierarchyCheckbox').prop("checked");
        if (value) {
            $('#ObjectiveCheckbox').addClass('newshidden');
            $('#ObjectiveHierarchyCheckbox').removeClass('newshidden');
            $('#ObjectiveAchievedCheckbox').addClass('newshidden');
            $('#deletIni').addClass('newshidden');


            $('#ObjectiveCheckboxUpdated').addClass('newshidden');
            $('#ObjectiveHierarchyCheckboxUpdated').removeClass('newshidden');
            $('#ObjectiveAchievedCheckboxUpdated').addClass('newshidden');

            $('#ObjectiveSearch').addClass('newshidden');
            $('#ObjectiveSearchHierarchy').removeClass('newshidden');
            $('#ObjectiveSearchAchieved').addClass('newshidden');

            $('#Objectivefilter').addClass('newshidden');
            $('#ObjectivefilterHierarchy').removeClass('newshidden');
            $('#ObjectivefilterAchieved').addClass('newshidden');


            $('#ObjectiveSearch').addClass('newshidden');
            $('#ObjectiveByParentSearch').removeClass('newshidden');
            $('#ObjectiveWithAchievedSearch').addClass('newshidden');


            $('#ObjectiveFilter').addClass('newshidden');
            $('#ObjectiveByParentFilter').removeClass('newshidden');
            $('#ObjectiveWithAchievedFilter').addClass('newshidden');
        }
        else {
            $('#ObjectiveCheckbox').addClass('newshidden');
            $('#ObjectiveHierarchyCheckbox').addClass('newshidden');
            $('#ObjectiveAchievedCheckbox').addClass('newshidden');
            $('#deletIni').removeClass('newshidden');


            $('#ObjectiveCheckboxUpdated').removeClass('newshidden');
            $('#ObjectiveHierarchyCheckboxUpdated').addClass('newshidden');
            $('#ObjectiveAchievedCheckboxUpdated').addClass('newshidden');

            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveSearchHierarchy').addClass('newshidden');
            $('#ObjectiveSearchAchieved').addClass('newshidden');

            $('#Objectivefilter').removeClass('newshidden');
            $('#ObjectivefilterHierarchy').addClass('newshidden');
            $('#ObjectivefilterAchieved').addClass('newshidden');


            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveByParentSearch').addClass('newshidden');
            $('#ObjectiveWithAchievedSearch').addClass('newshidden');


            $('#ObjectiveFilter').removeClass('newshidden');
            $('#ObjectiveByParentFilter').addClass('newshidden');
            $('#ObjectiveWithAchievedFilter').addClass('newshidden');
        }
    }
    function objectiveAchieved() {
        var value = $('#<%= ViewData["Scope"] %>ObjectiveAchievedCheckbox').prop("checked");
        if (value) {
            $('#ObjectiveCheckbox').addClass('newshidden');
            $('#ObjectiveHierarchyCheckbox').addClass('newshidden');
            $('#ObjectiveAchievedCheckbox').removeClass('newshidden');
            $('#deletIni').addClass('newshidden');


            $('#ObjectiveCheckboxUpdated').addClass('newshidden');
            $('#ObjectiveHierarchyCheckboxUpdated').addClass('newshidden');
            $('#ObjectiveAchievedCheckboxUpdated').removeClass('newshidden');

            $('#ObjectiveSearch').addClass('newshidden');
            $('#ObjectiveSearchHierarchy').addClass('newshidden');
            $('#ObjectiveSearchAchieved').removeClass('newshidden');

            $('#Objectivefilter').addClass('newshidden');
            $('#ObjectivefilterHierarchy').addClass('newshidden');
            $('#ObjectivefilterAchieved').removeClass('newshidden');


            $('#ObjectiveSearch').addClass('newshidden');
            $('#ObjectiveByParentSearch').addClass('newshidden');
            $('#ObjectiveWithAchievedSearch').removeClass('newshidden');


            $('#ObjectiveFilter').addClass('newshidden');
            $('#ObjectiveByParentFilter').addClass('newshidden');
            $('#ObjectiveWithAchievedFilter').removeClass('newshidden');
        }
        else {
            $('#ObjectiveCheckbox').addClass('newshidden');
            $('#ObjectiveHierarchyCheckbox').addClass('newshidden');
            $('#ObjectiveAchievedCheckbox').addClass('newshidden');
            $('#deletIni').removeClass('newshidden');

            $('#ObjectiveCheckboxUpdated').removeClass('newshidden');
            $('#ObjectiveHierarchyCheckboxUpdated').addClass('newshidden');
            $('#ObjectiveAchievedCheckboxUpdated').addClass('newshidden');

            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveSearchHierarchy').addClass('newshidden');
            $('#ObjectiveSearchAchieved').addClass('newshidden');

            $('#Objectivefilter').removeClass('newshidden');
            $('#ObjectivefilterHierarchy').addClass('newshidden');
            $('#ObjectivefilterAchieved').addClass('newshidden');


            $('#ObjectiveSearch').removeClass('newshidden');
            $('#ObjectiveByParentSearch').addClass('newshidden');
            $('#ObjectiveWithAchievedSearch').addClass('newshidden');


            $('#ObjectiveFilter').removeClass('newshidden');
            $('#ObjectiveByParentFilter').addClass('newshidden');
            $('#ObjectiveWithAchievedFilter').addClass('newshidden');
        }
    }
</script>
<asp:Panel ID="ObjectiveToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Objective") %>', '<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Objective") %>', '<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveAll')" />--%>
    <input id="ObjectiveCheckbox"  class="button newshidden" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Objective','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Objective") %>');" />
    <input id="ObjectiveHierarchyCheckbox" class="button newshidden" type="button" value="Delete" onclick="javascript: sendParamsToOperationByBrowse('Objective','Delete','ObjectiveByParent','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Objective") %>');" />
    <input id="ObjectiveAchievedCheckbox" class="button newshidden" type="button" value="Delete" onclick="javascript: sendParamsToOperationByBrowse('Objective','Delete','ObjectiveWithAchieved','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Objective") %>');" />
    <input id="deletIni" class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Objective','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Objective") %>');" />
    
    <input  id="ObjectiveCheckboxUpdated" class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Objective','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Objective") %>');" />
    <input  id="ObjectiveHierarchyCheckboxUpdated" class="button newshidden" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByBrowse('Objective','Duplicate','ObjectiveByParent','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Objective") %>');" />
    <input  id="ObjectiveAchievedCheckboxUpdated" class="button newshidden" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByBrowse('Objective','Duplicate','ObjectiveWithAchieved','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Objective") %>');" />
    
    <input id="ObjectiveSearch" class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveAll');" />
    <input id="ObjectiveSearchHierarchy" class="button newshidden" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveByParent');" />
    <input id="ObjectiveSearchAchieved" class="button newshidden" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveWithAchieved');" />
    
    <input id="Objectivefilter" class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveAll');" />
    <input id="ObjectivefilterHierarchy" class="button newshidden" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveByParent');" />
    <input id="ObjectivefilterAchieved" class="button newshidden" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Objective', 'ObjectiveWithAchieved');" />
        
    <input id="<%= ViewData["Scope"] %>ObjectiveCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'ObjectiveAll', 'ObjectiveAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');uncheckedOther(this,'<%= ViewData["Scope"] %>', 'ObjectiveAll', 'ObjectiveAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');objectiveallView();" />
    <label for="<%= ViewData["Scope"] %>ObjectiveCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="ObjectiveCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>   
    
    <input id="<%= ViewData["Scope"] %>ObjectiveHierarchyCheckbox" class="checkbox" type="checkbox" onclick="javascript:showObjectiveByHierarchy(this,'<%= ViewData["Scope"] %>', 'ObjectiveAll', 'ObjectiveAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');objectiveHierarchy();" />
    <label for="<%= ViewData["Scope"] %>ObjectiveHierarchyCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="ObjectiveHierarchyCheckbox" runat="server" Text="<%$ Resources:LabelResource, ObjectiveHierarchyChecked %>" />
    </label>   
    <input id="<%= ViewData["Scope"] %>ObjectiveAchievedCheckbox" class="checkbox" type="checkbox" onclick="javascript:showObjectiveWithAchieved(this,'<%= ViewData["Scope"] %>', 'ObjectiveAll', 'ObjectiveAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');objectiveAchieved();" />
    <label for="<%= ViewData["Scope"] %>ObjectiveAchievedCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="ObjectiveAchievedCheckbox" runat="server" Text="<%$ Resources:LabelResource, ObjectiveAchievedAndNotAchievedCheckbox %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Objective%>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Workspace:Objetives');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>ObjectiveAllSelectedOption" class="selectedOption"></span>
     
</asp:Panel>
<asp:Panel ID="ObjectiveDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflowObjective" class="gridOverflow" style="display: block"><%= Html.DataGrid("ObjectiveAll", "Objective", ViewData["Container"].ToString())%></div>
    <div id="gridHierarchyOverflowObjective" class="gridOverflow" style="display: none"><%= Html.DataGrid("ObjectiveByParent", "Objective", ViewData["Container"].ToString())%></div>
    <div id="gridAchievedOverflowObjective" class="gridOverflow" style="display: none"><%= Html.DataGrid("ObjectiveWithAchieved", "Objective", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="ObjectiveSearchContent" runat="server" >
  <div id="ObjectiveSearch"> <%= Html.GridSearch("ObjectiveAll") %></div>
  <div id="ObjectiveByParentSearch" class="newshidden"> <%= Html.GridSearch("ObjectiveByParent")%></div>
  <div id="ObjectiveWithAchievedSearch" class="newshidden"> <%= Html.GridSearch("ObjectiveWithAchieved")%></div>
</asp:Panel>
<asp:Panel ID="ObjectiveFilterContent" runat="server">
    <div id="ObjectiveFilter"><%= Html.GridFilter("ObjectiveAll") %></div>
    <div id="ObjectiveByParentFilter" class="newshidden"><%= Html.GridFilter("ObjectiveByParent")%></div>
    <div id="ObjectiveWithAchievedFilter" class="newshidden"><%= Html.GridFilter("ObjectiveWithAchieved")%></div>
</asp:Panel>