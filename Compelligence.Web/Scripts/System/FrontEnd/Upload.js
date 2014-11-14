var count = 0;
function generate() {
    count++;
    var upload = "";
    upload += "<div id='uploadfilebox" + count + "'>";
    upload += "<input type='file'id='uploadfile" + count + "' name='uploadfile' style='display: none'/>";
    upload += "<input type='text'id='TxtShowValue" + count + "' name='TxtShowValue' style='width: 295px; margin-right: 6px;'/>";
    upload += "<input type='button' id='BtnBrowse" + count + "' class='shortButton' value='Browse' onclick='SetOnclick(" + count + ");'/>";
    upload += "<input type='button' class='shortButton' value='Remove' onclick='removeUpload(" + count + ");'/><div/>";

    return upload;
 }

function addUploads() {
    $('#uploadfilebox').append(generate());
    };

    function removeUpload(nro) {
    $('#uploadfilebox' + nro).remove();
    };

    function updateText(nro) {
        UpdateUploadTxt();
    }
function addUploadFields(id) {
    if (!id) {
        id = 'uploadfilebox';
    }
    var el = document.getElementById(id);
    var span = document.getElementById(id).getElementsByTagName('span')[0];

    if ('none' == span.style.display) {
        // Show the file upload box
        span.style.display = 'inline';
        // Switch the buttons
        document.getElementById(id + '_attachafile').style.display = 'none';
        document.getElementById(id + '_attachanotherfile').style.display = 'inline';

    } else {
        // Copy the first file upload box and clear it's value
        var newBox = span.cloneNode(true);
        newBox.getElementsByTagName('input')[0].value = '';
        el.appendChild(newBox);
    }
}

function removeUploadField(element, id) {
    if (!id) {
        id = 'uploadfilebox';
    }
    var el = document.getElementById(id);
    var span = el.getElementsByTagName('span');
    if (1 == span.length) {
        // Clear and hide the box
        span[0].style.display = 'none';
        span[0].getElementsByTagName('input')[0].value = '';
        // Switch the buttons
        document.getElementById(id + '_attachafile').style.display = 'inline';
        document.getElementById(id + '_attachanotherfile').style.display = 'none';
    } else {
        el.removeChild(element.parentNode);
    }
}


/*JIQ*/
/*
*Send multipart form for attach file into iframe, using clone objects
*/
function SendMultiPartForm(form, iframeName, fncallback) {
    if (form.Response.value.length == 0) {
        $(form).parent().prepend(getHtmlError('Name is required'));
        return false;
    }
    var myForm = $(form).clone();
    //var myFormEdit = '#<%= formId %>';
    var myFormEdit = '#' + form.id;
    $("#" + iframeName).contents().find("body").append(myForm);
    var iframeForm = $("#" + iframeName).contents().find("body").children(myFormEdit);
    iframeForm.find("textarea").val(form.Response.value);
    iframeForm.onsubmit = null; //disable original onsubmit
    iframeForm.submit();
    //alert(fncallback);
    if (fncallback)
        eval(fncallback);

    return false;
}

var SetOnclick = function(nro) {
    UpdateUploadTxt();
    var buttonTypeFile = $("#uploadfile" + nro);
    buttonTypeFile.click();
};

var UpdateUploadTxt = function() {
    $("input:file").change(function(e) {
        var m = $(this);
        var id = $(this).prop("id");
        $("#TxtShowValue" + id.substring(10)).prop("value", m.val());
    });
}
var SetOnclickFile = function(inputSet) {
    //var buttonTypeFile = $("#" + inputSet);
    //buttonTypeFile[0].click();
    $("#" + inputSet).click();
    //$('#' + inputSet).trigger('click');
};

var SetValueFile = function(inputFile, txtToFill) {
    var test = $("#" + inputFile);
    if (test[0].value != '') {
        $("#" + txtToFill).prop("value", test[0].value);
        $("#" + inputFile).change(function(e) {
            var m = $(this);
            $("#" + txtToFill).prop("value", m.val());
        });
    }
    

};

