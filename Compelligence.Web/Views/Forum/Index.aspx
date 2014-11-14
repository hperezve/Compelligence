<%@ Page Title="Compelligence - Comments" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>
<%@ Import Namespace="Compelligence.Common.Utility.Parser" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
    <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet"  type="text/css" />--%>
    <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet"  type="text/css" />
    <link href="<%= Url.Content("~/Content/Styles/tinytable.style.css") %>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.css") %>" />
    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Feedback.js") %>" type="text/javascript">  </script>
    <script type="text/javascript" src="<%= Url.Content("~/Content/HtmlEditor/jquery.cleditor.js") %>"></script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

    <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
    
    <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>

    <script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>

    <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>

    <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
//        var test = function(v) {
//            var object = $("#subject");
//            var coment = object[0].value;
//            if (coment.length > 30) {
//                alert('texto demasiado grande');
//                object[0].value = '';
//            }
//            else {
//                ValidateFormComment(v);
//            }
//        };

        $(function() {
            $("#tableDiscussionDeal").treeTable({ initialState: "expanded", clickableNodeNames: true });
        });

        $(document).ready(function() {

            $(".tab_content").hide(); //Hide all content
            $("ul.tabs li:first").addClass("active").show(); //Activate first tab
            $(".tab_content:first").show(); //Show first tab content
            //On Click Event
            $("ul.tabs li").click(function() {
                $("ul.tabs li").removeClass("active"); //Remove any "active" class
                $(this).addClass("active"); //Add "active" class to selected tab
                $(".tab_content").hide(); //Hide all tab content
                var activeTab = $(this).find("a").attr("href"); //Find the rel attribute value to identify the active tab + content
                $(".tab_container").css('height', '600px');
                $(activeTab).fadeIn('slow', function() { $(".tab_container").css('height', 'auto'); }); //Fade in the active content
                return false;
            });
        });

        function ValidateFormComment(form) {
            if (form.response.value == "") {
                return false;
            }
            return true;            
        }
        function RemoveFormComment() {
            var source = $("#FormComment:first");
            source.remove();
        }
        function AddFormComment(id) {
            $("#ParentResponseId").val(id);
            var source = $("#FormComment").clone();
            var target = $("#" + id);
            source.append("<input type='button' value='Cancel' class='shortButton' onclick='RemoveFormComment()' />");
            target.append(source);
            $("#FormComment:first").children('#response').focus();
        };
        function EventComment(id) {
            var urlAction = '<%=Url.Action("Comments","Events") %>';
            location.href = urlAction + '/' + id;
        }

        function DealComment(id) {
            var urlAction = '<%=Url.Action("Comments","DealSupport") %>';
            location.href = urlAction + '/' + id;
        }

        function ProjectComment(id) {
            var urlAction = '<%=Url.Action("Comments","Project") %>';
            location.href = urlAction + '/' + id;
        }
        function ProductComment(id) {
            var urlAction = '<%=Url.Action("Comments","Product") %>';
            location.href = urlAction + '/' + id;
        }
        function vacio(q) {
            for (i = 0; i < q.length; i++) {
                if (q.charAt(i) != " ") {
                    return true
                }
            }
            return false
        }        
        //validate that the field isen't empty and not haven't spaces empty
        function valida(F) {
            var message = '';
            if (vacio(F.subject.value) == false) {
                //alert("Insert Text.")
                $("#subject").addClass('validation-error-items');
                message = '<li>Thread is required</li>';
                $(".error").html('<ul class="validation-summary-errors validation-summary-errors-forum">' + message + '</ul>');
                return false
            } else {
                
            }

        };
    </script>

