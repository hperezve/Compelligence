<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.Library>" %>
<% string formId = ViewData["Scope"].ToString() + "NewsEditForm"; %>
<link href="<%= Url.Content("~/Content/Styles/fileuploader.css") %>" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
<script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
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
    .newstools
    {
        height: 20px;
        width: 520px;      
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
</style>
<script type="text/javascript">
    var createObject = function() {
        var container = '<%= ViewData["Container"]%>';
        showLoadingDialogForSection(container);
        var entityName = $('#RelatedType');
        var Id = $('#LibraryIdHidden');
        var link = $('#Link').val();
        var xmlhttp;
        if (entityName[0].options[entityName[0].selectedIndex].value != '') {
            if (Id[0].value != '') {
                var parameters = { entityid: Id[0].value, entitytype: entityName[0].options[entityName[0].selectedIndex].value, link: link };
                $.get(
                '<%= Url.Action("CreateNewObjectByNews","Library") %>',
                parameters,
                function(data) {
                    if (data != null && data != '') {
                        result = data;
                        if (result != '' && result != undefined && result != null) {
                            GoToEntity(result, entityName[0].options[entityName[0].selectedIndex].value, entityName[0].options[entityName[0].selectedIndex].text, './' + entityName[0].options[entityName[0].selectedIndex].text + '.aspx/Edit');
                        }
                    }
                });                
            }
        }
        hideLoadingDialog();
    };

    var ResizeHeightForm = function() {
        var div = document.getElementById('ValidationSummaryNews');
        var ul = div.getElementsByTagName('ul');
        if ((ul != null) || (ul != undefined)) {
            var lis = div.getElementsByTagName('li');
            if (lis != null && lis != undefined) {
                if (lis.length != null && lis.length != undefined && lis.length > 0) {
                    var newHeigth = 328 - 70 - (10 * lis.length);
                    var edt = $('#NewsEditFormInternalContent').css('height', newHeigth + 'px');
                }
            }
        }
    };
    function viewDesign() {
        $('#DescriptionDiv').css("display", "none");
        $('#DescriptionViewDiv').removeClass('newshidden');
        setLinkTarget('DescriptionViewDiv');
    }
    function editDescription() {
      
        $('#DescriptionViewDiv').addClass('newshidden');
        $('#DescriptionDiv').css("display", "block");
        
        loadMiniHtmlEditor();
    }
    var loadMiniHtmlEditor = function() {

        $('#<%=formId%>Description').cleditor();
        $(".cleditorMain iframe").contents().find('body').bind('keyup', function() {
            var v = $(this).text(); // or .html() if desired
            $('#<%=formId%>Description').html(v);
        });
        var MySelect = $('#DescriptionView');
        MySelect.val = $('#<%=formId%>Description').val();

        var cledit = $('#<%=formId%>Description').cleditor()[0];
        $(cledit.$frame[0]).prop("id", "cleditCool");
        var cleditFrame;
        if (!document.frames) {
            cleditFrame = $("#cleditCool")[0].contentWindow.document;
        }
        else {
            cleditFrame = document.frames["cleditCool"].document;
        }
        $(cleditFrame).bind('keyup', function() {
            var v = $(this).find("body").html();
            $('#DescriptionView').html(v);
        });
        $("div.cleditorToolbar").bind("click", function() {
            var v = $(this).find("body").html();
            $('#DescriptionView').html(v);
        });
    };

    var setLibraryType = function() {
        var actionMethod = '<%= ViewData["ActionMethod"] %>';
        if (actionMethod == 'Create') {
            var options = $('#' + '<%= formId %>' + 'LibraryTypeId').prop("options");
            var value;
            if (options.length > 0) {
                for (var i = 0; i < options.length; i++) {
                    if (options[i].text == "News") {
                        options[i].selected = true;
                        value = options[i].value;
                    }
                }
            }
            getContentData('<%= Url.Action("GetDeletionDateByLibraryType", "Library")%>', value, '#' + '<%= formId %>' + 'DateDeletionFrm');
            $('#' + '<%= formId %>' + 'LibraryTypeId').prop("disabled", true)
        }
    };

    var loadUrl = function() {
    var url = $('#Link').prop('value');
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "Url", "scrollbars=1,width=900,height=500")
        }
    };

    var createDialogImport = function() {
        if ($('#DownloadRemoteDlg').length) { }
        else {
            var url = '<%= Url.Action("DownloadRemote", "Library")%>';
            var cajaImport = $('<div id="DownloadRemoteDlg" style="display:none"><input type="text" id="RemoteFile" value="http://" style="width:480px" /><input type="button" value="Import" Id="DownloadRemoteDlgImport"/></div>');
            $('#NewsEditFormInternalContent').append(cajaImport);
            $('#DownloadRemoteDlgImport').click(function() {
                DownloadRemote('<%= Url.Action("DownloadRemote", "Library")%>', '#RemoteFile');
            });
        }
    };
    var InitializeDescription = function() {
        var textDesc = $('#<%=formId%>Description').val();
        $("#DescriptionView").html(textDesc);
        $(".oculto").hide();     
    };
    $(function() {
        createDialogImport();

        initializeForm('#<%= formId %>', '<%= ViewData["UserSecurityAccess"] %>', '<%= ViewData["EntityLocked"] %>');
        initializeUploadFileRawMode('<%= Url.Action("UploadFileRawMode", "News") %>', '<%= ViewData["Scope"] %>', null, '#<%= formId %>FileNameLink', '#<%= formId %>FileNameResult', '#<%= formId %>FileName:#<%= formId %>FilePhysicalName', '<%= ViewData["Container"] %>', '<%= ViewData["HeaderType"] %>', '<%= ViewData["DetailFilter"] %>');
        ResizeHeightForm();
        setLibraryType();
      
        var textDesc = $('#<%=formId%>Description').val();
        $("#DescriptionView").html(textDesc);
        loadMiniHtmlEditor();
        $(".oculto").hide();              
         
    });

    function GoDown(urlAction) {
        window.location = urlAction;
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

        dlgObject.dialog("open");
    }
    function DownloadRemote(urlAction, remotefilename) {
        var filename = '<%= formId + "FileName"%>';
        var filephysicalname = '<%= formId + "FilePhysicalName"%>';
        $.getJSON(urlAction, { remotefilename: $(remotefilename).val() }, function(data) {
            $('#<%= formId %>FileNameResult').text(data.FileName);
            $("#" + filename).val(data.FileName);
            $("#" + filephysicalname).val(data.FilePhysicalName);
            $("#DownloadRemoteDlg").dialog("destroy");
        });
    }
    $(document).ready(function() {
    //$('#DescriptionViewDiv a').attr('target', '_blank');
    setLinkTarget('DescriptionViewDiv');
    });
