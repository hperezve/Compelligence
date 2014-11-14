

var ExamFunction = function () {
  this.name = "default name";
  this.$get = function () {
    this.name = "new name";
    return "Hello from ExamFunction.$get().this.name = " + this.name;
  };
  return "Hello from ExamFunction.this.name = " + this.name;
};

//return actual function
app.service( 'myService', ExamFunction );

//return function's return value
app.factory( 'myFactory', ExamFunction );

//returns the output of functions $get function
app.provider( 'myProvider', ExamFunction );

//function ExampleController ( $scope, myService, myFactory, myProvider) {
//  $scope.serviceOutput = "myService = " + myService;
//  $scope.factoryOutput = "myFactory = " + myFactory;
//  $scope.providerOutput = "myProvider = " + myProvider;
//}