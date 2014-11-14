<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>

<script type="text/javascript">
    $(function() {
        // Survey subtabs
        SurveySubtabs = new Ext.TabPanel({
            renderTo: 'SurveyContent',
            autoWidth: true,
            frame: true,
            //defaults: { autoHeight: true },
            height:640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>SurveyEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>SurveyEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Survey</u> > Header";
                        }
                        }
                     },
                    { contentEl: '<%= ViewData["Scope"] %>SurveyQuestionContent', title: 'Questions', id: '<%= ViewData["Scope"] %>SurveyQuestionContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Survey</u> > Question";
                            loadDetailList('<%= Url.Action("GetDetails", "Survey") %>', getIdValue('<%= ViewData["Scope"] %>', 'Survey'),
                            '<%= ViewData["Scope"] %>Survey', '<%= (int) DetailType.Question %>', '#<%= ViewData["Scope"] %>SurveyQuestionContent');
                        } 
                        }
                    },
//                    { contentEl: '<%= ViewData["Scope"] %>SurveyHtmlContent', title: 'Html Content', id: '<%= ViewData["Scope"] %>SurveyHtmlContent',
//                        listeners: { activate: function() {
//                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Survey</u> > Html Content";
//                            
//                            loadContent('<%= Url.Action("ShowHtml", "Survey") %>?QuizId=' + getIdValue('<%= ViewData["Scope"] %>', 'Survey'), '#<%= ViewData["Scope"] %>SurveyHtmlContent', '<%= ViewData["Scope"] %>');
//                        }
//                        }
//                    },
                    { contentEl: '<%= ViewData["Scope"] %>SurveyAnswerContent', title: 'Results', id: '<%= ViewData["Scope"] %>SurveyAnswerContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Survey</u> > Results";
                            loadDetailList('<%= Url.Action("GetDetails", "Survey") %>', getIdValue('<%= ViewData["Scope"] %>', 'Survey'),
                            '<%= ViewData["Scope"] %>Survey', '<%= (int) DetailType.Respond %>', '#<%= ViewData["Scope"] %>SurveyAnswerContent');
                        }}
                        },
                    { contentEl: '<%= ViewData["Scope"] %>SurveyResultContent', title: 'Respond', id: '<%= ViewData["Scope"] %>SurveyResultContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Survey</u> > Respond";
                            loadContentDetail('<%= Url.Action("ShowQuiz", "Answer") %>?QuizId=' + getIdValue('<%= ViewData["Scope"] %>', 'Survey'), '#<%= ViewData["Scope"] %>SurveyResultContent', '<%= ViewData["Scope"] %>','Survey');
                            //loadDetailList('<%= Url.Action("GetDetails", "Survey") %>', getIdValue('<%= ViewData["Scope"] %>', 'Survey'),
                            //'<%= ViewData["Scope"] %>Survey', '<%= (int) DetailType.Results %>', '#<%= ViewData["Scope"] %>SurveyResultContent');              
                        }
                        }
                    },
                    { contentEl: '<%= ViewData["Scope"] %>SurveyReorderContent',
                        title: 'Reorder',
                        id: '<%= ViewData["Scope"] %>SurveyReorderContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Survey</u> > Reorder";
                            var Id = getIdValue('<%= ViewData["Scope"] %>', 'Survey');
                            loadDetailList('<%= Url.Action("Reorder", "Survey") %>',Id,'Admin','','#<%= ViewData["Scope"] %>SurveyReorderContent', '');
                        }
                        }
                    },
                    {contentEl: '<%= ViewData["Scope"] %>SurveyQuizClassificationContent', title: 'Classification', id: '<%= ViewData["Scope"] %>SurveyQuizClassificationContent',
                        listeners: { activate: function() {
                        document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>Survey</u> > Classification";
                                loadDetailList('<%= Url.Action("GetDetails", "Survey") %>', getIdValue('<%= ViewData["Scope"] %>', 'Survey'), 
                                '<%= ViewData["Scope"] %>Survey', '<%= (int) DetailType.QuizClassification %>', '#<%= ViewData["Scope"] %>SurveyQuizClassificationContent'); 
                            } }}
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>SurveyList');
    });
</script>

<asp:Panel ID="SurveyListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SurveyList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS"><img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>"/></div>
</asp:Panel>
<br />
<asp:Panel ID="SurveyFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>SurveyEditFormContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SurveyQuestionContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SurveyHtmlContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SurveyAnswerContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SurveyResultContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SurveyReorderContent" class="x-hide-display" />
    <div id="<%= ViewData["Scope"] %>SurveyQuizClassificationContent" class="x-hide-display" />
</asp:Panel>
