using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Resources;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using System.Text;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class TeamController : BackEndAsyncFormController<Team, decimal>
    {

        #region Public Properties

        public ITeamService TeamService
        {
            get { return (ITeamService)_genericService; }
            set { _genericService = value; }
        }

        public IIndustryService IndustryService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IProjectService ProjectService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Team team, FormCollection formCollection)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (Validator.IsBlankOrNull(team.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.TeamNameRequiredError);
            }

            if (Validator.IsBlankOrNull(team.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.TeamAssignedToRequiredError);
            }

            if (Validator.IsBlankOrNull(team.Status))
            {
                ValidationDictionary.AddError("Status", LabelResource.TeamStatusRequiredError);
            }

            if ((team.Status.Equals(TeamStatus.Disabled)) && (TeamService.HassDependences(team, clientCompany)))
            {
                ValidationDictionary.AddError("Status", LabelResource.TeamStatusValueError);
            }
            if (!string.IsNullOrEmpty(team.Email))
            {
                if (!Validator.IsEmail(team.Email))
                {
                    ValidationDictionary.AddError("Email", LabelResource.TeamEmailFormatError);
                }
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> statusList = ResourceService.GetAll<TeamStatus>();
            //IList<Project> projectList = ProjectService.GetAllByClientCompany(clientCompany);
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);

            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            //ViewData["ProjectIdList"] = new SelectList(projectList, "Id", "Name");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");

        }

        protected override void SetEntityDataToForm(Team team)
        {
            ViewData["StartDateFrm"] = DateTimeUtility.ConvertToString(team.StartDate, GetFormatDate());
        }

        protected override void GetFormData(Team team, FormCollection collection)
        {
            team.StartDate = DateTimeUtility.ConvertToDate(collection["StartDateFrm"], GetFormatDate());
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Team;

            switch (detailType)
            {
                case DetailType.TeamRole:
                    AddFilter(detailFilter, "TeamRole.TeamId", parentId.ToString());
                    AddFilter(browseDetailFilter, "TeamRoleDetailView.TeamId", parentId.ToString());
                    childController = "TeamRole";
                    break;
                case DetailType.TeamMember:
                    AddFilter(detailFilter, "TeamMember.TeamId", parentId.ToString());
                    AddFilter(detailFilter, "TeamMember.EntityType", DomainObjectType.TeamMember);
                    AddFilter(browseDetailFilter, "TeamMemberDetailView.TeamId", parentId.ToString());
                    childController = "TeamMember";
                    break;
                case DetailType.Project:
                    AddFilter(detailFilter, "Project.TeamId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProjectTeamDetailView.TeamId", parentId.ToString());
                    childController = "Project:ProjectTeamDetail";
                    break;
                case DetailType.Competitor:
                    AddFilter(detailFilter, "Competitor.TeamId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorTeamDetailView.TeamId", parentId.ToString());
                    childController = "Competitor:CompetitorTeamDetail";
                    break;
                case DetailType.Product:
                    AddFilter(detailFilter, "Product.TeamId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProductTeamDetailView.TeamId", parentId.ToString());
                    childController = "Product:ProductTeamDetail";
                    break;


                //case DetailType.Team:
                //    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                //    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Project);
                //    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                //    childController = "Team";
                //    break;


            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Team team)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (TeamService.HasAccessToTeam(team, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Team entity = TeamService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }
        #endregion
    }
}
