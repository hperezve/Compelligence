var resizeGridId = null;
var resizeOrigHeight = '150';
var resizeContent = function(id) {

    var Item = id;
    $(Item).resizable({
        minWidth: 745,
        maxWidth: 745,
        maxHeight: 240,
        minHeight: 0,
        stop: function(event, ui) {
            var offsetHeight = ui.size.height - ui.originalSize.height;
            var newHeight = $('.contentFormEdit').height() - offsetHeight; // - * - = +
            if (newHeight < 316) {
                newHeight = newHeight + ui.originalSize.height;
                $('.contentFormEdit').css("height", newHeight);
            }
            else {
                //The height not should be more of 600 beacause this is the height by default
                if (newHeight < 600) {
                    $('.contentFormEdit').css("height", newHeight);
                }
                else {
                    $('.contentFormEdit').css("height", 580);
                }
            }
            var newGridHeight = (newHeight <= 328) ? resizeOrigHeight : (newHeight - 150);
            if (resizeGridId != null && resizeGridId != '' && $('#' + resizeGridId).length) {
                $('#' + resizeGridId).jqGrid('setGridHeight', newGridHeight);
            }
        }
    });
    $('.ui-resizable-s').dblclick(function() {
        $(Item).slideToggle("slow");
        $('Item2,.contentFormEdit').css("height", "580");

        if (resizeGridId != null && resizeGridId != '' && $('#' + resizeGridId).length) {
            $('#' + resizeGridId).jqGrid('setGridHeight', '550');
        }
    });
    $('.resizeS').dblclick(function() {
        $(Item).slideToggle("slow");
        $('.contentFormEdit').css("height", "300px");
        if (resizeGridId != null && resizeGridId != '' && $('#' + resizeGridId).length) {
            $('#' + resizeGridId).jqGrid('setGridHeight', '440');
        }
    });
};
