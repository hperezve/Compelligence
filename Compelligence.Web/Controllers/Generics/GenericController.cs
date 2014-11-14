using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Common.Validation;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Web.Attributes;
using Spring.Context;
using Compelligence.BusinessLogic.Interface;
using Spring.Context.Support;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Domain.Entity;
using System.Configuration;
using Compelligence.Common.Utility;
using Compelligence.Security.Managers;
using Compelligence.Util.Common;
using Compelligence.Common.Utility.Upload;

namespace Compelligence.Web.Controllers
{
    [HandleSystemError]
    public abstract class GenericController : Controller
    {
        private IApplicationContext _appcontext;

        private IValidationDictionary _validationDictionary;

        private IActionHistoryService _actionHistoryService;

        private ILibraryTypeService _genericLibraryTypeService;

        private IConfigurationLabelsService _configurationLabelsService;

        private IConfigurationDefaultsService _configurationDefaultsService;

        private IConfigurationUserTypeService _configurationUserTypeService;

        public GenericController()
        {
            this._validationDictionary = new ModelStateWrapper(ModelState);

            this._appcontext = ContextRegistry.GetContext();

            this._actionHistoryService = AppContext.GetObject("ActionHistoryService") as IActionHistoryService;

            this._genericLibraryTypeService = AppContext.GetObject("LibraryTypeService") as ILibraryTypeService;

            this._configurationLabelsService = AppContext.GetObject("ConfigurationLabelsService") as IConfigurationLabelsService;

            this._configurationDefaultsService = AppContext.GetObject("ConfigurationDefaultsService") as IConfigurationDefaultsService;

            this._configurationUserTypeService = AppContext.GetObject("ConfigurationUserTypeService") as IConfigurationUserTypeService;
        }

        public IApplicationContext AppContext
        {
            get { return _appcontext; }
        }

        protected IValidationDictionary ValidationDictionary
        {
            get { return _validationDictionary; }
        }

        protected IActionHistoryService ActionHistoryService
        {
            get { return _actionHistoryService; }
        }

        protected ILibraryTypeService GenericLibraryTypeService
        {
            get { return _genericLibraryTypeService; }
            set { _genericLibraryTypeService = value; }
        }

        public IConfigurationLabelsService ConfigurationLabelsService 
        {
            get { return _configurationLabelsService; }
            set { _configurationLabelsService = value; }
        }

        public IConfigurationDefaultsService ConfigurationDefaultsService
        {
            get { return _configurationDefaultsService; }
        }
        public IConfigurationUserTypeService ConfigurationUserTypeService
        {
            get { return _configurationUserTypeService; }
            set { _configurationUserTypeService = value; }
        }
        public virtual string CurrentUser
        {
            get { return StringUtility.CheckNull(Session["UserId"] as string); }
        }

        public virtual string CurrentCompany
        {
            get { return StringUtility.CheckNull(Session["ClientCompany"] as string); }
        }

        public virtual string CurrentSecurityGroup
        {
            get { return StringUtility.CheckNull(Session["SecurityGroupId"] as string); }
        }
        
        protected virtual void ActionHistory(decimal entityId, string entityAction, string entityType) { }

        protected IList<Library> GetLibrariesForEntity(string entityType, string libraryTypeKeyCode)
        {
            return GetLibrariesForEntity(0, entityType, libraryTypeKeyCode);
        }


        /// <summary>
        /// Get libraries for entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <param name="libraryTypeKeyCode"></param>
        /// <returns></returns>
        protected IList<Library> GetLibrariesForEntity(decimal entityId, string entityType, string libraryTypeKeyCode)
        {
            return GetLibrariesForEntity(entityId, entityType, libraryTypeKeyCode, CurrentUser, CurrentCompany);
        }

