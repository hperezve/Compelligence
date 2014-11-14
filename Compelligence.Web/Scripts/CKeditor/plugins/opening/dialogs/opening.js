/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/
    function addSections() {
	alert('secciones');
        var SectionPanel = $("#SectionPanel");
        var newSectionPanel= SectionPanel.clone();
        newSectionPanel.attr("style", "display:inline");
        newSectionPanel.addClass("SectionPanel");

        var newSectionName = "SectionPanel_" + ($(".SectionPanel").size() + 1);
        newSectionPanel.attr("id", newSectionName);
        
        var newSectionButton = newSectionPanel.find("#btnAdd");        //alert(newSectionButton.size());
        newSectionButton.attr("OnClick", "loadPopup('','NewsLetterSection','" + newSectionName + "','coco');");

        var newSectionRemove = newSectionPanel.find("#btnRemove");        
        newSectionRemove.attr("OnClick", "removeSection('"+newSectionName+"');");
        
        SectionPanel.parent().append(newSectionPanel);

    };
    
    function removeSection(sectionName) 
    {  var section = $('#'+sectionName);
       section.remove();

	};
CKEDITOR.dialog.add( 'opening', function( editor )
{
	

	return {
		title : 'Opening Dialog',
		minWidth : 390,
		minHeight : 230,
		contents : [
			{
				id : 'OpeningDialog',
				label : '',
				title : '',
				expand : true,
				padding : 0,
				elements :
				[
					{
						type : 'html',
						html :
						'<div class="field">'+
                   '<label for="WorkspaceNewsletterEditFormOpeningText" class="cke_dialog_ui_labeled_label">Opening Text:</label>'+
                    '<p>'+
                     '<textarea rows="2" name="OpeningText" id="WorkspaceNewsletterEditFormOpeningText" cols="20" class="textClass"></textarea>'+
                   '</p>'+
                '</div>'+
				'<div> <input type="button" value="Add Section" onclick="javascript: addSections();"> </div>'+
				
				'<div id="SectionPanelGroup" style="width:100%;float:left">'+
     '<div id="SectionPanel" style="width:100%;display:none">'+
         '<div id="SectionTitle" style="width:85%;float:left">'+
            '<label>Section :</label>'+
            '<input type="text" id="txtTitle" name="txtTitle" style=" border: none;width:580px"/>'+
            '<label>Description :</label><textarea id="txtDescription" name="txtDescription" class="nsDescription"></textarea>'+
         '</div>'+
         '<div  style="width:15%;float:left">'+
            '<input type="button" id="btnAdd"    onclick="javascript: void(0)" value="+" title="Add Project/Library" /> <br/>'+
            '<input type="button" id="btnRemove" onclick="javascript: void(0)" value="-" title="Remove this Section" /> '+
         '</div>'+
         '<div id="SectionItem">'+
            
            '<div class="newsletterhidden" style="margin-left:100px;float:left">'+
              '<label>Item :</label><input type="text" id="txtItem" name="txtItem" style="width:390px"/> '+
              '<textarea id="txtItemComment" name="txtItemComment" rows=10 cols=20></textarea>'+
            '</div>'+
            '<br />'+
         '</div>'+
    '</div>'
					}
				]
			}
		],
		buttons : [ CKEDITOR.dialog.cancelButton ]
	};
} );
