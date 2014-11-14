using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using System.Text;
using Compelligence.Web.Models.Web;
using System.Globalization;
using Compelligence.Util.Validation;
using Resources;
using Higuchi.Net.Pop3;
using Higuchi.Net.Mail;
using ActiveUp.Net.Mail;
using System.Threading;

namespace Compelligence.Web.Controllers
{
    public class ConfigurationController : BackEndAsyncFormController<Configuration, decimal>
    {
        public static string showMessage = string.Empty;
        public IConfigurationService ConfigurationService
        {
            get { return (IConfigurationService)_genericService; }
            set { _genericService = value; }
        }
        public IConfigurationLabelsService ConfigurationLabelsService { get; set; }

        public IEmailService EmailService { get; set; }

        public IConfigurationDefaultsService ConfigurationDefaultsService { get; set; }

        public IProjectService ProjectService { get; set; }

        public IApprovalListService ApprovalListService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IConfigurationUserTypeService ConfigurationUserTypeService { get; set; }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Configuration() 
        {
            SetEntityHelpDataToBackEnd();
            bool approval = Convert.ToBoolean(Request["Approval"]);            
           
            IList<Configuration> configurations = ConfigurationService.GetAllByClientCompany(CurrentCompany);
            ViewData["ApprovalCheck"] = false;
            //ViewData["SendEmailToNewsCheck"] = false;            
            ViewData["PopRequireSsl"] = false;
            ViewData["InboxEmail"] = string.Empty;
            ViewData["PopPort"] = string.Empty;
            ViewData["PopServer"] = string.Empty;
            ViewData["Kennwort"] = string.Empty;
            ViewData["TypeServerCheck"] = string.Empty;
            ViewData["TypeServerImapCheck"] = string.Empty;
            ViewData["TypeServerPop"] = false;
            ViewData["TypeServerImap"] = false;
            ViewData["messageSave"] = string.Empty;            
            if (configurations.Count > 0)
            {
                if (configurations[0].Approval.Equals(ProjectsApproval.True))
                {
                    ViewData["ApprovalCheck"] = true;
                }
                //if (!configurations[0].CanSendEmailToNews)
                //{
                //    ViewData["SendEmailToNewsCheck"] = true;
                //}
                ViewData["InboxEmail"] = configurations[0].InboxEmail;
                ViewData["PopPort"] = configurations[0].PopPort;
                ViewData["PopServer"] = configurations[0].PopServer;
                ViewData["Kennwort"] = configurations[0].InboxPassword;
                if (configurations[0].PopRequireSsl.Equals(ProjectsApproval.True))
                {
                    ViewData["PopRequireSsl"] = true;
                }
                if (configurations[0].TypeServer.Equals("Pop"))
                {
                    ViewData["TypeServerCheck"] = "checked";
                    ViewData["TypeServerPop"] = true;
                    
                }
                else if (configurations[0].TypeServer.Equals("Imap"))
                {
                    ViewData["TypeServerImapCheck"] = "checked";
                    ViewData["TypeServerImap"] = true;
                }
            }
            if (!(bool)ViewData["TypeServerPop"] && !(bool)ViewData["TypeServerImap"])
            {
                ViewData["TypeServerPop"] = true;
            }

            OperationStatus operationstatus = OperationStatus.Successful;
            ModelState.SetModelValue("OperationStatus", new ValueProviderResult(Convert.ToString(operationstatus), Convert.ToString(operationstatus), CultureInfo.InvariantCulture));
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Configuration(FormCollection formCollection)
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            string resultTestMail = formCollection["U"];
            //string approvalCheck = Request["Approval"];
            //string sendEmailToNewsCheck = Request["SendEmailToNews"];
            string inboxEmail = Request["InboxEmail"];
            string password = Request["Kennwort"];
            string rePassword = Request["ReKennwort"];
            string popPort = Request["PopPort"];
            string popServer = Request["PopServer"];
            string popRequireSsl = Request["PopRequireSsl"];
            string typeServer = Request["TypeServer"];
            bool passwordMarch = true;
            bool blankAll = false;
            Configuration configuration = new Configuration();
            IList<Configuration> configurations = ConfigurationService.GetAllByClientCompany(CurrentCompany);
            if (string.IsNullOrEmpty(resultTestMail) && string.IsNullOrEmpty(inboxEmail) && string.IsNullOrEmpty(password)
                && string.IsNullOrEmpty(rePassword) && string.IsNullOrEmpty(popPort) && string.IsNullOrEmpty(popServer))
            {
                blankAll = true;
                popRequireSsl = string.Empty;
                typeServer = "Pop";
            }
                if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(rePassword))
                {
                    if (password.Equals(rePassword))
                    {
                        passwordMarch = true;
                    }
                    else
                    {
                        passwordMarch = false;
                    }
                }

                //if (sendEmailToNewsCheck.IndexOf(',') != -1)
                //{
                //    string[] sendEmailToNewsChecks = sendEmailToNewsCheck.Split(',');
                //    sendEmailToNewsCheck = sendEmailToNewsChecks[0];
                //}
                //bool sendMailToNewsValue;
                //if (sendEmailToNewsCheck.Equals("false"))
                //{
                //    sendMailToNewsValue = true;
                //}
                //else
                //{
                //    sendMailToNewsValue = false;
                //}


