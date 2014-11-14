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
    public class FollowerController : BackEndAsyncFormController <Follower,decimal>
    {
        public IFollowerService FollowerService
        {
            get { return (IFollowerService)_genericService; }
            set { _genericService = value; }
        }
      

    }
}
