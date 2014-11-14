/**
 * LogoutController
 */
app.controller('LogoutController', function($scope, $log, $location, LogoutService, PuridiomHTTP, PuridiomData) {
	
	$scope.request = PuridiomData.request;

	$log.debug("logoutSubmit start");
	
	$scope.errorsLogout = false;
	$scope.errorsMessage = '';
	
	// Set request header
	LogoutService.setPuridiomHeader($scope.request);

	$log.debug("Set RequestHeader:");
	$log.debug($scope.request.requestHeader);

		
	$log.debug("Request:");
	$log.debug($scope.request);
		
	PuridiomHTTP.sendMessage($scope.request)
	.then(function(response) {
		$log.debug('Response:');
		$log.debug(response);
		if (response.responseStatus.statusCode !== "200") {
			$scope.errorsLogout = true;
			$scope.errorsMessage = response.responseStatus.message;
		} else {
			$location.path("/");							
		}
      }, function(response) {
    	  	$log.debug(response);
      });
});/**
 * 
 */