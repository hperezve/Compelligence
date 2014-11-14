using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.Util.Validation;

namespace Compelligence.Web.Controllers
{
    public class CriteriaSetController : BackEndAsyncFormController<CriteriaSet, decimal>
    {

        #region Public Properties

        public ICriteriaSetService CriteriaSetService
        {
            get { return (ICriteriaSetService)_genericService; }
            set { _genericService = value; }
        }

        public ICriteriaGroupService CriteriaGroupService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(CriteriaSet criteriaSet, FormCollection formCollection)
        {

            if (Validator.IsBlankOrNull(criteriaSet.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.CriteriaSetNameRequiredError);
            }

            if (Validator.IsBlankOrNull(criteriaSet.CriteriaGroupId))
            {
                ValidationDictionary.AddError("CriteriaGroupId", LabelResource.CriteriaSetCriteriaGroupIdRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];

            IList<CriteriaGroup> criteriaGroupList = CriteriaGroupService.GetAllActiveByClientCompany(clientCompany);

            ViewData["CriteriaGroupIdList"] = new SelectList(criteriaGroupList, "Id", "Name");
        }

        #endregion

    }
}
