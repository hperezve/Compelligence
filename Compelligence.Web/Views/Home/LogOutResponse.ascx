<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
        $(function() {
            $.blockUI(
                { message: $('#LogOutSessionDialog'), 
                  css: { width: '20%', margin: 'auto' } 
            });
            
            setTimeout('logOutSession()', 3000); 
            
        });
        
        var logOutSession = function () {
            location.href = '<%= Url.Action("Index", "Home") %>';
        };
</script>

<div id="LogOutSessionDialog" class="displayNone">
    <p>
        <span class="loadingDialog">Your session has finished ...</span>
    </p>
</div>
