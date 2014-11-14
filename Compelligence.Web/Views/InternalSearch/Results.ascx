<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.BusinessLogic.Interface" %>
<%@ Import namespace="Compelligence.BusinessLogic.Implementation" %>
<%@ Import namespace="Compelligence.Common.Search" %>
<%@ Import namespace="Compelligence.Common.Browse" %>

<div>
<%
    string sq = (string) ViewData["SearchQuery"];
    string sg = (string)ViewData["Group"];
    IList<SearchObject> sos =(List<SearchObject>) ViewData["SearchObjects"];
    string[] smallItems = {"Deals","Events","Calendars","Objetives","Customers","Win Loss","Source"};
    foreach (var item in sos)
    {
        //(Small), comment next line for full version, decomment for fullversion
        //if (!smallItems.Contains(item.Label))
        {
%>      <div id="TitleSearchResult"><%=item.Label%></div>
        <div style="background-color:#eeeeee">
        <%= Html.DataGridSearch(item.BrowseId, item.SearchId, sq, sg)%>
        </div>
        <div></div>
<%  }
    }
%>
</div>
