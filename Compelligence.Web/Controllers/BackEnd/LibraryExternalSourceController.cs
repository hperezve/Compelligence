using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Domain.Entity;
using Resources;
using Compelligence.Util.Validation;
using Compelligence.BusinessLogic.Implementation;
using System.Threading;
using Spring.Context;
using Spring.Context.Support;
using Compelligence.DataTransfer.Entity;

namespace Compelligence.Web.Controllers
{

    public class LibraryExternalSourceBgProcess
    {
        decimal id;
        string bgprocessresult=string.Empty;
        string CurrentCompany = string.Empty;

        public ILibraryExternalSourceService LibraryExternalSourceService { get; set; }
        public ILibraryService LibraryService { get; set; }
        public ILibraryTypeService LibraryTypeService { get; set; }
        public INewsScoringService NewsScoringService { get; set; }
        public IIndustryService IndustryService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IProductService ProductService { get; set; }
        public IConfigurationService ConfigurationService { get; set; }

        public LibraryExternalSourceBgProcess(decimal id, string CurrentCompany)
        {
            this.id = id;
            this.CurrentCompany = CurrentCompany;

            IApplicationContext context = ContextRegistry.GetContext();
            LibraryExternalSourceService = context.GetObject("LibraryExternalSourceService") as ILibraryExternalSourceService;
            LibraryService = context.GetObject("LibraryService") as ILibraryService;
            LibraryTypeService = context.GetObject("LibraryTypeService") as ILibraryTypeService;
            NewsScoringService = context.GetObject("NewsScoringService") as INewsScoringService;
            IndustryService = context.GetObject("IndustryService") as IIndustryService;
            CompetitorService = context.GetObject("CompetitorService") as ICompetitorService;
            ProductService = context.GetObject("ProductService") as IProductService;
            ConfigurationService = context.GetObject("ConfigurationService") as IConfigurationService;

        }

        public void process()
        {
            string result = string.Empty;
            bool successread = true;

            LibraryExternalSource item = LibraryExternalSourceService.GetById(this.id);
            if (item != null)
            {
                Configuration configuration = ConfigurationService.GetByCompany(CurrentCompany);
                LibraryType type = LibraryTypeService.GetByKeyCode(LibraryTypeKeyCode.News, CurrentCompany);
                IList<NewsScoring> newsScoring = NewsScoringService.GetEnabledByClientCompany(new string[] { CurrentCompany });
                IList<LibraryLinkDTO> libraryLinks = LibraryService.GetLinkByClientCompany(new string[] { CurrentCompany });
                IList<string> libraryLinksByCompany = new List<string>();

                if (libraryLinks != null)
                {
                    for (int i = 0; i < libraryLinks.Count; i++)
                    {
                        libraryLinksByCompany.Add(libraryLinks[i].Link);
                    }
                }

                if (item.Type.Equals(LibraryExternalSourceType.DynamicRss))
                {
                    if (string.Compare(item.Target, LibraryExternalSourceTarget.Competitors) == 0)
                    {
                        IList<Competitor> competitorsList = CompetitorService.GetWithKeyWord(new string[] { CurrentCompany });
                        successread = LibraryExternalSourceService.ExecuteDynamicRSSReaderByCompetitor(item, competitorsList, libraryLinksByCompany, type,
                            newsScoring);
                    }
                    else if (string.Compare(item.Target, LibraryExternalSourceTarget.Products) == 0)
                    {
                        IList<Product> productList = ProductService.GetWithKeyWord(new string[] { CurrentCompany });
                        successread = LibraryExternalSourceService.ExecuteDynamicRSSReaderByProduct(item, productList, libraryLinksByCompany, type,
                            newsScoring);
                    }
                }
                else if (item.Type.Equals(LibraryExternalSourceType.Rss))
                {
                    if (configuration != null)
                    {
                        successread = LibraryExternalSourceService.ExecuteRSSReader(item, libraryLinksByCompany, type,
                            null, newsScoring);
                    }
                    else
                    {
                        successread = LibraryExternalSourceService.ExecuteRSSReader(item, libraryLinksByCompany, type,
                                null, newsScoring);
                    }
                }

                else if (item.Type.Equals(LibraryExternalSourceType.Bing))
                {
                    if (string.Compare(item.Target, LibraryExternalSourceTarget.Products) == 0)
                    {
                        IList<Product> productList = ProductService.GetWithKeyWord(new string[] { CurrentCompany });
                        successread = LibraryExternalSourceService.ExecuteBingSearchApiByProduct(item, productList, libraryLinksByCompany, type,
                            newsScoring);
                    }
                    if (string.Compare(item.Target, LibraryExternalSourceTarget.Competitors) == 0)
                    {
                        IList<Competitor> competitorsList = CompetitorService.GetWithKeyWord(new string[] { CurrentCompany });
                        successread = LibraryExternalSourceService.ExecuteBingSearchApiByCompetitor(item, competitorsList, libraryLinksByCompany, type,
                            newsScoring);
                    }
                    if (string.Compare(item.Target, LibraryExternalSourceTarget.Industry) == 0)
                    {
                        IList<Industry> industryList = IndustryService.GetWithKeyWord(new string[] { CurrentCompany });
                        successread = LibraryExternalSourceService.ExecuteBingSearchApiByIndustry(item, industryList, libraryLinksByCompany, type,
                            newsScoring);
                    }
                }

                if (successread)
                {
                    LibraryService.DeleteDuplicateLibraries(CurrentCompany);
                }
            }
            result = successread.ToString();
            bgprocessresult=result;
        }
    }
    public class LibraryExternalSourceController : BackEndAsyncFormController<LibraryExternalSource, decimal>
    {

