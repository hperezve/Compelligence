<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    var SendEmail = function(browseid) {
        var urlAction = '<%= Url.Action("GetParametersOfEmail", "LibraryNews") %>';
        var xmlhttp;
        var results = null;
        var scope = '<%= ViewData["Scope"] %>';
        var id = $('#' + scope + browseid + 'ListTable').getGridParam('selrow');
        if (id) {
            $.get(
            urlAction + "?LibraryId=" + id,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    var posSubject = results.indexOf('LibraryNewsEmailSubject:');
                    var posBody = results.indexOf('LibraryNewsEmailBody:');
                    var subject = results.substring(0, posBody);
                    var bodyEmail = results.substring(posBody);
                    var posValueSubject = subject.indexOf(':');
                    var posValueBody = bodyEmail.indexOf(':');

                    var valueSubject = subject.substring(posValueSubject + 1);
                    var valueBody = bodyEmail.substring(posValueBody + 1);
                    location.href = 'mailto:?Subject=' + valueSubject + '&body=' + valueBody;
                }
            });            
        }
        else {
            showAlertSelectItemDialog();
        }
    };
</script>

<script type="text/javascript">
    $(function() {
        NewsSubtabs = new Ext.TabPanel({
            renderTo: 'NewsContent',
            autoWidth: true,
            frame: true,
            //defaults:{autoHeight: true},
            height: 640,
            listeners: {
                render: function(tabPanel) {
                    hideSubtabs(tabPanel);
                }
            },
            items: [
                    { contentEl: '<%= ViewData["Scope"] %>NewsEditFormContent', title: 'Header', id: '<%= ViewData["Scope"] %>NewsEditFormContent',
                        listeners: { activate: function() {
                            document.getElementById("breadcrumb").innerHTML = "<u>Workspace</u> > <u>News</u> > Header";
                            getEntityToRefresh('<%= Url.Action("Edit", "News") %>', '<%= ViewData["Scope"] %>', 'News', getIdBySelectedRow('<%= ViewData["Scope"] %>', 'NewsAll'), 'NewsAll', '#NewsContent');
                        }
                        }
                    }
                ]
        });
        resizeContent('#<%= ViewData["Scope"] %>NewsList');
    });
</script>

