//Directives for ship text box input types with a event to call browse to be used.
app.directive('vendorlinktextbox', function($compile) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			var htmlString = "";
			switch (scope.table.templateId) {
				case 'QuestionObject': 
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="ADDRESS_SUPPLIER_BROWSE,QUESTION_SUPPLIER_BROWSE,{{boxIndex}}"></input>';
					break;
				default:
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="ADDRESS_SUPPLIER_BROWSE,SUPPLIERADDRESS_BROWSE,{{boxIndex}}"></input>';
					break;
			}
				
			iElement[0].innerHTML = htmlString;
			$compile(iElement.contents())(scope);
		}
	};
});

app.directive('questionlinktextbox', function($compile) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			var htmlString = "";
			switch (scope.table.model[index]) {
				case 'requisitionerName': 
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="USERPROFILE_BROWSE,REQUISITIONER_PAGE_BROWSE,{{boxIndex}}"></input>';
					break;
				case 'shipTo': 
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="ADDRESS_SHIP_BROWSE,QUESTION_SHIPADDRESS_BROWSE,{{boxIndex}}"></input>';
					break;
				case 'currencyCode':
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="CURR_BROWSE,CURR_PAGE_BROWSE,{{boxIndex}}"></input>';
					break;
				default:
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="ADDRESS_SUPPLIER_BROWSE,SUPPLIERADDRESS_BROWSE,{{boxIndex}}"></input>';
					break;
			}
				
			iElement[0].innerHTML = htmlString;
			$compile(iElement.contents())(scope);
		}
	};
});