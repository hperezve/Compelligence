using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Web.Models.Web;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Common.Utility;
using Compelligence.Util.Type;
using Compelligence.Common.Utility.Upload;
using Compelligence.Common.Utility.Parser;
using System.Xml;
using System.Text.RegularExpressions;
using Compelligence.Web.Models.Util;
using Compelligence.Emails.Entity.Resource;

namespace Compelligence.Web.Controllers
{
    public class NewsletterController : BackEndAsyncFormController<Newsletter, decimal>
    {
        public List<listObjectItems> listSections;
        #region Public Properties

        public INewsletterService NewsletterService
        {
            get { return (INewsletterService)_genericService; }
            set { _genericService = value; }
        }
        
        public IFileService FileService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public ITemplateService TemplateService { get; set; }

        public IEmailService EmailService { get; set; }

        public ITeamService TeamService { get; set; }

        public IClientCompanyService ClientCompanyService { get; set; }

        #endregion

        #region Action Methods

        public ActionResult Editor()
        {
            decimal id = Convert.ToDecimal(Request["id"]);
            Newsletter entityObject = NewsletterService.GetById(id);

            OperationStatus operationStatusParam = OperationStatus.Initiated;

            //if (!string.IsNullOrEmpty(operationStatus))
            //{
            //    operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            //}

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);

            SetFormData();

            SetEntityDataToForm(entityObject);

            SetUserSecurityAccess(entityObject);

            SetEntityLocking(entityObject);

            ViewData["IdNews"] = entityObject.Id;
            return View(entityObject);
        }

        [ValidateInput(false)]
        public ActionResult SaveEdition(Newsletter entity, FormCollection collection)
        {
            decimal NewsletterId = (decimal)Session["NewsLetterId"];
            string RequestOpeningText = collection["OpeningText"];
            string RequestClosingText = collection["ClosingText"];

            Newsletter newsToUpdate = NewsletterService.GetById(NewsletterId);
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (newsToUpdate != null)
            {
                newsToUpdate.OpeningText = RequestOpeningText;
                newsToUpdate.ClosingText = RequestClosingText;
                NewsletterService.Update(newsToUpdate);
                operationStatus = OperationStatus.Successful;
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(newsToUpdate);

            SetUserSecurityAccess(newsToUpdate);

            SetEntityLocking(newsToUpdate);

            return View("Editor");
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Newsletter newsletter, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(newsletter.Title))
            {
                ValidationDictionary.AddError("Title", LabelResource.NewsletterTitleRequiredError);
            }

            if (Validator.IsBlankOrNull(newsletter.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.NewsletterAssignedToRequiredError);
            }
            if (Validator.IsBlankOrNull(newsletter.DestinationMail))
            {
                ValidationDictionary.AddError("DestinationMail", LabelResource.NewsLetterDestinationRequiredError);
            }
            if (Validator.IsBlankOrNull(newsletter.ContentText))
            {
                ValidationDictionary.AddError("ContentText", LabelResource.NewsletterContent);
            }
            else
            {
                if (!ValidatorEmails(newsletter.DestinationMail))
                {
                    ValidationDictionary.AddError("DestinationMail", LabelResource.NewsLetterDestinationFormatError);
                }
            }

            return ValidationDictionary.IsValid;

        }

