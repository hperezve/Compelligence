using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Resources;
using System.Text;
using Compelligence.Web.Models.Web;
using Compelligence.Util.Validation;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Web.Models.Util;
using Compelligence.Common.Utility.Web;
using System.Globalization;
using System.Configuration;
using Compelligence.Domain.Entity.Views;

namespace Compelligence.Web.Controllers
{
    public class ProjectArchiveController : BackEndAsyncFormController<Project, decimal>
    {
        #region Public Properties

        public IProjectService ProjectService
        {
            get { return (IProjectService)_genericService; }
            set { _genericService = value; }
        }

        public IUserProfileService UserProfileService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public IFileService FileService { get; set; }

        public IForumService ForumService { get; set; }

        public IForumResponseService ForumResponseService { get; set; }

        public ILibraryService LibraryService { get; set; }

        public IProjectIndustryService ProjectIndustryService { get; set; }

        public IProjectCompetitorService ProjectCompetitorService { get; set; }

        public IProjectProductService ProjectProductService { get; set; }

        public IIndustryProductService IndustryProductService { get; set; }

        public IIndustryCompetitorService IndustryCompetitorService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public IContentTypeService ContentTypeService { get; set; }

        public IResourceService ResourceService { get; set; }

        public IEmailService EmailService { get; set; }

        public IBudgetService BudgetService { get; set; }

        public IMarketTypeService MarketTypeService { get; set; }

        public IConfigurationService ConfigurationService { get; set; }

