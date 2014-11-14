//directive to control every 3 digits call the Browse
app.directive('ngEnter', function() {
    return function(scope, element, attrs) {
        element.bind("keyup", function(event) {
        	var model = element[0].value;
        	var browseId = "";
        	var pageId = "";
        	scope.searchFilter(model);
        	var reloaded = scope.getReloaded();
        	//with this conditional I know when I put 3 digits call the openBrowse
        	//reloaded flag tell us if the page was reloaded previusly, this prevents the browse shows twice in the main page
        	if (model.length === 3 && reloaded === false) {
        		var parameters = attrs.ngEnter.split(",");
        		if(parameters.length == 2) {
        			browseId = parameters[0];
        			pageId = parameters[1];
        			scope.setReloaded(true);
        			scope.openBrowsePage(browseId, pageId, model);        			
        		} 
        		else if (parameters.length == 3){
        			browseId = parameters[0];
        			pageId = parameters[1];  
        			index = parameters[2];
        			scope.setReloaded(true);
        			scope.openBrowseField(browseId, pageId, model, index);
        		} 
        		else {
        			scope.setReloaded(true);
        			browseId = parameters[0];
        			scope.openBrowseId(browseId, model);
        		}
            }
        });
    };
});