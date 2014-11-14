// Directives for input button types to be used.
app.directive('inputbutton', function($compile,$log) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			var htmlString = "";
			htmlString = '<button tabindex="{{table.tabIndex[' + index + ']}}" ng-click="clickEventButton(table.showColumns[' + index + '], table)">{{table.showColumns[' + index + '].labelText}}</button>';
			iElement[0].innerHTML = htmlString;
			$compile(iElement.contents())(scope);
		}
	};
});

//Directives for input button types to be used.
app.directive('inputimagebutton', function($compile,$log) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			var htmlString = "";
			htmlString = '<button ng-hide="' + scope.table.model[index] + '" tabindex="{{table.tabIndex[' + index + ']}}" ng-click="clickEventButton(table.showColumns[' + index + '], table, rowIndex)"><div class="icon-button">';
			switch (scope.table.model[index]) {
				case 'delete_white': 
					htmlString += '<object data="resources/images/icons/delete_white.svg" type="image/svg+xml"></object>';
					break;
				case 'edit_white': 
					htmlString += '<object data="resources/images/icons/edit_white.svg" type="image/svg+xml"></object>';
					break;
				case 'check_white': 
					htmlString += '<object data="resources/images/icons/check_white.svg" type="image/svg+xml"></object>';
					break;
				case 'add_white': 
					htmlString += '<object data="resources/images/icons/add_white.svg" type="image/svg+xml"></object>';
					break;
				case 'navigate_right2_white': 
					htmlString += '<object data="resources/images/icons/navigate_right2_white.svg" type="image/svg+xml"></object>';
					break;
				case 'navigate_left_white': 
					htmlString += '<object data="resources/images/icons/navigate_left_white.svg" type="image/svg+xml"></object>';
					break;
				case 'navigate_right_white': 
					htmlString += '<object data="resources/images/icons/navigate_right_white.svg" type="image/svg+xml"></object>';
					break;
				case 'arrow_shuffle_white': 
					htmlString += '<object data="resources/images/icons/arrow_shuffle_white.svg" type="image/svg+xml"></object>';
					break;
				case 'clipboard_white': 
					htmlString += '<object data="resources/images/icons/clipboard_white.svg" type="image/svg+xml"></object>';
					break;
				case 'magnifying_glass_white': 
					htmlString += '<object data="resources/images/icons/magnifying_glass_white.svg" type="image/svg+xml"></object>';
					break;
				case 'notebook3_white': 
					htmlString += '<object data="resources/images/icons/notebook3_white.svg" type="image/svg+xml"></object>';
					break;
			}
			htmlString += '</div></button>';
			
			iElement[0].innerHTML = htmlString;
			$compile(iElement.contents())(scope);
		}
	};
});