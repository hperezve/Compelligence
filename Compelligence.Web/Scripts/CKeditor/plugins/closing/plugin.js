(function() {
    //Section 1 : Code to execute when the toolbar button is pressed
    var des_cont = {
        exec: function(editor) {
            var theSelectedText = editor.getSelection().getNative();
            var textAll = editor.getData();
            var easy = textAll.indexOf("<closing>");
            if (easy == '-1') {
                var FormattedText = '<closing>' + theSelectedText + '</closing>';
                editor.insertHtml(FormattedText);
            }
            else {
                alert('can only enter a title closing');
            }

        }
    },

    //Section 2 : Create the button and add the functionality to it
cont = 'closing';
    CKEDITOR.plugins.add(cont, {
        init: function(editor) {
            editor.addCommand(cont, des_cont);
            editor.ui.addButton("closing", {
                label: 'closing',
                icon: this.path + "icon-closing.png",
                command: cont
            });
        }
    });
})();

