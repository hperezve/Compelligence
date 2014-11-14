<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.IndustryCriteria>" %>
<% string formId = ViewData["Scope"].ToString() + "IndustryCriteriaEditForm"; %>

<script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        });
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"] + "IndustryCriteria", null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "IndustryCriteriaEditFormContent",
               OnBegin = "showLoadingDialog",
               OnComplete = "hideLoadingDialog",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'IndustryCriteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div style="height: 316px; overflow: auto">
    <fieldset>
        <legend><%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">       
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'IndustryCriteria', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("IndustryId")%>
        <%= Html.Hidden("CriteriaIdOld")%>
        <div class="line">
            <div class="field">
                <label for="<%= formId %>CriteriaId" class="required">
                    <asp:Literal ID="IndustryCriteriaCriteriaId" runat="server" Text="<%$ Resources:LabelResource, IndustryCriteriaCriteriaId %>" />:</label>
                <%= Html.DropDownList("CriteriaId", (SelectList)ViewData["CriteriaList"], string.Empty, new { id = formId + "CriteriaId" })%>
                <%= Html.ValidationMessage("CriteriaId", "*")%>
            </div>
            <div class="field">
                <label for="<%= formId %>Value" class="required">
                    <asp:Literal ID="IndustryCriteriaValue" runat="server" Text="<%$ Resources:LabelResource, IndustryCriteriaValue %>" />:</label>
                <%= Html.TextBox("Value", null, new { id = formId + "Value" })%>
                <%= Html.ValidationMessage("Value", "*")%>
            </div>
        </div>
        
    </fieldset>
</div>
<% } %>
