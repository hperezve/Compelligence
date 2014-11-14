<%@ Page Title="Compelligence - Deal Support" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Deal>" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

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
  <script src="<%= Url.Content("~/Scripts/jquery.filestyle.js") %>" type="text/javascript" charset="utf-8"></script>
  <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
  <style type="text/css">
 	
 	#contentleft
  {
 	 width: 70%; 	 
  }
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
.validation-error-text
{
    /*border-color:#FF0000;    */
    color :#FF0000;
} 
</style>  
  
<script type="text/javascript">

    $(function() {
    UpdateUploadTxt();
         $("#tableDiscussionDeal").treeTable({ initialState: "expanded", clickableNodeNames: true });
     });


     function ValidateFormComment(form) {
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
     function RemoveFormComment() 
     {
         var source = $("#FormComment:first");
         source.remove();
     }
     function AddFormComment(id) 
     {
         $("#ParentResponseId").val(id);
         var source = $("#FormComment").clone();
         var target=$("#" + id);
         if (target.find("#FormComment").size() == 0) {
             target.append(source);
         }
         $("#FormComment:first").children('#response').focus();
     };

         
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
    <%--Deal Support  --%>
<asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:LabelResource, DealSupportCommentsDealSupport %>" />
        </h1>
  
    <br />
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <fieldset>
        <legend>
        <%--Deal Information  --%>
<asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsDealInformation %>" />
        </legend>
        <table id="Deal">
            <tr>
                <td  id="Name" class="textBold">
                
                <%--Deal Name:  --%>
<asp:Literal ID="Literal3"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsDealName %>" />
                     
                </td>
                <td  id="DealObjectName">
                    <%= Model.Name %>
                </td>
            </tr>
 <%--           <tr>
                <td  id="Industry" class="textBold">
                    Industry:
                </td>
                <td  id="DealObjectIndustry">
                    <%= Html.Encode((Model.Industry != null) ? Model.Industry.Name : string.Empty) %>
                </td>
            </tr>
            <tr>
                <td id="Competitor" class="textBold">
                    Competitor:
                </td>
                <td id="DealObjectCompetitor">
                    <%= Html.Encode((Model.Competitor != null) ? Model.Competitor.Name : string.Empty)%>
                </td>
            </tr>
            <tr>
                <td id="Product" class="textBold">
                    Product:
                </td>
                <td id="DealObjectProduct">
                    <%= Html.Encode((Model.Product != null) ? Model.Product.Name : string.Empty)%>
                </td>
            </tr>--%>
            <tr>
                <td id="Size" class="textBold">
                
                <%--Deal Size:  --%>
<asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsDealSize %>" />
                
                    
                </td>
                <td id="DealObjectSize">
                    <%= Html.Encode(Model.CurrencyId) %>
                </td>
            </tr>
            <tr>
                <td id="OpenedBy" class="textBold">
                
                 <%--Deal Opened By:  --%>
<asp:Literal ID="Literal5"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsDealOpenedBy %>" />
                    
                </td>
                <td id="DealObjectOpenedBy">
                    <%= Html.Encode(Model.CreatedByName)%>
                </td>
            </tr>
            <tr>
                <td id="AccountManager" class="textBold">
                
                <%--Competitor Account Manager:  --%>
<asp:Literal ID="Literal6"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsCompetitorAccountManager %>" />
                    
                </td>
                <td id="DealObjectAccountManager">
                    <%= Html.Encode(Model.CompetitorAccountManager)%>
                </td>
            </tr>
            <tr>
                <td id="AccountStrategy" class="textBold">
                <%--Competitor Account Strategy:  --%>
<asp:Literal ID="Literal7"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsCompetitorAccountStrategy %>" />
                    
                </td>
                <td id="DealObjectAccountStrategy">
                    <p>
                        <%= Html.Encode(Model.CompetitorAccountStrategy)%>
                    </p>
                </td>
            </tr>
            <tr>
                <td id="RequestSupported" class="textBold">
                <%--Request Supported:  --%>
<asp:Literal ID="Literal8"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsRequestSupported %>" />
                    
                </td>
                <td id="DealObjectRequestSupported">
                    <p>
                        <%= Html.Encode(Model.RequestSupported)%>
                    </p>
                </td>
            </tr>
            <tr>
                <td id="Details" class="textBold">
                
                <%--Details:  --%>
<asp:Literal ID="Literal9"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsDetails %>" />
                    
                </td>
                <td id="DealObjectDetails">
                    <p>
                        <%= Html.Encode(Model.Details)%>
                    </p>
                </td>
            </tr>
            <!--here we have "Amount" and "Expected Revenue", those were remove by ticket 1676-->
            <tr>
                <td id="Files">
                <%--Files Attached:--%>
                <% IList<Library> libraries = (IList<Library>)ViewData["Libraries"];
                   if (libraries != null && libraries.Count > 0)
                   {%>
                <asp:Literal ID="Literal12"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsFilesAttached %>" />
                    <%} %>
                    <br />
                    <ul>
                        <% foreach (Library library in (IList<Library>)ViewData["Libraries"])
                           { %>
                        <li>
                            <a href="javascript: void(0);" onclick="return downloadFile('<%= Url.Action("Download", "DealSupport") + "/" + library.Id %>');"><%= Html.Encode(library.FileName)%></a>
                        </li>
                        <% } %>
                    </ul>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <asp:Panel ID="OldComments" runat="server">
         <%--<%=Html.CommentControl("Download", "DealSupport", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>--%>
          <%=Html.CommentThreds("Download", "DealSupport", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>
        </asp:Panel>
    </fieldset>
    <div id="dialog" title="Discussion Dialog" style="display:none">Your comment was added.</div>
    <fieldset>
        <legend>
         <%--Add comment--%>
<asp:Literal ID="Literal13"  runat="server" Text="<%$ Resources:LabelResource, DelSupportCommentsAddComment %>" />
        </legend>
        <% using (Html.BeginForm("SendComment", "DealSupport", FormMethod.Post, new
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

<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
    <% Html.RenderPartial("Options"); %>
</asp:Content>
