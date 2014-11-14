// This is the only public method.  Initialised like:
// jQuery (#tableid).tableFilter(options)
(function(jQuery) {
    jQuery.fn.tableFilter = function(_options) {

        var lastkeytime;
        var lastTimerID;
        var grid;
        var cancelQuickFind;
        var filterJar;
        var columnsToIgnore;

        // Cached controls
        var filters;
        var headers;
        var rows;

        var options = jQuery.extend(jQuery.fn.tableFilter.defaults, _options);

        this.each(function() {
            grid = jQuery(this);
            initialiseFilters();
        });

        function initialiseFilters() {
            filterJar = jQuery.cookieJar(grid.prop('id') + '_filters');
            initialiseControlCaches();
            columnsToIgnore = headers.length - filters.length;
            registerListenersOnFilters();
            setFilterWidths();
            loadFiltersFromCookie();
        }

        function registerListenersOnFilters() {
            filters.keyup(onTableFilterChanged);
            if (options.clearFiltersControls) {
                for (var i = 0; i < options.clearFiltersControls.length; i++) {
                    options.clearFiltersControls[i].click(function() {
                        clearAllFilters();
                        return false;
                    });
                }
            }
            if (!options.additionalFilterTriggers) return;
            for (var i = 0; i < options.additionalFilterTriggers.length; i++) {
                var f = options.additionalFilterTriggers[i];
                switch (f.prop('type')) {
                    case 'text':
                        f.prop('title', jQuery.fn.tableFilter.filterToolTipMessage);
                        f.keyup(onTableFilterChanged);
                        break;
                    case 'checkbox':
                        f.click(onTableFilterChanged);
                        break;
                    default:
                        throw 'Filter type ' + f.prop('type') + ' is not supported';
                }
            }
        }

        function clearAllFilters() {
            filters.val('');
            if (options.additionalFilterTriggers) {
                for (var i = 0; i < options.additionalFilterTriggers.length; i++) {
                    var f = options.additionalFilterTriggers[i];
                    switch (f.prop('type')) {
                        case 'text':
                            f.val('');
                            break;
                        case 'checkbox':
                            f.prop('checked', false);
                            break;
                        default:
                            throw 'Filter type ' + f.prop('type') + ' is not supported';
                    }
                }
            }
            quickFindImpl();
        }

        function initialiseControlCaches() {
            headers = grid.children("thead tr:first th");
            buildFiltersRow();
            filters = grid.children("thead tr:last input");
            rows = jQuery('#' + grid.prop('id') + ' tbody tr');
        }

        function buildFiltersRow() {
            var tr = $("<tr class'filters'></tr>");
            for (var i = 0; i < headers.length; i++) {
                var headerText = $(headers[i]).text();
                var td = $(headerText.length > 1
                    ? "<td><input type='text' id='filter_" + i + "' class='filter' title='" + jQuery.fn.tableFilter.filterToolTipMessage + "'/></td>"
                    : "<td>&nbsp;</td>");
                tr.append(td);
            }
            grid.children("thead").append(tr);
        }

        function loadFiltersFromCookie() {
            var filterState = filterJar.get('val');
            applyFilterStates(filterState, true);
        }

        function setFilterWidths() {
            for (var i = 0; i < filters.length; i++) {
                var f = jQuery(filters[i]);
                f.width(jQuery(headers[i + columnsToIgnore]).width());
            }
        }

        function onTableFilterChanged(e) {
            lastkeytime = new Date().getTime();
            quickFindTimer();
        }

        function quickFindTimer() {
            if (lastTimerID) { clearTimeout(lastTimerID); }
            cancelQuickFind = true;

            var curtime = new Date().getTime();
            if (curtime - lastkeytime >= options.filterDelay) {
                quickFindImpl();
            } else {
                lastTimerID = setTimeout(quickFindTimer, options.filterDelay / 3);
            }
        }

        // TODO: Can this be optimised??
        function quickFindImpl() {
            cancelQuickFind = false;
            clearTimeout(lastTimerID);
            var filterStates = getFilterStates();
            applyFilterStates(filterStates, false);
            saveFiltersToCookie(filterStates);
        }

        function getFilterStates() {
            var filterStates = new Array();

            for (var i = 0; i < filters.length; i++) {
                var state = getFilterStateForFilter(jQuery(filters[i]));
                if (state) filterStates[filterStates.length] = state;
            }

            if (!options.additionalFilterTriggers) return filterStates;

            for (var i = 0; i < options.additionalFilterTriggers.length; i++) {
                var state = getFilterStateForFilter(options.additionalFilterTriggers[i]);
                if (state) filterStates[filterStates.length] = state;
            }
            return filterStates;
        }

        function getFilterStateForFilter(filter) {
            var type = filter.prop('type');
            var value;
            switch (type) {
                case 'text':
                    value = filter.val().toLowerCase();
                    break;
                case 'checkbox':
                    value = filter.prop('checked');
                    break;
                default:
                    throw 'Filter type ' + type + ' is not supported';
            }
            if (value == null || value.length <= 0) { return null; }
            var idx = getColumnIndexOfCurrentFilter(filter);
            return { id: filter.prop('id'), value: value, idx: idx, type: filter.prop('type') };
        }

        function saveFiltersToCookie(filterState) {
            filterJar.set('val', filterState);
        }

        function applyFilterStates(filterStates, setValueOnFilter) {
            clearRowFilteredStates();
            if ((!filterStates || filterStates.length) == 0 && (options.matchingRow == null || options.matchingCell)) {
                hideRowsThatDoNotMatchAnyFiltres();
                return;
            }
            if (filterStates == null || filterStates.length == 0) { applyStateToRows(null); }
            else {
                for (var i = 0; i < filterStates.length; i++) {
                    var state = filterStates[i];
                    if (setValueOnFilter && state.type) {
                        switch (state.type) {
                            case 'text':
                                jQuery('#' + state.id).val(state.value);
                                break;
                            case 'checkbox':
                                jQuery('#' + state.id).prop('checked', state.value == true);
                                break;
                            default:
                                throw 'Filter type ' + state.type + ' is not supported';
                        }
                    }
                    applyStateToRows(state);
                }
            }

            hideRowsThatDoNotMatchAnyFiltres();
        }

        function clearRowFilteredStates() {
            rows.removeAttr('filtermatch');
        }

        function applyStateToRows(filterState) {
            var normalisedTokens = filterState == null || filterState.type != 'text' ? null : parseSearchTokens(filterState.value);
            var colidx = filterState == null ? -1 : filterState.idx;
            for (var i = 0; i < rows.length; i++) {
                if (cancelQuickFind) return;
                var tr = jQuery(rows[i]);
                if (tr.prop('filtermatch')) { continue; }
                if (!doesRowContainText(filterState, tr, normalisedTokens, colidx)) { tr.prop('filtermatch', 'false'); }
            }
        }

        function hideRowsThatDoNotMatchAnyFiltres() {
            for (var i = 0; i < rows.length; i++) {
                if (cancelQuickFind) return;
                var tr = jQuery(rows[i]);
                if (tr.prop('filtermatch') == 'false') {
                    tr.hide();
                }
                else {
                    tr.show();
                }
            }
        }

        function getColumnIndexOfCurrentFilter(filter) {
            var filterCell = filter.parent('td');
            if (!filterCell || filterCell.length <= 0) { return -1; }
            var filterRow = filterCell.parent();
            return filterRow.children('td').index(filterCell);
        }

        function doesRowContainText(state, tr, textTokens, columnIdx) {
            var cells = tr.children('td');
            if (columnIdx < 0) {
                for (var j = columnsToIgnore; j < cells.length; j++) {
                    var cell = jQuery(cells[j]);
                    if (doesCellContainText(state, cell, textTokens)) { return checkMatchingRowCallback(state, tr, textTokens); }
                }
                return false;
            } else { return doesCellContainText(state, jQuery(cells[columnIdx]), textTokens) && checkMatchingRowCallback(state, tr, textTokens); }
        }

        function checkMatchingRowCallback(state, tr, textTokens) {
            if (!options.matchingRow) return true;
            return options.matchingRow(state, tr, textTokens);
        }

        function doesCellContainText(state, td, textTokens) {
            var text = td.text();
            if (!doesTextMatchTokens(text, textTokens)) { return false; }
            return !options.matchingCell || options.matchingCell(state, td, textTokens);
        }

        /*************************************************
        SEARCH FUNCTIONS
        TODO: Support quotes (phrases) (")
        **************************************************/

        var precedences;

        function parseSearchTokens(text) {
            if (!text) { return null; }
            if (!precedences) {
                precedences = new Object();
                precedences.or = 1;
                precedences.and = 2;
                precedences.not = 3;
            }
            text = text.toLowerCase();
            var normalisedTokens = normaliseExpression(text);
            normalisedTokens = allowFriendlySearchTerms(normalisedTokens);
            // window.alert('normalisedTokens: ' + normalisedTokens);
            var asPostFix = convertExpressionToPostFix(normalisedTokens);
            // window.alert('asPostFix: ' + asPostFix);
            var postFixTokens = asPostFix.split(' ');
            return postFixTokens;
        }

        function normaliseExpression(text) {
            var textTokens = text.split(' ');
            var normalisedTokens = new Array();

            for (var i = 0; i < textTokens.length; i++) {
                var token = textTokens[i];
                var parenthesisIdx = token.indexOf('(');
                while (parenthesisIdx != -1) {
                    if (parenthesisIdx > 0) {
                        normalisedTokens[normalisedTokens.length] = token.substring(0, parenthesisIdx);
                    }
                    normalisedTokens[normalisedTokens.length] = '(';
                    token = token.substring(parenthesisIdx + 1);
                    parenthesisIdx = token.indexOf('(');
                }

                parenthesisIdx = token.indexOf(')');

                while (parenthesisIdx != -1) {
                    if (parenthesisIdx > 0) {
                        normalisedTokens[normalisedTokens.length] = token.substring(0, parenthesisIdx);
                    }

                    normalisedTokens[normalisedTokens.length] = ')';
                    token = token.substring(parenthesisIdx + 1);
                    parenthesisIdx = token.indexOf(')');
                }

                if (token.length > 0) { normalisedTokens[normalisedTokens.length] = token; }
            }
            return normalisedTokens;
        }

        function allowFriendlySearchTerms(tokens) {
            var newTokens = new Array();
            var lastToken;

            for (var i = 0; i < tokens.length; i++) {
                var token = tokens[i];
                if (!token || token.length == 0) { continue; }
                if (token.indexOf('-') == 0) {
                    token = 'not';
                    tokens[i] = tokens[i].substring(1);
                    i--;
                }
                if (!lastToken) {
                    newTokens[newTokens.length] = token;
                } else {
                    if (lastToken != '(' && lastToken != 'not' && lastToken != 'and' && lastToken != 'or' && token != 'and' && token != 'or' && token != ')') {
                        newTokens[newTokens.length] = 'and';
                    }
                    newTokens[newTokens.length] = token;
                }
                lastToken = token;
            }
            return newTokens;
        }

        function convertExpressionToPostFix(normalisedTokens) {
            var postFix = '';
            var stackOps = new Array();
            var stackOperator;
            for (var i = 0; i < normalisedTokens.length; i++) {
                var token = normalisedTokens[i];
                if (token.length == 0) continue;
                if (token != 'and' && token != 'or' && token != 'not' && token != '(' && token != ')') {
                    postFix = postFix + " " + token;
                }
                else {
                    if (stackOps.length == 0 || token == '(') {
                        stackOps.push(token);
                    }
                    else {
                        if (token == ')') {
                            stackOperator = stackOps.pop();
                            while (stackOperator != '(') {
                                postFix = postFix + ' ' + stackOperator;
                                stackOperator = stackOps.pop();
                            }
                        }
                        else if (stackOps[stackOps.length - 1] == '(') {
                            stackOps.push(token);
                        } else {
                            while (stackOps.length != 0) {
                                if (stackOps[stackOps.length - 1] == '(') { break; }
                                if (precedences[stackOps[stackOps.length - 1]] > precedences[token]) {
                                    stackOperator = stackOps.pop();
                                    postFix = postFix + ' ' + stackOperator;
                                }
                                else { break; }
                            }
                            stackOps.push(token);
                        }
                    }
                }
            }
            while (stackOps.length > 0) { postFix = postFix + ' ' + stackOps.pop(); }
            return trim(postFix);
        }

        function trim(str) { return str.replace(/^\s\s*/, '').replace(/\s\s*$/, ''); }

        function doesTextMatchTokens(textToMatch, postFixTokens) {
            if (!postFixTokens) return true;
            textToMatch = textToMatch.toLowerCase();
            var stackResult = new Array();
            var stackResult1;
            var stackResult2;

            for (var i = 0; i < postFixTokens.length; i++) {
                token = postFixTokens[i];
                if (token != 'and' && token != 'or' && token != 'not') {
                    stackResult.push(textToMatch.indexOf(token) >= 0);
                }
                else {

                    if (token == 'and') {
                        stackResult1 = stackResult.pop();
                        stackResult2 = stackResult.pop();
                        stackResult.push(stackResult1 && stackResult2);
                    }
                    else if (token == 'or') {
                        stackResult1 = stackResult.pop();
                        stackResult2 = stackResult.pop();

                        stackResult.push(stackResult1 || stackResult2);
                    }
                    else if (token == 'not') {
                        stackResult1 = stackResult.pop();
                        stackResult.push(!stackResult1);
                    }
                }
            }
            return stackResult.length == 1 && stackResult.pop();
        }

        /*************************************************
        SEARCH FUNCTIONS TESTS
        **************************************************/
        // runTests(); // UNCOMMENT FOR TESTS
        function runTests() {
            // window.alert('Starting Tests');
            testArgumentParsing();
            testSimpleANDMatches();
            testSimpleORMatches();
            testSimpleNOTMatches();
            testSimpleGroupMatches();
            // TODO: Add complex queries
            // window.alert('All Tests Passed');
        }

        function testArgumentParsing() {
            var tokens1 = parseSearchTokens('text1 and text2');
            var tokens2 = parseSearchTokens('text1 text2');
            assertArraysAreSame(tokens1, tokens2);

            var tokens1 = parseSearchTokens('not text2');
            var tokens2 = parseSearchTokens('-text2');
            assertArraysAreSame(tokens1, tokens2);
        }

        function assertArraysAreSame(arr1, arr2) {
            if (arr1.length != arr2.length) throw new Error('ERROR: assertArraysAreSame:1');
            for (var i = 0; i < arr1.length; i++) {
                if (arr1[i] != arr2[i]) throw new Error('ERROR: assertArraysAreSame:2');
            }
        }

        function testSimpleANDMatches() {
            var tokens1 = parseSearchTokens('text1 and text2');

            if (doesTextMatchTokens("text1", tokens1)) throw new Error('ERROR: testAndMatches:1');
            if (doesTextMatchTokens("text1 text3", tokens1)) throw new Error('ERROR: testAndMatches:2');
            if (!doesTextMatchTokens("text1 text2", tokens1)) throw new Error('ERROR: testAndMatches:3');
            if (!doesTextMatchTokens("text2 text1", tokens1)) throw new Error('ERROR: testAndMatches:4');
            if (!doesTextMatchTokens("text2 text 3text1", tokens1)) throw new Error('ERROR: testAndMatches:5');
        }

        function testSimpleORMatches() {
            var tokens1 = parseSearchTokens('text1 or text2');

            if (!doesTextMatchTokens("text1", tokens1)) throw new Error('ERROR: testSimpleORMatches:1');
            if (!doesTextMatchTokens("text1 text3", tokens1)) throw new Error('ERROR: testSimpleORMatches:2');
            if (!doesTextMatchTokens("text1 text2", tokens1)) throw new Error('ERROR: testSimpleORMatches:3');
            if (!doesTextMatchTokens("text2 text1", tokens1)) throw new Error('ERROR: testSimpleORMatches:4');
            if (!doesTextMatchTokens("text2 text 3text1", tokens1)) throw new Error('ERROR: testSimpleORMatches:5');
            if (doesTextMatchTokens("text3 text4", tokens1)) throw new Error('ERROR: testSimpleORMatches:6');
        }

        function testSimpleNOTMatches() {
            var tokens1 = parseSearchTokens('not text2');
            if (!doesTextMatchTokens("text1", tokens1)) throw new Error('ERROR: testSimpleNOTMatches:1');
            if (doesTextMatchTokens("text1 text2", tokens1)) throw new Error('ERROR: testSimpleNOTMatches:2');
        }

        function testSimpleGroupMatches() {
            var tokens1 = parseSearchTokens('(text1 and text2) or text3');
            if (doesTextMatchTokens("text1", tokens1)) throw new Error('ERROR: testSimpleGroupMatches:1');
            if (!doesTextMatchTokens("text1 text2", tokens1)) throw new Error('ERROR: testSimpleGroupMatches:2');
            if (!doesTextMatchTokens("text3", tokens1)) throw new Error('ERROR: testSimpleGroupMatches:2');
            if (!doesTextMatchTokens("text33", tokens1)) throw new Error('ERROR: testSimpleGroupMatches:3');
        }
    };

    jQuery.fn.tableFilter.filterToolTipMessage = "Quotes (\") can be used for phrase search. Minus (-) excludes a match from the results. Or (|) can be used to do OR searches. I.e. [red | blue] will match either red or blue.";

    jQuery.fn.tableFilter.defaults = {
        additionalFilterTriggers: [],
        clearFiltersControls: [],
        matchingRow: null,
        matchingCell: null,
        filterDelay: 200
    };

})(jQuery);