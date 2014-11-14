// Directives for image input types to be used.
app.directive('imagen', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<img ng-src="table.rows[model]" />'
	};
});