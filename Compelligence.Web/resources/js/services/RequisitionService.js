/**
 * Requisition Service
 */
app.service("RequisitionService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {
		PuridiomData.request.requestHeader.module = routeParams.module;
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;
		PuridiomData.request.requestHeader.service = routeParams.service;
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};
	//setServiceParameters - adds values for the openService - icReqheader
	this.setServiceParameters = function(routeParams){
		$log.debug("ReviewFinalize Service - setServiceParameters");
		
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : routeParams.module,
				service : routeParams.service,
				object : {
					header: {
						requisitionType: routeParams.type 
					}
				}
			}
		};
		$log.debug(PuridiomData.request.requestBody);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("CreateService - getJsonData");
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