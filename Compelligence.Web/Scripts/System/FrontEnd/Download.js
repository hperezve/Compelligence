
var downloadFile = function(urlDownloadAction) {
    $.get(urlDownloadAction, { chk: 'true' }, function(data) {

        if (data.toString().indexOf("NotFound") > -1) {
            $('#DownloadFileNotFound').dialog("open");
        }
       
        if (data.toString().indexOf("Found") > -1) {
           
            $('#DownloadFileSection').prop("src", urlDownloadAction);
        }

        else {
            //$('#DownloadFileSection').attr("src", urlDownloadAction);
            window.open(data.toString(), "Compelligence");
            //window.location = urlDownloadAction;
        }
    });

    return false;
};