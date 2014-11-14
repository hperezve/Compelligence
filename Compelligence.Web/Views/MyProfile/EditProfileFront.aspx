<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSiteUser.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>EditProfileFront</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/thickbox.css") %>" rel="stylesheet"
        type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
<script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"
        type="text/javascript"></script>
        
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.bgiframe.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.ajaxQueue.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/thickbox-compressed.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.json.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookiejar.pack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.tableFilter.js") %>" type="text/javascript"></script>

   

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/FeedBack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>">  </script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--<h2>EditProfileFront</h2>--%>
 <% string formId = "UserProfileFrontForm"; %>
  <div>
        <div class="headerContentRightMenu">My Profile
        </div>
        <fieldset class="contentRightMenu">
           <%-- <div style="margin-left:11px;"> <h1>My Profile</h1> </div>--%>
            <%= Html.ValidationSummary() %>
            <% using (Html.BeginForm("EditProfileFront", "MyProfile", FormMethod.Post, new { id = "UserEditProfileForm", align = "left" }))
               { %>
               <%= Html.Hidden("Id", default(string))%>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>FirstName" class="required">
                        <asp:Literal ID="UserFirstName" runat="server" Text="<%$ Resources:LabelResource, UserFirstName %>" />:</label>
                    <%= Html.TextBox("FirstName", null, new { id=formId + "FirstName" })%>
                    <%= Html.ValidationMessage("FirstName", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>LastName" class="required">
                        <asp:Literal ID="UserLastName" runat="server" Text="<%$ Resources:LabelResource, UserLastName %>" />:</label>
                    <%= Html.TextBox("LastName", null, new { id = formId + "LastName" })%>
                    <%= Html.ValidationMessage("LastName", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Email" class="required">
                        <asp:Literal ID="UserEmail" runat="server" Text="<%$ Resources:LabelResource, UserEmail %>" />:</label>
                    <%= Html.TextBox("Email", null, new { id = formId + "Email" })%>
                    <%= Html.ValidationMessage("Email", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Title">
                        <asp:Literal ID="UserTitle" runat="server" Text="<%$ Resources:LabelResource, UserTitle %>" />:</label>
                    <%= Html.TextBox("Title", null, new { id = formId + "Title" })%>
                    <%= Html.ValidationMessage("Title", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CountryCode" class="required">
                        <asp:Literal ID="UserCountryCode" runat="server" Text="<%$ Resources:LabelResource, UserCountryId %>" />:</label>
                    <%= Html.DropDownList("CountryCode", (SelectList)ViewData["CountryCodeList"], string.Empty, new { id = formId + "CountryCode" })%>
                    <%= Html.ValidationMessage("CountryCode", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Address" class="required">
                        <asp:Literal ID="UserAddress" runat="server" Text="<%$ Resources:LabelResource, UserAddress %>" />:</label>
                    <%= Html.TextBox("Address", null, new { id = formId + "Address" })%>
                    <%= Html.ValidationMessage("Address", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>City" class="required">
                        <asp:Literal ID="UserCity" runat="server" Text="<%$ Resources:LabelResource, UserCity %>" />:</label>
                    <%= Html.TextBox("City", null, new { id = formId + "City" })%>
                    <%= Html.ValidationMessage("City", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Department" class="required">
                        <asp:Literal ID="UserDepartment" runat="server" Text="<%$ Resources:LabelResource, UserDepartment %>" />:</label>
                    <%= Html.TextBox("Department", null, new { id = formId + "Department" })%>
                    <%= Html.ValidationMessage("Department", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>ZipCode" class="required">
                        <asp:Literal ID="UserZipCode" runat="server" Text="<%$ Resources:LabelResource, UserZipCode %>" />:</label>
                    <%= Html.TextBox("ZipCode", null, new { id = formId + "ZipCode" })%>
                    <%= Html.ValidationMessage("ZipCode", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Phone" class="required">
                        <asp:Literal ID="UserPhone" runat="server" Text="<%$ Resources:LabelResource, UserPhone %>" />:</label>
                    <%= Html.TextBox("Phone", null, new { id = formId + "Phone" })%>
                    <%= Html.ValidationMessage("Phone", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Fax">
                        <asp:Literal ID="UserFax" runat="server" Text="<%$ Resources:LabelResource, UserFax %>" />:</label>
                    <%= Html.TextBox("Fax", null, new { id = formId + "Fax" })%>
                    <%= Html.ValidationMessage("Fax", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>SessionTimeout" class="required">
                        <asp:Literal ID="SessionTimeout" runat="server" Text="<%$ Resources:LabelResource, UserSessionTimeout %>" />:</label>
                    <%= Html.TextBox("SessionTimeout", null, new { id = formId + "SessionTimeout", style = "width: 50px;" })%>
                    <%= Html.DropDownList("TimeoutUnit", (SelectList)ViewData["TimeoutUnitList"], new { id = formId + "TimeoutUnit", style = "width: 80px;" })%>
                    <%= Html.ValidationMessage("SessionTimeoutErr", "*")%>
                </div>
            </div>
            <div class="line">
               <%-- <div class="field">
                    <%= Html.ActionLink("Edit email configuration", "EditPopEmailCfg", "MyProfile")%>
                </div>--%>
                <%--<div class="field">
                    <%= Html.ActionLink("Edit Outgoing Email Server", "EditEmailCfg", "MyProfile")%>
                </div>--%>
                <div class="field">
                    <%= Html.ActionLink("Edit password", "EditPasswordFront", "MyProfile")%>
                </div>
            </div>
            <div class="line" style="margin-left:11px;">
                <label>
                    <%= HttpUtility.HtmlEncode(LabelResource.FormRequiredFieldsMessage)%></label>
            </div>
            <div class="float-left" style="margin-top: 20px; margin-left: 12px; padding-left: 10px;
                padding-bottom: 10px;">
                <input class="button" type="submit" value="Save" />
                <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#UserEditProfileForm');" />
                <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("Index","Comparinator")%>'" />
            </div>
               <%} %>
        </fieldset>
        </div>
</asp:Content>

