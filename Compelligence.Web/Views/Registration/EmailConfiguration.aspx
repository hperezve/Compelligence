<%@ Page Title="Compelligence - Email configuration" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.EmailCfgRegistrationDTO>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <a href="<%= ViewData["CompanyUrl"] %>">Go to Login Page</a>
    </div>
    <br />
    <br />
    <div class="titleTextOne">
        Welcome to the Email registration Page of Compelligence System</div>
    <div class="titleTextTwo">
        your Competitive Intelligence System!!</div>
    <div id="contentRegister">
        <%= Html.ValidationSummary()%>
        <% using (Html.BeginForm("RegisterEmailConfiguration", "Registration", FormMethod.Post, new { id = "EmailConfigurationEditForm", align = "left" }))
           { %>
        <%= Html.Hidden("ClientCompany")%>
        <div class="marginTop10">
            <fieldset>
                <legend>Register Smtp Email Configuration</legend>
                <div class="line">
                    <div class="field">
                        <label for="EmailUser" class="required">
                            Email User:</label>
                        <%= Html.TextBox("EmailUser", null, new { @readonly = true})%>
                        <%= Html.ValidationMessage("EmailUser", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="EmailPassword" class="required">
                            Email Password:</label>
                        <%= Html.Password("EmailPassword")%>
                        <%= Html.ValidationMessage("EmailPassword", "*")%>
                    </div>
                    <div class="field">
                        <label for="ReEmailPassword" class="required">
                            Re-type Email Password:</label>
                        <%= Html.Password("ReEmailPassword")%>
                        <%= Html.ValidationMessage("ReEmailPassword", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="SmtpServer" class="required">
                            Server:</label>
                        <%= Html.TextBox("SmtpServer")%>
                        <%= Html.ValidationMessage("SmtpServer", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="SmtpPort" class="required">
                            Port:</label>
                        <%= Html.TextBox("SmtpPort")%>
                        <%= Html.ValidationMessage("SmtpPort", "*")%>
                    </div>
                    <div class="field">
                        <label for="SmtpRequireSsl">
                            Require Ssl:</label>
                        <%= Html.CheckBox("SmtpRequireSsl")%>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>Pop Configuration</legend>
                <div class="line">
                    <div class="field">
                        <label for="PopServer" class="required">
                            Server:</label>
                        <%= Html.TextBox("PopServer")%>
                        <%= Html.ValidationMessage("PopServer", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="PopPort" class="required">
                            Port:</label>
                        <%= Html.TextBox("PopPort")%>
                        <%= Html.ValidationMessage("PopPort", "*")%>
                    </div>
                    <div class="field">
                        <label for="PopRequireSsl">
                            Require Ssl:</label>
                        <%= Html.CheckBox("PopRequireSsl")%>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="float-left marginLR5px">
            <input class="button" type="submit" value="Save" /></div>
        <div class="float-left">
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#EmailConfigurationEditForm');" /></div>
        <% } %>
    </div>
</asp:Content>
