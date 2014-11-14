function validateVarible(varibale) {
    var result = false;
    if (varibale != undefined && varibale != null && varibale.length > 0) {
        result = true;
    }
    return result;
};

function GetValueByKeyAndToken(value, key, token) {
    var result = '';
    var pos = value.indexOf(key);
    if (pos != -1) {
        var subValu = value.substring(pos + key.length + 1);
        var tokenPos = subValu.indexOf(token);
        if (tokenPos != -1) {
            result = subValu.substring(0, tokenPos);
        }
        else {
            result = subValu;
        }
    }
    return result;
};
////dialogTitle: TITLE IN DIALOG POPUP
////urlAction: URL TO CREATE OR UPDATE
////dialogContent: CONTENT WHERE WILL OPEN THE DIALOG POPUP
////positioningAction:
////idContent: ID TO BUILD THE ITEMS OF FORM
////relation:P[POSITIONING]/CM[COMPETITIVE MESSAGING]
////clasification:CC[CLIENT COMPANY]/OC[OTHERS COMPANY]
function FormGeneralClientPositioningBox(object, dialogTitle, urlAction, dialogContent, MyCompanyId, MyUserId, positioningId, positioningAction, idContent, relation, clasification) {
    var positioningObject = $("#PositioningBox");
    positioningObject.empty();
    positioningObject.dialog({
        width: 575,
        modal: true,
        buttons: {
            "Ok": function() {
                var hdnIndustryId = $('#' + idContent + 'DialogFormEdit input[name=hdnIndustryId]').val();
                var hdnEntityId = $('#' + idContent + 'DialogFormEdit input[name=hdnEntityId]').val();
                var hdnEntityType = $('#' + idContent + 'DialogFormEdit input[name=hdnEntityType]').val();
                var hdnPositioningRelation = $('#' + idContent + 'DialogFormEdit input[name=hdnPositioningRelation]').val();
                var hdnAction = $('#' + idContent + 'DialogFormEdit input[name=hdnAction]').val();
                var hdnPositioningId = $('#' + idContent + 'DialogFormEdit input[name=hdnPositioningId]').val();
                var hcnIsCompany = $('#' + idContent + 'DialogFormEdit input[name=hcnIsCompany]').val();
                var statementField = $('#' + idContent + 'DialogFormEdit input[name=txtStatment]').val();
                statementField = convertTextPlainHtml(statementField);
                var howWePositionField = '';
                var howTheyAttackField = '';
                var howToDefendField = '';
                var howTheyPositionField = '';
                var howWeAttackField = '';

                if (relation === 'P') {
                    if (clasification === 'CC') {
                        howWePositionField = $('#' + idContent + 'DialogFormEdit').find('textarea[name=TxtHowWePosition]').val();
                    }
                    else {
                        howTheyPositionField = $('#' + idContent + 'DialogFormEdit').find('textarea[name=TxtHowTheyPosition]').val();
                        howWeAttackField = $('#' + idContent + 'DialogFormEdit').find('textarea[name=TxtHowWeAttack]').val();
                    }
                }
                else {
                    howTheyAttackField = $('#' + idContent + 'DialogFormEdit').find('textarea[name=TxtHowTheyAttack]').val();
                    howToDefendField = $('#' + idContent + 'DialogFormEdit').find('textarea[name=TxtHowToDefend]').val();
                }
                var lblValidSum = $('#LblValidationSumary');
                var isValidTheForm = true;
                var messageOfValidation = '';
                var hwpEncode = htmlEncode(howWePositionField);
                var htaEncode = htmlEncode(howTheyAttackField);
                var htdEncode = htmlEncode(howToDefendField);
                var htpEncode = htmlEncode(howTheyPositionField);
                var hwaEncode = htmlEncode(howWeAttackField);
                var parameters = { hdnIndustryId: hdnIndustryId,
                    hdnEntityId: hdnEntityId,
                    hdnEntityType: hdnEntityType,
                    hdnPositioningRelation: hdnPositioningRelation, hdnAction: hdnAction, hdnPositioningId: hdnPositioningId,
                    txtStatment: statementField,
                    TxtHowWePosition: hwpEncode,
                    TxtHowTheyAttack: htaEncode,
                    TxtHowToDefend: htdEncode,
                    TxtHowTheyPosition: htpEncode,
                    TxtHowWeAttack: hwaEncode,
                    hdnC: MyCompanyId, hdnU: MyUserId, hcnIsCompany: hcnIsCompany
                };
                var isInvalidateStatement = false;
                var isInvalidateFirstGroup = false; //howWePositionField [Positioning]/[ClientCompany]
                var isInvalidateSecondGroup = false; //howTheyPositionField && howWeAttackField [Positioning]/[OthersCompany]
                var isInvalidateThirdGroup = false; // howTheyAttackField && howToDefendField [CompetitiveMessaging]
                if (isEmpty(statementField) || statementField.length > 255) {
                    isInvalidateStatement = true;
                }
                if (relation === 'P') {
                    isInvalidateThirdGroup = false;
                    if (clasification === 'CC') {
                        isInvalidateSecondGroup = false;
                        if (isEmpty(howWePositionField) || howWePositionField.length > 8000) {////POSITIONING-HowWePosition ACCEPT 8000 CHARACTERS IN DATABASE
                            isInvalidateFirstGroup = true;
                        }
                    } else {
                        isInvalidateFirstGroup = false;
                        if (isEmpty(howTheyPositionField) || isEmpty(howWeAttackField) || howTheyPositionField.length > 8000 || howWeAttackField.length > 8000) {
                            isInvalidateSecondGroup = true;
                        }
                    }
                } else {
                    isInvalidateFirstGroup = false;
                    isInvalidateSecondGroup = false;
                    if (isEmpty(howTheyAttackField) || isEmpty(howToDefendField) || howTheyAttackField.length > 8000 || howToDefendField.length > 8000) {
                        isInvalidateThirdGroup = true;
                    }
                }

                if (isInvalidateStatement || isInvalidateFirstGroup || isInvalidateSecondGroup || isInvalidateThirdGroup) {
                    if (isEmpty(statementField)) {
                        messageOfValidation += 'Statement is required<br/>';
                    }
                    if (statementField.length > 255) {
                        messageOfValidation += 'Statement has more than 255 characters<br/>';
                    }
                    if (relation === 'P') {
                        if (clasification === 'CC') {
                            if (isEmpty(howWePositionField)) {
                                messageOfValidation += 'How We Position is required<br/>';
                            } else {
                                if (howWePositionField.length > 8000) {
                                    messageOfValidation += 'Fields have a limit of 8000 characters including HTML characters which may not be visible<br/>';
                                }
                            }
                        }
                        else {
                            if (isEmpty(howTheyPositionField)) {
                                messageOfValidation += 'How They Position is required<br/>';
                            }
                            if (isEmpty(howWeAttackField)) {
                                messageOfValidation += 'How We De-Position Them is required<br/>';
                            }
                            if (howTheyPositionField.length > 8000 || howWeAttackField.length > 8000) {
                                messageOfValidation += 'Fields have a limit of 8000 characters including HTML characters which may not be visible<br/>';
                            }
                        }
                    }
                    else {
                        if (isEmpty(howTheyAttackField)) {
                            messageOfValidation += 'How They De-Position Us is required<br/>';
                        }
                        if (isEmpty(howToDefendField)) {
                            messageOfValidation += 'How We Respond is required<br/>';
                        }
                        if (howTheyAttackField.length > 8000 || howToDefendField.length > 8000) {
                            messageOfValidation += 'Fields have a limit of 8000 characters including HTML characters which may not be visible<br/>';
                        }
                    }
                    lblValidSum.prop('innerHTML', messageOfValidation);
                    lblValidSum.css("color", "red");
                }
                else {
                    if (relation === 'P') {
                        if (clasification === 'CC') {
                            var divHWPToChange = $('#Whp_' + positioningId);
                            divHWPToChange[0].innerHTML = howWePositionField;
                        } else {
                            var divHTP = $('#Htp_' + positioningId);
                            divHTP[0].innerHTML = howTheyPositionField;
                            var divHWA = $('#Hwa_' + positioningId);
                            divHWA[0].innerHTML = howWeAttackField;
                        }
                    } else {
                        var divHTAToChange = $('#Hta_' + positioningId);
                        divHTAToChange[0].innerHTML = howTheyAttackField;

                        var divHWDToChange = $('#Hwd_' + positioningId);
                        divHWDToChange[0].innerHTML = howToDefendField;
                    }
                    $.post(urlAction, parameters, function(data) {// THE ITEMS WITH ID POSITIONING NEED UPDATED
                        var originalId = $(object).attr('id');
                        // example: ImgHta_1000000612_COMPT_P_N
                        if (positioningAction === 'Create') {
                            if (relation === 'P') {
                                if (clasification === 'CC') {
                                    $('#Whp_' + positioningId).attr('id', 'Whp_' + data);
                                    var newIdField1 = 'ImgWhp_' + data + '_' + hdnEntityType + '_P_N';
                                    $(object).attr('id', newIdField1);
                                }
                                else {
                                    $('#Hwa_' + positioningId).attr('id', 'Hwa_' + data);
                                    $('#Htp_' + positioningId).attr('id', 'Htp_' + data);
                                    var newIdField2 = 'ImgHtp_' + data + '_' + hdnEntityType + '_P_N';
                                    var newIdField3 = 'ImgHwa_' + data + '_' + hdnEntityType + '_P_N';

                                    var keyStr = 'ImgHtp';

                                    if (originalId.indexOf(keyStr) != -1) {
                                        $(object).attr('id', newIdField2);
                                        var complementId = originalId.replace('ImgHtp', 'ImgHwa');
                                        $('#' + complementId).attr('id', newIdField3);
                                    } else {
                                        $(object).attr('id', newIdField3);
                                        var complementId = originalId.replace('ImgHwa', 'ImgHtp');
                                        $('#' + complementId).attr('id', newIdField2);
                                    }
                                }
                            }
                            else {
                                $('#Hta_' + positioningId).attr('id', 'Hta_' + data);
                                $('#Hwd_' + positioningId).attr('id', 'Hwd_' + data);
                                var newIdField4 = 'ImgHta_' + data + '_' + hdnEntityType + '_CM_N';
                                var newIdField5 = 'ImgHwd_' + data + '_' + hdnEntityType + '_CM_N';
                                var keyStr = 'ImgHta';
                                if (originalId.indexOf(keyStr) != -1) {
                                    $(object).attr('id', newIdField4);
                                    var complementId = originalId.replace('ImgHta', 'ImgHwd');
                                    $('#' + complementId).attr('id', newIdField5);
                                } else {
                                    $(object).attr('id', newIdField5);
                                    var complementId = originalId.replace('ImgHwd', 'ImgHta');
                                    $('#' + complementId).attr('id', newIdField4);
                                }
                            }
                        }
                    });
                    $(this).dialog('destroy');
                }
            },
            "Close": function() {
                $(this).dialog('destroy');
            }
        }
    });
    positioningObject.dialog('option', 'title', dialogTitle);
    positioningObject.html(dialogContent);
    positioningObject.dialog("open");
    $('.txtBoxHtml').cleditor({ height: 150, width: 545 });
};

