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
using Compelligence.BusinessLogic.Implementation;

using Compelligence.Common.Browse;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    public class CompetitorFinancialController : BackEndAsyncFormController<CompetitorFinancial, decimal>
    {

        #region Public Properties

        public ICompetitorFinancialService CompetitorFinancialService
        {
            get { return (ICompetitorFinancialService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        #endregion

        #region Action Methods

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(CompetitorFinancial competitorFinancial, FormCollection formCollection)
        {
            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(CompetitorFinancial CompetitorFinancial, FormCollection formCollection)
        {
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Create(CompetitorFinancial entity, FormCollection collection)
        {
            CompetitorFinancialBalanceSheet entityBalanceSheet = new CompetitorFinancialBalanceSheet();
            CompetitorFinancialCashFlow entityCashflow = new CompetitorFinancialCashFlow();
            CompetitorFinancialIncomeStatement entityIncomeStatement = new CompetitorFinancialIncomeStatement();

            entityBalanceSheet.CompetitorId = entity.CompetitorId;
            entityCashflow.CompetitorId = entity.CompetitorId;
            entityIncomeStatement.CompetitorId = entity.CompetitorId;
                   
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateFormData(entity, collection))
            {
                DomainObject<decimal> domainObject = (DomainObject<decimal>)((object)entity);

                SetFormDataToEntity(entityBalanceSheet, collection);
                SetFormDataToEntity(entityCashflow, collection);
                SetFormDataToEntity(entityIncomeStatement, collection);

                SetDefaultDataForSave(entityBalanceSheet, entityCashflow, entityIncomeStatement);
                CompetitorFinancialService.SaveCompetitorFinancial(entityBalanceSheet, entityCashflow, entityIncomeStatement);

                decimal IdsEntity = Convert.ToDecimal(domainObject.Id);
         
                GetActionHistoryCreated(entity, collection);

                operationStatus = OperationStatus.Successful;

                return GetActionResultForCreate(domainObject, operationStatus);

            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entity);

            SetUserSecurityAccess();

            return View("Edit", entity);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditBS(decimal id, string operationStatus, FormCollection collection)
        {
            CompetitorFinancialBalanceSheet entityBalanceSheet = CompetitorFinancialService.GetFinancialBalanceSheetById(id);
                        
            CompetitorFinancial entityObject = GenericService.GetById(id);
            OperationStatus operationStatusParam = OperationStatus.Initiated;

            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();

            SetEntityDataToForm(entityBalanceSheet);

            SetUserSecurityAccess(entityObject);

            SetEntityLocking(entityObject);

            return View("Edit");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditCF(decimal id, string operationStatus, FormCollection collection)
        {
            CompetitorFinancialCashFlow entityCashflow = CompetitorFinancialService.GetFinancialCashFlowById(id);
            
            CompetitorFinancial entityObject = GenericService.GetById(id);
            OperationStatus operationStatusParam = OperationStatus.Initiated;

            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();

            SetEntityDataToForm(entityCashflow);

            SetUserSecurityAccess(entityObject);

            SetEntityLocking(entityObject);

            return View("Edit");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditIS(decimal id, string operationStatus, FormCollection collection)
        {
            CompetitorFinancialIncomeStatement entityIncomeStatement = CompetitorFinancialService.GetFinancialIncomeStatementById(id);

            CompetitorFinancial entityObject = GenericService.GetById(id);
            OperationStatus operationStatusParam = OperationStatus.Initiated;

            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();

            SetEntityDataToForm(entityIncomeStatement);

            SetUserSecurityAccess(entityObject);

            SetEntityLocking(entityObject);

            return View("Edit");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public override ActionResult Edit(decimal id, CompetitorFinancial formEntity, FormCollection collection)
        {
            string editView = FormFieldsUtility.SerializeValue(collection["EditView"]);

            CompetitorFinancialBalanceSheet entityResultBalanceSheet = new CompetitorFinancialBalanceSheet();
            CompetitorFinancialCashFlow entityResultCashflow = new CompetitorFinancialCashFlow();
            CompetitorFinancialIncomeStatement entityResultIncomeStatement = new CompetitorFinancialIncomeStatement();

            CompetitorFinancial entity = GenericService.GetById(id);

            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEditFormData(formEntity, collection))
            {
                if (editView == "All")
                {
                    CompetitorFinancialBalanceSheet entityBalanceSheet = CompetitorFinancialService.GetFinancialBalanceSheetById(id);
                    CompetitorFinancialCashFlow entityCashflow = CompetitorFinancialService.GetFinancialCashFlowById(id);
                    CompetitorFinancialIncomeStatement entityIncomeStatement = CompetitorFinancialService.GetFinancialIncomeStatementById(id);
                    SetFormDataToEntity(entityBalanceSheet, collection);
                    SetFormDataToEntity(entityCashflow, collection);
                    SetFormDataToEntity(entityIncomeStatement, collection);
                    SetDefaultDataForUpdate(entityBalanceSheet, entityCashflow, entityIncomeStatement);
                    CompetitorFinancialService.UpdateCompetitorFinancial(entityBalanceSheet, entityCashflow, entityIncomeStatement);
                    entityResultBalanceSheet = entityBalanceSheet;
                    entityResultCashflow = entityCashflow;
                    entityResultIncomeStatement = entityIncomeStatement;
                }
                else if (editView == "BS")
                {
                    CompetitorFinancialBalanceSheet entityBalanceSheet = CompetitorFinancialService.GetFinancialBalanceSheetById(id);
                    SetFormDataToEntity(entityBalanceSheet, collection);
                    SetDefaultDataForUpdate(entityBalanceSheet, null, null);
                    CompetitorFinancialService.UpdateCompetitorFinancial(entityBalanceSheet, null, null);
                    entityResultBalanceSheet = entityBalanceSheet;
                }
                else if (editView == "CF")
                {
                    CompetitorFinancialCashFlow entityCashflow = CompetitorFinancialService.GetFinancialCashFlowById(id);
                    SetFormDataToEntity(entityCashflow, collection);
                    SetDefaultDataForUpdate(null, entityCashflow, null);
                    CompetitorFinancialService.UpdateCompetitorFinancial(null, entityCashflow, null);
                    entityResultCashflow = entityCashflow;
                }
                else if (editView == "IS")
                {
                    CompetitorFinancialIncomeStatement entityIncomeStatement = CompetitorFinancialService.GetFinancialIncomeStatementById(id);
                    SetFormDataToEntity(entityIncomeStatement, collection);
                    SetDefaultDataForUpdate(null, null, entityIncomeStatement);
                    CompetitorFinancialService.UpdateCompetitorFinancial(null, null, entityIncomeStatement);
                    entityResultIncomeStatement = entityIncomeStatement;
                }

                //ActionHistoryService.BackEndActionHistory(IdsEntity, EntityAction.Updated, domainObjectEntity, domainObjectEntity, NameObject, CurrentUser, CurrentCompany);
                //GetActionHistoryOfFormData(entity, collection, EntityAction.Updated);

                

                operationStatus = OperationStatus.Successful;

            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

            SetFormData();

            //SetEntityDataToForm(entityResultBalanceSheet);
            //SetEntityDataToForm(entityResultCashflow);
            //SetEntityDataToForm(entityResultIncomeStatement);

            SetUserSecurityAccess();

            SetEntityLocking(entity);

            return GetActionResultForEdit(entity, operationStatus);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult DeleteIS(decimal id)
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();

            if (ids.Length == 1)
            {
                ids[0] = id.ToString();
            }

            foreach (object identifier in ids)
            {
                CompetitorFinancialIncomeStatement entity = CompetitorFinancialService.GetFinancialIncomeStatementById((decimal)Convert.ChangeType(identifier, typeof(decimal)));

                SetDefaultDataFromRequest(entity);
                CompetitorFinancialService.DeleteCompetitorFinancial(null,null,entity, userId);
            }

            if (errorMessage.Length > 0)
            {
                string[] messages = errorMessage.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                returnMessage.Append("<p><ul>");

                foreach (string message in messages)
                {
                    returnMessage.Append("<li>" + Server.HtmlEncode(message) + "</li>");
                }

                returnMessage.Append("</ul></p>");
            }

            return Content(returnMessage.ToString());
        }

        protected void SetDefaultDataFromRequest(CompetitorFinancialIncomeStatement entityObject)
        {
            entityObject.HeaderType = StringUtility.CheckNull(Request["HeaderType"]);
        }
        protected void SetDefaultDataFromRequest(CompetitorFinancialBalanceSheet entityObject)
        {
            entityObject.HeaderType = StringUtility.CheckNull(Request["HeaderType"]);
        }
        protected void SetDefaultDataFromRequest(CompetitorFinancialCashFlow entityObject)
        {
            entityObject.HeaderType = StringUtility.CheckNull(Request["HeaderType"]);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult DeleteBS(decimal id)
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();

            if (ids.Length == 1)
            {
                ids[0] = id.ToString();
            }

            foreach (object identifier in ids)
            {
                CompetitorFinancialBalanceSheet entity = CompetitorFinancialService.GetFinancialBalanceSheetById((decimal)Convert.ChangeType(identifier, typeof(decimal)));

                SetDefaultDataFromRequest(entity);
                CompetitorFinancialService.DeleteCompetitorFinancial(entity,null,null, userId);
            }

            if (errorMessage.Length > 0)
            {
                string[] messages = errorMessage.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                returnMessage.Append("<p><ul>");

                foreach (string message in messages)
                {
                    returnMessage.Append("<li>" + Server.HtmlEncode(message) + "</li>");
                }

                returnMessage.Append("</ul></p>");
            }

            return Content(returnMessage.ToString());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult DeleteCF(decimal id)
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();

            if (ids.Length == 1)
            {
                ids[0] = id.ToString();
            }

            foreach (object identifier in ids)
            {
                CompetitorFinancialCashFlow entity = CompetitorFinancialService.GetFinancialCashFlowById((decimal)Convert.ChangeType(identifier, typeof(decimal)));

                SetDefaultDataFromRequest(entity);
                CompetitorFinancialService.DeleteCompetitorFinancial(null,entity, null, userId);
            }

            if (errorMessage.Length > 0)
            {
                string[] messages = errorMessage.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                returnMessage.Append("<p><ul>");

                foreach (string message in messages)
                {
                    returnMessage.Append("<li>" + Server.HtmlEncode(message) + "</li>");
                }

                returnMessage.Append("</ul></p>");
            }

            return Content(returnMessage.ToString());
        }

        protected void SetFormDataToEntity(CompetitorFinancialBalanceSheet entityBalanceSheet, FormCollection collection)
        {
            entityBalanceSheet.PeriodEnding = Convert.ToDateTime(FormFieldsUtility.SerializeValue(collection["PeriodEnding"]));
            entityBalanceSheet.PeriodType = FormFieldsUtility.SerializeValue(collection["PeriodTypeList"]);
            entityBalanceSheet.CashAndCashEquivalents = FormFieldsUtility.SerializeValue(collection["BS1"]);
            entityBalanceSheet.ShortTermInvestments = FormFieldsUtility.SerializeValue(collection["BS2"]);
            entityBalanceSheet.NetReceivables = FormFieldsUtility.SerializeValue(collection["BS3"]);
            entityBalanceSheet.Inventory = FormFieldsUtility.SerializeValue(collection["BS4"]);
            entityBalanceSheet.OtherCurrentAssets = FormFieldsUtility.SerializeValue(collection["BS5"]);
            entityBalanceSheet.TotalCurrentAssets = FormFieldsUtility.SerializeValue(collection["BS6"]);
            entityBalanceSheet.LongTermInvestments = FormFieldsUtility.SerializeValue(collection["BS7"]);
            entityBalanceSheet.PropertyPlantandEquipment = FormFieldsUtility.SerializeValue(collection["BS8"]);
            entityBalanceSheet.Goodwill = FormFieldsUtility.SerializeValue(collection["BS9"]);
            entityBalanceSheet.IntangibleAssets = FormFieldsUtility.SerializeValue(collection["BS10"]);
            entityBalanceSheet.AccumulatedAmortization = FormFieldsUtility.SerializeValue(collection["BS11"]);
            entityBalanceSheet.OtherAssets = FormFieldsUtility.SerializeValue(collection["BS12"]);
            entityBalanceSheet.DeferredLongTermAssetCharges = FormFieldsUtility.SerializeValue(collection["BS13"]);
            entityBalanceSheet.TotalAssets = FormFieldsUtility.SerializeValue(collection["BS14"]);
            entityBalanceSheet.AccountsPayable = FormFieldsUtility.SerializeValue(collection["BS15"]);
            entityBalanceSheet.ShortCurrentLongTermDebt = FormFieldsUtility.SerializeValue(collection["BS16"]);
            entityBalanceSheet.OtherCurrentLiabilities = FormFieldsUtility.SerializeValue(collection["BS17"]);
            entityBalanceSheet.TotalCurrentLiabilities = FormFieldsUtility.SerializeValue(collection["BS18"]);
            entityBalanceSheet.LongTermDebt = FormFieldsUtility.SerializeValue(collection["BS19"]);
            entityBalanceSheet.OtherLiabilities = FormFieldsUtility.SerializeValue(collection["BS20"]);
            entityBalanceSheet.DeferredLongTermLiabilityCharges = FormFieldsUtility.SerializeValue(collection["BS21"]);
            entityBalanceSheet.MinorityInterest = FormFieldsUtility.SerializeValue(collection["BS22"]);
            entityBalanceSheet.NegativeGoodwill = FormFieldsUtility.SerializeValue(collection["BS23"]);
            entityBalanceSheet.TotalLiabilities = FormFieldsUtility.SerializeValue(collection["BS24"]);
            entityBalanceSheet.MiscStocksOptionsWarrants = FormFieldsUtility.SerializeValue(collection["BS25"]);
            entityBalanceSheet.RedeemablePreferredStock = FormFieldsUtility.SerializeValue(collection["BS26"]);
            entityBalanceSheet.PreferredStock = FormFieldsUtility.SerializeValue(collection["BS27"]);
            entityBalanceSheet.CommonStock = FormFieldsUtility.SerializeValue(collection["BS28"]);
            entityBalanceSheet.RetainedEarnings = FormFieldsUtility.SerializeValue(collection["BS29"]);
            entityBalanceSheet.TreasuryStock = FormFieldsUtility.SerializeValue(collection["BS30"]);
            entityBalanceSheet.CapitalSurplus = FormFieldsUtility.SerializeValue(collection["BS31"]);
            entityBalanceSheet.OtherStockholderEquity = FormFieldsUtility.SerializeValue(collection["BS32"]);
            entityBalanceSheet.TotaStockholderEquity = FormFieldsUtility.SerializeValue(collection["BS33"]);
            entityBalanceSheet.NetTangibleAssets = FormFieldsUtility.SerializeValue(collection["BS34"]);
        }

        protected void SetFormDataToEntity(CompetitorFinancialCashFlow entityCashflow, FormCollection collection)
        {
            entityCashflow.PeriodEnding = Convert.ToDateTime(FormFieldsUtility.SerializeValue(collection["PeriodEnding"]));
            entityCashflow.PeriodType = FormFieldsUtility.SerializeValue(collection["PeriodTypeList"]);
            entityCashflow.NetIncome = FormFieldsUtility.SerializeValue(collection["CF1"]);
            entityCashflow.Depreciation = FormFieldsUtility.SerializeValue(collection["CF2"]);
            entityCashflow.AdjustmentsToNetIncome = FormFieldsUtility.SerializeValue(collection["CF3"]);
            entityCashflow.ChangesInAccountsReceivables = FormFieldsUtility.SerializeValue(collection["CF4"]);
            entityCashflow.ChangesInLiabilities = FormFieldsUtility.SerializeValue(collection["CF5"]);
            entityCashflow.ChangesInInventories = FormFieldsUtility.SerializeValue(collection["CF6"]);
            entityCashflow.ChangesInOtherOperatingActivities = FormFieldsUtility.SerializeValue(collection["CF7"]);
            entityCashflow.TotalCashFlowFromOperatingActivities = FormFieldsUtility.SerializeValue(collection["CF8"]);
            entityCashflow.CapitalExpenditures = FormFieldsUtility.SerializeValue(collection["CF9"]);
            entityCashflow.Investments = FormFieldsUtility.SerializeValue(collection["CF10"]);
            entityCashflow.OtherCashflowsfromInvestingActivities = FormFieldsUtility.SerializeValue(collection["CF11"]);
            entityCashflow.TotalCashFlowsFromInvestingActivities = FormFieldsUtility.SerializeValue(collection["CF12"]);
            entityCashflow.DividendsPaid = FormFieldsUtility.SerializeValue(collection["CF13"]);
            entityCashflow.SalePurchaseofStock = FormFieldsUtility.SerializeValue(collection["CF14"]);
            entityCashflow.NetBorrowings = FormFieldsUtility.SerializeValue(collection["CF15"]);
            entityCashflow.OtherCashFlowsfromFinancingActivities = FormFieldsUtility.SerializeValue(collection["CF16"]);
            entityCashflow.TotalCashFlowsFromFinancingActivities = FormFieldsUtility.SerializeValue(collection["CF17"]);
            entityCashflow.EffectOfExchangeRateChanges = FormFieldsUtility.SerializeValue(collection["CF18"]);
            entityCashflow.ChangeInCashandCashEquivalents = FormFieldsUtility.SerializeValue(collection["CF19"]);
        }

        protected void SetFormDataToEntity(CompetitorFinancialIncomeStatement entityIncomeStatement, FormCollection collection)
        {
            entityIncomeStatement.PeriodEnding = Convert.ToDateTime(FormFieldsUtility.SerializeValue(collection["PeriodEnding"]));
            entityIncomeStatement.PeriodType = FormFieldsUtility.SerializeValue(collection["PeriodTypeList"]);
            entityIncomeStatement.TotalRevenue = FormFieldsUtility.SerializeValue(collection["IS1"]);
            entityIncomeStatement.CostofRevenue = FormFieldsUtility.SerializeValue(collection["IS2"]);
            entityIncomeStatement.GrossProfit = FormFieldsUtility.SerializeValue(collection["IS3"]);
            entityIncomeStatement.ResearchDevelopment = FormFieldsUtility.SerializeValue(collection["IS4"]);
            entityIncomeStatement.SellingGeneralandAdministrative = FormFieldsUtility.SerializeValue(collection["IS5"]);
            entityIncomeStatement.NonRecurring = FormFieldsUtility.SerializeValue(collection["IS6"]);
            entityIncomeStatement.Others = FormFieldsUtility.SerializeValue(collection["IS7"]);
            entityIncomeStatement.TotalOperatingExpenses = FormFieldsUtility.SerializeValue(collection["IS8"]);
            entityIncomeStatement.OperatingIncomeorLoss = FormFieldsUtility.SerializeValue(collection["IS9"]);
            entityIncomeStatement.TotalOtherIncomeExpensesNet = FormFieldsUtility.SerializeValue(collection["IS10"]);
            entityIncomeStatement.EarningsBeforeInterestAndTaxes = FormFieldsUtility.SerializeValue(collection["IS11"]);
            entityIncomeStatement.InterestExpense = FormFieldsUtility.SerializeValue(collection["IS12"]);
            entityIncomeStatement.IncomeBeforeTax = FormFieldsUtility.SerializeValue(collection["IS13"]);
            entityIncomeStatement.IncomeTaxExpense = FormFieldsUtility.SerializeValue(collection["IS14"]);
            string aaa = FormFieldsUtility.SerializeValue(collection["IS15"]);
            entityIncomeStatement.MinorityInterest = FormFieldsUtility.SerializeValue(collection["IS16"]);
            entityIncomeStatement.NetIncomeFromContinuingOps = FormFieldsUtility.SerializeValue(collection["IS17"]);
            entityIncomeStatement.DiscontinuedOperations = FormFieldsUtility.SerializeValue(collection["IS18"]);
            entityIncomeStatement.ExtraordinaryItems = FormFieldsUtility.SerializeValue(collection["IS19"]);
            entityIncomeStatement.EffectOfAccountingChanges = FormFieldsUtility.SerializeValue(collection["IS20"]);
            entityIncomeStatement.OtherItems = FormFieldsUtility.SerializeValue(collection["IS21"]);
            entityIncomeStatement.NetIncome = FormFieldsUtility.SerializeValue(collection["IS22"]);
            entityIncomeStatement.PreferredStockAndOtherAdjustments = FormFieldsUtility.SerializeValue(collection["IS23"]);
            entityIncomeStatement.NetIncomeApplicableToCommonShares = FormFieldsUtility.SerializeValue(collection["IS24"]);
        }

        protected void SetEntityDataToForm(CompetitorFinancialBalanceSheet entityBalanceSheet)
        {
            IList<ResourceObject> periodType = ResourceService.GetAll<FinancialPeriodType>();
            ViewData["PeriodType"] = new SelectList(periodType, "Id", "Value", entityBalanceSheet.PeriodType.Trim());
            ViewData["EditView"] = "BS";
            DateTime periodEnding = (DateTime)entityBalanceSheet.PeriodEnding;
            ViewData["PeriodEnding"] = periodEnding.ToShortDateString();
            ViewData["BS1"] = entityBalanceSheet.CashAndCashEquivalents;
            ViewData["BS2"] = entityBalanceSheet.ShortTermInvestments;
            ViewData["BS3"] = entityBalanceSheet.NetReceivables;
            ViewData["BS4"] = entityBalanceSheet.Inventory;
            ViewData["BS5"] = entityBalanceSheet.OtherCurrentAssets;
            ViewData["BS6"] = entityBalanceSheet.TotalCurrentAssets;
            ViewData["BS7"] = entityBalanceSheet.LongTermInvestments;
            ViewData["BS8"] = entityBalanceSheet.PropertyPlantandEquipment;
            ViewData["BS9"] = entityBalanceSheet.Goodwill;
            ViewData["BS10"] = entityBalanceSheet.IntangibleAssets;
            ViewData["BS11"] = entityBalanceSheet.AccumulatedAmortization;
            ViewData["BS12"] = entityBalanceSheet.OtherAssets;
            ViewData["BS13"] = entityBalanceSheet.DeferredLongTermAssetCharges;
            ViewData["BS14"] = entityBalanceSheet.TotalAssets;
            ViewData["BS15"] = entityBalanceSheet.AccountsPayable;
            ViewData["BS16"] = entityBalanceSheet.ShortCurrentLongTermDebt;
            ViewData["BS17"] = entityBalanceSheet.OtherCurrentLiabilities;
            ViewData["BS18"] = entityBalanceSheet.TotalCurrentLiabilities;
            ViewData["BS19"] = entityBalanceSheet.LongTermDebt;
            ViewData["BS20"] = entityBalanceSheet.OtherLiabilities;
            ViewData["BS21"] = entityBalanceSheet.DeferredLongTermLiabilityCharges;
            ViewData["BS22"] = entityBalanceSheet.MinorityInterest;
            ViewData["BS23"] = entityBalanceSheet.NegativeGoodwill;
            ViewData["BS24"] = entityBalanceSheet.TotalLiabilities;
            ViewData["BS25"] = entityBalanceSheet.MiscStocksOptionsWarrants;
            ViewData["BS26"] = entityBalanceSheet.RedeemablePreferredStock;
            ViewData["BS27"] = entityBalanceSheet.PreferredStock;
            ViewData["BS28"] = entityBalanceSheet.CommonStock;
            ViewData["BS29"] = entityBalanceSheet.RetainedEarnings;
            ViewData["BS30"] = entityBalanceSheet.TreasuryStock;
            ViewData["BS31"] = entityBalanceSheet.CapitalSurplus;
            ViewData["BS32"] = entityBalanceSheet.OtherStockholderEquity;
            ViewData["BS33"] = entityBalanceSheet.TotaStockholderEquity;
            ViewData["BS34"] = entityBalanceSheet.NetTangibleAssets;
        }

        protected void SetEntityDataToForm(CompetitorFinancialCashFlow entityCashflow)
        {
            IList<ResourceObject> periodType = ResourceService.GetAll<FinancialPeriodType>();
            ViewData["PeriodType"] = new SelectList(periodType, "Id", "Value", entityCashflow.PeriodType.Trim());
            ViewData["EditView"] = "CF";
            DateTime periodEnding = (DateTime)entityCashflow.PeriodEnding;
            ViewData["PeriodEnding"] = periodEnding.ToShortDateString();
            ViewData["CF1"] = entityCashflow.NetIncome;
            ViewData["CF2"] = entityCashflow.Depreciation;
            ViewData["CF3"] = entityCashflow.AdjustmentsToNetIncome;
            ViewData["CF4"] = entityCashflow.ChangesInAccountsReceivables;
            ViewData["CF5"] = entityCashflow.ChangesInLiabilities;
            ViewData["CF6"] = entityCashflow.ChangesInInventories;
            ViewData["CF7"] = entityCashflow.ChangesInOtherOperatingActivities;
            ViewData["CF8"] = entityCashflow.TotalCashFlowFromOperatingActivities;
            ViewData["CF9"] = entityCashflow.CapitalExpenditures;
            ViewData["CF10"] = entityCashflow.Investments;
            ViewData["CF11"] = entityCashflow.OtherCashflowsfromInvestingActivities;
            ViewData["CF12"] = entityCashflow.TotalCashFlowsFromInvestingActivities;
            ViewData["CF13"] = entityCashflow.DividendsPaid;
            ViewData["CF14"] = entityCashflow.SalePurchaseofStock;
            ViewData["CF15"] = entityCashflow.NetBorrowings;
            ViewData["CF16"] = entityCashflow.OtherCashFlowsfromFinancingActivities;
            ViewData["CF17"] = entityCashflow.TotalCashFlowsFromFinancingActivities;
            ViewData["CF18"] = entityCashflow.EffectOfExchangeRateChanges;
            ViewData["CF19"] = entityCashflow.ChangeInCashandCashEquivalents;
        }   
            
        protected void SetEntityDataToForm(CompetitorFinancialIncomeStatement entityIncomeStatement)
        {
            IList<ResourceObject> periodType = ResourceService.GetAll<FinancialPeriodType>();
            ViewData["PeriodType"] = new SelectList(periodType, "Id", "Value", entityIncomeStatement.PeriodType.Trim());
            ViewData["EditView"] = "IS";
            DateTime periodEnding = (DateTime)entityIncomeStatement.PeriodEnding;
            ViewData["PeriodEnding"] = periodEnding.ToShortDateString();
            ViewData["IS1"] = entityIncomeStatement.TotalRevenue;
            ViewData["IS2"] = entityIncomeStatement.CostofRevenue;
            ViewData["IS3"] = entityIncomeStatement.GrossProfit;
            ViewData["IS4"] = entityIncomeStatement.ResearchDevelopment;
            ViewData["IS5"] = entityIncomeStatement.SellingGeneralandAdministrative;
            ViewData["IS6"] = entityIncomeStatement.NonRecurring;
            ViewData["IS7"] = entityIncomeStatement.Others;
            ViewData["IS8"] = entityIncomeStatement.TotalOperatingExpenses;
            ViewData["IS9"] = entityIncomeStatement.OperatingIncomeorLoss;
            ViewData["IS10"] = entityIncomeStatement.TotalOtherIncomeExpensesNet;
            ViewData["IS11"] = entityIncomeStatement.EarningsBeforeInterestAndTaxes;
            ViewData["IS12"] = entityIncomeStatement.InterestExpense;
            ViewData["IS13"] = entityIncomeStatement.IncomeBeforeTax;
            ViewData["IS14"] = entityIncomeStatement.IncomeTaxExpense;
            ViewData["IS15"] = entityIncomeStatement.MinorityInterest;
            ViewData["IS16"] = entityIncomeStatement.NetIncomeFromContinuingOps;
            ViewData["IS17"] = entityIncomeStatement.DiscontinuedOperations;
            ViewData["IS18"] = entityIncomeStatement.ExtraordinaryItems;
            ViewData["IS19"] = entityIncomeStatement.EffectOfAccountingChanges;
            ViewData["IS20"] = entityIncomeStatement.OtherItems;
            ViewData["IS21"] = entityIncomeStatement.NetIncome;
            ViewData["IS22"] = entityIncomeStatement.PreferredStockAndOtherAdjustments;
            ViewData["IS23"] = entityIncomeStatement.NetIncomeApplicableToCommonShares;
        }

        protected void SetDefaultDataForSave(CompetitorFinancialBalanceSheet entityBalanceSheet, CompetitorFinancialCashFlow entityCashflow, CompetitorFinancialIncomeStatement entityIncomeStatement)
        {
            entityBalanceSheet.CreatedDate = DateTime.Now;
            entityBalanceSheet.ClientCompany = CurrentCompany;
            entityBalanceSheet.CreatedBy = CurrentUser;

            entityCashflow.CreatedDate = DateTime.Now;
            entityCashflow.ClientCompany = CurrentCompany;
            entityCashflow.CreatedBy = CurrentUser;

            entityIncomeStatement.CreatedDate = DateTime.Now;
            entityIncomeStatement.ClientCompany = CurrentCompany;
            entityIncomeStatement.CreatedBy = CurrentUser;

            SetDefaultDataForUpdate(entityBalanceSheet, entityCashflow, entityIncomeStatement);
        }

        protected void SetDefaultDataForUpdate(CompetitorFinancialBalanceSheet entityBalanceSheet, CompetitorFinancialCashFlow entityCashflow, CompetitorFinancialIncomeStatement entityIncomeStatement)
        {
            if (entityBalanceSheet != null)
            {
                entityBalanceSheet.LastChangedBy = CurrentUser;
                entityBalanceSheet.LastChangedDate = DateTime.Now;
            }
            if (entityCashflow != null)
            {
                entityCashflow.LastChangedBy = CurrentUser;
                entityCashflow.LastChangedDate = DateTime.Now;
            }
            if (entityIncomeStatement != null)
            {
                entityIncomeStatement.LastChangedBy = CurrentUser;
                entityIncomeStatement.LastChangedDate = DateTime.Now;
            }
        }

        protected ActionResult GetActionResultForCreate(CompetitorFinancial domainObject, OperationStatus operationStatus)
        {
            return RedirectToAction("Edit", new { id = domainObject.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], Container = Request["Container"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
        }
 
        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];

            IList<ResourceObject> financialView = ResourceService.GetAll<FinancialView>();
            ViewData["FinancialView"] = new SelectList(financialView, "Id", "Value");
            IList<ResourceObject> periodType = ResourceService.GetAll<FinancialTimePeriod>();
            ViewData["PeriodType"] = new SelectList(periodType, "Id", "Value", "Annual");
            decimal CompetitorId = Convert.ToDecimal(GetDetailFilterValue("CompetitorFinancial.CompetitorId"));
            ViewData["PeriodEnding"] = DateTime.Now.ToShortDateString();
            ViewData["CompetitorId"] = CompetitorId;
            ViewData["EditView"] = "All";
            ViewData["succesGet"] = "none";
            ViewData["grid"] = "_";
            int i = 0;
            for(i = 1;i<35;i++)
            {
                ViewData["BS" + i.ToString()] = string.Empty;
            }
            for (i = 1; i < 20; i++)
            {
                ViewData["CF" + i.ToString()] = string.Empty;
            }
            for (i = 1; i < 24; i++)
            {
                ViewData["IS" + i.ToString()] = string.Empty;
            }
            //IList<CompetitorFinancial> timePeriodYearList = CompetitorFinancialService.GetByTimePeriod(CompetitorId, FinancialTimePeriod.Year, CurrentCompany);
        }

        protected override void SetFormEntityDataToForm(CompetitorFinancial CompetitorFinancial)
        {

        }

        protected override void SetEntityDataToForm(CompetitorFinancial CompetitorFinancial)
        {

        }

        #endregion

        #region Private Methods



        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Index()
        {
            IList<ResourceObject> financialView = ResourceService.GetAll<FinancialView>();
            ViewData["FinancialView"] = new SelectList(financialView, "Id", "Value");

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public String GetData(String id)
        {
            decimal CompetitorId = Convert.ToDecimal(id);
            Competitor competitor = CompetitorService.GetById(CompetitorId);
            String succes = CompetitorFinancialService.SaveFinancialData(competitor).ToString();

            return succes;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult nDetailList(String id)
        {
            ViewData["Scope"] = "EnvironmentCompetitor";
            ViewData["HeaderType"] = "COMPT";

            ViewData["DetailFilter"] = "CompetitorFinancial.CompetitorId_" + id;
            ViewData["BrowseDetailName"] = Request["BrowseDetailName"];
            ViewData["BrowseDetailFilter"] = Request["BrowseDetailFilter"];
            ViewData["UserSecurityAccess"] = 3000;
            ViewData["EntityLocked"] = false;

            return View("DetailList");
        }

        #endregion

        private void SetCascadingData(CompetitorFinancial CompetitorFinancial)
        {

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public String GetNameCompetitor(String id)
        {
            decimal CompetitorId = Convert.ToDecimal(id);
            string result = string.Empty;
            Competitor competitor = CompetitorService.GetById(CompetitorId);
            if (competitor != null)
            {
                result = competitor.Name;
            }

            return result;
        }
    }
}
