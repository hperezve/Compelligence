// Directives for input number types to be used.
app.directive('number', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="text" ng-model="table.rows[model]" only-digits/>'
	};
});