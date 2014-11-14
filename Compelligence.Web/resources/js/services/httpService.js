//Service for implements all method for the HTTP
app.service("dataService", function($log, $http, MyService) {
	this.getData = function(callbackFunc) {
		$http({
			method: 'POST',
			url: 'http://localhost:8080/puridiom5/procure',
			data: MyService.data.params,
//			headers: {'Content-Type': 'application/x-www-form-urlencoded'}
		}).success(function(data){
			callbackFunc(data);
	    }).error(function(){
	        alert("error");
	    });
	};
});

