<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage"%>
<asp:Content ID="indexHead" ContentPlaceHolderID="Head" runat="server">
    <title>Compelligence - Backend</title>
</asp:Content>
<asp:Content ID="indexStyles" ContentPlaceHolderID="Styles" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery.treeview.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />    
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/jquery.searchFilter.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.jqgrid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.multiselect.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/ext-all.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/rte.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
    <style type="text/css">
        .ui-widget {
            font-size: 0.8em;
        }
        /*Fix zindex*/
        /*
        .ui-front
        {
        	z-index:1000;
        }*/
    </style>
</asp:Content>
<asp:Content ID="indexScripts" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-migrate-1.1.1.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/CKeditor/ckeditor.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.maskedinput-1.2.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"       type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.mousewheel.min.js") %>" type="text/javascript"></script>
    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.multiselect.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui/ui.spinner.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.locale-en-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/ui.multiselect.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.jqGrid.src.js") %>" type="text/javascript"></script>


    <script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.rte.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/ext/ext-base.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/ext/ext-all.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/ajaxupload.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/fileuploader.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-idleTimeout.js") %>" type="text/javascript""></script>
    <script src="<%= Url.Content("~/Scripts/d3.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
    <script type="text/javascript">
        function CStateTab() {
            this.childTabs = null;
            this.selectedId = null;
            this.objectType = null;
        }

        var BackEndTabs;
        var WorkspaceSubtabs;
        var EnvironmentSubtabs;
        var AdminSubtabs;
        var SearchSubtabs;
        //var ResearchSubtabs;

        var WorkspaceState = new CStateTab();
        var EnvironmentState = new CStateTab();
        var AdminState = new CStateTab();


        var ProjectSubtabs;
        var CompetitorSubtabs;
        var PartnerSubtabs;
        var EventSubtabs;
        var KitSubtabs;
        var UserSubtabs;
        var TeamSubtabs;
        var ObjectiveSubtabs;
        var ProductSubtabs;
        var WebsiteSubtabs;
        var FileSubtabs;
        var IndustrySubtabs;
        var DealSubtabs;
        var SupplierSubtabs;
        var CustomerSubtabs;
        var WorkflowSubtabs;
        var ContentTypeSubtabs;
        var ContentTypeSubtabs;
        var TemplateSubtabs;
        var Dashboardtabs;
        var CriteriaSetSubtabs;
        var CriteriaSubtabs;
        var NuggetSubtabs;
        //var WinLossSubtabs;
        var ComparinatorSubtabs;
        var CalendarSubtabs;
        var LibraryTypeSubtabs;
        var LibraryExternalSourceSubtabs;
        var urlBrowsePopup = '<%= Url.Action("GetBrowsePopup", "Browse") %>';
        var urlIndustryBrowsePopup = '<%= Url.Action("GetIndustryBrowsePopup", "Browse") %>';
        var urlBrowsePopupCols = '<%= Url.Action("GetBrowsePopupCols", "Browse") %>';
        var urlPositioningBrowsePopup = '<%= Url.Action("GetPositioningBrowsePopup", "Browse") %>';
        var urlBrowseNewsLetterPopup = '<%= Url.Action("GetBrowseNewsLetterPopup", "Browse") %>';
        var SourceSubtabs;
        var NewsletterSubtabs;
        var IndustryDetailSubtabs;
        var MarketTypeSubtabs;
        var ConfigurationSubtabs;
        var ConfigurationLabelsSubtabs;
        var TrendSubtabs;
        var ProjectArchiveSubtabs;
        var NewsScoringSubtabs;
        var NewsSubtabs;
        var PositioningSubtabs;
        //var WinLossAnalysisSubtabs;
        var ComparinatorCriteriaUploaderSubtabs;
        var SwotSubtabs;
        var SurveySubtabs;
        var ConfigurationGeneralSubtabs;
        var ProjectsApprovalSubtabs;
    </script>
    <%--(Small)--%>
    <%= Html.ScriptTabsSubTabs((ClientCompany)ViewData["ClientCompany"])%>

    <script type="text/javascript">

        var initializeForm = function(formId, urscas, entityLock, styledFormFields) {

            if (urscas == '<%= UserSecurityAccess.Read %>' || ((entityLock != null) && (entityLock.toLowerCase() == 'true'))) {
                disableFormFields(formId);
            }
            focusFirstFormField(formId);
            // If there are date fields in this form, so Datepicker will be applied
            if (styledFormFields != null) {
                for (var i = 0; i < styledFormFields.length; i++) {
                    var formFieldId = styledFormFields[i];
                    var ind = formFieldId.indexOf(':');

                    if (ind > 0) {

                        var prefix = formFieldId.substring(0, ind);
                        formFieldId = formFieldId.substr(ind + 1);

                        switch (prefix) {
                            case 'dt':
                                $(formFieldId).datepicker(
                                  {
                                      changeMonth: true,
                                      changeYear: true
                                  }
                              );
                                break;
                            case 'dc':
                                // Format for decimal value
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        };

        var executePostActions = function(formId, scope, entity, browseId, isDetail) {
            var componentId = scope + entity;
            var gridId = '#' + scope + browseId + 'ListTable';

            var operationCode = $(formId + ' input[type=hidden][name=OperationStatus]').val();

            if (operationCode == '<%= OperationStatus.Successful %>') {

                setTimeout("$.blockUI({ message: $('#SuccessUpdateMessage'), fadeIn: 700, fadeOut: 700, timeout: 3000," +
                            "showOverlay: false, centerY: false, css: { width: '300px', top: '10px', left: '', right: '10px', " +
                                "border: 'none', padding: '5px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: 0.6, color: '#fff' } });", 1000);

                if (isDetail) {
                    $('#' + componentId + 'EditFormContent').fadeOut('slow');
                    $('#' + componentId + 'DetailDataListContent').fadeIn('slow');
                }
                else {
                    if (entity != "")

                    { showafterSave(entity); }
                }

                reloadGrid(gridId);
            }
        };

        var executePostActionsConfigurationsTab = function(formId, scope, entity, isDetail) {
            var componentId = scope + entity;

            var operationCode = $(formId + ' input[type=hidden][name=OperationStatus]').val();

            if (operationCode == '<%= OperationStatus.Successful %>') {

                setTimeout("$.blockUI({ message: $('#SuccessUpdateMessage'), fadeIn: 700, fadeOut: 700, timeout: 3000," +
                            "showOverlay: false, centerY: false, css: { width: '300px', top: '10px', left: '', right: '10px', " +
                                "border: 'none', padding: '5px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: 0.6, color: '#fff' } });", 1000);

                if (isDetail) {
                    $('#' + componentId + 'EditFormContent').fadeOut('slow');
                    $('#' + componentId + 'DetailDataListContent').fadeIn('slow');
                }
                else {
                    if (entity != "") {
                        //    showafterSave(entity); 
                    }
                }
            }
        };

        var uploadFileComponent;


        var initializeUploadField = function(urlAction, scope, browseId, uploadFileLink, uploadFileResult, targetField, container, headerType, detailFilter, isAutoSubmit) {
            var gridId = scope + browseId;
            var isDetail = isDetailOperation(detailFilter);
            var autoSubmitValue = true;

            if (isAutoSubmit != null) {
                autoSubmitValue = isAutoSubmit;
            }

            uploadFileComponent = new AjaxUpload(uploadFileLink, {
                action: urlAction,
                data: {
                    IsDetail: isDetail,
                    HeaderType: headerType,
                    DetailFilter: detailFilter
                },
                autoSubmit: autoSubmitValue,
                onChange: function(file, extension) {
                    if (!autoSubmitValue) {
                        $("#UploadFileConfirmDialog").dialog('open');
                    }
                },
                onSubmit: function(file, ext) {

                    if (uploadFileResult != null) //have container for show message at submit
                    {
                        $(uploadFileResult).text('Uploading ' + file + ' ...');
                        showLoadingDialogForSection(container);
                    }
                },
                onComplete: function(file, response) {
                    if (!autoSubmitValue) {
                        var Message = '<p>Your document was successfully uploaded.</p>';
                        $('#AlertReturnMessageDialog').html(Message);
                        $('#AlertReturnMessageDialog').dialog('open');
                    }
                    if (uploadFileResult != null) {
                        if (response == "Fail") {
                            $(uploadFileResult).html('<b>Upload file failed..!<b>');

                        }
                        else {

                            $(uploadFileResult).text('Upload file ' + file + ' ');
                        }
                    }

                    if (targetField != null) {
                        if (targetField.indexOf(':') > 0) {
                            var targetComponents = targetField.split(':');

                            if (targetComponents.length > 1) {
                                $(targetComponents[0]).val(file);
                                $(targetComponents[1]).val(response);
                            }

                        } else {
                            $(targetField).val(response);
                        }
                    }

                    if (browseId != null) {
                        reloadGrid('#' + gridId + 'ListTable');
                    }

                    hideLoadingDialogForSection(container);

                } //OnComplete		
            });
        };


        var initializeUploadFileRawMode = function(urlAction, scope, browseId, uploadFileLink, uploadFileResult, targetField, container, headerType, detailFilter, isAutoSubmit) {
            var gridId = scope + browseId;
            var isDetail = isDetailOperation(detailFilter);
            var autoSubmitValue = true;

            if (isAutoSubmit != null) {
                autoSubmitValue = isAutoSubmit;
            }

            uploadFileComponent = new qq.FileUploader({
                action: urlAction,
                element: $(uploadFileLink)[0],
                sizeLimit: 20 * 1024 * 1024,
                minSizeLimit: 1,
                multiple: false,
                params: {
                    IsDetail: isDetail,
                    HeaderType: headerType,
                    DetailFilter: detailFilter
                },

                onComplete: function(id, fileName, responseJSON) {
                    //alert(id + "-" + fileName + "-" + responseJSON.result);

                    if (uploadFileResult != null) {
                        if (responseJSON.result == "Fail")
                            $(uploadFileResult).html('<b>Upload file failed..!<b>');
                        else
                            $(uploadFileResult).text(fileName);
                    }

                    if (targetField != null) {
                        if (targetField.indexOf(':') > 0) {
                            var targetComponents = targetField.split(':');

                            if (targetComponents.length > 1) {
                                $(targetComponents[0]).val(fileName);
                                $(targetComponents[1]).val(responseJSON.result);
                            }

                        } else {
                            $(targetField).val(responseJSON.result);
                        }
                    }

                    if (browseId != null) {
                        reloadGrid('#' + gridId + 'ListTable');
                    }
                } //OnComplete

            }
            );
            uploadFileComponent.classes = { button: 'button' };

        };

        var downloadFile = function(urlAction, scope, browseId, headerType, detailFilter) {
            var isDetail = isDetailOperation(detailFilter);
            urlAction += '?Scope=' + scope;
            urlAction += '&IsDetail=' + isDetail;
            urlAction += '&HeaderType=' + headerType;
            urlAction += '&DetailFilter=' + detailFilter;

            $('#DownloadFileSection').prop("src", urlAction);
        };

        var downloadFileById = function(urlAction, scope, browseId, headerType, detailFilter, entity) {
            var componentId = scope + entity;
            var gridId = scope + browseId;
            var selectedRow = $('#' + gridId + 'ListTable').getGridParam('selrow');
            var multiselect = getBooleanValue($('#' + gridId + 'ListTable').getGridParam('multiselect'));
            var selectedItems;
            if (multiselect) {
                selectedItems = $('#' + gridId + 'ListTable').getGridParam("selarrrow");
            } else {
                selectedItems = $('#' + gridId + 'ListTable').getGridParam("selrow");
            }
            if (selectedItems == null) {
                showAlertSelectItemDialog();
                return;
            }
            else if (selectedItems.length > 1) {
                var Message = '<p>You can only download one file at a time.</p>';
                MessageBackEndDialog('Error Dowload', Message);
                return;
            }
            else {
                var id;
                if (selectedItems.length > 1)
                    id = selectedItems[0];
                else id = selectedItems;
                var isDetail = isDetailOperation(detailFilter);
                urlAction += '?Scope=' + scope;
                urlAction += '&IsDetail=' + isDetail;
                urlAction += '&HeaderType=' + headerType;
                urlAction += '&DetailFilter=' + detailFilter;
                urlAction += '&FileId=' + id;
                $('#DownloadFileSection').prop("src", urlAction);
            }
        };

        var downloadTemplateFile = function(urlAction, scope, templateId) {
            urlAction += '?Scope=' + scope;
            urlAction += '&TemplateId=' + templateId;
            $('#DownloadFileSection').prop("src", urlAction);
        };

        var showSubtabs = function(tabPanel) {
            tabPanel.items.each(function(item, index, length) { tabPanel.unhideTabStripItem(item.id); });
        };

        var showFirstSubtab = function(tabPanel) {
            tabPanel.items.each(function(item, index, length) {
                if (index == 0) {
                    tabPanel.unhideTabStripItem(item.id);
                } else {
                    tabPanel.hideTabStripItem(item.id);
                }
            });
        };

        var hideSubtabs = function(tabPanel) {
            tabPanel.items.each(function(item, index, length) { tabPanel.hideTabStripItem(item.id); });
        };

        var RemoveSubTabOfTabPanel = function(tabPanelId, subTabId) {
            var tabPanelT = Ext.getCmp(tabPanelId);
            var subTabToRemove = Ext.getCmp(subTabId);
            tabPanelT.remove(subTabToRemove.id, true);
        };

        function reloadFrame(target, scope, urlReload, typeSource) //scope=Workspacexxxxx,xxxxx=Deal,Project
        {
            var content = $('#iframeBox').contents().find('body'); //iframe
            var scopeleft = scope.toString().substr(0, 9);
            var scoperight = scope.toString().substring(9);
            if (content != null && content.text().length == 0) {
                var IdObject = getIdValue(scopeleft, scoperight);
                var DetailType = '<%= (int) DetailType.Comment %>';
                var ToTarget = '#' + scope + 'CommentContent';

                if (typeSource == "Discussion") {
                    DetailType = '<%= (int) DetailType.Discussion %>'
                    ToTarget = '#' + scope + 'DiscussionContent';
                    //alert('Discussion' + " type"+typeof( typeSource ) + "urlReload"+urlReload + "IdObject"+ IdObject + "scope"+ scope + "DetailType"+ DetailType + "ToTarget"+ ToTarget);
                }
                //else
                //alert('Comment' + "urlReload"+urlReload + "IdObject"+ IdObject + "scope"+ scope + "DetailType"+ DetailType + "ToTarget"+ ToTarget);
                loadDetailList(urlReload, IdObject, scope, DetailType, ToTarget);
                clearInterval(idTimer);
            }

        }
        var idTimer;
        function loadiFrameContent(urlAction, target, scope, urlReload, typeSource) {   //alert(":"+scope);
            showLoadingDialogForSection(target);
            var innerStyle = "style='width:700px;height:350px;border:0px'"
            var innerHtml = "<iframe id='iframeBox' src=" + urlAction + " onload=\"hideLoadingDialogForSection('" + target + "')\" " + innerStyle + "></iframe>";
            $(target).html(innerHtml);
            idTimer = setInterval("reloadFrame('" + target + "','" + scope + "','" + urlReload + "','" + typeSource + "')", 1000);
        };        
    </script>
    <%--Functions.js should first load before that Datagrid.js--%>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Functions.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Utility.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>" type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/System/BackEnd/TreeNodes.js") %>" type="text/javascript"></script>--%>
    <%= Html.BuildTreeScript() %>
    
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/DataGrid.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Browse.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Filter.js") %>" type="text/javascript"></script>   

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Resizable.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/Help.js") %>" type="text/javascript"></script>
<div id="datatmp"></div>
   <script type="text/javascript">
       loadConfig('<%=Url.Action("loadConfig", "Browse") %>');
    </script>

</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="mainContent">
        
        <div>
            <iframe id="DownloadFileSection" src="javascript: void(0);" frameborder="0" marginheight="0"
                marginwidth="0"></iframe>
        </div>
        <div id="AlertFailedResponseDialog" title="Failed Response" style="display:none">
            <p>
                <span class="ui-icon ui-icon-alert alertFailedResponseDialog"></span>Server returned
                a failure response, please try again.
            </p>
        </div>
        <div id="AlertSelectItemDialog" title="Alert Message" style="display:none">
            <p>
                <span class="ui-icon ui-icon-alert alertSelectItemDialog"></span>You should select
                a valid item from list.
            </p>
        </div>
        <div id="SuccessDeleteMessage" class="ntfMsg" style="display: none;">
            <h2>Operation was successful</h2>
        </div>
        <div id="SuccessUpdateMessage" class="ntfMsg" style="display: none;">
            <h2>Operation was successful</h2>
        </div>
        <div id="AlertReturnMessageDialog" title="Return Message">
        </div>
        <div id="AletShoeMessageTemplateDialog" class="ntfMsg" style="display: none;">
        </div>
        <div id="LoadingDialog" class="displayNone">
            <p>
                <img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                    class="left" /><span class="loadingDialog">Loading ...</span>
            </p>
        </div>
        <div id="DeleteConfirmDialog" title="Confirm delete?" style="height:auto;display:none;">
            <p>
                <span class="ui-icon ui-icon-alert confirmDialog" style="padding-right:5px;margin-bottom: 70px; display:none;"></span>
                <%--<img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                    class="left" />--%>
                These items will be permanently
                deleted and cannot be recovered. Are you sure?</p>
        </div>
        <div id="AlertNoFoundCalendarDialog" title="There are not Data to Show" style="height:auto;display:none;">
            <p>
                <span class="ui-icon ui-icon-alert confirmDialog" style="padding-right:5px;margin-bottom: 70px; display:none;"></span>
                <%--<img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                    class="left" />--%>
                No events in system</p>
        </div>
        <div id="ChooseConfirmDueDate" title="Warning!" style="display:none">
        <p>
            <span class="ui-icon ui-icon-alert confirmDialog"></span>
            <%--<img src="<%= Url.Content("~/Content/Images/Ajax/loader.gif") %>" alt=""
                class="left" />--%>
            The Entered Due Date is in the Past. Please Confirm.</p>
        </div>
        <div id="DeleteDetailConfirmDialog" title="Confirm deletion?" style="display:none">
            <p>
                <span class="ui-icon ui-icon-alert confirmDialog"></span>These relations will be
                deleted. Are you sure?</p>
        </div>
        <div id="RemoveUsersConfirmDialog" title="Confirm remove Users?" style="display:none">
            <p>
                <span class="ui-icon ui-icon-alert confirmDialog"></span>These relations will be
                deleted. Are you sure?</p>
        </div> 
        <div id="UploadFileConfirmDialog" title="Confirm upload" style="display:none">
            <p>
                <span class="ui-icon ui-icon-alert confirmDialog"></span>Do you want to upload a new version?</p>
        </div>
        <div id="SuccessMessage" class="successMessage displayNone">
            Operation was successfully!!</div>
            
         <div id="SuccessMessageNewsletter" class="successMessage displayNone">
            Newsletter was sent successfully!</div>
         <div id="CheckingAddressNewsletter" class="successMessage displayNone">
            Newsletter Destination</div>
            
        <div id="FailureMessage" class="failureMessage displayNone">
            Operation was failed!!</div>
        <div id="SendResponseMessageDialog" title="Send Response"  class="ntfMsg" style="display: none;">
            <p>
                Thank you for your submission!
            </p>
        </div>
        <div id="ReSendEmailToApprovedDialog" title="ReSend Mail to User" class="ntfMsg" style="display: none;">
            <p>
                The email was forwarded successfully.
            </p>
        </div> 
        <div id="IncorrectProcessDialog" title="Incorrect Process" class="ntfMsg" style="display: none;">
            <p>
                The process could not complete the action successfully, please check the source field.
            </p>
        </div> 
        <div id="CreateObjectDialog" title="Create Object" class="ntfMsg" style="display: none;">
            <p>
                The Object was created successfully.
            </p>
        </div>
        <div id="DialogNote" style="display: none;">
               <textarea id="note" style="border: 1px solid #B4B8B4;"></textarea>
            </div>
         <div id="PublishConfirmationDialog" title="Confirm Publish Project"  class="ntfMsg" style="display: none;">
            <p>
                These Projects will be
                Published. Are you sure?
            </p>
        </div>  
        
        <div id="ProjectFileLinkDialog" title="Link" class="ntfMsg" style="display: none; height:auto; height: 36px; margin-top: 10px;">
                <div>
                <div style="float: left;width: 46%;">
                <label>Add link location: </label> 
                <%= Html.RadioButton("TypeLink", "AddLink",true, new { style = "vertical-align:bottom;margin-bottom: -3px;margin-top: 0;" })%>
                </div>
                <div style="float: left;width: 50%;">
                <label>Download file from link: </label> 
                <%= Html.RadioButton("TypeLink", "DownloadLink", false, new { style = "vertical-align:bottom;margin-bottom: -3px;margin-top: 0;" })%>
               </div> 
               </div>
                <%= Html.TextBox("ProjectFileLink", null, new { id = "ProjectFileLink" , style="width:260px" })%>
                <%= Html.ValidationMessage("ProjectFileLink", "*")%>
                <br />
               
        </div>        
         <div id="PositioningResultMessage" class="displayNone">
           </div>
        <div id="MessageBackEndDialog" class="displayNone"></div>
        <div id="HelpDialog" class="displayNone">
		</div>
        <asp:Panel ID="Content" runat="server">
           <%-- <% Html.RenderPartial("TreeviewMenu"); %>--%>
            <div id="AdminTabs">
                <div id="WorkspaceTab">
                    <!-- Sub Tabs -->
                    <div id="DashboardContent" class="x-hide-display borderpanel" >
                    </div>
                    <div id="ObjectiveContent" class="x-hide-display heightPanel borderpanel" >
                    </div>
                    <div id="KitContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ProjectContent" class="x-hide-display heightPanel borderpanel">
                    </div>   
                    <div id="NewsContent" class="x-hide-display heightPanel borderpanel">
                    </div>                 
                    <div id="EventContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="DealContent" class="x-hide-display heightPanel borderpanel">
                    </div>                    
                    <div id="SurveyContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="CalendarContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                  <%--  <div id="NewsletterContent" class="x-hide-display heightPanel">
                    </div>--%>
                    <div id="NewsletterContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                </div>
                <div id="EnvironmentTab" class="x-hide-display">
                    <div id="IndustryContent" class="x-hide-display borderpanel ">
                    </div>
                    <div id="CompetitorContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ProductContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="CustomerContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="SupplierContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="PartnerContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="LibraryContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="CriteriasContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="MarketTypeContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                </div>
                <div id="ToolsTab" class="x-hide-display heightPanel">
                    <div id="SwotContent" class="x-hide-display heightPanel borderpanel" >
                    </div>
                     <!--<div id="WinLossContent" class="x-hide-display heightPanel borderpanel">
                    </div>-->
                    <div id="NuggetContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="TrendContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ProjectArchiveContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="NewsScoringContent" class="x-hide-display heightPanel borderpanel">
                    </div>                    
                    <!--<div id="WinLossAnalysisContent" class="x-hide-display heightPanel borderpanel">
                   </div>-->
                    <div id="CriteriasContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ComparinatorCriteriaUploaderContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="SourceContent" class="x-hide-display heightPanel">
                    </div>
                </div>
                <!--<div id="ResearchTab" class="x-hide-display heightPanel">
                    <div id="SourceContent" class="x-hide-display heightPanel">
                    </div>
                </div>-->
                <div id="ReportsTab" class="x-hide-display heightPanel" style="min-height:600px;height:auto;">
                    <div id="ReportsWorkspaceContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ReportsEnvironmentContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ReportsAdminContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ReportsEventContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ReportsDynamicContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ReportsGeneralContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ReportsComparisonContent" class="x-hide-display heightPanel borderpanel" style="height:auto">
                    </div>
                </div>
                <div id="AdminTab" class="x-hide-display heightPanel">
                    <!-- Sub Tabs -->
                    <div id="UserContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="TeamContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="RoleContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="TemplateContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="WebsiteContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="WorkflowContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ContentTypeContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="LibraryTypeContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="EventTypeContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="LibraryExternalSourceContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="CustomFieldContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ConfigurationLabelsContent" class="x-hide-display heightPanel borderpanel">
                    </div>
                    <div id="ConfigurationsContent" class="x-hide-display heightPanel">
                    </div>
                    <div id="ConfigurationGeneralContent" class="x-hide-display heightPanel">
                    </div>
                </div>
                <div id="SearchTab" class="x-hide-display heightPanel borderpanel">
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>