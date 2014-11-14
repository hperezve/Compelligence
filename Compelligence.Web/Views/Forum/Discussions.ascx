<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Forum>" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function() {
        $("#tableComment").treeTable({ initialState: "expanded", clickableNodeNames: true });
    });

    function removeComment(forumresponseid, objecttype,entityid) {
        var urlAction = '<%=Url.Action("RemoveDiscussions","Forum") %>';
        $.get(urlAction + '?ObjectType=' + objecttype + '&EntityId=' + entityid + '&forumresponseid=' + forumresponseid,
         {}, function(data) {
           $("#FormDiscussions").dialog("destroy");// location.reload(true);
         });
    }
    
</script>


<%var entityid = ViewData["EntityId"]; %>

<div style="margin-left:20px; width:95.5%;">
    <table id="tableComment" class="simpleForumGrid" >
        <thead>
            <tr>
                <th>Discussions</th>
                <th>Create By</th>
                <th>Create Date</th>
                <th>Reply </th>
            </tr>
        </thead>
        <tbody>
            <%IList<ForumResponse> forumresponses = (IList<ForumResponse>)ViewData["Comments"];
              int count = forumresponses.Count;
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
                                <% var urlToDownload = Url.Action("DownloadExecute", "Library", new { id = library.Id });
                                   string cc = StringUtility.CheckNull((string) ViewData["C"]);
                                   if (!string.IsNullOrEmpty(cc))
                                   {
                                       urlToDownload = Url.Action("DownloadExecute", "SalesForce", new { id = library.Id });
                                   }
                                    %>
                                <a href="javascript:void(0)" onclick="return downloadFile('<%= urlToDownload %>');"><%=library.FileName%></a>
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
                              %>
                            <% var urlAction = Url.Action("RemoveDiscussions", "Forum", new { ObjectType = (string)ViewData["ObjectType"], EntityId = (decimal)ViewData["EntityId"], forumresponseid = forumresponse.Id, U = ViewData["U"], C = ViewData["C"] });
                               var urlActionParent = Url.Action("GetDiscussions", "Forum", new { EntityId = (decimal)ViewData["EntityId"], ObjectType = (string)ViewData["ObjectType"], U = ViewData["U"], C = ViewData["C"] });
                               //var urlActionParentFriend = Url.Action("CommentsResponse", "Forum", new { EntityId = (decimal)ViewData["EntityId"], ForumResponseId = 0, ObjectType = (string)ViewData["ObjectType"], IndustryId = (decimal)ViewData["IndustryId"], CriteriaId = (decimal)ViewData["CriteriaId"], EntityIdT = (decimal)ViewData["EntityIdT"] });
                               var urlActionParentFriend = Url.Action("DiscussionsResponse", "Forum", new { EntityId = (decimal)ViewData["EntityId"], ForumResponseId = 0, ObjectType = (string)ViewData["ObjectType"], U = ViewData["U"], C = ViewData["C"] });
                               var urlActionParentFull = "DiscussionFormDlg('" + urlActionParent + "','Discussion Form','" + urlActionParentFriend + "');";
                               var entityId = ViewData["EntityId"];
                                urlActionParentFull=urlActionParentFull.Replace("'", "|");
                               var stop = string.Empty;
                               string removeLink = "javascript:MessageConfirm('"+urlAction+"','"+urlActionParentFull+"');";
                               if (count == 1) { removeLink = "javascript:MessageConfirmAndRefreshIcon('" + urlAction + "','" + urlActionParentFull + "','"+entityid+"');"; }
                             %>                             
                            <a  href="javascript:void(0)" onclick="javascript:MessageConfirm('<%=urlAction %>','<%=urlActionParentFull %>','<%= entityid%>','<%= ViewData["ObjectType"]%>','<%= Url.Action("GetDiscussionsByProduct", "Forum")%>','#DiscussionsResponse');">Remove</a>
                      <%} %>                
                    <%string objectType = (string)ViewData["ObjectType"]; %>
                    <a href="javascript:void(0)" onclick="$('#DiscussionsResponse').dialog('destroy');DiscussionsResponseNewDlg('<%= Url.Action("DiscussionsResponse", "Forum",new {EntityId=entityid,ForumResponseId=forumresponse.Id,ObjectType=objectType,U=ViewData["U"],C=ViewData["C"]})%>','');return false;">
                        Reply </a>
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>
</div>




