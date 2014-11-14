
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
using Compelligence.Web.Models.Web;
using System.Text;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Util;
using Compelligence.Util.Type;
using Compelligence.Common.Utility.Web;
using System.Globalization;


namespace Compelligence.Web.Controllers
{
    public class WinLossController : BackEndAsyncFormController<Quiz, decimal>
    {

        #region Public Properties

        public IQuizService QuizService
        {
            get { return (IQuizService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IQuestionService QuestionService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

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

        public ActionResult ShowWinLoss(decimal QuizId)
        {
            ModelState.Clear();
            SetFormDataOfQuestion(CurrentUser);
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Initiated);
            SetEntityDataOfQuestionToForm(QuizId);
            ViewData["QuizId"] = QuizId;
            SetFormData();
            return View("WinLossBack");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AnswersBackend(FormCollection form)
        {
            Quiz quiz = QuizService.GetById(decimal.Parse(form["QuizId"]));
            string userSelect = form["Created"].ToString();

            IList<Answer> answers = new List<Answer>();
            decimal uniqueKey = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
            Boolean isEmptyAnswer = false;
            string _message = string.Empty;
            ViewData["Container"] = Request["Container"];
            foreach (Question question in quiz.Questions)
            {
                Answer answer = new Answer();
                answer.QuizResponseId = uniqueKey;
                answer.QuizId = question.QuizId;
                answer.QuestionId = question.Id;
                answer.AnswerText = form["Q" + question.Item];
                ViewData["Q" + question.Item] = form["Q" + question.Item];

                if (string.IsNullOrEmpty(answer.AnswerText))
                {
                    isEmptyAnswer = true;
                }
                answer.Author = form["Created"];
                SetDefaultDataForSave(answer);
                answers.Add(answer);
            }

            _message = LabelResource.QuestionAnswerIsEmpty;
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Unsuccessful);
            if (!isEmptyAnswer)
            {
                SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Successful);
                QuizService.SaveAnswers(quiz, answers);
                _message = "Thank you for your submission!";
                JavaScriptResult r = new JavaScriptResult();
                r.Script = "hiddenMessage();showSendResponseMessageDialog();";
                return r;
            }
            else
            {
                SetFormDataOfQuestion(userSelect);
                ViewData["quiz"] = quiz;
                ViewData["QuizId"] = quiz.Id;
                ViewData["QuizQuestions"] = quiz.Questions;
                ViewData["Message"] = _message;
                return View("WinLossBack");
            }
        }


        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Quiz quiz, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(quiz.Title))
            {
                ValidationDictionary.AddError("Title", LabelResource.WinLossTitleRequiredError);
            }

            if (Validator.IsBlankOrNull(quiz.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.WinLossAssignedToRequiredError);
            }

