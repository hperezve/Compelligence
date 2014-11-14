using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Views;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class StrengthWeaknessController : BackEndAsyncFormController<StrengthWeakness, decimal>
    {
        #region Public Properties

        public IIndustryService IndustryService { get; set; }

        public IStrengthWeaknessService StrengthWeaknessService
        {
            get { return (IStrengthWeaknessService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IStrengthWeaknessIndustryService StrengthWeaknessIndustryService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(StrengthWeakness strengthWeakness, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(strengthWeakness.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.StrengthWeaknessNameRequiredError);
            }
            if (Validator.IsBlankOrNull(strengthWeakness.Type))
            {
                ValidationDictionary.AddError("Type", LabelResource.StrengthWeaknessTypeRequired);
            }
            if (Validator.IsBlankOrNull(strengthWeakness.Description))
            {
                ValidationDictionary.AddError("Description", LabelResource.StrengthWeaknessDescriptionRequiredError);
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods
        protected override void SetFormData()
        {
            IList<ResourceObject> typeList = ResourceService.GetAll<StrengthWeaknessType>();
            ViewData["TypeList"] = new SelectList(typeList, "Id", "Value");

            string entityIdValue = GetDetailFilterValue("StrengthWeakness.EntityId");
            decimal competitorId = Convert.ToDecimal(entityIdValue);
            IndustryByHierarchyView industryByHierarchy = new IndustryByHierarchyView();
                industryByHierarchy.Id = -1;
                industryByHierarchy.Name = "Industry All(default)";
            IList<IndustryByHierarchyView> industryByHierarchyList = IndustryService.GetIndustryHierarchyByCompetitor(competitorId, CurrentCompany);
            if (industryByHierarchyList == null)
            {
                industryByHierarchyList = new List<IndustryByHierarchyView>();
                industryByHierarchyList.Add(industryByHierarchy);
            }
            else
            {
                industryByHierarchyList.Insert(0, industryByHierarchy);
            }
            ViewData["IndustryIdList"] = new SelectList(industryByHierarchyList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(StrengthWeakness strengthWeakness)
        {
            ViewData["LastDateFrm"] = DateTimeUtility.ConvertToString(strengthWeakness.LastDate, GetFormatDate());
            SetIndustryAndCompetitorValues(strengthWeakness);
        }

        protected override void SetFormEntityDataToForm(StrengthWeakness strengthWeakness)
        {
            SetIndustryAndCompetitorValues(strengthWeakness);
            strengthWeakness.OldIndustriesIds = strengthWeakness.IndustriesIds;
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(strengthWeakness.OldIndustriesIds, strengthWeakness.OldIndustriesIds, CultureInfo.InvariantCulture));
        }

        protected override void GetFormData(StrengthWeakness strengthWeakness, FormCollection collection)
        {
            strengthWeakness.LastDate = DateTimeUtility.ConvertToDate(collection["LastDateFrm"], GetFormatDate());

            string selectedIndustries = collection["IndustryIds"];

            if (!string.IsNullOrEmpty(selectedIndustries))
            {
                if (selectedIndustries.IndexOf("-1") == -1)
                {
                    strengthWeakness.IndustriesIds = selectedIndustries;
                }
                else
                {
                    strengthWeakness.IsGlobal = "Y";
                }
            }
            else
                strengthWeakness.IsGlobal = "Y";
        }

        #endregion

        private void SetIndustryAndCompetitorValues(StrengthWeakness strengthWeakness)
        {
            string entityIdValue = GetDetailFilterValue("StrengthWeakness.EntityId");
            string entityTypeValue = GetDetailFilterValue("StrengthWeakness.EntityType");
            if (!string.IsNullOrEmpty(entityTypeValue))
            {
                if (entityTypeValue.Equals(DomainObjectType.Competitor))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal competitorId = Convert.ToDecimal(entityIdValue);
                        strengthWeakness.IndustriesIds = GetIndustriesForStrengthWeakness(strengthWeakness.Id);
                        if (string.IsNullOrEmpty(strengthWeakness.IndustriesIds))
                        {
                            if (!string.IsNullOrEmpty(strengthWeakness.IsGlobal) && strengthWeakness.IsGlobal.Equals("Y"))
                            {
                                strengthWeakness.IndustriesIds = "-1";
                            }
                        }
                        strengthWeakness.OldIndustriesIds = strengthWeakness.IndustriesIds;
                        string[] selectedValues = strengthWeakness.IndustriesIds.Split(',');
                        var selected = selectedValues;
                        IList<IndustryByHierarchyView> industryByHierarchyList = IndustryService.GetIndustryHierarchyByCompetitorWithStatus(competitorId, CurrentCompany);
                        IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        industryAll.Name = "Industry All(default)";
                        industryAll.Id = -1;
                        industryByHierarchyList.Insert(0, industryAll);

                        ViewData["IndustryIdList"] = new MultiSelectList(industryByHierarchyList, "Id", "Name", selected);
                    }
                }
            }
        }

        private string GetIndustriesForStrengthWeakness(decimal idStrengthWeakness)
        {
            string ids = null;
            IList<StrengthWeakness_Industry> lstSWI = StrengthWeaknessIndustryService.GetByStrengthWeaknessId(idStrengthWeakness, CurrentCompany);
            int cont = 0;
            foreach (StrengthWeakness_Industry SWI in lstSWI)
            {
                cont++;

                if (lstSWI.Count == cont)
                {
                    ids = ids + SWI.Id.IndustryId.ToString();
                }
                else
                {
                    ids = ids + SWI.Id.IndustryId + ",";
                }
            }
            return ids;
        }
    }
}
