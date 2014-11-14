/**
 * Message Alert Service
 */
 
	app.service("MessageAlertService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {
		
		// Set Request Parameters for Message Alert Service
		this.setPuridiomHeader = function() {
			PuridiomData.request.requestHeader.module = 'browse';
			PuridiomData.request.requestHeader.service = 'open';
			PuridiomData.request.requestHeader.pageId = 'REQ_MESSAGE_ALERT';
			PuridiomData.request.requestHeader.moduleRequest = 'request';
			PuridiomData.request.requestHeader.option = '';
			PuridiomData.request.requestHeader.processKey = '';
			PuridiomData.request.requestHeader.browseKey = '';

			$log.debug("Message Alert Service - set request header");
			$log.debug(PuridiomData.request.requestHeader);
		};
		//setServiceParameters - adds values for the openService - icReqheader
		this.setServiceParameters = function(routeParams) {
			PuridiomData.request.requestBody = {
				BrowseObject : {
					module : 'browse',
					service : 'open',
					object : {
						browseId : 'REQ_MESSAGE_ALERT'
					}
				}
			};
		};

		// Get data from Puridiom Service
		this.getJsonData = function(request, routeParams) {
			$log.debug("MessageAlertService - getJsonData");
			$log.debug(routeParams);
			this.setServiceParameters(routeParams);
	
			var deferred = $q.defer();
			$http({
				method : 'POST',
				url : PurdiomConfig.puridiomscurl,
				data : request
			}).success(function(response) {
				deferred.resolve(response);
			}).error(function() {
				deferred.reject('There was an error');
			});
			return deferred.promise;
		};
	});
