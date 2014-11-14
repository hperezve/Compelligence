
var getCascadeObjects = function(urlAction, currentComponent, targetComponent, loaderComponent, otherChildComponents, urlParameters) {
    var selectedValue = $(currentComponent + ' > option:selected').prop('value');
    $(targetComponent).empty();

    // Iterate the child components for current component, clean these
    if (otherChildComponents != null) {
        for (var i = 0; i < otherChildComponents.length; i++) {
            $(otherChildComponents[i]).empty();
        }
    }

    // Build the url parameters for current component 
    var queryString = '';

    if (urlParameters != null) {
        for (var i = 0; i < urlParameters.length; i++) {
            if (i == 0) {
                queryString += '?';
            } else {
                queryString += '&';
            }

            queryString += ($(urlParameters[i]).prop('name') + '=' + $(urlParameters[i]).val());
        }
    }

    if ($('#EnvironmentProductProductCriteriaEditFormIndustryStandard')) {
        $('#EnvironmentProductProductCriteriaEditFormIndustryStandard').prop("value", "");
    }
    // If selected value in Parent component is valid, so do an Ajax request and fill current component
    //
    if (selectedValue != '') {

        $(loaderComponent).css('display', 'inline');

        $.ajax({
            type: "POST",
            url: urlAction + '/' + selectedValue + queryString,
            dataType: "json",
            success: function(data) {
                $.each(data, function() {
                    $(targetComponent).append($('<option></option>').val(this['Value']).html(this['Text']));
                });
                $(loaderComponent).css('display', 'none');
            }
        });
    }
};
                     
var getContentData = function(urlAction, parameter, target) {
    $.get(urlAction + '/' + parameter, function(data) { $(target).val(data); });
};

            

            