<%@ Page Title="Compelligence - Survey" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Survey - <%= ViewData["SurveyTitle"] %></h1>
    <hr />
    <br />
    <h2><%=(string)ViewData["Message"] %></h2>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

