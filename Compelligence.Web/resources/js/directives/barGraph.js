app.directive('bargraph',function() {
	return {
		restrict : 'A',
		transclude : true,
		template : '',
//		It can incluide before graph a button to update data for future testing
//		template : '<button class="updateGraph">Update</button>',
		link : function(scope, iElement, iAttrs, ctrl) {
			data = JSON.parse(iAttrs.data);
			chartBarGraph(iElement, data, iAttrs);
		}
	};
});

var chartBarGraph = function (element,data,opts) {

	var margin = {top: 20, right: 20, bottom: 30, left: 40},
	    width = 760 - margin.left - margin.right,
	    height = 400 - margin.top - margin.bottom;

	var x = d3.scale.ordinal()
	    .rangeRoundBands([0, width], .1);


	var y = d3.scale.linear()
	    .range([height, 0]);

	var xAxis = d3.svg.axis()
	    .scale(x)
	    .orient("bottom")
	    .ticks(5);

	var yAxis = d3.svg.axis()
	    .scale(y)
	    .orient("left")
	    .ticks(10);

	var svg = d3.select(element[0]).append("svg")
		.attr("class", "barGraph")
		.attr("viewBox", "0 0 750 400")
		.attr("preserveAspectRatio", "xMidYMid")
		.append("g")
		.attr("transform", "translate(" + margin.left + "," + margin.top + ")");

	x.domain(array2arrayGraph(data).map(function(d) { return d.label; }));
	y.domain([0, d3.max(array2arrayGraph(data), function(d) { return parseFloat(d.value); })]);

	  svg.append("g")
	      .attr("class", "x axis")
	      .attr("transform", "translate(0," + height + ")")
	      .call(xAxis);

	  svg.append("g")
	      .attr("class", "y axis")
	      .call(yAxis)
	    .append("text")
	      .attr("transform", "rotate(-90)")
	      .attr("y", 6)
	      .attr("dy", ".71em")
	      .style("text-anchor", "end")
	      .text("Status");



	  svg.selectAll(".bar")
	      .data(array2arrayGraph(data))
	    .enter().append("rect")
	      .attr("class", "bar")
	      .attr("x", function(d) {
	    	  return x(d.label);
	      })
	      .attr("width", x.rangeBand())
	      .attr("y", function(d) {
	    	  return y(parseFloat(d.value));
	      })
	      .attr("height", function(d) { return height - y(parseFloat(d.value)); });

};
/*
 * *Conver a object array request to common object Array for Graphs
 * which one will nevermind name of model for standardize
 *
 */
function array2arrayGraph(_Array){
    var _newArray = new Array();
    for(var key in _Array){
    	var newObject = new Object();
    	var newValues = object2array(_Array[key]);
        newObject.label = newValues[0];
        newObject.value = newValues[1];
        _newArray.push(newObject);
    }
    return _newArray;
}

function object2array(obj){
    var result = [];
    var keys = Object.keys(obj);
    keys.forEach(function(key){
        result.push(obj[key]);
    });
    return result;
}
