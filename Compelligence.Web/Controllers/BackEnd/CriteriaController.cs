using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.DataTransfer.Entity;
using Compelligence.Util.Validation;
using Compelligence.Web.Models.Util;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;


namespace Compelligence.Web.Controllers
{
    public class CriteriaController : BackEndAsyncFormController<Criteria, decimal>
    {

        #region Public Properties

        public ICriteriaService CriteriaService
        {
            get { return (ICriteriaService)_genericService; }
            set { _genericService = value; }
        }

        public ICriteriaGroupService CriteriaGroupService { get; set; }

        public IProductCriteriaService ProductCriteriaService { get; set; }

        public IProductService ProductService { get; set; }

        public ICompetitorCriteriaService CompetitorCriteriaService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IIndustryCriteriasService IndustryCriteriasService { get; set; }

        public ICriteriaSetService CriteriaSetService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public IResourceService ResourceService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriaGroups( decimal IndustryId)
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<System.Object[]> criteriaGroupList = IndustryCriteriasService.GetByGroupbyIndustry(IndustryId, clientCompany);

            return ControllerUtility.GetSelectOptionsFromGenericListOnlyObject(criteriaGroupList);            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriaSetByCriteriaGroup(decimal id)
        {
            IList<CriteriaSet> criteriaSetList = CriteriaSetService.GetByCriteriaGroupId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<CriteriaSet>(criteriaSetList, "Id", "Name");
        }

        public ActionResult EditDetail()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["IsDetail"] = Request["IsDetail"];
            SetCriteriasDetails();
            return View("EditDetail");
        }

        public JsonResult GetBenefit(string CriteriaId)
        {
            Criteria Criteria = CriteriaService.GetById(decimal.Parse(CriteriaId));
            string Benefit = Criteria.Benefit;

            JsonResult jsonResult = new JsonResult();
            var objectResult = new object();
            
            objectResult = new
                {
                    Value = Benefit,
                    Text = Benefit
                };

            jsonResult.Data = objectResult;
            return jsonResult;
        }

        public JsonResult GetCost(string CriteriaId)
        {
            Criteria Criteria = CriteriaService.GetById(decimal.Parse(CriteriaId));
            string Cost = Criteria.Cost;

            JsonResult jsonResult = new JsonResult();
            var objectResult = new object();

            objectResult = new
            {
                Value = Cost,
                Text = Cost
            };

            jsonResult.Data = objectResult;
            return jsonResult;
        }


        //public ActionResult SaveDetail(Criteria entity, FormCollection collection)
        //{
        //    //string[] RequestCriteriasId = collection["CriteriasId"].Split(',');
        //    //string[] RequestIndustriesId = collection["IndustriesId"].Split(',');
        //    //string[] RequestDisplayable = collection["Displayable"].Split(',');
        //    //for (int i = 0; i < RequestIndustriesId.Length; i++)
        //    //{
        //    //    IndustryCriteriasId idIndCrit = new IndustryCriteriasId(Convert.ToDecimal(RequestIndustriesId[i]), Convert.ToDecimal(RequestCriteriasId[i]));
        //    //    IndustryCriterias modifyIndCrit = IndustryCriteriasService.GetById(idIndCrit);
        //    //    modifyIndCrit.OrderGroup = i;
        //    //    modifyIndCrit.OrderSet = i;
        //    //    modifyIndCrit.Visible = RequestDisplayable[i];

        //    //    IndustryCriteriasService.Update(modifyIndCrit);
        //    //}
            
