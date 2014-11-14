<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity" %>
<%--<input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'EventReports','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>,'<%= Compelligence.Domain.Entity.Resource.Pages.EventReports %>'');" style="float: right;margin-right: 5px;margin-top:5px"/>--%>
<input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Reports','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Reports %>');" style="float: right;margin-right: 5px;margin-top:5px"/>

<%
    IList<Report> reportList = (IList<Report>)ViewData["ReportList"];
    foreach (Report report in reportList)
    {
        if (report.Id != null)
        {
%>
   
    <div id="<%= report.Id.Filter %>Link" class="itemListGroupReport">
    <img src="<%= Url.Content("~/content/Images/Icons/SurveySmallIcon.png") %>"/>
        <a href="javascript: void(0);" onclick= "javascript: loadFilterReport('<%= report.Id.Filter %>', '<%= report.Title %>', '<%= ViewData["ReportModule"] %>');">
            <%= report.Title%>
        </a>
    </div>
<%
    }
        else
        {%> 
        <div id="<%= report.Title%>Group" class="itemListTitleReport">
            <label style="font-size:1.2em;">
                   <%= report.Title%>:</label>
        </div>
        <%}
    }
%>