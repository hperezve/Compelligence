<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.Common.Utility.Web" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>
<%@ Import Namespace="Compelligence.Web.Models.Helpers" %>

<style type ="text/css">
.contentBoxData /*Panel Box*/
{
	border: 1px solid #bbbbbb;
	-moz-border-radius:10px 10px 8px 8px;
	-webkit-border-radius: 10px 10px 8px 8px;
	
	margin-left:5px;
	margin-bottom:10px;
	_margin:0px 0px 10px 10px;
	height:350px;
	border-style: solid;
	border-width: 1px 1px 1px 1px;
    border-color: #A4A4A4;
    color: black; 
}
.contentBoxDataHeader
{
    color:Black;
	padding-left:6px;
	padding-top:5px;
	padding-bottom:5px;
	font-weight:bold;	
	-moz-border-radius-topright: 10px;
	-moz-border-radius-topleft: 10px;
	-webkit-border-radius: 10px 10px 0px 0px;
}
.contentBoxDataList
{
 height :300px;
 overflow-y:auto;
 overflow-x:hidden; 
 padding:10px; 
 _padding:15px 0px 0px 0px;
}
.contentPositioning
{
 height :100px;
 overflow-y:auto;
 overflow-x:hidden; 
 padding:10px; 
 _padding:15px 0px 0px 0px;
}
.Body
{
 overflow-y:auto;
 overflow-x:hidden; 
}
</style>
<script type="text/javascript">
    var loadUrl = function() {
        var url = $('#LinkEntity').text();
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "Url", "scrollbars=1,width=900,height=500")
        }
    };
    var openUrl = function(url) {
        if (url != '') {
            if (url.indexOf("http://") == -1) {
                url = "http://" + url;
            }
            window.open(url, "News", "width=900,height=500")
        }
    };

       var el = document.getElementById("imgDetail")
        if (el) {
            if (el.height > 80) {
                var newWidth = Math.round(((80 * el.width) / el.height));
                el.style.width = "" + newWidth + "px";
                el.style.height = "80px";
            }
        }
    });
</script>
<script type="text/javascript">
    function resize(which) {
        var elem = document.getElementById(which);
        var height = elem.height;
        var width = elem.width;
        var percentajeRectWidth;
        var percentajeRectHeight;
        if (elem == undefined || elem == null) return false;
        if (elem.width > 250) {
            percentajeRectWidth = (25000 / width);
            elem.style.height = ((height * percentajeRectWidth) / 100).toString() + 'px';
            height = (height * percentajeRectWidth) / 100;
            elem.style.width = 250 + 'px';
            width = 250;
        }
        if (elem.height > 84) {
            percentajeRectHeight = (8400 / height);
            elem.style.width = ((width * percentajeRectHeight) / 100).toString() + 'px';
            elem.style.height = 84 + 'px';
        }
    };
