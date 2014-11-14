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

namespace Compelligence.Web.Controllers
{
    public class BudgetTypeController : BackEndAsyncFormController<BudgetType, decimal>
    {

        #region Public Properties

        public IBudgetTypeService BudgetTypeService
        {
            get { return (IBudgetTypeService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(BudgetType budgetType, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(budgetType.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.BudgetTypeNameRequiredError);
            }
            if (Validator.IsBlankOrNull(budgetType.UnitMeasure))
            {
                ValidationDictionary.AddError("UnitMeasure", LabelResource.BudgetTypeUnitMeasureRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            IList<ResourceObject> unitMeasureList = ResourceService.GetAll<BudgetTypeUnitMeasure>();
            ViewData["UnitMeasureList"] = new SelectList(unitMeasureList, "Id", "Value");
        }

        #endregion
    }
}
