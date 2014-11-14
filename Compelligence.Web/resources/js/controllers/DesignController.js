/**
 *
 */
app.controller('DesignController', function($scope) {
	//$scope.showPlease = 'false';

	$scope.selectedIndex1 = 'false';
	$scope.selectedIndex2 = 'false';
	$scope.selectedIndex3 = 'false';
	$scope.selectedIndex4 = 'false';
	$scope.openListPortlet = false;
	console.log($scope.openListPortlet);

	$scope.truePlease = 'true';
	$scope.falsePlease = 'false';

	$scope.showAccounts = 'true';
	$scope.showComments = 'false';
	$scope.showAttachments = 'false';

	$scope.filtersOpen = 'false';

//	<------ Below is the beginning test for the swipe command on the Tile page (dashboardTiles_2.html)-------
//   ------ (To view in browser use '/SwipeTest' for the route in the url)------->

	$scope.array = ['1','2','3'];
//	The currentIndex is a variable that represents which tile div the user is viewing (default to 1)
	$scope.currentIndex = 1;
//	document.getElementsByClassName('portletScreenTiles')[0].style.overflow="hidden";

// This prevSlide function works when the user swipes right. It decreases the currentIndex,
// making sure the currentIndex does not past the first index (1), but instead goes back to the last one (3).
// It also takes in a parameter which is supposedly the number of which tile/div the user was viewing before they swiped.
	$scope.prevSlide = function (index) {
	    $scope.direction = 'left';
	    $scope.currentIndex = ((index - 1) < 1) ? 3 : --$scope.currentIndex;

//==================================================================================================
//===== Another way to do the tile switching without ng-switch (all that is in comments below)======
//==================================================================================================

//	    var currentTile = document.getElementById('tiles' + index);
//	    currentTile.style.display='none';
//
//	    document.getElementById('tiles' + (currentIndex)).style.display="flex";
	};

// This nextSlide function works when the user swipes left and increases the currentIndex,
// making sure the currentIndex does not past the last index (in this case, 3), but instead goes back to the first one (1).
// It also takes in a parameter which is supposedly the number of which tile/div the user was viewing before they swiped.
	$scope.nextSlide = function (index) {
	    $scope.direction = 'right';
	    $scope.currentIndex = ((index + 1) > 3) ? 1 : ++$scope.currentIndex;

//	    var currentTile = document.getElementById('tiles' + index);
//	    currentTile.style.display='none';
//
//	    document.getElementById('tiles' + (currentIndex)).style.display="flex";
	};
});
