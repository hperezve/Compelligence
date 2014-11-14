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
using Compelligence.Util.Type;
using Compelligence.Web.Models.Util;

namespace Compelligence.Web.Controllers
{
    public class PriceController : BackEndAsyncFormController<Price, decimal>
    {
        #region Public Properties

        public IPriceService PriceService
        {
            get { return (IPriceService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }
        public IPricingTypeService PricingTypeService { get; set; }
        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Price price, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(price.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.PriceNameRequiredError);
            }
            //if (Validator.IsBlankOrNull(price.Type))
            //{
            //    ValidationDictionary.AddError("Type", LabelResource.PriceTypeRequiredError);
            //}
            if (Validator.IsBlankOrNull(price.PricingType))
            {
                ValidationDictionary.AddError("PricingType", LabelResource.PriceTypeRequiredError);
            }
            else
            { 
                if(price.PricingType.Length>60)
                    ValidationDictionary.AddError("PricingType", LabelResource.PriceTypeLengthError);
            }
            if (Validator.IsBlankOrNull(price.Units))
            {
                ValidationDictionary.AddError("Units", LabelResource.PriceUnitsRequiredError);
            }
            if (Validator.IsBlankOrNull(price.ValueFrm))
            {
                ValidationDictionary.AddError("ValueFrm", LabelResource.PriceValueRequiredError);
            }
            else if (!Validator.IsFloat(price.ValueFrm))
            {
                ValidationDictionary.AddError("ValueFrm", LabelResource.PriceValueInputError);
            }
            if(!string.IsNullOrEmpty(price.Url))
            {
            if (!Validator.IsBlankOrNull(price.Url) && !Validator.IsValidUrl(price.Url))
            {
                ValidationDictionary.AddError("Url", LabelResource.GlobalUrlFormatError);
            }
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            IList<ResourceObject> typeList = ResourceService.GetAll<PriceType>();
            IList<ResourceObject> unitList = ResourceService.GetAll<PriceUnits>();
            IList<ResourceObject> statusList = ResourceService.GetAll<PriceStatus>();
            IList<ResourceObject> requiredList = ResourceService.GetAll<PriceRequired>();

            IList<ResourceObject> typeListNew = new List<ResourceObject>();
            foreach (ResourceObject ro in typeList)
            {
                if (ro.Id.Equals(PriceType.Product))
                {
                    typeListNew.Add(ro);
                }
            }

            foreach (ResourceObject ro in typeList)
            {
                if (!ro.Id.Equals(PriceType.Product))
                {
                    typeListNew.Add(ro);
                }
            }
            ViewData["TypeList"] = new SelectList(typeListNew, "Id", "Value");
            ViewData["UnitsList"] = new SelectList(unitList, "Id", "Value");
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            ViewData["RequiredList"] = new SelectList(requiredList, "Id", "Value");

            SetDefaultDataToForm();
        }

        protected override void SetEntityDataToForm(Price price)
        {
            ViewData["ValueFrm"] = FormatUtility.GetFormatValue("{0:0.##}", price.Value);
            if (price.PricingTypeId != null)
            {
                price.OldPricingTypeId = price.PricingTypeId;
                PricingType pricingType = PricingTypeService.GetById((decimal)price.PricingTypeId);
                if (pricingType != null) price.PricingType = pricingType.Label;
            }
        }

        protected override void GetFormData(Price price, FormCollection collection)
        {
            price.Value = FormatUtility.GetDecimalValue(collection["ValueFrm"]);
        }
        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        //public JsonResult GetPricingTypesToIndustry(decimal EntityId)
        public JsonResult GetPricingTypesToIndustry(string EntityId, string EntityType)
        {
            decimal entityId = string.IsNullOrEmpty(EntityId) ? 0 : decimal.Parse(EntityId);
            string clientCompany = (string)Session["ClientCompany"];
            IList<System.Object[]> pricingTypeList = PricingTypeService.GetByEntityIdAndEntityType(entityId, DomainObjectType.Product, clientCompany);
            return ControllerUtility.GetSelectOptionsFromGenericListOnlyObject(pricingTypeList);
        }
        #endregion
        private void SetDefaultDataToForm()
        {
            string entityId = GetDetailFilterValue("Price.EntityId");
            string entityType = GetDetailFilterValue("Price.EntityType");
            ViewData["EntityId"]=entityId;
            ViewData["EntityType"] = entityType;
            if (entityType.Equals(DomainObjectType.Product))
            {
                IList<Price> priceList = PriceService.GetListByEntityIdAndEntityType(decimal.Parse(entityId), entityType, CurrentCompany);
                if (priceList != null && priceList.Count >= 1)
                {
                    if (!string.IsNullOrEmpty(priceList[0].Units))
                    {
                        ResourceObject ro = new ResourceObject();
                        ro.Id = priceList[0].Units;
                        ro.Value = ResourceService.GetName<PriceUnits>(priceList[0].Units);
                        IList<ResourceObject> roList = new List<ResourceObject>();
                        roList.Add(ro);
                        ViewData["UnitsList"] = new SelectList(roList, "Id", "Value");
                    }
                }
            }
        }
    }
}
