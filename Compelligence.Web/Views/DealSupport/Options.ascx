<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

    <div class="rightTitle">
    <%--Deal Support Options  --%>
<asp:Literal ID="LtDealSuportOptions"  runat="server" Text="<%$ Resources:LabelResource, DealSupportDealSupportOptions %>" /></label>
    </div>
    <div class="rightBodies">
    <ul>
        <li class="lineList">
        <%= Html.ActionLink("Open Deal ", "Create", "DealSupport")%>
        </li>
        <li class="lineList">
        <%= Html.ActionLink("Current Deals", "Index", "DealSupport")%>
        </li>
        <%--<li class="lineList">
        <%= Html.ActionLink("Import from SalesForce", "GetOpportunities", "DealSupport")%>
        <br />
        <br />
        </li>--y%>
    </ul>
    </div>

<%--<li>
        <div class="rightTitle">Comparinator</div>
        
            <% Html.RenderPartial("ComparinatorContent"); %>
</li>  --%>     
