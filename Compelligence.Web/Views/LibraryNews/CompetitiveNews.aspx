<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndPopupSite.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
	<title>Competitive News</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>CompetitiveNews</h2>
    <br />
    <% using (Html.BeginForm("SendMail", "LibraryNews", FormMethod.Post,
         new
         {
             LibraryId = (string)ViewData["LibraryId"],
             ENCTYPE = "multipart/form-data",
             onsubmit = "self.close()"
         }))
           { %>
    <div id="Email">
        <div id="ToEmail">
            <label for="ToEmail">
                        <asp:Literal ID="ToEmail" runat="server" Text="<%$ Resources:LabelResource, CompetitiveNewsEmailTo %>" />:</label>
            <%= Html.TextBox("ToEmail", null, new { style="margin-left: 26px;"})%>
        </div>
        <br />
        <div id="SubjectEmail">
            <label for="SubjectEmail">
                        <asp:Literal ID="SubjectEmail" runat="server" Text="<%$ Resources:LabelResource, CompetitiveNewsEmailSubject %>" />:</label>
            <%= Html.TextBox("SubjectEmail", (string)ViewData["Subject"], new { Readonly = "readonly" })%>
        </div>
        <br />
        <%= Html.Hidden("LibraryId") %>
        <div id="BodyEmail">
            <%= Html.TextArea("BodyEmail", (string)ViewData["Body"])%>
           <%-- <% Html.Encode((string)ViewData["Body"]); %>--%>
           <%--<%= Html.EmailBody((string)ViewData["Body"])%>--%>
        </div>
        <br />
        <div id="Butons">
            <%--<input id="SendMail" type="button" name="SendMail" value="Send" onclick="javascript: SendMail()" />--%>
            <input class="button" type="submit" value="Send" />
            <input class="shortButton" type="button" value="Cancel" onclick="self.close()" />
        </div>
    </div>
     <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <style type="text/css">
        body
        {
            background-color:White;
            }
        input[type="text"], input[type="password"]
{
    width: 600px;
	_width:200px; /*for ie6*/
    color: #333333;
    height:17px;
}
textarea
{
    width:650px;
    height: 200px;
    }
    </style>
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript">
        var SendMail = function() {
            alert('entro');
            var id = '<%= ViewData["LibraryId"] %>';
            alert(id);
            var urlAction = '<%= Url.Action("SendMail", "LibraryNews") %>';
            if (id) {
                alert('entro alert if');
                $.get(urlAction + "?LibraryId=" + id);
                alert('paso el get');
                showReSendEmailToApprovedDialog();
                //openPopup(urlAction + "?LibraryId=" + id);
            }
        };
    </script>
</asp:Content>
