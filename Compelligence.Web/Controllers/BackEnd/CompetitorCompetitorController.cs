using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Domain.Entity.Views;
using Compelligence.BusinessLogic.Implementation;

namespace Compelligence.Web.Controllers
{
    public class CompetitorCompetitorController : BackEndAsyncFormController<CompetitorCompetitor, CompetitorCompetitorId>
    {

        public ICompetitorCompetitorService CompetitorCompetitorService
        {
            get { return (ICompetitorCompetitorService)_genericService; }
            set { _genericService = value; }
        }
        
        //
        // GET: /CompetitorCompetitor/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public override ActionResult CreateDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string detailTypeParam = Request["DetailCreateType"];
            IList<CompetitorCompetitor> newEntities = new List<CompetitorCompetitor>();
            string detailFilter = Request["DetailFilter"];
            string[] parameters = detailFilter.Split('_');

            foreach (object id in ids)
            {
                if (!string.IsNullOrEmpty(id as string))
                {
                    if(decimal.Parse(parameters[2])!= decimal.Parse(id.ToString()))
                    {
                    CompetitorCompetitorId competitorCompetitorId = new CompetitorCompetitorId(decimal.Parse(parameters[2]), decimal.Parse(id.ToString()));
                    CompetitorCompetitor competitorCompetitor = CompetitorCompetitorService.GetById(competitorCompetitorId);
                    if (competitorCompetitor == null)
                    {
                        competitorCompetitor = new CompetitorCompetitor(competitorCompetitorId);
                        competitorCompetitor.CreatedBy = CurrentUser;
                        competitorCompetitor.CreatedDate = DateTime.Now;
                        competitorCompetitor.LastChangedBy = CurrentUser;
                        competitorCompetitor.LastChangedDate = DateTime.Now;
                        competitorCompetitor.ClientCompany = CurrentCompany;
                        competitorCompetitor.HeaderType = Request["HeaderType"];
                        SetDefaultDataForSave(competitorCompetitor);
                        SetDefaultDataForUpdate(competitorCompetitor);
                        SetDetailFilterData(competitorCompetitor);
                        newEntities.Add(competitorCompetitor);
                    }
                }
                }
            }
            CompetitorCompetitorService.SaveCollection(newEntities);
            return null;
        }
        /// <summary>
        /// Delete CompetitorCompetitor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ActionResult DeleteDetail() //(CompetitorCompetitorId id)
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string detailFilter = Request["DetailFilter"];
            string[] parameters = detailFilter.Split('_');
            foreach (object identifier in ids)
            {
                CompetitorCompetitorId competitorCompetitorId = new CompetitorCompetitorId(decimal.Parse(parameters[2]), decimal.Parse(identifier.ToString()));
                CompetitorCompetitor competitorCompetitor = CompetitorCompetitorService.GetById(competitorCompetitorId);

                if (competitorCompetitor != null)
                {
                    competitorCompetitor.HeaderType = StringUtility.CheckNull(Request["HeaderType"]);
                    SetDetailFilterData(competitorCompetitor);
                    SetDefaultDataForUpdate(competitorCompetitor);
                    CompetitorCompetitorService.Delete(competitorCompetitor);
                }
            }
            return null;
        }

        protected override void SetDetailFormData()
        {
            ViewData["HasRows"] = false;
            string headerType = Request["HeaderType"];
            if (!string.IsNullOrEmpty(headerType))
            {
                if (headerType.Equals(DomainObjectType.Competitor))
                {
                    string CompetitorId = GetDetailFilterValue("CompetitorCompetitor.CompetitorId");
                    if (!string.IsNullOrEmpty(CompetitorId))
                    {
                        decimal competitorId = decimal.Parse(CompetitorId);
                        IList<CompetitorCompetitorDetailView> competitorsByCompetitor = CompetitorCompetitorService.GetByCompetitorIdAndClientCompany(competitorId, CurrentCompany);
                        if (competitorsByCompetitor != null && competitorsByCompetitor.Count > 0)
                        {
                            ViewData["HasRows"] = true;
                        }
                    }
                }

            }
        }
    }
}
