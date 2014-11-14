<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    var showIndustryByHierarchy = function(invoke, scope, entity, entityall, assignedTo, userId) {
        if (invoke.checked) {
            reloadMyGrid(scope, entity);
            $('#' + scope + 'gridIndustryOverflow').css("display", "none");
            $('#'+scope+'gridHierarchyOverflow').css("display", "block");
            $('#' + entity + 'AddIndustry').css("display", "none");
            $('#' + entity + 'AddIndustryHierarchy').css("display", "inline");
            $('#' + entity + 'DeleteIndustry').css("display", "none");
            $('#' + entity + 'DeleteIndustryHierarchy').css("display", "inline");
        }
        else {
            reloadMyGrid(scope, entity);
            $('#' + scope + 'gridIndustryOverflow').css("display", "block");
            $('#' + scope + 'gridHierarchyOverflow').css("display", "none");
            $('#' + entity + 'AddIndustry').css("display", "inline");
            $('#' + entity + 'AddIndustryHierarchy').css("display", "none");
            $('#' + entity + 'DeleteIndustry').css("display", "inline");
            $('#' + entity + 'DeleteIndustryHierarchy').css("display", "none");
        }
    };
    var reloadMyGrid = function(scope, browsid) {
        RefreshGridId(scope, browsid);
        RefreshGridId(scope, browsid + 'Hierarchy');
        resizeGrid(scope + browsid + 'Hierarchy');
    };
</script>
<script type="text/javascript">

    $(function() {
    if ('<%= ViewData["BrowseDetailName"]%>' != 'IndustryTrendDetail') {
            IndustryDetailSubtabs = new Ext.TabPanel({
                renderTo: '<%= ViewData["Scope"] %>IndustryDetailContent',
                activeTab: 0,
                tabPosition: 'bottom',
                autoWidth: true,
                frame: true,
                defaults: { autoHeight: true },
                listeners: {
                    render: function(tabPanel) {

                        if ('<%= ViewData["HeaderType"] %>' == '<%= DomainObjectType.Product%>') {

                            tabPanel.hideTabStripItem('<%= ViewData["Scope"] %>DetailCompetitorComparinator');

                        } else if ('<%= ViewData["HeaderType"] %>' == '<%= DomainObjectType.Competitor%>') {

                            tabPanel.hideTabStripItem('<%= ViewData["Scope"] %>DetailProductComparinator');

                        }
                    },
                    beforetabchange: function(tabPanel, newTab, currentTab) {

                        if (newTab.getId() != '<%= ViewData["Scope"] %>IndustryDetailDataListContent') {

                            if (getSelectedRowGrid('#<%= ViewData["Scope"].ToString() + ViewData["BrowseDetailName"].ToString() %>ListTable') == null) {

                                showAlertSelectItemDialog();

                                return false;
                            }
                        }
                    }
                },
                items: [
                    { contentEl: '<%= ViewData["Scope"] %>IndustryDetailDataListContent', title: 'List', id: '<%= ViewData["Scope"] %>IndustryDetailDataListContent' },
                    { contentEl: '<%= ViewData["Scope"] %>DetailProductComparinator', title: 'Product Criteria', id: '<%= ViewData["Scope"] %>DetailProductComparinator',
                        listeners: {
                            activate: function() {
                                loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getSelectedRowGrid('#<%= ViewData["Scope"].ToString() + ViewData["BrowseDetailName"].ToString() %>ListTable'),
                                    '<%= ViewData["Scope"] %>', '<%= (int) DetailType.ProductCriteria %>', '#<%= ViewData["Scope"] %>DetailProductComparinator', '<%= ViewData["DetailFilter"] %>');
                            }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>DetailCompetitorComparinator', title: 'Competitor Criteria', id: '<%= ViewData["Scope"] %>DetailCompetitorComparinator',
                        listeners: {
                            activate: function() {
                                loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getSelectedRowGrid('#<%= ViewData["Scope"].ToString() + ViewData["BrowseDetailName"].ToString() %>ListTable'),
                                    '<%= ViewData["Scope"] %>', '<%= (int) DetailType.CompetitorCriteria %>', '#<%= ViewData["Scope"] %>DetailCompetitorComparinator', '<%= ViewData["DetailFilter"] %>');
                            }
                        }
                    }
                    ,
                //                    { contentEl: '<%= ViewData["Scope"] %>DetailPositioning', title: 'Positioning', id: '<%= ViewData["Scope"] %>DetailPositioning',
                //                        listeners: { 
                //                            activate: function() {
                //                                loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getSelectedRowGrid('#<%= ViewData["Scope"].ToString() + ViewData["BrowseDetailName"].ToString() %>ListTable'), 
                //                                    '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Positioning %>', '#<%= ViewData["Scope"] %>DetailPositioning', '<%= ViewData["DetailFilter"] %>');
                //                        }
                //                        }
                //                    }
                //                    ,
                    {contentEl: '<%= ViewData["Scope"] %>DetailImplication', title: 'Implication', id: '<%= ViewData["Scope"] %>DetailImplication',
                    listeners: {
                        activate: function() {
                            loadDetailList('<%= Url.Action("GetDetails", "Industry") %>', getSelectedRowGrid('#<%= ViewData["Scope"].ToString() + ViewData["BrowseDetailName"].ToString() %>ListTable'),
                                    '<%= ViewData["Scope"] %>', '<%= (int) DetailType.Implication %>', '#<%= ViewData["Scope"] %>DetailImplication', '<%= ViewData["DetailFilter"] %>');
                        }
                    }
                }
                ]
            });
        }
    });
