using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Validation;
using Compelligence.Common.Utility.Web;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class SourceController : BackEndAsyncFormController<Source, decimal>
    {

        #region Public Properties

        public ISourceService SourceService
        {
            get { return (ISourceService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Source source, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(source.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.SourcetNameRequiredError);
            }

            if (Validator.IsBlankOrNull(source.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.SourceAssignedToRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];

            IList<ResourceObject> sourceTypeList = ResourceService.GetAll<SourceType>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);


            ViewData["TypeList"] = new SelectList(sourceTypeList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;

        }

        protected override void SetEntityDataToForm(Source source)
        {
            ViewData["CreatedByFrm"] = UserProfileService.GetById(source.CreatedBy).Name;
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(source.MetaData);
            ViewData["CreatedByFrm"] = UserProfileService.GetById(source.CreatedBy).Name;
            source.OldName = source.Name;
        }

        protected override void SetUserSecurityAccess(Source source)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (SourceService.HasAccessToSource(source, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void GetFormData(Source source, FormCollection collection)
        {
            source.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        protected override void SetDefaultEntityDataForSave(Source source)
        {
            source.MetaData = source.Name + ":" + source.MetaData;
        }

        protected override void SetFormEntityDataToForm(Source source)
        {
            source.OldName = source.Name;
            source.MetaData = FormFieldsUtility.GetMultilineValue(source.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(source.MetaData, source.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(source.OldName, source.OldName, CultureInfo.InvariantCulture));
        }

        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Source entity = SourceService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }
        #endregion
    }
}
