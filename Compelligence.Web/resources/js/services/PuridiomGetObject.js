/**
 * Service for HTTP request to Puridiom with a return promise
 * 
 */

app.factory('PuridiomGetObject', function(PuridiomData, PurdiomConfig, $q, $log) {
	// Get data from Puridiom Service
	this.getObjectData = function(request, routeParams) {
		$log.debug("getObjectData");
		$log.debug(routeParams);

		var deferred = $q.defer();
		deferred.resolve(PuridiomData.data.response);
		return deferred.promise;
	};
});