// Directives for input text area types to be used.
app.directive('inputtextarea', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<textarea tabindex="{{table.tabIndex[' + index + ']}}" row="5" ng-model="table.rows[model]"></textarea>'
	};
});