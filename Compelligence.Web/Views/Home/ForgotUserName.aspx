<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Forgot User Name</title>
    <link rel="shortcut icon" type="image/x-icon" href="<%= Url.Content("~/Content/Images/Icons/favicon.ico") %>" />    

    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/Home.css") %>"  type="text/css" media="screen" />        
    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/jquery.alerts.css") %>"  type="text/css" media="screen" />        
    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/Design/jquery.simple-watermark.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.alerts.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    <style type="text/css">
        .home_bottom ul
        {
            padding-left: 0;
            line-height:30px;
        }       
    </style>
</head>
<body>
    <div id="ContentAll">
        <div align="right">
            <%=Html.ActionLink("Return to Login Page", "Index", "Home", new { style = "margin-right: 5px;text-decoration:none;", @class = "Static" })%>
        </div>
        <div id="lineHeader" style="height: 75%;">
        <div class="line">
            <div id="LoginLogoForgot">
                <img src="<%= Url.Content("~/Content/Images/Styles/compelligence-logo.png") %>" />
            </div>
        </div>
        <div class="line">
            <div class="itemInLine">
                <h3 style="text-align: center;">
                    Forgot User Name</h3>
            </div>
        </div>
        <div class="line">
            <div class="itemInLine" style="width: 100%;">
                <% using (Html.BeginForm("ForgotUserName", "Home", FormMethod.Post, new { id = "form-top" }))
                   { %>
                <div style="display: table;margin: 0 auto;">
                    <table>
                    <tr>
                    <td><label>First Name: </label></td>
                    <td><input id="FirstName" name="FirstName" style="width: 188px; height: 22px;" /></td>
                    </tr>
                    <tr>
                    <td><label>Last Name: </label></td>
                    <td><input id="LastName" name="LastName" style="width: 188px; height: 22px;" /></td>
                    </tr>
                    <tr>
                    <td><label>Email: </label></td>
                    <td><input id="Email" name="Email" style="width: 188px; height: 22px;" /></td>
                    </tr>
                    <tr>
                    <td><label>Phone</label></td>
                    <td><input id="Phone" name="Phone" style="width: 188px; height: 22px;" /></td>
                    </tr>
                    <tr><td></td>
                        <td>
                             <div style="border: 1px solid rgb(164, 204, 229); margin-bottom: 9px;">
                        <img width="240" border="0" height="60" src="<%=Url.Action("ShowCaptchaImage", new { textCaptcha = ViewData["TextCaptcha"]}) %>"
                            alt="MyCaptcha" />
                    </div>
                        </td>
                    </tr>
                    <tr>
                    <td>
                        <label>Verification Code: </label>
                    </td>
                    <td>
                        <input type="text" id="Captcha" name="Captcha"
                            class="ClassInput" style="width: 188px; height: 22px;" />
                    </td>
                    </tr>                    
                    <tr>
                        <td></td>
                        <td><input type="submit" name="Send" value="Send" tabindex="3" class="submitLogin" style="width: 44px"/></td>
                    </tr>
                    </table>                     
                </div>
                <%} %>
            </div>
        </div>
        <div class="line">
            <div class="itemInLine" style="background-color:#D2D2D2;width: 35%;">
               <div  style="margin: 0px 20px; padding: 10px; text-align: justify;">
               <label>
                    A request for your username will be sent to the system administrator,
                    who will contact you</label>
               </div>
            </div>
        </div>
        </div>
        <div id="LoginValidation" class="home_bottom">
            <%= Html.ValidationSummary()%>
        </div>
    </div>
</body>
</html>
