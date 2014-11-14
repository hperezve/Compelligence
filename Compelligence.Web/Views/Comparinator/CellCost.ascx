<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div style="text-align:center;">    

<img width="22px" onclick="FeedBackFormDlg('<%=Url.Action("SendFeedBackCost", "Comparinator", new { Id = ViewData["IndutryId"], U = ViewData["U"], C = ViewData["C"] })%>');" title="Add private feedback" src="<%= Url.Content("~/Content/Images/Icons/testfeedback.gif") %>">    </a>
<img width="22px" onclick="CellCostDlg('<%=ViewData["CriteriaId"] %>', '<%=ViewData["CriteriaDescription"]%>/<%=ViewData["CriteriaName"]%>','<%=Url.Action("SaveCost","Comparinator")%>')" title="Edit Cost" src="<%= Url.Content("~/Content/Images/Icons/properties.png") %>" >    

</div>
