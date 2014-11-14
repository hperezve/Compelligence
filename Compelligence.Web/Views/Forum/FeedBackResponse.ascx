<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script src="<%= Url.Content("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script> 
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>"></script>
<script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>"></script>

<% string listOfItemsAttachment = ViewData["ListOfItemsAttached"].ToString();
   string listOfItemsNoAttachment = ViewData["ListOfItemsNoAttached"].ToString();  %>
<script type="text/javascript">
    $(document).ready(function() 
    {
      parent.$.unblockUI();
    });
</script>
<style type="text/css">
    body {
    font-family: sans-serif;
    font-size: 12px;
    font-weight: bold;
    }
</style>
<% if (string.IsNullOrEmpty(listOfItemsAttachment) && string.IsNullOrEmpty(listOfItemsNoAttachment))
   {%>
<br /><br />
<br /><br />
<% }%>
<div style="vertical-align:middle;width:100%">
  <p style="text-align:center">Your feedback was submitted!</p>
</div><br /><br />
<% if (!string.IsNullOrEmpty(listOfItemsAttachment)) {
       %>
       The following files were successfully uploaded:
       <ul>
       <%
       string[] namesOfItems = listOfItemsAttachment.Split(':');
       foreach (string name in namesOfItems)
       { 
       %>
       <li><%= name %></li>
       <%
       }
      %>
      </ul>
       <%
   }
     %>

<% if (!string.IsNullOrEmpty(listOfItemsNoAttachment))
   {
       %>
       The following files was not successfully uploaded:
       <ul>
       <%
           string[] namesOfItems = listOfItemsNoAttachment.Split(':');
       foreach (string name in namesOfItems)
       { 
       %>
       <li>The file  <%= name %> was not successfully uploaded. Please re-submit</li>
       <%
       }
      %>
      </ul>
       <%
   }
     %>