//Directives for bill text box input types to be used.
app.directive('billtextbox', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="text" ng-model="table.specialRows[model]" />'
	};
});

//Directives for bill text box input types with a event to call browse to be used.
app.directive('billlinktextbox', function() {
	return {
		require: 'mainviewbuilder', 
		restrict: 'AE',
		replace: 'true',
		template: '<input type="text" ng-model="table.rows[model]" ng-enter="ADDRESS_BILL_BROWSE,BILLADDRESS_BROWSE"></input>'
	};
});