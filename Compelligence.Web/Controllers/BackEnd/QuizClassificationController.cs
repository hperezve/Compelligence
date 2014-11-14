using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Util;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class QuizClassificationController : BackEndAsyncFormController<QuizClassification, decimal>
    {

        #region Public Properties

        public IQuizClassificationService QuizClassificationService
        {
            get { return (IQuizClassificationService)_genericService; }
            set { _genericService = value; }
        }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IResourceService ResourceService { get; set; }

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

        protected override bool ValidateFormData(QuizClassification quizClassification, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (Validator.IsBlankOrNull(quizClassification.Visible))
            {
                ValidationDictionary.AddError("Visible", LabelResource.QuizClassificationVisibleRequiredError);
            }

            if (quizClassification.Root == false)
            {
                if (quizClassification.IndustryId != null)
                {
                    if ((QuizClassificationService.GetByIndustry((decimal)quizClassification.IndustryId, (decimal)quizClassification.QuizId, clientCompany).Count > 0) && (Validator.IsBlankOrNull(quizClassification.CompetitorId)))
                    {
                        ValidationDictionary.AddError("IndustryId", LabelResource.QuizClassificationIndustryIdMatchError);
                    }

                    if (!Validator.IsBlankOrNull(quizClassification.CompetitorId) && Validator.IsBlankOrNull(quizClassification.ProductId))
                    {
                        if (QuizClassificationService.GetByIndustryCompetitor((decimal)quizClassification.IndustryId, (decimal)quizClassification.CompetitorId, (decimal)quizClassification.QuizId, clientCompany).Count > 0)
                        {
                            ValidationDictionary.AddError("CompetitorId", LabelResource.QuizClassificationCompetitorIdMatchError);
                        }
                    }

                    if (!Validator.IsBlankOrNull(quizClassification.ProductId))
                    {
                        if (QuizClassificationService.GetByIndustryCompetitorProduct((decimal)quizClassification.IndustryId, (decimal)quizClassification.CompetitorId, (decimal)quizClassification.ProductId, (decimal)quizClassification.QuizId, clientCompany).Count > 0)
                        {
                            ValidationDictionary.AddError("ProductId", LabelResource.QuizClassificationProductIdMatchError);
                        }
                    }
                }
                else
                {
                    ValidationDictionary.AddError("IndustryId", LabelResource.QuizClassificationRootOrIndustryRequiredError);
                }

            }
            else if ((quizClassification.Root == true) && (QuizClassificationService.GetByRoot((decimal)quizClassification.QuizId, quizClassification.Visible, clientCompany).Count > 0))
            {
                ValidationDictionary.AddError("Root", LabelResource.QuizClassificationRootMatchError);
            }
            //if ((quizClassification.IndustryId != null) && (quizClassification.CompetitorId != null) && (quizClassification.ProductId != null))
            //{
            //    if (QuizClassificationService.GetByIndustryCompetitorProduct((decimal)quizClassification.IndustryId, (decimal)quizClassification.CompetitorId, (decimal)quizClassification.ProductId, (decimal)quizClassification.QuizId, clientCompany).Count > 0)
            //    {
            //        ValidationDictionary.AddError("ProductId", LabelResource.QuizClassificationMatchError);
            //    }
            //}
            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(QuizClassification quizClassification, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (Validator.IsBlankOrNull(quizClassification.Visible))
            {
                ValidationDictionary.AddError("Visible", LabelResource.QuizClassificationVisibleRequiredError);
            }

            if (quizClassification.Root == false)
            {

                if ((QuizClassificationService.GetByIndustryAndQuizClassification((decimal)quizClassification.IndustryId, (decimal)quizClassification.QuizId, quizClassification.Id, clientCompany).Count > 0) && (Validator.IsBlankOrNull(quizClassification.CompetitorId)))
                {
                    ValidationDictionary.AddError("IndustryId", LabelResource.QuizClassificationIndustryIdMatchError);
                }

                if (!Validator.IsBlankOrNull(quizClassification.CompetitorId) && Validator.IsBlankOrNull(quizClassification.ProductId))
                {
                    if (QuizClassificationService.GetByIndustryCompetitorAndQuizClassification((decimal)quizClassification.IndustryId, (decimal)quizClassification.CompetitorId, (decimal)quizClassification.QuizId, quizClassification.Id, clientCompany).Count > 0)
                    {
                        ValidationDictionary.AddError("CompetitorId", LabelResource.QuizClassificationCompetitorIdMatchError);
                    }
                }

                if (!Validator.IsBlankOrNull(quizClassification.ProductId))
                {
                    if (QuizClassificationService.GetByIndustryCompetitorProductAndQuizClassification((decimal)quizClassification.IndustryId, (decimal)quizClassification.CompetitorId, (decimal)quizClassification.ProductId, (decimal)quizClassification.QuizId, quizClassification.Id, clientCompany).Count > 0)
                    {
                        ValidationDictionary.AddError("ProductId", LabelResource.QuizClassificationProductIdMatchError);
                    }
                }

            }
            else if ((quizClassification.Root == true) && (QuizClassificationService.GetByRootAndQuizClassification((decimal)quizClassification.QuizId, quizClassification.Visible, quizClassification.Id, clientCompany).Count > 0))
            {
                ValidationDictionary.AddError("Root", LabelResource.QuizClassificationRootMatchError);
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> quizClassificationVisibleList = ResourceService.GetAll<QuizClassificationVisible>();
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            ViewData["QuizClassificationList"] = new SelectList(quizClassificationVisibleList, "Id", "Value", QuizClassificationVisible.No);
            ViewData["IndustryIdList"] = new SelectList(industryList, "Id", "Name");
            ViewData["QuizId"] = GetDetailFilterValue("QuizClassification.QuizId");
        }

        protected override void SetEntityDataToForm(QuizClassification quizClassification)
        {
            if (DecimalUtility.IsBlankOrNull(quizClassification.IndustryId))
            {
                quizClassification.Root = true;
            }

            SetCascadingData(quizClassification);
        }

        protected override void SetFormEntityDataToForm(QuizClassification quizClassification)
        {
            if (quizClassification.IndustryId == null)
            {
                quizClassification.Root = true;
            }
            SetCascadingData(quizClassification);
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(QuizClassification quizClassification)
        {
            if (!DecimalUtility.IsBlankOrNull(quizClassification.IndustryId))
            {
                IList<Competitor> competitorList = CompetitorService.GetByIndustryId(quizClassification.IndustryId.Value);

                ViewData["CompetitorIdList"] = new SelectList(competitorList, "Id", "Name");

                if (!DecimalUtility.IsBlankOrNull(quizClassification.CompetitorId))
                {
                    IList<Product> productList = ProductService.GetByIndustryAndCompetitor(quizClassification.IndustryId.Value, quizClassification.CompetitorId.Value);

                    ViewData["ProductIdList"] = new SelectList(productList, "Id", "Name");
                }
            }
        }

        #endregion
    }
}
