using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.BusinessLogic.Implementation;
using System.Configuration;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Util.Common;
using Compelligence.Common.Utility;
using System.Net;
using Compelligence.Security.Filters;
using Compelligence.Web.Models.Util;

namespace Compelligence.Web.Controllers
{
    public class WhitePaperController : Controller
    {

        public IWhitePaperService WhitePaperService { get; set; }
        public IWhitePaperDetailService WhitePaperDetailService { get; set; }

        //
        // GET: /WhitePaper/
        [AuthenticationMaintenanceFilter]
        public ActionResult Index()
        {            
            ViewData["WhitePapers"] = WhitePaperService.GetByStatus(WhitePaperStatus.Enabled);
            ViewData["WhitePaper"] = new WhitePaper();
            return View();
        }
        

        public void FindByIdWhitePaper()
        {
            WhitePaperDetail WhiteDetail = new WhitePaperDetail();
            ViewData["GetAll"] = WhitePaperDetailService.GetAll();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download(decimal id)
        {
            string check = StringUtility.CheckNull(Request["chk"]);
            WhitePaper whitepaper = WhitePaperService.GetById(id);
            if ( String.IsNullOrEmpty(whitepaper.FileName) )
                return Content("NotFound");

            string path = ConfigurationSettings.AppSettings["WhitePapersPath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            if (check.ToLower().Equals("true"))
            {
                if (System.IO.File.Exists(fullpath + whitepaper.FileName))
                    return Content("Found");
                else
                    return Content("NotFound");
            }
            else
            {
                DateTime Downloaded = DateTime.Now;
                ViewData["Downloaded"] = Downloaded;
                String remoteClientIp = (String)Request.ServerVariables["HTTP_X_FORWARDED_FOR"]; //try get proxy
                if (String.IsNullOrEmpty(remoteClientIp) ||
                   (!String.IsNullOrEmpty(remoteClientIp) && remoteClientIp.ToLower().Equals("unknown")))
                    remoteClientIp = (String)Request.ServerVariables["REMOTE_ADDR"];

                String remoteClientDns = "unknown";
                try
                {
                    IPHostEntry ClientDns = Dns.GetHostEntry(remoteClientIp);
                    remoteClientDns = ClientDns.HostName;
                }
                catch //if any error assume dns unknown
                { 
                }
                    
                WhitePaperDetail WhitepaperDetail = new WhitePaperDetail();
                WhitepaperDetail.WhitePaperId = whitepaper.Id;
                WhitepaperDetail.ClientIp = remoteClientIp;
                WhitepaperDetail.ClientDns = remoteClientDns;
                WhitepaperDetail.DownLoadedDate = Downloaded;
                WhitePaperDetailService.Save(WhitepaperDetail);                
                GetDownloadFileResponse(path, whitepaper.FileName, whitepaper.FileName);
            }

            return Content(string.Empty);
        }

        public ActionResult List()
        {
            ViewData["WhitePapers"] = WhitePaperService.GetByStatus(WhitePaperStatus.Enabled);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetEventTypes()
        {
            IList<WhitePaper> WtTypeList = WhitePaperService.GetByStatus(WhitePaperStatus.Enabled);
            return ControllerUtility.GetSelectOptionsFromGenericList(WtTypeList, "Id", "Name");
        }


        
        WhitePaper whitepaper = new WhitePaper();
        [ValidateInput(false)]
        [AuthenticationMaintenanceFilter]
        public ActionResult Save(FormCollection form)
        {
            string Label = (string)form["Label"];
            string Summary = (string)form["Summary"];
            string WhitePaperId = (string)form["WhitePaperId"];            
            string FileName = GetFile();
            

            if (WhitePaperId != null && WhitePaperId != "" && WhitePaperId != "0")
            {
                WhitePaper WhitePapers = WhitePaperService.GetById(Convert.ToDecimal(WhitePaperId));

                if (FileName != null && FileName != "")
                { WhitePapers.FileName = FileName; }                                               
                WhitePapers.Label = Label;                
                WhitePapers.Id = Convert.ToDecimal(WhitePaperId);
                WhitePapers.LastChangedDate = DateTime.Now;
                WhitePapers.Summary = Summary;
                WhitePaperService.Update(WhitePapers);
            }
            else{
                        
            
            if (!String.IsNullOrEmpty(Label)&& 
                !String.IsNullOrEmpty(Summary) && 
                !String.IsNullOrEmpty(FileName))
            {
                //for XSS
                Label = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Label"])); ;
                Summary = HttpUtility.HtmlEncode(StringUtility.CheckNull(form["Summary"]));
                //
                
                whitepaper.Id = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                whitepaper.FileName = FileName;
                whitepaper.Label = Label;
                whitepaper.Summary = Summary;
                whitepaper.Status = WhitePaperStatus.Enabled;
                whitepaper.DateUpload = DateTime.Today.Date;
                WhitePaperService.Save(whitepaper);
            }
                }
            ViewData["WhitePapers"] = WhitePaperService.GetByStatus(WhitePaperStatus.Enabled);
            ViewData["WhitePaper"] = whitepaper;
            return View("Index");
        }

        protected void GetDownloadFileResponse(string path, string physicalName, string fileName)
        {
            Response.ContentType = FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.Clear();
            Response.WriteFile("~\\" + path + physicalName);
            Response.End();
        }

        protected string GetFile()
        {
            string pathtemp = AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["WhitePapersPath"];

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

        [AuthenticationMaintenanceFilter]
        public ActionResult DeleteWhitePaper(decimal id)
        {                                    
            DeleteWhitePapers(id);
            return RedirectToAction("Index", "WhitePaper");
        }

        [AuthenticationMaintenanceFilter]
        public void DeleteWhitePapers(decimal id)
        {
            WhitePaper WhitePapers = WhitePaperService.GetById(id); 
            WhitePapers.Status = WhitePaperStatus.Deleted;
            WhitePapers.LastChangedDate = DateTime.Now;
            WhitePaperService.Update(WhitePapers);
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult Update(decimal id)
        {
            decimal WhiePaperId = Convert.ToDecimal(Request["WhitePaperId"]);
            WhitePaper Whitepaper = WhitePaperService.GetById(id);
            ViewData["WhitePaper"] = Whitepaper;                        
            return View("Edit");
        }
       
        public ActionResult WhiteDownloads(decimal id)
        {            
            ViewData["WhitePaperDetalDownload"] = WhitePaperDetailService.GetWhitePaperDetailById(id);
            //ViewData["WhitePaperName"] = WhitePaperService.GetById(id);
            WhitePaper WhiteNames = WhitePaperService.GetById(id);
            ViewData["NameWhite"] = WhiteNames.Label;            
            return View();
        }


    }
}
