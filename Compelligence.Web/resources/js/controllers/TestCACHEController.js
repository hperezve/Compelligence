/**
 * Test CACHE Controller
 */
app.controller('TestCACHEController', function($scope, $log, $location, PuridiomHTTP, PuridiomData) {

	$scope.request = PuridiomData.request;
	$scope.request.requestBody = {
		CacheAdminObject : {
			module : 'admin',
			service : 'getCacheJson',
			object : {
				cacheName : 'requisitionCache',
				cacheId : 'CINCO:34867046724524'
			}
		}
	};

	$scope.request.requestHeader.pageId = '';
	$scope.request.requestHeader.browseKey = '';
	$scope.request.requestHeader.processKey = '';
	$scope.request.requestHeader.moduleRequest = 'request';
	$scope.request.requestHeader.module = 'admin';
	$scope.request.requestHeader.service = 'getCacheJson';
	$scope.request.requestHeader.identity = {
		organizationId : 'CINCO',
		userId : 'JHUBBARD0000000',
		mailId : 'hubbardj@puridiom.com',
		sharedSecret : 'WELCOME@1',
		language : 'EN'
	};

	$scope.jsonTarget = '';

	$scope.getJson = function() {
		$scope.jsonTarget = 'cargando ...';
		$log.debug("get message:");
		$log.debug($scope.request);
		PuridiomHTTP.sendMessage($scope.request).then(function(response) {
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
});