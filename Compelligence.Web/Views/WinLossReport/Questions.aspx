<%@ Page Title="Compelligence - Win/Loss Report" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSiteOnly.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
   
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
   
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/Question.js") %>"></script>
    

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%Quiz winLoss = (Quiz)ViewData["UniqueWinLoss"]; %>
 <%--   <div class="headerTitleLefConteiner"> <b>
        Win/Loss Report -
        <%= Html.Encode(winLoss.Title) %>
        </b>
   </div>--%>
    <br />
    <div class="tblOne backLongSurvey">
        <%string errorMessage = (string)ViewData["ErrorMessage"]; %>
        <% if (!string.IsNullOrEmpty(errorMessage))
           { %>
        <span class="marginTextSurvey" style="color: red">
            <%= Html.Encode(errorMessage) %></span>
        <%}
           else
           { %>
        <span class="marginTextSurvey">Answer the following questions</span>
        <% } %>
        <% using (Html.BeginForm("Answer", "WinLossReport", FormMethod.Post,
         new
         {
             id = "WinLossForm",
             ENCTYPE = "multipart/form-data"
         }))
           { %>
        <input type="hidden" id="QuizId" name="Id" value="<%= winLoss.Id %>" />
        <table>
            <%foreach (Question question in winLoss.Questions)
              {%>
            <tr>
                <td>
                    <%=Html.QuestionControl(winLoss, question, ViewData["Q" + question.Item], (MultiSelectList)ViewData["IndustryIdList"], (MultiSelectList)ViewData["CompetitorIdList"], (MultiSelectList)ViewData["ProductIdList"], (String)ViewData["WinningItem"], (String)ViewData["ProductLabel"], (String)ViewData["CompetitorLabel"], (String)ViewData["IndustryLabel"])%>
                </td>
            </tr>
            <%}%>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>                
            </tr>            
        </table>
        <table>
        <tr>
        <td colspan ="2">
        <%=Html.MultiUploadControl() %>
        </td>
        </tr>
        <tr>
        <td>
        <input class="shortButton" id="FormSubmit" type="submit" value="Submit" />
        </td>
        <td align="right" >
        <input class="shortButton" type="button" value="Cancel" onclick="location.href='<%=Url.Action("Index","ContentPortal")%>'" />
        </td>
        </tr>
        </table>
        <div class="padding10">
            
            
        </div>
        <%} %>
    </div>
    <div id="detailWinLoss" style="display:none"></div>
</asp:Content>
