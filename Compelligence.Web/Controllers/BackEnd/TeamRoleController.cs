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
    public class TeamRoleController : BackEndAsyncFormController<TeamRole, decimal>
    {
        #region Public Properties

        public ITeamRoleService TeamRoleService
        {
            get { return (ITeamRoleService)_genericService; }
            set { _genericService = value; }
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(TeamRole teamRole, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(teamRole.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.TeamRoleNameRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

    }
}
