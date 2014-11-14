using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Compelligence.Domain.Entity;
using Compelligence.Util.Type;
using Compelligence.Util.Validation;
using Compelligence.BusinessLogic.Interface;
using Resources;

using Compelligence.DataTransfer.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Emails.Util;
using System.Text.RegularExpressions;
using Compelligence.Domain.Entity.Views;
using Compelligence.Common.Utility;
using System.Reflection;

namespace Compelligence.Web.Controllers
{
    public class RegistrationController : GenericController
    {
        private IUserProfileService _userProfileService;
        private IClientCompanyService _clientCompanyService;
        private IClientCompanyConfigurationService _clientCompanyConfigurationService;
        private ICountryService _countryService;
        private IResourceService _resourceService;
        private IEmailService _emailService;

        public IUserProfileService UserProfileService
        {
            get { return _userProfileService; }
            set { _userProfileService = value; }
        }

        public IUserActionFieldService UserActionFieldService { get; set; }

        public IHistoryFieldService HistoryFieldService { get; set; }

        public IClientCompanyService ClientCompanyService
        {
            get { return _clientCompanyService; }
            set { _clientCompanyService = value; }
        }

        public IClientCompanyConfigurationService ClientCompanyConfigurationService
        {
            get { return _clientCompanyConfigurationService; }
            set { _clientCompanyConfigurationService = value; }
        }

        public ICountryService CountryService
        {
            get { return _countryService; }
            set { _countryService = value; }
        }

        public IResourceService ResourceService
        {
            get { return _resourceService; }
            set { _resourceService = value; }
        }

        public IEmailService EmailService
        {
            get { return _emailService; }
            set { _emailService = value; }
        }

        //
        // GET: /Registration/

