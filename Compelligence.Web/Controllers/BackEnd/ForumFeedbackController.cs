using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class ForumFeedbackController : BackEndAsyncFormController<ForumResponse, decimal>
    {

        #region Public Properties

        public IForumResponseService ForumFeedbackResponseService
        {
            get { return (IForumResponseService)_genericService; }
            set { _genericService = value; }
        }

        public IForumService ForumFeedbackService { get; set; }

        #endregion

        #region Override Methods

        protected override void SetDetailFormData()
        {
            decimal entityId = Convert.ToDecimal(GetDetailFilterValue("ForumResponse.EntityId"));
            string entityType = GetDetailFilterValue("ForumResponse.EntityType");

            Forum forum = ForumFeedbackService.GetByEntityId(entityId, entityType, ForumType.FeedBack);
            IList<ForumResponse> forumresponses = new List<ForumResponse>();

            if (forum != null)
            {
                forumresponses = ForumFeedbackResponseService.GetByForumIdAndType(forum.Id, entityType, DomainObjectType.ForumResponse);
            }

            ViewData["Feedbacks"] = forumresponses;
        }

        #endregion
    }
}
