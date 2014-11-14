<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Compelligence.Domain.Entity.NewsLetter>" %>

    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/jquery.searchFilter.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.jqgrid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.multiselect.css") %>" rel="stylesheet"  type="text/css" />    
    <style type="text/css">
.SearchContainer
{
    width: 350px;
    float:left;
}
.ActionButton
{
    float:left;
}

ul.tabs {
	margin: 0;
	padding: 0;
	float: left;
	list-style: none;
	border-bottom: 1px solid #999;
	width: 100%;
}
ul.tabs li {
	margin: 0;
	margin-bottom: -1px;
	background: #55555;
	border: 1px solid #999999;
    float: left;
    height: 31px;
    line-height: 31px;
    margin-right: 5px;
    overflow: hidden;
    padding: 0;
    position: relative;	
    -moz-border-radius: 5px 5px 0 0;
	-webkit-border-radius: 5px 5px 0 0;
}

ul.tabs li a {
	text-decoration: none;
	color: #000;
	font-size: 1.2em;
	padding: 0 20px;
	outline: none;
}

ul.tabs li a:hover {
	background-color: #ccc;
}	

html ul.tabs li.active, html ul.tabs li.active a:hover  {
	background: #fff;
	border-bottom: 1px solid #fff;
	-moz-border-radius: 5px 5px 0 0;
	-webkit-border-radius: 5px 5px 0 0;
}

.tab_container {
	border: 1px solid #999;
	border-top: none;
	float: left; 
}

.tabsearch_content
{
    
}

</style>

    <script type="text/javascript">
        var CkValue = 'No'; 
        var FuncionaCk ='';      
    
        var filterOptionByNews = function(invoke, scope, entity, entityall) {
            var componentId = scope + entity;
            var formId = '#' + componentId + 'FilterForm';
            var gridId = '#' + componentId + 'ListTable';
            var dialogId = '#' + componentId + 'FilterDialog';
            var currentUrl = $(gridId).getGridParam("url");
            var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
            var query = '&filterCriteria=';
            var input = '&filterCriteria=';
            if (invoke.checked) {
                input += entityall + '.AddToNewsletter_Eq_Y';
                $(gridId).setGridParam({ url: urlAction + input, page: 1 }).trigger('reloadGrid');
            }
            else {
                $(gridId).setGridParam({ url: urlAction + input, page: 1 }).trigger('reloadGrid');
            }
        };

        var EventsDescrition = function(invoke, Ck) {

            if (invoke.checked) {
                CkValue = Ck + ':';
                FuncionaCk += CkValue;                      
            }            
        };

        function UpdateTabs(tab) {
            $(".tab_content").hide();
            $(".tabsearch_content").hide();
            $("#tab" + tab).show();
            $("#tab" + tab + "Search").show();
        }

        $(document).ready(function() {
        $(".tab_content").hide();
        $(".tabsearch_content").hide();
        $(".tab_content:first").show();
        $(".tabsearch_content:first").show();
        });
    </script>      
<style type ="text/css">
body
{
    background-color:White;
}
</style>

<div style="width:700; height:300;">
    <div id="AlertSelectItemDialog" title="Alert Message">            
        <p>
            <span class="ui-icon ui-icon-alert alertSelectItemDialog"></span>You should select
            a valid item from list.
        </p>
    </div>
    <div id="SearchContainer">
        <div id="tab1Search" class="tabsearch_content">
            <div class="SearchContainer">
                <%= Html.GridSearch((string)ViewData["SectionNews"], true)%>
            </div>
            <div style="float:left; width:300px;">
                <%= Html.CheckBox("OnlyNews", new { id = "OnlyNews", style="margin-top:15px;", onclick = "filterOptionByNews(this,'" + ViewData["Scope"] + "', 'NewsLetterSectionNews', 'NewsLetterSectionNewsView');" })%>
                <label for="OnlyNews" style="font-size: 1.1em;">Show Only Tagged News</label>
            </div>
        </div>
        <div id ="tab2Search" class="tabsearch_content">
            <div class="SearchContainer">
                <%= Html.GridSearch((string)ViewData["BrowseName"], true)%>
            </div>
        </div>  
        <div id="tab3Search" class="tabsearch_content">
            <div class="SearchContainer">
                <%= Html.GridSearch((string)ViewData["SectionEvent"], true)%>         
            </div>
        </div>
    </div>
<%= ViewData["BrowseName"] %>
       <%= ViewData["SectionEvent"] %>
       <%= ViewData["SectionNews"] %>
    <ul class="tabs">     
        <li onclick="javascript: UpdateTabs(1);"><a>News</a></li>           
        <li onclick="javascript: UpdateTabs(2);"><a>Projects</a></li>    
        <li onclick="javascript: UpdateTabs(3);"><a>Events</a></li>
    </ul>
    <div class="tab_container" >
        <div id="tab1" class="tab_content" >      
        <%= Html.DataGrid((string)ViewData["SectionNews"])%>
        </div>
        <div id="tab2" class="tab_content" >      
        <%= Html.DataGrid((string)ViewData["BrowseName"])%>   
        </div>
        <div id="tab3" class="tab_content" >                    
        <%= Html.DataGrid((string)ViewData["SectionEvent"])%>     
        </div>       
    </div> 
    </div>

