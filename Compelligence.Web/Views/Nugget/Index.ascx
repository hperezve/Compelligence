<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
	    $(function() {
	    // Nugget subtabs
	        NuggetSubtabs = new Ext.TabPanel({
                renderTo: 'NuggetContent',
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
                    { contentEl: '<%= ViewData["Scope"] %>NuggetEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>NuggetEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Nugget</u>  > Header";
                        }
                        }
                     },
                    { contentEl: '<%= ViewData["Scope"] %>NuggetQuestionContent', title: 'Question', id: '<%= ViewData["Scope"] %>NuggetQuestionContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Nugget</u>  > Question";
                        loadDetailList('<%= Url.Action("GetDetails", "Nugget") %>', getIdValue('<%= ViewData["Scope"] %>', 'Nugget'),
                            '<%= ViewData["Scope"] %>Nugget', '<%= (int) DetailType.Question %>', '#<%= ViewData["Scope"] %>NuggetQuestionContent');
                        } }},
                    {contentEl: '<%= ViewData["Scope"] %>NuggetQuizClassificationContent', title: 'Classification', id: '<%= ViewData["Scope"] %>NuggetQuizClassificationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Nugget</u>  > Classification";
                                loadDetailList('<%= Url.Action("GetDetails", "Nugget") %>', getIdValue('<%= ViewData["Scope"] %>', 'Nugget'), 
                                '<%= ViewData["Scope"] %>Nugget', '<%= (int) DetailType.QuizClassification %>', '#<%= ViewData["Scope"] %>NuggetQuizClassificationContent'); 
                            } }}
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>NuggetList');
	    });
</script>

<asp:Panel ID="NuggetListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NuggetList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="NuggetFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NuggetEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>NuggetQuestionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>NuggetQuizClassificationContent" class="x-hide-display" />
</asp:Panel>
