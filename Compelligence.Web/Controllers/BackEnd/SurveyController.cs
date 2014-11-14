using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using Resources;
using System.Text;


using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Common.Utility;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Security.Filters;
using Compelligence.Web.Models.Web;

using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility.Web;
using Compelligence.DataTransfer.Entity;
using Compelligence.Util.Collections;
using Compelligence.Util.Type;

using Spring.Context;
using Spring.Context.Support;
using Compelligence.Web.Models.Util;
using System.Globalization;
using System.Collections;
using Compelligence.Util.Common;

namespace Compelligence.Web.Controllers
{
    public class SurveyController : BackEndAsyncFormController<Quiz, decimal>
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
        public IQuestionDetailService QuestionDetailService { get; set; }
        public IAnswerService AnswerService { get; set; }
        public IQuizContentService QuizContentService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IIndustryService IndustryService { get; set; }
        public IFileService FileService { get; set; }

        #endregion

        #region Action Methods

        public ActionResult ShowSurvey(decimal QuizId)
        {
            ModelState.Clear();
            SetFormDataOfQuestion(CurrentUser);
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Initiated);
            SetEntityDataOfQuestionToForm(QuizId);
            ViewData["QuizId"] = QuizId;
            return View("SurveyBack");
        }

        public ActionResult ShowHtml(decimal QuizId)
        {
            ModelState.Clear();
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Initiated);
            QuizContent quizcontent = QuizContentService.GetByQuizId(QuizId);
            String htmlcontent = string.Empty;
            if (quizcontent != null)
                htmlcontent = quizcontent.Content;


            ViewData["HtmlContent"] = htmlcontent;
            ViewData["QuizId"] = QuizId;
            return View("SurveyHtml");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveHtml(FormCollection form)
        {
            Quiz quiz = QuizService.GetById(decimal.Parse(form["QuizId"]));
            String htmlcontent = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["HtmlContent"]));

            string _message = string.Empty;
            ViewData["Container"] = Request["Container"];


            _message = LabelResource.QuestionAnswerIsEmpty;
            SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Unsuccessful);
            if (!String.IsNullOrEmpty(htmlcontent))
            {
                SetDefaultRequestParametersToForm(ActionMethod.Edit, OperationStatus.Successful);

                QuizContent quizcontent = new QuizContent();
                quizcontent.QuizId = quiz.Id;
                quizcontent.Content = htmlcontent;
                SetDefaultDataForSave(quizcontent);
                QuizContentService.Save(quizcontent);

                _message = "Thank you for your submission!";
                JavaScriptResult r = new JavaScriptResult();
                r.Script = "hiddenMessage();showSendResponseMessageDialog();";
                return r;
            }
            else
            {
                ViewData["QuizId"] = quiz.Id;
                ViewData["Message"] = _message;
                ViewData["HtmlContent"] = htmlcontent;
                return View("SurveyHtml");
            }
        }

        public ActionResult GetAnswerbySurvey(decimal QuizResponseId)
        {
            IList<AnswerDTO> answerList = AnswerService.FindAnswersbySurvey(QuizResponseId);

            Quiz survey;

            foreach (AnswerDTO adto in answerList)
            {
                adto.Question.QuestionDetails = QuestionDetailService.GetByQuestionId(adto.Question.Id);
            }


            if (!Collection.IsNullOrEmpty<AnswerDTO>(answerList))
            {
                survey = QuizService.GetById(DecimalUtility.CheckNull(answerList[0].Question.QuizId));
                IList<Compelligence.Domain.Entity.File> fileList = new List<Compelligence.Domain.Entity.File>();
                ViewData["FileList"] = fileList;
                fileList = FileService.GetByQuizAndQuestionResponseId((decimal)survey.Id, QuizResponseId, CurrentCompany);

                if (fileList != null && fileList.Count > 0)
                {
                    ViewData["FileList"] = fileList;
                    //string nameFiles = string.Empty;
                    //foreach (Compelligence.Domain.Entity.File file in fileList)
                    //{
                    //    nameFiles += fileList;
                    //}
                }
                ViewData["Survey"] = survey;
                ViewData["AnswerList"] = answerList;
            }

            return View("Survey");

        }

        public ActionResult DownloadFileMailExecute(decimal id)
        {
            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            Compelligence.Domain.Entity.File file = FileService.GetById(id);
            if (file == null)
                return Content(string.Empty);
            fullpath += file.PhysicalName;

            if (System.IO.File.Exists(fullpath))
            {
                string mimeType = null;
                mimeType = FileUtility.GetMimeType("~\\" + path + file.PhysicalName);

                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + file.FileName.Replace(' ', '_'));
                Response.Clear();
                Response.WriteFile(fullpath);
                Response.End();
            }
            return Content(string.Empty);
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
                return View("SurveyBack");
            }
        }

        public ActionResult Answerss(FormCollection first)
        {
            string temp = first["QuizId"];
            Quiz QuizAnswered = QuizService.GetById(decimal.Parse(temp));
            IList<Question> QuestionsAnswered = QuestionService.GetByQuizId(QuizAnswered.Id);

            foreach (Question ActiveQuestion in QuestionsAnswered)
            {
                Answer answer = new Answer((decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey());
                answer.QuizId = QuizAnswered.Id;
                answer.QuestionId = ActiveQuestion.Id;
                answer.AnswerText = first["Q" + ActiveQuestion.Item];
                answer.LastChangedDate = DateTime.Today.Date;
                answer.CreatedDate = DateTime.Today.Date;
                AnswerService.Save(answer);
            }
            return RedirectToAction("Edit", "Answers");
        }

        public ActionResult ExportAll(string QuizId)
        {
            Decoder decoder = Encoding.Unicode.GetDecoder();

            IList<decimal> responseids = AnswerService.GetResponseIdByQuizId(decimal.Parse(QuizId));
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string uniqueKey = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            try
            {
                // Create the CSV file to which dataTable will be exported.	
                StreamWriter sw = new StreamWriter(fullpath + "temporally_" + uniqueKey, false);
                // First we will write the headers.	

                IDictionary<Question, IList<Answer>> matrix = new Dictionary<Question, IList<Answer>>();
                IList<Question> questionList = QuestionService.GetByQuizId(decimal.Parse(QuizId));
                foreach (decimal quizresponseid in responseids)
                {
                    IList<Answer> answers = AnswerService.GetByQuizResponseId(quizresponseid);
                    if (answers.Count < questionList.Count)
                    {
                        foreach (Question q in questionList)
                        {
                            bool needCreate = true;
                            foreach (Answer a in answers)
                            {
                                if (a.QuestionId == q.Id)
                                {
                                    needCreate = false;
                                }
                            }
                            if (needCreate)
                            {
                                Answer answerEmpty = new Answer();
                                answerEmpty.QuizId = decimal.Parse(QuizId);
                                answerEmpty.QuestionId = q.Id;
                                answerEmpty.QuizResponseId = quizresponseid;
                                answerEmpty.AnswerText = string.Empty;
                                answers.Add(answerEmpty);
                            }
                        }
                    }
                    foreach (Answer answer in answers)
                    {
                        Question question = QuestionService.GetById((decimal)answer.QuestionId);
                        if (question != null)
                        {
                            try
                            {
                                IList<Answer> subanswers = matrix[question];
                                subanswers.Add(answer);
                                matrix[question] = subanswers;
                            }
                            catch
                            {
                                IList<Answer> subanswers = new List<Answer>();
                                subanswers.Add(answer);
                                matrix[question] = subanswers;
                            }
                        }
                    }
                }
                string csvheader = " , , ";
                string csvDateHeader = " , , ";
                string csvSubHeader = "*, Question, ";
                foreach (decimal quizresponseid in responseids)
                {
                    csvSubHeader += "Response, ";
                    IList<Answer> answers = AnswerService.GetByQuizResponseId(quizresponseid);
                    if (answers != null && answers.Count > 0)
                    {
                        csvDateHeader += DateTimeUtility.ConvertToString(answers[0].CreatedDate, "MM/dd/yyyy HH:mm:ss") + ", ";
                        if (!string.IsNullOrEmpty(answers[0].Author))
                        {
                            UserProfile user = UserProfileService.GetById(answers[0].Author);
                            if (user != null)
                            {
                                csvheader += user.Name + ", ";
                            }
                        }
                    }
                }
                sw.WriteLine(csvheader);
                sw.WriteLine(csvDateHeader);
                sw.WriteLine(csvSubHeader);
                string csvbody = string.Empty;
                int n = 0;

                //Fill data
                foreach (Question q in matrix.Keys)
                {
                    if (q.Type.Equals(QuestionType.Competitors))
                    {
                        int num = (++n);
                        IList<Answer> subanswers = matrix[q];
                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Industries " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            csvbody += sAnswer[0].Replace(",", "; ") + ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Competitors " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[1].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Products " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[2].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Primary Industry " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[3].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Primary Competitor " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[4].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);
                    }
                    else
                    {
                        string valueTemp = q.QuestionText.Contains(",") ? string.Format("\"{0}\"", q.QuestionText) : q.QuestionText;
                        //                    csvbody = string.Empty + (++n) + "," + q.QuestionText.Replace(",", "/") + ", ";
                        csvbody = string.Empty + (++n) + "," + valueTemp + ",";
                        IList<Answer> subanswers = matrix[q];
                        foreach (Answer a in subanswers)
                        {
                            IList<QuestionDetail> detailQuestions = QuestionDetailService.GetByQuestionId((decimal)a.QuestionId);
                            foreach (QuestionDetail detail in detailQuestions)
                            {
                                if (a.AnswerText.Equals(detail.ResponseValue))
                                {
                                    a.AnswerText = detail.ResponseText;
                                    //a.AnswerText = detail.ResponseText.Contains(",") ? string.Format("\"{0}\"", detail.ResponseText) : detail.ResponseText;
                                }
                            }
                            //csvbody += a.AnswerText.ToString() + ", ";
                            string tempoAsnwer = a.AnswerText.Contains(",") ? string.Format("\"{0}\"", a.AnswerText) : a.AnswerText;
                            csvbody += tempoAsnwer + ",";

                        }
                        //end line
                        // csvbody += "&nbsp; ,";
                        sw.WriteLine(csvbody);
                    }
                }

                sw.Close();
            }
            catch (Exception ex)
            {

            }
            //return Content(string.Empty);
            return new DownloadResult { VirtualPath = "~\\" + path + "temporally_" + uniqueKey, FileDownloadName = "AnswerResults.csv".Replace(' ', '_') };
        }

        public ActionResult Export(string QuizResponseId)
        {
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            string uniqueKey = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            // Create the CSV file to which dataTable will be exported.	
            try
            {
                StreamWriter sw = new StreamWriter(fullpath + "temporally_" + uniqueKey, false);
                // First we will write the headers.	
                string[] qResponseIdArray = QuizResponseId.Split(':');
                Quiz quiz = QuizService.GetByQuizResponseId(decimal.Parse(qResponseIdArray[0]), CurrentCompany);
                IList<Question> questionList = QuestionService.GetByQuizId(quiz.Id);
                IDictionary<Question, IList<Answer>> matrix = new Dictionary<Question, IList<Answer>>();
                foreach (string quizresponseid in qResponseIdArray)
                {
                    IList<Answer> answers = AnswerService.GetByQuizResponseId(decimal.Parse(quizresponseid));
                    if (answers.Count < questionList.Count)
                    {
                        foreach (Question q in questionList)
                        {
                            bool needCreate = true;
                            foreach (Answer a in answers)
                            {
                                if (a.QuestionId == q.Id)
                                {
                                    needCreate = false;
                                }
                            }
                            if (needCreate)
                            {
                                Answer answerEmpty = new Answer();
                                answerEmpty.QuizId = quiz.Id;
                                answerEmpty.QuestionId = q.Id;
                                answerEmpty.QuizResponseId = decimal.Parse(quizresponseid);
                                answerEmpty.AnswerText = string.Empty;
                                answers.Add(answerEmpty);
                            }
                        }
                    }
                    foreach (Answer answer in answers)
                    {
                        Question question = QuestionService.GetById((decimal)answer.QuestionId);
                        if (question != null)
                        {
                            try
                            {
                                IList<Answer> subanswers = matrix[question];
                                subanswers.Add(answer);
                                matrix[question] = subanswers;
                            }
                            catch
                            {
                                IList<Answer> subanswers = new List<Answer>();
                                subanswers.Add(answer);
                                matrix[question] = subanswers;
                            }
                        }
                    }
                }

                string csvheader = " , ,";
                string csvDateHeader = " , , ";
                string csvSubHeader = "*, Question,";
                foreach (string id in qResponseIdArray)
                {
                    csvSubHeader += "Response, ";
                    IList<Answer> answers = AnswerService.GetByQuizResponseId(decimal.Parse(id));
                    if (answers != null && answers.Count > 0)
                    {
                        csvDateHeader += DateTimeUtility.ConvertToString(answers[0].CreatedDate, "MM/dd/yyyy HH:mm:ss") + ", ";
                        if (!string.IsNullOrEmpty(answers[0].Author))
                        {
                            UserProfile user = UserProfileService.GetById(answers[0].Author);
                            if (user != null)
                            {
                                csvheader += user.Name + ", ";
                            }
                        }
                    }
                }
                sw.WriteLine(csvheader);
                sw.WriteLine(csvDateHeader);
                sw.WriteLine(csvSubHeader);
                string csvbody = string.Empty;
                int n = 0;

                //Fill data
                foreach (Question q in matrix.Keys)
                {
                    if (q.Type.Equals(QuestionType.Competitors))
                    {
                        int num = (++n);
                        IList<Answer> subanswers = matrix[q];
                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Industries " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            csvbody += sAnswer[0].Replace(",", "; ") + ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Competitors " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[1].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Products " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[2].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Primary Industry " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[3].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);

                        csvbody = string.Empty + num + "," + q.QuestionText.Replace(",", "/") + ": Primary Competitor " + ", ";
                        foreach (Answer a in subanswers)
                        {
                            String[] sAnswer = a.AnswerText.Split(';');
                            if (sAnswer.Length > 1)
                                csvbody += sAnswer[4].Replace(",", "; ") + ", ";
                            else
                                csvbody += ", ";
                        }
                        sw.WriteLine(csvbody);
                    }
                    else
                    {
                        //csvbody = string.Empty + (++n) + "," + q.QuestionText.Replace(",", "/") + ", ";
                        string valueTemp = q.QuestionText.Contains(",") ? string.Format("\"{0}\"", q.QuestionText) : q.QuestionText;
                        csvbody = string.Empty + (++n) + "," + valueTemp + ",";
                        IList<Answer> subanswers = matrix[q];
                        foreach (Answer a in subanswers)
                        {
                            IList<QuestionDetail> detailQuestions = QuestionDetailService.GetByQuestionId((decimal)a.QuestionId);
                            foreach (QuestionDetail detail in detailQuestions)
                            {
                                if (a.AnswerText.Equals(detail.ResponseValue))
                                {
                                    a.AnswerText = detail.ResponseText;
                                }
                            }
                            //csvbody += a.AnswerText.ToString() + ", ";
                            string tempoAsnwer = a.AnswerText.Contains(",") ? string.Format("\"{0}\"", a.AnswerText) : a.AnswerText;
                            csvbody += tempoAsnwer + ",";

                        }
                        sw.WriteLine(csvbody);
                    }
                }

                sw.Close();
            }
            catch (Exception ex)
            {

            }
            //return Content(string.Empty);
            return new DownloadResult { VirtualPath = "~\\" + path + "temporally_" + uniqueKey, FileDownloadName = "AnswerResults.csv".Replace(' ', '_') };
        }


        public ActionResult Reorder(string Id)
        {
            IList<Question> questions = QuestionService.GetByQuizId(decimal.Parse(Id));
            ViewData["Questions"] = questions;
            return View("Reorder");
        }

        public ActionResult SaveReorder(FormCollection collection)
        {
            string QuizId = collection["QuizId"].ToString();
            string[] QuestionId = collection["txtQuestionId"].Split(',');
            decimal item = 1;
            foreach (string questionid in QuestionId)
            {
                Question Question = QuestionService.GetById(decimal.Parse(questionid));
                Question.Item = item;
                QuestionService.Update(Question);
                item++;
            }
            return null;
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Quiz quiz, FormCollection formCollection)
        {
            //if try Enable Won
            if (quiz.Status == SurveyStatus.Complete && quiz.Type == SurveyType.SalesForce && quiz.SalesForceType == SurveySalesForceType.Won)
            {
                if (QuizService.GetSelectTypeSalesForceEnable(QuizType.Survey, SurveyStatus.Complete, SurveyType.SalesForce, CurrentCompany, SurveySalesForceType.Won) > 0)
                {

                    ValidationDictionary.AddError("Status", LabelResource.SurveyDuplicateSalesForceCompleteWon);
                }
            }
            else
                //if try Enable Lost
                if (quiz.Status == SurveyStatus.Complete && quiz.Type == SurveyType.SalesForce && quiz.SalesForceType == SurveySalesForceType.Lost)
                {
                    if (QuizService.GetSelectTypeSalesForceEnable(QuizType.Survey, SurveyStatus.Complete, SurveyType.SalesForce, CurrentCompany, SurveySalesForceType.Lost) > 0)
                    {
                        ValidationDictionary.AddError("Status", LabelResource.SurveyDuplicateSalesForceCompleteLost);
                    }
                }

            if (Validator.IsBlankOrNull(quiz.Title))
            {
                ValidationDictionary.AddError("Title", LabelResource.SurveyTitleRequiredError);
            }

            if (Validator.IsBlankOrNull(quiz.Status))
            {
                ValidationDictionary.AddError("Status", LabelResource.SurveyStatusRequiredError);
            }
            else
            {
                if (quiz.SalesForceType.Equals(SurveySalesForceType.Html))
                {
                    if (String.IsNullOrEmpty(quiz.Description))
                    {
                        ValidationDictionary.AddError("Description", "Description is empty.");
                    }
                }
                else
                    if (quiz.Status == SurveyStatus.Complete && QuestionService.GetByQuizId(quiz.Id).Count <= 0)
                    {
                        ValidationDictionary.AddError("Status", LabelResource.SurveyStatusValueError);
                    }
            }

            if (Validator.IsBlankOrNull(quiz.Type))
            {
                ValidationDictionary.AddError("Type", LabelResource.SurveyTypeRequiredError);
            }
            if (Validator.IsBlankOrNull(quiz.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.SurveyAssignedToRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetDefaultEntityDataForSave(Quiz quiz)
        {
            quiz.TargetType = "Survey";
            quiz.MetaData = quiz.Title + ":" + quiz.MetaData;
        }


        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> SurveyStatusList = ResourceService.GetAll<SurveyStatus>();
            IList<ResourceObject> SurveyTypeList = ResourceService.GetAll<SurveyType>();
            IList<ResourceObject> SurveyVisibleList = ResourceService.GetAll<SurveyVisible>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<ResourceObject> closedTypeList = ResourceService.GetAll<SurveySalesForceType>();
            ViewData["StatusList"] = new SelectList(SurveyStatusList, "Id", "Value", SurveyStatus.Incomplete);
            ViewData["TypeList"] = new SelectList(SurveyTypeList, "Id", "Value");
            ViewData["VisibleList"] = new SelectList(SurveyVisibleList, "Id", "Value", SurveyVisible.No);
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["SalesForceTypeList"] = new SelectList(closedTypeList, "Id", "Value");

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
        }


        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Survey;

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

                case DetailType.Results:
                    AddFilter(detailFilter, "Answer.QuizId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ResultDetailView.Id", parentId.ToString());

                    childController = "Answer";
                    break;
                //case DetailType.Industry:
                //    AddFilter(detailFilter, "Industry.QuizId", parentId.ToString());
                //    //AddFilter(detailFilter, "Industry.EntityType", DomainObjectType.Competitor);
                //    AddFilter(browseDetailFilter, "IndustryQuizDetailView.Id", parentId.ToString());
                //    childController = "Industry:IndustryQuizDetail";
                //    break;
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

        protected override void SetEntityDataToForm(Quiz quiz)
        {
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(quiz.MetaData);
            quiz.OldTitle = quiz.Title;
        }

        protected override void SetFormEntityDataToForm(Quiz quiz)
        {
            quiz.OldTitle = quiz.Title;
            quiz.MetaData = FormFieldsUtility.GetMultilineValue(quiz.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(quiz.MetaData, quiz.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldTitle", new ValueProviderResult(quiz.OldTitle, quiz.OldTitle, CultureInfo.InvariantCulture));
        }

        protected override void GetFormData(Quiz quiz, FormCollection collection)
        {
            string clientCompany = (string)Session["ClientCompany"];
            quiz.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
            if (!quiz.Type.Equals(SurveyType.SalesForce))
            {
                if (!string.IsNullOrEmpty(quiz.SalesForceType))
                {
                    quiz.SalesForceType = string.Empty;
                }
            }
        }

        #endregion

        private void SetFormDataOfQuestion(string user)
        {
            IList<UserProfile> enduserList = UserProfileService.GetEndUsers(CurrentCompany);
            UserProfile User = UserProfileService.GetById(user);
            enduserList.Insert(0, User);
            ViewData["AssignedToEndUserList"] = new SelectList(enduserList, "Id", "Name");

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

            string clientCompany = (string)Session["ClientCompany"];
            IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);
            Question ExistQCO = QuestionService.GetByQuizIdAndType(QuizId, QuestionType.Competitors);
            Question ExistQWC = QuestionService.GetByQuizIdAndType(QuizId, QuestionType.WinningCompetitor);

            if (ExistQCO != null && ExistQWC != null)
            {
                if (ExistQWC.Item < ExistQCO.Item)
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                }
                else
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
                }
            }
            else
            {
                ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
            }

            ViewData["WinningItem"] = ExistQWC.Item.ToString();
        }


    }
}