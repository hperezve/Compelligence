using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Common.Browse;
using Compelligence.Common.Cache;
using Compelligence.Common.Utility.Parser;
using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Domain.Entity.Views;
using Compelligence.Reports.Helpers;
using Compelligence.Util.Collections;
using Compelligence.Security.Managers;
using System.Configuration;
using System.Data;
using Compelligence.Reports.Dynamic;
using Compelligence.Security.Filters;
using Compelligence.DataTransfer.Entity;
using Compelligence.Util.Type;
using Compelligence.Web.Models.Util;

namespace Compelligence.Web.Controllers
{
    public class ReportController : BrowseController
    {
        #region Public Properties

        private IReportService ReportService { get; set; }

        private IUserProfileService UserProfileService { get; set; }

        private IClientCompanyService ClientCompanyService { get; set; }
        public IIndustryService IndustryService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IIndustryCompetitorService IndustryCompetitorService { get; set; }
        public IResourceService ResourceService { get; set; }
        public IProductService ProductService { get; set; }
        public ICompetitorFinancialService CompetitorFinancialService { get; set; }

        public string appPath = System.Web.HttpContext.Current.Request.ApplicationPath.ToString();
        private static string reportContext = AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Workspace()
        {
            ViewData["ReportModule"] = ReportModule.Workspace;

            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Environment()
        {
            ViewData["ReportModule"] = ReportModule.Environment;

            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Admin()
        {
            ViewData["ReportModule"] = ReportModule.Admin;

            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Dynamic()
        {
            ViewData["ReportModule"] = ReportModule.Admin;

            return View("IndexDynamic");
        }

        //public ActionResult Deal()
        //{
        //    ViewData["ReportModule"] = ReportModule.Deal;

        //    return View("Index");
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult General()
        {
            ViewData["ReportModule"] = ReportModule.General;

            return View("Index");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult List()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;

            string reportModule = Request["ReportModule"];
            IList<Report> reportList = ReportService.GetByModule(reportModule);
            IList<ResourceObject> reportGroup = GetGroupByModule(reportList);
            ViewData["ReportGroup"] = reportGroup;
            IList<ReportGroupList> rgList = GetReportByGroup(reportList, reportGroup);
            ViewData["ReportGroupList"] = rgList;
            //reportList = CombineListAndGroup(reportList, reportGroup);
            //ViewData["ReportList"] = reportList;
            ViewData["ReportModule"] = Request["ReportModule"];

            //ReorderAndGroupReports(reportList);
            return View(reportList);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Filter()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;

            ViewData["ReportFilter"] = Request["ReportFilter"];
            ViewData["ReportTitle"] = Request["ReportTitle"];
            ViewData["ReportModule"] = Request["ReportModule"];

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Generate(FormCollection collection)
        {
            string browseId = Request["BrowseId"];
            string filterCriteria = Request["FilterCriteria"];
            string reportTitle = Request["Title"];
            string hidencColumnCriteria = Request["HiddenColumnCriteria"];

            ResourceDataManager.GetInstance().CleanNumberGroup();

            BrowseObject browseObject = GetBrowseObjectForQuery(browseId, filterCriteria);
            IList dataSourceObjects = ReportService.GetData(browseObject);
            string reportFilter = GetFilterCriteria(filterCriteria, browseObject);
            string reportHiddenColumn = GetHiddenColumnCriteria(hidencColumnCriteria);
            IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();

            IDictionary<string, string> columnDictionary = new Dictionary<string, string>();

            reportParameters["BrowseObject"] = browseObject;
            reportParameters["DataSource"] = dataSourceObjects;
            UserProfile userProfile = UserManager.GetInstance().GetUserProfile(Session["UserId"].ToString());
            reportParameters["UserId"] = userProfile.Name;
            ClientCompany clientCompany = ClientCompanyService.GetById(Session["ClientCompany"].ToString());
            reportParameters["ClientCompany"] = clientCompany.Name;
            reportParameters["ReportTitle"] = reportTitle;
            reportParameters["ReportFilter"] = reportFilter;

            //reportParameters["ReportColumn"] = reportHiddenColumn;
            string[] filterHCCriteriaArray = hidencColumnCriteria.Split(':');
            //foreach (string hiddenColumn in filterHCCriteriaArray)
            //{
            //    string[] operators = hiddenColumn.Split('_');
            //    string[] fields = operators[0].Split('.');
            //    if ((operators.Length == 3) && (operators[2].ToString().Equals("false")))
            //    {

            //        //reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
            //        columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "true");
            //        reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
            //    }
            //    else
            //    {
            //        columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "false");
            //        reportParameters[fields[1].ToString() + "HiddenColumn"] = "false";
            //    }
            //    //columnDictionary.Add(fields[1].ToString(), operators[2].ToString());
            //}
            bool isColumnHidden;
            for (int b = 0; b < browseObject.BrowseColumns.Count; b++)
            {
                if (browseObject.BrowseColumns[b].Filter)
                {
                    isColumnHidden = false;
                    for (int p = 0; p < filterHCCriteriaArray.Length; p++)
                    {
                        string[] operators = filterHCCriteriaArray[p].Split('_');
                        string[] fields = operators[0].Split('.');
                        if ((operators.Length == 3) && (browseObject.BrowseColumns[b].Property == fields[1]))
                        {
                            isColumnHidden = true;
                            p = filterHCCriteriaArray.Length;
                            if (operators[2].ToString().Equals("false"))
                            {
                                if (!columnDictionary.ContainsKey(fields[1].ToString() + "HiddenColumn"))
                                {
                                    columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "true");
                                    reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
                                }
                            }
                            else if (operators[2].ToString().Equals("true"))
                            {
                                if (!columnDictionary.ContainsKey(fields[1].ToString() + "HiddenColumn"))
                                {
                                    columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "false");
                                    reportParameters[fields[1].ToString() + "HiddenColumn"] = "false";
                                }
                            }
                        }
                    }
                    if (!isColumnHidden)
                    {
                        columnDictionary.Add(browseObject.BrowseColumns[b].Property.ToString() + "HiddenColumn", "false");
                        reportParameters[browseObject.BrowseColumns[b].Property + "HiddenColumn"] = "false";
                    }
                }
            }

            reportParameters["ColumnDictionary"] = columnDictionary;
            string reportFile = ReportHelper.PrintReport(reportParameters);

            return Content(reportFile);
        }

        #endregion

        #region Private Methods

        private BrowseObject GetBrowseObjectForQuery(string browseId, string filterCriteria)
        {
            BrowseObject browseObject = (BrowseObject)ReportManager.GetInstance().GetBrowseObject(browseId).Clone();
            browseObject.WhereClause = ExpressionParser.GetExpression(Session, browseObject.WhereClause);
            browseObject.WhereClause = ExpressionParser.GetExpression(browseObject.WhereClause);
            browseObject.InitializeBrowseFilters();
            GetFilterCriteria(browseObject, filterCriteria);

            return browseObject;
        }

        private string GetFilterCriteria(string filterCriteria)
        {
            string[] filterCriteriaArray = filterCriteria.Split(':');
            filterCriteria = "";

            foreach (string filter in filterCriteriaArray)
            {
                string[] operators = filter.Split('_');
                if ((operators.Length == 3) && (!string.IsNullOrEmpty(operators[2])))
                {
                    string[] fields = operators[0].Split('.');

                    string stringOperator = ResourceDataManager.GetInstance().GetLabel("FilterOperator", operators[1]);
                    stringOperator = (string)HttpContext.GetGlobalResourceObject("LabelResource", "FilterOperator" + stringOperator);

                    filterCriteria += fields[1] + " " + stringOperator + " " + operators[2] + "\n";
                }
            }

            if (string.IsNullOrEmpty(filterCriteria))
            {
                filterCriteria = "Print All";
            }

            return filterCriteria;
        }

        private string GetFilterCriteria(string filterCriteria, BrowseObject browseObject)
        {
            string[] filterCriteriaArray = filterCriteria.Split(':');
            filterCriteria = "";

            foreach (string filter in filterCriteriaArray)
            {
                string[] operators = filter.Split('_');
                if ((operators.Length == 3) && (!string.IsNullOrEmpty(operators[2])))
                {
                    string[] fields = operators[0].Split('.');
                    string filterColumn = fields[1];
                    string filterValue = operators[2];
                    string stringOperator = ResourceDataManager.GetInstance().GetLabel("FilterOperator", operators[1]);
                    string operatorTempo = stringOperator;
                    stringOperator = (string)HttpContext.GetGlobalResourceObject("LabelResource", "FilterOperator" + stringOperator);
                    if (string.IsNullOrEmpty(stringOperator))
                    {
                        stringOperator = operatorTempo;
                    }
                    for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
                    {
                        if (operators[0] == browseObject.BrowseColumns[i].Column)
                        {
                            filterColumn = browseObject.BrowseColumns[i].Label;
                            if (browseObject.BrowseColumns[i].ColumnTypeClass.Equals(Compelligence.Common.Browse.BrowseColumn.TypeClass.Standard))
                            {

                                filterValue = ResourceDataManager.GetInstance().GetLabel(browseObject.BrowseColumns[i].ColumnType.ToString(), operators[2]);
                            }
                            else if (browseObject.BrowseColumns[i].ColumnTypeClass.Equals(Compelligence.Common.Browse.BrowseColumn.TypeClass.MultiStandard))
                            {

                                filterValue = ResourceDataManager.GetInstance().GetLabel(browseObject.BrowseColumns[i].ColumnType.ToString(), operators[2]);
                            }
                        }
                    }
                    filterCriteria += filterColumn + " " + stringOperator + " " + filterValue + "\n";
                }
                else if ((operators.Length == 5) && (!string.IsNullOrEmpty(operators[2])) && (!string.IsNullOrEmpty(operators[4])))
                {
                    string[] fields = operators[0].Split('.');
                    string filterColumn = fields[1];
                    string filterValue = operators[2] + " And " + operators[4];
                    string stringOperator = ResourceDataManager.GetInstance().GetLabel("FilterOperator", operators[1]);
                    string operatorTempo = stringOperator;
                    stringOperator = (string)HttpContext.GetGlobalResourceObject("LabelResource", "FilterOperator" + stringOperator);
                    if (string.IsNullOrEmpty(stringOperator))
                    {
                        stringOperator = operatorTempo;
                    }
                    for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
                    {
                        if (operators[0] == browseObject.BrowseColumns[i].Column)
                        {
                            filterColumn = browseObject.BrowseColumns[i].Label;
                            //if (browseObject.BrowseColumns[i].ColumnTypeClass.Equals(Compelligence.Common.Browse.BrowseColumn.TypeClass.Standard))
                            //{

                            //    filterValue = ResourceDataManager.GetInstance().GetLabel(browseObject.BrowseColumns[i].ColumnType.ToString(), operators[2]);
                            //}
                        }
                    }
                    filterCriteria += filterColumn + " " + stringOperator + " " + filterValue + "\n";
                }
            }

            if (string.IsNullOrEmpty(filterCriteria))
            {
                filterCriteria = "Print All";
            }

            return filterCriteria;
        }

        private string GetHiddenColumnCriteria(string hidenColumnCriteria)
        {
            //string[] filterHCCriteriaArray = hidenColumnCriteria.Split(':');
            //hidenColumnCriteria = "";

            //foreach (string filter in filterHCCriteriaArray)
            //{
            //    string[] operators = filter.Split('_');
            //    if ((operators.Length == 3) && (!string.IsNullOrEmpty(operators[2])))
            //    {
            //        string[] fields = operators[0].Split('.');

            //        string stringOperator = ResourceDataManager.GetInstance().GetLabel("FilterOperator", operators[1]);
            //        stringOperator = (string)HttpContext.GetGlobalResourceObject("LabelResource", "FilterOperator" + stringOperator);

            //        hidenColumnCriteria += fields[1] + " " + stringOperator + " " + operators[2] + "\n";
            //    }
            //}

            //if (string.IsNullOrEmpty(hidenColumnCriteria))
            //{
            //    hidenColumnCriteria = "Print All";
            //}

            //return hidenColumnCriteria;
            string columnHiddens = string.Empty;
            string[] filterHCCriteriaArray = hidenColumnCriteria.Split(':');
            foreach (string hiddenColumn in filterHCCriteriaArray)
            {
                string[] operators = hiddenColumn.Split('_');
                if ((operators.Length == 3) && (operators[2].ToString().Equals("false")))
                {
                    string[] fields = operators[0].Split('.');
                    columnHiddens += fields[1].ToString() + " is hidden " + operators[2].ToString() + "\n";
                    //reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
                }
            }
            return columnHiddens;
        }

        #endregion

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Comparison()
        {
            ViewData["ReportModule"] = ReportModule.Comparison;
            string clientCompany = (string)Session["ClientCompany"];
            //IList<Industry> industryList = IndustryService.GetAllActiveByClientCompany(clientCompany);
            //ViewData["IndustryList"] = new MultiSelectList(industryList, "Id", "Name");

            IList<IndustryByHierarchyView> IndustryCollection = IndustryService.FindIndustryHierarchy(clientCompany);
            ViewData["IndustryList"] = new MultiSelectList(IndustryCollection, "Id", "Name");

            //IList<Competitor> competitorList = CompetitorService.GetAllActiveByClientCompany(clientCompany);
            IList<Competitor> competitorList = new List<Competitor>();
            ViewData["CompetitorList"] = new MultiSelectList(competitorList, "Id", "Name");
            //IList<Product> productList = ProductService.GetAllActiveByClientCompany(clientCompany);
            IList<Product> productList = new List<Product>();
            ViewData["ProductList"] = new MultiSelectList(productList, "Id", "Name");
            return View("IndexComparison");
        }

        public ContentResult GetDataComparison()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string industries = StringUtility.CheckNull(Request["ind"]);
            string competitors = StringUtility.CheckNull(Request["com"]);
            string products = StringUtility.CheckNull(Request["pro"]);
            string startDate = StringUtility.CheckNull(Request["sDate"]);
            string endDate = StringUtility.CheckNull(Request["eDate"]);
            string sort = StringUtility.CheckNull(Request["sort"]);
            string result = string.Empty;
            string objectValues = string.Empty;
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startDate))
            {
                sDate = DateTimeUtility.ConvertToDate(startDate, "MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                eDate = DateTimeUtility.ConvertToDate(endDate, "MM/dd/yyyy");
            }
            IList<System.Object> res = null;
            string[] productIds = products.Split(';');
            if (productIds != null && productIds.Length > 0)
            {
                decimal[] proIds = new decimal[productIds.Length];
                for (int p = 0; p < productIds.Length; p++)
                {
                    if (!string.IsNullOrEmpty(productIds[p]))
                    {
                        proIds[p] = decimal.Parse(productIds[p]);
                    }
                }
                if (!string.IsNullOrEmpty(industries))
                {
                    string[] industryIds = industries.Split(';');
                    if (industryIds != null && industryIds.Length > 0)
                    {
                        decimal[] indIds = new decimal[industryIds.Length];
                        for (int i = 0; i < industryIds.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(industryIds[i]))
                            {
                                indIds[i] = decimal.Parse(industryIds[i]);
                            }
                        }
                        res = ProductService.GetComparisonFilter(clientCompany, sDate, eDate, indIds, proIds,sort);
                        IList<System.Object> test = ProductService.FindAllComparisonFilter(clientCompany, sDate, eDate, indIds, proIds, sort);
                    }

                }
                else
                {
                    res = ProductService.GetComparisonFilter(clientCompany, sDate, eDate, null, proIds,sort);
                }

                foreach (System.Object[] objt in res)
                {
                    if (objectValues.Length > 0) objectValues += ";";
                    objectValues += objt[1] + "_" + objt[3] + "_" + objt[2];
                }
                result = objectValues;
                
            }
            return Content(result);
        }

