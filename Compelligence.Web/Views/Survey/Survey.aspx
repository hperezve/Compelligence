<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndPopupSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Survey Questions and Answers</title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/jquery.searchFilter.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.jqgrid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.multiselect.css") %>" rel="stylesheet"  type="text/css" />    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <script type="text/javascript">
        function GoDown(urlAction) {
            window.location = urlAction;
        };
        var openPopup = function(url) {
            var popupWindow = window.open(url, 'PopupWindow', 'width=700,height=400,scrollbars=yes');

            if (window.focus) {
                popupWindow.focus()
            }
        };
    </script>
    <style type ="text/css">
body
{
background-color:White;
}
/* TABLE
 * ========================================================================= */
table {
  /* border: 1px solid #888; */
  border-collapse: collapse;
  line-height: 1;
  /*margin: 1em auto;*/
  width: auto;
}

table span {
  background-position: center left;
  background-repeat: no-repeat;
  padding: .1em 0 .1em 1.2em;
}
field
{
    float: left;
    margin-left:10px;
    padding:  7px 25px 5px 0px;
    width: 255px;
}
 line {
   display: table-row;
   clear: both;
   float: left;
   clear: both;
   width: 98%;
   /*margin-left: 30px;*/
}
label {
  vertical-align:middle;
  padding-top: 2px;
}
input[type=radio], input.radio {
  vertical-align:middle;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <% Quiz survey = (Quiz)ViewData["Survey"]; %>
        <%if (survey != null)
          { %>
        <h1 style="margin-left:45px; color:black" ><%= survey.Title%></h1>
        <div id="LongSurveyForm">
            <table style="margin-left:45px; border:1px solid black">
                <% 
            IList<AnswerDTO> answerList = (IList<AnswerDTO>)ViewData["AnswerList"];
            foreach (AnswerDTO answerItem in answerList)
            {
                %>
                <tr>
                    <td style="border: 1px solid black">
                        <%=Html.QuestionControl(answerItem.Question, answerItem.Answer)%>
                    </td>
                </tr>
                <%}%>
            </table>
        </div>
        <div>
        <%IList<File> fileList = (List<File>)ViewData["FileList"];
          if (fileList != null && fileList.Count > 0)
          { %>
         <h3 style="margin-left:45px; color:black" >File attachments:</h3> 
         <table style="margin-left:45px;" >
         <% 
            foreach (File file in fileList)
            {%>
              <tr>
                    <td >
                        <%=file.FileName%>
                    </td>
                    <td>
                        <img class="library-toolbox" src="<%=Url.Content("~/Content/Images/Icons/library-view.png") %>" title="See File" onclick="javascript: openPopup('<%= Url.Action("ShowFileById", "Library") + "/" + file.Id %>'); return false;" /> 
                    </td>
                    <td>
                        <img class="library-toolbox" src="<%=Url.Content("~/Content/Images/Icons/library-download.png") %>" onclick="GoDown('<%= Url.Action("DownloadFileMailExecute", "Survey") + "/" + file.Id %>');return false;"  title="Download library file"/>
                    </td>
                </tr>
         <% }
          %>
         </table>
         <%} %>
        </div>
        
        <%} %>
</asp:Content>
