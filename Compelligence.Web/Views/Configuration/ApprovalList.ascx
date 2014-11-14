﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'ProjectsWithStatusCompleted');
    }).trigger('resize');
</script>
<% using (Html.BeginForm("ConfigurationSave", "Configuration", FormMethod.Post))
   { %>
   <div class="buttonLink">
   <input class="button" type="button" value="Publish Project" onclick="javascript: showPublishConfirmationDialog('<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("ApprovalProject", "Configuration") %>','<%= ViewData["Scope"] %>ProjectsWithStatusCompleted');" />
   <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', 'APROY','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Admin:Configurations:Approve Projects');" style="float: right;margin-right: 5px;"/>
   </div>
   <asp:Panel ID="ProjectDataListContent" runat="server" CssClass="contentDetailData">
    <%= Html.DataGrid("ProjectsWithStatusCompleted", "Project", ViewData["Container"].ToString())%>
</asp:Panel>
<% } %>