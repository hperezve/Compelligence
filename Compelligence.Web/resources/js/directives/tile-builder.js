/* Defining directive for the portlet screen tiles page */
app.directive('maintilebuilder', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<article class="dashboardTilesPortlet" ng-repeat="table in tables">' +
						'<h1 class="portlet-heading">Recent Documents' +
							'<a>' +
								'<span class="icon-tileRefresh">' +
									'<object data="resources/images/icons/refresh_white.svg" type="image/svg+xml"></object>' +
								'</span>' +
							'</a>' +
						'</h1>' +
						'<a>' +
							'<h3 class="full">' + 
								'<span class="icon-tile">' + 
									'<object data="resources/images/icons/clipboard_white.svg" type="image/svg+xml"></object>' + 
								'</span>' +
								'<span class="number">18</span>' + 
							'</h3>' +
							'<span class="description">Eighteen documents have been worked on in the past thirty days</span>' +
						'</a>' +
				   '</article>'
	};
});