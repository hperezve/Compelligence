<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPages/frontendsite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/FrontEndSite.css") %>" rel="stylesheet"
        type="text/css" />
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/hovertip.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/comparinator.css") %>" rel="stylesheet" type="text/css" />
    
    <title>Compelligence - Comparinator</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Scripts" runat="server">

   <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>

   <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>
   <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
   <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

   <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/System/FrontEnd/hovertip.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/System/FrontEnd/FeedBack.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Toggle.js") %>" type="text/javascript"></script>
   <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comparinator.js") %>" type="text/javascript"></script>

 <script type="text/javascript">

     var loadContent = function(urlAction, target) {
         showLoadingDialog();
         $('#' + target).load(urlAction, {}, function() { hideLoadingDialog(); });

     };

     function RemoveCompetitor(urlAction, target) {
         $.get(urlAction, {}, function(data) { $('#' + target).html(data); });
         $('#FormResults').empty();
     }

    
    </script>
    
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/CascadingDropDown.js") %>">  </script>


    <script type="text/javascript">
        $(function() {
        $('#IndustryId').prop("selectedIndex", 0);
        });
        $(function() {
            $('#MainContent').css("width", "100%");
        });
        
    </script>
    
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Comparinator for Companies</h1>
    <hr />
    <br />
  <% using (Ajax.BeginForm("AddCompetitor", "Comparinator", new AjaxOptions
     {
       HttpMethod ="POST",
       UpdateTargetId ="FormCompetitors",
       LoadingElementId ="Messages",
      }, new { id="CompetitorForm"}))
     { %>

    <div id="navigationbarComparinator">
        <label for="Industry">
            <asp:Literal ID="Industry" runat="server" Text="" /><%=ViewData["IndustryLabel"]%>:<%--<img
                src="<%= Url.Content("~/content/Images/Icons/IndustrySmallIcon.png") %>" />--%></label>
        <%= Html.CascadingParentDropDownList("IndustryId", (SelectList)ViewData["cmbIndustry"], string.Empty, Url.Action("GetCompetitorsByIndustry", "Comparinator"), "CompetitorId", new string[] { "ProductId" })%>
        <label for="Competitor">
            <asp:Literal ID="Competitor" runat="server" Text="" /><%=ViewData["CompetitorLabel"]%>:<%--<img
                src="<%= Url.Content("~/content/Images/Icons/CompetitorSmallIcon.png") %>" />--%></label>
        <%= Html.CascadingParentDropDownList("CompetitorId", (SelectList)ViewData["cmbCompetitor"], string.Empty, true, Url.Action("GetProductsByCompetitor", "Comparinator"), new string[] { "IndustryId" }, "ProductId")%>

        <input type="submit" class="longButton" value="Add to Compare" />
    <% } %>
    </div>
    <div id="FormCompetitors">    </div>
    <div id="FormResults">        </div>
        
    <div id="FormConfirm" title="Confirm deletion?" class="displayNone">
      <p>
      <span class="ui-icon ui-icon-alert confirmDialog"></span>These comment will be deleted. Are you sure?</p>
    </div>
    <div id="FormComments">   </div>
    <div id="FormMessage">    </div>  
    <div id="FormFeedBack">   </div>    
    <div id="MessageBox">     </div>
    <div id="MessageStatus">     </div>

</asp:Content>

<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
 </asp:Content>
