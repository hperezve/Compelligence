<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Source>" %>
<% string formId = ViewData["Scope"].ToString() + "SourceEditForm"; %>
<script type="text/javascript">
    var ResizeHeightForm = function() {
    var div = document.getElementById('ValidationSummarySource');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 58 - (10 * lis.length);
                var edt = $('#SourceEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };
</script>
<script type="text/javascript">

    var visibleLink = function() {
    var url = $('#<%=formId %>Url').prop('value');
        if (url != '') {
            $('#SourceShowLink').css("display", "block");
        }
    };
        $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        visibleLink();
        ResizeHeightForm();
        });
        
        var loadUrl = function() {
        var url = $('#<%=formId %>Url').prop('value');
            if (url != '') {
                if (url.indexOf("http://") == -1) {
                    url = "http://" + url;
                }
                window.open(url, "Url", "scrollbars=1,width=900,height=500")
            }
        };
</script>

<div id="ValidationSummarySource">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "SourceEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); ResizeHeightForm();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Source', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");visibleLink();}",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <% if (ViewData["UserSecurityAccess"].ToString().Equals(UserSecurityAccess.Edit))
           { %>
        <div class="buttonLink">
            <input class="button" type="submit" value="Save" />
            <input class="button" type="button" value="Reset" onclick="javascript: resetFormFields('#<%= formId %>');" />
            <input class="button" type="button" value="Cancel" onclick="javascript: cancelEntity('<%= ViewData["Scope"] %>', 'Source', '<%= ViewData["BrowseId"] %>', <%= ViewData["IsDetail"].ToString().ToLower() %>);" />
        </div>
        <% } %>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="SourceEditFormInternalContent" class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="SourceName" runat="server" Text="<%$ Resources:LabelResource, SourceName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" })%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="SourceAssignedTo" runat="server" Text="<%$ Resources:LabelResource, SourceAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="SourceType" runat="server" Text="<%$ Resources:LabelResource, SourceType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Url">
                        <asp:Literal ID="SourceUrl" runat="server" Text="<%$ Resources:LabelResource, SourceUrl %>" />:</label>
                    <%= Html.TextBox("Url", null, new { id = formId + "Url" })%>
                    <%= Html.ValidationMessage("Url", "*")%>
                    <div id="SourceShowLink" style="display: none">
                        <a href="javascript:void(0);" onclick="loadUrl();">Go To URL</a>
                    </div>
                </div>
                <div class="field">
                    <label for="<%= formId %>CreatedByFrm">
                        <asp:Literal ID="SourceCreatedByFrm" runat="server" Text="<%$ Resources:LabelResource, SourceOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByFrm", "*") %>
                </div>
            </div>
            <div class="line" >
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="SourceDescription" runat="server" Text="<%$ Resources:LabelResource, SourceDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="SourceMetaData" runat="server" Text="<%$ Resources:LabelResource, SourceMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
