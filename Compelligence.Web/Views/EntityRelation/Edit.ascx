<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.EntityRelation>" %>
<% string formId = ViewData["Scope"].ToString() + "EntityRelationEditForm"; %>

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
               UpdateTargetId = (string)ViewData["Scope"] + "EntityRelationEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'EntityRelation', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'EntityRelation', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
                    <label for="<%= formId %>Type" class="required">
                        <asp:Literal ID="EntityRelationType" runat="server" Text="<%$ Resources:LabelResource, EntityRelationType %>" />:</label>
                    <%--<%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>--%>
                    <%= Html.CascadingParentDropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, Url.Action("GetChildEntityIdByType", "EntityRelation"), formId, "ChildEntityId")%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>ChildEntityId" class="required">
                        <asp:Literal ID="EntityRelationChildEntityId" runat="server" Text="<%$ Resources:LabelResource, EntityRelationChildEntityId %>" />:</label>
                    <%= Html.CascadingChildDropDownList("ChildEntityId", (SelectList)ViewData["ChildEntityIdList"], string.Empty, formId)%>
                    <%--<%= Html.DropDownList("ChildEntityId", (SelectList)ViewData["ChildEntityIdList"], string.Empty, formId)%>--%>
                    <%= Html.ValidationMessage("ChildEntityId", "*")%>
                </div>
                <%--<div class="field">
                <label for="Type">
                    <asp:Literal ID="ProjectIndustryId" runat="server" Text="<%$ Resources:LabelResource, ProjectIndustryId %>" />:</label>
                <%= Html.CascadingParentDropDownList("IndustryId", (SelectList)ViewData["IndustryIdList"], string.Empty, Url.Action("GetCompetitorsByIndustry", "Project"), formId, "CompetitorId", new string[] { "ProductId" })%>
                <%= Html.ValidationMessage("IndustryId", "*")%>
            </div>
            <div class="field">
                <label for="ObjectiveName">
                    <asp:Literal ID="ProjectCompetitorId" runat="server" Text="<%$ Resources:LabelResource, ProjectCompetitorId %>" />:</label>
                <%= Html.CascadingParentDropDownList("CompetitorId", (SelectList)ViewData["CompetitorIdList"], string.Empty, true, Url.Action("GetProductsByCompetitor", "Project"), new string[] { "IndustryId" }, formId, "ProductId")%>
                <%= Html.ValidationMessage("CompetitorId", "*")%>
            </div>--%>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="EntityRelationDescription" runat="server" Text="<%$ Resources:LabelResource, EntityRelationDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
