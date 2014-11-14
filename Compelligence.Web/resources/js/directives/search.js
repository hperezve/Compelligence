// Directives for search input types to be used.
app.directive('search', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="search" ng-model="table.rows[model]" />'
	};
});