using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Common.Dashboard;
using System.Web.UI.DataVisualization.Charting;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Helpers;
using Compelligence.Common.Browse;
using Compelligence.Domain.Entity;
using Compelligence.Common.Utility.Parser;
using Compelligence.Security.Managers;
using Compelligence.Domain.Entity.Resource;
using Resources;
using System.Collections;
using System.Data;
using Compelligence.BusinessLogic.Implementation;


namespace Compelligence.Web.Controllers
{
    public class DashboardController : GenericBackEndController
    {

        #region Public Properties

        public IForumResponseService ForumResponseService { get; set; }

        public IForumService ForumService { get; set; }

        public IDashboardService DashboardService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IProjectService ProjectService { get; set; }

        public IActionHistoryService ActionHistoryService { get; set; }

        public IResourceService ResourceService { get; set; }



        #endregion

        #region Action Methods

        public ActionResult Index()
        {
            string userId = (string)Session["UserId"];
            string companyId = (string)Session["ClientCompany"];
            ViewData["SelectedUser"] = userId;
            int numberPoints = 0;
            ViewData["ShowChart"] = "true";
            UserProfile user = UserProfileService.GetById(userId);
            IList<DashboardObject> dashboardObjects = DashboardManager.GetInstance().GetAll();
            IList<ResourceObject> dashboardList = ResourceService.GetDashboardBySortValue<DashboardPanels>();
            IList<string> charts = new List<string>();
            IDictionary<string, string> dashboardParameters = CreateDashboardParameters();
            ViewData["ActionHistoryDas"] = ActionHistoryService.GetByDasboard(CurrentCompany);
            ViewData["DashboardList"] = new SelectList(dashboardList, "Id", "Value");

            foreach (DashboardObject dashboardObject in dashboardObjects)
            {
                var chart = new Chart();

                dashboardObject.HqlClause = ExpressionParser.GetExpression(dashboardParameters, dashboardObject.HqlClause);

                dashboardObject.HqlClause = ExpressionParser.GetExpression(Session, dashboardObject.HqlClause);

                dashboardObject.SeriesName = ExpressionParser.GetExpression(dashboardParameters, dashboardObject.SeriesName);

                DashboardService.SetDashboardData(dashboardObject);

                chart = CreateChart(dashboardObject);
                if (numberPoints < chart.Series[0].Points.Count)
                {
                    numberPoints = chart.Series[0].Points.Count;
                }
                charts.Add(chart.RenderHtml());

            }
            IList<UserProfile> userList = UserProfileService.GetSubordinatesUsers(companyId);
            Project project = ProjectService.GetLastProjectByUser(userId);
            Forum ActiveForum = null;
            IList<ForumResponse> forumresponses = null; //ForumResponseService.GetByForumId(); 

            if (project != null)
            {
                ActiveForum = ForumService.GetByEntityId(project.Id, DomainObjectType.Project, ForumType.Discussion);
            }
            forumresponses = ActiveForum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(ActiveForum.Id, DomainObjectType.Project);
            ViewData["ReportTo"] = "false";
            IList<UserProfile> usersWithReport = UserProfileService.GetByReportToUser(userId, companyId);
            if (usersWithReport.Count > 0)
            {
                ViewData["ReportTo"] = "true";
                UserProfile userRoot = UserProfileService.GetById(userId);
                usersWithReport.Insert(0, userRoot);
                ViewData["AssignedToList"] = new SelectList(usersWithReport, "Id", "Name", userId);
            }
            if (numberPoints > 12)
            {
                ViewData["ShowChart"] = "false";
            }            
            
            ViewData["DisplayPanel"] = user.DashboardPanels;
            ViewData["Comments"] = forumresponses;
            ViewData["ForumId"] = ActiveForum == null ? -1 : ActiveForum.Id;
            ViewData["ProjectId"] = project;
            ViewData["ActiveForum"] = ActiveForum;
            ViewData["charts"] = charts;
            return View();

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DashboardPanels(string Id, string UserId)
        {
            UserProfile user = new UserProfile();
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = CurrentUser;
            }
            user = UserProfileService.GetById(UserId);
            user.DashboardPanels = Id;
            UserProfileService.Update(user);
            return Content(string.Empty);
        }

        
        public ActionResult DashboardContent()
        {

            string userId = Request["SelectedUser"];
            string companyId = (string)Session["ClientCompany"];
            IList<DashboardObject> dashboardObjects = DashboardManager.GetInstance().GetAll();
            IList<string> charts = new List<string>();
            IDictionary<string, string> dashboardParameters = CreateDashboardParameters();
            int numberPoints = 0;
            ViewData["ShowChart"] = "true";
            foreach (DashboardObject dashboardObject in dashboardObjects)
            {
                var chart = new Chart();
                dashboardObject.HqlClause = ExpressionParser.GetExpression(dashboardParameters, dashboardObject.HqlClause);
                dashboardObject.HqlClause = ExpressionParser.GetExpression(Session, dashboardObject.HqlClause);

                dashboardObject.SeriesName = ExpressionParser.GetExpression(dashboardParameters, dashboardObject.SeriesName);

                DashboardService.SetDashboardData(dashboardObject);

                chart = CreateChart(dashboardObject);
                if (numberPoints < chart.Series[0].Points.Count)
                {
                    numberPoints = chart.Series[0].Points.Count;
                }

                charts.Add(chart.RenderHtml());
            }
            IList<UserProfile> userList = UserProfileService.GetSubordinatesUsers(companyId);
            UserProfile user = UserProfileService.GetById(userId);
            Project project = ProjectService.GetLastProjectByUser(userId);
            Forum ActiveForum = null;
            IList<ForumResponse> forumresponses = null; //ForumResponseService.GetByForumId(); 

            if (project != null)
            {
                ActiveForum = ForumService.GetByEntityId(project.Id, DomainObjectType.Project, ForumType.Discussion);
            }
            forumresponses = ActiveForum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(ActiveForum.Id,DomainObjectType.Project);
            if (numberPoints > 12)
            {
                ViewData["ShowChart"] = "false";
            }
            ViewData["DisplayPanel"] = user.DashboardPanels;
            ViewData["Comments"] = forumresponses;
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["ForumId"] = ActiveForum == null ? -1 : ActiveForum.Id;
            ViewData["ProjectId"] = project;
            ViewData["ActiveForum"] = ActiveForum;
            ViewData["SelectedUser"] = Request["SelectedUser"];
            ViewData["charts"] = charts;

            /*Add History*/
            //ViewData["ActionHistory"] = ActionHistoryService.GetBySocialLog(CurrentCompany);
            ViewData["ActionHistoryDas"] = ActionHistoryService.GetByDasboard(CurrentCompany);
            return View();
        }

