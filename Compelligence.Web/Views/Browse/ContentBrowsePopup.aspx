﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MasterPages/BackEndBlockingPopupSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="popupHead" ContentPlaceHolderID="Head" runat="server">
    <title>Coontent - Portal</title>
</asp:Content>
<asp:Content ID="popupStyles" ContentPlaceHolderID="Styles" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/jquery.searchFilter.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.jqgrid.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jqgrid/steel/ui.multiselect.css") %>" rel="stylesheet"  type="text/css" />    
</asp:Content>
<asp:Content ID="popupScripts" ContentPlaceHolderID="Scripts" runat="server">


    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>"
        type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>"
        type="text/javascript"></script>

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.locale-en-min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.base-min.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.celledit-min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.common-min.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.custom-min.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.formedit-min.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.import-min.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.inlinedit-min.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.jqueryui.js") %>" type="text/javascript"></script>
            
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.addons.js") %>" type="text/javascript"></script>        
        
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.postext.js") %>" type="text/javascript"></script>        
    <%--<script src="<%= Url.Content("~/Scripts/jqgrid/grid.postext-min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.setcolumns.js") %>" type="text/javascript"></script>
    <%--<script src="<%= Url.Content("~/Scripts/jqgrid/grid.setcolumns-min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.subgrid-min.js") %>" type="text/javascript"></script>    
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.tbltogrid-min.js") %>" type="text/javascript"></script>    
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/grid.treegrid-min.js") %>" type="text/javascript"></script>    
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/jqDnR.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/jqModal.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/jqModal.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.fmatter-min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/jquery.searchFilter.js") %>" type="text/javascript"></script>
    
    <script src="<%= Url.Content("~/Scripts/jqgrid/JsonXml-min.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jqgrid/ui.multiselect.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Utility.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/DataGrid.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/System/BackEnd/Form.js") %>" type="text/javascript"></script>

<style type ="text/css">
body
{
background-color:Silver;
}

/* TABLE
 * ========================================================================= */
table {
  /* border: 1px solid #888; */
  border-collapse: collapse;
  line-height: 1;
  /*margin: 1em auto;*/
  width: 90%;
}

table span {
  background-position: center left;
  background-repeat: no-repeat;
  padding: .1em 0 .1em 1.2em;
}
</style>
</asp:Content>
<asp:Content ID="popupContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="AlertSelectItemDialog" title="Alert Message">
        <p>
            <span class="ui-icon ui-icon-alert alertSelectItemDialog"></span>You should select
            a valid item from list.
        </p>
    </div>
    <div id="content"></div>
</asp:Content>
