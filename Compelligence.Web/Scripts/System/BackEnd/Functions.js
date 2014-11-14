    var projectHeaderId = null;
    var postLoadContent = function(){}; 
    var getCurrentHeaderId = function(scope, entity) {
        var prjDisId = getIdValue(scope, entity);
        if (prjDisId == null && projectHeaderId != null && projectHeaderId != '') {
            prjDisId = projectHeaderId;
        }

        return prjDisId;
    }
    /** Load any content type in a "target" section for create a new section in BackEnd workspace */ 
    var loadContent = function(urlAction, target, scope) {
    
        showLoadingDialogForSection(target);
        $(target).load(urlAction, {Scope: scope, Container: target}, function() {hideLoadingDialogForSection(target); postLoadContent(); postLoadContent = function(){}; });
    };
    /** Load any content type in a "target" section for create a new section in BackEnd with a parameter */ 
    var loadContentDetail = function(urlAction, target, scope, typeObject) {
        $(target).html('');
        showLoadingDialogForSection(target);
        $(target).load(urlAction, {Scope: scope, Container: target, TypeObject: typeObject}, function() {hideLoadingDialogForSection(target); postLoadContent(); postLoadContent = function(){}; });
    };
    /** Load form content in a "target" section for "New" operation */ 
    var loadForm = function(urlAction, target, scope, browseId, isDetail, container, headerType, detailFilter) {
        var parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail, Container: container};
        
        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
        
        
        if (isDetail)
        {
            parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail, Container: container, HeaderType: headerType, DetailFilter: detailFilter};
        }
        
        $.get(urlAction, parameters, function(data) {$(target).html(data); if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); }});
    };
    
    var loadForms = function(urlAction, target, scope, browseId, isDetail, container, headerType, detailFilter, competitorId) {
        var parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail, Container: container};
        
        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
        
        
        if (isDetail)
        {
            parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail, Container: container, HeaderType: headerType, DetailFilter: detailFilter};
        }
        
        $.get(urlAction + '/' + competitorId, parameters, function(data) {$(target).html(data); if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); }});
    };
    
    /** Load data content in a "target" section for "Edit" operation */ 
    var loadData = function(urlAction, id, target, scope, formId, browseId, isDetail, container, headerType, detailFilter) {
        var parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail, Container: container};
        
        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
        
        if (isDetail)
        {
            parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail, Container: container, HeaderType: headerType, DetailFilter: detailFilter};
        }
        
        $.get(urlAction + '/' + id, parameters, function(data) {$(target).html(data); /*if(!isDetail) { disableFormFields(formId); }*/ if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); } });
    };
    
    /** Delete detail data in a Grid, and reload a "target" section */
    var deleteDetailData = function(urlAction, id, target, targetSection, isDetail, container, headerType, detailFilter) {
		var newId = id + "";
		if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
        
		$.post(urlAction , { Id: newId, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter }, function() {reloadGrid(target); $(targetSection).empty(); if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); }});
    };
    
    /** Delete data in a Grid, and reload a "target" section */
    var deleteData = function(urlAction, id, target, targetSection, isDetail, container, headerType, detailFilter) {
        var newId = id + "";
        var parameters = {Id: newId};
        
		if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
		
		if (isDetail)
		{
		    parameters = { Id: newId, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter };
		}
	    $.ajax({
            type: "POST",
            url: urlAction,
            dataType: "json",
            data: parameters,            
            success: function(data) {
                reloadGrid(target); 
		        $(targetSection).empty(); 
		                
		        if (container != null) { 
		            hideLoadingDialogForSection(container); 
		        } else { 
		            hideLoadingDialog(); 
		        }
		                
		        if ((data.ReturnMessage != null) && (data.ReturnMessage != '')) {
		            var errorMessage = '<p><span class="ui-icon ui-icon-alert alertFailedResponseDialog"></span>Operation was not successful</p>';
		                    
		            $('#AlertReturnMessageDialog').html(errorMessage + data.ReturnMessage);
		            $('#AlertReturnMessageDialog').dialog('open');		            
		        } else {               
                    displayNote('HistoryField.aspx/UpdateFieldChanges',data.GroupIdUser);
                    
		            setTimeout ("$.blockUI({ message: $('#SuccessDeleteMessage'), fadeIn: 700, fadeOut: 700, timeout: 3000," + 
                                "showOverlay: false, centerY: false, css: { width: '300px', top: '10px', left: '', right: '10px', " +
                                "border: 'none', padding: '5px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: 0.6, color: '#fff' } });", 1000);
		        }
            }
        }); 		
    };
    
    var duplicateData = function(urlAction, scope, id, targetGrid, targetSection, formId, browseId, isDetail, container, headerType, detailFilter) {
        var parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail};
        
        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
        
        if (isDetail)
        {
            parameters = {Scope: scope, BrowseId: browseId, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter};
        }
        
        $.post(urlAction , parameters, function(data) {if(!isDetail) { $(targetSection).html(data) }; reloadGrid(targetGrid); /*if(!isDetail) { disableFormFields(formId); }*/ if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); }});
    };
    
    var loadSelectedDataItems = function(urlAction, scope, id, targetGrid, isDetail, headerType, detailFilter) {
        var parameters = {Id: id, Scope: scope, IsDetail: isDetail};
        showLoadingDialog();
        
        if (isDetail)
        {
            parameters = {Id: id, Scope: scope, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter};
        }
        
        $.post(urlAction, parameters, function() { reloadGrid(targetGrid); hideLoadingDialog();});
    };
    
    var loadReloadGrid = function(targetGrid) {
        showLoadingDialog();
        reloadGrid(targetGrid);hideLoadingDialog();
    };
   var loadSelectedDataItemsPositioning = function(urlAction, scope, id, targetGrid, isDetail, headerType, detailFilter, query) {
        var parameters = {Id: id, Scope: scope, IsDetail: isDetail};
        showLoadingDialog();
        
        if (isDetail)
        {
            parameters = {Id: id, Scope: scope, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter};
        }
        var currentUrl = $(targetGrid).getGridParam("url");
        var urlA = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
//        if (document.getElementById(scope + "PositioningAllSelectedOption") != null) {
//        if (result != '') {
//            document.getElementById(scope + "PositioningAllSelectedOption").innerHTML = 'Criteria Filter: ' + value + ' ' + value2 + " '" + result + "'";
//        } else {
//            document.getElementById(scope + "PositioningAllSelectedOption").innerHTML = '';
//        }
//    }    
//        if (document.getElementById(scope + "PositioningAllSelectedOption") != null) {
//            document.getElementById(scope + "PositioningAllSelectedOption").innerHTML = 'Criteria Filter: ' + value + ' ' + value2 + " '" + result + "'";
//        }
        $.post(urlAction, parameters, function() { $(targetGrid).setGridParam({ url: urlA + query, page: 1 }).trigger('reloadGrid'); hideLoadingDialog();});
    };
    
    var reloadGridPositioning = function(gridId, urlAction, query){
        $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid');
    };
    
    /** New entity method */
    var newEntity = function(urlAction, scope, entity, browseId, container, headerType, detailFilter) {
    
        var componentId =  scope + entity;
        var gridId = scope + browseId;
        var target = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        $('#' + gridId + 'ListTable').resetSelection();
        
        loadForm(urlAction, target, scope, browseId, isDetail, container, headerType, detailFilter);
        
        if (isDetail) {
            $('#' + componentId + 'DetailDataListContent').fadeOut('slow');
            $(target).empty();
            $(target).fadeIn('slow');       
        } else {
            eval('showFirstSubtab(' + entity + 'Subtabs);'); 
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };
    
    var newEntitys = function(urlAction, scope, entity, browseId, container, headerType, detailFilter, competitorId) {
    
        var componentId =  scope + entity;
        var gridId = scope + browseId;
        var target = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        $('#' + gridId + 'ListTable').resetSelection();
        
        loadForms(urlAction, target, scope, browseId, isDetail, container, headerType, detailFilter, competitorId);
        
        if (isDetail) {
            $('#' + componentId + 'DetailDataListContent').fadeOut('slow');
            $(target).empty();
            $(target).fadeIn('slow');       
        } else {
            eval('showFirstSubtab(' + entity + 'Subtabs);'); 
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };
    
     var newEntityHierarchy = function(urlAction, scope, entity, browseId, container, headerType, detailFilter) {
        var hierarchyCheck = $('#' + scope + entity + 'HierarchyCheckbox').prop('checked');
        if(hierarchyCheck){
        var componentId =  scope + entity;
        var browse = browseId.toString();
        var posAll = browse.indexOf('All');
        browseId=browse.substring(0,posAll);
        browseId=browseId+'ByParent';
        var gridId = scope + browseId;
        var target = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        $('#' + gridId + 'ListTable').resetSelection();
        
        loadForm(urlAction, target, scope, browseId, isDetail, container, headerType, detailFilter);
        
            if (isDetail) {
                $('#' + componentId + 'DetailDataListContent').fadeOut('slow');
                $(target).empty();
                $(target).fadeIn('slow');
            } else {
                eval('showFirstSubtab(' + entity + 'Subtabs);'); 
                eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
            }
        }
        else
        {
            newEntity(urlAction, scope, entity, browseId, container, headerType, detailFilter);
        }
    };
    
/** New entity method */
    function newEntityiFrame (urlAction, scope, entity, browseId, container, headerType, detailFilter) {
    
        var componentId =  scope + entity;
        var gridId = scope + browseId;
        var target = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        $('#' + gridId + 'ListTable').resetSelection();
        
        loadiFrameContent(urlAction, target, scope, browseId, isDetail, container, headerType, detailFilter);
        
        if (isDetail) {
            $('#' + componentId + 'DetailDataListContent').fadeOut('slow');
            $(target).empty();
            $(target).fadeIn('slow');
        } else {
            eval('showFirstSubtab(' + entity + 'Subtabs);'); 
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };    
    /** Edit entity method */
    var editEntity = function(urlAction, scope, entity, browseId, container, headerType, detailFilter) {
    
        var componentId =  scope + entity;
        var gridId = scope + browseId;
        var selectedRow = $('#' + gridId + 'ListTable').getGridParam('selrow');
        var formId = '#' + componentId + 'EditForm';
        var isDetail = isDetailOperation(detailFilter);
        
        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }
        
        if (isDetail) {
            // Hide Grid Detail and show Edit form
            getEntity(urlAction, scope, entity, selectedRow, browseId, container, headerType, detailFilter);
        } else {
            enableFormFields(formId);
        }
    };
    
    var editPositioning = function(urlAction, scope, entity, browseId, container, headerType, detailFilter) {
        var hierarchyCheckCompetitive = $('#' + scope + 'CompetitiveHierarchyCheckbox').prop('checked');
        var hierarchyCheckPositioning = $('#' + scope + 'PositioningHierarchyCheckbox').prop('checked');
        var gridId = scope + browseId;
        if(hierarchyCheckCompetitive ||hierarchyCheckPositioning ){
            gridId = scope+entity+ 'AllByHierarchy';
        }
        var componentId =  scope + entity;
        var selectedRow = $('#' + gridId + 'ListTable').getGridParam('selrow');
        var formId = '#' + componentId + 'EditForm';
        var isDetail = isDetailOperation(detailFilter);
        
        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }
        
        if (isDetail) {
            // Hide Grid Detail and show Edit form
            getEntity(urlAction, scope, entity, selectedRow, browseId, container, headerType, detailFilter);
        } else {
            enableFormFields(formId);
        }
    };
    
    /** Load entity method */
    var loadEntity = function(urlAction, scope, entity, selectedId, browseId, container, headerType, detailFilter) {
        var componentId =  scope + entity;
        var target = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        loadData(urlAction, selectedId, target, scope, null, browseId, isDetail, container, headerType, detailFilter);
    };
    
    var loadEntityData = function(urlAction, scope, entity, entityId, browseId, container, headerType, detailFilter) {
        var componentId =  scope + entity;
        var target = '#' + componentId + 'EditFormContent';
        var formId = '#' + componentId + 'EditForm';
        var isDetail = isDetailOperation(detailFilter);
        
        loadData(urlAction, entityId, target, scope, formId, browseId, isDetail, container, headerType, detailFilter);
    };
    
    /** Get entity method */
    var getEntity = function(urlAction, scope, entity, selectedRow, browseId, container, headerType, detailFilter) {
        var componentId =  scope + entity;
        var gridId = scope + browseId;
        var gridName = '#' + gridId + 'ListTable';
        var target = '#' + componentId + 'EditFormContent';
        var formId = '#' + componentId + 'EditForm';
        var isMultiselect = getBooleanValue($(gridName).getGridParam('multiselect'));
        var isDetail = isDetailOperation(detailFilter);
        var selectedItems;
        var isPressedKey = $(gridName).getGridParam('isPressedKey');
        
        if (isMultiselect) {
            selectedItems = $(gridName).getGridParam('selarrrow');
        } else {
            selectedItems = [$(gridName).getGridParam('selrow')];
        }
        
        if (isPressedKey != true)
        {

            loadData(urlAction, selectedRow, target, scope, formId, browseId, isDetail, container, headerType, detailFilter);
            
            if (isDetail) {
                $('#' + componentId + 'DetailDataListContent').fadeOut('slow');
                $(target).empty();
                $(target).fadeIn('slow');
            } else {
                eval('showSubtabs(' + entity + 'Subtabs);'); 
                eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
            }
            /*
            if (selectedItems.length == 0){
                $(target).empty();
        
                if (isDetail) {
                    $(target).fadeOut('slow');
                    $('#' + componentId + 'DetailDataListContent').fadeIn('slow');
                } else {
                    eval('hideSubtabs(' + entity + 'Subtabs);'); 
                }
            }
            */
        }
    };
    
    
    var getLibraryEntity = function(urlAction, scope, entity, selectedRow, browseId, container, headerType, detailFilter) {
        var ids = selectedRow.split('_');
        if(ids.length == 2)
        {
            getEntity(urlAction, scope, entity, ids[1], browseId, container, headerType, detailFilter);
        }
    };
    
    var getDiscussionEntityId = function(urlRoot, scope, entity, responseId) {
        $.get(urlRoot + '/ForumDiscussion.aspx/GetEntityId/' + responseId, {}, function(parentid){            
            projectHeaderId = parentid;
            eval("showSubtabs(" + entity + "Subtabs);");
            eval(entity + "Subtabs.setActiveTab('" + scope + entity + "DiscussionContent');");
        });
    };
    
    var getCommentEntity = function(urlRoot, scope, entity, responseId) {           
           var win = window.open(urlRoot + '/' + entity + '/Comments/' + responseId, '_blank');
           win.focus();         
    };
    
    var getFeedBackEntity = function(urlRoot, scope, entity, responseId) {           
            projectHeaderId = responseId;
            eval("showSubtabs(" + entity + "Subtabs);");
            eval(entity + "Subtabs.setActiveTab('" + scope + entity + "FeedbackContent');");        
    };
    
    var showEntity = function(urlAction, scope, entity, selectedRow, browseId, container, headerType, detailFilter) {
        var componentId =  scope + entity;
        var target = '#' + componentId + 'EditFormContent';
        var formId = '#' + componentId + 'EditForm';
        var isDetail = isDetailOperation(detailFilter);
     

        if (isDetail) {
            $('#' + componentId + 'DetailDataListContent').fadeOut('slow');
            $(target).empty();
            $(target).fadeIn('slow');
        } else {

            setTimeout("showDetailEntity('"+ entity + "','" + componentId+ "');", 6000);
            //showDetailEntity(entity, componentId);
        }
        
        setTimeout("loadData('"+ urlAction + "','" + selectedRow + "','" +  target + "','" + scope + "','" + formId + "','" + browseId + "'," + isDetail + ",'" + container + "','" + headerType + "','" + detailFilter + "');",1500);
        //loadData(urlAction , selectedRow, target, scope, formId, browseId, isDetail, container, headerType, detailFilter);
        
        //loadData(urlAction, selectedRow, target, scope, formId, browseId, isDetail, container, headerType, detailFilter);
    };
    
    var showDetailEntity = function(entity,componentId)
    {     
            eval('showSubtabs(' + entity + 'Subtabs);'); 
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
    };
    
    var sendParamsToOperation = function(entity, operation, scope, container, urlAction) {
        var browseName = scope + entity + 'All';
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }

        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Delete") {
                deleteMultipleEntities(urlAction, scope, entity, entity + 'All', container, IDsSelected);
            }
        if (operation == "Duplicate") {
                duplicateMultipleEntities(urlAction, scope, entity, entity + 'All', container, IDsSelected);
            }

    };    
    
    var sendParamsToOperationByBrowse = function(entity, operation, browseId, scope, container, urlAction) {
        var browseName = scope + browseId;
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }

        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Delete") {
                deleteMultipleEntities(urlAction, scope, entity, browseId, container, IDsSelected);
            }
        if (operation == "Duplicate") {
                duplicateMultipleEntities(urlAction, scope, entity, browseId, container, IDsSelected);
            }

    };
    
    var duplicateDetailEntity = function(urlAction, scope, entity, browseId, container, headerType, detailFilter)
    {
        var browseName = scope + browseId;
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }

        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        duplicateDetailMultipleEntities(urlAction, scope, entity, browseId, container, IDsSelected, headerType, detailFilter);
    };
    
    var duplicateDetailMultipleEntities = function(urlAction, scope, entity, browseId, container, Id, headerType, detailFilter) {
        var componentId = scope + entity;
        var gridId = scope + browseId;
        var formId = '#' + componentId + 'EditForm';
        var target = '#' + gridId + 'ListTable';
        var selectedRow = $(target).getGridParam('selrow');
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);

        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }

        duplicateDetailMultipleData(urlAction, scope, selectedRow, target, targetSection, formId, browseId, isDetail, container,Id, headerType, detailFilter);

        if (isDetail) {
        
        } else {
            eval('showFirstSubtab(' + entity + 'Subtabs);');
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };
    
    var duplicateDetailMultipleData = function(urlAction, scope, id, targetGrid, targetSection, formId, browseId, isDetail, container,id, headerType, detailFilter) {
        var newId = id + "";
        var parameters = { Scope: scope, BrowseId: browseId, IsDetail: isDetail, Id: newId, Container: container ,HeaderType: headerType, DetailFilter: detailFilter };

        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }

