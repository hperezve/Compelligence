using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Web;
using Compelligence.Security.Managers;
using Compelligence.DataTransfer.Entity;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Views;
using Compelligence.Emails.Entity;
using System.Net.Mail;
using System.Net;
using log4net;
using Compelligence.Emails.Util;
using Compelligence.Security.Filters;
namespace Compelligence.Web.Controllers
{
    public class MyProfileController : BackEndFormController<UserProfile, string>
    {

        #region Public Properties

        public IUserProfileService UserProfileService
        {
            get { return (IUserProfileService)_genericService; }
            set { _genericService = value; }
        }

        public ICountryService CountryService { get; set; }

        public IUserProfileConfigurationService UserProfileConfigurationService { get; set; }

        public IResourceService ResourceService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }
        #endregion

        #region Action Methods

        private void GetTimeOutOfUser()
        {
            string[] sessionTimeout = UserProfileConfigurationService.GetSessionTimeout(CurrentUser);
            if (sessionTimeout != null)
            {
                ViewData["SessionTimeout"] = sessionTimeout[0];
                ViewData["TimeoutUnit"] = sessionTimeout[1];
            }
            else
            {
                ViewData["SessionTimeout"] = Session.Timeout;
                ViewData["TimeoutUnit"] = TimeoutUnit.Minutes;
            }
        }

        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditProfile()
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);

            SetDefaultRequestParametersToForm(OperationStatus.Initiated);

            SetFormData();
            ViewData["DefaultPortNumber"] = "";
            //string[] sessionTimeout = UserProfileConfigurationService.GetSessionTimeout(CurrentUser);
            
            //if (sessionTimeout != null)
            //{
            //    ViewData["SessionTimeout"] = sessionTimeout[0];
            //    ViewData["TimeoutUnit"] = sessionTimeout[1];
            //}
            //else
            //{
            //    ViewData["SessionTimeout"] = Session.Timeout;
            //    ViewData["TimeoutUnit"] = TimeoutUnit.Minutes;
            //}
            GetTimeOutOfUser();
            return View("EditProfile", userProfile);
        }
        [AuthenticationFilter]
        //[AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditProfileFront()
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);

            SetDefaultRequestParametersToForm(OperationStatus.Initiated);
            SetFormData();
            ViewData["DefaultPortNumber"] = "";
            GetTimeOutOfUser();
            return View("EditProfileFront", userProfile);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult TestConfig()
        {
            string result = string.Empty;
            string SMTPServer = Request.Params["SMTPServer"];
            string PortNumber = Request.Params["PortNumber"];
            string UserIDs = Request.Params["UserIDs"];
            string SecurityMethod = Request.Params["SecurityMethod"];
            string ActiveSecurty = string.Empty;
            string AuthenticationMethod = Request.Params["AuthenticationMethod"];
            string Password = Request.Params["Password"];

            EmailTemplate emailTemplate = EmailUtilities.GetEmailTemplate(EmailType.NotificationSmtpServer);
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);
            ClientCompany clientCompany = ClientCompanyService.GetById(userProfile.ClientCompany);

            emailTemplate.AddParameter("ClientCompany", clientCompany.Name);
            emailTemplate.AddParameter("UserName", userProfile.Name);
            emailTemplate.AddParameter("Email", UserIDs);

            SmtpClient SmtpMailObjectMyProfile = new SmtpClient(SMTPServer);
            SmtpMailObjectMyProfile.Timeout = 9000;
            if (!string.IsNullOrEmpty(SecurityMethod))
            {
                SmtpMailObjectMyProfile.EnableSsl = Convert.ToBoolean(SecurityMethod);
            }
            else {
                SmtpMailObjectMyProfile.EnableSsl = false;
            }
            SmtpMailObjectMyProfile.UseDefaultCredentials = false;
            if (!string.IsNullOrEmpty(PortNumber))
            {
               if(DecimalUtility.IsNumeric(PortNumber))
                SmtpMailObjectMyProfile.Port = Convert.ToInt32(PortNumber);
            }
            SmtpMailObjectMyProfile.Credentials = new NetworkCredential(UserIDs, Password);


            if (Util.Validation.Validator.IsEmail(UserIDs))
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(UserIDs);
                mailMessage.To.Add(new MailAddress(userProfile.Email));
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = emailTemplate.Subject;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.Body = emailTemplate.Message;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                if (SmtpMailObjectMyProfile != null)
                {
                    try
                    {
                        SmtpMailObjectMyProfile.Send(mailMessage);
                        result = "true_K_"+"Login succeeded!  Configuration is correct";
                    }
                    catch (Exception exception)
                    {
                        result = "false_K_" +"Unable to login to SMTP server.  Error returned from server was  " + exception.Message;
                    }
                }
                else
                {
                    result = "false_K_" +"Unable to login to SMTP server.  Error returned from server was  " + "Smtp Configuration is not valid";
                }
            }
            else
            {
                result = "false_K_" + "the User ID does not have the correct format";
            }


            return new JsonResult() { Data = result };
        }


        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProfile(UserProfile userForm, FormCollection formCollection)
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);
            UserProfile userProfileResult = userForm;
            ViewData["SessionTimeout"] = formCollection["SessionTimeout"];
            ViewData["TimeoutUnit"] = formCollection["TimeoutUnit"];
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEditFormData(userForm, formCollection))
            {
                SetFormDataToEntity(userProfile, formCollection);
                GetFormData(userProfile, formCollection);
                SetDefaultDataForUpdate(userProfile);
                userProfile.OriginalStatus = userProfile.Status;
                UserProfileService.Update(userProfile);

                UserManager.GetInstance().SetUserProfileInCache(userProfile);

                userProfileResult = userProfile;

                UserProfileConfigurationService.SaveSessionTimeout(CurrentUser, formCollection["SessionTimeout"], formCollection["TimeoutUnit"]);

                int? sessionTimeout = UserProfileConfigurationService.GetSessionTimeoutMinutes(CurrentUser);

                if (sessionTimeout != null)
                {
                    Session.Timeout = sessionTimeout.Value;
                    Session["SessionTimeout"] = TimeSpan.FromMinutes(sessionTimeout.Value).TotalMilliseconds - TimeSpan.FromSeconds(30).TotalMilliseconds;
                }

                operationStatus = OperationStatus.Successful;
                return RedirectToAction("Index", "BackEnd");
            }

            SetDefaultRequestParametersToForm(operationStatus);

            SetFormData();

            return View("EditProfile", userProfileResult);
        }
        [AuthenticationFilter]
        //[AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProfileFront(UserProfile userForm, FormCollection formCollection)
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);
            UserProfile userProfileResult = userForm;
            ViewData["SessionTimeout"] = formCollection["SessionTimeout"];
            ViewData["TimeoutUnit"] = formCollection["TimeoutUnit"];
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEditFormData(userForm, formCollection))
            {
                SetFormDataToEntity(userProfile, formCollection);
                GetFormData(userProfile, formCollection);
                SetDefaultDataForUpdate(userProfile);
                userProfile.OriginalStatus = userProfile.Status;
                UserProfileService.Update(userProfile);

                UserManager.GetInstance().SetUserProfileInCache(userProfile);

                userProfileResult = userProfile;

                UserProfileConfigurationService.SaveSessionTimeout(CurrentUser, formCollection["SessionTimeout"], formCollection["TimeoutUnit"]);

                int? sessionTimeout = UserProfileConfigurationService.GetSessionTimeoutMinutes(CurrentUser);

                if (sessionTimeout != null)
                {
                    Session.Timeout = sessionTimeout.Value;
                    Session["SessionTimeout"] = TimeSpan.FromMinutes(sessionTimeout.Value).TotalMilliseconds - TimeSpan.FromSeconds(30).TotalMilliseconds;
                }

                operationStatus = OperationStatus.Successful;
                return RedirectToAction("Index", "Comparinator");
            }

            SetDefaultRequestParametersToForm(operationStatus);

            SetFormData();

            return View("EditProfileFront", userProfileResult);
            
        }

        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditEmailCfg()
        {
            SetDataToForm();

            EmailUserCfgRegistrationDTO emailUserCfgRegistration = UserProfileConfigurationService.GetEmailConfiguration(CurrentUser);
          
            if (string.IsNullOrEmpty(emailUserCfgRegistration.SmtpServer))
            {
                emailUserCfgRegistration.SmtpPort = "";
                emailUserCfgRegistration.EmailPassword = string.Empty;
                emailUserCfgRegistration.SmtpRequireSsl = false; /*cccccccc*/ 
                emailUserCfgRegistration.IsAuthenticationMethod =string.Empty;
                emailUserCfgRegistration.EmailUser = string.Empty; /*cccccccccccccccccc*/
            }
            ViewData["passwordView"] = emailUserCfgRegistration.EmailPassword;

            if (string.IsNullOrEmpty(emailUserCfgRegistration.PopServer))
            {
                emailUserCfgRegistration.PopPort = "110";
            }

            emailUserCfgRegistration.EditEmailPassword = "N";

            SetDefaultRequestParametersToForm(OperationStatus.Initiated);

            return View("EditEmailCfg", emailUserCfgRegistration);
        }

        public void SetDataToForm()
        {
            IList<ResourceObject> SecurityMethodList = ResourceService.GetAll<SecurityMethod>();
            ViewData["SecurityMethod"] = new SelectList(SecurityMethodList, "Id", "Value");
            IList<ResourceObject> AuthenticationMethodList = ResourceService.GetAll<AuthenticationMethod>();
            ViewData["AuthenticationMethod"] = new SelectList(AuthenticationMethodList, "Id", "Value");

        }
        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteEmailCfg(EmailUserCfgRegistrationDTO emailUserCfgRegistration, FormCollection formCollection)
        {
            SetDataToForm();
            SetFormData();
            emailUserCfgRegistration.UserId = CurrentUser;
            UserProfileConfigurationService.DeleteEmailConfiguration(emailUserCfgRegistration);
            return View("EditEmailCfg", emailUserCfgRegistration);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditEmailCfg(EmailUserCfgRegistrationDTO emailUserCfgRegistration, FormCollection formCollection)
        {
            UserProfile userprofile = UserProfileService.GetById(CurrentUser);
            SetDataToForm();
            EmailUserCfgRegistrationDTO email = new EmailUserCfgRegistrationDTO();
            email.SmtpServer = formCollection["SMTPServer"];
            email.SmtpPort = formCollection["SmtpPort"];
            email.EmailPassword = formCollection["EmailPassword"];
            if (!string.IsNullOrEmpty(formCollection["SmtpRequireSsl"]))
            {
                email.SmtpRequireSsl = Convert.ToBoolean(formCollection["SmtpRequireSsl"]);
            }
            email.EmailUser = formCollection["EmailUser"];
            email.IsAuthenticationMethod = formCollection["IsAuthenticationMethod"];
            email.UserId = CurrentUser;
      


            OperationStatus operationStatus = OperationStatus.Initiated;

            //if (ValidateEmailConfigurationData(emailUserCfgRegistration))
            //{
                UserProfileConfigurationService.SaveEmailConfiguration(email);

                emailUserCfgRegistration.EditEmailPassword = "N";

                ModelState.Clear();

                operationStatus = OperationStatus.Successful;
            //}
            SetFormData();
            SetDefaultRequestParametersToForm(operationStatus);
            GetTimeOutOfUser();
            return View("EditProfile", userprofile);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditPopEmailCfg()
        {
            EmailUserCfgRegistrationDTO emailUserCfgRegistration = UserProfileConfigurationService.GetEmailConfiguration(CurrentUser);

            if (string.IsNullOrEmpty(emailUserCfgRegistration.SmtpServer))
            {
                emailUserCfgRegistration.SmtpPort = "25";
            }

            if (string.IsNullOrEmpty(emailUserCfgRegistration.PopServer))
            {
                emailUserCfgRegistration.PopPort = "110";
            }

            emailUserCfgRegistration.EditEmailPassword = "N";

            SetDefaultRequestParametersToForm(OperationStatus.Initiated);

            return View("EditPopEmailCfg", emailUserCfgRegistration);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPopEmailCfg(EmailUserCfgRegistrationDTO emailUserCfgRegistration, FormCollection formCollection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEmailConfigurationData(emailUserCfgRegistration))
            {
                UserProfileConfigurationService.SaveEmailConfiguration(emailUserCfgRegistration);

                emailUserCfgRegistration.EditEmailPassword = "N";

                ModelState.Clear();

                operationStatus = OperationStatus.Successful;
            }

            SetDefaultRequestParametersToForm(operationStatus);

            return View("EditPopEmailCfg", emailUserCfgRegistration);
        }

        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditPassword()
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);

            return View(userProfile);
        }
        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPassword(UserProfile userProfileForm, FormCollection collection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;
            UserProfile userProfileResult = userProfileForm;
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);

            if (ValidateChangeFormPasswordData(userProfileForm))
            {
                SetFormDataToEntity(userProfile, collection);

                SetDefaultDataForUpdate(userProfile);

                UserProfileService.UpdatePassword(userProfile);

                userProfileResult = userProfile;

                operationStatus = OperationStatus.Successful;

                return RedirectToAction("EditProfile", new { operationStatus = operationStatus.ToString() });
            }

            return View(userProfileResult);
        }

        [AuthenticationFilter]
        //[AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditPasswordFront()
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);

            return View(userProfile);
        }
        [AuthenticationFilter]
        //[AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPasswordFront(UserProfile userProfileForm, FormCollection collection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;
            UserProfile userProfileResult = userProfileForm;
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);

            if (ValidateChangeFormPasswordData(userProfileForm))
            {
                SetFormDataToEntity(userProfile, collection);

                SetDefaultDataForUpdate(userProfile);

                UserProfileService.UpdatePassword(userProfile);

                userProfileResult = userProfile;

                operationStatus = OperationStatus.Successful;

                return RedirectToAction("EditProfileFront", new { operationStatus = operationStatus.ToString() });
            }

            return View(userProfileResult);
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateEditFormData(UserProfile userProfile, FormCollection formCollection)
        {
            string sessionTimeout = formCollection["SessionTimeout"];

            if (Validator.IsBlankOrNull(userProfile.FirstName))
            {
                ValidationDictionary.AddError("FirstName", LabelResource.UserFirstNameRequiredError);
            }

            if (Validator.IsBlankOrNull(userProfile.LastName))
            {
                ValidationDictionary.AddError("LastName", LabelResource.UserLastNameRequiredError);
            }

            if (!Validator.IsEmailOrWhite(userProfile.Email))
            {
                ValidationDictionary.AddError("Email", LabelResource.UserEmailFormatError);
            }
            else if (!UserProfileService.IsValidEmail(userProfile.Id, userProfile.Email, CurrentCompany))
            {
                ValidationDictionary.AddError("Email", LabelResource.UserEmailExistsError);
            }

            if (Validator.IsBlankOrNull(userProfile.CountryCode))
            {
                ValidationDictionary.AddError("CountryCode", LabelResource.UserCountryIdRequiredError);
            }

            if (Validator.IsBlankOrNull(userProfile.City))
            {
                ValidationDictionary.AddError("City", LabelResource.UserCityRequiredError);
            }

            if (Validator.IsBlankOrNull(userProfile.Department))
            {
                ValidationDictionary.AddError("Department", LabelResource.UserDepartmentRequiredError);
            }

            if (Validator.IsBlankOrNull(userProfile.ZipCode))
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeRequiredError);
            }
            else if (!StringUtility.IsValidStringER(userProfile.ZipCode, "(\\d){1,5}"))
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeMatchError);
            }
            else if (userProfile.ZipCode.Length > 5)
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.UserZipCodeLengthError);
            }

            if (Validator.IsBlankOrNull(userProfile.Address))
            {
                ValidationDictionary.AddError("Address", LabelResource.UserAddressRequiredError);
            }

            if (Validator.IsBlankOrNull(userProfile.Phone))
            {
                ValidationDictionary.AddError("Phone", LabelResource.UserPhoneRequiredError);
            }
            else
            {
                if (!Validator.NumberFaxAndPhone(userProfile.Phone))
                {
                    ValidationDictionary.AddError("Phone", LabelResource.ValidateTextPhone);
                }
            }
            if (!Validator.NumberFaxAndPhone(userProfile.Fax))
            {
                ValidationDictionary.AddError("Fax", LabelResource.ValidateTextFax);
            }
            if (Validator.IsBlankOrNull(sessionTimeout) || (!Validator.IsInt(sessionTimeout)) || (Convert.ToInt32(sessionTimeout) <= 0))
            {
                ValidationDictionary.AddError("SessionTimeoutErr", LabelResource.UserSessionTimeoutFormatError);
            }
            else if (Validator.IsBlankOrNull(formCollection["TimeoutUnit"]))
            {
                ValidationDictionary.AddError("SessionTimeoutErr", LabelResource.UserSessionTimeoutFormatError);
            }
            else if (((string.Compare(formCollection["TimeoutUnit"], TimeoutUnit.Minutes) == 0) && (Convert.ToInt32(sessionTimeout) > (60 * 24))) ||
                    ((string.Compare(formCollection["TimeoutUnit"], TimeoutUnit.Hours) == 0) && (Convert.ToInt32(sessionTimeout) > (24))))
            {
                ValidationDictionary.AddError("SessionTimeoutErr", LabelResource.UserSessionTimeoutRangeError);
            }

            return ValidationDictionary.IsValid;
        }

        protected bool ValidateEmailConfigurationData(EmailUserCfgRegistrationDTO emailUserCfgRegistration)
        {
            //if (!Validator.IsBlankOrNull(emailUserCfgRegistration.SmtpServer))
            //{
            //    if (Validator.IsBlankOrNull(emailUserCfgRegistration.SmtpPort))
            //    {
            //        ValidationDictionary.AddError("SmtpPort", "Smtp Port is required");
            //    }
            //    else if (!Validator.IsInt(emailUserCfgRegistration.SmtpPort))
            //    {
            //        ValidationDictionary.AddError("SmtpPort", "Smtp Port should be a number");
            //    }
            //    else if (!Validator.MinValue(Convert.ToInt32(emailUserCfgRegistration.SmtpPort), 1))
            //    {
            //        ValidationDictionary.AddError("SmtpPort", "Smtp Port is invalid");
            //    }
            //}

            //if (!Validator.IsBlankOrNull(emailUserCfgRegistration.PopServer))
            //{
            //    if (emailUserCfgRegistration.EditEmailPassword.Equals("Y"))
            //    {
            //        if (Validator.IsBlankOrNull(emailUserCfgRegistration.EmailPassword))
            //        {
            //            ValidationDictionary.AddError("EmailPassword", "Email Password is required");
            //        }
            //        else if (!Validator.MatchRegexp(emailUserCfgRegistration.EmailPassword, emailUserCfgRegistration.ReEmailPassword))
            //        {
            //            ValidationDictionary.AddError("ReEmailPassword", "Email Password is diferent to Re Password");
            //        }
            //    }

            //    if (Validator.IsBlankOrNull(emailUserCfgRegistration.PopPort))
            //    {
            //        ValidationDictionary.AddError("PopPort", "Pop Port is required");
            //    }
            //    else if (!Validator.IsInt(emailUserCfgRegistration.PopPort))
            //    {
            //        ValidationDictionary.AddError("PopPort", "Pop Port should be a number");
            //    }
            //    else if (!Validator.MinValue(Convert.ToInt32(emailUserCfgRegistration.PopPort), 1))
            //    {
            //        ValidationDictionary.AddError("PopPort", "Pop Port is invalid");
            //    }
            //}

            return ValidationDictionary.IsValid;
        }

        private bool ValidateChangeFormPasswordData(UserProfile user)
        {
            if (Validator.IsBlankOrNull(user.CurrentPassword))
            {
                ValidationDictionary.AddError("CurrentPassword", LabelResource.UserCurrentPasswordRequiredError);
            }
            else if (!UserProfileService.IsValidCurrentPassword(user.Id, user.CurrentPassword))
            {
                ValidationDictionary.AddError("CurrentPassword", LabelResource.UserCurrentPasswordExistsError);
            }

            if (Validator.IsBlankOrNull(user.Kennwort))
            {
                ValidationDictionary.AddError("Password", LabelResource.UserNewPasswordRequiredError);
            }
            else if (user.Kennwort.Length < 8)
            {
                ValidationDictionary.AddError("Password", LabelResource.UserNewPasswordLengthError);
            }
            else if (!Validator.MatchRegexp(user.Kennwort, user.ReKennwort))
            {
                ValidationDictionary.AddError("RePassword", LabelResource.UserRePasswordMatchError);
            }
            else if (user.CurrentPassword.Equals(user.Kennwort))
            {
                ValidationDictionary.AddError("Password", LabelResource.UserCurrentPasswordPasswordEqualError);
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
            IList<ResourceObject> timeoutUnitList = ResourceService.GetAll<TimeoutUnit>();
            //IList<Country> countryList = CountryService.GetAllActive();
            IList<CountryFirstUSAView> countryList = CountryService.GetFirstUSA();
            ViewData["TimeoutUnitList"] = new SelectList(timeoutUnitList, "Id", "Value");
            ViewData["CountryCodeList"] = new SelectList(countryList, "CountryCode", "Name");
            SetDataToFormByCompany();
        }

        protected override void SetDefaultRequestParametersToForm(OperationStatus operationStatus)
        {
            ViewData["OperationStatus"] = operationStatus;
        }

        #endregion

        #region Private Methods

        private void SetDataToFormByCompany()
        {
            //If "Use System E-mails for Feedback" is NOT checked, then the system should behave as it does today - no changes.
            //If "Use System E-mails for Feedback" is checked, then the following should occur in system:
            //1) Remove the link for "Edit Outgoing Email Server" in the "User Profile" section
            ViewData["UseSystemEmail"] = true;
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            if (clientCompany != null)
            {
                if (!string.IsNullOrEmpty(clientCompany.UseSystemEmail))
                {
                    if (clientCompany.UseSystemEmail.Equals("FALSE") || clientCompany.UseSystemEmail.Equals("False") || clientCompany.UseSystemEmail.Equals("false"))
                    {
                        ViewData["UseSystemEmail"] = false;
                    }
                }
            }
        }

        #endregion
    }
}
