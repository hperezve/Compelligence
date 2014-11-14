<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div><span>Relevancy</span></div>
  <div>
  <input type="radio" value="HIGH" <%=ViewData["RelevancyHIGH"]%> onclick="CallCellRelevancySave('<%= Url.Action("CellRelevancySave", "Comparinator") %>','<%=ViewData["cid"] %>','HIGH')" class="radrel" name="relevancy"/><label class="comp_labels_feature">High</label><br>
  <input type="radio" value="MEDI" <%=ViewData["RelevancyMEDI"]%> onclick="CallCellRelevancySave('<%= Url.Action("CellRelevancySave", "Comparinator") %>','<%=ViewData["cid"] %>','MEDI')" class="radrel" name="relevancy" /><label class="comp_labels_feature">Medium</label><br>
  <input type="radio" value="LOW"  <%=ViewData["RelevancyLOW"]%>  onclick="CallCellRelevancySave('<%= Url.Action("CellRelevancySave", "Comparinator") %>' ,'<%=ViewData["cid"] %>','LOW' )" class="radrel" name="relevancy" /><label class="comp_labels_feature">Low</label><br>
 </div>