</script>
  
<div id="ValidationSummaryNews">
    <%= Html.ValidationSummary()%>
</div>
<% using (Ajax.BeginForm((string)ViewData["ActionMethod"], null,
           new AjaxOptions
           {
               HttpMethod = "POST",
               UpdateTargetId = (string)ViewData["Scope"] + "NewsEditFormContent",
               OnBegin = "function(){ showLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnComplete = "function(){ hideLoadingDialogForSection('" + ViewData["Container"] + "'); }",
               OnSuccess = "function(){ initializeForm('#" + formId + "', '" + ViewData["UserSecurityAccess"] + "', '" + ViewData["EntityLocked"] + "'); loadMiniHtmlEditor(); InitializeDescription();ResizeHeightForm(); executePostActions('#" + formId + "', '" + ViewData["Scope"] + "', 'News', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + "); initializeUploadField('" + Url.Action("UploadFile", "News") + "', '" + ViewData["Scope"] + "', null, '#" + formId + "FileNameLink', '#" + formId + "FileNameResult', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "'); setLibraryType();createDialogImport();}",
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
                <div class="field" style="width:320px; padding-right:0px;margin-left:0px;">
                    <%= Html.SecurityButton(SecurityButtonType.Submit, SecurityButtonAction.Save)%>
                    <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Reset, new { onClick = "javascript: resetFormFields('#" + formId + "');" })%>
                    <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Cancel, new { onClick = "javascript: cancelEntity('" + ViewData["Scope"] + "', 'News', '" + ViewData["BrowseId"] + "', " + ViewData["IsDetail"].ToString().ToLower() + ");" })%>
                    <input class="button" type="button" value="Create" onclick="createObject();"/>
                    <% if ((Model != null) && (!string.IsNullOrEmpty(Model.Link))){%>                     
                            <input class="button" type="button" value="Views" onclick="loadUrl();"/>
                    <% } %>
                </div>
                <div class="field">
                    <div class="newstools">
                       <label class="newstoolslabel">Options:</label>
                       <img class="library-toolbox newstoolsobject" src="<%=Url.Content("~/Content/Images/Icons/library-view.png") %>" title="See File" onclick="javascript: openPopup('<%= Url.Action("ShowFile", "Library") + "/" + (Model==null? -1:Model.Id) %>'); return false;" />
                       <img class="library-toolbox newstoolsobject" src="<%=Url.Content("~/Content/Images/Icons/library-remote.png") %>" title='Upload from URL' onclick="DownloadRemoteDlg();" />
                        <%if ((Model != null) && (!string.IsNullOrEmpty(Model.FilePhysicalName)))
                          { %>
                            <img class="library-toolbox" src="<%=Url.Content("~/Content/Images/Icons/library-download.png") %>" onclick="GoDown('<%= Url.Action("DownloadExecute", "Library") + "/" + Model.Id %>');return false;"  title="Download library file" />
                        <%} %>
                        <%= Html.Hidden("FileName", null, new { id = formId + "FileName" })%>
                        <%= Html.Hidden("FilePhysicalName", null, new { id = formId + "FilePhysicalName" })%>
                        <div id="<%= formId %>FileNameLink" class="newstoolsobject" style="width:15px;">    
                            <noscript> <label for="<%= formId %>FileNameLink" class="required">
                                <asp:Literal ID="MessageFileNameLink" runat="server" Text="<%$ Resources:LabelResource, MessageFileNameLink %>" /></label>
                            </noscript>
                        </div>
                        <div style="float:left;">
                            <label  class="newstoolslabel"><asp:Literal ID="RelatedType" runat="server" Text="<%$ Resources:LabelResource, LibraryRelatedType %>" />:</label>
                        </div>
                        <div class="newstoolsobject">
                            <%= Html.DropDownList("RelatedType", (SelectList)ViewData["NewsObjectTypeList"], string.Empty, new  {Style="width:150px" })%>
                            <%= Html.ValidationMessage("RelatedType", "*")%>
                            <%= Html.Hidden("LibraryIdHidden",ViewData["LibraryIdHidden"]) %>
                        </div>
                        <div class="newstoolsobject">
                            <%= Html.CheckBox("ToNewsletter", new { id = formId + "ToNewsletter" })%>  
                        </div>
                        <div style="float:left;">          
                            <label class="newstoolslabel">Add to Newsletter</label>          
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
        <div class="line">
        <div id="NewsEditFormInternalContent"   class="contentFormEdit">
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
                    <%= Html.TextBox("DateDeletionFrm", ViewData["DateDeletionFrm"].ToString(), new { id = formId + "DateDeletionFrm", @readonly = "readonly" })%>
                    <%= Html.ValidationMessage("DateDeletionFrm", "*")%>
                </div>            
            </div>
            <div class="line">
                <div class="field">
                    <label for="Related">
                        <asp:Literal ID="LibraryRelated" runat="server" Text="<%$ Resources:LabelResource, LibraryRelated %>" />:</label>
                    <%= Html.DropDownList("Related", (SelectList)ViewData["LibraryRelatedList"], string.Empty)%>
                    <%= Html.ValidationMessage("Related", "*")%>
                </div>
                <div class="field">
                    <label for="Score">
                        <asp:Literal ID="LibraryScore" runat="server" Text="<%$ Resources:LabelResource, LibraryScore %>" />:</label>
                    <%= Html.TextBox("Score")%>
                    <%= Html.ValidationMessage("Score", "*")%>
                </div>
           </div>
           <div class="line">
                <div class="field">
                    <label for="Source">
                        <asp:Literal ID="LibrarySource" runat="server" Text="<%$ Resources:LabelResource, LibrarySource %>" />:</label>
                    <%= Html.TextBox("Source")%>
                    <%= Html.ValidationMessage("Source", "*")%>
                </div>
                <div class="field">
                    <label for="DateInNewsletterFrm">Date Added to Newsletter:</label>
                    <%= Html.TextBox("DateInNewsletterFrm", ViewData["DateInNewsletterFrm"].ToString(), new { id = formId + "DateInNewsletterFrm", @readonly = "readonly" })%>
                </div>                            
            </div>
            <div class="line">
                <div class="field">
                    <label for="Source" class="required">
                        <asp:Literal ID="LibraryLink" runat="server" Text="<%$ Resources:LabelResource, LibraryLink %>" />:</label>
                    <%= Html.TextBox("Link")%>
                    <%= Html.ValidationMessage("Link", "*")%>
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
                   <img class="library-toolbox" src="<%=Url.Content("~/Content/Images/Icons/library-download.png") %>" onclick="GoDown('<%= Url.Action("DownloadFileMailExecute", "News") + "/" + file.Id %>');return false;"  title="Download library file"/>
                   </div>
                   </div>
                <% } %>
                <%}
             %>            
            <div class="line">
                <div class="field">
                    <label for="FileName">Filename:</label>
                    <%= Html.ValidationMessage("FileName", "*")%>  	                
                    <p id="<%= formId %>FileNameResult"><%= (Model != null) ? Model.FileName : string.Empty%></p>
                </div>   
            </div>
            </div>
            <div class="newsleft">
                    <div>
                        <label for="Description">
                           <asp:Literal ID="LibraryDescription" runat="server" Text="<%$ Resources:LabelResource, LibraryDescription %>" />:</label>
                          
                     </div>
                        <div id="DescriptionDiv" class="oculto">
                         <input value="View" style="width:35px; height:15px; text-align:center;" class="button" title="Edit Description" id="Text1" onclick="javascript:viewDesign();" />
                        <%= Html.TextArea("Description", new { id = formId + "Description", style = "width:560px;height:227px" })%>
                       <%= Html.ValidationMessage("Description", "*")%>      
                       </div>
                         <div id ="DescriptionViewDiv">
                         <input value="Edit" style="width:35px; height:15px; text-align:center; " class="button" title="Edit Description" id="ButtonEditDesc" onclick="javascript:editDescription();" />                      
                         <div style="border: 1px solid #6495C1; width:560px; height:227px; overflow:auto; background-color:White;overflow-x:scroll;">
                         <p id="DescriptionView"></p></div>
                         </div >
                       
                </div>                  
               <br />     
        </div>       
        </div>
    </fieldset>
</div>
<% } %>