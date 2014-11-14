
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Compelligence.Domain.Entity.CustomField>>" %>

<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'CustomFieldAll');
    }).trigger('resize');
    var hideDuplicateDialog = function() {
        $.unblockUI();
    };
    var duplicateCustomField = function(entity, browseid, scope, container, urlAction) {
        var componentId = scope + entity;
        var gridId = scope + browseid;
        var formId = '#' + componentId + 'EditForm';
        var target = '#' + gridId + 'ListTable';
        var multiselect = getBooleanValue($(target).getGridParam('multiselect'));
        var selectedRowT;
        if (multiselect) {
            selectedRowT = $(target).getGridParam("selarrrow");
            selectedRowT = selectedRowT.join(':');
        } else {
            selectedRowT = $(target).getGridParam('selrow');
        }
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = false;
        if (selectedRowT == null) {
            showAlertSelectItemDialog();
            return;
        }
        var parameters = { Scope: scope, BrowseId: browseid, IsDetail: isDetail, Id: selectedRowT, Container: container };
        if (isDetail) {
            parameters = { Scope: scope, BrowseId: browseid, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter };
        }
        $.post(urlAction, parameters, function(data) {
            reloadGrid(target);
            if (data != '') {
                $('#AlertReturnMessageDialog').empty();
                $('#AlertReturnMessageDialog').html(data);
                $("#AlertReturnMessageDialog").dialog({
                    bgiframe: true,
                    autoOpen: false,
                    modal: true,
                    title: 'Error to Duplicate',
                    close: function() { hideDuplicateDialog(); },
                    buttons: { Ok: function() { $(this).dialog('close'); hideDuplicateDialog(); } }
                });

                $('#AlertReturnMessageDialog').dialog("open");
            }
        });
        if (isDetail) {

        } else {
            eval('showFirstSubtab(' + entity + 'Subtabs);');
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };
</script>

<asp:Panel ID="CustomFieldToolbarContent" runat="server" CssClass="buttonLink">
    <input class="button" type="button" value="New" onclick="javascript: newEntity('<%= Url.Action("Create", "CustomField") %>', '<%= ViewData["Scope"] %>', 'CustomField', 'CustomFieldAll','<%= ViewData["Container"] %>');" />
    <%--<input class="button" type="button" value="Edit" onclick="javascript: editEntity('<%= Url.Action("Edit", "CustomField") %>', '<%= ViewData["Scope"] %>', 'CustomField', 'CustomFieldAll')" />--%>
    <input class="button" type="button" value="Delete" onclick="javascript: sendParamsToOperation('CustomField','Delete','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("Delete", "CustomField") %>');" />
    <input class="button" type="button" value="Duplicate" onclick="javascript: duplicateCustomField('CustomField','CustomFieldAll','<%= ViewData["Scope"] %>','<%= ViewData["Container"] %>','<%= Url.Action("DuplicateCustomField", "CustomField") %>');" />
    <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'CustomField', 'CustomFieldAll');" />
    <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'CustomField', 'CustomFieldAll');" />
    <input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>', '<%= Compelligence.Domain.Entity.Resource.Pages.CustomField %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Admin:Custom Field');" style="float: right;margin-right: 5px;"/><br />
    <span id="<%= ViewData["Scope"] %>CustomFieldAllSelectedOption" class="selectedOption">
    
    </span>
</asp:Panel>
<asp:Panel ID="CustomFieldDataListContent" runat="server" CssClass="contentDetailData">
    <div class="gridOverflow">
        <%= Html.DataGrid("CustomFieldAll", "CustomField", ViewData["Container"].ToString())%></div>
</asp:Panel>
<asp:Panel ID="CustomFieldSearchContent" runat="server">
    <%= Html.GridSearch("CustomFieldAll")%>
</asp:Panel>
<asp:Panel ID="CustomFieldFilterContent" runat="server">
    <%= Html.GridFilter("CustomFieldAll")%>
</asp:Panel>