</script>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>');
        resizeGrid('<%= ViewData["Scope"]%>' + '<%=ViewData["BrowseDetailName"].ToString()%>' + 'Hierarchy');
    }).trigger('resize');
</script>
<div class="indexTwo">
    <div id="<%= ViewData["Scope"] %>IndustryDetailContent">
        <div id="<%= ViewData["Scope"] %>IndustryDetailDataListContent" >
            <asp:Panel ID="IndustryDetailToolbarContent" runat="server" CssClass="buttonLink">
                <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { id = ViewData["BrowseDetailName"]  + "AddIndustry", onClick = "javascript: addIndustry('" + Url.Action("CreateDetail", "Industry") + "', '" + ViewData["Scope"] + "', 'Industry', '" + ViewData["BrowseDetailName"] + "', 'IndustryDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');", style = "display:in-line;" })%>
                <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.AddDetail, new { id = ViewData["BrowseDetailName"] + "AddIndustryHierarchy", onClick = "javascript: addIndustry('" + Url.Action("CreateDetail", "Industry") + "', '" + ViewData["Scope"] + "', 'Industry', '" + ViewData["BrowseDetailName"] + "Hierarchy', 'IndustryDetailSelect', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"].ToString().Replace(ViewData["BrowseDetailName"].ToString(),ViewData["BrowseDetailName"].ToString() + "Hierarchy") + "');", style = "display:none;" })%>
                <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { id = ViewData["BrowseDetailName"] + "DeleteIndustry", onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Industry") + "', '" + ViewData["Scope"] + "', 'Industry', '" + ViewData["BrowseDetailName"] + "', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"] + "');", style = "display:in-line;" })%>
                <%= Html.SecurityButton(SecurityButtonType.Button, SecurityButtonAction.DeleteDetail, new { id = ViewData["BrowseDetailName"] + "DeleteIndustryHierarchy", onClick = "javascript: deleteDetailEntity('" + Url.Action("DeleteDetail", "Industry") + "', '" + ViewData["Scope"] + "', 'Industry', '" + ViewData["BrowseDetailName"] + "Hierarchy', '" + ViewData["Container"] + "', '" + ViewData["HeaderType"] + "', '" + ViewData["DetailFilter"].ToString().Replace(ViewData["BrowseDetailName"].ToString(), ViewData["BrowseDetailName"].ToString() + "Hierarchy") + "');", style = "display:none;" })%>
                 <% if (ViewData["BrowseDetailName"].ToString().IndexOf("Competitor") != -1 || ViewData["BrowseDetailName"].ToString().IndexOf("Product") != -1 || ViewData["BrowseDetailName"].ToString().IndexOf("Trend") != -1 || ViewData["BrowseDetailName"].ToString().IndexOf("Customers") == -1)
                    { %>
                <input id="<%= ViewData["Scope"] %>IndustryHierarchyCheckbox" class="checkbox" type="checkbox"
                onclick="javascript:showIndustryByHierarchy(this,'<%= ViewData["Scope"] %>', '<%= ViewData["BrowseDetailName"] %>', '<%= ViewData["BrowseDetailName"] %>View', 'AssignedTo' ,'<%= Session["UserId"] %>');" />
            <label for="<%= ViewData["Scope"] %>IndustryHierarchyCheckbox" style="font-size: 12px;
                color: black;">
                <asp:Literal ID="IndustryHierarchyCheckbox" runat="server" Text="<%$ Resources:LabelResource, IndustryHierarchyChecked %>" />
            </label>
            <%} %>
            </asp:Panel>
            <asp:Panel ID="IndustryDetailDataListContent" runat="server">
                <div id="<%= ViewData["Scope"] %>gridIndustryOverflow" class="gridOverflow" style="display: block"><%= Html.DataGrid(ViewData["BrowseDetailName"].ToString(), new { BrowseDetailFilter = ViewData["BrowseDetailFilter"] })%></div>
                <% if (ViewData["BrowseDetailName"].ToString().IndexOf("Competitor") != -1 || ViewData["BrowseDetailName"].ToString().IndexOf("Product") != -1 || ViewData["BrowseDetailName"].ToString().IndexOf("Trend") != -1 || ViewData["BrowseDetailName"].ToString().IndexOf("Customers") == -1)
                   { %>
                <div id="<%= ViewData["Scope"] %>gridHierarchyOverflow" class="gridOverflow" style="display: none;"><%= Html.DataGrid(ViewData["BrowseDetailName"].ToString() + "Hierarchy", "Indusry", string.Empty, new { BrowseDetailFilter = ViewData["BrowseDetailFilter"].ToString().Replace(ViewData["BrowseDetailName"].ToString(), ViewData["BrowseDetailName"].ToString() + "Hierarchy") })%></div>
                <% } %>
            </asp:Panel>
        </div>
        
        <div id="<%= ViewData["Scope"] %>DetailProductComparinator" class="x-hide-display" />
        <div id="<%= ViewData["Scope"] %>DetailCompetitorComparinator" class="x-hide-display" />
        <%--<div id="<%= ViewData["Scope"] %>DetailPositioning" class="x-hide-display" />--%>
        <div id="<%= ViewData["Scope"] %>DetailImplication" class="x-hide-display" />

    </div>
</div>