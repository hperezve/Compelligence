using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Configuration;

using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility;
using Resources;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Security.Filters;
using Compelligence.Util.Type;

namespace Compelligence.Web.Controllers
{
    public class ForumController : FrontEndFormController<Forum,decimal>
    {
        private IEmailService _emailservice;
        private IUserProfileService _userprofileservice;
        private IProjectService _projectservice;

        private IForumService _forumservice;
        private IEventService _eventservice;
        private IDealService _dealservice;
        private IQuizService _quizservice;
        private IForumResponseService _forumresponseservice;
        private IActionHistoryService _actionHistoryService;

        public IEmailService EmailService {get; set;}
        public IFileService FileService { get; set; }
        public IProductService ProductService { get; set; }
        public IProductCriteriaService ProductCriteriaService { get; set; }

        public IUserProfileService UserProfileService
        {
            get { return _userprofileservice; }
            set { _userprofileservice = value; }
        }

        public IProjectService ProjectService
        {
            get { return _projectservice; }
            set { _projectservice = value; }
        }

        public IForumService ForumService
        {
            get { return _forumservice; }
            set { _forumservice = value; }
        }

        public IEventService EventService
        {
            get { return _eventservice; }
            set { _eventservice = value; }
        }

        public IDealService DealService
        {
            get { return _dealservice; }
            set { _dealservice = value; }
        }

        public IQuizService QuizService
        {
            get { return _quizservice; }
            set { _quizservice = value; }
        }

        public IForumResponseService ForumResponseService
        {
            get { return _forumresponseservice; }
            set { _forumresponseservice = value; }
        }

        public IActionHistoryService ActionHistoryService
        {
            get { return _actionHistoryService; }
            set { _actionHistoryService = value; }
        }

        public IFollowerService FollowerService { get; set; }
        public IEntityFollowerService EntityFollowerService { get; set; }

        #region Override Methods
        protected override void SetDefaultDataByPage()
        {
            ViewData["Entity"] = FrontEndPages.Forum;
            ViewData["TitleHelp"] = "Forum";
        }
        #endregion

        [AuthenticationFilter]
        public ActionResult Index()
        {
            SetLabels();
            SetDefaultDataToLoadPage();
            UserProfile user = UserProfileService.GetById(CurrentUser);
            ViewData["UserProfileInSession"] = user;
            IList<Forum> forums = ForumService.GetByCompany(CurrentCompany, DomainObjectType.ClientCompany, ForumType.Comment);
            ViewData["ObjectType"] = DomainObjectType.Forum;
            ViewData["Forums"] = forums;
            
            ViewData["Scope"] = Request["Scope"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];

            ViewData["ProjectsComments"] = getActiveProjects();
            ViewData["DealsComments"] = getActiveDeals();
            ViewData["EventsComments"] = getActiveEvents();
            ViewData["ProductsComments"]= getActiveProducts();
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            GetDataOfConfiguration(CurrentCompany);
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return RedirectToAction("Index", "Comparinator");
            }
            return View();
        }
        public ActionResult NewForum()
        {
            decimal Id=decimal.Parse((string)Request["Id"]);
            string subject = (string)Request["subject"];
            if ( !string.IsNullOrEmpty(subject) && subject.Length <250)
            {
             Forum forum = ForumService.GetById(Id);
             if (forum == null)
             {
                forum = new Forum((decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey());
                SetDefaultDataForSave(forum);
                forum.EntityType = DomainObjectType.ClientCompany;
                forum.EntityId = CurrentCompany.GetHashCode();
                forum.Subject = subject;
                forum.Type = ForumType.Comment;
                forum.Status = ForumStatus.Enabled;
                ForumService.Save(forum);
             }
            }
            return RedirectToAction("Index");
        }

        [AuthenticationFilter]
        public ActionResult IndexDetail(decimal Id)
        {
            SetLabels();
            Forum forum = ForumService.GetById(Id);
            IList<ForumResponse> forumresponses = null;
            forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Forum);
            foreach (ForumResponse fr in forumresponses)
            {
                fr.HasAccess = ForumResponseService.HasAccess(fr, CurrentUser);
            }
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            ViewData["ObjectType"] = DomainObjectType.Forum;
            ViewData["Comments"] = forumresponses;
            ViewData["Forum"] = forum;

            ViewData["Scope"] = Request["Scope"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            GetDataOfConfiguration(CurrentCompany);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SendComment(decimal id, FormCollection form)
        {
            Forum forum = ForumService.GetById(id);
            ForumResponse forumResponse = new ForumResponse();
            SetDefaultDataForSave(forumResponse);
            forumResponse.ForumId = forum.Id;
            //for XSS
            forumResponse.Response = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Comment"]));

            forumResponse.ParentResponseId = (!string.IsNullOrEmpty(form["ParentResponseId"])) ? Convert.ToDecimal(form["ParentResponseId"]) : 0;
            forumResponse.Libraries = GetLibrariesForEntity(DomainObjectType.Forum, LibraryTypeKeyCode.File);
            ForumResponseService.Save(forumResponse);
            string followersemail = string.Empty;
            IList<string> emails = new List<string>();
            IList<EntityFollower> entityFollowerList = EntityFollowerService.GetByEntityId(forum.Id, CurrentCompany);
            if (entityFollowerList != null && entityFollowerList.Count > 0)
            {
                foreach(EntityFollower  entityFollower in entityFollowerList)
                {
                    Follower follower = FollowerService.GetById(entityFollower.Id.FollowerId);
                    if (follower != null)
                    {
                        //if (!string.IsNullOrEmpty(followersemail))
                        //{
                        //    followersemail += ",";
                        //}
                        //followersemail += follower.Email;
                        emails = AddEmailToList(follower.Email, emails);
                    }
                }
                if (emails.Count>0)
                {
                    EmailService.SendThreatCommentEmail(emails, forum.CreatedBy, forum.Subject, forum.EntityType, id, CurrentUser, forumResponse.Response, CurrentCompany, forumResponse.Libraries);
                }
            }

            //EmailService.SendCommentEmail(forumResponse.CreatedBy, forum.Name, DomainObjectType.Deal, id, CurrentUser, forumResponse.Response, CurrentCompany, forumResponse.Libraries);

            //ActionHistory(id, EntityAction.Commented, DomainObjectType.Forum);
            //ActionHistoryService.RecordActionHistory(id, EntityAction.Commented, DomainObjectType.Deal, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);

            return RedirectToAction("IndexDetail", new { Id = id });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download(decimal id)
        {
            File file = FileService.GetByEntityId(id, DomainObjectType.Library);
            string check = StringUtility.CheckNull(Request["chk"]);

            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            
            if (file == null)
                return Content("NotFound");

            if (check.ToLower().Equals("true"))
            {
                if ((file != null) && System.IO.File.Exists(fullpath + file.PhysicalName))
                    return Content("Found");
                else
                    return Content("NotFound");
            }
            else
            {
                //ActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File);
                //ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
            }

            return Content(string.Empty);
      
        }
        // FrontEnd: Get Comments [Comparinator, FrontEnd]
        //public ActionResult GetComments(decimal EntityId, string ObjectType)
        //{
        //    return GetComments(EntityId, ObjectType, null, null, null);
        //}
        //public ActionResult GetComments(decimal EntityId, string ObjectType, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        //{
        //    Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
        //    IList<ForumResponse> forumresponses = null;
        //    forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);
        //    foreach (ForumResponse fr in forumresponses)
        //    {
        //        fr.HasAccess = ForumResponseService.HasAccess(fr, CurrentUser);
        //    }
        //    ViewData["ObjectType"] = ObjectType;
        //    ViewData["Comments"] = forumresponses;
        //    ViewData["EntityId"] = EntityId;
        //    ViewData["Scope"] = Request["Scope"];
        //    ViewData["HeaderType"] = Request["HeaderType"];
        //    ViewData["DetailFilter"] = Request["DetailFilter"];
        //    ViewData["IndustryId"] = IndustryId;
        //    ViewData["CriteriaId"] = CriteriaId;
        //    ViewData["EntityIdT"] = EntityIdT;
        //    return View("Comments");
        //}

        public ActionResult GetComments(decimal EntityId, string ObjectType)//, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salesuserid) ? CurrentCompany : salescompanyid;

            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment); //Forum 
            IList<ForumResponse> forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);

            foreach (ForumResponse fr in forumresponses)
            {
                fr.HasAccess = ForumResponseService.HasAccess(fr, localuserid);
            }
            ViewData["ObjectType"] = ObjectType;
            ViewData["Comments"] = forumresponses;
            ViewData["EntityId"] = EntityId;
            ViewData["Scope"] = Request["Scope"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["U"] = (string)Request["U"];
            ViewData["C"] = (string)Request["C"];
            ViewData["ProductId"] = string.Empty;
            ViewData["IndustryId"] = string.Empty;
            GetDataOfConfiguration(localcompanyid); //it's wrong
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return Content(String.Empty);
            }
            return View("Comments");
        }
       
