<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>

<script type="text/javascript">
	    $(function() {
	    // WinLoss subtabs
	        WinLossSubtabs = new Ext.TabPanel({
                renderTo: 'WinLossContent',
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
                    { contentEl: '<%= ViewData["Scope"] %>WinLossEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>WinLossEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Win Loss</u>  > Header";
                        }
                        }
                     },
                    { contentEl: '<%= ViewData["Scope"] %>WinLossQuestionContent', title: 'Question', id: '<%= ViewData["Scope"] %>WinLossQuestionContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Win Loss</u>  > Question";
                        loadDetailList('<%= Url.Action("GetDetails", "WinLoss") %>', getIdValue('<%= ViewData["Scope"] %>', 'WinLoss'),
                            '<%= ViewData["Scope"] %>WinLoss', '<%= (int) DetailType.Question %>', '#<%= ViewData["Scope"] %>WinLossQuestionContent');
                        } }},
                      { contentEl: '<%= ViewData["Scope"] %>WinLossAnswerContent', title: 'Results', id: '<%= ViewData["Scope"] %>WinLossAnswerContent',
                          listeners: { activate: function() {
                          document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Win Loss</u>  > Results";
                            loadDetailList('<%= Url.Action("GetDetails", "WinLoss") %>', getIdValue('<%= ViewData["Scope"] %>', 'WinLoss'),
                            '<%= ViewData["Scope"] %>WinLoss', '<%= (int) DetailType.Respond %>', '#<%= ViewData["Scope"] %>WinLossAnswerContent');
                        }}
                    },
                     { contentEl: '<%= ViewData["Scope"] %>WinLossResultContent', title: 'Respond', id: '<%= ViewData["Scope"] %>WinLossResultContent',
                         listeners: { activate: function() {
                         document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Win Loss</u> > Respond";
                            loadContentDetail('<%= Url.Action("ShowQuiz", "Answer") %>?QuizId=' + getIdValue('<%= ViewData["Scope"] %>', 'WinLoss'), '#<%= ViewData["Scope"] %>WinLossResultContent', '<%= ViewData["Scope"] %>','WinLoss');
                             //loadDetailList('<%= Url.Action("GetDetails", "WinLoss") %>', getIdValue('<%= ViewData["Scope"] %>', 'WinLoss'),
                             //'<%= ViewData["Scope"] %>WinLoss', '<%= (int) DetailType.Results %>', '#<%= ViewData["Scope"] %>WinLossResultContent');              
                         }
                         }
                     },   
                    {contentEl: '<%= ViewData["Scope"] %>WinLossQuizClassificationContent', title: 'Classification', id: '<%= ViewData["Scope"] %>WinLossQuizClassificationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Tools</u> > <u>Win Loss</u>  > Classification";
                                loadDetailList('<%= Url.Action("GetDetails", "WinLoss") %>', getIdValue('<%= ViewData["Scope"] %>', 'WinLoss'), 
                                '<%= ViewData["Scope"] %>WinLoss', '<%= (int) DetailType.QuizClassification %>', '#<%= ViewData["Scope"] %>WinLossQuizClassificationContent'); 
                            } }}
                ]
            });
            resizeContent('#<%= ViewData["Scope"] %>WinLossList');
	    });
</script>

<asp:Panel ID="WinLossListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>WinLossList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="WinLossFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>WinLossEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>WinLossQuestionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>WinLossAnswerContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>WinLossResultContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>WinLossQuizClassificationContent" class="x-hide-display" />
</asp:Panel>
