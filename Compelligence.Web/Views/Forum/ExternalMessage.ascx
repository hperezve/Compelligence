<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<style type="text/css">
    body {
    font-family: sans-serif;
    font-size: 12px;
    font-weight: bold;
    }
</style>
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>   
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>"></script>
<script type="text/javascript">
    $(document).ready(function() {
        var entityId = '<%= ViewData["IdComments"] %>';
        if (entityId != null) {
            window.parent.$("#ImgComents" + entityId).removeClass("ImageCommentsN");
            window.parent.$("#ImgComents" + entityId).addClass("ImageCommentsY"); ;
            //for update Image icon when exist two ids in a same body as in comparinator result.ascx
            if (window.parent.$('img[name=ImgComents' + entityId + ']').length > 0) {
                window.parent.$('img[name=ImgComents' + entityId + ']').removeClass("ImageCommentsN");
                window.parent.$('img[name=ImgComents' + entityId + ']').addClass("ImageCommentsY");
            }            
        }
    });
</script>
<script type="text/javascript">
    $(document).ready(function() {
        parent.$.unblockUI();
    });
</script>
<br /><br />
<br /><br />
<br /><br />
<div style="vertical-align:middle;width:100%">
  <p style="text-align:center">Your comment was added.</p>
</div>