app.directive('graphchart',function() {
	var datos = [{"label":"REQ IN PROGRESS", "value":16,"code":"1000"},
		         {"label":"REQ APPROVING", "value":18,"code":"1030"},
		         {"label":"REQ APPROVED", "value":20,"code":"1035"},
		         {"label":"PO IN PROGRESS", "value":12,"code":"3000"},
		         {"label":"PO AWARDED", "value":15,"code":"3030"},
					{"label":"REQ APPROVED1", "value":20,"code":"1035"},
					{"label":"PO IN PROGRESS2", "value":12,"code":"3000"},
					{"label":"REQ APPROVED4", "value":20,"code":"1035"},
					{"label":"PO IN PROGRESS6", "value":12,"code":"3000"},
					{"label":"PO AWARDED3", "value":15,"code":"3030"},
		         {"label":"RCV RECEIVED", "value":8,"code":"4010"}];
	return {
		restrict : 'A',
		transclude : true,
		template : '<div class="graph"><div ng-transclude></div><div class="graph"></div></div>',
//		It can incluide before graph a button to update data for future testing
//		template : '<button class="updateGraph">Update</button>',
		link : function(scope, iElement, iAttrs, ctrl) {
			console.log('Datos : '+ datos);
			console.log('Graph Type : '+ iAttrs.graphtype);
			//data = datos;
			data = JSON.parse(iAttrs.data);
			gType = iAttrs.graphtype;
			if(gType === 'PIE'){
				chartNewPieGraph(iElement, data, iAttrs);
			} else if(gType === 'BAR'){
				chartBarGraph(iElement, data, iAttrs);
			} else if(gType === 'gauge'){
				var config = {size: 300,clipWidth: 300,clipHeight: 300,ringWidth: 60,maxValue: 10,transitionMs: 4000,};
				var gaugeGraph = chartGaugeGraph(iElement, data, config);
				gaugeGraph.render();
				gaugeGraph.update(Math.random() * 10);
			}
		}
	};
});

