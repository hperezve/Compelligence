<%@ Page Title="Compelligence - Respond" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Quiz quiz = (Quiz)ViewData["Quiz"]; %>
    <h1>
        <%=Html.Encode(quiz.TargetType) %> - <%= Html.Encode(quiz.Title) %></h1>
    <hr />
    <br />
    <div class="tblOne backLongSurvey">
        <% string message = (string)ViewData["ErrorMessage"]; %>
        <% if (!string.IsNullOrEmpty(message)) { %>
           <span class="marginTextSurvey" style="color:red"><%= Html.Encode(message) %></span>
        <% } else { %>
            <span class="marginTextSurvey">Answer the following questions</span>
        <% } %>
        <div id="LongSurveyForm">
        <% using (Html.BeginForm("FrontEndRespond", "Answer",
            FormMethod.Post, new { id = "QuizForm", ENCTYPE = "multipart/form-data" }))
        { %>
        <input type="hidden" id="QuizId" name="Id" value="<%= quiz.Id %>" />
        <table>
        <%  foreach (Question question in quiz.Questions) { %>
        <tr>
            <td>
               <% if (ViewData["PrimaryIndustryList"] != null || ViewData["PrimaryCompetitorList"] != null)
                       { %>
                <%=Html.QuestionControl(quiz, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"], (SelectList)ViewData["PrimaryIndustryList"], (SelectList)ViewData["PrimaryCompetitorList"])%>
                 <% }
                       else
                       { %>
                       <%=Html.QuestionControl(quiz, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"])%>
                       <% } %>
                       
            </td>
        </tr>
        <%} %>
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <%=Html.MultiUploadControl() %>
            </td>
        </tr>
        </table>
        <div class="padding10">
            <input class="shortButton" id="Submit1" type="submit" value="Submit" />
            <input class="shortButton" type="button" value="Cancel" onclick="location.href='<%=Url.Action("Index","ContentPortal") %>'" />
        </div>
        <%} %>
        </div>
    </div>
</asp:Content>
