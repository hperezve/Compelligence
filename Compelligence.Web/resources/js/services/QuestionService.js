/* Dashboard Service
 */
app.service("QuestionService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {	

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {
		//module not neccesary becouse comes from REVIEW AND FINALIZE
		PuridiomData.request.requestHeader.module = 'cd';
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;
		PuridiomData.request.requestHeader.service = 'doNothingResponse';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.identity.organizationId = 'CINCO';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};
	
	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("RegisterService getObjectData");
		$log.debug(routeParams);
		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};
		
		PuridiomData.request.requestBody = {
			ResponseQa : {
				module : 'cd',
				service : 'doNothingResponse',
				option : 'doNothingResponse',
				object : {}
			},
		};

		requestObject.requestBody = PuridiomData.request.requestBody;	
		requestObject.requestHeader = PuridiomData.request.requestHeader;	
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
	
	this.saveResponseData = function(dataObject) {
		$log.debug("TwoColService Save - Source Data");

		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'cd';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'saveResponses';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};
		
		PuridiomData.request.requestBody = {
			QuestionObject : {
				module : 'cd',
				service : 'saveResponses',
				option : 'question',
				object : {
					responseQa : dataObject
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
	
	this.createDocument = function(parseParams, routeParams) {
		PuridiomData.request.requestHeader.module = 'process';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = '';
		PuridiomData.request.requestHeader.option = 'parseObject';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = 'REQ_HEADER';
		PuridiomData.request.requestHeader.browseKey = '';
		PuridiomData.request.requestHeader.documentType = routeParams.type;
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};
		
		PuridiomData.request.requestBody = {
			ListContainerObject : {
				module : 'requisition',
				service : '',
				option : 'parseObject',
				documentType : routeParams.type,
				object : {
					extrinsic : parseParams
				}
			},
		};

		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;
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