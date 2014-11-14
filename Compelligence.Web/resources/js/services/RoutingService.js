/**
 * Routing Service
 */
app.service("RoutingService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {
		
		//module not neccesary becouse comes from REVIEW AND FINALIZE
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
	this.setServiceParameters = function(routeParams) {
		$log.debug("ReviewFinalize Service - setServiceParameters");
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : routeParams.module,
				service : routeParams.service,
				object : {
					header : {
						icReqHeader : routeParams.icReqHeader
					}
				}
			}
		};
		$log.debug(PuridiomData.request.requestBody);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("TwoColService getObjectData");
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
	
	// Add New Approval or Remove and save  the list of approvers.
	this.addApprover = function(module, pageId, service, newRow, routeParams) {
		$log.debug("TwoColService Save - Source Data");

		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = module;
		PuridiomData.request.requestHeader.pageId = pageId;
		PuridiomData.request.requestHeader.service = service;
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : module,
				service : service,
				object : {
					header: {
						icReqHeader : routeParams.icReqHeader
					},
					incomingParamaters : newRow
				}
			}
		};

		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : PuridiomData.request
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
	};

});