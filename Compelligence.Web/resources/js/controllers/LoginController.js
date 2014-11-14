/**
 * 
 */
app.controller('LoginController', function($scope, SharedService, $log, $location, $localStorage, $sessionStorage, LoginService, PuridiomHTTP, PuridiomData, ContainerData) {

	$scope.languages =[{languageId : 'EN', languageNombre : 'English' },
	           		{languageId : 'ES', languageNombre : 'Spanish' },
	           		{languageId : 'PO', languageNombre : 'Portuguese' },
	           		{languageId : 'CH', languageNombre : 'Chinese' }];
	
	$scope.rememberMe = false;

	// Handle Remember Me, save organizationId, language, mailId with Local Storage	
	if ($localStorage.rememberMe != undefined) {
		$scope.rememberMe = $localStorage.rememberMe;
		PuridiomData.request.requestHeader.identity.mailId = $localStorage.Pur56463;
		PuridiomData.request.requestHeader.identity.organizationId = $localStorage.Pur46421;
		PuridiomData.request.requestHeader.identity.language = $localStorage.language;
	} else {
		$localStorage.rememberMe = $scope.rememberMe;
		$localStorage.Pur56463 = PuridiomData.request.requestHeader.identity.mailId;
		$localStorage.Pur46421 = PuridiomData.request.requestHeader.identity.organizationId;
		$localStorage.language = PuridiomData.request.requestHeader.identity.language;
	}	
	
	
	$scope.request = PuridiomData.request;
	SharedService.isHideMenu(true);
	// Handle expiration session
	if (ContainerData.response != undefined && ContainerData.response.responseStatus != undefined) {
		$scope.errorsLogin = true;
		$scope.errorsMessage = ContainerData.response.responseStatus.message;
		ContainerData.response = {};
	}
	
	$scope.loginSubmit = function() {
		$log.debug("loginSubmit start");
		
		$scope.errorsLogin = false;
		$scope.errorsMessage = '';
		$scope.load = true;
		
		// Set request header
		LoginService.setPuridiomHeader($scope.request);

		$log.debug("Set RequestHeader:");
		$log.debug($scope.request.requestHeader);
		
		// Check valid data
		
		if (LoginService.validEmail($scope.request)) {
			
			$log.debug("Request:");
			$log.debug($scope.request);
			
			// Remember Me,organizationId and mailId set in LocalStorage.
			$localStorage.rememberMe = $scope.rememberMe;
			if($scope.rememberMe == false){
				$localStorage.Pur56463 = '';
				$localStorage.language = 'EN';
				$localStorage.Pur46421 = 'CINCO';
			} else {
				$localStorage.Pur56463 = $scope.request.requestHeader.identity.mailId;
				$scope.request.requestHeader.identity.organizationId = $scope.request.requestHeader.identity.organizationId.toUpperCase();
				$localStorage.Pur46421 = $scope.request.requestHeader.identity.organizationId;
				$localStorage.language = $scope.request.requestHeader.identity.language;
			}
			
			
			
			PuridiomHTTP.sendMessage($scope.request)
				.then(function(response) {

					$log.debug("Response:");
					$log.debug(response);
					$scope.load = false;
					if (response.responseStatus.statusCode !== "200") {
						$scope.errorsLogin = true;
						$scope.errorsMessage = response.responseStatus.message;
						//line useful for Valid max login attempts
						PuridiomData.request.requestHeader.identity.loginId = response.requestHeader.identity.loginId;
						return;
					} else {
						SharedService.isHideMenu(false);
						PuridiomData.request.requestHeader = response.requestHeader;
						document.getElementById('displayFullNameId').innerHTML = response.responseResult.result;
						PuridiomData.userProfile.userFullName = 'TEST';
						//If the response give message succesfully go to the next page
						$location.path('DashboardTiles/REQ_DASH_VIEW/REQ_DASH_VIEW');
						$log.debug("login got data and moving to dashboard");
						SharedService.runMessageAlertService();
					}
			      }, function(response) {
			    	  $log.debug(response);
			    	  $scope.load = false;
			      });
		} else {
			$scope.errorsMessage = "Mail no valid";
			$scope.load = false;
		}
	};
	
	$scope.registerSubmit = function() {
		$location.path('Register');
	};
});