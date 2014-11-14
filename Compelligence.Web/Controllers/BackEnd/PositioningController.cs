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
    public class PositioningController : BackEndAsyncFormController<Positioning, decimal>
    {

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

        public IClientCompanyService ClientCompanyService { get; set; }

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
                    //        errorMessageByProduct += "<li>" + positioning.Name + " exist other statement global in the current selection:" + LabelResource.PositioningCurrentStateValue + " " + ResourceService.GetName<PositioningStatus>(positioning.Status) + ".</li>";
                    //        errorMessageByProduct += "<li>" + positioning.Name + " could not be changed to Enabled." + "" + "enabled as a global positioning statement. Only one global level statement may be enabled at a time.</li>";
                    //    }
                    //}
                    // if exist a positioning in the database
                    string otherPostioningsGlobal = PositioningService.GetOtherGlobal(decimal.Parse(identifier), PositioningRelation.Positioning, EntityType, decimal.Parse(entityid), CurrentCompany);
                    //if (PositioningService.ExistOtherGlobal(decimal.Parse(identifier), PositioningRelation.Positioning, EntityType, decimal.Parse(entityid), CurrentCompany))
                    if (!string.IsNullOrEmpty(otherPostioningsGlobal))
                    {
                        validationChange = false;
                        if (string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                        errorMessageByProduct += "<li>" + string.Format(LabelResource.PositioningStateNotChangeSuccessfullyErrorGlobal, positioning.Name,otherPostioningsGlobal ) + ".</li>";
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
                    //        errorMessageByProduct += "<li>To " + positioning.Name + " exist other statement with the same industry in the selection.</li>";
                    //    }
                    //}
                    //if exist other positioning in database with the same industry
                    string postioningsWithSameIndutry = PositioningService.GetOtherPositioningWithSameIndustry(decimal.Parse(identifier), decimal.Parse(entityid), EntityType, PositioningRelation.Positioning, CurrentCompany);
                    //if (PositioningService.ExistOtherPositioningWithSameIndustry(decimal.Parse(identifier), decimal.Parse(entityid), EntityType, PositioningRelation.Positioning, CurrentCompany))
                    if(!string.IsNullOrEmpty(postioningsWithSameIndutry))
                    {
                        validationChange = false;
                        if (string.IsNullOrEmpty(errorMessageByProduct)) { errorMessageByProduct = "<ul class=LiEnableDisc>"; }
                        errorMessageByProduct += "<li> " + string.Format(LabelResource.PositioningStateNotChangeSuccessfullyErrorIndustry, positioning.Name, postioningsWithSameIndutry) + ".</li>";
                        //errorMessageByProduct += "<li>To " + positioning.Name + " exist other statement with the same industry in status enabled for this entity.</li>";
                    }
                }
                if (!positioning.Status.Equals(PositioningStatus.Enabled) && validationChange)
                {
                    positioning.Status = PositioningStatus.Enabled;
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
                    if (positioning.Status.Equals(PositioningStatus.Enabled))
                    {
                        message = "<li>" + positioning.Name + ": " + LabelResource.PositioningCurrentStateValue + " " + ResourceService.GetName<PositioningStatus>(positioning.Status) + ".</li>";
                    }
                    if (!string.IsNullOrEmpty(message))
                    {
                        errorMessageByProduct += message;
                    }
                }
            }
            if (!string.IsNullOrEmpty(nameEntitiesChangeSuccessful))
            {
                string[] namesEntitySuccessful = nameEntitiesChangeSuccessful.Split(':');
                string successMessage = string.Empty;
                successMessage += "<br/>";
                successMessage += LabelResource.PositioningStateChangeSuccessfully;
                successMessage += "<p><ul class=LiEnableDisc>";
                foreach (string neus in namesEntitySuccessful)
                {
                    successMessage += "<li>" + neus + ".</li>";
                }
                successMessage += "</ul></p>";
                messageFinal += successMessage;
            }
            if (!string.IsNullOrEmpty(errorMessageByProduct))
            {
                errorMessageByProduct = LabelResource.PositioningStateNotChangeSuccessfully + "<br/><ul class=LiEnableDisc>" + errorMessageByProduct + "</ul>";
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
            if (namesEntitiesChangesUnsuccessful.Count>0)
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
            positioning.IndustriesIds = selectedIndustries;
            if (Validator.IsBlankOrNull(positioning.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.PositioningNameRequiredError);
            }
            else
            {
                if (positioning.Id == 0 || positioning.Id == null)
                {
                    if (PositioningService.ExistByName(positioning.Name, PositioningRelation.Positioning, decimal.Parse(EntityId), EntityType, CurrentCompany))
                    {
                        ValidationDictionary.AddError("Name", LabelResource.PositioningNameMatchError);
                    }
                }
                else
                {
                    if (PositioningService.ExistByIdAndName(positioning.Id, positioning.Name, PositioningRelation.Positioning, decimal.Parse(EntityId), EntityType, CurrentCompany))
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
            if (!PositioningService.IsGlobalIndustry(PositioningRelation.Positioning, EntityType, decimal.Parse(EntityId), CurrentCompany))
            {
                if (string.IsNullOrEmpty(selectedIndustries))
                {
                    ValidationDictionary.AddError("IndustryId", LabelResource.PositioningIndustriesRequiredError);
                }
                else
                {
                    if (!string.IsNullOrEmpty(EntityId) && !string.IsNullOrEmpty(EntityType) && !string.IsNullOrEmpty(selectedIndustries) && (selectedIndustries != "-1") && positioning.Status.Equals(PositioningStatus.Enabled))
                    {
                        decimal eId = decimal.Parse(EntityId);
                        string objectIndustryName = string.Empty;
                        IList<Industry> industryList = new List<Industry>();
                        if (positioning.Id == 0 || positioning.Id == null)
                        {
                            industryList = IndustryService.GetByEntityIdAndIndustryId(eId, EntityType, PositioningRelation.Positioning, GetListOfIds(selectedIndustries), CurrentCompany);
                        }
                        else
                        {
                            industryList = IndustryService.GetByPositioningEntityIdAndIndustryId(positioning.Id, PositioningIsGlobal.No, eId, EntityType, PositioningRelation.Positioning, GetListOfIds(selectedIndustries), CurrentCompany);
                        }
                        if (industryList != null && industryList.Count > 0)
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
                        }
                    }
                }
            } 
            else
            {
                if (string.IsNullOrEmpty(selectedIndustries))
                {
                    ValidationDictionary.AddError("IndustryId", LabelResource.PositioningIndustriesRequiredError);
                }
                else
                {
                    if (!string.IsNullOrEmpty(EntityId) && !string.IsNullOrEmpty(EntityType) && !string.IsNullOrEmpty(selectedIndustries) && (selectedIndustries != "-1") && positioning.Status.Equals(PositioningStatus.Enabled))
                    {
                        decimal eId = decimal.Parse(EntityId);
                        string objectIndustryName = string.Empty;
                        IList<Industry> industryList = new List<Industry>();
                        if (positioning.Id == 0 || positioning.Id == null)
                        {
                            industryList = IndustryService.GetByEntityIdAndIndustryId(eId, EntityType, PositioningRelation.Positioning, GetListOfIds(selectedIndustries), CurrentCompany);
                        }
                        else
                        {
                            industryList = IndustryService.GetByPositioningEntityIdAndIndustryId(positioning.Id, PositioningIsGlobal.No, eId, EntityType, PositioningRelation.Positioning, GetListOfIds(selectedIndustries), CurrentCompany);
                        }
                        if (industryList != null && industryList.Count > 0)
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
                        }
                    }
                }
            }
            // if (!PositioningService.IsGlobalIndustry(PositioningRelation.Positioning, EntityType, decimal.Parse(EntityId), CurrentCompany))
            //if (PositioningService.GetPositioningWithSameIndustry(PositioningRelation.Positioning,EntityType, )) 
            //{ }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            ViewData["DetailFilter"] = Request["DetailFilter"];
            IList<ResourceObject> statusList = ResourceService.GetAll<PositioningStatus>();
            ViewData["StatusList"] = new SelectList(statusList, "Id", "Value");
            ViewData["EntityIdList"] = new SelectList(new int[] { });
            ViewData["IndustryVisible"] = "Y";
            ViewData["IndustryIdList"] = new SelectList(new List<ResourceObject>(), "Id", "Value");
            ViewData["IsCompetitorCompany"] = "N";
            string entityIdValue = GetDetailFilterValue("Positioning.EntityId");
            string entityTypeValue = GetDetailFilterValue("Positioning.EntityType");
            if (!string.IsNullOrEmpty(entityTypeValue))
            {
                if (entityTypeValue.Equals(DomainObjectType.Product))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal productId = Convert.ToDecimal(entityIdValue);
                        Product product = ProductService.GetById(productId);
                        if (product != null)
                        {
                            if (product.CompetitorId != null)
                            {
                                Competitor competitor = CompetitorService.GetById((decimal)product.CompetitorId);
                                if (competitor != null)
                                {
                                    ClientCompany cc = ClientCompanyService.GetById(CurrentCompany);
                                    if (cc != null)
                                    {
                                        if (cc.Name.ToUpper().Equals(competitor.Name.ToUpper()) || cc.Dns.ToUpper().Equals(competitor.Name.ToUpper()))
                                        {
                                            ViewData["IsCompetitorCompany"] = "Y";
                                        }
                                    }
                                }
                            }
                        }
                        IList<IndustryByHierarchyView> industryByHierarchyList = SetIndustryListToPositioning();
                        ViewData["IndustryIdList"] = new SelectList(industryByHierarchyList, "Id", "Name");
                    }
                }
                else if (entityTypeValue.Equals(DomainObjectType.Competitor))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal competitorId = Convert.ToDecimal(entityIdValue);
                        Competitor competitor = CompetitorService.GetById(competitorId);
                        if (competitor != null)
                        {
                            ClientCompany cc = ClientCompanyService.GetById(CurrentCompany);
                            if (cc != null)
                            {
                                if (cc.Name.ToUpper().Equals(competitor.Name.ToUpper()) || cc.Dns.ToUpper().Equals(competitor.Name.ToUpper()))
                                {
                                    ViewData["IsCompetitorCompany"] = "Y";
                                }
                            }

                        }
                        IList<IndustryByHierarchyView> industryByHierarchyList = SetIndustryListToPositioning();
                        ViewData["IndustryIdList"] = new SelectList(industryByHierarchyList, "Id", "Name");
                    }
                }
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

        protected override void SetEntityDataToForm(Positioning positioning)
        {
            positioning.OldIsMaster = positioning.IsMaster;
           
            SetIndustryAndCompetitorValues(positioning);
            UserProfile createdBy = UserProfileService.GetById(positioning.CreatedBy);
            if (createdBy != null)
            {
                positioning.CreatedByName = createdBy.Name;
                ViewData["OpenedByName"] = createdBy.Name;
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
            if ((PositioningService.HasAccessToPositioning(positioning, userId)) || CurrentSecurityGroup.Equals("ADMIN"))
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
                            if(string.IsNullOrEmpty(positioning.IsGlobal) || positioning.IsGlobal.Equals(PositioningIsGlobal.No))
                            positioning.IndustriesIds = GetIndustriesForPositioning(positioning.Id);
                        }
                        if (string.IsNullOrEmpty(positioning.IndustriesIds))
                        {
                            if (!string.IsNullOrEmpty(positioning.IsGlobal) && positioning.IsGlobal.Equals(PositioningIsGlobal.Yes))
                            {
                                positioning.IndustriesIds = "-1";
                            }
                        }
                        positioning.OldIndustriesIds = positioning.IndustriesIds;
                        string[] selectedValues = positioning.IndustriesIds.Split(',');
                        var selected = selectedValues;
                        //List empty
                        IList<IndustryByHierarchyView> industryByHierarchyList = new List<IndustryByHierarchyView>();
                        // By Default
                        IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        industryAll.Name = "Industry All(default)";
                        industryAll.Id = -1;
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.Positioning, entityTypeValue, productId, CurrentCompany))
                        {
                            industryByHierarchyList = IndustryService.GetIndustryHierarchyByProduct(productId, CurrentCompany);
                            if (PositioningService.ExistGlobal(PositioningRelation.Positioning, entityTypeValue, productId, CurrentCompany))
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
                            }
                        }
                        positioning.OldIndustriesIds = positioning.IndustriesIds;
                        string[] selectedValues = positioning.IndustriesIds.Split(',');
                        var selected = selectedValues;
                        //List empty
                        IList<IndustryByHierarchyView> industryByHierarchyList = new List<IndustryByHierarchyView>();
                        // By Default
                        IndustryByHierarchyView industryAll = new IndustryByHierarchyView();
                        industryAll.Name = "Industry All(default)";
                        industryAll.Id = -1;
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.Positioning, entityTypeValue, competitorId, CurrentCompany))
                        {
                            industryByHierarchyList = IndustryService.GetIndustryHierarchyByCompetitor(competitorId, CurrentCompany);
                            if (PositioningService.ExistGlobal(PositioningRelation.Positioning, entityTypeValue, competitorId, CurrentCompany))
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

                    }
                }
            }
            //if (!string.IsNullOrEmpty(positioning.IndustriesIds))
            //{ 
            //    positioning.PositioningIndustryIds 
            //}
        }

        protected override void SetFormEntityDataToForm(Positioning positioning)
        {
            SetIndustryAndCompetitorValues(positioning);
            if (!IsSubTabPositioning())
            {
                SetCascadingEntityData(positioning);
            }
            positioning.OldIsMaster = positioning.IsMaster;
            positioning.OldIndustriesIds = positioning.IndustriesIds;
            ModelState.SetModelValue("OldIsMaster", new ValueProviderResult(positioning.OldIsMaster, positioning.OldIsMaster, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(positioning.OldIndustriesIds, positioning.OldIndustriesIds, CultureInfo.InvariantCulture));
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
            positioning.PositioningRelation = PositioningRelation.Positioning;
            SetDataForClientCompany(positioning);
        }

        protected override void GetFormData(Positioning positioning, FormCollection collection)
        {
            string industryidCollection = collection["IndustryIds"];
            if (!string.IsNullOrEmpty(industryidCollection))
            {
                // positioning.IndustryId = decimal.Parse(industryidCollection);
            }
            //else
            //{ 
            //    if(PositioningService.IsGlobalIndustry(PositioningRelation.CompetitiveMessaging, 
            //}
            string entityTypeCollection = collection["EntityType"];
            string entityIdColleccion = collection["EntityId"];
            if (!string.IsNullOrEmpty(entityTypeCollection) && !string.IsNullOrEmpty(entityIdColleccion))
            {
                positioning.EntityId = decimal.Parse(entityIdColleccion);
                positioning.EntityType = entityTypeCollection;
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
            else
            {
                if (PositioningService.IsGlobalIndustry(PositioningRelation.Positioning, positioning.EntityType, (decimal)positioning.EntityId, CurrentCompany))
                {
                    positioning.IsGlobal = PositioningIsGlobal.Yes;
                }
            }
        }
        #endregion

        #region Private Methods


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

        #endregion

        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetPositioningByCompetitor()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string detailTypeParam = Request["DetailCreateType"];
            string id = ids[0];
            //IList<Competitor> newEntities = new List<Competitor>();
            //DetailCreateType detailType = DetailCreateType.Override;
            //return RedirectToAction("GetData", "Browse", new { bid = "PositioningAll", page = 1, limi = 10, sord = "asc", filterCriteria = "PositioningAllView.EntityId_Eq_" + id + ":PositioningAllView.EntityType_Eq_COMPT" });
            //return RedirectToAction("/Browse.aspx/GetData?bid=PositioningAll&eou&filterCriteria=PositioningAllView.EntityId_Eq_" + id + ":PositioningAllView.EntityType_Eq_COMPT");
            //JavaScriptResult r = new JavaScriptResult();
            ////r.Script = "$(#ToolsPositioningAllListTable).setGridParam({ url: /Browse.aspx/GetData?bid=PositioningAll&eou&filterCriteria=PositioningAllView.EntityId_Eq_+" + id + ":PositioningAllView.EntityType_Eq_COMPT, page: 1 }).trigger('reloadGrid');alert(" + id + ")";
            //r.Script = "GetListPositioning();";
            //return r;
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
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.Positioning, entityTypeValue, productId, CurrentCompany))
                        {
                            result = IndustryService.GetIndustryHierarchyByProduct(productId, CurrentCompany);
                            if (!PositioningService.ExistGlobal(PositioningRelation.Positioning, DomainObjectType.Product, productId, CurrentCompany))
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
                        if (PositioningService.HavePositioningsEntity(PositioningRelation.Positioning, entityTypeValue, competitorId, CurrentCompany))
                        {
                            result = IndustryService.GetIndustryHierarchyByCompetitor(competitorId, CurrentCompany);
                            if (!PositioningService.ExistGlobal(PositioningRelation.Positioning, entityTypeValue, competitorId, CurrentCompany))
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

        #region Public Methods
        public void SetDataForClientCompany(Positioning positioning)
        {
            positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.No;
            string clientCompany = (string)Session["ClientCompany"];
            string entityIdValue = GetDetailFilterValue("Positioning.EntityId");
            string entityTypeValue = GetDetailFilterValue("Positioning.EntityType");
            if (!string.IsNullOrEmpty(entityTypeValue))
            {
                if (entityTypeValue.Equals(DomainObjectType.Product))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal productId = Convert.ToDecimal(entityIdValue);
                        Product product = ProductService.GetById(productId);
                        if (product != null)
                        {
                            if (product.CompetitorId != null)
                            {
                                Competitor competitor = CompetitorService.GetById((decimal)product.CompetitorId);
                                if (competitor != null)
                                {
                                    ClientCompany cc = ClientCompanyService.GetById(CurrentCompany);
                                    if (cc != null)
                                    {
                                        if (cc.Name.ToUpper().Equals(competitor.Name.ToUpper()) || cc.Dns.ToUpper().Equals(competitor.Name.ToUpper()))
                                        {
                                            ViewData["IsCompetitorCompany"] = "Y";
                                            positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                                        }
                                    }
                                }
                            }
                        }
                        //IList<IndustryByHierarchyView> industryByHierarchyList = SetIndustryListToPositioning();
                        //ViewData["IndustryIdList"] = new SelectList(industryByHierarchyList, "Id", "Name");
                    }
                }
                else if (entityTypeValue.Equals(DomainObjectType.Competitor))
                {
                    if (!string.IsNullOrEmpty(entityIdValue))
                    {
                        decimal competitorId = Convert.ToDecimal(entityIdValue);
                        Competitor competitor = CompetitorService.GetById(competitorId);
                        if (competitor != null)
                        {
                            ClientCompany cc = ClientCompanyService.GetById(CurrentCompany);
                            if (cc != null)
                            {
                                if (cc.Name.ToUpper().Equals(competitor.Name.ToUpper()) || cc.Dns.ToUpper().Equals(competitor.Name.ToUpper()))
                                {
                                    ViewData["IsCompetitorCompany"] = "Y";
                                    positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                                }
                            }

                        }
                        //IList<IndustryByHierarchyView> industryByHierarchyList = SetIndustryListToPositioning();
                       // ViewData["IndustryIdList"] = new SelectList(industryByHierarchyList, "Id", "Name");
                    }
                }
            }
        }
        #endregion
    }
}
