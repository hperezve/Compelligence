<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div style="text-align:center">    
  <img width="22px" onclick="FeedBackFormDlg('<%=Url.Action("SendFeedBackBenefit", "Comparinator", new { Id=ViewData["IndustryId"],U=ViewData["U"],C=ViewData["C"]})%>');" title="Add private feedback" src="<%= Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" > 
  <img width="22px" onclick="CellBenefitDlg('<%=ViewData["CriteriaId"] %>', '<%=ViewData["CriteriaDescription"]%>/<%=ViewData["CriteriaName"]%>','<%=Url.Action("SaveBenefit","Comparinator")%>')" title="Edit Benefit" src="<%= Url.Content("~/Content/Images/Icons/properties.png") %>" >    
</div>