        //    string[] RequestCriteriasGroupId = collection["CriteriasGroupId"].Split(',');
        //    string[] RequestIndustryId = collection["IndustriesId"].Split(',');
        //    string[] RequestCriteriasSetId = collection["CriteriaSetId"].Split(',');
        //    string[] RequestCriteriasSetIdByGroupId = collection["CriteriaSetIdByGroup"].Split(',');
        //    IList<string[]> SetByGroupList = new List<string[]>();
        //    for (int j = 0; j < RequestCriteriasSetIdByGroupId.Length; j++)
        //    {
        //        string[] criteriaGroupAndSet = RequestCriteriasSetIdByGroupId[j].Split('_');
        //        SetByGroupList.Add(criteriaGroupAndSet);
        //    }
        //    decimal industryId = Convert.ToDecimal(Session["IndustryId"].ToString());
        //    IList<IndustryCriterias> industryCriterias = IndustryCriteriasService.GetByIndustryId(industryId);
        //    for (int i = 0; i < RequestIndustryId.Length; i++)
        //    {
        //        decimal idGroup = Convert.ToDecimal(RequestCriteriasGroupId[i]);
        //        foreach (IndustryCriterias indCrit in industryCriterias)
        //        {
        //            if (indCrit.CriteriaGroupId != null && indCrit.CriteriaGroupId == idGroup && indCrit.OrderGroup != i)
        //            {
        //                    indCrit.OrderGroup = i;
        //                    IndustryCriteriasService.Update(indCrit);
        //            }
        //        }
        //    }
        //    //return null;
        //    ViewData["BrowseDetailName"] = "CriteriaIndustryDetail";
        //    ViewData["BrowseDetailFilter"] = "IndustryId_Eq_" + entity.IndustryId;
        //    return View("DetailList", entity);
        //}

