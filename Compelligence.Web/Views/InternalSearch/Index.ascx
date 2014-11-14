<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    jQuery(window).bind('resize', function() {
        resizeGrid('<%= ViewData["Scope"]%>' + 'ProjectAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'DealAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'EventAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'SurveyAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CalendarAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'ProductAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'ObjectiveAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'kitAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'IndustryAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CompetitorAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'CustomerAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'LibraryAllSearch');
        //resizeGrid('<%= ViewData["Scope"]%>' + 'WinLossAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'SourceAllSearch');
        resizeGrid('<%= ViewData["Scope"]%>' + 'LibraryNewsAllSerarch');
    }).trigger('resize');
</script>
<script type="text/javascript">
    var executeInternalSearch = function() {
        var searchQuery = $('#SearchQuery').val();

        showLoadingDialogForSection('#InternalSearchSection');

        $('#SearchResults').load('<%= Url.Action("Search", "InternalSearch") %>', { SearchQuery: searchQuery }, function() { hideLoadingDialogForSection('#InternalSearchSection'); });
    };
    

</script>

<div id="InternalSearchSection">
<input class="button" type="button" value="Help" onclick="javascript: SetToShowHelpWithTitle('<%= ViewData["Scope"] %>','<%= Compelligence.Domain.Entity.Resource.Pages.Search %>','show','<%= Url.Action("GetHelp","Help") %>','<%= Url.Action("Update","Help") %>','<%= ViewData["EditHelp"] %>', '<%= ViewData["ActionFrom"] %>','Search');" style="float: right;margin-right: 5px;margin-top:5px"/>

    <div id="InternalSearchFormSection" style="height:70px">
        <form id="InternalSearchForm" action="" onsubmit=" javascript: executeInternalSearch(); return false;"
        method="post">
        <fieldset>
            <legend>Search</legend>            
            <div class="line" style="margin-left:10px;">
                <%= Html.TextBox("SearchQuery")%>
                <input class="button" type="submit" value="Search" style="vertical-align:top;" />
            </div>
        </fieldset>
        </form>
    </div>
    <div id="SearchResults" class="tableSearchresult" tyle="margin-left:10px">
    </div>
</div>
