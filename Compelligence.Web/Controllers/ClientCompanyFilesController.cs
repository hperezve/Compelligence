using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Security.Filters;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Common.Utility;
using Compelligence.Util.Type;
using System.Configuration;
using System.Net;
using Compelligence.Util.Common;

namespace Compelligence.Web.Controllers
{
    public class ClientCompanyFilesController : Controller
    {
        public IClientCompanyFilesService ClientCompanyFilesService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }

        [AuthenticationMaintenanceFilter]
        public ActionResult Index(string id)
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(id);
            ViewData["ClientCompanyId"] = clientCompany.ClientCompanyId;
            ViewData["ClientCompanyFile"] = ClientCompanyFilesService.GetAllActiveByClientCompany(clientCompany.ClientCompanyId);
            ViewData["CompanyFile"] = new ClientCompanyFiles();
            
            return View();
        }
               

        ClientCompanyFiles clientCompanyFiles = new ClientCompanyFiles();

        [ValidateInput(false)]
        [AuthenticationMaintenanceFilter]
        public ActionResult Save(FormCollection form)
        {
            string Name = (string)form["FileName"];
            string Description = (string)form["Description"];
            string ClientCompanyFileId = (string)form["ClientCompanyFilesId"];
            string ClientCompanyId = (string)form["ClientCompanyId"];
            //
            string FileName = GetFile();

            //for XSS
            Name = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["FileName"])); ;
            Description = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Description"]));
            
            //
            // it's need validate
            //

            if (String.IsNullOrEmpty(ClientCompanyFileId) ||
                (!String.IsNullOrEmpty(ClientCompanyFileId) && ClientCompanyFileId.Equals("0")))
            { //it's new

                if (!String.IsNullOrEmpty(Name) &&
                    !String.IsNullOrEmpty(Description) &&
                    !String.IsNullOrEmpty(FileName))
                {
                    
                    clientCompanyFiles.Id = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                    clientCompanyFiles.Name = Name;
                    clientCompanyFiles.FileName = FileName;
                    clientCompanyFiles.Description = Description;
                    clientCompanyFiles.CreatedDate = DateTime.Now;
                    clientCompanyFiles.ClientCompany = ClientCompanyId; 
                    ClientCompanyFilesService.Save(clientCompanyFiles);
                }

            }
            else //update
            {
              ClientCompanyFiles clientCompanyFile = ClientCompanyFilesService.GetById(Convert.ToDecimal(ClientCompanyFileId));            
              if (FileName != null && FileName != "")
              {
                clientCompanyFile.FileName = FileName;
              }
              clientCompanyFile.Name = Name;
              clientCompanyFile.Description = Description;
              clientCompanyFile.LastChangedDate = DateTime.Now;
              ClientCompanyFilesService.Update(clientCompanyFile);
            }
            return RedirectToAction("Index", "ClientCompanyFiles", new { id = ClientCompanyId });
        }

        protected string GetFile()
        {
            string pathtemp = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["ClientCompanyFilesPath"];

            string fileName = string.Empty;

            if (Request.Files == null || (Request.Files != null && Request.Files.Count == 0))
                return string.Empty;

            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
            if (hpf.ContentLength > 0)
            {
                fileName = System.IO.Path.GetFileName(hpf.FileName);
                fileName = fileName.Replace(' ', '_');
                hpf.SaveAs(System.IO.Path.Combine(pathtemp, fileName));
            }
            return fileName;
        }

        public ActionResult List()

        {
            string[] dates = Request.UrlReferrer.ToString().Split('/');
            string ClientCompanyId2 = dates[5];
            ViewData["ClientCompanyFile"] = ClientCompanyFilesService.GetAllActiveByClientCompany(ClientCompanyId2); 
            
            return View();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult Edit(decimal id)
        {
            ClientCompanyFiles CompanyFile = ClientCompanyFilesService.GetById(id);
            ViewData["CompanyFile"] = CompanyFile;
            ViewData["ClientCompanyId"] = CompanyFile.ClientCompany;
            return View("Edit");
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult Download(decimal id)
        {
            string check = StringUtility.CheckNull(Request["chk"]);
            ClientCompanyFiles clientCompanyFile = ClientCompanyFilesService.GetById(Convert.ToDecimal(id));
            if (String.IsNullOrEmpty(clientCompanyFile.FileName))
                return Content("NotFound");

            string path = ConfigurationSettings.AppSettings["ClientCompanyFilesPath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            if (check.ToLower().Equals("true"))
            {
                if (System.IO.File.Exists(fullpath + clientCompanyFile.FileName))
                    return Content("Found");
                else
                    return Content("NotFound");
            }
            else
            {
                GetDownloadFileResponse(path, clientCompanyFile.FileName, clientCompanyFile.FileName);
            }
            
            return Content(string.Empty);
        }

        protected void GetDownloadFileResponse(string path, string physicalName, string fileName)
        {
            Response.ContentType = FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.Clear();
            Response.WriteFile("~\\" + path + physicalName);
            Response.End();
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult Delete(decimal id)
        {
            ClientCompanyFiles clientCompanyFile = ClientCompanyFilesService.GetById(id);            
            ClientCompanyFilesService.Delete(clientCompanyFile);
            return RedirectToAction("Index", "ClientCompanyFiles", new { id = clientCompanyFile.ClientCompany });                                               
            
        }

    
    }
}
