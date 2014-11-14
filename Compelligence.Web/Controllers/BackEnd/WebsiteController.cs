using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Common.Utility;

namespace Compelligence.Web.Controllers
{
    public class WebsiteController : BackEndAsyncFormController<Website, decimal>
    {

        #region Public Properties

        public IWebsiteService WebsiteService
        {
            get { return (IWebsiteService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IWebsiteDetailService WebsiteDetailService { get; set; }

        public IContentTypeService ContentTypeService { get; set; }

        #endregion

        #region Action Methods

        public ActionResult EditDetail()
        {
            SetCMSDetail();
            return View("EditDetail");
        }

        public ActionResult SaveDetail(WebsiteDetail entity, FormCollection collection)
        {

            string[] RequestCmsConfigId = collection["CMSConfigId"].Split(',');
            string[] RequestLevel = collection["Level"].Split(',');
            string[] RequestContentTypeId = collection["ContentTypeId"].Split(',');
            string[] RequestDisplayable = collection["Displayable"].Split(',');
            string[] RequestAjust = collection["Ajust"].Split(',');

            //First Remove old Configure
            decimal WebsiteId = Convert.ToDecimal(RequestCmsConfigId[0]); //same for all
            WebsiteDetailService.DropByWebsiteId(WebsiteId);


            List<WebsiteDetail> WebsiteDetailsToSave = new List<WebsiteDetail>();

            for (int i = 0; i < RequestCmsConfigId.Length; i++)
            {
                UniqueKeyGenerator key = UniqueKeyGenerator.GetInstance();

                WebsiteDetail row = new WebsiteDetail();
                row.Id = (decimal)key.GetUniqueKey();
                row.WebsiteId = WebsiteId;
                row.ContentTypeId = Convert.ToDecimal(RequestContentTypeId[i]);
                row.Level = Convert.ToDecimal(RequestLevel[i]);
                row.Sequence = i; //Change with Group By ContentType.Size
                row.Displayable = RequestDisplayable[i];
                row.Ajust = RequestAjust[i];
                SetDefaultDataForSave(row);
                WebsiteDetailsToSave.Add(row);
            }
            WebsiteDetailService.SaveDetails(WebsiteDetailsToSave);

            return null;

        }

        public ActionResult EditPreview()
        {
            SetCMSDetail();
            return View("EditPreview");
        }

        public ActionResult EditPanel(decimal websiteid)
        {
            SetWebsitePanel(websiteid);
            return View("EditPanel");
        }
        public ActionResult SavePanel(FormCollection collection)
        {

            string[] RequestWebsiteId = collection["WebsiteId"].Split(',');
            string[] RequestLevel = collection["Level"].Split(',');
            string[] RequestComponentType = collection["ComponentType"].Split(',');
            string[] RequestComponentName = collection["ComponentName"].Split(',');
            string[] RequestDisplayable = collection["Displayable"].Split(',');
            

            //First Remove old Configure
            decimal WebsiteId = Convert.ToDecimal(RequestWebsiteId[0]); //same for all
            //WebsiteService.DropPanels(WebsiteId);
            WebsiteService.DropPanelsToDisplay(WebsiteId);

            List<WebsitePanel> websitepanels = new List<WebsitePanel>();

            for (int i = 0; i < RequestWebsiteId.Length; i++)
            {
                UniqueKeyGenerator key = UniqueKeyGenerator.GetInstance();

                WebsitePanel row = new WebsitePanel();
                row.Id = (decimal)key.GetUniqueKey();
                row.WebsiteId = WebsiteId;
                row.ComponentType = RequestComponentType[i];
                row.ComponentName = RequestComponentName[i];
                row.Level = Convert.ToDecimal(RequestLevel[i]);
                row.Sequence = i; //Change with Group By ContentType.Size
                row.Displayable = RequestDisplayable[i];
                SetDefaultDataForSave(row);
                websitepanels.Add(row);
            }
            WebsiteService.SavePanels(websitepanels);
            return null;
        }
        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Website website, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(website.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.WebsiteNameRequiredError);
            }

            if (Validator.IsBlankOrNull(website.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.WebsiteAssignedToRequiredError);
            }

            if (Validator.IsBlankOrNull(website.Status))
            {
                ValidationDictionary.AddError("Status", LabelResource.WebsiteStatusRequiredError);
            }
            else
                if (website.Status == WebsiteStatus.Enabled && WebsiteDetailService.GetByWebsiteId(website.Id).Count <= 0)
                {
                    ValidationDictionary.AddError("Status", LabelResource.WebsiteStatusValueError);
                }

            if (!(Validator.IsBlankOrNull(website.DateFrm) || Validator.IsDate(website.DateFrm, GetFormatDate())))
            {
                //Date should be in the format {0} //ProjectDueDateFormatError
                ValidationDictionary.AddError("DateFrm", string.Format(LabelResource.WebsiteDateFormatError, GetFormatDate()));
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> websiteStatusList = ResourceService.GetAll<WebsiteStatus>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);

            ViewData["StatusList"] = new SelectList(websiteStatusList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(Website website)
        {
            Session["CMSConfigId"] = website.Id;
            ViewData["DateFrm"] = DateTimeUtility.ConvertToString(website.Date, GetFormatDate());
        }

        protected override void GetFormData(Website website, FormCollection collection)
        {
            website.Date = DateTimeUtility.ConvertToDate(collection["DateFrm"], GetFormatDate());
            if (website.Status.Equals(WebsiteDetailStatus.Enabled))
            {
                //Verify other
                IList<Website> websites = WebsiteService.GetAllActiveByClientCompany(CurrentCompany);
                foreach (Website ws in websites)
                {
                    if ((ws.Status.Equals(WebsiteDetailStatus.Enabled)) && (ws.Id != website.Id))
                    {
                        ws.Status = WebsiteDetailStatus.Disabled;
                        WebsiteService.Update(ws);
                    }
                    
                }
            }
        }

        protected override void SetUserSecurityAccess(Website website)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (WebsiteService.HasAccessToWebsite(website, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion

        #region Private Methods

        private void SetCMSDetail()
        {
            decimal websiteId = Convert.ToDecimal(Session["CMSConfigId"].ToString());
            Website website = WebsiteService.GetById(websiteId);
            string userId = (string)Session["UserId"];
            IList<ContentType> ContentTypes = ContentTypeService.GetAllByClientCompany(website.ClientCompany);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Positioning Statements", CurrentCompany, userId);
            ContentTypeService.CreateIfNotExist(ContentTypes, "News", CurrentCompany, userId);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Details Industry Competitor Product", CurrentCompany, userId);
            //Disable Strengths/Weaknesses
            ContentTypeService.CreateIfNotExist(ContentTypes, "Strengths/Weaknesses", CurrentCompany, userId);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitive Messaging", CurrentCompany, userId);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Competitors in Industry", CurrentCompany, userId);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Products in Industry", CurrentCompany, userId);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Industries", CurrentCompany, userId);
            for (int i = 0; i < 4; i++)
            {
                IList<WebsiteDetail> details = new List<WebsiteDetail>();
                foreach (ContentType contenttype in ContentTypes)
                {
                    WebsiteDetail websitedetail = WebsiteDetailService.GetByContentType(websiteId, i, contenttype.Id);
                    details.Add(websitedetail);
                }
                var detailsOrder = (from d in details orderby d.Sequence select d);
                ViewData["CMSDetails" + i] = detailsOrder.ToList<WebsiteDetail>();
            }
        }

        private void SetWebsitePanel(decimal websiteid)
        {
            IList<WebsitePanel> websitepanels = WebsiteService.GetPanels(websiteid);
            if (websitepanels == null || (websitepanels!=null && websitepanels.Count < 5)) //not define
            {
                WebsiteService.CreateDefaultPanels(WebsiteService.GetById(websiteid));
                websitepanels = WebsiteService.GetPanels(websiteid);
            }
            ViewData["WebsitePanels"] = websitepanels ?? new List<WebsitePanel>();
        }
        #endregion

    }
}
