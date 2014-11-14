using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using Compelligence.Common.Utility;
using Compelligence.Domain.Entity;
using Compelligence.Util.Validation;

namespace Compelligence.Web.Controllers
{
    public class EmailApproveController : GenericController
    {
        private IApprovalListService _approvalListService;
        private IClientCompanyService _clientCompanyService;
        private IUserProfileService _userProfileService;
        private IProjectService _projectService;

        public IApprovalListService ApprovalListService
        {
            get { return _approvalListService; }
            set { _approvalListService = value; }
        }

        public IClientCompanyService ClientCompanyService
        {
            get { return _clientCompanyService; }
            set { _clientCompanyService = value; }
        }

        public IUserProfileService UserProfileService
        {
            get { return _userProfileService; }
            set { _userProfileService = value; }
        }

        public IProjectService ProjectService
        {
            get { return _projectService; }
            set { _projectService = value; }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            string clientCompany = Encryptor.Decode(StringUtility.CheckNull(Request["ccy"]));
            string userId = Encryptor.Decode(StringUtility.CheckNull(Request["ued"]));
            string entityType = Encryptor.Decode(StringUtility.CheckNull(Request["ete"]));
            decimal entityId = Convert.ToDecimal(Encryptor.Decode(StringUtility.CheckNull(Request["etd"])));

            // Validate if exist ClientCompany and UserProfile 
            if ((ClientCompanyService.GetById(clientCompany) != null)
                && (UserProfileService.GetById(userId) != null))
            {

                // If email approval request is for Project and this Project exists,
                // so do a redirection to Project controller
                if (entityType.Equals(DomainObjectType.Project))
                {
                    Project project = ProjectService.GetById((decimal)entityId);
                    if (project != null)
                    {
                        return ApproveProject(entityId, userId);
                    }
                    else
                    {
                        return View("ApproveProjectDeleteError");
                    }
                    //return RedirectToAction("Approve", "Project", new
                    //{
                    //    id = entityId,
                    //    clientCompany = clientCompany,
                    //    userId = userId,
                    //    projectType = entityType
                    //});
                }
            }

            return View("ApproveError");
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult ApproveProject(decimal id, string userId)
        //{
        //    Project project = ProjectService.GetById(id);

        //    if (project != null)
        //    {
        //        ViewData["ApprovalList"] = ApprovalListService.GetByEntity(id.ToString(), DomainObjectType.Project, ApprovalListActionType.ProjectApprove);

        //        if (!ApprovalListService.IsApprovedByUser(id.ToString(), DomainObjectType.Project, userId, ApprovalListActionType.ProjectApprove))
        //        {
        //            project.ApproverId = userId;

        //            return View("ApproveProject", project);
        //        }
        //    }

        //    return View("ApproveProjectError", project);
        //}
        //public ActionResult Index(decimal Projectid)
        //{
        //    string UserId = Session["UserId"].ToString();
        //    return View("ApproveProject", "EmailApprove", new { id = Projectid, userId = UserId });
        //}

        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GoToApproveProject(decimal Projectid)
        {
            string UserId = Session["UserId"].ToString();
            return RedirectToAction("ApproveProject", "EmailApprove", new { id = Projectid, userId = UserId });
            //return RedirectToAction(new { controller="EmailApprove",action= "ApproveProject",  id = Projectid, userId = UserId });
            //return ApproveProject(Projectid, UserId);
            //return View("ApproveProject", "EmailApprove", new { id = Projectid, userId = UserId });
            //return RedirectToRoute("ApproveProject", "EmailApprove", new { id = Projectid, userId = UserId });
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ApproveProject(decimal id, string userId)
        {
            if (CurrentUser == userId)
            {
                Project project = ProjectService.GetById(id);


                if (project != null)
                {
                    ViewData["ApprovalList"] = ApprovalListService.GetByEntity(id.ToString(), DomainObjectType.Project, ApprovalListActionType.ProjectApprove);

                    if (!ApprovalListService.IsApprovedByUser(id.ToString(), DomainObjectType.Project, userId, ApprovalListActionType.ProjectApprove))
                    {
                        project.ApproverId = userId;

                        return View("ApproveProject", project);
                    }
                }

                return View("ApproveProjectError", project);
            }
            else
            {
                ViewData["ProjectId"] = id;
                return RedirectToAction("LoginToApproveProject", "Home", new { id = id, approved = userId });
                //return null; 
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ApproveProject(decimal id, Project formProject, FormCollection collection)
        {
            ViewResult viewResult = null;
            Project projectResult = formProject;
            Project project = ProjectService.GetById(id);

            if (ValidateProjectApproveFormData(formProject, collection))
            {
                //string action = collection["ApprovalAction"];

                SetFormDataToEntity(project, collection);

                ProjectService.Approve(project, formProject.ApprovalStatus);

                projectResult = project;

                ViewData["ApprovalPendingList"] = ApprovalListService.GetApprovalProjectsByUserAndStatus(project.ApproverId,
                    ApprovalListApproveStatus.Pending);
                //ApprovalList approbalList = ApprovalListService.GetByEntityAndUser(project.Id.ToString(), DomainObjectType.Project, CurrentUser, CurrentCompany);
                //if (approbalList != null)
                //{
                //    approbalList.Approved = project.ApprovalStatus;
                //    ApprovalListService.Update(approbalList);
                //}
                viewResult = View("ApproveProjectList", projectResult);
            }
            else
            {
                viewResult = View("ApproveProject", projectResult);
            }

            ViewData["ApprovalList"] = ApprovalListService.GetByEntity(project.Id.ToString(), DomainObjectType.Project, ApprovalListActionType.ProjectApprove);

            return viewResult;
        }

        private bool ValidateProjectApproveFormData(Project project, FormCollection collection)
        {
            if (Validator.IsBlankOrNull(project.ApprovalNotes))
            {
                ValidationDictionary.AddError("ApprovalNotes", "Please, enter approval notes");
            }

            return ValidationDictionary.IsValid;
        }

    }
}