        #region Public Properties

        public ILibraryExternalSourceService LibraryExternalSourceService
        {
            get { return (ILibraryExternalSourceService)_genericService; }
            set { _genericService = value; }
        }

        public ILibraryService LibraryService { get; set; }
        public IResourceService ResourceService { get; set; }
        public IUserProfileService UserProfileService { get; set; }
        public ILibraryTypeService LibraryTypeService { get; set; }
        public INewsScoringService NewsScoringService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IProductService ProductService { get; set; }
        public IConfigurationService ConfigurationService { get; set; }

        #endregion

        #region Validation Methods

        protected override bool ValidateFormData(LibraryExternalSource libraryExternalSource, FormCollection formCollection)
        {
            if (Validator.IsBlankOrNull(libraryExternalSource.AssignedTo))
            {
                ValidationDictionary.AddError("AssignedTo", LabelResource.LibraryExternalSourceAssignedToRequiredError);
            }

            if (Validator.IsBlankOrNull(libraryExternalSource.Source) && !libraryExternalSource.Type.Equals(LibraryExternalSourceType.Bing))
            {
                ValidationDictionary.AddError("Source", LabelResource.LibraryExternalSourceSourceRequiredError);
            }

            if (Validator.IsBlankOrNull(libraryExternalSource.Type))
            {
                ValidationDictionary.AddError("Type", "Type is required");
            }
            else
            {
                if ( libraryExternalSource.Type.Equals(LibraryExternalSourceType.DynamicRss) 
                    && Validator.IsBlankOrNull(libraryExternalSource.Target ) )
                {
                    ValidationDictionary.AddError("Target", "Need select Target for Dynamic RSS");
                }
                if ( libraryExternalSource.Type.Equals(LibraryExternalSourceType.DynamicRss) 
                    &&  libraryExternalSource.Source.ToLower().IndexOf("[target]")<0 )
                {
                    ValidationDictionary.AddError("Source", "Need include [target] tag into source string for Dynamic RSS");
                }
            }
            //if (libraryExternalSource.Type.Equals(LibraryExternalSourceType.Email) && (!Validator.IsEmailOrWhite(libraryExternalSource.EmailFrom)))
            //{
            //    ValidationDictionary.AddError("EmailFrom", "Email From has invalid format");
            //}

            return ValidationDictionary.IsValid;
        }

        #endregion

        #region Override Methods

        protected override void SetFormData()
        {
            string clientCompany = (string)Session["ClientCompany"];
            IList<ResourceObject> libraryExternalSourceTypeList = ResourceService.GetAll<LibraryExternalType>();
            IList<ResourceObject> libraryExteranaSourceStatusList = ResourceService.GetAll<LibraryExternalSourceStatus>();
            IList<UserProfile> userList = UserProfileService.GetAdministratorUsers(clientCompany);
            ViewData["TypeList"] = new SelectList(libraryExternalSourceTypeList, "Id", "Value");
            ViewData["AssignedToList"] = new SelectList(userList, "Id", "Name");
            ViewData["StatusList"] = new SelectList(libraryExteranaSourceStatusList, "Id", "Value");
            ViewData["EmailList"] = new SelectList(userList, "Email", "Email");

            List<SelectListItem> frequencyReaderList = new List<SelectListItem>();

            for (int i = 1; i <= 24; i++)
            {
                frequencyReaderList.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() + " Hours" });
            }