        protected IList<Library> GetLibrariesForEntity(decimal entityId, string entityType, string libraryTypeKeyCode, string CurrentUser,string CurrentCompany)
        {
            IList<Library> libraries = new List<Library>();
            LibraryType libraryType = null;
            string pathtemp = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["TempFilePath"];

            if (!string.IsNullOrEmpty(libraryTypeKeyCode))
            {
                libraryType = GenericLibraryTypeService.GetByKeyCode(libraryTypeKeyCode, CurrentCompany);
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;

                if (hpf.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(hpf.FileName);
                    string physicalname = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                    Library library = new Library();

                    physicalname += "_" + fileName.Replace(' ', '_');
                    try
                    {
                        hpf.SaveAs(System.IO.Path.Combine(pathtemp, physicalname));
                        library.StatusUpload = LibraryStatusUpload.Successful;
                    }
                    catch (Exception exp)
                    {
                        library.StatusUpload = LibraryStatusUpload.Unsuccessful;
                    }

                    library.Name = fileName;
                    library.FileName = fileName;
                    library.FilePhysicalName = physicalname;

                    if (!UserManager.GetInstance().IsEndUser(CurrentUser))
                    {
                        UserProfile author = UserManager.GetInstance().GetUserProfile(CurrentUser);

                        library.Author = author.Name;
                    }

                    if (libraryType != null)
                    {
                        library.LibraryTypeId = libraryType.Id;
                    }

                    library.HeaderType = entityType;
                    library.EntityId = entityId;
                    library.EntityType = entityType;
                    library.AssignedTo = CurrentUser;
                    libraries.Add(library);
                }
            }

            return libraries;
        }

        protected void GetDownloadFileResponse(string path, string physicalName, string fileName)
        {
            Response.ContentType = FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.Clear();
            Response.WriteFile("~\\" + path + physicalName);
            Response.End();
        }

        protected void GetDownloadFileResponseByExtension(string path, string physicalName, string fileName, string extension)
        {
            Response.Clear();
            Response.ContentType = FileUtility.GetMimeType("~\\" + path + physicalName + extension);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            
            Response.WriteFile("~\\" + path + physicalName);
            Response.End();
        }

        protected void GetDownloadFileResponseByContentType(string path, string physicalName, string fileName, string contentType)
        {
            Response.ContentType = contentType;
            Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
            Response.Clear();
            Response.WriteFile("~\\" + path + physicalName);
            Response.End();
        }
        protected void GetDownloadFileWithSpaceResponse(string path, string physicalName, string fileName)
        {
            Response.ContentType = FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
            Response.Clear();
            Response.WriteFile("~\\" + path + physicalName);
            Response.End();
        }
        /// <summary>
        /// Set in the new entity object data does not considered in web form
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="collection"></param>
        protected virtual void SetFormDataToEntity<U>(U entity, FormCollection collection)
        {
            string[] formKeys = collection.AllKeys;
            Type entityType = typeof(U);
            object entityObject = (object)entity;

            foreach (string formKey in formKeys)
            {
                if (!formKey.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    TypeUtility.SetPropertyValue(entityType, entityObject, formKey, collection[formKey]);
                }
            }
        }

        public string GetFilePath(FileUploadType type)
        {
            string filePath;

            switch (type)
            {
                case FileUploadType.Template:
                    filePath = ConfigurationSettings.AppSettings["TemplateFilePath"];
                    break;
                case FileUploadType.Image:
                    filePath = ConfigurationSettings.AppSettings["ImageFilePath"];
                    break;
                default:
                    filePath = ConfigurationSettings.AppSettings["ContentFilePath"];
                    break;
            }

            return filePath;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Write to Log
            base.OnException(filterContext);
        }

