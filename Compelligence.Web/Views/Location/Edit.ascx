<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Location>" %>
<% string formId = ViewData["Scope"].ToString() + "LocationEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryLocation');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');

            if (lis.length > 0) {

                var edfg = $('#LocationEditFormInternalContent').prop("style", "height:177px");
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#LocationEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        });
</script>

<div id="ValidationSummaryLocation">
<%= Html.ValidationSummary()%>
</div>

<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "LocationEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Location', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Location', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="LocationEditFormInternalContent"  class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CountryCode" class="required">
                        <asp:Literal ID="LocationCountryCode" runat="server" Text="<%$ Resources:LabelResource, LocationCountryCode %>" />:</label>
                    <%= Html.TextBox("CountryCode", null, new { id = formId + "CountryCode" })%>
                    <%= Html.ValidationMessage("CountryCode", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AddressLine1" class="required">
                        <asp:Literal ID="LocationAddressLine1" runat="server" Text="<%$ Resources:LabelResource, LocationAddressLine1 %>" />:</label>
                    <%= Html.TextBox("AddressLine1", null, new { id = formId + "AddressLine1" })%>
                    <%= Html.ValidationMessage("AddressLine1", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AddressLine2">
                        <asp:Literal ID="LocationAddressLine2" runat="server" Text="<%$ Resources:LabelResource, LocationAddressLine2 %>" />:</label>
                    <%= Html.TextBox("AddressLine2", null, new { id = formId + "AddressLine2" })%>
                    <%= Html.ValidationMessage("AddressLine2", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>AddressLine3">
                        <asp:Literal ID="LocationAddressLine3" runat="server" Text="<%$ Resources:LabelResource, LocationAddressLine3 %>" />:</label>
                    <%= Html.TextBox("AddressLine3", null, new { id = formId + "AddressLine3" })%>
                    <%= Html.ValidationMessage("AddressLine3", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AddressLine4">
                        <asp:Literal ID="LocationAddressLine4" runat="server" Text="<%$ Resources:LabelResource, LocationAddressLine4 %>" />:</label>
                    <%= Html.TextBox("AddressLine4", null, new { id = formId + "AddressLine4" })%>
                    <%= Html.ValidationMessage("AddressLine4", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>City" class="required">
                        <asp:Literal ID="LocationCity" runat="server" Text="<%$ Resources:LabelResource, LocationCity %>" />:</label>
                    <%= Html.TextBox("City", null, new { id = formId + "City" })%>
                    <%= Html.ValidationMessage("City", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>State">
                        <asp:Literal ID="LocationState" runat="server" Text="<%$ Resources:LabelResource, LocationState %>" />:</label>
                    <%= Html.TextBox("State", null, new { id = formId + "State" })%>
                    <%= Html.ValidationMessage("State", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>PostalCode">
                        <asp:Literal ID="LocationPostalCode" runat="server" Text="<%$ Resources:LabelResource, LocationPostalCode %>" />:</label>
                    <%= Html.TextBox("PostalCode", null, new { id = formId + "PostalCode" })%>
                    <%= Html.ValidationMessage("PostalCode", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="LocationDescription" runat="server" Text="<%$ Resources:LabelResource, LocationDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
