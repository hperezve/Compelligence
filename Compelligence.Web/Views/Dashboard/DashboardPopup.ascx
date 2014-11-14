<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<System.Web.UI.DataVisualization.Charting.Chart>>" %>
<%@ Register assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Panel ID="DashboardPopupContentDiv" runat="server">
<div class="tblOne" style="float:left; padding:5px">
     <%foreach (var chart in (IList<string>) ViewData["charts"])
       { %>
     <%= chart %>

     <% } %>  
     <%--<% var charts = (IList<string>)ViewData["charts"];
        var chart = charts[0];  %>
        
        <%= chart %>--%>
</div>
</asp:Panel>