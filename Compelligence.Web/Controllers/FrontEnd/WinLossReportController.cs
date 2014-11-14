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
using Compelligence.Domain.Entity.Resource;
using Compelligence.Security.Managers;
using System.Configuration;
using Compelligence.Security.Filters;

namespace Compelligence.Web.Controllers
{
    public class WinLossReportController : GenericFrontEndController
    {

        #region Public Properties

        public IQuizService QuizService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IActionHistoryService ActionHistoryService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public ActionResult GetQuestions(decimal id)
        {
            Quiz quiz = QuizService.GetById(id);
            ViewData["UniqueWinLoss"] = quiz;

            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = IndustryService.GetAllActiveByClientCompany(clientCompany).Count;
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["SelectedCompetitorIdList"] = new SelectList(new List<Competitor>(), "Id", "Name");
            ViewData["SelectedIndustryIdList"] = new SelectList(new List<Industry>(), "Id", "Name");
            ViewData["EndUserIdMultiListDisabled"] = "false";
            ViewData["EmployeeIdList"] = new MultiSelectList(new List<Employee>(), "Id", "Name");
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

            quiz.Libraries = GetLibrariesForEntity(quiz.Id, DomainObjectType.WinLoss, string.Empty);

            quiz.UserId = CurrentUser;
            quiz.UserName = UserProfileService.GetById(CurrentUser).Name;

            if (!isEmptyAnswer)
            {
                QuizService.SaveAnswers(quiz, answers);
                //ActionHistory(quiz.Id, EntityAction.Answered, quiz.TargetType);
                ActionHistoryService.RecordActionHistory(quiz.Id, EntityAction.Answered, DomainObjectType.WinLoss, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                ViewData["WinLossTitle"] = quiz.Title;
                ViewData["Message"] = "Thank you for your submission!";
                return View("SuccessMessage");
            }
            else
            {
                ViewData["WinLoss"] = quiz;
                ViewData["ErrorMessage"] = LabelResource.QuestionAnswerIsEmpty;
                return View("Questions");
            }
        }

        #endregion

       
    }
}
