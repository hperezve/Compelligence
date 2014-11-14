<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'DealAll');
    resizeGrid('<%= ViewData["Scope"]%>' + 'DealArchivedAll');
    }).trigger('resize');
</script>
<asp:Panel ID="DealToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Deal") %>', '<%= ViewData["Scope"] %>', 'Deal', 'DealAll', '<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Deal") %>', '<%= ViewData["Scope"] %>', 'Deal', 'DealAll')" />--%>
     <%if (ViewData["ShowArchived"] == null)
       { %>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Deal','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Deal") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Deal','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Deal") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Deal', 'DealAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Deal', 'DealAll');" />
    <input id="<%= ViewData["Scope"] %>DealCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>','DealAll','DealAllView','AssignedTo','<%= Session["UserId"] %>');" />
    <%}
       else
       { %>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperationByBrowse('Deal','Delete','DealArchivedAll','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Deal") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByBrowse('Deal','Duplicate','DealArchivedAll','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Deal") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Deal', 'DealArchivedAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Deal', 'DealArchivedAll');" />
    <input id="<%= ViewData["Scope"] %>DealCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>','DealArchivedAll','DealAllView','AssignedTo','<%= Session["UserId"] %>');" />
    <%} %>
    <label for="<%= ViewData["Scope"] %>DealCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="DealCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>     
    <%if (ViewData["ShowArchived"] == null)
      { %>
    <input id="<%= ViewData["Scope"] %>DealArchivedCheckbox" class="checkbox" type="checkbox" value="Show Archived"  onclick="javascript:showArchived(this);" /> 
    <%}
      else
      {%>
    <input id="<%= ViewData["Scope"] %>DealArchivedCheckbox" class="checkbox" type="checkbox" value="Show Archived"  onclick="javascript:showArchived(this);"checked/> 
      <%
      } %>
    <label for="<%= ViewData["Scope"] %>DealCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, ArchivedDeal %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Deal%>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Workspace:Deal Support');" style="float: right;margin-right: 5px;"/><br />  
    <span id="<%= ViewData["Scope"] %>DealAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="DealDataListContent" runat="server" CssClass="contentDetailData">
<%if (ViewData["ShowArchived"] == null)
  { %>
    <%= Html.DataGrid("DealAll", "Deal", ViewData["Container"].ToString())%>
<%}
  else
  {%>
           <%= Html.DataGrid("DealArchivedAll", "Deal", ViewData["Container"].ToString())%>
<%} %>
</asp:Panel>
<asp:Panel ID="DealSearchContent" runat="server">
<%if (ViewData["ShowArchived"] == null)
  { %>
    <%= Html.GridSearch("DealAll")%>
    <%}
  else
  { %>
   <%= Html.GridSearch("DealArchivedAll")%>
    <%} %>
</asp:Panel>
<asp:Panel ID="DealFilterContent" runat="server">
<%if (ViewData["ShowArchived"] == null)
  { %>
    <%= Html.GridFilter("DealAll")%>
 <%}
  else
  {%>
   <%= Html.GridFilter("DealArchivedAll")%>
 <%} %>
</asp:Panel>