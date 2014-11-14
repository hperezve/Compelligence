<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.File>" %>
<% string formId = ViewData["Scope"].ToString() + "FileEditForm"; %>
<script type="text/javascript">
        $(function() {
            initializeForm('<%= ViewData["Scope"] %>', 'File');
            initializeUploadField('#FileFileNameLink', '#FileFileNameResult', '#FileFileName');
        });
</script>

<%= Html.ValidationSummary() %>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "FileEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('" + ViewData["Scope"] + "', 'File');  initializeUploadField('#FileFileNameLink', '#FileFileNameResult', '#FileFileName');  executePostActions('" + ViewData["Scope"] + "', 'File', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = (string)ViewData["Scope"] + "FileEditForm" }))
   { %>
<div style="height: 316px; overflow: auto">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <input class="button" type="submit" value="Save" />
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= ViewData["Scope"] %>FileEditForm');" />
            <input class="button" type="button" value="Cancel" onclick="javascript: cancelEntity('<%= ViewData["Scope"] %>', 'File', '<%= ViewData["BrowseId"] %>', <%= ViewData["IsDetail"].ToString().ToLower() %>);" />
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>FileName" class="required">
                    <a id="FileFileNameLink"><asp:Literal runat="server" Text="<%$ Resources:LabelResource, LibraryUploadFile %>" /></a></label><%= Html.ValidationMessage("FileName", "*")%>
                <p id="FileFileNameResult">
                    <%= (Model != null) ? Model.FileName : string.Empty %></p>
                <%= Html.Hidden("FileName", null, new { id = "FileFileName" })%>
            </div>
            <div class="field">
                <label for="<%= formId %>AssignedTo" class="required">
                    <asp:Literal ID="FileAssignedTo" runat="server" Text="<%$ Resources:LabelResource, FileAssignedTo %>" />:</label>
                <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["UserIdlist"], string.Empty, new { id = formId + "AssignedTo" })%>
                <%= Html.ValidationMessage("AssignedTo", "*")%>
            </div>
            <div class="field">
                <label for="<%= formId %>Version" class="required">
                    <asp:Literal ID="FileVersion" runat="server" Text="<%$ Resources:LabelResource, FileVersion %>" />:</label>
                <%= Html.TextBox("Version", null, new { id = formId + "Version" })%>
                <%= Html.ValidationMessage("Version", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>Size" class="required">
                    <asp:Literal ID="FileSize" runat="server" Text="<%$ Resources:LabelResource, FileSize %>" />:</label>
                <%= Html.TextBox("Size", null, new { id = formId + "Size" })%>
                <%= Html.ValidationMessage("Size", "*")%>
            </div>
        </div>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>Description" class="required">
                    <asp:Literal ID="FileDescription" runat="server" Text="<%$ Resources:LabelResource, FileDescription %>" />:</label>
                <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                <%= Html.ValidationMessage("Description", "*")%>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
