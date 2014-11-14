'use strict';

/**
 * 
 */
angular.module('puridiomSeedApp').controller('RequisitionController', function($scope, $log, PuridiomData, PuridiomHTTP) {
	
	$scope.request = PuridiomData.request;

	$scope.request.requestHeader.module = 'requisition';
	$scope.request.requestHeader.service = 'open';
	$scope.request.requestBody = {
		RequisitionObject : {
			module : 'requisition',
			service : 'open',
			object : {
				header: {
					icReqHeader: '27504149685092'
				}
			}
		}
	};

	// Initial load
	PuridiomHTTP.sendMessage($scope.request)
		.then(function(response) {
				if( response.responseStatus.statusCode == '200') {
					$scope.requisitionHeader = {};
					$scope.requisitionHeader = response.responseResult.result.header;
				} else {
					$log.debug('Status Code: ' + response.responseStatus.statusCode);
					$log.debug('Message: ' + response.responseStatus.message);
				}
				
			}, function(response) {
	    	  	$log.debug(response);
			});

	// Reload information
	$scope.reload = function() {
		PuridiomHTTP.sendMessage($scope.request)
			.then(function(response) {
					if( response.responseStatus.statusCode == '200') {
						$scope.requisitionHeader = {};
						$scope.requisitionHeader = response.responseResult.result.header;
					} else {
						$log.debug('Status Code: ' + response.responseStatus.statusCode);
						$log.debug('Message: ' + response.responseStatus.message);
					}
					
				}, function(response) {
		    	  	$log.debug(response);
				});
	};
	
	// Save in cache
	$scope.saveInCache = function() {
		
		$scope.request.requestHeader.module = 'requisition';
		$scope.request.requestHeader.service = 'putCacheHeader';
		
		$scope.request.requestBody.header = $scope.requisitionHeader;
		
		PuridiomHTTP.sendMessage($scope.request)
		.then(function(response) {
				if( response.responseStatus.statusCode == '200') {
					$scope.requisitionHeader = {};
					$scope.requisitionHeader = response.responseResult.result.header;
				} else {
					$log.debug('Status Code: ' + response.responseStatus.statusCode);
					$log.debug(response.responseStatus.message);
				}
				
			}, function(response) {
	    	  	$log.debug(response);
			});
	};
	
	// Save in database
	$scope.saveInDatabase = function() {
		
		$scope.request.requestHeader.module = 'requisition';
		$scope.request.requestHeader.service = 'save';
		
		$scope.request.requestBody.header = $scope.requisitionHeader;
		
		PuridiomHTTP.sendMessage($scope.request)
		.then(function(response) {
				if( response.responseStatus.statusCode == '200') {
					$scope.requisitionHeader = {};
					$scope.requisitionHeader = response.responseResult.result.header;
				} else {
					$log.debug('Status Code: ' + response.responseStatus.statusCode);
					$log.debug('Message: ' + response.responseStatus.message);
				}
				
			}, function(response) {
	    	  	$log.debug(response);
			});
	};
	
});