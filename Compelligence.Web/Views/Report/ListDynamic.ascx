<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%--<input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'DynamicReports','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.DynamicReports %>');" style="float: right;margin-right: 5px;margin-top:5px"/>--%>
<input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Reports','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Reports %>');" style="float: right;margin-right: 5px;margin-top:5px"/>

    <div class="itemListTitleReport">
        <label style="font-size:1.2em;"> Dynamic:</label>        
    </div>
    <div class="itemListGroupReport" style="width: 550px;">
        <img src="<%= Url.Content("~/content/Images/Icons/SurveySmallIcon.png") %>" />
        <a href="javascript: void(0);" onclick="javascript: loadFilterReportDynamic('Relation between Industry and Competitor Report','<%= Compelligence.Domain.Entity.Resource.DynamicReportKey.IndustryAndCompetitor %>','DYNAMIC','Industry','Competitor');">
            Relation between Industry and Competitor Report </a>
    </div>
    <div class="itemListGroupReport" style="width: 550px;">
        <img src="<%= Url.Content("~/content/Images/Icons/SurveySmallIcon.png") %>" />
        <a href="javascript: void(0);" onclick="javascript: loadFilterReportDynamic('Relation between Industry and Competitor Report with Product','<%= Compelligence.Domain.Entity.Resource.DynamicReportKey.IndustryAndCompetitorProduct %>','DYNAMIC','Industry','Competitor');">
            Relation between Industry and Competitor Report with Product </a>
    </div>
    <div class="itemListGroupReport" style="width: 550px;">
        <img src="<%= Url.Content("~/content/Images/Icons/SurveySmallIcon.png") %>" />
        <a href="javascript: void(0);" onclick="javascript: loadFilterDynamicFinancial('Financial Performance per Competitor','<%= Compelligence.Domain.Entity.Resource.DynamicReportKey.FinancialPerformanceCompetitor %>','DYNAMIC','Competitor');">
            Financial Performance per Competitor </a>
    </div>
