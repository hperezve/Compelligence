<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Positioning>" %>
<% string formId = ViewData["Scope"].ToString() + "CompetitiveMessagingEditForm"; %>
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>" rel="stylesheet" type="text/css" />
    <%--<link href="<%= Url.Content("~/Content/Styles/Sticky/stickytooltip.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />


    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.8.12.custom.min.js") %>" type="text/javascript"></script>--%>
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui/jquery.ui.widget.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui/jquery.ui.core.js") %>" type="text/javascript"></script>--%>
    
    <%--<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>--%>

    <%--<script src="<%= Url.Content("~/Scripts/System/BackEnd/Utility.js") %>" type="text/javascript"></script>--%>
    <%--<script src="<%= Url.Content("~/Scripts/jqgrid/ui.multiselect.js") %>" type="text/javascript"></script>--%>
    <%--<script src="<%= Url.Content("~/Scripts/jquery.multiselect.1.3.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>" type="text/javascript"></script>--%>
     <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
<script type="text/javascript">
    var enabledFormCancelButton = function(formId) {
        $(formId + 'BtnCancel').removeAttr("disabled");
    };
//    var SetValue = function() {
//        var checkbox = $('#<%= formId %>IsMasterPositioning');
//        if (checkbox[0].checked != undefined) {
//            if (checkbox[0].checked) {
//                //                $("#IndustryId").get(0).selectedIndex = 0;
//                //                $("#IndustryId").selectedIndex = 0;
//                $("#IsMasterPositioningValue")[0].value = "true";
//                $("#IndustryIdDiv").css("display", "none");
//            }
//            else {
//                $("#IndustryIdDiv").css("display", "block");
//                $("#IsMasterPositioningValue")[0].value = "false";
//            }
//        }

//    };

//    var resetEntityType = function(select, formid) {
//        var entityType = $("#EntityType");
//        if ($('#' + formid + 'EntityType')[0].selectedIndex != 0) {
//            $('#' + formid + 'EntityType')[0].selectedIndex = 0;
//            getCascadeObjects('/Positioning.aspx/GetUpdatePositioningEntityId', '#' + formid + ' [name="EntityType"]', '#' + formid + ' [name="EntityId"]', '#' + formid + 'EntityIdLoader', null, null);
//        }
    //    };
    var loadMiniHtmlCMEdit = function() {
        $('#<%=formId%>HowTheyAttack').cleditor();
        $('#<%=formId%>HowWeDefend').cleditor();
    };
</script>

<script type="text/javascript">
    $(function() {
    loadMiniHtmlCMEdit();
    ResizeHeightForm('#<%= ViewData["Scope"] %>CompetitveMessagingList');
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        enabledFormCancelButton('#<%= formId %>');
    });
    var ResizeHeightForm = function() {
        var div = $('#ValidationSummaryCompetitiveMessaging');
        var height = div.height();
        if (height > 0) {
            var newHeigth = 328 - height;
            var edt = $('#CompetitiveMessaginEditFormInternalContent').css('height', newHeigth + 'px');
        }
    };   
</script>
<div id="ValidationSummaryCompetitiveMessaging">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CompetitiveMessagingEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); loadMiniHtmlCMEdit();ResizeHeightForm();executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'CompetitiveMessaging', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");enabledFormCancelButton('#" + formId + "');}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.CancelDetail, new { id = formId + "BtnCancel", onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'CompetitiveMessaging', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
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
        <%= Html.Hidden("OldCompetitorsIds")%>
        <%= Html.Hidden("IsGlobal")%>
        <%-- <%= Html.Hidden("EntityId", (string)ViewData["EntityId"], new { id = formId + "EntityId" })%>
        <%= Html.Hidden("EntityType", (string)ViewData["EntityType"], new { id = formId + "EntityType" })%>
        <%= Html.Hidden("IndustryId", (string)ViewData["IndustryId"], new { id = formId + "IndustryId" })%>--%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="CompetitiveMessaginEditFormInternalContent" class="contentFormEdit">
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
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>HowTheyAttack">
                        <asp:Literal ID="PositioningHowTheyAttack" runat="server" Text="<%$ Resources:LabelResource, PositioningHowTheyAttack %>" />:</label>
                    <%= Html.TextArea("HowTheyAttack", new { id = formId + "HowTheyAttack", style = "width:1120px;" })%>
                    <%= Html.ValidationMessage("HowTheyAttack", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>HowWeDefend">
                        <asp:Literal ID="PositioningHowWeDefend" runat="server" Text="<%$ Resources:LabelResource, PositioningHowWeDefend %>" />:</label>
                    <%= Html.TextArea("HowWeDefend", new { id = formId + "HowWeDefend", style = "width:1120px;" })%>
                    <%= Html.ValidationMessage("HowWeDefend", "*")%>
                </div>
            </div>
            
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>CompetitorId">
                        <asp:Literal ID="PositioningCompetitorId" runat="server" Text="<%$ Resources:LabelResource, PositioningCompetitorId %>" />:</label>
                    <%--<%= Html.DropDownList("CompetitorId", (MultiSelectList)ViewData["CompetitorIdList"], string.Empty, new { id = formId + "CompetitorId", multiple = "multiple", style = "height:100px;" })%>--%>
                    <%--<%= Html.ListBox("CompetitorId", (MultiSelectList)ViewData["CompetitorIdList"], new { Multiple = "true", style = "height:100px;" })%>--%>
                    <%= Html.ListBox("CompetitorIds", (MultiSelectList)ViewData["CompetitorIdList"], new { id = formId + "CompetitorIds", Multiple = "true", style = "height:100px;" })%>
                    
                </div>
                <div class="field">
                    <label for="<%= formId %>IndustryId">
                        <asp:Literal ID="PositioningIndustryId" runat="server" Text="<%$ Resources:LabelResource, PositioningIndustryId %>" />:</label>
                    <%--<%= Html.DropDownList("IndustryId", (MultiSelectList)ViewData["IndustryIdList"], string.Empty, new { id = formId + "IndustryId", multiple = "multiple", style = "height:100px;" })%>--%>
                 <%--<%= Html.ListBox("IndustryId", (MultiSelectList)ViewData["IndustryIdList"], new { Multiple = "true", style = "height:100px;" })%>--%>
                    <%= Html.ListBox("IndustryIds", (MultiSelectList)ViewData["IndustryIdList"], new { id = formId + "IndustryIds", Multiple = "true", style = "height:100px;" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
