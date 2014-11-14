<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Compelligence.Domain.Entity" %>
<%@ Import Namespace="Compelligence.Domain.Entity.Resource" %>
<%@ Import Namespace="Compelligence.DataTransfer.Comparinator" %>
<%@ Import Namespace="Compelligence.DataTransfer.FrontEnd" %>
<%IList<Product> Titles = (IList<Product>)ViewData["Products"]; %>

<script src="<%= Url.Content("~/Scripts/jquery.autocomplete.min.js") %>" type="text/javascript"></script>
<script type="text/javascript">
    var loadContent = function(urlAction, target) {
        var MyCompanyId = '<%= ViewData["C"] %>';
        var MyUserId = '<%= ViewData["U"] %>';
        var parameters = { C: MyCompanyId, U: MyUserId };
        showLoadingDialog();
        $('#' + target).load(urlAction, parameters, function() { hideLoadingDialog(); });

    };
</script>

<div style="width: 100%">
<% 
    string c=(string)ViewData["C"];
    string u=(string)ViewData["U"];
    bool DefaultsDisabPublComm = Convert.ToBoolean(ViewData["DefaultsDisabPublComm"].ToString());
    IList<LibraryCatalog> libraryCatalog = (IList<LibraryCatalog>)ViewData["LibraryCatalog"];
    for (int i = 0; i < libraryCatalog.Count; i++)
    {
        LibraryCatalog oLibraryCatalog = libraryCatalog[i];
        if ((oLibraryCatalog.Projects.Count != 0 || oLibraryCatalog.Positioning.Count != 0 || oLibraryCatalog.Library.Count != 0))
        {
%>
    <div class=" <%=oLibraryCatalog.CssClass %>">
<%
            if (oLibraryCatalog.Projects.Count > 0)
            { 
%>
        <div style="padding-right:5px; ">
            <div id="contentPanel<%= i %>" class="contentBoxData">
                <div id="Div1" class="contentBoxDataHeader"> <%=libraryCatalog[i].Description%></div>
                <div id="Projects" class="contentBoxDataList">
                <%  foreach (var oProject in oLibraryCatalog.Projects)
                    {   
                %>
                    <div>
                    <div class="contentBoxDataSubList" style="<%=(oProject.Id != oLibraryCatalog.Projects[0].Id)?"border-top:1px solid #cccccc;":"border-top:0px; padding:0px;"%>">
                        <div>
                            <div class="contentBoxProject">                       
                                <%if (string.IsNullOrEmpty(c) && string.IsNullOrEmpty(u))
                                   { %>
                                <a id="ap<%=oProject.Id %>" href="javascript:void(0)" onclick="return downloadFile('<%=Url.Action("Download", "ContentPortal") + "/" + oProject.Id %>');">
                                    <%=oProject.Name%>
                                </a> 
                                <% }
                                   else
                                   { %>                 
                                <a id="a3" href="javascript:void(0)" onclick="return downloadFile('<%=Url.Action("DownloadSalesForce", "Comparinator", new {ProjectId=oProject.Id, U=(string)ViewData["U"]}) %>');">
                                    <%=oProject.Name%>
                                </a> 
                                <% } %>                 
                            </div>
                            <div class="float-right" style="width:55px; _width:55px;">  
                             <% if (!DefaultsDisabPublComm)
                               { %>
                            <%  string IdComments = "ImgComents" + oProject.Id;  %>
                                <a id="A1" href="javascript:void(0)" onclick="ExternalCommentsDlg('<%=Url.Action("GetComments", "Forum",new {EntityId=oProject.Id,ObjectType=DomainObjectType.Project,U=(string)ViewData["U"],C=(string)ViewData["C"]},null)%>','<%=Url.Action("ExternalResponse", "Forum",new {EntityId=oProject.Id,ForumResponseId=0,ObjectType=DomainObjectType.Project, C=c, U=u},null)%>','<%=oProject.Id%>')">
                                
                                <%  if (oProject.Comments == false)%>
                                <%  { %>
                                    <img id ="<%=IdComments%>" class="ImageCommentsN" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                                <%  } else {    %> 
                                    <img id ="<%=IdComments%>" class="ImageCommentsY" src="<%=Url.Content("~/Content/Images/Icons/CommentWhite.png") %>" width="22px" title="Add public comment" />
                                <%  }   %>
                                </a>
                                <%} %>
                                <%if (string.IsNullOrEmpty(c) && string.IsNullOrEmpty(u))
                                  { %>
                                <a id="A4" href="javascript: void(0);" style="float: right;padding-right:5px;" onclick="ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum", new {EntityId=oProject.Id, EntityType=DomainObjectType.Project,SubmittedVia =  FeedBackSubmitedVia.SalesTool})%>','FeedBack Dialog');">
                                    <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                                </a> 
                                <%}
                                  else
                                  { %>
                                    <a id="A2" href="javascript: void(0);" style="float: right;padding-right:5px;" onclick="ExternalFeedBackWithAttachedDlg('<%= Url.Action("FeedBackMessage", "Forum", new {EntityId=oProject.Id, EntityType=DomainObjectType.Project, U=u, SubmittedVia = FeedBackSubmitedVia.SalesTool}) %>','FeedBack Dialog');">
                                    <img src="<%=Url.Content("~/Content/Images/Icons/testfeedback.gif") %>" width="22px" title="Add private feedback" />
                                </a> 
                                <% } %>
                            </div><br/>
                           <%if (oProject.File != null && oProject.File.DateIn != null)
                             { %>
                        <div id="labelDate" class="float-left">
                            <div class="labelContent AddTargetBlankToLin" style="font-size:11px; color:#A9A9A9">            
                               Last Updated: <%=((DateTime)oProject.File.DateIn).ToString("MM/dd/yyyy")%>
                               </div>
                               </div>
                               <br/>
                               <%} %>
                        </div><br/>
                       <div id="labels" class="float-left">
                            <div class="labelContent AddTargetBlankToLin" style="font-size:11px; color:Gray">            
                                <%=oProject.TextToDisplay%>...
                            </div>
                        </div>
                    </div>
                        <div style="float: left; height: 30px; margin-left:10px;width:100%;font-size: 0.85em;">
                                <%  if (oProject.RatingCounter > 0)
                                    {   %>
                                    <div style="float:left; width:120px; margin-right:20px;">
                                       <label for="LblMessage5">
		                                    <asp:Literal ID="LtMessage5" runat="server" Text="<%$ Resources:LabelResource, ContentPortalMessage5%>" />
	                                    </label>
                                    </div>
                                    <div>
                                        Your Rating
                                    </div>
                                     <%if (string.IsNullOrEmpty(u)){%>
                                    <%=Html.RatingStarts(oProject.Id, oProject.Rating, oProject.RatingCounter, Url.Action("Rating", "ContentPortal" ,new {ProjectId=oProject.Id}), oProject.RatingAllowed)%>
                                    <%} else { %>
                                    <%=Html.RatingStarts(oProject.Id, oProject.Rating, oProject.RatingCounter, Url.Action("Rating", "SalesForce", new {ProjectId=oProject.Id, U = (string)ViewData["U"] }), oProject.RatingAllowed)%>
                                     <% } %>
                                <%  }
                                    else
                                    {   %>
                                    <div id="NoRatingImage" style="width:85px; height:25px; float:left; color:#e5332c; font-size:11px; width:200px;float:left;">
                                        This Document has not yet been rated
                                    </div>
                                    <%if (string.IsNullOrEmpty(u))
                                  {%>
                                    <%=Html.FirstRatingStarts(oProject.Id, oProject.Rating, oProject.RatingCounter, Url.Action("Rating", "ContentPortal", new { ProjectId = oProject.Id }), oProject.RatingAllowed)%>
                                    <%} else { %>
                                    <%=Html.FirstRatingStarts(oProject.Id, oProject.Rating, oProject.RatingCounter, Url.Action("Rating", "SalesForce", new { ProjectId = oProject.Id, U = (string)ViewData["U"] }), oProject.RatingAllowed)%>
                                    <% } %>
                                <%  }   %>
                                </div>
                </div> 
                    <%  }   %>
                </div>
            </div>
        </div>
<%          }   %>
    </div>
<%
        }
    }
%> 
</div>
<div style="display: none;">
    <iframe id="DownloadFileSection" src="javascript: void(0);" frameborder="0" marginheight="0"
        marginwidth="0"></iframe>
</div>