        public ContentResult GetDataComparisonByIndustry()
        {
            string clientCompany = (string)Session["ClientCompany"];
            string industries = StringUtility.CheckNull(Request["ind"]);
            string competitors = StringUtility.CheckNull(Request["com"]);
            string products = StringUtility.CheckNull(Request["pro"]);
            string startDate = StringUtility.CheckNull(Request["sDate"]);
            string endDate = StringUtility.CheckNull(Request["eDate"]);
            string sort = StringUtility.CheckNull(Request["sort"]);
            string result = string.Empty;
            string objectValues = string.Empty;
            DateTime? sDate = null;
            DateTime? eDate = null;
            if (!string.IsNullOrEmpty(startDate))
            {
                sDate = DateTimeUtility.ConvertToDate(startDate, "MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                eDate = DateTimeUtility.ConvertToDate(endDate, "MM/dd/yyyy");
            }
            IList<System.Object> res = null;
            string[] productIds = products.Split(';');
            if (productIds != null && productIds.Length > 0)
            {
                decimal[] proIds = new decimal[productIds.Length];
                for (int p = 0; p < productIds.Length; p++)
                {
                    if (!string.IsNullOrEmpty(productIds[p]))
                    {
                        proIds[p] = decimal.Parse(productIds[p]);
                    }
                }
                if (!string.IsNullOrEmpty(industries))
                {
                    string[] industryIds = industries.Split(';');
                    if (industryIds != null && industryIds.Length > 0)
                    {
                        decimal[] indIds = new decimal[industryIds.Length];
                        for (int i = 0; i < industryIds.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(industryIds[i]))
                            {
                                indIds[i] = decimal.Parse(industryIds[i]);
                            }
                        }
                        res = ProductService.FindAllComparisonFilter(clientCompany, sDate, eDate, indIds, proIds, sort); 
                        //IList<System.Object> test = 
                    }

                }
                else
                {
                    res = ProductService.GetComparisonFilter(clientCompany, sDate, eDate, null, proIds, sort);
                }

                foreach (System.Object[] objt in res)
                {
                    if (objectValues.Length > 0) objectValues += ";";
                    objectValues += objt[2] + "_" + objt[3] + "_" + objt[5] + "_" + objt[6];
                }
                result = objectValues;

            }
            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCompetitorsByIndustry()
        {
            string ids = Request["ids"];
            string clientCompany = (string)Session["ClientCompany"];
            ClientCompany company = ClientCompanyService.GetById(clientCompany);
            IList<Competitor> competitorList;

            if (!string.IsNullOrEmpty(ids))
            {
                string[] idsStr = ids.Split(';');
                decimal[] industryIds = new decimal[idsStr.Length];
                for (int i = 0; i < idsStr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(idsStr[i]))
                    {
                        industryIds[i] = decimal.Parse(idsStr[i]);
                    }
                }
                competitorList = CompetitorService.GetByIndustriesAndProducts(industryIds, clientCompany);
            }
            else
            {
                competitorList = null;
            }
            if (competitorList == null || competitorList.Count == 0)
            {
                competitorList = new List<Competitor>();
            }
            IList<Competitor> orderedList = competitorList.OrderBy(x =>
                !(x.Name.ToUpper().Equals(company.Dns.ToUpper()) || x.Name.ToUpper().Equals(company.Name.ToUpper())
                                                      )).ToList();
            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(orderedList, "Id", "Name");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsByCompetitor()
        {
            string ids = Request["ids"];///INDUSTRIES ID
            string cps = Request["cps"];///COMPETITORS ID
            string clientCompany = (string)Session["ClientCompany"];
            
            IList<ProductWithCriteriaValuesView> productListView = new List<ProductWithCriteriaValuesView>();

            if (!string.IsNullOrEmpty(ids) && !string.IsNullOrEmpty(cps))
            {
                /// GET THE IDS TO INDUSTRIES
                string[] idsStr = ids.Split(';');
                decimal[] industryIds = new decimal[idsStr.Length];
                for (int i = 0; i < idsStr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(idsStr[i]))
                    {
                        industryIds[i] = decimal.Parse(idsStr[i]);
                    }
                }
                /// GET THE IDS TO COMPETITORS
                string[] cpsStr = cps.Split(';');
                decimal[] competitorIds = new decimal[cpsStr.Length];
                for (int c = 0; c < cpsStr.Length; c++)
                {
                    if (!string.IsNullOrEmpty(cpsStr[c]))
                    {
                        competitorIds[c] = decimal.Parse(cpsStr[c]);
                    }
                }
                productListView = ProductService.GetByIndustriesAndCompetitorsView(industryIds, competitorIds);
               // competitorList = CompetitorService.GetByIndustriesAndProducts(industryIds, clientCompany);
            }
            else
            {
                productListView = null;
            }

             
            //IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(clientCompanyId);
            //bool DefaultsFeaturesTab = false;
            //if (configurations != null)
            //{
            //    if (configurations.Count > 0)
            //    {
            //        DefaultsFeaturesTab = Convert.ToBoolean(configurations[0].FeaturesTab);
            //    }
            //}

            if (productListView == null || productListView.Count == 0)
            {
                productListView = new List<ProductWithCriteriaValuesView>();
            }
            return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "HaveProductCriteria");
            //if (DefaultsFeaturesTab)
            //{
            //    return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "HaveProductCriteria");
            //}
            //else
            //{
            //    return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "ColumnDefault");
            //}
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Event()
        {
            ViewData["ReportModule"] = ReportModule.Event;

            return View("IndexEvent");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ListEvent()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            string reportModule = Request["ReportModule"];
            IList<Report> reportList = ReportService.GetByModule(reportModule);
            IList<ResourceObject> reportGroup = GetGroupByModule(reportList);
            ViewData["ReportGroup"] = reportGroup;
            reportList = CombineListAndGroup(reportList, reportGroup);
            ViewData["ReportList"] = reportList;
            ViewData["ReportModule"] = Request["ReportModule"];
            //ReorderAndGroupReports(reportList);
            return View(reportList);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FilterEvent()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;

            ViewData["ReportFilter"] = Request["ReportFilter"];
            string browseId = Request["ReportFilter"];

            ViewData["ReportTitle"] = Request["ReportTitle"];
            ViewData["ReportModule"] = Request["ReportModule"];
            string properties = string.Empty;
            BrowseObject browseObject = ReportManager.GetInstance().GetBrowseObject(browseId);
            for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
            {
                if (!browseObject.BrowseColumns[i].Hidden)
                {
                    if (browseObject.BrowseColumns[i].Filter)
                    {
                        if (!string.IsNullOrEmpty(properties))
                        {
                            properties += ",";
                        }
                        if (browseObject.BrowseColumns[i].ColumnType == "Date")
                        {
                            properties += browseObject.BrowseColumns[i].ColumnTypeClass + ":" + browseObject.BrowseColumns[i].Property;
                        }
                        else if (browseObject.BrowseColumns[i].ColumnTypeClass.ToString().Equals("Standard"))
                        {
                            properties += browseObject.BrowseColumns[i].ColumnType + ":" + browseObject.BrowseColumns[i].Property;
                        }
                        else
                        {
                            properties += "Single:" + browseObject.BrowseColumns[i].Property;
                        }
                    }
                }
            }
            ViewData["PropertiesOfBrowseId"] = properties;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateEvent(FormCollection collection)
        {
            string browseId = Request["BrowseId"];
            string filterCriteria = Request["FilterCriteria"];
            string reportTitle = Request["Title"];
            string hidencColumnCriteria = Request["HiddenColumnCriteria"];

            ResourceDataManager.GetInstance().CleanNumberGroup();

            BrowseObject browseObject = GetBrowseObjectForQuery(browseId, filterCriteria);
            IList dataSourceObjects = ReportService.GetData(browseObject);
            string reportFilter = GetFilterEventCriteria(filterCriteria, browseObject);
            string reportHiddenColumn = GetHiddenColumnCriteria(hidencColumnCriteria);
            IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();

            IDictionary<string, string> columnDictionary = new Dictionary<string, string>();

            reportParameters["BrowseObject"] = browseObject;
            reportParameters["DataSource"] = dataSourceObjects;
            UserProfile userProfile = UserManager.GetInstance().GetUserProfile(Session["UserId"].ToString());
            reportParameters["UserId"] = userProfile.Name;
            ClientCompany clientCompany = ClientCompanyService.GetById(Session["ClientCompany"].ToString());
            reportParameters["ClientCompany"] = clientCompany.Name;
            reportParameters["ReportTitle"] = reportTitle;
            reportParameters["ReportFilter"] = reportFilter;

            //reportParameters["ReportColumn"] = reportHiddenColumn;
            string[] filterHCCriteriaArray = hidencColumnCriteria.Split(':');

            bool isColumnHidden;
            for (int b = 0; b < browseObject.BrowseColumns.Count; b++)
            {
                if (browseObject.BrowseColumns[b].Filter)
                {
                    isColumnHidden = false;
                    for (int p = 0; p < filterHCCriteriaArray.Length; p++)
                    {
                        string[] operators = filterHCCriteriaArray[p].Split('_');
                        string[] fields = operators[0].Split('.');
                        if ((operators.Length == 3) && (browseObject.BrowseColumns[b].Property == fields[1]))
                        {
                            isColumnHidden = true;
                            p = filterHCCriteriaArray.Length;
                            if (operators[2].ToString().Equals("false"))
                            {
                                if (!columnDictionary.ContainsKey(fields[1].ToString() + "HiddenColumn"))
                                {
                                    columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "true");
                                    reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
                                }
                            }
                            else if (operators[2].ToString().Equals("true"))
                            {
                                if (!columnDictionary.ContainsKey(fields[1].ToString() + "HiddenColumn"))
                                {
                                    columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "false");
                                    reportParameters[fields[1].ToString() + "HiddenColumn"] = "false";
                                }
                            }
                        }
                    }
                    if (!isColumnHidden)
                    {
                        columnDictionary.Add(browseObject.BrowseColumns[b].Property.ToString() + "HiddenColumn", "false");
                        reportParameters[browseObject.BrowseColumns[b].Property + "HiddenColumn"] = "false";
                    }
                }
            }

