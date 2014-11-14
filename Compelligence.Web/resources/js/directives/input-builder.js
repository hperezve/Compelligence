//================= Directive to define which type of input needs to be used on the Review Finalize page. ========================//
app.directive('inputbuilder', function($compile, $log) {

	var getType = function(inputType, index) {
		var type = '';

		switch (inputType) {
			case '1': case '10': case '11': case '15': case '16': case '17': case '18':
				//create this pageType for know if the page has more objects so need own directive for display the data 
				type = '<textbox></textbox>';
				break;
			case '2':
				type = '<checkbox></checkbox>';
				break;
			case '3':
				type = '<radiobutton></radiobutton>';
				break;
			case '4':
				type = '<inputtextarea></inputtextarea>';
				break;
			case '5':
				type = '<hidden></hidden>';
				break;
			case '6':
				type = '<password></password>';
				break;
			case '7':
				type = '<imagen></imagen>';
				break;
			case '8':
				type = '<file></file>';
				break;
			case '9':
				type = '<number></number>';
				break;
			case '12': case '13': case '14': 
				type = '<inputdate></inputdate>';
				break;
			case '19': case '22': case'23':
				type = '<inputbutton></inputbutton>';
				break;
			case '20':
				type = '<range></range>';
				break;
			case '21':
				type = '<search></search>';
				break;
			case '24':
				type = '<checkboxtextbox></checkboxtextbox>';
				break;
			case '25':
				type = '<inputimagebutton></inputimagebutton>';
				break;
			case '26':
				type = '<divtext></divtext>';
				break;
			case '27':
				type = '<divtextlabel></divtextlabel>';
				break;
			case '30':
				type = '<doctextdiv></doctextdiv>';
				break;
			case '60':
				type = '<dropdown></dropdown>';
				break;
			case '120':
				type = '<shiplinktextbox></shiplinktextbox>';
				break;
			case '121':
				type = '<shiptextbox></shiptextbox>';
				break;
			case '130':
				type = '<billlinktextbox></billlinktextbox>';
				break;
			case '131':
				type = '<billtextbox></billtextbox>';
				break;
			case '140':
				type = '<vendorlinktextbox></vendorlinktextbox>';
				break;
			case '150':
				type = '<questionlinktextbox></questionlinktextbox>';
				break;
		}
		
		return type;
	};

	var linker = function(scope, element, attrs) {
		//I get the length of model and then I compare the actually model with every model of columns I get
		//then if is true get the input type for this column.
		if(scope.table.model.length > 0) {
			for (index = 0; index < scope.table.showColumns.length; index++ ){
				var pass = false;
				if(scope.button !== undefined) {
					if(scope.button.labelText == scope.table.showColumns[index].labelText)
						pass = true;
				} else {
					if(scope.model == scope.table.showColumns[index].model) {
						pass = true;
					}
				}
				if(pass) {
					$log.debug(scope);
					element.html(getType(scope.table.showColumns[index].type, index));
					$compile(element.contents())(scope);
				}
			};
		}
	};
	
	return {
		restrict: 'AE',
		replace: 'true',
		link: linker
	};
});
