<%@ Page Title="Compelligence - Survey" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
<%--    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"
        type="text/css" />
--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">


<%--    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"
        type="text/javascript"></script>--%>

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Quiz survey = (Quiz)ViewData["LongSurvey"]; %>
    <h1>
        Survey - <%= Html.Encode(survey.Title) %></h1>
    <hr />
    <br />
    <div class="tblOne backLongSurvey">
        <% string message = (string)ViewData["ErrorMessage"]; %>
        <% if (!string.IsNullOrEmpty(message))
           { %>
           <span class="marginTextSurvey" style="color:red"><%= Html.Encode(message) %></span>
        <% }
           else
           { %>
        <span class="marginTextSurvey">Answer the following questions</span>
        <% } %>
        <div id="LongSurveyForm">
            <% using (Html.BeginForm("Answer", "LongSurvey",
                   FormMethod.Post, new { id = "LongSurveyForm", ENCTYPE = "multipart/form-data" }))
               { %>
            <input type="hidden" id="QuizId" name="Id" value="<%= survey.Id %>" />
            <table>
                <%
                   foreach (Question question in survey.Questions)
                   {
                %>
                <tr>
                    <td>
                        <%=Html.QuestionControl(survey, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdMultiList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"])%>
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
