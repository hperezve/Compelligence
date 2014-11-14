using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Web;
using System.Text;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class TrendController : BackEndAsyncFormController<Trend, decimal>
    {
        #region Public Properties

        public ITrendService TrendService
        {
            get { return (ITrendService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }
        public IResourceService ResourceService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Trend trend, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(trend.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.TrendNameRequiredError);
            }

            if (Validator.IsBlankOrNull(trend.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.TrendAssignedToRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

         #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");

            IList<ResourceObject> typeList = ResourceService.GetAll<TrendType>();
            ViewData["TypeList"] = new SelectList(typeList, "Id", "Value");
            IList<ResourceObject> tierList = ResourceService.GetAll<TrendTier>();
            ViewData["TierList"] = new SelectList(tierList, "Id", "Value");
            IList<ResourceObject> industryTypeList = ResourceService.GetAll<TrendIndustryType>();
            ViewData["IndustryTypeList"] = new SelectList(industryTypeList, "Id", "Value");
            IList<ResourceObject> geoTypeList = ResourceService.GetAll<TrendGeoType>();
            ViewData["GeoTypeList"] = new SelectList(geoTypeList, "Id", "Value");
            IList<ResourceObject> forSwotList = ResourceService.GetAll<TrendForSwot>();
            ViewData["ForSwotList"] = new SelectList(forSwotList, "Id", "Value", TrendForSwot.OAndT);
            foreach (ResourceObject o in tierList)
            {
                o.Value = GeneralStringParser.InsertBlank(o.Value);
            }

        }

        protected override void SetEntityDataToForm(Trend trend)
        {
            ViewData["StartDateFrm"] = DateTimeUtility.ConvertToString(trend.StartDate, GetFormatDate());
            ViewData["EndDateFrm"] = DateTimeUtility.ConvertToString(trend.EndDate, GetFormatDate());
            ViewData["DateOfMaturityFrm"] = DateTimeUtility.ConvertToString(trend.DateOfMaturity, GetFormatDate());
        }

        protected override void SetFormEntityDataToForm(Trend trend)
        { 
            
        }

        protected override void SetUserSecurityAccess(Trend trend)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (TrendService.HasAccessToTrend(trend, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void GetFormData(Trend trend, FormCollection collection)
        {
            trend.StartDate = DateTimeUtility.ConvertToDate(collection["StartDateFrm"], GetFormatDate());
            trend.EndDate = DateTimeUtility.ConvertToDate(collection["EndDateFrm"], GetFormatDate());
            trend.DateOfMaturity = DateTimeUtility.ConvertToDate(collection["DateOfMaturityFrm"], GetFormatDate());
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Trend;

            switch (detailType)
            {
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Trend);
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Trend);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.Comment:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Trend);
                    childController = "ForumComment";
                    break;
               case DetailType.Industry:
                    AddFilter(detailFilter, "Industry.TrendId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "IndustryTrendDetailView.TrendId", parentId.ToString());
                    childController = "Industry:IndustryTrendDetail";
                    break;
                case DetailType.Competitor:
                    AddFilter(detailFilter, "Competitor.TrendId", parentId.ToString());
                    AddFilter(detailFilter, "Competitor.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Competitor.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "CompetitorTrendDetailView.TrendId", parentId.ToString());
                    childController = "Competitor:CompetitorTrendDetail";
                    break;
                case DetailType.Product:
                    AddFilter(detailFilter, "Product.TrendId", parentId.ToString());
                    AddFilter(detailFilter, "Product.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Product.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "ProductTrendDetailView.TrendId", parentId.ToString());
                    childController = "Product:ProductTrendDetail";
                    break;
                case DetailType.Event:
                    AddFilter(detailFilter, "Event.TrendId", parentId.ToString());
                    AddFilter(detailFilter, "Event.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "EntityEventDetailView.EntityId", parentId.ToString());
                    childController = "Event:EntityEventDetail";
                    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.News:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Trend);
                    //AddFilter(browseDetailFilter, "NewsTrendDetailView.EntityType", DomainObjectType.Trend);
                    AddFilter(browseDetailFilter, "NewsTrendDetailView.EntityId", parentId.ToString());
                    childController = "News:NewsTrendDetail";
                    break;
            }
            return childController;
        }

        protected override void SetDetailFormData()
        {
            ViewData["HasRows"] = false;
            string headerType = Request["HeaderType"];
            string XId = null;
            decimal count = 0;

            if (!string.IsNullOrEmpty(headerType))
            {
                if (headerType.Equals(DomainObjectType.Event))
                    XId = GetDetailFilterValue("Trend.EventId");

                if (!string.IsNullOrEmpty(XId))
                {
                    decimal xId = decimal.Parse(XId);

                    if (headerType.Equals(DomainObjectType.Event))
                    {
                        IList<TrendEventDetailView> xByEvent = TrendService.GetByEventIdAndClientCompany(xId, CurrentCompany);
                        if (xByEvent != null)
                        {
                            count = xByEvent.Count;
                        }
                    }
                    
                    if (count > 0)
                    {
                        ViewData["HasRows"] = true;
                    }
                }
            }
        }
 
        #endregion

        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Trend entity = TrendService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }
    }
}
