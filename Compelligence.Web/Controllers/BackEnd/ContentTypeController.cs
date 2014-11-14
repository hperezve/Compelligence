using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;

namespace Compelligence.Web.Controllers
{
    public class ContentTypeController : BackEndAsyncFormController<ContentType, decimal>
    {

        #region Public Properties

        public IContentTypeService ContentTypeService
        {
            get { return (IContentTypeService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        public override ActionResult Index()
        {
            
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            IList<ContentType> ContentTypes = ContentTypeService.GetAllActiveByClientCompany(CurrentCompany);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitive Messaging", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Positioning Statements", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "News", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Details Industry Competitor Product", CurrentCompany, CurrentUser);
            //Disable Strengths/Weaknesses
            ContentTypeService.CreateIfNotExist(ContentTypes, "Strengths/Weaknesses", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitors in Industry", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Products in Industry", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Industries", CurrentCompany, CurrentUser);
            return View();
        }
        protected override bool ValidateFormData(ContentType contentType, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(contentType.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ContentTypeNameRequiredError);
            }

            if (Validator.IsBlankOrNull(contentType.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.ContentTypeOwnerIdRequiredError);
            }
            if (!(Validator.IsBlankOrNull(contentType.DateFrm) || Validator.IsDate(contentType.DateFrm, GetFormatDate())))
            {
                ValidationDictionary.AddError("DueDateFrm", string.Format(LabelResource.ContentTypeDateFormatError, GetFormatDate()));
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(CurrentCompany);
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(ContentType contentType)
        {
            ViewData["DateFrm"] = DateTimeUtility.ConvertToString(contentType.Date, GetFormatDate());
        }

        protected override void GetFormData(ContentType contentType, FormCollection collection)
        {
            contentType.Date = DateTimeUtility.ConvertToDate(collection["DateFrm"], GetFormatDate());
        }

        protected override void SetUserSecurityAccess(ContentType contentType)
        {
            string securityAccess = UserSecurityAccess.Read;

            if (ContentTypeService.HasAccessToContentType(contentType, CurrentUser))
                securityAccess = UserSecurityAccess.Edit;

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override bool ValidateDeleteData(ContentType entity, System.Text.StringBuilder errorMessage)
        {
            //
            //Disable Strengths/Weaknesses
            if (entity.Name.Equals("Positioning Statements") ||
                entity.Name.Equals("News") ||
                entity.Name.Equals("Details Industry Competitor Product") ||
                entity.Name.Equals("Strengths/Weaknesses") ||
                entity.Name.Equals("Competitive Messaging") ||
                entity.Name.Equals("Competitors in Industry") ||
                entity.Name.Equals("Products in Industry") ||
                entity.Name.Equals("Industries"))
            {
                errorMessage.AppendLine("Object can't be deleted.");
                return false;
            }
          return base.ValidateDeleteData(entity, errorMessage);
        }

        //Duplicate Validate here,because Service not have Duplicate fucntion.
        //So, it's possible coding similary to ValidateDeleteData
        public override ActionResult Duplicate()
        {
            decimal id = decimal.Parse(Request["id"]);
            ContentType contenttype = ContentTypeService.GetById(id);
            //Disable Strengths/Weaknesses
            if (contenttype.Name.Equals("Positioning Statements")||
                contenttype.Name.Equals("News")||
                contenttype.Name.Equals("Details Industry Competitor Product") ||
                contenttype.Name.Equals("Competitive Messaging") ||
                contenttype.Name.Equals("Strengths/Weaknesses")||
                contenttype.Name.Equals("Competitors in Industry") ||
                contenttype.Name.Equals("Products in Industry") ||
                contenttype.Name.Equals("Industries"))
               return RedirectToAction("Edit", new { id = id, operationStatus = OperationStatus.Successful.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
            return base.Duplicate();
        }
        #endregion
    }
}
