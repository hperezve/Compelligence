/**
 *
 */
app.controller('ErrorController', function($scope, ContainerData) {

	$scope.detailError = ContainerData.response.responseStatus.message;

});
