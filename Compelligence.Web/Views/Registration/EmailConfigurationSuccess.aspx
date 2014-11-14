<%@ Page Title="Compelligence - Email configuration" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <a href="<%= ViewData["CompanyUrl"] %>">Go to Login Page</a>
    </div>
    <br />
    <div class="titleTextThanks">
        
        <% ClientCompany clientCompany = ViewData["ClientCompanyObject"] as ClientCompany;
           if (clientCompany != null)
           {
        %>
        Thanks for register Email properties for <%= clientCompany.Name %>
        <% }
         %>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
