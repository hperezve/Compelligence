<%@  Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<style type="text/css">
  
 	#contentleft
 	{
 	 width: 70%;
 	 
 	 }
</style>    


<link href="<%= Url.Content("~/Content/Styles/tinytable.style.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/tinytable.script.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(
		function() {
		    $("#myGrid tbody tr").click
		    (
               function() { //if ( isCtrl )
                   $("#myGrid tbody tr").removeClass("highlight");
                   $(this).addClass("highlight");
               }
             );
               $('#Literal1').text('Some Value Here ')
		}
	);
      
</script>

<div>
  
  <div class="headerContentRightMenu">
  <%--Last Deals  --%>
 <%if (ViewData["ShowArchived"] == null || ViewData["ShowArchived"].Equals("no"))
   { %>
<asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:LabelResource, DealSupportListLastDeals %>" />
  <%}
   else
   {%>
    <asp:Literal ID="Literal2"  runat="server" Text="<%$ Resources:LabelResource, DealSupportListArchiveDeals %>" />
  <%} %>
  </div>
   <div class="contentRightMenu">
   <% bool DefaultsDisabPublComm = Convert.ToBoolean(ViewData["DefaultsDisabPublComm"].ToString());
      if (DefaultsDisabPublComm)
      {
       %>
        <%=Html.StaticGrid("myGrid", "DealSupport", "SingleHeader", new string[] { "Edit,DealEdit", "Close,DealClose" }, new string[] { "Opened By,OpenedBy", "Due Date,DueDate", ViewData["CompetitorLabel"] + ",CompetitorName", ViewData["IndustryLabel"] + ",IndustryName" }, new string[] { ",DealComment" }, "380,250,250,200,150,0px", Url.Content("~/Content/Images/Icons/CommentWhite.png"), ViewData["DefaultsDisabPublComm"].ToString())%>	
	<% }
      else
      { %>
        <%=Html.StaticGrid("myGrid", "DealSupport", "SingleHeader", new string[] { "Edit,DealEdit", "Close,DealClose" }, new string[] { "Opened By,OpenedBy", "Due Date,DueDate", ViewData["CompetitorLabel"] + ",CompetitorName", ViewData["IndustryLabel"] + ",IndustryName" }, new string[] { ",DealComment" }, "300,250,250,200,150,80,0px", Url.Content("~/Content/Images/Icons/CommentWhite.png"))%>
	<% } %>
   </div>    
</div>


