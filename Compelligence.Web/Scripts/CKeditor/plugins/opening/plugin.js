(function() {
    //Section 1 : Code to execute when the toolbar button is pressed
    var des_cont = {
        exec: function(editor) {
            var theSelectedText = editor.getSelection().getNative();
            var textAll = editor.getData();
            var easy = textAll.indexOf("<opening>");
            if (easy == '-1') {
                var FormattedText = '<opening>' + theSelectedText + '</opening>';
                editor.insertHtml(FormattedText);
            }
            else {
                alert('can only enter a title opening');
            }
        }
    },

    //Section 2 : Create the button and add the functionality to it
cont = 'opening';
    CKEDITOR.plugins.add(cont, {
        init: function(editor) {
            editor.addCommand(cont, des_cont);
            editor.ui.addButton("opening", {
                label: 'opening',
                icon: this.path + "icon-opening.png",
                command: cont
            });
        }
    });
})();

