using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compelligence.Security.Filters;
using Compelligence.Web.Controllers;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity;
using Resources;

namespace Compelligence.Web.Controllers
{
    [AuthenticationFilter]
    public abstract class GenericBackEndController : GenericController
    {

        //protected override ViewResult View(string viewName, string masterName, object model)
        //{
        //    //if (IsAjaxRequest)
        //    //    return base.View(viewName, "Empty", model);

        //    return base.View(viewName, masterName, model);
        //}

        //protected virtual bool IsAjaxRequest
        //{
        //    //Both Prototype.js and jQuery send 
        //    // the X-Requested-With header in Ajax calls
        //    get
        //    {
        //        var request = ControllerContext.HttpContext.Request;
        //        return (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        //    }
        //}

        protected string GetFormatDate()
        {
            return "MM/dd/yyyy";
        }

        /// <summary>
        /// Register ActionHistory Need 
        /// </summary>
        //protected  void ActionHistory(ActionHistory action) 
        protected override void ActionHistory(decimal Id, string entityAction, string domainObjectType)
        {
            string actionFrom = "BackEnd";
            ActionHistory action = new ActionHistory(Id, entityAction, domainObjectType, actionFrom);
            string RealObjectType = this.GetType().ToString();

            action.CreatedBy = CurrentUser;
            action.ClientCompany = CurrentCompany;
            action.Description = string.Format(MessagesResource.ActionHistorySmall, CurrentUser, action.EntityAction, action.EntityType);

            ActionHistoryService.Register(action);
        }

        /// <summary>
        /// This method will set in ViewData the values , by defualt session and action
        /// and use the SetDefaultDataByPage Method to set the other values
        /// this method should be use in all Index
        /// </summary>
        protected virtual void SetDefaultDataToLoadPage()
        {
            SetDataSectionToHelp(Compelligence.Domain.Entity.Resource.ActionFrom.BackEnd);
            SetDefaultDataByPage();
        }
        /// <summary>
        /// THis method will use SetDataToHelp method to set values
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="titleHelp"></param>
        protected virtual void SetDefatultEntityToLoadPage(string entity, string titleHelp)
        {
            SetDataToHelp(Compelligence.Domain.Entity.Resource.ActionFrom.BackEnd, entity, titleHelp);
        }


        protected virtual void SetEntityHelpDataToBackEnd()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            SetGeneralHelpDataToBackEnd();
        }

        protected virtual void SetGeneralHelpDataToBackEnd()
        {
            SetDataSectionToHelp(Compelligence.Domain.Entity.Resource.ActionFrom.BackEnd);
        }
    }
}
