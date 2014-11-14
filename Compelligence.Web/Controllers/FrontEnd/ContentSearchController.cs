using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Views;
using System.Configuration;
using Compelligence.Common.Utility.Web;
using Compelligence.Domain.Entity;
using Compelligence.Util.Type;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Security.Filters;

namespace Compelligence.Web.Controllers
{
    public class ContentSearchController : GenericFrontEndController
    {
        #region Member Variables

        public const int maxpage = 10;//number results by page

        #endregion

        #region Public Properties

        public ISearchService SearchService { get; set; }

        public IProjectService ProjectService { get; set; }

        public ILibraryService LibraryService { get; set; }


        public IProductService ProductService { get; set; }

        public IDealService DealService { get; set; }

        public IEventService EventService { get; set; }

        public IObjectiveService ObjectiveService { get; set; }

        public IKitService KitService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public ICustomerService CustomerService { get; set; }

        public IQuizService QuizService { get; set; }

        public ISourceService SourceService { get; set; }


        public IFileService FileService { get; set; }

        public IUserProfileService UserProfileService { get; set; }

        #endregion

        #region Override Methods
        protected override void SetDefaultDataByPage()
        {
            ViewData["Entity"] = FrontEndPages.ContentSearch;
            ViewData["TitleHelp"] = "Content Search";
        }
        #endregion

        #region Action Methods
        [AcceptVerbs(HttpVerbs.Get), AuthenticationFilter]
        public ActionResult Index()
        {
            SetDefaultDataToLoadPage();
            string searchWord = Server.HtmlDecode(StringUtility.CheckNull(Request["q"]));
            GetDataOfConfiguration(CurrentCompany);
            int startIndex = Convert.ToInt32(Request["stidx"]);

            int count = SearchService.CountExecuteContentSearch(CurrentCompany, searchWord);
            int initialCount = count;

            // Results of Objects
            IList<Library> lstLibraries = LibraryService.GetNewsLibraries(CurrentCompany, searchWord);
            IList<Deal> lstDeals = DealService.GetDealsForSearch(CurrentCompany, searchWord);
            IList<Event> lstEvents = EventService.GetEventsForSearch(CurrentCompany, searchWord);
            IList<Product> lstProducts = ProductService.GetProductsForSearch(CurrentCompany, searchWord);
            IList<Objective> lstObjectives = ObjectiveService.GetObjectivesForSearch(CurrentCompany, searchWord);
            IList<Kit> lstKits = KitService.GetKitsForSearch(CurrentCompany, searchWord);
            IList<Industry> lstIndustries = IndustryService.GetIndustriesForSearch(CurrentCompany, searchWord);
            IList<Competitor> lstCompetitors = CompetitorService.GetCompetitorsForSearch(CurrentCompany, searchWord);
            IList<Customer> lstCustomers = CustomerService.GetCustomersForSearch(CurrentCompany, searchWord);

            count = count + lstLibraries.Count + lstDeals.Count + lstEvents.Count +
                lstProducts.Count + lstObjectives.Count + lstKits.Count + lstIndustries.Count +
                lstCompetitors.Count + lstCustomers.Count;

            decimal numPages = Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(maxpage));

            int page = (startIndex > 0) ? ((startIndex / maxpage) + 1) : 1;

            int pagesize = ((startIndex + maxpage) > count) ? count - startIndex : maxpage;

            IList<ProjectFileShowView> projectFileShowView = SearchService.GetContentSearch(CurrentCompany, searchWord, startIndex, pagesize);

            if (projectFileShowView.Count < 11)
            {
                if (projectFileShowView.Count !=     0)
                {
                    decimal contProj = projectFileShowView.Count;
                    foreach (Library library in lstLibraries)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = library.ClientCompany;
                        tempPjFile.Id = library.Id;
                        tempPjFile.ProjectName = library.Name;
                        tempPjFile.FileVersion = "URLNews";
                        tempPjFile.FilePhysicalName = library.Link;
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }
                    foreach (Deal deal in lstDeals)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = deal.ClientCompany;
                        tempPjFile.Id = deal.Id;
                        tempPjFile.ProjectName = deal.Name;
                        tempPjFile.FileVersion = "URLDeal";
                        tempPjFile.Description = deal.Details;
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }

                    foreach (Event events in lstEvents)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.Id = events.Id;
                        tempPjFile.ClientCompany = events.ClientCompany;
                        tempPjFile.ProjectName = events.EventName;
                        tempPjFile.FileVersion = "URLEvent";
                        tempPjFile.Description = events.Comment;
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }

