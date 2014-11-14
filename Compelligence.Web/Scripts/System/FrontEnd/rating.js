//***********************************************************************************************************
function zxcDocS() {
    if (!document.body.scrollTop) { return [document.documentElement.scrollLeft, document.documentElement.scrollTop]; }
    return [document.body.scrollLeft, document.body.scrollTop];
}

function zxcMse(event) {
    if (!event) var event = window.event;
    if (document.all) { return [event.clientX + zxcDocS()[0], event.clientY + zxcDocS()[1]]; }
    return [event.pageX, event.pageY];
}

function zxcPos(zxcobj) {
    zxclft = zxcobj.offsetLeft;
    zxctop = zxcobj.offsetTop;
    while (zxcobj.offsetParent != null) {
        zxcpar = zxcobj.offsetParent;
        zxclft += zxcpar.offsetLeft;
        zxctop += zxcpar.offsetTop;
        zxcobj = zxcpar;
    }
    return [zxclft, zxctop];
}

function mouseMove(event, source) {
    var parentObject = source.parentNode;  //Div Content

    var pos_x = (zxcMse(event)[0] - zxcPos(parentObject)[0]);
    //var pos_y = (zxcMse(event)[1] - zxcPos(parentObject)[1]);


    var DivBar = source.getElementsByTagName('div')[0];
    var DivPer = parentObject.getElementsByTagName('div')[2];


    // if ((pos_x > 0 && pos_x < 85)) {
        if ( (pos_x - 2) >= 0 )
        DivBar.style.width = pos_x-2 + 'px';
        DivPer.innerHTML = Math.round((pos_x-1) / 84 * 100) + '%';
   // }
  //  else  //restore value
  //  {
  //      DivBar.style.width = '0px';
  //      DivPer.innerHTML = '0%';
   // }

}


function mouseOut(event, source) {
    var parentObject = source.parentNode;  //Div Content
    var DivBar = source.getElementsByTagName('div')[0];
    var DivPer = parentObject.getElementsByTagName('div')[2];
    DivBar.style.width = '0px';
    DivPer.innerHTML = '0%';

}

function mouseUpdate(event, source, counter, urlAction) {   //  var event=obj.event;
    //    source.disabled = true;

    source.onmousedown = null;
    source.onmousemove = null;
    source.onmouseout = null;

    var DivAverage = $('#Ra' + source.id).children('.rating-average').children('.rating-foreground');
    var DivAverageValue = $('#Ra' + source.id).children('.rating-percent');

    var DivChanged = $('#Rc' + source.id).children('.rating').children('.rating-foreground');
    var DivChangedValue = $('#Rc' + source.id).children('.rating-percent');

    var DivChangedValueTwo = source.nextSibling.nextSibling;
    var DivAverageValueTwo = source.parentNode.previousSibling.previousSibling.getElementsByTagName('div')[2];
    var DivAverageTwo = source.parentNode.previousSibling.previousSibling.getElementsByTagName('div')[1].childNodes[0];
      
    //var v = parseInt(DivChangedValue.html());
    var v = parseInt(DivChangedValueTwo.innerHTML);

    try {
        //$.get(urlAction + '?ProjectId=' + source.id + '&Rating=' + v);
        $.get(urlAction + '&Rating=' + v);

        var vavg = parseInt(DivAverageValue.html());
        var counter = parseInt(counter);
        var RatingSum = vavg * counter; //Prev. Rating to RatingSum

        counter++;
        var newRating = Math.round((v + RatingSum) / counter);

        DivAverageValue.html(newRating + '%');
        DivAverage.css('width', Math.round(newRating * 84 / 100) + 'px');

        DivAverageValueTwo.innerHTML = newRating + '%';
        DivAverageTwo.style.width = Math.round(newRating * 84 / 100) + 'px';
        
        counter++;
    }
    catch (err) { }
}

