using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using System.Text;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Util;
using Compelligence.Common.Utility.Web;
using System.Globalization;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class IndustryFinancialController : BackEndAsyncFormController<IndustryFinancial, decimal>
    {

        #region Public Properties

        public IIndustryFinancialService IndustryFinancialService
        {
            get { return (IIndustryFinancialService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IIndustryService IndustryService { get; set; }

        #endregion

        #region Action Methods

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(IndustryFinancial industryFinancial, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(industryFinancial.TimePeriod))
            {
                ValidationDictionary.AddError("TimePeriod", LabelResource.IndustryFinancialTimePeriodRequiredError);
            }
            if (Validator.IsBlankOrNull(industryFinancial.TimePeriodValue))
            {
                ValidationDictionary.AddError("TimePeriodValue", LabelResource.IndustryFinancialTimePeriodValueRequiredError);
            }
            else
            {
                if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Annual))
                {
                    if (IndustryFinancialService.GetExistIndustryFinancialYear((decimal)industryFinancial.IndustryId, industryFinancial.TimePeriod, industryFinancial.TimePeriodValue, CurrentCompany))
                    {
                        ValidationDictionary.AddError("TimePeriodValue", LabelResource.IndustryFinancialTimePeriodMatchError);
                    }
                }
                else if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarterly))
                {
                    if (Validator.IsBlankOrNull(industryFinancial.Year))
                    {
                        ValidationDictionary.AddError("Year", LabelResource.IndustryFinancialYearRequiredError);
                    }
                    else
                    {
                        if (IndustryFinancialService.GetExistIndustryFinancialQuarter((decimal)industryFinancial.IndustryId, industryFinancial.TimePeriod, industryFinancial.TimePeriodValue, industryFinancial.Year, CurrentCompany))
                        {
                            ValidationDictionary.AddError("TimePeriodValue", LabelResource.IndustryFinancialTimePeriodMatchError);
                        }
                    }
                }
            }
            if (Validator.IsBlankOrNull(industryFinancial.TotalAddressableMarketGlobal))
            {
                ValidationDictionary.AddError("TotalAddressableMarketGlobal", LabelResource.IndustryFinancialTotalAddressableMarketGlobalRequiredError);
            }
            else
            {
                if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarterly))
                {
                    if (IndustryFinancialService.TAMQuarterIsGreaterToTAMYear((decimal)industryFinancial.TotalAddressableMarketGlobal, string.Empty, (decimal)industryFinancial.IndustryId, FinancialTimePeriod.Quarterly, industryFinancial.Year, CurrentCompany))
                    {
                        ValidationDictionary.AddError("TotalAddressableMarketGlobal", LabelResource.IndustryFinancialTAMActualMatchError);
                    }
                }
            }
            if (Validator.IsBlankOrNull(industryFinancial.TotalAddressableMarketProjected))
            {
                ValidationDictionary.AddError("TotalAddressableMarketProjected", LabelResource.IndustryFinancialTotalAddressableMarketProjectedRequiredError);
            }
            if (!Validator.IsBlankOrNull(industryFinancial.CAGRActual))
            {
                if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarterly))
                {
                    if (IndustryFinancialService.CAGRQuarterIsGreaterToCAGRYear((decimal)industryFinancial.CAGRActual, string.Empty, (decimal)industryFinancial.IndustryId, FinancialTimePeriod.Quarterly, industryFinancial.Year, CurrentCompany))
                    {
                        ValidationDictionary.AddError("CAGRActual", LabelResource.IndustryFinancialCAGRActualMatchError);
                    }
                }
            }
            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(IndustryFinancial industryFinancial, FormCollection formCollection)
        {

            if (Validator.IsBlankOrNull(industryFinancial.TimePeriod))
            {
                ValidationDictionary.AddError("TimePeriod", LabelResource.IndustryFinancialTimePeriodRequiredError);
            }
            if (Validator.IsBlankOrNull(industryFinancial.TimePeriodValue))
            {
                ValidationDictionary.AddError("TimePeriodValue", LabelResource.IndustryFinancialTimePeriodValueRequiredError);
            }
            else
            {
                if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Annual))
                {
                    if (!industryFinancial.TimePeriod.Equals(industryFinancial.TimePeriodOld) || !industryFinancial.TimePeriodValue.Equals(industryFinancial.TimePeriodValueOld) || !industryFinancial.Year.Equals(industryFinancial.YearOld))
                    {
                        if (IndustryFinancialService.GetExistIndustryFinancialYear((decimal)industryFinancial.IndustryId, industryFinancial.TimePeriod, industryFinancial.TimePeriodValue, CurrentCompany))
                        {
                            ValidationDictionary.AddError("TimePeriodValue", LabelResource.IndustryFinancialTimePeriodMatchError);
                        }
                    }
                    
                }
                else if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarterly))
                {
                    if (Validator.IsBlankOrNull(industryFinancial.Year))
                    {
                        ValidationDictionary.AddError("Year", LabelResource.IndustryFinancialYearRequiredError);
                    }
                    else
                    {
                        if (!industryFinancial.TimePeriod.Equals(industryFinancial.TimePeriodOld) || !industryFinancial.TimePeriodValue.Equals(industryFinancial.TimePeriodValueOld) || !industryFinancial.Year.Equals(industryFinancial.YearOld))
                        {
                            if (IndustryFinancialService.GetExistIndustryFinancialQuarter((decimal)industryFinancial.IndustryId, industryFinancial.TimePeriod, industryFinancial.TimePeriodValue, industryFinancial.Year, CurrentCompany))
                            {
                                ValidationDictionary.AddError("TimePeriodValue", LabelResource.IndustryFinancialTimePeriodMatchError);
                            }
                        }
                    }
                }
            }
            if (Validator.IsBlankOrNull(industryFinancial.TotalAddressableMarketGlobal))
            {
                ValidationDictionary.AddError("TotalAddressableMarketGlobal", LabelResource.IndustryFinancialTotalAddressableMarketGlobalRequiredError);
            }
            else
            {
                if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarterly))
                {
                    if (IndustryFinancialService.TAMQuarterIsGreaterToTAMYear((decimal)industryFinancial.TotalAddressableMarketGlobal, industryFinancial.TotalAddressableMarketGlobalOld, (decimal)industryFinancial.IndustryId, FinancialTimePeriod.Quarterly, industryFinancial.Year, CurrentCompany))
                    {
                        ValidationDictionary.AddError("TotalAddressableMarketGlobal", LabelResource.IndustryFinancialTAMActualMatchError);
                    }
                }
            }
            if (Validator.IsBlankOrNull(industryFinancial.TotalAddressableMarketProjected))
            {
                ValidationDictionary.AddError("TotalAddressableMarketProjected", LabelResource.IndustryFinancialTotalAddressableMarketProjectedRequiredError);
            }
            if (!Validator.IsBlankOrNull(industryFinancial.CAGRActual))
            {
                if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarterly))
                {
                    if (IndustryFinancialService.CAGRQuarterIsGreaterToCAGRYear((decimal)industryFinancial.CAGRActual, industryFinancial.CAGRActualOld, (decimal)industryFinancial.IndustryId, FinancialTimePeriod.Quarterly, industryFinancial.Year, CurrentCompany))
                    {
                        ValidationDictionary.AddError("CAGRActual", LabelResource.IndustryFinancialCAGRActualMatchError);
                    }
                }
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> financialTimePeriodList = ResourceService.GetAll<FinancialTimePeriod>();
            IList<ResourceObject> financialQuarterList = ResourceService.GetAll<IndustryFinancialTypeQuarter>();
            ViewData["TimePeriodList"] = new SelectList(financialTimePeriodList, "Id", "Value");
            ViewData["FinancialQuarterList"] = new SelectList(financialQuarterList, "Id", "Value");
            decimal industryId = Convert.ToDecimal(GetDetailFilterValue("IndustryFinancial.IndustryId"));
            ViewData["IndustryId"] = industryId;
            IList<IndustryFinancial> timePeriodYearList = IndustryFinancialService.GetByTimePeriod(industryId, FinancialTimePeriod.Annual, CurrentCompany);
            ViewData["YearList"] = new SelectList(timePeriodYearList, "TimePeriodValue", "TimePeriodValue");
         }

        protected override void SetFormEntityDataToForm(IndustryFinancial industryFinancial)
        {
            industryFinancial.TimePeriodOld = industryFinancial.TimePeriod;
            industryFinancial.TimePeriodValueOld = industryFinancial.TimePeriodValue;
            industryFinancial.YearOld = industryFinancial.Year;
            industryFinancial.TotalAddressableMarketGlobalOld = industryFinancial.TotalAddressableMarketGlobal.ToString();
            industryFinancial.TotalAddressableMarketProjectedOld = industryFinancial.TotalAddressableMarketProjected.ToString();
            industryFinancial.CAGRActualOld = industryFinancial.CAGRActual.ToString();
            industryFinancial.CAGRProjectedOld = industryFinancial.CAGRProjected.ToString();
            ModelState.SetModelValue("TimePeriodOld", new ValueProviderResult(industryFinancial.TimePeriodOld, industryFinancial.TimePeriodOld, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("TimePeriodValueOld", new ValueProviderResult(industryFinancial.TimePeriodValueOld, industryFinancial.TimePeriodValueOld, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("YearOld", new ValueProviderResult(industryFinancial.YearOld, industryFinancial.YearOld, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("TotalAddressableMarketGlobalOld", new ValueProviderResult(industryFinancial.TotalAddressableMarketGlobalOld, industryFinancial.TotalAddressableMarketGlobalOld, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("TotalAddressableMarketProjectedOld", new ValueProviderResult(industryFinancial.TotalAddressableMarketProjectedOld, industryFinancial.TotalAddressableMarketProjectedOld, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("CAGRActualOld", new ValueProviderResult(industryFinancial.CAGRActualOld, industryFinancial.CAGRActualOld, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("CAGRProjectedOld", new ValueProviderResult(industryFinancial.CAGRProjectedOld, industryFinancial.CAGRProjectedOld, CultureInfo.InvariantCulture));
            SetCascadingData(industryFinancial);
        }

        protected override void SetEntityDataToForm(IndustryFinancial industryFinancial)
        {
            if (String.IsNullOrEmpty(industryFinancial.UseParentFigures) || industryFinancial.UseParentFigures.Equals("false"))
            {
                industryFinancial.UseParentFiguresVal = false;
            }
            else
            {
                industryFinancial.UseParentFiguresVal = true;
            }

            if (String.IsNullOrEmpty(industryFinancial.Required) || industryFinancial.Required.Equals("false"))
            {
                industryFinancial.RequiredVal = false;
            }
            else
            {
                industryFinancial.RequiredVal = true;
            }
            industryFinancial.TimePeriodOld = industryFinancial.TimePeriod;
            industryFinancial.TimePeriodValueOld = industryFinancial.TimePeriodValue;
            industryFinancial.YearOld = industryFinancial.Year;
            industryFinancial.TotalAddressableMarketGlobalOld = industryFinancial.TotalAddressableMarketGlobal.ToString();
            industryFinancial.TotalAddressableMarketProjectedOld = industryFinancial.TotalAddressableMarketProjected.ToString();
            industryFinancial.CAGRActualOld = industryFinancial.CAGRActual.ToString();
            industryFinancial.CAGRProjectedOld = industryFinancial.CAGRProjected.ToString();
            SetCascadingData(industryFinancial);
        }

        protected override void GetFormData(IndustryFinancial industryFinancial, FormCollection collection)
        {
            if (industryFinancial.UseParentFiguresVal)
            {
                industryFinancial.UseParentFigures = "true";
            }
            else {
                industryFinancial.UseParentFigures = "false";
            }

            if (industryFinancial.RequiredVal)
            {
                industryFinancial.Required = "true";
            }
            else
            {
                industryFinancial.Required = "false";
            }
        }

        //protected override void SetDefaultEntityDataForSave(IndustryFinancial industryFinancial)
        //{
        //    if (!string.IsNullOrEmpty(industryFinancial.TimePeriod))
        //    {
        //        if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarter))
        //        {
        //            industryFinancial.Year = DateTime.Now.Year.ToString();
        //        }
        //    }
        //}


        #endregion

        #region Private Methods

        

        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUpdateTimePeriodValue(string id)
        {
            if (id.Equals(FinancialTimePeriod.Annual))
            {
                
                IList<DateTime> timePeriodYearList = new List<DateTime>();
                int throughYear = 20 + DateTime.Now.Year;
                for (int i = DateTime.Today.Year; i <= 20 + DateTime.Now.Year; i++)
                {
                    timePeriodYearList.Add(new DateTime(i, 1, 1));
                }
                return ControllerUtility.GetSelectOptionsFromGenericList<DateTime>(timePeriodYearList, "Year", "Year");
            }
            else if (id.Equals(FinancialTimePeriod.Quarterly))
            {
                //IList<string> timeQuarterYearList = new List<string>();
                //for (int i = DateTime.Today.Year; i <= 20 + DateTime.Now.Year; i++)
                //{
                //    for (int j = 0; j < 4; j++)
                //    {
                //        timeQuarterYearList.Add("Q" + j.ToString()+ " " + i.ToString());
                //    }
                //}
                
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(ResourceService.GetAll<IndustryFinancialTypeQuarter>(), "Id", "Value");
            }
            else
            { return null; }
        }

        #endregion

        private void SetCascadingData(IndustryFinancial industryFinancial)
        {
            if (!string.IsNullOrEmpty(industryFinancial.TimePeriod))
            {
                if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Quarterly))
                {
                    IList<ResourceObject> financialQuarterList = ResourceService.GetAll<IndustryFinancialTypeQuarter>();
                    ViewData["TimePeriodValueList"] = new SelectList(financialQuarterList, "Id", "Value");
                }
                else if (industryFinancial.TimePeriod.Equals(FinancialTimePeriod.Annual))
                {
                    IList<DateTime> eventYearList = new List<DateTime>();
                    int throughYear = 20 + DateTime.Now.Year;
                    for (int i = DateTime.Today.Year; i <= throughYear; i++)
                    {
                        eventYearList.Add(new DateTime(i, 1, 1));
                    }
                    ViewData["TimePeriodValueList"] = new SelectList(eventYearList, "Year", "Year");
                }
            }
        }
    }
}
