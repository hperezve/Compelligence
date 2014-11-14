/**
 * Dashboard Service
 */
app.service("DashboardTilesService", function($log, PuridiomData, PurdiomConfig, $http, $q) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {
		PuridiomData.request.requestHeader.module = 'process';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.service = 'execute';
		PuridiomData.request.requestHeader.browseKey = routeParams.browseKey;
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request) {
		$log.debug("Dashboard Service - getJsonData");

		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : request
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
	};

});