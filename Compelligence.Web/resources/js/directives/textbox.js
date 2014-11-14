// Directives for textbox input types to be used.
app.directive('textbox', function($compile,$log) {
	return {
		restrict: 'AE',
		replace: 'true',
		template: '',
		link: function(scope, iElement, iAttrs){
		//To previsualize accounts and attachments data page according to model it comes from row[model] and
		//to previsualize review and finalize data comes model tables.rows[model]
			if(scope.table.type=='HEADERLISTEDIT' || scope.table.type=='HEADERATTACHMENTLISTEDIT'){
				iElement[0].innerHTML = '<input type="text" ng-model="row[model]" />';	
			}else {
				iElement[0].innerHTML = '<input tabindex="{{table.tabIndex[' + index + ']}}" type="text" ng-model="table.rows[model]" />';
			}
			$compile(iElement.contents())(scope);
		}
	};
});