/* Dashboard Service
 */
app.service("ManagementService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {
		//module not neccesary becouse comes from REVIEW AND FINALIZE
		PuridiomData.request.requestHeader.module = '';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = '';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.identity.language = routeParams.language;
		PuridiomData.request.requestHeader.identity.organizationId = 'CINCO';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("RegisterService getObjectData");
		$log.debug(routeParams);
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

	this.cleanCache = function(){
		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'admin';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'clearCache';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};

		PuridiomData.request.requestBody = {
			CacheAdminObject : {
				module : 'admin',
				service : 'clearCache',
				object : {
					cacheName : 'all',
				},				
			},
		};

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