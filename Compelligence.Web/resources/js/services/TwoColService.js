/**
 * Dashboard Service
 */
app.service("TwoColService", function(PuridiomData, PurdiomConfig, $http, $q, $log) {

	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function(routeParams) {

		//module not neccesary becouse comes from REVIEW AND FINALIZE
		PuridiomData.request.requestHeader.module = routeParams.module;
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;
		PuridiomData.request.requestHeader.service = routeParams.service;
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';

		$log.debug("Dashboard Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};
	//setServiceParameters - adds values for the openService - icReqheader
	this.setServiceParameters = function(routeParams){
		$log.debug("ReviewFinalize Service - setServiceParameters");
		var browseIdName = "";
		if(routeParams.browseId != undefined) {
			browseIdName = routeParams.browseId.toUpperCase();
		}
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : routeParams.module,
				service : routeParams.service,
				object : {
					header: {
						icReqHeader: routeParams.icReqHeader
					}
				}
			},
			BrowseObject : {
				module : 'browse',
				service : 'open',
				object : {
					browseId: browseIdName.toUpperCase()
				}
			}
		};
		$log.debug(PuridiomData.request.requestBody);
	};

	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("TwoColService getObjectData");
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
	
	
	this.saveCache = function(dataObject) {
		$log.debug("TwoColService Save Cache - Data");
		
		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'saveCache';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};

		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : 'saveCache',
				object : dataObject.RequisitionObject
			},
		};

		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;	
		$log.debug("dataObject: ");
		$log.debug(dataObject);
		$log.debug("requestObject: " + requestObject);
		$log.debug(requestObject);
		
		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : requestObject
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
		
	};
	
	// Save the document in cache
	this.putCacheData = function(dataObject) {
		$log.debug("TwoColService Put - Source Data");
		
		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'putCacheHeader';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};
		
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : 'putCacheHeader',
				object : dataObject.RequisitionObject
			},
		};

		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;	
		$log.debug("dataObject: ");
		$log.debug(dataObject);
		$log.debug("requestObject: " + requestObject);
		$log.debug(requestObject);
		
		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : requestObject
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;	
	};
	
	// Save the document in cache
	this.putCacheDataAccount = function(dataObject, routeParams) {
		$log.debug("TwoColService Put - Source Data");
		
		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'putCacheAllAccounts';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};
		
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : 'putCacheAllAccounts',
				object : dataObject.RequisitionObject
			},
		};

		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;	
		$log.debug("dataObject: ");
		$log.debug(dataObject);
		$log.debug("requestObject: " + requestObject);
		$log.debug(requestObject);
		
		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : requestObject
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
	};
	
	this.modifyCommentData = function(dataObject, routeParams, service) {
		var browseIdName = "";
		if(routeParams.browseId != undefined) {
			browseIdName = routeParams.browseId.toUpperCase();
		}
		
		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.service = service;
		PuridiomData.request.requestHeader.pageId = routeParams.pageId;
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		var requestObject = {
			requestHeader: {},
			requestBody: {}
		};
		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : service,
				object : dataObject.RequisitionObject
			},
			BrowseObject : {
				module : 'browse',
				service : 'open',
				object : {
					browseId: browseIdName.toUpperCase()
				}
			}
		};

		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;	
		$log.debug("dataObject: ");
		$log.debug(dataObject);
		$log.debug("requestObject: " + requestObject);
		$log.debug(requestObject);
		
		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : requestObject
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
		
	};
	
	this.removeRowData = function(module, service, pageId, deleteRow, routeParams) {

		PuridiomData.request.requestHeader.pageId = pageId;
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);
		
		var browseIdName = "";
		if(routeParams.browseId != undefined) {
			browseIdName = routeParams.browseId.toUpperCase();
		}

		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : module,
				service : service,
				object : {
					header: {
						icReqHeader : routeParams.icReqHeader
					},
					incomingParamaters : deleteRow
				}
			},
			BrowseObject : {
				module : 'browse',
				service : 'open',
				object : {
					browseId: browseIdName.toUpperCase()
				}
			}
		};

		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : PuridiomData.request
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
	};
	
	// Save document in repository with cache update,
	// the document is update in repository and cache
	this.saveData = function(dataObject) {
		$log.debug("TwoColService Save - Source Data");

		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'save';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		PuridiomData.request.requestBody = {
			RequisitionObject : {
				module : 'requisition',
				service : 'save',
				object : dataObject.RequisitionObject
			},
		};

		requestObject.requestHeader = PuridiomData.request.requestHeader;
		requestObject.requestBody = PuridiomData.request.requestBody;
		$log.debug("dataObject: ");
		$log.debug(dataObject);
		$log.debug("requestObject: " + requestObject);
		$log.debug(requestObject);
		
		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : requestObject
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
	};
	
	// Save document in repository with cache update,
	// the document is update in repository and cache
	this.uploadAttachments = function(dataObject,routeParamters) { 
		$log.debug("TwoColService Save Cache - Attachments Data");
		
		$log.debug("Set header in PuridiomData.request:");
		PuridiomData.request.requestHeader.module = 'requisition';
		PuridiomData.request.requestHeader.pageId = '';
		PuridiomData.request.requestHeader.service = 'uploadAttachments';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.processKey = '';
		PuridiomData.request.requestHeader.browseKey = '';
		$log.debug(PuridiomData.request);

		PuridiomData.request.requestBody = {
				RequisitionObject : {
					module : 'requisition',
					service : 'uploadAttachments',
					object : dataObject.RequisitionObject
				},
			};

		var deferred = $q.defer();
		$http({
			method : 'POST',
			url : PurdiomConfig.puridiomscurl,
			data : PuridiomData.request
		}).success(function(response) {
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
		
	};
	
	
	// Upload files asynchronous using Box to fill returning a 
	//response object with file information
	this.uploadFileToUrl = function(file, uploadUrl, attachment){
        var formData = new FormData();
        	formData.append('file',file);
        	formData.append('docPrint',attachment.docPrint);
        	formData.append('docTitle',attachment.docTitle);
        	formData.append('organizationId',PuridiomData.request.requestHeader.identity.organizationId);
        var deferred = $q.defer();
		$http({
			method : 'POST',
			url : uploadUrl,
			data : formData,
			transformRequest: angular.identity,
            headers: {'Content-Type': undefined}
		}).success(function(response) {
			console.log(response);
			deferred.resolve(response);
		}).error(function() {
			deferred.reject('There was an error');
		});
		return deferred.promise;
    };
});