// Directives for dropdown types to be used.
app.directive('dropdown', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<select tabindex="{{table.tabIndex[' + index + ']}}" ng-model="table.rows[model]" ng-options="value.tableKey as value.description for value in table.typeData[' + index + ']"></select>'
	};
});