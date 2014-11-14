<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">

    $(function() {
        $('#<%= ViewData["Scope"] %>ForumCommentTable').treeTable({ initialState: "expanded", clickableNodeNames: true });
    });

    function removeComment(forumresponseid, objecttype) {
        var idObject=getIdFrom('<%= ViewData["Scope"] %>');
        var urlAction = '<%=Url.Action("RemoveComments","ForumComment") %>';

        $.get(urlAction + '?ObjectType=' + objecttype + '&EntityId='+idObject +'&forumresponseid=' + forumresponseid ,
         {},   function(data) {
           loadDetailList('<%= Url.Action("GetDetails", (string)ViewData["EntityTypeName"]) %>', getIdFrom('<%= ViewData["Scope"] %>'),'<%= ViewData["Scope"] %>', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>CommentContent');
         });
    }


    function OpenPopup(urlAction,f1,f2,container,header,filter) {
        var currentPopup = window.open(urlAction, 'Comment', 'height=350,width=720,location=0,status=0,scrollbars=1');
        if (currentPopup.focus) { currentPopup.focus() }
        return false;
    }
    function UpdateCommentDetailList() {
        loadDetailList('<%= Url.Action("GetDetails", (string)ViewData["EntityTypeName"]) %>', getIdFrom('<%= ViewData["Scope"] %>'),
                            '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Comment %>', '#<%= ViewData["Scope"] %>CommentContent');
    }
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>ForumCommentDetailDataListContent" class="absolute">
        <asp:Panel ID="ForumCommentDetailToolbarContent" runat="server" CssClass="buttonLink">
           <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "ForumComment") + "', '" + ViewData["Scope"] + "', 'ForumComment', 'ForumCommentDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
           <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: OpenPopup('" + Url.Action("Create", "ForumComment", new { Scope = ViewData["Scope"], HeaderType = ViewData["HeaderType"],DetailFilter=ViewData["DetailFilter"],IsDetail="true" }) + "', 'ForumComment', 'ForumCommentDetail', '" + ViewData["Container"] + "');" })%>
        </asp:Panel>
        <asp:Panel ID="ForumCommentDetailDataListContent" runat="server" CssClass="contentFormEdit">
            <table id="<%= ViewData["Scope"] %>ForumCommentTable" class="simpleForumGrid">

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
                               <br />
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
                            <br />
                            &nbsp;&nbsp;&nbsp;
                            <% string usersa = (string)ViewData["UserSecurityAccess"];
                               if (usersa.Equals(UserSecurityAccess.Edit))
                               {
                              %>
                              <input class="button" type="button" onclick="javascript:removeComment('<%=forumresponse.Id%>','<%= ViewData["HeaderType"] %>')" value="Remove" style="width:65px;" />
                              <%} %>
                             <%--<input class="button" type="button" onclick="javascript: newEntity('<%= Url.Action("Create", "ForumComment", new { parentId = forumresponse.Id })%>', '<%= ViewData["Scope"] %>', 'ForumComment', 'ForumCommentDetail', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" value="Reply" style="width:65px;" />--%>
                             <input class="button" type="button" onclick = "javascript: OpenPopup('<%=Url.Action("Create", "ForumComment", new { parentId = forumresponse.Id,Scope = ViewData["Scope"], HeaderType = ViewData["HeaderType"],DetailFilter=ViewData["DetailFilter"],IsDetail="true" })%>', 'ForumComment', 'ForumCommentDetail', '<%=ViewData["Container"]%>');" value="Reply" style="width:65px;" /> 
                            <br />                          
                        </td>

                    </tr>
                    <%} %>
                </tbody>
            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="ForumCommentDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ForumCommentEditFormContent">
        </div>
    </asp:Panel>
</div>