        // FrontEnd: Edit Comment [Comparinator, FrontEnd]
        public ActionResult CommentsResponse(decimal EntityId, decimal ForumResponseId, string ObjectType, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];

            ViewData["EntityId"] = EntityId;
            ViewData["ForumResponseId"] = ForumResponseId;
            ViewData["ResponseText"] = ForumResponseId == 0 ? string.Empty : ForumResponseService.GetById(ForumResponseId).Response;
            ViewData["DomainObjectType"] = ObjectType;
            ViewData["IdComments"] = EntityId;
            if (ObjectType.Equals("SILVER")) ViewData["IdComments"] = "_" + IndustryId + "_" + EntityIdT;
            ViewData["IndustryId"] = IndustryId;
            ViewData["CriteriaId"] = CriteriaId;
            ViewData["EntityIdT"] = EntityIdT;
            ViewData["U"]=salesuseridencode;
            ViewData["C"] = salesclientcompanyencode;
            return View("CommentsResponse");
        }
        //try 
        public ActionResult ExternalResponse(decimal EntityId, decimal ForumResponseId, string ObjectType, decimal? IndustryId, decimal? CriteriaId, decimal? ProductId)
        {
            ViewData["U"] = (string)Request["U"];
            ViewData["C"] = (string)Request["C"];

            ViewData["EntityId"] = EntityId;
            ViewData["ForumResponseId"] = ForumResponseId;
            ViewData["ResponseText"] = ForumResponseId == 0 ? string.Empty : ForumResponseService.GetById(ForumResponseId).Response;
            ViewData["DomainObjectType"] = ObjectType;
            ViewData["IndustryId"] = IndustryId;
            ViewData["CriteriaId"] = CriteriaId;
            ViewData["ProductId"] = ProductId;
            GetDataOfConfiguration(CurrentCompany);
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return null;
            }
            return View("ExternalResponse");
        }

        public ContentResult GetCommentByProduct(string ids, string ObjectType)
        {
            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            string result = "false";
            decimal EntityId = Convert.ToDecimal(ids);

            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
            IList<ForumResponse> forumresponses = null;
            forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);

            string userid = string.Empty;
            string useridencode = (string)Request["U"];
            if (string.IsNullOrEmpty(useridencode)) { userid = CurrentUser; }
            else { userid = Encryptor.Decode(useridencode); }
            foreach (ForumResponse fr in forumresponses)
            {
                fr.HasAccess = ForumResponseService.HasAccess(fr, userid);
                result = "true";
            } 
            
            ViewData["U"] = salesuseridencode;
            ViewData["C"] = salesclientcompanyencode;
            return Content(result);
        }

        public ContentResult GetSilverCommentByProduct(string industryId, string productId, string ObjectType)
        {
            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            string result = "false";
            if (!ObjectType.Equals("SILVER"))
            {


                //Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
                //IList<ForumResponse> forumresponses = null;
                //forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);

                //string userid = string.Empty;
                //string useridencode = (string)Request["U"];
                //if (string.IsNullOrEmpty(useridencode)) { userid = CurrentUser; }
                //else { userid = Encryptor.Decode(useridencode); }
                //foreach (ForumResponse fr in forumresponses)
                //{
                //    fr.HasAccess = ForumResponseService.HasAccess(fr, userid);
                //    result = "true";
                //}
            }
            else
            {
                if (!string.IsNullOrEmpty(industryId) && !string.IsNullOrEmpty(productId))
                {
                    decimal iId = decimal.Parse(industryId);
                    decimal pId = decimal.Parse(productId);
                    IList<ForumResponse> forumresponses = ForumResponseService.GetByEntityResponseAndType(iId, pId, ForumType.Comment);
                    if (forumresponses != null && forumresponses.Count > 0) 
                    {
                        result = "true";
                    }
                }
            }
            ViewData["U"] = salesuseridencode;
            ViewData["C"] = salesclientcompanyencode;
            return Content(result);
        }

        public ContentResult GetDiscussionsByProduct(string ids, string ObjectType)
        {
            string result = "false";
            decimal EntityId = Convert.ToDecimal(ids);
            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Discussion);
            IList<ForumResponse> forumresponses = null;
            forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);

            string userid = string.Empty;
            string useridencode = (string)Request["U"];
            if (string.IsNullOrEmpty(useridencode)) { userid = CurrentUser; }
            else { userid = Encryptor.Decode(useridencode); }
            foreach (ForumResponse fr in forumresponses)
            {
                fr.HasAccess = ForumResponseService.HasAccess(fr, userid);
                result = "true";
            } 

            return Content(result);
        }

        //Utility function for make forum and forumresponse is these not exists.
        public void CreateForumResponseRecord(string EntityId, string ObjectType,decimal responseid, string response,string localclientcompany, string localuserid)
        {
            Forum forum = ForumService.GetByEntityId(decimal.Parse(EntityId), ObjectType, ForumType.Comment);
            if (forum == null)
            {
                forum = new Forum();
                forum.EntityId = decimal.Parse(EntityId);
                forum.EntityType = ObjectType;
                forum.Status = ForumStatus.Enabled;
                forum.Type = ForumType.Comment;
                SetDefaultDataForSave(forum, localclientcompany, localuserid);
                ForumService.Save(forum);

            }
            ForumResponse forumresponse = new ForumResponse();
            SetDefaultDataForSave(forumresponse, localclientcompany, localuserid);
            forumresponse.CreatedDate = DateTime.Now;
            forumresponse.LastChangedDate = DateTime.Now;
            forumresponse.ForumId = forum.Id;
            forumresponse.Response = response;
            forumresponse.ClientCompany = localclientcompany;
            forumresponse.CreatedBy = localuserid;
            forumresponse.LastChangedBy = localuserid;
            forumresponse.Forum = forum; // these values are not stored in the database, only used for other methods
            forumresponse.EntityType = ObjectType;
            forumresponse.EntityId = decimal.Parse(EntityId);
            forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty, localuserid, localclientcompany);
            if (responseid != 0)
            {
                ForumResponse oldforumresponse = ForumResponseService.GetById(responseid);
                forumresponse.ParentResponseId = oldforumresponse.Id;
            }
            forumresponse.ParentResponseId = forumresponse.ParentResponseId ?? 0;
            ForumResponseService.Save(forumresponse);
            EmailService.SendCommentEmail(forumresponse.CreatedBy, forum.Subject, ObjectType, decimal.Parse(EntityId), localuserid, forumresponse.Response, forumresponse.ClientCompany, forumresponse.Libraries);
        }
        
        public ActionResult ExternalResponseSave(FormCollection form)
        {

            string localuserid = string.Empty;
            string localclientcompany = string.Empty;

            string ObjectType = (string)Request["DomainObjectType"];
            decimal EntityId = decimal.Parse(form["EntityId"]);
            string salesuserid = GetDecodeParam(form, "U");
            string salescompanyid = GetDecodeParam(form, "C");
            string IndustryId = (string)form["IndustryId"];
            string CriteriaId = (string)form["CriteriaId"];
            string ProductId = (string)form["ProductId"];

            localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            localclientcompany = string.IsNullOrEmpty(salescompanyid) ? CurrentCompany : salescompanyid;


            //for XSS
            string ForumResponse = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Response"])); ;
            decimal ForumResponseId = decimal.Parse(form["ForumResponseId"]);

            if (String.IsNullOrEmpty(ForumResponse)) //Temporally
                return Content(string.Empty);

            //
            //If not exist forum by entity create that.
            //
            if (!ObjectType.Equals("SILVER"))

                CreateForumResponseRecord(EntityId.ToString(), ObjectType, ForumResponseId, ForumResponse, localclientcompany, localuserid);
            else
            {
                if (!string.IsNullOrEmpty(IndustryId))
                {
                    decimal id = decimal.Parse(IndustryId);
                    decimal productId = decimal.Parse(ProductId);
                    if (id > 0)
                    {
                        //string salesuserid = GetDecodeParam("U");
                        //string salesclientid = GetDecodeParam("C");
                        string localCurrentUser = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
                        string localCurrentCompany = string.IsNullOrEmpty(salescompanyid) ? CurrentCompany : salescompanyid;
                        ForumResponse forumResponse = new ForumResponse();
                        forumResponse.EntityId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey(); //Id temporal no exist in database
                        forumResponse.EntityType = "SILVER";
                        forumResponse.ForumId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey(); //Id temporal no exist in database
                        SetDefaultDataForSaveEntity(forumResponse, localCurrentUser, localCurrentCompany);
                        forumResponse.Response = ForumResponse;
                        if (ForumResponseId != 0)
                        {
                            ForumResponse oldforumresponse = ForumResponseService.GetById(ForumResponseId);
                            forumResponse.ParentResponseId = oldforumresponse.Id;
                        }
                        forumResponse.ParentResponseId = forumResponse.ParentResponseId ?? 0;
                        forumResponse.Libraries = GetLibrariesForEntity(forumResponse.Id, ObjectType, string.Empty, localuserid, localclientcompany);
                        ForumService.SaveForumEntityResponse(forumResponse, id, productId, ForumType.Comment);
                        EmailService.SendCommentEmail(forumResponse.CreatedBy, "Comment", DomainObjectType.Product, productId, localuserid, forumResponse.Response, forumResponse.ClientCompany, forumResponse.Libraries);
                        EmailService.SendCommentEmail(forumResponse.CreatedBy, "Comment", DomainObjectType.Industry, id, localuserid, forumResponse.Response, forumResponse.ClientCompany, forumResponse.Libraries);
                    }
                }
            }
            //
            //First case is for: Result.ascx/Positioning, ObjectType.Product
            //                 SalestoolsResults.ascx, ObjectType.Project
            //                 LeftContent.ascx, ObjectType.Project
            //                 ContentPortal.ascx, ObjectType.Project
            //                 Comments.ascx, ObjectType=ObjectType.Project
            //
            if (String.IsNullOrEmpty(IndustryId) && String.IsNullOrEmpty(CriteriaId) && String.IsNullOrEmpty(ProductId))
            {
                ActionHistoryService.RecordActionHistory(EntityId, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, localuserid, localclientcompany);
            }
            //Second Case is for Comparinator Cells
            else if (!String.IsNullOrEmpty(IndustryId) && !String.IsNullOrEmpty(CriteriaId) && !String.IsNullOrEmpty(ProductId))
            {
                ProductCriteriaId productCriteriaId = new ProductCriteriaId(decimal.Parse(ProductId), decimal.Parse(CriteriaId), decimal.Parse(IndustryId));
                ProductCriteria productCriteria = ProductCriteriaService.GetById(productCriteriaId);
                if (productCriteria == null)
                {
                    productCriteria = new ProductCriteria(decimal.Parse(ProductId), decimal.Parse(CriteriaId), decimal.Parse(IndustryId), EntityId);
                    SetDefaultDataForSave(productCriteria, localclientcompany, localuserid);
                    ProductCriteriaService.Save(productCriteria);
                }
                CreateForumResponseRecord(ProductId, DomainObjectType.Product, ForumResponseId, ForumResponse, localclientcompany, localuserid);
                ActionHistoryService.CommentsActionHistory(EntityId, productCriteriaId, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            }
            if (!ObjectType.Equals("SILVER"))
                ViewData["IdComments"] = EntityId;
            else
                ViewData["IdComments"] = "_"+IndustryId+"_"+ProductId;
            return View("ExternalMessage");
        }      


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveComments(string ObjectType, decimal EntityId, decimal forumresponseid)
        {
            if (!ObjectType.Equals("SILVER"))
            {
                Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
                ForumResponseService.DeleteCascading(forum.Id, forumresponseid);

            }
            else
            {
                ForumResponseService.DeleteEntityResponeCascading(forumresponseid);
            }
            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            ViewData["U"] = salesuseridencode;
            ViewData["C"] = salesclientcompanyencode;
            return Content(string.Empty);
        }

        [ValidateInput(false)]
        public ActionResult CommentsResponseSave(FormCollection form)
        {
            string IndustryId = (string)form["IndustryId"];
            string CriteriaId = (string)form["CriteriaId"];
            string EntityIdT = (string)form["EntityIdT"];
            string ObjectType = (string)Request["DomainObjectType"];
            string UserId = string.Empty; 
            decimal EntityId = decimal.Parse(form["EntityId"]);            
            string clientcompany = string.Empty;          

            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];
            //string salesuserid = Encryptor.Decode(salesuseridencode);
            //string salesclientcompany = Encryptor.Decode(salesclientcompanyencode);
            if (String.IsNullOrEmpty(salesuseridencode) && String.IsNullOrEmpty(salesclientcompanyencode)) //no salesforce
            {
                UserId = (string)Session["UserId"];
                clientcompany = CurrentCompany;
            }
            else //Assume working using parameter U & C from salesforce
            {
                UserId = Encryptor.Decode(salesuseridencode);
                clientcompany = Encryptor.Decode(salesclientcompanyencode);
            }
            if (!ObjectType.Equals("SILVER"))
            {
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
                        SetDefaultDataForSave(forum);
                        ForumService.Save(forum);
                    }


                    ForumResponse forumresponse = new ForumResponse();
                    SetDefaultDataForSave(forumresponse); //it's problem
                    forumresponse.ForumId = forum.Id;
                    forumresponse.Response = ForumResponse;
                    if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                    {
                        forumresponse.ClientCompany = Encryptor.Decode(salesclientcompanyencode);
                        forumresponse.CreatedBy = Encryptor.Decode(salesuseridencode);
                        forumresponse.LastChangedBy = Encryptor.Decode(salesuseridencode);
                    }

                    Library NuggetLibrary = new Library();
                    if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                        forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty, Encryptor.Decode(salesuseridencode), Encryptor.Decode(salesclientcompanyencode));
                    else
                        forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty);


                    if (ForumResponseId != 0)
                    {
                        ForumResponse oldforumresponse = ForumResponseService.GetById(ForumResponseId);
                        forumresponse.ParentResponseId = oldforumresponse.Id;
                    }

                    forumresponse.ParentResponseId = forumresponse.ParentResponseId ?? 0;

                    ForumResponseService.Save(forumresponse);
                    EmailService.SendCommentEmail(forumresponse.CreatedBy, forum.Subject, ObjectType, EntityId, UserId, forumresponse.Response, forumresponse.ClientCompany, forumresponse.Libraries);
                    ActionHistoryService.RecordActionHistory(EntityId, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                    //return Content(string.Empty);

                    ViewData["IdComments"] = EntityId;
                    return View("ExternalMessage");

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
                                SetDefaultDataForSave(forumT);
                                ForumService.Save(forumT);

                            }
                            if (ForumResponse.Length == 0) //Temporally
                                return Content(string.Empty);
                            ForumResponse forumresponseT = new ForumResponse();
                            SetDefaultDataForSave(forumresponseT);
                            forumresponseT.ForumId = forumT.Id;
                            forumresponseT.Response = ForumResponse;
                            if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                            {
                                forumresponseT.ClientCompany = Encryptor.Decode(salesclientcompanyencode);
                                forumresponseT.CreatedBy = Encryptor.Decode(salesuseridencode);
                                forumresponseT.LastChangedBy = Encryptor.Decode(salesuseridencode);
                            }
                            Library NuggetLibraryT = new Library();

                            //forumresponseT.Libraries = GetLibrariesForEntity(forumresponseT.Id, ObjectType, string.Empty);
                            if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                                forumresponseT.Libraries = GetLibrariesForEntity(forumresponseT.Id, ObjectType, string.Empty, Encryptor.Decode(salesuseridencode), Encryptor.Decode(salesclientcompanyencode));
                            else
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
                        SetDefaultDataForSave(forum);
                        ForumService.Save(forum);
                    }


                    ForumResponse forumresponse = new ForumResponse();
                    SetDefaultDataForSave(forumresponse);
                    forumresponse.ForumId = forum.Id;
                    forumresponse.Response = ForumResponse;
                    if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                    {
                        forumresponse.ClientCompany = Encryptor.Decode(salesclientcompanyencode);
                        forumresponse.CreatedBy = Encryptor.Decode(salesuseridencode);
                        forumresponse.LastChangedBy = Encryptor.Decode(salesuseridencode);
                    }
                    Library NuggetLibrary = new Library();

                    //forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty);
                    if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                        forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty, Encryptor.Decode(salesuseridencode), Encryptor.Decode(salesclientcompanyencode));
                    else
                        forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty);


                    if (ForumResponseId != 0)
                    {
                        ForumResponse oldforumresponse = ForumResponseService.GetById(ForumResponseId);
                        forumresponse.ParentResponseId = oldforumresponse.Id;
                    }

                    forumresponse.ParentResponseId = forumresponse.ParentResponseId ?? 0;

                    ForumResponseService.Save(forumresponse);
                    EmailService.SendCommentEmail(forumresponse.CreatedBy, forum.Subject, ObjectType, EntityId, UserId, forumresponse.Response, forumresponse.ClientCompany, forumresponse.Libraries);
                    //ActionHistoryService.RecordActionHistory(EntityId, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);

                    ActionHistoryService.CommentsActionHistory(EntityId, productCriteriaId2, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                    //return Content(string.Empty);
                    ViewData["IdComments"] = EntityId;
                    return View("ExternalMessage");
                }
            }
            else {
                decimal id = decimal.Parse(IndustryId);
                decimal productId = decimal.Parse(EntityIdT);
                string ForumResponse = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Response"])); ;
                if (id > 0)
                {
                    //string salesuserid = GetDecodeParam("U");
                    //string salesclientid = GetDecodeParam("C");
                    //string localCurrentUser = string.IsNullOrEmpty(UserId);
                    //string localCurrentCompany = string.IsNullOrEmpty(salescompanyid) ? CurrentCompany : salescompanyid;
                    ForumResponse forumResponse = new ForumResponse();
                    forumResponse.EntityId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey(); //Id temporal no exist in database
                    forumResponse.EntityType = "SILVER";
                    forumResponse.ForumId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey(); //Id temporal no exist in database
                    SetDefaultDataForSaveEntity(forumResponse, UserId, clientcompany);
                    forumResponse.Response = ForumResponse;
                    forumResponse.Libraries = GetLibrariesForEntity(forumResponse.Id, ObjectType, string.Empty, UserId, clientcompany);
                    ForumService.SaveForumEntityResponse(forumResponse, id, productId, ForumType.Comment);
                    EmailService.SendCommentEmail(forumResponse.CreatedBy, "Comment", DomainObjectType.Product, productId, UserId, forumResponse.Response, forumResponse.ClientCompany, forumResponse.Libraries);
                    EmailService.SendCommentEmail(forumResponse.CreatedBy, "Comment", DomainObjectType.Industry, id, UserId, forumResponse.Response, forumResponse.ClientCompany, forumResponse.Libraries);
                }
                ViewData["IdComments"] = EntityId;
                return View("ExternalMessage");
            }
        }        


        //
        // FeedBack process
        //
        
        public ActionResult FeedBack(decimal EntityId,string ObjectType,FormCollection form) //For Any Type
        {
            string comment = form["Comment"];
            //ForumService.SaveForumResponse(.SaveFeedBack(EntityId, ObjectType, comment, CurrentUser, CurrentCompany);
           // ActionHistory(EntityId, EntityAction.FeedBack, ObjectType);
            ActionHistoryService.RecordActionHistory(EntityId, EntityAction.FeedBack, ObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            return null;
        }

        private string GetObjectType(string scope)
        {
            string objectType = string.Empty;
            if (scope.Contains("Environment"))
            {
                objectType = scope.Substring(11).ToUpper();
                if (objectType.Equals("OBJECTIVE"))
                {
                    objectType = DomainObjectType.Objective;
                }
                else if (objectType.Equals("INDUSTRY"))
                {
                    objectType = DomainObjectType.Industry;
                }
                else if (objectType.Equals("COMPETITOR"))
                {
                    objectType = DomainObjectType.Competitor;
                }
                else if (objectType.Equals("PRODUCT"))
                {
                    objectType = DomainObjectType.Product;
                }
                else if (objectType.Equals("CUSTOMER"))
                {
                    objectType = DomainObjectType.Customer;
                }
                else if (objectType.Equals("SUPPLIER"))
                {
                    objectType = DomainObjectType.Supplier;
                }
                else if (objectType.Equals("PARTNER"))
                {
                    objectType = DomainObjectType.CompetitorPartner;
                }
            }
            else if (scope.Contains("Workspace"))
            {
                objectType = scope.Substring(9).ToUpper();  //There is dangerous code //Workspacexxxxxx
                if (objectType.Equals("PROJECT")) //replace this with switch service or refactory TagNames
                    objectType = "PROJT";
            }
            else if (scope.Contains("Admin"))
            {
                objectType = scope.Substring(5);
                if (objectType.Equals("Template")) //replace this with switch service or refactory TagNames
                    objectType = DomainObjectType.Template;
            }
            return objectType;
        }
        
        private string[] ScopeToArray(string scope)
        {
            if (scope.Contains("Workspace"))
            {
                return new string[] { "Workspace", scope.Substring(9) };
            }
            else if (scope.Contains("Environment"))
            {
                return new string[] { "Environment", scope.Substring(11) };
            }
            else if (scope.Contains("Admin"))
            {
                return new string[] { "Admin", scope.Substring(5) };
            }
            return new string[] { "Unknow","Unknow"};
        }

        private IList<Project> getActiveProjects() {
            IList<Project> tempProjects = new List<Project>();
            IList<Project> resultProjects = new List<Project>();
           
            tempProjects = ProjectService.GetPublishedProjectsByCompany(CurrentCompany);
                 
            foreach (Project proj in tempProjects)
            {
                // get the Last Post Date
                Forum forum = ForumService.GetByEntityId(proj.Id, DomainObjectType.Project, ForumType.Comment);
                if (forum != null)
                {
                    DateTime dateLastPost = ForumResponseService.GetLastPostDate(forum.Id);
                    proj.LastPostDate = dateLastPost.ToString();
                    Decimal numberReplies = ForumResponseService.GetCountResponsesByForumId(forum.Id);
                    proj.RepliesNumber = numberReplies;
                    proj.ViewsNumber = forum.ViewsCounter;
                }
                else
                {
                    proj.LastPostDate = "Not Commented";
                    proj.RepliesNumber = 0;
                }
                if (proj.CreatedBy != null)
                {
                    UserProfile createdBy = UserProfileService.GetById(proj.CreatedBy);
                    if (createdBy == null)
                        proj.CreatedBy = "";
                    else
                        proj.CreatedBy = createdBy.Name;
                }
                if (proj.ViewsNumber == null)
                {
                    proj.ViewsNumber = 0;
                }
                
                
                
                resultProjects.Add(proj);
            }
            return resultProjects;
        }

        private IList<Event> getActiveEvents()
        {
            IList<Event> tempEvents = new List<Event>();
            IList<Event> resultEvents = new List<Event>();
            tempEvents = EventService.GetEnabledEventsByCompany(CurrentCompany);
            foreach (Event events in tempEvents)
            {
                Forum forum = ForumService.GetByEntityId(events.Id, DomainObjectType.Event, ForumType.Comment);
                IList<ForumResponse> forumresponses = null;
                forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Event);
                events.Comment = "false"; 
                foreach (ForumResponse fr in forumresponses)
                {
                    events.Comment = "true";
                }
                if (events.CreatedBy != null)
                {
                    UserProfile createdBy = UserProfileService.GetById(events.CreatedBy);
                    if (createdBy == null)
                        events.CreatedBy = "";
                    else
                        events.CreatedBy = createdBy.Name;
                }
                resultEvents.Add(events);
            }
            return resultEvents;
        }

        private IList<Deal> getActiveDeals()
        {
            IList<Deal> tempDeals = new List<Deal>();
            IList<Deal> resultDeals = new List<Deal>();
            tempDeals = DealService.GetEnabledDealsByCompany(CurrentCompany);
            foreach (Deal deal in tempDeals)
            {
                Forum forum = ForumService.GetByEntityId(deal.Id, DomainObjectType.Deal, ForumType.Comment);
                IList<ForumResponse> forumresponses = null;
                forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Deal);
                deal.CheckComments = false;
                foreach (ForumResponse fr in forumresponses)
                {
                    deal.CheckComments = true;     
                }
                if (deal.CreatedBy != null)
                {
                    UserProfile createdBy = UserProfileService.GetById(deal.CreatedBy);
                    if (createdBy == null)
                        deal.CreatedBy = "";
                    else
                        deal.CreatedBy = createdBy.Name;
                }
                resultDeals.Add(deal);
            }
            return resultDeals;
        }

        private IList<Product> getActiveProducts()
        {
            IList<Product> tempProducts = new List<Product>();
            IList<Product> resultProducts = new List<Product>();
            
            tempProducts = ProductService.GetProductWithForum(CurrentCompany);
            //tempProducts = ProductService.GetAllActiveByClientCompany(CurrentCompany);
            foreach (Product product in tempProducts)
            {
                Forum forum = ForumService.GetByEntityId(product.Id, DomainObjectType.Product, ForumType.Comment);
                IList<ForumResponse> forumresponses = null;
                forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Product);
                product.HasComment = false;
                foreach (ForumResponse fr in forumresponses)
                {
                    product.HasComment = true;
                }               
                if (product.CreatedBy != null)
                {
                    UserProfile createdBy = UserProfileService.GetById(product.CreatedBy);
                    if (createdBy == null)
                        product.CreatedBy = "";
                    else
                        product.CreatedBy = createdBy.Name;
                }
                resultProducts.Add(product);
            }
            return resultProducts;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SendFeedBack(decimal id, FormCollection form)
        {
            ForumResponse forumResponse = new ForumResponse();

            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Project;
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            //for XSS
            forumResponse.Response = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Comment"]));

            forumResponse.ClientCompany = CurrentCompany;

            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

            // ActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project);
            ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Project, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);

            return null;
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegisterFollower(decimal id, FormCollection form)
        {
            //string idtempo = Request["Id"];
            string idtempo = StringUtility.CheckNull(form["EntityId"]);
            decimal entityid = decimal.Parse(idtempo);
            Follower follower = new Follower();
            string name = string.Empty;
            string email = string.Empty;
            string operation = string.Empty;
            name = StringUtility.CheckNull(form["Name"]);
            email = StringUtility.CheckNull(form["Email"]);
            operation = StringUtility.CheckNull(form["Operation"]);


            if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(name))
            {
                if (operation.Equals("Subscribe"))
                {
                    follower.Email = email;
                    follower.Name = name;
                    follower.CreatedBy = CurrentUser;
                    follower.CreatedDate = DateTime.Now;
                    follower.LastChangedBy = CurrentUser;
                    follower.LastChangedDate = DateTime.Now;
                    follower.ClientCompany = CurrentCompany;
                    FollowerService.Save(follower);
                    if (entityid != null)
                    {
                        Forum forum = ForumService.GetById(entityid);
                        if (forum != null)
                        {
                            SaveNewEntityFollowerByFollower(entityid, forum.EntityType, follower);
                        }
                    }
                }
                else if (operation.Equals("Unsubscribe"))
                {
                    IList<EntityFollower> entityFollowerList = EntityFollowerService.GetByEntityId(entityid, CurrentCompany);
                    if (entityFollowerList != null && entityFollowerList.Count > 0)
                    {
                        foreach (EntityFollower entityFollower in entityFollowerList)
                        {
                            Follower followerTempo = FollowerService.GetById(entityFollower.Id.FollowerId);
                            if (followerTempo != null)
                            {
                                if (followerTempo.Email.Equals(email))
                                {
                                    EntityFollowerService.Delete(entityFollower);
                                    FollowerService.Delete(followerTempo);
                                }
                            }

                        }
                    }
                }
            }
           
            //return View();
            //return View("Index");
            //return RedirectToAction("Index", "Forum");
            //return null;
            return RedirectToAction("Index");
        }
        private void SaveNewEntityFollowerByFollower(decimal entityid, string entityType, Follower follower)
        {
            EntityFollowerId entityFollowerId = new EntityFollowerId(entityid, follower.Id);
            EntityFollower entityFollower = EntityFollowerService.GetById(entityFollowerId);
            if (entityFollower == null)
            {
                entityFollower = new EntityFollower(entityFollowerId);
                entityFollower.EntityType = entityType;
                entityFollower.CreatedBy = follower.LastChangedBy;
                entityFollower.CreatedDate = DateTime.Now;
                entityFollower.LastChangedBy = follower.LastChangedBy;
                entityFollower.LastChangedDate = DateTime.Now;
                entityFollower.ClientCompany = follower.ClientCompany;
                EntityFollowerService.Save(entityFollower);
            }
        }

        private IList<string> AddEmailToList(string email, IList<string> emailList)
        {
            IList<string> items = emailList;
            if (!items.Contains(email))
            {
                items.Add(email);
            }
            return items;
        }

        public ActionResult GetDiscussions(decimal EntityId, string ObjectType)//, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Discussion);
            IList<ForumResponse> forumresponses = null;
            forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);
            string userid = string.Empty;
            string useridencode = (string)Request["U"];
            if (string.IsNullOrEmpty(useridencode)){userid = CurrentUser;}
            else {userid = Encryptor.Decode(useridencode);}            
            foreach (ForumResponse fr in forumresponses)
            {
                fr.HasAccess = ForumResponseService.HasAccess(fr, userid);
            }
            ViewData["ObjectType"] = ObjectType;
            ViewData["Comments"] = forumresponses;
            ViewData["EntityId"] = EntityId;
            ViewData["Scope"] = Request["Scope"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["U"] = (string)Request["U"];
            ViewData["C"] = (string)Request["C"];
            GetDataOfConfiguration(CurrentCompany);
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return null;
            }
            return View("Discussions");
        }

        public ActionResult DiscussionsResponse(decimal EntityId, decimal ForumResponseId, string ObjectType, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            ViewData["U"] = (string)Request["U"];
            ViewData["C"] = (string)Request["C"];

            ViewData["EntityId"] = EntityId;
            ViewData["ForumResponseId"] = ForumResponseId;
            ViewData["ResponseText"] = ForumResponseId == 0 ? string.Empty : ForumResponseService.GetById(ForumResponseId).Response;
            ViewData["DomainObjectType"] = ObjectType;
            ViewData["IndustryId"] = IndustryId;
            ViewData["EntityIdT"] = EntityIdT;
            GetDataOfConfiguration(CurrentCompany);
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return null;
            }
            return View("DiscussionsResponse");
        }

        public ActionResult DiscussionsResponseSave(FormCollection form)
        {
            string IndustryId = (string)form["IndustryId"];
            //string CriteriaId = (string)form["CriteriaId"];
            string EntityIdT = (string)form["EntityIdT"];
            string ObjectType = (string)Request["DomainObjectType"];
            decimal EntityId = decimal.Parse(form["EntityId"]);

            //Work with UserId and ClientCompany over Parameters for compatibility with SalesForce
            string userid = string.Empty;
            string clientcompany = string.Empty;
          

            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];

            if (String.IsNullOrEmpty(salesuseridencode) && String.IsNullOrEmpty(salesclientcompanyencode)) //no salesforce
            {
                userid = CurrentUser;
                clientcompany = CurrentCompany;
            }
            else //Assume working using parameter U & C from salesforce
            {
                userid = Encryptor.Decode(salesuseridencode);
                clientcompany = Encryptor.Decode(salesclientcompanyencode);
            }

            //for XSS
            string ForumResponse = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Response"])); ;
            decimal ForumResponseId = decimal.Parse(form["ForumResponseId"]);

            if (ForumResponse.Length == 0) //Temporally
                return Content(string.Empty);

            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Discussion);
            if (forum == null)
            {
                forum = new Forum();
                forum.EntityId = EntityId;
                forum.EntityType = ObjectType;
                forum.Status = ForumStatus.Enabled;
                forum.Type = ForumType.Discussion;
                SetDefaultDataForSave(forum);
                ForumService.Save(forum);
            }


            ForumResponse forumresponse = new ForumResponse();
            SetDefaultDataForSave(forumresponse); //it's problem
            forumresponse.ForumId = forum.Id;
            forumresponse.Response = ForumResponse;
            if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
            {
                forumresponse.ClientCompany = Encryptor.Decode(salesclientcompanyencode);
                forumresponse.CreatedBy = Encryptor.Decode(salesuseridencode);
                forumresponse.LastChangedBy = Encryptor.Decode(salesuseridencode);
            } 
            Library NuggetLibrary = new Library();
            if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty, Encryptor.Decode(salesuseridencode), Encryptor.Decode(salesclientcompanyencode));
            else
                forumresponse.Libraries = GetLibrariesForEntity(forumresponse.Id, ObjectType, string.Empty);
            if (ForumResponseId != 0)
            {
                ForumResponse oldforumresponse = ForumResponseService.GetById(ForumResponseId);
                forumresponse.ParentResponseId = oldforumresponse.Id;
            }

            forumresponse.ParentResponseId = forumresponse.ParentResponseId ?? 0;

            ForumResponseService.Save(forumresponse);
            EmailService.SendCommentEmail(forumresponse.CreatedBy, forum.Subject, ObjectType, EntityId, userid, forumresponse.Response, forumresponse.ClientCompany, forumresponse.Libraries);
            if (ObjectType.Equals(DomainObjectType.Competitor) && !string.IsNullOrEmpty(IndustryId))
            {
                //if (StringUtility.  DecimalUtility.IsNumeric(IndustryId))
                //{
                    decimal industryId = Decimal.Parse(IndustryId);
                    ActionHistoryService.RecordCommentCompetitor(EntityId, EntityAction.Commented, ObjectType, industryId, DomainObjectType.Industry, ActionFrom.FrontEnd, userid, clientcompany);
                //}
            }
            else
            {
                ActionHistoryService.RecordActionHistory(EntityId, EntityAction.Commented, ObjectType, ActionFrom.FrontEnd, userid, clientcompany);
            }
            ViewData["IdComments"] = EntityId;
            return View("ExternalMessage");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveDiscussions(string ObjectType, decimal EntityId, decimal forumresponseid)
        {
            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Discussion);
            ForumResponseService.DeleteCascading(forum.Id, forumresponseid);
            return Content(string.Empty);
        }

        public ActionResult FeedBackMessage()
        {
            ViewData["EntityId"] = Request["EntityId"];
            ViewData["EntityType"] = Request["EntityType"];
            ViewData["IndustryId"] = Request["IndustryId"];
            ViewData["U"] = Request["U"];
            ViewData["C"] = Request["C"];
            ViewData["SubmittedVia"] = Request["SubmittedVia"];

            return View("FeedBackMessage");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FeedBackResponse(FormCollection form)
        {

            string entityid = StringUtility.CheckNull(form["hdnEntityId"]);
            string entitytype = StringUtility.CheckNull(form["EntityType"]);

            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salesuserid) ? CurrentCompany : salescompanyid;

            UserProfile user = UserProfileService.GetById(localuserid);

            ForumResponse forumResponse = new ForumResponse();
            forumResponse.EntityId = decimal.Parse(entityid);
            forumResponse.EntityType = entitytype;
            forumResponse.Response = StringUtility.CheckNull(form["txtComment"]);
            SetDefaultDataForSave(forumResponse, localcompanyid, localuserid);
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedDate = DateTime.Now;
            HttpPostedFileBase file = Request.Files["uploadfile"];

            string file2 = form["uploadfile"];
            forumResponse.Libraries = GetLibrariesForEntity(forumResponse.Id, forumResponse.EntityType, string.Empty, localuserid, localcompanyid);
            if (forumResponse.Libraries != null && forumResponse.Libraries.Count > 0)
            {
                foreach (Library library in forumResponse.Libraries)
                {
                    library.Notes = forumResponse.Response;
                    library.SubmittedAction = "FeedBack_" + entitytype + "_" + entityid;
                    string submittedVia = StringUtility.CheckNull(form["SubmittedVia"]);
                    if (!string.IsNullOrEmpty(submittedVia))
                        library.SubmittedVia = submittedVia;
                    library.PEntityId = decimal.Parse(entityid);
                    library.PEntityType = entitytype;
                    library.EntityType = DomainObjectType.ForumResponse; // CHECK TO SET IN THE FUNCTION GetLibrariesForEntity
                }
            }
            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);
            string industryId = StringUtility.CheckNull(form["IndustryId"]);
            if (string.IsNullOrEmpty(industryId))
            {
                ActionHistoryService.RecordActionHistory(decimal.Parse(entityid), EntityAction.FeedBack, entitytype, ActionFrom.FrontEnd, localuserid, localcompanyid);
            }
            else
            {
                decimal indId = Decimal.Parse(industryId);
                ActionHistoryService.RecordCommentCompetitor(decimal.Parse(entityid), EntityAction.FeedBack, forumResponse.EntityType, indId, DomainObjectType.Industry, ActionFrom.FrontEnd, localuserid, localcompanyid);
            }
            string result = string.Empty;
            string resultUnSuccessful = string.Empty;
            foreach (Library library in forumResponse.Libraries)
            {
                if (library.StatusUpload.Equals(LibraryStatusUpload.Successful))
                    result += library.Name + ":";
                else
                    resultUnSuccessful += library.Name + ":";
            }

            result = result.Length > 0 ? result.Substring(0, result.Length - 1) : result;
            resultUnSuccessful = resultUnSuccessful.Length > 0 ? resultUnSuccessful.Substring(0, resultUnSuccessful.Length - 1) : resultUnSuccessful;

            ViewData["ListOfItemsAttached"] = result;
            ViewData["ListOfItemsNoAttached"] = resultUnSuccessful;
            return View("FeedBackResponse");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult FeedBackValidate(FormCollection form)
        {
            string validation = string.Empty;
            string Response = StringUtility.CheckNull(form["txtComment"]);
            if (string.IsNullOrEmpty(Response))
            {
                validation += "<li>Comment is required</li>";
            }
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i].ContentLength == 0)
                    {
                        validation += "<li>" + Request.Files[i].FileName + " no exist</li>";
                    }
                }
            }
            if (!string.IsNullOrEmpty(validation)) { validation = "<ul>" + validation + "</ul>"; }
            //return  ContentResult(validation);
            return new JsonResult() { Data = validation };
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ValidateFile(HttpPostedFileBase file)
        {
            string value = string.Empty;
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    value = "Sucessfull";
                }
                else
                {
                    value = "UnSucessfull";
                }
            }
            return Content(value);
        }


        public ActionResult GetSilverBulletsPublicComments(decimal IndustryId, decimal ProductId, string ObjectType)//, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salesuserid) ? CurrentCompany : salescompanyid;

            IList<ForumResponse> forumresponses = ForumResponseService.GetByEntityResponseAndType(IndustryId, ProductId, ForumType.Comment);

            foreach (ForumResponse fr in forumresponses)
            {
                fr.HasAccess = ForumResponseService.HasAccess(fr, localuserid);
            }
            ViewData["ObjectType"] = ObjectType;
            ViewData["Comments"] = forumresponses;
            ViewData["EntityId"] = IndustryId;
            ViewData["ProductId"] = ProductId;
            ViewData["IndustryId"] = IndustryId;
            ViewData["Scope"] = Request["Scope"];
            ViewData["HeaderType"] = Request["HeaderType"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            ViewData["U"] = (string)Request["U"];
            ViewData["C"] = (string)Request["C"];
            GetDataOfConfiguration(localcompanyid); //it's wrong
            if (!ViewData["DefaultsDisabPublComm"].ToString().Equals("false"))
            {
                return Content(String.Empty);
            }
            return View("Comments");
        }
    }
}
