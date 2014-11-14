// Errors Login for compare with status of Response
//var path = {
//		loginSubmit: "RequisitionGeneral",
//		LoginController: "loginSubmit, RequisitionGeneral",
//		AUTHENTICATION_FAILED: "SC-10001",
//		INVALID_REQUEST_USER_ID: "SC-10002",
//		INVALID_REQUEST_ORGANIZATION_CODE: "SC-10003",
//		INVALID_REQUEST_SHARED_SECRET: "SC-10004",
//		INVALID_REQUEST_SESSION_ID: "SC-10005",
//		INVALID_REQUEST_REQUEST_ID: "SC-10006",
//		MAX_LOGIN_ATTEMPTS: "SC-10007",
//		INVALID_SERVER_REQUEST: "MS-10001"
//};

app.controller('LoginController', function($scope, $log, $location, $http, MyService, dataService) {    
	//This scope is a for combo box and display the languages
	$scope.languages =[{langaugeId : 'EN', languageNombre : 'English' },
		{langaugeId : 'ES', languageNombre : 'Spanish' },
		{langaugeId : 'PO', languageNombre : 'Portuguese' },
		{langaugeId : 'CH', languageNombre : 'Chinese' }];
	
	
	$scope.request = MyService.request;
	
//	$scope.request.requestHeader.identity.organizationId = 'CINCO'; 
//	$scope.request.requestHeader.identity.language = 'EN';
	
	
	$scope.loginSubmit = function() {
		$scope.request.requestHeader.module = 'authentication';
		$scope.request.requestHeader.service = 'login';
		
//		MyService.data.url = 'http://localhost:8080/puridiom5/procure';
		MyService.data.params = JSON.stringify($scope.request);
		
		//With this function I call my http for get the data
		dataService.getData(function(response) {
			MyService.request.requestHeader = response.requestHeader;
			if( response.responseStatus.statusCode !== "200") {
				$scope.errorsLogin = true;
				$scope.errorsMessage = response.responseStatus.message;
				return;
			} else {
				//If the response give message succesfully go to the next page
				$location.path('RequisitionGeneral');
			}
	    });		
	};
});

