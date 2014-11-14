app.controller('MessageAlertController', function($scope, $log, $interval, $location, MessageAlertService, PuridiomHTTP, PuridiomData, $routeParams) {

	$scope.startService = true;
//	if($location.path() == '/') {
//		$scope.startService = false;
//	}
	$scope.$on('eventMessageAlertService', function() {
		getDataJSON();
    });
	if($scope.startService == true){
	/* function for getting all data, and after getting data call function to make tables from that data */
	var getDataJSON = function() {
		// Set headers
		MessageAlertService.setPuridiomHeader($routeParams);
		// Set request from PuridiomData
		$scope.request = PuridiomData.request;
		// Get data from Service
		$log.debug($routeParams);
	
		MessageAlertService.getJsonData($scope.request, $routeParams).then(function(response) {
			$log.debug(response);
			if (response.responseStatus.statusCode !== "200") {
				$scope.errorsLogin = true;
				$scope.errorsMessage = response.responseStatus.message;
				return;
			} else {
				PuridiomData.request.requestHeader = response.requestHeader;
				
				$scope.allData = response.responseResult.result;
				PuridiomData.data.response = response;
				
				makeAlerts();
			};
		}, function(response) {
	    	  $log.debug(response);
	    });
	 };
	 //getDataJSON();
	};
	 
//function to create data for the alert drop-down in the header.	 
var makeAlerts = function() {
	// Gets the length of the array of alerts and makes that the number of alerts.	
	if(typeof $scope.allData.data.REQ_MESSAGE_ALERT !== "undefined"){
		$scope.numOfAlerts = $scope.allData.data.REQ_MESSAGE_ALERT.rows.length;
		if($scope.numOfAlerts === undefined){
			$scope.numOfAlerts = 0;
			document.getElementById('alertNumber').innerHTML = $scope.numOfAlerts;
		}
		else{
			document.getElementById('alertNumber').innerHTML = $scope.numOfAlerts;
		}
	}
	
	$scope.icons = [];
//	var icons = ["home", "clipboard", "delivery_truck", "portfolio_folder"];
	
 angular.forEach($scope.allData.data.REQ_MESSAGE_ALERT, function(value, key){	
		$scope.icons[key] = {};
		$scope.icons[key].alertTitle = $scope.allData.data.REQ_MESSAGE_ALERT[key].subject;
//		$scope.icons[key].icon = icons[key];
	});

};

// Interval for the Alerts on the Dashboard page
	$scope.getMessagesStarted = false;
    //populate message alerts
    $scope.getMessageAlerts = function(){
           $log.debug("Message Alerts");
           $scope.getMessagesStarted = true;
    };
    
    if(!$scope.getMessagesStarted){
           $interval(function() {
            $scope.getMessageAlerts();
           
        }, 3000);
    }
});