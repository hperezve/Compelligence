<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.LibraryExternalSource>" %>
<% string formId = ViewData["Scope"].ToString() + "LibraryExternalSourceEditForm"; %>

<script type="text/javascript">
    var disabledType = function() {
        var selectedValue = $('#<%= formId %>Type > option:selected').prop('value');
        if (selectedValue === '<%= Compelligence.Domain.Entity.Resource.LibraryExternalSourceType.Bing %>') {
            //$('#<%=formId %>Type').focus();
            $('#<%=formId %>Source').val('');
            //$('#<%=formId %>Source').prop('disabled', true);
            $('#divSource').hide();
        } else {
           // $('#<%=formId %>Source').prop('disabled', false);
           $('#divSource').show();
        }
    };
    var showEmail = function() {
    var selectedValue = $('#<%= formId %>Type > option:selected').prop('value');
        if (selectedValue == '<%= Compelligence.Domain.Entity.Resource.LibraryExternalSourceType.DynamicRss %>' || selectedValue == '<%= Compelligence.Domain.Entity.Resource.LibraryExternalSourceType.Bing %>') {
            $('#TargetFrom').css('display', 'block');
            if (selectedValue === '<%= Compelligence.Domain.Entity.Resource.LibraryExternalSourceType.Bing %>') {
                $('#<%=formId %>Source').val('');
                //$('#<%=formId %>Source').prop('disabled', true);
                $('#divSource').hide();
            }else{
                //$('#<%=formId %>Source').prop('disabled', false);
                $('#divSource').show();
            }
         }
        else {
            $('#TargetFrom').css('display', 'none');
           // $('#<%=formId %>Source').prop('disabled', false);
            $('#divSource').show();
         }
    };
    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>LibraryExternalSourceDateFrm']);
        showEmail();
        disabledType();
    });
    function executeLink(url) {
        showLoadingDialog();
        //showLoadingDialogForSection('<%= ViewData["Container"] %>');
        $.get(url, {}, function(data) {
            hideLoadingDialog();
        });
    }
    function newExecuteLink(id) {
        showLoadingDialog();
        showLoadingDialogForSection('<%= ViewData["Container"] %>');
        var parameters = { Id: id };
        var results = null;
        var xmlhttp;
        $.post(
            '<%= Url.Action("ExecuteRssReaderContent", "LibraryExternalSource") %>',
            parameters,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    var resultBoolean = true;
                    if (results == 'true' || results == 'True' || results == 'TRUE') {
                        resultBoolean = true;
                    }
                    else if (results == 'false' || results == 'False' || results == 'FALSE') {
                        resultBoolean = false;
                    }
                    if (!resultBoolean) {
                        showIncorrectProcessDialog();
                    }
                }
            });        
    }
</script>

<%= Html.ValidationSummary()%>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "LibraryExternalSourceEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "LibraryExternalSourceDateFrm']); showEmail();disabledType();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','LibraryExternalSource', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'LibraryExternalSource', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ")" })%>
            <% if (Model!= null)
               { %>
                <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Execute, new { onClick = "javascript: executeLink('" + Url.Action("ExecuteRSSReader", "LibraryExternalSource", new { id=Model.Id}) + "');" })%>
                <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Execute, new { onClick = "javascript: newExecuteLink('" + Model.Id + "');" })%>--%>
            <% }%>
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
                    <label for="<%= formId %>Type"  class="required">
                        <asp:Literal ID="LibraryExternalSourceType" runat="server" Text="<%$ Resources:LabelResource, LibraryExternalSourceType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type", onchange = "javascript: showEmail();" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
                <div class="doublefield" style="padding-right: 60px;" id="divSource">
                    <label for="<%= formId %>Source" class="required">
                        <asp:Literal ID="LibraryExternalSourceSource" runat="server" Text="<%$ Resources:LabelResource, LibraryExternalSourceSource %>" />:</label>
                    <%= Html.TextBox("Source", null, new { id = formId + "Source",style="width:530px" })%>
                    <%= Html.ValidationMessage("Source", "*")%>
                </div>
                
                <div class="field"  id="TargetFrom" style="display:none">
                  <label>Target:</label>
                    <%= Html.DropDownList("Target", (SelectList)ViewData["TargetList"], string.Empty, new { id = formId + "Target"})%>
                </div>
                
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="LibraryExternalSourceAssignedTo" runat="server" Text="<%$ Resources:LabelResource, LibraryExternalSourceAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
            
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="LibraryExternalSourceStatus" runat="server" Text="<%$ Resources:LabelResource, LibraryExternalSourceStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status"})%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>FrequencyReader">
                        <asp:Literal ID="LibraryExternalSourceFrequencyReader" runat="server" Text="<%$ Resources:LabelResource, LibraryExternalSourceFrequencyReader %>" />:</label>
                     <%= Html.DropDownList("FrequencyReader", (SelectList)ViewData["FrequencyReaderList"], string.Empty, new { id = formId + "FrequencyReader" })%>
                    <%= Html.ValidationMessage("FrequencyReader", "*")%>
                </div>
                <div class="field" id="EmailFrom" style="display:none">
                    <label for="<%= formId %>EmailFrom">Email from:</label>
                    <%= Html.DropDownList("EmailFrom", (SelectList)ViewData["EmailList"], string.Empty, new { id = formId + "EmailFrom" })%>
                    <%= Html.ValidationMessage("EmailFrom", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
