using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;
using Compelligence.Common.Browse;
using System.Reflection;
using Compelligence.BusinessLogic.Interface;
using System.Web.SessionState;
using Compelligence.Common.Utility.Parser;

using Compelligence.Common.Search;
using Compelligence.Common.Utility;

using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;

using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class BrowseController : Controller
    {

        private IBrowseService _browseService;

        public IBrowseService BrowseService
        {
            get { return _browseService; }
            set { _browseService = value; }
        }

        public IUserProfileCustomizationService UserProfileCustomizationService { get; set; }
        public IForumService ForumService { get; set; }
        private IForumResponseService _forumresponseservice;
        public IForumResponseService ForumResponseService
        {
            get { return _forumresponseservice; }
            set { _forumresponseservice = value; }
        }
       [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult saveConfig(string userId, string config, string value)
       {
           UserProfileCustomizationService.SaveConfig(userId, config, value);
           var data = new { name = "TestName"};
        
           return Json(data);
       }

       [AcceptVerbs(HttpVerbs.Get)]
       public ActionResult loadConfig()
       {
           string userId = Session["UserId"].ToString();
           IList<UserProfileCustomization> upc = UserProfileCustomizationService.GetByUserId(userId);
           var data = new ArrayList();
           foreach (UserProfileCustomization up in upc)
           { 
               var dato = new { config = up.Config , value = up.Value};
               data.Add(dato);
           }
           
           return Json(data);
       }

        public ActionResult GetBrowsePopup()
        {
            ViewData["BrowseName"] = Request["BrowseName"];

            return View("BrowsePopup");
        }

        public ActionResult GetContentBrowsePopup()
        {
            ViewData["BrowseName"] = Request["BrowseName"];

            return View("ContentBrowsePopup");
        }

        public ActionResult GetIndustryBrowsePopup()
        {
            ViewData["BrowseName"] = Request["BrowseName"];

            return View("IndustryBrowsePopup");
        }

        public ActionResult GetBrowsePopupCols()
        {
            ViewData["BrowseName"] = Request["BrowseName"];

            return View("BrowsePopupCols");
        }

        public ActionResult GetPositioningBrowsePopup()
        {
            ViewData["BrowseName"] = Request["BrowseName"];
            ViewData["EntityOption"] = Request["EntityOption"];
            ViewData["Scope"] = Request["Scope"];
            ViewData["BrowseId"] = Request["BrowseId"];
            ViewData["BrowseHierarchy"] = Request["BrowseHierarchy"];
            return View("PositioningBrowsePopup");
        }

        public ActionResult GetBrowseNewsLetterPopup()
        {
            ViewData["BrowseName"] = Request["BrowseName"];
            ViewData["Scope"] = Request["Scope"];
            ViewData["SectionEvent"] = "NewsLetterSectionEvent";
            ViewData["SectionNews"] = "NewsLetterSectionNews";
           

            return View("NewsLetterPopup");
        }

        //
        // GET: /Browse/BrowseName
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetData()
        {
            string browseId = Request["bid"];
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["rows"]);
            string sidx = Request["sidx"];
            string sord = Request["sord"];
            string defaultCriteria = Request["defaultCriteria"];
            string searchCriteria = Request["searchCriteria"];
            string filterCriteria = Request["filterCriteria"];
            string paramsCriteria = Request["prmcrt"];

            BrowseObject browseObject = (BrowseObject)BrowseManager.GetInstance().GetBrowseObject(browseId).Clone();
            var result = new JsonResult();
            int totalPages = 0;
            IDictionary<string, string> parameters = GetParametersValues(paramsCriteria);
            browseObject.WhereClause = ExpressionParser.GetExpression(GetParametersCriteria(paramsCriteria), browseObject.WhereClause);
            browseObject.WhereClause = ExpressionParser.GetExpression(Session, browseObject.WhereClause);
            browseObject.WhereClause = ExpressionParser.GetExpression(browseObject.WhereClause);

            GetFilterCriteria(browseObject, defaultCriteria);
            GetSearchCriteria(browseObject, searchCriteria);
            GetFilterCriteria(browseObject, filterCriteria);

            string[] orderObj = browseObject.OrderByClause.Split(' ');
            if (orderObj[0].Contains("NewsLetterSection"))
            {
                browseObject.OrderByClause = orderObj[0] + " " + sord;
            }
            else
            {               
                browseObject.OrderByClause = sidx + " " + sord;             
            }

            decimal records = BrowseService.CountByQueryFilter(browseObject, parameters);

            if (records > 0)
            {
                if (records <= limit)
                {
                    totalPages = 1;
                }
                else if (records > limit)
                {
                    totalPages = Convert.ToInt32(Math.Ceiling(records / limit));
                }
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            browseObject.Page = page;
            browseObject.PageSize = limit;

            var rows = BrowseService.GetData(browseObject, parameters);
            result.Data = new { page = page, records = records, rows, total = totalPages };
            return result;
        }

        private void GetSearchCriteria(BrowseObject browseObject, string searchCriteria)
        {
            if (!string.IsNullOrEmpty(searchCriteria))
            {
                IList<BrowseFilter> browseFilters = new List<BrowseFilter>();

                foreach (BrowseColumn browseColumn in browseObject.BrowseColumns)
                {
                    if (browseColumn.Filter)
                    {
                        browseFilters.Add(new BrowseFilter(browseColumn.Entity, browseColumn.Property, BrowseFilter.Operator.Cn, searchCriteria, BrowseOperator.Or));
                    }
                }

                browseObject.AddBrowseFilters(browseFilters);
            }
        }

        public IDictionary<string, string> GetParametersCriteria(string paramsCriteria)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(paramsCriteria))
            {
                string[] paramsCriteriaArray = paramsCriteria.Split(':');

                foreach (string paramCriteria in paramsCriteriaArray)
                {
                    string[] operators = paramCriteria.Split('_');

                    if ((operators.Length == 2) && (!string.IsNullOrEmpty(operators[1])))
                    {
                        parameters[operators[0]] = operators[1];
                    }
                }
            }

            return parameters;
        }

        public bool verifiedComments(decimal id, string type)
        {
            bool showComments = false;
            string ObjectType = string.Empty;
            if (type.Equals("EventComment"))
            {
                ObjectType = DomainObjectType.Event;
            }
            else
            {
                ObjectType = DomainObjectType.Deal;
            }
            Forum forum = ForumService.GetByEntityId(id, ObjectType, ForumType.Comment);
            IList<ForumResponse> forumresponses = null;
            forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);
            foreach (ForumResponse fr in forumresponses)
            {
                showComments = true;
            }            
            return showComments;
        }

        public void GetFilterCriteria(BrowseObject browseObject, string filterCriteria)
        {
            if (!string.IsNullOrEmpty(filterCriteria))
            {
                IList<BrowseFilter> browseFilters = new List<BrowseFilter>();
                string[] searchFilterArray = filterCriteria.Split(':');

                foreach (string searchFilter in searchFilterArray)
                {
                    string[] operators = searchFilter.Split('_');

                    if ((operators.Length == 3) && (!string.IsNullOrEmpty(operators[2])))
                    {
                        browseFilters.Add(new BrowseFilter(operators[0], (BrowseFilter.Operator)Enum.Parse(typeof(BrowseFilter.Operator), operators[1]), operators[2]));
                    }
                    else if ((operators.Length == 5) && (!string.IsNullOrEmpty(operators[2])) && (!string.IsNullOrEmpty(operators[4])))
                    {
                        browseFilters.Add(new BrowseFilter(operators[0], (BrowseFilter.Operator)Enum.Parse(typeof(BrowseFilter.Operator), operators[1]), operators[2] + "' AND '" + operators[4]));
                    }
                }

                browseObject.AddBrowseFilters(browseFilters);
            }
        }


        public object[] GetStaticData(string XmlSource, HttpSessionStateBase Session, int page)
        {
            //var page = Convert.ToInt32(Request["page"]);
            /*
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["rows"]);
            string sidx = Request["sidx"];
            string sord = Request["sord"];
            string defaultCriteria = Request["defaultCriteria"];
            string searchCriteria = Request["searchCriteria"];
            string filterCriteria = Request["filterCriteria"];
            

                */

            BrowseObject browseObject = (BrowseObject)BrowseManager.GetInstance().GetBrowseObject(XmlSource).Clone();

            browseObject.WhereClause = ExpressionParser.GetExpression(Session, browseObject.WhereClause);
            browseObject.WhereClause = ExpressionParser.GetExpression(browseObject.WhereClause);
            if (Session["Industry"] != null)
            {
                string idIndustry = (string)Session["Industry"];
                browseObject.WhereClause = browseObject.WhereClause + " AND EventAllView.IndustryId=" + idIndustry;
                Session["Industry"] = null;
            }

            if (Session["Competitor"] != null)
            {
                string idCompetitor = (string)Session["Competitor"];
                browseObject.WhereClause = browseObject.WhereClause + " AND EventAllView.CompetitorId=" + idCompetitor;
                Session["Competitor"] = null;
            }

            if (Session["ShowAll"] == null || Session["ShowAll"].Equals("no"))
            {
                browseObject.WhereClause = browseObject.WhereClause + " AND EventAllView.EndDate >= CONVERT(varchar, GETDATE(), 101) OR (EventAllView.StartDate >= CONVERT(varchar, GETDATE(), 101) AND EventAllView.EndDate IS NULL)";                
            }

            int totalPages = 0;
            int records = BrowseService.CountByQueryFilter(browseObject); ;
            int limit = 15;

            if (records > 0)
            {
                if (records <= limit)
                {
                    totalPages = 1;
                }
                else if (records > limit)
                {
                    totalPages = Convert.ToInt32(Math.Ceiling((decimal)records / limit));
                }
            }

            if (page > totalPages)
            {
                if (totalPages > 0)
                    page = totalPages;
                else
                    totalPages = 1;
            }


            browseObject.Page = page;
            browseObject.PageSize = limit;
            var rows = BrowseService.GetData(browseObject);
            var titles = (from column in browseObject.BrowseColumns select column.Label).ToList();
            int rowsbypage = rows.Length;

            object[] GridData = new object[] { rows, titles, rowsbypage, page, totalPages };
            return GridData;
        }


        public StaticGridData GetStaticGridData(string XmlSource, HttpSessionStateBase Session, int page, string groupby, string orderby, string sAsc, string sIndustry, string sCompetitor, string showAll, string dateFilter)
        {
            if ((sIndustry.Length > 0 || sCompetitor.Length > 0) && XmlSource.Equals("Events"))
            {
                XmlSource = "EventsForFilter";
            }
            if ((XmlSource.Equals("DealSupport") && groupby.Equals("CompetitorName")) || (XmlSource.Equals("DealSupport") && groupby.Equals("IndustryName")))
            {
                XmlSource = "DealSupportForFilter";
            }
            string requestDeal = (string)Session["ShowArchived"];

            if (XmlSource.Equals("DealSupport") && (requestDeal != null && requestDeal.Equals("yes")))
            {
                XmlSource = "DealSupportArchived";
            }

            StaticGridData Result = new StaticGridData();

            BrowseObject browseObject = (BrowseObject)BrowseManager.GetInstance().GetBrowseObject(XmlSource).Clone();

            if (XmlSource.Equals("EventsForFilter"))
            {
                browseObject.SelectClause = "Distinct EventsAllForFilterView.Id" + browseObject.SelectClause.Substring(25, browseObject.SelectClause.Length - 25);
            }
            browseObject.WhereClause = ExpressionParser.GetExpression(Session, browseObject.WhereClause);
            browseObject.WhereClause = ExpressionParser.GetExpression(browseObject.WhereClause);
            if ( sIndustry.Length> 0 )
            {
                //browseObject.WhereClause = browseObject.WhereClause + " AND EventsAllForFilterView.IndustryId=" + sIndustry;
                browseObject.WhereClause = browseObject.WhereClause + " AND (EventsAllForFilterView.IndustryId like '%" + sIndustry + "%' OR EventsAllForFilterView.IndustryLinage like '%" + sIndustry + "%')";
            }
            if (sCompetitor.Length> 0)
            {
                browseObject.WhereClause = browseObject.WhereClause + " AND EventsAllForFilterView.CompetitorId  like '%" + sCompetitor + "%'";
            }

            if (showAll.Equals("no") || showAll.Equals("") || showAll == null)
            {
                if (browseObject.BrowseId.IndexOf("Event") != -1)
                {
                if (sIndustry.Length > 0 || sCompetitor.Length > 0)
                    browseObject.WhereClause = browseObject.WhereClause + "  AND (EventsAllForFilterView.EndDate >= CONVERT(varchar, GETDATE(), 101) OR (EventsAllForFilterView.StartDate >= CONVERT(varchar, GETDATE(), 101) AND EventsAllForFilterView.EndDate IS NULL))";
                else
                    browseObject.WhereClause = browseObject.WhereClause + "  AND (EventAllView.EndDate >= CONVERT(varchar, GETDATE(), 101) OR (EventAllView.StartDate >= CONVERT(varchar, GETDATE(), 101) AND EventAllView.EndDate IS NULL))";
                    }
            }

            
            if (dateFilter != null && !dateFilter.Equals(""))
            {
                //DateTime tempDate = DateTime.Parse(dateFilter);
                //dateFilter = DateTimeUtility.ConvertToString(tempDate,"yyyy-MM-dd");
                //browseObject.WhereClause = browseObject.WhereClause + "  AND EventAllView.StartDate = '" + dateFilter + "'";
                //browseObject.WhereClause = browseObject.WhereClause + "  AND EventAllView.StartDate = CONVERT(varchar, '" + dateFilter + "', 101)";
                if (browseObject.BrowseId.IndexOf("Event") != -1)
                {
                    if (sIndustry.Length > 0 || sCompetitor.Length > 0)
                        browseObject.WhereClause = browseObject.WhereClause + "  AND EventsAllForFilterView.StartDate = '" + dateFilter + "'";
                    else
                        browseObject.WhereClause = browseObject.WhereClause + "  AND EventAllView.StartDate = '" + dateFilter + "'";
                }
            }

            //browseObject.WhereClause = browseObject.WhereClause + "  AND EventAllView.StartDate = '03/20/2010'";

            if (groupby.Length > 0 && orderby.Length > 0)
            {
                if ( groupby.Equals(orderby) )
                {
                    groupby = string.Empty;
                }

            }
            if (groupby.Length > 0)  //Have two nivels of order
                browseObject.OrderByClause = groupby + ", " +orderby + " "+ sAsc;
            else
                browseObject.OrderByClause = orderby + " " + sAsc;

            int totalPages = 0;
            int records = BrowseService.CountByQueryFilter(browseObject); ;
            int limit = 15;

            if (records > 0)
            {
                if (records <= limit)
                {
                    totalPages = 1;
                }
                else if (records > limit)
                {
                    totalPages = Convert.ToInt32(Math.Ceiling((decimal)records / limit));
                }
            }

            if (page > totalPages)
            {
                if (totalPages > 0)
                    page = totalPages;
                else
                    totalPages = 1;
            }


            browseObject.Page = page;
            browseObject.PageSize = limit;
            Result.rows = BrowseService.GetData(browseObject);
            Result.titles = (from column in browseObject.BrowseColumns select column.Label).ToList();
            Result.fields = (from column in browseObject.BrowseColumns select column.Property).ToList();
            Result.page = page;
            Result.rowsbypage = Result.rows.Length;
            Result.totalPages = totalPages;
            Result.totalRecords = records;
            Result.pageSize = limit;
            return Result;
        }

        public StaticGridData GetStaticGridDataNews(string XmlSource, HttpContextBase context, string cc, string type, int page, string groupby, string orderby, string sAsc, string browseDetailFilter)
        {
            StaticGridData Result = new StaticGridData();
            BrowseObject browseObject = (BrowseObject)BrowseManager.GetInstance().GetBrowseObject(XmlSource).Clone();
            if (type.Equals("Session"))
            {
                browseObject.WhereClause = ExpressionParser.GetExpression(context.Session, browseObject.WhereClause);
            }
            else if (type.Equals("Request"))
            {
                browseObject.WhereClause = browseObject.WhereClause.Replace("$S{ClientCompany}", cc);
            }
            browseObject.WhereClause = ExpressionParser.GetExpression(browseObject.WhereClause);
            if (!string.IsNullOrEmpty(browseDetailFilter))
            {
                if (!string.IsNullOrEmpty(browseObject.WhereClause))
                {
                    browseObject.WhereClause += " And ";
                }
                browseObject.WhereClause += " " +browseDetailFilter;
            }
            if (groupby.Length > 0 && orderby.Length > 0)
            {
                if (groupby.Equals(orderby))
                {
                    groupby = string.Empty;
                }

            }
            if (groupby.Length > 0)  //Have two nivels of order
                browseObject.OrderByClause = groupby + ", " + orderby + " " + sAsc;
            else
                browseObject.OrderByClause = orderby + " " + sAsc;

            int totalPages = 0;
            int records = BrowseService.CountByQueryFilter(browseObject); ;
            int limit = 15;

            if (records > 0)
            {
                if (records <= limit)
                {
                    totalPages = 1;
                }
                else if (records > limit)
                {
                    totalPages = Convert.ToInt32(Math.Ceiling((decimal)records / limit));
                }
            }

            if (page > totalPages)
            {
                if (totalPages > 0)
                    page = totalPages;
                else
                    totalPages = 1;
            }


            browseObject.Page = page;
            browseObject.PageSize = limit;
            Result.rows = BrowseService.GetData(browseObject);
            Result.titles = new List<String>();
            Result.fields = new List<String>();
            foreach (BrowseColumn browseColumn in browseObject.BrowseColumns)
            {
                if (!browseColumn.Hidden)
                {
                    Result.titles.Add(browseColumn.Label);
                    Result.fields.Add(browseColumn.Property);
                }
            }
            //Result.titles = (from column in browseObject.BrowseColumns select column.Label  ).ToList();
            //Result.fields = (from column in browseObject.BrowseColumns select column.Property).ToList();
            Result.page = page;
            Result.rowsbypage = Result.rows.Length;
            Result.totalPages = totalPages;
            Result.totalRecords = records;
            Result.pageSize = limit;
            return Result;
        }

        public IDictionary<string, string> GetParametersValues(string paramsCriteria)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(paramsCriteria))
            {
                string[] paramsCriteriaArray = paramsCriteria.Split(':');

                foreach (string paramCriteria in paramsCriteriaArray)
                {
                    string[] operators = paramCriteria.Split('_');

                    if ((operators.Length == 2) && (!string.IsNullOrEmpty(operators[1])))
                    {
                        parameters["WR_" + operators[0]] = operators[1];
                    }
                }
            }
            
            //Used for DashboardDiscussion and DashboardFeedback
            parameters.Add("WS_ClientCompany", Session["ClientCompany"].ToString());
            parameters.Add("WS_UserId", Session["UserId"].ToString());

            return parameters;
        }

    }
}

