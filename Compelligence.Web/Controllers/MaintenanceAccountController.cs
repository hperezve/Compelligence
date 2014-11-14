using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Util;
using Compelligence.Domain.Entity.Views;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Compelligence.Util.Type;
using Compelligence.Security.Filters;
using Compelligence.DataTransfer.Entity;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Util.Collections;
using System.Text.RegularExpressions;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    public class MaintenanceAccountController : GenericController
    {
        private IClientCompanyService _clientCompanyService;

        private IUserProfileService _userProfileService;

        private string _credentialPath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["SecurityXmlPath"];

        //cypher method
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        //key for method of cypher
        private string rsakey = "<RSAKeyValue><Modulus>+McsVBKK4vbNpoEs8t+qPv/j92n5JGHWs+CX1GngXUTgiKVjb7VKO+N20hauwuKoEQrq1W9lb9uSGqgCXXl/xdvLf++WmbsPnlV6EstT3/8NPnZhthdZe5q/gJLVdr4MO40MfCPO9sLEYSugfmXVl/J3iMAZ76zXZddSHUyvYds=</Modulus><Exponent>AQAB</Exponent><P>/2gs1ucNXyP6qgWYC1VwCFB6iQ4QnQxwhYjcXq7xPfA8nH86Wd3jqKi1h/Uy6b4HMgWN57oy0zOZF4hwnjEUwQ==</P><Q>+VsOt9X92qimf+V8HhyQbij6pTfVyJrzqwmqSzZleN3M4PgaLOHiTdBjxX5m8N0oB1qpVk80hg9ASFDCdVcRmw==</Q><DP>GBmUYZLbyCZ0+KYeerNNJvuxFE2nc6pA09jeMnD/goCwt3Op5eDyInAI8RNKApRTyXyMr1j6gsNTpszRE5w+AQ==</DP><DQ>bGoItikWHBGjgov2MOleambqwxbJnlSwiLbFEbpu1+QnhdCZINZ9HDP0jRNuEl81Xi0u3tXFElxjKI3kXjwi7Q==</DQ><InverseQ>a1Wm1JduX0ku/xk7kW9DJE87X7n3G0u3QsHMkeOxZXABWKszJSMHnmmNTZHRC7T4WN6O4p5PLKuA3PpJy0AcqQ==</InverseQ><D>U+FMB/jMjWQryNVLI4TYcS90XMqFb4fxjWrPppYgSTJdBpXFJjgZFIJIYOO2/wzPIBfvQBG0QPfmlkhJq9y4gIK/GdcR46Ofw575Ms/ASc9SDQ7cGbZPVue8zHKG7DWk+doZL2ccZRas10mJF3H+OqsVuqzBCEYuEwKBaW/l04E=</D></RSAKeyValue>";

        public IEmailService EmailService { get; set; }

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

        public IResourceService ResourceService { get; set; }

        public ICountryService CountryService { get; set; }

        public IApplicationConfigurationService ApplicationConfigurationService { get; set; }

        public override string CurrentUser
        {
            get { return StringUtility.CheckNull(Session["ManAccUserId"] as string); }
        }

        public override string CurrentCompany
        {
            get { return string.Empty; }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUserProfileByClientCompany(string id)
        {
            IList<UserProfile> userprofileList = UserProfileService.GetAllActiveByClientCompany(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<UserProfile>(userprofileList, "Id", "Name");
        }

        //
        // GET: /MaintenanceAccount/

        public ActionResult Index()
        {
            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
            return Index();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(FormCollection collection)
        {
            string Username = collection["Username"];
            string Password = collection["Kennwort"];
            Username = Username.Trim();
            //string[] credencial = readCredential();
            IList<string[]> credentials = readCredentialList();
            bool isUsers = false;
            foreach (string[] cd in credentials)
            {
                if (cd[0].Equals(Username) && cd[1].Equals(Password))
                {
                    isUsers = true;
                }
            }

            //if (credencial[0].Equals(Username) && credencial[1].Equals(Password))
            if (isUsers)
            {
                Session["ManAccUserId"] = "admingeneral";
                return List();
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Remove("ManAccUserId");
            return View("Index");
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List()
        {
            IList<ClientCompanyAllView> clientCompanyCollection = ClientCompanyService.GetViewAllActive();
            IList<ClientCompanyAllView> clientCompanyToDelete = ClientCompanyService.GetAllViewByToDelete(ClientCompanyToDelete.Yes);
            if (clientCompanyToDelete != null && clientCompanyToDelete.Count > 0)
            {
                ViewData["CompaniesToDeleteCount"] = clientCompanyToDelete.Count;
                ViewData["CompaniesToDeletelist"] = clientCompanyToDelete;
            }
            else
            {
                ViewData["CompaniesToDeleteCount"] = 0;
                ViewData["CompaniesToDeletelist"] = string.Empty;
            }
            IList<SelectList> lista = new List<SelectList>();
            ViewData["ClientCompanyCount"] = 0;

            if (clientCompanyCollection != null)
            {
                ViewData["ClientCompanyCount"] = clientCompanyCollection.Count;

                IList<ResourceObject> typeList = ResourceService.GetAll<ClientCompanyStatus>();//verificar si esta bien usar este estado

                foreach (ClientCompanyAllView client in clientCompanyCollection)
                {
                    lista.Add(new SelectList(typeList, "Id", "Value", client.Status));
                }
            }

            ViewData["ListStatusList"] = lista.ToArray();

            return View("List", clientCompanyCollection);
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult ResetUserPassword(string id)
        {
            SetFormData(id);
            //SetFormData();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResetUserPassword(FormCollection collection)
        {
            UserProfileService.ResetPassword(collection["UserId"]);

            return List();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult DeleteAccount()
        {
            SetFormData();

            return View();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult DeleteAccountFromList(string id)
        {
            ClientCompanyService.DeleteClientCompany(id);

            return List();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult UpdateToDeleteFromList(string id)
        {

            ClientCompanyService.UpdateToDeleteClientCompany(id);

            return RedirectToAction("List");
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult PermanentlyDeleteCompanies(string id)
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string result = string.Empty;
            foreach (string idT in ids)
            {
                if(!string.IsNullOrEmpty(idT))
                ClientCompanyService.DeleteClientCompany(idT);
            }
            result = "succesfull";
            return new JsonResult() { Data = result };
        }

        public ActionResult ReturnToTopList()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            foreach (string idT in ids)
            {
                if(!string.IsNullOrEmpty(idT))
                ClientCompanyService.UpdateNoToDeleteClientCompany(idT);
            }
            string result = "succesfull";
            return new JsonResult() { Data = result };
        }

        public ActionResult GetRedirection(string id, string clientCompanyId)
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);
            UserProfile userProfile = UserProfileService.GetById(id);
            //return RedirectToAction("LoginByUser", "Home", new { id = id, clientCompanyId = clientCompanyId });
            string newUrl = GetNewUrlByCompany(clientCompany.Dns, id, clientCompanyId);
            return new JsonResult() { Data = newUrl };
        }

        private string GetNewUrlByCompany(string dns, string userid, string clientcompanyId)
        {
            string newUrl = string.Empty;
            string urlLocationPart = string.Empty;
            string urlActionPart = "/Home.aspx/LoginByUser/" + userid + "?clientCompanyId=" + clientcompanyId;
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            currentUrl = currentUrl.Replace("localhost:1188", "c3.compelligence.com/exec3");
            if (currentUrl.IndexOf("localhost") != -1)
            {
                int posLH = currentUrl.IndexOf("/MaintenanceAccount.aspx");
                urlLocationPart = currentUrl.Substring(0, posLH);
            }
            
            else
            {
                int PosAc = currentUrl.IndexOf("/MaintenanceAccount.aspx");
                int PosPoint = currentUrl.IndexOf(".");
                int beginIndex = currentUrl.IndexOf("://");

                string firstString = currentUrl.Substring(0, PosPoint);
                string beginOfCurrentHost = firstString.Substring(beginIndex + 3);
                if (PosPoint > 0)
                {
                    string urlbuild = string.Empty;
                    if ((beginOfCurrentHost.IndexOf("compelligence") != -1) || (beginOfCurrentHost.IndexOf("COMPELLIGENCE") != -1) || (beginOfCurrentHost.IndexOf("COMPETITIVETEAM") != -1) || (beginOfCurrentHost.IndexOf("competitiveteam") != -1))
                    {
                        urlbuild = "." + currentUrl.Substring(beginIndex + 3, PosAc - (beginIndex + 3));
                    }
                    else
                    {
                        urlbuild = currentUrl.Substring(PosPoint, PosAc - PosPoint);
                    }

                    urlLocationPart = dns + urlbuild;
                }
            }
            newUrl = urlLocationPart + urlActionPart;
            return newUrl;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAccount(FormCollection collection)
        {
            ClientCompanyService.DeleteClientCompany(collection["Name"]);

            return View("Index");
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult EnableAccount()
        {
            IList<ClientCompany> clientcompanyList = ClientCompanyService.GetByStatusClientCompany(ClientCompanyStatus.Disabled);

            ViewData["ClientCompanyList"] = new SelectList(clientcompanyList, "ClientCompanyId", "Name");

            return View();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult EnableAccountFromList(string id)
        {
            ClientCompanyService.EnableClientCompany(id);

            return List();
        }

        //[AuthenticationFilter]
        //[AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EnableAccount(FormCollection collection)
        {
            ClientCompanyService.EnableClientCompany(collection["Name"]);

            return View("Index");
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult DisableAccount()
        {
            IList<ClientCompany> clientcompanyList = ClientCompanyService.GetByStatusClientCompany(ClientCompanyStatus.Enabled);

            ViewData["ClientCompanyList"] = new SelectList(clientcompanyList, "ClientCompanyId", "Name");

            return View();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult DisableAccountFromList(string id)
        {
            ClientCompanyService.DisableClientCompany(id);

            return List();
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateCompanyStatus(string id)
        {
            string state = StringUtility.CheckNull(Request["sts"]);

            switch (state)
            {
                case "ENBL": ClientCompanyService.EnableClientCompany(id);
                    break;
                case "DSBL": ClientCompanyService.DisableClientCompany(id);
                    break;
                case "WARN": ClientCompanyService.WarningClientCompany(id);
                    break;
            }

            return List();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DisableAccount(FormCollection collection)
        {
            ClientCompanyService.DisableClientCompany(collection["Name"]);

            return View("Index");
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult ViewFormResetPassword(string id)
        {
            id = HttpUtility.HtmlEncode(id);
            IList<UserProfile> userprofileList = UserProfileService.GetUserProfileByClientCompanyId(id);
            if (userprofileList == null)
            {
                userprofileList = new List<UserProfile>();
            }
            ViewData["AssignedToList"] = new SelectList(userprofileList, "id", "name");

            return View();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult ViewFormCredential()
        {

            return View("FormCredential");
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult ChangeCredential(FormCollection collection)
        {
            string user = collection["olduser"].Trim();
            string password = collection["oldKennwort"].Trim();
            string newuser = collection["user"].Trim();
            string newpassword = collection["Kennwort"].Trim();
            bool error = false;
            bool oldInfoEmpty = false;
            if (newuser.Length == 0)
            {
                ValidationDictionary.AddError("user", LabelResource.UserEmailEmptyError);
                error = true;
            }
            if (newpassword.Length == 0)
            {
                ValidationDictionary.AddError("password", LabelResource.UserPasswordEmptyError);
                error = true;
            }
            if (user.Length == 0)
            {
                ValidationDictionary.AddError("olduser", LabelResource.OldUserEmailEmptyError);
                error = true;
                oldInfoEmpty = true;
            }
            if (password.Length == 0)
            {
                ValidationDictionary.AddError("oldpassword", LabelResource.OldUserPasswordEmptyError);
                error = true;
                oldInfoEmpty = true;
            }

            IList<string[]> credentials = readCredentialList();
            bool isUsers = false;
            foreach (string[] cd in credentials)
            {
                if (cd[0].Equals(user) && cd[1].Equals(password))
                {
                    isUsers = true;
                }
            }
            if (!oldInfoEmpty)
            {
                if (isUsers && !error)
                {
                    if (Validator.IsStrongPassword(newpassword))
                    {
                        saveCredential(new String[] { newuser, newpassword });
                    }
                    else
                    {
                        ValidationDictionary.AddError("newpassword", LabelResource.UserPasswordExpression);
                        error = true;
                    }
                }
                else
                {
                    ValidationDictionary.AddError("olduser", LabelResource.OldUserEmailMatchError);
                    ValidationDictionary.AddError("oldpassword", LabelResource.OldUserPasswordMatchError);
                    error = true;
                }
            }
            if (!error)
            {
                return List();
            }
            else
            {
                return View("FormCredential");
            }
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult ConfigurationEmail()
        {
            string email = readInfoEmail();
            ViewData["Email"] = email;
            return View("ConfigurationEmail");
        }

        private string readInfoEmail()
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream READER = new FileStream(this._credentialPath + "Credential" + ".xml",
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.ReadWrite);
            xmlDocument.Load(READER);
            XmlElement root = xmlDocument.DocumentElement;
            string email = root.GetElementsByTagName("email")[0].InnerText;
            READER.Close();
            email = email.Trim();
            return email;
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeEmail(FormCollection collection)
        {
            bool error = false;
            string email = collection["Email"].Trim();
            if (!string.IsNullOrEmpty(email) && StringUtility.IsValidStringER(email, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"))
            {
                UpdateEmail(email);
                error = false;
            }
            else
            {
                ValidationDictionary.AddError("Email", "Email incorrect");
                error = true;
            }

            if (!error)
            {
                return List();
            }
            else
            {
                return View("ConfigurationEmail");
            }
        }

        private void UpdateEmail(string email)
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream READER = new FileStream(this._credentialPath + "Credential" + ".xml",
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.ReadWrite);

            xmlDocument.Load(READER);
            XmlElement root = xmlDocument.DocumentElement;
            XmlNodeList nodelistemail = root.GetElementsByTagName("email");
            nodelistemail[0].InnerText = email;
            FileStream WRITER = new FileStream(this._credentialPath + "Credential" + ".xml",
                                                FileMode.Truncate,
                                                FileAccess.Write,
                                                FileShare.ReadWrite);
            xmlDocument.Save(WRITER);
            WRITER.Close();
        }

        private void SetFormData(string ClientCompanyId)
        {
            IList<ClientCompany> clientcompanyList = ClientCompanyService.GetAllActive();

            IList<UserProfile> userprofileList = UserProfileService.GetAllActiveByClientCompany(ClientCompanyId);

            ViewData["ClientCompanyList"] = new SelectList(clientcompanyList, "ClientCompanyId", "Name", ClientCompanyId);
            ViewData["AssignedToList"] = new SelectList(userprofileList, "id", "name");

        }

        private void SetFormData()
        {
            IList<ClientCompany> clientcompanyList = ClientCompanyService.GetAllActive();

            ViewData["ClientCompanyList"] = new SelectList(clientcompanyList, "ClientCompanyId", "Name");
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditCompany(string id)
        {
            try
            {
                ClientCompany clientCompany = ClientCompanyService.GetById(id);
                UserProfile rootUser = UserProfileService.GetOriginalAdminSystem(id);
                ClientCompanySubscription companySubscription = ClientCompanyService.GetSubscriptionByCompany(id);

                ClientCompanyPayment nextPayment = ClientCompanyService.GetNextPaymentByCompany(id);
                ClientCompanyPayment lastPayment = ClientCompanyService.GetLastPaymentByCompany(id);

                if (clientCompany != null && rootUser != null)
                {
                    ClientCompanyDTO companyDTO = new ClientCompanyDTO();

                    companyDTO.CompanyId = clientCompany.ClientCompanyId;
                    companyDTO.CompanyEmail = clientCompany.Email;
                    companyDTO.CompanyName = clientCompany.Name;
                    companyDTO.CompanyDescription = clientCompany.Description;
                    companyDTO.CompanyDns = clientCompany.Dns;
                    companyDTO.CompanyAddress = clientCompany.Address;
                    companyDTO.CompanyCity = clientCompany.City;
                    companyDTO.CompanyCountryCode = clientCompany.CountryCode;
                    companyDTO.CompanyPhone = clientCompany.Phone;
                    companyDTO.CompanyState = clientCompany.State;
                    companyDTO.CompanyZipCode = clientCompany.ZipCode;
                    companyDTO.CompanyStatus = clientCompany.Status;

                    companyDTO.SalesForceUser = clientCompany.SalesForceUser;
                    companyDTO.SalesForceToken = clientCompany.SalesForceToken;

                    companyDTO.UserId = rootUser.Id;
                    companyDTO.UserFirstName = rootUser.FirstName;
                    companyDTO.UserLastName = rootUser.LastName;
                    companyDTO.UserEmail = rootUser.Email;
                    companyDTO.UserAddress = rootUser.Address;
                    companyDTO.UserCity = rootUser.City;
                    companyDTO.UserCountryCode = rootUser.CountryCode;
                    companyDTO.UserState = rootUser.Department;
                    companyDTO.UserZipCode = rootUser.ZipCode;
                    companyDTO.UserPhone = rootUser.Phone;
                    companyDTO.UserFax = rootUser.Fax;

                    if (companySubscription != null)
                    {
                        companyDTO.SubscriptionRate = FormatUtility.GetFormatValue("{0:#,0.00}", companySubscription.Rate);
                        companyDTO.SubscriptionDueDate = DateTimeUtility.ConvertToString(companySubscription.DueDate, "MM/dd/yyyy");
                        companyDTO.SubscriptionCurrencyType = companySubscription.CurrencyType;
                        companyDTO.SubscriptionMaxUsers = Convert.ToString(companySubscription.MaxUsers);
                    }

                    if (lastPayment != null)
                    {
                        companyDTO.PaymentLastDate = DateTimeUtility.ConvertToString(lastPayment.PaymentDate, "MM/dd/yyyy");
                        companyDTO.PaymentLastAmount = FormatUtility.GetFormatValue("{0:#,0.00}", lastPayment.Amount);
                        companyDTO.PaymentLastCurrencyType = lastPayment.CurrencyType;
                    }

                    if (nextPayment != null)
                    {
                        companyDTO.PaymentNextDate = DateTimeUtility.ConvertToString(nextPayment.PaymentDate, "MM/dd/yyyy");
                        companyDTO.PaymentNextStatus = nextPayment.Status;
                        companyDTO.PaymentNextAmount = FormatUtility.GetFormatValue("{0:#,0.00}", nextPayment.Amount);
                        companyDTO.PaymentNextCurrencyType = nextPayment.CurrencyType;
                    }

                    if (string.IsNullOrEmpty(companyDTO.SubscriptionCurrencyType))
                    {
                        companyDTO.SubscriptionCurrencyType = CurrencyType.UnitedStateDollar;
                    }

                    if (string.IsNullOrEmpty(companyDTO.PaymentLastCurrencyType))
                    {
                        companyDTO.PaymentLastCurrencyType = CurrencyType.UnitedStateDollar;
                    }

                    if (string.IsNullOrEmpty(companyDTO.PaymentNextCurrencyType))
                    {
                        companyDTO.PaymentNextCurrencyType = CurrencyType.UnitedStateDollar;
                    }

                    if (string.IsNullOrEmpty(companyDTO.PaymentNextStatus))
                    {
                        companyDTO.PaymentNextStatus = CompanyPaymentStatus.Pending;
                    }

                    SetFormCompanyData();
                    SetClientSettingsToForm(clientCompany);
                    return View("EditCompany", companyDTO);
                }
                else
                {
                    return List();
                }
            }
            catch (Exception ex)
            {
                ////////////////
                return List();
            }
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCompany(ClientCompanyDTO clientCompanyForm, FormCollection formCollection)
        {
            try
            {
                ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyForm.CompanyId);
                UserProfile rootUser = UserProfileService.GetOriginalAdminSystem(clientCompanyForm.CompanyId);

                if (clientCompany != null && rootUser != null &&
                    ValidateEditCompanyFormData(clientCompanyForm, formCollection))
                {
                    clientCompanyForm.CurrentUser = CurrentUser;
                    ClientCompanyService.UpdateCompanyDetails(clientCompanyForm);
                    ModelState.Clear();
                    return EditCompany(clientCompany.ClientCompanyId);
                }
                else
                {
                    if (rootUser != null)
                    {
                        clientCompanyForm.UserId = rootUser.Id;
                        clientCompanyForm.UserEmail = rootUser.Email;
                    }

                    SetFormCompanyData();
                    return View("EditCompany", clientCompanyForm);
                }
            }
            catch (Exception ex)
            {
                ////////////////
                return View("EditCompany", clientCompanyForm);
            }
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditCompanySalesforce(string id)
        {
            try
            {
                ClientCompany clientCompany = ClientCompanyService.GetById(id);

                if (clientCompany != null)
                {
                    CompanySalesforceDTO companySalesforce = new CompanySalesforceDTO();
                    companySalesforce.CompanyId = clientCompany.ClientCompanyId;
                    companySalesforce.SalesForceToken = clientCompany.SalesForceToken;
                    companySalesforce.SalesForceUser = clientCompany.SalesForceUser;
                    companySalesforce.SalesForcePassword = clientCompany.SalesForcePassword;

                    return View("EditCompanySalesforce", companySalesforce);
                }
                else
                {
                    return List();
                }
            }
            catch (Exception ex)
            {
                ////////////////
                return List();
            }
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCompanySalesforce(CompanySalesforceDTO companySalesforce, FormCollection formCollection)
        {
            try
            {
                ClientCompany clientCompany = ClientCompanyService.GetById(formCollection["CompanyId"]);

                if (clientCompany != null)
                {
                    if (ValidateEditCompanySalesforceFormData(companySalesforce, formCollection))
                    {
                        clientCompany.SalesForceToken = companySalesforce.SalesForceToken;
                        clientCompany.SalesForceUser = companySalesforce.SalesForceUser;
                        clientCompany.SalesForcePassword = companySalesforce.SalesForcePassword;

                        clientCompany.LastChangedDate = DateTime.Now;

                        ClientCompanyService.Update(clientCompany);
                        ModelState.Clear();
                        return EditCompany(clientCompany.ClientCompanyId);
                    }
                    else
                    {
                        companySalesforce.CompanyName = clientCompany.Name;

                        return View("EditCompanySalesforce", companySalesforce);
                    }
                }
                else
                {
                    return List();
                }
            }
            catch (Exception ex)
            {
                ////////////////
                return List();
            }
        }

        private bool ValidateEditCompanyFormData(ClientCompanyDTO clientCompanyForm, FormCollection formCollection)
        {
            long numUsers = UserProfileService.CountByCompany(clientCompanyForm.CompanyId);

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyName))
            {
                ValidationDictionary.AddError("CompanyName", LabelResource.CompanyNameRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyAddress))
            {
                ValidationDictionary.AddError("CompanyAddress", LabelResource.CompanyAddressRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyCity))
            {
                ValidationDictionary.AddError("CompanyCity", LabelResource.CompanyCityRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyState))
            {
                ValidationDictionary.AddError("CompanyState", LabelResource.CompanyStateRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyZipCode))
            {
                ValidationDictionary.AddError("CompanyZipCode", LabelResource.CompanyZipCodeRequiredError);
            }
            else if (!StringUtility.IsValidStringER(clientCompanyForm.CompanyZipCode, "(\\d){1,5}"))
            {
                ValidationDictionary.AddError("CompanyZipCode", LabelResource.CompanyZipCodeMatchError);
            }
            else if (clientCompanyForm.CompanyZipCode.Length > 5)
            {
                ValidationDictionary.AddError("CompanyZipCode", LabelResource.CompanyZipCodeLengthError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyCountryCode))
            {
                ValidationDictionary.AddError("CompanyCountryCode", LabelResource.CompanyCountryCodeRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyPhone))
            {
                ValidationDictionary.AddError("CompanyPhone", LabelResource.CompanyPhoneRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyDns))
            {
                ValidationDictionary.AddError("CompanyDns", LabelResource.CompanyDnsRequiredError);
            }
            else if (!Validator.IsUrl("http://" + clientCompanyForm.CompanyDns + LabelResource.DomainURL))
            {
                ValidationDictionary.AddError("CompanyDns", LabelResource.CompanyDnsFormatError);
            }
            else if (!ClientCompanyService.IsValidDns(clientCompanyForm.CompanyId, clientCompanyForm.CompanyDns))
            {
                ValidationDictionary.AddError("CompanyDns", "Company Dns is already registered");
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyEmail))
            {
                ValidationDictionary.AddError("CompanyEmail", LabelResource.CompanyEmailRequiredError);
            }
            else if (!Validator.IsEmailOrWhite(clientCompanyForm.CompanyEmail))
            {
                ValidationDictionary.AddError("CompanyEmail", LabelResource.CompanyEmailFormatError);
            }
            else if (!ClientCompanyService.IsValidEmail(clientCompanyForm.CompanyId, clientCompanyForm.CompanyEmail))
            {
                ValidationDictionary.AddError("CompanyEmail", "Company Email is already registered");
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.CompanyDescription))
            {
                ValidationDictionary.AddError("CompanyDescription", LabelResource.CompanyDescriptionRequiredError);
            }

            if (!(Validator.IsBlankOrNull(clientCompanyForm.SubscriptionRate) || Validator.IsDecimal(clientCompanyForm.SubscriptionRate)))
            {
                ValidationDictionary.AddError("SubscriptionRate", LabelResource.CompanySubscriptionRateFormatError);
            }

            if (!(Validator.IsBlankOrNull(clientCompanyForm.SubscriptionDueDate) || Validator.IsDate(clientCompanyForm.SubscriptionDueDate, "MM/dd/yyyy")))
            {
                ValidationDictionary.AddError("SubscriptionDueDate", LabelResource.CompanySubscriptionDueDateFormatError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.SubscriptionMaxUsers))
            {
                ValidationDictionary.AddError("SubscriptionMaxUsers", LabelResource.CompanySubscriptionMaxUsersRequiredError);
            }
            else if (!Validator.IsInt(clientCompanyForm.SubscriptionMaxUsers))
            {
                ValidationDictionary.AddError("SubscriptionMaxUsers", LabelResource.CompanySubscriptionMaxUsersFormatError);
            }
            else if (Convert.ToInt32(clientCompanyForm.SubscriptionMaxUsers) < numUsers)
            {
                ValidationDictionary.AddError("SubscriptionMaxUsers", string.Format(LabelResource.CompanySubscriptionMaxUsersMinNumError, clientCompanyForm.CompanyName, numUsers));
            }

            if (!(Validator.IsBlankOrNull(clientCompanyForm.PaymentNextAmount) || Validator.IsDecimal(clientCompanyForm.PaymentNextAmount)))
            {
                ValidationDictionary.AddError("PaymentNextAmount", LabelResource.CompanyPaymentNextAmountFormatError);
            }

            if (!(Validator.IsBlankOrNull(clientCompanyForm.PaymentNextDate) || Validator.IsDate(clientCompanyForm.PaymentNextDate, "MM/dd/yyyy")))
            {
                ValidationDictionary.AddError("PaymentNextDate", LabelResource.CompanyPaymentNextDateFormatError);
            }

            if (!(Validator.IsBlankOrNull(clientCompanyForm.PaymentLastAmount) || Validator.IsDecimal(clientCompanyForm.PaymentLastAmount)))
            {
                ValidationDictionary.AddError("PaymentLastAmount", LabelResource.CompanyPaymentLastAmountFormatError);
            }

            if (!(Validator.IsBlankOrNull(clientCompanyForm.PaymentLastDate) || Validator.IsDate(clientCompanyForm.PaymentLastDate, "MM/dd/yyyy")))
            {
                ValidationDictionary.AddError("PaymentLastDate", LabelResource.CompanyPaymentLastDateFormatError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserFirstName))
            {
                ValidationDictionary.AddError("UserFirstName", LabelResource.UserFirstNameRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserLastName))
            {
                ValidationDictionary.AddError("UserLastName", LabelResource.UserLastNameRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserCountryCode))
            {
                ValidationDictionary.AddError("UserCountryCode", LabelResource.UserCountryIdRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserCity))
            {
                ValidationDictionary.AddError("UserCity", LabelResource.UserCityRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserState))
            {
                ValidationDictionary.AddError("UserState", LabelResource.UserDepartmentRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserZipCode))
            {
                ValidationDictionary.AddError("UserZipCode", LabelResource.UserZipCodeRequiredError);
            }
            else if (!StringUtility.IsValidStringER(clientCompanyForm.UserZipCode, "(\\d){1,5}"))
            {
                ValidationDictionary.AddError("UserZipCode", LabelResource.UserZipCodeMatchError);
            }
            else if (clientCompanyForm.UserZipCode.Length > 5)
            {
                ValidationDictionary.AddError("UserZipCode", LabelResource.UserZipCodeLengthError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserAddress))
            {
                ValidationDictionary.AddError("UserAddress", LabelResource.UserAddressRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompanyForm.UserPhone))
            {
                ValidationDictionary.AddError("UserPhone", LabelResource.UserPhoneRequiredError);
            }

            return ValidationDictionary.IsValid;
        }

        private bool ValidateEditCompanySalesforceFormData(CompanySalesforceDTO companySalesforce, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(companySalesforce.SalesForceToken))
            {
                ValidationDictionary.AddError("SalesForceToken", LabelResource.CompanySalesForceTokenRequiredError);
            }

            if (Validator.IsBlankOrNull(companySalesforce.SalesForceUser))
            {
                ValidationDictionary.AddError("SalesForceUser", LabelResource.CompanySalesForceUserRequiredError);
            }

            if (Validator.IsBlankOrNull(companySalesforce.SalesForcePassword))
            {
                ValidationDictionary.AddError("SalesForcePassword", LabelResource.CompanySalesForcePasswordRequiredError);
            }
            else if (companySalesforce.SalesForcePassword.Length < 8)
            {
                ValidationDictionary.AddError("SalesForcePassword", LabelResource.CompanySalesForcePasswordLengthError);
            }
            else if (string.Compare(companySalesforce.SalesForcePassword, companySalesforce.SalesForceRePassword) != 0)
            {
                ValidationDictionary.AddError("SalesForcePassword", LabelResource.CompanySalesForceRePasswordMatchError);
            }

            return ValidationDictionary.IsValid;
        }

        private void SetFormCompanyData()
        {
            //IList<Country> countryList = CountryService.GetAllActive();
            IList<CountryFirstUSAView> countryList = CountryService.GetFirstUSA();
            IList<ResourceObject> statusList = ResourceService.GetAll<ClientCompanyStatus>();
            IList<ResourceObject> currencyList = ResourceService.GetAll<CurrencyType>();
            IList<ResourceObject> paymentStatusList = ResourceService.GetAll<CompanyPaymentStatus>();

            ViewData["CountryCodeList"] = new SelectList(countryList, "CountryCode", "Name");
            ViewData["CompanyStatusList"] = new SelectList(statusList, "Id", "Value");
            ViewData["PaymentStatusList"] = new SelectList(paymentStatusList, "Id", "Value");
            ViewData["CurrencyList"] = new SelectList(currencyList, "Id", "Value");
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetUsersOfCompany(string id)
        {
            ViewData["CountUser"] = string.Empty;
            ViewData["CompanyName"] = string.Empty;
            IList<UserProfileView> userProfileList = UserProfileService.GetUserProfileAllViewByCompany(id);
            ClientCompany clientCompany = ClientCompanyService.GetById(id);
            if (clientCompany != null)
            {
                ViewData["CompanyName"] = clientCompany.Name;
            }
            if (userProfileList != null && userProfileList.Count > 0)
            {
                ViewData["CountUser"] = userProfileList.Count;
            }
            return View("ListUser", userProfileList);
        }

        protected bool ValidateFormData(FormCollection formCollection)
        {
            return ValidationDictionary.IsValid;
        }

        //read administrator credential 
        private string[] readCredential()
        {
            XmlDocument xmlDocument = new XmlDocument();

            FileStream READER = new FileStream(this._credentialPath + "Credential" + ".xml",
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.ReadWrite);

            xmlDocument.Load(READER);

            XmlElement root = xmlDocument.DocumentElement;

            string user = root.GetElementsByTagName("user")[0].InnerText;
            string password = decrypt(root.GetElementsByTagName("password")[0].InnerText);

            string[] credential = { user, password };

            READER.Close();
            return credential;
        }

        //read administrator credential 
        private IList<string[]> readCredentialList()
        {
            XmlDocument xmlDocument = new XmlDocument();

            FileStream READER = new FileStream(this._credentialPath + "Credential" + ".xml",
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.ReadWrite);

            xmlDocument.Load(READER);

            XmlElement root = xmlDocument.DocumentElement;
            //string[] credentials = new string[2];
            IList<string[]> credentialList = new List<string[]>();
            for (int i = 0; i < 2; i++)
            {
                string userA = root.GetElementsByTagName("user")[i].InnerText;
                string passwordA = decrypt(root.GetElementsByTagName("password")[i].InnerText);

                string[] credentialA = { userA, passwordA };
                credentialList.Add(credentialA);
            }
            //string user = root.GetElementsByTagName("user")[0].InnerText;
            //string password = decrypt(root.GetElementsByTagName("password")[0].InnerText);

            //string[] credential = { user, password };

            READER.Close();
            return credentialList;
        }

        //save or update credential to xml file 
        private void saveCredential(String[] credential)
        {
            XmlDocument xmlDocument = new XmlDocument();

            FileStream READER = new FileStream(this._credentialPath + "Credential" + ".xml",
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.ReadWrite);

            xmlDocument.Load(READER);


            XmlElement root = xmlDocument.DocumentElement;

            XmlNodeList nodelistuser = root.GetElementsByTagName("user");
            XmlNodeList nodelistpassword = root.GetElementsByTagName("password");

            nodelistuser[0].InnerText = credential[0];
            nodelistpassword[0].InnerText = encrypt(credential[1]);

            FileStream WRITER = new FileStream(this._credentialPath + "Credential" + ".xml",
                                                FileMode.Truncate,
                                                FileAccess.Write,
                                                FileShare.ReadWrite);

            xmlDocument.Save(WRITER);
            WRITER.Close();

        }

        //Get:ForgotPassword
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //POST:ForgotPassword
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendForgotPassword()
        {
            String sAccount = Request["Username"];
            UserProfile userprofile = UserProfileService.GetByEmail(sAccount);
            if (userprofile == null)
            {
                ViewData["Message"] = "Your email doesn't exist..!";
                return View("ForgotPassword");
            }

            EmailService.SendForgotPasswordEmail(userprofile);
            ViewData["Message"] = "Your password will be sent to your email..!";
            return View("ForgotPassword");
        }


        //Method that cyphers the password of administrator, we as encryption method RSA 
        private String encrypt(String input)
        {

            byte[] _password;
            rsa.FromXmlString(rsakey);
            _password = rsa.Encrypt(UTF8Encoding.UTF8.GetBytes(input), false);

            string passwordEnc = Convert.ToBase64String(_password, 0, _password.Length);

            return passwordEnc;
        }

        //Method that decyphers the password of administrator, using RSA method
        private String decrypt(String input)
        {
            byte[] data, datades;
            data = Convert.FromBase64String(input);
            rsa.FromXmlString(rsakey);

            datades = rsa.Decrypt(data, false);

            return UTF8Encoding.UTF8.GetString(datades);

        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateNextPaymentDate(string id)
        {
            string duedate = Request["npd"];
            if ((!string.IsNullOrEmpty(id)) && (!string.IsNullOrEmpty(duedate))
                && Validator.IsDate(duedate, "MM/dd/yyyy"))
            {
                string currentUser = CurrentUser;
                ClientCompanySubscription companySubscription = ClientCompanyService.GetSubscriptionByCompany(id);

                if (companySubscription == null)
                {
                    companySubscription = ClientCompanyService.CreateDefaultCompanySubscription(id, CurrentUser);
                }

                if (companySubscription != null)
                {
                    ClientCompanyPayment nextCompanyPayment = ClientCompanyService.GetNextPaymentByCompany(id);
                    if (nextCompanyPayment == null)
                    {
                        nextCompanyPayment = new ClientCompanyPayment();
                        nextCompanyPayment.Amount = 0;
                        nextCompanyPayment.CurrencyType = CurrencyType.UnitedStateDollar;
                        nextCompanyPayment.Status = CompanyPaymentStatus.Pending;
                        nextCompanyPayment.SubscriptionId = companySubscription.Id;
                        nextCompanyPayment.CreatedBy = currentUser;
                        nextCompanyPayment.CreatedDate = DateTime.Now;
                        nextCompanyPayment.ClientCompany = id;
                    }

                    nextCompanyPayment.PaymentDate = DateTimeUtility.ConvertToDate(duedate, "MM/dd/yyyy");
                    nextCompanyPayment.LastChangedBy = currentUser;
                    nextCompanyPayment.LastChangedDate = DateTime.Now;

                    ClientCompanyService.SaveOrUpdateCompanyPayment(nextCompanyPayment);
                }
            }
            // return View();
            return null;
        }

        private void SetClientSettingsToForm(ClientCompany clientCompany)
        {
            ViewData["ShowDashboardTab"] = false;
            ViewData["ShowCalendarTab"] = false;
            ViewData["DontSendAnyMailSet"] = false;
            ViewData["UseSystemEmail"] = false;
            if (!string.IsNullOrEmpty(clientCompany.ShowDashboardTab))
            {
                if (clientCompany.ShowDashboardTab.Equals("TRUE") || clientCompany.ShowDashboardTab.Equals("True") || clientCompany.ShowDashboardTab.Equals("true"))
                {
                    ViewData["ShowDashboardTab"] = true;
                }
            }
            if (!string.IsNullOrEmpty(clientCompany.ShowCalendarTab))
            {
                if (clientCompany.ShowCalendarTab.Equals("TRUE") || clientCompany.ShowCalendarTab.Equals("True") || clientCompany.ShowCalendarTab.Equals("true"))
                {
                    ViewData["ShowCalendarTab"] = true;
                }
            }
            if (!string.IsNullOrEmpty(clientCompany.SendAnyMail))
            {
                if (clientCompany.SendAnyMail.Equals("TRUE") || clientCompany.SendAnyMail.Equals("True") || clientCompany.SendAnyMail.Equals("true"))
                {
                    ViewData["DontSendAnyMailSet"] = false;
                }
                else if (clientCompany.SendAnyMail.Equals("FALSE") || clientCompany.SendAnyMail.Equals("False") || clientCompany.SendAnyMail.Equals("false"))
                {
                    ViewData["DontSendAnyMailSet"] = true;
                }
            }
            if (!string.IsNullOrEmpty(clientCompany.UseSystemEmail))
            {
                if (clientCompany.UseSystemEmail.Equals("TRUE") || clientCompany.UseSystemEmail.Equals("True") || clientCompany.UseSystemEmail.Equals("true"))
                {
                    ViewData["UseSystemEmail"] = true;
                }
            }
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult UpdateSeting()
        {
            string key = string.Empty;
            ApplicationConfigurationId applicationConfigurationId = new ApplicationConfigurationId("BING", "ACCOUNT_KEY");
            ApplicationConfiguration applicationConfiguration = ApplicationConfigurationService.GetById(applicationConfigurationId);
            if (applicationConfiguration != null)
            {
                key = applicationConfiguration.Value;
            }
            ViewData["BingKey"] = key;
            return View();
        }

        [AuthenticationMaintenanceFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateSetting(FormCollection collection)
        {
            string bingKey = collection["BingKey"].Trim();
            ViewData["BingKey"] = bingKey;
            ApplicationConfigurationId applicationConfigurationId = new ApplicationConfigurationId("BING", "ACCOUNT_KEY");
            ApplicationConfiguration applicationConfiguration = ApplicationConfigurationService.GetById(applicationConfigurationId);
            if (applicationConfiguration != null)
            {
                applicationConfiguration.Value = bingKey;
                ApplicationConfigurationService.Update(applicationConfiguration);
            }
            else
            {
                applicationConfiguration = new ApplicationConfiguration();
                applicationConfiguration.Id = applicationConfigurationId;
                applicationConfiguration.Value = bingKey;
                ApplicationConfigurationService.Save(applicationConfiguration);
            }
            return View("UpdateSeting");
        }

        public ContentResult UpdateEditHelp()
        {
            string id = Request["id"];
            string editHelp = Request["editHelp"];
            UserProfile user = UserProfileService.GetById(id);
            if (user != null) 
            {
                user.EditHelp = editHelp;
                UserProfileService.Update(user);
            }
            return Content(string.Empty);
        }
    }
}
