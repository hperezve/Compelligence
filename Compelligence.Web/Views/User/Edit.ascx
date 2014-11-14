<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.UserProfile>" %>
<% string formId = ViewData["Scope"].ToString() + "UserEditForm";
   var urlAction = Url.Action("UpdateFieldChanges", "HistoryField");%>
<link rel="stylesheet" href="<%= Url.Content("~/Content/Styles/jquery.alerts.css") %>"
    type="text/css" media="screen" />

<script src="<%= Url.Content("~/Scripts/jquery-ui/jquery.numeric.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/system/backend/Utility.js") %>" type="text/javascript"></script>

<script src="<%= Url.Content("~/Scripts/jquery.alerts.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryUser');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth
                if (lis.length > 3) {
                    newHeigth = 328 - 60 - (11.5 * lis.length);
                }
                else {
                    newHeigth = 328 - 48 - (10 * lis.length);
                }
                var edt = $('#UserEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>

<script type="text/javascript">
    var idZipcode = "#<%= formId %>ZipCode";

    $(function() {$(idZipcode).numeric(); });
    $(document).ready(function() {$(idZipcode).keyup(function() {AlertZipCode(idZipcode); }); });

    $("#<%= formId %>AlertMaxUsersDialog").dialog({
        bgiframe: true,
        autoOpen: false,
        modal: true,
        buttons: {
            Ok: function() {
                $(this).dialog('close');
            }
        }
    });
    function sendGruopId(urlAction) {
        var getIds = $("#GroupIdUser").val();
        if (getIds != "" && getIds != null) {
            displayNote(urlAction, getIds);
        }
        $("#GroupIdUser").val("");
    }
    function checkNumUsers() {
        var isMaxUsers = false;    
        $.ajax({
            type: 'POST',
            url: '<%= Url.Action("CheckMaxNumUsers", "User") %>',
            async: false,
            dataType: 'json',
            success: function(data){
                $("#<%= formId %>AlertMaxUsersMsg").text(data.msg);
                isMaxUsers = data.imu;
            }
        });

        if (isMaxUsers) {
            $("#<%= formId %>AlertMaxUsersDialog").css('display', 'block');
            $("#<%= formId %>AlertMaxUsersDialog").dialog('open');
        }
        
        return !isMaxUsers;
    }    
</script>
<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        checkNumUsers();
        sendGruopId('<%= urlAction%>');
    });    
</script>
<div id="ValidationSummaryUser">
    <%= Html.ValidationSummary()%>
</div>
<% string editFormContent = "UserEditFormContent";
   string entitySucces = "User";

   if (ViewData["Scope"].ToString().Equals("AdminTeam") && ViewData["BrowseId"].ToString().Equals("TeamMemberDetail"))
   {
       editFormContent = "TeamMemberEditFormContent";
       entitySucces = "TeamMember";
   }

   using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + editFormContent,
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','User', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + "); checkNumUsers();sendGruopId('" + urlAction + "');}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   {
%>
<div class="indexTwo">
    <fieldset>
    
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save, new { onClick = ((ActionMethod)ViewData["ActionMethodValue"] == ActionMethod.Create) ? "javascript: return checkNumUsers();" : string.Empty})%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <% if (ViewData["Scope"].ToString().Equals("Admin") && ViewData["BrowseId"].ToString().Equals("UserAll"))
               { %>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'User', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
            <% }
               else if (ViewData["Scope"].ToString().Equals("AdminTeam") && ViewData["BrowseId"].ToString().Equals("TeamMemberDetail"))
               { %>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'TeamMember', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
            <% } %>
        </div>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("GroupIdUser")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OriginalStatus")%>
        <%= Html.Hidden("Id")%>
        <div id="UserEditFormInternalContent" class="contentFormEdit">
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
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="UserStatus" runat="server" Text="<%$ Resources:LabelResource, UserStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id=formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Email" class="required">
                        <asp:Literal ID="UserEmail" runat="server" Text="<%$ Resources:LabelResource, UserEmail %>" />:</label>
                    <%= Html.TextBox("Email", null, new { id = formId + "Email" })%>
                    <%= Html.ValidationMessage("Email", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Title">
                        <asp:Literal ID="UserTitle" runat="server" Text="<%$ Resources:LabelResource, UserTitle %>" />:</label>
                    <%= Html.TextBox("Title", null, new { id = formId + "Title" })%>
                    <%= Html.ValidationMessage("Title", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>SecurityGroupId" class="required">
                        <asp:Literal ID="UserSecurityGroupId" runat="server" Text="<%$ Resources:LabelResource, UserSecurityGroupId %>" />:</label>
                    <%= Html.DropDownList("SecurityGroupId", (SelectList)ViewData["SecurityGroupList"], string.Empty, new { id = formId + "SecurityGroupId" })%>
                    <%= Html.ValidationMessage("SecurityGroupId", "*")%>
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
                <div class="field">
                    <label for="<%= formId %>Department" class="required">
                        <asp:Literal ID="UserDepartment" runat="server" Text="<%$ Resources:LabelResource, UserDepartment %>" />:</label>
                    <%= Html.TextBox("Department", null, new { id = formId + "Department" })%>
                    <%= Html.ValidationMessage("Department", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>ZipCode" class="required">
                        <asp:Literal ID="UserZipCode" runat="server" Text="<%$ Resources:LabelResource, UserZipCode %>" />:</label>
                    <%= Html.TextBox("ZipCode", null, new { id = formId + "ZipCode" })%>
                    <%= Html.ValidationMessage("ZipCode", "*")%>
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
                <div class="field">
                    <label for="<%= formId %>ReportTo">
                        <asp:Literal ID="UserReportTo" runat="server" Text="<%$ Resources:LabelResource, UserReportTo %>" />:</label>
                    <%= Html.DropDownList("ReportTo", (SelectList)ViewData["ReportToList"], string.Empty, new { id = formId + "ReportTo" })%>
                    <%= Html.ValidationMessage("ReportTo", "*")%>
                </div>
            </div>
            <div class="line">
                <% if (Compelligence.Security.Managers.UserManager.GetInstance().IsAdminUser(Session["UserId"] as string))
                   {
                        if ((ActionMethod)ViewData["ActionMethodValue"] == ActionMethod.Create)
                        { %>
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
                                <%= Html.ValidationMessage("ReKennwort", "*")%>
                            </div>
                       <%}
                        else
                        {%>
                            <div class="field textInColumn">
                                <a href="javascript: void(0);" onclick="javascript: loadEntity('<%= Url.Action("EditPassword", "User") %>', '<%= ViewData["Scope"] %>', 'User', '<%= Model.Id %>', '<%= ViewData["BrowseId"] %>', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');">
                                Edit Password </a>
                            </div>
                       <%}
            } %>
            </div>
        </div>
    </fieldset>
    <div id="<%= formId %>AlertMaxUsersDialog" title="Company maximum number of Users" style="display:none;">
        <p>
            <span class="ui-icon ui-icon-alert alertFailedResponseDialog"></span><span id="<%= formId %>AlertMaxUsersMsg"></span>
        </p>
    </div>
</div>
<% } %>
