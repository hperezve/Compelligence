/**
 * Service for HTTP request to Puridiom with a return promise
 * 
 */

app.factory('PuridiomHTTP', function(PuridiomData, PurdiomConfig, $http, $q, $log) {
	var d = {};
	
    d.sendMessage = function(request) {
		var deferred = $q.defer();
	    $http({
	      method: 'POST',
	      url: PurdiomConfig.puridiomscurl,
	      //url: 'http://cincot.puridiom.com/puridiom5/procure',
	      data: request
	    }).success(function(response){
	      deferred.resolve(response);
	    }).error(function(){
	      deferred.reject('There was an error');
	    });
	    return deferred.promise;
    };

    return d;
});