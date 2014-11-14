<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Calendar>" %>
<% string formId = ViewData["Scope"].ToString() + "CalendarEditForm"; %>

<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
       <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
    <script type="text/javascript">
        var loadMiniHtmlEditor = function() {
            $('#<%=formId%>Description').cleditor();
            $(".cleditorMain iframe").contents().find('body').bind('keyup', function() {
                var v = $(this).text(); // or .html() if desired
                $('#<%=formId%>Description').html(v);
            });
        };

        $(function() {
            loadMiniHtmlEditor();

        });
        
      
    </script>
    
<script type="text/javascript">
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryCalendar');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var edt = $('#CalendarEditFormInternalContent');
                var tempo = 328 - 58 - (10 * lis.length);
                var edt = $('#CalendarEditFormInternalContent').css('height', tempo + 'px');
            }
        }
    };
    
    var EnableText = function() {
    var entityName = $('#EntityName');
    entityName.removeAttr("disabled");
    };
    
//    var GoToEntity = function(actionentity, entity, scope, entityid, browseid, content) {
//        alert('IIIIIIIIIIIIIIIIIIIII');
//        var uaction = '<%= Url.Action("' + actionentity + '", "' + entity + '") %>';
//        showEntity(uaction, scope, entity, entityid, browseid, content);
//    };
    var GoToEntity = function(entityid, entity, scope) {
        if (entity == 'Deal') {
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_DealContent');
            var uaction = '<%= Url.Action("Edit", "Deal") %>';
            var browseid = entity + 'All';
            var content = '#' + entity + 'Content';
            showEntity(uaction, scope, entity, entityid, browseid, content, '', '');
        }
        else if (entity == 'Event') {
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_EventContent');
            var uaction = '<%= Url.Action("Edit", "Event") %>';
            var browseid = entity + 'All';
            var content = '#' + entity + 'Content';
            showEntity(uaction, scope, entity, entityid, browseid, content, '', '');
        }
        else if (entity == 'Project') {
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_ProjectContent');
            var uaction = '<%= Url.Action("Edit", "Project") %>';
            var browseid = entity + 'All';
            var content = '#' + entity + 'Content';
            showEntity(uaction, scope, entity, entityid, browseid, content, '', '');
        }
        else if (entity == 'Kit') {

            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_KitContent');
            var uaction = '<%= Url.Action("Edit", "Kit") %>';
            var browseid = entity + 'All';
            var content = '#' + entity + 'Content';
            showEntity(uaction, scope, entity, entityid, browseid, content, '', '');
        }
        else if (entity == 'Objective') {
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            WorkspaceSubtabs.setActiveTab('WorkspaceTab_ObjectiveContent');
            var uaction = '<%= Url.Action("Edit", "Objective") %>';
            var browseid = entity + 'All';
            var content = '#' + entity + 'Content';
            showEntity(uaction, scope, entity, entityid, browseid, content, '', '');
        }
        else if (entity == 'Plan') {
            showEntityData(entityid, scope);
        }
    };
    </script>
    <script type="text/javascript">
        $(function() {
            initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>', ['dt:#<%= formId %>DateFrm']);
            ResizeHeightForm();
            EnableText();
        });
</script>

<div id="ValidationSummaryCalendar">
<%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "CalendarEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "', ['dt:#" + formId + "DateFrm']);loadMiniHtmlEditor();ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "','Calendar', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");}",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Calendar', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldName")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <div id="CalendarEditFormInternalContent" class="contentFormEdit resizeMe">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="CalendarName" runat="server" Text="<%$ Resources:LabelResource, CalendarName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name"})%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>Type">
                        <asp:Literal ID="CalendarType" runat="server" Text="<%$ Resources:LabelResource, CalendarType %>" />:</label>
                    <%= Html.DropDownList("Type", (SelectList)ViewData["TypeList"], string.Empty, new { id = formId + "Type" })%>
                    <%= Html.ValidationMessage("Type", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>DateFrm" class="required">
                        <asp:Literal ID="CalendarDate" runat="server" Text="<%$ Resources:LabelResource, CalendarDate %>" />:</label>
                    <%= Html.TextBox("DateFrm", null, new { id = formId + "DateFrm" })%>
                    <%= Html.ValidationMessage("DateFrm", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="CalendarAssignedTo" runat="server" Text="<%$ Resources:LabelResource, CalendarOwner %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%= Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>EntityName">
                    <asp:Literal ID="CalendarEntityName" runat="server" Text="<%$ Resources:LabelResource, CalendarEntityId %>" />:</label>
                    <%--<%= Html.TextBox("EntityName", null, new { @readonly = "readonly", onclick = "showEntity(" + ViewData["EntityLink"] + ");" })%>--%>
                    <%= Html.TextBox("EntityName", null, new { @readonly = "readonly", @enabled="true", onclick = "GoToEntity('" + ViewData["EntityLinkId"].ToString() + "','" + ViewData["EntityLinkEntity"].ToString() + "','" + ViewData["EntityLinkScope"].ToString() + "');" })%>
                    <%= Html.ValidationMessage("EntityName", "*") %>
                </div>
                <div class="field">
                    <label for="<%= formId %>Status">
                        <asp:Literal ID="CalendarStatus" runat="server" Text="<%$ Resources:LabelResource, CalendarStatus %>" />:</label>
                    <%= Html.DropDownList("Status", (SelectList)ViewData["StatusList"], string.Empty, new { id = formId + "Status" })%>
                    <%= Html.ValidationMessage("Status", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Description">
                        <asp:Literal ID="CalendarDescription" runat="server" Text="<%$ Resources:LabelResource, CalendarDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = formId + "Description" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>MetaData">
                        <asp:Literal ID="CalendarMetaData" runat="server" Text="<%$ Resources:LabelResource, CalendarMetaData %>" />:</label>
                    <%= Html.TextArea("MetaData", new { id = formId + "MetaData" })%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
