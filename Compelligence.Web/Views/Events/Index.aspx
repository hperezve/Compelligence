<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Compelligence - Events</title>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
 <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
 <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />
 <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

 <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery-validate/lib/jquery.form.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery-validate/jquery.validate.min.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/System/BackEnd/Dialogs.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/System/FrontEnd/FeedBack.js") %>" type="text/javascript"></script>
 <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
 <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Rating.js") %>" type="text/javascript">    </script>
 <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Feedback.js") %>" type="text/javascript">  </script>
 <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Comments.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Messages.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery.multiselect.js") %>" type="text/javascript"></script>
 <script src="<%= Url.Content("~/Scripts/jquery.multiselect.filter.js") %>" type="text/javascript"></script>
 <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.css") %>" rel="stylesheet" type="text/css" />
 <link href="<%= Url.Content("~/Content/Styles/jquery.multiselect.filter.css") %>" rel="stylesheet" type="text/css" />
 <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
 <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>   
    <%--<%Session.Timeout = 1;%>
    <script type="text/javascript">
        //it's for test session expired
        $(function() {
            setTimeout(function() {            alert('debug:session expired');        }, '<%=Session.Timeout*60*1000%>');
        });
    </script>        --%>

 <script type="text/javascript">
     $(function() {
         $('#txtStartDate').datepicker({ showButtonPanel: true, closeText: 'Clear',
             beforeShow: function(input) {
                 setTimeout(function() {
                     var clearButton = $(input)
            .datepicker("widget")
            .find(".ui-datepicker-close");
                     clearButton.unbind("click").bind("click", function() { $.datepicker._clearDate(input); });
                 }, 1);
             }
         });
    

         $("#Industry").multiselect({
             multiple: false,
             header: 'Select <%=ViewData["IndustryLabel"]%>',
             noneSelectedText: 'Select <%=ViewData["IndustryLabel"]%>',
             selectedList: 1,
             classes: "auto fixed"
         }).multiselectfilter();

         $("#Competitor").multiselect({
             multiple: false,
             header: 'Select <%=ViewData["CompetitorLabel"]%>',
             noneSelectedText: 'Select <%=ViewData["CompetitorLabel"]%>',
             selectedList: 1,
             classes: "auto fixed"
         }).multiselectfilter();
         $('input[name="multiselect_Industry"]').click(function(Ind) {
             showLoadingDialog();
             var currentOption = $('#Industry').val();
             var var_name = $("input[name=multiselect_Industry]:checked").val();
             selectIndustry = var_name;
             if (currentOption == var_name) {
                 var_name = "";
             }
             $('#Industry').val(var_name);
             var url = '<%= Url.Action("ChangeIndustry", "Events") %>';
             ChangeAction(url);
         })


         $('input[name="multiselect_Competitor"]').click(function(Comp) {
             showLoadingDialog();
             var currentOptionCompetitor = $('#Competitor').val();
             var verificar = $("input[name=multiselect_Competitor]:checked").val();
             if (currentOptionCompetitor == verificar) {
                 verificar = "";
             }
             $('#Competitor').val(verificar);
             //$("#Competitor").append('<option value="">New option</option>');
             var url = '<%= Url.Action("ChangeCompetitor", "Events") %>';
             ChangeAction(url);
         })

     });
     
     $(function() {
     $('th').attr('style', 'text-align:left;font-size:medium; '); // In IE8 change 'attr' by 'prop' to set styles
         var typeDeal = '<%= ViewData["ShowAll"] %>';
         if (typeDeal == null || typeDeal == "no" || typeDeal == "") {
             $("#LastEvents").attr("checked", true);
             $("#PastEvents").attr("checked", false);
         }
         else {
             $("#LastEvents").attr("checked", false);
             $("#PastEvents").attr("checked", true);
         }
     }
   );
     function LoadContent(urlAction, target, fnSuccess) {
         $.post(urlAction, function(data) {
             $('#' + target).html(data);
             if (fnSuccess)
                 eval(fnSuccess);
         });
     }
     function GetParameterList() {
         //var parameter = '';
         var parameter = 'new {';
         var page = '<%=ViewData["page"]%>';
         var group = '<%=ViewData["group"]%>';
         var order = '<%=ViewData["order"]%>';
         var asc = '<%=ViewData["asc"]%>';
         if (page == null || page == '') { page = '0'; }
         //         parameter = '?page=' + page;
         parameter = parameter+ 'page=' + page;
         if (order == null || order == '') { order = 'Name'; }
         parameter = parameter + ', order=' + order;
         if (group != null || group != '') { parameter = parameter + ',group=' + group; }
         if (asc != null || asc != '') { parameter = parameter + ',asc=' + asc; }
         parameter = parameter + '}';
         return parameter;
     };
     function EventEdit(id) {
         var urlAction = '<%=Url.Action("Edit","Events") %>';
         if (id == null)
             alert('Please select a row');
         else
             location.href = urlAction + "/" + id.id
     }
     function EventClose(id) {
         var urlAction = '<%=Url.Action("Close","Events") %>';
         if (id == null)
             alert('Please select a row');
         else
             location.href = urlAction + "/" + id.id
     }

     function EventFeedBack(id) {
         var urlAction = '<%= Url.Action("FeedBackMessage","Forum")%>';
         ExternalFeedBackWithAttachedDlg(urlAction + '?EntityId=' + id + '&EntityType=EVENT&SubmittedVia=Event', 'FeedBack Dialog');
     }

     function EventComment(id) {
         var urlAction = '<%=Url.Action("Comments","Events") %>';
         location.href = urlAction + '/' + id;
     }

     function ChangeAction(url) {
         //showLoadingDialog();
         $('#EventFrontEndForm').prop("action", url);
         $('#EventFrontEndForm').submit();
     }

     function ShowAllEvents(type) {
         if (type == "PastEvents") {
             var url = '<%=Url.Action("GetAllEvents","Events") %>';
             $('#EventFrontEndForm').prop("action", url);
             $('#EventFrontEndForm').submit();
         } else {

         var url = '<%=Url.Action("GetActualEvents","Events") %>';
         $('#EventFrontEndForm').prop("action", url);
             $('#EventFrontEndForm').submit();
         }
     }

     function ShowFilteredEvents() {
         var date = $('#txtStartDate').val();
         var showAll = "";
         if ($('#PastEvents').prop('checked')) {
             showAll = "yes";
         } else {
             showAll = "no";
         }
         //var param = GetParameterList();

         //alert(param);

         
         //if (param != null && param != '') {
        //     url = '<%=Url.Action("GetEventsByDate","Events", "' + param + '")%>';
         //}
         var parameters = { dateFilter: date, showAll: showAll };
         var url = '<%=Url.Action("GetEventsByDate","Events")%>';
         //$.post(url, parameters);
         $('#EventFrontEndForm').prop("action", url);
         $('#EventFrontEndForm').submit();
     };
     function ExternalFeedBackWithAttachedDlg(url, title) {
         var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
         ExternalFeedBackWithAttachedVDlg(url, title, urlValidate);
     };
     function ExternalResponseNewDlg(url) {
         var urlValidate = '<%= Url.Action("ValidateFile", "Forum")%>';
         ExternalResponseNewVDlg(url, urlValidate);
     };
 </script>
