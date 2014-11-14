<%@ Page Title="Compelligence - Register New Account" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.UserProfile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <a href="<%= ViewData["CompanyUrl"] %>">Go to Login Page</a>
    </div>
    <br />
    <div class="titleTextThanks">
            Thank you for registering with Compelligence! Your request has been sent to your company's administrator who will notify you when your account is active.
      <%--  Thanks <%= Model.Name %>, now your request for new user was sent to Admin of your company in Compelligence system. You will be notified for login.--%>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
