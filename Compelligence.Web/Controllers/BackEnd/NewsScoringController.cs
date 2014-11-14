using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class NewsScoringController : BackEndAsyncFormController<NewsScoring, decimal>
    {

        public INewsScoringService NewsScoringService
        {
            get { return (INewsScoringService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }
        //
        // GET: /NewsScoring/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        #region Validation Methods

        protected override bool ValidateFormData(NewsScoring newsScoring, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(newsScoring.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.NewsScoringNameRequiredError);
            }
            if (!string.IsNullOrEmpty(newsScoring.Positives))
            {
                if (!StringUtility.IsValidStringER(newsScoring.Positives, "\"(\\w+\\s*)+\"\\s\\(\\+\\d+\\)|\\w+\\s\\(\\+\\d+\\)"))
                {
                    ValidationDictionary.AddError("Positives", LabelResource.NewsScoringPositivesMatchError);
                }
            }
            if (!string.IsNullOrEmpty(newsScoring.Negatives))
            {
                if (!StringUtility.IsValidStringER(newsScoring.Negatives, "\"(\\w+\\s*)+\"\\s\\(\\-\\d+\\)|\\w+\\s\\(\\-\\d+\\)"))
                {
                    ValidationDictionary.AddError("Negatives", LabelResource.NewsScoringNegativesMatchError);
                }
            }
            return ValidationDictionary.IsValid;
        }
        #endregion

        #region Override Methods
        protected override void SetFormData()
        {
            IList<ResourceObject> newsScoringTypeList = ResourceService.GetAll<NewsScoringType>();
            IList<ResourceObject> newsScoringStatusList = ResourceService.GetAll<NewsScoringStatus>();
            ViewData["TypeList"] = new SelectList(newsScoringTypeList, "Id", "Value");
            ViewData["StatusList"] = new SelectList(newsScoringStatusList, "Id", "Value");
        }
        #endregion
    }
}
