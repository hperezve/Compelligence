<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    CriteriaGroupSubtabs = new Ext.TabPanel({
                renderTo: 'CriteriaGroupContent',
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
                    { contentEl: '<%= ViewData["Scope"] %>CriteriaGroupEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>CriteriaGroupEditFormContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Criterias</u> > <u>Criteria Group</u> > Header";
                        }
                        }
                    }
//                     ,
//                    { contentEl: '<%= ViewData["Scope"] %>CriteriaGroupIndustryContent', title: 'Industries', id: '<%= ViewData["Scope"] %>CriteriaGroupIndustryContent',
//                        listeners: { activate: function() {
//                        document.getElementById("breadcrumb").innerHTML = "<u>Environment</u> > <u>Criterias</u> > <u>Criteria Group</u> > Industries";
//                                loadDetailList('<%= Url.Action("GetDetails", "CriteriaGroup") %>', getIdValue('<%= ViewData["Scope"] %>', 'CriteriaGroup'), 
//                                '<%= ViewData["Scope"] %>CriteriaGroup', '<%= (int) DetailType.Industry %>', '#<%= ViewData["Scope"] %>CriteriaGroupIndustryContent'); 
//                            } }}
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>CriteriaGroupList');
	    });
</script>

<asp:Panel ID="CriteriaGroupListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CriteriaGroupList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="CriteriaGroupFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>CriteriaGroupEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>CriteriaGroupIndustryContent" class="x-hide-display" />
</asp:Panel>
