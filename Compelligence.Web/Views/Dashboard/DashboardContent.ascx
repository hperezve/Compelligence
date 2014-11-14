<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<System.Web.UI.DataVisualization.Charting.Chart>>" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Views" %>
<%@ Import Namespace="Compelligence.BusinessLogic.Implementation" %>
<asp:Panel ID="DashboardContentDiv" runat="server">

    <script type="text/javascript">
        var showChart = function() {       
            var SelectedUser = '<%= ViewData["SelectedUser"] %>';
            browsePopup = window.open('<%= Url.Action("CreateFullChart", "Dashboard") %>' + '?SelectedUser=' + SelectedUser, "BrowsePopup", "width=700,height=500");
            if (window.focus) {
                browsePopup.focus();
            }

        };

        jQuery(window).bind('resize', function() {

            ProjectGrid = '#' + '<%= ViewData["Scope"]%>' + 'ProjectDashboardListTable';
            ApprovalGrid = '#' + '<%= ViewData["Scope"]%>' + 'ApprovalDashboardListTable';
            DiscussionGrid = '#' + '<%= ViewData["Scope"]%>' + 'DiscussionDashboardListTable';
            DealGrid = '#' + '<%= ViewData["Scope"]%>' + 'DealDashboardListTable';
            CalendarGrid = '#' + '<%= ViewData["Scope"]%>' + 'CalendarDashboardListTable';
            FeedbackGrid = '#' + '<%= ViewData["Scope"]%>' + 'FeedbackDashboardListTable';
            EventGrid = '#' + '<%= ViewData["Scope"]%>' + 'EventDashboardListTable';
            CommentsGrid = '#' + '<%= ViewData["Scope"]%>' + 'CommentsDashboardListTable';
            PlanGrid = '#' + '<%= ViewData["Scope"]%>' + 'PlanDashboardListTable';
            SurveyResponseGrid = '#' + '<%= ViewData["Scope"]%>' + 'SurveyResponseDashboardListTable';
            //WinLossAnalysisGrid = '#' + '<%= ViewData["Scope"]%>' + 'WinLossAnalysisDashboardListTable';

            var width = Math.round(($(window).width() * 0.9535) / 2) + '';
            $(ProjectGrid).jqGrid('setGridWidth', width);
            $(ApprovalGrid).jqGrid('setGridWidth', width);
            $(DiscussionGrid).jqGrid('setGridWidth', width);
            $(DealGrid).jqGrid('setGridWidth', width);
            $(CalendarGrid).jqGrid('setGridWidth', width);
            $(FeedbackGrid).jqGrid('setGridWidth', width);
            $(EventGrid).jqGrid('setGridWidth', width);
            $(CommentsGrid).jqGrid('setGridWidth', width);
            $(PlanGrid).jqGrid('setGridWidth', width);
            $(SurveyResponseGrid).jqGrid('setGridWidth', width);
            //$(WinLossAnalysisGrid).jqGrid('setGridWidth', width);

        }).trigger('resize');
        $(function() {
            updatePanels($("#DisplayPanel").val());
        });
    </script>
    <input type="hidden" id="DisplayPanel" value="<%= ViewData["DisplayPanel"]%>"/>
    <div id="contentBodyDashboard" style="height: 100%;min-height:200px">
        <div id="C1" class="dashboardBox">
            <%= Html.Hidden("SelectedUser")%>
            <div class="titleDashboardBox">
                My Projects</div>
            <%= Html.DataGrid("ProjectDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--my projects--%>
        <div id="C2" class="dashboardBox">
            <div class="titleDashboardBox">
                Pending Approvals</div>
            <%= Html.DataGrid("ApprovalDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--Aproval & Requests --%>
        <%var projectid = ViewData["ProjectId"]; %>
        <div id="C3" class="dashboardBox">
            <div class="titleDashboardBox">
                Discussion</div>
            <%= Html.DataGrid("DiscussionDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--deals(Small)--%>
        <div id="C4" class="dashboardBox">
            <div class="titleDashboardBox">
                My Deal Support</div>
            <%= Html.DataGrid("DealDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--My Deal Support(Small)--%>
        <div id="C5" class="dashboardBox">
            <div class="titleDashboardBox">
                Calendar</div>
            <%= Html.DataGrid("CalendarDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--Calendar--%>
        <div id="C6" class="dashboardBox">
            <div class="titleDashboardBox">
                Feedback</div>
            <%= Html.DataGrid("FeedbackDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--Events(Small)--%>
        <div id="C7" class="dashboardBox">
            <div class="titleDashboardBox">
                Events</div>
            <%= Html.DataGrid("EventDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--Chart--%>
        <%-- Enable when we define the graph better, we can make it option again
        <div id="Cnose" class="tblOne" style="width: 49%; _width: 586px; height: 223px; _height: 220px;
            float: left; padding: 5px;">
            <%if ((string)ViewData["ShowChart"] == "true")
              { %>
            <%foreach (var chart in (IList<string>)ViewData["charts"])
              { %>
            <%= chart %>
            <% } %>
            <% }
              else
              {%>
            <div style="font-size: 1.3em; margin: 60px 60px 60px 60px;">
                <a href="javascript:void(0)" onclick="showChart();">You have many projects and your
                    graphics for Rating and Utilization are large, you can see these graphics here.</a>
            </div>
            <% } %>
        </div>--%>
        <%--Comments--%>
        <div id="C8" class="dashboardBox">
            <div class="titleDashboardBox">
                Comments</div>
            <%= Html.DataGrid("CommentsDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--Plan(Small)--%>
        <div id="C9" class="dashboardBox">
            <div class="titleDashboardBox">
                My Tasks</div>
            <%= Html.DataGrid("PlanDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        <%--My Tasks--%>
        <div id="C10" class="dashboardBox">
            <div class="titleDashboardBox">
                Survey Responses</div>
            <%= Html.DataGrid("SurveyResponseDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>
        
<%--        <div class="dashboardBox">
        
            <div class="titleDashboardBox">
                Win/Loss Analysis</div>
            <%= Html.DataGrid("WinLossAnalysisDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>--%>
        <%--WinLoss Analysis--%>
        
       <%-- <div class="dashboardBoxDuplex">
            <div class="titleDashboardBox">
                Action History</div>
            <%= Html.DataGrid("ActionHistoryDashboard", new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>
        </div>--%>
       <div id="C11" class="tblOne" style="width: 48.3%; _width: 582px; height: 223px; _height: 220px;
            float: left; padding:5px 5px 5px 13px; overflow-y: auto;display:none;">
           
            <div class="titleDashboardBox" style=" margin-left:-5px; margin-bottom: 5px; ">
                Action History</div>
               
            <ul>
            
            <% string userId = (string)Session["UserId"];
               IList<ActionHistoryDashboardView> actionList = (IList<ActionHistoryDashboardView>)ViewData["ActionHistoryDas"]; %>
            <% if (actionList != null)
               {

                   foreach (ActionHistoryDashboardView actionAllView in actionList)
                   {
            %>
            <li><%= actionAllView.Description%> by <b><%= actionAllView.CreatedBy%></b> at <%= actionAllView.CreatedDate%>
            
                
                <% } %>
                <br/>
            </li>
            <% } %>
            
      <%--      <% } %>--%>
      
        </ul>
        
        
            <%--<%= Html.DataGrid("ActionHistoryDashboard",  new Dictionary<string, string>() { { "UserId", ViewData["SelectedUser"].ToString() } })%>  --%>
         
        
        </div>
        
       <%--Action History--%>
        
        
        
        
        
    </div>
</asp:Panel>
