using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Validation;
using Resources;
using Compelligence.BusinessLogic.Interface;
using Compelligence.SalesForceAPI.Resource;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Views;
using Common.Logging;
using Compelligence.Security.Filters;
using Compelligence.Web.Models.Util;
using Compelligence.Common.Utility;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Common.Utility.Upload;



namespace Compelligence.Web.Controllers
{
    public class MyCompanyController : BackEndFormController<ClientCompany, string>
    {

        #region Public Properties
        private static readonly ILog LOG = LogManager.GetLogger(typeof(MyCompanyController));
        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        public IClientCompanyService ClientCompanyService
        {
            get { return (IClientCompanyService)_genericService; }
            set { _genericService = value; }
        }
        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }

        public IFileService FileService { get; set; }
        public IUserProfileService UserProfileService { get; set; }

        public ISalesForceService SalesForceService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public ICountryService CountryService { get; set; }

        #endregion

        #region Action Methods
        [AuthenticationFilterByUserRoot]
        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditProfile()
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            ViewData["Imageurl"] = clientCompany.Imageurl;
            ViewData["Email"] = clientCompany.Email;
            //IList<Country> countryList = CountryService.GetAllActive();
            IList<CountryFirstUSAView> countryList = CountryService.GetFirstUSA();
            ViewData["CountryCodeList"] = new SelectList(countryList, "CountryCode", "Name");
            SetDefaultRequestParametersToForm(OperationStatus.Initiated);

