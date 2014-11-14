
//================= Directive to define which template needs to be used for which portlet. ========================//
//=============================== E.g If it needs to be a 2-col or a browse template etc. =========================//

app.directive('iconbuilder', function($compile) {

	var key =
		'<svg id="svgID" class="primaryIconColor" xmlns="http://www.w3.org/2000/svg" xml:space="preserve" width="1024px" height="1024px" shape-rendering="geometricPrecision" text-rendering="geometricPrecision" image-rendering="optimizeQuality" fill-rule="evenodd" clip-rule="evenodd" viewBox="0 0 10240 10240" xmlns:xlink="http://www.w3.org/1999/xlink">' +
		'<title>key icon</title>'+
		'<desc>key icon from the IconExperience.com O-Collection. Copyright by INCORS GmbH (www.incors.com).</desc>' +
		'<path id="curve0"  d="M5064 1555c875,-875 2394,-773 3394,226 1000,1000 1101,2519 226,3394 -738,738 -1935,781 -2892,176l-3218 3218c-125,125 -328,125 -453,0l-453 -453c-125,-125 -125,-328 0,-453l3218 -3218c-605,-957 -562,-2154 176,-2892zm-2263 7241l1358 -1358 1131 1131 -453 453 -453 -453 -453 453 453 453 -453 453 -1131 -1131zm3168 -6336c375,-375 1084,-274 1584,226 500,500 601,1209 226,1584 -375,375 -1084,274 -1584,-226 -500,-500 -601,-1209 -226,-1584z"/>' +
	    '</svg>';

	var about_warning =
		'<svg xmlns="http://www.w3.org/2000/svg" xml:space="preserve" width="1024px" height="1024px" shape-rendering="geometricPrecision" text-rendering="geometricPrecision" image-rendering="optimizeQuality" fill-rule="evenodd" clip-rule="evenodd" viewBox="0 0 10240 10240" xmlns:xlink="http://www.w3.org/1999/xlink">'+
	'<title>about icon</title>'+
	'<desc>about icon from the IconExperience.com O-Collection. Copyright by INCORS GmbH (www.incors.com).</desc>'+
	'<path id="curve1" fill="#897700" d="M4700 6720l853 0c117,0 213,96 213,213l0 853c0,117 -96,213 -213,213l-853 0c-117,0 -213,-96 -213,-213l0 -853c0,-117 96,-213 213,-213zm-220 -4480l1280 0c88,0 160,72 160,160l0 1059c0,272 -20,504 -68,773l-308 1736c-14,77 -79,132 -158,132l-532 0c-78,0 -144,-55 -158,-132l-308 -1736c-48,-269 -68,-501 -68,-773l0 -1059c0,-88 72,-160 160,-160z"/>'+
	'<path id="curve0" fill="#897700" d="M5120 640c2474,0 4480,2006 4480,4480 0,2474 -2006,4480 -4480,4480 -2474,0 -4480,-2006 -4480,-4480 0,-2474 2006,-4480 4480,-4480zm0 640c2121,0 3840,1719 3840,3840 0,2121 -1719,3840 -3840,3840 -2121,0 -3840,-1719 -3840,-3840 0,-2121 1719,-3840 3840,-3840z"/>'+
	'</svg>';

	var getTemplate = function(iconType) {
		var template = '';

		switch (iconType) {
			case 'key':
				template = key;
				break;
			case 'about_warning':
				template = about_warning;
				break;		}
		return template;
	};

	var linker = function(scope, element, attrs) {

		element.html(getTemplate(scope.table.type));
		$compile(element.contents())(scope);
	};

	return {
		restrict: 'AE',
		replace: 'true',
		link: linker
	};
});
