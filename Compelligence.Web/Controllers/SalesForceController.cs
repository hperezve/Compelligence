using System.Web.Mvc;

using System.Collections.Generic;
using System.Linq;

using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;

using Compelligence.Domain.Entity;
using Compelligence.Common.Utility;


using Compelligence.Util.Type;
using Compelligence.Web.Models.Util;
using Compelligence.DataTransfer.FrontEnd;
using Compelligence.Domain.Entity.Views;
using Compelligence.Domain.Entity.Resource;
using Resources;
using System;
using System.Web;
using System.Configuration;
using Compelligence.Security.Managers;
using Common.Logging;



namespace Compelligence.Web.Controllers
{
    public class SalesForceController : GenericFrontEndController
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(SalesForceService));
        #region Public Properties

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

        public IFileService FileService { get; set; }

        public IWebsiteService WebsiteService { get; set; }

        public IWebsiteDetailService WebsiteDetailService { get; set; }

        public IForumService ForumService { get; set; }
        public IForumResponseService ForumResponseService { get; set; }

        public IActionHistoryService ActionHistoryService { get; set; }

        public IPositioningService PositioningService { get; set; }
        public ILibraryService LibraryService { get; set; }
        public IEntityNewsService EntityNewsService { get; set; }

        public IProjectIndustryService ProjectIndustryService { get; set; }
        public IProjectCompetitorService ProjectCompetitorService { get; set; }
        public IProjectProductService ProjectProductService { get; set; }
        public IReportService ReportService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }
        public IEmailService EmailService { get; set; }
        public IProductCriteriaService ProductCriteriaService { get; set; }
        public IStrengthWeaknessService StrengthWeaknessService { get; set; }
        public IConfigurationDefaultsService ConfigurationDefaultsService { get; set; }
        public ISalesForceService SalesForceService { get; set; }
        public IPositioningCompetitorService PositioningCompetitorService { get; set; }
        public IPositioningIndustryService PositioningIndustryService { get; set; }
        public ICompetitorFinancialService CompetitorFinancialService { get; set; }
        #endregion

        public ActionResult Index()
        {
            return View();
        }


        //For CrossDomains
        public JsonpResult GetIndustriesp(string id)
        {
            string key = Request["key"]; //verify that for security gettign data
            IList<IndustryByHierarchyView> result = IndustryService.FindIndustryHierarchy(id);
            JsonpResult jresult = ControllerUtility.GetJSonpList<IndustryByHierarchyView>(result, "Id", "Name");
            return jresult;
        }
        public JsonpResult GetCompetitorsp(decimal id)
        {
            string key = Request["key"]; //verify that for security gettign data
            IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);
            return ControllerUtility.GetJSonpList<Competitor>(competitorList, "Id", "Name");
        }

        public JsonpResult GetAllCompetitorsp()
        {
            //Autenticate ClientCompany
            //Validate apikey
            string salesemailencode = (string)Request["U"];
            string apikey = (string)Request["K"];

            string salesemail = Encryptor.Decode(StringUtility.CheckNull(salesemailencode));
            UserProfile userProfile = UserProfileService.GetByEmail(salesemail);
            if (userProfile == null)
                return null;
            ClientCompany clientCompany = ClientCompanyService.GetById(userProfile.ClientCompany);
            if (clientCompany.Status.Equals(ClientCompanyStatus.Disabled))
            {
                return null;
            }
            IList<Competitor> competitors = CompetitorService.GetAllActiveByClientCompany(userProfile.ClientCompany);
            return ControllerUtility.GetJSonpList<Competitor>(competitors, "Id", "Name");
        }




        public JsonpResult GetProductsp(decimal id)//id=CompetitorId
        {
            string sIndustryId=(string)Request["industryid"];
            string key = Request["key"]; //verify that for security gettign data
            IList<Product> competitorList = ProductService.GetByIndustryAndCompetitor(decimal.Parse(sIndustryId), id);
            return ControllerUtility.GetJSonpList<Product>(competitorList, "Id", "Name");
        }
        //Regular Json
        public JsonResult GetIndustries(string id)//id=ClientCompany
        {
            string key = Request["key"]; //verify that for security gettign data
            IList<IndustryByHierarchyView> result = IndustryService.FindIndustryHierarchy(id);
            JsonResult jresult = ControllerUtility.GetSelectOptionsFromGenericList<IndustryByHierarchyView>(result, "Id", "Name");
            return jresult;
        }
        public JsonResult GetCompetitors(decimal id)//id=IndustryId
        {
            string key = Request["key"]; //verify that for security gettign data
            IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);
            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
        }
        public JsonResult GetProducts(decimal id)//id=CompetitorId
        {
            string sIndustryId=(string)Request["industryid"];
            string key = Request["key"]; //verify that for security gettign data
            IList<Product> competitorList = ProductService.GetByIndustryAndCompetitor(decimal.Parse(sIndustryId), id);
            return ControllerUtility.GetSelectOptionsFromGenericList<Product>(competitorList, "Id", "Name");
        }

        //
        //ContentPortal Navigation
        //
        public ActionResult ContentPortal()
        {
            //Autenticate ClientCompany
            //Validate apikey
            string salesemailencode = (string)Request["U"];
            string apikey = (string)Request["K"];

            string sIndustryId = StringUtility.CheckNull(Request["Industry"]);
            string sCompetitorId = StringUtility.CheckNull(Request["Competitor"]);
            string sProductId = StringUtility.CheckNull(Request["Product"]);

            string salesemail = String.Empty;
            try
            {
                salesemail = Encryptor.Decode(StringUtility.CheckNull(salesemailencode));
            }
            catch (Exception ex)
            {
                return Content("Compelligence message: unknown error, does not have access to the Compelligence system.");
            }


            UserProfile userProfile = UserProfileService.GetByEmail(salesemail);
            if (userProfile == null)
                return Content("Compelligence message: " + salesemail + " does not have access to the Compelligence system.");

            ClientCompany clientCompany = ClientCompanyService.GetById(userProfile.ClientCompany);
            if (clientCompany.Status.Equals(ClientCompanyStatus.Disabled ) )
            {
                return Content("Your company's subscription has been suspended. Please contact your system administrator or <b>support@compelligence.com</b> for assistance.");
            }

            string salesclientcompany = userProfile.ClientCompany;
            string salesclientcompanyencode = Encryptor.Encode(salesclientcompany);
            //Need improve next lines
            if (sIndustryId.Length > 0 &&
                sCompetitorId.Length > 0 &&
                   sProductId.Length > 0)
            {
                return ContentPortalCHP();
            }
            else
                if (sIndustryId.Length > 0 &&
                  sCompetitorId.Length > 0)
                {
                    return ContentPortalCHC();
                }

            return RedirectToAction("ContentPortalCHI", new { U = Encryptor.Encode(userProfile.Id), C = salesclientcompanyencode });
        }

        public ActionResult ContentPortalCHI()
        {
            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            string salesuserid = Encryptor.Decode(salesuseridencode);
            string salesclientcompany = Encryptor.Decode(salesclientcompanyencode);

            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();

            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(salesclientcompany));
            UpdateDropDownWith("Competitor", new List<Competitor>());
            UpdateDropDownWith("Product", new List<Product>());

            string sIndustryId = Request.Params["Industry"];
            if (sIndustryId != null && sIndustryId.Length > 0)
            {
                decimal IndustryId = decimal.Parse(sIndustryId);

                LibraryCatalogCollection = GetContentTypeCatalog(salesuserid, salesclientcompany, IndustryId);

                UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(IndustryId));
                Industry industry = IndustryService.GetById(IndustryId);
                ViewData["IndustryImageUrl"] = industry.ImageUrl;

                ViewData["EntityDetail"] = "Industry";
                ViewData["NameDetail"] = industry.Name;
                ViewData["DescriptionDetail"] = industry.Description;
                ViewData["ImageDetail"] = industry.ImageUrl;
                ViewData["UrlDetail"] = industry.Website;
                ViewData["UrlDetailText"] = GetTextOfURL(industry.Website);
            }
            else
            {
                LibraryCatalogCollection = GetContentTypeCatalog(salesuserid,salesclientcompany);
            }

            ViewData["LibraryCatalog"] = LibraryCatalogCollection;
            ViewData["U"] = salesuseridencode;
            ViewData["C"] = salesclientcompanyencode;
            ViewData["CompetitorHasComment"] = false;
            GetDataOfConfiguration(salesclientcompany);
            SetCurrentUser(salesuserid);
            return View("ContentPortal");
        }

        public ActionResult ContentPortalCHC()
        {

            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            string salesuserid = Encryptor.Decode(salesuseridencode);
            string salesclientcompany = Encryptor.Decode(salesclientcompanyencode);

            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            string sIndustryId = Request.Params["Industry"];
            string sCompetitorId = Request.Params["Competitor"];
            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(salesclientcompany));
            UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(decimal.Parse(sIndustryId)));
            UpdateDropDownWith("Product", new List<Product>());
            bool hasComment = false;
            if (sCompetitorId.Length > 0)
            {
                decimal IndustryId = decimal.Parse(Request.Params["Industry"]);
                decimal CompetitorId = decimal.Parse(Request.Params["Competitor"]);
                LibraryCatalogCollection = GetContentTypeCatalog(salesuserid,salesclientcompany,IndustryId, CompetitorId);

                UpdateDropDownWith("Product", ProductService.GetByIndustryAndCompetitor(decimal.Parse(sIndustryId), decimal.Parse(sCompetitorId)));
                Industry industry = IndustryService.GetById(IndustryId);
                Competitor competitor = CompetitorService.GetById(CompetitorId);
                ViewData["IndustryImageUrl"] = competitor.ImageUrl;
                ViewData["CompetitorImageUrl"] = competitor.ImageUrl;
                Forum forum = ForumService.GetByEntityIdAndForumResponse(CompetitorId, DomainObjectType.Competitor, ForumType.Discussion);
                if (forum != null)
                {
                    hasComment = true;
                }
                ViewData["EntityDetail"] = "Competitor";
                ViewData["NameDetail"] = competitor.Name;
                ViewData["DescriptionDetail"] = competitor.Description;
                ViewData["ImageDetail"] = competitor.ImageUrl;
                ViewData["UrlDetail"] = competitor.Website;
                ViewData["UrlDetailText"] = GetTextOfURL(competitor.Website);

                IList<StrengthWeakness> strengthbyindustry = StrengthWeaknessService.GetByIndustryIdAndCompetitor(IndustryId, CompetitorId, StrengthWeaknessType.Strength, salesclientcompany);
                if (strengthbyindustry == null)
                {
                    strengthbyindustry = new List<StrengthWeakness>();
                }
                IList<StrengthWeakness> weaknessbyindustry = StrengthWeaknessService.GetByIndustryIdAndCompetitor(IndustryId, CompetitorId, StrengthWeaknessType.Weakness, salesclientcompany);
                if (weaknessbyindustry == null)
                {
                    weaknessbyindustry = new List<StrengthWeakness>();
                }
                ViewData["StrengthforIndustry"] = strengthbyindustry;
                ViewData["WeaknessforIndustry"] = weaknessbyindustry;
                ViewData["SWCompetitorId"] = CompetitorId.ToString();
            }
            else
            {
                return ContentPortalCHI();
            }
            ViewData["SWIndustryId"] = sIndustryId;
            ViewData["CompetitorHasComment"] = hasComment;
            ViewData["LibraryCatalog"] = LibraryCatalogCollection;

            ViewData["U"] = salesuseridencode;
            ViewData["C"] = salesclientcompanyencode;
            GetDataOfConfiguration(salesclientcompany);
            SetCurrentUser(salesuserid);
            return View("ContentPortal");

        }
        public ActionResult ContentPortalCHP()
        {

            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            string salesuserid = Encryptor.Decode(salesuseridencode);
            string salesclientcompany = Encryptor.Decode(salesclientcompanyencode);

            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            string sIndustryId = Request.Params["Industry"];
            string sCompetitorId = Request.Params["Competitor"];
            string sProductId = Request.Params["Product"];

            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(salesclientcompany));
            UpdateDropDownWith("Competitor", CompetitorService.GetByIndustryId(decimal.Parse(sIndustryId)));
            UpdateDropDownWith("Product", ProductService.GetByIndustryAndCompetitor(decimal.Parse(sIndustryId), decimal.Parse(sCompetitorId)));

            ViewData["U"] = salesuseridencode;
            ViewData["C"] = salesclientcompanyencode;
            ViewData["CompetitorHasComment"] = false;
            if (sProductId.Length > 0)
            {
                decimal IndustryId = decimal.Parse(Request.Params["Industry"]);
                decimal CompetitorId = decimal.Parse(Request.Params["Competitor"]);
                decimal ProductId = decimal.Parse(Request.Params["Product"]);

                LibraryCatalogCollection = GetContentTypeCatalog(salesuserid,salesclientcompany,IndustryId, CompetitorId, ProductId);
                Industry industry = IndustryService.GetById(IndustryId);
                Competitor competitor = CompetitorService.GetById(CompetitorId);
                Product product = ProductService.GetById(ProductId);
                if (ProductService.HaveCompetitorClient(product.Id, salesclientcompany))
                {
                    product.IsClientCompetitor = "Y";
                }
                string clientCompanyImageUrl = string.Empty;
                ClientCompany cc = ClientCompanyService.GetById(salesclientcompany);
                clientCompanyImageUrl = cc.Imageurl;
                SetImageUrlToProduct(product, competitor.ImageUrl, clientCompanyImageUrl);
                ViewData["IndustryImageUrl"] = product.ImageUrl;
                ViewData["CompetitorImageUrl"] = product.ImageUrl;
                ViewData["ProductImageUrl"] = product.ImageUrl;

                ViewData["EntityDetail"] = "Product";
                ViewData["NameDetail"] = product.Name;
                ViewData["DescriptionDetail"] = product.Description;
                ViewData["ImageDetail"] = product.ImageUrl;
                ViewData["UrlDetail"] = product.Url;
                ViewData["UrlDetailText"] = GetTextOfURL(product.Url);
            }
            else
                return ContentPortalCHC();

            ViewData["LibraryCatalog"] = LibraryCatalogCollection;

            GetDataOfConfiguration(salesclientcompany);
            SetCurrentUser(salesuserid);
            return View("ContentPortal");
        }
        //
        //ContentPortal Navigation-End
        //

        //Start Comment
        public ActionResult Comments() //it works with ContentPortal
        {

            decimal EntityId=decimal.Parse( (string)Request["EntityId"] ); 
            string ObjectType = (string)Request["ObjectType"];
            string salesuseridencode = (string)Request["U"];
            string salesuserid = Encryptor.Decode(salesuseridencode);

            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
            IList<ForumResponse> forumresponses = null;
            forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);

            foreach (ForumResponse fr in forumresponses)
            {
                fr.HasAccess = ForumResponseService.HasAccess(fr, salesuserid);
            }
            ViewData["ObjectType"] = ObjectType;
            ViewData["Comments"] = forumresponses;
            ViewData["EntityId"] = EntityId;
            ViewData["Scope"] = Request["Scope"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];

            ViewData["U"] = salesuseridencode;
            //ViewData["C"] = salesclientcompanyencode;
            ViewData["ProductId"] = string.Empty;
            ViewData["IndustryId"] = string.Empty;
            return View("Comments");
        }
        
        //Start Feedback
        public ActionResult FeedBack() //it works with ContentPortal
        {
            ViewData["EntityId"] = decimal.Parse((string)Request["EntityId"]);
            ViewData["ObjectType"] = (string)Request["ObjectType"];
            ViewData["U"] = (string)Request["U"];
            return View("FeedBack");
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FeedBackSave()
        {
            decimal entityid = decimal.Parse((string)Request["EntityId"]);
            string objecttype = (string)Request["ObjectType"];
            string salesuseridencode = (string)Request["U"];
            string salesuserid = Encryptor.Decode(salesuseridencode);
            string industryId = StringUtility.CheckNull(Request["IndustryId"]);
            //Feed Body
            ForumResponse forumResponse = new ForumResponse();
            forumResponse.EntityId = entityid;
            //forumResponse.EntityType = DomainObjectType.Project;
            forumResponse.EntityType = objecttype;
            forumResponse.Response = StringUtility.CheckNull(Request["Comment"]);

            SetDefaultDataForSave(forumResponse, salesuserid);

            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

            // ActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project);
            if (!string.IsNullOrEmpty(industryId))
            {
                decimal indId = Decimal.Parse(industryId);
                ActionHistoryService.RecordCommentCompetitor(entityid, EntityAction.FeedBack, objecttype, indId, DomainObjectType.Industry, ActionFrom.FrontEnd, salesuserid, forumResponse.ClientCompany);
            }
            else
            {
                ActionHistoryService.RecordActionHistory(entityid, EntityAction.FeedBack, objecttype, ActionFrom.FrontEnd, salesuserid, forumResponse.ClientCompany);
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateFeedBack(FormCollection form)
        {
            ForumResponse forumResponse = new ForumResponse();
            decimal id = decimal.Parse((string)form["EntityId"]);
            string ObjectType = (string)form["ObjectType"];
            string salesuseridencode = (string)Request["U"];
            string salesuserid = Encryptor.Decode(salesuseridencode);

            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Project;
            forumResponse.Response = StringUtility.CheckNull(form["txtComment"]);

            SetDefaultDataForSave(forumResponse, salesuserid);

            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

            ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project, ActionFrom.FrontEnd, salesuserid, forumResponse.ClientCompany);

            return null;
        }

        public ActionResult Rating(string ProjectId, string Rating)
        {
            if (ProjectId != null && Rating != null)
            {
                string salesuseridencode = (string)Request["U"];
                string salesuserid = Encryptor.Decode(salesuseridencode);
                UserProfile User = UserProfileService.GetById(salesuserid);
                ProjectService.SaveRating(decimal.Parse(ProjectId), decimal.Parse(Rating), User);
                ActionHistoryService.RecordActionHistory(decimal.Parse(ProjectId), EntityAction.SetedRating, DomainObjectType.Project, ActionFrom.FrontEnd, salesuserid, User.ClientCompany);
            }
            return Content(string.Empty);
        }
        // FrontEnd: Salesforce
        public ActionResult CreateResponse(decimal EntityId, decimal ForumResponseId, string ObjectType, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            string salesuseridencode = (string)Request["U"];
            string salesuserid = Encryptor.Decode(salesuseridencode);
            ViewData["U"] = salesuseridencode;

            ViewData["EntityId"] = EntityId;
            ViewData["ForumResponseId"] = ForumResponseId;
            ViewData["ResponseText"] = ForumResponseId == 0 ? string.Empty : ForumResponseService.GetById(ForumResponseId).Response;
            ViewData["DomainObjectType"] = ObjectType;
            ViewData["IndustryId"] = IndustryId;
            ViewData["CriteriaId"] = CriteriaId;
            ViewData["EntityIdT"] = EntityIdT;
            return View("Response");
        }

        private void SetDefaultDataForSave(DomainObject<decimal> entity,string userid)
        {
            entity.CreatedDate = DateTime.Today.Date;
            entity.LastChangedDate = DateTime.Today.Date;
            UserProfile UserProfile=UserProfileService.GetById(userid);
            entity.CreatedBy = userid;
            entity.LastChangedBy = userid;
            entity.ClientCompany = UserProfile.ClientCompany;
            //entity.Id = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateResponse(FormCollection form)
        {
            string salesuseridencode = (string)Request["U"];
            string salesuserid = Encryptor.Decode(salesuseridencode);

            string IndustryId = (string)form["IndustryId"];
            string CriteriaId = (string)form["CriteriaId"];
            string EntityIdT = (string)form["EntityIdT"];
            string ObjectType = (string)Request["DomainObjectType"];
            decimal EntityId = decimal.Parse(form["EntityId"]);
            if (IndustryId == "" && CriteriaId == "" && EntityIdT == "")
            {

                //for XSS
                string ForumResponse = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Response"])); ;
                decimal ForumResponseId = decimal.Parse(form["ForumResponseId"]);


                if (ForumResponse.Length == 0) //Temporally
                    return Content(string.Empty);

                Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
                if (forum == null)
                {
                    forum = new Forum();
                    forum.EntityId = EntityId;
                    forum.EntityType = ObjectType;
                    forum.Status = ForumStatus.Enabled;
                    forum.Type = ForumType.Comment;
                    SetDefaultDataForSave(forum, salesuserid);
                    ForumService.Save(forum);
                }


                ForumResponse forumresponse = new ForumResponse();
                SetDefaultDataForSave(forumresponse, salesuserid);
                //forumresponse.Id = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                forumresponse.ForumId = forum.Id;
                forumresponse.Response = ForumResponse;

                Library NuggetLibrary = new Library();

                forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty,salesuserid,forumresponse.ClientCompany);

                if (ForumResponseId != 0)
                {
                    ForumResponse oldforumresponse = ForumResponseService.GetById(ForumResponseId);
                    forumresponse.ParentResponseId = oldforumresponse.Id;
                }

                forumresponse.ParentResponseId = forumresponse.ParentResponseId ?? 0;

                ForumResponseService.Save(forumresponse);
                EmailService.SendCommentEmail(forumresponse.CreatedBy, forum.Subject, ObjectType, EntityId, salesuserid, forumresponse.Response, forumresponse.ClientCompany, forumresponse.Libraries);
                ActionHistoryService.RecordActionHistory(EntityId, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                //return Content(string.Empty);
                return Content("<script> self.close(); </script>");

            }
            else
            {
                ProductCriteriaId productCriteriaId2 = new ProductCriteriaId(decimal.Parse(EntityIdT), decimal.Parse(CriteriaId), decimal.Parse(IndustryId));

                decimal ForumResponseId = decimal.Parse(form["ForumResponseId"]);
                //for XSS
                string ForumResponse = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Response"])); ;

                if (ObjectType.Equals(DomainObjectType.ProductCriteria))
                {
                    if (IndustryId != null && CriteriaId != null && EntityIdT != null)
                    {
                        ProductCriteriaId productCriteriaId = new ProductCriteriaId(decimal.Parse(EntityIdT), decimal.Parse(CriteriaId), decimal.Parse(IndustryId));
                        ProductCriteria productCriteria = ProductCriteriaService.GetById(productCriteriaId);
                        if (productCriteria == null)
                        {
                            productCriteria = new ProductCriteria(decimal.Parse(EntityIdT), decimal.Parse(CriteriaId), decimal.Parse(IndustryId), EntityId);
                            productCriteria.CreatedBy = CurrentUser;
                            productCriteria.CreatedDate = DateTime.Now;
                            productCriteria.LastChangedBy = CurrentUser;
                            productCriteria.LastChangedDate = DateTime.Now;
                            productCriteria.ClientCompany = CurrentCompany;
                            ProductCriteriaService.Save(productCriteria);

                        }
                        Forum forumT = ForumService.GetByEntityId(decimal.Parse(EntityIdT), DomainObjectType.Product, ForumType.Comment);
                        if (forumT == null)
                        {
                            forumT = new Forum();
                            forumT.EntityId = decimal.Parse(EntityIdT);
                            forumT.EntityType = DomainObjectType.Product;
                            forumT.Status = ForumStatus.Enabled;
                            forumT.Type = ForumType.Comment;
                            SetDefaultDataForSave(forumT, salesuserid);
                            ForumService.Save(forumT);

                        }
                        if (ForumResponse.Length == 0) //Temporally
                            return Content(string.Empty);
                        ForumResponse forumresponseT = new ForumResponse();
                        SetDefaultDataForSave(forumresponseT, salesuserid);
                        forumresponseT.ForumId = forumT.Id;
                        forumresponseT.Response = ForumResponse;

                        Library NuggetLibraryT = new Library();

                        forumresponseT.Libraries = GetLibrariesForEntity(forumresponseT.Id, ObjectType, string.Empty);

                        if (ForumResponseId != 0)
                        {
                            ForumResponse oldforumresponse = ForumResponseService.GetById(ForumResponseId);
                            forumresponseT.ParentResponseId = oldforumresponse.Id;
                        }

                        forumresponseT.ParentResponseId = forumresponseT.ParentResponseId ?? 0;

                        ForumResponseService.Save(forumresponseT);
                    }
                }
                if (ForumResponse.Length == 0) //Temporally
                    return Content(string.Empty);

                Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
                if (forum == null)
                {
                    forum = new Forum();
                    forum.EntityId = EntityId;
                    forum.EntityType = ObjectType;
                    forum.Status = ForumStatus.Enabled;
                    forum.Type = ForumType.Comment;
                    SetDefaultDataForSave(forum, salesuserid);
                    ForumService.Save(forum);
                }


                ForumResponse forumresponse = new ForumResponse();
                SetDefaultDataForSave(forumresponse, salesuserid);
                forumresponse.ForumId = forum.Id;
                forumresponse.Response = ForumResponse;

                Library NuggetLibrary = new Library();

                forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty);

                if (ForumResponseId != 0)
                {
                    ForumResponse oldforumresponse = ForumResponseService.GetById(ForumResponseId);
                    forumresponse.ParentResponseId = oldforumresponse.Id;
                }

                forumresponse.ParentResponseId = forumresponse.ParentResponseId ?? 0;

                ForumResponseService.Save(forumresponse);
                EmailService.SendCommentEmail(forumresponse.CreatedBy, forum.Subject, ObjectType, EntityId, salesuserid, forumresponse.Response, forumresponse.ClientCompany, forumresponse.Libraries);
                //ActionHistoryService.RecordActionHistory(EntityId, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);

                ActionHistoryService.CommentsActionHistory(EntityId, productCriteriaId2, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, salesuserid, forumresponse.ClientCompany);
                //return Content(string.Empty);
                return Content("<script> self.close(); </script>");
            }
        }   
        //
        //Survey Navigation
        //
        public ActionResult SurveyContent()
        {

            //Autenticate ClientCompany
            //Validate apikey
            string salesemailencode = (string)Request["U"];
            string apikey = (string)Request["K"];
            string salesclosedtype = (string)Request["T"];
            string salesforcetype = string.Empty;
            if (String.IsNullOrEmpty(salesclosedtype) || (!String.IsNullOrEmpty(salesclosedtype) && salesclosedtype.Trim().Length==0))
                salesforcetype = SurveySalesForceType.ActiveDeal;
            else if( salesclosedtype.Equals("Closed Won") )
                salesforcetype = SurveySalesForceType.Won;
            else if (salesclosedtype.Equals("Closed Lost"))
                salesforcetype = SurveySalesForceType.Lost;

            LOG.Info("SalesForce.Survey: T=" + salesforcetype);
            string salesemail = Encryptor.Decode(StringUtility.CheckNull(salesemailencode));

            string sIndustryId = StringUtility.CheckNull(Request.Params["Industry"]);
            string sCompetitorId = StringUtility.CheckNull(Request.Params["Competitor"]);
            string sProductId = StringUtility.CheckNull(Request.Params["Product"]);

            UserProfile userProfile = UserProfileService.GetByEmail(salesemail);

            if (userProfile == null)
                return Content("Compelligence message: " + salesemail + " does not have access to the Compelligence system.");
            ClientCompany clientCompany = ClientCompanyService.GetById(userProfile.ClientCompany);
            if (clientCompany.Status.Equals(ClientCompanyStatus.Disabled))
            {
                return Content("Your company's subscription has been suspended. Please contact your system administrator or <b>support@compelligence.com</b> for assistance.");
            }
            
            string salesclientcompany = userProfile.ClientCompany;
            string salesclientcompanyencode = Encryptor.Encode(salesclientcompany);

            return RedirectToAction("Survey", new { U = Encryptor.Encode(userProfile.Id), C = salesclientcompanyencode, T=salesforcetype });
        }

        public ActionResult Survey()
        {
            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            string salesforcetype = (string)Request["T"];
            string salesuserid = string.Empty;

            try
            {
                salesuserid = Encryptor.Decode(StringUtility.CheckNull(salesuseridencode)); 
            }
            catch (Exception ex)
            {
                return Content("Compelligence message: unknown error, does not have access to the Compelligence system.");
            }

            string salesclientcompany = Encryptor.Decode(salesclientcompanyencode);

            UserProfile up = UserProfileService.GetById(salesuserid);

            IList<Quiz> survey = QuizService.GetBySalesForceType(QuizType.Survey, SurveyType.SalesForce, salesforcetype, SurveyStatus.Complete, up.ClientCompany);
            if (survey == null || (survey.Count == 0))
                survey = QuizService.GetBySalesForceType(QuizType.Survey, SurveyType.SalesForce, SurveySalesForceType.Html, SurveyStatus.Complete, up.ClientCompany);
            IList<Quiz> completesurvey = new List<Quiz>();
            foreach (Quiz q in survey)
            {
                q.Questions = QuestionService.GetByQuizId(q.Id);
                completesurvey.Add(q);
            }

            if (survey == null || (survey != null && survey.Count <= 0))
                ViewData["LongSurvey"] = null;
            else
                ViewData["LongSurvey"] = survey[0];

            ViewData["U"] = salesuseridencode;
            ViewData["C"] = Encryptor.Encode(up.ClientCompany);

            return View("Survey");
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Answer(decimal id, FormCollection form)
        {
            string salesuseridencode = (string)form["U"];
            string salesclientcompanyencode = (string)form["C"];
            string salesuserid = Encryptor.Decode(salesuseridencode);
            string salesclientcompany = Encryptor.Decode(salesclientcompanyencode);

            UpdateDropDownWith("Industry", IndustryService.FindIndustryHierarchy(salesclientcompany));
            UpdateDropDownWith("Competitor", new List<Competitor>());
            UpdateDropDownWith("Product", new List<Product>());

            Quiz quiz = QuizService.GetById(id);

            ViewData["LongSurvey"] = quiz;

            IList<Answer> answers = new List<Answer>();

            decimal uniqueKey = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();

            bool isEmptyAnswer = false;

            foreach (Question question in quiz.Questions)
            {
                Answer answer = new Answer();

                answer.QuizResponseId = uniqueKey;
                answer.QuizId = question.QuizId;
                answer.QuestionId = question.Id;
                answer.AnswerText = form["Q" + question.Item];

                ViewData["Q" + question.Item] = form["Q" + question.Item];

                if (string.IsNullOrEmpty(answer.AnswerText))
                {
                    isEmptyAnswer = true;
                }

                answer.Author = salesuserid;

                answer.CreatedBy = salesuserid;
                answer.CreatedDate = DateTime.Now;
                answer.LastChangedBy = salesuserid;
                answer.LastChangedDate = DateTime.Now;

                answers.Add(answer);
            }

            quiz.Libraries = GetLibrariesForEntity(quiz.Id, DomainObjectType.Survey, string.Empty, salesuserid, salesclientcompany);

            quiz.UserId = salesuserid;
            quiz.UserName = UserProfileService.GetById(salesuserid).Name;

            if (!isEmptyAnswer)
            {
                QuizService.SaveAnswers(quiz, answers);
                //ActionHistory(quiz.Id, EntityAction.Answered, quiz.TargetType);
                ViewData["SurveyTitle"] = quiz.Title;
                ViewData["Message"] = "Thank you for your submission!";
                ActionHistoryService.RecordActionHistory(quiz.Id, EntityAction.Answered, DomainObjectType.Survey, ActionFrom.FrontEnd, salesuserid, salesclientcompany);
                return View("Survey");
            }
            else
            {
                ViewData["LongSurvey"] = quiz;
                ViewData["ErrorMessage"] = LabelResource.QuestionAnswerIsEmpty;
                return View("Survey");
            }

        }


        //Get AllContentType and Inject Library,Competitor or Product
        private IList<LibraryCatalog> GetContentTypeCatalog(string salesCurrentUser, string salesCurrentCompany, params decimal[] SchemeIds)
        {

            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            IList<WebsiteDetail> ActiveConfigDetails = new List<WebsiteDetail>();
            IList<ContentType> ContentTypes = ContentTypeService.GetAllActiveByClientCompany(salesCurrentCompany);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitive Messaging", salesCurrentCompany, salesCurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Positioning Statements", salesCurrentCompany, salesCurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Details Industry Competitor Product", salesCurrentCompany, salesCurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "News", salesCurrentCompany, salesCurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitors in Industry", salesCurrentCompany, salesCurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Products in Industry", salesCurrentCompany, salesCurrentUser);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Industries", salesCurrentCompany, salesCurrentUser);

            ContentTypeService.CreateIfNotExist(ContentTypes, "Strengths/Weaknesses", salesCurrentCompany, salesCurrentUser);

            //Disable Strengths/Weaknesses
            //ContentTypeService.CreateIfNotExist(ContentTypes, "Strengths/Weaknesses", salesCurrentCompany, salesCurrentUser);

            Website ActiveConfig = WebsiteService.GetActiveWebsite(salesCurrentCompany);

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
                            File file = FileService.GetByEntityId(ActiveProject.Id, DomainObjectType.Project);
                            ActiveProject.File = file;
                            ActiveProject.Labels = LabelService.GetByEntityId(ActiveProject.Id);
                            ActiveProject.RatingAllowed = ProjectService.RatingAllowed(ActiveProject.Id, salesCurrentUser);
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
                                oLibraryCatalog.Position = PositioningService.GetPositiongAndParent(PositioningRelation.Positioning, DomainObjectType.Competitor, SchemeIds[1], SchemeIds[0], salesCurrentCompany);
                                PositioningIndustryId positioningIndustryId = new PositioningIndustryId(oLibraryCatalog.Position.Id, SchemeIds[0]);
                                PositioningIndustry pi = PositioningIndustryService.GetById(positioningIndustryId);

                                if (oLibraryCatalog.Position != null && (oLibraryCatalog.Position.EntityId != SchemeIds[1] || pi == null)) oLibraryCatalog.Position.IsGlobal = "Y";
                                ViewData["Positionig"] = oLibraryCatalog.Position;
                                break;
                            case 3:
                                oLibraryCatalog.Position = PositioningService.GetPositiongByProductAndParent(PositioningRelation.Positioning, DomainObjectType.Product, SchemeIds[2], SchemeIds[1], SchemeIds[0], salesCurrentCompany);
                                if (oLibraryCatalog.Position.Id == 0 || string.IsNullOrEmpty(oLibraryCatalog.Position.CreatedBy))
                                {
                                    oLibraryCatalog.Position = PositioningService.GetPositiongAndParent(PositioningRelation.Positioning, DomainObjectType.Competitor, SchemeIds[1], SchemeIds[0], salesCurrentCompany);
                                }
                                PositioningIndustryId piId = new PositioningIndustryId(oLibraryCatalog.Position.Id, SchemeIds[0]);
                                PositioningIndustry piT = PositioningIndustryService.GetById(piId);
                                if (oLibraryCatalog.Position != null && (oLibraryCatalog.Position.EntityId != SchemeIds[2] || piT == null)) oLibraryCatalog.Position.IsGlobal = "Y";
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
                                Competitor competitor = CompetitorService.GetCompetitorClient(salesCurrentCompany);
                                if (competitor != null && competitor.Id != SchemeIds[1])
                                {
                                    //oLibraryCatalog.CompetitiveMes = PositioningService.GetByCompetitorId(SchemeIds[1], SchemeIds[0], competitor.Id, DomainObjectType.Competitor, PositioningRelation.CompetitiveMessaging, PositioningStatus.Enabled, salesCurrentCompany);
                                    oLibraryCatalog.CompetitiveMes = PositioningService.GetCompetitiveMessaginAndParent(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, competitor.Id, SchemeIds[1], SchemeIds[0], salesCurrentCompany);
                                    PositioningCompetitorId positioningCompetitorId = new PositioningCompetitorId(oLibraryCatalog.CompetitiveMes.Id, SchemeIds[1]);
                                    PositioningCompetitor pc = PositioningCompetitorService.GetById(positioningCompetitorId);
                                    PositioningIndustryId positioningIndustryId = new PositioningIndustryId(oLibraryCatalog.CompetitiveMes.Id, SchemeIds[0]);
                                    PositioningIndustry pi = PositioningIndustryService.GetById(positioningIndustryId);
                                    if (oLibraryCatalog.CompetitiveMes != null && ( pc==null || pi==null )) oLibraryCatalog.CompetitiveMes.IsGlobal = "Y";
                                    ViewData["Competitive Messaging"] = oLibraryCatalog.CompetitiveMes;
                                }
                                else
                                {
                                    ViewData["Competitive Messaging"] = null;
                                }
                                break;
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
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Industry, SchemeIds[0], salesCurrentCompany); break;
                            case 2: //Industry+Competitor
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Competitor, SchemeIds[1], salesCurrentCompany); break;
                            case 3://Industry+Competitor+Product
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Product, SchemeIds[2], salesCurrentCompany); break;
                        }


                    }
                    if (ActiveContentType.Name.Equals("Strengths/Weaknesses"))
                    {
                        ViewData["SWcss"] = oLibraryCatalog.CssClass;
                    }
                    //Disable Strengths/Weaknesses
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
                                oLibraryCatalog.Industry = IndustryService.GetById(SchemeIds[0]);
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
                                oLibraryCatalog.ProductsWithCompetitor = ProductService.GetWithCompetitorByIndustryId(SchemeIds[0], salesCurrentCompany);
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
                                oLibraryCatalog.Industries = IndustryService.GetEnabledByCompetitorId(SchemeIds[1], IndustryStatus.Enabled, salesCurrentCompany);
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
                                oLibraryCatalog.CompetitorFinancialIncomeStatement = CompetitorFinancialService.GetByCompetitorIdAndType((decimal)SchemeIds[1], FinancialTimePeriod.Annual, salesCurrentCompany);
                                break;
                            case 3:
                                break;
                        }
                    }
                    LibraryCatalogCollection.Add(oLibraryCatalog);
                }
            }
            //Process with reference to ProjectId

            return LibraryCatalogCollection;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download()
        {
            decimal ProjectId = decimal.Parse(Request["ProjectId"]);
            string salesuseridencode = (string)Request["U"];
            string salesuserid = Encryptor.Decode(salesuseridencode);


            File file = FileService.GetByEntityId(ProjectId, DomainObjectType.Project);
            string check = StringUtility.CheckNull(Request["chk"]);
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
                    UserProfile user = UserProfileService.GetById(salesuserid);
                    ProjectService.SaveDownload(ProjectId, user);
                    ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, salesuserid, user.ClientCompany);
                    GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
                }
            }
            else
            {
                return Content(file.Description);
            }
            return Content(string.Empty);
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
        private string GetTextOfURL(string url)
        {
            string textUrl = url;
            if (url.IndexOf('_') != -1)
            {
                textUrl = url.Replace('_', '-');
            }

            return textUrl;
        }


        //simillary not overrided
        private IList<Library> GetLibrariesForEntity(decimal entityId, string entityType, string libraryTypeKeyCode, string CurrentUser,string CurrentCompany)
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
                    hpf.SaveAs(System.IO.Path.Combine(pathtemp, physicalname));

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

                    libraries.Add(library);
                }
            }

            return libraries;
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

        public void SetCurrentUser(string userId)
        {
            UserProfile user = UserProfileService.GetById(userId);
            ViewData["User"] = user;
            //ViewData["U"] = userId;
            //ViewData["C"] = user.ClientCompany;
        }
        private IList<string> GetExternalCompetitor(decimal competitorId, string ExternalCompetitorType)
        {
            String name = CompetitorService.GetById(competitorId).Name;
            ClientCompany clientcompany = ClientCompanyService.GetById(CurrentCompany);
            IList<string> result = new List<string>();
            if (!string.IsNullOrEmpty(clientcompany.SalesForceUser) && !string.IsNullOrEmpty(clientcompany.SalesForcePassword) && !string.IsNullOrEmpty(clientcompany.SalesForceToken))
                result = SalesForceService.GetSalesforceExternalCompetitors(clientcompany.SalesForceUser, clientcompany.SalesForcePassword + clientcompany.SalesForceToken, name, ExternalCompetitorType);
            return result;
        }
    }
}
