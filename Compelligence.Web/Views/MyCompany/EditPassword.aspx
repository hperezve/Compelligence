<%@ Page Title="Compelligence - My Company" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.ClientCompany>" %>

<asp:Content ID="indexHead" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="ClientCompanyEditPassword" ContentPlaceHolderID="MainContent" runat="server">
    <div class="EditProfile">
        <fieldset>
            <legend>Edit Salesforce.com Password</legend>
            <%= Html.ValidationSummary()%>
            <% using (Html.BeginForm("EditPassword", "MyCompany", FormMethod.Post, new { id = "ClientCompanyEditPasswordProfileForm", align = "left" }))
               { %>
            <%= Html.Hidden("Id", default(string))%>
            <div class="contentFormEdit">
                <div class="line">
                    <div class="field">
                        <label for="SalesForceToken">
                            <asp:Literal ID="ClientCompanySalesForceToken" runat="server" Text="<%$ Resources:LabelResource, ClientCompanySalesForceToken%>" />:</label>
                        <%= Html.TextBox("SalesForceToken", null, new { @disabled = "true"})%>                                                
                        <%= Html.ValidationMessage("SalesForceToken", "*")%>
                    </div>
                    <div class="field">
                        <label for="SalesForceUser">
                            <asp:Literal ID="ClientCompanySalesForceUser" runat="server" Text="<%$ Resources:LabelResource, ClientCompanySalesForceUser%>" />:</label>
                        <%= Html.TextBox("SalesForceUser", null, new { @disabled = "true" })%>
                        <%= Html.ValidationMessage("SalesForceUser", "*")%>
                    </div>
                </div>
<%--                <% if (!string.IsNullOrEmpty((string)ViewData["CurrentPassword"]))
                   { %>
                <div class="line">
                    <div class="field">
                        <label for="SalesForceCurrentPassword" class="required">
                            <asp:Literal ID="SalesForceCurrentPassword" runat="server" Text="<%$ Resources:LabelResource, ClientCompanySalesForceCurrentPassword %>" />:</label>
                        <%= Html.Password("SalesForceCurrentPassword")%>
                        <%= Html.ValidationMessage("SalesForceCurrentPassword", "*")%>
                    </div>
                </div>
                <% } %>--%>
                <div class="line">
                    <div class="field">
                        <label for="SalesForcePassword" class="required">
                            <asp:Literal ID="SalesForcePassword" runat="server" Text="<%$ Resources:LabelResource, ClientCompanySalesForceNewPassword %>" />:</label>
                        <%= Html.Password("SalesForcePassword", null, new { autocomplete = "off" })%>
                        <%= Html.ValidationMessage("SalesForcePassword", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="SalesForceRePassword" class="required">
                            <asp:Literal ID="SalesForceRePassword" runat="server" Text="<%$ Resources:LabelResource, ClientCompanySalesForceRePassword %>" />:</label>
                        <%= Html.Password("SalesForceRePassword", null, new { autocomplete = "off" })%>
                        <%= Html.ValidationMessage("SalesForceRePassword", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field float-left">
                        <input class="button" type="submit" value="Save" />
                        <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#ClientCompanyEditPasswordProfileForm');" />
                        <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("EditProfile","MyCompany")%>'" />
                    </div>
                </div>
                <% } %>
            </div>
        </fieldset>
    </div>
</asp:Content>
