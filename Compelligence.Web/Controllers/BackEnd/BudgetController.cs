using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Util;

namespace Compelligence.Web.Controllers
{
    public class BudgetController : BackEndAsyncFormController<Budget, decimal>
    {

        #region Public Properties

        public IBudgetService BudgetService
        {
            get { return (IBudgetService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IUnitMeasureService UnitMeasureService { get; set; }

        public IBudgetTypeService BudgetTypeService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUnitsByType(string id, string detailfilter)
        {
            string[] parameter = detailfilter.Split('/');
            string[] parameterfilter = parameter[0].Split(':');
            if (parameterfilter.Length == 2)
            {
                if ((parameterfilter[0].IndexOf("EntityId") != -1) && (parameterfilter[1].IndexOf("EntityType") != -1))
                {
                    string[] parameterEntityId = parameterfilter[0].Split('_');
                    string[] parameterEntityType = parameterfilter[1].Split('_');
                    string entityId = parameterEntityId[2];
                    string entityType = parameterEntityType[2];
                    if (entityType.Equals(DomainObjectType.Project) || entityType.Equals(DomainObjectType.Objective) || entityType.Equals(DomainObjectType.Kit))
                    {
                        Budget budget = BudgetService.GetByEntityAndType(decimal.Parse(entityId), entityType, parameter[1], CurrentCompany);
                        if (budget != null)
                        {
                            IList<ResourceObject> listTempo = new List<ResourceObject>();
                            if (parameter[1].Equals(BudgetTypeUnit.Financial))
                            {
                                IList<ResourceObject> budgetTypeFinancial = ResourceService.GetAll<BudgetTypeFinancial>();
                                //int i;
                                foreach (ResourceObject objectResource in budgetTypeFinancial)
                                {
                                    if (objectResource.Id.Equals(budget.UnitMeasureCode))
                                    {
                                        listTempo.Add(objectResource);
                                        //i = budgetTypeFinancial.IndexOf(objectResource);
                                    }
                                }
                                //foreach (ResourceObject objectResource in budgetTypeFinancial)
                                //{
                                //    if (objectResource.Id != budget.UnitMeasureCode)
                                //    {
                                //        listTempo.Add(objectResource);
                                //        i = budgetTypeFinancial.IndexOf(objectResource);
                                //    }
                                //}
                            }
                            else if (parameter[1].Equals(BudgetTypeUnit.Time))
                            {
                                IList<ResourceObject> budgetTypeTime = ResourceService.GetAllBySortValue<BudgetTypeTime>();
                                foreach (ResourceObject objectResource in budgetTypeTime)
                                {
                                    if (objectResource.Id.Equals(budget.UnitMeasureCode))
                                    {
                                        listTempo.Add(objectResource);
                                    }
                                }
                            }
                            else
                            {
                                return null;
                            }
                            return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(listTempo, "Id", "Value");
                        }
                        else
                        {
                            if (parameter[1].Equals(BudgetTypeUnit.Financial))
                            {
                                IList<ResourceObject> budgetTypeFinancial = ResourceService.GetAll<BudgetTypeFinancial>();
                                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(budgetTypeFinancial, "Id", "Value");
                            }
                            else if (parameter[1].Equals(BudgetTypeUnit.Time))
                            {
                                IList<ResourceObject> budgetTypeTime = ResourceService.GetAllBySortValue<BudgetTypeTime>();
                                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(budgetTypeTime, "Id", "Value");
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }                                   
                }
            }
            if (parameter[1].Equals(BudgetTypeUnit.Financial))
            {
                IList<ResourceObject> budgetTypeFinancial = ResourceService.GetAll<BudgetTypeFinancial>();
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(budgetTypeFinancial, "Id", "Value");
            }
            else if (parameter[1].Equals(BudgetTypeUnit.Time))
            {
                IList<ResourceObject> budgetTypeTime = ResourceService.GetAllBySortValue<BudgetTypeTime>();
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(budgetTypeTime, "Id", "Value");
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Budget budget, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(budget.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.BudgetNameRequiredError);
            }

            if (Validator.IsBlankOrNull(budget.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.BudgetOwnerRequiredError);
            }
            if (!Validator.IsBlankOrNull(budget.ValueFrm) && !Validator.IsDecimal(budget.ValueFrm))
            {
                ValidationDictionary.AddError("ValueFrm", LabelResource.BudgetValueFormatError);
            }
            if (Validator.IsBlankOrNull(budget.Type))
            {
                ValidationDictionary.AddError("BudgetType", LabelResource.BudgetTypeRequiredError);
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> budgetTypeList = ResourceService.GetAll<BudgetTypeUnit>();
            //IList<BudgetType> budgetTypeList = BudgetTypeService.GetAllActiveByClientCompany(clientCompany);
            IList<ResourceObject> budgetStatusList = ResourceService.GetAll<BudgetStatus>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            //IList<UnitMeasure> unitMeasureList = UnitMeasureService.GetAll(GetDefaultBrowseFilter(typeof(UnitMeasure)));
            IList<UnitMeasure> unitMeasureList = UnitMeasureService.GetAllActive();

            //ViewData["TypeList"] = new SelectList(budgetTypeList, "Id", "Name");
            ViewData["TypeList"] = new SelectList(budgetTypeList, "Id", "Value");
            ViewData["StatusList"] = new SelectList(budgetStatusList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["UnitMeasureList"] = new SelectList(unitMeasureList, "Id", "Description");
            //SetFormDataBudgetType();
            //string detailfilter = (string)ViewData["DetailFilter"];
        }

        protected override void SetEntityDataToForm(Budget budget)
        {
            budget.OldValue = budget.Value;
            budget.OldType = budget.Type;
            ViewData["ValueFrm"] = FormatUtility.GetFormatValue("{0:0.##}", budget.Value);
            SetCascadingData(budget);
        }

        protected override void GetFormData(Budget budget, FormCollection collection)
        {
            //budget.OldValue = budget.Value;
            //budget.OldType = budget.Type;
            budget.Value = FormatUtility.GetDecimalValue(collection["ValueFrm"]);

            //ModelState.SetModelValue("OldValue", new ValueProviderResult((decimal)budget.OldValue, (decimal)budget.OldValue, CultureInfo.InvariantCulture));

            //ModelState.SetModelValue("OldType", new ValueProviderResult(budget.OldType, budget.OldType, CultureInfo.InvariantCulture));
        }

        protected override void SetFormEntityDataToForm(Budget budget)
        {
            budget.OldValue = budget.Value;
            budget.OldType = budget.Type;
            SetCascadingData(budget);
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(Budget budget)
        {
            string detailfilter = (string)ViewData["DetailFilter"];
            if (!string.IsNullOrEmpty(budget.Type))
            {
                if (budget.Type.Equals(BudgetTypeUnit.Financial))
                {
                    IList<ResourceObject> budgetTypeFinancial = ResourceService.GetAll<BudgetTypeFinancial>();
                    ViewData["UnitMeasureList"] = new SelectList(budgetTypeFinancial, "Id", "Value");
                }
                else if (budget.Type.Equals(BudgetTypeUnit.Time))
                {
                    IList<ResourceObject> budgetTypeTime = ResourceService.GetAllBySortValue<BudgetTypeTime>();
                    ViewData["UnitMeasureList"] = new SelectList(budgetTypeTime, "Id", "Value");
                }
            }
        }

        private void SetFormDataBudgetType()
        {
            string detailfilter = (string)ViewData["DetailFilter"];
            int posentityid = detailfilter.IndexOf("EntityId_Eq");
            int possepr = detailfilter.IndexOf(":Budget");
            int posentityType = detailfilter.IndexOf("EntityType_Eq");
            if ((posentityid != -1) && (posentityType != -1))
            {
                string entityId = detailfilter.Substring(posentityid + 12, possepr - (posentityid + 12));
                string entityType = detailfilter.Substring(posentityType + 14, detailfilter.Length - (posentityType + 14));
                if ((entityType.Equals(DomainObjectType.Project)) || (entityType.Equals(DomainObjectType.Industry)))
                {
                    //Budget budget = BudgetService.GetByEntityIdAndEntityType(decimal.Parse(entityId), entityType, CurrentCompany);
                    Budget budget = BudgetService.GetByEntityAndType(decimal.Parse(entityId), entityType, BudgetTypeUnit.Financial, CurrentCompany);

                    Budget budgetTime = BudgetService.GetByEntityAndType(decimal.Parse(entityId), entityType, BudgetTypeUnit.Time, CurrentCompany);
                    
                    if (budget != null)
                    {

                        ViewData["BudgetType"] = budget.Type;
                        //ViewData["BudgetUnit"] = budget.UnitMeasureCode;
                        ViewData["BudgetUnitFinancial"] = budget.UnitMeasureCode;
                        if (budget.Type.Equals(BudgetTypeUnit.Financial))
                        {
                            IList<ResourceObject> budgetTypeFinancial = ResourceService.GetAll<BudgetTypeFinancial>();
                            ViewData["UnitMeasureList"] = new SelectList(budgetTypeFinancial, "Id", "Value");
                        }
                        //else if (budget.Type.Equals(BudgetTypeUnit.Time))
                        //{
                        //    IList<ResourceObject> budgetTypeTime = ResourceService.GetAll<BudgetTypeTime>();
                        //    ViewData["UnitMeasureList"] = new SelectList(budgetTypeTime, "Id", "Value");
                        //}
                    }

                    if (budgetTime != null)
                    {
                        ViewData["BudgetUnitTime"] = budgetTime.UnitMeasureCode;
                        IList<ResourceObject> budgetTypeTime = ResourceService.GetAllBySortValue<BudgetTypeTime>();
                        ViewData["UnitMeasureList"] = new SelectList(budgetTypeTime, "Id", "Value");
                    }
                }
            }
        }
        #endregion
    }
}
