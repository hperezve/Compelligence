<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
    $.get('<%= Url.Action("CreateSwot","Swot") %>', null, function(data) { $('#<%= ViewData["Scope"] %>SwotEditFormContent').html(data); });
    });
</script>

<asp:Panel ID="SwotListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SwotList">
    </div>
</asp:Panel>
<br />
<asp:Panel ID="SwotFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SwotEditFormContent" />
</asp:Panel>
