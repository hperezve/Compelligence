<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity"  %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<link href="<%= Url.Content("~/Content/Styles/Site.css") %>" rel="stylesheet" type="text/css" />

<style type="text/css">
     body 
     {
     	background-color:Transparent;     	     	
     }
.commentbuttons
{
    text-align:center;
}     
</style>

<style type=text/css media=all>
    
    .inputFile {
	BORDER-BOTTOM: #bbb 1px solid; 
	BORDER-LEFT: #bbb 1px solid; 	
	BORDER-TOP: #bbb 1px solid; 
	BORDER-RIGHT: #bbb 1px solid;
}
a
{
	text-decoration :none;
}


</style>

	<style type="text/css">
		#div-input-file{
			background:url(<%= ResolveUrl("~/Content/Images/Styles/Browse.jpg") %>) no-repeat 100% 1px;
			height:28px;
			width:333px;
			margin:0px;
			margin-top:5px;
		}
		
		#div-input-file #file-original{
			opacity: 0.0;
			-moz-opacity: 0.0;
			filter: alpha(opacity=00);
			font-size:18px;
			margin-top:-3px;
		}
		
		#div-input-falso{
			margin-top:-28px;
			width:250px;
		}
		#div-input-falso #file-original{
			font-family: Verdana;			
		}
		.discussionTitleReply
		{
			font-family: Verdana;
			height:auto;
			width:460px; 
			background:#EEEFEF; 
			border: 1px solid #AAAAAA;
			margin-left: auto;
            margin-right: auto;			
		}	
				
		
</style>

<div id="HeaderEdit">
<img width="100%" src="<%= Url.Content("~/Content/Images/Styles/TopBarFrontEnd.png") %>" style="margin-bottom: 45px;">      
</div>

<div id="ValidationSummaryWhitePaper">
<%=Html.ValidationSummary()%>
</div>

<div style="float:right;">
<a href="<%= Url.Action("List","MaintenanceAccount") %>">Back to Main Screen</a>
</div>
<div class="discussionTitleReply">

<% using (Html.BeginForm("Save", "ClientCompanyFiles", FormMethod.Post, 
     new { id = "CommentsResponseForm", ENCTYPE = "multipart/form-data" }))
  { %>   
    
       <div id="Container">
       
        <div style="margin-left: 8px; margin-top: 8px;">
            <label for="Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;">Name:</label> 
        </div>
        <br />
        <div style="margin-left: 90px; margin-top: -33px;">
            <%=Html.TextBox("FileName", ((ClientCompanyFiles)ViewData["CompanyFile"]).Name, new { @style = "width: 334px;", id = "FileName" })%> 
            <%=Html.ValidationMessage("FileName", "*")%>        
        </div>
         
        <div style="margin-left: 8px; margin-top: 10px;">
            <label for="Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;">Select File:</label>    
        </div>
        
        <div style="margin-left: 90px; margin-top: -22px;">
        <div id="div-input-file">
			<input name="file-original" width="18px" size="30" id="file-original" onchange="document.getElementById('ClientCompanyPfile').value = this.value;"  type="file">
			<div id="div-input-falso"><input value="" style="height:22px;" name="ClientCompanyPfile" id="ClientCompanyPfile" type="text"></div>
		</div>	
        </div>

        <div style="margin-top: 0px; margin-left: 10px;">
            <label for="Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;">Description:</label>                       
        </div>
        
        <div style="margin-left: 14px; margin-top: 5px;">
            <%=Html.TextArea("Description", ((ClientCompanyFiles)ViewData["CompanyFile"]).Description, new { style = "width:95%; height:97px" })%>             
            <%=Html.ValidationMessage("Description", "*")%>        
        </div>
       
        <div style="margin-left: 14px; margin-top: 7px;">
        
            <input id="Save" class="button" type="submit" value="Save"; style="margin-bottom:10px;"/>        
            
        </div>        
        <div style=" display: none">
            <%=Html.TextBox("ClientCompanyFilesId", ((ClientCompanyFiles)ViewData["CompanyFile"]).Id)%>             
            <%=Html.TextBox("ClientCompanyId", (string)ViewData["ClientCompanyId"])%>             
        </div>
    </div>
                                                                                                                                                                  
</div>
<%} %>
