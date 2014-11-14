<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Employee>" %>
<% string formId = ViewData["Scope"].ToString() + "EmployeeEditForm"; %>

<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        });
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "EmployeeEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Employee', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Employee', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>FirstName" class="required">
                        <asp:Literal ID="EmployeeFirstName" runat="server" Text="<%$ Resources:LabelResource, EmployeeFirstName %>" />:</label>
                    <%= Html.TextBox("FirstName", null, new { id = formId + "FirstName" })%>
                    <%= Html.ValidationMessage("FirstName", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>LastName" class="required">
                        <asp:Literal ID="EmployeeLastName" runat="server" Text="<%$ Resources:LabelResource, EmployeeLastName %>" />:</label>
                    <%= Html.TextBox("LastName", null, new { id = formId + "LastName" })%>
                    <%= Html.ValidationMessage("LastName", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Role">
                        <asp:Literal ID="EmployeeRole" runat="server" Text="<%$ Resources:LabelResource, EmployeeRole %>" />:</label>
                    <%= Html.TextBox("Role", null, new { id = formId + "Role" })%>
                    <%= Html.ValidationMessage("Role", "*")%>
                </div>
            </div>
             
            <div class="line">    
                <div class="field">
                    <label for="<%= formId %>PhoneNumber">
                        <asp:Literal ID="EmployeePhoneNumber" runat="server" Text="<%$ Resources:LabelResource, EmployeePhone %>" />:</label>
                    <%= Html.TextBox("PhoneNumber", null, new { id = formId + "PhoneNumber" })%>
                    <%= Html.ValidationMessage("PhoneNumber", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CellNumber" >
                        <asp:Literal ID="EmployeeCellNumber" runat="server" Text="<%$ Resources:LabelResource, EmployeeCellNumber %>" />:</label>
                    <%= Html.TextBox("CellNumber", null, new { id = formId + "CellNumber" })%>
                    <%= Html.ValidationMessage("CellNumber", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Fax" >
                        <asp:Literal ID="EmployeeFax" runat="server" Text="<%$ Resources:LabelResource, EmployeeFax %>" />:</label>
                    <%= Html.TextBox("Fax", null, new { id = formId + "Fax" })%>
                    <%= Html.ValidationMessage("Fax", "*")%>
                </div>
            </div>
                
            <div class="line">                                
                <% if (ViewData["HeaderType"].ToString().Equals(DomainObjectType.Partner) && ((ActionMethod)ViewData["ActionMethodValue"] == ActionMethod.Edit))
                   { %>
                <div class="field textInColumn">
                    <a href="javascript: void(0);" onclick="javascript: loadEntity('<%= Url.Action("EditPassword", "Employee") %>', '<%= ViewData["Scope"] %>', 'Employee', '<%= Model.Id %>', '<%= ViewData["BrowseId"] %>', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>')">
                        Edit Password </a>
                </div>
                <% } %>
           
              </div>
            <div class="line">
                <% if (ViewData["HeaderType"].ToString().Equals(DomainObjectType.Partner) && ((ActionMethod)ViewData["ActionMethodValue"] == ActionMethod.Create))
                   { %>
                <div class="field">
                    <label for="<%= formId %>Password" class="required">
                        <asp:Literal ID="EmployeePassword" runat="server" Text="<%$ Resources:LabelResource, EmployeePassword %>" />:</label>
                    <%= Html.Password("Kennwort", null, new { id = formId + "Password", autocomplete = "off" })%>
                    <%= Html.ValidationMessage("Password", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>RePassword" class="required">
                        <asp:Literal ID="EmployeeRePassword" runat="server" Text="<%$ Resources:LabelResource, EmployeeRePassword %>" />:</label>
                    <%= Html.Password("ReKennwort", null, new { id = formId + "RePassword", autocomplete = "off" })%>
                    <%= Html.ValidationMessage("RePassword", "*")%>
                </div>
                <% } %>
              
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Email">
                        <asp:Literal ID="EmployeeEmail" runat="server" Text="<%$ Resources:LabelResource, EmployeeEmail %>" />:</label>
                    <%= Html.TextBox("Email", null, new { id = formId + "Email" })%>
                    <%= Html.ValidationMessage("Email", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Division">
                        <asp:Literal ID="EmployeeDivision" runat="server" Text="<%$ Resources:LabelResource, EmployeeDivision %>" />:</label>
                    <%= Html.TextBox("Division", null, new { id = formId + "Division" })%>
                    <%= Html.ValidationMessage("Division", "*")%>
                </div>
                 </div>
                <div class="line">
                <div class="field">
                   <label for="<%= formId %>Notes">
                        <asp:Literal ID="EmployeeNotes" runat="server" Text="<%$ Resources:LabelResource, EmployeeNotes %>" />:</label>
                    <%= Html.TextArea("Notes", null, new { id = formId + "Notes" })%>
                    <%= Html.ValidationMessage("Notes", "*")%>
                </div>
                </div>
              
        </div>
    </fieldset>
</div>
<% } %>
