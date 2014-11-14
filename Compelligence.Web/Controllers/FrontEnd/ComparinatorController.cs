using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Compelligence.BusinessLogic.Interface;
using Compelligence.BusinessLogic.Implementation;

using Compelligence.Domain.Entity;
using Compelligence.Domain.Entity.Resource;

using Compelligence.Common.Utility;
using Compelligence.Common.Utility.Parser;

using Compelligence.DataTransfer.Comparinator;

using Resources;
using Compelligence.Util.Type;
using Compelligence.DataTransfer.Entity;
using Compelligence.Web.Models.Util;
using System.IO;
using System.Configuration;
using Compelligence.Domain.Entity.Views;
using Compelligence.Security.Filters;
using iTextSharp.text;
using System.Text;
using System.Collections;
using Compelligence.DataTransfer.FrontEnd;
using System.Web.UI;
using Compelligence.Common.Cache;
using System.Xml;
using Compelligence.Common.Utility.Comparinator;
using NPOI.HSSF.UserModel;
 
namespace Compelligence.Web.Controllers
{
    public class ComparinatorController : GenericFrontEndController
    {
        private string _contextFilePath = AppDomain.CurrentDomain.BaseDirectory;
        //private string _comparinatorPath = AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"];
        public string ContextFilePath
        {
            get { return _contextFilePath; }
            set { _contextFilePath = value; }
        }
        #region Public Properties

        public IWebsiteService WebsiteService { get; set; }
        public IWebsiteDetailService WebsiteDetailService { get; set; }
        public IProjectService ProjectService { get; set; }
        public ILabelService LabelService { get; set; }
        public IEntityNewsService EntityNewsService { get; set; }
        public ISalesForceService SalesForceService { get; set; }
        public IConfigurationDefaultsService ConfigurationDefaultsService { get; set; }
        public IUserProfileService UserProfileService { get; set; }

        public IContentTypeService ContentTypeService { get; set; }

        public ILibraryService LibraryService { get; set; }

        public IIndustryService IndustryService { get; set; }

        public ICompetitorService CompetitorService { get; set; }

        public IProductService ProductService { get; set; }

        public ICriteriaSetService CriteriaSetService { get; set; }

        public ICriteriaGroupService CriteriaGroupService { get; set; }

        public IIndustryCriteriaService IndustryCriteriaService { get; set; }

        public ICompetitorCriteriaService CompetitorCriteriaService { get; set; }

        public IProductCriteriaService ProductCriteriaService { get; set; }

        public ICriteriaService CriteriaService { get; set; }

        public IForumService ForumService { get; set; }

        public IQuizService QuizService { get; set; }

        

        public IActionHistoryService ActionHistoryService { get; set; }
        public IPositioningService PositioningService { get; set; }
        public IIndustryCriteriasService IndustryCriteriasService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }
        public IExportService ExportService { get; set; }
        public IFileService FileService { get; set; }
        public IForumResponseService ForumResponseService { get; set; }
        public IPriceService PriceService { get; set; }
        public IResourceService ResourceService { get; set; }
        public IIndustryProductService IndustryProductService { get; set; }
        public IPricingTypeService PricingTypeService { get; set; }
        public IConfigurationUserTypeService ConfigurationUserTypeService { get; set; }
        public ISecurityGroupService SecurityGroupService { get; set; }
        public IPositioningCompetitorService PositioningCompetitorService { get; set; }
        public IPositioningIndustryService PositioningIndustryService { get; set; }
        #endregion

        #region Action Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProductInfo(string Description, string Id)
        {
            Product product = ProductService.GetById(Convert.ToDecimal(Id));
            product.Description = HttpUtility.HtmlDecode(Description);
            ProductService.Update(product);
            return Content(product.Description);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCompetitorsByIndustry(decimal id)
        {
            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");

            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salesuserid) ? CurrentCompany : salescompanyid;

            SetLabels(localuserid,localcompanyid);

            Session["Products"] = null;

            ClientCompany company = ClientCompanyService.GetById(localcompanyid);
            IList<Competitor> competitorList;

            bool DefaultsFeaturesTab = false;
            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(localcompanyid);
            if (configurations != null)
            {
                if (configurations.Count > 0)
                {
                    DefaultsFeaturesTab = Convert.ToBoolean(configurations[0].FeaturesTab);
                }
            }
            if (DefaultsFeaturesTab)
            {
                competitorList = CompetitorService.GetByIndustryAndProducts(id, localcompanyid);
            }
            else
            {
                competitorList = CompetitorService.GetByIndustryAndProductsWOR(id, localcompanyid);
            }
            company = ClientCompanyService.GetById(localcompanyid);


            if (competitorList == null || competitorList.Count == 0)
            {
                competitorList = new List<Competitor>();
            }


            IList<Competitor> orderedList = competitorList.OrderBy(x => 
                !(x.Name.ToUpper().Equals(company.Dns.ToUpper())|| x.Name.ToUpper().Equals(company.Name.ToUpper()) 
                                                      )).ToList();
            
            return ControllerUtility.GetSelectOptionsFromGenericList<Competitor>(orderedList, "Id", "Name");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetProductsByCompetitor(decimal id)
        {
            SetLabels();
            decimal industryId = Convert.ToDecimal(StringUtility.CheckNull(Request["IndustryId"]));

            string clientCompanyId = String.Empty;
            string salesclientcompanyencode = StringUtility.CheckNull(Request["C"]);

            if (!String.IsNullOrEmpty(salesclientcompanyencode) ) //then get from salesforce
                clientCompanyId = Encryptor.Decode(salesclientcompanyencode);
            else
                clientCompanyId=CurrentCompany;

            IList<ProductWithCriteriaValuesView> productListView = ProductService.GetByIndustryAndCompetitorView(industryId, id);
            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(clientCompanyId);
            bool DefaultsFeaturesTab = false;
            if (configurations != null)
            {
                if (configurations.Count > 0)
                {
                    DefaultsFeaturesTab = Convert.ToBoolean(configurations[0].FeaturesTab);
                }
            }

            if (productListView == null || productListView.Count == 0)
            {
                productListView = new List<ProductWithCriteriaValuesView>();
            }

            if (DefaultsFeaturesTab)
            {
                return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "HaveProductCriteria");
            }
            else
            {
                return ControllerUtility.GetSelectOptionsEnabledFromGenericList<ProductWithCriteriaValuesView>(productListView, "Id", "Name", true, "ColumnDefault");
            }
        }

        [AuthenticationFilter]
        public ActionResult Index(string comparisonType)
        {
            SetDefaultDataToLoadPage();
            SetLabels();
            GetDataOfConfiguration(CurrentCompany);
            IList<IndustryByHierarchyView> IndustryCollection = IndustryService.FindIndustryHierarchy(CurrentCompany);

            //for save comparison
            IList<UserProfileComparison> UserProfileComparisonCollection = UserProfileService.GetComparisonList(CurrentUser);
            ViewData["UserProfileComparisonId"] = new SelectList(UserProfileComparisonCollection, "Id", "Name");

            ViewData["cmbIndustry"] = new SelectList(IndustryCollection, "Id", "Name");
            ViewData["cmbCompetitor"] = new SelectList(new int[] { });
            ViewData["cmbProduct"] = new SelectList(new int[] { });
            ViewData["ComparisonType"] = comparisonType == null ? ComparisonType.Products : comparisonType;
            ViewData["WinLoss"] = QuizService.GetQuizByTargetTypeAndStatus(QuizType.WinLoss, WinLossStatus.Complete, QuizClassificationVisible.Yes, CurrentCompany);
            String DefaultsSocialLog = "false";
            ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(CurrentSecurityGroup, CurrentCompany);
            if (configurationUserType != null)
            {
                DefaultsSocialLog = configurationUserType.SocialLog;
            }

            ViewData["DefaultsSocialLog"] = DefaultsSocialLog;
            Session["Products"] = null;
            return View("Index");
        }

        public ActionResult SalesForce()
        {
            string salesemailencode = (string)Request["U"];
            string apikey = (string)Request["K"];
            string salesemail=string.Empty;

            try
            {
                salesemail = Encryptor.Decode(StringUtility.CheckNull(salesemailencode));
            }
            catch (Exception ex)
            {
                return Content("Compelligence message: unknown error, does not have access to the Compelligence system.");
            }


            UserProfile userProfile = UserProfileService.GetByEmail(salesemail);
            if (userProfile == null)
                return Content("Compelligence message: " + salesemail + " does not have access to the Compelligence system.");

            ClientCompany clientCompany = ClientCompanyService.GetById(userProfile.ClientCompany);
            if (clientCompany.Status.Equals(ClientCompanyStatus.Disabled))
            {
                return Content("Your company's subscription has been suspended. Please contact your system administrator or <b>support@compelligence.com</b> for assistance.");
            }
            string salesclientcompany = userProfile.ClientCompany;
            string salesclientcompanyencode = Encryptor.Encode(salesclientcompany);

            SetLabelsToForm(userProfile.ClientCompany, userProfile.Id);
            IList<IndustryByHierarchyView> IndustryCollection = IndustryService.FindIndustryHierarchy(salesclientcompany);
            String DefaultsSocialLog = "false";
            SetSecurityGroupIdToUser(userProfile);
            ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(userProfile.SecurityGroupId, salesclientcompany);
            if (configurationUserType != null)
            {
                DefaultsSocialLog = configurationUserType.SocialLog;
            }
            
            //IList<SecurityGroup> securityList = SecurityGroupService.GetByUser(userProfile.Id);
            //if (securityList != null && securityList.Count > 0)
            //{
            //    ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(userProfile.SecurityGroupId, salesclientcompany);
            //    if (configurationUserType != null)
            //    {
            //        DefaultsSocialLog = configurationUserType.SocialLog;
            //    }
            //}

            //for save comparison
            IList<UserProfileComparison> UserProfileComparisonCollection = UserProfileService.GetComparisonList(userProfile.Id);
            ViewData["UserProfileComparisonId"] = new SelectList(UserProfileComparisonCollection, "Id", "Name");

            ViewData["DefaultsSocialLog"] = DefaultsSocialLog;
            ViewData["cmbIndustry"] = new SelectList(IndustryCollection, "Id", "Name");
            ViewData["cmbCompetitor"] = new SelectList(new int[] { });
            ViewData["cmbProduct"] = new SelectList(new int[] { });
            ViewData["ComparisonType"] = ComparisonType.Products;
            Session["Products"] = null;

            ViewData["U"] = Encryptor.Encode(userProfile.Id);
            ViewData["C"] = salesclientcompanyencode;

            return View("SalesForceCMP");
        }

        private string GetUrlOfImage(string url)
        {
            if (url.IndexOf("./") == 0)
            {
                url = "." + url;                
            }

            return url;
        }

        public ActionResult RemoveProduct(decimal ProductId)
        {
            SetLabels();
            string salesclientcompanyencode = (string)Request["C"];
            ViewData["C"] = salesclientcompanyencode;
            string cc = CurrentCompany;
            if (!string.IsNullOrEmpty(salesclientcompanyencode))
            {
                cc = Encryptor.Decode(salesclientcompanyencode);
            }

            String DefaultsSocialLog = "false";
            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(cc);
            if (configurations != null)
            {
                if (configurations.Count > 0)
                {
                    DefaultsSocialLog = configurations[0].SocialLog;
                }
            }

            ViewData["DefaultsSocialLog"] = DefaultsSocialLog;

            IList<Product> products = (List<Product>)Session["Products"];
            if (products != null)
            {
                products.Remove(ProductService.GetById(ProductId));
                Session["Products"] = products.Count == 0 ? null : products;
                ViewData["Products"] = products;
                ViewData["Counter"] = products.Count.ToString();
                //return View("Products");

                ///
                IList<decimal> productIdsList = new List<decimal>();
                decimal industryIdG = 0;
                foreach (Product product in products)
                {
                    productIdsList.Add(product.Id);
                    industryIdG = product.Competitor.Industry.Id;
                }
                IList<System.Object[]> listObject = IndustryCriteriasService.GetMostRelevantProduct(productIdsList, industryIdG, cc);
                IList<ComparinatorRecommendedProducts> crpList = new List<ComparinatorRecommendedProducts>();
                foreach (System.Object[] objt in listObject)
                {
                    ComparinatorRecommendedProducts crp = new ComparinatorRecommendedProducts();
                    crp.IndustryId = (decimal)(objt[1]);
                    crp.ProductId = (decimal)(objt[2]);
                    crp.ProductName = objt[3].ToString().Replace("'", "&#39;"); 
                    crp.ProductImageUrl = objt[4].ToString();
                    if (crp.ProductImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                    {
                        string ImageUrl = "";
                        string[] urlObjects = crp.ProductImageUrl.Split('/');
                        Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                        newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                        if (newFileImage.FileFormat == null)
                        {
                            ImageUrl = "/Content/Images/Icons/none.png";
                            crp.ProductImageUrl = ImageUrl;
                        }
                    }
                    crp.CompetitorName = (string)objt[5];
                    crp.NumberOfCriteria = (decimal)(objt[6]);
                    decimal? getLatValue = 0;
                    getLatValue = decimal.Parse(objt[7].ToString());
                    crp.ComparinatorPercent = Decimal.Round((decimal)(getLatValue * 100 / crp.NumberOfCriteria), 2);
                    crpList.Add(crp);
                }
                ViewData["RecommendProducts"] = crpList;
            }
            ///
            return View("Elements");
        }


        public ActionResult SaveComparisonList() //Save current products list
        {
            string[] industryids = StringUtility.CheckNull(Request["iids"]).Split(',');
            string[] competitorids = StringUtility.CheckNull(Request["cids"]).Split(',');
            string name = HttpUtility.HtmlEncode( StringUtility.CheckNull(Request["name"]) );

            //if call from salesforce, then use SFDC credentials
            string salesuserid = GetDecodeParam("U");
            string localCurrentUser = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            

            IList<Product> products = (List<Product>)Session["Products"];
            if (products == null)
                return Content(string.Empty);

            if (products == null || (products != null && products.Count == 0))
                return Content(string.Empty);
            
            

            decimal IndustryId = (decimal)products[0].IndustryId;
            decimal CompetitorId = (decimal)products[0].CompetitorId;

            //Make Xml

            XMLElement xmli = new XMLElement("Industry");
            xmli.AddAttribute("id", "" + IndustryId);
            XMLElement xmlc = new XMLElement("Competitor");
            xmlc.AddAttribute("id", "" + CompetitorId);

            XMLElement xmlp = new XMLElement("Products");
            foreach (Product p in products)
            {
                XMLElement tag = new XMLElement("Product");
                tag.AddAttribute("id", ""+p.Id);
                tag.AddAttribute("competitorid", "" + p.Competitor.Id); //because every product have competitor (by add)
                xmlp.AddValue(tag);
            }

            XMLElement xml = new XMLElement("xml");
            xml.AddAttribute("version", "0.2");
            xml.AddValue(xmli);
            xml.AddValue(xmlc);
            xml.AddValue(xmlp);
            string result = xml.toString();

            //setting comparison register
            UserProfileComparison upc = new UserProfileComparison();
            upc.Id =(decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
            upc.UserId = localCurrentUser;
            upc.Name = name;
            upc.Data = xml.toString();
            UserProfileService.SaveComparisonList(upc);

            //for return new comparison list
            IList<UserProfileComparison> comparisons = UserProfileService.GetComparisonList(localCurrentUser);

            var anonymouscomparisons = new object[comparisons.Count];
            int i = 0;
            foreach (UserProfileComparison p in comparisons)
            {
                anonymouscomparisons[i++] = new { value = p.Id.ToString(), text = p.Name };
            }

            return new JsonResult() { Data = anonymouscomparisons };
        }


        public ActionResult GetComparisonList()
        {
            string id = StringUtility.CheckNull(Request["id"]);

            //if call from salesforce, then use SFDC credentials
            string salesuserid = GetDecodeParam("U");
            string salesclientid = GetDecodeParam("C");
            string localCurrentCompany = string.IsNullOrEmpty(salesclientid) ? CurrentCompany : salesclientid;

            UserProfileComparison upc = UserProfileService.GetComparisonListById(decimal.Parse(id));
            XmlDocument upcdata = new XmlDocument();
            upcdata.LoadXml(upc.Data);
            XmlNode xmlIndustry = upcdata.FirstChild.FirstChild;
            XmlNode xmlCompetitor = xmlIndustry.NextSibling;
            XmlNodeList xmlProducts = xmlCompetitor.NextSibling.SelectNodes("Product");
            IList<Pair<string, string>> Elements = new List<Pair<string, string>>();

            foreach(XmlNode xmlProduct in xmlProducts)
            {
                Pair<string, string> value = new Pair<string, string>(xmlProduct.Attributes["id"].Value, xmlProduct.Attributes["competitorid"].Value);
                Elements.Add(value);
            }


            //Override Session for products
            Session["Products"] = null;
            ViewData["ShowCompareButton"] = string.Empty;

            bool isValidProducts = true;
            //foreach (Pair<string, string> e in Elements)
            for(int i=0; i<Elements.Count; i++)
            {
                Pair<string, string> e = Elements[i];
                decimal industryId = decimal.Parse(StringUtility.CheckNull(xmlIndustry.Attributes["id"].Value));
                string productId = e.First;
                if (!string.IsNullOrEmpty(productId))
                {
                    IndustryProductId industryProductId = new IndustryProductId(industryId, decimal.Parse(productId));
                    IndustryProduct industryProduct = IndustryProductService.GetById(industryProductId);
                    if (industryProduct == null)
                    {
                        isValidProducts = false;
                        i = Elements.Count;
                    }
                }
            }

            if (isValidProducts)
            {
                //iterate by competitorid blocks one by one
                foreach (Pair<string, string> e in Elements)
                {

                    FormCollection formcollection = new FormCollection();
                    formcollection.Add("IndustryId", StringUtility.CheckNull(xmlIndustry.Attributes["id"].Value));
                    formcollection.Add("CompetitorId", e.Second);
                    formcollection.Add("multiselect_ProductId", e.First);
                    AddProductGeneric(formcollection, localCurrentCompany);
                }
            }
            return View("Elements");



           // AddProductGeneric(FormCollection form, string clientCompanyId)
            //
            /*
            return new JsonResult() { Data = new { industryid=StringUtility.CheckNull(xmlIndustry.Attributes["id"].Value),
                                          competitorid=StringUtility.CheckNull(xmlCompetitor.Attributes["id"].Value),
                                          productids=ProductIds} };
             * */

        }


        public ActionResult GetComparisonListDropDownInfo()
        {
            
            
            string id = StringUtility.CheckNull(Request["id"]);

            //if call from salesforce, then use SFDC credentials
            string salesuserid = GetDecodeParam("U");
            string salesclientid = GetDecodeParam("C");
            string localCurrentCompany = string.IsNullOrEmpty(salesclientid) ? CurrentCompany : salesclientid;
            ClientCompany company = ClientCompanyService.GetById(localCurrentCompany);
            UserProfileComparison upc = UserProfileService.GetComparisonListById(decimal.Parse(id));
            XmlDocument upcdata = new XmlDocument();
            upcdata.LoadXml(upc.Data);
            XmlNode xmlIndustry = upcdata.FirstChild.FirstChild;
            XmlNode xmlCompetitor = xmlIndustry.NextSibling;
            XmlNodeList xmlProducts = xmlCompetitor.NextSibling.SelectNodes("Product");
            IList<string> ProductIds = new List<string>();
            foreach (XmlNode xmlProduct in xmlProducts)
            {
                ProductIds.Add(StringUtility.CheckNull(xmlProduct.Attributes["id"].Value));
            }

            //retrieve industry
            string industryid=StringUtility.CheckNull(xmlIndustry.Attributes["id"].Value);
            string competitorid = StringUtility.CheckNull(xmlCompetitor.Attributes["id"].Value);

            bool isValidProducts = true;
            //foreach (Pair<string, string> e in Elements)
            for (int i = 0; i < ProductIds.Count; i++)
            {
                string productId = ProductIds[i];
                decimal industryId = decimal.Parse(StringUtility.CheckNull(xmlIndustry.Attributes["id"].Value));
                //string productId = e;
                if (!string.IsNullOrEmpty(productId))
                {
                    IndustryProductId industryProductId = new IndustryProductId(industryId, decimal.Parse(productId));
                    IndustryProduct industryProduct = IndustryProductService.GetById(industryProductId);
                    if (industryProduct == null)
                    {
                        isValidProducts = false;
                        i = ProductIds.Count;
                    }
                }
            }

            if (isValidProducts)
            {
                IList<Competitor> competitors = CompetitorService.GetByIndustryAndProducts(decimal.Parse(industryid), localCurrentCompany);

                IList<Competitor> orderedList = competitors.OrderBy(x => !(x.Name.ToUpper().Equals(company.Dns.ToUpper()) ||
                                              x.Name.ToUpper().Equals(company.Name.ToUpper()))).ToList();

                var anonymouscompetitors = orderedList.Select(a => new { value = a.Id.ToString(), text = a.Name });


                //Get Configuration and evaluate
                IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(localCurrentCompany);
                bool DefaultsFeaturesTab = false;
                if (configurations != null && configurations.Count > 0)
                    DefaultsFeaturesTab = Convert.ToBoolean(configurations[0].FeaturesTab);

                IList<ProductWithCriteriaValuesView> products = ProductService.GetByIndustryAndCompetitorView(decimal.Parse(industryid), decimal.Parse(competitorid));
                var anonymousproducts = new object[products.Count];
                int i = 0;
                foreach (ProductWithCriteriaValuesView p in products)
                {
                    if (!DefaultsFeaturesTab) //no consider
                    {
                        if (ProductIds.Contains("" + p.Id))
                            anonymousproducts[i++] = new { value = p.Id.ToString(), text = p.Name, selected = "selected", disabled = "" };
                        else
                            anonymousproducts[i++] = new { value = p.Id.ToString(), text = p.Name, selected = "", disabled = "" };
                    }
                    else  //consider disable
                    {
                        if (ProductIds.Contains("" + p.Id))
                            anonymousproducts[i++] = new { value = p.Id.ToString(), text = p.Name, selected = "selected", disabled = p.HaveProductCriteria };
                        else
                            anonymousproducts[i++] = new { value = p.Id.ToString(), text = p.Name, selected = "", disabled = p.HaveProductCriteria };
                    }

                }

                return new JsonResult()
                {
                    Data = new
                    {
                        industryid = StringUtility.CheckNull(xmlIndustry.Attributes["id"].Value),
                        competitorid = StringUtility.CheckNull(xmlCompetitor.Attributes["id"].Value),
                        competitors = anonymouscompetitors,
                        products = anonymousproducts
                    }
                };
            }
            else {
                return null;
            }

        }


        public ActionResult RemoveComparisonList()
        {
            string id = StringUtility.CheckNull(Request["id"]);
            UserProfileService.DeleteComparisonList(decimal.Parse(id));

            Session["Products"] = null;
            //return new list
            string salesuserid = GetDecodeParam("U");
            string localCurrentUser = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            IList<UserProfileComparison> comparisons = UserProfileService.GetComparisonList(localCurrentUser);

            var anonymouscomparisons = new object[comparisons.Count];
            int i = 0;
            foreach (UserProfileComparison p in comparisons)
            {
                anonymouscomparisons[i++] = new { value = p.Id.ToString(), text = p.Name };
            }

            return new JsonResult() { Data = anonymouscomparisons };
        }


        public ActionResult RenameComparisonList() //Save current products list
        {

            string id = StringUtility.CheckNull(Request["id"]);
            string name = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["name"]));

            //if call from salesforce, then use SFDC credentials
            string salesuserid = GetDecodeParam("U");
            string salesclientid = GetDecodeParam("C");
            string localCurrentCompany = string.IsNullOrEmpty(salesclientid) ? CurrentCompany : salesclientid;
            string localCurrentUser = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;

            UserProfileComparison upc = UserProfileService.GetComparisonListById(decimal.Parse(id));
            upc.Name = name;

            UserProfileService.SaveComparisonList(upc);

            //for return new comparison list
            IList<UserProfileComparison> comparisons = UserProfileService.GetComparisonList(localCurrentUser);

            var anonymouscomparisons = new object[comparisons.Count];
            int i = 0;
            foreach (UserProfileComparison p in comparisons)
            {
                anonymouscomparisons[i++] = new { value = p.Id.ToString(), text = p.Name };
            }

            return new JsonResult() { Data = anonymouscomparisons };
        }

