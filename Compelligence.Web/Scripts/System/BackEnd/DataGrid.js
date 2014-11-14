
var executeSearch = function(scope, entity, modalDialog) {
    var componentId = scope + entity;
    var formId = '#' + componentId + 'SearchForm';
    var gridId = '#' + componentId + 'ListTable';
    var dialogId = '#' + componentId + 'SearchDialog';
    var formFields = $(formId + ' input[type="text"]');
    var currentUrl = $(gridId).getGridParam("url");
    var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
    var input = '';
    for (var i = 0; i < formFields.length; i++) {
        if (i == formFields.length - 1) {
            var formFieldValue = formFields[i].value;
            if (formFieldValue.indexOf('"') != -1) {
                var newFieldValue = formFieldValue.split('"');
                input += newFieldValue[1];
            }
            else {
                input += formFields[i].value;
            }

        }
    }
    var result = delWhiteSpaceFromExtremes(input);

    var query = '&searchCriteria=' + result;

    posentity = entity.indexOf('All');
    controller = entity.substring(0, posentity);
    if (currentUrl.indexOf('&filterCriteria') != -1) {
        var checkedall = $('#' + scope + controller + 'Checkbox').prop('checked');
        if (checkedall != undefined && checkedall != null) {
            if (checkedall) {
                pos = currentUrl.indexOf('&filterCriteria');
                temp = currentUrl.substring(pos, currentUrl.length);
                query += temp;
            }
        }
        else {
            var posFilter = currentUrl.indexOf('&filterCriteria');
            var filterCriteria = '';
            filterCriteria = currentUrl.substring(posFilter);
            if (filterCriteria != '') {
                query += filterCriteria
            }
        }
    }


    $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    $('#filterValue').val('');
    $("#filterColumn").val(0);
    $("#filterOperator88").val(0);
    $('#searchValue').val(result);
    if (document.getElementById(componentId + "SelectedOption") != null) {
        if (result != '') {
            document.getElementById(componentId + "SelectedOption").innerHTML = 'Search Criteria: ' + "'" + result + "'";
        } else {
            document.getElementById(componentId + "SelectedOption").innerHTML = '';
        }
    }

    if (modalDialog) {
        $(dialogId).dialog('close');
    }
};

var executeFilter = function(scope, entity) {
    var componentId = scope + entity;
    var formId = '#' + componentId + 'FilterForm';
    var gridId = '#' + componentId + 'ListTable';
    var dialogId = '#' + componentId + 'FilterDialog';
    var currentUrl = $(gridId).getGridParam("url");
    var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
    var query = '&filterCriteria=';

    posentity = entity.indexOf('All');
    controller = entity.substring(0, posentity);
    if (currentUrl.indexOf('&filterCriteria') != -1) {
        var checkedall = $('#' + scope + controller + 'Checkbox').prop('checked');
        if (checkedall) {
            pos = currentUrl.indexOf('&filterCriteria');
            temp = currentUrl.substring((pos + query.length), currentUrl.length);
            query += temp + ':';
        }
    }

    var filterColumnFields = $(formId + ' select[name=' + componentId + 'filterColumn]');
    var filterOperatorFields = $(formId + ' select[name=filterOperator]');
    var filterValueFields = $(formId + ' input[name=' + componentId +'filterValue]');
    var input = '';
    for (var i = 0; i < filterValueFields.length; i++) {
        if (i == filterValueFields.length - 1) {
            input += filterValueFields[i].value;
        }
    }
    var result = delWhiteSpaceFromExtremes(input);

    for (var i = 0; i < filterColumnFields.length; i++) {
        if (i > 0) {
            query += ':';
        }

        query += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + result);
    }
    $('#searchValue').val('');


    var ix = $("#" + componentId + "filterColumn").prop("selectedIndex");

    var value = $("#" + componentId + "filterColumn option:eq(" + ix + ")").text();

    var ix2 = $("#" + componentId + "filterOperator88").prop("selectedIndex");

    var value2 = $("#" + componentId + "filterOperator88 option:eq(" + ix2 + ")").text();
    if (document.getElementById(componentId + "SelectedOption") != null) {
        if (result != '') {
            document.getElementById(componentId + "SelectedOption").innerHTML = 'Criteria Filter: ' + value + ' ' + value2 + " '" + result + "'";
        } else {
            document.getElementById(componentId + "SelectedOption").innerHTML = '';
        }
    }
    $('#' + componentId + 'filterValue').val(result);
    $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    $(dialogId).dialog('close');
};

