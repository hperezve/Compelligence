<%@ Page Title="Compelligence - Email configuration" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage<Compelligence.DataTransfer.Entity.EmailUserCfgRegistrationDTO>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {

            if ($('#EditEmailPassword').val() == 'Y') {
                showEmailPassword();
            }

            $('#ShowEmailPasswordLine').click(showEmailPassword);
        });

        var showEmailPassword = function() {
            $('#EmailPasswordLine').slideToggle('slow', function() {
                if ($('#EmailPasswordLine').css('display').indexOf('none') >= 0) {
                    $('#EditEmailPassword').val('N');
                    $('#EmailPassword').val('');
                    $('#ReEmailPassword').val('');
                } else {
                    $('#EditEmailPassword').val('Y');
                }
            });
        };
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("BackEndFormMessages"); %>
    <div class="EditProfile">
        
        <%= Html.ValidationSummary()%>
        <% using (Html.BeginForm("EditPopEmailCfg", "MyProfile", FormMethod.Post, new { id = "EmailConfigurationEditForm", align = "left" }))
           { %>
        <%= Html.Hidden("UserId")%>
        <%= Html.Hidden("EmailUser")%>
        <%= Html.Hidden("ClientCompany")%>
        <%= Html.Hidden("EditEmailPassword")%>
        <fieldset>
            <div style="margin-left:11px;"><h1>My Email Configuration</h1></div>
            <div class="line">
                <div class="field">
                    <label for="EmailUser">
                        Email User:</label>
                    <%= Model.EmailUser %>
                </div>
            </div>
            <%-- <div class="line">
                <fieldset style="width: 95%;">
                    <legend>Smtp Configuration</legend>
                    <div class="line">
                        <div class="field">
                            <label for="SmtpServer">
                                Server:</label>
                            <%= Html.TextBox("SmtpServer")%>
                            <%= Html.ValidationMessage("SmtpServer", "*")%>
                        </div>
                    </div>
                    <div class="line">
                        <div class="field">
                            <label for="SmtpPort">
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
            </div>--%>
            <div class="line">
                <div class="field">
                    <h3>
                        Pop Configuration</h3>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="PopServer">
                        Server:</label>
                    <%= Html.TextBox("PopServer")%>
                    <%= Html.ValidationMessage("PopServer", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="PopPort">
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
            <div class="line">
                <div class="field">
                    <a href="javascript: void(0);" id="ShowEmailPasswordLine">Edit email password</a>
                </div>
            </div>
            <div class="line" id="EmailPasswordLine" style="display: none;">
                <div class="field">
                    <label for="EmailPassword">
                        Email Password:</label>
                    <%= Html.Password("EmailPassword")%>
                    <%= Html.ValidationMessage("EmailPassword", "*")%>
                </div>
                <div class="field">
                    <label for="ReEmailPassword">
                        Re-type Email Password:</label>
                    <%= Html.Password("ReEmailPassword")%>
                    <%= Html.ValidationMessage("ReEmailPassword", "*")%>
                </div>
            </div>
            <div class="float-left" style="margin-top: 20px; margin-left: 12px; padding-left: 10px;
                padding-bottom: 10px;">
                <input class="button" type="submit" value="Save" />
                <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#EmailConfigurationEditForm');" />
                <input class="button" type="button" value="Cancel" onclick="location.href='<%=Url.Action("EditProfile","MyProfile")%>'" />
            </div>
        </fieldset>
        <% } %>
    </div>
</asp:Content>