<style type="text/css">
   .ui-multiselect-menu ul li label span  {
                 padding-left: 3px;
    }
    
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <%--<div class="headerTitleLefConteiner">
   
      <b>  <asp:Literal ID="Literal1"  runat="server" Text="<%$ Resources:LabelResource, EventsIndexEvents %>" />
    </b>
    </div>--%> 
   
    <div id="contentBoxTop" class="menuFrontEnd" style="width: 100%">
        <% using (Html.BeginForm(null, null, FormMethod.Post, new { id = "EventFrontEndForm" }))
        { %>
        <div>
            <div style="width:80%;float:left">
            <%=Html.Hidden("page", ViewData["page"])%>
            <%=Html.Hidden("group", ViewData["group"])%>
            <%=Html.Hidden("order", ViewData["order"])%>
            <%=Html.Hidden("asc", ViewData["asc"])%>
            <%=Html.DropDownList("Industry",(SelectList)ViewData    ["IndustryCollection"] , new { onchange = "ChangeAction('" + Url.Action("ChangeIndustry", "Events") + "')" })%>
            
            <%= Html.DropDownList("Competitor", (SelectList)ViewData["CompetitorCollection"], new { onchange = "ChangeAction('" + Url.Action("ChangeCompetitor", "Events") + "')" })%> &nbsp;&nbsp;&nbsp;
            
            <%if (ViewData["dateFilter"] == null || ViewData["dateFilter"].Equals(""))
              { %>
            &nbsp;<span style="color:Black;">
           
            <%--Start Date  --%>
                <asp:Literal ID="Literal4"  runat="server" Text="<%$ Resources:LabelResource, EventsIndexStartDate %>" />
            </span><input id="txtStartDate" name="txtStartDate" onchange="ShowFilteredEvents()" type="text" style="width:85px;" class="inputSearch" />            
            <%}
              else
            { %>
            &nbsp;<span style="color:Black;">
                <%--Start Date  --%>
                <asp:Literal ID="Literal5"  runat="server" Text="<%$ Resources:LabelResource, EventsIndexStartDate %>" />
            </span><input id="txtStartDate" name="txtStartDate" onchange="ShowFilteredEvents()" value="<%=ViewData["dateFilter"]%>" type="text" style="width:85px;" class="inputSearch"/>            
            <%} %>
            </div>
                <div style="float: left;width: 20%;">
                    <input type="radio" name="lstevent" id="LastEvents" value="no" onclick = "ShowAllEvents('LastEvents');" style = "vertical-align:bottom;" />
                     <span style="color:Black;margin-right: 10%;">Show Upcoming Events </span>
                    <br />
                    <input type="radio" name="lstevent" id="PastEvents" value="yes" onclick = "ShowAllEvents('PastEvents');" style = "vertical-align:bottom;" />
                    <span style="color:Black;">Show Past Events</span>
                </div>
            </div>
            <br />
            <br />
            <br />
        <% } %>
    </div>
    <div id="EventListContent">
        <%Html.RenderPartial("List"); %>
    </div>
    <div id="EventDialog">
    </div>
</asp:Content>

<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
    <div id="FormComments">
    </div>
    <div id="FormQuiz">
    </div>
    <div id="FormFeedBack">
    </div>
    <div id="PrivateCommentNewDlg"></div>
<% Html.RenderPartial("Options"); %>
</asp:Content>