        public void SetLabels()// it was by default
        {
            SetLabels(CurrentUser, CurrentCompany); 
        }
        public void SetLabels(string CurrentUser, string CurrentCompany) //override for work from Salesforce
        {
            //Labels
            ConfigurationLabels ProductLabel = new ConfigurationLabels();
            ConfigurationLabels IndustryLabel = new ConfigurationLabels();
            ConfigurationLabels CompetitorLabel = new ConfigurationLabels();
            ProductLabel = ConfigurationLabelsService.GetByName("Product", CurrentCompany);
            IndustryLabel = ConfigurationLabelsService.GetByName("Industry", CurrentCompany);
            CompetitorLabel = ConfigurationLabelsService.GetByName("Competitor", CurrentCompany);
            //defaults
            String dProductLabel = "Product";
            String dIndustryLabel = "Industry";
            String dCompetitorLabel = "Competitor";

            if (IndustryLabel != null)
            {
                if (IndustryLabel.Status.Equals(ConfigurationLabesStatus.Enable))
                    dIndustryLabel = IndustryLabel.Value;
            }
            else
            {
                IndustryLabel = new ConfigurationLabels();
                IndustryLabel.Name = "Industry";
                IndustryLabel.Value = dIndustryLabel;
                IndustryLabel.Description = "Label for " + dIndustryLabel;
                IndustryLabel.Status = ConfigurationLabesStatus.Enable;
                IndustryLabel.CreatedBy = CurrentUser;
                IndustryLabel.CreatedDate = DateTime.Now;
                IndustryLabel.LastChangedBy = CurrentUser;
                IndustryLabel.LastChangedDate = DateTime.Now;
                IndustryLabel.ClientCompany = CurrentCompany;

                ConfigurationLabelsService.Save(IndustryLabel);
            }

            if (CompetitorLabel != null)
            {
                if (CompetitorLabel.Status.Equals(ConfigurationLabesStatus.Enable))
                    dCompetitorLabel = CompetitorLabel.Value;
            }
            else
            {
                CompetitorLabel = new ConfigurationLabels();
                CompetitorLabel.Name = "Competitor";
                CompetitorLabel.Value = dCompetitorLabel;
                CompetitorLabel.Description = "Label for " + dCompetitorLabel;
                CompetitorLabel.Status = ConfigurationLabesStatus.Enable;
                CompetitorLabel.CreatedBy = CurrentUser;
                CompetitorLabel.CreatedDate = DateTime.Now;
                CompetitorLabel.LastChangedBy = CurrentUser;
                CompetitorLabel.LastChangedDate = DateTime.Now;
                CompetitorLabel.ClientCompany = CurrentCompany;

                ConfigurationLabelsService.Save(CompetitorLabel);
            }

            if (ProductLabel != null)
            {
                if (ProductLabel.Status.Equals(ConfigurationLabesStatus.Enable))
                    dProductLabel = ProductLabel.Value;
            }
            else
            {
                ProductLabel = new ConfigurationLabels();
                ProductLabel.Name = "Product";
                ProductLabel.Value = dProductLabel;
                ProductLabel.Description = "Label for " + dProductLabel;
                ProductLabel.Status = ConfigurationLabesStatus.Enable;
                ProductLabel.CreatedBy = CurrentUser;
                ProductLabel.CreatedDate = DateTime.Now;
                ProductLabel.LastChangedBy = CurrentUser;
                ProductLabel.LastChangedDate = DateTime.Now;
                ProductLabel.ClientCompany = CurrentCompany;

                ConfigurationLabelsService.Save(ProductLabel);
            }

            ViewData["IndustryLabel"] = dIndustryLabel;
            ViewData["CompetitorLabel"] = dCompetitorLabel;
            ViewData["ProductLabel"] = dProductLabel;
            ViewData["Comparinator"] = "Comparinator";
            ViewData["Content"] = "Content";
            ConfigurationUserType cut = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(CurrentSecurityGroup, CurrentCompany);
            if (cut != null)
            {
                ViewData["Comparinator"] = cut.Comparinator;
                ViewData["Content"] = cut.Content;
            }
        }

        protected virtual void GetDataOfConfiguration(string clientCompanyId)
        {
            String DefaultsDisabPublComm = "false";
            string value = string.Empty;
            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(clientCompanyId);
            if (configurations != null)
            {
                if (configurations.Count > 0)
                {
                    DefaultsDisabPublComm = configurations[0].DisabledPublicComment;
                    if (configurations[0].DisabledPublicComment.Equals("true") || configurations[0].DisabledPublicComment.Equals("True") || configurations[0].DisabledPublicComment.Equals("TRUE"))
                    {
                        value = "hiddentab";
                    }
                }
            }
            ViewData["DefaultsDisabPublComm"] = DefaultsDisabPublComm;
            ViewData["ClsHiddenTab"] = value;
        }

        /// <summary>
        /// This Method will set in ViewData the properties to Help dialog
        /// </summary>
        /// <param name="editHelp">true/false</param>
        /// <param name="actionFrom">FrontEnd/BackEnd</param>
        protected virtual void SetDataSectionToHelp( string actionFrom)
        {
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = actionFrom;
        }

        /// <summary>
        /// This Method will set in ViewData the vaoues to HelpDialog and use SetDataSectionToHelp Method
        /// </summary>
        /// <param name="editHelp">true/false</param>
        /// <param name="actionFrom">FrontEnd/BackEnd</param>
        /// <param name="entity">data of domainObjectType or FrontEndPages</param>
        /// <param name="titleHelp">title</param>
        protected virtual void SetDataToHelp(string actionFrom, string entity, string titleHelp)
        {
            SetDataSectionToHelp(actionFrom);
            ViewData["Entity"] = entity;
            ViewData["TitleHelp"] = titleHelp;
        }


        /// <summary>
        /// This method will set in ViewData  the values to Entity and Title
        /// this method should be override in each controller on front end
        /// </summary>
        protected virtual void SetDefaultDataByPage()
        {
            ViewData["Entity"] = string.Empty;
            ViewData["TitleHelp"] = string.Empty;
        }
    }
}
