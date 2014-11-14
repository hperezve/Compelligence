<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Forum>" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function() {
        $("#tableComment").treeTable({ initialState: "expanded", clickableNodeNames: true });
    });

    function removeComment(forumresponseid, objecttype, entityid) {
        var urlAction = '<%=Url.Action("RemoveComments","Forum") %>';
        $.get(urlAction + '?ObjectType=' + objecttype + '&EntityId=' + entityid + '&forumresponseid=' + forumresponseid,
         {}, function(data) {
             $("#FormComments").dialog("destroy"); // location.reload(true);
         });
    }
    
</script>
<style type="text/css">
    a:hover {
        text-decoration: none;
        color: #CCCCCC;
    }
</style>

<% string industryId=string.Empty;
   if (ViewData["IndustryId"] != null) industryId = ViewData["IndustryId"].ToString();
   string productId = string.Empty;
   if (ViewData["ProductId"] != null) productId = ViewData["ProductId"].ToString();%>
<%var entityid = ViewData["EntityId"]; %>

<div style="margin-left:0px; width:100%;">
    <table id="tableComment" class="simpleForumGrid" >
        <thead>
            <tr style="border:1px solid #AAAAAA;">
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
            <tr id="n<%=forumresponse.Id %>" class="<%=classparent %>" style="border:1px solid #AAAAAA;">
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
                   <%string objectType = (string)ViewData["ObjectType"]; %>
                   <% if ( forumresponse.HasAccess )
                      {
                              %>
                            <% var urlAction = Url.Action("RemoveComments", "Forum", new { ObjectType = (string)ViewData["ObjectType"], EntityId = (decimal)ViewData["EntityId"], forumresponseid = forumresponse.Id, U = ViewData["U"], C = ViewData["C"] });
                               var urlActionParent = Url.Action("GetComments", "Forum", new { EntityId = (decimal)ViewData["EntityId"], ObjectType = (string)ViewData["ObjectType"], U = ViewData["U"], C = ViewData["C"] });
                               var urlActionParentFriend = Url.Action("CommentsResponse", "Forum", new { EntityId = (decimal)ViewData["EntityId"], ForumResponseId = 0, ObjectType = (string)ViewData["ObjectType"], U = ViewData["U"], C = ViewData["C"] });
                               var urlActionParentFull = "CommentFormDlg('" + urlActionParent + "','Comment Form','"+urlActionParentFriend+"');";
                               if (objectType.Equals("SILVER")) { 
                                   urlActionParent = Url.Action("GetSilverBulletsPublicComments", "Forum", new { EntityId = (decimal)ViewData["EntityId"], ObjectType = (string)ViewData["ObjectType"], IndustryId = industryId, ProductId = productId, U = ViewData["U"], C = ViewData["C"] });
                                   urlActionParentFriend = Url.Action("CommentsResponse", "Forum", new { EntityId = (decimal)ViewData["EntityId"], ForumResponseId = 0, ObjectType = (string)ViewData["ObjectType"], IndustryId = industryId, EntityIdT = productId, U = ViewData["U"], C = ViewData["C"] });
                                   urlActionParentFull = "CommentFormDlg('" + urlActionParent + "','Comment Form','" + urlActionParentFriend + "');";
                               }
                               urlActionParentFull=urlActionParentFull.Replace("'", "|");
                               var stop = string.Empty;                               
                             %>
                             <% if (objectType.Equals("SILVER"))
                                { %>
                             <a  href="javascript:void(0)" onclick="javascript:MessageConfirmRemoveSilverComment('<%=urlAction %>','<%=urlActionParentFull %>','<%= entityid%>','<%= ViewData["ObjectType"]%>','<%= Url.Action("GetSilverCommentByProduct", "Forum")%>','#ExternalResponse','<%= industryId%>','<%= productId%>');">Remove</a>
                             <% }
                                else
                                { %>
                             <a  href="javascript:void(0)" onclick="javascript:MessageConfirm('<%=urlAction %>','<%=urlActionParentFull %>','<%= entityid%>','<%= ViewData["ObjectType"]%>','<%= Url.Action("GetCommentByProduct", "Forum")%>','#ExternalResponse');">Remove</a> 
                              <%} %>
                      <%} %>                
                    
                    <a href="javascript:void(0)" onclick="$('#ExternalResponse').dialog('destroy');ExternalResponseNewDlg('<%= Url.Action("ExternalResponse", "Forum",new {EntityId=entityid,ForumResponseId=forumresponse.Id,ObjectType=objectType,IndustryId=industryId,ProductId=productId,U=ViewData["U"],C=ViewData["C"]})%>','');return false;">
                        Reply </a>
                </td>
            </tr>
            <%} %>
        </tbody>
    </table>
</div>
