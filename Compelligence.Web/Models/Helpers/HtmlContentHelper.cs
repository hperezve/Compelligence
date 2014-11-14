using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Compelligence.BusinessLogic.Interface;
using Spring.Context.Support;
using Compelligence.Domain.Entity.Resource;

namespace Compelligence.Web.Models.Helpers
{
    public static class HtmlContentHelper
    {
        public static IResourceService ResourceService = (IResourceService)ContextRegistry.GetContext().GetObject("ResourceService", typeof(IResourceService));

        public static string GetApprovalStatus(object value)
        {
            string projectStatus = string.Empty;

            if (value != null)
            {
                projectStatus = ResourceService.GetName<ApprovalListApproveStatus>(value.ToString());
            }

            return projectStatus;
        }

    }
}
