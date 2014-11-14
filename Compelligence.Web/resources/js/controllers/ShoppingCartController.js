app.controller('ShoppingCartController', function($scope, $log, $location, ContainerData) {
	//alert(PuridiomData.catalog);
	$scope.cartOpen = true; 
	$scope.cItems = ContainerData.catalog;
	
	$scope.deleteItem = function(row,index){
		//alert(row);
		$scope.cItems.splice(index,1);
		ContainerData.catalog = $scope.cItems;
	};
	
	$scope.updateItem = function(row,index){
		//alert();
		//alert($scope.cItems[index]);
		if($scope.cItems[index].quantity && row.quantity){
			$scope.cItems[index].quantity = row.quantity;
			$scope.cItems[index].total = row.quantity * row.cost;
			ContainerData.catalog = $scope.cItems;
			//getTotal();
		}
	};
	
	$scope.getTotal = function(){
	    var total = 0;
	    for(var i = 0; i < $scope.cItems.length; i++){
	        var product = $scope.cItems[i];
	        total += product.total;
	    }
	    return total;
	};
	
});