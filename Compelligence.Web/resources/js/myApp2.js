/* Defining filter for columns data, if object is empty return empty string, to look good and not to have {} look. */
angular.module('puridiomSeedApp')
	.filter('isEmpty', function () {
		return function (obj) {
			if (obj.length > 0 || typeof obj === "number") {
				return obj;
			}
			return " ";
		};
});

app.filter('setDecimal', function ($filter) {
    return function (input, places) {
        if (isNaN(input)) return input;
        // If we want 1 decimal place, we want to mult/div by 10
        // If we want 2 decimal places, we want to mult/div by 100, etc
        // So use the following to create that factor
        var factor = "1" + Array(+(places > 0 && places + 1)).join("0");
        return Math.round(input * factor) / factor;
    };
});

/* Adding Main controller for application, using scope, and http service */
app.controller('MainCtrl', ['$scope', '$log', '$http', '$location', '$window', 'resolveData', 'ContainerData', 'PuridiomData', 'PuridiomHTTP', '$routeParams', 'BrowsePopupOpenService', 'CatalogService', function($scope, $log, $http, $location, $window, resolveData, ContainerData, PuridiomData, PuridiomHTTP, $routeParams, BrowsePopupOpenService, CatalogService) {
	$scope.allData = {}; // al scope.finalTables = [];
	$scope.allDataObject = {}; // al scope.finalTables = [];
	$scope.allDataObjectBackup = {}; // al scope.finalTables = [];
	$scope.allDataBrowse = {}; // al scope.finalTables = [];
	$scope.checkOutItems = []; //  Array that contains the selected items.
	$scope.ResponseQaList = []; //Save all question and answer in this list
	$scope.paginationArray = [];	// number of pages showed in the pagination bar
	$scope.elementColums = [];
	$scope.elementModels = [];
	$scope.elementRows = [];
	$scope.elementHiddens = [];
	$scope.elementMetadata = [];
	$scope.showPopUp = [];
	$scope.showQuestionSection = [];
	$scope.setupQuestionSection = false;
	$scope.approval = {
	};
	$scope.comments = {
		docText: {}
	};
	$scope.attachment = {
			docPrint:'',
			docTitle:'',
			file:null,
			boxModel:'',
			boxId:''
		};
	$scope.tables = []; // tables array
	$scope.allRows = [];
	$scope.messageProcess = [];
	$scope.allColumns = []; //Get all columns in a list and use for some pages example: question & answer
	$scope.referenceModelList = {};
	$scope.messageCache = '';
	$scope.browseRoutingButton = false;
	$scope.browseAttachmentButton = false;
	$scope.showDragAndDrop = false;
	$scope.itemDetails = false;
	$scope.exceedPercent = false;
	$scope.exceedAmount = false;
	$scope.existSections = false;
	$scope.load = true;
	$scope.totalLineAmount = 0;
	$scope.currentBox = 0;
	$scope.defaultAmount = false;
	$scope.showAnswer = false; //If value is false show quiz and is true show question with answer.
	$scope.returnNewRow = false; // this is when need returnData from browse but need a new row.
	$scope.validateBox = false;
	var inners = {}; // variable to save array of page element id
	var i = -1; // variable to change which box to look into.
	var watcher;
	/* function for getting all data, and after getting data call function to make tables from that data */
	//alert(resolveData);
	
	$scope.check = function() {
		$log.debug($scope.allData);
	};
		
	var getDataJSON = function() {
		// Set headers
		resolveData.setPuridiomHeader($routeParams);
		// Set request from PuridiomData
		$scope.request = PuridiomData.request;
		// Get data from Service
		$log.debug($routeParams);
		if($routeParams.service === "createLine") {
			$scope.itemDetails = true;
		}

		//$http.get('REQDASHMETA.json').success(function(response) {
		resolveData.getJsonData($scope.request, $routeParams).then(function(response) {
			$log.debug(response);
			$scope.load = false;
			if(!angular.equals({},ContainerData.response)) {
				response = ContainerData.response;
			}
			if (response.responseStatus.statusCode !== "200") {
				$scope.errorsLogin = true;
				ContainerData.response = response;
				if (response.responseStatus.statusCode == 'SC-10100') {
					// Session Expired
					$scope.errorsMessage = response.responseStatus.message;
					$location.path('Login');	
				} else {
					// General Error
					$location.path('Error');	
				}
			} else {
				PuridiomData.request.requestHeader = response.requestHeader;

				$scope.allDataObject = response.responseResult.result;
				angular.copy($scope.allDataObject,$scope.allDataObjectBackup);				
				PuridiomData.data.response = response;
				$scope.copyData($scope.allDataObject);
				ContainerData.response = {};
				
				//watcher for know the old and new value
				if(watcher) {
					$scope.$watch('allData', function(newValue, oldValue) {
						$scope.newValueT = newValue;
						$scope.oldValueT = oldValue;
						if($scope.newValueT != $scope.oldValueT) {
						    $scope.dataHasChanged= angular.equals($scope.newValueT,$scope.oldValueT);
						    $log.debug($scope.dataHasChanged);
						}
						watchQuiz();	
						$log.debug("new" + newValue + "- olvalue" + oldValue);					
			        }, true);
					$scope.$watch('tables[1].rows.quantity',function(val,old){
						if ($scope.tables[1] != undefined) {
							if ($scope.tables[1].rows !== undefined && $scope.tables[1].rows.quantity !== undefined) {
								if($scope.tables[1].rows.quantity !== "") {
									$scope.tables[1].rows.quantity = parseFloat(val);
								} else {
									$scope.tables[1].rows.quantity = "";
								}
							}
						}
					});
					$scope.$watch('tables[1].rows.unitPrice',function(valu,oldu){
						if ($scope.tables[1] != undefined) {
							if ($scope.tables[1].rows !== undefined &&  $scope.tables[1].rows.unitPrice !== undefined) {
								if($scope.tables[1].rows.unitPrice !== "") {
									$scope.tables[1].rows.unitPrice = parseFloat(valu);
								} else {
									$scope.tables[1].rows.unitPrice = "";
								}
							}
						}
					});
				}
			}
	      }, function(response) {
	    	  $log.debug(response);
	      });
	};
	
	

	var makeTables = function() {
		angular.forEach(inners, function(value, key) {
			// var rowNumberIndex = 0; // for know the index of the row
			var columnLabels = false; // first initializing labels for each column
			if (typeof $scope.allData.metadata.boxList[key].elementList !== "undefined") {
				columnLabels = $scope.allData.metadata.boxList[key].elementList; //add values to columnLabels array
			}

			// adding rows, columns and title
			if (columnLabels !== false) {
				$scope.tables[key] = {};

				$scope.tables[key].boxId = $scope.allData.metadata.boxList[key].boxId;				
				$scope.tables[key].rows = $scope.allData.data[$scope.tables[key].boxId];
				$scope.tables[key].specialRows = $scope.allData.data[$scope.tables[key].boxId];				 

				$scope.tables[key].columns = [];
				$scope.tables[key].title = $scope.allData.metadata.boxList[key].labelTitle;
				$scope.tables[key].labelDescription = $scope.allData.metadata.boxList[key].labelDescription;
				$scope.tables[key].portlet = $scope.allData.metadata.boxList[key].portletType; // This is the name of the portlet used.
				$scope.tables[key].type = $scope.allData.metadata.boxList[key].type; // This is the name of the template used inside of the portlet.
				$scope.tables[key].icon = $scope.allData.metadata.boxList[key].icon;
				$scope.tables[key].templateId = $scope.allDataObject.metadata.model;
				
				$scope.tables[key].showBrowseBox = false;
				$scope.tables[key].showButtons = [];
				$scope.tables[key].typeData = [];
				$scope.tables[key].imageData = [];
				$scope.tables[key].showColumns = [];
				$scope.tables[key].classDisplay = [];
				$scope.tables[key].hideColumns = [];
				$scope.tables[key].eventParams = [];
				$scope.tables[key].allModel = [];
				$scope.tables[key].referenceModel = [];
				$scope.tables[key].model = [];
				$scope.tables[key].tabIndex = [];
				var isArrayValue = false;
				$scope.arrayItem = [];
				$scope.tables[key].boxModel = $scope.allData.metadata.boxList[key].model;
	
				if($scope.tables[key].templateId.indexOf("routing") > 0) {
					$scope.browseRoutingButton = true;
				}
				if($scope.tables[key].templateId.indexOf("attachments") > 0) {
					$scope.browseAttachmentButton = true;
				}
				if($scope.tables[key].portlet.indexOf("headingPortlet") >= 0) {
					if(!$scope.existSections) {
						$scope.showQuestionSection.push(true);
					}
				}
				if($scope.tables[key].boxModel.indexOf("ResponseQa") >= 0) {
					if(!$scope.existSections) {
						if(!$scope.setupQuestionSection) {
							$scope.showQuestionSection.push(true);
							$scope.setupQuestionSection = true;
							$scope.currentBox = key;
						} else {
							if(!$scope.showQuestionSection[key] || $scope.showQuestionSection[key] === undefined) {
								$scope.showQuestionSection.push(false);
							}
						}
					}
				} 
				if($scope.tables[key].type === "ITEMLIST") {
					$scope.tables[key].itemList = true;
				}
				
				if($scope.tables[key].boxModel.indexOf("messageProcess") >= 0) {
					$scope.tables[key].rows = $scope.allData.data;
					splitModel =  $scope.tables[key].boxModel.split(".");
					for (var i = 0; i < splitModel.length; i++) {
						if($scope.tables[key].rows != undefined && !angular.isArray($scope.tables[key].rows)) {
							$scope.tables[key].rows = $scope.tables[key].rows[splitModel[i]];
						} 
					}
					splitModel =  $scope.tables[key].rows.split("#");
					for (var j = 0; j < splitModel.length; j++) {
						if(splitModel[j] !== "") {
							$scope.messageProcess.push(splitModel[j]);
						}
					}
				}
				
				$scope.tables[key].eventList = $scope.allData.metadata.boxList[key].eventList;

				//{{table.eventList.eventType}}={{row[table.eventList.methodParams]}}
				if($scope.allData.metadata.boxList[key].eventList.length > 0){
					$scope.tables[key].attribute = $scope.tables[key].eventList[0].eventType + "=" +  $scope.tables[key].eventList[0].methodParams;
				}
				else{
					$scope.tables[key].attribute = "";
				}

				var k = 0;
				angular.forEach(columnLabels, function(inValue,inKey) {
					$scope.tables[key].columns.push(inValue);
					if(!isArrayValue) {
						//ask if exist BoxModel in the case exist I get this for get the object
						//Then I split the model = "header.requisitionNumber" for get the final value
						//splitModelElement  in the case when RequisitionObject.header contain another object shipToAddress.addressLine1
						//inValue.pageType for know if need data from shiptoaddress or billtoaddress
							
						if($scope.tables[key].boxModel != undefined) {
							watcher = true;
							//specialRow is using when in the same box has 2 objects like header and shiptoaddress
							//with this I get the elements from the second object
							if(inValue.specialRow) {
								$scope.tables[key].specialRows = $scope.allData.data;
							}
							$scope.tables[key].rows = $scope.allData.data;
							splitModel =  $scope.tables[key].boxModel.split(".");
							splitModelElement = inValue.model.split(".");
							if(splitModelElement.length > 1) {
								splitModel[splitModel.length] = splitModelElement[0];
								inValue.model = splitModelElement[1];
								inValue.pageType = splitModelElement[0];
							}
							for (var j = 0; j < splitModel.length; j++) {
								//This conditional is in the case I get Array don't process only stay in rows
								//If is Array I iterate this for get all objects inside of this Array then create a array
								//this array contain the data we need to the dislay in the same way of TwoCol.
								if($scope.tables[key].rows != undefined && !angular.isArray($scope.tables[key].rows)) {
									if(inValue.specialRow) {
										$scope.tables[key].specialRows = $scope.tables[key].specialRows[splitModel[j]];
									} else {
										$scope.tables[key].rows = $scope.tables[key].rows[splitModel[j]];
									}
								} else if (angular.isArray($scope.tables[key].rows)) {
									for (k = 0; k < $scope.tables[key].rows.length; k++) {
										$scope.getObject = $scope.tables[key].rows[k];
										$scope.getObject = $scope.getObject[splitModel[j]];
										//Iterator again because in some cases when open the Account need header and lines
										//so the lines contain account list for this reason I create the second iterator
										if (angular.isArray($scope.getObject) && $scope.getObject.length > 0) {
											$scope.arrayObject = $scope.getObject.length;
											$scope.arrayItem.push($scope.getObject);
										} else {
											$scope.arrayItem.push($scope.getObject);
										}
									}
									$scope.tables[key].rows = $scope.arrayItem;
									isArrayValue = true;
								} 
							}
						}
					}
					//allModel array is for know all elements in the page and referenceModel all elements from the browse and then
					//join these elements
					$scope.allRows.push($scope.tables[key].rows);
					$scope.tables[key].allModel.push(inValue.model);
					$scope.tables[key].referenceModel.push(inValue.referenceModel);
					
					if($scope.tables[key].rows && $scope.tables[key].rows.pagesTotal){
						var pageTotal = $scope.tables[key].rows.pagesTotal;
						if (pageTotal < 10){
							$scope.paginationArray = [];
							for (var p = 1; p <= pageTotal; p++) {
								$scope.paginationArray.push(p);
							} 
						}
						else {
							$scope.paginationArray = ['1','2','3','4','5','6','7','8','9','10'];
						}
					}
					
					if(inValue.visible === 'Y') {
						$scope.referenceModelList[inValue.model] = inValue.referenceModel;
						if(inValue.type !== "19" && inValue.type !== "25"){
							$scope.allColumns.push(inValue);
						}
						if(inValue.type === "120" || inValue.type === "140" || inValue.type === "150") {
							$scope.tables[key].showBrowseBox = true;
						}
						$scope.tables[key].showColumns.push(inValue);
						if(inValue.type === "4") {
							$scope.tables[key].classDisplay.push("fullWidth");
						}  else {
							$scope.tables[key].classDisplay.push("");
						}
						
						$scope.tables[key].typeData.push(inValue.typeData);
						if(inValue.model !== ""){
							$scope.tables[key].model.push(inValue.model);
							if(inValue.tabIndex !== undefined && inValue.tabIndex !== "") {
								$scope.tables[key].tabIndex.push(inValue.tabIndex);
							} else {
								$scope.tables[key].tabIndex.push(inValue.elementSequence);
							}						
						};
					} else {
						if (inValue.type === "7" ){
							$scope.tables[key].imageData.push(inValue.model);
						}
						$scope.tables[key].hideColumns.push(inValue);
					}
				});
				
				//Get params for my evenList icReqHeader give the value
				if($scope.tables[key].eventList.click !== undefined) {
					if($scope.tables[key].eventList.click.methodParams !== undefined) {
						$scope.paramsValue = "";
						var paramsDefaultSplit = "";
						$scope.methodParamsSplit = $scope.tables[key].eventList.click.methodParams.split(",");
						if($scope.tables[key].eventList.click.paramsDefault !== undefined) {
							paramsDefaultSplit = $scope.tables[key].eventList.click.paramsDefault.split(",");
						}
						if($scope.tables[key].rows != undefined) {
							$scope.rowList = $scope.tables[key].rows;
							if($scope.rowList.rows != undefined) {
								$scope.rowList = $scope.rowList.rows;
							}
							//if is array I save in a eventParams the array with the respective value this was you use because in 
							//some cases the params come multiples divide example: icHeader, icLine
							if (angular.isArray($scope.rowList)) {	
								if($scope.rowList.length > 0) {
									for (var rl = 0; rl < $scope.rowList.length; rl++) {
										for (var mp = 0; mp < $scope.methodParamsSplit.length; mp++) {
											if($scope.rowList[rl][$scope.methodParamsSplit[mp]] != undefined) {
												$scope.paramsValue = $scope.paramsValue + $scope.rowList[rl][$scope.methodParamsSplit[mp]] + "/";
											}
										}
										$scope.tables[key].eventParams.push($scope.paramsValue);
										$scope.paramsValue = "";
									}
								} else {
									for (var pd = 0; pd < paramsDefaultSplit.length; pd++) {
										var findValueSplit = paramsDefaultSplit[pd].split(".");
										if(findValueSplit.length > 1) {
											var findValue = $scope.allData.data;
											for (var fv = 0; fv < findValueSplit.length; fv++) {
												if(findValue != undefined && !angular.isArray(findValue)) {
													findValue = findValue[findValueSplit[fv]];
												}
											}
											if(findValue !== undefined && findValue !== "")
												$scope.paramsValue = $scope.paramsValue + findValue + "/";
										}
									}
									$scope.tables[key].eventParams.push($scope.paramsValue);
								}
							} else {
								for (var mp = 0; mp < $scope.methodParamsSplit.length; mp++) {
									if($scope.rowList[$scope.methodParamsSplit[mp]] != undefined) {
										$scope.paramsValue = $scope.paramsValue + $scope.rowList[$scope.methodParamsSplit[mp]] + "/";
									}
								}
								if($scope.paramsValue !== "") {
									for (var pd = 0; pd < paramsDefaultSplit.length; pd++) {
										var findValueSplit = paramsDefaultSplit[pd].split(".");
										if(findValueSplit.length > 1) {
											var findValue = $scope.allData.data;
											for (var fv = 0; fv < findValueSplit.length; fv++) {
												if(findValue != undefined && !angular.isArray(findValue)) {
													findValue = findValue[findValueSplit[fv]];
												}
											}
											if(findValue !== undefined && findValue !== "")
												$scope.paramsValue = $scope.paramsValue + findValue + "/";
										}
									}
								}
								$scope.tables[key].eventParams.push($scope.paramsValue);
							}
						}
					}
				}
				
				// This is to access the buttons for the heading portlets. 
				if($scope.tables[key].portlet === "headingPortlet" || $scope.tables[key].portlet === "tileButtons" || $scope.tables[key].portlet === "btnGroup" ){
					angular.forEach($scope.tables[key].showColumns, function(inValue,inKey) {
						if(inValue.type === "19" || inValue.type === "25"){
							$scope.tables[key].showButtons.push(inValue);
						};
					});
				}
			}
		});
		$scope.existSections = true;
	};
	$log.debug($scope);
	// function for return bool, is column visible or not
	$scope.toShowCol = function(showCols) {
		return showCols;
	};
	
	$scope.createLine = function(tableLine) {
		var newService = tableLine.eventList.click.urlParams.replace('getCacheLine','createLine');
		var paramsDefaultSplit = "";
		var paramsValue = "";
		if(tableLine.eventList.click.paramsDefault !== undefined) {
			paramsDefaultSplit = tableLine.eventList.click.paramsDefault.split(",");
		}

		for (var pd = 0; pd < paramsDefaultSplit.length; pd++) {
			var findValueSplit = paramsDefaultSplit[pd].split(".");
			if(findValueSplit.length > 1) {
				var findValue = $scope.allData.data;
				for (var fv = 0; fv < findValueSplit.length; fv++) {
					if(findValue != undefined && !angular.isArray(findValue)) {
						findValue = findValue[findValueSplit[fv]];
					}
				}
				if(findValue !== undefined && findValue !== "")
					paramsValue = paramsValue + findValue + "/";
			}
		}
		$location.path(tableLine.eventList.click.eventMethod + "/" + newService + paramsValue);
	};

	// return int for html class to work correctly
	$scope.tableLength = function(table) {
		var i = 0;
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

	// Save document in repository and update the document in cache,
	// the document is update in repository and cache
	$scope.saveData = function() {
		$log.debug('MainCtrl: Save - Data');
		$log.debug(resolveData);
		$scope.load = true;
		// the service is invoked for this operation
		resolveData.saveData($scope.allData.data, $routeParams).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
			$scope.load = false;
			$scope.allDataObject = response.responseResult.result;
			$scope.copyData($scope.allDataObject);
	    }, function(response) {
	    	$log.debug(response);
	    });
	};
	
	$scope.saveDataLine = function(path,param1,param2) {
		$log.debug('MainCtrl: Save - Data');
		$log.debug(resolveData);
		$scope.load = true;
		// the service is invoked for this operation
		resolveData.saveData($scope.allData.data, $routeParams).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
			$location.path(path + param1 + '/' + param2);
	    }, function(response) {
	    	$log.debug(response);
	    });
	};
	
	// Save document in repository and update the document in cache,
	// the document is update in repository and cache
	$scope.approvalDocument = function() {
		$log.debug('MainCtrl: Save - Data');
		$log.debug(resolveData);
		// the service is invoked for this operation
		var newRow = 'notes:' + $scope.approval.notes;
		resolveData.approvalData('requisition', 'approve', newRow, $routeParams).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
			$location.path('openRouting/requisition/open/REQ_APPROVAL_FORWARD/' + $routeParams.icReqHeader);
	    }, function(response) {
	    	$log.debug(response);
	    });
	};
	
	// First manage DocAttachment moving or removing them from temp repository to internal Repository
	// Save document in repository and update the document in cache,
	// the document is update in repository and cache
	$scope.uploadAttachments = function() {
		$log.debug('MainCtrl: Save Attachment - Data');
		$log.debug(resolveData);
		// the service is invoked for this operation
		resolveData.uploadAttachments($scope.allData.data, $routeParams).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
			$scope.allDataObject = response.responseResult.result;
			$scope.copyData($scope.allDataObject);
			$scope.attachment = {};
	      }, function(response) {
	    	  	$log.debug(response);
	      });
	}; 

	// Save contents of document in the cache to repository,
	$scope.saveCache = function() {
		$log.debug('MainCtrl: Save Cache - Data');
		// the service is invoked for this operation
		resolveData.saveCache($scope.allData.data).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
	      }, function(response) {
	    	  	$log.debug(response);
	      });
	};

	// Save the document in cache
	$scope.putCacheData = function() {
		$log.debug('MainCtrl: Put Cache - Data');
		// the service is invoked for this operation
		resolveData.putCacheData($scope.allData.data, $routeParams).then(function(response) {
			$log.debug('putCacheSourceData - response:');
			$log.debug(response);
	      }, function(response) {
	    	  	$log.debug(response);
	      });
	};
	
	// Save the document in cache
	$scope.putCacheDataLine = function() {
		$log.debug('MainCtrl: Put Cache - Data');
		// the service is invoked for this operation
		resolveData.putCacheData($scope.allData.data, $routeParams).then(function(response) {
			$log.debug('putCacheLineSourceData - response:');
			$log.debug(response);
			$scope.allDataObject = response.responseResult.result;
			$scope.copyData($scope.allDataObject);
	      }, function(response) {
	    	  	$log.debug(response);
	      });
	};
	
	// Save the Accounts in cache
	$scope.putCacheDataAccount = function() {
		$log.debug('MainCtrl: Put Cache - Data');
		// the service is invoked for this operation
		resolveData.putCacheDataAccount($scope.allData.data, $routeParams).then(function(response) {
			$log.debug('putCacheSourceData - response:');
			$log.debug(response);
	      }, function(response) {
	    	  	$log.debug(response);
	      });
	};
	
	// Save contents of document in the cache to repository,
	$scope.createComment = function(boxModel) {
		$scope.comment = [];
		$scope.comment.push($scope.comments);

		// the service is invoked for this operation
		splitModelElement = boxModel.split(".");
		if(splitModelElement.length > 1) {
			$scope.allData.data[splitModelElement[0]][splitModelElement[1]] = $scope.comment;
		}
		$log.debug($scope.allData.data);
		resolveData.modifyCommentData($scope.allData.data, $routeParams, 'createComment').then(function(response) {
			$scope.allDataObject = response.responseResult.result;
			$scope.copyData($scope.allDataObject);
			$scope.comments = {
				docText: {}
			};
	      }, function(response) {
	    	  	$log.debug(response);
	      });
	};
	
	//Param button get the information about this element
	//table is for get all rows values and find the right param for the function.
	$scope.clickEventButton = function(button, table, rowIndex) {
		if($scope.exceedPercent) {
			alert("Percent must be equal 100%");
		} else if($scope.exceedAmount) {
			alert("Amount must equal " + $scope.totalLineAmount);
		} else {
			if(button.eventList.click !== undefined || button.eventList.click !== "") {
				button = button.eventList.click;
				var callPage = false; //This var is for know if callPage
				var callFunc = false; //This var is for know if only Function
				var strParamPage = ""; //Params in the page
				var strParamFunc = ""; // Params in the Function
				var positionCall = [];
				if(button.eventAction !== undefined && button.eventAction !== "") {
					var eventActionSplit = button.eventAction.split(",");
					for(var ea = 0 ; ea < eventActionSplit.length ; ea++) {
						var eventActions = eventActionSplit[ea];
						switch (eventActions) {
							case 'callPage':
								positionCall.push("callPage");
								break;
							case 'callFunc':
								positionCall.push("callFunc");
								break;
							case 'L':
								if(button.methodParams !== undefined && button.methodParams !== "") {
									strParamFunc = button.methodParams;
								}
								break;
							case 'B':
								if(button.methodParams !== undefined && button.methodParams !== "") {
									strParamPage = button.methodParams;
									strParamFunc = button.methodParams;
								}
								break;
							case 'R':
								if(button.methodParams !== undefined && button.methodParams !== "") {
									strParamPage = button.methodParams;
								}
								break;
						}	
					}
					for(var pc = 0 ; pc < positionCall.length ; pc++) {
						if(positionCall[pc] === "callFunc") {
							$scope.callFunc(button.eventMethod, button.paramsDefault, strParamFunc, table.rows, rowIndex);
						} else if(positionCall[pc] === "callPage") {
							setTimeout(function() {$scope.callPage(button.urlParams, button.paramsDefault, strParamPage, table.rows);}, 1000);
						} 
					}
				} else {
					//if the button contain value in urlParams is because call a $location
					//else is a normal function and send the method and params if is neccesary
					if(button.urlParams !== undefined && button.urlParams !== "") {
						$scope.callPage(button.eventMethod + button.urlParams, button.paramsDefault, button.methodParams, table.rows);
					} else {
						if(button.eventMethod !== undefined && button.eventMethod !== "") {
							$scope.callFunc(button.eventMethod, button.paramsDefault, button.methodParams, table.rows, rowIndex);
						}
					}
				}
			}
		}
	};
	
	$scope.callPage = function(urlParams, paramsReference, strParamPage, rows) {
		var paramsValue = "";
		if(strParamPage !== undefined && strParamPage !== "") {
			var methodParamsSplit = strParamPage.split(",");
			var paramsReferenceSplit = "";
			if(paramsReference !== undefined && paramsReference !== "") {
				paramsReferenceSplit = paramsReference.split(",");
			}
			paramsValue = $scope.getValue(methodParamsSplit, paramsReferenceSplit, rows, paramsValue);
		}
			
		$log.debug(urlParams + paramsValue);
		$location.path(urlParams + paramsValue);
	};

	$scope.callFunc = function(eventMethod, paramsReference, strParamFunc, rows, rowIndex) {
		var paramsValue = "";
		if(strParamFunc !== undefined && strParamFunc !== "" && eventMethod !== "removeRow") {
			var methodParamsSplit = strParamFunc.split(",");
			var paramsReferenceSplit = "";
			if(paramsReference !== undefined && paramsReference !== "") {
				paramsReferenceSplit = paramsReference.split(",");
			}
			paramsValue = $scope.getValue(methodParamsSplit, paramsReferenceSplit, rows, paramsValue);
		} else if(eventMethod === "removeRow") {
			paramsValue = strParamFunc;
		}
		if(eventMethod !== undefined && eventMethod !== "") {
			var strFun = eventMethod;
			$log.debug($window);
			//Create the function
			var fn = $scope[strFun];

			//Call the function and pass the params and rowIndex
			fn(paramsValue, rowIndex);
		}
		return "";
	};

	$scope.goPageLocation = function(path,param1,param2) {
		$location.path(path + param1 + '/' + param2);
	};

	$scope.getValue = function(methodParamsSplit, paramsReferenceSplit, rows, paramsValue) {
		$scope.getObj = "";
		for(var mp = 0 ; mp < methodParamsSplit.length ; mp++) {
			var getValue = false;
			var methodParam = methodParamsSplit[mp];
			var paramReference = "";
			if(paramsReferenceSplit[mp] !== undefined && paramsReferenceSplit[mp] !== null) {
				paramReference = paramsReferenceSplit[mp];
			}
			
			if(rows !== undefined && rows !== "" && methodParam.indexOf(".") < 0) {
				if(rows[methodParam] !== undefined && rows[methodParam] !== "") {
					paramsValue = paramsValue +rows[methodParam] +"/";
					getValue = true;
				}
				if(!getValue && rows[paramReference] !== undefined && rows[paramReference] !== "") {
					paramsValue = paramsValue + rows[paramReference] +"/";
					getValue = true;
				}
			} 
			if(!getValue) {
				paramsValue = paramsValue + methodParam;
				if(methodParamsSplit.length > (mp + 1)) {
					paramsValue = paramsValue + "/";
				}
			}
		}
		return paramsValue;
	};
	
	//With this function call openBrowse for open the user profile list we send the browseId and pageId
	$scope.addApprover = function(fyi) {
		$scope.returnNewRow = true;
		$scope.fyi = fyi;
		$scope.openBrowsePage('APPROVER_BROWSE', 'REQ_ROUTING_BROWSE', '');
	};
	
	
	
	$scope.move = function(index,sum){
		var rows = [];
		angular.copy($scope.tables[1].rows, rows);
		var length = rows.length;
		var add = parseInt(sum);
		var go = false;
		if (add === 1 && index < length - 1){
			go = true;
		}	
		if (add === -1  && index > 0){
			go = true;
		}
		if(go){
//			var oldRow = {};
//			angular.copy(rows[index + add], oldRow);
//			var row = {};
//			angular.copy(rows[index], row);			
//			angular.copy(row,rows[index + add]);
//			angular.copy(oldRow,rows[index]);
//			angular.copy(rows,$scope.tables[1].rows);
			var incomingParams = 'row:' + index + ',newRow:' + (index + add);
			resolveData.addApprover('requisition', 'REQ_ROUTING', 'updateApprovers', incomingParams, $routeParams).then(function(response) {
				$log.debug('saveSourceData - response:');
				$log.debug(response);	
				$scope.allDataObject = response.responseResult.result;
				$scope.copyData($scope.allDataObject);
		    }, function(response) {
		    	$log.debug(response);
		    });
		}		
	};
	
	//browseId for retrieve data, pageId for know what element display in the browse, model the value for the filter
	$scope.openBrowsePage = function(browseId, pageId, model) {
		BrowsePopupOpenService.openBrowseRoute(browseId, pageId, model).then(function(responseBrowse) {
			$scope.showPopup = true;
			inners = {};
			var b = -1; 
			var browseOjectData = {};
			var allDataOld = {};
			angular.copy($scope.allData, allDataOld);	
			angular.copy($scope.allDataObjectBackup, $scope.allData);				
			angular.copy(allDataOld, browseOjectData);	
			$scope.allDataBrowse = responseBrowse.responseResult.result;
			browseOjectData.data[$scope.allDataBrowse.metadata.boxList[0].boxId] = $scope.allDataBrowse.data[$scope.allDataBrowse.metadata.boxList[0].boxId];
			browseOjectData.metadata.boxList = $scope.allData.metadata.boxList.concat($scope.allDataBrowse.metadata.boxList);			
			angular.copy(browseOjectData, $scope.allData);
			angular.forEach(browseOjectData.metadata.boxList, function(value, key) {
				if (key !== "HEADER_MENU") {
					b++;
					if (value.elementList.length > 0) {
						inners[b] = value.elementList[0].labelId;
					}
				}
			});			
			$scope.setReloaded(false);
			makeTables();
		});
	};
	
	$scope.openBrowseId = function(browseId, model) {
		BrowsePopupOpenService.openBrowseRoute(browseId, '', model).then(function(responseBrowse) {
			$log.debug(responseBrowse.responseResult.result);
			$scope.tables[3].rows.rows = responseBrowse.responseResult.result.data.REQ_REC_ITE_FIL_BROWSE.rows;
			$scope.itemDetails = true;
			$scope.setReloaded(false);
		});
	};
	
	$scope.openBrowseField = function(browseId, pageId, model, index) {
		BrowsePopupOpenService.openBrowseRoute(browseId, pageId, model).then(function(responseBrowse) {
			
			$scope.showPopUp[index] = true;
			var browseOjectResult = {};
			browseOjectResult = responseBrowse.responseResult.result;
			$scope.elementColums[index] = [];//
			$scope.elementHiddens[index] = [];
			$scope.elementRows[index] = [];
			$scope.elementModels[index] = [];	
			$scope.elementMetadata[index] = {};
			$scope.elementMetadata[index].allModel = [];
			$scope.elementMetadata[index].referenceModel = [];
			angular.copy($scope.allData.data,$scope.allDataObject.data);
			inners = {};
			i = -1; 			
			angular.forEach(browseOjectResult.metadata.boxList, function(value, key) {
				if (key !== "HEADER_MENU") {
					i++;
					if (value.portletType != undefined) {
						inners[i] = value.portletType;
					}
					if(value.type == "POPUPBROWSE") {
						$scope.showPopup = true;
					}
				}
			});			
			angular.forEach(inners, function(value, key) {
				// var rowNumberIndex = 0; // for know the index of the row
				var columnLabels = false; // first initializing labels for each column
				if (typeof browseOjectResult.metadata.boxList[key].elementList !== "undefined") {
					columnLabels = browseOjectResult.metadata.boxList[key].elementList; //add values to columnLabels array
				}

				// adding rows, columns and title
				if (columnLabels !== false) {			

					angular.forEach(columnLabels, function(inValue,inKey) {
						if(inValue.visible === 'Y') {
					
							$scope.elementColums[index].push(inValue);							
							if(inValue.model !== ""){
								$scope.elementModels[index].push(inValue.model);												
							};
						} else {		
							
							$scope.elementHiddens[index].push(inValue);
							
						}
						$scope.elementMetadata[index].allModel.push(inValue.model);
						$scope.elementMetadata[index].referenceModel.push(inValue.referenceModel);
						$scope.elementRows[index] = browseOjectResult.data[browseId]["rows"];						
					});
				}
			});
			$scope.setReloaded(false);  
		});
	};
	
	
	
	
	//This part is for hide section when the $routeParams contain icLine and compare with each section of template lineListEdit
	//and hide the section hasn't the same icLine.
	$scope.displayList = function(tableRow, table, index) {
		var tableReference = table.referenceModel;
		if($routeParams.icReqLine != null && $routeParams.icReqLine != undefined)
		{
			angular.forEach(tableReference, function(inValue,inKey) {
				if(inValue != "") {
					$scope.valueRow = $scope.allData.data;
					splitReference = inValue.split(".");
					for(i = 0; i < splitReference.length; i++) {
						if (angular.isArray($scope.valueRow[splitReference[i]])) {	
							$scope.valueRow = $scope.valueRow[splitReference[i]][index];
						} else {
							$scope.valueRow = $scope.valueRow[splitReference[i]];
						}
					}				
				}
			});
			if(tableRow != undefined && $scope.valueRow != null) {
				if($scope.valueRow === $routeParams.icReqLine) {
					return false;
				}
			}
			return true;
		}
		return false;
	};
	
	//row is the actual position where insert the new row
	//table is for get the referenceModel example: RequisitionObject.lines.line.icReqLine
	//index is for get the value in the case the row has array list and need to get the right value
	//with this function I create a new row with only the icheader and icline if has this addRow is work for account and comments
	//the any module req, po, ivc, etc
	$scope.addRow = function(row, index, table) {
//		var tableReference = table.referenceModel;
//		var newRow = '{"icHeader":"' + $routeParams.icReqHeader + '",';
//		$scope.valueRow = "0";
//		angular.forEach(tableReference, function(inValue,inKey) {
//			if(inValue != "") {
//				$scope.valueRow = $scope.allDataObject.data;
//				splitReference = inValue.split(".");
//				for(i = 0; i < splitReference.length; i++) {
//					if (angular.isArray($scope.valueRow[splitReference[i]])) {	
//						$scope.valueRow = $scope.valueRow[splitReference[i]][index];
//					} else {
//						$scope.valueRow = $scope.valueRow[splitReference[i]];
//					}
//				}				
//			}
//		});
//		newRow += '"icLine":"' + $scope.valueRow + '",';
//		newRow += '}';
//		newRow = newRow.replace(",}", "}");
//		var jsonObj = JSON.parse(newRow);
//		row.push(jsonObj);
		$scope.totalPercent = 0;
		if(row.length > 0) {
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalPercent = $scope.totalPercent + parseFloat(inValue.allocPercent);
				$scope.totalPercent = +(Math.round($scope.totalPercent + 'e+5') + 'e-5');
			});
		}
		var newRow = "{";
		$scope.substract = 100 - $scope.totalPercent;
		if($scope.allDataObject.data[table.templateId]["extrinsic"] !== undefined) {
			angular.forEach($scope.allDataObject.data[table.templateId]["extrinsic"], function(inValue,inKey) {
				if(inKey === "allocPercent" && row.length > 0) {
					if($scope.totalPercent < 100) {
						newRow += '"' + inKey + '":"' + (100 - $scope.totalPercent) + '",';
					} else {
						newRow += '"' + inKey + '":"0",';
					}
				} else if( inKey === "allocAmount" && row.length > 0) {
					if($scope.totalPercent < 100) {
						newRow += '"' + inKey + '":"' + ($scope.totalLineAmount * $scope.substract) / 100 + '",';
					} else {
						newRow += '"' + inKey + '":"0",';
					}
				} else if( inKey === "allocMethod") {
					newRow += '"' + inKey + '":"' + $scope.checkAccountMethod  + '",';
				} else {
					newRow += '"' + inKey + '":"' + inValue + '",';
				}
			});

			newRow += '}';
			newRow = newRow.replace(",}", "}");
			var jsonObj = JSON.parse(newRow);
			row.push(jsonObj);
		}

		angular.forEach(row, function(inValue,inKey) {
			inValue.allocMethod = $scope.checkAccountMethod;
		});
	};

	//With this remove the row select only need the listRowIndex is the index and the row for remove the element
	$scope.removeRowOnPage = function(listRowIndex, row) {	
		row.splice(listRowIndex, 1);
		if($scope.checkAccountMethod === "PL") {
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalPercent = $scope.totalPercent + parseFloat(inValue.allocPercent);
				$scope.totalPercent = +(Math.round($scope.totalPercent + 'e+5') + 'e-5');
			});
			if($scope.totalPercent !== 100 && row.length > 0) {
				$scope.exceedPercent = true;
			} else if(row.length < 1) {
				$scope.exceedPercent = false;
			}
		}
		if($scope.checkAccountMethod === "AL") {
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalAmount = $scope.totalAmount + parseFloat(inValue.allocAmount);
				$scope.totalAmount = +(Math.round($scope.totalAmount + 'e+5') + 'e-5');
			});
			if($scope.totalAmount !== $scope.totalLineAmount && row.length > 0) {
				$scope.exceedAmount = true;
			} else if(row.length < 1) {
				$scope.exceedAmount = false;
			}
		}
	};
	
	$scope.searchFilter = function(filter) {
		$scope.queryBrowse = filter;
	};
	
	//Return Data when you select a element from popupBrowse
	$scope.returnData = function(tableBrowse, row, index) {	
		if($scope.allDataObject.metadata.model.indexOf("routing") > 0) {
			//newRow is for send the variables for default go to the approvallog
			var newRow = 'fyiApprover:' + $scope.fyi + ',';
			newRow += 'amount:' + $scope.allDataObject.data.RequisitionObject.routingList[0]["amount"] + ',';
			newRow += 'udfValues:' + $scope.allDataObject.data.RequisitionObject.routingList[0]["udfValues"] + ',';
			newRow += 'userId:' + row["userId"];
			resolveData.addApprover('requisition', 'REQ_ROUTING', 'addApprover', newRow, $routeParams).then(function(response) {
				$log.debug('saveSourceData - response:');
				$log.debug(response);
				$scope.allDataObject = response.responseResult.result;
				$scope.copyData($scope.allDataObject);
		      }, function(response) {
		    	  	$log.debug(response);
		      });
		} else if($scope.allDataObject.metadata.model.indexOf("comments") > 0) {
			$scope.comments.docText.stdText = row["stdText"].replace(/'/g, "\'");
			$scope.comments.commentId = row["commentId"];
			$scope.comments.commentPrint = row["commentPrint"];
			$scope.comments.commentTitle = row["commentTitle"].replace(/'/g, "\'");
			$scope.createComment($scope.allDataObject.metadata.model);
		} else if($scope.allDataObject.metadata.model === "QuestionObject" || $scope.allDataObject.metadata.model === "RequisitionLineObject") {
			//This is when you return value from browse to individual fields
			for(var keyObject in $scope.allDataObject.data) {
				for(var keyModel in tableBrowse.allModel) {
					$scope.val = tableBrowse.allModel[keyModel];
					$scope.rowValue = row[$scope.val];
					$scope.referenceRow = tableBrowse.referenceModel[keyModel];
					splitReference = $scope.referenceRow.split(".");
					if(splitReference.length > 1) {
						if(splitReference.length < 2) {
							$scope.allDataObject.data[splitReference[0]] = $scope.rowValue;
						} else if(splitReference.length < 3) {
							$scope.allDataObject.data[splitReference[0]][splitReference[1]] = $scope.rowValue;
						} else if(splitReference.length < 4) {
							$scope.allDataObject.data[splitReference[0]][splitReference[1]][splitReference[2]] = $scope.rowValue;
						}
					}
				}
				break;
			}
			if (index !== undefined)
			{
				$scope.showPopUp[index] = false;
			}
			$scope.copyData($scope.allDataObject);
		} else {
			//This is when you return value from browse to individual fields
			for(var keyObject in $scope.allDataObject.data) {
				for(var keyModel in tableBrowse.allModel) {
					$scope.val = tableBrowse.allModel[keyModel];
					$scope.rowValue = row[$scope.val];
					$scope.referenceRow = tableBrowse.referenceModel[keyModel];
					splitReference = $scope.referenceRow.split(".");
					if(splitReference.length > 1) {
						if(splitReference.length < 2) {
							$scope.allDataObject.data[keyObject][splitReference[0]] = $scope.rowValue;
						} else if(splitReference.length < 3) {
							$scope.allDataObject.data[keyObject][splitReference[0]][splitReference[1]] = $scope.rowValue;
						} else if(splitReference.length < 4) {
							$scope.allDataObject.data[keyObject][splitReference[0]][splitReference[1]][splitReference[2]] = $scope.rowValue;
						}
					}
				}
				break;
			}
			if (index !== undefined)
			{
				$scope.showPopUp[index] = false;
			}
			$scope.copyData($scope.allDataObject);
		}
			
	};
	
	$scope.copyData = function(data) {
		$log.debug(data);
		angular.copy(data, $scope.allData);
		$scope.showPopup = false;
		inners = {};
		i = -1; 
		
		angular.forEach($scope.allDataObject.metadata.boxList, function(value, key) {
			if (key !== "HEADER_MENU") {
				i++;
				if (value.portletType != undefined) {
					inners[i] = value.portletType;
				}
				if(value.type == "POPUPBROWSE") {
					$scope.showPopup = true;
				}
			}
		});
		makeTables();
	};

	// call function to get all JSON data.
	getDataJSON();
	
	//With this remove the row select only need the listRowIndex is the index and the row for remove the element 
	//and reorden the sequence
	$scope.removeRowRouting = function(listRowIndex, row) {	
		var newRow = 'row:' + listRowIndex;
		resolveData.addApprover('requisition', 'REQ_ROUTING', 'removeApprover', newRow, $routeParams).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
			$scope.allDataObject = response.responseResult.result;
			$scope.copyData($scope.allDataObject);
	    }, function(response) {
	    	$log.debug(response);
	    });
	};
	
	$scope.removeRow = function(params, index) {	
		var deleteRow = 'row:' + index;
		splitModel =  params.split(",");
		eval('var obj=' + params);
		resolveData.removeRowData(obj.module, obj.service, obj.pageId, deleteRow, $routeParams).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
			$scope.allDataObject = response.responseResult.result;
			$scope.copyData($scope.allDataObject);
	    }, function(response) {
	    	$log.debug(response);
	    });
	};
	
	$scope.removeRowLine = function(params, index) {	
		var deleteRow = 'row:' + index;
		splitModel =  params.split(",");
		resolveData.removeRowData(splitModel[0], splitModel[1], splitModel[2], deleteRow, $routeParams).then(function(response) {
			$log.debug('saveSourceData - response:');
			$log.debug(response);
			$scope.allDataObject.data.RequisitionObject = response.responseResult.result.data.RequisitionObject;
			$scope.copyData($scope.allDataObject);
	    }, function(response) {
	    	$log.debug(response);
	    });
	};
	
	$scope.commentPlaceTypes =[
	{
		id : 'A', name : 'After' 
	},
	{
		id : 'B', name : 'Before' 
	}];
	// Upload files asynchronous using Box to fill that file in this table.row
	// with file information
	// url or file name of file generated in server is store in file.originalFilename
	 $scope.uploadFile = function(file){
	        console.log('File are ' + JSON.stringify(file));
	        var uploadUrl = "/puridiom5/procureUpload";
	        resolveData.uploadFileToUrl(file, uploadUrl,$scope.attachment).then(function(response) {
				$log.debug('Upload File - response:');
				/*	Find Table from all Box in View where boxModel is the same but is not that boxId
				*	for example if upload buttons are in heading portlet and rows are in another portlet with this funtion
				*	it will find the container of rows and not heading portlet
				*/
				var tableObject = $scope.objectFindByKey($scope.tables,"boxModel",$scope.attachment.boxModel,"boxId",$scope.attachment.boxId);
				//Updating tempAttachments and showing information
				var resultFile = response.responseResult.result;
				$scope.addAttachmentRow(tableObject.rows,-1, tableObject, resultFile.docAttachment);
		      }, function(response) {
		    	  	$log.debug(response);
		      });
	 };
	 
	//With this validate target Files and then call uploadFile function asyn
	 $scope.addAttachment = function(){
		var files = $scope.attachment.file;
	 	for(var f = 0 ; f < files.length; f++){
	 		if(files && files[f]){
	 			var reader = new FileReader();
	 			reader.onload = (function(file){
	 					$scope.uploadFile(file);
	 			})(files[f]);
	 			reader.readAsDataURL(files[f]);
	 		}
	 	}
	 };
	    
	 
	 $scope.addAttachmentRow = function(row, index, table, docAttachment) {
			var newRow = '{"icHeader":"' + $routeParams.icReqHeader + '",';
			$scope.valueRow = "0";
			if($routeParams.icReqLine){
				$scope.valueRow = $routeParams.icReqLine;
			}
			newRow += '"icLine":"' + $scope.valueRow + '",';
			newRow += '"docFilename":"' + docAttachment.docFilename + '",';
			newRow += '"docFilesize":"' + docAttachment.docFilesize + '",';
			newRow += '"docPrint":"' + docAttachment.docPrint + '",';
			newRow += '"docTitle":"' + docAttachment.docTitle + '",';
			newRow += '"docFiletype":"' + docAttachment.docFiletype + '",';
			newRow += '"docFilesize":"' + docAttachment.docFilesize + '",';
			newRow += '"internalFilename":"' + docAttachment.internalFilename + '",';
			newRow += '}';
			newRow = newRow.replace(",}", "}");
			var jsonObj = JSON.parse(newRow);
			row.push(jsonObj);	
	};
	    
	$scope.objectFindByKey = function(array, key, value, notkey, notvalue) {
	    for (var i = 0; i < array.length; i++) {
	        if (array[i][key] === value && array[i][notkey] !== notvalue) {
	            return array[i];
	        }
	    }
	    return null;
	};
	/*------------------------------CATALOG STUFF------------------------*/
	/*------------------------------CATALOG STUFF------------------------*/
	$scope.selectedIndex1 = 'false';
	$scope.selectedIndex2 = 'false';
	$scope.selectedIndex3 = 'false';
	$scope.selectedIndex4 = 'false';

	$scope.truePlease = 'true';
	$scope.falsePlease = 'false';

	$scope.showAccounts = 'true';
	$scope.showComments = 'false';
	$scope.showAttachments = 'false';

	$scope.filtersOpen = 'false';

	//getting groups

	$scope.groups = [];
	$scope.metaData = [];
	$scope.activeGroup ={};
	
	var getGroups = function(){

//		CatalogService.getSearchJson()
//		.then(function(res){
//			$scope.groups = res.data.responseResult.result.data;
//			
//			$scope.metaData = res.data.responseResult.result.metadata;
//
//			angular.forEach($scope.metaData.boxList,function(value,key){
//				
//				$scope.groups[value.boxId].labelTitle = value.labelTitle;
//				$scope.groups[value.boxId].boxId = value.boxId;
//				$scope.groups[value.boxId].elementList = value.elementList;
//				
//			});
//		}).then(function(){
//			if($routeParams.category){
//				$scope.activeGroup = $scope.groups[$routeParams.category];
//				
//			}
//		});
	};

	getGroups();

	$scope.viewAllClick = function(name) {
		$location.path("/CatalogSearch_viewAll/" + name + "/0/0"); 
	};

	$scope.getText=function(row,element) {	
		return row[element.model];
	};

	$scope.cleanCache = function() {
		$scope.cacheButton = true;
		 resolveData.cleanCache().then(function(response) {
			$log.debug('clean Cache response:' + response);
			$scope.messageCache = 'Your cache was successfully cleared';
			$scope.cacheButton = false;
		}, function(response) {
		 	$log.debug(response);
		});
	};

	$scope.saveResponseQa = function() {
		$scope.validateBox = true;
		$scope.load = true;
		if(!$scope.showAnswer) {
			$log.debug($scope.allData);
			$scope.parseParams = [];
			angular.forEach($scope.allData.data.ResponseQa, function(value, key) {
				if(value !== "" && value !== undefined) {
					var newRow = '{"question":"' + key + '",';
					newRow += '"response":"' + value + '"}';
					angular.forEach($scope.referenceModelList, function(refValue, refKey) {
						if(refValue !== "" && refValue !== undefined) {
							if(refKey === key && refValue !== undefined && value !== "") {
								$scope.parseParams.push(refValue + ":" + value);
							}
						}
					});	
					var jsonObj = JSON.parse(newRow);
					$scope.ResponseQaList.push(jsonObj);
				}
			});	
			if($scope.showQuestionSection !== undefined) {
				for(var sq = 1; $scope.showQuestionSection.length > sq; sq ++) {
					$scope.showQuestionSection[sq] = true;
				}
			};
			$scope.navigate_left_white = true;
			$scope.navigate_right_white = true;
			$scope.check_white = true;
			$scope.showAnswer = true;
			
			resolveData.saveResponseData($scope.ResponseQaList, $routeParams).then(function(response) {
				$scope.check_white = false;
				$scope.load = false;
				$log.debug('saveResponseData - response:');
				$log.debug(response);
		    }, function(response) {
		    	$log.debug(response);
		    });
		} else {
			if($scope.parseParams !== undefined) {
				resolveData.createDocument($scope.parseParams, $routeParams).then(function(response) {
					ContainerData.response = response;
					$log.debug('call multiProcess - response:');
					$log.debug(response);
					$location.path('MultiAction');
			    }, function(response) {
			    	$log.debug(response);
			    });
			}
		}
	};

	$scope.callMultiAction = function(processKey) {
		$scope.load = true;
		resolveData.callMultiAction(processKey, $routeParams).then(function(response) {
			ContainerData.response = response;
			$log.debug('call multiProcess - response:');
			$log.debug(response);
			$location.path('MultiAction');
	    }, function(response) {
	    	$log.debug(response);
	    });
	};

	$scope.getReloaded  = function() {
		if (!ContainerData.reloaded)
			return false;
		else
			return ContainerData.reloaded;
	};

	$scope.setReloaded = function(value) {
		ContainerData.reloaded = value;
	};

	/*------------------------------CATALOG STUFF------------------------*/
	$scope.additemToShoppingCart = function(row) {
		//alert($scope.checkOutItemsTotal);
		//alert(row.quantityBox);
		var itemFound = false;		
		if (!row.quantityBox)
			row.quantityBox = 0;
		if (row.unitPrice) {
			row.cost = row.unitPrice;
		}
		var checkOutItemsTotal = 1;		
		for (var i = 0; i < $scope.checkOutItems.length ; i++){
			itemFound = checkItem(row,$scope.checkOutItems[i]);
			if (itemFound){
				$scope.checkOutItems[i].quantity = $scope.checkOutItems[i].quantity  + row.quantityBox;
				$scope.checkOutItems[i].total = $scope.checkOutItems[i].quantityBox * row.cost;
				row.quantityBox = '';
				break;
			};
		}		
		if (!itemFound){
			row.total = row.quantityBox * row.cost;
			row.quantity = row.quantityBox;
			row.quantityBox = '';
			if (!row.priceBrk){
				row.priceBrk = '3';
			}
			$scope.checkOutItemsTotal = $scope.checkOutItemsTotal + checkOutItemsTotal;	
			$scope.checkOutItems.push(row);
		}		
		
		ContainerData.catalog = $scope.checkOutItems;

	};

	checkItem = function(itemToAdd,itemAdded) {
		if (itemToAdd.catalogId !== undefined && itemToAdd.itemNumber !== undefined){
			if (itemAdded.catalogId !== undefined && itemAdded.itemNumber !== undefined){
				return itemToAdd.catalogId === itemAdded.catalogId && itemToAdd.itemNumber === itemAdded.itemNumber;
			}
			else
				return false;
		}
		else if (itemToAdd.itemLocation !== undefined && itemToAdd.itemNumber !== undefined){
			if (itemAdded.itemLocation !== undefined && itemAdded.itemNumber !== undefined){
				return itemToAdd.itemLocation === itemAdded.itemLocation && itemToAdd.itemNumber === itemAdded.itemNumber;
			}
			else
				return false;
		}
		else if (itemToAdd.itemNumber !== undefined){
			if (itemAdded.itemNumber !== undefined){
				return itemToAdd.itemNumber === itemAdded.itemNumber;
			}
			else
				return false;
		}
	};

	$scope.goToPage = function(name,page) {
		$location.path("/CatalogSearch_viewAll/" + name + "/0/" + page);
	};
	/*------------------------------CATALOG STUFF------------------------*/

	$scope.puridiomPath = function(createText) {
		if(createText != undefined) {
			if(createText.toLowerCase().indexOf('check') >= 0) {
				$location.path('/question/REQK_QUESTION_PAGE/K');
			} else if(createText.toLowerCase().indexOf('expense') >= 0) {
				$location.path('/question/REQX_QUESTION_PAGE/X');
			} else if(createText.toLowerCase().indexOf('purchase req') >= 0) {
				$location.path('/createRequisition/requisition/create/REQP_REVIEW/P');
			} else {
				ContainerData.filter = createText;
				$location.path('/CatalogSearch');
			}
		} 
		else {
			$location.path('/CatalogSearch');
		}
	};

	$scope.showDetails = function() {
		if($scope.itemDetails) {
			$scope.itemDetails = false;
		} else {
			$scope.itemDetails = true;
		}
	};

	// method that checks the current phase before executing the function, this prevents '$apply already in progress' error happens. 
	$scope.safeApply = function(fn) {
		var phase = this.$root.$$phase;
		if (phase == '$apply' || phase == '$digest') {
			if(fn && (typeof(fn) === 'function')) {
				fn();
			}
		} else {
			this.$apply(fn);
		}
	};

	$scope.calculateTotal = function(row,model,checkAccountMethod) {
		$scope.totalPercent = 0;
		$scope.totalAmount = 0;
		$scope.exceedPercent = false;
		$scope.exceedAmount = false;
		if(checkAccountMethod === "PL") {
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalPercent = $scope.totalPercent + parseFloat(inValue.allocPercent);
				$scope.totalPercent = +(Math.round($scope.totalPercent + 'e+5') + 'e-5');
			});
			if($scope.totalPercent !== 100) {
				$scope.exceedPercent = true;
			}
			angular.forEach(row, function(inValue,inKey) {
				var div = parseFloat(inValue.allocPercent) / 100;
				inValue.allocAmount = div * $scope.totalLineAmount;
				inValue.allocAmount = +(Math.round(inValue.allocAmount + 'e+5') + 'e-5');
			});
			$log.debug(row);
		}
		if(checkAccountMethod === "AL") {
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalAmount = $scope.totalAmount + parseFloat(inValue.allocAmount);
				$scope.totalAmount = +(Math.round($scope.totalAmount + 'e+5') + 'e-5');
			});
			if($scope.totalAmount !== $scope.totalLineAmount) {
				$scope.exceedAmount = true;
			} 
			angular.forEach(row, function(inValue,inKey) {
				var mul = parseFloat(inValue.allocAmount) * 100;
				inValue.allocPercent = mul / $scope.totalLineAmount;
				inValue.allocPercent = +(Math.round(inValue.allocPercent + 'e+5') + 'e-5');
			});
			$log.debug(row);
		}
	};

	$scope.getAllocAmountTotal = function(row) {
		if(!$scope.defaultAmount) {
			$scope.defaultAmount = true;
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalLineAmount = $scope.totalLineAmount + parseFloat(inValue.allocAmount);
				$scope.totalLineAmount = +(Math.round($scope.totalLineAmount + 'e+5') + 'e-5');
			});
		}
	};

	$scope.allocMethodValue = function(row, checkAccount) {
		$scope.totalPercent = 0;
		$scope.totalAmount = 0;
		$scope.exceedPercent = false;
		$scope.exceedAmount = false;

		if(!$scope.defaultAmount && row.length > 0) {
			$scope.defaultAmount = true;
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalLineAmount = $scope.totalLineAmount + parseFloat(inValue.allocAmount);
				$scope.totalLineAmount = +(Math.round($scope.totalLineAmount + 'e+5') + 'e-5');
			});
		}

		angular.forEach(row, function(inValue,inKey) {
			inValue.allocMethod = checkAccount;
		});
		if(checkAccount === "PL") {
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalPercent = $scope.totalPercent + parseFloat(inValue.allocPercent);
				$scope.totalPercent = +(Math.round($scope.totalPercent + 'e+5') + 'e-5');
			});
			if($scope.totalPercent !== 100) {
				$scope.exceedPercent = true;
			}
		}
		if(checkAccount === "AL") {
			angular.forEach(row, function(inValue,inKey) {
				$scope.totalAmount = $scope.totalAmount + parseFloat(inValue.allocAmount);
				$scope.totalAmount = +(Math.round($scope.totalAmount + 'e+5') + 'e-5');
			});
			if($scope.totalAmount !== $scope.totalLineAmount) {
				$scope.exceedAmount = true;
			} 
			$log.debug(row);
		}
		$scope.checkAccountMethod = checkAccount;
	};

	$scope.getChecked = function(type,table) {
		 angular.forEach($scope.allDataObject.data[table.templateId]["extrinsic"], function(inValue,inKey) {
			if(inKey === "allocMethod" && $scope.checkAccountMethod === undefined) {
				if (inValue === type) {
					$scope.checkAccountMethod = inValue;
		        }
			}
		});
	};
	
	$scope.backBox = function() {	
		$scope.setupQuestionSection = false;
		if($scope.showQuestionSection !== undefined) {
			for(var sq = 1; $scope.showQuestionSection.length > sq; sq ++) {
				if(!$scope.setupQuestionSection) {
					var questionSec = $scope.showQuestionSection[sq]; 
					if(questionSec) {
						if((sq - 1) !== 0) {
							$scope.showQuestionSection[sq - 1] = true;
							$scope.showQuestionSection[sq] = false;
							$scope.setupQuestionSection = true;
							$scope.currentBox = sq - 1;
						} else {
							$scope.showQuestionSection[$scope.showQuestionSection.length - 1] = true;
							$scope.showQuestionSection[sq] = false;
							$scope.currentBox = $scope.showQuestionSection.length - 1;
						}
						$scope.navigate_left_white = true;
						$scope.navigate_right_white = true;
						$scope.check_white = true;
						if ($scope.validateBox){
							$scope.navigate_left_white = false;
							$scope.navigate_right_white = false;
							$scope.check_white = false;
						}
					}
				}
			}
		}
		watchQuiz();
	};
	
	$scope.nextBox = function() {	
		$scope.setupQuestionSection = false;
		if($scope.showQuestionSection !== undefined) {
			for(var sq = 1; $scope.showQuestionSection.length > sq; sq ++) {
				if(!$scope.setupQuestionSection) {
					var questionSec = $scope.showQuestionSection[sq]; 
					if(questionSec) {
						if((sq + 1) !== $scope.showQuestionSection.length) {
							$scope.showQuestionSection[sq + 1] = true;
							$scope.showQuestionSection[sq] = false;
							$scope.setupQuestionSection = true;
							$scope.currentBox = sq + 1;
						} else {
							$scope.showQuestionSection[1] = true;
							$scope.showQuestionSection[sq] = false;
							$scope.currentBox = 1;
						}
						$scope.navigate_left_white = true;
						$scope.navigate_right_white = true;
						$scope.check_white = true;
						if ($scope.validateBox === true){
							$scope.navigate_left_white = false;
							$scope.navigate_right_white = false;
							$scope.check_white = false;
						}
					}
				}
			}
		}
		watchQuiz();
	};
	
	var watchQuiz = function() {
		if($scope.tables[$scope.currentBox].model && $scope.allData.data.ResponseQa !== undefined){
			for (var ind = 0; ind < $scope.tables[$scope.currentBox].model.length; ind++){
				var value = $scope.allData.data.ResponseQa[$scope.tables[$scope.currentBox].model[ind]];
				if (value === "" || value === undefined){
					$scope.validateBox = false;
					$scope.navigate_left_white = true;
					$scope.navigate_right_white = true;
					$scope.check_white = true;
					break;
				}
				else{
					$scope.validateBox = true;
					$scope.navigate_left_white = false;
					$scope.navigate_right_white = false;
					$scope.check_white = false;
				}
					
			}
		}						
	};
}]);
