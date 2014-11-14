(function(){
//Section 1 : Code to execute when the toolbar button is pressed
var des_cont= {
    exec:function(editor){
//        loadNewsLetterPopup('', 'NewsLetterSection', 'WorkspaceNewsletterEditFormOpeningText', 'cocoa');
        loadNewsLetterAddItemDlg('', 'NewsLetterSection', 'WorkspaceNewsletterEditFormOpeningText', 'cocoa');
 	}
},

//Section 2 : Create the button and add the functionality to it
cont = 'items';
CKEDITOR.plugins.add(cont,{
	init:function(editor){
//	editor.addCommand(cont,des_cont);
//	editor.ui.addButton("items",{
//	label: 'Items',
//	icon: this.path + "AddItems.jpg",
//	command: cont
//	//command: cont + 'width:80px; background-repeat: no-repeat; margin-left: 5px; margin-top: 5px;"'
//	//command: 'style="width:80px; background-image:url(../Scripts/CKeditor/plugins/items/AddItems.jpg); background-repeat: no-repeat; margin-left: 5px; margin-top: 5px;'
//	
//			});
		}
	}); 
})();