var chartNewPieGraph = function(element, data, opts){

	var svg = d3.select(element[0])
		.append("svg")
		.attr("class", "pieGraph")
		.attr("viewBox", "0 0 960 500")
		.attr("preserveAspectRatio", "xMidYMid")
		.append("g");

	svg.append("g")
		.attr("class", "slices");
	svg.append("g")
		.attr("class", "labels");
	svg.append("g")
		.attr("class", "lines");

	var width = 960,
	    height = 450,
		radius = Math.min(width, height) / 2;

	var pie = d3.layout.pie()
		.sort(null)
		.value(function(d) {
			return d.value;
		});

	var arc = d3.svg.arc()
		.outerRadius(radius * 0.8)
		.innerRadius(radius * 0.4);

	var outerArc = d3.svg.arc()
		.innerRadius(radius * 0.9)
		.outerRadius(radius * 0.9);

	svg.attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

	var key = function(d){ return d.data.label; };

	var color = d3.scale.ordinal()
		.domain(["Lorem ipsum", "dolor sit", "amet", "consectetur", "adipisicing", "elit", "sed", "do", "eiusmod", "tempor", "incididunt"])
		.range(["#98abc5", "#8a89a6", "#7b6888", "#6b486b", "#a05d56", "#d0743c", "#ff8c00"]);

	function updateData(){
		var labels = color.domain();
		return labels.map(function(label){
			return { label: label, value: Math.random() };
		});
	}
	//array2arraGraph this function transform array object result from browse to common array for graph data
	change(array2arrayGraph(data));
//	old way to work with
//	change(updateData());

//	d3.select(".randomize")
//		.on("click", function(){
//			change(updateData());
//		});


	function change(data) {

		/* ------- PIE SLICES -------*/
		var slice = svg.select(".slices").selectAll("path.slice")
			.data(pie(data), key);

		slice.enter()
			.insert("path")
			//.style("fill", function(d) { return color(d.data.label); })
			.attr("class", "slice");

		slice
			.transition().duration(1000)
			.attrTween("d", function(d) {
				this._current = this._current || d;
				var interpolate = d3.interpolate(this._current, d);
				this._current = interpolate(0);
				return function(t) {
					return arc(interpolate(t));
				};
			})

		slice.exit()
			.remove();

		/* ------- TEXT LABELS -------*/

		var text = svg.select(".labels").selectAll("text")
			.data(pie(data), key);

		text.enter()
			.append("text")
			.attr("dy", ".35em")
			.text(function(d) {
				return d.data.label;
			});

		function midAngle(d){
			return d.startAngle + (d.endAngle - d.startAngle)/2;
		}

		text.transition().duration(1000)
			.attrTween("transform", function(d) {
				this._current = this._current || d;
				var interpolate = d3.interpolate(this._current, d);
				this._current = interpolate(0);
				return function(t) {
					var d2 = interpolate(t);
					var pos = outerArc.centroid(d2);
					pos[0] = radius * (midAngle(d2) < Math.PI ? 1 : -1);
					return "translate("+ pos +")";
				};
			})
			.styleTween("text-anchor", function(d){
				this._current = this._current || d;
				var interpolate = d3.interpolate(this._current, d);
				this._current = interpolate(0);
				return function(t) {
					var d2 = interpolate(t);
					return midAngle(d2) < Math.PI ? "start":"end";
				};
			});

		text.exit()
			.remove();

		/* ------- SLICE TO TEXT POLYLINES -------*/

		var polyline = svg.select(".lines").selectAll("polyline")
			.data(pie(data), key);

		polyline.enter()
			.append("polyline");

		polyline.transition().duration(1000)
			.attrTween("points", function(d){
				this._current = this._current || d;
				var interpolate = d3.interpolate(this._current, d);
				this._current = interpolate(0);
				return function(t) {
					var d2 = interpolate(t);
					var pos = outerArc.centroid(d2);
					pos[0] = radius * 0.95 * (midAngle(d2) < Math.PI ? 1 : -1);
					return [arc.centroid(d2), outerArc.centroid(d2), pos];
				};
			});

		polyline.exit()
			.remove();
	};

};
var chartBarGraph = function (element,data,opts) {

	var valueLabelWidth = 40; // space reserved for value labels (right)
	var barHeight = 20; // height of one bar
	var barLabelWidth = 100; // space reserved for bar labels
	var barLabelPadding = 5; // padding between bar and bar labels (left)
	var gridLabelHeight = 18; // space reserved for gridline labels
	var gridChartOffset = 3; // space between start of grid and first bar
	var maxBarWidth = 420; // width of the bar with the max value

	// accessor functions
	//var barLabel = function(d) { return d['Name']; };
	//var barValue = function(d) { return parseFloat(d['Population (mill)']); };
	var barLabel = function(d) {
		//$log.debug('bar label: '+ d['label']);
		//return d['label'];
		//Get First Element from array "Pending"
		return (object2array(d))[0];
	};
	var barValue = function(d) {
		//$log.debug('bar value: '+ d['value']);
		//return d['value'];
		//Get Second Element from array "123"
		return (object2array(d))[1];
	};
	// scales
	var yScale = d3.scale.ordinal().domain(d3.range(0, data.length)).rangeBands([0, data.length * barHeight]);
	var y = function(d, i) { return yScale(i); };
	var yText = function(d, i) { return y(d, i) + yScale.rangeBand() / 2; };
	var x = d3.scale.linear().domain([0, d3.max(data, barValue)]).range([0, maxBarWidth]);
	// svg container element
	var chart = d3.select(element[0]).append("svg")
	  .attr('width', maxBarWidth + barLabelWidth + valueLabelWidth)
	  .attr('height', gridLabelHeight + gridChartOffset + data.length * barHeight);
	// grid line labels
	var gridContainer = chart.append('g')
	  .attr('transform', 'translate(' + barLabelWidth + ',' + gridLabelHeight + ')');
	gridContainer.selectAll("text").data(x.ticks(10)).enter().append("text")
	  .attr("x", x)
	  .attr("dy", -3)
	  .attr("text-anchor", "middle")
	  .text(String);
	// vertical grid lines
	gridContainer.selectAll("line").data(x.ticks(10)).enter().append("line")
	  .attr("x1", x)
	  .attr("x2", x)
	  .attr("y1", 0)
	  .attr("y2", yScale.rangeExtent()[1] + gridChartOffset)
	  .style("stroke", "#ccc");
	// bar labels
	var labelsContainer = chart.append('g')
	  .attr('transform', 'translate(' + (barLabelWidth - barLabelPadding) + ',' + (gridLabelHeight + gridChartOffset) + ')');
	labelsContainer.selectAll('text').data(data).enter().append('text')
	  .attr('y', yText)
	  .attr('stroke', 'none')
	  .attr('fill', 'black')
	  .attr("dy", ".35em") // vertical-align: middle
	  .attr('text-anchor', 'end')
	  .text(barLabel);
	// bars
	var barsContainer = chart.append('g')
	  .attr('transform', 'translate(' + barLabelWidth + ',' + (gridLabelHeight + gridChartOffset) + ')');
	barsContainer.selectAll("rect").data(data).enter().append("rect")
	  .attr('y', y)
	  .attr('height', yScale.rangeBand())
	  .attr('width', function(d) { return x(barValue(d)); })
	  .attr('stroke', 'white')
	  .attr('fill', 'steelblue');
	// bar value labels
	barsContainer.selectAll("text").data(data).enter().append("text")
	  .attr("x", function(d) { return x(barValue(d)); })
	  .attr("y", yText)
	  .attr("dx", 3) // padding-left
	  .attr("dy", ".35em") // vertical-align: middle
	  .attr("text-anchor", "start") // text-align: right
	  .attr("fill", "black")
	  .attr("stroke", "none")
	  .text(function(d) { return d3.round(barValue(d), 2); });
	// start line
	barsContainer.append("line")
	  .attr("y1", -gridChartOffset)
	  .attr("y2", yScale.rangeExtent()[1] + gridChartOffset)
	  .style("stroke", "#000");
};

