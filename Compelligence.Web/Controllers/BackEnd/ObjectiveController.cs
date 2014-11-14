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
    public class ObjectiveController : BackEndAsyncFormController<Objective, decimal>
    {
        #region Public Properties

        public IObjectiveService ObjectiveService
        {
            get { return (IObjectiveService)_genericService; }
            set { _genericService = value; }
        }

        public IIndustryService IndustryService { get; set; }

        public IProductService ProductService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IObjectiveCompetitorService ObjectiveCompetitorService { get; set; }

        public IObjectiveIndustryService ObjectiveIndustryService { get; set; }

        public IObjectiveProductService ObjectiveProductService { get; set; }

        public IIndustryProductService IndustryProductService { get; set; }

        public IIndustryCompetitorService IndustryCompetitorService { get; set; }

        public IBudgetService BudgetService { get; set; }


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

        protected override bool ValidateFormData(Objective objective, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(objective.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ObjectiveNameRequiredError);
            }

            if (!Validator.IsBlankOrNull(objective.BudgetFrm) && !Validator.IsDecimal(objective.BudgetFrm))
            {
                ValidationDictionary.AddError("BudgetFrm", LabelResource.ObjectiveBudgetFormatError);
            }

            if (Validator.IsBlankOrNull(objective.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.ObjectiveCreatedByRequiredError);
            }

            if (!(Validator.IsBlankOrNull(objective.DueDateFrm) || Validator.IsDate(objective.DueDateFrm, GetFormatDate())))
            {
                ValidationDictionary.AddError("DueDateFrm", string.Format(LabelResource.ObjectiveDueDateFormatError, GetFormatDate()));
            }

            if (!ValidationDictionary.IsValid)
            {
                GetFormData(objective, formCollection);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            //MigrateObjectives();
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = IndustryService.GetAllActiveByClientCompany(clientCompany).Count;
            IList<ResourceObject> objectiveTypeList = ResourceService.GetAll<ObjectiveType>();
            IList<ResourceObject> objectiveStatusList = ResourceService.GetAll<ObjectiveStatus>();
            IList<Objective> objetiveParentList = ObjectiveService.GetAllExceptWithAchievedStatus(clientCompany);

            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["IndustryIdMultiListCount"] = industryListCount;
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["StatusList"] = new SelectList(objectiveStatusList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["TypeList"] = new SelectList(objectiveTypeList, "Id", "Value");
            ViewData["ObjetiveParentList"] = new SelectList(objetiveParentList, "Id", "Name");
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;
            ViewData["CheckIndustryIds"] = false;
        }

        protected override void SetEntityDataToForm(Objective objective)
        {
            //obtain all industries which are related with this objective
            objective.IndustriesIds = obtainIndustriesForKit(objective.Id);
            string[] selectedValues = objective.IndustriesIds.Split(',');
            var selected = selectedValues;
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(objective.ClientCompany);
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);

            ViewData["BudgetFrm"] = FormatUtility.GetFormatValue("{0:0.##}", objective.Budget);
            ViewData["DueDateFrm"] = DateTimeUtility.ConvertToString(objective.DueDate, GetFormatDate());
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(objective.MetaData);
            ViewData["CreatedByFrm"] = UserProfileService.GetById(objective.CreatedBy).Name;
            objective.OldName = objective.Name;
            SetCascadingData(objective, true);
            objective.OldIndustriesIds = objective.IndustriesIds;
            objective.OldCompetitorsIds = objective.CompetitorsIds;
            objective.OldProductsIds = objective.ProductsIds;
            
            IList<Objective> objectiveParentList = ObjectiveService.GetAllExceptWithAchievedStatus(CurrentCompany);
            IList<Objective> objectiveWitoutChildren = new List<Objective>();
            foreach (Objective objectiveOnList in objectiveParentList)
            {
                if (objectiveOnList.Id != objective.Id)
                {
                    if (objectiveOnList.Parent == null)
                    {
                        objectiveWitoutChildren.Add(objectiveOnList);
                    }
                    else
                    {
                        decimal parent = (decimal)objectiveOnList.Parent;
                        while (parent != 0)
                        {
                            Objective objectiveTempo = ObjectiveService.GetById(parent);
                            if (objectiveTempo != null)
                            {
                                if (objectiveTempo.Id != objective.Id)
                                {
                                    if (objectiveTempo.Parent == null)
                                    {
                                        objectiveWitoutChildren.Add(objectiveOnList);
                                        parent = 0;
                                    }
                                    else
                                    {
                                        parent = (decimal)objectiveTempo.Parent;
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
            ViewData["ObjetiveParentList"] = new SelectList(objectiveWitoutChildren, "Id", "Name");
            SetDataToBudget(objective);
        }

        protected override void SetFormEntityDataToForm(Objective objective)
        {
            //obtain all industries which are related with this kit
            string[] selected = objective.IndustriesIds.Split(',');
            string checkedbyHierarchy = ""; 
            IList<IndustryByHierarchyView> industryListByHierarchy = new List<IndustryByHierarchyView>();
            IList<Industry> industryList = new List<Industry>();
            if (objective.CheckIndustryIds)
            {
                if (!objective.ClientCompany.Equals(""))
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy(objective.ClientCompany);
                }
                else
                {
                    industryListByHierarchy = IndustryService.FindIndustryHierarchy((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryListByHierarchy, "Id", "Name", selected);
            }
            else
            {
                if (!objective.ClientCompany.Equals(""))
                {
                    industryList = IndustryService.GetAllActiveByClientCompany(objective.ClientCompany);
                }
                else
                {
                    industryList = IndustryService.GetAllActiveByClientCompany((string)Session["ClientCompany"]);
                }
                ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);
            }
            ViewData["CheckIndustryIds"] = objective.CheckIndustryIds;
            if (objective.CheckIndustryIds == true)
                checkedbyHierarchy = "true";
            else if (objective.CheckIndustryIds == false)
                checkedbyHierarchy = "false";

            ModelState.SetModelValue("checkedbyHierarchy", new ValueProviderResult(checkedbyHierarchy, checkedbyHierarchy, CultureInfo.InvariantCulture));
            SetCascadingData(objective, false);
            objective.OldName = objective.Name;
            objective.MetaData = FormFieldsUtility.GetMultilineValue(objective.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(objective.MetaData, objective.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(objective.OldName, objective.OldName, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(objective.OldIndustriesIds, objective.OldIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorsIds", new ValueProviderResult(objective.OldCompetitorsIds, objective.OldCompetitorsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldProductsIds", new ValueProviderResult(objective.OldProductsIds, objective.OldProductsIds, CultureInfo.InvariantCulture));
        }

        protected override void GetFormData(Objective objective, FormCollection collection)
        {
            objective.Budget = FormatUtility.GetDecimalValue(collection["BudgetFrm"]);
            objective.DueDate = DateTimeUtility.ConvertToDate(collection["DueDateFrm"], GetFormatDate());
            objective.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);

            string selectedIndustries = collection["ObjectiveIndustryIds"];
            string selectedCompetitors = collection["ObjectiveCompetitorIds"];
            string selectedProducts = collection["ObjectiveProductIds"];




            objective.OldIndustriesIds = obtainIndustriesForKit(objective.Id);
            objective.OldCompetitorsIds = obtainCompetitorsForObjective(objective.Id);
            objective.OldProductsIds =  obtainProductsForObjective(objective.Id);

            objective.IndustriesIds = selectedIndustries;
            objective.CompetitorsIds = selectedCompetitors;
            objective.ProductsIds = selectedProducts;
        }

        protected override void SetDefaultEntityDataForSave(Objective objective)
        {

            objective.MetaData = objective.Name + ":" + objective.MetaData;
            if (!string.IsNullOrEmpty(objective.IndustriesIds))
            {
                string[] industriesid = objective.IndustriesIds.Split(',');
                for (int i = 0; i < industriesid.Length; i++)
                {
                    Industry industry = IndustryService.GetById(decimal.Parse(industriesid[i]));
                    objective.MetaData += ":" + industry.Name;
                }
            }
            if (!string.IsNullOrEmpty(objective.CompetitorsIds))
            {
                string[] competitorsId = objective.CompetitorsIds.Split(',');
                for (int j = 0; j < competitorsId.Length; j++)
                {
                    Competitor competitor = CompetitorService.GetById(decimal.Parse(competitorsId[j]));
                    objective.MetaData += ":" + competitor.Name;
                }
            }
            if (!string.IsNullOrEmpty(objective.ProductsIds))
            {
                string[] productsid = objective.ProductsIds.Split(',');
                for (int k = 0; k < productsid.Length; k++)
                {
                    Product product = ProductService.GetById(decimal.Parse(productsid[k]));
                    objective.MetaData += ":" + product.Name;
                }
            }
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Objective;

            switch (detailType)
            {
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Objective);
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                case DetailType.Metric:
                    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                    childController = "Metric";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Objective);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Objective);
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
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Budget:
                    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                    childController = "Budget";
                    break;
                case DetailType.Project:
                    AddFilter(detailFilter, "Project.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Project.ObjectiveId", parentId.ToString());
                    AddFilter(detailFilter, "Project.EntityType", DomainObjectType.Objective);
                    AddFilter(browseDetailFilter, "ProjectObjecitveDetailView.ObjectiveId", parentId.ToString());
                    childController = "Project:ProjectObjecitveDetail";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Objective objective)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (ObjectiveService.HasAccessToObjective(objective, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(Objective objective, bool action)
        {
            if (action)//action indicates if we get data from entity or from a form(edit action or create action)
            {
                objective.CompetitorsIds = obtainCompetitorsForObjective(objective.Id);
                objective.ProductsIds = obtainProductsForObjective(objective.Id);
            }
            string[] selectedProducts = objective.ProductsIds.Split(',');
            string[] selectedCompetitors = objective.CompetitorsIds.Split(',');

            IList<Competitor> competitorList = new List<Competitor>();
            IList<Product> productList = new List<Product>();

            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");

            string[] idsInd = objective.IndustriesIds.Split(',');
            string clientCompany = (string)Session["ClientCompany"];

            if (idsInd[0].Equals(""))
                idsInd = null;

            if (objective.IndustriesIds != null && !objective.IndustriesIds.Equals(""))
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
            if (objective.CompetitorsIds != null && !objective.CompetitorsIds.Equals(""))
            {
                for (int j = 0; j < selectedCompetitors.Length; j++)
                {
                    IList<Product> tempProductList = ProductService.GetByCompetitor(Convert.ToDecimal(selectedCompetitors[j]));
                    //obtain products by Industry
                    foreach (Product prod in tempProductList)
                    {
                        bool addProduct = false;
                        if (!objective.IndustriesIds.Equals(""))
                        {
                            //Verifying if Products from the Competitor are from Industries too
                            string[] actualIndustries = objective.IndustriesIds.Split(',');
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
                if (objective.ProductsIds == null)
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name");
                else
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name", selectedProducts);
            }

            if (competitorList.Count > 0)
            {
                if (objective.CompetitorsIds == null)
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                else
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedCompetitors);
            }  
        }

        private string obtainCompetitorsForObjective(decimal idObjective)
        {
            string ids = null;
            IList<ObjectiveCompetitor> lstObjComp = ObjectiveCompetitorService.GetByObjectiveId(idObjective);
            int cont = 0;
            foreach (ObjectiveCompetitor objComp in lstObjComp)
            {
                cont++;

                if (lstObjComp.Count == cont)
                {
                    ids = ids + objComp.Id.CompetitorId.ToString();
                }
                else
                {
                    ids = ids + objComp.Id.CompetitorId + ",";
                }
            }
            return ids;
        }

        private string obtainProductsForObjective(decimal idObjective)
        {
            string ids = null;
            IList<ObjectiveProduct> lstObjectiveProd = ObjectiveProductService.GetByObjectiveId(idObjective);
            int cont = 0;
            foreach (ObjectiveProduct objProd in lstObjectiveProd)
            {
                cont++;

                if (lstObjectiveProd.Count == cont)
                {
                    ids = ids + objProd.Id.ProductId.ToString();
                }
                else
                {
                    ids = ids + objProd.Id.ProductId + ",";
                }

            }
            return ids;
        }

        private string obtainIndustriesForKit(decimal idObjective)
        {
            string ids = null;
            IList<ObjectiveIndustry> lstObjInd = ObjectiveIndustryService.GetByObjectiveId(idObjective);
            int cont = 0;
            foreach (ObjectiveIndustry objInd in lstObjInd)
            {
                cont++;

                if (lstObjInd.Count == cont)
                {
                    ids = ids + objInd.Id.IndustryId.ToString();
                }
                else
                {
                    ids = ids + objInd.Id.IndustryId + ",";
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
            string idObjective = Request["idObjective"];
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
        public void MigrateObjectives()
        {
            IList<Objective> objectives = ObjectiveService.GetAll();
            foreach (Objective p in objectives)
            {
                //if (p.IndustryId != null)
                //{
                //    ObjectiveIndustry pi = new ObjectiveIndustry(new ObjectiveIndustryId((decimal)p.Id, (decimal)p.IndustryId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    ObjectiveIndustryService.Save(pi);
                //}
                //if (p.CompetitorId != null)
                //{
                //    ObjectiveCompetitor pi = new ObjectiveCompetitor(new ObjectiveCompetitorId((decimal)p.Id, (decimal)p.CompetitorId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    ObjectiveCompetitorService.Save(pi);
                //}
                //if (p.ProductId != null)
                //{
                //    ObjectiveProduct pi = new ObjectiveProduct(new ObjectiveProductId((decimal)p.Id, (decimal)p.ProductId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    ObjectiveProductService.Save(pi);
                //}
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Objective entity = ObjectiveService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }

        public ContentResult ChangeIndustryList(bool IsChecked, string IndustryIds)
        {
            string[] tempo = new string[] { };
            var selected = tempo;
            if (!string.IsNullOrEmpty(IndustryIds))
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
        private Objective SetDataToBudget(Objective objective)
        {
            IList<Budget> budgetList = BudgetService.GetBudgetsByEntityIdAndType(objective.Id, DomainObjectType.Objective, CurrentCompany);
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
                        if (budget.Value == null)
                        {
                            budget.Value = 0;
                        }
                        sumValueBudgetsTime = sumValueBudgetsTime + (decimal)budget.Value;
                    }
                }
                objective.FinancialBudgetUnit = ResourceService.GetName<BudgetTypeFinancial>(unitOfBudgetsFinancial);
                objective.TimeBudgetUnit = ResourceService.GetName<BudgetTypeTime>(unitOfBudgetsTime);
                objective.TotalFinancialBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgetsFinancial);
                objective.TotalTimeBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgetsTime);
            }
            return objective;
        }
        #endregion
    }
}
