using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Common.Utility;
using Resources;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class ShortSurveyController : GenericFrontEndController
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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Answer(decimal id, FormCollection form)
        {
            Quiz shortSurvey = QuizService.GetById(id);

            IList<Answer> answers = new List<Answer>();

            decimal uniqueKey = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();

            bool isEmptyAnswer = false;

            foreach (Question question in shortSurvey.Questions)
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

            shortSurvey.UserId = CurrentUser;
            shortSurvey.UserName = UserProfileService.GetById(CurrentUser).Name;

            if (isEmptyAnswer)
            {
                SetFormData();
                ViewData["ShortSurvey"] = shortSurvey;
                ViewData["ErrorMessage"] = LabelResource.QuestionAnswerIsEmpty;

                return View("Questions");
            }
            else
            {
                QuizService.SaveAnswers(shortSurvey, answers);

                //ActionHistory(shortSurvey.Id, EntityAction.Answered, shortSurvey.TargetType);
                ActionHistoryService.RecordActionHistory(shortSurvey.Id, EntityAction.Answered, DomainObjectType.Survey, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                return Content("<h3>" + shortSurvey.Title + "</h3><hr /><h4>Thanks for you participation</h4>");
            }
        }

        #endregion

        protected void SetFormData()
        {
            SetLabels();  
            IList<ResourceObject> QuestionTypeList = ResourceService.GetAll<QuestionType>();
            ViewData["TypeList"] = new SelectList(QuestionTypeList, "Id", "Value");

            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetIndustryEnabled(clientCompany);
            decimal industryListCount = IndustryService.GetIndustryEnabled(clientCompany).Count;
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["SelectedCompetitorIdList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["SelectedIndustryIdList"] = new SelectList(new List<Industry>(), "Id", "Name");
            ViewData["EndUserIdMultiListDisabled"] = "false";
            ViewData["EmployeeIdList"] = new MultiSelectList(new List<Employee>(), "Id", "Name");
        }

    }
}
