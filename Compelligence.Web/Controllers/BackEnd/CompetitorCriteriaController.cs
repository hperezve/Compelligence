using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Type;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Web.Models.Util;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class CompetitorCriteriaController : BackEndAsyncFormController<CompetitorCriteria, CompetitorCriteriaId>
    {

        #region Public Properties

        public ICompetitorCriteriaService CompetitorCriteriaService
        {
            get { return (ICompetitorCriteriaService)_genericService; }
            set { _genericService = value; }
        }

        public IIndustryService IndustryService { get; set; }

        public IIndustryCriteriasService IndustryCriteriasService { get; set; }

        public ICriteriaService CriteriaService { get; set; }

        public ICriteriaSetService CriteriaSetService { get; set; }

        public ICriteriaGroupService CriteriaGroupService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateCompetitorCriteria(CompetitorCriteria competitorCriteria, FormCollection collection)
        {
            try
            {
                OperationStatus operationStatus = OperationStatus.Initiated;

                SetDetailFilterData(competitorCriteria);

                if (ValidateFormData(competitorCriteria, collection))
                {
                    GetFormData(competitorCriteria, collection);

                    SetDefaultDataForSave(competitorCriteria);

                    GenericService.Save(competitorCriteria);

                    operationStatus = OperationStatus.Successful;

                    return RedirectToAction("EditCompetitorCriteria", new { competitorId = competitorCriteria.CompetitorId, criteriaId = competitorCriteria.CriteriaId, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
                }

                SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

                SetFormData();

                SetFormEntityDataToForm(competitorCriteria);

                SetUserSecurityAccess();

                return View("Edit", competitorCriteria);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditCompetitorCriteria(string id, string operationStatus)
        {
            CompetitorCriteriaId competitorCriteriaId;
            string sIndustryId = GetDetailFilterValue("CompetitorCriteria.IndustryId");
            if (!string.IsNullOrEmpty(id))
            {
                string[] compId = id.Split('_');

                competitorCriteriaId = new CompetitorCriteriaId(Convert.ToDecimal(compId[0]), Convert.ToDecimal(compId[1]), decimal.Parse(sIndustryId));
            }
            else
            {
                decimal competitorId = Convert.ToDecimal(Request["competitorId"]);
                decimal criteriaId = Convert.ToDecimal(Request["criteriaId"]);

                competitorCriteriaId = new CompetitorCriteriaId(competitorId, criteriaId, decimal.Parse(sIndustryId));
            }

            CompetitorCriteria competitorCriteria = CompetitorCriteriaService.GetById(competitorCriteriaId);

            OperationStatus operationStatusParam = OperationStatus.Initiated;

            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();

            SetEntityDataToForm(competitorCriteria);

            SetUserSecurityAccess(competitorCriteria);

            SetEntityLocking(competitorCriteria);

            return View("Edit", competitorCriteria);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCompetitorCriteria(CompetitorCriteria formCompetitorCriteria, FormCollection collection)
        {
            string sIndustryId = GetDetailFilterValue("CompetitorCriteria.IndustryId");
            try
            {
                CompetitorCriteria competitorCriteriaResult = formCompetitorCriteria;
                CompetitorCriteriaId newId = new CompetitorCriteriaId((decimal)competitorCriteriaResult.CompetitorId, (decimal)competitorCriteriaResult.CriteriaId, decimal.Parse(sIndustryId));
                competitorCriteriaResult.Id = newId;
                OperationStatus operationStatus = OperationStatus.Initiated;

                CompetitorCriteriaId id = new CompetitorCriteriaId(DecimalUtility.CheckNull(formCompetitorCriteria.CompetitorId),
                    DecimalUtility.CheckNull(formCompetitorCriteria.CriteriaIdOld), decimal.Parse(sIndustryId));
                CompetitorCriteria competitorCriteria = CompetitorCriteriaService.GetById(id);

                if (ValidateEditFormData(formCompetitorCriteria, collection))
                {

                    // If selected "CriteriaId" value is diferent that "CriteriaIdOld", so use "SaveExisting" method from Service
                    if (DecimalUtility.CheckNull(formCompetitorCriteria.CriteriaId) != DecimalUtility.CheckNull(formCompetitorCriteria.CriteriaIdOld))
                    {
                        GetFormData(formCompetitorCriteria, collection);

                        SetDefaultDataForSave(formCompetitorCriteria);

                        CompetitorCriteriaService.SaveExisting(formCompetitorCriteria, competitorCriteria);

                        competitorCriteriaResult = formCompetitorCriteria;
                    }
                    else
                    {
                        SetFormDataToEntity(competitorCriteria, collection);

                        GetFormData(competitorCriteria, collection);

                        SetDefaultDataForUpdate(competitorCriteria);

                        CompetitorCriteriaService.Update(competitorCriteria);

                        competitorCriteriaResult = competitorCriteria;
                    }

                    operationStatus = OperationStatus.Successful;
                }

                SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

                SetFormData();

                SetFormEntityDataToForm(competitorCriteriaResult);

                SetUserSecurityAccess(competitorCriteriaResult);

                SetEntityLocking(competitorCriteriaResult);

                return View("Edit", competitorCriteriaResult);
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCompetitorCriteria(string id)
        {
            string sIndustryId = GetDetailFilterValue("CompetitorCriteria.IndustryId");
            try
            {
                string[] idComponents = id.Split('_');
                string userId = (string)Session["UserId"];

                CompetitorCriteria competitorCriteria = CompetitorCriteriaService.GetById(
                    new CompetitorCriteriaId(Convert.ToDecimal(idComponents[0]),
                        Convert.ToDecimal(idComponents[1]), decimal.Parse(sIndustryId)));

                SetDefaultDataFromRequest(competitorCriteria);

                CompetitorCriteriaService.Delete(competitorCriteria, userId);

                return null;
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriaSetsByCriteriaGroup(decimal id)
        {
            IList<CriteriaSet> criteriaSetList = CriteriaSetService.GetByCriteriaGroupId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<CriteriaSet>(criteriaSetList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriasByCriteriaSet(decimal id)
        {
            //decimal industryId = Convert.ToDecimal(StringUtility.CheckNull(Request["IndustryId"]));
            ////IList<Criteria> criteriaList = CriteriaService.GetByCriteriaSetAndIndustry(id, Convert.ToDecimal(industryId));
            //IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(industryId, null, null);

            IList<Criteria> criteriaList = CriteriaService.GetByCriteriaSetId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Criteria>(criteriaList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriasByGroupSet(string IndustryId, string CriteriaGroupId, string CriteriaSetId)
        {

            IList<Criteria> criteriaList = null;
            if (!string.IsNullOrEmpty(CriteriaGroupId) && !string.IsNullOrEmpty(CriteriaSetId))
                criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(IndustryId), decimal.Parse(CriteriaGroupId), decimal.Parse(CriteriaSetId));
            else
                if (!string.IsNullOrEmpty(CriteriaGroupId) && string.IsNullOrEmpty(CriteriaSetId))
                    criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(IndustryId), decimal.Parse(CriteriaGroupId), null);
                else
                    criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(IndustryId), null, null);

            return ControllerUtility.GetSelectOptionsFromGenericList<Criteria>(criteriaList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetIndustryStandard(decimal id)
        {
            Criteria criteria = CriteriaService.GetById(id);

            //return new ContentResult { Content = criteria.IndustryStandard, ContentType = "application/json" };
            return Content(criteria.IndustryStandard);
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(CompetitorCriteria competitorCriteria, FormCollection formCollection)
        {
            ValidationDictionary.RemoveError("Id");

            //if (Validator.IsBlankOrNull(competitorCriteria.CriteriaGroupId))
            //{
            //    ValidationDictionary.AddError("CriteriaGroupId", LabelResource.CompetitorCriteriaCriteriaGroupIdRequiredError);
            //}

            //if (Validator.IsBlankOrNull(competitorCriteria.CriteriaSetId))
            //{
            //    ValidationDictionary.AddError("CriteriaSetId", LabelResource.CompetitorCriteriaCriteriaSetIdRequiredError);
            //}

            if (Validator.IsBlankOrNull(competitorCriteria.CriteriaId))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.CompetitorCriteriaCriteriaIdRequiredError);
            }
            else if (!CompetitorCriteriaService.IsValidCompetitorCriteria(DecimalUtility.CheckNull(competitorCriteria.CompetitorId),
                DecimalUtility.CheckNull(competitorCriteria.CriteriaId), DecimalUtility.CheckNull(competitorCriteria.IndustryId)))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.CompetitorCriteriaCriteriaIdExistsError);
            }

            if (Validator.IsBlankOrNull(competitorCriteria.Value))
            {
                ValidationDictionary.AddError("Value", LabelResource.CompetitorCriteriaValueRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(CompetitorCriteria competitorCriteria, FormCollection formCollection)
        {
            ValidationDictionary.RemoveError("Id");

            //if (Validator.IsBlankOrNull(competitorCriteria.CriteriaGroupId))
            //{
            //    ValidationDictionary.AddError("CriteriaGroupId", LabelResource.CompetitorCriteriaCriteriaGroupIdRequiredError);
            //}

            //if (Validator.IsBlankOrNull(competitorCriteria.CriteriaSetId))
            //{
            //    ValidationDictionary.AddError("CriteriaSetId", LabelResource.CompetitorCriteriaCriteriaSetIdRequiredError);
            //}

            if (Validator.IsBlankOrNull(competitorCriteria.CriteriaId))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.CompetitorCriteriaCriteriaIdRequiredError);
            }
            else if ((DecimalUtility.CheckNull(competitorCriteria.CriteriaId) != DecimalUtility.CheckNull(competitorCriteria.CriteriaIdOld))
                && (!CompetitorCriteriaService.IsValidCompetitorCriteria(DecimalUtility.CheckNull(competitorCriteria.CompetitorId), DecimalUtility.CheckNull(competitorCriteria.CriteriaId), DecimalUtility.CheckNull(competitorCriteria.IndustryId))))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.CompetitorCriteriaCriteriaIdExistsError);
            }

            if (Validator.IsBlankOrNull(competitorCriteria.Value))
            {
                ValidationDictionary.AddError("Value", LabelResource.CompetitorCriteriaValueRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetDetailFormData()
        {
            string industryIdValue = GetDetailFilterValue("CompetitorCriteria.IndustryId");
           
            Industry industry = null;

            if (!string.IsNullOrEmpty(industryIdValue))
            {
                industry = IndustryService.GetById(Convert.ToDecimal(industryIdValue));
            }

            ViewData["CompetitorCriteriaIndustry"] = industry;
        }

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string industryIdValue = GetDetailFilterValue("CompetitorCriteria.IndustryId");
            IList<CriteriaGroup> criteriaGroupList = new List<CriteriaGroup>();

            if (!string.IsNullOrEmpty(industryIdValue))
            {
                criteriaGroupList = CriteriaGroupService.GetByIndustryId(Convert.ToDecimal(industryIdValue));
            }

            ViewData["CriteriaGroupList"] = new SelectList(criteriaGroupList, "Id", "Name");
            ViewData["IndustryId"] = industryIdValue;

            IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(industryIdValue), null, null);
            ViewData["Path"] = obtainPath();

            ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(CompetitorCriteria competitorCriteria)
        {
            ViewData["CriteriaIdOld"] = competitorCriteria.CriteriaId;
            string industryIdValue = GetDetailFilterValue("CompetitorCriteria.IndustryId");
            competitorCriteria.IndustryId = Convert.ToDecimal(industryIdValue);
            competitorCriteria.CriteriaId = competitorCriteria.Id.CriteriaId;
            competitorCriteria.CompetitorId = competitorCriteria.Id.CompetitorId;
            competitorCriteria.CriteriaIdOld = competitorCriteria.Id.CriteriaId;

            if (competitorCriteria.CriteriaId != null)
            {
                Criteria criteria = CriteriaService.GetById((decimal)competitorCriteria.CriteriaId);
                ViewData["ValStandard"] = criteria.IndustryStandard;
            }

            ViewData["Path"] = obtainPath();
            SetCascadingData(competitorCriteria);
        }

        protected override void SetFormEntityDataToForm(CompetitorCriteria competitorCriteria)
        {
            SetCascadingData(competitorCriteria);
            if (competitorCriteria.Id != null)
            {
                competitorCriteria.CriteriaIdOld = competitorCriteria.Id.CriteriaId;
                competitorCriteria.CompetitorId = competitorCriteria.Id.CompetitorId;
                ModelState.SetModelValue("CriteriaIdOld", new ValueProviderResult(competitorCriteria.CriteriaIdOld, competitorCriteria.CriteriaIdOld.ToString(), CultureInfo.InvariantCulture));
                ModelState.SetModelValue("CompetitorId", new ValueProviderResult(competitorCriteria.CompetitorId, competitorCriteria.CompetitorId.ToString(), CultureInfo.InvariantCulture));
            }
            
        }

        #endregion

        #region Private Methods

        //private void SetCascadingData(CompetitorCriteria competitorCriteria)
        //{
        //    string industryIdValue = GetDetailFilterValue("CompetitorCriteria.IndustryId");

        //    IndustryCriteriasId id = new IndustryCriteriasId(Convert.ToDecimal(industryIdValue),(decimal)competitorCriteria.CriteriaId);
        //    IndustryCriterias indCriterias = IndustryCriteriasService.GetById(id);
        //    if (indCriterias != null) {
        //        if (!DecimalUtility.IsBlankOrNull(indCriterias.CriteriaGroupId))
        //        competitorCriteria.CriteriaGroupId = indCriterias.CriteriaGroupId;
        //        if (!DecimalUtility.IsBlankOrNull(indCriterias.CriteriaSetId))
        //        competitorCriteria.CriteriaSetId = indCriterias.CriteriaSetId;
        //        competitorCriteria.CriteriaId = indCriterias.Id.CriteriaId;
        //    }

        //    if (!DecimalUtility.IsBlankOrNull(competitorCriteria.CriteriaGroupId))
        //    {
        //        IList<CriteriaSet> criteriaSetList = CriteriaSetService.GetByCriteriaGroupAndIndustry(competitorCriteria.CriteriaGroupId.Value, Convert.ToDecimal(industryIdValue));

        //        ViewData["CriteriaSetList"] = new SelectList(criteriaSetList, "Id", "Name");


        //    }

        //    //if (!DecimalUtility.IsBlankOrNull(competitorCriteria.CriteriaSetId))
        //    //{
        //        //IList<Criteria> criteriaList = CriteriaService.GetByCriteriaSetAndIndustry((decimal)competitorCriteria.CriteriaSetId, Convert.ToDecimal(industryIdValue));
        //        IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(industryIdValue), null, null);
        //       // IList<Criteria> criteriaList = CriteriaService.GetByCriteriaSetId(competitorCriteria.CriteriaSetId.Value);

        //        ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
        //    //}
        //}

        private void SetCascadingData(CompetitorCriteria competitorCriteria)
        {
            if (!DecimalUtility.IsBlankOrNull(competitorCriteria.CriteriaGroupId))
            {
                IList<CriteriaSet> criteriaSetList = CriteriaSetService.GetByCriteriaGroupId(competitorCriteria.CriteriaGroupId.Value);

                ViewData["CriteriaSetList"] = new SelectList(criteriaSetList, "Id", "Name");

                if (!DecimalUtility.IsBlankOrNull(competitorCriteria.CriteriaSetId))
                {
                    IList<Criteria> criteriaList = CriteriaService.GetByCriteriaSetId(competitorCriteria.CriteriaSetId.Value);
                    ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
                }
                else //Have Group,but not Set
                {
                    IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet((decimal)competitorCriteria.IndustryId, (decimal)competitorCriteria.CriteriaGroupId, null);
                    ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
                }
            }
            else //Not have Group
            {
                IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet((decimal)competitorCriteria.IndustryId, null, null);
                ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
            }
        }
        #endregion

        #region Public Methods

        public static string obtainPath()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            string path = "";
            bool start = false;
            int initPos = 0;
            int endPos = 0;
            int begin = 7;
            if (currentUrl.IndexOf("https://") != -1) { begin++; }
            for (int i = begin; i < currentUrl.Length; i++)
            {
                string actualChar = currentUrl[i].ToString();

                if (start == false && actualChar.Equals("/"))
                {
                    start = true;
                    initPos = i;
                    break;
                }

            }
            endPos = currentUrl.IndexOf("/CompetitorCriteria.aspx");
            path = currentUrl.Substring(initPos, endPos - initPos);
            return path;
        }

        #endregion
    }
}