                if (popRequireSsl.IndexOf(',') != -1)
                {
                    string[] popRequireSslChecks = popRequireSsl.Split(',');
                    popRequireSsl = popRequireSslChecks[0];
                }
                bool popRequiresSslValue;
                if (popRequireSsl.Equals("true"))
                {
                    popRequiresSslValue = true;
                }
                else
                {
                    popRequiresSslValue = false;
                }
                //ViewData["ApprovalCheck"] = approvalValue;

                if (resultTestMail.Equals("true") || blankAll)
                {
                    if (configurations.Count == 0)
                    {
                        //configuration.SendEmailToNews = sendMailToNewsValue.ToString();
                        configuration.InboxEmail = inboxEmail;                        
                        configuration.InboxPassword = password;                            
                        configuration.PopPort = popPort;
                        configuration.PopServer = popServer;
                        configuration.PopRequireSsl = popRequiresSslValue.ToString();
                        configuration.TypeServer = typeServer;
                        //configuration.Approval = approvalValue.ToString();
                        configuration.ClientCompany = CurrentCompany;
                        configuration.CreatedBy = CurrentUser;
                        configuration.CreatedDate = DateTime.Now;
                        configuration.LastChangedBy = CurrentUser;
                        configuration.LastChangedDate = DateTime.Now;
                        ConfigurationService.Save(configuration);
                    }
                    else
                    {
                        //configuration = configurations[0];
                        configuration = ConfigurationService.GetById((decimal)configurations[0].Id);
                        configuration.InboxEmail = inboxEmail;                        
                        configuration.InboxPassword = password;                        
                        configuration.PopPort = popPort;
                        configuration.PopServer = popServer;
                        configuration.PopRequireSsl = popRequiresSslValue.ToString();
                        configuration.TypeServer = typeServer;
                        //configuration.Approval = approvalValue.ToString();
                        //configuration.SendEmailToNews = sendMailToNewsValue.ToString();
                        configuration.LastChangedBy = CurrentUser;
                        configuration.LastChangedDate = DateTime.Now;
                        ConfigurationService.Update(configuration);
                    }
                    return Configuration();
                }
                IList<Configuration> configurationsSave = ConfigurationService.GetAllByClientCompany(CurrentCompany);
                if (configurations.Count != 0)
                {
                    if (blankAll)
                    {
                        configurationsSave[0].PopServer = "Pop";
                    }
                    configuration = ConfigurationService.GetById((decimal)configurations[0].Id);
                    configuration.InboxEmail = configurationsSave[0].InboxEmail;
                    configuration.InboxPassword = configurationsSave[0].InboxPassword;
                    configuration.PopPort = configurationsSave[0].PopPort;
                    configuration.PopServer = configurationsSave[0].PopServer;
                    configuration.PopRequireSsl = configurationsSave[0].PopRequireSsl;
                    configuration.TypeServer = configurationsSave[0].TypeServer;
                    //configuration.Approval = approvalValue.ToString();
                    //configuration.SendEmailToNews = sendMailToNewsValue.ToString();
                    configuration.LastChangedBy = CurrentUser;
                    configuration.LastChangedDate = DateTime.Now;
                    ConfigurationService.Update(configuration);
                }
                else
                {
                    //configuration.SendEmailToNews = sendMailToNewsValue.ToString();
                    configuration.InboxEmail = string.Empty;
                    if (!string.IsNullOrEmpty(password))
                    {
                        if (passwordMarch)
                        {
                            configuration.InboxPassword = string.Empty;
                        }
                    }
                    configuration.PopPort = string.Empty;
                    configuration.PopServer = string.Empty;
                    configuration.PopRequireSsl = string.Empty;
                    configuration.TypeServer = string.Empty;
                    //configuration.Approval = approvalValue.ToString();
                    configuration.ClientCompany = CurrentCompany;
                    configuration.CreatedBy = CurrentUser;
                    configuration.CreatedDate = DateTime.Now;
                    configuration.LastChangedBy = CurrentUser;
                    configuration.LastChangedDate = DateTime.Now;
                    ConfigurationService.Save(configuration);
                }
            //return View("Index");
            return Configuration();
            
            //return RedirectToAction("Index", "Configuration");
           // return Content(string.Empty);
           // return null;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult TestConfig()
        {
            string result = string.Empty;
            string Email = Request.Params["Email"];
            string Password = Request.Params["KeyPassword"];
            string Port = Request.Params["Port"];
            string Server = Request.Params["Server"];
            string ServerType = Request.Params["ServerType"];            
            string RequireSSL = Request.Params["RequireSSL"];

            Configuration configuration = new Configuration();
            if (ServerType.Equals("Pop"))
            {
                configuration.InboxEmail = Email;
                configuration.InboxPassword = Password;
                configuration.PopPort = Port;
                configuration.PopServer = Server;
                configuration.PopRequireSsl = RequireSSL;
                result = ValidateEmailPop(configuration);
            }
            else
            {
                configuration.InboxEmail = Email;
                configuration.InboxPassword = Password;
                configuration.PopPort = Port;
                configuration.PopServer = Server;
                configuration.PopRequireSsl = RequireSSL;
                result = ValidateEmailImap(configuration);
            }


            return new JsonResult() { Data = result };
        }
        public class Pop3MessageReaderInbox
        {

