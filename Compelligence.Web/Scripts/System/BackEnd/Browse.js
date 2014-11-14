    var browsePopup;
    var popupParameters;
    var parentUrl;
    var parentScope;
    var parentEntity;
    var parentBrowseId;
    var parentIsDetail; 
    var parentHeaderType;
    var parentDetailFilter;

    var parentSectionId;
    
    var addEntity = function(url, scope, entity, browseId, browsePopupId, headerType, detailFilter) {
        var gridId = scope + browseId;
        var isDetail = isDetailOperation(detailFilter);
        
        $('#' + gridId + 'ListTable').resetSelection();
        
        parentUrl = url;
        parentScope = scope;
        parentEntity = entity;
        parentBrowseId = browseId;
        parentIsDetail = isDetail; 
        parentHeaderType = headerType;
        parentDetailFilter = detailFilter;
        
        popupParameters = '?BrowseName=';
        popupParameters += browsePopupId;
        
        blockInterface();
        
        browsePopup = window.open (urlBrowsePopup + popupParameters, "BrowsePopup", "width=700,height=400"); 
        
        if (window.focus) {
            browsePopup.focus();
        }

    };

    var addIndustry = function(url, scope, entity, browseId, browsePopupId, headerType, detailFilter) {
        var gridId = scope + browseId;
        var isDetail = isDetailOperation(detailFilter);

        $('#' + gridId + 'ListTable').resetSelection();
        parentUrl = url;
        parentScope = scope;
        parentEntity = entity;
        parentBrowseId = browseId;
        parentIsDetail = isDetail;
        parentHeaderType = headerType;
        parentDetailFilter = detailFilter;

        popupParameters = '?BrowseName=';
        popupParameters += browsePopupId;
        blockInterface();
        browsePopup = window.open(urlIndustryBrowsePopup + popupParameters, "IndustryBrowsePopup", "width=700,height=450");

        if (window.focus) {
            browsePopup.focus();
        }

    };

    var addEntityToPositioning = function(url, scope, entity, browseId, browseHierarchy, browsePopupId, headerType, detailFilter, entityOption) {
        var gridId = scope + browseId;
        var isDetail = isDetailOperation(detailFilter);

        $('#' + gridId + 'ListTable').resetSelection();

        parentUrl = url;
        parentScope = scope;
        parentEntity = entity;
        parentBrowseId = browseId;
        parentIsDetail = isDetail;
        parentHeaderType = headerType;
        parentDetailFilter = detailFilter;

        popupParameters = '?BrowseName=';
        popupParameters += browsePopupId;
        popupParameters += '&EntityOption='
        popupParameters += entityOption;
        popupParameters += '&Scope='
        popupParameters += scope;
        popupParameters += '&BrowseId='
        popupParameters += browseId;
        popupParameters += '&BrowseHierarchy='
        popupParameters += browseHierarchy;
        blockInterface();
        browsePopup = window.open(urlPositioningBrowsePopup + popupParameters, "PositioningBrowsePopup", "width=700,height=400");

        if (window.focus) {
            browsePopup.focus();
        }

    };


    var loadSelectedPopupItems = function(selectedItems) {
        var gridId = parentScope + parentBrowseId;
        var targetGrid = '#' + gridId + 'ListTable';
        
        loadSelectedDataItems(parentUrl, parentScope, selectedItems, targetGrid, parentIsDetail, parentHeaderType, parentDetailFilter);
    };

    var loadSelectedPopupItemsPositioning = function(selectedItems, entityOption, scope, browseId, browseHierarchy) {
        var gridId = parentScope + parentBrowseId;
        var targetGrid = '#' + gridId + 'ListTable';
        var currentUrl = $(targetGrid).getGridParam("url");
        var lastFilter = GetLastFilterCriteria(currentUrl);
        var lastSearch = GetLastSearchCriteria(currentUrl);
        var query = '';
        var filterValues = lastFilter.split(':');
        if (lastFilter == '') {
            query = '&filterCriteria=';
        }
        else {
            query += lastFilter + ':';
        }
        if (entityOption == 'INDTR') {
            if (currentUrl.indexOf('IndustryId') == -1) {
                query += browseId + 'View.IndustryId_Eq_' + selectedItems;
            }
            else {
                query = '';
                for (var i = 0; i < filterValues.length; i++) {
                    if (filterValues[i].indexOf(browseId + 'View.IndustryId') == -1) {
                        if (query.length > 1) {
                            if (query.charAt(query.length - 1) != ':') {
                                query += ':';
                            }
                        }
                        query += filterValues[i];
                    }
                }
                query += ':' + browseId + 'View.IndustryId_Eq_' + selectedItems;
            }
        }
        else {
            if (currentUrl.indexOf('EntityId') == -1) {
                query += browseId + 'View.EntityId_Eq_' + selectedItems + ':' + browseId + 'View.EntityType_Eq_' + entityOption;
            }
            else {
                query = '';
                for (var j = 0; j < filterValues.length; j++) {
                    if ((filterValues[j].indexOf(browseId + 'View.EntityId') == -1) && (filterValues[j].indexOf(browseId + 'View.EntityType') == -1)) {
                        if (query.length > 1) {
                            if (query.charAt(query.length - 1) != ':') {
                                query += ':';
                            }
                        }
                        query += filterValues[j];
                    }
                }
                query += ':' + browseId + 'View.EntityId_Eq_' + selectedItems + ':' + browseId + 'View.EntityType_Eq_' + entityOption;
            }
        }
        query += lastSearch;
        loadSelectedDataItemsPositioning(parentUrl, parentScope, selectedItems, targetGrid, parentIsDetail, parentHeaderType, parentDetailFilter, query);
        loadSelectedPopupItemsPositioningH(selectedItems, entityOption, scope, browseId, browseHierarchy);
    };

    var loadSelectedPopupItemsPositioningH = function(selectedItems, entityOption, scope, browseId, browseHierarchy) {
        var gridId = parentScope + browseHierarchy;
        var targetGrid = '#' + gridId + 'ListTable';
        var currentUrl = $(targetGrid).getGridParam("url");
        var lastFilter = GetLastFilterCriteria(currentUrl);
        var lastSearch = GetLastSearchCriteria(currentUrl);
        var query = '';
        var filterValues = lastFilter.split(':');
        if (lastFilter == '') {
            query = '&filterCriteria=';
        }
        else {
            query += lastFilter + ':';
        }
        if (entityOption == 'INDTR') {
            if (currentUrl.indexOf('IndustryId') == -1) {
                query += browseHierarchy + 'View.IndustryId_Eq_' + selectedItems;
            }
            else {
                query = '';
                for (var i = 0; i < filterValues.length; i++) {
                    if (filterValues[i].indexOf(browseHierarchy + 'View.IndustryId') == -1) {
                        if (query.length > 1) {
                            if (query.charAt(query.length - 1) != ':') {
                                query += ':';
                            }
                        }
                        query += filterValues[i];
                    }
                }
                query += ':' + browseHierarchy + 'View.IndustryId_Eq_' + selectedItems;
            }
        }
        else {
            if (currentUrl.indexOf('EntityId') == -1) {
                query += browseHierarchy + 'View.EntityId_Eq_' + selectedItems + ':' + browseHierarchy + 'View.EntityType_Eq_' + entityOption;
            }
            else {
                query = '';
                for (var j = 0; j < filterValues.length; j++) {
                    if ((filterValues[j].indexOf(browseHierarchy + 'View.EntityId') == -1) && (filterValues[j].indexOf(browseHierarchy + 'View.EntityType') == -1)) {
                        if (query.length > 1) {
                            if (query.charAt(query.length - 1) != ':') {
                                query += ':';
                            }
                        }
                        query += filterValues[j];
                    }
                }
                query += ':' + browseHierarchy + 'View.EntityId_Eq_' + selectedItems + ':' + browseHierarchy + 'View.EntityType_Eq_' + entityOption;
            }
        }
        query += lastSearch;
        loadSelectedDataItemsPositioning(parentUrl, parentScope, selectedItems, targetGrid, parentIsDetail, parentHeaderType, parentDetailFilter, query);
    };

    var filterPositioningByForType = function(scope, browseId, browseHierarchy, forType) {
        var gridId = scope + browseHierarchy;
        var targetGrid = '#' + gridId + 'ListTable';
        var currentUrl = $(targetGrid).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        var lastFilter = GetLastFilterCriteria(currentUrl);
        var lastSearch = GetLastSearchCriteria(currentUrl);
        var query = '';
        var filterValues = lastFilter.split(':');
        for (var i = 0; i < filterValues.length; i++) {
            if (filterValues[i].indexOf(browseHierarchy + 'View.ForType') == -1) {
                if (query.length > 1) {
                    if (query.charAt(query.length - 1) != ':') {
                        query += ':';
                    }
                }
                query += filterValues[i];
            }
        }
        if (query.length > 0) {
            query += ':';
        }
        else {
            query = '&filterCriteria=';
        }
        query += browseHierarchy + 'View.ForType_Eq_' + forType;
        reloadGridPositioning(targetGrid, urlAction, query);
    };

    var showPositioningMaster = function(invoke, scope, browseId, browseHierarchy) {
    filterPositioningByMaster(scope, browseId, invoke);
    filterPositioningByMaster(scope, browseHierarchy, invoke);
    }

    var filterPositioningByMaster = function(scope, browseId, invoke) {
        var isMasterLevel = 'N';
        
        var gridId = scope + browseId;
        var targetGrid = '#' + gridId + 'ListTable';
        var currentUrl = $(targetGrid).getGridParam("url");
        var urlAction = currentUrl.substring(0, currentUrl.indexOf('&eou') + 4);
        var lastFilter = GetLastFilterCriteria(currentUrl);
        var lastSearch = GetLastSearchCriteria(currentUrl);
        var query = '';
        var filterValues = lastFilter.split(':');
        for (var i = 0; i < filterValues.length; i++) {
            if (filterValues[i].indexOf(browseId + 'View.IsMaster') == -1) {
                if (query.length > 1) {
                    if (query.charAt(query.length - 1) != ':') {
                        query += ':';
                    }
                }
                query += filterValues[i];
            }
        }
        
        if (invoke.checked) {
            if (query.length > 0) {
                query += ':';
            }
            else {
                query = '&filterCriteria=';
            }
            query += browseId + 'View.IsMaster_Eq_Y'; 
            }
        reloadGridPositioning(targetGrid, urlAction, query);
    };
    
    var SetId = function(id, e, entityType) {
    var libraryId = $('#WorkspaceNewsletterEditForm' + e);
    libraryId[0].value = id;
    var type = $('#WorkspaceNewsletterEditFormEntityType');
    type[0].value = entityType;
    };
    var blockInterface = function() {
      $.blockUI({ message: null});   
    };
    
    var unblockInterface = function() {
      $.unblockUI();
  };


  var loadPopup = function(browseId, browsePopupId, targetId, fncallback, pfields) {
      //var gridId = scope + browseId;
      //$('#' + gridId + 'ListTable').resetSelection();
      parentSectionId = targetId;
      popupParameters = '?BrowseName=';
      popupParameters += browsePopupId;
      loadSelectedFunction = fncallback;
      fields = pfields;
      blockInterface();

      browsePopup = window.open(urlBrowsePopupCols + popupParameters, "BrowsePopupCols", "width=700,height=400");

      if (window.focus) {
          browsePopup.focus();
      }

  };

  var loadNewsLetterPopup = function(browseId, browsePopupId, targetId, fncallback, pfields) {
      //var gridId = scope + browseId;
      //$('#' + gridId + 'ListTable').resetSelection();
      parentSectionId = targetId;
      popupParameters = '?BrowseName=';
      popupParameters += browsePopupId;
      loadSelectedFunction = fncallback;
      fields = pfields;
      blockInterface();

      browsePopup = window.open(urlBrowseNewsLetterPopup + popupParameters, "BrowseNewsLetterPopup", "width=700,height=400");

      if (window.focus) {
          browsePopup.focus();
      }

  };

  var loadNewsLetterAddItemDlg = function(browseId, browsePopupId, targetId, fncallback, pfields) {
      parentSectionId = targetId;
      popupParameters = '?BrowseName=';
      popupParameters += browsePopupId;
      loadSelectedFunction = fncallback;
      fields = pfields;
      blockInterface();

      NewsLetterAddItemDlg(urlBrowseNewsLetterPopup + popupParameters);

  };

  var GetLastFilterCriteria = function(currentUrl) {
      var filterCriteria = '';
      var posFilterCriteria = currentUrl.indexOf('&filterCriteria');
      var posSearchCriteria = currentUrl.indexOf('&searchCriteria');
      if (posFilterCriteria != -1) {
          if (posSearchCriteria != -1) {
              if (posFilterCriteria < posSearchCriteria) {
                  filterCriteria = currentUrl.substring(posFilterCriteria, posSearchCriteria);
              }
              else {
                  filterCriteria = currentUrl.substring(posFilterCriteria);
              }
          }
          else {
              filterCriteria = currentUrl.substring(posFilterCriteria);
          }
      }
      return filterCriteria;
  };

  var GetLastSearchCriteria = function(currentUrl) {
      var searchCriteria = '';
      var posFilterCriteria = currentUrl.indexOf('&filterCriteria');
      var posSearchCriteria = currentUrl.indexOf('&searchCriteria');
      if (posSearchCriteria != -1) {
          if (posFilterCriteria != -1) {
              if (posSearchCriteria < posFilterCriteria) {
                  searchCriteria = currentUrl.substring(posSearchCriteria, posFilterCriteria);
              }
              else {
                  searchCriteria = currentUrl.substring(posSearchCriteria);
              }
          }
          else {
              searchCriteria = currentUrl.substring(posSearchCriteria);
          }
      }
      return searchCriteria;
  };

  var showPositioningByHierarchy = function(invoke, scope, browseId, browseHierarchy, fortype) {
      if (invoke.checked) {
          $('#gridPositioningListOverflow').css("display", "none");
          $('#gridPositioningHierarchyOverflow').css("display", "block");
          $('#PositioningButtonA').css("display", "none");
          $('#PositioningButtonB').css("display", "block");
          filterPositioningByForType(scope, browseId, browseHierarchy, fortype);
          if (fortype == 'CTIV') {
              $('#' + scope + 'CompetitiveHierarchyCheckbox').prop('checked', false); ;
          }
          else if (fortype == 'PSTN') {
          $('#' + scope + 'PositioningHierarchyCheckbox').prop('checked', false); ;
          }
      }
      else {
          $('#gridPositioningListOverflow').css("display", "block");
          $('#gridPositioningHierarchyOverflow').css("display", "none");
          $('#PositioningButtonB').css("display", "none");
          $('#PositioningButtonA').css("display", "block");
      }
  };
  var reziseAfterToggleColumn = function(scope, browseId) {
      var gridId = scope + browseId;
      var target = gridId + 'ListTable';
      var gviewBrowse = 'gview_' + gridId + 'ListTable';
      var gboxBrowse = 'gbox_' + gridId + 'ListTable';
      var gviewPager = gridId + 'ListPager';
      var widthOfList = $('#' + target).width();
      if (widthOfList != 3000) {
          //$('#' + target + ' > div').width(1316);
          $('#' + gboxBrowse + ' > div').width(3000);
          $('#' + gviewBrowse + ' > div').width(3000);
          $('#' + gviewPager + ' > div').width(3000);
          $('#' + gboxBrowse).width(3000);
      }
  }; 
  //not yet optimized.
  function Export(urlAction, scope, all) {
      if (!all) {
          var id = $('#' + scope + 'AnswerDetailListTable').getGridParam('selrow');
          if (id) {

              //var reg = $('#WorkspaceSurveyAnswerDetailListTable').getRowData(id);
              window.location = urlAction + "?QuizResponseId=" + id;
          }
          else {
              showAlertSelectItemDialog();
          }
      }
      else {
          var id = $('#' + scope + 'AllListTable').getGridParam('selrow');
          if (id) {
              window.location = urlAction + "?QuizId="+id;
          }
      }
  }
  function ExportBySelected(urlAction, scope, browseId, all) {
      if (!all) {
          var gridId = scope + browseId;
          var target = '#' + gridId + 'ListTable';
          var multiselect = getBooleanValue($(target).getGridParam('multiselect'));
          var selectedRow = "";
          if (multiselect) {
              selectedRow = $(target).getGridParam("selarrrow");
              selectedRow = selectedRow.join(':');
          } else {
              selectedRow = $(target).getGridParam('selrow');
          }
          
         // var id = $('#' + scope + 'AnswerDetailListTable').getGridParam('selrow');

          if (selectedRow != null && selectedRow != undefined && selectedRow !='') {

              //var reg = $('#WorkspaceSurveyAnswerDetailListTable').getRowData(id);
              window.location = urlAction + "?QuizResponseId=" + selectedRow;
          }
          else {
              showAlertSelectItemDialog();
          }
      }
      else {
          var id = $('#' + scope + 'AllListTable').getGridParam('selrow');
          if (id) {
              window.location = urlAction + "?QuizId=" + id;
          }
      }
  }