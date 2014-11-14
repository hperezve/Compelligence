<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<!--.filetree span.folder-->
<link href="<%= Url.Content("~/Content/Styles/jquery.treeview.css") %>" rel="stylesheet" type="text/css" />

<div id="TreeViewList">
   <ul id="TreeviewMenu" class="filetree">
   <div class="headertree">
    	<span id="headertree">Navigation</span>
	</div>
	   <%= Html.BuildTreeDivs() %>
	</ul>	
</div>