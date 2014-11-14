<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSiteUser.Master" Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.UserProfile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>EditPasswordFront</title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
<script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div>
         <div class="headerContentRightMenu">Change My Password
        </div>
        <fieldset class="contentRightMenu">
            <%= Html.ValidationSummary()%>
            <% using (Html.BeginForm("EditPasswordFront", "MyProfile", FormMethod.Post, new { id = "EditCurrentPasswordForm", align = "left" }))
   { %>
            <%= Html.Hidden("Id", default(string))%>
            <div class="line">
                <div class="field">
                    <label for="CurrentPassword" class="required">
                        <asp:Literal ID="UserCurrentPassword" runat="server" Text="<%$ Resources:LabelResource, UserCurrentPassword %>" />:</label>
                    <%= Html.Password("CurrentPassword", null, new { autocomplete = "off" })%>
                    <%= Html.ValidationMessage("CurrentPassword", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="Password" class="required">
                        <asp:Literal ID="UserNewPassword" runat="server" Text="<%$ Resources:LabelResource, UserNewPassword %>" />:</label>
                    <%= Html.Password("Kennwort", null, new { autocomplete = "off" })%>
                    <%= Html.ValidationMessage("Password", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="RePassword" class="required">
                        <asp:Literal ID="UserRePassword" runat="server" Text="<%$ Resources:LabelResource, UserRePassword %>" />:</label>
                    <%= Html.Password("ReKennwort", null, new { autocomplete = "off" })%>
                    <%= Html.ValidationMessage("RePassword", "*")%>
                </div>
            </div>
            <div class="float-left" style="margin-top: 20px; margin-left: 12px; padding-left: 10px;
                padding-bottom: 10px;">
                <input class="button" type="submit" value="Save" />
                <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#EditCurrentPasswordForm');" />
                <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("EditProfileFront","MyProfile")%>'" />
            </div>
            <% } %>
        </fieldset>
    </div>

</asp:Content>

