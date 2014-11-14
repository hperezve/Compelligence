<%@ Page Title="WhitePaper" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">

<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
<%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
<%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
<script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

<style type="text/css">
    #HeaderEdit
	{
		display:none;		
	}
</style>
  
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" >
            
    <div id="Edit">
        <% Html.RenderPartial("Edit"); %>    
    </div>
    
    <div id="List">
        <% Html.RenderPartial("List"); %>            
    </div>            
    
</asp:Content>