//        if (isDetail) {
//            parameters = { Scope: scope, BrowseId: browseId, IsDetail: isDetail, Id: id,HeaderType: headerType, DetailFilter: detailFilter };
//        }        
        $.post(urlAction, parameters, function(data) { if (!isDetail) { $(targetSection).html(data) }; reloadGrid(targetGrid); /*if(!isDetail) { disableFormFields(formId); }*/if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); } });
        /*
        $.ajax({
        type:"POST",
        url: urlAction,
        data:parameters
        }).done(function(data) {
           if (!isDetail) 
           { $(targetSection).html(data) }; 
           reloadGrid(targetGrid); //if(!isDetail) { disableFormFields(formId); }
           if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog();   
           }
        });*/

    };
    
    var changeStatusOfEntity = function(entity, browseid, operation, scope, container, urlAction){
        var browseName = scope + browseid;
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }
        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Enable") {
                changeStatusMultipleEntities(urlAction, scope, entity, browseid, container, IDsSelected);
            }
        if (operation == "Disable") {
                changeStatusMultipleEntities(urlAction, scope, entity, browseid, container, IDsSelected);
            }
    };
    var changeStatusOfDetailEntity = function(entity, browseid, operation, scope, container, headerType, detailFilter, urlAction){
        var browseName = scope + browseid;
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }
        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Enable") {
                changeStatusMultipleEntities(urlAction, scope, entity, browseid, container, IDsSelected, headerType, detailFilter);
            }
        if (operation == "Disable") {
                changeStatusMultipleEntities(urlAction, scope, entity, browseid, container, IDsSelected, headerType, detailFilter);
            }
    };
    var changeStatusMultipleEntities = function(urlAction, scope, entity, browseId, container, Id, headerType, detailFilter) {
        var componentId = scope + entity;
        var gridId = scope + browseId;
        var formId = '#' + componentId + 'EditForm';
        var target = '#' + gridId + 'ListTable';
        var selectedRow = $(target).getGridParam('selrow');
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);

        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }

        changeStatusMultipleData(urlAction, scope, selectedRow, target, targetSection, formId, browseId, isDetail, container,Id, headerType, detailFilter);

        if (isDetail) {
        
        } else {
            eval('showFirstSubtab(' + entity + 'Subtabs);');
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };
    
    var changeStatusMultipleData = function(urlAction, scope, id, targetGrid, targetSection, formId, browseId, isDetail, container,id, headerType, detailFilter) {
        var parameters = { Scope: scope, BrowseId: browseId, IsDetail: isDetail, Id: id, Container: container };

        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }
        
        if (isDetail) {
            parameters = { Scope: scope, BrowseId: browseId, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter };
        }
         //added /0 for receive id=0, but into parameters Id array all ids
        $.post(urlAction , parameters, function(data) { if (!isDetail) { $(targetSection).html(data) }; reloadGrid(targetGrid); /*if(!isDetail) { disableFormFields(formId); }*/if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); } });
        //$.post(urlAction, parameters, function(data) { if (!isDetail) {alert(data); $(targetSection).html(data) }; reloadGrid(targetGrid); /*if(!isDetail) { disableFormFields(formId); }*/if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); } });
    };
    
    var sendParamsToOperationBrowse = function(entity, browseid,operation, scope, container, urlAction) {
        var browseName = scope +browseid;
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }

        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Delete") {
                deleteMultipleEntities(urlAction, scope, entity, browseid, container, IDsSelected);
            }
        if (operation == "Duplicate") {
                duplicateMultipleEntities(urlAction, scope, entity, browseid, container, IDsSelected);
            }

    };
    
    var sendParamsToOperationOnNews = function(entity,browseId, operation, scope, container, urlAction) {
        var browseName = scope + browseId;
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
            
        }
        
        
        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Delete") {
                deleteMultipleEntities(urlAction, scope, entity, browseId, container, IDsSelected);
            }
        if (operation == "Duplicate") {
                duplicateMultipleEntities(urlAction, scope, entity, browseId, container, IDsSelected);
            }

    };
    
    
    var sendParamsToOperationByHierarchy = function(entity, operation, scope, container, urlAction) {
        var hierarchyCheck = $('#' + scope + entity + 'HierarchyCheckbox').prop('checked');
        if(hierarchyCheck){
        var browseName = scope + entity + 'ByParent';
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }

        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Delete") {
                deleteMultipleEntities(urlAction, scope, entity, entity + 'ByParent', container, IDsSelected);
            }
        if (operation == "Duplicate") {
                duplicateMultipleEntities(urlAction, scope, entity, entity + 'ByParent', container, IDsSelected);
            }
        }
        else
        {
            sendParamsToOperation (entity, operation, scope, container, urlAction);
        }
    };
    
    var sendParamsToOperationByPositioning = function(entity, operation, scope, container, urlAction) {
        var hierarchyCheckCompetitive = $('#' + scope + 'CompetitiveHierarchyCheckbox').prop('checked');
        var hierarchyCheckPositioning = $('#' + scope + 'PositioningHierarchyCheckbox').prop('checked');
        if(hierarchyCheckCompetitive ||hierarchyCheckPositioning ){
        var browseName = scope + entity + 'AllByHierarchy';
        var gridId = '#' + browseName + 'ListTable';
        var multiselect = getBooleanValue($(gridId).getGridParam('multiselect'));
        var selectedItems;

        if (multiselect) {
            selectedItems = $(gridId).getGridParam("selarrrow");
            selectedItems = selectedItems.join(':');
        } else {
            selectedItems = $(gridId).getGridParam('selrow');
        }

        if (checkNullString(selectedItems) == '') {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedItems.split(":");
        if (operation == "Delete") {
                deleteMultipleEntities(urlAction, scope, entity, entity + 'AllByHierarchy', container, IDsSelected);
            }
        if (operation == "Duplicate") {
                duplicateMultipleEntities(urlAction, scope, entity, entity + 'AllByHierarchy', container, IDsSelected);
            }
        }
        else
        {
            sendParamsToOperation (entity, operation, scope, container, urlAction);
        }
    };
    
    var deleteEntity = function(urlAction, scope, entity, browseId, container, headerType, detailFilter) {
        var componentId = scope + entity;
        var gridId = scope + browseId;
        var target = '#' + gridId + 'ListTable';
        var selectedRow = $(target).getGridParam('selrow');
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }
        
        var deleteConfirmDialog = $("#DeleteConfirmDialog").dialog('option', 'itemId', selectedRow).dialog('option', 'urlAction', urlAction);
                
        deleteConfirmDialog.dialog('option', 'targetDelete', target);
        deleteConfirmDialog.dialog('option', 'targetSection', targetSection);
        deleteConfirmDialog.dialog('option', 'isDetail', isDetail);
        deleteConfirmDialog.dialog('option', 'headerType', headerType);
        deleteConfirmDialog.dialog('option', 'detailFilter', detailFilter);
        deleteConfirmDialog.dialog('option', 'containerSection', container);
                
        deleteConfirmDialog.dialog('open');
    };

    var deleteMultipleEntities = function(urlAction, scope, entity, browseId, container, Id, headerType, detailFilter) {

        var componentId = scope + entity;
        var gridId = scope + browseId;
        var target = '#' + gridId + 'ListTable';
        var selectedRow = Id;
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);

        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }

        var deleteConfirmDialog = $("#DeleteConfirmDialog").dialog('option', 'itemId', selectedRow).dialog('option', 'urlAction', urlAction);

        deleteConfirmDialog.dialog('option', 'targetDelete', target);
        deleteConfirmDialog.dialog('option', 'targetSection', targetSection);
        deleteConfirmDialog.dialog('option', 'isDetail', isDetail);
        deleteConfirmDialog.dialog('option', 'headerType', headerType);
        deleteConfirmDialog.dialog('option', 'detailFilter', detailFilter);
        deleteConfirmDialog.dialog('option', 'containerSection', container);
        deleteConfirmDialog.dialog('option', 'Id', Id);
        deleteConfirmDialog.dialog('open');

    };
    
    var deleteDetailEntity = function(urlAction, scope, entity, browseId, container, headerType, detailFilter) {
        var componentId = scope + entity;
        var gridId = scope + browseId;
        var target = '#' + gridId + 'ListTable';
        var multiselect = getBooleanValue($(target).getGridParam('multiselect'));
        //var selectedRow = $(target).getGridParam('selrow');
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        var selectedRow = "";
        if (multiselect) {
            selectedRow = $(target).getGridParam("selarrrow");
            selectedRow = selectedRow.join(':');
        } else {
            selectedRow = $(target).getGridParam('selrow');
        }
        
        if (selectedRow == null || selectedRow == "" ) {
            showAlertSelectItemDialog();
            return;
        }
        var IDsSelected = selectedRow.split(":");
        var deleteConfirmDialog = $("#DeleteDetailConfirmDialog").dialog('option', 'itemId', IDsSelected).dialog('option', 'urlAction', urlAction);
                
        deleteConfirmDialog.dialog('option', 'targetDelete', target);
        deleteConfirmDialog.dialog('option', 'targetSection', targetSection);
        deleteConfirmDialog.dialog('option', 'isDetail', isDetail);
        deleteConfirmDialog.dialog('option', 'headerType', headerType);
        deleteConfirmDialog.dialog('option', 'detailFilter', detailFilter);
        deleteConfirmDialog.dialog('option', 'containerSection', container);
                
        deleteConfirmDialog.dialog('open');
    };
    
    var duplicateEntity = function(urlAction, scope, entity, browseId, container, headerType, detailFilter) {
        var componentId = scope + entity;
        var gridId = scope + browseId;
        var formId = '#' + componentId + 'EditForm';
        var target = '#' + gridId + 'ListTable';
        var selectedRow = $(target).getGridParam('selrow');
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
        
        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }
        
        duplicateData(urlAction, scope, selectedRow, target, targetSection, formId, browseId, isDetail, container, headerType, detailFilter);
        
        if (isDetail) {
             
        } else {
//            eval('showFirstSubtab(' + entity + 'Subtabs);'); 
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };

    var duplicateMultipleEntities = function(urlAction, scope, entity, browseId, container, Id, headerType, detailFilter) {
        var componentId = scope + entity;
        var gridId = scope + browseId;
        var formId = '#' + componentId + 'EditForm';
        var target = '#' + gridId + 'ListTable';
        var selectedRow = $(target).getGridParam('selrow');
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);

        if (selectedRow == null) {
            showAlertSelectItemDialog();
            return;
        }

        duplicateMultipleData(urlAction, scope, selectedRow, target, targetSection, formId, browseId, isDetail, container,Id, headerType, detailFilter);

        if (isDetail) {
        
        } else {
            eval('showFirstSubtab(' + entity + 'Subtabs);');
            eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
        }
    };
    
    var showafterSave = function(entity)  
    {            
            eval('showSubtabs(' + entity + 'Subtabs);'); 
            //eval(entity + "Subtabs.setActiveTab('" + componentId + "EditFormContent');");
     };

    var duplicateMultipleData = function(urlAction, scope, id, targetGrid, targetSection, formId, browseId, isDetail, container,id, headerType, detailFilter) {
        var newId = id + "";
        var parameters = { Scope: scope, BrowseId: browseId, IsDetail: isDetail, Id: newId, Container: container };

        if (container != null) {
            showLoadingDialogForSection(container);
        } else {
            showLoadingDialog();
        }

        if (isDetail) {
            parameters = { Scope: scope, BrowseId: browseId, IsDetail: isDetail, HeaderType: headerType, DetailFilter: detailFilter };
        }
        //added /0 for receive id=0, but into parameters Id array all ids
        $.post(urlAction , parameters, function(data) { if (!isDetail) { $(targetSection).html(data) }; reloadGrid(targetGrid); /*if(!isDetail) { disableFormFields(formId); }*/if (container != null) { hideLoadingDialogForSection(container); } else { hideLoadingDialog(); } });
    };
    
    var searchEntity = function(scope, entity, browseId) {
        var componentId = scope + browseId;        
        var searchTextField = 	$('#' + componentId + 'SearchForm input[type="text"][name=searchValue]');
        var countOfSearch = searchTextField.length;
        var tempo = countOfSearch -1;
        if( countOfSearch>1)
        {   
	        for(var i=0;i<tempo;i++)
	        {
		        var parentFieldShort = searchTextField[i].parentNode;
		        var parentLineShort=parentFieldShort.parentNode;
		        var parentFieldSet=parentLineShort.parentNode;
		        var parentForm=parentFieldSet.parentNode;
		        var parentDialog=parentForm.parentNode;
		        var parentDialogFilter = parentDialog.parentNode;
		        parentDialogFilter.removeChild(parentDialog);
	        }
        }
        var searchTextFieldOnly = searchTextField[tempo];
        searchTextFieldOnly = searchTextField[tempo];
        if(searchTextFieldOnly.parentNode != undefined)
        {
	        var parentFieldShortOnly = searchTextFieldOnly.parentNode;
	        if(parentFieldShortOnly.parentNode != undefined)
	        {
		        var parentLineShortOnly=parentFieldShortOnly.parentNode;
		        if(parentLineShortOnly.parentNode != undefined)
		        {
			        var parentFieldSetOnly=parentLineShortOnly.parentNode;
			        if(parentFieldSetOnly.parentNode != undefined)
			        {
				        var parentFormOnly=parentFieldSetOnly.parentNode;
				        if(parentFormOnly.parentNode != undefined)
				        {
					        var parentDialogOnly=parentFormOnly.parentNode;
					        if(parentDialogOnly.parentNode != undefined)
					        {
					          var parentDialogFilterOnly = parentDialogOnly.parentNode; 
						        if(parentDialogFilterOnly.parentNode != undefined)
						        {
							        var dialogSearch = parentDialogFilterOnly.parentNode;
							        dialogSearch.style.display = "block";
							        dialogSearch.style.left = "517.5px";
							        dialogSearch.style.top = "162.5px";
							        							
							        if(entity == "ProductIndustry" || "WinLossAnalysis" ||"ProductCompetitor" )
							        {
								        //Filter Product
								        var dialogSearch = parentDialogOnly.parentNode;
								        dialogSearch.style.display = "block";
								        dialogSearch.style.left = "517.5px";
								        dialogSearch.style.top = "162.5px";
							        }
							        searchTextFieldOnly.focus();
					           }
					        }
				        }
			        }
		        }
	        }
        }
    };
    
    var filterEntity = function(scope, entity, browseId) {
        var componentId = scope + browseId;
        var filterTextField = $('#' + componentId + 'FilterForm input[type="text"][name=' + componentId + 'filterValue]');
        var countOfFilter = filterTextField.length;
        var tempo = countOfFilter -1;
        if( countOfFilter>1)
        {   
            for(var i=0;i<tempo;i++)
            {
                var parentFieldShort = filterTextField[i].parentNode;
                var parentLineShort=parentFieldShort.parentNode;
                var parentFieldSet=parentLineShort.parentNode;
                var parentForm=parentFieldSet.parentNode;
                var parentDialog=parentForm.parentNode;
                var parentDialogFilter = parentDialog.parentNode;
                parentDialogFilter.removeChild(parentDialog);
            }
        }
//        var filterTextFieldOnly = $('#' + componentId + 'FilterForm input[type="text"][name=' + componentId + 'filterValue]').get(0);
        var filterTextFieldOnly = filterTextField[tempo];
//        if(countOfFilter>1)
//        {
          filterTextFieldOnly = filterTextField[tempo];
         if(filterTextFieldOnly.parentNode != undefined)
         {
            var parentFieldShortOnly = filterTextFieldOnly.parentNode;
            if(parentFieldShortOnly.parentNode != undefined)
            {
                var parentLineShortOnly=parentFieldShortOnly.parentNode;
                if(parentLineShortOnly.parentNode != undefined)
                {
                    var parentFieldSetOnly=parentLineShortOnly.parentNode;
                    if(parentFieldSetOnly.parentNode != undefined)
                    {
                        var parentFormOnly=parentFieldSetOnly.parentNode;
                        if(parentFormOnly.parentNode != undefined)
                        {
                            var parentDialogOnly=parentFormOnly.parentNode;
                            if(parentDialogOnly.parentNode != undefined)
                            {
                              var parentDialogFilterOnly = parentDialogOnly.parentNode; 
                                if(parentDialogFilterOnly.parentNode != undefined)
                                {
                                    var dialogFilter = parentDialogFilterOnly.parentNode;
                                    dialogFilter.style.display = "block";
                                    dialogFilter.style.left = "517.5px";
                                    dialogFilter.style.top = "162.5px";
                                    filterTextFieldOnly.focus();
                                
                                 if(entity == "ProductIndustry" || "WinLossAnalysis" ||"ProductCompetitor" )
                                {
                                    //Filter Product
                                    var dialogFilter = parentDialogOnly.parentNode;
                                    dialogFilter.style.display = "block";
                                    dialogFilter.style.left = "517.5px";
                                    dialogFilter.style.top = "162.5px";
                                    filterTextFieldOnly.focus();
                                }
                                
                                
                                
                               }
                            }
                        }
                    }
                }
            }
         }
        
//        }
//        else
//        {
//        $('#' + componentId + 'FilterDialog').dialog('open');
//        filterTextFieldOnly.focus();
//        }
    };

    var optionDetailEntity = function(urlAction, urlEditAction,scope, entity, browseId, container, headerType, detailFilter) {
        var componentId = scope + entity;
        var gridId = scope + browseId;
        var target = '#' + gridId + 'ListTable';
        var multiselect = getBooleanValue($(target).getGridParam('multiselect'));
        var targetSection = '#' + componentId + 'EditFormContent';
        var isDetail = isDetailOperation(detailFilter);
		var selectedRow = "";
		var selectedRow = "";
        if (multiselect) {
            //selectedRow = $(target).getGridParam("selarrrow");
            //selectedRow = selectedRow.join(':');
			selectedRow = $(target).getGridParam('selrow');
        } else {
            selectedRow = $(target).getGridParam('selrow');
        }
		if (selectedRow == null || selectedRow == "" ) {
            //calll method
           // newEntity(urlAction,scope, entity, browseId, container, headerType, detailFilter);
           showAlertSelectItemDialog();
            return;
        }else{
			//Edit
			getEntity(urlEditAction, scope, entity, selectedRow, browseId, container, headerType, detailFilter);
		}
	};

    var filterEntityOriginal = function(scope, entity, browseId) {
        var componentId = scope + browseId;
        var filterTextField = $('#' + componentId + 'FilterForm input[type="text"][name=' + componentId + 'filterValue]');
        var countOfFilter = filterTextField.length;
        if( countOfFilter>1)
        {   var tempo = countOfFilter -1;
            for(var i=0;i<tempo;i++)
            {
                var parentFieldShort = filterTextField[i].parentNode;
                var parentLineShort=parentFieldShort.parentNode;
                var parentFieldSet=parentLineShort.parentNode;
                var parentForm=parentFieldSet.parentNode;
                var parentDialog=parentForm.parentNode;
                var parentDialogFilter = parentDialog.parentNode;
                parentDialogFilter.removeChild(parentDialog);
            }
        }
        var filterTextFieldOnly = $('#' + componentId + 'FilterForm input[type="text"][name=' + componentId + 'filterValue]').get(0);
        $('#' + componentId + 'FilterDialog').dialog('open');
        filterTextFieldOnly.focus();
    };
    
    var divShowCalendar= function(idDiv){
    var $divID=$('<div id="'+idDiv+'"/>');
        //$('#mainContent').append($divID);
        $('#'+idDiv).dialog({ autoOpen: false ,height: 450,width: 500, title: 'Show Calendar', resizable: false });
        $('#'+idDiv).dialog('open');
        $('#'+idDiv).datepicker({
            showWeek: true,
		    firstDay: 1,
		    changeMonth: true,
			changeYear: true,
			showOtherMonths: true,
			selectOtherMonths: true,
            showButtonPanel: true
			

		});
        
        

    };
    
    var divShowCalendarEvent= function(idDiv){
          var $divID=$('<div id="'+idDiv+'"/>');
        $('#mainContent').append($divID);
        $('#'+idDiv).dialog({ autoOpen: false ,height: 450,width: 500, title: 'Show Calendar', resizable: false });
        
         var url = '<%= Url.Action("Index", "CalendarEvent") %>';
         alert(url);
        $('#'+idDiv).load(url);
        $('#'+idDiv).dialog('open');

    };
    
    var cancelEntity = function(scope, entity, browseId, isDetail) {
        var componentId =  scope + entity;
        var gridId = scope + browseId;
        var target = '#' + componentId + 'EditFormContent';
        isDetail = getBooleanValue(isDetail);
        
        $('#' + gridId + 'ListTable').resetSelection();
        $(target).empty();
        
        if (isDetail) {
            $(target).fadeOut('slow');
            $('#' + componentId + 'DetailDataListContent').fadeIn('slow');
        } else {
            eval('hideSubtabs(' + entity + 'Subtabs);'); 
        }
    };
    
    var loadDetailList = function(urlAction, id, scope, detailType, target, detailFilter) {

        var isDetail = isDetailOperation(detailFilter);
        
        if (isDetail && (id == null)) {
            showAlertSelectItemDialog();
            return;
        }
        
        $(target).empty();
       
        showLoadingDialogForSection(target);
        $(target).load(urlAction + '/' + id, {Scope: scope, Container: target, DetailType: detailType, IsDetail: isDetail, DetailFilter: detailFilter}, function() {hideLoadingDialogForSection(target); });
    };
    
    var isDetailOperation = function(detailFilter) {
        var isDetail = false;
        
        if ((detailFilter != null) && (detailFilter.length > 0)) {
            isDetail = true;
        }
        
        return isDetail;
    };
    
    var successMessage = function(scope, entity) {
        var componentId = scope + entity;
        
        $("#SuccessMessage").prependTo("#" + componentId + "EditFormContent").fadeIn("slow");
        
        setTimeout("$('#SuccessMessage').fadeOut('slow')", 5000);
    };
    
    var failureMessage = function(scope, entity) {
        /*
        $(target).text(message).fadeIn("slow");
        
        setTimeout("$('" + target + "').fadeOut('slow')", 5000);
        */
    };
    