            return View("EditProfile", clientCompany);
        }
        [AuthenticationFilterByUserRoot]
        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProfile(ClientCompany clientCompanyForm, FormCollection formCollection)
        {
            string ImageUrl = Request["Imageurl"];
            List<string> resultImage = ResizeImages(ImageUrl);
            ViewData["Imageurl"] = string.Empty;
            string clientCompanyId = (string)Session["ClientCompany"];
            ClientCompany clientCompanyResult = clientCompanyForm;
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);
            OperationStatus operationStatus = OperationStatus.Initiated;

            if (ValidateEditFormData(clientCompanyForm))
            {
                SetFormDataToEntity(clientCompany, formCollection);

                clientCompany.Imageurl = resultImage[0];

                ClientCompanyService.Update(clientCompany);

                clientCompanyResult = clientCompany;
                if (resultImage.Count == 1)
                {
                    operationStatus = OperationStatus.Successful;
                }
                else
                {
                    ValidationDictionary.AddError("Imageurl", LabelResource.GlobalUrlFormatError);
                }
            }

            SetDefaultRequestParametersToForm(operationStatus);

            SetFormData();

            //string result = ResizeImages();            
            Session["Imageurl"] = clientCompany.Imageurl;
            ViewData["Imageurl"] = clientCompany.Imageurl;
            return View("EditProfile", clientCompanyResult);
        }

        public List<string> ResizeImages(string ImagerUrl)
        {
            List<string> result = new List<string>();
            string fileNameResult = string.Empty;
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            try
            {
                if (!string.IsNullOrEmpty(ImagerUrl))
                {
                    if (ImagerUrl.IndexOf("./FilesRepository/Images/") == -1)
                    {
                        byte[] imageData = ResizeImage.GetBytesFromUrl(ImagerUrl);
                        if (imageData != null)
                        {
                            System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
                            System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(stream);
                            stream.Close();

                            string[] urlObjects = ImagerUrl.Split('/');

                            int newWidth = 280;
                            int newHeight = 70;

                            fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                            fullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                            if (fullsizeImage.Width > newWidth)
                            {
                                newWidth = fullsizeImage.Width;
                            }
                            int resizeHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
                            if (resizeHeight > newHeight)
                            {
                                newWidth = fullsizeImage.Width * newHeight / fullsizeImage.Height;
                                resizeHeight = newHeight;
                            }

                            System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, resizeHeight, null, IntPtr.Zero);
                            fullsizeImage.Dispose();

                            Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();


                            if (urlObjects.Length > 0)
                            {
                                newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                                if (newFileImage.FileName.IndexOf("%20") != -1)
                                {
                                    newFileImage.FileName = newFileImage.FileName.Replace("%20", "-");
                                }
                            }
                            if (newFileImage.FileName.LastIndexOf('.') != -1)
                            {
                                newFileImage.FileFormat = newFileImage.FileName.Substring(newFileImage.FileName.LastIndexOf('.') + 1);//Errir
                            }
                            decimal genericid = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
                            string newPhysicalName = string.Empty + genericid + "_" + newFileImage.FileName;

                            newFileImage.CreatedBy = CurrentUser;
                            newFileImage.LastChangedBy = CurrentUser;
                            newFileImage.CreatedDate = DateTime.Now;
                            newFileImage.LastChangedDate = DateTime.Now;
                            newFileImage.ClientCompany = CurrentCompany;
                            newFileImage.PhysicalName = newPhysicalName;
                            FileService.Save(newFileImage);
                            fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFileImage.PhysicalName);
                            newImage.Save(fileNameResult);
                            ImagerUrl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), newFileImage.PhysicalName).Replace("\\", "/");
                            result.Add(ImagerUrl);
                        }
                        else
                        {
                            result.Add(clientCompany.Imageurl);
                            result.Add("Error");
                        }
                    }
                    else
                    {
                        result.Add(ImagerUrl);
                    }
                }
                else
                {
                    result.Add(ImagerUrl);
                }
            }
            catch
            {
                result.Add(clientCompany.Imageurl);
                result.Add("Error");
            }
            return (result);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AjaxUpload()
        {
            string clientCompanyId = (string)Session["ClientCompany"];
            string fileNameResult = string.Empty;
            string Result = string.Empty;
            string resultMessage = string.Empty;
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                Compelligence.Domain.Entity.File newFile = new Compelligence.Domain.Entity.File();

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

                newFile.CreatedDate = DateTime.Now;

                newFile.ClientCompany = CurrentCompany;

                newFile.CreatedBy = CurrentUser;

                newFile.LastChangedBy = CurrentUser;

                newFile.LastChangedDate = DateTime.Now;

                newFile.PhysicalName = newPhysicalName;

                FileService.Save(newFile);


                fileNameResult = System.IO.Path.Combine(ContextFilePath + GetFilePath(FileUploadType.Image), newFile.PhysicalName);

                hpf.SaveAs(fileNameResult);
                tempoImage = fileNameResult;
                Compelligence.Common.Utility.ResizeImage.GetInstance().Resize(fileNameResult, tempoImage, 280, 70);
                Result = newFile.PhysicalName;
                clientCompany.Imageurl = "." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), Result).Replace("\\", "/");
                ClientCompanyService.Update(clientCompany);
                resultMessage = ("." + System.IO.Path.Combine(GetFilePath(FileUploadType.Image), Result).Replace("\\", "/"));

            }
            ViewData["Imageurl"] = resultMessage;
            return Content(resultMessage);
        }
        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditPassword()
        {
            string clientCompanyId = (string)Session["ClientCompany"];
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);

            SetDefaultRequestParametersToForm(OperationStatus.Initiated);

            SetFormData();

            return View("EditPassword", clientCompany);
        }
        [AuthenticationTypeUserFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPassword(ClientCompany clientCompanyForm, FormCollection formCollection)
        {
            string clientCompanyId = (string)Session["ClientCompany"];
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);

            if (ValidateEditPasswordFormData(clientCompanyForm))
            {
                SetFormDataToEntity(clientCompany, formCollection);
                ClientCompanyService.UpdatePassword(clientCompany);
                ClientCompanyService.Update(clientCompany);
                return RedirectToAction("EditProfile", "MyCompany");
            }
            else
            {
                SetFormData();
            }
            return View("EditPassword");
        }

        public ActionResult ChangeSalesforceButtons()
        {
            //string dns = Request.Url.DnsSafeHost;
            string FromDns = Request.Url.Authority;
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);

            int panelResult = SalesForceService.SetPanelIntoSalesForce(clientCompany.SalesForceUser, clientCompany.SalesForcePassword + clientCompany.SalesForceToken, SalesForceResource.LinkLabel, SalesForceResource.LinkUrl, SalesForceResource.ComponentLabel);
            if (panelResult < 0)
                return Content("Error");
            int buttonResult = SalesForceService.SetButtonsIntoSalesForce(clientCompany.ClientCompanyId, clientCompany.Dns, clientCompany.SalesForceUser, clientCompany.SalesForcePassword + clientCompany.SalesForceToken, FromDns);
            if (buttonResult < 0)
                return Content("Error");

            return Content(string.Empty);
        }
        public ActionResult InstallApplication()
        {
            string FromDns = Request.Url.Authority;
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            SalesForceService.InstallApplication(clientCompany.ClientCompanyId, clientCompany.Dns, clientCompany.SalesForceUser, clientCompany.SalesForcePassword + clientCompany.SalesForceToken, FromDns);
            return new EmptyResult();
        }
        public ActionResult DeleteApplication()
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            SalesForceService.DeleteApplication(clientCompany.ClientCompanyId, clientCompany.SalesForceUser, clientCompany.SalesForcePassword + clientCompany.SalesForceToken);
            return new EmptyResult();
        }

        public ActionResult SyncCompetitors()
        {
            ClientCompany clientCompany = ClientCompanyService.GetById(CurrentCompany);
            IList<Competitor> competitors = CompetitorService.GetAllActiveByClientCompany(CurrentCompany);

            string[] ascompetitors = (from c in competitors select c.Name).ToArray();
            string sresult = string.Empty;
            try
            {
                int result = SalesForceService.SalesforceAddCompetitors(clientCompany.SalesForceUser, clientCompany.SalesForcePassword + clientCompany.SalesForceToken, ascompetitors);

            }
            catch (Exception ex)
            {
                LOG.Error(" SyncCompetitors:" + ex.Message, ex);

            }
            return Content(sresult);
        }

        #endregion

        #region Validation Methods

        protected bool ValidateEditPasswordFormData(ClientCompany clientCompany)
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);
            ClientCompany clientCompanySession = ClientCompanyService.GetById(userProfile.ClientCompany);

            if ((!string.IsNullOrEmpty(clientCompanySession.SalesForceToken)) && (!string.IsNullOrEmpty(clientCompanySession.SalesForceUser)))
            {

                //if (!string.IsNullOrEmpty(clientCompanySession.SalesForcePassword))
                //{
                //    if (Validator.IsBlankOrNull(clientCompany.SalesForceCurrentPassword))
                //    {
                //        ValidationDictionary.AddError("SalesForceCurrentPassword", LabelResource.CompanySalesForceCurrentPasswordRequiredError);
                //    }
                //    else if (!ClientCompanyService.IsValidCurrentPassword(userProfile.ClientCompany, clientCompany.SalesForceCurrentPassword))
                //    {
                //        ValidationDictionary.AddError("SalesForceCurrentPassword", LabelResource.CompanySalesForceCurrentPasswordExistsError);
                //    }
                //}


                if (Validator.IsBlankOrNull(clientCompany.SalesForcePassword))
                {
                    ValidationDictionary.AddError("SalesForcePassword", LabelResource.CompanySalesForcePasswordRequiredError);
                }
                else if (clientCompany.SalesForcePassword.Length < 8)
                {
                    ValidationDictionary.AddError("SalesForcePassword", LabelResource.CompanySalesForcePasswordLengthError);
                }
                else if (!Validator.MatchRegexp(clientCompany.SalesForcePassword, clientCompany.SalesForceRePassword))
                {
                    ValidationDictionary.AddError("SalesForceRePassword", LabelResource.CompanySalesForceRePasswordMatchError);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(clientCompanySession.SalesForceToken))
                {
                    ValidationDictionary.AddError("SalesForceToken", LabelResource.CompanySalesForceTokenRequiredError);
                }
                if (string.IsNullOrEmpty(clientCompanySession.SalesForceUser))
                {
                    ValidationDictionary.AddError("SalesForceUser", LabelResource.CompanySalesForceUserRequiredError);
                }
            }

            return ValidationDictionary.IsValid;
        }

        protected bool ValidateEditFormData(ClientCompany clientCompany)
        {
            if (Validator.IsBlankOrNull(clientCompany.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ClientCompanyNameRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompany.Address))
            {
                ValidationDictionary.AddError("Address", LabelResource.CompanyAddressRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompany.Description))
            {
                ValidationDictionary.AddError("Description", LabelResource.CompanyDescriptionRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompany.ZipCode))
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.CompanyZipCodeRequiredError);
            }
            else if (!StringUtility.IsValidStringER(clientCompany.ZipCode, "(\\d){1,5}"))
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.ClientCompanyZipCodeMatchError);
            }
            else if (clientCompany.ZipCode.Length > 5)
            {
                ValidationDictionary.AddError("ZipCode", LabelResource.ClientCompanyZipCodeLengthError);
            }

            if (Validator.IsBlankOrNull(clientCompany.City))
            {
                ValidationDictionary.AddError("City", LabelResource.CompanyCityRequiredError);
            }

            if (Validator.IsBlankOrNull(clientCompany.State))
            {
                ValidationDictionary.AddError("State", LabelResource.CompanyStateRequiredError);
            }
            if (Validator.IsBlankOrNull(clientCompany.CountryCode))
            {
                ValidationDictionary.AddError("CountryCode", LabelResource.CompanyCountryCodeRequiredError);
            }
            if (Validator.IsBlankOrNull(clientCompany.Phone))
            {
                ValidationDictionary.AddError("Phone", LabelResource.CompanyPhoneRequiredError);
            }
            else
            {
                if (!Validator.NumberFaxAndPhone(clientCompany.Phone))
                {
                    ValidationDictionary.AddError("Phone", LabelResource.ValidateTextPhone);
                }
            }
            //if (!Validator.IsBlankOrNull(clientCompany.SalesForceUser) || !Validator.IsBlankOrNull(clientCompany.SalesForceToken))
            //{
            //    if (Validator.IsBlankOrNull(clientCompany.SalesForcePassword))
            //    {
            //        ValidationDictionary.AddError("SalesForcePassword", LabelResource.CompanySalesForcePasswordRequiredError);
            //    }
            //    else if (clientCompany.SalesForcePassword.Length < 8)
            //    {
            //        ValidationDictionary.AddError("SalesForcePassword", LabelResource.CompanySalesForcePasswordLengthError);
            //    }
            //    else if (!Validator.MatchRegexp(clientCompany.SalesForcePassword, clientCompany.SalesForceRePassword))
            //    {
            //        ValidationDictionary.AddError("SalesForceRePassword", LabelResource.CompanySalesForceRePasswordMatchError);
            //    }
            //}

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            UserProfile userProfile = UserProfileService.GetById(CurrentUser);
            ClientCompany clientCompany = ClientCompanyService.GetById(userProfile.ClientCompany);
            ViewData["CurrentPassword"] = clientCompany.SalesForcePassword;
            IList<Country> countryList = CountryService.GetAllActive();
            ViewData["CountryCodeList"] = new SelectList(countryList, "CountryCode", "Name");
        }

        protected override void SetDefaultRequestParametersToForm(OperationStatus operationStatus)
        {
            ViewData["OperationStatus"] = operationStatus;
        }

        #endregion

    }
}