//// This method will get
//// positioningRelation: P[POSITIONING]/CM[COMPETITIVE MESSAGIN]
//// clasification: CC[CLIENT COMPANY]/OC[OTHER COMPANIES]
function builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, clasification, idContent) {
    builFormToPositioning(IndustryId, ProductId, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, clasification, idContent, "", "", "");
}
//firstField: Statement Name
//secondField:HowWePosition,HowTheyPosition,HowTheyAttack
//thirdField: HowWeAttack,HowWeDefend
function builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, clasification, idContent, firstField, secondField, thirdField) {
    var strCMPDForm = "<div id='" + idContent + "DialogFormEdit'>";
    var isCompany = 'N';
    if (positioningRelation === 'CM' || clasification === 'CC') isCompany = 'Y';
    strCMPDForm += "<input type='hidden' name='hdnIndustryId' value='" + IndustryId + "' />";
    strCMPDForm += "<input type='hidden' name='hdnEntityId' value='" + entityId + "' />";
    strCMPDForm += "<input type='hidden' name='hdnEntityType' value='" + entityType + "' />";
    strCMPDForm += "<input type='hidden' name='hdnPositioningRelation' value='" + positioningRelation + "' />";
    strCMPDForm += "<input type='hidden' name='hdnPositioningId' value='" + positioningId + "' />";
    strCMPDForm += "<input type='hidden' name='hdnAction' value='" + urlActionTo + "' />";
    strCMPDForm += "<input type='hidden' name='hdnC' value='" + MyCompanyId + "' />";
    strCMPDForm += "<input type='hidden' name='hdnU' value='" + MyUserId + "' />";
    strCMPDForm += "<input type='hidden' name='hcnIsCompany' value='" + isCompany + "' />";
    strCMPDForm += "<label id='LblValidationSumary'></label>";
    strCMPDForm += "<label>Statement Name:</label><br /><input type='text' name='txtStatment' ";
    if (validateVarible(firstField)) {
        strCMPDForm += " value='" + firstField + "' ";
    }
    strCMPDForm += "/><br />";
    if (positioningRelation === 'P') {// TO CREATE THE FIELDS TO CLIENT POSITIONING / COMPETITIVE MESSAGING
        if (clasification === 'CC') {//CREATE A FIEDL TO CLIENT POSITIONING
            strCMPDForm += "<label>How We Position:</label><br /><textarea name='TxtHowWePosition' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
            if (secondField != undefined && secondField != null && secondField.length > 0) {
                strCMPDForm += secondField;
            }
            strCMPDForm += "</textarea><br />";
        } else { // CREATED FIELDS TO CLIENT COMPETITIVE MESSAGING
            strCMPDForm += "<label>How They Position:</label><br /><textarea name='TxtHowTheyPosition' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
            if (validateVarible(secondField)) {
                strCMPDForm += secondField;
            }
            strCMPDForm += "</textarea><br />";
            strCMPDForm += "<label>How We De-Position Them:</label><br /><textarea name='TxtHowWeAttack' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
            if (validateVarible(thirdField)) {
                strCMPDForm += thirdField;
            }
            strCMPDForm += "</textarea><br />";
        }
    } else {// CREATE FIELDS TO OTHER COMPETITORS POSITIONING
        strCMPDForm += "<label>How They De-Position Us:</label><br /><textarea name='TxtHowTheyAttack' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
        if (validateVarible(secondField)) {
            strCMPDForm += secondField;
        }
        strCMPDForm += "</textarea><br />";
        strCMPDForm += "<label>How We Respond:</label><br /><textarea name='TxtHowToDefend' WRAP=SOFT COLS=50 ROWS=4 class='txtBoxHtml'>";
        if (validateVarible(thirdField)) {
            strCMPDForm += thirdField;
        }
        strCMPDForm += "</textarea><br />";
    }
    strCMPDForm += "</div>";
    return strCMPDForm;
};

