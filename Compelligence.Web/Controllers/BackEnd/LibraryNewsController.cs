using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Common.Utility;
using Compelligence.Util.Type;
using System.Configuration;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Common;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Domain.Entity;
using Compelligence.Web.Models.Web;
using System.Text;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class LibraryNewsController : BackEndAsyncFormController<Library, decimal>
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

        public IDealService DealService { get; set; }

        public IEventService EventService { get; set; }

        public IKitService KitService { get; set; }

        public IObjectiveService ObjectiveService { get; set; }

        public IProjectService ProjectService { get; set; }

        public ITrendService TrendService { get; set; }

        public IEmailService EmailService { get; set; }

        public IClientCompanyService ClientCompanyService { get; set; }

        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }

        public string appPath = System.Web.HttpContext.Current.Request.ApplicationPath.ToString();
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

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();

                if (hpf.ContentLength == 0)
                {
                    continue;
                }

                fileNameResult += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');

                hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"], fileNameResult));
            }

            return Content(fileNameResult);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowFile(decimal id)
        {
            File file = FileService.GetByEntity(id, DomainObjectType.Library);

            return View(file);
        }

        public ActionResult DownloadExecute(decimal id)
        {
            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            File file = FileService.GetByEntityId(id, DomainObjectType.Library);
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

                //return new DownloadResult { VirtualPath = "~\\" + path + file.PhysicalName, FileDownloadName = file.FileName.Replace(' ', '_') };
            }
            return Content(string.Empty);
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

        //public ActionResult Nugget()
        //{

        //    string Scope = Request["Scope"];
        //    string HeaderType = Request["HeaderType"];
        //    string DetailFilter = Request["DetailFilter"]; //Library.Id_Eq_19768202609375863
        //    string BrowseDetailName = Request["BrowseDetailName"];
        //    string BrowseDetailFilter = Request["BrowseDetailFilter"];
        //    string UserSecurityAccess = Request["UserSecurityAccess"];
        //    string EntityLocked = Request["EntityLocked"];
        //    decimal Id = decimal.Parse(DetailFilter.Substring(14));
        //    Library library = LibraryService.GetById(Id);

        //    string[] responses = library.FlexField.Split('§');
        //    IList<Pair<string, string>> questionResponses = new List<Pair<string, string>>();
        //    foreach (string response in responses)
        //    {
        //        string[] questionText = response.Split('¶');
        //        if (questionText.Length > 1)
        //            questionResponses.Add(new Pair<string, string>(questionText[0], questionText[1]));
        //    }
        //    ViewData["Title"] = library.Description;
        //    ViewData["QuestionAnswers"] = questionResponses;
        //    return View("Nugget");
        //}

        public ContentResult GetParametersOfEmail(string LibraryId)
        {
            string result = string.Empty;
            string mailto = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            string clientCompanyId = (string)Session["ClientCompany"];
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);
            //string urlCompany = clientCompany.Dns + ".compelligence.com" + appPath;
            string urlCompany = clientCompany.Dns + ".compelligence.com";
            decimal libraryNewsId = decimal.Parse(LibraryId);
            string userId = (string)Session["UserId"];
            Library library = LibraryService.GetById(libraryNewsId);
            //subject = urlCompany + " - Competitive News:" + library.Name;
            subject = "Competitive News: " + library.Name;
            string textBody = library.Name;
            if (!string.IsNullOrEmpty(library.Source))
            {
                textBody += " - " + library.Source;
            }
            textBody += "%0D%0A";
            textBody += "%0D%0A";
            textBody += StripHtml.GetInstance().GetTextPlain(library.Description);
            body = textBody;

            if (!string.IsNullOrEmpty(library.Link))
            {
                body += "%0D%0A";
                body += "%0D%0A";
                body += library.Link;
            }
            ViewData["LibraryId"] = LibraryId;
            //return View("CompetitiveNews");
            result = "LibraryNewsEmailSubject:" + subject + "LibraryNewsEmailBody:" + body ;
            return Content(result);

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

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];

            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<LibraryType> libraryTypeList = LibraryTypeService.GetAllActiveByClientCompany(clientCompany);
            IList<Library> libraryRelatedList = LibraryService.GetAllActiveByClientCompany(clientCompany);
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
            ViewData["RelatedName"] = string.Empty;
        }

        protected override void SetEntityDataToForm(Library library)
        {
            ViewData["DateAddedFrm"] = DateTimeUtility.ConvertToString(library.DateAdded, GetFormatDate());
            ViewData["DateDeletionFrm"] = DateTimeUtility.ConvertToString(library.DateDeletion, GetFormatDate());
            ViewData["CreatedByFrm"] = StringUtility.CheckNull(library.CreatedBy).Length == 0 ? string.Empty : UserProfileService.GetById(library.CreatedBy).Name;
            ViewData["RelatedName"] = string.Empty;
            //IList<string> metadataList = new List<string>();
            //foreach (string data in library.MetaData.Split(';'))
            //{
            //    metadataList.Add(data);
            //}
            //ViewData["MetaDataList"] = new SelectList(metadataList);
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
            library.OldDescription = library.Description;
        }

        protected override void GetFormData(Library library, FormCollection collection)
        {
            library.DateAdded = DateTimeUtility.ConvertToDate(collection["DateAddedFrm"], GetFormatDate());
            library.DateDeletion = DateTimeUtility.ConvertToDate(collection["DateDeletionFrm"], GetFormatDate());

            //IList<string> metadataList = new List<string>();
            //foreach (string data in library.MetaData.Split(';'))
            //{
            //    metadataList.Add(data);
            //}
            //ViewData["MetaDataList"] = new SelectList(metadataList);
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

        protected override void SetFormEntityDataToForm(Library library)
        {
            library.OldDescription = library.Description;
            ModelState.SetModelValue("OldDescription", new ValueProviderResult(library.OldDescription, library.OldDescription, CultureInfo.InvariantCulture));
        }

        #endregion

        public ActionResult CreateNewObject( string entityid, string entitytype)
        {
           // string cuserid = CurrentUser;
            
            if(!string.IsNullOrEmpty(entityid))
            { decimal id = decimal.Parse(entityid);
            Library library = LibraryService.GetById(id);
            if (library != null)
            {
                if (entitytype.Equals(NewsObjectType.Deal))
                {
                    CreateNewDeal(library);
                }
                else if (entitytype.Equals(NewsObjectType.Event))
                {
                    CreateNewEvent(library);
                }
                else if (entitytype.Equals(NewsObjectType.Kit))
                {
                    CreateNewKit(library);
                }
                else if (entitytype.Equals(NewsObjectType.Objective))
                {
                    CreateNewObjective(library);
                }
                else if (entitytype.Equals(NewsObjectType.Project))
                {
                    CreateNewProject(library);
                }
                else if (entitytype.Equals(NewsObjectType.Trend))
                {
                    CreateNewTrend(library);
                }
            }
            }
            return null;
        }

        private void CreateNewDeal(Library library)
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
        }

        private void CreateNewEvent(Library library)
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
            eventEntity.Url = library.Source;
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
        }

        private void CreateNewKit(Library library)
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
        }

        private void CreateNewObjective(Library library)
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
        }

        private void CreateNewProject(Library library)
        {
            Project project = new Project();
            project.Name =library.Name;
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
        }

        private void CreateNewTrend(Library library)
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
        }

        /// <summary>
        /// / Testing
        /// </summary>
        /// <param name="library"></param>
        /// <returns></returns>
        /// 

        public ContentResult CreateNewObjectByNews(string entityid, string entitytype)
        {
            string result = string.Empty;
            decimal idNewObject=0;
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
            return Content(idNewObject.ToString());
        }

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

        public ActionResult GetMail(string LibraryId)
        {
            string clientCompanyId = (string)Session["ClientCompany"];
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);
            string urlCompany = clientCompany.Dns + ".compelligence.com" + appPath;

            decimal libraryNewsId = decimal.Parse(LibraryId);
            string userId = (string)Session["UserId"];
            Library library = LibraryService.GetById(libraryNewsId);
            ViewData["Subject"] = urlCompany+" - Competitive News:" +library.Name;
            string textBody =StripHtml.GetInstance().GetTextPlain(library.Description);
            ViewData["Body"] = textBody;
            ViewData["LibraryId"] = LibraryId;
            return View("CompetitiveNews");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendMail(string LibraryId, FormCollection form)
        {
            string userId = (string)Session["UserId"];
            string libraryId = StringUtility.CheckNull(form["LibraryId"]);
            string to = StringUtility.CheckNull(form["ToEmail"]);
            string body = StringUtility.CheckNull(form["BodyEmail"]);
            decimal libraryNewsId = decimal.Parse(libraryId);
            Library library = LibraryService.GetById(libraryNewsId);
            EmailService.SendCompetitiveNewsEmail(library, userId, body, to);
            return null;
        }
    }
}
