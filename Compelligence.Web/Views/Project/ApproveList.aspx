<%@ Page Title="Compelligence - Approval Summary" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Project>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Thanks, Project
        <%= Model.Name %>
        was processed</h2>
    <fieldset>
        <legend>Approve List for Project
            <%= Model.Name %>
        </legend>
        <table style="width: 100%;">
            <tr>
                <th>
                    Approver
                </th>
                <th>
                    Approve status
                </th>
                <th>
                    Date assigned
                </th>
                <th>
                    Date approved
                </th>
                <th>
                    Approver notes
                </th>
            </tr>
            <% IList<ApprovalMemberDTO> approvalList = (IList<ApprovalMemberDTO>)ViewData["ApprovalList"];

               foreach (ApprovalMemberDTO approvalMember in approvalList)
               { %>
            <tr>
                <td>
                    <%= approvalMember.ApprovalMember.Name %>
                </td>
                <td>
                    <%= HtmlContentHelper.GetApprovalStatus(approvalMember.ApprovalLog.Approved) %>
                </td>
                <td>
                    <%= approvalMember.ApprovalLog.DateAssigned %>
                </td>
                <td>
                    <%= approvalMember.ApprovalLog.DateApproved %>
                </td>
                <td>
                    <%= approvalMember.ApprovalLog.ApproverNotes %>
                </td>
            </tr>
            <% } %>
        </table>
    </fieldset>
    <br />
    <% IList<ApprovalProjectDTO> approvalPendingList = (IList<ApprovalProjectDTO>)ViewData["ApprovalPendingList"];
       if ((approvalPendingList != null) && (approvalPendingList.Count > 0))
       {
    %>
    <fieldset>
        <legend>Projects pending approval </legend>
        <table style="width: 100%;">
            <tr>
                <th>
                    Project
                </th>
                <th>
                    Date assigned
                </th>
                <th>
                </th>
            </tr>
            <% 
                foreach (ApprovalProjectDTO approvalProject in approvalPendingList)
                { %>
            <tr>
                <td>
                    <%= approvalProject.Project.Name%>
                </td>
                <td>
                    <%= approvalProject.ApprovalLog.DateAssigned%>
                </td>
                <td>
                    <a href="<%= Compelligence.Emails.Util.EmailUtilities.BuildApprovalLink(Model.ClientCompany, approvalProject.ApprovalLog.UserId, approvalProject.ApprovalLog.EntityId, approvalProject.ApprovalLog.EntityType)%>">
                        Approve</a>
                </td>
            </tr>
            <% } %>
        </table>
    </fieldset>
    <% } %>
</asp:Content>
