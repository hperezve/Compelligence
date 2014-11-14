//================= Directive to define which html code is to be used in which portlet. ========================//
//================= E.g If it is a dashboard portlet or review portlet etc. ====================================//

app.directive('viewbuilder', function($compile) {

	var dashboardPortlet =
		'<h1 class="portlet-heading">' +
			'<div class="icon-portlet">' +
				'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>' +
			'</div>' +
			'{{table.title}}' +
			'<div class="icon-gearwheel portletSettings">' +
				'<object data="resources/images/icons/gearwheel.svg" type="image/svg+xml"></object>' +
			'</div>' +
		'</h1>' +
		'<div typebuilder></div>';

	var attachmentsPortlet =
		'<h1 class="portlet-heading">' +
			'<div class="icon-portlet">' +
				'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>' +
			'</div>' +
			'{{table.title}}' +
		'</h1>' +
		'<article class="attachmentsAddPortlet">'+
    		'<h1 class="portlet-heading">'+
    			'<div class="icon-portlet">'+
    				'<object data="resources/images/icons/portfolio_folder.svg" type="image/svg+xml"></object>'+
    			'</div>'+
    		'</h1>'+
    		'<div ng-show="showDragAndDrop" draganddrop></div>'+
    		'<div class="options">'+
    			'<div class="inputArea">'+
    				'<label class="title"><div class="text">Title:</div><input type="text" placeholder="title" ng-model="attachment.docTitle"/></label>'+
    				'<label class="file"><file></file></label>'+
      				'<label class="print"><div class="text">Print:</div><input type="checkbox" ng-model="attachment.docPrint" ng-true-value="Y" ng-false-value="N" /></label>'+
      			'</div>'+
         	'</div>'+
         '</article>';
        //Display attachment temporal according to model
		//+'<div typebuilder></div>';


	var graphPortlet =
		'<h1 class="portlet-heading">' +
			'<div class="icon-portlet">' +
				'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>' +
			'</div>' +
			'{{table.title}}' +
			'<div class="icon-gearwheel portletSettings">' +
				'<object data="resources/images/icons/gearwheel.svg" type="image/svg+xml"></object>' +
			'</div>' +
		'</h1>' +
		'<div graphzone></div>';

	var reviewPortlet =
		'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.eventParams[0]}}>' +
			'<h1 class="portlet-heading">'+
				'<div class="icon-portlet">' +
					'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>' +
				'</div>' +
				'{{table.title}}' +
				'<button ng-show="table.itemList" ng-click="createLine(table); $event.preventDefault()"><div class="icon-button">' +
					'<object data="resources/images/icons/add_white.svg" type="image/svg+xml"></object>' +
				'</div></button>' +
			'</h1>' +
			'<div typebuilder></div>' +
		'</a>';

	var confirmPortlet =
		'<h1 class="portlet-heading">'+
			'<div class="icon-portlet">' +
				'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>' +
			'</div>' +
			'{{table.title}}' +
		'</h1>' +
		'<div typebuilder></div>';

	var questionPortlet =
		'<h1 class="portlet-heading" ng-show="!showAnswer">'+
			'<div class="icon-portlet">' +
				'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>' +
			'</div>' +
			'{{table.title}}' +			
		'</h1>' +
		'<div typebuilder ng-show="!showAnswer"></div>' +
		'<div class="twoColContent" ng-show="showAnswer">' +
			'<label ng-repeat="model in table.model" ng-hide="table.showColumns[$index].type == 5">' +
				'<div class="labelText">{{table.showColumns[$index].labelText}}</div>' +
				'<div class="valueText">' +
					'{{table.rows[model]}}' +
				'</div>' +
			'</label>' +
		'</div>';

	var routingPortlet =
    	'<h1 class="portlet-heading">'+
			'<span class="icon-portfolio_folder"><object data="resources/images/icons/portfolio_folder.svg" type="image/svg+xml"></object></span>' +
			'{{table.title}}' +
		'</h1>' +
		'<h4 class="portlet-heading" ng-show="browseRoutingButton">' +
			'{{table.labelDescription}}' +
		'</h4>' +
		'<div typebuilder></div>';

	var approvalPortlet =
    	'<h1 class="portlet-heading">'+
			'<span class="icon-portlet"><object data="resources/images/icons/portfolio_folder.svg" type="image/svg+xml"></object></span>' +
			'{{table.title}}' +
		'</h1>' +
		'<div typebuilder></div>';

	var approvalNotesPortlet =
    	'<h1 class="portlet-heading">'+
			'<span class="icon-portlet"><object data="resources/images/icons/portfolio_folder.svg" type="image/svg+xml"></object></span>' +
			'{{table.title}}' +
		'</h1>' +
		'<div class="options">' +
			'<div class="inputArea">' +
				'<textarea ng-model="approval.notes" placeholder="This is where the user would input the text for the note."></textarea>' +
			'</div>' +
		'</div>';

	var headingPortlet =
		'<article class="headingInfo">' +
	      	'<div class="{{model}}" ng-repeat="model in table.model">' +
				'{{table.rows[model]}}' +
			'</div>' +
		'</article>' +
		'<article class="headingButtons">' +
			'<div class="btnGroup">' +
				'<div inputbuilder ng-repeat="button in table.showButtons"></div>' +
	        '</div>' +
		'</article>';

	var questionheadingPortlet =
		'<article class="headingInfo">' +
			'<h1 class="portlet-heading">'+
				'{{table.title}}' +
				'<div class="icon-gearwheel portletSettings" ng-show="validateBox">' +
					'<object data="resources/images/icons/check_valid.svg" type="image/svg+xml" width="25" height="25"></object>' +
				'</div>' +
				'<div class="icon-gearwheel portletSettings" ng-show="!validateBox">' +
					'<object data="resources/images/icons/delete_invalid.svg" type="image/svg+xml" width="25" height="25"></object>' +
				'</div>' +
			'</h1>' +
		'</article>' +
		'<article class="headingButtons">' +
			'<div class="btnGroup">' +
				'<div inputbuilder ng-repeat="button in table.showButtons"></div>' +
	        '</div>' +
		'</article>';
	
	var tileButtons =
		'<button ng-repeat="button in table.showButtons"><a href="">{{button.labelText}}</a></button>';

	var twoColPortlet =
		'<div class="labelContainer" ng-repeat="model in table.model" ng-init="boxIndex = (boxGeneralIndex * 10) + $index" ng-init="showPopUp[boxIndex] = false" ng-hide="table.showColumns[$index].type == 5">' + 
			'<label class="{{table.classDisplay[$index]}}">' +
				'<div class="labelText">{{table.showColumns[$index].labelText}}:</div>' +
				'<div inputbuilder class="inputArea">' +
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

	var popupBrowsePortlet =
		'<h1 ng-show="showPopup" class="portlet-heading">'+
			'<div class="icon-portlet">' +
				'<object data="resources/images/icons/{{table.icon}}.svg" type="image/svg+xml"></object>' +
			'</div>' +
			'{{table.title}}' +
		'</h1>' +
		'<div ng-show="showPopup" class="popupBrowse" typebuilder></div>' ;

	var dashboardTilesPortlet =
		'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.rows[table.eventList.click.methodParams]}}>' +
			'<h1 class="portlet-heading">' +
				'{{table.title}}' +
				'<div class="icon-tileRefresh">' +
				'<object data="resources/images/icons/refresh_white.svg" type="image/svg+xml"></object>' +
				'</div>' +
			'</h1>' +
			'<div typebuilder></div>';
		'</a>';

	var dashboardTilesDoublePortlet =
		'<h1 class="portlet-heading">' +
			'{{table.title}}' +
			'<div class="icon-tileRefresh">' +
			'<object data="resources/images/icons/refresh_white.svg" type="image/svg+xml"></object>' +
			'</div>' +
		'</h1>' +
		'<div typebuilder></div>';

	var commentsShowPortlet =
		'<h1 class="portlet-heading">' +
		    '<div class="icon-portlet">' +
		       '<object data="resources/images/icons/portfolio_folder.svg" type="image/svg+xml"></object>' +
		    '</div>' +
		    '{{table.title}}' +
		 '</h1>' +
		 '<div typebuilder></div>';

	var commentsAddPortlet =
		 '<h1 class="portlet-heading"> ' +
		    '<div class="icon-portlet">' +
		       '<object data="resources/images/icons/portfolio_folder.svg" type="image/svg+xml"></object>' +
		    '</div>' +
		    '{{table.title}}' +
		 '</h1>' +
		 '<div class="options">' +
		    '<div class="inputArea">' +
		       '<textarea ng-model="comments.docText.stdText" placeholder="This is where the user would input the text for the comment."></textarea>' +
		    '</div>' +
		    '<label class="print">Print:<input type="checkbox" ng-model="comments.commentPrint" ng-true-value="Y" ng-false-value="NO"/></label>' +
		    '<label class="placement">Placement:' +
		       '<select ng-model="comments.commentPlace" ng-options="commentPlaces.id as commentPlaces.name for commentPlaces in commentPlaceTypes"></select>' +
		    '</label>' +
		 '</div>';

	var totalsPortlet =
		'<ul class="lineItemList">' +
			'<li class="totals" ng-repeat="model in table.model">' +
				'<div class="totalsLine"><div class="labelText">{{table.showColumns[$index].labelText}}:</div>{{table.rows[model]}}<div class="currCode">USD</div></div>' +
			'</li>' +
		'</ul>';

	var logo =
		'<img src="resources/images/p-logo.png" />'  +
		'<h1>{{table.title}}</h1>'  +
		'<article class="helpText">'  +
			'{{table.labelDescription}}'  +
		'</article>';

	var inputs =
		'<label ng-repeat="model in table.model" ng-hide="table.showColumns[$index].type == 5">' +
			'<span>{{table.showColumns[$index].labelText}}:</span>' +
			'<div inputbuilder class="inputArea">' +
			'</div>' +
		'</label>';

	/*var btnGroup =
		'<a href="#/"><button>Back</button></a>' +
		ng-repeat="button in table.showButtons"
		'<a href=#{{table.eventList.click.eventMethod}}/{{table.eventList.click.urlParams}}{{table.rows[table.eventList.click.methodParams]}}><button>Submit</button></a>';
	*/
	var btnGroup =
		'<a ng-repeat="button in table.showButtons" href=#{{button.eventList.click.eventMethod}}/{{button.eventList.click.urlParams}}{{button.eventList.click.methodParams}} ><button>{{button.labelText}}</button></a>';
		//'steef';


	var getTemplate = function(contentType, type) {
		var portlet = '';

		switch(contentType) {
			case 'dashboardPortlet':
				portlet = dashboardPortlet;
				break;
			case 'dashboardTilesPortlet_Double':
				portlet = dashboardTilesDoublePortlet;
				break;
			case 'graphPortlet':
				portlet = graphPortlet;
				break;
			case 'reviewPortlet':
				if (type === "QUESTIONTWOCOL") {
					portlet = questionPortlet;
				} else if (type === "CONFIRM") {
					portlet = confirmPortlet;
				} else {
					portlet = reviewPortlet;
				}
				break;
			case 'reviewPortletFullWidth':
				portlet = reviewPortlet;
				break;
			case 'routingPortlet':
				portlet = routingPortlet;
				break;
			case 'approvalPortlet':
				portlet = approvalPortlet;
				break;
			case 'approvalNotesPortlet':
				portlet = approvalNotesPortlet;
				break;
			case 'headingPortlet':
				if (type === "QUESTION") {
					portlet = questionheadingPortlet;
				} else {
					portlet = headingPortlet;
				}
				
				break;
			case 'tileButtons':
				portlet = tileButtons;
				break;
			case 'twoColPortlet':
				portlet = twoColPortlet;
				break;
			case 'popupBrowsePortlet':
				portlet = popupBrowsePortlet;
				break;
			case 'commentsBrowsePortlet':
				portlet = commentsShowPortlet;
				break;
			case 'dashboardTilesPortlet':
				portlet = dashboardTilesPortlet;
				break;
			case 'totalsPortlet':
				portlet = totalsPortlet;
				break;
			case 'commentsShowPortlet':
				portlet = commentsShowPortlet;
				break;
			case 'commentsAddPortlet':
				portlet = commentsAddPortlet;
				break;
			case 'attachmentsPortlet':
				portlet = attachmentsPortlet;
				break;
			case 'logo':
				portlet = logo;
				break;
			case 'inputs':
				portlet = inputs;
				break;
			case 'btnGroup':
				portlet = btnGroup;
				break;
		}

		return portlet;
	};

	var linker = function(scope, element, attrs) {

		element.html(getTemplate(scope.table.portlet, scope.table.type));
		$compile(element.contents())(scope);
	};

	return {
		restrict: 'AE',
		replace: 'true',
		transclude: 'true',
		link: linker

	};

});
