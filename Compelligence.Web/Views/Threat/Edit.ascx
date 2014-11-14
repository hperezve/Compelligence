<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Threat>" %>
<% string formId = ViewData["Scope"].ToString() + "ThreatEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryThreat');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 70 - (10 * lis.length);
                var edt = $('#ThreatEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };    
</script>
<script type="text/javascript">
    $(function() {
    initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>StartDateFrm', 'dt:#<%= formId %>EndDateFrm']);
    ResizeHeightForm();
    });

</script>

<div id="ValidationSummaryThreat">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "ThreatEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "',['dt:#" + formId + "StartDateFrm'],['dt:#" + formId + "EndDateFrm']); ResizeHeightForm();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Threat', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = (string)ViewData["Scope"] + "ThreatEditForm" }))
   { %>
<div id="ThreatIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Threat', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
            <%--<input class="button" type="button" value="Report" onclick="javascript: executeReport('ThreatAll');" />--%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("Id", default(decimal))%>
        
        <div id="ThreatEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="ThreatName" runat="server" Text="<%$ Resources:LabelResource, ThreatName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "ThreatName" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Source">
                        <asp:Literal ID="ThreatSource" runat="server" Text="<%$ Resources:LabelResource, ThreatSource %>" />:</label>
                    <%= Html.TextBox("Source", null, new { id = formId + "ThreatSource" })%>
                    <%= Html.ValidationMessage("Source", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>StartDateFrm">
                        <asp:Literal ID="ThreatStartDate" runat="server" Text="<%$ Resources:LabelResource, ThreatStartDate %>" />:</label>
                    <%= Html.TextBox("StartDateFrm", null, new { id = formId + "StartDateFrm" })%>
                    <%= Html.ValidationMessage("StartDateFrm", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>EndDateFrm">
                        <asp:Literal ID="ThreatEndDate" runat="server" Text="<%$ Resources:LabelResource, ThreatEndDate %>" />:</label>
                    <%= Html.TextBox("EndDateFrm", null, new { id = formId + "EndDateFrm" })%>
                    <%= Html.ValidationMessage("EndDateFrm", "*")%>
                </div>

            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="ThreatDescription" runat="server" Text="<% $ Resources:LabelResource, ThreatDescription %>"/>:</label>
                        <%= Html.TextArea("Description", new  { id=formId + "Description"})%>
                        <%= Html.ValidationMessage("Description", "*") %>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>