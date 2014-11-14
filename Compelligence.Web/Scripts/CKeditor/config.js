/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/


CKEDITOR.editorConfig = function(config) {
    // Define changes to default configuration here. For example:
    config.language = 'en';
    // config.uiColor = '#AADC6E';
    config.width = 1000;
    config.height = 250;    
    config.resize_enabled = true;
  config.toolbar = 'MyToolbar';
 
    config.toolbar_MyToolbar =    [['Source','-','NewPage','Preview'],
	['Cut','Copy','Paste','PasteText','PasteFromWord'],
	['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat', '-', 'Link', 'Unlink', '-', 'Maximize'],
	['TextColor', 'BGColor'],
	['BidiLtr', 'BidiRtl'],
	['Subscript', 'Superscript'],
	['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'],
	'/',
	['Bold','Italic','Underline','Strike'],
	['NumberedList','BulletedList','-','Outdent','Indent','Blockquote'],
	['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
	['Styles','Format','Font','FontSize'],	
	['items']];
    config.extraPlugins = 'items';
};
