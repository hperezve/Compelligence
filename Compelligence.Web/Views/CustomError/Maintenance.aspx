<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<style type="text/css">    
    .shadow 
        {
            -moz-box-shadow: 3px 3px 4px #111;
            -webkit-box-shadow: 3px 3px 4px #111;
            box-shadow: 3px 3px 4px #111;
            /* IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#111111')";
            /* IE 5.5 - 7 */
            filter: progid:DXImageTransform.Microsoft.Shadow(Strength=4, Direction=135, Color='#111111');
            margin: 0 auto;
            width: 50%;    
        }
        .size
        {
            font-family:Arial,Helvetica,Geneva,sans-serif;
            color:#000000;
            margin:0 auto;
            text-align:center;
            width:80%;
        }
</style>
    <title></title>
</head>
<body>
    <div class="shadow">
        <div id="ImageCompelligence" style="text-align:center">
            <img src="<%= Url.Content("~/Content/Images/Styles/Compelligence.png") %>">
        </div>
        <div class="size">
        <label><h2>Repairs In Progress</h2></label>
        <label>The service you're looking is unavailable at the moment.
               We'll be doing a maintenance the aplication this will be up and running again before long, so please try again soon.
               Thanks for your patience!
        </label>
        <img style="width:100%;" src="<%= Url.Content("~/Content/Images/Styles/webMaintenance.jpg") %>">
        </div>
         
    </div>    
</body>
</html>
