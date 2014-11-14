<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.DataTransfer.Comparinator" %>
<% string sendDetailFilter = ViewData["DetailFilter"].ToString(); ; %>
<head>

	<meta http-equiv="content-type" content="text/html; charset=iso-8859-1"/>
	<title>jQuery treeview</title>
    <link href="<%= Url.Content("~/Content/Styles/jquery.treeview.css") %>" rel="stylesheet"  type="text/css" />
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
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jquery.treeview.min.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/BackEnd/Functions.js") %>" type="text/javascript"></script>

<script type="text/javascript">
    $(function() {
        $("#browser").treeview();
        $("#browser2").treeview();
    });
    function selectCriterias(source, target) {
        var chkCriterias = $(source).parent().children('#' + target).find("li :checkbox");

        //alert(chkCriterias.size());
        if (source.checked)
            chkCriterias.prop('checked', 'checked');
        else
            chkCriterias.prop('checked', '');

    }
    function selectSets(source, target) {
        var chkCriterias = $(source).parent().children('#' + target).find("li :checkbox");

        //alert(chkCriterias.size());
        if (source.checked)
            chkCriterias.prop('checked', 'checked');
        else
            chkCriterias.prop('checked', '');

    }

    function saveCriteria(urlForm) {
        var IndustryId = '<%= ViewData["IndustryId"]%>';
        var TargetIndustryId = '<%= ViewData["TargetIndustryId"]%>';
        var criteriaId;
         var count = 1;
        $("input[name='CriteriaId']").each(function() {
            if ($(this).is(':checked')) {
                if (count == 1) {
                    criteriaId = $(this).val() + ',';
                } else {
                    criteriaId += $(this).val() + ',';
                }
                count+=1;
            }
        });
        criteriaId = criteriaId.substring(0, criteriaId.length - 1);
        $.post(urlForm, { ids: criteriaId, industryIds: IndustryId, targetIndustryIds: TargetIndustryId }, function(data) {
            var evs = data;
            if (evs == "true") {
                window.opener.loadReloadGrid("#EnvironmentIndustryCriteriaIndustryDetailListTable");
                window.self.close();
            }            

        });
    }
</script>
	
</head>
<body>

<h1 id="banner">Select Criterias(Step two)</h1>

<div id="main">
    <ul id="browser2" class="filetree">
    <%foreach (var oRow in (IEnumerable<ComparinatorGroup>)ViewData["ComparinatorGroups"])
      {%>
		<li class="closed"><input type="checkbox" value="0" onclick="selectSets(this,'group<%=oRow.Id%>')"/><%=oRow.Name%>
	    <ul id="group<%=oRow.Id%>">
        <% foreach (var oRowTwo in oRow.ComparinatorSets)
        {%>
       	 <li class="closed"><input type="checkbox" value="0" onclick="selectCriterias(this,'set<%=oRowTwo.Id%>')" /><%=oRowTwo.Name%> 
       	 <ul id="set<%=oRowTwo.Id%>">
		 <% foreach (var oRowThree in oRowTwo.ComparinatorCriterias)
          {%>
            <li><input type="checkbox" name="CriteriaId" value="<%=oRowThree.Criteria.Id%>" /><%=oRowThree.Criteria.Name%></li>
         <%}%>
         </ul>
         </li>
         <%}%>
        </ul>
        </li>
    <%} %>            
    </ul>   
    <input type="hidden" name="IndustryId" value="<%=ViewData["IndustryId"]%>" />
    <input type="hidden" name="TargetIndustryId" value="<%=ViewData["TargetIndustryId"]%>" />
    <input class="button" type="button" value="Previous" onclick="location='<%=Url.Action("ReturnCustomCriteriaStep1","Industry",new { detailFilter = sendDetailFilter}) %>';"/>
    <input class="button" type="submit" value="Finish" onclick="saveCriteria('<%=Url.Action("multiAddCriteria","Industry") %>');" />
</div>

</body>