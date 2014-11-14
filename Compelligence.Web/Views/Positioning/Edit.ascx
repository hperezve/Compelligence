<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Positioning>" %>
<% string formId = ViewData["Scope"].ToString() + "PositioningEditForm"; %>
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>" rel="stylesheet" type="text/css" />
    <%--<link href="<%= Url.Content("~/Content/Styles/Sticky/stickytooltip.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
<script type="text/javascript">
    var enabledFormCancelButton = function(formId) {
        $(formId + 'BtnCancel').removeAttr("disabled");
    };
    var loadMiniHtmlPositioningEdit = function() {
        $('#<%=formId%>HowWePosition').cleditor({ Id: 'HtmlEditorHowWePosition' });
        $("#HtmlEditorHowWePosition  iframe").contents().find('body').bind('keyup', function() {
            var v = $(this).text(); // or .html() if desired
            $('#<%=formId%>HowWePosition').html(v);
        });

        $('#<%=formId%>HowTheyPosition').cleditor({ Id: 'HtmlEditorHowTheyPosition' });
        $("#HtmlEditorHowTheyPosition  iframe").contents().find('body').bind('keyup', function() {
            var v = $(this).text(); // or .html() if desired
            $('#<%=formId%>HowTheyPosition').html(v);
        });


        $('#<%=formId%>HowWeAttack').cleditor({ Id: 'HtmlEditorHowWeAttack' });
        $("#HtmlEditorHowWeAttack  iframe").contents().find('body').bind('keyup', function() {
            var v = $(this).text(); // or .html() if desired
            $('#<%=formId%>HowWeAttack').html(v);
        });
    };
</script>

<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = $('#ValidationSummaryPositioning');
        var height = div.height();
        if (height > 0) {
            var newHeigth = 328 - height;
            var edt = $('#PositioningEditFormInternalContent').css('height', newHeigth + 'px');
        }
    };
    $(function() {
        loadMiniHtmlPositioningEdit();
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        ResizeHeightForm();
        enabledFormCancelButton('#<%= formId %>');
    });
        
</script>
<div id="ValidationSummaryPositioning">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "PositioningEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); loadMiniHtmlPositioningEdit(); ResizeHeightForm();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Positioning', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + "); enabledFormCancelButton('#" + formId + "');}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.CancelDetail, new { id = formId + "BtnCancel", onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Positioning', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilterLocal", (string)ViewData["DetailFilter"], new { id = formId + "DetailFilter" })%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldIsMaster")%>
        <%= Html.Hidden("OldIndustriesIds")%>
        <%= Html.Hidden("IsGlobal")%>
        <%= Html.Hidden("IsCompetitorCompany")%>         
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="PositioningEditFormInternalContent" class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="PositioningName" runat="server" Text="<%$ Resources:LabelResource, PositioningName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name" ,style = "width:1120px;"})%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CreatedByName">
                        <asp:Literal ID="PositioningCreatedByName" runat="server" Text="<%$ Resources:LabelResource, PositioningCreatedBy %>" />:</label>
                    <% string openendByName = ViewData["OpenedByName"].ToString(); %>
                    <%= Html.TextBox("CreatedByName", openendByName, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByName", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status" class="required">
                        <asp:Literal ID="PositioningStatus" runat="server" Text="<%$ Resources:LabelResource, PositioningStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <% if (ViewData["IsCompetitorCompany"].ToString().Equals("Y"))
               { %>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>HowWePosition">
                        <asp:Literal ID="PositioningHowWePosition" runat="server" Text="<%$ Resources:LabelResource, PositioningHowWePosition %>" />:</label>
                    <%= Html.TextArea("HowWePosition", new { id = formId + "HowWePosition", style = "width:1120px;" })%>
                    <%= Html.ValidationMessage("HowWePosition", "*")%>
                </div>
            </div>
            <%}
               else
               { %>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>HowTheyPosition">
                        <asp:Literal ID="PositioningHowTheyPosition" runat="server" Text="<%$ Resources:LabelResource, PositioningHowTheyPosition %>" />:</label>
                    <%= Html.TextArea("HowTheyPosition", new { id = formId + "HowTheyPosition", style = "width:1120px;" })%>
                    <%= Html.ValidationMessage("HowTheyPosition", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>HowWeAttack">
                        <asp:Literal ID="PositioningHowWeAttack" runat="server" Text="<%$ Resources:LabelResource, PositioningHowWeAttack %>" />:</label>
                    <%= Html.TextArea("HowWeAttack", new { id = formId + "HowWeAttack", style = "width:1120px;" })%>
                    <%= Html.ValidationMessage("HowWeAttack", "*")%>
                </div>
            </div>
            <%} %>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>IndustryIds">
                        <asp:Literal ID="PositioningIndustryIds" runat="server" Text="<%$ Resources:LabelResource, PositioningIndustryId %>" />:</label>
                    <%= Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdList"], new { id = formId + "IndustryIds", Multiple = "true", style = "height:100px;" })%>
                    <%= Html.ValidationMessage("IndustryIds", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
