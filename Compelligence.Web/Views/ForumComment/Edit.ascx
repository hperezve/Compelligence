<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.ForumResponse>" %>
<head>
 <title>Comment</title>
 <link href="<%= Url.Content("~/Content/Styles/BackEndPopupComment.css") %>" rel="stylesheet"  type="text/css" /> 
</head>
<% string formId = ViewData["Scope"].ToString() + "ForumCommentEditForm"; %>

  <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script> 
  <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>  
  <script src="<%= Url.Content("~/Scripts/jquery.filestyle.js") %>" type="text/javascript" charset="utf-8"></script>


<script type="text/javascript">
    $(function() {
        $("input[type=file]").filestyle({
            image: '<%= Url.Content("~/Images/Styles/panelrigth_title_8.jpg") %>',
            imageheight: 20,
            imagewidth: 65,
            width: 280
        });
    });  
</script>

<style type="text/css"> 
.input-validation-error
{
    border: 1px solid red;
    background-color: #FFFFff;
    padding: 1px;
}
.validation-summary-errors
{
	color: #ff0000;
	font-weight: bold;
}

</style>



<% using (Html.BeginForm("Save", null, FormMethod.Post, new
   {
       id = formId,
       enctype = "multipart/form-data"
   }))
   { %>   
<div class="indexTwo">
    <fieldset style="width: auto; height: 100%;">
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("UserSecurityAccess")%>
        <%= Html.Hidden("EntityLocked")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("ParentResponseId")%>
        <div>
        <table style="text-align: left; border:1; width:97%; height:100%;">
        <tbody>
        <tr>
        <td colspan="4" rowspan="1">    
           <div>
         <label for="<%= formId %>Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold;">
         <asp:Literal ID="ForumCommentResponse" runat="server" Text="Response Discussion" />:</label>
         <%= Html.TextArea("Response", new { id = formId + "Response",  style = "width:100%; height:130px;" })%>
         <%= Html.ValidationMessage("Response", " ")%>
         <%= Html.ValidationSummary()%>
         </div>       
        </td>
        </tr>
        <tr>
          <td><%=Html.MultiUploadControlPersonalized() %></td>
          <td align ="right">
          <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
          <input class="button" type="reset" value="Reset" />
          <input class="button" type="button" value="Close" onclick="javascript:self.close()"/>
          </td>      
       </tr>
  </tbody>
</table>
            
            
        </div>

        
    </fieldset>
</div>
<% } %>


