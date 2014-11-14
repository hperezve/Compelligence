<%@ Page Title="Compelligence - My Profile" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.UserProfile>" %>

<asp:Content ID="indexHead" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/jquery.alerts.css") %>"  type="text/css" media="screen" />
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.meio.mask.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui/jquery.numeric.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/system/backend/Utility.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.alerts.js") %>" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="ProfileContent" ContentPlaceHolderID="ProfileContent" runat="server">
    <% string formId = "UserForm"; %>

    <script type="text/javascript">
        $(function() {
            $('#<%= formId %>SessionTimeout').setMask({ mask: '9999' });
        });
    </script>
    
    	<script type="text/javascript">
    	    var idZipcode = "#<%= formId %>ZipCode";      	    

    	    $(function() {$(idZipcode).numeric(); });
    	    $(document).ready(function() {$(idZipcode).keyup(function() { AlertZipCode(idZipcode); }); });        
	    </script>
    

    <% Html.RenderPartial("BackEndFormMessages"); %>
    <div class="EditProfile">
        <fieldset>
            <div style="margin-left:11px;"> <h1>My Profile</h1> </div>
            <%= Html.ValidationSummary() %>
            <% using (Html.BeginForm("EditProfile", "MyProfile", FormMethod.Post, new { id = "UserEditProfileForm", align = "left" }))
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
                <% bool UseSystemEmail = (bool)ViewData["UseSystemEmail"];
                   if (!UseSystemEmail)
                   { %>
                <div class="field">
                    <%= Html.ActionLink("Edit Outgoing Email Server", "EditEmailCfg", "MyProfile")%>
                </div>
                <% } %>
                <div class="field">
                    <%= Html.ActionLink("Edit password", "EditPassword", "MyProfile")%>
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
                <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("Index","BackEnd")%>'" />
            </div>
            <% } %>
        </fieldset>
    </div>
    <div />
</asp:Content>