var executeWithParametersFilter = function(scope, entity, attributesOfColumnFilter, dialog) {
    var dialogForm = dialog;
    var attributes = attributesOfColumnFilter.toString().split(';');
    var typeOperation;
    var tempo;

    var componentId = scope + entity;
    var formId = '#' + componentId + 'FilterForm';
    var gridId = '#' + componentId + 'ListTable';
    var dialogId = '#' + componentId + 'FilterDialog';
    var currentUrl = $(gridId).getGridParam("url");
    var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
    var query = '&filterCriteria=';

    posentity = entity.indexOf('All');
    controller = entity.substring(0, posentity);
    if (currentUrl.indexOf('&filterCriteria') != -1) {
        var checkedall = $('#' + scope + controller + 'Checkbox').prop('checked');
        if (checkedall) {
            pos = currentUrl.indexOf('&filterCriteria');
            temp = currentUrl.substring((pos + query.length), currentUrl.length);
            query += temp + ':';
        }
    }

    var filterColumnFields = $(formId + ' select[name=' + componentId + 'filterColumn]');
    var filterOperatorFields;
    var filterValueFields;
    var ddlValueOfStandartData;
    var ix = $("#" + componentId + "filterColumn").prop("selectedIndex");
    var value = $("#" + componentId + "filterColumn option:eq(" + ix + ")").text();
    if (value.indexOf(' ') != -1) {
        var valueTempo = $("#" + componentId + "filterColumn").prop('value');
        var proper = valueTempo.split('.');
        value = proper[1];
    }
    for (var m = 0; m < attributes.length; m++) {
        if (attributes[m].indexOf(value) != -1) {
            tempo = attributes[m].split(',');
            typeOperation = tempo[0];
            m = attributes.length;
        }
    }

    if (typeOperation == 'Date') {
        filterOperatorFields = $(formId + ' select[name=filterOperator89]');
        filterValueFields = $(formId + ' input[name=' + componentId + 'filterValue]');
    }
    else if (typeOperation == 'Standard') {
        filterOperatorFields = $(formId + ' select[name=filterOperator90]');
        filterValueFields = $(formId + ' select[name=' + tempo[1] + ']');
        ddlValueOfStandartData = $(formId + ' select[name=' + tempo[1] + ']');
        //        var valueText = $(formId + "select option:eq(" + ix + ")").text();
    }
    else {
        filterOperatorFields = $(formId + ' select[name=filterOperator]');
        filterValueFields = $(formId + ' input[name=' + componentId + 'filterValue]');
    }
    var input = '';
    for (var i = 0; i < filterValueFields.length; i++) {
        if (i == filterValueFields.length - 1) {
            input += filterValueFields[i].value;
        }
    }



    var result = delWhiteSpaceFromExtremes(input);

    for (var i = 0; i < filterColumnFields.length; i++) {
        if (i > 0) {
            query += ':';
        }

        query += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + result);
    }
    $('#searchValue').val('');
    var ix2;
    var value2;

    if (typeOperation == 'Date') {
        ix2 = $("#" + componentId + "filterOperator89").prop("selectedIndex");
        value2 = $("#" + componentId + "filterOperator89 option:eq(" + ix2 + ")").text();
    }
    else if (typeOperation == 'Standard') {
        ix2 = $("#" + componentId + "filterOperator90").prop("selectedIndex");
        value2 = $("#" + componentId + "filterOperator90 option:eq(" + ix2 + ")").text();
    }
    else {
        ix2 = $("#" + componentId + "filterOperator88").prop("selectedIndex");
        value2 = $("#" + componentId + "filterOperator88 option:eq(" + ix2 + ")").text();
    }

    var result2 = result;

    if (typeOperation == 'Standard') {
        var indexSelectedFV = ddlValueOfStandartData[0].selectedIndex;
        var txtValue = ddlValueOfStandartData[0].options[indexSelectedFV].text.toString().replace(/</g, "&lt;");
        //input = txtValue;
        result2 = txtValue;
    }


    if (document.getElementById(componentId + "SelectedOption") != null) {
        if (result != '') {
            document.getElementById(componentId + "SelectedOption").innerHTML = 'Criteria Filter: ' + value + ' ' + value2 + " '" + result2 + "'";
        } else {
            document.getElementById(componentId + "SelectedOption").innerHTML = '';
        }
    }

    if (typeOperation == 'Standard') {
        $('#' + componentId + tempo[1] + '_' + tempo[2]).prop('value', result);
    }
    else {
        $('#' + componentId + 'filterValue').val(result);
    }
    $('#for' + componentId + 'filterValue').css("display", "none");
    $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    $(dialogId).dialog('close');
    if (dialogForm.parentNode != undefined) {
        var filterDialog = dialogForm.parentNode;
        if (filterDialog.parentNode != undefined) {
            var filterContent = filterDialog.parentNode;
            if (filterContent.style.display == 'block') {
                filterContent.style.display = 'none';
            }
        }
    }
};


    var executeFilterReport = function(formId) {
        var filterCriteria = '';
        var filterColumnFields = $(formId + ' select[name=filterColumn]');
        var filterOperatorFields = $(formId + ' select[name=filterOperator]');
        var filterValueFields = $(formId + ' input[name=filterValue]');

        for(var i = 0; i < filterColumnFields.length; i++) {
            if (i > 0) {
                filterCriteria += ':';
            }

            filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + filterValueFields[i].value);
        }

        return filterCriteria;
    };

    var executeEventFilterReport = function(formId, properties, propertiesHidden) {
        var filterCriteria = '';
        //Normal
        var filterColumnFields = $(formId + ' select[name=filterColumn]');
        var filterOperatorFields = $(formId + ' select[name=filterOperator]');
        var filterValueFields = $(formId + ' input[name=filterValue]');
        /////


        //TimeFrame->StartIntervalDate
        var filterColumnStartIntervalDateFields = $(formId + ' select[name=filterColumnStartIntervalDateField]');
        var filterOperatorStartIntervalDateFields = $(formId + ' select[name=filterOperatorStartIntervalDateField]');
        var filterValueStartIntervalDateMFields = $(formId + ' select[name=filterValueStartIntervalDateM]');
        var filterValueStartIntervalDateQFields = $(formId + ' select[name=filterValueStartIntervalDateQ]');
        var filterValueStartIntervalDateYFields = $(formId + ' select[name=filterValueStartIntervalDateY]');
        //TimeFrame->StartDate
        var filterColumnSpecificDateFields = $(formId + ' select[name=filterColumnSpecificDateField]');
        var filterOperatorSpecificDateFields = $(formId + ' select[name=filterOperatorSpecificDateField]');
        var filterStartDateFields = $(formId + ' input[name=filterValueStartDate]');
        //TimeFrame->EndDate
        var filterColumnStartEndDateFields = $(formId + ' select[name=filterColumnStartEndDateField]');
        var filterOperatorStartEndDateFields = $(formId + ' select[name=filterOperatorStartEndDateField]');
        var filterEndDateFields = $(formId + ' input[name=filterValueEndDate]');
        var columns = properties.toString().split(',');
        var columnsHidden = propertiesHidden.toString().split(',');
        for (var i = 0; i < filterColumnFields.length; i++) {
            if (i > 0) {
                filterCriteria += ':';
            }
            if (filterValueFields[i].style.display != 'none') {
                filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + filterValueFields[i].value);
            }
            else {
                for (var c = 0; c < columns.length; c++) {
                    var column = columns[c].split(':');
                    if (column.length == 3) {
                        var filterColumnField = filterColumnFields[i].value.split('.');
                        if (column[1] == filterColumnField[1]) {
                            var selectOperator = $(formId + ' select[name=' + column[1] + ']');
                            filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + selectOperator[i].value);
                            c = columns.length;
                            var tempo = selectOperator[i].value;
                            if ((columnsHidden.length > 0) && (selectOperator[i].value != '')) {
                                if (selectOperator[i].value == 'SE') {
                                    for (var ch = 0; ch < columnsHidden.length; ch++) {
                                        var columnHidden = columnsHidden[ch].split(':');
                                        if (column[1] == columnHidden[1]) {
                                            if (filterStartDateFields[i].value != '') {
                                                var parameters = filterColumnFields[i].value;
                                                var parameterOfFilter = parameters.split('.');
                                                //filterCriteria += ':' + filterColumnSpecificDateFields[i].value + '_' + filterOperatorSpecificDateFields[i].value + '_' + filterStartDateFields[i].value;
                                                //filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_Ge_' + filterStartDateFields[i].value;
                                                filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_' + filterOperatorSpecificDateFields[i].value + '_' + filterStartDateFields[i].value;
                                            }
                                            if (filterEndDateFields[i].value != '') {
                                                var parameters = filterColumnFields[i].value;
                                                var parameterOfFilter = parameters.split('.');
                                                //filterCriteria += ':' + filterColumnStartEndDateFields[i].value + filterOperatorStartEndDateFields[i].value + filterEndDateFields[i].value;
                                                //filterCriteria += ':' + parameterOfFilter[0] + '.EndDate_Le_' + filterEndDateFields[i].value;
                                                filterCriteria += ':' + parameterOfFilter[0] + '.EndDate_' + filterOperatorStartEndDateFields[i].value + '_' + filterEndDateFields[i].value;
                                            }
                                            //ch = columnsHidden.length;
                                        }
                                    }
                                }
                                else if (selectOperator[i].value == 'D') {
                                    for (var sh = 0; sh < columnsHidden.length; sh++) {
                                        var columnHidden = columnsHidden[sh].split(':');
                                        if (column[1] == columnHidden[1]) {
                                            if (filterStartDateFields[i].value != '') {
                                                var parameters = filterColumnFields[i].value;
                                                var parameterOfFilter = parameters.split('.');
                                                //filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_Le_' + filterStartDateFields[i].value;
                                                //filterCriteria += ':' + filterColumnSpecificDateFields[i].value + filterOperatorSpecificDateFields[i].value + filterStartDateFields[i].value;
                                                filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_' + filterOperatorSpecificDateFields[i].value + '_' + filterStartDateFields[i].value;
                                            }
                                            ch = columnsHidden.length;
                                        }
                                    }
                                }
                                else if (selectOperator[i].value == 'Q') {
                                    for (var sh = 0; sh < columnsHidden.length; sh++) {
                                        var columnHidden = columnsHidden[sh].split(':');
                                        if (column[1] == columnHidden[1]) {
                                            if (filterValueStartIntervalDateQFields[i].value != '') {
                                                var parameters = filterColumnFields[i].value;
                                                var parameterOfFilter = parameters.split('.');
                                                //filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_Le_' + filterStartDateFields[i].value;
                                                //filterCriteria += ':' + filterColumnStartIntervalDateFields[i].value + filterOperatorStartIntervalDateFields[i].value + filterValueStartIntervalDateQFields[i].value;

                                                filterCriteria += ':' + parameterOfFilter[0] + '.StartIntervalDate_' + filterOperatorStartIntervalDateFields[i].value + '_' + filterValueStartIntervalDateQFields[i].value;
                                            }
                                            ch = columnsHidden.length;
                                        }
                                    }
                                }
                                else if (selectOperator[i].value == 'M') {
                                    for (var sh = 0; sh < columnsHidden.length; sh++) {
                                        var columnHidden = columnsHidden[sh].split(':');
                                        if (column[1] == columnHidden[1]) {
                                            if (filterValueStartIntervalDateMFields[i].value != '') {
                                                var parameters = filterColumnFields[i].value;
                                                var parameterOfFilter = parameters.split('.');
                                                //filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_Le_' + filterStartDateFields[i].value;
                                                //filterCriteria += ':' + filterColumnStartIntervalDateFields[i].value + filterOperatorStartIntervalDateFields[i].value + filterValueStartIntervalDateMFields[i].value;
                                                filterCriteria += ':' + parameterOfFilter[0] + '.StartIntervalDate_' + filterOperatorStartIntervalDateFields[i].value + '_' + filterValueStartIntervalDateMFields[i].value;
                                            }
                                            ch = columnsHidden.length;
                                        }
                                    }
                                }
                                else if (selectOperator[i].value == 'Y') {
                                    for (var sh = 0; sh < columnsHidden.length; sh++) {
                                        var columnHidden = columnsHidden[sh].split(':');
                                        if (column[1] == columnHidden[1]) {
                                            if (filterValueStartIntervalDateYFields[i].value != '') {
                                                var parameters = filterColumnFields[i].value;
                                                var parameterOfFilter = parameters.split('.');
                                                //filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_Le_' + filterStartDateFields[i].value;
                                              //filterCriteria += ':' + filterColumnStartIntervalDateFields[i].value + filterOperatorStartIntervalDateFields[i].value + filterValueStartIntervalDateYFields[i].value;
                                                filterCriteria += ':' + parameterOfFilter[0] + '.StartIntervalDate_' + filterOperatorStartIntervalDateFields[i].value + '_' + filterValueStartIntervalDateYFields[i].value;
                                            }
                                            ch = columnsHidden.length;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        return filterCriteria;
    };
    var executeHiddenColumnReport = function(formId) {
        var hiddenCriteria = '';
        var filterColumnFields = $(formId + ' select[name=filterColumn]');
        var filterOperatorFields = $(formId + ' select[name=filterOperator]');
        var filterValueFields = $(formId + ' input[name=filterValue]');
        var filterHiddenColummns = $(formId + ' input[name=check]');
        for (var i = 0; i < filterColumnFields.length; i++) {
            if (i > 0) {
                hiddenCriteria += ':';
            }

            hiddenCriteria += (filterColumnFields[i].value + '_isVisible_' + filterHiddenColummns[i].checked);
        }

        return hiddenCriteria;
    };

    var getSelectedRowGrid = function(idGrid) {
        return $(idGrid).getGridParam('selrow');
    };
    
    var reloadGrid = function(idGrid) {
        $(idGrid).trigger('reloadGrid');
    };

    var showAllData = function(gridId,scope,controller) {
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);

        $(gridId).setGridParam({ url: urlAction, page: 1 }).trigger('reloadGrid');
        $('#searchValue').val('');
        $('#filterValue').val('');
        $("#filterColumn").val(0);
        $("#filterOperator88").val(0);
        document.getElementById(scope + controller +"AllSelectedOption").innerHTML = '';
    };

    var showAllDataTwoBrowse = function(gridId, gridIdHidden,scope, controller) {
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        $(gridId).setGridParam({ url: urlAction, page: 1 }).trigger('reloadGrid');
        var currentUrlHidden = $(gridIdHidden).getGridParam("url");
        var urlActionHidden = currentUrlHidden.substring(0, currentUrlHidden.indexOf('&eou') + 4);
        $(gridIdHidden).setGridParam({ url: urlActionHidden, page: 1 }).trigger('reloadGrid');
        $('#searchValue').val('');
        $('#filterValue').val('');
        $("#filterColumn").val(0);
        $("#filterOperator88").val(0);
        document.getElementById('#'+scope + controller + "SelectedOption").innerHTML = '';
    };
    
    var toggleColumnPopup = function(columnPopupId){ 
    
        var displayValue = $(columnPopupId).css('display');
        
        if (displayValue == 'none') {
            
            displayValue = 'inline';
            
        } else {
        
            displayValue = 'none';
        }
        
        $(columnPopupId).css('display', displayValue);
    };

    var toggleColumnView = function(gridId, columnName, component, userId, url) {
        if (component.checked) {
            $(gridId).showCol(columnName);
            $('#datatmp').data(gridId + columnName + 'E', '');
            saveConfig(gridId + columnName + 'E', '', userId, url);
        } else {
            $(gridId).hideCol(columnName);
            $('#datatmp').data(gridId + columnName + 'E', 'hidden');
            $('#datatmp').data(gridId + columnName + 'C', component.id);
            saveConfig(gridId + columnName + 'E', 'hidden', userId, url);
            saveConfig(gridId + columnName + 'C', component.id, userId, url);
        }
        $(gridId).jqGrid('setGridWidth', Math.round($(window).width() * 0.98));

    };

    var checkHidden = function(gridId, columnName) {

            if ($('#datatmp').data(gridId + columnName + 'E') == 'hidden') {
                $(gridId).hideCol(columnName);
                $('#' + $('#datatmp').data(gridId + columnName + 'C')).prop('checked', false);
            }
        
    };

    var saveWidth = function(gridId, newwidth, index, userId, url) {

        var a = jQuery(gridId).jqGrid('getGridParam', 'colModel');
        $('#datatmp').data(gridId + a[index].name + 'W', newwidth);
        saveConfig(gridId + a[index].name + 'W', newwidth, userId, url);
    };

    var showMyEntities = function(invoke, scope, entity, entityall, assignedTo, userId) {
        var componentId = scope + entity;
        var formId = '#' + componentId + 'FilterForm';
        var gridId = '#' + componentId + 'ListTable';
        var dialogId = '#' + componentId + 'FilterDialog';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        var query = '&filterCriteria=';

        if (invoke.checked) {
            if (currentUrl.indexOf('&searchCriteria') != -1) {//si tiene searchcriteria
                posSearch = currentUrl.indexOf('&searchCriteria');
                posFilter = currentUrl.indexOf('&filterCriteria');
                if (currentUrl.indexOf('&filterCriteria') != -1) {
                    if (posFilter < posSearch) {
                        temp = currentUrl.substring(posFilter + query.length, posSearch - 1);
                        query += temp + ':';
                        query += (entityall + '.' + assignedTo + '_' + 'Eq' + '_' + userId);
                        $('#filterValue').val('');
                        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                        $(dialogId).dialog('close');
                    }
                    else {
                        temp = currentUrl.substring(posSearch, currentUrl.length);
                        query += temp + ':';
                        query += (entityall + '.' + assignedTo + '_' + 'Eq' + '_' + userId);
                        $('#filterValue').val('');
                        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                        $(dialogId).dialog('close');
                    }

                }
                else {
                    temp = currentUrl.substring(posSearch, currentUrl.length);
                    query = temp + query;
                    query += (entityall + '.' + assignedTo + '_' + 'Eq' + '_' + userId);
                    $('#filterValue').val('');
                    $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                    $(dialogId).dialog('close');
                }
            }
            else {
                if (currentUrl.indexOf('&filterCriteria') != -1) {
                    pos = currentUrl.indexOf('&filterCriteria');
                    temp = currentUrl.substring((pos + query.length), currentUrl.length);
                    query += temp + ':';
                    query += (entityall + '.' + assignedTo + '_' + 'Eq' + '_' + userId);
                }
                else {

                    query += (entityall + '.' + assignedTo + '_' + 'Eq' + '_' + userId);
                }
                $('#filterValue').val('');
                $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                $(dialogId).dialog('close');
            }
        }

        else // unchecked
        {
            if (currentUrl.indexOf(':') != -1) //1 unchecked + other filter
            {
                posAnd = currentUrl.indexOf(':');
                posFilterChecked = currentUrl.indexOf(assignedTo);
                if (currentUrl.indexOf('&searchCriteria') != -1) //2
                {
                    posSearch = currentUrl.indexOf('&searchCriteria');
                    posFilter = currentUrl.indexOf('&filterCriteria');
                    if (posSearch > posFilter) //3
                    {

                        if (posAnd > posFilterChecked) //4
                        {
                            tempQuery = currentUrl.substring(posAnd + 1, currentUrl.length);
                            query += tempQuery;
                            $('#filterValue').val('');
                            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                            $(dialogId).dialog('close');
                        }
                        else {//4
                            tempQuery = currentUrl.substring(posFilter + 1, posAnd) + currentUrl.substring(posSearch + 1, currentUrl.length);
                            query += tempQuery;
                            $('#filterValue').val('');
                            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                            $(dialogId).dialog('close');
                        }
                    }
                    else {//3
                        if (posAnd > posFilterChecked) {
                            tempQuery = currentUrl.substring(posSearch, posFilter) + query + currentUrl.substring(posAnd + 1, currentUrl.length);
                            query = tempQuery;
                            $('#filterValue').val('');
                            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                            $(dialogId).dialog('close');
                        }
                        else {
                            tempQuery = currentUrl.substring(posSearch, posAnd);
                            query = tempQuery;
                            $('#filterValue').val('');
                            $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                            $(dialogId).dialog('close');

                        }
                    }
                    //                    tempSearch = currentUrl.substring(posSearch, currentUrl.length);
                    //                    query += tempSearch;
                }
                else {//2 no tienen searchcritearia

                    if (posAnd > posFilterChecked) {
                        tempQuery = currentUrl.substring(posAnd + 1, currentUrl.length);
                        query = '&filterCriteria=' + tempQuery;
                        $('#filterValue').val('');
                        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                        $(dialogId).dialog('close');
                    }
                    else {
                        posCriteria = currentUrl.indexOf('&filterCriteria=');
                        tempQuery = currentUrl.substring(posCriteria + 16, posAnd);
                        query = '&filterCriteria=' + tempQuery;
                        $('#filterValue').val('');
                        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                        $(dialogId).dialog('close');
                    }

                }
            }
            else {//1 unchecked whitout other filter
                if (currentUrl.indexOf('&searchCriteria') != -1) {
                    posFilter = currentUrl.indexOf('&filterCriteria');
                    posSearch = currentUrl.indexOf('&searchCriteria');
                    tempSearch = currentUrl.substring(posSearch, posFilter);
                    query = tempSearch;
                    $('#filterValue').val('');
                    $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
                    $(dialogId).dialog('close');
                }
                else {
                    $('#filterValue').val('');
                    $(gridId).setGridParam({ url: urlAction, page: 1 }).trigger('reloadGrid');
                    $(dialogId).dialog('close');
                }
            }

        }
    };
    
    var unCheckMyEntities = function(grid, scope, controller) {
    var checkedall = $('#' + scope + controller + 'Checkbox').prop('checked');
        var checkedall2 = $('#' + scope + controller + 'Checkbox');
        if (checkedall) {
            checkedall2[0].checked = false;
        }
    };

    var CleanFilterValue = function(form,scope, browseId) {
        var componentId = scope + browseId;
        var forfiltervalue = $('#for' + componentId + 'filterValue');
        var filtervalue = $('#' + componentId + 'filterValue');
        forfiltervalue[0].innerHTML = "";
        filtervalue.focus();
        forfiltervalue.css("display", "none");
            form.value = "";
    };

    var validatorFilterValue = function(selected, scope, browseId, property) {
        var componentId = scope + browseId;
        var filtercolumn = $('#' + componentId + 'filterColumn');
        var filtervalue = $('#' + componentId + 'filterValue');
        var forfiltervalue = $('#for' + componentId + 'filterValue');
        var isIncorrectFormat = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        var alphaExp = /^[-_0-9a-zA-Z\s\.\,\:\;]+$/;
        var message;
        var isDate = false;
        if (property.toString().length > 0) {
            var properties = property.toString().split(';');
            for (var k = 0; k < properties.length; k++) {
                var value = browseId + 'View.' + properties[k];
                if (filtercolumn[0].value == value) {
                    isDate = true;

                }
                else {
                    if ((filtercolumn[0].value.indexOf('.') != -1) && (value.indexOf('.') != -1)) {
                        var firstValue = filtercolumn[0].value.substring(filtercolumn[0].value.indexOf('.'));
                        var secondValue = value.substring(value.indexOf('.'));
                        if (firstValue == secondValue) {
                            isDate = true;
                        }
                    }
                }
            }
        }

        if (isDate) {
            if (filtervalue[0].value != "") {
                if (!filtervalue[0].value.match(re)) {
                    isIncorrectFormat = true;
                    message = "Date should be in the format: mm/dd/YYYY"
                }
                else {
                    var date = filtervalue[0].value.split('/');
                    var year = parseInt(date[2]);
                    var day = parseInt(date[1]);
                    var month = parseInt(date[0]);
                    var dayobj = null;
                    dayobj = new Date(year, month - 1, day)
                    var monthField = dayobj.getMonth();
                    if ((monthField + 1 != month) || (dayobj.getDate() != day) || (dayobj.getFullYear() != year)) {
                        isIncorrectFormat = true;
                        message = "Invalid Day, Month, or Year";
                    }
                }

            }
        }
        else {
            var value = filtervalue[0].value;
            if (value.length > 0) {
                if (!filtervalue[0].value.match(alphaExp)) {
                    isIncorrectFormat = true;
                    message = "Expression incorrect"
                }
            }
        }

        if (isIncorrectFormat) {
            forfiltervalue[0].innerHTML = message;
            filtervalue.focus();
            forfiltervalue.css("display", "block");
        }
    };

    var validatorDate = function(selected, scope, browseId, property) {
        var properties = property.toString().split(';');
        var componentId = scope + browseId;
        var filtercolumn = $('#' + componentId + 'filterColumn');
        var filtervalue = $('#' + componentId + 'filterValue');
        var forfiltervalue = $('#for' + componentId + 'filterValue');
        var isIncorrectFormat = false;
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        var message;
        for (var k = 0; k < properties.length; k++) {
            var value = browseId + 'View.' + properties[k];
            if (filtercolumn[0].value == value) {
                if (filtervalue[0].value != "") {
                    if (!filtervalue[0].value.match(re)) {
                        isIncorrectFormat = true;
                        message = "Date should be in the format: mm/dd/YYYY"
                    }
                    else {
                        var date = filtervalue[0].value.split('/');
                        var year = parseInt(date[2]);
                        var day = parseInt(date[1]);
                        var month = parseInt(date[0]);
                        var dayobj = new Date(year, month - 1, day)
                        var monthField = dayobj.getMonth();
                        if ((monthField + 1 != month) || (dayobj.getDate() != day) || (dayobj.getFullYear() != year)) {
                            isIncorrectFormat = true;
                            message = "Invalid Day, Month, or Year";
                        }
                    }
                    if (isIncorrectFormat) {
                        forfiltervalue[0].innerHTML = message;
                        filtervalue.focus();
                        forfiltervalue.css("display", "block");
                    }
                }
            }
        }
    };

    var resetFilterOperator = function(formid, scope, browseId, attributes) {
        var componentId = scope + browseId;
        var filterOperator = $(formid + 'filterOperator88');
        $('#' + componentId + 'filterOperator88').css("display", "block");
        $('#' + componentId + 'filterOperator89').css("display", "none");
        $('#' + componentId + 'filterOperator90').css("display", "none");
        var filtervalue = $('#' + componentId + 'filterValue');
        filtervalue.css("display", "block");
        var forfiltervalue = $('#for' + componentId + 'filterValue');
        forfiltervalue.css("display", "none");
        forfiltervalue[0].innerHTML = "";
        var properties = attributes.toString();
        var splitProperties = properties.split(';');

        for (var p = 0; p < splitProperties.length; p++) {
            if (splitProperties[p].indexOf('Standard') != -1) {
                var itemOfProperties = splitProperties[p].split(',');
                $('#' + componentId + itemOfProperties[1] + '_' + itemOfProperties[2]).css("display", "none");
            }
        }
    };

    var changeOptions = function(selected, browseId, property, properties, propertiesWithType) {
        var filteroperator = $('#filterOperator88');
        var filteroperator89 = $('#filterOperator89');
        var filteroperator90 = $('#filterOperator90');
        var filtervalue = $('#filterValue');


        var properties2 = properties.toString();
        var properties3 = properties2.split(';');

        var propertiesSelected = selected.toString().split('.');

        var propertiesWithTypeString = propertiesWithType.toString();
        var splitProperties = propertiesWithTypeString.split(';');
        if (propertiesWithTypeString.indexOf(propertiesSelected[1]) != -1) {
            for (var k = 0; k < splitProperties.length; k++) {
                var tempo = splitProperties[k];
                if (tempo.indexOf(propertiesSelected[1]) != -1) {
                    var tempo2 = splitProperties[k].split(',');
                    if (tempo2[0] == 'Date') {
                        filteroperator.css("display", "none");
                        filteroperator89.css("display", "block");
                        filteroperator90.css("display", "none");
                        filtervalue.css("display", "block");
                        for (var q = 0; q < splitProperties.length; q++) {
                            if (splitProperties[q].indexOf('Standard') != -1) {
                                var itemOfProperties = splitProperties[q].split(',');
                                $('#' + itemOfProperties[1] + '_' + itemOfProperties[2]).css("display", "none");
                            }

                        }
                        var forfiltervalue = $('#forfilterValue');
                        forfiltervalue[0].innerHTML = "";
                    }
                    else if (tempo2[0] == 'Standard') {
                        $('#filterOperator88').css("display", "none");
                        $('#filterOperator89').css("display", "none");
                        $('#filterOperator90').css("display", "block");
                        var selectOption = $('#' + tempo2[1] + '_' + tempo2[2]);
                        selectOption.css("display", "block");
                        filtervalue.css("display", "none");
                        for (var q = 0; q < splitProperties.length; q++) {
                            if ((splitProperties[q].indexOf('Standard') != -1) && (splitProperties[q].indexOf(tempo2[1]) == -1)) {
                                var itemOfProperties = splitProperties[q].split(',');
                                $('#' + itemOfProperties[1] + '_' + itemOfProperties[2]).css("display", "none");
                            }

                        }
                        var forfiltervalue = $('#forfilterValue');
                        forfiltervalue[0].innerHTML = "";
                    }
                }
            }

        }
        else {
            $('#filterOperator88').css("display", "block");
            filtervalue.css("display", "block");
            $('#filterOperator89').css("display", "none");
            $('#filterOperator90').css("display", "none");
            for (var p = 0; p < splitProperties.length; p++) {
                if (splitProperties[p].indexOf('Standard') != -1) {
                    var itemOfProperties = splitProperties[p].split(',');
                    $('#' + itemOfProperties[1] + '_' + itemOfProperties[2]).css("display", "none");
                }

            }
            var forfiltervalue = $('#forfilterValue');
            forfiltervalue[0].innerHTML = "";
        }
    };

    var changeOptionsWPrope = function(selected, browseId, property, properties, propertiesWithType, scope) {
        var componentId = scope + browseId;
        var filteroperator = $('#' + componentId + 'filterOperator88');
        var filteroperator89 = $('#' + componentId + 'filterOperator89');
        var filteroperator90 = $('#' + componentId + 'filterOperator90');
        var filtervalue = $('#' + componentId + 'filterValue');

        var propertiesSelected = selected.toString().split('.');

        var propertiesWithTypeString = propertiesWithType.toString();
        var splitProperties = propertiesWithTypeString.split(';');
        if (propertiesWithTypeString.indexOf(propertiesSelected[1]) != -1) {
            for (var k = 0; k < splitProperties.length; k++) {
                var tempo = splitProperties[k];
                if (tempo.indexOf(propertiesSelected[1]) != -1) {
                    var tempo2 = splitProperties[k].split(',');
                    if (tempo2[0] == 'Date') {
                        filteroperator.css("display", "none");
                        filteroperator89.css("display", "block");
                        filteroperator90.css("display", "none");
                        filtervalue.css("display", "block");
                        k = splitProperties.length;
                        for (var q = 0; q < splitProperties.length; q++) {
                            if (splitProperties[q].indexOf('Standard') != -1) {
                                var itemOfProperties = splitProperties[q].split(',');
                                $('#' + componentId + itemOfProperties[1] + '_' + itemOfProperties[2]).css("display", "none");
                            }

                        }
                    }
                    else if (tempo2[0] == 'Standard') {
                        filteroperator.css("display", "none");
                        filteroperator89.css("display", "none");
                        filteroperator.css("height", "0px"); //SP nadie?
                        //filteroperator89.css("height", "0px"); //Sp
                        filteroperator90.css("display", "block");                      
                        var selectOption = $('#' + componentId + tempo2[1] + '_' + tempo2[2]);
                        selectOption.css("display", "block");
                        filtervalue.css("display", "none"); //problema
                        /*filtervalue.css("height", "0px");*/ //edit
                        k = splitProperties.length;                      
                        for (var q = 0; q < splitProperties.length; q++) {
                            if ((splitProperties[q].indexOf('Standard') != -1) && (splitProperties[q].indexOf(tempo2[1]) == -1)) {
                                var itemOfProperties = splitProperties[q].split(',');
                                $('#' + componentId + itemOfProperties[1] + '_' + itemOfProperties[2]).css("display", "none");
                            }

                        }
                    }
                }
            }

        }
        else {
            filteroperator.css("display", "block");
            filteroperator.css("height", "20px");
            filtervalue.css("display", "block");
            filtervalue.css("height", "16px");
            filteroperator89.css("display", "none");
            filteroperator90.css("display", "none");
            for (var p = 0; p < splitProperties.length; p++) {
                if (splitProperties[p].indexOf('Standard') != -1) {
                    var itemOfProperties = splitProperties[p].split(',');
                    $('#' + componentId + itemOfProperties[1] + '_' + itemOfProperties[2]).css("display", "none");
                }
            }
        }
        var forfiltervalue = $('#for' + componentId + 'filterValue');
        forfiltervalue[0].innerHTML = "";
    };

    var executeSpecialFilterReport = function(formId, properties) {
        var filterCriteria = '';
        var filterColumnFields = $(formId + ' select[name=filterColumn]');
        var filterOperatorFields = $(formId + ' select[name=filterOperator]');
        var filterValueFields = $(formId + ' input[name=filterValue]');
        var filterSecondValueFields = $(formId + ' input[name=filterSecondValue]');
        var columns = properties.toString().split(',');
        //var columnsHidden = propertiesHidden.toString().split(',');
        for (var i = 0; i < filterColumnFields.length; i++) {
            if (i > 0) {
                filterCriteria += ':';
            }
            if (filterValueFields[i].style.display != 'none') {
                if (filterOperatorFields[i].value == 'Bt' || filterOperatorFields[i].value == 'bt') {
                    filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + filterValueFields[i].value + '_And_' + filterSecondValueFields[i].value);
                }
                else {

                    filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + filterValueFields[i].value);
                }
            }
            //            else {
            //                for (var c = 0; c < columns.length; c++) {
            //                    var column = columns[c].split(':');
            //                    if (column.length == 3) {
            //                        var filterColumnField = filterColumnFields[i].value.split('.');
            //                        if (column[1] == filterColumnField[1]) {
            //                            var selectOperator = $(formId + ' select[name=' + column[1] + ']');
            //                            filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + selectOperator[i].value);
            //                            c = columns.length;
            //                            var tempo = selectOperator[i].value;
            //                            if ((columnsHidden.length > 0) && (selectOperator[i].value != '')) {
            //                                if (selectOperator[i].value == 'SE') {
            //                                    for (var ch = 0; ch < columnsHidden.length; ch++) {
            //                                        var columnHidden = columnsHidden[ch].split(':');
            //                                        if (column[1] == columnHidden[1]) {
            //                                            if (filterStartDateFields[i].value != '') {
            //                                                var parameters = filterColumnFields[i].value;
            //                                                var parameterOfFilter = parameters.split('.');
            //                                                filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_Ge_' + filterStartDateFields[i].value;
            //                                            }
            //                                            if (filterEndDateFields[i].value != '') {
            //                                                var parameters = filterColumnFields[i].value;
            //                                                var parameterOfFilter = parameters.split('.');
            //                                                filterCriteria += ':' + parameterOfFilter[0] + '.EndDate_Le_' + filterEndDateFields[i].value;
            //                                            }
            //                                            ch = columnsHidden.length;
            //                                        }
            //                                    }
            //                                }
            //                                else {
            //                                    for (var sh = 0; sh < columnsHidden.length; sh++) {
            //                                        var columnHidden = columnsHidden[sh].split(':');
            //                                        if (column[1] == columnHidden[1]) {
            //                                            if (filterStartDateFields[i].value != '') {
            //                                                var parameters = filterColumnFields[i].value;
            //                                                var parameterOfFilter = parameters.split('.');
            //                                                filterCriteria += ':' + parameterOfFilter[0] + '.StartDate_Le_' + filterStartDateFields[i].value;
            //                                            }
            //                                            ch = columnsHidden.length;
            //                                        }
            //                                    }
            //                                }
            //                            }

            //                        }
            //                    }
            //                }
            //            }
        }
        return filterCriteria;
    };

    var UpdateCheckBoxToggleColumn = function(checkedId, component) {
        var checkItem = $('#' + checkedId);
        if (component.checked) {
            $('#' + checkedId).prop('checked', true);
        } else {
        $('#' + checkedId).prop('checked', false);
        }
    };

    var executeFilterReportForm2 = function(formId, properties) {
        var filterCriteria = '';
        var filterColumnFields = $(formId + ' select[name=filterColumn]');
        var filterOperatorFields = $(formId + ' select[name=filterOperator]');
        var filterValueFields = $(formId + ' input[name=filterValue]');
        var filterSecondValueFields = $(formId + ' input[name=filterSecondValue]');
        var columns = properties.toString().split(',');
        for (var i = 0; i < filterColumnFields.length; i++) {
            if (i > 0) {
                filterCriteria += ':';
            }
            if (filterValueFields[i].style.display != 'none') {
                if (filterOperatorFields[i].value == 'Bt' || filterOperatorFields[i].value == 'bt') {
                    filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + filterValueFields[i].value + '_And_' + filterSecondValueFields[i].value);
                }
                else {

                    filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + filterValueFields[i].value);
                }
            }
            else {
                 for (var c = 0; c < columns.length; c++) {
                     var column = columns[c].split(':');
                     if (column.length == 3) {
                         var filterColumnField = filterColumnFields[i].value.split('.');
                         if (column[1] == filterColumnField[1]) {
                             var selectOperator = $(formId + ' select[name=' + column[1] + ']');
                             filterCriteria += (filterColumnFields[i].value + '_' + filterOperatorFields[i].value + '_' + selectOperator[i].value);
                             c = columns.length;
                             var tempo = selectOperator[i].value;
                                   }
                               }
                           }
                       }
        }
        return filterCriteria;
    };

    var RefreshGridId = function(scope, entity) {
        var componentId = scope + entity;
        var gridId = '#' + componentId + 'ListTable';
        var currentUrl = $(gridId).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);

        $(gridId).setGridParam({ url: urlAction, page: 1 }).trigger('reloadGrid');
    };

    var savePermutation = function(permutation, tableId, userId, url) {
        var permArray = permutation;
        var patron = $('#datatmp').data(tableId + 'patron');
        var patronArray = new Array();
        if (patron == null || patron == '' || patron == 'undefined') {
            for (i = 0; i < permArray.length; i++) {
                patronArray[i] = i;
            }
        }
        else {
            patronArray = patron.split(',');
        }
        patron = patronArray[permArray[0]];
        for (i = 1; i < permArray.length; i++) {
            patron = patron + ',' + patronArray[permArray[i]];
        }
        
        $('#datatmp').data(tableId + 'patron', patron);
        $('#datatmp').data(tableId + 'perm', patron);

        saveConfig(tableId + 'patron', patron, userId, url);
        saveConfig(tableId + 'perm', patron, userId, url);
    };

    var saveConfig = function(config, value, userId, url) {

        $.post(url,
                {userId: userId, config: config, value: value},
                function(data) {
                    //alert(data.name);
                }
          );
    };

    var loadConfig = function(url) {

        $.get(url,
               null,
                function(data) {
                    var myObject = eval('(' + data + ')');

                    for (i in myObject) {
                        if (i != 'remove') {
                            $('#datatmp').data(myObject[i]["config"], myObject[i]["value"]);
                        }
                    }
                }
          );
    };
