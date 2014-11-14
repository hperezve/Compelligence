<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"  
    Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Company Maintenance</title>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: right">
        <%= Html.ActionLink("Return to Home", "Index", "Home")%>
    </div>
       
    <div class="titleTextOne">
        Welcome to the Forgot  Password of Compelligence Systems</div>
    <div id="contentRegister">
        <%= Html.ValidationSummary()%>
        <div class="marginTop10">
            <fieldset>
                <table width="100%">
                    <tr>
                        <td align="center">
                            <% using (Html.BeginForm("SendForgotPassword", "MaintenanceAccount", FormMethod.Post))
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
                                                                E-mail Account</div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td>
                                            <table width="100px" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <input type="text" tabindex="1" value="" style="width: 180px" name="Username" id="textLogin">
                                                        </td>
                                                        <td>
                                                            <input type="submit" class="submitLogin" tabindex="3" value="Send" name="Login">
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
         </div>
        <div>
          <% if (ViewData["Message"] != null)
             { %>
           <%=ViewData["Message"]%>
           <%} %>
        </div>
</asp:Content>