        public ActionResult SaveDetail(Criteria entity, FormCollection collection)
        {
            //string[] RequestCriteriasId = collection["CriteriasId"].Split(',');
            //string[] RequestIndustriesId = collection["IndustriesId"].Split(',');
            //string[] RequestDisplayable = collection["Displayable"].Split(',');
            //for (int i = 0; i < RequestIndustriesId.Length; i++)
            //{
            //    IndustryCriteriasId idIndCrit = new IndustryCriteriasId(Convert.ToDecimal(RequestIndustriesId[i]), Convert.ToDecimal(RequestCriteriasId[i]));
            //    IndustryCriterias modifyIndCrit = IndustryCriteriasService.GetById(idIndCrit);
            //    modifyIndCrit.OrderGroup = i;
            //    modifyIndCrit.OrderSet = i;
            //    modifyIndCrit.Visible = RequestDisplayable[i];

            //    IndustryCriteriasService.Update(modifyIndCrit);
            //}

           
            string RequestIndustryId = collection["IndustriesId"];
           
            
            string[] RequestCriteriaId = collection["CriteriaId"].Split(',');
            




                    //order for criterias
            for (int i = 0; i < RequestCriteriaId.Count(); i++)
            {
                IndustryCriteriasId ici = new IndustryCriteriasId(decimal.Parse(RequestIndustryId), decimal.Parse(RequestCriteriaId[i]));
                IndustryCriterias ic = IndustryCriteriasService.GetById(ici);
                ic.Order = i;
                
                IndustryCriteriasService.Update(ic);
            }
            //
            return EditDetail();
        }
        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Criteria criteria, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(criteria.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.CriteriaNameRequiredError);
            }
            if (Validator.IsBlankOrNull(criteria.CriteriaGroupName))
            {
                ValidationDictionary.AddError("CriteriaGroupName", LabelResource.CriteriaGroupRequiredError);
            }
            if (Validator.IsBlankOrNull(criteria.CriteriaSetName))
            {
                ValidationDictionary.AddError("CriteriaSetName", LabelResource.CriteriaSetRequiredError);
            }
            if (Validator.IsBlankOrNull(criteria.Type))
            {
                ValidationDictionary.AddError("Type", LabelResource.CriteriaTypeRequiredError);
            }
            else 
            {
                if (criteria.Id != null && criteria.Id != 0 && !string.IsNullOrEmpty(criteria.OldType))
                {
                    if (!criteria.Type.Equals(criteria.OldType))
                    {
                        if (NoIsValidType(criteria))
                        {
                            ValidationDictionary.AddError("Type", LabelResource.CriteriaTypeUpdateError);
                        }
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
            decimal sIndustryId2 = 0;

            string DetailFilter = Request["DetailFilter"];

            if (DetailFilter.IndexOf("IndustryId_Eq_") > 0)
                 sIndustryId2 = Convert.ToDecimal(DetailFilter.Substring(23, DetailFilter.Length - 23));

            IList<ResourceObject> visibleList = ResourceService.GetAll<CriteriaVisible>();
            IList<CriteriaGroup> criteriaGroupList = CriteriaGroupService.GetAllActiveByClientCompany(clientCompany);
            IList<ResourceObject> criteriaVisibleList = ResourceService.GetAll<CriteriaVisible>();
            IList<ResourceObject> typeList = ResourceService.GetAll<CriteriaType>();
            IList<ResourceObject> relevancyList = ResourceService.GetAll<CriteriaRelevancy>();
            IList<ResourceObject> newRelevancyList = new List<ResourceObject>();
            newRelevancyList.Add(new ResourceObject(CriteriaRelevancy.High,"High"));
            newRelevancyList.Add(new ResourceObject( CriteriaRelevancy.Medium, "Medium"));
            newRelevancyList.Add(new ResourceObject(CriteriaRelevancy.Low, "Low"));
            IList<ResourceObject> newEmptyList = new List<ResourceObject>();
            IList<ResourceObject> criteriaMostDBooleanList = ResourceService.GetAll<CriteriaMostDesiredBoolean>();
            IList<ResourceObject> criteriaMostDNumericList = ResourceService.GetAll<CriteriaMostDesiredNumeric>();
            ViewData["VisibleList"] = new SelectList(visibleList, "Id", "Value", CriteriaVisible.Yes);
            ViewData["CriteriaGroupIdList"] = new SelectList(criteriaGroupList, "Id", "Name");
            ViewData["CriteriaVisibleList"] = new SelectList(criteriaVisibleList, "Id", "Value", CriteriaVisible.No);
            ViewData["CriteriaTypeList"] = new SelectList(typeList, "Id", "Value", CriteriaType.List);
            ViewData["CriteriaRelevancyList"] = new SelectList(newRelevancyList, "Id", "Value", CriteriaRelevancy.Medium);
            ViewData["CriteriaMostDesiredValueList"] = new SelectList(newEmptyList, "Id", "Value");
            ViewData["CriteriaMostDBooleanList"] = new SelectList(criteriaMostDBooleanList, "Id", "Value");
            ViewData["CriteriaMostDNumericList"] = new SelectList(criteriaMostDNumericList, "Id", "Value");
            ViewData["IndustryId"] = sIndustryId2;
        }

        protected override void GetFormData(Criteria criteria, FormCollection collection)
        { 
            string DetailFilter = Request["DetailFilter"];
            if (DetailFilter.IndexOf("IndustryId_Eq_") > 0)
            criteria.IndustryId = Convert.ToDecimal(DetailFilter.Substring(23, DetailFilter.Length - 23));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Duplicate()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');

            OperationStatus operationStatus = OperationStatus.Initiated;
            Criteria domainObject;
            decimal tempId = (decimal)Convert.ChangeType(ids[0], typeof(decimal));
            foreach (object identifier in ids)
            {
                Criteria entity = GenericService.GetById((decimal)Convert.ChangeType(identifier, typeof(decimal)));
                string DetailFilter = Request["DetailFilter"];
                if (DetailFilter != null && DetailFilter.IndexOf("IndustryId_Eq_") > 0)
                {
                    decimal idIndustry = Convert.ToDecimal(DetailFilter.Substring(23, DetailFilter.Length - 23));
                    entity.IndustryId = idIndustry;
                }
                SetDefaultDataFromRequest(entity);

                SetDefaultDataForSave(entity);

                domainObject = (Criteria)((object)GenericService.Duplicate(entity));
                tempId = domainObject.Id;
            }

            operationStatus = OperationStatus.Successful;
            return RedirectToAction("Edit", new { id = tempId, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"], Container = Request["Container"] });
        }

        //protected override void SetDefaultDataForSave(Criteria criteria)
        //{
        //    string DetailFilter = Request["DetailFilter"];
        //    if (DetailFilter != null && DetailFilter.IndexOf("IndustryId_Eq_") > 0)
        //    {
        //        decimal idIndustry = Convert.ToDecimal(DetailFilter.Substring(23, DetailFilter.Length - 23));
        //        criteria.IndustryId = idIndustry;
        //    }
        //}

        protected override void SetEntityDataToForm(Criteria criteria)
        {
            SetCascadingData(criteria);
            string DetailFilter = Request["DetailFilter"];
            if (DetailFilter != null && DetailFilter.IndexOf("IndustryId_Eq_") > 0)
            {
                decimal idIndustry = Convert.ToDecimal(DetailFilter.Substring(23, DetailFilter.Length - 23));
                criteria.IndustryId = idIndustry;
                IndustryCriteriasId id = new IndustryCriteriasId(idIndustry, criteria.Id);
                IndustryCriterias IndCrit = IndustryCriteriasService.GetById(id);
                if (IndCrit != null)
                {
                    if (IndCrit.CriteriaGroupId != null)
                    {
                        criteria.CriteriaGroupId = IndCrit.CriteriaGroupId;
                        CriteriaGroup group = CriteriaGroupService.GetById((decimal)criteria.CriteriaGroupId);
                        criteria.CriteriaGroupName = group.Name;
                    }

                    if (IndCrit.CriteriaSetId != null)
                    {
                        criteria.CriteriaSetId = IndCrit.CriteriaSetId;
                        CriteriaSet set = CriteriaSetService.GetById((decimal)criteria.CriteriaSetId);
                        criteria.CriteriaSetName = set.Name;
                    }

                    if (IndCrit.Visible != null)
                    {
                        criteria.Visible = IndCrit.Visible;
                    }
                }
            }
            if (criteria.CriteriaGroupId != null)
            {
                decimal idCriteriaGroup = (decimal)criteria.CriteriaGroupId;
                CriteriaGroup group = CriteriaGroupService.GetById(idCriteriaGroup);
                criteria.CriteriaGroupName = group.Name;
            }
            if (criteria.CriteriaSetId != null)
            {
                decimal idCriteriaSet = (decimal)criteria.CriteriaSetId;
                CriteriaSet group = CriteriaSetService.GetById(idCriteriaSet);
                criteria.CriteriaSetName = group.Name;
            }
            
            IList<ResourceObject> relevancyList = ResourceService.GetAll<CriteriaRelevancy>();
            ViewData["CriteriaRelevancyList"] = new SelectList(relevancyList, "Id", "Value", criteria.Relevancy);
            if (!string.IsNullOrEmpty(criteria.Type))
            {
                IList<ResourceObject> typeList = ResourceService.GetAll<CriteriaType>();
                ViewData["CriteriaTypeList"] = new SelectList(typeList, "Id", "Value", criteria.Type);

                if (criteria.Type.Equals(CriteriaType.Boolean))
                {
                    IList<ResourceObject> criteriaMostDBooleanList = ResourceService.GetAll<CriteriaMostDesiredBoolean>();
                    ViewData["CriteriaMostDesiredValueList"] = new SelectList(criteriaMostDBooleanList, "Id", "Value", criteria.MostDesiredValue);
                }
                else if (criteria.Type.Equals(CriteriaType.Numeric))
                {
                    IList<ResourceObject> criteriaMostDNumericList = ResourceService.GetAll<CriteriaMostDesiredNumeric>();
                    ViewData["CriteriaMostDesiredValueList"] = new SelectList(criteriaMostDNumericList, "Id", "Value", criteria.MostDesiredValue);
                }
                else
                {
                    IList<ResourceObject> newEmptyList = new List<ResourceObject>();
                    ViewData["CriteriaMostDesiredValueList"] = new SelectList(newEmptyList, "Id", "Value");
                }
            }
            criteria.OldType = criteria.Type;
            //ViewData["CriteriaMostDBooleanList"] = new SelectList(criteriaMostDBooleanList, "Id", "Value");
            //ViewData["CriteriaMostDNumericList"] = new SelectList(criteriaMostDNumericList, "Id", "Value");
        }

        protected override void SetFormEntityDataToForm(Criteria criteria)
        {
            SetCascadingData(criteria);
        }

        protected override bool ValidateDeleteData(Criteria entity, System.Text.StringBuilder errorMessage)
        {
            if (!ListRelated(entity.Id, entity.ClientCompany).Equals(""))
            {
                string lstEntities = ListRelated(entity.Id, entity.ClientCompany);
                errorMessage.AppendLine(LabelResource.RelatedToCriteriaCannotDeleteError + lstEntities + " ... and possibly more");
                return false;
            }
            return base.ValidateDeleteData(entity, errorMessage);
        }

        #endregion

        #region Private Methods

        private string ListRelated(decimal idCriteria, string clientCompany)
        {
            StringBuilder codeScript = new StringBuilder();
            string result = "";
            int numResults = 0;
            IList<ProductCriteria> lstProducts = ProductCriteriaService.GetByCriteriaId(idCriteria, clientCompany);
            if (lstProducts != null && lstProducts.Count > 0)
            {
                foreach (ProductCriteria prodCriteria in lstProducts)
                {
                    if (numResults < 6)
                    {
                        Product product = ProductService.GetById(prodCriteria.Id.ProductId);
                        codeScript.Append(" * PRODUCT: " + product.Name);
                        //result = result + HttpUtility.HtmlDecode("&lt;br&gt;") + "Product: " + product.Name;
                        numResults += numResults;
                    }
                }
            }

            if (numResults < 6)
            {
                IList<CompetitorCriteria> lstCompetitors = CompetitorCriteriaService.GetByCriteriaId(idCriteria, clientCompany);
                if (lstCompetitors != null && lstCompetitors.Count > 0)
                {
                    foreach (CompetitorCriteria compCriteria in lstCompetitors)
                    {
                        if (numResults < 6)
                        {
                            Competitor competitor = CompetitorService.GetById(compCriteria.Id.CompetitorId);
                            codeScript.Append(" * COMPETITOR: " + competitor.Name);
                            //result = result + HttpUtility.HtmlDecode("&lt;br&gt;") + "Competitor: " + competitor.Name;
                            numResults += numResults;
                        }
                    }
                }
            }

            return codeScript.ToString();
        }
        private void SetCascadingData(Criteria criteria)
        {
            if (!DecimalUtility.IsBlankOrNull(criteria.CriteriaGroupId))
            {
                IList<CriteriaSet> criteriaSetList = CriteriaSetService.GetByCriteriaGroupId(criteria.CriteriaGroupId.Value);

                ViewData["CriteriaSetIdList"] = new SelectList(criteriaSetList, "Id", "Name");
            }
        }

        private void SetCriteriasDetails()
        {
            decimal industryId = Convert.ToDecimal(Session["IndustryId"].ToString());
            Industry industry = IndustryService.GetById(industryId);

            IList<IndustryCriterias> industryCriterias = IndustryCriteriasService.GetByIndustryId(industryId, CurrentCompany);
            //Code from F
            CatalogOrderDTO catalogToOrder = new CatalogOrderDTO();
            
            CatalogCriteriaGroupDTO tmpCriteriaGroup;
            CatalogCriteriaSetDTO tmpCriteriaSet;
            CatalogCriteriaDTO tmpCriteria;
            int groupId;
            int setId;    
            catalogToOrder.IndustryId = industryId;
            
            //fill group
            foreach (IndustryCriterias indCriterias in industryCriterias) 
            {
                tmpCriteriaGroup = new CatalogCriteriaGroupDTO();
                tmpCriteriaSet = new CatalogCriteriaSetDTO();
                tmpCriteria = new CatalogCriteriaDTO();
                tmpCriteriaGroup.CriteriaGroupId = (decimal) (indCriterias.CriteriaGroupId ?? 0);
                if (indCriterias.CriteriaGroupId != null)
                {
                    CriteriaGroup tempGroup = CriteriaGroupService.GetById((decimal)indCriterias.CriteriaGroupId);
                    tmpCriteriaGroup.CriteriaGroupName = tempGroup.Name;
                }
                tmpCriteriaSet.CriteriaSetId = (decimal) (indCriterias.CriteriaSetId ?? 0);
                if (indCriterias.CriteriaSetId != null)
                {
                    CriteriaSet tempSet = CriteriaSetService.GetById((decimal)indCriterias.CriteriaSetId);
                    tmpCriteriaSet.CriteriaSetName = tempSet.Name;
                }
                tmpCriteria.CriteriaId = (decimal)(indCriterias.Id.CriteriaId);
                Criteria tempCriteria = CriteriaService.GetById((decimal)indCriterias.Id.CriteriaId);
                tmpCriteria.CriteriaName = tempCriteria.Name;
                tmpCriteria.CriteriaOrder = indCriterias.OrderGroup;

                //insert group
                if (catalogToOrder.LstCriteriaGroup == null)
                {
                    catalogToOrder.LstCriteriaGroup = new List<CatalogCriteriaGroupDTO>();
                }

                if (indCriterias.CriteriaGroupId == null)
                {
                    tmpCriteriaGroup.CriteriaGroupId = 0;
                    tmpCriteriaGroup.CriteriaGroupName = "Without Group";
                    groupId = catalogToOrder.SearchGroupById(tmpCriteriaGroup.CriteriaGroupId);
                    if (groupId == -1)
                        catalogToOrder.LstCriteriaGroup.Add(tmpCriteriaGroup);
                }
                else
                {
                    groupId = catalogToOrder.SearchGroupById(tmpCriteriaGroup.CriteriaGroupId);
                    if (groupId == -1)
                        catalogToOrder.LstCriteriaGroup.Add(tmpCriteriaGroup);
                }
                //insert set
                groupId = catalogToOrder.SearchGroupById(tmpCriteriaGroup.CriteriaGroupId);
                
                if (catalogToOrder.LstCriteriaGroup[groupId].LstCriteriaSet == null)
                {
                    catalogToOrder.LstCriteriaGroup[groupId].LstCriteriaSet = new List<CatalogCriteriaSetDTO>();
                }
                
                if (indCriterias.CriteriaSetId == null)
                {
                    tmpCriteriaSet.CriteriaSetId = 0;
                    tmpCriteriaSet.CriteriaSetName = "Without Set";
                    setId = catalogToOrder.LstCriteriaGroup[groupId].SearchSetById(tmpCriteriaSet.CriteriaSetId);
                    if(setId == -1)
                        catalogToOrder.LstCriteriaGroup[groupId].LstCriteriaSet.Add(tmpCriteriaSet);
                }
                else
                {
                    setId = catalogToOrder.LstCriteriaGroup[groupId].SearchSetById(tmpCriteriaSet.CriteriaSetId);
                    if (setId == -1)
                        catalogToOrder.LstCriteriaGroup[groupId].LstCriteriaSet.Add(tmpCriteriaSet);
                }
                //insert criteria
                setId = catalogToOrder.LstCriteriaGroup[groupId].SearchSetById(tmpCriteriaSet.CriteriaSetId);
                if (catalogToOrder.LstCriteriaGroup[groupId].LstCriteriaSet[setId].LstCriteria == null)
                {
                    catalogToOrder.LstCriteriaGroup[groupId].LstCriteriaSet[setId].LstCriteria = new List<CatalogCriteriaDTO>();
                }
                catalogToOrder.LstCriteriaGroup[groupId].LstCriteriaSet[setId].LstCriteria.Add(tmpCriteria);
            }

            ViewData["OrderedCatalogCriteria"] = catalogToOrder;

            //NEW CODE
            IList<CriteriaGroupContentDTO> lstGroupsToOrder = new List<CriteriaGroupContentDTO>();
            decimal tempCriteriaGroupId = 0;
            decimal tempCriteriaSetId = 0;
            bool sameGroup = false;
            bool sameSet = false;
            decimal cont = 0;
            CriteriaGroupContentDTO actualCriteriaGroup = null;

            foreach (IndustryCriterias indCriterias in industryCriterias)
            {
                cont += 1;
                // if it's the same group
                if (indCriterias.CriteriaGroupId != null && indCriterias.CriteriaGroupId > 0 && tempCriteriaSetId > 0 &&
                    indCriterias.CriteriaGroupId == tempCriteriaGroupId)
                {
                    sameGroup = true;
                    if(indCriterias.CriteriaSetId != tempCriteriaSetId)
                    {
                        sameSet = false;
                    }
                    if (!sameSet && indCriterias.CriteriaSetId != null && indCriterias.CriteriaSetId > 0)
                    {
                        if (actualCriteriaGroup.LstCriteriaSet == null)
                        {
                            actualCriteriaGroup.LstCriteriaSet = new List<CriteriaSet>();
                        }
                        CriteriaSet tempSet = CriteriaSetService.GetById((decimal)indCriterias.CriteriaSetId);
                        if(!actualCriteriaGroup.LstCriteriaSet.Contains(tempSet))
                        actualCriteriaGroup.LstCriteriaSet.Add(tempSet);
                    }
                }

                //  if it's a diferente group
                if (indCriterias.CriteriaGroupId != null && indCriterias.CriteriaGroupId > 0 &&
                    indCriterias.CriteriaGroupId != tempCriteriaGroupId)
                {
                    if (tempCriteriaGroupId > 0)
                    {
                        
                        sameGroup = false;
                        
                    }
                    else
                    {
                        sameGroup = true;
                        
                    }

                    sameSet = false;
                    if (tempCriteriaGroupId > 0 && !sameGroup)
                        lstGroupsToOrder.Add(actualCriteriaGroup);
                    actualCriteriaGroup = null;
                    actualCriteriaGroup = new CriteriaGroupContentDTO();
                    actualCriteriaGroup.CriteriaGroupId = (decimal)indCriterias.CriteriaGroupId;
                    CriteriaGroup tempGroup = CriteriaGroupService.GetById((decimal)indCriterias.CriteriaGroupId);
                    actualCriteriaGroup.CriteriaGroupName = tempGroup.Name;
                    actualCriteriaGroup.IndustryId = indCriterias.Id.IndustryId;
                    tempCriteriaGroupId = (decimal)(indCriterias.CriteriaGroupId ?? 0);
                    tempCriteriaSetId = (decimal)(indCriterias.CriteriaSetId ?? 0);

                    if (!sameSet && indCriterias.CriteriaSetId != null && indCriterias.CriteriaSetId > 0)
                    {
                        if (actualCriteriaGroup.LstCriteriaSet == null)
                        {
                            actualCriteriaGroup.LstCriteriaSet = new List<CriteriaSet>();
                        }
                        CriteriaSet tempSet = CriteriaSetService.GetById((decimal)indCriterias.CriteriaSetId);
                        actualCriteriaGroup.LstCriteriaSet.Add(tempSet);
                    }


                
                }

                if (industryCriterias.Count == cont && actualCriteriaGroup != null)
                {
                    lstGroupsToOrder.Add(actualCriteriaGroup);
                }


            }


            ViewData["OrderedCriteriaGroups"] = lstGroupsToOrder.ToList<CriteriaGroupContentDTO>();

            // OLD CODE
            IList<IndustryCriterias> industryCriteriasToForm = new List<IndustryCriterias>();
            foreach (IndustryCriterias indCriterias in industryCriterias)
            {
                Criteria criteria = CriteriaService.GetById(indCriterias.Id.CriteriaId);
                indCriterias.CriteriaName = criteria.Name;
                if (indCriterias.CriteriaGroupId != null && indCriterias.CriteriaSetId != null)
                {

                    CriteriaGroup group = CriteriaGroupService.GetById((decimal)indCriterias.CriteriaGroupId);
                    if (group != null)
                    {
                        indCriterias.GroupName = group.Name;
                    }
                    CriteriaSet set = CriteriaSetService.GetById((decimal)indCriterias.CriteriaSetId);
                    if (set != null)
                    {
                        indCriterias.SetName = set.Name;
                    }
                }
                else if (indCriterias.CriteriaGroupId != null && indCriterias.CriteriaSetId == null)
                {
                    CriteriaGroup group = CriteriaGroupService.GetById((decimal)indCriterias.CriteriaGroupId);
                    if (group != null)
                    {
                        indCriterias.GroupName = group.Name;
                    }
                }

                industryCriteriasToForm.Add(indCriterias);
            }

            var detailsOrder = (from d in industryCriteriasToForm orderby d.OrderGroup select d);
            ViewData["OrderedCriteriaDetails"] = detailsOrder.ToList<IndustryCriterias>();
        }

        private bool NoIsValidType(Criteria criteria)
        {
            bool result = true;
            string industryIdValue = GetDetailFilterValue("Criteria.IndustryId");
            if (!string.IsNullOrEmpty(industryIdValue))
            {
                decimal industryId = DecimalUtility.CheckNull(decimal.Parse(industryIdValue));
                IList<ProductCriteria> productCriteriaList = ProductCriteriaService.GetAllByCriteriaIdAndIndustryId(criteria.Id, industryId, CurrentCompany);
                if (productCriteriaList == null || productCriteriaList.Count == 0)
                {
                    //ProductCriteria Value no empty , have validation
                    result = false;
                }
            }

            return result;
        }
        #endregion


        public ContentResult GetMostDesiredValue2(string Type)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(Type))
            {
                if (Type.Equals(CriteriaType.Boolean))
                { 
                    
                }
            }
            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetMostDesiredValue(string id)
        {
            if (id.Equals(CriteriaType.Boolean))
            {
                IList<ResourceObject> criteriaMostDBooleanList = ResourceService.GetAll<CriteriaMostDesiredBoolean>();
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(criteriaMostDBooleanList, "Id", "Value");
            }
            else if (id.Equals(CriteriaType.Numeric))
            {
                IList<ResourceObject> criteriaMostDNumericList = ResourceService.GetAll<CriteriaMostDesiredNumeric>();
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(criteriaMostDNumericList, "Id", "Value");
            }
            else
            {
                IList<ResourceObject> emptyList = new List<ResourceObject>();
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(emptyList, "Id", "Value");
            }
        }

        //public void UpdateName(string type, string id, string name)
        //{ 
        
        //}
        public ContentResult UpdateName()
        {
            decimal id = decimal.Parse(Request["id"]);
            string type = (string)Request["type"];
             string name = (string)Request["name"];
             if (!string.IsNullOrEmpty(name))
             {
                 if (name.Length > 100) name = name.Substring(0, 100);
                 if (type.Equals("Group"))
                 {
                     CriteriaGroup criteriaGroup = CriteriaGroupService.GetById(id);
                     criteriaGroup.Name = name;
                     SetDefaultDataForUpdateHistory(criteriaGroup);
                     CriteriaGroupService.Update(criteriaGroup);
                 }
                 else if (type.Equals("Set"))
                 {
                     CriteriaSet criteriaSet = CriteriaSetService.GetById(id);
                     if (criteriaSet != null)
                     {
                         criteriaSet.Name = name;
                         SetDefaultDataForUpdateHistory(criteriaSet);
                         CriteriaSetService.Update(criteriaSet);
                     }
                 }
                 else { }
             }
            return Content(string.Empty);
        }
    }
}
