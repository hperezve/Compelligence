<%@ Page Title="Compelligence - Content Search" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace='Compelligence.Domain.Entity.Views' %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="indexStyles" ContentPlaceHolderID="Styles" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"
        type="text/css" />--%>
        <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
        <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
        
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    var loadUrl = function(indice) {
    var idUrl = '#urlNews' + indice;
    var url = $(idUrl).text();
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "Url", "scrollbars=1,width=900,height=500")
        }
    };

    function DealEdit(id) {
        var urlAction = '<%=Url.Action("Edit","DealSupport") %>';
        if (id == null)
            alert('Select row');
        else
            location.href = urlAction + "/" + id;
    }
    function EventEdit(id) {
        var urlAction = '<%=Url.Action("Edit","Events") %>';
        if (id == null)
            alert('Please select a row');
        else
            location.href = urlAction + "/" + id;
    }
</script>
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <div class="fieldSearch">
        <div class="headerContentRightMenu">
            Content Search</div>
        <div class="contentRightMenu">
            <div>
                <img src="<%= Url.Content("~/content/Images/Icons/searchPage.png") %>" />
            </div>
            <div class="textboxSearch">
                <% using (Html.BeginForm("Index", "ContentSearch", FormMethod.Get))
                   { %>
                <%= Html.TextBox("q", (string)ViewData["SearchWord"])%>
                <input class="shortButton" type="submit" id="Submit1" value="Search" />
                <% } %>
            </div>
            <% if (Convert.ToInt32(ViewData["CountFoundResults"]) > 0)
               {  %>
            <div class="textResultSearch">
                Results <span class="textResultItalic">
                    <%= ViewData["FoundResultStart"] %>
                    -
                    <%= ViewData["FoundResultEnd"] %>
                </span>of about <span class="textResultItalic">
                    <%= ViewData["CountFoundResults"] %></span> for <span class="textResultItalic">
                        <%= ViewData["SearchWord"] %>
                    </span>
            </div>
            <div class="tblOne backAlice">
                <% IList<ProjectFileShowView> results = (List<ProjectFileShowView>)ViewData["SearchProjects"];
                   int cont= 0;
                   decimal idEntity;
                   foreach (ProjectFileShowView item in results)
                   {
                       cont = cont + 1;
                       idEntity = item.Id; %>
                       
                   <%=Html.ItemToSearch(item)%>
                   <%} %>
            </div>
            <div class="marginTop10" style="height: 50px;">
                <% if (Convert.ToInt32(ViewData["NumPages"]) > 1)
                   { %>
                <div id="enumSearchContent" class="tblOne">
                    <% int currentPage = Convert.ToInt32(ViewData["CurrentPage"]);
                       int maxByPage = Convert.ToInt32(ViewData["MaxByPage"]);
                       int numPages = Convert.ToInt32(ViewData["NumPages"]);
                       
                       if (currentPage > 1)
                      {
                    %>
                      <a href="<%= Url.Action("Index", "ContentSearch", new { q = ViewData["SearchWord"], stidx = (Convert.ToInt32(ViewData["FoundResultStart"]) - (maxByPage + 1)) }) %>"><%= Html.Encode("<<") %></a>  
                    <%}
                      int startIndex = 0;
                      
                       for (int page = 1; page <= numPages; page++)
                      { 
                    %>
                   
                    <a href="<%= Url.Action("Index", "ContentSearch", new { q = ViewData["SearchWord"], stidx = startIndex }) %>" class="<%= (page == currentPage) ? "currentPage" : string.Empty %>"><%= page %></a>
                    <% startIndex += maxByPage;
                      }
                      if (currentPage < numPages)
                      {%>
                    <a href="<%= Url.Action("Index", "ContentSearch", new { q = ViewData["SearchWord"], stidx = (Convert.ToInt32(ViewData["FoundResultStart"]) + maxByPage - 1) }) %>"><%= Html.Encode(">>") %></a>  
                    <%}%>
                </div>
                <% } %>
                <div id="searchBarFooter">
                    <% using (Html.BeginForm("Index", "ContentSearch", FormMethod.Get))
                       { %>
                    <%= Html.TextBox("q", (string)ViewData["SearchWord"])%>
                    <input class="shortButton" type="submit" id="buttonSearchProject" value="Search" />
                    <% } %>
                </div>
            </div>
            <% } %>
    </div>
    </div>
</asp:Content>


