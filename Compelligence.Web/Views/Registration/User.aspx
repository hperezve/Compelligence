<%@ Page Title="Compelligence - Register User" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.Domain.Entity.UserProfile>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"
        type="text/css" />--%>
        <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/jquery.alerts.css") %>"
        type="text/css" media="screen" />

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.metadata.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"
        type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>

    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-ui/jquery.numeric.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.alerts.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/system/backend/Utility.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
   	    var idZipcode = "#RegistrationUserFormZipCode";
   	  
   	   $(function() { $(idZipcode).numeric();});
	   $(document).ready(function(){$(idZipcode).keyup(function() {AlertZipCode(idZipcode); });	});        
    </script>

    <script type="text/javascript">
        var validator;

        $(function() {

            $('#SendEmailDialog').dialog({ bgiframe: true, autoOpen: false, modal: true });

            var container = $('#ErrorContainer');

            validator = $("#SendEmailForm").validate({
                errorContainer: container,
                errorLabelContainer: $("ul", container),
                wrapper: 'li',
                meta: "validate"
            });

            var formOptions = {
                //target: '#SendEmailResult',
                beforeSubmit: showRequest,
                success: showResponse,
                clearForm: true
            };

            $('#SendEmailForm').submit(function() {

                $(this).ajaxSubmit(formOptions);

                return false;
            });



        });

        // pre-submit callback 
        function showRequest(formData, jqForm, options) {

            if (validator.valid()) {
                $('#SendEmailDialog').dialog('close');

                return true;
            } else {
                return false;
            }
        }

        // post-submit callback
        function showResponse(responseText, statusText, xhr, $form) {
            $('#SendEmailResult').html('<h2>' + responseText + '</h2>');
            //            alert('status: ' + statusText + '\n\nresponseText: \n' + responseText +
            //        '\n\nThe output div should have already been updated with the responseText.');
        }

        var openSendEmailPopup = function() {
            validator.resetForm();

            $('#SendEmailDialog').dialog('open');
            $('#SendEmailResult').empty();
        };  
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="SendEmailDialog" title="Send Email for request DNS" style="display:none;">
        <div id="ErrorContainer" style="display: none;">
            <ul>
                <li>
                    <label for="EmailTo" class="error">
                        Email To is not a valid email address</label></li>
                <li>
                    <label for="MyName" class="error">
                        My Name is required</label></li>
                <li>
                    <label for="MyEmail" class="error">
                        My Email is not a valid email address</label></li>
            </ul>
        </div>
        <form id="SendEmailForm" method="post" action="<%= Url.Action("SendEmailRequestDns", "Registration") %>">
        <div class="line">
            <div class="field">
                <label for="EmailTo">
                    Email to:</label>
                <input id="EmailTo" name="EmailTo" type="text" class="{validate:{required:true,email:true}}"
                    style="width: 200px;" />
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="MyName">
                    My name:</label>
                <input id="MyName" name="MyName" type="text" class="{validate:{required:true}}" style="width: 200px;" />
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="MyEmail">
                    My email:</label>
                <input id="MyEmail" name="MyEmail" type="text" class="{validate:{required:true,email:true}}"
                    style="width: 200px;" />
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="Comments">
                    Comments:</label>
                <textarea id="Comments" name="Comments" rows="2" style="width: 200px;"></textarea>
            </div>
        </div>
        <div class="line">
            <input class="button" type="submit" value="Send" />
            <input class="button" onclick="javascript: resetFormFields('#SendEmailForm');" type="button"
                value="Reset" />
        </div>
        </form>
    </div>
    <div>
        <a href="<%= ViewData["CompanyUrl"] %>">Go to Login Page</a>
    </div>
    <br />
    <br />
    <div class="titleTextOne">
        Welcome to the registration page of Compelligence System</div>
    <div class="titleTextTwo">
        your Competitive Intelligence System!!</div>
    <div id="contentRegister">
        <%= Html.ValidationSummary() %>
        <%  var formId = "RegistrationUserForm";
            using (Html.BeginForm("RegisterUser", "Registration", FormMethod.Post, new { id = formId }))
            { %>
        <div class="marginTop10">
            <fieldset>
                <h2>Register New User</h2>
                <div id="SendEmailResult" class="ntfMsg">
                </div>
                <div class="line">
                    <div class="field">
                        <label for="ClientCompanyDns" class="required">
                            Client Company Dns:</label>
                        <%= Html.TextBox("ClientCompanyDns", ViewData["CompanyDns"], new { @readonly = "readonly" })%>
                        <%--<span id="LinkSendEmail" style="display: none;"><a href="javascript:void(0);" title="Send a email for Request DNS"
                            onclick="javascript: openSendEmailPopup();">
                            <img alt="" src="<%= Url.Content("~/Content/Images/Icons/folder_go.png")%>" /></a></span>--%>
                        <%= Html.ValidationMessage("ClientCompanyDns", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="<%= formId %>FirstName" class="required">
                            <asp:Literal ID="UserFirstName" runat="server" Text="<%$ Resources:LabelResource, UserFirstName %>" />:</label>
                        <%= Html.TextBox("FirstName", null, new { id = formId + "FirstName" }) %>
                        <%= Html.ValidationMessage("FirstName", "*") %>
                    </div>
                    <div class="field">
                        <label for="<%= formId %>LastName" class="required">
                            <asp:Literal ID="UserLastName" runat="server" Text="<%$ Resources:LabelResource, UserLastName %>" />:</label>
                        <%= Html.TextBox("LastName", null, new { id = formId + "LastName" }) %>
                        <%= Html.ValidationMessage("LastName", "*") %>
                    </div>
                    <div class="field">
                        <label for="<%= formId %>Email" class="required">
                            <asp:Literal ID="UserEmail" runat="server" Text="<%$ Resources:LabelResource, UserEmail %>" />:</label>
                        <%= Html.TextBox("Email", null, new { id = formId + "Email" })%>
                        <%= Html.ValidationMessage("Email", "*")%>
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
                        <label for="<%= formId %>CountryCode" class="required">
                            <asp:Literal ID="UserCountryCode" runat="server" Text="<%$ Resources:LabelResource, UserCountryId %>" />:</label>
                        <%= Html.DropDownList("CountryCode", (SelectList)ViewData["CountryCodeList"], string.Empty, new { id = formId + "CountryCode" })%>
                        <%= Html.ValidationMessage("CountryCode", "*")%>
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
                        <%= Html.TextBox( "ZipCode", null, new { id = formId + "ZipCode" }  )%>
                        <%= Html.ValidationMessage("ZipCode", "*")%>
                    </div>
                    <div class="field">
                        <label for="<%= formId %>Phone" class="required">
                            <asp:Literal ID="UserPhone" runat="server" Text="<%$ Resources:LabelResource, UserPhone %>" />:</label>
                        <%= Html.TextBox("Phone", null, new { id = formId + "Phone" })%>
                        <%= Html.ValidationMessage("Phone", "*")%>
                    </div>
                </div>
                <div class="line">
                    <div class="field">
                        <label for="<%= formId %>Fax">
                            <asp:Literal ID="UserFax" runat="server" Text="<%$ Resources:LabelResource, UserFax %>" />:</label>
                        <%= Html.TextBox("Fax", null, new { id = formId + "Fax" })%>
                        <%= Html.ValidationMessage("Fax", "*")%>
                    </div>
                    <div class="field">
                        <label for="<%= formId %>Password" class="required">
                            <asp:Literal ID="UserPassword" runat="server" Text="<%$ Resources:LabelResource, UserPassword %>" />:</label>
                        <%= Html.Password("Kennwort", null, new { id = formId + "Password", autocomplete = "off" })%>
                        <%= Html.ValidationMessage("Password", "*")%>
                    </div>
                    <div class="field">
                        <label for="<%= formId %>RePassword" class="required">
                            <asp:Literal ID="UserRePassword" runat="server" Text="<%$ Resources:LabelResource, UserRePassword %>" />:</label>
                        <%= Html.Password("ReKennwort", null, new { id = formId + "RePassword", autocomplete = "off" })%>
                        <%= Html.ValidationMessage("RePassword", "*")%>
                    </div>
                </div>
                <div class="line">
                    <label>
                        <%= HttpUtility.HtmlEncode(LabelResource.FormRequiredFieldsMessage)%></label>
                </div>
            </fieldset>
        </div>
        <div class="float-left marginLR5px">
            <input class="button" type="submit" value="Save" /></div>
        <div class="float-left">
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" /></div>
        <% } %>
    </div>
</asp:Content>
