using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Util;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Web;
using System.Globalization;
using Compelligence.Domain.Entity.Views;
using System.Text;

namespace Compelligence.Web.Controllers
{
    public class CompetitiveMessagingController : BackEndAsyncFormController<Positioning, decimal>
    {
        //
        // GET: /CompetitiveMessaging/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        #region Public Properties

        public IPositioningService PositioningService
        {
            get { return (IPositioningService)_genericService; }
            set { _genericService = value; }
        }

        public IIndustryService IndustryService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IPositioningIndustryService PositioningIndustryService { get; set; }

        public IPositioningCompetitorService PositioningCompetitorService { get; set; }

        #endregion

        #region Action Methods
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUpdateType(string id)
        {

            IList<Positioning> postioningList = new List<Positioning>();
            if (id.Equals(PositioningForType.Competitive))
            {
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(ResourceService.GetAll<PositioningCompetitiveType>(), "Id", "Value");
            }
            else if (id.Equals(PositioningForType.Positioning))
            {
                return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(ResourceService.GetAll<PositioningCompetitorType>(), "Id", "Value");
            }
            else
            { return null; }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUpdateParent(string id)
        {
            string ForType = Request["ForType"];
            //string DetailFilter = Request["DetailFilter"];
            string EntityType = GetDetailFilterValue("Positioning.EntityType");
            string entityid = GetDetailFilterValue("Positioning.EntityId");
            string entityTypeTempo = Request["EntityType"];
            string entityIdP = Request["EntityId"];
            string industryId = Request["IndustryId"];
            decimal? EntityId = null;
            if (!string.IsNullOrEmpty(entityid))
            {
                EntityId = decimal.Parse(entityid);
            }
            decimal? EntityIdTempo = null;
            if (!string.IsNullOrEmpty(entityIdP) && !entityIdP.Equals("null"))
            {
                EntityIdTempo = decimal.Parse(entityIdP);
            }
            IList<Positioning> postioningList = new List<Positioning>();
            if (ForType.Equals(PositioningForType.Competitive)) //ForType.Competitive
            {
                if (id.Equals(PositioningCompetitiveType.CompellingReasonNotToBuy))
                {
                    return ControllerUtility.GetSelectOptionsFromGenericList<Positioning>(postioningList, "Id", "Name");
                }
                else if (id.Equals(PositioningCompetitiveType.CompetitiveKeyMessage))
                {
                    if (EntityId != null && (EntityType != null || !string.IsNullOrEmpty(EntityType)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitiveType.CompellingReasonNotToBuy, EntityType, (decimal)EntityId, CurrentCompany);
                    }
                    else if (EntityIdTempo != null && (entityTypeTempo != null || !string.IsNullOrEmpty(entityTypeTempo)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitiveType.CompellingReasonNotToBuy, entityTypeTempo, (decimal)EntityIdTempo, CurrentCompany);
                    }
                    else if (!string.IsNullOrEmpty(industryId))
                    {
                        decimal industryTempId = decimal.Parse(industryId);
                        postioningList = PositioningService.GetByOnlyIndustryId(industryTempId, PositioningCompetitiveType.CompellingReasonNotToBuy, CurrentCompany);
                    }
                    else
                    {
                        //postioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitiveType.CompellingReasonNotToBuy, CurrentCompany);
                    }
                    return ControllerUtility.GetSelectOptionsFromGenericList<Positioning>(postioningList, "Id", "Name");
                }
                else if (id.Equals(PositioningCompetitiveType.CompetitiveProofPoint))
                {
                    if (EntityId != null && (EntityType != null || !string.IsNullOrEmpty(EntityType)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitiveType.CompetitiveKeyMessage, EntityType, (decimal)EntityId, CurrentCompany);
                    }
                    else if (EntityIdTempo != null && (entityTypeTempo != null || !string.IsNullOrEmpty(entityTypeTempo)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitiveType.CompetitiveKeyMessage, entityTypeTempo, (decimal)EntityIdTempo, CurrentCompany);
                    }
                    else if (!string.IsNullOrEmpty(industryId))
                    {
                        decimal industryTempId = decimal.Parse(industryId);
                        postioningList = PositioningService.GetByOnlyIndustryId(industryTempId, PositioningCompetitiveType.CompetitiveKeyMessage, CurrentCompany);
                    }
                    else
                    {
                        //postioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitiveType.CompetitiveKeyMessage, CurrentCompany);
                    }
                    return ControllerUtility.GetSelectOptionsFromGenericList<Positioning>(postioningList, "Id", "Name");
                }
            }
            else
            {
                if (id.Equals(PositioningCompetitorType.CompellingReasonToBuy))
                {
                    return ControllerUtility.GetSelectOptionsFromGenericList<Positioning>(postioningList, "Id", "Name");
                }
                else if (id.Equals(PositioningCompetitorType.CompetitorKeyMessage))
                {
                    if (EntityId != null && (EntityType != null || !string.IsNullOrEmpty(EntityType)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitorType.CompellingReasonToBuy, EntityType, (decimal)EntityId, CurrentCompany);
                    }
                    else if (EntityIdTempo != null && (entityTypeTempo != null || !string.IsNullOrEmpty(entityTypeTempo)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitorType.CompellingReasonToBuy, entityTypeTempo, (decimal)EntityIdTempo, CurrentCompany);
                    }
                    else if (!string.IsNullOrEmpty(industryId))
                    {
                        decimal industryTempId = decimal.Parse(industryId);
                        postioningList = PositioningService.GetByOnlyIndustryId(industryTempId, PositioningCompetitorType.CompellingReasonToBuy, CurrentCompany);
                    }
                    else
                    {
                        //postioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitorType.CompellingReasonToBuy, CurrentCompany);
                    }
                    return ControllerUtility.GetSelectOptionsFromGenericList<Positioning>(postioningList, "Id", "Name");
                }
                else if (id.Equals(PositioningCompetitorType.ProofPoint))
                {
                    if (EntityId != null && (EntityType != null || !string.IsNullOrEmpty(EntityType)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitorType.CompetitorKeyMessage, EntityType, (decimal)EntityId, CurrentCompany);
                    }
                    else if (EntityIdTempo != null && (entityTypeTempo != null || !string.IsNullOrEmpty(entityTypeTempo)))
                    {
                        postioningList = PositioningService.GetByType(PositioningCompetitorType.CompetitorKeyMessage, entityTypeTempo, (decimal)EntityIdTempo, CurrentCompany);
                    }
                    else if (!string.IsNullOrEmpty(industryId))
                    {
                        decimal industryTempId = decimal.Parse(industryId);
                        postioningList = PositioningService.GetByOnlyIndustryId(industryTempId, PositioningCompetitorType.CompetitorKeyMessage, CurrentCompany);
                    }
                    else
                    {
                        //postioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitorType.CompetitorKeyMessage, CurrentCompany);
                    }
                    return ControllerUtility.GetSelectOptionsFromGenericList<Positioning>(postioningList, "Id", "Name");
                }
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUpdateEntityType(string id)
        {
            if (id.Equals(PositioningEntityType.Competitor))
            {
                IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(CurrentCompany);
                return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
            }
            else if (id.Equals(PositioningEntityType.Product))
            {
                IList<Product> productList = ProductService.GetAllActiveByClientCompany(CurrentCompany);
                return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
            }
            else
            { return null; }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetIndustryByEntity(decimal id)
        {
            string EntityType = Request["EntityType"];
            if (!string.IsNullOrEmpty(EntityType))
            {
                IList<Industry> industryList = new List<Industry>();
                if (EntityType.Equals(PositioningEntityType.Competitor))
                {
                    industryList = IndustryService.GetByCompetitorId(id, CurrentCompany);
                    return ControllerUtility.GetSelectOptionsFromGenericList<Industry>(industryList, "Id", "Name");
                }
                else if (EntityType.Equals(PositioningEntityType.Product))
                {
                    industryList = IndustryService.GetByProductId(id);
                    return ControllerUtility.GetSelectOptionsFromGenericList<Industry>(industryList, "Id", "Name");
                }
                else
                { return null; }
            }
            else
            { return null; }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUpdatePositioningEntityType(decimal id)
        {
            IList<ResourceObject> entityTypeList = ResourceService.GetAll<PositioningEntityType>();
            return ControllerUtility.GetSelectOptionsFromGenericList<ResourceObject>(ResourceService.GetAll<PositioningEntityType>(), "Id", "Value");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetUpdatePositioningEntityId(string id)
        {
            string IndustryId = Request["IndustryId"];
            if (!string.IsNullOrEmpty(IndustryId))
            {
                decimal ifOfIndustry = decimal.Parse(IndustryId);
                if (id.Equals(PositioningEntityType.Competitor))
                {
                    IList<Competitor> competitorList = CompetitorService.GetByIndustryId(ifOfIndustry);
                    return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
                }
                else if (id.Equals(PositioningEntityType.Product))
                {
                    IList<Product> productList = ProductService.GetByIndustryId(ifOfIndustry, CurrentCompany);
                    return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
                }
                else
                {
                    IList<int> listEmpty = new List<int>();
                    return ControllerUtility.GetSelectOptionsFromGenericList<int>(listEmpty, "", "");
                }
            }
            else
            {
                if (id.Equals(PositioningEntityType.Competitor))
                {
                    IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(CurrentCompany);
                    return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
                }
                else if (id.Equals(PositioningEntityType.Product))
                {
                    IList<Product> productList = ProductService.GetAllActiveByClientCompany(CurrentCompany);
                    return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
                }
                else
                {
                    List<int> listEmpty = new List<int>();
                    return ControllerUtility.GetSelectOptionsFromGenericList<int>(listEmpty, "", "");
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult UpdateEnableStatus()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            string nameEntitiesChangeSuccessful = string.Empty;
            string nameEntitiesChangesUnsuccessful = string.Empty;
            string EntityType = GetDetailFilterValue("Positioning.EntityType");
            string entityid = GetDetailFilterValue("Positioning.EntityId");
            if (ids.Length == 1)
            {
                //ids[0] = id.ToString();
            }
            string messageFinal = string.Empty;
            string errorMessageByProduct = "";
            foreach (string identifier in ids)
            {
                //bool test = PositioningService.ExistOtherPositioningGlobal(decimal.Parse(identifier), ids, CurrentCompany);
                Positioning positioning = PositioningService.GetById(decimal.Parse(identifier));
                bool validationChange = true;
                // if is positioning global
                if (positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                {
                    //To check other positioning in the positionings selected
                    //if (ids.Length > 1)
                    //{
                    //    if (PositioningService.ExistOtherPositioningGlobal(decimal.Parse(identifier), ids, CurrentCompany))
                    //    {
                    //        // Exist other positionigs Global in the selection of items
                    //        validationChange = false;
                    //        if (string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                    //        errorMessageByProduct += "<li>To " + positioning.Name + " exist other competitive messaging global in the current selection.</li>";
                    //    }
                    //}
                    // if exist a positioning in the database
                    string otherPostioningsGlobal = PositioningService.GetOtherGlobal(decimal.Parse(identifier), PositioningRelation.CompetitiveMessaging, EntityType, decimal.Parse(entityid), CurrentCompany);
                    //if (PositioningService.ExistOtherGlobal(decimal.Parse(identifier), PositioningRelation.CompetitiveMessaging, EntityType, decimal.Parse(entityid), CurrentCompany))
                    if (!string.IsNullOrEmpty(otherPostioningsGlobal))
                    {
                        validationChange = false;
                        if (!string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                        errorMessageByProduct += "<li>" + string.Format(LabelResource.PositioningStateNotChangeSuccessfullyErrorGlobal, positioning.Name, otherPostioningsGlobal) + ".</li>";
                    }
                }
                else
                {
                    //if (ids.Length > 1)
                    //{
                    //    // if in the selection exist other positioning with the same industry
                    //    if (PositioningService.ExistOtherWithSameIndustry(decimal.Parse(identifier), ids, CurrentCompany))
                    //    {
                    //        // exist other positioning with the same industry in the selection of postionings
                    //        validationChange = false;
                    //        if (string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                    //        errorMessageByProduct += "<li>To " + positioning.Name + " exist other competitive messaging with the same industry in the selection.</li>";
                    //    }
                    //}
                    //if exist other positioning in database with the same industry
                    string postioningsWithSameIndutry = PositioningService.GetOtherPositioningWithSameIndustry(decimal.Parse(identifier), decimal.Parse(entityid), EntityType, PositioningRelation.CompetitiveMessaging, CurrentCompany);
                    bool existIndustry = false;
                    //if (PositioningService.ExistOtherPositioningWithSameIndustry(decimal.Parse(identifier), decimal.Parse(entityid), EntityType, PositioningRelation.CompetitiveMessaging, CurrentCompany))
                    if (!string.IsNullOrEmpty(postioningsWithSameIndutry))
                    {
                        existIndustry = true;
                    }

                    // For competitor
                    //if (ids.Length > 1)
                    //{
                    //    // if in the selection exist other positioning with the same industry
                    //    if (PositioningService.ExistOtherWithSameCompetitor(decimal.Parse(identifier), ids, CurrentCompany))
                    //    {
                    //        // exist other positioning with the same industry in the selection of postionings
                    //        validationChange = false;
                    //        if (string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                    //        errorMessageByProduct += "<li>To " + positioning.Name + " exist other competitive messaging with the same competitor in the selection.</li>";
                    //    }
                    //}
                    
                    //if exist other positioning in database with the same industry
                    bool existCompetitor = false;
                    string positioningWithSameCompetitor = PositioningService.GetOtherPositioningWithSameCompetitor(decimal.Parse(identifier), decimal.Parse(entityid), EntityType, PositioningRelation.CompetitiveMessaging, CurrentCompany);
                    //if (PositioningService.ExistOtherPositioningWithSameCompetitor(decimal.Parse(identifier), decimal.Parse(entityid), EntityType, PositioningRelation.CompetitiveMessaging, CurrentCompany))
                    if(!string.IsNullOrEmpty(positioningWithSameCompetitor))
                    {
                        existCompetitor = true;
                    }
                    if (existIndustry && existCompetitor)
                    {
                        if (existIndustry) {
                            validationChange = false;
                            if (string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                            errorMessageByProduct += "<li>" + string.Format(LabelResource.PositioningStateNotChangeSuccessfullyErrorIndustry, positioning.Name, postioningsWithSameIndutry) + ".</li>";
                   
                        }
                        if (existCompetitor) {
                            validationChange = false;
                            if (string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                            errorMessageByProduct += "<li>" + string.Format(LabelResource.PositioningStateNotChangeSuccessfullyErrorCompetitor, positioning.Name, positioningWithSameCompetitor) + ".</li>";
                    
                        }
                    }
                }
                
                if (!positioning.Status.Equals(PositioningStatus.Enabled) && validationChange)
                {
                    positioning.Status = PositioningStatus.Enabled;
                    SetDefaultDataForUpdate(positioning);
                    PositioningService.Update(positioning);
                    if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
                    {
                        nameEntitiesChangesUnsuccessful += ":";
                    }
                    nameEntitiesChangesUnsuccessful += positioning.Name;
                }
                else
                {
                    string message = string.Empty;
                    if (positioning.Status.Equals(PositioningStatus.Enabled))
                    {
                        message = "<li>To " + positioning.Name + ", " + LabelResource.CompetitiveMessagingStatusCurrencyValueError + " " + ResourceService.GetName<PositioningStatus>(positioning.Status) + "</li>";
                    }
                    //if (!validationChange)
                    //{
                    //    message += "<li>To " + positioning.Name + ", " + message + "</li>";
                    //}
                    if (!string.IsNullOrEmpty(message))
                    {
                        errorMessageByProduct += message;
                    }
                }
            }
            if (!string.IsNullOrEmpty(nameEntitiesChangesUnsuccessful))
            {
                string[] namesEntityUnSuccessful = nameEntitiesChangesUnsuccessful.Split(':');
                string successMessage = string.Empty;
                successMessage += "<br/>";
                successMessage += LabelResource.PositioningStateChangeSuccessfully;
                successMessage += "<p><ul class=LiEnableDisc>";
                foreach (string neus in namesEntityUnSuccessful)
                {
                    successMessage += "<li>" + neus + "</li>";
                }
                successMessage += "</ul></p>";
                messageFinal += successMessage;
            }
            if (!string.IsNullOrEmpty(errorMessageByProduct))
            {
                errorMessageByProduct = LabelResource.PositioningStateNotChangeSuccessfully +"<br/><ul class=LiEnableDisc>" + errorMessageByProduct + "</ul>";
                if (!string.IsNullOrEmpty(messageFinal))
                {
                    messageFinal += "<br/>";
                }
                messageFinal += errorMessageByProduct;

            }
            return Content(messageFinal);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult UpdateDisableStatus()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string userId = (string)Session["UserId"];
            StringBuilder returnMessage = new StringBuilder();
            StringBuilder errorMessage = new StringBuilder();
            string nameEntitiesChangeSuccessful = string.Empty;
            IList<string> namesEntitiesChangesUnsuccessful = new List<string>();
            string messageFinal = string.Empty;
            if (ids.Length == 1)
            {
                //ids[0] = id.ToString();
            }
            foreach (string identifier in ids)
            {
                Positioning positioning = PositioningService.GetById(decimal.Parse(identifier));
                if (!positioning.Status.Equals(PositioningStatus.Disabled))
                {
                    positioning.Status = PositioningStatus.Disabled;
                    SetDefaultDataForUpdate(positioning);
                    PositioningService.Update(positioning);
                    if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
                    {
                        nameEntitiesChangeSuccessful += ":";
                    }
                    nameEntitiesChangeSuccessful += positioning.Name;
                }
                else
                {
                    string message = string.Empty;
                    if (positioning.Status.Equals(PositioningStatus.Disabled))
                    {
                        message = LabelResource.PositioningCurrentStateValue + " " + ResourceService.GetName<PositioningStatus>(positioning.Status);
                    }
                    namesEntitiesChangesUnsuccessful.Add(positioning.Name + ": " + message);
                }
            }
            if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
            {
                string[] namesEntitySuccessful = nameEntitiesChangeSuccessful.Split(':');
                returnMessage.Append(LabelResource.PositioningStateChangeSuccessfully);
                returnMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntitySuccessful)
                {
                    returnMessage.Append("<li>" + neus + ".</li>");
                }
                returnMessage.Append("</ul></p>");
            }
            if (namesEntitiesChangesUnsuccessful.Count > 0)
            {
                string[] namesEntityUnSuccessful = new string[namesEntitiesChangesUnsuccessful.Count];
                namesEntitiesChangesUnsuccessful.CopyTo(namesEntityUnSuccessful, 0);
                if (returnMessage.Length > 0) errorMessage.Append("<br/>");
                errorMessage.Append(LabelResource.PositioningStateNotChangeSuccessfully);
                errorMessage.Append("<p><ul class=LiEnableDisc>");
                foreach (string neus in namesEntityUnSuccessful)
                {
                    errorMessage.Append("<li>" + neus + ".</li>");
                }
                errorMessage.Append("</ul></p>");
            }
            return Content(returnMessage.ToString() + errorMessage.ToString());

        }
        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Positioning positioning, FormCollection formCollection)
        {
            string EntityType = GetDetailFilterValue("Positioning.EntityType");
            string EntityId = GetDetailFilterValue("Positioning.EntityId");
            string selectedIndustries = formCollection["IndustryIds"];
            string selectedCompetitors = formCollection["CompetitorIds"];
            positioning.IndustriesIds = selectedIndustries;
            positioning.CompetitorsIds = selectedCompetitors;
            bool industryNoEmpty = false;
            bool competitorNoEmpty = false;
            if (Validator.IsBlankOrNull(positioning.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.PositioningNameRequiredError);
            }
            else
            {
                if (positioning.Id == 0 || positioning.Id == null)
                {
                    if (PositioningService.ExistByName(positioning.Name, PositioningRelation.CompetitiveMessaging, decimal.Parse(EntityId), EntityType, CurrentCompany))
                    {
                        ValidationDictionary.AddError("Name", LabelResource.PositioningNameMatchError);
                    }
                }
                else
                {
                    if (PositioningService.ExistByIdAndName(positioning.Id, positioning.Name, PositioningRelation.CompetitiveMessaging, decimal.Parse(EntityId), EntityType, CurrentCompany))
                    {
                        ValidationDictionary.AddError("Name", LabelResource.PositioningNameMatchError);
                    }
                }
            }
            if (Validator.IsBlankOrNull(positioning.Status))
            {
                ValidationDictionary.AddError("Status", LabelResource.PositioningStatusRequiredError);
            }
            if (!string.IsNullOrEmpty(positioning.HowTheyAttack) && positioning.HowTheyAttack.Length > 8000)
            {
                ValidationDictionary.AddError("HowTheyAttack", LabelResource.PositionigHowTheyAttackLengthError);
            }
            if (!string.IsNullOrEmpty(positioning.HowTheyPosition) && positioning.HowTheyPosition.Length > 8000)
            {
                ValidationDictionary.AddError("HowTheyPosition", LabelResource.PositionigHowTheyPositionLengthError);
            }
            if (!string.IsNullOrEmpty(positioning.HowWeAttack) && positioning.HowWeAttack.Length > 8000)
            {
                ValidationDictionary.AddError("HowWeAttack", LabelResource.PositionigHowWeAttackLengthError);
            }
            if (!string.IsNullOrEmpty(positioning.HowWeDefend) && positioning.HowWeDefend.Length > 8000)
            {
                ValidationDictionary.AddError("HowWeDefend", LabelResource.PositionigHowWeDefendLengthError);
            }
            if (!string.IsNullOrEmpty(positioning.HowWePosition) && positioning.HowWePosition.Length > 8000)
            {
                ValidationDictionary.AddError("HowWePosition", LabelResource.PositionigHowWePositionLengthError);
            }
            bool shouldBeGlobal = PositioningService.IsGlobalIndustry(PositioningRelation.CompetitiveMessaging, EntityType, decimal.Parse(EntityId), CurrentCompany);

            if (string.IsNullOrEmpty(selectedIndustries))
            {
                if (!shouldBeGlobal)
                {
                    ValidationDictionary.AddError("IndustryId", LabelResource.PositioningIndustriesRequiredError);
                    industryNoEmpty = false;
                }
            }
            else
            {
                industryNoEmpty = true;
            }
            ///Competitor
            ///
            if (string.IsNullOrEmpty(selectedCompetitors))
            {
                if (!shouldBeGlobal)
                {
                    ValidationDictionary.AddError("CompetitorId", LabelResource.CompetitiveMessagingCompetitorsRequiredError);
                    competitorNoEmpty = false;
                }
            }
            else
            {
                competitorNoEmpty = true;
                //if (!string.IsNullOrEmpty(EntityId) && !string.IsNullOrEmpty(EntityType) && !string.IsNullOrEmpty(selectedCompetitors) && (selectedCompetitors != "-1"))
                //{
                //    decimal eId = decimal.Parse(EntityId);
                //    string objectCompetitorName = string.Empty;
                //    IList<Competitor> competitorList = CompetitorService.GetByEntityIdAndCompetitorId(eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedCompetitors), CurrentCompany);
                //    if (competitorList != null && competitorList.Count > 0)
                //    {
                //        if (competitorList.Count > 1)
                //        {
                //            objectCompetitorName = "Competitors";
                //        }
                //        else
                //        {
                //            objectCompetitorName = "Competitor";
                //        }
                //        string CompetitorNames = string.Empty;
                //        for (int i = 0; i < competitorList.Count; i++)
                //        {
                //            if (!string.IsNullOrEmpty(CompetitorNames))
                //            {
                //                if (i == competitorList.Count - 1)
                //                {
                //                    CompetitorNames += " and ";
                //                }
                //                else
                //                {
                //                    CompetitorNames += ", ";
                //                }
                //            }
                //            CompetitorNames += competitorList[i].Name;
                //        }
                //        string entity = ResourceService.GetName<DomainObjectType>(EntityType);
                //        ValidationDictionary.AddError("CompetitorId", string.Format(LabelResource.PositioningIndustriesMatchError, objectCompetitorName, CompetitorNames, entity));
                //    }
                //}
            }
            if (competitorNoEmpty && industryNoEmpty)
            {
                if (!string.IsNullOrEmpty(EntityId) && !string.IsNullOrEmpty(EntityType) && !string.IsNullOrEmpty(selectedIndustries) && (selectedIndustries != "-1") && positioning.Status.Equals(PositioningStatus.Enabled))
                {
                    decimal eId = decimal.Parse(EntityId);
                    string objectIndustryName = string.Empty;
                    string objectCompetitorName = string.Empty;
                    IList<Industry> industryList = new List<Industry>();
                    IList<Competitor> competitorList = new List<Competitor>();
                    if (positioning.Id == 0 || positioning.Id == null)
                    {
                        //industryList = IndustryService.GetByEntityIdAndIndustryId(eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedIndustries), CurrentCompany);
                        industryList = IndustryService.GetByPositioningIndustriesAndCompetitors(eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedIndustries), GetListOfIds(selectedCompetitors), CurrentCompany);
                        //competitorList = CompetitorService.GetByEntityIdAndCompetitorId(eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedCompetitors), CurrentCompany);
                        competitorList = CompetitorService.GetByPositioningIndustriesAndCompetitors(eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedCompetitors), GetListOfIds(selectedIndustries), CurrentCompany);
                    }
                    else
                    {
                        //industryList = IndustryService.GetByPositioningEntityIdAndIndustryId(positioning.Id, PositioningIsGlobal.No, eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedIndustries), CurrentCompany);
                        industryList = IndustryService.GetByPositioningIdIndustriesAndCompetitor(positioning.Id, PositioningIsGlobal.No, eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedIndustries), GetListOfIds(selectedCompetitors), CurrentCompany);
                        //competitorList = CompetitorService.GetByPositioningEntityIdAndCompetitorId(positioning.Id, PositioningIsGlobal.No, eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedCompetitors), CurrentCompany);
                        competitorList = CompetitorService.GetByPositioningIdCompetitorsAndIndustries(positioning.Id, PositioningIsGlobal.No, eId, EntityType, PositioningRelation.CompetitiveMessaging, GetListOfIds(selectedCompetitors), GetListOfIds(selectedIndustries), CurrentCompany);
                    }

                    if (industryList != null && industryList.Count > 0 && competitorList != null && competitorList.Count > 0)
                    {
                        if (industryList.Count > 1)
                        {
                            objectIndustryName = "Industries";
                        }
                        else
                        {
                            objectIndustryName = "Industry";
                        }
                        string IndustryNames = string.Empty;
                        for (int i = 0; i < industryList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(IndustryNames))
                            {
                                if (i == industryList.Count - 1)
                                {
                                    IndustryNames += " and ";
                                }
                                else
                                {
                                    IndustryNames += ", ";
                                }
                            }
                            IndustryNames += industryList[i].Name;
                        }
                        string entity = ResourceService.GetName<DomainObjectType>(EntityType);
                        ValidationDictionary.AddError("IndustryId", string.Format(LabelResource.PositioningIndustriesMatchError, objectIndustryName, IndustryNames, entity));

                        //Validation to Competitor
                        if (competitorList.Count > 1)
                        {
                            objectCompetitorName = "Competitors";
                        }
                        else
                        {
                            objectCompetitorName = "Competitor";
                        }
                        string CompetitorNames = string.Empty;
                        for (int i = 0; i < competitorList.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(CompetitorNames))
                            {
                                if (i == competitorList.Count - 1)
                                {
                                    CompetitorNames += " and ";
                                }
                                else
                                {
                                    CompetitorNames += ", ";
                                }
                            }
                            CompetitorNames += competitorList[i].Name;
                        }
                        string entity2 = ResourceService.GetName<DomainObjectType>(EntityType);
                        ValidationDictionary.AddError("CompetitorId", string.Format(LabelResource.PositioningIndustriesMatchError, objectCompetitorName, CompetitorNames, entity2));
                    }
                }
            }
            else
            {
                if (!industryNoEmpty)
                { 
                    
                }
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            IList<ResourceObject> statusList = ResourceService.GetAll<PositioningStatus>();
            ViewData["ForTypeList"] = new SelectList(ResourceService.GetAll<PositioningForType>(), "Id", "Value");
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            ViewData["EntityIdList"] = new SelectList(new int[] { });
            ViewData["EntityName"] = string.Empty;
            ViewData["IndustryVisible"] = "Y";
            ViewData["CompetitorIdList"] = string.Empty;
            ViewData["IndustryIdList"] = string.Empty;
            string entityIdValue = GetDetailFilterValue("Positioning.EntityId");
            string entityTypeValue = GetDetailFilterValue("Positioning.EntityType");
            if (!string.IsNullOrEmpty(entityTypeValue) && !string.IsNullOrEmpty(entityIdValue))
            {
                IList<Competitor> competitorList = SetCompetitorListToPositioing();
                ViewData["CompetitorIdList"] = new SelectList(competitorList, "Id", "Name");
                IList<IndustryByHierarchyView> industryByHierarchyList = SetIndustryListToPositioning();
                ViewData["IndustryIdList"] = new SelectList(industryByHierarchyList, "Id", "Name");
            }
            UserProfile userLoged = UserProfileService.GetById(CurrentUser);
            ViewData["OpenedByName"] = userLoged.Name;
        }

        protected override void SetDetailFormData()
        {
            string userId = (string)Session["UserId"];
            string securityAccess = string.Empty;
            if (CurrentSecurityGroup.Equals("ANALYST"))
            {
                securityAccess = UserSecurityAccess.Edit;
                ViewData["EntityLocked"] = false;
                ViewData["UserSecurityAccess"] = securityAccess;
            }
        }

        protected override void SetUserSecurityAccess(Positioning positioning)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;
            if (CurrentSecurityGroup.Equals("ANALYST"))
            {
                securityAccess = UserSecurityAccess.Edit;
                ViewData["EntityLocked"] = false;
            }
            if (PositioningService.HasAccessToPositioning(positioning, userId) || CurrentSecurityGroup.Equals("ADMIN"))
            {
                securityAccess = UserSecurityAccess.Edit;
                ViewData["UserSecurityAccess"] = securityAccess;
            }
            else
            {
                securityAccess = UserSecurityAccess.Read;
                ViewData["EntityLocked"] = true;
            }
            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override bool ValidateDeleteData(Positioning positioning, System.Text.StringBuilder errorMessage)
        {
            bool flag = false;
            if (PositioningService.HasAccessToPositioning(positioning, CurrentUser) || CurrentSecurityGroup.Equals("ADMIN"))
            {
                flag = true;
            }
            else
            {
                //string tempo = "You dont have permisons to delete this positioning"
                if (errorMessage.Length == 0)
                {
                    errorMessage.Append("You do not have permission to delete:");
                }
                if (errorMessage.Length > 38)
                {
                    errorMessage.Append(", ");
                }
                errorMessage.Append(positioning.Name);
            }
            return flag;
        }

        private void SetIndustryAndCompetitorValues(Positioning positioning)
        {
            string entityIdValue = GetDetailFilterValue("Positioning.EntityId");
            string entityTypeValue = GetDetailFilterValue("Positioning.EntityType");
            if (!string.IsNullOrEmpty(entityTypeValue))
            {
                if (entityTypeValue.Equals(DomainObjectType.Product))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal productId = Convert.ToDecimal(entityIdValue);
                        if (positioning.Id != null && positioning.Id != 0)
                        {
                            if (string.IsNullOrEmpty(positioning.IsGlobal) || positioning.IsGlobal.Equals(PositioningIsGlobal.No))
                            positioning.IndustriesIds = GetIndustriesForPositioning(positioning.Id);
                        }
                        if (string.IsNullOrEmpty(positioning.IndustriesIds))
                        {
                            if (!string.IsNullOrEmpty(positioning.IsGlobal) && positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                            {
                                positioning.IndustriesIds = "-1";
                                positioning.CompetitorsIds = "-1";
                            }
                        }
                        positioning.OldIndustriesIds = positioning.IndustriesIds;
                        if (positioning.Id != null && positioning.Id != 0)
                        {
                            if (string.IsNullOrEmpty(positioning.IsGlobal) || positioning.IsGlobal.Equals(PositioningIsGlobal.No))
                            positioning.CompetitorsIds = GetCompetitorsForPositioning(positioning.Id);
                        }
                        positioning.OldCompetitorsIds = positioning.CompetitorsIds;
                        //Industries
                        string[] selectedValues = positioning.IndustriesIds.Split(',');
                        var selected = selectedValues;
                        //IList<IndustryByHierarchyView> industryByHierarchyList = IndustryService.GetIndustryHierarchyByProduct(productId, CurrentCompany);
                        //IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        //industryAll.Name = "Industry All(default)";
                        //industryAll.Id = -1;
                        //industryByHierarchyList.Insert(0, industryAll);
                        //List empty
                        IList<IndustryByHierarchyView> industryByHierarchyList = new List<IndustryByHierarchyView>();
                        // By Default
                        IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        industryAll.Name = "Industry All(default)";
                        industryAll.Id = -1;
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, entityTypeValue, productId, CurrentCompany))
                        {
                            industryByHierarchyList = IndustryService.GetIndustryHierarchyByProduct(productId, CurrentCompany);
                            if (PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, entityTypeValue, productId, CurrentCompany))
                            {
                                if (positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                                {
                                    industryByHierarchyList.Insert(0, industryAll);
                                }
                            }
                            else
                            {
                                industryByHierarchyList.Insert(0, industryAll);
                            }
                        }
                        else
                        {
                            industryByHierarchyList.Insert(0, industryAll);
                        }
                        ViewData["IndustryIdList"] = new MultiSelectList(industryByHierarchyList, "Id", "Name", selected);
                        //Competitors
                        string[] selectedCValues = positioning.CompetitorsIds.Split(',');
                        var selectedC = selectedCValues;
                        IList<Competitor> competitorList = new List<Competitor>();
                        Competitor competitorAll = new Competitor();
                        competitorAll.Id = -1;
                        competitorAll.Name = "Competitor All(Default)";
                        //IList<Competitor> competitorList = CompetitorService.GetAllIndutryAndProduct(productId, CurrentCompany);
                        //competitorList.Insert(0, competitorAll);

                        if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, entityTypeValue, productId, CurrentCompany))
                        {
                            competitorList = CompetitorService.GetAllIndutryAndProduct(productId, CurrentCompany);
                            // industryByHierarchyList = IndustryService.GetIndustryHierarchyByCompetitor(productId, CurrentCompany);
                            if (PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, entityTypeValue, productId, CurrentCompany))
                            {
                                if (positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                                {
                                    competitorList.Insert(0, competitorAll);
                                }
                            }
                            else
                            {
                                competitorList.Insert(0, competitorAll);
                            }
                        }
                        else
                        {
                            competitorList.Insert(0, competitorAll);
                        }

                        ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedC);
                    }
                }
                else if (entityTypeValue.Equals(DomainObjectType.Competitor))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal competitorId = Convert.ToDecimal(entityIdValue);
                        if (positioning.Id != null && positioning.Id != 0)
                        {
                            if (string.IsNullOrEmpty(positioning.IsGlobal) || positioning.IsGlobal.Equals(PositioningIsGlobal.No))
                            positioning.IndustriesIds = GetIndustriesForPositioning(positioning.Id);
                        }
                        if (string.IsNullOrEmpty(positioning.IndustriesIds))
                        {
                            if (!string.IsNullOrEmpty(positioning.IsGlobal) && positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                            {
                                positioning.IndustriesIds = "-1";
                                positioning.CompetitorsIds = "-1";
                            }
                        }
                        positioning.OldIndustriesIds = positioning.IndustriesIds;
                        if (positioning.Id != null && positioning.Id != 0)
                        {
                            if (string.IsNullOrEmpty(positioning.IsGlobal) || positioning.IsGlobal.Equals(PositioningIsGlobal.No))
                            positioning.CompetitorsIds = GetCompetitorsForPositioning(positioning.Id);
                        }
                        positioning.OldCompetitorsIds = positioning.CompetitorsIds;
                        //Industries
                        string[] selectedValues = positioning.IndustriesIds.Split(',');
                        var selected = selectedValues;
                        //IList<IndustryByHierarchyView> industryByHierarchyList = IndustryService.GetIndustryHierarchyByCompetitor(competitorId, CurrentCompany);
                        //IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        //industryAll.Name = "Industry All(default)";
                        //industryAll.Id = -1;
                        //industryByHierarchyList.Insert(0, industryAll);
                        //List empty
                        IList<IndustryByHierarchyView> industryByHierarchyList = new List<IndustryByHierarchyView>();
                        // By Default
                        IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        industryAll.Name = "Industry All(default)";
                        industryAll.Id = -1;
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, entityTypeValue, competitorId, CurrentCompany))
                        {
                            industryByHierarchyList = IndustryService.GetIndustryHierarchyByCompetitor(competitorId, CurrentCompany);
                            if (PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, entityTypeValue, competitorId, CurrentCompany))
                            {
                                if (positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                                {
                                    industryByHierarchyList.Insert(0, industryAll);
                                }
                            }
                            else
                            {
                                industryByHierarchyList.Insert(0, industryAll);
                            }
                        }
                        else
                        {
                            industryByHierarchyList.Insert(0, industryAll);
                        }
                        ViewData["IndustryIdList"] = new MultiSelectList(industryByHierarchyList, "Id", "Name", selected);
                        //Competitors
                        string[] selectedCValues = positioning.CompetitorsIds.Split(',');
                        var selectedC = selectedCValues;
                        IList<Competitor> competitorList = new List<Competitor>();
                        Competitor competitorAll = new Competitor();
                        competitorAll.Id = -1;
                        competitorAll.Name = "Competitor All(Default)";
                        // IList<Competitor> competitorList = CompetitorService.GetAllIndutryAndCompetitorId(competitorId, CurrentCompany);
                        // competitorList.Insert(0, competitorAll);

                        if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, entityTypeValue, competitorId, CurrentCompany))
                        {
                            competitorList = CompetitorService.GetAllIndutryAndCompetitorId(competitorId, CurrentCompany);
                            if (PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, entityTypeValue, competitorId, CurrentCompany))
                            {
                                if (positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                                {
                                    competitorList.Insert(0, competitorAll);
                                }
                            }
                            else
                            {
                                competitorList.Insert(0, competitorAll);
                            }
                        }
                        else
                        {
                            competitorList.Insert(0, competitorAll);
                        }

                        ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedC);
                    }
                }
            }
        }

        protected override void SetEntityDataToForm(Positioning positioning)
        {
            positioning.OldIsMaster = positioning.IsMaster;
            //SetCascadingData(positioning);
            //SetDataForMasterPositioning(positioning);
           // SetCascadingEntityData(positioning);
            SetIndustryAndCompetitorValues(positioning);
            UserProfile createdBy = UserProfileService.GetById(positioning.CreatedBy);
            if (createdBy != null)
            {
                positioning.CreatedByName = createdBy.Name;
                ViewData["OpenedByName"] = createdBy.Name;
            }
        }

        protected override void SetFormEntityDataToForm(Positioning positioning)
        {
            SetIndustryAndCompetitorValues(positioning);
            //SetCascadingData(positioning);

            if (!IsSubTabPositioning())
            {
                SetCascadingEntityData(positioning);
            }
            positioning.OldIsMaster = positioning.IsMaster;
            ModelState.SetModelValue("OldIsMaster", new ValueProviderResult(positioning.OldIsMaster, positioning.OldIsMaster, CultureInfo.InvariantCulture));
            positioning.OldIndustriesIds = positioning.IndustriesIds;
            positioning.OldCompetitorsIds = positioning.CompetitorsIds;
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(positioning.OldIndustriesIds, positioning.OldIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorsIds", new ValueProviderResult(positioning.OldCompetitorsIds, positioning.OldCompetitorsIds, CultureInfo.InvariantCulture));
        }

        protected override void SetFormDataToEntity(Positioning entity, FormCollection collection)
        {
            string ismastertempo = collection["IsMasterPositioning"];
            if (!string.IsNullOrEmpty(ismastertempo))
            {
                string[] ismastervalues = ismastertempo.Split(',');
                if (ismastervalues[0].Equals("true"))
                {
                    entity.IsMaster = "Y";
                    entity.IsMasterPositioning = true;
                }
                else
                {
                    entity.IsMaster = "N";
                    entity.IsMasterPositioning = false;
                }
            }

            base.SetFormDataToEntity<Positioning>(entity, collection);
        }
        protected override void SetDefaultEntityDataForSave(Positioning positioning)
        {
            if (positioning.IsMasterPositioning)
            {
                positioning.IsMaster = "Y";
            }
            else
            {
                positioning.IsMaster = "N";
            }
            positioning.PositioningRelation = PositioningRelation.CompetitiveMessaging;
            positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
        }

        protected override void GetFormData(Positioning positioning, FormCollection collection)
        {
            if (positioning.IsMasterPositioning)
            {
                positioning.IsMaster = "Y";
            }
            else
            {
                positioning.IsMaster = "N";
            }
            string selectedIndustries = collection["IndustryIds"];
            if (!string.IsNullOrEmpty(selectedIndustries))
            {
                if (selectedIndustries.IndexOf("-1") == -1)
                {
                    positioning.IndustriesIds = selectedIndustries;
                    positioning.IsGlobal = PositioningIsGlobal.No;
                }
                else
                {
                    positioning.IsGlobal = PositioningIsGlobal.Yes;
                }
            }
            string selectedCompetitors = collection["CompetitorIds"];
            if (!string.IsNullOrEmpty(selectedCompetitors))
            {
                positioning.CompetitorsIds = selectedCompetitors;
            }
            
            string entityTypeCollection = collection["EntityType"];
            string entityIdColleccion = collection["EntityId"];
            if (!string.IsNullOrEmpty(entityTypeCollection) && !string.IsNullOrEmpty(entityIdColleccion))
            {
                positioning.EntityId = decimal.Parse(entityIdColleccion);
                positioning.EntityType = entityTypeCollection;
            }
            if (string.IsNullOrEmpty(selectedIndustries) && string.IsNullOrEmpty(selectedCompetitors))
            {
                if (PositioningService.IsGlobalIndustry(PositioningRelation.CompetitiveMessaging, positioning.EntityType, (decimal)positioning.EntityId, CurrentCompany))
                {
                    positioning.IsGlobal = PositioningIsGlobal.Yes;
                }
            }
        }
        #endregion

        #region Private Methods

        //Update Positioning.Type 
        private void SetCascadingData(Positioning positioning)
        {
            if (!String.IsNullOrEmpty(positioning.ForType))
            {
                IList<Positioning> positioningList = new List<Positioning>();
                if (positioning.ForType.Equals(PositioningForType.Competitive))
                {
                    ViewData["TypeList"] = new SelectList(ResourceService.GetAll<PositioningCompetitiveType>(), "Id", "Value");
                }
                else if (positioning.ForType.Equals(PositioningForType.Positioning))
                {
                    ViewData["TypeList"] = new SelectList(ResourceService.GetAll<PositioningCompetitorType>(), "Id", "Value");
                }
                if (!String.IsNullOrEmpty(positioning.ForType) && !String.IsNullOrEmpty(positioning.Type))
                {

                    /*if (positioning.Type == PositioningType.ProofPoint)
                    {
                        positioningList = PositioningService.GetAllByType(PositioningType.CompetitiveKeyMessage, positioning.ClientCompany);
                    }
                    else if (positioning.Type == PositioningType.CompetitiveKeyMessage)
                    {
                        positioningList = PositioningService.GetAllByType(PositioningType.CompellingReasonNotToBuy, positioning.ClientCompany);
                    }
                     */
                    if (positioning.ForType.Equals(PositioningForType.Competitive)) //ForType.Competitive
                    {
                        if (positioning.Type.Equals(PositioningCompetitiveType.CompetitiveProofPoint))
                        {
                            if (positioning.EntityId != null && !string.IsNullOrEmpty(positioning.EntityType))
                            {
                                positioningList = PositioningService.GetByType(PositioningCompetitiveType.CompetitiveKeyMessage, positioning.EntityType, (decimal)positioning.EntityId, CurrentCompany);
                            }
                            else
                            {
                                positioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitiveType.CompetitiveKeyMessage, CurrentCompany);
                            }
                        }
                        else if (positioning.Type.Equals(PositioningCompetitiveType.CompetitiveKeyMessage))
                        {
                            if (positioning.EntityId != null && !string.IsNullOrEmpty(positioning.EntityType))
                            {
                                positioningList = PositioningService.GetByType(PositioningCompetitiveType.CompellingReasonNotToBuy, positioning.EntityType, (decimal)positioning.EntityId, CurrentCompany);
                            }
                            else
                            {
                                positioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitiveType.CompellingReasonNotToBuy, CurrentCompany);
                            }
                        }
                    }
                    else
                    {
                        if (positioning.Type.Equals(PositioningCompetitorType.ProofPoint))
                        {
                            if (positioning.EntityId != null && !string.IsNullOrEmpty(positioning.EntityType))
                            {
                                positioningList = PositioningService.GetByType(PositioningCompetitorType.CompetitorKeyMessage, positioning.EntityType, (decimal)positioning.EntityId, CurrentCompany);
                            }
                            else
                            {
                                positioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitorType.CompetitorKeyMessage, CurrentCompany);
                            }
                        }
                        else if (positioning.Type.Equals(PositioningCompetitorType.CompetitorKeyMessage))
                        {
                            if (positioning.EntityId != null && !string.IsNullOrEmpty(positioning.EntityType))
                            {
                                positioningList = PositioningService.GetByType(PositioningCompetitorType.CompellingReasonToBuy, positioning.EntityType, (decimal)positioning.EntityId, CurrentCompany);
                            }
                            else
                            {
                                positioningList = PositioningService.GetByTypeAndCompany(PositioningCompetitorType.CompellingReasonToBuy, CurrentCompany);
                            }
                        }
                    }
                    ViewData["PositioningParentList"] = new SelectList(positioningList, "Id", "Name");
                }
            }
        }


        private void SetCascadingEntityData(Positioning positioning)
        {
            if (positioning.IndustryId != null)
            {
                if (positioning.EntityType.Equals(PositioningEntityType.Competitor))
                {
                    IList<Competitor> competitorList = CompetitorService.GetByIndustryId((decimal)positioning.IndustryId);
                    if (positioning.EntityId != null)
                    {
                        ViewData["EntityIdList"] = new SelectList(competitorList, "Id", "Name", positioning.EntityId);
                    }
                    else
                    {
                        ViewData["EntityIdList"] = new SelectList(competitorList, "Id", "Name");
                    }
                }
                else if (positioning.EntityType.Equals(PositioningEntityType.Product))
                {
                    IList<Product> productList = ProductService.GetByIndustryId((decimal)positioning.IndustryId, CurrentCompany);
                    if (positioning.EntityId != null)
                    {
                        ViewData["EntityIdList"] = new SelectList(productList, "Id", "Name", positioning.EntityId);
                    }
                    else
                    {
                        ViewData["EntityIdList"] = new SelectList(productList, "Id", "Name");
                    }
                }
                else
                {
                    IList<int> listEmpty = new List<int>();
                    ViewData["EntityIdList"] = new SelectList(listEmpty, "", "");
                }
            }
            else
            {
                if (positioning.EntityType.Equals(PositioningEntityType.Competitor))
                {
                    IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(CurrentCompany);
                    ViewData["EntityIdList"] = new SelectList(competitorList, "Id", "Name");
                }
                else if (positioning.EntityType.Equals(PositioningEntityType.Product))
                {
                    IList<Product> productList = ProductService.GetAllActiveByClientCompany(CurrentCompany);
                    ViewData["EntityIdList"] = new SelectList(productList, "Id", "Name");
                }
                else
                {
                    List<int> listEmpty = new List<int>();
                    ViewData["EntityIdList"] = new SelectList(listEmpty, "", "");
                }
            }
        }

        private bool IsSubTabPositioning()
        {
            bool result = false;
            string EntityType = GetDetailFilterValue("Positioning.EntityType");
            string EntityId = GetDetailFilterValue("Positioning.EntityId");
            if (!string.IsNullOrEmpty(EntityType) && !string.IsNullOrEmpty(EntityId))
            {
                result = true;
            }
            return result;
        }

        private void SetDataForMasterPositioning(Positioning positioning)
        {
            if (string.IsNullOrEmpty(positioning.IsMaster))
            {
                positioning.IsMasterPositioning = false;
            }
            else
            {
                if (positioning.IsMaster.Equals("Y"))
                {
                    positioning.IsMasterPositioning = true;
                    ViewData["IndustryVisible"] = "N";
                }
                else if (positioning.IsMaster.Equals("N"))
                {
                    positioning.IsMasterPositioning = false;
                    ViewData["IndustryVisible"] = "Y";
                }
            }
        }
        #endregion

        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetPositioningByCompetitor()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string detailTypeParam = Request["DetailCreateType"];
            string id = ids[0];
            return JavaScript("$(#ToolsPositioningAllListTable).setGridParam({ url: /Browse.aspx/GetData?bid=PositioningAll&eou&filterCriteria=PositioningAllView.EntityId_Eq_+" + id + ":PositioningAllView.EntityType_Eq_COMPT, page: 1 }).trigger('reloadGrid');alert(" + id + ")");
        }

        private string GetIndustriesForPositioning(decimal idPositioning)
        {
            string ids = null;
            IList<PositioningIndustry> lstPosiInd = PositioningIndustryService.GetByPositioningId(idPositioning, CurrentCompany);
            int cont = 0;
            foreach (PositioningIndustry posiInd in lstPosiInd)
            {
                cont++;

                if (lstPosiInd.Count == cont)
                {
                    ids = ids + posiInd.Id.IndustryId.ToString();
                }
                else
                {
                    ids = ids + posiInd.Id.IndustryId + ",";
                }

            }
            return ids;
        }

        private string GetCompetitorsForPositioning(decimal idPositioning)
        {
            string ids = null;
            IList<PositioningCompetitor> lstPosiInd = PositioningCompetitorService.GetByPositioningId(idPositioning, CurrentCompany);
            int cont = 0;
            foreach (PositioningCompetitor posiInd in lstPosiInd)
            {
                cont++;

                if (lstPosiInd.Count == cont)
                {
                    ids = ids + posiInd.Id.CompetitorId.ToString();
                }
                else
                {
                    ids = ids + posiInd.Id.CompetitorId + ",";
                }

            }
            return ids;
        }

        private IList<IndustryByHierarchyView> SetIndustryListToPositioning()
        {
            IList<IndustryByHierarchyView> result = new List<IndustryByHierarchyView>();
            string entityIdValue = GetDetailFilterValue("Positioning.EntityId");
            string entityTypeValue = GetDetailFilterValue("Positioning.EntityType");
            if (!string.IsNullOrEmpty(entityTypeValue))
            {
                if (entityTypeValue.Equals(DomainObjectType.Product))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal productId = Convert.ToDecimal(entityIdValue);
                        IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        industryAll.Name = "Industry All(default)";
                        industryAll.Id = -1;
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, entityTypeValue, productId, CurrentCompany))
                        {
                            result = IndustryService.GetIndustryHierarchyByProduct(productId, CurrentCompany);
                            if (!PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, DomainObjectType.Product, productId, CurrentCompany))
                            {
                                result.Insert(0, industryAll);
                            }
                        }
                        else
                        {
                            result.Insert(0, industryAll);
                        }
                    }
                }
                else if (entityTypeValue.Equals(DomainObjectType.Competitor))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal competitorId = Convert.ToDecimal(entityIdValue);
                        IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        industryAll.Name = "Industry All(default)";
                        industryAll.Id = -1;
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, entityTypeValue, competitorId, CurrentCompany))
                        {
                            result = IndustryService.GetIndustryHierarchyByCompetitor(competitorId, CurrentCompany);
                            if (!PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, entityTypeValue, competitorId, CurrentCompany))
                            {
                                result.Insert(0, industryAll);
                            }
                        }
                        else
                        {
                            result.Insert(0, industryAll);
                        }
                    }
                }
            }
            return result;
        }

        private IList<Competitor> SetCompetitorListToPositioing()
        {
            IList<Competitor> result = new List<Competitor>();
            string entityId = GetDetailFilterValue("Positioning.EntityId");
            string entityType = GetDetailFilterValue("Positioning.EntityType");
            if (!string.IsNullOrEmpty(entityType) && !string.IsNullOrEmpty(entityId))
            {
                if (entityType.Equals(DomainObjectType.Product))
                {
                    decimal productId = Convert.ToDecimal(entityId);
                    Competitor competitorAll = new Competitor();
                    competitorAll.Name = "Competitor All(default)";
                    competitorAll.Id = -1;

                    if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, DomainObjectType.Product, productId, CurrentCompany))
                    {
                        result = CompetitorService.GetAllIndutryAndProduct(productId, CurrentCompany);
                        if (!PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, DomainObjectType.Product, productId, CurrentCompany))
                        {
                            result.Insert(0, competitorAll);
                        }
                    }
                    else
                    {
                        result.Insert(0, competitorAll);
                    }
                }
                else if (entityType.Equals(DomainObjectType.Competitor))
                {
                    decimal competitorId = Convert.ToDecimal(entityId);
                    Competitor competitorAll = new Competitor();
                    competitorAll.Name = "Competitor All(default)";
                    competitorAll.Id = -1;
                    if (PositioningService.HavePositioningsEntity(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, competitorId, CurrentCompany))
                    {
                        result = CompetitorService.GetAllIndutryAndCompetitorId(competitorId, CurrentCompany);
                        if (!PositioningService.ExistGlobal(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, competitorId, CurrentCompany))
                        {
                            result.Insert(0, competitorAll);
                        }
                    }
                    else
                    {
                        result.Insert(0, competitorAll);
                    }
                }
            }
            return result;
        }

        private IList<decimal> GetListOfIds(string industriesIds)
        {
            IList<decimal> listOfIds = new List<decimal>();
            if (!string.IsNullOrEmpty(industriesIds))
            {
                string[] ids = industriesIds.Split(',');
                if (ids.Length > 0)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                        {
                            decimal id = decimal.Parse(ids[i]);
                            listOfIds.Add(id);
                        }
                    }
                }
            }
            return listOfIds;
        }
    }
}
