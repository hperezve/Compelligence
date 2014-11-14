using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
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
    public class KitController : BackEndAsyncFormController<Kit, decimal>
    {

        #region Public Properties

        public IKitService KitService
        {
            get { return (IKitService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IKitTypeService KitTypeService { get; set; }
        
        public IBudgetService BudgetService { get; set; }

        public IKitCompetitorService KitCompetitorService { get; set; }

        public IKitIndustryService KitIndustryService { get; set; }

        public IKitProductService KitProductService { get; set; }

        public IIndustryProductService IndustryProductService { get; set; }

        public IIndustryCompetitorService IndustryCompetitorService { get; set; }       


        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCompetitorsByIndustry(decimal id)
        {
            IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsByCompetitor(decimal id)
        {
            decimal industryId = Convert.ToDecimal(StringUtility.CheckNull(Request["IndustryId"]));

            IList<Product> productList = ProductService.GetByIndustryAndCompetitor(industryId, id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Kit kit, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(kit.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.KitNameRequiredError);
            }

            if (Validator.IsBlankOrNull(kit.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.KitAssignedToRequiredError);
            }

            if (!Validator.IsBlankOrNull(kit.BudgetFrm) && !Validator.IsDecimal(kit.BudgetFrm))
            {
                ValidationDictionary.AddError("BudgetFrm", LabelResource.KitBudgetFormatError);
            }

            if (!(Validator.IsBlankOrNull(kit.DueDateFrm) || Validator.IsDate(kit.DueDateFrm, GetFormatDate())))
            {
                ValidationDictionary.AddError("DueDateFrm", string.Format(LabelResource.KitDueDateFormatError, GetFormatDate()));
            }

            if (!ValidationDictionary.IsValid)
            {
                GetFormData(kit, formCollection);
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            //MigrateKits();
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];
            IList<ResourceObject> kitStatusList = ResourceService.GetAll<KitStatus>();
            IList<KitType> kitTypeList = KitTypeService.GetAllSortByClientCompany("Name", clientCompany);
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = IndustryService.GetAllActiveByClientCompany(clientCompany).Count;
            IList<Kit> kitParentList = KitService.GetAllSortByClientCompany("Name", clientCompany);

            ViewData["StatusList"] = new SelectList(kitStatusList, "Id", "Value");
            ViewData["TypeList"] = new SelectList(kitTypeList, "Id", "Name");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["IndustryIdMultiListCount"] = industryListCount;
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["KitParentList"] = new SelectList(kitParentList, "Id", "Name");
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;
            ViewData["CheckIndustryIds"] = false;
            
        }

        protected override void SetEntityDataToForm(Kit kit)
        {
            //obtain all industries which are related with this objective
            kit.KitIndustriesIds = obtainIndustriesForKit(kit.Id);
            string[] selectedValues = kit.KitIndustriesIds.Split(',');
            var selected = selectedValues;
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(kit.ClientCompany);
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);

            ViewData["BudgetFrm"] = FormatUtility.GetFormatValue("{0:0.##}", kit.Budget);
            ViewData["DueDateFrm"] = DateTimeUtility.ConvertToString(kit.DueDate, GetFormatDate());
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(kit.MetaData);
            ViewData["CreatedByFrm"] = UserProfileService.GetById(kit.CreatedBy).Name;
            SetCascadingData(kit, true);
            kit.OldKitIndustriesIds = kit.KitIndustriesIds;
            kit.OldKitCompetitorsIds = kit.KitCompetitorsIds;
            kit.OldKitProductsIds = kit.KitProductsIds;
            kit.OldName = kit.Name;

            IList<Kit> kitParentList = KitService.GetAllSortByClientCompany("Name", CurrentCompany);
            IList<Kit> kitWitoutChildren = new List<Kit>();
            foreach (Kit kitOnList in kitParentList)
            {
                if (kitOnList.Id != kit.Id)
                {
                    if (kitOnList.Parent == null)
                    {
                        kitWitoutChildren.Add(kitOnList);
                    }
                    else
                    {
                        decimal parent = (decimal)kitOnList.Parent;
                        while (parent != 0)
                        {
                            Kit kitTempo = KitService.GetById(parent);
                            if (kitTempo != null)
                            {
                                if (kitTempo.Id != kit.Id)
                                {
                                    if (kitTempo.Parent == null)
                                    {
                                        kitWitoutChildren.Add(kitOnList);
                                        parent = 0;
                                    }
                                    else
                                    {
                                        parent = (decimal)kitTempo.Parent;
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
            ViewData["KitParentList"] = new SelectList(kitWitoutChildren, "Id", "Name");
            SetDataToBudget(kit);
        }

        private Kit SetDataToBudget(Kit kit)
        {
            IList<Budget> budgetList = BudgetService.GetBudgetsByEntityIdAndType(kit.Id, DomainObjectType.Kit, CurrentCompany);
            if (budgetList.Count > 0)
            {
                decimal sumValueBudgetsFinancial = 0;
                decimal sumValueBudgetsTime = 0;
                string unitOfBudgetsFinancial = string.Empty;
                string unitOfBudgetsTime = string.Empty;
                foreach (Budget budgetUnit in budgetList)
                {
                    if (budgetUnit.Type == BudgetTypeUnit.Financial)
                    { unitOfBudgetsFinancial = budgetUnit.UnitMeasureCode; }
                    else if (budgetUnit.Type == BudgetTypeUnit.Time)
                    { unitOfBudgetsTime = budgetUnit.UnitMeasureCode; }

                }

                foreach (Budget budget in budgetList)
                {
                    if (budget.Type == BudgetTypeUnit.Financial && budget.UnitMeasureCode == unitOfBudgetsFinancial)
                    {
                        if (budget.Value == null)
                            budget.Value = 0;
                        sumValueBudgetsFinancial = sumValueBudgetsFinancial + (decimal)budget.Value;
                    }
                    else if (budget.Type == BudgetTypeUnit.Time && budget.UnitMeasureCode == unitOfBudgetsTime)
                    {
                        sumValueBudgetsTime = sumValueBudgetsTime + (decimal)budget.Value;
                    }
                }
                kit.FinancialBudgetUnit = ResourceService.GetName<BudgetTypeFinancial>(unitOfBudgetsFinancial);
                kit.TimeBudgetUnit = ResourceService.GetName<BudgetTypeTime>(unitOfBudgetsTime);
                kit.TotalFinancialBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgetsFinancial);
                kit.TotalTimeBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgetsTime);
              }
            return kit;
        }

        protected override void SetFormEntityDataToForm(Kit kit)
        {
            string[] selected = kit.KitIndustriesIds.Split(',');
            string checkedbyHierarchy = "";
            IList<IndustryByHierarchyView> industryListByHierarchy = new List<IndustryByHierarchyView>();
            IList<Industry> industryList = new List<Industry>();
            if (kit.CheckIndustryIds)
            {
                if (!kit.ClientCompany.Equals(""))
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy(kit.ClientCompany);
                }
                else
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryListByHierarchy, "Id", "Name", selected);
            }
            else
            {
                if (!kit.ClientCompany.Equals(""))
                {
                    industryList = IndustryService.GetAllActiveByClientCompany(kit.ClientCompany);
                }
                else
                {
                    industryList = IndustryService.GetAllActiveByClientCompany((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);
            }
            ViewData["CheckIndustryIds"] = kit.CheckIndustryIds;
            if (kit.CheckIndustryIds == true)
                checkedbyHierarchy = "true";
            else if (kit.CheckIndustryIds == false)
                checkedbyHierarchy = "false";

            ModelState.SetModelValue("checkedbyHierarchy", new ValueProviderResult(checkedbyHierarchy, checkedbyHierarchy, CultureInfo.InvariantCulture));
            SetCascadingData(kit, false);
            kit.MetaData = FormFieldsUtility.GetMultilineValue(kit.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(kit.MetaData, kit.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldKitIndustriesIds", new ValueProviderResult(kit.OldKitIndustriesIds, kit.OldKitIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldKitCompetitorsIds", new ValueProviderResult(kit.OldKitCompetitorsIds, kit.OldKitCompetitorsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldKitProductsIds", new ValueProviderResult(kit.OldKitProductsIds, kit.OldKitProductsIds, CultureInfo.InvariantCulture));
        }

        protected override void GetFormData(Kit kit, FormCollection collection)
        {
            kit.Budget = FormatUtility.GetDecimalValue(collection["BudgetFrm"]);
            kit.DueDate = DateTimeUtility.ConvertToDate(collection["DueDateFrm"], GetFormatDate());
            kit.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);

            string selectedIndustries = collection["KitIndustryIds"];
            string selectedCompetitors = collection["KitCompetitorIds"];
            string selectedProducts = collection["KitProductIds"];

            kit.OldKitIndustriesIds = obtainIndustriesForKit(kit.Id);
            kit.OldKitCompetitorsIds = obtainCompetitorsForKit(kit.Id);
            kit.OldKitProductsIds = obtainProductsForKit(kit.Id);


            kit.KitIndustriesIds = selectedIndustries;
            kit.KitCompetitorsIds = selectedCompetitors;
            kit.KitProductsIds = selectedProducts;
        }

        protected override void SetDefaultEntityDataForSave(Kit kit)
        {
            kit.MetaData = kit.Name + ":" + kit.MetaData;
            if (!string.IsNullOrEmpty(kit.KitIndustriesIds))
            {
                string[] industriesid = kit.KitIndustriesIds.Split(',');
                for (int i = 0; i < industriesid.Length; i++)
                {
                    Industry industry = IndustryService.GetById(decimal.Parse(industriesid[i]));
                    kit.MetaData += ":" + industry.Name;
                }
            }
            if (!string.IsNullOrEmpty(kit.KitCompetitorsIds))
            {
                string[] competitorsId = kit.KitCompetitorsIds.Split(',');
                for (int j = 0; j < competitorsId.Length; j++)
                {
                    Competitor competitor = CompetitorService.GetById(decimal.Parse(competitorsId[j]));
                    kit.MetaData += ":" + competitor.Name;
                }
            }
            if (!string.IsNullOrEmpty(kit.KitProductsIds))
            {
                string[] productsid = kit.KitProductsIds.Split(',');
                for (int k = 0; k < productsid.Length; k++)
                {
                    Product product = ProductService.GetById(decimal.Parse(productsid[k]));
                    kit.MetaData += ":" + product.Name;
                }
            }
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Kit;

            switch (detailType)
            {
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Kit);
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Budget:
                    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                    childController = "Budget";
                    break;
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                case DetailType.Metric:
                    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                    childController = "Metric";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Kit);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Kit);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Kit kit)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (KitService.HasAccessToKit(kit, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(Kit kit, bool action)
        {
            if (action)//action indicates if we get data from entity or from a form(edit action or create action)
            {
                kit.KitCompetitorsIds = obtainCompetitorsForKit(kit.Id);
                kit.KitProductsIds = obtainProductsForKit(kit.Id);
            }
            string[] selectedProducts = kit.KitProductsIds.Split(',');
            string[] selectedCompetitors = kit.KitCompetitorsIds.Split(',');

            IList<Competitor> competitorList = new List<Competitor>();
            IList<Product> productList = new List<Product>();

            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");

            string[] idsInd = kit.KitIndustriesIds.Split(',');
            string clientCompany = (string)Session["ClientCompany"];

            if (idsInd[0].Equals(""))
                idsInd = null;

            if (kit.KitIndustriesIds != null && !kit.KitIndustriesIds.Equals(""))
            {
                for (int i = 0; i < idsInd.Length; i++)
                {
                    IList<Competitor> tempCompetitorList = CompetitorService.GetByIndustryId(Convert.ToDecimal(idsInd[i]));
                    foreach (Competitor comp in tempCompetitorList)
                    {
                        if (competitorList.IndexOf(comp) == -1)
                        {
                            competitorList.Add(comp);
                        }
                    }
                }
            }
            if (kit.KitCompetitorsIds != null && !kit.KitCompetitorsIds.Equals(""))
            {
                for (int j = 0; j < selectedCompetitors.Length; j++)
                {
                    IList<Product> tempProductList = ProductService.GetByCompetitor(Convert.ToDecimal(selectedCompetitors[j]));
                    //obtain products by Industry
                    foreach (Product prod in tempProductList)
                    {
                        bool addProduct = false;
                        if (!kit.KitIndustriesIds.Equals(""))
                        {
                            //Verifying if Products from the Competitor are from Industries too
                            string[] actualIndustries = kit.KitIndustriesIds.Split(',');
                            foreach (string idInd in actualIndustries)
                            {
                                IndustryProductId idIndProd = new IndustryProductId(Convert.ToDecimal(idInd), prod.Id);
                                IndustryProduct tempIndProd = IndustryProductService.GetById(idIndProd);
                                if (tempIndProd != null)
                                {
                                    addProduct = true;
                                }
                            }
                        }
                        else
                        { addProduct = true; }

                        if (productList.IndexOf(prod) == -1 && addProduct)
                        {
                            productList.Add(prod);
                        }
                    }
                }
            }

            if (productList.Count > 0)
            {
                if (kit.KitProductsIds == null)
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name");
                else
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name", selectedProducts);
            }

            if (competitorList.Count > 0)
            {
                if (kit.KitCompetitorsIds == null)
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                else
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedCompetitors);
            }  
        }

        private string obtainCompetitorsForKit(decimal idKit)
        {
            string ids = null;
            IList<KitCompetitor> lstProjComp = KitCompetitorService.GetByKitId(idKit);
            int cont = 0;
            foreach (KitCompetitor projComp in lstProjComp)
            {
                cont++;

                if (lstProjComp.Count == cont)
                {
                    ids = ids + projComp.Id.CompetitorId.ToString();
                }
                else
                {
                    ids = ids + projComp.Id.CompetitorId + ",";
                }

            }
            return ids;
        }

        private string obtainProductsForKit(decimal idKit)
        {
            string ids = null;
            IList<KitProduct> lstKitProd = KitProductService.GetByKitId(idKit);
            int cont = 0;
            foreach (KitProduct kitProd in lstKitProd)
            {
                cont++;

                if (lstKitProd.Count == cont)
                {
                    ids = ids + kitProd.Id.ProductId.ToString();
                }
                else
                {
                    ids = ids + kitProd.Id.ProductId + ",";
                }
            }
            return ids;
        }

        private string obtainIndustriesForKit(decimal idKit)
        {
            string ids = null;
            IList<KitIndustry> lstProjInd = KitIndustryService.GetByKitId(idKit);
            int cont = 0;
            foreach (KitIndustry projInd in lstProjInd)
            {
                cont++;

                if (lstProjInd.Count == cont)
                {
                    ids = ids + projInd.Id.IndustryId.ToString();
                }
                else
                {
                    ids = ids + projInd.Id.IndustryId + ",";
                }

            }
            return ids;
        }
        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetCompetitorsOfIndustry(decimal id)
        {
            string result = string.Empty;
            IList<Competitor> lstCompetitors = CompetitorService.GetByIndustryId(id);
            int cont = 0;
            foreach (Competitor comp in lstCompetitors)
            {
                cont++;
                if (lstCompetitors.Count == cont)
                {
                    result = result + comp.Id + ":" + comp.Name;
                }
                else
                {
                    result = result + comp.Id + ":" + comp.Name + "_";
                }

            }
            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetMasiveCompetitors() //?ids=id1,id2,....
        {
            string ids = StringUtility.CheckNull(Request["ids"]);
            string[] industryids = ids.Split(',');
            string result = string.Empty;
            IList<Competitor> lstCompetitors = new List<Competitor>();
            foreach (string industryid in industryids) //is possible improve getting all data using unique query
            {
                lstCompetitors = lstCompetitors.Union(CompetitorService.GetByIndustryId(decimal.Parse(industryid))).ToList();
            }
            result += string.Join("_", lstCompetitors.Select(c => string.Format("{0}:{1}", c.Id, c.Name)).ToArray());
            return Content(result);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetMasiveProducts()
        {
            string result = string.Empty;
            string sidCompetitors = Request["idCompetitors"];
            string sidIndustries = Request["idIndustries"];

            decimal[] idIndustries = FormatUtility.GetDecimalArrayFromStringValues(sidIndustries.Split(','));
            decimal[] idCompetitors = FormatUtility.GetDecimalArrayFromStringValues(sidCompetitors.Split(','));

            IList<Product> lstProds = ProductService.GetByCompetitorAndIndustry(idCompetitors, idIndustries);

            result = string.Join("_", lstProds.Select(c => string.Format("{0}:{1}", c.Id, c.Name)).ToArray());
            return Content(result);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetProductsOfCompetitor(decimal id)
        {
            string result = string.Empty;
            string idKit = Request["idKit"];
            string idsIndustries = Request["idsIndustries"];
            string[] arrayIndustries = idsIndustries.Split(',');

            IList<Product> lstProds = ProductService.GetByCompetitor(id);
            int cont = 0;
            foreach (Product prod in lstProds)
            {
                cont++;
                bool addProd = false;

                if (!idsIndustries.Equals(""))
                {
                    if (prod.CompetitorId != null)
                    {
                        foreach (string idInd in arrayIndustries)
                        {
                            IndustryProductId idIndProd = new IndustryProductId(Convert.ToDecimal(idInd), prod.Id);
                            IndustryProduct tempIndProd = IndustryProductService.GetById(idIndProd);
                            if (tempIndProd != null)
                            {
                                addProd = true;
                            }
                        }
                    }
                }
                else
                {

                    addProd = true;
                }

                if (addProd)
                {

                    if (lstProds.Count == cont)
                    {
                        result = result + prod.Id + ":" + prod.Name;
                    }
                    else
                    {
                        result = result + prod.Id + ":" + prod.Name + "_";
                    }
                }
            }

            if (result.Length > 0 && result.Substring(result.Length - 1, 1).Equals("_"))
            {
                result = result.Substring(0, result.Length - 1);
            }
            return Content(result);
        }

        //it's for test delete or migrate any object
        //exist only for test
        public void MigrateKits()
        {
            IList<Kit> kits = KitService.GetAll();
            foreach (Kit p in kits)
            {
                //if (p.IndustryId != null)
                //{
                //    KitIndustry pi = new KitIndustry(new KitIndustryId((decimal)p.Id, (decimal)p.IndustryId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    KitIndustryService.Save(pi);
                //}
                //if (p.CompetitorId != null)
                //{
                //    KitCompetitor pi = new KitCompetitor(new KitCompetitorId((decimal)p.Id, (decimal)p.CompetitorId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    KitCompetitorService.Save(pi);
                //}
                //if (p.ProductId != null)
                //{
                //    KitProduct pi = new KitProduct(new KitProductId((decimal)p.Id, (decimal)p.ProductId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    KitProductService.Save(pi);
                //}

            }


        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetKitName(decimal id)
        {
            string result = string.Empty;
            Kit kit = KitService.GetById(id);
            if (kit != null) result = kit.Name;
            return Content(result);
        }
        #endregion

        public ContentResult ChangeIndustryList(bool IsChecked, string IndustryIds)
        {
            string[] selected = new string[] { };
            if ( !string.IsNullOrEmpty(IndustryIds) )
            {
                selected = IndustryIds.Split(',');
            }
            string result = string.Empty;
            bool isSelected = false;
            if (IsChecked)
            {
                IList<IndustryByHierarchyView> industryListHierarchy = IndustryService.FindAllActiveByHierarchy(CurrentCompany);

                int cont = 0;
                foreach (IndustryByHierarchyView industryHierarchy in industryListHierarchy)
                {
                    isSelected = false;
                    if (selected.Length > 0)
                    {
                        for (int m = 0; m < selected.Length; m++)
                        {
                            if (decimal.Parse(selected[m]) == industryHierarchy.Id)
                            {
                                isSelected = true;
                                m = selected.Length;
                            }
                        }
                    }

                    cont++;
                    if (industryListHierarchy.Count == cont)
                    {
                        result = result + industryHierarchy.Id + ":" + industryHierarchy.Name + ":" + isSelected.ToString();
                    }
                    else
                    {
                        result = result + industryHierarchy.Id + ":" + industryHierarchy.Name + ":" + isSelected.ToString() + "_";
                    }
                }
            }
            else
            {
                IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(CurrentCompany);
                int cont2 = 0;
                foreach (Industry industry in industryList)
                {
                    isSelected = false;
                    if (selected.Length > 0)
                    {
                        for (int m = 0; m < selected.Length; m++)
                        {
                            if (decimal.Parse(selected[m]) == industry.Id)
                            {
                                isSelected = true;
                                m = selected.Length;
                            }
                        }
                    }

                    cont2++;
                    if (industryList.Count == cont2)
                    {
                        result = result + industry.Id + ":" + industry.Name + ":" + isSelected.ToString();
                    }
                    else
                    {
                        result = result + industry.Id + ":" + industry.Name + ":" + isSelected.ToString() + "_";
                    }

                }
            }
            return Content(result);
        }
    }
}
