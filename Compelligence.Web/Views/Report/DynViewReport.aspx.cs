using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Compelligence.Reports.Dynamic;

namespace Compelligence.Web.Views.Report
{
    public partial class DynViewReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Rdlc hostreport = (Rdlc)Session["reportContainer"];
            string Title = Request.Params["Title"];
            DataSet customerData = (DataSet)Session["customerData"];
            DataTable customerDataTable = (DataTable)Session["customerDataTable"];
            if (!string.IsNullOrEmpty(Title))
            {
                ReportViewer1.LocalReport.DisplayName = Title + " " + DateTime.Now.ToString("MMM dd yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
            }
            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource(customerData.DataSetName, customerDataTable));
            ReportViewer1.LocalReport.LoadReportDefinition(hostreport.GetRdlcStream());
            //ReportViewer1..RefreshReport();

        }
    }
}
