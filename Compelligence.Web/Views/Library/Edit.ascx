<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Library>" %>
<% string formId = ViewData["Scope"].ToString() + "LibraryEditForm";
   decimal modelValue = 0;
   if (Model != null)
   {
       modelValue = Model.Id;
   }%>
<link href="<%= Url.Content("~/Content/Styles/fileuploader.css") %>" rel="stylesheet" type="text/css" />
<style>
    .newsright
    {
        float:left;
    }
    .newsleft
    {
        float: left;
        width: 620px;
    }
    .librarytools
    {
        float : rigtht; 
        display: inline-block;
        height: 20px;
        width: 220px;
    }
    .newshidden
    {
         display:none;
    }
    .newstoolslabel
    {
        float : left; 
        font-size: x-small;   
        font-weight: bold;    
        margin-left: 3px;
        margin-right: 3px;
    }
    .newstoolsobject
    {
        float: left;
        margin-left: 3px;
        margin-right: 3px;
    }
    .libraryupload
    {
        float : left; 
        height: 16px;
        margin-left: 3px;
        margin-right: 3px;
        width: 16px;
        background: url(<%= ResolveUrl("~/Content/Images/Icons/library-upload.png") %>) repeat scroll left top transparent;
    }

    .miDiv ul 
    {
       list-style-position   :inside;
       list-style-type:disc;
    }
      .miDiv ol 
    {
	   list-style-type: decimal ;
       list-style-position   :inside;
 
    }
 
