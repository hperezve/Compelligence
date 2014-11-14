
var checkNullString = function(value) {
    return (value == null) ? '' : value;
};

var getBooleanValue = function(value) {
    return (value == null) ? false : value;
};

var openPopup = function(url) {
    var popupWindow = window.open(url, 'PopupWindow', 'width=700,height=400,scrollbars=yes');

    if (window.focus) {
        popupWindow.focus();
    }
};

var openPopupCenter = function(url) {
    var w = 800, h = 600;
    if (document.body && document.body.offsetWidth) {
        w = document.body.offsetWidth;
        h = document.body.offsetHeight;
    }
    if (document.compatMode == 'CSS1Compat' &&
    document.documentElement &&
    document.documentElement.offsetWidth) {
        w = document.documentElement.offsetWidth;
        h = document.documentElement.offsetHeight;
    }
    if (window.innerWidth && window.innerHeight) {
        w = window.innerWidth;
        h = window.innerHeight;
    }


    var pw = 700, ph = 300;

    var left = (w - pw) / 2, top = (h - ph) / 2;

    var popupWindow = window.open(url, 'PopupWindow', 'width=' + pw + ',height=' + ph + ',scrollbars=yes' + ',top=' + top + ',left=' + left);
    if (window.focus) {
        popupWindow.focus()
    }
};


var getDropDownSelectedValue = function(component) {
    var selIndex = component.selectedIndex;
    
     return component.options[selIndex].value;
};

	var AlertZipCode = function(idzip) {

	    $(function() {
	    if ($(idzip).val() != '')
	        $(idzip).val($(idzip).prop('value').replace(/[^0-9]/g, ""));

	    if (parseInt($(idzip).val().length) > 5) {
	        jAlert('Invalid Zip Code', 'Information message');
	        $(idzip).val($(idzip).prop('value').replace(/[^'']/g, ""));
	        }
	    });

	}   

//<li class="closed collapsable">
//  <div class="hitarea closed-hitarea collapsable-hitarea"/>
//  <span id="tree_Environment" class="folder">Environment</span>
//  <ul style="display: block;">
//  </ul>
//</li>
function TreeNodeExpand(id) {  //tree_ID
    var TreeNode = $('#' + id);
    TreeNode.parent().removeClass('expandable');
    TreeNode.parent().addClass('collapsable');
    TreeNode.next().css('display', 'block');
}
function TreeNodeCollapse(id) {
    var TreeNode = $('#' + id);
    TreeNode.parent().removeClass('collapse');
    TreeNode.parent().addClass('expandable');
    TreeNode.next().css('display', 'none');
}

var delWhiteSpaceFromExtremes = function(input) {
    var n;
    var temp = '';
    var result = '';
    var start = 'f';
    for (var j = 0; j < input.length; j++) {
        n = input.charAt(j);
        if (n != ' ') {
            start = 't';
        }
        if (start == 't') {
            temp = temp + n;
            if (input.charAt(j) != ' ') {
                result = temp;
            }
        }
    }
    return result;
}
	    //<li class="closed collapsable">
	    //  <div class="hitarea closed-hitarea collapsable-hitarea"/>
	    //  <span id="tree_Environment" class="folder">Environment</span>
	    //  <ul style="display: block;">
	    //  </ul>
	    //</li>
	    function TreeNodeExpand(id) {  //tree_ID
	        var TreeNode = $('#' + id);
	        TreeNode.parent().removeClass('expandable');
	        TreeNode.parent().addClass('collapsable');
	        TreeNode.next().css('display', 'block');
	    }
	    function TreeNodeCollapse(id) {
	        var TreeNode = $('#' + id);
	        TreeNode.parent().removeClass('collapse');
	        TreeNode.parent().addClass('expandable');
	        TreeNode.next().css('display', 'none');
	    }





	    var delWhiteSpaceFromExtremes = function(input) {
	        var n;
	        var temp = '';
	        var result = '';
	        var start = 'f';
	        for (var j = 0; j < input.length; j++) {
	            n = input.charAt(j);
	            if (n != ' ') {
	                start = 't';
	            }
	            if (start == 't') {
	                temp = temp + n;
	                if (input.charAt(j) != ' ') {
	                    result = temp;
	                }
	            }
	        }
	        return result;
	    }


