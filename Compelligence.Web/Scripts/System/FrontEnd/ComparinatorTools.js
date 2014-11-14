
function GetHowWePositionByProduct(url, MyProductsAndCompetitorIds,MyCompanyId) {
    var productsAndCompetitorIds = MyProductsAndCompetitorIds.split(':');
    var MySelect = $('#ProductList');
    if (MySelect.val() != null && MySelect.val() != '' && MySelect.val() != undefined) {
        var parameters = { ProductId: MySelect.val(), ProdCompIds: MyProductsAndCompetitorIds, C: MyCompanyId };

        $.get(url, parameters, function(data) 
        {
         if (data != '') 
         {
            var dataToShow = $('#hwpBoxDataList');
            dataToShow[0].innerHTML = GetValueByKey(data, 'HowWePosition_' + MySelect.val());
            setLinkTarget('hwpBoxDataList');
            var hdnClPPositioningId = $('#HdnClPPositioningId');
            hdnClPPositioningId[0].value = GetValueByKey(data, 'PositioningId_' + MySelect.val());
            var hdnClPPositioningAction = $('#HdnClPPositioningAction');
            hdnClPPositioningAction[0].value = GetValueByKey(data, 'Action_' + MySelect.val());
            var statementValue = $('#HdnClPPositioningStatement');
            statementValue[0].value = GetValueByKey(data, 'Statement_' + MySelect.val());
            for (var m = 0; m < productsAndCompetitorIds.length; m++) 
            {
                var ids = productsAndCompetitorIds[m].split('_');
                if (ids.length == 2) 
                {
                    var divHTA = $('#DivHowTheyAttack' + ids[1]);
                    divHTA[0].innerHTML = GetValueByKey(data, 'HowTheyAttack_' + ids[1]);
                    setLinkTarget('DivHowTheyAttack' + ids[1]);
                    var divHTD = $('#DivHowToDefend' + ids[1]);
                    divHTD[0].innerHTML = GetValueByKey(data, 'HowWeDefend_' + ids[1]);
                    setLinkTarget('DivHowToDefend' + ids[1]);
                    var hdnCMPositioningId = $('#HdnCMPositioningId' + ids[1]);
                    hdnCMPositioningId[0].value = GetValueByKey(data, 'CompetitiveMessagingId_' + ids[1]);
                    setLinkTarget('HdnCMPositioningId' + ids[1]);
                    var hdnCMPositioningStatment = $('#HdnCMPositioningStatment' + ids[1]);
                    hdnCMPositioningStatment[0].value = GetValueByKey(data, 'CompetitiveMessagingStatement_' + ids[1]);
                    setLinkTarget('HdnCMPositioningStatment' + ids[1]);
                    var hdnCMPositioningAction = $('#HdnCMPositioningAction' + ids[1]);
                    hdnCMPositioningAction[0].value = GetValueByKey(data, 'CompetitiveMessagingAction_' + ids[1]);
                    setLinkTarget('HdnCMPositioningAction' + ids[1]);
                }
            }
         }
       }); //get
    }
};