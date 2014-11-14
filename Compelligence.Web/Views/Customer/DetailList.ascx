<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');

    var setHasRows = function() {
        var url = '<%= Url.Action("SetHasRows", "Customer")%>';
        var headerType = '<%= ViewData["HeaderType"] %>';
        var detailFilter = '<%= ViewData["DetailFilter"] %>';
        var parameters = { HeaderType: headerType, DetailFilter: detailFilter };
        $.post(url, parameters, function() {
            var hasRows = '<%= ViewData["HasRows"]%>';
            if (hasRows == 'False') {
                $('#editbutton').prop('disabled', 'disabled');
                $('#editbutton').css('display', 'none');
            }
            else if (hasRows == 'True') {
            $('#editbutton').prop('disabled', '');
                $('#editbutton').css('display', 'inline');
            }
        });
    };

    $(function() {
        setHasRows();
    });
</script>

<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>CustomerDetailDataListContent" class="absolute">                                                
    <% if (ViewData["UserSecurityAccess"].ToString().Equals(UserSecurityAccess.Edit))
       { %>
        <asp:Panel ID="CustomerDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Customer") + "', '" + ViewData["Scope"] + "', 'Customer', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "'); setHasRows();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { id = "editbutton", onClick = "javascript: editEntity('" + Url.Action("Edit", "Customer") + "', '" + ViewData["Scope"] + "', 'Customer', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>        
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Customer") + "', '" + ViewData["Scope"] + "', 'Customer', '" + ViewData["BrowseDetailName"] + "', 'CustomerDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Customer") + "', '" + ViewData["Scope"] + "', 'Customer', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
        </asp:Panel>
     <% } %>
        <asp:Panel ID="CustomerDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="CustomerDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>CustomerEditFormContent" />
    </asp:Panel>
</div>
