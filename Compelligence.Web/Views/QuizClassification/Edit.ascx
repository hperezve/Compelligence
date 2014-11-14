<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.QuizClassification>" %>
<% string formId = ViewData["Scope"].ToString() + "QuizClassificationEditForm"; %>

<script type="text/javascript">
    var initializeByRoot = function() {
    var rootValue = $('#<%= formId %>Root').prop('checked')
        if (rootValue) {
            $('#<%= formId %>IndustryId').prop('disabled', 'disabled')
            $('#<%= formId %>CompetitorId').prop('disabled', 'disabled')
            $('#<%= formId %>ProductId').prop('disabled', 'disabled')
        }
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        initializeByRoot();
    });
</script>

<script type="text/javascript">
    var root = function(invoke) {
        if (invoke.checked == true) {
            $('#<%= formId %>IndustryId').prop('disabled', 'disabled')
            $('#<%= formId %>IndustryId').prop('value', '')
            $('#<%= formId %>CompetitorId').prop('disabled', 'disabled')
            $('#<%= formId %>CompetitorId').prop('value', '')
            $('#<%= formId %>ProductId').prop('disabled', 'disabled')
            $('#<%= formId %>ProductId').prop('value', '')
        }
        else {
            $('#<%= formId %>IndustryId').removeAttr('disabled', 'disabled')
            $('#<%= formId %>CompetitorId').removeAttr('disabled', 'disabled')
            $('#<%= formId %>ProductId').removeAttr('disabled', 'disabled')
        }
    };
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "QuizClassificationEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "');  initializeByRoot(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'QuizClassification', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'QuizClassification', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("QuizId")%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Visible" class="required">
                        <asp:Literal ID="QuizClassificationVisible" runat="server" Text="<%$ Resources:LabelResource, QuizClassificationVisible %>" />:</label>
                    <%= Html.DropDownList("Visible", (SelectList)ViewData["QuizClassificationList"], string.Empty, new { id = formId + "Visible" })%>
                    <%= Html.ValidationMessage("Visible", "*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>Root" class="required">
                        <asp:Literal ID="QuizClassificationRoot" runat="server" Text="<%$ Resources:LabelResource, QuizClassificationRoot %>" />:</label>
                    <%= Html.CheckBox("Root", new { id = formId + "Root", onclick = "javascript:root(this);" })%>
                    <%= Html.ValidationMessage("Root", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>IndustryId">
                        <asp:Literal ID="QuizClassificationIndustryId" runat="server" Text="<%$ Resources:LabelResource, QuizClassificationIndustryId %>" />:</label>
                    <%= Html.CascadingParentDropDownList("IndustryId", (SelectList)ViewData["IndustryIdList"], string.Empty, Url.Action("GetCompetitorsByIndustry", "QuizClassification"), formId, "CompetitorId", new string[] { "ProductId" })%>
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CompetitorId">
                        <asp:Literal ID="QuizClassificationCompetitorId" runat="server" Text="<%$ Resources:LabelResource, QuizClassificationCompetitorId %>" />:</label>
                    <%= Html.CascadingParentDropDownList("CompetitorId", (SelectList)ViewData["CompetitorIdList"], string.Empty, true, Url.Action("GetProductsByCompetitor", "QuizClassification"), new string[] { "IndustryId"}, formId, "ProductId")%>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>ProductId">
                        <asp:Literal ID="QuizClassificationProductId" runat="server" Text="<%$ Resources:LabelResource, QuizClassificationProductId %>" />:</label>
                    <%= Html.CascadingChildDropDownList("ProductId", (SelectList)ViewData["ProductIdList"], string.Empty, formId)%>
                    <%= Html.ValidationMessage("ProductId", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
