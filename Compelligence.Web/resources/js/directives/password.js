// Directives for password input types to be used.
app.directive('password', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<input type="password"  ng-model="table.rows[model]" />'
	};
});