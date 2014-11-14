<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.WinLossAnalysis>" %>
<% string formId = ViewData["Scope"].ToString() + "WinLossAnalysisEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummaryWinLossAnalysis');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#WinLossAnalysisEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>StartDate']);
        ResizeHeightForm();
        });
</script>

<div id="ValidationSummaryWinLossAnalysis">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "WinLossAnalysisEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "',['dt:#" + formId + "StartDate']); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','WinLossAnalysis', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",                          
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'WinLossAnalysis', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldTitle")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="WinLossAnalysisEditFormInternalContent" class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>IndustryId" class="required">
                        <asp:Literal ID="WinLossTitle" runat="server" Text="<%$ Resources:LabelResource, WinLossAnalysisIndustry %>" />:</label>
                    <%= Html.DropDownList("IndustryId", (SelectList)ViewData["IndustryList"], string.Empty, new { id = formId + "IndustryId" })%>                                        
                    <%= Html.ValidationMessage("IndustryId", "*")%>
                </div>  
                
                <div class="field">
                    <label for="<%= formId %>CompetitorId" class="required">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, WinLossAnalysisCompetitor %>" />:</label>
                    <%= Html.DropDownList("CompetitorId", (SelectList)ViewData["CompetitorList"], string.Empty, new { id = formId + "CompetitorId" })%>
                    <%= Html.ValidationMessage("CompetitorId", "*")%>
                    
                    
                                       
                </div>   
                
                 <div class="field">
                    <label for="<%= formId %>TimePeriod" class="required">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:LabelResource, WinLossAnalysisTime %>" />:</label>
                    <%= Html.DropDownList("TimePeriod", (SelectList)ViewData["TimePeriodList"], string.Empty, new { id = formId + "TimePeriod" })%>
                    <%= Html.ValidationMessage("TimePeriod", "*")%>
                    
                </div>  
                
                </div>
                  <div class="line">
                  
                 <div class="field">
                    <label for="<%= formId %>NumberPeriods" class="required">
                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:LabelResource, WinLossAnalysisNumber %>" />:</label>
                    <%= Html.TextBox("NumberPeriods", null, new { id = formId + "NumberPeriods" })%>
                    <%= Html.ValidationMessage("NumberPeriods", "*")%>
                </div>  
                
                 <div class="field">
                    <label for="<%= formId %>StartDate" class="required">
                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:LabelResource, WinLossAnalysisDate %>" />:</label>
                    <%= Html.TextBox("StartDate", null, new { id = formId + "StartDate" })%>
                    <%= Html.ValidationMessage("StartDate", "*")%>
                </div>  
                
                  </div>                            
            
          
        </div>
    </fieldset>
</div>
<% } %>