var chartGaugeGraph = function(container,data, configuration) {
	var that = {};
	var config = {
		size: 200, clipWidth: 200, clipHeight: 110,
		ringInset: 20, ringWidth: 20, pointerWidth: 10,
		pointerTailLength: 5, pointerHeadLengthPercent: 0.9,
		minValue: 0, maxValue: 10,
		minAngle: -90, maxAngle: 90,
		transitionMs: 750, majorTicks: 5,
		labelFormat: d3.format(',g'),
		labelInset: 10,
		arcColorFn: d3.interpolateHsl(d3.rgb('#e8e2ca'), d3.rgb('#3e6c0a'))
	};
	var range = undefined;
	var r = undefined;
	var pointerHeadLength = undefined;
	var value = 0;

	var svg = undefined;
	var arc = undefined;
	var scale = undefined;
	var ticks = undefined;
	var tickData = undefined;
	var pointer = undefined;

	var donut = d3.layout.pie();

	function deg2rad(deg) {
		return deg * Math.PI / 180;
	}

	function newAngle(d) {
		var ratio = scale(d);
		var newAngle = config.minAngle + (ratio * range);
		return newAngle;
	}

	function configure(configuration) {
		var prop = undefined;
		for ( prop in configuration ) {
			config[prop] = configuration[prop];
		}

		range = config.maxAngle - config.minAngle;
		r = config.size / 2;
		pointerHeadLength = Math.round(r * config.pointerHeadLengthPercent);

		// a linear scale that maps domain values to a percent from 0..1
		scale = d3.scale.linear()
			.range([0,1])
			.domain([config.minValue, config.maxValue]);

		ticks = scale.ticks(config.majorTicks);
		tickData = d3.range(config.majorTicks).map(function() {return 1/config.majorTicks;});
		//draw Arc
		arc = d3.svg.arc()
			.innerRadius(r - config.ringWidth - config.ringInset)
			.outerRadius(r - config.ringInset)
			.startAngle(function(d, i) {
				var ratio = d * i;
				return deg2rad(config.minAngle + (ratio * range));
			})
			.endAngle(function(d, i) {
				var ratio = d * (i+1);
				return deg2rad(config.minAngle + (ratio * range));
			});
	}
	that.configure = configure;

	function centerTranslation() {
		return 'translate('+r +','+ r +')';
	}

	function isRendered() {
		return (svg !== undefined);
	}
	that.isRendered = isRendered;

	function render(newValue) {
		//$log.debug('Gauge Render');
		svg = d3.select(container[0]).append('svg:svg').attr('class', 'gauge').attr('width', config.clipWidth).attr('height', config.clipHeight);
		var centerTx = centerTranslation();
		var arcs = svg.append('g').attr('class', 'arc').attr('transform', centerTx);
		arcs.selectAll('path').data(tickData).enter().append('path').attr('fill', function(d, i) {
			return config.arcColorFn(d * i);
		}).attr('d', arc);
		var lg = svg.append('g').attr('class', 'label').attr('transform', centerTx);
		lg.selectAll('text').data(ticks).enter().append('text').attr('transform', function(d) {
			var ratio = scale(d);
			var newAngle = config.minAngle + (ratio * range);
			return 'rotate(' +newAngle +') translate(0,' +(config.labelInset - r) +')';
		}).text(config.labelFormat);
		var lineData = [ [config.pointerWidth / 2, 0], [0, -pointerHeadLength],[-(config.pointerWidth / 2), 0],[0, config.pointerTailLength],[config.pointerWidth / 2, 0] ];
		var pointerLine = d3.svg.line().interpolate('monotone');
		var pg = svg.append('g').data([lineData]).attr('class', 'pointer').attr('transform', centerTx);
		pointer = pg.append('path').attr('d', pointerLine).attr('transform', 'rotate(' +config.minAngle +')');
		update(newValue === undefined ? 0 : newValue);
	}
	that.render = render;

	function update(newValue, newConfiguration) {
		if ( newConfiguration  !== undefined) {
			configure(newConfiguration);
		}
		var ratio = scale(newValue);
		var newAngle = config.minAngle + (ratio * range);
		pointer.transition()
			.duration(config.transitionMs)
			.ease('elastic')
			.attr('transform', 'rotate(' +newAngle +')');
	}
	that.update = update;

	configure(configuration);

	return that;
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
