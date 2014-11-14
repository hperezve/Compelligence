<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Forum>" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function() {
        $("#tableComment").treeTable({ initialState: "expanded", clickableNodeNames: true });
    });

    function removeComment(forumresponseid, objecttype,entityid) {
        var urlAction = '<%=Url.Action("RemoveComments","Forum") %>';
        $.get(urlAction + '?ObjectType=' + objecttype + '&EntityId=' + entityid + '&forumresponseid=' + forumresponseid,
         {}, function(data) {
           $("#FormComments").dialog("destroy");//location.reload(true);
         });
    }
    
</script>


<%var entityid = ViewData["EntityId"]; %>

<div style="margin-left:20px; width:95.5%;">
    <table id="tableComment" class="simpleForumGrid" >
        <thead>
            <tr>
                <th>Comments</th>
                <th>CreateBy</th>
                <th>CreateDate</th>
                <th>Reply </th>
            </tr>
        </thead>
        <tbody>
            <%IList<ForumResponse> forumresponses = (IList<ForumResponse>)ViewData["Comments"];
              foreach (ForumResponse forumresponse in forumresponses)
              {
                  var classparent = string.Empty;
                  if (forumresponse.ParentResponseId != null && forumresponse.ParentResponseId > 0)
                  {
                      classparent = "child-of-n" + forumresponse.ParentResponseId.ToString();
                  } %>
            <tr id="n<%=forumresponse.Id %>" class="<%=classparent %>">
                <td class="columnDiscussion" style="background: white">
                    <span class="discussionName">
                        <img src="<%= Url.Content("~/content/Images/Styles/comment.png") %>" /><%=forumresponse.Response%>
                    </span>
                    <%
                  if (forumresponse.Libraries.Count > 0)
                       { %>
                 
                        <ul>
                            <% foreach (Library library in forumresponse.Libraries)
                               {
                                   File file = library.File;
                            %>
                            <li>
                                <% =Html.ActionLink(library.FileName, "DownloadExecute", "Library", new { Id = library.Id }, null)%>
                            </li>
                            <% } %>
                        </ul>
                     <%} %>                      
                </td>
                <td class="columnCreateBy" style="background: white">
                    <%=forumresponse.CreatedByName%>
                </td>
                <td class="columnCreateDate" style="background: white">
                    <%=forumresponse.CreatedDate%>
                </td>
                <td style="background: white" align="center">
                   <% if ( forumresponse.HasAccess )
                      {
                           var urlAction=Url.Action("RemoveComments","Forum",new {ObjectType=(string)ViewData["ObjectType"],EntityId=(decimal)ViewData["EntityId"],forumresponseid=forumresponse.Id});
                           var urlActionParent = Url.Action("Comments", "SalesForce", new { EntityId = (decimal)ViewData["EntityId"], ObjectType = (string)ViewData["ObjectType"], U = (string)ViewData["U"] });
                           var urlActionParentFriend = Url.Action("CreateResponse", "SalesForce", new { EntityId = (decimal)ViewData["EntityId"], ForumResponseId = 0, ObjectType = (string)ViewData["ObjectType"], U = (string)ViewData["U"] });
                           var urlActionParentFull = "CommentFormDlg('" + urlActionParent + "','Comment Form','"+urlActionParentFriend+"');";
                           urlActionParentFull=urlActionParentFull.Replace("'", "|");
                           var stop = string.Empty;
                         %>
                         <a  href="javascript:void(0)" onclick="javascript:MessageConfirm('<%=urlAction %>','<%=urlActionParentFull %>');">Remove</a>
                      <%} %>                
                    <%string objectType = (string)ViewData["ObjectType"]; %>
                    <a href="javascript:void(0)" onclick="$('#FormComments').dialog('destroy');openPopupCenter('<%= Url.Action("CreateResponse", "SalesForce",new {EntityId=entityid,ForumResponseId=forumresponse.Id,ObjectType=objectType,U=(string)ViewData["U"]})%>','');return false;">
                        Reply </a>
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>
</div>




