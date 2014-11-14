<%@ Page Title="Compelligence - Project" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Project>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
  <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
  <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
  <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

  <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
  <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
  <script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>
  <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
  <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

<script type="text/javascript">

    $(function()
     {
         $("#tableDiscussionProject").treeTable({ initialState: "expanded", clickableNodeNames: true });
     });

//     var setResponseId = function(id) {
//         $("#ParentResponseId").val(id);
//     };
     //
     function ValidateFormComment(form) 
     {
         if (form.response.value == "") 
         {
             return false;
         }
         return true;
     }
     function RemoveFormComment() {
         var source = $("#FormComment:first");
         source.remove();
     }
     function AddFormComment(id) {

         var source = $("#FormComment").clone();
         source.children("#ParentResponseId").val(id);
         var target = $('#comentarios').find("#" + id);
         source.append("<input type='button' value='Cancel' class='shortButton' onclick='RemoveFormComment()' />");
         target.append(source);
         $("#FormComment:first").children('#response').focus();
     };
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <label for="<%= formId %>ProjectCommentsInformation">
                    <asp:Literal ID="ProjectCommentsInformation" runat="server" Text="<%$ Resources:LabelResource, ProjectCommentsInformation %>" />:</label> </h1>
    <hr />
    <br />
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <fieldset>
        <legend>
        <label><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:LabelResource, ProjectsCommentsName %>" />:</label>
        </legend>
        <table id="Project">
            <tr>
                <td class="textBold">
                    <label>
                    <asp:Literal ID="ProjectsCommentsName" runat="server" Text="<%$ Resources:LabelResource, ProjectsCommentsName %>" />:</label>
                </td>
                <td >
                    <%= Model.Name %>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                    <label>
                    <asp:Literal ID="ProjectCommentsOpenedDate" runat="server" Text="<%$ Resources:LabelResource, ProjectCommentsOpenedDate %>" />:</label>
                </td>
                <td >
                    <%= Model.CreatedDate %>
                </td>
            </tr>
            <tr>
                <td class="textBold">
                <label>
                <asp:Literal ID="ProjectCommentsDueDate" runat="server" Text="<%$ Resources:LabelResource, ProjectCommentsDueDate %>" />:</label>
                    
                </td>
                <td >
                    <%= Model.DueDate%>
                </td>
            </tr>
            <tr>
                <td id="Files">                 
                 <label>
                <asp:Literal ID="ProjectCommentFAttached" runat="server" Text="<%$ Resources:LabelResource, ProjectCommentFAttached %>" />:</label>                                 
                 
                    <br />
                    <ul>
                        <% foreach (Library library in (IList<Library>)ViewData["Libraries"]){ %>
                        <li>
                            <a href="javascript: void(0);" onclick="return downloadFile('<%= Url.Action("Download", "Project") + "/" + library.Id %>');"><%= Html.Encode(library.FileName) %></a>
                        </li>
                        <% } %>
                    </ul>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <asp:Panel ID="OldComments" runat="server">
         <%--<%=Html.CommentControl("Download", "Project", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>--%>
          <%=Html.CommentThreds("Download", "Project", Model.Id, (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>
        </asp:Panel>
    </fieldset>
    <fieldset>
        <legend>ProjectsAddComment
        <label for="<%= formId %>ProjectsAddComment">
                <asp:Literal ID="ProjectsAddComment" runat="server" Text="<%$ Resources:LabelResource, ProjectsAddComment %>" /></label>
        
        </legend>
        <% using (Html.BeginForm("SendComment", "Project", FormMethod.Post, new
           {
               id = "FormComment",
               enctype = "multipart/form-data",
               onsubmit="return ValidateFormComment(this)"
           }))
           { %>
        <input id="Id" type="hidden" name="Id" value="<%= Model.Id %>" />
        <input id="ParentResponseId" type="hidden" name="ParentResponseId" />
        <textarea id="response" name="Comment" cols="72" rows="10"></textarea>
        <%=Html.MultiUploadControl()%>
        <input type="submit" value="Send" class="shortButton" />
        <%} %>
    </fieldset>    
    <div id="CommentForm"></div>
</asp:Content>

<%--<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
    <% Html.RenderPartial("Options"); %>
</asp:Content>--%>
