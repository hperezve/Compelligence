<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndSite.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery.treeview.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />

    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel2/grid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel2/jquery.searchFilter.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel2/ui.jqgrid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel2/ui.multiselect.css") %>" rel="stylesheet"  type="text/css" />

   <link href="<%= Url.Content("~/Content/Styles/ext-all.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/rte.css") %>" rel="stylesheet" type="text/css" />
    
    <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
<script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-1.3.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-ui/ui.spinner.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.locale-en.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.base.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.celledit.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.common.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.formedit.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.import.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.inlinedit.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.jqueryui.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.loader.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.postext.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.setcolumns.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.subgrid.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.tbltogrid.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.treegrid.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jqgrid/jqDnR.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jqModal.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.fmatter.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.searchFilter.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/JsonXml.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/ui.multiselect.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.rte.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/ext/ext-base.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/ext/ext-all.js") %>" type="text/javascript"></script>
    
  <script type="text/javascript">

      var BackEndTabs;
      var WorkspaceSubtabs;
      var EnvironmentSubtabs;
      var AdminSubtabs;
      var SearchSubtabs;
      var ResearchSubtabs;

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
      var WorkflowSubtabs;
      var ContentTypeSubtabs;
      var TemplateSubtabs;
      var Dashboardtabs;
      var CriteriaSetSubtabs;
      var CriteriaSubtabs;
      var NuggetSubtabs;
      var WinLossSubtabs;
      //        var CriteriaGrouptabs;
      var ComparinatorSubtabs;
      var CalendarSubtabs;
      var LibraryTypeSubtabs;
      var LibraryExternalSourceSubtabs;
      var urlBrowsePopup = '<%= Url.Action("GetBrowsePopup", "Browse") %>';
      var SourceSubtabs;
      var NewsletterSubtabs;

      var IndustryDetailSubtabs;
      //        var BudgetTypeSubtabs;
        
    </script>

   

    <script type="text/javascript">

        var initializeForm = function(formId, urscas, entityLock, styledFormFields) {

            if (urscas == '<%= UserSecurityAccess.Read %>' || ((entityLock != null) && (entityLock.toLowerCase() == 'true'))) {
                disableFormFields(formId);
            }
            //alert(formId + ", "+ urscas+ ", "+entityLock+ ", "+styledFormFields );
            focusFirstFormField(formId);
            //$(formId + ' input:visible:enabled:first').focus();
            //$(formId + ' input:text:visible:enabled:first').focus();

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
                                $(formFieldId).datepicker();
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
                //successMessage(scope, entity);
                setTimeout("$.blockUI({ message: $('#SuccessUpdateMessage'), fadeIn: 700, fadeOut: 700, timeout: 3000," +
                            "showOverlay: false, centerY: false, css: { width: '300px', top: '10px', left: '', right: '10px', " +
                                "border: 'none', padding: '5px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: 0.6, color: '#fff' } });", 1000);

                if (isDetail) {
                    $('#' + componentId + 'EditFormContent').fadeOut('slow');
                    $('#' + componentId + 'DetailDataListContent').fadeIn('slow');
                }

                reloadGrid(gridId);
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

            uploadFileComponent = new Ajax_upload(uploadFileLink, {
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
                    if (uploadFileResult != null) {
                        $(uploadFileResult).text('Uploading ' + file + ' ...');
                        showLoadingDialogForSection(container);
                    }
                },
                onComplete: function(file, response) {

                    if (uploadFileResult != null) {
                        $(uploadFileResult).text('Upload file ' + file + ' ' + response);
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
                }
            });
        };

        var downloadFile = function(urlAction, scope, browseId, headerType, detailFilter) {
            var isDetail = isDetailOperation(detailFilter);
            urlAction += '?Scope=' + scope;
            urlAction += '&IsDetail=' + isDetail;
            urlAction += '&HeaderType=' + headerType;
            urlAction += '&DetailFilter=' + detailFilter;

            $('#DownloadFileSection').attr("src", urlAction);
        };

        var downloadTemplateFile = function(urlAction, scope, templateId) {
            urlAction += '?Scope=' + scope;
            urlAction += '&TemplateId=' + templateId;
            $('#DownloadFileSection').attr("src", urlAction);
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

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Utility.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>" type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/System/BackEnd/TreeNodes.js") %>" type="text/javascript"></script>--%>
    <%--<%= Html.BuildTreeScript() %>--%>
    
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/DataGrid.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Browse.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Filter.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Functions.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Resizable.js") %>" type="text/javascript"></script>

    
    
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ProfileContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<asp:Panel ID="EventToolbarContent" runat="server" CssClass="buttonLink">
   <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "Event") %>', '<%= ViewData["Scope"] %>', 'Event', 'EventAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "Event") %>', '<%= ViewData["Scope"] %>', 'Event', 'EventAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('Event','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Event") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: sendParamsToOperation('Event','Duplicate','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Duplicate", "Event") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'Event', 'EventAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'Event', 'EventAll');" />
    <input id="<%= ViewData["Scope"] %>EventCheckbox" class="checkbox" type="checkbox" value="Show Mine" onchange="javascript:showMyEntities(this,'<%= ViewData["Scope"] %>', 'EventAll', 'EventAllView', 'EventAssignedTo','<%= Session["UserId"] %>' );" />
    <label for="<%= ViewData["Scope"] %>EventCheckbox" style="font-size: 12px; color: black;">
    <asp:Literal ID="EventCheckbox" runat="server" Text="<%$ Resources:LabelResource, EntityChecked %>" />
    </label>
    <span id="<%= ViewData["Scope"] %>EventAllSelectedOption" class="selectedOption"></span>
</asp:Panel>
<asp:Panel ID="EventDataListContent" runat="server" CssClass="contentDetailData">
<%--    <%= Html.DataGrid("EventAll", "Event", ViewData["Container"].ToString())%>--%>
<table cellpadding="0" cellspacing="0" class="scroll" id="WorkspaceEventAllListTable" style="z-index:2000"></table>
<div class="scroll" id="WorkspaceEventAllListPager" style="text-align: center;"></div>

<script type="text/javascript">
    $(function() {
        $('#WorkspaceEventAllListTable').jqGrid({
            url: '/Browse.aspx/GetData?bid=EventAll&eou',
            datatype: "json",
            mtype: "POST",
            colNames: ['',
            'Event Name',
            'Status',
            'Time Frame',
            'Industry',
            'Competitor',
            'Product',
             'Scenario',
             'Confidence',
             'Severity',
             'RecommendedActions',
              'Comment',
              'Owner',
              'Opened By',
              'Location',
              'Start Date',
               'End Date'],
            colModel: [{ name: 'EventAllView.Id', index: 'EventAllView.Id', hidden: true },
           { name: 'EventAllView.Name', index: 'EventAllView.Name' },
		   { name: 'EventAllView.Status', index: 'EventAllView.Status' },
		   { name: 'EventAllView.TimeFrame', index: 'EventAllView.TimeFrame' },
		   { name: 'EventAllView.IndustryName', index: 'EventAllView.IndustryName' },
		   { name: 'EventAllView.CompetitorName', index: 'EventAllView.CompetitorName' },
		   { name: 'EventAllView.ProductName', index: 'EventAllView.ProductName' },
		   { name: 'EventAllView.Scenario', index: 'EventAllView.Scenario' },
		   { name: 'EventAllView.Confidence', index: 'EventAllView.Confidence' },
		   { name: 'EventAllView.Severity', index: 'EventAllView.Severity' },
		   { name: 'EventAllView.RecommendedActions', index: 'EventAllView.RecommendedActions' },
		   { name: 'EventAllView.Comment', index: 'EventAllView.Comment' },
		   { name: 'EventAllView.AssignedTo', index: 'EventAllView.AssignedTo' },
		   { name: 'EventAllView.OpenedBy', index: 'EventAllView.OpenedBy' },
		   { name: 'EventAllView.Location', index: 'EventAllView.Location' },
		   { name: 'EventAllView.StartDate', index: 'EventAllView.StartDate' },
		   { name: 'EventAllView.EndDate', index: 'EventAllView.EndDate'}],
            pager: '#WorkspaceEventAllListPager',
            rowNum: 10,
            rowList: [10, 20, 30, 40, 50],
            imgpath: '/Content/Styles/jqgrid/sand/images',
            sortname: 'EventAllView.Name',
            multiselect: true,
            multikey: 'ctrlKey',
            viewrecords: true,
            sortorder: 'asc',
            scroll: true,
            onSelectRow: function(id) {
            /*getEntity('/Event.aspx/Edit', 'Workspace', 'Event', id, 'EventAll', '#EventContent'); */
            },
            loadComplete: function() { hideLoadingDialog(); }
        });


        $('#WorkspaceEventAllListTable').jqGrid('navGrid', '#WorkspaceEventAllListPager',
	   { edit: false, add: false, del: false, search: false, refresh: true });
	   $('#WorkspaceEventAllListTable').jqGrid('navButtonAdd', '#WorkspaceEventAllListPager',
         { caption: '', title: 'Load all data', buttonicon: 'setIconLoadAll', onClickButton: function() { showAllData('#WorkspaceEventAllListTable', 'Workspace', 'Event'); unCheckMyEntities('#WorkspaceEventAllListTable', 'Workspace', 'Event') }, position: 'last' }); $('#WorkspaceEventAllListTable').jqGrid('navButtonAdd', '#WorkspaceEventAllListPager', { caption: '', title: 'Toggle columns', buttonicon: 'setIconToggle', onClickButton: function() { toggleColumnPopup('#WorkspaceEventAllColumnPopup'); }, position: 'last' });
        var myWidth = Math.round($(window).width() * 0.98);
        $('#WorkspaceEventAllListTable').setForceGridWidth(myWidth, false);
    });
 </script>
</asp:Panel>
<asp:Panel ID="EventSearchContent" runat="server">
    <%= Html.GridSearch("EventAll") %>
</asp:Panel>
<asp:Panel ID="EventFilterContent" runat="server">
    <%= Html.GridFilter("EventAll") %>
</asp:Panel>


</asp:Content>