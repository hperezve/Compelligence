<%@ Page Title="Compelligence - Approval Summary" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Project>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
       
        <label for="<%= formId %>ProjectApproveLisTy">
        <asp:Literal ID="ProjectApproveLisTy" runat="server" Text="<%$ Resources:LabelResource, ProjectApproveLisTy %>" /></label>        
        <%= Model.Name %>
        <label for="<%= formId %>ProjectApproveLisTwas">
        <asp:Literal ID="ProjectApproveLisTwas" runat="server" Text="<%$ Resources:LabelResource, ProjectApproveLisTwas %>" /></label>        
        </h2>
    <fieldset>
        <legend>
        <label for="<%= formId %>ProjectApproveLis">
                    <asp:Literal ID="ProjectApproveLis" runat="server" Text="<%$ Resources:LabelResource, ProjectApproveLis %>" /></label>
        
        
            <%= Model.Name %>
        </legend>
        <table style="width: 100%;">
            <tr>
                <th>
                    <label>
                    <asp:Literal ID="ProjectArchiveApprove" runat="server" Text="<%$ Resources:LabelResource, ProjectArchiveApprove %>" /></label>
                </th>
                <th>
                    <label>
                    <asp:Literal ID="ProjectArchiveApproveStatus" runat="server" Text="<%$ Resources:LabelResource, ProjectArchiveApproveStatus %>" /></label>
                </th>
                <th>                      
                    <label>
                    <asp:Literal ID="ProjectArchiveDateAssigned" runat="server" Text="<%$ Resources:LabelResource, ProjectArchiveDateAssigned %>" /></label>
                </th>
                <th>
                    <label>
                    <asp:Literal ID="ProjectArchiveDateAp" runat="server" Text="<%$ Resources:LabelResource, ProjectArchiveDateAp %>" /></label>
                 
                </th>
                <th>
                    <label>
                    <asp:Literal ID="ProjectArchiveDateApNotes" runat="server" Text="<%$ Resources:LabelResource, ProjectArchiveDateApNotes %>" /></label>
                    
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
        <legend>      
         <label for="<%= formId %>Projectspendingap">
                <asp:Literal ID="Projectspendingap" runat="server" Text="<%$ Resources:LabelResource, Projectspendingap %>" /></label>
         </legend>
        <table style="width: 100%;">
            <tr>
                <th>
                 <label>
                <asp:Literal ID="ProjectApproveListProject" runat="server" Text="<%$ Resources:LabelResource, ProjectApproveListProject %>" /></label>
                    
                </th>
                <th>
                 <label>
                <asp:Literal ID="ProjectApproveLisDa" runat="server" Text="<%$ Resources:LabelResource, ProjectApproveLisDa %>" /></label>
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
