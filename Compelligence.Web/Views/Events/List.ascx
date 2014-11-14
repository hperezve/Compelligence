<%@  Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>


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
		}
	);
	

</script>
<%--<script type="text/javascript">
$(
 function()
 {
  //we will be change to toggle
  $("#myGrid tbody tr").toggle(
   function()
   { //if ( isCtrl )
    $(this).addClass("highlight");
   },
   function()
   {
    $(this).removeClass("highlight");
   }
  );
  //$("#myGrid tbody tr").click(function () {alert('Click on Row');});
  
 }
)

</script>--%>

<div>
    <div class="headerContentRightMenu">    
     <%if (ViewData["ShowAll"] == null || ViewData["ShowAll"].Equals("no"))
       { %>
		<asp:Literal ID="LtLastEvents" runat="server" Text="<%$ Resources:LabelResource, EventsListUpcomingEvents %>"/>
	<%}
       else
       { %>
       <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:LabelResource, EventsListPastEvents %>"/>
	<%} %>
    </div>
    <div class="contentRightMenu">
    <%=Html.StaticGrid("myGrid", "Events", "SingleHeader", new string[] { "Edit,EventEdit", "Close,EventClose" }, new string[] { "Opened By,OpenedBy", "Confidence,Confidence", "Severity,Severity", "Start Date,StartDate" }, new string[] { "&nbsp;,EventComment", "<img src='" + Url.Content("~/Content/Images/Icons/testfeedback.gif") + "' width='22px' title='Add private feedback'/>&nbsp;,EventFeedBack" }, "350,280,150,150,200,0px", Url.Content("~/Content/Images/Icons/CommentWhite.png"), ViewData["DefaultsDisabPublComm"].ToString())%>
	</div>

</div>