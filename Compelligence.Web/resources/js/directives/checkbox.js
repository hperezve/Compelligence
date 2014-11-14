// Directives for checkbox input types to be used.
app.directive('checkbox', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input tabindex="{{table.tabIndex[' + index + ']}}" type="checkbox" ng-model="table.rows[model]" ng-init="table.rows[model]=\'N\'" ng-true-value="Y" ng-false-value="N" />'
	};
});