/*JIQ*/
    function getHtmlError(message) 
    {
        return '<ul class="validation-summary-errors"><li>' + message + '</li></ul>';
    }
    function loadImageUrl(url) {
        if (url != '') 
        {
            if (url.indexOf("./") == 0) {
                    var myDomain = "http://" + location.hostname + ":" + location.port;
                    window.open(url, "Image", "width=640,height=384")
            }
            else
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;}
            window.open(url, "Image", "width=640,height=384")
            
            
        }
    };
    
    function getIdBySelectedRow(scope,browseid){
        var selectedRow = $('#'+scope+browseid+'ListTable').getGridParam('selrow');
        return selectedRow;
    };
    
    var getEntityToRefresh = function(urlAction, scope, entity, selectedRow, browseId, container) {
        //alert(selectedRow + ' - ' + projectHeaderId);
        if (selectedRow != null && selectedRow != ''){
            getEntity(urlAction, scope, entity, selectedRow, browseId, container);
        } else if (projectHeaderId != null && projectHeaderId != '') {
            getEntity(urlAction, scope, entity, projectHeaderId, browseId, container);
        }
    };
    
    var changeStatusOfProject = function(browse,scope,container,urlAction, selectedItems){ 
        var gridId = '#' + browse + 'ListTable';      
        var parameters = {ProjectIds:selectedItems,Scope:scope,Container:container};
        $.post(urlAction, parameters);
        reloadGrid(gridId);
    };
    
    var RedirectToApprove = function(urlAction, selectedRow){
         $.get(urlAction + "?ProjectId=" + selectedRow );
         window.location.href= urlAction + '?ProjectId=' + selectedRow; 
//         browsePopup = window.open(urlAction + '?ProjectId=' + selectedRow , "ApprovalProjectPopup", "width=800,height=730");
//            if (window.focus) {
//                browsePopup.focus();
//               // hideLoadingDialogForSection('#ReportsModuleContent');
//            };
    };
    
    function showEntityData(id,path){
        var xmlhttp;
        var parameters = {id: id};
        var entitypath = path;
        //if (path == 'undefined'){
        var url = entitypath + '/Plan.aspx/GetDataEntity/'+ id;
            $.get(
            url,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;                    
                    var data = results.split("_");
              var mainTab = data[2];
              if(mainTab == "Environment"){
               eval("BackEndTabs.setActiveTab('AdminTabs_" + mainTab + "Tab');");
              }
              if($('#gview_' + data[2] + data[0] + 'AllListTable').length <= 0){
              eval(mainTab + "Subtabs.setActiveTab('" + data[2] + "Tab_" + data[0] + "Content');");
              }
              //mainTab + Subtabs.setActiveTab("'" + data[2] + "Tab_" + data[0] + "Content'");
              showEntity(data[3] + '/' + data[0] + '.aspx/Edit', data[2], data[0], data[1], data[0] +'All', '#'+ data[0]+'Content');
                }
            });            
        
    }
    
    
    function searchProduct(url,id,path)
    {
        setTimeout("reloadGridForProduct('" + url + "','" + id + "','" + path + "')",1500);
       
    }
    
    function reloadGridForProduct(url,id,path)
    {
    var xmlhttp;
        var results = null;
        url = url + '/' + id;
        $.get(
            url,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;                    
                    var gridId = '#EnvironmentProductAllListTable';
                var urlAction = path + '/Browse.aspx/GetData?bid=ProductAll&eou'
                var query = '&searchCriteria=' + results;
                $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid'); 
                var idrow = "'#" + id +"'";
                if ($('#' + id))
                {
                    setTimeout("hightlightRow(" + id + ")",3500);
                }
                }
            });        
    }
    
    function searchKit(url,id,path)
    {
        setTimeout("reloadGridForKit('" + url + "','" + id + "','" + path + "')",1500);
    }
    
    function reloadGridForKit(url,id,path)
    {
    var xmlhttp;
        var results = null;
        url = url + '/' + id;
        $.get(
            url,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;                    
                    var gridId = '#EnvironmentKitAllListTable';
                    var urlAction = path + '/Browse.aspx/GetData?bid=KitAll&eou'
                    var query = '&searchCriteria=' + results;
                    $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid'); 
                    var idrow = "'#" + id +"'";
                    if ($('#' + id))
                    {
                        setTimeout("hightlightRow(" + id + ")",3500);
                    }
                }
            });        
    }
    
    function searchEntityWithTab(url,id,path,entity,tab)
    {
        setTimeout("reloadGridForEntityWithTa('" + url + "','" + id + "','" + path + "','" + entity + "','" + tab + "')",1500);
    }
    
    function reloadGridForEntityWithTa(url,id,path,entity,tab)
    {
    var xmlhttp;
        var results = null;
        url = url + '/' + id;
        $.get(
            url,
            null,
            function(data) {
                if (data != null && data != '') {
                    results = data;                    
                    var gridId = '#'+ tab + entity + 'AllListTable';
                    var urlAction = path + '/Browse.aspx/GetData?bid='+entity+'All&eou'
                    var query = '&searchCriteria=' + results;
                    $(gridId).setGridParam({ url: urlAction + query, page: 1 }).trigger('reloadGrid'); 
                    var idrow = "'#" + id +"'";
                    if ($('#' + id))
                    {
                        setTimeout("hightlightRow(" + id + ")",3500);
                    }
                }
            });       
    }
    
    function hightlightRow(id){
                var idRow = 'tr#' + id;
                var idCheck = '#jqg_' + id;
                if($(idRow))
                {
                    $(idRow).addClass("ui-state-highlight").prop("aria-selected", "true");
                    //document.getElementById(id).className += " ui-state-highlight";
                    $(idCheck).prop('checked', true);
                }
    }
    
    
    var reloadOtherGrid = function(scope, entity, browseid) {
        reloadGrid('#' + scope + entity + browseid+'ListTable');
    };
    
    var SetAndCleanActiveTab  = function(adminTab, entityTab){
        if(adminTab == 'AdminTabs_WorkspaceTab'){
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            if(WorkspaceSubtabs.activeTab == null){
                WorkspaceSubtabs.setActiveTab(entityTab);
            }
            else{
                var idTabWorkspace = WorkspaceSubtabs.activeTab.id.toString();
                if(idTabWorkspace == entityTab){
                    WorkspaceSubtabs.activeTab = null;
                    WorkspaceSubtabs.setActiveTab(entityTab);
                }
                else {
                    WorkspaceSubtabs.setActiveTab(entityTab);
                }
            }
        }
        else if(adminTab == 'AdminTabs_EnvironmentTab'){
            BackEndTabs.setActiveTab('AdminTabs_EnvironmentTab');
            if(EnvironmentSubtabs.activeTab == null){
                EnvironmentSubtabs.setActiveTab(entityTab);
            }
            else{
                var idTabEnvironment = EnvironmentSubtabs.activeTab.id.toString();
                if(idTabEnvironment == entityTab){
                    EnvironmentSubtabs.activeTab = null;
                    EnvironmentSubtabs.setActiveTab(entityTab);
                }
                else {
                    EnvironmentSubtabs.setActiveTab(entityTab);
                }
            }
            }
        else if(adminTab == 'AdminTabs_ResearchTab'){
            BackEndTabs.setActiveTab('AdminTabs_ResearchTab');
            if(ResearchSubtabs.activeTab == null){
                ResearchSubtabs.setActiveTab(entityTab);
            }
            else{
                var idTabResearch = ResearchSubtabs.activeTab.id.toString();
                if(idTabResearch == entityTab){
                    ResearchSubtabs.activeTab = null;
                    ResearchSubtabs.setActiveTab(entityTab);
                }
                else {
                    ResearchSubtabs.setActiveTab(entityTab);
                }
            }
        }
        else if(adminTab == 'AdminTabs_AdminTab'){
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            if(AdminSubtabs.activeTab == null){
                AdminSubtabs.setActiveTab(entityTab);
            }
            else{
                var idTabAdmin = AdminSubtabs.activeTab.id.toString();
                if(idTabAdmin == entityTab){
                    AdminSubtabs.activeTab = null;
                    AdminSubtabs.setActiveTab(entityTab);
                }
                else {
                    AdminSubtabs.setActiveTab(entityTab);
                }
            }
        }
        else if(adminTab == 'AdminTabs_ToolsTab'){
            BackEndTabs.setActiveTab('AdminTabs_ToolsTab');
            if(ToolsSubtabs.activeTab == null){
                ToolsSubtabs.setActiveTab(entityTab);
            }
            else{
                var idTabAdmin = ToolsSubtabs.activeTab.id.toString();
                if(idTabAdmin == entityTab){
                    ToolsSubtabs.activeTab = null;
                    ToolsSubtabs.setActiveTab(entityTab);
                }
                else {
                    ToolsSubtabs.setActiveTab(entityTab);
                }
            }
        }
    };
    
    var GetListPositioning = function(){
        alert('YHGTY');
    };
    
    var CleanOtherFields = function() {
        var dataSOToShow = $('#SO');
        var dataWOToShow = $('#WO');
        var dataSTToShow = $('#ST');
        var dataWTToShow = $('#WT');
        dataSOToShow[0].innerHTML = "";
        dataWOToShow[0].innerHTML = "";
        dataSTToShow[0].innerHTML = "";
        dataWTToShow[0].innerHTML = "";
    };
    
    var GoToEntity = function(entityid, entity, option, urlaction) {
        var scope='';
        if (entity == 'DEAL' || entity == 'EVENT' || entity == 'PROJT' || entity == 'KIT' || entity == 'OBJTV') {
            scope = 'Workspace';
            BackEndTabs.setActiveTab('AdminTabs_WorkspaceTab');
            if (WorkspaceSubtabs.activeTab == null) {
                WorkspaceSubtabs.setActiveTab('WorkspaceTab_'+option+'Content');
            }
            else {
                var idTab = WorkspaceSubtabs.activeTab.id.toString();
                if (idTab == 'WorkspaceTab_'+option+'Content') {
                    WorkspaceSubtabs.activeTab = null;
                    WorkspaceSubtabs.setActiveTab('WorkspaceTab_'+option+'Content');
                }
                else {
                    WorkspaceSubtabs.setActiveTab('WorkspaceTab_'+option+'Content');
                }
            }
        }
           
        else if (entity == 'TREND') {
            scope = 'Tools';
            BackEndTabs.setActiveTab('AdminTabs_ToolsTab');
            if (ToolsSubtabs.activeTab == null) {
                ToolsSubtabs.setActiveTab('ToolsTab_'+option+'Content');
            }
            else {
                var idTab = ToolsSubtabs.activeTab.id.toString();
                if (idTab == 'ToolsTab_'+option+'Content') {
                    ToolsSubtabs.activeTab = null;
                    ToolsSubtabs.setActiveTab('ToolsTab_'+option+'Content');
                }
                else {
                    ToolsSubtabs.setActiveTab('ToolsTab_'+option+'Content');
                }
            }
           }
            else if (entity == 'WEBSITE') {
            scope = 'Admin';
            BackEndTabs.setActiveTab('AdminTabs_AdminTab');
            if (AdminSubtabs.activeTab == null) {
                AdminSubtabs.setActiveTab('AdminTab_'+option+'Content');
            }
            else {
                var idTab = AdminSubtabs.activeTab.id.toString();
                if (idTab == 'AdminTab_'+option+'Content') {
                    AdminSubtabs.activeTab = null;
                    AdminSubtabs.setActiveTab('AdminTab_'+option+'Content');
                }
                else {
                    AdminSubtabs.setActiveTab('AdminTab_'+option+'Content');
                }
            } 
        }
        if(scope !=''){
            var browseid = option + 'All';
            var container = '#' + option + 'Content';
            showEntity(urlaction, scope, option, entityid, browseid, container, '', '');
        }
    };
    
    var displayNote = function(urlAction, ids){
        var validateId = ids.replace(/\D/g,'');        
        if(validateId.length != 0){
            var substr = ids.split(',');
            var note = "";  
            var NewDialog = $("#DialogNote");
            NewDialog.dialog({
                modal: true,
                title: "Account action note for " + substr[1],
                show: 'clip',
                hide: 'clip',
                height: "auto",
                width: "auto",
                close: function(){
                    sendParamsToHistory(urlAction, $("#note").val(), substr[0]);
                    $("#note").val("");
                },
                buttons: [
                    {text: "Save", click: function() {$(this).dialog("close")}},
                    {text: "Cancel", click: function() {$(this).dialog("close")}}
                ]
            }); 
        }       
    }
    var sendParamsToHistory = function(urlAction, note, ids){
        $.ajax({
            url: urlAction,
            type: 'POST',
            data: {Note: note, Ids: ids},
            success: function(Data) {

          }
        });
    }
    
    var showHistoryResult = function(id){ 
        var NewDialog = $('<div id="showHistory">\</div>');
        NewDialog.dialog({
            modal: true,
            title: "History",
            show: 'clip',
            hide: 'clip',
            height: "auto",
            width: 700,
            buttons: [                
                {text: "Ok", click: function() {$(this).dialog("close")}}
            ]
        });
        
       NewDialog.load('User.aspx/GetHistory', {Id: id}, function() {
        $(this).dialog("open");
        hideLoadingDialog();
    });
       
    }
    function resizeGrid(gridId) {
        $('#' + gridId + 'ListTable').jqGrid('setGridWidth', Math.round($(window).width() * 0.98) + '');
    };
    
    function CreateCalendar(Url) {
    browsePopup = window.open(Url, "BrowsePopup", "resizable=NO, location=NO, width=935,height=700, scrollbars=YES");       
        if (window.focus) {
        browsePopup.focus();
    }

}

    function setLinkTarget(divId){
         $('#'+divId+' a').attr('target', '_blank');
    };
    
    var RemoveSubTabOfTabPanel = function(tabPanelId, subTabId) {
    var tabPanelT = Ext.getCmp(tabPanelId);
    var subTabToRemove = Ext.getCmp(subTabId);
    tabPanelT.remove(subTabToRemove.id,true);
};
var resizeImageOfItem = function(id, idToHidde) {
        var imgeT = $('#'+id)[0];
        if (imgeT != undefined && imgeT != null && (imgeT.width > 100 || imgeT.height > 100)) {
            if (imgeT.width > imgeT.height) {
                if (imgeT.width > 100) {
                    $('#'+id).css('width', 100 + 'px');
                }
            } else {
                if (imgeT.height > 100) {
                    $('#'+id).css('height', 100 + 'px');
                }
            }
            $('#'+id).css('display', 'inline');
            $('#'+idToHidde).css('display', 'none');
        }
    };
    var resizeImageToSuccess = function(id, idToHidde) {
        setTimeout(function() { resizeImageOfItem(id, idToHidde); }, 600);
    };
