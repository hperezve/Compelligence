app.directive('graphzone',function($compile) {
	var pieGraph =
		'<div piegraph class="graphParent" data="{{table.rows.rows}}"></div>';
	
	var barGraph =
		'<div bargraph class="graphParent" data="{{table.rows.rows}}"></div>';
	
	var getGraph = function(contentType) {
		var template = '';
		switch (contentType) {
			case 'PIE':
				template = pieGraph;
				break;
			case 'BAR':
				template = barGraph;
				break;
		}
		return template;
	};

	var linker = function(scope, element, attrs) {

		element.html(getGraph(scope.table.type));
		$compile(element.contents())(scope);
	};
	
	return {
		restrict : 'AE',
		transclude : true,
		template : '',
		link : linker
	};
});
