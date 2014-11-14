using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Compelligence.Domain.Entity;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity.Resource;
using Compelligence.Util.Type;
using Compelligence.Reports.Helpers;
using System.Configuration;
using System.Data;
using Compelligence.Reports.Dynamic;
using Compelligence.Domain.Entity.Views;
using Compelligence.Util.Validation;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace Compelligence.Web.Controllers
{
    public class SwotController : BackEndAsyncFormController<Swot, decimal>
    {
        #region Public Properties

        public ISwotService SwotService
        {
            get { return (ISwotService)_genericService; }
            set { _genericService = value; }
        }

        public IIndustryService IndustryService { get; set; }
        public ICompetitorService CompetitorService { get; set; }
        public IStrengthWeaknessService StrengthWeaknessService { get; set; }
        public ITrendService TrendService { get; set; }
        public IUserProfileService UserProfileService { get; set; }
        public IClientCompanyService ClientCompanyService { get; set; }
        public ICompetitorFinancialService CompetitorFinancialService{get; set;}
        public IStrengthWeaknessIndustryService StrengthWeaknessIndustryService { get; set; }
        private static string reportContext = AppDomain.CurrentDomain.BaseDirectory;

        IList<string> OpportunitiesChildList = new List<string>();
        IList<string> ThreatsChildList = new List<string>();
        IList<string> ThOppChildList = new List<string>();
        IList<string> StrengthChildList= new List<string>();
        IList<string> WeaknessChildList = new List<string>();

        IList<Trend> OpportunitiesChildListTrend = new List<Trend>();
        IList<Trend> ThreatsChildListTrend = new List<Trend>();
        IList<Trend> ThOppChildListTrend = new List<Trend>();

        IList<StrengthWeakness> StrengthChildListStrength = new List<StrengthWeakness>();
        IList<StrengthWeakness> WeaknessChildListStrength = new List<StrengthWeakness>();

        IList<SwotObjectView> objectList = new List<SwotObjectView>();
        #endregion

        //[AcceptVerbs(HttpVerbs.Post)]
        //public override ActionResult Index()
        //{
            
        //    ViewData["Scope"] = "Tools";
        //    ViewData["Container"] = "SwotContent";
            
            
        //    return View();
        //}

        protected override void LoadFormData()
        {
            IList<Industry> industrylist = IndustryService.GetAllSortByClientCompany("Name", CurrentCompany);
            ViewData["IndustryList"] = new SelectList(industrylist, "Id", "Name");
            IList<Competitor> competitorList = CompetitorService.GetAllSortByClientCompany("Name", CurrentCompany);
            ViewData["CompetitorList"] = new SelectList(competitorList, "Id", "Name");
            ViewData["UserSecurityAccess"] = "Create";
            ViewData["EntityLocked"] = "false";
        }

        /// Create
        /// 
        public ActionResult CreateSwot()
        {
            IList<IndustryByHierarchyView> industrylist = IndustryService.FindIndustryHierarchy(CurrentCompany);
            ViewData["IndustryList"] = new SelectList(industrylist, "Id", "Name");
            IList<Competitor> competitorList = CompetitorService.GetAllSortByClientCompany("Name", CurrentCompany);
            ViewData["CompetitorList"] = new SelectList(competitorList, "Id", "Name");
            ViewData["Scope"] = "Tools";            
            ViewData["EditHelp"] = (string)Session["EditHelp"];
            ViewData["ActionFrom"] = ActionFrom.BackEnd;
            ViewData["UserSecurityAccess"] = "Create";
            ViewData["EntityLocked"] = "false";
            ViewData["Container"] = "SwotContent";
            return View("EditSwot");
        }

        /// <summary>
        /// CreateSwot Post
        /// </summary>
        /// <returns></returns>


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSwot(FormCollection form)
        {
            string sIndustryId = StringUtility.CheckNull(form["IndustryId"]);
            string sCompeitorId = StringUtility.CheckNull(form["CompetitorId"]);
            string sWO = StringUtility.CheckNull(form["WO"]);
            string sSO = StringUtility.CheckNull(form["SO"]);
            string sST = StringUtility.CheckNull(form["ST"]);
            string sWT = StringUtility.CheckNull(form["WT"]);
            string changeSelect = form["typeChangeSelect"];
            decimal industryId = 0;
            decimal competitorId = 0;
            Swot swot = new Swot();
            if (!string.IsNullOrEmpty(sIndustryId) && !string.IsNullOrEmpty(sCompeitorId))
            {
                industryId = decimal.Parse(sIndustryId);
                competitorId = decimal.Parse(sCompeitorId);
                swot = SwotService.GetByIndustryIdAndCompetitorId(industryId, competitorId, CurrentCompany);
                if (swot != null)
                {
                    swot.WeaknessThreats = HttpUtility.HtmlDecode(sWT);
                    swot.WeaknessOpportunities = HttpUtility.HtmlDecode(sWO);
                    swot.StrengthThreats = HttpUtility.HtmlDecode(sST);
                    swot.StrengthOpportunities = HttpUtility.HtmlDecode(sSO);
                    swot.LastChangedBy = CurrentUser;
                    swot.LastChangedDate = DateTime.Now;
                    SwotService.Update(swot);
                }
                else
                {
                    swot = new Swot();
                    swot.WeaknessThreats = HttpUtility.HtmlDecode(sWT);
                    swot.WeaknessOpportunities = HttpUtility.HtmlDecode(sWO);
                    swot.StrengthThreats = HttpUtility.HtmlDecode(sST);
                    swot.StrengthOpportunities = HttpUtility.HtmlDecode(sSO);
                    swot.LastChangedBy = CurrentUser;
                    swot.LastChangedDate = DateTime.Now;

                    swot.IndustryId = industryId;
                    swot.CompetitorId = competitorId;
                    swot.CreatedBy = CurrentUser;
                    swot.CreatedDate = DateTime.Now;
                    swot.ClientCompany = CurrentCompany;
                    SwotService.Save(swot);
                }
                return RedirectToAction("EditSwot", new { SwotId = swot.Id, IndustryId = industryId, CompetitorId = competitorId, ChangeSelect = changeSelect });
            }
            return View("EditSwot", swot);
        }

        ///New Edit Swot
        ///
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditSwot(decimal SwotId, decimal IndustryId, decimal CompetitorId, string ChangeSelect)
        {
            IList<IndustryByHierarchyView> industrylist = new List<IndustryByHierarchyView>();
            IList<Competitor> competitorList = new List<Competitor>();
            if (ChangeSelect.Equals("ChangeIndustry"))
            {
                industrylist = IndustryService.FindIndustryHierarchy(CurrentCompany);
                competitorList = CompetitorService.GetByIndustryId(Convert.ToDecimal(IndustryId));
            }
            else
            {
                competitorList = CompetitorService.GetAllSortByClientCompany("Name", CurrentCompany);
                industrylist = IndustryService.GetIndustryHierarchyByCompetitor(Convert.ToDecimal(CompetitorId), CurrentCompany);
            }
            ViewData["IndustryList"] = new SelectList(industrylist, "Id", "Name", IndustryId);
            ViewData["CompetitorList"] = new SelectList(competitorList, "Id", "Name", CompetitorId);
            ViewData["Scope"] = "Tools";
            ViewData["UserSecurityAccess"] = "Create";
            ViewData["EntityLocked"] = "false";
            ViewData["Container"] = "SwotContent";
            Swot swot = SwotService.GetById(SwotId);
            return View(swot);
        }

        /// Edit Post
        /// 

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSwot(FormCollection form)
        {
            string sIndustryId = StringUtility.CheckNull(form["IndustryId"]);
            string sCompeitorId = StringUtility.CheckNull(form["CompetitorId"]);
            string sWO = StringUtility.CheckNull(form["WO"]);
            string sSO = StringUtility.CheckNull(form["SO"]);
            string sST = StringUtility.CheckNull(form["ST"]);
            string sWT = StringUtility.CheckNull(form["WT"]);
            decimal industryId = 0;
            decimal competitorId = 0;
            Swot swot = new Swot();
            if (!string.IsNullOrEmpty(sIndustryId) && !string.IsNullOrEmpty(sCompeitorId))
            {
                industryId = decimal.Parse(sIndustryId);
                competitorId = decimal.Parse(sCompeitorId);
                swot = SwotService.GetByIndustryIdAndCompetitorId(industryId, competitorId, CurrentCompany);
                if (swot != null)
                {
                    swot.WeaknessThreats = sWT;
                    swot.WeaknessOpportunities = sWO;
                    swot.StrengthThreats = sST;
                    swot.StrengthOpportunities = sSO;
                    swot.LastChangedBy = CurrentUser;
                    swot.LastChangedDate = DateTime.Now;
                    SwotService.Update(swot);
                }
                else
                {
                    swot = new Swot();
                    swot.WeaknessThreats = sWT;
                    swot.WeaknessOpportunities = sWO;
                    swot.StrengthThreats = sST;
                    swot.StrengthOpportunities = sSO;
                    swot.LastChangedBy = CurrentUser;
                    swot.LastChangedDate = DateTime.Now;

                    swot.IndustryId = industryId;
                    swot.CompetitorId = competitorId;
                    swot.CreatedBy = CurrentUser;
                    swot.CreatedDate = DateTime.Now;
                    swot.ClientCompany = CurrentCompany;
                    SwotService.Save(swot);
                }
                return View("EditSwot", swot);
            }
            return null;
        }
        public ContentResult GetQuickProfileCompetitorId() //Update Competitor and Product
        {
            string sCompetitorId = Request.Params["CompetitorId"];
            string sIndustryId = Request.Params["IndustryId"];
            string result = string.Empty;
            string totalrenue =  string.Empty;
            if (!string.IsNullOrEmpty(sCompetitorId) && !Validator.IsBlankOrNull(sIndustryId) && !Validator.Equals(sIndustryId,"null"))
            {
                decimal id = decimal.Parse(sCompetitorId);
                decimal Industryid = decimal.Parse(sIndustryId);
                Competitor competitor =CompetitorService.GetById(id);

                result += "Name_" + sCompetitorId + ":";
                result += "<div class='line' style='width:100%;height:15%'>";
                result += "<div style='width:31%;float: left;list-style:none outside none'>";
                result += "<li>Name</li>";
                result += "</div>";
                result += "<div   style='width:69%;float: left;left;list-style:none outside none'>";
                result += "<li>";
                result += competitor.Name;
                result += "</li>";
                result += "</div>";
                result += "</div>" + "[T+K]";
                IList<CompetitorFinancialIncomeStatement> competitorFinancialIncomeStatement = CompetitorFinancialService.GetByCompetitorAndType(id, "Quarterly", CurrentCompany);
                if (competitorFinancialIncomeStatement != null && competitorFinancialIncomeStatement.Count > 0)
                {
                    result += "Quarters_" + sCompetitorId + ":";
                    result += "<div class='line' style='width:100%;height:20%'>";
                    result += "<div style='width:31%;float: left;list-style:none outside none'>";
                    result += "<li>Last 3 Quarters</li>";
                    result += "</div>";
                    totalrenue += "TotalRenueve_" + sCompetitorId + ":";
                    totalrenue += "<div class='line' style='width:100%;height:20%'>";
                    totalrenue += "<div style='width:31%;float: left;left;list-style:none outside none'>";
                    totalrenue += "<li>Total Revenues</li>";
                    totalrenue += "</div>";
                    foreach (CompetitorFinancialIncomeStatement competitorIncomeStatement in competitorFinancialIncomeStatement)
                    {
                        result += "<div   style='width:23%;float: left;left;list-style:none outside none'>";
                        result += "<li>";
                        result += String.Format("{0:MMM d, yyyy}", competitorIncomeStatement.PeriodEnding);
                        result += "</li>";
                        result += "</div>";

                        totalrenue += "<div   style='width:23%;float: left;left;list-style:none outside none'>";
                        totalrenue += "<li>";
                        totalrenue += "$"+competitorIncomeStatement.TotalRevenue;
                        totalrenue += "</li>";
                        totalrenue += "</div>";
                    }
                    result += "</div>" + "[T+K]";
                    totalrenue += "</div>" + "[T+K]";
                    result = totalrenue + result;
                }
                long  Total = CompetitorService.CountByProductIndustryandCompetitor(Industryid,id,CurrentCompany);
                    result += "TotalProduc_" + sCompetitorId + ":";
                    result += "<div class='line' style='width:100%;height:20%'>";
                    result += "<div style='width:31%;float: left;list-style:none outside none'>";
                    result += "<li>Number of products:</li>";
                    result += "</div>";
                    result += "<div   style='width:69%;float: left;left;list-style:none outside none'>";
                    result += "<li>";
                    result += Total;
                    result += "</li>";
                    result += "</div>";
                    result += "</div>" + "[T+K]";

                    long TotalDeal = CompetitorService.CountByDealCompetitor(id,CurrentCompany);
                    result += "TotalDeal_" + sCompetitorId + ":";
                    result += "<div class='line' style='width:100%;height:20%'>";
                    result += "<div style='width:31%;float: left;list-style:none outside none'>";
                    result += "<li>Number of open deal support cases:</li>";
                    result += "</div>";
                    result += "<div   style='width:69%;float: left;left;list-style:none outside none;padding-top:14px'>";
                    result += "<li>";
                    result += TotalDeal;
                    result += "</li>";
                    result += "</div>";
                    result += "</div>" + "[T+K]";


            }
            return Content(result);
        }

        public string resultSWByCompetitor(IList<StrengthWeakness> ListChild, string type, string sIndustryId)
        {
            string result = string.Empty;
            if (ListChild.Count > 0)
            {
                result += "<div id='" + type + sIndustryId + "'>";
                result += "<ul list-style='circle'  style='margin-left: 20px;'>";
                foreach (StrengthWeakness oportunity in ListChild)
                {
                    result += "<img src='../../Content/Images/Icons/sub-IndustryRoll-up.jpg' style='float:left;width:17px;height:16px'/>";
                    result += "<li style='padding-left:30px;' title='" + oportunity.Description + "'>";
                    result += oportunity.Name;
                    result += "</li>";
                }
                result += "</ul>";
            }
            result += "</div>";

            return result;
        }
        public ContentResult GetSWByCompetitorId() //Update Competitor and Product
        {

            StrengthChildListStrength = new List<StrengthWeakness>();
            WeaknessChildListStrength = new List<StrengthWeakness>();
            string sCompetitorId = Request.Params["CompetitorId"];
            string sIndustryId = Request.Params["IndustryId"];
            string result = string.Empty;
            decimal idindustry = 0;
            if (!Validator.IsBlankOrNull(sIndustryId))
            {
                if (!Validator.Equals(sIndustryId, "null"))
                {
                    idindustry = decimal.Parse(sIndustryId);
                }
            }
            decimal id = -1;
            if (!string.IsNullOrEmpty(sCompetitorId))
            {
                 id = decimal.Parse(sCompetitorId);

            }

            string WeaknessChild = string.Empty;
            string StrengthChild = string.Empty;
            string Strength = string.Empty;
            string Weakness = string.Empty;
            string StrengthGlobal = string.Empty;
            string WeaknessGlobal = string.Empty;

                IList<StrengthWeakness> CopyTotalListOpportunity = StrengthWeaknessIndustryService.GetByIndustryIdStrength(idindustry, CurrentCompany, "",id);
                if (!Validator.IsBlankOrNull(sIndustryId) && !Validator.Equals(sIndustryId, "null"))
                {

                    IList<decimal> industryParents = new List<decimal>();
                    industryParents = IndustryService.GetChildrenIdList(idindustry, industryParents);
                    string resultchild = string.Empty;

                    foreach (decimal inustryid in industryParents)
                    {
                        IList<StrengthWeakness> TotalListStrength = StrengthWeaknessIndustryService.GetByIndustryIdStrengthLikeChild(idindustry, CurrentCompany, "", inustryid);
                        foreach(StrengthWeakness strengthWeakness in TotalListStrength)
                        {
                            CopyTotalListOpportunity.Remove(strengthWeakness);
                        }
                        WeaknessChild = GetStrWeahtml("StrengthChildren_", sCompetitorId, sIndustryId, id, StrengthWeaknessType.Strength, CurrentCompany, "C", inustryid, null);
                        StrengthChild = GetStrWeahtml("WeaknessChildren_", sCompetitorId, sIndustryId, id, StrengthWeaknessType.Weakness, CurrentCompany, "C", inustryid, null);
                    }
                }
                StrengthChildListStrength = removeStrengthDuplicates(StrengthChildListStrength);
                WeaknessChildListStrength = removeStrengthDuplicates(WeaknessChildListStrength);


                StrengthChild = resultSWByCompetitor(StrengthChildListStrength,"StrengthChildren_", sCompetitorId);
                result += "StrengthChildren_" + sCompetitorId + ":";
                result += StrengthChild;
                result += "[T+K]";    

                WeaknessChild = resultSWByCompetitor(WeaknessChildListStrength, "WeaknessChildren_", sCompetitorId);
                result += "WeaknessChildren_" + sCompetitorId + ":";
                result += WeaknessChild;
                result += "[T+K]"; 

            Strength=GetStrWeahtml("Strength_", sCompetitorId, sIndustryId, id, StrengthWeaknessType.Strength, CurrentCompany, "N", idindustry, CopyTotalListOpportunity);
            result += "Strength_" + sCompetitorId + ":";
            result += Strength;
            result += "[T+K]"; 
   
            Weakness=GetStrWeahtml("Weakness_", sCompetitorId, sIndustryId, id, StrengthWeaknessType.Weakness, CurrentCompany, "N", idindustry, CopyTotalListOpportunity);
            result += "Weakness_" + sCompetitorId + ":";
            result += Weakness;
            result += "[T+K]";

            StrengthGlobal=GetStrWeahtml("StrengthGlobal_", sCompetitorId, sIndustryId, id, StrengthWeaknessType.Strength, CurrentCompany, "Y", idindustry,null);
            result += "StrengthGlobal_" + sCompetitorId + ":";
            result += StrengthGlobal;
            result += "[T+K]";

            WeaknessGlobal=GetStrWeahtml("WeaknessGlobal_", sCompetitorId, sIndustryId, id, StrengthWeaknessType.Weakness, CurrentCompany, "Y", idindustry, null);
            result += "WeaknessGlobal_" + sCompetitorId + ":";
            result += WeaknessGlobal;
            result += "[T+K]";  

            return Content(result);
        }
        public string GetTrendsByIndustryChildrenId(IList<Trend> ListChild,string type,string sIndustryId)
        {
            string result=string.Empty;
                if (ListChild.Count > 0)
                {
                    result += "<div id='" + type + sIndustryId + "'>";
                    result += "<ul list-style='circle'  style='margin-left: 20px;'>";
                    foreach (Trend oportunity in ListChild)
                    {
                       result += "<img src='../../Content/Images/Icons/sub-IndustryRoll-up.jpg' style='float:left;width:17px;height:16px'/>";
                       result += "<li style='padding-left:30px;' title='" + oportunity.Description + "'>";
                       result += oportunity.Name;
                       result += "</li>";
                    }
                       result += "</ul>";
                 }
                    result += "</div>";

            return  result;
        }
        public string resultTrendsandOportunity(string type, string sIndustryId, decimal id, string typestring, string CurrentCompany,string typeindustry,IList<Trend> TotalList)
        {
            string result =string.Empty;
            IList<Trend> opportunityList = new List<Trend>();
            if (typeindustry.Equals(TrendIndustryType.IndustrySpecific))
            {
                opportunityList =TotalList;
            }
            else
            {
                opportunityList = TrendService.GetAllByIndustryType(id, typestring, CurrentCompany, typeindustry);
            }
           
            result += type + sIndustryId + ":";
            if (opportunityList != null && opportunityList.Count > 0)
            {
                result += "<div id='"+ type + sIndustryId+"'>";
                result += "<ul list-style='circle'  style='margin-left: 20px;'>";
                foreach (Trend oportunity in opportunityList)
                {
                    if (typeindustry.Equals(TrendIndustryType.Global))
                    {
                        result += "<img src='../../Content/Images/Icons/global.jpg' style='float:left;width:17px;height:15px'/>";
                    }
                    else if(typeindustry.Equals(TrendIndustryType.IndustrySpecific))
                    {
                         result += "<img src='../../Content/Images/Icons/current_Industry.jpg' style='float:left;width:18px'/>";
                    }
                    result += "<li style='padding-left:30px;' title='" + oportunity.Description + "'>";
                    result += oportunity.Name;
                    result += "</li>";
                }
                result += "</ul>";
                result += "</div>";
            }
            result += "[T+K]";
            return result;
        }
        public ContentResult GetTrendsByIndustryId(){
            string sIndustryId = Request.Params["IndustryId"];
            string sCompetitorId = Request.Params["CompetitorId"];
            string result = string.Empty;
            if (!string.IsNullOrEmpty(sIndustryId))
            {
                decimal id = decimal.Parse(sIndustryId);
                ///Get Opportunity
                ///


                OpportunitiesChildListTrend = new List<Trend>();
                ThreatsChildListTrend = new List<Trend>();
                ThOppChildListTrend = new List<Trend>();


                IList<decimal> industryParents = new List<decimal>();
                industryParents = IndustryService.GetChildrenIdList(id, industryParents);


                string resultchild=string.Empty;

                 
                IList<Trend> CopyTotalListOpportunity = TrendService.GetAllByIndustryType(id, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.IndustrySpecific); 
                 
                IList<Trend> CopyTotalListTrend = TrendService.GetAllByIndustryType(id, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.IndustrySpecific); 
                
                IList<Trend> CopyTotalListOpportunityTrend = TrendService.GetAllByIndustryType(id, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.IndustrySpecific);

                string Opportunitychild = string.Empty;
                string Threatchild = string.Empty;
                string OAndTchild = string.Empty;
                string OAndTchildThreat = string.Empty;
                string TrendOpportunity = string.Empty;
                string TrendThreat = string.Empty;
                string TrendThreatOpportunity = string.Empty;
                string TrendOpportunityGlobal = string.Empty;
                string TrendThreatGlobal = string.Empty;
                string TrendThreatOpportunityGlobal = string.Empty;
                foreach (decimal inustryid in industryParents)
                {
                    IList<Trend> opportunityList = TrendService.GetChildrenAllByIndustryId(id, TrendForSwot.Opportunity, CurrentCompany, inustryid);
                    foreach (Trend tren in opportunityList)
                    {
                        CopyTotalListOpportunity.Remove(tren);
                    }
                    IList<Trend> ListTrend = TrendService.GetChildrenAllByIndustryId(id, TrendForSwot.Threat, CurrentCompany, inustryid);
                    foreach (Trend tren in ListTrend)
                    {
                        CopyTotalListTrend.Remove(tren);
                    }
                    IList<Trend> ListOpportunityTrend = TrendService.GetChildrenAllByIndustryId(id, TrendForSwot.OAndT, CurrentCompany, inustryid);
                    foreach (Trend tren in ListOpportunityTrend)
                    {
                        CopyTotalListOpportunityTrend.Remove(tren);
                    }

                    Opportunitychild = GetTrendAndOpportunitieshtml("TrendOpportunityChildren_", sIndustryId, inustryid, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.IndustrySpecific, null, "child");
                    Threatchild = GetTrendAndOpportunitieshtml("TrendThreatChildren_", sIndustryId, inustryid, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.IndustrySpecific, null, "child");
                    OAndTchild = GetTrendAndOpportunitieshtml("TrendThreatOpportunityChildren_", sIndustryId, inustryid, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.IndustrySpecific, null, "child");
                }

                OpportunitiesChildListTrend = removeGRADuplicates(OpportunitiesChildListTrend);
                ThreatsChildListTrend = removeGRADuplicates(ThreatsChildListTrend);
                ThOppChildListTrend = removeGRADuplicates(ThOppChildListTrend);

                Opportunitychild = GetTrendsByIndustryChildrenId(OpportunitiesChildListTrend, "TrendOpportunityChildren_", sIndustryId);
                Threatchild = GetTrendsByIndustryChildrenId(ThreatsChildListTrend, "TrendThreatChildren_", sIndustryId);
                OAndTchild = GetTrendsByIndustryChildrenId(ThOppChildListTrend, "TrendThreatOpportunityChildren_", sIndustryId);
                OAndTchildThreat = GetTrendsByIndustryChildrenId(ThOppChildListTrend, "TrendThreatOpportunityChildrentrend_", sIndustryId);
                result += "TrendOpportunityChildren_" + sIndustryId + ":";
                result += Opportunitychild;
                result += "[T+K]";

                result += "TrendThreatChildren_" + sIndustryId + ":";
                result += Threatchild;
                result += "[T+K]";

                result += "TrendThreatOpportunityChildren_" + sIndustryId + ":";
                result +=  OAndTchild;
                result += "[T+K]";

                result += "TrendThreatOpportunityChildrentrend_" + sIndustryId + ":";
                result += OAndTchildThreat;
                result += "[T+K]";

                TrendOpportunity = GetTrendAndOpportunitieshtml("TrendOpportunity_", sIndustryId, id, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.IndustrySpecific, CopyTotalListOpportunity,string.Empty); 
                result += "TrendOpportunity_" + sIndustryId + ":";
                result += TrendOpportunity;
                result += "[T+K]";

                TrendThreat = GetTrendAndOpportunitieshtml("TrendThreat_", sIndustryId, id, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.IndustrySpecific, CopyTotalListTrend, string.Empty);
                result += "TrendThreat_" + sIndustryId + ":";
                result += TrendThreat;
                result += "[T+K]";

                TrendThreatOpportunity = GetTrendAndOpportunitieshtml("TrendThreatOpportunity_", sIndustryId, id, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.IndustrySpecific, CopyTotalListOpportunityTrend, string.Empty);
                result += "TrendThreatOpportunity_" + sIndustryId + ":";
                result += TrendThreatOpportunity;
                result += "[T+K]";

                string TrendThreatOpportunityTrend = GetTrendAndOpportunitieshtml("TrendThreatOpportunityTrend_", sIndustryId, id, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.IndustrySpecific, CopyTotalListOpportunityTrend, string.Empty);
                result += "TrendThreatOpportunityTrend_" + sIndustryId + ":";
                result += TrendThreatOpportunityTrend;
                result += "[T+K]";

                TrendOpportunityGlobal = GetTrendAndOpportunitieshtml("TrendOpportunityGlobal_", sIndustryId, id, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.Global, null, string.Empty);
                result += "TrendOpportunityGlobal_" + sIndustryId + ":";
                result += TrendOpportunityGlobal;
                result += "[T+K]";

                TrendThreatGlobal = GetTrendAndOpportunitieshtml("TrendThreatGlobal_", sIndustryId, id, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.Global, null, string.Empty);
                result += "TrendThreatGlobal_" + sIndustryId + ":";
                result += TrendThreatGlobal;
                result += "[T+K]";

                TrendThreatOpportunityGlobal = GetTrendAndOpportunitieshtml("TrendThreatOpportunityGlobal_", sIndustryId, id, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.Global, null, string.Empty);
                result += "TrendThreatOpportunityGlobal_" + sIndustryId + ":";
                result += TrendThreatOpportunityGlobal;
                result += "[T+K]";

                string TrendThreatOpportunityGlobalTrend = GetTrendAndOpportunitieshtml("TrendThreatOpportunityGlobalTrend_", sIndustryId, id, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.Global, null, string.Empty);
                result += "TrendThreatOpportunityGlobalTrend_" + sIndustryId + ":";
                result += TrendThreatOpportunityGlobalTrend;
                result += "[T+K]";  
            }
            return Content(result);
        }

        public ContentResult GetSwot() {
            string sIndustryId = Request.Params["IndustryId"];
            string sCompetitorId = Request.Params["CompetitorId"];
            string result = string.Empty;
            if (!string.IsNullOrEmpty(sIndustryId) && !string.IsNullOrEmpty(sCompetitorId))
            {
                decimal industryId = decimal.Parse(sIndustryId);
                decimal competitorId = decimal.Parse(sCompetitorId);

                Swot swot = SwotService.GetByIndustryIdAndCompetitorId(industryId, competitorId, CurrentCompany);
                if (swot != null)
                {
                    result += "SwotStrengthOpportunities_" + sIndustryId + ":" + swot.StrengthOpportunities + "[T+K]";
                    result += "SwotStrengthThreats_" + sIndustryId + ":" + swot.StrengthThreats + "[T+K]";
                    result += "SwotWeaknessOpportunities_" + sIndustryId + ":" + swot.WeaknessOpportunities + "[T+K]";
                    result += "SwotWeaknessThreats_" + sIndustryId + ":" + swot.WeaknessThreats + "[T+K]";
                }
                else
                {
                    result += "SwotStrengthOpportunities_" + sIndustryId + ":[T+K]";
                    result += "SwotStrengthThreats_" + sIndustryId + ":[T+K]";
                    result += "SwotWeaknessOpportunities_" + sIndustryId + ":[T+K]";
                    result += "SwotWeaknessThreats_" + sIndustryId + ":[T+K]";
                }

                IList<decimal> industryParents = new List<decimal>();
                industryParents = IndustryService.GetChildrenIdList(industryId, industryParents);
                string resultchildSwotStrength=string.Empty;
                string resultchildWeaknessOpportunities=string.Empty;
                string resultchildStrengthThreats=string.Empty;
                string resultchildhreatsChildren=string.Empty;
                if (industryParents.Count > 0)
                {
                    foreach (decimal inustryid in industryParents)
                    {
                        Industry indus= IndustryService.GetById(inustryid);
                        Swot swotChild = SwotService.GetByIndustryIdAndCompetitorId(inustryid, competitorId, CurrentCompany);
                        if (swotChild != null)
                        {
                            if (!string.IsNullOrEmpty(swotChild.StrengthOpportunities) && !Validator.Equals(swotChild.StrengthOpportunities,"<br>"))
                            {
                                resultchildSwotStrength += "<div class ='miDiv'>";
                                resultchildSwotStrength += "<u>S&O Strategies for " + indus.Name + "</u><br>";
                                resultchildSwotStrength += swotChild.StrengthOpportunities;
                                resultchildSwotStrength +="</div>";
                            }
                            if (!string.IsNullOrEmpty(swotChild.WeaknessOpportunities) && !Validator.Equals(swotChild.WeaknessOpportunities, "<br>"))
                            {
                                resultchildWeaknessOpportunities += "<div class ='miDiv'>";
                                resultchildWeaknessOpportunities += "<u>W&O Strategies for " + indus.Name + "</u><br>";
                                resultchildWeaknessOpportunities += swotChild.WeaknessOpportunities;
                                resultchildWeaknessOpportunities += "</div>";
                            }
                            if (!string.IsNullOrEmpty(swotChild.StrengthThreats) && !Validator.Equals(swotChild.StrengthThreats, "<br>"))
                            {
                                resultchildStrengthThreats += "<div class ='miDiv'>";
                                resultchildStrengthThreats += "<u>S&T Strategies for " + indus.Name + "</u><br>";
                                resultchildStrengthThreats += swotChild.StrengthThreats;
                                resultchildStrengthThreats += "</div>";
                            }
                            if (!string.IsNullOrEmpty(swotChild.WeaknessThreats) && !Validator.Equals(swotChild.WeaknessThreats, "<br>"))
                            {
                                resultchildhreatsChildren += "<div class ='miDiv'>";
                                resultchildhreatsChildren += "<u>W&T Strategies for " + indus.Name + "</u><br>";
                                resultchildhreatsChildren += swotChild.WeaknessThreats;
                                resultchildhreatsChildren += "</div>";
                            }
                        }
                        else
                        {
 
                        }
                    }

                    result += "SwotStrengthOpportunitiesChildren_" + sIndustryId + ":" + resultchildSwotStrength + "[T+K]";
                    result += "SwotWeaknessOpportunitiesChildren_" + sIndustryId + ":" + resultchildWeaknessOpportunities + "[T+K]";
                    result += "SwotStrengthThreatsChildren_" + sIndustryId + ":" + resultchildStrengthThreats + "[T+K]";
                    result += "SwotWeaknessThreatsChildren_" + sIndustryId + ":" + resultchildhreatsChildren + "[T+K]";
                }

            }
            return Content(result);
        }
        public string GetStrWea(string sCompetitorId, string sIndustryId, decimal id, string typestring, string CurrentCompany, string typeindustry, decimal idindustry, IList<StrengthWeakness> stre,bool check)
        {
            string result = string.Empty;
            if (id == -1)
            {
                sCompetitorId = "-1";
            }
            IList<StrengthWeakness> strengList = null;
            Industry industry = null;
            if (typeindustry.Equals("Y") && id != -1)
            {
                strengList = StrengthWeaknessService.GetEntityIdAndCuncurrentType(id, DomainObjectType.Competitor, typestring, CurrentCompany, typeindustry);
            }
            else if (!Validator.Equals(sIndustryId, "null") || !Validator.IsBlankOrNull(sIndustryId))
            {
                industry = IndustryService.GetById(idindustry);
                if (typeindustry.Equals("N") || typeindustry.Equals("C"))
                {
                    if (typeindustry.Equals("N"))
                    {
                        strengList = stre;
                    }
                    else if (typeindustry.Equals("C"))
                    {
                        strengList = StrengthWeaknessIndustryService.GetByIndustryIdStrength(idindustry, CurrentCompany, string.Empty,id);
                    }
                }
            }
            if (strengList != null && strengList.Count > 0)
            {
                foreach (StrengthWeakness strength in strengList)
                {
                    if (strength.Type.Equals(typestring) && strength.EntityType.Equals(DomainObjectType.Competitor))
                    {
                        if (typeindustry.Equals("Y") && check)
                        {
                            SwotObjectView sub = new SwotObjectView();
                            sub.GroupTitle = "Global";
                            if (typestring.Equals(StrengthWeaknessType.Strength))
                            {
                                sub.Type = SwotObjectReportType.Strength;
                            }
                            else
                            {
                                sub.Type = SwotObjectReportType.Weakness;
                            }
                            sub.Value = strength.Name;
                            //Image-SubRepor
                            sub.NameImage = "./Content/Images/Icons/" + "icon-global1.jpg";
                            objectList.Add(sub);
                           
                        }
                        else if (typeindustry.Equals("N") && check)
                        {
                            SwotObjectView sub = new SwotObjectView();
                            sub.GroupTitle = "Current";
                            if (typestring.Equals(StrengthWeaknessType.Weakness))
                            {
                                sub.Type = SwotObjectReportType.Weakness;
                            }
                            else
                            {
                                sub.Type = SwotObjectReportType.Strength;
                            }
                            sub.Value = strength.Name;
                            //Image-SubRepor
                            sub.NameImage = "./Content/Images/Icons/" + "icon-rft-arrow-green1.jpg";
                            objectList.Add(sub);
                        }
                        else if (typeindustry.Equals("C"))
                        {
                            if (typestring.Equals(StrengthWeaknessType.Strength))
                            {
                                StrengthChildList.Add(strength.Name);
                            }
                            else if (typestring.Equals(StrengthWeaknessType.Weakness))
                            {
                                WeaknessChildList.Add(strength.Name);
                            }
                        }
                    }
                }
            }

            return result;
        }
        private IList<Trend> removeGRADuplicates(IList<Trend> inputList)
        {
            Dictionary<int, int> uniqueStore = new Dictionary<int, int>();
            IList<Trend> finalList = new List<Trend>();
            foreach (Trend gra in inputList)
            {
                if (!uniqueStore.ContainsKey(Convert.ToInt32(gra.Id)))
                {
                    uniqueStore.Add(Convert.ToInt32(gra.Id), 0);
                    finalList.Add(gra);
                }
            }
            return finalList;
        }
        public string GetTrendAndOpportunitiesTxt(IList<string> oportunity,string type, bool check)
        {
            string result = string.Empty;
            foreach (string op in oportunity)
            {
                if (type.Equals("Oportunity") && check)
                {
                    SwotObjectView sub = new SwotObjectView();
                    sub.GroupTitle = "Sub Industry";
                    sub.Type = SwotObjectReportType.Oportunity;
                    sub.Value = op;
                    //Image-SubRepor
                    sub.NameImage = "./Content/Images/Icons/" + "icon-lft-arrow-blue1.jpg";
                    objectList.Add(sub);
                }
                else if (type.Equals("Threats") && check)
                {
                    SwotObjectView sub = new SwotObjectView();
                    sub.GroupTitle = "Sub Industry";
                    sub.Type = SwotObjectReportType.Threats;
                    sub.Value = op;
                    //Image-SubRepor
                    sub.NameImage = "./Content/Images/Icons/" + "icon-lft-arrow-blue1.jpg";
                    objectList.Add(sub);
                }
                else if (type.Equals("OT") && check)
                {
                    SwotObjectView sub = new SwotObjectView();
                    sub.GroupTitle = "Sub Industry";
                    sub.Type = SwotObjectReportType.Threats;
                    sub.Value = op;
                    sub.NameImage = "./Content/Images/Icons/" + "icon-lft-arrow-blue1.jpg";
                    objectList.Add(sub);
                    SwotObjectView subOT = new SwotObjectView();
                    subOT.GroupTitle = "Sub Industry";
                    subOT.Type = SwotObjectReportType.Oportunity;
                    subOT.NameImage = "./Content/Images/Icons/" + "icon-lft-arrow-blue1.jpg";
                    subOT.Value = op;
                    //Image-SubRepor
                    objectList.Add(subOT);
                }
                else if (type.Equals("Strength") && check)
                {
                    SwotObjectView sub = new SwotObjectView();
                    sub.GroupTitle = "Sub Industry";
                    sub.Type = SwotObjectReportType.Strength;
                    sub.Value = op;
                    //Image-SubRepor
                    sub.NameImage = "./Content/Images/Icons/" + "icon-lft-arrow-blue1.jpg";
                    objectList.Add(sub);
                }
                else if (type.Equals("Weakness")&&check)
                {
                    SwotObjectView sub = new SwotObjectView();
                    sub.GroupTitle = "Sub Industry";
                    sub.Type = SwotObjectReportType.Weakness;
                    sub.Value = op;
                    //Image-SubRepor
                    sub.NameImage = "./Content/Images/Icons/" + "icon-lft-arrow-blue1.jpg";
                    objectList.Add(sub);
                }
                result += "\n" + op;
            }
            return result;
        }
        private IList<StrengthWeakness> removeStrengthDuplicates(IList<StrengthWeakness> inputList)
        {
            Dictionary<int, int> uniqueStore = new Dictionary<int, int>();
            IList<StrengthWeakness> finalList = new List<StrengthWeakness>();
            foreach (StrengthWeakness gra in inputList)
            {
                if (!uniqueStore.ContainsKey(Convert.ToInt32(gra.Id)))
                {
                    uniqueStore.Add(Convert.ToInt32(gra.Id), 0);
                    finalList.Add(gra);
                }
            }
            return finalList;
        }
        public string GetTrendAndOpportunities(string sIndustryId, decimal id, string typestring, string CurrentCompany, string typeindustry, IList<Trend> TotalList,string child,bool check)
        {
            string result = string.Empty;
            IList<Trend> opportunityList = new List<Trend>();
            if (typeindustry.Equals(TrendIndustryType.IndustrySpecific) && TotalList!=null)
            {
                opportunityList = TotalList;
            }
            else
            {
                opportunityList = TrendService.GetAllByIndustryType(id, typestring, CurrentCompany, typeindustry);
            }
            if (opportunityList != null && opportunityList.Count > 0)
            {
    
                foreach (Trend oportunity in opportunityList)
                {
                    if (typestring.Equals(TrendForSwot.Opportunity))
                    {
                        if (child.Equals("child"))
                        {
                            OpportunitiesChildList.Add(oportunity.Name);
                        }
                        else
                        {
                            if (check)
                            {
                                SwotObjectView soCu = new SwotObjectView();
                                soCu.Value = oportunity.Name;
                                if (typeindustry.Equals(TrendIndustryType.IndustrySpecific))
                                {
                                    soCu.GroupTitle = "Current";
                                    soCu.NameImage = "./Content/Images/Icons/" + "icon-rft-arrow-green1.jpg";
                                }
                                else
                                {
                                    soCu.GroupTitle = "Global";
                                    soCu.NameImage = "./Content/Images/Icons/" + "icon-global1.jpg";
                                }
                                soCu.Type = SwotObjectReportType.Oportunity;
                                //Image-SubRepor

                                objectList.Add(soCu);
                            }
                        }
                    }
                    else if (typestring.Equals(TrendForSwot.Threat))
                    {
                        if (child.Equals("child"))
                        {
                            ThreatsChildList.Add(oportunity.Name);
                        }
                        else
                        {
                            if (check)
                            {
                                SwotObjectView soCu = new SwotObjectView();
                                soCu.Value = oportunity.Name;
                                if (typeindustry.Equals(TrendIndustryType.IndustrySpecific))
                                {
                                    soCu.GroupTitle = "Current";
                                    soCu.NameImage = "./Content/Images/Icons/" + "icon-rft-arrow-green1.jpg";
                                }
                                else
                                {
                                    soCu.GroupTitle = "Global";
                                    soCu.NameImage = "./Content/Images/Icons/" + "icon-global1.jpg";
                                }
                                soCu.Type = SwotObjectReportType.Threats;
                                //Image-SubRepor
                                objectList.Add(soCu);
                            }
                        }
                    }
                    else if (typestring.Equals(TrendForSwot.OAndT))
                    {
                        if (child.Equals("child"))
                        {
                            ThOppChildList.Add(oportunity.Name);
                        }
                        else
                        {
                            if (check)
                            {
                                SwotObjectView soCu = new SwotObjectView();
                                soCu.Value = oportunity.Name;
                                if (typeindustry.Equals(TrendIndustryType.IndustrySpecific))
                                {
                                    soCu.GroupTitle = "Current";
                                    soCu.NameImage = "./Content/Images/Icons/" + "icon-rft-arrow-green1.jpg";
                                }
                                else
                                {
                                    soCu.GroupTitle = "Global";
                                    soCu.NameImage = "./Content/Images/Icons/" + "icon-global1.jpg";
                                }
                                soCu.Type = SwotObjectReportType.Oportunity;
                                //Image-SubRepor
                                objectList.Add(soCu);

                                SwotObjectView soCuT = new SwotObjectView();
                                soCuT.Value = oportunity.Name;
                                if (typeindustry.Equals(TrendIndustryType.IndustrySpecific))
                                {
                                    soCuT.GroupTitle = "Current";
                                    soCuT.NameImage = "./Content/Images/Icons/" + "icon-rft-arrow-green1.jpg";
                                }
                                else
                                {
                                    soCuT.GroupTitle = "Global";
                                    soCuT.NameImage = "./Content/Images/Icons/" + "icon-global1.jpg";
                                }
                                soCuT.Type = SwotObjectReportType.Threats;
                                //Image-SubRepor
                                objectList.Add(soCuT);
                            }
                        }
                    }
                   
                }
            }
            return result;
        }
        public string GetTrendAndOpportunitieshtml(string type,string sIndustryId, decimal id, string typestring, string CurrentCompany, string typeindustry, IList<Trend> TotalList, string child)
        {
            string result = string.Empty;
            IList<Trend> opportunityList = new List<Trend>();
            if (typeindustry.Equals(TrendIndustryType.IndustrySpecific) && TotalList != null)
            {
                opportunityList = TotalList;
            }
            else
            {
                opportunityList = TrendService.GetAllByIndustryType(id, typestring, CurrentCompany, typeindustry);
            }
            if (opportunityList != null && opportunityList.Count > 0)
            {
                result += "<div id='" + type + sIndustryId + "'>";
                result += "<ul list-style='circle'  style='margin-left: 20px;'>";
                foreach (Trend oportunity in opportunityList)
                {

                    if (typeindustry.Equals(TrendIndustryType.Global))
                    {
                        result += "<img src='../../Content/Images/Icons/global.jpg' style='float:left;width:17px;height:15px'/>";
                    }
                    else if (typeindustry.Equals(TrendIndustryType.IndustrySpecific))
                    {
                        result += "<img src='../../Content/Images/Icons/current_Industry.jpg' style='float:left;width:18px'/>";
                    }
                    result += "<li style='padding-left:30px;' title='" + oportunity.Description + "'>";
                    result += oportunity.Name;
                    result += "</li>";

                    if (typestring.Equals(TrendForSwot.Opportunity))
                    {
                        if (child.Equals("child"))
                        {
                            OpportunitiesChildListTrend.Add(oportunity);
                            
                        }
                    }
                    else if (typestring.Equals(TrendForSwot.Threat))
                    {
                        if (child.Equals("child"))
                        {
                            ThreatsChildListTrend.Add(oportunity);
                            
                        }
                    }
                    else if (typestring.Equals(TrendForSwot.OAndT))
                    {
                        if (child.Equals("child"))
                        {
                            ThOppChildListTrend.Add(oportunity);
                        }
                    }

                }
                result += "</ul>";
                result += "</div>";
            }
            return result;
        }
        public string GetStrWeahtml(string type,string sCompetitorId, string sIndustryId, decimal id, string typestring, string CurrentCompany, string typeindustry, decimal idindustry, IList<StrengthWeakness> stre)
        {
            string result = string.Empty;
            if (id == -1)
            {
                sCompetitorId = "-1";
            }
            IList<StrengthWeakness> strengList = null;
            Industry industry = null;
            if (typeindustry.Equals("Y") && id != -1)
            {
                strengList = StrengthWeaknessService.GetEntityIdAndCuncurrentType(id, DomainObjectType.Competitor, typestring, CurrentCompany, typeindustry);
            }
            else if (!Validator.Equals(sIndustryId, "null") || !Validator.IsBlankOrNull(sIndustryId))
            {
                industry = IndustryService.GetById(idindustry);
                if (typeindustry.Equals("N") || typeindustry.Equals("C"))
                {
                    if (typeindustry.Equals("N"))
                    {
                        strengList = stre;
                    }
                    else if (typeindustry.Equals("C"))
                    {
                        strengList = StrengthWeaknessIndustryService.GetByIndustryIdStrength(idindustry, CurrentCompany, string.Empty,id);
                    }
                }
            }
            if (strengList != null && strengList.Count > 0)
            {
                result += "<div id='" + type + sCompetitorId + "'>";
                result += "<ul list-style='circle' style='margin-left: 20px;'>";
                foreach (StrengthWeakness strength in strengList)
                {
                    if (strength.Type.Equals(typestring) && strength.EntityType.Equals(DomainObjectType.Competitor))
                    {
                        if (typeindustry.Equals("Y"))
                        {
                            result += "<img src='../../Content/Images/Icons/global.jpg' style='float:left;width:17px;height:15px'/>";
                        }
                        else if (typeindustry.Equals("N"))
                        {
                            result += "<img src='../../Content/Images/Icons/current_Industry.jpg' style='float:left;width:18px'/>";
                        }
                        else if (typeindustry.Equals("C"))
                        {
                            if (typestring.Equals(StrengthWeaknessType.Strength))
                            {
                                StrengthChildListStrength.Add(strength);
                            }
                            else if (typestring.Equals(StrengthWeaknessType.Weakness))
                            {
                                WeaknessChildListStrength.Add(strength);
                            }
                            result += "<img src='../../Content/Images/Icons/sub-IndustryRoll-up.jpg' style='float:left;width:17px;height:16px'/>";
                        }
                        result += "<li style='padding-left:30px;' title='" + strength.Description + "'>";
                        result += strength.Name;
                        result += "</li>";
                    }
                }
                result += "</ul>" + "</div>";
            }

            return result;
        }
        public IList<string> removeDuplicates(IList<string> inputList)
        {
            Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
            IList<string> finalList = new List<string>();
            foreach (string currValue in inputList)
            {
                if (!uniqueStore.ContainsKey(currValue))
                {
                    uniqueStore.Add(currValue, 0);
                    finalList.Add(currValue);
                }
            }
            return finalList;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GenerateDocumentoSwot()
        {
            string TDocument = HttpUtility.HtmlEncode(Request["TypeDocument"]);
            string IndustryId = HttpUtility.HtmlEncode(Request["IndustryId"]);
            string CompetitorId = HttpUtility.HtmlEncode(Request["CompetitorId"]);
            string currentIndustry = HttpUtility.HtmlEncode(Request["CurrentIndustry"]);
            string global = HttpUtility.HtmlEncode(Request["Global"]);
            string subIndustry = HttpUtility.HtmlEncode(Request["SubIndustry"]);
            
            bool showCurrentIndustry = GetValueBoolOfString(currentIndustry);
            bool showGlobalIndustry = GetValueBoolOfString(global);
            bool showSubIndustry = GetValueBoolOfString(subIndustry);

            OpportunitiesChildList = new List<string>();
            ThreatsChildList = new List<string>();
            ThOppChildList = new List<string>();

            string OpportunitiesIndustry = string.Empty;
            string Opportunities = string.Empty;
            string OpportunitiesChild = string.Empty;
            string OpportunitiesGlobal = string.Empty;

            string ThreatsIndustry = string.Empty;
            string Threats = string.Empty;
            string ThreatsChild = string.Empty;
            string ThreatsGlobal = string.Empty;

            string ThOpp = string.Empty;
            string ThOppChild = string.Empty;
            string ThOppGlobal = string.Empty;

            string Strength = string.Empty;
            string StrengthIndustry = string.Empty;
            string StrengthChild = string.Empty;
            string StrengthGlobal = string.Empty;

            string Weakness = string.Empty;
            string WeaknessIndustry = string.Empty;
            string WeaknessChild = string.Empty;
            string WeaknessGlobal = string.Empty;

            string SO = string.Empty;
            string SOChild = string.Empty;

            string WO = string.Empty;
            string WOChild = string.Empty;

            string ST = string.Empty;
            string STChild = string.Empty;

            string WT = string.Empty;
            string WTChild = string.Empty;

            string QuickProfile = string.Empty;
            if (!string.IsNullOrEmpty(IndustryId) && !string.IsNullOrEmpty(CompetitorId))
            {
                IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();
                decimal industryId = decimal.Parse(IndustryId);
                decimal competitorId = decimal.Parse(CompetitorId);
                Industry industry = IndustryService.GetById(industryId);
                Competitor competitor = CompetitorService.GetById(competitorId);
                if (industry != null && competitor != null)
                {
                    objectList = new List<SwotObjectView>();
                    Swot swot = SwotService.GetByIndustryIdAndCompetitorId(industryId, competitorId, CurrentCompany);
                    if (swot == null)
                    {
                        swot = new Swot();
                    }
                    SwotAllView swotAll = new SwotAllView();
                    IList<decimal> industryParents = new List<decimal>();
                    industryParents = IndustryService.GetChildrenIdList(industryId, industryParents);
                    IList<Trend> CopyTotalListOpportunity = TrendService.GetAllByIndustryType(industryId, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.IndustrySpecific);
                    IList<Trend> CopyTotalListTrend = TrendService.GetAllByIndustryType(industryId, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.IndustrySpecific);
                    IList<Trend> CopyTotalListOpportunityTrend = TrendService.GetAllByIndustryType(industryId, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.IndustrySpecific);

                    foreach (decimal inustryid in industryParents)
                    {
                        IList<Trend> opportunityList = TrendService.GetChildrenAllByIndustryId(industryId, TrendForSwot.Opportunity, CurrentCompany, inustryid);
                        foreach (Trend tren in opportunityList)
                        {
                            CopyTotalListOpportunity.Remove(tren);
                        }
                        IList<Trend> ListTrend = TrendService.GetChildrenAllByIndustryId(industryId, TrendForSwot.Threat, CurrentCompany, inustryid);
                        foreach (Trend tren in ListTrend)
                        {
                            CopyTotalListTrend.Remove(tren);
                        }
                        IList<Trend> ListOpportunityTrend = TrendService.GetChildrenAllByIndustryId(industryId, TrendForSwot.OAndT, CurrentCompany, inustryid);
                        foreach (Trend tren in ListOpportunityTrend)
                        {
                            CopyTotalListOpportunityTrend.Remove(tren);
                        }
                        OpportunitiesChild = GetTrendAndOpportunities(IndustryId, inustryid, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.IndustrySpecific, null, "child", true);
                        ThreatsChild = GetTrendAndOpportunities(IndustryId, inustryid, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.IndustrySpecific, null, "child", true);
                        ThOppChild = GetTrendAndOpportunities(IndustryId, inustryid, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.IndustrySpecific, null, "child", true);
                    }
                    OpportunitiesChildList = removeDuplicates(OpportunitiesChildList);
                    ThreatsChildList = removeDuplicates(ThreatsChildList);
                    ThOppChildList = removeDuplicates(ThOppChildList);

                    OpportunitiesChild = GetTrendAndOpportunitiesTxt(OpportunitiesChildList, "Oportunity", showSubIndustry);
                    ThreatsChild = GetTrendAndOpportunitiesTxt(ThreatsChildList, "Threats", showSubIndustry);
                    ThOppChild = GetTrendAndOpportunitiesTxt(ThOppChildList, "OT", showSubIndustry);

                    OpportunitiesIndustry += GetTrendAndOpportunities(IndustryId, industryId, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.IndustrySpecific, CopyTotalListOpportunity, string.Empty, showCurrentIndustry);
                    ThreatsIndustry += GetTrendAndOpportunities(IndustryId, industryId, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.IndustrySpecific, CopyTotalListTrend, string.Empty, showCurrentIndustry);
                    ThOpp += GetTrendAndOpportunities(IndustryId, industryId, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.IndustrySpecific, CopyTotalListOpportunityTrend, string.Empty, showCurrentIndustry);

                    OpportunitiesGlobal += GetTrendAndOpportunities(IndustryId, industryId, TrendForSwot.Opportunity, CurrentCompany, TrendIndustryType.Global, null, string.Empty, showGlobalIndustry);
                    ThreatsGlobal += GetTrendAndOpportunities(IndustryId, industryId, TrendForSwot.Threat, CurrentCompany, TrendIndustryType.Global, null, string.Empty, showGlobalIndustry);
                    ThOppGlobal += GetTrendAndOpportunities(IndustryId, industryId, TrendForSwot.OAndT, CurrentCompany, TrendIndustryType.Global, null, string.Empty, showGlobalIndustry);
                    string contentPart = "./Content/Images/Icons/";
                    IList<StrengthWeakness> CopyTotalListStrength = StrengthWeaknessIndustryService.GetByIndustryIdStrength(industryId, CurrentCompany, "", competitorId);
                    foreach (decimal inustryid in industryParents)
                    {
                        IList<StrengthWeakness> TotalListStrength = StrengthWeaknessIndustryService.GetByIndustryIdStrengthLikeChild(industryId, CurrentCompany, "", inustryid);
                        foreach (StrengthWeakness strengthWeakness in TotalListStrength)
                        {
                            CopyTotalListStrength.Remove(strengthWeakness);
                        }
                        WeaknessChild = GetStrWea(CompetitorId, IndustryId, competitorId, StrengthWeaknessType.Strength, CurrentCompany, "C", inustryid, null, true);
                        StrengthChild = GetStrWea(CompetitorId, IndustryId, competitorId, StrengthWeaknessType.Weakness, CurrentCompany, "C", inustryid, null, true);
                    }
                    StrengthChildList = removeDuplicates(StrengthChildList);
                    WeaknessChildList = removeDuplicates(WeaknessChildList);

                    StrengthChild = GetTrendAndOpportunitiesTxt(StrengthChildList, "Strength", showSubIndustry);
                    WeaknessChild = GetTrendAndOpportunitiesTxt(WeaknessChildList, "Weakness", showSubIndustry);
                    StrengthIndustry = GetStrWea(CompetitorId, IndustryId, competitorId, StrengthWeaknessType.Strength, CurrentCompany, "N", industryId, CopyTotalListStrength, showCurrentIndustry);
                    WeaknessIndustry = GetStrWea(CompetitorId, IndustryId, competitorId, StrengthWeaknessType.Weakness, CurrentCompany, "N", industryId, CopyTotalListStrength, showCurrentIndustry);
                    StrengthGlobal = GetStrWea(CompetitorId, IndustryId, competitorId, StrengthWeaknessType.Strength, CurrentCompany, "Y", industryId, null, showGlobalIndustry);
                    WeaknessGlobal = GetStrWea(CompetitorId, IndustryId, competitorId, StrengthWeaknessType.Weakness, CurrentCompany, "Y", industryId, null, showGlobalIndustry);



                    swotAll.Id = swot.Id;
                    swotAll.IndustryId = swot.IndustryId;
                    swotAll.CompetitorId = swot.CompetitorId;

                    swotAll.StrengthOpportunities = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swot.StrengthOpportunities));
                    swotAll.WeaknessOpportunities = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swot.WeaknessOpportunities));
                    swotAll.StrengthThreats = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swot.StrengthThreats));
                    swotAll.WeaknessThreats = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swot.WeaknessThreats));

                    foreach (decimal inustryid in industryParents)
                    {
                        Industry indus = IndustryService.GetById(inustryid);
                        Swot swotChild = SwotService.GetByIndustryIdAndCompetitorId(inustryid, competitorId, CurrentCompany);
                        if (swotChild != null)
                        {
                            if (!string.IsNullOrEmpty(swotChild.StrengthOpportunities) && !Validator.Equals(swotChild.StrengthOpportunities, "<br>"))
                            {
                                SwotObjectView sub = new SwotObjectView();
                                sub.GroupTitle = "S&O Strategies for " + indus.Name;
                                string values = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swotChild.StrengthOpportunities));
                                sub.Value = values;
                                sub.Type = SwotObjectReportType.SOSub;
                                sub.NameImage = contentPart + "bullet_black.jpg";
                                objectList.Add(sub);
                            }
                            if (!string.IsNullOrEmpty(swotChild.WeaknessOpportunities) && !Validator.Equals(swotChild.WeaknessOpportunities, "<br>"))
                            {
                                SwotObjectView sub = new SwotObjectView();
                                sub.GroupTitle = "W&O Strategies for " + indus.Name;
                                sub.Value = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swotChild.WeaknessOpportunities));
                                sub.Type = SwotObjectReportType.WOSub;
                                sub.NameImage = contentPart + "bullet_black.jpg";
                                objectList.Add(sub);
                            }
                            if (!string.IsNullOrEmpty(swotChild.StrengthThreats) && !Validator.Equals(swotChild.StrengthThreats, "<br>"))
                            {
                                SwotObjectView sub = new SwotObjectView();
                                sub.GroupTitle = "S&T Strategies for " + indus.Name;
                                sub.Value = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swotChild.StrengthThreats));
                                sub.Type = SwotObjectReportType.STSub;
                                sub.NameImage = contentPart + "bullet_black.jpg";
                                objectList.Add(sub);
                            }
                            if (!string.IsNullOrEmpty(swotChild.WeaknessThreats) && !Validator.Equals(swotChild.WeaknessThreats, "<br>"))
                            {
                                SwotObjectView sub = new SwotObjectView();
                                sub.GroupTitle = "W&T Strategies for " + indus.Name;
                                sub.Value = StringUtility.CleanHtmlTagAndSpecialCharacter(StringUtility.ReplaceHtmlTag(swotChild.WeaknessThreats));
                                sub.Type = SwotObjectReportType.WTSub;
                                sub.NameImage = contentPart + "bullet_black.jpg";
                                objectList.Add(sub);
                            }
                        }
                    }
                    SO = swot.StrengthOpportunities;
                    WO = swot.WeaknessOpportunities;
                    ST = swot.StrengthThreats;
                    WT = swot.WeaknessOpportunities;

                    string NameQuick = "\n Name \t" + competitor.Name;
                    string Quarters = string.Empty;
                    string ValuesQuarter = string.Empty;
                    string ValuesResultTotal = string.Empty;
                    string ResultTotal = string.Empty;
                    IList<CompetitorFinancialIncomeStatement> competitorFinancialIncomeStatement = CompetitorFinancialService.GetByCompetitorAndType(competitorId, "Quarterly", CurrentCompany);
                    if (competitorFinancialIncomeStatement != null && competitorFinancialIncomeStatement.Count > 0)
                    {
                        Quarters = "\n Last 3 Quarters";
                        ResultTotal = "\n Total Revenues";
                        foreach (CompetitorFinancialIncomeStatement competitorIncomeStatement in competitorFinancialIncomeStatement)
                        {
                            Quarters += DateTimeUtility.ConvertToString(competitorIncomeStatement.PeriodEnding, "dd/MM/yyyy");
                            ValuesQuarter += String.Format("{0:MMM d, yyyy}", competitorIncomeStatement.PeriodEnding) + "\t";
                            ResultTotal += competitorIncomeStatement.TotalRevenue;
                            ValuesResultTotal += "$"+competitorIncomeStatement.TotalRevenue.Trim() + "\t";
                        }
                    }
                    long Total = CompetitorService.CountByProductIndustryandCompetitor(industryId, competitorId, CurrentCompany);
                    string TotalProduct = "\n Number of products : " + Total;
                    long TotalDeal = CompetitorService.CountByDealCompetitor(competitorId, CurrentCompany);
                    string DealSuport = " \n Number of open deal support cases : " + TotalDeal;
                    QuickProfile = NameQuick + Quarters + ResultTotal + TotalProduct + DealSuport;

                    SwotObjectView sub1 = new SwotObjectView();
                    sub1.GroupTitle = "1";
                    sub1.Value = competitor.Name;
                    sub1.Type = "CompetitorQuick";
                    sub1.Label = "Name";
                    objectList.Add(sub1);

                    SwotObjectView sub2 = new SwotObjectView();
                    sub2.GroupTitle = "2";
                    sub2.Value = ValuesQuarter;
                    sub2.Type = "CompetitorQuick";
                    sub2.Label = "Last 3 Quarters";
                    objectList.Add(sub2);

                    SwotObjectView sub3 = new SwotObjectView();
                    sub3.GroupTitle = "3";
                    sub3.Value = ValuesResultTotal;
                    sub3.Type = "CompetitorQuick";
                    sub3.Label = "Total Revenues";
                    objectList.Add(sub3);

                    SwotObjectView sub4 = new SwotObjectView();
                    sub4.GroupTitle = "4";
                    sub4.Value = Total.ToString();
                    sub4.Type = "CompetitorQuick";
                    sub4.Label = "Number of products";
                    objectList.Add(sub4);

                    SwotObjectView sub5 = new SwotObjectView();
                    sub5.GroupTitle = "5";
                    sub5.Value = TotalDeal.ToString();
                    sub5.Type = "CompetitorQuick";
                    sub5.Label = "Number of open deal support cases";
                    objectList.Add(sub5);

                    swotAll.ClientCompany = swot.ClientCompany;
                    swotAll.IndustryName = industry.Name;
                    swotAll.CompetitorName = competitor.Name;
                    reportParameters["IndustryName"] = industry.Name;
                    reportParameters["CompetitorName"] = competitor.Name;
                    swot.Opportunities = Opportunities;
                    reportParameters["Opportunities"] = Opportunities;
                    swot.Threats = Threats;
                    reportParameters["Threats"] = Threats;
                    swot.Strength = Strength;
                    reportParameters["Strength"] = Strength;
                    swot.Weakness = Weakness;
                    reportParameters["Weakness"] = Weakness;
                    reportParameters["QuickProfile"] = QuickProfile;
                    reportParameters["StrengthOpportunities"] = HttpUtility.HtmlEncode(SO);
                    reportParameters["WeaknessOpportunities"] = HttpUtility.HtmlEncode(WO);
                    reportParameters["StrengthThreats"] = ST;
                    reportParameters["WeaknessThreats"] = WT;

                    reportParameters["CurrentIndustry"] = showCurrentIndustry.ToString();
                    reportParameters["GlobalIndustry"] = showGlobalIndustry.ToString();
                    reportParameters["SubIndustry"] = showSubIndustry.ToString();

                    reportParameters["WOSubIndustry"] = WOChild;
                    reportParameters["SOSubIndustry"] = SOChild;
                    reportParameters["STSubIndustry"] = STChild;
                    reportParameters["WTSubIndustry"] = WTChild;

                    ///COntent
                    ///
                    reportParameters["ReportContext"] = reportContext;
                    ///

                    string username = string.Empty;
                    UserProfile user = UserProfileService.GetById(CurrentUser);
                    if (user != null)
                    {
                        username = user.Name;
                    }
                    reportParameters["UserId"] = username;
                    string clientcompanyname = string.Empty;
                    ClientCompany clientcompany = ClientCompanyService.GetById(CurrentCompany);
                    if (clientcompany != null)
                    {
                        clientcompanyname = clientcompany.Name;
                    }
                    reportParameters["ClientCompany"] = clientcompanyname;
                    reportParameters["ReportTitle"] = "SWOT";

                    IList<SwotAllView> swotList = new List<SwotAllView>();
                    swotList.Add(swotAll);
                    if (TDocument.Equals("pdf"))
                    {
                        reportParameters["DataSource"] = (System.Collections.IList)swotList;
                        //reportParameters["DataSource2"] = (System.Collections.IList)objectList;
                        //string reportFile = ReportHelper.PrintDocumentReport("SwotAll", reportParameters);
                        ////string reportFile = ReportHelper.PrintDocumentReport2("SwotAll", "SwotObject", reportParameters);
                        reportParameters["DataSource2"] = (System.Collections.IList)objectList;

                        string reportFile = Compelligence.ReportsV12.Helpers.ReportHelper.PrintDocumentReport2("SwotAll", "SwotObject", reportParameters, TDocument);
                        string path = ConfigurationSettings.AppSettings["ReportFilePath"].Substring(1);
                        string fileToGenerate = "Report" + Compelligence.Common.Utility.UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                        GetDownloadFileResponse2(path, reportFile, "Swot.pdf");
                    }
                    else if (TDocument.Equals("word"))
                    {
                        reportParameters["DataSource"] = (System.Collections.IList)swotList;
                        //reportParameters["DataSource2"] = (System.Collections.IList)objectList;
                        //string reportFile = ReportHelper.PrintDocumentReport("SwotAll", reportParameters);
                        ////string reportFile = ReportHelper.PrintDocumentReport2("SwotAll", "SwotObject", reportParameters);
                        reportParameters["DataSource2"] = (System.Collections.IList)objectList;

                        string reportFile = Compelligence.ReportsV12.Helpers.ReportHelper.PrintDocumentReport2("SwotAll", "SwotObject", reportParameters, TDocument);
                        string path = ConfigurationSettings.AppSettings["ReportFilePath"].Substring(1);
                        string fileToGenerate = "Report" + Compelligence.Common.Utility.UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                        GetDownloadFileResponseWord2(path, reportFile, "Swot.doc");
                    }
                    else if (TDocument.Equals("xls"))
                    {
                        reportParameters["DataSource"] = (System.Collections.IList)swotList;
                        //reportParameters["DataSource2"] = (System.Collections.IList)objectList;
                        //string reportFile = ReportHelper.PrintDocumentReport("SwotAll", reportParameters);
                        ////string reportFile = ReportHelper.PrintDocumentReport2("SwotAll", "SwotObject", reportParameters);
                        reportParameters["DataSource2"] = (System.Collections.IList)objectList;

                        string reportFile = Compelligence.ReportsV12.Helpers.ReportHelper.PrintDocumentReport2("SwotAll", "SwotObject", reportParameters, TDocument);
                        string path = ConfigurationSettings.AppSettings["ReportFilePath"].Substring(1);
                        string fileToGenerate = "Report" + Compelligence.Common.Utility.UniqueKeyGenerator.GetInstance().GetUniqueKey().ToString();
                        GetDownloadFileResponseExcel2(path, reportFile, "Swot.xls");
                    }
                }
            }
            return null;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult GetComplexCompetitors(string ids)
        {
            string result = string.Empty;
            int l = 0;
            IList<Competitor> lstCompetitors = new List<Competitor>();

            decimal[] idIndustries ;
            if (!Validator.IsBlankOrNull(ids))
            {
                idIndustries = FormatUtility.GetDecimalArrayFromStringValues(ids.Split(','));
                lstCompetitors = CompetitorService.GetByIndustry(idIndustries);
            }
            else
            {
                lstCompetitors = CompetitorService.GetAllSortByClientCompany("Name", CurrentCompany);
            }
           
            foreach (Competitor comp in lstCompetitors)
            {
                l++;
                if (lstCompetitors.Count == l)
                    result = result + comp.Id + ":" + comp.Name;
                else
                    result = result + comp.Id + ":" + comp.Name + "_";
            }
            return Content(result);
        }
        public ContentResult GetComplexIndustry(string ids)
        {
            string result = string.Empty;
            int l = 0;
            decimal idCompetitor = 0;
            IList<IndustryByHierarchyView> lstIndustrys = new List<IndustryByHierarchyView>();
            if (!Validator.IsBlankOrNull(ids))
            {
                idCompetitor = Convert.ToDecimal(ids);
                lstIndustrys = IndustryService.GetIndustryHierarchyByCompetitor(idCompetitor, CurrentCompany);
                IndustryService.GetByCompetitorId(idCompetitor, CurrentCompany);
            }
            else
            {
                lstIndustrys = IndustryService.FindIndustryHierarchy(CurrentCompany);
            }
           
            foreach (IndustryByHierarchyView ind in lstIndustrys)
            {
                l++;
                if (lstIndustrys.Count == l)
                    result = result + ind.Id + ":" + ind.Name;
                else
                    result = result + ind.Id + ":" + ind.Name + "_";
            }
            return Content(result);
        }

        public ActionResult SelectAllValues()
        {
            int countInd = 0, countComp = 0;
            string resultIndustries = string.Empty, resultCompetitors = string.Empty;
            IList<IndustryByHierarchyView> industrylist = IndustryService.FindIndustryHierarchy(CurrentCompany);
            foreach (IndustryByHierarchyView ind in industrylist)
            {
                countInd++;
                if (industrylist.Count == countInd)
                    resultIndustries = resultIndustries + ind.Id + ":" + ind.Name;
                else
                    resultIndustries = resultIndustries + ind.Id + ":" + ind.Name + "_";
            }
            IList<Competitor> competitorList = CompetitorService.GetAllSortByClientCompany("Name", CurrentCompany);
            foreach (Competitor comp in competitorList)
            {
                countComp++;
                if (competitorList.Count == countComp)
                    resultCompetitors = resultCompetitors + comp.Id + ":" + comp.Name;
                else
                    resultCompetitors = resultCompetitors + comp.Id + ":" + comp.Name + "_";
            }
            return Json(new { ListIndustry = resultIndustries, ListCompetitor = resultCompetitors });
        }

        protected void GetDownloadFileResponse2(string path, string physicalName, string fileName)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ContentType = Compelligence.Util.Common.FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.TransmitFile("~\\" + path + physicalName + ".pdf");
            Response.End();
        }
        protected void GetDownloadFileResponseExcel2(string path, string physicalName, string fileName)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ContentType = Compelligence.Util.Common.FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.TransmitFile("~\\" + path + physicalName + ".xls");
            Response.End();
        }
        protected void GetDownloadFileResponseWord2(string path, string physicalName, string fileName)
        {
            Response.ContentType = Compelligence.Util.Common.FileUtility.GetMimeType("~\\" + path + physicalName);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName.Replace(' ', '_'));
            Response.Clear();
            Response.WriteFile("~\\" + path + physicalName + ".doc");
            Response.End();
        }
        private bool GetValueBoolOfString(string value)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Equals("True") || value.Equals("true") || value.Equals("TRUE"))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