                    foreach (Product product in lstProducts)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = product.ClientCompany;
                        tempPjFile.ProjectName = product.Name;
                        tempPjFile.FileVersion = "URLProduct";
                        tempPjFile.Description = product.Description;
                        tempPjFile.FilePhysicalName = product.Id.ToString();
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }

                    foreach (Objective objective in lstObjectives)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = objective.ClientCompany;
                        tempPjFile.ProjectName = objective.Name;
                        tempPjFile.FileVersion = "URLObjective";
                        tempPjFile.Description = objective.Detail;
                        tempPjFile.FilePhysicalName = objective.Id.ToString();
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }

                    foreach (Kit kit in lstKits)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = kit.ClientCompany;
                        tempPjFile.ProjectName = kit.Name;
                        tempPjFile.FileVersion = "URLKit";
                        tempPjFile.Description = kit.Comment;
                        tempPjFile.FilePhysicalName = kit.Id.ToString();
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }

                    foreach (Industry industry in lstIndustries)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = industry.ClientCompany;
                        tempPjFile.ProjectName = industry.Name;
                        tempPjFile.FileVersion = "URLIndustry";
                        tempPjFile.Description = industry.Description;
                        tempPjFile.FilePhysicalName = industry.Id.ToString();
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }

                    foreach (Competitor competitor in lstCompetitors)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = competitor.ClientCompany;
                        tempPjFile.ProjectName = competitor.Name;
                        tempPjFile.FileVersion = "URLCompetitor";
                        tempPjFile.Description = competitor.Description;
                        tempPjFile.FilePhysicalName = competitor.Id.ToString();
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }

                    foreach (Customer customer in lstCustomers)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = customer.ClientCompany;
                        tempPjFile.ProjectName = customer.Name;
                        tempPjFile.FileVersion = "URLCustomer";
                        tempPjFile.Description = customer.Description;
                        tempPjFile.FilePhysicalName = customer.Id.ToString();
                        if (contProj == maxpage)
                        {
                            break;
                        }
                        projectFileShowView.Add(tempPjFile);
                        contProj = contProj + 1;
                    }
                }
                else
                {
                    decimal contPags = (decimal)Math.Ceiling(((double)initialCount / (double)10));
                    decimal initialNews = initialCount % 10;
                    decimal firstNews = 0;
                    if (initialNews != 0)
                        firstNews = 10 - initialNews;
                    decimal startInd = firstNews + ((startIndex/10) - contPags) * 10;
                    decimal startTemp = startInd - 1;
                    decimal Cont = 0;
                    foreach (Library library in lstLibraries)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = library.ClientCompany;
                        tempPjFile.ProjectName = library.Name;
                        tempPjFile.FileVersion = "URLNews";
                        tempPjFile.FilePhysicalName = library.Link;
                        tempPjFile.Id = library.Id;
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;
                        
                    }

                    foreach (Deal deal in lstDeals)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.Id = deal.Id;
                        tempPjFile.ClientCompany = deal.ClientCompany;
                        tempPjFile.ProjectName = deal.Name;
                        tempPjFile.FileVersion = "URLDeal";
                        tempPjFile.Description = deal.Details;
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }

                    foreach (Event events in lstEvents)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.Id = events.Id;
                        tempPjFile.ClientCompany = events.ClientCompany;
                        tempPjFile.ProjectName = events.EventName;
                        tempPjFile.FileVersion = "URLEvent";
                        tempPjFile.Description = events.EventDescription;
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }

                    foreach (Product product in lstProducts)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = product.ClientCompany;
                        tempPjFile.Id = product.Id;
                        tempPjFile.ProjectName = product.Name;
                        tempPjFile.FileVersion = "URLProduct";
                        tempPjFile.Description = product.Description;
                        tempPjFile.FilePhysicalName = product.Id.ToString();
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }

                    foreach (Objective objective in lstObjectives)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = objective.ClientCompany;
                        tempPjFile.Id = objective.Id;
                        tempPjFile.ProjectName = objective.Name;
                        tempPjFile.FileVersion = "URLObjective";
                        tempPjFile.Description = objective.Detail;
                        tempPjFile.FilePhysicalName = objective.Id.ToString();
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }

                    foreach (Kit kit in lstKits)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = kit.ClientCompany;
                        tempPjFile.Id = kit.Id;
                        tempPjFile.ProjectName = kit.Name;
                        tempPjFile.FileVersion = "URLKit";
                        tempPjFile.Description = kit.Comment;
                        tempPjFile.FilePhysicalName = kit.Id.ToString();
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }

                    foreach (Industry industry in lstIndustries)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = industry.ClientCompany;
                        tempPjFile.Id = industry.Id;
                        tempPjFile.ProjectName = industry.Name;
                        tempPjFile.FileVersion = "URLKit";
                        tempPjFile.Description = industry.Description;
                        tempPjFile.FilePhysicalName = industry.Id.ToString();
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }

                    foreach (Competitor competitor in lstCompetitors)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = competitor.ClientCompany;
                        tempPjFile.Id = competitor.Id;
                        tempPjFile.ProjectName = competitor.Name;
                        tempPjFile.FileVersion = "URLCompetitor";
                        tempPjFile.Description = competitor.Description;
                        tempPjFile.FilePhysicalName = competitor.Id.ToString();
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }

                    foreach (Customer customer in lstCustomers)
                    {
                        ProjectFileShowView tempPjFile = new ProjectFileShowView();
                        tempPjFile.ClientCompany = customer.ClientCompany;
                        tempPjFile.Id = customer.Id;
                        tempPjFile.ProjectName = customer.Name;
                        tempPjFile.FileVersion = "URLCustomer";
                        tempPjFile.Description = customer.Description;
                        tempPjFile.FilePhysicalName = customer.Id.ToString();
                        if (startTemp == startInd + 10)
                        {
                            break;
                        }
                        if ((Cont > startTemp) && (Cont < startInd + 10))
                        {
                            projectFileShowView.Add(tempPjFile);
                        }
                        Cont = Cont + 1;

                    }
                }

            }

            ViewData["SearchWord"] = Server.HtmlEncode(searchWord);

            ViewData["FoundResultStart"] = startIndex + 1;

            ViewData["FoundResultEnd"] = startIndex + pagesize;

            ViewData["CountFoundResults"] = count;

            ViewData["SearchProjects"] = projectFileShowView;

            ViewData["CurrentPage"] = page;

            ViewData["MaxByPage"] = maxpage;

            ViewData["NumPages"] = Convert.ToInt32(numPages);

            return View("Results");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Download(decimal id)
        {
            File file = FileService.GetByEntityId(id, DomainObjectType.Project);
            string check = StringUtility.CheckNull(Request["chk"]);

            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
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
                UserProfile user = UserProfileService.GetById(CurrentUser);

                ProjectService.SaveDownload(id, user);

                //ActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File);
                ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
            }

            return Content(string.Empty);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
