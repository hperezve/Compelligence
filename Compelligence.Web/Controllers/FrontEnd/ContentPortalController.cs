using System;

using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using System.Collections.Specialized;
using System.Reflection;

using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;

using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility;
using Compelligence.Common.Utility.Web;
using Compelligence.Common.Utility.Parser;

using Compelligence.Common.Validation;

using Compelligence.DataTransfer.FrontEnd;
using Compelligence.Security.Managers;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;


using Resources;
using System.Threading;
using Compelligence.Security.Filters;
using Compelligence.Util.Common;
using System.Configuration;
using Compelligence.Domain.Entity.Views;
using Compelligence.Web.Models.Util;
using System.Text;
using Compelligence.Common.Browse;
using System.Collections;
using Compelligence.Common.Cache;
using Compelligence.Reports.Helpers;
//using System.IO;
using System.Drawing;
using Compelligence.Common.Utility.Upload;
using System.Web.UI;
using System.IO;

namespace Compelligence.Web.Controllers
{
    //[AuthenticationFilter] //Enabled because this class not is used from SFDC
    public class ContentPortalController : GenericFrontEndController
    {

        #region Public Properties
        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        public ISecurityGroupService SecurityGroupService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IQuizService QuizService { get; set; }

        public IQuestionService QuestionService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IProjectService ProjectService { get; set; }

        public IContentTypeService ContentTypeService { get; set; }

        public ILabelService LabelService { get; set; }

        public IConfigurationLabelsService ConfigurationLabelsService { get; set; }

        public IFileService FileService { get; set; }

        public IWebsiteService WebsiteService { get; set; }

        public IWebsiteDetailService WebsiteDetailService { get; set; }

        public IForumService ForumService { get; set; }

        public IActionHistoryService ActionHistoryService { get; set; }

        public IPositioningService PositioningService { get; set; }
        public ILibraryService LibraryService { get; set; }
        public IEntityNewsService EntityNewsService { get; set; }

