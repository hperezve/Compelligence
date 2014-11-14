using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Util;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class WinLossAnalysisController : BackEndAsyncFormController<WinLossAnalysis, decimal>
    {
         #region Validation Methods

        protected override bool ValidateFormData(WinLossAnalysis winLossAnalysis, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (Validator.IsBlankOrNull(winLossAnalysis.IndustryId))
            {
                ValidationDictionary.AddError("IndustryId", LabelResource.WinLossAnalysisIndustry);
            }

            if (Validator.IsBlankOrNull(winLossAnalysis.CompetitorId))
            {
                ValidationDictionary.AddError("CompetitorId", LabelResource.WinLossAnalysisCompetitor);
            }
            
           
            return ValidationDictionary.IsValid;
        }

        #endregion

        public IWinLossAnalysisService WinLossAnalysisService
        {
            get { return (IWinLossAnalysisService)_genericService; }
            set { _genericService = value; }
        }
        public IIndustryService IndustryService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IResourceService ResourceService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }

        protected override void SetFormData()
        {
            IList<Industry> IndustryCollection = IndustryService.GetAllSortByClientCompany("Name", CurrentCompany);
            IList<Competitor> CompetitorCollection = CompetitorService.GetAllSortByClientCompany("Name", CurrentCompany);
            
            IList<ResourceObject> TimePeriods = ResourceService.GetAll<WinLossAnalysisTimePeriod>();

            ViewData["IndustryList"] = new SelectList(IndustryCollection, "Id", "Name");
            ViewData["CompetitorList"] = new SelectList(CompetitorCollection, "Id", "Name");
            ViewData["TimePeriodList"] = new SelectList(TimePeriods, "Id", "Value");
        }

        public void ExecuteReader()
        {
            ClientCompany clientcompany = ClientCompanyService.GetById(CurrentCompany);
            WinLossAnalysisService.SaveWinLossAnalysisDetailFor(clientcompany);
        }
    
        public JsonResult Execute()
        {
            string competitor = (string)Request["competitor"];
            string startdate = (string)Request["startdate"];
            string enddate = (string)Request["enddate"];

            DateTime dtstartdate,dtenddate;
            DateTime.TryParseExact(startdate, "MM/dd/yyyy", null, DateTimeStyles.None, out dtstartdate);
            DateTime.TryParseExact(enddate, "MM/dd/yyyy", null, DateTimeStyles.None, out dtenddate);
            startdate = dtstartdate.ToString("yyyy-MM-dd");
            enddate = dtenddate.ToString("yyyy-MM-dd");
            ClientCompany clientcompany = ClientCompanyService.GetById(CurrentCompany);
            WinLossAnalysisDetail wad = WinLossAnalysisService.GetWinLossAnalysis(clientcompany.SalesForceUser, clientcompany.SalesForcePassword + clientcompany.SalesForceToken, competitor, startdate, enddate);
            var result = new JsonResult();
            if(wad!=null) //not logged to SFDC
              result.Data = new { percent = wad.WinLossPercent, amount = wad.WinLossTotal };
            else
                result.Data = new { percent = 0, amount = 0 };
            return result;
        }

        public JsonResult GetAllCompetitors()
        {
            IList<Competitor> competitors = CompetitorService.GetAllSortByClientCompany("Name", CurrentCompany);
            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitors, "Id", "Name");
        }
     }
}
