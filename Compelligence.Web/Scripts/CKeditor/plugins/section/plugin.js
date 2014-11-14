//var global = 0;
var extract_num = 0;
(function() {
    //Section 1 : Code to execute when the toolbar button is pressed
    var des_cont = {
        exec: function(editor) {

            var theSelectedText = editor.getSelection().getNative();
            str = '' + theSelectedText;
            str = str.replace(/# /g, '</li><li>');

            str = '' + theSelectedText;
            str = str.replace(/\* /g, '</li><li>');
            
            var nuevo = editor.getSelection().getNative();
            var FormattedText = '<sections style="background-color:#eef7ff;">' + nuevo + '</sections>';

            editor.insertHtml(FormattedText);
        }
    },

    //Section 2 : Create the button and add the functionality to it
cont = 'section';
    CKEDITOR.plugins.add(cont, {
        init: function(editor) {
            editor.addCommand(cont, des_cont);
            editor.ui.addButton("section", {
                label: 'Section',
                icon: this.path + "AddSection.jpg",
                command: cont   
            }             
            );
        }
    });
})();
