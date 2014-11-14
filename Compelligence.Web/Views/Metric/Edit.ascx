<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Metric>" %>
<% string formId = ViewData["Scope"].ToString() + "MetricEditForm"; %>

<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>StartDateFrm', 'dt:#<%= formId %>EndDateFrm']);
        });
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "MetricEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "',['dt:#" + formId + "StartDateFrm'], ['dt:#" + formId + "EndDateFrm']); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Metric', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Metric', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
                        <asp:Literal ID="MetricName" runat="server" Text="<%$ Resources:LabelResource, MetricName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="MetricType" runat="server" Text="<%$ Resources:LabelResource, MetricType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Actual">
                        <asp:Literal ID="MetricActual" runat="server" Text="<%$ Resources:LabelResource, MetricActual %>" />:</label>
                    <%= Html.TextBox("Actual", null, new { id = formId + "Actual", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("Actual", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Goal">
                        <asp:Literal ID="MetricGoal" runat="server" Text="<%$ Resources:LabelResource, MetricGoal %>" />:</label>
                    <%= Html.TextBox("Goal", null, new { id = formId + "Goal" })%>
                    <%= Html.ValidationMessage("Goal", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>StartDateFrm">
                        <asp:Literal ID="MetricStartDate" runat="server" Text="<%$ Resources:LabelResource, MetricStartDate %>" />:</label>
                    <%= Html.TextBox("StartDateFrm", null, new { id = formId + "StartDateFrm" })%>
                    <%= Html.ValidationMessage("StartDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>EndDateFrm">
                        <asp:Literal ID="MetricEndDate" runat="server" Text="<%$ Resources:LabelResource, MetricEndDate %>" />:</label>
                    <%= Html.TextBox("EndDateFrm", null, new { id = formId + "EndDateFrm" })%>
                    <%= Html.ValidationMessage("EndDateFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Comment">
                        <asp:Literal ID="MetricComment" runat="server" Text="<%$ Resources:LabelResource, MetricComment %>" />:</label>
                    <%= Html.TextArea("Comment", new { id = formId + "Comment" })%>
                    <%= Html.ValidationMessage("Comment", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
