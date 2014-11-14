using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Type;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Web.Models.Util;
using Compelligence.Domain.Entity.Resource;
using System.Resources;

namespace Compelligence.Web.Controllers
{
    public class ProductCriteriaController : BackEndAsyncFormController<ProductCriteria, ProductCriteriaId>
    {

        #region Public Properties

        public IProductCriteriaService ProductCriteriaService
        {
            get { return (IProductCriteriaService)_genericService; }
            set { _genericService = value; }
        }

        public IIndustryService IndustryService { get; set; }
        public IIndustryCriteriasService IndustryCriteriasService { get; set; }
        public ICriteriaService CriteriaService { get; set; }
        public ICriteriaSetService CriteriaSetService { get; set; }
        public ICriteriaGroupService CriteriaGroupService { get; set; }
        public IResourceService ResourceService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateProductCriteria(ProductCriteria productCriteria, FormCollection collection)
        {
            try
            {
                OperationStatus operationStatus = OperationStatus.Initiated;

                SetDetailFilterData(productCriteria);

                if (ValidateFormData(productCriteria, collection))
                {
                    GetFormData(productCriteria, collection);

                    SetDefaultDataForSave(productCriteria);

                    GenericService.Save(productCriteria);

                    operationStatus = OperationStatus.Successful;

                    return RedirectToAction("EditProductCriteria", new { productId = productCriteria.ProductId, criteriaId = productCriteria.CriteriaId, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
                }

                SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

                SetFormData();

                SetFormEntityDataToForm(productCriteria);

                SetUserSecurityAccess();

                return View("Edit", productCriteria);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditProductCriteria(string id, string operationStatus)
        {
            ProductCriteriaId productCriteriaId;
            string sIndustryId = GetDetailFilterValue("ProductCriteria.IndustryId");
            string[] compId = id.Split('_');

            if (!string.IsNullOrEmpty(id))
            {
                if (compId[0].Equals("N"))
                {
                    decimal idProduct = Convert.ToDecimal(Session["ProductId"]);
                    productCriteriaId = new ProductCriteriaId(idProduct, Convert.ToDecimal(compId[1]), decimal.Parse(sIndustryId));
                }
                else
                    productCriteriaId = new ProductCriteriaId(Convert.ToDecimal(compId[0]), Convert.ToDecimal(compId[1]), decimal.Parse(sIndustryId));
            }
            else
            {
                decimal productId = Convert.ToDecimal(Request["productId"]);
                decimal criteriaId = Convert.ToDecimal(Request["criteriaId"]);
                productCriteriaId = new ProductCriteriaId(productId, criteriaId, decimal.Parse(sIndustryId));
            }

            ProductCriteria productCriteria = ProductCriteriaService.GetById(productCriteriaId);
            if (productCriteria == null)
            {
                productCriteria = new ProductCriteria();
                productCriteria.Id = productCriteriaId;
                //productCriteria.Id = productCriteriaId;
                IndustryCriteriasId idIndCrit = new IndustryCriteriasId(decimal.Parse(sIndustryId), productCriteriaId.CriteriaId);
                IndustryCriterias indCrit = IndustryCriteriasService.GetById(idIndCrit);
                if (indCrit != null)
                {
                    if (indCrit.CriteriaSetId != null)
                    productCriteria.CriteriaSetId = indCrit.CriteriaSetId;
                    if (indCrit.CriteriaGroupId != null)
                    productCriteria.CriteriaGroupId = indCrit.CriteriaGroupId;
                    productCriteria.IndustryId = productCriteriaId.IndustryId;
                    productCriteria.CriteriaId = productCriteriaId.CriteriaId;
                    productCriteria.ProductId = productCriteriaId.ProductId;
                }
            
            }
            OperationStatus operationStatusParam = OperationStatus.Initiated;

            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            if (compId[0].Equals("N"))
                SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatusParam);
            else
                SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();
            //if (!compId[0].Equals("N"))
            SetEntityDataToForm(productCriteria);

            SetUserSecurityAccess(productCriteria);

            SetEntityLocking(productCriteria);

            return View("Edit", productCriteria);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProductCriteria(ProductCriteria formProductCriteria, FormCollection collection)
        {
            string sIndustryId = GetDetailFilterValue("ProductCriteria.IndustryId");
            try
            {
                ProductCriteria productCriteriaResult = formProductCriteria;

                OperationStatus operationStatus = OperationStatus.Initiated;

                if (ValidateEditFormData(formProductCriteria, collection))
                {
                    ProductCriteriaId id=new ProductCriteriaId(
                        DecimalUtility.CheckNull(formProductCriteria.ProductId),
                        DecimalUtility.CheckNull(formProductCriteria.CriteriaIdOld),
                        decimal.Parse(sIndustryId));
                    ProductCriteria productCriteria = ProductCriteriaService.GetById(id);

                    // If selected "CriteriaId" value is diferent that "CriteriaIdOld", so use "SaveExisting" method from Service
                    if (DecimalUtility.CheckNull(formProductCriteria.CriteriaId) != DecimalUtility.CheckNull(formProductCriteria.CriteriaIdOld))
                    {
                        GetFormData(formProductCriteria, collection);

                        SetDefaultDataForSave(formProductCriteria);

                        ProductCriteriaService.SaveExisting(formProductCriteria, productCriteria);

                        productCriteriaResult = formProductCriteria;
                    }
                    else
                    {
                        SetFormDataToEntity(productCriteria, collection);

                        GetFormData(productCriteria, collection);

                        SetDefaultDataForUpdate(productCriteria);

                        ProductCriteriaService.Update(productCriteria);

                        productCriteriaResult = productCriteria;
                    }

                    operationStatus = OperationStatus.Successful;
                }

                SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

                SetFormData();

                SetFormEntityDataToForm(productCriteriaResult);

                SetUserSecurityAccess(productCriteriaResult);

                SetEntityLocking(productCriteriaResult);

                return View("Edit", productCriteriaResult);
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProductCriteria(string id)
        {
            string sIndustryId = GetDetailFilterValue("ProductCriteria.IndustryId");
            try
            {
                string[] idComponents = id.Split('_');
                string userId = (string)Session["UserId"];
                if (idComponents.Length == 2 && DecimalUtility.IsNumericLength(idComponents[0]) && DecimalUtility.IsNumericLength(idComponents[1]))
                {
                    ProductCriteria productCriteria = ProductCriteriaService.GetById(
                         new ProductCriteriaId(Convert.ToDecimal(idComponents[0]),
                             Convert.ToDecimal(idComponents[1]), decimal.Parse(sIndustryId)));

                    SetDefaultDataFromRequest(productCriteria);

                    ProductCriteriaService.Delete(productCriteria, userId);

                    
                }
                return null;
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriaSetsByCriteriaGroup(decimal id)
        {
            IList<CriteriaSet> criteriaSetList = CriteriaSetService.GetByCriteriaGroupId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<CriteriaSet>(criteriaSetList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriasByCriteriaSet(decimal id)
        {
            IList<Criteria> criteriaList = CriteriaService.GetByCriteriaSetId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Criteria>(criteriaList, "Id", "Name");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCriteriasByGroupSet(string IndustryId,string CriteriaGroupId,string CriteriaSetId)
        {

            IList<Criteria> criteriaList = null;
            if (!string.IsNullOrEmpty(CriteriaGroupId) && !string.IsNullOrEmpty(CriteriaSetId))
                criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(IndustryId), decimal.Parse(CriteriaGroupId),decimal.Parse(CriteriaSetId));
            else
              if (!string.IsNullOrEmpty(CriteriaGroupId) && string.IsNullOrEmpty(CriteriaSetId))
                criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(IndustryId), decimal.Parse(CriteriaGroupId), null);
              else
                criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(IndustryId), null, null);

            return ControllerUtility.GetSelectOptionsFromGenericList<Criteria>(criteriaList, "Id", "Name");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetIndustryStandard(decimal id)
        {
            Criteria criteria = CriteriaService.GetById(id);

            //return new ContentResult { Content = criteria.IndustryStandard, ContentType = "application/json" };
            return Content(criteria.IndustryStandard);
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(ProductCriteria productCriteria, FormCollection formCollection)
        {
            ValidationDictionary.RemoveError("Id");

            //if (Validator.IsBlankOrNull(productCriteria.CriteriaGroupId))
            //{
            //    ValidationDictionary.AddError("CriteriaGroupId", LabelResource.ProductCriteriaCriteriaGroupIdRequiredError);
            //}

            //if (Validator.IsBlankOrNull(productCriteria.CriteriaSetId))
            //{
            //    ValidationDictionary.AddError("CriteriaSetId", LabelResource.ProductCriteriaCriteriaSetIdRequiredError);
            //}

            if (Validator.IsBlankOrNull(productCriteria.CriteriaId))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.ProductCriteriaCriteriaIdRequiredError);
            }
            else if (!ProductCriteriaService.IsValidProductCriteria(DecimalUtility.CheckNull(productCriteria.ProductId),
                DecimalUtility.CheckNull(productCriteria.CriteriaId),DecimalUtility.CheckNull(productCriteria.IndustryId)))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.ProductCriteriaCriteriaIdExistsError);
            }

            if (productCriteria.CriteriaType.Equals(CriteriaType.Boolean))
            {
                if (Validator.IsBlankOrNull(productCriteria.ValueSelectFrm))
                {
                    ValidationDictionary.AddError("ValueSelectFrm", LabelResource.ProductCriteriaValueRequiredError);
                }
            }
            else if (productCriteria.CriteriaType.Equals(CriteriaType.Numeric))
            {
                if (Validator.IsBlankOrNull(productCriteria.ValueTextFrm))
                {
                    ValidationDictionary.AddError("ValueTextFrm", LabelResource.ProductCriteriaValueRequiredError);
                }
            }
            else
            {
                if (Validator.IsBlankOrNull(productCriteria.Value))
                {
                    ValidationDictionary.AddError("Value", LabelResource.ProductCriteriaValueRequiredError);
                }
            }
            //if (Validator.IsBlankOrNull(productCriteria.Feature))
            //{
            //    ValidationDictionary.AddError("Feature", LabelResource.ProductCriteriaFeatureError);
            //}

            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(ProductCriteria productCriteria, FormCollection formCollection)
        {
            ValidationDictionary.RemoveError("Id");

            //if (Validator.IsBlankOrNull(productCriteria.CriteriaGroupId))
            //{
            //    ValidationDictionary.AddError("CriteriaGroupId", LabelResource.ProductCriteriaCriteriaGroupIdRequiredError);
            //}

            //if (Validator.IsBlankOrNull(productCriteria.CriteriaSetId))
            //{
            //    ValidationDictionary.AddError("CriteriaSetId", LabelResource.ProductCriteriaCriteriaSetIdRequiredError);
            //}

            if (Validator.IsBlankOrNull(productCriteria.CriteriaId))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.ProductCriteriaCriteriaIdRequiredError);
            }
            else if ((DecimalUtility.CheckNull(productCriteria.CriteriaId) != 
                DecimalUtility.CheckNull(productCriteria.CriteriaIdOld))
                && (!ProductCriteriaService.IsValidProductCriteria(DecimalUtility.CheckNull(productCriteria.ProductId), DecimalUtility.CheckNull(productCriteria.CriteriaId), DecimalUtility.CheckNull(productCriteria.IndustryId))))
            {
                ValidationDictionary.AddError("CriteriaId", LabelResource.ProductCriteriaCriteriaIdExistsError);
            }
            if (productCriteria.CriteriaType.Equals(CriteriaType.Boolean))
            {
                if (Validator.IsBlankOrNull(productCriteria.ValueSelectFrm))
                {
                    ValidationDictionary.AddError("ValueSelectFrm", LabelResource.ProductCriteriaValueRequiredError);
                }
            }
            else if (productCriteria.CriteriaType.Equals(CriteriaType.Numeric))
            {
                if (Validator.IsBlankOrNull(productCriteria.ValueTextFrm))
                {
                    ValidationDictionary.AddError("ValueTextFrm", LabelResource.ProductCriteriaValueRequiredError);
                }
            }
            else
            {
                if (Validator.IsBlankOrNull(productCriteria.Value))
                {
                    ValidationDictionary.AddError("Value", LabelResource.ProductCriteriaValueRequiredError);
                }
            }
            //if (Validator.IsBlankOrNull(productCriteria.Feature))
            //{
            //    ValidationDictionary.AddError("Feature", LabelResource.ProductCriteriaFeatureError);
            //}

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetDetailFormData()
        {
            string industryIdValue = GetDetailFilterValue("ProductCriteria.IndustryId");
            Industry industry = null;

            if (!string.IsNullOrEmpty(industryIdValue))
            {
                industry = IndustryService.GetById(Convert.ToDecimal(industryIdValue));
            }

            ViewData["ProductCriteriaIndustry"] = industry;
        }

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string sIndustryId = GetDetailFilterValue("ProductCriteria.IndustryId");
            IList<CriteriaGroup> criteriaGroupList = new List<CriteriaGroup>();
            IList<Criteria> criterias = null;
            
            if (!string.IsNullOrEmpty(sIndustryId))
            {
                //criteriaGroupList = CriteriaGroupService.GetByIndustryId(Convert.ToDecimal(sIndustryId));
                criteriaGroupList = CriteriaGroupService.GetAllByClientCompany(clientCompany);
                criterias=IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(sIndustryId),null,null);
                
                
                    
               // IndustryCriteriasService.GetCriteriasByGroupSet(decimal.Parse(sIndustryId), null, null);

            }

            

            ViewData["CriteriaGroupList"] = new SelectList(criteriaGroupList, "Id", "Name");
            ViewData["IndustryId"] = sIndustryId;
            List<SelectListItem> features = new List<SelectListItem>();
            features.Add(new SelectListItem { Text = "Best in Class",Value = "BC"});
            features.Add(new SelectListItem { Text = "Market Advantage", Value = "MA" });
            features.Add(new SelectListItem { Text = "Market Parity", Value = "MP" });
            features.Add(new SelectListItem { Text = "Market Disadvantage", Value = "MD" });
            features.Add(new SelectListItem { Text = "Lagging Market", Value = "LM" });



            ViewData["Features"] = new SelectList(features,"Value","Text");
            //ViewData["Value"] = new SelectList(values);
            ViewData["Path"] = obtainPath();

            if ( criterias==null)
              criterias= new List<Criteria>();
            
            ViewData["CriteriaList"] = new SelectList(criterias,"Id","Name");
            ViewData["CriteriaType"] = string.Empty;
            ViewData["CriteriaMostDesired"] = string.Empty;
            IList<ResourceObject> criteriaMostDBooleanList = ResourceService.GetAll<ProductCriteriaValueBoolean>();
            ViewData["ProductCriteriaBooleanValue"] = new SelectList(criteriaMostDBooleanList, "Id", "Value");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetValues_Industry(string IndustryId, string CriteriaId)
        {
            Criteria ListCriteria = CriteriaService.GetById(decimal.Parse(CriteriaId));
            if (ListCriteria == null)
                return null;
            string NameCriteria = ListCriteria.Name;
            IList<string> valuesList = ProductCriteriaService.GetByIndustryValues(decimal.Parse(IndustryId), NameCriteria);
            return ControllerUtility.GetSelectOptionsFromGenericListOnly(valuesList);         
            
        }

        protected override void SetEntityDataToForm(ProductCriteria productCriteria)
        {
            if (productCriteria.CriteriaId != null){
                Criteria criteria = CriteriaService.GetById((decimal)productCriteria.CriteriaId);
                ViewData["ValStandard"] = criteria.IndustryStandard;
                ViewData["CriteriaType"] = criteria.Type;
                ViewData["CriteriaMostDesired"] = criteria.MostDesiredValue;
                productCriteria.CriteriaType = criteria.Type;
                productCriteria.CriteriaMostDesired = criteria.MostDesiredValue;
                if (criteria.Type.Equals(CriteriaType.Boolean))
                {
                    productCriteria.ValueSelectFrm = productCriteria.Value;
                }
                else if (criteria.Type.Equals(CriteriaType.Numeric))
                {
                    if (productCriteria.ValueDecimal !=  null && productCriteria.ValueDecimal != 0)
                    {
                        productCriteria.ValueTextFrm = string.Format("{0:0.########}", productCriteria.ValueDecimal);// productCriteria.ValueDecimal.ToString();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(productCriteria.Value) && productCriteria.Value.Equals("N/A"))
                        {
                            productCriteria.ValueTextFrm = productCriteria.Value;
                        }
                        if (!string.IsNullOrEmpty(productCriteria.Value) && productCriteria.Value.Equals("0"))
                        {
                            productCriteria.ValueTextFrm = productCriteria.Value;
                        }
                    }
                }
            }
            ViewData["Path"] = obtainPath();
            ViewData["CriteriaIdOld"] = productCriteria.CriteriaId;

            
            
            SetCascadingData(productCriteria);
        }

        protected override void SetFormEntityDataToForm(ProductCriteria productCriteria)
        {
            SetCascadingData(productCriteria);
        }

        protected override void SetDefaultEntityDataForSave(ProductCriteria productCriteria)
        {
            if (productCriteria.CriteriaType.Equals(CriteriaType.Boolean))
            {
                productCriteria.Value = productCriteria.ValueSelectFrm;
            }
            else if (productCriteria.CriteriaType.Equals(CriteriaType.Numeric))
            {
                productCriteria.Value = productCriteria.ValueTextFrm;
            }
        }

        protected override void GetFormData(ProductCriteria productCriteria, FormCollection collection)
        {
            SetValuesByCriteriaType(productCriteria);
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(ProductCriteria productCriteria)
        {
            if (!DecimalUtility.IsBlankOrNull(productCriteria.CriteriaGroupId))
            {
                IList<CriteriaSet> criteriaSetList = CriteriaSetService.GetByCriteriaGroupId(productCriteria.CriteriaGroupId.Value);

                ViewData["CriteriaSetList"] = new SelectList(criteriaSetList, "Id", "Name");

                if (!DecimalUtility.IsBlankOrNull(productCriteria.CriteriaSetId))
                {
                    IList<Criteria> criteriaList = CriteriaService.GetByCriteriaSetId(productCriteria.CriteriaSetId.Value);
                    ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
                }
                else //Have Group,but not Set
                {
                    if (productCriteria.IndustryId != null)
                    {
                        IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet((decimal)productCriteria.IndustryId, (decimal)productCriteria.CriteriaGroupId, null);
                        ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
                    }
                    else
                    {
                        string sIndustryId = GetDetailFilterValue("ProductCriteria.IndustryId");
                        productCriteria.IndustryId = decimal.Parse(sIndustryId);
                        IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet((decimal)productCriteria.IndustryId, (decimal)productCriteria.CriteriaGroupId, null);
                        ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
                    }
                }
            }
            else //Not have Group
            {
                IList<Criteria> criteriaList = IndustryCriteriasService.GetCriteriasByGroupSet((decimal)productCriteria.IndustryId, null, null);
                ViewData["CriteriaList"] = new SelectList(criteriaList, "Id", "Name");
            }
        }

        private void SetValuesByCriteriaType(ProductCriteria productCriteria)
        {
            if (productCriteria.CriteriaType.Equals(CriteriaType.Boolean))
            {
                productCriteria.Value = productCriteria.ValueSelectFrm;
            }
            else if (productCriteria.CriteriaType.Equals(CriteriaType.Numeric))
            {
                if(productCriteria.ValueTextFrm.ToUpper().Equals("N/A"))
                {
                    productCriteria.Value = productCriteria.ValueTextFrm.ToUpper();//set the value in String
                    productCriteria.ValueDecimal = null; //if exist some value numeric should be removed
                }
                else if (DecimalUtility.IsDecimal(productCriteria.ValueTextFrm))
                {
                   // productCriteria.Value = productCriteria.ValueTextFrm;
                    productCriteria.ValueDecimal = DecimalUtility.ConvertStringToDecimal(productCriteria.ValueTextFrm);
                    //string tempo = DecimalUtility.ConvertDecimalToStringWithFormat(productCriteria.ValueDecimal);
                    productCriteria.Value = DecimalUtility.ConvertDoubleToString(Convert.ToDouble(productCriteria.ValueDecimal));
                }
            }
            else if (productCriteria.CriteriaType.Equals(CriteriaType.List))
            {
                if (productCriteria.Value.Length > 225)
                {
                    productCriteria.Value = productCriteria.Value.Substring(0,225);
                }
            }
        }

        #endregion

        #region Public Methods

        public static string obtainPath()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            string path = "";
            bool start = false;
            int initPos = 0;
            int endPos = 0;
            int begin = 7;
            if (currentUrl.IndexOf("https://") != -1) { begin++; }
            for (int i = begin; i < currentUrl.Length; i++)
            {
                string actualChar = currentUrl[i].ToString();

                if (start == false && actualChar.Equals("/"))
                {
                    start = true;
                    initPos = i;
                    break;
                }

            }
            endPos = currentUrl.IndexOf("/ProductCriteria.aspx");
            path = currentUrl.Substring(initPos, endPos - initPos);
            return path;
        }

        #endregion
    }
}
