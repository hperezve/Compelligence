/**
 * Dashboard Service
 */
app.service("MarketPlaceDashboardService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function() {
		PuridiomData.request.requestHeader.module = 'process';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.service = 'execute';
		PuridiomData.request.requestHeader.browseKey = 'MARKET_DASHBOARD';
		PuridiomData.request.requestHeader.pageId = 'MARKET_DASHBOARD';

		$log.debug("Market Place Dashboard Service - set request header");
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