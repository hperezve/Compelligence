<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
        var opSt = '<%= ViewData["OperationStatus"] %>';
        
        if (opSt == '<%= OperationStatus.Successful %>') {
            setTimeout("$.blockUI({ message: $('#SuccessUpdateMessage'), fadeIn: 700, fadeOut: 700, timeout: 3000," +
                            "showOverlay: false, centerY: false, css: { width: '300px', top: '10px', left: '', right: '10px', " +
                                "border: 'none', padding: '5px', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', opacity: 0.6, color: '#fff' } });", 1000);
        }
    });
</script>

<div id="SuccessUpdateMessage" class="ntfMsg" style="display: none;">
    <h2>
        Operation was successful</h2>
</div>