        public ActionResult SaveCriteria()
        {
            string groupid = (string)Request["groupid"];
            string setid = (string)Request["setid"];
            string name = (string)Request["name"];
            string benefitValue = (string)Request["benefitValue"];
            string relevancy = (string)Request["relevancy"];
            string type = (string)Request["type"];
            string mostDesiredValue = (string)Request["mostDesiredValue"];
            
            //// To get ClientCompany and UserId of Salesforce
            string salesclientcompanyencode = (string)Request["C"];
            ViewData["C"] = salesclientcompanyencode;
            string salesuseridencode = (string)Request["U"];
            ViewData["U"] = salesuseridencode;
            string salestoolsclientcompany = CurrentCompany;
            string salestoolsuserid = CurrentUser;
            if (!string.IsNullOrEmpty(salesclientcompanyencode))
            {
                salestoolsclientcompany = Encryptor.Decode(salesclientcompanyencode);
            }
            if (!string.IsNullOrEmpty(salesuseridencode))
            {
                salestoolsuserid = Encryptor.Decode(salesuseridencode);
            }
          
            //Column parameters
            ViewData["pis"] = (string)Request["pis"] ?? string.Empty; //Industry Standard
            ViewData["pb"] = (string)Request["pb"] ?? string.Empty; //Benefit
            ViewData["pc"] = (string)Request["pc"] ?? string.Empty; //Cost

            ViewData["indstandard"] = (string)Request["indstandard"];
            ViewData["benefit"] = (string)Request["benefit"];
            if (String.IsNullOrEmpty(relevancy))
                relevancy = CriteriaRelevancy.Medium;
            if (String.IsNullOrEmpty(type))
                type = CriteriaType.List;
            if (String.IsNullOrEmpty(mostDesiredValue))
                mostDesiredValue = string.Empty;

            Criteria criteria = new Criteria((decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey());
            SetDefaultDataForSave(criteria);
            criteria.Name = name.Trim();
            criteria.Benefit = benefitValue.Trim();
            criteria.Relevancy = relevancy;
            criteria.Type = type;
            criteria.MostDesiredValue = mostDesiredValue;
            CriteriaService.Save(criteria);


             IList<Product> products = (List<Product>)Session["Products"];
            decimal IndustryId = (decimal)products[0].IndustryId;
            IndustryCriteriasId industrycriteriasid=new IndustryCriteriasId(IndustryId,criteria.Id);
            IndustryCriterias industrycriterias = new IndustryCriterias(industrycriteriasid);
            SetDefaultDataForSaveEntity(industrycriterias, salestoolsuserid, salestoolsclientcompany);
            industrycriterias.Visible = "Y";
            industrycriterias.CriteriaGroupId = decimal.Parse(groupid);
            industrycriterias.CriteriaSetId = decimal.Parse(setid);

            IndustryCriteriasService.Save(industrycriterias);

            CriteriaDTO criteriadto = new CriteriaDTO();
            criteriadto.Id = criteria.Id;
            criteriadto.Name = criteria.Name;
            criteriadto.Type = criteria.Type;
            criteriadto.Benefit = criteria.Benefit;
            criteriadto.Relevancy = criteria.Relevancy;
            criteriadto.MostDesiredValue = criteria.MostDesiredValue;

            ComparinatorCriteria rowCriteria = new ComparinatorCriteria(criteriadto);

            //make cells /p1 /p2 /p3 /p4 /p5 / ....
            foreach (Product p in products) //Criterias x Products
            {
                rowCriteria.Values[p.Id]=null;
            }

            ViewData["Products"] = products;
            ViewData["IndustryId"] = IndustryId;

            ViewData["User"] = UserProfileService.GetById(salestoolsuserid);
            ViewData["ComparinatorCriteria"] = rowCriteria;
            return View("RowCriteria");
        }

        public ActionResult CellDetail()
        {
            string pid = StringUtility.CheckNull(Request["pid"]);  //productid
            string cid =StringUtility.CheckNull(Request["cid"]); //criteriaid
            string iid = StringUtility.CheckNull(Request["iid"]);  //industryid
            string spc = StringUtility.CheckNull(Request["spc"]);  //show public comment
            pid = string.IsNullOrEmpty(pid) ? "0" : pid;
            cid = string.IsNullOrEmpty(cid) ? "0" : cid;
            iid = string.IsNullOrEmpty(iid) ? "0" : iid;
            spc = string.IsNullOrEmpty(spc) ? "false" : spc;//False by Deafult, in the case no is checked and no exist in configuration
            bool dpc =  Convert.ToBoolean( spc);
            ProductCriteriaId pcid = new ProductCriteriaId(decimal.Parse(pid),decimal.Parse(cid),decimal.Parse(iid));
            ProductCriteria productCriteria = ProductCriteriaService.GetById(pcid);

            if (productCriteria == null)
            {
                productCriteria=ProductCriteriaService.GetNew( decimal.Parse(iid), decimal.Parse(pid), decimal.Parse(cid) );
            }
            if (productCriteria.ExternalId == null)
            {
                productCriteria.ExternalId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey();
            }
            //Verify if exist comments
            Forum forum = ForumService.GetByEntityId((decimal)productCriteria.ExternalId, DomainObjectType.ProductCriteria, ForumType.Comment);
            ViewData["HasComments"] = false;
            if (forum != null && ForumResponseService.GetCountResponsesByForumId(forum.Id) > 0)
            {
                ViewData["HasComments"] = true;
            }
            ViewData["ProductCriteria"] = productCriteria; //+ExternalId
            ViewData["ObjectType"] = DomainObjectType.ProductCriteria;

            Criteria criteria = CriteriaService.GetById(decimal.Parse(cid));
            Product product = ProductService.GetById(decimal.Parse(pid));
            ViewData["CellPropertyTitle"] = criteria.Name.Replace("'", "\\'") + "/" + product.Name.Replace("'", "\\'");

            ViewData["pid"] = Request["pid"];
            ViewData["U"] = Request["U"];
            ViewData["C"] = Request["C"];
            ViewData["spc"] = spc;
            return View();
        }

        public ActionResult CellIndustryStandard()
        {
            string criteriaid = StringUtility.CheckNull(Request["cid"]);
            string industryid = StringUtility.CheckNull(Request["iid"]);
            ViewData["CriteriaId"] = criteriaid;
            Criteria criteria = CriteriaService.GetById(decimal.Parse(criteriaid));
            ViewData["IndustryStandard"] = criteria.IndustryStandard;
            ViewData["CriteriaName"] = criteria.Name;
            ViewData["IndustryId"] = industryid;
            return View();
        }

        public ActionResult CellProperty()
        {
            string pid = StringUtility.CheckNull(Request["pid"]);  //productid
            string cid = StringUtility.CheckNull(Request["cid"]); //criteriaid
            string iid = StringUtility.CheckNull(Request["iid"]);  //industryid
            string C = StringUtility.CheckNull(Request["C"]);  //C
            string U = StringUtility.CheckNull(Request["U"]);  //U
            pid = string.IsNullOrEmpty(pid) ? "0" : pid;
            cid = string.IsNullOrEmpty(cid) ? "0" : cid;
            iid = string.IsNullOrEmpty(iid) ? "0" : iid;

            ProductCriteriaId pcid = new ProductCriteriaId(decimal.Parse(pid), decimal.Parse(cid), decimal.Parse(iid));
            ProductCriteria productCriteria = ProductCriteriaService.GetById(pcid);

            if (productCriteria == null)
            {
                productCriteria = ProductCriteriaService.GetNew(decimal.Parse(iid), decimal.Parse(pid), decimal.Parse(cid));
            }

            Criteria criteria=CriteriaService.GetById(decimal.Parse(cid)); //doing this because productcriteria not save criteriatype
            productCriteria.CriteriaType = criteria.Type;
            if (criteria.Type.Equals("NUM"))
            {
                if (!productCriteria.Value.Equals("N/A") && productCriteria.ValueDecimal!= null)
                    productCriteria.Value = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.########}", productCriteria.ValueDecimal);// productCriteria.ValueDecimal.ToString(); productCriteria.ValueDecimal.ToString();
            }
            ViewData["ProductCriteria"] = productCriteria;
            ViewData["C"] = C;
            ViewData["U"] = U;
            return View();
        }


        public ActionResult CellPropertySave(FormCollection form)
        {
            string industryid = form["IndustryId"];
            string criteriaid = form["CriteriaId"];
            string productid = form["ProductId"];
            string returnMessage = string.Empty;
            string value = form["txtValue"];
            string notes = form["txtNotes"];
            string links = form["txtLinks"];
            string feature = form["Feature"];
            string localuserid = string.Empty;
            string localclientcompany = string.Empty;
            string salesuserid = GetDecodeParam(form, "U");
            string salescompanyid = GetDecodeParam(form, "C");
            localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            localclientcompany = string.IsNullOrEmpty(salescompanyid) ? CurrentCompany : salescompanyid;
            string criteriatype = form["txtCriteriaType"];
            int allproducts = 1; // 0=selected products, 1=all products

            string Data = string.Empty;
            decimal sameValueDecimal;//to store the value decimal and compare if all rows are equal
            IList<Product> products = (List<Product>)Session["Products"];

            bool createAction = false;//if the productcriteria exist no CREATE NEW PRODUCT CRITERIA
            ProductCriteria productcriteria = new ProductCriteria();
            ProductCriteriaId id = new ProductCriteriaId(decimal.Parse(productid), decimal.Parse(criteriaid), decimal.Parse(industryid));
            productcriteria = ProductCriteriaService.GetById(id);
            if (productcriteria == null) {
                createAction = true;
                productcriteria = ProductCriteriaService.GetNew(decimal.Parse(industryid), decimal.Parse(productid), decimal.Parse(criteriaid));
                SetDefaultDataForSave(productcriteria, localclientcompany, localuserid);
            }
            if (criteriatype.Equals(CriteriaType.Numeric))
            {
                if (value.ToUpper().Equals("N/A"))
                {
                    productcriteria.Value = value.ToUpper();//set the value in String
                    productcriteria.ValueDecimal = null; //if exist some value numeric should be removed
                }
                else if (DecimalUtility.IsDecimal(value))
                {
                    productcriteria.ValueDecimal = DecimalUtility.ConvertStringToDecimal(value);
                    //string tempo = DecimalUtility.ConvertDecimalToStringWithFormat(productcriteria.ValueDecimal);
                    productcriteria.Value = DecimalUtility.ConvertDoubleToString(Convert.ToDouble(productcriteria.ValueDecimal));
                }
            }
            else if (criteriatype.Equals(CriteriaType.List))
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 225)
                {
                    returnMessage = string.Format(LabelResource.ComparinatorCriteriaListValueMaxLength, value, "225", "225");
                    value = value.Substring(0, 225);                   
                }
                productcriteria.Value = value;
                if (string.IsNullOrEmpty(feature)) feature = "MP";//the value by default is MP
            }
            else
            {
                productcriteria.Value = value;
            }
            productcriteria.Notes = notes;
            productcriteria.Links = links;
            productcriteria.Feature = feature;
            productcriteria.CriteriaUploader = false;
            productcriteria.CriteriaType = criteriatype;
            if (createAction) ProductCriteriaService.Save(productcriteria);//if product criteria no exist then create new product criteria
            else 
            {
                SetDefaultDataForUpdate(productcriteria, localuserid);
                ProductCriteriaService.Update(productcriteria); //Recalculate High/Low for all Products
            }
            IList<object> currentfeatures = new List<object>(); //for retrieve current row with all products+feature

            Criteria criteria = CriteriaService.GetById((decimal)productcriteria.CriteriaId);
            IList<ProductCriteria> all = ProductCriteriaService.GetByIndustryAndCriteria((decimal)productcriteria.IndustryId, (decimal)productcriteria.CriteriaId, productcriteria.ClientCompany);

            //Evaluate Values
            int sameCounter = 0;
            string sameValue = string.Empty;

            #region Numeric
            
            if (criteria.Type.Equals(CriteriaType.Numeric))
            {
                if(value.ToUpper().Equals("N/A")) value=value.ToUpper();//is value is N/A should show N/A
                else if(!value.Equals("N/A") && DecimalUtility.IsDecimal(value))//to set the value decimal with the correct format 123,456,789.123
                {
                    //if (value.IndexOf(",") != -1) value = value.Replace(",", "");//case 123,123,456
                    //sameValue = value;
                    //string valueToDecimal = value.Replace(".", ",");
                    //value = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:#,#0.########}", Decimal.Parse(valueToDecimal));//text to show in cell of comparinator
                    decimal? valueDecimal =DecimalUtility.ConvertStringToDecimal(value);
                    value = DecimalUtility.ConvertDoubleToString(Convert.ToDouble(valueDecimal));
                }
                decimal? HighValueSelected = 0;
                decimal? LowValueSelected = int.MaxValue;

                decimal? HighValueAll = criteria.HighValueAllProducts;
                decimal? LowValueAll = criteria.LowValueAllProducts;
                decimal? sameValueDec = all[0].ValueDecimal;
                foreach (Product p in products)
                {
                    ProductCriteria pc = GetFromProductCriteriaList(p, all);
                    if(pc!=null)
                    {
                        if (pc.ValueDecimal!=  null)
                        {
                            decimal? vvalue = pc.ValueDecimal;
                            if (vvalue > HighValueSelected)
                                HighValueSelected = vvalue;
                            if (vvalue < LowValueSelected)
                                LowValueSelected = vvalue;
                            if (sameValueDec == (decimal)pc.ValueDecimal)
                            {
                                sameCounter++;
                            }
                        }
                    }
                }
                //We have High/Low both cases selected and all

                //Prepare retireve list
                foreach (Product p in products)
                {
                    ProductCriteria pc = GetFromProductCriteriaList(p, all);
                    if (pc != null )
                    {
                      if (pc.ValueDecimal!= null)
                      {
                        if (HighValueSelected != LowValueSelected)//if range between productus selected is diferent to 0 , WOR = WithOut Range
                        {
                            string nfeature = AutoFeature.GetNumericFeature((decimal)pc.ValueDecimal,criteria.MostDesiredValue, (decimal)HighValueSelected, (decimal)LowValueSelected);
                            string afeature = AutoFeature.GetNumericFeature((decimal)pc.ValueDecimal, criteria.MostDesiredValue, (decimal)criteria.HighValueAllProducts, (decimal)criteria.LowValueAllProducts);
                            currentfeatures.Add(new { pid = p.Id.ToString(), s = nfeature,a = afeature });
                        }
                        else
                            currentfeatures.Add(new { pid = p.Id.ToString(), s = "WOR", a = "ROW" });
                      }
                      else
                        currentfeatures.Add(new { pid = p.Id.ToString(), s = "NF", a = "NF" }); //empty
                    } 
                    else
                      currentfeatures.Add(new { pid = p.Id.ToString(), s = "NF",a = "NF" });
                    //Result is Data="F1 F2 F3 ...
                }
            }
            #endregion