var RemoveFile = function(divToRemove) {
    $('#' + divToRemove).remove();
    var divsline = $('#uploadfileuploadbox').children('div').prop('class', 'lineDialog');
    if (divsline != null && divsline != undefined) {
        if (divsline.length == 0) {
            $('#btnaddnewfile').prop("value", "Attach a file (max. 2 MB)");
        }
    } else {
        $('#btnaddnewfile').prop("value", "Attach a file (max. 2 MB)");
    }
};

var AddNewVersionInputFile = function() {
    var id = 'uploadfileuploadbox';
    var el = document.getElementById(id);
    var number = 1;
    var divLines = el.childNodes;

    if (divLines != null && divLines != undefined && divLines.length > 0) {
        var lastItem = divLines.length - 1;
        var idOfLastDiv = divLines[lastItem];
        if (idOfLastDiv != null) {
            var idOfDiv = idOfLastDiv.id;
            var toRemove = 'divlinefile';
            var indexToCheck = idOfDiv.indexOf(toRemove);
            if (indexToCheck == 0) {
                var lastNumberStr = idOfDiv.replace(toRemove, '');
                if (lastNumberStr != null && lastNumberStr != undefined && lastNumberStr != '') {
                    var lastNumber = parseInt(lastNumberStr);
                    number = number + lastNumber;
                }
            }
        }
    }
    // DIV LINE
    var divLine = document.createElement("div");
    divLine.setAttribute('class', 'lineDialog');
    divLine.setAttribute('className', 'lineDialog');
    divLine.id = 'divlinefile' + number;
    // DIV FIELD (TEXTBOX)
    var divField1 = document.createElement("div");
    divField1.setAttribute('class', 'fieldDialog');
    divField1.setAttribute('className', 'fieldDialog');
    // DIV FIELD (BROWSE BUTTON)
    var divField2 = document.createElement("div");
    divField2.setAttribute('class', 'fieldDialog');
    divField2.setAttribute('className', 'fieldDialog');
    // DIV FIELD (REMOVE BUTTON)
    var divField3 = document.createElement("div");
    divField3.setAttribute('class', 'fieldDialog');
    divField3.setAttribute('className', 'fieldDialog');

    // TO FIRST DIV FIELD
    var newText = document.createElement('input');
    newText.type = 'text';
    newText.id = 'txtuploadfile' + number;
    newText.name = 'txtuploadfile';
    //newText.setAttribute('style', 'width:240px;');
    newText.setAttribute('class', 'textDialogUpload');
    newText.setAttribute('className', 'textDialogUpload');
    divField1.appendChild(newText);
    // TO SECOND DIV FIELD
    var divContentField = document.createElement("div");
    divContentField.setAttribute('class', 'divButtonDialog ');
    divContentField.setAttribute('className', 'divButtonDialog');
    divContentField.id = 'mybutton' + number;
    divContentField.onclick = function() { $('#uploadfile' + number).trigger('click'); };


    var newInputFile = document.createElement('input');
    newInputFile.setAttribute('class', 'fileDialogUpload ');
    newInputFile.setAttribute('className', 'fileDialogUpload');
    newInputFile.type = 'file';
    newInputFile.id = 'uploadfile' + number;
    newInputFile.name = 'upload';
    newInputFile.onchange = function() { $("#txtuploadfile" + number).prop("value", newInputFile.value); };

    var newLabel = document.createElement("label");
    newLabel.setAttribute('class', 'labelButtonDialog ');
    newLabel.setAttribute('className', 'labelButtonDialog ');

    var newSpan = document.createElement("span");
    newSpan.innerHTML = "Browse";
    //newSpan.setAttribute('style', 'margin-top: 1px;');
    newLabel.appendChild(newSpan);

    divContentField.appendChild(newLabel);
    divContentField.appendChild(newInputFile);
    divField2.appendChild(divContentField);

    // TO THIRD DIV FIELD
    var removeLabel = document.createElement("label");
    removeLabel.setAttribute('class', 'labelButtonDialog ');
    removeLabel.setAttribute('className', 'labelButtonDialog ');

    var removeSpan = document.createElement("span");
    //removeSpan.setAttribute('style', 'margin-top: 1px;');
    removeLabel.innerHTML = "Remove";

    newLabel.appendChild(removeSpan);

    var divButtonReset = document.createElement("div");
    divButtonReset.setAttribute('class', 'divButtonDialog ');
    divButtonReset.setAttribute('className', 'divButtonDialog');
    //divButtonReset.setAttribute('style', 'padding-right: 0px;');
    divButtonReset.id = 'mybuttonreset' + number;
    divButtonReset.onclick = function() { RemoveFile('divlinefile' + number); };

    divButtonReset.appendChild(removeLabel);
    divField3.appendChild(divButtonReset);

    divLine.appendChild(divField1);
    divLine.appendChild(divField2);
    divLine.appendChild(divField3);

    el.appendChild(divLine);
};
var AddNewInputFile = function() {
    var id = 'uploadfileuploadbox';
    var el = document.getElementById(id);

    var number = 1;
    var inputFile = el.childNodes;
    if (inputFile != null && inputFile != undefined && inputFile.length > 0) {
        for (var i = 0; i < inputFile.length; i++) {
            tempInput = inputFile[i];
            if ((tempInput.nodeName == 'input' || tempInput.nodeName == 'INPUT') && (tempInput.type == 'file' || tempInput.type == 'FILE')) {
                number++;
            }
        }
    }
    var divLine = document.createElement("div");
    divLine.setAttribute('class', 'lineDialog');
    divLine.setAttribute('className', 'lineDialog');
    divLine.id = 'divlinefile' + number;

    var divField1 = document.createElement("div");
    divField1.setAttribute('class', 'fieldDialog');
    divField1.setAttribute('className', 'fieldDialog');

    var divField2 = document.createElement("div");
    divField2.setAttribute('class', 'fieldDialog');
    divField2.setAttribute('className', 'fieldDialog');

    var divField3 = document.createElement("div");
    divField3.setAttribute('class', 'fieldDialog');
    divField3.setAttribute('className', 'fieldDialog');

    var newInputFile = document.createElement('input');
    newInputFile.type = 'file';
    newInputFile.id = 'uploadfile' + number;
    newInputFile.name = 'uploadfile';
    newInputFile.setAttribute('style', 'display:none;');
    newInputFile.setAttribute('visible', 'false');
    newInputFile.setAttribute('visibility', 'hidden');
    newInputFile.setAttribute('class', 'fileUpload');
    newInputFile.setAttribute('className', 'fileUpload');
    newInputFile.onchange = function() { $("#txtuploadfile" + number).prop("value", newInputFile.value); };

    var newText = document.createElement('input');
    newText.type = 'text';
    newText.id = 'txtuploadfile' + number;
    newText.name = 'txtuploadfile';
    newText.setAttribute('class', 'textDialogUpload');
    newText.setAttribute('className', 'textDialogUpload');

    divField1.appendChild(newText);

    var newBtn = document.createElement('input');
    newBtn.type = 'button';
    newBtn.id = 'btnFile' + number;
    newBtn.name = 'btnFile';
    newBtn.value = 'Browse';
    newBtn.setAttribute('class', 'button');
    newBtn.setAttribute('className', 'button');
    newBtn.setAttribute('onclick', "javascript:SetOnclickFile('uploadfile" + number + "')");
    //For IE
    newBtn.onclick = function() { SetOnclickFile('uploadfile' + number); };
    newBtn.setAttribute('onfocus', "javascript:SetValueFile('uploadfile" + number + "','txtuploadfile" + number + "')");
    //For IE
    newBtn.onfocus = function() { SetValueFile('uploadfile' + number, 'txtuploadfile' + number); };

    divField2.appendChild(newBtn);

    var newBtnR = document.createElement('input');
    newBtnR.type = 'button';
    newBtnR.id = 'btnRemove' + number;
    newBtnR.name = 'btnRemove';
    newBtnR.value = 'Remove';
    newBtnR.setAttribute('class', 'button');
    newBtnR.setAttribute('className', 'button');
    newBtnR.setAttribute('onclick', "javascript:RemoveFile('divlinefile" + number + "')");
    //For IE
    newBtnR.onclick = function() { RemoveFile('divlinefile' + number); };
    divField3.appendChild(newBtnR);

    var br = document.createElement('br');

    divLine.appendChild(divField1);
    divLine.appendChild(divField2);
    divLine.appendChild(divField3);

    el.appendChild(newInputFile);
    el.appendChild(divLine);

    $('#btnaddnewfile').prop("value", "Attach another file (max. 2 MB)");
};