</style>
<script type="text/javascript">
    var createObject = function() {
        var entityName = $('#RelatedType');
        var Id = $('#LibraryIdHidden');
        var xmlhttp;
        if (entityName[0].options[entityName[0].selectedIndex].value != '') {
            if (Id[0].value != '') {
                var parameters = { entityid: Id[0].value, entitytype: entityName[0].options[entityName[0].selectedIndex].value };
                $.get(
                '<%= Url.Action("CreateNewObjectByNews","Library") %>',
                parameters,
                function(data) {
                if (data != null && data != '') {
                    var results = data;
                    if (result != '' && result != undefined && result != null) {
                        GoToEntity(result, entityName[0].options[entityName[0].selectedIndex].value, entityName[0].options[entityName[0].selectedIndex].text, './' + entityName[0].options[entityName[0].selectedIndex].text + '.aspx/Edit');
                    }
                 }
                });
            }
        }
    };
    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryLibrary');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis.length > 0) {
                var newHeigth = 328 - 70 - (10 * lis.length);
                var edt = $('#LibraryEditFormInternalContent').css('height', newHeigth + 'px');
            }
        }
    };

    var createDialogImport = function() {
        if ($('#DownloadRemoteDlg').length) {}
        else {
            var url = '<%= Url.Action("DownloadRemote", "Library")%>';
            var cajaImport = $('<div id="DownloadRemoteDlg" style="display:none"><input type="text" id="RemoteFile" value="http://" style="width:480px" /><input type="button" value="Import" Id="DownloadRemoteDlgImport"/></div>');
            $('#LibraryEditFormInternalContent').append(cajaImport);

            var errorMessage = '<div id="messageerror"  class="newshidden" ></br><p style="color: red;margin-left:3px;font-family:Verdana,Arial,sans-serif;font-size=0.8em" >Data not uploaded correctly from URL.</p></div>';
            $('#DownloadRemoteDlg').append(errorMessage);
            $('#DownloadRemoteDlgImport').click(function() {
                DownloadRemote('<%= Url.Action("DownloadRemote", "Library")%>', '#RemoteFile');
            });
        }
    };
 
    $(function() {
        createDialogImport();
        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        initializeUploadField('<%= Url.Action("UploadFile", "Library") %>',
             '<%= ViewData["Scope"] %>',
              null,
               '#<%= formId %>FileNameLink',
               '#<%= formId %>FileNameResult',
                '#<%= formId %>FileName:#<%= formId %>FilePhysicalName',
                 '<%= ViewData["Container"] %>',
                  '<%= ViewData["HeaderType"] %>',
                   '<%= ViewData["DetailFilter"] %>', false);

        var foId = '<%= formId %>';
        var textDesc = $('textarea#<%= formId %>Description').val();
        textDesc = textDesc.replace(/<a/g, "<a target='_blank'");
        $('textarea#<%= formId %>Description').val(textDesc);
        $("#contentDesc").html(textDesc);
        ResizeHeightForm();
    });

    var downloadFile2 = function(urlDownloadAction) {
        $.get(urlDownloadAction, { chk: 'true' }, function(data) {
            if (data.toString().indexOf("NotFound") > -1) {

                $('#DownloadFileNotFound').dialog({
                    bgiframe: true,
                    autoOpen: false,
                    modal: true,
                    buttons: {
                        Ok: function() {
                            $(this).dialog('close');
                        }
                    }
                });
                $('#DownloadFileNotFound').dialog('open');

            } else {
            $('#DownloadFileSection').prop("src", urlDownloadAction);
                //window.location = urlDownloadAction;
            }
        });

        return false;
    };    
    
    function GoDown(urlAction) {
        downloadFile2(urlAction);
    }

    function editDescription() {
        $('#designDesc').addClass('newshidden');
        $('#editDesc').removeClass('newshidden');
    }
    function viewDesign() {
        $('#editDesc').addClass('newshidden');
        $('#designDesc').removeClass('newshidden');
        var textDesc = $('textarea#<%= formId %>Description').val();
        $("#contentDesc").html(textDesc);
    }
    function updateViewDesc() {
        var textDesc = $('textarea#<%= formId %>Description').val();
        $("#contentDesc").html(textDesc);
    }

    /*********************************************/
    function DownloadRemoteDlg() {
        //showLoadingDialog();
        var dlgObject = $("#DownloadRemoteDlg");
        //dlgObject.empty();
        dlgObject.dialog(
          {
              bgiframe: true,
              autoOpen: false,
              title: "Enter URL",
              height: 150,
              width: 600,
              buttons:
             {
                 "Close": function() { $(this).dialog("destroy"); }
             }
          });
          $("#RemoteFile").prop('value', 'http://');
          $('#messageerror').addClass('newshidden');
          //quizObject.html(url, {}, function() {
          dlgObject.dialog("open");
         
        //hideLoadingDialog();

    }
    function DownloadRemote(urlAction, remotefilename) 
    {
        var filename = '<%= formId + "FileName"%>';
        var filephysicalname = '<%= formId + "FilePhysicalName"%>';
        $.getJSON(urlAction, { remotefilename: $(remotefilename).val() }, function(data) {
            $('#<%= formId %>FileNameResult').text(data.FileName);
            $("#" + filename).val(data.FileName);
            $("#" + filephysicalname).val(data.FilePhysicalName);
            if (data.Result == 'Fail') {
                $('#messageerror').removeClass('newshidden');
            }
            else {
                $("#DownloadRemoteDlg").dialog("destroy");
                $('#messageerror').addClass('newshidden');
                var Message = '<p>Your document was successfully uploaded.</p>';
                $('#AlertReturnMessageDialog').html(Message);
                $('#AlertReturnMessageDialog').dialog('open');
            }
        });
    }
    var donwloadTemplate = function(urlCheck, urlAction) {

    var FromId;
    var tipeAction = '<%= ViewData["ActionMethod"] %>';
    var xmlhttp;
    var results = null;
    var MySelect = $('#IndustryId');
    var MySelect1 = $('#chkValue:checked');
    if (tipeAction == 'Edit') {
        FromId = '<%= modelValue %>';
    }
    else {
        FromId = $('#Id').val();
    }
    var parametro = { IndustryId: MySelect.val(), chkValue: MySelect1.val(), FromIdCheck: FromId };
    $("#ajaxUploadForm").block({ message: '<h1> Download a Template:...</h1>' });
            $.get(
            urlCheck,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        $("#ajaxUploadForm").unblock();
                        var newResult = results.replace('"', '');
                        newResult = newResult.replace('"', '');
                        if (newResult == 'Exist') {
                            $('#DownloadFileSection').prop("src", urlAction);
                        }
                        else {
                            //alert(results);
                            var errorMessage = '<p>' + results + '</p>';

                            $('#AlertReturnMessageDialog').html(errorMessage);
                            //$('#AlertReturnMessageDialog').dialog('open');
                            //                        $('#AlertReturnMessageDialog').dialog({ autoOpen: false, open: function() {
                            //                        $('#AlertReturnMessageDialog').dialog("option", "title", "File does not exist");
                            //                        }
                            //                        });
                            $('#AlertReturnMessageDialog').dialog('open');
                        }
                    }
                }
            });
    };
    var SetOnclick = function() {
    
    var buttonTypeFile = $('#FileNameLink');
        buttonTypeFile.click();
    };
    $(document).ready(function() {
        setLinkTarget('contentDesc');
    });