<script type="text/javascript">
    var ExecuteFilterByIndustry = function(select, value, scope, entity, browseid, browseIndustry, dayListOptions) {
        if (value != '') {
//            $('#grid' + browseid).css("display", "none");
//            $('#DeleteButton' + browseid).css("display", "none");
//            $('#SendButton' + browseid).css("display", "none");
//            $('#grid' + browseIndustry).css("display", "block");
//            $('#gridNewsAllByCompetitor').css("display", "none");
//            $('#gridNewsAllByProduct').css("display", "none");
//            $('#DeleteButton' + browseIndustry).css("display", "inline");
//            $('#SendButton' + browseIndustry).css("display", "inline");
//            $('#DeleteButtonNewsAllByCompetitor').css("display", "none");
//            $('#SendButtonNewsAllByCompetitor').css("display", "none");
//            $('#DeleteButtonNewsAllByProduct').css("display", "none");
//            $('#SendButtonNewsAllByProduct').css("display", "none");
//            $('#filterButtonNewsAllByCompetitor').css("display", "none");
//            $('#filterButtonNewsAllByProduct').css("display", "none");
//            $('#filterButton' + browseIndustry).css("display", "inline");
//            $('#filterButton' + browseid).css("display", "none");
//            $('#searchButtonNewsAllByCompetitor').css("display", "none");
//            $('#searchButtonNewsAllByProduct').css("display", "none");
//            $('#searchButton' + browseIndustry).css("display", "inline");
//            $('#searchButton' + browseid).css("display", "none");
            var componentId = scope + browseid;
            var formId = '#' + componentId + 'FilterForm';
            var gridId = '#' + componentId + 'ListTable';
            var dialogId = '#' + componentId + 'FilterDialog';
            var currentUrl = $(gridId).getGridParam("url");
            var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
            var query = '&filterCriteria=';

            var input = '';
            input = value;
            var valueInput = $('#FilterScoreId');
            var minscore = valueInput[0].value;
            if (minscore != '') {
                input += ':' + browseid + 'View.Score_Ge_' + minscore;
            }
            var selectDate = $('#' + scope + 'NewsAllDate');
            var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
            if (valueOfSelectDate != '') {
                var optionDay = dayListOptions.split(',');
                var valueDay;
                if (valueOfSelectDate == 100) {
                    valueDay = optionDay[0];
                }
                else if (valueOfSelectDate == 200) {
                    valueDay = optionDay[1];
                }
                else if (valueOfSelectDate == 300) {
                    valueDay = optionDay[2];
                }
                else if (valueOfSelectDate == 400) {
                    valueDay = optionDay[3];
                }
                else if (valueOfSelectDate == 500) {
                    valueDay = optionDay[4];
                }
                else if (valueOfSelectDate == 600) {
                    valueDay = optionDay[5];
                }
                else if (valueOfSelectDate == 700) {
                    valueDay = optionDay[6];
                }
                input += ':' + browseid + 'View.DateAdded_Ge_' + valueDay;
            }
            if ($('#' + scope + 'NewsAllCompetitorId')[0].options.length > 0) {
                $('#' + scope + 'NewsAllCompetitorId')[0].options[0].selected = true;
            }
            if ($('#' + scope + 'NewsAllProductId')[0].options.length > 0) {
                $('#' + scope + 'NewsAllProductId')[0].options[0].selected = true;
            }
            // $('#' + scope + 'NewsAllDate')[0].options[0].selected = true;
            query += '' + browseid + 'View.EntityType_Cn_Industry:' + browseid + 'View.EntityId_Cn_' + input;
            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
        }
        else {
            $('#grid' + browseid).css("display", "block");
            //$('#grid' + browseid).css("display", "none");
//            $('#gridNewsAllByCompetitor').css("display", "none");
//            $('#gridNewsAllByProduct').css("display", "none");
            reloadGridPrincipal(scope, entity, browseid, dayListOptions);
        }

    };

    var ExecuteFilterByIndustryT = function(select, value, scope, entity, browseid, browseIndustry, dayListOptions) {
        if (value != '') {
            $('#grid' + browseid).css("display", "none");
            $('#DeleteButton' + browseid).css("display", "none");
            $('#SendButton' + browseid).css("display", "none");
            $('#grid' + browseIndustry).css("display", "block");
            $('#gridNewsAllByCompetitor').css("display", "none");
            $('#gridNewsAllByProduct').css("display", "none");
            $('#DeleteButton' + browseIndustry).css("display", "inline");
            $('#SendButton' + browseIndustry).css("display", "inline");
            $('#DeleteButtonNewsAllByCompetitor').css("display", "none");
            $('#SendButtonNewsAllByCompetitor').css("display", "none");
            $('#DeleteButtonNewsAllByProduct').css("display", "none");
            $('#SendButtonNewsAllByProduct').css("display", "none");
            var componentId = scope + browseIndustry;
            var formId = '#' + componentId + 'FilterForm';
            var gridId = '#' + componentId + 'ListTable';
            var dialogId = '#' + componentId + 'FilterDialog';
            var currentUrl = $(gridId).getGridParam("url");
            var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
            var query = '&filterCriteria=';

            var input = '';
            input = value;
            var valueInput = $('#FilterScoreId');
            var minscore = valueInput[0].value;
            if (minscore != '') {
                input += ':' + browseIndustry + 'View.Score_Ge_' + minscore;
            }
            var selectDate = $('#' + scope + 'NewsAllDate');
            var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
            if (valueOfSelectDate != '') {
                var optionDay = dayListOptions.split(',');
                var valueDay;
                if (valueOfSelectDate == 100) {
                    valueDay = optionDay[0];
                }
                else if (valueOfSelectDate == 200) {
                    valueDay = optionDay[1];
                }
                else if (valueOfSelectDate == 300) {
                    valueDay = optionDay[2];
                }
                else if (valueOfSelectDate == 400) {
                    valueDay = optionDay[3];
                }
                else if (valueOfSelectDate == 500) {
                    valueDay = optionDay[4];
                }
                else if (valueOfSelectDate == 600) {
                    valueDay = optionDay[5];
                }
                else if (valueOfSelectDate == 700) {
                    valueDay = optionDay[6];
                }
                input += ':' + browseIndustry + 'View.DateAdded_Ge_' + valueDay;
            }
            //$('#' + scope + 'NewsAllCompetitorId')[0].options[0].selected = true;
            //$('#' + scope + 'NewsAllProductId')[0].options[0].selected = true;
            // $('#' + scope + 'NewsAllDate')[0].options[0].selected = true;
            query += '' + browseIndustry + 'View.EntityType_Eq_INDTR:' + browseIndustry + 'View.EntityId_Eq_' + input;
            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
        }
        else {
            $('#grid' + browseid).css("display", "block");
            $('#grid' + browseIndustry).css("display", "none");
            $('#gridNewsAllByCompetitor').css("display", "none");
            $('#gridNewsAllByProduct').css("display", "none");
            //reloadGridPrincipal(scope, entity, browseid, dayListOptions);
        }

    };

    var ExecuteFilterByCompetitor = function(select, value, scope, entity, browseid, browseCompetitor, dayListOptions) {
        if (value != '') {
            $('#grid' + browseid).css("display", "none");
            $('#DeleteButton' + browseid).css("display", "none");
            $('#SendButton' + browseid).css("display", "none");
            $('#gridNewsAllByIndustry').css("display", "none");
            $('#gridNewsAllByProduct').css("display", "none");
            $('#grid' + browseCompetitor).css("display", "block");
            $('#DeleteButton' + browseCompetitor).css("display", "inline");
            $('#SendButton' + browseCompetitor).css("display", "inline");
            $('#DeleteButtonNewsAllByIndustry').css("display", "none");
            $('#DeleteButtonNewsAllByProduct').css("display", "none");
            $('#SendButtonNewsAllByIndustry').css("display", "none");
            $('#SendButtonNewsAllByProduct').css("display", "none");
            $('#filterButtonNewsAllIndustry').css("display", "none");
            $('#filterButtonNewsAllProduct').css("display", "none");
            $('#filterButton' + browseCompetitor).css("display", "inline");
            $('#filterButton' + browseid).css("display", "none");
            $('#searchButtonNewsAllByIndustry').css("display", "none");
            $('#searchButtonNewsAllByProduct').css("display", "none");
            $('#searchButton' + browseCompetitor).css("display", "inline");
            $('#searchButton' + browseid).css("display", "none");
            var componentId = scope + browseCompetitor;
            var formId = '#' + componentId + 'FilterForm';
            var gridId = '#' + componentId + 'ListTable';
            var dialogId = '#' + componentId + 'FilterDialog';
            var currentUrl = $(gridId).getGridParam("url");
            var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
            var query = '&filterCriteria=';
            var input = '';
            input = value;
            var valueInput = $('#FilterScoreId');
            var minscore = valueInput[0].value;
            if (minscore != '') {
                input += ':' + browseCompetitor + 'View.Score_Ge_' + minscore;
            }
            var selectDate = $('#' + scope + 'NewsAllDate');
            var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
            if (valueOfSelectDate != '') {
                var optionDay = dayListOptions.split(',');
                var valueDay;
                if (valueOfSelectDate == 100) {
                    valueDay = optionDay[0];
                }
                else if (valueOfSelectDate == 200) {
                    valueDay = optionDay[1];
                }
                else if (valueOfSelectDate == 300) {
                    valueDay = optionDay[2];
                }
                else if (valueOfSelectDate == 400) {
                    valueDay = optionDay[3];
                }
                else if (valueOfSelectDate == 500) {
                    valueDay = optionDay[4];
                }
                else if (valueOfSelectDate == 600) {
                    valueDay = optionDay[5];
                }
                else if (valueOfSelectDate == 700) {
                    valueDay = optionDay[6];
                }
                input += ':' + browseCompetitor + 'View.DateAdded_Ge_' + valueDay;
            }
            if ($('#' + scope + 'NewsAllIndustryId')[0].options.length > 0) {
                $('#' + scope + 'NewsAllIndustryId')[0].options[0].selected = true;
            }
            if ($('#' + scope + 'NewsAllProductId')[0].options.length > 0) {
                $('#' + scope + 'NewsAllProductId')[0].options[0].selected = true;
            }
            //$('#' + scope + 'NewsAllDate')[0].options[0].selected = true;
            query += browseCompetitor + 'View.EntityType_Eq_COMPT:' + browseCompetitor + 'View.EntityId_Eq_' + input;
            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
        }
        else {
            $('#grid' + browseid).css("display", "block");
            $('#gridNewsAllByIndustry').css("display", "none");
            $('#gridNewsAllByProduct').css("display", "none");
            $('#grid' + browseCompetitor).css("display", "none");
            var selectIndustry = $('#' + scope + 'NewsAllIndustryId');
            var valueOfSelectIndustry = selectIndustry[0].options[selectIndustry[0].selectedIndex].value;
            if (selectIndustry != '') {
                $('#filterButtonNewsAllIndustry').css("display", "inline");
                $('#searchButtonNewsAllByIndustry').css("display", "inline");
                $('#filterButton' + browseid).css("display", "none");
                $('#searchButton' + browseid).css("display", "none");
            }
            else {
                $('#filterButtonNewsAllByIndustry').css("display", "none");
                $('#searchButtonNewsAllByIndustry').css("display", "none");
                $('#filterButton' + browseid).css("display", "inline");
                $('#searchButton' + browseid).css("display", "inline");
                $('#grid' + browseid).css("display", "block");
                $('#gridNewsAllByIndustry').css("display", "none");
                $('#gridNewsAllByCompetitor').css("display", "none");
                $('#gridNewsAllByProduct').css("display", "none");
            }
            $('#filterButtonNewsAllProduct').css("display", "none");
            $('#filterButton' + browseCompetitor).css("display", "none");
            $('#searchButtonNewsAllByProduct').css("display", "none");
            $('#searchButton' + browseCompetitor).css("display", "none");
            reloadGridPrincipal(scope, entity, browseid, dayListOptions);
        }
    };

    var ExecuteFilterByCompetitorT = function(select, value, scope, entity, browseid, browseCompetitor, dayListOptions) {
        var query = '&filterCriteria=';
        var inputOtherFields = '';
        var valueInput = $('#FilterScoreId');
        var minscore = valueInput[0].value;
        if (minscore != '') {
            inputOtherFields += '' + browseid + 'View.Score_Ge_' + minscore;
        }
        var selectDate = $('#' + scope + 'NewsAllDate');
        var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
        if (valueOfSelectDate != '') {
            var optionDay = dayListOptions.split(',');
            var valueDay;
            if (valueOfSelectDate == 100) {
                valueDay = optionDay[0];
            }
            else if (valueOfSelectDate == 200) {
                valueDay = optionDay[1];
            }
            else if (valueOfSelectDate == 300) {
                valueDay = optionDay[2];
            }
            else if (valueOfSelectDate == 400) {
                valueDay = optionDay[3];
            }
            else if (valueOfSelectDate == 500) {
                valueDay = optionDay[4];
            }
            else if (valueOfSelectDate == 600) {
                valueDay = optionDay[5];
            }
            else if (valueOfSelectDate == 700) {
                valueDay = optionDay[6];
            }
            inputOtherFields += ':' + browseid + 'View.DateAdded_Ge_' + valueDay;
        }

        if (value != '') {
            var componentId = scope + browseid;
            var formId = '#' + componentId + 'FilterForm';
            var gridId = '#' + componentId + 'ListTable';
            var dialogId = '#' + componentId + 'FilterDialog';
            var currentUrl = $(gridId).getGridParam("url");
            var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);

            var input = '';
            input = value;
            if (inputOtherFields != '') {
                input += ':' + inputOtherFields;
            }
            if ($('#' + scope + 'NewsAllIndustryId')[0].options.length > 0) {
                $('#' + scope + 'NewsAllIndustryId')[0].options[0].selected = true;
            }
            //$('#' + scope + 'NewsAllProductId')[0].options[0].selected = true;
            //$('#' + scope + 'NewsAllDate')[0].options[0].selected = true;
            query += browseid + 'View.EntityType_Cn_Competitor:' + browseid + 'View.EntityId_Cn_' + input;
            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
        }
        else {
            //            var selectIndustry = $('#' + scope + 'NewsAllIndustryId');
            //            var valueOfSelectIndustry = selectIndustry[0].options[selectIndustry[0].selectedIndex].value;
            //            if (valueOfSelectIndustry != '') {
            //                query += '' + browseid + 'View.EntityType_Cn_Industry:' + browseid + 'View.EntityId_Cn_' + valueOfSelectIndustry;
            //                $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
            //            }
            //            else {
            query = inputOtherFields;
           // alert(inputOtherFields);
            if (inputOtherFields != '') {
                $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
            } else {
                reloadGridPrincipal(scope, entity, browseid, dayListOptions);
            }
            //}
            reloadGridPrincipal(scope, entity, browseid, dayListOptions);
        }
        // $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    };

    var ExecuteFilterByProduct = function(select, value, scope, entity, browseid, browseProduct, dayListOptions) {
        if (value != '') {
            $('#grid' + browseid).css("display", "none");
            $('#DeleteButton' + browseid).css("display", "none");
            $('#SendButton' + browseid).css("display", "none");
            $('#gridNewsAllByIndustry').css("display", "none");
            $('#gridNewsAllByCompetitor').css("display", "none");
            $('#grid' + browseProduct).css("display", "block");
            $('#DeleteButton' + browseProduct).css("display", "inline");
            $('#DeleteButtonNewsAllByCompetitor').css("display", "none");
            $('#DeleteButtonNewsAllByIndustry').css("display", "none");
            $('#SendButton' + browseProduct).css("display", "inline");
            $('#SendButtonNewsAllByCompetitor').css("display", "none");
            $('#SendButtonNewsAllByIndustry').css("display", "none");
            $('#filterButtonNewsAllByIndustry').css("display", "none");
            $('#filterButtonNewsAllByCompetitor').css("display", "none");
            $('#filterButton' + browseProduct).css("display", "inline");
            $('#filterButton' + browseid).css("display", "none");
            $('#searchButtonNewsAllByIndustry').css("display", "none");
            $('#searchButtonNewsAllByCompetitor').css("display", "none");
            $('#searchButton' + browseProduct).css("display", "inline");
            $('#searchButton' + browseid).css("display", "none");
            var componentId = scope + browseProduct;
            var formId = '#' + componentId + 'FilterForm';
            var gridId = '#' + componentId + 'ListTable';
            var dialogId = '#' + componentId + 'FilterDialog';
            var currentUrl = $(gridId).getGridParam("url");
            var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
            var query = '&filterCriteria=';
            var input = '';
            input = value;
            var valueInput = $('#FilterScoreId');
            var minscore = valueInput[0].value;
            if (minscore != '') {
                input += ':' + browseProduct + 'View.Score_Ge_' + minscore;
            }
            var selectDate = $('#' + scope + 'NewsAllDate');
            var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
            if (valueOfSelectDate != '') {
                var optionDay = dayListOptions.split(',');
                var valueDay;
                if (valueOfSelectDate == 100) {
                    valueDay = optionDay[0];
                }
                else if (valueOfSelectDate == 200) {
                    valueDay = optionDay[1];
                }
                else if (valueOfSelectDate == 300) {
                    valueDay = optionDay[2];
                }
                else if (valueOfSelectDate == 400) {
                    valueDay = optionDay[3];
                }
                else if (valueOfSelectDate == 500) {
                    valueDay = optionDay[4];
                }
                else if (valueOfSelectDate == 600) {
                    valueDay = optionDay[5];
                }
                else if (valueOfSelectDate == 700) {
                    valueDay = optionDay[6];
                }
                input += ':' + browseProduct + 'View.DateAdded_Ge_' + valueDay;
            }
            if ($('#' + scope + 'NewsAllIndustryId')[0].options.length > 0) {
                $('#' + scope + 'NewsAllIndustryId')[0].options[0].selected = true;
            }
            if ($('#' + scope + 'NewsAllCompetitorId')[0].options.length > 0) {
                $('#' + scope + 'NewsAllCompetitorId')[0].options[0].selected = true;
            }
            //$('#' + scope + 'NewsAllDate')[0].options[0].selected = true;
            query += browseProduct + 'View.EntityType_Eq_PRODT:' + browseProduct + 'View.EntityId_Eq_' + input;
            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
        }
        else {
            $('#grid' + browseid).css("display", "block");
            $('#gridNewsAllByIndustry').css("display", "none");
            $('#gridNewsAllByCompetitor').css("display", "none");
            $('#grid' + browseProduct).css("display", "none");
            $('#filterButtonNewsAllByIndustry').css("display", "none");
            $('#filterButtonNewsAllByCompetitor').css("display", "inline");
            $('#filterButton' + browseProduct).css("display", "none");
            $('#filterButton' + browseid).css("display", "none");
            $('#searchButtonNewsAllByIndustry').css("display", "none");
            $('#searchButtonNewsAllByCompetitor').css("display", "inline");
            $('#searchButton' + browseProduct).css("display", "none");
            $('#searchButton' + browseid).css("display", "none");
            reloadGridPrincipal(scope, entity, browseid, dayListOptions);
        }
    };

    var ExecuteFilterByProductT = function(select, value, scope, entity, browseid, browseProduct, dayListOptions) {
        if (value != '') {
            //            $('#grid' + browseid).css("display", "none");
            //            $('#DeleteButton' + browseid).css("display", "none");
            //            $('#SendButton' + browseid).css("display", "none");
            //            $('#gridNewsAllByIndustry').css("display", "none");
            //            $('#gridNewsAllByCompetitor').css("display", "none");
            //            $('#grid' + browseProduct).css("display", "block");
            //            $('#DeleteButton' + browseProduct).css("display", "inline");
            //            $('#DeleteButtonNewsAllByCompetitor').css("display", "none");
            //            $('#DeleteButtonNewsAllByIndustry').css("display", "none");
            //            $('#SendButton' + browseProduct).css("display", "inline");
            //            $('#SendButtonNewsAllByCompetitor').css("display", "none");
            //            $('#SendButtonNewsAllByIndustry').css("display", "none");
            //            $('#filterButtonNewsAllByIndustry').css("display", "none");
            //            $('#filterButtonNewsAllByCompetitor').css("display", "none");
            //            $('#filterButton' + browseProduct).css("display", "inline");
            //            $('#filterButton' + browseid).css("display", "none");
            //            $('#searchButtonNewsAllByIndustry').css("display", "none");
            //            $('#searchButtonNewsAllByCompetitor').css("display", "none");
            //            $('#searchButton' + browseProduct).css("display", "inline");
            //            $('#searchButton' + browseid).css("display", "none");
            var componentId = scope + browseid;
            var formId = '#' + componentId + 'FilterForm';
            var gridId = '#' + componentId + 'ListTable';
            var dialogId = '#' + componentId + 'FilterDialog';
            var currentUrl = $(gridId).getGridParam("url");
            var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
            var query = '&filterCriteria=';
            var input = '';
            input = value;
            var valueInput = $('#FilterScoreId');
            var minscore = valueInput[0].value;
            if (minscore != '') {
                input += ':' + browseid + 'View.Score_Ge_' + minscore;
            }
            var selectDate = $('#' + scope + 'NewsAllDate');
            var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
            if (valueOfSelectDate != '') {
                var optionDay = dayListOptions.split(',');
                var valueDay;
                if (valueOfSelectDate == 100) {
                    valueDay = optionDay[0];
                }
                else if (valueOfSelectDate == 200) {
                    valueDay = optionDay[1];
                }
                else if (valueOfSelectDate == 300) {
                    valueDay = optionDay[2];
                }
                else if (valueOfSelectDate == 400) {
                    valueDay = optionDay[3];
                }
                else if (valueOfSelectDate == 500) {
                    valueDay = optionDay[4];
                }
                else if (valueOfSelectDate == 600) {
                    valueDay = optionDay[5];
                }
                else if (valueOfSelectDate == 700) {
                    valueDay = optionDay[6];
                }
                input += ':' + browseid + 'View.DateAdded_Ge_' + valueDay;
            }
            //$('#' + scope + 'NewsAllCompetitorId')[0].options[0].selected = true;
            //$('#' + scope + 'NewsAllIndustryId')[0].options[0].selected = true;
            //$('#' + scope + 'NewsAllDate')[0].options[0].selected = true;
            query += browseid + 'View.EntityType_Cn_Product:' + browseid + 'View.EntityId_Cn_' + input;
            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
        }
        else {
            var selectCompetitor = $('#' + scope + 'NewsAllCompetitorId');
            var valueCompetitor = '';
            if (!selectCompetitor[0].options[0].selected) {
                valueCompetitor = selectCompetitor[0].options[selectCompetitor[0].selectedIndex].value
            }
            ExecuteFilterByCompetitorT(selectCompetitor, valueCompetitor, scope, entity, browseid, 'NewsAllByCompetitor', dayListOptions);
            //            $('#grid' + browseid).css("display", "none");
            //            $('#gridNewsAllByIndustry').css("display", "none");
            //            $('#gridNewsAllByCompetitor').css("display", "block");
            //            $('#grid' + browseProduct).css("display", "none");
            //            $('#filterButtonNewsAllByIndustry').css("display", "none");
            //            $('#filterButtonNewsAllByCompetitor').css("display", "inline");
            //            $('#filterButton' + browseProduct).css("display", "none");
            //            $('#filterButton' + browseid).css("display", "none");
            //            $('#searchButtonNewsAllByIndustry').css("display", "none");
            //            $('#searchButtonNewsAllByCompetitor').css("display", "inline");
            //            $('#searchButton' + browseProduct).css("display", "none");
            //            $('#searchButton' + browseid).css("display", "none");
            //reloadGridPrincipal(scope, entity, browseid, dayListOptions);
        }
    };

    var ExecuteFilterByDate = function(select, value, scope, entity, browseid, dayListOptions) {
        var query = '&filterCriteria=';
        var optionDay = dayListOptions.split(',');
        var input = '';
        var browseTempo = browseid;
        if (value == 100) {
            input = optionDay[0];
        }
        else if (value == 200) {
            input = optionDay[1];
        }
        else if (value == 300) {
            input = optionDay[2];
        }
        else if (value == 400) {
            value = optionDay[3];
        }
        else if (value == 500) {
            input = optionDay[4];
        }
        else if (value == 600) {
            input = optionDay[5];
        }
        else if (value == 700) {
            input = optionDay[6];
        }
        var selectIndustry = $('#' + scope + 'NewsAllIndustryId');
        var selectCompetitor = $('#' + scope + 'NewsAllCompetitorId');
        var selectProduct = $('#' + scope + 'NewsAllProductId');
        if (!selectIndustry[0].options[0].selected) {
            browseTempo = 'NewsAllByIndustry';
            input += ':' + browseTempo + 'View.EntityType_Eq_INDTR:' + browseTempo + 'View.EntityId_Eq_' + selectIndustry[0].options[selectIndustry[0].selectedIndex].value;

        }
        else if (!selectCompetitor[0].options[0].selected) {
            browseTempo = 'NewsAllByCompetitor';
            input += ':' + browseTempo + 'View.EntityType_Eq_COMPT:' + browseTempo + 'View.EntityId_Eq_' + selectCompetitor[0].options[selectCompetitor[0].selectedIndex].value;
        }
        else if (!selectProduct[0].options[0].selected) {
            browseTempo = 'NewsAllByProduct';
            input += ':' + browseTempo + 'View.EntityType_Eq_PRODT:' + browseTempo + 'View.EntityId_Eq_' + selectProduct[0].options[selectProduct[0].selectedIndex].value;
        }
        else {
            browseTempo = browseid;
        }
        var componentId = scope + browseTempo;
        var formId = '#' + componentId + 'FilterForm';
        var gridId = '#' + componentId + 'ListTable';
        var dialogId = '#' + componentId + 'FilterDialog';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);


        var valueInput = $('#FilterScoreId');
        var minscore = valueInput[0].value;
        if (minscore != '') {
            input += ':' + browseTempo + 'View.Score_Ge_' + minscore;
        }

        query += '' + browseTempo + 'View.DateAdded_Ge_' + input;
        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    };

    var ExecuteFilterByDateT = function(select, value, scope, entity, browseid, dayListOptions) {
        var query = '&filterCriteria=';
        var optionDay = dayListOptions.split(',');
        var input = '';
        var browseTempo = browseid;

        var stringFilter = '';

        if (value != '') {
            if (value == 100) {
                input = optionDay[0];
            }
            else if (value == 200) {
                input = optionDay[1];
            }
            else if (value == 300) {
                input = optionDay[2];
            }
            else if (value == 400) {
                value = optionDay[3];
            }
            else if (value == 500) {
                input = optionDay[4];
            }
            else if (value == 600) {
                input = optionDay[5];
            }
            else if (value == 700) {
                input = optionDay[6];
            }
            stringFilter = browseTempo + 'View.DateAdded_Ge_';
        } 
        var selectIndustry = $('#' + scope + 'NewsAllIndustryId');
        var selectCompetitor = $('#' + scope + 'NewsAllCompetitorId');
        var selectProduct = $('#' + scope + 'NewsAllProductId');
        if (selectProduct[0].options != null && selectProduct[0].options != undefined && selectProduct[0].options.length > 0 && !selectProduct[0].options[0].selected) {
            //browseTempo = 'NewsAllByProduct';
            input += ':' + browseTempo + 'View.EntityType_Cn_Product:' + browseTempo + 'View.EntityId_Cn_' + selectProduct[0].options[selectProduct[0].selectedIndex].value;
        }
        else if (selectCompetitor[0].options != null && selectCompetitor[0].options != undefined && selectCompetitor[0].options.length > 0 && !selectCompetitor[0].options[0].selected) {
            //browseTempo = 'NewsAllByCompetitor';
            input += ':' + browseTempo + 'View.EntityType_Cn_Competitor:' + browseTempo + 'View.EntityId_Cn_' + selectCompetitor[0].options[selectCompetitor[0].selectedIndex].value;
        }
        else if (selectIndustry[0].options != null && selectIndustry[0].options != undefined && selectIndustry[0].options.length > 0 && !selectIndustry[0].options[0].selected) {
            //browseTempo = 'NewsAllByIndustry';
            input += ':' + browseTempo + 'View.EntityType_Cn_Industry:' + browseTempo + 'View.EntityId_Cn_' + selectIndustry[0].options[selectIndustry[0].selectedIndex].value;
        }
        else {
            browseTempo = browseid;
        }
        var componentId = scope + browseTempo;
        var formId = '#' + componentId + 'FilterForm';
        var gridId = '#' + componentId + 'ListTable';
        var dialogId = '#' + componentId + 'FilterDialog';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);


        var valueInput = $('#FilterScoreId');
        var minscore = valueInput[0].value;
        if (minscore != '') {
            input += ':' + browseTempo + 'View.Score_Ge_' + minscore;
        }

        query += '' + stringFilter + input;
        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    };


    var ExecuteFilterByScore = function(scope, entity, browseid, dayListOptions) {
        var browseTempo = browseid;
        var query = '&filterCriteria=';
        var valueInput = $('#FilterScoreId');
        var input = '';

        input = valueInput.val();

        var selectIndustry = $('#' + scope + 'NewsAllIndustryId');
        var selectCompetitor = $('#' + scope + 'NewsAllCompetitorId');
        var selectProduct = $('#' + scope + 'NewsAllProductId');
        if (!selectIndustry[0].options[0].selected) {
            browseTempo = 'NewsAllByIndustry';
            input += ':' + browseTempo + 'View.EntityType_Eq_INDTR:' + browseTempo + 'View.EntityId_Eq_' + selectIndustry[0].options[selectIndustry[0].selectedIndex].value;
        }
        else if (!selectCompetitor[0].options[0].selected) {
            browseTempo = 'NewsAllByCompetitor';
            input += ':' + browseTempo + 'View.EntityType_Eq_COMPT:' + browseTempo + 'View.EntityId_Eq_' + selectCompetitor[0].options[selectCompetitor[0].selectedIndex].value;
        }
        else if (!selectProduct[0].options[0].selected) {
            browseTempo = 'NewsAllByProduct';
            input += ':' + browseTempo + 'View.EntityType_Eq_PRODT:' + browseTempo + 'View.EntityId_Eq_' + selectProduct[0].options[selectProduct[0].selectedIndex].value;
        }
        else {
            browseTempo = browseid;
        }

        var selectDate = $('#' + scope + 'NewsAllDate');
        var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
        if (valueOfSelectDate != '') {
            var optionDay = dayListOptions.split(',');
            var valueDay;
            if (valueOfSelectDate == 100) {
                valueDay = optionDay[0];
            }
            else if (valueOfSelectDate == 200) {
                valueDay = optionDay[1];
            }
            else if (valueOfSelectDate == 300) {
                valueDay = optionDay[2];
            }
            else if (valueOfSelectDate == 400) {
                valueDay = optionDay[3];
            }
            else if (valueOfSelectDate == 500) {
                valueDay = optionDay[4];
            }
            else if (valueOfSelectDate == 600) {
                valueDay = optionDay[5];
            }
            else if (valueOfSelectDate == 700) {
                valueDay = optionDay[6];
            }
            input += ':' + browseTempo + 'View.DateAdded_Ge_' + valueDay;
        }

        var componentId = scope + browseTempo;
        var formId = '#' + componentId + 'FilterForm';
        var gridId = '#' + componentId + 'ListTable';
        var dialogId = '#' + componentId + 'FilterDialog';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        query += '' + browseTempo + 'View.Score_Ge_' + input;
        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    };

    var ExecuteFilterByScoreT = function(scope, entity, browseid, dayListOptions) {
        var browseTempo = browseid;
        var query = '&filterCriteria=';
        var valueInput = $('#FilterScoreId');
        var input = '';

        input = valueInput.val();

        var selectIndustry = $('#' + scope + 'NewsAllIndustryId');
        var selectCompetitor = $('#' + scope + 'NewsAllCompetitorId');
        var selectProduct = $('#' + scope + 'NewsAllProductId');
        if (selectProduct[0].options != null && selectProduct[0].options != undefined && selectProduct[0].options.length > 0 && !selectProduct[0].options[0].selected) {
            browseTempo = 'NewsAllByProduct';
            input += ':' + browseTempo + 'View.EntityType_Eq_PRODT:' + browseTempo + 'View.EntityId_Eq_' + selectProduct[0].options[selectProduct[0].selectedIndex].value;
        }
        else if (selectCompetitor[0].options != null && selectCompetitor[0].options != undefined && selectCompetitor[0].options.length > 0 && !selectCompetitor[0].options[0].selected) {
            browseTempo = 'NewsAllByCompetitor';
            input += ':' + browseTempo + 'View.EntityType_Eq_COMPT:' + browseTempo + 'View.EntityId_Eq_' + selectCompetitor[0].options[selectCompetitor[0].selectedIndex].value;
        }
        else if (selectIndustry[0].options != null && selectIndustry[0].options != undefined && selectIndustry[0].options.length > 0 && !selectIndustry[0].options[0].selected) {
            browseTempo = 'NewsAllByIndustry';
            input += ':' + browseTempo + 'View.EntityType_Eq_INDTR:' + browseTempo + 'View.EntityId_Eq_' + selectIndustry[0].options[selectIndustry[0].selectedIndex].value;
        }
        else {
            browseTempo = browseid;
        }

        var selectDate = $('#' + scope + 'NewsAllDate');
        if (selectDate[0].options != null && selectDate[0].options != undefined && selectDate[0].options.length > 0 && !selectDate[0].options[0].selected) {
            var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
            if (valueOfSelectDate != '') {
                var optionDay = dayListOptions.split(',');
                var valueDay;
                if (valueOfSelectDate == 100) {
                    valueDay = optionDay[0];
                }
                else if (valueOfSelectDate == 200) {
                    valueDay = optionDay[1];
                }
                else if (valueOfSelectDate == 300) {
                    valueDay = optionDay[2];
                }
                else if (valueOfSelectDate == 400) {
                    valueDay = optionDay[3];
                }
                else if (valueOfSelectDate == 500) {
                    valueDay = optionDay[4];
                }
                else if (valueOfSelectDate == 600) {
                    valueDay = optionDay[5];
                }
                else if (valueOfSelectDate == 700) {
                    valueDay = optionDay[6];
                }
                input += ':' + browseTempo + 'View.DateAdded_Ge_' + valueDay;
            }
        }
        var componentId = scope + browseTempo;
        var formId = '#' + componentId + 'FilterForm';
        var gridId = '#' + componentId + 'ListTable';
        var dialogId = '#' + componentId + 'FilterDialog';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        query += '' + browseTempo + 'View.Score_Ge_' + input;
        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    };

    var reloadGridPrincipal = function(scope, entity, browseid, dayListOptions) {
        var componentId = scope + browseid;
        var formId = '#' + componentId + 'FilterForm';
        var gridId = '#' + componentId + 'ListTable';
        var dialogId = '#' + componentId + 'FilterDialog';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        var query = '&filterCriteria=';
        var valueInput = $('#FilterScoreId');
        var input = '';

        input = valueInput.val();
        var selectDate = $('#' + scope + 'NewsAllDate');
        var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
        if (valueOfSelectDate != '') {
            var optionDay = dayListOptions.split(',');
            var valueDay;
            if (valueOfSelectDate == 100) {
                valueDay = optionDay[0];
            }
            else if (valueOfSelectDate == 200) {
                valueDay = optionDay[1];
            }
            else if (valueOfSelectDate == 300) {
                valueDay = optionDay[2];
            }
            else if (valueOfSelectDate == 400) {
                valueDay = optionDay[3];
            }
            else if (valueOfSelectDate == 500) {
                valueDay = optionDay[4];
            }
            else if (valueOfSelectDate == 600) {
                valueDay = optionDay[5];
            }
            else if (valueOfSelectDate == 700) {
                valueDay = optionDay[6];
            }
            input += ':NewsAllView.DateAdded_Ge_' + valueDay;
        }
        if ($('#' + scope + 'NewsAllProductId')[0].options.length > 0) {
            $('#' + scope + 'NewsAllProductId')[0].options[0].selected = true;
        }
        if ($('#' + scope + 'NewsAllCompetitorId')[0].options.length > 0) {
            $('#' + scope + 'NewsAllCompetitorId')[0].options[0].selected = true;
        }
        if ($('#' + scope + 'NewsAllIndustryId')[0].options.length > 0) {
            $('#' + scope + 'NewsAllIndustryId')[0].options[0].selected = true;
        }

        query += 'NewsAllView.Score_Ge_' + input;
        var valueInput = $('#FilterScoreId');
        var minscore = valueInput[0].value;
        if (minscore != '') {
            input += ':NewsAllView.Score_Ge_' + minscore;
        }
        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
        $('#DeleteButton' + browseid).css("display", "inline");
        $('#DeleteButtonNewsAllByIndustry').css("display", "none");
        $('#DeleteButtonNewsAllByCompetitor').css("display", "none");
        $('#DeleteButtonNewsAllByProduct').css("display", "none");
        $('#SendButton' + browseid).css("display", "inline");
        $('#SendButtonNewsAllByIndustry').css("display", "none");
        $('#SendButtonNewsAllByCompetitor').css("display", "none");
        $('#SendButtonNewsAllByProduct').css("display", "none");
    };

    var ExecuteFilterByNewToday = function(scope, entity, browseid, dayListOptions) {
        var browseTempo = browseid;
        var query = '&filterCriteria=';
        var input = '';
        var selectIndustry = $('#' + scope + 'NewsAllIndustryId');
        var selectCompetitor = $('#' + scope + 'NewsAllCompetitorId');
        var selectProduct = $('#' + scope + 'NewsAllProductId');
        if (!selectProduct[0].options[0].selected) {
            browseTempo = 'NewsAllByProduct';
            input += ':' + browseTempo + 'View.EntityType_Eq_PRODT:' + browseTempo + 'View.EntityId_Eq_' + selectProduct[0].options[selectProduct[0].selectedIndex].value;
        }
        else if (!selectCompetitor[0].options[0].selected) {
            browseTempo = 'NewsAllByCompetitor';
            input += ':' + browseTempo + 'View.EntityType_Eq_COMPT:' + browseTempo + 'View.EntityId_Eq_' + selectCompetitor[0].options[selectCompetitor[0].selectedIndex].value;
        }
        else if (!selectIndustry[0].options[0].selected) {
            browseTempo = 'NewsAllByIndustry';
            input += ':' + browseTempo + 'View.EntityType_Eq_INDTR:' + browseTempo + 'View.EntityId_Eq_' + selectIndustry[0].options[selectIndustry[0].selectedIndex].value;
        }
        else {
            browseTempo = browseid;
        }
        var selectDate = $('#' + scope + 'NewsAllDate');
        var valueOfSelectDate = selectDate[0].options[selectDate[0].selectedIndex].value;
        if (valueOfSelectDate != '') {
            var optionDay = dayListOptions.split(',');
            var valueDay;
            if (valueOfSelectDate == 100) {
                valueDay = optionDay[0];
            }
            else if (valueOfSelectDate == 200) {
                valueDay = optionDay[1];
            }
            else if (valueOfSelectDate == 300) {
                valueDay = optionDay[2];
            }
            else if (valueOfSelectDate == 400) {
                valueDay = optionDay[3];
            }
            else if (valueOfSelectDate == 500) {
                valueDay = optionDay[4];
            }
            else if (valueOfSelectDate == 600) {
                valueDay = optionDay[5];
            }
            else if (valueOfSelectDate == 700) {
                valueDay = optionDay[6];
            }
            input += ':' + browseTempo + 'View.DateAdded_Ge_' + valueDay;
        }
        var componentId = scope + browseTempo;
        var formId = '#' + componentId + 'FilterForm';
        var gridId = '#' + componentId + 'ListTable';
        var dialogId = '#' + componentId + 'FilterDialog';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        query += '' + browseTempo + 'View.NewToday_Eq_Y' + input;
        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    };
</script>

<asp:Panel ID="NewsListContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NewsList" class="indexOne">
        <% Html.RenderPartial("List"); %>
    </div>
    <div class="resizeS">
        <img src="<%= Url.Content("~/content/Images/Styles/angle-nxs.gif") %>" /></div>
</asp:Panel>
<br />
<asp:Panel ID="NewsFormContent" runat="server">
    <div id="<%= ViewData["Scope"] %>NewsEditFormContent" class="x-hide-display heightSubPanels" />
</asp:Panel>
