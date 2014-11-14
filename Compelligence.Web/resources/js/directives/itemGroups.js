//Directives for items/groups of items

//Catalog search
app.directive('groups', function() {
	console.log("directive");
	return {
		restrict: 'AE',
		replace: 'true',
		//templateUrl: '../../../views/groups.html',
		template:'<section class="searchCategoryTemplate" ng-repeat="table in tables">'+
			// '<div class="overflowArrow greaterThan">'+
			// 	'<span class="icon-navigateRight">'+
			// 		'<object data="resources/images/icons/navigate_right.svg" type="image/svg+xml"></object>'+
			// 	'</span>'+
			// '</div>'+
			// '<div class="overflowArrow lessThan">'+
			// 	'<span class="icon-navigateLeft">'+
			// 		'<object data="resources/images/icons/navigate_left.svg" type="image/svg+xml"></object>'+
			// 	'</span>'+
			// '</div>'+
			'<h1><div class="numberItems">{{table.rows.rowsTotal}}</div>{{table.title}}<button ng-click="viewAllClick(table.boxId)">View All</button></h1>'+

			'<ul class="searchCategoryList">'+
				'<li class="item" ng-repeat="row in table.rows.rows">'+
						'<div class="itemImage">'+
							//'{{row[table.imageData]}}'+
							'<img ng-show="row[table.imageData].length > 0 " ng-src="{{row[table.imageData]}}">'+
							'<div class="noImageAval" ng-show="!row[table.imageData] || row[table.imageData].length <= 0 "></div>'+
						'</div>'+
						'<div class="{{model}}" ng-repeat="model in table.model">'+
							'<label>{{table.showColumns[$index].labelText}}</label>'+
							'<p>{{row[model]}}</p>'+
						'</div>'+
						'<div class="quantity">'+
							'<div class="input-group">'+
								'<input class="form-control" min="1" name="ConsolidatedItem_quantity_0" placeholder="Quantity" type="number"  ng-model="row.quantityBox" value="">'+
								'<button ng-click="additemToShoppingCart(row)">Add</button>'+ //'<button ng-click="fireLeftEvent(table.rows.rows)">move left</button>'+
							'</div>'+
						'</div>'+
//						'<div class="comparing"><p>Compare</p><input type="checkbox"></div>'+
				'</li>'+
			'</ul>'+
		'</section>'
	};
});

//Directive for view all elements

app.directive('group', function() {
	return {
		restrict: 'AE',
		replace: 'true',
		//templateUrl: '../../../views/group.html'
		template:'<div ng-repeat="table in tables" ng-show="table.rows.rowsTotal != undefined"><h1><span class="numberItems">{{table.rows.rowsTotal}} - </span>{{table.title}}</h1>'+
		'<label ng-repeat="i in paginationArray">' +
		      '<button ng-click="goToPage(table.boxId,{{i}})">{{i}}</button>'+
		'</label>'+
				'<ul class="searchCategoryList">'+
					'<li class="item" ng-repeat="row in table.rows.rows">'+
							'<div class="itemImage">'+
								//'{{row[table.imageData]}}'+
								'<img ng-show="row[table.imageData].length > 0 " ng-src="{{row[table.imageData]}}">'+
								'<div class="noImageAval" ng-show="!row[table.imageData] || row[table.imageData].length <= 0 "></div>'+
							'</div>'+
							'<div data-ng-class="{{model}}" ng-repeat="model in table.model">'+
									'<label>{{table.showColumns[$index].labelText}}</label>'+
									'<p>{{row[model]}}</p>'+
							'</div>'+
							'<div class="quantity">'+
								'<div class="input-group">'+
									'<input class="form-control" min="1" name="ConsolidatedItem_quantity_0" placeholder="Quantity" type="number"  ng-model="row.quantityBox" value="">'+
									'<button ng-click="additemToShoppingCart(row)">Add</button>'+ //'<button ng-click="fireLeftEvent(table.rows.rows)">move left</button>'+
								'</div>'+
							'</div>'+
//							'<div class="comparing"><p>Compare</p><input type="checkbox"></div>'+
					'</li>'+

				'</ul>'+
			'</div>'
	};
});
