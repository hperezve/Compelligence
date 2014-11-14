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
using Compelligence.Web.Models.Web;
using System.Text;
using Compelligence.Common.Utility.Web;
using System.Globalization;

namespace Compelligence.Web.Controllers
{
    public class SupplierController : BackEndAsyncFormController<Supplier, decimal>
    {

        #region Public Properties

        public ISupplierService SupplierService
        {
            get { return (ISupplierService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public IProductService ProductService { get; set; }

        public IResourceService ResourceService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Supplier supplier, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(supplier.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.SupplierNameRequiredError);
            }
            if (Validator.IsBlankOrNull(supplier.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.SupplierAssignedToRequiredError);
            }
            return ValidationDictionary.IsValid;

        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            IList<Product> productList = ProductService.GetAllActiveByClientCompany(clientCompany);

            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["IndustryIdList"] = new SelectList(industryList, "Id", "Name");
            ViewData["ProductIdList"] = new SelectList(productList, "Id", "Name");
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Supplier;

            switch (detailType)
            {
                case DetailType.Employee:
                    AddFilter(detailFilter, "Employee.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Employee.CompanyType", CompanyType.Supplier);
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyType", CompanyType.Supplier);
                    childController = "Employee";
                    break;
                case DetailType.Location:
                    AddFilter(detailFilter, "Location.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Location.CompanyType", CompanyType.Supplier);
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyType", CompanyType.Supplier);
                    childController = "Location";
                    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Supplier);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Supplier);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                case DetailType.Budget:
                    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Supplier);
                    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                    childController = "Budget";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Supplier);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "Forum.EntityId", parentId.ToString());
                    childController = "Forum::DetailGrid";
                    break;
                case DetailType.Competitor:
                    AddFilter(detailFilter, "Competitor.SupplierId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorSupplierDetailView.SupplierId", parentId.ToString());
                    childController = "Competitor:CompetitorSupplierDetail";
                    break;
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Supplier);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                case DetailType.Industry:
                    AddFilter(detailFilter, "Industry.SupplierId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Industry.EntityType", DomainObjectType.Supplier);
                    AddFilter(browseDetailFilter, "IndustrySupplierDetailView.SupplierId", parentId.ToString());
                    childController = "Industry:IndustrySupplierDetail";
                    break;
                case DetailType.Event:
                    AddFilter(detailFilter, "Event.SupplierId", parentId.ToString());
                    AddFilter(detailFilter, "Event.EntityType", DomainObjectType.Supplier);
                    AddFilter(browseDetailFilter, "EventDetailView.SupplierId", parentId.ToString());
                    childController = "Event";
                    break;
                case DetailType.Product:
                    AddFilter(detailFilter, "Product.SupplierId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProductSupplierDetailView.SupplierId", parentId.ToString());
                    childController = "Product:ProductSupplierDetail";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Supplier supplier)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (SupplierService.HasAccessToSupplier(supplier, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void SetEntityDataToForm(Supplier supplier)
        {
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(supplier.MetaData);
            supplier.OldName = supplier.Name;
        }

        protected override void GetFormData(Supplier supplier, FormCollection collection)
        {
            supplier.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        protected override void SetDefaultEntityDataForSave(Supplier supplier)
        {
            supplier.MetaData = supplier.Name + ":" + supplier.MetaData;
        }

        protected override void SetFormEntityDataToForm(Supplier supplier)
        {
            supplier.OldName = supplier.Name;
            supplier.MetaData = FormFieldsUtility.GetMultilineValue(supplier.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(supplier.MetaData, supplier.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(supplier.OldName, supplier.OldName, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
