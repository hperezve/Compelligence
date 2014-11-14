using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Compelligence.Common.Validation;
using Compelligence.Common.Utility;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Security.Managers;
using Compelligence.Util.Validation;
using Resources;
using Spring.Web.UI;
using System.Text;
using Common.Logging;

namespace Compelligence.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private static readonly ILog LOG = LogManager.GetLogger(typeof(HomeController));
        private IValidationDictionary _validationDictionary;

        public IEmailService EmailService { get; set; }

        public HomeController()
        {
            _validationDictionary = new ModelStateWrapper(this.ModelState);
        }

        protected IValidationDictionary ValidationDictionary
        {
            get { return _validationDictionary; }
            set { _validationDictionary = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IClientCompanyService ClientCompanyService { get; set; }

        public ISecurityGroupService SecurityGroupService { get; set; }

        public IUserProfileConfigurationService UserProfileConfigurationService { get; set; }

        #region Public ActionResult

        public ActionResult Index()
        {
            SetDefaultFormValues();
            return View();
        }
        public ActionResult TimeOut()
        {           
            ViewData["messageTimeOut"] = LabelResource.SessionTimeOutMessage;
            SetDefaultFormValues();
            return View("Index");
        }
        public ActionResult About()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
            SetDefaultFormValues();
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(FormCollection collection)
        {
            string username = collection["Username"];
            string password = collection["Kennwort"];
            ClientCompany clientCompany = GetCompanyByUrl();
            if (!string.IsNullOrEmpty(username)) { username = username.Trim(); }
            if (!string.IsNullOrEmpty(password)) { password = password.Trim(); }
            SetDefaultFormValues();

            if (isTest() || (clientCompany != null))
            {
                LOG.Info("if (isTest() || (clientCompany != null))");
                if (isTest() || ValidateCompany(clientCompany))
                {
                    LOG.Info("if (isTest() || ValidateCompany(clientCompany))");

                    if (ValidateData(username, password))
                    {
                        LOG.Info("if (ValidateData(username, password))");

                        UserProfile userProfile = GetUserProfile(username, password, clientCompany);

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
                                    return RedirectToAction("Index", "BackEnd");

                                }
                                else if (accessType.Equals(SecurityAccessType.FrontEnd))
                                {
                                    UserProfileService.UpdateLogged(userProfileLog);
                                    return RedirectToAction("Index", "Comparinator");
                                }
                                else
                                {
                                    UserProfileService.UpdateLogged(userProfileLog);
                                    return RedirectToAction("Index", "Comparinator");
                                }
                            }
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

            ViewData["Username"] = username;
            return View("Index");
        }

        public ActionResult LoginByUser(string id, string clientCompanyId)
        {

            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);
            UserProfile userProfile = UserProfileService.GetById(id);
            string username = string.Empty;
            string password = string.Empty;
            if (userProfile != null && clientCompany != null)
            {

                username = userProfile.Name;
                password = BlackBox.Decrypt(userProfile.Kennwort);
                SetDefaultFormValues();
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
                        return RedirectToAction("Index", "BackEnd");

                    }
                    else if (accessType.Equals(SecurityAccessType.FrontEnd))
                    {
                        UserProfileService.UpdateLogged(userProfileLog);
                        return RedirectToAction("Index", "Comparinator");
                    }
                    else
                    {
                        UserProfileService.UpdateLogged(userProfileLog);
                        return RedirectToAction("Index", "Comparinator");
                    }
                }
                else
                {
                    ViewData["message"] = LabelResource.LoginCompanyUrlError;
                }
            }
            else
            {
                ViewData["message"] = LabelResource.LoginCompanyUrlError;
            }

            ViewData["Username"] = username;
            return View("Index");
        }

        public void Logout()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            int pos = currentUrl.IndexOf("Home");
            string newUrl = currentUrl.Substring(0, pos) + "index.htm#index";
            Session.Abandon();
            Response.Redirect(newUrl, true);
        }
        public void GoToHome()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            int pos = currentUrl.IndexOf("Home");
            string newUrl = currentUrl.Substring(0, pos) + "index.htm#demo";
            Response.Redirect(newUrl, true);
        }
        public ActionResult LogOutResponse()
        {
            return View();
        }
        public ActionResult Shutdown()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoginToApprovedEmail(decimal id, string userid)
        {
            SetDefaultFormValues();
            ViewData["NewUserId"] = id;
            ViewData["UserApprovedId"] = userid;
            return View("IndexEmailApproved");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LoginToApprovedEmail(FormCollection collection)
        {

            string username = collection["Username"];
            string password = collection["Kennwort"];

            string indexUserId = collection["IndexNewUser"];
            string approvedUser = collection["IndexUserApproved"];
            ViewData["NewUserId"] = indexUserId;
            ViewData["UserApprovedId"] = approvedUser;
            SetDefaultFormValues();
            ClientCompany clientCompany = GetCompanyByUrl();

            if (isTest() || (clientCompany != null))
            {
                if (isTest() || ValidateCompany(clientCompany))
                {

                    if (ValidateData(username, password))
                    {
                        UserProfile userProfile = GetUserProfile(username, password, clientCompany);

                        if (userProfile != null)
                        {
                            if (ValidateUserProfileEnabled(userProfile.Status))
                            {
                                UserManager.GetInstance().SetUserProfileInCache(userProfile);
                                RoleManager.GetInstance().SetSecurityAccessInCache(userProfile);
                                SetSessionParameters(userProfile);
                                string accessType = userProfile.AccessType;
                                if (accessType.Equals(SecurityAccessType.BackEnd) || accessType.Equals(SecurityAccessType.Partner))
                                {
                                    if ((!string.IsNullOrEmpty(indexUserId)) && (approvedUser == userProfile.Id))
                                    {
                                        return RedirectToAction("ApproveNewUser", "EmailApproveRegistration", new { id = decimal.Parse(indexUserId), userId = userProfile.Id });
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "BackEnd");
                                    }

                                }
                                else if (accessType.Equals(SecurityAccessType.FrontEnd))
                                {
                                    return RedirectToAction("Index", "ContentPortal");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "ContentPortal");
                                }
                            }
                        }
                        //else
                        //{
                        //    ValidationDictionary.AddError("Username", LabelResource.LoginCompanyUrlError);
                        //}
                    }
                }
            }
            else
            {
                //ValidationDictionary.AddError("Username", LabelResource.LoginCompanyUrlError);
                // System.Windows.Forms.MessageBox.Show(LabelResource.LoginCompanyUrlError);
            }

            ViewData["Username"] = username;

            return View("IndexEmailApproved");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoginToApproveProject(decimal id, string approved)
        {
            SetDefaultFormValues();
            ViewData["ApprovedProjectId"] = id;
            ViewData["ApprovedUserId"] = approved;
            return View("IndexApproved");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LoginToApproveProject(FormCollection collection)
        {

            string username = collection["Username"];
            string password = collection["Kennwort"];

            string indexApprovedId = collection["IndexApproved"];
            string approved = collection["IndexUserApproved"];
            ViewData["ApprovedProjectId"] = indexApprovedId;
            ViewData["ApprovedUserId"] = approved;
            SetDefaultFormValues();
            ClientCompany clientCompany = GetCompanyByUrl();

            if (isTest() || (clientCompany != null))
            {
                if (isTest() || ValidateCompany(clientCompany))
                {

                    if (ValidateData(username, password))
                    {
                        UserProfile userProfile = GetUserProfile(username, password, clientCompany);

                        if (userProfile != null)
                        {
                            if (ValidateUserProfileEnabled(userProfile.Status))
                            {
                                UserManager.GetInstance().SetUserProfileInCache(userProfile);
                                RoleManager.GetInstance().SetSecurityAccessInCache(userProfile);
                                SetSessionParameters(userProfile);
                                string accessType = userProfile.AccessType;
                                if (accessType.Equals(SecurityAccessType.BackEnd) || accessType.Equals(SecurityAccessType.Partner))
                                {
                                    if ((!string.IsNullOrEmpty(indexApprovedId)) && (approved == userProfile.Id))
                                    {
                                        return RedirectToAction("ApproveProject", "EmailApprove", new { id = decimal.Parse(indexApprovedId), userId = userProfile.Id });
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "BackEnd");
                                    }

                                }
                                else if (accessType.Equals(SecurityAccessType.FrontEnd))
                                {
                                    return RedirectToAction("Index", "ContentPortal");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "ContentPortal");
                                }
                            }
                        }
                        //else
                        //{
                        //    ValidationDictionary.AddError("Username", LabelResource.LoginCompanyUrlError);
                        //}
                    }
                }
            }
            else
            {
                //ValidationDictionary.AddError("Username", LabelResource.LoginCompanyUrlError);
                // System.Windows.Forms.MessageBox.Show(LabelResource.LoginCompanyUrlError);
            }

            ViewData["Username"] = username;

            return View("IndexApproved");
        }

        public ActionResult SendMailUserHome(FormCollection form)
        {
            StringBuilder gridFilterScript = new StringBuilder();

            string Name = form["Name"];
            string Email = form["Email"];
            string PhoneNumber = form["Phone Number"];
            string Company = form["Company"];
            string Message = form["Message"];
            string AlertMessage = LabelResource.HomeEmailMessage;

            EmailService.SendEmailUserHome(Name, Email, PhoneNumber, Company, Message);

            //return RedirectToAction("Index", "Home", new { al = "test" });
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgotKennwort()
        {
            AddCaptchaImage();
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgotKennwort(FormCollection collection)
        {
            string username = collection["Username"];
            string textCaptcha = collection["Captcha"];
            if (!string.IsNullOrEmpty(username)) { username = username.Trim(); }
            if (!string.IsNullOrEmpty(textCaptcha)) { textCaptcha = textCaptcha.Trim(); }
            ClientCompany clientCompany = GetCompanyByUrl();
            if (isTest() || (clientCompany != null))
            {
                if (isTest() || ValidateCompany(clientCompany))
                {
                    if (ValidateUserName(username, textCaptcha))
                    {
                        UserProfile userProfile = GetUserProfileByEmail(username, clientCompany);
                        if (userProfile != null)
                        {
                            if (ValidateUserProfileEnabled(userProfile.Status))
                            {
                                //send email
                                UserProfileService.UpdateForgotKennwort(userProfile);
                                return RedirectToAction("UpdateSucessful");
                            }
                        }
                    }
                }
            }
            AddCaptchaImage();
            return View("ForgotKennwort");
        }
      
        public ActionResult ForgotUserName()
        {
            AddCaptchaImage();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ForgotUserName(FormCollection collection)
        {
            string firstName = collection["FirstName"];
            string lastName = collection["LastName"];
            string email = collection["Email"];
            string phone = collection["Phone"];
            string textCaptcha = collection["Captcha"];
            if (!string.IsNullOrEmpty(firstName)) { firstName = firstName.Trim(); }
            if (!string.IsNullOrEmpty(lastName)) { lastName = lastName.Trim(); }
            if (!string.IsNullOrEmpty(email)) { email = email.Trim(); }
            if (!string.IsNullOrEmpty(phone)) { phone = phone.Trim(); }
            if (!string.IsNullOrEmpty(textCaptcha)) { textCaptcha = textCaptcha.Trim(); }
            ClientCompany clientCompany = GetCompanyByUrl();
            if (isTest() || (clientCompany != null))
            {
                if (isTest() || ValidateCompany(clientCompany))
                {
                    if (ValidateForgotUser(firstName, lastName, email, textCaptcha))
                    {
                        if (string.IsNullOrEmpty(phone))
                        {
                            phone = string.Empty;
                        }
                        if (!GetUserRoot(firstName, lastName, email, phone, clientCompany))
                        {
                            ValidationDictionary.AddError("UserNotFound", LabelResource.UserNotFound);
                            AddCaptchaImage();
                            return View();
                        }                        
                        return View("ForgotUserSuccess");
                    }
                }
            }
            AddCaptchaImage();
            return View();
        }

        public ActionResult UpdateSucessful()
        {
            return View();
        }
        public ActionResult ShowCaptchaImage(string textCaptcha)
        {
            string newTextCaptcha = string.Empty;
            if (string.IsNullOrEmpty(textCaptcha)) { textCaptcha = string.Empty; }
            for (int i = 0; i < textCaptcha.Length; i++)
            {
                newTextCaptcha += textCaptcha[i];
                newTextCaptcha += "  ";//two space blank
            }
            textCaptcha = newTextCaptcha;
            var rand = new Random((int)DateTime.Now.Ticks);
            bool noisy = true;
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(250, 50);
            System.Drawing.Graphics gfx = System.Drawing.Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.FillRectangle(System.Drawing.Brushes.White, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));
            //add noise 
            if (noisy)
            {
                int i, r, x, y;
                var pen = new System.Drawing.Pen(System.Drawing.Color.Yellow);
                for (i = 1; i < 10; i++)
                {
                    pen.Color = System.Drawing.Color.FromArgb(
                    (rand.Next(0, 255)),
                    (rand.Next(0, 255)),
                    (rand.Next(0, 255)));

                    r = rand.Next(0, (250 / 3));
                    x = rand.Next(0, 250);
                    y = rand.Next(0, 50);
                    gfx.DrawEllipse(pen, x - r, y - r, r, r);
                }
            }

            //add question 
            gfx.DrawString(textCaptcha, new System.Drawing.Font("Arial Black", 28), System.Drawing.Brushes.DarkGray, 2, -3);
            System.IO.MemoryStream imageStream = new System.IO.MemoryStream();
            bmp.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.ContentType = "image/png";
            imageStream.WriteTo(Response.OutputStream);
            gfx.Dispose();
            bmp.Dispose();
            return null;
        }
        #endregion

        #region Public Methods
        public void AddCaptchaImage()
        {
            //5 characters, percentage mayus 20 and percentage number 20
            string textCaptcha = Compelligence.Util.Generator.TextCaptchaGenerator.GetTextCaptcha(5, 20, 20);
            ViewData["TextCaptcha"] = textCaptcha;
            Session["TextCaptcha"] = textCaptcha;
        }

        public void SetSessionParameters(UserProfile userProfile)
        {
            int? sessionTimeout = UserProfileConfigurationService.GetSessionTimeoutMinutes(userProfile.Id);
            Session["UserId"] = userProfile.Id;
            Session["ClientCompany"] = userProfile.ClientCompany;
            Session["EditHelp"] = userProfile.EditHelp;
            SecurityGroup securityGroup = SecurityGroupService.GetById(userProfile.SecurityGroupList[0]);
            userProfile.AccessType = securityGroup.AccessType;
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

            Session["SecurityGroupId"] = securityGroup.Id;
            UserProfileService.UpdateLogged(userProfile);
        }
        #endregion

        #region Private Methods
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
        private UserProfile GetUserProfileByEmail(string username, ClientCompany clientCompany)
        {
            UserProfile userProfile;

            if (isTest())
            {
                userProfile = UserProfileService.GetByEmail(username);
            }
            else
            {
                userProfile = UserProfileService.GetByEmail(username, clientCompany.ClientCompanyId);
            }

            if (userProfile == null)
            {
                ValidationDictionary.AddError("Username", LabelResource.LoginEmailIncorrect);
            }
            return userProfile;
        }
        private bool GetUserRoot(string firstName, string lastName, string email, string phone, ClientCompany clientCompany)
        {
            bool userRoot;
            if (isTest())
            {
                userRoot = UserProfileService.GetForgotUserName(firstName, lastName, email, phone, "");
                //userProfile = new UserProfile();
            }
            else
            {
                userRoot = UserProfileService.GetForgotUserName(firstName, lastName, email, phone, clientCompany.ClientCompanyId);
            }
            return userRoot;
        }

        private ClientCompany GetCompanyByUrl()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            int beginUrl = currentUrl.IndexOf("://");
            string companyDns = currentUrl.Substring(beginUrl + 3, currentUrl.IndexOf('.') - (beginUrl + 3));

            return ClientCompanyService.GetByDns(companyDns.ToLower());
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

        private bool ValidateForgotUser(string firstname, string lastname, string email, string textCaptcha)
        {
            
            if (Validator.IsBlankOrNull(firstname))
            {
                ValidationDictionary.AddError("FirstName", LabelResource.UserFirstNameRequiredError);
            }

            if (Validator.IsBlankOrNull(lastname))
            {
                ValidationDictionary.AddError("LastName", LabelResource.UserLastNameRequiredError);
            }
            if (Validator.IsBlankOrNull(email))
            {
                ValidationDictionary.AddError("Email", LabelResource.UserEmailRequiredError);
            }
            else if (!Validator.IsEmailOrWhite(email))
            {
                ValidationDictionary.AddError("Email", LabelResource.UserEmailFormatError);
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

        private void SetDefaultFormValues()
        {
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            //currentUrl = "https://companyb.competitivesalestool.com/"; //company enabled
            //currentUrl = "https://c6.competitivesalestool.com/"; // company disabled
            //string testing = System.Web.HttpContext.Current.Request.Url.Host;
            //string test = "companyb.competitivesalestool.com";
            //string test = "c6.competitivesalestool.com";
            //string[] NameCompany = test.Split('.');
            string[] NameCompany = System.Web.HttpContext.Current.Request.Url.Host.Split('.');
            string scheme = System.Web.HttpContext.Current.Request.Url.Scheme;
            ViewData["ShowNameCompany"] = string.Empty;
            bool IsValidDns = true;
            bool enableup = true;
            bool showRegisterCompany=true;
            bool showRegisterUser = true;
            // competitiveteam,competitivesalestool is for our test in 1and1 and godaddy
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