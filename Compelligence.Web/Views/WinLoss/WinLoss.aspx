<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndPopupSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>WinLoss Questions and Answers</title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="backLongWinLoss">
        <% Quiz WinLoss = (Quiz)ViewData["WinLoss"]; %>
        <h1 style="margin-left:45px; color:black" ><%= WinLoss.Title %></h1>
        <div id="LongWinLossForm">
            <table style="margin-left:45px; border:1px solid black">
                <% 
                   IList<AnswerDTO> answerList = (IList<AnswerDTO>)ViewData["AnswerList"];
                   foreach (AnswerDTO answerItem in answerList)
                   {
                %>
                <tr>
                    <td style="border: 1px solid black">
                        <%=Html.QuestionControl(answerItem.Question,answerItem.Answer)%>
                    </td>
                </tr>
                <%}%>
            </table>
        </div>
    </div>
</asp:Content>
