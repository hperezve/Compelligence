/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.dialog.add( 'closing', function( editor )
{
	

	return {
		title : 'Closing Dialog',
		minWidth : 390,
		minHeight : 230,
		contents : [
			{
				id : 'ClosingDialog',
				label : '',
				title : '',
				expand : true,
				padding : 0,
				elements :
				[
					{
						type : 'html',
						html :
						'<div style="width: 85%; float: left;" id="ClosingTitle">'+
							'<p><label class="cke_dialog_ui_labeled_label cke_required">Closing :</label>'+
							'<textarea class="textClass" name="txtDescriptionClosing" 					id="txtDescriptionClosing"></textarea></p>'+
						'</div>'
					}
				]
			}
		
		],
				buttons : [ CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton ],
				onOk : function() {
					// "this" is now a CKEDITOR.dialog object.
					// Accessing dialog elements:
					var textareaObj = this.getContentElement( 'ClosingDialog', 'txtDescriptionClosing' );
					alert( "You have entered: " + textareaObj.getValue() );
				}
		
	};
} );
