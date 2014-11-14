<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	        PartnerSubtabs = new Ext.TabPanel({
                renderTo: 'PartnerContent',
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
                    { contentEl: '<%= ViewData["Scope"] %>PartnerEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>PartnerEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Partners</u> > Header";
                        }
                        }
                     },
                    {contentEl:'<%= ViewData["Scope"] %>PartnerEmployeeContent', title: 'People', id: '<%= ViewData["Scope"] %>PartnerEmployeeContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Partners</u> > People";
                            loadDetailList('<%= Url.Action("GetDetails", "Partner") %>', getIdValue('<%= ViewData["Scope"] %>', 'Partner'), 
                            '<%= ViewData["Scope"] %>Partner', '<%= (int) DetailType.Employee %>', '#<%= ViewData["Scope"] %>PartnerEmployeeContent'); 
                        } }},
                    {contentEl:'<%= ViewData["Scope"] %>PartnerLocationContent', title: 'Location', id: '<%= ViewData["Scope"] %>PartnerLocationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Partners</u> > Location";
                            loadDetailList('<%= Url.Action("GetDetails", "Partner") %>', getIdValue('<%= ViewData["Scope"] %>', 'Partner'), 
                            '<%= ViewData["Scope"] %>Partner', '<%= (int) DetailType.Location %>', '#<%= ViewData["Scope"] %>PartnerLocationContent'); 
                        } }},
                        { contentEl: '<%= ViewData["Scope"] %>PartnerTeamContent', title: 'Team', id: '<%= ViewData["Scope"] %>PartnerTeamContent',
                            listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Partners</u> > Team";
                            loadDetailList('<%= Url.Action("GetDetails", "Partner") %>', getIdValue('<%= ViewData["Scope"] %>', 'Partner'),
                            '<%= ViewData["Scope"] %>Partner', '<%= (int) DetailType.Team %>', '#<%= ViewData["Scope"] %>PartnerTeamContent');
                        }} },
                        {contentEl: '<%= ViewData["Scope"] %>PartnerBudgetContent', title: 'Budget', id: '<%= ViewData["Scope"] %>PartnerBudgetContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Partner</u> > Budget";
                            loadDetailList('<%= Url.Action("GetDetails", "Partner") %>', getIdValue('<%= ViewData["Scope"] %>', 'Partner'),
                            '<%= ViewData["Scope"] %>Partner', '<%= (int) DetailType.Budget %>', '#<%= ViewData["Scope"] %>PartnerBudgetContent');
                        }}}, 
                        {contentEl: '<%= ViewData["Scope"] %>PartnerDiscussionContent', title: 'Discussion', id: '<%= ViewData["Scope"] %>PartnerDiscussionContent',
                            listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Partners</u> > Discussion";
                               loadDetailList('<%= Url.Action("GetDetails", "Partner") %>', getIdValue('<%= ViewData["Scope"] %>', 'Partner'), 
                            '<%= ViewData["Scope"] %>Partner', '<%= (int) DetailType.Discussion %>', '#<%= ViewData["Scope"] %>PartnerDiscussionContent');
                        }}},  
                    {contentEl: '<%= ViewData["Scope"] %>PartnerEntityRelationContent', title: 'Related', id: '<%= ViewData["Scope"] %>PartnerEntityRelationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Partners</u> > Related";
                               loadDetailList('<%= Url.Action("GetDetails", "Partner") %>', getIdValue('<%= ViewData["Scope"] %>', 'Partner'), 
                            '<%= ViewData["Scope"] %>Partner', '<%= (int) DetailType.EntityRelation %>', '#<%= ViewData["Scope"] %>PartnerEntityRelationContent');
                        }}},
                    {contentEl: '<%= ViewData["Scope"] %>PartnerLibraryContent', title: 'Library', id: '<%= ViewData["Scope"] %>PartnerLibraryContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Partners</u> > Library";
                            loadDetailList('<%= Url.Action("GetDetails", "Partner") %>', getIdValue('<%= ViewData["Scope"] %>', 'Partner'),
                            '<%= ViewData["Scope"] %>Partner', '<%= (int) DetailType.Library %>', '#<%= ViewData["Scope"] %>PartnerLibraryContent');
                        }}}
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>PartnerList');
	    });
</script>

<asp:Panel ID="PartnerListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>PartnerList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="PartnerFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>PartnerEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>PartnerEmployeeContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>PartnerLocationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>PartnerTeamContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>PartnerBudgetContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>PartnerDiscussionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>PartnerEntityRelationContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>PartnerLibraryContent" class="x-hide-display" />
</asp:Panel>
