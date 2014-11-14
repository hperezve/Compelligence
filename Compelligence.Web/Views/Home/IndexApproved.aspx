<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">   
<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
<head> 
    <title>Compelligence Home</title>
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1">        
    <link rel="shortcut icon" type="image/x-icon" href="<%= Url.Content("~/Content/Images/Icons/favicon.ico") %>" />    

    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/Home.css") %>"  type="text/css" media="screen" />        
    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/jquery.alerts.css") %>"  type="text/css" media="screen" />        
    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/Design/jquery.simple-watermark.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.alerts.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    

<script type="text/javascript">
    $(function() {
    var IsValidDns = '<%=(bool)ViewData["IsValidDns"]%>';
        if (IsValidDns == 'False') 
        {
            $('#LoginCentre').css('width', '60%');
            $('#Register').css('height', '62px');
        }
        $("#Username").simpleWaterMark('Enter your username');
        $("#Password1").simpleWaterMark('Enter your password'); 
    });
</script>

</head> 
    <body>
        <div Id="ContentAll">
            <div Id="ContentIformation">
                <div id="Msg" style=" color:Black;display:none;"><%=Html.ViewData["message"]%></div>  
                <script type="text/javascript">
                    $(document).ready(function() {
                        if ($("#Msg").html() != '') {
                            jAlert('This company instance has been disabled. Please contact support@&#99compelligence.com for assistance.', 'Information message');
                        }
                    });
                 </script>
            	<div Id="Header">
				
					<div id="LoginLogo">
					    <img src="<%= Url.Content("~/Content/Images/Styles/compelligence-logo.png") %>"/>						
					</div>
					
					<div id="LoginCentre">
                            <% if ((bool)ViewData["ShowRegisterCompany"])
                            { %>
                             <div id="Register">	
							    <a Id="LinkRegister" style="font-color:#000;font-size:61%;" href="<%= Url.Action("RegisterClientCompany", "Registration") %>"> 
							    <%if (!(bool)ViewData["IsValidDns"])
                                {%>
							    <%=ViewData["ShowNameCompany"]%> is not currently registered with Compelligence.<br/>You may register as a new client by clicking here.
							    <%}
                                 else
                                 { %>
							         Register your company with Compelligence
							    <%} %>
							    </a>
							 </div>   
					        <%}
                            else
                            {%>
                              <div Id="RegisterUser">	
							    <a Id="LinkRegister" style="font-color:#000;font-size:61%" href="<%= Url.Action("RegisterUser", "Registration") %>">Register your account with <%= ViewData["ShowNameCompany"]%></a>
                               </div> 
                            <%} %>			    
					</div>
					<!--**********************
					*       Form Login       *
					************************-->					
					<div id="LoginForm">
					  <%if ((bool)ViewData["EnableUp"])
                      {%>
					  <div id="ShowLogin">
                        <% using (Html.BeginForm("LoginToApproveProject", "Home", FormMethod.Post, new { id = "form-top" }))
                           { %>      
                              <%= Html.Hidden("IndexApproved", ViewData["ApprovedProjectId"].ToString())%>
							  <%= Html.Hidden("IndexUserApproved", ViewData["ApprovedUserId"].ToString())%>    
                            <div>															
                                <input placeholder="Enter your username" type="text" id="Username" name="Username" class="ClassInput"  tabindex="1" style="width: 188px; height: 22px;"/>
                            </div>
                            <div>
                               <input placeholder="Enter your password" type="password" id="Password1" name="Kennwort" tabindex="2" class="ClassInput" style="width: 188px; height: 22px;" autocomplete="off" />	    
                            </div>
                            
                            <input type="submit" name="Login" value="GO" tabindex="3" class="submitLogin"/> 
                      		<div style="margin-top: -20px; margin-left: 43px;">
					            <a href="http://twitter.com/compelligence" class="twitter-follow-button" data-show-count="false" >Follow @Compelligence</a>
					            <script src="http://platform.twitter.com/widgets.js" type="text/javascript"></script>
				            </div>
                       <% } %>
                      </div>  
                      <%} %>
                    </div>
                                       
                  <div id="Div3" style="color:White; display:none;">
                   <%=Html.ViewData["message"]%>  
                  </div>                  				
					
				</div>								                
	
            </div>
           <%if (!(bool)ViewData["IsValidDns"])
             { %>
             <div class="home_bottom">
               <div class="home_text">This is not a recognized system on Compelligence. Please verify that you have entered the URL correctly.
               </div>
            </div>
           <%} %>
           <div id="LoginValidation" class="home_bottom">
              <%= Html.ValidationSummary()%>
           </div>
        </div>   
    </body>
</html>