/*Try Optimize/Reduce*/
function HTMLTag(objecttype,classvalue) 
{
    var result = document.createElement(objecttype);
    result.setAttribute('class', classvalue);
    return result;
}

function GetLineInputFile(containerid) //containerid=uploadfileuploadbox
{
    var el = document.getElementById(containerid); //retrieve container
    var number = 1;
    var divLines = el.childNodes;//elements inner container
    if (divLines != null && divLines != undefined && divLines.length > 0) {
        var lastItem = divLines.length - 1;
        var idOfLastDiv = divLines[lastItem];
        if (idOfLastDiv != null) {
            var idOfDiv = idOfLastDiv.id;
            var toRemove = 'divlinefile';
            var indexToCheck = idOfDiv.indexOf(toRemove);
            if (indexToCheck == 0) {
                var lastNumberStr = idOfDiv.replace(toRemove, '');
                if (lastNumberStr != null && lastNumberStr != undefined && lastNumberStr != '') {
                    var lastNumber = parseInt(lastNumberStr);
                    number = number + lastNumber;
                }
            }
        }
    }
    
    // DIV LINE

    var divLine = HTMLTag("div", 'lineDialog');
     divLine.id = 'divlinefile' + number;
    
    var divField1 = HTMLTag("div", 'fieldDialog'); //TextBox
    var divField2 = HTMLTag("div", 'fieldDialog'); //Browse
    var divField3 = HTMLTag("div", 'fieldDialog'); //Remove

    // TO FIRST DIV FIELD
    var newText = HTMLTag("input", 'textDialogUpload'); //
    newText.type = 'text';
    newText.id = 'txtuploadfile' + number;
    newText.name = 'txtuploadfile';
    divField1.appendChild(newText);
    
    // TO SECOND DIV FIELD

    var divContentField = HTMLTag("div", 'divButtonDialog'); //
    divContentField.id = 'mybutton' + number;
    divContentField.onclick = function() { $('#uploadfile' + number).trigger('click'); };

    var newInputFile = HTMLTag("input", 'fileDialogUpload'); //
    newInputFile.type = 'file';
    newInputFile.id = 'uploadfile' + number;
    newInputFile.name = 'upload';
    newInputFile.onchange = function() { $("#txtuploadfile" + number).prop("value", newInputFile.value); };

    var newLabel = HTMLTag("label", 'labelButtonDialog');

    var newSpan = document.createElement("span");
    newSpan.innerHTML = "Browse";
    newLabel.appendChild(newSpan);

    divContentField.appendChild(newLabel);
    divContentField.appendChild(newInputFile);
    divField2.appendChild(divContentField);

    // TO THIRD DIV FIELD
    var removeLabel = HTMLTag("label", 'labelButtonDialog');

    var removeSpan = document.createElement("span");
    removeLabel.innerHTML = "Remove";

    newLabel.appendChild(removeSpan);

    var divButtonReset = HTMLTag("div", 'divButtonDialog');
    divButtonReset.id = 'mybuttonreset' + number;
    divButtonReset.onclick = function() { RemoveFile('divlinefile' + number); };
    divButtonReset.appendChild(removeLabel);
    
    divField3.appendChild(divButtonReset);

    divLine.appendChild(divField1);
    divLine.appendChild(divField2);
    divLine.appendChild(divField3);

    el.appendChild(divLine);
}
