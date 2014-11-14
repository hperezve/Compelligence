/**
 * TO DO:
 * 	MyService.data.url SHOULD BE POPULATED FROM PROPERTIES
 */ 
 
'use strict';

angular.module('puridiomSeedApp').controller('MainCtrl', function($scope, $log, $http, MyService, dataService, PurdiomConfig) {
	$scope.request = MyService.request;

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

	MyService.data.url = PurdiomConfig.puridiomscurl;
	MyService.data.params = JSON.stringify($scope.request);
	
	dataService.getData(function(response) {
		
		$scope.request.requestHeader = response.requestHeader;
		$scope.request.requestBody = response.responseResult.result;
		
		// Update the requisitionHeader with the data of the response
		$scope.requisitionHeader = {};
		$scope.requisitionHeader = response.responseResult.result.header;
	});

	$scope.reload = function() {
		
		$scope.request.requestHeader.module = 'requisition';
		$scope.request.requestHeader.service = 'open';
		
		MyService.data.url = '/puridiom5/jsonServlet';
		MyService.data.params = JSON.stringify($scope.request);
		
		dataService.getData(function(response) {
			// Update the requisitionHeader with the data of the response
			$scope.requisitionHeader = response.responseResult.result.header;
		});
	};

});
