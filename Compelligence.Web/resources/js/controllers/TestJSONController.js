/**
 * 
 */
app.controller('TestJSONController', function($scope, $log, $location, PuridiomHTTP, PuridiomData) {

	$scope.request = PuridiomData.request;
	$scope.request.requestBody = {
		BrowseObject : {
			module : 'browse',
			service : 'open',
			object : {
				filters : [
					{
						groupId : 'AA',
						model : 'description',
						operator : 'like',
						value : 'TEST'
					},
					{
						groupId : 'AA',
						model : 'itemNumber',
						operator : 'like',
						value : 'TEST'
					}
				]
			}
		}
	};
	$scope.request.requestHeader.pageId = 'CATALOG_SEARCH';
	$scope.request.requestHeader.browseKey = 'CATALOG_SEARCH';
	$scope.request.requestHeader.moduleRequest = 'request';
	$scope.request.requestHeader.module = 'process';
	$scope.request.requestHeader.service = 'execute';
	$scope.request.requestHeader.multipleObjects = true;
	$scope.request.requestHeader.identity = {
		organizationId: "CINCO",
		userId: "JHUBBARD0000000",
		mailId: "hubbardj@puridiom.com",
		sharedSecret: "WELCOME@1",
		language: "EN"
	};

	$scope.jsonTarget = '';
	
	$scope.getJson = function() {
		$scope.jsonTarget = 'cargando ...';
		$log.debug("get message:");
		$log.debug($scope.request);
		PuridiomHTTP.sendMessage($scope.request)
			.then(function(response) {
				$log.debug("Get response");
				$log.debug(response);
				if (response.responseStatus.statusCode !== "200") {
					$scope.errorsLogin = true;
					$scope.jsonTarget = response.responseStatus.message;
				} else {
					$scope.request.requestHeader = response.requestHeader;
					$scope.request.requestBody = response.responseResult.result;
					$scope.jsonTarget = JSON.stringify(response, undefined, 2);	
				}
		      }, function(response) {
		    	  	$log.debug(response);
		      });
	};
	
	$scope.sendJson = function() {
		
		$scope.request.requestHeader.pageId = '';
		$scope.request.requestHeader.processKey = '';
		$scope.request.requestHeader.moduleRequest = '';
		$scope.request.requestHeader.module = 'requisition';
		$scope.request.requestHeader.service = 'save';
		
		$scope.jsonTarget = 'cargando ...';
		$log.debug("send message");
		$log.debug($scope.request);
		PuridiomHTTP.sendMessage($scope.request)
			.then(function(response) {
				$log.debug("Get response");
				$log.debug(response);
				if (response.responseStatus.statusCode !== "200") {
					$scope.errorsLogin = true;
					$scope.jsonTarget = response.responseStatus.message;
				} else {
					$scope.request.requestHeader = response.requestHeader;
					$scope.request.requestBody = response.responseResult.result;
					$scope.jsonTarget = JSON.stringify(response.responseResult.result, undefined, 2);	
				}
		      }, function(response) {
		    	  	$log.debug(response);
		      });
	};
	
});