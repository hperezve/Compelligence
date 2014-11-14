<%@ Page Title="Compelligence - Register New Account" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master" Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.AccountRegistrationDTO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <a href="<%= ViewData["CompanyUrl"] %>">Go to Login Page</a>
    </div>
    <br />
    <div class="titleTextThanks">
        <%--Thanks your company <%= Model.ClientCompanyName %> is now a new client in Compelligence system. You will be contacted by support to complete the installation process--%>
        Thank you for registering with Compelligence! <%= Model.ClientCompanyName %> is now configured in the system. You will soon be contacted by an admin who will activate your account.
        <br />
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