            if (Validator.IsBlankOrNull(quiz.Status))
            {
                ValidationDictionary.AddError("Status", LabelResource.WinLossStatusRequiredError);
            }
            else
            {
                if (quiz.Status == WinLossStatus.Complete && QuestionService.GetByQuizId(quiz.Id).Count <= 0)
                {
                    ValidationDictionary.AddError("Status", LabelResource.WinLossStatusValueError);
                }
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetDefaultEntityDataForSave(Quiz quiz)
        {
            quiz.TargetType = QuizType.WinLoss;
            quiz.MetaData = quiz.Title + ":" + quiz.MetaData;
        }

        protected override void SetFormData()
        {
            SetLabels();  
            IList<Industry> IndustryCollection = IndustryService.GetAllActiveByClientCompany(CurrentCompany);
            IList<ResourceObject> WinLossStatusList = ResourceService.GetAll<WinLossStatus>();
            IList<Quiz> quizList = QuizService.GetAllActiveByClientCompany(CurrentCompany);
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(CurrentCompany);

            ViewData["Industry"] = new SelectList(IndustryCollection, "Id", "Name");
            ViewData["StatusList"] = new SelectList(WinLossStatusList, "Id", "Value", WinLossStatus.Incomplete);
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");

            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetIndustryEnabled(clientCompany);
            IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = IndustryService.GetIndustryEnabled(clientCompany).Count;
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["SelectedCompetitorIdList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["SelectedIndustryIdList"] = new SelectList(new List<Industry>(), "Id", "Name");
        }

        protected override void SetEntityDataToForm(Quiz quiz)
        {
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(quiz.MetaData);
            quiz.OldTitle = quiz.Title;
            SetCascadingData(quiz);
        }

        protected override void SetFormEntityDataToForm(Quiz quiz)
        {
            SetCascadingData(quiz);
            quiz.OldTitle = quiz.Title;
            quiz.MetaData = FormFieldsUtility.GetMultilineValue(quiz.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(quiz.MetaData, quiz.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldTitle", new ValueProviderResult(quiz.OldTitle, quiz.OldTitle, CultureInfo.InvariantCulture));
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.WinLoss;

            switch (detailType)
            {
                case DetailType.Question:
                    AddFilter(detailFilter, "Question.QuizId", parentId.ToString());
                    AddFilter(browseDetailFilter, "QuestionView.QuizId", parentId.ToString());

                    childController = "Question";
                    break;
                case DetailType.Respond:
                    AddFilter(detailFilter, "Answer.QuizId", parentId.ToString());
                    AddFilter(browseDetailFilter, "AnswerDetailView.QuizId", parentId.ToString());

                    childController = "Answer";
                    break;
                case DetailType.QuizClassification:
                    AddFilter(detailFilter, "QuizClassification.QuizId", parentId.ToString());
                    AddFilter(browseDetailFilter, "QuizClassificationDetailView.QuizId", parentId.ToString());
                    childController = "QuizClassification";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Quiz quiz)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (QuizService.HasAccessToQuiz(quiz, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void SetEntityLocking(Quiz quiz)
        {
            ViewData["EntityLocked"] = GenericService.IsEntityLocked(quiz).ToString();
            string userId = (string)Session["UserId"];
            if (userId.Equals(quiz.AssignedTo))
            {
                ViewData["EntityLocked"] = false;
            }
        }

        protected override void GetFormData(Quiz quiz, FormCollection collection)
        {
            quiz.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(Quiz quiz)
        {
            if (!DecimalUtility.IsBlankOrNull(quiz.IndustryId))
            {
                IList<Competitor> competitorList = CompetitorService.GetByIndustryId(quiz.IndustryId.Value);

                ViewData["Competitor"] = new SelectList(competitorList, "Id", "Name");

                if (!DecimalUtility.IsBlankOrNull(quiz.CompetitorId))
                {
                    IList<Product> productList = ProductService.GetByIndustryAndCompetitor(quiz.IndustryId.Value, quiz.CompetitorId.Value);

                    ViewData["Product"] = new SelectList(productList, "Id", "Name");
                }
            }
        }

        private void SetFormDataOfQuestion(string user)
        {
            IList<UserProfile> enduserList = UserProfileService.GetEndUsers(CurrentCompany);
            UserProfile User = UserProfileService.GetById(user);
            enduserList.Insert(0, User);
            ViewData["AssignedToEndUserList"] = new SelectList(enduserList, "Id", "Name");
        }

        private void SetEntityDataOfQuestionToForm(decimal QuizId)
        {
            Quiz ActiveQuiz = QuizService.GetById(QuizId);
            IList<Question> QuestionCollection = QuestionService.GetByQuizId(ActiveQuiz.Id);
            if (QuestionCollection == null)
            {
                QuestionCollection = new List<Question>();
            }

            foreach (Question ActiveQuestion in QuestionCollection)
            {
                ActiveQuestion.QuestionDetails = QuestionService.GetDetails(ActiveQuestion.Id);
            }
            ViewData["CountQuestions"] = QuestionCollection.Count;
            ViewData["quiz"] = ActiveQuiz;
            ViewData["QuizQuestions"] = QuestionCollection;
        }

        #endregion

    }
}