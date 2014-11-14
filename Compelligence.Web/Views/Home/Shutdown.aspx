<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="LogOutSessionDialog" class="displayNone">
        <p>
            <span class="loadingDialog">Your session has finished ...</span>
        </p>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
<!-- Need update for merge with 1256(jquery) issue-->
<!-- Compatibility-->
<script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    
<script type="text/javascript">
    $(function() {
        $.blockUI(
                { message: $('#LogOutSessionDialog'),
                    css: { width: '20%', margin: 'auto' }
                });

        setTimeout('logOutSession()', 3000);

    });

    var logOutSession = function() {
        location.href = '<%= Url.Action("Index", "Home") %>';
    };
</script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="RightMainContent" runat="server">
</asp:Content>
