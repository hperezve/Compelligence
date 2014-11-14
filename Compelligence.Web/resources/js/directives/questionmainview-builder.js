/* Defining directive for articles, with template to bind to section */
app.directive('questionmainviewbuilder', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<section class="portletScreen"><div ng-show="load" class="loadingScreen"></div><article viewbuilder class="{{table.portlet}}" ng-repeat="table in tables" ng-init="boxGeneralIndex = $index" ng-show="showQuestionSection[$index]"></article></section>'
	};
});

