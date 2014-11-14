<%@ Page Title="Compelligence - Approve New User Registration" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/ApprovalSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.UserProfile>" %>
 <asp:Content ID="indexScripts" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-idleTimeout.js") %>" type="text/javascript""></script>
 </asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
.line {
 margin-left:0;
}
.field {
    float: left;
    margin-left: 2px;
    padding: 7px 25px 5px 0;
    width: 255px;
}
</style>
    <script type="text/javascript">
        var setApprovalStatus = function(status) {
            document.getElementById('ApprovalStatus').value = status;
        };
    </script>
    <div align="right">
        <%=Html.ActionLink("Go to BackEnd","Index","BackEnd") %>
    </div>
    <%= Html.ValidationSummary()%>
    <% using (Html.BeginForm("ApproveNewUser", "EmailApproveRegistration", FormMethod.Post, new { id = "ApproveNewUser" }))
       { %>
    <fieldset>
        <h2>Approve User registration for "<%= Model.Name %>" </h2>
        <div class="buttonLink">
            <input class="button" type="submit" value="Approve" onclick="javascript: setApprovalStatus('<%= ApprovalListApproveStatus.Approved %>')" />
            <input class="button" type="submit" value="Disapprove" onclick="javascript: setApprovalStatus('<%= ApprovalListApproveStatus.Disapproved %>')" />
        </div>
        <%= Html.Hidden("Id")%>
        <%= Html.Hidden("ApproverId")%>
        <%= Html.Hidden("ApprovalStatus")%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label>
                        First Name:</label>
                    <%= Html.Encode(Model.FirstName)%>
                </div>
                <div class="field">
                    <label>
                        Last Name:</label>
                    <%= Html.Encode(Model.LastName)%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label>
                        Email:</label>
                    <%= Html.Encode(Model.Email)%>
                </div>
                <div class="field">
                    <label>
                        Country:</label>
                    <%= Html.Encode(Model.CountryName)%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label>
                        City:</label>
                    <%= Html.Encode(Model.City)%>
                </div>
                <div class="field">
                    <label>
                        State:</label>
                    <%= Html.Encode(Model.Department)%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label>
                        Phone:</label>
                    <%= Html.Encode(Model.Phone)%>
                </div>
                <div class="field">
                    <label>
                        Fax:</label>
                    <%= Html.Encode(Model.Fax)%>
                </div>
            </div>
            <div class="line">
            <div class="field">
               <label for="SecurityGroupId" class="required">
                            <asp:Literal ID="UserSecurityGroupId" runat="server" Text="User Type" />:</label>
                        <%= Html.DropDownList("SecurityGroupId", (SelectList)ViewData["SecurityGroupList"], string.Empty, new { id = "SecurityGroupId" })%>
                        <%= Html.ValidationMessage("SecurityGroupId", "*")%>
            </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="ApprovalNotes" class="required">
                        <asp:Literal ID="NewUserApprovalNotes" runat="server" Text="Approval Notes" />:</label>
                    <%= Html.TextArea("ApprovalNotes")%>
                    <%= Html.ValidationMessage("ApprovalNotes", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
    <% } %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
