using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using System.Configuration;
using Compelligence.Common.Utility;
using Compelligence.Common.Utility.Web;
using Compelligence.Web.Models.Web;
using System.Text;
using Compelligence.Util.Common;
using System.Threading;
using System.Net;
using Compelligence.Web.Models.Util;
using System.Web.Configuration;
using Compelligence.Util.Validation;

namespace Compelligence.Web.Controllers
{
    public class ConfigurationLabelsController : BackEndAsyncFormController<ConfigurationLabels, decimal>
    {
        #region Member Variables

        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;

        #endregion

        #region Public Properties

        public IConfigurationLabelsService ConfigurationLabelsService
        {
            get { return (IConfigurationLabelsService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }
        
        #endregion

        #region Action Methods

       #endregion

        #region Validation Methods

        protected override bool ValidateFormData(ConfigurationLabels entity, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(entity.Value))
            {
                ValidationDictionary.AddError("Value", LabelResource.ConfigurationLabelsValueRequiredError);
            }
            return ValidationDictionary.IsValid;
        }

        #endregion
                
        #region Override Methods

        public  ActionResult IndexLabels(string Scope, string Container)
        {
            ViewData["Scope"] = Scope;
            ViewData["Container"] = Container;
            IList<ResourceObject> statusList = ResourceService.GetAll<ConfigurationLabesStatus>();
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            //check Labels
            SetLabels();  

            return View("Index");
        }

        public ActionResult List()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            return View("List");
        }

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> statusList = ResourceService.GetAll<ConfigurationLabesStatus>();
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");

        }

        protected override void SetEntityDataToForm(ConfigurationLabels entity)
        {
            base.SetEntityDataToForm(entity);
        }
        
        protected override void LoadFormData()
        {
            SetLabels();
        }
        
        #endregion


        #region Public Methods

        #endregion


    }
}