        public IProjectIndustryService ProjectIndustryService { get; set; }
        public IProjectCompetitorService ProjectCompetitorService { get; set; }
        public IProjectProductService ProjectProductService { get; set; }
        public IReportService ReportService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }
        public ISalesForceService SalesForceService { get; set; }
        public ICompetitorPartnerService CompetitorPartnerService { get; set; }
        public IConfigurationDefaultsService ConfigurationDefaultsService { get; set; }
        public IProductCriteriaService ProductCriteriaService { get; set; }
        public IConfigurationUserTypeService ConfigurationUserTypeService { get; set; }
        public IPositioningCompetitorService PositioningCompetitorService { get; set; }
        public IPositioningIndustryService PositioningIndustryService { get; set; }
        public ICompetitorFinancialService CompetitorFinancialService { get; set; }
        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }

        public IStrengthWeaknessService StrengthWeaknessService { get; set; }

        #endregion

        //Get AllContentType and Inject Library,Competitor or Product
        private IList<LibraryCatalog> GetContentTypeCatalog(params decimal[] SchemeIds)
        {

            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            IList<WebsiteDetail> ActiveConfigDetails = new List<WebsiteDetail>();
            IList<ContentType> ContentTypes = ContentTypeService.GetAllActiveByClientCompany(CurrentCompany);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitive Messaging", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Positioning Statements", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Details Industry Competitor Product", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "News", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitors in Industry", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Products in Industry", CurrentCompany, CurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Industries", CurrentCompany, CurrentUser);
            //Disable Strengths/Weaknesses
            //Enable strengths/Weaknesses
            ContentTypeService.CreateIfNotExist(ContentTypes, "Strengths/Weaknesses", CurrentCompany, CurrentUser);
            //
            
            Website ActiveConfig = WebsiteService.GetActiveWebsite(CurrentCompany);

            if (ActiveConfig != null) // Getting Values by default
            {
                foreach (ContentType contenttype in ContentTypes)
                {
                    WebsiteDetail websitedetail = WebsiteDetailService.GetByContentType(ActiveConfig.Id, SchemeIds.Length, contenttype.Id);
                    ActiveConfigDetails.Add(websitedetail);
                    if (contenttype.Name.Equals("Details Industry Competitor Product"))
                    {
                        WebsiteDetail tempWebSiteDetail = WebsiteDetailService.GetByContentType(ActiveConfig.Id, 0, contenttype.Id);
                        if (tempWebSiteDetail.Displayable.Equals("N"))
                        {
                            ViewData["DetailsEntities"] = "no";
                        }
                    }
                }
                var ActiveConfigDetailsOrder = (from d in ActiveConfigDetails orderby d.Sequence select d);
                ActiveConfigDetails = ActiveConfigDetailsOrder.ToList<WebsiteDetail>();
                //Improve next lines
                IList<WebsitePanel> wspanels = WebsiteService.GetPanels(ActiveConfig.Id);
                string lefcontentWidth = null;
                if (wspanels == null || (wspanels != null && wspanels.Count < 5))
                {
                    WebsiteService.CreateDefaultPanels(ActiveConfig);
                    wspanels = WebsiteService.GetPanels(ActiveConfig.Id, WebsiteComponentType.Panel);
                }
                else
                {
                    for (int p = wspanels.Count - 1; p >= 0; p--)
                    {
                        if (!wspanels[p].ComponentType.Equals(WebsiteComponentType.Panel) || !wspanels[p].Displayable.Equals("Y"))
                        {
                            if (wspanels[p].ComponentType.Equals(WebsiteComponentType.Panel) && wspanels[p].Displayable.Equals("N"))
                            {
                                lefcontentWidth += "N";
                            }
                            wspanels.RemoveAt(p);

                        }
                    }


                }

                ViewData["Panels"] = wspanels ?? new List<WebsitePanel>();
                ViewData["LefContentWidth"] = lefcontentWidth;
            }


            //Fill All ContentType
            foreach (WebsiteDetail ActiveConfigDetail in ActiveConfigDetails) //Process all ContentTypeIds 
            {
                ContentType ActiveContentType = ContentTypeService.GetById(ActiveConfigDetail.ContentTypeId);

                if (ActiveContentType != null)  //User delete from ContenTypes
                {

                    LibraryCatalog oLibraryCatalog = new LibraryCatalog(ActiveContentType.Name);
                    oLibraryCatalog.CssClass = ActiveConfigDetail.Ajust.Equals("S") ? "contentBoxContainerSingle" : "contentBoxContainerDouble";
                    oLibraryCatalog.Displayable = ActiveConfigDetail.Displayable.Equals("Y");
                    IList<Project> Projects = new List<Project>();

                    switch (SchemeIds.Length)
                    {
                        case 0:
                            Projects = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId); break;
                        case 1:
                            Projects = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Industry, SchemeIds[0]); break;
                        case 2: //Process with Industry & Competitor
                            {
                                //Projects = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Industry, SchemeIds[0]); break;
                                IList<Project> ProjectCompetitors = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Competitor, SchemeIds[1]);
                                foreach (Project p in ProjectCompetitors)
                                {
                                    if (!Projects.Contains(p))
                                    {
                                        Projects.Add(p);
                                    }
                                }
                                break;
                            }
                        case 3://Process with Industry & Competitor & Product
                            {
                                //Projects = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Industry, SchemeIds[0]); break;
                                //IList<Project> ProjectCompetitors = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Competitor, SchemeIds[1]);
                                //foreach (Project p in ProjectCompetitors)
                                //{
                                //    if (!Projects.Contains(p))
                                //    {
                                //        Projects.Add(p);
                                //    }
                                //}
                                IList<Project> ProjectProducts = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Product, SchemeIds[2]);
                                foreach (Project p in ProjectProducts)
                                {
                                    if (!Projects.Contains(p))
                                    {
                                        Projects.Add(p);
                                    }
                                }

                                break;
                            }
                    }
                    foreach (Project ActiveProject in Projects)
                    {
                        if (ActiveProject.Status.Equals(ProjectStatus.Published) && !ActiveProject.Visibility.Equals(ProjectVisibility.BackEnd))
                        {
                            Compelligence.Domain.Entity.File file = FileService.GetByEntityId(ActiveProject.Id, DomainObjectType.Project);
                            ActiveProject.File = file;
                            ActiveProject.Labels = LabelService.GetByEntityId(ActiveProject.Id);
                            ActiveProject.RatingAllowed = ProjectService.RatingAllowed(ActiveProject.Id, CurrentUser);
                            oLibraryCatalog.Projects.Add(ActiveProject);
                        }
                    }
                    //Improve next sentences
                    if (ActiveContentType.Name.Equals("Positioning Statements"))
                    {
                        switch (SchemeIds.Length)
                        {
                            case 1:
                                ViewData["Positionig"] = null;
                                break;
                            case 2:
                                oLibraryCatalog.Position =  PositioningService.GetPositiongAndParent(PositioningRelation.Positioning, DomainObjectType.Competitor, SchemeIds[1], SchemeIds[0], CurrentCompany);
                                PositioningIndustryId positioningIndustryId = new PositioningIndustryId(oLibraryCatalog.Position.Id, SchemeIds[0]);
                                PositioningIndustry pi = PositioningIndustryService.GetById(positioningIndustryId);

                                if (oLibraryCatalog.Position != null && ( oLibraryCatalog.Position.EntityId != SchemeIds[1] || pi==null)) oLibraryCatalog.Position.IsGlobal = "Y";
                                ViewData["Positionig"] = oLibraryCatalog.Position;
                                break;
                            case 3:
                                oLibraryCatalog.Position = PositioningService.GetPositiongByProductAndParent(PositioningRelation.Positioning, DomainObjectType.Product, SchemeIds[2],SchemeIds[1], SchemeIds[0], CurrentCompany);
                                if (oLibraryCatalog.Position.Id == 0 || string.IsNullOrEmpty(oLibraryCatalog.Position.CreatedBy))
                                {
                                    oLibraryCatalog.Position = PositioningService.GetPositiongAndParent(PositioningRelation.Positioning, DomainObjectType.Competitor, SchemeIds[1], SchemeIds[0], CurrentCompany);
                                }
                                PositioningIndustryId piId = new PositioningIndustryId(oLibraryCatalog.Position.Id, SchemeIds[0]);
                                PositioningIndustry piT = PositioningIndustryService.GetById(piId);
                                if (oLibraryCatalog.Position != null &&( oLibraryCatalog.Position.EntityId != SchemeIds[2] || piT==null)) oLibraryCatalog.Position.IsGlobal = "Y";
                                ViewData["Positionig"] = oLibraryCatalog.Position;
                                break;
                        }
                    }
                    if (ActiveContentType.Name.Equals("Competitive Messaging"))
                    {
                        switch (SchemeIds.Length)
                        {
                            case 1:
                                ViewData["Competitive Messaging"] = null;
                                break;
                            case 2:
                                Competitor competitor = CompetitorService.GetCompetitorClient(CurrentCompany);
                                if (competitor != null && competitor.Id != SchemeIds[1])// COMPETITIVE MESSAGING ONLY SHOULD BE VISIBLE TO COMPETITORS AND NO CLIENT_COMPETITOR
                                {
                                    oLibraryCatalog.CompetitiveMes = PositioningService.GetCompetitiveMessaginAndParent(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, competitor.Id,  SchemeIds[1], SchemeIds[0], CurrentCompany);
                                    PositioningCompetitorId positioningCompetitorId = new PositioningCompetitorId(oLibraryCatalog.CompetitiveMes.Id, SchemeIds[1]);
                                    PositioningCompetitor pc = PositioningCompetitorService.GetById(positioningCompetitorId);
                                    PositioningIndustryId positioningIndustryId = new PositioningIndustryId(oLibraryCatalog.CompetitiveMes.Id, SchemeIds[0]);
                                    PositioningIndustry pi = PositioningIndustryService.GetById(positioningIndustryId);
                                    if (oLibraryCatalog.CompetitiveMes != null && (pc == null || pi==null)) oLibraryCatalog.CompetitiveMes.IsGlobal = "Y";
                                    ViewData["Competitive Messaging"] = oLibraryCatalog.CompetitiveMes;
                                }
                                else {
                                    ViewData["Competitive Messaging"] = null;
                                }
                                break;
                            ///TO FUTURO WILL WORK WITH THIS
                            case 3:
                                ViewData["Competitive Messaging"] = null;
                                break;
                        }
                    }
                    if (ActiveContentType.Name.Equals("News"))
                    {

                        switch (SchemeIds.Length)
                        {
                            case 1: //Industry
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Industry, SchemeIds[0], CurrentCompany); break;
                            case 2: //Industry+Competitor
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Competitor, SchemeIds[1], CurrentCompany); break;
                            case 3://Industry+Competitor+Product
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Product, SchemeIds[2], CurrentCompany); break;
                        }


                    }
                    if (ActiveContentType.Name.Equals("Strengths/Weaknesses"))
                    {
                        ViewData["SWcss"] = oLibraryCatalog.CssClass;
                    }
                    //Disable Strengths/Weaknesses
                    //Enable Strengths/Weaknesses
                    if (ActiveContentType.Name.Equals("Strengths/Weaknesses"))
                    {
                        if (SchemeIds.Length > 1)
                        {
                            try
                            {
                                oLibraryCatalog.Strenths = GetExternalCompetitor(SchemeIds[1], LocalCompetitorType.Strengths);
                                oLibraryCatalog.Weaknesses = GetExternalCompetitor(SchemeIds[1], LocalCompetitorType.Weaknesses);
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                        }
                    }

                    if (ActiveContentType.Name.Equals("Competitors in Industry"))
                    {
                        switch (SchemeIds.Length)
                        {
                            case 1:
                                //ViewData["Competitive Messaging"] = null;
                                IList<Competitor> competitors = CompetitorService.GetByIndustryId(SchemeIds[0]);
                                oLibraryCatalog.Competitors = competitors;
                                oLibraryCatalog.Industry = IndustryService.GetById( SchemeIds[0]);
                                break;
                            case 2:

                                break;
                            ///TO FUTURO WILL WORK WITH THIS
                            case 3:
                                break;
                        }
                    }
                    if (ActiveContentType.Name.Equals("Products in Industry"))
                    {
                        switch (SchemeIds.Length)
                        {
                            case 1:
                                oLibraryCatalog.ProductsWithCompetitor = ProductService.GetWithCompetitorByIndustryId(SchemeIds[0],CurrentCompany);
                                oLibraryCatalog.Industry = IndustryService.GetById(SchemeIds[0]);
                                break;
                            case 2:
                                oLibraryCatalog.Products = ProductService.GetByIndustryAndCompetitor(SchemeIds[0], SchemeIds[1]);
                                oLibraryCatalog.Industry = IndustryService.GetById(SchemeIds[0]);
                                oLibraryCatalog.CompetitorId = SchemeIds[1];
                                break;
                            ///TO FUTURO WILL WORK WITH THIS
                            case 3:
                                break;
                        }
                    }
                    if (ActiveContentType.Name.Equals("Industries"))
                    {
                        switch (SchemeIds.Length)
                        {
                            case 1:
                                break;
                            case 2:
                                oLibraryCatalog.Competitor = CompetitorService.GetById(SchemeIds[1]);
                                oLibraryCatalog.Industries = IndustryService.GetEnabledByCompetitorId(SchemeIds[1], IndustryStatus.Enabled, CurrentCompany);
                                break;
                            case 3:
                                break;
                        }
                    }
                    if (ActiveContentType.Name.Equals("Financial Information"))
                    {
                        switch (SchemeIds.Length)
                        {
                            case 1:
                                //oLibraryCatalog.IndustryFinancial = IndustryFinancialService.GetByIndustryId(SchemeIds[0], CurrentCompany);
                                break;
                            case 2:
                                //Competitor competitor = CompetitorService.GetCompetitorClient(CurrentCompany);
                                //if (competitor != null && competitor.Id != SchemeIds[1])// COMPETITIVE MESSAGING ONLY SHOULD BE VISIBLE TO COMPETITORS AND NO CLIENT_COMPETITOR
                                //{
                                //    oLibraryCatalog.ClientFinancialIncomeStatement = CompetitorFinancialService.GetByCompetitorIdAndType((decimal)competitor.Id, FinancialTimePeriod.Annual, CurrentCompany);
                                //    oLibraryCatalog.ClientName = competitor.Name;
                                //}
                                oLibraryCatalog.CompetitorFinancialIncomeStatement = CompetitorFinancialService.GetByCompetitorIdAndType((decimal)SchemeIds[1], FinancialTimePeriod.Annual, CurrentCompany);
                                //oLibraryCatalog.Competitor = CompetitorService.GetById(SchemeIds[1]);
                                break;
                            case 3:
                                break;
                        }
                    }
                    //
                    LibraryCatalogCollection.Add(oLibraryCatalog);
                }
            }
            //Process with reference to ProjectId

            return LibraryCatalogCollection;
        }


        //Block code for keeping Any selected on DropDownList
        private void UpdateDropDownWith<T>(string Label, IList<T> Collection)
        {
            IList<T> TempCollection = new List<T>();

            foreach (T objectCol in Collection)
            {
                if (Label.Equals("Industry"))
                {
                    IndustryByHierarchyView tempInd = (IndustryByHierarchyView)((object)objectCol);
                    if (tempInd.Status.Equals("ENBL"))
                    {
                        TempCollection.Add(objectCol);
                    }
                }
                if (Label.Equals("Competitor"))
                {
                    Competitor tempComp = (Competitor)((object)objectCol);
                    if (tempComp.Status.Equals("ENBL"))
                    {
                        TempCollection.Add(objectCol);
                    }
                }
                if (Label.Equals("Product"))
                {
                    Product tempProd = (Product)((object)objectCol);
                    if (tempProd.Status.Equals("ENBL"))
                    {
                        TempCollection.Add(objectCol);
                    }
                }
            }
            decimal Id = -1;
            T EmptyObject = default(T);

            if (Request.Params[Label] == null)
            {
                TempCollection.Insert(0, EmptyObject);
                ViewData[Label + "Collection"] = new SelectList(TempCollection, "Id", "Name", Id);
            }
            else
            {
                TempCollection.Insert(0, EmptyObject);
                try
                {
                    Id = decimal.Parse(Request.Params[Label]);
                    ViewData[Label + "Collection"] = new SelectList(TempCollection, "Id", "Name", Id);
                }
                catch
                {
                    ViewData[Label + "Collection"] = new SelectList(TempCollection, "Id", "Name");
                }
            }
        }

        #region Override Methods
        protected override void SetDefaultDataByPage()
        {
            ViewData["Entity"] = FrontEndPages.ContentPortal;
            ViewData["TitleHelp"] = "Content Portal";
        }
        #endregion

        [AuthenticationFilter]
        public ActionResult Index()
        {
            SetLabels();
            SetDefaultDataToLoadPage();
            //For Categorize Win/Loss Report
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            ViewData["Nuggets"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.Nugget, NuggetStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);

            ViewData["LongSurvey"] = QuizService.GetWinLossByStatus(QuizType.Survey, SurveyType.Long, NuggetStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            Quiz surveyShort = QuizService.GetByShortStatusVisible(SurveyType.Short, SurveyStatus.Complete, SurveyVisible.Yes, CurrentCompany);
            ViewData["ShortSurvey"] = surveyShort;
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(CurrentCompany);
            //ViewData["SocialLog"] = ActionHistoryService.GetBySocialLog(CurrentCompany);
            ViewData["CompetitorHasComment"] = false;
            ViewData["CompetitorStrenths"] = string.Empty;
            ViewData["CompetitorWeaknesses"] = string.Empty;
            //MycodeY
           // String DefaultsSocialLog = "false";
           //It takes tre true or false in Comparinator
           // ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(CurrentSecurityGroup, CurrentCompany);
           // if (configurationUserType != null)
           // {
           //     DefaultsSocialLog = configurationUserType.SocialLog;
           // }
           // ViewData["DefaultsSocialLog"] = DefaultsSocialLog;

            HttpBrowserCapabilitiesBase browser = Request.Browser;
            ViewData["Browser"] = browser;
            if (surveyShort != null)
                SetFormDataOfQuestion(CurrentUser, surveyShort.Id);
            string sIndustryId = Request.Params["Industry"];
            if (sIndustryId != null && sIndustryId.Length > 0)
            {
                decimal IndustryId = decimal.Parse(sIndustryId);
                surveyShort = QuizService.GetBySurveyShort(QuizType.Survey, SurveyType.Short, SurveyStatus.Complete, QuizClassificationVisible.Yes, IndustryId);
                SetFormDataOfQuestion(CurrentUser, surveyShort.Id);
            }

            ////mensaje = string.Format(mensaje, msg);
            //ScriptManager.RegisterStartupScript(this, this.GetType, "msgBox", mensaje, true);
            return ChangeIndustry();
        }


        public void setSocialLogByUser()
        {
            String DefaultsSocialLog = "false";
            //It takes tre true or false in Comparinator
            ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(CurrentSecurityGroup, CurrentCompany);
            if (configurationUserType != null)
            {
                DefaultsSocialLog = configurationUserType.SocialLog;
            }
            ViewData["DefaultsSocialLog"] = DefaultsSocialLog;

        }
        [AuthenticationFilter]
        public ActionResult ContentPopup(string title, string browseId, string industry, string competitor, string product)
        {
            //For Categorize Win/Loss Report
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            ViewData["Nuggets"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.Nugget, NuggetStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);

            ViewData["LongSurvey"] = QuizService.GetWinLossByStatus(QuizType.Survey, SurveyType.Long, NuggetStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            ViewData["ShortSurvey"] = QuizService.GetByShortStatusVisible(SurveyType.Short, SurveyStatus.Complete, SurveyVisible.Yes, CurrentCompany);
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(CurrentCompany);
            //ViewData["SocialLog"] = ActionHistoryService.GetBySocialLog(CurrentCompany);

            ViewData["CompetitorStrenths"] = string.Empty;
            ViewData["CompetitorWeaknesses"] = string.Empty;

            HttpBrowserCapabilitiesBase browser = Request.Browser;
            ViewData["Browser"] = browser;
            ChangeIndustry();
            ChangeCompetitor();
            if (product != null && product != "" && product != "undefined")
                ChangeProduct();

            //Server.Transfer("~/Views/ContentPortal/LeftContent.ascx?Title=" + title);
            return View("PopupContent");
            //return new EmptyResult();
        }

        public ActionResult CreateNews()
        {
            LibraryService.CreateEntityNewsOfOldLibraries(CurrentCompany);
            return Content("Done..!");
        }
        public ActionResult CreateContentType()
        {
            IList<UserProfile> usersRoot = UserProfileService.GetAllRootUsers();
            foreach (UserProfile user in usersRoot)
            {
                IList<ContentType> ContentTypes = ContentTypeService.GetAllActiveByClientCompany(user.ClientCompany);
                ContentTypeService.CreateIfNotExist(ContentTypes, "Competitive Messaging", user.ClientCompany, user.Id);
                ContentTypeService.CreateIfNotExist(ContentTypes, "Strengths/Weaknesses", user.ClientCompany, user.Id);
                ContentTypeService.CreateIfNotExist(ContentTypes, "Competitors in Industry", user.ClientCompany, user.Id);
                ContentTypeService.CreateIfNotExist(ContentTypes, "Products in Industry", user.ClientCompany, user.Id);
                ContentTypeService.CreateIfNotExist(ContentTypes, "Industries", user.ClientCompany, user.Id);
                ContentTypeService.CreateIfNotExist(ContentTypes, "Financial Information", user.ClientCompany, user.Id);
            }
            return Content("Done....Content Type was created!");
        }
        public ActionResult CreateNewsD(string dateBegin, string dateEnd)
        {
            DateTime? beginDatetime = DateTimeUtility.ConvertToDate(dateBegin, "MM/dd/yyyy");
            DateTime? endDatetime = DateTimeUtility.ConvertToDate(dateEnd, "MM/dd/yyyy");
            if (beginDatetime != null && endDatetime != null)
            {
                LibraryService.CreateEntityNewsOfOldLibrariesByDate((DateTime)beginDatetime, (DateTime)endDatetime, CurrentCompany);
                return Content("Done..!");
            }
            else
            {
                return Content("Incorrect ..!");
            }
        }
        public ActionResult ResizeImages()
        {
            IList<Industry> industryList = IndustryService.GetAllByClientCompany(CurrentCompany);
            if (industryList != null)
            {
                if (industryList.Count > 0)
                {
                    foreach (Industry industry in industryList)
                    {
                        if (!string.IsNullOrEmpty(industry.ImageUrl))
                        {
                            if (industry.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                            {
                                byte[] imageData = ResizeImage.GetBytesFromUrl(industry.ImageUrl);
                                if (imageData != null)
                                {
                                    System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
                                    Image fullsizeImage = Image.FromStream(stream);
                                    stream.Close();

                                    string[] urlObjects = industry.ImageUrl.Split('/');

                                    int newWidth = 250;
                                    int newHeight = 80;

                                    fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    if (fullsizeImage.Width > newWidth)
                                    {
                                        newWidth = fullsizeImage.Width;
                                    }
                                    int resizeHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                                    if (resizeHeight > newHeight)
                                    {
                                        newWidth = fullsizeImage.Width * newHeight / fullsizeImage.Height;
                                        resizeHeight = newHeight;
                                    }

                                    System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, resizeHeight, null, IntPtr.Zero);
                                    fullsizeImage.Dispose();

                                    Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();


                                    if (urlObjects.Length > 0)
                                    {
                                        newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                                        if (newFileImage.FileName.IndexOf("%20") != -1)
                                        {
                                            newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
                                        }
                                    }
                                    if (newFileImage.FileName.LastIndexOf('.') != -1)
                                    {
                                        newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir
                                    }
                                    decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                                    string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
                                    string fileNameResult = string.Empty;
                                    newFileImage.CreatedBy = CurrentUser;
                                    newFileImage.CreatedDate = DateTime.Now;
                                    newFileImage.LastChangedBy = CurrentUser;
                                    newFileImage.LastChangedDate = DateTime.Now;
                                    newFileImage.ClientCompany = CurrentCompany;
                                    newFileImage.PhysicalName = newPhysicalName;

                                    FileService.Save(newFileImage);
                                    fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
                                    newImage.Save(fileNameResult);
                                    industry.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");
                                    IndustryService.Update(industry);
                                }
                            }
                        }
                    }
                }
            }
            IList<Competitor> competitorList = CompetitorService.GetAllByClientCompany(CurrentCompany);
            if (competitorList != null)
            {
                if (competitorList.Count > 0)
                {
                    foreach (Competitor competitor in competitorList)
                    {
                        if (!string.IsNullOrEmpty(competitor.ImageUrl))
                        {
                            if (competitor.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                            {
                                byte[] imageData = ResizeImage.GetBytesFromUrl(competitor.ImageUrl);
                                if (imageData != null)
                                {
                                    System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
                                    Image fullsizeImage = Image.FromStream(stream);
                                    stream.Close();

                                    string[] urlObjects = competitor.ImageUrl.Split('/');

                                    int newWidth = 250;
                                    int newHeight = 80;

                                    fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    if (fullsizeImage.Width > newWidth)
                                    {
                                        newWidth = fullsizeImage.Width;
                                    }
                                    int resizeHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                                    if (resizeHeight > newHeight)
                                    {
                                        newWidth = fullsizeImage.Width * newHeight / fullsizeImage.Height;
                                        resizeHeight = newHeight;
                                    }

                                    System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, resizeHeight, null, IntPtr.Zero);
                                    fullsizeImage.Dispose();

                                    Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();


                                    if (urlObjects.Length > 0)
                                    {
                                        newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                                        if (newFileImage.FileName.IndexOf("%20") != -1)
                                        {
                                            newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
                                        }
                                    }
                                    if (newFileImage.FileName.LastIndexOf('.') != -1)
                                    {
                                        newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir
                                    }
                                    decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                                    string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
                                    string fileNameResult = string.Empty;
                                    newFileImage.CreatedBy = CurrentUser;
                                    newFileImage.CreatedDate = DateTime.Now;
                                    newFileImage.LastChangedBy = CurrentUser;
                                    newFileImage.LastChangedDate = DateTime.Now;
                                    newFileImage.ClientCompany = CurrentCompany;
                                    newFileImage.PhysicalName = newPhysicalName;

                                    FileService.Save(newFileImage);
                                    fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
                                    newImage.Save(fileNameResult);
                                    competitor.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");
                                    CompetitorService.Update(competitor);
                                }
                            }
                        }
                    }
                }
            }
            IList<Product> productList = ProductService.GetAllByClientCompany(CurrentCompany);
            if (productList != null)
            {
                if (productList.Count > 0)
                {
                    foreach (Product product in productList)
                    {
                        if (!string.IsNullOrEmpty(product.ImageUrl))
                        {
                            if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                            {
                                byte[] imageData = ResizeImage.GetBytesFromUrl(product.ImageUrl);

                                if (imageData != null)
                                {
                                    System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
                                    Image fullsizeImage = Image.FromStream(stream);
                                    stream.Close();

                                    string[] urlObjects = product.ImageUrl.Split('/');

                                    int newWidth = 250;
                                    int newHeight = 80;

                                    fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                    if (fullsizeImage.Width > newWidth)
                                    {
                                        newWidth = fullsizeImage.Width;
                                    }
                                    int resizeHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                                    if (resizeHeight > newHeight)
                                    {
                                        newWidth = fullsizeImage.Width * newHeight / fullsizeImage.Height;
                                        resizeHeight = newHeight;
                                    }

                                    System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, resizeHeight, null, IntPtr.Zero);
                                    fullsizeImage.Dispose();

                                    Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();


                                    if (urlObjects.Length > 0)
                                    {
                                        newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                                        if (newFileImage.FileName.IndexOf("%20") != -1)
                                        {
                                            newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
                                        }
                                    }
                                    if (newFileImage.FileName.LastIndexOf('.') != -1)
                                    {
                                        newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir
                                    }
                                    decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                                    string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
                                    string fileNameResult = string.Empty;
                                    newFileImage.CreatedBy = CurrentUser;
                                    newFileImage.CreatedDate = DateTime.Now;
                                    newFileImage.LastChangedBy = CurrentUser;
                                    newFileImage.LastChangedDate = DateTime.Now;
                                    newFileImage.ClientCompany = CurrentCompany;
                                    newFileImage.PhysicalName = newPhysicalName;

                                    FileService.Save(newFileImage);
                                    fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
                                    newImage.Save(fileNameResult);
                                    product.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");
                                    ProductService.Update(product);
                                }
                            }
                        }
                    }
                }
            }
            return Content("Done..!");
        }

        public JsonpResult GetIndustries(string id)
        {
            string key = Request["key"]; //verify that for security gettign data
            IList<Industry> result = IndustryService.GetAllActiveByClientCompany(id);
            JsonpResult jresult = ControllerUtility.GetJSonpList<Industry>(result, "Id", "Name");
            return jresult;
        }
        public JsonpResult GetCompetitors(decimal id)
        {
            string key = Request["key"]; //verify that for security gettign data
            IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);
            return ControllerUtility.GetJSonpList<Competitor>(competitorList, "Id", "Name");
        }
        public JsonpResult GetProducts(decimal id)
        {
            string key = Request["key"]; //verify that for security gettign data
            IList<Product> competitorList = ProductService.GetByCompetitor(id);
            return ControllerUtility.GetJSonpList<Product>(competitorList, "Id", "Name");
        }

        [AuthenticationFilter]
        public ActionResult ChangeIndustry() //Update Competitor and Product
        {
            SetLabels();
            setSocialLogByUser();
            ViewData["CompetitorStrenths"] = string.Empty;
            ViewData["CompetitorWeaknesses"] = string.Empty;
            ViewData["CompetitorHasComment"] = false;
            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(CurrentCompany);

            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(CurrentCompany));
            UpdateDropDownWith("Competitor", new List<Competitor>());
            UpdateDropDownWith("Product", new List<Product>());

            string sIndustryId = Request.Params["Industry"];
            if (sIndustryId != null && sIndustryId.Length > 0)
            {
                decimal IndustryId = decimal.Parse(sIndustryId);

                IList<Quiz> winloss = QuizService.GetByTargetType(QuizType.WinLoss, "", WinLossStatus.Complete, QuizClassificationVisible.Yes, IndustryId);
                IList<Quiz> nugget = QuizService.GetByTargetType(QuizType.Nugget, "", NuggetStatus.Complete, QuizClassificationVisible.Yes, IndustryId);
                IList<Quiz> survey = QuizService.GetByTargetType(QuizType.Survey, SurveyType.Long, SurveyStatus.Complete, QuizClassificationVisible.Yes, IndustryId);
                Quiz surveyShort = QuizService.GetBySurveyShort(QuizType.Survey, SurveyType.Short, SurveyStatus.Complete, QuizClassificationVisible.Yes, IndustryId);
                //Will be improve this code
                ViewData["WinLoss"] = winloss.Count == 0 ? QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany) : winloss;
                ViewData["LongSurvey"] = survey.Count == 0 ? QuizService.GetWinLossByStatus(QuizType.Survey, SurveyType.Long, NuggetStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany) : survey;
                if (surveyShort == null)
                {
                    surveyShort = QuizService.GetByShortStatusVisible(SurveyType.Short, SurveyStatus.Complete, SurveyVisible.Yes, CurrentCompany);
                    ViewData["ShortSurvey"] = surveyShort;
                }
                else
                {
                    surveyShort.Questions = QuestionService.GetByQuizId(surveyShort.Id);
                    ViewData["ShortSurvey"] = surveyShort;
                }
                if (nugget == null)
                {
                    ViewData["Nuggets"] = QuizService.GetByTargetType(QuizType.Nugget, NuggetStatus.Complete, CurrentCompany);
                }
                else
                {
                    ViewData["Nuggets"] = nugget;
                }
                LibraryCatalogCollection = GetContentTypeCatalog(IndustryId);

                UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(IndustryId));
                Industry industry = IndustryService.GetById(IndustryId);
                string industryImageUrl = industry.ImageUrl;
                ViewData["IndustryImageUrl"] = industryImageUrl;

                ViewData["EntityDetail"] = "Industry";
                ViewData["NameDetail"] = industry.Name;
                ViewData["DescriptionDetail"] = industry.Description;
                ViewData["ImageDetail"] = industryImageUrl;
                ViewData["UrlDetail"] = industry.Website;
                ViewData["UrlDetailText"] = GetTextOfURL(industry.Website);
                if (surveyShort != null)
                {
                    SetFormDataOfQuestion(CurrentUser, surveyShort.Id);
                }

            }
            else
            {
                LibraryCatalogCollection = GetContentTypeCatalog();
            }

            ViewData["LibraryCatalog"] = LibraryCatalogCollection;
            GetDataOfConfiguration(CurrentCompany);
            SetCurrentUser(CurrentUser);
            return View("Index");
        }

        public void SetCurrentUser(string userId)
        {
            UserProfile user = UserProfileService.GetById(userId);
            ViewData["User"] = user;
            ViewData["U"] = userId;
            ViewData["C"] = user.ClientCompany;
        }

        [AuthenticationFilter]
        public ActionResult SocialLogComparinator() //Update Competitor and Product
        {
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(CurrentCompany);
            GetDataOfConfiguration(CurrentCompany);
            return View("SocialLogContent");
        }

        public ActionResult SocialLogComparinatorSales() //Update Competitor and Product
        {
            string salesclientcompanyencode = (string)Request["C"];
            string cc = CurrentCompany;
            if (!string.IsNullOrEmpty(salesclientcompanyencode))
            {
                cc = Encryptor.Decode(salesclientcompanyencode);
            }
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(cc);
            GetDataOfConfiguration(cc);
            return View("SocialLogContent");
        }
        public ActionResult SocialLogComparinatorIndex() //Update Competitor and Product
        {
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(CurrentCompany);
            GetDataOfConfiguration(CurrentCompany);
            return View("SocialLogContent");
        }

        private void SetFormDataOfQuestion(string user, decimal QuizId)
        {
            IList<UserProfile> enduserList = UserProfileService.GetEndUsers(CurrentCompany);
            UserProfile User = UserProfileService.GetById(user);
            enduserList.Insert(0, User);
            Quiz ActiveQuiz = QuizService.GetById(QuizId);
            IList<Question> QuestionCollection = QuestionService.GetByQuizId(ActiveQuiz.Id);
            if (QuestionCollection == null)
            {
                QuestionCollection = new List<Question>();
            }

            foreach (Question ActiveQuestion in QuestionCollection)
            {
                ActiveQuestion.QuestionDetails = QuestionService.GetDetails(ActiveQuestion.Id);
            }

            ViewData["AssignedToEndUserList"] = new SelectList(enduserList, "Id", "Name");

            string clientCompany = (string)Session["ClientCompany"];
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);

            ViewData["IndustryIdList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            ViewData["CountQuestions"] = QuestionCollection.Count;
            ViewData["quiz"] = ActiveQuiz;
            ViewData["QuizQuestions"] = QuestionCollection;

            Question ExistQCO = QuestionService.GetByQuizIdAndType(QuizId, QuestionType.Competitors);
            Question ExistQWC = QuestionService.GetByQuizIdAndType(QuizId, QuestionType.WinningCompetitor);

            if (ExistQCO != null && ExistQWC != null)
            {
                if (ExistQWC.Item < ExistQCO.Item)
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                    ViewData["WinningItem"] = "X";
                }
                else
                {
                    ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
                    ViewData["WinningItem"] = ExistQWC.Item.ToString();
                }
            }
            else
            {
                ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                ViewData["WinningItem"] = "X";
            }


        }

        [AuthenticationFilter]
        public ActionResult ChangeCompetitor() //Update Competitor and Product
        {
            SetLabels();
            setSocialLogByUser();
            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(CurrentCompany);
            //ViewData["SocialLog"] = ActionHistoryService.GetBySocialLog(CurrentCompany);
            string sIndustryId = Request.Params["Industry"];
            string sCompetitorId = Request.Params["Competitor"];

            //UpdateDropDownWith("Industry", IndustryService.GetAllActiveByClientCompany(CurrentCompany));
            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(CurrentCompany));
            UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(decimal.Parse(sIndustryId)));
            UpdateDropDownWith("Product", new List<Product>());
            bool hasComment = false;
            if (sCompetitorId.Length > 0)
            {
                decimal IndustryId = decimal.Parse(Request.Params["Industry"]);
                decimal CompetitorId = decimal.Parse(Request.Params["Competitor"]);
                LibraryCatalogCollection = GetContentTypeCatalog(IndustryId, CompetitorId);

                IList<Quiz> winloss = QuizService.GetByTargetType(QuizType.WinLoss, "", WinLossStatus.Complete, QuizClassificationVisible.Yes, IndustryId, CompetitorId);
                IList<Quiz> survey = QuizService.GetByTargetType(QuizType.Survey, SurveyType.Long, SurveyStatus.Complete, QuizClassificationVisible.Yes, IndustryId, CompetitorId);
                IList<Quiz> nugget = QuizService.GetByTargetType(QuizType.Nugget, "", NuggetStatus.Complete, QuizClassificationVisible.Yes, IndustryId, CompetitorId);
                Quiz surveyShort = QuizService.GetBySurveyShort(QuizType.Survey, SurveyType.Short, SurveyStatus.Complete, QuizClassificationVisible.Yes, IndustryId, CompetitorId);
                IList<StrengthWeakness> strengthbyindustry = StrengthWeaknessService.GetByIndustryIdAndCompetitor(IndustryId, CompetitorId, StrengthWeaknessType.Strength, CurrentCompany);
                if (strengthbyindustry == null)
                {
                    strengthbyindustry = new List<StrengthWeakness>();
                }
                IList<StrengthWeakness> weaknessbyindustry = StrengthWeaknessService.GetByIndustryIdAndCompetitor(IndustryId, CompetitorId, StrengthWeaknessType.Weakness, CurrentCompany);
                if (weaknessbyindustry == null)
                {
                    weaknessbyindustry = new List<StrengthWeakness>();
                }
                Forum forum = ForumService.GetByEntityIdAndForumResponse(CompetitorId, DomainObjectType.Competitor, ForumType.Discussion);
                if (forum != null)
                {
                    hasComment = true;
                }
                ViewData["StrengthforIndustry"] = strengthbyindustry;
                ViewData["WeaknessforIndustry"] = weaknessbyindustry;
                ViewData["SWCompetitorId"] = sCompetitorId;
                ViewData["SWIndustryId"] = sIndustryId;
                //Will be improve this code
                if (winloss.Count == 0)
                    ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
                else
                    ViewData["WinLoss"] = winloss;

                if (survey.Count == 0)
                {
                    ViewData["LongSurvey"] = QuizService.GetWinLossByStatus(QuizType.Survey, SurveyType.Long, NuggetStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
                }
                else
                {
                    ViewData["LongSurvey"] = survey;
                }
                if (surveyShort == null)
                {
                    surveyShort = QuizService.GetByShortStatusVisible(SurveyType.Short, SurveyStatus.Complete, SurveyVisible.Yes, CurrentCompany);
                    ViewData["ShortSurvey"] = surveyShort;
                }
                else
                {
                    surveyShort.Questions = QuestionService.GetByQuizId(surveyShort.Id);
                    ViewData["ShortSurvey"] = surveyShort;
                }
                if (nugget == null)
                {
                    ViewData["Nuggets"] = QuizService.GetByTargetType(QuizType.Nugget, NuggetStatus.Complete, CurrentCompany);
                }
                else
                {
                    ViewData["Nuggets"] = nugget;
                }

                UpdateDropDownWith("Product", ProductService.GetByIndustryAndCompetitor(decimal.Parse(sIndustryId), decimal.Parse(sCompetitorId)));
                Industry industry = IndustryService.GetById(IndustryId);
                Competitor competitor = CompetitorService.GetById(CompetitorId);

                //string urlindustryimage = GetUrlOfImage(industry.ImageUrl);                
                string urlcompeittor = GetUrlOfImage(competitor.ImageUrl);
                ViewData["IndustryImageUrl"] = urlcompeittor;
                ViewData["CompetitorImageUrl"] = urlcompeittor;
                ViewData["CompetitorHasComment"] = hasComment;
                ViewData["EntityDetail"] = "Competitor";
                ViewData["NameDetail"] = competitor.Name;
                ViewData["DescriptionDetail"] = competitor.Description;
                //ViewData["ImageDetail"] = competitor.ImageUrl;
                ViewData["ImageDetail"] = urlcompeittor;
                ViewData["UrlDetail"] = competitor.Website;
                ViewData["UrlDetailText"] = GetTextOfURL(competitor.Website);
                if (surveyShort != null)
                {
                    SetFormDataOfQuestion(CurrentUser, surveyShort.Id);
                }
            }
            else
            {
                return ChangeIndustry();
            }


            ViewData["LibraryCatalog"] = LibraryCatalogCollection;
            GetDataOfConfiguration(CurrentCompany);
            SetCurrentUser(CurrentUser);
            return View("Index");
        }
        [AuthenticationFilter]
        public ActionResult ChangeProduct() //Update Competitor and Product
        {
            SetLabels();
            setSocialLogByUser();
            ViewData["CompetitorStrenths"] = string.Empty;
            ViewData["CompetitorWeaknesses"] = string.Empty;
            ViewData["CompetitorHasComment"] = false;
            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            ViewData["Social"] = ActionHistoryService.GetBySocialVisibleFE(CurrentCompany);
            //ViewData["SocialLog"] = ActionHistoryService.GetBySocialLog(CurrentCompany);
            string sIndustryId = Request.Params["Industry"];
            string sCompetitorId = Request.Params["Competitor"];
            string sProductId = Request.Params["Product"];

            //UpdateDropDownWith("Industry", IndustryService.GetAllActiveByClientCompany(CurrentCompany));
            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(CurrentCompany));
            UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(decimal.Parse(sIndustryId)));
            UpdateDropDownWith("Product", ProductService.GetByIndustryAndCompetitor(decimal.Parse(sIndustryId), decimal.Parse(sCompetitorId)));

            if (sProductId.Length > 0)
            {
                decimal IndustryId = decimal.Parse(Request.Params["Industry"]);
                decimal CompetitorId = decimal.Parse(Request.Params["Competitor"]);
                decimal ProductId = decimal.Parse(Request.Params["Product"]);
                IList<Quiz> winloss = QuizService.GetByTargetType(QuizType.WinLoss, "", WinLossStatus.Complete, QuizClassificationVisible.Yes, IndustryId, CompetitorId, ProductId);
                IList<Quiz> survey = QuizService.GetByTargetType(QuizType.Survey, SurveyType.Long, SurveyStatus.Complete, SurveyVisible.Yes, IndustryId, CompetitorId, ProductId);
                IList<Quiz> nugget = QuizService.GetByTargetType(QuizType.Nugget, "", NuggetStatus.Complete, QuizClassificationVisible.Yes, IndustryId, CompetitorId, ProductId);
                Quiz surveyShort = QuizService.GetBySurveyShort(QuizType.Survey, SurveyType.Short, SurveyStatus.Complete, QuizClassificationVisible.Yes, IndustryId, CompetitorId, ProductId);
                //Will be improve this code

                if (winloss.Count == 0)
                {
                    ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
                }
                else
                {
                    ViewData["WinLoss"] = winloss;
                }

                if (survey.Count == 0)
                {
                    ViewData["LongSurvey"] = QuizService.GetWinLossByStatus(QuizType.Survey, SurveyType.Long, NuggetStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
                }
                else
                {
                    ViewData["LongSurvey"] = survey;
                }

                if (surveyShort == null)
                {
                    surveyShort = QuizService.GetByShortStatusVisible(SurveyType.Short, SurveyStatus.Complete, SurveyVisible.Yes, CurrentCompany);
                    ViewData["ShortSurvey"] = surveyShort;
                }
                else
                {
                    surveyShort.Questions = QuestionService.GetByQuizId(surveyShort.Id);
                    ViewData["ShortSurvey"] = surveyShort;
                }

                if (nugget == null)
                {
                    ViewData["Nuggets"] = QuizService.GetByTargetType(QuizType.Nugget, NuggetStatus.Complete, CurrentCompany);
                }
                else
                {
                    ViewData["Nuggets"] = nugget;
                }

                LibraryCatalogCollection = GetContentTypeCatalog(IndustryId, CompetitorId, ProductId);
                Industry industry = IndustryService.GetById(IndustryId);
                Competitor competitor = CompetitorService.GetById(CompetitorId);
                Product product = ProductService.GetById(ProductId);
                if (ProductService.HaveCompetitorClient(product.Id, CurrentCompany))
                {
                    product.IsClientCompetitor = "Y";
                }
                string clientCompanyImageUrl = string.Empty;
                if (string.IsNullOrEmpty(CurrentCompany))
                {
                    ClientCompany cc = ClientCompanyService.GetById(CurrentCompany);
                    clientCompanyImageUrl = cc.Imageurl;
                }
                else clientCompanyImageUrl = (string)Session["Imageurl"];
                SetImageUrlToProduct(product, competitor.ImageUrl, clientCompanyImageUrl);
                string industryImageUrl = GetTextOfURL(industry.ImageUrl);
                //ViewData["IndustryImageUrl"] = industryImageUrl;
                string competitorImageUrl = GetTextOfURL(competitor.ImageUrl);
                //ViewData["CompetitorImageUrl"] = competitorImageUrl;
                string productImageUrl = GetUrlOfImage(product.ImageUrl);

                ViewData["ProductImageUrl"] = productImageUrl;
                ViewData["IndustryImageUrl"] = productImageUrl;
                ViewData["CompetitorImageUrl"] = productImageUrl;

                ViewData["EntityDetail"] = ViewData["ProductLabel"];
                ViewData["NameDetail"] = product.Name;
                ViewData["DescriptionDetail"] = product.Description;

                ViewData["ImageDetail"] = productImageUrl;
                ViewData["UrlDetail"] = product.Url;
                ViewData["UrlDetailText"] = GetTextOfURL(product.Url);
                if (surveyShort != null)
                {
                    SetFormDataOfQuestion(CurrentUser, surveyShort.Id);
                }
            }
            else
                return ChangeCompetitor();

            ViewData["LibraryCatalog"] = LibraryCatalogCollection;
            GetDataOfConfiguration(CurrentCompany);
            SetCurrentUser(CurrentUser);
            return View("Index");
        }

        public ActionResult DiscoverSFDC()
        {
            StringBuilder result = new StringBuilder();
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);
            string usermail = Encryptor.Encode(StringUtility.CheckNull(userProfile.Email));
            string clientCompany = Encryptor.Encode(StringUtility.CheckNull(CurrentCompany));
            result.AppendLine(CurrentUser + "-" + userProfile.Email + "-" + usermail + " <br>");
            result.AppendLine(CurrentCompany + "-" + clientCompany);
            return Content(result.ToString());

        }

        public ActionResult DiscoverMail()
        {
            StringBuilder result = new StringBuilder();
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);
            string usermail = Encryptor.Encode(StringUtility.CheckNull(userProfile.Email));
            string clientCompany = Encryptor.Encode(StringUtility.CheckNull(CurrentCompany));
            result.AppendLine(CurrentUser + "-" + userProfile.Email + "-" + usermail + " <br>");
            result.AppendLine(CurrentCompany + "-" + clientCompany);
            return Content(result.ToString());

        }

        public ActionResult Discover()
        { 

            string result=BlackBox.Decrypt( StringUtility.CheckNull(Request["K"]) );
            return Content(result);

        }

        public ActionResult Rating(string ProjectId, string Rating)
        {
            if (ProjectId != null && Rating != null)
            {
                string userid = (string)Session["UserId"];
                UserProfile User = UserProfileService.GetById(userid);
                ProjectService.SaveRating(decimal.Parse(ProjectId), decimal.Parse(Rating), User);
                //ActionHistory(decimal.Parse(ProjectId), EntityAction.SetedRating, DomainObjectType.Project);
                ActionHistoryService.RecordActionHistory(decimal.Parse(ProjectId), EntityAction.SetedRating, DomainObjectType.Project, ActionFrom.FrontEnd, userid, CurrentCompany);
                //ActionHistory(decimal.Parse(ProjectId), EntityAction.SetedRating, DomainObjectType.Project);
            }
            //return Content(string.Empty);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download(decimal id)
        {
            string check = StringUtility.CheckNull(Request["chk"]);
            Compelligence.Domain.Entity.File file = FileService.GetByEntityId(id, DomainObjectType.Project);
            if (file == null)
                return Content("NotFound");

            if (file.FileFormat == "Link")
            {
                if (file.Description == null || file.Description == "")
                {
                    string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
                    string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

                    if (check.ToLower().Equals("true"))
                    {
                        if ((file != null) && System.IO.File.Exists(fullpath + file.PhysicalName))
                            return Content("Found");
                        else
                            return Content("NotFound");
                    }
                    else
                    {
                        UserProfile user = UserProfileService.GetById(CurrentUser);
                        ProjectService.SaveDownload(id, user);
                        ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                        GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
                    }

                }
                else
                {
                    ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                    return Content(file.Description);
                }


            }
            else
            {

                string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
                string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

                if (check.ToLower().Equals("true"))
                {
                    if ((file != null) && System.IO.File.Exists(fullpath + file.PhysicalName))
                        return Content("Found");
                    else
                        return Content("NotFound");
                }
                else
                {
                    UserProfile user = UserProfileService.GetById(CurrentUser);
                    ProjectService.SaveDownload(id, user);
                    ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                    GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
                }



            }
            return Content(string.Empty);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBack(decimal id, FormCollection form)
        {
            ForumResponse forumResponse = new ForumResponse();
            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Project;
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            forumResponse.ClientCompany = CurrentCompany;

            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

            // ActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project);
            ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);

            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackSalesTools(decimal id, FormCollection form)
        {
            ForumResponse forumResponse = new ForumResponse();
            string salesclientcompanyencode = StringUtility.CheckNull(form["C"]);
            string salesuseridencode = StringUtility.CheckNull(form["U"]);
            if (!string.IsNullOrEmpty(salesclientcompanyencode) && !string.IsNullOrEmpty(salesuseridencode))
            {
                string cc = Encryptor.Decode(salesclientcompanyencode);
                string u = Encryptor.Decode(salesuseridencode);
                forumResponse.EntityId = id;
                forumResponse.EntityType = DomainObjectType.Project;
                forumResponse.CreatedBy = u;
                forumResponse.CreatedDate = DateTime.Now;
                forumResponse.LastChangedBy = u;
                forumResponse.LastChangedDate = DateTime.Now;
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                forumResponse.ClientCompany = cc;

                ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

                // ActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project);
                ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project, ActionFrom.FrontEnd, u, cc);
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackByType(FormCollection form)
        {
            string entityId = StringUtility.CheckNull(form["EntityId"]);
            string industryId = StringUtility.CheckNull(form["IndustryId"]);
            decimal entityIdOfObject = 0;
            if (!string.IsNullOrEmpty(entityId))
            {
                entityIdOfObject = Decimal.Parse(entityId);
            }
            ForumResponse forumResponse = new ForumResponse();
            forumResponse.EntityId = entityIdOfObject;
            forumResponse.EntityType = StringUtility.CheckNull(form["EntityType"]);
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            forumResponse.ClientCompany = CurrentCompany;

            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

            // ActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project);
            if (!string.IsNullOrEmpty(industryId))
            {
                decimal indId = Decimal.Parse(industryId);
                ActionHistoryService.RecordCommentCompetitor(entityIdOfObject, EntityAction.FeedBack, forumResponse.EntityType, indId, DomainObjectType.Industry, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            }
            else
            {
                ActionHistoryService.RecordActionHistory(entityIdOfObject, EntityAction.FeedBack, forumResponse.EntityType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackSalesToolsByType(decimal id, FormCollection form)
        {
            ForumResponse forumResponse = new ForumResponse();
            string salesclientcompanyencode = StringUtility.CheckNull(form["C"]);
            string salesuseridencode = StringUtility.CheckNull(form["U"]);
            if (!string.IsNullOrEmpty(salesclientcompanyencode) && !string.IsNullOrEmpty(salesuseridencode))
            {
                string cc = Encryptor.Decode(salesclientcompanyencode);
                string u = Encryptor.Decode(salesuseridencode);
                forumResponse.EntityId = id;
                forumResponse.EntityType = DomainObjectType.Project;
                forumResponse.CreatedBy = u;
                forumResponse.CreatedDate = DateTime.Now;
                forumResponse.LastChangedBy = u;
                forumResponse.LastChangedDate = DateTime.Now;
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                forumResponse.ClientCompany = cc;

                ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

                // ActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project);
                ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project, ActionFrom.FrontEnd, u, cc);
            }
            return null;
        }

        public ActionResult GetLibrary(decimal libraryid)
        {
            return Content(LibraryService.GetById(libraryid).Description);
        }

        //it's for test delete or migrate any object
        //exist only for test
        public ActionResult MigrateProjects()
        {
            IList<Project> projects = ProjectService.GetAll();
            foreach (Project p in projects)
            {
                //if (p.IndustryId != null)
                //{
                //    ProjectIndustry pi = new ProjectIndustry(new ProjectIndustryId((decimal)p.Id, (decimal)p.IndustryId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    ProjectIndustryService.Save(pi);
                //}
                //if (p.CompetitorId != null)
                //{
                //    ProjectCompetitor pi = new ProjectCompetitor(new ProjectCompetitorId((decimal)p.Id, (decimal)p.CompetitorId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    ProjectCompetitorService.Save(pi);
                //}
                //if (p.ProductId != null)
                //{
                //    ProjectProduct pi = new ProjectProduct(new ProjectProductId((decimal)p.Id, (decimal)p.ProductId));
                //    pi.ClientCompany = p.ClientCompany;
                //    pi.CreatedBy = p.CreatedBy;
                //    pi.CreatedDate = p.CreatedDate;
                //    pi.LastChangedBy = p.LastChangedBy;
                //    pi.LastChangedDate = p.LastChangedDate;
                //    ProjectProductService.Save(pi);
                //}

            }

            return Content("Done..!");
        }

        public ContentResult UpdateStrength()
        {
            string industryid = Request["industryid"];
            string id = StringUtility.CheckNull(Request["Id"]);
            string name = Request["N"];
            string desc = Request["D"];
            string global = Request["G"];
            //to salesforce and system
            string salesuserid = GetDecodeParam("U");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;

            StrengthWeakness sw = StrengthWeaknessService.GetById(decimal.Parse(id));
            
            if(sw != null)
            {
                StrengthWeaknessService.GetById(decimal.Parse(id));
                sw.Name= name;
                sw.Description = desc;

                if (global == "true" || global == "True" || global == "TRUE" || global == "Y" || global == "y")
                {
                    global = "Y";
                    industryid = string.Empty;
                }
                else
                    global = " ";

                sw.IsGlobal = global;                
                sw.IndustriesIds = industryid;
                //sw.CreatedBy = strength.LastChangedBy;
                //sw.CreatedDate = DateTime.Now;
                sw.LastChangedBy = salesuserid;
                sw.LastChangedDate = DateTime.Now;                
                StrengthWeaknessService.Update(sw);
            }
            return Content(sw.Id.ToString());
        }

        public ContentResult UpdateWeakness()
        {
            string industryid = Request["industryid"];
            string id = StringUtility.CheckNull(Request["Id"]);
            string name = Request["N"];
            string desc = Request["D"];
            string global = Request["G"];
            //to salesforce and system
            string salesuserid = GetDecodeParam("U");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;            
            StrengthWeakness sw = StrengthWeaknessService.GetById(decimal.Parse(id));

            if (sw != null)
            {
                StrengthWeaknessService.GetById(decimal.Parse(id));
                sw.Name = name;
                sw.Description = desc;

                if (global == "true" || global == "True" || global == "TRUE" || global == "Y" || global == "y")
                {
                    global = "Y";
                    industryid = string.Empty;
                }
                else
                    global = " ";

                sw.IsGlobal = global;                
                sw.IndustriesIds = industryid;
                //sw.CreatedBy = strength.LastChangedBy;
                //sw.CreatedDate = DateTime.Now;
                sw.LastChangedBy = localuserid;                
                sw.LastChangedDate = DateTime.Now;
                //sw.ClientCompany = product.ClientCompany;
                StrengthWeaknessService.Update(sw);                
            }
            return Content(sw.Id.ToString());
        }

        public ContentResult createStrength()
        {
            string entityId = Request["entityId"];
            string industryid = Request["industryid"];           
            string name = Request["N"];
            string desc = Request["D"];
            string global = Request["G"];
            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salescompanyid) ? CurrentCompany : salescompanyid;
            if (global == "true" || global == "True" || global == "TRUE"  || global == "Y"  || global == "y")
            {
                global = "Y";
                industryid = string.Empty;
            }
            else
                global = " ";

                StrengthWeakness sw = new StrengthWeakness();

                sw.EntityId = decimal.Parse(entityId);
                sw.EntityType = DomainObjectType.Competitor;
                sw.Name = name;
                sw.Description = desc;
                sw.IsGlobal = global;
                sw.Type = StrengthWeaknessType.Strength;
                sw.IndustriesIds = industryid;
                sw.CreatedBy = localuserid;
                sw.CreatedDate = DateTime.Now;
                sw.LastChangedBy = localuserid;
                sw.LastChangedDate = DateTime.Now;
                sw.ClientCompany = localcompanyid;

                StrengthWeaknessService.Save(sw);

                return Content(sw.Id.ToString());
        }

        public ContentResult createWeakness()
        {
            string entityId = Request["entityId"];
            string industryid = Request["industryid"];
            string name = Request["N"];
            string desc = Request["D"];
            string global = Request["G"];//true  

            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salescompanyid) ? CurrentCompany : salescompanyid;
            if (global == "true" || global == "True" || global == "TRUE" || global == "Y" || global == "y")
            {
                global = "Y";
                industryid = string.Empty;
            }
            else
                global = " ";
                        
            StrengthWeakness sw = new StrengthWeakness();

            sw.EntityId = decimal.Parse(entityId);
            sw.EntityType = DomainObjectType.Competitor;
            sw.Name = name;
            sw.Description = desc;
            sw.IsGlobal = global;
            sw.Type = StrengthWeaknessType.Weakness;
            sw.IndustriesIds = industryid;
            sw.CreatedBy = localuserid;
            sw.CreatedDate = DateTime.Now;
            sw.LastChangedBy = localuserid;
            sw.LastChangedDate = DateTime.Now;
            sw.ClientCompany = localcompanyid;

            StrengthWeaknessService.Save(sw);

            return Content(sw.Id.ToString());
        }

        public void deleteStrength()
        {
            string id = StringUtility.CheckNull(Request["Id"]);
 
            StrengthWeakness sw = StrengthWeaknessService.GetById(decimal.Parse(id));

            if (sw != null)            
            {
                StrengthWeaknessService.Delete(sw);
            }
            //return View("ContentPortal");
        }

        public void deleteWeakness()
        {
            string id = StringUtility.CheckNull(Request["Id"]);

            StrengthWeakness sw = StrengthWeaknessService.GetById(decimal.Parse(id));

            if (sw != null)
            {
                StrengthWeaknessService.Delete(sw);
            }
            //return View("ContentPortal");
        }

        public ActionResult DeleteLibraries()
        {
            IList<Library> libraries = LibraryService.GetAll();
            foreach (Library l in libraries)
            {
                LibraryService.Delete(l);
            }
            return Content("Done..!");
        }

        public ActionResult UpdateProductCriteriaNumeric()
        {
            ProductCriteriaService.UpdateValueDecimal();
            return Content("Done..!");
        }

        public ActionResult CreateConfigurationUserTypeToCompanies()
        {
            ClientCompanyService.CreateConfigurationUserType();
            return Content("Done..!");
        }

        public ActionResult ChangueUrlImage()
        {
            IList<Product> products = ProductService.GetAll();
            foreach (Product product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                    {
                        byte[] imageData = ResizeImage.GetBytesFromUrl(product.ImageUrl);
                        if (imageData != null)
                        {
                            MemoryStream stream = new MemoryStream(imageData);
                            try
                            {
                                Image fullsizeImage = Image.FromStream(stream);
                                stream.Close();

                                string[] urlObjects = product.ImageUrl.Split('/');

                                int newWidth = 250;
                                int newHeight = 80;

                                fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                                if (fullsizeImage.Width > newWidth)
                                {
                                    newWidth = fullsizeImage.Width;
                                }
                                int resizeHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                                if (resizeHeight > newHeight)
                                {
                                    newWidth = fullsizeImage.Width * newHeight / fullsizeImage.Height;
                                    resizeHeight = newHeight;
                                }

                                System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, resizeHeight, null, IntPtr.Zero);
                                fullsizeImage.Dispose();

                                Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();


                                if (urlObjects.Length > 0)
                                {
                                    newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                                    if (newFileImage.FileName.IndexOf("%20") != -1)
                                    {
                                        newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
                                    }
                                    if (newFileImage.FileName.IndexOf("?") != -1)
                                    {
                                        string[] parameterBegin = newFileImage.FileName.Split('?');
                                        if (parameterBegin.Length > 1)
                                        {
                                            newFileImage.FileName = parameterBegin[0];
                                        }
                                    }
                                    if (newFileImage.FileName.IndexOf("&") != -1)
                                    {
                                        string[] parameterOther = newFileImage.FileName.Split('?');
                                        if (parameterOther.Length > 1)
                                        {
                                            newFileImage.FileName = parameterOther[0];
                                        }
                                    }
                                    if (newFileImage.FileName.IndexOf("=") != -1)
                                    {
                                        string[] parameterAssignment = newFileImage.FileName.Split('?');
                                        if (parameterAssignment.Length > 1)
                                        {
                                            newFileImage.FileName = parameterAssignment[0];
                                        }
                                    }
                                }
                                if (newFileImage.FileName.LastIndexOf('.') != -1)
                                {
                                    newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir
                                }
                                else if (newFileImage.FileFormat != null)
                                {
                                    if (newFileImage.FileFormat.Equals("ashx"))
                                    {
                                        newFileImage.FileFormat = "jpg";
                                        newFileImage.FileName = newFileImage.FileName.Substring(0, newFileImage.FileName.LastIndexOf('.')) + "." + newFileImage.FileFormat;
                                    }
                                }
                                else if (newFileImage.FileFormat == null)
                                {

                                    newFileImage.FileFormat = "jpg";
                                    newFileImage.FileName = newFileImage.FileName + "." + newFileImage.FileFormat;

                                }
                                decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                                string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;
                                string fileNameResult = string.Empty;

                                newFileImage.CreatedBy = product.LastChangedBy;
                                newFileImage.CreatedDate = DateTime.Now;
                                newFileImage.LastChangedBy = product.LastChangedBy;
                                newFileImage.LastChangedDate = DateTime.Now;
                                newFileImage.ClientCompany = product.ClientCompany;

                                newFileImage.PhysicalName = newPhysicalName;

                                FileService.Save(newFileImage);
                                fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
                                newImage.Save(fileNameResult);
                                product.ImageUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");

                                ProductService.Update(product);
                            }
                            catch
                            {
                                bool ErrorImageS = true;

                            }
                        }


                    }

                }
            }
            return Content("Done..!");
        }



        public ActionResult VerifyDuplicateLibraries()
        {
            IList<Library> libraries = LibraryService.GetAllByClientCompany(CurrentCompany);
            StringBuilder sb = new StringBuilder();
            foreach (Library l in libraries)
            {
                LibraryType lt = LibraryService.GetLibraryType((decimal)l.LibraryTypeId);
                if (lt.KeyCode.Equals(LibraryTypeKeyCode.News))
                {
                    sb.AppendLine("" + l.Name + ", " + l.Link + ", (" + LibraryService.CountByLink(l.Link, l.Name, CurrentCompany) + ")<br>");
                }
            }
            return Content(sb.ToString() + "<br>Done..!");
        }

        public ActionResult DeleteDuplicateLibraries()
        {
            IList<Library> libraries = LibraryService.GetAllByClientCompany(CurrentCompany);
            StringBuilder sb = new StringBuilder();

            for (int librarycount = libraries.Count - 1; librarycount > 0; librarycount--)
            {
                Library l = libraries[librarycount];
                LibraryType lt = LibraryService.GetLibraryType((decimal)l.LibraryTypeId);
                if (lt != null && lt.KeyCode != null && lt.KeyCode.Equals(LibraryTypeKeyCode.News))
                {
                    if (LibraryService.CountByLink(l.Link, l.Name, CurrentCompany) > 1)
                    {
                        LibraryService.Delete(l);
                        sb.AppendLine("" + l.Name + ", " + l.Link + "<br>");
                    }
                }
            }
            return Content(sb.ToString() + "<br>Done..!");
        }
        public ActionResult DeletePastLibraries()
        {
            LibraryService.DeletePastLibraries(CurrentCompany);
            return Content("<br>Done..!");
        }
        public ActionResult RelationPartnersCompetitor()
        {
            CompetitorPartnerService.SaveRelationCompetitorPartner();
            return Content("<br>Done..!");
        }
        public ActionResult SalesForceMetric()
        {

            ViewData["ReportFilter"] = Request["ReportFilter"];
            ViewData["ReportTitle"] = Request["ReportTitle"];
            ViewData["ReportModule"] = Request["ReportModule"];
            return View("SalesForceMetric");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateReport(FormCollection collection)
        {
            string browseId = Request["BrowseId"];
            string filterCriteria = Request["FilterCriteria"];
            string reportTitle = Request["Title"];
            string hidencColumnCriteria = Request["HiddenColumnCriteria"];

            ResourceDataManager.GetInstance().CleanNumberGroup();

            BrowseObject browseObject = GetBrowseObjectForQuery(browseId, filterCriteria);
            IList dataSourceObjects = ReportService.GetData(browseObject);
            string reportFilter = GetFilterCriteria(filterCriteria, browseObject);
            string reportHiddenColumn = GetHiddenColumnCriteria(hidencColumnCriteria);
            IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();

            IDictionary<string, string> columnDictionary = new Dictionary<string, string>();

            reportParameters["BrowseObject"] = browseObject;
            reportParameters["DataSource"] = dataSourceObjects;
            UserProfile userProfile = UserManager.GetInstance().GetUserProfile(Session["UserId"].ToString());
            reportParameters["UserId"] = userProfile.Name;
            ClientCompany clientCompany = ClientCompanyService.GetById(Session["ClientCompany"].ToString());
            reportParameters["ClientCompany"] = clientCompany.Name;
            reportParameters["ReportTitle"] = reportTitle;
            reportParameters["ReportFilter"] = reportFilter;
            string[] filterHCCriteriaArray = hidencColumnCriteria.Split(':');
            foreach (string hiddenColumn in filterHCCriteriaArray)
            {
                string[] operators = hiddenColumn.Split('_');
                string[] fields = operators[0].Split('.');
                if ((operators.Length == 3) && (operators[2].ToString().Equals("false")))
                {
                    columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "true");
                    reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
                }
                else
                {
                    columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "false");
                    reportParameters[fields[1].ToString() + "HiddenColumn"] = "false";
                }
            }
            reportParameters["ColumnDictionary"] = columnDictionary;
            string reportFile = ReportHelper.PrintReport(reportParameters);

            return Content(reportFile);
        }

        private BrowseObject GetBrowseObjectForQuery(string browseId, string filterCriteria)
        {
            BrowseObject browseObject = (BrowseObject)ReportManager.GetInstance().GetBrowseObject(browseId).Clone();
            browseObject.WhereClause = ExpressionParser.GetExpression(Session, browseObject.WhereClause);
            browseObject.WhereClause = ExpressionParser.GetExpression(browseObject.WhereClause);
            browseObject.InitializeBrowseFilters();
            GetFilterCriteria(browseObject, filterCriteria);

            return browseObject;
        }



        public void GetFilterCriteria(BrowseObject browseObject, string filterCriteria)
        {
            if (!string.IsNullOrEmpty(filterCriteria))
            {
                IList<BrowseFilter> browseFilters = new List<BrowseFilter>();
                string[] searchFilterArray = filterCriteria.Split(':');

                foreach (string searchFilter in searchFilterArray)
                {
                    string[] operators = searchFilter.Split('_');

                    if ((operators.Length == 3) && (!string.IsNullOrEmpty(operators[2])))
                    {
                        browseFilters.Add(new BrowseFilter(operators[0], (BrowseFilter.Operator)Enum.Parse(typeof(BrowseFilter.Operator), operators[1]), operators[2]));
                    }
                }

                browseObject.AddBrowseFilters(browseFilters);
            }
        }

        private string GetFilterCriteria(string filterCriteria, BrowseObject browseObject)
        {
            string[] filterCriteriaArray = filterCriteria.Split(':');
            filterCriteria = "";
            foreach (string filter in filterCriteriaArray)
            {
                string[] operators = filter.Split('_');
                if ((operators.Length == 3) && (!string.IsNullOrEmpty(operators[2])))
                {
                    string[] fields = operators[0].Split('.');
                    string filterColumn = fields[1];
                    string filterValue = operators[2];
                    string stringOperator = ResourceDataManager.GetInstance().GetLabel("FilterOperator", operators[1]);
                    string operatorTempo = stringOperator;
                    stringOperator = (string)HttpContext.GetGlobalResourceObject("LabelResource", "FilterOperator" + stringOperator);
                    if (string.IsNullOrEmpty(stringOperator))
                    {
                        stringOperator = operatorTempo;
                    }
                    for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
                    {
                        if (operators[0] == browseObject.BrowseColumns[i].Column)
                        {
                            filterColumn = browseObject.BrowseColumns[i].Label;
                            if (browseObject.BrowseColumns[i].ColumnTypeClass.Equals(Compelligence.Common.Browse.BrowseColumn.TypeClass.Standard))
                            {

                                filterValue = ResourceDataManager.GetInstance().GetLabel(browseObject.BrowseColumns[i].ColumnType.ToString(), operators[2]);
                            }
                        }
                    }
                    filterCriteria += filterColumn + " " + stringOperator + " " + filterValue + "\n";
                }
            }
            if (string.IsNullOrEmpty(filterCriteria))
            {
                filterCriteria = "Print All";
            }
            return filterCriteria;
        }

        private string GetHiddenColumnCriteria(string hidenColumnCriteria)
        {
            string columnHiddens = string.Empty;
            string[] filterHCCriteriaArray = hidenColumnCriteria.Split(':');
            foreach (string hiddenColumn in filterHCCriteriaArray)
            {
                string[] operators = hiddenColumn.Split('_');
                if ((operators.Length == 3) && (operators[2].ToString().Equals("false")))
                {
                    string[] fields = operators[0].Split('.');
                    columnHiddens += fields[1].ToString() + " is hidden " + operators[2].ToString() + "\n";
                }
            }
            return columnHiddens;
        }

        private string GetTextOfURL(string url)
        {
            string textUrl = url;
            if (url.IndexOf('_') != -1)
            {
                textUrl = url.Replace('_', '-');
            }

            return textUrl;
        }


        private string GetUrlOfImage(string url)
        {
            //string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            //string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            //string localPath = Request.Url.LocalPath;
            //string originalString = Request.Url.OriginalString;
            //string host = Request.Url.Host;
            //string port = Request.Url.Port.ToString();

            if (url.IndexOf("./") == 0)
            {
                url = "." + url;
                //if (originalString.IndexOf(localPath) != -1)
                //{
                //    originalString = originalString.Replace(localPath, "");
                //    string urltempo = url.Replace("./", "");
                //    //url = originalString + "/" + urltempo;
                //    url = AppDomain.CurrentDomain.BaseDirectory + "/" + urltempo;
                //}
            }

            return url;
        }

        private IList<string> GetExternalCompetitor(decimal competitorId, string ExternalCompetitorType)
        {
            String name = CompetitorService.GetById(competitorId).Name;
            ClientCompany clientcompany = ClientCompanyService.GetById(CurrentCompany);
            IList<string> result = new List<string>();
            if(!string.IsNullOrEmpty(clientcompany.SalesForceUser) && !string.IsNullOrEmpty(clientcompany.SalesForcePassword) && !string.IsNullOrEmpty(clientcompany.SalesForceToken))
                result = SalesForceService.GetSalesforceExternalCompetitors(clientcompany.SalesForceUser, clientcompany.SalesForcePassword + clientcompany.SalesForceToken, name, ExternalCompetitorType);
            return result;
        }

        public void Contentwidth(string C50, string C100)
        {
            ViewData["C50"] = C50;
            ViewData["C100"] = C100;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsByCompetitor(decimal id)
        {
            //SetLabels();
            decimal industryId = Convert.ToDecimal(StringUtility.CheckNull(Request["IndustryId"]));

            string clientCompanyId = String.Empty;
            string salesclientcompanyencode = StringUtility.CheckNull(Request["C"]);

            if (!String.IsNullOrEmpty(salesclientcompanyencode) && string.IsNullOrEmpty(CurrentCompany)) //then get from salesforce
                clientCompanyId = Encryptor.Decode(salesclientcompanyencode);
            else
                clientCompanyId = CurrentCompany;
            IList<Product> productList = ProductService.GetByIndustryAndCompetitor(industryId, id);

            //IList<ProductWithCriteriaValuesView> productListView = ProductService.GetByIndustryAndCompetitorView(industryId, id);
            //IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(clientCompanyId);
            //bool DefaultsFeaturesTab = false;
            //if (configurations != null)
            //{
            //    if (configurations.Count > 0)
            //    {
            //        DefaultsFeaturesTab = Convert.ToBoolean(configurations[0].FeaturesTab);
            //    }
            //}

            //if (productListView == null || productListView.Count == 0)
            //{
            //    productListView = new List<ProductWithCriteriaValuesView>();
            //}
            return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
            //return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "ColumnDefault");

            //if (DefaultsFeaturesTab)
            //{
            //    return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "HaveProductCriteria");
            //}
            //else
            //{
            //    return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "ColumnDefault");
            //}
        }
    }
}