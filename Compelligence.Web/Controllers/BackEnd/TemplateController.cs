using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;
using System.Text;

namespace Compelligence.Web.Controllers
{
    public class TemplateController : BackEndAsyncFormController<Template, decimal>
    {

        #region Public Properties

        public ITemplateService TemplateService
        {
            get { return (ITemplateService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IResourceService ResourceService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Template template, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(template.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.TemplateNameRequiredError);
            }

            if (Validator.IsBlankOrNull(template.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.TemplateAssignedToRequiredError);
            }
            if ((template.Status.Equals(TemplateStatus.Enabled)) && (!TemplateService.HasFile(template)))
            {
                ValidationDictionary.AddError("Status", LabelResource.TemplateStatusRequiredError);
            }
            //if (!Validator.IsValidUploadFile(template.FileName, FileValidator.Target.Templates))
            //{
            //    ValidationDictionary.AddError("FileName", LabelResource.TemplateFileNameUploadFileError);
            //}

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<ResourceObject> templateLanguageList = ResourceService.GetAll<TemplateLanguaje>();
            IList<ResourceObject> templateFormatList = ResourceService.GetAll<FileFormat>();
            IList<ResourceObject> templateType = ResourceService.GetAll<TemplateType>();
            IList<ResourceObject> templateStatus = ResourceService.GetAll<TemplateStatus>();

            ViewData["LanguageList"] = new SelectList(templateLanguageList, "Id", "Value");
            ViewData["FormatList"] = new SelectList(templateFormatList, "Id", "Value");
            ViewData["AssignedTolist"] = new SelectList(userList, "Id", "Name");
            ViewData["Typelist"] = new SelectList(templateType, "Id", "Value");
            ViewData["StatusList"] = new SelectList(templateStatus, "Id", "Value");
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Template;

            switch (detailType)
            {
                case DetailType.File:
                    AddFilter(detailFilter, "File.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "File.EntityType", DomainObjectType.Template);
                    AddFilter(browseDetailFilter, "FileDetailView.EntityId", parentId.ToString());
                    childController = "File:FileDetail";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Template);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Template);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Template);
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Template);
                    childController = "ForumDiscussion";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Template template)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (TemplateService.HasAccessToTemplate(template, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion
    }
}
