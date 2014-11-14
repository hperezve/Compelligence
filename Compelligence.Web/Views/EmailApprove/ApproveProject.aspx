<%@ Page Title="Compelligence - Aprove Project" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/ApprovalSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.Project>" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>    

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="indexScripts" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-idleTimeout.js") %>" type="text/javascript""></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var setApprovalStatus = function(status) {
            document.getElementById('ApprovalStatus').value = status;
        };
    </script>
    <div align="right">
        <%=Html.ActionLink("Go to BackEnd","Index","BackEnd") %>
    </div>
    <%= Html.ValidationSummary()%>
    <% using (Html.BeginForm("ApproveProject", "EmailApprove", FormMethod.Post, new { id = "ApproveForm" }))
       { %>
    <fieldset>
        <legend>Approve Project
            <%= Model.Name %>
        </legend>
        <div class="buttonLink">
            <input class="button" type="submit" value="Approve" onclick="javascript: setApprovalStatus('<%= ApprovalListApproveStatus.Approved %>')" />
            <input class="button" type="submit" value="Disapprove" onclick="javascript: setApprovalStatus('<%= ApprovalListApproveStatus.Disapproved %>')" />
        </div>
        <%= Html.Hidden("Id")%>
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