        public ActionResult CreateFullChart(string SelectedUser)
        {
            string userId = Request["SelectedUser"];
            string companyId = (string)Session["ClientCompany"];
            ViewData["SelectedUser"] = userId;
            IList<DashboardObject> dashboardObjects = DashboardManager.GetInstance().GetAll();
            IList<string> charts = new List<string>();
            IDictionary<string, string> dashboardParameters = CreateDashboardParameters();

            foreach (DashboardObject dashboardObject in dashboardObjects)
            {
                var chart = new Chart();
                dashboardObject.HqlClause = ExpressionParser.GetExpression(dashboardParameters, dashboardObject.HqlClause);
                dashboardObject.HqlClause = ExpressionParser.GetExpression(Session, dashboardObject.HqlClause);
                dashboardObject.SeriesName = ExpressionParser.GetExpression(dashboardParameters, dashboardObject.SeriesName);
                DashboardService.SetDashboardData(dashboardObject);
                chart.Height = dashboardObject.Height;
                chart.Width = 2 * dashboardObject.Width;
                chart.ImageType = ChartImageType.Png;
                chart.ImageStorageMode = ImageStorageMode.UseHttpHandler;
                chart.ImageLocation = Server.MapPath(".");

                var legend = new Legend("Legend of Graph");
                legend.Font = new System.Drawing.Font("Trebuchet MS", 7, System.Drawing.FontStyle.Bold);
                legend.Docking = Docking.Bottom;
                chart.Legends.Add(legend);

                String area = dashboardObject.Name;
                chart.Titles.Add(dashboardObject.Name);
                chart.ChartAreas.Add(area);
                chart.ChartAreas[area].AxisX.Title = dashboardObject.AxisXTitle;
                chart.ChartAreas[area].AxisY.Title = dashboardObject.AxisYTitle;
                var series = new Series(dashboardObject.SeriesName);
                if (dashboardObject.Type.Equals("Bar", StringComparison.OrdinalIgnoreCase))
                {
                    series.ChartType = SeriesChartType.Column;
                }
                if (dashboardObject.Type.Equals("Line", StringComparison.OrdinalIgnoreCase))
                {
                    series.ChartType = SeriesChartType.Line;
                }

                series.BorderWidth = 3;
                series.ShadowOffset = 2;


                int column = 0;
                IList<Point> points = dashboardObject.Points;
                int count = points.Count;

                for (int i = 0; i < count; i++)
                {
                    column = (int)points[i].Column;
                    string labelAxis = points[i].Status;
                    if (labelAxis.Length > 15)
                    { labelAxis = labelAxis.Substring(0, 15) + ".."; }
                    series.Points.AddXY(labelAxis, column);
                }

                series.IsVisibleInLegend = true;
                chart.Series.Add(series);
                chart.ChartAreas[area].AxisX.Interval = 1;
                charts.Add(chart.RenderHtml());
            }
            ViewData["charts"] = charts;
            return View("DashboardPopup");
        }

