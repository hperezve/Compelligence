<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    function OpeniFramePopup(url) {
        var commentObject = $("#ForumDiscussionDlg");
        commentObject.dialog({ autoOpen: false,
            title: "Discussion Dialog",
            width: 680,
            modal: true,
            buttons: { "Close": function() {
                UpdateCommentDetailList();
                $(this).dialog("destroy");
            }
            }
        });
        commentObject.html("<iframe src='" + url + "' width=656 height=290 frameBorder=0></iframe>");
        commentObject.dialog("open");

    }
    $(function() {
        $('#<%= ViewData["Scope"] %>ForumDiscussionTable').treeTable({ initialState: "expanded", clickableNodeNames: true });
    });

    function removeComment(forumresponseid, objecttype) {
        var idObject = getIdFromDiscussion('<%= ViewData["Scope"] %>', '<%= ViewData["DetailFilter"] %>');
        var urlAction = '<%=Url.Action("RemoveComments","ForumDiscussion") %>';

        $.get(urlAction + '?ObjectType=' + objecttype + '&EntityId=' + idObject + '&forumresponseid=' + forumresponseid,
         {}, function(data) {
         loadDetailList('<%= Url.Action("GetDetails", (string)ViewData["EntityTypeName"]) %>', idObject,
                            '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>DiscussionContent');

         });

     }

     function OpenPopup(urlAction, f1, f2, container, header, filter) {
         var currentPopup = window.open(urlAction, 'Comment', 'height=295,width=720,location=0,status=0,scrollbars=1');
         if (currentPopup.focus) { currentPopup.focus() }
         return false;
     }
     function UpdateCommentDetailList() {
         loadDetailList('<%= Url.Action("GetDetails", (string)ViewData["EntityTypeName"]) %>', getIdFromDiscussion('<%= ViewData["Scope"] %>', '<%= ViewData["DetailFilter"] %>'),
                            '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>DiscussionContent');
     }    
</script>

<div id="ForumDiscussionDlg"></div>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>ForumDiscussionDetailDataListContent" class="absolute">
        <asp:Panel ID="ForumDiscussionDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "ForumDiscussion") + "', '" + ViewData["Scope"] + "', 'ForumDiscussion', 'ForumDiscussionDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: OpeniFramePopup('" + Url.Action("Create", "ForumDiscussion", new { Scope = ViewData["Scope"], HeaderType = ViewData["HeaderType"], DetailFilter = ViewData["DetailFilter"], IsDetail = "true" }) + "', 'ForumComment', 'ForumCommentDetail', '" + ViewData["Container"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="ForumDiscussionDetailDataListContent" runat="server" CssClass="contentFormEdit">
            <table id="<%= ViewData["Scope"] %>ForumDiscussionTable" class="simpleForumGrid">
                 <tbody>
                    <%IList<ForumResponse> forumresponses = (IList<ForumResponse>)ViewData["Discussions"];
                      
                      foreach (ForumResponse forumresponse in forumresponses)
                      {
                          var classparent = string.Empty;
                          if (forumresponse.ParentResponseId != null && forumresponse.ParentResponseId > 0)
                          {
                              classparent = "child-of-n" + forumresponse.ParentResponseId.ToString();
                          } %>
                    <tr id="n<%=forumresponse.Id %>" class="<%=classparent %>">
                        <td class="columnDiscussion" style="background: white; border: 1px solid #7F9DB9;">
                            <img src="<%= Url.Content("~/content/Images/Styles/iconComments.png") %>" />
                            <span style="font-size:14px; font-weight: bold;"><%=forumresponse.CreatedByName%></span> &nbsp;&nbsp;<span style="font-size:10px; text-decoration: underline;font-style: oblique;"><%=forumresponse.CreatedDate%></span>
                            <div style="float:right;"><img src="<%= Url.Content("~/content/Images/Styles/user_icon.jpg") %>" width="55px" /></div>
                            <br /><br />
                            <span class="discussionName">
                                <%=forumresponse.Response%>
                            </span>
                            <% if (forumresponse.Libraries.Count > 0)
                               { %>
                                <br /><br />
                               <span style="margin-left:3.5em">
                               Files:</span><br />
                            <ul>
                                <% foreach (Library library in forumresponse.Libraries)
                                   {
                                       File file = library.File;
                                %>
                                <li>
                                    <span style="margin-left:3.5em">
                                    <%--<% =Html.ActionLink(library.FileName, "DownloadExecute", "Library", new { Id = library.Id }, null)%>--%>
                                    <a href="javascript:void(0)" onclick="return downloadFile('<%=Url.Action("DownloadExecute", "Library") + "/" + library.Id %>');">
                                    <%=library.FileName%></a>                                    
                                    </span>
                                    
                                    
                                </li>
                                <% } %>
                            </ul>
                            <%} %>
                            <br /><br />
                            &nbsp;&nbsp;&nbsp;
                            <% string usersa = (string)ViewData["UserSecurityAccess"];
                               if (usersa.Equals(UserSecurityAccess.Edit))
                               {
                              %>
                              <input class="button" type="button" onclick="javascript:removeComment('<%=forumresponse.Id%>','<%= ViewData["HeaderType"] %>')" value="Remove" style="width:65px;" />
                            <%--<a  href="javascript:void(0)" onclick="javascript:removeComment('<%=forumresponse.Id%>','<%= ViewData["HeaderType"] %>')">Remove</a>--%>
                             <%} %>
                             <%--<input class="button" type="button" onclick="javascript: newEntity('<%= Url.Action("Create", "ForumDiscussion", new { parentId = forumresponse.Id })%>', '<%= ViewData["Scope"] %>', 'ForumDiscussion', 'ForumDiscussionDetail', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" value="Reply" style="width:65px;" />--%>
                             <input class="button" type="button" onclick = "javascript: OpeniFramePopup('<%=Url.Action("Create", "ForumDiscussion", new { parentId = forumresponse.Id,Scope = ViewData["Scope"], HeaderType = ViewData["HeaderType"],DetailFilter=ViewData["DetailFilter"],IsDetail="true" })%>', 'ForumComment', 'ForumCommentDetail', '<%=ViewData["Container"]%>');" value="Reply" style="width:65px;" /> 
                             
                            <br /><br /> 
                        </td> 
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="ForumDiscussionDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ForumDiscussionEditFormContent">
        </div>
    </asp:Panel>
</div>