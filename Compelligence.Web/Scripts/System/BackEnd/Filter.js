
var getIdValue = function(scope, entity) {
    var componentId = scope + checkNullString(entity);
    var formId = '#' + componentId + 'EditForm';
    var elmId = $(formId + ' input[type=hidden][name=Id]');
    if (elmId.length > 0) {
        return elmId.get(0).value;
    } else {
        return null;
    }
};

/*JIQ*/
function getIdFrom(componentId) {
    var formId = '#' + componentId + 'EditForm';
    return $(formId + ' input[type=hidden][name=Id]').get(0).value;
};

function getIdFromDiscussion(componentId, DetailFilter) {

    var formId = '#' + componentId + 'EditForm';
    var detailFilterValue = '';
    var parameter = "ForumResponse.EntityId";
    var elmId = $(formId + ' input[type=hidden][name=Id]');
    if (elmId.length > 0) {
        return elmId.get(0).value;
    } else {
        if (DetailFilter != null && DetailFilter != '') {
            var filters = DetailFilter.split(':');
            jQuery.each(filters, function(i, val) {
            var filterComponents = val.split('_');
            if (filterComponents.length == 3) {                    
                    if (parameter == filterComponents[0]) {                        
                        detailFilterValue = filterComponents[2];
                    }
                }
            });
        }
    }
    return detailFilterValue;
}