            IList<Pop3Message> _messages;
            Pop3Client pop3Client;
            DateTime? newestDate = null;

            public DateTime? NewestDate
            {
                get { return newestDate; }
                set { newestDate = value; }
            }

            public Pop3MessageReaderInbox(Pop3Client pop3Client)
            {

                Pop3Client = pop3Client;
                Messages = new List<Pop3Message>();
            }
            public Pop3Client Pop3Client
            {
                get { return pop3Client; }
                set { pop3Client = value; }
            }

            public IList<Pop3Message> Messages
            {
                get { return _messages; }
                set { _messages = value; }
            }

        }
        public class Imap4MessageReaderInbox
        {
            Imap4Client imap4Client = new Imap4Client();

            public Imap4Client Imap4Client
            {
                get
                {
                    if (imap4Client == null)
                    {
                        imap4Client = new Imap4Client();
                    }
                    return imap4Client;
                }
                set { imap4Client = value; }
            }

            public Imap4MessageReaderInbox(string mailServer, int port, bool ssl, string login, string password)
            {
                imap4Client.ReceiveTimeout = 8 * 60000;
                if (ssl)
                {
                    imap4Client.ConnectSsl(mailServer, port);
                }
                else
                {
                    imap4Client.Connect(mailServer, port);
                }
                if (imap4Client.IsConnected)
                {
                    imap4Client.Login(login, password);
                }
            }


            public IEnumerable<Message> GetAllMails(string mailInbox)
            {
                return GetMails(mailInbox, "ALL").Cast<Message>();
            }

            public IEnumerable<Message> GetUnreadMails(string mailInbox)
            {
                return GetMails(mailInbox, "UNSEEN").Cast<Message>();
            }

            private MessageCollection GetMails(string mailBox, string searchParse)
            {
                Mailbox mails = Imap4Client.SelectMailbox(mailBox);
                MessageCollection messages = mails.SearchParse(searchParse);

                return messages;
            }

            public void Delete(string inbox)
            {
                Imap4Client.DeleteMailbox(inbox);
            }

            public void DeleteInbox(string mailBox)
            {
                Mailbox mails = Imap4Client.SelectMailbox(mailBox);
                mails.Delete();//Delete the mailbox
                //mails.DeleteMessage
            }

            public void ImapClose()
            {
                Imap4Client.Close();
            }

            public void ImapDisconnect()
            {
                Imap4Client.Disconnect();
            }
        }

        private string ValidateEmailPop(Configuration configuration)
        {
            string result = "1";
            Pop3Client pop3Client = new Pop3Client(configuration.InboxEmail, configuration.InboxPassword, configuration.PopServer);
            pop3Client.Ssl = Convert.ToBoolean(configuration.PopRequireSsl);
            try
            {
            pop3Client.Port = Convert.ToInt32(configuration.PopPort);
            Pop3MessageReaderInbox popMessageReader = new Pop3MessageReaderInbox(pop3Client);
            
                pop3Client.ReceiveTimeout = 8 * 60000;
                pop3Client.Open();

                if (pop3Client.State == Pop3ConnectionState.Connected)
                {
                    pop3Client.AuthenticateMode = Pop3AuthenticateMode.Pop;
                    pop3Client.Authenticate();
                    if (pop3Client.State == Pop3ConnectionState.Authenticated)
                    {
                    }
                    else
                    {
                        result = "2";
                    }
                }
                else
                {
                    result = "2";
                }
            }
            catch (Exception exception)
            {
                result = exception.Message;
            }
            return result;
        }

