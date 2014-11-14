<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'PositioningDetail');
    }).trigger('resize');
    var updateStatusOfDetailEntity = function(entity, browseid, operation, scope, container, headerType, detailFilter, urlAction) {
        var browseName = scope + browseid;
        var componentId = scope + entity;
        var gridId = '#' + browseName + 'ListTable';
        var formId = '#' + componentId + 'EditForm';
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }
        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var selectedRow = $(gridId).getGridParam('selrow');
        //changeStatusMultipleEntities(urlAction, scope, entity, browseid, container, selectedItems, headerType, detailFilter, multiselect);
        var parameters = { Scope: scope, BrowseId: browseid, IsDetail: isDetail, Id: selectedItems, Container: container };
        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
        if (isDetail) {
            parameters = { Scope: scope, BrowseId: browseid, IsDetail: isDetail, Id: selectedItems, HeaderType: headerType, DetailFilter: detailFilter };
        }

        $.post(urlAction, parameters, function(data) {
            if (!isDetail) {
                $(targetSection).html(data)

            } else {
                if (data != null && data != '') {
                    $('#PositioningResultMessage').empty();
                    $('#PositioningResultMessage').html(data);
                    $("#PositioningResultMessage").dialog({
                        bgiframe: true,
                        autoOpen: false,
                        modal: true,
                        title: 'Result Update',
                        width: '500',
                        resizable: true,
                        close: function() {  $(this).dialog("destroy"); },
                        buttons: { Ok: function() { $(this).dialog('close');  $(this).dialog("destroy"); } }
                    });

                    $('#PositioningResultMessage').dialog("open");
                }
            }
            reloadGrid(gridId);
            /*if(!isDetail) { disableFormFields(formId); }*/
            if (container != null) {
                hideLoadingDialogForSection(container);
            }
            else
            { hideLoadingDialog(); }
        });
        if (!isDetail) {
            eval('showFirstSubtab(' + entity + 'Subtabs);');
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };
</script>

<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>PositioningDetailDataListContent" class="absolute">
        <asp:Panel ID="PositioningDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Positioning") + "', '" + ViewData["Scope"] + "', 'Positioning', 'PositioningDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Positioning") + "', '" + ViewData["Scope"] + "', 'Positioning', 'PositioningDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperationByBrowse('Positioning','Delete', 'PositioningDetail','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "Positioning") %>');" />
            <input class="button" type="button" value="Enable" onclick="javascript: updateStatusOfDetailEntity('Positioning','PositioningDetail','Enable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= ViewData["HeaderType"] %>','<%= ViewData["DetailFilter"] %>','<%= Url.Action("UpdateEnableStatus", "Positioning") %>');" />
            <input class="button" type="button" value="Disable" onclick="javascript: updateStatusOfDetailEntity('Positioning','PositioningDetail','Disable','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= ViewData["HeaderType"] %>','<%= ViewData["DetailFilter"] %>','<%= Url.Action("UpdateDisableStatus", "Positioning") %>');" />
        </asp:Panel>
        <asp:Panel ID="PositioningDetailDataListContent" runat="server" CssClass="contentDetailData">
            <%= Html.DataGrid("PositioningDetail", new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%>
        </asp:Panel>
    </div>
    <asp:Panel ID="PositioningDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>PositioningEditFormContent">
        </div>
    </asp:Panel>
</div>
