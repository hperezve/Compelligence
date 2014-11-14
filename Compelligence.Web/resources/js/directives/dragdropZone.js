// Directives for file input types to be used.
app.directive('draganddrop', function($compile,$log,$parse ) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '<div><div id="dropzone">Drop files here to upload</div><div id="dropzone" class="progress" ng-show="percentage">{{percentage}}<div class="bar" style="width: {{percentage}}%;"></div></div></div>',
		link: function(scope, element, attrs){
			 //The on-image-drop event attribute
            //var onImageDrop = $parse(attrs.onImageDrop);
            //When an item is dragged over the document, add .dragOver to the body
            var onDragOver = function (e) {
                e.preventDefault();
            };

            //When the user leaves the window, cancels the drag or drops the item
            var onDragEnd = function (e) {
                e.preventDefault();
            };
            //When a file is dropped on the overlay
            var loadFile = function (files) {
            	scope.attachment.file = files;
                //scope.addAttachment();
            };

            //Dragging begins on the document (shows the overlay)
            //$document.bind("dragover", onDragOver);
            document.addEventListener( 'dragover', onDragOver );

            //Dragging ends on the overlay, which takes the whole window
            element.bind("dragleave", onDragEnd)
                   .bind("drop", function (e) {
                       onDragEnd(e);
                       loadFile(e.dataTransfer.files); /* This is the file */
					});
		}
	};
});