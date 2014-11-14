/**
* CheckOutService Service
*/
app.service("CheckOutService", function($log, ContainerData, PuridiomData, PurdiomConfig, $http, $q) {
	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {
		PuridiomData.request.requestHeader.module = 'process';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = '';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = routeParams.processKey;
		PuridiomData.request.requestHeader.browseKey = '';		
		$log.debug(PuridiomData.request);

	
		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};
	
	this.setServiceParameters = function(routeParams){
		$log.debug("ReviewFinalize Service - setServiceParameters");		
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : '',
				object : {
					header : {requistionType : 'P'},
					catalogs : ContainerData.catalog
				}
			}
		};
		$log.debug(PuridiomData.request.requestBody);
	};
	
	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("CheckOutService Service - getJsonData");
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
	
	this.callMultiAction = function(processKey, routeParams) {
		PuridiomData.request.requestHeader.module = 'process';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = '';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = processKey;
		PuridiomData.request.requestHeader.browseKey = '';		
		$log.debug(PuridiomData.request);
		
		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};

		this.setServiceParameters(routeParams);
		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;
		
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
