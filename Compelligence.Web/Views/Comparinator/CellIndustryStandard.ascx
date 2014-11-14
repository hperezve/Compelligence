<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div style="text-align:center;">    

<img width="22px" onclick="CellIndustryStandardDlg('<%=ViewData["CriteriaId"] %>', '<%=ViewData["IndustryStandard"]%>','<%=ViewData["CriteriaName"]%>','<%=Url.Action("SaveIndustryStandard","Comparinator")%>')" title="Edit Industry Standard" src="<%= Url.Content("~/Content/Images/Icons/properties.png") %>" >    

</div>