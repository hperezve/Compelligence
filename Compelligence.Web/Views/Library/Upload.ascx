<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
<style type="text/css">
    .ufbutton
    {height:22px;
     width  :70px;
    }
</style>
<script type="text/javascript">
        $(document).ready(function() {
            $('#clickToClose').click(
                function() {
                    window.parent.$("#modalDiv").dialog('close');
                    return false;
            });
            // uncomment and use the below line close when document is ready (no click action by user needed)
            // the same call could be put elsewhere depending on desired functionality (after successful form submit, etc.)
            // window.parent.$("#modalDiv").dialog('close');
        });
        function UploadSingleFile(frmUploadFile) {
            $('#barUploading').css({display:'block'});
            $(frmUploadFile).submit();
            return true;
        }
    </script>
    
    <fieldset>
        <legend>Select file</legend>
        <% using (Html.BeginForm("UploadSingleFile", "Library", FormMethod.Post, new
           {
               enctype = "multipart/form-data",
               id="UploadSingleFile"
           }))
           { %>
        <input id="Id" type="hidden" name="Id" value="<%=(decimal)ViewData["Id"]%>" />
        <input id="file" name="file" type="file" size=55 onchange="UploadSingleFile('#UploadSingleFile');"/>
        <input type="submit" value="Upload" class="ufbutton" />
        <button id="clickToClose" class="ufbutton">Close</button> 
        <%} %>
        <div id="barUploading" style="display:none"><img src='<%= Url.Content("~/Content/Images/Styles/uploading.gif")%>' alt=''/></div>
    </fieldset>    
    

