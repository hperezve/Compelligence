// Directives for hidden input types to be used.
app.directive('hidden', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="hidden" ng-model="table.rows[model]" />'
	};
});
