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

namespace Compelligence.Web.Controllers
{
    public class TeamMemberController : BackEndAsyncFormController<TeamMember, decimal>
    {

        #region Public Properties

        public ITeamMemberService TeamMemberService
        {
            get { return (ITeamMemberService)_genericService; }
            set { _genericService = value; }
        }

        public ITeamRoleService TeamRoleService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(TeamMember teamMember, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(teamMember.UserId))
            {
                ValidationDictionary.AddError("UserId", LabelResource.TeamMemberUserIdRequiredError);
            }

            if (Validator.IsBlankOrNull(teamMember.TeamRoleId))
            {
                ValidationDictionary.AddError("TeamRoleId", LabelResource.TeamMemberTeamRoleIdRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);

            ViewData["UserList"] = new SelectList(userList, "Id", "Name");


            if (IsDetailForm())
            {
                string detailFilterValue = GetDetailFilterValue("TeamMember.TeamId");
                IList<TeamRole> teamRoleList = new List<TeamRole>();

                if (!string.IsNullOrEmpty(detailFilterValue))
                {
                    teamRoleList = TeamRoleService.GetByTeamId(Convert.ToDecimal(detailFilterValue));
                }

                ViewData["TeamRoleList"] = new SelectList(teamRoleList, "Id", "Name");
            }

        }

        protected override void SetEntityDataToForm(TeamMember teamMember)
        {
            IList<TeamRole> teamRoleList = TeamRoleService.GetByTeamId((decimal)teamMember.TeamId);
            ViewData["TeamRoleList"] = new SelectList(teamRoleList, "Id", "Name");
        }

        #endregion

    }
}
