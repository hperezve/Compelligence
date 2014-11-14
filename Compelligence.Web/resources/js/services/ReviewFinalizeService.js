/**
 * Dashboard Service
 */
app.service("ReviewFinalizeService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function() {
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.service = 'open';
		PuridiomData.request.requestHeader.pageId = 'REQP_REVIEW';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		PuridiomData.request.requestHeader.documentType = '';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};
	//setServiceParameters - adds values for the openService - icReqheader
	this.setServiceParameters = function(routeParams){
		$log.debug("ReviewFinalize Service - setServiceParameters");
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : 'open',
				object : {
					header: {
						icReqHeader: routeParams.icReqHeader
					}
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
	
	// Save document in repository with cache update,
	// the document is update in repository and cache
	this.saveData = function(dataObject, routeParams) {
		$log.debug("TwoColService Save - Source Data");

		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.pageId = 'REQP_REVIEW';
		PuridiomData.request.requestHeader.service = 'save';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};
		
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : 'save',
				object : {
					header: {
						icReqHeader: routeParams.icReqHeader
					}
				}
			},
		};

		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;
		$log.debug("dataObject: ");
		$log.debug(dataObject);
		$log.debug("requestObject: " + requestObject);
		$log.debug(requestObject);
		
		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : requestObject
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
	};

});