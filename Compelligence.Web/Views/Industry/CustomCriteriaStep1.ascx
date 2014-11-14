<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% string sendDetailFilter = ViewData["DetailFilter"].ToString();%>
<head>

	<meta http-equiv="content-type" content="text/html; charset=iso-8859-1"/>
	<title>jQuery treeview</title>
    <link href="<%= Url.Content("~/Content/Styles/jquery.treeview.css") %>" rel="stylesheet"  type="text/css" />
    <script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.treeview.min.js") %>" type="text/javascript"></script>

	
	<script type="text/javascript">
	    $(function() {
	        $("#browser").treeview();
	        $("#browser2").treeview();
	    });
	</script>
	<style type="text/css">
	#banner {
    background: url(<%= ResolveUrl("~/Content/Images/Styles/popup-header-bg.gif") %>) repeat-x scroll 0 0 transparent;
    border-bottom: 1px solid #CCCCCC;
    color: white;
    font-size: large;
    padding: 15px;
    text-align: center;
}
</style>
<script type="text/javascript">
    function nullValue() {

        var valor = $("#IndustryIdSelect option:selected").html();

        if (valor == '' || valor == null || valor == 'undefined') {
            window.self.close();
        }
    }
</script>
</head>
<body>

<%IList<Industry> industries = (IList<Industry>)ViewData["Industries"]; %>
<h1 id="banner">Select Industry(Step One)</h1>
<form action="<%=Url.Action("CustomCriteriaStep2","Industry") %>" method="get">
  <select id="IndustryIdSelect" name="IndustryId" size="10">
     <%foreach (Industry industry in industries)
       { %>
         <option value="<%=industry.Id%>"><%=industry.Name%></option>
     <%} %>
  </select>
<input type="hidden" name="DetailFilter" value="<%=ViewData["DetailFilter"]%>" />  
<input type="hidden" name="TargetIndustryId" value="<%=ViewData["TargetIndustryId"]%>" />
<input class="button" type="submit" value="Next" onclick="nullValue();"/>
<input class="button" type="button" value="Close" onclick="window.self.close();"/>
</form>
</body>