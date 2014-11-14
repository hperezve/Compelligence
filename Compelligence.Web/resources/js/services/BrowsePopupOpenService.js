/**
 * Dashboard Service
 */
app.service("BrowsePopupOpenService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.openBrowseRoute = function(browseId, pageId, model) {
		
		//module not neccesary becouse comes from REVIEW AND FINALIZE
		PuridiomData.request.requestHeader.module = "browse";
		PuridiomData.request.requestHeader.pageId = pageId.toUpperCase();
		PuridiomData.request.requestHeader.service = "open";
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		PuridiomData.request.requestHeader.moduleRequest = 'request';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
		
		PuridiomData.request.requestBody = {
			BrowseObject : {
				module : 'browse',
				service : 'open',
				object : {
					browseId : browseId.toUpperCase(),
					filters : [
						{
							name : 'criteriaValue',
							operator : '=',
							value : model
						}
					]
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