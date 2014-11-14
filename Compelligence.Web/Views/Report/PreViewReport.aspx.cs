using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Compelligence.Common.Browse;
using System.Collections;
using Compelligence.BusinessLogic.Implementation;
using Compelligence.Common.Utility.Parser;
using Compelligence.Common.Cache;
using Compelligence.Web.Controllers;
using Compelligence.BusinessLogic.Interface;
using Compelligence.Domain.Entity;
using Compelligence.Security.Managers;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Configuration;
using Compelligence.Util.Type;
namespace Compelligence.Web.Views.Report
{
    public partial class PreViewReport : System.Web.UI.Page
    {
        public IReportService ReportService {get; set;}
        private static string reportPath = ConfigurationSettings.AppSettings["ReportFilePath"];
        private string binReportPath = ConfigurationSettings.AppSettings["BinReportsFilePath"];
        private static string reportContext = AppDomain.CurrentDomain.BaseDirectory;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateReport();
                form1.Action = Request.RawUrl;
            }
        }

        private void CreateReport()
        {
                       
            IDictionary<string, Object> reportParameters = new Dictionary<string, Object>();
            IDictionary<string, string> columnDictionary = new Dictionary<string, string>();
            string BrowseId = HttpUtility.HtmlEncode(StringUtility.CheckNull(Request.Params["BrowseId"]));
            if (BrowseId.IndexOf(",") != -1)
            {
                string[] brwId = BrowseId.Split(',');
                BrowseId = brwId[0];
            }
            string Userid = Request.Params["UserId"];
            ReportViewer reportViewer = (ReportViewer)Session["ReportViewer"];
            IList dataSource = (IList)Session["DataSourceObjects"];

            BindingSource bindingSource = new BindingSource();
            ReportDataSource reportDataSource = new ReportDataSource(BrowseId + "View", bindingSource);
            BrowseObject browseObject = (BrowseObject)Session["BrowseObject"];
            string HiddenColumnCriteria = Request.Params["HiddenColumnCriteria"];
            string urlPath = Request.Params["UrlPath"];
            reportParameters["BrowseObject"] = browseObject;
            reportParameters["DataSource"] = dataSource;
            reportParameters["UserId"] = Request.Params["UserId"];
            reportParameters["ClientCompany"] = Request.Params["ClientCompany"];
            reportParameters["ReportTitle"] = Request.Params["Title"];
            reportParameters["ReportFilter"] = Request.Params["ReportFilter"];
            reportParameters["ReportContext"] = urlPath;
            if (HiddenColumnCriteria.IndexOf(",") != -1)
            {
                string[] hdnColumnCriteria = HiddenColumnCriteria.Split(',');
                HiddenColumnCriteria = hdnColumnCriteria[0];
            }
            string[] filterHCCriteriaArray = HiddenColumnCriteria.Split(':');
            foreach (string hiddenColumn in filterHCCriteriaArray)
            {
                string[] operators = hiddenColumn.Split('_');
                string[] fields = operators[0].Split('.');
                if (IsNotKey(columnDictionary, fields[1].ToString() + "HiddenColumn"))
                {
                    if ((operators.Length == 3) && (operators[2].ToString().Equals("false")))
                    {
                        columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "true");
                        reportParameters[fields[1].ToString() + "HiddenColumn"] = "true";
                    }
                    else
                    {

                        columnDictionary.Add(fields[1].ToString() + "HiddenColumn", "false");
                        reportParameters[fields[1].ToString() + "HiddenColumn"] = "false";
                    }
                }
            }
            reportParameters["ColumnDictionary"] = columnDictionary;

            IList<ReportParameter> reportParameterList = new List<ReportParameter>();
            ReportParameterInfoCollection reportParameterInfoCollection = reportViewer.LocalReport.GetParameters();
            foreach (ReportParameterInfo reportParameterInfo in reportParameterInfoCollection)
            {
                if (reportParameters.ContainsKey(reportParameterInfo.Name))
                {
                    reportParameterList.Add(new ReportParameter(reportParameterInfo.Name, reportParameters[reportParameterInfo.Name].ToString()));
                }
                else
                {
                    reportParameterList.Add(new ReportParameter(reportParameterInfo.Name, ""));
                }
            }



            rptViewer.ID = BrowseId;

            if (reportContext.IndexOf("inetpub\\wwwroot") != -1)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reportContext + binReportPath + BrowseId + "View.rdlc");
                StringReader sr = new StringReader(doc.OuterXml);

                rptViewer.ProcessingMode = ProcessingMode.Local;
                rptViewer.LocalReport.DisplayName = Request.Params["Title"] + " " + DateTime.Now.ToString("MMM dd yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                rptViewer.LocalReport.DataSources.Add(reportDataSource);
                rptViewer.LocalReport.EnableExternalImages = true;
                rptViewer.LocalReport.ReportPath = reportContext + binReportPath + BrowseId + "View.rdlc";
                rptViewer.LocalReport.LoadReportDefinition(sr);
                rptViewer.ShowPrintButton = true;
                string aaa = rptViewer.AppRelativeTemplateSourceDirectory;

                rptViewer.LocalReport.SetParameters(reportParameterList);
                rptViewer.LocalReport.AddTrustedCodeModuleInCurrentAppDomain("Compelligence.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                rptViewer.LocalReport.ReportEmbeddedResource = reportContext + "bin\\Reports\\" + BrowseId + "View.rdlc";

                bindingSource.DataSource = dataSource;
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reportContext + binReportPath + BrowseId + "View.rdlc");
                //doc.Load("Compelligence.Reports\\Reports\\" + BrowseId + "View.rdlc");
                StringReader sr = new StringReader(doc.OuterXml);
                rptViewer.LocalReport.DisplayName = Request.Params["Title"] + " " + DateTime.Now.ToString("MMM dd yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                rptViewer.LocalReport.DataSources.Add(reportDataSource);
                rptViewer.LocalReport.LoadReportDefinition(sr);
                rptViewer.LocalReport.EnableExternalImages = true;
                rptViewer.LocalReport.SetParameters(reportParameterList);
                rptViewer.LocalReport.AddTrustedCodeModuleInCurrentAppDomain("Compelligence.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

                bindingSource.DataSource = dataSource;
            }
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
    }
}
