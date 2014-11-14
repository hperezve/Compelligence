using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Util;

namespace Compelligence.Web.Controllers
{
    public class EntityRelationController : BackEndAsyncFormController<EntityRelation, decimal>
    {

        #region Public Properties

        public IEntityRelationService EntityRelationService
        {
            get { return (IEntityRelationService)_genericService; }
            set { _genericService = value; }
        }

        public IResourceService ResourceService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IProjectService ProjectService { get; set; }

        public IDealService DealService { get; set; }

        public IEventService EventService { get; set; }

        public IQuizService QuizService { get; set; }

        //public ICalendarService CalendarService { get; set; }

        public IObjectiveService ObjectiveService { get; set; }

        public IKitService KitService { get; set; }

        public ICustomerService CustomerService { get; set; }

        public ISupplierService SupplierService { get; set; }

        public ICompetitorPartnerService CompetitorPartnerService { get; set; }

        public ILibraryService LibraryService { get; set; }

        public ICriteriaGroupService CriteriaGroupService { get; set; }

        //public ICriteriaSetService CriteriaSetService { get; set; }

        //public ICriteriaService CriteriaService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        public ITeamService TeamService { get; set; }

        public ITemplateService TemplateService { get; set; }

        public IWebsiteService WebsiteService { get; set; }

        public ITeamMemberService TeamMemberService { get; set; }

        public IFileService FileService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetChildEntityIdByType(string id)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (id.Equals(EntityRelationType.Competitor))
            {
                IList<Competitor> competitorList = CompetitorService.GetAllSortByClientCompany("Name", clientCompany);
                return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
            }
            else
            {
                if (id.Equals(EntityRelationType.Industry))
                {
                    IList<Industry> industryList = IndustryService.GetAllSortByClientCompany("Name", clientCompany);
                    return ControllerUtility.GetSelectOptionsFromGenericList<Industry>(industryList, "Id", "Name");
                }
                else
                {
                    if (id.Equals(EntityRelationType.Product))
                    {
                        IList<Product> productList = ProductService.GetAllSortByClientCompany("Name", clientCompany);
                        return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
                    }
                    else
                    {
                        if (id.Equals(EntityRelationType.Project))
                        {
                            IList<Project> projectList = ProjectService.GetAllSortByClientCompany("Name", clientCompany);
                            return ControllerUtility.GetSelectOptionsFromGenericList<Project>(projectList, "Id", "Name");
                        }
                        else
                        {
                            if (id.Equals(EntityRelationType.Deal))
                            {
                                IList<Deal> dealList = DealService.GetAllSortByClientCompany("Name", clientCompany);
                                return ControllerUtility.GetSelectOptionsFromGenericList<Deal>(dealList, "Id", "Name");
                            }
                            else
                            {
                                if (id.Equals(EntityRelationType.Event))
                                {
                                    IList<Event> eventList = EventService.GetAllSortByClientCompany("EventName", clientCompany);
                                    return ControllerUtility.GetSelectOptionsFromGenericList<Event>(eventList, "Id", "EventName");
                                }
                                else
                                {

                                    if (id.Equals(EntityRelationType.Objective))
                                    {
                                        IList<Objective> objectiveList = ObjectiveService.GetAllSortByClientCompany("Name", clientCompany);
                                        return ControllerUtility.GetSelectOptionsFromGenericList<Objective>(objectiveList, "Id", "Name");
                                    }
                                    else
                                    {
                                        if (id.Equals(EntityRelationType.Kit))
                                        {
                                            IList<Kit> kitList = KitService.GetAllSortByClientCompany("Name", clientCompany);
                                            return ControllerUtility.GetSelectOptionsFromGenericList<Kit>(kitList, "Id", "Name");
                                        }
                                        else
                                        {
                                            if (id.Equals(EntityRelationType.Customer))
                                            {
                                                IList<Customer> customerList = CustomerService.GetAllSortByClientCompany("Name", clientCompany);
                                                return ControllerUtility.GetSelectOptionsFromGenericList<Customer>(customerList, "Id", "Name");
                                            }
                                            else
                                            {
                                                if (id.Equals(EntityRelationType.Supplier))
                                                {
                                                    IList<Supplier> supplierList = SupplierService.GetAllSortByClientCompany("Name", clientCompany);
                                                    return ControllerUtility.GetSelectOptionsFromGenericList<Supplier>(supplierList, "Id", "Name");
                                                }
                                                else
                                                {
                                                    if (id.Equals(EntityRelationType.Library))
                                                    {
                                                        IList<Library> libraryList = LibraryService.GetAllSortByClientCompany("Name", clientCompany);
                                                        return ControllerUtility.GetSelectOptionsFromGenericList<Library>(libraryList, "Id", "Name");
                                                    }
                                                    else
                                                    {
                                                        return new JsonResult();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(EntityRelation entityRelation, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(entityRelation.Type))
            {
                ValidationDictionary.AddError("Type", LabelResource.EntityRelationTypeRequiredError);
            }
            if (Validator.IsBlankOrNull(entityRelation.ChildEntityId))
            {
                ValidationDictionary.AddError("ChildEntityId", LabelResource.EntityRelationChildEntityIdRequiredError);
            }
            return ValidationDictionary.IsValid;

        }

        #endregion

        #region Override Methods

        protected override void SetDefaultEntityDataForSave(EntityRelation entityRelation)
        {
            entityRelation.ParentType = Request["HeaderType"];
        }

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> entityRelationTypeList = ResourceService.GetAll<EntityRelationType>();

            ViewData["TypeList"] = new SelectList(entityRelationTypeList, "Id", "Value");
        }

        protected override void SetEntityDataToForm(EntityRelation entityRelation)
        {
            SetCascadingData(entityRelation);
        }

        protected override void SetFormEntityDataToForm(EntityRelation entityRelation)
        {
            SetCascadingData(entityRelation);
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(EntityRelation entityRelation)
        {
            string clientCompany = (string)Session["ClientCompany"];
            if (entityRelation.Type.Equals(DomainObjectType.Competitor))
            {
                IList<Competitor> competitorList = CompetitorService.GetAllSortByClientCompany("Name",clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(competitorList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Customer))
            {
                IList<Customer> customerList = CustomerService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(customerList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Deal))
            {
                IList<Deal> dealList = DealService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(dealList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Event))
            {
                IList<Event> eventList = EventService.GetAllSortByClientCompany("EventName", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(eventList, "Id", "EventName");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Industry))
            {
                IList<Industry> industryList = IndustryService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(industryList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Kit))
            {
                IList<Kit> kitList = KitService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(kitList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Library))
            {
                IList<Library> libraryList = LibraryService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(libraryList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Objective))
            {
                IList<Objective> objectiveList = ObjectiveService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(objectiveList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Product))
            {
                IList<Product> productList = ProductService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(productList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Project))
            {
                IList<Project> projectList = ProjectService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(projectList, "Id", "Name");
            }
            else if (entityRelation.Type.Equals(DomainObjectType.Supplier))
            {
                IList<Supplier> supplierList = SupplierService.GetAllSortByClientCompany("Name", clientCompany);
                ViewData["ChildEntityIdList"] = new SelectList(supplierList, "Id", "Name");
            }
        }

        #endregion

    }
}
