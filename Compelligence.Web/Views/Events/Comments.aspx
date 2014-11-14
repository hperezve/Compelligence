<%@ Page Title="Compelligence - Events" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Event>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
 <%-- <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
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

<script type="text/javascript">
    $(function() {
    UpdateUploadTxt();
        $("#tableDiscussionEvent").treeTable({ initialState: "expanded", clickableNodeNames: true });
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
    function RemoveFormComment() {
        var source = $("#FormComment:first");
        source.remove();
    }
    function AddFormComment(id) {

        var source = $("#FormComment").clone();
        source.children("#ParentResponseId").val(id);
        var target = $("#" + id);
        if (target.find("#FormComment").size() == 0) {
            target.append(source);
        }
        $("#FormComment:first").children('#response').focus();
    };
</script>
<style type="text/css">
 #contentleft
 {
 	 width: 70%; 
 }
/*  	    .validation-summary-errors {
    border: 1px solid #AAAAAA;
    color: #FF0000;
    min-height: 24px;
}*/
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
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dialog" title="Discussion Dialog" style="display:none">Your comment was added.</div>
    <h1>
        Events</h1>
    <br />
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <fieldset>
        <legend>Event Information</legend>
        <table id="Event">
            <tr>
                <td class="textBold">
                    Event Name:
                </td>
                <td >
                    <%= Model.EventName %>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                    Comment:
                </td>
                <td >
                    <%= Model.Comment %>
                </td>
            </tr>

            <tr>
                <td class="textBold">
                    <%=ViewData["IndustryLabel"]%>:
                </td>
                <td >
                    <%= Html.Encode(Model.NamesIndustries) %>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                    <%=ViewData["CompetitorLabel"]%>:
                </td>
                <td >
                    <%= Html.Encode(Model.NamesCompetitors)%>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                    <%=ViewData["ProductLabel"]%>:
                </td>
                <td >
                    <%= Html.Encode(Model.NamesProducts)%>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                    Opened By:
                </td>
                <td >
                    <%= Html.Encode(Model.CreatedByFrm)%>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                   <%IList<Library> libraries=(IList<Library>)ViewData["Libraries"]; 
                     if( libraries!=null && libraries.Count>0 )
                     {%>
                    Files Attached:
                    <%}%>
                </td>
                <td>    
                    <ul>
                        <% foreach (Library library in libraries){ %>
                            <% if (!string.IsNullOrEmpty(library.FileName))
                               { %>
                        <li>
                            <a href="javascript: void(0);" onclick="return downloadFile('<%= Url.Action("Download", "Events") + "/" + library.Id %>');"><%= Html.Encode(library.FileName)%></a>
                        </li>
                        <% } %>
                        <% } %>
                    </ul>
                </td>
            </tr>
        </table>
    </fieldset>

    <fieldset>
        <%--<legend>Comments</legend>--%>
        <asp:Panel ID="OldComments" runat="server">

            <%--<%=Html.CommentControl("Download", "Events", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>--%>
             <%=Html.CommentThreds("Download", "Events", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>
            <%--<table id="tableDiscussionEvent" class="simpleForumGrid" style="margin-left:20px">
                <thead>
                    <tr>
                        <th> Comment    </th>
                        <th> CreatedBy   </th>
                        <th> CreatedDate </th>
                        <th>      </th>
                    </tr>
                </thead>
                <tbody>
                    <% List<ForumResponse> Comments = (List<ForumResponse>)ViewData["Comments"]; %>
                    <%foreach (ForumResponse fr in Comments)
                      {
                          var classparent = string.Empty;
                          if (fr.ParentResponseId != null && fr.ParentResponseId > 0)
                          {
                              classparent = "child-of-n" + fr.ParentResponseId.ToString();
                          }        %>
                    <tr id="n<%=fr.Id %>" class="<%=classparent %>">
                        <td class="columnDiscussion" style="background: white">
                            <span class="DiscussionName">
                                <img src="<%= Url.Content("~/content/Images/Styles/comment.png") %>" /><%=fr.Response%></span>
                             <br />
                            <% if (fr.Libraries.Count > 0)
                               { %>
                                <ul>
                                    <% foreach (Library library in fr.Libraries)
                                       {
                                           File file = library.File;
                                    %>
                                    <li>
                                        <a href="javascript: void(0);" onclick="return downloadFile('<%= Url.Action("Download", "Events") + "/" + library.Id %>');"><%= Html.Encode(library.FileName) %></a>
                                    </li>
                                    <% } %>
                                </ul>
                            <%} %>   
                        </td>
                        <td class="columnCreateBy" style="background: white">
                            <%=fr.CreatedByName%>
                        </td>
                        <td class="columnCreateDate" style="background: white">
                            <%=fr.CreatedDate%>
                        </td>
                        <td style="background: white">
                            <a href="javascript: void(0)" onclick="javascript: setResponseId('<%= fr.Id %>');">
                               Reply </a>                                                        
                        </td>
                    </tr>
                    <% } %>
                </tbody>
            </table>--%>
        </asp:Panel>
    </fieldset>
    <fieldset>
        <legend>Add comment</legend>
        <% using (Html.BeginForm("SendComment", "Events", FormMethod.Post, new
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