/**
 * Dashboard Service
 */
app.service("MultiProcessService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function() {
		PuridiomData.request.requestHeader.module = '';
		PuridiomData.request.requestHeader.service = '';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.moduleRequest = '';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("ReviewFinalizeService - getJsonData");
		$log.debug(routeParams);
		
		var deferred = $q.defer();
		deferred.resolve('Multi Process!');
		return deferred.promise;
	};

});