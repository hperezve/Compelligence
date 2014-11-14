<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

        <div class="rightTitle">
        
            
                  <%--Event Options  --%>
                 <label for="LblEventOptions" width="100px">
                    <asp:Literal ID="LtEventOptions" runat="server" Text="<%$ Resources:LabelResource, EventsOptionsEventOptions %>" />
                 </label>
            </div>
       <div class="rightBodies">
        <ul>
            <li class="lineList">
                <%= Html.ActionLink("New event", "Create", "Events")%>
            </li>
            <li class="lineList">
                <%= Html.ActionLink("Upcoming Events", "Index", "Events")%>
            </li>
        </ul>
        </div>

<%--    <li>
        <div class="rightTitle">
            Comparinator</div>
        <% Html.RenderPartial("ComparinatorContent"); %>
    </li>--%>

