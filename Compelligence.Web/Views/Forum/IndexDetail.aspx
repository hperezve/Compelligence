<%@ Page Title="Compelligence - Coments" Language="C#" MasterPageFile="~/Views/Shared/MasterPages/FrontEndSite.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="Styles" runat="server">
  <%--<link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.7.custom.css") %>" rel="stylesheet" type="text/css" />--%>
  <link href="<%= Url.Content("~/Content/Styles/jquery-ui-1.10.3/jquery-ui-1.10.3.custom.css") %>" rel="stylesheet" type="text/css" />
  <link href="<%= Url.Content("~/Content/Styles/Discussion.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="server">

  <%--<script src="<%= Url.Content("~/Scripts/jquery-ui-1.7.custom.min.js") %>" type="text/javascript"></script>--%>
  <script src="<%= Url.Content("~/Scripts/jquery-ui-1.10.3.custom.js") %>" type="text/javascript"></script>
  <script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>
  <script type="text/javascript" src="<%= Url.Content("~/Scripts/System/FrontEnd/Upload.js") %>"></script>
  <script src="<%= Url.Content("~/Scripts/System/FrontEnd/Utils.js") %>" type="text/javascript"></script>
<script type="text/javascript">

    $(function() {
        $("#tableDiscussionDeal").treeTable({ initialState: "expanded", clickableNodeNames: true });
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
//        source.append("<input type='button' value='Cancel' class='shortButton' onclick='RemoveFormComment()' />");
        if (target.find("#FormComment").size() == 0) {
            target.append(source);
        }
        $("#FormComment:first").children('#response').focus();
    };

         
</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%Forum forum = (Forum)ViewData["Forum"]; %>
    <h2>
        <%=forum.Subject %></h2>
    <hr />
    <br />
    <% Html.RenderPartial("FrontEndDownloadSection"); %>
    <fieldset>
        <asp:Panel ID="OldComments" runat="server">
        <%Forum forum = (Forum)ViewData["Forum"]; %>
         <%--<%=Html.CommentControl("Download", "Forum", (decimal)forum.EntityId , (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>--%>
         <%=Html.CommentThreds("Download", "Forum", (decimal)forum.EntityId , (List<ForumResponse>)ViewData["Comments"], (string)ViewData["UserSecurityAccess"])%>
        </asp:Panel>
    </fieldset>
    <fieldset>
        <legend>
        	 <%--Add comment  --%>
	        <label for="LblAddcomment">
		        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:LabelResource, ForumIndexDetailsAddcomment %>" />
	        </label>
        
        </legend>
        <% using (Html.BeginForm("SendComment", "Forum", FormMethod.Post, new
           {
               id = "FormComment",
               enctype = "multipart/form-data",
               onsubmit="return ValidateFormComment(this)"
              
           }))
           { %>
        <input id="Id" type="hidden" name="Id" value="<%=forum.Id %>" />
        <input id="ParentResponseId" type="hidden" name="ParentResponseId" />
        <textarea id="response" name="Comment" cols="72" rows="10"></textarea>
        <%=Html.MultiUploadControl()%>
        <input type="submit" value="Send" class="shortButton" />        
        <input type='button' value='Cancel' class='shortButton' onclick='RemoveFormComment()' />
        
        
        <%} %>
    </fieldset>    
    <div id="CommentForm"></div>
</asp:Content>

<asp:Content ID="IndexRightContent" ContentPlaceHolderID="RightMainContent" runat="server">
        <ul>
<li>
    <div class="rightTitle">
    	<%--Actions  --%>
	<label for="LblActions">
		<asp:Literal ID="LtActions" runat="server" Text="<%$ Resources:LabelResource, ForumIndexDetailActions %>" />
	</label>
    
    </div>
    <div class="rightBodies">
    <ul>
        <li class="lineList">
        <%= Html.ActionLink("Root of Comments ", "Index", "Forum")%>
        </li>
    </ul>
    </div>
</li>
</ul>
</asp:Content>
