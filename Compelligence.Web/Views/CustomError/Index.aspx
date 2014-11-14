<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
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
     <script type="text/javaScript">
            var interval;
            var c = 60;
            function cuenta(){
                interval = setInterval(iniciar,1000);
            }
            function iniciar(){
                if(c>0){
                    c--;
                    document.getElementById("b1").value=c;
                }
                else{
                   location.href = '<%= ViewData["Url"]%>';
                }
            }
        </script>
</head>
<body onload="cuenta()">    
    <div class="shadow">
        <div id="ImageCompelligence" style="text-align:center">
            <img src="<%= Url.Content("~/Content/Images/Styles/Compelligence.png") %>">
        </div>
        <div class="size">
        <label><h2>An Error Has Ocurred</h2></label>
        <br />
        <label>There was an error in the application of Compelligence, we are working on this bug. We apologize for any inconvenience this to end in a few seconds</label>    
             <div id="LoadingDialog" >
        <p>
            <img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                class="left" /><span class="loadingDialog"><input type="button" value="60" id="b1" disabled="" style="width:100%;color:#000;background: none repeat scroll 0 0 transparent;border: medium none;" />
                </span>
        </p>
        </div>
        
        </div>
    </div>
</body>
</html>
