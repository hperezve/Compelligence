using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Web;
using System.Text;
using Compelligence.Common.Utility.Web;
using System.Globalization;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class CustomerController : BackEndAsyncFormController<Customer, decimal>
    {

        #region Public Properties

        public ICustomerService CustomerService
        {
            get { return (ICustomerService)_genericService; }
            set { _genericService = value; }
        }

        public ICompetitorService CompetitorService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public IResourceService ResourceService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Customer customer, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(customer.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.CustomerNameRequiredError);
            }

            if (Validator.IsBlankOrNull(customer.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.CustomerAssignedToRequiredError);
            }

            if (Validator.IsBlankOrNull(customer.Description))
            {
                ValidationDictionary.AddError("Description", LabelResource.CustomerDescriptionRequiredError);
            }

            if (!Validator.IsBlankOrNull(customer.Website) && !Validator.IsUrl(customer.Website))
            {
                ValidationDictionary.AddError("Website", LabelResource.GlobalUrlFormatError);
            }

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<ResourceObject> customerStatusList = ResourceService.GetAll<CustomerStatus>();
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["StatusList"] = new SelectList(customerStatusList, "Id", "Value");
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Customer;

            switch (detailType)
            {
                case DetailType.Employee:
                    AddFilter(detailFilter, "Employee.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Employee.CompanyType", CompanyType.Customer);
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EmployeeDetailView.CompanyType", CompanyType.Customer);
                    childController = "Employee";
                    break;

                case DetailType.Location:
                    AddFilter(detailFilter, "Location.CompanyId", parentId.ToString());
                    AddFilter(detailFilter, "Location.CompanyType", CompanyType.Customer);
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyId", parentId.ToString());
                    AddFilter(browseDetailFilter, "LocationDetailView.CompanyType", CompanyType.Customer);
                    childController = "Location";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Customer);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Customer);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Customer);
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Budget:
                    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Customer);
                    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                    childController = "Budget";
                    break;
                case DetailType.Competitor:
                    AddFilter(detailFilter, "Competitor.CustomerId", parentId.ToString());
                    AddFilter(browseDetailFilter, "CompetitorCustomerDetailView.CustomerId", parentId.ToString());
                    childController = "Competitor:CompetitorCustomerDetail";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Customer);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Customer);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Metric:
                    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Customer);
                    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                    childController = "Metric";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Customer);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Product:
                    AddFilter(detailFilter, "Product.CustomerId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ProductCustomerDetailView.CustomerId", parentId.ToString());
                    childController = "Product:ProductCustomerDetail";
                    break;
                case DetailType.Industry:
                    AddFilter(detailFilter, "Industry.CustomerId", parentId.ToString());
                    AddFilter(browseDetailFilter, "IndustryCustomerDetailView.CustomerId", parentId.ToString());
                    childController = "Industry:IndustryCustomerDetail";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Customer customer)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (CustomerService.HasAccessToCustomer(customer, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        protected override void SetEntityDataToForm(Customer customer)
        {
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(customer.MetaData);
            customer.OldName = customer.Name;
        }

        protected override void GetFormData(Customer customer, FormCollection collection)
        {
            customer.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);
        }

        protected override void SetDefaultEntityDataForSave(Customer customer)
        {
            customer.MetaData = customer.Name + ":" + customer.MetaData;
        }

        protected override void SetFormEntityDataToForm(Customer customer)
        {
            customer.OldName = customer.Name;
            customer.MetaData = FormFieldsUtility.GetMultilineValue(customer.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(customer.MetaData, customer.MetaData, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(customer.OldName, customer.OldName, CultureInfo.InvariantCulture));
        }

        protected override void SetDetailFormData()
        {
            ViewData["HasRows"] = false;
            string headerType = Request["HeaderType"];
            string XId = null;
            decimal count = 0;

            if (!string.IsNullOrEmpty(headerType))
            {
                if (headerType.Equals(DomainObjectType.Competitor))
                    XId = GetDetailFilterValue("Customer.CompetitorId");

                else if (headerType.Equals(DomainObjectType.Product))
                    XId = GetDetailFilterValue("Customer.ProductId");

                if (!string.IsNullOrEmpty(XId))
                {
                    decimal xId = decimal.Parse(XId);

                    if (headerType.Equals(DomainObjectType.Competitor))
                    {
                        IList<CustomerCompetitorDetailView> xByCompetitor = CustomerService.GetByCompetitorIdAndClientCompany(xId, CurrentCompany);
                        if (xByCompetitor != null)
                        {
                            count = xByCompetitor.Count;
                        }
                    }
                    else if (headerType.Equals(DomainObjectType.Product))
                    {
                        IList<CustomerProductDetailView> xByProduct = CustomerService.GetByProductIdAndClientCompany(xId, CurrentCompany);
                        if (xByProduct != null)
                        {
                            count = xByProduct.Count;
                        }
                    }
                    if (count > 0)
                    {
                        ViewData["HasRows"] = true;
                    }
                }
            }
        }

        #endregion

        #region Public Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public ContentResult GetEntityName(decimal id)
        {
            string result = string.Empty;
            Customer entity = CustomerService.GetById(id);
            if (entity != null) result = entity.Name;
            return Content(result);
        }

        public ActionResult SetHasRows()
        {
            SetDetailFormData();
            return null;
        }
        #endregion
    }
}