        public ActionResult RegisterUser()
        {
            SetFormUserData();

            return View("User");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public ActionResult RegisterUser(UserProfile user, FormCollection collection)
        {
            try
            {
                ClientCompany clientCompany = ClientCompanyService.GetByDns(StringUtility.CheckNull(user.ClientCompanyDns));

                if (ValidateUserData(user, clientCompany))
                {
                    decimal historyId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();

                    user.ClientCompany = clientCompany.ClientCompanyId;

                    UserProfileService.RegisterNewUser(user);

                    SetActionUser(user, historyId, "Create", clientCompany);

                    NewUserHistory(user, historyId, clientCompany.ClientCompanyId);
                    
                    ViewData["CompanyUrl"] = GetCompanyHomeUrl(clientCompany.Dns);

                    return View("UserSuccess", user);
                }

                SetFormUserData();

                return View("User", user);
            }
            catch
            {
                return View();
            }


        }

        public void NewUserHistory(UserProfile user, decimal generateId, string clientCompany)
        {

            int cAction = 0;
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
                        historyField.ClientCompany = clientCompany;
                        historyField.CreatedDate = DateTime.Now;
                        historyField.LastChangedDate = DateTime.Now;
                        historyField.CreatedBy = clientCompany;
                        historyField.LastChangedBy = clientCompany;
                        HistoryFieldService.Save(historyField);
                    }
                }
            }
        }

        public void SetActionUser(UserProfile entity, decimal GroupFieldId, string Action, ClientCompany clientCompany)
        {           
            UserActionField userActionField = new UserActionField();
            userActionField.Action = Action;
            userActionField.Id = GroupFieldId;
            userActionField.EntityId = entity.Id;
            userActionField.UserName = entity.FirstName + " " + entity.LastName;
            userActionField.ByWhom = clientCompany.Name;
            userActionField.Date = DateTime.Now;
            userActionField.ClientCompany = clientCompany.ClientCompanyId;
            UserActionFieldService.Save(userActionField);            
        }

        //
        // GET: /Registration/

        public ActionResult RegisterClientCompany()
        {
            SetFormClientCompanyData();

            return View("ClientCompany");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public ActionResult RegisterClientCompany(AccountRegistrationDTO registration, FormCollection collection)
        {
            try
            {
                if (ValidateClientCompanyData(registration))
                {
                    //SetDefaultDataForSave(registration);

                    ClientCompanyService.SaveRegistration(registration);

                    ViewData["CompanyUrl"] = GetCompanyHomeUrl(registration.ClientCompanyDns);
                    return View("ClientCompanySuccess", registration);
                }

                SetFormClientCompanyData();

                return View("ClientCompany", registration);
            }
            catch
            {
                return View();
            }

        }

        //
        // GET: /Registration/

        public ActionResult RegisterEmailConfiguration()
        {
            string clientCompanyId = StringUtility.CheckNull(Request["cc"]);
            EmailCfgRegistrationDTO emailCfgRegistration = new EmailCfgRegistrationDTO();

            emailCfgRegistration.SmtpPort = "25";

            emailCfgRegistration.PopPort = "110";

            SetFormEmailConfigurationData(emailCfgRegistration, clientCompanyId);

            return View("EmailConfiguration", emailCfgRegistration);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegisterEmailConfiguration(EmailCfgRegistrationDTO emailCfgRegistration, FormCollection collection)
        {
            try
            {
                if (ValidateEmailConfigurationData(emailCfgRegistration))
                {
                    ClientCompanyConfigurationService.SaveEmailConfiguration(emailCfgRegistration);

                    //ViewData["CompanyUrl"] = "http://" + registration.ClientCompanyDns + LabelResource.DomainURL;

                    SetFormEmailConfigurationData(emailCfgRegistration, emailCfgRegistration.ClientCompany);

                    return View("EmailConfigurationSuccess");
                }

                SetFormEmailConfigurationData(emailCfgRegistration, emailCfgRegistration.ClientCompany);

                return View("EmailConfiguration", emailCfgRegistration);
            }
            catch
            {
                return View();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendEmailRequestDns(FormCollection collection)
        {
            string emailTo = collection["EmailTo"];
            string userName = collection["MyName"];
            string userEmail = collection["MyEmail"];
            string comments = collection["Comments"];

            EmailService.SendRequestDnsMail(emailTo, userName, userEmail, comments);

            return Content("Email was send successfully");
        }

        private bool ValidateUserData(UserProfile user, ClientCompany clientCompany)
        {
            if (!Validator.IsAlphaNum(user.ClientCompanyDns))
            {
                ValidationDictionary.AddError("ClientCompanyDns", string.Format(LabelResource.ValidateTextAlphaNumeric, "Client Company Dns"));
            }
            if (!Validator.IsAlphaNum(user.FirstName))
            {
                ValidationDictionary.AddError("FirstName", string.Format(LabelResource.ValidateTextAlphaNumeric, "First Name"));
            }
            if (!Validator.IsAlphaNum(user.LastName))
            {
                ValidationDictionary.AddError("LastName", string.Format(LabelResource.ValidateTextAlphaNumeric, "Last Name"));
            }
            if (!Validator.IsAlphaNum(user.Address))
            {
                ValidationDictionary.AddError("Address", string.Format(LabelResource.ValidateTextAlphaNumeric, "Address"));
            }
            if (!Validator.IsAlphaNum(user.City))
            {
                ValidationDictionary.AddError("City", string.Format(LabelResource.ValidateTextAlphaNumeric, "City"));
            }
            if (!Validator.IsAlphaNum(user.Department))
            {
                ValidationDictionary.AddError("Department", string.Format(LabelResource.ValidateTextAlphaNumeric, "Country"));
            }
            if (!Validator.NumberFaxAndPhone(user.Phone))
            {
                ValidationDictionary.AddError("Phone", string.Format(LabelResource.ValidateTextPhone, "Phone"));
            }
            if (!Validator.NumberFaxAndPhone(user.Fax))
            {
                ValidationDictionary.AddError("Fax", string.Format(LabelResource.ValidateTextFax, "Fax"));
            }
            if (Validator.IsBlankOrNull(user.ClientCompanyDns))
            {
                ValidationDictionary.AddError("ClientCompanyDns", LabelResource.CompanyDnsRequiredError);
            }
            else if (!Validator.IsUrl("http://" + user.ClientCompanyDns + LabelResource.DomainURL))
            {
                ValidationDictionary.AddError("ClientCompanyDns", LabelResource.CompanyDnsFormatError);
            }
            else if (clientCompany == null)
            {
                ValidationDictionary.AddError("ClientCompanyDns", "Company does not exist");
            }
            else
            {
                long numUsers = UserProfileService.CountByCompany(clientCompany.ClientCompanyId);
                ClientCompanySubscription companySubscription = ClientCompanyService.GetSubscriptionByCompany(clientCompany.ClientCompanyId);

                if (companySubscription != null && companySubscription.MaxUsers != null
                    && (numUsers >= companySubscription.MaxUsers.Value))
                {
                    ValidationDictionary.AddError("Email", string.Format(LabelResource.UserCompanyMaxNumError, companySubscription.MaxUsers));
                }
                else
                {
                    if (!Validator.IsEmailOrWhite(user.Email))
                    {
                        ValidationDictionary.AddError("Email", LabelResource.UserEmailFormatError);
                    }
                    else if ((clientCompany != null) && (!UserProfileService.IsValidEmail(null, user.Email, clientCompany.ClientCompanyId)))
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

                    if (Validator.IsBlankOrNull(user.Kennwort))
                    {
                        ValidationDictionary.AddError("Password", LabelResource.UserPasswordRequiredError);
                    }
                    else if (!Validator.MatchRegexp(user.Kennwort, user.ReKennwort))
                    {
                        ValidationDictionary.AddError("RePassword", LabelResource.UserRePasswordMatchError);
                    }

                    else if (!Validator.IsStrongPassword(user.Kennwort))
                    {
                        ValidationDictionary.AddError("Password", LabelResource.UserPasswordExpression);
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
                }
            }

            return ValidationDictionary.IsValid;
        }

        protected bool ValidateClientCompanyData(AccountRegistrationDTO registration)
        {
            if (!Validator.IsAlphaNum(registration.ClientCompanyName))
            {
                ValidationDictionary.AddError("ClientCompanyName", string.Format(LabelResource.ValidateTextAlphaNumeric, "Company Name"));
            }
            if (!Validator.IsAlphaNum(registration.UserProfileFirstName))
            {
                ValidationDictionary.AddError("UserProfileFirstName", string.Format(LabelResource.ValidateTextAlphaNumeric, "First Name"));
            }
            if (!Validator.IsAlphaNum(registration.UserProfileLastName))
            {
                ValidationDictionary.AddError("UserProfileLastName", string.Format(LabelResource.ValidateTextAlphaNumeric, "Last Name"));
            }
            if (!Validator.IsAlphaNum(registration.ClientCompanyAddress))
            {
                ValidationDictionary.AddError("ClientCompanyAddress", string.Format(LabelResource.ValidateTextAlphaNumeric, "Company Address"));
            }
            if (!Validator.IsAlphaNum(registration.ClientCompanyCity))
            {
                ValidationDictionary.AddError("ClientCompanyCity", string.Format(LabelResource.ValidateTextAlphaNumeric, "Company City"));
            }

            if (!Validator.NumberFaxAndPhone(registration.ClientCompanyPhone))
            {
                ValidationDictionary.AddError("ClientCompanyPhone", string.Format(LabelResource.ValidateTextPhone, "Company Phone"));
            }            
            if (Validator.IsBlankOrNull(registration.ClientCompanyName))
            {
                ValidationDictionary.AddError("ClientCompanyName", "ClientCompanyName");
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyAddress))
            {
                ValidationDictionary.AddError("ClientCompanyAddress", LabelResource.CompanyAddressRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.IndustryName))
            {
                ValidationDictionary.AddError("IndustryName", LabelResource.CompanyIndustryNameRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyCity))
            {
                ValidationDictionary.AddError("ClientCompanyCity", LabelResource.CompanyCityRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyState))
            {
                ValidationDictionary.AddError("ClientCompanyState", LabelResource.CompanyStateRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyZipCode))
            {
                ValidationDictionary.AddError("ClientCompanyZipCode", LabelResource.CompanyZipCodeRequiredError);
            }
            else if (!StringUtility.IsValidStringER(registration.ClientCompanyZipCode, "(\\d){1,5}"))
            {
                ValidationDictionary.AddError("ClientCompanyZipCode", LabelResource.CompanyZipCodeMatchError);
            }
            else if (registration.ClientCompanyZipCode.Length > 5)
            {
                ValidationDictionary.AddError("ClientCompanyZipCode", LabelResource.CompanyZipCodeLengthError);
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyCountryCode))
            {
                ValidationDictionary.AddError("ClientCompanyCountryCode", LabelResource.CompanyCountryCodeRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyPhone))
            {
                ValidationDictionary.AddError("ClientCompanyPhone", LabelResource.CompanyPhoneRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyDns))
            {
                ValidationDictionary.AddError("ClientCompanyDns", LabelResource.CompanyDnsRequiredError);
            }
            else if (!Validator.IsUrl("http://" + registration.ClientCompanyDns + LabelResource.DomainURL))
            {
                ValidationDictionary.AddError("ClientCompanyDns", LabelResource.CompanyDnsFormatError);
            }
            else if (!Validator.IsDns(registration.ClientCompanyDns))
            {
                ValidationDictionary.AddError("ClientCompanyDns", LabelResource.CompanyDnsFormatError);
            }
            else if (!ClientCompanyService.IsValidDns(string.Empty, registration.ClientCompanyDns))
            {
                ValidationDictionary.AddError("ClientCompanyDns", "Company Dns is already registered");
            }

            if (Validator.IsBlankOrNull(registration.ClientCompanyDescription))
            {
                ValidationDictionary.AddError("ClientCompanyDescription", LabelResource.CompanyDescriptionRequiredError);
            }

            if (!Validator.IsBlankOrNull(registration.ClientCompanySalesForceUser) ||
                !Validator.IsBlankOrNull(registration.ClientCompanySalesForceToken))
            {
                if (Validator.IsBlankOrNull(registration.ClientCompanySalesForcePassword))
                {
                    ValidationDictionary.AddError("ClientCompanySalesForcePassword", LabelResource.CompanySalesForcePasswordRequiredError);
                }
                else if (registration.ClientCompanySalesForcePassword.Length < 8)
                {
                    ValidationDictionary.AddError("ClientCompanySalesForcePassword", LabelResource.CompanySalesForcePasswordLengthError);
                }
                else if (!Validator.MatchRegexp(registration.ClientCompanySalesForcePassword, registration.ClientCompanySalesForceRePassword))
                {
                    ValidationDictionary.AddError("ClientCompanySalesForceRePassword", LabelResource.CompanySalesForceRePasswordMatchError);
                }
            }

            if (Validator.IsBlankOrNull(registration.UserProfileEmail))
            {
                ValidationDictionary.AddError("UserProfileEmail", LabelResource.UserEmailRequiredError);
            }
            else if (!Validator.IsEmailOrWhite(registration.UserProfileEmail))
            {
                ValidationDictionary.AddError("UserProfileEmail", LabelResource.UserEmailFormatError);
            }
            else
            {
                UserProfile User = UserProfileService.GetByEmail(registration.UserProfileEmail);
                if (User != null)
                {
                    ValidationDictionary.AddError("EmailUser", "Email is not valid. That email address already exists");
                }
            }

            if (Validator.IsBlankOrNull(registration.UserProfileFirstName))
            {
                ValidationDictionary.AddError("UserProfileFirstName", LabelResource.UserFirstNameRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.UserProfileLastName))
            {
                ValidationDictionary.AddError("UserProfileLastName", LabelResource.UserLastNameRequiredError);
            }

            if (Validator.IsBlankOrNull(registration.UserProfilePassword))
            {
                ValidationDictionary.AddError("UserProfilePassword", LabelResource.UserPasswordRequiredError);
            }
            else if (registration.UserProfilePassword.Length < 8)
            {
                ValidationDictionary.AddError("UserProfilePassword", LabelResource.UserPasswordLengthError);
            }
            else if (!Validator.MatchRegexp(registration.UserProfilePassword, registration.UserProfileRePassword))
            {
                ValidationDictionary.AddError("UserProfileRePassword", LabelResource.UserRePasswordMatchError);
            }
            else if (!Validator.IsStrongPassword(registration.UserProfilePassword))
            {
                ValidationDictionary.AddError("UserProfilePassword", LabelResource.UserPasswordExpression);
            }

            //if (Validator.IsBlankOrNull(registration.UserProfileCountryCode))
            //{
            //    ValidationDictionary.AddError("UserProfileCountryCode", LabelResource.UserCountryIdRequiredError);
            //}

            //if (Validator.IsBlankOrNull(registration.UserProfileCity))
            //{
            //    ValidationDictionary.AddError("UserProfileCity", LabelResource.UserCityRequiredError);
            //}

            return ValidationDictionary.IsValid;
        }

        protected bool ValidateEmailConfigurationData(EmailCfgRegistrationDTO emailCfgRegistration)
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(emailCfgRegistration.ClientCompany);

            if (clientCompany == null)
            {
                ValidationDictionary.AddError("EmailUser", "Company does not exist, you can not register data");
            }
            else
            {
                if (Validator.IsBlankOrNull(emailCfgRegistration.EmailUser))
                {
                    ValidationDictionary.AddError("EmailUser", "Credential User is required");
                }
                else
                {
                    UserProfile User = UserProfileService.GetByEmail(emailCfgRegistration.EmailUser);
                    if (User != null)
                    {
                        ValidationDictionary.AddError("EmailUser", "Email is not valid. That email address already exists");
                    }
                }

                if (Validator.IsBlankOrNull(emailCfgRegistration.EmailPassword))
                {
                    ValidationDictionary.AddError("EmailPassword", "Credential Password is required");
                }
                else if (!Validator.MatchRegexp(emailCfgRegistration.EmailPassword, emailCfgRegistration.ReEmailPassword))
                {
                    ValidationDictionary.AddError("ReEmailPassword", "Credential Password is diferent to Re Password");
                }

                if (Validator.IsBlankOrNull(emailCfgRegistration.SmtpServer))
                {
                    ValidationDictionary.AddError("SmtpServer", "Smtp Server is required");
                }

                if (Validator.IsBlankOrNull(emailCfgRegistration.SmtpPort))
                {
                    ValidationDictionary.AddError("SmtpPort", "Smtp Port is required");
                }
                else if (!Validator.IsInt(emailCfgRegistration.SmtpPort))
                {
                    ValidationDictionary.AddError("SmtpPort", "Smtp Port should be a number");
                }
                else if (!Validator.MinValue(Convert.ToInt32(emailCfgRegistration.SmtpPort), 1))
                {
                    ValidationDictionary.AddError("SmtpPort", "Smtp Port is invalid");
                }

                if (Validator.IsBlankOrNull(emailCfgRegistration.PopServer))
                {
                    ValidationDictionary.AddError("PopServer", "Pop Server is required");
                }

                if (Validator.IsBlankOrNull(emailCfgRegistration.PopPort))
                {
                    ValidationDictionary.AddError("PopPort", "Pop Port is required");
                }
                else if (!Validator.IsInt(emailCfgRegistration.PopPort))
                {
                    ValidationDictionary.AddError("PopPort", "Pop Port should be a number");
                }
                else if (!Validator.MinValue(Convert.ToInt32(emailCfgRegistration.PopPort), 1))
                {
                    ValidationDictionary.AddError("PopPort", "Pop Port is invalid");
                }
            }

            return ValidationDictionary.IsValid;
        }

        private void SetFormUserData()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            int beginDns = currentUrl.IndexOf("://");
            int endDns = currentUrl.IndexOf('/', beginDns + 3);

            string currentDns = currentUrl.Substring(beginDns + 3, endDns - (beginDns + 3));
            string companyDns = string.Empty;
            ClientCompany clientCompany = null;

            endDns = currentDns.IndexOf('.');

            if (endDns > 0)
            {
                companyDns = currentDns.Substring(0, endDns);
                clientCompany = ClientCompanyService.GetByDns(companyDns);

                if (clientCompany != null)
                {
                    companyDns = clientCompany.Dns;
                }
                if (companyDns.Equals("www") || companyDns.Equals("WWW"))
                {
                    companyDns = string.Empty;
                }
            }

            //IList<Country> countryList = CountryService.GetAllActive();
            IList<CountryFirstUSAView> countryList = CountryService.GetFirstUSA();

            ViewData["CountryCodeList"] = new SelectList(countryList, "CountryCode", "Name");

            ViewData["CompanyDns"] = companyDns;

            ViewData["CompanyUrl"] = GetCompanyHomeUrl(companyDns);
        }

        private void SetFormClientCompanyData()
        {
            IList<CountryFirstUSAView> countryList = CountryService.GetFirstUSA();
            ViewData["CountryCodeList"] = new SelectList(countryList, "CountryCode", "Name");
        }

        private void SetFormEmailConfigurationData(EmailCfgRegistrationDTO emailCfgRegistration, string clientCompanyId)
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);

            if (clientCompany != null)
            {
                ViewData["ClientCompanyObject"] = clientCompany;
                emailCfgRegistration.ClientCompany = clientCompany.ClientCompanyId;
                emailCfgRegistration.EmailUser = clientCompany.Email;

                ViewData["CompanyUrl"] = GetCompanyHomeUrl(clientCompany.Dns);
            }
            else
            {
                ViewData["CompanyUrl"] = GetCompanyHomeUrl(string.Empty);
            }
        }

        private string GetCompanyHomeUrl(string companyDns)
        {
            string currentHost = EmailUtilities.GetApplicationHost();
            //string currentHost = "http://companyb.compelligence.com/compelligencetest";
            string regExpCompUrl = "^(\\w+)://(\\w+)(\\.)(\\w+)(\\.)(\\w+)$";
            string regExpParUrl = "^(\\w+)://(\\w+)(\\.)(\\w+)$";
            if (!string.IsNullOrEmpty(companyDns))
            {
                int beginIndex = currentHost.IndexOf("://");

                if (Regex.IsMatch(currentHost, regExpCompUrl))
                {
                    string currentDomain = currentHost.Substring(beginIndex + 3, currentHost.IndexOf('.') - (beginIndex + 3));
                    currentHost = currentHost.Replace("://" + currentDomain + ".", "://" + companyDns + ".");
                }
                else if (Regex.IsMatch(currentHost, regExpParUrl))
                {
                    currentHost = currentHost.Insert(beginIndex + 3, companyDns + ".");
                }
                else
                {
                    if (currentHost.IndexOf(companyDns) == -1)
                    {
                        int endDns = currentHost.IndexOf('.');
                        if (endDns > 0)
                        {
                            string firstString = currentHost.Substring(0,endDns);
                            string beginOfCurrentHost = firstString.Substring(beginIndex + 3);
                            if ((beginOfCurrentHost.IndexOf("compelligence") != -1) || (beginOfCurrentHost.IndexOf("COMPELLIGENCE") != -1) || (beginOfCurrentHost.IndexOf("COMPETITIVETEAM") != -1) || (beginOfCurrentHost.IndexOf("competitiveteam") != -1) || (beginOfCurrentHost.IndexOf("competitivesalestool") != -1) || (beginOfCurrentHost.IndexOf("COMPETITIVESALESTOOL") != -1))
                            {
                                //http://compelligence.com/exec4/
                                currentHost = currentHost.Substring(0, beginIndex + 3) + companyDns + "." + currentHost.Substring(beginIndex + 3);
                            }
                            else
                            {
                                //http://www.compelligence.com/exec4/ or //http://companyb.compelligence.com/exec4/
                                currentHost = currentHost.Substring(0, beginIndex + 3) + companyDns + currentHost.Substring(endDns);
                            }
                        }
                    }
                }
            }

            return currentHost;
        }
    }
}
