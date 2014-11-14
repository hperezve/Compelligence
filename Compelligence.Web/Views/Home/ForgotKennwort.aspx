<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compelligence Home</title>
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
    <link rel="shortcut icon" type="image/x-icon" href="<%= Url.Content("~/Content/Images/Icons/favicon.ico") %>" />
    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/Home.css") %>" type="text/css"
        media="screen" />
    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/jquery.alerts.css") %>"
        type="text/css" media="screen" />
    <style type="text/css">
        .home_bottom ul
        {
            padding-left: 0;
            line-height:30px;
        } 
    </style>

    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/Design/jquery.simple-watermark.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.alerts.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
        $("#Username").simpleWaterMark('Enter your Compelligence-linked email');
        $("#Captcha").simpleWaterMark('Enter verification code');
        });
    </script>

</head>
<body>
    <div id="ContentAll">
        <div align="right">
            <%=Html.ActionLink("Return to Login Page", "Index", "Home", new { style = "margin-right: 5px;text-decoration:none;", @class = "Static" })%>
        </div>
        <div id="lineHeader" style="height: 388px;">
            <div class="line">
                <div id="LoginLogoForgot">
                    <img src="<%= Url.Content("~/Content/Images/Styles/compelligence-logo.png") %>" />
                </div>
            </div>
            <div class="line">
                <div class="itemInLine">
                    <h3 style="text-align: center;">
                        Forgot Password</h3>
                </div>
            </div>
            <div class="line">
                <div class="itemInLine" style="width: 31%;">
                    <% using (Html.BeginForm("ForgotKennwort", "Home", FormMethod.Post, new { id = "form-top" }))
                       { %>
                    <div>
                        <input placeholder="Enter your Compelligence-linked email" type="text" id="Username" name="Username"
                            class="ClassInput" tabindex="1" style="width: 278px; height: 22px;" />
                    </div>
                    <div style="border: 1px solid rgb(164, 204, 229); margin-bottom: 9px; margin-left: 1px;">
                        <img width="240" border="0" height="60" src="<%=Url.Action("ShowCaptchaImage", new { textCaptcha = ViewData["TextCaptcha"]}) %>"
                            alt="MyCaptcha" />
                    </div>
                    <div>
                        <input placeholder="Enter verification code" type="text" id="Captcha" name="Captcha"
                            class="ClassInput" style="width: 188px; height: 22px;" />
                        <input type="submit" name="Login" value="GO" tabindex="3" class="submitLogin" />
                    </div>
                    <%} %>
                </div>
            </div>
            <div class="line">
                <div class="itemInLine" style="background-color: #D2D2D2; width: 35%;">
                    <div style="margin: 0px 20px; padding: 10px; text-align: justify;">
                        <label>
                            A link with a temporary password will be sent to the email account associated with
                            this username</label>
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
