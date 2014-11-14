//directive to control every 3 digits call the Browse
app.directive('onlyDigits', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attr, ctrl) {
          function inputValue(val) {
            if (val) {
              var digits = val.replace(/[^0-9.]/g, '');

              if (digits !== val) {
                ctrl.$setViewValue(digits);
                ctrl.$render();
              }
              return parseFloat(digits,10.0000);
            }
            return undefined;
          }            
          ctrl.$parsers.push(inputValue);
        }
      };
})