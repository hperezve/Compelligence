<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/frontendsite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
    <%--Compelligence - Deal Support  --%>
<asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:LabelResource, DealSupportIndexCompelligenceDealSupport %>" />
    </title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/jquery.autocomplete.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/thickbox.css") %>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script> 
<%--    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
<script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.bgiframe.min.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.ajaxQueue.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/thickbox-compressed.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>

    <%--//Filter table--%>

    <script src="<%= Url.Content("~/Scripts/jquery.json.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.cookiejar.pack.js") %>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jquery.tableFilter.js") %>" type="text/javascript"></script>    
    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript">    </script>    
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript">    </script>    
    <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
    <script type="text/javascript">

         $(document).ready(function() {
             $('th').attr('style', 'text-align:left;font-size:medium '); // In IE8 change 'attr' by 'prop' to set styles
             var typeDeal = '<%= ViewData["ShowArchived"] %>';
             if (typeDeal == null || typeDeal == "no" || typeDeal == "") {
                 $("#CurrentDeal").attr("checked", true);
                 $("#ArchivedDeal").attr("checked", false);
             }
             else {
                 $("#CurrentDeal").attr("checked", false);
                 $("#ArchivedDeal").attr("checked", true);
             }
         }
   );
      
   function LoadContent(urlAction, target, fnSuccess) 
   {
       $.post(urlAction, function(data) 
       {
           $('#' + target).html(data);
           if (fnSuccess)
              eval(fnSuccess); 
       });
   }
   function DealSupportDialog(url,dialogTitle,dialogContent) 
   {
      //showLoadingDialog();
      var dealObject = $("#DealSupportDialog");
      dealObject.dialog(
      { autoOpen: false,
          title: dialogTitle,
          buttons: { "Ok": function() {
              $(this).dialog("destroy");
              window.location.reload();
          } 
          }
      }
      );
      //dealObject.load(url);
      dealObject.html(dialogContent);
      dealObject.dialog("open");
      return false;
    }

    function DealSupportClose(urlAction,Id,IdBox)
     {
         //alert('are you close deal.');
         var dealObject = $("#" + IdBox);
         dealObject.dialog(
      { autoOpen: false,
          title: "Please select rating before closing.",
          buttons: { "Ok": function() {
              $.get(urlAction + "?DealId=" + Id, {}, function(data) //Close
              {
                  location.href = '<%= Url.Action("Index","DealSupport") %>';
                  dealObject.dialog("close");
              }
              );
          }
          }
      }
      );
       $.get(urlAction+"Rating?DealId="+Id, {}, function(data) {dealObject.html(data);dealObject.dialog("open"); }); //CloseRating
        
       return false;
         
    } 
      
      function DealEdit(id) 
      {  var urlAction='<%=Url.Action("Edit","DealSupport") %>';
         if(id==null)
           alert('Select row');
         else  
         location.href=urlAction+ "/"+ id.id
      }
      function DealClose(id) 
      {
       DealSupportClose('<%=Url.Action("Close","DealSupport") %>',id.id,'DealSupportDialog');
      }
      
      function DealComment(id)
      {
        var urlAction='<%=Url.Action("Comments","DealSupport") %>';
        location.href=urlAction + '/'+id;
      }
      
      var isCtrl = false; 
      $(document).keyup(function (e) { if(e.which == 17) isCtrl=false; }).keydown(function (e) { if(e.which == 17) isCtrl=true; if(e.which == 83 && isCtrl == true) 
      { //run code for CTRL+S -- ie, save! 
        return false; 
        } });

        var ShowArchived = function(type) {
            if (type == 'ArchivedDeal') {
                var url = '<%=Url.Action("GetArchivedDeals","DealSupport") %>';
                $('#DealFrontEndForm').prop("action", url);
                $('#DealFrontEndForm').submit();
            } else {
                var url = '<%=Url.Action("GetEnabledDeals","DealSupport") %>';
                $('#DealFrontEndForm').prop("action", url);
                $('#DealFrontEndForm').submit();
            }
        };
    </script>

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
   <%--<div class="headerTitleLefConteiner">
    
<b><asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:LabelResource, DealSupportIndexDealSupport %>" />
   </b>   
      </div>--%>
      <%-- <hr />--%>
       <div id="contentBoxTop" class="menuFrontEnd" style="width: 100%">
        <% using (Html.BeginForm(null, null, FormMethod.Post, new { id = "DealFrontEndForm", style = " margin-left: 0;" }))
        { %>
        <div style="padding-left: 11px;">       
            <input type="radio" name="typedeal" id="CurrentDeal" value="no" onclick = "ShowArchived('CurrentDeal');" style = "vertical-align:bottom;" />
             <span style="color:Black;margin-right: 3%;">Show Current Deals</span>
            
            <input type="radio" name="typedeal" id="ArchivedDeal" value="yes" onclick = "ShowArchived('ArchivedDeal');" style = "vertical-align:bottom;" />
            <span style="color:Black;">Show Archived Deals</span>
          </div>      
        <% } %>
    </div>
    <div id="DealContent">
        <%Html.RenderPartial("List"); %>
    </div>
    <div id="DealSupportDialog">    </div>
    <div id="DealSupportCloser" style="display:none">   </div>
    <div id="MessageBox" style="display:none"></div>
</asp:Content>
<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
<% Html.RenderPartial("Options"); %>
</asp:Content>
