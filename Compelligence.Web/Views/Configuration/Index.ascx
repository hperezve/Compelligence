<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(function() {
    var urlAction = '<%= Url.Action("Configuration","Configuration") %>';
       
       $.get(urlAction,null, function(data){$("#ConfigurationModuleContent").html(data);});
   });
</script>

<div id="ConfigurationModuleContent">

</div>

