using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class CompetitorSupplierController : BackEndAsyncFormController<CompetitorSupplier,CompetitorSupplierId>
    {
        #region Public Properties

        public ICompetitorSupplierService CompetitorSupplierService
        {
            get { return (ICompetitorSupplierService)_genericService; }
            set { _genericService = value; }
        }

        #endregion
        //
        // GET: /CompetitorSupplier/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        /// <summary>
        /// Make CompetitorSupplier after select on Popup
        /// </summary>
        public override ActionResult CreateDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string detailTypeParam = Request["DetailCreateType"];
            IList<CompetitorSupplier> newEntities = new List<CompetitorSupplier>();
            string detailFilter = Request["DetailFilter"];
            string[] parameters = detailFilter.Split('_');

            foreach (object id in ids)
            {
                if (!string.IsNullOrEmpty(id as string))
                {
                    if (decimal.Parse(parameters[2]) != decimal.Parse(id.ToString()))
                    {
                        CompetitorSupplierId competitorSupplierId = new CompetitorSupplierId(decimal.Parse(parameters[2]), decimal.Parse(id.ToString()));
                        CompetitorSupplier competitorSupplier = CompetitorSupplierService.GetById(competitorSupplierId);
                        if (competitorSupplier == null)
                        {
                            competitorSupplier = new CompetitorSupplier(competitorSupplierId);
                            competitorSupplier.HeaderType = Request["HeaderType"];
                            SetDefaultDataForSave(competitorSupplier);
                            SetDefaultDataForUpdate(competitorSupplier);
                            SetDetailFilterData(competitorSupplier);
                            newEntities.Add(competitorSupplier);
                        }
                    }
                }
            }
            CompetitorSupplierService.SaveCollection(newEntities);
            return null;
        }
        /// <summary>
        /// Delete CompetitorSupplier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ActionResult DeleteDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string detailFilter = Request["DetailFilter"];
            string[] parameters = detailFilter.Split('_');
            foreach (object identifier in ids)
            {
                CompetitorSupplierId competitorSupplierId = new CompetitorSupplierId(decimal.Parse(parameters[2]), decimal.Parse(identifier.ToString()));
                CompetitorSupplier competitorSupplier = CompetitorSupplierService.GetById(competitorSupplierId);

                if (competitorSupplier != null)
                {
                    competitorSupplier.HeaderType = StringUtility.CheckNull(Request["HeaderType"]);
                    SetDetailFilterData(competitorSupplier);
                    SetDefaultDataForUpdate(competitorSupplier);
                    CompetitorSupplierService.Delete(competitorSupplier);
                }
            }
            return null;
        }

        //#region Public Methods
        //[AcceptVerbs(HttpVerbs.Get)]
        //public ContentResult GetEntityName(decimal id)
        //{
        //    string result = string.Empty;
        //    CompetitorSupplier entity = CompetitorSupplierService.GetById(id);
        //    if (entity != null) result = entity.Name;
        //    return Content(result);
        //}
    }
}
