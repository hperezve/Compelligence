using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;

namespace Compelligence.Web.Controllers
{
    public class UserRelationController : BackEndAsyncFormController<UserRelation, decimal>
    {
        public IUserRelationService UserRelationService
        {
            get { return (IUserRelationService)_genericService; }
            set { _genericService = value; }
        }
              

    }
}
