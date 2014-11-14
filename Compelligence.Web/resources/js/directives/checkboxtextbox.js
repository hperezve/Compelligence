// Directives for checkbox input types to be used.
app.directive('checkboxtextbox', function() {
	var template =
		'<div>' +
			'<input type="checkbox" tabindex="{{table.tabIndex[' + index + ']}}" ng-model="check_' + index + '" ng-init="check_' + index + ' = false" />' +
			'<input type="text" tabindex="{{table.tabIndex[' + index + ']}}" ng-model="table.rows[model]" ng-hide="!check_' + index + '" ng-show="check_' + index + '" />' +
		'</div>';
	return {
		restrict: 'AE',
		replace: 'true',
		template: template
	};
});