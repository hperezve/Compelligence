using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Common.Utility;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Security.Filters;
using Compelligence.Common.Validation;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Collections;
using Compelligence.Web.Models.Web;

namespace Compelligence.Web.Controllers
{
    public class QuestionController : BackEndAsyncFormController<Question, decimal>
    {

        private IUserProfileService _userProfileService;
        private IResourceService _resourceService;
        private IQuestionDetailService _questionDetailService;

        public ICompetitorService CompetitorService { get; set; }

        public IIndustryService IndustryService { get; set; }



        public IUserProfileService UserProfileService
        {
            get { return _userProfileService; }
            set { _userProfileService = value; }
        }

        public IResourceService ResourceService
        {
            get { return _resourceService; }
            set { _resourceService = value; }
        }

        public IQuestionService QuestionService
        {
            get { return (IQuestionService)_genericService; }
            set { _genericService = value; }
        }

        public IQuestionDetailService QuestionDetailService
        {
            get { return _questionDetailService; }
            set { _questionDetailService = value; }
        }

        protected override bool ValidateFormData(Question question, FormCollection formCollection)
        {
            GetFormData(question, formCollection);
            int minOptionsRequired = 0;
            Boolean isEmptyResponseTex = false;
            Boolean isEmptyResponseValue = false;
            Boolean duplicateResponseValue = false;
            if (Validator.IsBlankOrNull(question.Type))
            {
                ValidationDictionary.AddError("Type", LabelResource.QuestionTypeRequiredError);
            }
            if (Validator.IsBlankOrNull(question.QuestionText))
            {
                ValidationDictionary.AddError("QuestionText", LabelResource.QuestionTextRequiredError);
            }
            if (question.Type.Equals(QuestionType.MultipleChoice))
            {
                for (int j = 0; j < question.QuestionDetails.Count; j++)
                {
                    if (!string.IsNullOrEmpty(question.QuestionDetails[j].ResponseValue))
                    {
                        minOptionsRequired = minOptionsRequired + 1;
                        if (string.IsNullOrEmpty(question.QuestionDetails[j].ResponseText))
                        {
                            isEmptyResponseTex = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(question.QuestionDetails[j].ResponseText))
                    {
                        if (string.IsNullOrEmpty(question.QuestionDetails[j].ResponseValue))
                        {
                            isEmptyResponseValue = true;
                        }
                    }
                    for (int k = 0; k <= j; k++)
                    {
                        if (!question.QuestionDetails[j].ResponseValue.Equals("") && !question.QuestionDetails[k].ResponseValue.Equals(""))
                        {
                            if (k != j && question.QuestionDetails[j].ResponseValue == question.QuestionDetails[k].ResponseValue)
                            {
                                duplicateResponseValue = true;
                            }
                        }
                    }
                }
                if (minOptionsRequired < 2)
                {
                    ValidationDictionary.AddError("QuestionResponseValue1", LabelResource.QuestionResponseValueMinOptionsError);
                }
                if (isEmptyResponseTex)
                {
                    ValidationDictionary.AddError("QuestionResponseText1", LabelResource.QuestionResponseValueMatchError);
                }
                if (isEmptyResponseValue)
                {
                    ValidationDictionary.AddError("QuestionResponseText1", LabelResource.QuestionResponseTextMatchError);
                }
                if (duplicateResponseValue)
                {
                    ValidationDictionary.AddError("QuestionResponseText1", LabelResource.QuestionDuplicateResponseValue);
                }
            }
            return ValidationDictionary.IsValid;
        }

        protected override void SetFormData()
        {
            String QuizId = ViewData["DetailFilter"].ToString().Replace("Question.QuizId_Eq_", "");
            Question ExistQCO = QuestionService.GetByQuizIdAndType(Convert.ToDecimal(QuizId), QuestionType.Competitors);
            Question ExistQWC = QuestionService.GetByQuizIdAndType(Convert.ToDecimal(QuizId), QuestionType.WinningCompetitor);
            IList<ResourceObject> QuestionTypeList = ResourceService.GetAll<QuestionType>();
            SelectList typeList = new SelectList(QuestionTypeList, "Id", "Value");
            IList<SelectListItem> newTypeList = new List<SelectListItem>();
            foreach (SelectListItem item in typeList)
            {
                if (item.Value == QuestionType.Competitors)
                {
                    if (ExistQCO == null)
                    {
                        newTypeList.Add(item);
                    }
                }
                else if (item.Value == QuestionType.WinningCompetitor)
                {
                    if (ExistQWC == null)
                    {
                        newTypeList.Add(item);
                    }
                }
                else
                {
                    newTypeList.Add(item);
                }
            }

            ViewData["TypeList"] = new SelectList(newTypeList, "Value", "Text");

            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = IndustryService.GetAllActiveByClientCompany(clientCompany).Count;
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["SelectedCompetitorIdList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["SelectedIndustryIdList"] = new SelectList(new List<Industry>(), "Id", "Name");
            ViewData["EndUserIdMultiListDisabled"] = "false";
            ViewData["EmployeeIdList"] = new MultiSelectList(new List<Employee>(), "Id", "Name");
            ViewData["Counter"] = "0";
            IList<QuestionDetail> questionDetailList = new List<QuestionDetail>();
            for (int i = 0; i < 5; i++)
            {
                QuestionDetail qd = new QuestionDetail();
                questionDetailList.Add(qd);
            }
            ViewData["QuestionDetail"] = questionDetailList;
        }
        protected override void SetEntityDataToForm(Question question)
        {
            int valueindex = 1;
            int textindex = 1;
            foreach (QuestionDetail questionDetail in question.QuestionDetails)
            {
                ViewData["ResponseValue" + (valueindex++)] = questionDetail.ResponseValue;
                ViewData["ResponseText" + (textindex++)] = questionDetail.ResponseText;

            }
            ViewData["QuestionDetail"] = question.QuestionDetails;
            ViewData["Counter"] = question.QuestionDetails.Count;
            if (question.Type.Equals(QuestionType.Competitors) || question.Type.Equals(QuestionType.WinningCompetitor))
            {
                IList<ResourceObject> QuestionTypeList = ResourceService.GetAll<QuestionType>();
                SelectList typeList = new SelectList(QuestionTypeList, "Id", "Value");
                IList<SelectListItem> newTypeList = new List<SelectListItem>();
                if (question.Type.Equals(QuestionType.Competitors))
                {
                    foreach (SelectListItem item in typeList)
                    {
                        if (item.Value != QuestionType.WinningCompetitor)
                        {
                            newTypeList.Add(item);
                        }
                    }
                }
                else if (question.Type.Equals(QuestionType.WinningCompetitor))
                {
                    foreach (SelectListItem item in typeList)
                    {
                        if (item.Value != QuestionType.Competitors)
                        {
                            newTypeList.Add(item);
                        }
                    }
                }


                ViewData["TypeList"] = new SelectList(newTypeList, "Value", "Text");

            }
        }
        protected override void GetFormData(Question question, FormCollection collection)
        {
            IList<QuestionDetail> questionDetails = new List<QuestionDetail>();

            int limit = 10; // check if this can be 5
            if (collection["Counter"] != null)
            {
                string counterStr = collection["Counter"];
                if (!string.IsNullOrEmpty(counterStr))
                {

                    if (Compelligence.Util.Type.DecimalUtility.IsNumeric(counterStr))
                    {
                        int count = Int32.Parse(counterStr);
                        if (count > limit) limit = count;
                    }
                }
            }
            for (int i = 0; i < limit; i++)
            {
                string responseTextKey = "ResponseText";
                string responseValueKey = "ResponseValue";


                if (collection[responseTextKey + (i + 1).ToString()] != null)
                {
                    QuestionDetail questionDetail = new QuestionDetail(
                        collection[responseTextKey + (i + 1).ToString()],
                        collection[responseValueKey + (i + 1).ToString()]);

                    questionDetails.Add(questionDetail);
                }
            }
            question.QuestionDetails = questionDetails;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Create(Question entity, FormCollection collection)
        {
            try
            {
                OperationStatus operationStatus = OperationStatus.Initiated;

                if (ValidateFormData(entity, collection))
                {
                    DomainObject<decimal> domainObject = (DomainObject<decimal>)(entity);
                    SetDetailFilterData(entity);

                    GetFormData(entity, collection);

                    SetDefaultEntityDataForSave(entity);

                    SetDefaultDataForSave(entity);

                    GenericService.Save(entity);

                    operationStatus = OperationStatus.Successful;

                    return GetActionResultForCreate(domainObject, operationStatus);
                }

                SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

                SetFormData();

                SetFormEntityDataToForm(entity);

                SetUserSecurityAccess();

                return View("Edit", entity);
            }
            catch
            {
                return View();
            }
        }

        protected void GetFormDataCreate(Question question, FormCollection collection)
        {
            IList<QuestionDetail> questionDetails = new List<QuestionDetail>();

            for (int i = 0; i < 10; i++)
            {
                string responseTextKey = "ResponseText";
                string responseValueKey = "ResponseValue";


                if (collection[responseTextKey + (i + 1).ToString()] != null)
                {
                    QuestionDetail questionDetail = new QuestionDetail(
                        collection[responseTextKey + (i + 1).ToString()],
                        collection[responseValueKey + (i + 1).ToString()]);

                    questionDetails.Add(questionDetail);
                }
            }
            question.QuestionDetails = questionDetails;
        }

        protected override void SetFormEntityDataToForm(Question question)
        {
            if (question.QuestionDetails.Count > 0)
            {
                if (question.QuestionDetails.Count > 1)
                {
                    ViewData["ResponseValue1"] = question.QuestionDetails[0].ResponseValue;
                    ViewData["ResponseText1"] = question.QuestionDetails[0].ResponseText;
                    ViewData["ResponseValue2"] = question.QuestionDetails[1].ResponseValue;
                    ViewData["ResponseText2"] = question.QuestionDetails[1].ResponseText;
                    ViewData["ResponseValue3"] = question.QuestionDetails[2].ResponseValue;
                    ViewData["ResponseText3"] = question.QuestionDetails[2].ResponseText;
                }
                if (question.QuestionDetails.Count > 4)
                {
                    ViewData["ResponseValue4"] = question.QuestionDetails[3].ResponseValue;
                    ViewData["ResponseText4"] = question.QuestionDetails[3].ResponseText;
                    ViewData["ResponseValue5"] = question.QuestionDetails[4].ResponseValue;
                    ViewData["ResponseText5"] = question.QuestionDetails[4].ResponseText;
                }
                if (question.QuestionDetails.Count > 5)
                {
                    for (int i = 5; i < question.QuestionDetails.Count; i++)
                    {
                        ViewData["ResponseValue" + (i + 1).ToString()] = question.QuestionDetails[i].ResponseValue;
                        ViewData["ResponseText" + (i + 1).ToString()] = question.QuestionDetails[i].ResponseText;
                    }
                }
                ViewData["QuestionDetail"] = question.QuestionDetails;
                ViewData["Counter"] = question.QuestionDetails.Count;
            }
        }
    }
}
