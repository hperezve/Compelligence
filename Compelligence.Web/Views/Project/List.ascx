<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ProjectAll');
    resizeGrid('<%= ViewData["Scope"]%>' + 'ProjectInactiveAll');
    }).trigger('resize');
</script>
<asp:Panel ID="ProjectToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Project") %>', '<%= ViewData["Scope"] %>', 'Project', 'ProjectAll', '<%= ViewData["Container"] %>');" />
    
    
    <%if (ViewData["ShowInactiveProjects"] == null)
      { %>
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Project','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Project") %>');" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Project','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Project") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Project', 'ProjectAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Project', 'ProjectAll');" />
    <input id="<%= ViewData["Scope"] %>ProjectCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>','ProjectAll', 'ProjectAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
    <%}
      else
      { %>
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperationByBrowse('Project','Duplicate', 'ProjectInactiveAll','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Project") %>');" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperationByBrowse('Project','Delete', 'ProjectInactiveAll','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Project") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Project', 'ProjectInactiveAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Project', 'ProjectInactiveAll');" />
    <input id="<%= ViewData["Scope"] %>ProjectCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>','ProjectInactiveAll', 'ProjectAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
     <%} %>
     
     
    <label for="<%= ViewData["Scope"] %>ProjectCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="ProjectCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    
    <%if (ViewData["ShowInactiveProjects"] == null)
      { %>
        <input id="<%= ViewData["Scope"] %>InactiveProjectsCheckbox" class="checkbox" type="checkbox" value="Show Inactive Projects" onclick="javascript:showInactiveProjects(this);" />
        
    <%}
      else
      { %>
        <input id="<%= ViewData["Scope"] %>InactiveProjectsCheckbox" class="checkbox" type="checkbox" value="Show Inactive Projects" onclick="javascript:showInactiveProjects(this);" checked />
      <%} %>
    <label for="<%= ViewData["Scope"] %>InactiveProjectsCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="InactiveProjectsCheckbox" runat="server" Text="<%$ Resources:LabelResource, InactiveProjectsChecked %>" />
    </label>
    <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Project','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Project %>','<%= Compelligence.Domain.Entity.Resource.Pages.Project %>');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>ProjectAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="ProjectDataListContent" runat="server" CssClass="contentDetailData">
    <%if (ViewData["ShowInactiveProjects"]== null)
      { %>
        <%= Html.DataGrid("ProjectAll", "Project", ViewData["Container"].ToString())%>
    <%}
      else
      { %>
        <%= Html.DataGrid("ProjectInactiveAll", "Project", ViewData["Container"].ToString())%>
      <%} %>
    
</asp:Panel>
<asp:Panel ID="ProjectSearchContent" runat="server">
    <%if (ViewData["ShowInactiveProjects"]== null)
      { %>
        <%= Html.GridSearch("ProjectAll")%>    
    <%}
      else
      { %>
        <%= Html.GridSearch("ProjectInactiveAll")%>
      <%} %>
    
</asp:Panel>
<asp:Panel ID="ProjectFilterContent" runat="server">
    <%if (ViewData["ShowInactiveProjects"]== null)
      { %>
        <%= Html.GridFilter("ProjectAll")%>
    <%}
      else
      { %>
        <%= Html.GridFilter("ProjectInactiveAll")%>
      <%} %>
    
</asp:Panel>