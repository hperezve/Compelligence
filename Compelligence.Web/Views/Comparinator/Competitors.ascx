<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<% var CompetitorCollection = (IList<Competitor>)ViewData["Competitors"]; %>
<% if (CompetitorCollection != null && CompetitorCollection.Count > 0)
   {%>
<br />
<h2>
    Competitors for comparison</h2>
<hr />
<br />
<div id="ComparinatorElements">
    <table>
        <tr>
            <th class="headerTableItem width15">
                Industry
            </th>
            <th class="headerTableItem width15">
                Competitor
            </th>
            <th class="headerTableItem width50">
                Description
            </th>
            <th class="headerTableItem width15">
                Owner
            </th>
            <th class="headerTableItem width15">
                Status
            </th>
            <th class="headerTableItem width15">
                <br />
            </th>
        </tr>
        <% foreach (var oCompetitor in CompetitorCollection)
           {%>
        <tr id="ComparinatorRowContent">
            <td>
                <%=oCompetitor.Industry.Name%>
            </td>
            <td>
                <%=oCompetitor.Name%>
            </td>
            <td>
                <span id="txt<%=oCompetitor.Id%>" onclick="DynamicText(this,'<%=oCompetitor.Id%>',0,'<%=oCompetitor.Description %>')" title="Click to reduce the text.">
                 <%=oCompetitor.Description%>
               </span>
            </td>
            <td>
                <%=oCompetitor.AssignedTo%>
            </td>
            <td>
                <%=oCompetitor.Status%>
            </td>
            <td>
                <input class="shortButton" type="button" value="Remove" onclick="RemoveCompetitor('<%=Url.Action("RemoveCompetitor", "Comparinator",new { CompetitorId=oCompetitor.Id}) %>','FormCompetitors');return false" />
                
            </td>
        </tr>
        <%} %>
    </table>
    Total Competitors :<%=ViewData["Counter"]%>
    <br />
    <center>
        <input class="shortButton" type="button" value="Compare" onclick="javascript: loadContent('<%= Url.Action("CompareCompetitors", "Comparinator") %>','FormResults');" />
    </center>
</div>
<%} %>
