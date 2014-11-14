/* Dashboard Service
 */
app.service("RegisterService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {	

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {
		//module not neccesary becouse comes from REVIEW AND FINALIZE
		PuridiomData.request.requestHeader.module = routeParams.module;
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;
		PuridiomData.request.requestHeader.service = routeParams.service;
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.documentType = 'MARKETPLACE';
		PuridiomData.request.requestHeader.identity.language = routeParams.language;
		PuridiomData.request.requestHeader.identity.organizationId = 'CINCO';
		PuridiomData.request.requestHeader.identity.mailId='hubbardj@puridiom.com';
		PuridiomData.request.requestHeader.identity.userPassword='WELCOME@1';

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
	
});