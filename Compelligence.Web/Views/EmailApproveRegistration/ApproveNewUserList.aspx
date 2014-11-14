<%@ Page Title="Compelligence - Approval Summary" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/ApprovalSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.UserProfile>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
  <link href="<%= Url.Content("~/Content/Styles/comparinator.css") %>" rel="stylesheet"
        type="text/css" />
  <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="indexScript" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-idleTimeout.js") %>" type="text/javascript""></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div align="right">
        <%=Html.ActionLink("Go to BackEnd","Index","BackEnd") %>
    </div>
    <h2>
        User
        <%= Model.Name %>
        has been  processed</h2>
     <% IList<ApprovalUserRegistrationDTO> approvalPendingList = (IList<ApprovalUserRegistrationDTO>)ViewData["ApprovalNewUserPendingList"];
       if ((approvalPendingList != null) && (approvalPendingList.Count > 0))
       {
    %>
    <fieldset>
        <h2>New registrations pending for approval </h2>
        <table class="comp_table filtered" style="width: 100%;">
        <thead class="comp_head">
            <tr>
                <th>
                    User name
                </th>
                <th>
                    Email
                </th>
                <th>
                    Date assigned
                </th>
                <th>
                </th>
            </tr>
          </thead>
          <tbody class="Dottedcell td">
            <% 
                foreach (ApprovalUserRegistrationDTO approvalNewUser in approvalPendingList)
                { %>
            <tr>
                <td>
                    <%= approvalNewUser.NewUser.Name %>
                </td>
                <td>
                    <%= approvalNewUser.NewUser.Email %>
                </td>
                <td>
                    <%= approvalNewUser.ApprovalLog.DateAssigned%>
                </td>
                <td>
                    <a href="<%= Compelligence.Emails.Util.EmailUtilities.BuildApprovalRegistrationUserLink(Model.ClientCompany, approvalNewUser.ApprovalLog.UserId, approvalNewUser.ApprovalLog.EntityId, approvalNewUser.ApprovalLog.EntityType)%>">
                        Approve</a>
                </td>
            </tr>
            <% } %>
            </tbody>
        </table>
    </fieldset>
    <% } %>
</asp:Content>
