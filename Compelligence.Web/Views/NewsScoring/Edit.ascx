<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.NewsScoring>" %>
<% string formId = ViewData["Scope"].ToString() + "NewsScoringEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryNewsScoring');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 70 - (10 * lis.length);
                var edt = $('#NewsScoringEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };    
</script>
<script type="text/javascript">
    $(function() {
    initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>StartDateFrm', 'dt:#<%= formId %>EndDateFrm', 'dt:#<%= formId %>DateOfMaturityFrm']);
    ResizeHeightForm();
    });

</script>

<div id="ValidationSummaryNewsScoring">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "NewsScoringEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "',['dt:#" + formId + "StartDateFrm'],['dt:#" + formId + "EndDateFrm'],['dt:#" + formId + "DateOfMaturityFrm']); ResizeHeightForm();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','NewsScoring', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = (string)ViewData["Scope"] + "NewsScoringEditForm" }))
   { %>
<div id="NewsScoringIndexTwo" class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');resetMultiSelect();" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'NewsScoring', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
            <%--<input class="button" type="button" value="Report" onclick="javascript: executeReport('NewsScoringAll');" />--%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("Id", default(decimal))%>
        
        <div id="NewsScoringEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="NewsScoringName" runat="server" Text="<%$ Resources:LabelResource, NewsScoringName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "NewsScoringName" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="NewsScoringType" runat="server" Text="<%$ Resources:LabelResource, NewsScoringType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id=formId + "Type"})%>
                    <%= Html.ValidationMessage("Type","*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="NewsScoringStatus" runat="server" Text="<%$ Resources:LabelResource, NewsScoringStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id=formId + "Status"})%>
                    <%= Html.ValidationMessage("Status","*") %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Positives">
                        <asp:Literal ID="NewsScoringPositives" runat="server" Text="<%$ Resources:LabelResource, NewsScoringPositives %>" />:</label>
                    <%= Html.TextArea("Positives", null, new { id = formId + "Positives" })%>
                    <%= Html.ValidationMessage("Positives", "*")%>
                </div>
                </div>
                <div class="line">
                <div class="field">
                    <label for="<%= formId %>Negatives">
                        <asp:Literal ID="NewsScoringNegatives" runat="server" Text="<%$ Resources:LabelResource, NewsScoringNegatives %>" />:</label>
                    <%= Html.TextArea("Negatives", null, new { id = formId + "Negatives" })%>
                    <%= Html.ValidationMessage("Negatives", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>