        private string ValidateEmailImap(Configuration configuration)
        {
            string result = "1";
            try
            {
                Imap4MessageReaderInbox imap4Messagereader = new Imap4MessageReaderInbox(configuration.PopServer, Convert.ToInt32(configuration.PopPort), Convert.ToBoolean(configuration.PopRequireSsl), configuration.InboxEmail, configuration.InboxPassword);

            }
            catch (Exception exception)
            {
                result = "2";
            }
            return result;
        }
        String checkResult(String check)
        {
            if (check.IndexOf(',') != -1)
            {
                string[] checks = check.Split(',');
                check = checks[0];
            }
            return check;
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Defaults()
        {
            SetEntityHelpDataToBackEnd();
            IList<ResourceObject> securityGroupBaseList = ResourceService.GetAll<SecurityGroupBase>();
            IList<ConfigurationUserType> cutList = ConfigurationUserTypeService.GetByClientCompany(CurrentCompany);
            if (cutList == null || cutList.Count < 5)
            {
                //If no exist some configuartion to user type then this should be created
                IList<ConfigurationUserType> newCutList = new List<ConfigurationUserType>();//copy the current list
                foreach (ResourceObject ro in securityGroupBaseList)
                {
                    if (!ro.Id.Equals(SecurityGroupBase.Root))//no should create to root user becuase root user have permisons like administrator
                    {
                        bool noExist = true;
                        for (int i = 0; i < newCutList.Count; i++)
                        {
                            if (ro.Id.Equals(newCutList[i].SecurityGroupId))
                            {
                                noExist = false;
                                i = newCutList.Count;
                            }
                        }
                        if (noExist)
                        {
                            ConfigurationUserType newcut = ConfigurationUserTypeService.CreateConfigurationUserType(ro.Id, CurrentUser, CurrentCompany);
                            cutList.Add(newcut);//the new ConfigurationUserType is add to list
                        }
                    }
                }
            }
            if (cutList != null && cutList.Count > 0)
            {

                bool comparinatorExportAll = true;
                bool enableToolsAll = true;
                bool industryInfoAll = true;
                bool infoTabAll = true;
                bool silverBulletsTabAll = true;
                bool positioningTabAll = true;
                bool pricingTabAll = true;
                bool featuresTabAll = true;
                bool salesToolsTabAll = true;
                bool newsTabAll = true;
                bool socialLogAll = true;
                bool industryStandarsAll = true;
                bool benefitAll = true;
                bool costAll = true;

                bool comparinatorAll = true;//FLAG TO COMPARINATOR
                string comparinatorAllStr = "";//VARIABLE TO COMPARE IF ALL VALUES ARE EQUALS

                bool contentAll = true;
                string contentAllStr = "";
                for (int i = 0; i < cutList.Count; i++)
                {
                    ViewData["ComparinatorExport_" + cutList[i].SecurityGroupId] = cutList[i].ComparinatorExport;
                    if (comparinatorExportAll) comparinatorExportAll = Convert.ToBoolean(cutList[i].ComparinatorExport);

                    if (cutList[i].SecurityGroupId.Equals(SecurityGroupBase.Administrator) || cutList[i].SecurityGroupId.Equals(SecurityGroupBase.Analyst) || cutList[i].SecurityGroupId.Equals(SecurityGroupBase.Manager))
                    {
                        ViewData["EnableTools_" + cutList[i].SecurityGroupId] = cutList[i].EnableTools;
                        if (enableToolsAll) enableToolsAll = Convert.ToBoolean(cutList[i].EnableTools);
                    }
                    ViewData["IndustryInfoTab_" + cutList[i].SecurityGroupId] = cutList[i].IndustryInfo;
                    if (industryInfoAll) industryInfoAll = Convert.ToBoolean(cutList[i].IndustryInfo);
                    ViewData["InfoTab_" + cutList[i].SecurityGroupId] = cutList[i].Info;
                    if (infoTabAll) infoTabAll = Convert.ToBoolean(cutList[i].Info);
                    ViewData["SilverBulletsTab_" + cutList[i].SecurityGroupId] = cutList[i].SilverBullets;
                    if (silverBulletsTabAll) silverBulletsTabAll = Convert.ToBoolean(cutList[i].SilverBullets);
                    ViewData["PositioningTab_" + cutList[i].SecurityGroupId] = cutList[i].Positioning;
                    if (positioningTabAll) positioningTabAll = Convert.ToBoolean(cutList[i].Positioning);
                    ViewData["PricingTab_" + cutList[i].SecurityGroupId] = cutList[i].Pricing;
                    if (pricingTabAll) pricingTabAll = Convert.ToBoolean(cutList[i].Pricing);
                    ViewData["FeaturesTab_" + cutList[i].SecurityGroupId] = cutList[i].Features;
                    if (featuresTabAll) featuresTabAll = Convert.ToBoolean(cutList[i].Features);
                    ViewData["SalesToolsTab_" + cutList[i].SecurityGroupId] = cutList[i].SalesTools;
                    if (salesToolsTabAll) salesToolsTabAll = Convert.ToBoolean(cutList[i].SalesTools);
                    ViewData["NewsTab_" + cutList[i].SecurityGroupId] = cutList[i].News;
                    if (newsTabAll) newsTabAll = Convert.ToBoolean(cutList[i].News);

                    ViewData["SocialLog_" + cutList[i].SecurityGroupId] = cutList[i].SocialLog;
                    if (socialLogAll) socialLogAll = Convert.ToBoolean(cutList[i].SocialLog);

                    ViewData["IndustryStandars_" + cutList[i].SecurityGroupId] = cutList[i].IndustryStandars;
                    if (industryStandarsAll) industryStandarsAll = Convert.ToBoolean(cutList[i].IndustryStandars);
                    ViewData["Benefit_" + cutList[i].SecurityGroupId] = cutList[i].Benefit;
                    if (benefitAll) benefitAll = Convert.ToBoolean(cutList[i].Benefit);
                    ViewData["Cost_" + cutList[i].SecurityGroupId] = cutList[i].Cost;
                    if (costAll) costAll = Convert.ToBoolean(cutList[i].Cost);
                    if (i == 0) {//ASIGNED THE FIRST VALUE TO VARIABLE TEMPORAL
                        contentAllStr = cutList[i].Content;
                        comparinatorAllStr = cutList[i].Comparinator;
                    }
                    ViewData["Comparinator_" + cutList[i].SecurityGroupId] = cutList[i].Comparinator;
                    if (comparinatorAll)
                    {
                        if (!comparinatorAllStr.Equals(cutList[i].Comparinator))
                        {
                            comparinatorAll = false;
                        }
                    }
                    ViewData["Content_" + cutList[i].SecurityGroupId] = cutList[i].Content;
                    if (contentAll)
                    {
                        if (!contentAllStr.Equals(cutList[i].Content))
                        {
                            contentAll = false;
                        }
                    }
                }
                //To all chekcboxs
                ViewData["ComparinatorExport_ALL"] = comparinatorExportAll.ToString();

                ViewData["EnableTools_ALL"] = enableToolsAll.ToString();

                ViewData["IndustryInfoTab_ALL"] = industryInfoAll.ToString();
                ViewData["InfoTab_ALL"] = infoTabAll.ToString();
                ViewData["SilverBulletsTab_ALL"] = silverBulletsTabAll.ToString();
                ViewData["PositioningTab_ALL"] = positioningTabAll.ToString();
                ViewData["PricingTab_ALL"] = pricingTabAll.ToString();
                ViewData["FeaturesTab_ALL"] = featuresTabAll.ToString();
                ViewData["SalesToolsTab_ALL"] = salesToolsTabAll.ToString();
                ViewData["NewsTab_ALL"] = newsTabAll.ToString();

                ViewData["SocialLog_ALL"] = socialLogAll.ToString();

                ViewData["IndustryStandars_ALL"] = industryStandarsAll.ToString();
                ViewData["Benefit_ALL"] = benefitAll.ToString();
                ViewData["Cost_ALL"] = costAll.ToString();
                
                if (!contentAll) contentAllStr = "";
                ViewData["Content_ALL"] = contentAllStr;
                if (!comparinatorAll) comparinatorAllStr = "";
                ViewData["Comparinator_ALL"] = comparinatorAllStr;

            }
            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(CurrentCompany);
            IList<ResourceObject> sameValuesList = ResourceService.GetAll<SameValues>();
            sameValuesList = SortingOptionList(sameValuesList);

            bool DisabledPublicComment = false;
            SelectList SameValues = new SelectList(sameValuesList, "Value", "Id", "all");

            if (configurations != null)
            {
                if (configurations.Count > 0)
                {
                    SameValues = new SelectList(sameValuesList, "Value", "Id", configurations[0].SameValues);
                    if (configurations[0].DisabledPublicComment.Equals("true"))
                    {
                        DisabledPublicComment = true;
                    }
                }
            }
            ViewData["SameValuesList"] = SameValues;
            ViewData["DisabledPublicComment"] = DisabledPublicComment;
            SetLabels();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DefaultsSave(FormCollection form)
        {
			IList<ResourceObject> securityGroupBaseList = ResourceService.GetAll<SecurityGroupBase>();
            IList<ResourceObject> configurationDefaultFieldsList = ResourceService.GetAll<ConfigurationDefaultFields>();

            IList<ConfigurationUserType> cutList = ConfigurationUserTypeService.GetByClientCompany(CurrentCompany);

            if (cutList != null && cutList.Count > 0)
            {
                bool comparinatorExportAll = true;
                bool enableToolsAll = true;
                bool industryInfoTabAll = true;
                bool infoTabAll = true;
                bool silverBulletsTabAll = true;
                bool positioningTabAll = true;
                bool pricingTabAll = true;
                bool featuresTabAll = true;
                bool salesToolsTabAll = true;
                bool newsTabAll = true;
                bool socialLogAll = true;
                bool industryStandarsAll = true;
                bool benefitAll = true;
                bool costAll = true;
                
                bool comparinatorAll = true;
                string comparinatorAllStr = "";

                bool contentAll = true;
                string contentAllStr = "";
                for (int i = 0; i < cutList.Count; i++)
                {
                    cutList[i].IndustryInfo = checkResult(form["IndustryInfoTab_" + cutList[i].SecurityGroupId]);//Set the value of Form to Entity
                    ViewData["IndustryInfoTab_" + cutList[i].SecurityGroupId] = cutList[i].IndustryInfo;//Set the Value of Entity in ViewData to Send on View
                    if (industryInfoTabAll) industryInfoTabAll = Convert.ToBoolean(cutList[i].IndustryInfo);//If
                    cutList[i].Info = checkResult(form["InfoTab_" + cutList[i].SecurityGroupId]);//Set the value of Form to Entity
                    ViewData["InfoTab_" + cutList[i].SecurityGroupId] = cutList[i].Info;//Set the Value of Entity in ViewData to Send on View
                    if (infoTabAll) infoTabAll = Convert.ToBoolean(cutList[i].Info);//If
                    cutList[i].SilverBullets = checkResult(form["SilverBulletsTab_" + cutList[i].SecurityGroupId]);
                    ViewData["SilverBulletsTab_" + cutList[i].SecurityGroupId] = cutList[i].SilverBullets;
                    if (silverBulletsTabAll) silverBulletsTabAll = Convert.ToBoolean(cutList[i].SilverBullets);
                    cutList[i].Positioning = checkResult(form["PositioningTab_" + cutList[i].SecurityGroupId]);
                    ViewData["PositioningTab_" + cutList[i].SecurityGroupId] = cutList[i].Positioning;
                    if (positioningTabAll) positioningTabAll = Convert.ToBoolean(cutList[i].Positioning);
                    cutList[i].Pricing = checkResult(form["PricingTab_" + cutList[i].SecurityGroupId]);
                    ViewData["PricingTab_" + cutList[i].SecurityGroupId] = cutList[i].Pricing;
                    if (pricingTabAll) pricingTabAll = Convert.ToBoolean(cutList[i].Pricing);
                    cutList[i].Features = checkResult(form["FeaturesTab_" + cutList[i].SecurityGroupId]);
                    ViewData["FeaturesTab_" + cutList[i].SecurityGroupId] = cutList[i].Features;
                    if (featuresTabAll) featuresTabAll = Convert.ToBoolean(cutList[i].Features);
                    cutList[i].SalesTools = checkResult(form["SalesToolsTab_" + cutList[i].SecurityGroupId]);
                    ViewData["SalesToolsTab_" + cutList[i].SecurityGroupId] = cutList[i].SalesTools;
                    if (salesToolsTabAll) salesToolsTabAll = Convert.ToBoolean(cutList[i].SalesTools);
                    cutList[i].News = checkResult(form["NewsTab_" + cutList[i].SecurityGroupId]);
                    ViewData["NewsTab_" + cutList[i].SecurityGroupId] = cutList[i].News;
                    if (newsTabAll) newsTabAll = Convert.ToBoolean(cutList[i].News);

                    if (!Convert.ToBoolean(cutList[i].Info) && !Convert.ToBoolean(cutList[i].SilverBullets) && !Convert.ToBoolean(cutList[i].Positioning) && !Convert.ToBoolean(cutList[i].Pricing) && !Convert.ToBoolean(cutList[i].Features))
                    {
                        cutList[i].ComparinatorExport = false.ToString();
                    }
                    else
                    {
                        cutList[i].ComparinatorExport = checkResult(form["ComparinatorExport_" + cutList[i].SecurityGroupId]);
                    }
                    ViewData["ComparinatorExport_" + cutList[i].SecurityGroupId] = cutList[i].ComparinatorExport;
                    if (comparinatorExportAll) comparinatorExportAll = Convert.ToBoolean(cutList[i].ComparinatorExport);

                    cutList[i].SocialLog = checkResult(form["SocialLog_" + cutList[i].SecurityGroupId]);
                    ViewData["SocialLog_" + cutList[i].SecurityGroupId] = cutList[i].SocialLog;
                    if (socialLogAll) socialLogAll = Convert.ToBoolean(cutList[i].SocialLog);

                    cutList[i].IndustryStandars = checkResult(form["IndustryStandars_" + cutList[i].SecurityGroupId]);
                    ViewData["IndustryStandars_" + cutList[i].SecurityGroupId] = cutList[i].IndustryStandars;
                    if (industryStandarsAll) industryStandarsAll = Convert.ToBoolean(cutList[i].IndustryStandars);
                    cutList[i].Benefit = checkResult(form["Benefit_" + cutList[i].SecurityGroupId]);
                    ViewData["Benefit_" + cutList[i].SecurityGroupId] = cutList[i].Benefit;
                    if (benefitAll) benefitAll = Convert.ToBoolean(cutList[i].Benefit);
                    cutList[i].Cost = checkResult(form["Cost_" + cutList[i].SecurityGroupId]);
                    ViewData["Cost_" + cutList[i].SecurityGroupId] = cutList[i].Cost;
                    if (costAll) costAll = Convert.ToBoolean(cutList[i].Cost);

                    if (cutList[i].SecurityGroupId.Equals(SecurityGroupBase.Administrator) || cutList[i].SecurityGroupId.Equals(SecurityGroupBase.Analyst) || cutList[i].SecurityGroupId.Equals(SecurityGroupBase.Manager))
                    {
                        cutList[i].EnableTools = checkResult(form["EnableTools_" + cutList[i].SecurityGroupId]);
                        ViewData["EnableTools_" + cutList[i].SecurityGroupId] = cutList[i].EnableTools;
                        if (enableToolsAll) enableToolsAll = Convert.ToBoolean(cutList[i].EnableTools);
                    }
                   
                    
                    if (i == 0)
                    {//ASIGNED THE FIRST VALUE TO VARIABLE TEMPORAL
                        contentAllStr = cutList[i].Content;
                        comparinatorAllStr = cutList[i].Comparinator;
                    }
                    cutList[i].Comparinator = checkResult(form["Comparinator_" + cutList[i].SecurityGroupId]);
                    ViewData["Comparinator_" + cutList[i].SecurityGroupId] = cutList[i].Comparinator;

                    if (comparinatorAll)
                    {
                        if (!comparinatorAllStr.Equals(cutList[i].Comparinator))
                        {
                            comparinatorAll = false;
                        }
                    }
                    cutList[i].Content = checkResult(form["Content_" + cutList[i].SecurityGroupId]);
                    ViewData["Content_" + cutList[i].SecurityGroupId] = cutList[i].Content;
                    if (contentAll)
                    {
                        if (!contentAllStr.Equals(cutList[i].Content))
                        {
                            contentAll = false;
                        }
                    }
                    
                    SetDefaultDataForUpdate(cutList[i]);
                    ConfigurationUserTypeService.Update(cutList[i]);
                }
                //To all chekcboxs
                ViewData["ComparinatorExport_ALL"] = comparinatorExportAll.ToString();

                ViewData["EnableTools_ALL"] = enableToolsAll.ToString();

                ViewData["IndustryInfoTab_ALL"] = industryInfoTabAll.ToString();
                ViewData["InfoTab_ALL"] = infoTabAll.ToString();
                ViewData["SilverBulletsTab_ALL"] = silverBulletsTabAll.ToString();
                ViewData["PositioningTab_ALL"] = positioningTabAll.ToString();
                ViewData["PricingTab_ALL"] = pricingTabAll.ToString();
                ViewData["FeaturesTab_ALL"] = featuresTabAll.ToString();
                ViewData["SalesToolsTab_ALL"] = salesToolsTabAll.ToString();
                ViewData["NewsTab_ALL"] = newsTabAll.ToString();

                ViewData["SocialLog_ALL"] = socialLogAll.ToString();

                ViewData["IndustryStandars_ALL"] = industryStandarsAll.ToString();
                ViewData["Benefit_ALL"] = benefitAll.ToString();
                ViewData["Cost_ALL"] = costAll.ToString();
                if (!contentAll) contentAllStr = "";
                ViewData["Content_ALL"] = contentAllStr;
                if (!comparinatorAll) comparinatorAllStr = "";
                ViewData["Comparinator_ALL"] = comparinatorAllStr;
            }
            
            SetEntityHelpDataToBackEnd();
            string SameValues = checkResult(form["SameValues"]);
            string DisabledPublicComment = checkResult(form["DisabledPublicComment"]);
            ConfigurationDefaults configuration = new ConfigurationDefaults();
            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(CurrentCompany);
            IList<ResourceObject> sameValuesList = ResourceService.GetAll<SameValues>();
            sameValuesList = SortingOptionList(sameValuesList);
            SelectList SameValues2 = new SelectList(sameValuesList, "Value", "Id", "all");
            if (configurations.Count == 0)
            {
                configuration.SameValues = SameValues;
                configuration.DisabledPublicComment = DisabledPublicComment;
                configuration.ClientCompany = CurrentCompany;
                configuration.CreatedBy = CurrentUser;
                configuration.CreatedDate = DateTime.Now;
                configuration.LastChangedBy = CurrentUser;
                configuration.LastChangedDate = DateTime.Now;
                ConfigurationDefaultsService.Save(configuration);
            }
            else
            {
                configuration = ConfigurationDefaultsService.GetById((decimal)configurations[0].Id);
                configuration.SameValues = SameValues;
                configuration.DisabledPublicComment = DisabledPublicComment;
                configuration.LastChangedBy = CurrentUser;
                configuration.LastChangedDate = DateTime.Now;
                ConfigurationDefaultsService.Update(configuration);
                SameValues2 = new SelectList(sameValuesList, "Value", "Id", configurations[0].SameValues);
            }

            OperationStatus operationstatus = OperationStatus.Successful;
            ModelState.SetModelValue("OperationStatus", new ValueProviderResult(Convert.ToString(operationstatus), Convert.ToString(operationstatus), CultureInfo.InvariantCulture));
            ViewData["SameValuesList"] = SameValues2;
            ViewData["DisabledPublicComment"] = DisabledPublicComment;
            SetLabels();
            return RedirectToAction("Defaults", new { Scope = Request["Scope"], Container = "#" + Request["Container"] + "Defaults" });
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ApprovalProject()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            bool approval = Convert.ToBoolean(Request["Approval"]);
            bool sendEmailToNews = Convert.ToBoolean(Request["SendEmailToNews"]);
            IList<Configuration> configurations = ConfigurationService.GetAllByClientCompany(CurrentCompany);
            ViewData["ApprovalCheck"] = false;
            ViewData["SendEmailToNewsCheck"] = false;
            if (configurations.Count > 0)
            {
                if (configurations[0].Approval.Equals(ProjectsApproval.True))
                {
                    ViewData["ApprovalCheck"] = true;
                }
                if (!configurations[0].CanSendEmailToNews)
                {
                    ViewData["SendEmailToNewsCheck"] = true;
                }
            }
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ApprovalProject(string ProjectIds)
        {
            string[] ids = ProjectIds.Split(':');
            if (ids.Length > 0)
            {
                foreach (string id in ids)
                {
                    decimal projectid = decimal.Parse(id);
                    Project project = ProjectService.GetById(projectid);
                    if (project != null)
                    {
                        IList<ApprovalList> approvalList = ApprovalListService.GetByUnApprovedEntityId(project.Id.ToString(), DomainObjectType.Project, CurrentCompany);
                        if (approvalList != null)
                        {
                            foreach (ApprovalList approval in approvalList)
                            {
                                if (approval.Approved.Equals(ApprovalListApproveStatus.Pending))
                                {
                                    approval.Approved = ApprovalListApproveStatus.Approved;
                                    approval.ApproverNotes = "Approved via override";
                                    approval.DateApproved = DateTime.Now;
                                    ApprovalListService.Update(approval);
                                }
                            }
                        }
                        project.Status = ProjectStatus.Published;
                        project.LastChangedDate = DateTime.Now;
                        project.LastChangedBy = CurrentUser;
                        ProjectService.Update(project);
                        
                    }
                }
            }
            return RedirectToAction("IndexApproval", new {Scope= Request["Scope"], Container=Request["Container"]});
        }

        public ActionResult IndexApproval()
        {
            SetEntityHelpDataToBackEnd();
            return View();
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IndexConfiguration()
        {
            SetEntityHelpDataToBackEnd();

            bool approval = Convert.ToBoolean(Request["Approval"]);
            bool sendEmailToNews = Convert.ToBoolean(Request["SendEmailToNews"]);
            IList<Configuration> configurations = ConfigurationService.GetAllByClientCompany(CurrentCompany);
            ViewData["ApprovalCheck"] = false;
            ViewData["SendEmailToNewsCheck"] = false;
            if (configurations.Count > 0)
            {
                if (configurations[0].Approval.Equals(ProjectsApproval.True))
                {
                    ViewData["ApprovalCheck"] = true;
                }
                if (!configurations[0].CanSendEmailToNews)
                {
                    ViewData["SendEmailToNewsCheck"] = true;
                }
            }

            return View();
        }

        public ActionResult Test()
        {
            return RedirectToAction("Configuration", new { Scope = Request["Scope"], Container = Request["Container"] });
        }

        public ActionResult IndexDefault()
        {
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            return RedirectToAction("Defaults");
        }

        public ActionResult UserToReceiveTop()
        {
            SetEntityHelpDataToBackEnd();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Labels()
        {
            string scope = Request["Scope"];
            string container = Request["Container"];
            
            return RedirectToAction("IndexLabels", "ConfigurationLabels", new {Scope= scope, Container= container });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateUsers()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            if (ids != null && ids.Length > 0)
            {
                foreach (string id in ids)
                {
                    UserProfile user = UserProfileService.GetById(id);
                    if (user != null)
                    {
                        user.ReceiveMail = "Y";
                        UserProfileService.Update(user);
                    }
                }
            }
           JavaScriptResult r = new JavaScriptResult();
           r.Script = "reloadGrid('#UserProfileAllListTable');";

           return r;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveToReceive()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();

            if (ids != null && ids.Length > 0)
            {
                foreach (string id in ids)
                {
                    UserProfile user = UserProfileService.GetById(id);
                    if (user != null)
                    {
                        user.ReceiveMail = "N";
                        UserProfileService.Update(user);
                    }
                }
            }
            JavaScriptResult r = new JavaScriptResult();
            r.Script = "reloadGrid('#UserProfileAllListTable');";
            return r;
        }

        private IList<ResourceObject> SortingOptionList(IList<ResourceObject> source)
        {
            IList<ResourceObject> sameValuesListSort = new List<ResourceObject>();
            if (source != null && source.Count > 2)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    if (i == 1)
                    {
                        sameValuesListSort.Add(source[2]);
                    }
                    else if (i == 2)
                    {
                        sameValuesListSort.Add(source[1]);
                    }
                    else
                    {
                        sameValuesListSort.Add(source[i]);
                    }
                }
                source = sameValuesListSort;
            }
            return source;
        }

        public void SetLabels()
        {
            ConfigurationLabels IndustryLabel = new ConfigurationLabels();
            IndustryLabel = ConfigurationLabelsService.GetByName("Industry", CurrentCompany);
            String dIndustryLabel = "Industry";
            if (IndustryLabel != null)
            {
                if (IndustryLabel.Status.Equals(ConfigurationLabesStatus.Enable))
                    dIndustryLabel = IndustryLabel.Value;
            }
            else
            {
                IndustryLabel = new ConfigurationLabels();
                IndustryLabel.Name = "Industry";
                IndustryLabel.Value = dIndustryLabel;
                IndustryLabel.Description = "Label for " + dIndustryLabel;
                IndustryLabel.Status = ConfigurationLabesStatus.Enable;
                IndustryLabel.CreatedBy = CurrentUser;
                IndustryLabel.CreatedDate = DateTime.Now;
                IndustryLabel.LastChangedBy = CurrentUser;
                IndustryLabel.LastChangedDate = DateTime.Now;
                IndustryLabel.ClientCompany = CurrentCompany;

                ConfigurationLabelsService.Save(IndustryLabel);
            }
            ViewData["IndustryLabel"] = string.Format(LabelResource.ComparinatorResultIndustryInfoTab, dIndustryLabel);
        }
    }
}
