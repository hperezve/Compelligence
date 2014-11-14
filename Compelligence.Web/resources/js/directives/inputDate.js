// Directives for input date types to be used.
app.directive('inputdate', function() {
	return {
		restrict: 'AE',
		replace: 'true',
//		template: '<input type="text" date-time ng-model="table.rows[model]" view="month"/>'
		template: '<input type="text" class="form-control" tabindex="{{table.tabIndex[' + index + ']}}" ng-model="table.rows[model]" data-date-format="dd-MM-yyyy" data-date-type="string" data-autoclose="1"  bs-datepicker/>'
	};
});