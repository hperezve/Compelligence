<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.UserProfile>" %>
<% string formId = ViewData["Scope"].ToString() + "UserEditPasswordForm";
   var urlAction = Url.Action("UpdateFieldChanges", "HistoryField");%>

<script type="text/javascript">
    function sendGruopId(urlAction) {
        var getIds = $("#GroupIdUser").val();
        if (getIds != "" && getIds != null) {
            displayNote(urlAction, getIds);
        }
        $("#GroupIdUser").val("");
    }
        $(function() {
        initializeForm('#<%= formId %>');
        sendGruopId('<%= urlAction%>');
        });
</script>

<%= Html.ValidationSummary()%>
<%using (Ajax.BeginForm("EditPassword", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "UserEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "'); executePostActions('#" + ViewData["Scope"].ToString() + "UserEditForm" + "', '" + ViewData["Scope"] + "', 'User', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");sendGruopId('" + urlAction + "');}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <legend>Edit Password</legend>
        <div class="buttonLink">
            <input class="button" type="submit" value="Save" />
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
            <input class="button" type="button" value="Cancel" onclick="javascript: getEntity('<%= Url.Action("Edit", "User") %>', '<%= ViewData["Scope"] %>', 'User', '<%= Model.Id %>', '<%= ViewData["BrowseId"] %>', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');" />
        </div>
        <%= Html.Hidden("GroupIdUser")%>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="Password" class="required">
                        <asp:Literal ID="UserNewPassword" runat="server" Text="<%$ Resources:LabelResource, UserNewPassword %>" />:</label>
                    <%= Html.Password("Kennwort",null, new { autocomplete = "off" })%>
                    <%= Html.ValidationMessage("Password", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="RePassword" class="required">
                        <asp:Literal ID="UserRePassword" runat="server" Text="<%$ Resources:LabelResource, UserRePassword %>" />:</label>
                    <%= Html.Password("ReKennwort", null, new { autocomplete = "off" })%>
                    <%= Html.ValidationMessage("ReKennwort", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
