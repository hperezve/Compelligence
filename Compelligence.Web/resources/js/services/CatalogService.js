/**
* Catalog Service
*/
app.service("CatalogService", function($log, PuridiomData, ContainerData, PurdiomConfig, $http, $q) {
	// Set Request Parameters for Dashboard Service
	this.setPuridiomHeader = function() {
		PuridiomData.request.requestHeader.module = 'process';
		PuridiomData.request.requestHeader.moduleRequest = 'request';
		PuridiomData.request.requestHeader.service = 'execute';
		PuridiomData.request.requestHeader.browseKey = 'CATALOG_SEARCH';
		PuridiomData.request.requestHeader.pageId = 'CATALOG_SEARCH';
		PuridiomData.request.requestHeader.documentType = '';
	
		$log.debug("CatalogService Service - set request header");
		$log.debug(PuridiomData.request.requestHeader);
	};	
	
	this.setServiceParameters = function(routeParams){
		$log.debug("CatalogService Service - setServiceParameters");
		var mainFilters = [];
		if (ContainerData.filter != undefined){
			if (ContainerData.filter.length > 0){
				var filterDescrip = {};
				filterDescrip.groupId = '1';
				filterDescrip.model = 'description';
				filterDescrip.operator = 'like';
				filterDescrip.value = ContainerData.filter;
				mainFilters.push(filterDescrip);
				var filterItemNumber = {};
				filterItemNumber.groupId = '1';
				filterItemNumber.model = 'itemNumber';
				filterItemNumber.operator = 'like';
				filterItemNumber.value = ContainerData.filter;
				mainFilters.push(filterItemNumber);
			}			
		}	
		ContainerData.filter = "";
		
		PuridiomData.request.requestBody = {
			BrowseObject : {
				module : 'browse',
				service : 'open',
				object : {
					filters : mainFilters
				}
			}
		};
		$log.debug(PuridiomData.request.requestBody);
	};
	
	// Get data from Puridiom Service
	this.getJsonData = function(request, routeParams) {
		$log.debug("Catalog Service - getJsonData");
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
/*(function(){
	'use strict';

	 angular.module('puridiomSeedApp')
	 	.service('CatalogService',CatalogService);

	 CatalogService.$inject=['$http']

	 function CatalogService($http){

	 	this.getSearchJson=function(){
	 		return $http.get('../../../catalog_search.json');
	 	}
	 }
})();*/