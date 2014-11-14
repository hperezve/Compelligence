<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ProjectArchiveAll');
    }).trigger('resize');
</script>
<asp:Panel ID="ProjectArchiveToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "ProjectArchive") %>', '<%= ViewData["Scope"] %>', 'ProjectArchive', 'ProjectArchive', '<%= ViewData["Container"] %>');" />
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('ProjectArchive','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "ProjectArchive") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('ProjectArchive','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "ProjectArchive") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'ProjectArchive', 'ProjectArchiveAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'ProjectArchive', 'ProjectArchiveAll');" />
    <input id="<%= ViewData["Scope"] %>ProjectCheckbox" class="checkbox" type="checkbox" value="Show Mine" onclick="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>','ProjectArchiveAll', 'ProjectAllView', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
      
     
    <label for="<%= ViewData["Scope"] %>ProjectCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="ProjectCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    

    <span id="<%= ViewData["Scope"] %>ProjectArchiveSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="ProjectArchiveDataListContent" runat="server" CssClass="contentDetailData">
       <%= Html.DataGrid("ProjectArchiveAll", "ProjectArchive", ViewData["Container"].ToString())%>
</asp:Panel>
<asp:Panel ID="ProjectArchiveSearchContent" runat="server">
          <%= Html.GridSearch("ProjectArchiveAll")%>    
    </asp:Panel>
<asp:Panel ID="ProjectArchiveFilterContent" runat="server">
    
        <%= Html.GridFilter("ProjectArchiveAll")%>
    
</asp:Panel>