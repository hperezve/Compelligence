using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Util.Validation;
using Compelligence.Web.Models.Util;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class ProductFamilyController : BackEndAsyncFormController<ProductFamily, decimal>
    {

        #region Public Properties

        public IProductFamilyService ProductFamilyService
        {
            get { return (IProductFamilyService)_genericService; }
            set { _genericService = value; }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsFamily(decimal id)
        {

            IList<ProductFamily> pfList = ProductFamilyService.GetByCompetitor(id);
            return ControllerUtility.GetSelectOptionsFromGenericList<ProductFamily>(pfList, "Id", "Name");
        }

        #endregion

        #region Action Methods


        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(ProductFamily productfamily, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(productfamily.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ProductFamilyNameRequiredError);
            }

            return ValidationDictionary.IsValid;

        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
        }

        protected override void SetEntityDataToForm(ProductFamily criteria)
        {
            //
        }

        protected override void SetFormEntityDataToForm(ProductFamily criteria)
        {
            //
        }

        #endregion

        #region Private Methods
        #endregion

    }
}
