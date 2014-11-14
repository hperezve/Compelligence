using System;
using System.Collections.Generic;
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
namespace Compelligence.Web.Controllers
{
    public class IndustryCriteriaController : BackEndAsyncFormController<IndustryCriteria, IndustryCriteriaId>
    {

        #region Public Properties

        public IIndustryCriteriaService IndustryCriteriaService
        {
            get { return (IIndustryCriteriaService)_genericService; }
            set { _genericService = value; }
        }

        public ICriteriaService CriteriaService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateIndustryCriteria(IndustryCriteria industryCriteria, FormCollection collection)
        {
            try
            {
                OperationStatus operationStatus = OperationStatus.Initiated;

                SetDetailFilterData(industryCriteria);

                if (ValidateFormData(industryCriteria, collection))
                {
                    GetFormData(industryCriteria, collection);

                    SetDefaultDataForSave(industryCriteria);

                    GenericService.Save(industryCriteria);

                    operationStatus = OperationStatus.Successful;

                    return RedirectToAction("EditIndustryCriteria", new { industryId = industryCriteria.IndustryId, criteriaId = industryCriteria.CriteriaId, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
                }

                SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

                SetFormData();

                return View("Edit", industryCriteria);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditIndustryCriteria(string id, string operationStatus)
        {
            IndustryCriteriaId industryCriteriaId;

            if (!string.IsNullOrEmpty(id))
            {
                string[] compId = id.Split('_');

                industryCriteriaId = new IndustryCriteriaId(Convert.ToDecimal(compId[0]), Convert.ToDecimal(compId[1]));
            }
            else
            {
                decimal industryId = Convert.ToDecimal(Request["industryId"]);
                decimal criteriaId = Convert.ToDecimal(Request["criteriaId"]);

                industryCriteriaId = new IndustryCriteriaId(industryId, criteriaId);
            }

            IndustryCriteria industryCriteria = IndustryCriteriaService.GetById(industryCriteriaId);

            OperationStatus operationStatusParam = OperationStatus.Initiated;

            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();

            SetEntityDataToForm(industryCriteria);

            return View("Edit", industryCriteria);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditIndustryCriteria(IndustryCriteria formIndustryCriteria, FormCollection collection)
        {
            try
            {
                IndustryCriteria industryCriteriaResult = formIndustryCriteria;

                OperationStatus operationStatus = OperationStatus.Initiated;

                if (ValidateEditFormData(formIndustryCriteria, collection))
                {
                    IndustryCriteria industryCriteria = IndustryCriteriaService.GetById(
                    new IndustryCriteriaId(DecimalUtility.CheckNull(formIndustryCriteria.IndustryId),
                        DecimalUtility.CheckNull(formIndustryCriteria.CriteriaIdOld)));

                    // If selected "CriteriaId" value is diferent that "CriteriaIdOld", so use "SaveExisting" method from Service
                    if (DecimalUtility.CheckNull(formIndustryCriteria.CriteriaId) != DecimalUtility.CheckNull(formIndustryCriteria.CriteriaIdOld))
                    {
                        GetFormData(formIndustryCriteria, collection);

                        SetDefaultDataForSave(formIndustryCriteria);

                        IndustryCriteriaService.SaveExisting(formIndustryCriteria, industryCriteria);

                        industryCriteriaResult = formIndustryCriteria;
                    }
                    else
                    {
                        SetFormDataToEntity(industryCriteria, collection);

                        GetFormData(industryCriteria, collection);

                        SetDefaultDataForUpdate(industryCriteria);

                        IndustryCriteriaService.Update(industryCriteria);

                        industryCriteriaResult = industryCriteria;
                    }

                    operationStatus = OperationStatus.Successful;
                }

                SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

                SetFormData();

                return View("Edit", industryCriteriaResult);
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteIndustryCriteria(string id)
        {
            try
            {
                string[] idComponents = id.Split('_');
                string userId = (string)Session["UserId"];

                IndustryCriteria industryCriteria = IndustryCriteriaService.GetById(
                    new IndustryCriteriaId(Convert.ToDecimal(idComponents[0]),
                        Convert.ToDecimal(idComponents[1])));

                SetDefaultDataFromRequest(industryCriteria);

                IndustryCriteriaService.Delete(industryCriteria, userId);

                return null;
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(IndustryCriteria industryCriteria, FormCollection formCollection)
        {
            ValidationDictionary.RemoveError("Id");

            if (Validator.IsBlankOrNull(industryCriteria.CriteriaId))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.IndustryCriteriaCriteriaIdRequiredError);
            }
            else if (!IndustryCriteriaService.IsValidIndustryCriteria(DecimalUtility.CheckNull(industryCriteria.IndustryId),
                DecimalUtility.CheckNull(industryCriteria.CriteriaId)))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.IndustryCriteriaCriteriaIdExistsError);
            }

            if (Validator.IsBlankOrNull(industryCriteria.Value))
            {
                ValidationDictionary.AddError("Value", LabelResource.IndustryCriteriaValueRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(IndustryCriteria industryCriteria, FormCollection formCollection)
        {
            ValidationDictionary.RemoveError("Id");

            if (Validator.IsBlankOrNull(industryCriteria.CriteriaId))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.IndustryCriteriaCriteriaIdRequiredError);
            }
            else if ((DecimalUtility.CheckNull(industryCriteria.CriteriaId) != DecimalUtility.CheckNull(industryCriteria.CriteriaIdOld))
                && (!IndustryCriteriaService.IsValidIndustryCriteria(DecimalUtility.CheckNull(industryCriteria.IndustryId), DecimalUtility.CheckNull(industryCriteria.CriteriaId))))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.IndustryCriteriaCriteriaIdExistsError);
            }

            if (Validator.IsBlankOrNull(industryCriteria.Value))
            {
                ValidationDictionary.AddError("Value", LabelResource.IndustryCriteriaValueRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<Criteria> criteriaList = CriteriaService.GetAllActiveByClientCompany(clientCompany);

            ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Description");
        }

        protected override void SetEntityDataToForm(IndustryCriteria industryCriteria)
        {
            ViewData["CriteriaIdOld"] = industryCriteria.CriteriaId;
        }

        #endregion

    }
}