        private bool ValidatorEmails(string emails)
        {
            string companyId = (string)Session["ClientCompany"];
            bool result = true;
            string[] items = emails.Split(',');
            foreach (string item in items)
            {
                string evaItem = item.Trim();
                string groupName = string.Empty;
                if (evaItem.Length > 0)
                {
                    if (evaItem.Substring(0, 1).Equals("(") && evaItem.EndsWith(")")) //Group
                    {
                        evaItem = evaItem.Substring(1, evaItem.Length - 2);
                        Team team = TeamService.GetByName(evaItem, companyId);
                        result &= (team != null);
                    }
                    else
                        result &= Validator.IsEmailOrWhite(evaItem);
                }
            }
            return result;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Template> templateList = TemplateService.GetAllSortByClientCompany("Name", clientCompany);
            templateList = (from o in templateList where o.TemplateType == TemplateType.Newsletter select o).ToList<Template>();

            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["TemplateIdList"] = new SelectList(templateList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(Newsletter news)
        {
            ViewData["Sections"] = NewsletterService.GetSections(news.Id); ;
            Session["NewsLetterId"] = news.Id;

        }

        protected override void SetFormEntityDataToForm(Newsletter news)
        {
            news.Sections = NewsletterService.GetSections(news.Id);
            ViewData["Sections"] = news.Sections;
        }

        /*add opening,closing,sections,items*/
        private string getSingleOption(string busqueda,string iniEtiq, string endEtiq){

            var soloOpen = (from word in busqueda.Split('\n', '\t')
                            where word.StartsWith(iniEtiq)
                            where word.EndsWith(endEtiq)
                            select word).FirstOrDefault();
            
            var incicioO = soloOpen.IndexOf(">");
            var finalO = soloOpen.LastIndexOf("<");

            var textoOpen = soloOpen.Substring(incicioO + 1, (finalO-incicioO)-1);

            return textoOpen;
        }

        private List<listObjectItems> getSingleOptions(string busqueda, string iniEtiq, string endEtiq,string index)
        {
            listSections = new List<listObjectItems>();

            var listado = (from word in busqueda.Split('\n', '\t')
                           where word.Contains(iniEtiq)
                           where word.EndsWith(endEtiq)
                           select word).ToList();

            foreach(string sections in listado){
                var incicioL = sections.IndexOf(">");
                var finalL = sections.LastIndexOf("<");

                var ID = sections.IndexOf(index);
                var ConvertText= sections.Replace('\"',' ');
                int increment =0;
                if(index=="<items id"){
                    increment= 10;
                }else{
                    increment = 13;
                }

                var extract_id = ConvertText.Substring(ID + increment, 3);
                
                var textoSections = sections.Substring(incicioL + 1, (finalL - incicioL)-1);
                //var idSections = sections.Substring(incicioL + 1, (finalL - incicioL) - 1);

                
                listSections.Add(new listObjectItems(Convert.ToInt16(extract_id),textoSections));
               
            }
            return listSections;

        }
        /*
        private List<int,string> getItemsOptions(string busqueda, string iniEtiq,string endEtiq)
        {

            //return listaItems;
        }*/
            

        private string savexmlValues(XMLElement item,string ibs){

            return "";

            }
        //
        //Save XML for news
        //
        protected override void GetFormData(Newsletter news, FormCollection form)
        {
            /*
            string busqueda = form["OpeningText"];
            
            busqueda = busqueda.Replace("\n", "");
            busqueda = busqueda.Replace("\\", "");
            busqueda = busqueda.Replace("\t", "");
            busqueda = busqueda.Replace("<p>", "\n");
            busqueda = busqueda.Replace("</p>", "\n");
            busqueda = busqueda.Replace("<items>\n", "<items>");
            busqueda = busqueda.Replace("\n</items>", "</items>");

            var openingText=getSingleOption(busqueda,"<opening>","</opening>");
            var closingText = getSingleOption(busqueda, "<closing>", "</closing>");
            List<listObjectItems> listaSeccionts = getSingleOptions(busqueda, "<sections ", "</sections>","<sections id");
            List<listObjectItems> listaItems = getSingleOptions(busqueda, "<items ", "</items>", "<items id");


            //XMLElement section = new XMLElement("Sections");
            XMLElement opening = new XMLElement("Opening");
            opening.AddValue(openingText);
            news.XmlOpening = opening.toString();
            
            XMLElement section = new XMLElement("Sections");
            foreach (listObjectItems list_ in listaSeccionts)
            {
                XMLElement tag = new XMLElement("Section");
//                tag.AddValue(new XMLElement("SectionsID", list_.idItem.ToString()));
                tag.AddValue(new XMLElement("Sections", list_.Sections));
                                
                var singleItems = (from word in listaItems
                                   where word.idItem == list_.idItem
                                   select word).ToList();

                XMLElement items = new XMLElement("Items");   
                foreach(var itemnew in singleItems)
                {
                    var n = itemnew.Sections;
                    n=n.Replace("<ul>", "");
                    n=n.Replace("</ul>", "");
                    n=n.Replace("<li>", "*");
                    n=n.Replace("</li>", "");
                    
                    var listening = (from word in n.Split('*')
                                    select word).ToList();

                    foreach (string nn in listening)
                    {
                        XMLElement item = new XMLElement("Item", itemnew.Sections);
                        if(nn!=""){
                        item.AddAttribute("Type", "A");
                        item.AddAttribute("Value", nn);
                        items.AddValue(item);
                        }
                    }
                    
                }
                tag.AddValue(items);
                                
                section.AddValue(tag);
            }
            
            news.XmlSections = section.toString();
            
             XMLElement closing = new XMLElement("Closing");
             closing.AddValue(closingText);
             news.XmlClosing = closing.toString();*/

            /*
            for (int i = 1; i < txtTitle.Length; i++)
            {
                XMLElement tag = new XMLElement("Section");
                tag.AddValue(new XMLElement("Title", txtTitle[i]));
                tag.AddValue(new XMLElement("Description", txtDescription[i]));

                string txtItem = form["txtItem_" + i];

                if (txtItem != null)
                {
                    XMLElement items = new XMLElement("Items");
                    string[] txtItems = txtItem.Split(',');
                    string comments = form["txtItemComment_" + i];
                    string[] txtItemsComment = comments.Split(',');
                    for (int j = 0; j < txtItems.Length; j++)
                    {
                        XMLElement item = new XMLElement("Item", txtItemsComment[j]);
                        item.AddAttribute("Type", "A");
                        item.AddAttribute("Value", txtItems[j]);
                        items.AddValue(item);
                    }
                    tag.AddValue(items);
                }
                section.AddValue(tag);
            }

            news.XmlSections = section.toString();*/
            
        }

        #endregion

        //public ActionResult LoadEditor()
        //{
        //    decimal Id = (decimal)Session["NewsLetterId"];
        //    File file=FileService.GetByEntityId(Id);
        //    ViewData["Content"] = "<b> Read from file Example.</b>";
        //    return View("Editor");
        //}

        ////[ValidateRequest(false)] 
        //[ValidateInput(false)]
        //public ActionResult SaveEdition(FormCollection form)
        //{
        //    decimal Id = (decimal)Session["NewsLetterId"];
        //    File file = FileService.GetByEntityId(Id);
        //    string editorValue = HttpUtility.HtmlDecode(Request["txtEditor"].ToString());
        //    string rute = AppDomain.CurrentDomain.BaseDirectory + FileService.GetFilePathByType(FileUploadType.NewsLetter);
        //    if (file == null)
        //    {
        //        file = new File();
        //        SetDefaultDataForSave(file);
        //        System.IO.StreamWriter writer = System.IO.File.CreateText(rute + "Test.html");
        //        writer.WriteLine(editorValue);
        //        file.FileName = "Test.html";
        //        writer.Close();
        //        FileService.Save(file);

        //    }
        //    else
        //    {
        //        System.IO.StreamWriter writer = System.IO.File.CreateText(rute+file.FileName);
        //        writer.WriteLine(editorValue);
        //        writer.Close();
        //    }

        //    return null;
        //}

        #region Private Methods
        
        public JsonResult PreviewContent(string id)
        {
         
            Template template = TemplateService.GetById(Convert.ToDecimal(id));

           string content = PrePreviewTemplate(Convert.ToDecimal(id));
           return Json(new
            {
                idtemplate = template.Id,
                contenttemplate = content
            });
        }

        //SendMail play some role?
        protected void SendMail(string Id)
        { 
            Newsletter news = NewsletterService.GetById(decimal.Parse(Id));
            string UserId = (string)Session["UserId"];
            news.Sections = NewsletterService.GetSections(news.Id);
            //Change "news.ContentText"  for "PrePreview , en Content is include template             
            EmailService.SendNewsLetterMail(news, news.ContentText, UserId);
        }

        public ActionResult SendMail2(string Id)
        {
            string result = string.Empty;
            string resultbylist = string.Empty;
            Newsletter news = NewsletterService.GetById(decimal.Parse(Id));
            string UserId = (string)Session["UserId"];
            news.Sections = NewsletterService.GetSections(news.Id);
            //Change "news.ContentText"  for "PrePreview , en Content is include template
            //EmailService.SendNewsLetterMail(news, news.ContentText, UserId);
           // IList<EmailHistory> emailHistoryList = EmailService.SendNewsLetterMailByHistory(news, news.ContentText, UserId);
            //IList<EmailHistory> emailHistoryList = EmailService.SendNewsLetterMailAlter(news, news.ContentText, UserId);
            IList<EmailHistory> emailHistoryList = EmailService.SendNewsLetterMailAlterNew(news, news.ContentText, UserId);
            if (emailHistoryList != null && emailHistoryList.Count>0) { 
                foreach(EmailHistory eh in emailHistoryList)
                {
                    if (!eh.Status.Equals(EmailStatus.Sent))
                    {
                        if (!string.IsNullOrEmpty(resultbylist))
                        {
                            resultbylist += ", ";
                        }
                        resultbylist += eh.EmailTo;
                    }
                
                }
                if (string.IsNullOrEmpty(resultbylist))
                {
                    result = "Success";
                }
                else { 
                    result = "There was a problem and we were unable to send the mail to:" +resultbylist;
                }
            }

            else { result = "Error"; }

            //return null;
            return Content(result);
        }


        public ContentResult CheckDestinationAddress()
        {
            string result = string.Empty;
            string sDestination = Request.Params["Destination"];
            string[] address = sDestination.Split(',');
            string addressIncorrectFormat = string.Empty;
            string addressCorrectFormat = string.Empty;
            string addressErrorSent = string.Empty;
            if (address.Length > 0)
            {
                for (int i = 0; i < address.Length; i++)
                { 
                    if(Validator.IsEmailOrWhite( address[i].Trim()))
                    {
                        if (!string.IsNullOrEmpty(addressCorrectFormat))
                        {
                            addressCorrectFormat += ", ";
                        }
                        addressCorrectFormat += address[i];
                    }
                    else
                    {
                        if(!string.IsNullOrEmpty(addressIncorrectFormat))
                        {
                            addressIncorrectFormat+=", ";
                        }
                        addressIncorrectFormat += address[i];
                    }
                }
                if (!string.IsNullOrEmpty(addressCorrectFormat))
                {
                    IList<EmailHistory> ehList = EmailService.SendNewsLetterCheckAddress(addressCorrectFormat, CurrentUser, CurrentCompany);
                    if (ehList != null && ehList.Count > 0)
                    {
                        foreach (EmailHistory eh in ehList)
                        {
                            if (eh.Status.Equals(EmailStatus.ErrorSent))
                            {
                                if (!string.IsNullOrEmpty(addressErrorSent))
                                {
                                    addressErrorSent += ", ";
                                }
                                addressErrorSent += eh.EmailTo;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(addressIncorrectFormat))
            {
                //result += "Address with incorrect Format: " + addressIncorrectFormat;
                result += addressIncorrectFormat;
            }
            if (!string.IsNullOrEmpty(addressErrorSent))
            {
                if (!string.IsNullOrEmpty(addressIncorrectFormat))
                {
                    //result += "\n";
                    result += ", ";
                }
                //result += "Address " + addressErrorSent;
                result += addressErrorSent;
            }
            return Content(result);
        }

        #endregion

        public ActionResult CreateCalendar()
        {

            int year = DateTime.Today.Year;

            ViewData["Calendars"] = NewsletterService.GetItemsByYear(year, CurrentCompany);
            return View("NewsLetterCalendar");
        }


        private string PrePreviewTemplate(decimal Id)
        {
            
            HtmlParser html = new HtmlParser();
            File file = null;
            string rute = string.Empty;
            try
            {
                file = FileService.GetByEntityId(Id, DomainObjectType.Template);
                rute = GenericService.GetPathBase("TemplateFilePath", true);
            }
            catch
            {
                return "Template File not found...!";
            }
            html.Load(rute + file.PhysicalName);

            //html.AddParameter("title", news.Title);
            html.AddParameter("date", DateTime.Today.Date.ToString());
            html.AddParameter("time", DateTime.Today.TimeOfDay.ToString());

            HtmlParserRegex htmlparser = HtmlParserRegex.New();
           
            string textContent = string.Empty;                         
            ClientCompany clientcompany =  ClientCompanyService.GetById(Convert.ToString(Session["ClientCompany"]));
            string nameClientcompany = clientcompany.Name;


            html.AddParameter("contenttext", textContent);           
            html.AddParameter("time", DateTime.Today.TimeOfDay.ToString());
            html.AddParameter("opentext", "");            
            html.AddParameter("closetext", "");
            html.AddParameter("title", "");

            html.AddParameter("ClientCompany", nameClientcompany);

           string AllContent = (string)html.innerHTML.Clone();


           AllContent = AllContent.Replace("\n", "");
           AllContent = AllContent.Replace("\\", "");
           AllContent = AllContent.Replace("\t", "");
           AllContent = AllContent.Replace("\r", "");
           

            return html.MakeString(AllContent);
    
    }

        public JsonResult GetNameestination(decimal id)
        {
            UserProfile userprofile = UserProfileService.GetById(Convert.ToString(id));

            string Nameuser = "";
            string Nametype = "";                

            if (userprofile != null)
            {
               Nameuser = userprofile.Email;
               Nametype = "User";                
            }
            else {

                Team team = TeamService.GetById(id);
                Nameuser = team.Name;
                Nametype = "Team";            
            }            

            return Json(new
            {
                Name = Nameuser,
                Type = Nametype
                

            });
        }

    }



    public class listObjectItems{
        private int _iditem;
        private string _sections;
        
        public int idItem { 
            get { return _iditem;}
            set { _iditem = value; }
        }
        public string Sections {
            get {return _sections ; }
            set {_sections=value; }
        }

        public listObjectItems(int iditem,string sections){
            this.idItem = iditem;
            this.Sections = sections;
        }
    }

     
    
}

