using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility;
using Compelligence.Util.Type;
using Compelligence.Common.Utility.Upload;
using Compelligence.Util.Common;
using System.Web.Configuration;
using Compelligence.Web.Models.Web.Attributes;
using Compelligence.Util.Validation;
using System.Net;
using Compelligence.Common.Utility.Parser;
//using System.IO;

using System.Text.RegularExpressions;
using Compelligence.Web.Models.Web;

namespace Compelligence.Web.Controllers
{

    public class FileController : BackEndAsyncFormController<File, decimal>
    {
        #region Member Variables

        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        protected static IDictionary<string, FileUploadType> UploadTypes = new Dictionary<string, FileUploadType>();

        #endregion

        static FileController()
        {
            UploadTypes[DomainObjectType.Project] = FileUploadType.Content;
            UploadTypes[DomainObjectType.Template] = FileUploadType.Template;
        }

        #region Public Properties

        public IFileService FileService
        {
            get { return (IFileService)_genericService; }
            set { _genericService = value; }
        }

        public ITemplateService TemplateService { get; set; }
        public IUserProfileService UserProfileService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }

        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }

        #endregion

        #region Action Methods


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckInError(string error)
        {
            string raction = Request["action"];
            if (raction.Equals("exception"))
            {
                return Content("Fail");//Content(Resources.LabelResource.LibraryUploadFileSizeError);
            }
            return Content(string.Empty);
        }

        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckIn()
        {  
            string fileNameResult = string.Empty;

            //Artifice for return Fail if file size is more than 20Mb, it works with Application_BeginRequest in Global.asax.cs + ajaxupload.js
            HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
            int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;//Approx 100 Kb(for page content) size
            if ((Request.ContentLength > maxRequestLength))
            {
                return Content("Fail");
            }
            //End-Artifice


            string headerType = (string)Request["HeaderType"];
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                File newFile = new File();

                if (hpf.ContentLength == 0)
                {
                    continue;
                }

                newFile.FileName = System.IO.Path.GetFileName(hpf.FileName);

                newFile.FileFormat = newFile.FileName.Substring(newFile.FileName.LastIndexOf('.') + 1);

                if (string.Compare(headerType, DomainObjectType.Template) == 0)
                {
                    string entityId = GetDetailFilterValue("File.EntityId");

                    if (!string.IsNullOrEmpty(entityId))
                    {
                        Template template = TemplateService.GetById(Convert.ToDecimal(entityId));

                        if ((template != null) && template.TemplateType.Equals(TemplateType.Newsletter))
                        {
                            if (!string.IsNullOrEmpty(newFile.FileFormat))
                            {
                                if (!((string.Compare(newFile.FileFormat.ToLower(), "htm") == 0) ||
                                    (string.Compare(newFile.FileFormat.ToLower(), "html") == 0)))
                                {
                                    return Content(", failed operation wrong format!");
                                }
                            }
                            else
                            {
                                return Content(", failed operation wrong format!");
                            }
                        }
                    }
                }

                SetDetailFilterData(newFile);

                SetDefaultDataForSave(newFile);

                FileService.SaveNewFileVersion(newFile, CurrentUser);

                hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + GetFilePath(UploadTypes[newFile.EntityType]), newFile.PhysicalName));

                fileNameResult = newFile.FileName;
            }

            return Content(" done.!");

        }

        public JsonResult SaveFileLink(string valuelink, string EntityId, string TypeLink)
        {
            string msg = "Fail";

            if (!Validator.IsBlankOrNull(valuelink))
            {
                File newFile = new File();            
                msg = "ok";

                newFile.EntityType = DomainObjectType.Project;
                newFile.EntityId = Convert.ToDecimal(EntityId);               
                newFile.FileFormat = FileFormat.Link;

                string[] urlObjects = valuelink.Split('.');
                int last = urlObjects.Length - 1;
                string formaturl = urlObjects[last];

                if (valuelink.IndexOf("http://") == -1 && valuelink.IndexOf("https://") == -1)
                {
                    string parameter2 = "http://" + valuelink;
                    valuelink = parameter2;
                }

                if (TypeLink.Equals("AddLink"))
                {
                    if (Validator.IsValidUrlDowloadFile(valuelink))
                    {
                        newFile.FileName = valuelink;
                        newFile.Description = valuelink;
                        SetDetailFilterData(newFile);
                        SetDefaultDataForSave(newFile);
                        FileService.SaveNewFileVersion(newFile, CurrentUser);
                    }
                    else
                    {
                        msg = "Fail";
                    }
                }
                else
                {
                    
                        if (Validator.IsValidUrlDowloadFile(valuelink))
                        {
                            if (string.IsNullOrEmpty(System.IO.Path.GetExtension(valuelink)))
                            {
                                valuelink = valuelink + ".html";
                            }
                            UrlParser url = new UrlParser(valuelink);
                            string valuename = MakeValidFileName(url.GetLastUrlObjects(valuelink));                            
                            newFile.FileName = valuename;
                            SetDetailFilterData(newFile);
                            SetDefaultDataForSave(newFile);
                            FileService.SaveNewFileVersion(newFile, CurrentUser);
                            if (StringUtility.IsURlFile(formaturl))
                            {
                                DownloadWebPage(valuelink, newFile, false);
                            }
                            else{
                                DownloadWebPage(valuelink, newFile, true);
                                 msg = "StandardFileError";
                            }
                        }
                        else
                        {
                            msg = "Fail";
                        }                    
                }       
            }        

            return Json(new
            {
                message = msg
            
            });
                    
        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return System.Text.RegularExpressions.Regex.Replace(name, invalidReStr, "_");
        }

        protected void DownloadWebPage(string parameter, File newFile, bool invalidFile)
        {
            WebClient webClient = new WebClient();
            string site = System.IO.Path.Combine(ContextFilePath + GetFilePath(UploadTypes[newFile.EntityType]), newFile.PhysicalName);
            Uri url = new Uri(parameter);
            if (!invalidFile)
            {                
                webClient.DownloadFileAsync(url, site);
            }
            else
            {               

                using (System.IO.StreamWriter sw = System.IO.File.CreateText(site))
                {
                    sw.Write(webClient.DownloadString(url));
                }
                
            }
        }
       
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage()
        {
            string fileNameResult = string.Empty;
            string headerType = (string)Request["HeaderType"];
            string userId = (string)Session["UserId"];
            string Result = string.Empty;
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                File newFile = new File();

                if (hpf.ContentLength == 0)
                {
                    continue;
                }

                newFile.FileName = System.IO.Path.GetFileName(hpf.FileName);

                newFile.FileFormat = newFile.FileName.Substring(newFile.FileName.LastIndexOf('.') + 1);

                

                //SetDetailFilterData(newFile);
                decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                string newPhysicalName = string.Empty + genericid + "_" + newFile.FileName;

                string tempoImage = string.Empty;
                
                SetDefaultDataForSave(newFile);
                newFile.PhysicalName = newPhysicalName;

                FileService.Save(newFile);
                

                fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFile.PhysicalName);
                
                hpf.SaveAs(fileNameResult);
                tempoImage = fileNameResult;
                Compelligence.Common.Utility.ResizeImage.GetInstance().Resize(fileNameResult, tempoImage, 170, 170);
                Result = newFile.PhysicalName;
            }

            return Content("."+System.IO.Path.Combine( GetFilePath(FileUploadType.Image), Result).Replace("\\","/"));

        }

        public ActionResult UploadFileRawMode()
        {
            string fileNameResult = string.Empty;
            fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            string FileName = Request.Params["HTTP_X_FILE_NAME"];

            fileNameResult += "_" + System.IO.Path.GetFileName(FileName).Replace(' ', '_');
            System.IO.FileStream RawFile = new System.IO.FileStream(System.IO.Path.Combine(ContextFilePath + ConfigurationSettings.AppSettings["TempFilePath"], fileNameResult), System.IO.FileMode.OpenOrCreate);
            byte[] buffer = new byte[1024];
            int ibytes;

            while ((ibytes = Request.InputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                RawFile.Write(buffer, 0, ibytes);
            }
            RawFile.Close();

            return Content("{result:'" + fileNameResult + "'}");

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckOut()
        {
            string userId = (string)Session["UserId"];
            bool isDetail = Convert.ToBoolean(Request["IsDetail"]);
            string detailFilter = Request["DetailFilter"];
            File file = new File();
            string mimeType = null;
            //            Response.ContentType = 
            if (isDetail && (!string.IsNullOrEmpty(detailFilter)))
            {
                SetDetailFilterData(file);

                File lastVersion = FileService.UpdateLastFileVersionToCheckOut(DecimalUtility.CheckNull(file.EntityId), file.EntityType);

                if (lastVersion != null)
                {
                    mimeType = FileUtility.GetMimeType("~" + GetFilePath(UploadTypes[file.EntityType]) + lastVersion.PhysicalName);
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + lastVersion.FileName.Replace(' ', '_'));
                    Response.Clear();
                    Response.WriteFile("~" + GetFilePath(UploadTypes[file.EntityType]) + lastVersion.PhysicalName);
                    FileService.SendEmailByFileCheckOut(lastVersion, userId);
                    Response.End();
                    //return new DownloadResult { VirtualPath = "~" + GetFilePath(UploadTypes[file.EntityType]) + lastVersion.PhysicalName, FileDownloadName = lastVersion.FileName.Replace(' ', '_') };
                }
            }

            return Content("File was not found");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckOutById()
        {
            string userId = (string)Session["UserId"];
            bool isDetail = Convert.ToBoolean(Request["IsDetail"]);
            string detailFilter = Request["DetailFilter"];
            string fileId = Request["FileId"];
            if (!string.IsNullOrEmpty(fileId))
            {
                decimal id = decimal.Parse(fileId);
                string mimeType = null;

                if (isDetail && (!string.IsNullOrEmpty(detailFilter)))
                {
                    File file = FileService.GetById(id);
                    if (file != null)
                    {
                        mimeType = FileUtility.GetMimeType("~" + GetFilePath(UploadTypes[file.EntityType]) + file.PhysicalName);
                        Response.ContentType = mimeType;
                        Response.AddHeader("content-disposition", "attachment; filename=" + file.FileName.Replace(' ', '_'));
                        Response.Clear();
                        Response.WriteFile("~" + GetFilePath(UploadTypes[file.EntityType]) + file.PhysicalName);
                        FileService.SendEmailByFileCheckOut(file, userId);
                        Response.End();
                    }
                }
            }
            return Content("File was not found");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckOutTemplate()
        {
            string templateId = Request["TemplateId"];
            FileUploadType uploadType = FileUploadType.Template;

            if (!string.IsNullOrEmpty(templateId))
            {
                string filePath = GetFilePath(uploadType);
                string mimeType = null;
                Template template = TemplateService.GetById(Convert.ToDecimal(templateId));
                File file = FileService.GetByEntityId(template.Id, DomainObjectType.Template);

                if (file != null)
                {
                    mimeType = FileUtility.GetMimeType("~" + filePath + file.PhysicalName);
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.FileName.Replace(' ', '_'));
                    Response.Clear();
                    Response.WriteFile("~" + filePath + file.PhysicalName);
                    Response.End();
                    //return new DownloadResult { VirtualPath = "~" + filePath + file.PhysicalName, FileDownloadName = file.FileName };
                }
            }

            return Content("File was not found");
        }

        public ActionResult CreateDetailOfLibrary()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string headerType = (string)Request["HeaderType"];

            if (headerType.Equals(DomainObjectType.Project))
            {
                string detailTypeParam = Request["DetailCreateType"];
                string entityId = GetDetailFilterValue("File.EntityId");
                string entityType = GetDetailFilterValue("File.EntityType");

                string fileNameResult = string.Empty;


                foreach (string idlibrary in ids)
                {
                   // File file = FileService.GetByEntityId(decimal.Parse(idlibrary), DomainObjectType.Library);
                    File fileO = FileService.GetById(decimal.Parse(idlibrary));
                    if (fileO != null) //have file
                    {
                        File file = new File();
                        file = fileO;
                        if (file.Id == null)
                        {
                            file.Id = 0;
                        }
                        file.EntityId = decimal.Parse(entityId);
                        file.EntityType = entityType;
                        String oldPhysicalName = file.PhysicalName;
                        FileService.SaveNewFileVersion(file, CurrentUser);
                        try
                        {
                            System.IO.File.Move(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["LibraryFilePath"], oldPhysicalName),
                                                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["ContentFilePath"], file.PhysicalName));
                            //LibraryService.Delete(library.Id);
                        }
                        catch { }
                    }
                }
                
            }
            return null;
        }

        #endregion

        #region Override Methods

        protected override void SetDetailFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<Template> templateList = TemplateService.GetAllActiveByClientCompany(clientCompany);

            string entityId2 = GetDetailFilterValue("File.EntityId");           
            ViewData["TemplateList"] = new SelectList(templateList, "Id", "Name");
            ViewData["EntityIdfilter"] = entityId2;
        }

        protected override void SetEntityDataToForm(File file)
        {
            if (file.EntityType.Equals(DomainObjectType.Template) && string.IsNullOrEmpty(file.HtmlDescription))
            {
                file.HtmlDescription = GetSourceCodeOfFile(file);
            }
        }

        private string GetSourceCodeOfFile(File file)
        {

            HtmlParser html = new HtmlParser();
            string rute = string.Empty;
            try
            {
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
            ClientCompany clientcompany = ClientCompanyService.GetById(CurrentCompany);
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

        #endregion

        #region Private Methods
        
        private string GetFilePath(FileUploadType type)
        {
            string filePath;

            switch (type)
            {
                case FileUploadType.Template:
                    filePath = ConfigurationSettings.AppSettings["TemplateFilePath"];
                    break;
                case FileUploadType.Image:
                    filePath = ConfigurationSettings.AppSettings["ImageFilePath"];
                    break;
                default:
                    filePath = ConfigurationSettings.AppSettings["ContentFilePath"];
                    break;
            }

            return filePath;
        }

        #endregion

        protected override void SetFormData()
        {
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(CurrentCompany);
            ViewData["UserIdlist"] = new SelectList(userList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CreateTemplate()
        {
            SetDefaultRequestParametersToForm(ActionMethod.Create, OperationStatus.Initiated);
           ViewData["ActionMethod"] = "SaveTemplate";
            SetFormData();

            SetUserSecurityAccess();

            return View("EditTemplate");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SaveTemplate(File entity, FormCollection collection)
        {
            OperationStatus operationStatus = OperationStatus.Initiated;
            if (ValidateFormData(entity, collection))
            {
                SetDetailFilterData(entity);
                GetFormData(entity, collection);
                SetDefaultEntityDataForSave(entity);
                SetDefaultDataForSave(entity);
                SetValueToFile(entity);
                FileService.SaveNewFileVersion(entity, CurrentUser);
                string headerType = (string)Request["HeaderType"];
                string fileName = entity.PhysicalName;
                string path = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Template), fileName); ;
                if (!System.IO.File.Exists(path))
                {
                    using (System.IO.StreamWriter sw = System.IO.File.CreateText(path))
                    {
                        sw.WriteLine("<HTML>");
                        sw.WriteLine("<HEAD>");
                        sw.WriteLine("<TITLE>" + entity.FileName + "</TITLE>");
                        sw.WriteLine("</HEAD><BODY>");
                        sw.Write(entity.HtmlDescription);
                        sw.WriteLine("</BODY></HTML>");
                    }
                }
                // Open the file to read from.
                using (System.IO.StreamReader sr = System.IO.File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                operationStatus = OperationStatus.Successful;
                RedirectToAction("EditTemplate", new { id = entity.Id, operationStatus = operationStatus.ToString(), Scope = Request["Scope"], BrowseId = Request["BrowseId"], IsDetail = Request["IsDetail"], Container = Request["Container"], HeaderType = Request["HeaderType"], DetailFilter = Request["DetailFilter"] });
            }
            SetDefaultRequestParametersToForm(ActionMethod.Create, operationStatus);
            SetFormData();
            SetFormEntityDataToForm(entity);
            SetUserSecurityAccess();
            return View("EditTemplate", entity);
        }

        public void SetValueToFile(File file)
        {
            if (!string.IsNullOrEmpty(file.HtmlDescription))
            {
                file.Description = StripHtml.GetInstance().GetTextPlain(file.HtmlDescription);
                if (file.Description.Length > 2000)
                {
                    file.Description = file.Description.Substring(0, 2000);
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditTemplate(decimal id, string operationStatus)
        {
            File entityObject = GenericService.GetById(id);

            OperationStatus operationStatusParam = OperationStatus.Initiated;



            if (!string.IsNullOrEmpty(operationStatus))
            {
                operationStatusParam = (OperationStatus)Enum.Parse(typeof(OperationStatus), operationStatus);
            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatusParam);
            ViewData["ActionMethod"] = "EditTemplate";
            SetFormData();
            //ReadHtmlFileAndSetBody(entityObject);
            SetEntityDataToForm(entityObject);

            SetUserSecurityAccess(entityObject);

            SetEntityLocking(entityObject);

            return View("EditTemplate", entityObject);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)] //it's dangerous, enabledonly for Library and Newsletter
        public ActionResult EditTemplate(decimal id, File formEntity, FormCollection collection)
        {

            File entityResult = formEntity;
            File entity = GenericService.GetById(id);

            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEditFormData(formEntity, collection))
            {
                SetFormDataToEntity(entity, collection);

                GetFormData(entity, collection);

                SetDefaultDataForUpdate(entity);

                GenericService.Update(entity);

                string headerType = (string)Request["HeaderType"];
                string fileName = entity.PhysicalName;
                string path = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Template), fileName);
                if (System.IO.File.Exists(path))
                {
                    string idTemp = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                    string newFileName = idTemp + "_" + entity.PhysicalName;
                    string newPath = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Template), newFileName);


                    System.IO.File.Move(path, newPath);

                    using (System.IO.StreamWriter sw = System.IO.File.CreateText(path))
                    {
                        sw.WriteLine("<HTML>");
                        sw.WriteLine("<HEAD>");
                        sw.WriteLine("<TITLE>" + entity.FileName + "</TITLE>");
                        sw.WriteLine("</HEAD><BODY>");
                        sw.Write(entity.HtmlDescription);
                        sw.WriteLine("</BODY></HTML>");
                    }
                }
                else
                {
                    using (System.IO.StreamWriter sw = System.IO.File.CreateText(path))
                    {
                        sw.WriteLine("<HTML>");
                        sw.WriteLine("<HEAD>");
                        sw.WriteLine("<TITLE>" + entity.FileName + "</TITLE>");
                        sw.WriteLine("</HEAD><BODY>");
                        sw.Write(entity.HtmlDescription);
                        sw.WriteLine("</BODY></HTML>");
                    }
                }


                GetActionHistoryUpdated(entity, collection);

                entityResult = entity;

                operationStatus = OperationStatus.Successful;

            }

            SetDefaultRequestParametersToForm(ActionMethod.Edit, operationStatus);

            SetFormData();

            SetFormEntityDataToForm(entityResult);

            SetUserSecurityAccess(entity);

            SetEntityLocking(entity);

            return GetActionResultForEdit(entity, operationStatus);
        }


        private void ReadHtmlFile(File file)
        {
            string path = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Template), file.PhysicalName + ".html");
            System.IO.FileStream fileObject = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);

            // Create a new stream to read from a file
            System.IO.StreamReader sr = new System.IO.StreamReader(fileObject);

            // Read contents of file into a string
            string cval = sr.ReadToEnd();
            file.HtmlDescription = cval;
            Response.Write(cval);
            // Close StreamReader
            sr.Close();

            // Close file
            fileObject.Close();
        }
    }
}
