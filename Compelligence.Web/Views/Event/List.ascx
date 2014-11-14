<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Event>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'EventAll');
    resizeGrid('<%= ViewData["Scope"]%>' + 'EventPastAll');
    }).trigger('resize');
</script>
<asp:Panel ID="EventToolbarContent" runat="server" CssClass="buttonLink">
   <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Event") %>', '<%= ViewData["Scope"] %>', 'Event', 'EventAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Event") %>', '<%= ViewData["Scope"] %>', 'Event', 'EventAll')" />--%>
    <%if (ViewData["ShowPastEvents"] == null)
      { %>
        <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Event','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Event") %>');" />
        <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Event','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Event") %>');" />
        <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Event', 'EventAll');" />
        <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Event', 'EventAll');" />
        <input class="button" type="button" value="Show Calendar" onclick="CreateCalendar('<%= Url.Action("CreateCalendar", "Event") %>');" />        
        <input id="EventCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'EventAll', 'EventAllView', 'EventAssignedTo','<%= Session["UserId"] %>' );" />
        <%}
      else
      { %>
        <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('EventPast','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Event") %>');" />
        <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Event','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Event") %>');" />
        <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Event', 'EventPastAll');" />
        <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Event', 'EventPastAll');" />
        <input class="button" type="button" value="Show Calendar" onclick="CreateCalendar('<%= Url.Action("CreateCalendar", "Event") %>');" />
        <input id="EventCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'EventPastAll', 'EventAllView', 'EventAssignedTo','<%= Session["UserId"] %>' );" />      
      <%} %>
    <label for="EventCheckbox" style="font-size: 12px; color: black;">
        <asp:Literal ID="EventCheckboxTxt" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <%--<input id="AllEventCheckbox" class="checkbox" type="checkbox" value="Show Past Events" onchange="javascript:showEvents(this,'<%= ViewData["Scope"] %>', 'EventAll', 'EventAllView', 'EventAssignedTo','<%= Session["UserId"] %>' );" />--%>
    <%if (ViewData["ShowPastEvents"] == null)
      { %>
        <input id="AllEventCheckbox" class="checkbox" type="checkbox" value="Show Past Events" onclick="javascript:loadEvents();" />
        
    <%}
      else
      { %>
        <input id="AllEventCheckbox" class="checkbox" type="checkbox" value="Show Past Events" onclick="javascript:loadEvents();" checked />
      <%} %>
    
    <label for="AllEventCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="AllEventCheckboxTxt" runat="server" Text="<%$ Resources:LabelResource, PastEvents %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.Event %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Workspace:Events');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>EventAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="EventDataListContent" runat="server" CssClass="contentDetailData" >
    <%if (ViewData["ShowPastEvents"] == null)
      { %>
        <%= Html.DataGrid("EventAll", "Event", ViewData["Container"].ToString())%>
    <%}
      else
      { %>
        <%= Html.DataGrid("EventPastAll", "Event", ViewData["Container"].ToString())%>
      <%} %>
<%--<table cellpadding="0" cellspacing="0" class="scroll" id="WorkspaceEventAllListTable" style="z-index:2000"></table>
<div class="scroll" id="WorkspaceEventAllListPager" style="text-align: center;"></div>

<script type="text/javascript">
    $(function() {
        $('#WorkspaceEventAllListTable').jqGrid({
            url: '/Browse.aspx/GetData?bid=EventAll&eou',
            datatype: "json",
            mtype: "POST",
            colNames: ['', 'Event Name', 'Status', 'Time Frame', 'Industry', 'Competitor', 'Product', 'Scenario', 'Confidence', 'Severity', 'RecommendedActions', 'Comment', 'Owner', 'Opened By', 'Location', 'Start Date', 'End Date'],
            colModel: [{ name: 'EventAllView.Id', index: 'EventAllView.Id', hidden: true },
           { name: 'EventAllView.Name', index: 'EventAllView.Name' },
		   { name: 'EventAllView.Status', index: 'EventAllView.Status' },
		   { name: 'EventAllView.TimeFrame', index: 'EventAllView.TimeFrame' },
		   { name: 'EventAllView.IndustryName', index: 'EventAllView.IndustryName' },
		   { name: 'EventAllView.CompetitorName', index: 'EventAllView.CompetitorName' },
		   { name: 'EventAllView.ProductName', index: 'EventAllView.ProductName' },
		   { name: 'EventAllView.Scenario', index: 'EventAllView.Scenario' },
		   { name: 'EventAllView.Confidence', index: 'EventAllView.Confidence' },
		   { name: 'EventAllView.Severity', index: 'EventAllView.Severity' },
		   { name: 'EventAllView.RecommendedActions', index: 'EventAllView.RecommendedActions' },
		   { name: 'EventAllView.Comment', index: 'EventAllView.Comment' },
		   { name: 'EventAllView.AssignedTo', index: 'EventAllView.AssignedTo' },
		   { name: 'EventAllView.OpenedBy', index: 'EventAllView.OpenedBy' },
		   { name: 'EventAllView.Location', index: 'EventAllView.Location' },
		   { name: 'EventAllView.StartDate', index: 'EventAllView.StartDate' },
		   { name: 'EventAllView.EndDate', index: 'EventAllView.EndDate'}],
            pager: '#WorkspaceEventAllListPager',
            rowNum: 10,
            rowList: [10, 20, 30, 40, 50],
            imgpath: '/Content/Styles/jqgrid/sand/images',
            sortname: 'EventAllView.Name',
            multiselect: true,
            multikey: 'ctrlKey',
            viewrecords: true,
            sortorder: 'asc',
            scroll: true,            
            onSelectRow: function(id) { getEntity('/Event.aspx/Edit', 'Workspace', 'Event', id, 'EventAll', '#EventContent'); },
            loadComplete: function() { hideLoadingDialog(); }
        });


        $('#WorkspaceEventAllListTable').jqGrid('navGrid', '#WorkspaceEventAllListPager',
	   { edit: false, add: false, del: false, search: false, refresh: true });
	   $('#WorkspaceEventAllListTable').jqGrid('navButtonAdd', '#WorkspaceEventAllListPager',
         { caption: '', title: 'Load all data', buttonicon: 'setIconLoadAll', onClickButton: function() { showAllData('#WorkspaceEventAllListTable', 'Workspace', 'Event'); unCheckMyEntities('#WorkspaceEventAllListTable', 'Workspace', 'Event') }, position: 'last' }); $('#WorkspaceEventAllListTable').jqGrid('navButtonAdd', '#WorkspaceEventAllListPager', { caption: '', title: 'Toggle columns', buttonicon: 'setIconToggle', onClickButton: function() { toggleColumnPopup('#WorkspaceEventAllColumnPopup'); }, position: 'last' });
        var myWidth = Math.round($(window).width() * 0.98);
        $('#WorkspaceEventAllListTable').setForceGridWidth(myWidth, false);
    });
 </script>--%>
</asp:Panel>
<asp:Panel ID="EventSearchContent" runat="server">
    <%if (ViewData["ShowPastEvents"] == null)
      { %>
        <%= Html.GridSearch("EventAll") %>
    <%}
      else
      { %>
        <%= Html.GridSearch("EventPastAll") %>
      <%} %>
    
</asp:Panel>
<asp:Panel ID="EventFilterContent" runat="server">
    <%if (ViewData["ShowPastEvents"] == null)
      { %>
        <%= Html.GridFilter("EventAll") %>
    <%}
      else
      { %>
        <%= Html.GridFilter("EventPastAll") %>
      <%} %>
    
</asp:Panel>