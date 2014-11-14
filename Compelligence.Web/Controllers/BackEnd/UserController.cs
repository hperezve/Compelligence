using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Resources;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using System.Collections;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Type;
using Compelligence.Security.Managers;
using Compelligence.DataTransfer.Entity;
using System.Globalization;
using System.Text;
using Compelligence.Domain.Entity.Views;
using System.Reflection;
using Compelligence.Common.Utility;


namespace Compelligence.Web.Controllers
{
    public class UserController : BackEndAsyncFormController<UserProfile, string>
    {

        #region Public Properties

        public IUserProfileService UserProfileService
        {
            get { return (IUserProfileService)_genericService; }
            set { _genericService = value; }
        }
        public IHistoryFieldService HistoryFieldService { get; set; }

        public IUserActionFieldService UserActionFieldService { get; set; }

        public IClientCompanyService ClientCompanyService { get; set; }

        public ICountryService CountryService { get; set; }

        public ISecurityGroupService SecurityGroupService { get; set; }

        public IResourceService ResourceService { get; set; }
               
        public IUserRelationService UserRelationService  {   get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CheckMaxNumUsers()
        {
            JsonResult result = new JsonResult();
            int maxNumUsers = 0;
            long numUsers = UserProfileService.CountByCompany(CurrentCompany);
            string message = string.Empty;
            bool isMaxUsers = false;
            ClientCompanySubscription companySubscription = ClientCompanyService.GetSubscriptionByCompany(CurrentCompany);

            if (companySubscription == null)
            {
                companySubscription = ClientCompanyService.CreateDefaultCompanySubscription(CurrentCompany, CurrentUser);
            }

            if (companySubscription != null && companySubscription.MaxUsers != null)
            {
                maxNumUsers = companySubscription.MaxUsers.Value;

                if (numUsers >= maxNumUsers)
                {
                    isMaxUsers = true;
                    message = string.Format(LabelResource.UserCompanyMaxNumError, maxNumUsers);
                }
            }

            result.Data = new { cnumu = numUsers, mnumu = maxNumUsers, imu = isMaxUsers, msg = message };

            return result;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditPassword(string id)
        {
            UserProfile userProfile = UserProfileService.GetById(id);

            SetDefaultRequestParametersToForm(ActionMethod.EditPassword, OperationStatus.Initiated);

            return View(userProfile);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPassword(string id, UserProfile userProfileForm, FormCollection collection)
        {
            try
            {

                OperationStatus operationStatus = OperationStatus.Initiated;
                UserProfile userProfileResult = userProfileForm;
                UserProfile userProfile = UserProfileService.GetById(id);

                if (ValidateEditFormPasswordData(userProfileForm))
                {
                    CompareForm(userProfile, userProfileForm, collection, ActionMethod.EditPassword);

                    SetFormDataToEntity(userProfile, collection);

                    SetDefaultDataForUpdate(userProfile);

                    UserProfileService.UpdatePassword(userProfile);

                    userProfileResult = userProfile;

                    operationStatus = OperationStatus.Successful;

                    return RedirectToAction("Edit", new { id = userProfile.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"], Container = Request["Container"], GroupIdUser = ViewData["GroupIdUser"] });
                }

                SetDefaultRequestParametersToForm(ActionMethod.EditPassword, operationStatus);

                return View(userProfileResult);
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetHistory(decimal Id)
        {

            IList<HistoryField> listHistoryField = HistoryFieldService.GetByGroupField(Id);
            UserActionField userActionField = UserActionFieldService.GetById(Id);

            ViewData["ListHistoryField"] = listHistoryField;
            ViewData["UserActionField"] = userActionField;
            return View("DetailHistory");
        }

        public void NewUserHistory(UserProfile user, decimal generateId)
        {
           
            int cAction=0;
            foreach (PropertyInfo property in user.GetType().GetProperties())
            {
                HistoryField historyField = new HistoryField();    
                object value = property.GetValue(user, null);
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    historyField = UserProfileService.SetDataChanged(property.Name, "", value.ToString(), cAction);
                    if (historyField != null)
                    {
                        historyField.GroupField = generateId;
                        historyField.EntityId = user.Id;                        
                        historyField.EntityType = DomainObjectType.User;
                        SetDefaultDataForSaveHistory(historyField);
                        HistoryFieldService.Save(historyField);
                    }
                }
            }           
        }

        public override string SetActionUser(UserProfile entity, decimal GroupFieldId, string Action)
        {
            UserProfile user = GenericService.GetById(CurrentUser);
            UserActionField userActionField = new UserActionField();
            userActionField.Action = Action;
            userActionField.Id = GroupFieldId;
            userActionField.EntityId = entity.Id;
            userActionField.UserName = entity.FirstName + " " + entity.LastName;
            userActionField.ByWhom = user.FirstName + " " + user.LastName;
            userActionField.Date = DateTime.Now;
            userActionField.ClientCompany = CurrentCompany;
            UserActionFieldService.Save(userActionField);
            return Convert.ToString(GroupFieldId) + "," + userActionField.UserName;

        }
        public override void CompareForm(UserProfile entity, UserProfile formEntity, FormCollection collection, ActionMethod actionMethod)
        {            
            decimal generateId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
            int cAction = 0;
            string typeAction = string.Empty;
            HistoryField historyField = new HistoryField();
            if (actionMethod.Equals(ActionMethod.EditPassword) || actionMethod.Equals(ActionMethod.Create))
            {
                historyField.GroupField = generateId;
                historyField.EntityId = entity.Id;
                historyField.EntityType = DomainObjectType.User;
                typeAction = UserActionHistory.PasswordChanged;
                HistoryFieldService.Save(historyField);
            }
            else
            {
                foreach (PropertyInfo property in entity.GetType().GetProperties())
                {
                    object oldValue = property.GetValue(entity, null);
                    object value = property.GetValue(formEntity, null);
                    if (oldValue != null && value != null && !oldValue.Equals(value) && !property.Name.Equals("Kennwort"))
                    {
                        historyField = new HistoryField();
                        historyField = UserProfileService.SetDataChanged(property.Name, oldValue.ToString(), value.ToString(), cAction);
                        if (historyField != null)
                        {
                            cAction++;
                            historyField.GroupField = generateId;
                            historyField.EntityId = entity.Id;
                            historyField.EntityType = DomainObjectType.User;
                            SetDefaultDataForSaveHistory(historyField);
                            HistoryFieldService.Save(historyField);
                            typeAction = historyField.TypeAction;
                        }

                    }

                }
            }
            if (!string.IsNullOrEmpty(typeAction))
            {
                ViewData["GroupIdUser"] = SetActionUser(entity, generateId, typeAction);
            }
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Create(UserProfile entity, FormCollection collection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;

            string groupIdUser = string.Empty;

            if (ValidateFormData(entity, collection))
            {
                DomainObject<string> domainObject = (DomainObject<string>)((object)entity);                

                SetDetailFilterData(entity);

                GetFormData(entity, collection);

                SetDefaultEntityDataForSave(entity);

                SetDefaultDataForSave(entity);

                GenericService.Save(entity);

                decimal groupId = Convert.ToDecimal(UniqueKeyGenerator.GetInstance().GetUniqueKey());
                groupIdUser = SetActionUser(entity, groupId, Convert.ToString(ActionMethod.Create));

                NewUserHistory(entity, groupId);
                
                operationStatus = OperationStatus.Successful;

                //string userIdTempo = Request["UserId"];

                string tempo = entity.ToString();

                string browseId = Request["BrowseId"];
                if ((browseId.Equals("TeamMemberDetail")) && (operationStatus.Equals(OperationStatus.Successful)) && (tempo.Equals("Compelligence.Domain.Entity.UserProfile")))
                {
                    //return View("Create", "TeamMember");
                    return RedirectToAction("Create", "TeamMember", new { Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], Container = Request["Container"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
                }
                else
                {
                    return RedirectToAction("Edit", new { id = entity.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], Container = Request["Container"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"], GroupIdUser = groupIdUser });
                }
            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entity);

            SetUserSecurityAccess();

            return View("Edit", entity);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        private ActionResult CreateTeamMember(UserProfile entity, FormCollection collection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateFormData(entity, collection))
            {
                SetDetailFilterData(entity);

                GetFormData(entity, collection);

                SetDefaultEntityDataForSave(entity);

                SetDefaultDataForSave(entity);

                GenericService.Save(entity);

                operationStatus = OperationStatus.Successful;

                //return GetActionResultForCreate(domainObject, operationStatus);
                RedirectToAction("Create", "TeamMember");
            }

            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entity);

            SetUserSecurityAccess();

            return View("Edit", entity);
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(UserProfile user, FormCollection formCollection)
        {
            long numUsers = UserProfileService.CountByCompany(CurrentCompany);
            ClientCompanySubscription companySubscription = ClientCompanyService.GetSubscriptionByCompany(CurrentCompany);

            if (companySubscription != null && companySubscription.MaxUsers != null
                && (numUsers >= companySubscription.MaxUsers.Value))
            {
                ValidationDictionary.AddError("Email", string.Format(LabelResource.UserCompanyMaxNumError, companySubscription.MaxUsers));
            }
            else
            {
                if (Validator.IsBlankOrNull(user.Email))
                {
                    ValidationDictionary.AddError("Email", LabelResource.UserEmailRequiredError);
                }
                else if (!Validator.IsEmailOrWhite(user.Email))
                {
                    ValidationDictionary.AddError("Email", LabelResource.UserEmailFormatError);
                }
                else if (!UserProfileService.IsValidEmail(user.Id, user.Email, CurrentCompany))
                {
                    ValidationDictionary.AddError("Email", LabelResource.UserEmailExistsError);
                }

                if (Validator.IsBlankOrNull(user.FirstName))
                {
                    ValidationDictionary.AddError("FirstName", LabelResource.UserFirstNameRequiredError);
                }

                if (Validator.IsBlankOrNull(user.LastName))
                {
                    ValidationDictionary.AddError("LastName", LabelResource.UserLastNameRequiredError);
                }

                if (Validator.IsBlankOrNull(user.CountryCode))
                {
                    ValidationDictionary.AddError("CountryCode", LabelResource.UserCountryIdRequiredError);
                }

                if (Validator.IsBlankOrNull(user.City))
                {
                    ValidationDictionary.AddError("City", LabelResource.UserCityRequiredError);
                }

                if (Validator.IsBlankOrNull(user.Department))
                {
                    ValidationDictionary.AddError("Department", LabelResource.UserDepartmentRequiredError);
                }

                if (Validator.IsBlankOrNull(user.ZipCode))
                {
                    ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeRequiredError);
                }
                else if (!StringUtility.IsValidStringER(user.ZipCode, "(\\d){1,5}"))
                {
                    ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeMatchError);
                }
                else if (user.ZipCode.Length > 5)
                {
                    ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeLengthError);
                }

                if (Validator.IsBlankOrNull(user.Address))
                {
                    ValidationDictionary.AddError("Address", LabelResource.UserAddressRequiredError);
                }

                if (Validator.IsBlankOrNull(user.Phone))
                {
                    ValidationDictionary.AddError("Phone", LabelResource.UserPhoneRequiredError);
                }
                else
                {
                    if (!Validator.NumberFaxAndPhone(user.Phone))
                    {
                        ValidationDictionary.AddError("Phone", LabelResource.ValidateTextPhone);
                    }
                }
                if (!Validator.NumberFaxAndPhone(user.Fax))
                {
                    ValidationDictionary.AddError("Fax", LabelResource.ValidateTextFax);
                }
                if (Validator.IsBlankOrNull(user.Status))
                {
                    ValidationDictionary.AddError("Status", LabelResource.UserStatusRequiredError);
                }

                if (Validator.IsBlankOrNull(user.SecurityGroupId))
                {
                    ValidationDictionary.AddError("SecurityGroupId", LabelResource.UserSecurityGroupIdRequiredError);
                }
                ValidateEditFormPasswordData(user);
            }

            return ValidationDictionary.IsValid;
        }

        protected override bool ValidateEditFormData(UserProfile user, FormCollection formCollection)
        {
            string clientCompany = StringUtility.CheckNull((string)Session["ClientCompany"]);

            if (Validator.IsBlankOrNull(user.Email))
            {
                ValidationDictionary.AddError("Email", LabelResource.UserEmailRequiredError);
            }
            else if (!Validator.IsEmailOrWhite(user.Email))
            {
                ValidationDictionary.AddError("Email", LabelResource.UserEmailFormatError);
            }
            else if (!UserProfileService.IsValidEmail(user.Id, user.Email, clientCompany))
            {
                ValidationDictionary.AddError("Email", LabelResource.UserEmailExistsError);
            }

            if (Validator.IsBlankOrNull(user.FirstName))
            {
                ValidationDictionary.AddError("FirstName", LabelResource.UserFirstNameRequiredError);
            }

            if (Validator.IsBlankOrNull(user.LastName))
            {
                ValidationDictionary.AddError("LastName", LabelResource.UserLastNameRequiredError);
            }

            if (Validator.IsBlankOrNull(user.CountryCode))
            {
                ValidationDictionary.AddError("CountryCode", LabelResource.UserCountryIdRequiredError);
            }

            if (Validator.IsBlankOrNull(user.City))
            {
                ValidationDictionary.AddError("City", LabelResource.UserCityRequiredError);
            }

            if (Validator.IsBlankOrNull(user.Department))
            {
                ValidationDictionary.AddError("Department", LabelResource.UserDepartmentRequiredError);
            }

            if (Validator.IsBlankOrNull(user.ZipCode))
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeRequiredError);
            }
            else if (!StringUtility.IsValidStringER(user.ZipCode, "(\\d){1,5}"))
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeMatchError);
            }
            else if (user.ZipCode.Length > 5)
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeLengthError);
            }

            if (Validator.IsBlankOrNull(user.Address))
            {
                ValidationDictionary.AddError("Address", LabelResource.UserAddressRequiredError);
            }

            if (Validator.IsBlankOrNull(user.Phone))
            {
                ValidationDictionary.AddError("Phone", LabelResource.UserPhoneRequiredError);
            }
            else
            {
                if (!Validator.NumberFaxAndPhone(user.Phone))
                {
                    ValidationDictionary.AddError("Phone", LabelResource.ValidateTextPhone);
                }
            }
            if (!Validator.NumberFaxAndPhone(user.Fax))
            {
                ValidationDictionary.AddError("Fax", LabelResource.ValidateTextFax);
            }
            if ((user.Status.Equals(UserStatus.Disabled)) && (UserProfileService.HassDependences(user, clientCompany)))
            {
                ValidationDictionary.AddError("Status", LabelResource.UserStatusValueError);
            }

            return ValidationDictionary.IsValid;
        }
              
        protected override bool  ValidateDeleteData(UserProfile entity, StringBuilder errorMessage)
         {
            string clientCompany = StringUtility.CheckNull((string)Session["ClientCompany"]);
            string AssignedTo =  entity.Id;
             
            //string message = string.Empty;
            IList<EntityAllAssignedToView> UserRelationCollection = UserRelationService.GetViewAllActive(clientCompany, AssignedTo);
           
            if (UserRelationCollection.Count != 0)
            {
                //ViewData["UserRelationCount"] = LabelResource.UserRelationErrorDeleted;
                errorMessage.AppendLine(LabelResource.UserRelationErrorDeleted);
                return false;
            }
           
            return base.ValidateDeleteData(entity, errorMessage);
        }

        private bool ValidateEditFormPasswordData(UserProfile user)
        {
            if (Validator.IsBlankOrNull(user.Kennwort))
            {
                ValidationDictionary.AddError("Password", LabelResource.UserNewPasswordRequiredError);
            }
            else if (user.Kennwort.Length < 8)
            {
                ValidationDictionary.AddError("Password", LabelResource.UserNewPasswordLengthError);
            }
            else if (!Validator.Equals(user.Kennwort, user.ReKennwort))
            {
                ValidationDictionary.AddError("RePassword", LabelResource.UserRePasswordMatchError);
            }
            else if (!Validator.IsStrongPassword(user.Kennwort))
            {
                ValidationDictionary.AddError("Password", LabelResource.UserPasswordExpression);
            }
            return ValidationDictionary.IsValid;
        }
        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> userStatusList = ResourceService.GetAll<UserStatus>();
            IList<SecurityGroup> securityGroupList = SecurityGroupService.GetUserGroups(clientCompany);
            IList<CountryFirstUSAView> countryList = CountryService.GetFirstUSA();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);

            ViewData["StatusList"] = new SelectList(userStatusList, "Id", "Value");
            ViewData["CountryCodeList"] = new SelectList(countryList, "CountryCode", "Name");
            ViewData["SecurityGroupList"] = new SelectList(securityGroupList, "Id", "Name");
            ViewData["ReportToList"] = new SelectList(userList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(UserProfile userProfile)
        {
            userProfile.OriginalStatus = userProfile.Status;
        }

        protected override void SetDefaultRequestParametersToForm(OperationStatus operationStatus)
        {
            ViewData["OperationStatus"] = operationStatus;
        }

        protected override void SetFormEntityDataToForm(UserProfile userProfile)
        {
            userProfile.OriginalStatus = userProfile.Status;

            ModelState.SetModelValue("OriginalStatus", new ValueProviderResult(userProfile.OriginalStatus, userProfile.OriginalStatus, CultureInfo.InvariantCulture));
        }

        protected override string  SetDetailFilters(string parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.User;

            switch (detailType)
            {
                case DetailType.UserRelation:
                    AddFilter(detailFilter, "UserRelation.Id", parentId.ToString());
                    AddFilter(detailFilter, "UserRelation.EntityType", DomainObjectType.User);
                    AddFilter(browseDetailFilter, "EntityAllAssignedToView.AssignedTo", parentId.ToString());
                    childController = "UserRelation";
                    break;
                case DetailType.HistoryField:                   
                    AddFilter(browseDetailFilter, "UserAccountHistoryDetailView.EntityId", parentId.ToString());
                    childController = "HistoryField";
                    break;
            }
            return childController;
        }
        #endregion
    }
}

