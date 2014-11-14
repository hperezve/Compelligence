<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Compelligence Home</title>
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">
    <link rel="shortcut icon" type="image/x-icon" href="<%= Url.Content("~/Content/Images/Icons/favicon.ico") %>" />
    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/Home.css") %>" type="text/css"
        media="screen" />
    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/jquery.alerts.css") %>"
        type="text/css" media="screen" />

    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/Design/jquery.simple-watermark.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.alerts.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#Username").simpleWaterMark('Enter your username');
        });
    </script>

</head>
<body>
    <div id="ContentAll">
        <div align="right">
            <%=Html.ActionLink("Return to Login Page", "Index", "Home", new { style = "margin-right: 5px;text-decoration:none;", @class = "Static" })%>
        </div>
        <div id="lineHeader">
            <div class="line">
                <div id="LoginLogoForgot">
                    <img src="<%= Url.Content("~/Content/Images/Styles/compelligence-logo.png") %>" />
                </div>
            </div>
            <div class="line">
                <div class="itemInLine" style="background-color: #D2D2D2; width: 35%;">
                    <div style="margin: 0px 20px; padding: 10px; text-align: justify;">
                        <label>
                            An email will be sent to the system administrator, who will contact you.</label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
