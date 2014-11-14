using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Xml;
using Compelligence.Emails.Entity;
using Compelligence.Emails.Util;
using Common.Logging;
using System.Net.NetworkInformation;
using Compelligence.Util.MirrorConection;
using System.Net.Sockets;

namespace Compelligence.Web.Controllers
{
    public class CustomErrorController : Controller
    {
        protected static ILog Logger;
        private static string EmailTemplatePath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["EmailTemplateXmlPath"];
        //
        // GET: /CustomError/

        public ActionResult Index(Exception exception, string currentController, string currentAction)
        {
            string excep = Convert.ToString(exception);
            ViewData["Url"] = HttpUtility.HtmlEncode(Request.Url.OriginalString); 
            XmlDocument xmlDocument = SettingMirror.ReadXml();
            XmlElement root = xmlDocument.DocumentElement;

            notoficationError(root);
            sendMailError(excep, root);

            //bool pingHost = PingHost(root.GetElementsByTagName("ServerIp")[0].InnerText,Convert.ToInt32(root.GetElementsByTagName("dbPort")[0].InnerText));
            //if (pingHost)
            //{
            //    //return View(currentAction, currentController);
            //}
            //else
            //{
            //    try
            //    {

            //        ConectionMirror.concetionMirror(xmlDocument);
            //    }
            //    catch(Exception ex)
            //    {
            //        Logger = LogManager.GetLogger(this.GetType());
            //        Logger.Error("Error Excute Command Alter parther " + ex);
            //    }
            //}
            return View();
        }
        public bool PingHost(string nameOrAddress, int port)
        {
            bool pingable = false;
            try
            {
                TcpClient c = new TcpClient(nameOrAddress, port);
                pingable = c.Connected;
            }
            catch (Exception e)
            {
                // Discard PingExceptions and return false;
            }

            return pingable;
        }

        public ActionResult Maintenance()
        {
            return View();
        }

        public void sendMailError(string message, XmlElement root)
        { 
            string emailSubject = Request.Url.Host + " DataBase Exec Error";
            string emailUser = root.GetElementsByTagName("user")[0].InnerText;
            string password = root.GetElementsByTagName("password")[0].InnerText;
            string smtpServer = root.GetElementsByTagName("serverips")[0].InnerText;
            string smtpPort = root.GetElementsByTagName("port")[0].InnerText;
            string smtpRequireSsl = root.GetElementsByTagName("requiressl")[0].InnerText;
            string cc = root.GetElementsByTagName("cc")[0].InnerText;
            string bcc = root.GetElementsByTagName("bcc")[0].InnerText;
            string EmailErrorTo = root.GetElementsByTagName("EmailErrorTo")[0].InnerText;
            
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(emailUser);
            mail.To.Add(new MailAddress(EmailErrorTo));
            mail.CC.Add(new MailAddress(cc));
            mail.Bcc.Add(new MailAddress(bcc));
            mail.IsBodyHtml = false;
            mail.Subject = (!string.IsNullOrEmpty(emailSubject)) ? emailSubject : "Error de Sistema Xstrata";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = message.ToString();
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            SmtpClient smtpMail = new SmtpClient(smtpServer);
            smtpMail.Port = Convert.ToInt32(smtpPort);
            smtpMail.EnableSsl = Convert.ToBoolean(smtpRequireSsl);
            smtpMail.Credentials = new NetworkCredential(emailUser, password);
            
            try
            {
                smtpMail.Send(mail);
            }
            catch (Exception e)
            {
                Logger = LogManager.GetLogger(this.GetType());
                Logger.Error("Error in send Email " + e);
            }

        }

        public void notoficationError(XmlElement root)
        {            
            string emailSubject = Request.Url.Host + " DataBase Exec Error";
            string emailUser = root.GetElementsByTagName("user")[0].InnerText;
            string password = root.GetElementsByTagName("password")[0].InnerText;
            string smtpServer = root.GetElementsByTagName("serverips")[0].InnerText;
            string smtpPort = root.GetElementsByTagName("port")[0].InnerText;
            string smtpRequireSsl = root.GetElementsByTagName("requiressl")[0].InnerText;
            string cc = root.GetElementsByTagName("mit")[0].InnerText;
            string bcc = root.GetElementsByTagName("ed")[0].InnerText;
            string EmailErrorTo = root.GetElementsByTagName("EmailErrorTo")[0].InnerText;




            EmailTemplate emailTemplate = EmailUtilities.GetEmailTemplate(EmailType.CustomErrorNotification);
            emailTemplate.AddParameter("keyWordCompany", Request.Url.Host);
            SmtpClient SmtpMailObject = new SmtpClient(smtpServer);
            //SmtpMailObject.Timeout = 9000;
            SmtpMailObject.EnableSsl = Convert.ToBoolean(smtpRequireSsl);
            SmtpMailObject.UseDefaultCredentials = false;
            SmtpMailObject.Port = Convert.ToInt32(smtpPort);
            SmtpMailObject.Credentials = new NetworkCredential(emailUser, password);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailUser);
            mailMessage.To.Add(new MailAddress(bcc));
            mailMessage.CC.Add(new MailAddress(cc));
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = emailTemplate.Subject;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.Body = emailTemplate.Message;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;

            try
            {
                SmtpMailObject.Send(mailMessage);
            }
            catch (Exception e)
            {
                Logger = LogManager.GetLogger(this.GetType());
                Logger.Error("Error in send Email " + e);
            }
         

        }
    }
}
