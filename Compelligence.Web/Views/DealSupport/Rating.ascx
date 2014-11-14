<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
    <% var Id = (decimal)ViewData["DealId"];
         %>
       <%=Html.FirstRatingStarts(Id, 0, 0, Url.Action("Rating", "DealSupport"), true)%>
  