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
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using System.Configuration;
using Compelligence.Util.Common;
using Compelligence.Web.Models.Util;
using System.Globalization;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    public class NewsController : BackEndAsyncFormController<Library, decimal>
    {
        #region Public Properties

        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        public ILibraryService LibraryService
        {
            get { return (ILibraryService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public ILibraryTypeService LibraryTypeService { get; set; }

        public IFileService FileService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

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

        #endregion

        #region Validation Methods

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
            ViewData["DateDeletionFrm"] = "";
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;
            ViewData["AddToNewsletter"] = "false";            
            ViewData["DateInNewsletterFrm"] = string.Empty;
            ViewData["FileList"] = string.Empty;
        }

        protected override void SetEntityDataToForm(Library library)
        {
            ViewData["DateAddedFrm"] = DateTimeUtility.ConvertToString(library.DateAdded, GetFormatDate());
            ViewData["DateDeletionFrm"] = DateTimeUtility.ConvertToString(library.DateDeletion, GetFormatDate());
            ViewData["CreatedByFrm"] = StringUtility.CheckNull(library.CreatedBy).Length == 0 ? string.Empty : UserProfileService.GetById(library.CreatedBy).Name;
            ViewData["RelatedName"] = string.Empty;
            ViewData["LibraryIdHidden"] = library.Id;
            library.OldDescription = library.Description;
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
            SetNewsLetterDataToForm(library);
            SetFileAttachmentsInMail(library);
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

        protected override void GetFormData(Library library, FormCollection collection)
        {
            string isNewsletter = collection["ToNewsletter"];
            if (!string.IsNullOrEmpty(isNewsletter))
            {
                string[] parameter = isNewsletter.Split(',');
                if (parameter[0].Equals("true"))
                {
                    library.AddToNewsletter = "Y";               
                }
            }
            library.DateAdded = DateTimeUtility.ConvertToDate(collection["DateAddedFrm"], GetFormatDate());
            library.DateDeletion = DateTimeUtility.ConvertToDate(collection["DateDeletionFrm"], GetFormatDate());
        }

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
            LibraryType librarytype = null;
            if (!Validator.IsBlankOrNull(library.LibraryTypeId))
            {
                 librarytype = LibraryTypeService.GetById(Convert.ToDecimal(library.LibraryTypeId));
            }
            if (!Validator.Equals(librarytype.KeyCode, LibraryTypeKeyCode.Email) || librarytype==null)
            {
                if (Validator.IsBlankOrNull(library.Link))
                {
                    ValidationDictionary.AddError("Link", LabelResource.LibraryLinkRequiredError);
                }
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

        public JsonResult SaveAddtoNewletter(string libraryid)
        {
            string Result1 = "No";               
            if (!string.IsNullOrEmpty(libraryid)) {
                Library library = LibraryService.GetById(decimal.Parse(libraryid));
                library.AddToNewsletter = "Y";
                LibraryService.Update(library);
                Result1 = "Yes";               
            }
            
                return Json(new
                {
                     Result = Result1
                });
            
            }

      
        protected override void SetFormEntityDataToForm(Library library)
        {
            SetNewsLetterDataToForm(library);
            SetFileAttachmentsInMail(library);
            ViewData["DateDeletionFrm"] = DateTimeUtility.ConvertToString(library.DateDeletion, GetFormatDate());
            library.OldDescription = library.Description;
            ModelState.SetModelValue("OldDescription", new ValueProviderResult(library.OldDescription, library.OldDescription, CultureInfo.InvariantCulture));
        }

        protected override void LoadFormData()
        {
            IList<Industry> industryList = IndustryService.GetByStatusAndTier(IndustryStatus.Enabled, IndustryTier.Tier1, CurrentCompany);
            ViewData["IndustryList"] = new SelectList(industryList, "Id", "Name");
            IList<Competitor> competitorList = CompetitorService.GetByStatusAndTier(CompetitorStatus.Enabled, CompetitorTier.Tier1, CurrentCompany);
            //IList<Competitor> competitorList = new List<Competitor>();
            ViewData["CompetitorList"] = new SelectList(competitorList, "Id", "Name");
            //            IList<Product> productList = ProductService.GetByStatusAndTier(ProductStatus.Enabled, ProductTier.Tier1, CurrentCompany);
            IList<Product> productList = new List<Product>();
            ViewData["ProductList"] = new SelectList(productList, "Id", "Name");
            IList<ResourceObject> dateList = ResourceService.GetAll<NewsDateFilter>();
            ViewData["DateList"] = new SelectList(dateList, "Id", "Value");
            DateTime Today = new DateTime();
            Today = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            DateTime PastTwoDays = Today.AddDays(-2);
            DateTime PastThreeDays = Today.AddDays(-3);
            DateTime PastWeek = Today.AddDays(-7);
            DateTime PastMonth = Today.AddMonths(-1);
            DateTime PastQuarter = Today.AddMonths(-4);
            DateTime PastYear = GetLastYear();
            ViewData["OptionDateList"] = Today.ToString("MM/dd/yyyy") + "," + PastTwoDays.ToString("MM/dd/yyyy") + "," + PastThreeDays.ToString("MM/dd/yyyy") + "," + PastWeek.ToString("MM/dd/yyyy") + "," + PastMonth.ToString("MM/dd/yyyy") + "," + PastQuarter.ToString("MM/dd/yyyy") + "," + PastYear.ToString("MM/dd/yyyy");
        }

        private DateTime GetYesterDay()
        {
            DateTime yesterday = new DateTime();
            int day = DateTime.Today.Day - 1;
            if (day == 0)
            {
                int mount = DateTime.Today.Month - 1;
                if (mount == 0)
                {
                    yesterday = GetLastDayOfMonth(DateTime.Today.Year - 1, 12);
                    yesterday = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 6, 0, 0);
                }
                else
                {
                    yesterday = GetLastDayOfMonth(DateTime.Today.Year, DateTime.Today.Month);
                    yesterday = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 6, 0, 0);
                }
            }
            else
            {
                yesterday = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 1, 6, 0, 0);
            }
            return yesterday;
        }

        private DateTime GetLastDayOfMonth(int year, int month)
        {
            DateTime dtTo = new DateTime(year, month, 1);
            dtTo = dtTo.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));
            return dtTo;
        }

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFileRawMode()
        {
            string fileNameResult = string.Empty;
            fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            string FileName = Server.UrlEncode(Request["HTTP_X_FILE_NAME"].Replace(@"../", string.Empty));

            if (FileName == null)
            {
                FileName = "name";
            }

            fileNameResult += "_" + System.IO.Path.GetFileName(FileName).Replace(' ', '_');
            try
            {
                System.IO.FileStream RawFile = new System.IO.FileStream(System.IO.Path.Combine(ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"], fileNameResult), System.IO.FileMode.OpenOrCreate);
                byte[] buffer = new byte[1024];
                int ibytes;

                while ((ibytes = Request.InputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    RawFile.Write(buffer, 0, ibytes);
                }
                Session["fileName"] = FileName;
                Session["fileNameResult"] = fileNameResult;
                RawFile.Close();
            }
            catch (Exception ex)
            { }
            return Content("{result:'" + fileNameResult + "'}");

        }

        private void SetFileAttachmentsInMail(Library library)
        {
            if (library.Id != 0)
            {
                IList<File> fileList = FileService.GetFilesByEntityId(library.Id, DomainObjectType.LibraryMail, CurrentCompany);
                if (fileList != null && fileList.Count > 0)
                {
                    ViewData["FileList"] = fileList;
                }
            }
        }

        private void SetNewsLetterDataToForm(Library library)
        {
            if (!string.IsNullOrEmpty(library.AddToNewsletter) && library.AddToNewsletter.Equals("Y"))
            {
               ViewData["AddToNewsletter"] = "true";
               library.ToNewsletter = true;
            }
            else
            {
                library.ToNewsletter = false;
            }
            ViewData["DateInNewsletterFrm"] = DateTimeUtility.ConvertToString(library.DateInNewsletter , GetFormatDate());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCompetitorByIndustry(decimal id)
        {
            IList<Competitor> competitorList = CompetitorService.GetByIndustryStatusAndTier(id,CompetitorStatus.Enabled, CompetitorTier.Tier1, CurrentCompany);
            //IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);
            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductByCompetitor(decimal id)
        {
            //IList<Product> productList = ProductService.GetByCompetitor(id);
            IList<Product> productList = ProductService.GetByCompetitorStatusAndTier(id,ProductStatus.Enabled, ProductTier.Tier1, CurrentCompany);
            return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
        }

        private DateTime GetLastYear()
        {
            DateTime result = new DateTime();
            int daysInCurrentMont = System.DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            int daysInPastMont = System.DateTime.DaysInMonth(DateTime.Today.Year - 1, DateTime.Today.Month);
            if (daysInPastMont >= DateTime.Today.Day)
            {
                result = new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, DateTime.Today.Day);
            }
            else
            {
                result = new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, daysInPastMont);
            }
            return result;
        }
    }
}
