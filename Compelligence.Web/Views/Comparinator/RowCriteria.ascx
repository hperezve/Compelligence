<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.DataTransfer.Comparinator" %>
<%String IndustryId = Convert.ToString(ViewData["IndustryId"]);%>
<%IList<Product> Titles = (IList<Product>)ViewData["Products"]; %>
<%UserProfile user = (UserProfile)ViewData["User"]; %>
<%ComparinatorCriteria ComparinatorCriteria = (ComparinatorCriteria)ViewData["ComparinatorCriteria"]; %>
<%=Html.CriteriaRow(ComparinatorCriteria, user, Titles, IndustryId, false, (string)ViewData["pis"], (string)ViewData["pb"], (string)ViewData["pc"], (string)ViewData["U"], (string)ViewData["C"])%>
