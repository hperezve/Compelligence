<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.Calendar>>" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'CalendarAll');
    }).trigger('resize');
</script>
<asp:Panel ID="TaskCalendar" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Calendar") %>', '<%= ViewData["Scope"] %>', 'Calendar', 'CalendarAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Calendar") %>', '<%= ViewData["Scope"] %>', 'Calendar', 'CalendarAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Calendar','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Calendar") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Calendar','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Calendar") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Calendar', 'CalendarAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Calendar', 'CalendarAll');" />
    <input class="button" type="button" value="Show Calendar" onclick="javascript: Calendar('<%= Url.Action("CreateCalendar", "Calendar") %>');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Calendar','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Calendar %>');" style="float: right;margin-right: 5px;"/><br />
    <%--<a href= "<%=Url.Action("GetItems","Calendar",new {datemount="03" , dateyear ="2011"}) %>">Here</a>--%>
    <span id="<%= ViewData["Scope"] %>CalendarAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="CalendarDataListContent" runat="server" CssClass="contentDetailData">
    <div id="gridOverflow" class="gridOverflow">
        <%= Html.DataGrid("CalendarAll", "Calendar", ViewData["Container"].ToString())%>
    </div>
</asp:Panel>
<asp:Panel ID="CalendarSearchContent" runat="server">
    <%= Html.GridSearch("CalendarAll")%>
</asp:Panel>
<asp:Panel ID="CalendarFilterContent" runat="server">
    <%= Html.GridFilter("CalendarAll")%>
</asp:Panel>

<script type="text/javascript">
    function Calendar(Url) {
        
        var browseName = '<%= ViewData["Scope"] %>' + 'CalendarAll';
        var gridId = '#' + browseName + 'ListTable';
        var items = $(gridId).getGridParam('records');

        if (items > 0)
        {
            CreateCalendar(Url);
        }
        else {
            showAlertNoFoundCalendarDialog();

        }
    }
    var showAlertNoFoundCalendarDialog = function() {
        $("#AlertNoFoundCalendarDialog").dialog('open');
    };
    $("#AlertNoFoundCalendarDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });
</script>