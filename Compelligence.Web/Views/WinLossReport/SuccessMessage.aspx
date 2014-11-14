<%@ Page Title="Compelligence - Win/Loss Report" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSiteOnly.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   <%-- <h1>
        Win/Loss Report - <%= ViewData["WinLossTitle"] %></h1>
    <hr />--%>
    <br />
    <h2><%=(string)ViewData["Message"] %></h2>
    <div id="detailWinLoss" style="display:none"></div>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>

