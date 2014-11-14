<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ViewFormResetPassword</title>
</head>
<body>
<% string formId = "ClientCompanyForm"; %>
    <%= Html.ValidationSummary()%>
        <% using (Html.BeginForm("ResetUserPassword", "MaintenanceAccount", FormMethod.Post, new { id = formId, align = "left" }))
        { %>    
       <div class="field">
                        <label for="UserId">
                            <asp:Literal ID="UserProfileId" runat="server" Text="<%$ Resources:LabelResource, UserProfileUserName %>" />:</label>
                        <%= Html.DropDownList("UserId", (SelectList)ViewData["AssignedToList"], "Select user...", new { id = formId + "UserId" })%>
                        <%= Html.ValidationMessage("UserProfileId", "*")%>
                        <%= Html.Hidden("HdnSelected") %>
                    </div>    
        <% } %>
</body>
</html>
