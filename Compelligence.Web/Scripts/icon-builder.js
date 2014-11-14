app.directive('iconbuilder', function($compile) {
    var url;

    var linker = function(scope, element, attrs) {
        //	element.html(getTemplate(scope.table.type));			
        var template = '<div ng-include src="\'resources/images/icons/' + attrs.icon + '.svg\'"></div>';        
        element.html(template);
        $compile(element.contents())(scope);
    };
    return {
        restrict: 'E',
        replace: 'true',
        link: linker
    };
});