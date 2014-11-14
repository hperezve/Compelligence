using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class ForumDiscussionController : BackEndAsyncFormController<ForumResponse, decimal>
    {

        #region Public Properties

        public IForumResponseService ForumDiscussionResponseService 
        {
            get { return (IForumResponseService)_genericService; }
            set { _genericService = value; }
        }

        public IForumService ForumDiscussionService { get; set; }
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

                ForumDiscussionService.SaveForumResponse(discussionResponse, ForumType.Discussion);
                operationStatus = OperationStatus.Successful;
                //return GetActionResultForCreate(discussionResponse, operationStatus);
                //return Content("<script> window.opener.UpdateCommentDetailList();self.close(); </script>");
                return View("ExternalMessage");
            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);
            ViewData["UserSecurityAccess"] = Request["UserSecurityAccess"];
            ViewData["EntityLocked"] = Request["EntityLocked"];
            SetFormData();

            SetFormEntityDataToForm(discussionResponse);

            SetUserSecurityAccess();

            //return View("Edit", discussionResponse);
            return View("ExternalMessage");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Create(ForumResponse discussionResponse, FormCollection collection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateFormData(discussionResponse, collection))
            {
                SetDetailFilterData(discussionResponse);

                GetFormData(discussionResponse, collection);

                SetDefaultEntityDataForSave(discussionResponse);

                SetDefaultDataForSave(discussionResponse);
                //Next line don't validated for work, verify with Library
                discussionResponse.Libraries = GetLibrariesForEntity(discussionResponse.EntityType, LibraryTypeKeyCode.File);

                ForumDiscussionService.SaveForumResponse(discussionResponse, ForumType.Discussion);

                operationStatus = OperationStatus.Successful;

                //return GetActionResultForCreate(discussionResponse, operationStatus);
                return Content(string.Empty);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            ViewData["UserSecurityAccess"] = Request["UserSecurityAccess"];
            ViewData["EntityLocked"] = Request["EntityLocked"];

            SetFormData();

            SetFormEntityDataToForm(discussionResponse);

            SetUserSecurityAccess();

            return View("Edit", discussionResponse);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEntityId(decimal id) //Receive forumresposeid
        {
            Forum forum = ForumDiscussionService.GetByForumResponse(id);
            string entityId = Convert.ToString(forum.EntityId);
            if (forum != null)
            {
                return Json(entityId); //when send paramater as decimal increases +1
            }
            else
            {
                return Json(-1);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveComments(string ObjectType, decimal EntityId, decimal forumresponseid)
        {
            Forum forum = ForumDiscussionService.GetByEntityId(EntityId, ObjectType, ForumType.Discussion);
            ForumDiscussionResponseService.DeleteCascading(forum.Id, forumresponseid);
            return Content(string.Empty);
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
            //Next block only works for ...
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

            Forum forum = ForumDiscussionService.GetByEntityId(entityId, entityType, ForumType.Discussion);
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
                forumresponses = ForumDiscussionResponseService.GetByForumId(forum.Id,entityType);
            }

            ViewData["Discussions"] = forumresponses;
        }

        protected override ActionResult GetActionResultForCreate(DomainObject<decimal> domainObject, OperationStatus operationStatus)
        {
            return RedirectToAction("DetailList", new { id = domainObject.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], Container = Request["Container"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"], UserSecurityAccess = Request["UserSecurityAccess"], EntityLocked = Request["EntityLocked"] });
        }


        #endregion

    }
}
