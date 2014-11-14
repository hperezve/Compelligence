<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>

<ul>
    <li class="lineList">
        <%=Html.ActionLink("Comparison for Products", "Index", "Comparinator")%>
    </li>
    <li class="lineList">
        <%=Html.ActionLink("Comparison for Companies", "Index", "Comparinator", new { ComparisonType = ComparisonType.Competitors }, null)%>
    </li>
</ul>
