using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Common.Utility;
using Compelligence.Domain.Entity.Resource;
using Resources;
using Compelligence.Security.Filters;

namespace Compelligence.Web.Controllers
{
    public class LongSurveyController : GenericFrontEndController
    {
        #region Public Properties

        public IQuizService QuizService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IActionHistoryService ActionHistoryService { get; set; }

        private IResourceService _resourceService;

        public IResourceService ResourceService
        {
            get { return _resourceService; }
            set { _resourceService = value; }
        }

        public ICompetitorService CompetitorService { get; set; }

        public IIndustryService IndustryService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public ActionResult GetQuestions(decimal id)
        {
            Quiz survey = QuizService.GetById(id);

            ViewData["LongSurvey"] = survey;
            SetFormData();

            return View("Questions");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Answer(decimal id, FormCollection form)
        {
            Quiz quiz = QuizService.GetById(id);

            IList<Answer> answers = new List<Answer>();

            decimal uniqueKey = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();

            bool isEmptyAnswer = false;

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

                answer.Author = CurrentUser;

                answer.CreatedBy = CurrentUser;
                answer.CreatedDate = DateTime.Now;
                answer.LastChangedBy = CurrentUser;
                answer.LastChangedDate = DateTime.Now;

                answers.Add(answer);
            }

            quiz.Libraries = GetLibrariesForEntity(quiz.Id, DomainObjectType.Survey, string.Empty);

            quiz.UserId = CurrentUser;
            quiz.UserName = UserProfileService.GetById(CurrentUser).Name;

            if (!isEmptyAnswer)
            {
                QuizService.SaveAnswers(quiz, answers);
                //ActionHistory(quiz.Id, EntityAction.Answered, quiz.TargetType);
                ViewData["SurveyTitle"] = quiz.Title;
                ViewData["Message"] = "Thank you for your submission!";
                ActionHistoryService.RecordActionHistory(quiz.Id, EntityAction.Answered, DomainObjectType.Survey, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                return View("SuccessMessage");
            }
            else
            {
                SetFormData();
                ViewData["LongSurvey"] = quiz;
                ViewData["ErrorMessage"] = LabelResource.QuestionAnswerIsEmpty;
                return View("Questions");
            }

        }

        #endregion

        protected void SetFormData()
        {
            SetLabels();  
            IList<ResourceObject> QuestionTypeList = ResourceService.GetAll<QuestionType>();
            ViewData["TypeList"] = new SelectList(QuestionTypeList, "Id", "Value");
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);

            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetIndustryEnabled(clientCompany);
            IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = IndustryService.GetIndustryEnabled(clientCompany).Count;
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["SelectedCompetitorIdList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["SelectedIndustryIdList"] = new SelectList(new List<Industry>(), "Id", "Name");
            ViewData["EndUserIdMultiListDisabled"] = "false";
            ViewData["EmployeeIdList"] = new MultiSelectList(new List<Employee>(), "Id", "Name");
        }
    }
}
