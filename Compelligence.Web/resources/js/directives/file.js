// Directives for file input types to be used.
app.directive('file', function($compile,$log) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, element, iAttrs){
			var boxModel  = scope.table.boxModel;
			var boxId  = scope.table.boxId;
			element[0].innerHTML = '<input type="file" file-model="attachment.file" box-model="'+boxModel+'" box-id="'+boxId+'"/>';
			$compile(element.contents())(scope);
		}
	};
});

app.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function(scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;
            
            var modelBox = $parse('attachment.boxModel');
            var modelBoxSetter = modelBox.assign;
            modelBoxSetter(scope, attrs.boxModel);
            
            var modelId = $parse('attachment.boxId');
            var modelIdSetter = modelId.assign;
            modelIdSetter(scope, attrs.boxId);
            element.bind('change', function(){
                scope.$apply(function(){
                    modelSetter(scope, element[0].files);
                });
            });
        }
    };
}]);