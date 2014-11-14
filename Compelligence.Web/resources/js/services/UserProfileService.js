/**
 * Dashboard Service
 */
app.service("UserProfileService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function() {
		PuridiomData.request.requestHeader.module = 'table';
		PuridiomData.request.requestHeader.service = 'open';
		PuridiomData.request.requestHeader.option = 'user';
		PuridiomData.request.requestHeader.pageId = 'USER_PROFILE_PAGE';
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
			tableObject : {
				module : 'table',
				service : 'open',
				option : 'user',
				object : {
					userProfile : {
						organizationId : routeParams.organizationId,
						userId : routeParams.userId
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

});