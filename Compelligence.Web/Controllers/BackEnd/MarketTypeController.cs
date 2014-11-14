using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility.Web;
using Compelligence.Util.Validation;
using Resources;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class MarketTypeController : BackEndAsyncFormController<MarketType, decimal>
    {
        //
        // GET: /MarketType/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public IMarketTypeService MarketTypeService
        {
            get { return (IMarketTypeService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }
        public IUserProfileService UserProfileService { get; set; }

        #region Validation Methods

        protected override bool ValidateFormData(MarketType marketType, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (Validator.IsBlankOrNull(marketType.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.MarketTypeNameRequiredError);
            }

            if (Validator.IsBlankOrNull(marketType.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.MarketTypeAssignedToRequiredError);
            }
            //if (!Validator.IsBlankOrNull(industry.BudgetFrm) && !Validator.IsDecimal(industry.BudgetFrm))
            //{
            //    ValidationDictionary.AddError("BudgetFrm", LabelResource.IndustryBudgetFormatError);
            //}

            //if ((industry.Status.Equals(IndustryStatus.Disabled)) && (IndustryService.HassDependences(industry, clientCompany)))
            //{
            //    ValidationDictionary.AddError("Status", LabelResource.IndustryStatusValueError);
            //}
            //if (!Validator.IsBlankOrNull(industry.Parent) && (industry.Id != 0) && (IndustryService.IsChild((decimal)industry.Id, (decimal)industry.Parent)))
            //{
            //    ValidationDictionary.AddError("Parent", LabelResource.IndustryParentRequiredError);
            //}
            return ValidationDictionary.IsValid;
        }

        #endregion
        
        
        protected override void SetFormData()
        {
            IList<ResourceObject> marketTypeStatusList = ResourceService.GetAll<MarketTypeStatus>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(CurrentCompany);
            IList<MarketType> marketTypeParentList = MarketTypeService.GetAllSortByClientCompany("Name", CurrentCompany);

            ViewData["StatusList"] = new SelectList(marketTypeStatusList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["MarketTypeParentList"] = new SelectList(marketTypeParentList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(MarketType marketType)
        {
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(marketType.MetaData);
            IList<MarketType> marketTypeParentList = MarketTypeService.GetAllActiveByClientCompany(CurrentCompany);
            IList<MarketType> marketTypeWitoutChildren = new List<MarketType>();
            foreach (MarketType marketTypeOnList in marketTypeParentList)
            {
                if (marketTypeOnList.Id != marketType.Id)
                {
                    if (marketTypeOnList.Parent == null)
                    {
                        marketTypeWitoutChildren.Add(marketTypeOnList);
                    }
                    else
                    {
                        decimal parent = (decimal)marketTypeOnList.Parent;
                        while (parent != 0)
                        {
                            MarketType marketTypeTempo = MarketTypeService.GetById(parent);
                            if (marketTypeTempo != null)
                            {
                                if (marketTypeTempo.Id != marketType.Id)
                                {
                                    if (marketTypeTempo.Parent == null)
                                    {
                                        marketTypeWitoutChildren.Add(marketTypeOnList);
                                        parent = 0;
                                    }
                                    else
                                    {
                                        parent = (decimal)marketTypeTempo.Parent;
                                    }
                                }
                                else
                                {
                                    parent = 0;
                                }
                            }
                            else
                            {
                                parent = 0;
                            }
                        }
                    }
                }
            }
            ViewData["MarketTypeParentList"] = new SelectList(marketTypeWitoutChildren, "Id", "Name");
        }
        protected override void SetUserSecurityAccess(MarketType marketType)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (MarketTypeService.HasAccessToMarketType(marketType, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void GetFormData(MarketType marketType, FormCollection collection) {

            marketType.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        protected override void SetDefaultEntityDataForSave(MarketType marketType)
        {
            marketType.MetaData = marketType.Name + ":" + marketType.MetaData;
        }

        protected override void SetFormEntityDataToForm(MarketType marketType)
        {
            marketType.MetaData = FormFieldsUtility.GetMultilineValue(marketType.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(marketType.MetaData, marketType.MetaData, CultureInfo.InvariantCulture));
        }
    }
}