        #endregion

        #region Private Methods

        private Chart CreateChart(DashboardObject dashboardObject)
        {
            Chart chart = new Chart();

            chart.Height = dashboardObject.Height;
            chart.Width = dashboardObject.Width;
            chart.ImageType = ChartImageType.Png;
            chart.ImageStorageMode = ImageStorageMode.UseHttpHandler;
            chart.ImageLocation = Server.MapPath(".");

            var legend = new Legend("Legend of Graph");
            legend.IsTextAutoFit = true;
            legend.Font = new System.Drawing.Font("Trebuchet MS", 7, System.Drawing.FontStyle.Bold);
            legend.Docking = Docking.Bottom;
            chart.Legends.Add(legend);

            String area = dashboardObject.Name;
            chart.Titles.Add(dashboardObject.Name);
            chart.ChartAreas.Add(area);
            chart.ChartAreas[area].AxisX.Title = dashboardObject.AxisXTitle;
            chart.ChartAreas[area].AxisY.Title = dashboardObject.AxisYTitle;
            var series = new Series(dashboardObject.SeriesName);
            if (dashboardObject.Type.Equals("Bar", StringComparison.OrdinalIgnoreCase))
            {
                series.ChartType = SeriesChartType.Column;
            }
            if (dashboardObject.Type.Equals("Line", StringComparison.OrdinalIgnoreCase))
            {
                series.ChartType = SeriesChartType.Line;
            }

            series.BorderWidth = 3;
            series.ShadowOffset = 2;

            int column = 0;
            IList<Point> points = dashboardObject.Points;
            int count = points.Count;

            for (int i = 0; i < count; i++)
            {
                column = (int)points[i].Column;
                series.Points.AddXY(i + 1, column);
                string labelAxis = points[i].Status;
                if (labelAxis.Length > 15)
                { labelAxis = labelAxis.Substring(0, 15) + ".."; }
                series.Points[i].AxisLabel = labelAxis;
            }
            chart.Series.Add(series);
            chart.ChartAreas[area].AxisX.Interval = 1;
            return chart;
        }

        private IDictionary<string, string> CreateDashboardParameters()
        {
            string userId = Request["SelectedUser"];

            if (string.IsNullOrEmpty(userId))
            {
                userId = (string)Session["UserId"];
            }

            UserProfile userProfile = UserManager.GetInstance().GetUserProfile(userId);

            IDictionary<string, string> dashboardParameters = new Dictionary<string, string>();

            dashboardParameters.Add("UserId", userProfile.Id);
            dashboardParameters.Add("UserName", userProfile.Name);



            return dashboardParameters;
        }

        #endregion
    }
}
