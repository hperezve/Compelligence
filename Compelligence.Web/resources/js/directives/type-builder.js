
//================= Directive to define which template needs to be used for which portlet. ========================//
//=============================== E.g If it needs to be a 2-col or a browse template etc. =========================//

app.directive('typebuilder', function($compile) {

	var browse =
		'<ul class="columnList_{{table.showColumns.length}}">' +
			'<li class="columnRow listHeader hide-on-mobile">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.showColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
			'<li class="columnRow" ng-repeat="row in table.rows.rows|filter:query|limitTo:10" ng-init="rowIndex = $index">' +
				'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.eventParams[rowIndex]}}>' +
					'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model track by $index">' +
						'<div class="detailLabel hide-on-desktop hide-on-tablet">{{table.showColumns[$index].labelText}}:</div>' +
						'{{row[model]}}' +
					'</div>' +
				'</a>' +
			'</li>' +
			//This is data that should not be seen by the users.
			'<li class="columnRow listHeader hide-on-mobile" ng-hide="true">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.hideColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
		'</ul>';

	var browseEdit =
		'<ul class="columnList_{{table.showColumns.length}}">' +
			'<li class="columnRow listHeader hide-on-mobile">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.showColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
			'<li class="columnRow" ng-repeat="row in table.rows.rows|limitTo:10" ng-init="rowIndex = $index">' +
				'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model track by $index">' +
					'<div class="detailLabel hide-on-desktop hide-on-tablet">{{table.showColumns[$index].labelText}}:</div>' +
					'<input type="text" ng-model="table.rows.rows[rowIndex][model]" />' +
				'</div>' +
			'</li>' +
			//This is data that should not be seen by the users.
			'<li class="columnRow listHeader hide-on-mobile" ng-hide="true">' +
			'<a>' +
				'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.hideColumns">' +
					'{{column.labelText}}' +
				'</div>' +
			'</a>' +
		'</li>' +
		'</ul>';

	var browsePopup =
		'<ul class="columnList_{{table.showColumns.length}}">' +
			'<li class="columnRow listHeader hide-on-mobile">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.showColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
			'<li class="columnRow" ng-repeat="row in table.rows.rows | filter:queryBrowse | limitTo:10" ng-click="returnData(table, row)" >' +
				'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model track by $index">' +
					'<div class="detailLabel hide-on-desktop hide-on-tablet">{{table.showColumns[$index].labelText}}:</div>' +
					'{{row[model]}}' +
				'</div>' +
			'</li>' +
			//This is data that should not be seen by the users.
			'<li class="columnRow listHeader hide-on-mobile" ng-hide="true">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.hideColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
		'</ul>';

	var browseDocument =
		'<ul class="columnList_{{table.showColumns.length}}">' +
			'<li class="columnRow listHeader hide-on-mobile">' +
				'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.eventParams[0]}}>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.showColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
			'<li class="columnRow" ng-repeat="row in table.rows|filter:query|limitTo:10" ng-init="rowIndex = $index">' +
				'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.eventParams[rowIndex]}}>' +
					'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model track by $index">' +
						'<div class="detailLabel hide-on-desktop hide-on-tablet">{{table.showColumns[$index].labelText}}:</div>' +
						'{{row[model]}}' +
					'</div>' +
				'</a>' +
				'<div class="column_{{table.model.length+1}} buttons" ng-show="browseAttachmentButton">'+
					'<button ng-click="removeRowOnPage($index, table.rows)">'+
						'<div class="icon-button">'+
							'<object data="resources/images/icons/delete_white.svg" type="image/svg+xml"></object>'+
						'</div>'+
					'</button>'+
				'</div>'+
			'</li>' +
			//This is data that should not be seen by the users.
			'<li class="columnRow listHeader hide-on-mobile" ng-hide="true">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.hideColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
		'</ul>';

	var browseRouting =
		'<ul class="columnList_{{table.showColumns.length + 2}}">' +
			'<li class="columnRow listHeader hide-on-mobile">' +
				'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.eventParams[0]}}>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.showColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
			'<li class="columnRow" ng-repeat="row in rows = table.rows|filter:query|limitTo:10" ng-init="rowIndex = $index">' +
				'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model track by $index">' +
					'<div class="detailLabel hide-on-desktop hide-on-tablet">{{table.showColumns[$index].labelText}}:</div>' +
					'{{row[model]}}' +
				'</div>' +
				'<div class="column_{{table.model.length+1}} buttons" ng-show="browseRoutingButton">' +
					'<button ng-click="move(rowIndex,-1)">' +
						'<div class="icon-approval"><object data="resources/images/icons/navigate_up_white.svg" type="image/svg+xml"></object></div>' +
					'</button>' +
					'<button ng-click="move(rowIndex,1)">' +
	               		'<div class="icon-approval"><object data="resources/images/icons/navigate_down_white.svg" type="image/svg+xml"></object></div>' +
	               	'</button>' +
	            '</div>' +
				'<div class="column_{{table.model.length+2}} delete" ng-show="browseRoutingButton" ng-click="removeRowRouting(rowIndex)">' +
	               '<button>' +
	                  '<div class="icon-approval"><object data="resources/images/icons/delete_white.svg" type="image/svg+xml"></object></div>' +
	               '</button>' +
	            '</div>' +
			'</li>' +
			//This is data that should not be seen by the users.
			'<li class="columnRow listHeader hide-on-mobile" ng-hide="true">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.hideColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
		'</ul>';

	var headerListEdit =
		'<ul ng-hide="displayList(header, table, -1)" class="columnList_{{table.showColumns.length}}">' +
			'<li class="columnRow listHeader hide-on-mobile">' +
				'<a>' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.showColumns">' +
						'{{column.labelText}}' +
					'</div>' +
				'</a>' +
			'</li>' +
			'<li class="columnRow listHeader hide-on-mobile">' +
				'<input type="button" value="Add" ng-click="addRow(table.rows,-1, table)"/>' +
			'</li>' +
			'<li class="columnRow" ng-repeat="row in table.rows|limitTo:10" ng-init="rowIndex = $index">' +
				'<span class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model track by $index">' +
					'<span class="detailLabel hide-on-desktop hide-on-tablet" ng-hide="table.showColumns[$index].type == 5">{{table.showColumns[$index].labelText}}:</span>' +
					'<span inputbuilder ng-model="row[model]" ></span>' +
				'</span>' +
				'<input type="button" value="Remove" ng-click="removeRowOnPage($index, table.rows)"/>' +
			'</li>' +
			//This is data that should not be seen by the users.
			'<li class="columnRow listHeader hide-on-mobile" ng-hide="true">' +
			'<a>' +
				'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.hideColumns">' +
					'{{column.labelText}}' +
				'</div>' +
			'</a>' +
		'</li>' +
		'</ul>';

	var lineListEdit =
		'<ul ng-hide="displayList(table.rows[rowIndex], table, $index)" class="columnList_{{table.showColumns.length}}" ng-repeat="row in table.rows" ng-init="rowIndex = $index">' +
			'<li ng-hide="!$first" class="columnRow listHeader hide-on-mobile">' +
					'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in table.showColumns" >' +
						'{{column.labelText}}' +
					'</div>' +
					'<button class="invisible">' +
						'<div class="icon-button">' +
						'</div>' +
					'</button>' +
			'</li>' +
			'<li class="lineNumber hide-on-mobile">' +
				'<div class="text">' +
					'Line {{$index + 1}}' +
				'</div>' +
				'<div class="radioButtons">' +
					'<label>' +
						'<input type="radio" value="PL" ng-model="checkAccountMethod" ng-change="allocMethodValue(table.rows[rowIndex], checkAccountMethod)" ng-checked="getChecked(\'\PL\'\,table)"/>Percent' +
					'</label>' +
					'<label>' +
						'<input type="radio" value="AL" ng-model="checkAccountMethod" ng-change="allocMethodValue(table.rows[rowIndex], checkAccountMethod)" ng-checked="getChecked(\'\AL\'\,table)"/>Amount' +
					'</label>' +
				'</div>' +
				'<button ng-click="addRow(table.rows[rowIndex], $index, table)">' +
					'<div class="icon-button">' +
						'<object data="resources/images/icons/add_white.svg" type="image/svg+xml"></object>' +
					'</div>' +
				'</button>' +
			'</li>' +
			'<li class="columnRow" ng-repeat="listRow in table.rows[rowIndex] track by $index" ng-init="listRowIndex = $index" >' +
				'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model track by $index">' +
					'<div class="detailLabel hide-on-desktop hide-on-tablet">{{table.showColumns[$index].labelText}}:</div>' +
					'<input type="text" ng-model="listRow[model]" ng-change="calculateTotal(table.rows[rowIndex],model,checkAccountMethod)" ng-focus="getAllocAmountTotal(table.rows[rowIndex])" />' +
				'</div>' +
				'<button ng-click="removeRowOnPage($index, row)">' +
					'<div class="icon-button">' +
						'<object data="resources/images/icons/delete.svg" type="image/svg+xml"></object>' +
					'</div>' +
				'</button>' +
			'</li>' +
		'</ul>';

	var dashboardTile =
		'<h3 class="full">'+
			'<div class="icon-tile">'+
				'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>'+
			'</div>'+
			'<div class="number">{{table.rows.rows[0].count}}</div>'+
		'</h3>'+
		'<div class="description">{{table.labelDescription}}</div>';

	var dashboardTileDouble =
		'<h3 class="full">'+
			// '<div class="icon-tile">'+
			// 	'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>'+
			// '</div>'+
			'<div class="number"><div class="input-group"><input type="text" ng-model="createText" focus="true" press-enter-key="puridiomPath(createText)"/>' +
			'<button ng-click="puridiomPath(createText)"><div class="icon-button"><object data="resources/images/icons/navigate_right_white.svg" type="image/svg+xml"></object></div></button>' +
			'</div>'+
		'</h3>'+
		'<div class="description">{{table.labelDescription}}</div>';

	var twoCol =
		'<div class="twoColContent">' +
			'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.rows[table.eventList.click.methodParams]}}>' +
			'<label ng-repeat="model in table.model track by $index">' +
				'<div class="labelText">' +
					'{{table.showColumns[$index].labelText}}:' +
				'</div>' +
				'<div class="valueText">' +
					'{{table.rows[model]}}' +
				'</div>' +
			'</label>' +
			'</a>' +
		'</div>';

	var twoColInput =
		'<div class="twoColInputsContent" ng-repeat="model in table.model" ng-init="boxIndex = (boxGeneralIndex * 10) + $index" ng-hide="table.showColumns[$index].type == 5">' +
			'<label>' +
				'<div class="labelText">{{table.showColumns[$index].labelText}}:</div>' +
				'<div inputbuilder class="valueText">' +
				'</div>' +
			'</label>' +
			'<ul class="columnList_{{elementColums[boxIndex].length}}" ng-show="table.showBrowseBox && showPopUp[boxIndex]">' +
				'<li class="columnRow listHeader hide-on-mobile">' +
					'<a>' +
						'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in elementColums[boxIndex]">' +
							'{{column.labelText}}' +
						'</div>' +
					'</a>' +
				'</li>' +
				'<li class="columnRow" ng-repeat="row in elementRows[boxIndex] | filter:queryBrowse | limitTo:10" ng-click="returnData(elementMetadata[boxIndex],row,boxIndex)" >' +
					'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in elementModels[boxIndex] track by $index">' +
						'<div class="detailLabel hide-on-desktop hide-on-tablet">{{elementColums[boxIndex][$index].labelText}}:</div>' +
						'{{row[model]}}' +
					'</div>' +
				'</li>' +
				'<li class="columnRow listHeader hide-on-mobile" ng-hide="true">' +
					'<a>' +
						'<div class="column_{{$index + 1}} headerLabel" ng-repeat="column in elementHiddens[boxIndex]">' +
							'{{column.labelText}}' +
						'</div>' +
					'</a>' +
				'</li>' +
			'</ul>';
		'</div>';

	var confirm =
		'<ul class="confirmation">' +
			'<li ng-repeat="message in messageProcess">' +
				'<div class="bulletPoint">' +
					'<div class="icon-confirm"><object data="resources/images/icons/navigate_right_white.svg" type="image/svg+xml"></object></div>' +
				'</div>' +
				'<div class="information">' +
					'{{message}}' +
				'</div>' +
			'</li>' +
		'</ul>';

	var itemList =
		'<ul class="lineItemList">' +
			'<li ng-repeat="row in table.rows|limitTo:10 track by $index">' +
				'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.eventParams[$index]}}>' +
					'<div class="{{model}}" ng-repeat="model in table.model track by $index">' +
					'<div class="detailLabel">{{table.showColumns[$index].labelText}}</div>{{row[model]}}</div>' +
				'</a>' +
			'</li>' +
		'</ul>';

	var comment =
		'<ul class="commentList_{{table.showColumns.length}}">' +
			'<li ng-repeat="row in table.rows" ng-init="rowIndex = $index" ng-hide="table.showColumns[$index].type == 5">' +
				'<div class="column_{{$index + 1}} {{model}}" ng-repeat="model in table.model">' +
					'<inputbuilder></inputbuilder>' +
			    '</div>' +
			'</li>' +
		'</ul>';

	var getTemplate = function(contentType) {
		var template = '';

		switch (contentType) {
			case 'BROWSE':
				template = browse;
				break;
			case 'POPUPBROWSE':
				template = browsePopup;
				break;
			case 'BROWSEEDIT':
				template = browseEdit;
				break;
			case 'BROWSEROUTING':
				template = browseRouting;
				break;
			case 'BROWSEDOCUMENT':
				template = browseDocument;
				break;
			case 'HEADERLISTEDIT':
				template = headerListEdit;
				break;
			case 'LINELISTEDIT':
				template = lineListEdit;
				break;
			case 'TWOCOL':
				template = twoCol;
				break;
			case 'CONFIRM':
				template = confirm;
				break;
			case 'QUESTIONTWOCOL':
				template = twoColInput;
				break;
			case 'DASHTILE':
				template = dashboardTile;
				break;
			case 'DASHTILE_DOUBLE':
				template = dashboardTileDouble;
				break;
			case 'ITEMLIST':
				template = itemList;
				break;
			case 'COMMENT':
				template = comment;
				break;		}
		return template;
	};

	var linker = function(scope, element, attrs) {

		element.html(getTemplate(scope.table.type));
		$compile(element.contents())(scope);
	};

	return {
		restrict: 'AE',
		replace: 'true',
		link: linker
	};
});
