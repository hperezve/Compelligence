<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">

    $(function() {
        $('#<%= ViewData["Scope"] %>ForumFeedbackTable').treeTable({ initialState: "expanded", clickableNodeNames: true });
    });
     
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>ForumFeedbackDetailDataListContent" class="absolute">
        <asp:Panel ID="ForumFeedbackDetailToolbarContent" runat="server" CssClass="buttonLink">
        </asp:Panel>
        <asp:Panel ID="ForumFeedbackDetailDataListContent" runat="server" CssClass="contentFormEdit">
            <table id="<%= ViewData["Scope"] %>ForumFeedbackTable" class="simpleForumGrid">
                <tbody>
                    <%IList<ForumResponse> forumresponses = (IList<ForumResponse>)ViewData["Feedbacks"];
                      
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
                        </td>
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </asp:Panel>
    </div>
    <asp:Panel ID="ForumFeedbackDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>ForumFeedbackEditFormContent">
        </div>
    </asp:Panel>
</div>
