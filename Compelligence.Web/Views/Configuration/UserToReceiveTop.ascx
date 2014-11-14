<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + 'UserProfileAll');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>NewsUserProfileDetailDataListContent" >
            <asp:Panel ID="NewsUserProfileetailToolbarContent" runat="server" CssClass="buttonLink">
                <input class="button" type="button" value="Add Users" onclick="javascript: addUser('<%= Url.Action("UpdateUsers", "Configuration") %> ', 'Admin', 'Configuration', 'UserProfileAll', 'UserProfileDetailSelect', '<%= ViewData["HeaderType"] %> ', '<%= ViewData["DetailFilter"]%>')" />
                <input class="button" type="button" value="Remove Users" onclick="javascript: removeUsers('Configuration','Admin','#UsersToReceiveTopContent','<%= Url.Action("RemoveToReceive", "Configuration")%>','<%= ViewData["Scope"] %>UserProfileAll');reloadGrid('<%= ViewData["Scope"] %>UserProfileAll');" />
                <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', 'USRNWS','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Admin:Configurations:Users to Receive News');" style="float: right;margin-right: 5px;"/>                
            </asp:Panel>
            <asp:Panel  ID="NewsUserProfileDetailDataListContent" runat="server" CssClass="contentDetailData">
                   <%= Html.DataGrid("UserProfileAll")%>
            </asp:Panel>
    </div>
</div>    