<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    // Supplier subtabs
	        SupplierSubtabs = new Ext.TabPanel({
                renderTo: 'SupplierContent',
                autoWidth:true,
                frame:true,
                //defaults:{autoHeight: true},
                height:640,
                listeners: {        
                        render: function(tabPanel) {
                            hideSubtabs(tabPanel);
                        }    
                },
                items:[
                    { contentEl: '<%= ViewData["Scope"] %>SupplierEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>SupplierEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Header";
                        }
                        }
                     },
                    {contentEl:'<%= ViewData["Scope"] %>SupplierEmployeeContent', title: 'People', id: '<%= ViewData["Scope"] %>SupplierEmployeeContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > People";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'), 
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Employee %>', '#<%= ViewData["Scope"] %>SupplierEmployeeContent'); 
                        } }},
                    {contentEl: '<%= ViewData["Scope"] %>SupplierIndustryContent', title: 'Industries', id: '<%= ViewData["Scope"] %>SupplierIndustryContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Industries";
                                loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'), 
                                '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>SupplierIndustryContent'); 
                            } }},
                    { contentEl: '<%= ViewData["Scope"] %>SupplierProductContent', title: 'Product', id: '<%= ViewData["Scope"] %>SupplierProductContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Product";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'),
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Product %>', '#<%= ViewData["Scope"] %>SupplierProductContent');
                        }}},
                    { contentEl: '<%= ViewData["Scope"] %>SupplierEventContent', title: 'Event', id: '<%= ViewData["Scope"] %>SupplierEventContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Event";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'),
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Event %>', '#<%= ViewData["Scope"] %>SupplierEventContent');
                        }}},
                    {contentEl:'<%= ViewData["Scope"] %>SupplierLocationContent', title: 'Location', id: '<%= ViewData["Scope"] %>SupplierLocationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Location";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'), 
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Location %>', '#<%= ViewData["Scope"] %>SupplierLocationContent'); 
                        } }},
                    {contentEl: '<%= ViewData["Scope"] %>SupplierTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>SupplierTeamContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'),
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>SupplierTeamContent');
                        }
                        }
                    },
                    {contentEl: '<%= ViewData["Scope"] %>SupplierBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>SupplierBudgetContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Supplier</u> > Budget";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'),
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>SupplierBudgetContent');
                        }}},
                    { contentEl: '<%= ViewData["Scope"] %>SupplierSourceContent', title: 'Source', id: '<%= ViewData["Scope"] %>SupplierSourceContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Source";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'),
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Source %>', '#<%= ViewData["Scope"] %>SupplierSourceContent');
                        }}},  
                    {contentEl: '<%= ViewData["Scope"] %>SupplierEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>SupplierEntityRelationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Related";
                               loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'), 
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>SupplierEntityRelationContent');
                        }}},
                    {contentEl: '<%= ViewData["Scope"] %>SupplierLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>SupplierLibraryContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'),
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>SupplierLibraryContent');
                        }}}, 
                        {contentEl: '<%= ViewData["Scope"] %>SupplierDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>SupplierDiscussionContent',
                            listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Discussion";
                               loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'), 
                            '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>SupplierDiscussionContent');
                        }}},
                    {contentEl: '<%= ViewData["Scope"] %>SupplierCompetitorContent', title: 'Competitor', id: '<%= ViewData["Scope"] %>SupplierCompetitorContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Suppliers</u> > Competitor";
                                loadDetailList('<%= Url.Action("GetDetails", "Supplier") %>', getIdValue('<%= ViewData["Scope"] %>', 'Supplier'), 
                                '<%= ViewData["Scope"] %>Supplier', '<%= (int) DetailType.Competitor %>', '#<%= ViewData["Scope"] %>SupplierCompetitorContent'); 
                            } }}
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>SupplierList');
	    });
</script>

<asp:Panel ID="SupplierListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SupplierList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS">
        <img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>" /></div>
</asp:Panel>
<br />
<asp:Panel ID="SupplierFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SupplierEditFormContent" class="x-hide-display heightSubPanels" />
    <div id="<%= ViewData["Scope"] %>SupplierIndustryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierProductContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierEventContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierEmployeeContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierLocationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierSourceContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierLibraryContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SupplierCompetitorContent" class="x-hide-display" />
</asp:Panel>
