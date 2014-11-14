/**
 * 
 */
app.controller('headerController', function($scope, $log, $route, SharedService, $location, ContainerData, PuridiomData) {
	$scope.hideMenu = SharedService.hideMenu;
	$scope.$on('eventHideMenu', function() {
        $scope.hideMenu = SharedService.hideMenu;
    });
	$scope.puridiomPath = function(createText) {		
		if(createText != undefined) {
			if(createText.toLowerCase().indexOf('check') >= 0) {
				pageCall('/question/REQK_QUESTION_PAGE/K');				
			} else if(createText.toLowerCase().indexOf('expense') >= 0) {
				pageCall('/question/REQX_QUESTION_PAGE/X');
			} else if(createText.toLowerCase().indexOf('purchase req') >= 0) {
				pageCall('/createRequisition/requisition/create/REQP_REVIEW/P');
			} else {
				ContainerData.filter = createText;
				pageCall('/CatalogSearch');
			}
		} 
		else {
			pageCall('/CatalogSearch');	
		}
	};	
	
	pageCall = function (path){
		$scope.safeApply(function() {
			$scope.searchText = '';
			//We use reload method instead of location.path() method because, when
			//we are in the same page that we need to call, the location.path() method doens't reload the page
			if ($location.path() === path){
				$route.reload();
			}
			else {				
				$location.path(path);
			}
		});	
	};	
	
	// method that checks the current phase before executing the function, this prevents '$apply already in progress' error happens. 
	$scope.safeApply = function(fn) {
		  var phase = this.$root.$$phase;
		  if(phase == '$apply' || phase == '$digest') {
		    if(fn && (typeof(fn) === 'function')) {
		      fn();
		    }
	    } else {
	    this.$apply(fn);
	  }
	};
});
