using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Validation;

namespace Compelligence.Web.Controllers
{
    public class ImplicationController : BackEndAsyncFormController<Implication, decimal>
    {

        #region Public Properties

        public IImplicationService ImplicationService
        {
            get { return (IImplicationService)_genericService; }
            set { _genericService = value; }
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Implication implication, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(implication.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ImplicationNameRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion
    }
}
