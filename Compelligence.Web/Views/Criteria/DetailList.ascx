<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    function Step2() {
        var detailfilter = '<%= ViewData["DetailFilter"] %>';
        var urlAction = '<%=Url.Action("CustomCriteriaStep1","Industry") %>'+'?DetailFilter='+detailfilter;
        openPopup(urlAction);
    }
    /*
    function loadSelectedPopupItems(ids) {
        alert(ids);
    }*/
</script>
<script type="text/javascript">
    $(function() {
        resizeGridId = '<%= ViewData["Scope"] %><%= ViewData["BrowseDetailName"] %>ListTable';
        $('#' + resizeGridId).jqGrid('setGridHeight', '540');
    });
</script>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
    }).trigger('resize');
</script>
<div class="indexTwo" >
    <div id="<%= ViewData["Scope"] %>CriteriaDetailDataListContent" class="absolute">
        <asp:Panel ID="CriteriaDetailToolbarContent" runat="server" CssClass="buttonLink">
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.NewDetail, new { onClick = "javascript: newEntity('" + Url.Action("Create", "Criteria") + "', '" + ViewData["Scope"] + "', 'Criteria',  '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.EditDetail, new { onClick = "javascript: editEntity('" + Url.Action("Edit", "Criteria") + "', '" + ViewData["Scope"] + "', 'Criteria', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { onClick = "javascript: addEntity('" + Url.Action("CreateDetail", "Criteria") + "', '" + ViewData["Scope"] + "', 'Criteria', '" + ViewData["BrowseDetailName"] + "', 'CriteriaDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Criteria") + "', '" + ViewData["Scope"] + "', 'CriteriaG', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.Sort, new { onClick = "javascript: newEntity('" + Url.Action("EditDetail", "Criteria") + "', '" + ViewData["Scope"] + "', 'Criteria', 'CriteriaDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <input class="button" type="button" value="Search" onclick="javascript: searchEntity('<%= ViewData["Scope"] %>', 'CriteriaIndustry', 'CriteriaIndustryDetail');" />
            <input class="button" type="button" value="Filter" onclick="javascript: filterEntity('<%= ViewData["Scope"] %>', 'CriteriaIndustry', 'CriteriaIndustryDetail');" />
            <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateDetailEntity('" + Url.Action("Duplicate", "Criteria") + "', '" + ViewData["Scope"] + "', 'IndustryCriteria', 'CriteriaIndustryDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>
            <%--<%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DuplicateDetail, new { onClick = "javascript: duplicateEntity('" + Url.Action("Duplicate", "Criteria") + "', '" + ViewData["Scope"] + "', 'IndustryCriteria', 'CriteriaIndustryDetail', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');" })%>--%>
            <%--<input class="button" type="button" value="Duplicate" onclick="javascript: duplicateDetailEntity('<%=Url.Action("Duplicate", "Criteria") %>',  '<%= ViewData["Scope"] %>','IndustryCriteria', 'CriteriaIndustryDetail','<%= ViewData["Container"] %>','<%= ViewData["HeaderType"] %>','<%= ViewData["DetailFilter"] %>');" />--%>
            <input class="button" type="button" value="Multi Add" onclick="javascript: Step2();" />
        </asp:Panel>
        <div class="contentDetailData" style="height: auto;" >
            <%=Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] }) %>
        </div>
    </div>
    <asp:Panel ID="CriteriaDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>CriteriaEditFormContent" />
    </asp:Panel>
    <%--    <asp:Panel ID="CriteriaDetailFormContent" runat="server">
        <div id="<%= ViewData["Scope"] %>CriteriaEditFormContent" />
    </asp:Panel>
    <asp:Panel ID="CriteriaIndustryDetailFilterContent" runat="server">
    <%= Html.GridFilter("CriteriaIndustryDetail") %>
    </asp:Panel>--%>
    <asp:Panel ID="CriteriaIndustryDetailSearchContent" runat="server">
        <%= Html.GridSearch("CriteriaIndustryDetail")%>
    </asp:Panel>
    <asp:Panel ID="CriteriaIndustryDetailFilterContent" runat="server">
        <%= Html.GridFilter("CriteriaIndustryDetail")%>
    </asp:Panel>
</div>