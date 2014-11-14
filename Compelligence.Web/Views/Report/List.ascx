<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import namespace="Compelligence.Domain.Entity" %>
<div>
  <input class="button" type="button" value="Help" onclick="javascript: SetValuesToShowHelp('<%= ViewData["Scope"] %>', 'Reports','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Reports %>');" style="float: right;margin-right: 5px;margin-top:5px"/>
</div>

   <% IList<ReportGroupList> reportGList = (IList<ReportGroupList>)ViewData["ReportGroupList"];
   for (int r = 0; r < reportGList.Count; r++)
   {
       if (r < reportGList.Count)
       {
    %>
    <div class="line">
        <%if (r < reportGList.Count)
          { %>
        <div class="fieldColumn">
            <div class="itemListTitleReport">
            <label style="font-size:1.2em;">
                   <%= reportGList[r].HeaderGroup%>:</label>
            </div>
            <% foreach (Report report1 in reportGList[r].ReportList)
               { %>
                    <div class="itemListGroupReport"><img src="<%= Url.Content("~/content/Images/Icons/SurveySmallIcon.png") %>"/>
                    <a href="javascript: void(0);" onclick= "javascript: loadFilterReport('<%= report1.Id.Filter %>', '<%= report1.Title %>', '<%= ViewData["ReportModule"] %>');">
                    <%= report1.Title%>
                    </a>
                    </div>
            <%} r++; %>
        </div>
        <% } %>
        
        <%if (r < reportGList.Count)
          { %>
        <div class="fieldColumn">
            <div class="itemListTitleReport">
            <label style="font-size:1.2em;">
                   <%= reportGList[r].HeaderGroup%>:</label>
            </div>
            <% foreach (Report report2 in reportGList[r].ReportList)
               { %>
                    <div class="itemListGroupReport"><img src="<%= Url.Content("~/content/Images/Icons/SurveySmallIcon.png") %>"/>
                    <a href="javascript: void(0);" onclick= "javascript: loadFilterReport('<%= report2.Id.Filter %>', '<%= report2.Title %>', '<%= ViewData["ReportModule"] %>');">
                    <%= report2.Title%>
                    </a>
                    </div>
            <%} r++; %>
        </div>
        <% } %>
        
        <%if (r < reportGList.Count)
          { %>
        <div class="fieldColumn">
            <div class="itemListTitleReport">
            <label style="font-size:1.2em;">
                   <%= reportGList[r].HeaderGroup%>:</label>
            </div>
            <% foreach (Report report3 in reportGList[r].ReportList)
               { %>
                    <div class="itemListGroupReport"><img src="<%= Url.Content("~/content/Images/Icons/SurveySmallIcon.png") %>"/>
                    <a href="javascript: void(0);" onclick= "javascript: loadFilterReport('<%= report3.Id.Filter %>', '<%= report3.Title %>', '<%= ViewData["ReportModule"] %>');">
                    <%= report3.Title%>
                    </a>
                    </div>
            <%}  %>
        </div>
        <% } %>
    </div>
    <%}
   } %>