</script>
<div class="Body">
<%
    IList<LibraryCatalog> libraryCatalog = (IList<LibraryCatalog>)ViewData["LibraryCatalog"];
    for (int i = 0; i < libraryCatalog.Count; i++)
    {
        LibraryCatalog oLibraryCatalog = libraryCatalog[i];
        if (oLibraryCatalog.Displayable && (oLibraryCatalog.Projects.Count != 0 || oLibraryCatalog.Positioning.Count != 0 || oLibraryCatalog.Library.Count != 0))
        {   
            if (oLibraryCatalog.Projects.Count > 0)
            {   %>
               <div id="contentPanel<%=i%>" class="contentBoxData" style="width:<%=oLibraryCatalog.Width %>" >
               <div id="contentPanelTitle" class="contentBoxDataHeader"> <%=libraryCatalog[i].Description%></div>
               <div id="Projects"          class="contentBoxDataList">
                <% 
                foreach (var oProject in oLibraryCatalog.Projects)
                {   %>
               <div>
                    <div class="contentBoxDataSubList" style="<%=(oProject.Id != oLibraryCatalog.Projects[0].Id)?"border-top:1px solid #cccccc;":"border-top:0px; padding:0px;"%>">
                        <div>
                            <div class="contentBoxProject">
                                <a href="javascript:void(0)" onclick="return downloadFile('<%=Url.Action("Download", "ContentPortal") + "/" + oProject.Id %>');">
                                <%=oProject.Name%></a>
                    
                            </div>
                            <div class="float-right" style="width:55px; _width:55px;">            
                                <a id="testfeedbackicon" href="javascript: void(0);" onclick="FeedBackFormDlg('<%= Url.Action("SendFeedBack", "ContentPortal") + "/" + oProject.Id %>');">
                                 <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private comment" alt="FeedBack" /></a>  
                                <a id="testforumicon" href="javascript:void(0)" onclick="CommentFormDlg('<%=Url.Action("GetComments", "Forum",new {EntityId=oProject.Id,ObjectType=DomainObjectType.Project},null)%>','Comment Form','<%=Url.Action("CommentsResponse", "Forum",new {EntityId=oProject.Id,ForumResponseId=0,ObjectType=DomainObjectType.Project},null)%>')">
                                <img src="<%=Url.Content("~/Content/Images/Icons/testforum.gif") %>" width="22px" title="Add public comment" alt="Comments" /></a>
                            </div><br />
                        </div><br />
                        <div id="labels" class="float-left">
                <%foreach (var label in oProject.Labels)
                { %>
                            <div class="labelContent" style="font-size:11px; color:Gray">            
                            <%=label.Content%>...
                            </div>
              <%} %>
            </div>
        </div>
         <div style="float: left; height: 30px; margin-left:10px;width:100%">
            <% if (oProject.RatingCounter > 0)
               {%>
               <div style="float:left; width:120px; margin-right:20px;">
               Other user Ratings
               </div>
               <div>
               Your Rating
               </div>
            <%=Html.RatingStarts(oProject.Id, oProject.Rating, oProject.RatingCounter, Url.Action("Rating", "ContentPortal" ,new {ProjectId=oProject.Id}), oProject.RatingAllowed)%>
            <%}
               else
               { %>
            <div id="NoRatingImage" style="width:85px; height:25px; float:left; color:#e5332c; font-size:11px; width:200px;float:left;">
                This Document has not yet been rated
            </div>
            <%=Html.FirstRatingStarts(oProject.Id, oProject.Rating, oProject.RatingCounter, Url.Action("Rating", "ContentPortal", new { ProjectId = oProject.Id }), oProject.RatingAllowed)%>
            <%} %>
        </div>
         </div> 
       <%}%>
       </div>
       </div>
       <%}
    else if (oLibraryCatalog.Positioning.Count > 0)
    { %>
       <div id="contentPanel<%= i %>" >
       <div id="contentPanelTitle" class="contentBoxDataHeader"> <%=libraryCatalog[i].Description%></div>
       <div id="Positioning"  class="contentPositioning">
         <% 
    foreach (var oPositioning in oLibraryCatalog.Positioning)
    {
        string ClassRow = string.Empty;
        if (oPositioning.Type.Equals(PositioningCompetitiveType.CompellingReasonNotToBuy) || oPositioning.Type.Equals(PositioningCompetitorType.CompellingReasonToBuy))
        { ClassRow = "border-top:2px solid #cccccc;font-size:1.2em; font-weight:bold"; }
        else if (oPositioning.Type.Equals(PositioningCompetitiveType.CompetitiveProofPoint) || oPositioning.Type.Equals(PositioningCompetitorType.ProofPoint))
        {
            ClassRow = string.Empty; 
        }
        
        string works;
            %>
         
            <% if (oPositioning.Type.Equals(PositioningCompetitiveType.CompetitiveKeyMessage) || oPositioning.Type.Equals(PositioningCompetitorType.CompetitorKeyMessage))
               { %>
                <ul style="list-style-type:disc;margin-left:5px;margin-top: 5px; padding-bottom: 0px; margin-bottom: 5px; padding-top: 0px;"><li>
                <%}
               else if (oPositioning.Type.Equals(PositioningCompetitiveType.CompetitiveProofPoint) || oPositioning.Type.Equals(PositioningCompetitorType.ProofPoint))
               {%>
               <ul style="list-style-type:circle;margin-left:15px;margin-top: 5px; padding-bottom: 0px; margin-bottom: 5px; padding-top: 0px;font-style:italic;color:Gray"><li>
               <%} %>
                   <div class="contentAreaItemsList" style="<%=ClassRow%>" >
                   <%=oPositioning.Content%> 
               </div>
               <% if (oPositioning.Type.Equals(PositioningCompetitiveType.CompetitiveKeyMessage) || oPositioning.Type.Equals(PositioningCompetitorType.CompetitorKeyMessage))
               { %>
                </li></ul>
                <%}
               else if (oPositioning.Type.Equals(PositioningCompetitiveType.CompetitiveProofPoint) || oPositioning.Type.Equals(PositioningCompetitorType.ProofPoint))
               {%>
               </li></ul>
               <%} %>
        <%-- <br />--%>
       <%}%>
       </div>
       </div>
       <%}
    else if (oLibraryCatalog.Library.Count > 0)
    { %>
       <div id="Div1" class="contentBoxData">
       <div id="contentPanelTitle" class="contentBoxDataHeader"> <%=libraryCatalog[i].Description%></div>
       <div id="Positioning"  class="contentPositioning">
       
         <%
    foreach (var oLibrary in oLibraryCatalog.Library)
    {
                 
            %><div class="contentAreaItems" style="border-bottom: 1px solid rgb(204, 204, 204);" >
            <a href="javascript:void(0)"  onclick="javascript:MessageDlgUrl('News','<%=Url.Action("GetLibrary","ContentPortal",new {libraryid=oLibrary.Id}) %>','<%=oLibrary.Link%>')"><%=oLibrary.Name%></a>
             <br />
            </div>
            <%} %>
        
       </div>
       </div>
        <%}%>
   <%}
     else {
         if (oLibraryCatalog.Displayable && oLibraryCatalog.Description.Equals("Details Industry Competitor Product"))
         {
     %>
     <% if (ViewData["DetailsEntities"] == null && ViewData["EntityDetail"] != null)
        { 
     %>  
     <div id="contentPanelDetail" class="contentBoxData">
     <div id="contentPanelDetailTitle" class="contentBoxDataHeader"> 
     
     	<%--Details of--%>
	<label for="LblDetailsof">
		<asp:Literal ID="LtDetailsof" runat="server" Text="<%$ Resources:LabelResource, PortalContentDetailsof%>" />
	</label>
     <%= ViewData["EntityDetail"]%>: <%= ViewData["NameDetail"]%></div>
     <div id="DetailsList" class="contentBoxDataList">
     <table width="100%">
     <tr>
     <td>
     <table width="100%">
     <tr>
     <td style="width:50%;">
      <div class="contentBoxProject" style="color:#666666;">
      <%--Picture--%>
	<label for="LblLeftContentPicture">
		<asp:Literal ID="LtLeftContentPicture" runat="server" Text="<%$ Resources:LabelResource, ContentPortalLeftContentPicture%>" />
	</label>
      
     </div><br />
      <div>
         <% if (ViewData["ImageDetail"] != null && !ViewData["ImageDetail"].Equals(""))
            {%>
            
            <img id="imgDetail" src="<%= ViewData["ImageDetail"]%>" onload="resize('imgDetail')" data-tooltip="sticky1">
         
         <%}
            else
            { %>
            
            	<%--There's no image available.--%>
	<label for="LblLeftContentMessage1">
		<asp:Literal ID="LtLeftContentMessage1" runat="server" Text="<%$ Resources:LabelResource, ContentPortalLeftContentMessage1%>" />
	</label>
         <%} %>
     </div>
    </td>
    <td style="width:50%;" valign="top">
     <div class="contentBoxProject" style="color:#666666;">
     
     	<%--URL--%>
	<label for="LblLeftContentURL">
		<asp:Literal ID="LtLeftContentURL" runat="server" Text="<%$ Resources:LabelResource, ContentPortalLeftContentURL%>" />
	</label>
     </div><br />

          <% if (ViewData["UrlDetail"] != null && !ViewData["UrlDetail"].Equals(""))
             {%>
            <div>
            <a href="<%=ViewData["UrlDetailText"]%>" id="LinkEntity" onclick="javascript:loadUrl();"> <%=ViewData["UrlDetailText"]%> </a>
            </div>
         <%}
             else
             { %>
             <%--There's no url available.--%>
	<label for="LblLeftContentMessage2">
		<asp:Literal ID="LtLeftContentMessage2" runat="server" Text="<%$ Resources:LabelResource, ContentPortalLeftContentMessage2%>" />
	</label>
            
         <%} %>

         </td>
         </tr>
        </table>
      </td>
     </tr>
     <tr>
     <td>
     <div class="contentBoxProject" style="color:#666666;">
             <%--Description--%>
	<label for="LblLeftContentDescripcion">
		<asp:Literal ID="LtLeftContentDescripcion" runat="server" Text="<%$ Resources:LabelResource, ContentPortalLeftContentDescripcion%>" />
	</label>
     
     </div><br />
     <div>
         <% if (ViewData["DescriptionDetail"] != null && !ViewData["DescriptionDetail"].Equals(""))
            {%>
         <%= ViewData["DescriptionDetail"]%>
         <%}
            else
            { %>
             <%--There's no description available.--%>
	<label for="LblLeftContentMessage3">
		<asp:Literal ID="LtLeftContentMessage3" runat="server" Text="<%$ Resources:LabelResource, ContentPortalLeftContentMessage3%>" />
	</label>
            
         <%} %>
     </div>
     </td>
     </tr>
     </table>
     </div>
     </div>
       <%} %>
     <%}
         else if (oLibraryCatalog.Weaknesses.Count > 0 || oLibraryCatalog.Strenths.Count>0)
         {%>

       <div id="Div2" class="contentBoxData" style="width:<%=oLibraryCatalog.Width %>" >
       <div id="contentPanelTitle" class="contentBoxDataHeader"> <%=libraryCatalog[i].Description%></div>
       <div id="Projects"          class="contentBoxDataList">
            <% foreach (string competitorS in oLibraryCatalog.Strenths)
               {
                   string result = competitorS.Replace("\n","<br />");
               
                    %>
                <div class="labelContent" style="font-size:11px; color:Gray">            
                            <%= result %>
                </div>
                <br />
            <%} %>
            <% foreach (string competitorW in oLibraryCatalog.Weaknesses)
               {
                   string result = competitorW.Replace("\n", "<br />");
                   %>
                <div class="labelContent" style="font-size:11px; color:Gray">            
                            <%= result %>
                </div>
                <br />
            <%} %>
       </div>
       </div>

         
         <%}
    }%>
<%}%>
</div>