        public IQuizService QuizService { get; set; }

        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCompetitorsByIndustry(decimal id)
        {
            IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsByCompetitor(decimal id)
        {
            decimal industryId = Convert.ToDecimal(StringUtility.CheckNull(Request["IndustryId"]));

            IList<Product> productList = ProductService.GetByIndustryAndCompetitor(industryId, id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Product>(productList, "Id", "Name");
        }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(Project project, FormCollection formCollection)
        {
            string userId = (string)Session["UserId"];
            string clientCompany = (string)Session["ClientCompany"];
            UserProfile userReportTo = UserProfileService.GetById(userId);
            if (Validator.IsBlankOrNull(project.Name))
            {
                ValidationDictionary.AddError("Name", LabelResource.ProjectNameRequiredError);
            }

            if (Validator.IsBlankOrNull(project.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.ProjectUserIdRequiredError);
            }

            if (!(Validator.IsBlankOrNull(project.DueDateFrm) || Validator.IsDate(project.DueDateFrm, GetFormatDate())))
            {
                //Date should be in the format {0} //ProjectDueDateFormatError
                ValidationDictionary.AddError("DueDateFrm", string.Format(LabelResource.ProjectDueDateFormatError, GetFormatDate()));
            }

            if (project.Status.Equals(ProjectStatus.Published) && (!ProjectService.IsValidToPublish(project)))
            {
                ValidationDictionary.AddError("Status", LabelResource.ProjectStatusChangeToPublishError);
            }

            if (project.Status.Equals(ProjectStatus.Completed))
            {
                if ((!UserProfileService.IsRootUser(userId, clientCompany)) && (string.IsNullOrEmpty(userReportTo.ReportTo)))
                    ValidationDictionary.AddError("Status", LabelResource.ProjectStatusChangeToCompleteError);
            }
            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string userId = (string)Session["UserId"];

            IList<ResourceObject> projectStatusList = ResourceService.GetAll<ProjectStatus>();
            IList<ResourceObject> projectVisibilityList = ResourceService.GetAll<ProjectVisibility>();
            IList<ContentType> contentTypeList = ContentTypeService.FindNoAutomatic(clientCompany);
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            decimal industryListCount = (IndustryService.GetAllActiveByClientCompany(clientCompany).Count)*15;
            IList<MarketType> marketTypeList = MarketTypeService.GetAllActiveByClientCompany(clientCompany);

            ViewData["StatusList"] = new SelectList(projectStatusList, "Id", "Value");
            ViewData["VisibilityList"] = new SelectList(projectVisibilityList, "Id", "Value", ProjectVisibility.Complete);
            ViewData["ContentTypeList"] = new SelectList(contentTypeList, "Id", "Name");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            //ViewData["IndustryIdList"] = new SelectList(industryList, "Id", "Name");
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name");
            ViewData["IndustryIdMultiListCount"] = industryListCount;
            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");
            //ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selectedValues);
            ViewData["CreatedByFrm"] = UserProfileService.GetById(userId).Name;
            ViewData["CreatedDateFrm"] = DateTimeUtility.ConvertToString(DateTime.Now, GetFormatDate());
            ViewData["MarketTypeList"] = new SelectList(marketTypeList, "Id", "Name");
        }

        protected override void SetEntityDataToForm(Project project)
        {
            //obtain all industries which are related with this project
            project.IndustriesIds = obtainIndustriesForProject(project.Id);
            string[] selectedValues = project.IndustriesIds.Split(',');
            var selected = selectedValues;
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(project.ClientCompany);
            decimal industryListCount = (IndustryService.GetAllActiveByClientCompany(project.ClientCompany).Count) * 15;
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);
            ViewData["IndustryIdMultiListCount"] = industryListCount;

            ViewData["DueDateFrm"] = DateTimeUtility.ConvertToString(project.DueDate, GetFormatDate());
            ViewData["CreatedDateFrm"] = DateTimeUtility.ConvertToString(project.CreatedDate, GetFormatDate());
            ViewData["MetaData"] = FormFieldsUtility.GetMultilineValue(project.MetaData);
            project.OriginalStatus = project.Status;
            project.OldAssignedTo = project.AssignedTo;
            SetCascadingData(project);
            ViewData["CreatedByFrm"] = UserProfileService.GetById(project.CreatedBy).Name;
            project.OldName = project.Name;
            project.OldIndustriesIds = project.IndustriesIds;
            project.OldCompetitorsIds = project.CompetitorsIds;
            project.OldProductsIds = project.ProductsIds;
            SetDataToBudget(project);
        }

        protected override void SetFormEntityDataToForm(Project project)
        {
            //obtain all industries which are related with this project

            project.IndustriesIds = obtainIndustriesForProject(project.Id);
            string[] selected = project.IndustriesIds.Split(',');
            IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(project.ClientCompany);
            if (!project.ClientCompany.Equals(""))
                industryList = IndustryService.GetAllActiveByClientCompany(project.ClientCompany);
            else
                industryList = IndustryService.GetAllActiveByClientCompany((string)Session["ClientCompany"]);
            ViewData["IndustryIdMultiList"] = new MultiSelectList(industryList, "Id", "Name", selected);

            SetCascadingData(project);
            project.MetaData = FormFieldsUtility.GetMultilineValue(project.MetaData);
            ModelState.SetModelValue("MetaData", new ValueProviderResult(project.MetaData, project.MetaData, CultureInfo.InvariantCulture));
            project.OldName = project.Name;
            project.OldIndustriesIds = project.IndustriesIds;
            project.OldCompetitorsIds = project.CompetitorsIds;
            project.OldProductsIds = project.ProductsIds;
            project.OriginalStatus = project.Status;
            project.OldAssignedTo = project.AssignedTo;
            ModelState.SetModelValue("OriginalStatus", new ValueProviderResult(project.OriginalStatus, project.OriginalStatus, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldAssignedTo", new ValueProviderResult(project.OldAssignedTo, project.OldAssignedTo, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldName", new ValueProviderResult(project.OldName, project.OldName, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldIndustriesIds", new ValueProviderResult(project.OldIndustriesIds, project.OldIndustriesIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldCompetitorsIds", new ValueProviderResult(project.OldCompetitorsIds, project.OldCompetitorsIds, CultureInfo.InvariantCulture));
            ModelState.SetModelValue("OldProductsIds", new ValueProviderResult(project.OldProductsIds, project.OldProductsIds, CultureInfo.InvariantCulture));
        }

        protected override void GetFormData(Project project, FormCollection collection)
        {
            project.DueDate = DateTimeUtility.ConvertToDate(collection["DueDateFrm"], GetFormatDate());

            project.MetaData = FormFieldsUtility.SerializeValue(collection["MetaData"]);

            string selectedIndustries = collection["IndustryIds"];
            string selectedCompetitors = collection["CompetitorIds"];
            string selectedProducts = collection["ProductIds"];

            project.IndustriesIds = selectedIndustries;
            project.CompetitorsIds = selectedCompetitors;
            project.ProductsIds = selectedProducts;

            //if (project.Status.IndexOf(',') != -1)
            //{
            //    string[] tempor = project.Status.Split(',');
            //    project.Status = tempor[0];
            //    if (project.Status.Equals(ProjectStatus.Completed))
            //    {
            //        Compelligence.Domain.Entity.Configuration configuration = ConfigurationService.GetByCompany(CurrentCompany);
            //        if ((configuration != null) && (configuration.Approval.Equals(ProjectsApproval.True)))
            //        {
            //            project.Status = ProjectStatus.Published;
            //        }
            //    }
            //}
            //else
            //{
            //    if (project.Status.Equals(ProjectStatus.Completed))
            //    {
            //        Compelligence.Domain.Entity.Configuration configuration = ConfigurationService.GetByCompany(CurrentCompany);
            //        if ((configuration != null) && (configuration.Approval.Equals(ProjectsApproval.True)))
            //        {
            //            project.Status = ProjectStatus.Published;
            //        }
            //    }
            //}
        }

        protected override void SetDefaultEntityDataForSave(Project project)
        {
            //if (project.ProductId != null)
            //{
            //    Product product = ProductService.GetById((decimal)project.ProductId);
            //    project.MetaData = product.Name + ":" + project.MetaData;
            //}
            //if (project.CompetitorId != null)
            //{
            //    Competitor competitor = CompetitorService.GetById((decimal)project.CompetitorId);
            //    project.MetaData = competitor.Name + ":" + project.MetaData;
            //}
            //if (project.IndustryId != null)
            //{
            //    Industry industry = IndustryService.GetById((decimal)project.IndustryId);
            //    project.MetaData = industry.Name + ":" + project.MetaData;
            //}
            project.MetaData = project.Name + ":" + project.MetaData;
            if (!string.IsNullOrEmpty(project.IndustriesIds))
            {
                string[] industriesid = project.IndustriesIds.Split(',');
                for (int i = 0; i < industriesid.Length; i++)
                {
                    Industry industry = IndustryService.GetById(decimal.Parse(industriesid[i]));
                    project.MetaData += ":" + industry.Name;
                }
            }
            if (!string.IsNullOrEmpty(project.CompetitorsIds))
            {
                string[] competitorsId = project.CompetitorsIds.Split(',');
                for (int j = 0; j < competitorsId.Length; j++)
                {
                    Competitor competitor = CompetitorService.GetById(decimal.Parse(competitorsId[j]));
                    project.MetaData += ":" + competitor.Name;
                }
            }
            if (!string.IsNullOrEmpty(project.ProductsIds))
            {
                string[] productsid = project.ProductsIds.Split(',');
                for (int k = 0; k < productsid.Length; k++)
                {
                    Product product = ProductService.GetById(decimal.Parse(productsid[k]));
                    project.MetaData += ":" + product.Name;
                }
            }

            if (project.Status.Equals(ProjectStatus.Completed))
            {
                Compelligence.Domain.Entity.Configuration configuration = ConfigurationService.GetByCompany(CurrentCompany);
                if ((configuration != null) && (configuration.Approval.Equals(ProjectsApproval.True)))
                {
                    project.Status = ProjectStatus.Published;
                }
            }
        }

        protected override string SetDetailFilters(decimal parentId, DetailType detailType, StringBuilder detailFilter, StringBuilder browseDetailFilter)
        {
            string childController = string.Empty;
            ViewData["HeaderType"] = DomainObjectType.Project;

            switch (detailType)
            {
                case DetailType.File:
                    AddFilter(detailFilter, "File.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "File.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "FileDetailView.EntityId", parentId.ToString());
                    childController = "File";
                    break;
                case DetailType.Objective:
                    AddFilter(detailFilter, "Objective.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Objective.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "ObjectiveDetailView.EntityId", parentId.ToString());
                    childController = "Objective";
                    break;
                case DetailType.Kit:
                    AddFilter(detailFilter, "Kit.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Kit.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "KitDetailView.EntityId", parentId.ToString());
                    childController = "Kit";
                    break;
                case DetailType.Team:
                    AddFilter(detailFilter, "Team.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Team.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "TeamDetailView.EntityId", parentId.ToString());
                    childController = "Team";
                    break;
                //User
                case DetailType.User:
                    AddFilter(detailFilter, "UserProfile.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "UserProfile.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "UserDetailView.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "UserDetailView.EntityType", DomainObjectType.Project);
                    childController = "User";
                    break;
                //EndUSer
                case DetailType.Budget:
                    AddFilter(detailFilter, "Budget.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Budget.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "BudgetDetailView.EntityId", parentId.ToString());
                    childController = "Budget";
                    break;
                case DetailType.Library:
                    AddFilter(detailFilter, "Library.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Library.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "LibraryDetailView.EntityId", parentId.ToString());
                    childController = "Library";
                    break;
                case DetailType.Plan:
                    AddFilter(detailFilter, "Plan.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Plan.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "PlanDetailView.EntityId", parentId.ToString());
                    childController = "Plan";
                    break;
                case DetailType.Label:
                    AddFilter(detailFilter, "Label.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Label.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "LabelDetailView.EntityId", parentId.ToString());
                    childController = "Label";
                    break;
                case DetailType.Implication:
                    AddFilter(detailFilter, "Implication.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Implication.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "ImplicationDetailView.EntityId", parentId.ToString());
                    childController = "Implication";
                    break;
                case DetailType.Metric:
                    AddFilter(detailFilter, "Metric.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Metric.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "MetricDetailView.EntityId", parentId.ToString());
                    childController = "Metric";
                    break;
                case DetailType.Feedback:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Project);
                    childController = "ForumFeedback";
                    break;
                case DetailType.Discussion:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Project);
                    childController = "ForumDiscussion";
                    break;
                case DetailType.Comment:
                    AddFilter(detailFilter, "ForumResponse.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ForumResponse.EntityType", DomainObjectType.Project);
                    childController = "ForumComment";
                    break;
                case DetailType.ApprovalList:
                    AddFilter(detailFilter, "ApprovalList.EntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "ApprovalListDetailView.EntityId", parentId.ToString());
                    childController = "ApprovalList";
                    break;
                case DetailType.Source:
                    AddFilter(detailFilter, "Source.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "Source.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "SourceDetailView.EntityId", parentId.ToString());
                    childController = "Source";
                    break;
                case DetailType.EntityRelation:
                    AddFilter(detailFilter, "EntityRelation.ParentEntityId", parentId.ToString());
                    AddFilter(browseDetailFilter, "EntityRelationDetailView.ParentEntityId", parentId.ToString());
                    childController = "EntityRelation";
                    break;
                case DetailType.ActionHistory:
                    AddFilter(detailFilter, "ActionHistory.EntityId", parentId.ToString());
                    AddFilter(detailFilter, "ActionHistory.EntityType", DomainObjectType.Project);
                    AddFilter(browseDetailFilter, "ActionHistoryDetailView.EntityId", parentId.ToString());
                    childController = "ActionHistory";
                    break;
            }

            return childController;
        }

        protected override void SetUserSecurityAccess(Project project)
        {
            string userId = (string)Session["UserId"];
            string securityAccess = UserSecurityAccess.Read;

            if (ProjectService.HasAccessToProject(project, userId))
            {
                securityAccess = UserSecurityAccess.Edit;
            }

            ViewData["UserSecurityAccess"] = securityAccess;
        }

        #endregion

        #region Private Methods

        private void SetCascadingData(Project project)
        {
            project.CompetitorsIds = obtainCompetitorsForProject(project.Id);
            project.ProductsIds = obtainProductsForProject(project.Id);

            string[] selectedProducts = project.ProductsIds.Split(',');
            string[] selectedCompetitors = project.CompetitorsIds.Split(',');

            IList<Competitor> competitorList = new List<Competitor>();
            IList<Product> productList = new List<Product>();

            ViewData["CompetitorIdList"] = new MultiSelectList(new List<Competitor>(), "Id", "Name");
            ViewData["ProductIdList"] = new MultiSelectList(new List<Product>(), "Id", "Name");

            string clientCompany = (string)Session["ClientCompany"];

            string[] idsInd = project.IndustriesIds.Split(',');

            if (idsInd[0].Equals(""))
                idsInd = null;

            if (project.IndustriesIds != null && !project.IndustriesIds.Equals(""))
            {
                for (int i = 0; i < idsInd.Length; i++)
                {
                    IList<Competitor> tempCompetitorList = CompetitorService.GetByIndustryId(Convert.ToDecimal(idsInd[i]));
                    foreach (Competitor comp in tempCompetitorList)
                    {
                        if (competitorList.IndexOf(comp) == -1)
                        {
                            competitorList.Add(comp);
                        }
                    }



                }
            }
            if (project.CompetitorsIds != null && !project.CompetitorsIds.Equals(""))
            {
                for (int j = 0; j < selectedCompetitors.Length; j++)
                {
                    IList<Product> tempProductList = ProductService.GetByCompetitor(Convert.ToDecimal(selectedCompetitors[j]));
                    //obtain products by Industry
                    foreach (Product prod in tempProductList)
                    {
                        bool addProduct = false;
                        if (!project.IndustriesIds.Equals(""))
                        {
                            //Verifying if Products from the Competitor are from Industries too
                            string[] actualIndustries = project.IndustriesIds.Split(',');
                            foreach (string idInd in actualIndustries)
                            {
                                IndustryProductId idIndProd = new IndustryProductId(Convert.ToDecimal(idInd), prod.Id);
                                IndustryProduct tempIndProd = IndustryProductService.GetById(idIndProd);
                                if (tempIndProd != null)
                                {
                                    addProduct = true;
                                }
                            }

                        }
                        else
                        { addProduct = true; }

                        if (productList.IndexOf(prod) == -1 && addProduct)
                        {
                            productList.Add(prod);
                        }


                    }
                }
            }

            if (productList.Count > 0)
            {
                if (project.ProductsIds == null)
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name");
                else
                    ViewData["ProductIdList"] = new MultiSelectList(productList, "Id", "Name", selectedProducts);
            }

            if (competitorList.Count > 0)
            {
                if (project.CompetitorsIds == null)
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name");
                else
                    ViewData["CompetitorIdList"] = new MultiSelectList(competitorList, "Id", "Name", selectedCompetitors);
            }

        }

        private Project SetDataToBudget(Project project)
        {
            IList<Budget> budgetList = BudgetService.GetBudgetsByEntityIdAndType(project.Id, DomainObjectType.Project, CurrentCompany);
            if (budgetList.Count > 0)
            {
                decimal sumValueBudgetsFinancial = 0;
                decimal sumValueBudgetsTime = 0;
                string unitOfBudgetsFinancial = string.Empty;
                string unitOfBudgetsTime = string.Empty;
                foreach (Budget budgetUnit in budgetList)
                {
                    if (budgetUnit.Type == BudgetTypeUnit.Financial)
                    { unitOfBudgetsFinancial = budgetUnit.UnitMeasureCode; }
                    else if (budgetUnit.Type == BudgetTypeUnit.Time)
                    { unitOfBudgetsTime = budgetUnit.UnitMeasureCode; }

                }

                foreach (Budget budget in budgetList)
                {
                    if (budget.Type == BudgetTypeUnit.Financial && budget.UnitMeasureCode == unitOfBudgetsFinancial)
                    {
                        if (budget.Value == null)
                            budget.Value = 0;
                        sumValueBudgetsFinancial = sumValueBudgetsFinancial + (decimal)budget.Value;
                    }
                    else if (budget.Type == BudgetTypeUnit.Time && budget.UnitMeasureCode == unitOfBudgetsTime)
                    {
                        sumValueBudgetsTime = sumValueBudgetsTime + (decimal)budget.Value;
                    }
                }
                project.FinancialBudgetUnit = ResourceService.GetName<BudgetTypeFinancial>(unitOfBudgetsFinancial);
                project.TimeBudgetUnit = ResourceService.GetName<BudgetTypeTime>(unitOfBudgetsTime);
                project.TotalFinancialBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgetsFinancial);
                project.TotalTimeBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgetsTime);

                //if (budgetList[0].Type == BudgetTypeUnit.Financial)
                //{
                //    project.FinancialBudgetUnit = ResourceService.GetName<BudgetTypeFinancial>(unitMeasureDefault);
                //    foreach (Budget budget in budgetList)
                //    {
                //        if (unitMeasureDefault == budget.UnitMeasureCode)
                //        {
                //            if (sumValueBudgets == 0)
                //            {
                //                sumValueBudgets = (decimal)budget.Value;
                //            }
                //            else
                //            {
                //                sumValueBudgets = sumValueBudgets + (decimal)budget.Value;
                //            }
                //        }
                //    }
                //    project.TotalFinancialBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgets);
                //}
                //else if (budgetList[0].Type == BudgetTypeUnit.Time)
                //{
                //    project.TimeBudgetUnit = ResourceService.GetName<BudgetTypeTime>(unitMeasureDefault);
                //    foreach (Budget budget in budgetList)
                //    {
                //        if (unitMeasureDefault == budget.UnitMeasureCode)
                //        {
                //            if (sumValueBudgets == 0)
                //            {
                //                sumValueBudgets = (decimal)budget.Value;
                //            }
                //            else
                //            {
                //                sumValueBudgets = sumValueBudgets + (decimal)budget.Value;
                //            }
                //        }
                //    }
                //    project.TotalTimeBudget = FormatUtility.GetFormatValue("{0:0.##}", sumValueBudgets);
                //}
            }
            return project;
        }

        private string obtainIndustriesForProject(decimal idProject)
        {
            string ids = null;
            IList<ProjectIndustry> lstProjInd = ProjectIndustryService.GetByProjectId(idProject);
            int cont = 0;
            foreach (ProjectIndustry projInd in lstProjInd)
            {
                cont++;

                if (lstProjInd.Count == cont)
                {
                    ids = ids + projInd.Id.IndustryId.ToString();
                }
                else
                {
                    ids = ids + projInd.Id.IndustryId + ",";
                }

            }
            return ids;
        }

        private string obtainCompetitorsForProject(decimal idProject)
        {
            string ids = null;
            IList<ProjectCompetitor> lstProjComp = ProjectCompetitorService.GetByProjectId(idProject);
            int cont = 0;
            foreach (ProjectCompetitor projComp in lstProjComp)
            {
                cont++;

                if (lstProjComp.Count == cont)
                {
                    ids = ids + projComp.Id.CompetitorId.ToString();
                }
                else
                {
                    ids = ids + projComp.Id.CompetitorId + ",";
                }

            }
            return ids;
        }

        private string obtainProductsForProject(decimal idProject)
        {
            string ids = null;
            IList<ProjectProduct> lstProjProd = ProjectProductService.GetByProjectId(idProject);
            int cont = 0;
            foreach (ProjectProduct projProd in lstProjProd)
            {
                cont++;

                if (lstProjProd.Count == cont)
                {
                    ids = ids + projProd.Id.ProductId.ToString();
                }
                else
                {
                    ids = ids + projProd.Id.ProductId + ",";
                }

            }
            return ids;
        }
        #endregion

        #region Public Methods
        public String GetName(decimal id)
        {
            Project project = ProjectService.GetById(id);
            if (project == null) return string.Empty;
            return project.Name;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCompetitors(decimal id)
        {
            IList<Competitor> competitorList = CompetitorService.GetByIndustryId(id);

            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(competitorList, "Id", "Name", false);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetCompetitorsOfIndustry(decimal id)
        {
            string result = string.Empty;
            IList<Competitor> lstCompetitors = CompetitorService.GetByIndustryId(id);
            int cont = 0;
            foreach (Competitor comp in lstCompetitors)
            {
                cont++;
                if (lstCompetitors.Count == cont)
                {
                    result = result + comp.Id + ":" + comp.Name;
                }
                else
                {
                    result = result + comp.Id + ":" + comp.Name + "_";
                }

            }
            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetProductsOfCompetitor(decimal id)
        {
            string result = string.Empty;
            string idProject = Request["idProject"];
            string idsIndustries = Request["idsIndustries"];
            string[] arrayIndustries = idsIndustries.Split(',');
            IList<Product> lstProds = ProductService.GetByCompetitor(id);
            int cont = 0;
            foreach (Product prod in lstProds)
            {
                cont++;
                bool addProd = false;

                if (!idsIndustries.Equals(""))
                {
                    if (prod.CompetitorId != null)
                    {
                        foreach (string idInd in arrayIndustries)
                        {
                            IndustryProductId idIndProd = new IndustryProductId(Convert.ToDecimal(idInd), prod.Id);
                            IndustryProduct tempIndProd = IndustryProductService.GetById(idIndProd);
                            if (tempIndProd != null)
                            {
                                addProd = true;
                            }
                        }
                    }
                }
                else
                {

                    addProd = true;
                }

                if (addProd)
                {
                    if (lstProds.Count == cont)
                    {
                        result = result + prod.Id + ":" + prod.Name;
                    }
                    else
                    {
                        result = result + prod.Id + ":" + prod.Name + "_";
                    }
                }

            }

            if (result.Length > 0 && result.Substring(result.Length - 1, 1).Equals("_"))
            {
                result = result.Substring(0, result.Length - 1);
            }
            return Content(result);
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Comments(decimal id)
        {
            Project eventProject = ProjectService.GetById(id);
            Forum forum = ForumService.GetByEntityId(eventProject.Id, DomainObjectType.Project, ForumType.Comment);
            if (forum != null)
            {
                if (forum.ViewsCounter == null)
                    forum.ViewsCounter = 0;
                forum.ViewsCounter = forum.ViewsCounter + 1;
                ForumService.Update(forum);
            }
            ViewData["Libraries"] = LibraryService.GetByEntityId(eventProject.Id, DomainObjectType.Project);
            ViewData["Comments"] = (forum == null) ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, DomainObjectType.Project);
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            //
            SetUserSecurityAccess(eventProject);
            //
            //Commented due to Columns IndustryId, CompetitorId and ProductId from Events
            //eventObject.Industry = IndustryService.GetById(DecimalUtility.CheckNull(eventObject.IndustryId));
            //eventObject.Competitor = CompetitorService.GetById(DecimalUtility.CheckNull(eventObject.CompetitorId));
            //eventObject.Product = ProductService.GetById(DecimalUtility.CheckNull(eventObject.ProductId));

            return View("Comments", eventProject);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RemoveComments(decimal id, decimal forumresponseid)
        {
            Project deal = ProjectService.GetById(id);
            Forum forum = ForumService.GetByEntityId(deal.Id, DomainObjectType.Project, ForumType.Comment);

            ForumResponseService.DeleteCascading(forum.Id, forumresponseid);
            return Comments(id);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download(decimal id)
        {
            File file = FileService.GetByEntityId(id, DomainObjectType.Library);
            string check = StringUtility.CheckNull(Request["chk"]);

            string path = ConfigurationSettings.AppSettings["LibraryFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;

            if (check.ToLower().Equals("true"))
            {
                if ((file != null) && System.IO.File.Exists(fullpath + file.PhysicalName))
                    return Content("Found");
                else
                    return Content("NotFound");
            }
            else
            {
                //ActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File);
                ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
            }

            return Content(string.Empty);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendComment(decimal id, FormCollection form)
        {
            Project projectObject = ProjectService.GetById(id);
            ForumResponse forumResponse = new ForumResponse();

            forumResponse.EntityId = id;
            forumResponse.EntityType = DomainObjectType.Project;
            forumResponse.CreatedBy = CurrentUser;
            forumResponse.CreatedDate = DateTime.Now;
            forumResponse.LastChangedBy = CurrentUser;
            forumResponse.LastChangedDate = DateTime.Now;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            forumResponse.ParentResponseId = (!string.IsNullOrEmpty(form["ParentResponseId"])) ? Convert.ToDecimal(form["ParentResponseId"]) : 0;
            forumResponse.ClientCompany = CurrentCompany;

            forumResponse.Libraries = GetLibrariesForEntity(projectObject.Id, DomainObjectType.Project, LibraryTypeKeyCode.Project);

            ForumService.SaveForumResponse(forumResponse, ForumType.Comment);

            EmailService.SendCommentEmail(forumResponse.CreatedBy, projectObject.Name, DomainObjectType.Project, id, CurrentUser, forumResponse.Response, CurrentCompany, forumResponse.Libraries);

            //ActionHistory(id, EntityAction.Commented, DomainObjectType.Forum);
            ActionHistoryService.RecordActionHistory(id, EntityAction.Commented, DomainObjectType.Project, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            return RedirectToAction("Comments", new { id = id });
        }

        //public ActionResult GetInactiveProjects()
        //{
        //    ViewData["Scope"] = Request["Scope"];
        //    ViewData["Container"] = Request["Container"];
        //    ViewData["ShowInactiveProjects"] = "all";
        //    return View("Index");
        //}
        #endregion

        public ContentResult ChangeIndustryList(bool IsChecked, string[] IndustryIds)
        {
            string[] tempo = new string[] { };
            var selected = tempo;
            if (IndustryIds != null)
            {
                selected = IndustryIds;
            }
            string result = string.Empty;
            bool isSelected = false;
            if (IsChecked)
            {
                IList<IndustryByHierarchyView> industryListHierarchy = IndustryService.FindAllActiveByHierarchy(CurrentCompany);

                int cont = 0;
                foreach (IndustryByHierarchyView industryHierarchy in industryListHierarchy)
                {
                    isSelected = false;
                    if (selected.Length > 0)
                    {
                        for (int m = 0; m < selected.Length; m++)
                        {
                            if (decimal.Parse(selected[m]) == industryHierarchy.Id)
                            {
                                isSelected = true;
                                m = selected.Length;
                            }
                        }
                    }

                    cont++;
                    if (industryListHierarchy.Count == cont)
                    {
                        result = result + industryHierarchy.Id + ":" + industryHierarchy.Name + ":" + isSelected.ToString();
                    }
                    else
                    {
                        result = result + industryHierarchy.Id + ":" + industryHierarchy.Name + ":" + isSelected.ToString() + "_";
                    }

                }

            }
            else
            {
                IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(CurrentCompany);
                int cont2 = 0;
                foreach (Industry industry in industryList)
                {
                    isSelected = false;
                    if (selected.Length > 0)
                    {
                        for (int m = 0; m < selected.Length; m++)
                        {
                            if (decimal.Parse(selected[m]) == industry.Id)
                            {
                                isSelected = true;
                                m = selected.Length;
                            }
                        }
                    }

                    cont2++;
                    if (industryList.Count == cont2)
                    {
                        result = result + industry.Id + ":" + industry.Name + ":" + isSelected.ToString();
                    }
                    else
                    {
                        result = result + industry.Id + ":" + industry.Name + ":" + isSelected.ToString() + "_";
                    }

                }
            }
            return Content(result);

        }

        public override ActionResult CreateDetail()
        {
            string[] ids = StringUtility.CheckNull(Request["Id"]).Split(':');
            string detailTypeParam = Request["DetailCreateType"];


            string entityId = GetDetailFilterValue("File.EntityId");
            string entityType = GetDetailFilterValue("File.EntityType");

            string fileNameResult = string.Empty;
            string headerType = (string)Request["HeaderType"];

            foreach (string idlibrary in ids)
            {
                Library library = LibraryService.GetById(decimal.Parse(idlibrary));

                File file = FileService.GetByEntityId(decimal.Parse(idlibrary), DomainObjectType.Library);
                if (file != null) //have file
                {
                    file.EntityId = decimal.Parse(entityId);
                    file.EntityType = entityType;
                    String oldPhysicalName = file.PhysicalName;
                    FileService.SaveNewFileVersion(file, CurrentUser);

                    System.IO.File.Move(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["LibraryFilePath"], oldPhysicalName),
                                        System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + ConfigurationSettings.AppSettings["ContentFilePath"], file.PhysicalName));
                    LibraryService.Delete(library.Id);
                }
            }
            return Content(" done.!");

        }

    }
}