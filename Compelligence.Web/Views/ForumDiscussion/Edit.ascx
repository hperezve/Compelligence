<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.ForumResponse>" %>

<head>
 <title>Discussion</title>
 <link href="<%= Url.Content("~/Content/Styles/BackEndPopupComment.css") %>" rel="stylesheet"  type="text/css" />  
</head>
<style type="text/css">
    body 
     {
     	background-color:Transparent;
     	margin-left:0px;
     	width :100%;
     }    
    .ertext {
    border-color: darkgray;
    border-style: solid;
    border-width: thin;
    height: 157px;
    width: 650px;
}
</style>
<% string formId = ViewData["Scope"].ToString() + "ForumDiscussionEditForm"; %>

<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>  
<script src="<%= Url.Content("~/Scripts/jquery.filestyle.js") %>" type="text/javascript" charset="utf-8"></script>

<script type="text/javascript">
    $(function() {
        $("input[type=file]").filestyle({
            image: "<%= Url.Content("~/Images/Styles/panelrigth_title_8.jpg") %>",
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
.lineDialog {
   display: table-row;
   clear: both;
   float: left;
   clear: both;
   width: 98%;
}
.fieldDialog
{
    float: left;
    padding:  1px 5px 5px 0;
        vertical-align:middle;
}
.textDialogUpload
{
    height:20px;
    width:280px;
    readonly:readonly;

    }
    .fileUpload { display:none;
                  visibility: hidden;
                   } 

</style>
<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        });
        function reloadDetailList() {
            loadDetailList('<%= Url.Action("GetDetails", (string)ViewData["EntityTypeName"]) %>', getIdFrom('<%= ViewData["Scope"] %>'),
                            '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>DiscussionContent');
        }        
</script>


<% using (Html.BeginForm("Save", null, FormMethod.Post, new
   {
       id = formId,
       enctype = "multipart/form-data"
   }))
   { %>      
<div >
   
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("UserSecurityAccess")%>
        <%= Html.Hidden("EntityLocked")%>
        <%= Html.Hidden("Id", default(decimal)) %>
        <%= Html.Hidden("ParentResponseId")%>
        
        <table style="text-align: left; border:0;">
        <tbody>
        <tr>
            <td colspan="4" rowspan="1">   
            <div>
                
                    <label for="<%= formId %>Response" class="required" style="font-size:13px; font-family:Arial,sans-serif; font-weight: bold; ">
                        <asp:Literal ID="Literal1" runat="server" Text="Response Discussion"/>:</label>                                           
                    <%= Html.TextArea("Response", new { id = formId + "Response", Class = "ertext" })%>
                    <%= Html.ValidationMessage("Response", " " )%>
                    <%= Html.ValidationSummary()%>
               
            </div>
            </td>
         </tr>
         <tr>
            <td>              
               <%=Html.MultiUploadControlPersonalized() %>
            </td>
            <td align ="right" style="vertical-align:bottom">
                <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
                <input class="button" type="reset" value="Reset" />
            </td>
        
        </tr>
  </tbody> 
  </table>     
      

</div>
<% } %>
