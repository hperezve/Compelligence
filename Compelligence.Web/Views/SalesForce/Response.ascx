<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import namespace="Compelligence.Domain.Entity"  %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<link href="<%= Url.Content("~/Content/Styles/BackEndSite.css") %>" rel="stylesheet" type="text/css" />




<style type="text/css">
     body 
     {
     	background-color:Transparent;
     	margin-left:0px;
     	width :700px;
     }
.commentbuttons
{
    text-align:center;
}     
</style>
<STYLE type=text/css media=all>
    .inputFile {
	BORDER-BOTTOM: 
	#bbb 1px solid; BORDER-LEFT: #bbb 1px solid; 
	HEIGHT: 22px; BORDER-TOP: #bbb 1px solid; 
	BORDER-RIGHT: #bbb 1px solid
	
	
}
a
{
	text-decoration :none;
}
</STYLE>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>" type="text/javascript" ></script>


<div class="discussionTitleReply">
<%=ViewData["ResponseText"] %>  

<% using (Html.BeginForm("CreateResponse", "SalesForce", FormMethod.Post, 
     new { id = "CommentsResponseForm", ENCTYPE = "multipart/form-data" }))
  { %>
    
    <input type="hidden" id="EntityId" name="EntityId" value='<%=ViewData["EntityId"] %>' />
    <input type="hidden" id="ForumResponseId" name="ForumResponseId" value='<%=ViewData["ForumResponseId"] %>' />
    <input type="hidden" id="DomainObjectType" name="DomainObjectType" value='<%=ViewData["DomainObjectType"] %>' />
    <input type="hidden" id="IndustryId" name="IndustryId" value='<%=ViewData["IndustryId"] %>' />
    <input type="hidden" id="CriteriaId" name="CriteriaId" value='<%=ViewData["CriteriaId"] %>' />
    <input type="hidden" id="EntityIdT" name="EntityIdT" value='<%=ViewData["EntityIdT"] %>' />
    <input type="hidden" id="U" name="U" value='<%=ViewData["U"] %>' />
    
    <table>
  <tbody>
    <tr>
      <td colspan="4">
   
       
            <label for="Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;">Comment/Response:</label>
            <%=Html.TextArea("Response", new { style="width:650px;height:157px"})%>
            <%= Html.ValidationMessage("Response", "*")%>
        
   
    </td>
    </tr>
    <tr>
      <td>
      <%=Html.MultiUploadControl() %>                  
      </td>
      <td align ="right" >
      <input class="button" type="submit" value="Save" />
      <input class="button" type="reset" value="Reset" />
      <input class="button" type="button" value="Close" onclick="javascript:self.close()"/>
      </td>
      
    </tr>
  </tbody>
</table>



</div>
<%} %>

