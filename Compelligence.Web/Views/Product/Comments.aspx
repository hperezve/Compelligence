<%@ Page Title="Compelligence - Product" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSiteOnly.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Product>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
  <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
  <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
  <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

  <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
  <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
  <script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>
  <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
  <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
  <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>  
  <style type="text/css">
    .validation-summary-errors-forum
{
    margin-left: 0;
    margin-right: 0;    
}
.validation-error-items
{
    /*border-color:#FF0000;    */
    border :1px solid #FF0000;
}
  </style>  
<script type="text/javascript">
    $(function() {
        UpdateUploadTxt();
         $("#tableDiscussionProduct").treeTable({ initialState: "expanded", clickableNodeNames: true });
     });

     function ValidateFormComment(form) 
     {
         var message = '';
         var success = true;
         if (form.response.value == "") {
             $("#response").addClass('validation-error-items');
             message = '<li>Comment is required</li>';
             success = false;
         } else {
             $("#response").removeClass('validation-error-items');
         }
         var inputFiles = $('input:file[name=uploadfile]');
         if (inputFiles != undefined && inputFiles != null && inputFiles.length > 0) {
             var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
             for (var i = 0; i < inputFiles.length; i++) {
                 var v = inputFiles[i].value;
                 var file = inputFiles[i];
                 var fileId = file.id;
                 if (v != undefined && v != null && v != '') {
                     if (file.files != null || file.files != undefined) {
                         if (file.files.length != null && file.files.length != undefined && file.files.length > 0) {
                             file = file.files[0];
                         }
                     }
                     var response = '';
                     if (file) {
                         var fd = new FormData();
                         fd.append('file', file);
                         var xhr = new XMLHttpRequest();

                         xhr.onreadystatechange = function() {
                             if (xhr.readyState == 4 && xhr.status == 200) {
                                 response = xhr.responseText;
                                 fileId = fileId.replace('uploadfile', '');
                                 if (response == 'Sucessfull') {
                                     $("#TxtShowValue" + fileId).removeClass('validation-error-items');
                                 }
                                 else if (response == 'UnSucessfull') {
                                     success = false;
                                     message = message + '<li>The file ' + v + ' was not successfully uploaded. Please re-submit</li> ';
                                     $("#TxtShowValue" + fileId).addClass('validation-error-items');
                                 } else {
                                     $("#TxtShowValue" + fileId).removeClass('validation-error-items');
                                 }
                             }
                         };
                         xhr.open('POST', urlValidate, false);
                         xhr.send(fd);

                     }
                 } else {
                     // file is empty
                     message = message + '<li>The file is empty</li> ';
                     success = false;
                     fileId = fileId.replace('uploadfile', '');
                     $("#TxtShowValue" + fileId).addClass('validation-error-items');
                 }
             }
         }
         if (!success) {
             $(".error").html('<ul class="validation-summary-errors validation-summary-errors-forum">' + message + '</ul>');
             return false;
         }
         return true;
     }
     $(function() {
     var showMessage = '<%= ViewData["showResultMessage"] %>';
         if (showMessage == 'true') {
             $("#dialog").dialog({                 
                 buttons: {
                     Ok: function() {
                         $(this).dialog('close');
                     }
                 }
             });
         }        
     });
     function RemoveFormComment() {
         var source = $("#FormComment:first");
         source.remove();
     }
     function AddFormComment(id) {
         var source = $("#FormComment").clone();
         source.children("#ParentResponseId").val(id);
         var target = $('#comentarios').find("#" + id);
         if (target.find("#FormComment").size() == 0) {
             target.append(source);
         }
         $("#FormComment:first").children('#response').focus();
     };
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dialog" title="Discussion Dialog" style="display:none">Your comment was added.</div>
    <h1>
        <%=ViewData["ProductLabel"]%></h1>
    <hr />
    <br />
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <fieldset>
        <legend><%=ViewData["ProductLabel"]%> Information</legend>
        <table id="Product">
            <tr>
                <td class="textBold">
                    <%=ViewData["ProductLabel"]%> Name:
                </td>
                <td >
                    <%= Model.Name %>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                    Created By:
                </td>
                <td >
                    <%= Model.CreatedBy %>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                    Created Date:
                </td>
                <td >
                    <%= Model.CreatedDate%>
                </td>
            </tr>
            <tr>
                <td id="Files">
                    Files Attached
                    <br />
                    <ul>
                        <% foreach (Library library in (IList<Library>)ViewData["Libraries"]){ %>
                        <li>
                            <a href="javascript: void(0);" onclick="return downloadFile('<%= Url.Action("DownloadExecute", "Library") + "/" + library.Id %>');"><%= Html.Encode(library.FileName) %></a>
                        </li>
                        <% } %>
                    </ul>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <asp:Panel ID="OldComments" runat="server">
         <%--<%=Html.CommentControl("Download", "Product", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>--%>
          <%=Html.CommentThreds("DownloadExecute", "Product", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>
        </asp:Panel>
    </fieldset>
    <fieldset>
        <legend>Add comment</legend>
        <% using (Html.BeginForm("SendComment", "Product", FormMethod.Post, new
           {
               id = "FormComment",
               enctype = "multipart/form-data",
               onsubmit="return ValidateFormComment(this)"
           }))
           { %>
        <div class="error" style="color: red;"></div>
        <input id="Id" type="hidden" name="Id" value="<%= Model.Id %>" />
        <input id="ParentResponseId" type="hidden" name="ParentResponseId" />
        <textarea id="response" name="Comment" cols="150" rows="10"></textarea>
        <%=Html.MultiUploadControl()%>
        <input type="submit" value="Send" class="shortButton" />
        <input type='button' value='Cancel' class='shortButton' onclick='RemoveFormComment()' />
        <%} %>
    </fieldset>    
    <div id="CommentForm"></div>
</asp:Content>

