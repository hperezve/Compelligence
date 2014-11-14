<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
        var opSt = '<%= ViewData["OperationStatus"] %>';
        
        if (opSt == '<%= OperationStatus.Successful %>') {
//            setTimeout("$.blockUI({ message: $('#SuccessUpdateMessage'), fadeIn: 700, fadeOut: 700, timeout: 3000," +
//                            "showOverlay: false, centerY: true, css: { width: '300px', top: '0px', left: '', right: '10px', " +
            //                                "border: 'none', backgroundColor: '#000', '-webkit-border-radius': '10px', '-moz-border-radius': '10px', 'font-size': '0.8em', 'height': '40px', opacity: 0.6, color: '#fff' } });", 1000);
            $('#SuccessSubmitted').dialog("open");
        }
    });
</script>
<%--
<div id="SuccessUpdateMessage" class="ntfMsg" style="display: none;padding">
    <h2>
        It was successfully submitted!</h2>
</div>--%>
