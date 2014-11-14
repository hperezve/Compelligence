// Directives for radio button input types to be used.
app.directive('radiobutton', function($compile) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			var htmlString = '<div ng-repeat="dataRadio in table.typeData[' + index + ']">';
			if(scope.table.rows !== undefined) {
				htmlString = htmlString + '<input tabindex="{{table.tabIndex[' + index + ']}}" type="radio" ng-model="table.rows[model]" value="{{dataRadio.tableKey}}"/>{{dataRadio.description}}</div>';
			} else {
				htmlString = htmlString + '<input tabindex="{{table.tabIndex[' + index + ']}}" type="radio" ng-model="table.showColumns[' + index + '].model" value="{{dataRadio.tableKey}}"/>{{dataRadio.description}}</div>';
			}
			iElement[0].innerHTML = htmlString;
			$compile(iElement.contents())(scope);
		}
	};
});