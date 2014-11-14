<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import namespace="Compelligence.Domain.Entity"  %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<link href="<%= Url.Content("~/Content/Styles/BackEndPopupComment.css") %>" rel="stylesheet"  type="text/css" /> 




<style type="text/css">
     body 
     {
     	background-color:Transparent;
     	margin-left:0px;
     	width :100%;
     }
.commentbuttons
{
    text-align:center;
}    
.disableButtom
    {
        background: none repeat scroll 0 0 transparent !important;
        cursor: pointer !important;
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
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script> 
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>  
<script src="<%= Url.Content("~/Scripts/jquery.filestyle.js") %>" type="text/javascript" charset="utf-8"></script>


<script type="text/javascript">

    $(function() {
        $("input[type=file]").filestyle({
            image: "url(<%= ResolveUrl("~/Images/Styles/panelrigth_title_8.jpg") %>)",
            imageheight: 22,
            imagewidth: 65,
            width: 280
        });
        inspectTextArea();
    });
    var AddComentsclassImage = function() {
        $('#ImgComents' + '<%= ViewData["EntityId"]%>', window.parent.document).removeClass("ImageCommentsN");
        $('#ImgComents' + '<%= ViewData["EntityId"]%>', window.parent.document).addClass("ImageCommentsY");
    };
    function inspectTextArea() {
        var textArea = $("#commentExternalId").val();
        if (textArea === '') {
            $('#saveMessage').attr('disabled', 'disabled');
            $('#saveMessage').addClass('disableButtom');
        }
        else {
            $('#saveMessage').removeAttr('disabled');
            $('#saveMessage').removeClass('disableButtom');
        }
    }
</script>

<div class="discussionTitleReply">
<%=ViewData["ResponseText"] %>  

<% using (Html.BeginForm("CommentsResponseSave", "Forum", FormMethod.Post, 
     new { id = "CommentsResponseForm", ENCTYPE = "multipart/form-data" }))
  { %>
    <%=Html.Hidden("U",(string)ViewData["U"]) %>
    <%=Html.Hidden("C",(string)ViewData["C"]) %>

    <input type="hidden" id="EntityId" name="EntityId" value='<%=ViewData["EntityId"] %>' />
    <input type="hidden" id="ForumResponseId" name="ForumResponseId" value='<%=ViewData["ForumResponseId"] %>' />
    <input type="hidden" id="DomainObjectType" name="DomainObjectType" value='<%=ViewData["DomainObjectType"] %>' />
    <input type="hidden" id="IndustryId" name="IndustryId" value='<%=ViewData["IndustryId"] %>' />
    <input type="hidden" id="CriteriaId" name="CriteriaId" value='<%=ViewData["CriteriaId"] %>' />
    <input type="hidden" id="EntityIdT" name="EntityIdT" value='<%=ViewData["EntityIdT"] %>' />
    
    <table>
  <tbody>
    <tr>
      <td colspan="4" style=" width:250;">
   
       
            <label for="Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;">Comment/Response:</label>
            <%=Html.TextArea("Response", new { id = "commentExternalId", style = "width:650px;height:157px", onkeyup = "inspectTextArea()" })%>
            <%= Html.ValidationMessage("Response", "*")%>
        
   
    </td>
    </tr>
    <tr>
      <td>
      <%=Html.MultiUploadControlPersonalized() %>                  
      </td>
      <td align ="right">
      <input id="saveMessage" class="button" type="submit" value="Save" onclick="AddComentsclassImage();"/>
      <input class="button" type="reset" value="Reset" />
      
      </td>
      
    </tr>
  </tbody>
</table>



</div>
<%} %>

