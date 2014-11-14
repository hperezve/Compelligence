<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%--<link href="<%= Url.Content("~/Content/Styles/tinytable.style.css") %>" rel="stylesheet"
    type="text/css" />
<script src="<%= Url.Content("~/Scripts/tinytable.script.js") %>" type="text/javascript"></script>--%>
<link href="<%= Url.Content("~/Content/Styles/tinytable2.style.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/tinytable.script.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(
		function() {
    //$("#myGrid").addClass("sortable");
//    $("#myGrid thead th tr h3").addClass("tinytable");
//    $("#myGrid h3").addClass("tinytable");
    $("#myGrid tbody th").addClass("sortable");
    $("#myGrid tbody tr").addClass("sortable");
    //$("#myGrid").addClass("tinytable");
		    $('.browseObjectClass').css("color", "Blue");
		    $('.browseObjectClass').mouseover(function() { $(this).css("color", "Black") }).mouseout(function() { $(this).css("color", "Blue") });
		    $("#myGrid tbody tr").click
		    (
               function() { //if ( isCtrl )
                   $("#myGrid tbody tr").removeClass("highlight");
                   $(this).addClass("highlight");
                  
               }
             );
		}
	);
</script>

<% if (!string.IsNullOrEmpty(ViewData["CompetitorName"].ToString()))
   { %>
<div class="headerContentRightMenu" style="margin-right: 5px;">
    <label for="LblLastEvents">
        <%= ViewData["ComparinatorNewsListTitle"].ToString()%>
    </label>
</div>
    <asp:Panel ID="CompetitorNewsDetailDataListContent" runat="server" class="contentRightMenu" style="margin-right: 5px;">
        <%=Html.StaticListGrid("myGrid", "NewsByCompetitorDetail", ViewData["BrowseDetailFilter"].ToString(), ViewData["CompetitorId"].ToString(), ViewData["EntityType"].ToString(), ViewData["ClientCompany"].ToString(), "SingleHeader", new string[] { }, new string[] { }, new string[] { }, "1,60,20,20")%>
    </asp:Panel>
<%} %>