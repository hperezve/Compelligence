<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.CompanySalesforceDTO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Edit Company Salesforce.com</title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="text-align: right;">
        <a href="<%=Url.Action("EditCompany", "MaintenanceAccount") + "/" + Model.CompanyId %>">
            Return to Company </a>
    </div>
    <%= Html.ValidationSummary()%>
    <% using (Html.BeginForm("EditCompanySalesforce", "MaintenanceAccount", FormMethod.Post, new { id = "CompanySalesforceEditForm" }))
       { %>
    <%= Html.Hidden("CompanyId")%>
    <div>
        <label>
            <%= HttpUtility.HtmlEncode(LabelResource.FormRequiredFieldsMessage)%></label>
    </div>
    <div class="marginTop10">
        <fieldset>
            <h2>
                Salesforce.com for Company
                <%= HttpUtility.HtmlEncode(Model.CompanyName) %></h2>
            <div class="line">
                <div class="field">
                    <label for="SalesForceToken" class="required">
                        <asp:Literal ID="CompanySalesForceToken" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceToken%>" />:</label>
                    <%= Html.TextBox("SalesForceToken")%>
                    <%= Html.ValidationMessage("SalesForceToken", "*")%>
                </div>
                <div class="field">
                    <label for="SalesForceUser" class="required">
                        <asp:Literal ID="CompanySalesForceUser" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceUser%>" />:</label>
                    <%= Html.TextBox("SalesForceUser")%>
                    <%= Html.ValidationMessage("SalesForceUser", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="SalesForcePassword" class="required">
                        <asp:Literal ID="CompanySalesForcePassword" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForcePassword %>" />:</label>
                    <%= Html.Password("SalesForcePassword")%>
                    <%= Html.ValidationMessage("SalesForcePassword", "*")%>
                </div>
                <div class="field">
                    <label for="SalesForceRePassword" class="required">
                        <asp:Literal ID="CompanySalesForceRePassword" runat="server" Text="<%$ Resources:LabelResource, CompanySalesForceRePassword %>" />:</label>
                    <%= Html.Password("SalesForceRePassword")%>
                    <%= Html.ValidationMessage("SalesForceRePassword", "*")%>
                </div>
            </div>
        </fieldset>
    </div>
    <div class="float-left marginLR5px">
        <input class="button" type="submit" value="Save" /></div>
    <div class="float-left">
        <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#CompanySalesforceEditForm');" /></div>
    <% } %>
</asp:Content>