            ViewData["FrequencyReaderList"] = new SelectList(frequencyReaderList, "Value", "Text");
            
            IList<ResourceObject> targetList = ResourceService.GetAll<LibraryExternalSourceTarget>();
            ViewData["TargetList"] = new SelectList(targetList, "Id", "Value");
        }

        protected override void GetActionHistoryCreated(LibraryExternalSource libraryExternalSource, FormCollection collection)
        {
            string domainObjectEntity = libraryExternalSource.GetType().Name;
            ActionHistoryService.BackEndActionHistory(libraryExternalSource.Id, EntityAction.Created, domainObjectEntity, domainObjectEntity, libraryExternalSource.Source, CurrentUser, CurrentCompany);
        }
        protected override void GetActionHistoryUpdated(LibraryExternalSource libraryExternalSource, FormCollection collection)
        {
            string domainObjectEntity = libraryExternalSource.GetType().Name;
            ActionHistoryService.BackEndActionHistory(libraryExternalSource.Id, EntityAction.Updated, domainObjectEntity, domainObjectEntity, libraryExternalSource.Source, CurrentUser, CurrentCompany);
        }
        protected override void GetActionHistoryDeleted(LibraryExternalSource libraryExternalSource)
        {
            string domainObjectEntity = libraryExternalSource.GetType().Name;
            ActionHistoryService.BackEndActionHistory(libraryExternalSource.Id, EntityAction.Deleted, domainObjectEntity, domainObjectEntity, libraryExternalSource.Source, CurrentUser, CurrentCompany);
        }
        #endregion

        public ContentResult ExecuteRssReaderContent(decimal Id)
        {
            string result = string.Empty;
            bool successread = true;


            LibraryExternalSource item = LibraryExternalSourceService.GetById(Id);
            if (item != null)
            {
                Configuration configuration = ConfigurationService.GetByCompany(CurrentCompany);
                LibraryType type = LibraryTypeService.GetByKeyCode(LibraryTypeKeyCode.News, CurrentCompany);
                IList<NewsScoring> newsScoring = NewsScoringService.GetEnabledByClientCompany(new string[] { CurrentCompany });
                IList<LibraryLinkDTO> libraryLinks = LibraryService.GetLinkByClientCompany(new string[] { CurrentCompany });
                IList<string> libraryLinksByCompany = new List<string>();

                if (libraryLinks != null)
                {
                    for (int i = 0; i < libraryLinks.Count; i++)
                    {
                        libraryLinksByCompany.Add(libraryLinks[i].Link);
                    }
                }

                if (item.Type.Equals(LibraryExternalSourceType.DynamicRss))
                {
                    if (string.Compare(item.Target, LibraryExternalSourceTarget.Competitors) == 0)
                    {
                        IList<Competitor> competitorsList = CompetitorService.GetWithKeyWord(new string[] { CurrentCompany });
                        successread = LibraryExternalSourceService.ExecuteDynamicRSSReaderByCompetitor(item, competitorsList, libraryLinksByCompany, type,
                            newsScoring);
                    }
                    else if (string.Compare(item.Target, LibraryExternalSourceTarget.Products) == 0)
                    {
                        IList<Product> productList = ProductService.GetWithKeyWord(new string[] { CurrentCompany });
                        successread = LibraryExternalSourceService.ExecuteDynamicRSSReaderByProduct(item, productList, libraryLinksByCompany, type,
                            newsScoring);
                    }
                }
                else if (item.Type.Equals(LibraryExternalSourceType.Rss))
                {
                    successread = LibraryExternalSourceService.ExecuteRSSReader(item, libraryLinksByCompany, type,
                            null, newsScoring);
                }
            }

            if (successread)
            {
                LibraryService.DeleteDuplicateLibraries(CurrentCompany);
            }

            result = successread.ToString();
            return Content(result);
        }


        public void ExecuteRSSReader(decimal id)
        {
            LibraryExternalSourceBgProcess bg = new LibraryExternalSourceBgProcess(id, CurrentCompany);
            Thread thread = new Thread(new ThreadStart(bg.process));
            thread.Start();
        }
    }
}
