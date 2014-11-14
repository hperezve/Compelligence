<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreViewReport.aspx.cs" Inherits="Compelligence.Web.Views.Report.PreViewReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<script language="javascript" type="text/javascript">
    ResizeReport();

    function ResizeReport() {
        var viewer = document.getElementById("<%= rptViewer.ClientID %>");
        var htmlheight = document.documentElement.clientHeight;
        //viewer.style.height = (htmlheight - 30) + "px";
        viewer.style.width = "100%";
        viewer.style.height = "100%";
    }

    window.onresize = function resize() { ResizeReport(); }
</script>

<head runat="server">
    <title>Report</title>
</head>
<body>
    
    <form id="form1" method="post" runat="server">
    <div style="Width:auto;height:auto;">
        <rsweb:ReportViewer ID="rptViewer" runat="server" Width="100%" Height="575px">
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
