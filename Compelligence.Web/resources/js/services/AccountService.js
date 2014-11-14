/**
 * Account Service
 */
app.service("AccountService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Account Service
	this.setPuridiomHeader = function(routeParams) {

		// Module not necessary because comes from REVIEW AND FINALIZE
		PuridiomData.request.requestHeader.module = routeParams.module;
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;
		PuridiomData.request.requestHeader.service = routeParams.service;
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';

		$log.debug("AccountService - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};

	// setServiceParameters - adds values for the openService - icReqheader
	this.setServiceParameters = function(routeParams) {

		$log.debug("AccountService - setServiceParameters");

		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : routeParams.module,
				service : routeParams.service,
				object : {
					header : {
						icReqHeader : routeParams.icReqHeader
					}
				}
			},
			RequestParameters : {
				icReqLine : routeParams.icReqLine
			}
		};

		$log.debug(PuridiomData.request.requestBody);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {

		$log.debug("AccountService getObjectData");
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

	// Save the document in cache
	this.putCacheDataAccount = function(dataObject, routeParams) {
		$log.debug("TwoColService Put - Source Data");

		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'putCacheAllAccounts';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader : {},
			requestBody : {}
		};

		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : 'putCacheAllAccounts',
				object : dataObject.RequisitionObject
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
