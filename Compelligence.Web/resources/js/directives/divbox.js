// Directives for textbox input types to be used.
app.directive('doctextdiv', function($compile,$log) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			if (angular.isArray(scope.table.rows)) {
				iElement[0].innerHTML = '{{row.docText[model]}}';	
			}else {
				iElement[0].innerHTML = '{{table.rows.docText[model]}}';
			}
			$compile(iElement.contents())(scope);
		}
	};
});

// Directives for textbox input types to be used.
app.directive('divtext', function($compile,$log) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			if (angular.isArray(scope.table.rows)) {
				iElement[0].innerHTML = '{{row[model]}}';	
			}else {
				iElement[0].innerHTML = '{{table.rows[model]}}';
			}
			$compile(iElement.contents())(scope);
		}
	};
});

//Directives for textbox input types to be used.
app.directive('divtextlabel', function($compile,$log) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
			if (angular.isArray(scope.table.rows)) {
				iElement[0].innerHTML = '<div class="detailLabel">{{table.showColumns[$index].labelText}}:</div>' +
					'<div class="text">{{row[model]}}</div>';	
			}else {
				iElement[0].innerHTML = '<div class="detailLabel">{{table.showColumns[$index].labelText}}:</div>' +
					'<div class="text">{{table.rows[model]}}</div>';
			}
			$compile(iElement.contents())(scope);
		}
	};
});