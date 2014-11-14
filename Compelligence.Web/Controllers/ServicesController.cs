using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Resources;
using Compelligence.Util.Validation;
using Common.Logging;
using Compelligence.Common.Validation;
using Compelligence.Security.Managers;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    public class ServicesController : Controller
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(HomeController));
        public IUserProfileService UserProfileService { get; set; }

        public IClientCompanyService ClientCompanyService { get; set; }

        public ISecurityGroupService SecurityGroupService { get; set; }

        public IUserProfileConfigurationService UserProfileConfigurationService { get; set; }
        private IValidationDictionary _validationDictionary;

        public IEmailService EmailService { get; set; }

        public ServicesController()
        {
            _validationDictionary = new ModelStateWrapper(this.ModelState);
        }

        protected IValidationDictionary ValidationDictionary
        {
            get { return _validationDictionary; }
            set { _validationDictionary = value; }
        }
        //
        // GET: /Services/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ValidateUser(string username, string kennwort)
        {
            //string username = string.Empty; string kennwort=string.Empty;
            string u = Request["username"];
            ClientCompany clientCompany = GetCompanyByUrl();
            if (!string.IsNullOrEmpty(username)) { username = username.Trim(); }
            if (!string.IsNullOrEmpty(kennwort)) { kennwort = kennwort.Trim(); }
            SetDefaultFormValues();
            var message = string.Empty;
            if (isTest() || (clientCompany != null))
            {
                LOG.Info("if (isTest() || (clientCompany != null))");
                if (isTest() || ValidateCompany(clientCompany))
                {
                    LOG.Info("if (isTest() || ValidateCompany(clientCompany))");

                    if (ValidateData(username, kennwort))
                    {
                        LOG.Info("if (ValidateData(username, password))");

                        UserProfile userProfile = GetUserProfile(username, kennwort, clientCompany);

                        if (userProfile != null)
                        {
                            if (ValidateUserProfileEnabled(userProfile.Status))
                            {
                                UserManager.GetInstance().SetUserProfileInCache(userProfile);
                                RoleManager.GetInstance().SetSecurityAccessInCache(userProfile);
                                int? sessionTimeout = UserProfileConfigurationService.GetSessionTimeoutMinutes(userProfile.Id);
                                ClientCompanySubscription companySubscription = ClientCompanyService.GetSubscriptionByCompany(userProfile.ClientCompany);

                                if (companySubscription == null)
                                {
                                    ClientCompanyService.CreateDefaultCompanySubscription(userProfile.ClientCompany, userProfile.Id);
                                }

                                Session["UserId"] = userProfile.Id;
                                Session["ClientCompany"] = userProfile.ClientCompany;
                                Session["EditHelp"] = userProfile.EditHelp;
                                ClientCompany clientCompanyImage = ClientCompanyService.GetById(userProfile.ClientCompany);
                                Session["Imageurl"] = clientCompanyImage.Imageurl;
                                if (sessionTimeout != null)
                                {
                                    Session.Timeout = sessionTimeout.Value;
                                    Session["SessionTimeout"] = TimeSpan.FromMinutes(sessionTimeout.Value).TotalMilliseconds - TimeSpan.FromSeconds(30).TotalMilliseconds;
                                }
                                else
                                {
                                    Session["SessionTimeout"] = TimeSpan.FromMinutes(Session.Timeout).TotalMilliseconds - TimeSpan.FromSeconds(30).TotalMilliseconds;
                                }
                                if (userProfile.SecurityGroupList.Count > 1)
                                {
                                    SecurityGroup securityGroupRoot = SecurityGroupService.GetById(userProfile.SecurityGroupList[1]);
                                    Session["SecurityGroupIdRoot"] = securityGroupRoot.Id;
                                }

                                SecurityGroup securityGroup = SecurityGroupService.GetById(userProfile.SecurityGroupList[0]);
                                Session["SecurityGroupId"] = securityGroup.Id;
                                string accessType = securityGroup.AccessType;
                                UserProfile userProfileLog = UserProfileService.GetById(userProfile.Id);
                                if (accessType.Equals(SecurityAccessType.BackEnd) || accessType.Equals(SecurityAccessType.Partner))
                                {
                                    UserProfileService.UpdateLogged(userProfileLog);
                                    return Json(new { user = new { userId = userProfile.Id, firstName = userProfile.FirstName, lastName = userProfile.LastName, status = userProfile.Status, company = userProfile.ClientCompany, securityGroup = securityGroup.Id, logoClient = clientCompanyImage.Imageurl }, action = "BackEnd.aspx/Index" });
                                    //return RedirectToAction("Index", "BackEnd");

                                }
                                else if (accessType.Equals(SecurityAccessType.FrontEnd))
                                {
                                    UserProfileService.UpdateLogged(userProfileLog);
                                    return Json(new { user = new { userId = userProfile.Id, firstName = userProfile.FirstName, lastName = userProfile.LastName, status = userProfile.Status, company = userProfile.ClientCompany, securityGroup = securityGroup.Id, logoClient = clientCompanyImage.Imageurl }, action = "Comparinator.aspx/Index" });

                                }
                                else
                                {
                                    UserProfileService.UpdateLogged(userProfileLog);
                                    return Json(new { user = new { userId = userProfile.Id, firstName = userProfile.FirstName, lastName = userProfile.LastName, status = userProfile.Status, company = userProfile.ClientCompany, securityGroup = securityGroup.Id, logoClient = clientCompanyImage.Imageurl }, action = "Comparinator.aspx/Index" });
                                }
                            }
                            else {
                                message = LabelResource.LoginUserEnabledError;
                            }
                        }
                        else {
                            message = "The email or password provided is incorrect";
                        }
                    }
                }
            }
            else
            {
                //ValidationDictionary.AddError("Username", LabelResource.LoginCompanyUrlError);               
                //ViewData["message"] = LabelResource.LoginCompanyUrlError;
                bool istest = isTest();
                string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
                ViewData["message"] = "Login()/First Level wrong isTest()=" + istest + ", ClientCompany=" + clientCompany + ", Url" + currentUrl;
            }


            return Json(null);
        }
        #region Private Methods

        private bool ValidateUserProfileEnabled(string userProfileStatus)
        {
            if (userProfileStatus.Equals(UserStatus.Enabled))
            {
                return true;
            }
            else
            {
                ValidationDictionary.AddError("Username", LabelResource.LoginUserEnabledError);
                return false;
            }
        }
        
        private UserProfile GetUserProfile(string username, string password, ClientCompany clientCompany)
        {
            UserProfile userProfile;

            if (isTest())
            {
                userProfile = UserProfileService.GetByEmailAndPassword(username, BlackBox.Encrypt(password));
            }
            else
            {
                userProfile = UserProfileService.GetForAuthentication(username, BlackBox.Encrypt(password), clientCompany.ClientCompanyId);
            }

            if (userProfile == null)
            {
                ValidationDictionary.AddError("Username", LabelResource.LoginEmailPasswordIncorrect);
            }

            return userProfile;
        }
        private bool ValidateData(string username, string password)
        {
            if (Validator.IsBlankOrNull(username) || Validator.IsBlankOrNull(password))
            {
                ValidationDictionary.AddError("Username", LabelResource.LoginEmailPasswordEmpty);
            }

            return ValidationDictionary.IsValid;
        }

        private bool ValidateUserName(string username, string textCaptcha)
        {
            if (Validator.IsBlankOrNull(username))
            {
                ValidationDictionary.AddError("Username", LabelResource.LoginEmailEmpty);
            }
            else
            {
                if (!Validator.IsEmail(username))
                {
                    ValidationDictionary.AddError("Username", LabelResource.UserEmailFormatError);
                }
            }
            if (Validator.IsBlankOrNull(textCaptcha))
            {
                ValidationDictionary.AddError("Captcha", LabelResource.TextCaptchaEmptyError);
            }
            else
            {
                if (!textCaptcha.Equals(Session["TextCaptcha"].ToString()))
                {
                    ValidationDictionary.AddError("Captcha", LabelResource.TextCaptchaMatchError);
                }
            }
            return ValidationDictionary.IsValid;
        }

        private bool ValidateCompany(ClientCompany clientCompany)
        {
            if (!clientCompany.Status.Equals(ClientCompanyStatus.Disabled))
            {
                return true;
            }
            else
            {
                //ValidationDictionary.AddError("Username", LabelResource.LoginCompanyUrlError);
                ViewData["message"] = LabelResource.LoginCompanyUrlError;
                return false;
            }
        }

        private bool isTest()
        {
            bool isTest = false;
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            //currentUrl = "https://companyb.competitivesalestool.com/";  //company enabled
            //currentUrl = "https://c6.competitivesalestool.com/"; // company disabled
            if (Validator.MatchRegexp(currentUrl, "216.37.169.78") ||
                Validator.MatchRegexp(currentUrl, "190.41.113.34") ||
                Validator.MatchRegexp(currentUrl, "173.161.150.210") || //www.competitiveteam.com
                Validator.MatchRegexp(currentUrl, "173.161.150.211") || //www.compelligence.com
                Validator.MatchRegexp(currentUrl, "192.168.20.") || //remote local network
                Validator.MatchRegexp(currentUrl, "localhost") ||
                Validator.MatchRegexp(currentUrl, "192.168.1.")
                )
            {
                isTest = true;
            }

            return isTest;
        }

        private ClientCompany GetCompanyByUrl()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            int beginUrl = currentUrl.IndexOf("://");
            string companyDns = currentUrl.Substring(beginUrl + 3, currentUrl.IndexOf('.') - (beginUrl + 3));

            return ClientCompanyService.GetByDns(companyDns.ToLower());
        }

        private void SetDefaultFormValues()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            string[] NameCompany = System.Web.HttpContext.Current.Request.Url.Host.Split('.');
            string scheme = System.Web.HttpContext.Current.Request.Url.Scheme;
            ViewData["ShowNameCompany"] = string.Empty;
            bool IsValidDns = true;
            bool enableup = true;
            bool showRegisterCompany = true;
            bool showRegisterUser = true;
            if ((currentUrl.IndexOf(scheme + "://localhost") == 0) || (currentUrl.IndexOf(scheme + "://www.") == 0) || (currentUrl.IndexOf(scheme + "://competitiveteam.com/") == 0) ||
                (currentUrl.IndexOf(scheme + "://compelligence.com/") == 0) || (currentUrl.IndexOf(scheme + "://competitivesalestool.com/") == 0))
            {
                showRegisterCompany = true;
                enableup = false;
                if (currentUrl.IndexOf(scheme + "://localhost") == 0)
                {
                    enableup = true;
                }
            }
            else
            {
                ClientCompany clientCompany = ClientCompanyService.GetByDns(NameCompany[0]);
                if (clientCompany == null) //company not exist then disable u/p and show alert message
                {
                    IsValidDns = false;
                    enableup = false;
                    showRegisterCompany = true;
                }
                else
                {
                    if (clientCompany.Status.Equals(ClientCompanyStatus.Enabled))
                    {
                        ViewData["ShowNameCompany"] = clientCompany.Name;// NameCompany[0];
                        showRegisterCompany = false;
                    }
                    else
                    {
                        enableup = false;
                        showRegisterCompany = false;
                        showRegisterUser = false;
                    }
                }
            }
            ViewData["ShowRegisterUser"] = showRegisterUser;
            ViewData["ShowRegisterCompany"] = showRegisterCompany;
            ViewData["IsValidDns"] = IsValidDns;
            ViewData["EnableUp"] = enableup;
            if (string.IsNullOrEmpty(ViewData["ShowNameCompany"].ToString()))
                ViewData["ShowNameCompany"] = NameCompany[0];
        }
        #endregion
    }
}
