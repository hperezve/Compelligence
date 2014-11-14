/**
 * 
 */
app.controller('DasboardController', function($scope, $log, $location, DashboardService, PuridiomHTTP, PuridiomData) {
	$scope.request = PuridiomData.request;
	$scope.allData = {}; // al scope.finalTables = [];
	$scope.tables = []; // tables array
	var inners = {}; // variable to save array of page element id
	var i = -1; // variable to change which box to look into.
	// Set request header
	DashboardService.setPuridiomHeader($scope.request);
	
	//createDashoard will get metadata/data and build the html.
	var createDashboard = function() {
		$log.debug("createDashboard");
			//get data for dashboard view	
			PuridiomHTTP.sendMessage($scope.request)
				.then(function(response) {
						
					if( response.responseStatus.statusCode !== "200") {
						$scope.errorsLogin = true;
						$scope.errorsMessage = response.responseStatus.message;
						return;
					} else {
						$log.debug("dashbaord got data");
						PuridiomData.request.requestHeader = response.requestHeader;
						$scope.allData = response.responseResult.result;
						angular.forEach(response.responseResult.result.metadata.boxList, function(value, key) {

							if (key !== "HEADER_MENU") {
								i++;
								if (value.elementList.length > 0) {
									inners[i] = value.elementList[0].labelId;
								}
							}
						});
						makeTables();
					}
			      }, function(response) {
			    	  	$log.debug(response);
			      });
			
		
	};
	// function for return bool, is column visible or not
	$scope.toShowCol = function(showCols) {
		return showCols;
	};

	// return int for html class to work correctly
	$scope.tableLength = function(table) {
		var i = 0;
		$log.debug(i);
		angular.forEach(table.showColumns, function(value, key) {
			if (value) {
				i++;
			}
		});

		return i;
	};

	// function to return int for each data in table to work with correctly with css classes
	$scope.columnNumber = function(table, index) {
		var i = 0;
		var ret = 1;
		for (i = 0; i < index; i++) {
			if (table.showColumns[i]) {
				ret++;
			}
		}
		return ret;
	};

	
	//Maketables
	var makeTables = function() {
		$log.debug("maketables start");
		angular.forEach(inners, function(value, key) {
			var columnLabels = false; // first initializing labels for each column
			if (typeof $scope.allData.metadata.boxList[key].elementList !== "undefined") {
				columnLabels = $scope.allData.metadata.boxList[key].elementList; //add values to columnLabels array
			}

			// adding rows, columns and title
			if (columnLabels !== false) {
				$scope.tables[key] = {};
				// $scope.tables[key].rows = $scope.allData.metadata.data;
				$scope.tables[key].columns = [];
				$scope.tables[key].title = $scope.allData.metadata.boxList[key].title;
				$scope.tables[key].portlet = $scope.allData.metadata.boxList[key].portletType; // This is the name of the portlet used.
				$scope.tables[key].type = $scope.allData.metadata.boxList[key].type; // This is the name of the template used inside of the portlet.

				$scope.tables[key].showColumns = [];
				angular.forEach(columnLabels, function(inValue,inKey) {
					$scope.tables[key].columns.push(inValue);
					$scope.tables[key].showColumns.push(inValue.visible);
				});
			}
		});
	};
	// call function to get all JSON data.
	createDashboard();
	
});