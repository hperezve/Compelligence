using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Resources;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Common.Utility;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;
using System.Text;
using Compelligence.Common.Utility.Web;
using System.Globalization;
using Compelligence.Util.Type;


namespace Compelligence.Web.Controllers
{
    public class CompetitorPartnerController : BackEndAsyncFormController<CompetitorPartner, CompetitorPartnerId>
    {

        #region Public Properties

        public ICompetitorPartnerService CompetitorPartnerService
        {
            get { return (ICompetitorPartnerService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(CompetitorPartner partner, FormCollection formCollection)
        {

            //if (Validator.IsBlankOrNull(partner.Name))
            //{
            //    ValidationDictionary.AddError("Name", LabelResource.PartnerNameRequiredError);
            //}

            //if (Validator.IsBlankOrNull(partner.AssignedTo))
            //{
            //    ValidationDictionary.AddError("AssignedTo", LabelResource.PartnerAssignedToRequiredError);
            //}
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);

            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
        }
        /// <summary>
        /// Make CompetitorParter after select on Popup
        /// </summary>
        public override ActionResult CreateDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string detailTypeParam = Request["DetailCreateType"];
            IList<CompetitorPartner> newEntities = new List<CompetitorPartner>();
            IList<CompetitorPartner> newEntitiesRelation = new List<CompetitorPartner>();
            string detailFilter = Request["DetailFilter"];
            string[] parameters = detailFilter.Split('_');

            foreach (object id in ids)
            {
                if (!string.IsNullOrEmpty(id as string))
                {
                    if (decimal.Parse(parameters[2]) != decimal.Parse(id.ToString()))
                    {
                        CompetitorPartnerId competitorPartnerId = new CompetitorPartnerId(decimal.Parse(parameters[2]), decimal.Parse(id.ToString()));
                        CompetitorPartner competitorPartner = CompetitorPartnerService.GetCompetitorPartner(competitorPartnerId);
                        CompetitorPartnerId competitorPartnerRetationId =new CompetitorPartnerId(decimal.Parse(id.ToString()),decimal.Parse(parameters[2]));
                        CompetitorPartner competitorPartnerRelation = CompetitorPartnerService.GetCompetitorPartner(competitorPartnerRetationId);
                        if (competitorPartner == null)
                        {
                            competitorPartner = new CompetitorPartner(competitorPartnerId);
                            competitorPartner.HeaderType = Request["HeaderType"];
                            SetDefaultDataForSave(competitorPartner);
                            SetDefaultDataForUpdate(competitorPartner);
                            SetDetailFilterData(competitorPartner);
                            newEntities.Add(competitorPartner);
                        }
                        if (competitorPartnerRelation == null)
                        {
                            competitorPartnerRelation = new CompetitorPartner(competitorPartnerRetationId);
                            competitorPartnerRelation.HeaderType = Request["HeaderType"];
                            SetDefaultDataForSave(competitorPartnerRelation);
                            SetDefaultDataForUpdate(competitorPartnerRelation);
                            SetDetailFilterData(competitorPartnerRelation);
                            newEntitiesRelation.Add(competitorPartnerRelation);
                        }
                    }
                }
            }
            CompetitorPartnerService.SaveCollection(newEntities);
            CompetitorPartnerService.SaveCollection(newEntitiesRelation);
            return null;
        }

        public override ActionResult DeleteDetail() //(CompetitorPartnerId id)
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(',');
            string detailFilter = Request["DetailFilter"];
            string[] parameters = detailFilter.Split('_');
            foreach (object identifier in ids)
            {
                //T entity = GenericService.GetById((IdT)Convert.ChangeType(identifier, typeof(IdT)));
                CompetitorPartnerId competitorPartnerId = new CompetitorPartnerId(decimal.Parse(parameters[2]), decimal.Parse(identifier.ToString()));
                CompetitorPartner competitorPartner = CompetitorPartnerService.GetCompetitorPartner(competitorPartnerId);

                CompetitorPartnerId competitorPartnerRetationId = new CompetitorPartnerId(decimal.Parse(identifier.ToString()), decimal.Parse(parameters[2]));
                CompetitorPartner competitorPartnerRelation = CompetitorPartnerService.GetCompetitorPartner(competitorPartnerRetationId);
                if (competitorPartner != null)
                {

                    //SetDefaultDataFromRequest(competitorPartner);
                    competitorPartner.HeaderType = StringUtility.CheckNull(Request["HeaderType"]);

                    SetDetailFilterData(competitorPartner);

                    SetDefaultDataForUpdate(competitorPartner);

                    CompetitorPartnerService.Delete(competitorPartner);
                    //GenericService.DeleteRelations(competitorPartner);
                }
                if (competitorPartnerRelation != null)
                {

                    //SetDefaultDataFromRequest(competitorPartnerRelation);
                    competitorPartnerRelation.HeaderType = StringUtility.CheckNull(Request["HeaderType"]);

                    SetDetailFilterData(competitorPartnerRelation);

                    SetDefaultDataForUpdate(competitorPartnerRelation);

                    CompetitorPartnerService.Delete(competitorPartnerRelation);
                    //GenericService.DeleteRelations(competitorPartnerRelation);
                }
            }

            return null;
        }

        protected override string SetDetailFilters(CompetitorPartnerId parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.CompetitorPartner;

            switch (detailType)
            {
                case DetailType.Employee:
                    AddFilter(detailFilter, "Employee.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Employee.CompanyType", CompanyType.Partner);
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyType", CompanyType.Partner);
                    childController = "Employee";
                    break;
                case DetailType.Location:
                    AddFilter(detailFilter, "Location.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Location.CompanyType", CompanyType.Partner);
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyType", CompanyType.Partner);
                    childController = "Location";
                    break;
                //case DetailType.Objective:
                //    AddFilter(detailFilter, "Objective.EntityId", parentId.ToString());
                //    AddFilter(browseDetailFilter, "ObjectiveDetailView.EntityId", parentId.ToString());
                //    childController = "Objective";
                //    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.CompetitorPartner);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                case DetailType.Budget:
                    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.CompetitorPartner);
                    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                    childController = "Budget";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.CompetitorPartner);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.CompetitorPartner);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(CompetitorPartner partner)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (CompetitorPartnerService.HasAccessToPartner(partner, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void SetEntityDataToForm(CompetitorPartner partner)
        {
            //ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(partner.MetaData);
            //partner.OldName = partner.Name;
        }

        protected override void GetFormData(CompetitorPartner partner, FormCollection collection)
        {
            //partner.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        protected override void SetDefaultEntityDataForSave(CompetitorPartner partner)
        {
            //partner.MetaData = partner.Name + ":" + partner.MetaData;
        }

        protected override void SetFormEntityDataToForm(CompetitorPartner partner)
        {
            //partner.OldName = partner.Name;
            //partner.MetaData = FormFieldsUtility.GetMultilineValue(partner.MetaData);
            //ModelState.SetModelValue("MetaData", new ValueProviderResult(partner.MetaData, partner.MetaData, CultureInfo.InvariantCulture));
            //ModelState.SetModelValue("OldName", new ValueProviderResult(partner.OldName, partner.OldName, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
