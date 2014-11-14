var focusFirstFormField = function(formId) {
    var fields = $(formId + ' input[type=text]:visible:enabled:first').focus();
};
var resetFormFields = function(formId) {
    $(formId).resetForm();
};
var enableFormFields = function(formId) {
    var formFields = $(formId + ' :input');
    formFields.each(function() {
        $(this).removeAttr("disabled");
    });
};
var disableFormFields = function(formId) {
    var formFields = $(formId + ' :input');
    formFields.each(function() {
        $(this).prop("disabled", "disabled");
    });
};