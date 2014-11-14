using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Web;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class ForumCommentController : BackEndAsyncFormController<ForumResponse, decimal>
    {
        #region Public Properties

        public IForumResponseService ForumCommentResponseService
        {
            get { return (IForumResponseService)_genericService; }
            set { _genericService = value; }
        }

        public IForumService ForumCommentService { get; set; }
        public IResourceService ResourceService { get; set; }
        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public  ActionResult Save(ForumResponse discussionResponse, FormCollection collection)
        {
            //for XSS
            discussionResponse.Response = HttpUtility.HtmlEncode(StringUtility.CheckNull(discussionResponse.Response));
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateFormData(discussionResponse, collection))
            {
                SetDetailFilterData(discussionResponse);

                GetFormData(discussionResponse, collection);

                SetDefaultEntityDataForSave(discussionResponse);

                SetDefaultDataForSave(discussionResponse);
                
                //Next line don't validated for work, verify with Library
                discussionResponse.Libraries = GetLibrariesForEntity(discussionResponse.EntityType, LibraryTypeKeyCode.File);

                ForumCommentService.SaveForumResponse(discussionResponse, ForumType.Comment);
                operationStatus = OperationStatus.Successful;
                //return GetActionResultForCreate(discussionResponse, operationStatus);
                return Content("<script> window.opener.UpdateCommentDetailList();self.close(); </script>");

            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            ViewData["UserSecurityAccess"] = Request["UserSecurityAccess"];
            ViewData["EntityLocked"] = Request["EntityLocked"];

            SetFormData();

            SetFormEntityDataToForm(discussionResponse);

            SetUserSecurityAccess();

            return View("Edit", discussionResponse);
            
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(ForumResponse discussionResponse, FormCollection formCollection)
        {

            if (Validator.IsBlankOrNull(discussionResponse.Response))
            {
                ValidationDictionary.AddError("Response", "Response text is required");
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string parentId = Request["parentId"];
            //Next block only works for Workspace/Deal,Event and Project
            try
            {
                ViewData["EntityTypeName"] = ResourceService.GetName<DomainObjectType>(Request["HeaderType"]);
            }
            catch
            {
                ViewData["EntityTypeName"] = "None";
            }

            if (!string.IsNullOrEmpty(parentId))
            {
                ViewData["ParentResponseId"] = parentId;
            }
        }

        protected override void SetDetailFormData()
        {
            decimal entityId = Convert.ToDecimal(GetDetailFilterValue("ForumResponse.EntityId"));
            string entityType = GetDetailFilterValue("ForumResponse.EntityType");

            Forum forum = ForumCommentService.GetByEntityId(entityId, entityType, ForumType.Comment);
            IList<ForumResponse> forumresponses = new List<ForumResponse>();

            try
            {
                ViewData["EntityTypeName"] = ResourceService.GetName<DomainObjectType>(Request["HeaderType"]);
            }
            catch
            {
                ViewData["EntityTypeName"] = "None";
            }

            if (forum != null)
            {
                if (entityType.Equals(DomainObjectType.Industry) || entityType.Equals(DomainObjectType.Product))
                    forumresponses = ForumCommentResponseService.GetByEntityAndType(entityId, entityType, ForumType.Comment);            
                else
                    forumresponses = ForumCommentResponseService.GetByForumId(forum.Id,entityType);
            }

            ViewData["Comments"] = forumresponses;
        }

        protected override ActionResult GetActionResultForCreate(DomainObject<decimal> domainObject, OperationStatus operationStatus)
        {
            return RedirectToAction("DetailList", new { id = domainObject.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], Container = Request["Container"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"], UserSecurityAccess = Request["UserSecurityAccess"], EntityLocked = Request["EntityLocked"] });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveComments(string ObjectType,decimal EntityId, decimal forumresponseid)
        {
            Forum forum = ForumCommentService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
            ForumCommentResponseService.DeleteCascading(forum.Id, forumresponseid);
            return Content(string.Empty);
        }
        #endregion

    }
}
