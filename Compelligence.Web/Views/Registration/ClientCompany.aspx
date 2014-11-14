<%@ Page Title="Compelligence - Register New Account" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.AccountRegistrationDTO>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        var trim = function(textField) {
            var resultText = '';
            for (var i = 0; i < textField.value.length; i++) {
                if (textField.value.charAt(i) != ' ')
                    resultText += textField.value.charAt(i);
            }
            textField.value = resultText;
        };
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <div class="titleTextOne">
        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyHeader %>" />
    </div>
    <div class="titleTextTwo">
        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:LabelResource, ClientCompanyHeaderSub %>" />
    </div>
    <div id="contentRegister">
        <%= Html.ValidationSummary()%>
        <% using (Html.BeginForm("RegisterClientCompany", "Registration", FormMethod.Post, new { id = "ClientCompanyUserEditForm", align = "left" }))
           { %>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    Register
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, Company %>" /></h2>
                <div class="line">
                    <div class="field">
                        <label for="ClientCompanyName" class="required">
                            <asp:Literal ID="ClientCompanyName" runat="server" Text="<%$ Resources:LabelResource, CompanyName %>" />:</label>
                        <%= Html.TextBox("ClientCompanyName")%>
                        <%= Html.ValidationMessage("ClientCompanyName", "*")%>
                    </div>
                    <div class="field">
                        <label for="IndustryName" class="required">
                            <asp:Literal ID="IndustryName" runat="server" Text="<%$ Resources:LabelResource, CompanyIndustryName %>" />:</label>
                        <%= Html.TextBox("IndustryName")%>
                        <%= Html.ValidationMessage("IndustryName", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="ClientCompanyDns" class="required" title="Compelligence subdomain of your Company">
                            <asp:Literal ID="ClientCompanyDns" runat="server" Text="<%$ Resources:LabelResource, CompanyDns %>" />:</label>http://
                        <%= Html.TextBox("ClientCompanyDns", null, new { @style = "width: 100px;text-transform: lowercase;", @onchange = "javascript: trim(this);" })%><asp:Literal
                            ID="Literal3" runat="server" Text="<%$ Resources:LabelResource, DomainUrl %>" />
                        <%= Html.ValidationMessage("ClientCompanyDns", "*")%>
                    </div>
                    <div class="field">
                        <label for="ClientCompanyDescription" class="required">
                            <asp:Literal ID="ClientCompanyDescription" runat="server" Text="<%$ Resources:LabelResource, CompanyDescription %>" />:</label>
                        <%= Html.TextArea("ClientCompanyDescription")%>
                        <%= Html.ValidationMessage("ClientCompanyDescription", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="ClientCompanyAddress" class="required">
                            <asp:Literal ID="ClientCompanyAddress" runat="server" Text="<%$ Resources:LabelResource, CompanyAddress %>" />:</label>
                        <%= Html.TextBox("ClientCompanyAddress")%>
                        <%= Html.ValidationMessage("ClientCompanyAddress", "*")%>
                    </div>
                    <div class="field">
                        <label for="ClientCompanyCountryCode" class="required">
                            <asp:Literal ID="ClientCompanyCountryCode" runat="server" Text="<%$ Resources:LabelResource, CompanyCountryId %>" />:</label>
                        <%= Html.DropDownList("ClientCompanyCountryCode", (SelectList)ViewData["CountryCodeList"], string.Empty)%>
                        <%= Html.ValidationMessage("ClientCompanyCountryCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="ClientCompanyCity" class="required">
                            <asp:Literal ID="ClientCompanyCity" runat="server" Text="<%$ Resources:LabelResource, CompanyCity %>" />:</label>
                        <%= Html.TextBox("ClientCompanyCity")%>
                        <%= Html.ValidationMessage("ClientCompanyCity", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="ClientCompanyState" class="required">
                            <asp:Literal ID="ClientCompanyState" runat="server" Text="<%$ Resources:LabelResource, CompanyState %>" />:</label>
                        <%= Html.TextBox("ClientCompanyState")%>
                        <%= Html.ValidationMessage("ClientCompanyState", "*")%>
                    </div>
                    <div class="field">
                        <label for="ClientCompanyZipCode" class="required">
                            <asp:Literal ID="ClientCompanyZipCode" runat="server" Text="<%$ Resources:LabelResource, CompanyZipCode %>" />:</label>
                        <%= Html.TextBox("ClientCompanyZipCode")%>
                        <%= Html.ValidationMessage("ClientCompanyZipCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="ClientCompanyPhone" class="required">
                            <asp:Literal ID="ClientCompanyPhone" runat="server" Text="<%$ Resources:LabelResource, CompanyPhone %>" />:</label>
                        <%= Html.TextBox("ClientCompanyPhone")%>
                        <%= Html.ValidationMessage("ClientCompanyPhone", "*")%>
                    </div>
                </div>
                <!--
                <div class="line">
                    <div class="field">
                        <label for="ClientCompanySalesForceToken">
                            <asp:Literal ID="ClientCompanySalesForceToken" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceToken%>" />:</label>
                        <%= Html.TextBox("ClientCompanySalesForceToken")%>
                        <%= Html.ValidationMessage("ClientCompanySalesForceToken", "*")%>
                    </div>
                    <div class="field">
                        <label for="ClientCompanySalesForceUser">
                            <asp:Literal ID="ClientCompanyClientCompanySalesForceUser" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceUser%>" />:</label>
                        <%= Html.TextBox("ClientCompanySalesForceUser")%>
                        <%= Html.ValidationMessage("ClientCompanySalesForceUser", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="ClientCompanySalesForcePassword">
                            <asp:Literal ID="ClientCompanySalesForcePassword" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForcePassword %>" />:</label>
                        <%= Html.Password("ClientCompanySalesForcePassword")%>
                        <%= Html.ValidationMessage("ClientCompanySalesForcePassword", "*")%>
                    </div>
                    <div class="field">
                        <label for="ClientCompanySalesForceRePassword">
                            <asp:Literal ID="ClientCompanySalesForceRePassword" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceRePassword %>" />:</label>
                        <%= Html.Password("ClientCompanySalesForceRePassword")%>
                        <%= Html.ValidationMessage("ClientCompanySalesForceRePassword", "*")%>
                    </div>
                </div>
                 -->
            </fieldset>
        </div>
        <div class="marginTop10">
            <fieldset>
                <h2>
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:LabelResource, UserRootRegistration %>" /></h2>
                <div class="line">
                    <div class="field">
                        <label for="UserProfileFirstName" class="required">
                            <asp:Literal ID="UserProfileFirstName" runat="server" Text="<%$ Resources:LabelResource, UserFirstName %>" />:</label>
                        <%= Html.TextBox("UserProfileFirstName")%>
                        <%= Html.ValidationMessage("UserProfileFirstName", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserProfileLastName" class="required">
                            <asp:Literal ID="UserProfileLastName" runat="server" Text="<%$ Resources:LabelResource, UserLastName %>" />:</label>
                        <%= Html.TextBox("UserProfileLastName")%>
                        <%= Html.ValidationMessage("UserProfileLastName", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserProfileEmail" class="required">
                            <asp:Literal ID="UserProfileEmail" runat="server" Text="<%$ Resources:LabelResource, UserEmail %>" />
                            (Enter an existing email address):</label>
                        <%= Html.TextBox("UserProfileEmail", null, new { @style ="text-transform: lowercase;", @onchange = "javascript: trim(this);" })%>
                        <%= Html.ValidationMessage("UserProfileEmail", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="UserProfilePassword" class="required">
                            <asp:Literal ID="UserProfilePassword" runat="server" Text="<%$ Resources:LabelResource, UserPassword %>" />:</label>
                        <%= Html.Password("UserProfilePassword", null, new { autocomplete = "off" })%>
                        <%= Html.ValidationMessage("UserProfilePassword", "*")%>
                    </div>
                    <div class="field">
                        <label for="UserProfileRePassword" class="required">
                            <asp:Literal ID="UserProfileRePassword" runat="server" Text="<%$ Resources:LabelResource, UserRePassword %>" />:</label>
                        <%= Html.Password("UserProfileRePassword", null, new { autocomplete = "off" })%>
                        <%= Html.ValidationMessage("UserProfileRePassword", "*")%>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="line">
            <label>
                <%= HttpUtility.HtmlEncode(LabelResource.FormRequiredFieldsMessage)%></label>
        </div>
        <div class="line">
            &nbsp;
        </div>
        <div class="float-left marginLR5px">
            <input class="button" type="submit" value="Save" /></div>
        <div class="float-left">
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#ClientCompanyUserEditForm');" /></div>
        <% } %>
    </div>
</asp:Content>
