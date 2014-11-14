<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Download.js") %>" type="text/javascript">    </script>   
<script type="text/javascript">
    $(function() {
        $('#DownloadFileNotFound').dialog({
            bgiframe: true,
            autoOpen: false,
            modal: false,
            buttons: {
                Ok: function() {
                    $(this).dialog('close');
                }
            }
        });
        $('#SuccessSubmitted').dialog({
            bgiframe: true,
            autoOpen: false,
            modal: false,
            buttons: {
                Ok: function() {
                    $(this).dialog('close');
                }
            }
        });
    });
</script>
<div style="display: none;">
    <iframe id="DownloadFileSection" src="javascript: void(0);" frameborder="0" marginheight="0"
        marginwidth="0"></iframe>
</div>
<div id="DownloadFileNotFound" title="Error" style="display:none">
    <p>
        <span class="ui-icon ui-icon-alert"></span><strong>File not found...!</strong>
    </p>
</div>
<div id="SuccessSubmitted" title="Message" style="display:none">
    <p>
        <strong>It was successfully submitted!</strong>
    </p>
</div>
