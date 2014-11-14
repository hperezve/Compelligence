using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Util.Validation;
using System.Configuration;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;
using Compelligence.Common.Utility.Web;
using Compelligence.Util.Collections;
using Compelligence.Util.Type;
using Compelligence.Common.Utility.Upload;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    public class AnswerController : BackEndAsyncFormController<Answer, decimal>
    {

        #region Public Properties

        public IAnswerService AnswerService
        {
            get { return (IAnswerService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IProductService ProductService { get; set; }
        public IIndustryService IndustryService { get; set; }
        public IQuestionService QuestionService { get; set; }
        public IQuizService QuizService { get; set; }

        #endregion

        public ActionResult GetQuestions(decimal id)
        {
            Quiz quiz = QuizService.GetById(id);

            ViewData["Quiz"] = quiz;
            ViewData["QuizType"] = quiz.TargetType;
            ///this controller use BackEndAsyncFormController then we use SetDataToHelp of GenericController
            if (quiz.TargetType.Equals("Win/Loss"))
            {
                SetDataToHelp(ActionFrom.FrontEnd, FrontEndPages.WinLoss, "Winn Loss");
            }
            else if (quiz.TargetType.Equals("Survey"))
            {
                SetDataToHelp( ActionFrom.FrontEnd, FrontEndPages.Survey, "Survey");
            }
            else
            {
                SetDataToHelp( ActionFrom.FrontEnd, string.Empty, string.Empty);
            }
            SetFormData();
            SetFormDataOfQuestion(CurrentUser,quiz.Id);
            GetDataOfConfiguration(CurrentCompany);
            return View("FrontEndRespond");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Respond(FormCollection form)
        {
            Quiz quiz = QuizService.GetById(decimal.Parse(form["QuizId"]));
            string userSelect = form["Created"].ToString();

            IList<Answer> answers = new List<Answer>();
            decimal uniqueKey = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
            Boolean isEmptyAnswer = false;
            string _message = string.Empty;
            ViewData["Container"] = Request["Container"];
            SetDetailContent();

            _message = LabelResource.QuestionAnswerIsEmpty;
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Unsuccessful);
            if (ValidateQuizResponseData(quiz, form))
            {
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
            }
            else
            {
                SetFormDataOfQuestion(userSelect, quiz.Id);
                Fr(quiz, form);
                ViewData["Quiz"] = quiz;
                ViewData["QuizId"] = quiz.Id;
                ViewData["QuizQuestions"] = quiz.Questions;
                return View("Respond");
            }
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Successful);
            QuizService.SaveAnswers(quiz, answers);
            _message = "Thank you for your submission!";
            JavaScriptResult r = new JavaScriptResult();
            r.Script = "hiddenMessage();showSendResponseMessageDialog();";
            return r;
            //foreach (Question question in quiz.Questions)
            //{
            //    if (ValidateRespondData(question, form))
            //    {
            //        Answer answer = new Answer();
            //        answer.QuizResponseId = uniqueKey;
            //        answer.QuizId = question.QuizId;
            //        answer.QuestionId = question.Id;
            //        answer.AnswerText = form["Q" + question.Item];
            //        ViewData["Q" + question.Item] = form["Q" + question.Item];

            //        if (string.IsNullOrEmpty(answer.AnswerText))
            //        {
            //            isEmptyAnswer = true;
            //        }
            //        answer.Author = form["Created"];
            //        SetDefaultDataForSave(answer);
            //        answers.Add(answer);
            //    }
            //    else
            //    {
            //        SetFormDataOfQuestion(userSelect, quiz.Id);
            //        Fr(quiz, form);
            //        ViewData["Quiz"] = quiz;
            //        ViewData["QuizId"] = quiz.Id;
            //        ViewData["QuizQuestions"] = quiz.Questions;
            //        return View("Respond");
            //    }

            //}
            //if (!isEmptyAnswer)
            //{
            //    SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Successful);
            //    QuizService.SaveAnswers(quiz, answers);
            //    _message = "Thank you for your submission!";
            //    JavaScriptResult r = new JavaScriptResult();
            //    r.Script = "hiddenMessage();showSendResponseMessageDialog();";
            //    return r;
            //}
            //else
            //{
            //    SetFormDataOfQuestion(userSelect,quiz.Id);
            //    ViewData["Quiz"] = quiz;
            //    ViewData["QuizId"] = quiz.Id;
            //    ViewData["QuizQuestions"] = quiz.Questions;
            //    ViewData["Message"] = _message;
            //    return View("respond");
            //}
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShortRespond(decimal id, FormCollection form)
        {
            Quiz shortSurvey = QuizService.GetById(id);

            IList<Answer> answers = new List<Answer>();

            decimal uniqueKey = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();

            bool isEmptyAnswer = false;

            foreach (Question question in shortSurvey.Questions)
            {
                if (ValidateRespondData(question, form))
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
                    answer.ClientCompany = CurrentCompany;
                    answers.Add(answer);
                }
                else
                {
                    SetFormData();
                    Fr(shortSurvey, form);
                    ViewData["ShortSurvey"] = shortSurvey;
                    ViewData["ErrorMessage"] = string.Format(LabelResource.RespondTypeOpenTextError, question.Item, form["Q" + question.Item].Length);
                    return View("ShortRespond");
                }
            }

            shortSurvey.UserId = CurrentUser;
            shortSurvey.UserName = UserProfileService.GetById(CurrentUser).Name;

            if (isEmptyAnswer)
            {
                SetFormData();
                Fr(shortSurvey, form);
                ViewData["ShortSurvey"] = shortSurvey;
                ViewData["ErrorMessage"] = LabelResource.QuestionAnswerIsEmpty;

                return View("ShortRespond");
            }
            else
            {
                QuizService.SaveAnswers(shortSurvey, answers);

                //ActionHistory(shortSurvey.Id, EntityAction.Answered, shortSurvey.TargetType);
                ActionHistoryService.RecordActionHistory(shortSurvey.Id, EntityAction.Answered, DomainObjectType.Survey, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                return Content("<h3>" + shortSurvey.Title + "</h3><hr /><h4>Thanks for you participation</h4>");
            }
        }
        private void Fr(Quiz shortSurvey, FormCollection form)
        {
            SetLabels();
            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            //IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);
            IList<Competitor> competitorList = new List<Competitor>();
            IList<Product> productList = new List<Product>();

            foreach (Question question in shortSurvey.Questions)
            {
                if (question.Type.Equals(QuestionType.Competitors))
                { 
                  string industrySelected = form["QuestionIndustriesIds"+question.Item];
                   
                    
                    string primaryIndustry = form["PrimaryIndustry"+question.Item];
                    string primaryCompetitor = form["PrimaryCompetitor" + question.Item];
                    IList<Industry> primaryIndustryList = new List<Industry>();
                    IList<Competitor> primaryCompetitorList = new List<Competitor>();
                    if (!string.IsNullOrEmpty(industrySelected))
                    {
                        string[] selectedIndustryValues = industrySelected.Split(',');
                        var selectedIndustries = selectedIndustryValues;

                        ViewData["IndustryIdList"] = new MultiSelectList(industryList, "Id", "Name", selectedIndustries);
                    
                    if (selectedIndustries.Length > 0)
                    {
                        for (int ind = 0; ind < industryList.Count; ind++) 
                        {
                            for (int index = 0; index < selectedIndustries.Length; index++)
                            {
                                if (industryList[ind].Id.ToString().Equals(selectedIndustries[index]))
                                {
                                    primaryIndustryList.Add(industryList[ind]);
                                }
                            }
                        }
                        primaryIndustryList.Insert(0, new Industry());
                        ViewData["PrimaryIndustryList"] = new SelectList(primaryIndustryList, "Id", "Name", primaryIndustry);
                        for (int i = 0; i < selectedIndustries.Length; i++)
                        {
                            IList<Competitor> tempCompetitorList = CompetitorService.GetByIndustryId(Convert.ToDecimal(selectedIndustries[i]));
                            foreach (Competitor comp in tempCompetitorList)
                            {
                                if (competitorList.IndexOf(comp) == -1)
                                {
                                    competitorList.Add(comp);
                                }
                            }
                        }
                        string competitorSelected = form["QuestionCompetitorsIds" + question.Item];
                        if (!string.IsNullOrEmpty(competitorSelected))
                        {
                            string[] selectedCompetitorValues = competitorSelected.Split(',');
                            var selectedCompetitors = selectedCompetitorValues;
                            ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedCompetitors);
                            if (selectedCompetitors.Length > 0)
                            {
                                for (int comp = 0; comp < competitorList.Count; comp++)
                                {
                                    for (int co = 0; co < selectedCompetitors.Length; co++)
                                    {
                                        if (competitorList[comp].Id.ToString().Equals(selectedCompetitors[co]))
                                        {
                                            primaryCompetitorList.Add(competitorList[comp]);
                                        }
                                    }
                                }
                                primaryCompetitorList.Insert(0, new Competitor());
                                ViewData["PrimaryCompetitorList"] = new SelectList(primaryCompetitorList, "Id", "Name", primaryCompetitor);
                                for (int c = 0; c < selectedCompetitors.Length; c++)
                                {
                                    IList<Product> tempProductList = ProductService.GetByCompetitorAndStatusEnabled(Convert.ToDecimal(selectedCompetitors[c]));
                                    int cont = 0;
                                    foreach (Product prod in tempProductList)
                                    {
                                        if (productList.IndexOf(prod) == -1)
                                        {
                                            productList.Add(prod);
                                        }
                                    }
                                }
                                string productSelected = form["QuestionProductsIds" + question.Item];
                                if (!string.IsNullOrEmpty(productSelected))
                                {
                                    string[] selectedproductValues = productSelected.Split(',');
                                    var selectedProducts = selectedproductValues;
                                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name", selectedProducts);
                                }
                                else
                                {
                                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name");
                                }

                            }
                        }
                        else
                        {
                            ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                            ViewData["PrimaryCompetitorList"] = new SelectList(primaryCompetitorList, "Id", "Name");
                        }
                    }
                    }
                    else
                    {
                        ViewData["IndustryIdList"] = new MultiSelectList(industryList, "Id", "Name"); }
                    string answerToQuestionC = form["Q" + question.Item];
                    if (string.IsNullOrEmpty(answerToQuestionC)) { answerToQuestionC = string.Empty; }
                    ViewData["Q" + question.Item] = answerToQuestionC;
                }
                else if (question.Type.Equals(QuestionType.WinningCompetitor))
                {
                    Question ExistQCO = QuestionService.GetByQuizIdAndType(shortSurvey.Id, QuestionType.Competitors);
                    Question ExistQWC = QuestionService.GetByQuizIdAndType(shortSurvey.Id, QuestionType.WinningCompetitor);

                    if (ExistQCO != null && ExistQWC != null)
                    {
                        if (ExistQWC.Item < ExistQCO.Item)
                        {
                            ViewData["WinningItem"] = "X";
                        }
                        else
                        {
                            ViewData["WinningItem"] = ExistQWC.Item.ToString();
                        }
                    }
                    else
                    {
                        ViewData["WinningItem"] = "X";
                    }
                    string answerToWI = form["Q" + question.Item];
                    if (string.IsNullOrEmpty(answerToWI)) { answerToWI = string.Empty; }
                    ViewData["Q" + question.Item] = answerToWI;
                }
                else if (question.Type.Equals(QuestionType.MultipleChoice))
                {
                    string answerToQuestionMC = form["Q" + question.Item];
                    if (string.IsNullOrEmpty(answerToQuestionMC)) { answerToQuestionMC = string.Empty; }
                    ViewData["Q" + question.Item] = answerToQuestionMC;
                }
                else if (question.Type.Equals(QuestionType.OpenText))
                {
                    string answerToQuestionOT = form["Q" + question.Item];
                    if (string.IsNullOrEmpty(answerToQuestionOT)) { answerToQuestionOT = string.Empty; }
                    ViewData["Q" + question.Item] = answerToQuestionOT;
                }
                else if (question.Type.Equals(QuestionType.YesorNot))
                {
                    string answerToQuestionYN = form["Q" + question.Item];
                    if (string.IsNullOrEmpty(answerToQuestionYN)) { answerToQuestionYN = string.Empty; }
                    ViewData["Q" + question.Item] = answerToQuestionYN;
                }
            }
        }

        protected bool ValidateRespondData(Question question, FormCollection form)
        {
            if (question.Type.Equals("OT") && form["Q" + question.Item].Length>1000)
            {
                ValidationDictionary.AddError("Q" + question.Item, string.Format(LabelResource.RespondTypeOpenTextError, question.Item, form["Q" + question.Item].Length));
            }
            return ValidationDictionary.IsValid;
        }

        protected bool ValidateQuizResponseData(Quiz quiz, FormCollection form)
        {
            foreach (Question question in quiz.Questions)
            {
                if (string.IsNullOrEmpty(form["Q" + question.Item]))
                {
                    ValidationDictionary.AddError("Q" + question.Item, string.Format(LabelResource.AnswerRequiredError, question.Item));
                }
                if (question.Type.Equals("OT") && form["Q" + question.Item].Length > 1000)
                {
                    ValidationDictionary.AddError("Q" + question.Item, string.Format(LabelResource.RespondTypeOpenTextError, question.Item, form["Q" + question.Item].Length));
                }
            }
            return ValidationDictionary.IsValid;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FrontEndRespond(decimal id, FormCollection form)
        {
            Quiz quiz = QuizService.GetById(id);

            IList<Answer> answers = new List<Answer>();

            decimal uniqueKey = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();

            bool isEmptyAnswer = false;
            int result = 0;
            foreach (Question question in quiz.Questions)
            {
                if (ValidateRespondData(question, form))
                {
                    Answer answer = new Answer();

                    answer.QuizResponseId = uniqueKey;
                    answer.QuizId = question.QuizId;
                    answer.QuestionId = question.Id;
                    answer.AnswerText = form["Q" + question.Item];

                    ViewData["Q" + question.Item] = form["Q" + question.Item];

                    if (string.IsNullOrEmpty(answer.AnswerText) && !question.Type.Equals(QuestionType.Competitors) && !question.Type.Equals(QuestionType.WinningCompetitor))
                    {
                        isEmptyAnswer = true;
                    }
                    if (!string.IsNullOrEmpty(answer.AnswerText))
                    {
                        result++;
                    }
                    answer.Author = CurrentUser;

                    answer.CreatedBy = CurrentUser;
                    answer.CreatedDate = DateTime.Now;
                    answer.LastChangedBy = CurrentUser;
                    answer.LastChangedDate = DateTime.Now;

                    answers.Add(answer);
                }
                else
                {
                    SetFormDataOfQuestion(CurrentUser, quiz.Id);
                    SetFormData();
                    Fr(quiz, form);
                    ViewData["Quiz"] = quiz;
                    ViewData["ErrorMessage"] = string.Format(LabelResource.RespondTypeOpenTextError, question.Item, form["Q" + question.Item].Length);
                    return View("FrontEndRespond");
                }
            }
            String EntityType = "";
            if(quiz.TargetType.Equals(QuizType.Survey))
                EntityType = DomainObjectType.Survey;
            if (quiz.TargetType.Equals(QuizType.WinLoss))
                EntityType = DomainObjectType.WinLoss;

            quiz.Libraries = GetLibrariesForEntity(quiz.Id, EntityType, string.Empty);
            //System.Net.Mail.AttachmentCollection attchementFiles = new System.Net.Mail.AttachmentCollection(
            //string file = "";
            //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(file);
            quiz.UserId = CurrentUser;
            quiz.UserName = UserProfileService.GetById(CurrentUser).Name;

            if (result != 0)
            {
                QuizService.SaveAnswers(quiz, answers);
                SetFormData();
                ViewData["QuizTitle"] = quiz.Title;
                ViewData["Message"] = "Thank you for your submission!";
                ActionHistoryService.RecordActionHistory(quiz.Id, EntityAction.Answered, EntityType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                return View("SuccessMessage");
            }
            else
            {
                SetFormDataOfQuestion(CurrentUser, quiz.Id);
                SetFormData();
                ViewData["Quiz"] = quiz;
                //ViewData["ErrorMessage"] = LabelResource.QuestionAnswerIsEmpty;
                return View("FrontEndRespond");
            }

        }

        public ActionResult ShowQuiz(decimal QuizId)
        {
            ModelState.Clear();
            SetFormData();
            SetFormDataOfQuestion(CurrentUser,QuizId);
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Initiated);
            SetDetailContent();
            ViewData["QuizId"] = QuizId;
            return View("Respond");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteByResponseId()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');


            foreach (string identifier in ids)
            {
                IList<Answer> answers = AnswerService.GetByQuizResponseId(decimal.Parse(identifier));
                foreach (Answer answer in answers)
                {
                    SetDefaultDataFromRequest(answer);

                    SetDetailFilterData(answer);

                    SetDefaultDataForUpdate(answer);

                    // GenericService.DeleteRelations(answer);
                    AnswerService.DeleteEntity(answer);
                }
            }
            return null;
        }

        private void SetFormDataOfQuestion(string user, decimal QuizId)
        {
            IList<UserProfile> enduserList = UserProfileService.GetEndUsers(CurrentCompany);
            UserProfile User = UserProfileService.GetById(user);
            enduserList.Insert(0, User);
            Quiz ActiveQuiz = QuizService.GetById(QuizId);
       
            SetLabels();  

            IList<Question> QuestionCollection = QuestionService.GetByQuizId(ActiveQuiz.Id);
            if (QuestionCollection == null)
            {
                QuestionCollection = new List<Question>();
            }

            foreach (Question ActiveQuestion in QuestionCollection)
            {
                ActiveQuestion.QuestionDetails = QuestionService.GetDetails(ActiveQuestion.Id);
            }

            ViewData["AssignedToEndUserList"] = new SelectList(enduserList, "Id", "Name");

            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);
            
            ViewData["IndustryIdList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["CountQuestions"] = QuestionCollection.Count;
            ViewData["quiz"] = ActiveQuiz;
            ViewData["QuizQuestions"] = QuestionCollection;

            Question ExistQCO = QuestionService.GetByQuizIdAndType(QuizId, QuestionType.Competitors);
            Question ExistQWC = QuestionService.GetByQuizIdAndType(QuizId, QuestionType.WinningCompetitor);

            if (ExistQCO != null && ExistQWC != null)
            {
                if (ExistQWC.Item < ExistQCO.Item)
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                    ViewData["WinningItem"] = "X";
                }
                else
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
                    ViewData["WinningItem"] = ExistQWC.Item.ToString();
                }
            }
            else
            {
                ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                ViewData["WinningItem"] = "X";
            }

           
        }

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            GetDataOfConfiguration(CurrentCompany);
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
        }
        #endregion

    }
}
