<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Company Maintenance</title>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<style>
.Messages
{
    display:none;
}
</style>
    <div style="text-align: right">
        <%= Html.ActionLink("Return to Home", "Index", "Home")%>
    </div>
    <div class="titleTextOne">
        Welcome to the Maintenance Page of Compelligence Systems</div>
    <div class="titleTextTwo">
        your Competitive Intelligence Systems!!</div>
    <div id="contentRegister" align="center">
        <%= Html.ValidationSummary()%>
        <div class="marginTop10" align="center" style="width:500px;">
            <fieldset>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <% using (Html.BeginForm("Login", "MaintenanceAccount", FormMethod.Post))
                               { %>
                            <table >
                                <tbody>
                                    <tr>
                                        <td>
                                            <table width="50px">
                                                <tbody>
                                                    <tr>
                                                        <td class="style1">
                                                            <div class="labelLogin" style="text-align: right">
                                                                Login</div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="labelPassword">
                                                                Password</div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        <div class="validation"><span><%= Html.ValidationSummary() %></span></div>
                                                         </div>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td>
                                            <table width="100px" border="1">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <input type="text" tabindex="1" value="" style="width: 180px" name="Username" id="textLogin">
                                                        </td>
                                                        <td>
                                                            <input type="submit" class="submitLogin" tabindex="3" value="GO" name="Login">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <input type="password" style="width: 180px" tabindex="2" name="Kennwort" id="Password2" autocomplete="off">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <%} %>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
</asp:Content>