            reportParameters["ColumnDictionary"] = columnDictionary;
            string reportFile = ReportHelper.PrintReport(reportParameters);

            return Content(reportFile);
        }



        private string GetFilterEventCriteria(string filterCriteria, BrowseObject browseObject)
        {
            string[] filterCriteriaArray = filterCriteria.Split(':');
            filterCriteria = "";

            foreach (string filter in filterCriteriaArray)
            {
                string[] operators = filter.Split('_');
                if ((operators.Length == 3) && (!string.IsNullOrEmpty(operators[2])))
                {
                    string[] fields = operators[0].Split('.');
                    string filterColumn = fields[1];
                    string filterValue = operators[2];
                    string stringOperator = ResourceDataManager.GetInstance().GetLabel("FilterOperator", operators[1]);
                    string operatorTempo = stringOperator;
                    stringOperator = (string)HttpContext.GetGlobalResourceObject("LabelResource", "FilterOperator" + stringOperator);
                    if (string.IsNullOrEmpty(stringOperator))
                    {
                        stringOperator = operatorTempo;
                    }
                    for (int i = 0; i < browseObject.BrowseColumns.Count; i++)
                    {
                        if ((operators[0] == browseObject.BrowseColumns[i].Column) && (!browseObject.BrowseColumns[i].Hidden))
                        {
                            filterColumn = browseObject.BrowseColumns[i].Label;
                            if (browseObject.BrowseColumns[i].ColumnTypeClass.Equals(Compelligence.Common.Browse.BrowseColumn.TypeClass.Standard))
                            {
                                if ((!string.IsNullOrEmpty(browseObject.BrowseColumns[i].ColumnType)) && (browseObject.BrowseColumns[i].ColumnType.Equals("EventTypePeriod")) && (browseObject.BrowseColumns[i].Property.Equals("TimeFrame")))
                                {
                                    filterValue = ResourceDataManager.GetInstance().GetLabel(browseObject.BrowseColumns[i].ColumnType.ToString(), operators[2]);
                                    string valueStartDate = string.Empty;
                                    string nameStartDate = string.Empty;
                                    string valueEndDate = string.Empty;
                                    string nameEndDate = string.Empty;
                                    for (int j = 0; j < filterCriteriaArray.Length; j++)
                                    {
                                        string[] newoperators = filterCriteriaArray[j].Split('_');
                                        if ((newoperators.Length == 3) && (!string.IsNullOrEmpty(newoperators[2])))
                                        {
                                            string[] newfields = newoperators[0].Split('.');
                                            string newfilterColumn = newfields[1];
                                            //
                                            if (newfilterColumn.Equals("StartDate") || newfilterColumn.Equals("EndDate"))
                                            {
                                                string newfilterValue = newoperators[2];
                                                string newstringOperator = ResourceDataManager.GetInstance().GetLabel("FilterOperator", newoperators[1]);
                                                string newoperatorTempo = newstringOperator;
                                                newstringOperator = (string)HttpContext.GetGlobalResourceObject("LabelResource", "FilterOperator" + newstringOperator);

                                                if (string.IsNullOrEmpty(newstringOperator))
                                                {
                                                    newstringOperator = newoperatorTempo;
                                                }
                                                for (int m = 0; m < browseObject.BrowseColumns.Count; m++)
                                                {
                                                    if ((newoperators[0] == browseObject.BrowseColumns[m].Column) && (browseObject.BrowseColumns[m].Hidden))
                                                    {
                                                        if (browseObject.BrowseColumns[m].Property.Equals("StartDate"))
                                                        {
                                                            nameStartDate = browseObject.BrowseColumns[m].Label;
                                                            valueStartDate = newoperators[2];
                                                            m = browseObject.BrowseColumns.Count;
                                                        }
                                                        else if (browseObject.BrowseColumns[m].Property.Equals("EndDate"))
                                                        {
                                                            nameEndDate = browseObject.BrowseColumns[m].Label;
                                                            valueEndDate = newoperators[2];
                                                            m = browseObject.BrowseColumns.Count;
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(valueEndDate))
                                    {
                                        if (!string.IsNullOrEmpty(valueStartDate))
                                        {
                                            filterValue += " with all Events between " + valueStartDate + " and " + valueEndDate;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(valueStartDate))
                                        {
                                            filterValue += " with all events past " + nameStartDate + ": " + valueStartDate;
                                        }
                                    }
                                }
                                else
                                {
                                    filterValue = ResourceDataManager.GetInstance().GetLabel(browseObject.BrowseColumns[i].ColumnType.ToString(), operators[2]);
                                }
                            }
                            filterCriteria += filterColumn + " " + stringOperator + " " + filterValue + "\n";
                        }
                    }


                }
            }

            if (string.IsNullOrEmpty(filterCriteria))
            {
                filterCriteria = "Print All";
            }

            return filterCriteria;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult PreViewReport()
        {
            return null;
        }

        public ActionResult ReportViewer(string Scope, string BrowseId, string Title, string FilterCriteria, string HiddenColumnCriteria)
        {
            ResourceDataManager.GetInstance().CleanNumberGroup();
            BrowseObject browseObject = GetBrowseObjectForQuery(BrowseId, FilterCriteria);
            IList dataSourceObjects = ReportService.GetData(browseObject);
            string reportFilter = GetFilterCriteria(FilterCriteria, browseObject);
            string reportHiddenColumn = GetHiddenColumnCriteria(HiddenColumnCriteria);
            IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();
            IDictionary<string, string> columnDictionary = new Dictionary<string, string>();
            reportParameters["BrowseObject"] = browseObject;
            reportParameters["DataSource"] = dataSourceObjects;
            UserProfile userProfile = UserManager.GetInstance().GetUserProfile(Session["UserId"].ToString());
            reportParameters["UserId"] = userProfile.Name;
            ClientCompany clientCompany = ClientCompanyService.GetById(Session["ClientCompany"].ToString());
            reportParameters["ClientCompany"] = clientCompany.Name;
            reportParameters["ReportTitle"] = Title;
            reportParameters["ReportFilter"] = reportFilter;
            string url = GetPathToImages(clientCompany.ClientCompanyId);
            string[] filterHCCriteriaArray = HiddenColumnCriteria.Split(':');
            foreach (string hiddenColumn in filterHCCriteriaArray)
            {
                string[] operators = hiddenColumn.Split('_');
                string[] fields = operators[0].Split('.');
                if ((operators.Length == 3) && (operators[2].ToString().Equals("false")))
                {
                    if (IsNotKey(columnDictionary, fields[1].ToString() + "HiddenColumn"))
                    {
                        columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "true");
                        reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
                    }
                }
                else
                {
                    if (IsNotKey(columnDictionary, fields[1].ToString() + "HiddenColumn"))
                    {
                        columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "false");
                        reportParameters[fields[1].ToString() + "HiddenColumn"] = "false";
                    }
                }
            }
            reportParameters["ColumnDictionary"] = columnDictionary;
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = ReportHelper.PrintReportViewer(reportParameters);
            reportViewer.LocalReport.AddTrustedCodeModuleInCurrentAppDomain("Compelligence.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Session["LocalReport"] = reportViewer.LocalReport;
            Session["ReportViewer"] = reportViewer;
            Session["DataSourceObjects"] = dataSourceObjects;
            Session["BrowseObject"] = browseObject;
            Server.Transfer("~/Views/Report/PreViewReport.aspx ?Scope=" + Scope + "&BrowseId=" + BrowseId + "&Title=" + Title + "&FilterCriteria =" + FilterCriteria + "&HiddenColumnCriteria=" + HiddenColumnCriteria + "&UserId=" + userProfile.Name + "&ClientCompany=" + clientCompany.Name + "&ReportFilter=" + reportFilter + "&UrlPath=" + url);
            return new EmptyResult();
        }

        public ActionResult DynReport()
        {
            DataTable customerDataTable = GetCustomerData("CustomerDataTable");
            System.Data.DataSet customerData = customerDataTable.DataSet;

            customerData.DataSetName = "CustomerData";

            Rdlc report = new Rdlc(customerData);
            Session["reportContainer"] = report;
            Session["customerData"] = customerData;
            Session["customerDataTable"] = customerDataTable;

            Server.Transfer("~/Views/Report/DynViewReport.aspx");
            return new EmptyResult();
        }
        //simulate a database call that reutns a datatable
        private DataTable GetCustomerData(string datatablename)
        {
            DataSet newDataSet = new DataSet();
            DataTable returnValue = new DataTable();
            returnValue.TableName = datatablename;
            newDataSet.Tables.Add(returnValue);

            //create the columns
            returnValue.Columns.Add("Id");
            returnValue.Columns.Add("FirstName");
            returnValue.Columns.Add("LastName");

            //add some data
            returnValue.Rows.Add(new object[] { 1, "Clark", "Kent" });
            returnValue.Rows.Add(new object[] { 2, "Lois", "Lane" });
            returnValue.Rows.Add(new object[] { 3, "Bruce", "Wayne" });

            //return the data
            return returnValue;
        }

        public ActionResult DynICReport(string title, string reportId, string ItemsA, string ItemsB, string FilterValueA, string FilterValueB)
        {
            if (!string.IsNullOrEmpty(ItemsA) && !string.IsNullOrEmpty(ItemsB))
            {
                DataTable customerDataTable = GetIndustryAndCompetitorData("CustomerDataTable", reportId, ItemsA, ItemsB, FilterValueA, FilterValueB);
                System.Data.DataSet customerData = customerDataTable.DataSet;
                string tempo = Request["Items"];
                customerData.DataSetName = "CustomerData";

                Rdlc report = new Rdlc(customerData);
                Session["reportContainer"] = report;
                Session["customerData"] = customerData;
                Session["customerDataTable"] = customerDataTable;

                Server.Transfer("~/Views/Report/DynViewReport.aspx?Title=" + title);
            }
            return new EmptyResult();
        }

        public ActionResult DynICReportFinancial(string title, string reportId, string Items, string FilterValue, string financial, string periodType, string periodValue, string year)
        {
            if (!string.IsNullOrEmpty(Items))
            {
                DataTable customerDataTable = GetFinancialDataByCompetitor("CustomerDataTable", reportId, Items, FilterValue, financial, periodType, periodValue, year);
                System.Data.DataSet customerData = customerDataTable.DataSet;
                string tempo = Request["Items"];
                customerData.DataSetName = "CustomerData";

                Rdlc report = new Rdlc(customerData);
                Session["reportContainer"] = report;
                Session["customerData"] = customerData;
                Session["customerDataTable"] = customerDataTable;

                Server.Transfer("~/Views/Report/DynViewReport.aspx?Title=" + title);
            }
            return new EmptyResult();
        }

        private DataTable GetIndustryAndCompetitorData(string datatablename, string reportId, string itemsA, string itemsB, string filterValueA, string filterValueB)
        {
            DataSet newDataSet = new DataSet();
            DataTable returnValue = new DataTable();
            returnValue.TableName = datatablename;
            newDataSet.Tables.Add(returnValue);

            string clientCompany = Session["ClientCompany"].ToString();
            IList<IndustryByHierarchyView> industries = IndustryService.FindAllActiveByHierarchy(clientCompany);
            industries = GetIndustryList(industries, itemsA, filterValueA);
            IList<Competitor> competitors = CompetitorService.GetAllByClientCompany(clientCompany);
            competitors = GetCompetitorList(competitors, itemsB, filterValueB);
            returnValue.Columns.Add("Industry_Competitor");
            foreach (Competitor competitor in competitors)
            {
                competitor.Name = System.Text.RegularExpressions.Regex.Replace(competitor.Name, "[^a-zA-Z0-9]", "_");
                if (competitor.Name.IndexOf("0") == 0 || competitor.Name.IndexOf("1") == 0 || competitor.Name.IndexOf("2") == 0 || competitor.Name.IndexOf("3") == 0 || competitor.Name.IndexOf("4") == 0 || competitor.Name.IndexOf("5") == 0 || competitor.Name.IndexOf("6") == 0 || competitor.Name.IndexOf("7") == 0 || competitor.Name.IndexOf("8") == 0 || competitor.Name.IndexOf("9") == 0)
                {
                    competitor.Name = "COMPETITOR_" + competitor.Name;
                }
                while (returnValue.Columns.Contains(competitor.Name))
                {
                    competitor.Name = competitor.Name + "_COPY";
                }
                returnValue.Columns.Add(competitor.Name);
            }
            IList<IndustryByHierarchyView> industryFormatName = new List<IndustryByHierarchyView>();
            foreach (IndustryByHierarchyView industry in industries)
            {
                while (industryFormatName.Contains(industry))
                {
                    industry.Name = industry.Name + "_COPY";
                }
                industryFormatName.Add(industry);
            }
            industries = industryFormatName;
            for (int i = 0; i < industries.Count; i++)
            {
                string data = string.Empty;
                data = industries[i].Name.Replace(",", "_");
                foreach (Competitor searchCompetitor in competitors)
                {
                    IndustryCompetitorId industryCompetitorId = new IndustryCompetitorId(industries[i].Id, searchCompetitor.Id);
                    IndustryCompetitor industryCompetitor = IndustryCompetitorService.GetById(industryCompetitorId);
                    if (industryCompetitor != null)
                    {
                        if (reportId.Equals(DynamicReportKey.IndustryAndCompetitorProduct))
                        {
                            string productsName = string.Empty;
                            IList<Product> productList = ProductService.GetByIndustryIdAndCompetitorId(industryCompetitor.Id.IndustryId, industryCompetitor.Id.CompetitorId, clientCompany);
                            if (productList != null && productList.Count > 0)
                            {
                                foreach (Product product in productList)
                                {
                                    if (!string.IsNullOrEmpty(productsName))
                                    {
                                        productsName += System.Environment.NewLine;
                                    }
                                    productsName += product.Name;
                                }
                            }
                            if (!string.IsNullOrEmpty(data))
                            {
                                data += ";";
                            }
                            data += productsName;
                        }
                        else if (reportId.Equals(DynamicReportKey.IndustryAndCompetitor))
                        {
                            if (!string.IsNullOrEmpty(data))
                            {
                                data += ";";
                            }
                            data += "Yes";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(data))
                        {
                            data += ";";
                        }
                        data += "No";
                    }
                }
                object[] row = data.Split(';');
                returnValue.Rows.Add(row);
                data = string.Empty;
            }
            return returnValue;
        }

        private String[] GetBalanceSheetTitles()
        {
            String[] data = new String[35];

            for (int i = 0; i < 35; i++)
            {
                data[i] = BalanceSheetItems.Value[i];
            }

            return data;
        }

        private String[] GetCashFlowTitles()
        {
            String[] data = new String[20];

            for (int i = 0; i < 20; i++)
            {
                data[i] = CashFlowItems.Value[i];
            }

            return data;
        }

        private String[] GetIncomeStatementTitles()
        {
            String[] data = new String[24];

            for (int i = 0; i < 24; i++)
            {
                data[i] = IncomeStatementItems.Value[i];
            }

            return data;
        }

        void updateData(string[] data, CompetitorFinancialBalanceSheet cfbs)
        {
            data[0] += ";" + cfbs.PeriodEnding;
            data[1] += ";" + cfbs.CashAndCashEquivalents;
            data[2] += ";" + cfbs.ShortTermInvestments;
            data[3] += ";" + cfbs.NetReceivables;
            data[4] += ";" + cfbs.Inventory;
            data[5] += ";" + cfbs.OtherCurrentAssets;
            data[6] += ";" + cfbs.TotalCurrentAssets;
            data[7] += ";" + cfbs.LongTermInvestments;
            data[8] += ";" + cfbs.PropertyPlantandEquipment;
            data[9] += ";" + cfbs.Goodwill;
            data[10] += ";" + cfbs.IntangibleAssets;
            data[11] += ";" + cfbs.AccumulatedAmortization;
            data[12] += ";" + cfbs.OtherAssets;
            data[13] += ";" + cfbs.DeferredLongTermAssetCharges;
            data[14] += ";" + cfbs.TotalAssets;
            data[15] += ";" + cfbs.AccountsPayable;
            data[16] += ";" + cfbs.ShortCurrentLongTermDebt;
            data[17] += ";" + cfbs.OtherCurrentLiabilities;
            data[18] += ";" + cfbs.TotalCurrentLiabilities;
            data[19] += ";" + cfbs.LongTermDebt;
            data[20] += ";" + cfbs.OtherLiabilities;
            data[21] += ";" + cfbs.DeferredLongTermLiabilityCharges;
            data[22] += ";" + cfbs.MinorityInterest;
            data[23] += ";" + cfbs.NegativeGoodwill;
            data[24] += ";" + cfbs.TotalLiabilities;
            data[25] += ";" + cfbs.MiscStocksOptionsWarrants;
            data[26] += ";" + cfbs.RedeemablePreferredStock;
            data[27] += ";" + cfbs.PreferredStock;
            data[28] += ";" + cfbs.CommonStock;
            data[29] += ";" + cfbs.RetainedEarnings;
            data[30] += ";" + cfbs.TreasuryStock;
            data[31] += ";" + cfbs.CapitalSurplus;
            data[32] += ";" + cfbs.OtherStockholderEquity;
            data[33] += ";" + cfbs.TotaStockholderEquity;
            data[34] += ";" + cfbs.NetTangibleAssets;
        }

        void updateData(string[] data, CompetitorFinancialCashFlow cfcf)
        {
            data[0] += ";" + cfcf.PeriodEnding;
            data[1] += ";" + cfcf.NetIncome;
            data[2] += ";" + cfcf.Depreciation;
            data[3] += ";" + cfcf.AdjustmentsToNetIncome;
            data[4] += ";" + cfcf.ChangesInAccountsReceivables;
            data[5] += ";" + cfcf.ChangesInLiabilities;
            data[6] += ";" + cfcf.ChangesInInventories;
            data[7] += ";" + cfcf.ChangesInOtherOperatingActivities;
            data[8] += ";" + cfcf.TotalCashFlowFromOperatingActivities;
            data[9] += ";" + cfcf.CapitalExpenditures;
            data[10] += ";" + cfcf.Investments;
            data[11] += ";" + cfcf.OtherCashflowsfromInvestingActivities;
            data[12] += ";" + cfcf.TotalCashFlowsFromInvestingActivities;
            data[13] += ";" + cfcf.DividendsPaid;
            data[14] += ";" + cfcf.SalePurchaseofStock;
            data[15] += ";" + cfcf.NetBorrowings;
            data[16] += ";" + cfcf.OtherCashFlowsfromFinancingActivities;
            data[17] += ";" + cfcf.TotalCashFlowsFromFinancingActivities;
            data[18] += ";" + cfcf.EffectOfExchangeRateChanges;
            data[19] += ";" + cfcf.ChangeInCashandCashEquivalents;
        }

        void updateData(string[] data, CompetitorFinancialIncomeStatement cfis)
        {
            data[0] += ";" + cfis.PeriodEnding;
            data[1] += ";" + cfis.TotalRevenue;
            data[2] += ";" + cfis.CostofRevenue;
            data[3] += ";" + cfis.GrossProfit;
            data[4] += ";" + cfis.ResearchDevelopment;
            data[5] += ";" + cfis.SellingGeneralandAdministrative;
            data[6] += ";" + cfis.NonRecurring;
            data[7] += ";" + cfis.Others;
            data[8] += ";" + cfis.TotalOperatingExpenses;
            data[9] += ";" + cfis.OperatingIncomeorLoss;
            data[10] += ";" + cfis.TotalOtherIncomeExpensesNet;
            data[11] += ";" + cfis.EarningsBeforeInterestAndTaxes;
            data[12] += ";" + cfis.InterestExpense;
            data[13] += ";" + cfis.IncomeBeforeTax;
            data[14] += ";" + cfis.IncomeTaxExpense;
            data[15] += ";" + cfis.MinorityInterest;
            data[16] += ";" + cfis.NetIncomeFromContinuingOps;
            data[17] += ";" + cfis.DiscontinuedOperations;
            data[18] += ";" + cfis.ExtraordinaryItems;
            data[19] += ";" + cfis.EffectOfAccountingChanges;
            data[20] += ";" + cfis.OtherItems;
            data[21] += ";" + cfis.NetIncome;
            data[22] += ";" + cfis.PreferredStockAndOtherAdjustments;
            data[23] += ";" + cfis.NetIncomeApplicableToCommonShares;
        }

        void updateData(string[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] += ";" + "-";
            }
        }

        private DataTable GetFinancialDataByCompetitor(string datatablename, string reportId, string items, string filterValue, string financial, string periodType, string periodValue, string year)
        {
            DataSet newDataSet = new DataSet();
            DataTable returnValue = new DataTable();
            returnValue.TableName = datatablename;
            newDataSet.Tables.Add(returnValue);

            string clientCompany = Session["ClientCompany"].ToString();
            IList<Competitor> competitors = CompetitorService.GetAllByClientCompany(clientCompany);
            competitors = GetCompetitorList(competitors, items, filterValue);
            returnValue.Columns.Add("Financial_Competitor");

            IList<CompetitorFinancialBalanceSheet> cfbs;
            IList<CompetitorFinancialIncomeStatement> cfis;
            IList<CompetitorFinancialCashFlow> cfcf;

            string[] data = null;
            if (financial == "BalanceSheet")
            {
                data = GetBalanceSheetTitles();
            }
            else if (financial == "CashFlow")
            {
                data = GetCashFlowTitles();
            }
            else if (financial == "IncomeStatement")
            {
                data = GetIncomeStatementTitles();
            }

            foreach (Competitor competitor in competitors)
            {
                competitor.Name = System.Text.RegularExpressions.Regex.Replace(competitor.Name, "[^a-zA-Z0-9]", "_");
                if (competitor.Name.IndexOf("0") == 0 || competitor.Name.IndexOf("1") == 0 || competitor.Name.IndexOf("2") == 0 || competitor.Name.IndexOf("3") == 0 || competitor.Name.IndexOf("4") == 0 || competitor.Name.IndexOf("5") == 0 || competitor.Name.IndexOf("6") == 0 || competitor.Name.IndexOf("7") == 0 || competitor.Name.IndexOf("8") == 0 || competitor.Name.IndexOf("9") == 0)
                {
                    competitor.Name = "COMPETITOR_" + competitor.Name;
                }
                while (returnValue.Columns.Contains(competitor.Name))
                {
                    competitor.Name = competitor.Name + "_COPY";
                }
                returnValue.Columns.Add(competitor.Name);

                if (financial == "BalanceSheet")
                {
                    cfbs = CompetitorFinancialService.GetFinancialBalanceSheetPerformance(competitor.Id, periodType, periodValue, year, clientCompany);
                    if (cfbs.Count != 0)
                    {
                        updateData(data, cfbs[0]);
                    }
                    else
                        updateData(data);
                }
                else if (financial == "CashFlow")
                {
                    cfcf = CompetitorFinancialService.GetFinancialCashFlowPerformance(competitor.Id, periodType, periodValue, year, clientCompany);
                    if (cfcf.Count != 0)
                    {
                        updateData(data, cfcf[0]);
                    }
                    else
                        updateData(data);
                }
                else if (financial == "IncomeStatement")
                {
                    cfis = CompetitorFinancialService.GetFinancialIncomeStatementPerformance(competitor.Id, periodType, periodValue, year, clientCompany);
                    if (cfis.Count != 0)
                    {
                        updateData(data, cfis[0]);
                    }
                    else
                        updateData(data);
                }
            }
            for (int i = 0; i < data.Length; i++)
            {
                object[] row = data[i].Split(';');
                returnValue.Rows.Add(row);
            }
            return returnValue;
        }

        static DataTable GetTable()
        {
            //
            // Here we create a DataTable with four columns.
            //
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            //
            // Here we add five DataRows.
            //
            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            return table;
        }

        public IList<IndustryByHierarchyView> GetIndustryList(IList<IndustryByHierarchyView> list, string filterString, string filterValue)
        {
            IList<IndustryByHierarchyView> result = new List<IndustryByHierarchyView>();
            string[] industryIds = filterString.Split(',');
            string[] filterValues = filterValue.Split(':');
            foreach (string id in industryIds)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    decimal industryId = decimal.Parse(id);
                    foreach (IndustryByHierarchyView industry in list)
                    {
                        if (industry.Id == industryId)
                        {
                            bool condition = true;
                            if (!string.IsNullOrEmpty(filterValue))
                            {
                                foreach (string filter in filterValues)
                                {

                                    if (!string.IsNullOrEmpty(filter))
                                    {
                                        string[] operators = filter.Split('_');
                                        if (operators.Length == 3)
                                        {
                                            if (operators[0] == "IndustryStatus")
                                            {
                                                if (operators[1] == "Eq")
                                                {
                                                    if (!industry.Status.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                                else if (operators[1] == "Ne")
                                                {
                                                    if (industry.Status.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                            }
                                            else if (operators[0] == "IndustryTier")
                                            {
                                                if (operators[1] == "Eq")
                                                {
                                                    if (!industry.Tier.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                                else if (operators[1] == "Ne")
                                                {
                                                    if (industry.Tier.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (condition)
                            {
                                result.Add(industry);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public IList<Competitor> GetCompetitorList(IList<Competitor> list, string filterString, string filterValue)
        {
            IList<Competitor> result = new List<Competitor>();
            string[] competitorIds = filterString.Split(',');
            string[] filterValues = filterValue.Split(':');
            foreach (string id in competitorIds)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    decimal competitorId = decimal.Parse(id);
                    foreach (Competitor competitor in list)
                    {
                        if (competitor.Id == competitorId)
                        {
                            bool condition = true;
                            if (!string.IsNullOrEmpty(filterValue))
                            {
                                foreach (string filter in filterValues)
                                {

                                    if (!string.IsNullOrEmpty(filter))
                                    {
                                        string[] operators = filter.Split('_');
                                        if (operators.Length == 3)
                                        {
                                            if (operators[0] == "CompetitorStatus")
                                            {
                                                if (operators[1] == "Eq")
                                                {
                                                    if (!competitor.Status.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                                else if (operators[1] == "Ne")
                                                {
                                                    if (competitor.Status.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                            }
                                            else if (operators[0] == "CompetitorTier")
                                            {
                                                if (operators[1] == "Eq")
                                                {
                                                    if (!competitor.Tier.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                                else if (operators[1] == "Ne")
                                                {
                                                    if (competitor.Tier.Equals(operators[2]))
                                                    {
                                                        condition = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (condition)
                            {
                                result.Add(competitor);
                            }
                        }
                    }
                }
            }

            return result;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ListDynamic()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            string reportModule = Request["ReportModule"];
            IList<Report> reportList = ReportService.GetByModule(reportModule);
            ViewData["ReportList"] = reportList;
            ViewData["ReportModule"] = Request["ReportModule"];
            return View(reportList);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FilterDynamic()
        {
            ViewData["Scope"] = Request["Scope"];
            ViewData["Container"] = Request["Container"];
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;

            ViewData["ReportTitle"] = Request["ReportTitle"];
            ViewData["ReportModule"] = Request["ReportModule"];
            ViewData["ReportId"] = Request["ReportId"];
            ViewData["EntityRow"] = Request["EntityRow"];
            ViewData["EntityColumn"] = Request["EntityColumn"];
            string clientCompany = Session["ClientCompany"].ToString();
            IList<IndustryByHierarchyView> industries = IndustryService.FindAllIndustryHierarchy(clientCompany);
            ViewData["Industries"] = new SelectList(industries, "Id", "Name"); ;
            IList<Competitor> competitors = CompetitorService.GetAllOrderByName(clientCompany);

            ViewData["Competitors"] = new SelectList(competitors, "Id", "Name");
            IList<ResourceObject> industryStatusList = ResourceService.GetAll<IndustryStatus>();
            ViewData["IndustryStatusList"] = new SelectList(industryStatusList, "Id", "Value");
            IList<ResourceObject> industryTierList = ResourceService.GetAll<IndustryTier>();
            ViewData["IndustryTierList"] = new SelectList(industryTierList, "Id", "Value");
            IList<ResourceObject> competitorStatusList = ResourceService.GetAll<CompetitorStatus>();
            ViewData["CompetitorStatusList"] = new SelectList(competitorStatusList, "Id", "Value");
            IList<ResourceObject> competitorTierList = ResourceService.GetAll<CompetitorTier>();
            ViewData["CompetitorTierList"] = new SelectList(competitorTierList, "Id", "Value");
            IList<ResourceObject> filterOperator = ResourceService.GetAll<FilterOperatorStandardData>();
            ViewData["FilterOperator"] = new SelectList(filterOperator, "Id", "Value");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FilterDynamicFinancial()
        {
            ViewData["ReportTitle"] = Request["ReportTitle"];
            ViewData["ReportModule"] = Request["ReportModule"];
            ViewData["ReportId"] = Request["ReportId"];
            ViewData["EntityRow"] = Request["EntityRow"];
            ViewData["EntityColumn"] = Request["EntityColumn"];
            string clientCompany = Session["ClientCompany"].ToString();
            IList<IndustryByHierarchyView> industries = IndustryService.FindAllIndustryHierarchy(clientCompany);
            ViewData["Industries"] = new SelectList(industries, "Id", "Name"); ;
            IList<Competitor> competitors = CompetitorService.GetAllOrderByName(clientCompany);

            ViewData["Competitors"] = new SelectList(competitors, "Id", "Name");
            IList<ResourceObject> financialViewList = ResourceService.GetAll<FinancialView>();
            ViewData["FinancialViewList"] = new SelectList(financialViewList, "Value", "Id");
            IList<ResourceObject> financialPeriodTypeList = ResourceService.GetAll<FinancialPeriodType>();
            ViewData["FinancialPeriodTypeList"] = new SelectList(financialPeriodTypeList, "Id", "Value");
            IList<ResourceObject> financialPeriodValueList = ResourceService.GetAll<FinancialPeriodValue>();
            ViewData["FinancialPeriodValueList"] = new SelectList(financialPeriodValueList, "Id", "Value");
            IList<ResourceObject> competitorStatusList = ResourceService.GetAll<CompetitorStatus>();
            ViewData["CompetitorStatusList"] = new SelectList(competitorStatusList, "Id", "Value");
            IList<ResourceObject> competitorTierList = ResourceService.GetAll<CompetitorTier>();
            ViewData["CompetitorTierList"] = new SelectList(competitorTierList, "Id", "Value");
            IList<ResourceObject> filterOperator = ResourceService.GetAll<FilterOperatorStandardData>();
            ViewData["FilterOperator"] = new SelectList(filterOperator, "Id", "Value");
            return View();
        }

        public bool IsNotKey(IDictionary<string, string> dictionary, string key)
        {
            bool iskey = true;
            foreach (KeyValuePair<string, string> par in dictionary)
            {
                if (par.Key.Equals(key))
                {
                    iskey = false;
                }
            }

            return iskey;
        }

        public string GetPathToImages(string clientCompanyId)
        {
            string url = string.Empty;
            string fileContent = string.Empty;
            string pathImages = ConfigurationSettings.AppSettings["ImageFilePath"].Substring(1);
            ClientCompany clientCompany = ClientCompanyService.GetById(clientCompanyId);
            string contentFile = reportContext;
            string currentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            if (contentFile.IndexOf("wwwroot") != -1)
            {
                if (contentFile.IndexOf("/") != -1)
                {
                    string[] objectsUrl = contentFile.Split('/');
                    if (objectsUrl.Length > 1)
                    {
                        fileContent = objectsUrl[objectsUrl.Length - 1];
                    }
                }

            }

            string[] currentUrlArray = currentUrl.Split('/');
            if (currentUrlArray.Length > 2)
            {
                string currentDomain = currentUrlArray[2];
                if (currentDomain.IndexOf("localhost") == -1)//domain.compelligence.com/exec
                {
                    if (clientCompany != null)
                    {
                        url = currentUrlArray[0] + "//" + currentUrlArray[2] + appPath + fileContent;
                    }
                }
                else //this is localhost by example "http://localhost:5783"
                {
                    url = currentUrlArray[0] + "//" + currentUrlArray[2];
                }
            }
            return url;
        }

        public IList<ResourceObject> GetGroupByModule(IList<Report> reportList)
        {
            IList<ResourceObject> result = new List<ResourceObject>();
            IList<ResourceObject> reportGroup = ResourceService.GetAll<ReportGroup>();
            foreach (ResourceObject resourceObject in reportGroup)
            {
                bool containtModule = false;
                foreach (Report report in reportList)
                {
                    if (report.Group.Equals(resourceObject.Id))
                    {
                        containtModule = true;
                    }
                }
                if (containtModule)
                {
                    result.Add(resourceObject);
                }
            }


            return result;
        }

        public IList<Report> GetListByIndex(IList<Report> reportList, int begin, int end)
        {
            IList<Report> result = new List<Report>();

            for (int i = begin; i < end; i++)
            {
                if (i <= reportList.Count)
                    result.Add(reportList[i]);
            }

            return result;
        }

        public IList<ReportGroupList> GetReportByGroup(IList<Report> reportList, IList<ResourceObject> reportGroup)
        {
            IList<ReportGroupList> result = new List<ReportGroupList>();
            foreach (ResourceObject resourceObject in reportGroup)
            {
                ReportGroupList rgl = new ReportGroupList();
                rgl.Group = resourceObject.Id;
                rgl.HeaderGroup = resourceObject.Value;
                rgl.ReportList = new List<Report>();
                foreach (Report report in reportList)
                {
                    if (report.Group.Equals(resourceObject.Id))
                    {
                        rgl.ReportList.Add(report);
                    }
                }
                result.Add(rgl);
            }
            //foreach (Report report2 in reportList)
            //{
            //    if (string.IsNullOrEmpty(report2.Group))
            //    {
            //        result.Add(report2);
            //    }
            //}
            return result;
        }

        public IList<Report> CombineListAndGroup(IList<Report> reportList, IList<ResourceObject> reportGroup)
        {
            IList<Report> result = new List<Report>();
            foreach (ResourceObject resourceObject in reportGroup)
            {
                Report newReportTitle = new Report();
                newReportTitle.Title = resourceObject.Value;
                newReportTitle.Group = resourceObject.Id;
                result.Add(newReportTitle);
                foreach (Report report in reportList)
                {
                    if (report.Group.Equals(resourceObject.Id))
                    {
                        result.Add(report);
                    }
                }
            }
            foreach (Report report2 in reportList)
            {
                if (string.IsNullOrEmpty(report2.Group))
                {
                    result.Add(report2);
                }
            }
            return result;
        }

        public void ReorderAndGroupReports(IList<Report> reportList)
        {
            int lenghtColumn = 0;
            IList<Report> reportList1 = new List<Report>();
            IList<Report> reportList2 = new List<Report>();
            IList<Report> reportList3 = new List<Report>();
            if (reportList.Count > 20)
            {
                if (reportList.Count % 3 == 0)
                {
                    lenghtColumn = reportList.Count / 3;
                    reportList1 = GetListByIndex(reportList, 0, lenghtColumn);
                    reportList2 = GetListByIndex(reportList, lenghtColumn, lenghtColumn * 2);
                    reportList3 = GetListByIndex(reportList, lenghtColumn * 2, lenghtColumn * 3);
                }
                else if (reportList.Count % 3 == 1)
                {
                    lenghtColumn = (reportList.Count - 1) / 3;
                    reportList1 = GetListByIndex(reportList, 0, lenghtColumn + 1);
                    reportList2 = GetListByIndex(reportList, lenghtColumn + 1, (lenghtColumn * 2) + 1);
                    reportList3 = GetListByIndex(reportList, (lenghtColumn * 2) + 1, (lenghtColumn * 3) + 1);
                }
                else if (reportList.Count % 3 == 2)
                {
                    lenghtColumn = (reportList.Count - 2) / 3;
                    reportList1 = GetListByIndex(reportList, 0, lenghtColumn + 1);
                    reportList2 = GetListByIndex(reportList, lenghtColumn + 1, (lenghtColumn * 2) + 2);
                    reportList3 = GetListByIndex(reportList, (lenghtColumn * 2) + 2, (lenghtColumn * 3) + 2);
                }
            }
            else if (reportList.Count > 10)
            {
                reportList1 = GetListByIndex(reportList, 0, 10);
                reportList2 = GetListByIndex(reportList, 10, reportList.Count);
            }
            else
            {
                reportList1 = reportList;
            }
            ViewData["ReportListColumn1"] = reportList1;
            ViewData["ReportListColumn2"] = reportList2;
            ViewData["ReportListColumn3"] = reportList3;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetIndustryByStatus(string IndustryStatusOperator, string IndustryStatusValue, string IndustryTierOperator, string IndustryTierValue)
        {
            string result = string.Empty;
            string clientCompany = Session["ClientCompany"].ToString();
            IList<IndustryByHierarchyView> industries = new List<IndustryByHierarchyView>();
            if (!string.IsNullOrEmpty(IndustryStatusOperator) && !string.IsNullOrEmpty(IndustryStatusValue) && !string.IsNullOrEmpty(IndustryTierOperator) && !string.IsNullOrEmpty(IndustryTierValue))
            {
                industries = IndustryService.FindAllByHierarchyAndStatusTier(clientCompany, IndustryStatusOperator, IndustryStatusValue, IndustryTierOperator, IndustryTierValue);
            }
            else if (!string.IsNullOrEmpty(IndustryStatusOperator) && !string.IsNullOrEmpty(IndustryStatusValue) && string.IsNullOrEmpty(IndustryTierValue))
            {
                industries = IndustryService.FindAllActiveByHierarchyAndStatus(clientCompany, IndustryStatusValue, IndustryStatusOperator);
            }
            else if (string.IsNullOrEmpty(IndustryStatusValue) && !string.IsNullOrEmpty(IndustryTierOperator) && !string.IsNullOrEmpty(IndustryTierValue))
            {
                industries = IndustryService.FindAllByHierarchyAndTier(clientCompany, IndustryTierValue, IndustryTierOperator);
            }
            else
            {
                industries = IndustryService.FindAllIndustryHierarchy(clientCompany);
            }

            int cont = 0;
            if (industries != null && industries.Count > 0)
            {
                foreach (IndustryByHierarchyView ibhv in industries)
                {
                    cont++;
                    if (industries.Count == cont)
                    {
                        result = result + ibhv.Id + ":" + ibhv.Name;
                    }
                    else
                    {
                        result = result + ibhv.Id + ":" + ibhv.Name + "_";
                    }

                }
            }
            return Content(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetCompetitorByStatus(string CompetitorStatusOperator, string CompetitorStatusValue, string CompetitorTierOperator, string CompetitorTierValue)
        {
            string result = string.Empty;
            string clientCompany = Session["ClientCompany"].ToString();
            IList<Competitor> competitors = CompetitorService.GetAllByStatusAndTierOperator(clientCompany, CompetitorStatusValue, CompetitorStatusOperator, CompetitorTierValue, CompetitorTierOperator);
            int cont = 0;
            if (competitors != null && competitors.Count > 0)
            {
                foreach (Competitor comp in competitors)
                {
                    cont++;
                    if (competitors.Count == cont)
                    {
                        result = result + comp.Id + ":" + comp.Name;
                    }
                    else
                    {
                        result = result + comp.Id + ":" + comp.Name + "_";
                    }
                }
            }
            return Content(result);
        }

        [AuthenticationMaintenanceFilter]
        public ActionResult GenerateReportClient()
        {
            string browseId = Request["BrowseId"];
            string filterCriteria = Request["FilterCriteria"];
            string reportTitle = Request["Title"];
            string hidencColumnCriteria = Request["HiddenColumnCriteria"];
            browseId = "ReportClient";
            filterCriteria = string.Empty;
            reportTitle = "Report Client";
            hidencColumnCriteria = string.Empty;
            ResourceDataManager.GetInstance().CleanNumberGroup();

            BrowseObject browseObject = GetBrowseObjectForQuery(browseId, filterCriteria);
            IList dataSourceObjects = ReportService.GetData(browseObject);
            IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();

            IDictionary<string, string> columnDictionary = new Dictionary<string, string>();

            reportParameters["BrowseObject"] = browseObject;
            reportParameters["DataSource"] = dataSourceObjects;
            //UserProfile userProfile = UserManager.GetInstance().GetUserProfile(Session["UserId"].ToString());
            reportParameters["ReportTitle"] = reportTitle;
            reportParameters["ColumnDictionary"] = columnDictionary;
            string reportFile = ReportHelper.PrintReportExel(reportParameters);
            string path = ConfigurationSettings.AppSettings["ReportFilePath"].Substring(1);
            string tempfile = "Report" + Compelligence.Common.Utility.UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            GetDownloadFileResponse(path, reportFile, "Report Client.xls");
            return null;
        }

        protected void GetDownloadFileResponse(string path, string physicalName, string fileName)
        {
            Response.ContentType = Compelligence.Util.Common.FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.Clear();
            Response.WriteFile("~\\" + path + physicalName + ".xls");
            Response.End();
        }
    }
}