            #region boolean
            else if (criteria.Type.Equals(CriteriaType.Boolean))
            {
                sameValue = all[0].Value;
                int yc = 0, nc = 0;
              foreach (Product p in products)
              {
                ProductCriteria pc = GetFromProductCriteriaList(p, all);
                if (pc != null )
                {
                    if (!string.IsNullOrEmpty(pc.Value))
                    {
                        pc.Value = pc.Value.ToUpper();
                        if (pc.Value.Equals("YES") || pc.Value.Equals("Y"))
                            yc++;
                        else if (pc.Value.Equals("NO") || pc.Value.Equals("N"))
                            nc++;
                    }
                    if (pc.Value.Equals(sameValue))
                    {
                        sameCounter++;
                    }
                 }
               }

               foreach (Product p in products)
               {
                    ProductCriteria pc = GetFromProductCriteriaList(p, all);
                    if (pc != null )
                    {
                       string bfeature = AutoFeature.GetBooleanFeature(pc.Value, criteria.MostDesiredValue);
                       string afeature = AutoFeature.GetBooleanFeature(pc.Value, criteria.MostDesiredValue);
                       currentfeatures.Add(new { pid = p.Id.ToString(), s = bfeature,a=afeature });
                    }
                    else
                        currentfeatures.Add(new { pid = p.Id.ToString(), s = "NF", a = "NF" });
               }
            }
            #endregion
            #region List
            if (criteria.Type.Equals(CriteriaType.List))
            {
                sameValue = all[0].Value;
                foreach (Product p in products)
                {
                    ProductCriteria pc = GetFromProductCriteriaList(p, all);
                    if( pc!=null )
                    {
                        string lfeature= pc.Feature; //persistent feature
                        currentfeatures.Add(new { pid = p.Id.ToString(), s = lfeature, a = lfeature });
                        if (pc.Value.Equals(sameValue))
                        {
                            sameCounter++;
                        }
                    }
                    else
                        currentfeatures.Add(new { pid = p.Id.ToString(), s = "NF", a = "NF" });
                }
            }
            #endregion
            string classRowEqual = "comp_neq";
            if (sameCounter == products.Count) classRowEqual = "comp_eq";
            return new JsonResult() { Data = new { value = value, cid = criteriaid, type = criteria.Type, pid = productid, pfea = currentfeatures, creq = classRowEqual, rtm = returnMessage } };
        }


        public ActionResult SilverBullets(FormCollection form)
        {
            //we need two list products, own and others using competitor criteria
            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");

            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salesuserid) ? CurrentCompany : salescompanyid;

            IList<Product> products = (IList<Product>)Session["Products"];
            IList<ComparinatorGroup> groups = (IList<ComparinatorGroup>)Session["ComparinatorGroups"];

            IDictionary<string, object> productlistG1=PreSilverBullets.FirstGroup(products, groups);
            IDictionary<string, object> productlistG2=PreSilverBullets.SecondGroup(products, groups);

            return new JsonResult() { Data = new { g1=productlistG1.ToList(), g2=productlistG2.ToList() } };
        }


        
        ProductCriteria GetFromProductCriteriaList(Product p, IList<ProductCriteria> list)
        {
            ProductCriteria result = null;
            for(int k=0;k<list.Count;k++)
            {
                ProductCriteria pc=list[k];
                if (pc.Id.ProductId.Equals(p.Id))
                {
                    result = pc;
                    break;
                }
            }
            return result;
        }

        public ActionResult RowHeaderFilter()
        {
            string productid = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["pid"]));
            string[] features =HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["features[]"])).Split(',');

            ViewData["BC"] = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["BC"]));
            ViewData["MA"] = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["MA"]));
            ViewData["MP"] = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["MP"]));
            ViewData["MD"] = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["MD"]));
            ViewData["LM"] = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request["LM"]));

            ViewData["productid"] = productid;
            ViewData["features"] = features;
            return View();
        }

        public ActionResult CellRelevancy()
        {
            string cid = StringUtility.CheckNull(Request["cid"]);
            Criteria criteria=CriteriaService.GetById( decimal.Parse(cid) );
            ViewData["cid"] = cid;
            ViewData["RelevancyHIGH"]=criteria.Relevancy.Equals(CriteriaRelevancy.High) ? "checked":"";
            ViewData["RelevancyMEDI"]=criteria.Relevancy.Equals(CriteriaRelevancy.Medium) ? "checked":"";
            ViewData["RelevancyLOW"] = criteria.Relevancy.Equals(CriteriaRelevancy.Low) ? "checked" : "";
            return View();
        }
        public void CellRelevancySave()
        {
            string cid = (string)Request["cid"];
            string relevant = (string)Request["relevant"];
            Criteria criteria = CriteriaService.GetById(decimal.Parse(cid));
            if (relevant.Equals("HIGH")) { criteria.Relevancy = CriteriaRelevancy.High; }
            else if (relevant.Equals("MEDI")) { criteria.Relevancy = CriteriaRelevancy.Medium; }
            else { criteria.Relevancy = CriteriaRelevancy.Low; }
            CriteriaService.Update(criteria);
        }


        public ActionResult CellBenefit()
        {
            string criteriaid = StringUtility.CheckNull(Request["cid"]);
            string industryid = StringUtility.CheckNull(Request["iid"]);
            ViewData["CriteriaId"] = criteriaid;
            Criteria criteria = CriteriaService.GetById(decimal.Parse(criteriaid));
            ViewData["CriteriaDescription"] = String.IsNullOrEmpty( criteria.Description.Trim() ) ? criteria.Name : criteria.Description;
            ViewData["CriteriaName"] = criteria.Name;
            ViewData["IndustryId"] = industryid;
            return View();
        }

        
        public ActionResult CellCost()
        {
            string criteriaid = StringUtility.CheckNull(Request["cid"]);
            string industryid = StringUtility.CheckNull(Request["iid"]);

            ViewData["CriteriaId"] = criteriaid;
            Criteria criteria = CriteriaService.GetById(decimal.Parse(criteriaid));
            ViewData["CriteriaDescription"] = String.IsNullOrEmpty(criteria.Description.Trim()) ? criteria.Name : criteria.Description;
            ViewData["CriteriaName"] = criteria.Name;
            ViewData["IndustryId"] = industryid;

            return View();
        }

        //[CompressFilter]
        public ActionResult CompareSalesForceProducts() //Teying work this method from both
        {
            return CompareProducts();
        }

        [AuthenticationFilter]
        //[CompressFilter]
        public ActionResult CompareProducts() //Teying work this method from both
        {


            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");

            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salesuserid) ? CurrentCompany : salescompanyid;

            //everything need work using local*
            ViewData["C"] = (string)Request["C"];
            ViewData["U"] = (string)Request["U"];
            SetLabels(localuserid,localcompanyid);

            ComparinatorCatalog catalog = new ComparinatorCatalog();
            //for pricing
            //IList<ComparinatorPrice> comparinatorprices777 = new List<ComparinatorPrice>();
            IList<Product> products = (List<Product>)Session["Products"];
            if (products == null || (products != null && products.Count == 0))
                return Content(string.Empty);

            foreach (Product p in products)
            {
                p.HasComment = getComments(p.Id, DomainObjectType.Product);
                p.HasSilverComment = getSilverCommentComments((decimal)p.IndustryId, p.Id);
                p.CompetitiveMessaging = SetPostioningToProduct(PositioningRelation.Positioning, p.Id, p.Competitor.Id, (decimal)p.Competitor.Industry.Id, localcompanyid);
            }
            if (!HaveProductOfClientCompetitor(products))
            {
                string message = string.Format(LabelResource.ComparinatorCompareProductRequiredError, "");
                ClientCompany client = ClientCompanyService.GetById(localcompanyid);
                if (client != null)
                {
                    message = string.Format(LabelResource.ComparinatorCompareProductRequiredError, client.Name);
                }
                return Content("<br /><h2 style='padding-top: 25px; color: Red;'>" + message + "</h2>");
            }
            UserProfile User = UserProfileService.GetById(localuserid);
            SetSecurityGroupIdToUser(User);
            ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(User.SecurityGroupId, localcompanyid);
            decimal IndustryId = (decimal)products[0].IndustryId;
            decimal CompetitorId = (decimal)products[0].CompetitorId; //required by export
            Industry industry = IndustryService.GetById(IndustryId);
            ViewData["IndustrySelected"] = industry;
            CriteriaGroup AnonymousGroup = CriteriaGroupService.GetNew(IndustryId);
            AnonymousGroup.Name = "No Group Identified, Please reassign";
            CriteriaSet AnonymousSet = CriteriaSetService.GetNew(IndustryId);
            AnonymousSet.Name = "No Set Identifed, Please assign";

            DateTime runtime = DateTime.Now;

            //IDictionary<decimal,Criteria> criterias = IndustryCriteriasService.GetCriteriasByIndustry(IndustryId, CurrentCompany).ToDictionary(x=>x.Id,x=>x);
            IDictionary<decimal?, CriteriaDTO> criterias = IndustryCriteriasService.GetByIndustryIdExt(IndustryId).ToDictionary(x => x.Id, x => x);

            string productids = string.Join(",", products.Select(a => a.ProductId.ToString()).ToArray());
            IList<ProductCriteriaDTO> productcriterias = ProductCriteriaService.GetCriteriasOf(IndustryId, productids, localcompanyid);

            DateTime runtime2 = DateTime.Now;
            //Fill Catalog with Criterias and Products

            int r=0;
            while( r<productcriterias.Count )
            {
                #region crierias
                ProductCriteriaDTO pcDTO=productcriterias[r];

                if (!criterias.ContainsKey(pcDTO.CriteriaId) ) //if not exist in list was deleted, then it will ignore
                {
                    r++;
                    continue;
                }

                ComparinatorCriteria rowCriteria = new ComparinatorCriteria( criterias[pcDTO.CriteriaId ] ); //Row
                
                //Make Cells /p1 /p2 /p3 /p4 /p5 / ....
                foreach (Product p in products) //Criterias x Products
                {
                    rowCriteria.Values[p.Id]=null;
                }

                //skip & fill cols
                decimal? criteriaid = productcriterias[r].CriteriaId; //current
                while (r < productcriterias.Count && criteriaid == productcriterias[r].CriteriaId)
                {
                    if (productcriterias[r].ProductId != null)
                        rowCriteria.Values[(decimal)productcriterias[r].ProductId] = new ProductCriteriaCell(
                           productcriterias[r].Value, 
                           productcriterias[r].Feature,
                           productcriterias[r].ExternalId,
                           productcriterias[r].ProductId, 
                           productcriterias[r].LNotes, 
                           productcriterias[r].LLinks, 
                           productcriterias[r].CComments,
                           productcriterias[r].ValueDecimal);
                           
                   r++;
                }
                //Everytime finish while block, pointer(r) is over next record
                //Work over row, productlist

                //
                // Need improve scatter groups and sets
                //

                CriteriaDTO temp=criterias[criteriaid];

                if (temp.CriteriaGroupId != null && temp.CriteriaSetId != null)
                {
                    rowCriteria.CriteriaSet = new CriteriaSet((decimal)temp.CriteriaSetId,temp.CriteriaSetName);
                    rowCriteria.CriteriaGroup = new CriteriaGroup((decimal)temp.CriteriaGroupId, temp.CriteriaGroupName);
                }
                else if (temp.CriteriaGroupId != null && temp.CriteriaSetId == null)
                {
                    rowCriteria.CriteriaSet = AnonymousSet;
                    rowCriteria.CriteriaGroup = new CriteriaGroup((decimal)temp.CriteriaGroupId, temp.CriteriaGroupName);
                }
                else //Asume Set and Group = null
                {
                    rowCriteria.CriteriaSet = AnonymousSet;
                    rowCriteria.CriteriaGroup = AnonymousGroup;
                }

                catalog.Push(rowCriteria);
                #endregion crierias
            }

            //Verify if rows have same values
            foreach (ComparinatorGroup cg in catalog.Groups)
            {
                foreach (ComparinatorSet cs in cg.ComparinatorSets)
                {
                    foreach (ComparinatorCriteria rowCriteria in cs.ComparinatorCriterias)
                    {
                        rowCriteria.HighValueSelectedProducts =  0;
                        rowCriteria.LowValueSelectedProduct = int.MaxValue;
                        
                        //Evaluate Values
                        int sameCounter = 0;
                        string sameValue = string.Empty;
                        bool withoutValueToCompare = false;
                        int valueCounter = 0;
                        ProductCriteriaCell obj = (ProductCriteriaCell)rowCriteria.Values[products[0].Id];
                        if (obj != null)
                            sameValue = TypeUtility.GetAnonymous(obj, "value");

                        int yc = 0, nc = 0;
                        foreach (Product p in products)
                        {
                            object objProductCriteria = rowCriteria.Values[p.Id];
                            string cvalue = TypeUtility.GetAnonymous(objProductCriteria, "value");
                            rowCriteria.IsModify = true;
                            if (rowCriteria.Criteria.Type.Equals(CriteriaType.Numeric))
                            {
                                string valuedecimal = TypeUtility.GetAnonymous(objProductCriteria, "valuedecimal");
                                //cvalue = valuedecimal;
                                if (!string.IsNullOrEmpty(valuedecimal) && DecimalUtility.IsDecimal(valuedecimal)) //Empty is skiped
                                {
                                    decimal dcvalue = decimal.Parse(valuedecimal);
                                    if (dcvalue > rowCriteria.HighValueSelectedProducts)
                                        rowCriteria.HighValueSelectedProducts = dcvalue;

                                    if (dcvalue < rowCriteria.LowValueSelectedProduct)
                                        rowCriteria.LowValueSelectedProduct = dcvalue;
                                }
                                else 
                                {
                                    if (!string.IsNullOrEmpty(cvalue))
                                    { 
                                    
                                    }
                                }
                            }
                            else if (rowCriteria.Criteria.Type.Equals(CriteriaType.Boolean))
                            {
                                cvalue = cvalue.ToUpper();
                                yc = yc + ((cvalue.Equals("YES") || cvalue.Equals("Y")) ? 1 : 0);
                                nc = nc + ((cvalue.Equals("NO") || cvalue.Equals("N")) ? 1 : 0);
                            }
                            //Code for evaluate if is equal
                            if (rowCriteria.Criteria.Type.Equals(CriteriaType.Boolean))
                            {
                                if (cvalue.Equals(sameValue.ToUpper()))
                                    sameCounter++;
                                if (string.IsNullOrEmpty(cvalue) || cvalue.Equals(sameValue.ToUpper()))
                                    valueCounter++;
                            }
                            else
                            {
                                if (cvalue.Equals(sameValue))
                                    sameCounter++;
                            }
                        }
                        rowCriteria.IsEqual = sameCounter == products.Count;
                        //define boolean values
                        if ( rowCriteria.Criteria.Type.Equals(CriteriaType.Boolean) )
                        {
                            if (rowCriteria.IsEqual || (products.Count == valueCounter))
                            {
                                rowCriteria.HighValueSelectedProducts = null; //count Y
                                rowCriteria.LowValueSelectedProduct = null; //count N

                            }
                            else
                            {
                                rowCriteria.HighValueSelectedProducts = yc; //count Y
                                rowCriteria.LowValueSelectedProduct = nc; //count N
                            }
                        }

                    }
                }
            }



            //Start Price section
            IList<ComparinatorPriceRequired> pricerequired=new List<ComparinatorPriceRequired>();
            ComparinatorPriceRequired optional = new ComparinatorPriceRequired("Optional");
            ComparinatorPriceRequired required = new ComparinatorPriceRequired("Required");

            
            IList<Pair<string,string>> pricetypes = new List<Pair<string,string>>();
            IList<System.Object[]> ptBI = PricingTypeService.GetByIndustryId(IndustryId, localcompanyid);
            if (ptBI != null && ptBI.Count > 0)
            {
                for (int i = 0; i < ptBI.Count; i++)
                {
                    pricetypes.Add(new Pair<string, string>(((object[])(object)ptBI[i])[1].ToString(), ((object[])(object)ptBI[i])[0].ToString()));
                }
            }
            //optional
            foreach (Pair<string, string> pt in pricetypes)
            {
                
                ComparinatorPriceType cpt=new ComparinatorPriceType();
                cpt.Name = pt.First;
                int maxLongPrices = 1;//need a row empty
                bool existSomePriceToProduct = false;
                foreach (Product p in products) //Criterias x Products
                {
                    
                    ComparinatorPrice price = new ComparinatorPrice();
                    price.Product = p;
                    price.Prices = PriceService.GetByRequiredPricingType(DomainObjectType.Product, p.Id, PriceRequired.Optional, decimal.Parse(pt.Second), localcompanyid);
                    if (price.Prices.Count > maxLongPrices)
                    {
                        maxLongPrices = price.Prices.Count;
                    }
                    if (price.Prices.Count > 0)
                    {
                        existSomePriceToProduct = true;
                    }
                    cpt.Prices.Add(price);
                }
                if (existSomePriceToProduct)
                {
                    int countOfProducts = products.Count;
                    cpt.x = maxLongPrices;
                    cpt.y = products.Count;
                    Price[,] array = new Price[maxLongPrices, countOfProducts];
                    for (int i = 0; i < countOfProducts; i++)
                    {
                        if (cpt.Prices.Count > i)
                        {
                            ComparinatorPrice temp = cpt.Prices[i];
                            for (int j = 0; j < maxLongPrices; j++)
                            {
                                array[j, i] = new Price(); //initialize with empty
                                if (temp.Prices.Count > 0 && j < temp.Prices.Count)
                                {
                                    array[j, i] = temp.Prices[j];
                                }
                            }
                        }
                    }
                    cpt.ArrayPrices = array;
                    optional.PriceTypes.Add(cpt);
                }
            }

            //required
            foreach (Pair<string, string> pt in pricetypes)
            {
                ComparinatorPriceType cpt = new ComparinatorPriceType();
                cpt.Name = pt.First;
                int maxLongPrices = 1;//need a row empty
                bool existSomePriceToProduct = false;
                foreach (Product p in products) //Criterias x Products
                {
                    ComparinatorPrice price = new ComparinatorPrice();
                    price.Product = p;
                    price.Prices = PriceService.GetByRequiredPricingType(DomainObjectType.Product, p.Id, PriceRequired.Required, decimal.Parse(pt.Second), localcompanyid);
                    if (price.Prices.Count > maxLongPrices)
                    {
                        maxLongPrices = price.Prices.Count;
                    }
                    if (price.Prices.Count > 0)
                    {
                        existSomePriceToProduct = true;
                    }
                        cpt.Prices.Add(price);
                }
                if (existSomePriceToProduct)
                {
                    int countOfProducts = products.Count;
                    cpt.x = maxLongPrices;
                    cpt.y = products.Count;
                    Price[,] array = new Price[maxLongPrices, countOfProducts];
                    for (int i = 0; i < countOfProducts; i++)
                    {
                        if (cpt.Prices.Count > i)
                        {
                            ComparinatorPrice temp = cpt.Prices[i];

                            for (int j = 0; j < maxLongPrices; j++)
                            {
                                if (temp.Prices.Count > 0)
                                {
                                    if (j < temp.Prices.Count)
                                    {
                                        array[j, i] = temp.Prices[j];
                                    }
                                    else
                                    {
                                        array[j, i] = new Price();
                                    }
                                }
                                else
                                {
                                    array[j, i] = new Price();
                                }

                            }

                        }
                    }
                    cpt.ArrayPrices = array;
                    required.PriceTypes.Add(cpt);
                }
            }
            if(required.PriceTypes.Count>0)
            pricerequired.Add(required);
            if (optional.PriceTypes.Count > 0)
            pricerequired.Add(optional);
            ExportDTO export = new ExportDTO();
            ViewData["CurrentCompanyName"]=string.Empty;
            ClientCompany clientCompany = ClientCompanyService.GetById(localcompanyid);
            decimal selectedValue = 0;
            if(clientCompany != null)
            {
                ViewData["CurrentCompanyName"] = clientCompany.Name;
                export.CurrentCompanyName = clientCompany.Name;
            }
            IList<Product> productList = new List<Product>();
            IList<Product> competitorProductList = new List<Product>();
            IList<Competitor> competitorIdByProductNameList = new List<Competitor>();
            string competitorProductIds = string.Empty;
            string prouctIdAndNames = string.Empty;
            foreach (Product product in products)
            {
                prouctIdAndNames += product.Id.ToString() + "_" + product.Name + ";";
                if (product.Competitor.Name.ToUpper().Equals(clientCompany.Name.ToUpper()) || product.Competitor.Name.ToUpper().Equals(clientCompany.Dns.ToUpper()))
                {
                    productList.Add(product);
                }
                else
                {
                    competitorProductList.Add(product);
                    if (!string.IsNullOrEmpty(competitorProductIds))
                    {
                        competitorProductIds += ":";
                    }
                    competitorProductIds += product.CompetitorId + "_" + product.Id;
                }
                
                if (product.Competitor.Id != null)
                {
                    Competitor objectCompetitor = new Competitor(product.Competitor.Id, product.Competitor.Name);
                    if (!competitorIdByProductNameList.Contains(objectCompetitor))
                    {
                        competitorIdByProductNameList.Add(objectCompetitor);
                        if (clientCompany.Name.ToUpper().Equals(product.Competitor.Name.ToUpper()) || clientCompany.Dns.ToUpper().Equals(product.Competitor.Name.ToUpper()))
                        {
                            selectedValue = product.Competitor.Id;
                        }
                    }
                }
            }
            prouctIdAndNames = prouctIdAndNames.Substring(0, prouctIdAndNames.Length - 1);
            ViewData["ProductIdAndNames"] = prouctIdAndNames;
            ViewData["ProductList"] = new SelectList(productList, "Id", "Name");
            export.ProductList = productList;
            ViewData["CompetitorIdByProduct"] = export.CompetitorIdByProduct = new SelectList(competitorIdByProductNameList, "Id", "Name", selectedValue);
            ViewData["ClientCompetitorId"] = export.ClientCompetitorId = selectedValue;
            ViewData["Products"] = export.Products = products; 
            ViewData["CompetitorProducts"] = export.CompetitorProducts = competitorProductList;
            ViewData["IndustryId"] = export.IndustryId = IndustryId;
            ViewData["CompetitorId"] = export.CompetitorId = CompetitorId;
            ViewData["ComparinatorGroups"] = export.ComparinatorGroups = catalog.Groups;
            ViewData["ComparinatorPriceRequired"] = export.ComparinatorPriceRequired = pricerequired;
            Session["ComparinatorGroups"] = catalog.Groups;
            ViewData["CompetitorProductIds"] = competitorProductIds;

            IList<Product> productsG1 = products.Where(a => a.IsClientCompetitor.Equals("Y")).ToList();
            IList<Product> productsG2 = products.Where(a => !a.IsClientCompetitor.Equals("Y")).ToList();

            ViewData["G1"] = productsG1;
            ViewData["G2"] = productsG2;

            ViewData["User"] = User;
            export.TabToExport = "All";
            export.ProductLabel = "Product";
            export.PositioningList = GetPositioning(export.ProductList, export.CompetitorProducts,localcompanyid);
            export.Industry = industry;
            export.IndustryLabel = ViewData["IndustryLabel"].ToString();
            export.ProductLbl = ViewData["ProductLabel"].ToString();
            Session["Export"] = export;
   
            ActionHistoryService.RecordCompareProductsAction((List<Product>)products, EntityAction.ComparedProduct, DomainObjectType.Product, ActionFrom.FrontEnd, localuserid, localcompanyid);

            ConfigurationDefaults configurationdefaults = new ConfigurationDefaults();
            configurationdefaults.ComparinatorExport = "false";
            configurationdefaults.IndustryStandars="false";
            configurationdefaults.EnableTools = "false";
            configurationdefaults.SameValues = "false";
            configurationdefaults.Benefit = "false";
            configurationdefaults.IndustryInfoTab = "false";
            configurationdefaults.InfoTab = "false";
            configurationdefaults.SilverBulletsTab = "false";
            configurationdefaults.PositioningTab = "false";
            configurationdefaults.PricingTab = "false";
            configurationdefaults.FeaturesTab = "false";
            configurationdefaults.SalesToolsTab = "false";
            configurationdefaults.NewsTab = "false";
            configurationdefaults.Cost = "false";
            configurationdefaults.DisabledPublicComment = "false";

            

            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(localcompanyid);
            if (configurations != null && configurations.Count > 0)
            {
                configurationdefaults = configurations[0];
                if (configurationUserType != null)
                {
                    configurationdefaults.ComparinatorExport = configurationUserType.ComparinatorExport;
                    
                    configurationdefaults.EnableTools = configurationUserType.EnableTools;

                    configurationdefaults.IndustryInfoTab = configurationUserType.IndustryInfo; ;
                    configurationdefaults.InfoTab = configurationUserType.Info;
                    configurationdefaults.SilverBulletsTab = configurationUserType.SilverBullets;
                    configurationdefaults.PositioningTab = configurationUserType.Positioning;
                    configurationdefaults.PricingTab = configurationUserType.Pricing;
                    configurationdefaults.FeaturesTab = configurationUserType.Features;
                    configurationdefaults.SalesToolsTab = configurationUserType.SalesTools;
                    configurationdefaults.NewsTab = configurationUserType.News;
                    
                    configurationdefaults.IndustryStandars = configurationUserType.IndustryStandars;
                    configurationdefaults.Benefit = configurationUserType.Benefit;
                    configurationdefaults.Cost = configurationUserType.Cost;
                }
            }
            if (!Convert.ToBoolean(configurationdefaults.InfoTab) && !Convert.ToBoolean(configurationdefaults.SilverBulletsTab) && !Convert.ToBoolean(configurationdefaults.PositioningTab) && !Convert.ToBoolean(configurationdefaults.PricingTab) && !Convert.ToBoolean(configurationdefaults.FeaturesTab))
                configurationdefaults.ComparinatorExport = "false";

            ViewData["ConfigurationDefaults"] = configurationdefaults;

           return View("Result");
        }

        public ActionResult GetNewsByProduct()
        {
            string cc = (string)Request["C"];
            string company = string.Empty;
            string CompetitorId = (string)Request["CompetitorId"];
            string EntityType = (string)Request["EntityType"];
            ViewData["CompetitorId"] = CompetitorId;
            ViewData["EntityType"] = EntityType;
            ViewData["CompetitorName"] = string.Empty;
            ViewData["ComparinatorNewsListTitle"] = string.Empty;
            if (!string.IsNullOrEmpty(CompetitorId))
            {
                decimal compId = decimal.Parse(CompetitorId);
                Competitor competitor = CompetitorService.GetById(compId);
                ViewData["CompetitorName"] = competitor.Name;
                ViewData["ComparinatorNewsListTitle"] = string.Format(LabelResource.ComparinatorNewsListByCompetitor, competitor.Name);
            }
            if (!string.IsNullOrEmpty(cc))
            {
                company = Encryptor.Decode(cc);
            }
            ViewData["BrowseDetailName"] = "NewsByCompetitorDetail";
            ViewData["DetailFilter"] = "{NewsByCompetitorDetailView.EntityId_Eq_" + CompetitorId + "}";
            ViewData["BrowseDetailFilter"] = "NewsByCompetitorDetailView.EntityId = " + CompetitorId + " And NewsByCompetitorDetailView.EntityType='COMPT'";
            ViewData["page"] = (string)Request["page"];
            ViewData["group"] = (string)Request["group"];
            ViewData["order"] = (string)Request["order"];
            ViewData["asc"] = (string)Request["asc"];
            ViewData["ClientCompany"] = company;
            return View("ResultNews");
        }

        public ActionResult SalesToolsProducts(String IndustryId, String CompetitorId, String ProductId)
        {
            string salesclientcompanyencode = (string)Request["C"];
            ViewData["C"] = salesclientcompanyencode;
            string salesuseridencode = (string)Request["U"];
            ViewData["U"] = salesuseridencode;
            string salestoolsclientcompany  = CurrentCompany;
            string salestoolsuserid = CurrentUser;
            if(!string.IsNullOrEmpty(salesclientcompanyencode))
            {
                salestoolsclientcompany = Encryptor.Decode(salesclientcompanyencode);
            }
            if (!string.IsNullOrEmpty(salesuseridencode))
            {
                salestoolsuserid = Encryptor.Decode(salesuseridencode); 
            }
            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            Product product = ProductService.GetById(Convert.ToDecimal(ProductId));
            if(product!= null &&  !CompetitorId.Equals(product.CompetitorId.ToString()))
            {
                CompetitorId=product.CompetitorId.ToString();
            }
            decimal[] listOfIds = new decimal[3] { Convert.ToDecimal(IndustryId), Convert.ToDecimal(CompetitorId), Convert.ToDecimal(ProductId)};
            LibraryCatalogCollection = GetContentTypeCatalog(listOfIds, salestoolsuserid, salestoolsclientcompany);
            
            ViewData["LibraryCatalog"] = LibraryCatalogCollection;
            string productImageUrl = GetUrlOfImage(product.ImageUrl);
            if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
            {
                string[] urlObjects = product.ImageUrl.Split('/');
                Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                if (newFileImage.FileFormat == null)
                {
                    productImageUrl = "/Content/Images/Icons/none.png";
                }
            }
            ViewData["ProductImageUrl"] = productImageUrl;
            ViewData["EntityDetail"] = "Product";
            ViewData["NameDetail"] = product.Name;
            ViewData["DescriptionDetail"] = product.Description;
            ViewData["ImageDetail"] = productImageUrl;
            ViewData["UrlDetail"] = product.Url;
            ViewData["UrlDetailText"] = GetTextOfURL(product.Url);
            GetDataOfConfiguration(salestoolsclientcompany);
            return View("SalesToolsResult");
        }

        private string GetTextOfURL(string url)
        {
            string textUrl = url;
            if (url.IndexOf('_') != -1)
            {
                textUrl = url.Replace('_', '-');
            }

            return textUrl;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DownloadSalesForce()
        {
            decimal ProjectId = decimal.Parse(Request["ProjectId"]);
            string salesuseridencode = (string)Request["U"];
            string salesuserid = Encryptor.Decode(salesuseridencode);

            UserProfile user = UserProfileService.GetByEmail(salesuserid);
            if (user == null)
            {
                user = UserProfileService.GetById(salesuserid);
            }
            salesuserid = user.Id;
            Compelligence.Domain.Entity.File file = FileService.GetByEntityId(ProjectId, DomainObjectType.Project);
            string check = StringUtility.CheckNull(Request["chk"]);
            if (file != null)
            {
                if (file.Description == null || file.Description == "")
                {
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
                        ProjectService.SaveDownload(ProjectId, user);
                        ActionHistoryService.RecordActionHistory(file.Id, EntityAction.Downloaded, DomainObjectType.File, ActionFrom.FrontEnd, salesuserid, user.ClientCompany);
                        if (System.IO.File.Exists(fullpath + file.PhysicalName))
                        {
                            try
                            {
                                GetDownloadFileResponse(path, file.PhysicalName, file.FileName);
                            }
                            catch (Exception e)
                            { }
                        }
                    }
                }
                else
                {
                    return Content(file.Description);
                }
            }
            else
            {
                return Content("NotFound");
            }
            return Content(string.Empty);
        }

        private IList<LibraryCatalog> GetContentTypeCatalog(decimal[] SchemeIds, string salestoolsuserid , string salestoolcompany)
        {

            IList<LibraryCatalog> LibraryCatalogCollection = new List<LibraryCatalog>();
            IList<WebsiteDetail> ActiveConfigDetails = new List<WebsiteDetail>();
            IList<ContentType> ContentTypes = ContentTypeService.GetAllActiveByClientCompany(salestoolcompany);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Positioning Statements", salestoolcompany, salestoolsuserid);
            ContentTypeService.CreateIfNotExist(ContentTypes, "Details Industry Competitor Product", salestoolcompany, salestoolsuserid);
            ContentTypeService.CreateIfNotExist(ContentTypes, "News", salestoolcompany, salestoolsuserid);
            //Disable Strengths/Weaknesses
            //ContentTypeService.CreateIfNotExist(ContentTypes, "Strengths/Weaknesses", salestoolcompany, salestoolsuserid);

            Website ActiveConfig = WebsiteService.GetActiveWebsite(salestoolcompany);

            if (ActiveConfig != null) // Getting Values by default
            {
                foreach (ContentType contenttype in ContentTypes)
                {
                    WebsiteDetail websitedetail = WebsiteDetailService.GetByContentType(ActiveConfig.Id, SchemeIds.Length, contenttype.Id);
                    ActiveConfigDetails.Add(websitedetail);
                    if (contenttype.Name.Equals("Details Industry Competitor Product"))
                    {
                        WebsiteDetail tempWebSiteDetail = WebsiteDetailService.GetByContentType(ActiveConfig.Id, 0, contenttype.Id);
                        if (tempWebSiteDetail.Displayable.Equals("N"))
                        {
                            ViewData["DetailsEntities"] = "no";
                        }
                    }
                }
                var ActiveConfigDetailsOrder = (from d in ActiveConfigDetails orderby d.Sequence select d);
                ActiveConfigDetails = ActiveConfigDetailsOrder.ToList<WebsiteDetail>();
                //Improve next lines
                IList<WebsitePanel> wspanels = WebsiteService.GetPanels(ActiveConfig.Id);
                string lefcontentWidth = null;
                if (wspanels == null || (wspanels != null && wspanels.Count < 5))
                {
                    WebsiteService.CreateDefaultPanels(ActiveConfig);
                    wspanels = WebsiteService.GetPanels(ActiveConfig.Id, WebsiteComponentType.Panel);
                }
                else
                {
                    for (int p = wspanels.Count - 1; p >= 0; p--)
                    {
                        if (!wspanels[p].ComponentType.Equals(WebsiteComponentType.Panel) || !wspanels[p].Displayable.Equals("Y"))
                        {
                            if (wspanels[p].ComponentType.Equals(WebsiteComponentType.Panel) && wspanels[p].Displayable.Equals("N"))
                            {
                                lefcontentWidth += "N";
                            }
                            wspanels.RemoveAt(p);

                        }
                    }


                }

                ViewData["Panels"] = wspanels ?? new List<WebsitePanel>();
                ViewData["LefContentWidth"] = lefcontentWidth;
            }


            //Fill All ContentType
            foreach (WebsiteDetail ActiveConfigDetail in ActiveConfigDetails) //Process all ContentTypeIds 
            {
                ContentType ActiveContentType = ContentTypeService.GetById(ActiveConfigDetail.ContentTypeId);

                if (ActiveContentType != null)  //User delete from ContenTypes
                {

                    LibraryCatalog oLibraryCatalog = new LibraryCatalog(ActiveContentType.Name);


                    //oLibraryCatalog.Width = ActiveConfigDetail.Ajust.Equals("S") ? "48%" : "97%";
                    //oLibraryCatalog.Height = ActiveConfigDetail.Ajust.Equals("S") ? "300" : "150";
                    oLibraryCatalog.CssClass = ActiveConfigDetail.Ajust.Equals("S") ? "contentBoxContainerSingle" : "contentBoxContainerDouble";
                    oLibraryCatalog.Displayable = ActiveConfigDetail.Displayable.Equals("Y");
                    IList<Project> Projects = new List<Project>();

                    switch (SchemeIds.Length)
                    {
                        case 0:
                            Projects = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId); break;
                        case 1:
                            Projects = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Industry, SchemeIds[0]); break;
                        case 2: //Process with Industry & Competitor
                            {
                                //Projects = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Industry, SchemeIds[0]); break;
                                IList<Project> ProjectCompetitors = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Competitor, SchemeIds[1]);
                                foreach (Project p in ProjectCompetitors)
                                {
                                    if (!Projects.Contains(p))
                                    {
                                        Projects.Add(p);
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                IList<Project> ProjectProducts = ProjectService.GetByContentType(ActiveConfigDetail.ContentTypeId, DomainObjectType.Product, SchemeIds[2]);
                                foreach (Project p in ProjectProducts)
                                {
                                    if (!Projects.Contains(p))
                                    {
                                        Projects.Add(p);
                                    }
                                }

                                break;
                            }
                    }
                    foreach (Project ActiveProject in Projects)
                    {
                        if (ActiveProject.Status.Equals(ProjectStatus.Published) && !ActiveProject.Visibility.Equals(ProjectVisibility.BackEnd))
                        {
                            Compelligence.Domain.Entity.File file = FileService.GetByEntityId(ActiveProject.Id, DomainObjectType.Project);
                            ActiveProject.File = file;
                            ActiveProject.Labels = LabelService.GetByEntityId(ActiveProject.Id);
                            ActiveProject.RatingAllowed = ProjectService.RatingAllowed(ActiveProject.Id, salestoolsuserid);
                            oLibraryCatalog.Projects.Add(ActiveProject);
                        }
                    }
                    //Improve next sentences
                    if (ActiveContentType.Name.Equals("News"))
                    {

                        switch (SchemeIds.Length)
                        {
                            case 1: //Industry
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Industry, SchemeIds[0], salestoolcompany); break;
                            case 2: //Industry+Competitor
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Competitor, SchemeIds[1], salestoolcompany); break;
                            case 3://Industry+Competitor+Product
                                oLibraryCatalog.Library = EntityNewsService.GetByEntityType(DomainObjectType.Product, SchemeIds[2], salestoolcompany); break;
                        }


                    }
                    if (ActiveContentType.Name.Equals("Strengths/Weaknesses"))
                    {
                        if (SchemeIds.Length > 1)
                        {
                            try
                            {
                                oLibraryCatalog.Strenths = GetExternalCompetitor(SchemeIds[1], LocalCompetitorType.Strengths);
                                oLibraryCatalog.Weaknesses = GetExternalCompetitor(SchemeIds[1], LocalCompetitorType.Weaknesses);
                            }
                            catch (Exception ex)
                            {
                                oLibraryCatalog.Strenths = null;
                                oLibraryCatalog.Weaknesses = null;
                            }
                        }
                    }

                    LibraryCatalogCollection.Add(oLibraryCatalog);
                }
            }
            //Process with reference to ProjectId

            return LibraryCatalogCollection;
        }

        private IList<string> GetExternalCompetitor(decimal competitorId, string ExternalCompetitorType)
        {
            String name = CompetitorService.GetById(competitorId).Name;
            ClientCompany clientcompany = ClientCompanyService.GetById(CurrentCompany);
            IList<string> result = SalesForceService.GetSalesforceExternalCompetitors(clientcompany.SalesForceUser, clientcompany.SalesForcePassword + clientcompany.SalesForceToken, name, ExternalCompetitorType);
            return result;
        }


        public ActionResult GetEntityValues(String ObjectType, String entityid, String criteriaid, String industryid)
        {
            string result = string.Empty;

            if (ObjectType.Equals(DomainObjectType.Product))
            {
                ProductCriteriaId id = new ProductCriteriaId(decimal.Parse(entityid), decimal.Parse(criteriaid), decimal.Parse(industryid));
                ProductCriteria productcriteria = ProductCriteriaService.GetById(id);
                if (productcriteria == null)
                {
                    result = "@@@";
                }
                else
                {
                    result = productcriteria.Value + "@";
                    result += productcriteria.Notes + "@";
                    result += productcriteria.Links + "@";
                    result += productcriteria.Feature;
                }
            }
            return Content(result);
        }

        public ActionResult SaveBenefit(FormCollection form)
        {
            string criteriaid = form["txtCriteriaId"];
            string value = form["txtValue"];

            Criteria criteria = CriteriaService.GetById(Convert.ToDecimal(criteriaid));

            if (criteria != null)
            {
                value = value.Trim();//remove the space empty to begin and end
                if (value.Length > 200) value = value.Substring(0, 200);//if the value[benefit] is greater to 200 characters then get the first 200 charactes
                criteria.Benefit = value;
                CriteriaService.Update(criteria);
            }

            return Content(string.Empty + value + "@");
        }
        public ActionResult SaveCost(FormCollection form)
        {
            string criteriaid = form["txtCriteriaId"];
            string value = form["txtValue"];

            Criteria criteria = CriteriaService.GetById(Convert.ToDecimal(criteriaid));

            if (criteria != null)
            {
                value = value.Trim();//remove the space empty to begin and end
                if (value.Length > 200) value = value.Substring(0, 200);//if the value[cost] is greater to 200 characters then get the first 200 charactes
                criteria.Cost = value;
                CriteriaService.Update(criteria);
            }

            return Content(string.Empty + value + "@");
        }
        public ActionResult SaveIndustryStandard(FormCollection form)
        {
            string criteriaid = form["txtCriteriaId"];
            string value = form["txtValue"];

            Criteria criteria = CriteriaService.GetById(Convert.ToDecimal(criteriaid));

            if (criteria != null)
            {
                criteria.IndustryStandard = value;
                CriteriaService.Update(criteria);
            }
            return Content(string.Empty + value + "@");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBack(decimal id, FormCollection form)
        {
            string sIndustryId = form["IndustryId"];
            string criteriaid = form["txtCriteriaId"];
            string entityid = form["txtEntityId"];

            string salesuseridencode = (string)Request["U"];
            string salesclientcompanyencode = (string)Request["C"];

            if (sIndustryId == null || criteriaid == null || entityid == null)
            {
                ForumResponse forumResponse = new ForumResponse();
                string domainObjectType = StringUtility.CheckNull(Request["objt"]);

                SetDefaultDataForSave(forumResponse);
                forumResponse.EntityId = id;
                forumResponse.EntityType = domainObjectType;
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                {
                    forumResponse.ClientCompany = Encryptor.Decode(salesclientcompanyencode);
                    forumResponse.CreatedBy = Encryptor.Decode(salesuseridencode);
                    forumResponse.LastChangedBy = Encryptor.Decode(salesuseridencode);
                }

                ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);
                ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, domainObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                return null;
            }
            else
            {
                ProductCriteriaId ProductCriteriaId = new ProductCriteriaId(decimal.Parse(entityid), decimal.Parse(criteriaid), decimal.Parse(sIndustryId));
                ForumResponse forumResponse = new ForumResponse();
                string domainObjectType = StringUtility.CheckNull(Request["objt"]);

                forumResponse.EntityId = id;
                forumResponse.EntityType = domainObjectType;
                SetDefaultDataForSave(forumResponse);
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                if (!String.IsNullOrEmpty(salesclientcompanyencode)) // Get From SFDC, temporally
                {
                    forumResponse.ClientCompany = Encryptor.Decode(salesclientcompanyencode);
                    forumResponse.CreatedBy = Encryptor.Decode(salesuseridencode);
                    forumResponse.LastChangedBy = Encryptor.Decode(salesuseridencode);
                }
                ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);
                ActionHistoryService.CommentsActionHistory(id, ProductCriteriaId, EntityAction.FeedBack, domainObjectType, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
                return null;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackBenefit(decimal id, FormCollection form)
        {
            if (id > 0)
            {
                string salesuserid = GetDecodeParam("U");
                string salesclientid = GetDecodeParam("C");
                string localCurrentUser = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
                string localCurrentCompany = string.IsNullOrEmpty(salesclientid) ? CurrentCompany : salesclientid;
                ForumResponse forumResponse = new ForumResponse();
                forumResponse.EntityId = id;
                forumResponse.EntityType = DomainObjectType.Industry;
                SetDefaultDataForSave(forumResponse,localCurrentCompany,localCurrentUser);
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);
            }
            return Content(string.Empty);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackCost(decimal id, FormCollection form)
        {
            if (id > 0)
            {
                string salesuserid = GetDecodeParam("U");
                string salesclientid = GetDecodeParam("C");
                string localCurrentUser = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
                string localCurrentCompany = string.IsNullOrEmpty(salesclientid) ? CurrentCompany : salesclientid;
                ForumResponse forumResponse = new ForumResponse();

                forumResponse.EntityId = id;
                forumResponse.EntityType = DomainObjectType.Industry;
                SetDefaultDataForSave(forumResponse,localCurrentCompany,localCurrentUser);
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);
            }
            return Content(string.Empty);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackSilverBullets(FormCollection form)
        {
            decimal id = decimal.Parse(Request["Id"]);
            decimal productId = decimal.Parse(Request["pId"]);
            if (id > 0)
            {
                string salesuserid = GetDecodeParam("U");
                string salesclientid = GetDecodeParam("C");
                string localCurrentUser = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
                string localCurrentCompany = string.IsNullOrEmpty(salesclientid) ? CurrentCompany : salesclientid;
                ForumResponse forumResponse = new ForumResponse();
                forumResponse.EntityId = (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey(); //Id temporal no exist in database
                forumResponse.EntityType = "SILVER";
                forumResponse.ForumId= (decimal)UniqueKeyGenerator.GetInstance().GetUniqueKey(); //Id temporal no exist in database
                SetDefaultDataForSaveEntity(forumResponse, localCurrentUser, localCurrentCompany);
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                ForumService.SaveForumEntityResponse(forumResponse,id, productId ,ForumType.FeedBack);
            }
            return Content(string.Empty);
        }

        public void Export()
        {
            string type = StringUtility.CheckNull(Request["tp"]);
            bool exportFiltered = (string.Compare(StringUtility.CheckNull(Request["ef"]), "y", true) == 0);
            string filters = StringUtility.CheckNull(Request["ft"]);
            string textFilter = StringUtility.CheckNull(Request["tf"]);
            bool hideSameValues = (string.Compare(StringUtility.CheckNull(Request["he"]), "y", true) == 0);
            bool showIndustryStandard = (string.Compare(StringUtility.CheckNull(Request["sis"]), "y", true) == 0);
            bool showBenefit = (string.Compare(StringUtility.CheckNull(Request["sb"]), "y", true) == 0);
            string criteriaSetIdHidden = StringUtility.CheckNull(Request["hcs"]);
            string criteriaIdHidden = StringUtility.CheckNull(Request["hc"]);
            bool showCost = (string.Compare(StringUtility.CheckNull(Request["sc"]), "y", true) == 0);
            if (!string.IsNullOrEmpty(criteriaSetIdHidden))
            {
                criteriaSetIdHidden = criteriaSetIdHidden.Replace("tbl", "");
            }
            List<string> filterOptions = new List<string>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterOptions = filters.Split(new char[] { ':' }).ToList();
            }
            string csIdBCHidden = StringUtility.CheckNull(Request["csbc"]);
            if (!string.IsNullOrEmpty(csIdBCHidden))
            {
                csIdBCHidden = csIdBCHidden.Replace("tbl", "");
            }
            string fileNameTemplate = GetFileNameWithCompanyAndIndustry("Comparinator Content Features","csv");
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            // Create the CSV file to which dataTable will be exported.	
            ExportService.ExportComparinatorToCSV(Session, type, exportFiltered, filterOptions, textFilter, hideSameValues, showIndustryStandard, showBenefit, showCost,  fullpath, tempfile, criteriaSetIdHidden, criteriaIdHidden, csIdBCHidden, CurrentUser, CurrentCompany);
            //GetDownloadFileResponse(path, tempfile+".csv", "ComparinatorContent.csv");
            //GetDownloadFileResponseByExtension(path, tempfile, "ComparinatorContent.csv", ".csv");
            GetDownloadFileResponseByContentType(path, tempfile, fileNameTemplate, "application/vnd.ms-excel");
        }

		public void ExportToWord()
        {
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();

            ExportDTO export = (ExportDTO)Session["Export"];

            bool DefaultsIndustryInfoTab = false;
            bool DefaultsInfoTab = false;
            bool DefaultsSilverBulletsTab = false;
            bool DefaultsPositioningTab = false;
            bool DefaultsPricingTab = false;
            bool DefaultsFeaturesTab = false;

            string salesclientcompanyencode=StringUtility.CheckNull(Request["C"]);
            string localclientcompany = string.IsNullOrEmpty(salesclientcompanyencode) ? CurrentCompany : Encryptor.Decode(salesclientcompanyencode);

            string salesuserid = GetDecodeParam("U");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            UserProfile User = UserProfileService.GetById(localuserid);
            SetSecurityGroupIdToUser(User);
            ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(User.SecurityGroupId, localclientcompany);
            if (configurationUserType != null)
            {
                DefaultsIndustryInfoTab = Convert.ToBoolean(configurationUserType.IndustryInfo);
                DefaultsInfoTab = Convert.ToBoolean(configurationUserType.Info);
                DefaultsSilverBulletsTab = Convert.ToBoolean(configurationUserType.SilverBullets);
                DefaultsPositioningTab = Convert.ToBoolean(configurationUserType.Positioning);
                DefaultsPricingTab = Convert.ToBoolean(configurationUserType.Pricing);
                DefaultsFeaturesTab = Convert.ToBoolean(configurationUserType.Features);
            }

            bool exit = ExportService.ExportToWord(fullpath + tempfile, export, DefaultsIndustryInfoTab, DefaultsInfoTab, DefaultsSilverBulletsTab, DefaultsPositioningTab, DefaultsPricingTab, DefaultsFeaturesTab);
            string fileNameTemplate = GetFileNameWithCompanyAndIndustry("Comparinator Content ", "docx");
            if(exit)
                GetDownloadFileResponseByContentType(path, tempfile, fileNameTemplate, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }

		public void ExportComparinatorTabsToPDF()
        {
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();

            bool DefaultsIndustryInfoTab = false;
            bool DefaultsInfoTab = false;
            bool DefaultsSilverBulletsTab = false;
            bool DefaultsPositioningTab = false;
            bool DefaultsPricingTab = false;
            bool DefaultsFeaturesTab = false;

            string salesclientcompanyencode = StringUtility.CheckNull(Request["C"]);
            string localclientcompany = string.IsNullOrEmpty(salesclientcompanyencode) ? CurrentCompany : Encryptor.Decode(salesclientcompanyencode);
            
            string salesuserid = GetDecodeParam("U");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            UserProfile User = UserProfileService.GetById(localuserid);
            SetSecurityGroupIdToUser(User);
            ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(User.SecurityGroupId, localclientcompany);
            if (configurationUserType != null)
            {
                DefaultsIndustryInfoTab = Convert.ToBoolean(configurationUserType.IndustryInfo);
                DefaultsInfoTab = Convert.ToBoolean(configurationUserType.Info);
                DefaultsSilverBulletsTab = Convert.ToBoolean(configurationUserType.SilverBullets);
                DefaultsPositioningTab = Convert.ToBoolean(configurationUserType.Positioning);
                DefaultsPricingTab = Convert.ToBoolean(configurationUserType.Pricing);
                DefaultsFeaturesTab = Convert.ToBoolean(configurationUserType.Features);
            }

            ExportDTO export = (ExportDTO)Session["Export"];
            string fileNameTemplate = GetFileNameWithCompanyAndIndustry("Comparinator Content ", "pdf");
            bool exit = ExportService.ExportToPDF(fullpath + tempfile, export, DefaultsIndustryInfoTab, DefaultsInfoTab, DefaultsSilverBulletsTab, DefaultsPositioningTab, DefaultsPricingTab, DefaultsFeaturesTab);
            
            if (exit)
                GetDownloadFileResponseByContentType(path, tempfile, fileNameTemplate, "application/pdf");
        }

		public void ExportComparinatorTabsToExcel()
        {
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();

            bool DefaultsIndustryInfoTab = false;
            bool DefaultsInfoTab = false;
            bool DefaultsSilverBulletsTab = false;
            bool DefaultsPositioningTab = false;
            bool DefaultsPricingTab = false;
            bool DefaultsFeaturesTab = false;

            string salesclientcompanyencode = StringUtility.CheckNull(Request["C"]);
            string localclientcompany = string.IsNullOrEmpty(salesclientcompanyencode) ? CurrentCompany : Encryptor.Decode(salesclientcompanyencode);

            string salesuserid = GetDecodeParam("U");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            UserProfile User = UserProfileService.GetById(localuserid);
            SetSecurityGroupIdToUser(User);
            ConfigurationUserType configurationUserType = ConfigurationUserTypeService.GetBySecurityGroupAndCompany(User.SecurityGroupId, localclientcompany);
            if (configurationUserType != null)
            {
                DefaultsIndustryInfoTab = Convert.ToBoolean(configurationUserType.IndustryInfo);
                DefaultsInfoTab = Convert.ToBoolean(configurationUserType.Info);
                DefaultsSilverBulletsTab = Convert.ToBoolean(configurationUserType.SilverBullets);
                DefaultsPositioningTab = Convert.ToBoolean(configurationUserType.Positioning);
                DefaultsPricingTab = Convert.ToBoolean(configurationUserType.Pricing);
                DefaultsFeaturesTab = Convert.ToBoolean(configurationUserType.Features);
            }
            ExportDTO export = (ExportDTO)Session["Export"];
            string fileNameTemplate = GetFileNameWithCompanyAndIndustry("Comparinator Content ", "xlsx");
            bool exit = ExportService.ExportToExcel(fullpath + tempfile, export, DefaultsIndustryInfoTab, DefaultsInfoTab, DefaultsSilverBulletsTab, DefaultsPositioningTab, DefaultsPricingTab, DefaultsFeaturesTab);
            if (exit)
                GetDownloadFileResponseByContentType(path, tempfile, fileNameTemplate, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public void ExportToPDF()
        {
            string type = StringUtility.CheckNull(Request["tp"]);
            bool exportFiltered = (string.Compare(StringUtility.CheckNull(Request["ef"]), "y", true) == 0);
            string filters = StringUtility.CheckNull(Request["ft"]);
            string textFilter = StringUtility.CheckNull(Request["tf"]);
            bool hideSameValues = (string.Compare(StringUtility.CheckNull(Request["he"]), "y", true) == 0);
            bool showIndustryStandard = (string.Compare(StringUtility.CheckNull(Request["sis"]), "y", true) == 0);
            bool showBenefit = (string.Compare(StringUtility.CheckNull(Request["sb"]), "y", true) == 0);
            bool showCost = (string.Compare(StringUtility.CheckNull(Request["sc"]), "y", true) == 0);
            string criteriaSetIdHidden = StringUtility.CheckNull(Request["hcs"]);
            string csIdBCHidden = StringUtility.CheckNull(Request["csbc"]);
            string criteriaIdHidden = StringUtility.CheckNull(Request["hc"]);
            if (!string.IsNullOrEmpty(csIdBCHidden))
            {
                csIdBCHidden = csIdBCHidden.Replace("tbl", "");
            }
            if (!string.IsNullOrEmpty(criteriaSetIdHidden))
            {
                criteriaSetIdHidden = criteriaSetIdHidden.Replace("tbl", "");
            }
            List<string> filterOptions = new List<string>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterOptions = filters.Split(new char[] { ':' }).ToList();
            }

            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            string fileNameTemplate = GetFileNameWithCompanyAndIndustry("Comparinator Content Features","pdf");
            // Create the PDF file to which dataTable will be exported.
            ExportService.ExportComparinatorToPDF(Session, type, exportFiltered, filterOptions, textFilter, hideSameValues, showIndustryStandard, showBenefit, showCost, fullpath, tempfile, criteriaSetIdHidden, criteriaIdHidden, csIdBCHidden, CurrentUser, CurrentCompany);
            //GetDownloadFileResponse(path, tempfile + ".pdf", "ComparinatorContent.pdf");
            //GetDownloadFileResponseByExtension(path, tempfile, "ComparinatorContent.pdf", ".pdf");
            GetDownloadFileResponseByContentType(path, tempfile, fileNameTemplate, "application/pdf");
        }

        public void ExportToExcel()
        {
            string type = StringUtility.CheckNull(Request["tp"]);
            bool exportFiltered = (string.Compare(StringUtility.CheckNull(Request["ef"]), "y", true) == 0);
            string filters = StringUtility.CheckNull(Request["ft"]);
            string textFilter = StringUtility.CheckNull(Request["tf"]);
            bool hideSameValues = (string.Compare(StringUtility.CheckNull(Request["he"]), "y", true) == 0);
            bool showIndustryStandard = (string.Compare(StringUtility.CheckNull(Request["sis"]), "y", true) == 0);
            bool showBenefit = (string.Compare(StringUtility.CheckNull(Request["sb"]), "y", true) == 0);
            string criteriaSetIdHidden = StringUtility.CheckNull(Request["hcs"]);
            string csIdBCHidden = StringUtility.CheckNull(Request["csbc"]);
            string criteriaIdHidden = StringUtility.CheckNull(Request["hc"]);
            bool showCost = (string.Compare(StringUtility.CheckNull(Request["sc"]), "y", true) == 0);
            if (!string.IsNullOrEmpty(criteriaSetIdHidden))
            {
                criteriaSetIdHidden = criteriaSetIdHidden.Replace("tbl", "");
            }
            if (!string.IsNullOrEmpty(csIdBCHidden))
            {
                csIdBCHidden = csIdBCHidden.Replace("tbl", "");
            }
            List<string> filterOptions = new List<string>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterOptions = filters.Split(new char[] { ':' }).ToList();
            }

            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            string fileNameTemplate = GetFileNameWithCompanyAndIndustry("Comparinator Content Features", "xls");
            ExportService.ExportComparinatorToExcel(Session, type, exportFiltered, filterOptions, textFilter, hideSameValues, showIndustryStandard, showBenefit, showCost, fullpath, tempfile, criteriaSetIdHidden, criteriaIdHidden, csIdBCHidden, CurrentUser, CurrentCompany);
            //GetDownloadFileResponse(path, tempfile +".xls", "ComparinatorContent.xls");
            //GetDownloadFileResponseByExtension(path, tempfile, "ComparinatorContent.xls", ".xls");
            GetDownloadFileResponseByContentType(path, tempfile, fileNameTemplate, "application/vnd.ms-excel");

        }
        public void ExportToExcelHtml()
        {
            string type = StringUtility.CheckNull(Request["tp"]);
            string filters = StringUtility.CheckNull(Request["ft"]);
            List<string> filterOptions = new List<string>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterOptions = filters.Split(new char[] { ':' }).ToList();
            }
            string path = ConfigurationSettings.AppSettings["ContentFilePath"].Substring(1);
            string fullpath = AppDomain.CurrentDomain.BaseDirectory + path;
            string tempfile = "temp" + UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
            IList<Product> products = (List<Product>)Session["Products"];
            //BEGIN EXPORT HTML          
            FileStream fs = new FileStream(fullpath + tempfile, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter WriteLine = new StreamWriter(fs);
            StringBuilder Html = new StringBuilder();
            int Columns, Colspan;
            Columns = 4 + products.Count;

            string Fonts = " font: bold 11px tahoma,arial,helvetica;";
            string FontsRows = "style='font: 11px Verdana,Arial;'";
            Html.Append(@"<tr> <b>");
            Html.Append(@"<table WIDTH=800  border=1 BORDERCOLOR='#AAAAAA'>");

            if (type.Equals("2"))
            {
                IList<Competitor> competitors = (List<Competitor>)Session["Competitors"];
                Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> INDUSTRY </th>");
                Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> COMPETITOR </th>");
                Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> DESCRIPTION </th>");
                foreach (Competitor c in competitors)
                {
                    string sIndustry = c.Industry == null ? string.Empty : c.Industry.Name;
                }
                ActionHistoryService.RecordCompareCompetitorsAction((List<Competitor>)competitors, EntityAction.ExportComparisonCriterias, DomainObjectType.CriteriaGroup, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            }
            else
            {
                ActionHistoryService.RecordCompareProductsAction((List<Product>)products, EntityAction.ExportComparisonCriterias, DomainObjectType.CriteriaGroup, ActionFrom.FrontEnd, CurrentUser, CurrentCompany);
            }
            Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> GROUP </th>");
            Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> SET </th>");
            Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> CRITERIA </th>");
            Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> INDUSTRY STANDARD </th>");
            string csvheader = "GROUP,SET,CRITERIA, INDUSTRY STANDARD,";
            if (type.Equals("2"))
            {
                IList<Competitor> competitors = (List<Competitor>)Session["Competitors"];
                foreach (Competitor c in competitors)
                {
                    Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> " + c.Name + "</th>");
                }
            }
            else
            {
                foreach (Product p in products)
                {
                    Html.Append(@"<th style='background-color:#EEEFEF;" + Fonts + "'> " + p.Competitor.Name + " - " + p.Name + " </th>");

                }
                Html.Append(@"</b> </tr>");
            }
            IList<ComparinatorGroup> comparinatorgroups = (IList<ComparinatorGroup>)Session["ComparinatorGroups"];
            foreach (ComparinatorGroup cg in comparinatorgroups)
            {
                string csvNameGroup = string.Empty;
                csvNameGroup += cg.Name + ",";
                foreach (ComparinatorSet cs in cg.ComparinatorSets)
                {
                    string csvNameGroupAndSet = string.Empty;
                    csvNameGroupAndSet += csvNameGroup + cs.Name + ",";
                    foreach (ComparinatorCriteria cc in cs.ComparinatorCriterias)
                    {
                        bool isFilteredCriteria = false;
                        string Pro1 = string.Empty;
                        string Pro2 = string.Empty;
                        string Pro3 = string.Empty;
                        string Pro4 = string.Empty;
                        string Pro5 = string.Empty;
                        string csvLine = string.Empty;
                        string ProducValue = string.Empty;
                        string ProducIsEmpty01 = string.Empty;

                        IList<string> listass = new List<string>();
                        foreach (object value in cc.Values)
                        {
                            if (value is IndustryCriteria)
                            { }
                            else if (value is CompetitorCriteria)
                            {
                                CompetitorCriteria creload = CompetitorCriteriaService.GetByOtherIds((decimal)(value as CompetitorCriteria).Id.CompetitorId, (decimal)(value as CompetitorCriteria).Id.CriteriaId);
                            }
                            else if (value is ProductCriteria)
                            {
                                ProductCriteria critProd = (ProductCriteria)value;
                                listass.Add(critProd.Value);

                                if (filterOptions.Contains(critProd.Feature))
                                {
                                    isFilteredCriteria = true;
                                }
                            }
                        }
                        if (isFilteredCriteria || (filterOptions.Count == 0))
                        {
                            string GroupName;
                            GroupName = csvNameGroup.Substring(0, csvNameGroup.Length - 1);
                            if (Columns == 5)
                            {
                                Pro1 = listass[0].ToString();
                                Html.Append(@"<tr>");
                                Html.Append(@"<td " + FontsRows + ">" + GroupName + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cs.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.IndustryStandard + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro1 + "</td>");
                                Html.Append(@"</tr>");
                            }
                            else if (Columns == 6)
                            {
                                Pro1 = listass[0].ToString();
                                Pro2 = listass[1].ToString();

                                Html.Append(@"<tr>");
                                Html.Append(@"<td " + FontsRows + ">" + GroupName + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cs.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.IndustryStandard + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro1 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro2 + "</td>");
                                Html.Append(@"</tr>");
                            }
                            else if (Columns == 7)
                            {
                                Pro1 = listass[0].ToString();
                                Pro2 = listass[1].ToString();
                                Pro3 = listass[2].ToString();
                                Html.Append(@"<tr>");
                                Html.Append(@"<td " + FontsRows + ">" + GroupName + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cs.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.IndustryStandard + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro1 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro2 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro3 + "</td>");
                                Html.Append(@"</tr>");
                            }
                            else if (Columns == 8)
                            {
                                Pro1 = listass[0].ToString();
                                Pro2 = listass[1].ToString();
                                Pro3 = listass[2].ToString();
                                Pro4 = listass[3].ToString();
                                Html.Append(@"<tr>");
                                Html.Append(@"<td " + FontsRows + ">" + GroupName + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cs.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.IndustryStandard + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro1 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro2 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro3 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro4 + "</td>");
                                Html.Append(@"</tr>");
                            }
                            else if (Columns == 9)
                            {
                                Pro1 = listass[0].ToString();
                                Pro2 = listass[1].ToString();
                                Pro3 = listass[2].ToString();
                                Pro4 = listass[3].ToString();
                                Pro5 = listass[4].ToString();
                                Html.Append(@"<tr>");
                                Html.Append(@"<td " + FontsRows + ">" + GroupName + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cs.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.Name + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + cc.Criteria.IndustryStandard + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro1 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro2 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro3 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro4 + "</td>");
                                Html.Append(@"<td " + FontsRows + ">" + Pro5 + "</td>");
                                Html.Append(@"</tr>");
                            }
                        }
                    }
                }
            }
            try
            {
                WriteLine.Write(Html.ToString());
            }
            catch (Exception ex)
            { }
            finally
            {
                if (WriteLine != null)
                    WriteLine.Close();
            }
            GetDownloadFileResponse(path, tempfile, "ComparinatorContent.xls");
        }
        public ActionResult EditValues()
        {
            String industryid = Request.Params["IndustryId"];
            String objecttype = Request.Params["Type"];
            String entityid = Request.Params["EntityId"];
            String criteriaid = Request.Params["CriteriaId"];
            ProductCriteria pc = null;
            if (objecttype.Equals(DomainObjectType.Product))
            {
                pc = ProductCriteriaService.GetByIndustry(decimal.Parse(industryid), decimal.Parse(entityid), decimal.Parse(criteriaid));
                if (pc == null)
                    pc = ProductCriteriaService.GetNew(decimal.Parse(industryid), decimal.Parse(entityid), decimal.Parse(criteriaid));
            }
            else//Competitor, Not Yet
            {

            }
            ViewData["IndustryId"] = industryid;
            ViewData["EntityId"] = industryid;
            ViewData["Type"] = objecttype;
            ViewData["ExternalId"] = pc.ExternalId;
            ViewData["Value"] = pc.Value;
            ViewData["Notes"] = pc.Notes;
            ViewData["Links"] = pc.Links;
            ViewData["Feature"] = pc.Feature;

            return View("Edit");
        }


        #endregion

        #region Private Methods


        #endregion

        public ContentResult GetHowWePosition() //Update Competitor and Product
        {
            string sProductId = Request.Params["ProductId"];
            string sProdCompIds = Request.Params["ProdCompIds"];
            string[] pcIds = sProdCompIds.Split(':');
            string result = string.Empty;

            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");

            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salescompanyid) ? CurrentCompany : salescompanyid;

            IList<Product> products = (List<Product>)Session["Products"];
            if (products != null)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].Id.ToString().Equals(sProductId))
                    {
                        result = products[i].Name;
                        Positioning positioning = PositioningService.GetPositiongByProductAndParent(PositioningRelation.Positioning, DomainObjectType.Product, products[i].Id,(decimal) products[i].CompetitorId, (decimal)products[i].Competitor.Industry.Id, localcompanyid);
                                
                        if (positioning != null)
                        {
                            PositioningIndustryId positioningIndustryId = new PositioningIndustryId(positioning.Id, (decimal)products[i].Competitor.Industry.Id);
                            PositioningIndustry pi = PositioningIndustryService.GetById(positioningIndustryId);

                            if(positioning.EntityId==products[i].Id && pi!= null && !positioning.IsGlobal.Equals("Y"))
                                positioning.VirtualAction = "Update";
                            else
                                positioning.VirtualAction = "Create";
                        }
                        else
                        {
                            positioning = PositioningService.GetPositiongAndParent(PositioningRelation.Positioning, DomainObjectType.Competitor, (decimal)products[i].CompetitorId, (decimal)products[i].Competitor.Industry.Id, localcompanyid);
                            if (positioning == null)
                            {
                                if (positioning == null)
                                {
                                    positioning = new Positioning();
                                }
                            }
                            positioning.VirtualAction = "Create";
                        }

                        result = "HowWePosition_" + sProductId + ":" + positioning.HowWePosition;
                        result += "[T+K]PositioningId_" + sProductId + ":" + positioning.Id;
                        result += "[T+K]Action_" + sProductId + ":" + positioning.VirtualAction;
                        result += "[T+K]Statement_" + sProductId + ":" + positioning.Name;

                        if (pcIds.Length > 0)
                        {
                            foreach (string key in pcIds)
                            {
                                if (!string.IsNullOrEmpty(key))
                                {
                                    string[] ids = key.Split('_');
                                    if (ids.Length == 2)
                                    {
                                        decimal competitorId = decimal.Parse(ids[0]);
                                        Positioning competitiveMessaging = PositioningService.GetCompetitiveMessaginByProdt(PositioningRelation.CompetitiveMessaging, DomainObjectType.Product, decimal.Parse(sProductId), competitorId, (decimal)products[i].Competitor.Industry.Id, localcompanyid);
                                        if (competitiveMessaging != null)
                                        {
                                            PositioningCompetitorId positioningCompetitorId = new PositioningCompetitorId(competitiveMessaging.Id, competitorId);
                                            PositioningCompetitor pc = PositioningCompetitorService.GetById(positioningCompetitorId);
                                            PositioningIndustryId positioningIndustryId = new PositioningIndustryId(competitiveMessaging.Id, (decimal)products[i].Competitor.Industry.Id);
                                            PositioningIndustry pi = PositioningIndustryService.GetById(positioningIndustryId);
                                    
                                            if(competitiveMessaging.EntityId==products[i].Id && pc!=null && pi!= null && !competitiveMessaging.IsGlobal.Equals("Y"))
                                                competitiveMessaging.VirtualAction = "Update";
                                            else
                                                competitiveMessaging.VirtualAction = "Create";
                                        }
                                        else
                                        {
                                            Competitor competitor = CompetitorService.GetCompetitorClient(localcompanyid);
                                                                                                  //GetCompetitiveMessaginAndParent(string positioningRelation, string entitytype,         decimal entityid, decimal competitorId, decimal industryId, string clientCompany);
                                            competitiveMessaging = PositioningService.GetCompetitiveMessaginAndParent(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, competitor.Id, competitorId, products[i].Competitor.Industry.Id, localcompanyid);
                                            //competitiveMessaging = PositioningService.GetCMParentListByProductOnComparinator(PositioningRelation.CompetitiveMessaging, DomainObjectType.Product, products[i].Id, (decimal)products[i].Competitor.Industry.Id, competitorId, localcompanyid);
                                            if (competitiveMessaging == null)
                                            {
                                                //IList<Positioning> competitiveMessagingList = PositioningService.GetPositioningListByProductOnComparinator(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, products[i].Competitor.Id, (decimal)products[i].Competitor.Industry.Id, competitorId, localcompanyid);
                                                //if (competitiveMessagingList != null && competitiveMessagingList.Count > 0)
                                                //{
                                                //    competitiveMessaging = competitiveMessagingList[0];
                                                //}
                                                //else
                                                //{
                                                    competitiveMessaging = new Positioning();
                                                //}
                                            }
                                            competitiveMessaging.VirtualAction = "Create";
                                        }
                                        if (!string.IsNullOrEmpty(result))
                                        {
                                            result += "[T+K]";
                                        }
                                        result += "HowTheyAttack_" + ids[1] + ":" + competitiveMessaging.HowTheyAttack + "[T+K]HowWeDefend_" + ids[1] + ":" + competitiveMessaging.HowWeDefend + "[T+K]CompetitiveMessagingId_" + ids[1] + ":" + competitiveMessaging.Id + "[T+K]CompetitiveMessagingProductId_" + ids[1] + ":" + sProductId + "[T+K]CompetitiveMessagingAction_" + ids[1] + ":" + competitiveMessaging.VirtualAction + "[T+K]CompetitiveMessagingStatement_" + ids[1] + ":" + competitiveMessaging.Name;
                                    }
                                }
                            }
                        }
                        i = products.Count;
                    }
                }
            }
            return Content(result);
        }

		public IList<PositioningDTO> GetPositioning(IList<Product> Products, IList<Product> competitorProductList, string CurrentCompany) //Update Competitor and Product
        {
            IList<PositioningDTO> PositioningList = new List<PositioningDTO>();
            if (Products != null)
            {
                foreach(Product oProduct in Products)
                {
                    PositioningDTO tmpPositioning = new PositioningDTO();
                    tmpPositioning.Product = oProduct;

                    Positioning positioning = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Product, oProduct.Id, (decimal)oProduct.Competitor.Industry.Id, CurrentCompany);
                    if (positioning.Id == 0 || string.IsNullOrEmpty(positioning.CreatedBy))
                    {
                        positioning = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Competitor, oProduct.Competitor.Id, (decimal)oProduct.Competitor.Industry.Id, CurrentCompany);
                    }
                    if (positioning != null)
                    {
                        tmpPositioning.HowWePosition = positioning.HowWePosition;
                    }
                    if (competitorProductList.Count > 0)
                    {
                        foreach (Product Competitor in competitorProductList)
                        {
                            decimal competitorId = Convert.ToDecimal(Competitor.CompetitorId);
                            IList<Positioning> competitiveMessaging = PositioningService.GetPositioningListByProductOnComparinator(PositioningRelation.CompetitiveMessaging, DomainObjectType.Product, oProduct.Id, (decimal)oProduct.Competitor.Industry.Id, competitorId, CurrentCompany);
                            if (competitiveMessaging == null || competitiveMessaging.Count == 0)
                            {
                                competitiveMessaging = PositioningService.GetPositioningListByProductOnComparinator(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, oProduct.Competitor.Id, (decimal)oProduct.Competitor.Industry.Id, competitorId, CurrentCompany);
                            }
                            if (competitiveMessaging != null && competitiveMessaging.Count > 0)
                            {
                                CompProdPositioningDTO tmpCPP = new CompProdPositioningDTO();
                                tmpCPP.CompetitorProduct = Competitor;
                                tmpCPP.HowTheyAttack = competitiveMessaging[0].HowTheyAttack;
                                tmpCPP.HowToDefend = competitiveMessaging[0].HowWeDefend;
                                tmpPositioning.CompProdPositioningList.Add(tmpCPP);
                            }
                        }
                    }
                    PositioningList.Add(tmpPositioning);
                }
            }
            return PositioningList;
        }

        public IList<PositioningDTO> GetPositioningToSalesForece(IList<Product> Products, IList<Product> competitorProductList, string companyid) //Update Competitor and Product
        {
            IList<PositioningDTO> PositioningList = new List<PositioningDTO>();
            if (Products != null)
            {
                foreach (Product oProduct in Products)
                {
                    PositioningDTO tmpPositioning = new PositioningDTO();
                    tmpPositioning.Product = oProduct;

                    Positioning positioning = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Product, oProduct.Id, (decimal)oProduct.Competitor.Industry.Id, companyid);
                    if (positioning.Id == 0 || string.IsNullOrEmpty(positioning.CreatedBy))
                    {
                        positioning = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Competitor, oProduct.Competitor.Id, (decimal)oProduct.Competitor.Industry.Id, companyid);
                    }
                    if (positioning != null)
                    {
                        tmpPositioning.HowWePosition = StringUtility.ConvertHtmlToTextPlain(positioning.HowWePosition);
                    }
                    if (competitorProductList.Count > 0)
                    {
                        foreach (Product Competitor in competitorProductList)
                        {
                            decimal competitorId = Convert.ToDecimal(Competitor.CompetitorId);
                            IList<Positioning> competitiveMessaging = PositioningService.GetPositioningListByProductOnComparinator(PositioningRelation.CompetitiveMessaging, DomainObjectType.Product, oProduct.Id, (decimal)oProduct.Competitor.Industry.Id, competitorId, companyid);
                            if (competitiveMessaging == null || competitiveMessaging.Count == 0)
                            {
                                competitiveMessaging = PositioningService.GetPositioningListByProductOnComparinator(PositioningRelation.CompetitiveMessaging, DomainObjectType.Competitor, oProduct.Competitor.Id, (decimal)oProduct.Competitor.Industry.Id, competitorId, companyid);
                            }
                            if (competitiveMessaging != null && competitiveMessaging.Count > 0)
                            {
                                CompProdPositioningDTO tmpCPP = new CompProdPositioningDTO();
                                tmpCPP.CompetitorProduct = Competitor;
                                tmpCPP.HowTheyAttack = StringUtility.ConvertHtmlToTextPlain(competitiveMessaging[0].HowTheyAttack);
                                tmpCPP.HowToDefend = StringUtility.ConvertHtmlToTextPlain(competitiveMessaging[0].HowWeDefend);
                                tmpPositioning.CompProdPositioningList.Add(tmpCPP);
                            }

                        }
                    }
                    PositioningList.Add(tmpPositioning);

                }
            }
            return PositioningList;
        }



        public JsonResult GetImageUrl()
        {
           
            string sProductId = Request.Params["ProductId"];
            decimal id = Convert.ToDecimal(sProductId);
            string tempo=string.Empty;
            Product prod = null;
            IList<Product> products = (List<Product>)Session["Products"];
             for (int i = 0; i < products.Count; i++)
             {
                 if (products[i].Id == id)
                 {
                     prod = products[i];
                     i = products.Count;
                 }
             }
            if(prod==null) prod = ProductService.GetById(id);//in the case no get of List of Session
            string path = prod.ImageUrl;
            prod.HasComment = getComments(prod.Id, DomainObjectType.Product);
            var result = new JsonResult();
            if (string.IsNullOrEmpty(path))
            {
                path = "/Content/Images/Icons/none.png";

            }
            else
            {
                string Server = ApplicationConfigurationManager.GetInstance().GetConfiguration("URL", "URL_SERVER");
                if (!Server.Equals("local"))
                {
                    string patch2 = "/" + Server + "/";
                    path = path.Replace("./", patch2);
                }
            }
            result.Data = new { Path = path, HasComment = prod.HasComment ? "ImageCommentsY" : "ImageCommentsN" };
            return result;
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetExelFile()
        {
            string fileNameResult = string.Empty;

            System.Web.Configuration.HttpRuntimeSection runTime = (System.Web.Configuration.HttpRuntimeSection)System.Web.Configuration.WebConfigurationManager.GetSection("system.web/httpRuntime");
            int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;//Approx 100 Kb(for page content) size
            if ((Request.ContentLength > maxRequestLength))
            {
                return Content("Fail");
            }

            string hpf2 = (string)Request["FileUpload"];
            string headerType = (string)Request["HeaderType"];
            string tempo = string.Empty;
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                Compelligence.Domain.Entity.File newFile = new Compelligence.Domain.Entity.File();
                if (hpf.ContentLength == 0)
                {
                    continue;
                }
                newFile.FileName = System.IO.Path.GetFileName(hpf.FileName);
                newFile.FileFormat = newFile.FileName.Substring(newFile.FileName.LastIndexOf('.') + 1);
                SetDefaultDataForSave(newFile);
                FileService.Save(newFile);
                fileNameResult = UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                if (hpf.ContentLength == 0) continue;
                fileNameResult += "_" + System.IO.Path.GetFileName(hpf.FileName).Replace(' ', '_');
                hpf.SaveAs(System.IO.Path.Combine(ContextFilePath + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"], fileNameResult));
                tempo = newFile.FileName;
            }
            string filestreamv = System.IO.Path.Combine(ContextFilePath + System.Configuration.ConfigurationSettings.AppSettings["ComparinatorPath"], fileNameResult);
            int leng = filestreamv.Length;
            Byte[] buffer = new Byte[filestreamv.Length];
            FileStream tyem = new FileStream(filestreamv.ToString(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            tyem.Read(buffer, 0, leng);
            try
            {
                tyem.Read(buffer, 0, leng);
            }
            catch (Exception exc)
            {
                tyem.Dispose();
            }
            try
            {
                NPOI.POIFS.FileSystem.POIFSFileSystem fs = new NPOI.POIFS.FileSystem.POIFSFileSystem(tyem);
                HSSFWorkbook hssfworkbook = new HSSFWorkbook(fs, true);
                int pos = 0;
                HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(pos);
                int numberRow = sheet.PhysicalNumberOfRows;
                string industryName = GetValueOfCell(sheet, 0, 1);
                if (!string.IsNullOrEmpty(industryName))
                {
                    int beginRow = 1;
                    int endRow = numberRow;
                    try
                    {
                        HSSFCell CellTempo = (HSSFCell)sheet.GetRow(1).GetCell(0);
                    }
                    catch
                    {
                        beginRow++;
                        endRow++;
                    }
                    int totalColumns = sheet.GetRow(beginRow).LastCellNum;
                    IList<string> nameProductList = new List<string>();
                    IList<ComparinatorCell> cellList = new List<ComparinatorCell>();
                    for (int r = beginRow + 1; r < endRow + 1; r++)
                    {
                        string groupName = string.Empty;
                        string setName = string.Empty;
                        string criteriaName = string.Empty;
                        groupName = GetValueOfCell(sheet, r, 0);
                        setName = GetValueOfCell(sheet, r, 1);
                        criteriaName = GetValueOfCell(sheet, r, 2);
                        for (int c = 3; c < totalColumns; c++)
                        {
                            string value = string.Empty;
                            string productName = string.Empty;
                            productName = GetValueOfCell(sheet, beginRow, c);
                            value = GetValueOfCell(sheet, r, c);
                            if (!string.IsNullOrEmpty(value))
                            {
                                ComparinatorCell comparinatorCell = new ComparinatorCell();
                                comparinatorCell.ValueCell = value;
                                comparinatorCell.Group = groupName;
                                comparinatorCell.Set = setName;
                                comparinatorCell.Criteria = criteriaName;
                                cellList.Add(comparinatorCell);
                            }
                        }
                    }
                    SaveOrUpdateNewValues(cellList);
                }
            }
            catch (Exception e)
            { }
            return RedirectToAction("Index", "ContentPortal");
        }

        private string GetValueOfCell(HSSFSheet sheet, int row, int column)
        {
            string value = string.Empty;
            try
            {
                HSSFCell cell = (HSSFCell)sheet.GetRow(row).GetCell(column);
                if (cell.CellType.ToString().Equals("STRING"))
                {
                    value = cell.StringCellValue.Trim();
                }
                else if (cell.CellType.ToString().Equals("NUMERIC"))
                {
                    value = cell.NumericCellValue.ToString().Trim();
                }
                else
                { }
            }
            catch
            {

            }
            return value;
        }

        public void SaveOrUpdateNewValues(IList<ComparinatorCell> cellList)
        {
            if (cellList != null && cellList.Count > 0)
            {
                foreach (ComparinatorCell cell in cellList)
                {
                    CriteriaGroup criteriaGroup = CriteriaGroupService.GetFirstCriteriaGroupByName(cell.Group, (decimal)cell.IndustryId, CurrentCompany);
                        if (criteriaGroup == null)
                        {
                            criteriaGroup = new CriteriaGroup();
                            criteriaGroup.Name = cell.Group;
                            criteriaGroup.CreatedBy = CurrentUser;
                            criteriaGroup.IndustryId = cell.IndustryId;
                            criteriaGroup.CreatedDate = DateTime.Now;
                            criteriaGroup.LastChangedBy = CurrentUser;
                            criteriaGroup.LastChangedDate = DateTime.Now;
                            criteriaGroup.ClientCompany = CurrentCompany;
                            CriteriaGroupService.Save(criteriaGroup);
                        }

                        if (criteriaGroup != null)
                        {
                            //CriteriaSet criteriaSet = CriteriaSetService.GetFirstResultByName(cell.Set, industry.Id, criteriaGroup.Id, CurrentCompany);
                            CriteriaSet criteriaSet = CriteriaSetService.GetFirstCriteriaSetByName(cell.Set, CurrentCompany, criteriaGroup.Id);
                            if (criteriaSet == null)
                            {
                                criteriaSet = new CriteriaSet();
                                criteriaSet.Name = cell.Set;
                                criteriaSet.CreatedBy = CurrentUser;
                                criteriaSet.IndustryId = cell.IndustryId;
                                criteriaSet.CreatedDate = DateTime.Now;
                                criteriaSet.LastChangedBy = CurrentUser;
                                criteriaSet.LastChangedDate = DateTime.Now;
                                criteriaSet.ClientCompany = CurrentCompany;
                                criteriaSet.CriteriaGroupId = criteriaGroup.Id;
                                CriteriaSetService.Save(criteriaSet);
                            }

                            if (criteriaSet != null)
                            {
                                //Criteria criteria = CriteriaService.GetFirstResultByName(cell.Criteria, industry.Id, criteriaSet.Id, criteriaGroup.Id , CurrentCompany);
                                Criteria criteria = CriteriaService.GetFirstCriteriaByName(cell.Criteria, CurrentCompany);
                                if (criteria == null)
                                {
                                    criteria = new Criteria();
                                    criteria.Name = cell.Criteria;
                                    criteria.CreatedBy = CurrentUser;
                                    criteria.IndustryId = cell.IndustryId;
                                    criteria.CreatedDate = DateTime.Now;
                                    criteria.LastChangedBy = CurrentUser;
                                    criteria.LastChangedDate = DateTime.Now;
                                    criteria.ClientCompany = CurrentCompany;
                                    criteria.CriteriaSetId = criteriaSet.Id;
                                    criteria.CriteriaGroupId = criteriaGroup.Id;
                                    CriteriaService.Save(criteria);
                                }
                                
                                if (criteria != null)
                                {
                                    IndustryCriteriaId industryCriteriaId = new IndustryCriteriaId((decimal)cell.IndustryId, criteria.Id);
                                    IndustryCriteria industryCriteria = IndustryCriteriaService.GetById(industryCriteriaId);
                                    if (industryCriteria == null)
                                    {
                                        industryCriteria = new IndustryCriteria(industryCriteriaId);
                                        industryCriteria.CriteriaId = criteria.Id;
                                        
                                        industryCriteria.CreatedBy = CurrentUser;
                                        industryCriteria.IndustryId = (decimal)cell.IndustryId;
                                        industryCriteria.CreatedDate = DateTime.Now;
                                        industryCriteria.LastChangedBy = CurrentUser;
                                        industryCriteria.LastChangedDate = DateTime.Now;
                                        industryCriteria.ClientCompany = CurrentCompany;
                                        IndustryCriteriaService.Save(industryCriteria);
                                    }
                                    //IndustryCriteriasId industryCriteriasId = new IndustryCriteriasId(industry.Id, criteria.Id);
                                    //IndustryCriterias industryCriterias = IndustryCriteriasService.GetById(industryCriteriasId);
                                    //if (industryCriterias == null)
                                    //{
                                    //    industryCriterias = new IndustryCriterias(industryCriteriasId);
                                    //    industryCriterias.CriteriaGroupId = criteriaGroup.Id;
                                    //    industryCriterias.GroupName = criteriaGroup.Name;
                                    //    industryCriterias.CriteriaSetId = criteriaSet.Id;
                                    //    industryCriterias.SetName = criteriaSet.Name;
                                    //    industryCriterias.CriteriaName = criteria.Name;
                                    //    industryCriterias.CreatedBy = CurrentUser;
                                    //    industryCriterias.CreatedDate = DateTime.Now;
                                    //    industryCriterias.LastChangedBy = CurrentUser;
                                    //    industryCriterias.LastChangedDate = DateTime.Now;
                                    //    industryCriterias.ClientCompany = CurrentCompany;
                                    //    IndustryCriteriasService.Save(industryCriterias);
                                    //}

                                    ProductCriteriaId id = new ProductCriteriaId((decimal)cell.ProductId, criteria.Id, (decimal)cell.IndustryId);
                                        ProductCriteria productcriteria = ProductCriteriaService.GetById(id);
                                        if (productcriteria == null)
                                        {
                                            productcriteria = ProductCriteriaService.GetNew((decimal)cell.IndustryId, (decimal)cell.ProductId, criteria.Id);
                                            SetDefaultDataForSave(productcriteria);
                                            productcriteria.Value = cell.ValueCell;
                                            productcriteria.CriteriaUploader = false;
                                            ProductCriteriaService.Save(productcriteria);
                                        }
                                        else
                                        {
                                            productcriteria.Value = cell.ValueCell;
                                            productcriteria.CriteriaUploader = false;
                                            SetDefaultDataForUpdate(productcriteria);
                                            ProductCriteriaService.Update(productcriteria);
                                        }
                                }
                            }
                        }

                }
            }
        }

        private bool HaveProductOfClientCompetitor(IList<Product> productList)
        {
            bool result = false;
            for (int i = 0; i < productList.Count; i++ )
            {
                if (productList[i].IsClientCompetitor.Equals("Y"))
                {
                    result = true;
                    i = productList.Count;
                }
            }
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackComment(decimal id, FormCollection form)
        {
            string salesuserid = GetDecodeParam("U");
            string salescompanyid = GetDecodeParam("C");
            string localuserid = string.IsNullOrEmpty(salesuserid) ? CurrentUser : salesuserid;
            string localcompanyid = string.IsNullOrEmpty(salesuserid) ? CurrentCompany : salescompanyid;

            ForumResponse forumResponse = new ForumResponse();
            forumResponse.EntityId=id;
            forumResponse.EntityType = DomainObjectType.Product;
            forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
            SetDefaultDataForSave(forumResponse, localcompanyid, localuserid);
            ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);

            ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Product, ActionFrom.FrontEnd, localuserid, localcompanyid);

            return null;
        }

        //get Forum and forumresponses of a product
        public bool getComments(decimal EntityId, string ObjectType)//, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            bool answer = false;
            Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
            if (forum != null)
            {
                IList<ForumResponse> forumresponses = null;
                forumresponses = forum == null ? new List<ForumResponse>() : ForumResponseService.GetByForumId(forum.Id, ObjectType);
                if (forumresponses.Count > 0)
                {
                    answer = true;
                }
            }

            return answer;
        }
        public bool getSilverCommentComments(decimal industryId, decimal productId)//, decimal? IndustryId, decimal? CriteriaId, decimal? EntityIdT)
        {
            bool answer = false;
            //Forum forum = ForumService.GetByEntityId(EntityId, ObjectType, ForumType.Comment);
            //if (forum != null)
            //{
                
            IList<ForumResponse> forumresponses = null;
            forumresponses = ForumResponseService.GetByEntityResponseAndTypeNoChild(industryId,productId,ForumType.Comment);
                if (forumresponses.Count > 0)
                {
                    answer = true;
                }
            //}
            return answer;
        }
        public Positioning SetPostioningToProduct(string postioningType, decimal productId, decimal competitorId, decimal industryId, string companyId)
        {
            Positioning positioning = new Positioning();
            positioning = PositioningService.GetPositiongByProductAndParent(PositioningRelation.Positioning, DomainObjectType.Product, productId, competitorId, industryId, companyId);
            if (positioning != null)
            {
                PositioningIndustryId piId = new PositioningIndustryId(positioning.Id, industryId);
                PositioningIndustry piT = PositioningIndustryService.GetById(piId);
                if (positioning.EntityId == productId && piT != null && !positioning.IsGlobal.Equals("Y"))
                    positioning.VirtualAction = "Update"; //The positioning is to product with the current industry
                else
                    positioning.VirtualAction = "Create"; // the positioning no is product, and can be to industry parent
            }
            else
            {
                positioning = PositioningService.GetPositiongAndParent(PositioningRelation.Positioning, DomainObjectType.Competitor, competitorId, industryId, companyId);
                if (positioning == null)
                {
                    positioning = new Positioning();
                }
                positioning.VirtualAction = "Create";
            }
            positioning.VirtualProductId = productId;
            positioning.VirtualCompetitorId = competitorId;
            positioning.VirtualIndustryId = industryId;
            return positioning;
        }
        public void AddProductGeneric(FormCollection form, string clientCompanyId)
        {
            SetLabels();
            string IndustryId = form["IndustryId"];
            string CompetitorId = form["CompetitorId"];
            string[] selectedProductId = { };
            if (!String.IsNullOrEmpty(form["multiselect_ProductId"]))
                selectedProductId = form["multiselect_ProductId"].Split(',');
            IList<Product> products = (List<Product>)Session["Products"];
            if (products == null)
                products = new List<Product>();

            foreach (string ProductId in selectedProductId)
            {

                if (ProductId.Length > 0)
                {
                    GetEntitiesToAddProduct(IndustryId, CompetitorId, ProductId, products, clientCompanyId);
                }
            }
            SetValuesToAddProduct(IndustryId, CompetitorId, products, clientCompanyId);
        }

        [AuthenticationFilter]
        public ActionResult AddProduct(FormCollection form)
        {
            AddProductGeneric(form, CurrentCompany);
            return View("Elements");
        }

        public ActionResult AddSalesForceProduct(FormCollection form)
        {
            string salesuserid = GetDecodeParam(form,"U");
            string salescompanyid = GetDecodeParam(form, "C");

            AddProductGeneric(form, salescompanyid);
            SetLabelsToForm(salescompanyid, salesuserid);

            String DefaultsSocialLog = "false";
            IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(salescompanyid);
            if (configurations != null && configurations.Count > 0)
            {
                DefaultsSocialLog = configurations[0].SocialLog;
            }

            ViewData["DefaultsSocialLog"] = DefaultsSocialLog;
            ViewData["C"] = form["C"]; //probably is not necessary
            return View("Elements");
        }

        public void SetLabelsToForm(string clientCompanyId, string userId)
        {
            //Labels
            ConfigurationLabels ProductLabel = new ConfigurationLabels();
            ConfigurationLabels IndustryLabel = new ConfigurationLabels();
            ConfigurationLabels CompetitorLabel = new ConfigurationLabels();
            ProductLabel = ConfigurationLabelsService.GetByName("Product", clientCompanyId);
            IndustryLabel = ConfigurationLabelsService.GetByName("Industry", clientCompanyId);
            CompetitorLabel = ConfigurationLabelsService.GetByName("Competitor", clientCompanyId);
            //defaults
            String dProductLabel = "Product";
            String dIndustryLabel = "Industry";
            String dCompetitorLabel = "Competitor";

            if (IndustryLabel != null)
            {
                if (IndustryLabel.Status.Equals(ConfigurationLabesStatus.Enable))
                    dIndustryLabel = IndustryLabel.Value;
            }
            else
            {
                IndustryLabel = new ConfigurationLabels();
                IndustryLabel.Name = "Industry";
                IndustryLabel.Value = dIndustryLabel;
                IndustryLabel.Description = "Label for " + dIndustryLabel;
                IndustryLabel.Status = ConfigurationLabesStatus.Enable;
                
                SetDefaultDataForSave(IndustryLabel,clientCompanyId,userId);
                ConfigurationLabelsService.Save(IndustryLabel);
            }

            if (CompetitorLabel != null)
            {
                if (CompetitorLabel.Status.Equals(ConfigurationLabesStatus.Enable))
                    dCompetitorLabel = CompetitorLabel.Value;
            }
            else
            {
                CompetitorLabel = new ConfigurationLabels();
                CompetitorLabel.Name = "Competitor";
                CompetitorLabel.Value = dCompetitorLabel;
                CompetitorLabel.Description = "Label for " + dCompetitorLabel;
                CompetitorLabel.Status = ConfigurationLabesStatus.Enable;
                SetDefaultDataForSave(CompetitorLabel,clientCompanyId,userId);
                ConfigurationLabelsService.Save(CompetitorLabel);
            }

            if (ProductLabel != null)
            {
                if (ProductLabel.Status.Equals(ConfigurationLabesStatus.Enable))
                    dProductLabel = ProductLabel.Value;
            }
            else
            {
                ProductLabel = new ConfigurationLabels();
                ProductLabel.Name = "Product";
                ProductLabel.Value = dProductLabel;
                ProductLabel.Description = "Label for " + dProductLabel;
                ProductLabel.Status = ConfigurationLabesStatus.Enable;
                SetDefaultDataForSave(ProductLabel, clientCompanyId, userId);
                ConfigurationLabelsService.Save(ProductLabel);
            }

            ViewData["IndustryLabel"] = dIndustryLabel;
            ViewData["CompetitorLabel"] = dCompetitorLabel;
            ViewData["ProductLabel"] = dProductLabel;
            ViewData["SelectIndustryLabel"] = SetLabelToShowByFirstCharacter(dIndustryLabel);
            ViewData["SelectCompetitorLabel"] = SetLabelToShowByFirstCharacter(dCompetitorLabel);
            ViewData["SelectProductLabel"] = SetLabelToShowByFirstCharacter(dProductLabel);
        }

        public string SetLabelToShowByFirstCharacter(string value)
        {
            string result = string.Empty;
            value = value.ToLower();
            if (value.IndexOf('a') == 0 || value.IndexOf('e') == 0 || value.IndexOf('i') == 0 || value.IndexOf('o') == 0 || value.IndexOf('u') == 0 )
            {
                result = string.Format(LabelResource.ComparinatorSelectFieldLabelAn, value);
            }
            else
            {
                result = string.Format(LabelResource.ComparinatorSelectFieldLabelA, value);
            }
            return result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SendFeedBackSalesToolsForProduct(decimal id, FormCollection form)
        {
            ForumResponse forumResponse = new ForumResponse();
            string salescompanyid = GetDecodeParam(form,"C");
            string salesuserid = GetDecodeParam(form, "U");

            if (!string.IsNullOrEmpty(salescompanyid) && !string.IsNullOrEmpty(salesuserid))
            {
                forumResponse.EntityId = id;
                forumResponse.EntityType = DomainObjectType.Product;
                forumResponse.Response = StringUtility.CheckNull(form["Comment"]);
                SetDefaultDataForSave(forumResponse, salescompanyid, salesuserid);
                ForumService.SaveForumResponse(forumResponse, ForumType.FeedBack);
                ActionHistoryService.RecordActionHistory(id, EntityAction.FeedBack, DomainObjectType.Product, ActionFrom.FrontEnd, salesuserid, salescompanyid);
            }
            return null;
        }

        public ActionResult CreatePositioningOnComparinator()
        {
            string sIndustryId = Request.Params["hdnIndustryId"];
            string sCompetitorId = Request.Params["hdnCompetitorId"];
            string sProductId = Request.Params["hdnProductId"];
            string sPositioningId = Request.Params["hdnPositioningId"];
            string sPositioningRelation = Request.Params["hdnPositioningRelation"];
            string sStatment = Request.Params["txtStatment"];
            //For Positioning - Client 
            string sHowWePosition = Request.Params["TxtHowWePosition"];
            //For Positioning 
            string sHowTheyPosition = Request.Params["TxtHowTheyPosition"];
            string sHowWeAttack = Request.Params["TxtHowWeAttack"];
            //For Competitive Messaging
            string sHowTheyAttack = Request.Params["TxtHowTheyAttack"];
            string sHowToDefend = Request.Params["TxtHowToDefend"];
            //Fields to SalesForce
            string salesclientcompanyencode = Request.Params["hdnC"];
            string salesuseridencode = Request.Params["hdnU"];
            //Is client company
            string sIsCompany = Request.Params["hcnIsCompany"];
            //
            string clientCOmpanyId = CurrentCompany;
            string userId = CurrentUser;
            if (!string.IsNullOrEmpty(salesclientcompanyencode) && !string.IsNullOrEmpty(salesuseridencode))
            {
                userId = Encryptor.Decode(salesuseridencode);
                clientCOmpanyId = Encryptor.Decode(salesclientcompanyencode);
            }
            Positioning positioning = new Positioning();
            positioning.Name = StringUtility.CheckWhiteSpaces(sStatment);
            positioning.HowWePosition = StringUtility.CheckWhiteSpaces(sHowWePosition);
            positioning.HowWePosition = HttpUtility.HtmlDecode(positioning.HowWePosition);
            positioning.HowTheyPosition = StringUtility.CheckWhiteSpaces(sHowTheyPosition);
            positioning.HowTheyPosition = HttpUtility.HtmlDecode(positioning.HowTheyPosition);
            positioning.HowWeAttack = StringUtility.CheckWhiteSpaces(sHowWeAttack);
            positioning.HowWeAttack = HttpUtility.HtmlDecode(positioning.HowWeAttack);
            positioning.HowTheyAttack = StringUtility.CheckWhiteSpaces(sHowTheyAttack);
            positioning.HowTheyAttack = HttpUtility.HtmlDecode(positioning.HowTheyAttack);
            positioning.HowWeDefend = StringUtility.CheckWhiteSpaces(sHowToDefend);
            positioning.HowWeDefend = HttpUtility.HtmlDecode(positioning.HowWeDefend);
            positioning.PositioningRelation = sPositioningRelation;
            positioning.EntityId = decimal.Parse(sProductId);
            positioning.EntityType = DomainObjectType.Product;
            positioning.IndustriesIds = sIndustryId;
            positioning.CompetitorsIds = sCompetitorId;
            positioning.Status = PositioningStatus.Enabled;
            if (!string.IsNullOrEmpty(sStatment))
            {
                if (sStatment.Equals(PositioningRelation.CompetitiveMessaging))
                {
                    positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                }
            }
            else if (sStatment.Equals(PositioningRelation.Positioning))
            {
                if (!string.IsNullOrEmpty(sIsCompany))
                {
                    if (sIsCompany.Equals(PositioningIsCompetitorCompany.Yes))
                    {
                        positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                    }
                    else 
                    {
                        positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.No;
                    }
                }
            }
            else
            {}
            positioning.CreatedBy = userId;//Change to salesforce
            positioning.CreatedDate = DateTime.Now;
            positioning.LastChangedBy = userId;//change to salesforce
            positioning.LastChangedDate = DateTime.Now;
            positioning.ClientCompany = clientCOmpanyId;//change to salesforce
            PositioningService.Save(positioning);
            return Content(positioning.Id.ToString());
        }

        public ActionResult UpdatePositioningOnComparinator()
        {
            string sIndustryId = Request.Params["hdnIndustryId"];
            string sCompetitorId = Request.Params["hdnCompetitorId"];
            string sProductId = Request.Params["hdnProductId"];
            string sPositioningId = Request.Params["hdnPositioningId"];
            string sPositioningRelation = Request.Params["hdnPositioningRelation"];
            string sStatment = Request.Params["txtStatment"];
            //For Positioning - Client
            string sHowWePosition = Request.Params["TxtHowWePosition"];
            //For Positioning
            string sHowTheyPosition = Request.Params["TxtHowTheyPosition"];
            string sHowWeAttack = Request.Params["TxtHowWeAttack"];
            //For Competitive Messaging
            string sHowTheyAttack = Request.Params["TxtHowTheyAttack"];
            string sHowToDefend = Request.Params["TxtHowToDefend"];
            //Fields to SalesForce
            string salesclientcompanyencode = Request.Params["hdnC"];
            string salesuseridencode = Request.Params["hdnU"];
            //Is client company
            string sIsCompany = Request.Params["hcnIsCompany"];
            //
            string clientCOmpanyId = CurrentCompany;
            string userId = CurrentUser;
            if (string.IsNullOrEmpty(CurrentCompany) && string.IsNullOrEmpty(CurrentUser) && !string.IsNullOrEmpty(salesclientcompanyencode) && !string.IsNullOrEmpty(salesuseridencode))
            {
                userId = Encryptor.Decode(salesuseridencode);
                clientCOmpanyId = Encryptor.Decode(salesclientcompanyencode);
            }
            decimal productId = decimal.Parse(sProductId);
            decimal industryId = decimal.Parse(sIndustryId);
            decimal id = decimal.Parse(sPositioningId);
            //Positioning positioning = PositioningService.GetById(id);
            Positioning positioning = PositioningService.GetByEntityAndIndustry(sPositioningRelation, DomainObjectType.Product, productId, (decimal)industryId, clientCOmpanyId);
            string action = "Update";
            if (positioning == null)
            {
                positioning = new Positioning();
                action = "Create";
            }
            if (positioning != null)
            {
                positioning.Name = StringUtility.CheckWhiteSpaces(sStatment);
                if (!string.IsNullOrEmpty(sStatment))
                {
                    if (sStatment.Equals(PositioningRelation.CompetitiveMessaging))
                    {
                        positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                    }
                }
                else if (sStatment.Equals(PositioningRelation.Positioning))
                {
                    if (!string.IsNullOrEmpty(sIsCompany))
                    {
                        if (sIsCompany.Equals(PositioningIsCompetitorCompany.Yes))
                        {
                            positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                        }
                        else
                        {
                            positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.No;
                        }
                    }
                }
                else
                { }
                positioning.Status = PositioningStatus.Enabled;
                positioning.HowWePosition = StringUtility.CheckWhiteSpaces(sHowWePosition);
                positioning.HowWePosition = HttpUtility.HtmlDecode(positioning.HowWePosition);
                positioning.HowTheyPosition = StringUtility.CheckWhiteSpaces(sHowTheyPosition);
                positioning.HowTheyPosition = HttpUtility.HtmlDecode(positioning.HowTheyPosition);
                positioning.HowWeAttack = StringUtility.CheckWhiteSpaces(sHowWeAttack);
                positioning.HowWeAttack = HttpUtility.HtmlDecode(positioning.HowWeAttack);
                positioning.HowTheyAttack = StringUtility.CheckWhiteSpaces(sHowTheyAttack);
                positioning.HowTheyAttack = HttpUtility.HtmlDecode(positioning.HowTheyAttack);
                positioning.HowWeDefend = StringUtility.CheckWhiteSpaces(sHowToDefend);
                positioning.HowWeDefend = HttpUtility.HtmlDecode(positioning.HowWeDefend);
                positioning.LastChangedBy = userId;//change to salesforce
                positioning.LastChangedDate = DateTime.Now;
                if (action.Equals("Update"))
                {
                    PositioningService.Update(positioning);
                }
                else
                {
                    positioning.PositioningRelation = sPositioningRelation;
                    positioning.EntityId = decimal.Parse(sProductId);
                    positioning.EntityType = DomainObjectType.Product;
                    positioning.IndustriesIds = sIndustryId;
                    positioning.CompetitorsIds = sCompetitorId;
                    positioning.Status = PositioningStatus.Enabled;
                    positioning.CreatedBy = userId;//change to salesforce
                    positioning.CreatedDate = DateTime.Now;
                    PositioningService.Save(positioning);
                }
            }
            return Content(positioning.Id.ToString());
        }

        public ContentResult GetPositioningById() //Update Competitor and Product
        {
            string sPrositioningId = StringUtility.CheckNull(Request.Params["PositioningId"]);
            string result=string.Empty;
            decimal id = decimal.Parse(sPrositioningId);
            Positioning positioning = PositioningService.GetById(id);
            if (positioning != null)
            {
                result = "PositioningStatment_" + sPrositioningId + "." + positioning.Name + "[TK" + sPrositioningId + "]";
                result += "PositioningHowWePosition_" + sPrositioningId + "." + positioning.HowWePosition + "[TK" + sPrositioningId + "]";
                result += "PositioningHowTheyAttack_" + sPrositioningId + "." + positioning.HowTheyAttack + "[TK" + sPrositioningId + "]";
                result += "PositioningHowToDefend_" + sPrositioningId + "." + positioning.HowWeDefend + "[TK" + sPrositioningId + "]";
                result += "PositioningHowTheyPosition_" + sPrositioningId + "." + positioning.HowTheyPosition + "[TK" + sPrositioningId + "]";
                result += "PositioningHowWeAttack_" + sPrositioningId + "." + positioning.HowWeAttack + "[TK" + sPrositioningId + "]";
            }
            return Content(result);
        }

        public ActionResult CreatePositioning()
        {
            string sIndustryId = Request.Params["hdnIndustryId"];
            string sEntityId = Request.Params["hdnEntityId"];
            string sEntityType = Request.Params["hdnEntityType"];
            string sPositioningId = Request.Params["hdnPositioningId"];
            string sPositioningRelation = Request.Params["hdnPositioningRelation"];
            string sStatment = Request.Params["txtStatment"];
            //For Positioning - Client 
            string sHowWePosition = Request.Params["TxtHowWePosition"];
            //For Positioning 
            string sHowTheyPosition = Request.Params["TxtHowTheyPosition"];
            string sHowWeAttack = Request.Params["TxtHowWeAttack"];
            //For Competitive Messaging
            string sHowTheyAttack = Request.Params["TxtHowTheyAttack"];
            string sHowToDefend = Request.Params["TxtHowToDefend"];
            //Fields to SalesForce
            string salesclientcompanyencode = Request.Params["hdnC"];
            string salesuseridencode = Request.Params["hdnU"];
            //Is client company
            string sIsCompany = Request.Params["hcnIsCompany"];
            //
            string clientCOmpanyId = CurrentCompany;
            string userId = CurrentUser;
            if (string.IsNullOrEmpty(CurrentUser) && string.IsNullOrEmpty(CurrentCompany) && !string.IsNullOrEmpty(salesclientcompanyencode) && !string.IsNullOrEmpty(salesuseridencode))
            {
                userId = Encryptor.Decode(salesuseridencode);
                clientCOmpanyId = Encryptor.Decode(salesclientcompanyencode);
            }
            Positioning positioning = new Positioning();
            positioning.Name = StringUtility.CheckWhiteSpaces(sStatment);
            positioning.HowWePosition = StringUtility.CheckWhiteSpaces(sHowWePosition);
            positioning.HowWePosition = HttpUtility.HtmlDecode(positioning.HowWePosition);
            positioning.HowTheyPosition = StringUtility.CheckWhiteSpaces(sHowTheyPosition);
            positioning.HowTheyPosition = HttpUtility.HtmlDecode(positioning.HowTheyPosition);
            positioning.HowWeAttack = StringUtility.CheckWhiteSpaces(sHowWeAttack);
            positioning.HowWeAttack = HttpUtility.HtmlDecode(positioning.HowWeAttack);
            positioning.HowTheyAttack = StringUtility.CheckWhiteSpaces(sHowTheyAttack);
            positioning.HowTheyAttack = HttpUtility.HtmlDecode(positioning.HowTheyAttack);
            positioning.HowWeDefend = StringUtility.CheckWhiteSpaces(sHowToDefend);
            positioning.HowWeDefend = HttpUtility.HtmlDecode(positioning.HowWeDefend);
            positioning.PositioningRelation = sPositioningRelation;
            positioning.EntityId = decimal.Parse(sEntityId);
            positioning.EntityType = sEntityType;
            positioning.IndustriesIds = sIndustryId;
            //positioning.CompetitorsIds = sCompetitorId;
            /// NO EXIST LOGIC TO COMPETITIVE MESSAGIN TO PRODUCT LEVEL, TO FUTURE
            if (positioning.PositioningRelation.Equals(PositioningRelation.CompetitiveMessaging) && sEntityType.Equals(DomainObjectType.Competitor))
            {
                Competitor competitor = CompetitorService.GetCompetitorClient(clientCOmpanyId);
                if (competitor != null)
                {
                    positioning.EntityId = competitor.Id;
                    positioning.CompetitorsIds = sEntityId;
                }
            }
            
            positioning.Status = PositioningStatus.Enabled;
            if (!string.IsNullOrEmpty(sStatment))
            {
                if (sStatment.Equals(PositioningRelation.CompetitiveMessaging))
                {
                    positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                }
            }
            else if (sStatment.Equals(PositioningRelation.Positioning))
            {
                if (!string.IsNullOrEmpty(sIsCompany))
                {
                    if (sIsCompany.Equals(PositioningIsCompetitorCompany.Yes))
                    {
                        positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                    }
                    else
                    {
                        positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.No;
                    }
                }
            }
            else
            { }
            positioning.CreatedBy = userId;//Change to salesforce
            positioning.CreatedDate = DateTime.Now;
            positioning.LastChangedBy = userId;//change to salesforce
            positioning.LastChangedDate = DateTime.Now;
            positioning.ClientCompany = clientCOmpanyId;//change to salesforce
            PositioningService.Save(positioning);
            return Content(positioning.Id.ToString());
        }

        public ActionResult UpdatePositioning()
        {
            string sIndustryId = Request.Params["hdnIndustryId"];
           // string sCompetitorId = Request.Params["hdnCompetitorId"];
            string sEntityId = Request.Params["hdnEntityId"];
            string sEntityType = Request.Params["hdnEntityType"];
            string sPositioningId = Request.Params["hdnPositioningId"];
            string sPositioningRelation = Request.Params["hdnPositioningRelation"];
            string sStatment = Request.Params["txtStatment"];
            //For Positioning - Client
            string sHowWePosition = Request.Params["TxtHowWePosition"];
            //For Positioning
            string sHowTheyPosition = Request.Params["TxtHowTheyPosition"];
            string sHowWeAttack = Request.Params["TxtHowWeAttack"];
            //For Competitive Messaging
            string sHowTheyAttack = Request.Params["TxtHowTheyAttack"];
            string sHowToDefend = Request.Params["TxtHowToDefend"];
            //Fields to SalesForce
            string salesclientcompanyencode = Request.Params["hdnC"];
            string salesuseridencode = Request.Params["hdnU"];
            //Is client company
            string sIsCompany = Request.Params["hcnIsCompany"];
            //
            string clientCOmpanyId = CurrentCompany;
            string userId = CurrentUser;
            if (string.IsNullOrEmpty(CurrentCompany) && string.IsNullOrEmpty(CurrentUser) && !string.IsNullOrEmpty(salesclientcompanyencode) && !string.IsNullOrEmpty(salesuseridencode))
            {
                userId = Encryptor.Decode(salesuseridencode);
                clientCOmpanyId = Encryptor.Decode(salesclientcompanyencode);
            }
            decimal entityId = decimal.Parse(sEntityId);
            decimal industryId = decimal.Parse(sIndustryId);
            decimal id = decimal.Parse(sPositioningId);
            //Positioning positioning = PositioningService.GetById(id);

            //positioning.CompetitorsIds = sCompetitorId;
            /// NO EXIST LOGIC TO COMPETITIVE MESSAGIN TO PRODUCT LEVEL, TO FUTURE
            //if (sPositioningRelation.Equals(PositioningRelation.CompetitiveMessaging) && sEntityType.Equals(DomainObjectType.Competitor))
            //{
            //    Competitor competitor = CompetitorService.GetCompetitorClient(clientCOmpanyId);
            //    if (competitor != null)
            //    {
            //        positioning.EntityId = competitor.Id;
            //        positioning.CompetitorsIds = sEntityId;
            //    }
            //}

            Positioning positioning = PositioningService.GetById(id); ;
            //Positioning positioning = PositioningService.GetByEntityAndIndustry(sPositioningRelation, sEntityType, entityId, (decimal)industryId, clientCOmpanyId);
            string action = "Update";
            if (positioning == null)
            {
                positioning = new Positioning();
                action = "Create";
            }
            if (positioning != null)
            {
                positioning.Name = StringUtility.CheckWhiteSpaces(sStatment);
                if (!string.IsNullOrEmpty(sStatment))
                {
                    if (sStatment.Equals(PositioningRelation.CompetitiveMessaging))
                    {
                        positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                    }
                }
                else if (sStatment.Equals(PositioningRelation.Positioning))
                {
                    if (!string.IsNullOrEmpty(sIsCompany))
                    {
                        if (sIsCompany.Equals(PositioningIsCompetitorCompany.Yes))
                        {
                            positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.Yes;
                        }
                        else
                        {
                            positioning.IsCompetitorCompany = PositioningIsCompetitorCompany.No;
                        }
                    }
                }
                else
                { }
                positioning.Status = PositioningStatus.Enabled;
                positioning.HowWePosition = StringUtility.CheckWhiteSpaces(sHowWePosition);
                positioning.HowWePosition = HttpUtility.HtmlDecode(positioning.HowWePosition);
                positioning.HowTheyPosition = StringUtility.CheckWhiteSpaces(sHowTheyPosition);
                positioning.HowTheyPosition = HttpUtility.HtmlDecode(positioning.HowTheyPosition);
                positioning.HowWeAttack = StringUtility.CheckWhiteSpaces(sHowWeAttack);
                positioning.HowWeAttack = HttpUtility.HtmlDecode(positioning.HowWeAttack);
                positioning.HowTheyAttack = StringUtility.CheckWhiteSpaces(sHowTheyAttack);
                positioning.HowTheyAttack = HttpUtility.HtmlDecode(positioning.HowTheyAttack);
                positioning.HowWeDefend = StringUtility.CheckWhiteSpaces(sHowToDefend);
                positioning.HowWeDefend = HttpUtility.HtmlDecode(positioning.HowWeDefend);
                positioning.LastChangedBy = userId;//change to salesforce
                positioning.LastChangedDate = DateTime.Now;
                if (action.Equals("Update"))
                {
                    PositioningService.Update(positioning);
                }
                else
                {
                    //positioning.PositioningRelation = sPositioningRelation;
                    //positioning.EntityId = decimal.Parse(sEntityId);
                    //positioning.EntityType = DomainObjectType.Product;
                    //positioning.IndustriesIds = sIndustryId;
                    //positioning.CompetitorsIds = sCompetitorId;
                    //positioning.Status = PositioningStatus.Enabled;
                    //positioning.CreatedBy = userId;//change to salesforce
                    //positioning.CreatedDate = DateTime.Now;
                    //PositioningService.Save(positioning);
                }
            }
            return Content(positioning.Id.ToString());
        }

        //[AuthenticationFilter]
        public ActionResult AddProductOfRecommended()
        {
            ViewData["C"] = Request["C"]; //This ensure received/sent is same value.
            SetLabels();
            string industryIdRequest = (string)Request["IndustryId"];
            string competitorIdRequest = (string)Request["CompetitorId"];
            string productIdRequest = (string)Request["ProductId"];

            string salesclientcompanyencode = StringUtility.CheckNull(Request["C"]);
            string localclientcompany = string.IsNullOrEmpty(salesclientcompanyencode) ? CurrentCompany : Encryptor.Decode(salesclientcompanyencode);
            
            ViewData["DefaultsSocialLog"] = string.Empty;
            if (!String.IsNullOrEmpty(salesclientcompanyencode)) //then get from salesforce
            {
                
                String DefaultsSocialLog = "false";
                IList<ConfigurationDefaults> configurations = ConfigurationDefaultsService.GetAllByClientCompany(localclientcompany);
                if (configurations != null && configurations.Count > 0)
                {
                    DefaultsSocialLog = configurations[0].SocialLog;
                }

                ViewData["DefaultsSocialLog"] = DefaultsSocialLog;
            }

            IList<Product> products = (List<Product>)Session["Products"];
            if (products == null)
                products = new List<Product>();
            if (!string.IsNullOrEmpty(industryIdRequest) && !string.IsNullOrEmpty(competitorIdRequest) && !string.IsNullOrEmpty(productIdRequest))
            {
                GetEntitiesToAddProduct(industryIdRequest, competitorIdRequest, productIdRequest, products, localclientcompany);
            }
            SetValuesToAddProduct(industryIdRequest, competitorIdRequest, products, localclientcompany);
            return View("Elements");
        }

        public void GetEntitiesToAddProduct(string IndustryId, string CompetitorId, string ProductId, IList<Product> products, string clientCompanyId)
        {
            Industry industry = IndustryService.GetById(decimal.Parse(IndustryId));
            Competitor competitor = CompetitorService.GetById(decimal.Parse(CompetitorId));
            Product product = ProductService.GetById(decimal.Parse(ProductId));
            string clientCompanyImageUrl = string.Empty;
            if (string.IsNullOrEmpty(CurrentCompany))
            {
                ClientCompany cc = ClientCompanyService.GetById(clientCompanyId);
                clientCompanyImageUrl = cc.Imageurl;
            } else clientCompanyImageUrl=(string)Session["Imageurl"];
            if (ProductService.HaveCompetitorClient(product.Id, clientCompanyId))
            {
                product.IsClientCompetitor = "Y";
            }
            //this method is to set the value check that this value no will save in database
            SetImageUrlToProduct(product, competitor.ImageUrl, clientCompanyImageUrl);
            if (products.Count == 0)
            {
                competitor.Industry = industry;
                product.IndustryId = industry.Id;
                product.Competitor = competitor;
                
                product.CompetitiveMessaging = new Positioning();
                product.CompetitiveMessaging = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Product, product.Id, (decimal)product.Competitor.Industry.Id, clientCompanyId);
                if (product.CompetitiveMessaging.Id == 0 || string.IsNullOrEmpty(product.CompetitiveMessaging.CreatedBy))
                {
                    product.CompetitiveMessaging = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Competitor, product.Competitor.Id, (decimal)product.Competitor.Industry.Id, clientCompanyId);
                }
                if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                {
                    string[] urlObjects = product.ImageUrl.Split('/');
                    Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                    newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                    if (newFileImage.FileFormat == null)
                    {
                        product.ImageUrl = "/Content/Images/Icons/none.png";
                    }
                }
                products.Add(product);
                string ImageUrl = GetUrlOfImage(product.ImageUrl);                
                                    
                ViewData["ImageDetail"] = ImageUrl;
            }
            else
            {
                if (products[0].IndustryId == industry.Id) // Other Industry
                {
                    if (!products.Contains(product))
                    {
                        competitor.Industry = industry;
                        product.IndustryId = industry.Id;
                        product.Competitor = competitor;
                        product.CompetitiveMessaging = new Positioning();
                        product.CompetitiveMessaging = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Product, product.Id, (decimal)product.Competitor.Industry.Id, clientCompanyId);
                        if (product.CompetitiveMessaging.Id == 0 || string.IsNullOrEmpty(product.CompetitiveMessaging.CreatedBy))
                        {
                            product.CompetitiveMessaging = PositioningService.GetByEntityAndIndustry(PositioningRelation.Positioning, DomainObjectType.Competitor, product.Competitor.Id, (decimal)product.Competitor.Industry.Id, clientCompanyId);
                        }
                        if (product.ImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                        {
                            string[] urlObjects = product.ImageUrl.Split('/');
                            Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                            newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                            if (newFileImage.FileFormat == null)
                            {
                                product.ImageUrl = "/Content/Images/Icons/none.png";
                            }
                        }
                        products.Add(product);
                        string ImageUrl = GetUrlOfImage(product.ImageUrl);                        
                        ViewData["ImageDetail"] = ImageUrl;
                    }
                }
                else
                {
                    ViewData["ShowMessage"] = true;
                }
            }
        }

        public void SetValuesToAddProduct(string industryId, string competitorId, IList<Product> products, string clientCompanyId)
        {
            IList<decimal> productIdsList = new List<decimal>();
            decimal industryIdG = 0;
            if (products != null && products.Count > 0)
            {
                industryIdG = products[0].Competitor.Industry.Id;
            }
            foreach (Product product in products)
            {
                productIdsList.Add(product.Id);
            }
            IList<System.Object[]> listObject = IndustryCriteriasService.GetMostRelevantProduct(productIdsList, industryIdG, clientCompanyId);
            IList<ComparinatorRecommendedProducts> crpList = new List<ComparinatorRecommendedProducts>();
            foreach (System.Object[] objt in listObject)
            {
                ComparinatorRecommendedProducts crp = new ComparinatorRecommendedProducts();
                crp.IndustryId = (decimal)(objt[1]);
                crp.ProductId = (decimal)(objt[2]);
                crp.ProductName = (string)objt[3] ;
                crp.ProductImageUrl = objt[4].ToString();
                if (crp.ProductImageUrl.IndexOf("./FilesRepository/Images/") == -1)
                {
                    string ImageUrl = "";
                    string[] urlObjects = crp.ProductImageUrl.Split('/');
                    Compelligence.Domain.Entity.File newFileImage = new Compelligence.Domain.Entity.File();
                    newFileImage.FileName = System.IO.Path.GetFileName(urlObjects[urlObjects.Length - 1]);
                    if (newFileImage.FileFormat == null)
                    {
                        ImageUrl = "/Content/Images/Icons/none.png";
                        crp.ProductImageUrl = ImageUrl;
                    }
                }
                crp.CompetitorName = (string)objt[5];
                crp.NumberOfCriteria = (decimal)(objt[6]);
                decimal? getLatValue = 0;
                getLatValue = decimal.Parse(objt[7].ToString());
                crp.ComparinatorPercent = Decimal.Round((decimal)(getLatValue / crp.NumberOfCriteria) * 100, 2);
                crpList.Add(crp);
            }
            ViewData["RecommendProducts"] = crpList;
            Session["Products"] = products;
            ViewData["Products"] = products;
            ViewData["Counter"] = products.Count.ToString();
            ViewData["IndustryId"] = industryId;
            ViewData["CompetitorId"] = competitorId;
            ViewData["ProductList"] = new MultiSelectList(products, "Id", "Name");
        }

        private string GetFileNameWithCompanyAndIndustry(string templateName, string extension)
        {
            string fileName = templateName;
            fileName += "-" + DateTime.Today.ToString("MMM-dd-yy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))+"."+extension;
            return fileName;
        }

        private void SetSecurityGroupIdToUser(UserProfile userProfile)
        {
            if (string.IsNullOrEmpty(userProfile.SecurityGroupId) || userProfile.SecurityGroupId.Equals("ROOT"))
            {
                IList<SecurityGroup> securityList = SecurityGroupService.GetByUser(userProfile.Id);
                if (securityList != null && securityList.Count > 0)
                {
                    if (securityList.Count > 1)
                    {
                        //to case the original  user, this can be ROOT and ADMIN
                        foreach (SecurityGroup securityGroup in securityList)
                        {
                            if (!securityGroup.Id.Equals("ROOT"))
                            {
                                userProfile.SecurityGroupId = securityGroup.Id;
                            }
                        }
                    }
                    else 
                        userProfile.SecurityGroupId = securityList[0].Id;
                }
            }
        }
        #region Override Methods
        protected override void SetDefaultDataByPage()
        {
            ViewData["Entity"] = FrontEndPages.Comparinator;
            ViewData["TitleHelp"] = "Comparinator";
        }
        #endregion
    }
}

