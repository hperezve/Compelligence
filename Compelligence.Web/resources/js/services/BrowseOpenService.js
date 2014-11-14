/**
 * Dashboard Service
 */
app.service("BrowseOpenService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function() {
		PuridiomData.request.requestHeader.module = "browse";
//		PuridiomData.request.requestHeader.pageId = "REQ_RECENTDOC_VIEW";
		PuridiomData.request.requestHeader.service = "open";
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		PuridiomData.request.requestHeader.moduleRequest = 'request';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};
	//setServiceParameters - adds values for the openService - icReqheader
	this.setServiceParameters = function(routeParams) {
		$log.debug("ReviewFinalize Service - setServiceParameters");
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;
		PuridiomData.request.requestBody = {
			BrowseObject : {
				module : 'browse',
				service : 'open',
				object : {
					browseId: routeParams.browseId.toUpperCase()
				}
			}
		};
		$log.debug(PuridiomData.request.requestBody);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("ReviewFinalizeService - getJsonData");
		$log.debug(routeParams);
		this.setServiceParameters(routeParams);

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