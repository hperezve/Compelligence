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
using Compelligence.Common.Utility.Web;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class NuggetController : BackEndAsyncFormController<Quiz, decimal>
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

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Quiz quiz, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(quiz.Title))
            {
                ValidationDictionary.AddError("Title", LabelResource.NuggetTitleRequiredError);
            }

            if (Validator.IsBlankOrNull(quiz.Status))
            {
                ValidationDictionary.AddError("Status", LabelResource.NuggetStatusRequiredError);
            }
            else
            {
                if (quiz.Status == NuggetStatus.Complete && QuestionService.GetByQuizId(quiz.Id).Count <= 0)
                {
                    ValidationDictionary.AddError("Status", LabelResource.NuggetStatusValueError);
                }
            }

            if (Validator.IsBlankOrNull(quiz.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.NuggetAssignedToRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetDefaultEntityDataForSave(Quiz quiz)
        {
            quiz.TargetType = "Nugget";
            quiz.MetaData = quiz.Title + ":" + quiz.MetaData;
        }

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> NuggetStatusList = ResourceService.GetAll<NuggetStatus>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Quiz> quizList = QuizService.GetAllActiveByClientCompany(clientCompany);

            ViewData["StatusList"] = new SelectList(NuggetStatusList, "Id", "Value", NuggetStatus.Incomplete);
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Nugget;

            switch (detailType)
            {
                case DetailType.Question:
                    AddFilter(detailFilter, "Question.QuizId", parentId.ToString());
                    AddFilter(browseDetailFilter, "QuestionView.QuizId", parentId.ToString());

                    childController = "Question";
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

        protected override void SetEntityDataToForm(Quiz quiz)
        {
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(quiz.MetaData);
            quiz.OldTitle = quiz.Title;
        }

        protected override void GetFormData(Quiz quiz, FormCollection collection)
        {
            quiz.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        protected override void SetFormEntityDataToForm(Quiz quiz)
        {
            quiz.OldTitle = quiz.Title;
            quiz.MetaData = FormFieldsUtility.GetMultilineValue(quiz.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(quiz.MetaData, quiz.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldTitle", new ValueProviderResult(quiz.OldTitle, quiz.OldTitle, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}