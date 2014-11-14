/* Defining directive for articles, with template to bind to section */
app.directive('mainmarketviewbuilder', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<div class="register{{table.icon}}OuterParent"><div class="register{{table.icon}}InnerParent"><section class="register{{table.icon}}Page"><article viewbuilder class="{{table.portlet}}" ng-repeat="table in tables" ng-init="boxGeneralIndex = $index"></article></section></div></div>'
	};
});

