using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Common.Utility;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Util.Validation;
using Compelligence.BusinessLogic.Implementation;
using Resources;

namespace Compelligence.Web.Controllers
{
    public class EmailApproveRegistrationController : GenericController
    {
        private IApprovalListService _approvalListService;
        private IClientCompanyService _clientCompanyService;
        private IUserProfileService _userProfileService;
        private ICountryService _countryService;

        public IHistoryFieldService HistoryFieldService { get; set; }

        public IUserActionFieldService UserActionFieldService { get; set; }

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
        public ISecurityGroupService SecurityGroupService { get; set; }
        public IUserProfileService UserProfileService
        {
            get { return _userProfileService; }
            set { _userProfileService = value; }
        }

        public ICountryService CountryService
        {
            get { return _countryService; }
            set { _countryService = value; }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {

            string clientCompany = Encryptor.Decode(StringUtility.CheckNull(Request["ccy"]));
            string userId = Encryptor.Decode(StringUtility.CheckNull(Request["ued"]));

            string entityType = Encryptor.Decode(StringUtility.CheckNull(Request["ete"]));

            // Validate if exist ClientCompany and UserProfile 
            if ((ClientCompanyService.GetById(clientCompany) != null)
                && (UserProfileService.GetById(userId) != null))
            {
                // If email approval request is for enable new User registration
                if (entityType.Equals(DomainObjectType.User))
                {
                    string newUserId = Encryptor.Decode(StringUtility.CheckNull(Request["nued"]));

                    return ApproveNewUser(newUserId, userId);
                }
            }

            return View("ApproveNewUserError");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ApproveNewUser(string id, string userId)
        {
            if (CurrentUser == userId)
            {
                UserProfile newUser = UserProfileService.GetById(id);
                if (newUser != null)
                {
                    IList<SecurityGroup> securityGroupList = SecurityGroupService.GetUserGroups(newUser.ClientCompany);
                    ViewData["SecurityGroupList"] = new SelectList(securityGroupList, "Id", "Name");
                    ApprovalList approvalObject = ApprovalListService.GetByEntityAndUser(id, DomainObjectType.User, userId, ApprovalListActionType.NewUserRegistration);
                    TimeSpan intervalTime = DateTime.Now.Subtract(approvalObject.CreatedDate);

                    if (intervalTime.Hours < 24)
                    {
                        // Check for link only active 24 hours
                        if (!ApprovalListService.IsApprovedByUser(id, DomainObjectType.User, userId, ApprovalListActionType.NewUserRegistration))
                        {
                            newUser.ApproverId = userId;

                            if (!string.IsNullOrEmpty(newUser.CountryCode))
                            {
                                Country country = CountryService.GetById(newUser.CountryCode);

                                newUser.CountryName = (country != null) ? country.Name : string.Empty;
                            }

                            return View("ApproveNewUser", newUser);
                        }
                    }
                }

                return View("ApproveNewUserError", newUser);
            }
            else
            {
                ViewData["newUserId"] = id;
                return RedirectToAction("LoginToApprovedEmail", "Home", new { id = id, userid = userId });
            }
        }

        public void SetActionUser(UserProfile entity, decimal GroupFieldId, string Action, string Notes, string ApprovalStatus)
        {
            UserProfile user = UserProfileService.GetById(CurrentUser);
            UserActionField userActionField = new UserActionField();
            HistoryField historyField = new HistoryField();
            string approvingStatus = ApprovalListService.ApproveEntity(entity.Id, DomainObjectType.User, ApprovalListActionType.NewUserRegistration, entity.ApproverId, ApprovalStatus, entity.ApprovalNotes);
            string approvedStatus = UserStatus.Enabled;
            if (!approvingStatus.Equals(ApprovalListApproveStatus.Approved))
                approvedStatus = UserStatus.Disabled;

            historyField.FieldName = "Status";
            historyField.OldValue = string.Empty;
            historyField.Value = approvedStatus;
            historyField.EntityType = DomainObjectType.User;
            historyField.ClientCompany = CurrentCompany;
            historyField.GroupField = GroupFieldId;
            historyField.EntityId = entity.Id;
            HistoryFieldService.Save(historyField);

            userActionField.Action = Action;
            userActionField.Id = GroupFieldId;
            userActionField.EntityId = entity.Id;
            userActionField.UserName = entity.FirstName + " " + entity.LastName;
            userActionField.ByWhom = user.FirstName + " " + user.LastName;
            userActionField.Date = DateTime.Now;
            userActionField.ClientCompany = CurrentCompany;
            userActionField.Notes = Notes;
            UserActionFieldService.Save(userActionField);            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ApproveNewUser(string id, UserProfile formUser, FormCollection collection)
        {
            ViewResult viewResult = null;
            UserProfile user = UserProfileService.GetById(id);
            IList<SecurityGroup> securityGroupList = SecurityGroupService.GetUserGroups(user.ClientCompany);
            ViewData["SecurityGroupList"] = new SelectList(securityGroupList, "Id", "Name");
            if (ValidateNewUserApproveFormData(formUser, collection))
            {
                //string action = collection["ApprovalAction"];

                SetFormDataToEntity(user, collection);

                UserProfileService.ApproveNewUser(user, formUser.ApprovalStatus);

                SetActionUser(user, (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey(), UserActionHistory.StatusChanged, collection["ApprovalNotes"], formUser.ApprovalStatus);

                ViewData["ApprovalNewUserPendingList"] = ApprovalListService.GetApprovalUserRegistrationByUserAndStatus(user.ApproverId,
                  ApprovalListApproveStatus.Pending);

                viewResult = View("ApproveNewUserList", user);
            }
            else
            {
                formUser.FirstName = user.FirstName;
                formUser.LastName = user.LastName;
                formUser.Email = user.Email;
                if (!string.IsNullOrEmpty(user.CountryCode))
                {
                    Country country = CountryService.GetById(user.CountryCode);

                    user.CountryName = (country != null) ? country.Name : string.Empty;
                }
                formUser.CountryName = user.CountryName;
                formUser.City = user.City;
                formUser.Department = user.Department;
                formUser.Phone = user.Phone;
                formUser.Fax = user.Fax;

                viewResult = View("ApproveNewUser", formUser);
            }
            

            return viewResult;
        }

        private bool ValidateNewUserApproveFormData(UserProfile formUser, FormCollection collection)
        {
            if (Validator.IsBlankOrNull(formUser.ApprovalNotes))
            {
                ValidationDictionary.AddError("ApprovalNotes", "Please, enter approval notes");
            }
            if (formUser.ApprovalStatus.Equals(ApprovalListApproveStatus.Approved))
            {
                if (Validator.IsBlankOrNull(formUser.SecurityGroupId))
                {
                    ValidationDictionary.AddError("SecurityGroupId", "Please select a User Type before approving User.");
                }
            }

            return ValidationDictionary.IsValid;
        }

    }
}
