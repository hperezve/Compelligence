// Directives for range input types to be used.
app.directive('range', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="range" ng-model="table.rows[model]" min="0" max="1000" />'
	};
});