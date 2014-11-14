<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Library>" %>
<% string formId = ViewData["Scope"].ToString() + "LibraryNewsEditForm"; %>

<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>

<script type="text/javascript">
//    var EnableDropDownList = function() {
//    var entityName = $('#EntityType');
//        entityName.removeAttr("disabled");
//    };

    var createObject = function() {
        var entityName = $('#RelatedType');
        var Id = $('#Id');
        var LibraryId = $('#LibraryNewId');
        var xmlhttp;
        if (entityName[0].options[entityName[0].selectedIndex].value != '') {
            if (LibraryId[0].value != '') {
                var parameters = { entityid: LibraryId[0].value, entitytype: entityName[0].options[entityName[0].selectedIndex].value };
                //                $.get('<%= Url.Action("CreateNewObject","LibraryNews") %>', parameters);
                //                showCreateObjectDialog();
                $.get(
                '<%= Url.Action("CreateNewObjectByNews","LibraryNews") %>',
                parameters,
                function(data) {
                    if (data != null && data != '') {
                        results = data;
                        if (result != '' && result != undefined && result != null) {
                            GoToEntity(result, entityName[0].options[entityName[0].selectedIndex].value, entityName[0].options[entityName[0].selectedIndex].text, './' + entityName[0].options[entityName[0].selectedIndex].text + '.aspx/Edit');
                        }
                    }
                });                
            }
        }
    };

    function InitializaeClEditor() {
        $('#Description').cleditor({ height: 250, width: "200%" });
    };

    $(function() {
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        //initializeUploadField('<%= Url.Action("UploadFile", "LibraryNews") %>', '<%= ViewData["Scope"] %>', null, '#<%= formId %>FileNameLink', '#<%= formId %>FileNameResult', '#<%= formId %>FileName:#<%= formId %>FilePhysicalName', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');
        initializeUploadFileRawMode('<%= Url.Action("UploadFileRawMode", "LibraryNews") %>', '<%= ViewData["Scope"] %>', null, '#<%= formId %>FileNameLink', '#<%= formId %>FileNameResult', '#<%= formId %>FileName:#<%= formId %>FilePhysicalName', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');
        //        EnableDropDownList();
        InitializaeClEditor();
    });
    function GoDown(urlAction) {
        window.location = urlAction;
    }
</script>

<%= Html.ValidationSummary() %>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "LibraryNewsEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'LibraryNews', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + "); " +
           //"initializeUploadField('" + Url.Action("UploadFile", "Library") + "', '" + ViewData["Scope"] + "', null, '#" + formId + "FileNameLink', '#" + formId + "FileNameResult', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "'); }",
               "initializeUploadFileRawMode('" + Url.Action("UploadFileRawMode", "Library") + "', '" + ViewData["Scope"] + "', null, '#" + formId + "FileNameLink', '#" + formId + "FileNameResult', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "'); }",
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
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'LibraryNews', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
        </div>
        <%= Html.Hidden("Scope")%>
        <%= Html.Hidden("BrowseId")%>
        <%= Html.Hidden("IsDetail")%>
        <%= Html.Hidden("Container")%>
        <%= Html.Hidden("OperationStatus")%>
        <%= Html.Hidden("HeaderType")%>
        <%= Html.Hidden("DetailFilter")%>
        <%= Html.Hidden("OldDescription")%>
        <%= Html.Hidden("Id", default(decimal))%>
        <%= Html.Hidden("LibraryNewId", Model.Id)%>
        <div class="contentFormEdit">
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>Name" class="required">
                        <asp:Literal ID="LibraryName" runat="server" Text="<%$ Resources:LabelResource, LibraryName %>" />:</label>
                    <%= Html.TextBox("Name", null, new { id = formId + "Name"})%>
                    <%= Html.ValidationMessage("Name", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>LibraryTypeId">
                        <asp:Literal ID="LibraryLibraryTypeId" runat="server" Text="<%$ Resources:LabelResource, LibraryType %>" />:</label>
                    <%= Html.DropDownList("LibraryTypeId", (SelectList)ViewData["LibraryTypeList"], string.Empty, new { id = formId + "LibraryTypeId", onchange = "javascript: getContentData('" + Url.Action("GetDeletionDateByLibraryType", "Library") + "', getDropDownSelectedValue(this),'#" + formId + "DateDeletionFrm');" })%>
                    <%= Html.ValidationMessage("LibraryTypeId", "*")%>
                </div>
                <div class="field">
                    <label for="Author">
                        <asp:Literal ID="LibraryAuthor" runat="server" Text="<%$ Resources:LabelResource, LibraryAuthor %>" />:</label>
                    <%=Html.TextBox("Author")%>
                    <%=Html.ValidationMessage("Author", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="LibraryAssignedTo" runat="server" Text="<%$ Resources:LabelResource, LibraryAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>CreatedByFrm">
                        <asp:Literal ID="LibraryCreatedByFrm" runat="server" Text="<%$ Resources:LabelResource, LibraryOpenedBy %>" />:</label>
                    <%= Html.TextBox("CreatedByFrm", null, new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("CreatedByFrm", "*") %>
                </div>
                <div class="field">
                    <label for="Permanent">
                        <asp:Literal ID="LibraryPermanent" runat="server" Text="<%$ Resources:LabelResource, LibraryPermanent %>" />:</label>
                    <%= Html.DropDownList("Permanent", (SelectList)ViewData["PermanentList"], string.Empty)%>
                    <%= Html.ValidationMessage("Permanent", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="DateAddedFrm">
                        <asp:Literal ID="LibraryDateAdded" runat="server" Text="<%$ Resources:LabelResource, LibraryDateAdded %>" />:</label>
                    <%= Html.TextBox("DateAddedFrm", null, new { disabled = "disabled" })%>
                    <%= Html.ValidationMessage("DateAddedFrm", "*")%>
                </div>
                <div class="field">
                    <label for="DateDeletionFrm">
                        <asp:Literal ID="LibraryDateDeletion" runat="server" Text="<%$ Resources:LabelResource, LibraryDateDeletion %>" />:</label>
                    <%= Html.TextBox("DateDeletionFrm", null, new { id = formId + "DateDeletionFrm", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("DateDeletionFrm", "*")%>
                </div>
                <div class="field">
                    <label for="Related">
                        <asp:Literal ID="LibraryRelated" runat="server" Text="<%$ Resources:LabelResource, LibraryRelated %>" />:</label>
                    <%= Html.DropDownList("Related", (SelectList)ViewData["LibraryRelatedList"], string.Empty)%>
                    <%= Html.ValidationMessage("Related", "*")%>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="Score">
                        <asp:Literal ID="LibraryScore" runat="server" Text="<%$ Resources:LabelResource, LibraryScore %>" />:</label>
                    <%= Html.TextBox("Score")%>
                    <%= Html.ValidationMessage("Score", "*")%>
                </div>
               <%-- <div class="field">
                    <label for="Score">
                    <asp:Literal ID="LibraryScore" runat="server" Text="<%$ Resources:LabelResource, LibraryScore %>" />:</label>
                    <%= Html.TextBox("Score")%>
                    <%= Html.ValidationMessage("Score", "*"); %>
                </div>--%>
            </div>
            <div class="line">
                <div class="field textInColumn">
                    <a id="<%= formId %>FileNameLink"><asp:Literal runat="server" Text="<%$ Resources:LabelResource, LibraryUploadFile %>" /></a><%= Html.ValidationMessage("FileName", "*")%>
                    <p id="<%= formId %>FileNameResult">
                        <%= (Model != null) ? Model.FileName : string.Empty %></p>
                    <%
                        if ((Model != null) && (!string.IsNullOrEmpty(Model.FilePhysicalName)))
                            {
                    %>
                    <a href="<%= Url.Action("ShowFile", "Library") + "/" + Model.Id %>" onclick="javascript: openPopup(this.href); return false;"
                        title="See library file">
                        <img alt="See library file" src="<%= Url.Content("~/Content/Images/Icons/productSmall.png") %>" /></a>
                    <a href="javascript:void(0)" onclick="GoDown('<%= Url.Action("DownloadExecute", "Library") + "/" + Model.Id %>');"
                        title="Download library file">Download library file</a>
                    <% } %>
                    <%= Html.Hidden("FileName", null, new { id = formId + "FileName" })%>
                    <%= Html.Hidden("FilePhysicalName", null, new { id = formId + "FilePhysicalName" })%>
                </div>
                <div class="field">
                    <label for="Source">
                        <asp:Literal ID="LibrarySource" runat="server" Text="<%$ Resources:LabelResource, LibrarySource %>" />:</label>
                    <%= Html.TextBox("Source")%>
                    <%= Html.ValidationMessage("Source", "*")%>
                </div>
                <div class="field textInColumn">
                    <% if ((Model != null) && (!string.IsNullOrEmpty(Model.Link)))
                       {
                     %>
                     <a href="<%= Model.Link %>" target="_blank">View news</a>
                     <% } %>
                </div>
            </div>
            <div class="line">
                <div class="field">
                    <label for="RelatedType">
                        <asp:Literal ID="RelatedType" runat="server" Text="<%$ Resources:LabelResource, LibraryRelatedType %>" />:</label>
                    <%= Html.DropDownList("RelatedType", (SelectList)ViewData["NewsObjectTypeList"], string.Empty)%>
                    <%= Html.ValidationMessage("RelatedType", "*")%>
                    
                </div>
               <%-- 
                <% if (string.IsNullOrEmpty(ViewData["RelatedName"].ToString()))
                   {
                    %>--%>
                <div class="field textInColumn">
                    <a  href="javascript:void(0)"  onclick="createObject();">Create Object</a>
                    <%--<%= Html.ActionLink("Home", "Index", "BackEnd")%>--%>
                </div>
              <%--  <% }
                   else
                   { %>
                    <div class="field">
                    <label for="RelatedId">
                        <asp:Literal ID="RelatedId" runat="server" Text="<%$ Resources:LabelResource, LibraryRelatedId %>" />:</label>
                    <%= Html.TextBox("RelatedId", ViewData["RelatedName"].ToString(), new { @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("RelatedId", "*")%>
                </div>
                <% } %>--%>
            </div>
            
            <div class="line">
                <div class="field">
                    <label for="Description">
                        <asp:Literal ID="LibraryDescription" runat="server" Text="<%$ Resources:LabelResource, LibraryDescription %>" />:</label>
                    <%= Html.TextArea("Description", new { id = "Description", @class = "txtarealibrarynews" })%>
                    <%= Html.ValidationMessage("Description", "*")%>
                </div>
            </div>
        </div>
    </fieldset>
</div>
<% } %>