</script>
<div id="ValidationSummaryLibrary">
    <%= Html.ValidationSummary()%>
</div>

<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "LibraryEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'Library', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + "); updateViewDesc(); createDialogImport();" +
               "initializeUploadFileRawMode('" + Url.Action("UploadFileRawMode", "Library") + "', '" + ViewData["Scope"] + "', null, '#" + formId + "FileNameLink', '#" + formId + "FileNameResult', '#" + formId + "FileName:#" + formId + "FilePhysicalName:', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "'); }",
               OnFailure = "showFailedResponseDialog"
           },
           new { id = formId }))
   { %>
<div class="indexTwo">
    <fieldset>
        <legend>
            <%= Html.Encode(ViewData["FormHeaderMessage"])%></legend>
        <div class="line">
            <div class="buttonLink">
                <div class="field" style="width:auto; padding-right:0px;margin-left:0px;">
                    <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
                    <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
                    <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'Library', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>                        
                    <input class="button" type="button" value="Create" onclick="createObject();"/>
                        <% if ((Model != null) && (!string.IsNullOrEmpty(Model.Link)))
                           {%>                     
                                <a href="<%= Model.Link %>" target="_blank"><input class="button" type="button" value="View" /></a>
                        <% } %>
                </div>
                <div class="field">
                    <div class="librarytools">
                        <label class="newstoolslabel">Options:</label>
                        <img class="library-toolbox newstoolsobject" src="<%=Url.Content("~/Content/Images/Icons/library-view.png") %>" title="See File" onclick="javascript: openPopup('<%= Url.Action("ShowFile", "Library") + "/" + (Model==null? -1:Model.Id) %>'); return false;" />
                        <img class="library-toolbox newstoolsobject" src="<%=Url.Content("~/Content/Images/Icons/library-remote.png") %>" title='Upload from URL' onclick="DownloadRemoteDlg();" />
                       <% if ((Model != null) && (!string.IsNullOrEmpty(Model.FilePhysicalName))) { %>            
                            <img class="library-toolbox" src="<%=Url.Content("~/Content/Images/Icons/library-download.png") %>" onclick="GoDown('<%= Url.Action("DownloadExecute", "Library") + "/" + Model.Id %>');return false;"  title="Download library file" />
                       <% } %>
                        <%= Html.Hidden("FileName", null, new { id = formId + "FileName" })%>
                        <%= Html.Hidden("FilePhysicalName", null, new { id = formId + "FilePhysicalName" })%>
                        <div id="<%= formId %>FileNameLink" class="libraryupload" >
                            <noscript>
                                <label for="<%= formId %>FileNameLink" class="required">
                                    <asp:Literal ID="MessageFileNameLink" runat="server" Text="<%$ Resources:LabelResource, MessageFileNameLink %>" />
                                </label>
                            </noscript>
                        </div>
                    </div>
                </div>
            </div>
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
        <div id="LibraryEditFormInternalContent"   class="contentFormEdit">
        <div class="newsleft">
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
             </div>
             <div class="line">
                <div class="field">
                    <label for="Author">
                        <asp:Literal ID="LibraryAuthor" runat="server" Text="<%$ Resources:LabelResource, LibraryAuthor %>" />:</label>
                    <%=Html.TextBox("Author")%>
                    <%=Html.ValidationMessage("Author", "*")%>
                </div>
                <div class="field">
                    <label for="<%= formId %>AssignedTo" class="required">
                        <asp:Literal ID="LibraryAssignedTo" runat="server" Text="<%$ Resources:LabelResource, LibraryAssignedTo %>" />:</label>
                    <%= Html.DropDownList("AssignedTo", (SelectList)ViewData["AssignedToList"], string.Empty, new { id = formId + "AssignedTo" })%>
                    <%=Html.ValidationMessage("AssignedTo", "*")%>
                </div>   
            </div>
            <div class="line">
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
            </div> 
            <div class="line">
                <div class="field">
                    <label for="Score">
                        <asp:Literal ID="LibraryScore" runat="server" Text="<%$ Resources:LabelResource, LibraryScore %>" />:</label>
                    <%= Html.TextBox("Score")%>
                    <%= Html.ValidationMessage("Score", "*")%>
                </div>  
                <div class="field">
                    <label for="Source">
                        <asp:Literal ID="LibrarySource" runat="server" Text="<%$ Resources:LabelResource, LibrarySource %>" />:</label>
                    <%= Html.TextBox("Source")%>
                    <%= Html.ValidationMessage("Source", "*")%>
                </div>        
            </div>
            <div class="line">
                <div class="field">
                    <label for="RelatedType">
                        <asp:Literal ID="RelatedType" runat="server" Text="<%$ Resources:LabelResource, LibraryRelatedType %>" />:</label>
                    <%= Html.DropDownList("RelatedType", (SelectList)ViewData["NewsObjectTypeList"], string.Empty)%>
                    <%= Html.ValidationMessage("RelatedType", "*")%>
                    <%= Html.Hidden("LibraryIdHidden",ViewData["LibraryIdHidden"]) %>
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
                    <label  for="Notes"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:LabelResource, LibraryNotes %>" />:</label>
                    <%= Html.TextArea("Notes", null, new { id = formId + "Notes", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("Notes", "*")%>
                </div>
            </div> 
            <div class="line">
                <div class="field">
                    <label for="SubmittedViaFrm"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:LabelResource, LibrarySubmittedVia %>" />:</label>
                    <%= Html.TextArea("SubmittedViaFrm", null, new { id = formId + "SubmittedViaFrm", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("SubmittedViaFrm", "*")%>
                </div>
            </div>                      
            <div class="line">
                <div class="field">
                    <label for="FileName">Filename:</label>
                    <%= Html.ValidationMessage("FileName", "*")%>  	        
                    <p id="<%= formId %>FileNameResult"><%= (Model != null) ? Model.FileName : string.Empty %></p>
                    <input class="button" type="button" value="Download" onclick="javascript:donwloadTemplate('<%= Url.Action("CheckExistFile", "Library") %>','<%= Url.Action("CheckOut", "Library") %>')" />       
                </div>
            </div>
            <% if (!string.IsNullOrEmpty(ViewData["FileList"].ToString()))
               {  %>
                <div class="line">
                <div class="field">
                    <label for="FileAttachments">File attachments:</label>
                </div>
                <div class="field">
                    <label for="OptionsFile">Options:</label>
                </div>
               </div>
                <% foreach (File file in (List<File>)ViewData["FileList"])
                   {%>
                   <div class="line">
                 
                   <div class="field">
                   <%= file.FileName %> 
                   </div>
                <div class="field">
                   <img class="library-toolbox" src="<%=Url.Content("~/Content/Images/Icons/library-view.png") %>" title="See File" onclick="javascript: openPopup('<%= Url.Action("ShowFileById", "Library") + "/" + file.Id %>'); return false;" /> 
                   <img class="library-toolbox" src="<%=Url.Content("~/Content/Images/Icons/library-download.png") %>" onclick="GoDown('<%= Url.Action("DownloadFileMailExecute", "Library") + "/" + file.Id %>');return false;"  title="Download library file"/>
                   </div>
                   </div>
                <% } %>
                <%}
             %>
            </div>
           <div id="editDesc" class="newshidden newsright" >
                <table>
                    <tr>
                        <td style="width:60px;">
                            <div>
                                <label for="Description">
                                    <asp:Literal ID="LibraryDescription" runat="server" Text="<%$ Resources:LabelResource, LibraryDescription %>" />:</label>
                            </div>
                        </td>
                        <td>
                            <div>
                                <input class="button" type="button" value="View" id="Text1"  onclick="javascript:viewDesign();"/>                                               
                            </div>
                        </td>
                    </tr>
                </table>
                <%= Html.TextArea("Description", new { id = formId + "Description", style = "width:560px;height:227px" })%>
                <%= Html.ValidationMessage("Description", "*")%>
           </div>            
           <div id="designDesc" class="newsright" >
                <table>
                    <tr>
                        <td style="width:60px;">
                            <label for="Description">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, LibraryDescription %>" />:</label>
                        </td>
                            <td>                                                
                                <input class="button" type="button" value="Edit" id="ButtonEditDesc" title="Edit Description"  onclick="javascript:editDescription();"/>                                               
                            </td>
                     </tr>
                </table>   
                <div id="contentDesc"  class ="miDiv" style="border: 1px solid #6495C1; width:560px; height:227px; overflow:auto; background-color:White;overflow-y:scroll;">
                </div>
            </div>
           <br />              
       </div>
    </fieldset>
</div>

<% } %>

<div id="DownloadFileNotFound" title="Error" style ="display:none">
    <p>
        <span class="ui-icon ui-icon-alert"></span><strong>File not found...!</strong>
    </p>
</div>                  
          