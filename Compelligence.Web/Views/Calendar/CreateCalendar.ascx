<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

<asp:Panel ID="CreateCalendar" runat="server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<link href="<%= Url.Content("~/Content/Styles/calendartheme/theme.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Url.Content("~/Content/Styles/fullcalendar.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/BackEnd/fullcalendar.min.js") %>" type="text/javascript"></script>


<style type='text/css'>

	body {
		margin-top: 40px;
		text-align: center;
		font-size: 14px;
		font-family: "Lucida Grande",Helvetica,Arial,Verdana,sans-serif;
		overflow:auto;
		_overflow-x:scroll;
		}

	#calendar {
		width: 700px;
		margin: 0 auto;
		}

</style>
</head>
<body>
<% IList<Compelligence.Domain.Entity.Calendar> calendarList = (List<Compelligence.Domain.Entity.Calendar>)ViewData["Calendars"];

   if (calendarList != null && calendarList.Count > 0)
   { %>
<%=Html.MakeCalendar("#calendar", (IList<Compelligence.Domain.Entity.Calendar>)ViewData["Calendars"])%>
<%} %>
<div id='calendar'></div>  


</body>
</html>
</asp:Panel>

