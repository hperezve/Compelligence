/**
 * Service for HTTP request to Compelligence with a return promise
 * 
 */

app.factory('CompelligenceHTTP', function(CompelligenceData, CompelligenceConfig, $http, $q, $log) {
	var d = {};
	
    d.sendMessage = function(request) {
	console.log("sendMessage");
		var deferred = $q.defer();
	    $http({
	      method: 'POST',
	      url: CompelligenceConfig.compelligencescurl + '?username=' + request.username + '&kennwort=' + request.kennwort,
		  //url:'http://localhost:28982/Home.aspx/ValidateUser?username=compelligence_companyb@yahoo.com.pe&kennwort=password',
	      //url: 'http://cincot.puridiom.com/puridiom5/procure',
	      data: request
	    }).success(function(response){
	      //console.log(response);
		  deferred.resolve(response);
		  
	    }).error(function(){
			//console.log('no success');
	        deferred.reject('There was an error');
	      
	    });
	    return deferred.promise;
    };

    return d;
});

app.factory("CompelligenceConfig", function() {
	// This service is for pass the URL
	return {
	compelligencescurl: './Services.aspx/ValidateUser'
	//compelligencescurl: '/Services.aspx/ValidateUser?username=compelligence_companyb@yahoo.com.pe&kennwort=password'
		//compelligencescurl: '/Home.aspx/ValidateUser'
		//compelligencescurl: 'http://localhost:28982/Home.aspx/ValidateUser?username=compelligence_companyb@yahoo.com.pe&kennwort=password'
	};
});
