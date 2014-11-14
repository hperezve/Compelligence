using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class LabelController : BackEndAsyncFormController<Label, decimal>
    {

        #region Public Properties

        public ILabelService LabelService
        {
            get { return (ILabelService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Label label, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(label.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.LabelNameRequiredError);
            }

            if (!Validator.IsBlankOrNull(label.OrderFrm) && !Validator.IsDecimal(label.OrderFrm))
            {
                ValidationDictionary.AddError("OrderFrm", LabelResource.LabelOrderFormatError);
            }
            return ValidationDictionary.IsValid;

        }


        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            IList<ResourceObject> labelTypeList = ResourceService.GetAll<LabelType>();
            IList<ResourceObject> labelShowList = ResourceService.GetAll<LabelShow>();
            ViewData["TypeList"] = new SelectList(labelTypeList, "Id", "Value");

            //IList<object> options = new List<object>();
            //options.Add(new {Id = "Y", Value = "Yes" });
            //options.Add(new { Id = "N", Value = "No" });

            //ViewData["ShowList"] = new SelectList(options, "Id", "Value");
            ViewData["ShowList"] = new SelectList(labelShowList, "Id", "Value");
        }

        protected override void SetEntityDataToForm(Label label)
        {
            ViewData["OrderFrm"] = FormatUtility.GetFormatValue("{0:0.##}", label.Order);
        }

        protected override void GetFormData(Label label, FormCollection collection)
        {
            label.Order = FormatUtility.GetDecimalValue(collection["OrderFrm"]);
        }


        #endregion
    }
}
