// Directives for vendor text box input types to be used.
app.directive('vendortextbox', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="text" ng-model="table.rows.vendorAddress[model]" />'
	};
});