app.directive('pressEnterKey', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if(event.which === 13) {
                scope.safeApply(function (){
                    scope.$eval(attrs.pressEnterKey);
                });
                event.preventDefault();
            }
        });
    };	
});