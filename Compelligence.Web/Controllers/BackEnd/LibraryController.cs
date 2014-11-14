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
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class LibraryController : BackEndAsyncFormController<Library, decimal>
    {
        #region Member Variables

        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;

        #endregion

        #region Public Properties

        public ILibraryService LibraryService
        {
            get { return (ILibraryService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public ILibraryTypeService LibraryTypeService { get; set; }

        public IFileService FileService { get; set; }

        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }

        public IProjectService ProjectService
        {
            get;
            set;
        }

        public IObjectiveService ObjectiveService
        { get; set; }

        public IKitService KitService
        { get; set; }

        public IEventService EventService
        { get; set; }

        public IDealService DealService
        { get; set; }

        public ITrendService TrendService
        { get; set; }

        public ICompetitorService CompetitorService
        {
            get;
            set;
        }

        public IProductService ProductService
        { get; set; }

        public IContentTypeService ContentTypeService
        { get; set; }
        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetDeletionDateByLibraryType(decimal id)
        {
            string result = string.Empty;
            DateTime currentDate = DateTime.Now;
            DateTime? deletionDate = LibraryService.GetDeletionDateByLibraryType(id, currentDate);

            if (deletionDate != null)
            {
                result = DateTimeUtility.ConvertToString(deletionDate.Value, GetFormatDate());
            }

            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile()
        {
            string fileNameResult = string.Empty;

            //Artifice for return Fail if file size is more than 20Mb, it works with Application_BeginRequest in Global.asax.cs + ajaxupload.js
            HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
            int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;//Approx 100 Kb(for page content) size
            if ((Request.ContentLength > maxRequestLength))
            {
                return Content("Fail");
            }
            //End-Artifice
            string fileName="";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                fileName = fileNameResult;
                if (hpf.ContentLength == 0)  continue;

                fileNameResult += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');
                fileName = System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_'); 
                hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"], fileNameResult));

            }
            Session["fileName"] = fileName;
            Session["fileNameResult"] = fileNameResult;
            return Content(fileNameResult);
         
        }



        //Sample how to upload using Raw; it don't work with IE
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFileRawMode()
        {
            string fileNameResult = string.Empty;
            fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();

            string FileName = Request.Params["HTTP_X_FILE_NAME"];

            if (FileName == null)
            {
                FileName = "name";
            }
           // HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
            //String fileName = FileUpload1.FileName;

            fileNameResult += "_" + System.IO.Path.GetFileName(FileName).Replace(' ', '_');
            System.IO.FileStream RawFile = new System.IO.FileStream(System.IO.Path.Combine(ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"], fileNameResult), System.IO.FileMode.OpenOrCreate);
            byte[] buffer = new byte[1024];
            int ibytes;
            
            while ((ibytes = Request.InputStream.Read(buffer, 0, buffer.Length))>0 ) 
            {
                RawFile.Write(buffer, 0, ibytes);
            }
            Session["fileName"] = FileName;
            Session["fileNameResult"] = fileNameResult;
            RawFile.Close();
            
            return Content("{result:'"+fileNameResult+"'}");

        }
        


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadSingleFile()
        {
            string fileNameResult = string.Empty;

            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();

                    if (hpf.ContentLength == 0) continue;

                    fileNameResult += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');
                    hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"], fileNameResult));

                }
            }
            catch
            {
                return Content("Fail");
            }
            return Content(fileNameResult);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult UploadFile(string error)
        {
            string raction = Request["action"];
            if (raction.Equals("exception"))
            {
                return Content("Fail");//Content(Resources.LabelResource.LibraryUploadFileSizeError);
            }
            return Content(string.Empty);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult UploadSingleFile(string error)
        {
            string raction = Request["action"];
            if ( raction.Equals("exception") )
            {
                return Content("Fail");//Content(Resources.LabelResource.LibraryUploadFileSizeError);
            }
            return Content(string.Empty);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowFile(decimal id)
        {
            File file = FileService.GetByEntity(id, DomainObjectType.Library);

            return View(file);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowFileById(decimal id)
        {
            File file = FileService.GetById(id);
            return View("ShowFile",file);
        }

        public ActionResult DownloadExecute(decimal id)
        {
            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string check = StringUtility.CheckNull(Request["chk"]);

            File file = FileService.GetByEntityId(id, DomainObjectType.Library);
            if (file == null)
                return Content("NotFound");

            fullpath += file.PhysicalName;

            if (check.ToLower().Equals("true"))
            {
                if ((file != null) && System.IO.File.Exists(fullpath))
                    return Content("Found");
                else
                    return Content("NotFound");
            }
            else
            {
                GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
            }

            return Content(string.Empty);
        }
        public JsonResult DownloadRemote(String remotefilename)
        {
            string path = ConfigurationSettings.AppSettings["TempFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string localfilename=System.IO.Path.GetFileName(remotefilename);
            string localfilephysicalname =UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString()+"_"+localfilename;
            WebClient web = new WebClient();
            try
            {
                if (!Validator.IsBlankOrNull(localfilename) && localfilephysicalname.LastIndexOf('.') > 0)
                {
                    web.DownloadFile(remotefilename, fullpath + localfilephysicalname);
                }
                else
                {
                    return Json(new { Result = "Fail", FileName = "", FilePhysicalName = "" });
                }
                
            }
            catch
            {
                return Json(new {Result="Fail",FileName="",FilePhysicalName=""});
            }
            Session["fileNameResult"] = localfilephysicalname;
            Session["fileName"] = localfilename;
            return Json(new { Result = "Success", FileName = localfilename, FilePhysicalName = localfilephysicalname });
        }
              

        public JsonResult GetName(decimal id)
        {
            string key = Request["key"];
            Library library = LibraryService.GetById(id);
            library.DateInNewsletter = DateTime.Now;
            library.AddToNewsletter = "N";            
            LibraryService.Update(library);

            return Json(new
            {
                Name = StringUtility.CheckNull(library.Name).ToString(),
                Link = StringUtility.CheckNull(library.Link).ToString(),
                Description = StringUtility.CheckNull(library.Description)
            });
        }


        public ActionResult Entities()
        {

            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["BrowseDetailName"] = Request["BrowseDetailName"];
            ViewData["BrowseDetailFilter"] = Request["BrowseDetailFilter"];
            ViewData["UserSecurityAccess"] = Request["UserSecurityAccess"];
            ViewData["EntityLocked"] = Request["EntityLocked"];

            return View();
        }


        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Library library, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(library.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.LibraryNameRequiredError);
            }

            if (Validator.IsBlankOrNull(library.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.LibraryAssignedToRequiredError);
            }

            string score = formCollection["Score"];
            if (!score.Equals(""))
            {
                if (!Validator.IsDecimal(library.Score.ToString()))
                {
                    ValidationDictionary.AddError("Score", LabelResource.LibraryScoreTypeError);
                }
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];
            ViewData["RelatedName"] = string.Empty;
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<LibraryType> libraryTypeList = LibraryTypeService.GetAllSortByClientCompany("Name", clientCompany);
            IList<Library> libraryRelatedList = LibraryService.GetAllSortByClientCompany("Name", clientCompany);
            IList<ResourceObject> libraryPermanentList = ResourceService.GetAll<LibraryPermanent>();
            IList<ResourceObject> newsObjectTypeList = ResourceService.GetAll<NewsObjectType>();

            ViewData["LibraryTypeList"] = new SelectList(libraryTypeList, "Id", "Name");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["LibraryRelatedList"] = new SelectList(libraryRelatedList, "Id", "Name");
            ViewData["PermanentList"] = new SelectList(libraryPermanentList, "Id", "Value", LibraryPermanent.No);
            ViewData["MetaDataList"] = new SelectList(new List<string>());
            ViewData["NewsObjectTypeList"] = new SelectList(newsObjectTypeList, "Id", "Value");
            ViewData["DateAddedFrm"] = DateTimeUtility.ConvertToString(DateTime.Now, GetFormatDate());
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;
            ViewData["FileList"] = string.Empty;
            Session["fileNameResult"] = string.Empty;
        }

        protected override void SetEntityDataToForm(Library library)
        {
            ViewData["DateAddedFrm"] = DateTimeUtility.ConvertToString(library.DateAdded, GetFormatDate());
            ViewData["DateDeletionFrm"] = DateTimeUtility.ConvertToString(library.DateDeletion, GetFormatDate());
            ViewData["CreatedByFrm"] = StringUtility.CheckNull(library.CreatedBy).Length == 0 ? string.Empty : UserProfileService.GetById(library.CreatedBy).Name;
            ViewData["RelatedName"] = string.Empty;
            ViewData["LibraryIdHidden"] = library.Id;
            Session["fileNameResult"] = StringUtility.CheckNull(library.FilePhysicalName);
            Session["fileName"] = StringUtility.CheckNull(library.FileName).Replace(' ', '_');
            if (library.RelatedId != null)
            {
                if (StringUtility.CheckNull(library.RelatedType).Equals(NewsObjectType.Deal))
                {
                    Deal deal = DealService.GetById((decimal)library.RelatedId);
                    if (deal != null)
                    {
                        ViewData["RelatedName"] = deal.Name;
                    }
                }
                else if (StringUtility.CheckNull(library.RelatedType).Equals(NewsObjectType.Event))
                {
                    Event eventEntity = EventService.GetById((decimal)library.RelatedId);
                    if (eventEntity != null)
                    {
                        ViewData["RelatedName"] = eventEntity.EventName;
                    }
                }
                else if (StringUtility.CheckNull(library.RelatedType).Equals(NewsObjectType.Kit))
                {
                    Kit kit = KitService.GetById((decimal)library.RelatedId);
                    if (kit != null)
                    {
                        ViewData["RelatedName"] = kit.Name;
                    }
                }
                else if (StringUtility.CheckNull(library.RelatedType).Equals(NewsObjectType.Objective))
                {
                    Objective objective = ObjectiveService.GetById((decimal)library.RelatedId);
                    if (objective != null)
                    {
                        ViewData["RelatedName"] = objective.Name;
                    }
                }
                else if (StringUtility.CheckNull(library.RelatedType).Equals(NewsObjectType.Project))
                {
                    Project project = ProjectService.GetById((decimal)library.RelatedId);
                    if (project != null)
                    {
                        ViewData["RelatedName"] = project.Name;
                    }
                }
                else if (StringUtility.CheckNull(library.RelatedType).Equals(NewsObjectType.Trend))
                {
                    Trend trend = TrendService.GetById((decimal)library.RelatedId);
                    if (trend != null)
                    {
                        ViewData["RelatedName"] = trend.Name;
                    }
                }
            }
            SetFileAttachmentsInMail(library);
            library.OldDescription = library.Description;
            SetActionVia(library);
        }

        protected override void GetFormData(Library library, FormCollection collection)
        {
            library.DateAdded = DateTimeUtility.ConvertToDate(collection["DateAddedFrm"], GetFormatDate());
            library.DateDeletion = DateTimeUtility.ConvertToDate(collection["DateDeletionFrm"], GetFormatDate());
        }

        protected override void SetUserSecurityAccess(Library library)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (LibraryService.HasAccessToLibrary(library, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Library;

            switch (detailType)
            {
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.Id", parentId.ToString());
                    childController = "Library::Nugget";
                    break;
                case DetailType.EntityRelation: //Or another
                    AddFilter(detailFilter, "Library.Id", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityLibraryAllView.LibraryId", parentId.ToString());

                    childController = "Library::Entities";
                    break;

            }

            return childController;
        }

        #endregion

        public ContentResult CreateNewObjectByNews(string entityid, string entitytype, string link)
        {
            string result = string.Empty;
            decimal idNewObject = 0;
            if (!string.IsNullOrEmpty(entityid))
            {
                decimal id = decimal.Parse(entityid);
                Library library = LibraryService.GetById(id);
                if (library != null)
                {
                    if (entitytype.Equals(NewsObjectType.Deal))
                    {
                        idNewObject = CreateNewDealAndGetId(library);
                    }
                    else if (entitytype.Equals(NewsObjectType.Event))
                    {
                        idNewObject = CreateNewEventAndGetId(library);
                    }
                    else if (entitytype.Equals(NewsObjectType.Kit))
                    {
                        idNewObject = CreateNewKitAndGetId(library);
                    }
                    else if (entitytype.Equals(NewsObjectType.Objective))
                    {
                        idNewObject = CreateNewObjectiveAndGetId(library);
                    }
                    else if (entitytype.Equals(NewsObjectType.Project))
                    {
                        idNewObject = CreateNewProjectAndGetId(library);
                    }
                    else if (entitytype.Equals(NewsObjectType.Trend))
                    {
                        idNewObject = CreateNewTrendAndGetId(library);
                    }
                }
            }
            if (idNewObject != 0)
            {
                return Content(idNewObject.ToString());
            }
            else
            {
                return Content(string.Empty);

            }
        }
        #region Private Methods
        private decimal CreateNewDealAndGetId(Library library)
        {
            Deal deal = new Deal();
            deal.Name = library.Name;
            //deal.Status = DealStatus.Enable;
            deal.AssignedTo = CurrentUser;
            deal.CreatedBy = CurrentUser;
            deal.LastChangedBy = CurrentUser;
            deal.CreatedDate = DateTime.Now;
            deal.LastChangedDate = DateTime.Now;
            deal.ClientCompany = library.ClientCompany;
            DealService.Save(deal);
            LibraryService.CreateEntityLibrary(library, deal.Id, DomainObjectType.Deal);
            //library.Related = deal.Id;
            library.RelatedId = deal.Id;
            library.Permanent = LibraryPermanent.Yes;
            library.RelatedType = DomainObjectType.Deal;
            LibraryService.Update(library);
            return deal.Id;
        }

        private decimal CreateNewEventAndGetId(Library library)
        {
            Event eventEntity = new Event();
            eventEntity.EventName = library.Name;
            eventEntity.Status = EventStatus.Enable;
            //eventEntity.Scenario = library.Description;
            eventEntity.Scenario = library.Name;
            eventEntity.Confidence = EventConfidence.Certain;
            eventEntity.TimeFrame = EventTypePeriod.SpecificDate;
            eventEntity.StartDate = DateTime.Now;
            eventEntity.Severity = EventSeverity.Medium; //moderate
            eventEntity.Url = library.Link;
            eventEntity.AssignedTo = CurrentUser;
            eventEntity.CreatedBy = CurrentUser;
            eventEntity.LastChangedBy = CurrentUser;
            eventEntity.CreatedDate = DateTime.Now;
            eventEntity.LastChangedDate = DateTime.Now;
            eventEntity.ClientCompany = library.ClientCompany;
            EventService.Save(eventEntity);
            LibraryService.CreateEntityLibrary(library, eventEntity.Id, DomainObjectType.Event);
            library.Permanent = LibraryPermanent.Yes;
            //library.Related = eventEntity.Id;
            library.RelatedId = eventEntity.Id;
            library.RelatedType = DomainObjectType.Event;
            LibraryService.Update(library);
            return eventEntity.Id;
        }

        private decimal CreateNewKitAndGetId(Library library)
        {
            Kit kit = new Kit();
            kit.Name = library.Name;
            kit.AssignedTo = CurrentUser;
            kit.CreatedBy = CurrentUser;
            kit.LastChangedBy = CurrentUser;
            kit.CreatedDate = DateTime.Now;
            kit.LastChangedDate = DateTime.Now;
            kit.ClientCompany = library.ClientCompany;
            KitService.Save(kit);
            LibraryService.CreateEntityLibrary(library, kit.Id, DomainObjectType.Kit);
            library.RelatedId = kit.Id;
            library.RelatedType = DomainObjectType.Kit;
            library.Permanent = LibraryPermanent.Yes;
            LibraryService.Update(library);
            return kit.Id;
        }

        private decimal CreateNewObjectiveAndGetId(Library library)
        {
            Objective objective = new Objective();
            objective.Name = library.Name;
            objective.AssignedTo = CurrentUser;
            objective.CreatedBy = CurrentUser;
            objective.LastChangedBy = CurrentUser;
            objective.CreatedDate = DateTime.Now;
            objective.LastChangedDate = DateTime.Now;
            objective.ClientCompany = library.ClientCompany;
            ObjectiveService.Save(objective);
            LibraryService.CreateEntityLibrary(library, objective.Id, DomainObjectType.Objective);
            library.RelatedId = objective.Id;
            library.RelatedType = DomainObjectType.Objective;
            library.Permanent = LibraryPermanent.Yes;
            LibraryService.Update(library);
            return objective.Id;
        }

        private decimal CreateNewProjectAndGetId(Library library)
        {
            Project project = new Project();
            project.Name = library.Name;
            project.Status = ProjectStatus.Proposed;
            project.Visibility = ProjectVisibility.Complete;
            project.Description = library.Description;
            project.AssignedTo = CurrentUser;
            project.CreatedBy = CurrentUser;
            project.LastChangedBy = CurrentUser;
            project.CreatedDate = DateTime.Now;
            project.LastChangedDate = DateTime.Now;
            project.ClientCompany = library.ClientCompany;

            ProjectService.Save(project);
            LibraryService.CreateEntityLibrary(library, project.Id, DomainObjectType.Project);

            library.Permanent = LibraryPermanent.Yes;
            //library.Related = project.Id;
            library.RelatedId = project.Id;
            library.RelatedType = DomainObjectType.Project;
            LibraryService.Update(library);
            return project.Id;
        }

        private decimal CreateNewTrendAndGetId(Library library)
        {
            Trend trend = new Trend();
            trend.Name = library.Name;
            trend.AssignedTo = CurrentUser;
            trend.CreatedBy = CurrentUser;
            trend.LastChangedBy = CurrentUser;
            trend.CreatedDate = DateTime.Now;
            trend.LastChangedDate = DateTime.Now;
            trend.ClientCompany = library.ClientCompany;
            TrendService.Save(trend);
            LibraryService.CreateEntityLibrary(library, trend.Id, DomainObjectType.Trend);
            library.RelatedId = trend.Id;
            library.RelatedType = DomainObjectType.Trend;
            library.Permanent = LibraryPermanent.Yes;
            LibraryService.Update(library);
            return trend.Id;
        }

        public void SetActionVia(Library library)
        {
            if (!string.IsNullOrEmpty(library.SubmittedVia) && !string.IsNullOrEmpty(library.SubmittedAction))
            {
                string[] parameters = library.SubmittedAction.Split('_');
                string name = string.Empty;
                string location = string.Empty;
                if (parameters.Length > 2)
                {
                    if (!string.IsNullOrEmpty(parameters[1]))
                    {
                        decimal id = decimal.Parse(parameters[2]);
                        if (parameters[1].Equals(DomainObjectType.Project))
                        {
                            Project project = ProjectService.GetById(id);
                            if (project != null)
                            {
                                name = " Project \"" + project.Name + "\"";
                                ContentType contentType = ContentTypeService.GetById((decimal)project.ContentTypeId);
                                if (contentType != null)
                                {
                                    name += " of the " + contentType.Name + " Content";
                                }
                            }
                        }
                        else if (parameters[1].Equals(DomainObjectType.Product))
                        {
                            Product product = ProductService.GetById(id);
                            if (product != null)
                            {
                                name = "Product \"" + product.Name + "\"";
                            }
                        }
                        else if (parameters[1].Equals(DomainObjectType.Competitor))
                        {
                            Competitor competitor = CompetitorService.GetById(id);
                            if (competitor != null)
                            {
                                name = " Competitor \"" + competitor.Name + "\"";
                                if (library.SubmittedVia.Equals(FeedBackSubmitedVia.StrengthWeakness))
                                {
                                    name += " of the " + FeedBackSubmitedVia.StrengthWeakness + " Content";
                                }
                            }
                        }
                        else if (parameters[1].Equals(DomainObjectType.Event))
                        {
                            Event eventEntity = EventService.GetById(id);
                            if (eventEntity != null)
                            {
                                name =" Event \""+ eventEntity.EventName + "\"" ;
                            }
                        }
                        else
                        { }

                       // location 

                        if (library.SubmittedVia.Equals(FeedBackSubmitedVia.StrengthWeakness))
                        {
                            location = FeedBackSubmitedVia.ContentPortal;
                        }
                        else if (library.SubmittedVia.Equals(FeedBackSubmitedVia.Features) || library.SubmittedVia.Equals(FeedBackSubmitedVia.Positioning) || library.SubmittedVia.Equals(FeedBackSubmitedVia.SalesTool) || library.SubmittedVia.Equals(FeedBackSubmitedVia.ProductInfo))
                        {
                            location=  library.SubmittedVia + " of Comparinator";
                        }
                        else
                        {
                            location =  library.SubmittedVia;
                        }
                    }
                }
                library.SubmittedViaFrm = string.Format(LibrarySubmittedVia.FillFeedBack, location, name); 
            }
        }
        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Library entity = LibraryService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }
        #endregion

        public ActionResult DownloadFileMailExecute(decimal id)
        {
            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            File file = FileService.GetById(id);
            if (file == null)
                return Content(string.Empty);
            fullpath += file.PhysicalName;

            if (System.IO.File.Exists(fullpath))
            {
                string mimeType = null;
                mimeType = FileUtility.GetMimeType("~\\" + path + file.PhysicalName);

                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + file.FileName.Replace(' ', '_'));
                Response.Clear();
                Response.WriteFile(fullpath);
                Response.End();
            }
            return Content(string.Empty);
        }

        private void SetFileAttachmentsInMail(Library library)
        {
            IList<File> fileList = FileService.GetFilesByEntityId(library.Id, DomainObjectType.LibraryMail, CurrentCompany);
            if (fileList != null && fileList.Count > 0)
            {
                ViewData["FileList"] = fileList;
            }
            Session["fileNameResult"] = library.FilePhysicalName;
        }

        protected override void SetFormEntityDataToForm(Library library)
        {
            SetFileAttachmentsInMail(library);
            library.OldDescription = library.Description;
            ModelState.SetModelValue("OldDescription", new ValueProviderResult(library.OldDescription, library.OldDescription, CultureInfo.InvariantCulture));
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckOut()
        {
            decimal id = (decimal)Session["IdLibrary"];
            File file = FileService.GetByEntity(id, DomainObjectType.Library);
            
            string userId = (string)Session["UserId"];
            //Compelligence.Domain.Entity.File file = new Compelligence.Domain.Entity.File();
            string mimeType = null;
           // Compelligence.Domain.Entity.File lastVersion = FileService.UpdateLastFileVersionToCheckOut(DecimalUtility.CheckNull(file.EntityId), file.EntityType);
            string filePath = string.Empty;
           
            filePath = ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"];
            string fileName = (string)Session["fileNameResult"];
            string path = filePath + fileName;
          
            string filenames = (string)Session["fileName"];
            if (System.IO.File.Exists(path))
            {
                mimeType = FileUtility.GetMimeType(filePath + fileName);
                fileName = (string)Session["fileNameResult"];
                
                path = filePath + fileName;
                if (System.IO.File.Exists(path))
                {
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + filenames);
                    Response.Clear();
                    Response.WriteFile(filePath + fileName);
                    Response.End();
                    return Content("File was not found");
                }              
            }
            else
            {
                filePath = ContextFilePath + ConfigurationSettings.AppSettings["LibraryFilePath"];
                fileName= file.PhysicalName;
                mimeType = FileUtility.GetMimeType(filePath + fileName);
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename=" + filenames);
                Response.Clear();
                Response.WriteFile(filePath + fileName);
                Response.End();
                return Content("File was not found");
            }
            return Content("File was not found");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckExistFile()
        {
            string From=Request.Params["FromIdCheck"];
            decimal id = Convert.ToDecimal(From);
            Session["IdLibrary"] = id;
            File file = FileService.GetByEntity(id, DomainObjectType.Library);
            string result = "File was not found";
            string filePath = string.Empty;
            
            filePath = ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"];
            string fileName = (string)Session["fileNameResult"];
            string path = filePath + fileName;
            if (System.IO.File.Exists(path))
            {
                path = filePath + fileName;
                if (System.IO.File.Exists(path))
                {
                    result = "Exist";
                }
            }
            else
            {
                filePath = ContextFilePath + ConfigurationSettings.AppSettings["LibraryFilePath"];
                if (file == null)
                {
                    return new JsonResult() { Data = result };
                }
                fileName = file.PhysicalName;
                path = filePath + fileName;
                if (System.IO.File.Exists(path))
                {
                    result = "Exist";
                }

            }
            return new JsonResult() { Data = result };
        }

    }
}