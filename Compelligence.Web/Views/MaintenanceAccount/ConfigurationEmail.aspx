<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titleTextTwo">
        Configuration Email
    </div>
    <div>
        <div>
            <fieldset>
                <a href="<%= Url.Action("List","MaintenanceAccount") %>">Return</a>
                <%= Html.ValidationSummary()%>
                <% using (Html.BeginForm("ChangeEmail", "MaintenanceAccount", FormMethod.Post, new { id = "idformchangecredential" }))
               { %>
                   <table>
                       <tr>
                        <td>Email:</td>
                        <td>
                        <%--<input name="email" type="text" id="idemail" /><%= Html.ValidationMessage("email", "*")%>--%>
                        <%--<asp:TextBox ID="Email"></asp:TextBox>--%>
                        <%= Html.TextBox("Email",ViewData["Email"].ToString())%>
                        <%= Html.ValidationMessage("email", "*")%>
                        </td>
                       </tr>
                       <tr>
                        <td colspan="2"><input type="submit" value="Change" /></td>
                        </tr>
                   </table>
                <%} %>
            </fieldset>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
