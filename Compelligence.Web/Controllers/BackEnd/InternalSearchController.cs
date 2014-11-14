using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Common.Search;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Common.Browse;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class InternalSearchController : GenericBackEndController
    {
        #region Public Properties

        public ISearchService SearchService { get; set; }

        public IBrowseService BrowseService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Search()
        {
            string searchQuery = Server.HtmlDecode(Request["SearchQuery"]);
            IList<SearchObject> searchObjects = SearchManager.GetInstance().GetSearchObjects();
            if (CurrentSecurityGroup.Equals("MANAGER"))
            {
                searchObjects = SearchManager.GetInstance().GetSearchObjectsByBrowseId("InternalSearchManager.xml");
            }
            else if (CurrentSecurityGroup.Equals("ANALYST"))
            {
                searchObjects = SearchManager.GetInstance().GetSearchObjectsByBrowseId("InternalSearchAnalyst.xml");
            }
            else if (CurrentSecurityGroup.Equals("PARTNER"))
            {
                searchObjects = SearchManager.GetInstance().GetSearchObjectsByBrowseId("InternalSearchPartner.xml");
            }
            ViewData["SearchQuery"] = searchQuery;
            ViewData["SearchObjects"] = searchObjects;
            ViewData["Group"] = CurrentSecurityGroup;
            return View("Results");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SearchData()
        {
            string browseId = Request["bid"];
            string searchId = Request["sid"];
            string searchQuery = Request["sq"];
            int page = Convert.ToInt32(Request["page"]);
            int limit = Convert.ToInt32(Request["rows"]);
            string sidx = Request["sidx"];
            string sord = Request["sord"];

            BrowseObject browseObject = (BrowseObject)BrowseManager.GetInstance().GetBrowseObject(browseId).Clone();
            var result = new JsonResult();
            int totalPages = 0;

            browseObject.WhereClause = ExpressionParser.GetExpression(Session, browseObject.WhereClause);

            browseObject.WhereClause = browseObject.WhereClause + " And " + SearchService.BuildWhereClause(browseObject, searchId, searchQuery);

            browseObject.OrderByClause = sidx + " " + sord;

            decimal records = BrowseService.CountByQueryFilter(browseObject);

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

            var rows = BrowseService.GetData(browseObject);

            result.Data = new { page = page, records = records, rows, total = totalPages };

            return result;
        }

        #endregion

    }
}
