<%@ Page Title="Compelligence - Aprove Project" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Project>" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>    

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var setApprovalStatus = function(status) {
            document.getElementById('ApprovalStatus').value = status;
        };
    </script>
    <%= Html.ValidationSummary()%>
    <% using (Html.BeginForm("Approve", "Project", FormMethod.Post, new { id = "ApproveForm" }))
       { %>
    <fieldset>
        <legend>Approve Project
            <%= Model.Name %>
        </legend>
        <div class="buttonLink">
            <input class="button" type="submit" value="Approve" onclick="javascript: setApprovalStatus('<%= ApprovalListApproveStatus.Approved %>')" />
            <input class="button" type="submit" value="Disapprove" onclick="javascript: setApprovalStatus('<%= ApprovalListApproveStatus.Disapproved %>')" />
        </div>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("ApproverId")%>
        <%= Html.Hidden("ApprovalStatus")%>
        <div class="contentFormEdit">
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
                        <%= HtmlContentHelper.GetApprovalStatus(approvalMember.ApprovalLog.Approved)%>
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
            <div class="line">
                <div class="field">
                    <label for="ApprovalNotes">
                        <asp:Literal ID="ProjectApprovalNotes" runat="server" Text="<%$ Resources:LabelResource, ProjectApprovalNotes %>" />:</label>
                    <%= Html.TextArea("ApprovalNotes")%>
                    <%= Html.ValidationMessage("ApprovalNotes", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
    <% } %>
</asp:Content>
