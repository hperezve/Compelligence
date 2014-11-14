<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndBlockingPopupSite.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="popupHead" ContentPlaceHolderID="Head" runat="server">
    <title>Compelligence - Backend</title>
</asp:Content>
<asp:Content ID="popupStyles" ContentPlaceHolderID="Styles" runat="server">
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/jquery.searchFilter.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.jqgrid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.multiselect.css") %>" rel="stylesheet"  type="text/css" />    
</asp:Content>
<asp:Content ID="popupScripts" ContentPlaceHolderID="Scripts" runat="server">
    <script src="<%= Url.Content("~/Scripts/jquery-1.3.2.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.locale-en-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.base-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.celledit-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.common-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.custom-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.formedit-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.import-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.inlinedit-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.jqueryui.js") %>" type="text/javascript"></script>      
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.addons.js") %>" type="text/javascript"></script>          
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.postext.js") %>" type="text/javascript"></script>        
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.setcolumns.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.subgrid-min.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.tbltogrid-min.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.treegrid-min.js") %>" type="text/javascript"></script>    
    <script src="<%= Url.Content("~/Scripts/jqgrid/jqDnR.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jqModal.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.fmatter-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.searchFilter.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/JsonXml-min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jqgrid/ui.multiselect.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Utility.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/DataGrid.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>  
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

        var returnValues = function() {

            if (existsWindowOpener) {

                var browseName = '<%= ViewData["BrowseName"] %>';
                var browseEvents = '<%= ViewData["SectionEvent"] %>';
                var browseNews = '<%= ViewData["SectionNews"] %>';

                var gridId = '#' + browseName + 'ListTable';
                var gridIdEvents = '#' + browseEvents + 'ListTable';
                var gridIdNews = '#' + browseNews + 'ListTable';
                var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
                var multiselect2 = getBooleanValue($(gridIdEvents).getGridParam('multiselect'));
                var multiselect3 = getBooleanValue($(gridIdNews).getGridParam('multiselect'));

                var selectedItems;

                if (multiselect) {
                    selectedItems = $(gridId).getGridParam("selarrrow");
                    selectedItems = selectedItems.join(':');
                    selectedItems2 = $(gridIdNews).getGridParam("selarrrow");
                    selectedItems2 = selectedItems2.join(':');
                    selectedItems3 = $(gridIdEvents).getGridParam("selarrrow");
                    selectedItems3 = selectedItems3.join(':');

                }

                else {
                    selectedItems = $(gridId).getGridParam('selrow');
                    //alert
                    alert(selectedItems + " selrow");
                }

                var funcional = selectedItems.split(":")
                var funcional2 = selectedItems2.split(":")
                var funcional3 = selectedItems3.split(":")

                var gRow = $(gridId).jqGrid('getRowData', $(gridId).getGridParam('selrow'));
                var entityType = 'Project';
                window.opener.SetId(selectedItems, 'LibrariesId', entityType);


                var gRowNews = $(gridIdNews).jqGrid('getRowData', $(gridIdNews).getGridParam('selrow'));
                var entityTypeNews = 'News';
                window.opener.SetId(selectedItems, 'LibrariesId', entityTypeNews);


                var gRowEvents = $(gridIdEvents).jqGrid('getRowData', $(gridIdEvents).getGridParam('selrow'));
                var entityTypeEvents = 'Events';
                window.opener.SetId(selectedItems, 'LibrariesId', entityTypeEvents);

                if (checkNullString(selectedItems) == '' && checkNullString(selectedItems2) == '' && checkNullString(selectedItems3) == '') {
                    showAlertSelectItemDialog();
                    return;
                }
                if (window.opener.loadSelectedFunction == undefined) {
                    window.opener.loadSelectedPopupItems(selectedItems);
                    //alert
                    //alert(window.opener.loadSelectedFunction + " undefined0");
                }
                else //alter reference to new function
                {
                    if (funcional != "") {
                        $.each(funcional, function(key, value) {
                        var param = { id: value, type: 'Project', Description: FuncionaCk };
                            var trigger = "window.opener." + window.opener.loadSelectedFunction + "(param);";
                            eval(trigger);
                        });
                    }

                    if (funcional2 != "") {
                        $.each(funcional2, function(key, value) {
                        var param2 = { id: value, type: 'News', Description: FuncionaCk };
                            var trigger = "window.opener." + window.opener.loadSelectedFunction + "(param2);";
                            eval(trigger);
                        });
                    }

                    if (funcional3 != "") {
                        $.each(funcional3, function(key, value) {
                        var param3 = { id: value, type: 'Events', Description: FuncionaCk };                            
                            var trigger = "window.opener." + window.opener.loadSelectedFunction + "(param3);";
                            eval(trigger);
                        });
                    }

                    window.opener.loadSelectedFunction = null;
                }

            }

            closeBrowsePopup();
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
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
    $('#' + '<%=ViewData["SectionNews"]%>' + 'ListTable').jqGrid('setGridWidth', Math.round($(window).width() * 0.98) + '');
    $('#' + '<%=ViewData["BrowseName"]%>' + 'ListTable').jqGrid('setGridWidth', Math.round($(window).width() * 0.98) + '');
    $('#' + '<%=ViewData["SectionEvent"]%>' + 'ListTable').jqGrid('setGridWidth', Math.round($(window).width() * 0.98) + '');
    }).trigger('resize');
</script>
<style type ="text/css">
body
{
    background-color:White;
}
</style>
</asp:Content>
<asp:Content ID="popupContent" ContentPlaceHolderID="MainContent" runat="server">
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
    <div class="ActionButton">
         <%= Html.GridSearchOptions((string) ViewData["BrowseName"])%>                        
    </div>
</asp:Content>