//// typeClasification:CC[ClientCompany]/OC[OthersCompany]
function AddGeneralClientPositioningDialog(object, Title, urlActionC, urlActionU, urlActionG, IndustryId, entityId, entityType, positioningRelation, MyCompanyId, MyUserId, positioningId, positioningAction, typeClasification) {
    var urlActionTo = urlActionC;
    var statementValue = '';
    if (positioningAction == 'Update') {
        urlActionTo = urlActionU;
        var parametro = { PositioningId: positioningId };
        var xmlhttp;
        var results = null;
        $.get(urlActionG,
            parametro,
            function(data) {
                if (data != null && data != '') {
                    results = data;
                    if (results != "") {
                        statementValue = GetValueByKeyAndToken(results, 'PositioningStatment_' + positioningId, '[TK' + positioningId + ']');
                        if (positioningRelation === 'CM') {
                            divHTAToChange = GetValueByKeyAndToken(results, 'PositioningHowTheyAttack_' + positioningId, '[TK' + positioningId + ']');
                            divHTDToChange = GetValueByKeyAndToken(results, 'PositioningHowToDefend_' + positioningId, '[TK' + positioningId + ']');
                            var strCMPDForm = builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, typeClasification, 'CompetitiveMessaging', statementValue, divHTAToChange, divHTDToChange);
                            FormGeneralClientPositioningBox(object, Title, urlActionTo, strCMPDForm, MyCompanyId, MyUserId, positioningId, positioningAction, 'CompetitiveMessaging', positioningRelation, typeClasification);
                        }
                        else {
                            if (typeClasification === 'CC') {
                                divHWPToChange = GetValueByKeyAndToken(results, 'PositioningHowWePosition_' + positioningId, '[TK' + positioningId + ']');
                                var strCMPDForm = builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, typeClasification, 'ClientPositionig', statementValue, divHWPToChange, '');
                                FormGeneralClientPositioningBox(object, Title, urlActionTo, strCMPDForm, MyCompanyId, MyUserId, positioningId, positioningAction, 'ClientPositionig', positioningRelation, typeClasification);
                            }
                            else {
                                divHTPToChange = GetValueByKeyAndToken(results, 'PositioningHowTheyPosition_' + positioningId, '[TK' + positioningId + ']');
                                divHWAToChange = GetValueByKeyAndToken(results, 'PositioningHowWeAttack_' + positioningId, '[TK' + positioningId + ']');
                                var strCMPDForm = builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, typeClasification, 'CompetitorPositionig', statementValue, divHTPToChange, divHWAToChange);
                                FormGeneralClientPositioningBox(object, Title, urlActionTo, strCMPDForm, MyCompanyId, MyUserId, positioningId, positioningAction, 'CompetitorPositionig', positioningRelation, typeClasification);
                            }
                        }
                    }
                }
            });
    }
    if (positioningRelation === 'CM') {
        var strCMPDForm = builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, typeClasification, 'ClientPositionig');
        FormGeneralClientPositioningBox(object, Title, urlActionTo, strCMPDForm, MyCompanyId, MyUserId, positioningId, positioningAction, 'ClientPositionig', positioningRelation, typeClasification);
    }
    else {
        if (typeClasification === 'CC') {
            var strCMPDForm = builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, typeClasification, 'CompetitorPositionig');
            FormGeneralClientPositioningBox(object, Title, urlActionTo, strCMPDForm, MyCompanyId, MyUserId, positioningId, positioningAction, 'CompetitorPositionig', positioningRelation, typeClasification);
        } else {
            var strCMPDForm = builFormToPositioning(IndustryId, entityId, entityType, positioningRelation, positioningId, urlActionTo, MyCompanyId, MyUserId, typeClasification, 'CompetitorPositionig');
            FormGeneralClientPositioningBox(object, Title, urlActionTo, strCMPDForm, MyCompanyId, MyUserId, positioningId, positioningAction, 'CompetitorPositionig', positioningRelation, typeClasification);
        }
    }
};