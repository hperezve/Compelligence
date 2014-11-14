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
    public class HelpController : BackEndFormController<ClientCompanyHelp, decimal>
    {
        private IClientCompanyHelpService _clientCompanyHelpService;

        public IClientCompanyHelpService ClientCompanyHelpService
        {
            get { return _clientCompanyHelpService; }
            set { _clientCompanyHelpService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public ContentResult GetHelp(string entityType, string actionFrom)
        {
            string type = entityType;
            //if (actionFrom.Equals(ActionFrom.BackEnd))
            //{
            //    type=ResourceService.GetValue<DomainObjectType>(entityType);
            //}
            //else {
            //    type = entityType;
            //}
            string result = string.Empty;
            ///to future this should change to get ClientCompanyHelp by Client Company, the next line will get the 
            ///ClientCompanyHelp by CurrentCompany
            //ClientCompanyHelp clientCompanyHelp = ClientCompanyHelpService.GetByEntityType(type,CurrentCompany);

            // by the moment we will use the same ClientCompanyHelp to ALL COMPANIES AND THE SECOND PARAMETER IS complligece
            ClientCompanyHelp clientCompanyHelp = ClientCompanyHelpService.GetByEntityType(type,actionFrom, "compelligence");
            if (clientCompanyHelp == null)
            {
                clientCompanyHelp = new ClientCompanyHelp();
                clientCompanyHelp.EntityType = type;
                clientCompanyHelp.ActionFrom = actionFrom;
                clientCompanyHelp.Subject = string.Empty;
                clientCompanyHelp.Content = string.Empty;
                /// THIS METHOD WILL SET THE VALUES TO COMPANY
                //SetDefaultDataForSave(clientCompanyHelp);

                /// BY THE MOMENT WE WILL SET IN USERID=SYSADMIN AND CLIENTCOMPANY=compelligence
                ///AUDIT FIELDS
                clientCompanyHelp.CreatedBy = "SYSADMIN";
                clientCompanyHelp.CreatedDate = DateTime.Now;
                clientCompanyHelp.LastChangedBy = "SYSADMIN";
                clientCompanyHelp.LastChangedDate = DateTime.Now;
                clientCompanyHelp.ClientCompany = "compelligence";
                ClientCompanyHelpService.Save(clientCompanyHelp);
                result = clientCompanyHelp.Subject + "[TOKEN]" + clientCompanyHelp.Content;
            }
            result = clientCompanyHelp.Subject + "[TOKEN]" + clientCompanyHelp.Content;
            return Content(result);
        }

        public ActionResult Update()
        {
            string scope = Request.Params["scope"];
            string entity = Request.Params["entity"];
            string actionFrom = Request.Params["actionFrom"];
            string subject = Request.Params["subject"];
            string content = Request.Params["content"];
            string type = string.Empty;
            type = entity;
            //if (actionFrom.Equals(ActionFrom.BackEnd))
            //{
            //    type = ResourceService.GetValue<DomainObjectType>(entity);
            //}
            //else
            //{
            //    type = entity;
            //}

            ///to future this should change to get ClientCompanyHelp by Client Company, the next line will get the 
            ///ClientCompanyHelp by CurrentCompany
            //ClientCompanyHelp clientCompanyHelp = ClientCompanyHelpService.GetByEntityType(type,CurrentCompany);

            // by the moment we will use the same ClientCompanyHelp to ALL COMPANIES AND THE SECOND PARAMETER IS complligece
            ClientCompanyHelp clientCompanyHelp = ClientCompanyHelpService.GetByEntityType(type, actionFrom, "compelligence");
            if (clientCompanyHelp != null)
            {
                //clientCompanyHelp.EntityType = type;
                clientCompanyHelp.Subject = subject;
                clientCompanyHelp.Content = content;
                /// THIS METHOD WILL SET THE VALUES TO COMPANY
                //SetDefaultDataForUpdate(clientCompanyHelp);

                /// BY THE MOMENT WE WILL SET IN USERID=SYSADMIN AND CLIENTCOMPANY=compelligence
                clientCompanyHelp.LastChangedDate = DateTime.Now;
                ClientCompanyHelpService.Update(clientCompanyHelp);
            }
            return Content("");
        }
    }
}
