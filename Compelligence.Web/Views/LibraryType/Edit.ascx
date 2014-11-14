<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.LibraryType>" %>
<% string formId = ViewData["Scope"].ToString() + "LibraryTypeEditForm"; %>

<script type="text/javascript">
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
       /* $("#<%= formId %>DeletionUnit").spinner({ buttons: 'auto', min: 0 });*/

    });
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "LibraryTypeEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','LibraryType', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'LibraryType', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="LibraryTypeName" runat="server" Text="<%$ Resources:LabelResource, LibraryTypeName %>" />:</label>
                    <% if ((Model != null) && Model.Permanent.Equals(EntityPermanent.Yes))
                       {
                    %>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name", disabled = "disabled"})%>
                    <%
                        }
                       else
                       {         
                    %>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <% } %>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DeletionUnit" class="required">
                        <asp:Literal ID="LibraryTypeDeletionUnit" runat="server" Text="<%$ Resources:LabelResource, LibraryTypeDeletionUnit %>" />:</label>
                    <%= Html.TextBox("DeletionUnit", null, new { id = formId + "DeletionUnit" })%>
                    <%= Html.ValidationMessage("DeletionUnit", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DeletionPeriod" class="required">
                        <asp:Literal ID="LibraryTypeDeletionPeriod" runat="server" Text="<%$ Resources:LabelResource, LibraryTypeDeletionPeriod %>" />:</label>
                    <%= Html.DropDownList("DeletionPeriod", (SelectList)ViewData["DeletionPeriodList"], string.Empty, new { id = formId + "DeletionPeriod" })%>
                    <%= Html.ValidationMessage("DeletionPeriod", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="LibraryTypeDescription" runat="server" Text="<%$ Resources:LabelResource, LibraryTypeDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