<style type="text/css">
    h1
    {
        font-size: 3em;
        margin: 20px 0;
    }
    .container
    {
        width: 100%;
        margin: 10px auto;
    }
 
     html ul.tabs li.active, html ul.tabs li.active a:hover
    {
        background: #fff;
        border-bottom: 1px solid #fff;
        -moz-border-radius: 5px 5px 0 0;
        -webkit-border-radius: 5px 5px 0 0;
    }
    .validation-summary-errors-forum
{
    margin-left: 0;
   /* margin-right: 0;    */
}
.validation-error-items
{
    /*border-color:#FF0000;    */
    border :1px solid #FF0000;
}       
</style>
</asp:Content>
<asp:Content ID="FrontEndComments" ContentPlaceHolderID="MainContent" runat="server">
    <div id="FrontEndComments">
  <%--      <div class="headerTitleLefConteiner">
            <%--Threads  
            <label for="LblThreads">
                <b>
                    <asp:Literal ID="LtThreads" runat="server" Text="<%$ Resources:LabelResource, ForumIndexThreads %>" />
                </b>
            </label>
        </div>--%>
        <br />
        <% Html.RenderPartial("FrontEndDownloadSection"); %>
        <fieldset class="WithoutRightMargin">
            <asp:Panel ID="OldComments" runat="server">
                <table class="sortable">
                    <colgroup>
                        <col width="640px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col width="160px" />
                    </colgroup>
                    <thead style="font-weight: bold">
                    <%--Thead--%>
                        <td style="background-color: rgb(233, 234, 236);">
                            <label for="LblThread">
                                <asp:Literal ID="LtThread" runat="server" Text="<%$ Resources:LabelResource, ForumIndexThread %>" />
                            </label>
                        </td>
                        <%--Created By--%>
                        <td style="background-color: rgb(233, 234, 236);">
                            <label for="LblCreatedby">
                                <asp:Literal ID="LtCreatedby" runat="server" Text="<%$ Resources:LabelResource, ForumIndexCreatedby %>" />
                            </label>
                        </tdstyle="background-color: rgb(233, 234, 236);">
                        <%--Created Date  --%>
                        <td style="background-color: rgb(233, 234, 236);">
                            <label for="LblCreatedDate">
                                <asp:Literal ID="LtCreatedDate" runat="server" Text="<%$ Resources:LabelResource, ForumIndexCreatedDate %>" />
                            </label>
                        </td>
                        <%--Last Post Date  --%>
                        <td style="background-color: rgb(233, 234, 236);">
                            <label for="LblCreatedDate">
                                <asp:Literal ID="LtLasChangedDate" runat="server" Text="<%$ Resources:LabelResource, ForumIndexLastChangedDate %>" />
                            </label>
                        </td>
                        
                        
                        <%--  <td>
     <div class="float-right" style="width:55px; _width:55px;">            
                <a id="testfeedbackicon" href="javascript: void(0);" onclick="ComentsForm('<%= Url.Action("RegisterFollower", "ContentPortal") %>','Coments');">
                    <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private comment"
                        alt="FeedBack" />
                </a>  
            </div>
          
          </td>--%>
          <td align="center" style="background-color: rgb(233, 234, 236);">
          Follower
          </td>
        </thead>
        <%IList<Forum> forums = (IList<Forum>)ViewData["Forums"];
          UserProfile user = (UserProfile)ViewData["UserProfileInSession"];
          foreach (Forum f in forums)
          {
              //f.Subject = ExpressionParser.GetExpression(GeneralStringParser.ShorterSufix(f.Subject.ToString(), 60));
              f.Subject = GeneralStringParser.ShorterSufix(f.Subject.ToString(), 60);
              //if (f.Subject=="")
              //{
              //    return;
              //}
            %>
          <tr>
            <td><a href="<%=Url.Action("IndexDetail","Forum",new {Id=f.Id}) %>"><%=f.Subject %></a></td>
            <td> <%=f.CreatedBy %></td>
            <td> <%=f.CreatedDate %></td>
            <td> <%=f.LastChangedDate %></td>
            <td align="center">
                 <a id="testfeedbackicon" href="javascript: void(0);" onclick="FollowerForm('<%= Url.Action("RegisterFollower", "Forum")  + "/" + f.Id %>','<%= f.Id %>' ,'<%= user.Email %>','<%= user.Name %>','Follower');">
                    <img src="<%=Url.Content("~/Content/Images/Icons/followers.jpeg") %>" width="22px" title="Setting Follower"
                        alt="Comment" />
                </a>  
            </td>
          </tr>
          <%} %>
        </table>
         
        </asp:Panel>
    </fieldset>
    <fieldset class="WithoutRightMargin">
        <legend>
                             <%-- Add thread --%>
	        <label for="LblAddthread">
		        <asp:Literal ID="LtAddthread" runat="server" Text="<%$ Resources:LabelResource, ForumIndexAddthread %>" />
	        </label>
        
        </legend>
        <% using (Html.BeginForm("NewForum", "Forum", FormMethod.Post, new
           {
               id = "FormComment",
               enctype = "multipart/form-data",
               onsubmit = "return valida(this);"
               //return ValidateFormComment(this)

           }))
               { %>
            <%= Html.Hidden("Id",default(decimal)) %>
            <div class="error" style="color: red;"></div>
            <textarea id="subject" name="subject" cols="72" rows="5"></textarea>
            <input type="submit" value="Send" class="shortButton" />
            <%} %>
        </fieldset>
        <div id="CommentForm">
        </div>
    </div>
    <h2>
        <%-- All Comments--%>
        <label for="LblAllComments">
            <asp:Literal ID="LtAllComments" runat="server" Text="<%$ Resources:LabelResource, ForumIndexAllComments %>" />
        </label>
    </h2>
    <hr />
    <br />
    <ul class="tabs">
        <li><a href="#tab1">
            <asp:Literal ID="LtProjects" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjects %>" />
        </a></li>
        <li><a href="#tab2">
            <asp:Literal ID="LtDeals" runat="server" Text="<%$ Resources:LabelResource, ForumIndexDeals %>" />
        </a></li>
        <li><a href="#tab3">
            <asp:Literal ID="LtEvents" runat="server" Text="<%$ Resources:LabelResource, ForumIndexEvents%>" />
        </a></li>
        <li><a href="#tab4">
            <asp:Literal ID="LtProducts" runat="server" Text="" /><%=ViewData["ProductLabel"]%>
        </a></li>
    </ul>
    <div class="tab_container">
        <div id="tab1" class="tab_content">
            <%---------------------------------------------------Begin of projects -------------------------------------- --%>
            <table class="sortable" style="font: 11px Verdana,Arial; table-layout: fixed">
                <colgroup>
                    <col width="180px" /> <!--200-->
                    <col width="70px" /> <!--100-->
                    <col width="100px" /> <!--100-->
                    <col width="100px" /> <!--100-->
                    <col width="80px" /> <!--50-->
                    <col width="50px" /> <!--200-->
                    <col width="50px" /> <!--200-->
                </colgroup>
                <thead style="font-weight: bold">
                    <td style="background-color: rgb(233, 234, 236);>
                        <%--Project Name  --%>
                        <label for="LblProjectName">
                            <asp:Literal ID="LtProjectName" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjectName %>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);>
                        <%--Created by--%>
                        <label for="LblProjectCreatedby">
                            <asp:Literal ID="LtProjectCreatedby" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjectCreatedby %>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);>
                        <%--Created Date--%>
                        <label for="LblProjectCreatedDate">
                            <asp:Literal ID="LtProjectCreatedDate" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjectCreatedDate %>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);>
                        <%--Last Post Date--%>
                        <label for="LblProjectLastPostDate">
                            <asp:Literal ID="LtProjectLastPostDate" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjectLastPostDate %>" />
                        </label>
                    </td>
                    <td align ="center" style="background-color: rgb(233, 234, 236); >
                        <%--Number of Replies--%>
                        <label for="LblProjectNumberofReplies">
                            <asp:Literal ID="LtProjectNumberofReplies" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjectNumberofReplies %>" />
                        </label>
                    </td>
                    <td align ="center" style="background-color: rgb(233, 234, 236); >
                        <%--Views--%>
                        <label for="LblProjectViews">
                            <asp:Literal ID="LtProjectViews" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjectViews %>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);>
                        <%--Comments--%>
                        <label for="LblProjectComments">
                            <asp:Literal ID="LtProjectComments" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProjectComments %>" />
                        </label>
                    </td>
                </thead>
                <%IList<Project> projects = (IList<Project>)ViewData["ProjectsComments"];%>
                 <% foreach (Project p in projects)
                  {
                      if (p.RepliesNumber > 0)
                      { %>
                <tr>
                    <%}
                          else
                          { %>
                    <tr>
                        <%} %>
                        <td class="textcut">
                            <a href="javascript:void(0)" onclick="ProjectComment('<%= p.Id%>')">
                                <%=p.Name %></a>
                        </td>
                        <td>
                            <%=p.CreatedBy %>
                        </td>
                        <td>
                            <%=p.CreatedDate %>
                        </td>
                        <td>
                            <%=p.LastPostDate %>
                        </td>
                        <td class="CenterTD">
                            <%=p.RepliesNumber %>
                        </td>
                        <td class="CenterTD">
                            <%=p.ViewsNumber %>
                        </td>
                        <td align ="center" >
                     
                            <a href="javascript:void(0)" onclick="ProjectComment('<%= p.Id%>')">
                                <!--<img width="22px" title="Add comment" src="/Content/Images/Icons/testfeedback.gif" alt ="Coment"> -->
                                
                                 <!--choose an icon for each case - if a comment exists or not (Cristian)-->
                                 <% if (p.RepliesNumber == 0)
                                    { %>
                                      <img class="ImageCommentsN" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                                 <% } else {%> 
                                    <img class="ImageCommentsY" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                                 <% }%> 
                            </a>
                     
                        </td>
                    </tr>
                    <%} %>
            </table>
            <%---------------------------------------------------end of projects -------------------------------------- --%>
        </div>
        <div id="tab2" class="tab_content">
            <%---------------------------------------------------Begin of Deals-------------------------------------- --%>
            <table class="sortable" style="font: 11px Verdana,Arial; table-layout: fixed">
                <colgroup>
                    <col width="200px" />
                    <col width="150px" />
                    <col width="150px" />
                    <col width="85px" />
                </colgroup>
                <thead style="font-weight: bold">
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Deal Name--%>
                        <label for="LblDealName">
                            <asp:Literal ID="LtDealName" runat="server" Text="<%$ Resources:LabelResource, ForumIndexDealName %>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Created by--%>
                        <label for="LblDealCreatedby">
                            <asp:Literal ID="LtDealCreatedby" runat="server" Text="<%$ Resources:LabelResource, ForumIndexDealCreatedby %>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Created Date--%>
                        <label for="LblDealCreatedDate">
                            <asp:Literal ID="LtDealCreatedDate" runat="server" Text="<%$ Resources:LabelResource, ForumIndexDealCreatedDate%>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Comments--%>
                        <label for="LblDealComments">
                            <asp:Literal ID="LtDealComments" runat="server" Text="<%$ Resources:LabelResource, ForumIndexDealComments%>" />
                        </label>
                    </td>
                </thead>
                <%IList<Deal> deals = (IList<Deal>)ViewData["DealsComments"];
                  foreach (Deal d in deals)
                  {
                %>
                <tr>
                    <td class="textcut">
                        <a href="javascript:void(0)" onclick="DealComment('<%= d.Id%>')">
                            <%=d.Name %></a>
                    </td>
                    <td>
                        <%=d.CreatedBy %>
                    </td>
                    <td>
                        <%=d.CreatedDate %>
                    </td>
                    <td class="CenterTD">
                        <a href="javascript:void(0)" onclick="DealComment('<%= d.Id%>')">
                            <% if (!d.CheckComments)
                                    { %>
                                      <img class="ImageCommentsN" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                                 <% } else {%> 
                                    <img class="ImageCommentsY" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                                 <% }%> 
                                 
                            </a>
                    </td>
                </tr>
                <%} %>
            </table>
            <%---------------------------------------------------End of Deals-------------------------------------- --%>
        </div>
        <div id="tab3" class="tab_content">
            <%---------------------------------------------------Begin of Events-------------------------------------- --%>
            <table class="sortable" style="font: 11px Verdana,Arial; table-layout: fixed">
                <colgroup>
                    <col width="200px" />
                    <col width="150px" />
                    <col width="150px" />
                    <col width="85px" />
                </colgroup>
                <thead style="font-weight: bold">
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Event Name--%>
                        <label for="LblCommentsEventName">
                            <asp:Literal ID="LtEventName" runat="server" Text="<%$ Resources:LabelResource, ForumIndexEventName%>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Created by--%>
                        <label for="LblEventCreatedby">
                            <asp:Literal ID="LtEventCreatedby" runat="server" Text="<%$ Resources:LabelResource, ForumIndexEventCreatedby%>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Created Date--%>
                        <label for="LblEventCreatedDate">
                            <asp:Literal ID="LtEventCreatedDate" runat="server" Text="<%$ Resources:LabelResource, ForumIndexEventCreatedDate%>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <%--Comments--%>
                        <label for="LblEventComments">
                            <asp:Literal ID="LtEventComments" runat="server" Text="<%$ Resources:LabelResource, ForumIndexEventComments%>" />
                        </label>
                    </td>
                </thead>
                <%IList<Event> events = (IList<Event>)ViewData["EventsComments"];
                  foreach (Event e in events)
                  {
                %>
                <tr>
                    <td class="textcut">
                        <a href="javascript:void(0)" onclick="EventComment('<%= e.Id%>')">
                            <%=e.EventName %></a>
                    </td>
                    <td>
                        <%=e.CreatedBy %>
                    </td>
                    <td>
                        <%=e.CreatedDate %>
                    </td>
                    <td style="text-align: center">
                        <a href="javascript:void(0)" onclick="EventComment('<%= e.Id%>')">
                           <% if (!e.Comment.Equals("true"))
                                    { %>
                                      <img class="ImageCommentsN" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                           <% } else {%> 
                                    <img class="ImageCommentsY" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                           <% }%> 
                           
                            </a>
                    </td>
                </tr>
                <%} %>
            </table>
            <%---------------------------------------------------End of Events-------------------------------------- --%>
        </div>
        <div id="tab4" class="tab_content">
            <table class="sortable" style="font: 11px Verdana,Arial; table-layout: fixed">
                <colgroup>
                    <col width="200px" />
                    <col width="150px" />
                    <col width="150px" />
                    <col width="85px" />
                </colgroup>
                <thead style="font-weight: bold">
                    <td style="background-color: rgb(233, 234, 236);">
                        <label>
                            <asp:Literal ID="LtProductName" runat="server" Text="" /><%=ViewData["ProductLabel"]%>
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <label>
                            <asp:Literal ID="LtProductCreatedBy" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProductCreatedBy%>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <label>
                            <asp:Literal ID="LtProductCreatedDate" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProductCreatedDate%>" />
                        </label>
                    </td>
                    <td style="background-color: rgb(233, 234, 236);">
                        <label>
                            <asp:Literal ID="LtProductComments" runat="server" Text="<%$ Resources:LabelResource, ForumIndexProductComments%>" />
                        </label>
                    </td>
                </thead>
                <% IList<Product> products = (IList<Product>)ViewData["ProductsComments"];
                   foreach (Product product in products)
                   { %>
                   <tr>
                    <td class="textcut">
                        <a href="javascript:void(0)" onclick="ProductComment('<%= product.Id%>')">
                        <%= product.Name %>
                        </a>
                    </td>
                    <td>
                        <%= product.CreatedBy %>
                    </td>
                    <td>
                        <%= product.CreatedDate %>
                    </td>
                    <td style="text-align: center">
                        <a href="javascript:void(0)" onclick="ProductComment('<%= product.Id%>')">
                           <% if (!product.HasComment)
                                    { %>
                                      <img class="ImageCommentsN" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                           <% } else {%> 
                                    <img class="ImageCommentsY" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" style="margin:20px 0 0 0;" title="Add comment" />
                           <% }%>
                        </a>
                    </td>
                   </tr>
                   
                    <%} %>
            </table>
        </div>
    </div>
    <div id="FrmFollowers">
    </div>
</asp:Content>

<%--<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
   <%-- <div class="rightTitle">
        <%--Actions
        <label for="LblEventActions">
            <asp:Literal ID="LtEventActions" runat="server" Text="<%$ Resources:LabelResource, ForumIndexEventActions%>" />
        </label>
    </div>
    <div class="rightBodies">
        <ul>
            <li class="lineList">
                <%= Html.ActionLink("Root of Comments ", "Index", "Forum")%>
            </li>
        </ul>
    </div>
</asp:Content>--%>
