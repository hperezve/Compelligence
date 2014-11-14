<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<script src="<%= Url.Content("~/Scripts/jquery.treeTable.js") %>" type="text/javascript"></script>

<style type ="text/css">

pre.listing {
  background: #eee;
  border: 1px solid #ccc;
  margin: .3em auto;
  padding: .1em .3em;
  width: 90%;
}

pre.listing b {
  color: #f00;
}

/* TABLE
 *  */
table {
  border: 1px solid #888;
  border-collapse: collapse;
  line-height: 1;
  /*margin: 1em auto;*/
  width: 90%;
}

/* Caption
 * ------------------------------------------------------------------------- */
table caption {
  font-size: .9em;
  font-weight: bold;
  text-align:center;
  margin:5px;
  
}

/* Header
 * ------------------------------------------------------------------------- */
table thead {
  background: #aaa url(<%= ResolveUrl("~/images/bg-table-thead.png") %>) repeat-x top left;
  font-size: .9em;
}

table thead tr th {
  border: 1px solid #888;
  font-weight: normal;
  padding: .3em 1.67em .1em 1.67em;
  text-align: left;
  font-weight:bold;
  font-size:12px;
}

/* Body
 * ------------------------------------------------------------------------- */
table tbody tr td {
  cursor: default;
  /*padding: .3em 1.5em;*/
  border:1px solid gray;  
  /*background:#ffffff;*/
}

table tbody tr.even {
  background: #f3f3f3;
}

table tbody tr.odd {
  background: #fff;
}

table span {
  background-position: center left;
  background-repeat: no-repeat;
  padding: .2em 0 .2em 1.5em;
}

table span.discussionText {
  background-image: url(url(<%= ResolveUrl("~/content/Styles/jquery-ui/images/page_white_text.png") %>) );
  font-style:italic;
}

table span.discussionTitle {
  /*background-image: url(../content/Styles/jquery-ui/images/discussionTitle.png);*/
  background-image: url(<%= ResolveUrl("~/Content/Images/Styles/comment.png") %>);
}

/* jquery.treeTable.collapsible
 * ------------------------------------------------------------------------- */
.treeTable tr td .expander {
  background-position: left center;
  background-repeat: no-repeat;
  cursor: pointer;
  padding: 0;
  zoom: 1; /* IE7 Hack */
}

.treeTable tr.collapsed td .expander {
  background-image: url(<%= ResolveUrl("~/content/Styles/jquery-ui/images/toggle-expand-dark.png") %>);
}

.treeTable tr.expanded td .expander {
  background-image: url(<%= ResolveUrl("~/content/Styles/jquery-ui/images/toggle-collapse-dark.png") %>);
}

/* jquery.treeTable.sortable
 * ------------------------------------------------------------------------- */
.treeTable tr.selected, .treeTable tr.accept {
  background-color: #3875d7;
  color: #fff;
}

.treeTable tr.collapsed.selected td .expander, .treeTable tr.collapsed.accept td .expander {
  background-image: url(<%= ResolveUrl("~/content/Styles/jquery-ui/images/toggle-expand-light.png") %>);  
}

.treeTable tr.expanded.selected td .expander, .treeTable tr.expanded.accept td .expander {
  background-image: url(<%= ResolveUrl("~/content/Styles/jquery-ui/images/toggle-collapse-light.png") %>);
}

.treeTable .ui-draggable-dragging {
  color: #000;
  z-index: 1;
}

.contentDetail
{
	padding-left:20px;	
	padding-top:5px;
	height:316px; 
	overflow:auto;
	background-color:#dddddd;
}

.columnDiscussion
{
	width:60%;
}
.columnCreateBy
{
	width:10%;	
}
.columnCreateDate
{
	width:20%;
}


</style>

<script type="text/javascript">

    $(document).ready(function() {
        $("#tableDiscussion").treeTable();

        // Make visible that a row is clicked
        $("table#tableDiscussion tbody tr").mousedown(function() {
            $("tr.selected").removeClass("selected"); // Deselect currently selected rows
            $(this).addClass("selected");
        });

        // Make sure row is selected when span is clicked
        $("table#tableDiscussion tbody tr span").mousedown(function() {
            $($(this).parents("tr")[0]).trigger("mousedown");
        });
    });
  </script>


<div id="<%= ViewData["Scope"] %>DiscussionDetailDataListContent" class="contentDetail">

<div class="height25">
    <div class="discussionTitle">FeedBacks</div>
</div>
<div>
    <table id="tableDiscussion">
      <thead>
        <tr>
          <th>Content</th>
          <th>CreateBy</th>
          <th>CreateDate</th>
        </tr>
      </thead>
      

      <tbody>  
      <%IList<ForumResponse> forumresponses = (IList<ForumResponse>)ViewData["FeedBacks"];
         int q = 1;
         foreach (ForumResponse forumresponse in forumresponses)
         {  
            var classparent = string.Empty;
              if (forumresponse.ParentResponseId != null && forumresponse.ParentResponseId > 0)
              {
                  classparent = "child-of-n" + forumresponse.ParentResponseId.ToString();
              } %>      
          <tr id="n<%=forumresponse.Id %>" class="<%=classparent %>">
          <td class="columnDiscussion" style="background:white"><span class="discussionName"><%=forumresponse.Response%></span></td>
          <td class="columnCreateBy" style="background:white"><%=forumresponse.CreatedByName%></td>
          <td class="columnCreateDate" style="background:white"><%=forumresponse.CreatedDate%></td>
          </tr>
     <%} %>
     
      </tbody>
    </table>
</div> 
</div>

