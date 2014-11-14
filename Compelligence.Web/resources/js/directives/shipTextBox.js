// Directives for ship text box input types to be used.
app.directive('shiptextbox', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="text" ng-model="table.specialRows[model]" />'
	};
});

//Directives for ship text box input types with a event to call browse to be used.
app.directive('shiplinktextbox', function($compile) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			var htmlString = "";
			switch (scope.table.templateId) {
				case 'QuestionObject': 
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="ADDRESS_SHIP_BROWSE,QUESTION_SHIPADDRESS_BROWSE,{{boxIndex}}"></input>';
					break;
				default:
					htmlString += '<input type="text" ng-model="table.rows[model]" ng-enter="ADDRESS_SHIP_BROWSE,SHIPADDRESS_BROWSE,{{boxIndex}}"></input>';
					break;
			}
				
			iElement[0].innerHTML = htmlString;
			$compile(iElement.contents())(scope);
		}
	};
});

